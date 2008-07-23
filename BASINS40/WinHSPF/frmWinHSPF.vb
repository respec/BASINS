Imports atcUCIForms

Public Class frmWinHSPF

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        'go ahead and load UCI for now
        OpenUCI()
        'set UCI name in caption
        Me.Text = Me.Text & ": " & pUCI.Name

    End Sub

    Private Sub GlobalToolStripMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles GlobalToolStripMenuItem.Click
        UCIForms.Edit(Me, pUCI.GlobalBlock)
    End Sub

    Private Sub FilesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FilesToolStripMenuItem.Click
        UCIForms.Edit(Me, pUCI.FilesBlock)
    End Sub

    Private Sub ReachEditorToolStripMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ReachEditorToolStripMenuItem.Click
        ReachEditor()
    End Sub

    Private Sub LandUseEditorToolStripMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LandUseEditorToolStripMenuItem.Click

    End Sub

End Class
