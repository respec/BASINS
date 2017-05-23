Imports atcUtility
Imports MapWinUtility

Public Class frmAddConnection

    Private pIcon As clsIcon
    Friend Schematic As ctlSchematic

    Public Sub SetUCIs(aUpUCI As atcUCI.HspfUci, aDownUCI As atcUCI.HspfUci)
        lblUpstream.Text = "Upstream UCI:  " & IO.Path.GetFileName(aUpUCI.Name)
        lblDownstream.Text = "Downstream UCI:  " & IO.Path.GetFileName(aDownUCI.Name)
        cboUpstream.Items.Clear()
        For Each lOper As atcUCI.HspfOperation In aUpUCI.OpnBlks("RCHRES").Ids
            cboUpstream.Items.Add(lOper.Id)
        Next
        cboUpstream.SelectedIndex = cboUpstream.Items.Count - 1
        cboDownstream.Items.Clear()
        For Each lOper As atcUCI.HspfOperation In aDownUCI.OpnBlks("RCHRES").Ids
            cboDownstream.Items.Add(lOper.Id)
        Next
        cboDownstream.SelectedIndex = 0
    End Sub

    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click


        Me.Close()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

End Class