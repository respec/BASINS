Imports atcData
Imports atcUtility

Friend Class frmBLM
    Private pPlugIn As BasinsBLM.PlugIn

    Friend Sub New(ByVal aPlugIn As BasinsBLM.PlugIn, Optional ByRef aTimeseriesGroup As atcData.atcTimeseriesGroup = Nothing)
        MyBase.New()
        InitializeComponent() 'required by Windows Form Designer

        pPlugIn = aPlugIn

        If aTimeseriesGroup Is Nothing Then
            aTimeseriesGroup = atcDataManager.DataSets()
        End If

        If aTimeseriesGroup.Count > 0 Then
            PopulateCriteriaList(aTimeseriesGroup)
            'Dim DisplayPlugins As ICollection = atcDataManager.GetPlugins(GetType(atcDataDisplay))
            'For Each ldisp As atcDataDisplay In DisplayPlugins
            '    Dim lMenuText As String = ldisp.Name
            '    If lMenuText.StartsWith("Analysis::") Then lMenuText = lMenuText.Substring(10)
            '    'mnuAnalysis.MenuItems.Add(lMenuText, New EventHandler(AddressOf mnuAnalysis_Click))
            'Next
        Else 'user declined to specify Data
            Me.Close()
        End If
    End Sub

    Private Sub btnRun_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRun.Click
        Dim lSource As ListSource = atcGridLocation.Source
        For lIndex As Integer = 0 To lSource.SelectedItems.Count - 1
            Dim lRowIndex As Integer = lSource.SelectedItems.Keys(lIndex)
            pPlugIn.RunBLM(lSource.CellValue(lRowIndex, 0), lSource.CellValue(lRowIndex, 1))
        Next
    End Sub

    Private Sub PopulateCriteriaList(ByVal aDataGroup As atcTimeseriesGroup)
        Dim lAttributeName As String = "Location"

        Dim lLocationList As New atcCollection
        Dim lLocationName As New atcCollection
        For Each lDataSet As atcDataSet In aDataGroup
            Dim lLocation As String = lDataSet.Attributes.GetValue(lAttributeName, "")
            If lLocationList.Increment(lLocation, 1) = 1 Then
                lLocationName.Add(lLocation, lDataSet.Attributes.GetValue("StaNam", ""))
            End If
        Next

        Dim lLocationListOK As New atcCollection
        For Each lLocation As String In lLocationList.Keys
            If lLocationList.ItemByKey(lLocation) > 8 Then 'have enough data (maybe!)
                lLocationListOK.Add(lLocation, lLocationName.ItemByKey(lLocation))
            End If
        Next

        With atcGridLocation
            .Initialize(New ListSource(lLocationListOK))
            .Visible = True
            .Refresh()
        End With
    End Sub

    Private Sub atcGridLocation_MouseDownCell(ByVal aGrid As atcControls.atcGrid, ByVal aRow As Integer, ByVal aColumn As Integer) Handles atcGridLocation.MouseDownCell
        Dim lSource As ListSource = aGrid.Source
        Dim lIndex As Integer = lSource.SelectedItems.IndexFromKey(aRow)
        If lIndex >= 0 Then
            lSource.SelectedItems.RemoveAt(lIndex)
        Else
            lSource.SelectedItems.Add(aRow, lSource.CellValue(aRow, 0))  'use the location
        End If
        aGrid.Refresh()
    End Sub
End Class

Friend Class ListSource
    Inherits atcControls.atcGridSource

    Private pValues As atcCollection
    Private pSelected As atcCollection

    Public Property SelectedItems() As atcCollection
        Get
            Return pSelected
        End Get
        Set(ByVal newValue As atcCollection)
            pSelected = newValue
        End Set
    End Property

    Sub New(ByVal aValues As atcCollection, Optional ByVal aSelected As atcCollection = Nothing)
        pValues = aValues
        If aSelected Is Nothing Then
            pSelected = New atcCollection
        Else
            pSelected = aSelected
        End If
    End Sub

    Overrides Property Columns() As Integer
        Get
            Return 2
        End Get
        Set(ByVal Value As Integer)
        End Set
    End Property

    Overrides Property Rows() As Integer
        Get
            If pValues Is Nothing Then Return 1
            Return pValues.Count
        End Get
        Set(ByVal Value As Integer)
        End Set
    End Property

    Overrides Property CellValue(ByVal aRow As Integer, ByVal aColumn As Integer) As String
        Get
            Try
                If aColumn = 0 Then
                    Return pValues.Keys(aRow)
                Else
                    Return pValues.ItemByIndex(aRow)
                End If
            Catch
                Return ""
            End Try
        End Get
        Set(ByVal Value As String)
        End Set
    End Property

    Overrides Property CellSelected(ByVal aRow As Integer, ByVal aColumn As Integer) As Boolean
        Get
            Return pSelected.Keys.Contains(aRow) ' & "," & aColumn)
        End Get
        Set(ByVal newValue As Boolean)
            If newValue Then
                If Not pSelected.Keys.Contains(aRow) Then
                    pSelected.Add(aRow, CellValue(aRow, aColumn))
                End If
            Else
                pSelected.RemoveByKey(aRow)
            End If
        End Set
    End Property
End Class