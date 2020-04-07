Imports System.Collections.Specialized
Imports System.Reflection
Imports atcUtility
Imports atcData
Imports MapWinUtility
#If GISProvider = "DotSpatial" Then
Imports DotSpatial.Controls
Imports DotSpatial.Extensions
#Else
Imports atcMwGisUtility
#End If

''' <summary>
''' 
''' </summary>
''' <remarks></remarks>
Public Module modBasinsPlugin
    'Declare this as global so that it can be accessed throughout the plug-in project.
    'These variables are initialized in the plugin_Initialize event.
#If GISProvider = "DotSpatial" Then
    Public g_MapWin As AppManager
    Friend g_Menus As Menu
    Friend g_StatusBar As StatusBar
    Friend g_Toolbar As ToolBar
    Friend g_Plugins As Object
    Friend g_Project As Object
    Friend g_MapWinWindowHandle As Integer
#Else
    Friend g_MapWin As MapWindow.Interfaces.IMapWin
    Friend g_Menus As MapWindow.Interfaces.Menus
    Friend g_StatusBar As MapWindow.Interfaces.StatusBar
    Friend g_Toolbar As MapWindow.Interfaces.Toolbar
    Friend g_Plugins As MapWindow.Interfaces.Plugins
    Friend g_Project As MapWindow.Interfaces.Project
    Friend g_MapWinWindowHandle As Integer
#End If

#If ProgramName = "USGS GW Toolbox" Then
    Public Const g_AppNameRegistry As String = "USGS-GW" 'For preferences in registry
    Friend Const g_AppNameShort As String = "GW Toolbox"
    Friend Const g_AppNameLong As String = "USGS GW Toolbox 1.3.2"
    Friend Const g_URL_Home As String = "http://water.usgs.gov/ogw/gwtoolbox/"
    Friend Const g_URL_Register As String = "http://hspf.com/pub/USGS-GW/register.html"
#ElseIf ProgramName = "USGS SW Toolbox" Then
    Public Const g_AppNameRegistry As String = "USGS-SW" 'For preferences in registry
    Friend Const g_AppNameShort As String = "SW Toolbox"
    Friend Const g_AppNameLong As String = "USGS SW Toolbox 1.0.5"
    Friend Const g_URL_Home As String = "https://water.usgs.gov/osw/swtoolbox/"
    Friend Const g_URL_Register As String = "http://hspf.com/pub/USGS-SW/register.html"
#ElseIf ProgramName = "USGS Hydrologic Toolbox" Then
    Public Const g_AppNameRegistry As String = "USGS-Hydro" 'For preferences in registry
    Friend Const g_AppNameShort As String = "Hydro Toolbox"
    Friend Const g_AppNameLong As String = "USGS Hydro Toolbox 1.0.0"
    Friend Const g_URL_Home As String = "https://water.usgs.gov/ogw/gwtoolbox/"
    Friend Const g_URL_Register As String = "http://hspf.com/pub/USGS-SW/register.html"
#Else
    Public Const g_AppNameRegistry As String = "BASINS" 'For preferences in registry
    Friend Const g_AppNameShort As String = "BASINS"
    Friend Const g_AppNameLong As String = "BASINS 4.5"
    Friend Const g_URL_Home As String = "http://www2.epa.gov/exposure-assessment-models/basins-user-information-and-guidance"
    Friend Const g_URL_Register As String = "http://hspf.com/pub/basins4/register.html"
