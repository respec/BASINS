Imports System.Windows.Forms
Imports System.IO
Imports System.Text
Imports atcData

Public Class frmSelect
    Private pRawDataGroup As clsWQDUSLocations
    Private pSelectedLocations As List(Of String)
    Private pSelectedConstituents As List(Of String)
    Private pAllLocationsSelected As Boolean = False

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
        Dim lSortedListPreferred As New SortedList(Of String, Integer)()
        For Each cons As String In aRawDataGroup.GetUniqueConstituentList()
            If clsWQDUSPreferred.IsPreferredWaterQuality(cons) Then
                If Not lSortedListPreferred.ContainsKey(cons) Then
                    lSortedListPreferred.Add(cons, 0)
                End If
            Else
                If Not lSortedList.ContainsKey(cons) Then
                    lSortedList.Add(cons, 0)
                End If
            End If
        Next
        For Each cons As String In lSortedListPreferred.Keys()
            lstConstituents.Items().Add(cons)
        Next
        For Each cons As String In lSortedList.Keys()
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
        If Not pAllLocationsSelected Then
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
        If cbxSave.Checked Then
            SaveSelected()
        End If
        Me.DialogResult = DialogResult.OK
        Close()
    End Sub

    Private Sub btnSelectAllCons_Click(sender As Object, e As EventArgs) Handles btnSelectAllCons.Click
        For ind As Integer = 0 To lstConstituents.Items.Count - 1
            lstConstituents.SelectedItem = lstConstituents.Items(ind)
            pSelectedConstituents.Add(lstConstituents.Items(ind))
            lstConstituents.SetItemChecked(ind, True)
        Next
    End Sub

    Private Sub btnSelectAllLoc_Click(sender As Object, e As EventArgs) Handles btnSelectAllLoc.Click
        pAllLocationsSelected = True
        For ind As Integer = 0 To lstLocations.Items.Count - 1
            lstLocations.SelectedItem = lstLocations.Items(ind)
            pSelectedLocations.Add(lstLocations.Items(ind))
            lstLocations.SetItemChecked(ind, True)
        Next
        pAllLocationsSelected = False
    End Sub

    Private Sub SaveSelected()
        'open the original file
        Dim lSB As New StringBuilder
        Dim lStreamReader As New StreamReader(pRawDataGroup.FileName)
        Dim lCurrentRecord As String
        lCurrentRecord = lStreamReader.ReadLine 'first line is header
        lSB.AppendLine(lCurrentRecord)
        Do
            lCurrentRecord = lStreamReader.ReadLine
            If lCurrentRecord Is Nothing Then
                Exit Do
            Else
                Dim lAppend As Boolean = False
                For Each lLoc As String In pSelectedLocations
                    If lCurrentRecord.Contains(lLoc) Then
                        For Each lCon As String In pSelectedConstituents
                            Dim lConShort As String = lCon.Remove(lCon.IndexOf("-"))
                            If lCurrentRecord.Contains(lConShort) Then
                                lAppend = True
                                Exit For
                            End If
                        Next
                    End If
                Next
                If lAppend Then
                    lSB.AppendLine(lCurrentRecord)
                End If
            End If
        Loop
        atcUtility.SaveFileString(pRawDataGroup.FileName & ".filtered", lSB.ToString)

        ''rename file in project
        'Dim lDataSource As atcTimeseriesWaterQualUS = Nothing
        'lDataSource = atcDataManager.DataSourceBySpecification(pRawDataGroup.FileName)
        'If lDataSource IsNot Nothing Then
        '    lDataSource.Specification = pRawDataGroup.FileName & ".filtered"
        'End If
    End Sub
End Class