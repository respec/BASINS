Imports atcUtility
Imports atcData
Imports atcTimeseriesMath

Public Module MonthlyAverageCompareStats
    Public Function Report(ByVal aUci As atcUCI.HspfUci, _
                           ByVal aCons As String, _
                           ByVal aSite As String, _
                           ByVal aUnits As String, _
                           ByVal aSimTSer As atcTimeseries, _
                           ByVal aObsTSer As atcTimeseries, _
                           ByVal aRunMade As String, _
                           Optional ByVal aSDateJ As Double = 0, _
                           Optional ByVal aEDateJ As Double = 0) As String

        Dim lStr As String
        lStr = "Monthly Average Simulated and Observed " & aCons & " Statistics for '" & IO.Path.GetFileNameWithoutExtension(aUci.Name) & "' scenario." & vbCrLf
        lStr &= "   Run Made " & aRunMade & vbCrLf
        lStr &= "   " & aUci.GlobalBlock.RunInf.Value & vbCrLf

        Dim lSDateJ As Double = aSDateJ
        If Math.Abs(lSDateJ) < 0.00001 Then lSDateJ = aUci.GlobalBlock.SDateJ
        Dim lEDateJ As Double = aEDateJ
        If Math.Abs(lEDateJ) < 0.00001 Then lEDateJ = aUci.GlobalBlock.EdateJ

        lStr &= "   " & TimeSpanAsString(lSDateJ, lEDateJ)
        lStr &= "   (Units:" & aUnits & ")" & vbCrLf & vbCrLf

        CheckDateJ(aObsTSer, "Observed", lSDateJ, lEDateJ, lStr)
        CheckDateJ(aSimTSer, "Simulated", lSDateJ, lEDateJ, lStr)

        Dim lNewSimTSer As atcTimeseries = SubsetByDate(aSimTSer, lSDateJ, lEDateJ, Nothing)
        Dim lNewObsTSer As atcTimeseries = SubsetByDate(aObsTSer, lSDateJ, lEDateJ, Nothing)

        If lNewSimTSer.numValues <> lNewObsTSer.numValues Then
            lStr &= "   SimCount " & lNewSimTSer.numValues & " ObsCount " & lNewObsTSer.numValues & vbCrLf & vbCrLf
        End If

        Dim lSeasons As New atcSeasons.atcSeasonsMonth
        Dim lSeasonalAttributes As New atcDataAttributes
        lSeasonalAttributes.SetValue("Mean", 0) 'fluxes are summed from daily, monthly or annual to annual

        Dim lNewSimTSerMonth As atcTimeseries = Aggregate(lNewSimTSer, atcTimeUnit.TUMonth, 1, atcTran.TranSumDiv)
        Dim lNewSimTSerMonthCalculatedAttributes As New atcDataAttributes
        lSeasons.SetSeasonalAttributes(lNewSimTSerMonth, lSeasonalAttributes, lNewSimTSerMonthCalculatedAttributes)

        Dim lNewObsTSerMonth As atcTimeseries = Aggregate(lNewObsTSer, atcTimeUnit.TUMonth, 1, atcTran.TranSumDiv)
        Dim lNewObsTSerMonthCalculatedAttributes As New atcDataAttributes
        lSeasons.SetSeasonalAttributes(lNewObsTSerMonth, lSeasonalAttributes, lNewObsTSerMonthCalculatedAttributes)

        lStr &= Space(8) & vbTab & "Average".PadLeft(12) _
                         & vbTab & "Average".PadLeft(12) _
                         & vbTab & "Average".PadLeft(12) _
                         & vbTab & "Percent".PadLeft(12) & vbCrLf
        lStr &= "Month".PadLeft(8) & vbTab & "Simulated".PadLeft(12) _
                                   & vbTab & "Observed".PadLeft(12) _
                                   & vbTab & "Residual".PadLeft(12) _
                                   & vbTab & "Error".PadLeft(12) & vbCrLf

        Dim lSimSum As Double = 0.0
        Dim lObsSum As Double = 0.0
        Dim lResidualSum As Double = 0.0
        For Each lSimAttribute As atcDefinedValue In lNewSimTSerMonthCalculatedAttributes
            Dim lName As String = lSimAttribute.Definition.Name
            Dim lSimValue As Double = lSimAttribute.Value
            lSimSum += lSimValue
            Dim lObsValue As Double = lNewObsTSerMonthCalculatedAttributes.GetDefinedValue(lName).Value
            lObsSum += lObsValue
            Dim lResidual As Double = lSimValue - lObsValue
            lResidualSum += lResidual
            lStr &= lName.Substring(lName.Length - 3).PadLeft(8) & vbTab & DecimalAlign(lSimValue, 12, 2).PadLeft(12) _
                                                                 & vbTab & DecimalAlign(lObsValue, 12, 2).PadLeft(12) _
                                                                 & vbTab & DecimalAlign(lResidual, 12, 2).PadLeft(12) _
                                                                 & vbTab & DecimalAlign(100 * lResidual / lObsValue, 12, 2).PadLeft(12) & vbCrLf
        Next
        lStr &= "Totals".PadLeft(8) & vbTab & DecimalAlign(lSimSum, 12, 2).PadLeft(12) _
                                    & vbTab & DecimalAlign(lObsSum, 12, 2).PadLeft(12) _
                                    & vbTab & DecimalAlign(lResidualSum, 12, 2).PadLeft(12) _
                                    & vbTab & DecimalAlign(100 * lResidualSum / lObsSum, 12, 2).PadLeft(12) & vbCrLf
        Return lStr
    End Function
End Module