#End If

    Friend g_BasinsDataDirs As New Generic.List(Of String)
    Friend g_ProgramDir As String = ""
    Public g_CacheDir As String = ""
    Friend g_ProgressPanel As System.Windows.Forms.Panel
    Friend pBuildFrm As frmBuildNew

    Friend pExistingMapWindowProjectName As String = ""
    Friend pCommandLineScript As Boolean = False

    'File menu -- created by MapWindow

    Friend Const ProjectsMenuName As String = "ProgramProjects"
    Friend ProjectsMenuString As String = "Open BASINS Project"

    Friend pWelcomeScreenShow As Boolean = False

    Friend Const RegisterMenuName As String = "RegisterProgram"
    Friend RegisterMenuString As String = "Register as a BASINS user"

    Friend Const CheckForUpdatesMenuName As String = "CheckForUpdates"
    Friend Const CheckForUpdatesMenuString As String = "Check For Updates"

    Friend Const ShowStatusMenuName As String = "ShowStatus"
    Friend Const ShowStatusMenuString As String = "Show Status Monitor"

    Friend Const TopHelpMenuName As String = "mnuHelp"
    Friend Const OurHelpMenuName As String = "BasinsHelp"
    Friend OurHelpMenuString As String = "BASINS Documentation"

    Friend Const ProgramWebPageMenuName As String = "ProgramWebPage"
    Friend ProgramWebPageMenuString As String = "BASINS Web Page"

    Friend Const SendFeedbackMenuName As String = "SendFeedback"
    Friend Const SendFeedbackMenuString As String = "Send Feedback"

    Friend Const DataMenuName As String = "BasinsData"
    Friend Const DataMenuString As String = "Data"
    Friend pLoadedDataMenu As Boolean = False

    Private Const Basins40DataPath As String = "Basins\data\"
    Private Const Basins41DataPath As String = "Basins41\data\"
    Private Const Basins42DataPath As String = "Basins42\data\"
    Friend BasinsDataPath As String = Basins41DataPath
    Private Const NationalProjectFilename As String = "national.mwprj"

    ''' <summary>
    ''' Find all of the data paths for this application on this system and add them to g_BasinsDataDirs
    ''' </summary>
    Friend Sub FindBasinsDrives()
        If g_BasinsDataDirs.Count = 0 Then
            If g_AppNameShort = "GW Toolbox" Then
                g_BasinsDataDirs.Add("C:\USGS-GWToolbox\data\")
            ElseIf g_AppNameShort = "SW Toolbox" Then
                g_BasinsDataDirs.Add("C:\USGS-SWToolbox\data\")
            ElseIf g_AppNameShort = "Hydro Toolbox" Then
                g_BasinsDataDirs.Add("C:\USGS-HydroToolbox\data\")
            End If
            Dim lSavedPaths As String = GetSetting(g_AppNameRegistry, "Folders", "DataPaths")
            If lSavedPaths.Length > 0 Then
                For Each lSavedPath As String In lSavedPaths.Split(";")
                    If FileExists(lSavedPath, True, False) Then
                        g_BasinsDataDirs.Add(lSavedPath)
                    Else
                        Logger.Dbg("Did not find saved path: " & lSavedPath)
                    End If
                Next
            End If

            If g_BasinsDataDirs.Count = 0 Then

                Dim lCheckDir As String = DefaultBasinsDataDir()
                If FileExists(lCheckDir, True, False) Then g_BasinsDataDirs.Add(lCheckDir)

                AddExistingDirs(BasinsDataPath)

                Try
                    CheckDataDir(IO.Path.Combine(IO.Path.GetDirectoryName(IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly.Location)), "data") & g_PathChar)
                Catch
                End Try

                If Not BasinsDataPath.Equals(Basins42DataPath) Then
                    'If this is not BASINS42, also look for a BASINS42 data folder
                    AddExistingDirs(Basins42DataPath)
                End If

                If Not BasinsDataPath.Equals(Basins41DataPath) Then
                    'If this is not BASINS41, also look for a BASINS41 data folder
                    AddExistingDirs(Basins41DataPath)
                End If

                If Not BasinsDataPath.Equals(Basins40DataPath) Then
                    'look for older BASINS data folders too
                    AddExistingDirs(Basins40DataPath)
                End If

                Select Case g_BasinsDataDirs.Count
                    Case 0 : Logger.Msg("No " & BasinsDataPath & " folders found on any drives on this computer", "Find Data")
                    Case 1 : Logger.Dbg("Found data path: " & g_BasinsDataDirs(0))
                    Case Is > 1
                        Dim lAllDirs As String = ""
                        For Each lDir As String In g_BasinsDataDirs
                            lAllDirs &= lDir & "  "
                        Next
                        Logger.Dbg("Found data paths: " & lAllDirs)
                End Select
                SaveSetting(g_AppNameRegistry, "Folders", "DataPaths", String.Join(";", g_BasinsDataDirs.ToArray))
            End If
        End If
    End Sub

    ''' <summary>
    ''' Search all local hard drives for the given path. Add all found paths to g_BasinsDataDirs.
    ''' </summary>
    ''' <param name="aPathToSearchFor">Search for this path</param>
    ''' <remarks></remarks>
    Private Sub AddExistingDirs(ByVal aPathToSearchFor As String)
        Dim lCheckDir As String
        For Each lDriveInfo As IO.DriveInfo In IO.DriveInfo.GetDrives
            Try
                With lDriveInfo
                    lCheckDir = .Name & aPathToSearchFor
                    If Not g_BasinsDataDirs.Contains(lCheckDir) Then
                        Logger.Status("Checking for " & lCheckDir, True)
                        If .IsReady AndAlso .DriveType = IO.DriveType.Fixed OrElse .DriveType = IO.DriveType.Network Then
                            CheckDataDir(lCheckDir)
                        End If
                    End If
                End With
            Catch 'Skip drives that we cannot successfully check
            End Try
        Next
        Try
            CheckDataDir(IO.Path.Combine(My.Computer.FileSystem.SpecialDirectories.ProgramFiles, aPathToSearchFor))
        Catch
        End Try
        Try
            CheckDataDir(IO.Path.Combine(My.Computer.FileSystem.SpecialDirectories.MyDocuments, aPathToSearchFor))
        Catch
        End Try
        Logger.Status("")
    End Sub

    Private Sub CheckDataDir(ByVal aDirectory As String)
        If Not g_BasinsDataDirs.Contains(aDirectory) AndAlso FileExists(aDirectory, True, False) Then
            g_BasinsDataDirs.Add(aDirectory)
        End If
    End Sub

    ''' <summary>
    ''' 1. Load the nationwide project file shipped with the application, probably "national.mwprj"
    ''' 2. Select the catalog units layer.
    ''' 3. Open the form for building a new project.
    ''' </summary>
    Public Sub LoadNationalProject()
        If Not NationalProjectIsOpen() Then
            Dim lFileName As String = IO.Path.Combine(g_ProgramDir, "Data\national" & g_PathChar & NationalProjectFilename)
            If Not FileExists(lFileName) Then
                For Each lDir As String In g_BasinsDataDirs
                    lFileName = lDir & "national" & g_PathChar & NationalProjectFilename
                    If FileExists(lFileName) Then 'found existing national project
                        Exit For
                    End If
                Next
            End If
            If Not FileExists(lFileName) Then
                lFileName = FindFile("Please locate BASINS national project", NationalProjectFilename, , , True)
            End If
            If FileExists(lFileName) Then  'load national project
                g_Project.Load(lFileName)
#If GISProvider = "DotSpatial" Then

#Else
                g_MapWin.View.ClearSelectedShapes()
#End If
                g_Project.Modified = False
                'See if we need to also process and load place names
                'Dim lInstructions As String = D4EMDataManager.SpatialOperations.CheckPlaceNames(IO.Path.GetDirectoryName(lFileName), g_Project.ProjectProjection)
                'If lInstructions.Length > 0 Then
                '    Dim lDisplayMessageBoxes As Boolean = Logger.DisplayMessageBoxes
                '    Logger.DisplayMessageBoxes = False 'Don't show a message box after adding these layers
                '    ProcessDownloadResults(lInstructions)
                '    Logger.DisplayMessageBoxes = lDisplayMessageBoxes
                'End If
            Else
                Logger.Msg("Unable to find '" & NationalProjectFilename & "'", "Open National")
                Exit Sub
            End If
        End If

        If NationalProjectIsOpen() Then
            'Select the Cataloging Units layer by default 
#If GISProvider = "DotSpatial" Then
#Else
            For iLayer As Integer = 0 To g_MapWin.Layers.NumLayers - 1
                If g_MapWin.Layers(g_MapWin.Layers.GetHandle(iLayer)).Name = "Cataloging Units" Then
                    g_MapWin.Layers.CurrentLayer = g_MapWin.Layers.GetHandle(iLayer)
                    Exit For
                End If
            Next
            g_Toolbar.PressToolbarButton("tbbSelect")
#End If
            pBuildFrm = New frmBuildNew
            pBuildFrm.Show()
            Try
                pBuildFrm.Top = GetSetting(g_AppNameRegistry, "Window Positions", "BuildTop", "300")
                If pBuildFrm.Top < 0 Then
                    pBuildFrm.Top = 0
                ElseIf pBuildFrm.Top + pBuildFrm.Height > System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height Then
                    pBuildFrm.Top = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height - pBuildFrm.Height
                End If
            Catch
            End Try

            Try
                pBuildFrm.Left = GetSetting(g_AppNameRegistry, "Window Positions", "BuildLeft", "0")
                If pBuildFrm.Left < 0 Then
                    pBuildFrm.Left = 0
                ElseIf pBuildFrm.Left + pBuildFrm.Width > System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width Then
                    pBuildFrm.Left = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width - pBuildFrm.Width
                End If
            Catch
            End Try

            UpdateSelectedFeatures()
        Else
            Logger.Msg("Unable to open national project", "Open National")
        End If
    End Sub

    ''' <summary>
    ''' True if the national project is currently open, False if it is not
    ''' </summary>
    Public Function NationalProjectIsOpen() As Boolean
        Return (g_Project IsNot Nothing) _
            AndAlso (g_Project.FileName IsNot Nothing) _
            AndAlso g_Project.FileName.ToLower.EndsWith(NationalProjectFilename.ToLower)
    End Function

    ''' <summary>
    ''' True if current project has both a catalog unit layer and a state layer
    ''' </summary>
    Friend Function IsBASINSProject() As Boolean
        Dim lHaveCatLayer As Boolean = False
        Dim lHaveStateLayer As Boolean = False
#If GISProvider = "DotSpatial" Then
        For Each lLayer As IMapFeatureLayer In g_MapWin.Map.GetLayers()
#Else
        For Each lLayer As MapWindow.Interfaces.Layer In g_MapWin.Layers
#End If
#If GISProvider = "DotSpatial" Then
            Select Case FilenameNoPath(lLayer.DataSet.Filename).ToLower
#Else
            Select Case FilenameNoPath(lLayer.FileName).ToLower
#End If
                Case "st.shp" : lHaveStateLayer = True : If lHaveCatLayer Then Exit For
                Case "cat.shp" : lHaveCatLayer = True : If lHaveStateLayer Then Exit For
            End Select
        Next
        Return lHaveCatLayer And lHaveStateLayer
    End Function

    ''' <summary>
    ''' Try two ways of finding a data directory in the user's own directory, default to a directory in the root if not found
    ''' </summary>
    Public Function DefaultBasinsDataDir() As String
        If g_BasinsDataDirs IsNot Nothing AndAlso g_BasinsDataDirs.Count > 0 Then
            'Already found at least one data directory, default to the first one
            Return g_BasinsDataDirs(0)
        Else
            Dim lDir As String
            Try
                lDir = My.Computer.FileSystem.SpecialDirectories.MyDocuments & g_PathChar & BasinsDataPath
                If IO.Directory.Exists(lDir) Then Return lDir
            Catch
            End Try
            Try
                lDir = Environment.GetFolderPath(Environment.SpecialFolder.Personal) & g_PathChar & BasinsDataPath
                If IO.Directory.Exists(lDir) Then Return lDir
            Catch
            End Try
            Return IO.Path.GetPathRoot(CurDir) & BasinsDataPath
        End If
    End Function

    Friend Sub UpdateSelectedFeatures()
#If GISProvider = "DotSpatial" Then
#Else
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
#End If
    End Sub

#If GISProvider = "DotSpatial" Then
    Public Sub Initialize(ByVal aSettingArgs As atcData.atcDataAttributes)
        Dim lMapWin As AppManager = Nothing
        If aSettingArgs IsNot Nothing Then
            With aSettingArgs
                lMapWin = .GetValue("MapWin", Nothing)
                g_CacheDir = .GetValue("CacheDir", "")
            End With
        End If
        If lMapWin IsNot Nothing Then
            Dim lPlugin As atcBasinsPlugIn = New atcBasinsPlugIn()
            lPlugin.Initialize(lMapWin, Nothing)
        Else
            Throw New ApplicationException("Map Control Cannot Be Found.")
        End If
    End Sub
#End If

    Friend Sub ClearLayers()
#If GISProvider = "DotSpatial" Then
        g_MapWin.Map.Layers.Clear()
#Else
        g_MapWin.Layers.Clear()
#End If
    End Sub

    Friend Sub RefreshView()
#If GISProvider = "DotSpatial" Then
        g_MapWin.Map.Refresh()
#Else
        g_MapWin.Refresh()
#End If
    End Sub
End Module