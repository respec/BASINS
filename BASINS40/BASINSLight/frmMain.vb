Imports MapWinUtility
Imports System.Xml
Imports System.IO
Imports System.Text

Friend Class MapWindowForm
    Implements MapWinUtility.IProgressStatus
    Implements Interfaces.IMapWin

    Private Const pAppName = "DFLOWmain"
    Private Const RecentProjectPrefix As String = "mnuRecentProjects_"

    Private pFormLoaded As Boolean = False
    Friend m_HasBeenSaved As Boolean
    Friend m_Menu As MapWindow.Menus
    Friend m_Toolbar As MapWindow.Toolbar
    Friend m_ComboBoxes As New Hashtable        'Stores combo boxes that were dynamically added as controls to the toolbar area.
    Friend m_PluginManager As Plugins_IPlugin
    Friend m_Project As MapWindow.Project
    Friend CustomWindowTitle As String = ""
    Friend Title_ShowFullProjectPath As Boolean = False
    Private resources As System.Resources.ResourceManager = _
    New System.Resources.ResourceManager("MapWindow.GlobalResource", System.Reflection.Assembly.GetExecutingAssembly())


    Private Sub btnExecute_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Cursor = Cursors.WaitCursor
        Logger.Dbg("btnExecuteClicked")

        Try
            SaveWindowState()
            lblStatus.Text = "Downloading"

            'Logger.Dbg("DataManagerStatus:ExtensionCount " & pDataManager.AvailableExtensions.Count)
            'Dim lResult As String = pDataManager.Execute(txtRawQuery.Text)
            Logger.Dbg("Execute Complete")
            'Logger.Msg(lResult)
            'DisplayResults(txtRawQuery.Text)
            lblStatus.Text = "Finished Download"
        Catch ex As Exception
            lblStatus.Text = "Error: " & ex.Message
        End Try

        Cursor = Cursors.Default
    End Sub

    Private Sub frmMain_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        InitializeVars()
        LoadWindowState()
        pFormLoaded = True
    End Sub

    Private Sub MapWindowForm_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        ProjInfo.SaveConfig()

        If Not m_HasBeenSaved Or ProjInfo.Modified Then
            If PromptToSaveProject() = MsgBoxResult.Cancel Then
                e.Cancel = True
                Me.DialogResult = DialogResult.Cancel
                Exit Sub
            End If
        End If

        g_SyncPluginMenuDefer = True
        m_PluginManager.UnloadAll() ' cleans up plugins on shutdown
        m_PluginManager.UnloadApplicationPlugins()
        Me.DialogResult = DialogResult.OK
        'm_PluginManager.WriteSettingsFile()
    End Sub

    'Private Function GetDataManager() As D4EMDataManager.DataManager
    '    If (pDataManager Is Nothing) Then
    '        MapWindow.g_PluginManager = New MapWindow.Plugins_IPlugin
    '        MapWindow.g_PluginManager.LoadPlugins()
    '        pDataManager = New D4EMDataManager.DataManager(MapWindow.g_PluginManager.LoadedPlugins)
    '    End If
    '    Return pDataManager
    'End Function

    Private Sub LoadWindowState()
        'txtCache.Text = GetSetting(pAppName, "Defaults", "CacheFolder", txtCache.Text)
    End Sub

    Private Sub SaveWindowState()
        'SaveSetting(pAppName, "Defaults", "CacheFolder", txtCache.Text)
    End Sub

    'Public Sub Busy(ByVal aBusy As Boolean) Implements MapWinUtility.IProgressStatus.Busy
    'End Sub

    Public Sub Progress(ByVal CurrentPosition As Integer, ByVal LastPosition As Integer) Implements MapWinUtility.IProgressStatus.Progress
        If CurrentPosition < LastPosition Then
            lblProgress.Text = CurrentPosition & " of " & LastPosition
        Else
            lblProgress.Text = ""
        End If
    End Sub

    Public Sub Status(ByVal StatusMessage As String) Implements MapWinUtility.IProgressStatus.Status
        lblStatus.Text = StatusMessage
        lblStatus.Refresh()
        Windows.Forms.Application.DoEvents()
    End Sub

    Public Sub InitializeVars()
        'This used to be done as part of the frmMain "New" stuff.  However, this
        'helped MapWindow be very slow to load.  Instead, it is now (in version 4) 
        'in a separate function that can be called once, and partly obscurred by a 
        'splash screen.

        m_Project = New Project
        m_Menu = New Menus
        m_Toolbar = New ToolBar
        m_PluginManager = New Plugins_IPlugin
        m_HasBeenSaved = True
        'm_StatusBar = New MapWindow.StatusBar
        'm_Reports = New MapWindow.Reports
        'm_Labels = New LabelClass

        frmMain.SetUpMenus()    'Creates all of the menus

        ' Scan for plugins
        m_PluginManager.LoadApplicationPlugins(AppInfo.ApplicationPluginDir)
        m_PluginManager.LoadPlugins()

        'save all menus and toolbars
        XmlProjectFile.SaveMainToolbarButtons()

        Me.KeyPreview = True

    End Sub

    Public Sub tlbMain_ButtonClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ToolStripItemClickedEventArgs) Handles tlbMain.ItemClicked
        Dim Btn As Windows.Forms.ToolStripItem = e.ClickedItem
        Dim BtnName As String = CType(Btn.Name, String)
        If BtnName.Trim() = "" And TypeOf (Btn.Tag) Is String Then BtnName = CStr(Btn.Tag)

        Dim Handled As Boolean
        Handled = m_PluginManager.ItemClicked(BtnName)
        If Handled = False Then 'the plugin didn't override
            Select Case BtnName
                Case "tbbPrint" : DoPrint()
                Case "tbbSave" : DoSave()
                Case "tbbNew" : DoNew()
                Case "tbbOpen" : DoOpen()
            End Select
        End If
    End Sub

    Public Sub CustomCombo_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'Fires when user clicks a custom combo box on the main toolbar or floating toolbar.
        Dim cbClicked As Windows.Forms.ToolStripItem = CType(sender, Windows.Forms.ToolStripItem)
        m_PluginManager.ItemClicked(cbClicked.Name)
    End Sub

    Public Sub SetUpMenus()
        Dim Nil As Object = Nothing
        'Set up the File menu
        m_Menu.AddMenu("mnuFile", Nil, resources.GetString("mnuFile.Text"))
        m_Menu.AddMenu("mnuNew", "mnuFile", Nil, resources.GetString("mnuNew.Text"))
        m_Menu.AddMenu("mnuOpen", "mnuFile", Nil, resources.GetString("mnuOpen.Text"))
        m_Menu.AddMenu("mnuFileBreak0", "mnuFile", Nil, "-")
        m_Menu.AddMenu("mnuSave", "mnuFile", Nil, resources.GetString("mnuSave.Text"))
        m_Menu.AddMenu("mnuSaveAs", "mnuFile", Nil, resources.GetString("mnuSaveAs.Text"))
        m_Menu.AddMenu("mnuFileBreak2", "mnuFile", Nil, "-")
        'm_Menu.AddMenu("mnuPrint", "mnuFile", Nil, resources.GetString("mnuPrint.Text"))
        'm_Menu.AddMenu("mnuFileBreak3", "mnuFile", Nil, "-")
        'm_Menu.AddMenu("mnuProjectSettings", "mnuFile", Nil, resources.GetString("mnuProjectSettings.Text"))
        m_Menu.AddMenu("mnuRecentProjects", "mnuFile", Nil, resources.GetString("mnuRecentProjects.Text"))
        m_Menu.AddMenu("mnuFileBreak4", "mnuFile", Nil, "-")
        'm_Menu.AddMenu("mnuCheckForUpdates", "mnuFile", Nil, resources.GetString("mnuCheckUpdates.Text"))
        m_Menu.AddMenu("mnuFileBreak5", "mnuFile", Nil, "-")
        m_Menu.AddMenu("mnuClose", "mnuFile", Nil, resources.GetString("mnuClose.Text"))
        m_Menu.AddMenu("mnuExit", "mnuFile", Nil, resources.GetString("mnuExit.Text"))

        'Set up the Edit menu
        'm_Menu.AddMenu("mnuEdit", Nil, resources.GetString("mnuEdit.Text"))


        'Set up the Plug-ins menu
        m_Menu.AddMenu("mnuPlugins", Nil, resources.GetString("mnuPlugins.Text"))
        m_Menu.AddMenu("mnuEditPlugins", "mnuPlugins", Nil, resources.GetString("mnuEditPlugins.Text"))
        m_Menu.AddMenu("mnuScript", "mnuPlugins", Nil, resources.GetString("mnuScript.Text"))
        m_Menu.AddMenu("mnuPluginsBreak1", "mnuPlugins", Nil, "-")

        'Set up the Help menu
        m_Menu.AddMenu("mnuHelp", Nil, resources.GetString("mnuHelp.Text"))
        m_Menu.AddMenu("mnuContents", "mnuHelp", Nil, resources.GetString("mnuContents.Text"))

        'Additional help menu items - cdm 12/22/2005.
        'These are hidden or displayed according to need in modMain.LoadMainForm.
        'This includes hiding offline if connection is available, etc.
        m_Menu.AddMenu("mnuOnlineDocs", "mnuHelp", Nil, resources.GetString("mnuOnlineDocs.Text"))
        m_Menu.AddMenu("mnuOfflineDocs", "mnuHelp", Nil, resources.GetString("mnuOfflineDocs.Text"))

        m_Menu.AddMenu("mnuHelpBreak1", "mnuHelp", Nil, "-")
        m_Menu.AddMenu("mnuShortcuts", "mnuHelp", Nil, resources.GetString("mnuShortcuts.Text"))
        m_Menu.AddMenu("mnuHelpBreak2", "mnuHelp", Nil, "-")

        m_Menu.AddMenu("mnuWelcomeScreen", "mnuHelp", Nil, resources.GetString("mnuWelcomeScreen.Text"))
        'm_Menu.AddMenu("mnuAboutMapWindow", "mnuHelp", Nil, resources.GetString("mnuAboutMapWindow.Text"))

    End Sub

    Private Sub doPluginNameClick(ByVal PluginKey As String)
        'This sub is called when the user clicks a menu item that was placed on the plugins menu during "SynchPluginMenu"
        '1/25/2005 - dpa
        '3/16/2005 - updated to work off a runtime created plugin parent menu

        If m_PluginManager.PluginIsLoaded(PluginKey) Then
            m_PluginManager.StopPlugin(PluginKey)
            m_Menu("plugin_" & PluginKey).Checked = False
        Else
            m_PluginManager.StartPlugin(PluginKey)
            m_Menu("plugin_" & PluginKey).Checked = True
        End If

        'Bugzilla 380 -- Plugins are stored in the project, so a plugin change
        'should set the modified flag.
        SetModified(True)
    End Sub

    Friend Sub CustomMenu_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        'This sub is called when the user clicks a menu item that was placed on the 
        'main menu by a plugin
        '1/30/2005 - dpa - updated
        '3/16/2005 - dpa - using this event for regular menu clicks as well now (e.g. file/new)

        Dim item As ToolStripItem = CType(sender, ToolStripItem)

        'First see if it is a plugin name menu
        If item.Name.StartsWith("plugin_") = True Then
            doPluginNameClick(item.Name.Substring(7))
            Exit Sub
        End If

        'send the click event to all the plugins
        If Not (m_PluginManager.ItemClicked(item.Name)) Then

            'If we get here, then the menu click event was not handled by a plug-in 
            'so we will try to handle it here.  For example in the case of File/New the
            'plugin could handle this click, if so then we don't get to this point.
            'If no plugin handles File/New, then we'll do it.

            Select Case item.Name
                'help menus - do these first so that the logic about keeping help at the
                'end of the menu list works.
                Case "mnuOnlineDocs"
                    System.Diagnostics.Process.Start("http://www.MapWindow.org/wiki")
                Case "mnuOfflineDocs"
                    System.Diagnostics.Process.Start(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) & "\OfflineDocs\index.html")
                    'Case "mnuContents" : doContents()
                    'Case "mnuWelcomeScreen" : ShowWelcomeScreen()
                    'Case "mnuAboutMapWindow" : doAboutMapWindow()
                    'Case "mnuMapWindowDotCom" : doMapWindowDotCom()
                    'Case "mnuNew" : DoNew()
                Case "mnuOpen" : DoOpen()
                    'Case "mnuOpenProjectIntoGroup" : DoOpenIntoCurrent()
                Case "mnuSave" : DoSave()
                Case "mnuSaveAs" : DoSaveAs()
                Case "mnuPrint" : DoPrint()
                    'Case "mnuProjectSettings" : doProjectSettings()
                Case "mnuClose" : doClose()
                    'Case "mnuCheckForUpdates" : CheckForUpdates()
                Case "mnuExit" : doExit()

                    'edit menus

                    'plugins menus
                Case "mnuEditPlugins" : m_PluginManager.ShowPluginDialog()
                Case "mnuScript"
                    'Chris Michaelis Jan 1 2006 - Adapted from the script system written by Mark Gray of AquaTerra.
                    If Scripts Is Nothing OrElse Scripts.IsDisposed Then
                        Scripts = New frmScript
                    End If
                    Scripts.Show()

                Case "mnuShortcuts"
                    Dim strMessage As String
                    strMessage = resources.GetString("msgShortcutsTitle.Text") + vbCrLf + vbCrLf + _
                                 resources.GetString("msgShortcutsDel.Text") + vbCrLf + _
                                 resources.GetString("msgShortcutsIns.Text") + vbCrLf + vbCrLf + _
                                 resources.GetString("msgShortcutsCtrlS.Text") + vbCrLf + _
                                 resources.GetString("msgShortcutsCtrlO.Text") + vbCrLf + _
                                 resources.GetString("msgShortcutsCtrlC.Text") + vbCrLf + _
                                 resources.GetString("msgShortcutsCtrlP.Text") + vbCrLf + _
                                 resources.GetString("msgShortcutsCtrlF4.Text") + vbCrLf + vbCrLf + _
                                 resources.GetString("msgShortcutsHome.Text") + vbCrLf + _
                                 resources.GetString("msgShortcutsCtrlHome.Text") + vbCrLf + _
                                 resources.GetString("msgShortcutsPlus.Text") + vbCrLf + _
                                 resources.GetString("msgShortcutsMinus.Text") + vbCrLf + vbCrLf + _
                                 resources.GetString("msgShortcutsPageUp.Text") + vbCrLf + _
                                 resources.GetString("msgShortcutsPageDown.Text") + vbCrLf + _
                                 resources.GetString("msgShortcutsArrowUp.Text") + vbCrLf + _
                                 resources.GetString("msgShortcutsArrowDown.Text") + vbCrLf + _
                                 resources.GetString("msgShortcutsArrowLeft.Text") + vbCrLf + _
                                 resources.GetString("msgShortcutsArrowRight.Text") + vbCrLf + _
                                 resources.GetString("msgShortcutsCtrlSpace.Text") + vbCrLf + _
                                 resources.GetString("msgShortcutsCtrlArrows.Text") + vbCrLf + _
                                 resources.GetString("msgShortcutsCtrlEnter.Text")

                    MapWinUtility.Logger.Msg(strMessage, MsgBoxStyle.Information, "Keyboard Shortcuts")
                Case Else
                    If item.Name.StartsWith(RecentProjectPrefix) Then
                        'Load a recent project

                        If Not m_HasBeenSaved Then 'OrElse ProjInfo.Modified Then
                            If PromptToSaveProject() = MsgBoxResult.Cancel Then
                                Exit Sub
                            End If
                        End If

                        'Chris Michaelis June 30 2005, also see BuildRecentProjectsMenu
                        If Not Project.Load(item.Name.Substring(RecentProjectPrefix.Length).Replace("{32}", " ")) Then
                            MapWinUtility.Logger.Msg("Could not load " & item.Name.Substring(RecentProjectPrefix.Length), MsgBoxStyle.OkOnly, "Recent Project")
                        End If
                    End If
            End Select
        End If
    End Sub

    Public Sub BuildRecentProjectsMenu()
        '3/17/05 mg
        Dim i As Integer
        Dim filename As String
        Dim key As String
        Dim keysToRemove As New ArrayList

        'Find RecentProject menu items to remove
        'note: cannot remove within For Each, so we remove them next
        For Each key In m_Menu.m_MenuTable.Keys
            If key.StartsWith(RecentProjectPrefix) Then
                keysToRemove.Add(key)
            End If
        Next

        For Each key In keysToRemove
            m_Menu.Remove(key)
        Next

        'Add all current ProjInfo.RecentProjects to the menu
        For i = 0 To ProjInfo.RecentProjects.Count - 1
            filename = Trim(ProjInfo.RecentProjects(i).ToString)
            If Not filename = "" And Not filename = ".mwprj" Then
                ' Chris Michaelis June 30 2005 -- when a path with spaces gets put here, the spaces get cut out when it's turned into a key.
                ' Replacing the space with {32}, the ascii code for space. Also see CustomMenu_Click which does the reverse.
                key = RecentProjectPrefix & filename.Replace(" ", "{32}")
                m_Menu.AddMenu(key, "mnuRecentProjects", Nothing, System.IO.Path.GetFileNameWithoutExtension(filename))
            End If
        Next
    End Sub

    Friend Function PromptToSaveProject() As MsgBoxResult
        '1/31/2005 Modified this to read like the equivalent MS Word dialog. 
        Dim cdlSave As New SaveFileDialog
        Dim Result As MsgBoxResult

        cdlSave.Filter = "MapWindow Project (*.mwprj)|*.mwprj"
        If System.IO.Path.GetFileNameWithoutExtension(ProjInfo.ProjectFileName) = "" Then
            '13/10/2005 - PM
            'Result = mapwinutility.logger.msg("Do you want to save the changes to this project?", MsgBoxStyle.YesNoCancel Or MsgBoxStyle.Exclamation, AppInfo.Name)
            Result = mapwinutility.logger.msg(resources.GetString("msgSaveProject1.Text") & resources.GetString("msgSaveProject2.Text"), MsgBoxStyle.YesNoCancel Or MsgBoxStyle.Exclamation, AppInfo.Name)
        Else
            '13/10/2005 - PM
            'Result = mapwinutility.logger.msg("Do you want to save the changes to " & System.IO.Path.GetFileNameWithoutExtension(ProjInfo.ProjectFileName) & "?", MsgBoxStyle.YesNoCancel Or MsgBoxStyle.Exclamation, AppInfo.Name)
            Result = mapwinutility.logger.msg(resources.GetString("msgSaveProject1.Text") & System.IO.Path.GetFileNameWithoutExtension(ProjInfo.ProjectFileName) & "?", MsgBoxStyle.YesNoCancel Or MsgBoxStyle.Exclamation, AppInfo.Name)
        End If

        Select Case Result
            Case MsgBoxResult.Yes
                If m_HasBeenSaved = True And MapWinUtility.Strings.IsEmpty(ProjInfo.ProjectFileName) = False Then
                    ProjInfo.SaveProject()
                    m_HasBeenSaved = True
                    SetModified(False)
                Else
                    cdlSave.InitialDirectory = AppInfo.DefaultDir
                    If cdlSave.ShowDialog() = DialogResult.Cancel Then Return MsgBoxResult.Cancel

                    If (System.IO.Path.GetExtension(cdlSave.FileName) <> ".mwprj") Then
                        cdlSave.FileName &= ".mwprj"
                    End If
                    ProjInfo.ProjectFileName = cdlSave.FileName
                    ProjInfo.SaveProject()
                    m_HasBeenSaved = True
                    ProjInfo.ProjectFileName = cdlSave.FileName
                    SetModified(False)
                End If
                Return MsgBoxResult.Yes
            Case MsgBoxResult.Cancel
                Return MsgBoxResult.Cancel
            Case MsgBoxResult.No
                Return MsgBoxResult.No
        End Select
    End Function

    Private Sub doClose()
        'Closes the current project
        If Not m_HasBeenSaved Or ProjInfo.Modified Then
            If PromptToSaveProject() = MsgBoxResult.Cancel Then
                Exit Sub
            End If
        End If
        ProjInfo.ProjectFileName = ""
        m_HasBeenSaved = True
        SetModified(False)
    End Sub

    Private Sub doExit()
        'Exit MapWindow
        'save the current configuration
        ProjInfo.SaveConfig()
        If Not m_HasBeenSaved Or ProjInfo.Modified Then
            If PromptToSaveProject() = MsgBoxResult.Cancel Then
                Exit Sub
            End If
        End If
        Me.Close()
    End Sub

    Friend Sub SynchPluginMenu()
        'Clears the list of plug-ins from the plugins menu and then refreshes them.
        '1/25/2005 - dpa - updated
        '3/16/2005 - dpa - changed to work on run-time created plug-in parent menu.
        If Not g_SyncPluginMenuDefer Then

            Dim ParentMenu As Windows.Forms.ToolStripMenuItem
            Dim ChildMenu As Interfaces.MenuItem
            Dim MenuKey As String
            Dim i As Integer = 0

            ParentMenu = CType(m_Menu.m_MenuTable("mnuPlugins"), Windows.Forms.ToolStripMenuItem)

            Dim alph_PluginList As Hashtable = m_PluginManager.PluginsList
            Dim Names() As String, Keys() As String
            i = 0
            ReDim Names(alph_PluginList.Count - 1)
            ReDim Keys(alph_PluginList.Count - 1)
            Dim ienum As IDictionaryEnumerator = alph_PluginList.GetEnumerator()
            While ienum.MoveNext
                Names(i) = ienum.Value.name
                Keys(i) = ienum.Value.key
                i += 1
            End While

            ' 2 - Sort using a custom IComparer
            Array.Sort(Names, Keys)

            ' 3 - Now add the plugin menu items at the end of the menu, using the sorted arraylist
            For i = 0 To Names.Length - 1
                MenuKey = "plugin_" & Keys(i)

                ' Chris Michaelis June 30 2005 - allow a plug-in
                ' to specify that it belongs in a submenu of the plugins menu
                ' via the syntax "Subcategory::Plugin Name" as the plugin name string.
                Try
                    If InStr(Names(i), "::") > 0 Then
                        Dim subCat As String = "subcat_" + Names(i).Substring(0, InStr(Names(i), "::"))
                        If Not m_Menu.Contains(subCat) Then m_Menu.AddMenu(subCat, "mnuPlugins", Nothing, Names(i).Substring(0, InStr(Names(i), "::") - 1))
                        ChildMenu = m_Menu.AddMenu(MenuKey, subCat, Nothing, Names(i).Substring(InStr(Names(i), "::") + 1))
                        ChildMenu.Checked = m_PluginManager.PluginIsLoaded(Keys(i))
                    Else
                        ChildMenu = m_Menu.AddMenu(MenuKey, "mnuPlugins", Nothing, Names(i))
                        ChildMenu.Checked = m_PluginManager.PluginIsLoaded(Keys(i))
                    End If
                Catch ex As Exception
                    MapWinUtility.Logger.Msg(ex.ToString())
                End Try
            Next
        End If
    End Sub

