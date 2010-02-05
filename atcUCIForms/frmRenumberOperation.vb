Imports atcUCI
Imports atcUtility
Imports MapWinUtility

Public Class frmRenumberOperation

    Dim pUci As HspfUci
    Dim pParentForm As Object

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        
    End Sub

    Friend Sub Init(ByVal aHspfOpnSeqBlk As HspfOpnSeqBlk, ByVal aParentForm As Windows.Forms.Form)
        pUci = aHspfOpnSeqBlk.Uci
        pParentForm = aParentForm
        Me.Icon = aParentForm.Icon

        cboOperationType.Items.Clear()
        For Each lOpn As HspfOperation In pUci.OpnSeqBlock.Opns
            cboOperation.Items.Add(lOpn.Name & " " & lOpn.Id)
        Next
        atxNew.Text = 1
        cboOperation.SelectedIndex = 0
    End Sub

    Private Sub cmdOK1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOK1.Click
        Dim lSelectedText As String = cboOperation.SelectedItem
        Dim lOperName As String = StrRetRem(lSelectedText)
        Dim lOldId As Integer = Int(lSelectedText)
        Dim lNewId As Integer = atxNew.Text

        For Each lOpn As HspfOperation In pUci.OpnSeqBlock.Opns
            If lOpn.Name = lOperName And lOpn.Id = lOldId Then
                lOpn.Id = lNewId
            End If
        Next

        Me.Dispose()
    End Sub

    Private Sub cmdClose1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdClose1.Click
        Me.Dispose()
    End Sub
End Class