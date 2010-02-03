Imports atcUtility
Imports atcData
Imports MapWinUtility

Public Module modStat
    Public Function IntervalReport(ByVal aSite As String, ByVal aTimeUnit As atcTimeUnit, _
                                   ByVal aTser1 As atcTimeseries, _
                                   ByVal aTSer2 As atcTimeseries, _
                          Optional ByVal aListIntervalValues As Boolean = False, _
                          Optional ByVal aTserOpt As atcTimeseries = Nothing) As String
        Dim lInterval As String = ""
        Select Case aTimeUnit
            Case atcTimeUnit.TUDay : lInterval = "Daily"
            Case atcTimeUnit.TUMonth : lInterval = "Monthly"
            Case atcTimeUnit.TUYear : lInterval = "Annual"
        End Select
        Dim lUnits As String = aTser1.Attributes.GetValue("Units", "cfs")
        Dim lTran As atcTran = atcTran.TranAverSame
        If lUnits.Contains("inches") Then lTran = atcTran.TranSumDiv

        Dim lStr As String = ""
        lStr &= aSite & ":" & lInterval.PadLeft(16) & vbCrLf

        Dim lPadLength As Integer = 36
        Dim lTserOptCons As String = ""
        If aTserOpt IsNot Nothing Then lTserOptCons = aTserOpt.Attributes.GetValue("Constituent", "")
        If lTserOptCons = "PREC" Then lTserOptCons = "Precip"
        If lTserOptCons.Length > 0 Then
            lPadLength = 24
        End If
        If aListIntervalValues Then
            If lInterval = "Annual" Then
                lStr &= "Year".PadLeft(lPadLength)
            Else
                lStr &= "Date".PadLeft(lPadLength)
            End If
            If lTserOptCons.Length > 0 Then
                lStr &= lTserOptCons.PadLeft(36 - lPadLength)
            End If
        Else
            lStr &= Space(lPadLength)
        End If
        lStr &= "Simulated".PadLeft(12) & "Observed".PadLeft(12)
        If aListIntervalValues Then
            lStr &= "Residual".PadLeft(12) & "% Error".PadLeft(12)
        End If
        lStr &= vbCrLf

        Dim lTSer1 As atcTimeseries = Aggregate(aTser1, aTimeUnit, 1, lTran)
        Dim lTSer2 As atcTimeseries = Aggregate(aTSer2, aTimeUnit, 1, lTran)

        Dim lTSerOpt As atcTimeseries = Nothing
        If aTserOpt IsNot Nothing Then
            lTSerOpt = Aggregate(aTserOpt, aTimeUnit, 1, lTran)
        End If

        If aListIntervalValues Then
            Dim lDateFormat As New atcDateFormat
            With lDateFormat
                .IncludeMonths = False
                .IncludeDays = False
                .IncludeHours = False
                .IncludeMinutes = False
            End With
            For lIndex As Integer = 1 To lTSer1.numValues
                Dim lValue1 As Double = lTSer1.Value(lIndex)
                Dim lValue2 As Double = lTSer2.Value(lIndex)
                Dim lResidual As Double = lValue1 - lValue2
                lStr &= lDateFormat.JDateToString(lTSer1.Dates.Value(lIndex)).PadLeft(lPadLength)
                If lTSerOpt IsNot Nothing Then
                    lStr &= DecimalAlign(lTSerOpt.Value(lIndex), 12, 2).PadLeft(36 - lPadLength)
                End If
                lStr &= DecimalAlign(lValue1, 12, 2).PadLeft(12) _
                      & DecimalAlign(lValue2, 12, 2).PadLeft(12) _
                      & DecimalAlign(lResidual, 12, 2).PadLeft(12) _
                      & DecimalAlign(100 * lResidual / lValue2, 11, 2).PadLeft(11) & "%" & vbCrLf
            Next
            lStr &= vbCrLf
        End If

        lStr &= Attributes("Count", lTSer1, lTSer2, lTSerOpt)
        If lTran = atcTran.TranSumDiv Then
            lStr &= Attributes("Sum", lTSer1, lTSer2, lTSerOpt)
        End If
        Dim lIncludeResidual As Boolean = False
        If aTimeUnit = atcTimeUnit.TUYear Then
            lIncludeResidual = True
        End If
        lStr &= Attributes("Mean", lTSer1, lTSer2, lTSerOpt, lIncludeResidual)
        lStr &= Attributes("Geometric Mean", lTSer1, lTSer2, lTSerOpt, lIncludeResidual)

        Dim lReport As New DurationReport
        Dim lLimit As Generic.List(Of Double) = lReport.ClassLimitsNeeded(lTSer1)
        If lLimit Is Nothing Then
            Logger.Msg("lLimit is nothing")
        End If

        lStr &= vbCrLf & CompareStats(lTSer1, lTSer2, lLimit)

        lTSer1.Clear()
        lTSer1.Dates.Clear()
        lTSer1 = Nothing
        lTSer2.Clear()
        lTSer2.Dates.Clear()
        lTSer2 = Nothing
        lStr &= vbCrLf & vbCrLf
        Return lStr
    End Function

    ''' <summary>Calculate duration statistics of a timeseries</summary>
    ''' <remarks>related to CompareStats that compares two data series</remarks>
    Friend Function DurationStats(ByVal aTimeseries As atcTimeseries, _
                                  ByVal aDurationReport As DurationReport) As String
        Dim lNote As String = ""
        Dim lValue As Double
        Dim lSkipCount As Integer = 0
        Dim lGoodCount As Integer = 0

        Dim lClassBuckets As New atcCollection 'of ClassBucket
        Dim lClassBucket As ClassBucket
        Dim lClassLimit As Double

        For lIndex As Integer = 1 To aTimeseries.numValues
            lValue = aTimeseries.Values(lIndex)
            If Not Double.IsNaN(lValue) Then
                lGoodCount += 1
                lClassLimit = aDurationReport.ClassLimits(0)
                For Each lLimit As Double In aDurationReport.ClassLimits
                    If lLimit > lValue Then Exit For
                    lClassLimit = lLimit
                Next

                If lClassBuckets.Keys.Contains(lClassLimit) Then
                    lClassBucket = lClassBuckets.ItemByKey(lClassLimit)
                Else
                    lClassBucket = New ClassBucket
                    'lClassBucket.setErrInt(lErrInt)
                    lClassBuckets.Add(lClassLimit, lClassBucket)
                End If
                lClassBucket.IncrementCount(lValue)
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

        'The duration curve table, which is a shortened version of Table 2 in CompareStats
        'TODO: don't have Flow hard coded - use constituent attibute instead
        Dim lStr As String = ""
        lStr &= "                Flow duration curve" & vbCrLf
        lStr &= "                  " & TimeserIdString(aTimeseries) & vbCrLf & vbCrLf

        'TODO: include season attributes, date span of input data

        lStr &= "               Cases equal or" & vbCrLf
        lStr &= "               exceeding lower    Cases equal or" & vbCrLf
        lStr &= "               limit and less     exceeding lower" & vbCrLf
        lStr &= "    Lower     than upper limit      class limit" & vbCrLf
        lStr &= "    class    ------------------- -------------------" & vbCrLf
        lStr &= "    limit      Cases    Percent    Cases    Percent " & vbCrLf
        lStr &= "------------ --------- --------- --------- ---------" & vbCrLf

        Dim lPctFrac As Double = 0.0
        Dim lNumExceedPct As Double = 0.0
        Dim lAvgClass As Double = 0.0
        For Each lLimit As Double In aDurationReport.ClassLimitsNeeded(aTimeseries)
            lStr &= DecimalAlign(lLimit, 12, 0)
            If lClassBuckets.Keys.Contains(lLimit) Then
                lClassBucket = lClassBuckets.ItemByKey(lLimit)
                With lClassBucket
                    lStr &= CStr(.Count1).PadLeft(10)

                    lPctFrac = .Count1 * 100.0 / lGoodCount
                    lStr &= DecimalAlign(lPctFrac, 10, 2)
                    lNumExceedPct = NumberExceeding(lLimit, lClassBuckets, False)
                    lStr &= DecimalAlign(lNumExceedPct, 10, 0)
                    lNumExceedPct = NumberExceeding(lLimit, lClassBuckets, False) * 100.0 / lGoodCount
                    lStr &= DecimalAlign(lNumExceedPct, 10, 2)
                End With
            Else
                Logger.Dbg("No Bucket for " & lLimit)
            End If
            lStr &= vbCrLf
        Next

        lStr &= vbCrLf
        lStr &= "Percent time value was exceeded" & vbCrLf
        lStr &= vbCrLf
        lStr &= "    Flow     %" & vbCrLf
        lStr &= " --------- ---" & vbCrLf

        For Each lExceedPercent As Double In aDurationReport.ExceedPercents
            Try
                Dim lNonExceedPercent As Double = 100 - lExceedPercent
                Dim lNonExceedPercentString As String = (lNonExceedPercent).ToString.PadLeft(2, "0")
                Dim lNonExceedValue As Double = aTimeseries.Attributes.GetDefinedValue("%" & lNonExceedPercentString).Value
                lStr &= DecimalAlign(lNonExceedValue, 10, 2) & lExceedPercent.ToString.PadLeft(4) & vbCrLf
            Catch lEx As Exception
                Logger.Dbg("At ExceedPercent " & lExceedPercent & " Exception " & lEx.ToString)
            End Try

        Next

        lStr &= vbCrLf
        If lNote.Length > 0 Then
            lStr &= lNote
        End If
        lStr &= vbCrLf

        Return lStr
    End Function

    ''' <summary>
    ''' Compare stats for DS1 (or the observed data) and DS2 (or the simulated data)
    ''' </summary>
    Public Function CompareStats(ByVal aTSer1 As atcTimeseries, _
                                 ByVal aTSer2 As atcTimeseries, _
                        Optional ByVal aClassLimits As Generic.List(Of Double) = Nothing) As String
        Dim lStr As String = ""
        Dim lNote As String = ""
        Dim lMeanError As Double = 0.0#
        Dim lMeanAbsoluteError As Double = 0.0#
        Dim lMeanSMO2M1 As Double = 0.0#
        Dim lRmsError As Double = 0.0#
        Dim lValDiff As Double
        Dim lVal1 As Double
        Dim lVal2 As Double
        Dim lSkipCount As Integer = 0
        Dim lGoodCount As Integer = 0

        Dim lClassBuckets As New atcCollection 'of ClassBucket
        Dim lClassBucket As ClassBucket
        Dim lClassLimit As Double
        Dim lErrInt() As Double = {-60.0, -30.0, -10.0, 0.0, 10.0, 30.0, 60.0}
        'Logger.Msg("Length: " & lErrInt.Length)
        'Logger.Msg("GetLength: " & lErrInt.GetLength(1))
        'Logger.Msg("GetUpperBound: " & lErrInt.GetLowerBound(1))
        'Logger.Msg("GetLowerBound: " & lErrInt.GetLowerBound(1))
        'Logger.Msg("Rank:" & lErrInt.Rank)

        'Dim lErrInt(7) As Double
        'lErrInt(0) = -60.0
        'lErrInt(1) = -30.0
        'lErrInt(2) = -10.0
        'lErrInt(3) = 0.0
        'lErrInt(4) = 10.0
        'lErrInt(5) = 30.0
        'lErrInt(6) = 60.0


        If aClassLimits Is Nothing Then
            Logger.Msg("aClassLimits is nothing")
        End If
        For lIndex As Integer = 1 To aTSer1.numValues
            lVal1 = aTSer1.Values(lIndex)
            lVal2 = aTSer2.Values(lIndex)
            'Logger.Dbg("lVal1:" & CStr(lVal1) & ":lVal2:" & CStr(lVal2))
            If Not Double.IsNaN(lVal1) And Not Double.IsNaN(lVal2) Then

                'DBG:
                'If (lVal1 >= 4.0 And lVal1 <= 5.7) Then
                '    Logger.Msg("DBG: found a val 1: " & CStr(lVal1))
                'End If
                'If (lVal2 >= 4.0 And lVal2 <= 5.7) Then
                '    Logger.Msg("DBG: found a val 2: " & CStr(lVal2))
                'End If
                lValDiff = lVal1 - lVal2
                lMeanError += lValDiff
                lMeanSMO2M1 += lVal2 - lVal1
                lMeanAbsoluteError += Math.Abs(lValDiff)
                lRmsError += lValDiff * lValDiff
                lGoodCount += 1
                If aClassLimits IsNot Nothing Then
                    lClassLimit = aClassLimits(0)
                    For Each lLimit As Double In aClassLimits
                        If lLimit > lVal1 Then Exit For
                        lClassLimit = lLimit
                    Next

                    If lClassBuckets.Keys.Contains(lClassLimit) Then
                        lClassBucket = lClassBuckets.ItemByKey(lClassLimit)
                    Else
                        lClassBucket = New ClassBucket
                        lClassBucket.setErrInt(lErrInt)
                        lClassBuckets.Add(lClassLimit, lClassBucket)
                    End If
                    lClassBucket.Increment(lVal1, lVal2)
                    lClassBucket.IncrementErr(lVal1, lVal2, lClassLimit)

                    lClassLimit = aClassLimits(0)
                    For Each lLimit As Double In aClassLimits
                        If lLimit > lVal2 Then Exit For
                        lClassLimit = lLimit
                    Next
                    If lClassBuckets.Keys.Contains(lClassLimit) Then
                        lClassBucket = lClassBuckets.ItemByKey(lClassLimit)
                    Else ' this else branch will not happen because all the bins are created already for aVal1
                        lClassBucket = New ClassBucket
                        lClassBucket.setErrInt(lErrInt)
                        lClassBuckets.Add(lClassLimit, lClassBucket)
                    End If
                    lClassBucket.Count2 += 1
                    lClassBucket.Total2 += lVal2

                End If
            Else
                lSkipCount += 1
                If lSkipCount = 1 Then
                    lNote = "*** Note - compare skipped index " & lIndex
                End If
            End If
        Next
        '
        'When all values are screened once, devide the count into the sum of differences
        '
        Dim lTPDIF As Double
        Dim lTPDIF2 As Double
        Dim lTPBias As Double
        Dim lTSUMA As Double
        Dim lTSUMB As Double
        For Each lClassBucket In lClassBuckets
            With lClassBucket
                lTPDIF += .TotalDifferencePCT
                lTPDIF2 += .TotalSSPCT ' has to be put here to have the correct sum
                lTPBias += .TotalBiasPCT
                lTSUMA += .Total1
                lTSUMB += .Total2
                .TotalDifference /= .Count1
                .TotalSS /= .Count1
                .TotalSS = Math.Pow(.TotalSS, 0.5)
                .TotalBias /= .Count1
                .TotalDifferencePCT /= .Count1
                .TotalDifferencePCT *= 100
                .TotalSSPCT /= .Count1
                .TotalSSPCT = Math.Sqrt(.TotalSSPCT)
                .TotalSSPCT *= 100
                .TotalBiasPCT /= .Count1
                .TotalBiasPCT *= 100

            End With
        Next

        If lNote.Length > 0 Then
            lNote &= " and " & lSkipCount - 1 & " more" & vbCrLf
        End If

        Dim lNashSutcliffeNumerator As Double = lRmsError

        lMeanError /= lGoodCount
        lMeanAbsoluteError /= lGoodCount
        lMeanSMO2M1 /= lGoodCount
        lRmsError /= lGoodCount

        If lRmsError > 0 Then
            lRmsError = Math.Sqrt(lRmsError)
        End If

        Dim lCorrelationCoefficient As Double = 0.0#
        Dim lNashSutcliffe As Double = 0.0#
        Dim lMean1 As Double = aTSer1.Attributes.GetValue("Mean")
        Dim lMean2 As Double = aTSer2.Attributes.GetValue("Mean")
        For lIndex As Integer = 1 To aTSer1.numValues
            lVal1 = aTSer1.Values(lIndex)
            lVal2 = aTSer2.Values(lIndex)
            If Not Double.IsNaN(lVal1) And Not Double.IsNaN(lVal2) Then
                lCorrelationCoefficient += (lVal1 - lMean1) * (lVal2 - lMean2)
                lNashSutcliffe += (lVal2 - lMean2) ^ 2
            End If
        Next
        lCorrelationCoefficient /= (aTSer1.numValues - 1)
        Dim lSD1 As Double = aTSer1.Attributes.GetValue("Standard Deviation")
        Dim lSD2 As Double = aTSer2.Attributes.GetValue("Standard Deviation")
        If Math.Abs(lSD1 * lSD2) > 0.0001 Then
            lCorrelationCoefficient /= (lSD1 * lSD2)
        End If
        If lNashSutcliffe > 0 Then
            lNashSutcliffe = lNashSutcliffeNumerator / lNashSutcliffe
        End If

        lStr &= "Note: TS, Time Series" & vbCrLf & vbCrLf
        lStr &= "Correlation Coefficient".PadLeft(36) & DecimalAlign(lCorrelationCoefficient, 18) & vbCrLf
        lStr &= "Coefficient of Determination".PadLeft(36) & DecimalAlign(lCorrelationCoefficient ^ 2, 18) & vbCrLf
        lStr &= "Mean Error".PadLeft(36) & DecimalAlign(lMeanError, 18) & vbCrLf
        lStr &= "Mean Absolute Error".PadLeft(36) & DecimalAlign(lMeanAbsoluteError, 18) & vbCrLf
        lStr &= "RMS Error".PadLeft(36) & DecimalAlign(lRmsError, 18) & vbCrLf
        lStr &= "Model Fit Efficiency".PadLeft(36) & DecimalAlign(1 - lNashSutcliffe, 18) & vbCrLf
        lStr &= vbCrLf & vbCrLf & vbFormFeed & vbCrLf


        If aClassLimits IsNot Nothing Then
            'lStr &= "Time Series 1" & vbCrLf & "Time Series 2" & vbCrLf

            lStr &= "            Data Series 1 - " & TimeserIdString(aTSer1) & vbCrLf
            lStr &= "            Data Series 2 - " & TimeserIdString(aTSer2) & vbCrLf & vbCrLf
            lStr &= "                           Mean               Root mean" & vbCrLf
            lStr &= "Lower    Number    absolute error(1)     square error(2)        Bias(3)      " & vbCrLf
            lStr &= "class      of     ------------------- ------------------- -------------------" & vbCrLf
            lStr &= "limit     cases   Average    Percent   Average    Percent  Average   Percent " & vbCrLf
            lStr &= "--------- --------- --------- --------- --------- --------- --------- ---------" & vbCrLf
            For Each lLimit As Double In aClassLimits
                lStr &= DecimalAlign(lLimit, 12, 0)
                Dim lCount As Integer = 0
                If lClassBuckets.Keys.Contains(lLimit) Then
                    lClassBucket = lClassBuckets.ItemByKey(lLimit)
                    With lClassBucket
                        lStr &= CStr(.Count1).PadLeft(8)
                        If Double.IsNaN(.TotalDifference) Then .TotalDifference = 0.0
                        If Double.IsNaN(.TotalDifferencePCT) Then .TotalDifferencePCT = 0.0
                        If Double.IsNaN(.TotalSS) Then .TotalSS = 0.0
                        If Double.IsNaN(.TotalSSPCT) Then .TotalSSPCT = 0.0
                        If Double.IsNaN(.TotalBias) Then .TotalBias = 0.0
                        If Double.IsNaN(.TotalBiasPCT) Then .TotalBiasPCT = 0.0

                        lStr &= DecimalAlign(.TotalDifference, 12, 3)
                        lStr &= DecimalAlign(.TotalDifferencePCT, 12, 1)
                        lStr &= DecimalAlign(.TotalSS, 12, 3)
                        lStr &= DecimalAlign(.TotalSSPCT, 12, 1)
                        lStr &= DecimalAlign(.TotalBias, 12, 3)
                        lStr &= DecimalAlign(.TotalBiasPCT, 12, 1)
                    End With
                Else
                    Logger.Dbg("No Bucket for " & lLimit)
                End If
                lStr &= vbCrLf
            Next
        End If

        lStr &= "--------- --------- --------- --------- --------- --------- --------- ---------" & vbCrLf
        lStr &= CStr(lGoodCount).PadLeft(18)
        lStr &= DecimalAlign(lMeanAbsoluteError, 15)
        lTPDIF *= 100.0
        lTPDIF /= lGoodCount
        lStr &= DecimalAlign(lTPDIF, 10, 2) 'Average Percent Difference: TotalDiffPercent/Total#ofObs
        lStr &= DecimalAlign(lRmsError, 12)
        'Logger.Msg("lRmsError = " & CStr(lRmsError))
        'Logger.Msg("TPDIF2 = " & CStr(TPDIF2))
        lTPDIF2 /= lGoodCount
        lTPDIF2 = Math.Sqrt(lTPDIF2)
        lTPDIF2 *= 100.0

        'Logger.Msg("TPDIF2 * 100 / lGoodcount = " & CStr(TPDIF2))
        lStr &= DecimalAlign(lTPDIF2, 10, 2) 'Average Percent for Square of Difference: TotalSquareDifference/Total#ofObs
        'lStr &= DecimalAlign(Math.Abs(lMeanAbsoluteError), 15)
        lStr &= DecimalAlign(Math.Abs(lMeanSMO2M1), 15)
        lTPBias *= 100.0
        lTPBias /= lGoodCount
        lStr &= DecimalAlign(lTPBias, 10, 2) 'Average Percent Bias: TotalPercentBias/Total#ofObs
        lStr &= vbCrLf & vbCrLf
        Dim STEST As Double
        STEST = Math.Sqrt(lGoodCount / (lGoodCount - 1) * (lRmsError ^ 2 - Math.Abs(lMeanSMO2M1) ^ 2))
        lStr &= "Standard error of estimate = " & DecimalAlign(STEST, 8, 2) & vbCrLf 'Standard error of estimate
        lStr &= "         = square root((n/n-1)*((tot.col.5)**2-(tot.col.7)**2))" & vbCrLf


        lStr &= "(1) Average = sum(|TS2-TS1|/TS1)" & vbCrLf
        lStr &= "    Percent = 100 * (sum(|TS2-TS1|/TS1))/n for all TS1 > 0" & vbCrLf
        lStr &= "(2) Average = square root(sum((TS2-TS1)**2)/n)" & vbCrLf
        lStr &= "    Percent = 100 * square root(sum(((TS2-TS1)/TS1)**2)/n) for all TS1 > 0" & vbCrLf
        lStr &= "(3) Average = sum(TS2-TS1)/n" & vbCrLf
        lStr &= "    Percent = 100 * sum(((TS2-TS1)/TS1)/n) for all TS1 > 0" & vbCrLf
        lStr &= vbCrLf & vbCrLf & vbFormFeed & vbCrLf


        'Table nubmer 2

        If aClassLimits IsNot Nothing Then
            lStr &= "Table 2" & vbCrLf
            'lStr &= "Time Series 1" & vbCrLf & "Time Series 2" & vbCrLf
            'lStr &="             Simulated - 11519500 Scott River near Fort Jones, CA.             " & vbCrLf 
            'lStr &="               Observed  - 11517500 Shasta River near Yreka, CA.               " & vbCrLf 
            lStr &= "            Data Series 1 - " & TimeserIdString(aTSer1) & vbCrLf
            lStr &= "            Data Series 2 - " & TimeserIdString(aTSer2) & vbCrLf & vbCrLf

            lStr &= "" & vbCrLf
            lStr &= "         Cases equal or exceeding lower" & vbCrLf
            lStr &= "          limit & less then upper limit     Percent cases" & vbCrLf
            lStr &= "           ----------------------------       equal or        Average of cases" & vbCrLf
            lStr &= "   Lower     Cases         Percent         exceeding limit   within class limits" & vbCrLf
            lStr &= "   class   --------- ------------------- ------------------- -------------------" & vbCrLf
            lStr &= "   limit   Sim   Obs Simulated  Observed Simulated  Observed Simulated  Observed" & vbCrLf
            lStr &= " --------- ---- ---- --------- --------- --------- --------- --------- ---------" & vbCrLf
            Dim pctfrac As Double = 0.0
            Dim numExceedPct As Double = 0.0
            Dim avgClass As Double = 0.0
            For Each lLimit As Double In aClassLimits
                lStr &= DecimalAlign(lLimit, 12, 0)
                If lClassBuckets.Keys.Contains(lLimit) Then
                    lClassBucket = lClassBuckets.ItemByKey(lLimit)
                    With lClassBucket
                        lStr &= CStr(.Count1).PadLeft(5)
                        lStr &= CStr(.Count2).PadLeft(5)
                        pctfrac = .Count1 * 100.0 / lGoodCount
                        lStr &= DecimalAlign(CStr(pctfrac), 15, 2)
                        pctfrac = .Count2 * 100.0 / lGoodCount
                        lStr &= DecimalAlign(CStr(pctfrac), 15, 2)
                        numExceedPct = NumberExceeding(lLimit, lClassBuckets, False) * 100.0 / lGoodCount
                        lStr &= DecimalAlign(numExceedPct, 15, 2)
                        numExceedPct = NumberExceeding(lLimit, lClassBuckets, True) * 100.0 / lGoodCount
                        lStr &= DecimalAlign(numExceedPct, 15, 2)

                        avgClass = .Total1 / .Count1
                        If Double.IsNaN(avgClass) Then
                            lStr &= DecimalAlign(0.0, 15, 2)
                        Else
                            lStr &= DecimalAlign(avgClass, 15, 2)
                        End If

                        avgClass = .Total2 / .Count2
                        If Double.IsNaN(avgClass) Then
                            lStr &= DecimalAlign(0.0, 15, 2)
                        Else
                            lStr &= DecimalAlign(avgClass, 15, 2)
                        End If

                    End With
                Else
                    Logger.Dbg("No Bucket for " & lLimit)
                End If
                lStr &= vbCrLf
            Next
        End If

        lStr &= " --------- ---- ---- --------- --------- ---------- --------- --------- ---------" & vbCrLf
        lStr &= CStr(lGoodCount).PadLeft(30) & CStr(lGoodCount).PadLeft(8)
        lStr &= "100.00".PadLeft(10) 'total percentage of TS1
        lStr &= "100.00".PadLeft(10) 'total percentage of TS2
        lTSUMA /= lGoodCount
        lTSUMB /= lGoodCount
        lStr &= DecimalAlign(lTSUMA, 12, 2) ' Avg of TS1 raw value
        lStr &= DecimalAlign(lTSUMB, 12, 2) ' Avg of TS2 raw value
        lStr &= vbCrLf & vbCrLf & vbFormFeed & vbCrLf


        'Table Number 3

        Dim lEdIndex As Integer
        If aClassLimits IsNot Nothing Then
            'lStr &= "Time Series 1" & vbCrLf & "Time Series 2" & vbCrLf
            'lStr &= "   Time Series 1 " & vbCrLf & "   Time Series 2 " & vbCrLf
            lStr &= "            Data Series 1 - " & TimeserIdString(aTSer1) & vbCrLf
            lStr &= "            Data Series 2 - " & TimeserIdString(aTSer2) & vbCrLf & vbCrLf

            lStr &= "   Lower         Number of occurrences between indicated deviations    " & vbCrLf
            lStr &= "   class    -------------------------------------------------------------" & vbCrLf
            lStr &= "   limit          -60%    -30%    -10%      0%     10%     30%     60%" & vbCrLf
            lStr &= " ---------  -------------------------------------------------------------" & vbCrLf
            For Each lLimit As Double In aClassLimits
                lStr &= DecimalAlign(lLimit, 12, 2)
                'Dim lCount As Integer = 0
                If lClassBuckets.Keys.Contains(lLimit) Then
                    lClassBucket = lClassBuckets.ItemByKey(lLimit)
                    With lClassBucket
                        For lEdIndex = 0 To .ErrDevCount.Length - 1
                            If lEdIndex > 0 Then
                                lStr &= CStr(.ErrDevCount(lEdIndex)).PadLeft(8)
                            ElseIf lEdIndex > 3 Then
                                lStr &= CStr(.ErrDevCount(lEdIndex)).PadLeft(10)
                            ElseIf lEdIndex > 5 Then
                                lStr &= CStr(.ErrDevCount(lEdIndex)).PadLeft(14)
                            Else
                                lStr &= CStr(.ErrDevCount(lEdIndex)).PadLeft(6)
                            End If

                        Next
                    End With
                Else
                    Logger.Dbg("No Bucket for " & lLimit)
                End If
                lStr &= vbCrLf
            Next
        End If

        lStr &= " ---------  -------------------------------------------------------------" & vbCrLf
        'Loop through ErrIntv then loop through classlimit to sum up total entry in each errintv
        Dim i As Integer
        Dim lErrTot As Integer = 0
        lStr &= "          "
        For i = 0 To lErrInt.Length
            lErrTot = 0
            For Each lLimit As Double In aClassLimits
                If lClassBuckets.Keys.Contains(lLimit) Then
                    lClassBucket = lClassBuckets.ItemByKey(lLimit)
                    lErrTot += lClassBucket.ErrDevCount(i)
                End If
            Next
            lStr &= CStr(lErrTot).PadLeft(7)
        Next

        lStr &= vbCrLf & vbCrLf & vbFormFeed & vbCrLf

        If lNote.Length > 0 Then
            lStr &= lNote
        End If
        Return lStr
    End Function

    Private Function TimeserIdString(ByVal aTSer As atcTimeseries) As String
        Dim lStr As String = aTSer.Attributes.GetValue("ISTAID", "")
        If lStr.Trim = "0" Then lStr = ""
        lStr &= " " & aTSer.Attributes.GetValue("STANAM", "")
        If lStr.Trim.Length = 0 Then
            lStr = aTSer.ToString
        End If
        Return lStr.Trim
    End Function

    Private Function NumberExceeding(ByVal aClassLimit As Double, ByVal aClassList As atcCollection, ByVal aTS2 As Boolean) As Double
        Dim lBucket As ClassBucket
        For Each lClassLimit As Double In aClassList.Keys
            If aClassLimit <= lClassLimit Then
                lBucket = aClassList.ItemByKey(lClassLimit)
                If aTS2 Then
                    NumberExceeding += lBucket.Count2
                Else
                    NumberExceeding += lBucket.Count1
                End If
            End If
        Next
    End Function

    Friend Class DurationReport
        Private pClassLimits As Generic.List(Of Double)
        Private pExceedPercents As Generic.List(Of Double)

        Public Sub New()
            'Dim lClassLimits() As Double = {1, 2, 5, 10, 20, 25, 50, 100, 200, 250, 500, 1000, 2000, 5000, 10000, 20000, 25000, 50000, 100000}
            Dim lClassLimits() As Double = {0, 1, 1.4, 2, 2.8, 4, 5.7, 8.1, 11, 16, 23, 33, 46, 66, 93, 130, 190, 270, 380, 530, 760, 1100, 1500, 2200, 3100, 4300, 6100, 8700, 12000, 17000, 25000, 35000, 50000, 71000, 100000}
            pClassLimits = lClassLimits.ToList
            Dim lExceedPercents() As Double = {99, 98, 95, 90, 85, 80, 75, 70, 65, 55, 50, 45, 40, 35, 30, 25, 20, 15, 10, 5, 2, 1}
            pExceedPercents = lExceedPercents.ToList
        End Sub

        Friend Property ClassLimits() As Generic.List(Of Double)
            Get
                Return pClassLimits
            End Get
            Set(ByVal aClassLimits As Generic.List(Of Double))
                pClassLimits = aClassLimits
            End Set
        End Property

        Friend Property ExceedPercents() As Generic.List(Of Double)
            Get
                Return pExceedPercents
            End Get
            Set(ByVal aExceedPercents As Generic.List(Of Double))
                pExceedPercents = aExceedPercents
            End Set
        End Property

        Friend Function ClassLimitsNeeded(ByVal aTimser As atcTimeseries) As Generic.List(Of Double)
            Dim lLimits As New Generic.List(Of Double)
            Dim lMin As Double = aTimser.Attributes.GetValue("Min")
            Dim lMax As Double = aTimser.Attributes.GetValue("Max")
            For Each lValue As Double In pClassLimits
                If lValue < lMax Then lLimits.Add(lValue)
            Next
            Return lLimits
        End Function
    End Class

    Private Class ClassBucket
        'Public Count As Integer ' in this Class
        Public Count1 As Integer ' for ts 1
        Public Count2 As Integer ' for ts 2
        Public Total1 As Double ' for ts 1
        Public Total2 As Double ' for ts 2
        Public TotalDifference As Double ' Sum of abs(diff)
        Public TotalSS As Double ' Sum of Square
        Public TotalBias As Double 'Sum of bias
        Public TotalDifferencePCT As Double ' Sum of abs(diff) percent
        Public TotalSSPCT As Double ' Sum of Square percent
        Public TotalBiasPCT As Double  'Sum of bias percent
        Public ErrIntv() As Double ' Error interval (TSCBAT, ERRINT)
        Public ErrDevCount() As Integer ' Error deviation for me classLimit
        'Public ClassLimit As Double

        Sub Increment(ByVal aVal1 As Double, ByVal aVal2 As Double)
            Count1 += 1
            Total1 += aVal1
            Dim lValDiff As Double = aVal2 - aVal1
            TotalDifference += Math.Abs(lValDiff)
            TotalSS += lValDiff ^ 2
            TotalBias += lValDiff
            If aVal1 > 0 Then
                TotalDifferencePCT += Math.Abs(lValDiff) / aVal1
                TotalSSPCT += (lValDiff / aVal1) ^ 2
                TotalBiasPCT += (lValDiff / aVal1)
            End If
        End Sub

        Friend Sub IncrementCount(ByVal aVal As Double)
            Count1 += 1
            Total1 += aVal
        End Sub

        Friend Sub IncrementErr(ByVal aVal1 As Double, ByVal aVal2 As Double, ByVal aLimit As Double)
            '
            'Increment the error deviation count
            '
            Dim d As Integer
            Dim x As Double
            Dim lDiff As Double = aVal2 - aVal1
            x = aVal1
            If x <= 0 Then x = aLimit
            If x <= 0 Then
                x = 0.0
            Else
                x = 100 * lDiff / x
            End If

            Dim lTopOff As Boolean = True

            For d = 0 To ErrIntv.Length - 1
                If x < ErrIntv(d) Then
                    ErrDevCount(d) += 1
                    lTopOff = False
                    'lStr = "Error :" & CStr(x) & " < " & CStr(ErrIntv(d)) & " ; Err Intv :" & CStr(d) & vbCrLf
                    'Logger.Dbg(lStr)
                    Exit For
                End If
            Next
            If lTopOff Then 'this is to handle the biggest (or the above last) error class
                'Logger.Dbg("ErrIntv() current index: " & CStr(d))
                'lStr = "Error :" & CStr(x) & " > " & CStr(ErrIntv(d - 1)) & " ; Err Intv :" & CStr(d - 1) & vbCrLf
                'Logger.Dbg(lStr)
                ErrDevCount(ErrDevCount.GetUpperBound(0)) += 1
            End If

        End Sub
        Friend Sub setErrInt(ByVal aEI() As Double)
            Dim lC As Integer = 0
            'Logger.Msg("Argument length: " & aEI.Length)
            ErrIntv = aEI.Clone
            ReDim ErrDevCount(ErrIntv.GetUpperBound(0) + 1)
        End Sub
    End Class

    Private Function Attributes(ByVal aName As String, _
                               ByVal aTSer1 As atcTimeseries, _
                               ByVal aTSer2 As atcTimeseries, _
                      Optional ByVal aTSerOpt As atcTimeseries = Nothing, _
                      Optional ByVal aIncludeResidual As Boolean = False) As String
        Dim lStr As String = ""
        If aTSerOpt Is Nothing Then
            lStr = aName.PadLeft(36)
        Else
            lStr = aName.PadLeft(24) & DecimalAlign(aTSerOpt.Attributes.GetFormattedValue(aName), 12, 2)
        End If
        Dim lValue1 As Double = aTSer1.Attributes.GetValue(aName)
        Dim lValue2 As Double = aTSer2.Attributes.GetValue(aName)
        lStr &= DecimalAlign(lValue1, 12, 2) & _
                DecimalAlign(lValue2, 12, 2)
        If aIncludeResidual Then
            Dim lResidual As Double = lValue1 - lValue2
            lStr &= DecimalAlign(lResidual, 12, 2).PadLeft(12) _
                  & DecimalAlign(100 * lResidual / lValue2, 11, 2).PadLeft(11) & "%"
        End If
        Return lStr & vbCrLf
    End Function
End Module
