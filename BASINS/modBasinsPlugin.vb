Imports System.Collections.Specialized
Imports System.Reflection
Imports atcUtility
Imports atcData
Imports MapWinUtility
Imports atcMwGisUtility

''' <summary>
''' 
''' </summary>
''' <remarks></remarks>
Public Module modBasinsPlugin
    'Declare this as global so that it can be accessed throughout the plug-in project.
    'These variables are initialized in the plugin_Initialize event.
    Friend g_MapWin As MapWindow.Interfaces.IMapWin
    Friend g_Menus As MapWindow.Interfaces.Menus
    Friend g_StatusBar As MapWindow.Interfaces.StatusBar
    Friend g_Toolbar As MapWindow.Interfaces.Toolbar
    Friend g_Plugins As MapWindow.Interfaces.Plugins
    Friend g_Project As MapWindow.Interfaces.Project
    Friend g_MapWinWindowHandle As Integer
    Friend g_AppName As String = "BASINS4"
    Friend g_BasinsDataDirs As New ArrayList
    Friend g_BasinsDir As String = ""
    Friend g_ProgressPanel As Windows.Forms.Panel
    Friend pBuildFrm As frmBuildNew

    Friend pExistingMapWindowProjectName As String = ""
    Friend pCommandLineScript As Boolean = False

    'File menu -- created by MapWindow

    Friend Const ProjectsMenuName As String = "BasinsProjects"
    Friend Const ProjectsMenuString As String = "Open BASINS Project"

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

    Private Const BasinsDataPath As String = "Basins\data\"
    Private Const NationalProjectFilename As String = "national.mwprj"

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks></remarks>
    Friend Sub FindBasinsDrives()
        If g_BasinsDataDirs.Count = 0 Then
            Dim lCheckDir As String = DefaultBasinsDataDir()
            If FileExists(lCheckDir, True, False) Then g_BasinsDataDirs.Add(lCheckDir)

            For Each lDrive As IO.DriveInfo In IO.DriveInfo.GetDrives()
                With lDrive
                    If .IsReady AndAlso .DriveType = IO.DriveType.Fixed OrElse .DriveType = IO.DriveType.Network Then
                        lCheckDir = .Name & BasinsDataPath
                        If Not g_BasinsDataDirs.Contains(lCheckDir) AndAlso FileExists(lCheckDir, True, False) Then
                            g_BasinsDataDirs.Add(lCheckDir)
                        End If
                    End If
                End With
            Next

            Select Case g_BasinsDataDirs.Count
                Case 0 : Logger.Msg("No BASINS folders found on any drives on this computer", "FindBasinsDrives")
                Case 1 : Logger.Dbg("Found BASINS Data: " & g_BasinsDataDirs(0))
                Case Is > 1
                    Dim lAllDirs As String = ""
                    For Each lDir As String In g_BasinsDataDirs
                        lAllDirs &= lDir & "  "
                    Next
                    Logger.Dbg("Found BASINS Data: " & lAllDirs)
            End Select
        End If
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks></remarks>
    Friend Sub LoadNationalProject()
        If Not NationalProjectIsOpen() Then
            Dim lFileName As String = IO.Path.Combine(g_BasinsDir, "Data\national\" & NationalProjectFilename)
            If Not FileExists(lFileName) Then
                For Each lDir As String In g_BasinsDataDirs
                    lFileName = lDir & "national\" & NationalProjectFilename
                    If FileExists(lFileName) Then 'found existing national project
                        Exit For
                    End If
                Next
            End If

            If FileExists(lFileName) Then  'load national project

                g_Project.Load(lFileName)

                'See if we need to also process and load place names
                Dim lInstructions As String = D4EMDataManager.SpatialOperations.CheckPlaceNames(IO.Path.GetDirectoryName(lFileName), g_Project.ProjectProjection)
                If lInstructions.Length > 0 Then
                    Dim lDisplayMessageBoxes As Boolean = Logger.DisplayMessageBoxes
                    Logger.DisplayMessageBoxes = False 'Don't show a message box after adding these layers
                    ProcessDownloadResults(lInstructions)
                    Logger.DisplayMessageBoxes = lDisplayMessageBoxes
                End If
            Else
                Logger.Msg("Unable to find '" & NationalProjectFilename & "'", "Open National")
                Exit Sub
            End If
        End If

        If NationalProjectIsOpen() Then
            'Select the Cataloging Units layer by default 
            For iLayer As Integer = 0 To g_MapWin.Layers.NumLayers - 1
                If g_MapWin.Layers(g_MapWin.Layers.GetHandle(iLayer)).Name = "Cataloging Units" Then
                    g_MapWin.Layers.CurrentLayer = g_MapWin.Layers.GetHandle(iLayer)
                    Exit For
                End If
            Next
            g_Toolbar.PressToolbarButton("tbbSelect")
            pBuildFrm = New frmBuildNew
            pBuildFrm.Show()
            pBuildFrm.Top = GetSetting("BASINS4", "Window Positions", "BuildTop", "300")
            pBuildFrm.Left = GetSetting("BASINS4", "Window Positions", "BuildLeft", "0")
            UpdateSelectedFeatures()
        Else
            Logger.Msg("Unable to open national project", "Open National")
        End If
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function NationalProjectIsOpen() As Boolean
        If (Not g_Project Is Nothing) _
            AndAlso (Not g_Project.FileName Is Nothing) _
            AndAlso g_Project.FileName.ToLower.EndsWith(NationalProjectFilename) Then
            Return True
        Else
            Return False
        End If
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Friend Function IsBASINSProject() As Boolean
        'make sure the project has a cat layer and state layer
        Dim lHaveCatLayer As Boolean = False
        Dim lHaveStateLayer As Boolean = False
        For i As Integer = 0 To GisUtil.NumLayers - 1
            If FilenameNoPath(GisUtil.LayerFileName(i)) = "st.shp" Then
                lHaveStateLayer = True
            End If
            If FilenameNoPath(GisUtil.LayerFileName(i)) = "cat.shp" Then
                lHaveCatLayer = True
            End If
        Next
        If lHaveCatLayer And lHaveStateLayer Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Function DefaultBasinsDataDir() As String
        'TODO: change to using MyDocuments when the installer starts using Progra~1
        'Return My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\" & BasinsDataPath

        If Not g_BasinsDataDirs Is Nothing AndAlso g_BasinsDataDirs.Count > 0 Then
            Return g_BasinsDataDirs(0)
        Else
            Return My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\" & BasinsDataPath
        End If
    End Function

    Friend Sub UpdateSelectedFeatures()
        If Not pBuildFrm Is Nothing AndAlso g_MapWin.Layers.NumLayers > 0 AndAlso g_MapWin.Layers.CurrentLayer > -1 Then
            Dim lFieldName As String = ""
            Dim lFieldDesc As String = ""
            Dim lField As Integer
            Dim lNameIndex As Integer = -1
            Dim lDescIndex As Integer = -1
            Dim lCurLayer As MapWinGIS.Shapefile
            Dim ctext As String

            RefreshView()
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

    Friend Sub ClearLayers()
        g_MapWin.Layers.Clear()
    End Sub

    Friend Sub RefreshView()
        g_MapWin.Refresh()
    End Sub
End Module