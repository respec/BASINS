Option Strict Off
Option Explicit On

Imports MapWinUtility
'Imports atcUCI

Public Class UCIForms

    Public Shared Function Edit(ByVal aParent As Windows.Forms.Form, _
                                ByVal aObject As Object) As Boolean
        Dim lForm As Windows.Forms.Form

        Select Case aObject.GetType.Name
            Case "HspfFilesBlk"
                'TODO: don't create multiple forms to edit files block!
                Dim lFormEdit As New frmEdit(aParent)
                Dim lEditFilesBlock As New ctlEditFilesBlock(aObject, aParent)
                lFormEdit.Text = lEditFilesBlock.Caption
                lFormEdit.EditControl = lEditFilesBlock
                lForm = lFormEdit
            Case "HspfGlobalBlk"
                Dim lFormEdit As New frmEdit(aParent)
                Dim lEditGlobalBlock As New ctlEditGlobalBlock(aObject, aParent)
                lFormEdit.Text = lEditGlobalBlock.Caption
                lFormEdit.EditControl = lEditGlobalBlock
                lFormEdit.AddRemoveFlag = False
                lForm = lFormEdit
            Case "HspfFtable"
                Dim lFormEdit As New frmEdit(aParent)
                Dim lEditFTables As New ctlEditFTables(aObject, aParent)
                lFormEdit.Text = lEditFTables.Caption
                lFormEdit.EditControl = lEditFTables
                lFormEdit.AddRemoveFlag = False
                lForm = lFormEdit
            Case Else
                lForm = Nothing
        End Select

        If Not lForm Is Nothing Then
            lForm.Icon = aParent.Icon
            lForm.Show()
        End If
    End Function
End Class
