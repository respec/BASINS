Imports atcUtility
Imports atcData
Imports System.Windows.Forms

Public Class frmBatchMap
    Private pListStations As atcCollection
    Private pBatchGroupCount As Integer = 1

    Public Sub Initiate(ByVal aList As atcCollection)
        pListStations = aList
        If pListStations Is Nothing Then pListStations = New atcCollection()
    End Sub

    Private Sub frmBatchMap_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        lstStations.LeftLabel = "Stations from map"
        Dim lindex As Integer = 0
        For Each lStationID As String In pListStations
            lstStations.LeftItem(lindex) = lStationID
            lindex += 1
        Next
    End Sub

    Private Sub btnCreateGroup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCreateGroup.Click
        Dim lnewTreeNode As New Windows.Forms.TreeNode("BatchGroup_" & pBatchGroupCount)
        treeBFGroups.Nodes.Add(lnewTreeNode)
        For I As Integer = 0 To lstStations.RightCount - 1
            With lnewTreeNode
                .Nodes.Add(lstStations.RightItem(I))
            End With
        Next
        pBatchGroupCount += 1
    End Sub

    Private Sub treeBFGroups_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles treeBFGroups.MouseUp
        ' Show menu only if Right Mouse button is clicked
        If e.Button = Windows.Forms.MouseButtons.Right Then
            ' Point where mouse is clicked
            Dim p As System.Drawing.Point = New System.Drawing.Point(e.X, e.Y)
            ' Go to the node that the user clicked
            Dim node As TreeNode = treeBFGroups.GetNodeAt(p)
            If node IsNot Nothing Then
                'm_OldSelectNode = treeBFGroups.SelectedNode
                treeBFGroups.SelectedNode = node
                'If node.Name.StartsWith("BatchGroup_") Then
                'Else
                'End If
                cmsNode.Show(treeBFGroups, p)
                ' Highlight the selected node
                'treeBFGroups.SelectedNode = m_OldSelectNode
                'm_OldSelectNode = Nothing
            Else

            End If
        End If
    End Sub

    Private Sub cmsNode_ItemClicked(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolStripItemClickedEventArgs) Handles cmsNode.ItemClicked
        Dim lFound As String = ""
        Dim p As System.Drawing.Point = New System.Drawing.Point(e.ClickedItem.Bounds.Location.X, e.ClickedItem.Bounds.Location.Y)
        ' Go to the node that the user clicked
        Dim node As TreeNode = treeBFGroups.GetNodeAt(p)
        Dim lParentGroupName As String = node.Parent().Text
        Dim lFullpath As String = node.FullPath
    End Sub
End Class