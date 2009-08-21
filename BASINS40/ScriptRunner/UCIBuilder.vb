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
        SetPerviousGrid(pLandUseClassFile)
        pMetStations = New atcCollection
        pMetBaseDsns = New atcCollection
        BuildListofMetStationNames(pMetWDM, pMetStations, pMetBaseDsns)
        AtcGridMet = New atcControls.atcGrid
        AtcGridMet.Source = New atcControls.atcGridSource
        SetMetSegmentGrid(pSubbasinThemeName, pSubbasinFieldName, pSubbasinSegmentName)

        If PreProcessChecking(pOutputPath, pBaseOutputName, pLUType, pSubbasinThemeName, pLandUseThemeName) Then 'early checks OK
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
            CreateLanduseRecordsShapefile(lSubbasinsSelected, lLucodes, lSubids, lAreas, aSubbasinThemeName, aSubbasinFieldName, aLandUseThemeName)

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

    Private Sub CreateLanduseRecordsGIRAS(ByVal aSubbasinsSelected As atcCollection, ByRef aLucode As Collection, _
                                          ByRef aSubid As Collection, ByRef aArea As Collection, _
                                          ByVal aSubbasinThemeName As String, ByVal aSubbasinFieldName As String)

        'perform overlay for GIRAS 
        'Dim lSubbasinThemeName As String = cboSubbasins.Items(cboSubbasins.SelectedIndex)
        'Dim lSubbasinLayerIndex As Long = GisUtil.LayerIndex(lSubbasinThemeName)
        Dim lSubbasinLayerIndex As Long = GisUtil.LayerIndex(aSubbasinThemeName)
        'Dim lSubbasinFieldName As String = cboSub1.Items(cboSub1.SelectedIndex)

        'todo: replace below with logger/status message on main BASINS form?
        'lblStatus.Text = "Selecting land use tiles for overlay"
        'Me.Refresh()

        'set land use index layer
        Dim lLandUseThemeName As String = "Land Use Index"
        Dim lLanduseFieldName As String = "COVNAME"
        Dim lLanduseLayerIndex As Integer = GisUtil.LayerIndex(lLandUseThemeName)
        Dim lLandUseFieldIndex As Integer = GisUtil.FieldIndex(lLanduseLayerIndex, lLanduseFieldName)
        Dim lLandUsePathName As String = PathNameOnly(GisUtil.LayerFileName(lLanduseLayerIndex)) & "\landuse"

        'figure out which land use tiles to overlay
        Dim lLandUseTiles As New atcCollection
        For i As Integer = 1 To GisUtil.NumFeatures(lLanduseLayerIndex)
            'loop thru each shape of land use index shapefile
            For j As Integer = 0 To aSubbasinsSelected.Count - 1
                'loop thru each selected subbasin (or all if none selected)
                Dim lShapeIndex As Long = aSubbasinsSelected.Keys(j)
                If GisUtil.OverlappingPolygons(lLanduseLayerIndex, i - 1, lSubbasinLayerIndex, lShapeIndex) Then
                    'add this to collection of tiles we'll need
                    lLandUseTiles.Add(GisUtil.FieldValue(lLanduseLayerIndex, i - 1, lLandUseFieldIndex))
                End If
            Next j
        Next i

        'add tiles if not already on map
        'figure out how many polygons to overlay, for status message
        Dim lTotalPolygonCount As Integer = 0
        Dim lTileFileNames As New atcCollection
        For Each lLandUseTile As String In lLandUseTiles
            Dim lNewFileName As String = lLandUsePathName & "\" & lLandUseTile & ".shp"
            lTileFileNames.Add(lNewFileName)
            If Not GisUtil.IsLayerByFileName(lNewFileName) Then
                If Not GisUtil.AddLayer(lNewFileName, lLandUseTile) Then
                    Logger.Msg("The GIRAS Landuse Shapefile " & lNewFileName & "does not exist." & _
                                vbCrLf & "Run the Download tool to bring this data into your project.", vbOKOnly, "HSPF Problem")
                    'EnableControls(True)
                    Exit Sub
                End If
            End If
            lTotalPolygonCount += GisUtil.NumFeatures(GisUtil.LayerIndex(lNewFileName))
        Next
        lTotalPolygonCount *= aSubbasinsSelected.Count

        'reset selected features since they may have become unselected
        If aSubbasinsSelected.Count < GisUtil.NumFeatures(lSubbasinLayerIndex) Then
            GisUtil.ClearSelectedFeatures(lSubbasinLayerIndex)
            For Each lSubbasin As Integer In aSubbasinsSelected.Keys
                GisUtil.SetSelectedFeature(lSubbasinLayerIndex, lSubbasin)
            Next
        End If

        lLanduseFieldName = "LUCODE"
        Dim lFirst As Boolean = True
        Dim lTileIndex As Integer = 0
        For Each lTileFileName As String In lTileFileNames
            lTileIndex += 1
            'todo: replace below with logger/status message on main BASINS form?
            'lblStatus.Text = "Overlaying Land Use and Subbasins (Tile " & lTileIndex & " of " & lTileFileNames.Count & ")"
            'Me.Refresh()
            'do overlay
            GisUtil.Overlay(lTileFileName, lLanduseFieldName, aSubbasinThemeName, aSubbasinFieldName, _
                            lLandUsePathName & "\overlay.shp", lFirst)
            lFirst = False
        Next

        'todo: replace below with logger/status message on main BASINS form?
        'compile areas, slopes and lengths
        'lblStatus.Text = "Compiling Overlay Results"
        'Me.Refresh()

        Dim lTable As IatcTable = atcUtility.atcTableOpener.OpenAnyTable(lLandUsePathName & "\overlay.dbf")
        For i As Integer = 1 To lTable.NumRecords
            lTable.CurrentRecord = i
            aLucode.Add(lTable.Value(1))
            aSubid.Add(lTable.Value(2))
            aArea.Add(CDbl(lTable.Value(3)))
        Next i
    End Sub

    Private Sub CreateLanduseRecordsShapefile(ByVal aSubbasinsSelected As atcCollection, ByRef aLucode As Collection, _
                                              ByRef aSubid As Collection, ByRef aArea As Collection, _
                                              ByVal aSubbasinThemeName As String, ByVal aSubbasinFieldName As String, _
                                              Optional ByVal aLandUseThemeName As String = "", Optional ByVal aLandUseFieldName As String = "")

        'perform overlay for other shapefiles (not GIRAS) 
        'Dim lSubbasinThemeName As String = cboSubbasins.Items(cboSubbasins.SelectedIndex)
        'Dim lSubbasinFieldName As String = cboSub1.Items(cboSub1.SelectedIndex)

        'todo: replace below with logger/status message on main BASINS form?
        'lblStatus.Text = "Overlaying Land Use and Subbasins"
        'Me.Refresh()

        'Dim lLandUseThemeName As String = ""
        'If cboLandUseLayer.SelectedIndex > -1 Then
        '    lLandUseThemeName = cboLandUseLayer.Items(cboLandUseLayer.SelectedIndex)
        'End If
        'Dim lLanduseFieldName As String = ""
        'If cboDescription.SelectedIndex > -1 Then
        '    lLanduseFieldName = cboDescription.Items(cboDescription.SelectedIndex)
        'End If
        Dim lLanduseLayerIndex As Integer = GisUtil.LayerIndex(aLandUseThemeName)
        Dim lLandUsePathName As String = PathNameOnly(GisUtil.LayerFileName(lLanduseLayerIndex))

        'do overlay
        GisUtil.Overlay(aLandUseThemeName, aLandUseFieldName, aSubbasinThemeName, aSubbasinFieldName, _
                        lLandUsePathName & "\overlay.shp", True)

        'todo: replace below with logger/status message on main BASINS form?
        'compile areas and slopes
        'lblStatus.Text = "Compiling Overlay Results"
        'Me.Refresh()

        Dim lTable As IatcTable = atcUtility.atcTableOpener.OpenAnyTable(lLandUsePathName & "\overlay.dbf")
        For i As Integer = 1 To lTable.NumRecords
            lTable.CurrentRecord = i
            aLucode.Add(lTable.Value(1))
            aSubid.Add(lTable.Value(2))
            aArea.Add(CDbl(lTable.Value(3)))
        Next i
    End Sub

    Private Sub CreateLanduseRecordsGrid(ByVal aSubbasinsSelected As atcCollection, ByRef aLucode As Collection, _
                                         ByRef aSubid As Collection, ByRef aArea As Collection, _
                                         ByVal aSubbasinThemeName As String, Optional ByVal aLandUseThemeName As String = "")

        'perform overlay for land use grid
        'Dim lSubbasinThemeName As String = cboSubbasins.Items(cboSubbasins.SelectedIndex)
        'Dim lSubbasinLayerIndex As Long = GisUtil.LayerIndex(lSubbasinThemeName)
        Dim lSubbasinLayerIndex As Long = GisUtil.LayerIndex(aSubbasinThemeName)

        'todo: replace below with logger/status message on main BASINS form?
        'lblStatus.Text = "Overlaying Land Use and Subbasins"
        'Me.Refresh()

        'Dim lLandUseThemeName As String = ""
        'If cboLandUseLayer.SelectedIndex > -1 Then
        '    lLandUseThemeName = cboLandUseLayer.Items(cboLandUseLayer.SelectedIndex)
        'End If
        Dim lLanduseLayerIndex As Integer = GisUtil.LayerIndex(aLandUseThemeName)
        If GisUtil.LayerType(lLanduseLayerIndex) = 4 Then
            'the landuse layer is a grid

            Dim k As Integer = Convert.ToInt32(GisUtil.GridLayerMaximum(lLanduseLayerIndex))
            Dim lAreaLS(k, GisUtil.NumFeatures(lSubbasinLayerIndex)) As Double
            GisUtil.TabulateAreas(lLanduseLayerIndex, lSubbasinLayerIndex, lAreaLS)

            For Each lShapeindex As String In aSubbasinsSelected.Keys
                'loop thru each selected subbasin (or all if none selected)
                Dim lSubid As String = aSubbasinsSelected.ItemByKey(CInt(lShapeindex)).ToString
                For i As Integer = 1 To Convert.ToInt32(GisUtil.GridLayerMaximum(lLanduseLayerIndex))
                    If lAreaLS(i, lShapeindex) > 0 Then
                        aLucode.Add(i)
                        aArea.Add(lAreaLS(i, lShapeindex))
                        aSubid.Add(lSubid)
                    End If
                Next i
            Next

        End If

    End Sub

    Private Function CreateLanduses(ByVal aSubbasinsSlopes As atcCollection, ByVal aLucodes As Collection, _
                                    ByVal aSubids As Collection, ByVal aAreas As Collection, ByVal aReaches As Reaches) As LandUses

        Dim lLandUses As New LandUses
        For lIndex As Integer = 1 To aLucodes.Count
            Dim lLandUse As New LandUse
            With lLandUse
                .Code = aLucodes(lIndex)
                .ModelID = aSubids(lIndex)
                .Area = aAreas(lIndex)
                .Slope = aSubbasinsSlopes.ItemByKey(CInt(aSubids(lIndex)))
                .Description = .Code
                '.Distance()
                '.ImperviousFraction()
                For Each lReach As Reach In aReaches
                    If lReach.Id = aSubids(lIndex) Then
                        .Reach = lReach
                        Exit For
                    End If
                Next
                .Type = "COMPOSITE"
            End With
            Dim lExistIndex As Integer = lLandUses.IndexOf(lLandUse)
            If lLandUses.Contains(lLandUse.Description & ":" & lLandUse.Reach.Id) Then  'already have, add area
                lLandUses.Item(lLandUse.Description & ":" & lLandUse.Reach.Id).Area += lLandUse.Area
            Else 'new
                lLandUses.Add(lLandUse)
            End If
        Next

        Return lLandUses
    End Function

    Private Function CreateReachSegments(ByVal aSubbasinsSelected As atcCollection, ByVal aSubbasinsModelSegmentIds As atcCollection, _
                                         ByVal aStreamLayerName As String, ByVal aStreamFields() As String) As Reaches

        'for reaches in selected subbasins, populate reach class from dbf
        Dim lReaches As New Reaches
        'Dim lStreamsLayerIndex As Integer = GisUtil.LayerIndex(cboStreams.Items(cboStreams.SelectedIndex))
        Dim lStreamsLayerIndex As Integer = GisUtil.LayerIndex(aStreamLayerName)
        'Dim lStreamsFieldIndex As Integer = GisUtil.FieldIndex(lStreamsLayerIndex, cboStream1.Items(cboStream1.SelectedIndex))
        'Dim lStreamsRIndex As Integer = GisUtil.FieldIndex(lStreamsLayerIndex, cboStream2.Items(cboStream2.SelectedIndex))
        'Dim lLen2Index As Integer = GisUtil.FieldIndex(lStreamsLayerIndex, cboStream3.Items(cboStream3.SelectedIndex))
        'Dim lSlo2Index As Integer = GisUtil.FieldIndex(lStreamsLayerIndex, cboStream4.Items(cboStream4.SelectedIndex))
        'Dim lWid2Index As Integer = GisUtil.FieldIndex(lStreamsLayerIndex, cboStream5.Items(cboStream5.SelectedIndex))
        'Dim lDep2Index As Integer = GisUtil.FieldIndex(lStreamsLayerIndex, cboStream6.Items(cboStream6.SelectedIndex))
        'Dim lMinelIndex As Integer = GisUtil.FieldIndex(lStreamsLayerIndex, cboStream7.Items(cboStream7.SelectedIndex))
        'Dim lMaxelIndex As Integer = GisUtil.FieldIndex(lStreamsLayerIndex, cboStream8.Items(cboStream8.SelectedIndex))
        'Dim lSnameIndex As Integer = GisUtil.FieldIndex(lStreamsLayerIndex, cboStream9.Items(cboStream9.SelectedIndex))
        Dim lStreamsFieldIndex As Integer = GisUtil.FieldIndex(lStreamsLayerIndex, aStreamFields(0))
        Dim lStreamsRIndex As Integer = GisUtil.FieldIndex(lStreamsLayerIndex, aStreamFields(1))
        Dim lLen2Index As Integer = GisUtil.FieldIndex(lStreamsLayerIndex, aStreamFields(2))
        Dim lSlo2Index As Integer = GisUtil.FieldIndex(lStreamsLayerIndex, aStreamFields(3))
        Dim lWid2Index As Integer = GisUtil.FieldIndex(lStreamsLayerIndex, aStreamFields(4))
        Dim lDep2Index As Integer = GisUtil.FieldIndex(lStreamsLayerIndex, aStreamFields(5))
        Dim lMinelIndex As Integer = GisUtil.FieldIndex(lStreamsLayerIndex, aStreamFields(6))
        Dim lMaxelIndex As Integer = GisUtil.FieldIndex(lStreamsLayerIndex, aStreamFields(7))
        Dim lSnameIndex As Integer = GisUtil.FieldIndex(lStreamsLayerIndex, aStreamFields(8))

        For lStreamIndex As Integer = 1 To GisUtil.NumFeatures(lStreamsLayerIndex)
            For Each lSubbasinId As Integer In aSubbasinsSelected
                If lSubbasinId = GisUtil.FieldValue(lStreamsLayerIndex, lStreamIndex - 1, lStreamsFieldIndex) Then
                    'this is one we want
                    Dim lReach As New Reach
                    With lReach
                        .Id = lSubbasinId
                        .Name = GisUtil.FieldValue(lStreamsLayerIndex, lStreamIndex - 1, lSnameIndex)
                        If Len(Trim(.Name)) = 0 Then
                            .Name = "STREAM " + lSubbasinId.ToString
                        End If
                        .WsId = lSubbasinId
                        .NExits = 1
                        .Type = "S"
                        .DownID = GisUtil.FieldValue(lStreamsLayerIndex, lStreamIndex - 1, lStreamsRIndex)
                        .Manning = 0.05
                        .Order = lReaches.Count + 1
                        If IsNumeric(GisUtil.FieldValue(lStreamsLayerIndex, lStreamIndex - 1, lLen2Index)) Then
                            .Length = (CSng(GisUtil.FieldValue(lStreamsLayerIndex, lStreamIndex - 1, lLen2Index)) * 3.28) / 5280
                        Else
                            .Length = 0.0#
                        End If
                        If IsNumeric(GisUtil.FieldValue(lStreamsLayerIndex, lStreamIndex - 1, lDep2Index)) Then
                            .Depth = CSng(GisUtil.FieldValue(lStreamsLayerIndex, lStreamIndex - 1, lDep2Index)) * 3.28
                        Else
                            .Depth = 0.0#
                        End If
                        If IsNumeric(GisUtil.FieldValue(lStreamsLayerIndex, lStreamIndex - 1, lWid2Index)) Then
                            .Width = CSng(GisUtil.FieldValue(lStreamsLayerIndex, lStreamIndex - 1, lWid2Index)) * 3.28
                        Else
                            .Width = 0.0#
                        End If
                        Dim lMinEl As Single
                        If IsNumeric(GisUtil.FieldValue(lStreamsLayerIndex, lStreamIndex - 1, lMinelIndex)) Then
                            lMinEl = CSng(GisUtil.FieldValue(lStreamsLayerIndex, lStreamIndex - 1, lMinelIndex)) * 3.28
                        Else
                            lMinEl = 0.0#
                        End If
                        Dim lMaxEl As Single
                        If IsNumeric(GisUtil.FieldValue(lStreamsLayerIndex, lStreamIndex - 1, lMaxelIndex)) Then
                            lMaxEl = CSng(GisUtil.FieldValue(lStreamsLayerIndex, lStreamIndex - 1, lMaxelIndex)) * 3.28
                        Else
                            lMaxEl = 0.0#
                        End If
                        .Elev = ((lMaxEl + lMinEl) / 2)
                        .DeltH = lMaxEl - lMinEl
                        .SegmentId = aSubbasinsModelSegmentIds.ItemByKey(lSubbasinId)
                    End With
                    If Not lReaches.Contains(lReach.Id) Then
                        lReaches.Add(lReach)
                    End If
                End If
            Next
        Next
        Return lReaches
    End Function

    Private Function CreateStreamChannels(ByVal aReaches As Reaches) As Channels

        'for reaches in selected subbasins, populate channel class from dbf
        Dim lChannels As New Channels

        For Each lReach As Reach In aReaches
            Dim lChannel As New Channel
            With lChannel
                .Reach = lReach
                .Length = lReach.Length * 5280
                .DepthMean = lReach.Depth
                .WidthMean = lReach.Width
                .ManningN = 0.05
                .SlopeProfile = Math.Abs(lReach.DeltH / (lReach.Length * 5280))
                If .SlopeProfile < 0.0001 Then
                    .SlopeProfile = 0.001
                End If
                .SlopeSideUpperFPLeft = 0.5
                .SlopeSideLowerFPLeft = 0.5
                .WidthZeroSlopeLeft = lReach.Width
                .SlopeSideLeft = 1
                .SlopeSideRight = 1
                .WidthZeroSlopeRight = lReach.Width
                .SlopeSideLowerFPRight = 0.5
                .SlopeSideUpperFPRight = 0.5
                .DepthChannel = lReach.Depth * 1.25
                .DepthSlopeChange = lReach.Depth * 1.875
                .DepthMax = lReach.Depth * 62.5
            End With
            If Not lChannels.Contains(lChannel.Reach.Id) Then
                lChannels.Add(lChannel)
            End If
        Next
        Return lChannels
    End Function

    Friend Sub WriteWSDFile(ByVal aWsdFileName As String, _
                            ByVal aLandUses As LandUses)

        Dim lSB As New StringBuilder
        lSB.AppendLine("""LU Name""" & "," & """Type (1=Impervious, 2=Pervious)""" & "," & """Watershd-ID""" & "," & _
                       """Area""" & "," & """Slope""" & "," & """Distance""")
        For Each lLandUse As LandUse In aLandUses
            Dim lType As String = "2"
            Dim lArea As Double = lLandUse.Area * (1 - lLandUse.ImperviousFraction) / 4046.8564
            If lArea > 0 Then 'or CInt(lArea)
                lSB.AppendLine(Chr(34) & lLandUse.Description & Chr(34) & "     " & _
                               lType & "     " & _
                               lLandUse.ModelID & "     " & _
                               Format(lArea, "0.0") & "     " & _
                               Format(lLandUse.Slope, "0.000000") & "     " & _
                               Format(lLandUse.Distance, "0.0000"))
            End If
            lType = "1"
            lArea = lLandUse.Area * lLandUse.ImperviousFraction / 4046.8564
            If lArea > 0 Then 'or CInt(lArea)
                lSB.AppendLine(Chr(34) & lLandUse.Description & Chr(34) & "     " & _
                               lType & "     " & _
                               lLandUse.ModelID & "     " & _
                               Format(lArea, "0.0") & "     " & _
                               Format(lLandUse.Slope, "0.000000") & "     " & _
                               Format(lLandUse.Distance, "0.0000"))
            End If
        Next lLandUse
        SaveFileString(aWsdFileName, lSB.ToString)
    End Sub

    Friend Sub WriteRCHFile(ByVal aRchFileName As String, _
                            ByVal aReaches As Reaches)

        Dim lSBRch As New StringBuilder

        lSBRch.AppendLine("""Rivrch""" & "," & """Pname""" & "," & """Watershed-ID""" & "," & """HeadwaterFlag""" & "," & _
                  """Exits""" & "," & """Milept""" & "," & """Stream/Resevoir Type""" & "," & """Segl""" & "," & _
                  """Delth""" & "," & """Elev""" & "," & """Ulcsm""" & "," & """Urcsm""" & "," & """Dscsm""" & "," & """Ccsm""" & "," & _
                  """Mnflow""" & "," & """Mnvelo""" & "," & """Svtnflow""" & "," & """Svtnvelo""" & "," & """Pslope""" & "," & _
                  """Pdepth""" & "," & """Pwidth""" & "," & """Pmile""" & "," & """Ptemp""" & "," & """Pph""" & "," & """Pk1""" & "," & _
                  """Pk2""" & "," & """Pk3""" & "," & """Pmann""" & "," & """Psod""" & "," & """Pbgdo""" & "," & _
                  """Pbgnh3""" & "," & """Pbgbod5""" & "," & """Pbgbod""" & "," & """Level""" & "," & """ModelSeg""")

        Dim lSlope As Double
        For Each lReach As Reach In aReaches
            With lReach
                lSlope = Math.Abs(.DeltH / (.Length * 5280))
                If lSlope < 0.00001 Then
                    lSlope = 0.001
                End If
                lSBRch.AppendLine(.Id & " " & Chr(34) & .Name & Chr(34) & " " & .Id & " " & _
                       " 0 1 0 S " & Format(.Length, "0.00") & " " & Format(Math.Abs(.DeltH), "0.00") & " " & _
                       Format(.Elev, "0.") & " 0 0 " & .DownID & " 0 0 0 0 0 " & _
                       Format(lSlope, "0.000000") & " " & Format(.Depth, "0.0000") & " " & Format(.Width, "0.000") & _
                       " 0 0 0 0 0 0 0 0 0 0 0 0 0 " & .SegmentId)
                If (2 * .Depth) > .Width Then 'problem
                    Logger.Msg("The depth and width values specified for Reach " & lReach.Id & ", coupled with the trapezoidal" & vbCrLf & _
                           "cross section assumptions of WinHSPF, indicate a physical imposibility." & vbCrLf & _
                           "(Given 1:1 side slopes, the depth of the channel cannot be more than half the width.)" & vbCrLf & vbCrLf & _
                           "This problem can be corrected in WinHSPF by revising the FTABLE or by " & vbCrLf & _
                           "importing the ptf with modifications to the width and depth values." & vbCrLf & _
                           "See the WinHSPF manual for more information.", vbOKOnly, "Channel Problem")
                End If
            End With
        Next

        SaveFileString(aRchFileName, lSBRch.ToString)
    End Sub

    Friend Sub WritePTFFile(ByVal aPtfFileName As String, _
                            ByVal aChannels As Channels)
        Dim lSBPtf As New StringBuilder

        lSBPtf.AppendLine("""Reach Number""" & "," & """Length(ft)""" & "," & _
            """Mean Depth(ft)""" & "," & """Mean Width (ft)""" & "," & _
            """Mannings Roughness Coeff.""" & "," & """Long. Slope""" & "," & _
            """Type of x-section""" & "," & """Side slope of upper FP left""" & "," & _
            """Side slope of lower FP left""" & "," & """Zero slope FP width left(ft)""" & "," & _
            """Side slope of channel left""" & "," & """Side slope of channel right""" & "," & _
            """Zero slope FP width right(ft)""" & "," & """Side slope lower FP right""" & "," & _
            """Side slope upper FP right""" & "," & """Channel Depth(ft)""" & "," & _
            """Flood side slope change at depth""" & "," & """Max. depth""" & "," & _
            """No. of exits""" & "," & """Fraction of flow through exit 1""" & "," & _
            """Fraction of flow through exit 2""" & "," & """Fraction of flow through exit 3""" & "," & _
            """Fraction of flow through exit 4""" & "," & """Fraction of flow through exit 5""")

        For Each lChannel As Channel In aChannels
            With lChannel
                lSBPtf.AppendLine(.Reach.Id & " " & Format(.Length, "0.") & " " & _
                       Format(.DepthMean, "0.00000") & " " & Format(.WidthMean, "0.00000") & " " & _
                       Format(.ManningN, "0.00") & " " & Format(.SlopeProfile, "0.00000") & " " & "Trapezoidal" & " " & _
                       Format(.SlopeSideUpperFPLeft, "0.0") & " " & Format(.SlopeSideLowerFPLeft, "0.0") & " " & _
                       Format(.WidthZeroSlopeLeft, "0.000") & " " & .SlopeSideLeft & " " & .SlopeSideRight & " " & _
                       Format(.WidthZeroSlopeRight, "0.000") & " " & _
                       Format(.SlopeSideLowerFPRight, "0.0") & " " & Format(.SlopeSideUpperFPRight, "0.0") & " " & _
                       Format(.DepthChannel, "0.0000") & " " & Format(.DepthSlopeChange, "0.0000") & " " & _
                       Format(.DepthMax, "0.000") & " 1 1 0 0 0 0")
            End With
        Next

        SaveFileString(aPtfFileName, lSBPtf.ToString)
    End Sub

    Friend Sub WritePSRFile(ByVal aPsrFileName As String, ByVal aUniqueSubids As atcCollection, ByVal aOutSubs As Collection, _
                            ByVal aLayerIndex As Integer, ByVal aPointIndex As Integer, ByVal aChkCustom As Boolean, _
                            ByVal aLblCustom As String, ByVal aChkCalculate As Boolean, ByVal aYear As String)
        Dim lSB As New StringBuilder

        Dim lNPDESSites As New Collection
        Dim lSubbasins As New Collection
        Dim lFlows As New Collection
        Dim lMipts As New Collection
        Dim lFacNames As New Collection
        Dim lHucs As New Collection

        Dim lDbName As String = ""
        Dim lRowCount As Long = 0
        Dim lPcsLayerIndex As Integer
        If aOutSubs.Count > 0 Then 'build collection of npdes sites to output
            For i As Integer = 1 To aOutSubs.Count
                For j As Integer = 0 To aUniqueSubids.Count - 1
                    If aOutSubs(i) = aUniqueSubids(j) Then 'found this subbasin in selected list
                        If GisUtil.FieldValue(aLayerIndex, i - 1, aPointIndex).Length > 0 Then
                            lNPDESSites.Add(GisUtil.FieldValue(aLayerIndex, i - 1, aPointIndex))
                            lSubbasins.Add(aOutSubs(i))
                        End If
                    End If
                Next j
            Next i

            'If cNPDES.Count = 0 Then
            '  MsgBox("No point sources have been added to the outlets layer." & vbCrLf & _
            '  "To add point sources, update the outlets layer using the" & vbCrLf & _
            '  "BASINS watershed delineator or update it manually.", vbOKOnly, "BASINS HSPF Information")
            'End If

            If Not aChkCustom Then 'use pcs data
                If GisUtil.IsLayer("Permit Compliance System") Then
                    'set pcs shape file
                    lPcsLayerIndex = GisUtil.LayerIndex("Permit Compliance System")
                    Dim lNpdesFieldIndex As Integer = GisUtil.FieldIndex(lPcsLayerIndex, "NPDES")
                    Dim lFlowFieldIndex As Integer = GisUtil.FieldIndex(lPcsLayerIndex, "FLOW_RATE")
                    Dim lFacFieldIndex As Integer = GisUtil.FieldIndex(lPcsLayerIndex, "FAC_NAME")
                    Dim lCuFieldIndex As Integer = GisUtil.FieldIndex(lPcsLayerIndex, "BCU")
                    If lNpdesFieldIndex > -1 Then
                        For lNpdesIndex As Integer = 1 To lNPDESSites.Count
                            Dim lFlow As Double = 0.0
                            Dim lFacName As String = ""
                            Dim lHuc As String = ""
                            Dim lMipt As Single = 0.0#
                            If lNPDESSites(lNpdesIndex).ToString.Trim.Length > 0 Then
                                For j As Integer = 1 To GisUtil.NumFeatures(lPcsLayerIndex)
                                    If GisUtil.FieldValue(lPcsLayerIndex, j - 1, lNpdesFieldIndex) = lNPDESSites(lNpdesIndex) Then
                                        'this is the one
                                        If IsNumeric(GisUtil.FieldValue(lPcsLayerIndex, j - 1, lFlowFieldIndex)) Then
                                            lFlow = GisUtil.FieldValue(lPcsLayerIndex, j - 1, lFlowFieldIndex) * 1.55
                                        Else
                                            lFlow = 0.0
                                        End If
                                        lFacName = GisUtil.FieldValue(lPcsLayerIndex, j - 1, lFacFieldIndex)
                                        If aChkCalculate Then
                                            'calculate mile point on stream
                                            'dist = myGISTools.NearestPositionOnLineToPoint(StreamsThemeName, StreamsField, cSubbasin(i), IO.Path.GetFileNameWithoutExtension(OutletsJoinThemeName), PCSIdField, pNPDES(j))
                                            'mipt = dist / 1609.3
                                        Else
                                            lMipt = 0.0#
                                        End If
                                        lHuc = GisUtil.FieldValue(lPcsLayerIndex, j - 1, lCuFieldIndex)
                                        Exit For
                                    End If
                                Next j
                            End If
                            lFlows.Add(lFlow)
                            lMipts.Add(lMipt)
                            lFacNames.Add(lFacName)
                            lHucs.Add(lHuc)
                        Next lNpdesIndex
                    End If
                    'check for dbf associated with each npdes point
                    Dim i As Integer = 1
                    lDbName = PathNameOnly(GisUtil.LayerFileName(lPcsLayerIndex)) & "\pcs\"
                    For Each lNpdesSite As Object In lNPDESSites
                        Dim lDbfFileName As String = lHucs(i).ToString.Trim & ".dbf"
                        If IO.File.Exists(lDbName & lDbfFileName) > 0 And lNpdesSite.ToString.Trim.Length > 0 Then
                            'yes, it exists
                            i += 1
                        Else 'remove from collection
                            lNPDESSites.Remove(i)
                            lSubbasins.Remove(i)
                            lFlows.Remove(i)
                            lMipts.Remove(i)
                            lFacNames.Remove(i)
                            lHucs.Remove(i)
                        End If
                    Next lNpdesSite
                Else
                    'no pcs layer, clear out
                    Do While lNPDESSites.Count > 0
                        lNPDESSites.Remove(1)
                    Loop
                End If
            Else
                'using custom table
                'must have these fields in this order:
                'pcsid (same as outlets layer)
                'facname
                'load (flow or other value) lbs/yr or cfs
                'parm (flow or other name)
                Dim lDbf As IatcTable = atcUtility.atcTableOpener.OpenAnyTable(aLblCustom)

                Dim i As Integer = 1
                Do While i <= lNPDESSites.Count
                    Dim lMipt As Single = 0.0#
                    If aChkCalculate Then
                        'calculate mile point on stream
                        'dist = myGISTools.NearestPositionOnLineToPoint(StreamsThemeName, StreamsField, cSubbasin(i), IO.Path.GetFileNameWithoutExtension(OutletsJoinThemeName), PCSIdField, pNPDES(j))
                        'mipt = dist / 1609.3
                    Else
                        lMipt = 0.0#
                    End If
                    lMipts.Add(lMipt)
                    Dim lFound As Boolean = False
                    For j As Integer = 1 To lDbf.NumRecords
                        lDbf.CurrentRecord = j
                        If lNPDESSites(i) = lDbf.Value(1) Then
                            lFacNames.Add(lDbf.Value(2))
                            lFound = True
                            Exit For
                        End If
                    Next j
                    If Not lFound Then
                        lNPDESSites.Remove(i)
                        lSubbasins.Remove(i)
                        lMipts.Remove(i)
                    Else
                        i += 1
                    End If
                Loop
            End If
        End If

        'write first part of point source file
        lSB.AppendLine(" " & lNPDESSites.Count.ToString)
        lSB.AppendLine(" ")
        lSB.AppendLine("FacilityName Npdes Cuseg Mi")
        Dim lStr As String
        For lNpdesSiteIndex As Integer = 1 To lNPDESSites.Count
            lStr = Chr(34) & lFacNames(lNpdesSiteIndex) & Chr(34) & " " & lNPDESSites(lNpdesSiteIndex) & " " & lSubbasins(lNpdesSiteIndex) & " " & Format(lMipts(lNpdesSiteIndex), "0.000000")
            lSB.AppendLine(lStr)
        Next lNpdesSiteIndex

        Dim lParmCode(0) As String, lParmName(0) As String
        If Not aChkCustom Then 'read in Permitted Discharges Parameter Table
            If lNPDESSites.Count > 0 Then 'open dbf file
                Dim lDbf As IatcTable = atcUtility.atcTableOpener.OpenAnyTable(PathNameOnly(GisUtil.LayerFileName(lPcsLayerIndex)) & "\pcs3_prm.dbf")
                lRowCount = lDbf.NumRecords
                ReDim lParmCode(lRowCount)
                ReDim lParmName(lRowCount)
                For i As Integer = 1 To lRowCount
                    lDbf.CurrentRecord = i
                    lParmCode(i) = lDbf.Value(1)
                    lParmName(i) = lDbf.Value(2)
                Next i
            End If
        End If

        lSB.AppendLine(" ")
        lSB.AppendLine("OrdinalNumber Pollutant Load(lbs/hr)")
        Dim lValue As String
        If Not aChkCustom Then  'using pcs data
            Dim lPrevDbf As String = ""
            Dim lTableYear(0) As String
            Dim lTableParm(0) As String
            Dim lTableLoad(0) As String
            Dim lTableNPDES(0) As String
            For lNpdesSiteIndex As Integer = 1 To lNPDESSites.Count
                'open dbf file
                Dim lDbfFileName As String = lDbName & lHucs(lNpdesSiteIndex).ToString.Trim & ".dbf"
                Dim lDbfRowCount As Long
                If IO.File.Exists(lDbfFileName) Then
                    If lDbfFileName <> lPrevDbf Then
                        Dim lDbf As IatcTable = atcUtility.atcTableOpener.OpenAnyTable(lDbfFileName)
                        lPrevDbf = lDbfFileName
                        Dim lYearField As Integer, lParmField As Integer
                        Dim lLoadField As Integer, lNPDESField As Integer
                        For lFieldIndex As Integer = 1 To lDbf.NumFields
                            If lDbf.FieldName(lFieldIndex).ToUpper = "YEAR" Then
                                lYearField = lFieldIndex
                            End If
                            If lDbf.FieldName(lFieldIndex).ToUpper = "PARM" Then
                                lParmField = lFieldIndex
                            End If
                            If lDbf.FieldName(lFieldIndex).ToUpper = "LOAD" Then
                                lLoadField = lFieldIndex
                            End If
                            If lDbf.FieldName(lFieldIndex).ToUpper = "NPDES" Then
                                lNPDESField = lFieldIndex
                            End If
                        Next lFieldIndex
                        lDbfRowCount = lDbf.NumRecords
                        ReDim lTableYear(lDbfRowCount)
                        ReDim lTableParm(lDbfRowCount)
                        ReDim lTableLoad(lDbfRowCount)
                        ReDim lTableNPDES(lDbfRowCount)
                        For lRowIndex As Integer = 1 To lDbfRowCount
                            lDbf.CurrentRecord = lRowIndex
                            lTableYear(lRowIndex) = lDbf.Value(lYearField)
                            lTableParm(lRowIndex) = lDbf.Value(lParmField)
                            lTableLoad(lRowIndex) = lDbf.Value(lLoadField)
                            lTableNPDES(lRowIndex) = lDbf.Value(lNPDESField)
                        Next lRowIndex
                    End If
                    For lDbfRowIndex As Integer = 1 To lDbfRowCount
                        If lTableNPDES(lDbfRowIndex) = lNPDESSites(lNpdesSiteIndex) And lTableYear(lDbfRowIndex) = aYear Then
                            'found one, output it
                            Dim lPollutantName As String = ""
                            For lRowIndex As Integer = 0 To lRowCount - 1
                                If lTableParm(lDbfRowIndex) = lParmCode(lRowIndex) Then
                                    lPollutantName = lParmName(lRowIndex)
                                    Exit For
                                End If
                            Next lRowIndex
                            lValue = lTableLoad(lDbfRowIndex) / 8760 'lbs/hr
                            lStr = CStr(lNpdesSiteIndex - 1) & " " & Chr(34) & Trim(lPollutantName) & Chr(34) & " " & Format(CSng(lValue), "0.000000")
                            lSB.AppendLine(lStr)
                        End If
                    Next lDbfRowIndex
                End If
            Next lNpdesSiteIndex
            'now output flows
            For lNpdesSiteIndex As Integer = 1 To lNPDESSites.Count
                lStr = CStr(lNpdesSiteIndex - 1) & " Flow " & Format(lFlows(lNpdesSiteIndex), "0.000000")
                lSB.AppendLine(lStr)
            Next lNpdesSiteIndex
        Else 'using custom data
            Dim lDbf As IatcTable = atcUtility.atcTableOpener.OpenAnyTable(aLblCustom)
            For i As Integer = 1 To lNPDESSites.Count
                For j As Integer = 1 To lDbf.NumRecords
                    lDbf.CurrentRecord = j
                    If lNPDESSites(i) = lDbf.Value(1) Then
                        If lDbf.Value(4).ToUpper = "FLOW" Then
                            lStr = CStr(i - 1) & " Flow " & Format(CStr(lDbf.Value(3)), "0.000000")
                        Else
                            lValue = CSng(lDbf.Value(3)) / 8760 'lbs/hr
                            lStr = CStr(i - 1) & " " & Chr(34) & Trim(lDbf.Value(4)) & Chr(34) & " " & Format(CStr(lValue), "0.000000")
                        End If
                        lSB.AppendLine(lStr)
                    End If
                Next j
            Next i
        End If

        SaveFileString(aPsrFileName, lSB.ToString)
    End Sub

    Friend Sub WriteSEGFile(ByVal aSegFileName As String, _
                            ByVal aMetSegIds As atcCollection, _
                            ByVal aMetIndices As atcCollection, _
                            ByVal aMetBaseDsns As atcCollection)
        Dim lSB As New StringBuilder
        lSB.AppendLine("SegID" & vbTab & _
                       "PrecWdmId PrecDsn PrecTstype PrecMFactPI PrecMFactR " & _
                       "AtemWdmId AtemDsn AtemTstype AtemMFactPI AtemMFactR " & _
                       "DewpWdmId DewpDsn DewpTstype DewpMFactPI DewpMFactR " & _
                       "WindWdmId WindDsn WindTstype WindMFactPI WindMFactR " & _
                       "SolrWdmId SolrDsn SolrTstype SolrMFactPI SolrMFactR " & _
                       "ClouWdmId ClouDsn ClouTstype ClouMFactPI ClouMFactR " & _
                       "PevtWdmId PevtDsn PevtTstype PevtMFactPI PevtMFactR")
        For lIndex As Integer = 0 To aMetIndices.Count - 1
            Dim lBaseDsn As Integer = aMetBaseDsns(aMetIndices(lIndex))
            lSB.AppendLine(CStr(aMetSegIds(lIndex)) & " WDM2 " & lBaseDsn.ToString & " PREC 1 1" & _
                                                      " WDM2 " & (lBaseDsn + 2).ToString & " ATEM 1 1" & _
                                                      " WDM2 " & (lBaseDsn + 6).ToString & " DEWP 1 1" & _
                                                      " WDM2 " & (lBaseDsn + 3).ToString & " WIND 1 1" & _
                                                      " WDM2 " & (lBaseDsn + 4).ToString & " SOLR 1 1" & _
                                                      " WDM2 " & (lBaseDsn + 7).ToString & " CLOU 0 1" & _
                                                      " WDM2 " & (lBaseDsn + 5).ToString & " PEVT 1 1")
        Next
        SaveFileString(aSegFileName, lSB.ToString)
    End Sub

    Friend Sub WriteMAPFile(ByVal aMapFileName As String)
        Dim lSB As New StringBuilder
        lSB.AppendLine("EXT " & GisUtil.MapExtentXmin & _
                          " " & GisUtil.MapExtentYmax & _
                          " " & GisUtil.MapExtentXmax & _
                          " " & GisUtil.MapExtentYmin)
        For lLayerIndex As Integer = 0 To GisUtil.NumLayers - 1
            If GisUtil.LayerType(lLayerIndex) = 1 Or _
               GisUtil.LayerType(lLayerIndex) = 2 Or _
               GisUtil.LayerType(lLayerIndex) = 3 Then
                'shapefile
                Dim lTemp As String = "LYR '" + GisUtil.LayerFileName(lLayerIndex) & "', " & GisUtil.LayerColor(lLayerIndex)
                If GisUtil.LayerType(lLayerIndex) = 3 Then
                    'polygon 
                    If Not GisUtil.LayerTransparent(lLayerIndex) Then
                        lTemp &= ",Style Transparent "
                    End If
                    lTemp &= ",Outline " & GisUtil.LayerOutlineColor(lLayerIndex)
                End If
                'hide the layers not turned on
                If Not GisUtil.LayerVisible(lLayerIndex) Then
                    lTemp &= ",Hide"
                End If
                'add theme name as caption
                lTemp &= ",Name '" & GisUtil.LayerName(lLayerIndex) & "'"
                lSB.AppendLine(lTemp)
            End If
        Next lLayerIndex
        SaveFileString(aMapFileName, lSB.ToString)
    End Sub

    Private Sub SetPerviousGrid(ByVal aLandUseTableName As String)
        If AtcGridPervious.Source Is Nothing Then Exit Sub

        AtcGridPervious.Clear()
        With AtcGridPervious.Source
            .Rows = 1
            .Columns = 5
            .CellValue(0, 0) = "Code"
            .CellValue(0, 1) = "Group Description"
            .CellValue(0, 2) = "Impervious Percent"
            .CellValue(0, 3) = "Multiplier"
            .CellValue(0, 4) = "Subbasin"
            .ColorCells = True
            .FixedRows = 1
            .FixedColumns = 2
        End With

        'giras, nlcd, or other with reclass file set
        Dim lReclassTable As IatcTable = atcUtility.atcTableOpener.OpenAnyTable(aLandUseTableName)
        'do pre-scan to set up grid
        Dim lPrevCode As Integer = -1
        Dim lShowMults As Boolean = False
        Dim lShowCodes As Boolean = False
        Dim lGroupNames As New Collection
        Dim lGroupPercent As New Collection  'dont want to use atccollection because we may want to add mult times
        Dim lGroupIndex As Integer
        For lRecordIndex As Integer = 1 To lReclassTable.NumRecords
            'scan to see if multiple records for the same code
            lReclassTable.CurrentRecord = lRecordIndex
            Dim lCode As Long = lReclassTable.Value(1)
            If lCode = lPrevCode Then
                lShowMults = True
            End If
            lPrevCode = lCode
            'scan to see if perv percent varies within a group
            Dim lInCollection As Boolean = False
            For lGroupIndex = 1 To lGroupNames.Count
                If lGroupNames(lGroupIndex) = lReclassTable.Value(2) Then
                    lInCollection = True
                    If lGroupPercent(lGroupIndex) <> lReclassTable.Value(3) Then
                        lShowCodes = True
                    End If
                    Exit For
                End If
            Next lGroupIndex

            If Not lInCollection Then
                lGroupNames.Add(lReclassTable.Value(2))
                lGroupPercent.Add(lReclassTable.Value(3))
            End If
        Next lRecordIndex

        If lShowMults Then
            lShowCodes = True
        End If

        'sort list items
        Dim llReclassTableSorted As New atcCollection
        For lRecordIndex As Integer = 1 To lReclassTable.NumRecords
            lReclassTable.CurrentRecord = lRecordIndex
            llReclassTableSorted.Add(lRecordIndex, lReclassTable.Value(1))
        Next lRecordIndex
        llReclassTableSorted.SortByValue()

        'now populate grid
        With AtcGridPervious.Source
            For Each lRow As Integer In llReclassTableSorted.Keys
                lReclassTable.CurrentRecord = lRow
                If Not lShowCodes Then
                    'just show group desc and percent perv
                    Dim lInCollection As Boolean = False
                    For lRowIndex As Integer = 1 To .Rows
                        If .CellValue(lRowIndex - 1, 1) = lReclassTable.Value(2) Then
                            lInCollection = True
                        End If
                    Next
                    If Not lInCollection Then
                        .Rows += 1
                        .CellValue(.Rows - 1, 1) = lReclassTable.Value(2)
                        .CellValue(.Rows - 1, 2) = lReclassTable.Value(3)
                        .CellEditable(.Rows - 1, 2) = True
                        ' .CellColor(.Rows - 1, 1) = Me.BackColor
                    End If
                Else 'need to show whole table
                    If lReclassTable.Value(1) > 0 Then
                        .Rows += 1
                        .CellValue(.Rows - 1, 0) = lReclassTable.Value(1)
                        .CellValue(.Rows - 1, 1) = lReclassTable.Value(2)
                        .CellValue(.Rows - 1, 2) = lReclassTable.Value(3)
                        .CellValue(.Rows - 1, 3) = lReclassTable.Value(4)
                        .CellValue(.Rows - 1, 4) = lReclassTable.Value(5)
                    End If
                End If
            Next
        End With

        AtcGridPervious.SizeAllColumnsToContents()
        If lShowMults Then
            lShowCodes = True
        Else
            AtcGridPervious.ColumnWidth(3) = 0
            AtcGridPervious.ColumnWidth(4) = 0
        End If
        If Not lShowCodes Then
            AtcGridPervious.ColumnWidth(0) = 0
        End If

        With AtcGridPervious.Source
            .CellColor(0, 0) = SystemColors.ControlDark
            .CellColor(0, 1) = SystemColors.ControlDark
            .CellColor(0, 2) = SystemColors.ControlDark
            .CellColor(0, 3) = SystemColors.ControlDark
            .CellColor(0, 4) = SystemColors.ControlDark
            For lRowIndex As Integer = 1 To .Rows - 1
                .CellEditable(lRowIndex, 2) = True
                .CellEditable(lRowIndex, 3) = True
                .CellEditable(lRowIndex, 4) = True
                .CellColor(lRowIndex, 0) = SystemColors.ControlDark
                .CellColor(lRowIndex, 1) = SystemColors.ControlDark
            Next lRowIndex
        End With
        AtcGridPervious.Refresh()
    End Sub

    Friend Function ReclassifyLandUses(ByVal aReclassifyFile As String, _
                                       ByVal aGridPervious As atcControls.atcGrid, _
                                       ByVal aLandUses As LandUses) As LandUses
        Dim lReclassifyLandUses As New LandUses
        'if simple reclassifyfile exists, read it in
        Dim lRcode As New atcCollection
        Dim lUseSimpleGrid As Boolean = False
        If aReclassifyFile.Length > 0 And aGridPervious.ColumnWidth(0) = 0 Then
            'have the simple percent pervious grid, need to know which 
            'lucodes correspond to which lugroups
            lUseSimpleGrid = True
            'open dbf file
            Dim lTable As IatcTable = atcTableOpener.OpenAnyTable(aReclassifyFile)
            For lTableRecordIndex As Integer = 1 To lTable.NumRecords
                lTable.CurrentRecord = lTableRecordIndex
                lRcode.Add(lTable.Value(1), lTable.Value(2))
            Next lTableRecordIndex
        End If
        Logger.Dbg("ReclassifyFile:'" & aReclassifyFile & "' Count" & lRcode.Count)

        'create summary array 
        '  area of each land use group in each subbasin

        'build collection of unique subbasin ids
        Dim lUniqueSubids As New atcCollection
        For Each lLandUse As LandUse In aLandUses
            lUniqueSubids.Add(lLandUse.ModelID)
        Next
        Logger.Dbg("LandUseCount:" & aLandUses.Count & " UniqueSubidCount:" & lUniqueSubids.Count)

        'build collection of unique landuse groups
        Dim lUniqueLugroups As New atcCollection
        For lRowIndex As Integer = 1 To aGridPervious.Source.Rows
            lUniqueLugroups.Add(aGridPervious.Source.CellValue(lRowIndex, 1))
        Next lRowIndex
        Logger.Dbg("GridRowCount:" & aGridPervious.Source.Rows & " UniqueLugroupCount:" & lUniqueLugroups.Count)

        Dim lPerArea(lUniqueSubids.Count, lUniqueLugroups.Count) As Double
        Dim lImpArea(lUniqueSubids.Count, lUniqueLugroups.Count) As Double
        Dim lLength(lUniqueSubids.Count) As Double
        Dim lSlope(lUniqueSubids.Count) As Single

        'loop through each polygon (or grid subid/lucode combination)
        'and populate array with area values
        If lUseSimpleGrid Then
            For Each lLandUse As LandUse In aLandUses
                'find subbasin position in the area array
                Dim spos As Integer
                For j As Integer = 0 To lUniqueSubids.Count - 1
                    If lLandUse.ModelID = lUniqueSubids(j) Then
                        spos = j
                        Exit For
                    End If
                Next j
                'find lugroup that corresponds to this lucode
                Dim lLandUseName As String = lRcode.ItemByKey(lLandUse.Code.ToString)
                If lLandUseName IsNot Nothing Then
                    'find percent perv that corresponds to this lugroup
                    Dim lPercentImperv As Double
                    For lRowIndex As Integer = 1 To aGridPervious.Source.Rows
                        If lLandUseName = aGridPervious.Source.CellValue(lRowIndex, 1) Then
                            If Double.TryParse(aGridPervious.Source.CellValue(lRowIndex, 2), lPercentImperv) Then
                                Exit For
                            Else
                                Logger.Dbg("Warning: non-parsable percent impervious value at row " & lRowIndex & " '" & aGridPervious.Source.CellValue(lRowIndex, 2) & "' for land use name " & lLandUseName)
                            End If
                        End If
                    Next lRowIndex
                    'find lugroup position in the area array
                    Dim lpos As Long
                    For j As Integer = 0 To lUniqueLugroups.Count - 1
                        If lLandUseName = lUniqueLugroups(j) Then
                            lpos = j
                            Exit For
                        End If
                    Next j

                    With lLandUse
                        lPerArea(spos, lpos) += (.Area * (100 - lPercentImperv) / 100)
                        lImpArea(spos, lpos) += (.Area * lPercentImperv / 100)
                        lLength(spos) = 0.0
                        lSlope(spos) = .Slope / 100.0
                    End With
                End If
            Next lLandUse
        Else 'using custom table for landuse classification
            For Each lLandUse As LandUse In aLandUses
                'loop through each polygon (or grid subid/lucode combination)
                'find subbasin position in the area array
                Dim spos As Integer
                For j As Integer = 1 To lUniqueSubids.Count
                    If lLandUse.ModelID = lUniqueSubids(j - 1) Then
                        spos = j - 1
                        Exit For
                    End If
                Next j

                'find lugroup that corresponds to this lucode, could be multiple matches
                For lSourceRowIndex As Integer = 1 To aGridPervious.Source.Rows
                    Dim lLandUseName As String = ""
                    Dim lpos As Integer = -1
                    Dim lPercentImperv As Double
                    If aGridPervious.Source.CellValue(lSourceRowIndex, 0) <> "" Then
                        If lLandUse.Code = aGridPervious.Source.CellValue(lSourceRowIndex, 0) Then
                            'see if any of these are subbasin-specific
                            If Not Double.TryParse(aGridPervious.Source.CellValue(lSourceRowIndex, 2), lPercentImperv) Then
                                Logger.Dbg("Warning: non-parsable percent impervious value at row " & lSourceRowIndex & " '" & aGridPervious.Source.CellValue(lSourceRowIndex, 2) & "' for land use code " & lLandUse.Code)
                            Else
                                Dim lMultiplier As Double
                                If Not Double.TryParse(aGridPervious.Source.CellValue(lSourceRowIndex, 3), lMultiplier) Then
                                    lMultiplier = 1.0
                                End If
                                Dim lSubbasin As String = aGridPervious.Source.CellValue(lSourceRowIndex, 4)
                                Dim lSubbasinSpecific As Boolean = False
                                If Not lSubbasin Is Nothing Then
                                    If lSubbasin.Length > 0 And lSubbasin <> "Invalid Field Number" Then
                                        lSubbasinSpecific = True
                                    End If
                                End If
                                If lSubbasinSpecific Then
                                    'this row is subbasin-specific
                                    If lSubbasin = lLandUse.ModelID Then
                                        lLandUseName = aGridPervious.Source.CellValue(lSourceRowIndex, 1)
                                    End If
                                Else
                                    'make sure that no other rows of this lucode are 
                                    'subbasin-specific for this subbasin and that we 
                                    'should therefore not use this row
                                    Dim lUseIt As Boolean = True
                                    For k As Integer = 1 To aGridPervious.Source.Rows
                                        If k <> lSourceRowIndex Then
                                            If aGridPervious.Source.CellValue(k, 0) = aGridPervious.Source.CellValue(lSourceRowIndex, 0) Then
                                                'this other row has same lucode
                                                If aGridPervious.Source.CellValue(k, 1) = aGridPervious.Source.CellValue(lSourceRowIndex, 1) Then
                                                    'and the same group name
                                                    lSubbasin = aGridPervious.Source.CellValue(k, 4)
                                                    If lSubbasin IsNot Nothing AndAlso IsNumeric(lSubbasin) Then
                                                        'and its subbasin-specific
                                                        If lSubbasin = lLandUse.ModelID Then
                                                            'and its specific to this subbasin
                                                            lUseIt = False
                                                        End If
                                                    End If
                                                End If
                                            End If
                                        End If
                                    Next k
                                    If lUseIt Then 'we want this one now
                                        lLandUseName = aGridPervious.Source.CellValue(lSourceRowIndex, 1)
                                    End If
                                End If

                                If lLandUseName.Length > 0 Then 'find lugroup position in the area array
                                    For k As Integer = 1 To lUniqueLugroups.Count
                                        If lLandUseName = lUniqueLugroups(k - 1) Then
                                            lpos = k - 1
                                            Exit For
                                        End If
                                    Next k
                                End If

                                If lpos > -1 Then
                                    With lLandUse
                                        lPerArea(spos, lpos) += (.Area * lMultiplier * (100 - lPercentImperv) / 100)
                                        lImpArea(spos, lpos) += (.Area * lMultiplier * lPercentImperv / 100)
                                        lLength(spos) = 0.0 'were not computing lsur since winhspf does that
                                        lSlope(spos) = .Slope / 100.0
                                    End With
                                End If
                            End If
                        End If
                    End If
                Next lSourceRowIndex
            Next lLandUse
        End If

        For spos As Integer = 0 To lUniqueSubids.Count - 1
            For lpos As Integer = 0 To lUniqueLugroups.Count - 1
                Dim lArea As Double = lPerArea(spos, lpos) + lImpArea(spos, lpos)
                If lArea > 0 Then
                    Dim lLandUse As New LandUse
                    With lLandUse
                        .Description = lUniqueLugroups(lpos)
                        .Distance = lLength(spos)
                        .Slope = lSlope(spos)
                        .Area = lArea
                        .Code = lpos
                        .ImperviousFraction = lImpArea(spos, lpos) / .Area
                        .ModelID = lUniqueSubids(spos)
                        For Each lOrigLandUse As LandUse In aLandUses
                            If lOrigLandUse.ModelID = .ModelID Then
                                .Reach = lOrigLandUse.Reach
                                Exit For
                            End If
                        Next
                        .Type = "COMPOSITE"
                    End With
                    lReclassifyLandUses.Add(lLandUse)
                End If
            Next
        Next

        Return lReclassifyLandUses
    End Function

    Private Sub SetMetSegmentGrid(ByVal aSubbasinThemeName As String, ByVal aSubbasinFieldName As String, Optional ByVal aModelSegmentFieldName As String = "")

        If AtcGridMet.Source Is Nothing Then Exit Sub

        'If cboSubbasins.SelectedIndex = -1 Or cboSub1.SelectedIndex = -1 Then Exit Sub

        'Dim lSubbasinsLayerName As String = cboSubbasins.Items(cboSubbasins.SelectedIndex)
        Dim lSubbasinsLayerIndex As Integer = GisUtil.LayerIndex(aSubbasinThemeName)

        'Dim lSubbasinsFieldName As String = cboSub1.Items(cboSub1.SelectedIndex)
        Dim lSubbasinsFieldIndex As Integer = GisUtil.FieldIndex(lSubbasinsLayerIndex, aSubbasinFieldName)

        pUniqueModelSegmentNames = New atcCollection
        pUniqueModelSegmentIds = New atcCollection
        Dim lUniqueModelSegmentIntegerIds As New atcCollection
        'If cboSub3.SelectedIndex > 0 Then
        If aModelSegmentFieldName.Length > 0 Then
            'see if we have some model segments in the subbasin dbf
            'Dim lModelSegmentFieldName As String = cboSub3.Items(cboSub3.SelectedIndex)
            Dim lModelSegmentFieldIndex As Integer = GisUtil.FieldIndex(lSubbasinsLayerIndex, aModelSegmentFieldName)
            Dim lIsInteger As Boolean = True
            For lIndex As Integer = 1 To GisUtil.NumFeatures(lSubbasinsLayerIndex)
                Dim lModelSegment As String = GisUtil.FieldValue(lSubbasinsLayerIndex, lIndex - 1, lModelSegmentFieldIndex)
                If pUniqueModelSegmentNames.IndexFromKey(lModelSegment) = -1 Then
                    pUniqueModelSegmentNames.Add(lModelSegment)
                    lUniqueModelSegmentIntegerIds.Add(lUniqueModelSegmentIntegerIds.Count + 1)
                    If IsInteger(lModelSegment) Then
                        If Int(lModelSegment) > 0 Then
                            pUniqueModelSegmentIds.Add(lModelSegment)   'can use this as the integer model segment id
                        Else
                            lIsInteger = False
                        End If
                    Else
                        lIsInteger = False
                    End If
                End If
            Next
            If Not lIsInteger Then
                'one or more segment names are not valid integers
                pUniqueModelSegmentIds = lUniqueModelSegmentIntegerIds
            End If
        End If

        If pUniqueModelSegmentIds.Count = 0 Then
            'lstMet.Visible = True
            AtcGridMet.Visible = False
        Else
            'lstMet.Visible = False
            AtcGridMet.Visible = True
            AtcGridMet.Clear()
            With AtcGridMet.Source
                .Columns = 2
                .ColorCells = True
                .FixedRows = 1
                .FixedColumns = 1
                .CellColor(0, 0) = SystemColors.ControlDark
                .CellColor(0, 1) = SystemColors.ControlDark
                .Rows = 1 + pUniqueModelSegmentNames.Count
                .CellValue(0, 0) = "Model Segment"
                .CellValue(0, 1) = "Met Station"
                For lIndex As Integer = 1 To pUniqueModelSegmentNames.Count
                    .CellValue(lIndex, 0) = pUniqueModelSegmentNames(lIndex - 1)
                    .CellColor(lIndex, 0) = SystemColors.ControlDark
                    If pMetStations.Count > 0 Then
                        'try to match segment/station names
                        Dim i As Integer = 0
                        Dim lSegName As String = pUniqueModelSegmentNames(lIndex - 1).ToString
                        Dim lStaName As String
                        While i < pMetStations.Count
                            lStaName = pMetStations(i).ToString
                            If lStaName.StartsWith("*") Then lStaName = lStaName.Substring(1)
                            If lStaName.Length > 7 AndAlso lSegName.Length > 7 Then
                                If lStaName.Substring(0, 8) = lSegName.Substring(0, 8) Then
                                    .CellValue(lIndex, 1) = pMetStations(i)
                                    i = pMetStations.Count
                                End If
                            End If
                            i += 1
                        End While
                        If i = pMetStations.Count Then 'didn't find a match, just use 1st station
                            .CellValue(lIndex, 1) = pMetStations(0)
                        End If
                        .CellEditable(lIndex, 1) = True
                    End If
                Next
            End With
            AtcGridMet.ValidValues = pMetStations
            AtcGridMet.SizeAllColumnsToContents()
            AtcGridMet.Refresh()
        End If

    End Sub

    Friend Sub BuildListofMetStationNames(ByRef aMetWDMName As String, _
                                          ByVal aMetStations As atcCollection, _
                                          ByVal aMetBaseDsns As atcCollection)
        aMetStations.Clear()
        aMetBaseDsns.Clear()
        Dim lDataSource As New atcWDM.atcDataSourceWDM
        If IO.File.Exists(aMetWDMName) Then
            Dim lFound As Boolean = False
            For Each lBASINSDataSource As atcTimeseriesSource In atcDataManager.DataSources
                If lBASINSDataSource.Specification.ToUpper = aMetWDMName.ToUpper Then
                    'found it in the BASINS data sources
                    lDataSource = lBASINSDataSource
                    lFound = True
                    Exit For
                End If
            Next

            If Not lFound Then 'need to open it here
                If lDataSource.Open(aMetWDMName) Then
                    lFound = True
                End If
            End If

            If lFound Then
                Dim lCounter As Integer = 0
                For Each lDataSet As atcTimeseries In lDataSource.DataSets
                    lCounter += 1
                    Logger.Progress(lCounter, lDataSource.DataSets.Count)

                    If (lDataSet.Attributes.GetValue("Scenario") = "OBSERVED" Or lDataSet.Attributes.GetValue("Scenario") = "COMPUTED") _
                        And lDataSet.Attributes.GetValue("Constituent") = "PREC" Then
                        Dim lLoc As String = lDataSet.Attributes.GetValue("Location")
                        Dim lStanam As String = lDataSet.Attributes.GetValue("Stanam")
                        Dim lDsn As Integer = lDataSet.Attributes.GetValue("Id")
                        'get the common dates from prec and pevt at this location
                        Dim lSJDay As Double = lDataSet.Dates.Value(0)
                        Dim lEJDay As Double = lDataSet.Dates.Value(lDataSet.Dates.numValues)
                        'find pevt dataset at the same location
                        '*** TEMPORARY CHANGE - Add met location even if PEVT not found
                        Dim lAddIt As Boolean = True 'False
                        For Each lDataSet2 As atcData.atcTimeseries In lDataSource.DataSets
                            If lDataSet2.Attributes.GetValue("Constituent") = "PEVT" And _
                               lDataSet2.Attributes.GetValue("Location") = lLoc Then
                                Dim lSJDay2 As Double = lDataSet2.Dates.Value(0)
                                Dim lEJDay2 As Double = lDataSet2.Dates.Value(lDataSet2.Dates.numValues)
                                If lSJDay2 > lSJDay Then lSJDay = lSJDay2
                                If lEJDay2 < lEJDay Then lEJDay = lEJDay2
                                lAddIt = True
                                Exit For
                            End If
                        Next
                        'if this one is computed and observed also exists at same location, just use observed
                        If lDataSet.Attributes.GetValue("Scenario") = "COMPUTED" Then
                            For Each lDataSet2 As atcData.atcTimeseries In lDataSource.DataSets
                                If lDataSet2.Attributes.GetValue("Constituent") = "PREC" And _
                                   lDataSet2.Attributes.GetValue("Scenario") = "OBSERVED" And _
                                   lDataSet2.Attributes.GetValue("Location") = lLoc Then
                                    lAddIt = False
                                    Exit For
                                End If
                            Next
                        End If
                        If lAddIt Then
                            Dim lLeadingChar As String = ""
                            If IsBASINSMetWDM(lDataSource.DataSets, lDsn, lLoc) Then
                                'full set available here
                                lLeadingChar = "*"
                            End If
                            Dim lSdate(6) As Integer
                            Dim lEdate(6) As Integer
                            J2Date(lSJDay, lSdate)
                            J2Date(lEJDay, lEdate)
                            Dim lDateString As String = "(" & lSdate(0) & "/" & lSdate(1) & "/" & lSdate(2) & "-" & lEdate(0) & "/" & lEdate(1) & "/" & lEdate(2) & ")"
                            aMetStations.Add(lLeadingChar & lLoc & ":" & lStanam & " " & lDateString)
                            aMetBaseDsns.Add(lDsn)
                        End If
                    End If
                    'set valuesneedtoberead so that the dates and values will be forgotten, to free up memory
                    lDataSet.ValuesNeedToBeRead = True
                Next
            End If
        End If
    End Sub

    Private Function IsBASINSMetWDM(ByVal aDataSets As atcData.atcTimeseriesGroup, _
                                    ByVal aBaseDsn As Integer, _
                                    ByVal aLoc As String) As Boolean
        Dim lCheckCount As Integer = 0
        For Each lDataSet As atcTimeseries In aDataSets
            If lDataSet.Attributes.GetValue("Location") = aLoc Then
                If lDataSet.Attributes.GetValue("ID") = aBaseDsn Then
                    If lDataSet.Attributes.GetValue("TSTYPE") = "PREC" Then
                        lCheckCount += 1
                    End If
                ElseIf lDataSet.Attributes.GetValue("ID") = aBaseDsn + 2 Then
                    If lDataSet.Attributes.GetValue("TSTYPE") = "ATEM" Then
                        lCheckCount += 1
                    End If
                ElseIf lDataSet.Attributes.GetValue("ID") = aBaseDsn + 3 Then
                    If lDataSet.Attributes.GetValue("TSTYPE") = "WIND" Then
                        lCheckCount += 1
                    End If
                ElseIf lDataSet.Attributes.GetValue("ID") = aBaseDsn + 4 Then
                    If lDataSet.Attributes.GetValue("TSTYPE") = "SOLR" Then
                        lCheckCount += 1
                    End If
                ElseIf lDataSet.Attributes.GetValue("ID") = aBaseDsn + 5 Then
                    If lDataSet.Attributes.GetValue("TSTYPE") = "PEVT" Then
                        lCheckCount += 1
                    End If
                ElseIf lDataSet.Attributes.GetValue("ID") = aBaseDsn + 6 Then
                    If lDataSet.Attributes.GetValue("TSTYPE") = "DEWP" Then
                        lCheckCount += 1
                    End If
                ElseIf lDataSet.Attributes.GetValue("ID") = aBaseDsn + 7 Then
                    If lDataSet.Attributes.GetValue("TSTYPE") = "CLOU" Then
                        lCheckCount += 1
                    End If
                End If
            End If
        Next

        If lCheckCount = 7 Then 'needed datasets found
            Return True
        Else
            Return False
        End If
    End Function

    Private Function PreProcessChecking(ByVal OutputPath As String, ByVal BaseOutputName As String, _
                                        ByVal aLUType As Integer, ByVal aSubbasinThemeName As String, ByVal aLandUseThemename As String) As Boolean
        'If pModelName <> "AQUATOX" Then
        'If cboLanduse.Items(cboLanduse.SelectedIndex) = "USGS GIRAS Shapefile" Then
        If aLUType = 0 Then
            If GisUtil.LayerIndex("Land Use Index") = -1 Then
                'cant do giras without land use index layer
                Logger.Msg("When using GIRAS Landuse, the 'Land Use Index' layer must exist and be named as such.", vbOKOnly, "HSPF GIRAS Problem")
                'EnableControls(True)
                Return False
            End If
        End If

        'If cboLanduse.SelectedIndex <> 0 Then
        If aLUType <> 0 Then
            'not giras, make sure subbasins and land use layers aren't the same
            'If cboSubbasins.Items(cboSubbasins.SelectedIndex) = cboLandUseLayer.Items(cboLandUseLayer.SelectedIndex) Then
            If aSubbasinThemeName = aLandUseThemename Then
                'same layer cannot be used for both
                Logger.Msg("The same layer cannot be used for the subbasins layer and the landuse layer.", vbOKOnly, "BASINS HSPF Problem")
                'EnableControls(True)
                Return False
            End If
        End If

        If pMetStations.Count = 0 Then
            'cannot proceed if there are no met stations, need to specify a met wdm
            Logger.Msg("No met stations are available.  Use the 'Met Stations' tab to specify a WDM file with valid met stations.", vbOKOnly, "BASINS HSPF Problem")
            'EnableControls(True)
            Return False
        End If

        'see if these files already exist
        Dim lWsdFileName As String = OutputPath & "\" & BaseOutputName & ".wsd"
        If FileExists(lWsdFileName) Then  'already exists
            If Logger.Msg("HSPF Project '" & BaseOutputName & "' already exists.  Do you want to overwrite it?", vbOKCancel, "Overwrite?") = MsgBoxResult.Cancel Then
                'EnableControls(True)
                Return False
            End If
        End If
        'Else 'in AQUATOX, see if these files already exist
        '    Dim lRchFileName As String = OutputPath & "\" & BaseOutputName & ".rch"
        '    If FileExists(lRchFileName) Then 'already exists
        '        If Logger.Msg("AQUATOX Project '" & BaseOutputName & "' already exists.  Do you want to overwrite it?", vbOKCancel, "Overwrite?") = MsgBoxResult.Cancel Then
        '            EnableControls(True)
        '            Return False
        '        End If
        '    End If
        'End If
        Return True
    End Function

    Friend Function CreateUCI(ByVal aUciName As String, _
                              ByVal aMetWDMName As String) As Boolean
        ChDriveDir(PathNameOnly(aUciName))
        'get message file ready
        Dim lMsg As New atcUCI.HspfMsg("hspfmsg.mdb")

        'get starter uci ready
        Dim lBasinsBinLoc As String = PathNameOnly(System.Reflection.Assembly.GetEntryAssembly.Location)
        Dim lStarterUciName As String = "starter.uci"
        Dim lStarterPath As String = lBasinsBinLoc.Substring(0, lBasinsBinLoc.Length - 3) & "models\hspf\bin\starter\" & lStarterUciName
        If Not IO.File.Exists(lStarterPath) Then
            lStarterPath = "\basins\models\hspf\bin\starter\" & lStarterUciName
            If Not IO.File.Exists(lStarterPath) Then
                lStarterPath = FindFile("Please locate " & lStarterUciName, lStarterUciName)
            End If
        End If
        lStarterUciName = lStarterPath

        'location master pollutant list 
        Dim lPollutantListFileName As String = "poltnt_2.prn"
        Dim lPollutantListPath As String = lBasinsBinLoc.Substring(0, lBasinsBinLoc.Length - 3) & "models\hspf\bin\" & lPollutantListFileName
        If Not IO.File.Exists(lPollutantListPath) Then
            lPollutantListPath = "\basins\models\hspf\bin\" & lPollutantListFileName
            If Not FileExists(lPollutantListPath) Then
                lPollutantListPath = FindFile("Please locate " & lPollutantListFileName, lPollutantListFileName)
            End If
        End If
        lPollutantListFileName = lPollutantListPath

        'open project wdm
        Dim lDataSources As New Collection(Of atcData.atcTimeseriesSource)
        Dim lDataSource As New atcWDM.atcDataSourceWDM
        Dim lProjectWDMName As String = IO.Path.GetFileNameWithoutExtension(aUciName) & ".wdm"
        Dim lFound As Boolean = False
        For Each lBASINSDataSource As atcTimeseriesSource In atcDataManager.DataSources
            If lBASINSDataSource.Specification.ToUpper = lProjectWDMName.ToUpper Then
                'found it in the BASINS data sources
                lDataSource = lBASINSDataSource
                lFound = True
                Exit For
            End If
        Next

        If Not lFound Then 'need to open it here
            If lDataSource.Open(lProjectWDMName) Then
                lFound = True
            End If
        End If
        lDataSources.Add(lDataSource)

        'open met wdm
        lDataSource = New atcWDM.atcDataSourceWDM
        lFound = False
        For Each lBASINSDataSource As atcTimeseriesSource In atcDataManager.DataSources
            If lBASINSDataSource.Specification.ToUpper = aMetWDMName.ToUpper Then
                'found it in the BASINS data sources
                lDataSource = lBASINSDataSource
                lFound = True
                Exit For
            End If
        Next

        If Not lFound Then 'need to open it here
            If lDataSource.Open(aMetWDMName) Then
                lFound = True
            End If
        End If
        lDataSources.Add(lDataSource)

        ChDriveDir(PathNameOnly(aUciName))

        Dim lWatershedName As String = IO.Path.GetFileNameWithoutExtension(aUciName)
        Dim lWatershed As New Watershed
        Dim lCreateUCI As Boolean = False
        If lWatershed.Open(lWatershedName) = 0 Then  'everything read okay, continue
            Dim lHspfUci As New atcUCI.HspfUci
            lHspfUci.Msg = lMsg
            lHspfUci.CreateUciFromBASINS(lWatershed, _
                                         lDataSources, _
                                         lStarterUciName, lPollutantListFileName)
            lHspfUci.Save()
            lCreateUCI = True
        End If
        Return lCreateUCI
    End Function
End Module
