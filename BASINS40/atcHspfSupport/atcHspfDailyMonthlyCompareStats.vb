Imports atcUtility
Imports atcData
Imports atcTimeseriesMath

Public Module DailyMonthlyCompareStats
    Public Function Report(ByVal aUci As atcUCI.HspfUci, _
                           ByVal aCons As String, _
                           ByVal aSite As String, _
                           ByVal aSimTSer As atcTimeseries, _
                           ByVal aObsTSer As atcTimeseries, _
                           Optional ByVal aSDateJ As Double = 0, _
                           Optional ByVal aEDateJ As Double = 0) As String

        Dim lStr As String
        lStr = "Daily and Monthly " & aCons & " Statistics for '" & FilenameOnly(aUci.Name) & "' scenario." & vbCrLf
        lStr &= "   Run Made " & FileDateTime(aUci.Name) & vbCrLf
        lStr &= "   " & aUci.GlobalBlock.RunInf.Value & vbCrLf

        Dim lSDateJ As Double = aSDateJ
        If Math.Abs(lSDateJ) < 0.00001 Then lSDateJ = aUci.GlobalBlock.SDateJ
        Dim lEDateJ As Double = aEDateJ
        If Math.Abs(lEDateJ) < 0.00001 Then lEDateJ = aUci.GlobalBlock.EdateJ

        Dim lYrCnt As Double = timdifJ(lSDateJ, lEDateJ, 6, 1)
        lStr &= "Simulation Period: " & lYrCnt & " years"
        lStr &= " from " & Format(Date.FromOADate(lSDateJ), "yyyy/MM/dd")
        lStr &= " to " & Format(Date.FromOADate(lEDateJ), "yyyy/MM/dd") & vbCrLf

        lStr &= "   (Units:CFS days)" & vbCrLf & vbCrLf 'TODO: do this in inches too?

        CheckDateJ(aObsTSer, "Observed", lSDateJ, lEDateJ, lStr)
        CheckDateJ(aSimTSer, "Simulated", lSDateJ, lEDateJ, lStr)

        Dim lNewSimTSer As atcTimeseries = SubsetByDate(aSimTSer, lSDateJ, lEDateJ, Nothing)
        Dim lNewObsTSer As atcTimeseries = SubsetByDate(aObsTSer, lSDateJ, lEDateJ, Nothing)

        If lNewSimTSer.numValues <> lNewObsTSer.numValues Then
            lStr &= "   SimCount " & lNewSimTSer.numValues & " ObsCount " & lNewObsTSer.numValues & vbCrLf & vbCrLf
        End If

        lStr &= IntervalReport(aSite, atcTimeUnit.TUDay, lNewSimTSer, lNewObsTSer)
        lStr &= IntervalReport(aSite, atcTimeUnit.TUMonth, lNewSimTSer, lNewObsTSer)

        Return lStr
    End Function

    Private Sub CheckDateJ(ByVal aTSer As atcTimeseries, ByVal aName As String, _
                           ByRef aSDateJ As Double, ByRef aEDateJ As Double, ByRef aStr As String)
        Dim lDateTmp As Double = aTSer.Dates.Values(0)
        If aSDateJ < lDateTmp Then
            aStr &= "   Adjusted Start Date from " & Format(Date.FromOADate(aSDateJ), "yyyy/MM/dd") & _
                                        "to " & Format(Date.FromOADate(lDateTmp), "yyyy/MM/dd") & _
                                        " due to " & aName & vbCrLf & vbCrLf
            aSDateJ = lDateTmp
        End If
        lDateTmp = aTSer.Dates.Values(aTSer.numValues)
        If aEDateJ > lDateTmp Then
            aStr &= "   Adjusted End Date from " & Format(Date.FromOADate(aEDateJ), "yyyy/MM/dd") & _
                                        " to " & Format(Date.FromOADate(lDateTmp), "yyyy/MM/dd") & _
                                        " due to " & aName & vbCrLf & vbCrLf
            aEDateJ = lDateTmp
        End If
    End Sub

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
        lTSer1 = Aggregate(aTser1, aTimeUnit, 1, atcTran.TranAverSame)

        Dim lTSer2 As atcTimeseries
        lTSer2 = Aggregate(aTSer2, aTimeUnit, 1, atcTran.TranAverSame)

        lStr &= Attributes("Count", lTSer1, lTSer2)
        lStr &= Attributes("Mean", lTSer1, lTSer2)
        lStr &= Attributes("Geometric Mean", lTSer1, lTSer2)

        lStr &= CompareStats(lTSer1, lTSer2)

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
                                ByVal aTSer2 As atcTimeseries) As String
        Dim lStr As String = aName.PadLeft(36) & _
                             aTSer1.Attributes.GetFormattedValue(aName).PadLeft(10) & _
                             aTSer2.Attributes.GetFormattedValue(aName).PadLeft(10) & vbCrLf
        Return lStr
    End Function

    Function CompareStats(ByVal aTSer1 As atcTimeseries, _
                          ByVal aTSer2 As atcTimeseries) As String

        Dim lStr As String = ""
        Dim lNote As String = ""
        Dim lMeanError As Double = 0.0#
        Dim lMeanAbsoluteError As Double = 0.0#
        Dim lRmsError As Double = 0.0#
        Dim lValDiff As Double
        Dim lVal1 As Double
        Dim lVal2 As Double
        Dim lSkipCount As Integer = 0
        Dim lGoodCount As Integer = 0

        For lIndex As Integer = 1 To aTSer1.numValues
            lVal1 = aTSer1.Values(lIndex)
            lVal2 = aTSer2.Values(lIndex)
            If Not Double.IsNaN(lVal1) And Not Double.IsNaN(lVal2) Then
                lValDiff = lVal1 - lVal2
                lMeanError += lValDiff
                lMeanAbsoluteError += Math.Abs(lValDiff)
                lRmsError += lValDiff * lValDiff
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

        Dim lNashSutcliffeNumerator As Double = lRmsError

        lMeanError /= lGoodCount
        lMeanAbsoluteError /= lGoodCount
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

        lStr &= "Correlation Coefficient".PadLeft(36) & DecimalAlign(lCorrelationCoefficient, 15) & vbCrLf
        lStr &= "Coefficient of Determination".PadLeft(36) & DecimalAlign(lCorrelationCoefficient ^ 2, 15) & vbCrLf
        lStr &= "Mean Error".PadLeft(36) & DecimalAlign(lMeanError, 15) & vbCrLf
        lStr &= "Mean Absolute Error".PadLeft(36) & DecimalAlign(lMeanAbsoluteError, 15) & vbCrLf
        lStr &= "RMS Error".PadLeft(36) & DecimalAlign(lRmsError, 15) & vbCrLf
        lStr &= "Model Fit Efficiency".PadLeft(36) & DecimalAlign(1 - lNashSutcliffe, 15) & vbCrLf
        If lNote.Length > 0 Then
            lStr &= lNote
        End If
        Return lStr
    End Function
End Module
