Imports atcUtility
Imports MapWinUtility

Public Class frmWASPConstituents

    Dim pWASPConstituents As New atcUtility.atcCollection
    Dim pfrmWASPSetup As frmWASPSetup

    Private Sub cmdOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOK.Click
        pWASPConstituents.Clear()

        For Each lCons As String In lstConstituents.Items
            If Not pWASPConstituents.Contains(lCons) Then
                pWASPConstituents.Add(lCons)
            End If
        Next

        pfrmWASPSetup.pPlugIn.WASPProject.WASPConstituents = pWASPConstituents
        pfrmWASPSetup.SetLoadStationGrid()

        Me.Dispose()
    End Sub

    Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        Me.Dispose()
    End Sub

    Private Sub cmdAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAdd.Click
        If txtAdd.Text.ToString.Trim.Length > 0 Then
            lstConstituents.Items.Add(txtAdd.Text.ToString.Trim)
        End If
    End Sub

    Public Sub Init(ByVal aWASPConstituents As atcCollection, ByVal afrmWASPSetup As frmWASPSetup)
        pWASPConstituents = aWASPConstituents
        pfrmWASPSetup = afrmWASPSetup

        For Each lCons As String In aWASPConstituents
            lstConstituents.Items.Add(lCons)
        Next
    End Sub

    Private Sub cmdRemove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdRemove.Click
        If lstConstituents.SelectedItems.Count > 0 Then
            For Each lIndex As Integer In lstConstituents.SelectedIndices
                lstConstituents.Items.RemoveAt(lIndex)
            Next
        Else
            Logger.Msg("Nothing is selected in the list.  To remove an item from the list, select the item and then click 'Remove'.", MsgBoxStyle.OkOnly, "Remove Problem")
        End If
    End Sub

End Class