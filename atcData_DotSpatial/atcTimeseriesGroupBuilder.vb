Imports atcUtility
Imports atcData
Imports MapWinUtility

Public Class atcTimeseriesGroupBuilder

    Private pDataSource As atcTimeseriesSource
    Private pBuilders As atcCollection
    Private pSharedAttributes As atcDataAttributes

    Public Sub New(ByVal aDataSource As atcTimeseriesSource)
        pDataSource = aDataSource
        pBuilders = New atcCollection
    End Sub

    Public Function Count() As Integer
        Return pBuilders.Count
    End Function

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

    ''' <summary>
    ''' Create all builders and assign a key to each one
    ''' </summary>
    ''' <param name="aKeys"></param>
    ''' <remarks>Call this before using Builder</remarks>
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
        Dim lIntegerKey As Integer
        Dim lKeyIsInteger As Boolean = Integer.TryParse(aDataSetKey, lIntegerKey)
        If lKeyIsInteger Then
            aDataSetKey = aDataSetKey.PadLeft(10, "0"c)
        End If
        Dim lBuilder As atcTimeseriesBuilder
        Dim lBuilderIndex As Integer = pBuilders.BinarySearchForKey(aDataSetKey)
        If lBuilderIndex = pBuilders.Count OrElse _
          (aDataSetKey <> pBuilders.Keys.Item(lBuilderIndex)) Then
            'Not a duplicate, add to the list
            lBuilder = New atcTimeseriesBuilder(pDataSource)
            pBuilders.Insert(lBuilderIndex, aDataSetKey, lBuilder)
            If lKeyIsInteger Then 'Use key for ID
                lBuilder.Attributes.SetValue("ID", lIntegerKey)
            Else 'Default ID is 1 for first dataset, 2 for second
                lBuilder.Attributes.SetValue("ID", pBuilders.Count)
                lBuilder.Attributes.SetValue("Key", aDataSetKey)
            End If
        Else
            lBuilder = pBuilders.ItemByIndex(lBuilderIndex)
        End If
        Return lBuilder
    End Function

    Public Function CreateTimeseriesGroup() As atcTimeseriesGroup
        Dim lDataSets As New atcTimeseriesGroup
        CreateTimeseriesAddToGroup(lDataSets)
        Return lDataSets
    End Function

    Public Sub CreateTimeseriesAddToGroup(ByVal aGroup As atcTimeseriesGroup)
        Logger.Status("Creating Timeseries")
        If pSharedAttributes IsNot Nothing Then
            pSharedAttributes.AddHistory("Read from " & pDataSource.Specification)
        End If
        Dim lLastBuilder As Integer = pBuilders.Count - 1
        For lBuilderIndex As Integer = 0 To lLastBuilder
            Dim lDataSet As atcTimeseries = pBuilders.ItemByIndex(lBuilderIndex).CreateTimeseries
            Dim lKey As Object = lDataSet.Attributes.GetValue("ID", lBuilderIndex)
            If aGroup.Keys.Contains(lKey) Then
                aGroup.Add(lDataSet)
            Else
                aGroup.Add(lKey, lDataSet)
            End If
            If pSharedAttributes Is Nothing Then
                lDataSet.Attributes.AddHistory("Read from " & pDataSource.Specification)
            Else
                lDataSet.Attributes.SharedAttributes = pSharedAttributes
            End If
            Logger.Progress(lBuilderIndex, lLastBuilder)
        Next
        pSharedAttributes = Nothing
        Logger.Progress("", 0, 0)
    End Sub

    Public Sub SetAttributeForAll(aAttributeName As String, aAttributeValue As Object)
        If pSharedAttributes Is Nothing Then pSharedAttributes = New atcDataAttributes
        pSharedAttributes.Add(aAttributeName, aAttributeValue)
    End Sub

End Class
