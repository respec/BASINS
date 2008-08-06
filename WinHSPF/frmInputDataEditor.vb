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
        For Each lBlock As HspfBlockDef In pMsg.BlockDefs
            lBlockKey = lBlock.Name
            treUci.Nodes.Add(lBlockKey, lBlock.Name)
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
        'Call BoldActive()

    End Sub

    Private Sub frmInputDataEditor_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        pUCI.Source2MetSeg()
        pUCI.Source2Point()
    End Sub

End Class