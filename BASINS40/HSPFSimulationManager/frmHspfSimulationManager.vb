﻿Imports atcUtility
Imports MapWinUtility

Public Class frmHspfSimulationManager

    Friend WithEvents SchematicDiagram As HSPFSimulationManager.ctlSchematic
    Private pSpecFileName As String = String.Empty

    Private Sub frmHspfSimulationManager_DragDrop(sender As Object, e As DragEventArgs) Handles Me.DragDrop
        SchematicDiagram.ctlSchematic_DragDrop(sender, e)
    End Sub

    Private Sub frmHspfSimulationManager_DragEnter(sender As Object, e As DragEventArgs) Handles Me.DragEnter
        SchematicDiagram.ctlSchematic_DragEnter(sender, e)
    End Sub

    Private Sub frmHspfSimulationManager_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        pSpecFileName = GetSetting(g_AppNameShort, "Defaults", "FileName", pSpecFileName)
        If FileExists(pSpecFileName) Then
            OpenFile(pSpecFileName)
        Else
            SchematicDiagram.Visible = False
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
        If BrowseOpen("Open Simulation Specification File", "*.hsmspec", ".hsmspec", Me, lFileName) Then
            OpenFile(lFileName)
        End If
    End Sub

    Private Sub OpenFile(aFileName As String)
        Try
            If FileExists(aFileName) Then
                With SchematicDiagram
                    Dim lIconSize As New Drawing.Size(SchematicDiagram.IconWidth, .IconHeight)
                    Dim lNewIcons As IconCollection = clsSimulationManagerSpecFile.Open(Me.Size, lIconSize, aFileName)
                    .BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
                    .BackgroundImage = Nothing
                    .IconWidth = lIconSize.Width
                    .IconHeight = lIconSize.Height
                    .BuildTree(lNewIcons)
                End With
                pSpecFileName = aFileName
                Me.Text = g_AppNameLong & " - " & pSpecFileName
                SaveSetting(g_AppNameShort, "Defaults", "FileName", pSpecFileName)
            End If
        Catch ex As Exception
            Logger.Msg("Could not open: " & aFileName & vbCrLf & ex.Message, MsgBoxStyle.Critical, "Error opening specification file")
        End Try
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
        If SchematicDiagram.AllIcons.Count = 0 Then
            Logger.Msg("There is nothing to save.", MsgBoxStyle.Critical, g_AppNameLong)
            Exit Sub
        End If
        Dim lFileDialog As New Windows.Forms.SaveFileDialog()
        With lFileDialog
            .Title = "Save Simulation Specification File"
            .FileName = pSpecFileName
            .DefaultExt = ".hsmspec"
            .Filter = "*" & .DefaultExt & "|*" & .DefaultExt & "*.*|*.*"
            .FilterIndex = 0
            '.CheckFileExists = False
            '.CheckPathExists = False
            If .ShowDialog(Me) = DialogResult.OK Then
                Try
                    clsSimulationManagerSpecFile.Save(SchematicDiagram.AllIcons, Me.Size, New Drawing.Size(SchematicDiagram.IconWidth, SchematicDiagram.IconHeight), .FileName)
                    pSpecFileName = .FileName
                    Me.Text = g_AppNameLong & " - " & pSpecFileName
                    SaveSetting(g_AppNameShort, "Defaults", "FileName", pSpecFileName)
                Catch ex As Exception
                    Logger.Msg("Could not save: " & .FileName & vbCrLf & ex.Message, MsgBoxStyle.Critical, "Error saving specification file")
                End Try
            End If
        End With
    End Sub

    Private Sub CloseToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CloseToolStripMenuItem.Click
        If SaveIfChanged() Then
            Exit Sub
        End If
        SchematicDiagram.Visible = False
        SchematicDiagram.AllIcons.Clear()
        SchematicDiagram.BuildTree(SchematicDiagram.AllIcons)
        Me.Text = g_AppNameLong
    End Sub

    Private Sub btnRunHSPF_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRunHSPF.Click
        If SchematicDiagram.AllIcons.Count = 0 Then
            Logger.Msg("Add watersheds or open a specification file before running.", MsgBoxStyle.Critical, g_AppNameLong)
            Exit Sub
        End If
        Me.Enabled = False
        Me.Cursor = Cursors.WaitCursor
        Dim lRun As New frmRunHSPF
        lRun.Icon = Me.Icon
        lRun.SchematicDiagram = SchematicDiagram
        lRun.ShowDialog(Me)
        Me.Cursor = Cursors.Default
        Me.Enabled = True
    End Sub

    Private Sub AddWatershedToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AddWatershedToolStripMenuItem.Click
        Dim lWatershedForm As New frmEditWatershed
        lWatershedForm.Schematic = SchematicDiagram
        Dim lWatershedName As String = "New Watershed"

        For lIndex As Integer = 1 To 100
            lWatershedName = "New Watershed"
            If lIndex > 1 Then lWatershedName &= " " & lIndex
            If Not SchematicDiagram.AllIcons.Contains(lWatershedName.ToLowerInvariant) Then
                Exit For
            End If
        Next

        lWatershedForm.ModelIcon = SchematicDiagram.AllIcons.FindOrAddIcon(lWatershedName)
        'Place new icon in middle of diagram, not at extreme upper left which triggers unwanted total layout refresh
        If lWatershedForm.ModelIcon.Left = 0 AndAlso lWatershedForm.ModelIcon.Top = 0 Then
            Dim lRandom As New Random()
            lWatershedForm.ModelIcon.Left = (SchematicDiagram.Width - SchematicDiagram.IconWidth) / 2 + lRandom.Next(SchematicDiagram.Width / 3)
            lWatershedForm.ModelIcon.Top = (SchematicDiagram.Height - SchematicDiagram.IconHeight) / 2 + lRandom.Next(SchematicDiagram.Height / 3)
        End If
        lWatershedForm.ShowDialog(Me)
    End Sub

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        g_ProgramDir = PathNameOnly(Reflection.Assembly.GetEntryAssembly.Location)
        Try
            Environment.SetEnvironmentVariable("PATH", g_ProgramDir & ";" & Environment.GetEnvironmentVariable("PATH"))
        Catch eEnv As Exception
        End Try
        If g_ProgramDir.EndsWith("bin") Then g_ProgramDir = PathNameOnly(g_ProgramDir)
        g_ProgramDir &= g_PathChar

        ' Moved SchematicDiagram out of InitializeComponent because form designer was having trouble:
        SchematicDiagram = New HSPFSimulationManager.ctlSchematic()
        With SchematicDiagram
            .Dock = System.Windows.Forms.DockStyle.Fill
            .Name = "SchematicDiagram"
            .TabIndex = 3
            .AllowDrop = True
            Me.Controls.Add(SchematicDiagram)
            .BringToFront()
            '.Location = New System.Drawing.Point(0, MenuStrip1.Height + Me.ToolStripMain.Height) ' 49)
        End With

        Dim lLogFolder As String = IO.Path.Combine(My.Computer.FileSystem.SpecialDirectories.MyDocuments, g_AppNameShort & "Logs") & g_PathChar
        Logger.StartToFile(lLogFolder & Format(Now, "yyyy-MM-dd") & "at" & Format(Now, "HH-mm") & "-" & g_AppNameShort & ".log")
        Logger.Icon = Me.Icon

        If Logger.ProgressStatus Is Nothing OrElse Not (TypeOf (Logger.ProgressStatus) Is MonitorProgressStatus) Then
            'Start running status monitor to give better progress and status indication during long-running processes
            g_StatusMonitor = New MonitorProgressStatus
            If g_StatusMonitor.StartMonitor(FindFile("Find Status Monitor", "StatusMonitor.exe"), _
                                            g_ProgramDir, _
                                            System.Diagnostics.Process.GetCurrentProcess.Id) Then
                g_StatusMonitor.InnerProgressStatus = Logger.ProgressStatus
                Logger.ProgressStatus = g_StatusMonitor
                Logger.Status("LABEL TITLE " & g_AppNameLong & " Status")
                Logger.Status("PROGRESS TIME ON") 'Enable time-to-completion estimation
                Logger.Status("")
            Else
                g_StatusMonitor.StopMonitor()
                g_StatusMonitor = Nothing
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

    Private Sub btnConnectionReport_Click(sender As Object, e As EventArgs) Handles btnConnectionReport.Click
        If SchematicDiagram.AllIcons.Count = 0 Then
            Logger.Msg("Add watersheds or open a specification file before requesting a report.", MsgBoxStyle.Critical, g_AppNameLong)
            Exit Sub
        End If
        Me.Enabled = False
        Me.Cursor = Cursors.WaitCursor
        Dim lReport As String = g_AppNameLong & " Connection Report" & vbCrLf
        Dim lIconIndex As Integer = 0
        Dim lSeparatorLine As New String("_", 80)
        For Each lIcon As clsIcon In SchematicDiagram.AllIcons
            Logger.Progress("Checking " & lIcon.WatershedName, lIconIndex, SchematicDiagram.AllIcons.Count)
            If lIcon.Scenario Is Nothing Then
                lReport &= lSeparatorLine & vbCrLf & lIcon.WatershedName & ", No Scenario" & vbCrLf
            Else
                lReport &= lSeparatorLine & vbCrLf & lIcon.WatershedName & ", " & lIcon.Scenario.ScenarioName & vbCrLf

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
                        lReport &= "To: " & lDownstreamIcon.WatershedName & ", " & lDownstreamIcon.Scenario.ScenarioName & vbCrLf & vbCrLf
                        lReport &= ConnectionReport(lUpstreamUCI, lDownstreamUCI)
                        'lReport &= "Upstream UCI file: " & vbCrLf & lIcon.UciFileName & vbCrLf & vbCrLf & "Downstream UCI file:" & vbCrLf & lDownstreamIcon.UciFileName & vbCrLf & vbCrLf
                        'Dim lConnCheck As List(Of String) = modUCI.ConnectionSummary(lUpstreamUCI, lDownstreamUCI)
                        'If lConnCheck Is Nothing OrElse lConnCheck.Count = 0 Then
                        '    lReport &= "No connecting datasets found." & vbCrLf
                        'Else
                        '    Dim lWDMFileName As String = String.Empty
                        '    For Each lReportLine In lConnCheck
                        '        Dim lFields() As String = lReportLine.Split("|"c)
                        '        If lFields(0) <> lWDMFileName Then
                        '            lWDMFileName = lFields(0)
                        '            lReport &= "WDM file: " & vbCrLf & lWDMFileName & vbCrLf
                        '        End If
                        '        For lField = 1 To lFields.Length - 1
                        '            lReport &= vbTab & lFields(lField)
                        '        Next
                        '        lReport &= vbCrLf
                        '    Next
                        'End If
                    End If
                End If
            End If
            lIconIndex += 1
        Next
        Logger.Progress(lIconIndex, lIconIndex)
        If lReport.Length > 0 Then
            Dim lText As New frmText
            lText.Icon = Me.Icon
            lText.Text = g_AppNameLong & " Connection Report"
            lText.txtMain.Text = lReport
            lText.Show()
        End If
        Me.Cursor = Cursors.Default
        Me.Enabled = True
    End Sub

End Class
