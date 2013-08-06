Public Class frmSelectMet

    Public Choice As String = ""

    Private Sub Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click, Button2.Click, Button3.Click, Button4.Click, Button5.Click, Button6.Click, Button7.Click, Button8.Click, Button10.Click, Button11.Click, Button12.Click
        Dim lButton As Button = sender
        Choice = lButton.Tag
        If String.IsNullOrEmpty(Choice) Then
            Choice = lButton.Text
        End If
        Me.Close()
    End Sub
End Class