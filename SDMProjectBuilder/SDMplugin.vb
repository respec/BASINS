Imports atcData
Imports atcUtility
Imports MapWinUtility

Public Class SDMplugin
    Inherits atcDataPlugin

    Private Const NationalProjectFilename As String = "sdm.mwprj"

    Public Overrides ReadOnly Property Name() As String
        Get
            Return "SDM"
        End Get
    End Property

    Public Overrides Sub Message(ByVal aMsg As String, ByRef aHandled As Boolean)
        If aMsg.StartsWith("WELCOME_SCREEN") Then
            Stop
        End If
    End Sub

    <CLSCompliant(False)> _
    Public Overrides Sub Initialize(ByVal aMapWin As MapWindow.Interfaces.IMapWin, ByVal aParentHandle As Integer)
        g_MapWin = aMapWin        
        g_MapWin.ApplicationInfo.ApplicationName = g_AppNameLong

        atcMwGisUtility.GisUtil.MappingObject = g_MapWin
        atcDataManager.MapWindow = g_MapWin

        g_MapWinWindowHandle = aParentHandle
        g_MapWin.ApplicationInfo.WelcomePlugin = "plugin" 'tell the main app to Plugins.BroadcastMessage("WELCOME_SCREEN") instead of showing default MW welcome screen
        'Set g_ProgramDir to folder above the Bin folder where the app and plugins live
        g_ProgramDir = PathNameOnly(PathNameOnly(Reflection.Assembly.GetEntryAssembly.Location)) & g_PathChar
        Logger.Icon = g_MapWin.ApplicationInfo.FormIcon

        'Logger.MsgCustom("Test Message", "Test Title", "Button One", "2")
        'For i As Integer = 1 To 10
        '    If Logger.MsgCustom("Test Message " & i, "Test Title " & i, "Button One", "2", "All") = "All" Then Exit For
        'Next
        'For i As Integer = 1 To 10
        '    If Logger.MsgCustomCheckbox("Test Message " & i, "Test Title " & i, g_AppNameShort, "Test", "Buttons", "Button One", "2", "All") = "All" Then Exit For
        'Next

        Try
            Dim lKey As String = g_MapWin.Plugins.GetPluginKey("Timeseries::Statistics")
            'If Not g_MapWin.Plugins.PluginIsLoaded(lKey) Then 
            g_MapWin.Plugins.StartPlugin(lKey)
        Catch lEx As Exception
            Logger.Dbg("Exception loading Timeseries::Statistics - " & lEx.Message)
        End Try

        atcDataManager.LoadPlugin("Timeseries::Statistics")
        atcDataManager.LoadPlugin("D4EM Data Download::Main")

    End Sub

    Public Overrides Sub Terminate()
    End Sub

    Public Overrides Sub ProjectLoading(ByVal aProjectFile As String, ByVal aSettingsString As String)
        If pBuildFrm Is Nothing AndAlso aProjectFile IsNot Nothing AndAlso aProjectFile.EndsWith("sdm.mwprj") Then LoadNationalProject()
    End Sub

    Public Overrides Sub ShapesSelected(ByVal aHandle As Integer, ByVal aSelectInfo As MapWindow.Interfaces.SelectInfo)
        If NationalProjectIsOpen() Then
            UpdateSelectedFeatures()
        End If
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub LoadNationalProject()
        If Not NationalProjectIsOpen() Then
            Dim lFileName As String = IO.Path.Combine(CurDir, "Data\national" & g_PathChar & NationalProjectFilename)
            If FileExists(lFileName) Then  'load national project

                g_MapWin.Project.Load(lFileName)

                'See if we need to also process and load place names
                Dim lInstructions As String = D4EMDataManager.SpatialOperations.CheckPlaceNames(IO.Path.GetDirectoryName(lFileName), g_MapWin.Project.ProjectProjection)
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
            g_MapWin.Toolbar.PressToolbarButton("tbbSelect")
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
        If (Not g_MapWin.Project Is Nothing) _
            AndAlso (Not g_MapWin.Project.FileName Is Nothing) _
            AndAlso g_MapWin.Project.FileName.ToLower.EndsWith(NationalProjectFilename) Then
            Return True
        Else
            Return False
        End If
    End Function

End Class
