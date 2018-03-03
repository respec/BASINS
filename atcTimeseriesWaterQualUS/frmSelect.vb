Imports System.Windows.Forms

Public Class frmSelect
    Private pRawDataGroup As clsWQDUSLocations
    Private pSelectedLocations As List(Of String)
    Private pSelectedConstituents As List(Of String)

    Public Sub New(ByVal aRawDataGroup As clsWQDUSLocations,
                   ByRef aSelectedLocations As List(Of String),
                   ByRef aSelectedConstituents As List(Of String))

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        pRawDataGroup = aRawDataGroup
        pSelectedLocations = aSelectedLocations
        pSelectedConstituents = aSelectedConstituents
        PopulateData(pRawDataGroup)
    End Sub

    Public Sub PopulateData(ByVal aRawDataGroup As clsWQDUSLocations)
        Dim lSortedList As New SortedList(Of String, Integer)()
        For Each loc As String In aRawDataGroup.Keys()
            lSortedList.Add(loc, 0)
        Next
        For Each loc As String In lSortedList.Keys
            lstLocations.Items().Add(loc)
        Next
        lSortedList.Clear()
        For Each cons As String In aRawDataGroup.GetUniqueConstituentList()
            lSortedList.Add(cons, 0)
        Next
        For Each cons As String In lSortedList.Keys
            lstConstituents.Items().Add(cons)
        Next
    End Sub

    Private Sub btnClearLoc_Click(sender As Object, e As EventArgs) Handles btnClearLoc.Click
        For Each ind As Integer In lstLocations.CheckedIndices
            lstLocations.SetItemChecked(ind, False)
        Next
    End Sub

    Private Sub btnClearCons_Click(sender As Object, e As EventArgs) Handles btnClearCons.Click
        For Each ind As Integer In lstConstituents.CheckedIndices
            lstConstituents.SetItemChecked(ind, False)
        Next
    End Sub

    Private Sub lstConstituents_ItemCheck(sender As Object, e As ItemCheckEventArgs) Handles lstConstituents.ItemCheck
        If e.NewValue = CheckState.Checked Then
            If Not pSelectedConstituents.Contains(lstConstituents.SelectedItem.ToString()) Then
                pSelectedConstituents.Add(lstConstituents.SelectedItem.ToString())
            End If
        Else
            If pSelectedConstituents.Contains(lstConstituents.SelectedItem.ToString()) Then
                pSelectedConstituents.Remove(lstConstituents.SelectedItem.ToString())
            End If
        End If
    End Sub
    Private Sub lstLocations_ItemCheck(sender As Object, e As ItemCheckEventArgs) Handles lstLocations.ItemCheck
        txtMsgCons.Text = ""
        If e.NewValue = CheckState.Checked Then
            If Not pSelectedLocations.Contains(lstLocations.SelectedItem.ToString()) Then
                pSelectedLocations.Add(lstLocations.SelectedItem.ToString())
            End If
            Dim loc As clsWQDUSLocation = pRawDataGroup.Item(lstLocations.SelectedItem)
            Dim cons As List(Of String) = loc.DataKeys()
            Dim lnalist As New List(Of String)()
            For Each Item As String In lstConstituents.CheckedItems
                If Not cons.Contains(Item) Then
                    lnalist.Add(Item)
                End If
            Next
            Dim lmsg As String = loc.Location & " has none of the following:" & vbCrLf
            If lnalist.Count > 0 Then
                For Each itm As String In lnalist
                    lmsg &= itm & vbCrLf
                Next
                txtMsgCons.Text = lmsg
            End If
        Else
            If pSelectedLocations.Contains(lstLocations.SelectedItem.ToString()) Then
                pSelectedLocations.Remove(lstLocations.SelectedItem.ToString())
            End If
        End If
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        lstLocations.ClearSelected()
        lstConstituents.ClearSelected()
        lstLocations.Items.Clear()
        lstConstituents.Items.Clear()
        Me.DialogResult = DialogResult.Cancel
        Close()
    End Sub

    Private Sub btnDoTser_Click(sender As Object, e As EventArgs) Handles btnDoTser.Click
        Me.DialogResult = DialogResult.OK
        Close()
    End Sub

End Class