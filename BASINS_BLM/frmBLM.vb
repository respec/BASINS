Imports atcData
Imports atcUtility

Friend Class frmBLM
    Private Const NOTHING_VALUE As String = "~Missing~"
    Dim pPlugIn As BasinsBLM.PlugIn

    Friend Sub New(ByVal aPlugIn As BasinsBLM.PlugIn, Optional ByRef aDataGroup As atcData.atcDataGroup = Nothing)
        MyBase.New()
        InitializeComponent() 'required by Windows Form Designer

        pPlugIn = aPlugIn

        If aDataGroup Is Nothing Then
            aDataGroup = atcDataManager.DataSets()
        End If

        If aDataGroup.Count > 0 Then
            'pDataGroup = aDataGroup 'Don't assign to pDataGroup too soon or it may slow down UserSelectData
            PopulateCriteriaList(aDataGroup)
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

    Private WithEvents pDataGroup As atcDataGroup   'group of atcData displayed

    Private Sub btnRun_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRun.Click
        Dim lSource As ListSource = atcGridLocation.Source
        For Each lLocation As String In lSource.SelectedItems
            pPlugIn.RunBLM(lLocation)
        Next
    End Sub

    Private Sub PopulateCriteriaList(ByVal aDataGroup As atcDataGroup)
        Dim lAttributeName As String = "Location"

        Dim lLocationList As New atcCollection
        For Each lDataSet As atcDataSet In aDataGroup
            Dim lLocation As String = lDataSet.Attributes.GetValue(lAttributeName, "")
            lLocationList.Increment(lLocation, 1)
        Next

        Dim lLocationListOK As New atcCollection
        For Each lLocation As String In lLocationList.Keys
            If lLocationList.ItemByKey(lLocation) > 8 Then 'have enough data (maybe!)
                lLocationListOK.Add(lLocation)
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
            lSource.SelectedItems.Add(aRow, lSource.CellValue(aRow, aColumn))
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
            Return 1
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
                Return pValues.ItemByIndex(aRow)
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