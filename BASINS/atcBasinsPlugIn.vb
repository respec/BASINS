Imports System.Collections.Specialized
Imports System.Windows.Forms.Application
Imports atcUtility
Imports atcData
Imports MapWinUtility
#If GISProvider = "DotSpatial" Then
Imports DotSpatial.Controls
Imports DotSpatial.Extensions
#Else
Imports atcMwGisUtility
#End If

Public Class atcBasinsPlugIn
#If GISProvider = "DotSpatial" Then
#Else
    Implements MapWindow.Interfaces.IPlugin

#Region "Plug-in Information"
    Public ReadOnly Property Name() As String Implements MapWindow.Interfaces.IPlugin.Name
        'This is the name that appears in the Plug-ins menu
        Get
            Return g_AppNameLong
        End Get
    End Property

    Public ReadOnly Property Author() As String Implements MapWindow.Interfaces.IPlugin.Author
        Get
            Return "AQUA TERRA Consultants"
        End Get
    End Property

    Public ReadOnly Property SerialNumber() As String Implements MapWindow.Interfaces.IPlugin.SerialNumber
        Get
            Return "G14R/KCU1FOWVVI"
        End Get
    End Property

    Public ReadOnly Property Description() As String Implements MapWindow.Interfaces.IPlugin.Description
        'Appears in the plug-ins dialog box when a user selects the plug-in.  
        Get
            Return g_AppNameLong & " extension"
        End Get
    End Property

    Public ReadOnly Property BuildDate() As String Implements MapWindow.Interfaces.IPlugin.BuildDate
        Get
            Return IO.File.GetLastWriteTime(Me.GetType().Assembly.Location)
        End Get
    End Property

    Public ReadOnly Property Version() As String Implements MapWindow.Interfaces.IPlugin.Version
        Get
            Return Diagnostics.FileVersionInfo.GetVersionInfo(Me.GetType().Assembly.Location).FileVersion
        End Get
    End Property
#End Region
#End If
    Private pStatusMonitor As MonitorProgressStatus
#If GISProvider = "DotSpatial" Then
    <CLSCompliant(False)>
    Public ReadOnly Property MapWin() As AppManager
#Else
    <CLSCompliant(False)> _
    Public ReadOnly Property MapWin() As MapWindow.Interfaces.IMapWin
#End If
        Get
            Return g_MapWin
        End Get
    End Property
#If GISProvider = "DotSpatial" Then
    Public Property MainWin() As Form
        Get
            Return g_MainWin
        End Get
        Set(value As Form)
            g_MainWin = value
        End Set
    End Property
    <CLSCompliant(False)>
    Public Sub Initialize(ByVal aMapWin As AppManager, ByVal aParentHandle As Integer)
#Else
    <CLSCompliant(False)> _
    Public Sub Initialize(ByVal aMapWin As MapWindow.Interfaces.IMapWin, ByVal aParentHandle As Integer) Implements MapWindow.Interfaces.IPlugin.Initialize
#End If
        'fired when 
        '   1) user loads plug-in through plug-in dialog or
        '      by checkmarking it in the plug-ins menu.
        '   2) project refererencing plug-in loads

        'This is where buttons or menu items are added.
        g_MapWin = aMapWin
#If GISProvider = "DotSpatial" Then
        'g_MainWin = aMainWin
#Else
        GisUtil.MappingObject = g_MapWin
#End If
        atcDataManager.MapWindow = g_MapWin

        'These should match, but now g_AppNameLong is a constant and we do not set it here.
        'g_AppNameLong = aMapWin.ApplicationInfo.ApplicationName

        ProjectsMenuString = "Open " & g_AppNameShort & " Project"
        RegisterMenuString = "Register as a " & g_AppNameShort & " user"
        ProgramWebPageMenuString = g_AppNameShort & " Web Page"
        OurHelpMenuString = g_AppNameShort & " Documentation"

        g_MapWinWindowHandle = aParentHandle
#If GISProvider = "DotSpatial" Then
#Else
        g_MapWin.ApplicationInfo.WelcomePlugin = "plugin" 'tell the main app to Plugins.BroadcastMessage("WELCOME_SCREEN") instead of showing default MW welcome screen
