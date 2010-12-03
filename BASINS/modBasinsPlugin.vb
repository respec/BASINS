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
    Friend g_AppNameRegistry As String = "BASINS4" 'For preferences in registry
    Friend g_AppNameShort As String = "Basins"
    Friend g_AppNameLong As String = "BASINS 4"
    Friend g_URL_Home As String = ""
    Friend g_URL_Register As String = ""

    Friend g_BasinsDataDirs As New ArrayList
    Friend g_ProgramDir As String = ""
    Friend g_ProgressPanel As Windows.Forms.Panel
    Friend pBuildFrm As frmBuildNew

    Friend pExistingMapWindowProjectName As String = ""
    Friend pCommandLineScript As Boolean = False

    'File menu -- created by MapWindow

    Friend Const ProjectsMenuName As String = "ProgramProjects"
    Friend ProjectsMenuString As String = "Open BASINS Project"

    Friend pWelcomeScreenShow As Boolean = False

    Friend Const RegisterMenuName As String = "RegisterProgram"
    Friend RegisterMenuString As String = "Register as a BASINS user"

    Friend Const CheckForUpdatesMenuName As String = "CheckForUpdates"
    Friend Const CheckForUpdatesMenuString As String = "Check For Updates"

    Friend Const ShowStatusMenuName As String = "ShowStatus"
    Friend Const ShowStatusMenuString As String = "Show Status Monitor"

    Friend Const HelpMenuName As String = "mnuHelp"
    Friend Const BasinsHelpMenuName As String = "BasinsHelp"
    Friend Const BasinsHelpMenuString As String = "BASINS Documentation"

    Friend Const ProgramWebPageMenuName As String = "ProgramWebPage"
    Friend ProgramWebPageMenuString As String = "BASINS Web Page"

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

            For Each lDriveInfo As IO.DriveInfo In IO.DriveInfo.GetDrives()
                With lDriveInfo
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
    Public Sub LoadNationalProject()
        If Not NationalProjectIsOpen() Then
            Dim lFileName As String = IO.Path.Combine(g_ProgramDir, "Data\national" & g_PathChar & NationalProjectFilename)
            If Not FileExists(lFileName) Then
                For Each lDir As String In g_BasinsDataDirs
                    lFileName = lDir & "national" & g_PathChar & NationalProjectFilename
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
            Try
                pBuildFrm.Top = GetSetting(g_AppNameRegistry, "Window Positions", "BuildTop", "300")
                If pBuildFrm.Top < 0 Then
                    pBuildFrm.Top = 0
                ElseIf pBuildFrm.Top + pBuildFrm.Height > Windows.Forms.Screen.PrimaryScreen.Bounds.Height Then
                    pBuildFrm.Top = Windows.Forms.Screen.PrimaryScreen.Bounds.Height - pBuildFrm.Height
                End If
            Catch
            End Try

            Try
                pBuildFrm.Left = GetSetting(g_AppNameRegistry, "Window Positions", "BuildLeft", "0")
                If pBuildFrm.Left < 0 Then
                    pBuildFrm.Left = 0
                ElseIf pBuildFrm.Left + pBuildFrm.Width > Windows.Forms.Screen.PrimaryScreen.Bounds.Width Then
                    pBuildFrm.Left = Windows.Forms.Screen.PrimaryScreen.Bounds.Width - pBuildFrm.Width
                End If
            Catch
            End Try

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
    ''' check to see if project has both a catalog unit layer and a state layer
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Friend Function IsBASINSProject() As Boolean
        Dim lHaveCatLayer As Boolean = False
        Dim lHaveStateLayer As Boolean = False
        For Each lLayer As MapWindow.Interfaces.Layer In g_MapWin.Layers
            Select Case FilenameNoPath(lLayer.FileName).ToLower
                Case "st.shp" : lHaveStateLayer = True
                Case "cat.shp" : lHaveCatLayer = True
            End Select
            If lHaveCatLayer And lHaveStateLayer Then
                Exit For
            End If
        Next
        Return lHaveCatLayer And lHaveStateLayer
    End Function

    Public Function DefaultBasinsDataDir() As String
        If g_BasinsDataDirs IsNot Nothing AndAlso g_BasinsDataDirs.Count > 0 Then
            Return g_BasinsDataDirs(0)
        Else
            Try
                Return My.Computer.FileSystem.SpecialDirectories.MyDocuments & g_PathChar & BasinsDataPath
            Catch
                Try
                    Return Environment.GetFolderPath(Environment.SpecialFolder.Personal & g_PathChar & BasinsDataPath)
                Catch ex As Exception
                    Return g_PathChar & BasinsDataPath
                End Try
            End Try
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
            If g_MapWin.Layers.Item(g_MapWin.Layers.CurrentLayer).LayerType = MapWindow.Interfaces.eLayerType.PolygonShapefile Then
                lCurLayer = g_MapWin.Layers.Item(g_MapWin.Layers.CurrentLayer).GetObject
                If g_MapWin.View.SelectedShapes.NumSelected > 0 Then
                    ctext = "Selected Features:"
                    Select Case IO.Path.GetFileNameWithoutExtension(lCurLayer.Filename).ToLower
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
        End If
    End Sub

    Friend Sub ClearLayers()
        g_MapWin.Layers.Clear()
    End Sub

    Friend Sub RefreshView()
        g_MapWin.Refresh()
    End Sub
End Module