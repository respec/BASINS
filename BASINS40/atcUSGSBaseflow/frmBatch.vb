Imports System.Windows.Forms

Public Class frmBatch

    Private Sub btnBrowse_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowse.Click
        Dim lFileOpenDiag As New OpenFileDialog()
        With lFileOpenDiag
            .DefaultExt = "txt"
            .Filter = ""
            .InitialDirectory = ""
            If .ShowDialog = Windows.Forms.DialogResult.OK Then
                txtSpecFile.Text = .FileName
            End If
        End With
    End Sub

    Private Sub btnDoBatch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDoBatch.Click
        Dim lBatch As New clsBatchBF()
        Dim lBatchConfig As New clsBatchBFSpec()
        lBatchConfig.SpecFilename = txtSpecFile.Text
        lBatchConfig.PopulateScenarios()

    End Sub
End Class