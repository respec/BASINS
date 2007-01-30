Public Class frmChooseColumns

    Public Function AskUser(ByVal aAvailable() As String, ByVal aSelected() As String) As String()
        Dim lSelected() As String = aSelected
        Dim lCurrentIndex As Integer

        lstColumns.Items.Clear()
        lstColumns.Items.AddRange(aAvailable)

        'Select items in aSelected
        For lCurrentIndex = 0 To aAvailable.GetUpperBound(0)
            If Array.IndexOf(aSelected, aAvailable(lCurrentIndex)) >= 0 Then
                lstColumns.SetItemChecked(lCurrentIndex, True)
            End If
        Next

        If Me.ShowDialog() = Windows.Forms.DialogResult.OK Then
            ReDim lSelected(lstColumns.CheckedIndices.Count - 1)
            lCurrentIndex = 0
            For Each lCheckedIndex As Integer In lstColumns.CheckedIndices
                lSelected(lCurrentIndex) = aAvailable(lCheckedIndex)
                lCurrentIndex += 1
            Next
        End If

        Return lSelected
    End Function

    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub btnAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAll.Click
        For lCurrentIndex As Integer = 0 To lstColumns.Items.Count - 1
            lstColumns.SetItemChecked(lCurrentIndex, True)
        Next
    End Sub
End Class