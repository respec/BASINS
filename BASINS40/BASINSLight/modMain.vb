'********************************************************************************************************
'File Name: modMain.vb
'Description: Entry point for MapWindow
'********************************************************************************************************
'The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License"); 
'you may not use this file except in compliance with the License. You may obtain a copy of the License at 
'http://www.mozilla.org/MPL/ 
'Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF 
'ANY KIND, either express or implied. See the License for the specificlanguage governing rights and 
'limitations under the License. 
'
'The Original Code is MapWindow Open Source. 
'
'The Initial Developer of this version of the Original Code is Daniel P. Ames using portions created by 
'Utah State University and the Idaho National Engineering and Environmental Lab that were released as 
'public domain in March 2004.  
'
'Contributor(s): (Open source contributors should list themselves and their modifications here). 
'1/12/2005 - new entry point for MapWindow (dpa)
'1/31/2005 - minor modifications. (dpa)
'2/2/2005  - commented out redundant call to frmMain.InitializeVars() in LoadMainForm (jlk)
'2/3/2005  - moved display of WelcomeScreen (jlk)
'7/29/2005 - added a exception handler class(Lailin Chen)
'7/29/2005 - added a event handler to the Application object to handle uncaught exceptions(Lailin Chen)
'9/22/2005 - added function to send welcome screen message to a configured plug-in
'12/21/2005 - Added ability to load a layer from the command line. (cdm)
'********************************************************************************************************

Imports System.Threading

