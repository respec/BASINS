Imports atcUtility.modFile
Imports atcUtility.modDate
Imports atcUtility.modString
Imports atcData

Module DailyMonthlyFlowStats
    Dim pSJdate As Double, pEJdate As Double
    'TODO: get these from exs file (in driver?)
    Dim pSimDsn As Integer = 1001
    Dim pObsDsn As Integer = 261
    Dim pArea As Double = 54831.0#
    Dim pSimConv As Double = pArea * 43560.0# / (12.0# * 24.0# * 3600.0#)

    Function Report(ByVal aUci As atcUCI.HspfUci, _
                    ByVal aDataSource As atcDataSource, _
                    ByVal aSites() As String) As String
        pSJdate = aUci.GlobalBlock.SDateJ
        pEJdate = aUci.GlobalBlock.EdateJ

        Dim lStr As String
        lStr = "Daily and Monthly Flow Statistics for '" & aUci.Name & "' scenario." & vbCrLf
        lStr &= "   Run Made " & FileDateTime(aUci.Name) & vbCrLf
        lStr &= "   " & aUci.GlobalBlock.RunInf.Value & vbCrLf
        Dim lYrCnt As Double = timdifJ(pSJdate, pEJdate, 6, 1)
        lStr &= "   Simulation Period: " & lYrCnt & " years"
        lStr &= " from " & Format(Date.FromOADate(pSJdate), "yyyy/MM/dd")
        lStr &= " to " & Format(Date.FromOADate(pEJdate), "yyyy/MM/dd") & vbCrLf
        lStr &= "   (Units:CFS)" & vbCrLf & vbCrLf 'TODO: do this in inches too?

        For Each lSite As String In aSites
            Dim lSimTSer As atcTimeseries = aDataSource.DataSets(aDataSource.DataSets.IndexFromKey(pSimDsn))
            Dim lNewSimTSer As atcTimeseries = SubsetByDate(lSimTSer, pSJdate, pEJdate, Nothing)
            lSimTSer = Nothing
            Dim lObsTSer As atcTimeseries = aDataSource.DataSets(aDataSource.DataSets.IndexFromKey(pObsDsn))
            Dim lNewObsTSer As atcTimeseries = SubsetByDate(lObsTSer, pSJdate, pEJdate, Nothing)
            lObsTSer = Nothing

            lStr &= IntervalReport(lSite, atcTimeUnit.TUDay, lNewSimTSer, lNewObsTSer)
            lStr &= IntervalReport(lSite, atcTimeUnit.TUMonth, lNewSimTSer, lNewObsTSer)
        Next

        Return lStr
    End Function

    Private Function IntervalReport(ByVal aSite As String, ByVal aTimeUnit As atcTimeUnit, _
                                    ByVal aTser1 As atcTimeseries, _
                                    ByVal aTSer2 As atcTimeseries) As String
        Dim lInterval As String = ""
        Select Case aTimeUnit
            Case atcTimeUnit.TUDay : lInterval = "Daily"
            Case atcTimeUnit.TUMonth : lInterval = "Monthly"
        End Select

        Dim lStr As String = ""
        lStr &= aSite & ":" & lInterval.PadLeft(16) & vbCrLf
        lStr &= Space(36) & "Simulated".PadLeft(10) & "Observed".PadLeft(10) & vbCrLf

        Dim lTSer1 As atcTimeseries
        lTSer1 = Aggregate(aTser1, aTimeUnit, 1, atcTran.TranSumDiv)

        Dim lTSer2 As atcTimeseries
        lTSer2 = Aggregate(aTSer2, aTimeUnit, 1, atcTran.TranSumDiv)
        lStr &= Attributes("Count", lTSer1, lTSer2)
        lStr &= Attributes("Mean", lTSer1, lTSer2, True)
        lStr &= Attributes("Geometric Mean", lTSer1, lTSer2, True)

        lStr &= CompareStats(lTSer1, lTSer2, True)

        lTSer1.Clear()
        lTSer1.Dates.Clear()
        lTSer1 = Nothing
        lTSer2.Clear()
        lTSer2.Dates.Clear()
        lTSer2 = Nothing
        lStr &= vbCrLf & vbCrLf
        Return lStr
    End Function

    Private Function Attributes(ByVal aName As String, _
                                ByVal aTSer1 As atcTimeseries, _
                                ByVal aTSer2 As atcTimeseries, _
                                Optional ByVal aConv1 As Boolean = False) As String
        Dim lValue1 As Double = aTSer1.Attributes.GetFormattedValue(aName)
        If aConv1 Then
            lValue1 *= pSimConv
        End If
        Dim lValueStr As String = DoubleToString(lValue1).PadLeft(10)

        Return aName.PadLeft(36) & lValueStr & _
               aTSer2.Attributes.GetFormattedValue(aName).PadLeft(10) & vbCrLf
    End Function

    Function CompareStats(ByVal aTSer1 As atcTimeseries, _
                          ByVal aTSer2 As atcTimeseries, _
                          Optional ByVal aConv1 As Boolean = False) As String

        Dim lMeanError As Double = 0.0#
        Dim lMeanAbsoluteError As Double = 0.0#
        Dim lRmsError As Double = 0.0#
        Dim lValDiff As Double
        Dim lVal1 As Double
        Dim lVal2 As Double

        For lIndex as Integer  = 1 To aTSer1.numValues
            lVal1 = aTSer1.Values(lIndex)
            If aConv1 Then
                lVal1 *= pSimConv
            End If
            lValDiff = lVal1 - aTSer2.Values(lIndex)
            lMeanError += lValDiff
            lMeanAbsoluteError += Math.Abs(lValDiff)
            lRmsError += lValDiff * lValDiff
        Next
        Dim lNashSutcliffeNumerator As Double = lRmsError

        lMeanError /= aTSer1.numValues
        lMeanAbsoluteError /= aTSer1.numValues
        lRmsError /= aTSer1.numValues

        If lRmsError > 0 Then
            lRmsError = Math.Sqrt(lRmsError)
        End If

        Dim lCorrelationCoefficient As Double = 0.0#
        Dim lNashSutcliffe As Double = 0.0#
        Dim lMean1 As Double = aTSer1.Attributes.GetDefinedValue("Mean").Value
        If aConv1 Then
            lMean1 *= pSimConv
        End If
        Dim lMean2 As Double = aTSer2.Attributes.GetDefinedValue("Mean").Value
        For lIndex As Integer = 1 To aTSer1.numValues
            lVal1 = aTSer1.Values(lIndex)
            If aConv1 Then
                lVal1 *= pSimConv
            End If
            lVal2 = aTSer2.Values(lIndex)
            lCorrelationCoefficient += (lVal1 - lMean1) * (lVal2 - lMean2)
            lNashSutcliffe += (lVal2 - lMean2) ^ 2
        Next
        lCorrelationCoefficient /= (aTSer1.numValues - 1)
        Dim lSD1 As Double = aTSer1.Attributes.GetDefinedValue("Standard Deviation").Value
        If aConv1 Then
            lSD1 *= pSimConv
        End If
        Dim lSD2 As Double = aTSer2.Attributes.GetDefinedValue("Standard Deviation").Value
        If Math.Abs(lSD1 * lSD2) > 0.0001 Then
            lCorrelationCoefficient /= (lSD1 * lSD2)
        End If
        If lNashSutcliffe > 0 Then
            lNashSutcliffe = lNashSutcliffeNumerator / lNashSutcliffe
        End If

        Dim lStr As String = ""
        lStr &= "Correlation Coefficient".PadLeft(36) & DoubleToString(lCorrelationCoefficient).PadLeft(15) & vbCrLf
        lStr &= "Coefficient of Determination".PadLeft(36) & DoubleToString(lCorrelationCoefficient ^ 2).PadLeft(15) & vbCrLf
        lStr &= "Mean Error".PadLeft(36) & DoubleToString(lMeanError).PadLeft(15) & vbCrLf
        lStr &= "Mean Absolute Error".PadLeft(36) & DoubleToString(lMeanAbsoluteError).PadLeft(15) & vbCrLf
        lStr &= "RMS Error".PadLeft(36) & DoubleToString(lRmsError).PadLeft(15) & vbCrLf
        lStr &= "Model Fit Efficiency".PadLeft(36) & DoubleToString(1 - lNashSutcliffe).PadLeft(15) & vbCrLf

        Return lStr
    End Function
End Module
