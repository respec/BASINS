Imports atcUtility
Imports atcData
Imports MapWinUtility
Imports System.Text

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
                'Dim lNonExceedValue As Double = aTimeseries.Attributes.GetDefinedValue("%" & lNonExceedPercentString).Value
                Dim lNonExceedValue As Double = aTimeseries.Attributes.GetValue("%" & lNonExceedPercentString)
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
            Dim lErr As String = "Class limits are not defined, unable to conduct compare analysis."
            Logger.Msg(lErr)
            Return lErr
        ElseIf aClassLimits.Count = 0 Then
            Dim lErr As String = "There are no values within the specified class limits." & vbNewLine & _
                                 "Select more class limits and try again."
            Logger.Msg(lErr)
            Return lErr
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

            lStr &= "            TS 1 - " & TimeserIdString(aTSer1) & vbCrLf
            lStr &= "            TS 2 - " & TimeserIdString(aTSer2) & vbCrLf & vbCrLf
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


        lStr &= "(1) Average = sum(|TS2-TS1|/n)" & vbCrLf
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
            lStr &= "            TS 1 - " & TimeserIdString(aTSer1) & vbCrLf
            lStr &= "            TS 2 - " & TimeserIdString(aTSer2) & vbCrLf & vbCrLf

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
            lStr &= "            TS 1 - " & TimeserIdString(aTSer1) & vbCrLf
            lStr &= "            TS 2 - " & TimeserIdString(aTSer2) & vbCrLf & vbCrLf

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

        lStr &= vbCrLf
        lStr &= "Percent time value was exceeded" & vbCrLf
        lStr &= vbCrLf
        lStr &= "    Flow     %" & vbCrLf
        lStr &= "     TS1        TS2   %Exceedance" & vbNewLine
        lStr &= "---------- ---------- ------------" & vbNewLine

        Dim lDurationReport As New DurationReport
        For Each lExceedPercent As Double In lDurationReport.ExceedPercents
            Try
                Dim lNonExceedPercent As Double = 100 - lExceedPercent
                Dim lNonExceedPercentString As String = (lNonExceedPercent).ToString.PadLeft(2, "0")
                Dim lNonExceedValue1 As Double = aTSer1.Attributes.GetValue("%" & lNonExceedPercentString)
                Dim lNonExceedValue2 As Double = aTSer2.Attributes.GetValue("%" & lNonExceedPercentString)
                lStr &= DecimalAlign(lNonExceedValue1, 10, 2) & DecimalAlign(lNonExceedValue2, 10, 2) & lExceedPercent.ToString.PadLeft(4) & vbCrLf
            Catch lEx As Exception
                Logger.Dbg("At ExceedPercent " & lExceedPercent & " Exception " & lEx.ToString)
            End Try
        Next

        lStr &= vbCrLf
        'If lNote.Length > 0 Then
        '    lStr &= lNote
        'End If
        lStr &= vbCrLf

        lStr &= vbCrLf & vbCrLf & vbFormFeed & vbCrLf

        If lNote.Length > 0 Then
            lStr &= lNote
        End If
        Return lStr
    End Function

    'Public Function DurationHydrograph(ByVal aTS As atcTimeseries) As String
    '    Dim lStr As StringBuilder = New StringBuilder()
    '    Dim lExceedancePcts As Double() = {10, 20, 30, 50, 70, 80, 90}
    '    Dim lExceedanceMeans(lExceedancePcts.Count - 1) As Double
    '    Dim lSumMax As Double
    '    Dim lSumMin As Double
    '    Dim lLastDayOfMonth As New Dictionary(Of Integer, Integer)
    '    lLastDayOfMonth.Add(1, 31)
    '    lLastDayOfMonth.Add(2, 28)
    '    lLastDayOfMonth.Add(3, 31)
    '    lLastDayOfMonth.Add(4, 30)
    '    lLastDayOfMonth.Add(5, 31)
    '    lLastDayOfMonth.Add(6, 30)
    '    lLastDayOfMonth.Add(7, 31)
    '    lLastDayOfMonth.Add(8, 31)
    '    lLastDayOfMonth.Add(9, 30)
    '    lLastDayOfMonth.Add(10, 31)
    '    lLastDayOfMonth.Add(11, 30)
    '    lLastDayOfMonth.Add(12, 31)

    '    'Make sure it is daily timeseries
    '    Select Case aTS.Attributes.GetValue("Time Unit")
    '        Case atcTimeUnit.TUDay
    '        Case Else
    '            aTS = Aggregate(aTS, atcTimeUnit.TUDay, 1, atcTran.TranAverSame) 'trans aver same for flow
    '    End Select

    '    Dim lSeasonDay As New atcSeasonsDayOfYear
    '    Dim lSplit As atcTimeseriesGroup = lSeasonDay.Split(aTS, Nothing)

    '    Dim lDateFormat As New atcDateFormat
    '    Dim lStartDate As String = lDateFormat.JDateToString(aTS.Attributes.GetValue("Start Date"))
    '    Dim lEndDate As String = lDateFormat.JDateToString(aTS.Attributes.GetValue("End Date"))

    '    Dim lHeaderLines As String = "1" & vbCrLf
    '    lHeaderLines &= Space(10) & "Duration hydrograph for " & aTS.Attributes.GetValue("STANAM", "") & vbCrLf
    '    lHeaderLines &= Space(30) & "Station id  " & aTS.Attributes.GetValue("ISTAID", "") & vbCrLf
    '    lHeaderLines &= Space(22) & "For period " & lStartDate & " to " & lEndDate & vbCrLf
    '    lHeaderLines &= Space(6) & "Num" & Space(30) & "Percentile" & vbCrLf
    '    lHeaderLines &= " ZZZ  yrs     Max    0.10    0.20    0.30    0.50    0.70    0.80    0.90     Min" 'TODO: Check if always starts with Oct!

    '    Dim lYrs As Integer = 0
    '    Dim lTempVal As Double = 0
    '    Dim lprevTimeStep As Integer = -99
    '    For Each lTS As atcTimeseries In lSplit
    '        lYrs = lTS.Attributes.GetValue("count")
    '        Dim ldates(5) As Integer
    '        J2Date(lTS.Dates.Values(0), ldates)
    '        If ldates(1) <> lprevTimeStep Then
    '            Dim lHeader As String = lHeaderLines.Replace("ZZZ", MonthName3(ldates(1)))
    '            lStr.AppendLine(lHeader) : lStr.AppendLine()
    '            lprevTimeStep = ldates(1)
    '        End If

    '        lStr.Append(ldates(2).ToString.PadLeft(4) & lYrs.ToString.PadLeft(5))
    '        lTempVal = lTS.Attributes.GetValue("Max")
    '        lStr.Append(lTempVal.ToString.PadLeft(9))
    '        lSumMax += lTempVal
    '        For Each lExceedPercent As Double In lExceedancePcts
    '            Dim I As Integer = Array.IndexOf(lExceedancePcts, lExceedPercent)
    '            Try
    '                Dim lNonExceedPercent As Double = 100 - lExceedPercent
    '                Dim lNonExceedPercentString As String = (lNonExceedPercent).ToString.PadLeft(2, "0")
    '                Dim lNonExceedValue1 As Double = lTS.Attributes.GetValue("%" & lNonExceedPercentString)
    '                lStr.Append(DecimalAlign(lNonExceedValue1.ToString, 8, 0).Replace(",", ""))
    '                lExceedanceMeans(I) += lNonExceedValue1
    '            Catch lEx As Exception
    '                Logger.Dbg("At ExceedPercent " & lExceedPercent & " Exception " & lEx.ToString)
    '            End Try
    '        Next
    '        lTempVal = lTS.Attributes.GetValue("Min")
    '        lStr.AppendLine(DecimalAlign(lTempVal.ToString, 8, 0).Replace(",", ""))
    '        lSumMin += lTempVal
    '        If ldates(2) = lLastDayOfMonth(ldates(1)) Then
    '            lStr.AppendLine()
    '            lStr.Append("    Mean ")
    '            lStr.Append(DecimalAlign((lSumMax / ldates(2)).ToString, 9, 0).Replace(",", ""))
    '            For I As Integer = 0 To lExceedanceMeans.Count - 1
    '                lStr.Append(DecimalAlign((lExceedanceMeans(I) / ldates(2)).ToString, 8, 0).Replace(",", ""))
    '                lExceedanceMeans(I) = 0.0
    '            Next
    '            lStr.Append(DecimalAlign((lSumMin / ldates(2)).ToString, 8, 0).Replace(",", ""))
    '            lStr.AppendLine()
    '            lSumMax = 0.0 : lSumMin = 0.0
    '        End If
    '    Next
    '    Return lStr.ToString
    'End Function

    Public Function DurationHydrograph(ByVal aTS As atcTimeseries) As String
        Dim lStr As StringBuilder = New StringBuilder()
        Dim lExceedancePcts As Double() = {0, 10, 20, 30, 50, 70, 80, 90, 100}
        Dim lExceedanceMeans(lExceedancePcts.Count - 1) As Double
        Dim lLastDayOfMonth As New Dictionary(Of Integer, Integer)
        lLastDayOfMonth.Add(1, 31)
        lLastDayOfMonth.Add(2, 28)
        lLastDayOfMonth.Add(3, 31)
        lLastDayOfMonth.Add(4, 30)
        lLastDayOfMonth.Add(5, 31)
        lLastDayOfMonth.Add(6, 30)
        lLastDayOfMonth.Add(7, 31)
        lLastDayOfMonth.Add(8, 31)
        lLastDayOfMonth.Add(9, 30)
        lLastDayOfMonth.Add(10, 31)
        lLastDayOfMonth.Add(11, 30)
        lLastDayOfMonth.Add(12, 31)

        'Make sure it is daily timeseries
        Select Case aTS.Attributes.GetValue("Time Unit")
            Case atcTimeUnit.TUDay
            Case Else
                aTS = Aggregate(aTS, atcTimeUnit.TUDay, 1, atcTran.TranAverSame) 'trans aver same for flow
        End Select

        Dim lSeasonDay As New atcSeasonsDayOfYear
        Dim lSplit As atcTimeseriesGroup = lSeasonDay.Split(aTS, Nothing)

        Dim lDateFormat As New atcDateFormat
        Dim lStartDate As String = lDateFormat.JDateToString(aTS.Attributes.GetValue("Start Date"))
        Dim lEndDate As String = lDateFormat.JDateToString(aTS.Attributes.GetValue("End Date"))

        Dim lHeaderLines As String = "1" & vbCrLf
        lHeaderLines &= Space(10) & "Duration hydrograph for " & aTS.Attributes.GetValue("STANAM", "") & vbCrLf
        lHeaderLines &= Space(30) & "Station id  " & aTS.Attributes.GetValue("ISTAID", "") & vbCrLf
        lHeaderLines &= Space(22) & "For period " & lStartDate & " to " & lEndDate & vbCrLf
        lHeaderLines &= Space(6) & "Num" & Space(30) & "Percentile" & vbCrLf
        lHeaderLines &= " ZZZ  yrs     Max    0.10    0.20    0.30    0.50    0.70    0.80    0.90     Min" 'TODO: Check if always starts with Oct!

        Dim lYrs As Integer = 0
        Dim lTempVal As Double = 0
        Dim lprevTimeStep As Integer = -99
        For Each lTS As atcTimeseries In lSplit
            lYrs = lTS.Attributes.GetValue("count")
            Dim ldates(5) As Integer
            J2Date(lTS.Dates.Values(0), ldates)
            If ldates(1) <> lprevTimeStep Then
                Dim lHeader As String = lHeaderLines.Replace("ZZZ", MonthName3(ldates(1)))
                lStr.AppendLine(lHeader) : lStr.AppendLine()
                lprevTimeStep = ldates(1)
            End If

            lStr.Append(ldates(2).ToString.PadLeft(4) & lYrs.ToString.PadLeft(5))
            For Each lExceedPercent As Double In lExceedancePcts
                Dim I As Integer = Array.IndexOf(lExceedancePcts, lExceedPercent)
                Try
                    Dim lNonExceedPercent As Double = 100 - lExceedPercent
                    Dim lNonExceedPercentString As String = (lNonExceedPercent).ToString.PadLeft(2, "0")
                    Dim lNonExceedValue1 As Double = lTS.Attributes.GetValue("%" & lNonExceedPercentString)
                    Dim lWidth As Integer = 8
                    If I = 0 Then lWidth = 9
                    lStr.Append(DecimalAlign(lNonExceedValue1.ToString, 8, 0).Replace(",", ""))
                    lExceedanceMeans(I) += lNonExceedValue1
                Catch lEx As Exception
                    Logger.Dbg("At ExceedPercent " & lExceedPercent & " Exception " & lEx.ToString)
                End Try
            Next
            If ldates(2) = lLastDayOfMonth(ldates(1)) Then
                lStr.AppendLine()
                lStr.Append("    Mean ")
                For I As Integer = 0 To lExceedanceMeans.Count - 1
                    lStr.Append(DecimalAlign((lExceedanceMeans(I) / ldates(2)).ToString, 8, 0).Replace(",", ""))
                    lExceedanceMeans(I) = 0.0
                Next
                lStr.AppendLine()
            End If
        Next
        Return lStr.ToString
    End Function

    Public Function doDurHydPlot(ByVal aTS As atcTimeseries) As Boolean
        Dim doneIt As Boolean = True
        Dim lp As String = ""
        Dim lgraphForm As New atcGraph.atcGraphForm()

        Dim lSeasonDay As New atcSeasonsDayOfYear
        Dim lSplit As atcTimeseriesGroup = lSeasonDay.Split(aTS, Nothing)
        Dim lDataGroup As New atcDataGroup
        Dim lExceedancePcts As Double() = {0, 10, 20, 30, 50, 70, 80, 90, 100}
        For Each lExceedPct As Double In lExceedancePcts


        Next


        'Dim lZgc As ZedGraphControl = lgraphForm.ZedGraphCtrl
        'Dim lGraphDur As New clsGraphProbability(aDatagroup, lZgc)
        'lgraphForm.Grapher = lGraphDur

        'With lGraphDur.ZedGraphCtrl.GraphPane
        '    With .XAxis
        '        .Scale.MaxAuto = False
        '        .Scale.MinAuto = False
        '        .MinorGrid.IsVisible = False
        '        .MajorGrid.IsVisible = False
        '        .Scale.Min = 0.001
        '    End With
        '    '.YAxis.Type = AxisType.Linear
        '    .YAxis.MinorGrid.IsVisible = False
        '    .YAxis.MajorGrid.IsVisible = False

        '    If .YAxis.Scale.Min < 1 Then
        '        .YAxis.Scale.MinAuto = False
        '        .YAxis.Scale.Min = 1
        '        '.YAxis.Scale.Max = 1000000
        '        .AxisChange()
        '    End If

        '    '.Legend.Position = LegendPos.TopFlushLeft
        '    '.IsPenWidthScaled = True
        '    '.LineType = LineType.Stack
        '    '.ScaledPenWidth(50, 2)
        '    .CurveList.Item(0).Color = Drawing.ColorTranslator.FromHtml("#FF0000") 'Base condition: red
        '    '.CurveList.Item(1).Color = Drawing.ColorTranslator.FromHtml("#00FF00") 'Natural condition: green

        '    For Each li As LineItem In .CurveList
        '        li.Line.Width = 2
        '        Dim lFS As New FontSpec
        '        lFS.FontColor = li.Line.Color
        '        li.Label.FontSpec = lFS
        '        li.Label.FontSpec.Border.IsVisible = False
        '    Next
        'End With

        lgraphForm.Show()

        Return doneIt
    End Function

    Public Function DRNHYD(ByVal aTS As atcTimeseries, _
                           ByVal aSTMO As Integer, _
                           ByVal aEDMO As Integer, _
                           ByVal aDECPLA As Integer, _
                           ByVal aSIGDIG As Integer, _
                           ByVal aFLDWID As Integer, _
                           ByVal aNCI As Integer, _
                           ByVal aPCTILE() As Double) As String
        Dim lStr As String = String.Empty
        Dim MXYR As Integer = 150

        Dim lNCNT(365) As Integer
        Dim lNMIS(365) As Integer
        Dim lFLOW(366) As Double
        Dim lFLDR(MXYR, 365) As Double
        Dim lFPCT(365, 12) As Double
        Dim lPYRS, LPYEAR, lPDAY, lIPT, lNPTS As Integer
        Dim lSDATIM(5), lEDATTIM(5), TEMP(11), TEMPX(5), TEMPY(5) As Integer
        Dim lTUNITS As Integer = 4
        Dim lTSTEP As Integer = 1

        Dim lDateFormat As New atcDateFormat
        Dim lStartDate As String = lDateFormat.JDateToString(aTS.Attributes.GetValue("Start Date"))
        Dim lEndDate As String = lDateFormat.JDateToString(aTS.Attributes.GetValue("End Date"))

        'Construct Title Lines
        lStr = Space(10) & "Duration hydrograph for " & aTS.Attributes.GetValue("STANAM", "") & vbCrLf
        lStr &= Space(30) & "Station id  " & aTS.Attributes.GetValue("ISTAID", "") & vbCrLf
        lStr &= Space(22) & "For period " & lStartDate & " to " & lEndDate & vbCrLf & vbCrLf
        lStr &= Space(6) & "Num" & Space(30) & "Percentile" & vbCrLf
        lStr &= " Oct  yrs     Max    0.10    0.20    0.30    0.50    0.70    0.80    0.90     Min" 'TODO: Check if always starts with Oct!

        'Initialize arrays and counters
        For I As Integer = 1 To 365
            lNCNT(I) = 0 : lNMIS(I) = 0 : lFLOW(I) = 0
            For J As Integer = 1 To MXYR
                lFLDR(I, J) = 0.0 'for each day, multiple years
            Next
        Next

        'TEMP is available begin date and end date for first season
        'TEMPX is start date for first season
        'TEMPY is start date for last season

        DHBEGN(lSDATIM, lEDATTIM, aSTMO, aEDMO, TEMP, TEMPX, TEMPY, lIPT)
        DHLEAP(TEMPX, aSTMO, aEDMO, lPDAY, LPYEAR)
        'get period in 1st season, start dates of 1st and last season
        'find leap day and index to first leap year
        lPYRS = 4 - LPYEAR
        Dim lavailDateBegin1stSeason(5) As Integer
        Dim lavailDateEnd1stSeason(5) As Integer
        For I As Integer = 0 To 5
            lavailDateBegin1stSeason(I) = TEMP(I)
        Next
        For I As Integer = 6 To 11
            lavailDateEnd1stSeason(I) = TEMP(I)
        Next
        TimDif(lavailDateBegin1stSeason, lavailDateEnd1stSeason, lTUNITS, lTSTEP, lNPTS)

