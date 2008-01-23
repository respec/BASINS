Public Class frmChooseGraphs

    Private Sub btnAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAll.Click
        For lIndex As Integer = 0 To lstChooseGraphs.Items.Count - 1
            lstChooseGraphs.SetItemChecked(lIndex, True)
        Next
    End Sub

    Private Sub btnNone_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNone.Click
        For lIndex As Integer = 0 To lstChooseGraphs.Items.Count - 1
            lstChooseGraphs.SetItemChecked(lIndex, False)
        Next
    End Sub

End Class