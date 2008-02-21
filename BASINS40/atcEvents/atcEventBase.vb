Imports atcData
Imports atcUtility

Public Module atcEventBase

    ''' <summary>
    ''' Divide a timeseries into a group of timeseries, one per event
    ''' </summary>
    ''' <param name="aTS">data to be split</param>
    ''' <param name="aSource">atcDataSource to assign as the source of event timeseries</param>
    ''' <param name="aThreshold">Threshold value that must be exceeded to begin an event</param>
    ''' <param name="aDaysGapAllowed">Number of days of values not exceeding aThresh to include within an event.
    ''' If zero, any value at or below aThresh ends the event.</param>
    ''' <param name="aHigh">True to select values above aThresh, False to select values below aThresh</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function EventSplit(ByVal aTS As atcTimeseries, _
                               ByVal aSource As atcDataSource, _
                               ByVal aThreshold As Double, _
                               ByVal aDaysGapAllowed As Double, _
                               ByVal aHigh As Boolean) As atcDataGroup
        Dim lNewGroup As New atcDataGroup
        Dim lEventIndex As Integer = 0
        Dim lPoint As Boolean = aTS.Attributes.GetValue("point", False)
        Dim lPointOffset As Integer
        Dim lInEvent As Boolean = False
        Dim lSPos As Integer
        Dim lEPos As Integer
        Dim lEDate As Double = aTS.Dates.Value(1)
        Dim lDelayEventEnd As Boolean
        Dim lCurrentDate As Double = 0
        Dim lLastEventDate As Double = 0
        Dim lLastEventIndex As Integer = 0

        If aDaysGapAllowed > 0 Then
            aDaysGapAllowed += JulianMillisecond 'avoid floating point error
            lDelayEventEnd = True
        Else
            lDelayEventEnd = False
        End If

        If lPoint Then
            lPointOffset = 0
        Else
            lPointOffset = -1
        End If
        lEDate = aTS.Dates.Value(1 + lPointOffset)

        If aHigh Then
            lSPos = FindHighEventStart(aTS, aThreshold, 1)
            While lSPos >= 0
                If lDelayEventEnd Then
                    lEPos = FindHighEventEnd(aTS, aThreshold, aDaysGapAllowed, lPointOffset, lSPos)
                Else
                    lEPos = FindHighEventEnd(aTS, aThreshold, lSPos)
                End If
                AddEventTS(aTS, aSource, aThreshold, aDaysGapAllowed, aHigh, lSPos, lEPos, lPointOffset, lNewGroup)
                lSPos = FindHighEventStart(aTS, aThreshold, lEPos + 1)
            End While
        Else 'Low events
            lSPos = FindLowEventStart(aTS, aThreshold, 1)
            While lSPos >= 0
                If lDelayEventEnd Then
                    lEPos = FindLowEventEnd(aTS, aThreshold, aDaysGapAllowed, lPointOffset, lSPos)
                Else
                    lEPos = FindLowEventEnd(aTS, aThreshold, lSPos)
                End If
                AddEventTS(aTS, aSource, aThreshold, aDaysGapAllowed, aHigh, lSPos, lEPos, lPointOffset, lNewGroup)
                lSPos = FindLowEventStart(aTS, aThreshold, lEPos + 1)
            End While
        End If

        Return lNewGroup
    End Function

    Private Function FindHighEventStart(ByVal aTS As atcTimeseries, _
                                        ByVal aThreshold As Double, _
                                        ByVal aStartAt As Integer) As Integer
        For lCurrentIndex As Integer = aStartAt To aTS.numValues
            If (aTS.Value(lCurrentIndex) > aThreshold) Then
                Return lCurrentIndex
            End If
        Next
        Return -1
    End Function

    Private Function FindLowEventStart(ByVal aTS As atcTimeseries, _
                                        ByVal aThreshold As Double, _
                                        ByVal aStartAt As Integer) As Integer
        For lCurrentIndex As Integer = aStartAt To aTS.numValues
            If (aTS.Value(lCurrentIndex) < aThreshold) Then
                Return lCurrentIndex
            End If
        Next
        Return -1
    End Function

    Private Function FindHighEventEnd(ByVal aTS As atcTimeseries, _
                                      ByVal aThreshold As Double, _
                                      ByVal aStartAt As Integer) As Integer
        For lCurrentIndex As Integer = aStartAt To aTS.numValues
            If (aTS.Value(lCurrentIndex) <= aThreshold) Then
                Return lCurrentIndex - 1
            End If
        Next
        Return aTS.numValues
    End Function

    Private Function FindLowEventEnd(ByVal aTS As atcTimeseries, _
                                      ByVal aThreshold As Double, _
                                      ByVal aStartAt As Integer) As Integer
        For lCurrentIndex As Integer = aStartAt To aTS.numValues
            If (aTS.Value(lCurrentIndex) >= aThreshold) Then
                Return lCurrentIndex - 1
            End If
        Next
        Return aTS.numValues
    End Function

    Private Function FindHighEventEnd(ByVal aTS As atcTimeseries, _
                                      ByVal aThreshold As Double, _
                                      ByVal aDaysGapAllowed As Double, _
                                      ByVal aPointOffset As Integer, _
                                      ByVal aStartAt As Integer) As Integer

        Dim lLastGoodIndex As Integer = aStartAt - 1
        Dim lLastGoodDate As Double = 0
        For lCurrentIndex As Integer = aStartAt To aTS.numValues
            If (aTS.Value(lCurrentIndex) > aThreshold) Then
                lLastGoodIndex = lCurrentIndex
                lLastGoodDate = aTS.Dates.Value(lLastGoodIndex + aPointOffset)
            Else
                If aTS.Dates.Value(lCurrentIndex + aPointOffset) - lLastGoodDate > aDaysGapAllowed Then
                    Return lLastGoodIndex
                End If
            End If
        Next
        Return aTS.numValues
    End Function

    Private Function FindLowEventEnd(ByVal aTS As atcTimeseries, _
                                     ByVal aThreshold As Double, _
                                     ByVal aDaysGapAllowed As Double, _
                                     ByVal aPointOffset As Integer, _
                                     ByVal aStartAt As Integer) As Integer

        Dim lLastGoodIndex As Integer = aStartAt - 1
        Dim lLastGoodDate As Double = 0

        For lCurrentIndex As Integer = aStartAt To aTS.numValues
            If (aTS.Value(lCurrentIndex) < aThreshold) Then
                lLastGoodIndex = lCurrentIndex
                lLastGoodDate = aTS.Dates.Value(lLastGoodIndex + aPointOffset)
            Else
                If aTS.Dates.Value(lCurrentIndex + aPointOffset) - lLastGoodDate > aDaysGapAllowed Then
                    Return lLastGoodIndex
                End If
            End If
        Next
        Return aTS.numValues
    End Function

    Private Sub AddEventTS(ByVal aTS As atcTimeseries, _
                           ByVal aSource As atcDataSource, _
                           ByVal aThreshold As Double, _
                           ByVal aDaysGapAllowed As Double, _
                           ByVal aHigh As Boolean, _
                           ByVal aStartPos As Integer, _
                           ByVal aEndPos As Integer, _
                           ByVal aPointOffset As Integer, _
                           ByVal aGroup As atcDataGroup)
        Dim lNewNumValues As Integer = aEndPos - aStartPos + 1
        If lNewNumValues > 0 Then
            Dim lNewTS As New atcTimeseries(aSource)
            CopyBaseAttributes(aTS, lNewTS)
            lNewTS.Dates = New atcTimeseries(aSource)
            lNewTS.numValues = lNewNumValues
            lNewTS.Dates.numValues = lNewNumValues
            Array.Copy(aTS.Values, aStartPos, lNewTS.Values, 1, lNewNumValues)
            Array.Copy(aTS.Dates.Values, aStartPos + aPointOffset, lNewTS.Dates.Values, 1 + aPointOffset, lNewNumValues - aPointOffset)
            If aHigh Then
                lNewTS.Attributes.AddHistory("Event above " & aThreshold)
            Else
                lNewTS.Attributes.AddHistory("Event below " & aThreshold)
            End If
            lNewTS.Attributes.Add("EventIndex", aGroup.Count + 1)
            lNewTS.Attributes.Add("EventThreshold", aThreshold)
            lNewTS.Attributes.Add("EventAllowedGap", aDaysGapAllowed)
            If aGroup.Count > 0 Then 'Can compute time since last event
                Dim lPreviousEvent As atcTimeseries = aGroup.ItemByIndex(aGroup.Count - 1)
                Dim lSinceLast As Double = lNewTS.Dates.Value(1) - lPreviousEvent.Dates.Value(lPreviousEvent.numValues)
                lNewTS.Attributes.Add("EventTimeSincePrevious", lSinceLast)
            End If
            aGroup.Add(aGroup.Count + 1, lNewTS)
        End If
    End Sub

End Module