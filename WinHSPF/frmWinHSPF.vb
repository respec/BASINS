Imports atcUCIForms

Public Class frmWinHSPF

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Private Sub FilesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FilesToolStripMenuItem.Click
        UCIForms.Edit(pUCI.FilesBlock)
    End Sub
End Class
