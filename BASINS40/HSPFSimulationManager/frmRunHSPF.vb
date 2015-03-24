Imports atcUtility
Imports MapWinUtility

Public Class frmRunHSPF
    Friend SchematicDiagram As HSPFSimulationManager.ctlSchematic

    Private Sub frmRunHSPF_Load(sender As Object, e As EventArgs) Handles Me.Load
        If SchematicDiagram IsNot Nothing Then
            Dim lModels As New List(Of clsIcon)
            Dim lIcon As clsIcon
            For Each lIcon In SchematicDiagram.AllIcons
                lModels.Add(lIcon)
            Next
            Dim lIconIndex As Integer
SortDownstream:
            For lIconIndex = 0 To lModels.Count - 1
                lIcon = lModels(lIconIndex)
                If lIcon.DownstreamIcon IsNot Nothing Then
                    Dim lDownstreamIndex As Integer = lModels.IndexOf(lIcon.DownstreamIcon)
                    If lDownstreamIndex < lIconIndex Then
                        lModels.RemoveAt(lDownstreamIndex)
                        'After removing downstream icon from earlier in the list, lIconIndex now points at location just after lIcon
                        lModels.Insert(lIconIndex, lIcon.DownstreamIcon)
                        GoTo SortDownstream
                    End If
                End If
            Next
            For Each lIcon In lModels
                lstModels.Items.Add(lIcon.WatershedName & ": " & lIcon.UciFileName, lIcon.Selected)
            Next
        End If
    End Sub


    Public Shared Sub RunUCI(ByVal aExeName As String, ByVal aUCIFilename As String)
        Dim lHspfExe As String = atcUtility.FindFile("Please locate " & aExeName, aExeName)
        If Not IO.File.Exists(lHspfExe) OrElse Not lHspfExe.ToLower.EndsWith(aExeName.ToLower) Then
            lHspfExe = atcUtility.FindFile("Please locate " & aExeName, aExeName, , , True)
            If Not IO.File.Exists(lHspfExe) OrElse Not lHspfExe.ToLower.EndsWith(aExeName.ToLower) Then
                Logger.Msg("Unable to locate " & aExeName & ", not running.", aExeName)
                Exit Sub
            End If
        End If

        MsgBox("Run: " & aExeName & " on " & aUCIFilename, MsgBoxStyle.OkOnly, "Skipping RunUCI")
        Exit Sub

        Logger.Status("Running " & aUCIFilename & " (" & lHspfExe & ")")
        MapWinUtility.LaunchProgram(lHspfExe, IO.Path.GetDirectoryName(aUCIFilename), aUCIFilename)
    End Sub

    'Private Sub btnWinHSPF_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnWinHSPF.Click
    '    RunUCI("WinHSPF.exe", cboUciFiles.Text)
    'End Sub

    Private Sub btnRun_Click(sender As Object, e As EventArgs) Handles btnRun.Click
        Dim lNumToRun As Integer = lstModels.CheckedItems.Count
        Dim lFinishedRunning As Integer = 0
        If lstModels.CheckedItems.Count = 0 Then
            Logger.Msg("No models selected to run", "Run HSPF")
        Else
            For Each lSelection As String In lstModels.CheckedItems
                Dim lColonPos As Integer = lSelection.IndexOf(":")
                If lColonPos >= 0 Then
                    Dim lUCIFilename As String = SafeSubstring(lSelection, lColonPos + 1)
CheckUCIExists:
                    If FileExists(lUCIFilename) Then
                        RunUCI("WinHSPFlt.exe", lUCIFilename)
                        Dim lUCIFile As atcUCI.HspfUci = OpenUCI(lUCIFilename)
                        If Not RunComplete(lUCIFile) Then
                            Logger.Msg(lSelection, MsgBoxStyle.Critical, "HSPF Run Did Not Complete")
                            Exit Sub
                        End If
                    Else
                        Select Case Logger.Msg(lUCIFilename, MsgBoxStyle.AbortRetryIgnore, "UCI File Not Found")
                            Case MsgBoxResult.Abort : Exit Sub
                            Case MsgBoxResult.Retry : GoTo CheckUCIExists
                        End Select
                    End If
                End If
                lFinishedRunning += 1
                Logger.Progress(lFinishedRunning, lNumToRun)
            Next
        End If
    End Sub

End Class