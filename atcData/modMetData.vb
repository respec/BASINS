Imports System.IO
Imports atcUtility
Imports MapWinUtility
Imports MapWinUtility.Strings

Public Module modMetData

    Private pNaN As Double = GetNaN()
    Private pMaxValue As Double = GetMaxValue()
    Private Const pEpsilon As Double = 0.000000001

    'Use for filling DAILY timeseries
    'Fill missing values in timeseries aTS2Fill with values from nearby timeseries (aTSAvail).
    'Use Obs Time timeseries aTS2FillOT and aTSAvaillOT to determine 
    'whether or not a nearby daily timeseries is appropriate to use.
    'Also use Obs Time if looking at a nearby station that is less than daily
    'time step to determine what 24-hour time span to aggregate data from.
    'Use Tolerance factor aTol to determine which nearby stations are acceptable
    'for distributing accumulated data (i.e. missing time distribution).
    Public Sub FillDailyTser(ByRef aTS2Fill As atcTimeseries, ByVal aTS2FillOT As atcTimeseries, ByVal aTSAvail As atcCollection, ByVal aTSAvailOT As atcCollection, ByVal aMVal As Double, ByVal aMAcc As Double, ByVal aTol As Double)
        Dim i As Integer = 0
        Dim j As Integer
        Dim k As Integer
        Dim lInd As Long
        Dim ii As Integer
        Dim lCurInd As Integer
        Dim lFPos As Integer
        Dim lSPos As Integer
        Dim lMLen As Integer
        Dim lMTyp As Integer
        Dim ld(5) As Integer
        Dim lFillOT As Integer
        Dim lStaOT As Integer
        Dim s As String
        Dim lStr As String
        Dim lTUStr As String
        Dim lSJDay As Double
        Dim lEJDay As Double
        Dim lSubSJDay As Double
        Dim lMJDay As Double
        Dim lFVal As Double
        Dim lFilledIt As Boolean
        Dim lFillAdjust As Double
        Dim lStaAdjust As Double
        Dim lRatio As Double
        Dim lCarry As Double
        Dim lRndOff As Double = 0.001
        Dim lAccVal As Double
        Dim lMaxDlyVal As Double
        Dim lMaxDlyInd As Integer
        Dim lDist As atcCollection
        Dim lTSer As atcTimeseries = Nothing
        Dim lCons As String = aTS2Fill.Attributes.GetValue("Constituent")
        Dim lAdjustAttribute As String = GetAdjustingAttribute(aTS2Fill)
        Const lValMin As Double = -90
        Const lValMax As Double = 200
        Dim lClosestObsDiff As Integer
        Dim lClosestObsInd As Integer
        Dim lTol As Double

        If lTol > 1 Then 'passed in as percentage
            lTol = lTol / 100
        End If
        Logger.Dbg("Filling Daily values for " & aTS2Fill.ToString & ", " & aTS2Fill.Attributes.GetValue("STANAM"))
        s = MissingDataSummary(aTS2Fill, aMVal, aMAcc, lValMin, lValMax, 2)
        lStr = StrSplit(s, "DETAIL:DATA", "")
        If Len(s) > 0 Then 'missing data found
            lDist = Nothing
            lDist = CalcMetDistances(aTS2Fill, aTSAvail, lAdjustAttribute)
            lFillAdjust = aTS2Fill.Attributes.GetValue(lAdjustAttribute, -999)
            If Math.Abs(lFillAdjust + 999) > pEpsilon Then
                Logger.Dbg("  (Historical average is " & lFillAdjust & ")")
            Else
                Logger.Dbg("  (Historical averages not in use)")
            End If
            lSJDay = aTS2Fill.Attributes.GetValue("SJDay")
            While Len(s) > 0
                lStr = StrSplit(s, ",", "")
                lMJDay = CDbl(StrSplit(s, ",", ""))
                lMLen = CLng(StrSplit(s, ",", ""))
                lMTyp = CLng(StrSplit(s, ",", ""))
                J2Date(lMJDay, ld)
                If lMTyp = 1 Then 'fill missing period
                    Logger.Dbg("  For period starting " & ld(0) & "/" & ld(1) & "/" & ld(2) & " lasting " & lMLen & " days:")
                    lCurInd = -1
                    lClosestObsDiff = 24
                    lClosestObsInd = -1
                    For k = 1 To lMLen
                        lFPos = lMJDay - lSJDay + k - 1
                        lFillOT = CurrentObsTime(aTS2FillOT, lFPos)
                        lFilledIt = False
                        j = 1
                        While j < aTSAvail.Count
                            lInd = lDist.IndexFromKey(CStr(j))
                            lTSer = aTSAvail.ItemByIndex(lInd)
                            lFVal = lValMin - 1
                            If (lMJDay + k - 1) >= lTSer.Dates.Value(0) And _
                               (lMJDay + k - 1) < lTSer.Dates.Value(lTSer.numValues) Then
                                'within date range, check value
                                If lTSer.Attributes.GetValue("TU") = atcTimeUnit.TUDay Then 'check daily value
                                    lSPos = lMJDay - lTSer.Dates.Value(0) + k - 1
                                    If lTSer.Value(lSPos) > lValMin And lTSer.Value(lSPos) < lValMax Then 'check obs time
                                        lStaOT = CurrentObsTime(aTSAvailOT(lInd), lSPos)
                                        Select Case lCons.ToUpper
                                            Case "TMAX" 'assume TMax occurs at hour 16 (4 pm)
                                                If lStaOT = lFillOT OrElse _
                                                   (lStaOT < 17 AndAlso lFillOT < 17) OrElse _
                                                   (lStaOT > 16 AndAlso lFillOT > 16) Then
                                                    'both values are for the same day
                                                    lFVal = lTSer.Value(lSPos)
                                                ElseIf lFillOT > 16 AndAlso lStaOT < 17 Then
                                                    'try filling with previous day's value
                                                    If lSPos > 1 AndAlso _
                                                       lTSer.Value(lSPos - 1) > lValMin AndAlso _
                                                       lTSer.Value(lSPos - 1) < lValMax Then
                                                        lFVal = lTSer.Value(lSPos - 1)
                                                    End If
                                                ElseIf lFillOT < 17 AndAlso lStaOT > 16 Then
                                                    'try filling with next day's value
                                                    If lSPos < lTSer.numValues AndAlso _
                                                       lTSer.Value(lSPos + 1) > lValMin AndAlso _
                                                       lTSer.Value(lSPos + 1) < lValMax Then
                                                        lFVal = lTSer.Value(lSPos + 1)
                                                    End If
                                                End If
                                            Case "TMIN" 'assume TMin occurs at hour 6 (6 am)
                                                If lStaOT = lFillOT OrElse _
                                                   (lStaOT < 6 AndAlso lFillOT < 6) OrElse _
                                                   (lStaOT > 5 AndAlso lFillOT > 5) Then
                                                    'both values are for the same day
                                                    lFVal = lTSer.Value(lSPos)
                                                ElseIf lFillOT < 6 AndAlso lStaOT > 5 Then
                                                    'try filling with previous day's value
                                                    If lSPos > 1 AndAlso _
                                                       lTSer.Value(lSPos - 1) > lValMin AndAlso _
                                                       lTSer.Value(lSPos - 1) < lValMax Then
                                                        lFVal = lTSer.Value(lSPos - 1)
                                                    End If
                                                ElseIf lFillOT > 5 AndAlso lStaOT < 6 Then
                                                    'try filling with next day's value
                                                    If lSPos < lTSer.numValues AndAlso _
                                                       lTSer.Value(lSPos + 1) > lValMin AndAlso _
                                                       lTSer.Value(lSPos + 1) < lValMax Then
                                                        lFVal = lTSer.Value(lSPos + 1)
                                                    End If
                                                End If
                                            Case Else
                                                'for matching obs time of 24, accept anything in the afternoon
                                                If (Math.Abs(lStaOT - lFillOT) <= 6) OrElse _
                                                   (lFillOT = 24 AndAlso lStaOT > 12) Then 'obs times close enough
                                                    lFVal = lTSer.Value(lSPos)
                                                ElseIf Math.Abs(lStaOT - lFillOT) < lClosestObsDiff AndAlso _
                                                       lTSer.Attributes.GetValue("SJDay") <= lMJDay AndAlso _
                                                       lTSer.Attributes.GetValue("EJDay") >= lMJDay + lMLen Then
                                                    'save closest Obs Time difference for last ditch filling option
                                                    'timseries must bracket missing period
                                                    lClosestObsDiff = Math.Abs(lStaOT - lFillOT)
                                                    lClosestObsInd = lInd
                                                End If
                                        End Select
                                    End If
                                ElseIf lTSer.Attributes.GetValue("TU") = atcTimeUnit.TUHour Then '*** for now, assume hourly !!!
                                    lSPos = 24 * (lMJDay - lTSer.Dates.Value(0) + k - 1) + lFillOT
                                    lFVal = 0
                                    For ii = lSPos To lSPos - 23 Step -1
                                        If ii > 0 And ii <= lTSer.numValues Then
                                            lFVal += lTSer.Value(ii)
                                        End If
                                    Next ii
                                End If
                                If lFVal > lValMin And lFVal < lValMax Then 'valid fill value found
                                    j = aTSAvail.Count
                                    lFilledIt = True
                                End If
                            End If
                            j += 1
                        End While
                        If Not lFilledIt AndAlso lClosestObsInd >= 0 Then 'no acceptable station value found, 
                            'use station with closest Obs Time difference
                            lInd = lClosestObsInd
                            lTSer = aTSAvail.ItemByIndex(lInd)
                            lSPos = lMJDay - lTSer.Dates.Value(0) + k - 1
                            lFVal = lTSer.Value(lSPos)
                            lFilledIt = True
                            Logger.Dbg("NOTE - Using station with Obs Time difference (" & lClosestObsDiff & ") greater than acceptable.")
                        End If
                        If lFilledIt Then
                            lStaAdjust = lTSer.Attributes.GetValue(lAdjustAttribute, -999)
                            If Math.Abs(lFillAdjust + 999) > pEpsilon AndAlso _
                               Math.Abs(lStaAdjust + 999) > pEpsilon Then
                                'adjust for historical averages
                                aTS2Fill.Value(lFPos) = lFillAdjust / lStaAdjust * lFVal
                            Else 'use value without adjustment
                                aTS2Fill.Value(lFPos) = lFVal
                            End If
                            J2Date(lMJDay + k - 1, ld)
                            If lCurInd <> lInd Then 'changing station used to fill
                                If lTSer.Attributes.GetValue("TU") = atcTimeUnit.TUDay Then
                                    lTUStr = "Daily"
                                Else 'assume hourly
                                    lTUStr = "Hourly"
                                End If
                                Logger.Dbg("    Filling from " & lTUStr & " TS " & lTSer.ToString & ", " & lTSer.Attributes.GetValue("STANAM"))
                                If Math.Abs(lFillAdjust + 999) > pEpsilon Then
                                    Logger.Dbg("      (Adjusting values using historical average of " & lStaAdjust & ")")
                                End If
                                lCurInd = lInd
                            End If
                            Logger.Dbg("      " & ld(0) & "/" & ld(1) & "/" & ld(2) & " - " & aTS2Fill.Value(lFPos))
                        Else 'no station had a value for this date
                            Logger.Dbg("PROBLEM - Could not find acceptable nearby station for filling")
                        End If
                    Next k
                Else 'fill accumulated period
                    Logger.Dbg("  For Accumulated period starting " & ld(0) & "/" & ld(1) & "/" & ld(2) & " lasting " & lMLen & " days:")
                    lAccVal = CDbl(StrSplit(s, vbCrLf, ""))
                    lSubSJDay = lMJDay - 1 'back up an interval for SubSetByDate call in ClosestPrecip
                    lEJDay = lSubSJDay + lMLen
                    lFPos = lMJDay - lSJDay
                    lFillOT = CurrentObsTime(aTS2FillOT, lFPos)
                    lTSer = ClosestPrecip(aTS2Fill, aTSAvail, lAccVal, lSubSJDay, lEJDay, aTol, lFillOT)
                    If Not lTSer Is Nothing Then
                        Logger.Dbg("    Distributing " & lAccVal & " from TS " & lTSer.ToString & ", " & lTSer.Attributes.GetValue("STANAM"))
                        lRatio = lAccVal / lTSer.Attributes.GetValue("Sum")
                        For k = 1 To lMLen
                            lFPos = lMJDay - lSJDay + k - 1
                            If lTSer.Attributes.GetValue("TU") = atcTimeUnit.TUDay Then 'check daily value
                                lFVal = lTSer.Value(k)
                            ElseIf lTSer.Attributes.GetValue("TU") = atcTimeUnit.TUHour Then '*** for now, assume hourly !!!
                                lSPos = 24 * (k - 1)
                                lFVal = 0
                                For ii = 1 To 24
                                    lFVal += lTSer.Value(lSPos + ii)
                                Next ii
                            End If
                            aTS2Fill.Value(lFPos) = lRatio * lFVal + lCarry
                            If aTS2Fill.Value(lFPos) > pEpsilon Then
                                lCarry = aTS2Fill.Value(lFPos) - (Math.Round(aTS2Fill.Value(lFPos) / lRndOff) * lRndOff)
                                aTS2Fill.Value(lFPos) = aTS2Fill.Value(lFPos) - lCarry
                            Else
                                aTS2Fill.Value(lFPos) = 0.0#
                            End If
                            If aTS2Fill.Value(lFPos) > lMaxDlyVal Then
                                lMaxDlyVal = aTS2Fill.Value(lFPos)
                                lMaxDlyInd = lFPos
                            End If
                            J2Date(lMJDay + (k - 1), ld)
                            Logger.Dbg("      " & ld(0) & "/" & ld(1) & "/" & ld(2) & " - " & aTS2Fill.Value(lFPos))
                        Next k
                        If lCarry > 0 Then 'add remainder to max hourly value
                            aTS2Fill.Value(lMaxDlyInd) = aTS2Fill.Value(lMaxDlyInd) + lCarry
                        End If
                    Else
                        J2Date(lEJDay, ld)
                        Logger.Dbg("      *** No nearby station found for distributing accumulated value of " & lAccVal & " on " & ld(0) & "/" & ld(1) & "/" & ld(2))
                        For k = 1 To lMLen - 1 'set missing dist period to 0 except for accum value at end
                            lFPos = lMJDay - lSJDay + k - 1
                            aTS2Fill.Value(lFPos) = 0
                        Next k
                    End If
                End If
                'lOStr &= ls
                lStr = StrSplit(s, "DETAIL:DATA", "")
            End While
        Else 'no missing data
            'lOStr &= "  No missing data to fill!" & vbCrLf
            Logger.Dbg("  No missing data to fill!")
        End If
        'lFName = "DV_Fill_" & lTS2Fill.Attributes.GetValue("Location") & ".sum"
        'SaveFileString(lFName, lOStr)
    End Sub

    'Use for filling HOURLY OR LESS timeseries.
    'Fill missing values in timeseries aTS2Fill with values from nearby timeseries (aTSAvail).
    'Use Tolerance factor aTol to determine which nearby stations are acceptable
    'for distributing accumulated data (i.e. missing time distribution).
    Public Sub FillHourlyTser(ByVal aTS2Fill As atcTimeseries, ByVal aTSAvail As atcCollection, ByVal aMVal As Double, ByVal aMAcc As Double, ByVal aTol As Double)
        Dim j As Integer
        Dim k As Integer
        Dim lInd As Integer
        Dim lCurInd As Integer
        Dim lFPos As Integer
        Dim lSPos As Integer
        Dim lMLen As Integer
        Dim ld(5) As Integer
        Dim lMTyp As Integer
        Dim lNSta As Integer
        Dim s As String
        Dim lstr As String
        Dim lSJDay As Double
        Dim lMJDay As Double
        Dim lEJDay As Double
        Dim lSubSJDay As Double
        Dim lFVal As Double
        Dim lFillAdjust As Double
        Dim lStaAdjust As Double
        Dim lRatio As Double
        Dim lCarry As Double
        Dim lRndOff As Double = 0.001
        Dim lAccVal As Double
        Dim lMaxHrVal As Double
        Dim lMaxHrInd As Integer
        Dim lDist As atcCollection
        Dim lTSer As atcTimeseries
        Dim lAdjustAttribute As String = GetAdjustingAttribute(aTS2Fill)
        Const lValMin As Double = -90
        Const lValMax As Double = 200
        Dim lTol As Double
        Dim lFilledIt As Boolean
        Dim lTU As Integer = aTS2Fill.Attributes.GetValue("tu")
        Dim lTS As Integer = aTS2Fill.Attributes.GetValue("ts")
        Dim lIntsPerDay As Double
        Dim lTUStr As String = ""

        If lTol > 1 Then 'passed in as percentage
            lTol = lTol / 100
        End If
        lNSta = 10
        Select Case lTU
            Case atcTimeUnit.TUHour
                lIntsPerDay = 24 / lTS
                lTUStr = "Hour"
            Case atcTimeUnit.TUMinute
                lIntsPerDay = 1440 / lTS
                lTUStr = "Minute"
            Case atcTimeUnit.TUSecond
                lIntsPerDay = 86400 / lTS
                lTUStr = "Second"
            Case Else
                Logger.Dbg("  PROBLEM - Time Units of TSer being filled are not hours, minutes, or seconds")
        End Select
        Logger.Dbg("Filling " & lTS & "-" & lTUStr & " values for " & aTS2Fill.ToString & ", " & aTS2Fill.Attributes.GetValue("STANAM"))
        s = MissingDataSummary(aTS2Fill, aMVal, aMAcc, lValMin, lValMax, 2)
        lstr = StrSplit(s, "DETAIL:DATA", "")
        If Len(s) > 0 Then 'missing data found
            lDist = Nothing
            lDist = CalcMetDistances(aTS2Fill, aTSAvail, lAdjustAttribute)
            lFillAdjust = aTS2Fill.Attributes.GetValue(lAdjustAttribute, -999)
            If Math.Abs(lFillAdjust + 999) > pEpsilon Then
                Logger.Dbg("  (Historical average is " & lFillAdjust & ")")
            Else
                Logger.Dbg("  (Historical averages not in use)")
            End If
            lSJDay = aTS2Fill.Attributes.GetValue("SJDay")
            While Len(s) > 0
                lstr = StrSplit(s, ",", "")
                lMJDay = CDbl(StrSplit(s, ",", ""))
                lMLen = CLng(StrSplit(s, ",", ""))
                lMTyp = CLng(StrSplit(s, ",", ""))
                J2Date(lMJDay, ld)
                If lMTyp = 1 Then 'fill missing period
                    Logger.Dbg("  For Missing period starting " & ld(0) & "/" & ld(1) & "/" & ld(2) & " " & ld(3) & " lasting " & lMLen & " intervals:")
                    lCurInd = -1
                    For k = 1 To lMLen
                        lFPos = lIntsPerDay * (lMJDay - lSJDay) + k - 1
                        lFilledIt = False
                        j = 1
                        While j < aTSAvail.Count
                            lInd = lDist.IndexFromKey(CStr(j))
                            lTSer = aTSAvail.ItemByIndex(lInd)
                            lFVal = -1
                            If (lMJDay + (k - 1) / lIntsPerDay) + JulianMillisecond - lTSer.Dates.Values(1) > pEpsilon And _
                               (lMJDay + (k - 1) / lIntsPerDay) - JulianMillisecond <= lTSer.Dates.Values(lTSer.numValues) Then 'check value
                                lSPos = lIntsPerDay * (lMJDay - lTSer.Attributes.GetValue("SJDay")) + k - 1
                                If lTSer.Value(lSPos) <> aMVal AndAlso lTSer.Value(lSPos) > lValMin AndAlso lTSer.Value(lSPos) < lValMax Then 'good value
                                    lFVal = lTSer.Value(lSPos)
                                    lStaAdjust = lTSer.Attributes.GetValue(lAdjustAttribute, -999)
                                    If Math.Abs(lFillAdjust + 999) > pEpsilon AndAlso _
                                       Math.Abs(lStaAdjust + 999) > pEpsilon Then
                                        aTS2Fill.Value(lFPos) = lFillAdjust / lStaAdjust * lFVal
                                    Else
                                        aTS2Fill.Value(lFPos) = lFVal
                                    End If
                                    J2Date(lMJDay + (k - 1) / lIntsPerDay, ld)
                                    If lCurInd <> lInd Then 'changing station used to fill
                                        Logger.Dbg("    Filling from TS " & lTSer.ToString & ", " & lTSer.Attributes.GetValue("STANAM"))
                                        If Math.Abs(lFillAdjust + 999) > 0.0001 Then 'pEpsilon Then
                                            Logger.Dbg("      (Adjusting values using historical average of " & lStaAdjust & ")")
                                        End If
                                        lCurInd = lInd
                                    End If
                                    If lTU = atcTimeUnit.TUHour Then
                                        Logger.Dbg("      " & ld(0) & "/" & ld(1) & "/" & ld(2) & " " & ld(3) & " - " & aTS2Fill.Value(lFPos))
                                    ElseIf lTU = atcTimeUnit.TUMinute Then
                                        Logger.Dbg("      " & ld(0) & "/" & ld(1) & "/" & ld(2) & " " & ld(3) & ":" & ld(4) & " - " & aTS2Fill.Value(lFPos))
                                    ElseIf lTU = atcTimeUnit.TUSecond Then
                                        Logger.Dbg("      " & ld(0) & "/" & ld(1) & "/" & ld(2) & " " & ld(3) & ":" & ld(4) & ":" & ld(5) & " - " & aTS2Fill.Value(lFPos))
                                    End If
                                    j = aTSAvail.Count
                                    lFilledIt = True
                                End If
                            End If
                            j = j + 1
                        End While
                        If Not lFilledIt Then
                            Logger.Dbg("PROBLEM - Could not find acceptable nearby station for filling")
                        End If
                    Next k
                Else 'fill accumulated period
                    If lTU = atcTimeUnit.TUHour Then
                        Logger.Dbg("  For Accumulated period starting " & ld(0) & "/" & ld(1) & "/" & ld(2) & " " & ld(3) & " lasting " & lMLen & " intervals:")
                    ElseIf lTU = atcTimeUnit.TUMinute Then
                        Logger.Dbg("  For Accumulated period starting " & ld(0) & "/" & ld(1) & "/" & ld(2) & " " & ld(3) & ":" & ld(4) & " lasting " & lMLen & " intervals:")
                    ElseIf lTU = atcTimeUnit.TUSecond Then
                        Logger.Dbg("  For Accumulated period starting " & ld(0) & "/" & ld(1) & "/" & ld(2) & " " & ld(3) & ":" & ld(4) & ":" & ld(5) & " lasting " & lMLen & " intervals:")
                    End If
                    lAccVal = CDbl(StrSplit(s, vbCrLf, ""))
                    lSubSJDay = lMJDay - 1 / lIntsPerDay 'back up one interval for SubSetByDate in ClosestPrecip
                    lEJDay = lSubSJDay + lMLen / lIntsPerDay
                    lTSer = ClosestPrecip(aTS2Fill, aTSAvail, lAccVal, lSubSJDay, lEJDay, aTol)
                    If Not lTSer Is Nothing Then
                        Logger.Dbg("    Distributing " & lAccVal & " from TS# " & lTSer.ToString)
                        lRatio = lAccVal / lTSer.Attributes.GetValue("Sum")
                        For k = 1 To lMLen
                            lFPos = lIntsPerDay * (lMJDay - lSJDay) + k - 1
                            aTS2Fill.Value(lFPos) = lRatio * lTSer.Value(k) + lCarry
                            If aTS2Fill.Value(lFPos) > pEpsilon Then
                                lCarry = aTS2Fill.Value(lFPos) - (Math.Round(aTS2Fill.Value(lFPos) / lRndOff) * lRndOff)
                                aTS2Fill.Value(lFPos) = aTS2Fill.Value(lFPos) - lCarry
                            Else
                                aTS2Fill.Value(lFPos) = 0.0#
                            End If
                            If aTS2Fill.Value(lFPos) > lMaxHrVal Then
                                lMaxHrVal = aTS2Fill.Value(lFPos)
                                lMaxHrInd = lFPos
                            End If
                            J2Date(lMJDay + (k - 1) / lIntsPerDay, ld)
                            If lTU = atcTimeUnit.TUHour Then
                                Logger.Dbg("      " & ld(0) & "/" & ld(1) & "/" & ld(2) & " " & ld(3) & " - " & aTS2Fill.Value(lFPos))
                            ElseIf lTU = atcTimeUnit.TUMinute Then
                                Logger.Dbg("      " & ld(0) & "/" & ld(1) & "/" & ld(2) & " " & ld(3) & ":" & ld(4) & " - " & aTS2Fill.Value(lFPos))
                            ElseIf lTU = atcTimeUnit.TUSecond Then
                                Logger.Dbg("      " & ld(0) & "/" & ld(1) & "/" & ld(2) & " " & ld(3) & ":" & ld(4) & ":" & ld(5) & " - " & aTS2Fill.Value(lFPos))
                            End If
                        Next k
                        If lCarry > 0 Then 'add remainder to max hourly value
                            aTS2Fill.Value(lMaxHrInd) = aTS2Fill.Value(lMaxHrInd) + lCarry
                        End If
                    Else
                        J2Date(lEJDay, ld)
                        Logger.Dbg("      *** No nearby station found for distributing accumulated value of " & lAccVal & " on " & ld(0) & "/" & ld(1) & "/" & ld(2))
                        For k = 1 To lMLen - 1 'set missing dist period to 0 except for accum value at end
                            lFPos = lIntsPerDay * (lMJDay - lSJDay) + k - 1
                            aTS2Fill.Value(lFPos) = 0
                        Next k
                    End If
                End If
                lstr = StrSplit(s, "DETAIL:DATA", "")
            End While
        Else 'no missing data
            Logger.Dbg("  No missing data to fill!")
        End If
    End Sub

    Public Function GetAdjustingAttribute(ByVal aTSer As atcTimeseries) As String
        'based on input TSer's constituent, try to find an adjustment
        'factor attribute based on a historical average for that constituent
        Dim lCons As String = aTSer.Attributes.GetValue("Constituent")

        Select Case lCons
            Case "PREC", "HPCP", "PRCP", "HPRC", "HPCP1"
                Return "PRECIP"
            Case "TEMP", "HTMP", "ATMP", "ATEM", "DTMP", "DPTP", "DEWP", "DWPT", "TMIN", "TMAX"
                Return "UBC190" 'user defined att for temperature adjustment
            Case "CLOU"
                'don't use adjusting attribute for cloud cover
                'otherwise, values greater than valid max (10) can occur
                Return ""
            Case Else 'just use average for timeseries
                Return "MEAN"
        End Select

    End Function

    Public Function CalcMetDistances(ByVal aTSer As atcTimeseries, ByVal aStations As atcCollection, Optional ByVal aAdjustAttribute As String = "") As atcCollection
        'calculate distance between a single station (lTSer) and a collection of nearby Stations
        'considering both geographic distance and difference between historical averages 
        'stored on attribute aAdjustAttribute, from PRISM or other source
        'NOTE: Geographic distances may be passed in as keys for each nearby stations or
        'calculated from WDM attributes used by module GeoDist (UBC024 and UBC025)
        Dim i As Integer
        Dim lAnnAdj As Double
        Dim lTSerAdjust As Double
        Dim lStaAdjust As Double
        Dim lCompareAnnuals As Boolean
        Dim lStaTSer As atcTimeseries
        Dim lGDist As Double
        Dim lDistance(aStations.Count) As Double
        Dim lOrder(aStations.Count) As Integer
        Dim lRank(aStations.Count) As Integer
        Dim lDist As New atcCollection

        If aAdjustAttribute.Length > 0 Then
            lTSerAdjust = aTSer.Attributes.GetValue(aAdjustAttribute, -999)
        Else
            lTSerAdjust = -999
        End If
        If Math.Abs(lTSerAdjust + 999) < pEpsilon Then
            lCompareAnnuals = False
        Else
            lCompareAnnuals = True
        End If
        'stations part of 0-based collection
        For i = 0 To aStations.Count - 1
            lAnnAdj = 1 'assume no adjustment for annual averages
            lStaTSer = aStations.ItemByIndex(i)
            If lCompareAnnuals Then
                lStaAdjust = lStaTSer.Attributes.GetValue(aAdjustAttribute, -999)
                If Math.Abs(lStaAdjust + 999) > pEpsilon Then
                    lAnnAdj = lTSerAdjust / lStaAdjust
                    If lAnnAdj < 1 Then lAnnAdj = 1 / lAnnAdj
                End If
            End If
            If IsNumeric(aStations.Keys.Item(i)) Then 'distance value already exists
                lGDist = aStations.Keys.Item(i)
            Else 'try to calculate distance from attributes
                lGDist = GeoDist(aTSer, lStaTSer) 'now calculate geographic distance
            End If
            'distance (and order and rank) array is 1-based
            lDistance(i + 1) = lAnnAdj * lGDist
        Next i
        'sort modules are 1-based
        SortRealArray(0, aStations.Count, lDistance, lOrder)
        SortIntegerArray(0, aStations.Count, lOrder, lRank)
        For i = 1 To aStations.Count
            lDist.Add(CStr(lRank(i)), lDistance(i))
        Next i
        Return lDist
    End Function

    Public Function GeoDist(ByVal aTS1 As atcTimeseries, ByVal aTS2 As atcTimeseries) As Single
        'calculate the geographic distance between two stations (TSers)
        'based on projected feet ()
        Dim X1 As Double = aTS1.Attributes.GetValue("UBC024")
        Dim Y1 As Double = aTS1.Attributes.GetValue("UBC025")
        Dim X2 As Double = aTS2.Attributes.GetValue("UBC024")
        Dim Y2 As Double = aTS2.Attributes.GetValue("UBC025")
        If X1 = 0 Or X2 = 0 Or Y1 = 0 Or Y2 = 0 Then 'something wrong, make it huge
            GeoDist = 1.0E+30
        Else
            GeoDist = Math.Sqrt((X1 - X2) ^ 2 + (Y1 - Y2) ^ 2)
        End If
    End Function

    Public Function CurrentObsTime(ByVal aTS As atcTimeseries, ByVal aPos As Integer) As Integer
        'given a daily Timeseries and a position in its data,
        'return the current observation time
        Dim lOT As Integer = -1
        Dim lInc As Integer = -1 'look back in values if initial "pos" value is undefined
        Dim lCPos As Integer = aPos

        While lOT < 0
            If aTS.Value(lCPos) > 0 And aTS.Value(lCPos) < 25 Then
                lOT = aTS.Value(lCPos)
            Else
                lCPos = lCPos + lInc
            End If
            If lCPos < 1 Then 'at beginning of values, look ahead of initial "pos"
                lInc = 1
                lCPos = aPos + 1
            ElseIf lCPos > aTS.numValues Then 'at end of values, no obs time found, set to 24
                lOT = 24
            End If
        End While
        Return lOT
    End Function

    Public Function ClosestPrecip(ByVal aTSer As atcTimeseries, ByVal aStations As atcCollection, ByVal aDV As Double, ByVal aSDt As Double, ByVal aEDt As Double, ByVal aTol As Double, Optional ByVal aObsTime As Integer = 24) As atcTimeseries
        'find the precip station (from the collection aStations) that is closest to
        'a precip station being filled (aTSer) considering both geographic distance and
        'difference between totals for the time specified by sdt and edt
        '(aStation totals adjusted based on PRISM average annual values)
        'ratio of totals must be within specified tolerance (aTol)
        Dim i As Integer
        Dim lTSerAnn As Double = aTSer.Attributes.GetValue("PRECIP")
        Dim lStaAnn As Double
        Dim lCompareAnnuals As Boolean
        Dim lGDist As Double
        Dim lPrecDist As Double
        Dim lClosestDist As Double = pMaxValue
        Dim lSumTSer As atcTimeseries
        Dim lStaTSer As atcTimeseries
        Dim lPrecTSer As atcTimeseries
        Dim lSum As Double
        Dim lRatio As Double
        Dim lInd As Integer
        Dim lFillDaily As Boolean = aTSer.Attributes.GetValue("TU") = atcTimeUnit.TUDay
        Dim lLoc As String = aTSer.Attributes.GetValue("Location")
        Dim lCons As String = aTSer.Attributes.GetValue("Constituent")
        Dim lSDt As Double
        Dim lEDt As Double
        Static lDist As atcCollection
        Static lCurLoc As String
        Static lCurCons As String

        If lLoc <> lCurLoc OrElse lCons <> lCurCons Then 'calculate station distances
            lDist = New atcCollection
            Dim lDistance(aStations.Count) As Double
            Dim lOrder(aStations.Count) As Integer
            Dim lRank(aStations.Count) As Integer
            Dim lAnnAdj As Double
            If lTSerAnn = 0 Then
                lCompareAnnuals = False
            Else
                lCompareAnnuals = True
            End If
            'stations part of 0-based collection
            For i = 0 To aStations.Count - 1
                lAnnAdj = 1 'assume no adjustment for annual averages
                lStaTSer = aStations.ItemByIndex(i)
                If lCompareAnnuals Then
                    lStaAnn = lStaTSer.Attributes.GetValue("PRECIP")
                    If lStaAnn > 0 Then
                        lAnnAdj = lTSerAnn / lStaAnn
                        If lAnnAdj < 1 Then lAnnAdj = 1 / lAnnAdj
                    End If
                End If
                If IsNumeric(aStations.Keys.Item(i)) Then 'distance value already exists
                    lGDist = aStations.Keys.Item(i)
                Else 'try to calculate distance from attributes
                    lGDist = GeoDist(aTSer, lStaTSer) 'now calculate geographic distance
                End If
                'distance (and order and rank) array is 1-based
                lDistance(i + 1) = lAnnAdj * lGDist
            Next i
            'sort modules are 1-based
            SortRealArray(0, aStations.Count, lDistance, lOrder)
            SortIntegerArray(0, aStations.Count, lOrder, lRank)
            For i = 1 To aStations.Count
                lDist.Add(CStr(lRank(i)), lDistance(i))
            Next i
            lCurLoc = lLoc
            lCurCons = lCons
        End If

        lPrecTSer = Nothing
        For i = 1 To aStations.Count 'look through closest nsta stations for an acceptable daily total
            lInd = lDist.IndexFromKey(CStr(i))
            lStaTSer = aStations.ItemByIndex(lInd)
            If lFillDaily And lStaTSer.Attributes.GetValue("TU") < aTSer.Attributes.GetValue("TU") Then
                'filling daily with less than daily time step, use obs time to set date subset
                'calling routine (FillDailyTSer) backed up start date, so adjust forward with ObsTime
                lSDt = aSDt + aObsTime
                lEDt = aEDt + aObsTime
            Else
                lSDt = aSDt
                lEDt = aEDt
            End If
            If lStaTSer.Attributes.GetValue("SJDay") <= lSDt AndAlso _
               lStaTSer.Attributes.GetValue("EJDay") >= lEDt Then
                'tser contains time span being filled
                lSumTSer = SubsetByDate(lStaTSer, lSDt, lEDt, Nothing)
                lSum = lSumTSer.Attributes.GetValue("Sum")
                If lSum > 0 Then
                    If lCompareAnnuals Then
                        lStaAnn = lSumTSer.Attributes.GetValue("PRECIP")
                        If lStaAnn > 0 Then
                            lSum = lSum * lTSerAnn / lStaAnn
                        End If
                    End If
                    lRatio = aDV / lSum
                    If lRatio > 1 Then lRatio = 1 / lRatio
                    If lRatio >= 1 - aTol Then 'acceptable total, include distance factor
                        'lGDist = GeoDist(aTSer, lSumTSer) 'now calculate geographic distance
                        'try 2 as a "weighting factor" for daily precip ratio
                        lPrecDist = (2 - lRatio) * lDist(lInd) 'lGDist
                        If lPrecDist < lClosestDist Then
                            lClosestDist = lPrecDist
                            lPrecTSer = lSumTSer
                        End If
                    End If
                End If
            End If
        Next i
        Return lPrecTSer
    End Function

    Public Function MissingDataSummary(ByVal aTSer As atcTimeseries, Optional ByVal aMVal As Double = -9.99, Optional ByVal aMAcc As Double = -9.98, Optional ByVal aFMin As Double = -1, Optional ByVal aFMax As Double = 90000, Optional ByVal aRepTyp As Integer = 0) As String
        'scan aTSer for missing/accumulated/faulty data 
        'as defined by aMVal, aMAcc, and aFMin/aFMax
        'returns string of summary in 3 different output forms based on aRepTyp:
        '0 - standard text output
        '1 - DBF parsing form, overall summary only
        '2 - DBF parsing form, detailed summary
        Dim lMisPd As Integer
        Dim lMPos As Integer
        Dim lMCod As Integer
        Dim lNMVals As Integer
        Dim lPos As Integer
        Dim lTotMissIncs As Integer
        Dim lNMVPd As Integer = 0
        Dim lNMDPd As Integer = 0
        Dim lNBVPd As Integer = 0
        Dim lTotMV As Integer = 0
        Dim lTotMD As Integer = 0
        Dim lTotBV As Integer = 0
        Dim lAccVal As Double
        Dim ldate(5) As Integer
        Dim lDateStr As String
        Dim lDateStrEnd As String
        Dim lTSStr As String
        Dim lTUStr() As String = {"", "seconds", "minutes", "hours", "days", "months", "years"}
        Dim s As New System.Text.StringBuilder
        Dim ls As String = ""

        If aRepTyp = 0 Then
            s.Append("For Data-set " & aTSer.ToString)
            ls = aTSer.Attributes.GetValue("Description")
            If Len(ls) > 0 Then
                s.Append(", " & ls)
            End If
            s.Append(vbCrLf)
        Else
            s.Append("STATION:HEADER  Index, Scenario, Location, Constituent, Description, TStep, TUnits" & vbCrLf)
            s.Append("STATION:DATA  " & aTSer.Attributes.GetFormattedValue("ID") & ", " & _
                                       aTSer.Attributes.GetValue("Scenario") & ", " & _
                                       aTSer.Attributes.GetValue("Location") & ", " & _
                                       aTSer.Attributes.GetValue("Constituent") & ", " & _
                                       aTSer.Attributes.GetValue("Description") & ", " & _
                                       aTSer.Attributes.GetValue("TS") & ", " & _
                                       aTSer.Attributes.GetValue("TU") & vbCrLf)
        End If
        'build time step/units string for display
        lTSStr = lTUStr(aTSer.Attributes.GetValue("TU"))  'start with time units
        If aTSer.Attributes.GetValue("TS") > 1 Then 'need time step too
            lTSStr = aTSer.Attributes.GetValue("TS") & " " & lTSStr
            'remove plural from time units, add 'intervals'
            lTSStr = lTSStr.TrimEnd("s") & " intervals"
        End If
        lMisPd = 0
        lMCod = 1
        lPos = 1
        Do While lMCod > 0
            Call NxtMis(lPos, aTSer.Values, aMVal, aMAcc, aFMin, aFMax, lMCod, lMPos, lNMVals, lAccVal)
            If lMCod > 0 Then 'somethings missing
                J2Date(aTSer.Dates.Value(lMPos), ldate)
                lDateStr = ldate(0) & "/" & ldate(1) & "/" & ldate(2)
                If aTSer.Attributes.GetValue("TU") < 4 Then 'display h,m,s along with y,m,d
                    lDateStr &= " " & ldate(3) & ":" & ldate(4) & ":" & ldate(5)
                End If
                If lMCod = 1 Then 'missing values
                    ls = " of missing values starting "
                    lNMVPd += 1
                    lTotMV += lNMVals
                ElseIf lMCod = 2 Then 'accumulated values
                    ls = " of missing time distribution starting "
                    lNMDPd += 1
                    lTotMD += lNMVals
                ElseIf lMCod = 3 Then 'screwball values
                    ls = " of faulty values starting "
                    lNBVPd += 1
                    lTotBV += lNMVals
                End If
                lMisPd += 1
                lPos = lMPos + lNMVals
                If aRepTyp = 0 Then
                    s.Append("  " & lNMVals & " " & lTSStr & ls & lDateStr & vbCrLf)
                ElseIf aRepTyp = 2 Then
                    s.Append("DETAIL:HEADER  Index, Start, Length, Type, AccumVal" & vbCrLf)
                    s.Append("DETAIL:DATA  " & lMisPd & ", " & Date2J(ldate) & ", " & lNMVals & ", " & lMCod & ", " & lAccVal & vbCrLf)
                End If
            End If
        Loop
        lTotMissIncs = lTotMV + lTotMD + lTotBV
        If aRepTyp = 0 Then
            If lMisPd > 0 Then 'missing data found for this time series
                s.Append(lMisPd & " period(s) of missing or bad data." & vbCrLf)
                s.Append("TotalIncs:" & RightJustify(aTSer.numValues, 8) & vbCrLf & _
                     "  MisValPd:" & RightJustify(lNMVPd, 9) & vbCrLf & _
                     "  MisValTot:" & RightJustify(lTotMV, 8) & vbCrLf & _
                     "  MisDisPd:" & RightJustify(lNMDPd, 9) & vbCrLf & _
                     "  MisDisTot: " & RightJustify(lTotMD, 7) & vbCrLf & _
                     "  BadValPd:" & RightJustify(lNBVPd, 9) & vbCrLf & _
                     "  BadValTot:" & RightJustify(lTotBV, 8) & vbCrLf & _
                     "  PctMiss: " & DoubleToString(100 * lTotMissIncs / aTSer.numValues, 9, "##0.0000", , , 5) & vbCrLf)
            Else 'nothing missing
                s.Append("  No missing data for this time series" & vbCrLf)
            End If
        Else
            J2Date(aTSer.Dates.Value(0), ldate)
            lDateStr = ldate(0) & "/" & ldate(1) & "/" & ldate(2)
            J2Date(aTSer.Dates.Value(aTSer.numValues), ldate)
            lDateStrEnd = ldate(0) & "/" & ldate(1) & "/" & ldate(2)
            s.Append("SUMMARY:HEADER  Start, End, TotalIncs, TotalMissPd, TotalMissIncs, MisValPd, MisValTot, MisDisPd, MisDisTot, BadValPd, BadValTot, PctMiss" & vbCrLf)
            s.Append("SUMMARY:DATA  " & lDateStr & ", " & lDateStrEnd & ", " & _
                                    aTSer.numValues & ", " & lMisPd & ", " & lTotMissIncs & ", " & _
                                    lNMVPd & ", " & lTotMV & ", " & lNMDPd & ", " & lTotMD & ", " & _
                                    lNBVPd & ", " & lTotBV & ", " & DoubleToString(100 * lTotMissIncs / aTSer.numValues, 6, "##0.0000", , , 5) & vbCrLf)
        End If
        Return s.ToString

    End Function

    Public Sub NxtMis(ByVal aBufPos As Integer, ByVal aDBuff() As Double, ByVal aValMis As Double, ByVal aValAcc As Double, ByVal aFaultMin As Double, ByVal aFaultMax As Double, ByRef aMisCod As Integer, ByRef aMisPos As Integer, ByRef aNVals As Integer, ByRef aMVal As Double)

        'Find the next missing value or distribution or screwball value
        'in the data buffer (aDBUFF) starting at positions aBufPos.
        'If none found, return MISCOD = 0.

        'aBufPos  - size of data buffer array
        'aDBUFF   - array of data values in which to look for missing data
        'aValMis  - value indicating missing data values
        'aValAcc  - value indicating accumulated values (missing distribution)
        'aFaultMin- value indicating value is too low to be valid
        'aFaultMax- value indicating value is too high to be valid
        'aMisCod  - type of missing data
        '           1 - missing value
        '           2 - missing distribution
        '           3 - garbage value
        'aMisPos  - position in array where missing data starts
        'aNVals   - number of data values missing
        'aMVal    - accumulated value (for accumulated data) or dummy
        '           for other types of missing data

        Dim i As Integer = aBufPos
        Dim lMisFlg As Integer = 0
        Dim lAccFlg As Integer = 0
        Dim lBadFlg As Integer = 0
        Dim lDonFlg As Integer = 0

        Do While lDonFlg = 0 And i < aDBuff.Length
            If Double.IsNaN(aDBuff(i)) Or Math.Abs(aDBuff(i) - aValMis) < pEpsilon Then
                'we have a missing value
                If lAccFlg > 0 Then
                    'we were in the midst of an accum value array,
                    'change it all to missing because dont know total
                    lMisFlg = lAccFlg
                    lAccFlg = 0
                End If
                lMisFlg += 1
                If lMisFlg = 1 Then
                    'first missing value, save start position
                    aMisPos = i
                End If
            ElseIf Math.Abs(aDBuff(i) - aValAcc) < pEpsilon Then
                'we have an accumulated value
                If lMisFlg > 0 Then
                    'report only missing values now, will get accum next time
                    lDonFlg = 1
                Else
                    'accumulated value w/no distribution
                    lAccFlg += 1
                    If lAccFlg = 1 Then
                        'first accumulated value, save start position
                        aMisPos = i
                    End If
                End If
            ElseIf aDBuff(i) < aFaultMin Or aDBuff(i) > aFaultMax Then
                'we have a screwball value
                lBadFlg += 1
                If lBadFlg = 1 Then
                    'first bad value, save start position
                    aMisPos = i
                End If
            Else
                'no problem with this value
                If lMisFlg > 0 Or lAccFlg > 0 Or lBadFlg > 0 Then
                    'end of missing period
                    lDonFlg = 1
                    If lAccFlg > 0 Then
                        'include first good value in accumulated count
                        lAccFlg += 1
                    End If
                End If
            End If
            'check next data value
            i += 1
        Loop

        If lMisFlg > 0 Then
            'missing values
            aMisCod = 1
            aNVals = lMisFlg
            aMVal = pNaN
        ElseIf lAccFlg > 0 Then
            'missing distribution
            aMisCod = 2
            aNVals = lAccFlg
            'save accumulated value for possible distribution later
            aMVal = aDBuff(i - 1)
        ElseIf lBadFlg > 0 Then
            'screwball values
            aMisCod = 3
            aNVals = lBadFlg
            aMVal = -1.0E+30
        Else
            'no missing data in buffer
            aMisCod = 0
            aMisPos = 0
            aNVals = 0
            aMVal = 0.0#
        End If

    End Sub

    Public Sub BuildDBFSummary(ByVal aSummStr As String, ByVal aFName As String)
        Dim i As Integer ', j As Long, TSCount As Long, 
        Dim lFldLen As Integer
        'Dim tmpv As Long, maxv As Long, 
        Dim lTmpStr As String
        Dim lStr As String
        Dim lFldNames As New atcCollection
        Dim lFldTypes As New atcCollection
        Dim lFirstVals As New atcCollection
        Dim lDBF As New atcTableDBF

        Logger.Dbg("BuildDBFSummary:Start")
        'maxv = -1000000.0#
        'TSCount = 0
        'i = InStr(s, "SUMMARY:DATA")
        'While i > 0
        '  i = i + 12
        '  j = InStr(i, s, ",")
        '  tmpv = CLng(Mid(s, i, j - i))
        '  If tmpv > maxv Then maxv = tmpv
        '  TSCount = TSCount + 1
        '  i = InStr(j, s, "SUMMARY:DATA")
        'End While
        'tmpstr = maxv
        'FldLen = Len(tmpstr)
        lFldLen = 6
        'build dbf file
        'dbf.NumRecords = TSCount
        'extract field names, first from Station record
        lTmpStr = StrSplit(aSummStr, "STATION:HEADER", "")
        lTmpStr = StrSplit(aSummStr, vbCrLf, "")
        While Len(lTmpStr) > 0
            lFldNames.Add(StrSplit(lTmpStr, ",", ""))
        End While
        Logger.Dbg("BuildDBFSummary:Extracted field names from Station record")

        lTmpStr = StrSplit(aSummStr, "STATION:DATA", "")
        lTmpStr = StrSplit(aSummStr, vbCrLf, "")
        While Len(lTmpStr) > 0
            lStr = StrSplit(lTmpStr, ",", "")
            If lStr.Length > 1 And lStr.StartsWith("0") Then  'preceeding 0 usually means character field
                lFldTypes.Add("C")
            ElseIf Not IsNumeric(lStr) Then
                lFldTypes.Add("C")
            Else
                lFldTypes.Add("N")
            End If
            lFirstVals.Add(lStr)
        End While
        Logger.Dbg("BuildDBFSummary:Extracted field types from Station record")

        'now from Summary record
        lTmpStr = StrSplit(aSummStr, "SUMMARY:HEADER", "")
        lTmpStr = Trim(StrSplit(aSummStr, vbCrLf, ""))
        While Len(lTmpStr) > 0
            lFldNames.Add(StrSplit(lTmpStr, ",", ""))
        End While
        Logger.Dbg("BuildDBFSummary:Extracted field names from Summary record")

        lTmpStr = StrSplit(aSummStr, "SUMMARY:DATA", "")
        lTmpStr = StrSplit(aSummStr, vbCrLf, "")
        While Len(lTmpStr) > 0
            lStr = StrSplit(lTmpStr, ",", "")
            If lStr.IndexOf("/") > 0 Then 'date field
                lFldTypes.Add("D")
            ElseIf lStr.Length > 1 And lStr.StartsWith("0") Then  'preceeding 0 usually means character field
                lFldTypes.Add("C")
            ElseIf Not IsNumeric(lStr) Then
                lFldTypes.Add("C")
            Else
                lFldTypes.Add("N")
            End If
            lFirstVals.Add(lStr)
        End While
        Logger.Dbg("BuildDBFSummary:Extracted field types names from Summary record")
        lDBF.NumFields = lFldNames.Count
        Logger.Dbg("BuildDBFSummary:Set number of fields")

        For i = 0 To lDBF.NumFields - 1
            lDBF.FieldType(i + 1) = lFldTypes.ItemByIndex(i)
            lDBF.FieldName(i + 1) = lFldNames.ItemByIndex(i)
            If lFldTypes.ItemByIndex(i) = "N" Then
                lDBF.FieldLength(i + 1) = lFldLen 'use length of max number for numeric fields
            ElseIf lFldTypes.ItemByIndex(i) = "D" Then
                lDBF.FieldLength(i + 1) = 10
            Else
                If lFldNames.ItemByIndex(i) = "Description" Then
                    lDBF.FieldLength(i + 1) = 32
                Else 'all other character fields use width of 8
                    lDBF.FieldLength(i + 1) = 8
                End If
            End If
            If i < lDBF.NumFields Then
                lDBF.FieldDecimalCount(i + 1) = 0
            Else
                lDBF.FieldDecimalCount(i + 1) = 1 'last field (percent missing) needs a decimal
            End If
        Next i
        Logger.Dbg("BuildDBFSummary:Set DBF Field info")

        lDBF.InitData()
        lDBF.CurrentRecord = 1
        Logger.Dbg("BuildDBFSummary:Initialized data for DBF")
        For i = 0 To lDBF.NumFields - 1
            lDBF.Value(i + 1) = lFirstVals.ItemByIndex(i)
        Next i
        Logger.Dbg("BuildDBFSummary:Set first record values")

        While Len(aSummStr) > 0
            lDBF.CurrentRecord = lDBF.CurrentRecord + 1
            lTmpStr = StrSplit(aSummStr, "STATION:DATA", "")
            lTmpStr = StrSplit(aSummStr, vbCrLf, "")
            i = 0
            While Len(lTmpStr) > 0
                i += 1
                lDBF.Value(i) = StrSplit(lTmpStr, ",", "")
            End While
            lTmpStr = StrSplit(aSummStr, "SUMMARY:DATA", "")
            lTmpStr = StrSplit(aSummStr, vbCrLf, "")
            While Len(lTmpStr) > 0
                i += 1
                lDBF.Value(i) = StrSplit(lTmpStr, ",", "")
            End While
        End While
        Logger.Dbg("BuildDBFSummary:set all record values")

        lDBF.WriteFile(aFName)
        Logger.Dbg("BuildDBFSummary:Wrote DBF File")
    End Sub

    Public Sub SortIntegerArray(ByVal aOpt As Integer, ByVal aCnt As Integer, ByRef aIVal() As Integer, ByRef aPos() As Integer)
        ' ##SUMMARY Sorts integers in array into ascending order.
        ' ##PARAM aOpt I Sort option (0 = sort in place, 1 = move values in array to sorted position)
        ' ##PARAM aCnt I Count of integers to sort
        ' ##PARAM aIVal M Array of integers to sort
        ' ##PARAM aPos O Array containing sorted order of integers
        Dim i As Long
        Dim j As Long
        Dim jpt As Long
        Dim jpt1 As Long
        Dim itmp As Long
        ' ##LOCAL i - long counter for outer loop
        ' ##LOCAL j - long counter for inner loop
        ' ##LOCAL jpt - long pointer to j index
        ' ##LOCAL jpt1 - long pointer to (j + 1) index
        ' ##LOCAL itmp - long temporary holder for values in iVal array

        'set default positions(assume in order)
        For i = 1 To aCnt
            aPos(i) = i
        Next i

        'make a pointer to values with bubble sort
        For i = aCnt To 2 Step -1
            For j = 1 To i - 1
                jpt = aPos(j)
                jpt1 = aPos(j + 1)
                If (aIVal(jpt) > aIVal(jpt1)) Then
                    aPos(j + 1) = jpt
                    aPos(j) = jpt1
                End If
            Next j
        Next i

        If (aOpt = 1) Then
            'move integer values to their sorted positions
            For i = 1 To aCnt
                If (aPos(i) <> i) Then
                    'need to move ints, first save whats in target space
                    itmp = aIVal(i)
                    'move sorted data to target position
                    aIVal(i) = aIVal(aPos(i))
                    'move temp data to source position
                    aIVal(aPos(i)) = itmp
                    'find the pointer to the other value we are moving
                    j = i