Module modMain
    Friend AppInfo As New cAppInfo          'stores info about the current MapWindow configuration 
    Friend frmMain As MapWindowForm
    Friend ProjInfo As New XmlProjectFile   'stores info about the current MapWindow project
    Friend Scripts As New frmScript
    Public g_error As String 'Last error message
    Public g_KillList As New ArrayList()
    Friend g_SyncPluginMenuDefer As Boolean = False

    Public Sub Main()
        ' Creates an instance of the methods that will handle the exception.
        MapWinUtility.Logger.StartToFile(IO.Path.GetDirectoryName(Application.ExecutablePath) & "\logs\BASINSLight-" _
                 & Format(Now, "yyyy-MM-dd") & "at" & Format(Now, "HH-mm") & ".log")

        Dim eh As CustomExceptionHandler = New CustomExceptionHandler
        '7/29/2005 - added a event handler to the Application object to handle uncaught exceptions(Lailin Chen)
        ' Adds the event handler to the event.
        AddHandler Application.ThreadException, AddressOf CustomExceptionHandler.OnThreadException
        'Chris M 11/11/2006 - Please leave thread exception handler at very
        'beginning, so users won't get "Send to Microsoft" ugli crashes when
        'missing DLLs etc

        'Support a /resettodefaults command line for a start menu item that the 4.3 (and up)
        'installer will create, useful if things get corrupt or if upgrading from a prior config version that's acting funny.
        'Note that just deleting them is fine - they'll be rewritten with defaults on the next run.
        If Microsoft.VisualBasic.Command().ToLower().Contains("/resettodefaults") Then
            Try
                If System.IO.File.Exists(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\light.mwcfg") Then
                    System.IO.File.Delete(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\light.mwcfg")
                End If
            Catch
            End Try
            Try
                If System.IO.File.Exists(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\LightDock.config") Then
                    System.IO.File.Delete(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\LightDock.config")
                End If
            Catch
            End Try

            MapWinUtility.Logger.Msg("MapWindow defaults have been restored.", MsgBoxStyle.Information, "Defaults Restored")
            End
        End If

        'Change Culture User Interface to Locale at the Regional Options of the Control Panel
        Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture

        Dim broadcastCmdLine As Boolean = False
        'Run any config command line
        RunConfigCommandLine(broadcastCmdLine)

        'Note that with version 4, no initialization happens yet.
        'There used to be a bunch of stuff in the constructor which made
        'it take several seconds just to get to the splash screen.
        frmMain = New MapWindowForm
        MapWinUtility.Logger.ProgressStatus = frmMain

        LoadMainForm()

        LoadConfig()

        'Determine whether or not to show the welcome screen
        'if the app was started with a project then don't show the welcome screen.
        'If (AppInfo.ShowWelcomeScreen And Not broadcastCmdLine) Then
        '    ShowWelcomeScreen()
        'End If

        'If there was a project file, extract it into the projinfo object.
        'This will also handle loading of shapefiles.
        Dim broadcastCmdLine_2 As Boolean = False
        RunProjectCommandLine(Microsoft.VisualBasic.Command(), broadcastCmdLine_2)

        If broadcastCmdLine Or broadcastCmdLine_2 Then
            frmMain.Plugins.BroadcastMessage("COMMAND_LINE:" & Microsoft.VisualBasic.Command())
        End If

        'All is ready and done... so if a script is waiting to run, do it now.
        If Not Scripts.pFileName = "" Then
            Scripts.RunSavedScript()
        End If

        Try
            Application.EnableVisualStyles()
            Application.SetCompatibleTextRenderingDefault(False)
        Catch
        End Try

        Try
            Application.Run(frmMain)
        Catch e As System.ObjectDisposedException
            'ignore, occurs when application.exit called 
        End Try

        'ANY CODE below this point will be executed when the application terminates.
        For Each s As String In g_KillList
            Try
                System.IO.File.Delete(s)
            Catch e As Exception
                Debug.WriteLine("Failed to delete temp file: " & s & " " & e.Message)
            End Try
        Next
        g_KillList.Clear()

        'Show a survey on the first run if the user has elected to take it.
        Try
            Dim regKey As Microsoft.Win32.RegistryKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("Software\MapWindow", True)
            If regKey.GetValue("ShowSurvey", "False") = "True" Then
                System.Diagnostics.Process.Start("http://www.MapWindow.org/EndFirstRunSurvey.php")
                regKey.DeleteValue("ShowSurvey")
            End If
        Catch e As Exception
            MapWinUtility.Logger.Dbg("DEBUG: " + e.ToString())
        End Try
    End Sub

    Public Sub ShowError(ByVal ex As System.Exception)
        MapWinUtility.Logger.Msg(ex.ToString())
    End Sub

    Public Sub RunProjectCommandLine(ByVal CommandLine As String, ByRef broadcastCmdLine As Boolean)
        'Used to get command line project or config file names.

        If Len(CommandLine) <> 0 Then
            'remove the quotes(") on both sides of the string if they exist
            CommandLine = CommandLine.Replace("""", "")

            'if it is a MapWindow file then open it
            Dim ext As String = System.IO.Path.GetExtension(CommandLine).ToLower()

            If ext = ".mwprj" Then
                'First, however, ensure the current project has been saved if the
                'thing dragged was a project file.
                If Not frmMain.m_HasBeenSaved Or ProjInfo.Modified Then
                    If frmMain.PromptToSaveProject() = MsgBoxResult.Cancel Then
                        Exit Sub
                    End If
                End If

                AppInfo.DefaultDir = System.IO.Path.GetDirectoryName(CommandLine)
                ProjInfo.ProjectFileName = CommandLine
                ProjInfo.LoadProject(CommandLine)
            ElseIf ext = ".cs" Or ext = ".vb" Then
                'It's probably a script - run it. Do it later, though, so just set filename for now.
                AppInfo.DefaultDir = System.IO.Path.GetDirectoryName(CommandLine)
                Scripts.pFileName = CommandLine
            ElseIf ext = ".grd" Then
                'Warn the user.
                MapWinUtility.Logger.Msg("The file you've attempted to open, " + System.IO.Path.GetFileName(CommandLine) + ", could be either a surfer grid or an ESRI grid image." _
                + vbCrLf + vbCrLf + "If the former, please use the GIS Tools plug-in to convert the grid to a compatible format." + vbCrLf + "If the latter, please open the sta.adf file instead.", MsgBoxStyle.Information, "Grid Conversion Required")
            Else
                'Broadcast this cmdline message to all the plugins
                'But can not do it now because the frmMain still haven't initialized
                broadcastCmdLine = True
            End If
        End If
    End Sub

    Private Sub RunConfigCommandLine(ByRef broadcastCmdLine As Boolean)
        'Used to get command line project or config file names.
        Dim S As String = Microsoft.VisualBasic.Command()
        If Len(S) <> 0 Then
            'remove the quotes(") on both sides of the string if they exist
            S = S.Replace("""", "")

            'if it is a MapWindow file then open it
            Dim ext As String = System.IO.Path.GetExtension(S).ToLower()

            If ext = ".mwcfg" Then
                ProjInfo.ConfigFileName = S
            Else
                'Broadcast this cmdline message to all the plugins
                'But can not do it now because the frmMain still haven't initialized
                broadcastCmdLine = True
            End If
        End If
    End Sub

    Private Sub LoadConfig()
        'Load the project file if there is one
        'The project file will indicate which config file to use
        If Len(ProjInfo.ProjectFileName) > 0 Then
            ProjInfo.LoadProject(ProjInfo.ProjectFileName)
        End If

        'Create a configuration file if it does not exist
        If Len(ProjInfo.ConfigFileName) = 0 Then
            ProjInfo.ConfigFileName = App.Path + "\light.mwcfg"
            If System.IO.File.Exists(ProjInfo.ConfigFileName) Then
                ProjInfo.LoadConfig(True)
            Else
                'Set Defaults

                'Firstly, Prepare the default application plugin location:
                AppInfo.ApplicationPluginDir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\ApplicationPlugins"
                frmMain.m_PluginManager.LoadApplicationPlugins(AppInfo.ApplicationPluginDir)
            End If
        ElseIf ProjInfo.ConfigLoaded = False Then
            ProjInfo.LoadConfig(True)
        End If
    End Sub

    Private Sub LoadMainForm()
        '3/16/2005 - dpa - modified so all menus are created dynamically at run-time.
        Dim loadPlugins As Boolean = True
        'Dim SplashScreen As New SplashScreenForm

        frmMain.Show()

        'Put together the help menu item(s).
        'Chris Michaelis 12/22/2005
        'First: If the appinfo help file exists, display the "Contents" help item. This is to be
        'a custom help file defined in the configuration file.
        If (System.IO.File.Exists(AppInfo.HelpFilePath)) Then
            frmMain.m_Menu("mnuContents").Visible = True
        Else
            frmMain.m_Menu("mnuContents").Visible = False
        End If

        'Second: Add the menu item for the online documentation 
        'Checking for web connection can slow down startup, so just go ahead and show the menu item
        frmMain.m_Menu("mnuOnlineDocs").Visible = True

        ' If the offline documentation exists, add a menu item for that
        If (System.IO.File.Exists(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) & "\OfflineDocs\index.html")) Then
            frmMain.m_Menu("mnuOfflineDocs").Visible = True
        Else
            frmMain.m_Menu("mnuOfflineDocs").Visible = False
        End If

        frmMain.Update()

        frmMain.SetModified(False)
        frmMain.m_HasBeenSaved = True

        'make sure all plugins are loaded in the plugin menu
        frmMain.SynchPluginMenu()
    End Sub

    Public Class CustomExceptionHandler
        Public Shared Sub OnThreadException(ByVal sender As Object, ByVal t As Threading.ThreadExceptionEventArgs)
            OnThreadException(t.Exception)
        End Sub

        Public Shared Sub OnThreadException(ByVal e As Exception)
            MapWinUtility.Logger.Msg(e.Message)
            'TODO: send feedback
        End Sub
    End Class
End Module
