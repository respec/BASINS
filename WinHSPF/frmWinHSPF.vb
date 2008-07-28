Imports atcUCIForms
Imports MapWinUtility

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

    Private Sub FTablesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FTablesToolStripMenuItem.Click

        If pUCI.OpnBlks("RCHRES").Count > 0 Then
            UCIForms.Edit(Me, pUCI.OpnBlks("RCHRES").Ids(0).FTable)
        Else
            Logger.Message("The current project contains no reaches.", "FTable Editor Problem", _
                           MessageBoxButtons.OK, MessageBoxIcon.Information, Windows.Forms.DialogResult.OK)
        End If
    End Sub
End Class