50:                 'CONTINUE
                    j = j + 1
                    If (aPos(j) <> i) Then GoTo 50
                    aPos(j) = aPos(i)
                    aPos(i) = i
                End If
            Next i
        End If

    End Sub

    Public Sub SortRealArray(ByVal aOpt As Integer, ByVal aCnt As Integer, _
                             ByRef aRVal() As Double, ByRef aPos() As Integer)
        ' ##SUMMARY Sorts array of real numbers into ascending order.
        ' ##PARAM aOpt I Integer indicating sort option: 0 - sort in place, 
        '                                                1 - move values in array to sorted position.
        ' ##PARAM aCnt I Count of real numbers to sort.
        ' ##PARAM aRVal M Array of real numbers to sort.
        ' ##PARAM aPos O Integer array containing sorted order of real numbers.
        Dim i As Long
        Dim j As Long
        Dim jpt As Long
        Dim jpt1 As Long
        Dim rtmp As Single
        ' ##LOCAL i - long counter for outer loop
        ' ##LOCAL j - long counter for inner loop
        ' ##LOCAL jpt - long pointer to j index
        ' ##LOCAL jpt1 - long pointer to (j + 1) index
        ' ##LOCAL itmp - long temporary holder for values in rVal array

        'set default positions(assume in order)
        For i = 1 To aCnt
            aPos(i) = i
        Next i

        'make a pointer to values with bubble sort
        For i = aCnt To 2 Step -1
            For j = 1 To i - 1
                jpt = aPos(j)
                jpt1 = aPos(j + 1)
                If (aRVal(jpt) > aRVal(jpt1)) Then
                    aPos(j + 1) = jpt
                    aPos(j) = jpt1
                End If
            Next j
        Next i

        If (aOpt = 1) Then
            'move real values to their sorted positions
            For i = 1 To aCnt
                If (aPos(i) <> i) Then
                    'need to move reals, first save whats in target space
                    rtmp = aRVal(i)
                    'move sorted data to target position
                    aRVal(i) = aRVal(aPos(i))
                    'move temp data to source position
                    aRVal(aPos(i)) = rtmp
                    'find the pointer to the other value we are moving
                    j = i
