Imports System.Collections.Specialized
Imports System.Reflection
Imports atcUtility
Imports atcData
Imports MapWinUtility
Imports atcMwGisUtility

''' <summary>
''' 
''' </summary>
''' <remarks></remarks>
Friend Module modBasinsPlugin
    'Declare this as global so that it can be accessed throughout the plug-in project.
    'These variables are initialized in the plugin_Initialize event.
    Public g_MapWin As MapWindow.Interfaces.IMapWin
    Public g_Menus As MapWindow.Interfaces.Menus
    Public g_StatusBar As MapWindow.Interfaces.StatusBar
    Public g_Toolbar As MapWindow.Interfaces.Toolbar
    Public g_Plugins As MapWindow.Interfaces.Plugins
    Public g_Project As MapWindow.Interfaces.Project
    Public g_MapWinWindowHandle As Integer
    Public g_AppName As String = "BASINS4"
    Public g_BasinsDataDirs As New ArrayList
    Public g_BasinsDir As String = ""
    Public pBuildFrm As frmBuildNew

    Friend pExistingMapWindowProjectName As String = ""
    Friend pCommandLineScript As Boolean = False

    'File menu -- created by MapWindow
    Friend Const FileMenuName As String = "mnuFile"

    Friend Const ProjectsMenuName As String = "BasinsProjects"
    Friend Const ProjectsMenuString As String = "Open BASINS Project"

    Friend Const NewDataMenuName As String = "BasinsNewData"
    Friend Const NewDataMenuString As String = "New Data"

    Friend Const OpenDataMenuName As String = "BasinsOpenData"
    Friend Const OpenDataMenuString As String = "Open Data"

    Friend Const DownloadMenuName As String = "BasinsDownloadData"
    Friend Const DownloadMenuString As String = "Download Data"

    Friend Const ManageDataMenuName As String = "BasinsManageData"
    Friend Const ManageDataMenuString As String = "Manage Data"

    Friend Const SaveDataMenuName As String = "BasinsSaveData"
    Friend Const SaveDataMenuString As String = "Save Data In..."

    Friend Const AnalysisMenuName As String = "BasinsAnalysis"
    Friend Const AnalysisMenuString As String = "Analysis"

    Friend Const ModelsMenuName As String = "BasinsModels"
    Friend Const ModelsMenuString As String = "Models"

    Friend Const ComputeMenuName As String = "BasinsCompute"
    Friend Const ComputeMenuString As String = "Compute"

    Friend pWelcomeScreenShow As Boolean = False

    Friend Const RegisterMenuName As String = "RegisterBASINS"
    Friend Const RegisterMenuString As String = "Register as a BASINS user"

    Friend Const CheckForUpdatesMenuName As String = "CheckForUpdates"
    Friend Const CheckForUpdatesMenuString As String = "Check For Updates"

    Friend Const HelpMenuName As String = "mnuHelp"
    Friend Const BasinsHelpMenuName As String = "BasinsHelp"
    Friend Const BasinsHelpMenuString As String = "BASINS Documentation"

    Friend Const BasinsWebPageMenuName As String = "BasinsWebPage"
    Friend Const BasinsWebPageMenuString As String = "BASINS Web Page"

    Friend Const SendFeedbackMenuName As String = "SendFeedback"
    Friend Const SendFeedbackMenuString As String = "Send Feedback"

    Friend Const DataMenuName As String = "BasinsData"
    Friend Const DataMenuString As String = "Data"
    Friend pLoadedDataMenu As Boolean = False

    Private Const BasinsDataPath As String = "Basins\data\"
    Private Const NationalProjectFilename As String = "national.mwprj"

    Friend Sub OpenedData(ByVal aDataSource As atcData.atcDataSource)
        RefreshSaveDataMenu()
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks></remarks>
    Friend Sub RefreshSaveDataMenu()
        g_Menus.Remove(SaveDataMenuName)
        AddMenuIfMissing(SaveDataMenuName, FileMenuName, SaveDataMenuString, "mnuSaveAs")
        For Each lDataSource As atcDataSource In atcDataManager.DataSources
            If lDataSource.CanSave Then
                AddMenuIfMissing(SaveDataMenuName & "_" & lDataSource.Specification, SaveDataMenuName, lDataSource.Specification)
            End If
        Next
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks></remarks>
    Friend Sub FindBasinsDrives()
        If g_BasinsDataDirs.Count = 0 Then
            Dim lCheckDir As String = DefaultBasinsDataDir()
            If FileExists(lCheckDir, True, False) Then g_BasinsDataDirs.Add(lCheckDir)

            For Each lDrive As IO.DriveInfo In IO.DriveInfo.GetDrives()
                With lDrive
                    If .IsReady AndAlso .DriveType = IO.DriveType.Fixed OrElse .DriveType = IO.DriveType.Network Then
                        lCheckDir = .Name & BasinsDataPath
                        If FileExists(lCheckDir, True, False) Then g_BasinsDataDirs.Add(lCheckDir)
                    End If
                End With
            Next

            Select Case g_BasinsDataDirs.Count
                Case 0 : Logger.Msg("No BASINS folders found on any drives on this computer", "FindBasinsDrives")
                Case 1 : Logger.Dbg("Found BASINS Data: " & g_BasinsDataDirs(0))
                Case Is > 1
                    Dim lAllDirs As String = ""
                    For Each lDir As String In g_BasinsDataDirs
                        lAllDirs &= lDir & "  "
                    Next
                    Logger.Dbg("Found BASINS Data: " & lAllDirs)
            End Select
        End If
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks></remarks>
    Friend Sub LoadNationalProject()
        If Not NationalProjectIsOpen() Then
            Dim lFileName As String = g_BasinsDir & "\Data\national\" & NationalProjectFilename
            If Not FileExists(lFileName) Then
                For Each lDir As String In g_BasinsDataDirs
                    lFileName = lDir & "national\" & NationalProjectFilename
                    If FileExists(lFileName) Then 'found existing national project
                        Exit For
                    End If
                Next
            End If

            If FileExists(lFileName) Then  'load national project
                g_Project.Load(lFileName)
            Else
                Logger.Msg("Unable to find '" & NationalProjectFilename & "'", "Open National")
                Exit Sub
            End If
        End If

        If NationalProjectIsOpen() Then
            'Select the Cataloging Units layer by default 
            For iLayer As Integer = 0 To g_MapWin.Layers.NumLayers - 1
                If g_MapWin.Layers(g_MapWin.Layers.GetHandle(iLayer)).Name = "Cataloging Units" Then
                    g_MapWin.Layers.CurrentLayer = g_MapWin.Layers.GetHandle(iLayer)
                    Exit For
                End If
            Next
            g_Toolbar.PressToolbarButton("tbbSelect")
            pBuildFrm = New frmBuildNew
            pBuildFrm.Show()
            pBuildFrm.Top = GetSetting("BASINS4", "Window Positions", "BuildTop", "300")
            pBuildFrm.Left = GetSetting("BASINS4", "Window Positions", "BuildLeft", "0")
            UpdateSelectedFeatures()
        Else
            Logger.Msg("Unable to open national project", "Open National")
        End If
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Friend Function NationalProjectIsOpen() As Boolean
        If (Not g_Project Is Nothing) _
            AndAlso (Not g_Project.FileName Is Nothing) _
            AndAlso g_Project.FileName.ToLower.EndsWith(NationalProjectFilename) Then
            Return True
        Else
            Return False
        End If
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks></remarks>
    Friend Sub BASINSNewMenu()
        'take appropriate actions if the user selects the 'New' menu
        'with a BASINS project open, or just a MapWindow project open

        GisUtil.MappingObject = g_MapWin
        Dim lResponse As Microsoft.VisualBasic.MsgBoxResult
        Dim lInputProjection As String = ""

        If IsBASINSProject() Then
            'if currently coming from a BASINS project,
            'ask if the user wants to subset this project
            'or create an entirely new one
            lResponse = Logger.Msg("Do you want to create a BASINS project based on the selected feature(s) in this BASINS project?" & vbCrLf & vbCrLf & _
                                   "(Answer 'No' to create an entirely new BASINS project)", vbYesNoCancel, "Create BASINS Project From Selected Features?")
            If lResponse = MsgBoxResult.No Then
                LoadNationalProject()
            ElseIf lResponse = MsgBoxResult.Yes Then
                'are any features selected?
                If GisUtil.NumSelectedFeatures(GisUtil.CurrentLayer) > 0 Then
                    'is this a polygon shapefile?
                    If GisUtil.LayerType(GisUtil.CurrentLayer) = 3 Then
                        'this is a polygon shapefile

                        'build collection of selected shapes
                        Dim lSelectedLayer As Integer = GisUtil.CurrentLayer
                        Dim lSelectedShapeIndexes As New Collection
                        For i As Integer = 1 To GisUtil.NumSelectedFeatures(lSelectedLayer)
                            lSelectedShapeIndexes.Add(GisUtil.IndexOfNthSelectedFeatureInLayer(i - 1, lSelectedLayer))
                        Next

                        'come up with a suggested name for the new project
                        Dim lDataPath As String = DefaultBasinsDataDir()
                        Dim lDefDirName As String = FilenameOnly(GisUtil.ProjectFileName)
                        Dim lDefaultProjectFileName As String = CreateDefaultNewProjectFileName(lDataPath, lDefDirName)
                        lDefDirName = PathNameOnly(lDefaultProjectFileName)
                        Logger.Dbg("CreateNewProjectDirectory:" & lDefDirName)
                        System.IO.Directory.CreateDirectory(lDefDirName)

                        'prompt user for new name
                        Dim lProjectFileName As String = PromptForNewProjectFileName(lDefDirName, lDefaultProjectFileName)
                        Dim lOldDataDir As String = PathNameOnly(GisUtil.ProjectFileName) & "\"
                        Dim lNewDataDir As String = PathNameOnly(lProjectFileName) & "\"

                        If lProjectFileName.Length > 0 Then

                            'Check to see if chosen data dir already contains any files
                            Dim lNumFiles As Long = System.IO.Directory.GetFiles(lNewDataDir).LongLength
                            Dim lNumDirs As Long = System.IO.Directory.GetDirectories(lNewDataDir).LongLength
                            If lNumFiles + lNumDirs > 0 Then
                                Logger.Msg("The folder '" & lNewDataDir & "'" & vbCr _
                                       & "already contains " & lNumFiles & " files and " & lNumDirs & " folders." & vbCr _
                                       & "The folder must be empty before a new project can be created here.", "BASINS Build New")
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

                                CopyFeaturesWithinExtent(lSelectedLayer, lSelectedShapeIndexes, lOldDataDir, lNewDataDir)

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
            If GisUtil.NumLayers > 0 Then
                lResponse = Logger.Msg("Do you want to create a BASINS project based on this MapWindow project?" & vbCrLf & vbCrLf & _
                                       "(Answer 'No' to create an entirely new BASINS project)", vbYesNoCancel, "Convert MapWindow Project to BASINS Project?")
                If lResponse = MsgBoxResult.No Then
                    'create entirely new BASINS project
                    LoadNationalProject()
                ElseIf lResponse = MsgBoxResult.Yes Then
                    'create a BASINS project based on this MapWindow project
                    If GisUtil.ProjectFileName Is Nothing Then
                        Logger.Msg("The current MapWindow project must be saved before converting it to a BASINS Project.", "Convert MapWindow Project Problem")
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
                'there are no layers 
                LoadNationalProject()
            End If
        End If
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Friend Function IsBASINSProject() As Boolean
        'make sure the project has a cat layer and state layer
        Dim lHaveCatLayer As Boolean = False
        Dim lHaveStateLayer As Boolean = False
        For i As Integer = 0 To GisUtil.NumLayers - 1
            If FilenameNoPath(GisUtil.LayerFileName(i)) = "st.shp" Then
                lHaveStateLayer = True
            End If
            If FilenameNoPath(GisUtil.LayerFileName(i)) = "cat.shp" Then
                lHaveCatLayer = True
            End If
        Next
        If lHaveCatLayer And lHaveStateLayer Then
            Return True
        Else
            Return False
        End If
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks></remarks>
    Friend Sub SpecifyAndCreateNewProject()
        pBuildFrm = Nothing

        Dim lThemeTag As String = ""
        Dim lFieldName As String = ""
        Dim lField As Integer
        Dim lFieldMatch As Integer = -1
        Dim lCurLayer As MapWinGIS.Shapefile
        lCurLayer = g_MapWin.Layers.Item(g_MapWin.Layers.CurrentLayer).GetObject

        Select Case FilenameOnly(lCurLayer.Filename).ToLower
            Case "cat", "huc", "huc250d3"
                lThemeTag = "huc_cd"
                lFieldName = "CU"
            Case "cnty"
                lThemeTag = "county_cd"
                lFieldName = "FIPS"
            Case "st"
                lThemeTag = "state_abbrev"
                lFieldName = "ST"
            Case Else
                Logger.Msg("Unknown layer for selection, using lFirst field", "Area Selection")
                lThemeTag = "huc_cd"
                lFieldMatch = 1
        End Select

        lFieldName = lFieldName.ToLower
        For lField = 0 To lCurLayer.NumFields - 1
            If lCurLayer.Field(lField).Name.ToLower = lFieldName Then
                lFieldMatch = lField
            End If
        Next

        If lFieldMatch >= 0 Then
            If pExistingMapWindowProjectName.Length = 0 Then
                'This is the normal case for building a new project,
                'Save national project as the user has zoomed it
                g_Project.Save(g_Project.FileName)
                CreateNewProjectAndDownloadCoreDataInteractive(lThemeTag, GetSelected(lFieldMatch))
            Else
                'build new basins project from mapwindow project
                Dim lDataPath As String = DefaultBasinsDataDir()
                Dim lNewDataDir As String = PathNameOnly(pExistingMapWindowProjectName) & "\"
                'download and project core data
                Logger.Dbg("DownloadData:" & lThemeTag)
                CreateNewProjectAndDownloadCoreData(lThemeTag, GetSelected(lFieldMatch), lDataPath, lNewDataDir, pExistingMapWindowProjectName, True)
                pExistingMapWindowProjectName = ""
            End If
        Else
            Logger.Msg("Could not find field " & lFieldName & " in " & lCurLayer.Filename, "Could not create project")
        End If

    End Sub

    Public Function DefaultBasinsDataDir() As String
        Return My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\" & BasinsDataPath
    End Function

    Private Function GetSelected(ByVal aField As Integer) As ArrayList
        Dim lSelected As Integer
        Dim lShape As Integer
        Dim lSf As MapWinGIS.Shapefile = g_MapWin.Layers.Item(g_MapWin.Layers.CurrentLayer).GetObject
        Dim lRetval As New ArrayList(g_MapWin.View.SelectedShapes().NumSelected)
        For lSelected = 0 To g_MapWin.View.SelectedShapes.NumSelected - 1
            lShape = g_MapWin.View.SelectedShapes.Item(lSelected).ShapeIndex()
            lRetval.Add(lSf.CellValue(aField, lShape))
        Next
        Return lRetval
    End Function

    Private Sub CopyFeaturesWithinExtent(ByVal aSelectedLayer As Integer, ByVal aSelectedShapeIndexes As Collection, ByVal aOldFolder As String, ByVal aNewFolder As String)
        'copy features that are within selected indexes of selected layer to new folder
        Dim i As Integer
        Dim lCurrentSf As MapWinGIS.Shapefile
        Dim lResultSf As MapWinGIS.Shapefile
        Dim lNewName As String

        Dim lExtentShape As MapWinGIS.Shape = Nothing
        Dim lResultShape As MapWinGIS.Shape = Nothing
        Dim lExtentsSf As New MapWinGIS.Shapefile
        Dim lExtentsFileName As String = GisUtil.LayerFileName(aSelectedLayer)
        If lExtentsSf.Open(lExtentsFileName) Then
            lExtentShape = lExtentsSf.Shape(aSelectedShapeIndexes(1))
            For i = 2 To aSelectedShapeIndexes.Count
                MapWinGeoProc.SpatialOperations.MergeShapes(lExtentShape, lExtentsSf.Shape(aSelectedShapeIndexes(i)), lResultShape)
                lExtentShape = lResultShape
            Next
        End If

        For iLayer As Integer = 0 To GisUtil.NumLayers - 1
            If iLayer <> aSelectedLayer Then
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

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="aMenuName"></param>
    ''' <param name="aParent"></param>
    ''' <param name="aMenuText"></param>
    ''' <param name="aAfter"></param>
    ''' <param name="aBefore"></param>
    ''' <param name="aAlphabetical"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Friend Function AddMenuIfMissing(ByVal aMenuName As String, _
                                            ByVal aParent As String, _
                                            ByVal aMenuText As String, _
                                   Optional ByVal aAfter As String = "", _
                                   Optional ByVal aBefore As String = "", _
                                   Optional ByVal aAlphabetical As Boolean = False) _
                                   As MapWindow.Interfaces.MenuItem

        Dim lMenus As MapWindow.Interfaces.Menus = g_Menus
        With lMenus
            Dim lMenu As MapWindow.Interfaces.MenuItem = .Item(aMenuName)
            If Not lMenu Is Nothing Then 'This item already exists
                Return lMenu
            ElseIf aAlphabetical And aParent.Length > 0 Then
                'Need parent to do alphabetical search for position
                Dim lParentMenu As MapWindow.Interfaces.MenuItem = .Item(aParent)
                Dim lSubmenuIndex As Integer = 0
                Dim lExistingMenu As MapWindow.Interfaces.MenuItem

                If aAfter.Length > 0 Then
                    'First make sure we are after a particular item
                    While lSubmenuIndex < lParentMenu.NumSubItems
                        lExistingMenu = lParentMenu.SubItem(lSubmenuIndex)
                        If Not lExistingMenu Is Nothing AndAlso _
                           Not lExistingMenu.Name Is Nothing AndAlso _
                               lExistingMenu.Name.Equals(aAfter) Then
                            Exit While
                        End If
                        lExistingMenu = Nothing
                        lSubmenuIndex += 1
                    End While
                    If lSubmenuIndex >= lParentMenu.NumSubItems Then
                        'Did not find menu aAfter, so start at first subitem
                        lSubmenuIndex = 0
                    End If
                End If

                'Find alphabetical position for new menu item
                While lSubmenuIndex < lParentMenu.NumSubItems
                    lExistingMenu = lParentMenu.SubItem(lSubmenuIndex)
                    If Not lExistingMenu Is Nothing AndAlso _
                       Not lExistingMenu.Name Is Nothing Then
                        If (aBefore.Length > 0 AndAlso lExistingMenu.Text = aBefore) OrElse _
                           lExistingMenu.Text > aMenuText Then
                            'Add before existing menu with alphabetically later text
                            Return .AddMenu(aMenuName, aParent, aMenuText, lExistingMenu.Name)
                        End If
                    End If
                    lSubmenuIndex += 1
                End While
                'Add at default position, after last parent subitem
                Return .AddMenu(aMenuName, aParent, Nothing, aMenuText)
            ElseIf aBefore.Length > 0 Then
                Return .AddMenu(aMenuName, aParent, aMenuText, aBefore)
            Else
                Return .AddMenu(aMenuName, aParent, Nothing, aMenuText, aAfter)
            End If
        End With
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="aIgnore"></param>
    ''' <remarks></remarks>
    Friend Sub RefreshAnalysisMenu(Optional ByVal aIgnore As String = "")
        If pLoadedDataMenu Then
            AddMenuIfMissing(AnalysisMenuName, "", AnalysisMenuString, FileMenuName)
            AddMenuIfMissing(AnalysisMenuName & "_ArcView3", AnalysisMenuName, "ArcView 3")
            AddMenuIfMissing(AnalysisMenuName & "_ArcGIS", AnalysisMenuName, "ArcGIS")
            AddMenuIfMissing(AnalysisMenuName & "_GenScn", AnalysisMenuName, "GenScn")
            AddMenuIfMissing(AnalysisMenuName & "_WDMUtil", AnalysisMenuName, "WDMUtil")

            Dim lPlugins As ICollection = atcDataManager.GetPlugins(GetType(atcDataDisplay))
            If lPlugins.Count > 0 Then
                Dim lSeparatorName As String = AnalysisMenuName & "_Separator1"
                AddMenuIfMissing(lSeparatorName, AnalysisMenuName, "-")
                For Each lDisp As atcDataDisplay In lPlugins
                    Dim lMenuText As String = lDisp.Name
                    If Not lMenuText.Equals(aIgnore) AndAlso lMenuText.StartsWith("Analysis::") Then
                        AddMenuIfMissing(AnalysisMenuName & "_" & lDisp.Name, AnalysisMenuName, lMenuText.Substring(10), lSeparatorName, , True)
                    End If
                Next
            End If
        End If
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks></remarks>
    Friend Sub RefreshComputeMenu()
        g_Menus.Remove(ComputeMenuName)
        g_Menus.AddMenu(ComputeMenuName, "", Nothing, ComputeMenuString, FileMenuName)
        Dim lDataSources As atcCollection = atcDataManager.GetPlugins(GetType(atcDataSource))
        For Each ds As atcDataSource In lDataSources
            If ds.Category <> "File" Then
                Dim lCategoryMenuName As String = ComputeMenuName & "_" & ds.Category
                Dim lOperations As atcDataAttributes = ds.AvailableOperations
                If Not lOperations Is Nothing AndAlso lOperations.Count > 0 Then
                    For Each lOperation As atcDefinedValue In lOperations
                        Select Case lOperation.Definition.TypeString
                            Case "atcTimeseries", "atcDataGroup"
                                AddMenuIfMissing(lCategoryMenuName, ComputeMenuName, ds.Category, , , True)
                                'Operations might have categories to further divide them
                                If lOperation.Definition.Category.Length > 0 Then
                                    Dim lSubCategoryName As String = lCategoryMenuName & "_" & lOperation.Definition.Category
                                    AddMenuIfMissing(lSubCategoryName, lCategoryMenuName, lOperation.Definition.Category, , , True)
                                    AddMenuIfMissing(lSubCategoryName & "_" & lOperation.Definition.Name, lSubCategoryName, lOperation.Definition.Name, , , True)
                                Else
                                    AddMenuIfMissing(lCategoryMenuName & "_" & lOperation.Definition.Name, lCategoryMenuName, lOperation.Definition.Name, , , True)
                                End If
                        End Select
                    Next
                Else
                    AddMenuIfMissing(lCategoryMenuName & "_" & ds.Description, lCategoryMenuName, ds.Description, , , True)
                End If
            End If
        Next
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks></remarks>
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
            lCurLayer = g_MapWin.Layers.Item(g_MapWin.Layers.CurrentLayer).GetObject

            If g_MapWin.View.SelectedShapes.NumSelected > 0 Then
                ctext = "Selected Features:"
                Select Case FilenameOnly(lCurLayer.Filename).ToLower
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
    End Sub

    Friend Sub ClearLayers()
        g_MapWin.Layers.Clear()
    End Sub

    Friend Sub RefreshView()
        g_MapWin.Refresh()
    End Sub
End Module