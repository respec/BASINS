Imports System.Drawing
Imports MapWinUtility
Imports atcUCI
Imports atcControls

Public Class frmInputDataEditor

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.Icon = pIcon

        If pUCI.MetSegs.Count > 0 Then
            pUCI.MetSeg2Source()
        End If
        pUCI.Point2Source()

        Dim lBlockKey As String
        Dim lSectionKey As String
        Dim lTableKey As String
        Dim lPad As String = "    "   'used to make sure whole length is visible when bolded
        For Each lBlock As HspfBlockDef In pMsg.BlockDefs
            lBlockKey = lBlock.Name
            treUci.Nodes.Add(lBlockKey, lBlock.Name & lPad)
            For Each lSection As HspfSectionDef In lBlock.SectionDefs
                If lSection.Name = "<NONE>" Then
                    lSectionKey = lBlock.Name
                Else
                    treUci.SelectedNode = treUci.Nodes(lBlockKey)
                    lSectionKey = lBlock.Name & ":" & lSection.Name
                    treUci.SelectedNode.Nodes.Add(lSectionKey, lSection.Name)
                End If
                For Each lTable As HspfTableDef In lSection.TableDefs
                    If lTable.Name = "<NONE>" Then
                        lTableKey = lSectionKey
                    Else
                        If lSectionKey = lBlock.Name Then
                            treUci.SelectedNode = treUci.Nodes(lSectionKey)
                        Else
                            Dim lNode As TreeNode = treUci.Nodes(lBlock.Name)
                            treUci.SelectedNode = lNode.Nodes(lSectionKey)
                        End If
                        lTableKey = lSectionKey & ":" & lTable.Name
                        treUci.SelectedNode.Nodes.Add(lTableKey, lTable.Name)
                    End If
                Next lTable
            Next lSection
        Next lBlock

        'now bold the ones that are active
        Call BoldActive()

    End Sub

    Private Sub frmInputDataEditor_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        pUCI.Source2MetSeg()
        pUCI.Source2Point()
    End Sub

    Private Sub BoldActive()
        'bold the blocks/tables that are active
        Dim lOpnBlk As HspfOpnBlk
        For Each lNode As TreeNode In treUci.Nodes
            If lNode.Nodes.Count = 0 Then
                'this is a non-operation block
                If lNode.Name = "PATHNAMES" Or lNode.Name = "FORMATS" Then
                    'ElseIf lNode.Name = "CATEGORY" Then
                Else
                    lNode.NodeFont = New System.Drawing.Font(treUci.Font, FontStyle.Bold)
                End If
            Else
                'this is an operation (todo: this could be done recursively)
                lOpnBlk = pUCI.OpnBlks(Trim(lNode.Text))
                If lOpnBlk.Count > 0 Then
                    'this opn exists
                    lNode.NodeFont = New System.Drawing.Font(treUci.Font, FontStyle.Bold)
                    'go through the children of this node
                    For Each lLevel2Node As TreeNode In lNode.Nodes
                        If lLevel2Node.Nodes.Count = 0 Then
                            'these are the actual tables 
                            If lOpnBlk.TableExists(Trim(lLevel2Node.Text)) Then
                                lLevel2Node.NodeFont = New System.Drawing.Font(treUci.Font, FontStyle.Bold)
                            End If
                        Else
                            'this is a section header
                            For Each lLevel3Node As TreeNode In lLevel2Node.Nodes
                                'these are the actual tables 
                                If lOpnBlk.TableExists(Trim(lLevel3Node.Text)) Then
                                    lLevel3Node.NodeFont = New System.Drawing.Font(treUci.Font, FontStyle.Bold)
                                    lLevel2Node.NodeFont = New System.Drawing.Font(treUci.Font, FontStyle.Bold)
                                End If
                            Next
                        End If
                    Next
                End If
            End If
        Next

    End Sub

    Private Sub TableSelected()
        '        Dim lTable As HspfTable
        '        Dim lOpnBlk As HspfOpnBlk
        '        Dim S$, ilen&, iresp&
        '        Dim opname$
        '        Dim lOper As HspfOperation, vOper As Object
        '        Dim M As String

        '        On Error GoTo notFound

        '        If treUci.SelectedItem.Children > 0 Then
        '            'do nothing in this case -- this is not a table name
        '            Exit Sub
        '        Else
        '            'we have selected the table name
        '            S = treUci.SelectedItem.FullPath
        '            ilen = InStr(1, S, "\")
        '            If ilen = 0 Then
        '                opname = S
        '            Else
        '                opname = Mid(S, 1, ilen - 1)
        '            End If
        '            tabname = treUci.SelectedItem
        '        End If

        '        Me.Hide()
        '        If tabname = "GLOBAL" Then
        '            myUci.GlobalBlock.Edit()
        '        ElseIf tabname = "OPN SEQUENCE" Then
        '            myUci.OpnSeqBlock.Edit()
        '        ElseIf tabname = "FILES" Then
        '            myUci.filesblock.Edit()
        '        ElseIf tabname = "CATEGORY" And Not myUci.categoryblock Is Nothing Then
        '            myUci.categoryblock.Edit()
        '        ElseIf tabname = "FTABLES" Then
        '            myUci.OpnBlks("RCHRES").Ids(1).Ftable.Edit()
        '        ElseIf tabname = "MONTH-DATA" And Not myUci.MonthData Is Nothing Then
        '            myUci.MonthData.Edit()
        '        ElseIf tabname = "EXT SOURCES" Then
        '            myUci.Connections(1).EditExtSrc()
        '        ElseIf tabname = "NETWORK" Then
        '            myUci.Connections(1).EditNetwork()
        '        ElseIf tabname = "SCHEMATIC" Then
        '            myUci.Connections(1).EditSchematic()
        '        ElseIf tabname = "EXT TARGETS" Then
        '            myUci.Connections(1).EditExtTar()
        '        ElseIf tabname = "MASS-LINK" Then
        '            myUci.MassLinks(1).Edit()
        '        ElseIf tabname = "SPEC-ACTIONS" And Not myUci.SpecialActionBlk Is Nothing Then
        '            myUci.SpecialActionBlk.Edit()
        '        Else
        '            'regular case
        '            lOpnBlk = myUci.OpnBlks(opname)
        '            If lOpnBlk.Count > 0 Then
        '                'check to see if this table exists
        '                If Not lOpnBlk.TableExists(tabname) Then
        '                    iresp = myMsgBox.Show("Table " & tabname & " does not exist.  Do you want to add it?", "WinHSPF - Input Data Editor", "+&OK", "&Cancel")
        '                    If iresp = 1 Then
        '                        lOpnBlk.AddTableForAll(tabname, opname)
        '                        setDefaultsForTable(myUci, defUci, opname, tabname)
        '                        Call SetMissingValuesToDefaults(myUci, defUci)
        '                        Call BoldActive()
        '                    End If
        '                End If
        '                If lOpnBlk.TableExists(tabname) Then
        '                    lTable = lOpnBlk.tables(tabname)
        '                    lTable.Edit()
        '                    'check for missing tables, add if needed
        '                    CheckAndAddMissingTables(opname)
        '                    CheckAndAddMassLinks()
        '                    Call SetMissingValuesToDefaults(myUci, defUci)
        '                    Call BoldActive()
        '                End If
        '            Else
        '                myMsgBox.Show("No Operations of this type available", "Edit Problem", "+&Ok")
        '            End If
        '        End If
        '        Me.Show(vbModal)
        '        Exit Sub

        'notFound:
        '        M = Err.Description
        '        Err.Clear()
        '        On Error GoTo 0
        '        myMsgBox.Show("Table/Block " & tabname & " not found." & vbCrLf & M, "Edit Problem", "+&Ok")
    End Sub

    Private Sub treUci_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles treUci.DoubleClick
        TableSelected()
    End Sub
End Class