Imports atcData
Imports atcUtility
Imports atcMwGisUtility
Imports MapWinUtility

Public Class SDMplugin
    Inherits atcDataPlugin

    Private Const NationalProjectFilename As String = "ProjectBuilder.mwprj"
    Private NationalProjectFullPath As String = ""
    Private pStatusMonitor As MonitorProgressStatus

    Public Overrides ReadOnly Property Name() As String
        Get
            Return "SDMProjectBuilder"
        End Get
    End Property

    'Public Overrides Sub Message(ByVal aMsg As String, ByRef aHandled As Boolean)
    '    If aMsg.StartsWith("WELCOME_SCREEN") Then
    '        Stop
    '    End If
    'End Sub

    <CLSCompliant(False)> _
    Public Overrides Sub Initialize(ByVal aMapWin As MapWindow.Interfaces.IMapWin, ByVal aParentHandle As Integer)
        g_MapWin = aMapWin        
        g_MapWin.ApplicationInfo.ApplicationName = g_AppNameLong

        atcMwGisUtility.GisUtil.MappingObject = g_MapWin
        atcDataManager.MapWindow = g_MapWin
        D4EMDataManager.SpatialOperations.MapOCX = g_MapWin.GetOCX

        g_MapWinWindowHandle = aParentHandle
        g_MapWin.ApplicationInfo.WelcomePlugin = "plugin" 'tell the main app to Plugins.BroadcastMessage("WELCOME_SCREEN") instead of showing default MW welcome screen
        'Set g_ProgramDir to folder above the Bin folder where the app and plugins live
        g_ProgramDir = PathNameOnly(PathNameOnly(Reflection.Assembly.GetEntryAssembly.Location)) & g_PathChar

        Logger.StartToFile(g_ProgramDir & "cache\log" & g_PathChar _
                         & Format(Now, "yyyy-MM-dd") & "at" & Format(Now, "HH-mm") & "-" & g_AppNameShort & ".log", False, True, True)
        Logger.Icon = g_MapWin.ApplicationInfo.FormIcon

        UnloadNonSDMPlugins()
        LoadSDMPlugins()

        If Logger.ProgressStatus Is Nothing OrElse Not (TypeOf (Logger.ProgressStatus) Is MonitorProgressStatus) Then
            'Start running status monitor to give better progress and status indication during long-running processes
            pStatusMonitor = New MonitorProgressStatus
            If pStatusMonitor.StartMonitor(FindFile("Find Status Monitor", "StatusMonitor.exe"), _
                                            g_ProgramDir & "cache\log" & g_PathChar, _
                                            System.Diagnostics.Process.GetCurrentProcess.Id) Then
                'put our seperate executable status monitor (StatusMonitor.exe) between the Logger and the default MW status monitor
                pStatusMonitor.InnerProgressStatus = Logger.ProgressStatus
                Logger.ProgressStatus = pStatusMonitor
                'Logger.Status("PROGRESS TIME ON") 'Enable time-to-completion estimation
                Logger.Status("")
            Else
                pStatusMonitor.StopMonitor()
                pStatusMonitor = Nothing
            End If
        End If
        Logger.Status("LABEL TITLE " & g_AppNameShort & " Status")
        g_MapWin.Toolbar.PressToolbarButton("tbbSelect")

    End Sub

    Public Overrides Sub ItemClicked(ByVal aItemName As String, ByRef aHandled As Boolean)
        Select Case aItemName
            Case "mnuNew", "tbbNew"  'Override new project behavior
                aHandled = True
                LoadNationalProject()
        End Select
    End Sub

    ''' <summary>
    ''' Skip default behavior of atcDataPlugin.Terminate
    ''' </summary>
    Public Overrides Sub Terminate()
    End Sub

    Public Overrides Sub LayerSelected(ByVal aHandle As Integer)
        If NationalProjectIsOpen() Then
            UpdateSelectedFeatures()
        End If
    End Sub

    Public Overrides Sub ProjectLoading(ByVal aProjectFile As String, ByVal aSettingsString As String)
        If Not BuildFormIsOpen() AndAlso aProjectFile IsNot Nothing AndAlso aProjectFile.ToLower.EndsWith(NationalProjectFilename.ToLower) Then
            If FileExists(aProjectFile) Then NationalProjectFullPath = aProjectFile
            LoadNationalProject()
        End If
    End Sub

    Public Overrides Sub ShapesSelected(ByVal aHandle As Integer, ByVal aSelectInfo As MapWindow.Interfaces.SelectInfo)
        If NationalProjectIsOpen() Then
            UpdateSelectedFeatures()
        Else
            'for debugging
            'Dim lMetWDMFileName As String = PathNameOnly(GisUtil.LayerFileName("Weather Station Sites 2006")) & "\met.wdm"
            'Dim lLandUseFileName As String = GisUtil.LayerFileName("NLCD 2001 Landcover")
            'Dim lElevationFileName As String = GisUtil.LayerFileName("NHDPlus Elevation")
            'Dim lSimplifiedCatchmentsFileName As String = GisUtil.LayerFileName("Simplified_Catchments")
            'Dim lSimplifiedFlowlinesFileName As String = GisUtil.LayerFileName("Simplified_Flowlines")
            'Dim lDataDir As String = "C:\dev\BASINS40\data\02060006-11\"
            'Dim lSelectedHuc As String = "02060006"
            ''BatchHSPF.BatchHSPF(lSimplifiedCatchmentsFileName, lSimplifiedFlowlinesFileName, _
            ''                    lLandUseFileName, lElevationFileName, _
            ''                    lMetWDMFileName, lDataDir, lSelectedHuc)
            'BatchSWAT(lSelectedHuc, lDataDir, lSimplifiedCatchmentsFileName, lSimplifiedFlowlinesFileName, _
            '          lLandUseFileName, lElevationFileName)
        End If
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub LoadNationalProject()
        UnloadNonSDMPlugins()
        LoadSDMPlugins()
        If Not NationalProjectIsOpen() Then
            If Not FileExists(NationalProjectFullPath) Then
                NationalProjectFullPath = FindFile("Open " & NationalProjectFilename, IO.Path.Combine(CurDir, "Data\national" & g_PathChar & NationalProjectFilename))
            End If
            If FileExists(NationalProjectFullPath) Then  'load national project

                g_MapWin.Project.Load(NationalProjectFullPath)

                'See if we need to also process and load place names
                Dim lInstructions As String = D4EMDataManager.SpatialOperations.CheckPlaceNames(IO.Path.GetDirectoryName(NationalProjectFullPath), g_MapWin.Project.ProjectProjection)
                If lInstructions.Length > 0 Then
                    Dim lDisplayMessageBoxes As Boolean = Logger.DisplayMessageBoxes
                    Logger.DisplayMessageBoxes = False 'Don't show a message box after adding these layers
                    ProcessDownloadResults(lInstructions)
                    Logger.DisplayMessageBoxes = lDisplayMessageBoxes
                End If
            Else
                Logger.Msg("Unable to find '" & NationalProjectFilename & "'", "LoadNationalProject")
                Exit Sub
            End If
        End If

        If NationalProjectIsOpen() Then
            'Select HUC-12 layer by default, or HUC-8 if no HUC-12 is on map
            Dim lHuc12Layer As Integer = Huc12Layer()
            If lHuc12Layer > -1 Then
                g_MapWin.Layers.CurrentLayer = lHuc12Layer
            Else
                Dim lHuc8Layer As Integer = Huc8Layer()
                If lHuc8Layer > -1 Then
                    g_MapWin.Layers.CurrentLayer = lHuc8Layer
                End If
            End If

            ReadParametersTextFile(IO.Path.Combine(IO.Path.GetDirectoryName(NationalProjectFullPath), PARAMETER_FILE))

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
            g_MapWin.Toolbar.PressToolbarButton("tbbSelect")

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
        If (Not g_MapWin.Project Is Nothing) _
            AndAlso (Not g_MapWin.Project.FileName Is Nothing) _
            AndAlso g_MapWin.Project.FileName.EndsWith(NationalProjectFilename) Then
            Return True
        Else
            Return False
        End If
    End Function

End Class
