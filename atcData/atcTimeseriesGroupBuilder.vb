Imports atcUtility
Imports atcData
Imports MapWinUtility

Public Class atcTimeseriesGroupBuilder

    Private pDataSource As atcDataSource
    Private pBuilders As atcCollection

    Public Sub New(ByVal aDataSource As atcDataSource)
        pDataSource = aDataSource
        pBuilders = New atcCollection
    End Sub

    ''' <summary>
    ''' Add a values to the set of timeseries being built
    ''' </summary>
    ''' <param name="aDate">Date that these values share</param>
    ''' <param name="aValues">Set of values to add, one for each timeseries being built</param>
    ''' <remarks></remarks>
    Public Sub AddValues(ByVal aDate As Date, ByVal ParamArray aValues() As Double)
        Dim lIndex As Integer

        If pBuilders.Count = 0 Then 'Create new builders with numbers starting at 1 for keys
            For lIndex = 0 To aValues.GetUpperBound(0)
                Builder(lIndex + 1)
            Next
        End If

        If pBuilders.Count = aValues.Length Then 'add values to builders by index
            For lIndex = 0 To aValues.GetUpperBound(0)
                pBuilders.ItemByIndex(lIndex).AddValue(aDate, aValues(lIndex))
            Next
        Else
            Throw New ApplicationException("Tried to add " & aValues.Length & " different values to " & pBuilders.Count & " timeseries")
        End If
    End Sub

    Public Sub CreateBuilders(ByVal ParamArray aKeys() As String)
        pBuilders.Clear()
        For Each lKey As String In aKeys
            Builder(lKey)
        Next
    End Sub

    ''' <summary>
    ''' Retrieve or create the builder for the dataset with the specified key
    ''' </summary>
    ''' <param name="aDataSetKey"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Builder(ByVal aDataSetKey As String) As atcTimeseriesBuilder
        Dim lBuilder As atcTimeseriesBuilder
        Dim lBuilderIndex As Integer = pBuilders.IndexFromKey(aDataSetKey)
        If lBuilderIndex >= 0 Then
            lBuilder = pBuilders.ItemByIndex(lBuilderIndex)
        Else
            lBuilder = New atcTimeseriesBuilder(pDataSource)
            pBuilders.Add(aDataSetKey, lBuilder)
            If IsNumeric(aDataSetKey) Then 'Use key for ID
                lBuilder.Attributes.SetValue("ID", CInt(aDataSetKey))
            Else 'Default ID is 1 for first dataset, 2 for second
                lBuilder.Attributes.SetValue("ID", pBuilders.Count)
                lBuilder.Attributes.SetValue("Key", aDataSetKey)
            End If
        End If
        Return lBuilder
    End Function

    Public Function CreateTimeseriesGroup() As atcDataGroup
        Dim lDataSets As New atcDataGroup
        CreateTimeseriesAddToGroup(lDataSets)
        Return lDataSets
    End Function

    Public Sub CreateTimeseriesAddToGroup(ByVal aGroup As atcDataGroup)
        Logger.Status("Creating Timeseries")
        Dim lCount As Integer = 0
        For Each lBuilder As atcTimeseriesBuilder In pBuilders
            Dim lDataSet As atcTimeseries = lBuilder.CreateTimeseries
            aGroup.Add(lDataSet.Attributes.GetValue("ID"), lDataSet)
            lCount += 1
            Logger.Progress(lCount, pBuilders.Count)
        Next
    End Sub

End Class
