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

    Private Const XMLappName As String = "BASINS System Application"

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub BASINSNewMenu()
        'take appropriate actions if the user selects the 'New' menu
        'with a BASINS project open, or just a MapWindow project open

        Dim lResponse As Microsoft.VisualBasic.MsgBoxResult
        Dim lInputProjection As String = ""

        If IsBASINSProject() Then
            'if currently coming from a BASINS project,
            'ask if the user wants to subset this project
            'or create an entirely new one
            lResponse = Logger.Msg("Do you want to create a new project based on the selected feature(s) in this project?" & vbCrLf & vbCrLf & _
                                   "(Answer 'No' to create a new project from scratch)", vbYesNoCancel, "New Project")
            If lResponse = MsgBoxResult.No Then
                LoadNationalProject()
            ElseIf lResponse = MsgBoxResult.Yes Then
                'are any features selected?
                If GisUtil.NumSelectedFeatures(GisUtil.CurrentLayer) > 0 Then
                    'is this a polygon shapefile?
                    If GisUtil.LayerType(GisUtil.CurrentLayer) = 3 Then
                        'this is a polygon shapefile

                        'come up with a suggested name for the new project
                        Dim lDataPath As String = DefaultBasinsDataDir()
                        Dim lDefDirName As String = IO.Path.GetFileNameWithoutExtension(GisUtil.ProjectFileName)
                        Dim lDefaultProjectFileName As String = CreateDefaultNewProjectFileName(lDataPath, lDefDirName)
                        lDefDirName = PathNameOnly(lDefaultProjectFileName)
                        Logger.Dbg("CreateNewProjectDirectory:" & lDefDirName)
                        System.IO.Directory.CreateDirectory(lDefDirName)

                        'prompt user for new name
                        Dim lOldDataDir As String = PathNameOnly(GisUtil.ProjectFileName) & g_PathChar
                        Dim lProjectFileName As String = PromptForNewProjectFileName(lDefDirName, lDefaultProjectFileName)
                        If lProjectFileName.Length > 0 Then
                            'Check to see if chosen data dir already contains any files
                            Dim lNewDataDir As String = PathNameOnly(lProjectFileName) & g_PathChar
                            Dim lNumFiles As Long = System.IO.Directory.GetFiles(lNewDataDir).LongLength
                            Dim lNumDirs As Long = System.IO.Directory.GetDirectories(lNewDataDir).LongLength
                            If lNumFiles + lNumDirs > 0 Then
                                Logger.Msg("The folder '" & lNewDataDir & "'" & vbCr _
                                       & "already contains " & lNumFiles & " files and " & lNumDirs & " folders." & vbCr _
                                       & "The folder must be empty before a new project can be created here.", "New Project")
                            Else
                                'got a good name for the new project
                                g_MapWin.View.MapCursor = MapWinGIS.tkCursor.crsrWait

                                'make dirs from the old project in the new one
                                Dim lTarget As String
                                Dim lDirs As String() = System.IO.Directory.GetDirectories(PathNameOnly(GisUtil.ProjectFileName))
                                For Each lDirName As String In lDirs
                                    'make this dir
                                    lTarget = lNewDataDir & Mid(lDirName, Len(PathNameOnly(GisUtil.ProjectFileName)) + 2)
                                    System.IO.Directory.CreateDirectory(lTarget)
                                Next

                                CopyFeaturesWithinExtent(lOldDataDir, lNewDataDir)

                                'copy all other files from the old project directory to the new one
                                Dim lFilenames As NameValueCollection
                                lFilenames = New NameValueCollection
                                AddFilesInDir(lFilenames, PathNameOnly(GisUtil.ProjectFileName), True)
                                For Each lFilename As String In lFilenames
                                    If Not FileExt(lFilename) = "mwprj" And Not FileExt(lFilename) = "bmp" Then
                                        lTarget = lNewDataDir & Mid(lFilename, Len(PathNameOnly(GisUtil.ProjectFileName)) + 2)
                                        If Not FileExists(lTarget) Then
                                            g_StatusBar(1).Text = "Copying " & FilenameNoPath(lFilename.ToString)
                                            RefreshView()
                                            IO.File.Copy(lFilename, lTarget, False)
                                        End If
                                    End If
                                Next
                                g_StatusBar(1).Text = ""
                                RefreshView()

                                'copy the mapwindow project file
                                IO.File.Copy(GisUtil.ProjectFileName, lProjectFileName)
                                'open the new mapwindow project file
                                g_Project.Load(lProjectFileName)
                                g_MapWin.PreviewMap.Update()
                                If Not (g_Project.Save(lProjectFileName)) Then
                                    Logger.Dbg("BASINSNewMenu:Save2Failed:" & g_MapWin.LastError)
                                End If

                                g_MapWin.View.MapCursor = MapWinGIS.tkCursor.crsrMapDefault

                            End If
                        End If
                    Else
                        Logger.Msg("The selected map feature must be a polygon shapefile to use this option.", "Create BASINS Project Problem")
                    End If
                Else
                    Logger.Msg("One or more map features must be selected to use this option.", "Create BASINS Project Problem")
                End If
            End If

        Else
            'if not coming from a BASINS project,
            'ask if user wants to make this into a BASINS
            'project or create an entirely new one
            If Not NationalProjectIsOpen() AndAlso GisUtil.NumLayers > 0 Then
                lResponse = Logger.Msg("Do you want to create a new project based on this MapWindow project?" & vbCrLf & vbCrLf & _
                                       "(Answer 'No' to create a new project from scratch)", vbYesNoCancel, "New Project")
                If lResponse = MsgBoxResult.No Then
                    'create entirely new BASINS project
                    LoadNationalProject()
                ElseIf lResponse = MsgBoxResult.Yes Then
                    'create a BASINS project based on this MapWindow project
                    If GisUtil.ProjectFileName Is Nothing Then
                        Logger.Msg("The current MapWindow project must be saved before converting it.", "Convert MapWindow Project Problem")
                    Else
                        'set up temporary extents shapefile
                        Dim lProjectDir As String = PathNameOnly(GisUtil.ProjectFileName)
                        Dim lTitle As String = ""
                        MkDirPath(lProjectDir & "\temp")
                        Dim lNewShapeName As String = lProjectDir & "\temp\tempextent.shp"
                        TryDeleteShapefile(lNewShapeName)
                        'are any features selected?
                        If GisUtil.NumSelectedFeatures(GisUtil.CurrentLayer) > 0 Then
                            'make temp shapefile from selected features
                            GisUtil.SaveSelectedFeatures(GisUtil.LayerName(GisUtil.CurrentLayer), lNewShapeName, False)
                            lInputProjection = GisUtil.ShapefileProjectionString(GisUtil.CurrentLayer)
                            lTitle = "MapWindow Selected Features"
                        ElseIf GisUtil.CurrentLayer > -1 And (GisUtil.LayerType = MapWindow.Interfaces.eLayerType.PointShapefile Or GisUtil.LayerType = MapWindow.Interfaces.eLayerType.LineShapefile Or GisUtil.LayerType = MapWindow.Interfaces.eLayerType.PolygonShapefile) Then
                            'make temp shapefile from current layer
                            Dim lBaseName As String = FilenameNoExt(GisUtil.LayerFileName(GisUtil.CurrentLayer))
                            System.IO.File.Copy(lBaseName & ".shp", lNewShapeName)
                            System.IO.File.Copy(lBaseName & ".dbf", lProjectDir & "\temp\tempextent.dbf")
                            System.IO.File.Copy(lBaseName & ".shx", lProjectDir & "\temp\tempextent.shx")
                            lInputProjection = GisUtil.ShapefileProjectionString(GisUtil.CurrentLayer)
                            lTitle = "MapWindow Current Layer"
                        Else
                            'make temp shapefile from extents of map
                            GisUtil.CreateShapefileOfCurrentMapExtents(lNewShapeName)
                            lTitle = "MapWindow Project Extents"
                        End If

                        'make sure projection is defined
                        If Len(lInputProjection) = 0 Or (lInputProjection Is Nothing) Then
                            'dont have a projection yet, try to use the project's projection
                            lInputProjection = g_Project.ProjectProjection
                        End If
                        If Len(lInputProjection) = 0 Or (lInputProjection Is Nothing) Then
                            'see if there is a prj.proj
                            If FileExists(lProjectDir & "\prj.proj") Then
                                lInputProjection = WholeFileString(lProjectDir & "\prj.proj")
                                lInputProjection = CleanUpUserProjString(lInputProjection)
                            End If
                        End If
                        If Len(lInputProjection) = 0 Or (lInputProjection Is Nothing) Then
                            'still don't have an projection for this mapwindow project, prompt for it
                            lInputProjection = atcProjector.Methods.AskUser
                        End If

                        If lInputProjection.Length > 0 Then
                            'make prj.proj if it doesn't exist
                            If Not FileExists(lProjectDir & "\prj.proj") Then
                                SaveFileString(lProjectDir & "\prj.proj", lInputProjection)
                            End If
                            'project the extents shapefile
                            Dim lExtentsSf As New MapWinGIS.Shapefile
                            If lExtentsSf.Open(lNewShapeName) Then
                                Dim lOutputProjection As String = "+proj=aea +ellps=GRS80 +lon_0=-96 +lat_0=23.0 +lat_1=29.5 +lat_2=45.5 +x_0=0 +y_0=0 +datum=NAD83 +units=m"
                                If lInputProjection <> lOutputProjection Then
                                    If Not MapWinGeoProc.SpatialReference.ProjectShapefile(lInputProjection, lOutputProjection, lExtentsSf) Then
                                        Logger.Msg("Problem projecting the extents shapefile.", "Convert MapWindow Project Problem")
                                    End If
                                End If
                                lExtentsSf.Close()
                            End If

                            'remember the name of this mapwindow project to go back to later
                            pExistingMapWindowProjectName = g_Project.FileName
                            'now open national project with temp shapefile on the map
                            LoadNationalProject()
                            'set symbology for this layer
                            lExtentsSf.Open(lNewShapeName)
                            g_MapWin.Layers.Add(lExtentsSf, lTitle)
                            g_MapWin.Layers(g_MapWin.Layers.GetHandle(g_MapWin.Layers.NumLayers - 1)).Color = System.Drawing.Color.Transparent
                            g_MapWin.Layers(g_MapWin.Layers.GetHandle(g_MapWin.Layers.NumLayers - 1)).OutlineColor = System.Drawing.Color.Black
                            g_MapWin.Layers(g_MapWin.Layers.GetHandle(g_MapWin.Layers.NumLayers - 1)).DrawFill = False
                            g_MapWin.Layers(g_MapWin.Layers.GetHandle(g_MapWin.Layers.NumLayers - 1)).LineStipple = MapWinGIS.tkLineStipple.lsDotted
                            'zoom near this layer
                            g_MapWin.Layers(g_MapWin.Layers.GetHandle(g_MapWin.Layers.NumLayers - 1)).ZoomTo()
                            'select corresponding HUC on the map
                            Dim lTempLayerIndex As Integer = GisUtil.LayerIndex(lTitle)
                            Dim lCatLayerIndex As Integer = GisUtil.LayerIndex("Cataloging Units")
                            Dim lPtX As Double
                            Dim lPtY As Double
                            For j As Integer = 1 To GisUtil.NumFeatures(lTempLayerIndex)
                                If (lExtentsSf.ShapefileType = MapWinGIS.ShpfileType.SHP_POLYGON OrElse _
                                    lExtentsSf.ShapefileType = MapWinGIS.ShpfileType.SHP_POLYGONM OrElse _
                                    lExtentsSf.ShapefileType = MapWinGIS.ShpfileType.SHP_POLYGONZ) Then
                                    GisUtil.ShapeCentroid(lTempLayerIndex, j - 1, lPtX, lPtY)
                                Else
                                    GisUtil.PointXY(lTempLayerIndex, j - 1, lPtX, lPtY)
                                End If
                                GisUtil.SetSelectedFeature(lCatLayerIndex, GisUtil.PointInPolygonXY(lPtX, lPtY, lCatLayerIndex))
                            Next j
                            UpdateSelectedFeatures()

                        End If
                    End If
                End If
            Else
                'there are no layers or national project is already open
                LoadNationalProject()
            End If
        End If
    End Sub

    Private Function GetSelectedRegion() As String
        Try
            Dim lNumSelected As Integer = g_MapWin.View.SelectedShapes.NumSelected
            If lNumSelected > 0 Then
                Dim lXML As String = ""
                Dim lThemeTag As String = ""
                Dim lFieldName As String = ""
                Dim lField As Integer
                Dim lFieldMatch As Integer = -1
                Dim lCurLayer As MapWinGIS.Shapefile
                lCurLayer = g_MapWin.Layers.Item(g_MapWin.Layers.CurrentLayer).GetObject

                Select Case IO.Path.GetFileNameWithoutExtension(lCurLayer.Filename).ToLower
                    Case "cat", "huc", "huc250d3"
                        lThemeTag = "HUC8" '"huc_cd"
                        lFieldName = "CU"
                    Case "cnty"
                        lThemeTag = "county_cd"
                        lFieldName = "FIPS"
                    Case "st"
                        lThemeTag = "state_abbrev"
                        lFieldName = "ST"
                End Select

                lFieldName = lFieldName.ToLower
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
                    lXML &= "  <projection>" & g_Project.ProjectProjection & "</projection>" & vbCrLf
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
                    g_StatusBar(1).Text = "Selecting " & GisUtil.LayerName(iLayer)
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
        g_StatusBar(1).Text = ""
        RefreshView()
    End Sub

    Public Sub SpecifyAndCreateNewProject()
        pBuildFrm = Nothing

        Dim lRegion As String = GetSelectedRegion()
        If lRegion.Length > 0 Then
            If pExistingMapWindowProjectName.Length = 0 Then
                'This is the normal case for building a new project,
                'Save national project as the user has zoomed it
                g_Project.Save(g_Project.FileName)
                CreateNewProjectAndDownloadCoreDataInteractive(lRegion)
            Else
                'build new basins project from mapwindow project
                Dim lDataPath As String = DefaultBasinsDataDir()
                Dim lNewDataDir As String = PathNameOnly(pExistingMapWindowProjectName) & g_PathChar
                'download and project core data
                If Not IO.File.Exists(lNewDataDir & "prj.proj") Then
                    IO.File.WriteAllText(lNewDataDir & "prj.proj", g_Project.ProjectProjection)
                End If
                CreateNewProjectAndDownloadCoreData(lRegion, lDataPath, lNewDataDir, pExistingMapWindowProjectName, True)
                pExistingMapWindowProjectName = ""
            End If
        Else
            'prompt about creating a project with no data
            CreateNewProjectAndDownloadCoreDataInteractive(lRegion)
        End If

    End Sub

    'Returns file name of new project or "" if not built
    Friend Function CreateNewProjectAndDownloadCoreDataInteractive(ByVal aRegion As String) As String
        Dim lDataPath As String
        Dim lDefaultProjectFileName As String        
        Dim lNoData As Boolean = False
        Dim lDefDirName As String = "NewProject"
        Dim lMyProjection As String