50:                 'CONTINUE
                    j = j + 1
                    If (aPos(j) <> i) Then GoTo 50
                    aPos(j) = aPos(i)
                    aPos(i) = i
                End If
            Next i
        End If

    End Sub

    Public Function ReadNOAAAttributes(ByVal aFilename As String) As atcCollection
        Dim lStaCode As String
        Dim lStateCode As String
        Dim lDecDeg As Double
        Dim lNOAAattribs As atcCollection
        Dim lStations As New atcCollection
        Dim lStationIndex As Integer
        Dim lCurState As String = ""
        Dim lStart As Object
        Dim lOldStation As atcCollection

        For Each lStr As String In LinesInFile(aFilename)
            lStaCode = lStr.Substring(0, 6)
            If IsNumeric(lStaCode) Then
                lStateCode = lStr.Substring(59, 2)
                If lStaCode > 0 And lStateCode.Length > 0 Then ' lStateCode.ToUpper = "GA" Then
                    If lStateCode <> lCurState Then
                        Logger.Dbg("ReadNOAAAttributes:  New State is " & lStateCode)
                        lCurState = lStateCode
                    End If
                    lNOAAattribs = New atcCollection
                    lNOAAattribs.Add("STAID", lStaCode)
                    lNOAAattribs.Add("STCODE", lStateCode)
                    lNOAAattribs.Add("WBAN", lStr.Substring(10, 5))
                    lNOAAattribs.Add("WMO", lStr.Substring(16, 5))
                    lNOAAattribs.Add("ICAO", lStr.Substring(33, 4))
                    lNOAAattribs.Add("COCODE", lStr.Substring(62, 30))
                    lNOAAattribs.Add("TIME", lStr.Substring(93, 4))
                    lNOAAattribs.Add("STANAM", lStr.Substring(99, 30))
                    'lNOAAattribs.Add("DESCRP", Mid(lStr, 143, 30))
                    lNOAAattribs.Add("START", lStr.Substring(130, 8))
                    lNOAAattribs.Add("END", lStr.Substring(139, 8))
                    If IsNumeric(lStr.Substring(148, 3)) Then
                        lDecDeg = CLng(lStr.Substring(148, 3))
                        If lDecDeg > 0 Then
                            lDecDeg = lDecDeg + CLng(lStr.Substring(152, 2)) / 60 + CLng(lStr.Substring(155, 2)) / 3600
                        Else
                            lDecDeg = lDecDeg - (CLng(lStr.Substring(152, 2)) / 60 + CLng(lStr.Substring(155, 2)) / 3600)
                        End If
                        lNOAAattribs.Add("LATDEG", lDecDeg)
                    End If
                    If IsNumeric(lStr.Substring(158, 4)) Then
                        lDecDeg = CLng(lStr.Substring(158, 4))
                        If lDecDeg > 0 Then
                            lDecDeg = lDecDeg + CLng(lStr.Substring(163, 2)) / 60 + CLng(lStr.Substring(166, 2)) / 3600
                        Else
                            lDecDeg = lDecDeg - (CLng(lStr.Substring(163, 2)) / 60 + CLng(lStr.Substring(166, 2)) / 3600)
                        End If
                        lNOAAattribs.Add("LNGDEG", lDecDeg)
                    End If
                    lNOAAattribs.Add("ELEV", lStr.Substring(169, 5))
                    lStationIndex = lStations.IndexFromKey(lStaCode)
                    If lStationIndex >= 0 Then 'If there is an older station history entry, overwrite it
                        If CDbl(lNOAAattribs.ItemByKey("END")) > CDbl(lStations.ItemByIndex(lStationIndex).ItemByKey("END")) Then
                            lOldStation = lStations.ItemByIndex(lStationIndex)
                            lStart = lOldStation.ItemByKey("START") 'get original start date
                            lNOAAattribs.RemoveByKey("START") 'remove new station start date
                            lNOAAattribs.Add("START", lStart) 'set new station start date to original
                            lStations.RemoveAt(lStationIndex)
                            lStations.Add(lStaCode, lNOAAattribs)
                        End If
                    Else
                        lStations.Add(lStaCode, lNOAAattribs)
                    End If
                End If
            End If
        Next
        Return lStations
    End Function

    Public Function ReadNOAAHPDAttributes(ByVal aFilename As String) As atcCollection
        Dim lStaCode As String
        Dim lStateCode As String
        Dim lDecDeg As Double
        Dim NOAAHPDAttribs As atcCollection
        Dim lStations As New atcCollection
        Dim lStationIndex As Integer
        Dim lCurState As String = ""

        For Each lStr As String In LinesInFile(aFilename)
            lStaCode = lStr.Substring(0, 6)
            If IsNumeric(lStaCode) Then
                lStateCode = lStr.Substring(78, 2)
                If lStaCode > 0 And lStateCode.Length > 0 Then ' lStateCode.ToUpper = "GA" Then
                    If lStateCode <> lCurState Then
                        Logger.Dbg("ReadNOAAHPDAttributes:  New State is " & lStateCode)
                        lCurState = lStateCode
                    End If
                    NOAAHPDAttribs = New atcCollection
                    NOAAHPDAttribs.Add("STAID", lStaCode)
                    NOAAHPDAttribs.Add("STCODE", lStateCode)
                    NOAAHPDAttribs.Add("STANAM", lStr.Substring(47, 30))
                    'NOAAHPDAttribs.Add("DESCRP", Mid(lStr, 143, 30))
                    NOAAHPDAttribs.Add("START", lStr.Substring(7, 8))
                    NOAAHPDAttribs.Add("END", lStr.Substring(16, 8))
                    If IsNumeric(lStr.Substring(25, 4)) Then
                        lDecDeg = CDbl(lStr.Substring(25, 2)) + CDbl(lStr.Substring(27, 2)) / 60
                        If lStr.Chars(29) = "S" Then
                            lDecDeg = -lDecDeg
                        End If
                        NOAAHPDAttribs.Add("LATDEG", lDecDeg)
                    End If
                    If IsNumeric(lStr.Substring(31, 5)) Then
                        lDecDeg = CDbl(lStr.Substring(31, 3)) + CDbl(lStr.Substring(34, 2)) / 60
                        If lStr.Chars(36) = "W" Then
                            lDecDeg = -lDecDeg
                        End If
                        NOAAHPDAttribs.Add("LNGDEG", lDecDeg)
                    End If
                    NOAAHPDAttribs.Add("ELEV", lStr.Substring(38, 4))
                    lStationIndex = lStations.IndexFromKey(lStaCode)
                    If lStationIndex >= 0 Then 'If there is an older station history entry, overwrite it
                        If CDbl(NOAAHPDAttribs.ItemByKey("END")) > CDbl(lStations.ItemByIndex(lStationIndex).ItemByKey("END")) Then
                            lStations.RemoveAt(lStationIndex)
                            lStations.Add(lStaCode, NOAAHPDAttribs)
                        End If
                    Else
                        lStations.Add(lStaCode, NOAAHPDAttribs)
                    End If
                End If
            End If
        Next
        Return lStations
    End Function

End Module