#Region "Interfaces.IMapWin"
    Public Sub RefreshMap() Implements Interfaces.IMapWin.Refresh
        frmMain.Refresh()
    End Sub

    Public Function GetProjectionFromUser(ByVal DialogCaption As String, ByVal DefaultProjection As String) As String Implements Interfaces.IMapWin.GetProjectionFromUser
        Return ""
    End Function

    Public ReadOnly Property ApplicationInfo() As Interfaces.AppInfo Implements Interfaces.IMapWin.ApplicationInfo
        Get
            Return modMain.AppInfo
        End Get
    End Property

    Public ReadOnly Property UserInteraction() As Interfaces.UserInteraction Implements Interfaces.IMapWin.UserInteraction
        Get
            Return Nothing
        End Get
    End Property

    Public Sub ClearCustomWindowTitle() Implements Interfaces.IMapWin.ClearCustomWindowTitle
        CustomWindowTitle = ""
    End Sub

    Public ReadOnly Property GetOCX() As Object Implements Interfaces.IMapWin.GetOCX
        Get
            Return Nothing
        End Get
    End Property

    Private Sub DoPrint()
        'Prints a simple layout
        'This is called by a menu click and a button click.
        'Dim printForm As New frmPrint
        'printForm.ShowDialog()
    End Sub

    Private Sub DoNew()
        'Starts a new project                    
        If Not m_HasBeenSaved Or ProjInfo.Modified Then
            If PromptToSaveProject() = MsgBoxResult.Cancel Then
                Exit Sub
            End If
        End If
        ProjInfo.ProjectFileName = ""
        SetModified(False)
        ProjInfo.SaveConfig() 'Save any configuration-level changes before we reload the config. 3/23/2006 by CDM for bug 102
        ProjInfo.LoadConfig(True)
    End Sub

    Private Sub DoOpen()
        'Opens an existing project"
        Dim cdlOpen As New OpenFileDialog
        cdlOpen.Filter = "MapWindow Project (*.mwprj)|*.mwprj"

        'check to see if they want to save the project
        If Not m_HasBeenSaved Or ProjInfo.Modified Then
            If PromptToSaveProject() = MsgBoxResult.Cancel Then
                Exit Sub
            End If
        End If

        'open a new project
        If (System.IO.Directory.Exists(AppInfo.DefaultDir)) Then
            cdlOpen.InitialDirectory = AppInfo.DefaultDir
        End If
        cdlOpen.ShowDialog()

        If System.IO.File.Exists(cdlOpen.FileName) Then
            'save the location of the last open dir
            AppInfo.DefaultDir = System.IO.Path.GetDirectoryName(cdlOpen.FileName)
            ProjInfo.ProjectFileName = cdlOpen.FileName
            ProjInfo.LoadProject(cdlOpen.FileName)

            m_HasBeenSaved = True
            ProjInfo.ProjectFileName = cdlOpen.FileName
            SetModified(False)
        End If
    End Sub

    Private Sub DoSave()
        'Saves the current project
        Try
            ' this looks like a bunch of changes in a diff, but it's not (tws 6/27/07)
            Dim cdlSave As New SaveFileDialog
            cdlSave.Filter = "MapWindow Project (*.mwprj)|*.mwprj"
            If Not m_HasBeenSaved Or ProjInfo.ProjectFileName = String.Empty Then
                cdlSave.InitialDirectory = AppInfo.DefaultDir
                If (cdlSave.ShowDialog = DialogResult.Cancel) Then Exit Sub

                If (System.IO.Path.GetExtension(cdlSave.FileName) <> ".mwprj") Then
                    cdlSave.FileName &= ".mwprj"
                End If
                ProjInfo.ProjectFileName = cdlSave.FileName
                Me.Cursor = Cursors.WaitCursor
                If (ProjInfo.SaveProject()) Then
                    m_HasBeenSaved = True
                    ProjInfo.ProjectFileName = cdlSave.FileName
                    SetModified(False)
                End If
            Else
                Me.Cursor = Cursors.WaitCursor
                If (ProjInfo.SaveProject()) Then
                    m_HasBeenSaved = True
                    SetModified(False)
                End If
            End If
        Finally ' exceptions still propagate up, but we will NEVER leave the hourglass on
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub DoSaveAs()
        'Saves the project under a new file name
        Dim cdlSave As New SaveFileDialog
        cdlSave.Filter = "MapWindow Project (*.mwprj)|*.mwprj"
        If (cdlSave.ShowDialog = DialogResult.Cancel) Then Exit Sub

        If (System.IO.Path.GetExtension(cdlSave.FileName) <> ".mwprj") Then
            cdlSave.FileName &= ".mwprj"
        End If

        ProjInfo.ProjectFileName = cdlSave.FileName
        If (ProjInfo.SaveProject()) Then
            m_HasBeenSaved = True
            ProjInfo.ProjectFileName = cdlSave.FileName
            SetModified(False)
        End If
    End Sub

    Public Sub SetModified(ByVal Status As Boolean)
        'If Not ProjInfo Is Nothing AndAlso ((ProjInfo.ProjectFileName Is Nothing OrElse ProjInfo.ProjectFileName.Trim() = "") And frmMain.MapMain.NumLayers = 0) Then
        '    Status = False
        'End If

        'ProjInfo.Modified = Status

        '' cdm 7/11/05 - added custom window title option support
        'If MapWinUtility.Strings.IsEmpty(ProjInfo.ProjectFileName) Then
        frmMain.Text = AppInfo.Name + " " + CType(IIf(CustomWindowTitle = "", "", " - " + CustomWindowTitle), String) + CType(IIf(Status, "*", ""), String)
        'Else
        '    frmMain.Text = AppInfo.Name + " " + CType(IIf(CustomWindowTitle = "", "", " - " + CustomWindowTitle), String) + " - " + CType(IIf(frmMain.Title_ShowFullProjectPath, ProjInfo.ProjectFileName, System.IO.Path.GetFileNameWithoutExtension(ProjInfo.ProjectFileName)), String) + CType(IIf(Status, "*", ""), String)
        'End If
    End Sub


    Public WriteOnly Property DisplayFullProjectPath() As Boolean Implements Interfaces.IMapWin.DisplayFullProjectPath
        Set(ByVal Value As Boolean)
            Title_ShowFullProjectPath = Value
            SetModified(False) 'Force rewrite of title
        End Set
    End Property

    Public Sub SetCustomWindowTitle(ByVal NewTitleText As String) Implements Interfaces.IMapWin.SetCustomWindowTitle
        CustomWindowTitle = NewTitleText
        SetModified(False) 'Force rewrite of title
    End Sub

    Public ReadOnly Property LastError() As String Implements Interfaces.IMapWin.LastError
        Get
            Dim tStr As String

            If g_error Is Nothing Then Return ""

            tStr = String.Copy(g_error)
            g_error = ""
            Return tStr
        End Get
    End Property

    Public ReadOnly Property Layers() As Interfaces.Layers Implements Interfaces.IMapWin.Layers
        Get
            Return Nothing
        End Get
    End Property

    Public ReadOnly Property View() As Interfaces.View Implements Interfaces.IMapWin.View
        Get
            Return Nothing
        End Get
    End Property

    Public ReadOnly Property Menus() As Interfaces.Menus Implements Interfaces.IMapWin.Menus
        Get
            Return m_Menu
        End Get
    End Property

    Public ReadOnly Property Plugins() As Interfaces.Plugins Implements Interfaces.IMapWin.Plugins
        Get
            Return m_PluginManager
        End Get
    End Property

    Public ReadOnly Property PreviewMap() As Interfaces.PreviewMap Implements Interfaces.IMapWin.PreviewMap
        Get
            Return Nothing
        End Get
    End Property

    Public ReadOnly Property StatusBar() As Interfaces.StatusBar Implements Interfaces.IMapWin.StatusBar
        Get
            Return Nothing 'm_StatusBar
        End Get
    End Property

    Public ReadOnly Property Toolbar() As Interfaces.Toolbar Implements Interfaces.IMapWin.Toolbar
        Get
            Return m_Toolbar
        End Get
    End Property

    Public ReadOnly Property Reports() As MapWindow.Interfaces.Reports Implements MapWindow.Interfaces.IMapWin.Reports
        Get
            Return Nothing
        End Get
    End Property

    Public ReadOnly Property UIPanel() As MapWindow.Interfaces.UIPanel Implements MapWindow.Interfaces.IMapWin.UIPanel
        Get
            Return Nothing
        End Get
    End Property

    Public ReadOnly Property Project() As MapWindow.Interfaces.Project Implements MapWindow.Interfaces.IMapWin.Project
        Get
            Return m_Project
        End Get
    End Property

    Public Sub ShowErrorDialog(ByVal ex As System.Exception) Implements MapWindow.Interfaces.IMapWin.ShowErrorDialog
        ShowError(ex)
    End Sub

#End Region

End Class