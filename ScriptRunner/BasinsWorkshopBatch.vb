Imports System
Imports System.Collections 
Imports System.Collections.Specialized
Imports MapWindow.Interfaces
Imports MapWinUtility

Imports atcUtility
Imports atcData
Imports atcManDelin
Imports BASINS
Imports D4EMDataManager

Module BasinsWorkshopBatch
    Private pMapWin As IMapWin
    Private pProjectNames() As String = {"06010105", "Patuxent"}

    Private pProjectName As String
    Private pHUC8 As String = ""
    Private pDrive As String = "c:"
    Private pBaseFolder As String = pDrive & "\BASINS\"
    Private pProjectFolder As String = ""
    Private pCacheFolder As String = ""
    Private pCacheFolderSave As String = ""
    Private pCacheClear As Boolean = False
    Private pBoundingBoxAea As String = ""
    Private pBoundingBoxLL As String = ""
    Private pMetStationIds As String = ""
    Private pDischargeStationIds As String = ""
    Private pProjection As String = ""
    Private pMaxStations As Integer = 20

    Private Sub Initialize(ByVal aProjectName As String)
        pProjectName = aProjectName
        pProjectFolder = pBaseFolder & "Data\WorkshopBatch\" & pProjectName & "\"
        pCacheFolder = pProjectFolder & "Cache\"
        pCacheFolderSave = pCacheFolder.Replace("Batch", "BatchSave")

        Select Case aProjectName
            Case "Patuxent"
                pHUC8 = "02060006"
                pProjection = "+proj=utm +zone=18 +ellps=GRS80 +lon_0=-75 +lat_0=0 +k=0.9996 +x_0=500000.0 +y_0=0 +datum=NAD83 +units=m"
                pBoundingBoxAea = _
                      "   <northbc>1975392.91047589</northbc>" & _
                      "   <southbc>1866978.51560156</southbc>" & _
                      "   <eastbc>1684619.12695581</eastbc>" & _
                      "   <westbc>1595425.21946512</westbc>"
                pBoundingBoxLL = _
                      "   <northbc>39.3668056784273</northbc>" & _
                      "   <southbc>38.257273329737</southbc>" & _
                      "   <eastbc>-76.4009709099128</eastbc>" & _
                      "   <westbc>-77.2070255407762</westbc>"
                pMetStationIds = _
                      "<stationid>MD180193</stationid> <stationid>MD180193</stationid> <stationid>MD180193</stationid> <stationid>MD180460</stationid> <stationid>MD180460</stationid> <stationid>MD180460</stationid> <stationid>MD180465</stationid> <stationid>MD180465</stationid> <stationid>MD180465</stationid> <stationid>MD180465</stationid> <stationid>MD180465</stationid> <stationid>MD180465</stationid> <stationid>MD180465</stationid> " & _
                      "<stationid>MD180465</stationid> <stationid>MD180470</stationid> <stationid>MD180470</stationid> <stationid>MD180470</stationid> <stationid>MD180470</stationid> <stationid>MD180475</stationid> <stationid>MD180475</stationid> <stationid>MD180475</stationid> <stationid>MD180700</stationid> <stationid>MD180700</stationid> <stationid>MD180700</stationid> <stationid>MD180700</stationid> <stationid>MD180700</stationid> <stationid>MD180700</stationid> <stationid>MD180700</stationid> <stationid>MD180700</stationid> <stationid>MD180701</stationid> <stationid>MD180701</stationid> <stationid>MD180701</stationid> <stationid>MD180702</stationid> <stationid>MD180702</stationid> <stationid>MD180702</stationid> <stationid>MD180703</stationid> <stationid>MD180703</stationid> <stationid>MD180703</stationid> <stationid>MD180704</stationid> <stationid>MD180704</stationid> <stationid>MD180704</stationid> <stationid>MD180705</stationid> <stationid>MD180705</stationid> <stationid>MD180705</stationid> <stationid>MD180705</stationid> " & _
                      "<stationid>MD180706</stationid> <stationid>MD180706</stationid> <stationid>MD180706</stationid> <stationid>MD180795</stationid> <stationid>MD180800</stationid> <stationid>MD180800</stationid> <stationid>MD180800</stationid> <stationid>MD181125</stationid> <stationid>MD181135</stationid> <stationid>MD181135</stationid> <stationid>MD181170</stationid> <stationid>MD181278</stationid> <stationid>MD181685</stationid> <stationid>MD181685</stationid> <stationid>MD181685</stationid> <stationid>MD181710</stationid> <stationid>MD181710</stationid> <stationid>MD181710</stationid> <stationid>MD181862</stationid> <stationid>MD181862</stationid> <stationid>MD181862</stationid> <stationid>MD181995</stationid> <stationid>MD181995</stationid> <stationid>MD181995</stationid> <stationid>MD181995</stationid> <stationid>MD182325</stationid> <stationid>MD182325</stationid> <stationid>MD182325</stationid> <stationid>MD182585</stationid> <stationid>MD182585</stationid> <stationid>MD182585</stationid> <stationid>MD182660</stationid> " & _
                      "<stationid>MD182660</stationid> <stationid>MD182660</stationid> <stationid>MD183230</stationid> <stationid>MD183230</stationid> <stationid>MD183230</stationid> <stationid>MD183645</stationid> <stationid>MD183675</stationid> <stationid>MD183675</stationid> <stationid>MD183675</stationid> <stationid>MD183860</stationid> <stationid>MD183860</stationid> <stationid>MD183860</stationid> <stationid>MD185080</stationid> <stationid>MD185080</stationid> <stationid>MD185080</stationid> <stationid>MD185111</stationid> <stationid>MD185111</stationid> <stationid>MD185111</stationid> <stationid>MD185201</stationid> <stationid>MD185201</stationid> <stationid>MD185201</stationid> <stationid>MD185718</stationid> <stationid>MD185718</stationid> <stationid>MD185718</stationid> <stationid>MD185718</stationid> <stationid>MD185865</stationid> <stationid>MD185865</stationid> <stationid>MD185865</stationid> <stationid>MD185916</stationid> <stationid>MD185916</stationid> <stationid>MD185916</stationid> <stationid>MD186350</stationid> " & _
                      "<stationid>MD186350</stationid> <stationid>MD186350</stationid> <stationid>MD186770</stationid> <stationid>MD186770</stationid> <stationid>MD186770</stationid> <stationid>MD186915</stationid> <stationid>MD186915</stationid> <stationid>MD186915</stationid> <stationid>MD187325</stationid> <stationid>MD187325</stationid> <stationid>MD187325</stationid> <stationid>MD187615</stationid> <stationid>MD187615</stationid> <stationid>MD187615</stationid> <stationid>MD187705</stationid> <stationid>MD187705</stationid> <stationid>MD187705</stationid> <stationid>MD188656</stationid> <stationid>MD188656</stationid> <stationid>MD188656</stationid> <stationid>MD188725</stationid> <stationid>MD188725</stationid> <stationid>MD188725</stationid> <stationid>MD189035</stationid> <stationid>MD189035</stationid> <stationid>MD189035</stationid> <stationid>MD189070</stationid> <stationid>MD189070</stationid> <stationid>MD189070</stationid> <stationid>MD189187</stationid> <stationid>MD189187</stationid> <stationid>MD189187</stationid> " & _
                      "<stationid>MD189195</stationid> <stationid>MD189195</stationid> <stationid>MD189195</stationid> <stationid>MD189290</stationid> <stationid>MD189290</stationid> <stationid>MD189290</stationid> <stationid>MD189290</stationid> <stationid>MD189314</stationid> <stationid>MD189314</stationid> <stationid>MD189314</stationid> <stationid>MD189502</stationid> <stationid>MD189502</stationid> <stationid>MD189502</stationid> <stationid>MD189750</stationid> <stationid>MD189750</stationid> <stationid>MD189750</stationid> <stationid>MD724040</stationid> <stationid>MD724040</stationid> <stationid>MD724040</stationid> <stationid>MD724040</stationid> <stationid>MD724040</stationid> <stationid>MD724040</stationid> <stationid>MD745940</stationid> <stationid>MD745940</stationid> <stationid>MD745940</stationid> <stationid>MD745940</stationid> <stationid>MD745940</stationid> <stationid>MD745940</stationid> <stationid>MD994400</stationid> <stationid>MD994400</stationid> <stationid>MD994400</stationid> <stationid>VA440090</stationid> " & _
                      "<stationid>VA440090</stationid> <stationid>VA440090</stationid> <stationid>VA440097</stationid> <stationid>VA440097</stationid> <stationid>VA440097</stationid> <stationid>VA441729</stationid> <stationid>VA442195</stationid> <stationid>VA442195</stationid> <stationid>VA442195</stationid> <stationid>VA442809</stationid> <stationid>VA442809</stationid> <stationid>VA442809</stationid> <stationid>VA442922</stationid> <stationid>VA442922</stationid> <stationid>VA442922</stationid> <stationid>VA443635</stationid> <stationid>VA448906</stationid> <stationid>VA448906</stationid> <stationid>VA448906</stationid> <stationid>VA448906</stationid> <stationid>VA448906</stationid> <stationid>VA448906</stationid> <stationid>VA448906</stationid> <stationid>VA448906</stationid> <stationid>VA448938</stationid> <stationid>VA448938</stationid> <stationid>VA448938</stationid> "
                pDischargeStationIds = "<stationid>01594526</stationid>"
            Case Else
                pHUC8 = aProjectName
                pProjection = "+proj=utm +zone=17 +ellps=GRS80 +lon_0=-75 +lat_0=0 +k=0.9996 +x_0=500000.0 +y_0=0 +datum=NAD83 +units=m"
                pBoundingBoxAea = ""
                pBoundingBoxLL = ""
                pMetStationIds = ""
                pDischargeStationIds = ""
        End Select
    End Sub

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        pMapWin = aMapWin
        Dim lOriginalFolder As String = IO.Directory.GetCurrentDirectory
        Dim lOriginalLog As String = Logger.FileName
        Dim lOriginalDisplayMessages As Boolean = Logger.DisplayMessageBoxes
        Logger.DisplayMessageBoxes = False
        Logger.Dbg("BasinsWorkshopBatch:OriginalDir:" & lOriginalFolder)

        Dim lPlugins As New ArrayList
        For lPluginIndex As Integer = 0 To pMapWin.Plugins.Count
            Try
                If Not pMapWin.Plugins.Item(lPluginIndex) Is Nothing Then
                    lPlugins.Add(pMapWin.Plugins.Item(lPluginIndex))
                End If
            Catch lEx As Exception
                Logger.Dbg(lPluginIndex.ToString & " Problem:" & lEx.ToString)
            End Try
        Next
        Dim lDownloadManager As New D4EMDataManager.DataManager(lPlugins)

        For Each lProject As String In pProjectNames
            Initialize(lProject)
            Logger.Dbg("  AboutToChangeLog NewProject " & lProject)
            Try
                If IO.Directory.Exists(pProjectFolder) Then
                    IO.Directory.Delete(pProjectFolder, True)
                    Logger.Dbg("ExistingProjectFolderDeleted:" & pProjectFolder)
                End If
                If Not IO.Directory.Exists(pCacheFolder) Then
                    IO.Directory.CreateDirectory(pCacheFolder)
                    Logger.Dbg("CacheFolderCreated:" & pCacheFolder)
                End If
                Logger.StartToFile(pCacheFolder & "BasinsWorkshopBatch.log", , , True)

                If Not pCacheClear AndAlso IO.Directory.Exists(pCacheFolderSave) Then
                    'copy existing cache
                    pMapWin.StatusBar.Item(1).Text = "Copy Existing Cache"
                    Logger.Dbg("UsingExistingCacheFrom:" & pCacheFolderSave)
                    Dim lFileCopyCount As Integer = 0
                    For Each lFile As String In IO.Directory.GetFiles(pCacheFolderSave, "*", IO.SearchOption.AllDirectories)
                        If IO.Path.GetExtension(lFile) = ".log" Then
                            Logger.Dbg("Skip " & lFile)
                        Else
                            Dim lNewFolder As String = IO.Path.GetDirectoryName(lFile).Replace(pCacheFolderSave.Trim("\"), pCacheFolder.Trim("\")) & "\"
                            If Not IO.Directory.Exists(lNewFolder) Then
                                IO.Directory.CreateDirectory(lNewFolder)
                            End If
                            Dim lFileNew As String = lNewFolder & IO.Path.GetFileName(lFile)
                            If IO.File.Exists(lFileNew) Then
                                Logger.Dbg("UsingExisting " & lFileNew)
                            Else
                                IO.File.Copy(lFile, lNewFolder & IO.Path.GetFileName(lFile))
                                lFileCopyCount += 1
                            End If
                        End If
                    Next
                    Logger.Dbg("UseExistingCache:FileCount:" & lFileCopyCount)
                End If

                If Not Exercise1(pProjectFolder, lDownloadManager, pCacheFolder) Then
                    Logger.Dbg("***** Exercise1 FAIL *****")
                ElseIf Not Exercise2() Then
                    Logger.Dbg("***** Exercise2 FAIL *****")
                ElseIf Not Exercise4(pProjectFolder, lDownloadManager, pCacheFolder) Then
                    Logger.Dbg("***** Exercise4 FAIL *****")
                ElseIf Not Exercise6(pProjectFolder) Then
                    Logger.Dbg("***** Exercise6 FAIL *****")
                End If
                SnapShotAndSave("AllDone " & lProject)
            Catch lEx As Exception
                Logger.Dbg("ProblemProject " & lProject & " " & lEx.ToString)
            End Try
            For Each lDataSource As atcDataSource In atcDataManager.DataSources
                lDataSource.Clear()
            Next
            pMapWin.Layers.Clear()
            pMapWin.PreviewMap.GetPictureFromMap()
        Next
        IO.Directory.SetCurrentDirectory(lOriginalFolder)
        Logger.Dbg("BasinsWorkshopBatchDone")
        Logger.StartToFile(lOriginalLog, True, , True)
        Logger.Dbg("BackFromBasinsWorkshopBatch")
        Logger.DisplayMessageBoxes = lOriginalDisplayMessages
    End Sub

    Private Function Exercise1(ByVal aBasinsProjectDataFolder As String, ByVal aDownloadManager As D4EMDataManager.DataManager, ByVal aCacheFolder As String) As Boolean
        'Adding Data to a New BASINS Project
        '  Build a New BASINS Project
        '  TODO: open National Project and select pProject in code

        '  create project
        SaveFileString(aBasinsProjectDataFolder & "prj.proj", pProjection) 'Side effect: makes data directory
        IO.Directory.SetCurrentDirectory(aBasinsProjectDataFolder)
        If Not IO.Directory.Exists("snapshots") Then IO.Directory.CreateDirectory("snapshots")

        Dim lRegion As String = "<region>" & pBoundingBoxAea & _
           "   <projection>+proj=aea +lat_1=29.5 +lat_2=45.5 +lat_0=23 +lon_0=-96 +x_0=0 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs</projection>" & _
           "   <HUC8 status=""set by BASINS System Application"">" & pHUC8 & "</HUC8>" & _
           "</region> "
        CreateNewProjectAndDownloadCoreData(lRegion, DefaultBasinsDataDir, aBasinsProjectDataFolder, aBasinsProjectDataFolder & pProjectName & ".mwprj", False, aCacheFolder)
        SnapShotAndSave("E1_AfterDownloadCore")

        '  Navigate the BASINS 4.0 GIS Environment
        '  TODO: Turn reach layer off and on, Zoom in and out, zoom to cataloging unit layer extent, edit attribute table

        '  General Specs for download
        Dim lQuery As String = _
          "<function name='#Name#'>" & _
          "  <arguments>" & _
          "    <DataType>#DataType#</DataType>" & _
          "    <SaveIn>" & aBasinsProjectDataFolder & "</SaveIn>" & _
          "    <CacheFolder>" & aCacheFolder & "</CacheFolder>" & _
          "    <DesiredProjection>" & pProjection & "</DesiredProjection>" & _
          "    <region>" & _
          "       <HUC8>" & pHUC8 & "</HUC8>" & _
          "       <preferredformat>huc8</preferredformat>" & _
          "       <projection>+proj=latlong +datum=NAD83</projection>" & _
          "    </region>" & _
          "    <clip>False</clip>" & _
          "    <merge>False</merge>" & _
          "    <joinattributes>true</joinattributes>" & _
          "  </arguments>" & _
          "</function>"

        '  Add LandUse Data
        Dim lQueryLU As String = lQuery.Replace("#DataType#", "Giras").Replace("#Name#", "GetBASINS")
        Dim lResultLU As String = aDownloadManager.Execute(lQueryLU)
        If lResultLU Is Nothing OrElse lResultLU.Length = 0 Then
            'Nothing to report, no success or error
        ElseIf lResultLU.StartsWith("<success>") Then
            BASINS.ProcessDownloadResults(lResultLU)
            SnapShotAndSave("E1_AfterDownloadLandUse")
            For lLayerIndex As Integer = 0 To pMapWin.Layers.NumLayers - 1
                With pMapWin.Layers(lLayerIndex)
                    If .Name.ToLower.Contains("land use") Then
                        .Visible = False
                    End If
                End With
            Next
        Else
            Logger.Msg(atcUtility.ReadableFromXML(lResultLU), "LUDownload Result")
        End If

        '  Add NHDPlus Data
        Dim lQueryNHD As String = lQuery.Replace("#DataType#", "hydrography</DataType> <DataType>Catchment").Replace("#Name#", "GetNHDPlus")
        Dim lResultNHD As String = aDownloadManager.Execute(lQueryNHD)
        If lResultNHD Is Nothing OrElse lResultNHD.Length = 0 Then
            'Nothing to report, no success or error
        ElseIf lResultNHD.StartsWith("<success>") Then
            BASINS.ProcessDownloadResults(lResultNHD)
            SnapShotAndSave("E1_AfterDownloadNHD")
            For Each lLayer As MapWindow.Interfaces.Layer In pMapWin.Layers
                Dim lName As String = lLayer.Name.ToLower
                If lName.Contains("flowline") OrElse _
                   lName.Contains("catchment") OrElse _
                   lName.Contains("area features") OrElse _
                   lName.Contains("waterbody features") Then
                    lLayer.Visible = False
                End If
            Next
        Else
            Logger.Msg(atcUtility.ReadableFromXML(lResultNHD), "NHDDownload Result")
        End If

        If pProjectName = "Patuxent" Then
            '  Add BASINS census and TIGER line data
            Dim lQueryCensus As String = lQuery.Replace("#DataType#", "Census").Replace("#Name#", "GetBASINS")
            Dim lResultCensus As String = aDownloadManager.Execute(lQueryCensus)
            If lResultCensus Is Nothing OrElse lResultCensus.Length = 0 Then
                'Nothing to report, no success or error
            ElseIf lResultCensus.StartsWith("<success>") Then
                BASINS.ProcessDownloadResults(lResultCensus)
                SnapShotAndSave("E1_AfterDownloadCensus")
                For Each lLayer As MapWindow.Interfaces.Layer In pMapWin.Layers
                    If lLayer.Name.ToLower.Contains("tiger") Then
                        lLayer.Visible = False
                    End If
                Next
            Else
                Logger.Msg(atcUtility.ReadableFromXML(lResultNHD), "CensusDownload Result")
            End If
        End If

        '  Add BASINS Digital Elevation Model (DEM) grids
        pMapWin.Layers.CurrentLayer = pMapWin.Layers.NumLayers - 1
        Dim lQueryDEMG As String = lQuery.Replace("#DataType#", "DEMG").Replace("#Name#", "GetBASINS")
        Dim lResultDEMG As String = aDownloadManager.Execute(lQueryDEMG)
        If lResultDEMG Is Nothing OrElse lResultDEMG.Length = 0 Then
            'Nothing to report, no success or error
        ElseIf lResultDEMG.StartsWith("<success>") Then
            BASINS.ProcessDownloadResults(lResultDEMG)
        Else
            Logger.Msg(atcUtility.ReadableFromXML(lResultNHD), "DEMGDownload Result")
        End If
        SnapShotAndSave("E1_AfterDownloadDEM")

        '  Import other shapefiles
        For Each lLayer As MapWindow.Interfaces.Layer In pMapWin.Layers
            If lLayer.Name.ToLower.Contains("flowline features") Then
                lLayer.Visible = True
                pMapWin.Layers.CurrentLayer = lLayer.Handle
                Exit For
            End If
        Next

        If pProjectName = "Patuxent" Then
            Dim lPreDefDelinDir As String = aBasinsProjectDataFolder & "tutorial\"
            If Not IO.Directory.Exists(lPreDefDelinDir) Then
                IO.Directory.CreateDirectory(lPreDefDelinDir)
            End If
            Dim lPreDefDelinName As String = "w_branch"
            For Each lFileName As String In IO.Directory.GetFiles(aBasinsProjectDataFolder.Replace("WorkshopBatch\Patuxent\", "tutorial\"), lPreDefDelinName)
                IO.File.Copy(lFileName, lPreDefDelinDir & IO.Path.GetFileName(lFileName))
            Next
            Dim lPreDefDelinFile As String = lPreDefDelinDir & lPreDefDelinName & ".shp"
            If IO.File.Exists(lPreDefDelinFile) Then
                Dim lLayerAdd As MapWindow.Interfaces.Layer = pMapWin.Layers.Add(lPreDefDelinFile, lPreDefDelinName, True, True)
                With lLayerAdd
                    .OutlineColor = Drawing.Color.Red
                    .DrawFill = False
                End With
            Else
                Logger.Dbg("FileNotFound:" & lPreDefDelinFile)
            End If
            SnapShotAndSave("E1_AfterDownloadPredifined")
        End If

        '  Download timeseries data for use in modeling
        Dim lQueryMetStations As String = lQuery.Replace("#DataType#", "MetStations").Replace("#Name#", "GetBASINS")
        Dim lResultMetStations As String = aDownloadManager.Execute(lQueryMetStations)
        If lResultMetStations Is Nothing OrElse lResultMetStations.Length = 0 Then
            'Nothing to report, no success or error
        ElseIf lResultMetStations.StartsWith("<success>") Then
            BASINS.ProcessDownloadResults(lResultMetStations)
        Else
            Logger.Msg(atcUtility.ReadableFromXML(lResultMetStations), "MetStationsDownload Result")
        End If
        Logger.Dbg("ProjectSaved:" & pMapWin.Project.Save(pMapWin.Project.FileName))

        If pMetStationIds.Length = 0 Then
            Dim lLayer As MapWindow.Interfaces.Layer = Nothing
            For Each lLayer In pMapWin.Layers
                If lLayer.Name.Contains("Weather Station Sites 2006") Then
                    pMapWin.Layers.CurrentLayer = lLayer.Handle
                    Exit For
                End If
            Next
            Dim lStationShapefile As MapWinGIS.Shapefile = lLayer.GetObject
            Dim lFieldIndex As Integer = 0
            For lFieldIndex = 0 To lStationShapefile.NumFields - 1
                If lStationShapefile.Field(lFieldIndex).Name = "LOCATION" Then
                    Exit For
                End If
            Next
            pMetStationIds = ""
            For lShapeIndex As Integer = 0 To lStationShapefile.NumShapes - 1
                pMetStationIds &= "<stationid>" & lStationShapefile.CellValue(lFieldIndex, lShapeIndex) & "</stationid>"
                If lShapeIndex > pMaxStations Then Exit For
            Next
        End If

        Dim lQueryMetData As String = "<function name='GetBASINS'> <arguments> <DataType>MetData</DataType>" & _
                                      "<SaveWDM>" & aBasinsProjectDataFolder & "met\met.wdm</SaveWDM>" & _
                                      "<SaveIn>" & aBasinsProjectDataFolder & "</SaveIn>" & _
                                      "<CacheFolder>" & aCacheFolder & "</CacheFolder>" & _
                                      "<DesiredProjection>" & pProjection & "</DesiredProjection> " & _
                                      "<region> " & _
                                      "<HUC8>" & pHUC8 & "</HUC8> <preferredformat>huc8</preferredformat> <projection>+proj=latlong +datum=NAD83</projection> </region> " & _
                                      pMetStationIds & _
                                      "<clip>False</clip> <merge>False</merge> <joinattributes>true</joinattributes> </arguments> </function>  "
        Dim lResultMetData As String = aDownloadManager.Execute(lQueryMetData)
        If lResultMetData Is Nothing OrElse lResultMetData.Length = 0 Then
            'Nothing to report, no success or error
        ElseIf lResultMetData.StartsWith("<success>") Then
            BASINS.ProcessDownloadResults(lResultMetData)
        Else
            Logger.Msg(atcUtility.ReadableFromXML(lResultMetData), "MetDataDownload Result")
        End If
        SnapShotAndSave("E1_AfterDownloadMetData")

        Return True
    End Function

    Private Function Exercise2() As Boolean
        Dim lLayersActive() As String = {"cataloging unit boundaries", "state boundaries", "flowline features", "w_branch"}
        For Each lLayer As MapWindow.Interfaces.Layer In pMapWin.Layers
            Dim lLayerName As String = lLayer.Name.ToLower
            If Array.IndexOf(lLayersActive, lLayerName) = -1 Then
                lLayer.Visible = False
            Else
                lLayer.Visible = True
                If lLayer.Name.ToLower.Contains("w_branch") Then
                    pMapWin.Layers.CurrentLayer = lLayer.Handle
                End If
            End If
        Next
        SnapShotAndSave("E2_AfterLayerSetting")

        Dim lPlugInsActive() As String = {"manual delineation", "watershed delineation"}
        Dim lManualDelinPlugIn As atcManDelin.ManDelinPlugIn = Nothing
        For lPlugInIndex As Integer = 0 To pMapWin.Plugins.Count - 1
            Dim lPlugIn As MapWindow.Interfaces.IPlugin = pMapWin.Plugins.Item(lPlugInIndex)
            If Not lPlugIn Is Nothing Then
                Dim lPlugInName As String = lPlugIn.Name.ToLower
                Dim lPlugInKey As String = pMapWin.Plugins.GetPluginKey(lPlugInName)
                If Array.IndexOf(lPlugInsActive, lPlugInName) >= 0 Then
                    If pMapWin.Plugins.PluginIsLoaded(lPlugInName) Then
                        Logger.Dbg("AlreadyLoaded " & lPlugInName)
                    Else
                        pMapWin.Plugins.StartPlugin(lPlugInName)
                    End If
                    If lPlugInName.Contains("manual") Then
                        lManualDelinPlugIn = pMapWin.Plugins(lPlugInIndex)
                    End If
                ElseIf lPlugInName.Contains("deli") Then
                    Logger.Dbg("WhyGotHere:" & lPlugInName)
                End If
            End If
        Next

        pMapWin.Layers(pMapWin.Layers.CurrentLayer).ZoomTo()
        SnapShotAndSave("E2_AfterZoomTo")

        Dim lSubBasinThemeName As String = "Cataloging Unit Boundaries"
        If pProjectName = "Patuxent" Then
            lSubBasinThemeName = "W_Branch"
        End If

        If My.Computer.Info.OSFullName.Contains("x64") Then
            Logger.Dbg("SkippingManDelinOn64Bit")
        Else
            ManDelinPlugIn.CalculateSubbasinParameters(lSubBasinThemeName, "Digital Elevation Model")
            ManDelinPlugIn.CalculateReaches(lSubBasinThemeName, "Reach File, V1", "Digital Elevation Model", False, False, "")
            'TODO: Do some more stuff here
        End If
        Return True
    End Function

    Private Function Exercise4(ByVal aBasinsProjectDataFolder As String, ByVal aDownloadManager As D4EMDataManager.DataManager, ByVal aCacheFolder As String) As Boolean
        Dim lBaseQuery As String = _
              "<SaveIn>" & aBasinsProjectDataFolder & "</SaveIn>" & _
              "<CacheFolder>" & aCacheFolder & "</CacheFolder>" & _
              "<DesiredProjection>" & pProjection & "</DesiredProjection>" & _
              "<region> " & _
              pBoundingBoxLL & _
              "<HUC8>" & pHUC8 & "</HUC8> " & _
              "<preferredformat>huc8</preferredformat> <projection>+proj=latlong +datum=NAD83</projection> </region>" & _
              "<clip>False</clip>" & _
              "<merge>False</merge>" & _
              "<joinattributes>true</joinattributes>"

        Dim lQueryDischargeStations As String = _
          "<function name='GetNWISStations'>" & _
            "<arguments>" & _
              "<DataType>discharge</DataType>" & _
              lBaseQuery & _
            "</arguments>" & _
          "</function>"
        Dim lResultDischargeStations As String = aDownloadManager.Execute(lQueryDischargeStations)
        BASINS.ProcessDownloadResults(lResultDischargeStations)
        SnapShotAndSave("E4_AfterDischargeStations")

        If pDischargeStationIds.Length = 0 Then
            Dim lLayer As MapWindow.Interfaces.Layer = Nothing
            For Each lLayer In pMapWin.Layers
                If lLayer.Name.Contains("NWIS Daily Discharge Stations") Then
                    pMapWin.Layers.CurrentLayer = lLayer.Handle
                    Exit For
                End If
            Next
            Dim lStationShapefile As MapWinGIS.Shapefile = lLayer.GetObject
            Dim lFieldIndex As Integer = 0
            For lFieldIndex = 0 To lStationShapefile.NumFields - 1
                If lStationShapefile.Field(lFieldIndex).Name = "site_no" Then
                    Exit For
                End If
            Next
            For lShapeIndex As Integer = 0 To lStationShapefile.NumShapes - 1
                pDischargeStationIds &= "<stationid>" & lStationShapefile.CellValue(lFieldIndex, lShapeIndex) & "</stationid>"
                If lShapeIndex > pMaxStations Then Exit For
            Next
        End If

        Dim lQueryDischargeData As String = _
          "<function name='GetNWISDischarge'>" & _
            "<arguments>" & _
              "<SaveWDM>" & aBasinsProjectDataFolder & "nwis\flow.wdm</SaveWDM>" & _
              pDischargeStationIds & _
              lBaseQuery & _
            "</arguments>" & _
          "</function>"
        Dim lResultDischargeData As String = aDownloadManager.Execute(lQueryDischargeData)
        BASINS.ProcessDownloadResults(lResultDischargeData)

        Return True
    End Function

    Private Function Exercise6(ByVal aBasinsProjectDataFolder As String) As Boolean
        Dim lOutputFolder As String = aBasinsProjectDataFolder & "ReportsAndGraphs\"
        If Not IO.Directory.Exists(lOutputFolder) Then
            IO.Directory.CreateDirectory(lOutputFolder)
        End If
        Dim lDataGroup As atcTimeseriesGroup = atcDataManager.DataSets.FindData("ID", 1)
        Dim lZGC As ZedGraph.ZedGraphControl = atcGraph.CreateZgc
        lZGC.Width = 1024
        lZGC.Height = 768

        Dim lGraphTime As New atcGraph.clsGraphTime(lDataGroup, lZGC)
        lGraphTime.ZedGraphCtrl.SaveIn(lOutputFolder & "GraphTime2.emf")
        lGraphTime.ZedGraphCtrl.SaveIn(lOutputFolder & "GraphTime2.png")
        lGraphTime.Dispose()

        lDataGroup = lDataGroup.FindData("CONSTITUENT", "FLOW")
        lGraphTime = New atcGraph.clsGraphTime(lDataGroup, lZGC)
        lGraphTime.ZedGraphCtrl.SaveIn(lOutputFolder & "GraphTime1.emf")
        lGraphTime.ZedGraphCtrl.SaveIn(lOutputFolder & "GraphTime1.png")

        lZGC.Dispose()
        lGraphTime.Dispose()
        lZGC = atcGraph.CreateZgc
        lZGC.Width = 1024
        lZGC.Height = 768
        lDataGroup = lDataGroup.FindData("CONSTITUENT", "FLOW")
        lGraphTime = New atcGraph.clsGraphTime(lDataGroup, lZGC)
        lGraphTime.ZedGraphCtrl.SaveIn(lOutputFolder & "GraphTime1x.emf")
        lGraphTime.ZedGraphCtrl.SaveIn(lOutputFolder & "GraphTime1x.png")

        Dim lDataTree As New atcDataTree.atcDataTreePlugin
        lDataTree.Save(lDataGroup, lOutputFolder & "DataTree.txt")

        Dim lList As New atcList.atcListPlugin
        'TODO: set what to list explicitly here
        lList.Save(lDataGroup, lOutputFolder & "List.txt")

        'Dim lStats As New atcSWSTAT.clsSWSTATPlugin
        'lStats.Save(lDataGroup, lOutputFolder & "Stats.txt")

        Return True
    End Function

    Private Function SnapShotAndSave(ByVal aSnapShotName As String) As Boolean
        Dim lSnapShotName As String = aSnapShotName
        If Not lSnapShotName.ToLower.EndsWith(".png") Then lSnapShotName &= ".png"
        SnapShot(lSnapShotName)
        Logger.Dbg("ProjectSavedAndMapSnapshot:" & pMapWin.Project.Save(pMapWin.Project.FileName) & ":" & lSnapShotName)
        pMapWin.StatusBar.Item(1).Text = IO.Path.GetFileNameWithoutExtension(lSnapShotName)
        Return True
    End Function

    Private Function SnapShot(ByVal aSnapShotName As String) As Boolean
        With pMapWin.View
            Dim lImage As MapWinGIS.Image = .Snapshot(.Extents)
            Dim lDrawImg As System.Drawing.Image = MapWinUtility.ImageUtils.ObjectToImage(lImage.Picture)
            Dim lBaseFolder As String = ""
            If IO.Directory.Exists("snapshots") Then lBaseFolder = "snapshots\"
            lDrawImg.Save(lBaseFolder & aSnapShotName, System.Drawing.Imaging.ImageFormat.Png)
            Dim lLegendFileName As String = IO.Path.GetFileNameWithoutExtension(aSnapShotName) & "Legend.png"
            .LegendControl.Snapshot().Save(lBaseFolder & lLegendFileName)
            lLegendFileName = IO.Path.GetFileNameWithoutExtension(aSnapShotName) & "LegendVisible.png"
            .LegendControl.Snapshot(True).Save(lBaseFolder & lLegendFileName)
        End With
        Return True
    End Function

End Module
