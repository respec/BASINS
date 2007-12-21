Public Class frmChooseGraphs

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        lstChooseGraphs.Items.Clear()
        Me.Close()
    End Sub

    Private Sub btnGenerate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGenerate.Click
        Me.Close()
    End Sub

End Class