Imports atcUCI
Imports atcUtility

Public Class frmBMPTools

    Dim pHspfFtable As HspfFtable

    Private Sub cmdOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOpen.Click
        atcFtableBuilder.clsGlobals.gToolType = atcFtableBuilder.clsGlobals.ToolType.Gray
        atcFtableBuilder.clsGlobals.pFTable = pHspfFtable
        atcFtableBuilder.clsGlobals.pHelpManualName = pWinHSPFManualName
        atcFtableBuilder.clsGlobals.pHelpManualPage = "User's Guide\Detailed Functions\BMP Reach Toolkit.html"
        Dim lBuilder As New atcFtableBuilder.mainForm
        lBuilder.ShowDialog()
    End Sub

    Private Sub cmdLID_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdLID.Click
        atcFtableBuilder.clsGlobals.gToolType = atcFtableBuilder.clsGlobals.ToolType.Green
        atcFtableBuilder.clsGlobals.pFTable = pHspfFtable
        atcFtableBuilder.clsGlobals.pHelpManualName = pWinHSPFManualName
        atcFtableBuilder.clsGlobals.pHelpManualPage = "User's Guide\Detailed Functions\BMP Reach Toolkit.html"
        Dim lBuilder As New atcFtableBuilder.mainForm
        lBuilder.ShowDialog()
    End Sub

    Private Sub frmBMPTools_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyValue = Windows.Forms.Keys.F1 Then
            ShowHelp(pWinHSPFManualName)
            ShowHelp("User's Guide\Detailed Functions\BMP Reach Toolkit.html")
        End If
    End Sub

    Private Sub frmBMPTools_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim lOper As HspfOperation
        For Each lOper In pUCI.OpnBlks("RCHRES").Ids
            cboID.Items.Add(lOper.Id & " - " & lOper.Description)
        Next
        cboID.SelectedIndex = 0
    End Sub

    Private Sub cboID_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboID.SelectedIndexChanged
        Dim tempID As Integer
        Dim lOper As Integer
        lOper = InStr(1, cboID.SelectedItem, "-")
        If lOper > 0 Then
            tempID = CInt(Mid(cboID.SelectedItem, 1, lOper - 2))
        Else
            tempID = CInt(cboID.SelectedItem)
        End If
        pHspfFtable = pUCI.OpnBlks("RCHRES").Ids("K" & tempID).FTable
    End Sub
End Class