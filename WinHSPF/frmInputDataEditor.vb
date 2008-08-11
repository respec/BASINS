Imports System.Drawing
Imports MapWinUtility
Imports atcUCI
Imports atcControls
Imports atcUCIForms

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
        treUci.SelectedNode = treUci.Nodes("GLOBAL")  'make the global the selected node

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

        Dim lTableName As String = ""
        On Error GoTo notFound

        If treUci.SelectedNode.Nodes.Count > 0 Then
            'do nothing in this case -- this is not a table name
            Exit Sub
        Else
            'we have selected the table name
            Dim lName As String = treUci.SelectedNode.Name
            Dim lcolonpos = InStr(1, lName, ":")
            Dim lOperationName As String = ""
            If lcolonpos > 0 Then
                lOperationName = Mid(lName, 1, lcolonpos - 1)
            End If
            lTableName = Trim(treUci.SelectedNode.Text)

            If lOperationName.Length = 0 Then
                EditBlock(Me, lTableName)
            Else
                'regular case
                Dim lOpnBlk As HspfOpnBlk = pUCI.OpnBlks(lOperationName)
                If lOpnBlk.Count > 0 Then
                    'check to see if this table exists
                    If Not lOpnBlk.TableExists(lTableName) Then
                        If Logger.Msg("Table " & lTableName & " does not exist.  Do you want to add it?", MsgBoxStyle.YesNo, "WinHSPF - Input Data Editor") = MsgBoxResult.Ok Then
                            lOpnBlk.AddTableForAll(lTableName, lOperationName)
                            'setDefaultsForTable(myUci, defUci, opname, tabname)
                            'SetMissingValuesToDefaults(myUci, defUci)
                            BoldActive()
                        End If
                    End If
                    If lOpnBlk.TableExists(lTableName) Then
                        Dim lTable As HspfTable = lOpnBlk.Tables(lTableName)
                        UCIForms.Edit(Me, lTable)
                        'check for missing tables, add if needed
                        'CheckAndAddMissingTables(opname)
                        'CheckAndAddMassLinks()
                        'SetMissingValuesToDefaults(myUci, defUci)
                        BoldActive()
                    End If
                Else
                    Logger.Msg("No Operations of this type available", MsgBoxStyle.OkOnly, "Edit Problem")
                End If
            End If
        End If
        Exit Sub

notFound:
        Logger.Msg("Table/Block " & lTableName & " not found." & vbCrLf & Err.Description, MsgBoxStyle.OkOnly, "Edit Problem")
    End Sub

    Private Sub treUci_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles treUci.DoubleClick
        TableSelected()
    End Sub

    Private Sub cmdClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdClose.Click
        pUCI.Source2MetSeg()
        pUCI.Source2Point()
        Me.Dispose()
    End Sub
End Class