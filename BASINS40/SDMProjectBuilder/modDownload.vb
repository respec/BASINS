Option Strict Off
Option Explicit On

Imports MapWinGIS
Imports MapWindow.Interfaces
Imports System.Collections.Specialized
Imports System.Windows.Forms.Application
Imports MapWinUtility
Imports MapWinUtility.Strings
Imports atcData
Imports atcUtility
Imports atcMwGisUtility

''' <summary>
''' 
''' </summary>
''' <remarks></remarks>
Public Module modDownload

    Private Const XMLappName As String = "SDMProjectBuilder"
    Private SDMPluginNames() As String = {"SDMProjectBuilder", _
                                          "Tiled Map", _
                                          "Timeseries::Statistics", _
                                          "Timeseries::WDM", _
                                          "D4EM Data Download::Main", _
                                          "D4EM Data Download::BASINS", _
                                          "D4EM Data Download::NHDPlus", _
                                          "D4EM Data Download::NLCD2001" _
                                          }

    Private Function GetSelectedRegion() As String
        Try
            Dim lNumSelected As Integer = g_MapWin.View.SelectedShapes.NumSelected
            If lNumSelected = 1 Then
                Dim lXML As String = ""
                Dim lThemeTag As String = ""
                Dim lFieldName As String = ""
                Dim lField As Integer
                Dim lFieldMatch As Integer = -1
                Dim lCurLayer As MapWinGIS.Shapefile
                lCurLayer = g_MapWin.Layers.Item(g_MapWin.Layers.CurrentLayer).GetObject

                lFieldName = DBFKeyFieldName(lCurLayer.Filename).ToLower
                lThemeTag = DBFThemeFieldName(lCurLayer.Filename).ToLower

                For lField = 0 To lCurLayer.NumFields - 1
                    If lCurLayer.Field(lField).Name.ToLower = lFieldName Then
                        lFieldMatch = lField
                    End If
                Next

                With g_MapWin.View.SelectedShapes.SelectBounds
                    lXML &= "  <northbc>" & .yMax & "</northbc>" & vbCrLf
                    lXML &= "  <southbc>" & .yMin & "</southbc>" & vbCrLf
                    lXML &= "  <eastbc>" & .xMax & "</eastbc>" & vbCrLf
                    lXML &= "  <westbc>" & .xMin & "</westbc>" & vbCrLf
                    lXML &= "  <projection>" & g_MapWin.Project.ProjectProjection & "</projection>" & vbCrLf
                End With

                If lFieldMatch >= 0 Then
                    For lSelected As Integer = 0 To lNumSelected - 1
                        Dim lShapeIndex As Integer = g_MapWin.View.SelectedShapes.Item(lSelected).ShapeIndex()
                        lXML &= "  <" & lThemeTag & " status=""set by " & XMLappName & """>" & lCurLayer.CellValue(lFieldMatch, lShapeIndex) & "</" & lThemeTag & ">" & vbCrLf
                    Next
                End If
                If lXML.Length > 0 Then
                    Return "<region>" & vbCrLf & lXML & "</region>" & vbCrLf
                End If
            End If
        Catch e As Exception
            Logger.Dbg("Exception getting selected region: " & e.Message)
        End Try
        Return ""
    End Function

    'Private Function GetHucRegion(ByVal aHUC As String) As String
    '    Try
    '        Dim lXML As String = ""
    '        Dim lThemeTag As String = "HUC" & aHUC.Length

    '        'TODO: add bounding box of HUC shape?
    '        'With g_MapWin.View.SelectedShapes.SelectBounds
    '        '    lXML &= "  <northbc>" & .yMax & "</northbc>" & vbCrLf
    '        '    lXML &= "  <southbc>" & .yMin & "</southbc>" & vbCrLf
    '        '    lXML &= "  <eastbc>" & .xMax & "</eastbc>" & vbCrLf
    '        '    lXML &= "  <westbc>" & .xMin & "</westbc>" & vbCrLf
    '        lXML &= "  <projection>" & D4EMDataManager.SpatialOperations.AlbersProjections(0) & "</projection>" & vbCrLf
    '        'End With

    '        lXML &= "  <" & lThemeTag & " status=""set by " & XMLappName & """>" & aHUC & "</" & lThemeTag & ">" & vbCrLf

    '        If aHUC.Length > 8 Then
    '            lXML &= "  <HUC8>" & aHUC.Substring(0, 8) & "</HUC8>" & vbCrLf
    '        End If

    '        If lXML.Length > 0 Then
    '            Return "<region>" & vbCrLf & lXML & "</region>" & vbCrLf
    '        End If
    '    Catch e As Exception
    '        Logger.Dbg("Exception getting selected region: " & e.Message)
    '    End Try
    '    Return ""
    'End Function

    Private Sub CopyFeaturesWithinExtent(ByVal aOldFolder As String, ByVal aNewFolder As String)
        'copy features that are within selected indexes of selected layer to new folder
        Dim i As Integer

        'build collection of selected shapes
        Dim lSelectedLayer As Integer = GisUtil.CurrentLayer
        Dim lSelectedShapeIndexes As New Collection
        For i = 1 To GisUtil.NumSelectedFeatures(lSelectedLayer)
            lSelectedShapeIndexes.Add(GisUtil.IndexOfNthSelectedFeatureInLayer(i - 1, lSelectedLayer))
        Next

        Dim lCurrentSf As MapWinGIS.Shapefile
        Dim lResultSf As MapWinGIS.Shapefile
        Dim lNewName As String

        Dim lExtentShape As MapWinGIS.Shape = Nothing
        Dim lResultShape As MapWinGIS.Shape = Nothing
        Dim lExtentsSf As New MapWinGIS.Shapefile
        Dim lExtentsFileName As String = GisUtil.LayerFileName(lSelectedLayer)
        If lExtentsSf.Open(lExtentsFileName) Then
            lExtentShape = lExtentsSf.Shape(lSelectedShapeIndexes(1))
            For i = 2 To lSelectedShapeIndexes.Count
                MapWinGeoProc.SpatialOperations.MergeShapes(lExtentShape, lExtentsSf.Shape(lSelectedShapeIndexes(i)), lResultShape)
                lExtentShape = lResultShape
            Next
        End If

        For iLayer As Integer = 0 To GisUtil.NumLayers - 1
            If iLayer <> lSelectedLayer Then
                If FilenameNoPath(GisUtil.LayerFileName(iLayer)) <> "wdm.shp" And FilenameNoPath(GisUtil.LayerFileName(iLayer)) <> "metpt.shp" Then
                    'want to keep met pts
                    Logger.Status("Selecting " & GisUtil.LayerName(iLayer))
                    RefreshView()
                    Logger.Dbg("CopyFeaturesWithinExtent:" & GisUtil.LayerName(iLayer))

                    If InStr(GisUtil.LayerFileName(iLayer), aOldFolder) > 0 Then
                        'this layer is in our project folder

                        lNewName = ReplaceString(GisUtil.LayerFileName(iLayer), aOldFolder, aNewFolder)
                        Dim lLayerType As Integer = GisUtil.LayerType(iLayer)

                        Dim rslt As Boolean = False
                        If lLayerType = 1 Or lLayerType = 2 Or lLayerType = 3 Then
                            'point, line, or polygon
                            lCurrentSf = New MapWinGIS.Shapefile
                            lResultSf = New MapWinGIS.Shapefile
                            If lCurrentSf.Open(GisUtil.LayerFileName(iLayer)) Then
                                If lResultSf.CreateNew(lNewName, lCurrentSf.ShapefileType) Then
                                    rslt = False
                                    If lLayerType = 1 Then
                                        Logger.Dbg("  SelectingPoints")
                                        rslt = MapWinGeoProc.Selection.SelectPointsWithPolygon(lCurrentSf, lExtentShape, lResultSf)
                                        Logger.Dbg("  FinishedSelectingPoints")
                                    ElseIf lLayerType = 3 Then
                                        Logger.Dbg("  SelectingPolygons")
                                        rslt = MapWinGeoProc.Selection.SelectPolygonsWithPolygon(lCurrentSf, lExtentShape, lResultSf)
                                        Logger.Dbg("  FinishedSelectingPolygons")
                                    ElseIf lLayerType = 2 Then
                                        Logger.Dbg("  SelectingLines")
                                        rslt = MapWinGeoProc.Selection.SelectLinesWithPolygon(lCurrentSf, lExtentShape, lResultSf)
                                        Logger.Dbg("  FinishedSelectingLines")
                                    End If
                                    If rslt Then
                                        lResultSf.SaveAs(lNewName)
                                    End If
                                    lResultSf.Close()
                                End If
                                lCurrentSf.Close()
                            End If

                        ElseIf lLayerType = 4 Then
                            'grid, need to clip to extents

                            Dim lFileName As String = GisUtil.LayerFileName(iLayer)
                            If Not MapWinGeoProc.SpatialOperations.ClipGridWithPolygon(lFileName, lExtentShape, lNewName, True) Then
                                Logger.Dbg("RemoveFeaturesBeyondExtent:ClipGridFailed:" & MapWinGeoProc.Error.GetLastErrorMsg)
                            End If
                        End If
                    End If
                End If
            End If
        Next iLayer
        Logger.Status("")
        RefreshView()
    End Sub

    Public Function SpecifyAndCreateNewProject() As String
        Logger.Status("LABEL TITLE " & g_AppNameShort & " Status")
        Dim lBuildForm As frmBuildNew = pBuildFrm 'Save local reference for FindText, but forget pBuildFrm
        pBuildFrm = Nothing

        Logger.Dbg("SpecifyAndCreateNewProject")
        Logger.Dbg("  MinCatchmentKM2 " & g_MinCatchmentKM2)
        Logger.Dbg("  MinFlowlineKM " & g_MinFlowlineKM)
        Logger.Dbg("  IgnoreBelowFrac " & g_LandUseIgnoreBelowFraction)
        Logger.Dbg("  SWATDatabaseName " & g_SWATDatabaseName)
        Logger.Dbg("  DoHSPF,SWAT " & g_DoHSPF & " " & g_DoSWAT)

        Dim lNationalProjectFilename As String = g_MapWin.Project.FileName.Clone
        Dim lNationalHuc8Filename As String = g_MapWin.Layers(Huc8Layer).FileName
        Dim lParametersFilename As String = IO.Path.Combine(IO.Path.GetDirectoryName(lNationalProjectFilename), PARAMETER_FILE)
        Dim lCreatedMapWindowProjectFilename As String = ""

        'Save national project as the user has adjusted it
        g_MapWin.Project.Save(g_MapWin.Project.FileName)

        'Save parameters before starting to create project(s)
        WriteParametersTextFile(lParametersFilename, g_MapWin.Project.FileName)

        Dim lRegion As String = ""
        If g_HucList IsNot Nothing AndAlso g_HucList.Count > 0 Then
            Dim lHucIndex As Integer = 0
            For Each lHuc As String In g_HucList
                Logger.Progress("Creating project for " & lHuc, lHucIndex, g_HucList.Count)
                Using lLevel As New ProgressLevel(True)
                    If lBuildForm.FindText(lHuc) Then
                        lRegion = GetSelectedRegion()
                        If String.IsNullOrEmpty(lRegion) Then
                            Logger.Dbg("Region not found for " & lHuc)
                        Else
                            lCreatedMapWindowProjectFilename = CreateNewProjectAndDownloadCoreDataInteractive(lRegion)

                            If IO.File.Exists(lCreatedMapWindowProjectFilename) Then
                                Logger.Status("Finished Building " & g_MapWin.Project.FileName, True)
                                WriteParametersTextFile(lParametersFilename, lCreatedMapWindowProjectFilename)
                                IO.File.Copy(lParametersFilename, IO.Path.Combine(IO.Path.GetDirectoryName(g_MapWin.Project.FileName), PARAMETER_FILE))
                            End If
                        End If
                    Else
                        Logger.Dbg("Could not find region '" & lHuc)
                    End If
                End Using
                lHucIndex += 1
                g_MapWin.Layers.Clear()
                If lHucIndex < g_HucList.Count Then
                    'g_MapWin.Project.Load(lNationalProjectFilename)
                    g_MapWin.Layers.Add(lNationalHuc8Filename)
                    g_MapWin.Project.ProjectProjection = g_MapWin.Layers(Huc8Layer).Projection
                    g_MapWin.Project.Save(IO.Path.GetDirectoryName(lNationalProjectFilename) & g_PathChar & "huc8only.mwprj")
                End If
            Next
            Logger.Msg("Finished Building " & lHucIndex & " Projects", g_AppNameLong)
            lCreatedMapWindowProjectFilename = ""
        Else
            lRegion = GetSelectedRegion()
            If String.IsNullOrEmpty(lRegion) Then
                Logger.Msg("Exactly one HUC-8 or HUC-12 must be selected to build a project")
            Else
                lCreatedMapWindowProjectFilename = CreateNewProjectAndDownloadCoreDataInteractive(lRegion)

                If IO.File.Exists(lCreatedMapWindowProjectFilename) Then
                    Logger.Status("")
                    Logger.Msg("Finished Building " & g_MapWin.Project.FileName, g_AppNameLong)
                    WriteParametersTextFile(lParametersFilename, lCreatedMapWindowProjectFilename)
                    IO.File.Copy(lParametersFilename, IO.Path.Combine(IO.Path.GetDirectoryName(g_MapWin.Project.FileName), PARAMETER_FILE))
                End If
            End If
        End If
        Return lCreatedMapWindowProjectFilename
    End Function

    'Returns file name of new project or "" if not built
    Friend Function CreateNewProjectAndDownloadCoreDataInteractive(ByVal aRegion As String) As String
        Dim lDataPath As String
        Dim lDefaultProjectFileName As String
        Dim lNoData As Boolean = False
        Dim lDefDirName As String = "NewProject"
        Dim lMyProjection As String
        Dim lAreaOfInterestProjection As String = ""

        Logger.Dbg("Region " & aRegion)