#End If
        'Set g_ProgramDir to folder above the Bin folder where the app and plugins live
        g_ProgramDir = PathNameOnly(PathNameOnly(Reflection.Assembly.GetEntryAssembly.Location)) & g_PathChar

        'Using RegCacheDir allows a user to specify a custom cache folder via regedit by opening registry folder:
        ' HKEY_CURRENT_USER\SOFTWARE\VB And VBA Program Settings\BASINS\DataDownload
        ' and setting string value Cache_dir
        Dim RegCacheDir As String = GetSetting(g_AppNameRegistry, "DataDownload", "Cache_dir")
        If IsWritableFolder(RegCacheDir) Then
            g_CacheDir = RegCacheDir
        Else
            g_CacheDir = IO.Path.Combine(g_ProgramDir, "cache")
            If Not IsWritableFolder(g_CacheDir) Then
                Try
                    IO.Directory.CreateDirectory(g_CacheDir)
                Catch ex As Exception
                    g_CacheDir = IO.Path.GetTempPath
                End Try
            End If
        End If
        If Not g_CacheDir.EndsWith(g_PathChar) Then
            g_CacheDir &= g_PathChar
        End If
        If g_CacheDir <> RegCacheDir Then
            SaveSetting(g_AppNameRegistry, "DataDownload", "Cache_dir", g_CacheDir)
        End If

        Dim lHelpFilename As String = String.Empty
        Select Case g_AppNameShort
            Case "SW Toolbox"
                lHelpFilename = FindFile("Please Find Help Document", g_ProgramDir & "docs\USGSToolbox.chm")
                BasinsDataPath = "USGS-SW\data\"
                With atcUtility.HelpSubstitutions
                    .Add("BASINS Details", "SW Toolbox Details")
                    .Add("Welcome to BASINS 4.5 Window", "Welcome to SW Toolbox Window")
                    .Add("Build BASINS Project", "Build New Project")
                End With

            Case "GW Toolbox"
                lHelpFilename = FindFile("Please Find Help Document", g_ProgramDir & "docs\GWToolbox.chm")
                BasinsDataPath = "USGS-GW\data\"
                With atcUtility.HelpSubstitutions
                    .Add("BASINS Details", "GW Toolbox Details")
                    .Add("Welcome to BASINS 4.5 Window", "Welcome to GW Toolbox Window")
                    .Add("Build BASINS Project", "Build New Project")
                End With
            Case "Hydro Toolbox"
                lHelpFilename = FindFile("Please Find Help Document", g_ProgramDir & "docs\GWToolbox.chm")
                BasinsDataPath = "USGS-WATER\data\"
                With atcUtility.HelpSubstitutions
                    .Add("BASINS Details", "GW Toolbox Details")
                    .Add("Welcome to BASINS 4.5 Window", "Welcome to GW Toolbox Window")
                    .Add("Build BASINS Project", "Build New Project")
                End With

            Case Else ' BASINS 
                lHelpFilename = FindFile("", g_ProgramDir & "docs\BASINS4.5.chm")
        End Select

        Logger.StartToFile(g_CacheDir & "log" & g_PathChar _
                         & Format(Now, "yyyy-MM-dd") & "at" & Format(Now, "HH-mm") & "-" & g_AppNameShort.Replace(" ", "") & ".log")
#If GISProvider = "DotSpatial" Then
#Else
        Logger.Icon = g_MapWin.ApplicationInfo.FormIcon
#End If
        If Logger.ProgressStatus Is Nothing OrElse Not (TypeOf (Logger.ProgressStatus) Is MonitorProgressStatus) Then
            'Start running status monitor to give better progress and status indication during long-running processes
            pStatusMonitor = New MonitorProgressStatus
            If pStatusMonitor.StartMonitor(FindFile("Find Status Monitor", "StatusMonitor.exe"),
                                            g_CacheDir & "log" & g_PathChar,
                                            System.Diagnostics.Process.GetCurrentProcess.Id) Then
                'put our status monitor (StatusMonitor.exe) between the Logger and the default MW status monitor
                pStatusMonitor.InnerProgressStatus = Logger.ProgressStatus
                Logger.ProgressStatus = pStatusMonitor
                Logger.Status("LABEL TITLE " & g_AppNameShort & " Status")
                Logger.Status("PROGRESS TIME ON") 'Enable time-to-completion estimation
                Logger.Status("")
            Else
                pStatusMonitor.StopMonitor()
                pStatusMonitor = Nothing
            End If
        End If

        'CheckForUpdates(True)

        atcDataManager.LoadPlugin("Timeseries::Statistics")

        If FileExists(lHelpFilename) Then
            ShowHelp(lHelpFilename)
        Else
            Logger.Dbg("Help File Not Found")
        End If
        'BuiltInScript(False)

        FindBasinsDrives()

#If GISProvider = "DotSpatial" Then
        g_Project = g_MapWin.SerializationManager
#Else
        g_Menus = g_MapWin.Menus
        g_StatusBar = g_MapWin.StatusBar
        g_Toolbar = g_MapWin.Toolbar
        g_Plugins = g_MapWin.Plugins
        g_Project = g_MapWin.Project

        atcDataManager.AddMenuIfMissing(ProjectsMenuName, atcDataManager.FileMenuName, ProjectsMenuString, "mnuRecentProjects")

        atcDataManager.AddMenuIfMissing(OurHelpMenuName, TopHelpMenuName, OurHelpMenuString, , "mnuOnlineDocs")
        atcDataManager.AddMenuIfMissing(ProgramWebPageMenuName, TopHelpMenuName, ProgramWebPageMenuString, , "mnuOnlineDocs")
#End If

        Select Case g_AppNameShort
            Case "SW Toolbox"
            Case "GW Toolbox"
            Case Else ' BASINS 
                'atcDataManager.AddMenuIfMissing(RegisterMenuName, TopHelpMenuName, RegisterMenuString, , "mnuShortcuts")
        End Select

