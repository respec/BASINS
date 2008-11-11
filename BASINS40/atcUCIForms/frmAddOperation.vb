Imports atcUCI

Public Class frmAddOperation

    Dim pUci As HspfOpnSeqBlk
    Dim pCtl As Windows.Forms.Form

    Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.MinimumSize = Me.Size
        Me.MaximumSize = Me.Size

    End Sub

    Friend Sub Init(ByVal aHspfOpnSeqBlk As HspfOpnSeqBlk, ByVal aCtl As Windows.Forms.Form)
        pUci = aHspfOpnSeqBlk
        pCtl = aCtl
        Me.Icon = aCtl.Icon
    End Sub

    Private Sub cmdClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdClose.Click
        Me.Dispose()
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Me.Dispose()
    End Sub
End Class