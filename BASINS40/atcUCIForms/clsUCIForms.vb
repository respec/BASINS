Option Strict Off
Option Explicit On

Imports atcUtility
Imports MapWinUtility
Imports System.Windows.Forms
'Imports atcUCI

Public Class UCIForms

    Public Shared Function Edit(ByVal aParent As Windows.Forms.Form, _
                                ByVal aObject As Object, Optional ByVal aTag As String = "", _
                                Optional ByVal aUsersManualFileName As String = "") As Boolean
        Dim lForm As Windows.Forms.Form = Nothing
        Dim lPage As String = "Users Control Input/FORMAT OF THE USERS CONTROL INPUT/" & aTag & " Block.html"

        Select Case aObject.GetType.Name
            Case "HspfFilesBlk"
                Dim lFormEdit As New frmEdit(aParent, aUsersManualFileName, lPage)
                Dim lEditFilesBlock As New ctlEditFilesBlock(aObject, aParent)
                lFormEdit.Text = lEditFilesBlock.Caption
                lFormEdit.EditControl = lEditFilesBlock
                lFormEdit.MinimumSize = lFormEdit.Size
                lFormEdit.EditFlag = False
                lForm = lFormEdit
            Case "HspfGlobalBlk"
                Dim lFormEdit As New frmEdit(aParent, aUsersManualFileName, lPage)
                Dim lEditGlobalBlock As New ctlEditGlobalBlock(aObject, aParent)
                lFormEdit.Text = lEditGlobalBlock.Caption
                lFormEdit.EditControl = lEditGlobalBlock
                lFormEdit.MinimumSize = lFormEdit.Size
                lFormEdit.AddRemoveFlag = False
                lFormEdit.EditFlag = False
                lForm = lFormEdit
            Case "HspfOpnSeqBlk"
                Dim lFormEdit As New frmEdit(aParent, aUsersManualFileName, lPage)
                Dim lEditOpnSeqBlock As New ctlEditOpnSeqBlock(aObject, aParent)
                lFormEdit.Text = lEditOpnSeqBlock.Caption
                lFormEdit.EditControl = lEditOpnSeqBlock
                lFormEdit.MinimumSize = lFormEdit.Size
                lFormEdit.AddRemoveFlag = True
                lFormEdit.EditFlag = False
                lForm = lFormEdit
            Case "HspfFtable"
                Dim lFormEdit As New frmEdit(aParent, aUsersManualFileName, lPage)
                Dim lEditFTables As New ctlEditFTables(aObject, aParent)
                lFormEdit.Text = lEditFTables.Caption
                lFormEdit.EditControl = lEditFTables
                lFormEdit.MinimumSize = lFormEdit.Size
                lFormEdit.AddRemoveFlag = False
                lFormEdit.EditFlag = False
                lForm = lFormEdit
            Case "HspfConnection"
                If aTag = "SCHEMATIC" Then
                    lPage = "Users Control Input/FORMAT OF THE USERS CONTROL INPUT/TIME SERIES LINKAGES/SCHEMATIC and MASS-LINK Blocks/SCHEMATIC Block.html"
                Else
                    lPage = "Users Control Input/FORMAT OF THE USERS CONTROL INPUT/TIME SERIES LINKAGES/" & aTag & " Block.html"
                End If
                Dim lFormEdit As New frmEdit(aParent, aUsersManualFileName, lPage)
                Dim lEditConnections As New ctlEditConnections(aObject, aParent, aTag)
                lFormEdit.Text = lEditConnections.Caption
                lFormEdit.EditControl = lEditConnections
                lFormEdit.MinimumSize = lFormEdit.Size
                lFormEdit.AddRemoveFlag = True
                lFormEdit.EditFlag = False
                lForm = lFormEdit
            Case "HspfCategoryBlk"
                Dim lFormEdit As New frmEdit(aParent, aUsersManualFileName, lPage)
                Dim lEditCategory As New ctlEditCategory(aObject, aParent)
                lFormEdit.Text = lEditCategory.Caption
                lFormEdit.EditControl = lEditCategory
                lFormEdit.MinimumSize = lFormEdit.Size
                lFormEdit.AddRemoveFlag = True
                lFormEdit.EditFlag = False
                lForm = lFormEdit
            Case "HspfSpecialActionBlk"
                Dim lFormEdit As New frmEdit(aParent, aUsersManualFileName, lPage)
                Dim lEditSpecialAction As New ctlEditSpecialAction(aObject, aParent, aTag)
                lFormEdit.Text = lEditSpecialAction.Caption
                lFormEdit.MinimumSize = lFormEdit.Size
                lFormEdit.EditControl = lEditSpecialAction
                lFormEdit.AddRemoveFlag = True
                lFormEdit.EditFlag = False
                lForm = lFormEdit
            Case "HspfTable"
                Dim lMsgResponse As MsgBoxResult = MsgBoxResult.No
                If aObject.Name = "PWAT-PARM1" Or aObject.Name = "IWAT-PARM1" Or aObject.Name = "HYDR-PARM1" Then
                    'choose regular or deluxe version to edit
                    lMsgResponse = Logger.Msg("Do you want to edit using the enhanced interface for this table?", MsgBoxStyle.YesNo, aObject.Name & " Edit Option")
                End If
                If lMsgResponse = MsgBoxResult.Yes Then
                    If aObject.Name = "PWAT-PARM1" Then
                        Dim frmPwatEdit As New frmPwatEdit
                        frmPwatEdit.Icon = aParent.Icon
                        frmPwatEdit.Init(aObject)
                        frmPwatEdit.ShowDialog()
                    ElseIf aObject.Name = "IWAT-PARM1" Then
                        Dim frmIwatEdit As New frmIwatEdit
                        frmIwatEdit.Icon = aParent.Icon
                        frmIwatEdit.Init(aObject)
                        frmIwatEdit.ShowDialog()
                    ElseIf aObject.Name = "HYDR-PARM1" Then
                        Dim frmHydrEdit As New frmHydrEdit
                        frmHydrEdit.Icon = aParent.Icon
                        frmHydrEdit.Init(aObject)
                        frmHydrEdit.ShowDialog()
                    End If
                    lForm = Nothing
                Else
                    Dim lName1 As String = MapWinUtility.Strings.StrSplit(aTag, ":", "")
                    Dim lName2 As String = MapWinUtility.Strings.StrSplit(aTag, ":", "")
                    Dim lName3 As String = MapWinUtility.Strings.StrSplit(aTag, ":", "")
                    If lName3.Length > 0 Then
                        lPage = "Users Control Input/FORMAT OF THE USERS CONTROL INPUT/" & lName1 & " Block/" & lName2 & " input/" & lName3 & ".html"
                    Else
                        lPage = "Users Control Input/FORMAT OF THE USERS CONTROL INPUT/" & lName1 & " Block/" & lName2 & ".html"
                    End If
                    Dim lFormEdit As New frmEdit(aParent, aUsersManualFileName, lPage)
                    Dim lEditTable As New ctlEditTable(aObject, aParent)
                    lFormEdit.Text = lEditTable.Caption
                    lFormEdit.EditControl = lEditTable
                    lFormEdit.AddRemoveFlag = False
                    lFormEdit.EditFlag = False
                    lForm = lFormEdit
                End If
            Case "HspfMassLink"
                    lPage = "Users Control Input/FORMAT OF THE USERS CONTROL INPUT/TIME SERIES LINKAGES/SCHEMATIC and MASS-LINK Blocks/MASS-LINK Block.html"
                    Dim lFormEdit As New frmEdit(aParent, aUsersManualFileName, lPage)
                    Dim lEditMassLinks As New ctlEditMassLinks(aObject, aParent, aTag)
                    lFormEdit.Text = lEditMassLinks.Caption
                    lFormEdit.MinimumSize = New System.Drawing.Size(650, 428)
                    lFormEdit.EditControl = lEditMassLinks
                    lFormEdit.AddRemoveFlag = True
                    lFormEdit.EditFlag = False
                    lForm = lFormEdit
            Case "HspfMonthData"
                    Dim lFormEdit As New frmEdit(aParent, aUsersManualFileName, lPage)
                    Dim lEditMonthData As New ctlEditMonthData(aObject, aParent)
                    lFormEdit.Text = lEditMonthData.Caption
                    lFormEdit.EditControl = lEditMonthData
                    lFormEdit.MinimumSize = lFormEdit.Size
                    lFormEdit.AddRemoveFlag = True
                    lFormEdit.EditFlag = False
                    lForm = lFormEdit
            Case Else
                    lForm = Nothing
        End Select

        If Not lForm Is Nothing Then

            lForm.Icon = aParent.Icon
            lForm.ShowDialog()

        End If
    End Function
End Class
