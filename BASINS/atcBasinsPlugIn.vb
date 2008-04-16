Imports System.Collections.Specialized
Imports System.Windows.Forms.Application
Imports atcUtility
Imports atcData
Imports MapWinUtility

Public Class atcBasinsPlugIn
    Implements MapWindow.Interfaces.IPlugin

    Private pBusy As Integer = 0 'Incremented by setting Busy = True, decremented by setting Busy = False

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
        atcMwGisUtility.GisUtil.MappingObject = g_MapWin
        atcDataManager.MapWindow = g_MapWin

        g_MapWinWindowHandle = aParentHandle
        g_MapWin.ApplicationInfo.WelcomePlugin = "BASINS"
        'Set g_BasinsDir to folder above the Bin folder where the app and plugins live
        g_BasinsDir = PathNameOnly(PathNameOnly(Reflection.Assembly.GetEntryAssembly.Location)) & "\"

        Logger.StartToFile(g_BasinsDir & "cache\log\" _
                         & Format(Now, "yyyy-MM-dd") & "at" & Format(Now, "HH-mm") & "-Basins.log")
        'Logger.MapWin = g_MapWin

        CheckForUpdates(True)

        Try
            Dim lKey As String = g_MapWin.Plugins.GetPluginKey("Timeseries::Statistics")
            'If Not g_MapWin.Plugins.PluginIsLoaded(lKey) Then 
            g_MapWin.Plugins.StartPlugin(lKey)
        Catch e As Exception
            Logger.Dbg("Exception loading Timeseries::Statistics - " & e.Message)
        End Try

        Dim lHelpFilename As String = FindFile("Please locate BASINS 4 help file", g_BasinsDir & "docs\Basins4.0.chm")
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
        atcDataManager.AddMenuIfMissing(BasinsWebPageMenuName, HelpMenuName, BasinsWebPageMenuString, , "mnuOnlineDocs")

        atcDataManager.AddMenuIfMissing(RegisterMenuName, HelpMenuName, RegisterMenuString, , "mnuShortcuts")

        g_Menus.Remove("mnuCheckForUpdates") 'Remove MW update menu so only ours will be present
        g_Menus.Remove("mnuFileBreak5")      'Remove MW separator after mnuCheckForUpdates

        atcDataManager.AddMenuIfMissing(CheckForUpdatesMenuName, HelpMenuName, CheckForUpdatesMenuString, RegisterMenuName)
        atcDataManager.AddMenuIfMissing(SendFeedbackMenuName, HelpMenuName, SendFeedbackMenuString, CheckForUpdatesMenuName)

        Dim mnu As MapWindow.Interfaces.MenuItem
        For Each lDataDir As String In g_BasinsDataDirs
            For Each lProjectDir As String In IO.Directory.GetDirectories(lDataDir)
                Dim DirShortName As String = IO.Path.GetFileName(lProjectDir)
                'TODO: differentiate between projects in different data dirs If g_BasinsDrives.Length > 0 Then DirShortName = DriveLetter & ": " & DirShortName
                mnu = atcDataManager.AddMenuIfMissing(ProjectsMenuName & "_" & DirShortName, _
                                       ProjectsMenuName, DirShortName)
                mnu.Tooltip = lProjectDir
            Next
        Next

        pLoadedDataMenu = True

        atcDataManager.AddMenuIfMissing(atcDataManager.LaunchMenuName, "", atcDataManager.LaunchMenuString, atcDataManager.FileMenuName)
        atcDataManager.AddMenuIfMissing(atcDataManager.LaunchMenuName & "_ArcView3", atcDataManager.LaunchMenuName, "ArcView 3")
        atcDataManager.AddMenuIfMissing(atcDataManager.LaunchMenuName & "_ArcGIS", atcDataManager.LaunchMenuName, "ArcGIS")
        atcDataManager.AddMenuIfMissing(atcDataManager.LaunchMenuName & "_GenScn", atcDataManager.LaunchMenuName, "GenScn")
        atcDataManager.AddMenuIfMissing(atcDataManager.LaunchMenuName & "_WDMUtil", atcDataManager.LaunchMenuName, "WDMUtil")

        atcDataManager.LoadPlugin("Timeseries::Statistics")
        atcDataManager.LoadPlugin("D4EM Data Download::Main")

        'g_ProgressPanel = g_MapWin.UIPanel.CreatePanel("Progress", MapWindow.Interfaces.MapWindowDockStyle.Top)
        'g_ProgressPanel.Visible = False
    End Sub

    Public Sub Terminate() Implements MapWindow.Interfaces.IPlugin.Terminate
        g_MapWin.Menus.Remove(ProjectsMenuName)
        g_MapWin.Menus.Remove(BasinsHelpMenuName)
        g_MapWin.Menus.Remove(BasinsWebPageMenuName)
        g_MapWin.Menus.Remove(RegisterMenuName)
        g_MapWin.Menus.Remove(CheckForUpdatesMenuName)
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
    End Sub

    Public Sub ItemClicked(ByVal aItemName As String, ByRef aHandled As Boolean) Implements MapWindow.Interfaces.IPlugin.ItemClicked
        'A menu item or toolbar button was clicked
        Logger.Dbg(aItemName)
        aHandled = True 'Assume we will handle it
        Select Case aItemName
            Case "mnuNew"            'Override File/New menu
                BASINSNewMenu()
            Case "mnuAboutMapWindow" 'Override Help/About menu
                Dim lAbout As New frmAbout
                lAbout.ShowAbout()
            Case RegisterMenuName
                OpenFile("http://hspf.com/pub/basins4/register.html")
            Case CheckForUpdatesMenuName
                CheckForUpdates(False)
            Case BasinsWebPageMenuName
                OpenFile("http://www.epa.gov/waterscience/basins/index.html")
            Case SendFeedbackMenuName
                SendFeedback()
            Case BasinsHelpMenuName
                ShowHelp("")
            Case atcDataManager.LaunchMenuName & "_ArcView3"
                'create apr if it does not exist, then open it
                Dim lAprFileName As String = "\basins\apr\" & FilenameOnly(g_Project.FileName) & ".apr"
                If Not FileExists(lAprFileName) Then 'build it
                    Dim lExeName As String = _
                       FindFile("Please locate BasinsArchive.exe", _
                       "\BASINS\etc\basinsarchive\BasinsArchive.exe")
                    If Len(lExeName) > 0 Then
                        Dim Exec_Str As String = lExeName & " /build, " & PathNameOnly(g_Project.FileName) & ", " & FilenameOnly(lAprFileName)
                        Shell(Exec_Str, AppWinStyle.NormalFocus, False)
                    End If
                End If
                Try
                    Process.Start(lAprFileName)
                Catch
                    Logger.Msg("No application is associated with APR files - ArcView3 does not appear to be installed.", vbOKOnly, "BASINS/ArcView Problem")
                End Try
            Case atcDataManager.LaunchMenuName & "_ArcGIS"
                Dim buildmxdFilename As String = FindFile("Please Locate build.mxd", "\BASINS\etc\build.mxd")
                If Len(buildmxdFilename) = 0 Then
                    Logger.Msg("Unable to locate Build.mxd", vbOKOnly, "BASINS/ArcGIS Problem")
                Else
                    Try
                        'write directive file here
                        SaveFileString(PathNameOnly(buildmxdFilename) & "\ArcMapInstructions.txt", "Build," & g_Project.FileName)
                        'now start the build mxd
                        Process.Start(buildmxdFilename)
                    Catch
                        Logger.Msg("No application is associated with MXD files - ArcGIS does not appear to be installed.", vbOKOnly, "BASINS/ArcGIS Problem")
                    End Try
                End If
            Case Else
                If aItemName.StartsWith(atcDataManager.LaunchMenuName & "_") Then
                    Dim lExeName As String = ""
                    Select Case aItemName.Substring(atcDataManager.AnalysisMenuName.Length + 1).ToLower
                        Case "genscn" : lExeName = FindFile("Please locate GenScn.exe", "\BASINS\models\HSPF\bin\GenScn.exe")
                        Case "wdmutil" : lExeName = FindFile("Please locate WDMUtil.exe", "\BASINS\models\HSPF\WDMUtil\WDMUtil.exe")
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
                If GetSetting("BASINS4", "Update", "LastCheck", "Never") = lToday Then Exit Sub
                lQuiet = "quiet "
            End If
            SaveSetting("BASINS4", "Update", "LastCheck", lToday)

            Dim lSavePath As String = IO.Path.Combine(g_BasinsDir, "cache")
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

            lPrjFileName = aDataDirName & "\" & FilenameOnly(aDataDirName) & ".mwprj"
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

    Public Property Busy() As Boolean
        Get
            If pBusy > 0 Then Return True Else Return False
        End Get
        Set(ByVal newValue As Boolean)
            If newValue Then
                pBusy += 1
                If pBusy = 1 Then 'We just became busy, so set the main cursor
                    g_MapWin.View.MapCursor = MapWinGIS.tkCursor.crsrWait
                End If
            Else
                pBusy -= 1
                If pBusy = 0 Then 'Not busy any more, set cursor back to default
                    g_MapWin.View.MapCursor = MapWinGIS.tkCursor.crsrMapDefault
                End If
            End If
        End Set
    End Property

    Private Sub SendFeedback()
        Dim lName As String = ""
        Dim lEmail As String = ""
        Dim lMessage As String = ""

        Dim lFeedbackForm As New frmFeedback

        'TODO: format as an html document?
        Dim lFeedback As String = lFeedbackForm.FeedbackGenericSystemInformation()
        Dim lSectionFooter As String = "___________________________" & vbCrLf

        lFeedback &= "Project: " & g_Project.FileName & vbCrLf
        lFeedback &= "Config: " & g_Project.ConfigFileName & vbCrLf

        'plugin info
        lFeedback &= vbCrLf & "Plugins loaded:" & vbCrLf
        Dim lLastPlugIn As Integer = g_Plugins.Count() - 1
        For iPlugin As Integer = 0 To lLastPlugIn
            Dim lCurPlugin As MapWindow.Interfaces.IPlugin = g_Plugins.Item(iPlugin)
            If Not lCurPlugin Is Nothing Then
                With lCurPlugin
                    lFeedback &= .Name & vbTab & .Version & vbTab & .BuildDate & vbCrLf
                End With
            End If
        Next
        lFeedback &= lSectionFooter

        'TODO: add map layers info?
        lFeedback &= vbCrLf & "Information from MapWinUtility.MiscUtils.GetDebugInfo" & vbCrLf & _
                              MapWinUtility.MiscUtils.GetDebugInfo & vbCrLf & vbCrLf

        Dim lSkipFilename As Integer = g_BasinsDir.Length
        lFeedback &= vbCrLf & "Files in " & g_BasinsDir & vbCrLf

        Dim lallFiles As New NameValueCollection
        AddFilesInDir(lallFiles, g_BasinsDir, True)
        'lFeedback &= vbCrLf & "Modified" & vbTab & "Size" & vbTab & "Filename" & vbCrLf
        For Each lFilename As String In lallFiles
            lFeedback &= FileDateTime(lFilename).ToString("yyyy-MM-dd HH:mm:ss") & vbTab & StrPad(Format(FileLen(lFilename), "#,###"), 10) & vbTab & lFilename.Substring(lSkipFilename) & vbCrLf
        Next

        If lFeedbackForm.ShowFeedback(lName, lEmail, lMessage, lFeedback) Then
            Dim lFeedbackCollection As New NameValueCollection
            lFeedbackCollection.Add("name", Trim(lName))
            lFeedbackCollection.Add("email", Trim(lEmail))
            lFeedbackCollection.Add("message", Trim(lMessage))
            lFeedbackCollection.Add("sysinfo", lFeedback)
            Dim lClient As New System.Net.WebClient
            lClient.UploadValues("http://hspf.com/cgi-bin/feedback-basins4.cgi", "POST", lFeedbackCollection)
            Logger.Msg("Feedback successfully sent", "Send Feedback")
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

    Public Sub LayerSelected(ByVal Handle As Integer) Implements MapWindow.Interfaces.IPlugin.LayerSelected
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

    Public Sub Message(ByVal msg As String, ByRef Handled As Boolean) Implements MapWindow.Interfaces.IPlugin.Message
        Dim lErrors As String = ""
        Dim lScriptFileName As String = ""

        If msg.StartsWith("WELCOME_SCREEN") Then
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
                Dim frmWelBsn As New frmWelcomeScreenBasins(g_Project, g_MapWin.ApplicationInfo)

                frmWelBsn.ShowDialog()
            Else 'Skip displaying welcome on launch
                Logger.Dbg("Welcome:Skip")
            End If
            pWelcomeScreenShow = True 'Be sure to do it next time (when requested from menu)
        ElseIf msg.StartsWith("<success>") Then
            ProcessDownloadResults(msg)
        ElseIf msg.StartsWith("FileDropEvent") Then
            Dim lMsg() As String = msg.Split("|")
            Handled = atcDataManager.OpenDataSource(lMsg(3))
            If Handled Then
                atcDataManager.UserManage(, atcDataManager.DataSources.Count - 1)
            End If
        Else
            Logger.Dbg("Ignore:" & msg)
        End If
    End Sub

    Public Sub ProjectLoading(ByVal aProjectFile As String, ByVal aSettingsString As String) Implements MapWindow.Interfaces.IPlugin.ProjectLoading
        If Not aProjectFile Is Nothing AndAlso aProjectFile.Length > 0 Then
            Try
                ChDriveDir(IO.Path.GetDirectoryName(aProjectFile))
            Catch
                Logger.Dbg("FailedToSetCurdirFrom:" & CurDir() & ":to:" & IO.Path.GetDirectoryName(aProjectFile))
            End Try
        End If
        If aSettingsString.Length > 0 Then
            Dim lXML As New Chilkat.Xml
            lXML.LoadXml(aSettingsString)
            atcDataManager.XML = lXML.FindChild("DataManager")
        End If
    End Sub

    Public Sub ProjectSaving(ByVal aProjectFile As String, ByRef aSettingsString As String) Implements MapWindow.Interfaces.IPlugin.ProjectSaving
        Dim saveXML As New Chilkat.Xml
        saveXML.Tag = "BASINS"
        saveXML.AddChildTree(atcDataManager.XML)
        aSettingsString = saveXML.GetXml
    End Sub

    <CLSCompliant(False)> _
    Public Sub ShapesSelected(ByVal Handle As Integer, ByVal SelectInfo As MapWindow.Interfaces.SelectInfo) Implements MapWindow.Interfaces.IPlugin.ShapesSelected
        If NationalProjectIsOpen() Then
            UpdateSelectedFeatures()
        End If
    End Sub

End Class