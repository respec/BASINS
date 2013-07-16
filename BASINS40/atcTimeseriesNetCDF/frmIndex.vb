Imports MapWinUtility

Public Class frmIndex

    Private Shared pFilter As String = "NetCDF Files (*.nc)|*.nc|All Files (*.*)|*.*"

    Private Sub rdoAggregationFile_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdoAggregationFile.CheckedChanged
        txtGridFileName.Enabled = True
        txtXIndex.Enabled = False
        txtYIndex.Enabled = False
    End Sub

    Private Sub rdoIndices_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdoIndices.CheckedChanged
        txtGridFileName.Enabled = False
        txtXIndex.Enabled = True
        txtYIndex.Enabled = True
    End Sub

    Private Sub frmIndex_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        rdoAggregationFile.Checked = True
    End Sub

    Private Sub txtGridFileName_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtGridFileName.Click
        With New Windows.Forms.OpenFileDialog
            .Title = "Select " & Name & " file to open"
            .Filter = pFilter
            .FilterIndex = 1
            .DefaultExt = ".nc"
            If .ShowDialog() = Windows.Forms.DialogResult.OK Then
                txtGridFileName.Text = .FileName
            End If
        End With

    End Sub
End Class