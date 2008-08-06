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

    End Sub

    Private Sub ReachEditorToolStripMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ReachEditorToolStripMenuItem.Click
        ReachEditor()
    End Sub

    Private Sub LandUseEditorToolStripMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LandUseEditorToolStripMenuItem.Click

    End Sub

    Private Sub InputDataEditorToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles InputDataEditorToolStripMenuItem.Click
        frmInputDataEditor.Show()
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
            Dim lTableName As String = e.ClickedItem.Text
            If pUCI.MetSegs.Count > 0 Then
                pUCI.MetSeg2Source()
            End If
            pUCI.Point2Source()

            If lTableName = "GLOBAL" Then
                UCIForms.Edit(Me, pUCI.GlobalBlock)
            ElseIf lTableName = "OPN SEQUENCE" Then

            ElseIf lTableName = "FILES" Then
                UCIForms.Edit(Me, pUCI.FilesBlock)
            ElseIf lTableName = "CATEGORY" Then

            ElseIf lTableName = "FTABLES" Then
                If pUCI.OpnBlks("RCHRES").Count > 0 Then
                    UCIForms.Edit(Me, pUCI.OpnBlks("RCHRES").Ids(0).FTable)
                Else
                    Logger.Message("The current project contains no reaches.", "FTable Editor Problem", _
                                   MessageBoxButtons.OK, MessageBoxIcon.Information, Windows.Forms.DialogResult.OK)
                End If
            ElseIf lTableName = "MONTH-DATA" Then

            ElseIf lTableName = "EXT SOURCES" Then
                UCIForms.Edit(Me, pUCI.Connections(0), lTableName)
            ElseIf lTableName = "NETWORK" Then
                UCIForms.Edit(Me, pUCI.Connections(0), lTableName)
            ElseIf lTableName = "SCHEMATIC" Then
                UCIForms.Edit(Me, pUCI.Connections(0), lTableName)
            ElseIf lTableName = "EXT TARGETS" Then
                UCIForms.Edit(Me, pUCI.Connections(0), lTableName)
            ElseIf lTableName = "MASS-LINK" Then

            ElseIf lTableName = "SPEC-ACTIONS" Then

            Else
                'DisableAll(True)
                Logger.Msg("Table/Block " & lTableName & " not found.", MsgBoxStyle.OkOnly, "Edit Problem")
                'DisableAll(False)
            End If
            pUCI.Source2MetSeg()
            pUCI.Source2Point()

            'ClearTree()
            'BuildTree()
            'UpdateLegend()
            'UpdateDetails()
        End If
    End Sub

End Class
