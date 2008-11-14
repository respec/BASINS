Imports atcUtility
Imports MapWinUtility

Public Module modTimeseriesMath

    Private pNaN As Double = GetNaN()
    Private pMaxValue As Double = GetMaxValue()

    Public Function FindDateAtOrAfter(ByVal aDatesToSearch() As Double, ByVal aDate As Double, Optional ByVal aStartAt As Integer = 0) As Integer
        aDate -= JulianMillisecond 'Allow for floating point error
        Dim lIndex As Integer = Array.BinarySearch(aDatesToSearch, aDate)
        If lIndex < 0 Then
            lIndex = lIndex Xor -1
        End If
        Return lIndex

        '    Dim lEnd As Integer = aDates.GetUpperBound(0)
        '    While aStartAt < lEnd AndAlso aDates(aStartAt) < aDate
        '        aStartAt += 1
        '    End While
        '    Debug.Print(lIndex & " " & aStartAt)
        '    Return aStartAt
    End Function

    Public Function SubsetByDate(ByVal aTimeseries As atcTimeseries, _
                                 ByVal aStartDate As Double, _
                                 ByVal aEndDate As Double, _
                                 ByVal aDataSource As atcDataSource) As atcTimeseries
        'TODO: boundary conditions...
        If aTimeseries Is Nothing OrElse aTimeseries.Dates Is Nothing Then Return Nothing
        Dim lStart As Integer = FindDateAtOrAfter(aTimeseries.Dates.Values, aStartDate)
        Dim lEnd As Integer = FindDateAtOrAfter(aTimeseries.Dates.Values, aEndDate, lStart) - 1
        If lEnd > aTimeseries.numValues - 1 Then 'adjust end to actual end
            lEnd = aTimeseries.numValues - 1
        End If

        Dim lnewTS As New atcTimeseries(aDataSource)
        lnewTS.Dates = New atcTimeseries(aDataSource)
        lnewTS.Attributes.SetValue("Parent Timeseries", aTimeseries)

        Dim numNewValues As Integer = lEnd - lStart + 1
        If numNewValues > 0 Then
            Dim newValues(numNewValues) As Double
            Dim newDates(numNewValues) As Double
            newValues(0) = GetNaN()

            System.Array.Copy(aTimeseries.Dates.Values, lStart, newDates, 0, numNewValues + 1)
            System.Array.Copy(aTimeseries.Values, lStart + 1, newValues, 1, numNewValues)

            lnewTS.Values = newValues
            lnewTS.Dates.Values = newDates
            CopyBaseAttributes(aTimeseries, lnewTS, numNewValues, lStart + 1, 1)
            lnewTS.Attributes.SetValue("SJDAY", aStartDate)
            lnewTS.Attributes.SetValue("EJDAY", aEndDate)
        Else
            CopyBaseAttributes(aTimeseries, lnewTS)
        End If
        Return lnewTS
    End Function

    ''' <summary>
    ''' Trim a timeseries if needed to make it start and end at the desired year boundary.
    ''' Useful when complete calendar or water years are needed
    ''' </summary>
    ''' <param name="aTimeseries"></param>
    ''' <param name="aStartMonth"></param>
    ''' <param name="aStartDay"></param>
    ''' <param name="aDataSource"></param>
    ''' <param name="aFirstYear"></param>
    ''' <param name="aLastYear"></param>
    ''' <param name="aEndMonth"></param>
    ''' <param name="aEndDay"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function SubsetByDateBoundary(ByVal aTimeseries As atcTimeseries, _
                                         ByVal aStartMonth As Integer, _
                                         ByVal aStartDay As Integer, _
                                         ByVal aDataSource As atcDataSource, _
                                Optional ByVal aFirstYear As Integer = 0, _
                                Optional ByVal aLastYear As Integer = 0, _
                                Optional ByVal aEndMonth As Integer = 0, _
                                Optional ByVal aEndDay As Integer = 0) As atcTimeseries
        Dim lStartDate As Double
        Dim lEndDate As Double
        Dim lStartTimeseriesDate As Date
        Dim lEndTimeseriesDate As Date
        Dim lStartYear As Integer
        Dim lEndYear As Integer

        If aEndMonth = 0 Then aEndMonth = aStartMonth
        If aEndDay = 0 Then aEndDay = aStartDay

        aTimeseries.EnsureValuesRead()

        If aFirstYear > 0 AndAlso (aEndMonth < aStartMonth OrElse (aEndMonth = aStartMonth AndAlso aEndDay < aStartDay)) Then
            'Convert water year into calendar year
            aFirstYear -= 1
        End If

        lStartTimeseriesDate = Date.FromOADate(aTimeseries.Dates.Value(0))
        With lStartTimeseriesDate
            'Roll back end of year by one day if it matches beginning of year
            If aEndMonth = aStartMonth AndAlso aEndDay = aStartDay Then
                aEndDay -= 1
                If aEndDay = 0 Then
                    aEndMonth -= 1
                    If aEndMonth = 0 Then aEndMonth = 12
                    aEndDay = daymon(.Year, aEndMonth)
                End If
            End If

            lStartYear = .Year
            If aFirstYear > lStartYear Then
                lStartYear = aFirstYear
            Else
                If .Month > aStartMonth Then
                    lStartYear += 1
                ElseIf .Month = aStartMonth Then
                    If .Day > aStartDay Then
                        lStartYear += 1
                    End If
                End If
            End If
            lStartDate = Jday(lStartYear, aStartMonth, aStartDay, 0, 0, 0)
        End With

        lEndTimeseriesDate = Date.FromOADate(aTimeseries.Dates.Value(aTimeseries.Dates.numValues))
        With lEndTimeseriesDate
            lEndYear = .Year
            If aLastYear > 0 AndAlso aLastYear < lEndYear Then
                lEndYear = aLastYear
            Else
                If .Month < aEndMonth Then
                    lEndYear -= 1
                ElseIf .Month = aEndMonth Then
                    If .Day < aEndDay Then
                        lEndYear -= 1
                    End If
                End If
            End If
            lEndDate = Jday(lEndYear, aEndMonth, aEndDay, 0, 0, 0)
        End With

        SubsetByDateBoundary = SubsetByDate(aTimeseries, lStartDate, lEndDate, aDataSource)
        SubsetByDateBoundary.Attributes.Add("seasbg", aStartMonth)
        SubsetByDateBoundary.Attributes.Add("seadbg", aStartDay)
        SubsetByDateBoundary.Attributes.Add("seasnd", aEndMonth)
        SubsetByDateBoundary.Attributes.Add("seadnd", aEndDay)

    End Function

    ''' <summary>
    ''' Copy any attributes that copies inherit from aFromDataSet into aToDataSet
    ''' </summary>
    ''' <param name="aFromDataset">dataset containing attributes to copy</param>
    ''' <param name="aToDataSet">dataset to copy attributes into</param>
    ''' <param name="aNumValues">Number of values to copy value attributes of</param>
    ''' <param name="aStartFrom">Start index for copying value attributes from</param>
    ''' <param name="aStartTo">Start index for copying value attributes to</param>
    ''' <remarks>Only copies general attributes if aNumValues is omitted or is less than 1, 
    ''' Also copes value attributes if aNumValues > 0</remarks>
    Public Sub CopyBaseAttributes(ByVal aFromDataset As atcTimeseries, ByVal aToDataSet As atcTimeseries, _
                                  Optional ByVal aNumValues As Integer = 0, _
                                  Optional ByVal aStartFrom As Integer = 0, _
                                  Optional ByVal aStartTo As Integer = 0)

        For Each lAttribute As atcDefinedValue In aFromDataset.Attributes
            If lAttribute.Definition.CopiesInherit Then
                aToDataSet.Attributes.SetValue(lAttribute.Definition, lAttribute.Value)
            End If
        Next

        For lIndex As Integer = 0 To aNumValues - 1
            If aFromDataset.ValueAttributesExist(lIndex + aStartFrom) Then
                For Each lAttribute As atcDefinedValue In aFromDataset.ValueAttributes(lIndex + aStartFrom)
                    If lAttribute.Definition.CopiesInherit Then
                        aToDataSet.ValueAttributes(lIndex + aStartTo).SetValue(lAttribute.Definition, lAttribute.Value)
                    End If
                Next
            End If
        Next
    End Sub

    ''' <summary>Merge a group of atcTimeseries</summary>
    ''' <param name="aGroup">Group of atcTimeseries to merge</param>
    ''' <param name="aFilterNoData">True to skip missing values, False to include missing values in result</param>
    ''' <returns>atcTimeseries containing all unique dates from the group</returns>
    ''' <remarks>Each atcTimeseries in aGroup is assumed to be in order by date within itself.
    ''' If duplicate dates exist in aGroup, some values will be left out of result.</remarks>
    Public Function MergeTimeseries(ByVal aGroup As atcDataGroup, _
                           Optional ByVal aFilterNoData As Boolean = False) As atcTimeseries
        Dim lNewTS As New atcTimeseries(Nothing) 'will contain new (merged) dates
        Dim lNewIndex As Integer
        Dim lTotalNumValues As Integer = 0
        Dim lOldTS As atcTimeseries 'points to current timeseries from aGroup
        Dim lMinDate As Double = pMaxValue
        Dim lMaxGroupIndex As Integer = aGroup.Count - 1
        Dim lIndex As Integer
        Dim lMinIndex As Integer
        Dim lNextIndex() As Integer
        Dim lNextDate() As Double

        ReDim lNextIndex(aGroup.Count - 1)
        ReDim lNextDate(aGroup.Count - 1)

        MergeAttributes(aGroup, lNewTS)
        'lNewTS.Attributes.AddHistory("Merged " & aGroup.Count)

        lNewTS.Dates = New atcTimeseries(Nothing)

        'Count total number of values and set up 
        For lIndex = 0 To lMaxGroupIndex
            lOldTS = aGroup.Item(lIndex)
            Try
                lTotalNumValues += lOldTS.numValues
                GetNextDateIndex(lOldTS, aFilterNoData, lNextIndex(lIndex), lNextDate(lIndex))
                'Find minimum starting date and take date before from that TS
                If lNextDate(lIndex) < lMinDate Then
                    lMinDate = lOldTS.Dates.Value(lNextIndex(lIndex) - 1)
                End If
            Catch 'Can't get dates values from this TS
                lNextIndex(lIndex) = -1
            End Try
        Next

        If lTotalNumValues > 0 Then
            lNewTS.numValues = lTotalNumValues
            lNewTS.Dates.numValues = lTotalNumValues
            If lMinDate < pMaxValue Then
                lNewTS.Dates.Value(0) = lMinDate
            Else
                lNewTS.Dates.Value(0) = pNaN
            End If
            lNewTS.Value(0) = pNaN

            For lNewIndex = 1 To lTotalNumValues
                'Find earliest date not yet used
                lMinIndex = -1
                lMinDate = pMaxValue
                For lIndex = 0 To lMaxGroupIndex
                    If lNextIndex(lIndex) > 0 AndAlso lNextDate(lIndex) < lMinDate Then
                        lMinIndex = lIndex
                        lMinDate = lNextDate(lIndex)
                    End If
                Next

                'TODO: could make common cases faster with Array.Copy
                'Now that we have found lMinDate, could also find the lNextMinDate when the 
                'minimum date from a different TS happens, then find out how many more values 
                'from this TS are earlier than lNextMinDate, then copy all of them to the 
                'new TS at once

                'Add earliest date and value to new TS
                If lMinIndex >= 0 Then
                    'Logger.Dbg("---found min date in data set " & lMinIndex)
                    lOldTS = aGroup.Item(lMinIndex)
                    If lOldTS.ValueAttributesGetValue(lNextIndex(lMinIndex), "Inserted", False) Then
                        'Logger.Dbg("---discarding inserted value")
                        'This value was inserted during splitting and will now be discarded
                        lNewIndex -= 1
                        lTotalNumValues -= 1
                        GetNextDateIndex(lOldTS, aFilterNoData, _
                                         lNextIndex(lMinIndex), _
                                         lNextDate(lMinIndex))
                    Else
                        'Logger.Dbg("---MergeTimeseries adding date " & lMinDate & " value " & lOldTS.Value(lNextIndex(lMinIndex)) & " from dataset " & lMinIndex)
                        lNewTS.Dates.Value(lNewIndex) = lMinDate
                        lNewTS.Value(lNewIndex) = lOldTS.Value(lNextIndex(lMinIndex))

                        GetNextDateIndex(lOldTS, aFilterNoData, lNextIndex(lMinIndex), lNextDate(lMinIndex))

                        For lIndex = 0 To lMaxGroupIndex
                            'Discard next value from any TS that falls within one millisecond
                            'Don't need Math.Abs because we already found minimum
                            While lNextIndex(lIndex) > 0 AndAlso (lNextDate(lIndex) - lMinDate) < JulianMillisecond
                                lOldTS = aGroup.Item(lIndex)
                                'Logger.Dbg("---MergeTimeseries discarding date " & DumpDate(lNextDate(lIndex)) & " value " & lOldTS.Value(lNextIndex(lIndex)) & " from dataset " & lIndex)
                                lTotalNumValues -= 1    'This duplicate no longer counts toward total
                                GetNextDateIndex(lOldTS, aFilterNoData, lNextIndex(lIndex), lNextDate(lIndex))
                            End While
                        Next
                    End If
                Else 'out of values in all the datasets
                    'Logger.Dbg("Warning:MergeTimeseries:Ran out of values at " & lNewIndex & " of " & lTotalNumValues)
                    lTotalNumValues = lNewIndex - 1
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

    Private Sub GetNextDateIndex(ByVal aTs As atcTimeseries, _
                                 ByVal aFilterNoData As Boolean, _
                                 ByRef aIndex As Integer, _
                                 ByRef aNextDate As Double)
        aIndex += 1
        While aIndex <= aTs.numValues
            If (Not aFilterNoData) OrElse (Not Double.IsNaN(aTs.Value(aIndex))) Then
                aNextDate = aTs.Dates.Value(aIndex)
                Exit While
            Else
                aIndex += 1
            End If
        End While
        If aIndex > aTs.numValues Then
            aNextDate = pNaN 'is this necessary?
            aIndex = -1 'out of values
        End If
    End Sub

    Private Sub MergeAttributes(ByVal aGroup As atcDataGroup, ByVal aTarget As atcTimeseries)
        For Each lAttribute As atcDefinedValue In aGroup(0).Attributes
            If lAttribute.Definition.CopiesInherit Then
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
        Select Case aObj.GetType.Name
            Case "atcDataGroup" : Return aObj
            Case "atcTimeseries" : Return New atcDataGroup(aObj)
            Case Else
                Logger.Dbg("DatasetOrGroupToGroup: Unrecognized type '" & aObj.GetType.Name & "'")
                Return Nothing
        End Select
    End Function

    'Fill values in constant interval timeseries with specified values.
    'Assumes dates are at the end of each value's interval and that the
    '0th position in the Dates array is the beginning of the first interval.
    Public Function FillValues(ByVal aOldTSer As atcTimeseries, _
                               ByVal aTU As atcTimeUnit, _
                               Optional ByVal aTS As Long = 1, _
                               Optional ByVal aFillVal As Double = 0, _
                               Optional ByVal aMissVal As Double = -999, _
                               Optional ByVal aAccumVal As Double = -999, _
                               Optional ByVal aDataSource As atcDataSource = Nothing) As atcTimeseries
        'aTU - Time units (1-sec, 2-min, 3-hour, 4-day, 5-month, 6-year, 7-century).
        'aTS - Timestep in units of aTU.
        'aFillVal - Value to Fill data gaps with.
        'aMissVal - Value indicating missing data.
        'aAccumVal - Value indicating accumulated data.

        If aOldTSer.numValues > 0 Then
            Dim lDate(5) As Integer
            Dim lNewNumVals As Integer
            Dim lNewInd As Integer
            Dim lOldInd As Integer
            Dim lDateNew As Double
            Dim lDateOld As Double
            Dim lValOld As Double
            Dim lNewVals() As Double
            Dim lNewDates() As Double = NewDates(aOldTSer, aTU, aTS)

            If lNewDates.GetUpperBound(0) > 0 Then 'dates for new timeseries set
                lNewNumVals = lNewDates.GetUpperBound(0)
                ReDim lNewVals(lNewNumVals)
                lNewVals(0) = pNaN
                lOldInd = 1
                lDateOld = aOldTSer.Dates.Value(lOldInd)
                lNewInd = 1
                lDateNew = lNewDates(lNewInd)
                Dim lAnyValueAttributes As Boolean = aOldTSer.ValueAttributesExist
                Dim lNewValueAttributes(lNewDates.GetUpperBound(0)) As atcDataAttributes

                While lNewInd <= lNewNumVals
                    While lDateNew < lDateOld - JulianMillisecond 'Fill values not present in original data
                        Select Case lValOld
                            Case aMissVal
                                If aOldTSer.Value(lOldInd) = aMissVal Then
                                    lNewVals(lNewInd) = aMissVal
                                    lNewValueAttributes(lNewInd) = New atcDataAttributes
                                    lNewValueAttributes(lNewInd).SetValue("Missing", True)
                                Else
                                    lNewVals(lNewInd) = aFillVal
                                    lNewValueAttributes(lNewInd) = New atcDataAttributes
                                    lNewValueAttributes(lNewInd).SetValue("Filled", True)
                                End If
                            Case aAccumVal
                                lNewVals(lNewInd) = aAccumVal
                                lNewValueAttributes(lNewInd) = New atcDataAttributes
                                lNewValueAttributes(lNewInd).SetValue("Accumulated", True)
                            Case Else
                                lNewVals(lNewInd) = aFillVal
                                lNewValueAttributes(lNewInd) = New atcDataAttributes
                                lNewValueAttributes(lNewInd).SetValue("Filled", True)
                        End Select
                        'lNewDates(lNewInd) = lDateNew
                        lNewInd += 1
                        lDateNew = lNewDates(lNewInd)
                        'If lVarLength = 1 Then
                        '  Call J2Date(lDateNew, lDate)
                        '  lDateNew = lDateNew + daymon(lDate(0), lDate(1))
                        'ElseIf lVarLength = 2 Then
                        '  Call J2Date(lDateNew, lDate)
                        '  lDateNew = lDateNew + 365 + (daymon(lDate(0), 2) - 28)
                        'Else
                        '  lDateNew = lDateNew + lIntvl
                        'End If
                    End While
                    lValOld = aOldTSer.Value(lOldInd)
                    lNewVals(lNewInd) = lValOld
                    If lAnyValueAttributes AndAlso aOldTSer.ValueAttributesExist(lOldInd) Then
                        lNewValueAttributes(lNewInd) = aOldTSer.ValueAttributes(lOldInd)
                    End If
                    'lNewDates(lNewInd) = lDateNew
                    'If lVarLength = 1 Then 'monthly
                    '  Call J2Date(lDateNew, lDate)
                    '  lDateNew = lDateNew + daymon(lDate(0), lDate(1))
                    'ElseIf lVarLength = 2 Then 'yearly
                    '  Call J2Date(lDateNew, lDate)
                    '  lDateNew = lDateNew + 365 + (daymon(lDate(0), 2) - 28)
                    'Else
                    '  lDateNew = lDateNew + lIntvl
                    'End If
                    lNewInd += 1
                    If lNewInd <= lNewNumVals Then
                        lDateNew = lNewDates(lNewInd)
                    End If
                    lOldInd += 1
                    If lOldInd <= aOldTSer.numValues Then
                        lDateOld = aOldTSer.Dates.Value(lOldInd)
                    End If
                End While
                Dim lNewTSer As New atcTimeseries(aDataSource)
                CopyBaseAttributes(aOldTSer, lNewTSer)
                lNewTSer.Dates = New atcTimeseries(Nothing)
                lNewTSer.Dates.Values = lNewDates
                lNewTSer.Values = lNewVals
                For lNewInd = 1 To lNewValueAttributes.GetUpperBound(0)
                    If lNewValueAttributes(lNewInd) IsNot Nothing Then
                        lNewTSer.ValueAttributes(lNewInd) = lNewValueAttributes(lNewInd)
                    End If
                Next
                With lNewTSer.Attributes
                    .SetValue("point", False)
                    .SetValue("tu", aTU)
                    .SetValue("ts", aTS)
                    .SetValue("TSFILL", aFillVal)
                    .SetValue("MVal", aMissVal)
                    .SetValue("MAcc", aAccumVal)
                End With

                Return lNewTSer
            Else
                Logger.Dbg("Problem with dates in Timeseries " & aOldTSer.ToString & ".")
                Return Nothing
            End If
        Else
            Logger.Dbg("No data values in Timeseries " & aOldTSer.ToString & ".")
            Return Nothing
        End If
    End Function

    ''' <summary>
    ''' Fill missing periods in a timeseries using interpolation
    ''' </summary>
    ''' <param name="aOldTSer">Timeseries containing missing values</param>
    ''' <param name="aMaxFillLength">Max span, in Julian Days, over which interpolation is allowed</param>
    ''' <param name="aFillInstances">Array returning length of each missing period filled</param>
    ''' <returns>atcTimeseries clone of original timeseries along with interpolated values</returns>
    ''' <remarks></remarks>
    Public Function FillMissingByInterpolation(ByVal aOldTSer As atcTimeseries, _
                                      Optional ByVal aMaxFillLength As Double = Double.NaN, _
                                      Optional ByVal aFillInstances As ArrayList = Nothing) As atcTimeseries
        Dim lNewTSer As atcTimeseries = aOldTSer.Clone

        Dim lInd As Integer = 1
        Dim lIndPrevNotMissing As Integer = 1
        Dim lIndNextNotMissing As Integer
        Logger.Dbg("FillMissingByInterp:NumValues:" & lNewTSer.numValues & ":" & aMaxFillLength)
        While lInd <= lNewTSer.numValues
            If Double.IsNaN(lNewTSer.Values(lInd)) Then 'look for next good value
                lIndNextNotMissing = FindNextNotMissing(lNewTSer, lInd)
                Dim lMissingLength As Double
                With lNewTSer.Dates 'find missing length
                    lMissingLength = .Values(lIndNextNotMissing) - .Values(lIndPrevNotMissing)
                End With
                'Logger.Dbg("FillMissingByInterp:Missing:", lInd, lIndPrevNotMissing, lIndNextNotMissing, lMissingLength)
                If Double.IsNaN(aMaxFillLength) OrElse lMissingLength < aMaxFillLength Then
                    If Not aFillInstances Is Nothing AndAlso lInd = lIndPrevNotMissing + 1 Then
                        '1st interval of a missing period, log/record it
                        Logger.Dbg("FillMissingByInterp: Starting " & DumpDate(lNewTSer.Dates.Value(lInd)) & ", interpolating over a span of " & lMissingLength & " days.")
                        aFillInstances.Add(lMissingLength)
                    End If
                    With lNewTSer
                        If Double.IsNaN(.Values(lIndPrevNotMissing)) Then 'missing at start, use first good value
                            .Values(lInd) = .Values(lIndNextNotMissing)
                            'Logger.Dbg("FillMissingByInterp:UseFirstNotMissing:" & .Values(lInd))
                        ElseIf Double.IsNaN(.Values(lIndNextNotMissing)) Then 'missing at end, use last good value
                            .Values(lInd) = .Values(lIndPrevNotMissing)
                            'Logger.Dbg("FillMissingByInterp:UseLastNotMissing:" & .Values(lInd))
                        Else 'values prev and next, interpolate
                            Dim lFracMissing As Double
                            With .Dates
                                lFracMissing = (.Values(lInd) - .Values(lIndPrevNotMissing)) / _
                                               (.Values(lIndNextNotMissing) - .Values(lIndPrevNotMissing))
                            End With
                            Dim lIncValue As Double = lFracMissing * (.Values(lIndNextNotMissing) - .Values(lIndPrevNotMissing))
                            .Values(lInd) = .Values(lIndPrevNotMissing) + lIncValue
                            'Logger.Dbg("FillMissingByInterp:Interp:" & .Values(lInd) & ":" & lFracMissing & ":" & lIncValue)
                        End If
                    End With
                End If
            Else 'good value, remember index
                lIndPrevNotMissing = lInd
            End If
            lInd += 1
        End While
        Return lNewTSer
    End Function

    Private Function FindNextNotMissing(ByVal aTser As atcTimeseries, ByVal aInd As Integer) As Integer
        Dim lInd As Integer = aInd
        While Double.IsNaN(aTser.Values(lInd))
            lInd += 1
            If lInd >= aTser.numValues Then
                Return aTser.numValues
            End If
        End While
        Return lInd
    End Function

    Public Function Aggregate(ByVal aTimeseries As atcTimeseries, _
                              ByVal aTU As atcTimeUnit, _
                              ByVal aTS As Integer, _
                              ByVal aTran As atcTran, _
                              Optional ByVal aDataSource As atcDataSource = Nothing) As atcTimeseries
        If aTimeseries.Attributes.GetValue("tu") = aTU AndAlso _
           aTimeseries.Attributes.GetValue("ts") = aTS Then
            ' Already have desired time unit and time step, clone so we consistently return a new TS
            Return aTimeseries.Clone(aDataSource)
        Else
            Dim lNewDates() As Double = NewDates(aTimeseries, aTU, aTS)
            Dim lNumNewVals As Integer = lNewDates.GetUpperBound(0)
            If lNumNewVals > 0 Then
                Dim lNaN As Double = GetNaN()
                Dim lNewTSer As New atcTimeseries(aDataSource)
                lNewTSer.Dates = New atcTimeseries(aDataSource)
                CopyBaseAttributes(aTimeseries, lNewTSer)
                lNewTSer.Attributes.SetValue("TU", aTU)
                lNewTSer.Attributes.SetValue("TS", aTS)
                lNewTSer.Attributes.SetValue("point", False)
                If aTimeseries.ValueAttributesExist Then 'TODO:: Something with value attributes
                End If
                lNewTSer.Dates.Values = lNewDates
                Dim lNewIndex As Integer = 1
                Dim lNewVals(lNumNewVals) As Double
                Dim lDateNew As Double = lNewDates(1)
                Dim lDateOld As Double
                Dim lValOld As Double
                Dim lOldIndex As Integer = 1
                Dim lPrevDateOld As Double = lNewDates(0) 'old and new TSers should have same start date
                Dim lPrevDateNew As Double = lNewDates(0)
                Dim lOverlapStart As Double
                Dim lOverlapEnd As Double
                Dim lNumOldVals As Integer = aTimeseries.numValues
                Dim lFraction As Double
                Dim lCumuFrac As Double

                If aTimeseries.numValues > 0 Then
                    lValOld = aTimeseries.Value(1)
                    lDateOld = aTimeseries.Dates.Value(1)
                End If

                Select Case aTran

                    Case atcTran.TranAverSame, atcTran.TranSumDiv
                        While lNewIndex <= lNumNewVals
                            lDateNew = lNewDates(lNewIndex)
                            lNewVals(lNewIndex) = 0
                            While lPrevDateOld < lDateNew And lOldIndex <= lNumOldVals
                                If lPrevDateOld > lPrevDateNew Then lOverlapStart = lPrevDateOld Else lOverlapStart = lPrevDateNew
                                If lDateNew > lDateOld Then lOverlapEnd = lDateOld Else lOverlapEnd = lDateNew
                                lFraction = (lOverlapEnd - lOverlapStart) / (lDateNew - lPrevDateNew)
                                lCumuFrac += lFraction
                                If aTran = atcTran.TranSumDiv Then
                                    lFraction = (lOverlapEnd - lOverlapStart) / (lDateOld - lPrevDateOld)
                                End If
                                lNewVals(lNewIndex) += lFraction * lValOld
                                If lPrevDateOld < lDateNew Then
                                    If lDateOld > lDateNew Then 'use remaining part of this old interval on next new interval
                                        lPrevDateOld = lDateNew
                                        If aTran = atcTran.TranSumDiv Then lValOld = lValOld - lValOld * lFraction
                                    Else
                                        lPrevDateOld = lDateOld
                                        lOldIndex = lOldIndex + 1
                                        If lOldIndex <= lNumOldVals Then
                                            lDateOld = aTimeseries.Dates.Value(lOldIndex)
                                            lValOld = aTimeseries.Value(lOldIndex)
                                        End If
                                        'lCumuFrac = 0
                                    End If
                                End If
                            End While
                            lPrevDateNew = lDateNew
                            If aTran = atcTran.TranSumDiv AndAlso lCumuFrac > 0.01 AndAlso lCumuFrac < 0.999 Then
                                lNewVals(lNewIndex) = lNewVals(lNewIndex) / lCumuFrac
                                lCumuFrac = 0
                            End If
                            lNewIndex = lNewIndex + 1
                        End While

                    Case atcTran.TranMax
                        Dim lMinValue As Double = GetMinValue()
                        While lNewIndex <= lNumNewVals
                            lDateNew = lNewDates(lNewIndex)
                            lNewVals(lNewIndex) = lMinValue
                            While lDateOld <= lDateNew AndAlso lOldIndex <= lNumOldVals
                                If lValOld > lNewVals(lNewIndex) Then lNewVals(lNewIndex) = lValOld
                                lOldIndex = lOldIndex + 1
                                If lOldIndex <= lNumOldVals Then
                                    lDateOld = aTimeseries.Dates.Value(lOldIndex)
                                    lValOld = aTimeseries.Value(lOldIndex)
                                End If
                            End While
                            If lNewVals(lNewIndex) = lMinValue Then
                                lNewVals(lNewIndex) = lNaN
                            End If
                            lNewIndex = lNewIndex + 1
                        End While

                    Case atcTran.TranMin
                        Dim lMaxValue As Double = GetMaxValue()
                        While lNewIndex <= lNumNewVals
                            lDateNew = lNewDates(lNewIndex)
                            lNewVals(lNewIndex) = lMaxValue
                            While lDateOld <= lDateNew AndAlso lOldIndex <= lNumOldVals
                                If lValOld < lNewVals(lNewIndex) Then lNewVals(lNewIndex) = lValOld
                                lOldIndex = lOldIndex + 1
                                If lOldIndex <= lNumOldVals Then
                                    lDateOld = aTimeseries.Dates.Value(lOldIndex)
                                    lValOld = aTimeseries.Value(lOldIndex)
                                End If
                            End While
                            If lNewVals(lNewIndex) = lMaxValue Then
                                lNewVals(lNewIndex) = lNaN
                            End If
                            lNewIndex = lNewIndex + 1
                        End While

                End Select
                lNewTSer.Values = lNewVals
                Return lNewTSer
            Else
                Return Nothing
            End If
        End If
    End Function

    'Build Date array for a timeseries with start/end of aTSer and time units/step of aTU/aTS
    Public Function NewDates(ByVal aTSer As atcTimeseries, ByVal aTU As atcTimeUnit, ByVal aTS As Integer) As Double()
        Dim lSJDay As Double
        Dim lEJDay As Double
        If aTU >= atcTimeUnit.TUSecond AndAlso aTU <= atcTimeUnit.TUCentury Then
            'get start date/time for existing TSer
            aTSer.EnsureValuesRead()
            Dim lDate(5) As Integer
            If aTSer.Dates.Value(0) <= 0 OrElse Double.IsNaN(aTSer.Dates.Value(0)) Then
                If aTSer.Attributes.ContainsAttribute("tu") Then
                    J2Date(aTSer.Dates.Value(1), lDate)
                    TIMADD(lDate, aTSer.Attributes.GetValue("tu"), aTSer.Attributes.GetValue("ts", 1), -1, lDate)
                ElseIf aTSer.numValues > 1 Then
                    J2Date(aTSer.Dates.Value(1) - (aTSer.Dates.Value(2) - aTSer.Dates.Value(1)), lDate)
                End If
            Else
                J2Date(aTSer.Dates.Value(0), lDate)
            End If
            Dim lSDate(5) As Integer
            Select Case aTU
                Case atcTimeUnit.TUSecond
                Case atcTimeUnit.TUMinute
                    lDate(5) = 0 'clear seconds
                Case atcTimeUnit.TUHour
                    lDate(4) = 0 'clear minutes
                    lDate(5) = 0 'clear seconds
                Case atcTimeUnit.TUDay
                    lDate(3) = 0 'clear hours
                    lDate(4) = 0 'clear minutes
                    lDate(5) = 0 'clear seconds
                Case atcTimeUnit.TUMonth
                    lDate(2) = 1 'set to beginning of month
                    lDate(3) = 0 'clear hours
                    lDate(4) = 0 'clear minutes
                    lDate(5) = 0 'clear seconds
                Case atcTimeUnit.TUYear
                    'Skip setting month and day to allow drought/flood years to be preserved
                    'lDate(1) = 1 'set to beginning of Jan
                    'lDate(2) = 1 'set to beginning of month
                    lDate(3) = 0 'clear hours
                    lDate(4) = 0 'clear minutes
                    lDate(5) = 0 'clear seconds
                Case atcTimeUnit.TUCentury
                    lDate(0) = Math.Floor(lDate(0) / 100) * 100
                    lDate(1) = 1 'set to beginning of Jan
                    lDate(2) = 1 'set to beginning of month
                    lDate(3) = 0 'clear hours
                    lDate(4) = 0 'clear minutes
                    lDate(5) = 0 'clear seconds
            End Select
            lSJDay = Date2J(lDate)
            lEJDay = aTSer.Dates.Value(aTSer.numValues)
        End If
        Return NewDates(lSJDay, lEJDay, aTU, aTS)
    End Function

    ''' <summary>
    ''' Build a constant-interval date array
    ''' </summary>
    ''' <param name="aStartDate">Beginning of the first interval</param>
    ''' <param name="aEndDate">End of the last interval</param>
    ''' <param name="aTU">Time Units</param>
    ''' <param name="aTS">Time Step (number of Time Units per step)</param>
    Public Function NewDates(ByVal aStartDate As Double, ByVal aEndDate As Double, ByVal aTU As atcTimeUnit, ByVal aTS As Integer) As Double()
        Dim lNewDates(0) As Double
        If aTU >= atcTimeUnit.TUSecond AndAlso aTU <= atcTimeUnit.TUCentury Then
            Dim lIntvl As Double
            Dim lNewNumDates As Integer
            Dim lDate(5) As Integer
            Dim lSDate(5) As Integer

            'get interval of new TSer
            Select Case aTU
                Case atcTimeUnit.TUSecond : lIntvl = aTS * JulianSecond
                Case atcTimeUnit.TUMinute : lIntvl = aTS * JulianMinute
                Case atcTimeUnit.TUHour : lIntvl = aTS * JulianHour
                Case atcTimeUnit.TUDay : lIntvl = aTS
                Case atcTimeUnit.TUMonth : lIntvl = aTS * 30.44
                Case atcTimeUnit.TUYear : lIntvl = aTS * 365.25
                Case atcTimeUnit.TUCentury : lIntvl = aTS * 36525
            End Select
            lNewNumDates = CInt((aEndDate - aStartDate) / lIntvl)
            ReDim lNewDates(lNewNumDates)
            lNewDates(0) = aStartDate
            If aTU > modDate.atcTimeUnit.TUDay Then J2Date(aStartDate, lSDate) 'will need start date array
            For i As Integer = 1 To lNewNumDates
                If aTU > modDate.atcTimeUnit.TUDay Then 'need to use special TIMADD function for varying length intervals (month, year)
                    TIMADD(lSDate, aTU, aTS, i, lDate)
                    lNewDates(i) = Date2J(lDate)
                Else
                    lNewDates(i) = aStartDate + lIntvl * i
                End If
            Next
        End If
        Return lNewDates
    End Function

    Public Function NewTimeseries(ByVal aStartDate As Double, ByVal aEndDate As Double, _
                                  ByVal aTU As atcTimeUnit, ByVal aTS As Integer, _
                         Optional ByVal aDataSource As atcDataSource = Nothing, _
                         Optional ByVal aSetAllValues As Double = 0) As atcTimeseries
        Dim lDates As New atcTimeseries(aDataSource)
        lDates.Values = NewDates(aStartDate, aEndDate, aTU, aTS)
        Dim lNewTimeseries As New atcTimeseries(aDataSource)
        lNewTimeseries.Dates = lDates
        lNewTimeseries.numValues = lNewTimeseries.Dates.numValues
        lNewTimeseries.Value(0) = GetNaN()
        If aSetAllValues <> 0 Then
            For lIndex As Integer = 1 To lNewTimeseries.numValues
                lNewTimeseries.Value(lIndex) = aSetAllValues
            Next
        End If
        Return lNewTimeseries
    End Function


    ''Make bins, sort data values into the bins, and assign collection of Bins as new attribute
    'Public Sub MakeBins(ByVal aTS As atcTimeseries, Optional ByVal aMaxBinSize As Integer = 100)
    '  Dim lNumValues As Integer = aTS.numValues
    '  Dim lCurValue As Double
    '  Dim lBinIndex As Integer
    '  Dim lCurBin As New ArrayList
    '  Dim lBins As New atcCollection
    '  lBins.Add(aTS.Attributes.GetValue("Max"), lCurBin)

    '  Logger.Dbg("Sorting " & lNumValues & " values into bins of at most " & aMaxBinSize)
    '  For lOldIndex As Integer = 1 To lNumValues
    '    lCurValue = aTS.Value(lOldIndex)

    '    'find first bin with maximum >= lCurValue
    '    lBinIndex = 0
    '    While lCurValue > lBins.Keys(lBinIndex)
    '      lBinIndex += 1
    '    End While
    '    lCurBin = lBins.Item(lBinIndex)

    '    'Insert in numeric order within bin
    '    Dim lInsertIndex As Integer = 0
    '    Dim lLastIndex As Integer = lCurBin.Count - 1
    '    If lLastIndex > -1 Then  'Find position to insert
    '      While lCurValue > lCurBin.Item(lInsertIndex)
    '        lInsertIndex += 1
    '        If lInsertIndex > lLastIndex Then Exit While
    '      End While
    '    End If
    '    lCurBin.Insert(lInsertIndex, lCurValue)

    '    If lCurBin.Count > aMaxBinSize Then
    '      SplitBin(lBins, lCurBin, lBinIndex)
    '    End If

    '  Next
    '  Logger.Dbg("Created " & lBins.Count & " bins")
    '  For lBinIndex = 0 To lBins.Count - 1
    '    lCurBin = lBins.Item(lBinIndex)
    '    Logger.Dbg("Bin " & lBinIndex & " (" & lBins.Keys(lBinIndex) & ") contains " & lCurBin.Count)
    '    For Each lCurValue In lCurBin
    '      Logger.Dbg(DoubleToString(lCurValue))
    '    Next
    '    lNumValues -= lCurBin.Count
    '  Next
    '  If lNumValues <> 0 Then
    '    Logger.Dbg("Wrong number of values in bins -- " & lNumValues & " were in dataset but not in bins")
    '  End If
    '  aTS.Attributes.SetValue("Bins", lBins)
    'End Sub

    'Make bins, sort data values into the bins
    'Default maximum bin size is 1% of total number of values
    Public Function MakeBins(ByVal aTS As atcTimeseries, Optional ByVal aMaxBinSize As Integer = 0) As atcCollection
        Dim lNumValues As Integer = aTS.numValues
        Dim lCurValue As Double
        Dim lCurBinMax As Double = aTS.Attributes.GetValue("Max")
        Dim lBinIndex As Integer = 0
        Dim lCurBin As New ArrayList
        Dim lBins As New atcCollection                     'Keys of lBins are the maximum value in each bin                                   
        lBins.Add(lCurBinMax, lCurBin) 'First bin created is assigned maximum value for dataset
        'Bins created later are inserted before this bin, which remains the "last" bin containing the highest values
        If aMaxBinSize < 1 Then
            aMaxBinSize = lNumValues / 100 'Default to max of 1% of values in each bin
            If aMaxBinSize < 10 Then aMaxBinSize = 10
        End If
        Logger.Progress("Sorting values from " & aTS.ToString & " into bins. ", 0, lNumValues)
        For lOldIndex As Integer = 1 To lNumValues
            lCurValue = aTS.Value(lOldIndex)
            If Not Double.IsNaN(lCurValue) Then
                'If the previously used bin does not fit, find first bin with maximum >= lCurValue
                'If lCurValue > lCurBinMax OrElse (lBinIndex > 0 AndAlso lCurValue < lBins.Keys.Item(lBinIndex - 1)) Then
                lBinIndex = BinarySearchFirstGreaterDoubleArrayList(lBins.Keys, lCurValue)
                lCurBin = lBins.Item(lBinIndex)
                'lCurBinMax = lBins.Keys.Item(lBinIndex)
                'End If

                'Insert in numeric order within bin
                Dim lInsertIndex As Integer = BinarySearchFirstGreaterDoubleArrayList(lCurBin, lCurValue)
                lCurBin.Insert(lInsertIndex, lCurValue)

                If lCurBin.Count > aMaxBinSize Then
                    SplitBin(lBins, lCurBin, lBinIndex)
                    '  lCurBin = lBins.Item(lBinIndex)
                    '  lCurBinMax = lBins.Keys.Item(lBinIndex)
                    Logger.Progress("Sorting values into " & lBins.Count & " bins", lOldIndex, lNumValues)
                End If
            End If
        Next
        Logger.Progress("Sorted values into " & lBins.Count & " bins", lNumValues, lNumValues)
        'For lBinIndex = 0 To lBins.Count - 1
        '  lCurBin = lBins.Item(lBinIndex)
        '  Logger.Dbg("Bin " & lBinIndex & " (" & lBins.Keys(lBinIndex) & ") contains " & lCurBin.Count)
        '  For Each lCurValue In lCurBin
        '    Logger.Dbg(DoubleToString(lCurValue))
        '  Next
        '  lNumValues -= lCurBin.Count
        'Next
        'If lNumValues <> 0 Then
        '  Logger.Dbg("Wrong number of values in bins -- " & lNumValues & " were in dataset but not in bins")
        'End If
        Return lBins
    End Function

    'aBins = collection of bins
    'aBin = bin to be split in half
    'aBinIndex = current index of aBin in aBins
    Private Sub SplitBin(ByVal aBins As atcCollection, ByVal aBin As ArrayList, ByVal aBinIndex As Integer)
        Dim lSplitStart As Integer = 0
        Dim lSplitCount As Integer = aBin.Count / 2
        Dim lNewBin As New ArrayList(aBin.GetRange(lSplitStart, lSplitCount))
        aBin.RemoveRange(lSplitStart, lSplitCount)
        aBins.Insert(aBinIndex, lNewBin.Item(lSplitCount - 1), lNewBin)
    End Sub

    ''' <summary>
    ''' Binary search through an ArrayList containing Double values sorted in ascending order
    ''' </summary>
    ''' <param name="aArray">Array to search</param>
    ''' <param name="aValue">Value to search for</param>
    ''' <returns>Return the index of the first value >= aValue</returns>
    ''' <remarks>Returns aArray.Count if aArray contains no values >= aValue</remarks>
    Private Function BinarySearchFirstGreaterDoubleArrayList(ByVal aArray As ArrayList, ByVal aValue As Double) As Integer
        Dim lHigher As Integer = aArray.Count - 1
        If lHigher < 0 Then Return 0 'No values present to compare to
        Dim lLower As Integer = -1 'Note: this starts one *lower than* start of where to search in array
        Dim lProbe As Integer
        While (lHigher - lLower > 1)
            lProbe = (lHigher + lLower) / 2
            If aArray(lProbe) < aValue Then
                lLower = lProbe
            Else
                lHigher = lProbe
            End If
        End While
        If aValue > aArray(lHigher) Then
            Return lHigher + 1
        Else
            Return lHigher
        End If
    End Function

    Public Sub ComputePercentileSum(ByVal aTimeseries As atcTimeseries, ByVal aPercentile As Double)
        Dim lAttrName As String = "%sum" & Format(aPercentile, "00.####")
        Dim lNumValues As Integer = aTimeseries.numValues
        Select Case lNumValues
            Case Is < 1
                'Can't compute with no values
            Case 1
                aTimeseries.Attributes.SetValue(lAttrName, aTimeseries.Value(0))
            Case Else
                Dim lBins As atcCollection = aTimeseries.Attributes.GetValue("Bins")
                Dim lCountPercentileDone As Integer = aPercentile * lNumValues / 100.0 - 1
                If lCountPercentileDone < 0 Then lCountPercentileDone = 0
                If lCountPercentileDone >= lNumValues Then lCountPercentileDone = lNumValues - 1

                Dim lSum As Double = 0
                Dim lCount As Integer = 0
                For Each lBin As ArrayList In lBins
                    For Each lValue As Double In lBin
                        If lCount >= lCountPercentileDone Then GoTo Finished
                        lCount += 1
                        lSum += lValue
                    Next
                Next
