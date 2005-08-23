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

    CopyBaseAttributes(aTimeseries, lnewTS)

    lnewTS.Attributes.SetValue("Parent Timeseries", aTimeseries)

    Return lnewTS

  End Function

  Public Sub CopyBaseAttributes(ByVal aFromDataset As atcDataSet, ByVal aToDataSet As atcDataSet)
    For Each lAttribute As atcDefinedValue In aFromDataset.Attributes
      If Not (lAttribute.Definition.Calculated) Then
        aToDataSet.Attributes.Add(lAttribute)
      End If
    Next
  End Sub
End Module
