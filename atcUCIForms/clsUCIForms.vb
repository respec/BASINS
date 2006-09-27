Option Strict Off
Option Explicit On

Imports atcUCI

Public Class UCIForms

    Public Shared Function Edit(ByVal aParent As Windows.Forms.Form, _
                                ByVal aObject As Object, _
                                Optional ByVal aModal As Boolean = True) As Boolean
        Dim lForm As Windows.Forms.Form
        Select Case aObject.GetType.Name
            Case "HspfFilesBlk"
                Dim lFormEdit As New frmEdit
                lFormEdit.EditControl = New ctlEditFilesBlock(aObject)
                lForm = lFormEdit
            Case Else
                lForm = Nothing
        End Select

        If Not lForm Is Nothing Then
            lForm.Text = aObject.Caption
            lForm.Icon = aParent.Icon
            If aModal Then
                lForm.ShowDialog()
            Else
                lForm.Show()
            End If
        End If
    End Function
End Class