StartOver:
        lDataPath = IO.Path.Combine(g_ProgramDir, "data\") 'TODO: save in desired location for FRAMES

        If aRegion.Length > 0 Then
            Dim lRegionXML As New Xml.XmlDocument

            With lRegionXML
                .LoadXml(aRegion)
                For Each lChild As Xml.XmlNode In .ChildNodes(0).ChildNodes
                    If lChild.InnerText.Length > 0 Then
                        Select Case lChild.Name.ToLower
                            Case "northbc", "top"
                            Case "southbc", "bottom"
                            Case "westbc", "left"
                            Case "eastbc", "right"
                            Case "projection", "boxprojection"
                                lAreaOfInterestProjection = lChild.InnerText
                            Case Else
                                'Name project after selected area, use more specific (12-digit) HUC if also have less specific (8-digit) one
                                If lDefDirName = "NewProject" OrElse lChild.InnerText.Length > lDefDirName.Length Then
                                    lDefDirName = lChild.InnerText
                                ElseIf Not lDefDirName.StartsWith(lChild.InnerText) Then
                                    lDefDirName = "Multiple" 'More than one different area selected
                                End If
                        End Select
                    End If
                Next
            End With
        End If

        If lDefDirName = "NewProject" Then
            'If lNoData Then
            '    'Already came through here, don't ask again
            'Else
            '    lNoData = True
            '    If Logger.Msg("No features have been selected.  Do you wish to create a project with no data?", MsgBoxStyle.YesNo, "Data Extraction") = MsgBoxResult.No Then
            Logger.Msg("Choose Build only after selecting a HUC-12 or HUC-8", MsgBoxStyle.OkOnly, "Build")
            Return ""
            '    End If
            'End If
        End If

        lDefaultProjectFileName = CreateDefaultNewProjectFileName(lDataPath, lDefDirName)
        lDefDirName = PathNameOnly(lDefaultProjectFileName)
        Logger.Dbg("CreateNewProjectDirectory:" & lDefDirName)
        IO.Directory.CreateDirectory(lDefDirName)

        Dim lProjectFileName As String
        If g_HucList Is Nothing Then
            lProjectFileName = PromptForNewProjectFileName(lDefDirName, lDefaultProjectFileName)
        Else
            lProjectFileName = lDefaultProjectFileName
        End If

        If lProjectFileName.Length = 0 Then
            Return ""
        Else
            Dim lNewDataDir As String = PathNameOnly(lProjectFileName) & g_PathChar
            If Not lNewDataDir.ToLower.StartsWith(lDefDirName.ToLower) Then
                'If the user did not choose the default folder or a subfolder of it
                'Remove speculatively created folder since they chose something else
                Try
                    System.IO.Directory.Delete(lDefDirName, False) 'Cancelled save dialog or changed directory
                    Logger.Dbg("RemoveSpeculativeProjectDir:" & lDefDirName)
                Catch ex As Exception
                    Logger.Dbg("CreateNewProjectAndDownloadCoreDataInteractive: Could not delete " & lDefDirName & vbCr & ex.Message)
                End Try
            End If

            'Check to see if chosen data dir already contains any files
            If IO.Directory.Exists(lNewDataDir) Then
                Dim lNumFiles As Long = System.IO.Directory.GetFiles(lNewDataDir).LongLength
                Dim lNumDirs As Long = System.IO.Directory.GetDirectories(lNewDataDir).LongLength
                If lNumFiles + lNumDirs > 0 Then
                    Dim lMessage As String = "The folder '" & lNewDataDir & "'" & vbCr & "already contains "
                    If lNumFiles > 0 Then
                        lMessage &= lNumFiles & " files"
                        If lNumDirs > 0 Then lMessage &= " and "
                    End If
                    If lNumDirs > 0 Then lMessage &= lNumDirs & " folders"
                    Logger.Msg(lMessage & "." & vbCr & "The folder must be empty before a new project can be created here.", "New Project")
                    GoTo StartOver
                End If
            End If

            lMyProjection = D4EMDataManager.SpatialOperations.AlbersProjections(0) 'atcProjector.Methods.AskUser
            If lMyProjection.Length = 0 Then 'Cancelled projection specification dialog
                Try 'remove already created data directory
                    System.IO.Directory.Delete(lNewDataDir, False)
                    Logger.Dbg("RemoveProjectDir:" & lNewDataDir & " dueToProjectionCancel")
                Catch ex As Exception
                    Logger.Dbg("CreateNewProjectAndDownloadCoreDataInteractive: Could not delete " & lNewDataDir & vbCr & ex.Message)
                End Try
                Return ""
            Else
                Logger.Dbg("Projection:" & lMyProjection)
                SaveFileString(lNewDataDir & "prj.proj", lMyProjection) 'Side effect: makes data directory
                If lNoData Then
                    Logger.Dbg("EmptyProjectCreated")
                    ClearLayers()
                    g_MapWin.PreviewMap.GetPictureFromMap()
                    g_MapWin.Project.Save(lProjectFileName)
                    g_MapWin.Project.Modified = True
                    g_MapWin.Project.Save(lProjectFileName)
                    g_MapWin.Project.Modified = False
                Else
                    'download and project batch data
                    CreateNewProjectAndDownloadBatchData(aRegion, lDataPath, lNewDataDir, lProjectFileName, lAreaOfInterestProjection, lMyProjection)
                End If
                Return lProjectFileName
            End If
        End If
    End Function

    Private Sub CopyFromIfNeeded(ByVal aFilename As String, ByVal aFromDir As String, ByVal aToDir As String)
        If Not IO.File.Exists(aToDir & aFilename) Then
            If IO.File.Exists(aFromDir & aFilename) Then
                IO.File.Copy(aFromDir & aFilename, aToDir & aFilename)
            Else
                Logger.Dbg("Could not copy " & aFilename & " from " & aFromDir & " to " & aToDir)
            End If
        End If
    End Sub

    Public Sub EnsureGlobalCacheSet(ByVal aDataPath As String)
        If g_CacheFolder Is Nothing OrElse g_CacheFolder.Length = 0 OrElse Not IO.Directory.Exists(g_CacheFolder) Then
            g_CacheFolder = aDataPath.TrimEnd(g_PathChar)
            Dim lDataPos As Integer = g_CacheFolder.IndexOf(g_PathChar & "data" & g_PathChar)
            If lDataPos >= 0 Then
                g_CacheFolder = IO.Path.Combine(g_CacheFolder.Substring(0, lDataPos), "cache")
            Else
                If IsNumeric(IO.Path.GetFileName(g_CacheFolder)) Then
                    g_CacheFolder = IO.Path.GetDirectoryName(g_CacheFolder)
                End If
                If IO.Directory.Exists(IO.Path.Combine(IO.Path.GetDirectoryName(g_CacheFolder), "cache")) Then
                    g_CacheFolder = IO.Path.Combine(IO.Path.GetDirectoryName(g_CacheFolder), "cache")
                Else
                    g_CacheFolder = IO.Path.Combine(g_CacheFolder, "cache")
                End If
            End If
        End If
    End Sub

    'Returns file name of new project or "" if not built
    Public Sub CreateNewProjectAndDownloadBatchData(ByVal aRegion As String, _
                                                    ByVal aDataPath As String, _
                                                    ByVal aNewDataDir As String, _
                                                    ByVal aProjectFileName As String, _
                                                    ByVal aAreaOfInterestProjection As String, _
                                                    ByVal aDesiredProjection As String, _
                                                    Optional ByVal aExistingMapWindowProject As Boolean = False)
        Dim lQuery As String
        Dim lQueries As New Generic.List(Of String)

        EnsureGlobalCacheSet(aDataPath)

        Dim lSelectedHuc As String = GetSelectedHUC()
        Dim lSelectedShape As MapWinGIS.Shape = GetSelectedShape()

        'TODO: & "<DataType>huc12</DataType>" _

        lQuery = "<function name='GetBASINS'>" _
               & "<arguments>" _
               & "<DataType>core31</DataType>" _
               & "<SaveIn>" & aNewDataDir & "</SaveIn>" _
               & "<CacheFolder>" & g_CacheFolder & "</CacheFolder>" _
               & "<DesiredProjection>" & aDesiredProjection & "</DesiredProjection>" _
               & aRegion _
               & "<clip>False</clip>" _
               & "<merge>True</merge>" _
               & "<joinattributes>true</joinattributes>" _
               & "</arguments>" _
               & "</function>"
        lQueries.Add(lQuery)

        lQuery = "<function name='GetNHDplus'>" _
               & "<arguments>" _
               & "<DataType>hydrography</DataType>" _
               & "<DataType>Catchment</DataType>" _
               & "<DataType>elev_cm</DataType>" _
               & "<SaveIn>" & aNewDataDir & "</SaveIn>" _
               & "<CacheFolder>" & g_CacheFolder & "</CacheFolder>" _
               & "<DesiredProjection>" & aDesiredProjection & "</DesiredProjection>" _
               & aRegion _
               & "<clip>False</clip>" _
               & "<merge>False</merge>" _
               & "<joinattributes>true</joinattributes>" _
               & "</arguments>" _
               & "</function>"
        lQueries.Add(lQuery)

        lQuery = "<function name='GetNLCD2001'>" _
               & "<arguments>" _
               & "<DataType>LandCover</DataType>" _
               & "<SaveIn>" & aNewDataDir & "</SaveIn>" _
               & "<CacheFolder>" & g_CacheFolder & "</CacheFolder>" _
               & "<DesiredProjection>" & aDesiredProjection & "</DesiredProjection>" _
               & aRegion _
               & "<clip>False</clip>" _
               & "<merge>False</merge>" _
               & "<joinattributes>true</joinattributes>" _
               & "</arguments>" _
               & "</function>"
        lQueries.Add(lQuery)

        Dim lRegion As String = aRegion
        lRegion = aRegion.Substring(0, aRegion.IndexOf("<projection>")) & "<preferredformat>" & "Closest" & "</preferredformat>" & aRegion.Substring(aRegion.IndexOf("<projection>"))
        lQuery = "<function name='GetBASINS'>" _
               & "<arguments>" _
               & "<DataType>met</DataType>" _
               & "<SaveIn>" & aNewDataDir & "</SaveIn>" _
               & "<SaveWDM>met.wdm</SaveWDM>" _
               & "<CacheFolder>" & g_CacheFolder & "</CacheFolder>" _
               & "<DesiredProjection>" & aDesiredProjection & "</DesiredProjection>" _
               & lRegion _
               & "<clip>True</clip>" _
               & "<merge>False</merge>" _
               & "</arguments>" _
               & "</function>"
        lQueries.Add(lQuery)

        UnloadPlugin("Tiled Map")
        Dim lDownloadManager As D4EMDataManager.DataManager = CreateDataManager()
        Dim lStepCount As Integer = lQueries.Count + 4
        If g_DoHSPF Then lStepCount += 1
        If g_DoSWAT Then lStepCount += 1

        Logger.Progress(1, lStepCount)
        Dim lResult As String = ""
        For Each lQuery In lQueries
            Dim lLabelStart As Integer = lQuery.IndexOf("'Get") + 4
            Logger.Status("Downloading " & lQuery.Substring(lLabelStart, lQuery.IndexOf("'>") - lLabelStart))
            Using lLevel As New ProgressLevel(True)
RetryQuery:
                Dim lThisResult As String = lDownloadManager.Execute(lQuery)
                If lThisResult.Contains("<error>") Then
                    Select Case Logger.Msg(lThisResult.Replace("<error>", "").Replace("</error>", "") & vbCrLf _
                                           & vbCrLf _
                                           & "Abort building this project" & vbCrLf _
                                           & "Retry this download" & vbCrLf _
                                           & "or Ignore the error and continue to build an incomplete project", MsgBoxStyle.AbortRetryIgnore, "Download Error")
                        Case MsgBoxResult.Abort
                            Logger.Progress("", 0, 0)
                            lResult = Nothing
                            Exit For
                        Case MsgBoxResult.Retry
                            GoTo RetryQuery
                    End Select
                Else
                    lResult &= lThisResult
                End If
            End Using
        Next

        'Logger.Msg(lResult, "Result of Query from DataManager")

        If lResult IsNot Nothing AndAlso lResult.Length > 0 AndAlso lResult.StartsWith("<success>") Then
            Logger.Status("Download Successful, Building Project")
            Using lLevel As New ProgressLevel(True)
                If Not aExistingMapWindowProject Then
                    'regular case, not coming from existing mapwindow project
                    ClearLayers()
                    If Not (g_MapWin.Project.Save(aProjectFileName)) Then
                        Logger.Dbg("CreateNewProjectAndDownloadBatchData:Save1Failed:" & g_MapWin.LastError)
                    End If
                Else
                    'open existing mapwindow project again
                    g_MapWin.Project.Load(aProjectFileName)
                    Dim lProjectDir As String = PathNameOnly(aProjectFileName)
                    Dim lNewShapeName As String = lProjectDir & "\temp\tempextent.shp"
                    TryDeleteShapefile(lNewShapeName)
                End If

                UnloadPlugin("Tiled Map")

                're-project selected shape aoi if necessary, and add it to the map
                Dim lSelectedSf As New MapWinGIS.Shapefile
                Dim lAOIName As String = aNewDataDir & "aoi.shp"
                TryDeleteShapefile(lAOIName)
                If lSelectedSf.CreateNew(lAOIName, ShpfileType.SHP_POLYGON) Then
                    'add an id field to the new shapefile
                    Dim lField As New MapWinGIS.Field
                    lField.Name = "ID"
                    lField.Type = MapWinGIS.FieldType.STRING_FIELD
                    lField.Width = 10
                    lSelectedSf.StartEditingTable()
                    Dim lBsuc As Boolean = lSelectedSf.EditInsertField(lField, 0)
                    lSelectedSf.StopEditingTable()
                    'now insert the shape
                    If lSelectedSf.StartEditingShapes(True) Then
                        If lSelectedSf.EditInsertShape(lSelectedShape, 0) Then
                            If aAreaOfInterestProjection <> aDesiredProjection Then
                                If MapWinGeoProc.SpatialReference.ProjectShapefile(aAreaOfInterestProjection, aDesiredProjection, lSelectedSf) Then
                                    If lSelectedSf.Open(lAOIName) Then
                                        lSelectedShape = lSelectedSf.Shape(0)
                                        GisUtil.AddLayer(lAOIName, "Area_of_Interest")
                                        Dim lAOIIndex As Integer = GisUtil.LayerIndex(lAOIName)
                                        GisUtil.DrawFill(lAOIIndex) = False
                                        GisUtil.LayerVisible(lAOIIndex) = True
                                        GisUtil.ZoomToLayerExtents(lAOIIndex)
                                        RefreshView()
                                        g_MapWin.PreviewMap.Update(MapWindow.Interfaces.ePreviewUpdateExtents.CurrentMapView)
                                        DoEvents()
                                    End If
                                End If
                            End If
                        End If
                    End If
                End If
            End Using

            g_MapWin.Project.Modified = True
            Logger.Status("Processing Downloaded Data")
            Using lLevel As New ProgressLevel(True)
                ProcessDownloadResults(lResult) 'TODO: skip message box describing what has been downloaded?
            End Using
            AddAllShapesInDir(aNewDataDir, aNewDataDir)
            g_MapWin.PreviewMap.Update(MapWindow.Interfaces.ePreviewUpdateExtents.CurrentMapView)
            If Not aExistingMapWindowProject Then
                'regular case, not coming from existing mapwindow project
                'set mapwindow project projection to projection of first layer
                g_MapWin.Project.ProjectProjection = aDesiredProjection

                If Not (g_MapWin.Project.Save(aProjectFileName)) Then
                    Logger.Dbg("CreateNewProjectAndDownloadBatchData:Save2Failed:" & g_MapWin.LastError)
                End If
            End If
            g_MapWin.Project.Save(aProjectFileName)

            'process the network 
            Logger.Status("Processing Network")
            Dim lSimplifiedFlowlinesFileName As String = ""
            Dim lSimplifiedCatchmentsFileName As String = ""
            Using lLevel As New ProgressLevel(True)
                ProcessNetwork(lSelectedHuc, lSelectedShape, aNewDataDir, lSimplifiedFlowlinesFileName, lSimplifiedCatchmentsFileName)
                If lSimplifiedFlowlinesFileName.Length > 0 Then
                    GisUtil.AddLayerToGroup(lSimplifiedFlowlinesFileName, "Simplified_Flowlines", "Data Layers")
                    GisUtil.LayerVisible(GisUtil.LayerIndex(lSimplifiedFlowlinesFileName)) = True
                Else
                    lSimplifiedFlowlinesFileName = GisUtil.LayerFileName("Flowline Features")
                End If
                If lSimplifiedCatchmentsFileName.Length > 0 Then
                    GisUtil.AddLayerToGroup(lSimplifiedCatchmentsFileName, "Simplified_Catchments", "Data Layers")
                    GisUtil.LayerVisible(GisUtil.LayerIndex(lSimplifiedCatchmentsFileName)) = True
                Else
                    lSimplifiedCatchmentsFileName = GisUtil.LayerFileName("Catchment")
                End If
            End Using
            g_MapWin.Project.Save(aProjectFileName)

            Dim lElevationFileName As String = GisUtil.LayerFileName("NHDPlus Elevation")
            Dim lLandUseFileName As String = GisUtil.LayerFileName("NLCD 2001 Landcover")

            'if building hspf project, do this:
            If g_DoHSPF Then
                Dim lMetWDMFileName As String = PathNameOnly(GisUtil.LayerFileName("Weather Station Sites 2006")) & "\met.wdm"
                Logger.Status("Creating HSPF input sequence")
                Using lLevel As New ProgressLevel(True)
                    BatchHSPF.BatchHSPF(lSimplifiedCatchmentsFileName, lSimplifiedFlowlinesFileName, _
                                        lLandUseFileName, lElevationFileName, _
                                        lMetWDMFileName, aNewDataDir, lSelectedHuc)
                End Using
            End If

            'if building swat project, do this:
            If g_DoSWAT Then
                Logger.Status("Creating SWAT input sequence")
                Using lLevel As New ProgressLevel(True)
                    BatchSWAT(lSelectedHuc, aNewDataDir, lSimplifiedCatchmentsFileName, lSimplifiedFlowlinesFileName, _
                              lLandUseFileName, lElevationFileName, lSelectedShape)
                End Using
            End If
            g_MapWin.Project.Save(aProjectFileName)

        End If
    End Sub

    Friend Function CreateDataManager() As D4EMDataManager.DataManager
        UnloadNonSDMPlugins()
        LoadSDMPlugins()
        Dim lPlugins As New ArrayList
        For lPluginIndex As Integer = 0 To g_MapWin.Plugins.Count
            Try
                If Not g_MapWin.Plugins.Item(lPluginIndex) Is Nothing Then
                    lPlugins.Add(g_MapWin.Plugins.Item(lPluginIndex))
                End If
            Catch ex As Exception
            End Try
        Next
        Return New D4EMDataManager.DataManager(lPlugins)
    End Function

    Friend Sub UnloadPlugin(ByVal aPluginName As String)
        Dim lKey As String = g_MapWin.Plugins.GetPluginKey(aPluginName)
        If Not String.IsNullOrEmpty(lKey) Then
            Logger.Dbg("Unloading " & aPluginName & "=" & lKey)
            g_MapWin.Plugins.StopPlugin(lKey)
        End If
    End Sub

    Friend Sub UnloadNonSDMPlugins()
        For lPluginIndex As Integer = g_MapWin.Plugins.Count - 1 To 0 Step -1
            Dim lPlugin As MapWindow.Interfaces.IPlugin = g_MapWin.Plugins.Item(lPluginIndex)
            If lPlugin IsNot Nothing AndAlso Array.IndexOf(SDMPluginNames, lPlugin.Name) < 0 Then
                UnloadPlugin(lPlugin.Name)
            End If
        Next
    End Sub

    Friend Sub LoadSDMPlugins()
        Dim lNumSDMPlugins As Integer = SDMPluginNames.Length
        Logger.Dbg("Load " & lNumSDMPlugins & " SDM Plugins")
        For lPluginIndex As Integer = 0 To lNumSDMPlugins - 1
            Logger.Dbg("LoadSDMPlugin " & lPluginIndex)
            Logger.Dbg("LoadSDMPlugin " & SDMPluginNames(lPluginIndex))
            atcDataManager.LoadPlugin(SDMPluginNames(lPluginIndex))
            Logger.Dbg("LoadedSDMPlugin " & SDMPluginNames(lPluginIndex))
        Next
        Logger.Dbg("LoadedSDMPlugins")
    End Sub

    'Download new data for an existing project
    Public Sub DownloadNewData(ByRef aProjectDir As String)
        Dim lDownloadFilename As String = aProjectDir & "download.xml"
        SaveFileString(lDownloadFilename, "<clsWebDataManager>" & vbCrLf & _
                                          " <status_variables>" & vbCrLf & _
                                          "  <launched_by>" & XMLappName & "</launched_by>" & vbCrLf & _
                                          "  <project_dir status=""set by " & XMLappName & """>" & aProjectDir & "</project_dir>" & vbCrLf & _
                                          " </status_variables>" & vbCrLf & _
                                          "</clsWebDataManager>")

        Dim lProjectorFilename As String = PathNameOnly(aProjectDir.Substring(0, aProjectDir.Length - 1)) & "\ATCProjector.xml"
        If FileExists(lProjectorFilename) Then
            Kill(lProjectorFilename)
        End If

        If DataDownload(lDownloadFilename) Then
            ProcessDownloadResults(lProjectorFilename)
        End If

        If FileExists(lProjectorFilename) Then
            Kill(lProjectorFilename)
        End If
    End Sub

    Public Sub ProcessDownloadResults(ByVal aInstructions As String)
        Dim lXmlInstructions As New Xml.XmlDocument
        lXmlInstructions.LoadXml("<AllInstructions>" & aInstructions & "</AllInstructions>")
        Dim lMessage As String = ""
        Windows.Forms.Application.DoEvents()
        For Each lInstructionNode As Xml.XmlNode In lXmlInstructions.ChildNodes(0).ChildNodes
            lMessage &= ProcessDownloadResult(lInstructionNode.OuterXml) & vbCrLf
            Windows.Forms.Application.DoEvents()
        Next
        'If lMessage.Length > 2 Then
        '    Logger.Msg(lMessage, "Data Download")
        '    If Logger.DisplayMessageBoxes AndAlso lMessage.Contains(" Data file") Then
        '        atcDataManager.UserManage()
        '    End If
        'End If
    End Sub

    Private Function ProcessDownloadResult(ByVal aInstructions As String) As String
        Logger.Dbg("ProcessDownloadResult: " & aInstructions)
        Dim lProjectorNode As Xml.XmlNode
        Dim lInputDirName As String
        Dim lOutputDirName As String
        Dim lOutputFileName As String
        Dim InputFileList As New NameValueCollection
        Dim lFileObject As Object
        Dim lCurFilename As String
        Dim lDefaultsXML As Xml.XmlDocument = Nothing
        Dim lSuccess As Boolean
        Dim lInputProjection As String
        Dim lOutputProjection As String
        Dim lLayersAdded As New ArrayList
        Dim lDataAdded As New ArrayList
        Dim lProjectDir As String = ""

        ProcessDownloadResult = ""

        If g_MapWin IsNot Nothing AndAlso g_MapWin.Project IsNot Nothing AndAlso g_MapWin.Project.FileName IsNot Nothing AndAlso g_MapWin.Project.FileName.Contains(g_PathChar) Then
            lProjectDir = IO.Path.GetDirectoryName(g_MapWin.Project.FileName)
            If lProjectDir.Length > 0 AndAlso Not lProjectDir.EndsWith(g_PathChar) Then lProjectDir &= g_PathChar
        End If

        If Not aInstructions.StartsWith("<") Then
            If FileExists(aInstructions) Then
                aInstructions = WholeFileString(aInstructions)
            Else
                Logger.Dbg("No instructions to process")
                Exit Function
            End If
        End If

        Dim lInstructionsXML As New Xml.XmlDocument
        lInstructionsXML.LoadXml(aInstructions)
        Dim lInstructionsNode As Xml.XmlNode = lInstructionsXML.FirstChild
        If lInstructionsNode.Name.ToLower = "success" Then
            g_MapWin.View.LockLegend()
            g_MapWin.View.MapCursor = tkCursor.crsrWait
            For Each lProjectorNode In lInstructionsNode.ChildNodes
                Logger.Dbg("Processing XML: " & lProjectorNode.OuterXml)
                If lProjectorNode.Name <> "message" Then Logger.Status(ReadableFromXML(lProjectorNode.OuterXml))
                Select Case LCase(lProjectorNode.Name.ToLower)
                    Case "add_data"
                        Dim lDataType As String = lProjectorNode.Attributes.GetNamedItem("type").InnerText
                        lOutputFileName = lProjectorNode.InnerText
                        If lDataType.Length = 0 Then
                            Logger.Dbg("Could not add data from '" & lOutputFileName & "' - no type specified")
                        Else
                            Dim lNewDataSource As atcData.atcTimeseriesSource = atcData.atcDataManager.DataSourceByName(lDataType)
                            If lNewDataSource Is Nothing Then 'Try loading needed plugin for this data type
                                If atcData.atcDataManager.LoadPlugin("Timeseries::" & lDataType) Then
                                    'Try again with newly loaded plugin
                                    lNewDataSource = atcData.atcDataManager.DataSourceByName(lDataType)
                                    If lNewDataSource IsNot Nothing Then Logger.Dbg("Successfully loaded plugin " & lDataType & " to read " & lOutputFileName)
                                Else
                                    Logger.Dbg("Could not add data from '" & lOutputFileName & "' - unknown type '" & lDataType & "'")
                                End If
                            End If
                            If lNewDataSource IsNot Nothing Then
                                If atcData.atcDataManager.OpenDataSource(lNewDataSource, lOutputFileName, Nothing) Then
                                    lDataAdded.Add(lNewDataSource.Specification)
                                End If
                            End If
                        End If
                    Case "add_shape"
                        lOutputFileName = lProjectorNode.InnerText
                        If lDefaultsXML Is Nothing Then lDefaultsXML = GetDefaultsXML()
                        Dim lLayer As MapWindow.Interfaces.Layer = AddShapeToMW(lOutputFileName, GetDefaultsFor(lOutputFileName, lProjectDir, lDefaultsXML))
                        If lLayer Is Nothing Then
                            Logger.Msg("Failed add shape layer '" & lOutputFileName & "'")
                        Else
                            lLayersAdded.Add(lLayer.Name)
                        End If
                    Case "add_grid"
                        lOutputFileName = lProjectorNode.InnerText
                        '    Select Case IO.Path.GetFileName(lOutputFileName).ToLower
                        '        Case "fac.tif", "fdr.tif", "cat.tif"
                        '            Logger.Dbg("Skipping adding grid to MapWindow: " & lOutputFileName)
                        '            GetDefaultRenderer(lOutputFileName)
                        '            lMessage &= IO.Path.GetFileName(lOutputFileName) & " available, not added to map" & vbCrLf
                        '        Case Else
                        If lDefaultsXML Is Nothing Then lDefaultsXML = GetDefaultsXML()
                        Dim lLayer As MapWindow.Interfaces.Layer = AddGridToMW(lOutputFileName, GetDefaultsFor(lOutputFileName, lProjectDir, lDefaultsXML))
                        If lLayer Is Nothing Then
                            Logger.Msg(lOutputFileName, "Failed add grid layer")
                        Else
                            lLayersAdded.Add(lLayer.Name)
                        End If
                        If Not FileExists(FilenameNoExt(lOutputFileName) & ".prj") Then
                            'create .prj file as work-around for bug
                            SaveFileString(FilenameNoExt(lOutputFileName) & ".prj", "")
                        End If
                        '    End Select
                    Case "add_allshapes"
                        lOutputFileName = lProjectorNode.InnerText
                        lLayersAdded.AddRange(AddAllShapesInDir(lOutputFileName, lProjectDir))
                    Case "remove_data"
                        atcDataManager.RemoveDataSource(lProjectorNode.InnerText.ToLower)
                    Case "remove_layer", "remove_shape", "remove_grid"
                        Try
                            atcMwGisUtility.GisUtil.RemoveLayer(atcMwGisUtility.GisUtil.LayerIndex(lProjectorNode.InnerText))
                        Catch ex As Exception
                            Logger.Dbg("Error removing layer '" & lProjectorNode.InnerText & "': " & ex.Message)
                        End Try
                    Case "clip_grid"
                        lOutputFileName = lProjectorNode.Attributes.GetNamedItem("output").InnerText
                        lCurFilename = lProjectorNode.InnerText
                        'create extents shape to clip to, at the extents of the cat unit
                        Dim lShape As New MapWinGIS.Shape
                        Dim lExtentsSf As New MapWinGIS.Shapefile
                        lShape.Create(ShpfileType.SHP_POLYGON)
                        Dim lSf As New MapWinGIS.Shapefile
                        lSf.Open(lProjectDir & "cat.shp")
                        Dim lPoint1 As New MapWinGIS.Point
                        lPoint1.x = lSf.Extents.xMin
                        lPoint1.y = lSf.Extents.yMin
                        lSuccess = lShape.InsertPoint(lPoint1, 0)
                        Dim lPoint2 As New MapWinGIS.Point
                        lPoint2.x = lSf.Extents.xMax
                        lPoint2.y = lSf.Extents.yMin
                        lSuccess = lShape.InsertPoint(lPoint2, 0)
                        Dim lPoint3 As New MapWinGIS.Point
                        lPoint3.x = lSf.Extents.xMax
                        lPoint3.y = lSf.Extents.yMax
                        lSuccess = lShape.InsertPoint(lPoint3, 0)
                        Dim lPoint4 As New MapWinGIS.Point
                        lPoint4.x = lSf.Extents.xMin
                        lPoint4.y = lSf.Extents.yMax
                        lSuccess = lShape.InsertPoint(lPoint4, 0)
                        Dim lPoint5 As New MapWinGIS.Point
                        lPoint5.x = lSf.Extents.xMin
                        lPoint5.y = lSf.Extents.yMin
                        lSuccess = lShape.InsertPoint(lPoint5, 0)
                        If InStr(lOutputFileName, "\nlcd" & g_PathChar) > 0 Then
                            'project the extents into the albers projection for nlcd
                            MkDirPath(lProjectDir & "nlcd")
                            TryDeleteShapefile(lProjectDir & "nlcd\catextent.shp")
                            lSuccess = lExtentsSf.CreateNew(lProjectDir & "nlcd\catextent.shp", ShpfileType.SHP_POLYGON)
                            lSuccess = lExtentsSf.StartEditingShapes(True)
                            lSuccess = lExtentsSf.EditInsertShape(lShape, 0)
                            lSuccess = lExtentsSf.Close()
                            lInputProjection = WholeFileString(lProjectDir & "prj.proj")
                            lInputProjection = CleanUpUserProjString(lInputProjection)
                            lOutputProjection = "+proj=aea +ellps=GRS80 +lon_0=-96 +lat_0=23.0 +lat_1=29.5 +lat_2=45.5 +x_0=0 +y_0=0 +datum=NAD83 +units=m"
                            If lInputProjection <> lOutputProjection Then
                                lSuccess = MapWinGeoProc.SpatialReference.ProjectShapefile(lInputProjection, lOutputProjection, lExtentsSf)
                                lSuccess = lExtentsSf.Open(lProjectDir & "nlcd\catextent.shp")
                                lShape = lExtentsSf.Shape(0)
                            End If
                        End If
                        'clip to cat extents
                        Logger.Status("Clipping Grid...")
                        RefreshView()
                        DoEvents()
                        If Not FileExists(FilenameNoExt(lCurFilename) & ".prj") Then
                            'create .prj file as work-around for clipping bug
                            SaveFileString(FilenameNoExt(lCurFilename) & ".prj", "")
                        End If
                        Logger.Dbg("ClipGridWithPolygon: " & lCurFilename & " " & lProjectDir & "nlcd\catextent.shp" & " " & lOutputFileName & " " & "True")
                        lSuccess = MapWinGeoProc.SpatialOperations.ClipGridWithPolygon(lCurFilename, lShape, lOutputFileName, True)
                        lSuccess = lExtentsSf.Close
                        Logger.Status("")
                    Case "project_dir"
                        lProjectDir = lProjectorNode.InnerText
                        If Not lProjectDir.EndsWith(g_PathChar) Then lProjectDir &= g_PathChar
                    Case "convert_shape"
                        lOutputFileName = lProjectorNode.Attributes.GetNamedItem("output").InnerText
                        lCurFilename = lProjectorNode.InnerText
                        ShapeUtilMerge(lCurFilename, lOutputFileName, lProjectDir & "prj.proj")
                        'attempt to assign prj file
                        lOutputProjection = WholeFileString(lProjectDir & "prj.proj")
                        lOutputProjection = CleanUpUserProjString(lOutputProjection)
                        Dim sf As New MapWinGIS.Shapefile
                        sf.Open(lOutputFileName, Nothing)
                        sf.Projection = lOutputProjection
                        sf.Close()
                        'if adding to some specific point layers in basins we need to refresh that map layer
                        If Right(lOutputFileName, 8) = "pcs3.shp" Or _
                           Right(lOutputFileName, 8) = "gage.shp" Or _
                           Right(lOutputFileName, 9) = "wqobs.shp" Then
                            'get handle of this layer
                            Dim lLayerHandle As Integer = LayerHandle(lOutputFileName)
                            If lLayerHandle > -1 Then
                                Dim llayername As String = g_MapWin.Layers(lLayerHandle).Name
                                Dim lRGBcolor As Integer = RGB(g_MapWin.Layers(lLayerHandle).Color.R, g_MapWin.Layers(lLayerHandle).Color.G, g_MapWin.Layers(lLayerHandle).Color.B)
                                Dim lmarkersize As Integer = g_MapWin.Layers(lLayerHandle).LineOrPointSize
                                Dim ltargroup As Integer = g_MapWin.Layers(lLayerHandle).GroupHandle
                                Dim lnewpos As Integer = g_MapWin.Layers(lLayerHandle).GroupPosition
                                Dim MWlay As MapWindow.Interfaces.Layer
                                Dim shpFile As MapWinGIS.Shapefile
                                g_MapWin.Layers.Remove(lLayerHandle)
                                shpFile = New MapWinGIS.Shapefile
                                shpFile.Open(lOutputFileName)
                                MWlay = g_MapWin.Layers.Add(shpFile, llayername, lRGBcolor, lRGBcolor, lmarkersize)
                                g_MapWin.Layers.MoveLayer(MWlay.Handle, lnewpos, ltargroup)
                            End If
                        End If
                    Case "convert_grid"
                        lOutputFileName = lProjectorNode.Attributes.GetNamedItem("output").InnerText
                        lCurFilename = lProjectorNode.InnerText
                        'remove output file
                        TryDelete(lOutputFileName)
                        lOutputProjection = WholeFileString(lProjectDir & "prj.proj")
                        lOutputProjection = CleanUpUserProjString(lOutputProjection)
                        If InStr(lOutputFileName, "\nlcd" & g_PathChar) > 0 Then
                            'exception for nlcd data, already in albers
                            lInputProjection = "+proj=aea +ellps=GRS80 +lon_0=-96 +lat_0=23.0 +lat_1=29.5 +lat_2=45.5 +x_0=0 +y_0=0 +datum=NAD83 +units=m"
                            If lOutputProjection = "+proj=aea +ellps=clrk66 +lon_0=-96 +lat_0=23.0 +lat_1=29.5 +lat_2=45.5 +x_0=0 +y_0=0 +datum=NAD83 +units=m" Then
                                'special exception, don't bother to reproject with only this slight datum shift
                                lInputProjection = "+proj=aea +ellps=clrk66 +lon_0=-96 +lat_0=23.0 +lat_1=29.5 +lat_2=45.5 +x_0=0 +y_0=0 +datum=NAD83 +units=m"
                            End If
                        Else
                            lInputProjection = "+proj=longlat +datum=NAD83"
                        End If
                        If lInputProjection = lOutputProjection Then
                            System.IO.File.Copy(lCurFilename, lOutputFileName)
                        Else
                            'project it
                            Logger.Status("Projecting Grid...")
                            RefreshView()
                            DoEvents()
                            lSuccess = MapWinGeoProc.SpatialReference.ProjectGrid(lInputProjection, lOutputProjection, lCurFilename, lOutputFileName, True)
                            Logger.Status("")
                            If Not FileExists(FilenameNoExt(lOutputFileName) & ".prj") Then
                                'create .prj file as work-around for bug
                                SaveFileString(FilenameNoExt(lOutputFileName) & ".prj", "")
                            End If
                            If Not lSuccess Then
                                Logger.Msg("Failed to project grid" & vbCrLf & MapWinGeoProc.Error.GetLastErrorMsg, "ProcessProjectorFile")
                                System.IO.File.Copy(lCurFilename, lOutputFileName)
                            End If
                        End If
                    Case "convert_dir"
                        'loop through a directory, projecting all files in it
                        lInputDirName = lProjectorNode.InnerText
                        lOutputDirName = lProjectorNode.Attributes.GetNamedItem("output").InnerText
                        If lOutputDirName Is Nothing OrElse lOutputDirName.Length = 0 Then
                            lOutputDirName = lInputDirName
                        End If
                        If Right(lOutputDirName, 1) <> g_PathChar Then lOutputDirName &= g_PathChar

                        InputFileList.Clear()

                        AddFilesInDir(InputFileList, lInputDirName, False, "*.shp")

                        lOutputProjection = WholeFileString(lProjectDir & "prj.proj")
                        lOutputProjection = CleanUpUserProjString(lOutputProjection)
                        Dim sf As New MapWinGIS.Shapefile

                        For Each lFileObject In InputFileList
                            lCurFilename = lFileObject
                            If (FileExt(lCurFilename) = "shp") Then
                                'this is a shapefile
                                lOutputFileName = lOutputDirName & FilenameNoPath(lCurFilename)
                                'change projection and merge
                                If (FileExists(lOutputFileName) And (InStr(1, lOutputFileName, "\landuse" & g_PathChar) > 0)) Then
                                    'if the output file exists and it is a landuse shape, dont bother
                                Else
                                    ShapeUtilMerge(lCurFilename, lOutputFileName, lProjectDir & "prj.proj")
                                    'attempt to assign prj file
                                    sf.Open(lOutputFileName, Nothing)
                                    sf.Projection = lOutputProjection
                                    sf.Close()
                                End If
                            End If
                        Next lFileObject
                    Case "message"
                        Logger.Msg(lProjectorNode.InnerText)
                    Case "select_layer"
                        Dim lLayerName As String = lProjectorNode.InnerText
                        Try
                            Dim lLayerIndex As Integer = atcMwGisUtility.GisUtil.LayerIndex(lLayerName)
                            atcMwGisUtility.GisUtil.CurrentLayer = lLayerIndex
                            Logger.Dbg("Selected layer '" & lLayerName & "'")
                        Catch
                            Logger.Dbg("Failed to select layer '" & lLayerName & "'")
                        End Try
                    Case Else
                        Logger.Msg("Cannot yet follow directive:" & vbCr & lProjectorNode.Name, "ProcessProjectorFile")
                End Select
            Next

            RemoveEmptyGroups()

            If lLayersAdded.Count = 1 Then
                ProcessDownloadResult &= "Downloaded Layer: " & lLayersAdded(0).ToString
            ElseIf lLayersAdded.Count > 1 Then
                ProcessDownloadResult &= "Downloaded " & lLayersAdded.Count & " Layers"
            End If

            If lLayersAdded.Count > 0 AndAlso lDataAdded.Count > 0 Then
                ProcessDownloadResult &= vbCrLf
            End If

            If lDataAdded.Count = 1 Then
                ProcessDownloadResult &= "Downloaded Data file: " & lDataAdded(0).ToString
            ElseIf lDataAdded.Count > 1 Then
                ProcessDownloadResult &= "Downloaded " & lDataAdded.Count & " Data files"
            End If

            g_MapWin.View.MapCursor = tkCursor.crsrMapDefault
            g_MapWin.View.UnlockLegend()
        Else
            ProcessDownloadResult &= lInstructionsNode.Name & ": " & lInstructionsNode.InnerXml
        End If
        Logger.Status("")
    End Function

    Public Function LayerHandle(ByVal aLayerFilename As String) As Integer
        aLayerFilename = aLayerFilename.ToLower
        For lLayerIndex As Integer = g_MapWin.Layers.NumLayers - 1 To 0 Step -1
            Dim lLayerHandle As Integer = g_MapWin.Layers.GetHandle(lLayerIndex)
            Dim lLayer As Layer = g_MapWin.Layers(lLayerHandle)
            If Not (lLayer Is Nothing) AndAlso lLayer.FileName.ToLower = aLayerFilename Then
                Return lLayerHandle
            End If
        Next
        Return -1
    End Function

    Public Function CleanUpUserProjString(ByVal aProjString As String) As String
        Dim lPos As Integer
        Dim lFirst As Boolean

        Do While Mid(aProjString, 1, 1) = "#"
            'eliminate comment lines at beginning
            lPos = InStr(aProjString, vbCrLf)
            aProjString = Mid(aProjString, lPos + 2)
        Loop
        lFirst = True
        Do While InStr(aProjString, vbCrLf) > 0
            'strip out unneeded stuff
            lPos = InStr(aProjString, vbCrLf)
            If lFirst Then
                aProjString = Mid(aProjString, lPos + 2)
                lFirst = False
            Else
                aProjString = Mid(aProjString, 1, lPos - 1) & " " & Mid(aProjString, lPos + 2)
            End If
        Loop
        If InStr(aProjString, " end ") > 0 Then
            aProjString = Mid(aProjString, 1, InStr(aProjString, " end ") - 1)
        End If
        If Len(aProjString) > 0 Then
            If Mid(aProjString, 1, 9) = "+proj=dd " Then
                aProjString = "+proj=longlat " & Mid(aProjString, 10)
                aProjString = aProjString & " +datum=NAD83"
            Else
                aProjString = aProjString & " +datum=NAD83 +units=m"
            End If
        Else
            aProjString = "<none>"
        End If
        CleanUpUserProjString = aProjString
    End Function

    Public Function CreateDefaultNewProjectFileName(ByVal aDataPath As String, ByVal aDefDirName As String) As String
        Dim lSuffix As Integer = 1
        Dim lDirName As String = aDataPath & aDefDirName
        While FileExists(lDirName, True)  'Find a suffix that will make name unique
            If IO.Directory.Exists(lDirName) AndAlso _
               IO.Directory.GetFiles(lDirName).Length = 0 AndAlso _
               IO.Directory.GetDirectories(lDirName).Length = 0 Then
                Exit While 'Go ahead and use existing empty directory
            End If
            lSuffix += 1
            lDirName = aDataPath & aDefDirName & "-" & lSuffix
        End While
        If lSuffix > 1 Then 'Also add suffix to project file name
            Return IO.Path.Combine(lDirName, aDefDirName & "-" & lSuffix & ".mwprj")
        Else
            Return IO.Path.Combine(lDirName, aDefDirName & ".mwprj")
        End If
    End Function

    Public Function PromptForNewProjectFileName(ByVal aDefDirName As String, ByVal aDefaultProjectFileName As String) As String
        'prompt user for new name
        Dim lSaveDialog As New Windows.Forms.SaveFileDialog
        Dim lChosenFileName As String = ""
        With lSaveDialog
            .Title = "Save new project as..."
            .CheckPathExists = False
            .FileName = aDefaultProjectFileName
            .DefaultExt = ".mwprj"
            If .ShowDialog() <> Windows.Forms.DialogResult.OK Then
                TryDelete(aDefDirName, False) 'Cancelled save dialog
                Logger.Dbg("CreateNewProject:CANCELLED")
            Else
                lChosenFileName = .FileName
                Dim lNewDataDir As String = PathNameOnly(lChosenFileName) & g_PathChar
                Logger.Dbg("CreateNewProjectDir:" & lNewDataDir)
                Logger.Dbg("CreateNewProjectName:" & lChosenFileName)
                'Make sure lSaveDialog is not holding a reference to the file so we can delete the dir if needed
                lSaveDialog.FileName = g_PathChar
                lSaveDialog.Dispose()
                'If the user did not choose the default folder or a subfolder of it
                If Not lNewDataDir.ToLower.StartsWith(aDefDirName.ToLower) Then
                    'Remove speculatively created folder since they chose something else
                    Try
                        System.IO.Directory.Delete(aDefDirName, False) 'Cancelled save dialog or changed directory
                        Logger.Dbg("RemoveSpeculativeProjectDir:" & aDefDirName)
                    Catch ex As Exception
                        Logger.Dbg("CreateNewProjectAndDownloadCoreDataInteractive: Could not delete " & aDefDirName & vbCr & ex.Message)
                    End Try
                End If
            End If
        End With
        Return lChosenFileName
    End Function

    Private Sub ShapeUtilMerge(ByVal aCurFilename As String, ByVal aOutputFilename As String, ByVal aProjectionFilename As String)
        Dim lShapeUtilExe As String = FindFile("Please locate ShapeUtil.exe", "\basins\etc\datadownload\ShapeUtil.exe")
        If lShapeUtilExe.Length > 0 Then
            Dim lLayersDbf As String = GetSetting("ShapeMerge", "files", "layers.dbf")
            If Not FileExists(lLayersDbf) Then
                Logger.Dbg("Did not find layers.dbf in registry " & lLayersDbf)
                lLayersDbf = PathNameOnly(lShapeUtilExe) & "\layers.dbf"
                If FileExists(lLayersDbf) Then
                    Logger.Dbg("Saving layers.dbf location for ShapeUtil: " & lLayersDbf)
                    SaveSetting("ShapeMerge", "files", "layers.dbf", lLayersDbf)
                Else
                    Logger.Dbg("Did not find layers.dbf in same path as ShapeUtil " & lLayersDbf)
                End If
                'Else
                '   Logger.Dbg("Found " & lLayersDbf)
            End If
            'Logger.Msg("MSG2 Merging " & aCurFilename)
            'Logger.Msg("MSG6 into " & aOutputFilename)
            Shell(lShapeUtilExe & " """ & aOutputFilename & """ """ & aCurFilename & """ """ & aProjectionFilename & """", AppWinStyle.NormalNoFocus, True)
        Else
            Logger.Dbg("Failed to find ShapeUtil.exe for merging " & aCurFilename & " into " & aOutputFilename)
        End If
    End Sub

    Private Function DataDownload(ByRef aCommandLine As String) As Boolean
        Dim lDataDownloadExe As String = FindFile("Please locate DataDownload.exe", "\Basins\etc\DataDownload\DataDownload.exe")
        If lDataDownloadExe.Length > 0 Then
            If Shell(lDataDownloadExe & " " & aCommandLine, AppWinStyle.NormalFocus, True) = 0 Then
                Return True
            Else
                Return False
            End If
        End If
        '        Dim pManager As WebDataManager.clsWebDataManager
        '        Dim curStep As String

        '        On Error GoTo ErrHand

        '        curStep = "Set pManager = New clsWebDataManager"
        '        pManager = New WebDataManager.clsWebDataManager
        '        'Set pManager.Logger = glogger

        '        curStep = "CurrentStatusFromFile '" & aCommandLine & "'"
        '        If Len(aCommandLine) > 0 Then pManager.CurrentStatusFromFile(aCommandLine)

        '        curStep = "Manager.SelectDataType"
        '        pManager.SelectDataType()
        '        While pManager.State < 1000
        '            System.Windows.Forms.Application.DoEvents()
        '            System.Threading.Thread.Sleep(50)
        '        End While

        '        Exit Sub

        'ErrHand:
        '        Logger.Msg(curStep & vbCr & Err.Description, MsgBoxStyle.OKOnly, "BASINS Data Download Main")
    End Function

    Private Function GetDefaultsXML() As Xml.XmlDocument
        Dim lDefaultsXML As Xml.XmlDocument = Nothing
        Dim lDefaultsPath As String 'full file name of defaults XML
        lDefaultsPath = FindFile("Please Locate BasinsDefaultLayers.xml", "\basins\etc\BasinsDefaultLayers.xml")
        If FileExists(lDefaultsPath) Then
            lDefaultsXML = New Xml.XmlDocument
            lDefaultsXML.Load(lDefaultsPath)
        End If
        Return lDefaultsXML
    End Function

    'Adds all shape files found in aPath to the current MapWindow project
    Public Function AddAllShapesInDir(ByVal aPath As String, ByVal aProjectDir As String) As atcCollection
        Dim lLayer As Integer
        Dim Filename As String
        Dim allFiles As New NameValueCollection
        Dim lDefaultsXML As Xml.XmlDocument = GetDefaultsXML()

        Logger.Dbg("AddAllShapesInDir: '" & aPath & "'")

        If Right(aPath, 1) <> g_PathChar Then aPath = aPath & g_PathChar
        AddFilesInDir(allFiles, aPath, True, "*.shp")

        AddAllShapesInDir = New atcCollection
        For lLayer = 0 To allFiles.Count - 1
            Filename = allFiles.Item(lLayer)
            AddAllShapesInDir.Add(Filename, AddShapeToMW(Filename, GetDefaultsFor(Filename, aProjectDir, lDefaultsXML)))
        Next

        RemoveEmptyGroups()
        g_MapWin.PreviewMap.GetPictureFromMap()

    End Function

    ''' <summary>
    ''' Remove any groups from the legend that do not contain any layers 
    ''' for example, groups "Data Layers" and "New Group" are often created but then no layers end up in them
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub RemoveEmptyGroups()
        For lGroupIndex As Integer = g_MapWin.Layers.Groups.Count - 1 To 0 Step -1
            Dim lGroup As LegendControl.Group = g_MapWin.Layers.Groups.ItemByPosition(lGroupIndex)
            Try
                If lGroup IsNot Nothing AndAlso lGroup.LayerCount = 0 Then
                    g_MapWin.Layers.Groups.Remove(lGroup.Handle)
                End If
            Catch e As Exception
                Logger.Dbg("Could not remove group", e.Message, e.StackTrace)
            End Try
        Next
    End Sub

    ''' <summary>
    ''' If the named layer does not have a .mwsr (for shape) or .mwleg (for grid) then look for the default file
    ''' and put a copy of the default renderer with the layer
    ''' </summary>
    Private Function GetDefaultRenderer(ByVal aLayerFilename As String) As String
        GetDefaultRenderer = ""

        If aLayerFilename IsNot Nothing AndAlso aLayerFilename.Length > 0 Then
            Dim lRendererExt As String
            If IO.Path.GetExtension(aLayerFilename).ToLower = ".shp" Then
                lRendererExt = ".mwsr"
            Else
                lRendererExt = ".mwleg"
            End If

            If lRendererExt IsNot Nothing Then
                Dim lRendererFilename As String = IO.Path.ChangeExtension(aLayerFilename, lRendererExt)

                If lRendererFilename.Length > 0 AndAlso Not IO.File.Exists(lRendererFilename) Then
                    Dim lRendererFilenameNoPath As String = IO.Path.GetFileName(lRendererFilename)
                    Dim lRenderersPath As String = g_ProgramDir & "etc\renderers" & g_PathChar
                    Dim lDefaultRendererFilename As String = FindFile("", lRenderersPath & lRendererFilenameNoPath)
                    If Not FileExists(lDefaultRendererFilename) Then
                        If lRendererFilenameNoPath.Contains("_") Then 'Some layers are named huc8_xxx.shp, renderer is named _xxx & lRendererExt
                            lDefaultRendererFilename = FindFile("", lRenderersPath & lRendererFilenameNoPath.Substring(lRendererFilenameNoPath.IndexOf("_")))
                        End If
                        If Not FileExists(lDefaultRendererFilename) Then 'Try trimming off numbers before extension in layername2.mwleg
                            lDefaultRendererFilename = FindFile("", lRenderersPath & IO.Path.GetFileNameWithoutExtension(aLayerFilename).TrimEnd("0"c, "1"c, "2"c, "3"c, "4"c, "5"c, "6"c, "7"c, "8"c, "9"c) & lRendererExt)
                        End If
                    End If
                    If FileExists(lDefaultRendererFilename) Then
                        IO.File.Copy(lDefaultRendererFilename, lRendererFilename)
                        GetDefaultRenderer = lRendererFilename
                    End If
                End If
            End If
        End If
    End Function

    'Given a file name and the XML describing how to render it, add a shape layer to MapWindow
    Private Function AddShapeToMW(ByVal aFilename As String, _
                                  ByRef layerXml As Xml.XmlNode) As MapWindow.Interfaces.Layer
        Dim lFileName As String = aFilename.ToLower
        Dim LayerName As String
        Dim Group As String = ""
        Dim Visible As Boolean

        'Don't add layer again if we already have it
        Dim lLayerHandle As Integer = LayerHandle(lFileName)
        If lLayerHandle >= 0 Then Return g_MapWin.Layers(lLayerHandle)

        Dim MWlay As MapWindow.Interfaces.Layer = Nothing
        Dim shpFile As MapWinGIS.Shapefile

        Try
            Dim lRendererName As String = GetDefaultRenderer(aFilename)

            LayerName = IO.Path.GetFileNameWithoutExtension(aFilename)
            If layerXml Is Nothing Then
                Visible = True
                If lRendererName.Length = 0 Then
                    Group = "Other"
                End If
            Else
                Group = "Other"
                Visible = False
                For Each lAttribute As Xml.XmlAttribute In layerXml.Attributes
                    Select Case lAttribute.Name.ToLower
                        Case "name" : LayerName = lAttribute.InnerText
                        Case "group" : Group = lAttribute.InnerText
                        Case "visible" : Visible = (lAttribute.InnerText.ToLower = "yes") OrElse (lAttribute.InnerText.ToLower = "true")
                    End Select
                Next
            End If

            Logger.Status("Opening " & aFilename)
            shpFile = New MapWinGIS.Shapefile
            If shpFile.Open(aFilename) Then
                MWlay = g_MapWin.Layers.Add(shpFile, LayerName)
            Else
                Logger.Dbg("Could not open shape file '" & aFilename & "' (" & shpFile.ErrorMsg(shpFile.LastErrorCode) & ")")
            End If
            If Not MWlay Is Nothing Then
                If lRendererName.Length > 0 Then
                    MWlay.LoadShapeLayerProps(lRendererName)
                    Dim lRendererXML As New Xml.XmlDocument
                    lRendererXML.Load(lRendererName)
                    Dim lLayerXML As Xml.XmlNode = lRendererXML.GetElementsByTagName("SFRendering")(0).ChildNodes(0)
                    If lLayerXML.Attributes("GroupName") IsNot Nothing Then
                        Group = lLayerXML.Attributes("GroupName").InnerText
                    End If
                    If lLayerXML.Attributes("Visible") IsNot Nothing Then
                        Visible = CBool(lLayerXML.Attributes("Visible").InnerText)
                    End If
                End If
                MWlay.Visible = Visible

                'TODO: replace hard-coded SetLandUseColors and others with full renderer from defaults
                If lFileName.IndexOf(g_PathChar & "landuse" & g_PathChar) > 0 Then
                    SetLandUseColors(MWlay, shpFile)
                ElseIf lFileName.IndexOf(g_PathChar & "nhd" & g_PathChar) > 0 Then
                    If InStr(IO.Path.GetFileNameWithoutExtension(shpFile.Filename), "NHD") > 0 Then
                        MWlay.Name = IO.Path.GetFileNameWithoutExtension(shpFile.Filename)
                    Else
                        MWlay.Name &= " " & IO.Path.GetFileNameWithoutExtension(shpFile.Filename)
                    End If
                ElseIf lFileName.IndexOf(g_PathChar & "census" & g_PathChar) > 0 Then
                    SetCensusColors(MWlay, shpFile)
                ElseIf lFileName.IndexOf(g_PathChar & "dem" & g_PathChar) > 0 Then
                    SetDemColors(MWlay, shpFile)
                ElseIf lFileName.EndsWith("huc12.shp") Then
                    Dim lHUC8 As String = IO.Path.GetFileName(IO.Path.GetDirectoryName(shpFile.Filename))
                    If lHUC8.Length = 8 AndAlso Not MWlay.Name.Contains(lHUC8) Then MWlay.Name &= " in " & lHUC8
                End If
                If Group.Length > 0 Then
                    AddLayerToGroup(MWlay, Group)
                End If

                If MWlay.Visible Then
                    g_MapWin.View.Redraw()
                    DoEvents()
                End If
                g_MapWin.Project.Modified = True
            End If
        Catch ex As Exception
            Logger.Msg("Could not add '" & aFilename & "' to the project. " & ex.ToString & vbCr & ex.StackTrace, "Add Shape")
        End Try
        Logger.Status("")

        Return MWlay
    End Function

    'Given a file name and the XML describing how to render it, add a grid layer to MapWindow
    Private Function AddGridToMW(ByVal aFilename As String, _
                                 ByRef layerXml As Xml.XmlNode) As MapWindow.Interfaces.Layer
        Dim LayerName As String
        Dim Group As String = "Other"
        Dim Visible As Boolean
        'Dim Style As atcRenderStyle = New atcRenderStyle

        'Don't add layer again if we already have it
        Dim lLayerHandle As Integer = LayerHandle(aFilename)
        If lLayerHandle >= 0 Then Return g_MapWin.Layers(lLayerHandle)

        Dim MWlay As MapWindow.Interfaces.Layer = Nothing
        Dim g As MapWinGIS.Grid

        Try
            GetDefaultRenderer(aFilename)

            Logger.Status("Opening " & aFilename)

            If layerXml Is Nothing Then
                MWlay = g_MapWin.Layers.Add(aFilename)
                MWlay.Visible = True
            Else
                LayerName = layerXml.Attributes.GetNamedItem("Name").InnerText
                'Style.xml = layerXml.FirstChild
                Group = layerXml.Attributes.GetNamedItem("Group").InnerText
                If Group Is Nothing Then Group = "Other"
                Select Case layerXml.Attributes.GetNamedItem("Visible").InnerText.ToLower
                    Case "yes" : Visible = True
                    Case "no" : Visible = False
                    Case Else : Visible = False
                End Select
                If LayerName = "" Then LayerName = IO.Path.GetFileNameWithoutExtension(aFilename)
                If LayerName = "National Elevation Dataset" OrElse LayerName = "DEM Elevation Model" Then
                    LayerName &= " (" & IO.Path.GetFileNameWithoutExtension(aFilename) & ")"
                End If
                g = New MapWinGIS.Grid
                g.Open(aFilename)
                'TODO: ensure correct Nodata value is set in NLCD grids
                'If aFilename.ToLower.Contains(g_PathChar & "nlcd" & g_PathChar) Then
                '    If g.Header.NodataValue <> 0 Then
                '        g.Header.NodataValue = 0
                '        Try
                '            g.Save(aFilename)
                '            g.Close()
                '            g.Open(aFilename)
                '        Catch e As Exception
                '            Logger.Msg(aFilename & vbLf & e.Message & vbLf & "This may cause unwanted display of areas with missing data", "Unable to set zero as NoData value")
                '        End Try
                '    End If
                'End If

                MWlay = g_MapWin.Layers.Add(g, LayerName)
                MWlay.Visible = Visible
                MWlay.UseTransparentColor = True

                'TODO: replace hard-coded SetElevationGridColors with full renderer from defaults
                If LCase(aFilename).IndexOf("\demg" & g_PathChar) > 0 Then
                    SetElevationGridColors(MWlay, g)
                ElseIf LCase(aFilename).IndexOf("\ned" & g_PathChar) > 0 Then
                    SetElevationGridColors(MWlay, g)
                End If
            End If


            If Not Group Is Nothing AndAlso Group.Length > 0 Then AddLayerToGroup(MWlay, Group)

            If MWlay.Visible Then
                g_MapWin.View.Redraw()
                DoEvents()
            End If
            g_MapWin.Project.Modified = True
        Catch ex As Exception
            Logger.Msg("Could not add '" & aFilename & "' to the project. " & ex.ToString & vbCr & ex.StackTrace, "Add Grid")
        End Try
        Logger.Status("")

        Return MWlay
    End Function

    Private Sub AddLayerToGroup(ByVal aLay As MapWindow.Interfaces.Layer, ByVal aGroupName As String)
        Dim lExistingGroupIndex As Integer = 0
        While lExistingGroupIndex < g_MapWin.Layers.Groups.Count AndAlso _
              g_MapWin.Layers.Groups.ItemByPosition(lExistingGroupIndex).Text <> aGroupName
            lExistingGroupIndex += 1
        End While

        Dim lGroupHandle As Integer
        If lExistingGroupIndex < g_MapWin.Layers.Groups.Count Then
            lGroupHandle = g_MapWin.Layers.Groups.ItemByPosition(lExistingGroupIndex).Handle
        Else
            'TODO: read group order from a file rather than hard-coded array
            Dim lGroupOrder() As String = {"Data Layers", _
                                           "Observed Data Stations", _
                                           "Point Sources & Withdrawals", _
                                           "Hydrology", _
                                           "Political", _
                                           "Census", _
                                           "Transportation", _
                                           "Soil, Land Use/Cover", _
                                           "Elevation", _
                                           "Other"}
            Dim lNewGroupIndex As Integer = Array.IndexOf(lGroupOrder, aGroupName)
            lExistingGroupIndex = g_MapWin.Layers.Groups.Count
            If lNewGroupIndex > 0 Then
                While lExistingGroupIndex > 0 AndAlso _
                      Array.IndexOf(lGroupOrder, _
                                    g_MapWin.Layers.Groups.ItemByPosition(lExistingGroupIndex - 1).Text) < lNewGroupIndex
                    lExistingGroupIndex -= 1
                End While
            End If
            lGroupHandle = g_MapWin.Layers.Groups.Add(aGroupName, lExistingGroupIndex)
        End If

        Select Case aLay.LayerType
            Case MapWindow.Interfaces.eLayerType.Grid, MapWindow.Interfaces.eLayerType.Image, MapWindow.Interfaces.eLayerType.PolygonShapefile
                aLay.MoveTo(0, lGroupHandle) 'move grid/image/polygon to bottom of group
            Case MapWindow.Interfaces.eLayerType.LineShapefile, MapWindow.Interfaces.eLayerType.PointShapefile
                aLay.MoveTo(99, lGroupHandle) 'move line/point layer to top of group
            Case Else
                Logger.Dbg("AddLayerToGroup: Unexpected layer type: " & aLay.LayerType & " for layer " & aLay.Name)
                aLay.MoveTo(0, lGroupHandle)
        End Select
    End Sub

    Private Sub SetLandUseColors(ByVal MWlay As MapWindow.Interfaces.Layer, ByVal shpFile As MapWinGIS.Shapefile)
        Dim colorBreak As MapWinGIS.ShapefileColorBreak
        Dim colorScheme As MapWinGIS.ShapefileColorScheme

        MWlay.Name &= " " & IO.Path.GetFileNameWithoutExtension(shpFile.Filename).Substring(2)
        colorScheme = New MapWinGIS.ShapefileColorScheme
        colorScheme.FieldIndex = ShpFieldNumFromName(shpFile, "lucode")
        If colorScheme.FieldIndex > 0 Then
            colorBreak = New MapWinGIS.ShapefileColorBreak
            colorBreak.Caption = "Urban or Built-up Land"
            colorBreak.StartValue = 1
            colorBreak.EndValue = 19
            colorBreak.StartColor = System.Convert.ToUInt32(RGB(120, 120, 120))
            colorBreak.EndColor = colorBreak.StartColor
            colorScheme.Add(colorBreak)

            colorBreak = New MapWinGIS.ShapefileColorBreak
            colorBreak.Caption = "Agricultural Land"
            colorBreak.StartValue = 20
            colorBreak.EndValue = 29
            colorBreak.StartColor = System.Convert.ToUInt32(RGB(0, 255, 0))
            colorBreak.EndColor = colorBreak.StartColor
            colorScheme.Add(colorBreak)

            colorBreak = New MapWinGIS.ShapefileColorBreak
            colorBreak.Caption = "Rangeland"
            colorBreak.StartValue = 30
            colorBreak.EndValue = 39
            colorBreak.StartColor = System.Convert.ToUInt32(RGB(146, 174, 47))
            colorBreak.EndColor = colorBreak.StartColor
            colorScheme.Add(colorBreak)

            colorBreak = New MapWinGIS.ShapefileColorBreak
            colorBreak.Caption = "Forest Land"
            colorBreak.StartValue = 40
            colorBreak.EndValue = 49
            colorBreak.StartColor = System.Convert.ToUInt32(RGB(161, 102, 50))
            colorBreak.EndColor = colorBreak.StartColor
            colorScheme.Add(colorBreak)

            colorBreak = New MapWinGIS.ShapefileColorBreak
            colorBreak.Caption = "Water"
            colorBreak.StartValue = 50
            colorBreak.EndValue = 59
            colorBreak.StartColor = System.Convert.ToUInt32(RGB(0, 0, 255))
            colorBreak.EndColor = colorBreak.StartColor
            colorScheme.Add(colorBreak)

            colorBreak = New MapWinGIS.ShapefileColorBreak
            colorBreak.Caption = "Wetland"
            colorBreak.StartValue = 60
            colorBreak.EndValue = 69
            colorBreak.StartColor = System.Convert.ToUInt32(RGB(0, 209, 220))
            colorBreak.EndColor = colorBreak.StartColor
            colorScheme.Add(colorBreak)

            colorBreak = New MapWinGIS.ShapefileColorBreak
            colorBreak.Caption = "Barren Land"
            colorBreak.StartValue = 70
            colorBreak.EndValue = 79
            colorBreak.StartColor = System.Convert.ToUInt32(RGB(255, 255, 0))
            colorBreak.EndColor = colorBreak.StartColor
            colorScheme.Add(colorBreak)

            colorBreak = New MapWinGIS.ShapefileColorBreak
            colorBreak.Caption = "Tundra"
            colorBreak.StartValue = 80
            colorBreak.EndValue = 89
            colorBreak.StartColor = System.Convert.ToUInt32(RGB(60, 105, 0))
            colorBreak.EndColor = colorBreak.StartColor
            colorScheme.Add(colorBreak)

            colorBreak = New MapWinGIS.ShapefileColorBreak
            colorBreak.Caption = "Perennial Snow or Ice"
            colorBreak.StartValue = 90
            colorBreak.EndValue = 99
            colorBreak.StartColor = System.Convert.ToUInt32(RGB(210, 210, 210))
            colorBreak.EndColor = colorBreak.StartColor
            colorScheme.Add(colorBreak)

            MWlay.ColoringScheme = colorScheme
            MWlay.DrawFill = True
            MWlay.LineOrPointSize = 0
            MWlay.OutlineColor = System.Drawing.Color.Black
        End If
    End Sub

    Private Sub SetDemColors(ByVal MWlay As MapWindow.Interfaces.Layer, ByVal shpFile As MapWinGIS.Shapefile)
        Dim colorBreak As MapWinGIS.ShapefileColorBreak
        Dim colorScheme As MapWinGIS.ShapefileColorScheme
        Dim minelev As Single
        Dim maxelev As Single
        Dim inc As Single
        Dim i As Integer

        colorScheme = New MapWinGIS.ShapefileColorScheme
        colorScheme.FieldIndex = ShpFieldNumFromName(shpFile, "elev_m")
        'determine min and max elevations
        minelev = 9999999
        maxelev = -9999999
        For i = 1 To shpFile.NumShapes
            If shpFile.CellValue(colorScheme.FieldIndex, i) > maxelev Then
                maxelev = shpFile.CellValue(colorScheme.FieldIndex, i)
            End If
            If shpFile.CellValue(colorScheme.FieldIndex, i) < minelev Then
                minelev = shpFile.CellValue(colorScheme.FieldIndex, i)
            End If
        Next i
        inc = (maxelev - minelev) / 10

        colorBreak = New MapWinGIS.ShapefileColorBreak
        colorBreak.Caption = CStr(minelev) & " - " & CStr(minelev + inc)
        colorBreak.StartValue = minelev
        colorBreak.EndValue = minelev + inc
        colorBreak.StartColor = System.Convert.ToUInt32(32768)
        colorBreak.EndColor = colorBreak.StartColor
        colorScheme.Add(colorBreak)

        colorBreak = New MapWinGIS.ShapefileColorBreak
        colorBreak.Caption = CStr(minelev + inc) & " - " & CStr(minelev + (2 * inc))
        colorBreak.StartValue = minelev + inc
        colorBreak.EndValue = minelev + (2 * inc)
        colorBreak.StartColor = System.Convert.ToUInt32(888090)
        colorBreak.EndColor = colorBreak.StartColor
        colorScheme.Add(colorBreak)

        colorBreak = New MapWinGIS.ShapefileColorBreak
        colorBreak.Caption = CStr(minelev + (2 * inc)) & " - " & CStr(minelev + (3 * inc))
        colorBreak.StartValue = minelev + (2 * inc)
        colorBreak.EndValue = minelev + (3 * inc)
        colorBreak.StartColor = System.Convert.ToUInt32(1743412)
        colorBreak.EndColor = colorBreak.StartColor
        colorScheme.Add(colorBreak)

        colorBreak = New MapWinGIS.ShapefileColorBreak
        colorBreak.Caption = CStr(minelev + (3 * inc)) & " - " & CStr(minelev + (4 * inc))
        colorBreak.StartValue = minelev + (3 * inc)
        colorBreak.EndValue = minelev + (4 * inc)
        colorBreak.StartColor = System.Convert.ToUInt32(2598734)
        colorBreak.EndColor = colorBreak.StartColor
        colorScheme.Add(colorBreak)

        colorBreak = New MapWinGIS.ShapefileColorBreak
        colorBreak.Caption = CStr(minelev + (4 * inc)) & " - " & CStr(minelev + (5 * inc))
        colorBreak.StartValue = minelev + (4 * inc)
        colorBreak.EndValue = minelev + (5 * inc)
        colorBreak.StartColor = System.Convert.ToUInt32(3454056)
        colorBreak.EndColor = colorBreak.StartColor
        colorScheme.Add(colorBreak)

        colorBreak = New MapWinGIS.ShapefileColorBreak
        colorBreak.Caption = CStr(minelev + (5 * inc)) & " - " & CStr(minelev + (6 * inc))
        colorBreak.StartValue = minelev + (5 * inc)
        colorBreak.EndValue = minelev + (6 * inc)
        colorBreak.StartColor = System.Convert.ToUInt32(4309378)
        colorBreak.EndColor = colorBreak.StartColor
        colorScheme.Add(colorBreak)

        colorBreak = New MapWinGIS.ShapefileColorBreak
        colorBreak.Caption = CStr(minelev + (6 * inc)) & " - " & CStr(minelev + (7 * inc))
        colorBreak.StartValue = minelev + (6 * inc)
        colorBreak.EndValue = minelev + (7 * inc)
        colorBreak.StartColor = System.Convert.ToUInt32(5164700)
        colorBreak.EndColor = colorBreak.StartColor
        colorScheme.Add(colorBreak)

        colorBreak = New MapWinGIS.ShapefileColorBreak
        colorBreak.Caption = CStr(minelev + (7 * inc)) & " - " & CStr(minelev + (8 * inc))
        colorBreak.StartValue = minelev + (7 * inc)
        colorBreak.EndValue = minelev + (8 * inc)
        colorBreak.StartColor = System.Convert.ToUInt32(6020022)
        colorBreak.EndColor = colorBreak.StartColor
        colorScheme.Add(colorBreak)

        colorBreak = New MapWinGIS.ShapefileColorBreak
        colorBreak.Caption = CStr(minelev + (8 * inc)) & " - " & CStr(minelev + (9 * inc))
        colorBreak.StartValue = minelev + (8 * inc)
        colorBreak.EndValue = minelev + (9 * inc)
        colorBreak.StartColor = System.Convert.ToUInt32(6875344)
        colorBreak.EndColor = colorBreak.StartColor
        colorScheme.Add(colorBreak)

        colorBreak = New MapWinGIS.ShapefileColorBreak
        colorBreak.Caption = CStr(minelev + (9 * inc)) & " - " & CStr(minelev + (10 * inc))
        colorBreak.StartValue = minelev + (9 * inc)
        colorBreak.EndValue = minelev + (10 * inc)
        colorBreak.StartColor = System.Convert.ToUInt32(7730666)
        colorBreak.EndColor = colorBreak.StartColor
        colorScheme.Add(colorBreak)

        MWlay.ColoringScheme = colorScheme
        MWlay.DrawFill = True
        MWlay.LineOrPointSize = 0
        MWlay.OutlineColor = System.Drawing.Color.Black

    End Sub

    Private Sub SetElevationGridColors(ByVal MWlay As MapWindow.Interfaces.Layer, ByVal g As MapWinGIS.Grid)
        Dim colorScheme As MapWinGIS.GridColorScheme

        colorScheme = New MapWinGIS.GridColorScheme
        colorScheme.UsePredefined(g.Minimum, g.Maximum, PredefinedColorScheme.FallLeaves)
        MWlay.ColoringScheme = colorScheme
    End Sub

    Friend Sub SetCensusRenderer(ByVal MWlay As MapWindow.Interfaces.Layer, Optional ByVal shpFile As MapWinGIS.Shapefile = Nothing)
        If shpFile Is Nothing Then shpFile = MWlay.GetObject

        g_MapWin.View.LockMap() 'keep the map from updating during the loop.

        'NOTE: using atcTableDBF - with 80K records - processing reduced from 3.5 to 2.5 seconds
        Dim lShapeDbf As New atcTableDBF
        lShapeDbf.OpenFile(IO.Path.ChangeExtension(shpFile.Filename, ".dbf"))
        Dim lFieldCFCC As Integer = lShapeDbf.FieldNumber("CFCC")
        'Dim lFieldCFCC As Integer = ShpFieldNumFromName(shpFile, "CFCC")

        Dim lCurrentCFCC As Integer
        Dim lLastShapeIndex As Integer = shpFile.NumShapes - 1
        For iShape As Integer = 0 To lLastShapeIndex
            'lCurrentCFCC = shpFile.CellValue(lFieldCFCC, iShape).ToString.Substring(1)
            lCurrentCFCC = lShapeDbf.Value(lFieldCFCC).Substring(1)
            lShapeDbf.MoveNext()
            Select Case lCurrentCFCC
                Case 4, 41 To 48, 63, 64 'most likely
                Case 1 To 3, 11 To 38
                    MWlay.Shapes(iShape).LineOrPointSize = 2
                Case 2, 21 To 28
                    MWlay.Shapes(iShape).LineOrPointSize = 2
                Case 3, 31 To 38
                    MWlay.Shapes(iShape).LineOrPointSize = 2
                Case Else 'A5, A51, A52, A53, A6, A60, A61, A62, A65, A7, A70, A71, A72, A73, A74
                    MWlay.Shapes(iShape).LineStipple = MapWinGIS.tkLineStipple.lsDotted
            End Select
        Next

        g_MapWin.View.UnlockMap() 'let the map redraw again
    End Sub

    Private Sub SetCensusColors(ByVal MWlay As MapWindow.Interfaces.Layer, ByVal shpFile As MapWinGIS.Shapefile)
        Dim colorBreak As MapWinGIS.ShapefileColorBreak
        Dim colorScheme As MapWinGIS.ShapefileColorScheme
        Dim prefix As String = (MWlay.FileName.ToUpper.Chars(MWlay.FileName.Length - 5))

        MWlay.Name &= " " & Left(IO.Path.GetFileNameWithoutExtension(shpFile.Filename), 8)

        If MWlay.FileName.ToLower.EndsWith("_tgr_a.shp") Or _
           MWlay.FileName.ToLower.EndsWith("_tgr_p.shp") Then
            'Color the roads
            colorScheme = New MapWinGIS.ShapefileColorScheme
            colorScheme.FieldIndex = ShpFieldNumFromName(shpFile, "CFCC")
            If colorScheme.FieldIndex > 0 Then
                colorBreak = New MapWinGIS.ShapefileColorBreak
                colorBreak.Caption = "Primary limited access"
                colorBreak.StartValue = prefix & "1"
                colorBreak.EndValue = prefix & "18"
                colorBreak.StartColor = System.Convert.ToUInt32(RGB(132, 0, 0))
                colorBreak.EndColor = colorBreak.StartColor
                'TODO: renderer should be able to change line width: LineWidth = 2
                colorScheme.Add(colorBreak)

                colorBreak = New MapWinGIS.ShapefileColorBreak
                colorBreak.Caption = "Primary non-limited access"
                colorBreak.StartValue = prefix & "2"
                colorBreak.EndValue = prefix & "28"
                colorBreak.StartColor = System.Convert.ToUInt32(RGB(0, 0, 0))
                colorBreak.EndColor = colorBreak.StartColor
                'TODO: renderer should be able to change line width: LineWidth = 2
                colorScheme.Add(colorBreak)

                colorBreak = New MapWinGIS.ShapefileColorBreak
                colorBreak.Caption = "Secondary"
                colorBreak.StartValue = prefix & "3"
                colorBreak.EndValue = prefix & "38"
                colorBreak.StartColor = System.Convert.ToUInt32(RGB(122, 122, 122))
                'TODO: renderer should be able to change line width: LineWidth = 2
                colorBreak.EndColor = colorBreak.StartColor
                colorScheme.Add(colorBreak)

                colorBreak = New MapWinGIS.ShapefileColorBreak
                colorBreak.Caption = "Local"
                colorBreak.StartValue = prefix & "4" 'TODO: A4, A41, A42, A43, A44, A45, A46, A47, A48, A63, A64
                colorBreak.EndValue = prefix & "48"
                colorBreak.StartColor = System.Convert.ToUInt32(RGB(166, 166, 166))
                colorBreak.EndColor = colorBreak.StartColor
                colorScheme.Add(colorBreak)

                colorBreak = New MapWinGIS.ShapefileColorBreak
                colorBreak.Caption = "Other"
                colorBreak.StartValue = prefix & "5" 'TODO: A5, A51, A52, A53, A6, A60, A61, A62, A65, A7, A70, A71, A72, A73, A74
                colorBreak.EndValue = "Z"
                colorBreak.StartColor = System.Convert.ToUInt32(RGB(200, 200, 200))
                'TODO: renderer should be able to change line style: LineStyle = Dashed
                colorBreak.EndColor = colorBreak.StartColor
                colorScheme.Add(colorBreak)

                MWlay.ColoringScheme = colorScheme
                MWlay.LineOrPointSize = 1
                MWlay.OutlineColor = System.Drawing.Color.Black

                SetCensusRenderer(MWlay, shpFile)
            End If
        End If
    End Sub

    Private Function ShpFieldNumFromName(ByVal aShpFile As MapWinGIS.Shapefile, ByVal aFieldName As String) As Integer
        Dim lFieldName As String = LCase(aFieldName)
        Dim iField As Integer
        For iField = 0 To aShpFile.NumFields
            If LCase(aShpFile.Field(iField).Name) = lFieldName Then Return iField
        Next
        Return 0
    End Function

    Private Function GetDefaultsFor(ByVal Filename As String, _
                                    ByVal aProjectDir As String, _
                                    ByRef aDefaultsXml As Xml.XmlDocument) As Xml.XmlNode
        Dim lName As String = Filename.ToLower
        If lName.StartsWith(aProjectDir.ToLower) Then
            lName = lName.Substring(aProjectDir.Length)
        End If

        If Not aDefaultsXml Is Nothing Then
            For Each lChild As Xml.XmlNode In aDefaultsXml.FirstChild.ChildNodes
                'Debug.Print("Testing " & lName & " Like " & "*" & lChild.Attributes.GetNamedItem("Filename").InnerText & "*")
                If lName Like "*" & lChild.Attributes.GetNamedItem("Filename").InnerText.ToLower & "*" Then
                    Return lChild
                End If
            Next
        End If
        Return Nothing
    End Function

    Private Function GetSelectedHUC() As String
        Try
            Dim lNumSelected As Integer = g_MapWin.View.SelectedShapes.NumSelected
            If lNumSelected > 0 Then
                Dim lFieldName As String = ""
                Dim lFieldMatch As Integer = -1
                Dim lCurLayer As MapWinGIS.Shapefile
                lCurLayer = g_MapWin.Layers.Item(g_MapWin.Layers.CurrentLayer).GetObject

                Dim lLayerFilenameOnly As String = IO.Path.GetFileNameWithoutExtension(lCurLayer.Filename).ToLower
                Select Case lLayerFilenameOnly
                    Case "cat", "huc", "huc250d3"
                        lFieldName = "CU"
                    Case "huc12"
                        lFieldName = "HUC_12"
                    Case "cnty"
                        lFieldName = "FIPS"
                    Case "st"
                        lFieldName = "ST"
                    Case Else
                        If lLayerFilenameOnly.StartsWith("wbdhu8") Then
                            lFieldName = "HUC_8"
                        End If
                End Select

                lFieldName = lFieldName.ToLower
                For lField = 0 To lCurLayer.NumFields - 1
                    If lCurLayer.Field(lField).Name.ToLower = lFieldName Then
                        lFieldMatch = lField
                    End If
                Next

                If lFieldMatch >= 0 Then
                    For lSelected As Integer = 0 To lNumSelected - 1
                        Dim lShapeIndex As Integer = g_MapWin.View.SelectedShapes.Item(lSelected).ShapeIndex()
                        Return lCurLayer.CellValue(lFieldMatch, lShapeIndex)
                    Next
                End If
            End If
        Catch e As Exception
            Logger.Dbg("Exception getting selected huc: " & e.Message)
        End Try
        Return ""
    End Function

    Private Function GetSelectedShape() As MapWinGIS.Shape
        Try
            Dim lNumSelected As Integer = g_MapWin.View.SelectedShapes.NumSelected
            If lNumSelected > 0 Then
                For lSelected As Integer = 0 To lNumSelected - 1
                    Dim lShapeIndex As Integer = g_MapWin.View.SelectedShapes.Item(lSelected).ShapeIndex()
                    Dim lCurLayer As MapWinGIS.Shapefile = g_MapWin.Layers.Item(g_MapWin.Layers.CurrentLayer).GetObject
                    Return lCurLayer.Shape(lShapeIndex)
                    'Dim lCurLayerDS As New MapWinGIS.Shapefile
                    'lCurLayerDS.Open(lCurLayer.Filename)
                    'Return lCurLayerDS.Shape(lShapeIndex)
                Next
            End If
        Catch e As Exception
            Logger.Dbg("Exception getting selected shape: " & e.Message)
        End Try
        Return Nothing
    End Function
End Module
