'Copyright 2014 AQUA TERRA Consultants - Royalty-free use permitted under open source license
Option Strict Off
Option Explicit On

Imports atcUtility
Imports atcMwGisUtility
Imports atcData
Imports atcSegmentation
Imports MapWinUtility
Imports System.Text
Imports System.Collections.ObjectModel

Public Class HspfSetup

    Public GridPervious As atcControls.atcGrid
    Public MetBaseDsns As atcCollection
    Public MetWdmIds As atcCollection
    Public UniqueModelSegmentNames As atcCollection
    Public UniqueModelSegmentIds As atcCollection
    Public OutputPath As String
    Public BaseOutputName As String
    Public SubbasinLayerName As String
    Public SubbasinFieldName As String
    Public SubbasinSlopeName As String
    Public StreamLayerName As String
    Public StreamFields() As String
    Public LUType As Integer  '0 - USGS GIRAS Shape, 1 - NLCD grid, 2 - Other shape, 3 - Other grid
    Public LandUseThemeName As String
    Public LUInclude() As Integer
    Public OutletsLayerName As String
    Public PointFieldName As String
    Public PointYear As String
    Public LandUseFieldName As String
    Public LandUseClassFile As String
    Public SubbasinSegmentName As String
    Public PSRCustom As Boolean
    Public PSRCustomFile As String
    Public PSRCalculate As Boolean
    Public SnowOption As Integer = 0
    Public ElevationFileName As String = ""
    Public ElevationUnits As String = ""
    Public DoWetlands As Boolean = False
    Public ToWetlandsFileName As String = ""
    Public MetricUnits As Boolean = False

    Public Function SetupHSPF() As Boolean
        Logger.Status("Preparing to process")
        Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor
        Dim lBasinsFolder As String = My.Computer.Registry.GetValue("HKEY_LOCAL_MACHINE\SOFTWARE\AQUA TERRA Consultants\BASINS", "Base Directory", "C:\Basins")

        'build collection of selected subbasins 
        Dim lSubbasinsSelected As New atcCollection  'key is index, value is subbasin id
        Dim lSubbasinsSlopes As New atcCollection    'key is subbasin id, value is slope
        Dim lSubbasinId As Integer
        Dim lSubbasinSlope As Double
        Dim lSubbasinLayerIndex As Long = GisUtil.LayerIndex(SubbasinLayerName)
        Dim lSubbasinFieldIndex As Long = GisUtil.FieldIndex(lSubbasinLayerIndex, SubbasinFieldName)
        Dim lSubbasinSlopeIndex As Long = GisUtil.FieldIndex(lSubbasinLayerIndex, SubbasinSlopeName)
        For i As Integer = 1 To GisUtil.NumSelectedFeatures(lSubbasinLayerIndex)
            Dim lSelectedIndex As Integer = GisUtil.IndexOfNthSelectedFeatureInLayer(i - 1, lSubbasinLayerIndex)
            lSubbasinId = GisUtil.FieldValue(lSubbasinLayerIndex, lSelectedIndex, lSubbasinFieldIndex)
            lSubbasinSlope = GisUtil.FieldValue(lSubbasinLayerIndex, lSelectedIndex, lSubbasinSlopeIndex)
            lSubbasinsSelected.Add(lSelectedIndex, lSubbasinId)
            'TODO: be sure SubbasinIds are unique before this!
            lSubbasinsSlopes.Add(lSubbasinId, lSubbasinSlope)
        Next
        If lSubbasinsSelected.Count = 0 Then 'no subbasins selected, act as if all are selected
            For i As Integer = 1 To GisUtil.NumFeatures(lSubbasinLayerIndex)
                lSubbasinId = GisUtil.FieldValue(lSubbasinLayerIndex, i - 1, lSubbasinFieldIndex)
                lSubbasinSlope = GisUtil.FieldValue(lSubbasinLayerIndex, i - 1, lSubbasinSlopeIndex)
                lSubbasinsSelected.Add(i - 1, lSubbasinId)
                lSubbasinsSlopes.Add(lSubbasinId, lSubbasinSlope)
            Next
        End If

        'build collection of model segment ids for each subbasin
        Dim lSubbasinsModelSegmentIds As New atcCollection    'key is subbasin id, value is model segment id
        Dim lSubbasinSegmentFieldIndex As Integer = -1
        If SubbasinSegmentName <> "<none>" And SubbasinSegmentName <> "" Then 'see if we have some model segments in the subbasin dbf
            lSubbasinSegmentFieldIndex = GisUtil.FieldIndex(lSubbasinLayerIndex, SubbasinSegmentName)
        End If
        For Each lSubbasinIndex As Integer In lSubbasinsSelected.Keys
            lSubbasinId = lSubbasinsSelected.ItemByKey(lSubbasinIndex)
            If lSubbasinSegmentFieldIndex > -1 And UniqueModelSegmentIds.Count > 0 Then
                Dim lModelSegment As String = GisUtil.FieldValue(lSubbasinLayerIndex, lSubbasinIndex, lSubbasinSegmentFieldIndex)
                lSubbasinsModelSegmentIds.Add(lSubbasinId, UniqueModelSegmentIds(UniqueModelSegmentNames.IndexFromKey(lModelSegment)))
            Else
                lSubbasinsModelSegmentIds.Add(lSubbasinId, 1)
            End If
        Next

        'each land use code, subbasin id, and area is a single land use record
        Dim lLandUseSubbasinOverlayRecords As New Collection '(Of LandUseSubbasinOverlayRecord)
        Dim lReclassifyFileName As String = ""

        If LUType = 0 Then
            'usgs giras is the selected land use type
            Logger.Status("Performing overlay for GIRAS landuse")
            Dim lSuccess As Boolean = CreateLanduseRecordsGIRAS(lSubbasinsSelected, lLandUseSubbasinOverlayRecords, SubbasinLayerName, SubbasinFieldName, _
                                                                SnowOption, ElevationFileName, ElevationUnits, DoWetlands, ToWetlandsFileName)

            If lLandUseSubbasinOverlayRecords.Count = 0 Or Not lSuccess Then
                'problem occurred, get out
                Return False
                Exit Function
            End If
            'set reclassify file name for giras
            Dim lLandUsePathName As String = PathNameOnly(GisUtil.LayerFileName(GisUtil.LayerIndex("Land Use Index"))) & "\landuse"
            Dim lBasinsBinLoc As String = PathNameOnly(System.Reflection.Assembly.GetEntryAssembly.Location)
            lReclassifyFileName = lBasinsBinLoc.Substring(0, lBasinsBinLoc.Length - 3) & "etc\"
            If IO.Directory.Exists(lReclassifyFileName) Then
                If DoWetlands Then
                    lReclassifyFileName &= "girasWetlands.dbf"
                Else
                    lReclassifyFileName &= "giras.dbf"
                End If
            Else
                If DoWetlands Then
                    lReclassifyFileName = lBasinsFolder & "\etc\girasWetlands.dbf"
                Else
                    lReclassifyFileName = lBasinsFolder & "\etc\giras.dbf"
                End If
            End If

        ElseIf LUType = 1 Or LUType = 3 Then
            'nlcd grid or other grid is the selected land use type
            Logger.Status("Overlaying Land Use and Subbasins")
            CreateLanduseRecordsGrid(lSubbasinsSelected, lLandUseSubbasinOverlayRecords, SubbasinLayerName, LandUseThemeName, _
                                     SnowOption, ElevationFileName, ElevationUnits, DoWetlands, ToWetlandsFileName)

            If LUType = 1 Then 'nlcd grid
                If IO.File.Exists(LandUseClassFile) Then
                    lReclassifyFileName = LandUseClassFile
                Else
                    'try to default if not set
                    Dim lBasinsBinLoc As String = PathNameOnly(System.Reflection.Assembly.GetEntryAssembly.Location)
                    lReclassifyFileName = lBasinsBinLoc.Substring(0, lBasinsBinLoc.Length - 3) & "etc\"
                    If IO.Directory.Exists(lReclassifyFileName) Then
                        If DoWetlands Then
                            lReclassifyFileName &= "nlcdWetlands.dbf"
                        Else
                            lReclassifyFileName &= "nlcd.dbf"
                        End If
                    Else
                        If DoWetlands Then
                            lReclassifyFileName = lBasinsFolder & "\etc\nlcdWetlands.dbf"
                        Else
                            lReclassifyFileName = lBasinsFolder & "\etc\nlcd.dbf"
                        End If
                    End If
                End If
            Else
                If LandUseClassFile <> "<none>" Then
                    lReclassifyFileName = LandUseClassFile
                End If
            End If

        ElseIf LUType = 2 Then
            'other shape
            Logger.Status("Overlaying Land Use and Subbasins")
            CreateLanduseRecordsShapefile(lSubbasinsSelected, lLandUseSubbasinOverlayRecords, SubbasinLayerName, SubbasinFieldName, LandUseThemeName, LandUseFieldName, _
                                          SnowOption, ElevationFileName, ElevationUnits, DoWetlands, ToWetlandsFileName)

            lReclassifyFileName = ""
            If LandUseClassFile <> "<none>" Then
                lReclassifyFileName = LandUseClassFile
            End If

        End If

        'convert units if the GIS layer units are feet instead of meters
        Dim lDistFactor As Double = 1.0
        If GisUtil.MapUnits.ToUpper = "FEET" Then
            lDistFactor = 3.2808
            For Each lRec As LandUseSubbasinOverlayRecord In lLandUseSubbasinOverlayRecords
                lRec.Area = lRec.Area / (lDistFactor * lDistFactor)
            Next
        End If

        'special code to always include certain land uses
        Dim lSub As Integer
        Dim lFoundLU As Boolean = False
        Dim lInd As Integer
        For Each lLU As Integer In LUInclude
            Dim lTempRec1 As New LandUseSubbasinOverlayRecord
            Dim lTempRecInd As New LandUseSubbasinOverlayRecord
            lTempRec1 = lLandUseSubbasinOverlayRecords(1)
            lSub = lTempRec1.SubbasinId
            lInd = 1
            While lInd <= lLandUseSubbasinOverlayRecords.Count
                lTempRecInd = lLandUseSubbasinOverlayRecords(lInd)
                If lTempRecInd.LuCode = lLU Then lFoundLU = True
                If lTempRecInd.SubbasinId <> lSub OrElse lInd = lLandUseSubbasinOverlayRecords.Count Then
                    'new subbasin, if LU not found, need to add it
                    If Not lFoundLU Then
                        Dim lRec As New LandUseSubbasinOverlayRecord
                        lRec.LuCode = lLU
                        lRec.SubbasinId = lSub
                        lRec.Area = 0.001
                        lRec.MeanLatitude = lTempRec1.MeanLatitude
                        lRec.MeanElevation = lTempRec1.MeanElevation
                        lRec.PercentToWetlands = 0.0
                        lLandUseSubbasinOverlayRecords.Add(lRec, , lInd)
                        lInd += 1
                        lTempRecInd = lLandUseSubbasinOverlayRecords(lInd)
                        lSub = lTempRecInd.SubbasinId
                    End If
                    lFoundLU = False
                End If
                lInd += 1
            End While
        Next

        Logger.Status("Completed overlay of subbasins and land use layers")

        'Create Reach Segments
        Dim lReaches As Reaches = CreateReachSegments(lSubbasinsSelected, lSubbasinsModelSegmentIds, StreamLayerName, StreamFields, MetricUnits)

        'Create Stream Channels
        Dim lChannels As Channels = CreateStreamChannels(lReaches, MetricUnits)

        'Create LandUses
        Dim lLandUses As LandUses = CreateLanduses(lSubbasinsSlopes, lLandUseSubbasinOverlayRecords, lReaches)

        'figure out which outlets are in which subbasins
        Dim lOutSubs As New Collection
        If OutletsLayerName <> "<none>" Then
            Logger.Status("Joining point sources to subbasins")
            Dim i As Integer = GisUtil.LayerIndex(OutletsLayerName)
            For j As Integer = 1 To GisUtil.NumFeatures(i)
                Dim k As Integer = GisUtil.PointInPolygon(i, j - 1, lSubbasinLayerIndex)
                If k > -1 Then
                    lOutSubs.Add(GisUtil.FieldValue(lSubbasinLayerIndex, k, lSubbasinFieldIndex))
                Else
                    lOutSubs.Add(-1)
                End If
            Next j
        End If

        'make output folder
        MkDirPath(OutputPath)
        Dim lBaseFileName As String = OutputPath & "\" & BaseOutputName

        'write wsd file
        Logger.Status("Writing WSD file")
        Dim lReclassifyLanduses As LandUses = ReclassifyLandUses(lReclassifyFileName, GridPervious, lLandUses)
        WriteWSDFile(lBaseFileName & ".wsd", lReclassifyLanduses, SnowOption, DoWetlands)

        'write rch file 
        Logger.Status("Writing RCH file")
        WriteRCHFile(lBaseFileName & ".rch", lReaches)

        'write ptf file
        Logger.Status("Writing PTF file")
        WritePTFFile(lBaseFileName & ".ptf", lChannels)

        'write psr file
        Logger.Status("Writing PSR file")
        Dim lOutletsLayerIndex As Integer
        Dim lPointLayerIndex As Integer
        If lOutSubs.Count > 0 Then
            lOutletsLayerIndex = GisUtil.LayerIndex(OutletsLayerName)
            lPointLayerIndex = GisUtil.FieldIndex(lOutletsLayerIndex, PointFieldName)
        End If
        WritePSRFile(lBaseFileName & ".psr", lSubbasinsSelected, lOutSubs, lOutletsLayerIndex, lPointLayerIndex, _
                     PSRCustom, PSRCustomFile, PSRCalculate, PointYear)

        'write seg file
        Logger.Status("Writing SEG file")
        WriteSEGFile(lBaseFileName & ".seg", UniqueModelSegmentIds, MetBaseDsns, MetWdmIds)

        'write map file
        Logger.Status("Writing MAP file")
        WriteMAPFile(lBaseFileName & ".map")

        Logger.Status("")
        Return True
    End Function
End Class