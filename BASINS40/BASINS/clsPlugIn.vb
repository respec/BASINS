Imports System.Collections.Specialized
Imports System.Windows.Forms.Application
Imports atcGraph
Imports atcUtility
Imports atcData

Public Class PlugIn
  Implements MapWindow.Interfaces.IPlugin

  Private Const NationalProjectFilename = "national.mwprj"

  'Private Const NewProjectMenuName As String = "BasinsNewProject"
  'Private Const NewProjectMenuString As String = "&New Project"

  Private Const ProjectsMenuName As String = "BasinsProjects"
  Private Const ProjectsMenuString As String = "Open &BASINS Project"

  Private Const DataMenuName As String = "BasinsData"
  Private Const DataMenuString As String = "&Data"

  Private Const ToolsMenuName As String = "BasinsTools"
  Private Const ToolsMenuString As String = "&Tools"

  Private Const ModelsMenuName As String = "BasinsModels"
  Private Const ModelsMenuString As String = "&Models"

  Private Const BasinsProjectPath As String = "\Basins\apr\"
  Private Const BasinsDataPath As String = "\Basins\data\"

  Private pNationalDataDir As String = ""
  Private pLogFilename As String = ""
  Private WithEvents pDataManager As atcDataManager

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

    pDataManager = New atcDataManager(g_MapWin, Me)

    FindBasinsDrives()

    'g_MapWin.Menus.AddMenu(NewProjectMenuName, "mnuFile", Nothing, NewProjectMenuString, "mnuNew")
    'g_MapWin.Menus.Remove("New")
    g_MapWin.Menus.AddMenu(ProjectsMenuName, "mnuFile", Nothing, ProjectsMenuString, "mnuRecentProjects")

    For iDrive = 0 To g_BasinsDrives.Length - 1
      DriveLetter = g_BasinsDrives.Substring(iDrive, 1)
      'Scan folder for project data, and populate menu
      Dim DataDirs() As String = System.IO.Directory.GetDirectories( _
                                        DriveLetter & ":\BASINS\data")
      For iDirectory = 0 To DataDirs.GetUpperBound(0)
        Dim DirShortName As String = System.IO.Path.GetFileName(DataDirs(iDirectory))
        If g_BasinsDrives.Length > 0 Then DirShortName = DriveLetter & ": " & DirShortName
        mnu = g_MapWin.Menus.AddMenu(ProjectsMenuName & "_" & DirShortName, _
                                     ProjectsMenuName, Nothing, DataDirs(iDirectory))
        mnu.Tooltip = DataDirs(iDirectory)
      Next
    Next

    RefreshDataMenu()

    RefreshToolsMenu()

    g_MapWin.Menus.AddMenu(ModelsMenuName, "", Nothing, ModelsMenuString, ToolsMenuName)
    'mnu = g_MapWin.Menus.AddMenu(ModelsMenuName & "_HSPF", ModelsMenuName, Nothing, "&HSPF")
    'mnu.Tooltip = "Hydrological Simulation Program - Fortran"
    mnu = g_MapWin.Menus.AddMenu(ModelsMenuName & "_SWAT", ModelsMenuName, Nothing, "&SWAT")
    mnu.Tooltip = "SWAT"
    mnu = g_MapWin.Menus.AddMenu(ModelsMenuName & "_PLOAD", ModelsMenuName, Nothing, "&PLOAD")
    mnu.Tooltip = "PLOAD"
    mnu = g_MapWin.Menus.AddMenu(ModelsMenuName & "_AGWA", ModelsMenuName, Nothing, "&AGWA")
    mnu.Tooltip = "AGWA"
    mnu = g_MapWin.Menus.AddMenu(ModelsMenuName & "_AQUATOX", ModelsMenuName, Nothing, "AQUA&TOX")
    mnu.Tooltip = "AQUATOX"

    'load HSPF plugin (an integral part of BASINS)
    'g_MapWin.Plugins.StartPlugin("atcHSPF_PlugIn")

  End Sub

  Private Sub RefreshToolsMenu()
    Dim mnu As MapWindow.Interfaces.MenuItem
    Dim iPlugin As Integer
    With g_MapWin.Menus
      .Remove(ToolsMenuString)
      .AddMenu(ToolsMenuName, "", Nothing, ToolsMenuString, DataMenuName)

      'mnu = .AddMenu(ToolsMenuName & "_TestDBF", ToolsMenuName, Nothing, "Test DBF")
      mnu = .AddMenu(ToolsMenuName & "_ArcView3", ToolsMenuName, Nothing, "ArcView &3")
      mnu = .AddMenu(ToolsMenuName & "_ArcGIS", ToolsMenuName, Nothing, "&ArcGIS")
      mnu = .AddMenu(ToolsMenuName & "_GenScn", ToolsMenuName, Nothing, "&GenScn")
      mnu = .AddMenu(ToolsMenuName & "_WDMUtil", ToolsMenuName, Nothing, "&WDMUtil")
      mnu = .AddMenu(ToolsMenuName & "_RunScript", ToolsMenuName, Nothing, "Run Script")
      mnu = .AddMenu(ToolsMenuName & "_OpenScript", ToolsMenuName, Nothing, "Edit Script")
      'mnu = .AddMenu(ToolsMenuName & "_ChangeProjection", ToolsMenuName, Nothing, "Change &Projection")

      Dim DisplayPlugins As ICollection = pDataManager.GetPlugins(GetType(atcDataDisplay))
      If DisplayPlugins.Count > 0 Then
        mnu = .AddMenu(ToolsMenuName & "_Separator1", ToolsMenuName, Nothing, "-")
      End If
      For Each lDisp As atcDataDisplay In DisplayPlugins
        mnu = .AddMenu(ToolsMenuName & "_" & lDisp.Name, ToolsMenuName, Nothing, lDisp.Name)
      Next
    End With
  End Sub

  Private Sub RefreshDataMenu()
    Dim mnu As MapWindow.Interfaces.MenuItem
    Dim iPlugin As Integer
    With g_MapWin.Menus
      .Remove(DataMenuString)
      .AddMenu(DataMenuName, "", Nothing, DataMenuString, "mnuFile")
      mnu = .AddMenu(DataMenuName & "_Download", DataMenuName, Nothing, "&Download")
      mnu = .AddMenu(DataMenuName & "_AddData", DataMenuName, Nothing, "&Add Data")
      mnu = .AddMenu(DataMenuName & "_ManageDataSources", DataMenuName, Nothing, "&Manage Sources")

      'With g_MapWin.Plugins
      '  For iPlugin = 0 To .Count - 1
      '    If Not .Item(iPlugin) Is Nothing Then
      '      Dim pluginName As String = .Item(iPlugin).Name
      '      If CType(.Item(iPlugin), Object).GetType().IsSubclassOf(GetType(atcDataPlugin)) Then
      '        mnu = g_MapWin.Menus.AddMenu(DataMenuName & "_" & pluginName, DataMenuName, Nothing, pluginName)
      '      End If
      '    End If
      '  Next
      'End With
    End With
  End Sub

  Public Sub Terminate() Implements MapWindow.Interfaces.IPlugin.Terminate
    'This event is fired when the user unloads your plug-in either through the plug-in dialog 
    'box, or by un-checkmarking it in the plug-ins menu.  This is where you would remove any
    'buttons from the tool bar tool bar or menu items from the menu that you may have added.
    'If you don't do this, then you will leave dangling menus and buttons that don't do anything.

    g_MapWin.Menus.Remove(DataMenuName & "_Download")
    g_MapWin.Menus.Remove(DataMenuName & "_AddData")
    g_MapWin.Menus.Remove(DataMenuName & "_ManageDataSources")
    g_MapWin.Menus.Remove(DataMenuName)

    'TODO: remove DisplayPlugins menu items
    g_MapWin.Menus.Remove(ToolsMenuName & "_Separator1")
    'g_MapWin.Menus.Remove(ToolsMenuName & "_TestDBF")
    g_MapWin.Menus.Remove(ToolsMenuName & "_ArcView3")
    g_MapWin.Menus.Remove(ToolsMenuName & "_ArcGIS")
    g_MapWin.Menus.Remove(ToolsMenuName & "_GenScn")
    g_MapWin.Menus.Remove(ToolsMenuName & "_WDMUtil")
    g_MapWin.Menus.Remove(ToolsMenuName & "_RunScript")
    g_MapWin.Menus.Remove(ToolsMenuName & "_OpenScript")
    g_MapWin.Menus.Remove(ToolsMenuName)

    g_MapWin.Menus.Remove(ModelsMenuName & "_SWAT")
    g_MapWin.Menus.Remove(ModelsMenuName & "_PLOAD")
    g_MapWin.Menus.Remove(ModelsMenuName & "_AGWA")
    g_MapWin.Menus.Remove(ModelsMenuName & "_AQUATOX")
    g_MapWin.Menus.Remove(ModelsMenuName) 'TODO: don't unload if another plugin is still using it

    g_MapWin.Menus.Remove(ProjectsMenuName)

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

    Debug.WriteLine("BASINS4:clsPlugIn:ItemClicked: " & ItemName)
    If ItemName.Equals("mnuNew") Then 'Override File/New menu item behavior
      Debug.WriteLine(g_MapWin.PreviewMap.BackColor)
      If NationalProjectIsOpen() Then
        MsgBox("Select the area of interest, " & vbCr & "then Download from the Data menu" & vbCr _
             & "to create a new BASINS project", , "New Project")
      Else
        Dim allFiles As NameValueCollection
        Dim iDrive As Integer
        Dim sDrive As String
        allFiles = New NameValueCollection
        For iDrive = 0 To g_BasinsDrives.Length - 1
          sDrive = UCase(g_BasinsDrives.Chars(iDrive))
          AddFilesInDir(allFiles, sDrive & ":\BASINS\Data\national\", True, NationalProjectFilename)
        Next
        If allFiles.Count > 0 Then
          g_MapWin.Project.Load(allFiles.Item(0))
        Else
          LogMsg("Unable to find '" & NationalProjectFilename & "' on drives: " & g_BasinsDrives & " in folder \BASINS\Data\national\", "Open National")
        End If
      End If
      Handled = True
    ElseIf ItemName.StartsWith(ProjectsMenuName & "_") Then
      DataDirName = g_MapWin.Menus(ItemName).Text ' g_MapWin.Menus(ItemName).Tooltip
      If FileExists(DataDirName, True, False) Then
        PrjFileName = DataDirName & "\" & FilenameOnly(DataDirName) & ".mwprj"
        If FileExists(PrjFileName) Then
          g_MapWin.Project.Load(PrjFileName)
        Else
          'TODO: look for other *.mwprj before creating a new one?
          g_MapWin.Layers.Clear()
          g_MapWin.Refresh()
          g_MapWin.PreviewMap.GetPictureFromMap()
          DoEvents()
          AddAllShapesInDir(DataDirName, DataDirName)
          g_MapWin.Project.Save(PrjFileName)
          g_MapWin.Project.Modified = False
        End If
      Else
        MsgBox("Tool tip was not perserved - cannot open " & ItemName, , ProjectsMenuString)
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
                FieldName = "FIPS"
              Case Else
                LogMsg("Unknown layer for selection, using first field", "Area Selection")
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
              LogMsg("Could not find field " & FieldName & " in " & curLayer.Filename, "Could not create project")
            End If
          Else
            DownloadNewData(PathNameOnly(g_MapWin.Project.FileName) & "\")
          End If
        Case "AddData"
          'Dim pCategories As New ArrayList
          'Dim pGroup As New atcDataGroup
          pDataManager.UserOpenDataSource() 'pCategories, pGroup)
        Case "ManageDataSources"
          pDataManager.UserManage()
        Case Else : MsgBox("Data Tool " & ItemName)
      End Select
      Handled = True
    ElseIf ItemName.StartsWith(ToolsMenuName & "_") Then
      Handled = LaunchTool(ItemName.Substring(ToolsMenuName.Length + 1))
    ElseIf ItemName.StartsWith(ModelsMenuName & "_") Then
      Handled = LaunchTool(ItemName.Substring(ModelsMenuName.Length + 1))
    Else 'Not our item
      'MsgBox("Other button: " & ItemName)
    End If
  End Sub

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

  Private Sub pDataManager_OpenedData(ByVal aFile As atcData.atcDataSource) Handles pDataManager.OpenedData
    Dim s As String
    Dim i As Integer
    Dim lDebugFile As String = "c:\test\BASINS4\wdmFileDump.txt"
    Dim lAttributeDetailsShow As Boolean
    Dim lNvals As Integer
    Dim lDataset As atcTimeseries

    g_MapWin.StatusBar.ProgressBarValue = 5
    g_MapWin.StatusBar.ShowProgressBar = True

    g_MapWin.StatusBar.ProgressBarValue = 50

    s = aFile.Specification & " contains " & aFile.DataSets.Count() & " datasets" & vbCrLf & vbCrLf
    SaveFileString(lDebugFile, s)

    'lAttributeDetailsShow = True
    'For Each lDataset In aFile.Timeseries
    '  lNvals = lDataset.numValues
    '  s = "DSN " & lDataset.Attributes.GetValue("id") & " contains " & lNvals & " values" & vbCrLf
    '  For Each lAttribute As DictionaryEntry In lDataset.Attributes.GetAll
    '    s &= "  " & lAttribute.Key & " '" & lAttribute.Value & "'"
    '    If lAttributeDetailsShow Then
    '      s &= " (" & Trim(lDataset.Attributes.GetDefinition(lAttribute.Key).Description)
    '    End If
    '    s &= vbCrLf
    '  Next
    '  For i = 0 To 3
    '    s &= "  Value(" & i & ") = " & lDataset.Dates.Value(i) & " " & lDataset.Value(i) & vbCrLf
    '  Next
    '  AppendFileString(lDebugFile, s)
    'Next

    g_MapWin.StatusBar.ProgressBarValue = 75
    g_MapWin.StatusBar.ProgressBarValue = 100
    g_MapWin.StatusBar.ShowProgressBar = False
  End Sub

  'Private Sub TestDBF()
  '  Dim dbfname As String = "C:\test\atcUtility\test.dbf"
  '  Dim tmpDbf As IATCTable
  '  Dim i As Long, j As Long
  '  Dim baserow As Long

  '  'does this dbf already exist?
  '  If FileExists(dbfname) Then
  '    'delete this file first
  '    System.IO.File.Delete(dbfname)
  '  End If
  '  tmpDbf = atcUtility.TableOpener.OpenAnyTable(dbfname)
  '  tmpDbf.NumFields = 5
  '  tmpDbf.FieldName(1) = "Value"
  '  tmpDbf.FieldType(1) = "N"
  '  tmpDbf.FieldLength(1) = 10
  '  tmpDbf.FieldName(2) = "Landuse"
  '  tmpDbf.FieldType(2) = "C"
  '  tmpDbf.FieldLength(2) = 30
  '  tmpDbf.FieldName(3) = "Pervious"
  '  tmpDbf.FieldType(3) = "C"
  '  tmpDbf.FieldLength(3) = 10
  '  tmpDbf.FieldName(4) = "Multiplier"
  '  tmpDbf.FieldType(4) = "C"
  '  tmpDbf.FieldLength(4) = 10
  '  tmpDbf.FieldName(5) = "Subbasin"
  '  tmpDbf.FieldType(5) = "C"
  '  tmpDbf.FieldLength(5) = 10

  '  For i = 1 To 10
  '    tmpDbf.CurrentRecord = i
  '    tmpDbf.Value(1) = i & ",1"
  '    tmpDbf.Value(2) = i & ",2"
  '    tmpDbf.Value(3) = i & ",3"
  '    tmpDbf.Value(4) = i & ",4"
  '    tmpDbf.Value(5) = i & ",5"
  '  Next i
  '  Debug.WriteLine("Before write: " & tmpDbf.Summary)
  '  tmpDbf.WriteFile(dbfname)
  '  Debug.WriteLine("After write: " & tmpDbf.Summary)
  'End Sub

  Private Function LaunchTool(ByVal ToolName As String, Optional ByVal cmdLine As String = "") As Boolean
    Dim exename As String
    Select Case ToolName
      'Case "TestDBF"
      '  TestDBF()
    Case "GenScn" : exename = FindFile("Please locate GenScn.exe", "\BASINS\models\HSPF\bin\GenScn.exe")
      Case "WDMUtil" : exename = FindFile("Please locate WDMUtil.exe", "\BASINS\models\HSPF\WDMUtil\WDMUtil.exe")
      Case "HSPF"
        'If g_MapWin.Plugins.PluginIsLoaded("atcModelSetup_PlugIn") Then 'defer to other plugin
        Return False
        'End If
        'exename = FindFile("Please locate WinHSPF.exe", "\BASINS\models\HSPF\bin\WinHSPF.exe")
      Case "RunScript"
        'atcScriptTest.Main(pDataManager, Me)
        'Exit Function
        exename = FindFile("Please locate script to run", "", "vb", "VB.net Files (*.vb)|*.vb|All files (*.*)|*.*", True)
        If FileExists(exename) Then
          Dim errors As String
          Dim args() As Object
          If cmdLine.Length > 0 Then
            args = cmdLine.Split(",")
          Else
            ReDim args(1)
            args(0) = "DataManager"
            args(1) = "BasinsPlugIn"
          End If
          RunBasinsScript(FileExt(exename), WholeFileString(exename), errors, args)
          If Not errors Is Nothing Then
            MsgBox(errors, MsgBoxStyle.Exclamation, "Script Error")
          End If
          Return True
        Else
          LogMsg("Unable to find script " & exename, "Launch")
          Return False
        End If
      Case "OpenScript"
        Dim lfrm As New frmScript
        lfrm.BasinsPlugin = Me
        lfrm.Show()
        Return True
      Case Else 'Search for plugin to launch
        Dim newDisplay As atcDataDisplay
        Dim DisplayPlugins As ICollection = pDataManager.GetPlugins(GetType(atcDataDisplay))
        For Each lDisp As atcDataDisplay In DisplayPlugins
          If lDisp.Name = ToolName Then
            Dim typ As System.Type = lDisp.GetType()
            Dim asm As System.Reflection.Assembly = System.Reflection.Assembly.GetAssembly(typ)
            newDisplay = asm.CreateInstance(typ.FullName)
            newDisplay.Show(pDataManager)
            Return True
          End If
        Next
    End Select

    If FileExists(exename) Then
      'Ensure space between exename and cmdline if needed
      If cmdLine.Length > 0 AndAlso Not cmdLine.StartsWith(" ") Then cmdLine = " " & cmdLine

      Shell("""" & exename & """" & cmdLine, AppWinStyle.NormalFocus, False)
      Return True
    Else
      LogMsg("Unable to launch " & ToolName, "Launch")
      Return False
    End If
  End Function

  Public Function RunBasinsScript(ByVal aLanguage As String, _
                                  ByVal aScript As String, _
                                  ByRef errors As String, _
                                  ByVal args() As Object) _
                                  As Object

    If Not args Is Nothing Then 'replace some text arguments with objects
      For iArg As Integer = 0 To args.GetUpperBound(0)
        If args(iArg).GetType Is GetType(String) Then
          Select Case args(iArg).ToLower
            Case "datamanager" : args(iArg) = pDataManager
            Case "basinsplugin" : args(iArg) = Me
            Case "mapwin" : args(iArg) = g_MapWin
          End Select
        End If
      Next
    End If

    RunScript(aLanguage, aScript, errors, args, MakeScriptName)

  End Function

  Private Function MakeScriptName() As String
    Dim tryName As String
    Dim iTry As Integer = 0

    Do
      tryName = g_MapWin.Plugins.PluginFolder & System.IO.Path.DirectorySeparatorChar _
                          & "RemoveMe-Script-" & iTry & ".dll"
      iTry += 1
    Loop While FileExists(tryName)
    Return tryName
  End Function


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
    'Plug-ins can communicate with eachother using Messages.  If a message is sent then this event fires.
    'If you know the message is "for you" then you can set Handled=True and then it will not be sent to any
    'other plug-ins.

    If msg.StartsWith("atcDataPlugin") Then
      Me.RefreshToolsMenu()
      'If msg.Substring(15).StartsWith("loading") Then

      'End If
    ElseIf msg.StartsWith("basins") Then
      If msg.Substring(7).StartsWith("script") Then
        Dim errors As String
        Dim args() As Object = {"dataManager", "basinsplugin"}
        RunBasinsScript("vb", WholeFileString(msg.Substring(14)), errors, args)
        If Not errors Is Nothing Then
          MsgBox(errors, MsgBoxStyle.Exclamation, "Script Error")
        End If
      End If
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
    Dim newXML As New Chilkat.Xml
    newXML.LoadXml(SettingsString)
    pDataManager.XML = newXML.FindChild("TimeseriesManager")
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
        LogMsg("No BASINS folders found on any drives on this computer", "FindBasinsDrives")
      Else
        LogDbg("Found BasinsDrives: " & g_BasinsDrives)
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
End Class
