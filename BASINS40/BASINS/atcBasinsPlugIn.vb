Imports System.Collections.Specialized
Imports System.Windows.Forms.Application
Imports atcUtility
Imports atcData
Imports MapWinUtility

Public Class atcBasinsPlugIn
    Implements MapWindow.Interfaces.IPlugin

    Private pBusy As Integer = 0 'Incremented by setting Busy = True, decremented by setting Busy = False

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
        g_MapWin.ApplicationInfo.WelcomePlugin = "BASINS"
        'Set g_BasinsDir to folder above the Bin folder where the app and plugins live
        g_BasinsDir = PathNameOnly(PathNameOnly(Reflection.Assembly.GetEntryAssembly.Location)) & "\"

        Logger.StartToFile(g_BasinsDir & "cache\log\" _
                         & Format(Now, "yyyy-MM-dd") & "at" & Format(Now, "HH-mm") & "-Basins.log")
        'Logger.MapWin = g_MapWin

        CheckForUpdates(True)

        atcDataManager.MapWindow = g_MapWin
        AddHandler atcDataManager.OpenedData, AddressOf OpenedData

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

        AddMenuIfMissing(NewDataMenuName, FileMenuName, NewDataMenuString, "mnuNew")
        AddMenuIfMissing(OpenDataMenuName, FileMenuName, OpenDataMenuString, "mnuOpen")
        AddMenuIfMissing(DownloadMenuName, FileMenuName, DownloadMenuString, OpenDataMenuName)
        AddMenuIfMissing(ManageDataMenuName, FileMenuName, ManageDataMenuString, DownloadMenuName)
        AddMenuIfMissing(SaveDataMenuName, FileMenuName, SaveDataMenuString, "mnuSaveAs")
        AddMenuIfMissing(ProjectsMenuName, FileMenuName, ProjectsMenuString, "mnuRecentProjects")

        AddMenuIfMissing(BasinsHelpMenuName, HelpMenuName, BasinsHelpMenuString, , "mnuOnlineDocs")
        AddMenuIfMissing(BasinsWebPageMenuName, HelpMenuName, BasinsWebPageMenuString, , "mnuOnlineDocs")

        AddMenuIfMissing(RegisterMenuName, HelpMenuName, RegisterMenuString, , "mnuShortcuts")

        g_Menus.Remove("mnuCheckForUpdates") 'Remove MW update menu so only ours will be present
        g_Menus.Remove("mnuFileBreak5")      'Remove MW separator after mnuCheckForUpdates

        AddMenuIfMissing(CheckForUpdatesMenuName, HelpMenuName, CheckForUpdatesMenuString, RegisterMenuName)
        AddMenuIfMissing(SendFeedbackMenuName, HelpMenuName, SendFeedbackMenuString, CheckForUpdatesMenuName)

        Dim mnu As MapWindow.Interfaces.MenuItem
        For Each lDataDir As String In g_BasinsDataDirs
            For Each lProjectDir As String In IO.Directory.GetDirectories(lDataDir)
                Dim DirShortName As String = IO.Path.GetFileName(lProjectDir)
                'TODO: differentiate between projects in different data dirs If g_BasinsDrives.Length > 0 Then DirShortName = DriveLetter & ": " & DirShortName
                mnu = AddMenuIfMissing(ProjectsMenuName & "_" & DirShortName, _
                                       ProjectsMenuName, DirShortName)
                mnu.Tooltip = lProjectDir
            Next
        Next

        pLoadedDataMenu = True

        AddMenuIfMissing(ModelsMenuName, "", ModelsMenuString, FileMenuName)

        'HSPF and AQUATOX are handled in ModelSetup plugin

        mnu = AddMenuIfMissing(ModelsMenuName & "_SWAT", ModelsMenuName, "SWAT")
        mnu.Tooltip = "SWAT"
        mnu.Enabled = False
        mnu.Enabled = False
        mnu = AddMenuIfMissing(ModelsMenuName & "_AGWA", ModelsMenuName, "AGWA")
        mnu.Tooltip = "AGWA"
        mnu.Enabled = False
        'AddMenuIfMissing(AnalysisMenuName & "_ModelsSeparator", AnalysisMenuName, "-")

        RefreshAnalysisMenu()
        RefreshComputeMenu()
    End Sub

    Public Sub Terminate() Implements MapWindow.Interfaces.IPlugin.Terminate
        RemoveHandler atcDataManager.OpenedData, AddressOf OpenedData

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
            Case NewDataMenuName
                UserOpenDataFile(False, True)
            Case OpenDataMenuName
                UserOpenDataFile()
            Case DownloadMenuName
                If NationalProjectIsOpen() Then
                    SpecifyAndCreateNewProject()
                Else
                    DownloadNewData(PathNameOnly(g_Project.FileName) & "\")
                End If
            Case ManageDataMenuName
                atcDataManager.UserManage()
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
            Case AnalysisMenuName & "_ArcGIS"
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
                If aItemName.StartsWith(ComputeMenuName & "_") Then
                    aItemName = aItemName.Replace(" ", "")
                    Dim lNewSource As atcDataSource = Nothing
                    Dim lDataSources As atcCollection = atcDataManager.GetPlugins(GetType(atcDataSource))
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
                        If atcDataManager.OpenDataSource(lNewSource, lNewSource.Specification, Nothing) Then
                            If lNewSource.DataSets.Count > 0 Then
                                Dim lTitle As String = lNewSource.ToString
                                atcDataManager.UserSelectDisplay(lTitle, lNewSource.DataSets)
                            End If
                        End If
                    End If
                ElseIf aItemName.StartsWith(AnalysisMenuName & "_") Then
                    aHandled = LaunchTool(aItemName.Substring(AnalysisMenuName.Length + 1))
                ElseIf aItemName.StartsWith(ModelsMenuName & "_") Then
                    aHandled = LaunchTool(aItemName.Substring(ModelsMenuName.Length + 1))
                ElseIf aItemName.StartsWith(SaveDataMenuName & "_") Then
                    aHandled = UserSaveData(aItemName.Substring(SaveDataMenuName.Length + 1))
                    'TODO: add case where not save data destinations are available, ask user for destination?
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

    Private Function UserSaveData(ByVal aSpecification As String) As Boolean
        Dim lSaveIn As atcDataSource = Nothing
        Dim lSaveGroup As atcDataGroup = atcDataManager.UserSelectData("Select Data to Save")
        If Not lSaveGroup Is Nothing AndAlso lSaveGroup.Count > 0 Then
            For Each lDataSource As atcDataSource In atcDataManager.DataSources
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

    Private Function UserOpenDataFile(Optional ByVal aNeedToOpen As Boolean = True, _
                                      Optional ByVal aNeedToSave As Boolean = False) As atcDataSource
        Dim lFilesOnly As New ArrayList(1)
        lFilesOnly.Add("File")
        Dim lNewSource As atcDataSource = atcDataManager.UserSelectDataSource(lFilesOnly, "Select a File Type", aNeedToOpen, aNeedToSave)
        If Not lNewSource Is Nothing Then 'user did not cancel
            If Not atcDataManager.OpenDataSource(lNewSource, lNewSource.Specification, Nothing) Then
                If Logger.LastDbgText.Length > 0 Then
                    Logger.Msg(Logger.LastDbgText, "Data Open Problem")
                End If
            End If
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
                'If g_Plugins.PluginIsLoaded("atcModelSetup_PlugIn") Then 'defer to other plugin
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

    Private Function LaunchDisplay(ByVal aToolName As String, Optional ByVal aCmdLine As String = "") As Boolean
        Dim searchForName As String = aToolName.ToLower
        Dim ColonPos As Integer = searchForName.LastIndexOf(":")
        If ColonPos > 0 Then
            searchForName = searchForName.Substring(ColonPos + 1)
        End If
        searchForName = ReplaceString(searchForName, " ", "")
        Dim DisplayPlugins As ICollection = atcDataManager.GetPlugins(GetType(atcDataDisplay))
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
                newDisplay.Show()
                Return True
            End If
        Next
    End Function

    Public Sub LayerRemoved(ByVal Handle As Integer) Implements MapWindow.Interfaces.IPlugin.LayerRemoved
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
    End Sub

    Public Sub LayerSelected(ByVal Handle As Integer) Implements MapWindow.Interfaces.IPlugin.LayerSelected
        'This event fires when a user selects a layer in the legend. 
        If NationalProjectIsOpen() Then
            UpdateSelectedFeatures()
        End If
    End Sub

    <CLSCompliant(False)> _
    Public Sub LegendDoubleClick(ByVal Handle As Integer, ByVal Location As MapWindow.Interfaces.ClickLocation, ByRef Handled As Boolean) Implements MapWindow.Interfaces.IPlugin.LegendDoubleClick
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
        'g_StatusBar(2).Text = "X = " & ProjX & " Y = " & ProjY
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
        ElseIf msg.StartsWith("atcDataPlugin") Then
            If msg.IndexOf("Analysis::") > 0 Then
                Dim lUnloading As String = ""
                Logger.Dbg(msg)
                If msg.StartsWith("atcDataPlugin unloading") Then
                    lUnloading = msg.Substring(24)
                    g_Menus.Remove(AnalysisMenuName)
                End If
                RefreshAnalysisMenu(lUnloading)
            End If
            RefreshComputeMenu()
        ElseIf msg.StartsWith("<success>") Then
            ProcessDownloadResults(msg)
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
        atcDataManager.XML = lXML.FindChild("DataManager")
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
        saveXML.AddChildTree(atcDataManager.XML)
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