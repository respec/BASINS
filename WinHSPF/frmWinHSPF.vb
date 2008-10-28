Imports atcUCIForms
Imports MapWinUtility
Imports atcUCI

Public Class frmWinHSPF

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        'set edit menu
        SetEditMenu()

        'go ahead and load UCI for now
        OpenUCI()
        'set UCI name in caption
        Me.Text = Me.Text & ": " & pUCI.Name

        'Set the tool tips when mouse-over buttons occur

    End Sub

    Private Sub ReachEditorToolStripMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ReachEditorToolStripMenuItem.Click
        ReachEditor()
    End Sub

    Private Sub LandUseEditorToolStripMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LandUseEditorToolStripMenuItem.Click
        frmLand.Show()
    End Sub

    Private Sub InputDataEditorToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles InputDataEditorToolStripMenuItem.Click
        frmInputDataEditor.Show()
    End Sub

    Private Sub PollutantToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PollutantToolStripMenuItem.Click
        PollutantSelectorCheck()
    End Sub

    Private Sub SetEditMenu()
        Dim lAddFlag As Boolean
        For Each lBlock As HspfBlockDef In pMsg.BlockDefs
            lAddFlag = False
            If lBlock.SectionDefs(0).TableDefs.Count = 0 Then
                lAddFlag = True
            ElseIf lBlock.SectionDefs(0).TableDefs.Count = 1 Then
                If lBlock.SectionDefs(0).TableDefs(0).Name = "<NONE>" Then
                    lAddFlag = True
                End If
            End If
            If lAddFlag Then
                Me.EditToolStripMenuItem.DropDownItems.Add(lBlock.Name)
            End If
        Next lBlock
    End Sub

    Private Sub EditToolStripMenuItem_DropDownItemClicked(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolStripItemClickedEventArgs) Handles EditToolStripMenuItem.DropDownItemClicked

        If pUCI.Name.Length < 1 Then
            'DisableAll(True)
            Logger.Msg("No Project is active." & vbCrLf & vbCrLf & _
                       "Open or Create a Project before selecting this menu item.", MsgBoxStyle.OkOnly, _
                       "WinHSPF: Edit Problem")
            'DisableAll(False)
        Else
            If pUCI.MetSegs.Count > 0 Then
                pUCI.MetSeg2Source()
            End If
            pUCI.Point2Source()

            EditBlock(Me, e.ClickedItem.Text)

            pUCI.Source2MetSeg()
            pUCI.Source2Point()

            'ClearTree()
            'BuildTree()
            'UpdateLegend()
            'UpdateDetails()
        End If
    End Sub

    Private Sub ToolStripButton2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton2.Click
        frmActivityAll.Show()
    End Sub

    Private Sub ToolStripButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton1.Click
        frmOutput.Show()
    End Sub
End Class
