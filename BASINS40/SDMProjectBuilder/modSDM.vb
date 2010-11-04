Imports atcUtility
Imports atcMwGisUtility
Imports MapWinUtility
Imports MapWinUtility.Strings


Public Module modSDM
    Friend g_MapWin As MapWindow.Interfaces.IMapWin
    Friend g_AppNameRegistry As String = "FramesSDM" 'For preferences in registry
    Friend g_AppNameShort As String = "FramesSDM"
    Friend g_AppNameLong As String = "Frames SDM"

    Friend g_DoHSPF As Boolean = True
    Friend g_DoSWAT As Boolean = True
    Friend g_MinCatchmentKM2 As Double = 1.0 'Minimum catchment size
    Friend g_MinFlowlineKM As Double = 5.0 'Minimum flowline length

    Friend g_MapWinWindowHandle As Integer
    Friend g_ProgramDir As String = ""
    Friend g_ClipCatchments As Boolean = True

    Public g_KeepConnectingRemovedFlowLines As Boolean = True
    Friend pBuildFrm As frmBuildNew

    'Private g_Clip As String = "" 'if using old style clip = "Clip"
    'Private g_Project As String = "apes" '"0401" '
    Public g_CacheFolder As String '= "d:\Basins\Cache\" 'Downloaded data is kept here to avoid downloading the same thing again later
    Friend g_SWATDatabaseName As String = FindFile("", "SWAT2005.mdb")
    'Private g_PresetCatchments As String = "" '"G:\Project\APES-Kraemer\ms_30m_01\Watershed\Shapes"
    'Private g_UseNhdPlus As Boolean = False
    'Private g_NHDPlusInProjectFolder As Boolean = False
    'Private g_MultiThread As Boolean = False
    'Private g_UseCropDataLandUse As Boolean = False
    Public g_UseMgtCropFile As Boolean = False
    Public g_ParameterShapefileName As String = ""
    Public g_CreateArcSWATFiles As Boolean = False

    ''for debugging catchment/flowline aggregation
    'Public g_ShowCatchmentsFlowlines As Boolean = False 'True to save snapshots showing the simplified catchments and flowlines instead of continuing processing
    'Public g_MapWindowIndex As Integer = 0
    'Private g_BufferMeters As Double = 100

    'Simplification parameters: dissolve some HRUs and spread their area among other HRUs in the subbasin
    Private g_AreaIgnoreBelowFraction As Double = 0       'HRUs smaller than this fraction of their subbasin will be dissolved 
    Private g_AreaIgnoreBelowAbsolute As Double = 0       'HRUs smaller than this will be dissolved
    Friend g_LandUseIgnoreBelowFraction As Double = 0.07 'Land uses covering less than this much of a subbasin will have all their HRUs in that subbasin dissolved
    Friend g_LandUseIgnoreBelowAbsolute As Double = 0    'Absolute total area of a land use within a subbasin below which to dissolve its HRUs
    Friend g_SimulationStartYear As Integer = 1990
    Friend g_SimulationEndYear As Integer = 2000

    Private g_WhichSoilIndex As WhichSoilIndex = WhichSoilIndex.SWATSTATSGO

    Private g_GeoProcess As Boolean = True
    Private g_BuildDatabase As Boolean = True
    Private g_RunModel As Boolean = False
    Private g_OutputSummarize As Boolean = False

    Private pLayerFilenames() As String
    Private pResume As Boolean = False  'True to use existing full or partially complete overlay, False to do overlay from the start

    Private pLastPrivateMemory As Integer = 0
    Private pLastGcMemory As Integer = 0
    'Private pThreadMax As Integer = 2
    Private pThreadCount As Integer = 0

    'Const SDM_REGISTRY_KEY As String = "SOFTWARE\\US EPA\\D4EM\\SDMProjectBuilder"
    Friend Const PARAMETER_FILE As String = "SDMParameters.txt"

    Private _sdmBaseDirectory As String = ""
    Private _parametersFile As String = ""

    Private _projFolder As String = ""
    Private _defaultUnit As String = ""
    '    Private _swatSoilsDB As String = ""

    'private bool _defaultUnitChanged = false;
    'private bool _flagDefaultUnitChanged = false;


    Friend Sub WriteParametersTextFile(ByVal aFilename As String, ByVal aMapWindowProjectFilename As String)
        Dim sb As New Text.StringBuilder
        sb.AppendLine("ProjectsPath," & IO.Path.GetDirectoryName(IO.Path.GetDirectoryName(aMapWindowProjectFilename)))
        sb.AppendLine("DefaultUnit," & IO.Path.GetFileName(IO.Path.GetDirectoryName(aMapWindowProjectFilename)))
        sb.AppendLine("SWAT2005Database," & g_SWATDatabaseName)
        'sb.AppendLine("SWATSoilsDatabase," & _swatSoilsDB)
        sb.AppendLine("MinimumStreamLength," & g_MinFlowlineKM)
        sb.AppendLine("MinimumCatchmentArea," & g_MinCatchmentKM2)
        sb.AppendLine("MinumumLandUsePercent," & g_LandUseIgnoreBelowFraction * 100)
        sb.AppendLine("SimulationStartYear," & g_SimulationStartYear)
        sb.AppendLine("SimulationEndYear," & g_SimulationEndYear)
        sb.AppendLine("RunSWAT," & g_DoSWAT)
        sb.AppendLine("RunHSPF," & g_DoHSPF)
        IO.File.WriteAllText(aFilename, sb.ToString)
    End Sub

    Friend Sub ReadParametersTextFile(ByVal aFilename As String)
        If IO.File.Exists(aFilename) Then
            For Each line As String In LinesInFile(aFilename)
                If (line IsNot Nothing) AndAlso (line <> "") Then
                    Dim items() As String = line.Split(",")
                    If items.Length = 2 Then
                        Select Case items(0)
                            Case "ProjectsPath" : _projFolder = items(1)
                            Case "DefaultUnit" : _defaultUnit = items(1)
                            Case "SWAT2005Database" : g_SWATDatabaseName = items(1)
                                'Case "SWATSoilsDatabase" : _swatSoilsDB = items(1)
                            Case "MinimumStreamLength" : g_MinFlowlineKM = Convert.ToDouble(items(1))
                            Case "MinimumCatchmentArea" : g_MinCatchmentKM2 = Convert.ToDouble(items(1))
                            Case "MinumumLandUsePercent" : g_LandUseIgnoreBelowFraction = Convert.ToDouble(items(1)) / 100
                            Case "SimulationStartYear" : g_SimulationStartYear = Convert.ToInt32(items(1))
                            Case "SimulationEndYear" : g_SimulationEndYear = Convert.ToInt32(items(1))
                            Case "RunSWAT" : g_DoSWAT = Convert.ToBoolean(items(1))
                            Case "RunHSPF" : g_DoHSPF = Convert.ToBoolean(items(1))
                            Case Else
                                'Log something here?
                        End Select
                    Else
                        Logger.Dbg("Found " & items.Length & " but expected 2 comma-separated values in line '" & line & "' in file '" & aFilename & "'")
                    End If
                End If
            Next
        End If
    End Sub

    Friend Function BuildFormIsOpen() As Boolean
        If pBuildFrm Is Nothing Then
            Return False
        ElseIf pBuildFrm.IsDisposed Then
            pBuildFrm = Nothing
            Return False
        Else
            Return True
        End If
    End Function

    Friend Function DBFKeyFieldName(ByVal aDBFFileName As String) As String
        Dim lFileNameOnly As String = IO.Path.GetFileNameWithoutExtension(aDBFFileName).ToLower
        Select Case lFileNameOnly
            Case "cat", "huc", "huc250d3" : Return "CU"
            Case "huc12" : Return "HUC_12"
            Case "cnty" : Return "FIPS"
            Case "st" : Return "ST"
            Case Else
                If lFileNameOnly.StartsWith("wbdhu8") Then
                    Return "HUC_8"
                End If
        End Select
        Return ""
    End Function

    Friend Function DBFDescriptionFieldName(ByVal aDBFFileName As String) As String
        Dim lFileNameOnly As String = IO.Path.GetFileNameWithoutExtension(aDBFFileName).ToLower
        Select Case lFileNameOnly
            Case "cat", "huc", "huc250d3" : Return "catname"
            Case "huc12" : Return "HU_12_NAME"
            Case "cnty" : Return "cntyname"
            Case "st" : Return "ST"
            Case Else
                If lFileNameOnly.StartsWith("wbdhu8") Then
                    Return "SUBBASIN"
                End If
        End Select
        Return ""
    End Function

    Friend Function DBFThemeFieldName(ByVal aDBFFileName As String) As String
        Dim lFileNameOnly As String = IO.Path.GetFileNameWithoutExtension(aDBFFileName).ToLower
        Select Case lFileNameOnly
            Case "cat", "huc", "huc250d3" : Return "HUC8"
            Case "huc12" : Return "HUC12"
            Case "cnty" : Return "county_cd"
            Case "st" : Return "state_abbrev"
            Case Else
                If lFileNameOnly.StartsWith("wbdhu8") Then
                    Return "HUC8"
                End If
        End Select
        Return ""
    End Function

    Friend Sub UpdateSelectedFeatures()
        If BuildFormIsOpen() AndAlso g_MapWin.Layers.NumLayers > 0 AndAlso g_MapWin.Layers.CurrentLayer > -1 Then
            Dim lFieldName As String = ""
            Dim lFieldDesc As String = ""
            Dim lField As Integer
            Dim lNameIndex As Integer = -1
            Dim lDescIndex As Integer = -1
            Dim lCurLayer As MapWinGIS.Shapefile
            Dim ctext As String

            RefreshView()
            ctext = "Selected:" & vbCrLf & "  <none>"
            If g_MapWin.Layers.Item(g_MapWin.Layers.CurrentLayer).LayerType = MapWindow.Interfaces.eLayerType.PolygonShapefile Then
                lCurLayer = g_MapWin.Layers.Item(g_MapWin.Layers.CurrentLayer).GetObject
                If g_MapWin.View.SelectedShapes.NumSelected > 0 Then
                    ctext = "Selected:"

                    lFieldName = DBFKeyFieldName(lCurLayer.Filename).ToLower
                    lFieldDesc = DBFDescriptionFieldName(lCurLayer.Filename).ToLower

                    For lField = 0 To lCurLayer.NumFields - 1
                        If lCurLayer.Field(lField).Name.ToLower = lFieldName Then
                            lNameIndex = lField
                        End If
                        If lCurLayer.Field(lField).Name.ToLower = lFieldDesc Then
                            lDescIndex = lField
                        End If
                    Next

                    Dim lSelected As Integer
                    Dim lShape As Integer
                    Dim lName As String
                    Dim lDesc As String
                    Dim lSf As MapWinGIS.Shapefile = g_MapWin.Layers.Item(g_MapWin.Layers.CurrentLayer).GetObject
                    For lSelected = 0 To g_MapWin.View.SelectedShapes.NumSelected - 1
                        lShape = g_MapWin.View.SelectedShapes.Item(lSelected).ShapeIndex()
                        lName = ""
                        lDesc = ""
                        Dim lLoadingHUC12 As Boolean = False
                        If lNameIndex > -1 Then
                            lName = lSf.CellValue(lNameIndex, lShape)
                            If lName.Length = 8 AndAlso (lFieldName = "cu" OrElse lFieldName = "huc_8") AndAlso pBuildFrm.rdoHUC12.Checked Then
                                lLoadingHUC12 = True
                                LoadHUC12(lName) 'Make sure HUC-12 layer matching this HUC-8 layer is on the map
                            End If
                        End If
                        If Not lLoadingHUC12 Then
                            If lDescIndex > -1 Then
                                lDesc = lSf.CellValue(lDescIndex, lShape)
                            End If
                            If (lName & lDesc).Length = 0 Then
                                ctext &= vbCrLf & "  " & lShape
                            Else
                                ctext &= vbCrLf & "  " & lName & " : " & lDesc
                            End If
                        End If
                    Next
                End If
                pBuildFrm.txtSelected.Text = ctext
            End If
        End If
    End Sub

    ''' <summary>
    ''' Layer Handle of first 8-digit HUC layer found or -1 if no HUC-8 layer is on the map
    ''' </summary>
    Friend Function Huc8Layer() As Integer
        Dim lHuc8Layer As Integer = -1
        For iLayer As Integer = 0 To g_MapWin.Layers.NumLayers - 1
            Dim lLayerHandle As Integer = g_MapWin.Layers.GetHandle(iLayer)
            Dim lFileNameOnly As String = IO.Path.GetFileName(g_MapWin.Layers(lLayerHandle).FileName).ToLower
            Select Case lFileNameOnly
                Case "cat.shp", "huc250d3.shp", "wbdhu8.shp"
                    Return lLayerHandle
            End Select
        Next
        Return -1
    End Function

    ''' <summary>
    ''' Layer Handle of first 12-digit HUC layer found or -1 if no HUC-12 layer is on the map
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Friend Function Huc12Layer() As Integer
        For iLayer As Integer = 0 To g_MapWin.Layers.NumLayers - 1
            Dim lLayerHandle As Integer = g_MapWin.Layers.GetHandle(iLayer)
            If g_MapWin.Layers(lLayerHandle).FileName.ToLower.Contains("huc12") Then
                Return lLayerHandle
            End If
        Next
        Return -1
    End Function

    Friend Sub LoadHUC12(ByVal aHUC8 As String)
        Dim lHUC12ShapeFileName As String = HUC12ShapeFilename(aHUC8).ToLower
        If IO.File.Exists(lHUC12ShapeFileName) Then
            If LayerHandle(lHUC12ShapeFileName) >= 0 Then
                Exit Sub 'Layer is already on the map
            End If
        Else 'download HUC-12 layer
            EnsureGlobalCacheSet(IO.Path.GetDirectoryName(g_MapWin.Project.FileName))
            Dim lQuery As String = "<function name='GetBASINS'>" _
                                 & "<arguments>" _
                                 & "<DataType>huc12</DataType>" _
                                 & "<SaveIn>" & IO.Path.GetDirectoryName(lHUC12ShapeFileName) & "</SaveIn>" _
                                 & "<CacheFolder>" & g_CacheFolder & "</CacheFolder>" _
                                 & "<DesiredProjection>" & g_MapWin.Project.ProjectProjection & "</DesiredProjection>" _
                                 & "<region>" & vbCrLf _
                                 & "  <HUC8>" & aHUC8 & "</HUC8>" & vbCrLf _
                                 & "</region>" & vbCrLf _
                                 & "<clip>False</clip>" _
                                 & "<merge>True</merge>" _
                                 & "<joinattributes>true</joinattributes>" _
                                 & "</arguments>" _
                                 & "</function>"
            Dim lDownloadManager As D4EMDataManager.DataManager = CreateDataManager()
            Dim lResult As String = lDownloadManager.Execute(lQuery)
            'If Not lResult Is Nothing AndAlso lResult.Length > 0 AndAlso lResult.StartsWith("<success>") Then
            '    ProcessDownloadResults(lResult)
            'End If
            'Dim lCacheFilename As String = "S:\Scratch\Downloads\Data\HUC-12\split\" & aHUC8 & "\huc12.shp"
            'If IO.File.Exists(lCacheFilename) Then
            '    TryCopyShapefile(lCacheFilename, lHUC12ShapeFileName)
            'End If
        End If

        If IO.File.Exists(lHUC12ShapeFileName) Then
            ProcessDownloadResults("<success><add_shape>" & lHUC12ShapeFileName & "</add_shape></success>")
        End If
        Logger.Status("")
    End Sub

    Private Function HUC12ShapeFilename(ByVal aHUC8) As String
        Return IO.Path.GetDirectoryName(g_MapWin.Project.FileName) & g_PathChar & "huc12" & g_PathChar & aHUC8 & g_PathChar & "huc12.shp"
    End Function

    Friend Sub ClearLayers()
        g_MapWin.Layers.Clear()
    End Sub

    Friend Sub RefreshView()
        g_MapWin.Refresh()
    End Sub

    Friend Sub ProcessNetwork(ByVal aSelectedHuc As String, _
                              ByVal aSelectedShape As MapWinGIS.Shape, _
                              ByVal aNewDataDir As String, _
                              ByRef aSimplifiedFlowlinesFileName As String, _
                              ByRef aSimplifiedCatchmentsFileName As String)
        Dim lProblem As String = ""

        aSimplifiedFlowlinesFileName = ""
        aSimplifiedCatchmentsFileName = ""

        Dim lNHDPlusFolder As String = IO.Path.Combine(aNewDataDir, "nhdplus" & SafeSubstring(aSelectedHuc, 0, 8))

        Try
            Dim lCatchmentsToUseFilename As String
            Dim lFlowLinesToUseFilename As String

            lCatchmentsToUseFilename = lNHDPlusFolder & "\drainage\catchment.shp"
            lFlowLinesToUseFilename = lNHDPlusFolder & "\hydrography\nhdflowline.shp"

            Dim lFlowLinesShapeFilename As String = lFlowLinesToUseFilename
            Dim lCatchmentsShapeFilename As String = lCatchmentsToUseFilename
            lCatchmentsToUseFilename = lNHDPlusFolder & "\drainage\usecatchment.shp"
            lFlowLinesToUseFilename = lNHDPlusFolder & "\hydrography\usenhdflowline.shp"

            Dim lCatchments As New MapWinGIS.Shapefile
            If lCatchments.Open(lCatchmentsShapeFilename) Then
                Logger.Status("CatchmentCount before Clipping " & lCatchments.NumShapes)
                If g_ClipCatchments Then
                    ClipCatchments(lCatchmentsToUseFilename, lCatchments, aSelectedShape)
                Else
                    atcUtility.TryCopyShapefile(lCatchmentsShapeFilename, lCatchmentsToUseFilename)
                End If
            Else
                Logger.Status("Unable to open NHDPlus catchments in '" & lCatchmentsShapeFilename & "'")
            End If

            'check for channel with no contrib area - no associated catchment COMID, remove and report
            If ClipFlowLinesToCatchments(lCatchmentsToUseFilename, lFlowLinesShapeFilename, lFlowLinesToUseFilename) Then
                Logger.Status("Clipped Flowlines")
            Else
                Logger.Status("Unable to clip NHDPlus flowlines in '" & lFlowLinesShapeFilename & "'")
            End If

            Logger.Status("CombineShortOrBraidedFlowlines for " & aSelectedHuc & " with MinCatchment " & g_MinCatchmentKM2)
            CombineShortOrBraidedFlowlines(lFlowLinesToUseFilename, lCatchmentsToUseFilename, _
                                           g_MinCatchmentKM2, g_MinFlowlineKM)
            aSimplifiedFlowlinesFileName = lFlowLinesToUseFilename.Replace("flowline", "flowlineNoShort")
            aSimplifiedCatchmentsFileName = lCatchmentsToUseFilename.Replace("catchment", "catchmentNoShort")

            Logger.Status("DoneSimplifyHydrography")
            My.Computer.FileSystem.CurrentDirectory = aNewDataDir
            Logger.Status("DoneSimplifyHydrography " & MemUsage())

        Catch lEx As Exception
            lProblem = "Exception " & lEx.Message & vbCrLf & lEx.StackTrace
            Logger.Status(lProblem)
        End Try

    End Sub

    Friend Sub SetStatus(ByVal aStatus As String)
        Logger.Dbg(aStatus)
        If aStatus.StartsWith("Exit") Then
            pThreadCount -= 1
        End If
    End Sub

    Friend Sub BatchSWAT(ByVal aHuc As String, ByVal aProjectFolder As String, _
                         ByVal aSimplifiedCatchmentsFileName As String, _
                         ByVal aSimplifiedFlowlinesFileName As String, _
                         ByVal aLandUseFileName As String, _
                         ByVal aDemGridFileName As String, _
                         ByVal aSelectedShape As MapWinGIS.Shape)
        Dim lProblem As String = ""

        Try
            'KLW 12/21/2009 Generate files for running ArcSWAT
            If (g_CreateArcSWATFiles) Then
                modArcSWAT.CopyShapeFiles(aSimplifiedFlowlinesFileName, aProjectFolder & "\ArcSwat\")
                modArcSWAT.CopyShapeFiles(aSimplifiedCatchmentsFileName, aProjectFolder & "\ArcSwat\")
                Dim sfile As String = IO.Path.GetFileNameWithoutExtension(aSimplifiedFlowlinesFileName)
                Dim numRecs As Integer
                numRecs = modArcSWAT.ConvertFlowLineFile(aProjectFolder & "\ArcSwat\" & sfile & ".dbf", _
                                                         aProjectFolder + "\ArcSwat\" & sfile & ".dbf")
                sfile = IO.Path.GetFileNameWithoutExtension(aSimplifiedCatchmentsFileName)
                modArcSWAT.FillCatchmentFile(aProjectFolder & "\ArcSwat\" & sfile & ".dbf", numRecs)
            End If
            ' End Generate files for running ArcSWAT

            If g_BuildDatabase AndAlso Not FileExists(g_SWATDatabaseName) Then
                g_SWATDatabaseName = FindFile("Please locate SWAT2005.mdb", "SWAT2005.mdb").Replace("swat", "SWAT")
                If Not FileExists(g_SWATDatabaseName) Then
                    Logger.Msg("SWAT Database not found: '" & g_SWATDatabaseName & "'", "SWAT2005.mdb Required")
                    Exit Sub
                End If
            End If

            Dim lHRUGridFileName As String = IO.Path.Combine(aProjectFolder, "HRUs.tif")
            Dim lHruTableFilename As String = IO.Path.ChangeExtension(lHRUGridFileName, ".table.txt")
            Dim lHruTable As D4EMDataManager.clsHruTable = Nothing
            My.Computer.FileSystem.CurrentDirectory = aProjectFolder

            Dim lDisplayTags As New Generic.List(Of String)
            lDisplayTags.Add("SubBasin")
            lDisplayTags.Add("Soil")
            lDisplayTags.Add("SlopeReclass")
            lDisplayTags.Add("LandUse")

            If g_GeoProcess Then
                'Slope - Elevation
                Dim lSlopeGridFileName As String = PathNameOnly(aDemGridFileName) & "\slopeswat.tif"
                If IO.File.Exists(lSlopeGridFileName) Then
                    Logger.Status("UsingExisting " & lSlopeGridFileName)
                Else
                    'clip grid to extents
                    Dim lDEMGrid As String = PathNameOnly(aDemGridFileName) & "\tempdem.tif"
                    Dim lSuccess As Boolean = MapWinGeoProc.SpatialOperations.ClipGridWithPolygon(aDemGridFileName, aSelectedShape, lDEMGrid, True)

                    'z factor - 0.01 = cm to m
                    If MapWinGeoProc.TerrainAnalysis.Slope(lDEMGrid, 0.01, lSlopeGridFileName, True, Nothing) Then
                        Logger.Status("Calculated Slopes " & MemUsage())
                    Else
                        Logger.Status("Unable to calculate slope in  '" & aDemGridFileName & "'")
                    End If
                End If

                Dim lSlopeReclassifyGridFileName As String = PathNameOnly(aDemGridFileName) & "\slopeReclassify.tif"
                If IO.File.Exists(lSlopeReclassifyGridFileName) Then
                    Logger.Status("UsingExisting " & lSlopeReclassifyGridFileName)
                Else
                    Dim lReclassiflyScheme As New Generic.List(Of Double)
                    With lReclassiflyScheme
                        .Add(-0.00001) 'no slope data
                        .Add(0.5)
                        .Add(2)
                        .Add(GetMaxValue) 'All values above previous category. Note: may include some "bad" slope values
                    End With
                    If ReclassifySlope(lSlopeGridFileName, lReclassiflyScheme, lSlopeReclassifyGridFileName) Then
                        Logger.Status("Slopes Reclassified " & MemUsage())
                    Else
                        Logger.Status("Unable to reclassify slopes in  '" & lSlopeGridFileName & "'")
                    End If
                End If

                'subbasin gis properties
                Logger.Status("CalculateCatchmentProperty " & MemUsage())
                CalculateCatchmentProperty(aSimplifiedCatchmentsFileName, aDemGridFileName)
                Logger.Status("CalculateCatchmentPropertyDone " & MemUsage())
                'flowline gis properties 
                CalculateFlowlineProperty(aSimplifiedFlowlinesFileName, aDemGridFileName)
                Logger.Status("CalculateFlowlinePropertyDone " & MemUsage())

                Dim lSoilsLayer As String = GisUtil.LayerFileName("State Soil")
                Dim lLayers() As String = { _
                 aLandUseFileName & "|Tag=LandUse", _
                 lSoilsLayer & "|Tag=Soil|IdField=2|Idname=MUID", _
                 lSlopeReclassifyGridFileName & "|Tag=SlopeReclass", _
                 aSimplifiedCatchmentsFileName & "|IdField=0|IdName=COMID|Required=True|Tag=SubBasin"}
                'Dim lLayers() As String = { _
                ' aLandUseFileName & "|Tag=LandUse", _
                ' lSlopeReclassifyGridFileName & "|Tag=SlopeReclass", _
                ' aSimplifiedCatchmentsFileName & "|IdField=0|IdName=COMID|Required=True|Tag=SubBasin"}
                pLayerFilenames = lLayers

                If g_GeoProcess AndAlso pLayerFilenames Is Nothing OrElse pLayerFilenames.Length < 1 Then
                    Logger.Msg("No layers specified for overlay", "Geoprocessing Overlay Requires Layers")
                    Exit Sub
                End If

                If g_GeoProcess Then 'Check for layer that does not exist (e.g. soils)
                    For Each lLayerName As String In pLayerFilenames
                        Dim lFileName As String = StrSplit(lLayerName, "|", "")
                        If Not IO.File.Exists(lFileName) Then
                            Logger.Msg(lFileName, "Layer for overlay does not exist")
                            Exit Sub
                        End If
                    Next
                End If

                If pResume AndAlso IO.File.Exists(lHruTableFilename) Then
                    lHruTable = New D4EMDataManager.clsHruTable(lHruTableFilename)
                End If
                If lHruTable Is Nothing OrElse lHruTable.Count = 0 Then
                    Logger.Status("Doing Overlay")
                    lHruTable = D4EMDataManager.clsOverlayReclassify.Overlay(lHRUGridFileName, _
                                                              lSlopeGridFileName, _
                                                              pResume, _
                                                              pLayerFilenames)
                    If lHruTable IsNot Nothing Then Logger.Status("Overlay Successful")
                End If

                If lHruTable Is Nothing Then
                    Logger.Status("HRU table not produced by overlay")
                ElseIf lHruTable.Count = 0 Then
                    Logger.Status("HRU table produced by overlay is empty")
                Else
                    Dim lHruReportBuilder As New System.Text.StringBuilder
                    lHruTable = lHruTable.Sort(True)
                    D4EMDataManager.clsOverlayReclassify.ReportByTag(lHruReportBuilder, New atcCollection(lHruTable), lDisplayTags)
                    IO.File.WriteAllText(IO.Path.GetDirectoryName(lHRUGridFileName) & "\Hrus.txt", lHruReportBuilder.ToString)
                End If
                Logger.Status("DoneGeoProcessing " & MemUsage())
            Else
                lHruTable = New D4EMDataManager.clsHruTable(lHruTableFilename)
            End If

            If lHruTable.Count > 0 Then
                Logger.Status("CountOfRawHrus " & lHruTable.Count)
                Dim lOriginalIDs As Generic.List(Of String) = Nothing
                Dim lNewIds As Generic.List(Of String) = Nothing

                'Look for any files like g_BaseFolder\ReclassifyLandUse.csv, follow the table if such files exist
                For Each lTag As String In lDisplayTags
                    Dim lReclassifyFileName As String = IO.Path.Combine(aProjectFolder, "Reclassify") & lTag & ".csv"
                    If IO.File.Exists(lReclassifyFileName) Then
                        lHruTable.ReadReclassifyCSV(lReclassifyFileName, lOriginalIDs, lNewIds, ":")
                        lHruTable.Reclassify(lTag, lOriginalIDs, lNewIds)
                        'TODO: lHruTable.Tags(lHruTable.Tags.IndexOf("SubBasin") = "MetSeg"
                        Logger.Status("CountOfHrusAfterReclassify" & lTag & " " & lHruTable.Count)
                    End If
                Next

                If g_AreaIgnoreBelowFraction > 0 OrElse g_AreaIgnoreBelowAbsolute > 0 Then
                    lHruTable = D4EMDataManager.clsOverlayReclassify.Simplify(lHruTable.Tags, lHruTable.SplitByTag("SubBasin"), _
                                "Area", g_AreaIgnoreBelowFraction, g_AreaIgnoreBelowAbsolute, lHRUGridFileName)
                    Logger.Status("CountOfHrusAfterSimplifyArea " & lHruTable.Count)
                End If

                If g_LandUseIgnoreBelowFraction > 0 OrElse g_LandUseIgnoreBelowAbsolute > 0 Then
                    lHruTable = D4EMDataManager.clsOverlayReclassify.Simplify(lHruTable.Tags, lHruTable.SplitByTag("SubBasin"), _
                                "LandUse", g_LandUseIgnoreBelowFraction, g_LandUseIgnoreBelowAbsolute, lHRUGridFileName)
                    Logger.Status("CountOfHrusAfterSimplifyLandUse " & lHruTable.Count)
                End If

                If g_BuildDatabase Then
                    Dim lParamTable As atcTable = Nothing
                    Dim lSubBasinToParamIndex As atcCollection = Nothing
                    If IO.File.Exists(g_ParameterShapefileName) Then
                        lSubBasinToParamIndex = OverlayShapefiles(g_ParameterShapefileName, aSimplifiedCatchmentsFileName, "COMID")
                        lParamTable = New atcTableDBF
                        lParamTable.OpenFile(IO.Path.ChangeExtension(g_ParameterShapefileName, ".dbf"))
                    End If
                    Logger.Status("BuildSWATDatabase")
                    BuildSwatDatabase(g_CacheFolder, aProjectFolder, aSimplifiedFlowlinesFileName, aHuc, g_SWATDatabaseName, lParamTable, lSubBasinToParamIndex, lHruTable, g_WhichSoilIndex)
                    Logger.Status("After BuildSwatDatabase " & MemUsage())
                    If (g_CreateArcSWATFiles) Then

                    End If
                End If
                Dim lExitCode As Integer = 0
                Dim lInputFilePath As String = IO.Path.Combine(aProjectFolder, "Scenarios\" & aHuc & "\TxtInOut")
                If g_RunModel Then
                    If Not IO.Directory.Exists(lInputFilePath) Then
                        Logger.Status("Cannot start model, input folder does not exist: " & lInputFilePath)
                        lExitCode = -1
                    Else
                        Dim lExePath As String = FindFile("SWAT Model", "Swat2005.exe")
                        If IO.File.Exists(lExePath) Then
                            Logger.Status("StartModel " & lExePath)
                            MapWinUtility.LaunchProgram(lExePath, lInputFilePath)
                            'lExitCode = MapWinUtility.LaunchProgram(IO.Path.Combine(g_SWATProgramBase, "Swat2005.exe"), lInputFilePath)
                            Logger.Status("DoneModelRunExitCode " & lExitCode & " " & MemUsage())
                        Else
                            Logger.Msg("Could not launch SWAT model", "Swat2005.exe not found")
                            lExitCode = -1
                        End If
                    End If
                End If

                If g_OutputSummarize AndAlso lExitCode = 0 Then
                    Logger.Status("PostProcessModelResults")
                    OutputSummarize(aProjectFolder, lInputFilePath, aHuc)
                    Logger.Status("DoneOutputSummarize " & MemUsage())
                End If
                Logger.Status("**** Finished " & aHuc)
            End If
        Catch lEx As Exception
            lProblem = "Exception " & lEx.Message & vbCrLf & lEx.StackTrace
            Logger.Status(lProblem)
        End Try
        SetStatus(("Exit " & aHuc & " " & lProblem).Trim)
    End Sub

    Friend Function MemUsage() As String
        System.GC.Collect()
        System.GC.WaitForPendingFinalizers()
        Dim lPrivateMemory As Integer = System.Diagnostics.Process.GetCurrentProcess.PrivateMemorySize64 / (2 ^ 20)
        Dim lGcMemory As Integer = System.GC.GetTotalMemory(True) / (2 ^ 20)
        MemUsage = "Megabytes: " & lPrivateMemory & " " & Format((lPrivateMemory - pLastPrivateMemory), "+0;-0") & " " _
                   & " GC: " & lGcMemory & " " & Format((lGcMemory - pLastGcMemory), "+0;-0")
        pLastPrivateMemory = lPrivateMemory
        pLastGcMemory = lGcMemory
    End Function
End Module
