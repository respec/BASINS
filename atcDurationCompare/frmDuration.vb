Imports atcData
Imports atcUtility
Imports atcDurationCompare

Public Class frmDuration
    Private WithEvents pDataGroup As atcTimeseriesGroup
    Public Sub Initialize(ByVal aTimeseriesGroup As atcData.atcTimeseriesGroup)
        pDataGroup = aTimeseriesGroup

        Dim lReport As New DurationReport
        With txtReport
            .Text = ""
            For Each lTs As atcTimeseries In aTimeseriesGroup
                .Text &= "Duration Report for " & lTs.ToString & vbCrLf & vbCrLf & DurationStats(lTs, lReport)
            Next
            .SelectionStart = 0
            .SelectionLength = 0
        End With
    End Sub

    Private Sub mnuSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim lSaveFileDialog As New Windows.Forms.SaveFileDialog
        With lSaveFileDialog
            .Title = "Save Duration Report As..."
            .DefaultExt = "txt"
            .Filter = "Text Files|*.txt|All Files|*.*"
            .FilterIndex = 0
            If .ShowDialog = Windows.Forms.DialogResult.OK Then
                IO.File.WriteAllText(.FileName, txtReport.Text)
                OpenFile(.FileName)
            End If
        End With
    End Sub

    Public Sub mnuAnalysis_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        atcDataManager.ShowDisplay(sender.Text, pDataGroup)
    End Sub

    Private pHelpLocation As String = "BASINS Details\Analysis\Time Series Functions\Duration.html"
    Private Sub mnuHelp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuHelp.Click
        ShowHelp(pHelpLocation)
    End Sub
End Class