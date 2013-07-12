Public Class frmIndex

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
End Class