Imports atcUtility
Imports atcData
Imports atcTimeseriesMath
Imports atcDurationCompare

Public Module DailyMonthlyCompareStats
    Public Function Report(ByVal aUci As atcUCI.HspfUci,
                           ByVal aCons As String,
                           ByVal aSite As String,
                           ByVal aSimTSer As atcTimeseries,
                           ByVal aObsTSer As atcTimeseries,
                           ByVal aRunMade As String,
                  Optional ByVal aSDateJ As Double = 0,
                  Optional ByVal aEDateJ As Double = 0,
                           Optional ByVal aPercentMissingData As Double = 0.0) As String

        Dim lSDateJ As Double = aSDateJ
        If Math.Abs(lSDateJ) < 0.00001 Then lSDateJ = aUci.GlobalBlock.SDateJ
        Dim lEDateJ As Double = aEDateJ
        If Math.Abs(lEDateJ) < 0.00001 Then lEDateJ = aUci.GlobalBlock.EdateJ

        Dim lStr As String
        lStr = "Daily and Monthly " & aCons & " Statistics for '" & IO.Path.GetFileNameWithoutExtension(aUci.Name) & "' scenario." & vbCrLf
        lStr &= "   Run Made " & aRunMade & vbCrLf
        lStr &= "   " & aUci.GlobalBlock.RunInf.Value & vbCrLf
        lStr &= "   " & TimeSpanAsString(aSDateJ, aEDateJ, "Analysis Period: ")
        lStr &= "   (Units:CFS days)" & vbCrLf & vbCrLf 'TODO: do this in inches too?

        CheckDateJ(aObsTSer, "Observed", lSDateJ, lEDateJ, lStr)
        CheckDateJ(aSimTSer, "Simulated", lSDateJ, lEDateJ, lStr)

        If aPercentMissingData > 0 Then
            lStr &= "The observed data is not continuous in this analysis period. The analysis utilizes " & vbCrLf &
                  "simulated and observed data only on the days (time periods) when observed data are " & vbCrLf &
                 "available. Use the results with caution." & vbCrLf
            lStr &= FormatNumber(aPercentMissingData, 1) & "% of observed data is missing." & vbCrLf & vbCrLf
        End If

        Dim lNewSimTSer As atcTimeseries = SubsetByDate(aSimTSer, lSDateJ, lEDateJ, Nothing)
        Dim lNewObsTSer As atcTimeseries = SubsetByDate(aObsTSer, lSDateJ, lEDateJ, Nothing)

        If lNewSimTSer.numValues <> lNewObsTSer.numValues Then
            lStr &= "   SimCount " & lNewSimTSer.numValues & " ObsCount " & lNewObsTSer.numValues & vbCrLf & vbCrLf
        End If

        lStr &= IntervalReport(aSite, atcTimeUnit.TUDay, lNewSimTSer, lNewObsTSer)
        lStr &= IntervalReport(aSite, atcTimeUnit.TUMonth, lNewSimTSer, lNewObsTSer)

        Return lStr
    End Function
End Module
