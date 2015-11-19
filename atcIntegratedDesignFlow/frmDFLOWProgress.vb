Public Class frmDFLOWProgress

    Private Sub frmDFLOWProgress_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        e.Cancel = True
        'If (MsgBox("Are you sure you want to close this DFLOW analysis" & vbCrLf & "and return to the main BASINS form?", MsgBoxStyle.YesNo, "Cancel DFLOW Analysis") = MsgBoxResult.No) Then
        '    e.Cancel = False
        'Else
        '    e.Cancel = True
        '    Me.Owner.Close()
        'End If
    End Sub
End Class