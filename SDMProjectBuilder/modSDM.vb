Imports atcUtility
Imports MapWinUtility

Public Module modSDM
    Friend g_MapWin As MapWindow.Interfaces.IMapWin
    Friend g_AppNameRegistry As String = "FramesSDM" 'For preferences in registry
    Friend g_AppNameShort As String = "FramesSDM"
    Friend g_AppNameLong As String = "Frames SDM"

    Friend g_DoHSPF As Boolean = True
    Friend g_DoSWAT As Boolean = True
    Private g_MinCatchmentKM2 As Double = 1.0 'Minimum catchment size
    Private g_MinFlowlineKM As Double = 15.0 'Minimum flowline length

    Friend g_MapWinWindowHandle As Integer
    Friend g_ProgramDir As String = ""
    Friend g_ClipCatchments As Boolean = True

    Public g_KeepConnectingRemovedFlowLines As Boolean = True
    Friend pBuildFrm As frmBuildNew

    'Private g_Clip As String = "" 'if using old style clip = "Clip"
    'Private g_BaseDrive As String = "g"
    'Private g_Project As String = "apes" '"0401" '
    Public g_BaseFolder As String
    Private g_CacheFolder As String '= "d:\Basins\Cache\" 'Downloaded data is kept here to avoid downloading the same thing again later
    Private g_SWATProgramBase As String = "G:\BASINS"
    Private g_SWATDatabaseName As String = g_SWATProgramBase & "\Databases\SWAT2005.mdb"
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
    Private g_LandUseIgnoreBelowFraction As Double = 0.07 'Land uses covering less than this much of a subbasin will have all their HRUs in that subbasin dissolved
    Private g_LandUseIgnoreBelowAbsolute As Double = 0    'Absolute total area of a land use within a subbasin below which to dissolve its HRUs

    Private g_WhichSoilIndex As WhichSoilIndex = WhichSoilIndex.SWATSTATSGO

    'Private g_Download As Boolean = False 'True = replace exisiting project folder with fresh copy of downloaded data, False = use existing project folder if present
    'Private g_GetMetData As Boolean = True
    'Private g_GetNLCDData As Boolean = True
    'Private g_GetNHDPlusData As Boolean = True
    'Private g_SimplifyCatchmentsAndStreams As Boolean = True
    Private g_GeoProcess As Boolean = True
    Private g_BuildDatabase As Boolean = True
    Private g_RunModel As Boolean = False
    Private g_OutputSummarize As Boolean = False

    'Private g_Huc8Shapefilename As String = ""
    'Private g_HucShapefilename As String
    'Private g_SkipUntilHuc As String = ""   '"030101010803" '"030102050302" ' "030102030502"
    'Private g_ProcessOnlyHuc As String '= "04010201" '"030101020501" '"030101010803" ' "030102050901" "030202030604" '"030102050302"
    'Private g_HucFieldId As Integer = -1

    'Private g_Statistics As New atcTimeseriesStatistics.atcTimeseriesStatistics

    'Private pSlopeGridFileName As String
    'Private pSlopeGridReclassFileName As String
    'Private pSubBasinFileName As String
    Private pLayerFilenames() As String
    Private pResume As Boolean = True  'True to use existing full or partially complete overlay, False to do overlay from the start

    Private pLastPrivateMemory As Integer = 0
    Private pLastGcMemory As Integer = 0
    'Private pThreadMax As Integer = 2
    Private pThreadCount As Integer = 0

    'Delegate Sub StatusCallback(ByVal aStatus As String)
    'Private pStatusCallback As New StatusCallback(AddressOf SetStatus)

    Friend Sub UpdateSelectedFeatures()
        If Not pBuildFrm Is Nothing AndAlso g_MapWin.Layers.NumLayers > 0 AndAlso g_MapWin.Layers.CurrentLayer > -1 Then
            Dim lFieldName As String = ""
            Dim lFieldDesc As String = ""
            Dim lField As Integer
            Dim lNameIndex As Integer = -1
            Dim lDescIndex As Integer = -1
            Dim lCurLayer As MapWinGIS.Shapefile
            Dim ctext As String

            RefreshView()
            ctext = "Selected Features:" & vbCrLf & "  <none>"
            If g_MapWin.Layers.Item(g_MapWin.Layers.CurrentLayer).LayerType = MapWindow.Interfaces.eLayerType.PolygonShapefile Then
                lCurLayer = g_MapWin.Layers.Item(g_MapWin.Layers.CurrentLayer).GetObject
                If g_MapWin.View.SelectedShapes.NumSelected > 0 Then
                    ctext = "Selected Features:"
                    Select Case IO.Path.GetFileNameWithoutExtension(lCurLayer.Filename).ToLower
                        Case "cat", "huc", "huc250d3"
                            lFieldName = "CU"
                            lFieldDesc = "catname"
                        Case "cnty"
                            lFieldName = "FIPS"
                            lFieldDesc = "cntyname"
                        Case "st"
                            lFieldName = "ST"
                            lFieldDesc = "name"
                    End Select

                    lFieldName = lFieldName.ToLower
                    lFieldDesc = lFieldDesc.ToLower
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
                    Dim lname As String
                    Dim ldesc As String
                    Dim lSf As MapWinGIS.Shapefile = g_MapWin.Layers.Item(g_MapWin.Layers.CurrentLayer).GetObject
                    For lSelected = 0 To g_MapWin.View.SelectedShapes.NumSelected - 1
                        lShape = g_MapWin.View.SelectedShapes.Item(lSelected).ShapeIndex()
                        lname = ""
                        ldesc = ""
                        If lNameIndex > -1 Then
                            lname = lSf.CellValue(lNameIndex, lShape)
                        End If
                        If lDescIndex > -1 Then
                            ldesc = lSf.CellValue(lDescIndex, lShape)
                        End If
                        ctext = ctext & vbCrLf & "  " & lname & " : " & ldesc
                    Next
                End If
                pBuildFrm.txtSelected.Text = ctext
            End If
        End If
    End Sub

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

        Try
            Dim lCatchmentsToUseFilename As String
            Dim lFlowLinesToUseFilename As String

            lCatchmentsToUseFilename = IO.Path.Combine(aNewDataDir, "nhdplus" & aSelectedHuc & "\drainage\catchment.shp")
            lFlowLinesToUseFilename = IO.Path.Combine(aNewDataDir, "nhdplus" & aSelectedHuc & "\hydrography\nhdflowline.shp")

            Dim lFlowLinesShapeFilename As String = lFlowLinesToUseFilename
            Dim lCatchmentsShapeFilename As String = lCatchmentsToUseFilename
            lCatchmentsToUseFilename = IO.Path.Combine(aNewDataDir, "nhdplus" & aSelectedHuc & "\drainage\usecatchment.shp")
            lFlowLinesToUseFilename = IO.Path.Combine(aNewDataDir, "nhdplus" & aSelectedHuc & "\hydrography\usenhdflowline.shp")

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
                         ByVal aSimplifiedFlowlinesFileName As String)
        Dim lProblem As String = ""

        Try
            'KLW 12/21/2009 Generate files for running ArcSWAT
            'If (g_CreateArcSWATFiles) Then
            '    modArcSWAT.CopyShapeFiles(lSimplifiedFlowlinesFileName, lProjectFolder + "\ArcSwat\")
            '    modArcSWAT.CopyShapeFiles(lSimplifiedCatchmentsFileName, lProjectFolder + "\ArcSwat\")
            '    Dim sfile As String = IO.Path.GetFileNameWithoutExtension(lSimplifiedFlowlinesFileName)
            '    Dim numRecs As Integer
            '    numRecs = modArcSWAT.ConvertFlowLineFile(lProjectFolder + "\ArcSwat\" + sfile + ".dbf", lProjectFolder + "\ArcSwat\" + sfile + ".dbf")
            '    sfile = IO.Path.GetFileNameWithoutExtension(lSimplifiedCatchmentsFileName)
            '    modArcSWAT.FillCatchmentFile(lProjectFolder + "\ArcSwat\" + sfile + ".dbf", numRecs)
            'End If
            'Dim numRecs As Integer = modArcSWAT.ConvertFlowLineFile(lSimplifiedFlowlinesFileName, arcSwatFlowlines)
            ' End Generate files for running ArcSWAT

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
                Dim lDemGridFilename As String = IO.Path.Combine(aProjectFolder, "nhdplus\elev_cm.tif")
                Dim lSlopeGridFileName As String = IO.Path.Combine(aProjectFolder, "nhdplus\slope.tif")
                If IO.File.Exists(lSlopeGridFileName) Then
                    Logger.Status("UsingExisting " & lSlopeGridFileName)
                Else
                    'z factor - cm to m
                    If MapWinGeoProc.TerrainAnalysis.Slope(lDemGridFilename, 0.01, lSlopeGridFileName, True, Nothing) Then
                        Logger.Status("Calculated Slopes " & MemUsage())
                    Else
                        Logger.Status("Unable to calculate slope in  '" & lDemGridFilename & "'")
                    End If
                End If

                Dim lSlopeReclassifyGridFileName As String = IO.Path.Combine(aProjectFolder, "nhdplus\slopeReclassify.tif")
                If IO.File.Exists(lSlopeReclassifyGridFileName) Then
                    Logger.Status("UsingExisting " & lSlopeReclassifyGridFileName)
                Else
                    Dim lReclassiflyScheme As New ArrayList
                    With lReclassiflyScheme
                        .Add(-0.00001) 'no slope data
                        .Add(0.5)
                        .Add(2)
                        .Add(9999)
                    End With
                    If ReclassifySlope(lSlopeGridFileName, lReclassiflyScheme, lSlopeReclassifyGridFileName) Then
                        Logger.Status("Slopes Reclassified " & MemUsage())
                    Else
                        Logger.Status("Unable to reclassify slopes in  '" & lSlopeGridFileName & "'")
                    End If
                End If

                'subbasin gis properties
                Logger.Status("CalculateCatchmentProperty " & MemUsage())
                CalculateCatchmentProperty(aSimplifiedCatchmentsFileName, lDemGridFilename)
                Logger.Status("CalculateCatchmentPropertyDone " & MemUsage())
                'flowline gis properties 
                CalculateFlowlineProperty(aSimplifiedFlowlinesFileName, lDemGridFilename)
                Logger.Status("CalculateFlowlinePropertyDone " & MemUsage())

                If pResume AndAlso IO.File.Exists(lHruTableFilename) Then
                    lHruTable = New D4EMDataManager.clsHruTable(lHruTableFilename)
                End If
                If lHruTable Is Nothing OrElse lHruTable.Count = 0 Then
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
                    Dim lReclassifyFileName As String = IO.Path.Combine(g_BaseFolder, "Reclassify") & lTag & ".csv"
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
                        Logger.Status("StartModel")
                        MapWinUtility.LaunchProgram(IO.Path.Combine(g_SWATProgramBase, "Swat2005.exe"), lInputFilePath)
                        'lExitCode = MapWinUtility.LaunchProgram(IO.Path.Combine(g_SWATProgramBase, "Swat2005.exe"), lInputFilePath)
                        Logger.Status("DoneModelRunExitCode " & lExitCode & " " & MemUsage())
                    End If
                End If

                If g_OutputSummarize AndAlso lExitCode = 0 Then
                    Logger.Status("PostProcessModelResults")
                    OutputSummarize(aProjectFolder, lInputFilePath, aHuc)
                    Logger.Status("DoneOutputSummarize " & MemUsage())
                End If
                Logger.Status("**** Done HUC12 " & aHuc)
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
        MemUsage = "Megabytes: " & lPrivateMemory & " (" & Format((lPrivateMemory - pLastPrivateMemory), "+0;-0") & ") " _
                   & " GC: " & lGcMemory & " (" & Format((lGcMemory - pLastGcMemory), "+0;-0") & ") "
        pLastPrivateMemory = lPrivateMemory
        pLastGcMemory = lGcMemory
    End Function
End Module
