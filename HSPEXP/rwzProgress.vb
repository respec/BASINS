Public Class RWZProgress

    Private Sub cmdExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdExit.Click
        Me.Close()
        StartUp.Close()
    End Sub

    Private Sub txtProgress_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtProgress.TextChanged
        txtProgress.SelectAll()
        txtProgress.ScrollToCaret()
        txtProgress.DeselectAll()
    End Sub
End Class