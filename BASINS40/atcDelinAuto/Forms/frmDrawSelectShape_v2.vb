' 18.10.09 CWG Point Source option added

Public Class frmDrawSelectShape_v2
    Private myAutomaticForm As frmAutomatic_v2
    Private stopclose As Boolean

    Public Sub Initialize(ByRef autoForm As frmAutomatic_v2, ByVal strText As String, ByVal drawOutletsInlets As Boolean)
        myAutomaticForm = autoForm
        Me.Show()
        myAutomaticForm.Visible = False

        lblInstructions.Text = strText

        If drawOutletsInlets Then
            Me.Height = 106
            rdobtnOutlets.Visible = True
            rdobtnInlets.Visible = True
            chkbxRes.Visible = True
            chkbxSrc.Visible = True
            rdobtnOutlets.Checked = True
        Else
            Me.Height = 62
            rdobtnOutlets.Visible = False
            rdobtnInlets.Visible = False
            chkbxRes.Visible = False
            chkbxSrc.Visible = False
        End If
    End Sub

    Private Sub frmDrawSelectShape_v2_FormClosed(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles MyBase.FormClosed
        If g_SelectingMask Or g_DrawingMask Then
            myAutomaticForm.setMaskSelected()
        End If
        If g_SelectingOutlets Or g_DrawingOutletsOrInlets Then
            myAutomaticForm.setOutletsSelected()
        End If
        g_DrawingOutletsOrInlets = False
        g_SelectingOutlets = False
        g_DrawingMask = False
        g_SelectingMask = False
        Me.Hide()
        myAutomaticForm.Visible = True
    End Sub

    Private Sub btnDone_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDone.Click
        Me.Close()
    End Sub

    Public Sub disableDone(ByVal disable As Boolean)
        stopclose = disable
        btnDone.Enabled = Not disable
    End Sub

    Private Sub frmDrawSelectShape_v2_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        If stopclose Then
            e.Cancel = True
        End If
    End Sub

    Private Sub rdobtnOutlets_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdobtnOutlets.CheckedChanged
        g_DrawingInlets = Not rdobtnOutlets.Checked
        If g_DrawingInlets Then
            chkbxRes.Enabled = False
            chkbxSrc.Enabled = True
            g_DrawingReservoir = False
            g_DrawingPointSource = chkbxSrc.Checked
        Else
            chkbxRes.Enabled = True
            chkbxSrc.Enabled = False
            g_DrawingReservoir = chkbxRes.Checked
            g_DrawingPointSource = False
        End If
    End Sub

    Private Sub chkbxRes_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkbxRes.CheckedChanged
        If Not g_DrawingInlets Then
            g_DrawingReservoir = chkbxRes.Checked
        End If
    End Sub

    Private Sub chkbxSrc_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkbxSrc.CheckedChanged
        If g_DrawingInlets Then
            g_DrawingPointSource = chkbxSrc.Checked
        End If
    End Sub
End Class