G200:
        'retrieve data (modDate:TimDif, atcDataSourceWDM.ReadData
        lNPTS = lNPTS + lIPT - 1
        lIPT = 1

        If lPYRS = 4 Or lPYRS = 0 Then
            'leap year, send status report
            If lPDAY > 0 And lPDAY <= lNPTS Then
                'leap day needs to be removed
                lNPTS = lNPTS - 1
                For J As Integer = lPDAY To lNPTS
                    lFLOW(J) = lFLOW(J + 1)
                Next
                lFLOW(lNPTS + 1) = -999.0
            End If
            lPYRS = 1
        Else
            'don't need status or to remove leap day
            lPYRS = lPYRS + 1
        End If

        Dim K As Integer
        For J As Integer = 1 To lNPTS
            If lFLOW(J) >= 0.0 Then
                lNCNT(J) = lNCNT(J) + 1
                K = lNCNT(J)
                lFLDR(K, J) = lFLOW(J)
            Else
                lNMIS(J) = lNMIS(J) + 1
            End If
        Next

        'adjust dates for next year of data
        lIPT = 1
        Dim lproblem As Integer
        If lproblem Then GoTo G200

        Dim lFMIN As Double = Double.MaxValue
        Dim lFMAX As Double = Double.MinValue

        For I As Integer = 1 To 365
            'sort the flows
            'CALL ASRTRP(NCNT(I), FLDR(1, I))
            For J As Integer = 1 To aNCI
                If lNCNT(I) < 2 Then
                    'no data, or only 1 good value
                    lFPCT(I, J) = -999.0
                Else
                    'compute the percentiles
                    If aPCTILE(J) < 0.00001 Then
                        'must be 0, use maximum
                        lFPCT(I, J) = lFLDR(1, I)
                    Else
                        'compute via linear interpolation
                        'get base location in sorted array for the
                        'percentile (flip-flop percentiles)
                    End If
                End If
            Next
        Next

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

    Public Function DHLPYR(ByVal aYEAR As Integer) As Integer
        Dim lYear1 As Integer = aYEAR + 1
        Dim lYear2 As Integer = aYEAR + 2
        Dim lYear3 As Integer = aYEAR + 3
        If (aYEAR Mod 4 = 0 And Not aYEAR Mod 100 = 0) Or aYEAR Mod 400 = 0 Then
            Return 0 'the current year is leap
        ElseIf (lYear1 Mod 4 = 0 And Not lYear1 Mod 100 = 0) Or lYear1 Mod 400 = 0 Then
            Return 1 'next year is leap year
        ElseIf (lYear2 Mod 4 = 0 And Not lYear2 Mod 100 = 0) Or lYear2 Mod 400 = 0 Then
            Return 2 'leap year is two years later
        ElseIf (lYear3 Mod 4 = 0 And Not lYear3 Mod 100 = 0) Or lYear3 Mod 400 = 0 Then
            Return 3 'leap year is three years later
        End If
    End Function

    Public Sub DHLEAP(ByVal aDATBGN() As Integer, _
                      ByVal aSTMO As Integer, _
                      ByVal aEDMO As Integer, _
                      ByRef aLPDAY As Integer, ByRef aLPYRS As Integer)

        Dim lDATLEP() As Integer = {0, 2, 28, 24, 0, 0} 'year,month,day,hour,minute,second
        Dim lTUNITS As Integer = 4
        Dim lTSSTEP As Integer = 1

        If (aSTMO = 2 Or aEDMO = 2) Or _
           (aSTMO > 2 And aEDMO >= 2 And aSTMO > aEDMO) Or _
           (aSTMO = 1 And aEDMO >= 2) Then
            'February is included in season, which position is leap day
            If aSTMO <= 2 Then
                'February in starting year of season
                lDATLEP(0) = aDATBGN(0)
                aLPYRS = DHLPYR(aDATBGN(0))
            Else
                'February in second year of season
                lDATLEP(0) = aDATBGN(0) + 1
                aLPYRS = DHLPYR(aDATBGN(0) + 1)
            End If
            'find Feb 28 and increment by 1 to the 29th
            TimDif(aDATBGN, lDATLEP, lTUNITS, lTSSTEP, aLPDAY)
            aLPDAY += 1
        Else
            'February is not included in season
            aLPDAY = 0
            aLPYRS = DHLPYR(aDATBGN(0))
        End If
    End Sub

    Public Sub DHBEGN(ByVal aDATBGN() As Integer, _
                      ByVal aDATEND() As Integer, _
                      ByVal aSTMO As Integer, _
                      ByVal aEDMO As Integer, _
                      ByRef aSEASON() As Integer, _
                      ByRef aSEASBG() As Integer, _
                      ByRef aSEASND() As Integer, _
                      ByRef aIPT As Integer)
        'C     + + + DUMMY ARGUMENTS + + +
        'INTEGER   DATBGN(6), DATEND(6), STMO, EDMO,
        '$         SEASON(12), SEASBG(6), SEASND(6), IPT

        Dim lTUNITS As Integer = 4
        Dim lTSSTEP As Integer = 1
        Dim lI6 As Integer = 6

        'COPYI(I6, DATBGN, SEASON)
        For I As Integer = 0 To 5
            aSEASON(I) = aDATBGN(I)
        Next
        aSEASON(7) = aEDMO
        aSEASON(9) = 24
        aSEASON(10) = 0
        aSEASON(11) = 0
        aSEASBG(1) = aSTMO
        aSEASBG(2) = 1
        aSEASBG(3) = 0
        aSEASBG(4) = 0
        aSEASBG(5) = 0
        aSEASND(1) = aSTMO
        aSEASND(2) = 1
        aSEASND(3) = 0
        aSEASND(4) = 0
        aSEASND(5) = 0

        If aSTMO <= aEDMO Then
            'season begins and ends in same calendar year
            If aDATBGN(1) <= aEDMO Then
                'start date begins before end of season
                aSEASON(6) = aDATBGN(0)
                aSEASBG(0) = aDATBGN(0)
            Else
                'start date begins after end of season, next year
                aSEASON(0) = aDATBGN(0) + 1
                aSEASON(1) = aSTMO
                aSEASON(6) = aDATBGN(0) + 1
                aSEASBG(0) = aDATBGN(0) + 1
            End If
            If aDATEND(1) >= aSTMO Then
                'end date ends after end of season
                aSEASND(0) = aDATEND(0)
            Else
                'end date ends before beginning of season, previous year
                aSEASND(0) = aDATEND(0) - 1
            End If
        Else
            'season span calendar years
            If aDATBGN(1) >= aSTMO Then
                'start date begins before jan 1, season ends in next year
                aSEASON(6) = aDATBGN(0) + 1
                aSEASBG(0) = aDATBGN(0)
            Else
                'start date begins after jan 1, season ends in same year
                aSEASON(6) = aDATBGN(0)
                aSEASBG(0) = aDATBGN(0) - 1
            End If

            If aDATEND(1) <= aEDMO Then
                'end date is after jan 1, last season begins in previous year
                aSEASND(0) = aDATEND(0) - 1
            Else
                'end date is before jan 1, last season begins in same year
                aSEASND(0) = aDATEND(0)
            End If
        End If
        aSEASON(8) = daymon(aSEASON(6), aSEASON(7))

        'where in season does data begin
        TimDif(aSEASBG, aSEASON, lTUNITS, lTSSTEP, aIPT)
        aIPT += 1
    End Sub

    Public Class DurationReport
        Private pClassLimits As Generic.List(Of Double)
        Private pExceedPercents As Generic.List(Of Double)

        Public Sub New(Optional ByVal aClassLimits As Double() = Nothing)
            'Dim lClassLimits() As Double = {1, 2, 5, 10, 20, 25, 50, 100, 200, 250, 500, 1000, 2000, 5000, 10000, 20000, 25000, 50000, 100000}
            Dim lClassLimits() As Double = {0, 1, 1.4, 2, 2.8, 4, 5.7, 8.1, 11, 16, 23, 33, 46, 66, 93, 130, 190, 270, 380, 530, 760, 1100, 1500, 2200, 3100, 4300, 6100, 8700, 12000, 17000, 25000, 35000, 50000, 71000, 100000}
            If aClassLimits IsNot Nothing Then
                lClassLimits = aClassLimits
            End If

            pClassLimits = lClassLimits.ToList
            Dim lExceedPercents() As Double = {99, 98, 95, 90, 85, 80, 75, 70, 65, 60, 55, 50, 45, 40, 35, 30, 25, 20, 15, 10, 5, 2, 1}
            pExceedPercents = lExceedPercents.ToList
        End Sub

        Public Property ClassLimits() As Generic.List(Of Double)
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

    Public Function GenerateClasses(ByVal aNumFDclasses As Integer, ByVal fddat As atcDataGroup) As Double()

        'determine bounds for duration analysis
        Dim lMin As Double = 1000000.0#
        Dim lMax As Double = -1000000.0#
        Dim vmin(fddat.Count - 1) As Double
        Dim vmax(fddat.Count - 1) As Double
        Dim i, j, iexp, l, nci As Long
        Dim bound(1) As Single
        Dim cr, clog As Single
        Dim clas(aNumFDclasses) As Double
        Dim c As Double

        For j = 0 To fddat.Count - 1
            vmin(j) = fddat(j).Attributes.GetValue("Min")
            vmax(j) = fddat(j).Attributes.GetValue("Max")
            If vmin(j) < lMin Then lMin = vmin(j)
            If vmax(j) > lMax Then lMax = vmax(j)
        Next j

        iexp = Fix(Log10(lMax))
        bound(1) = 10.0# ^ (iexp + 1)
        If lMin <= 0.0# Then lMin = lMax / 1000.0#
        iexp = Int(Log10(lMin))
        bound(0) = 10.0# ^ (iexp)

        'set up class intervals

        cr = (bound(0) / bound(1)) ^ (1.0# / (aNumFDclasses + 1))
        clas(0) = 0.0#
        clas(1) = bound(0)
        clas(aNumFDclasses) = bound(1)

        For j = 1 To aNumFDclasses - 2
            i = aNumFDclasses - j
            clas(i) = clas(i + 1) * cr
        Next j

        'round off class intervals

        For i = 1 To aNumFDclasses
            c = clas(i)
            clog = Log10(c) + 0.001
            If clog < 0.0# Then clog = clog - 1
            l = Fix(clog)
            l = l - 1
            c = (c / (10.0# ^ l)) + 0.5
            clas(i) = (Fix(c)) * (10.0# ^ l)
        Next i
        'nci = aNumFDclasses + 1
        Return clas
    End Function
End Module