StartOver:
        lDataPath = DefaultBasinsDataDir()

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
                            Case Else
                                If lDefDirName = "NewProject" Then
                                    lDefDirName = lChild.InnerText
                                Else
                                    lDefDirName = "Multiple"
                                End If
                        End Select
                    End If
                Next
            End With
        End If

        If lDefDirName = "NewProject" Then
            If lNoData Then
                'Already came through here, don't ask again
            Else
                lNoData = True
                If Logger.Msg("No features have been selected.  Do you wish to create a project with no data?", MsgBoxStyle.YesNo, "Data Extraction") = MsgBoxResult.No Then
                    Return ""
                End If
            End If
        End If

        lDefaultProjectFileName = CreateDefaultNewProjectFileName(lDataPath, lDefDirName)
        lDefDirName = PathNameOnly(lDefaultProjectFileName)
        Logger.Dbg("CreateNewProjectDirectory:" & lDefDirName)
        System.IO.Directory.CreateDirectory(lDefDirName)

        Dim lProjectFileName As String = PromptForNewProjectFileName(lDefDirName, lDefaultProjectFileName)
        If lProjectFileName.Length = 0 Then
            Return ""
        Else
            'If the user did not choose the default folder or a subfolder of it
            Dim lNewDataDir As String = PathNameOnly(lProjectFileName) & g_PathChar
            If Not lNewDataDir.ToLower.StartsWith(lDefDirName.ToLower) Then
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

            lMyProjection = atcProjector.Methods.AskUser
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
                    g_Project.Save(lProjectFileName)
                    g_Project.Modified = True
                    g_Project.Save(lProjectFileName)
                    g_Project.Modified = False
                Else
                    'download and project core data
                    CreateNewProjectAndDownloadCoreData(aRegion, lDataPath, lNewDataDir, lProjectFileName)
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

    'Returns file name of new project or "" if not built
    Public Sub CreateNewProjectAndDownloadCoreData(ByVal aRegion As String, _
                                                   ByVal aDataPath As String, _
                                                   ByVal aNewDataDir As String, _
                                                   ByVal aProjectFileName As String, _
                                                   Optional ByVal aExistingMapWindowProject As Boolean = False, _
                                                   Optional ByVal aCacheFolder As String = "")
        Dim lQuery As String
        Dim lProjection As String = CleanUpUserProjString(IO.File.ReadAllText(aNewDataDir & "prj.proj"))
        Dim lNationalDir As String = IO.Path.Combine(g_ProgramDir, "Data\national" & g_PathChar)
        If Not IO.Directory.Exists(lNationalDir) Then
            lNationalDir = IO.Path.Combine(aDataPath, "national" & g_PathChar)
        End If
        If IO.Directory.Exists(lNationalDir) Then
            CopyFromIfNeeded("sic.dbf", lNationalDir, aNewDataDir)
            CopyFromIfNeeded("storetag.dbf", lNationalDir, aNewDataDir)
            CopyFromIfNeeded("wqcriter.dbf", lNationalDir, aNewDataDir)
            CopyFromIfNeeded("wqobs_prm.dbf", lNationalDir, aNewDataDir)
        End If

        Dim lCacheFolder As String = IO.Path.Combine(aDataPath, "cache")
        If aCacheFolder.Length > 0 Then
            lCacheFolder = aCacheFolder
        End If
        lQuery = "<function name='GetBASINS'>" _
               & "<arguments>" _
               & "<DataType>core31</DataType>" _
               & "<SaveIn>" & aNewDataDir & "</SaveIn>" _
               & "<CacheFolder>" & lCacheFolder & "</CacheFolder>" _
               & "<DesiredProjection>" & lProjection & "</DesiredProjection>" _
               & aRegion _
               & "<clip>False</clip>" _
               & "<merge>True</merge>" _
               & "<joinattributes>true</joinattributes>" _
               & "</arguments>" _
               & "</function>"

        atcDataManager.LoadPlugin("D4EM Data Download::BASINS")
        Dim lPlugins As New ArrayList
        For lPluginIndex As Integer = 0 To g_MapWin.Plugins.Count
            Try
                If Not g_MapWin.Plugins.Item(lPluginIndex) Is Nothing Then
                    lPlugins.Add(g_MapWin.Plugins.Item(lPluginIndex))
                End If
            Catch ex As Exception
            End Try
        Next
        Dim lDownloadManager As New D4EMDataManager.DataManager(lPlugins)
        Dim lResult As String = lDownloadManager.Execute(lQuery)
        'Logger.Msg(lResult, "Result of Query from DataManager")

        If Not lResult Is Nothing AndAlso lResult.Length > 0 AndAlso lResult.StartsWith("<success>") Then
            If Not aExistingMapWindowProject Then
                'regular case, not coming from existing mapwindow project
                ClearLayers()
                If Not (g_Project.Save(aProjectFileName)) Then
                    Logger.Dbg("CreateNewProjectAndDownloadCoreData:Save1Failed:" & g_MapWin.LastError)
                End If
            Else
                'open existing mapwindow project again
                g_Project.Load(aProjectFileName)
                Dim lProjectDir As String = PathNameOnly(aProjectFileName)
                Dim lNewShapeName As String = lProjectDir & "\temp\tempextent.shp"
                TryDeleteShapefile(lNewShapeName)
            End If
            g_Project.Modified = True
            ProcessDownloadResults(lResult) 'TODO: skip message box describing what has been downloaded?

            AddAllShapesInDir(aNewDataDir, aNewDataDir)
            g_MapWin.PreviewMap.Update(MapWindow.Interfaces.ePreviewUpdateExtents.CurrentMapView)
            If Not aExistingMapWindowProject Then
                'regular case, not coming from existing mapwindow project
                'set mapwindow project projection to projection of first layer
                g_Project.ProjectProjection = lProjection

                Dim lKey As String = g_MapWin.Plugins.GetPluginKey("Tiled Map")
                If Not String.IsNullOrEmpty(lKey) Then g_MapWin.Plugins.StopPlugin(lKey)

                If Not (g_Project.Save(aProjectFileName)) Then
                    Logger.Dbg("CreateNewProjectAndDownloadCoreData:Save2Failed:" & g_MapWin.LastError)
                End If
            End If
        End If
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
        Application.DoEvents()
        For Each lInstructionNode As Xml.XmlNode In lXmlInstructions.ChildNodes(0).ChildNodes
            lMessage &= ProcessDownloadResult(lInstructionNode.OuterXml) & vbCrLf
            Application.DoEvents()
        Next
        If lMessage.Length > 2 Then
            Logger.Msg(lMessage, "Data Download")
            If Logger.DisplayMessageBoxes AndAlso lMessage.Contains(" Data file") Then
                atcDataManager.UserManage()
            End If
        End If
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
                        g_StatusBar(1).Text = "Clipping Grid..."
                        RefreshView()
                        DoEvents()
                        If Not FileExists(FilenameNoExt(lCurFilename) & ".prj") Then
                            'create .prj file as work-around for clipping bug
                            SaveFileString(FilenameNoExt(lCurFilename) & ".prj", "")
                        End If
                        Logger.Dbg("ClipGridWithPolygon: " & lCurFilename & " " & lProjectDir & "nlcd\catextent.shp" & " " & lOutputFileName & " " & "True")
                        lSuccess = MapWinGeoProc.SpatialOperations.ClipGridWithPolygon(lCurFilename, lShape, lOutputFileName, True)
                        lSuccess = lExtentsSf.Close
                        g_StatusBar(1).Text = ""
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
                            Dim lLayerHandle As Integer = -1
                            For i As Integer = 0 To g_MapWin.Layers.NumLayers
                                Dim lLayer As Layer = g_MapWin.Layers(g_MapWin.Layers.GetHandle(i))
                                If Not (lLayer Is Nothing) AndAlso lLayer.FileName = lOutputFileName Then
                                    lLayerHandle = g_MapWin.Layers.GetHandle(i)
                                End If
                            Next
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
                            g_StatusBar(1).Text = "Projecting Grid..."
                            RefreshView()
                            DoEvents()
                            lSuccess = MapWinGeoProc.SpatialReference.ProjectGrid(lInputProjection, lOutputProjection, lCurFilename, lOutputFileName, True)
                            g_StatusBar(1).Text = ""
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
        Dim lBasinsFolder As String = My.Computer.Registry.GetValue("HKEY_LOCAL_MACHINE\SOFTWARE\AQUA TERRA Consultants\BASINS", "Base Directory", "C:\Basins")
        Dim lShapeUtilExe As String = FindFile("Please locate ShapeUtil.exe", lBasinsFolder & "\etc\datadownload\ShapeUtil.exe")
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
        Dim lBasinsFolder As String = My.Computer.Registry.GetValue("HKEY_LOCAL_MACHINE\SOFTWARE\AQUA TERRA Consultants\BASINS", "Base Directory", "C:\Basins")
        Dim lDataDownloadExe As String = FindFile("Please locate DataDownload.exe", lBasinsFolder & "\etc\DataDownload\DataDownload.exe")
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
        Dim lBasinsFolder As String = My.Computer.Registry.GetValue("HKEY_LOCAL_MACHINE\SOFTWARE\AQUA TERRA Consultants\BASINS", "Base Directory", "C:\Basins")
        lDefaultsPath = FindFile("Please Locate BasinsDefaultLayers.xml", lBasinsFolder & "\etc\BasinsDefaultLayers.xml")
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
        Dim LayerName As String
        Dim Group As String = ""
        Dim Visible As Boolean
        'Dim Style As atcRenderStyle = Nothing

        Dim MWlay As MapWindow.Interfaces.Layer
        Dim shpFile As MapWinGIS.Shapefile
        'Dim RGBcolor As Int32
        'Dim RGBoutline As Int32

        'Don't add layer again if we already have it
        For lLayer As Integer = 0 To g_MapWin.Layers.NumLayers - 1
            MWlay = g_MapWin.Layers(g_MapWin.Layers.GetHandle(lLayer))
            If MWlay.FileName.ToLower = aFilename.ToLower Then
                Return MWlay
            End If
        Next
        MWlay = Nothing

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

                'If Not layerXml.FirstChild Is Nothing Then
                '    Style = New atcRenderStyle
                '    Style.xml = layerXml.FirstChild
                'End If
            End If

            g_StatusBar.Item(1).Text = "Opening " & aFilename
            shpFile = New MapWinGIS.Shapefile
            shpFile.Open(aFilename)

            Select Case shpFile.ShapefileType
                Case MapWinGIS.ShpfileType.SHP_POINT, _
                        MapWinGIS.ShpfileType.SHP_POINTM, _
                        MapWinGIS.ShpfileType.SHP_POINTZ, _
                        MapWinGIS.ShpfileType.SHP_MULTIPOINT
                    'If Style Is Nothing Then
                    MWlay = g_MapWin.Layers.Add(shpFile, LayerName)
                    'Else
                    'RGBcolor = RGB(Style.MarkColor.R, Style.MarkColor.G, Style.MarkColor.B)
                    'MWlay = g_MapWin.Layers.Add(shpFile, LayerName, RGBcolor, RGBcolor, Style.MarkSize)
                    'Select Case Style.MarkStyle                        'TODO: translate Cross, X, Bitmap properly
                    '    Case "Circle" : MWlay.PointType = MapWinGIS.tkPointType.ptCircle
                    '    Case "Square" : MWlay.PointType = MapWinGIS.tkPointType.ptSquare
                    '    Case "Cross" : MWlay.PointType = MapWinGIS.tkPointType.ptTriangleRight
                    '    Case "X" : MWlay.PointType = MapWinGIS.tkPointType.ptTriangleLeft
                    '    Case "Diamond" : MWlay.PointType = MapWinGIS.tkPointType.ptDiamond
                    '    Case "Bitmap"
                    '        Select Case Style.MarkBitsAsHex
                    '            Case "3C 42 81 99 99 81 42 3C" : MWlay.PointType = MapWinGIS.tkPointType.ptCircle
                    '            Case "00 7E 7E 7E 7E 7E 7E 00" : MWlay.PointType = MapWinGIS.tkPointType.ptSquare
                    '            Case "0000 0000 0000 3FF8 3FF8 1FF0 1FF0 0FE0 0FE0 07C0 07C0 0380 0380 0100 0000 0000"
                    '                MWlay.PointType = MapWinGIS.tkPointType.ptTriangleDown
                    '            Case Else
                    '                MWlay.PointType = MapWinGIS.tkPointType.ptDiamond
                    '        End Select
                    '        'MWlay.PointType = MapWinGIS.tkPointType.ptUserDefined
                    '        'mwlay.UserPointType = 'TODO: translate bitmap into Image
                    'End Select
                    'End If

                Case MapWinGIS.ShpfileType.SHP_POLYLINE, MapWinGIS.ShpfileType.SHP_POLYLINEM, MapWinGIS.ShpfileType.SHP_POLYLINEZ
                    'If Style Is Nothing Then
                    MWlay = g_MapWin.Layers.Add(shpFile, LayerName)
                    'Else
                    'RGBcolor = RGB(Style.LineColor.R, Style.LineColor.G, Style.LineColor.B)
                    'MWlay = g_MapWin.Layers.Add(shpFile, LayerName, RGBcolor, RGBcolor, Style.LineWidth)
                    'End If
                Case MapWinGIS.ShpfileType.SHP_POLYGON, MapWinGIS.ShpfileType.SHP_POLYGONM, MapWinGIS.ShpfileType.SHP_POLYGONZ
                    'If Style Is Nothing Then
                    MWlay = g_MapWin.Layers.Add(shpFile, LayerName)
                    'Else
                    'RGBcolor = RGB(Style.FillColor.R, Style.FillColor.G, Style.FillColor.B)
                    'RGBoutline = RGB(Style.LineColor.R, Style.LineColor.G, Style.LineColor.B)
                    'MWlay = g_MapWin.Layers.Add(shpFile, LayerName, RGBcolor, RGBoutline, Style.LineWidth)
                    'Select Case LCase(Style.FillStyle)
                    '    Case "none"
                    '        MWlay.FillStipple = MapWinGIS.tkFillStipple.fsNone
                    '        If MWlay.Color.Equals(System.Drawing.Color.Black) Then
                    '            MWlay.Color = System.Drawing.Color.White
                    '        End If
                    '        MWlay.DrawFill = False
                    '    Case "solid" '"Solid"
                    '    Case "horizontal" : MWlay.FillStipple = MapWinGIS.tkFillStipple.fsHorizontalBars
                    '    Case "vertical" : MWlay.FillStipple = MapWinGIS.tkFillStipple.fsVerticalBars
                    '    Case "down" : MWlay.FillStipple = MapWinGIS.tkFillStipple.fsDiagonalDownRight
                    '    Case "up" : MWlay.FillStipple = MapWinGIS.tkFillStipple.fsDiagonalDownLeft
                    '    Case "cross"
                    '    Case "diagcross"
                    'End Select
                    'End If
            End Select
            'If Not Style Is Nothing Then
            '    Select Case Style.LineStyle.ToLower
            '        Case "solid" : MWlay.LineStipple = MapWinGIS.tkLineStipple.lsNone
            '        Case "dash" : MWlay.LineStipple = MapWinGIS.tkLineStipple.lsDashed
            '        Case "dot" : MWlay.LineStipple = MapWinGIS.tkLineStipple.lsDotted
            '        Case "dashdot" : MWlay.LineStipple = MapWinGIS.tkLineStipple.lsDashDotDash
            '        Case "dashdotdot" : MWlay.LineStipple = MapWinGIS.tkLineStipple.lsDashDotDash
            '            'Case "alternate" : MWlay.LineStipple = MapWinGIS.tkLineStipple.lsCustom
            '            '    MWlay.UserLineStipple = 
            '    End Select
            'End If

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
                If LCase(aFilename).IndexOf("\landuse" & g_PathChar) > 0 Then
                    SetLandUseColors(MWlay, shpFile)
                ElseIf LCase(aFilename).IndexOf("\nhd" & g_PathChar) > 0 Then
                    If InStr(IO.Path.GetFileNameWithoutExtension(shpFile.Filename), "NHD") > 0 Then
                        MWlay.Name = IO.Path.GetFileNameWithoutExtension(shpFile.Filename)
                    Else
                        MWlay.Name &= " " & IO.Path.GetFileNameWithoutExtension(shpFile.Filename)
                    End If
                ElseIf LCase(aFilename).IndexOf("\census" & g_PathChar) > 0 Then
                    SetCensusColors(MWlay, shpFile)
                ElseIf LCase(aFilename).IndexOf("\dem" & g_PathChar) > 0 Then
                    SetDemColors(MWlay, shpFile)
                ElseIf LCase(aFilename).EndsWith("cat.shp") Then
                    MWlay.ZoomTo()
                End If
                If Group.Length > 0 Then
                    AddLayerToGroup(MWlay, Group)
                End If

                If MWlay.Visible Then
                    g_MapWin.View.Redraw()
                    DoEvents()
                End If
                g_Project.Modified = True
            End If
        Catch ex As Exception
            Logger.Msg("Could not add '" & aFilename & "' to the project. " & ex.ToString & vbCr & ex.StackTrace, "Add Shape")
        End Try
        g_StatusBar.Item(1).Text = ""

        Return MWlay
    End Function

    'Given a file name and the XML describing how to render it, add a grid layer to MapWindow
    Private Function AddGridToMW(ByVal aFilename As String, _
                                 ByRef layerXml As Xml.XmlNode) As MapWindow.Interfaces.Layer
        Dim LayerName As String
        Dim Group As String = "Other"
        Dim Visible As Boolean
        'Dim Style As atcRenderStyle = New atcRenderStyle

        Dim MWlay As MapWindow.Interfaces.Layer
        Dim g As MapWinGIS.Grid

        'Don't add layer again if we already have it
        For lLayer As Integer = 0 To g_MapWin.Layers.NumLayers - 1
            MWlay = g_MapWin.Layers(g_MapWin.Layers.GetHandle(lLayer))
            If MWlay.FileName.ToLower = aFilename.ToLower Then
                Return MWlay
            End If
        Next
        MWlay = Nothing

        Try
            GetDefaultRenderer(aFilename)

            g_StatusBar.Item(1).Text = "Opening " & aFilename

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
            g_Project.Modified = True
        Catch ex As Exception
            Logger.Msg("Could not add '" & aFilename & "' to the project. " & ex.ToString & vbCr & ex.StackTrace, "Add Grid")
        End Try
        g_StatusBar.Item(1).Text = ""

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
End Module
