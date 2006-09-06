Imports System.Collections.Specialized
Imports System.Windows.Forms.Application
Imports atcUtility
Imports atcData
Imports MapWinUtility

Public Class atcBasinsPlugIn
    Implements MapWindow.Interfaces.IPlugin

    Private pBusy As Integer = 0 'Incremented by setting Busy = True, decremented by setting Busy = False
    Private pBeforeBusyCursor As MapWinGIS.tkCursor

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

    Public ReadOnly Property DataManager() As atcDataManager
        Get
            Return pDataManager
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
        g_MapWinWindowHandle = aParentHandle
        'Set g_BasinsDir to folder above the Bin folder where the app and plugins live
        g_BasinsDir = PathNameOnly(PathNameOnly(Reflection.Assembly.GetEntryAssembly.Location)) & "\"

        Logger.StartToFile(g_BasinsDir & "logs\" _
                         & Format(Now, "yyyy-MM-dd") & "at" & Format(Now, "HH-mm") & "-Basins.log")
        'Logger.MapWin = g_MapWin

        CheckForUpdates(True)

        pDataManager = New atcDataManager(g_MapWin)

        Dim lHelpFilename As String = FindFile("Please locate BASINS 4 help file", g_BasinsDir & "docs\Basins4.0.chm")
        If FileExists(lHelpFilename) Then
            ShowHelp(lHelpFilename)
        Else
            Logger.Dbg("Help File Not Found")
        End If
        'BuiltInScript(False)

        FindBasinsDrives()

        AddMenuIfMissing(NewDataMenuName, FileMenuName, NewDataMenuString, "mnuNew")
        AddMenuIfMissing(OpenDataMenuName, FileMenuName, OpenDataMenuString, "mnuOpen")
        AddMenuIfMissing(DownloadMenuName, FileMenuName, DownloadMenuString, OpenDataMenuName)
        AddMenuIfMissing(ManageDataMenuName, FileMenuName, ManageDataMenuString, DownloadMenuName)
        AddMenuIfMissing(SaveDataMenuName, FileMenuName, SaveDataMenuString, "mnuSaveAs")
        AddMenuIfMissing(ProjectsMenuName, FileMenuName, ProjectsMenuString, "mnuRecentProjects")

        'AddMenuIfMissing(ComputeMenuName, "", ComputeMenuString, FileMenuName)

        Dim mnu As MapWindow.Interfaces.MenuItem

        AddMenuIfMissing("BasinsHelp_Separator1", "mnuHelp", "-")
        mnu = AddMenuIfMissing(BasinsHelpMenuName, "mnuHelp", BasinsHelpMenuString, "")
        mnu = AddMenuIfMissing(BasinsWebPageMenuName, "mnuHelp", BasinsWebPageMenuString, "")
        AddMenuIfMissing("BasinsHelp_Separator2", "mnuHelp", "-")
        mnu = AddMenuIfMissing(RegisterMenuName, "mnuHelp", RegisterMenuString, "")
        mnu = AddMenuIfMissing(CheckForUpdatesMenuName, "mnuHelp", CheckForUpdatesMenuString, "")
        mnu = AddMenuIfMissing(SendFeedbackMenuName, "mnuHelp", SendFeedbackMenuString, "")

        For lDrive As Integer = 0 To g_BasinsDrives.Length - 1
            Dim DriveLetter As String = g_BasinsDrives.Substring(lDrive, 1)
            'Scan folder for project data, and populate menu
            Dim lDataDirs() As String = IO.Directory.GetDirectories( _
                                              DriveLetter & ":\BASINS\data")
            For lDirectory As Integer = 0 To lDataDirs.GetUpperBound(0)
                Dim DirShortName As String = IO.Path.GetFileName(lDataDirs(lDirectory))
                If g_BasinsDrives.Length > 0 Then DirShortName = DriveLetter & ": " & DirShortName
                mnu = AddMenuIfMissing(ProjectsMenuName & "_" & DirShortName, _
                                       ProjectsMenuName, lDataDirs(lDirectory))
                mnu.Tooltip = lDataDirs(lDirectory)
            Next
        Next

        'RefreshDataMenu()
        pLoadedDataMenu = True

        AddMenuIfMissing(ModelsMenuName, "", ModelsMenuString, FileMenuName)

        'HSPF and AQUATOX are handled in ModelSetup plugin

        mnu = AddMenuIfMissing(ModelsMenuName & "_SWAT", ModelsMenuName, "SWAT")
        mnu.Tooltip = "SWAT"
        mnu.Enabled = False
        mnu = AddMenuIfMissing(ModelsMenuName & "_PLOAD", ModelsMenuName, "PLOAD")
        mnu.Tooltip = "PLOAD"
        mnu.Enabled = False
        mnu = AddMenuIfMissing(ModelsMenuName & "_AGWA", ModelsMenuName, "AGWA")
        mnu.Tooltip = "AGWA"
        mnu.Enabled = False
        'AddMenuIfMissing(AnalysisMenuName & "_ModelsSeparator", AnalysisMenuName, "-")

        RefreshAnalysisMenu()
        RefreshComputeMenu()
    End Sub

    Public Sub Terminate() Implements MapWindow.Interfaces.IPlugin.Terminate
        'This event is fired when the user unloads your plug-in either through the plug-in dialog 
        'box, or by un-checkmarking it in the plug-ins menu.  This is where you would remove any
        'buttons from the tool bar tool bar or menu items from the menu that you may have added.
        'If you don't do this, then you will leave dangling menus and buttons that don't do anything.

        ShowHelp("CLOSE") 'Close any active Help window

        g_MapWin.Menus.Remove(DataMenuName)
        pLoadedDataMenu = False

        g_MapWin.Menus.Remove(AnalysisMenuName) 'TODO: don't unload if another plugin is still using it

        g_MapWin.Menus.Remove(ModelsMenuName & "_SWAT")
        g_MapWin.Menus.Remove(ModelsMenuName & "_PLOAD")
        g_MapWin.Menus.Remove(ModelsMenuName & "_AGWA")
        If g_MapWin.Menus.Item(ModelsMenuName).NumSubItems = 0 Then
            g_MapWin.Menus.Remove(ModelsMenuName)
        End If

        g_MapWin.Menus.Remove(NewDataMenuName)
        g_MapWin.Menus.Remove(OpenDataMenuName)
        g_MapWin.Menus.Remove(DownloadMenuName)
        g_MapWin.Menus.Remove(ManageDataMenuName)
        g_MapWin.Menus.Remove(SaveDataMenuName)
        g_MapWin.Menus.Remove(ProjectsMenuName)

        g_MapWin.Menus.Remove(ComputeMenuName)

        g_MapWin.Menus.Remove("BasinsHelp_Separator1")
        g_MapWin.Menus.Remove(BasinsHelpMenuName)
        g_MapWin.Menus.Remove(BasinsWebPageMenuName)
        g_MapWin.Menus.Remove("BasinsHelp_Separator2")
        g_MapWin.Menus.Remove(RegisterMenuName)
        g_MapWin.Menus.Remove(CheckForUpdatesMenuName)
        g_MapWin.Menus.Remove(SendFeedbackMenuName)

        g_MapWin.ApplicationInfo.WelcomePlugin = "WelcomeScreen"
        g_MapWin.ClearCustomWindowTitle()

        'LogStopMonitor()
    End Sub

    Public Sub ItemClicked(ByVal aItemName As String, ByRef aHandled As Boolean) Implements MapWindow.Interfaces.IPlugin.ItemClicked
        'A menu item or toolbar button was clicked
        Logger.Dbg(aItemName)
        aHandled = True 'Assume we will handle it
        Select Case aItemName
            Case "mnuNew"            'Override File/New menu
                LoadNationalProject()
            Case "mnuAboutMapWindow" 'Override Help/About menu
                Dim lAbout As New frmAbout
                lAbout.ShowAbout()
            Case NewDataMenuName
                UserOpenDataFile(False, True)
            Case OpenDataMenuName
                UserOpenDataFile()
            Case DownloadMenuName
                If NationalProjectIsOpen() Then
                    SpecifyAndCreateNewProject()
                Else
                    DownloadNewData(PathNameOnly(g_MapWin.Project.FileName) & "\")
                End If
            Case ManageDataMenuName
                pDataManager.UserManage()
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
            Case AnalysisMenuName & "_ArcView3"
                'create apr if it does not exist, then open it
                Dim lAprFileName As String = "\basins\apr\" & FilenameOnly(g_MapWin.Project.FileName) & ".apr"
                If Not FileExists(lAprFileName) Then 'build it
                    Dim lExeName As String = _
                       FindFile("Please locate BasinsArchive.exe", _
                       "\BASINS\etc\basinsarchive\BasinsArchive.exe")
                    If Len(lExeName) > 0 Then
                        Dim Exec_Str As String = lExeName & " /build, " & PathNameOnly(g_MapWin.Project.FileName) & ", " & FilenameOnly(lAprFileName)
                        Shell(Exec_Str, AppWinStyle.NormalFocus, False)
                    End If
                End If
                Try
                    Process.Start(lAprFileName)
                Catch
                    Logger.Msg("No application is associated with APR files - ArcView3 does not appear to be installed.", vbOKOnly, "BASINS/ArcView Problem")
                End Try
            Case AnalysisMenuName & "_ArcGIS"
                Dim buildmxdFilename As String = FindFile("Please Locate build.mxd", "\BASINS\etc\build.mxd")
                If Len(buildmxdFilename) = 0 Then
                    Logger.Msg("Unable to locate Build.mxd", vbOKOnly, "BASINS/ArcGIS Problem")
                Else
                    Try
                        'write directive file here
                        SaveFileString(PathNameOnly(buildmxdFilename) & "\ArcMapInstructions.txt", "Build," & g_MapWin.Project.FileName)
                        'now start the build mxd
                        Process.Start(buildmxdFilename)
                    Catch
                        Logger.Msg("No application is associated with MXD files - ArcGIS does not appear to be installed.", vbOKOnly, "BASINS/ArcGIS Problem")
                    End Try
                End If
            Case Else
                If aItemName.StartsWith(ComputeMenuName & "_") Then
                    aItemName = aItemName.Replace(" ", "")
                    Dim lNewSource As atcDataSource = Nothing
                    Dim lDataSources As atcCollection = pDataManager.GetPlugins(GetType(atcDataSource))
                    For Each ds As atcDataSource In lDataSources
                        If ds.Category <> "File" Then
                            Dim lCategoryMenuName As String = ComputeMenuName & "_" & ds.Category
                            Dim lOperations As atcDataAttributes = ds.AvailableOperations
                            If Not lOperations Is Nothing AndAlso lOperations.Count > 0 Then
                                For Each lOperation As atcDefinedValue In lOperations
                                    Select Case lOperation.Definition.TypeString
                                        Case "atcTimeseries", "atcDataGroup"
                                            'Operations might have categories to further divide them
                                            If aItemName.Equals((lCategoryMenuName & "_" & lOperation.Definition.Name).Replace(" ", "")) OrElse _
                                               aItemName.Equals((lCategoryMenuName & "_" & lOperation.Definition.Category & "_" & lOperation.Definition.Name).Replace(" ", "")) Then
                                                lNewSource = ds.NewOne
                                                lNewSource.Specification = lOperation.Definition.Name
                                                Exit For
                                            End If
                                    End Select
                                Next
                            Else
                                If aItemName.Equals(lCategoryMenuName & "_" & ds.Description) Then
                                    lNewSource = ds.NewOne
                                    Exit For
                                End If
                            End If
                        End If
                    Next
                    If Not lNewSource Is Nothing Then
                        If pDataManager.OpenDataSource(lNewSource, lNewSource.Specification, Nothing) Then
                            AskUserToDisplayGroup("Display Computed Data", lNewSource.DataSets)
                        End If
                    End If
                ElseIf aItemName.StartsWith(AnalysisMenuName & "_") Then
                    aHandled = LaunchTool(aItemName.Substring(AnalysisMenuName.Length + 1))
                ElseIf aItemName.StartsWith(ModelsMenuName & "_") Then
                    aHandled = LaunchTool(aItemName.Substring(ModelsMenuName.Length + 1))
                ElseIf aItemName.StartsWith(SaveDataMenuName & "_") Then
                    aHandled = UserSaveData(aItemName.Substring(SaveDataMenuName.Length + 1))
                ElseIf aItemName.StartsWith(ProjectsMenuName & "_") Then
                    aHandled = UserOpenProject(g_MapWin.Menus(aItemName).Text)
                Else
                    aHandled = False 'Not our item to handle
                End If
        End Select
    End Sub

    Friend Sub AskUserToDisplayGroup(ByVal aTitle As String, ByVal aGroup As atcDataGroup)
        Dim lPlugins As ICollection = pDataManager.GetPlugins(GetType(atcDataDisplay))
        If lPlugins.Count > 0 Then
            Dim lDisplayNames As New ArrayList
            'For Each lDisp As atcDataDisplay In lPlugins
            '    If lDisp.Name.StartsWith("Analysis::") Then
            '        lDisplayNames.Add(lDisp.Name.Substring(10))
            '    End If
            'Next
            lDisplayNames.Add("Graph")
            lDisplayNames.Add("List")
            lDisplayNames.Add("Data Tree")
            lDisplayNames.Add("Do not display")
            Dim lForm As New frmButtons
            Dim lSelectedDisplay As String = lForm.AskUser(aTitle, lDisplayNames)
            Select Case lSelectedDisplay
                Case "Cancel", "Do not display"
                Case Else
                    pDataManager.ShowDisplay("Analysis::" & lSelectedDisplay, aGroup)
            End Select
        End If
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

    Private Function UserSaveData(ByVal aSpecification As String) As Boolean
        Dim lSaveIn As atcDataSource = Nothing
        Dim lSaveGroup As atcDataGroup = pDataManager.UserSelectData("Select Data to Save")
        If Not lSaveGroup Is Nothing AndAlso lSaveGroup.Count > 0 Then
            For Each lDataSource As atcDataSource In pDataManager.DataSources
                If lDataSource.Specification = aSpecification Then
                    lSaveIn = lDataSource
                    Exit For
                End If
            Next

            If lSaveIn Is Nothing Then
                lSaveIn = UserOpenDataFile(False, True)
            End If

            If Not lSaveIn Is Nothing And lSaveIn.Specification.Length > 0 Then
                For Each lDataSet As atcDataSet In lSaveGroup
                    lSaveIn.AddDataSet(lDataSet, atcData.atcDataSource.EnumExistAction.ExistRenumber)
                Next
                Return lSaveIn.Save(lSaveIn.Specification)
            End If
        End If
        Return False
    End Function

    Private Function UserOpenProject(ByVal aDataDirName As String) As Boolean
        Dim lPrjFileName As String

        If FileExists(aDataDirName, True, False) Then
            lPrjFileName = aDataDirName & "\" & FilenameOnly(aDataDirName) & ".mwprj"
            If FileExists(lPrjFileName) Then
                Logger.Dbg("Opening project " & lPrjFileName)
                Return g_MapWin.Project.Load(lPrjFileName)
            Else
                'TODO: look for other *.mwprj before creating a new one?
                Logger.Dbg("Creating new project " & lPrjFileName)
                g_MapWin.Layers.Clear()
                g_MapWin.Refresh()
                g_MapWin.PreviewMap.GetPictureFromMap()
                DoEvents()
                AddAllShapesInDir(aDataDirName, aDataDirName)
                g_MapWin.Project.Save(lPrjFileName)
                g_MapWin.Project.Modified = False
                Return True
            End If
        End If
        Return False
    End Function

    Private Function UserOpenDataFile(Optional ByVal aNeedToOpen As Boolean = True, _
                                      Optional ByVal aNeedToSave As Boolean = False) As atcDataSource
        Dim lFilesOnly As New ArrayList(1)
        lFilesOnly.Add("File")
        Dim lNewSource As atcDataSource = pDataManager.UserSelectDataSource(lFilesOnly, "Select a File Type", aNeedToOpen, aNeedToSave)
        If Not lNewSource Is Nothing Then 'user did not cancel
            pDataManager.OpenDataSource(lNewSource, lNewSource.Specification, Nothing)
            'If Not lNewSource.DataSets Is Nothing AndAlso lNewSource.DataSets.Count > 0 Then
            'Dim lForm As New frmSelectDisplay
            'lForm.AskUser(pDataManager, lNewSource.DataSets)
            'End If
        End If
        Return lNewSource
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

    Private Function LaunchTool(ByVal aToolName As String) As Boolean ', Optional ByVal aCmdLine As String = "") As Boolean
        Dim exename As String = ""
        Select Case aToolName
            Case "GenScn" : exename = FindFile("Please locate GenScn.exe", "\BASINS\models\HSPF\bin\GenScn.exe")
            Case "WDMUtil" : exename = FindFile("Please locate WDMUtil.exe", "\BASINS\models\HSPF\WDMUtil\WDMUtil.exe")
                'Case "HSPF"
                'If g_MapWin.Plugins.PluginIsLoaded("atcModelSetup_PlugIn") Then 'defer to other plugin
                'Return False
                'End If
                'exename = FindFile("Please locate WinHSPF.exe", "\BASINS\models\HSPF\bin\WinHSPF.exe")
            Case Else
                'If aToolName.StartsWith("RunBuiltInScript") Then
                '  Try
                '    BuiltInScript(True)
                '  Catch e As Exception
                '    Logger.Msg(e.ToString, "Error Running Built-in Script")
                '  End Try
                '  Return True

                'ElseIf aToolName.StartsWith("RunScript") Then
                '  aToolName = aToolName.Substring(9)
                '  exename = StrSplit(aToolName, " ", """")
                '  Dim args() As Object = aToolName.Split(",")
                '  Dim errors As String

                '  If exename.ToLower = "findfile" OrElse Not FileExists(exename) Then
                '    Dim lScriptFileName As String = ScriptFolder() & "\" & exename
                '    If FileExists(lScriptFileName) Then
                '      exename = lScriptFileName
                '    Else
                '      exename = FindFile("Please locate script to run", "", "vb", "VB.net Files (*.vb)|*.vb|All files (*.*)|*.*", True)
                '    End If
                '    If Len(args(0)) = 0 Then args = New Object() {"DataManager", "BasinsPlugIn"}
                '  End If
                '  If FileExists(exename) Then
                '    RunBasinsScript(FileExt(exename), exename, errors, args)
                '    If Not errors Is Nothing Then
                '      Logger.Msg(errors, "Run Script Error")
                '    End If
                '    Return True
                '  Else
                '    Logger.Msg("Unable to find script " & exename, "LaunchTool")
                '    Return False
                '  End If
                'Else 'Search for DisplayPlugin to launch
                If LaunchDisplay(aToolName) Then
                    Return True
                Else
                    Logger.Dbg("LaunchDisplay cannot launch " & aToolName, "Option not yet functional")
                End If
                'End If
        End Select

        If FileExists(exename) Then
            Shell("""" & exename & """", AppWinStyle.NormalFocus, False)
            Return True
        Else
            Logger.Dbg("Unable to launch " & aToolName, "Launch")
            Return False
        End If
    End Function

    Private Sub SendFeedback()
        Dim lSystemInformation As String = ""
        Dim lName As String = ""
        Dim lEmail As String = ""
        Dim lMessage As String = ""

        Dim lFeedbackForm As New frmFeedback
        If lFeedbackForm.ShowFeedback(lName, lEmail, lMessage, lSystemInformation) Then
            Dim lFeedbackCollection As New NameValueCollection
            lFeedbackCollection.Add("name", Trim(lName))
            lFeedbackCollection.Add("email", Trim(lEmail))
            lFeedbackCollection.Add("message", Trim(lMessage))
            lFeedbackCollection.Add("sysinfo", lSystemInformation)
            Dim lClient As New System.Net.WebClient
            lClient.UploadValues("http://hspf.com/cgi-bin/feedback-basins4.cgi", "POST", lFeedbackCollection)
            Logger.Msg("Feedback successfully sent", "Send Feedback")
        End If
    End Sub

    Private Function LaunchDisplay(ByVal aToolName As String, Optional ByVal aCmdLine As String = "") As Boolean
        Dim searchForName As String = aToolName.ToLower
        Dim ColonPos As Integer = searchForName.LastIndexOf(":")
        If ColonPos > 0 Then
            searchForName = searchForName.Substring(ColonPos + 1)
        End If
        searchForName = ReplaceString(searchForName, " ", "")
        Dim DisplayPlugins As ICollection = pDataManager.GetPlugins(GetType(atcDataDisplay))
        For Each lDisp As atcDataDisplay In DisplayPlugins
            Dim foundName As String = lDisp.Name.ToLower
            ColonPos = foundName.LastIndexOf(":")
            If ColonPos > 0 Then
                foundName = foundName.Substring(ColonPos + 1)
            End If
            If ReplaceString(foundName, " ", "") = searchForName Then
                Dim typ As Type = lDisp.GetType()
                Dim asm As Reflection.Assembly = Reflection.Assembly.GetAssembly(typ)
                Dim newDisplay As atcDataDisplay = asm.CreateInstance(typ.FullName)
                newDisplay.Initialize(g_MapWin, g_MapWinWindowHandle)
                newDisplay.Show(pDataManager)
                Return True
            End If
        Next
    End Function

    'Public Function RunBasinsScript(ByVal aLanguage As String, _
    '                                  ByVal aScript As String, _
    '                                  ByRef aErrors As String, _
    '                                  ByVal ParamArray aArgs() As Object) As Object

    '  Logger.Dbg(aLanguage & vbCr & aScript) ', "atcBasinsPlugIn:RunBasinsScript")
    '  If Not aArgs Is Nothing Then 'replace some text arguments with objects
    '    For iArg As Integer = 0 To aArgs.GetUpperBound(0)
    '      If aArgs(iArg).GetType Is GetType(String) Then
    '        Select Case CStr(aArgs(iArg)).ToLower
    '          Case "datamanager" : aArgs(iArg) = pDataManager
    '          Case "basinsplugin" : aArgs(iArg) = Me
    '          Case "mapwin" : aArgs(iArg) = g_MapWin
    '        End Select
    '      End If
    '    Next
    '  End If

    '  If Not FileExists(aScript) Then
    '    Dim lScriptFileName As String = ScriptFolder() & "\" & aScript
    '    If FileExists(lScriptFileName) Then
    '      aScript = lScriptFileName
    '    End If
    '  End If

    '  Return RunScript(aLanguage, MakeScriptName, aScript, aErrors, aArgs)

    'End Function

    'Private Function MakeScriptName() As String
    '  Dim tryName As String
    '  Dim iTry As Integer = 1

    '  Do
    '    tryName = g_MapWin.Plugins.PluginFolder & _
    '              "\Basins\RemoveMe-Script-" & iTry & ".dll"
    '    iTry += 1
    '  Loop While FileExists(tryName)
    '  Return tryName
    'End Function

    'Public Sub CompilePlugin(ByVal aScript As String, _
    '                         ByRef aErrors As String, _
    '                         ByVal refs() As String, _
    '                         ByVal aFileName As String)
    '  CompileScript(aScript, aErrors, refs, aFileName)
    '  If aErrors.Length = 0 Then
    '    g_MapWin.Plugins.AddFromFile(aFileName)
    '  End If
    'End Sub

    Public Sub LayerRemoved(ByVal Handle As Integer) Implements MapWindow.Interfaces.IPlugin.LayerRemoved
        'This event fires when the user removes a layer from MapWindow.  This is useful to know if your
        'plug-in depends on a particular layer being present. 
    End Sub

    <CLSCompliant(False)> _
    Public Sub LayersAdded(ByVal Layers() As MapWindow.Interfaces.Layer) Implements MapWindow.Interfaces.IPlugin.LayersAdded
        'This event fires when the user adds a layer to MapWindow.  This is useful to know if your
        'plug-in depends on a particular layer being present. Also, if you keep an internal list of 
        'available layers, for example you may be keeping a list of all "point" shapefiles, then you
        'would use this event to know when layers have been added or removed.

        For Each MWlay As MapWindow.Interfaces.Layer In Layers
            If MWlay.FileName.ToLower.EndsWith("_tgr_a.shp") Or _
               MWlay.FileName.ToLower.EndsWith("_tgr_p.shp") Then
                SetCensusRenderer(MWlay)
            End If
        Next
    End Sub

    Public Sub LayersCleared() Implements MapWindow.Interfaces.IPlugin.LayersCleared
        'This event fires when the user clears all of the layers from MapWindow.  As with LayersAdded 
        'and LayersRemoved, this is useful to know if your plug-in depends on a particular layer being 
        'present or if you are maintaining your own list of layers.
    End Sub

    Public Sub LayerSelected(ByVal Handle As Integer) Implements MapWindow.Interfaces.IPlugin.LayerSelected
        'This event fires when a user selects a layer in the legend. 
        If NationalProjectIsOpen() Then
            UpdateSelectedFeatures()
        End If
    End Sub

    <CLSCompliant(False)> _
    Public Sub LegendDoubleClick(ByVal Handle As Integer, ByVal Location As MapWindow.Interfaces.ClickLocation, ByRef Handled As Boolean) Implements MapWindow.Interfaces.IPlugin.LegendDoubleClick
        'This event fires when a user double-clicks a layer in the legend.
    End Sub

    <CLSCompliant(False)> _
    Public Sub LegendMouseDown(ByVal Handle As Integer, ByVal Button As Integer, ByVal Location As MapWindow.Interfaces.ClickLocation, ByRef Handled As Boolean) Implements MapWindow.Interfaces.IPlugin.LegendMouseDown
        'This event fires when a user holds a mouse button down in the legend.
    End Sub

    <CLSCompliant(False)> _
    Public Sub LegendMouseUp(ByVal Handle As Integer, ByVal Button As Integer, ByVal Location As MapWindow.Interfaces.ClickLocation, ByRef Handled As Boolean) Implements MapWindow.Interfaces.IPlugin.LegendMouseUp
        'This event fires when a user releases a mouse button in the legend.
    End Sub

    Public Sub MapDragFinished(ByVal Bounds As Drawing.Rectangle, ByRef Handled As Boolean) Implements MapWindow.Interfaces.IPlugin.MapDragFinished
        'If a user drags (ie draws a box) with the mouse on the map, this event fires at completion of the drag
        'and returns a drawing.rectangle that has the bounds of the box that was "drawn"
    End Sub

    Public Sub MapExtentsChanged() Implements MapWindow.Interfaces.IPlugin.MapExtentsChanged
        'This event fires any time there is a zoom or pan that changes the extents of the map.
    End Sub

    Public Sub MapMouseDown(ByVal Button As Integer, ByVal Shift As Integer, ByVal x As Integer, ByVal y As Integer, ByRef Handled As Boolean) Implements MapWindow.Interfaces.IPlugin.MapMouseDown
        'This event fires when the user holds a mouse button down on the map. Note that x and y are returned
        'as screen coordinates (in pixels), not map coordinates.  So if you really need the map coordinates
        'then you need to use g_MapWin.View.PixelToProj()
    End Sub

    Public Sub MapMouseMove(ByVal ScreenX As Integer, ByVal ScreenY As Integer, ByRef Handled As Boolean) Implements MapWindow.Interfaces.IPlugin.MapMouseMove
        'This event fires when the user moves the mouse over the map. Note that x and y are returned
        'as screen coordinates (in pixels), not map coordinates.  So if you really need the map coordinates
        'then you need to use g_MapWin.View.PixelToProj()
        'Dim ProjX As Double, ProjY As Double
        'g_MapWin.View.PixelToProj(ScreenX, ScreenY, ProjX, ProjY)
        'g_MapWin.StatusBar(2).Text = "X = " & ProjX & " Y = " & ProjY
    End Sub

    Public Sub MapMouseUp(ByVal Button As Integer, ByVal Shift As Integer, ByVal x As Integer, ByVal y As Integer, ByRef Handled As Boolean) Implements MapWindow.Interfaces.IPlugin.MapMouseUp
        'This event fires when the user releases a mouse button down on the map. Note that x and y are returned
        'as screen coordinates (in pixels), not map coordinates.  So if you really need the map coordinates
        'then you need to use g_MapWin.View.PixelToProj()
    End Sub

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

            'If (g_MapWin.Project.FileName Is Nothing And Not pCommandLineScript) then 
            'we did not load a project or run a script on the command line

            If pWelcomeScreenShow _
               OrElse Not g_MapWin.ApplicationInfo.ShowWelcomeScreen _
               OrElse (g_MapWin.Project.FileName Is Nothing And Not pCommandLineScript) Then
                Logger.Dbg("Welcome:Show")
                Dim frmWelBsn As New frmWelcomeScreenBasins(g_MapWin.Project, g_MapWin.ApplicationInfo)
                frmWelBsn.ShowDialog()
            Else 'Skip displaying welcome on launch
                Logger.Dbg("Welcome:Skip")
            End If
            pWelcomeScreenShow = True 'Be sure to do it next time (when requested from menu)
        ElseIf msg.StartsWith("atcDataPlugin") Then
            If msg.IndexOf("Analysis::") > 0 Then
                Dim lUnloading As String = ""
                Logger.Dbg(msg)
                If msg.StartsWith("atcDataPlugin unloading") Then
                    lUnloading = msg.Substring(24)
                    g_MapWin.Menus.Remove(AnalysisMenuName)
                End If
                RefreshAnalysisMenu(lUnloading)
            End If
            RefreshComputeMenu()
        Else
            Logger.Dbg("Ignore:" & msg)
        End If
    End Sub

    Public Sub ProjectLoading(ByVal ProjectFile As String, ByVal SettingsString As String) Implements MapWindow.Interfaces.IPlugin.ProjectLoading
        'When the user opens a project in MapWindow, this event fires.  The ProjectFile is the file name of the
        'project that the user opened (including its path in case that is important for this this plug-in to know).
        'The SettingsString variable contains any string of data that is connected to this plug-in but is stored 
        'on a project level. For example, a plug-in that shows streamflow data might allow the user to set a 
        'separate database for each project (i.e. one database for the upper Missouri River Basin, a different 
        'one for the Lower Colorado Basin.) In this case, the plug-in would store the database name in the 
        'SettingsString of the project. 
        Dim lXML As New Chilkat.Xml
        lXML.LoadXml(SettingsString)
        pDataManager.XML = lXML.FindChild("DataManager")
    End Sub

    Public Sub ProjectSaving(ByVal ProjectFile As String, ByRef SettingsString As String) Implements MapWindow.Interfaces.IPlugin.ProjectSaving
        'When the user saves a project in MapWindow, this event fires.  The ProjectFile is the file name of the
        'project that the user is saving (including its path in case that is important for this this plug-in to know).
        'The SettingsString variable contains any string of data that is connected to this plug-in but is stored 
        'on a project level. For example, a plug-in that shows streamflow data might allow the user to set a 
        'separate database for each project (i.e. one database for the upper Missouri River Basin, a different 
        'one for the Lower Colorado Basin.) In this case, the plug-in would store the database name in the 
        'SettingsString of the project. 
        Dim saveXML As New Chilkat.Xml
        saveXML.Tag = "BASINS"
        saveXML.AddChildTree(pDataManager.XML)
        SettingsString = saveXML.GetXml
    End Sub

    <CLSCompliant(False)> _
    Public Sub ShapesSelected(ByVal Handle As Integer, ByVal SelectInfo As MapWindow.Interfaces.SelectInfo) Implements MapWindow.Interfaces.IPlugin.ShapesSelected
        'This event fires when the user selects one or more shapes using the select tool in MapWindow. Handle is the 
        'Layer handle for the shapefile on which shapes were selected. SelectInfo holds information abou the 
        'shapes that were selected. 
        If NationalProjectIsOpen() Then
            UpdateSelectedFeatures()
        End If
    End Sub

End Class