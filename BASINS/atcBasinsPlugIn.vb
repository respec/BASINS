Imports System.Collections.Specialized
Imports System.Windows.Forms.Application
Imports atcUtility
Imports atcData
Imports MapWinUtility
Imports atcMwGisUtility

Public Class atcBasinsPlugIn
    Implements MapWindow.Interfaces.IPlugin

#Region "Plug-in Information"
    Public ReadOnly Property Name() As String Implements MapWindow.Interfaces.IPlugin.Name
        'This is the name that appears in the Plug-ins menu
        Get
            Return "BASINS 4"
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
            Return "BASINS extension"
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

    Private pStatusMonitor As MonitorProgressStatus

    <CLSCompliant(False)> _
    Public ReadOnly Property MapWin() As MapWindow.Interfaces.IMapWin
        Get
            Return g_MapWin
        End Get
    End Property

    <CLSCompliant(False)> _
    Public Sub Initialize(ByVal aMapWin As MapWindow.Interfaces.IMapWin, ByVal aParentHandle As Integer) Implements MapWindow.Interfaces.IPlugin.Initialize
        'fired when 
        '   1) user loads plug-in through plug-in dialog or
        '      by checkmarking it in the plug-ins menu.
        '   2) project refererencing plug-in loads

        'This is where buttons or menu items are added.
        g_MapWin = aMapWin
        g_AppNameLong = aMapWin.ApplicationInfo.ApplicationName
        Select Case g_AppNameLong
            Case "USGS Surface Water Analysis"
                g_AppNameRegistry = "USGS-SW"
                g_AppNameShort = "USGS-SW"
                g_URL_Home = "http://water.usgs.gov/software/lists/surface_water/"
                g_URL_Register = "http://hspf.com/pub/USGS-SW/register.html"

            Case Else '"BASINS 4"
                g_AppNameRegistry = "BASINS4"
                g_AppNameShort = "BASINS"
                g_URL_Home = "http://www.epa.gov/waterscience/BASINS/"
                g_URL_Register = "http://hspf.com/pub/basins4/register.html"
        End Select

        ProjectsMenuString = "Open " & g_AppNameShort & " Project"
        RegisterMenuString = "Register as a " & g_AppNameShort & " user"
        ProgramWebPageMenuString = g_AppNameShort & " Web Page"

        GisUtil.MappingObject = g_MapWin
        atcDataManager.MapWindow = g_MapWin

        g_MapWinWindowHandle = aParentHandle
        g_MapWin.ApplicationInfo.WelcomePlugin = "plugin" 'tell the main app to Plugins.BroadcastMessage("WELCOME_SCREEN") instead of showing default MW welcome screen
        'Set g_ProgramDir to folder above the Bin folder where the app and plugins live
        g_ProgramDir = PathNameOnly(PathNameOnly(Reflection.Assembly.GetEntryAssembly.Location)) & g_PathChar

        Logger.StartToFile(g_ProgramDir & "cache\log" & g_PathChar _
                         & Format(Now, "yyyy-MM-dd") & "at" & Format(Now, "HH-mm") & "-" & g_AppNameShort & ".log")
        Logger.Icon = g_MapWin.ApplicationInfo.FormIcon

        'Logger.MsgCustom("Test Message", "Test Title", "Button One", "2")
        'For i As Integer = 1 To 10
        '    If Logger.MsgCustom("Test Message " & i, "Test Title " & i, "Button One", "2", "All") = "All" Then Exit For
        'Next
        'For i As Integer = 1 To 10
        '    If Logger.MsgCustomCheckbox("Test Message " & i, "Test Title " & i, g_AppNameShort, "Test", "Buttons", "Button One", "2", "All") = "All" Then Exit For
        'Next

        If Logger.ProgressStatus Is Nothing OrElse Not (TypeOf (Logger.ProgressStatus) Is MonitorProgressStatus) Then
            'Start running status monitor to give better progress and status indication during long-running processes
            pStatusMonitor = New MonitorProgressStatus
            If pStatusMonitor.StartMonitor(FindFile("Find Status Monitor", "StatusMonitor.exe"), _
                                            g_ProgramDir & "cache\log" & g_PathChar, _
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

        CheckForUpdates(True)

        atcDataManager.LoadPlugin("Timeseries::Statistics")

        Dim lHelpFilename As String = FindFile("", g_ProgramDir & "docs\BASINS4.0.chm")
        If FileExists(lHelpFilename) Then
            ShowHelp(lHelpFilename)
        Else
            Logger.Dbg("Help File Not Found")
        End If
        'BuiltInScript(False)

        FindBasinsDrives()

        g_Menus = g_MapWin.Menus
        g_StatusBar = g_MapWin.StatusBar
        g_Toolbar = g_MapWin.Toolbar
        g_Plugins = g_MapWin.Plugins
        g_Project = g_MapWin.Project

        atcDataManager.AddMenuIfMissing(ProjectsMenuName, atcDataManager.FileMenuName, ProjectsMenuString, "mnuRecentProjects")

        atcDataManager.AddMenuIfMissing(BasinsHelpMenuName, HelpMenuName, BasinsHelpMenuString, , "mnuOnlineDocs")
        atcDataManager.AddMenuIfMissing(ProgramWebPageMenuName, HelpMenuName, ProgramWebPageMenuString, , "mnuOnlineDocs")

        atcDataManager.AddMenuIfMissing(RegisterMenuName, HelpMenuName, RegisterMenuString, , "mnuShortcuts")

        g_Menus.Remove("mnuCheckForUpdates") 'Remove MW update menu so only ours will be present
        g_Menus.Remove("mnuFileBreak5")      'Remove MW separator after mnuCheckForUpdates

        atcDataManager.AddMenuIfMissing(CheckForUpdatesMenuName, HelpMenuName, CheckForUpdatesMenuString, RegisterMenuName)
        atcDataManager.AddMenuIfMissing(ShowStatusMenuName, HelpMenuName, ShowStatusMenuString, CheckForUpdatesMenuName)
        atcDataManager.AddMenuIfMissing(SendFeedbackMenuName, HelpMenuName, SendFeedbackMenuString, ShowStatusMenuName)

        Dim lMenuItem As MapWindow.Interfaces.MenuItem
        For Each lDataDir As String In g_BasinsDataDirs
            For Each lProjectDir As String In IO.Directory.GetDirectories(lDataDir)
                Dim DirShortName As String = IO.Path.GetFileName(lProjectDir)
                'TODO: differentiate between projects in different data dirs If g_BasinsDrives.Length > 0 Then DirShortName = DriveLetter & ": " & DirShortName
                lMenuItem = atcDataManager.AddMenuIfMissing(ProjectsMenuName & "_" & DirShortName, _
                                       ProjectsMenuName, DirShortName)
                lMenuItem.Tooltip = lProjectDir
            Next
        Next

        pLoadedDataMenu = True

        atcDataManager.AddMenuIfMissing(atcDataManager.LaunchMenuName, "", atcDataManager.LaunchMenuString, atcDataManager.FileMenuName)
        atcDataManager.AddMenuIfMissing(atcDataManager.LaunchMenuName & "_ArcView3", atcDataManager.LaunchMenuName, "ArcView 3")
        atcDataManager.AddMenuIfMissing(atcDataManager.LaunchMenuName & "_ArcGIS", atcDataManager.LaunchMenuName, "ArcGIS")
        atcDataManager.AddMenuIfMissing(atcDataManager.LaunchMenuName & "_GenScn", atcDataManager.LaunchMenuName, "GenScn")
        atcDataManager.AddMenuIfMissing(atcDataManager.LaunchMenuName & "_WDMUtil", atcDataManager.LaunchMenuName, "WDMUtil")

        atcDataManager.LoadPlugin("D4EM Data Download::Main")

        Try 'atcDataManager.XML gets loaded when opening a project. This makes sure it gets loaded even without a project
            Dim lAttributesString As String = GetSetting(g_AppNameRegistry, "DataManager", "SelectionAttributes")
            If lAttributesString.Length > 0 Then
                atcDataManager.SelectionAttributes.Clear()
                atcDataManager.SelectionAttributes.AddRange(lAttributesString.Split(vbTab))
            End If
            lAttributesString = GetSetting(g_AppNameRegistry, "DataManager", "DisplayAttributes")
            If lAttributesString.Length > 0 Then
                atcDataManager.DisplayAttributes.Clear()
                atcDataManager.DisplayAttributes.AddRange(lAttributesString.Split(vbTab))
            End If
        Catch
        End Try
        'g_ProgressPanel = g_MapWin.UIPanel.CreatePanel("Progress", MapWindow.Interfaces.MapWindowDockStyle.Top)
        'g_ProgressPanel.Visible = False
    End Sub

    Public Sub Terminate() Implements MapWindow.Interfaces.IPlugin.Terminate
        g_MapWin.Menus.Remove(ProjectsMenuName)
        g_MapWin.Menus.Remove(BasinsHelpMenuName)
        g_MapWin.Menus.Remove(ProgramWebPageMenuName)
        g_MapWin.Menus.Remove(RegisterMenuName)
        g_MapWin.Menus.Remove(CheckForUpdatesMenuName)
        g_MapWin.Menus.Remove(ShowStatusMenuName)
        g_MapWin.Menus.Remove(SendFeedbackMenuName)

        g_MapWin.Menus.Remove(atcDataManager.LaunchMenuName & "_ArcView3")
        g_MapWin.Menus.Remove(atcDataManager.LaunchMenuName & "_ArcGIS")
        g_MapWin.Menus.Remove(atcDataManager.LaunchMenuName & "_GenScn")
        g_MapWin.Menus.Remove(atcDataManager.LaunchMenuName & "_WDMUtil")
        atcDataManager.RemoveMenuIfEmpty(atcDataManager.LaunchMenuName)

        ShowHelp("CLOSE") 'Close any active Help window

        pLoadedDataMenu = False

        g_MapWin.ApplicationInfo.WelcomePlugin = ""
        g_MapWin.ClearCustomWindowTitle()

        CloseForms()

        If pStatusMonitor IsNot Nothing Then
            Logger.ProgressStatus = pStatusMonitor.InnerProgressStatus
            pStatusMonitor.StopMonitor()
        End If

        SaveSetting(g_AppNameRegistry, "DataManager", "SelectionAttributes", String.Join(vbTab, atcDataManager.SelectionAttributes.ToArray()))
        SaveSetting(g_AppNameRegistry, "DataManager", "DisplayAttributes", String.Join(vbTab, atcDataManager.DisplayAttributes.ToArray()))
    End Sub

    Private Sub CloseForms()
        If pBuildFrm IsNot Nothing Then
            Try
                pBuildFrm.Close()
            Catch
            End Try
        End If
    End Sub

    Public Sub ItemClicked(ByVal aItemName As String, ByRef aHandled As Boolean) Implements MapWindow.Interfaces.IPlugin.ItemClicked
        'A menu item or toolbar button was clicked
        Logger.Dbg(aItemName)
        Dim lBasinsFolder As String = My.Computer.Registry.GetValue("HKEY_LOCAL_MACHINE\SOFTWARE\AQUA TERRA Consultants\BASINS", "Base Directory", "C:\Basins")
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
            Case BasinsHelpMenuName
                ShowHelp("")
            Case atcDataManager.LaunchMenuName & "_ArcView3"
                'create apr if it does not exist, then open it
                Dim lAprFileName As String = lBasinsFolder & "\apr" & g_PathChar & IO.Path.GetFileNameWithoutExtension(g_Project.FileName) & ".apr"
                If Not FileExists(lAprFileName) Then 'build it
                    Dim lExeName As String = _
                       FindFile("Please locate BasinsArchive.exe", _
                       lBasinsFolder & "\etc\basinsarchive\BasinsArchive.exe")
                    If Len(lExeName) > 0 Then
                        Dim Exec_Str As String = lExeName & " /build, " & PathNameOnly(g_Project.FileName) & ", " & IO.Path.GetFileNameWithoutExtension(lAprFileName)
                        Shell(Exec_Str, AppWinStyle.NormalFocus, False)
                    End If
                End If
                Try
                    Process.Start(lAprFileName)
                Catch
                    Logger.Msg("No application is associated with APR files - ArcView3 does not appear to be installed.", vbOKOnly, "ArcView Problem")
                End Try
            Case atcDataManager.LaunchMenuName & "_ArcGIS"
                Dim buildmxdFilename As String = FindFile("Please Locate build.mxd", lBasinsFolder & "\etc\build.mxd")
                If Len(buildmxdFilename) = 0 Then
                    Logger.Msg("Unable to locate Build.mxd", vbOKOnly, "ArcGIS Problem")
                Else
                    Try
                        'write directive file here
                        SaveFileString(PathNameOnly(buildmxdFilename) & "\ArcMapInstructions.txt", "Build," & g_Project.FileName)
                        'now start the build mxd
                        Process.Start(buildmxdFilename)
                    Catch
                        Logger.Msg("No application is associated with MXD files - ArcGIS does not appear to be installed.", vbOKOnly, "ArcGIS Problem")
                    End Try
                End If
            Case Else
                If aItemName.StartsWith(atcDataManager.LaunchMenuName & "_") Then
                    Dim lExeName As String = ""
                    Select Case aItemName.Substring(atcDataManager.LaunchMenuName.Length + 1).ToLower
                        Case "genscn" : lExeName = FindFile("Please locate GenScn.exe", lBasinsFolder & "\models\HSPF\bin\GenScn.exe")
                        Case "wdmutil" : lExeName = FindFile("Please locate WDMUtil.exe", lBasinsFolder & "\models\HSPF\WDMUtil\WDMUtil.exe")
                    End Select
                    If FileExists(lExeName) Then
                        Shell("""" & lExeName & """", AppWinStyle.NormalFocus, False)
                        aHandled = True
                    Else
                        Logger.Dbg("Unable to launch " & aItemName, "Launch")
                        aHandled = False
                    End If
                ElseIf aItemName.StartsWith(ProjectsMenuName & "_") Then
                    aHandled = UserOpenProject(g_Menus(aItemName).Text)
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

            Dim lSavePath As String = IO.Path.Combine(g_ProgramDir, "cache")
            Dim lExePath As String = IO.Path.GetDirectoryName(Reflection.Assembly.GetEntryAssembly.Location)
            Dim lUpdateCheckerPath As String = IO.Path.Combine(lExePath, "UpdateCheck.exe")
            If IO.File.Exists(lUpdateCheckerPath) Then
                Shell("""" & lUpdateCheckerPath & """" & " " _
                    & lQuiet _
                    & Process.GetCurrentProcess.Id & " " _
                    & """" & lSavePath & """", AppWinStyle.Hide)
            ElseIf Not aQuiet Then 'If manually checking and UpdateCheck.exe is not found, open update web page
                Logger.Dbg("Did not find update checker at '" & lUpdateCheckerPath & "'")
                OpenFile("http://hspf.com/pub/basins4/updates.html")
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
        Dim lPrjFileName As String

        If Not FileExists(aDataDirName, True, False) Then
            'Look for folder in BASINS dirs
            For Each lDataDir As String In g_BasinsDataDirs
                For Each lProjectDir As String In IO.Directory.GetDirectories(lDataDir)
                    If IO.Path.GetFileName(lProjectDir) = aDataDirName Then
                        aDataDirName = lProjectDir
                        GoTo FoundDir
                    End If
                Next
            Next
        End If

        If FileExists(aDataDirName, True, False) Then
FoundDir:
            If g_Project.Modified Then
                If PromptToSaveProject(g_Project.FileName) = MsgBoxResult.Cancel Then
                    Return False
                End If
            End If

            lPrjFileName = aDataDirName & g_PathChar & IO.Path.GetFileNameWithoutExtension(aDataDirName) & ".mwprj"
            If FileExists(lPrjFileName) Then
                Logger.Dbg("Opening project " & lPrjFileName)
                Return g_Project.Load(lPrjFileName)
            Else
                'TODO: look for other *.mwprj before creating a new one?
                Logger.Dbg("Creating new project " & lPrjFileName)
                ClearLayers()
                RefreshView()
                g_MapWin.PreviewMap.GetPictureFromMap()
                DoEvents()
                AddAllShapesInDir(aDataDirName, aDataDirName)
                g_Project.Save(lPrjFileName)
                g_Project.Modified = False
                Return True
            End If
        End If
        Return False
    End Function

    'TODO: merge with function of same name in MapWindow:frmMain
    Private Function PromptToSaveProject(ByVal aProjectFileName As String) As MsgBoxResult
        Dim lResult As MsgBoxResult
        Dim lTitle As String = g_MapWin.ApplicationInfo.ApplicationName & " Save Changes?"

        If aProjectFileName Is Nothing OrElse FilenameNoExt(aProjectFileName) = "" Then
            lResult = Logger.Msg("Do you want to save the changes to the currently open project?", _
                             MsgBoxStyle.YesNoCancel Or MsgBoxStyle.Exclamation, lTitle)
        Else
            lResult = Logger.Msg("Do you want to save the changes to " & FilenameNoExt(aProjectFileName) & "?", _
                             MsgBoxStyle.YesNoCancel Or MsgBoxStyle.Exclamation, lTitle)
        End If

        Select Case lResult
            Case MsgBoxResult.Yes
                Dim lCdlSave As New SaveFileDialog
                lCdlSave.Filter = "MapWindow Project (*.mwprj)|*.mwprj"
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

        lFeedback &= "Project: " & g_Project.FileName & vbCrLf
        lFeedback &= "Config: " & g_Project.ConfigFileName & vbCrLf

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

    <CLSCompliant(False)> _
    Public Sub LayersAdded(ByVal Layers() As MapWindow.Interfaces.Layer) Implements MapWindow.Interfaces.IPlugin.LayersAdded
        For Each MWlay As MapWindow.Interfaces.Layer In Layers
            If MWlay.FileName.ToLower.EndsWith("_tgr_a.shp") Or _
               MWlay.FileName.ToLower.EndsWith("_tgr_p.shp") Then
                SetCensusRenderer(MWlay)
            End If
        Next
    End Sub

    Public Sub LayerSelected(ByVal aHandle As Integer) Implements MapWindow.Interfaces.IPlugin.LayerSelected
        If NationalProjectIsOpen() Then
            UpdateSelectedFeatures()
        End If
    End Sub

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

    Public Sub Message(ByVal aMessage As String, ByRef aHandled As Boolean) Implements MapWindow.Interfaces.IPlugin.Message
        If aMessage.StartsWith("WELCOME_SCREEN") Then
            'We always show the welcome screen when requested EXCEPT we skip it when:
            'it is the initial welcome screen AND we have loaded a project or script on the command line.

            'If pWelcomeScreenShow is True, then 
            'it is not the initial welcome screen because it is not the first time we got this message

            'If Not g_MapWin.ApplicationInfo.ShowWelcomeScreen Then 
            'it is not the initial welcome screen because MapWindow does not have given us the message in that case

            'If (g_Project.FileName Is Nothing And Not pCommandLineScript) then 
            'we did not load a project or run a script on the command line

            If pWelcomeScreenShow _
               OrElse Not g_MapWin.ApplicationInfo.ShowWelcomeScreen _
               OrElse (g_Project.FileName Is Nothing And Not pCommandLineScript) Then
                Logger.Dbg("Welcome:Show")
                Dim lfrmWelcomeScreen As New frmWelcomeScreen(g_Project, g_MapWin.ApplicationInfo)
                lfrmWelcomeScreen.ShowDialog()
            Else 'Skip displaying welcome on launch
                Logger.Dbg("Welcome:Skip")
            End If
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

    Public Sub ProjectLoading(ByVal aProjectFile As String, ByVal aSettingsString As String) Implements MapWindow.Interfaces.IPlugin.ProjectLoading
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
    End Sub

    Public Sub ProjectSaving(ByVal aProjectFile As String, ByRef aSettingsString As String) Implements MapWindow.Interfaces.IPlugin.ProjectSaving
        aSettingsString = "<BASINS>" & atcDataManager.XML & "</BASINS>"
    End Sub

    <CLSCompliant(False)> _
    Public Sub ShapesSelected(ByVal aHandle As Integer, ByVal aSelectInfo As MapWindow.Interfaces.SelectInfo) Implements MapWindow.Interfaces.IPlugin.ShapesSelected
        If NationalProjectIsOpen() Then
            UpdateSelectedFeatures()
        End If
    End Sub

End Class