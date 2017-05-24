Imports atcUtility
Imports MapWinUtility

Public Class frmAddConnection
    Private pUpUCI As atcUCI.HspfUci
    Private pDownUCI As atcUCI.HspfUci

    Public Sub SetUCIs(aUpUCI As atcUCI.HspfUci, aDownUCI As atcUCI.HspfUci)
        pUpUCI = aUpUCI
        pDownUCI = aDownUCI
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
        'connect these 2 reaches
        Dim lUp As Integer = Int(cboUpstream.SelectedItem)
        Dim lDown As Integer = Int(cboDownstream.SelectedItem)
        'prompt for name of transfer wdm
        Dim lTransferWDMName As String = ""
        Dim lFileDialog As New Windows.Forms.OpenFileDialog()
        With lFileDialog
            .Title = "Transfer WDM Name"
            If IO.File.Exists("transfer.wdm") Then
                .FileName = "transfer.wdm"
            End If
            .Filter = "WDM Files (*.wdm) | *.wdm"
            .FilterIndex = 0
            .DefaultExt = "wdm"
            .CheckFileExists = False
            If .ShowDialog(Me) = DialogResult.OK Then
                lTransferWDMName = .FileName
            End If
        End With
        If lTransferWDMName.Length > 0 Then
            Me.Cursor = Cursors.WaitCursor
            AddReachConnections(lTransferWDMName, pUpUCI, lUp, pDownUCI, lDown)
            FileCopy(pUpUCI.Name, pUpUCI.Name & "Save")
            pUpUCI.Save()
            FileCopy(pDownUCI.Name, pDownUCI.Name & "Save")
            pDownUCI.Save()
            Me.Cursor = Cursors.Default
        End If
        Me.Close()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

End Class