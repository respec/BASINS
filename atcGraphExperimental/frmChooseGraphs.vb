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

    Private Sub cbxMultiple_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbxMultiple.CheckedChanged
        If cbxMultiple.Checked = True Then
            'remove any warnings about needing just 2 data sets
            For lIndex As Integer = 0 To lstChooseGraphs.Items.Count - 1
                Dim lPos As Integer = InStr(lstChooseGraphs.Items(lIndex), "(two datasets needed")
                If lPos > 0 Then
                    lstChooseGraphs.Items(lIndex) = Mid(lstChooseGraphs.Items(lIndex), 1, (lPos - 2))
                End If
            Next
        End If
    End Sub
End Class