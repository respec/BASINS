Public Class frmBMP

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        Me.Icon = pIcon
        Me.MinimumSize = Me.Size
        Me.MaximumSize = Me.Size

    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdBMPEffic.Click

        If IsNothing(pfrmBMPEffic) Then
            pfrmBMPEffic = New frmBMPEffic
            pfrmBMPEffic.Show()
        Else
            If pfrmBMPEffic.IsDisposed Then
                pfrmBMPEffic = New frmBMPEffic
                pfrmBMPEffic.Show()
            Else
                pfrmBMPEffic.WindowState = FormWindowState.Normal
                pfrmBMPEffic.BringToFront()
            End If
        End If

    End Sub
End Class