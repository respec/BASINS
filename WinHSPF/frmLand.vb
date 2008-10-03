Imports System.Drawing
Imports MapWinUtility
Imports atcUCI
Imports atcControls
Imports atcUCIForms
Public Class frmLand
    Dim pOrigTotal As Double

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.Text = "WinHSPF - LandUse Editor"
        Me.Icon = pIcon

    End Sub

    Private Sub agdLand_CommitChange(ByVal ChangeFromRow As Long, ByVal ChangeToRow As Long, ByVal ChangeFromCol As Long, ByVal ChangeToCol As Long)
        Dim t As Double

        t = 1000
        pOrigTotal = 1000

        With grdLand.Source
            'For i = 1 To .Rows
            '    t = t + .CellValue(i, 4)
            'Next
            If t = pOrigTotal Then
                txtTotal.Text = CStr(Format(t, "#####0.00")).PadLeft(18)

                txtLabelOrigTotal.Visible = False
                txtLabelDifference.Visible = False
                txtOrigTotal.Visible = False
                txtDifference.Enabled = False

                '.NET conversion: Switch Differnece label font to ControlText (un-do Red coloring)
                txtDifference.ForeColor = System.Drawing.SystemColors.ControlText
                txtLabelDifference.ForeColor = System.Drawing.SystemColors.ControlText
            Else
                txtLabelOrigTotal.Enabled = True
                txtLabelDifference.Enabled = True
                txtOrigTotal.Enabled = True
                txtDifference.Enabled = True

                txtTotal.Text = CStr(Format(t, "#####0.00")).PadLeft(18)
                txtOrigTotal.Text = CStr(Format(pOrigTotal, "#####0.00")).PadLeft(18)
                txtDifference.Text = CStr(Format(t - pOrigTotal, "#####0.00")).PadLeft(18)

                txtDifference.ForeColor = Color.Red
                txtLabelDifference.ForeColor = Color.Red
            End If
        End With



    End Sub

    Private Sub cmdOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOK.Click
        agdLand_CommitChange(1, 1, 1, 2)
    End Sub
End Class