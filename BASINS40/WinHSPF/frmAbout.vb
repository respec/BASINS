Public Class frmAbout
    Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.Icon = pIcon
        Me.MinimumSize = Me.Size
        Me.MaximumSize = Me.Size

        PictureBox1.Image = ImageList1.Images(0)
        PictureBox2.Image = ImageList1.Images(1)
        PictureBox1.Cursor = Windows.Forms.Cursors.Hand
        PictureBox2.Cursor = Windows.Forms.Cursors.Hand

    End Sub

    Private Sub cmdClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdClose.Click
        Me.Dispose()
    End Sub

    Private Sub PictureBox1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox1.Click
        System.Diagnostics.Process.Start("http://www.epa.gov/OST/BASINS/")
    End Sub

    Private Sub PictureBox2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox2.Click
        System.Diagnostics.Process.Start("http://www.aquaterra.com")
    End Sub

End Class