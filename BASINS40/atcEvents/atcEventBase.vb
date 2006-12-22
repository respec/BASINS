Imports atcData
Imports atcUtility

Public Module atcEventBase
    'Divide the data in aTS into a group of TS, one per event
    Public Function EventSplit(ByVal aTS As atcTimeseries, _
                               ByVal aSource As atcDataSource, _
                               ByVal aThresh As Double, _
                               ByVal aHigh As Boolean) As atcDataGroup
        Dim lNewGroup As New atcDataGroup
        Dim lEventIndex As Integer = 0
        Dim lNewTS As atcTimeseries
        Dim lPoint As Boolean = aTS.Attributes.GetValue("point", False)
        Dim lInEvent As Boolean = False
        Dim lSPos As Integer
        Dim lEPos As Integer
        Dim iValue As Integer = 1
        Dim lEDate As Double = aTS.Dates.Value(1)
        Dim lSinceLast As Double

        If lPoint Then
            lEDate = aTS.Dates.Value(1)
        Else
            lEDate = aTS.Dates.Value(0)
        End If
        While iValue <= aTS.numValues

            If lInEvent Then
                If (aHigh AndAlso aTS.Value(iValue) < aThresh) OrElse _
                   (Not aHigh AndAlso aTS.Values(iValue) > aThresh) Then
                    'clean up latest event tser
                    lEPos = iValue - 1
                    lNewTS = New atcTimeseries(aSource)
                    CopyBaseAttributes(aTS, lNewTS)
                    lNewTS.Dates = New atcTimeseries(aSource)
                    lNewTS.numValues = lEPos - lSPos + 1
                    lNewTS.Dates.numValues = lNewTS.numValues
                    Array.Copy(aTS.Values, lSPos, lNewTS.Values, 1, lNewTS.numValues)
                    If lPoint Then
                        Array.Copy(aTS.Dates.Values, lSPos, lNewTS.Dates.Values, 1, lNewTS.numValues)
                    Else
                        Array.Copy(aTS.Dates.Values, lSPos - 1, lNewTS.Dates.Values, 0, lNewTS.numValues + 1)
                    End If
                    lEDate = lNewTS.Dates.Value(lNewTS.numValues)
                    If aHigh Then
                        lNewTS.Attributes.AddHistory("Event above " & aThresh)
                    Else
                        lNewTS.Attributes.AddHistory("Event below " & aThresh)
                    End If
                    lNewTS.Attributes.Add("EventIndex", lEventIndex)
                    lNewTS.Attributes.Add("EventThreshold", aThresh)
                    lNewTS.Attributes.Add("EventTimeSincePrevious", lSinceLast)
                    lNewGroup.Add(lEventIndex, lNewTS)
                    lInEvent = False
                End If
            ElseIf (aHigh AndAlso aTS.Value(iValue) >= aThresh) OrElse _
                   (Not aHigh AndAlso aTS.Value(iValue) <= aThresh) Then
                'new event
                lEventIndex += 1
                lSPos = iValue
                lInEvent = True
                If lPoint Then
                    lSinceLast = aTS.Dates.Value(lSPos) - lEDate
                Else
                    lSinceLast = aTS.Dates.Value(lSPos - 1) - lEDate
                End If
            End If

            iValue += 1
        End While

        Return lNewGroup
    End Function
End Module