Public Class ctlStatus

    Public Property Label(ByVal aIndex As Integer) As String
        Get
            Select Case aIndex
                Case 0 : Return Me.Text
                Case 1 : Return lblTop.Text
                Case 2 : Return lblLeft.Text
                Case 3 : Return lblMiddle.Text
                Case 4 : Return lblRight.Text
                Case 5 : Return lblBottom.Text
                Case Else : Return ""
            End Select
        End Get
        Set(ByVal aNewValue As String)
            Select Case aIndex
                Case 0 : Me.Text = aNewValue
                Case 1 : lblTop.Text = aNewValue
                Case 2 : lblLeft.Text = aNewValue
                Case 3 : lblMiddle.Text = aNewValue
                Case 4 : lblRight.Text = aNewValue : lblRight.Left = Me.ClientRectangle.Width - lblRight.Width - lblLeft.Left
                Case 5 : lblBottom.Text = aNewValue
            End Select
        End Set
    End Property

    Private Sub ctlStatus_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
        Try
            lblRight.Left = Me.ClientRectangle.Width - lblRight.Width ' - lblLeft.Left
            Dim lControlWidth As Integer = Me.ClientRectangle.Width '- Progress.Left * 2
            lblMiddle.Width = lControlWidth
            Progress.Width = lControlWidth
        Catch
        End Try
    End Sub
End Class