#If GISProvider = "DotSpatial" Then
#Else
        g_Menus.Remove("mnuCheckForUpdates") 'Remove MW update menu so only ours will be present
        g_Menus.Remove("mnuFileBreak5")      'Remove MW separator after mnuCheckForUpdates
        'atcDataManager.AddMenuIfMissing(CheckForUpdatesMenuName, TopHelpMenuName, CheckForUpdatesMenuString, RegisterMenuName)
        atcDataManager.AddMenuIfMissing(ShowStatusMenuName, TopHelpMenuName, ShowStatusMenuString, CheckForUpdatesMenuName)
        'atcDataManager.AddMenuIfMissing(SendFeedbackMenuName, TopHelpMenuName, SendFeedbackMenuString, ShowStatusMenuName)

        Dim lMenuItem As MapWindow.Interfaces.MenuItem
        For Each lDataDir As String In g_BasinsDataDirs
            For Each lProjectDir As String In IO.Directory.GetDirectories(lDataDir)
                For Each lProjectFilename As String In IO.Directory.GetFiles(lProjectDir, "*.mwprj")
                    Dim DirShortName As String = lProjectFilename
                    If g_BasinsDataDirs.Count < 2 Then
                        'shorten path since we don't need to differentiate between different data dirs
                        DirShortName = IO.Path.GetFileNameWithoutExtension(lProjectFilename)
                    End If
                    lMenuItem = atcDataManager.AddMenuIfMissing(ProjectsMenuName & "_" & DirShortName, ProjectsMenuName, DirShortName)
                    lMenuItem.Tooltip = lProjectFilename
                Next
            Next
        Next
#End If

        pLoadedDataMenu = True

        'If g_AppNameShort = "BASINS" Then
        '    atcDataManager.AddMenuIfMissing(atcDataManager.LaunchMenuName, "", atcDataManager.LaunchMenuString, atcDataManager.FileMenuName)
        '    atcDataManager.AddMenuIfMissing(atcDataManager.LaunchMenuName & "_ArcView3", atcDataManager.LaunchMenuName, "ArcView 3")
        '    atcDataManager.AddMenuIfMissing(atcDataManager.LaunchMenuName & "_ArcGIS", atcDataManager.LaunchMenuName, "ArcGIS")
        '    atcDataManager.AddMenuIfMissing(atcDataManager.LaunchMenuName & "_GenScn", atcDataManager.LaunchMenuName, "GenScn")
        '    atcDataManager.AddMenuIfMissing(atcDataManager.LaunchMenuName & "_WDMUtil", atcDataManager.LaunchMenuName, "WDMUtil")
        'End If

        atcDataManager.LoadPlugin("D4EM Data Download::Main")

        Try 'atcDataManager.XML gets loaded when opening a project. This makes sure it gets loaded even without a project
            Dim lAttributesString As String = GetSetting(g_AppNameRegistry, "DataManager", "SelectionAttributes")
            If lAttributesString.Length > 0 Then
                atcDataManager.SelectionAttributesSet(lAttributesString.Split(vbTab))
            End If
            lAttributesString = GetSetting(g_AppNameRegistry, "DataManager", "DisplayAttributes")
            If lAttributesString.Length > 0 Then
                atcDataManager.DisplayAttributesSet(lAttributesString.Split(vbTab))
            End If
        Catch
        End Try
        'g_ProgressPanel = g_MapWin.UIPanel.CreatePanel("Progress", MapWindow.Interfaces.MapWindowDockStyle.Top)
        'g_ProgressPanel.Visible = False
    End Sub

#If GISProvider = "DotSpatial" Then
#Else
    Public Sub Terminate() Implements MapWindow.Interfaces.IPlugin.Terminate
        On Error Resume Next
        g_MapWin.Menus.Remove(ProjectsMenuName)
        g_MapWin.Menus.Remove(OurHelpMenuName)
        g_MapWin.Menus.Remove(ProgramWebPageMenuName)
        'g_MapWin.Menus.Remove(RegisterMenuName)
        'g_MapWin.Menus.Remove(CheckForUpdatesMenuName)
        g_MapWin.Menus.Remove(ShowStatusMenuName)
        'g_MapWin.Menus.Remove(SendFeedbackMenuName)

        'g_MapWin.Menus.Remove(atcDataManager.LaunchMenuName & "_ArcView3")
        'g_MapWin.Menus.Remove(atcDataManager.LaunchMenuName & "_ArcGIS")
        'g_MapWin.Menus.Remove(atcDataManager.LaunchMenuName & "_GenScn")
        'g_MapWin.Menus.Remove(atcDataManager.LaunchMenuName & "_WDMUtil")
        'atcDataManager.RemoveMenuIfEmpty(atcDataManager.LaunchMenuName)

        ShowHelp("CLOSE") 'Close any active Help window

        pLoadedDataMenu = False

        g_MapWin.ApplicationInfo.WelcomePlugin = ""
        g_MapWin.ClearCustomWindowTitle()

        CloseForms()

        If pStatusMonitor IsNot Nothing Then
            Logger.ProgressStatus = pStatusMonitor.InnerProgressStatus
            pStatusMonitor.StopMonitor()
        End If
        Dim lSelectionAttributes As String = String.Join(vbTab, atcDataManager.SelectionAttributes.ToArray())
        'Logger.Dbg("Saving SelectionAttributes " & lSelectionAttributes)
        SaveSetting(g_AppNameRegistry, "DataManager", "SelectionAttributes", lSelectionAttributes)

        Dim lDisplayAttributes As String = String.Join(vbTab, atcDataManager.DisplayAttributes.ToArray())
        'Logger.Dbg("Saving DisplayAttributes " & lDisplayAttributes)
        SaveSetting(g_AppNameRegistry, "DataManager", "DisplayAttributes", lDisplayAttributes)
    End Sub
