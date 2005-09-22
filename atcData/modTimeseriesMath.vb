Imports atcUtility

Public Module modTimeseriesMath

  Public Function SubsetByDate(ByVal aTimeseries As atcTimeseries, _
                               ByVal aStartDate As Double, _
                               ByVal aEndDate As Double, _
                               ByVal aDataSource As atcDataSource) As atcTimeseries
    'TODO: boundary conditions...
    Dim iStart As Integer = 0
    Dim iEnd As Integer = aTimeseries.numValues - 1

    'TODO: binary search for iStart and iEnd could be faster
    While iStart < iEnd AndAlso aTimeseries.Dates.Value(iStart) < aStartDate
      iStart += 1
    End While

    While iEnd > iStart AndAlso aTimeseries.Dates.Value(iEnd) >= aEndDate
      iEnd -= 1
    End While

    Dim numNewValues As Integer = iEnd - iStart + 1
    Dim newValues(numNewValues) As Double
    Dim newDates(numNewValues) As Double

    System.Array.Copy(aTimeseries.Dates.Values, iStart, newDates, 0, numNewValues + 1)
    System.Array.Copy(aTimeseries.Values, iStart + 1, newValues, 1, numNewValues)

    Dim lnewTS As New atcTimeseries(aDataSource)
    lnewTS.Dates = New atcTimeseries(aDataSource)
    lnewTS.Values = newValues
    lnewTS.Dates.Values = newDates
    CopyBaseAttributes(aTimeseries, lnewTS, numNewValues, iStart + 1, 1)

    lnewTS.Attributes.SetValue("Parent Timeseries", aTimeseries)

    Return lnewTS

  End Function

  Public Function SubsetByDateBoundary(ByVal aTimeseries As atcTimeseries, _
                                       ByVal aBoundaryMonth As Integer, _
                                       ByVal aBoundaryDay As Integer, _
                                       ByVal aDataSource As atcDataSource) As atcTimeseries
    Dim lStartDate As Double
    Dim lEndDate As Double
    Dim lStartTimeseriesDate As Date
    Dim lEndTimeseriesDate As Date
    Dim lStartYear As Integer
    Dim lEndYear As Integer

    'TODO: boundary conditions...
    lStartTimeseriesDate = Date.FromOADate(aTimeseries.Dates.Value(0))
    With lStartTimeseriesDate
      lStartYear = .Year
      If .Month > aBoundaryMonth Then
        lStartYear += 1
      ElseIf .Month = aBoundaryMonth Then
        If .Day > aBoundaryDay Then
          lStartYear += 1
        End If
      End If
      lStartDate = Jday(lStartYear, aBoundaryMonth, aBoundaryDay, 0, 0, 0)
    End With

    lEndTimeseriesDate = Date.FromOADate(aTimeseries.Dates.Value(aTimeseries.Dates.numValues))
    With lEndTimeseriesDate
      lEndYear = .Year
      If .Month < aBoundaryMonth Then
        lEndYear -= 1
      ElseIf .Month = aBoundaryMonth Then
        If .Day < aBoundaryDay Then
          lEndYear -= 1
        End If
      End If
      lEndDate = Jday(lEndYear, aBoundaryMonth, aBoundaryDay, 0, 0, 0)
    End With

    Return SubsetByDate(aTimeseries, lStartDate, lEndDate, aDataSource)
  End Function

  Public Sub CopyBaseAttributes(ByVal aFromDataset As atcTimeseries, ByVal aToDataSet As atcTimeseries, _
                                Optional ByVal aNumValues As Integer = 0, _
                                Optional ByVal aStartFrom As Integer = 0, _
                                Optional ByVal aStartTo As Integer = 0)

    For Each lAttribute As atcDefinedValue In aFromDataset.Attributes
      If Not (lAttribute.Definition.Calculated) Then
        aToDataSet.Attributes.SetValue(lAttribute.Definition, lAttribute.Value)
      End If
    Next

    For lIndex As Integer = 0 To aNumValues - 1
      If aFromDataset.ValueAttributesExist(lIndex + aStartFrom) Then
        For Each lAttribute As atcDefinedValue In aFromDataset.ValueAttributes(lIndex + aStartFrom)
          If Not (lAttribute.Definition.Calculated) Then
            aToDataSet.ValueAttributes(lIndex + aStartTo).SetValue(lAttribute.Definition, lAttribute.Value)
          End If
        Next
      End If
    Next
  End Sub
  'Merge a group of atcTimeseries
  'Each atcTimeseries is assumed to be in order by date within itself
  'Resulting atcTimeseries will contain all dates and values from the group, sorted by date
  'If overlapping dates exist, duplicate dates will occur in the result
  Public Function MergeTimeseries(ByVal aGroup As atcDataGroup) As atcTimeseries
    Dim lNewTS As New atcTimeseries(Nothing)
    Dim lNewIndex As Integer
    Dim lTotalNumValues As Integer = 0
    Dim lOldTS As atcTimeseries
    Dim lMinDate As Double = Double.MaxValue
    Dim lMaxGroupIndex As Integer = aGroup.Count - 1
    Dim lIndex As Integer
    Dim lMinIndex As Integer
    Dim lNextIndex() As Integer
    Dim lNextDate() As Double
    Dim lOldValue As Double
    Dim lValueAttributes As atcDataAttributes

    ReDim lNextIndex(aGroup.Count - 1)
    ReDim lNextDate(aGroup.Count - 1)

    MergeAttributes(aGroup, lNewTS)
    'lNewTS.Attributes.AddHistory("Merged " & aGroup.Count)

    lNewTS.Dates = New atcTimeseries(Nothing)

    'Count total number of values and set up 
    For lIndex = 0 To lMaxGroupIndex
      lOldTS = aGroup.ItemByIndex(lIndex)
      Try
        lTotalNumValues += lOldTS.numValues
        lNextIndex(lIndex) = 1
        lNextDate(lIndex) = lOldTS.Dates.Value(1)
        'Find minimum starting date and take date 0 from that TS
        If lNextDate(lIndex) < lMinDate Then
          lMinDate = lOldTS.Dates.Value(0)
        End If
      Catch 'Can't get dates values from this TS
        lNextIndex(lIndex) = -1
      End Try
    Next

    If lTotalNumValues > 0 Then
      lNewTS.numValues = lTotalNumValues
      lNewTS.Dates.numValues = lTotalNumValues
      If lMinDate < Double.MaxValue Then
        lNewTS.Dates.Value(0) = lMinDate
      Else
        lNewTS.Dates.Value(0) = Double.NaN
      End If
      lNewTS.Value(0) = Double.NaN

      For lNewIndex = 1 To lTotalNumValues
        'Find earliest date not yet used
        lMinIndex = -1
        lMinDate = Double.MaxValue
        For lIndex = 0 To lMaxGroupIndex
          If lNextIndex(lIndex) > 0 Then
            If lNextDate(lIndex) < lMinDate Then
              lMinIndex = lIndex
              lMinDate = lNextDate(lIndex)
            End If
          End If
        Next
        'TODO: could make common cases faster with Array.Copy
        'Now that we have found lMinDate, could also find the lNextMinDate when the 
        'minimum date from a different TS happens, then find out how many more values 
        'from this TS are earlier than lNextMinDate, then copy all of them to the 
        'new TS at once

        'Add earliest date/value to new TS
        If lMinIndex >= 0 Then
          lOldTS = aGroup.ItemByIndex(lMinIndex)
          lOldValue = lOldTS.Value(lNextIndex(lMinIndex))
          If lOldTS.ValueAttributesGetValue(lNextIndex(lMinIndex), "Inserted", False) Then
            'This value was inserted during splitting and will now be removed
            lNewIndex -= 1
            lTotalNumValues -= 1
          Else
            lNewTS.Dates.Value(lNewIndex) = lMinDate
            lNewTS.Value(lNewIndex) = lOldValue
          End If

          lNextIndex(lMinIndex) += 1
          If lNextIndex(lMinIndex) <= lOldTS.numValues Then
            lNextDate(lMinIndex) = lOldTS.Dates.Value(lNextIndex(lMinIndex))
          Else
            lNextIndex(lMinIndex) = -1
          End If

        Else 'ran out of values in all the datasets
          Exit For
        End If
      Next
      If lTotalNumValues < lNewTS.numValues Then
        lNewTS.numValues = lTotalNumValues
        lNewTS.Dates.numValues = lTotalNumValues
      End If
    End If
    Return lNewTS
  End Function

  Private Sub MergeAttributes(ByVal aGroup As atcDataGroup, ByVal aTarget As atcTimeseries)
    For Each lAttribute As atcDefinedValue In aGroup(0).Attributes
      If Not lAttribute.Definition.Calculated Then
        Dim lMatch As Boolean = True
        For Each lData As atcDataSet In aGroup
          Try
            If lData.Attributes.GetValue(lAttribute.Definition.Name) <> lAttribute.Value Then
              lMatch = False
              Exit For
            End If
          Catch 'Can't test for equality, don't assign this one a value in aTarget
            lMatch = False
            Exit For
          End Try
        Next
        If lMatch Then aTarget.Attributes.SetValue(lAttribute.Definition, lAttribute.Value, lAttribute.Arguments)
      End If
    Next
  End Sub

  Public Function DatasetOrGroupToGroup(ByVal aObj As Object) As atcDataGroup
    Try
      Return aObj
    Catch
    End Try
    Try
      Return New atcDataGroup(aObj)
    Catch
    End Try

  End Function
End Module
