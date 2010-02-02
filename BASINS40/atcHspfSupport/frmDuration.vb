Imports atcData
Imports atcUtility

Public Class frmDuration
    Private pTSer As atcTimeseries

    Public Sub Initialize(ByVal aTimeseriesGroup As atcData.atcTimeseriesGroup)
        txtReport.Text = ""
        For Each lTs As atcTimeseries In aTimeseriesGroup
            txtReport.Text &= "Duration Report for " & lTs.ToString & vbCrLf & DurationStats(lTs, GetClassLimits(lTs))
        Next
    End Sub

    Private Sub mnuSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuSave.Click
        Dim cdlg As New Windows.Forms.SaveFileDialog
        With cdlg
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
End Class