#End If

    Private Sub CloseForms()
        If pBuildFrm IsNot Nothing Then
            Try
                pBuildFrm.Close()
            Catch
            End Try
        End If
    End Sub

#If GISProvider = "DotSpatial" Then
    Public Sub ItemClicked(ByVal aItemName As String, ByRef aHandled As Boolean)
#Else
    Public Sub ItemClicked(ByVal aItemName As String, ByRef aHandled As Boolean) Implements MapWindow.Interfaces.IPlugin.ItemClicked
#End If
        'A menu item or toolbar button was clicked
        Logger.Dbg(aItemName)
        Dim lProgramFolder As String = My.Computer.Registry.GetValue("HKEY_LOCAL_MACHINE\SOFTWARE\AQUA TERRA Consultants\" & g_AppNameRegistry, "Base Directory", "C:\" & g_AppNameRegistry)
        aHandled = True 'Assume we will handle it
        Select Case aItemName
            Case "mnuNew", "tbbNew"  'Override new project behavior
                BASINSNewMenu()
            Case "mnuAboutMapWindow" 'Override Help/About menu
                Dim lAbout As New frmAbout
                lAbout.ShowAbout()
            Case RegisterMenuName
                OpenFile(g_URL_Register)
            Case CheckForUpdatesMenuName
                CheckForUpdates(False)
            Case ProgramWebPageMenuName
                OpenFile(g_URL_Home)
            Case SendFeedbackMenuName
                SendFeedback()
            Case ShowStatusMenuName
                Logger.Status("SHOW")
            Case OurHelpMenuName
                ShowHelp("")
                'Case atcDataManager.LaunchMenuName & "_ArcView3"
                '    'create apr if it does not exist, then open it
                '    Dim lAprFileName As String = lProgramFolder & "\apr" & g_PathChar & IO.Path.GetFileNameWithoutExtension(g_Project.FileName) & ".apr"
                '    If Not FileExists(lAprFileName) Then 'build it
                '        Dim lEmptyAprName As String = lProgramFolder & "\etc\buildapr.dat"
                '        If FileExists(lEmptyAprName) Then
                '            IO.Directory.CreateDirectory(IO.Path.GetDirectoryName(lAprFileName))
                '            IO.File.Copy(lEmptyAprName, lAprFileName)
                '        Else
                '            Logger.Msg("Unable to locate template apr file buildapr.dat", vbOKOnly, "Launch ArcView Problem")
                '        End If
                '    End If
                '    Try
                '        Process.Start(lAprFileName)
                '    Catch
                '        Logger.Msg("No application is associated with APR files - ArcView3 does not appear to be installed.", vbOKOnly, "Launch ArcView Problem")
                '    End Try
                'Case atcDataManager.LaunchMenuName & "_ArcGIS"
                '    'create mxd if it does not exist, then open it
                '    Dim lMxdFileName As String = lProgramFolder & "\mxd" & g_PathChar & IO.Path.GetFileNameWithoutExtension(g_Project.FileName) & ".mxd"
                '    If Not FileExists(lMxdFileName) Then 'build it
                '        Dim lEmptyMxdName As String = lProgramFolder & "\etc\buildmxd.dat"
                '        If FileExists(lEmptyMxdName) Then
                '            IO.Directory.CreateDirectory(IO.Path.GetDirectoryName(lMxdFileName))
                '            IO.File.Copy(lEmptyMxdName, lMxdFileName)
                '        Else
                '            Logger.Msg("Unable to locate template mxd file buildmxd.dat", vbOKOnly, "Launch ArcGIS Problem")
                '        End If
                '    End If
                '    Try
                '        Process.Start(lMxdFileName)
                '    Catch
                '        Logger.Msg("No application is associated with MXD files - ArcGIS does not appear to be installed.", vbOKOnly, "Launch ArcGIS Problem")
                '    End Try
            Case Else
                If aItemName.StartsWith(atcDataManager.LaunchMenuName & "_") Then
                    Dim lExeName As String = ""
                    'Select Case aItemName.Substring(atcDataManager.LaunchMenuName.Length + 1).ToLower
                    '    Case "genscn" : lExeName = FindFile("Please locate GenScn.exe", lProgramFolder & "\models\HSPF\bin\GenScn.exe")
                    '    Case "wdmutil" : lExeName = FindFile("Please locate WDMUtil.exe", lProgramFolder & "\models\HSPF\WDMUtil\WDMUtil.exe")
                    'End Select
                    If FileExists(lExeName) Then
                        Shell("""" & lExeName & """", AppWinStyle.NormalFocus, False)
                        aHandled = True
                    Else
                        Logger.Dbg("Unable to launch " & aItemName, "Launch")
                        aHandled = False
                    End If
                ElseIf aItemName.StartsWith(ProjectsMenuName & "_") Then
#If GISProvider = "DotSpatial" Then
                    aHandled = UserOpenProject(aItemName)
#Else
                    aHandled = UserOpenProject(g_Menus(aItemName).Text)
#End If
                Else
                    aHandled = False 'Not our item to handle
                End If
        End Select
    End Sub

    Private Sub CheckForUpdates(ByVal aQuiet As Boolean)
        Try
            Dim lQuiet As String = ""
            Dim lToday As String = Format(Date.Today, "yyyy-MM-dd")
            If aQuiet Then 'Make sure automatic checking happens at most once a day
                If GetSetting(g_AppNameRegistry, "Update", "LastCheck", "Never") = lToday Then Exit Sub
                lQuiet = "quiet "
            End If
            SaveSetting(g_AppNameRegistry, "Update", "LastCheck", lToday)

            Dim lExePath As String = IO.Path.GetDirectoryName(Reflection.Assembly.GetEntryAssembly.Location)
            Dim lUpdateCheckerPath As String = IO.Path.Combine(lExePath, "UpdateCheck.exe")
            If IO.File.Exists(lUpdateCheckerPath) Then
                Shell("""" & lUpdateCheckerPath & """" & " " _
                    & lQuiet _
                    & Process.GetCurrentProcess.Id & " " _
                    & """" & g_CacheDir & """", AppWinStyle.Hide)
            ElseIf Not aQuiet Then 'If manually checking and UpdateCheck.exe is not found, open update web page
                Logger.Dbg("Did not find update checker at '" & lUpdateCheckerPath & "'")
                OpenFile("http://hspf.com/pub/basins41/updates.html")
            End If
        Catch ex As Exception
            If aQuiet Then
                Logger.Dbg("Error in CheckForUpdates: " & ex.Message)
            Else
                Logger.Msg("Error: " & ex.Message, "Check For Updates")
            End If
        End Try
    End Sub

    Private Function UserOpenProject(ByVal aDataDirName As String) As Boolean
        Dim lPrjFileName As String = Nothing

        If Not FileExists(aDataDirName, True, False) Then
            'Look for project in BASINS dirs
            For Each lDataDir As String In g_BasinsDataDirs
                For Each lProjectDir As String In IO.Directory.GetDirectories(lDataDir)
                    If IO.Path.GetFileName(lProjectDir) = aDataDirName Then
                        aDataDirName = lProjectDir
                        GoTo FoundDir
                    End If
                    For Each lScanProjectFilename As String In IO.Directory.GetFiles(lProjectDir, "*.mwprj")
                        If lScanProjectFilename.Contains(aDataDirName) Then
                            aDataDirName = lProjectDir
                            lPrjFileName = lScanProjectFilename
                            GoTo FoundDir
                        End If
                    Next
                Next
            Next
        End If

        If FileExists(aDataDirName, True, False) Then
FoundDir:
#If GISProvider = "DotSpatial" Then
            If g_Project IsNot Nothing AndAlso g_Project.IsDirty Then
                If PromptToSaveProject(g_Project.CurrentProjectFile) = MsgBoxResult.Cancel Then
                    Return False
                End If
            End If
#Else
            If g_Project.Modified Then
                If PromptToSaveProject(g_Project.FileName) = MsgBoxResult.Cancel Then
                    Return False
                End If
            End If
#End If

            If lPrjFileName Is Nothing Then
                lPrjFileName = aDataDirName & g_PathChar & IO.Path.GetFileNameWithoutExtension(aDataDirName) & ".mwprj"
            End If

            If FileExists(lPrjFileName) Then
                Logger.Dbg("Opening project " & lPrjFileName)
#If GISProvider = "DotSpatial" Then
                Try
                    g_Project.OpenProject(lPrjFileName)
                    Return True
                Catch ex As Exception
                    Return False
                End Try
#Else
                Return g_Project.Load(lPrjFileName)
#End If
            Else
                'TODO: look for other *.mwprj before creating a new one?
                Logger.Dbg("Creating new project " & lPrjFileName)
                ClearLayers()
                RefreshView()
#If GISProvider = "DotSpatial" Then
                DoEvents()
                g_Project.SaveProject(lPrjFileName)
                AddAllShapesInDir(aDataDirName, aDataDirName)
#Else
                g_MapWin.PreviewMap.GetPictureFromMap()
                DoEvents()
                AddAllShapesInDir(aDataDirName, aDataDirName)
                g_Project.Save(lPrjFileName)
                g_Project.Modified = False
#End If
                Return True
            End If
        End If
        Return False
    End Function

    'TODO: merge with function of same name in MapWindow:frmMain
    Private Function PromptToSaveProject(ByVal aProjectFileName As String) As MsgBoxResult
        Dim lResult As MsgBoxResult
#If GISProvider = "DotSpatial" Then
        Dim lTitle As String = "Save Changes?"
#Else
        Dim lTitle As String = g_MapWin.ApplicationInfo.ApplicationName & " Save Changes?"
#End If

        If aProjectFileName Is Nothing OrElse FilenameNoExt(aProjectFileName) = "" Then
            lResult = Logger.Msg("Do you want to save the changes to the currently open project?",
                             MsgBoxStyle.YesNoCancel Or MsgBoxStyle.Exclamation, lTitle)
        Else
            lResult = Logger.Msg("Do you want to save the changes to " & FilenameNoExt(aProjectFileName) & "?",
                             MsgBoxStyle.YesNoCancel Or MsgBoxStyle.Exclamation, lTitle)
        End If

        Select Case lResult
            Case MsgBoxResult.Yes
                Dim lCdlSave As New SaveFileDialog
                lCdlSave.Filter = "MapWindow Project (*.mwprj)|*.mwprj"
#If GISProvider = "DotSpatial" Then
                If g_Project.IsDirty And (Not String.IsNullOrEmpty(aProjectFileName)) Then
                    g_Project.SaveProject(aProjectFileName)
                    'g_Project.Modified = False
                Else
                    If lCdlSave.ShowDialog() = DialogResult.Cancel Then
                        Return MsgBoxResult.Cancel
                    End If

                    If (System.IO.Path.GetExtension(lCdlSave.FileName) <> ".mwprj") Then
                        lCdlSave.FileName &= ".mwprj"
                    End If
                    g_Project.SaveProject(lCdlSave.FileName)
                    'g_Project.Modified = False
                End If
#Else
                If g_Project.Modified = True And MapWinUtility.Strings.IsEmpty(aProjectFileName) = False Then
                    g_Project.Save(aProjectFileName)
                    g_Project.Modified = False
                Else
                    If lCdlSave.ShowDialog() = DialogResult.Cancel Then
                        Return MsgBoxResult.Cancel
                    End If

                    If (System.IO.Path.GetExtension(lCdlSave.FileName) <> ".mwprj") Then
                        lCdlSave.FileName &= ".mwprj"
                    End If
                    g_Project.Save(lCdlSave.FileName)
                    g_Project.Modified = False
                End If
#End If
                Return MsgBoxResult.Yes
            Case MsgBoxResult.Cancel
                Return MsgBoxResult.Cancel
            Case MsgBoxResult.No
                Return MsgBoxResult.No
        End Select
    End Function

    Private Sub SendFeedback()
        Dim lName As String = ""
        Dim lEmail As String = ""
        Dim lMessage As String = ""

        Dim lFeedbackForm As New frmFeedback
        Dim lFeedback As String = ""

#If GISProvider = "DotSpatial" Then
        lFeedback &= "Project: " & g_Project.CurrentProjectFile & vbCrLf
#Else
        lFeedback &= "Project: " & g_Project.FileName & vbCrLf
        lFeedback &= "Config: " & g_Project.ConfigFileName & vbCrLf
#End If

        'plugin info
        'lFeedback &= vbCrLf & "Plugins loaded:" & vbCrLf
        'Dim lLastPlugIn As Integer = g_Plugins.Count() - 1
        'For iPlugin As Integer = 0 To lLastPlugIn
        '    Dim lCurPlugin As MapWindow.Interfaces.IPlugin = g_Plugins.Item(iPlugin)
        '    If Not lCurPlugin Is Nothing Then
        '        With lCurPlugin
        '            lFeedback &= .Name & vbTab & .Version & vbTab & .BuildDate & vbCrLf
        '        End With
        '    End If
        'Next
        'lFeedback &= "___________________________" & vbCrLf

        If lFeedbackForm.ShowFeedback(lName, lEmail, lMessage, lFeedback, True, True, True, g_ProgramDir) Then
            Dim lFeedbackCollection As New NameValueCollection
            lFeedbackCollection.Add("name", Trim(lName))
            lFeedbackCollection.Add("email", Trim(lEmail))
            lFeedbackCollection.Add("message", Trim(lMessage))
            lFeedbackCollection.Add("sysinfo", lFeedback)
            Try
                Dim lClient As New System.Net.WebClient
                lClient.Proxy.Credentials = System.Net.CredentialCache.DefaultCredentials
                lClient.UploadValues("http://hspf.com/cgi-bin/feedback-basins4.cgi", "POST", lFeedbackCollection)
                Logger.Msg("Feedback successfully sent", "Send Feedback")
            Catch e As Exception
                Logger.Msg("Feedback could not be sent", "Send Feedback")
            End Try
        End If
    End Sub

#If GISProvider = "DotSpatial" Then
    Public Shared Sub ShapesSelected(ByVal sender As Object, ByVal aSelectChangedArgs As EventArgs)
        If sender.Layers.SelectedLayer IsNot Nothing Then
            Try
                If CType(sender.Layers.SelectedLayer, IMapFeatureLayer).Selection.Count = 0 Then
                    Exit Sub
                Else
                    For Each imap As IMapFeatureLayer In atcMwGisUtility.GisUtilDS.GetFeatureLayers(Nothing)
                        If imap.Checked Then
                            If imap.DataSet.Name <> sender.Layers.SelectedLayer.DataSet.Name Then
                                imap.Selection.Clear()
                            End If
                        End If
                    Next
                End If
            Catch ex As Exception
                Exit Sub
            End Try
        Else
            Exit Sub
        End If
        If NationalProjectIsOpen() Then
            UpdateSelectedFeatures()
        End If
    End Sub

    Public Shared Sub LayerSelected(ByVal sender As Object, ByVal aLayerSelectedEventArgs As DotSpatial.Symbology.LayerSelectedEventArgs)
        If sender.SelectedLayer Is Nothing Then
            Exit Sub
        End If
        If NationalProjectIsOpen() Then
            UpdateSelectedFeatures()
        End If
    End Sub

    Public Shared Sub LayersAdded(ByVal sender As Object, ByVal aLayerEventArgs As DotSpatial.Symbology.LayerEventArgs)
        Dim lFilename As String = ""
        Try
            lFilename = IO.Path.GetFileName(aLayerEventArgs.Layer.DataSet.Filename).ToLower
        Catch ex As Exception
            lFilename = ""
        End Try
        If Not String.IsNullOrEmpty(lFilename) AndAlso lFilename = "met.shp" Then
            'modDownload.SetMetIcons(MWlay, MWlay) 'ToDo: need to get this to work for BASINS
        End If
        Logger.Progress(0, 0)
    End Sub

#Else
    <CLSCompliant(False)>
    Public Sub LayersAdded(ByVal Layers() As MapWindow.Interfaces.Layer) Implements MapWindow.Interfaces.IPlugin.LayersAdded
        For Each MWlay As MapWindow.Interfaces.Layer In Layers
            'If MWlay.FileName.ToLower.EndsWith("_tgr_a.shp") Or _
            '   MWlay.FileName.ToLower.EndsWith("_tgr_p.shp") Then
            '    SetCensusRenderer(MWlay)
            'End If
            If IO.Path.GetFileName(MWlay.FileName).ToLower = "met.shp" Then
                modDownload.SetMetIcons(MWlay, MWlay.GetObject)
            End If
        Next
        Logger.Progress(0, 0)
    End Sub

    Public Sub LayerSelected(ByVal aHandle As Integer) Implements MapWindow.Interfaces.IPlugin.LayerSelected
        If NationalProjectIsOpen() Then
            UpdateSelectedFeatures()
        End If
    End Sub
#End If

#If GISProvider = "DotSpatial" Then
#Else
#Region "Unused Plug-in Interface Elements"
    Public Sub LayerRemoved(ByVal Handle As Integer) Implements MapWindow.Interfaces.IPlugin.LayerRemoved
    End Sub

    Public Sub LayersCleared() Implements MapWindow.Interfaces.IPlugin.LayersCleared
    End Sub

    <CLSCompliant(False)> _
    Public Sub LegendDoubleClick(ByVal Handle As Integer, ByVal Location As MapWindow.Interfaces.ClickLocation, ByRef Handled As Boolean) Implements MapWindow.Interfaces.IPlugin.LegendDoubleClick
    End Sub

    <CLSCompliant(False)> _
    Public Sub LegendMouseDown(ByVal Handle As Integer, ByVal Button As Integer, ByVal Location As MapWindow.Interfaces.ClickLocation, ByRef Handled As Boolean) Implements MapWindow.Interfaces.IPlugin.LegendMouseDown
    End Sub

    <CLSCompliant(False)> _
    Public Sub LegendMouseUp(ByVal Handle As Integer, ByVal Button As Integer, ByVal Location As MapWindow.Interfaces.ClickLocation, ByRef Handled As Boolean) Implements MapWindow.Interfaces.IPlugin.LegendMouseUp
    End Sub

    Public Sub MapDragFinished(ByVal Bounds As Drawing.Rectangle, ByRef Handled As Boolean) Implements MapWindow.Interfaces.IPlugin.MapDragFinished
    End Sub

    Public Sub MapExtentsChanged() Implements MapWindow.Interfaces.IPlugin.MapExtentsChanged
    End Sub

    Public Sub MapMouseDown(ByVal Button As Integer, ByVal Shift As Integer, ByVal x As Integer, ByVal y As Integer, ByRef Handled As Boolean) Implements MapWindow.Interfaces.IPlugin.MapMouseDown
    End Sub

    Public Sub MapMouseMove(ByVal ScreenX As Integer, ByVal ScreenY As Integer, ByRef Handled As Boolean) Implements MapWindow.Interfaces.IPlugin.MapMouseMove
    End Sub

    Public Sub MapMouseUp(ByVal Button As Integer, ByVal Shift As Integer, ByVal x As Integer, ByVal y As Integer, ByRef Handled As Boolean) Implements MapWindow.Interfaces.IPlugin.MapMouseUp
    End Sub
#End Region

#End If

#If GISProvider = "DotSpatial" Then
    Public Sub Message(ByVal aMessage As String, ByRef aHandled As Boolean)
#Else

    Public Sub Message(ByVal aMessage As String, ByRef aHandled As Boolean) Implements MapWindow.Interfaces.IPlugin.Message
#End If
        If aMessage.StartsWith("WELCOME_SCREEN") Then
            'We always show the welcome screen when requested EXCEPT we skip it when:
            'it is the initial welcome screen AND we have loaded a project or script on the command line.

            'If pWelcomeScreenShow is True, then 
            'it is not the initial welcome screen because it is not the first time we got this message

            'If Not g_MapWin.ApplicationInfo.ShowWelcomeScreen Then 
            'it is not the initial welcome screen because MapWindow does not have given us the message in that case

            'If (g_Project.FileName Is Nothing And Not pCommandLineScript) then 
            'we did not load a project or run a script on the command line
#If GISProvider = "DotSpatial" Then
            If pWelcomeScreenShow _
               OrElse ((g_Project Is Nothing OrElse g_Project.CurrentProjectFile Is Nothing) And Not pCommandLineScript) Then
                Logger.Dbg("Welcome:Show")
                Dim lfrmWelcomeScreen As New frmWelcomeScreen(g_Project, Nothing)
                lfrmWelcomeScreen.ShowDialog()
            Else 'Skip displaying welcome on launch
                Logger.Dbg("Welcome:Skip")
            End If
#Else
            If pWelcomeScreenShow _
               OrElse Not g_MapWin.ApplicationInfo.ShowWelcomeScreen _
               OrElse (g_Project.FileName Is Nothing And Not pCommandLineScript) Then
                Logger.Dbg("Welcome:Show")
                Dim lfrmWelcomeScreen As New frmWelcomeScreen(g_Project, g_MapWin.ApplicationInfo)
                lfrmWelcomeScreen.ShowDialog()
            Else 'Skip displaying welcome on launch
                Logger.Dbg("Welcome:Skip")
            End If
#End If

            pWelcomeScreenShow = True 'Be sure to do it next time (when requested from menu)
        ElseIf aMessage.StartsWith("<success>") Then
            ProcessDownloadResults(aMessage)
        ElseIf aMessage.StartsWith("FileDropEvent") Then 'Try to open dropped file as a data source
            Dim lWasDisplayingMessageBoxes As Boolean = Logger.DisplayMessageBoxes
            Logger.DisplayMessageBoxes = False           'Avoid message box if unable to open as a data source
            Dim lMessage() As String = aMessage.Split("|")
            aHandled = atcDataManager.OpenDataSource(lMessage(3))
            If aHandled Then
                atcDataManager.UserManage(, atcDataManager.DataSources.Count - 1)
            End If
            Logger.DisplayMessageBoxes = lWasDisplayingMessageBoxes
        Else
            'Logger.Dbg("Ignore:" & aMessage)
        End If
    End Sub

#If GISProvider = "DotSpatial" Then
    Public Sub ProjectLoadingDS(sender As Object, evt As SerializingEventArgs)
        Dim lSettingsString As String = ""
        Dim lProjectFile As String = sender.CurrentProjectFile
        If lProjectFile.EndsWith("mwprj") Then
            Dim lxmlDoc As New Xml.XmlDocument()
            lxmlDoc.Load(lProjectFile)
            Dim nodes As Xml.XmlNodeList = lxmlDoc.DocumentElement.SelectNodes("/Mapwin/MapWindow4/Plugins")
            For Each node As Xml.XmlNode In nodes(0).ChildNodes
                If node IsNot Nothing Then
                    lSettingsString = node.Attributes(0).InnerText
                    If lSettingsString.StartsWith("<BASINS><DataManager>") Then
                        Exit For
                    End If
                End If
            Next
        ElseIf lProjectFile.EndsWith("dspx") Then
            lSettingsString = sender.GetCustomSetting("ProjectData", "")
        End If
        If Not String.IsNullOrEmpty(lSettingsString) Then
            ProjectLoading(lProjectFile, lSettingsString)
        End If
    End Sub

    Public Sub ProjectLoading(ByVal aProjectFile As String, ByVal aSettingsString As String)
#Else

    Public Sub ProjectLoading(ByVal aProjectFile As String, ByVal aSettingsString As String) Implements MapWindow.Interfaces.IPlugin.ProjectLoading
#End If
        CloseForms()
        If aProjectFile IsNot Nothing AndAlso aProjectFile.Length > 0 Then
            Try
                ChDriveDir(IO.Path.GetDirectoryName(aProjectFile))
            Catch
                Logger.Dbg("FailedToSetCurdirFrom:" & CurDir() & " to directory of " & aProjectFile)
            End Try
        End If
        If aSettingsString.Length > 0 Then
            Dim lXML As New Xml.XmlDocument
            Try
                lXML.LoadXml(aSettingsString)
                Dim lDataManagerNode As Xml.XmlNode = lXML.SelectSingleNode("/BASINS/DataManager")
                If lDataManagerNode Is Nothing Then
                    atcDataManager.Clear()
                Else
                    atcDataManager.XML = lDataManagerNode.OuterXml
                End If
            Catch e As Exception
                Logger.Dbg("Unable to load project settings string from '" & aProjectFile & "': " & e.Message)
            End Try
        End If
#If GISProvider = "DotSpatial" Then
#Else
        g_MapWin.ApplicationInfo.ApplicationName = g_AppNameLong
#End If
    End Sub

#If GISProvider = "DotSpatial" Then
    Public Sub ProjectSavingDS(sender As Object, evt As SerializingEventArgs)
        Dim manager As SerializationManager = CType(sender, SerializationManager)
        Dim lProjectFile As String = manager.CurrentProjectFile
        Dim lSettingsString As String = ""
        ProjectSaving(lProjectFile, lSettingsString)
        manager.SetCustomSetting("ProjectData", lSettingsString)
    End Sub

    Public Sub ProjectSaving(ByVal aProjectFile As String, ByRef aSettingsString As String)
#Else
    Public Sub ProjectSaving(ByVal aProjectFile As String, ByRef aSettingsString As String) Implements MapWindow.Interfaces.IPlugin.ProjectSaving
#End If
        ChDir(PathNameOnly(aProjectFile))
        aSettingsString = "<BASINS>" & atcDataManager.XML & "</BASINS>"
    End Sub

#If GISProvider = "DotSpatial" Then
#Else
    <CLSCompliant(False)>
    Public Sub ShapesSelected(ByVal aHandle As Integer, ByVal aSelectInfo As MapWindow.Interfaces.SelectInfo) Implements MapWindow.Interfaces.IPlugin.ShapesSelected
        If NationalProjectIsOpen() Then
            UpdateSelectedFeatures()
        End If
    End Sub
#End If

End Class