Imports System.Windows.Forms

Public Class frmStartup

    Private Sub lnkCreateNew_LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lnkCreateNew.LinkClicked, lnkOpenExisting.LinkClicked, lnkOpenLast.LinkClicked
        With CType(Owner, frmWASPSetup)
            If sender Is lnkCreateNew Then
                .mnuNewSelect.PerformClick()
            ElseIf sender Is lnkOpenExisting Then
                .mnuOpenProj.PerformClick()
            Else
                .mnuOpenLast.PerformClick()
            End If
        End With
        Close()
    End Sub

End Class
