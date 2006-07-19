Imports System.Collections.Specialized
Imports System.Reflection
Imports atcUtility
Imports atcData
Imports MapWinUtility

Friend Module modBasinsPlugin
    'Declare this as global so that it can be accessed throughout the plug-in project.
    'These variables are initialized in the plugin_Initialize event.
    Public g_MapWin As MapWindow.Interfaces.IMapWin
    Public g_MapWinWindowHandle As Integer
    Public g_AppName As String = "BASINS4"
    Public g_BasinsDrives As String = ""
    Public g_BasinsDir As String = ""
    Public pBuildFrm As frmBuildNew

    Friend pCommandLineScript As Boolean = False
    Friend pBuiltInScriptExists As Boolean = False

    'Private Const NewProjectMenuName As String = "BasinsNewProject"
    'Private Const NewProjectMenuString As String = "&New Project"

    'File menu -- created by MapWindow
    Friend Const FileMenuName As String = "mnuFile"

    Friend Const ProjectsMenuName As String = "BasinsProjects"
    Friend Const ProjectsMenuString As String = "Open &BASINS Project"

    Friend Const NewDataMenuName As String = "BasinsNewData"
    Friend Const NewDataMenuString As String = "New Data"

    Friend Const OpenDataMenuName As String = "BasinsOpenData"
    Friend Const OpenDataMenuString As String = "Open Data"

    Friend Const DownloadMenuName As String = "BasinsDownloadData"
    Friend Const DownloadMenuString As String = "&Download Data"

    Friend Const ManageDataMenuName As String = "BasinsManageData"
    Friend Const ManageDataMenuString As String = "&Manage Data"

    Friend Const SaveDataMenuName As String = "BasinsSaveData"
    Friend Const SaveDataMenuString As String = "Save Data In..."

    Friend Const AnalysisMenuName As String = "BasinsAnalysis"
    Friend Const AnalysisMenuString As String = "&Analysis"

    Friend Const ModelsMenuName As String = "BasinsModels"
    Friend Const ModelsMenuString As String = "&Models"

    Friend Const ComputeMenuName As String = "BasinsCompute"
    Friend Const ComputeMenuString As String = "&Compute"

    Friend pWelcomeScreenShow As Boolean = False

    Friend Const RegisterMenuName As String = "RegisterBASINS"
    Friend Const RegisterMenuString As String = "&Register as a BASINS user"

    Friend Const CheckForUpdatesMenuName As String = "CheckForUpdates"
    Friend Const CheckForUpdatesMenuString As String = "&Check For Updates"

    Friend Const BasinsHelpMenuName As String = "BasinsHelp"
    Friend Const BasinsHelpMenuString As String = "BASINS Documentation"

    Friend Const BasinsWebPageMenuName As String = "BasinsWebPage"
    Friend Const BasinsWebPageMenuString As String = "BASINS Web Page"

    Friend Const SendFeedbackMenuName As String = "SendFeedback"
    Friend Const SendFeedbackMenuString As String = "Send &Feedback"

    Friend Const DataMenuName As String = "BasinsData"
    Friend Const DataMenuString As String = "&Data"
    Friend pLoadedDataMenu As Boolean = False

    Private Const BasinsDataPath As String = "\Basins\data\"
    Private Const NationalProjectFilename As String = "national.mwprj"

    Friend WithEvents pDataManager As atcDataManager

    Private Sub pDataManager_OpenedData(ByVal aDataSource As atcData.atcDataSource) Handles pDataManager.OpenedData
        RefreshSaveDataMenu()
    End Sub

    Friend Sub RefreshSaveDataMenu()
        g_MapWin.Menus.Remove(SaveDataMenuName)
        AddMenuIfMissing(SaveDataMenuName, FileMenuName, SaveDataMenuString, "mnuSaveAs")
        For Each lDataSource As atcDataSource In pDataManager.DataSources
            If lDataSource.CanSave Then
                AddMenuIfMissing(SaveDataMenuName & "_" & lDataSource.Specification, SaveDataMenuName, lDataSource.Specification)
            End If
        Next
    End Sub

    Friend Sub FindBasinsDrives()
        If g_BasinsDrives.Length = 0 Then
            Dim lAllDrives As String() = Environment.GetLogicalDrives
            For i As Integer = 0 To lAllDrives.Length - 1
                Dim lDrv As String = UCase(lAllDrives(i).Chars(0))
                If (Asc(lDrv) > Asc("B")) Then
                    If FileExists(lDrv & ":" & BasinsDataPath, True, False) Then
                        g_BasinsDrives = g_BasinsDrives & lDrv
                    End If
                End If
            Next
            Select Case g_BasinsDrives.Length
                Case 0 : Logger.Msg("No BASINS folders found on any drives on this computer", "FindBasinsDrives")
                Case 1 : Logger.Dbg("Found BASINS Drive: " & g_BasinsDrives)
                Case Is > 1 : Logger.Dbg("Found BASINS Drives: " & g_BasinsDrives)
            End Select
        End If
    End Sub

    Friend Sub LoadNationalProject()
        If Not NationalProjectIsOpen() Then
            Dim lDrive As Integer = 0

            Dim lFileName As String = g_BasinsDir & "\Data\national\" & NationalProjectFilename
            While Not FileExists(lFileName) AndAlso lDrive < g_BasinsDrives.Length
                lFileName = UCase(g_BasinsDrives.Chars(lDrive)) & ":" _
                          & "\BASINS\Data\national\" & NationalProjectFilename
                'found existing national project, save name for later loading
            End While

            If FileExists(lFileName) Then  'load national project
                g_MapWin.Project.Load(lFileName)
            Else
                Logger.Msg("Unable to find '" & NationalProjectFilename & "' on drives: " & g_BasinsDrives, "Open National")
                Exit Sub
            End If
        End If

        If NationalProjectIsOpen() Then
            'Select the Cataloging Units layer by default 
            For iLayer As Integer = 0 To g_MapWin.Layers.NumLayers - 1
                If g_MapWin.Layers(iLayer).Name = "Cataloging Units" Then
                    g_MapWin.Layers.CurrentLayer = iLayer
                    Exit For
                End If
            Next
            pBuildFrm = New frmBuildNew
            pBuildFrm.Show()
            pBuildFrm.Top = GetSetting("BASINS4", "Window Positions", "BuildTop", "300")
            pBuildFrm.Left = GetSetting("BASINS4", "Window Positions", "BuildLeft", "0")
            UpdateSelectedFeatures()
        Else
            Logger.Msg("Unable to open national project on drives: " & g_BasinsDrives, "Open National")
        End If
    End Sub

    Friend Function NationalProjectIsOpen() As Boolean
        If (Not g_MapWin.Project Is Nothing) _
            AndAlso (Not g_MapWin.Project.FileName Is Nothing) _
            AndAlso g_MapWin.Project.FileName.ToLower.EndsWith(NationalProjectFilename) Then
            Return True
        Else
            Return False
        End If
    End Function

    Friend Sub SpecifyAndCreateNewProject()
        pBuildFrm = Nothing

        Dim lThemeTag As String = ""
        Dim lFieldName As String = ""
        Dim lField As Integer
        Dim lFieldMatch As Integer = -1
        Dim lCurLayer As MapWinGIS.Shapefile
        lCurLayer = g_MapWin.Layers.Item(g_MapWin.Layers.CurrentLayer).GetObject

        Select Case FilenameOnly(lCurLayer.Filename).ToLower
            Case "cat", "huc", "huc250d3"
                lThemeTag = "huc_cd"
                lFieldName = "CU"
            Case "cnty"
                lThemeTag = "county_cd"
                lFieldName = "FIPS"
            Case "st"
                lThemeTag = "state_abbrev"
                lFieldName = "ST"
            Case Else
                Logger.Msg("Unknown layer for selection, using lFirst field", "Area Selection")
                lThemeTag = "huc_cd"
                lFieldMatch = 1
        End Select

        lFieldName = lFieldName.ToLower
        For lField = 0 To lCurLayer.NumFields - 1
            If lCurLayer.Field(lField).Name.ToLower = lFieldName Then
                lFieldMatch = lField
            End If
        Next

        If lFieldMatch >= 0 Then
            'Save national project as the user has zoomed it
            g_MapWin.Project.Save(g_MapWin.Project.FileName)
            CreateNewProjectAndDownloadCoreDataInteractive(lThemeTag, GetSelected(lFieldMatch))
        Else
            Logger.Msg("Could not find field " & lFieldName & " in " & lCurLayer.Filename, "Could not create project")
        End If
    End Sub

    Private Function GetSelected(ByVal aField As Integer) As ArrayList
        Dim lSelected As Integer
        Dim lShape As Integer
        Dim lSf As MapWinGIS.Shapefile = g_MapWin.Layers.Item(g_MapWin.Layers.CurrentLayer).GetObject
        Dim lRetval As New ArrayList(g_MapWin.View.SelectedShapes().NumSelected)
        For lSelected = 0 To g_MapWin.View.SelectedShapes.NumSelected - 1
            lShape = g_MapWin.View.SelectedShapes.Item(lSelected).ShapeIndex()
            lRetval.Add(lSf.CellValue(aField, lShape))
        Next
        Return lRetval
    End Function

    Friend Function AddMenuIfMissing(ByVal aMenuName As String, _
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

    Friend Sub RefreshAnalysisMenu(Optional ByVal aIgnore As String = "")
        'Dim lScriptMenuName As String = AnalysisMenuName & "_Scripting"
        'Dim iPlugin As Integer
        If pLoadedDataMenu Then
            AddMenuIfMissing(AnalysisMenuName, "", AnalysisMenuString, FileMenuName)
            'AddMenuIfMissing(AnalysisMenuName & "_TestDBF", AnalysisMenuName, "Test DBF")
            AddMenuIfMissing(AnalysisMenuName & "_ArcView3", AnalysisMenuName, "ArcView &3")
            AddMenuIfMissing(AnalysisMenuName & "_ArcGIS", AnalysisMenuName, "&ArcGIS")
            AddMenuIfMissing(AnalysisMenuName & "_GenScn", AnalysisMenuName, "&GenScn")
            AddMenuIfMissing(AnalysisMenuName & "_WDMUtil", AnalysisMenuName, "&WDMUtil")
            'AddMenuIfMissing(lScriptMenuName, AnalysisMenuName, "Scripting")
            'If pBuiltInScriptExists Then
            '  AddMenuIfMissing(AnalysisMenuName & "_RunBuiltInScript", lScriptMenuName, "Run Built In Script")
            'End If
            'AddMenuIfMissing(AnalysisMenuName & "_ScriptEditor", lScriptMenuName, "Script Editor")
            'AddMenuIfMissing(AnalysisMenuName & "_RunScript", lScriptMenuName, "Select Script to Run")

            'For Each lScriptFilename As String In IO.Directory.GetFiles(ScriptFolder, "*.vb")
            '  Dim lMenuLabel As String = FilenameOnly(lScriptFilename)
            '  If Not lMenuLabel.ToLower.StartsWith("sub") AndAlso Not lMenuLabel.ToLower.StartsWith("fun") Then
            '    AddMenuIfMissing(AnalysisMenuName & "_RunScript" & FilenameNoPath(lScriptFilename), lScriptMenuName, "Run " & lMenuLabel)
            '  End If
            'Next

            'AddMenuIfMissing(AnalysisMenuName & "_ChangeProjection", AnalysisMenuName, "Change &Projection")

            Dim lPlugins As ICollection = pDataManager.GetPlugins(GetType(atcDataDisplay))
            If lPlugins.Count > 0 Then
                AddMenuIfMissing(AnalysisMenuName & "_Separator1", AnalysisMenuName, "-")
                For Each lDisp As atcDataDisplay In lPlugins
                    Dim lMenuText As String = lDisp.Name
                    If Not lMenuText.Equals(aIgnore) AndAlso lMenuText.StartsWith("Analysis::") Then
                        AddMenuIfMissing(AnalysisMenuName & "_" & lDisp.Name, AnalysisMenuName, lMenuText.Substring(10))
                    End If
                Next
            End If
        End If
    End Sub

    'Friend Sub RefreshDataMenu()
    '  AddMenuIfMissing(DataMenuName, "", DataMenuString, FileMenuName)
    '  pLoadedDataMenu = True

    '  'Dim mnu As MapWindow.Interfaces.MenuItem
    '  'Dim iPlugin As Integer
    '  'With g_MapWin.Plugins
    '  '  For iPlugin = 0 To .Count - 1
    '  '    If Not .Item(iPlugin) Is Nothing Then
    '  '      Dim pluginName As String = .Item(iPlugin).Name
    '  '      If CType(.Item(iPlugin), Object).GetType().IsSubclassOf(GetType(atcDataPlugin)) Then
    '  '        mnu = AddMenuIfMissing(DataMenuName & "_" & pluginName, DataMenuName, pluginName)
    '  '      End If
    '  '    End If
    '  '  Next
    '  'End With
    'End Sub

    'Friend Sub BuiltInScript(ByVal aRun As Boolean)
    '  Try
    '    Dim lFlags As BindingFlags = BindingFlags.NonPublic Or BindingFlags.Public Or _
    '                                 BindingFlags.Static Or BindingFlags.Instance Or _
    '                                 BindingFlags.DeclaredOnly
    '    Dim lAssembly As [Assembly] = [Assembly].Load("atcScriptTest")
    '    Dim lTypes As Type() = lAssembly.GetTypes
    '    For Each lType As Type In lTypes
    '      Dim lMethods As MethodInfo() = lType.GetMethods(lFlags)
    '      For Each lMethod As MethodInfo In lMethods
    '        If lMethod.Name.ToLower = "main" AndAlso _
    '           lMethod.GetParameters.Length = 2 AndAlso _
    '           (lMethod.GetParameters(0).ParameterType Is pDataManager.GetType _
    '            OrElse lMethod.GetParameters(0).ParameterType.Name = "Object") AndAlso _
    '           (lMethod.GetParameters(1).ParameterType Is Me.GetType _
    '            OrElse lMethod.GetParameters(1).ParameterType.Name = "Object") Then 'found built in script

    '          pBuiltInScriptExists = True
    '          If aRun Then
    '            Dim lParameters() As Object = {pDataManager, Me}
    '            lMethod.Invoke(Nothing, lParameters)
    '          End If
    '        End If
    '      Next
    '    Next
    '  Catch ex As Exception
    '    Logger.Msg("Exception:" & ex.ToString, "clsPlugIn:BuiltInScript")
    '  End Try
    'End Sub

    Friend Sub UpdateSelectedFeatures()
        If Not pBuildFrm Is Nothing AndAlso g_MapWin.Layers.NumLayers > 0 AndAlso g_MapWin.Layers.CurrentLayer > -1 Then
            Dim lFieldName As String = ""
            Dim lFieldDesc As String = ""
            Dim lField As Integer
            Dim lNameIndex As Integer = -1
            Dim lDescIndex As Integer = -1
            Dim lCurLayer As MapWinGIS.Shapefile
            Dim ctext As String

            g_MapWin.Refresh()
            ctext = "Selected Features:" & vbCrLf & "  <none>"
            lCurLayer = g_MapWin.Layers.Item(g_MapWin.Layers.CurrentLayer).GetObject

            If g_MapWin.View.SelectedShapes.NumSelected > 0 Then
                ctext = "Selected Features:"
                Select Case FilenameOnly(lCurLayer.Filename).ToLower
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
    End Sub

    Friend Function FeedbackSystemInformation() As String
        'TODO: format as an html document?
        Dim lFeedback As String = "Feedback at " & Now.ToString("u") & vbCrLf
        lFeedback &= "Project: " & g_MapWin.Project.FileName & vbCrLf
        lFeedback &= "Config: " & g_MapWin.Project.ConfigFileName & vbCrLf
        lFeedback &= "CommandLine: " & Environment.CommandLine & vbCrLf
        lFeedback &= "User: " & Environment.UserName & vbCrLf
        lFeedback &= "Machine: " & Environment.MachineName & vbCrLf
        lFeedback &= "OSVersion: " & Environment.OSVersion.ToString & vbCrLf
        lFeedback &= "CLRVersion: " & Environment.Version.ToString & vbCrLf

        lFeedback &= "LogFile: " & Logger.FileName & vbCrLf
        'TODO: add current log file contents (not too much!!!)

        'plugin info
        lFeedback &= vbCrLf & "Plugins loaded:" & vbCrLf
        Dim lLastPlugIn As Integer = g_MapWin.Plugins.Count() - 1
        For iPlugin As Integer = 0 To lLastPlugIn
            Dim lCurPlugin As MapWindow.Interfaces.IPlugin = g_MapWin.Plugins.Item(iPlugin)
            If Not lCurPlugin Is Nothing Then
                With lCurPlugin
                    lFeedback &= .Name & vbTab & .Version & vbTab & .BuildDate & vbCrLf
                End With
            End If
        Next

        'TODO: add map layers info?

        Dim lSkipFilename As Integer = g_BasinsDir.Length
        lFeedback &= vbCrLf & "Files in " & g_BasinsDir & vbCrLf

        Dim lallFiles As New NameValueCollection
        AddFilesInDir(lallFiles, g_BasinsDir, True)
        'lFeedback &= vbCrLf & "Modified" & vbTab & "Size" & vbTab & "Filename" & vbCrLf
        For Each lFilename As String In lallFiles
            lFeedback &= FileDateTime(lFilename).ToString("yyyy-MM-dd HH:mm:ss") & vbTab & StrPad(Format(FileLen(lFilename), "#,###"), 10) & vbTab & lFilename.Substring(lSkipFilename) & vbCrLf
        Next

        Return lFeedback
    End Function

End Module