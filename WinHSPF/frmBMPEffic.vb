Public Class frmBMPEffic

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        Me.Icon = pIcon
        Me.MinimumSize = Me.Size

    End Sub

    Private Sub cmdClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdClose.Click
        Me.Dispose()
    End Sub
End Class