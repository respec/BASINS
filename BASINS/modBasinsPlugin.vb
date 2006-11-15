Imports System.Collections.Specialized
Imports System.Reflection
Imports atcUtility
Imports atcData
Imports MapWinUtility

''' <summary>
''' 
''' </summary>
''' <remarks></remarks>
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
    'Private Const NewProjectMenuString As String = "New Project"

    'File menu -- created by MapWindow
    Friend Const FileMenuName As String = "mnuFile"

    Friend Const ProjectsMenuName As String = "BasinsProjects"
    Friend Const ProjectsMenuString As String = "Open BASINS Project"

    Friend Const NewDataMenuName As String = "BasinsNewData"
    Friend Const NewDataMenuString As String = "New Data"

    Friend Const OpenDataMenuName As String = "BasinsOpenData"
    Friend Const OpenDataMenuString As String = "Open Data"

    Friend Const DownloadMenuName As String = "BasinsDownloadData"
    Friend Const DownloadMenuString As String = "Download Data"

    Friend Const ManageDataMenuName As String = "BasinsManageData"
    Friend Const ManageDataMenuString As String = "Manage Data"

    Friend Const SaveDataMenuName As String = "BasinsSaveData"
    Friend Const SaveDataMenuString As String = "Save Data In..."

    Friend Const AnalysisMenuName As String = "BasinsAnalysis"
    Friend Const AnalysisMenuString As String = "Analysis"

    Friend Const ModelsMenuName As String = "BasinsModels"
    Friend Const ModelsMenuString As String = "Models"

    Friend Const ComputeMenuName As String = "BasinsCompute"
    Friend Const ComputeMenuString As String = "Compute"

    Friend pWelcomeScreenShow As Boolean = False

    Friend Const RegisterMenuName As String = "RegisterBASINS"
    Friend Const RegisterMenuString As String = "Register as a BASINS user"

    Friend Const CheckForUpdatesMenuName As String = "CheckForUpdates"
    Friend Const CheckForUpdatesMenuString As String = "Check For Updates"

    Friend Const HelpMenuName As String = "mnuHelp"
    Friend Const BasinsHelpMenuName As String = "BasinsHelp"
    Friend Const BasinsHelpMenuString As String = "BASINS Documentation"

    Friend Const BasinsWebPageMenuName As String = "BasinsWebPage"
    Friend Const BasinsWebPageMenuString As String = "BASINS Web Page"

    Friend Const SendFeedbackMenuName As String = "SendFeedback"
    Friend Const SendFeedbackMenuString As String = "Send Feedback"

    Friend Const DataMenuName As String = "BasinsData"
    Friend Const DataMenuString As String = "Data"
    Friend pLoadedDataMenu As Boolean = False

    Private Const BasinsDataPath As String = "\Basins\data\"
    Private Const NationalProjectFilename As String = "national.mwprj"

    Friend WithEvents pDataManager As atcDataManager

    Private Sub pDataManager_OpenedData(ByVal aDataSource As atcData.atcDataSource) Handles pDataManager.OpenedData
        RefreshSaveDataMenu()
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks></remarks>
    Friend Sub RefreshSaveDataMenu()
        g_MapWin.Menus.Remove(SaveDataMenuName)
        AddMenuIfMissing(SaveDataMenuName, FileMenuName, SaveDataMenuString, "mnuSaveAs")
        For Each lDataSource As atcDataSource In pDataManager.DataSources
            If lDataSource.CanSave Then
                AddMenuIfMissing(SaveDataMenuName & "_" & lDataSource.Specification, SaveDataMenuName, lDataSource.Specification)
            End If
        Next
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks></remarks>
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

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks></remarks>
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
            g_MapWin.Toolbar.PressToolbarButton("tbbSelect")
            pBuildFrm = New frmBuildNew
            pBuildFrm.Show()
            pBuildFrm.Top = GetSetting("BASINS4", "Window Positions", "BuildTop", "300")
            pBuildFrm.Left = GetSetting("BASINS4", "Window Positions", "BuildLeft", "0")
            UpdateSelectedFeatures()
        Else
            Logger.Msg("Unable to open national project on drives: " & g_BasinsDrives, "Open National")
        End If
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Friend Function NationalProjectIsOpen() As Boolean
        If (Not g_MapWin.Project Is Nothing) _
            AndAlso (Not g_MapWin.Project.FileName Is Nothing) _
            AndAlso g_MapWin.Project.FileName.ToLower.EndsWith(NationalProjectFilename) Then
            Return True
        Else
            Return False
        End If
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks></remarks>
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

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="aMenuName"></param>
    ''' <param name="aParent"></param>
    ''' <param name="aMenuText"></param>
    ''' <param name="aAfter"></param>
    ''' <param name="aBefore"></param>
    ''' <param name="aAlphabetical"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Friend Function AddMenuIfMissing(ByVal aMenuName As String, _
                                            ByVal aParent As String, _
                                            ByVal aMenuText As String, _
                                   Optional ByVal aAfter As String = "", _
                                   Optional ByVal aBefore As String = "", _
                                   Optional ByVal aAlphabetical As Boolean = False) _
                                   As MapWindow.Interfaces.MenuItem

        Dim lMenus As MapWindow.Interfaces.Menus = g_MapWin.Menus
        With lMenus
            Dim lMenu As MapWindow.Interfaces.MenuItem = .Item(aMenuName)
            If Not lMenu Is Nothing Then 'This item already exists
                Return lMenu
            ElseIf aAlphabetical And aParent.Length > 0 Then
                'Need parent to do alphabetical search for position
                Dim lParentMenu As MapWindow.Interfaces.MenuItem = .Item(aParent)
                Dim lSubmenuIndex As Integer = 0
                Dim lExistingMenu As MapWindow.Interfaces.MenuItem

                If aAfter.Length > 0 Then
                    'First make sure we are after a particular item
                    While lSubmenuIndex < lParentMenu.NumSubItems
                        lExistingMenu = lParentMenu.SubItem(lSubmenuIndex)
                        If Not lExistingMenu Is Nothing AndAlso _
                           Not lExistingMenu.Name Is Nothing AndAlso _
                               lExistingMenu.Name.Equals(aAfter) Then
                            Exit While
                        End If
                        lExistingMenu = Nothing
                        lSubmenuIndex += 1
                    End While
                    If lSubmenuIndex >= lParentMenu.NumSubItems Then
                        'Did not find menu aAfter, so start at first subitem
                        lSubmenuIndex = 0
                    End If
                End If

                'Find alphabetical position for new menu item
                While lSubmenuIndex < lParentMenu.NumSubItems
                    lExistingMenu = lParentMenu.SubItem(lSubmenuIndex)
                    If Not lExistingMenu Is Nothing AndAlso _
                       Not lExistingMenu.Name Is Nothing Then
                        If (aBefore.Length > 0 AndAlso lExistingMenu.Text = aBefore) OrElse _
                           lExistingMenu.Text > aMenuText Then
                            'Add before existing menu with alphabetically later text
                            Return .AddMenu(aMenuName, aParent, aMenuText, lExistingMenu.Name)
                        End If
                    End If
                    lSubmenuIndex += 1
                End While
                'Add at default position, after last parent subitem
                Return .AddMenu(aMenuName, aParent, Nothing, aMenuText)
            ElseIf aBefore.Length > 0 Then
                Return .AddMenu(aMenuName, aParent, aMenuText, aBefore)
            Else
                Return .AddMenu(aMenuName, aParent, Nothing, aMenuText, aAfter)
            End If
        End With
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="aIgnore"></param>
    ''' <remarks></remarks>
    Friend Sub RefreshAnalysisMenu(Optional ByVal aIgnore As String = "")
        If pLoadedDataMenu Then
            AddMenuIfMissing(AnalysisMenuName, "", AnalysisMenuString, FileMenuName)
            AddMenuIfMissing(AnalysisMenuName & "_ArcView3", AnalysisMenuName, "ArcView 3")
            AddMenuIfMissing(AnalysisMenuName & "_ArcGIS", AnalysisMenuName, "ArcGIS")
            AddMenuIfMissing(AnalysisMenuName & "_GenScn", AnalysisMenuName, "GenScn")
            AddMenuIfMissing(AnalysisMenuName & "_WDMUtil", AnalysisMenuName, "WDMUtil")

            Dim lPlugins As ICollection = pDataManager.GetPlugins(GetType(atcDataDisplay))
            If lPlugins.Count > 0 Then
                Dim lSeparatorName As String = AnalysisMenuName & "_Separator1"
                AddMenuIfMissing(lSeparatorName, AnalysisMenuName, "-")
                For Each lDisp As atcDataDisplay In lPlugins
                    Dim lMenuText As String = lDisp.Name
                    If Not lMenuText.Equals(aIgnore) AndAlso lMenuText.StartsWith("Analysis::") Then
                        AddMenuIfMissing(AnalysisMenuName & "_" & lDisp.Name, AnalysisMenuName, lMenuText.Substring(10), lSeparatorName, , True)
                    End If
                Next
            End If
        End If
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks></remarks>
    Friend Sub RefreshComputeMenu()
        g_MapWin.Menus.Remove(ComputeMenuName)
        g_MapWin.Menus.AddMenu(ComputeMenuName, "", Nothing, ComputeMenuString, FileMenuName)
        Dim lDataSources As atcCollection = pDataManager.GetPlugins(GetType(atcDataSource))
        For Each ds As atcDataSource In lDataSources
            If ds.Category <> "File" Then
                Dim lCategoryMenuName As String = ComputeMenuName & "_" & ds.Category
                Dim lOperations As atcDataAttributes = ds.AvailableOperations
                If Not lOperations Is Nothing AndAlso lOperations.Count > 0 Then
                    For Each lOperation As atcDefinedValue In lOperations
                        Select Case lOperation.Definition.TypeString
                            Case "atcTimeseries", "atcDataGroup"
                                AddMenuIfMissing(lCategoryMenuName, ComputeMenuName, ds.Category, , , True)
                                'Operations might have categories to further divide them
                                If lOperation.Definition.Category.Length > 0 Then
                                    Dim lSubCategoryName As String = lCategoryMenuName & "_" & lOperation.Definition.Category
                                    AddMenuIfMissing(lSubCategoryName, lCategoryMenuName, lOperation.Definition.Category, , , True)
                                    AddMenuIfMissing(lSubCategoryName & "_" & lOperation.Definition.Name, lSubCategoryName, lOperation.Definition.Name, , , True)
                                Else
                                    AddMenuIfMissing(lCategoryMenuName & "_" & lOperation.Definition.Name, lCategoryMenuName, lOperation.Definition.Name, , , True)
                                End If
                        End Select
                    Next
                Else
                    AddMenuIfMissing(lCategoryMenuName & "_" & ds.Description, lCategoryMenuName, ds.Description, , , True)
                End If
            End If
        Next
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks></remarks>
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
End Module