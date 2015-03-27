Imports atcUtility
Imports MapWinUtility

Public Class frmHspfSimulationManager

    Friend g_ProgramDir As String = ""
    Friend Const g_AppNameShort As String = "HspfSimulationManager"
    Friend Const g_AppNameLong As String = "HSPF Simulation Manager"

    Friend WithEvents SchematicDiagram As HSPFSimulationManager.ctlSchematic
    Private pStatusMonitor As MonitorProgressStatus

    Private pSpecFileName As String = String.Empty

    Private Sub frmHspfSimulationManager_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        pSpecFileName = GetSetting(g_AppNameShort, "Defaults", "FileName", pSpecFileName)
        If FileExists(pSpecFileName) Then
            OpenFile(pSpecFileName)
            'Else
            '    OpenToolStripMenuItem_Click(Nothing, Nothing)
        End If
    End Sub

    Private Sub frmHspfSimulationManager_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        If SaveIfChanged() Then
            e.Cancel = True
        End If
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        If Not SaveIfChanged() Then
            End
        End If
    End Sub

    ''' <summary>
    ''' Prompt to save changes if there are any changes
    ''' Returns True if user cancelled
    ''' </summary>
    Private Function SaveIfChanged() As Boolean
        Dim lPromptToSave As Boolean = False
        If Not FileExists(pSpecFileName) Then
            lPromptToSave = True
        Else
            If SchematicDiagram.AllIcons.Count > 0 Then
                Dim lTempFileName As String = GetTemporaryFileName(g_AppNameShort, ".txt")
                clsSimulationManagerSpecFile.Save(SchematicDiagram.AllIcons, Me.Size, New Drawing.Size(SchematicDiagram.IconWidth, SchematicDiagram.IconHeight), lTempFileName)
                Dim lNewFileContents As String = IO.File.ReadAllText(lTempFileName)
                Dim lExistingFileContents As String = IO.File.ReadAllText(pSpecFileName)
                If lNewFileContents <> lExistingFileContents Then
                    lPromptToSave = True
                End If
                TryDelete(lTempFileName)
            End If
        End If
        If lPromptToSave Then
            Dim lStyle As MsgBoxStyle = MsgBoxStyle.YesNoCancel
            Select Case Logger.Msg("Save specification file?", lStyle, g_AppNameLong)
                Case MsgBoxResult.Yes
                    SaveToolStripMenuItem_Click(Nothing, Nothing)
                Case MsgBoxResult.Cancel
                    Return True
            End Select
        End If
        Return False
    End Function

    Private Sub OpenToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OpenToolStripMenuItem.Click
        Dim lFileName As String = String.Empty
        If BrowseOpen("Open Simulation Specification File", "*.txt", ".txt", Me, lFileName) Then
            OpenFile(lFileName)
        End If
    End Sub

    Private Sub OpenFile(aFileName As String)
        If FileExists(aFileName) Then
            Me.Text = g_AppNameLong & " - " & aFileName
            Dim lIconSize As New Drawing.Size(SchematicDiagram.IconWidth, SchematicDiagram.IconHeight)
            Dim lNewIcons As IconCollection = clsSimulationManagerSpecFile.Open(Me.Size, lIconSize, aFileName)
            SchematicDiagram.IconWidth = lIconSize.Width
            SchematicDiagram.IconHeight = lIconSize.Height
            SchematicDiagram.BuildTree(lNewIcons)

            SaveSetting(g_AppNameShort, "Defaults", "FileName", aFileName)
        End If
    End Sub

    Public Shared Function BrowseOpen(ByVal aTitle As String, ByVal aFilter As String, ByVal aExtension As String, ByVal aParentForm As Form, ByRef aFileName As String) As Boolean
        Dim lFileDialog As New Windows.Forms.OpenFileDialog()
        With lFileDialog
            .Title = aTitle
            If IO.File.Exists(aFileName) Then
                .FileName = aFileName
            End If
            If Not String.IsNullOrEmpty(aFilter) Then
                If Not aFilter.Contains("|") Then aFilter &= "|" & aFilter
                .Filter = aFilter
                .FilterIndex = 0
            End If
            .DefaultExt = aExtension
            .CheckFileExists = True
            If .ShowDialog(aParentForm) = DialogResult.OK Then
                aFileName = .FileName
                Return True
            End If
        End With
        Return False
    End Function

    Private Sub SaveToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveToolStripMenuItem.Click
        Dim lFileDialog As New Windows.Forms.SaveFileDialog()
        With lFileDialog
            .Title = "Save Simulation Specification File"
            .FileName = pSpecFileName
            '.Filter = aFilter
            '.FilterIndex = 0
            '.DefaultExt = aExtension
            '.CheckFileExists = False
            '.CheckPathExists = False
            If .ShowDialog(Me) = DialogResult.OK Then
                clsSimulationManagerSpecFile.Save(SchematicDiagram.AllIcons, Me.Size, New Drawing.Size(SchematicDiagram.IconWidth, SchematicDiagram.IconHeight), .FileName)
                SaveSetting(g_AppNameShort, "Defaults", "FileName", .FileName)
            End If
        End With
    End Sub

    Private Sub btnRunHSPF_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRunHSPF.Click
        ShowMessageIfNoConnection()
        Dim lRun As New frmRunHSPF
        lRun.Icon = Me.Icon
        lRun.SchematicDiagram = SchematicDiagram
        lRun.Show(Me)
        'frmModel.RunUCI()
    End Sub

    Private Sub AddWatershedToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AddWatershedToolStripMenuItem.Click
        Dim lModelForm As New frmModel
        lModelForm.Schematic = SchematicDiagram
        lModelForm.ModelIcon = SchematicDiagram.AllIcons.FindOrAddIcon("New Watershed")
        'Place new icon in middle of diagram, not at extreme upper left which triggers unwanted total layout refresh
        If lModelForm.ModelIcon.Left = 0 AndAlso lModelForm.ModelIcon.Top = 0 Then
            lModelForm.ModelIcon.Left = (SchematicDiagram.Width - SchematicDiagram.IconWidth) / 2
            lModelForm.ModelIcon.Top = (SchematicDiagram.Height - SchematicDiagram.IconHeight) / 2
        End If
        lModelForm.Show()
    End Sub

    Private Sub ShowMessageIfNoConnection()
        Dim lReport As String = ""
        Dim lReportLine As String

        For Each lIcon As clsIcon In SchematicDiagram.AllIcons
            Dim lUpstreamUCI As atcUCI.HspfUci = lIcon.UciFile
            If lUpstreamUCI Is Nothing Then
                lReportLine = "UCI file not found: " & lIcon.UciFileName
                If Not lReport.Contains(lReportLine) Then
                    lReport &= lReportLine & vbCrLf
                End If
            ElseIf lIcon.DownstreamIcon IsNot Nothing Then
                Dim lDownstreamUCI As atcUCI.HspfUci = lIcon.DownstreamIcon.UciFile
                If lDownstreamUCI IsNot Nothing Then
                    Dim lConnCheck As List(Of String) = modUCI.ConnectionSummary(lUpstreamUCI, lDownstreamUCI)
                    If lConnCheck Is Nothing OrElse lConnCheck.Count = 0 Then
                        lReport &= "No datasets found connecting " & vbCrLf & lIcon.UciFileName & vbCrLf & "to" & vbCrLf & lIcon.DownstreamIcon.UciFileName & vbCrLf
                    End If
                End If
            End If
        Next

        If lReport.Length > 0 Then
            MsgBox(lReport, MsgBoxStyle.OkOnly, "Connection Report")
        End If

    End Sub

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Moved SchematicDiagram out of InitializeComponent because form designer was having trouble:
        SchematicDiagram = New HSPFSimulationManager.ctlSchematic()
        With SchematicDiagram
            .BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
            .Dock = System.Windows.Forms.DockStyle.Fill
            .Name = "SchematicDiagram"
            .TabIndex = 3
            .AllowDrop = True
            Me.Controls.Add(SchematicDiagram)
            .BringToFront()
            '.Location = New System.Drawing.Point(0, MenuStrip1.Height + Me.ToolStripMain.Height) ' 49)
        End With

        ' Add any initialization after the InitializeComponent() call.
        g_ProgramDir = PathNameOnly(Reflection.Assembly.GetEntryAssembly.Location)
        Try
            Environment.SetEnvironmentVariable("PATH", g_ProgramDir & ";" & Environment.GetEnvironmentVariable("PATH"))
        Catch eEnv As Exception
        End Try
        If g_ProgramDir.EndsWith("bin") Then g_ProgramDir = PathNameOnly(g_ProgramDir)
        g_ProgramDir &= g_PathChar

        Dim lLogFolder As String = g_ProgramDir & "cache"
        If IO.Directory.Exists(lLogFolder) Then
            lLogFolder = lLogFolder & g_PathChar & "log" & g_PathChar
        Else
            lLogFolder = IO.Path.Combine(My.Computer.FileSystem.SpecialDirectories.MyDocuments, "log") & g_PathChar
        End If
        Logger.StartToFile(lLogFolder & Format(Now, "yyyy-MM-dd") & "at" & Format(Now, "HH-mm") & "-" & g_AppNameShort & ".log")
        Logger.Icon = Me.Icon

        If Logger.ProgressStatus Is Nothing OrElse Not (TypeOf (Logger.ProgressStatus) Is MonitorProgressStatus) Then
            'Start running status monitor to give better progress and status indication during long-running processes
            pStatusMonitor = New MonitorProgressStatus
            If pStatusMonitor.StartMonitor(FindFile("Find Status Monitor", "StatusMonitor.exe"), _
                                            g_ProgramDir, _
                                            System.Diagnostics.Process.GetCurrentProcess.Id) Then
                pStatusMonitor.InnerProgressStatus = Logger.ProgressStatus
                Logger.ProgressStatus = pStatusMonitor
                Logger.Status("LABEL TITLE " & g_AppNameLong & " Status")
                Logger.Status("PROGRESS TIME ON") 'Enable time-to-completion estimation
                Logger.Status("")
            Else
                pStatusMonitor.StopMonitor()
                pStatusMonitor = Nothing
            End If
        End If

        atcData.atcDataManager.Clear()
        With atcData.atcDataManager.DataPlugins
            '.Add(New atcHspfBinOut.atcTimeseriesFileHspfBinOut)
            '.Add(New atcWdmVb.atcWDMfile)
            .Add(New atcWDM.atcDataSourceWDM)
            .Add(New atcTimeseriesScript.atcTimeseriesScriptPlugin)

        End With

        'atcTimeseriesStatistics.atcTimeseriesStatistics.InitializeShared()

    End Sub

    Private Sub FullReportToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles FullReportToolStripMenuItem.Click
        Me.Enabled = False
        Me.Cursor = Cursors.WaitCursor
        Dim lReport As String = frmHspfSimulationManager.g_AppNameLong & " Connection Report" & vbCrLf
        Dim lReportLine As String
        For Each lIcon As clsIcon In SchematicDiagram.AllIcons
            lReport &= vbCrLf & lIcon.WatershedName & ": " & vbCrLf
            Dim lUpstreamUCI As atcUCI.HspfUci = lIcon.UciFile
            Dim lDownstreamIcon As clsIcon = lIcon.DownstreamIcon
            If lUpstreamUCI Is Nothing Then
                lReport &= "UCI file not found: " & lIcon.UciFileName & vbCrLf
            ElseIf lDownstreamIcon Is Nothing Then
                lReport &= "No downstream watershed specified" & vbCrLf
            Else
                Dim lDownstreamUCI As atcUCI.HspfUci = lDownstreamIcon.UciFile
                If lDownstreamUCI Is Nothing Then
                    lReport &= "Downstream UCI file not found: " & lDownstreamIcon.UciFileName & vbCrLf
                Else
                    lReport &= "Upstream UCI file: " & vbCrLf & lIcon.UciFileName & vbCrLf & vbCrLf & "Downstream UCI file:" & vbCrLf & lDownstreamIcon.UciFileName & vbCrLf & vbCrLf
                    Dim lConnCheck As List(Of String) = modUCI.ConnectionSummary(lUpstreamUCI, lDownstreamUCI)
                    If lConnCheck Is Nothing OrElse lConnCheck.Count = 0 Then
                        lReport &= "No connecting datasets found." & vbCrLf
                    Else
                        Dim lWDMFileName As String = String.Empty
                        For Each lReportLine In lConnCheck
                            Dim lFields() As String = lReportLine.Split("|"c)
                            If lFields(0) <> lWDMFileName Then
                                lWDMFileName = lFields(0)
                                lReport &= "WDM file: " & vbCrLf & lWDMFileName & vbCrLf
                            End If
                            For lField = 1 To lFields.Length - 1
                                lReport &= vbTab & lFields(lField)
                            Next
                            lReport &= vbCrLf
                        Next
                    End If
                End If
            End If
        Next
        If lReport.Length > 0 Then
            Dim lText As New frmText
            lText.Icon = Me.Icon
            lText.Text = frmHspfSimulationManager.g_AppNameLong & " Connection Report"
            lText.txtMain.Text = lReport
            lText.Show()
        End If
        Me.Cursor = Cursors.Default
        Me.Enabled = True
    End Sub
End Class
