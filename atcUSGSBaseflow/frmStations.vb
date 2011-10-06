Imports atcUtility
Imports System.Text.RegularExpressions

Public Class frmStations
    Private pHeaderText As String = "File ""station.txt""" & vbCrLf & _
"This file is read by programs PREP, RECESS, RORA, and PART, to" & vbCrLf & _
"obtain the drainage area.  Note: This file should have ten header" & vbCrLf & _
"lines.  The streamflow file name should be 12 characters or less." & vbCrLf & _
"----------------------------------------------------------------" & vbCrLf & _
"              Drainage" & vbCrLf & _
" Name of       area     The space below, after drainage area, is" & vbCrLf & _
" streamflow   (Square   for optional information that is not read" & vbCrLf & _
" file          miles)   by the programs.  This is free-form." & vbCrLf & _
"---------------------- ------------------------------------------"
    Private pStationList As atcCollection
    Private pFinalSelectedIndex As Integer
    Private pDataIsDirty As Boolean = False

    Public Event StationInfoChanged(ByVal aLastIndex As Integer, ByVal aStationList As atcCollection)

    Private Sub frmStations_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        txtHeader.Text = pHeaderText
    End Sub

    Public Sub New(ByVal aStationsList As atcCollection)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        pStationList = aStationsList
        PopulateStationGrid()
    End Sub

    Private Sub PopulateStationGrid()
        If pStationList.Count = 0 OrElse pStationList Is Nothing Then
            Exit Sub
        End If
        Dim lExtraText As String = ""
        Dim lMatches As MatchCollection
        With dgvStationEntries
            For I As Integer = 0 To pStationList.Count - 1
                .Rows.Add()
                .Item(0, I).Value = CType(pStationList(I), USGSGWStation).Filename
                .Item(1, I).Value = CType(pStationList(I), USGSGWStation).DrainageArea.ToString
                lExtraText = CType(pStationList(I), USGSGWStation).ExtraInfo
                lMatches = Regex.Matches(lExtraText, "\d+")
                If lMatches.Count = 0 Then
                    .Item(2, I).Value = ""
                    .Item(3, I).Value = lExtraText
                Else
                    .Item(2, I).Value = lMatches(0).ToString
                    .Item(3, I).Value = lExtraText.Substring(lMatches(0).ToString.Length).Trim()
                End If
            Next
        End With
    End Sub

    Private Sub btnOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If pFinalSelectedIndex >= 0 Then
            If dgvStationEntries.Rows(pFinalSelectedIndex).Cells(0).EditedFormattedValue Is Nothing OrElse _
               dgvStationEntries.Rows(pFinalSelectedIndex).Cells(1).EditedFormattedValue Is Nothing Then
                pFinalSelectedIndex = -99
            End If
        End If

        Dim lFld1 As String
        Dim lFld2 As String
        Dim lFld3 As String
        Dim lFld4 As String
        Dim lNewStationList As New atcCollection
        For I As Integer = 0 To pStationList.Count - 1
            lNewStationList.Add(CType(pStationList(I), USGSGWStation).Filename, pStationList(I))
        Next
        With dgvStationEntries
            For I As Integer = pStationList.Count To .Rows.Count - 1
                If .Rows(I).Cells(0).EditedFormattedValue.ToString.Trim() <> "" AndAlso _
                   .Rows(I).Cells(1).EditedFormattedValue.ToString.Trim() <> "" Then

                    lFld1 = .Rows(I).Cells(0).EditedFormattedValue.ToString.Trim()
                    lFld2 = .Rows(I).Cells(1).EditedFormattedValue.ToString.Trim()
                    lFld3 = .Rows(I).Cells(2).EditedFormattedValue
                    lFld4 = .Rows(I).Cells(3).EditedFormattedValue

                    Dim lNewStation As New USGSGWStation
                    With lNewStation
                        .Filename = lFld1
                        If Not Double.TryParse(lFld2, .DrainageArea) Then .DrainageArea = -99.99
                        If lFld3 IsNot Nothing Then .ExtraInfo &= lFld3.ToString.Trim()
                        If lFld4 IsNot Nothing Then .ExtraInfo &= lFld4.ToString.Trim()
                    End With
                    lNewStationList.Add(lNewStation.Filename, lNewStation)
                End If
            Next
        End With

        RaiseEvent StationInfoChanged(pFinalSelectedIndex, aStationList:=lNewStationList)
        pStationList.Clear()
        pStationList = Nothing
        Me.Dispose()
        Me.Close()
    End Sub

    Private Sub dgvStationEntries_CellContentClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvStationEntries.CellContentClick
        If e.RowIndex >= 0 Then pFinalSelectedIndex = e.RowIndex
    End Sub

    Private Sub dgvStationEntries_CurrentCellDirtyStateChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgvStationEntries.CurrentCellDirtyStateChanged
        pDataIsDirty = True
    End Sub

    Private Sub dgvStationEntries_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgvStationEntries.RowsAdded
        pDataIsDirty = True
    End Sub
End Class