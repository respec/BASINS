Imports atcUtility
Imports MapWinUtility

Public Class frmHspfSimulationManager

    Public Const AppName As String = "HspfSimulationManager"

    Private pSpecFileName As String = String.Empty

    Private Sub frmHspfSimulationManager_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        pSpecFileName = GetSetting(AppName, "Defaults", "FileName", pSpecFileName)
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
                Dim lTempFileName As String = GetTemporaryFileName("HspfSimMgrSpec", ".txt")
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
            Select Case Logger.Msg("Save specification file?", lStyle, "HSPF Simulation Manager")
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
            Me.Text = "SARA HSPF Simulation Manager - " & aFileName
            Dim lIconSize As New Drawing.Size(SchematicDiagram.IconWidth, SchematicDiagram.IconHeight)
            Dim lNewIcons As IconCollection = clsSimulationManagerSpecFile.Open(Me.Size, lIconSize, aFileName)
            SchematicDiagram.IconWidth = lIconSize.Width
            SchematicDiagram.IconHeight = lIconSize.Height
            SchematicDiagram.BuildTree(lNewIcons)

            SaveSetting(AppName, "Defaults", "FileName", aFileName)
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
                SaveSetting(AppName, "Defaults", "FileName", .FileName)
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
                        lReport &= "No datasets found connecting " & lIcon.UciFileName & " to " & lIcon.DownstreamIcon.UciFileName & vbCrLf
                    End If
                End If
            End If
        Next

        If lReport.Length > 0 Then
            MsgBox(lReport, MsgBoxStyle.OkOnly, "Connection Report")
        End If

    End Sub
End Class
