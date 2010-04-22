Imports atcUtility
Imports atcData
Imports atcUCI
Imports atcSegmentation
Imports atcModelSetup
Imports MapWindow.Interfaces
Imports MapWinUtility
Imports MapWinGeoProc
Imports atcMwGisUtility
Imports System.Text
Imports System.Collections.Specialized
Imports System.Collections.ObjectModel
Imports System.Drawing

Module UCIBuilder
    Private pOutputPath As String = "C:\BASINS\modelout\"
    Private pInputPath As String = "C:\Projects\TT_GCRP\ProjectsBasins\070200\"
    Private pBaseOutputName As String = "UpperMS"
    Private pSubbasinThemeName As String = "UpperMS_subbasin_HSPF_V1a"
    'Private pSubbasinThemeName As String = "Test_catchments"
    Private pSubbasinFieldName As String = "SUBBASIN"
    Private pSubbasinSlopeName As String = "SLO1"
    Private pSubbasinSegmentName As String = "ModelSeg"
    Private pLandUseClassFile As String = "C:\Projects\TT_GCRP\Model Setup\GCRP_MinnRiver.dbf"
    Private pLUType As Integer = 3
    'Private pLandUseThemeName As String = "nlcd_landcover_2001"
    Private pLandUseThemeName As String = "hru_mnriv"
    Private pFieldNameLandUse As String = ""
    Private pMetWDM As String = pInputPath & "met\MNRV_MET.wdm"
    Private pPointThemeName As String = ""
    Private pPointYear As String = ""
    Private pStreamLayerName As String = "streams"
    Private pStreamFields() As String = {"SUBBASIN", "SUBBASINR", "LEN2", "SLO2", "WID2", "DEP2", "MINEL", "MAXEL", "SNAME"}


    Friend AtcGridMet As atcControls.atcGrid
    Friend AtcGridPervious As atcControls.atcGrid
    Friend pMetBaseDsns As atcCollection
    Friend pUniqueModelSegmentIds As atcCollection
    Friend pUniqueModelSegmentNames As atcCollection
    Friend pMetStations As atcCollection


    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        Logger.Dbg("Start")
        ChDriveDir(pOutputPath)  'change to the directory of the current project
        Logger.Dbg(" CurDir:" & CurDir())

        Dim lBasinsBinLoc As String = PathNameOnly(System.Reflection.Assembly.GetEntryAssembly.Location)
        Dim lOutputPath As String = lBasinsBinLoc.Substring(0, lBasinsBinLoc.Length - 3) & "modelout\"
        If FileExists(lOutputPath) Then
            lOutputPath &= pBaseOutputName 'tbxName.Text
        Else
            Dim lDriveLetter As String = CurDir().Substring(0, 1)
            lOutputPath = lDriveLetter & ":\BASINS\modelout\" & pBaseOutputName 'tbxName.Text
        End If

        AtcGridPervious = New atcControls.atcGrid
        AtcGridPervious.Source = New atcControls.atcGridSource
        SetPerviousGrid(AtcGridPervious, pLandUseClassFile, pLUType, pLandUseThemeName, pFieldNameLandUse)
        pMetStations = New atcCollection
        pMetBaseDsns = New atcCollection
        BuildListofMetStationNames(pMetWDM, pMetStations, pMetBaseDsns)
        AtcGridMet = New atcControls.atcGrid
        AtcGridMet.Source = New atcControls.atcGridSource
        SetMetSegmentGrid(AtcGridMet, pMetStations, pUniqueModelSegmentNames, pUniqueModelSegmentIds, _
                          pSubbasinThemeName, pSubbasinFieldName, pSubbasinSegmentName)

        If PreProcessChecking(pOutputPath, pBaseOutputName, "HSPF", pLUType, pMetStations.Count, _
                              pSubbasinThemeName, pLandUseThemeName) Then 'early checks OK
            If SetupHSPFGen(lOutputPath, pBaseOutputName, pSubbasinThemeName, pSubbasinFieldName, pSubbasinSlopeName, _
                                    pStreamLayerName, pStreamFields, pLUType, pLandUseThemeName, , , , , pSubbasinSegmentName) Then
                If CreateUCI(lOutputPath & "\" & pBaseOutputName & ".uci", pMetWDM) Then
                    Logger.Dbg("UCIBuilder:  Created UCI file " & lOutputPath & "\" & pBaseOutputName & ".uci")
                    'Else 'old way of creating a uci, in WinHSPF
                    '    StartWinHSPF(lOutputPath & "\" & lBaseOutputName & ".wsd")
                End If
            End If
        End If
    End Sub

    'aLUType - Land use layer type (0 - USGS GIRAS Shape, 1 - NLCD grid, 2 - Other shape, 3 - Other grid)
    Public Function SetupHSPFGen(ByVal aOutputPath As String, ByVal aBaseOutputName As String, _
                                 ByVal aSubbasinThemeName As String, ByVal aSubbasinFieldName As String, ByVal aSubbasinSlopeName As String, _
                                 ByVal aStreamLayerName As String, ByVal aStreamFields() As String, _
                                 ByVal aLUType As Integer, ByVal aLandUseThemeName As String, _
                                 Optional ByVal aOutletsThemeName As String = "<none>", _
                                 Optional ByVal aPointThemeName As String = "", _
                                 Optional ByVal aPointYear As String = "", _
                                 Optional ByVal aLandUseClassFile As String = "<none>", _
                                 Optional ByVal aSubbasinSegmentName As String = "", _
                                 Optional ByVal aPSRCustom As Boolean = False, _
                                 Optional ByVal aPSRCustomFile As String = "", _
                                 Optional ByVal aPSRCalculate As Boolean = False) As Boolean

        'todo: replace below with logger/status message on main BASINS form?
        'lblStatus.Text = "Preparing to process"
        'Me.Refresh()
        'EnableControls(False)

        'If Not PreProcessChecking(aOutputPath, aBaseOutputName) Then 'failed early checks
        '    Exit Function
        'End If
        Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        'build collection of selected subbasins 
        Dim lSubbasinsSelected As New atcCollection  'key is index, value is subbasin id
        Dim lSubbasinsSlopes As New atcCollection    'key is subbasin id, value is slope
        Dim lSubbasinId As Integer
        Dim lSubbasinSlope As Double
        'Dim lSubbasinThemeName As String = cboSubbasins.Items(cboSubbasins.SelectedIndex)
        Dim lSubbasinLayerIndex As Long = GisUtil.LayerIndex(aSubbasinThemeName)
        'Dim lSubbasinFieldName As String = cboSub1.Items(cboSub1.SelectedIndex)
        Dim lSubbasinFieldIndex As Long = GisUtil.FieldIndex(lSubbasinLayerIndex, aSubbasinFieldName)
        'Dim lSubbasinSlopeIndex As Long = GisUtil.FieldIndex(lSubbasinLayerIndex, cboSub2.Items(cboSub2.SelectedIndex))
        Dim lSubbasinSlopeIndex As Long = GisUtil.FieldIndex(lSubbasinLayerIndex, aSubbasinSlopeName)
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
        'If cboSub3.SelectedIndex > 0 Then 'see if we have some model segments in the subbasin dbf
        If aSubbasinSegmentName.Length > 0 Then 'see if we have some model segments in the subbasin dbf
            lSubbasinSegmentFieldIndex = GisUtil.FieldIndex(lSubbasinLayerIndex, aSubbasinSegmentName)
        End If
        For Each lSubbasinIndex As Integer In lSubbasinsSelected.Keys
            lSubbasinId = lSubbasinsSelected.ItemByKey(lSubbasinIndex)
            If lSubbasinSegmentFieldIndex > -1 And pUniqueModelSegmentIds.Count > 0 Then
                Dim lModelSegment As String = GisUtil.FieldValue(lSubbasinLayerIndex, lSubbasinIndex, lSubbasinSegmentFieldIndex)
                lSubbasinsModelSegmentIds.Add(lSubbasinId, pUniqueModelSegmentIds(pUniqueModelSegmentNames.IndexFromKey(lModelSegment)))
            Else
                lSubbasinsModelSegmentIds.Add(lSubbasinId, 1)
            End If
        Next

        'todo: make into a new class 
        'each land use code, subbasin id, and area is a single land use record
        Dim lLucodes As New Collection
        Dim lSubids As New Collection
        Dim lAreas As New Collection
        Dim lReclassifyFileName As String = ""

        If aLUType = 0 Then ' cboLanduse.SelectedIndex = 0 Then
            'usgs giras is the selected land use type
            CreateLanduseRecordsGIRAS(lSubbasinsSelected, lLucodes, lSubids, lAreas, aSubbasinThemeName, aSubbasinFieldName)

            If lLucodes.Count = 0 Then
                'TODO: report problem?
                Exit Function
            End If
            'set reclassify file name for giras
            Dim lLandUsePathName As String = PathNameOnly(GisUtil.LayerFileName(GisUtil.LayerIndex("Land Use Index"))) & "\landuse"
            Dim lBasinsBinLoc As String = PathNameOnly(System.Reflection.Assembly.GetEntryAssembly.Location)
            lReclassifyFileName = lBasinsBinLoc.Substring(0, lBasinsBinLoc.Length - 3) & "etc\"
            If FileExists(lReclassifyFileName) Then
                lReclassifyFileName &= "giras.dbf"
            Else
                lReclassifyFileName = lLandUsePathName.Substring(0, 1) & ":\basins\etc\giras.dbf"
            End If

        ElseIf aLUType = 1 Or aLUType = 3 Then 'cboLanduse.SelectedIndex = 1 Or cboLanduse.SelectedIndex = 3 Then
            'nlcd grid or other grid is the selected land use type
            CreateLanduseRecordsGrid(lSubbasinsSelected, lLucodes, lSubids, lAreas, aSubbasinThemeName, aLandUseThemeName)

            If aLUType = 1 Then 'cboLanduse.SelectedIndex = 1 Then 
                'nlcd grid
                Dim lBasinsBinLoc As String = PathNameOnly(System.Reflection.Assembly.GetEntryAssembly.Location)
                lReclassifyFileName = lBasinsBinLoc.Substring(0, lBasinsBinLoc.Length - 3) & "etc\"
                If FileExists(lReclassifyFileName) Then
                    lReclassifyFileName &= "nlcd.dbf"
                Else
                    lReclassifyFileName = "\BASINS\etc\nlcd.dbf"
                End If
            Else
                If aLandUseClassFile <> "<none>" Then 'lblClass.Text <> "<none>" Then
                    lReclassifyFileName = aLandUseClassFile 'lblClass.Text
                End If
            End If

        ElseIf aLUType = 2 Then 'cboLanduse.SelectedIndex = 2 Then
            'other shape
            CreateLanduseRecordsShapefile(lSubbasinsSelected, lLucodes, lSubids, lAreas, aSubbasinThemeName, aSubbasinFieldName, aLandUseThemeName, "")

            lReclassifyFileName = ""
            If aLandUseClassFile <> "none" Then 'lblClass.Text <> "<none>" Then
                lReclassifyFileName = aLandUseClassFile 'lblClass.Text
            End If

        End If

        'special code to always include certain land uses
        Dim lLUInclude() As Integer = {61, 71, 81, 91}
        Dim lSub As Integer
        Dim lFoundLU As Boolean = False
        Dim lInd As Integer
        For Each lLU As Integer In lLUInclude
            lSub = lSubids(1)
            lInd = 1
            While lInd <= lSubids.Count
                If lLucodes(lInd) = lLU Then lFoundLU = True
                If lSubids(lInd) <> lSub OrElse lInd = lSubids.Count Then
                    'new subbasin, if LU not found, need to add it
                    If Not lFoundLU Then
                        lLucodes.Add(lLU, , lInd)
                        lSubids.Add(lSub, , lInd)
                        lAreas.Add(0.001, , lInd)
                        lInd += 1
                        lSub = lSubids(lInd)
                    End If
                    lFoundLU = False
                End If
                lInd += 1
            End While
        Next

        'todo: replace below with logger/status message on main BASINS form?
        'lblStatus.Text = "Completed overlay of subbasins and land use layers"
        'Me.Refresh()

        'Create Reach Segments
        Dim lReaches As Reaches = CreateReachSegments(lSubbasinsSelected, lSubbasinsModelSegmentIds, aStreamLayerName, aStreamFields)

        'Create Stream Channels
        Dim lChannels As Channels = CreateStreamChannels(lReaches)

        'Create LandUses
        Dim lLandUses As LandUses = CreateLanduses(lSubbasinsSlopes, lLucodes, lSubids, lAreas, lReaches)

        'figure out which outlets are in which subbasins
        'Dim lOutletsThemeName As String = cboOutlets.Items(cboOutlets.SelectedIndex)
        Dim lOutSubs As New Collection
        'If lOutletsThemeName <> "<none>" Then
        If aOutletsThemeName <> "<none>" Then
            'todo: replace below with logger/status message on main BASINS form?
            'lblStatus.Text = "Joining point sources to subbasins"
            'Me.Refresh()
            Dim i As Integer = GisUtil.LayerIndex(aOutletsThemeName)
            For j As Integer = 1 To GisUtil.NumFeatures(i)
                Dim k As Integer = GisUtil.PointInPolygon(i, j, lSubbasinLayerIndex)
                If k > -1 Then
                    lOutSubs.Add(GisUtil.FieldValue(lSubbasinLayerIndex, k, lSubbasinFieldIndex))
                Else
                    lOutSubs.Add(-1)
                End If
            Next j
        End If

        'make output folder
        MkDirPath(aOutputPath)
        Dim lBaseFileName As String = aOutputPath & "\" & aBaseOutputName

        'write wsd file
        'todo: replace below with logger/status message on main BASINS form?
        'lblStatus.Text = "Writing WSD file"
        'Me.Refresh()
        Dim lReclassifyLanduses As LandUses = ReclassifyLandUses(lReclassifyFileName, AtcGridPervious, lLandUses)
        WriteWSDFile(lBaseFileName & ".wsd", lReclassifyLanduses)
        'WriteWSDFile(lBaseFileName & ".wsd", lAreas, lLucodes, lSubids, cSubSlope, lReclassifyFileName, AtcGridPervious)

        'write rch file 
        'todo: replace below with logger/status message on main BASINS form?
        'lblStatus.Text = "Writing RCH file"
        'Me.Refresh()
        WriteRCHFile(lBaseFileName & ".rch", lReaches)

        'write ptf file
        'todo: replace below with logger/status message on main BASINS form?
        'lblStatus.Text = "Writing PTF file"
        'Me.Refresh()
        WritePTFFile(lBaseFileName & ".ptf", lChannels)

        'write psr file
        'todo: replace below with logger/status message on main BASINS form?
        'lblStatus.Text = "Writing PSR file"
        'Me.Refresh()
        Dim lOutletsLayerIndex As Integer
        Dim lPointLayerIndex As Integer
        'Dim lYear As String = ""
        If lOutSubs.Count > 0 Then
            lOutletsLayerIndex = GisUtil.LayerIndex(aOutletsThemeName) '(cboOutlets.Items(cboOutlets.SelectedIndex))
            lPointLayerIndex = GisUtil.FieldIndex(lOutletsLayerIndex, aPointThemeName) 'cboPoint.Items(cboPoint.SelectedIndex))
            'lYear = cboYear.Items(cboYear.SelectedIndex)
        End If
        WritePSRFile(lBaseFileName & ".psr", lSubbasinsSelected, lOutSubs, lOutletsLayerIndex, lPointLayerIndex, _
                        aPSRCustom, aPSRCustomFile, aPSRCalculate, aPointYear)
        'chkCustom.Checked, lblCustom.Text, chkCalculate.Checked, aPointYear)

        'write seg file
        'todo: replace below with logger/status message on main BASINS form?
        'lblStatus.Text = "Writing SEG file"
        'Me.Refresh()
        Dim lMetIndices As New atcCollection
        Dim lUniqueModelSegmentIds As New atcCollection
        If pUniqueModelSegmentIds.Count = 0 Then
            'use a single met station
            'lMetIndices.Add(lstMet.SelectedIndex)
            lMetIndices.Add(1)
            lUniqueModelSegmentIds.Add(1)
        Else
            'use the specified segmentation scheme
            For lRow As Integer = 1 To AtcGridMet.Source.Rows - 1
                lMetIndices.Add(pMetStations.IndexFromKey(AtcGridMet.Source.CellValue(lRow, 1)))
            Next
            lUniqueModelSegmentIds = pUniqueModelSegmentIds
        End If
        WriteSEGFile(lBaseFileName & ".seg", lUniqueModelSegmentIds, lMetIndices, pMetBaseDsns)

        'write map file
        'todo: replace below with logger/status message on main BASINS form?
        'lblStatus.Text = "Writing MAP file"
        'Me.Refresh()
        WriteMAPFile(lBaseFileName & ".map")

        'todo: replace below with logger/status message on main BASINS form?
        'lblStatus.Text = ""
        'Me.Refresh()
        'Me.Dispose()
        'Me.Close()

        Return True
    End Function

End Module
