Option Strict Off
Option Explicit On

Imports MapWinUtility
'Imports atcUCI

Public Class UCIForms

    Public Shared Function Edit(ByVal aParent As Windows.Forms.Form, _
                                ByVal aObject As Object, Optional ByVal aTag As String = "") As Boolean
        Dim lForm As Windows.Forms.Form = Nothing

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
            Case "HspfOpnSeqBlk"
                Dim lFormEdit As New frmEdit(aParent)
                Dim lEditOpnSeqBlock As New ctlEditOpnSeqBlock(aObject, aParent)
                lFormEdit.Text = lEditOpnSeqBlock.Caption
                lFormEdit.EditControl = lEditOpnSeqBlock
                lFormEdit.AddRemoveFlag = True
                lForm = lFormEdit
            Case "HspfFtable"
                Dim lFormEdit As New frmEdit(aParent)
                Dim lEditFTables As New ctlEditFTables(aObject, aParent)
                lFormEdit.Text = lEditFTables.Caption
                lFormEdit.EditControl = lEditFTables
                lFormEdit.AddRemoveFlag = False
                lForm = lFormEdit
            Case "HspfConnection"
                Dim lFormEdit As New frmEdit(aParent)
                Dim lEditConnections As New ctlEditConnections(aObject, aParent, aTag)
                lFormEdit.Text = lEditConnections.Caption
                lFormEdit.EditControl = lEditConnections
                lFormEdit.AddRemoveFlag = True
                lForm = lFormEdit
            Case "HspfCategoryBlk"
                Dim lFormEdit As New frmEdit(aParent)
                Dim lEditCategory As New ctlEditCategory(aObject, aParent)
                lFormEdit.Text = lEditCategory.Caption
                lFormEdit.EditControl = lEditCategory
                lFormEdit.AddRemoveFlag = True
                lForm = lFormEdit
            Case "HspfSpecialActionBlk"
                Dim lFormEdit As New frmEdit(aParent)
                Dim lEditSpecialAction As New ctlEditSpecialAction(aObject, aParent, aTag)
                lFormEdit.Text = lEditSpecialAction.Caption
                lFormEdit.EditControl = lEditSpecialAction
                lFormEdit.AddRemoveFlag = True
                lForm = lFormEdit
            Case "HspfTable"
                Dim lMsgResponse As MsgBoxResult = MsgBoxResult.No
                If aObject.Name = "PWAT-PARM1" Or aObject.Name = "IWAT-PARM1" Or aObject.Name = "HYDR-PARM1" Then
                    'choose regular or deluxe version to edit
                    lMsgResponse = Logger.Msg("Do you want to edit using the enhanced interface for this table?", MsgBoxStyle.YesNo, aObject.Name & " Edit Option")
                End If
                If lMsgResponse = MsgBoxResult.Yes Then
                    If aObject.Name = "PWAT-PARM1" Then
                        '    frmPwatEdit.init(Me, Me.Opn.Uci.icon)
                        '    frmPwatEdit.Show(vbModal)
                    ElseIf aObject.Name = "IWAT-PARM1" Then
                        '    frmIwatEdit.init(Me, Me.Opn.Uci.icon)
                        '    frmIwatEdit.Show(vbModal)
                    ElseIf aObject.Name = "HYDR-PARM1" Then
                        '    frmHydrEdit.init(Me, Me.Opn.Uci.icon)
                        '    frmHydrEdit.Show(vbModal)
                    End If
                    lForm = Nothing
                Else
                    Dim lFormEdit As New frmEdit(aParent)
                    Dim lEditTable As New ctlEditTable(aObject, aParent)
                    lFormEdit.Text = lEditTable.Caption
                    lFormEdit.EditControl = lEditTable
                    lFormEdit.AddRemoveFlag = False
                    lForm = lFormEdit
                End If
            Case "HspfMassLink"
                Dim lFormEdit As New frmEdit(aParent)
                Dim lEditMassLinks As New ctlEditMassLinks(aObject, aParent, aTag)
                lFormEdit.Text = lEditMassLinks.Caption
                lFormEdit.EditControl = lEditMassLinks
                lFormEdit.AddRemoveFlag = True
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
