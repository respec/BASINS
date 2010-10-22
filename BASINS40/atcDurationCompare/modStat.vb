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
            'lStr &= DecimalAlign(lLimit, 12, 1)
            If lClassBuckets.Keys.Contains(lLimit) Then
                lStr &= DecimalAlign(lLimit, 12, 1)
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
                lStr &= vbCrLf
            Else
                Logger.Dbg("No Bucket for " & lLimit)
            End If
            'lStr &= vbCrLf
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
        Dim lStrBuilder As New Text.StringBuilder
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

        Dim lClassLimits As Generic.List(Of Double) = aClassLimits
        For lClassLimitsIndex As Integer = 0 To lClassLimits.Count - 1
            lClassLimits(lClassLimitsIndex) = SignificantDigits(lClassLimits(lClassLimitsIndex), 4)
        Next

        Dim lTimeStep As Double = 1
        If aTSer1.numValues > 1 Then
            lTimeStep = aTSer1.Dates.Value(2) - aTSer1.Dates.Value(1)
        End If
        Dim lIndex2 As Integer = 1

        Dim lNeedToMatchDates As Boolean = False
        If aTSer1.Attributes.GetValue("Start Date") <> aTSer2.Attributes.GetValue("Start Date") OrElse _
           aTSer1.Attributes.GetValue("End Date") <> aTSer2.Attributes.GetValue("End Date") Then
            lNeedToMatchDates = True
        ElseIf aTSer1.Attributes.GetFormattedValue("Point") <> aTSer2.Attributes.GetFormattedValue("Point") Then
            lNeedToMatchDates = True
        End If

        Dim lTS1Values As New List(Of Double)
        Dim lTS2Values As New List(Of Double)

        For lIndex As Integer = 1 To aTSer1.numValues
            lVal1 = aTSer1.Values(lIndex)
            If lNeedToMatchDates Then
                lIndex2 = atcData.FindDateAtOrAfter(aTSer2.Dates.Values, aTSer1.Dates.Value(lIndex), lIndex2)
                If lIndex2 < 1 OrElse lIndex2 > aTSer2.numValues Then
                    Logger.Dbg("CompareStats: matching value not found at " & lIndex)
                    Continue For
                ElseIf aTSer1.Dates.Value(lIndex) - aTSer2.Dates.Value(lIndex2) > lTimeStep Then
                    Logger.Dbg("CompareStats: matching value found at " & lIndex2 & " is too far from date at " & lIndex)
                    Continue For
                End If
                lVal2 = aTSer2.Value(lIndex2)
            Else
                lVal2 = aTSer2.Value(lIndex)
            End If

            lVal1 = SignificantDigits(lVal1, 4)
            lVal2 = SignificantDigits(lVal2, 4)
            If Not Double.IsNaN(lVal1) And Not Double.IsNaN(lVal2) Then
                lTS1Values.Add(lVal1)
                lTS2Values.Add(lVal2)

                lValDiff = lVal1 - lVal2
                lMeanError += lValDiff
                lMeanSMO2M1 += lVal2 - lVal1
                lMeanAbsoluteError += Math.Abs(lValDiff)
                lRmsError += lValDiff * lValDiff
                lGoodCount += 1
                lClassLimit = lClassLimits(0)
                For Each lLimit As Double In lClassLimits
                    If lLimit > lVal1 Then Exit For
                    lClassLimit = lLimit
                Next

                If lClassBuckets.Keys.Contains(lClassLimit) Then
                    lClassBucket = lClassBuckets.ItemByKey(lClassLimit)
                Else
                    lClassBucket = New ClassBucket(lClassLimit)
                    lClassBucket.setErrInt(lErrInt)
                    lClassBuckets.Add(lClassLimit, lClassBucket)
                End If
                lClassBucket.Increment(lVal1, lVal2)
                lClassBucket.IncrementErr(lVal1, lVal2, lClassLimit)

                lClassLimit = lClassLimits(0)
                For Each lLimit As Double In lClassLimits
                    If lLimit > lVal2 Then Exit For
                    lClassLimit = lLimit
                Next
                If lClassBuckets.Keys.Contains(lClassLimit) Then
                    lClassBucket = lClassBuckets.ItemByKey(lClassLimit)
                Else ' this else branch will not happen because all the bins are created already for aVal1
                    lClassBucket = New ClassBucket(lClassLimit)
                    lClassBucket.setErrInt(lErrInt)
                    lClassBuckets.Add(lClassLimit, lClassBucket)
                End If
                lClassBucket.Count2 += 1
                lClassBucket.Total2 += lVal2
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

        Dim lMean1 As Double = 0.0
        Dim lMean2 As Double = 0.0

        If lNeedToMatchDates Then
            Dim lOverallTotal1 As Double = 0.0
            Dim lOverallTotal2 As Double = 0.0

            For Each lClassBucket In lClassBuckets
                lOverallTotal1 += lClassBucket.Total1
                lOverallTotal2 += lClassBucket.Total2
            Next
            lMean1 = lOverallTotal1 / lGoodCount
            lMean2 = lOverallTotal2 / lGoodCount
        Else
            lMean1 = aTSer1.Attributes.GetValue("Mean")
            lMean2 = aTSer2.Attributes.GetValue("Mean")
        End If

        lIndex2 = 1
        For lIndex As Integer = 1 To aTSer1.numValues
            lVal1 = aTSer1.Values(lIndex)

            If lNeedToMatchDates Then
                lIndex2 = atcData.FindDateAtOrAfter(aTSer2.Dates.Values, aTSer1.Dates.Value(lIndex), lIndex2)
                If lIndex2 < 1 OrElse lIndex2 > aTSer2.numValues Then
                    Logger.Dbg("CompareStats:Corr.Coef.: matching value not found at " & lIndex)
                    Continue For
                ElseIf aTSer1.Dates.Value(lIndex) - aTSer2.Dates.Value(lIndex2) > lTimeStep Then
                    Logger.Dbg("CompareStats:Corr.Coef.: matching value found at " & lIndex2 & " is too far from date at " & lIndex)
                    Continue For
                End If
                lVal2 = aTSer2.Value(lIndex2)
            Else
                lVal2 = aTSer2.Value(lIndex)
            End If

            If Not Double.IsNaN(lVal1) And Not Double.IsNaN(lVal2) Then
                lCorrelationCoefficient += (lVal1 - lMean1) * (lVal2 - lMean2)
                lNashSutcliffe += (lVal2 - lMean2) ^ 2
            End If
        Next
        'lCorrelationCoefficient /= (aTSer1.numValues - 1)
        lCorrelationCoefficient /= (lGoodCount - 1)

        Dim lSD1 As Double = 0.0
        Dim lSD2 As Double = 0.0
        If lNeedToMatchDates Then
            lSD1 = StandardDeviation(lTS1Values.ToArray, lMean1)
            lSD2 = StandardDeviation(lTS2Values.ToArray, lMean2)
        Else
            lSD1 = aTSer1.Attributes.GetValue("Standard Deviation")
            lSD2 = aTSer2.Attributes.GetValue("Standard Deviation")
        End If
        If Math.Abs(lSD1 * lSD2) > 0.0001 Then
            lCorrelationCoefficient /= (lSD1 * lSD2)
        End If
        If lNashSutcliffe > 0 Then
            lNashSutcliffe = lNashSutcliffeNumerator / lNashSutcliffe
        End If

        lStrBuilder.AppendLine("1")
        lStrBuilder.AppendLine("Note: TS, Time Series")
        lStrBuilder.AppendLine()
        lStrBuilder.AppendLine("Correlation Coefficient".PadLeft(36) & DecimalAlign(lCorrelationCoefficient, 18))
        lStrBuilder.AppendLine("Coefficient of Determination".PadLeft(36) & DecimalAlign(lCorrelationCoefficient ^ 2, 18))
        lStrBuilder.AppendLine("Mean Error".PadLeft(36) & DecimalAlign(lMeanError, 18))
        lStrBuilder.AppendLine("Mean Absolute Error".PadLeft(36) & DecimalAlign(lMeanAbsoluteError, 18))
        lStrBuilder.AppendLine("RMS Error".PadLeft(36) & DecimalAlign(lRmsError, 18))
        lStrBuilder.AppendLine("Model Fit Efficiency".PadLeft(36) & DecimalAlign(1 - lNashSutcliffe, 18))
        lStrBuilder.AppendLine()
        lStrBuilder.AppendLine()
        lStrBuilder.AppendLine()

        'lStr &= "Time Series 1" & vbCrLf & "Time Series 2" & vbCrLf

        lStrBuilder.AppendLine("            TS 1 - " & TimeserIdString(aTSer1))
        lStrBuilder.AppendLine("            TS 2 - " & TimeserIdString(aTSer2))
        lStrBuilder.AppendLine()
        lStrBuilder.AppendLine("                           Mean               Root mean")
        lStrBuilder.AppendLine("   Lower    Number    absolute error(1)     square error(2)        Bias(3)      ")
        lStrBuilder.AppendLine("   class      of     ------------------- ------------------- -------------------")
        lStrBuilder.AppendLine("   limit     cases   Average    Percent   Average    Percent  Average   Percent ")
        lStrBuilder.AppendLine(" --------- --------- --------- --------- --------- --------- --------- ---------")

        For Each lLimit As Double In lClassLimits
            'lStrBuilder.Append(DoubleToString(lLimit, 10, "0.00").PadLeft(10)) 'lower class limit
            Dim lCount As Integer = 0
            If lClassBuckets.Keys.Contains(lLimit) Then
                lStrBuilder.Append(DoubleToString(lLimit, 10, "0.00").PadLeft(10)) 'lower class limit
                lClassBucket = lClassBuckets.ItemByKey(lLimit)
                With lClassBucket
                    lStrBuilder.Append(CStr(.Count1).PadLeft(9).PadRight(10)) ' number of cases
                    If Double.IsNaN(.TotalDifference) Then .TotalDifference = 0.0
                    If Double.IsNaN(.TotalDifferencePCT) Then .TotalDifferencePCT = 0.0
                    If Double.IsNaN(.TotalSS) Then .TotalSS = 0.0
                    If Double.IsNaN(.TotalSSPCT) Then .TotalSSPCT = 0.0
                    If Double.IsNaN(.TotalBias) Then .TotalBias = 0.0
                    If Double.IsNaN(.TotalBiasPCT) Then .TotalBiasPCT = 0.0

                    lStrBuilder.Append(DoubleToString(.TotalDifference, 10, "0.000").PadLeft(10)) 'mean average
                    lStrBuilder.Append(DoubleToString(.TotalDifferencePCT, 10, "0.0").PadLeft(10)) 'mean percent
                    lStrBuilder.Append(DoubleToString(.TotalSS, 10, "0.000").PadLeft(10)) 'root mean average
                    lStrBuilder.Append(DoubleToString(.TotalSSPCT, 10, "0.0").PadLeft(10)) 'root mean percent
                    lStrBuilder.Append(DoubleToString(.TotalBias, 10, "0.000").PadLeft(10)) 'bias average
                    lStrBuilder.Append(DoubleToString(.TotalBiasPCT, 10, "0.0").PadLeft(10)) 'bias percent
                End With
                lStrBuilder.AppendLine()
            Else
                Logger.Dbg("No Bucket for " & lLimit)
            End If
            'lStrBuilder.AppendLine()
        Next

        lStrBuilder.AppendLine(" --------- --------- --------- --------- --------- --------- --------- ---------")
        lStrBuilder.Append(CStr(lGoodCount).PadLeft(19).PadRight(20)) 'total count of values
        lStrBuilder.Append(DoubleToString(lMeanAbsoluteError, 10, "0.000").PadLeft(10)) 'mean abs error
        lTPDIF *= 100.0
        lTPDIF /= lGoodCount
        lStrBuilder.Append(DoubleToString(lTPDIF, 10, "0.0").PadLeft(10)) 'Average Percent Difference: TotalDiffPercent/Total#ofObs
        lStrBuilder.Append(DoubleToString(lRmsError, 10, "0.000").PadLeft(10)) 'mean rms error
        'Logger.Msg("lRmsError = " & CStr(lRmsError))
        'Logger.Msg("TPDIF2 = " & CStr(TPDIF2))
        lTPDIF2 /= lGoodCount
        lTPDIF2 = Math.Sqrt(lTPDIF2)
        lTPDIF2 *= 100.0

        'Logger.Msg("TPDIF2 * 100 / lGoodcount = " & CStr(TPDIF2))
        lStrBuilder.Append(DoubleToString(lTPDIF2, 10, "0.0").PadLeft(10)) 'Average Percent for Square of Difference: TotalSquareDifference/Total#ofObs
        'lStr &= DecimalAlign(Math.Abs(lMeanAbsoluteError), 15)
        lStrBuilder.Append(DoubleToString(Math.Abs(lMeanSMO2M1), 10, "0.000").PadLeft(10)) 'mean bias
        lTPBias *= 100.0
        lTPBias /= lGoodCount
        lStrBuilder.AppendLine(DoubleToString(lTPBias, 10, "0.0").PadLeft(10)) 'Average Percent Bias: TotalPercentBias/Total#ofObs
        lStrBuilder.AppendLine()
        Dim STEST As Double
        STEST = Math.Sqrt(lGoodCount / (lGoodCount - 1) * (lRmsError ^ 2 - Math.Abs(lMeanSMO2M1) ^ 2))
        lStrBuilder.AppendLine("Standard error of estimate = " & DecimalAlign(STEST, 8, 2)) 'Standard error of estimate
        lStrBuilder.AppendLine("         = square root((n/n-1)*((tot.col.5)**2-(tot.col.7)**2))")

        lStrBuilder.AppendLine("(1) Average = sum(|TS2-TS1|/n)")
        lStrBuilder.AppendLine("    Percent = 100 * (sum(|TS2-TS1|/TS1))/n for all TS1 > 0")
        lStrBuilder.AppendLine("(2) Average = square root(sum((TS2-TS1)**2)/n)")
        lStrBuilder.AppendLine("    Percent = 100 * square root(sum(((TS2-TS1)/TS1)**2)/n) for all TS1 > 0")
        lStrBuilder.AppendLine("(3) Average = sum(TS2-TS1)/n")
        lStrBuilder.AppendLine("    Percent = 100 * sum(((TS2-TS1)/TS1)/n) for all TS1 > 0")
        lStrBuilder.AppendLine()
        lStrBuilder.AppendLine()
        lStrBuilder.AppendLine()

        'Table nubmer 2

        lStrBuilder.AppendLine("1")
        lStrBuilder.AppendLine("Table 2")
        lStrBuilder.AppendLine("            TS 1 - " & TimeserIdString(aTSer1))
        lStrBuilder.AppendLine("            TS 2 - " & TimeserIdString(aTSer2))
        lStrBuilder.AppendLine()

        lStrBuilder.AppendLine("         Cases equal or exceeding lower")
        lStrBuilder.AppendLine("          limit & less then upper limit     Percent cases")
        lStrBuilder.AppendLine("           ----------------------------       equal or        Average of cases")
        lStrBuilder.AppendLine("   Lower     Cases         Percent         exceeding limit   within class limits")
        lStrBuilder.AppendLine("   class   --------- ------------------- ------------------- -------------------")
        'lStrBuilder.AppendLine("   limit   Sim   Obs Simulated  Observed Simulated  Observed Simulated  Observed")
        lStrBuilder.AppendLine("   limit   TS1   TS2       TS1       TS2       TS1       TS2       TS1       TS2")
        lStrBuilder.AppendLine(" --------- ---- ---- --------- --------- --------- --------- --------- ---------")
        Dim pctfrac As Double = 0.0
        Dim numExceedPct As Double = 0.0
        Dim avgClass As Double = 0.0
        For Each lLimit As Double In lClassLimits
            'lStrBuilder.Append(DoubleToString(lLimit, 10, "0.00").PadLeft(10)) 'class limit
            If lClassBuckets.Keys.Contains(lLimit) Then
                lStrBuilder.Append(DoubleToString(lLimit, 10, "0.00").PadLeft(10)) 'class limit
                lClassBucket = lClassBuckets.ItemByKey(lLimit)
                With lClassBucket
                    lStrBuilder.Append(CStr(.Count1).PadLeft(5)) 'count 1
                    lStrBuilder.Append(CStr(.Count2).PadLeft(5)) 'count 2
                    pctfrac = .Count1 * 100.0 / lGoodCount
                    'round pct values to two decimal places, force it if need to, to guard against exp notation
                    'do this manuver in subsequent instances
                    pctfrac = Int(pctfrac * 100 + 0.5) / 100.0
                    lStrBuilder.Append(DoubleToString(pctfrac, 10, "0.00").PadLeft(10)) 'pct fraction 1
                    pctfrac = .Count2 * 100.0 / lGoodCount
                    pctfrac = Int(pctfrac * 100 + 0.5) / 100.0
                    lStrBuilder.Append(DoubleToString(pctfrac, 10, "0.00").PadLeft(10)) 'pct fraction 2
                    numExceedPct = NumberExceeding(lLimit, lClassBuckets, False) * 100.0 / lGoodCount
                    numExceedPct = Int(numExceedPct * 100 + 0.5) / 100.0
                    lStrBuilder.Append(DoubleToString(numExceedPct, 10, "0.00").PadLeft(10)) 'pe 1
                    numExceedPct = NumberExceeding(lLimit, lClassBuckets, True) * 100.0 / lGoodCount
                    numExceedPct = Int(numExceedPct * 100 + 0.5) / 100.0
                    lStrBuilder.Append(DoubleToString(numExceedPct, 10, "0.00").PadLeft(10)) 'pe 2

                    avgClass = .Total1 / .Count1 '1 average within class
                    If Double.IsNaN(avgClass) Then
                        lStrBuilder.Append(DoubleToString(0.0, 10, "0.00").PadLeft(10))
                    Else
                        lStrBuilder.Append(DoubleToString(avgClass, 10, "0.00").PadLeft(10))
                    End If

                    avgClass = .Total2 / .Count2 '2 average within class
                    If Double.IsNaN(avgClass) Then
                        lStrBuilder.Append(DoubleToString(0.0, 10, "0.00").PadLeft(10))
                    Else
                        lStrBuilder.Append(DoubleToString(avgClass, 10, "0.00").PadLeft(10))
                    End If
                End With
                lStrBuilder.AppendLine()
            Else
                Logger.Dbg("No Bucket for " & lLimit)
            End If
            'lStrBuilder.AppendLine()
        Next

        lStrBuilder.AppendLine(" --------- ---- ---- --------- --------- ---------- --------- --------- ---------")
        lStrBuilder.Append(CStr(lGoodCount).PadLeft(15) & CStr(lGoodCount).PadLeft(6))
        lStrBuilder.Append("100.00".PadLeft(9)) 'total percentage of TS1
        lStrBuilder.Append("100.00".PadLeft(10)) 'total percentage of TS2
        lTSUMA /= lGoodCount
        lTSUMB /= lGoodCount
        lStrBuilder.Append(DoubleToString(lTSUMA, 10, "0.00").PadLeft(30)) ' Avg of TS1 raw value
        lStrBuilder.Append(DoubleToString(lTSUMB, 10, "0.00").PadLeft(10)) ' Avg of TS2 raw value
        lStrBuilder.AppendLine()
        lStrBuilder.AppendLine()
        lStrBuilder.AppendLine()

        'Table Number 3

        Dim lEdIndex As Integer
        lStrBuilder.AppendLine("1")
        lStrBuilder.AppendLine("            TS 1 - " & TimeserIdString(aTSer1))
        lStrBuilder.AppendLine("            TS 2 - " & TimeserIdString(aTSer2))
        lStrBuilder.AppendLine()
        lStrBuilder.AppendLine("   Lower         Number of occurrences between indicated deviations    ")
        lStrBuilder.AppendLine("   class    -------------------------------------------------------------")
        lStrBuilder.AppendLine("   limit          -60%    -30%    -10%      0%     10%     30%     60%")
        lStrBuilder.AppendLine(" ---------  -------------------------------------------------------------")
        For Each lLimit As Double In lClassLimits
            'lStrBuilder.Append(DoubleToString(lLimit, 10, "0.00").PadLeft(10))
            'Dim lCount As Integer = 0
            If lClassBuckets.Keys.Contains(lLimit) Then
                lStrBuilder.Append(DoubleToString(lLimit, 10, "0.00").PadLeft(10))
                lClassBucket = lClassBuckets.ItemByKey(lLimit)
                With lClassBucket
                    For lEdIndex = 0 To .ErrDevCount.Length - 1
                        If lEdIndex > 0 Then
                            lStrBuilder.Append(CStr(.ErrDevCount(lEdIndex)).PadLeft(8))
                        ElseIf lEdIndex > 3 Then
                            lStrBuilder.Append(CStr(.ErrDevCount(lEdIndex)).PadLeft(10))
                        ElseIf lEdIndex > 5 Then
                            lStrBuilder.Append(CStr(.ErrDevCount(lEdIndex)).PadLeft(14))
                        Else
                            lStrBuilder.Append(CStr(.ErrDevCount(lEdIndex)).PadLeft(6))
                        End If
                    Next
                End With
                lStrBuilder.AppendLine()
            Else
                Logger.Dbg("No Bucket for " & lLimit)
            End If
            'lStrBuilder.AppendLine()
        Next

        lStrBuilder.AppendLine(" ---------  -------------------------------------------------------------")
        'Loop through ErrIntv then loop through classlimit to sum up total entry in each errintv
        Dim lErrTot As Integer = 0
        lStrBuilder.Append(Space(10))
        For i As Integer = 0 To lErrInt.Length
            lErrTot = 0
            For Each lLimit As Double In lClassLimits
                If lClassBuckets.Keys.Contains(lLimit) Then
                    lClassBucket = lClassBuckets.ItemByKey(lLimit)
                    lErrTot += lClassBucket.ErrDevCount(i)
                End If
            Next
            If i = 0 Then
                lStrBuilder.Append(CStr(lErrTot).PadLeft(7))
            Else
                lStrBuilder.Append(CStr(lErrTot).PadLeft(8))
            End If
        Next
        lStrBuilder.AppendLine()
        lStrBuilder.AppendLine()

        'Table 3
        lStrBuilder.AppendLine("1")
        lStrBuilder.AppendLine(" Percent time value was exceeded")
        lStrBuilder.AppendLine()
        lStrBuilder.AppendLine("     Flow      Flow")
        lStrBuilder.AppendLine("      TS1       TS2   %Exceedance")
        lStrBuilder.AppendLine(" --------- ---------- -----------")
        Dim lDurationReport As New DurationReport
        For Each lExceedPercent As Double In lDurationReport.ExceedPercents
            Try
                Dim lNonExceedPercent As Double = 100 - lExceedPercent
                Dim lNonExceedPercentString As String = (lNonExceedPercent).ToString.PadLeft(2, "0")
                Dim lNonExceedValue1 As Double = aTSer1.Attributes.GetValue("%" & lNonExceedPercentString)
                Dim lNonExceedValue2 As Double = aTSer2.Attributes.GetValue("%" & lNonExceedPercentString)
                lStrBuilder.Append(DoubleToString(lNonExceedValue1, 10, "0.0").PadLeft(10))
                lStrBuilder.Append(DoubleToString(lNonExceedValue2, 10, "0.0").PadLeft(10))
                lStrBuilder.AppendLine(lExceedPercent.ToString.PadLeft(6))
            Catch lEx As Exception
                Logger.Dbg("At ExceedPercent " & lExceedPercent & " Exception " & lEx.ToString)
            End Try
        Next

        lStrBuilder.AppendLine()
        lStrBuilder.AppendLine()
        lStrBuilder.AppendLine()

        If lNote.Length > 0 Then
            lStrBuilder.AppendLine(lNote)
        End If
        Return lStrBuilder.ToString
    End Function

    Public Function StandardDeviation(ByVal aValues() As Double, ByVal aMean As Double) As Double
        Dim lSumSquaredDeviation As Double = 0
        For Each lVal As Double In aValues
            lSumSquaredDeviation += (lVal - aMean) * (lVal - aMean)
        Next
        Return Math.Sqrt(lSumSquaredDeviation / (aValues.Length - 1))
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

    Public Function DurationHydrograph(ByVal aTS As atcTimeseries, ByVal aPcts As Double()) As String
        If aPcts Is Nothing OrElse aPcts.Length = 0 Then
            aPcts = DefaultExceedances()
        End If
        Dim lStr As StringBuilder = New StringBuilder()
        Dim lExceedancePcts(aPcts.Length - 1) As Double
        For I As Integer = 0 To aPcts.Length - 1
            lExceedancePcts(I) = aPcts(I)
        Next
        For I As Integer = 0 To lExceedancePcts.Length - 1
            lExceedancePcts(I) = lExceedancePcts(I) * 100
        Next
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

        Dim lSeasonDay As New atcSeasonsMonthDay
        Dim lSplit As atcTimeseriesGroup = lSeasonDay.Split(aTS, Nothing)

        ''For debugging ------------
        'Dim lsw As IO.StreamWriter = New IO.StreamWriter("G:\Admin\USGS20801_DO11-SWSTAT\DurationHydrograph\zSeasonValues.txt", False)
        'For I As Integer = 0 To lSplit(0).Values.Length - 1
        '    Dim ldates(5) As Integer
        '    J2Date(lSplit(0).Dates.Value(I), ldates)
        '    Dim lval As Double = lSplit(0).Value(I)
        '    If Not Double.IsNaN(lval) Then lsw.WriteLine(ldates(1) & "/" & ldates(2) & "/" & ldates(0) & "," & lval)
        'Next
        'lsw.Close()

        'lsw = New IO.StreamWriter("G:\Admin\USGS20801_DO11-SWSTAT\DurationHydrograph\zSeasonValuesOriginal.txt", False)
        'For I As Integer = 0 To aTS.Values.Length - 1
        '    Dim ldates(5) As Integer
        '    J2Date(aTS.Dates.Value(I), ldates)
        '    If ldates(1) = 10 And ldates(2) = 2 Then
        '        Dim lval As Double = aTS.Value(I)
        '        lsw.WriteLine(ldates(1) & "/" & ldates(2) & "/" & ldates(0) & "," & lval)
        '    End If
        'Next
        'lsw.Close()
        ''For debugging -------------

        Dim lDateFormat As New atcDateFormat
        Dim lStartDate As String = lDateFormat.JDateToString(aTS.Attributes.GetValue("Start Date"))
        Dim lEndDate As String = lDateFormat.JDateToString(aTS.Attributes.GetValue("End Date"))

        Dim lHeaderLines As String = "1" & vbCrLf
        lHeaderLines &= Space(10) & "Duration hydrograph for " & aTS.Attributes.GetValue("STANAM", "") & vbCrLf
        lHeaderLines &= Space(30) & "Station id  " & aTS.Attributes.GetValue("ISTAID", "") & vbCrLf
        lHeaderLines &= Space(22) & "For period " & lStartDate & " to " & lEndDate & vbCrLf & vbCrLf
        lHeaderLines &= Space(6) & "Num" & Space(30) & "Percentile" & vbCrLf
        lHeaderLines &= " ZZZ  yrs     Max"
        For Each lpct As Double In aPcts
            If lpct > 0 And lpct < 1 Then
                lHeaderLines &= DecimalAlign(lpct.ToString, 8, 2)
            End If
        Next
        lHeaderLines &= "     Min"

        Dim lYrs As Integer = 0
        Dim lTempVal As Double = 0
        Dim lprevTimeStep As Integer = -99
        For Each lTS As atcTimeseries In lSplit
            lYrs = lTS.Attributes.GetValue("count")
            'Dim ldates(5) As Integer
            'J2Date(lTS.Dates.Values(0), ldates)
            'timcnv(ldates)
            Dim lMonthDay() As String = lTS.Attributes.GetValue("seasonname").split("/")
            Dim lMonth As Integer = CInt(lMonthDay(0))
            Dim lDay As Integer = CInt(lMonthDay(1))
            If lMonth <> lprevTimeStep Then
                Dim lHeader As String = lHeaderLines.Replace("ZZZ", MonthName3(lMonth))
                lStr.AppendLine(lHeader) : lStr.AppendLine()
                lprevTimeStep = lMonth
            End If

            lStr.Append(lDay.ToString.PadLeft(4) & lYrs.ToString.PadLeft(5))
            For Each lExceedPercent As Double In lExceedancePcts
                Dim I As Integer = Array.IndexOf(lExceedancePcts, lExceedPercent)
                Try
                    Dim lNonExceedPercent As Double = 100 - lExceedPercent
                    Dim lNonExceedPercentString As String = (lNonExceedPercent).ToString.PadLeft(2, "0")
                    Dim lNonExceedValue1 As Double = lTS.Attributes.GetValue("%" & lNonExceedPercentString)
                    Dim lWidth As Integer = 8
                    If I = 0 Then lWidth = 9
                    lStr.Append(DecimalAlign(lNonExceedValue1.ToString, lWidth, 0).Replace(",", ""))
                    lExceedanceMeans(I) += lNonExceedValue1
                Catch lEx As Exception
                    Logger.Dbg("At ExceedPercent " & lExceedPercent & " Exception " & lEx.ToString)
                End Try
            Next
            lStr.AppendLine()

            If lDay = lLastDayOfMonth(lMonth) Then
                lStr.AppendLine()
                lStr.Append("    Mean ")
                For I As Integer = 0 To lExceedanceMeans.Count - 1
                    Dim lWidth As Integer = 8
                    If I = 0 Then lWidth = 9
                    lStr.Append(DecimalAlign((lExceedanceMeans(I) / lDay).ToString, lWidth, 0).Replace(",", ""))
                    lExceedanceMeans(I) = 0.0
                Next
                lStr.AppendLine()
            End If
        Next
        Return lStr.ToString
    End Function

    Public Function DurationHydrographSeasons(ByVal aTS As atcTimeseries, ByVal aPcts As Double()) As atcTimeseriesGroup
        If aPcts Is Nothing OrElse aPcts.Length = 0 Then
            aPcts = DefaultExceedances()
        End If
        Dim lDataGroup As New atcTimeseriesGroup
        Dim lExceedancePcts(aPcts.Length - 1) As Double
        For I As Integer = 0 To aPcts.Length - 1
            lExceedancePcts(I) = aPcts(I)
        Next
        For I As Integer = 0 To lExceedancePcts.Length - 1
            lExceedancePcts(I) = lExceedancePcts(I) * 100
        Next
        'Make sure it is daily timeseries
        Select Case aTS.Attributes.GetValue("Time Unit")
            Case atcTimeUnit.TUDay
            Case Else
                aTS = Aggregate(aTS, atcTimeUnit.TUDay, 1, atcTran.TranAverSame) 'trans aver same for flow
        End Select


        Dim lStartYMD As String = String.Empty
        Dim lEndYMD As String = String.Empty

        Dim lDateYMD(5) As Integer
        J2Date(aTS.Attributes.GetValue("start date"), lDateYMD)
        lStartYMD = lDateYMD(0) & "/" & DoubleToString(lDateYMD(1) * 1.0, 2, "##").PadLeft(2) & "/" & DoubleToString(lDateYMD(2) * 1.0, 2, "##").PadLeft(2)
        J2Date(aTS.Attributes.GetValue("end date"), lDateYMD)
        lEndYMD = lDateYMD(0) & "/" & DoubleToString(lDateYMD(1) * 1.0, 2, "##").PadLeft(2) & "/" & DoubleToString(lDateYMD(2) * 1.0, 2, "##").PadLeft(2)

        Dim lSeasonDay As New atcSeasonsMonthDay
        Dim lSplit As atcTimeseriesGroup = lSeasonDay.Split(aTS, Nothing)
        'Get timeseries dates
        Dim lDates As New List(Of Double)
        Dim lFakeYear As Integer = 0
        For Each lts As atcTimeseries In lSplit
            Dim lMonthDay() As String = lts.Attributes.GetValue("seasonname").split("/")
            Dim lMonth As Integer = CInt(lMonthDay(0))
            Dim lDay As Integer = CInt(lMonthDay(1))
            If lFakeYear = 0 Then
                If lMonth > 2 Then
                    lFakeYear = 1999
                Else
                    lFakeYear = 2000
                End If
            ElseIf lMonth = 1 AndAlso lDay = 1 Then
                lFakeYear += 1
            End If
            lDates.Add(Jday(lFakeYear, lMonth, lDay, 24, 0, 0))
        Next
        lDates.Sort()
        lDates.Insert(0, atcUtility.GetNaN)

        For Each lExceedPercent As Double In lExceedancePcts
            Dim lNonExceedPercent As Double = 100 - lExceedPercent
            Dim lNonExceedPercentString As String = (lNonExceedPercent).ToString.PadLeft(2, "0")
            Dim lPEDecimal As String = DoubleToString(lExceedPercent / 100.0, 5, "#0.00").PadLeft(5)

            Dim lNewPETS As New atcTimeseries(Nothing)
            lNewPETS.Dates = New atcTimeseries(Nothing)
            lNewPETS.numValues = lDates.Count - 1
            lNewPETS.Dates.Values = lDates.ToArray()
            lNewPETS.Attributes.SetValue("STAID", aTS.Attributes.GetValue("staid", ""))
            lNewPETS.Attributes.SetValue("STANAM", aTS.Attributes.GetValue("stanam", ""))
            lNewPETS.Attributes.SetValue("StartYMD", lStartYMD)
            lNewPETS.Attributes.SetValue("EndYMD", lEndYMD)
            If Not aTS.Attributes.GetValue("Constituent").ToString.Trim = "" Then
                lNewPETS.Attributes.SetValue("Constituent", aTS.Attributes.GetValue("Constituent"))
            ElseIf Not aTS.Attributes.GetValue("TSTYPE").ToString.Trim = "" Then
                lNewPETS.Attributes.SetValue("TSTYPE", aTS.Attributes.GetValue("TSTYPE"))
            End If
            lNewPETS.Attributes.SetValue("PEDecimal", lPEDecimal)

            For J As Integer = 1 To lNewPETS.Dates.numValues
                Dim lDateArray(5) As Integer
                J2Date(lNewPETS.Dates.Value(J), lDateArray)
                Dim lSeasonName As String = lDateArray(1) & "/" & lDateArray(2)
                For Each lTS As atcTimeseries In lSplit
                    If lTS.Attributes.GetValue("seasonname") = lSeasonName Then
                        Try
                            lNewPETS.Value(J) = lTS.Attributes.GetValue("%" & lNonExceedPercentString)
                        Catch lEx As Exception
                            Logger.Dbg("At ExceedPercent " & lExceedPercent & " Exception " & lEx.ToString)
                        End Try
                        Exit For
                    End If
                Next 'lTS in lSplit
            Next ' J date
            lDataGroup.Add(lNewPETS)
        Next ' lExccedPercent
        Return lDataGroup
    End Function

    Public Function DefaultExceedances() As Double()
        Dim lDef() As Double = {0.0, 0.1, 0.2, 0.3, 0.5, 0.7, 0.8, 0.9, 1.0}
        Return lDef
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

    Public Class DurationReport
        Private pClassLimits As Generic.List(Of Double)
        Private pExceedPercents As Generic.List(Of Double)

        Public Sub New(Optional ByVal aClassLimits As Double() = Nothing)
            'Dim lClassLimits() As Double = {1, 2, 5, 10, 20, 25, 50, 100, 200, 250, 500, 1000, 2000, 5000, 10000, 20000, 25000, 50000, 100000}
            '20801-DO-11 example class limits
            Dim lClassLimits() As Double = {0, 1, 1.4, 2, 2.8, 4, 5.7, 8.1, 11, 16, 23, 33, 46, 66, 93, 130, 190, 270, 380, 530, 760, 1100, 1500, 2200, 3100, 4300, 6100, 8700, 12000, 17000, 25000, 35000, 50000, 71000, 100000}
            'Original SWSTAT class limits
            'Dim lClassLimits() As Double = {0.0, 1.0, 1.3, 1.7, 2.3, 3.1, 4.0, 5.3, 7.1, 9.3, 12.0, 16.0, 22.0, 28.0, 38.0, 50.0, 66.0, 87.0, 110.0, 150.0, 200.0, 270.0, 350.0, 460.0, 610.0, 810.0, 1100.0, 1400.0, 1900.0, 2500.0, 3300.0, 4300.0, 5700.0, 7600.0, 10000.0}
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

        ''' <summary>
        ''' This is for duration analysis as it only target one timeseries
        ''' </summary>
        ''' <param name="aTimser"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Friend Function ClassLimitsNeeded(ByVal aTimser As atcTimeseries) As Generic.List(Of Double)
            Dim lLimits As New Generic.List(Of Double)
            Dim lMin As Double = aTimser.Attributes.GetValue("Min")
            Dim lMax As Double = aTimser.Attributes.GetValue("Max")
            For Each lValue As Double In pClassLimits
                If lValue < lMax Then lLimits.Add(lValue)
            Next
            Return lLimits
        End Function

        ''' <summary>
        ''' This is for compare analysis as it need to include full range of class for BOTH timeseries
        ''' </summary>
        ''' <param name="aTimser1"></param>
        ''' <param name="aTimser2"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Friend Function ClassLimitsNeeded(ByVal aTimser1 As atcTimeseries, ByVal aTimser2 As atcTimeseries) As Generic.List(Of Double)
            Dim lLimits As New Generic.List(Of Double)
            Dim lMin1 As Double = aTimser1.Attributes.GetValue("Min")
            Dim lMin2 As Double = aTimser2.Attributes.GetValue("Min")
            Dim lMax1 As Double = aTimser1.Attributes.GetValue("Max")
            Dim lMax2 As Double = aTimser2.Attributes.GetValue("Max")
            Dim lMax As Double
            If lMax1 > lMax2 Then
                lMax = lMax1
            Else
                lMax = lMax2
            End If

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
        Public myClassLimit As Double

        Public Sub New(Optional ByVal aClassLimit = Nothing)
            If aClassLimit IsNot Nothing Then
                myClassLimit = aClassLimit
            End If
        End Sub

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
                If x <= ErrIntv(d) Then
                    ErrDevCount(d) += 1
                    lTopOff = False
                    'Dim lStr As String = Me.myClassLimit & ":" & aVal2 & ":-:" & aVal1 & ":" & DoubleToString(x, 10, "0.00") & " :<: " & CStr(ErrIntv(d)) & " : Err Intv :" & CStr(d)
                    'Logger.Dbg(lStr)
                    Exit For
                End If
            Next
            If lTopOff Then 'this is to handle the biggest (or the above last) error class
                ErrDevCount(ErrDevCount.GetUpperBound(0)) += 1
                'Logger.Dbg("ErrIntv() current index: " & CStr(d))
                'Dim lStr As String = Me.myClassLimit & ":" & aVal2 & ":-:" & aVal1 & ":" & DoubleToString(x, 10, "0.00") & " :>: " & CStr(ErrIntv(d - 1)) & " : Err Intv :" & CStr(d - 1)
                'Logger.Dbg(lStr)
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

    ''' <summary>
    ''' Default SWSTAT4.1 class limits
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GenerateClasses() As Double()
        '20801-DO-11 default class limits
        Dim lClassLimits() As Double = {0, 1, 1.4, 2, 2.8, 4, 5.7, 8.1, 11, 16, 23, 33, 46, 66, 93, 130, 190, 270, 380, 530, 760, 1100, 1500, 2200, 3100, 4300, 6100, 8700, 12000, 17000, 25000, 35000, 50000, 71000, 100000}
        'Default SWSTAT4.1 class limits
        'Dim lClassLimits() As Double = {0.0, 1.0, 1.3, 1.7, 2.3, 3.1, 4.0, 5.3, 7.1, 9.3, 12.0, 16.0, 22.0, 28.0, 38.0, 50.0, 66.0, 87.0, 110.0, 150.0, 200.0, 270.0, 350.0, 460.0, 610.0, 810.0, 1100.0, 1400.0, 1900.0, 2500.0, 3300.0, 4300.0, 5700.0, 7600.0, 10000.0}
        Return lClassLimits
    End Function

    Public Function GenerateClasses(ByVal aNumFDclasses As Integer, ByVal fddat As atcDataGroup, Optional ByVal aMin As Double = -9999.0, Optional ByVal aMax As Double = -9999.0) As Double()

        'determine bounds for duration analysis
        Dim lMin As Double = 1000000.0#
        Dim lMax As Double = -1000000.0#
        Dim vmin(fddat.Count - 1) As Double
        Dim vmax(fddat.Count - 1) As Double
        Dim i, j, iexp, l As Long
        Dim bound(1) As Single
        Dim cr, clog As Single
        Dim clas(aNumFDclasses) As Double
        Dim c As Double

        If aMax < 0 AndAlso aMin < 0 Then
            For j = 0 To fddat.Count - 1
                vmin(j) = fddat(j).Attributes.GetValue("Min")
                vmax(j) = fddat(j).Attributes.GetValue("Max")
                If vmin(j) < lMin Then lMin = vmin(j)
                If vmax(j) > lMax Then lMax = vmax(j)
            Next j
        Else
            lMin = aMin
            lMax = aMax
        End If

        iexp = Fix(Math.Log10(lMax))
        bound(1) = 10.0# ^ (iexp + 1)
        If lMin <= 0.0# Then lMin = lMax / 1000.0#
        iexp = Int(Math.Log10(lMin))
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
            clog = Math.Log10(c) + 0.001
            If clog < 0.0# Then clog = clog - 1
            l = Fix(clog)
            l = l - 1
            c = (c / (10.0# ^ l)) + 0.5
            clas(i) = (Fix(c)) * (10.0# ^ l)
        Next i
        'nci = aNumFDclasses + 1

        If aMax < 0 AndAlso aMin < 0 Then
            Return clas
        Else
            'Screen the class limits to cut off at aMin and aMax
            Dim lNewClassLimits As New List(Of Double)
            For i = 0 To clas.Length - 1
                If clas(i) >= aMin And clas(i) <= aMax Then
                    lNewClassLimits.Add(clas(i))
                End If
            Next
            Return lNewClassLimits.ToArray()
        End If
    End Function
End Module