Finished:
                aTimeseries.Attributes.SetValue(lAttrName, lSum)
        End Select

    End Sub

    Public Sub ComputePercentile(ByVal aTimeseries As atcTimeseries, ByVal aPercentile As Double)
        Dim lAttrName As String = "%" & Format(aPercentile, "00.####")
        Dim lNumValues As Integer = aTimeseries.numValues
        Select Case lNumValues
            Case Is < 1
                'Can't compute with no values
            Case 1
                aTimeseries.Attributes.SetValue(lAttrName, aTimeseries.Value(0))
            Case Else
                Dim lBins As atcCollection = aTimeseries.Attributes.GetValue("Bins")
                'TODO: could interpolate between closest two values rather than choosing closest one, should we?
                Dim lAccumulatedCount As Integer = 0
                Dim lNextAccumulatedCount As Integer = 0
                Dim lBinIndex As Integer = -1
                Dim lPercentileIndex As Integer = aPercentile * lNumValues / 100.0 - 1
                If lPercentileIndex < 0 Then lPercentileIndex = 0
                If lPercentileIndex >= lNumValues Then lPercentileIndex = lNumValues - 1
                While lNextAccumulatedCount <= lPercentileIndex
                    lAccumulatedCount = lNextAccumulatedCount
                    lBinIndex += 1
                    lNextAccumulatedCount = lAccumulatedCount + lBins(lBinIndex).Count
                End While
                Dim lBin As ArrayList = lBins(lBinIndex)
                aTimeseries.Attributes.SetValue(lAttrName, lBin.Item(lPercentileIndex - lAccumulatedCount))
        End Select
    End Sub

    ''' <summary>
    ''' fit a line through a set of data points using least squares regression.
    ''' </summary>
    ''' <param name="aTSerX"></param>
    ''' <param name="aTSerY"></param>
    ''' <param name="aACoef">'a' coefficient in regression line (y=ax+b)</param>
    ''' <param name="aBCoef">'b' coefficient in regression line (y=ax+b)</param>
    ''' <param name="aRSquare">'r squared', the coefficient of determination</param>
    ''' <remarks>from fortran-newaqt-FITLIN</remarks>
    Public Sub FitLine(ByVal aTSerX As atcTimeseries, ByVal aTSerY As atcTimeseries, _
                       ByRef aACoef As Double, ByRef aBCoef As Double, ByRef aRSquare As Double)
        Dim lProblem As String = ""
        If aTSerX.numValues <> aTSerY.numValues Then
            lProblem &= aTSerX.ToString & " has " & aTSerX.numValues & " values, " & _
                        aTSerY.ToString & " has " & aTSerY.numValues & "." & vbCrLf
        End If
        If aTSerX.Dates.Value(0) <> aTSerY.Dates.Value(0) Then
            lProblem &= aTSerX.ToString & " starts on " & aTSerX.Dates.Value(0).ToString & ", " & _
                        aTSerY.ToString & " starts on " & aTSerY.Dates.Value(0).ToString & "." & vbCrLf
        End If
        If lProblem.Length > 0 Then
            Throw New ApplicationException("Timeseries are not compatible." & vbCrLf & lProblem)
        End If

        Dim lNote As String = ""
        Dim lSumX As Double = 0.0
        Dim lValX As Double
        Dim lAvgX As Double

        Dim lSumY As Double = 0.0
        Dim lValY As Double
        Dim lAvgY As Double
        Dim lSkipCount As Integer = 0
        Dim lGoodCount As Integer = 0

        For lIndex As Integer = 1 To aTSerX.numValues
            lValX = aTSerX.Values(lIndex)
            lValY = aTSerY.Values(lIndex)
            If Not Double.IsNaN(lValX) And Not Double.IsNaN(lValY) Then
                lSumX += lValX
                lSumY += lValY
                lGoodCount += 1
            Else
                lSkipCount += 1
                If lSkipCount = 1 Then
                    lNote = "*** Note - compare skipped index " & lIndex
                End If
            End If
        Next
        If lNote.Length > 0 Then
            lNote &= " and " & lSkipCount - 1 & " more" & vbCrLf
        End If

        If (lSumX > 0.0 And lSumY > 0.0 And lGoodCount > 0) Then 'go ahead and compute
            lAvgX = lSumX / lGoodCount
            lAvgY = lSumY / lGoodCount

            Dim lSum3 As Double = 0.0
            Dim lSum4 As Double = 0.0
            For lIndex As Integer = 1 To aTSerX.numValues
                lValX = aTSerX.Values(lIndex)
                lValY = aTSerY.Values(lIndex)
                If Not Double.IsNaN(lValX) And Not Double.IsNaN(lValY) Then
                    lSum3 += (lValX - lAvgX) * (lValY - lAvgY)
                    lSum4 += (lValY - lAvgY) * (lValY - lAvgY)
                End If
            Next lIndex
            aACoef = lSum3 / lSum4
            aBCoef = lAvgX - (aACoef * lAvgY)

            Dim lSum5 As Double = 0
            Dim lSum6 As Double = 0
            For lIndex As Integer = 1 To aTSerX.numValues
                lValX = aTSerX.Values(lIndex)
                lValY = aTSerY.Values(lIndex)
                If Not Double.IsNaN(lValX) And Not Double.IsNaN(lValY) Then
                    lSum5 += ((aACoef * lValY + aBCoef - lAvgX) * (aACoef * lValY) + aBCoef - lAvgX)
                    lSum6 += (lValX - lAvgX) * (lValX - lAvgX)
                End If
            Next lIndex
            aRSquare = lSum5 / lSum6
        Else 'regression doesnt make sense, return NaN
            aACoef = GetNaN()
            aBCoef = GetNaN()
            aRSquare = GetNaN()
        End If
        If lNote.Length > 0 Then
            Logger.Dbg("Note:" & lNote)
        End If
    End Sub
End Module
