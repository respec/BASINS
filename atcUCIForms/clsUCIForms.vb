Option Strict Off
Option Explicit On

Imports atcUCI

Public Class UCIForms

    Public Shared Function Edit(ByVal aObject As Object, Optional ByVal aModal As Boolean = True) As Boolean
        Dim lForm As Windows.Forms.Form
        Select Case aObject.GetType.Name
            Case "HspfFilesBlk"
                lForm = New frmEdit
            Case Else
                lForm = Nothing
        End Select

        If Not lForm Is Nothing Then
            lForm.Text = aObject.Caption
            If aModal Then
                lForm.ShowDialog()
            Else
                lForm.Show()
            End If
        End If
    End Function
End Class
