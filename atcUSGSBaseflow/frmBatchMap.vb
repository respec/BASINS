Imports atcUtility
Imports atcData

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

    Private Sub treeBFGroups_NodeMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeNodeMouseClickEventArgs) Handles treeBFGroups.NodeMouseClick

    End Sub
End Class