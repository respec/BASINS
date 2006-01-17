Imports System.Collections.Specialized
Imports System.Windows.Forms.Application
Imports System.Reflection
Imports atcUtility
Imports atcData
Imports MapWinUtility

Public Class atcBasinsPlugIn
  Implements MapWindow.Interfaces.IPlugin

  Private Const NationalProjectFilename As String = "national.mwprj"

  'Private Const NewProjectMenuName As String = "BasinsNewProject"
  'Private Const NewProjectMenuString As String = "&New Project"

  Private Const ProjectsMenuName As String = "BasinsProjects"
  Private Const ProjectsMenuString As String = "Open &BASINS Project"

  Private Const OpenDataMenuName As String = "BasinsOpenData"
  Private Const OpenDataMenuString As String = "Open Data"

  Private Const SaveDataMenuName As String = "BasinsSaveData"
  Private Const SaveDataMenuString As String = "Save Data In..."

  Private Const ToolsMenuName As String = "BasinsTools"
  Private Const ToolsMenuString As String = "&Tools"

  Private Const ModelsMenuName As String = "BasinsModels"
  Private Const ModelsMenuString As String = "&Models"

  Private pWelcomeScreenShow As Boolean = False
  Private Const WelcomeScreenShowMenuName As String = "ShowWelcome"
  Private Const WelcomeScreenShowMenuString As String = "&Show Welcome Screen"

  Private Const CheckForUpdatesMenuName As String = "CheckForUpdates"
  Private Const CheckForUpdatesMenuString As String = "&Check For Updates"

  Private Const BasinsWebPageMenuName As String = "BasinsWebPage"
  Private Const BasinsWebPageMenuString As String = "&Basins Web Page"

  Private Const SendFeedbackMenuName As String = "SendFeedback"
  Private Const SendFeedbackMenuString As String = "Send &Feedback"

  Private Const BasinsHelpMenuName As String = "BASINS Help"
  Private Const BasinsHelpMenuString As String = "BASINS Help"

  Private Const BasinsProjectPath As String = "\Basins\apr\"
  Private Const BasinsDataPath As String = "\Basins\data\"

  Private Const DataMenuName As String = "BasinsData"
  Private Const DataMenuString As String = "&Data"
  Private pLoadedDataMenu As Boolean = False

  Private pLogFilename As String = ""
  Private WithEvents pDataManager As atcDataManager

  Private pCommandLineScript As Boolean = False
  Private pBuiltInScriptExists As Boolean = False

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
      Return "Open BASINS data in MapWindow"
    End Get
  End Property

  Public ReadOnly Property BuildDate() As String Implements MapWindow.Interfaces.IPlugin.BuildDate
    Get
      Return System.IO.File.GetLastWriteTime(Me.GetType().Assembly.Location)
    End Get
  End Property

  Public ReadOnly Property Version() As String Implements MapWindow.Interfaces.IPlugin.Version
    Get
      Return System.Diagnostics.FileVersionInfo.GetVersionInfo(Me.GetType().Assembly.Location).FileVersion
    End Get
  End Property

  Public ReadOnly Property MapWin() As MapWindow.Interfaces.IMapWin
    Get
      Return g_MapWin
    End Get
  End Property

  Public Sub Initialize(ByVal aMapWin As MapWindow.Interfaces.IMapWin, ByVal aParentHandle As Integer) Implements MapWindow.Interfaces.IPlugin.Initialize

    'fired when the user loads plug-in through plug-in dialog 
    'or by checkmarking it in the plug-ins menu.
    'This is where buttons or menu items are added.
    Dim DriveLetter As String
    Dim iDrive As Integer
    Dim iDirectory As Integer
    Dim mnu As MapWindow.Interfaces.MenuItem

    g_MapWin = aMapWin
    g_MapWinWindowHandle = aParentHandle
    Dim lLogFileName As String = PathNameOnly(PathNameOnly(System.Reflection.Assembly.GetEntryAssembly.Location)) _
                               & "\logs\" _
                               & Format(Now, "yyyy-MM-dd") & "at" & Format(Now, "HH-mm") & "-Basins.log"

    Logger.StartToFile(lLogFileName)
    'LogStartMonitor()
    Logger.MapWin = g_MapWin

    pDataManager = New atcDataManager(g_MapWin) ', Me)

    BuiltInScript(False)

    FindBasinsDrives()

    'AddMenuIfMissing(NewProjectMenuName, "mnuFile", NewProjectMenuString, "mnuNew")
    'g_MapWin.Menus.Remove("New")
    AddMenuIfMissing(OpenDataMenuName, "mnuFile", OpenDataMenuString, "mnuOpen")
    AddMenuIfMissing(SaveDataMenuName, "mnuFile", SaveDataMenuString, "mnuSaveAs")
    AddMenuIfMissing(ProjectsMenuName, "mnuFile", ProjectsMenuString, "mnuRecentProjects")

    AddMenuIfMissing("BasinsHelp_Separator1", "mnuHelp", "-")
    'mnu = AddMenuIfMissing(WelcomeScreenShowMenuName, "mnuHelp", WelcomeScreenShowMenuString, "")
    mnu = AddMenuIfMissing(BasinsWebPageMenuName, "mnuHelp", BasinsWebPageMenuString, "")
    AddMenuIfMissing("BasinsHelp_Separator2", "mnuHelp", "-")
    mnu = AddMenuIfMissing(CheckForUpdatesMenuName, "mnuHelp", CheckForUpdatesMenuString, "")
    mnu = AddMenuIfMissing(SendFeedbackMenuName, "mnuHelp", SendFeedbackMenuString, "")
    'AddMenuIfMissing(BasinsHelpMenuName, "mnuHelp", BasinsHelpMenuString, "")

    For iDrive = 0 To g_BasinsDrives.Length - 1
      DriveLetter = g_BasinsDrives.Substring(iDrive, 1)
      'Scan folder for project data, and populate menu
      Dim DataDirs() As String = System.IO.Directory.GetDirectories( _
                                        DriveLetter & ":\BASINS\data")
      For iDirectory = 0 To DataDirs.GetUpperBound(0)
        Dim DirShortName As String = System.IO.Path.GetFileName(DataDirs(iDirectory))
        If g_BasinsDrives.Length > 0 Then DirShortName = DriveLetter & ": " & DirShortName
        mnu = AddMenuIfMissing(ProjectsMenuName & "_" & DirShortName, _
                               ProjectsMenuName, DataDirs(iDirectory))
        mnu.Tooltip = DataDirs(iDirectory)
      Next
    Next

    RefreshDataMenu()

    RefreshToolsMenu()

    AddMenuIfMissing(ModelsMenuName, "", ModelsMenuString, ToolsMenuName)
    'mnu = AddMenuIfMissing(ModelsMenuName & "_HSPF", ModelsMenuName, "&HSPF")
    'mnu.Tooltip = "Hydrological Simulation Program - Fortran"
    mnu = AddMenuIfMissing(ModelsMenuName & "_SWAT", ModelsMenuName, "&SWAT")
    mnu.Tooltip = "SWAT"
    mnu = AddMenuIfMissing(ModelsMenuName & "_PLOAD", ModelsMenuName, "&PLOAD")
    mnu.Tooltip = "PLOAD"
    mnu = AddMenuIfMissing(ModelsMenuName & "_AGWA", ModelsMenuName, "&AGWA")
    mnu.Tooltip = "AGWA"
    mnu = AddMenuIfMissing(ModelsMenuName & "_AQUATOX", ModelsMenuName, "AQUA&TOX")
    mnu.Tooltip = "AQUATOX"

    'load HSPF plugin (an integral part of BASINS)
    'g_MapWin.Plugins.StartPlugin("atcHSPF_PlugIn")

  End Sub

  'Private Function ScriptFolder() As String
  '  Return PathNameOnly(g_MapWin.Plugins.PluginFolder) & "\script"
  'End Function

  Private Function AddMenuIfMissing(ByVal aMenuName As String, _
                                    ByVal aParent As String, _
                                    ByVal aMenuText As String, _
                           Optional ByVal aAfter As String = "") As MapWindow.Interfaces.MenuItem
    With g_MapWin.Menus
      If .Item(aMenuName) Is Nothing Then
        Return .AddMenu(aMenuName, aParent, Nothing, aMenuText, aAfter)
      Else
        Return .Item(aMenuName)
      End If
    End With
  End Function

  Private Sub RefreshToolsMenu()
    Dim lScriptMenuName As String = ToolsMenuName & "_Scripting"
    Dim iPlugin As Integer
    If pLoadedDataMenu Then
      AddMenuIfMissing(ToolsMenuName, "", ToolsMenuString, DataMenuName)
      'AddMenuIfMissing(ToolsMenuName & "_TestDBF", ToolsMenuName, "Test DBF")
      AddMenuIfMissing(ToolsMenuName & "_ArcView3", ToolsMenuName, "ArcView &3")
      AddMenuIfMissing(ToolsMenuName & "_ArcGIS", ToolsMenuName, "&ArcGIS")
      AddMenuIfMissing(ToolsMenuName & "_GenScn", ToolsMenuName, "&GenScn")
      AddMenuIfMissing(ToolsMenuName & "_WDMUtil", ToolsMenuName, "&WDMUtil")
      'AddMenuIfMissing(lScriptMenuName, ToolsMenuName, "Scripting")
      'If pBuiltInScriptExists Then
      '  AddMenuIfMissing(ToolsMenuName & "_RunBuiltInScript", lScriptMenuName, "Run Built In Script")
      'End If
      'AddMenuIfMissing(ToolsMenuName & "_ScriptEditor", lScriptMenuName, "Script Editor")
      'AddMenuIfMissing(ToolsMenuName & "_RunScript", lScriptMenuName, "Select Script to Run")

      'For Each lScriptFilename As String In System.IO.Directory.GetFiles(ScriptFolder, "*.vb")
      '  Dim lMenuLabel As String = FilenameOnly(lScriptFilename)
      '  If Not lMenuLabel.ToLower.StartsWith("sub") AndAlso Not lMenuLabel.ToLower.StartsWith("fun") Then
      '    AddMenuIfMissing(ToolsMenuName & "_RunScript" & FilenameNoPath(lScriptFilename), lScriptMenuName, "Run " & lMenuLabel)
      '  End If
      'Next

      'AddMenuIfMissing(ToolsMenuName & "_ChangeProjection", ToolsMenuName, "Change &Projection")

      Dim DisplayPlugins As ICollection = pDataManager.GetPlugins(GetType(atcDataDisplay))
      If DisplayPlugins.Count > 0 Then
        AddMenuIfMissing(ToolsMenuName & "_Separator1", ToolsMenuName, "-")
      End If
      For Each lDisp As atcDataDisplay In DisplayPlugins
        Dim lMenuText As String = lDisp.Name
        If lMenuText.StartsWith("Tools::") Then lMenuText = lMenuText.Substring(7)
        AddMenuIfMissing(ToolsMenuName & "_" & lDisp.Name, ToolsMenuName, lMenuText)
      Next
    End If
  End Sub

  Private Sub RefreshDataMenu()
    'Dim mnu As MapWindow.Interfaces.MenuItem
    Dim iPlugin As Integer
    AddMenuIfMissing(DataMenuName, "", DataMenuString, "mnuFile")
    AddMenuIfMissing(DataMenuName & "_Download", DataMenuName, "&Download")
    AddMenuIfMissing(DataMenuName & "_ComputeData", DataMenuName, "&Compute")
    AddMenuIfMissing(DataMenuName & "_ManageDataSources", DataMenuName, "&Manage Sources")
    pLoadedDataMenu = True
    'With g_MapWin.Plugins
    '  For iPlugin = 0 To .Count - 1
    '    If Not .Item(iPlugin) Is Nothing Then
    '      Dim pluginName As String = .Item(iPlugin).Name
    '      If CType(.Item(iPlugin), Object).GetType().IsSubclassOf(GetType(atcDataPlugin)) Then
    '        mnu = AddMenuIfMissing(DataMenuName & "_" & pluginName, DataMenuName, pluginName)
    '      End If
    '    End If
    '  Next
    'End With
  End Sub

  Public Sub Terminate() Implements MapWindow.Interfaces.IPlugin.Terminate
    'This event is fired when the user unloads your plug-in either through the plug-in dialog 
    'box, or by un-checkmarking it in the plug-ins menu.  This is where you would remove any
    'buttons from the tool bar tool bar or menu items from the menu that you may have added.
    'If you don't do this, then you will leave dangling menus and buttons that don't do anything.

    g_MapWin.Menus.Remove(DataMenuName)
    pLoadedDataMenu = False
    g_MapWin.Menus.Remove(ToolsMenuName)
    g_MapWin.Menus.Remove(ModelsMenuName) 'TODO: don't unload if another plugin is still using it
    g_MapWin.Menus.Remove(ProjectsMenuName)
    g_MapWin.Menus.Remove(OpenDataMenuName)
    g_MapWin.Menus.Remove(SaveDataMenuName)

    g_MapWin.Menus.Remove("BasinsHelp_Separator1")
    g_MapWin.Menus.Remove(WelcomeScreenShowMenuName)
    g_MapWin.Menus.Remove(CheckForUpdatesMenuName)
    g_MapWin.Menus.Remove("BasinsHelp_Separator2")
    g_MapWin.Menus.Remove(BasinsWebPageMenuName)
    g_MapWin.Menus.Remove(SendFeedbackMenuName)


    'LogStopMonitor()

  End Sub

  Public Function NationalProjectIsOpen() As Boolean
    If (Not g_MapWin.Project Is Nothing) _
        AndAlso (Not g_MapWin.Project.FileName Is Nothing) _
        AndAlso g_MapWin.Project.FileName.ToLower.EndsWith(NationalProjectFilename) Then

      Return True
    Else
      Return False
    End If
  End Function

  Public Sub ItemClicked(ByVal ItemName As String, ByRef Handled As Boolean) Implements MapWindow.Interfaces.IPlugin.ItemClicked
    'A menu item or toolbar button was clicked
    Dim DataDirName As String
    Dim PrjFileName As String

    Logger.Dbg(ItemName)
    If ItemName.Equals("mnuNew") Then 'Override File/New menu item behavior
      If NationalProjectIsOpen() Then
        MsgBox("Select the area of interest, " & vbCr & "then Download from the Data menu" & vbCr _
             & "to create a new BASINS project", , "New Project")
      Else
        Dim allFiles As NameValueCollection
        Dim iDrive As Integer
        Dim sDrive As String
        Dim lFileName As String = PathNameOnly(PathNameOnly(g_MapWin.Plugins.PluginFolder)) & "\Data\national\" & NationalProjectFilename
        allFiles = New NameValueCollection

        If FileExists(lFileName) Then
          allFiles.Add(lFileName.ToLower, lFileName)
        Else
          For iDrive = 0 To g_BasinsDrives.Length - 1
            sDrive = UCase(g_BasinsDrives.Chars(iDrive))
            AddFilesInDir(allFiles, sDrive & ":\BASINS\Data\national\", True, NationalProjectFilename)
          Next
        End If
        If allFiles.Count > 0 Then
          g_MapWin.Project.Load(allFiles.Item(0))
        Else
          Logger.Msg("Unable to find '" & NationalProjectFilename & "' on drives: " & g_BasinsDrives & " in folder \BASINS\Data\national\", "Open National")
        End If
      End If
      Handled = True
    ElseIf ItemName.StartsWith(SaveDataMenuName & "_") Then
      Dim lSaveIn As atcDataSource
      Dim lSaveGroup As atcDataGroup = pDataManager.UserSelectData("Select Data to Save")
      If Not lSaveGroup Is Nothing AndAlso lSaveGroup.Count > 0 Then
        If ItemName.Length > SaveDataMenuName.Length Then
          Dim lSpecification As String = ItemName.Substring(SaveDataMenuName.Length + 1)
          For Each lDataSource As atcDataSource In pDataManager.DataSources
            If lDataSource.Specification = lSpecification Then
              lSaveIn = lDataSource
              Exit For
            End If
          Next
        End If

        If lSaveIn Is Nothing Then
          lSaveIn = UserOpenDataFile(False, True)
        End If

        If Not lSaveIn Is Nothing And lSaveIn.Specification.Length > 0 Then
          For Each lDataSet As atcDataSet In lSaveGroup
            lSaveIn.AddDataSet(lDataSet, atcData.atcDataSource.EnumExistAction.ExistRenumber)
          Next
          lSaveIn.Save(lSaveIn.Specification)
        End If
      End If
    ElseIf ItemName.Equals(OpenDataMenuName) Then
      UserOpenDataFile()
    ElseIf ItemName.StartsWith(ProjectsMenuName & "_") Then
      DataDirName = g_MapWin.Menus(ItemName).Text ' g_MapWin.Menus(ItemName).Tooltip
      If FileExists(DataDirName, True, False) Then
        PrjFileName = DataDirName & "\" & FilenameOnly(DataDirName) & ".mwprj"
        If FileExists(PrjFileName) Then
          Logger.Dbg("Opening project " & PrjFileName)
          g_MapWin.Project.Load(PrjFileName)
        Else
          'TODO: look for other *.mwprj before creating a new one?
          Logger.Dbg("Creating new project " & PrjFileName)
          g_MapWin.Layers.Clear()
          g_MapWin.Refresh()
          g_MapWin.PreviewMap.GetPictureFromMap()
          DoEvents()
          AddAllShapesInDir(DataDirName, DataDirName)
          g_MapWin.Project.Save(PrjFileName)
          g_MapWin.Project.Modified = False
        End If
      End If
      Handled = True
    ElseIf ItemName.StartsWith(DataMenuName & "_") Then
      Select Case ItemName.Substring(DataMenuName.Length + 1)
        'Case "DownloadTest"
        '  If FileExists("d:\temp\NHDM2003.zip") Then Kill("d:\temp\NHDM2003.zip")
        '  atcUtility.Downloader.DownloadURL("ftp://nhdftp.usgs.gov/SubRegions/Medium/NHDM2003.zip", "d:\temp\NHDM2003.zip")
      Case "Download"
          If NationalProjectIsOpen() Then
            Dim themeTag As String
            Dim FieldName As String
            Dim iField As Integer
            Dim iFieldMatch As Integer = -1
            Dim curLayer As MapWinGIS.Shapefile
            curLayer = g_MapWin.Layers.Item(g_MapWin.Layers.CurrentLayer).GetObject
            Select Case FilenameOnly(curLayer.Filename).ToLower
              Case "cat", "huc", "huc250d3"
                themeTag = "huc_cd"
                FieldName = "CU"
              Case "cnty"
                themeTag = "county_cd"
                FieldName = "FIPS"
              Case "st"
                themeTag = "state_abbrev"
                FieldName = "ST"
              Case Else
                Logger.Msg("Unknown layer for selection, using first field", "Area Selection")
                themeTag = "huc_cd"
                iFieldMatch = 1
            End Select
            FieldName = FieldName.ToLower
            For iField = 0 To curLayer.NumFields - 1
              If curLayer.Field(iField).Name.ToLower = FieldName Then iFieldMatch = iField
            Next
            If iFieldMatch >= 0 Then
              'Save national project as the user has zoomed it
              g_MapWin.Project.Save(g_MapWin.Project.FileName)
              CreateNewProjectAndDownloadCoreDataInteractive(themeTag, GetSelected(iFieldMatch))
            Else
              Logger.Msg("Could not find field " & FieldName & " in " & curLayer.Filename, "Could not create project")
            End If
          Else
            DownloadNewData(PathNameOnly(g_MapWin.Project.FileName) & "\")
          End If
        Case "ComputeData"
          Dim lNotFiles As New ArrayList
          Dim lDataSources As atcCollection = pDataManager.GetPlugins(GetType(atcDataSource))
          For Each ds As atcDataSource In lDataSources
            If ds.Category <> "File" AndAlso Not lNotFiles.Contains(ds.Category) Then
              lNotFiles.Add(ds.Category)
            End If
          Next
          Dim lNewSource As atcDataSource = pDataManager.UserSelectDataSource(lNotFiles, "Select a Computation")
          If Not lNewSource Is Nothing Then 'user did not cancel
            pDataManager.OpenDataSource(lNewSource, lNewSource.Specification, Nothing)
            If Not lNewSource.DataSets Is Nothing AndAlso lNewSource.DataSets.Count > 0 Then
              Dim lForm As New frmSelectDisplay
              lForm.AskUser(pDataManager, lNewSource.DataSets)
            End If
          End If
        Case "ManageDataSources"
          pDataManager.UserManage()
        Case Else : MsgBox("Data Tool " & ItemName)
      End Select
      Handled = True
    ElseIf ItemName.StartsWith(ToolsMenuName & "_") Then
      Handled = LaunchTool(ItemName.Substring(ToolsMenuName.Length + 1))
    ElseIf ItemName.StartsWith(ModelsMenuName & "_") Then
      Handled = LaunchTool(ItemName.Substring(ModelsMenuName.Length + 1))
    ElseIf ItemName.StartsWith(WelcomeScreenShowMenuName) Then
      g_MapWin.Plugins.BroadcastMessage("WELCOME_SCREEN")
      Handled = True
    ElseIf ItemName.StartsWith(CheckForUpdatesMenuName) Then
      OpenFile("http://hspf.com/pub/basins4/updates.html", True)
      Handled = True
    ElseIf ItemName.StartsWith(BasinsWebPageMenuName) Then
      OpenFile("http://www.epa.gov/waterscience/basins/index.html")
      Handled = True
    ElseIf ItemName.StartsWith(SendFeedbackMenuName) Then
      SendFeedback()
      Handled = True
    ElseIf ItemName.StartsWith(BasinsHelpMenuName) Then
      Dim lHelpFilename As String = FindFile("Please locate BASINS 4 help file", g_MapWin.ApplicationInfo.DefaultDir & "\docs\Basins4.chm")
      If FileExists(lHelpFilename) Then System.Diagnostics.Process.Start(lHelpFilename)
    Else 'Not our item
      'MsgBox("Other button: " & ItemName)
    End If
  End Sub

  Private Function UserOpenDataFile(Optional ByVal aNeedToOpen As Boolean = True, _
                                    Optional ByVal aNeedToSave As Boolean = False) As atcDataSource
    Dim lFilesOnly As New ArrayList(1)
    lFilesOnly.Add("File")
    Dim lNewSource As atcDataSource = pDataManager.UserSelectDataSource(lFilesOnly, "Select a File Type", aNeedToOpen, aNeedToSave)
    If Not lNewSource Is Nothing Then 'user did not cancel
      pDataManager.OpenDataSource(lNewSource, lNewSource.Specification, Nothing)
      If Not lNewSource.DataSets Is Nothing AndAlso lNewSource.DataSets.Count > 0 Then
        Dim lForm As New frmSelectDisplay
        lForm.AskUser(pDataManager, lNewSource.DataSets)
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
    Dim exename As String
    Select Case aToolName
      Case "GenScn" : exename = FindFile("Please locate GenScn.exe", "\BASINS\models\HSPF\bin\GenScn.exe")
      Case "WDMUtil" : exename = FindFile("Please locate WDMUtil.exe", "\BASINS\models\HSPF\WDMUtil\WDMUtil.exe")
      Case "HSPF"
        'If g_MapWin.Plugins.PluginIsLoaded("atcModelSetup_PlugIn") Then 'defer to other plugin
        Return False
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
            Logger.Msg("Not yet able to launch " & aToolName, "Option not yet functional")
          End If
        'End If
    End Select

    If FileExists(exename) Then
      Shell("""" & exename & """", AppWinStyle.NormalFocus, False)
      Return True
    Else
      Logger.Msg("Unable to launch " & aToolName, "Launch")
      Return False
    End If
  End Function

  Private Sub SendFeedback()
    Dim lFeedback As String = "Feedback at " & Now.ToString("u") & vbCrLf
    lFeedback &= "CommandLine: " & System.Environment.CommandLine & vbCrLf
    lFeedback &= "User: " & System.Environment.UserName & vbCrLf
    lFeedback &= "Machine: " & System.Environment.MachineName & vbCrLf
    lFeedback &= "OSVersion: " & System.Environment.OSVersion.ToString & vbCrLf
    lFeedback &= "CLRVersion: " & System.Environment.Version.ToString & vbCrLf

    Dim lStartDir As String = PathNameOnly(PathNameOnly(g_MapWin.Plugins.PluginFolder))
    Dim lSkipFilename As Integer = lStartDir.Length + 1
    lFeedback &= vbCrLf & "Files in " & lStartDir & vbCrLf

    Dim lallFiles As New NameValueCollection
    AddFilesInDir(lallFiles, lStartDir, True)
    lFeedback &= vbCrLf & "Filename" & vbTab & "Size" & vbTab & "Modified" & vbCrLf
    For Each lFilename As String In lallFiles
      lFeedback &= FileDateTime(lFilename).ToString("yyyy-MM-dd HH:mm:ss") & vbTab & Format(FileLen(lFilename), "#,###") & vbTab & lFilename.Substring(lSkipFilename) & vbCrLf
    Next

    Dim client As New System.Net.WebClient
    Dim lFeedbackCollection As New NameValueCollection
    lFeedbackCollection.Add("sysinfo", lFeedback)
    client.UploadValues("http://hspf.com/cgi-bin/feedback-basins4.cgi", "POST", lFeedbackCollection)
    Logger.Msg("Feedback successfully sent", "Send Feedback")
  End Sub

  Private Sub BuiltInScript(ByVal aRun As Boolean)
    Try
      Dim lFlags As BindingFlags = BindingFlags.NonPublic Or BindingFlags.Public Or _
                                   BindingFlags.Static Or BindingFlags.Instance Or _
                                   BindingFlags.DeclaredOnly
      Dim lAssembly As [Assembly] = [Assembly].Load("atcScriptTest")
      Dim lTypes As Type() = lAssembly.GetTypes
      For Each lType As Type In lTypes
        Dim lMethods As MethodInfo() = lType.GetMethods(lFlags)
        For Each lMethod As MethodInfo In lMethods
          If lMethod.Name.ToLower = "main" AndAlso _
             lMethod.GetParameters.Length = 2 AndAlso _
             (lMethod.GetParameters(0).ParameterType Is pDataManager.GetType _
              OrElse lMethod.GetParameters(0).ParameterType.Name = "Object") AndAlso _
             (lMethod.GetParameters(1).ParameterType Is Me.GetType _
              OrElse lMethod.GetParameters(1).ParameterType.Name = "Object") Then 'found built in script

            pBuiltInScriptExists = True
            If aRun Then
              Dim lParameters() As Object = {pDataManager, Me}
              lMethod.Invoke(Nothing, lParameters)
            End If
          End If
        Next
      Next
    Catch ex As Exception
      Logger.Msg("Exception:" & ex.ToString, "clsPlugIn:BuiltInScript")
    End Try
  End Sub

  Private Function LaunchDisplay(ByVal aToolName As String, Optional ByVal aCmdLine As String = "") As Boolean
    Dim searchForName As String = aToolName.ToLower
    Dim ColonPos As Integer = searchForName.LastIndexOf(":")
    If ColonPos > 0 Then
      searchForName = searchForName.Substring(ColonPos + 1)
    End If
    Dim DisplayPlugins As ICollection = pDataManager.GetPlugins(GetType(atcDataDisplay))
    For Each lDisp As atcDataDisplay In DisplayPlugins
      Dim foundName As String = lDisp.Name.ToLower
      ColonPos = foundName.LastIndexOf(":")
      If ColonPos > 0 Then
        foundName = foundName.Substring(ColonPos + 1)
      End If
      If foundName = searchForName Then
        Dim typ As System.Type = lDisp.GetType()
        Dim asm As System.Reflection.Assembly = System.Reflection.Assembly.GetAssembly(typ)
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
  End Sub

  Public Sub LegendDoubleClick(ByVal Handle As Integer, ByVal Location As MapWindow.Interfaces.ClickLocation, ByRef Handled As Boolean) Implements MapWindow.Interfaces.IPlugin.LegendDoubleClick
    'This event fires when a user double-clicks a layer in the legend.
  End Sub

  Public Sub LegendMouseDown(ByVal Handle As Integer, ByVal Button As Integer, ByVal Location As MapWindow.Interfaces.ClickLocation, ByRef Handled As Boolean) Implements MapWindow.Interfaces.IPlugin.LegendMouseDown
    'This event fires when a user holds a mouse button down in the legend.
  End Sub

  Public Sub LegendMouseUp(ByVal Handle As Integer, ByVal Button As Integer, ByVal Location As MapWindow.Interfaces.ClickLocation, ByRef Handled As Boolean) Implements MapWindow.Interfaces.IPlugin.LegendMouseUp
    'This event fires when a user releases a mouse button in the legend.
  End Sub

  Public Sub MapDragFinished(ByVal Bounds As System.Drawing.Rectangle, ByRef Handled As Boolean) Implements MapWindow.Interfaces.IPlugin.MapDragFinished
    'If a user drags (ie draws a box) with the mouse on the map, this event fires at completion of the drag
    'and returns a system.drawing.rectangle that has the bounds of the box that was "drawn"
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
      Logger.Dbg("RefreshToolsMenu:" & msg)
      If msg.StartsWith("atcDataPlugin unloading") Then
        g_MapWin.Menus.Remove(ToolsMenuName)
      End If
      RefreshToolsMenu()
      'ElseIf msg.StartsWith("COMMAND_LINE:broadcast:basins") Then
      'COMMAND_LINE:broadcast:basins:script:c:\test\BASINS4\scripts\dummy.vb
      'Logger.Dbg("BASINS:Message:" & msg)
      'Dim s As String = msg.Substring(23)
      'If s.Substring(7).StartsWith("script") Then
      '  lScriptFileName = s.Substring(14)
      '  ChDriveDir(PathNameOnly(lScriptFileName)) 'start where script is
      '  RunBasinsScript(FileExt(lScriptFileName), lScriptFileName, lErrors, "dataManager", "basinsplugin")
      '  If Not lErrors Is Nothing AndAlso lErrors.Length > 0 Then
      '    Logger.Msg(lErrors, "Command Line Script Error", "OK")
      '  End If
      '  pCommandLineScript = True
      'End If
      'ElseIf msg.StartsWith("RUN_BASINS_SCRIPT:") Then
      '  lScriptFileName = msg.Substring(18).Trim
      '  RunBasinsScript(FileExt(lScriptFileName), lScriptFileName, lErrors, "dataManager", "basinsplugin")
      '  If Not lErrors Is Nothing AndAlso lErrors.Length > 0 Then
      '    Logger.Msg(lErrors, "Script Error")
      '  End If
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

  Public Sub ShapesSelected(ByVal Handle As Integer, ByVal SelectInfo As MapWindow.Interfaces.SelectInfo) Implements MapWindow.Interfaces.IPlugin.ShapesSelected
    'This event fires when the user selects one or more shapes using the select tool in MapWindow. Handle is the 
    'Layer handle for the shapefile on which shapes were selected. SelectInfo holds information abou the 
    'shapes that were selected. 
  End Sub

  Private Sub FindBasinsDrives()
    Dim i As Integer
    Dim drv As String
    If g_BasinsDrives.Length = 0 Then
      Dim allDrives As String() = Environment.GetLogicalDrives
      For i = 0 To allDrives.Length - 1
        drv = UCase(allDrives(i).Chars(0))
        If (Asc(drv) > Asc("B")) Then
          If FileExists(drv & ":" & BasinsDataPath, True, False) Then
            g_BasinsDrives = g_BasinsDrives & drv
          End If
        End If
      Next
      If g_BasinsDrives.Length = 0 Then
        Logger.Msg("No BASINS folders found on any drives on this computer", "FindBasinsDrives")
      Else
        Logger.Dbg("Found BasinsDrives: " & g_BasinsDrives)
      End If
    End If
  End Sub

  Private Function GetSelected(ByVal iField As Integer) As ArrayList
    Dim iSelected As Integer
    Dim iShape As Integer
    Dim sf As MapWinGIS.Shapefile = g_MapWin.Layers.Item(g_MapWin.Layers.CurrentLayer).GetObject
    Dim retval As New ArrayList(g_MapWin.View.SelectedShapes().NumSelected)
    For iSelected = 0 To g_MapWin.View.SelectedShapes.NumSelected - 1
      iShape = g_MapWin.View.SelectedShapes.Item(iSelected).ShapeIndex()
      retval.Add(sf.CellValue(iField, iShape))
    Next
    Return retval
  End Function

  Private Sub pDataManager_OpenedData(ByVal aDataSource As atcData.atcDataSource) Handles pDataManager.OpenedData
    RefreshSaveDataMenu()
  End Sub

  Private Sub RefreshSaveDataMenu()
    g_MapWin.Menus.Remove(SaveDataMenuName)
    AddMenuIfMissing(SaveDataMenuName, "mnuFile", SaveDataMenuString, "mnuSaveAs")
    For Each lDataSource As atcDataSource In pDataManager.DataSources
      If lDataSource.CanSave Then
        AddMenuIfMissing(SaveDataMenuName & "_" & lDataSource.Specification, SaveDataMenuName, lDataSource.Specification)
      End If
    Next
  End Sub

End Class
