Imports atcUtility
Imports atcData
Imports atcTimeseriesMath
Imports atcDurationCompare

Public Module DailyMonthlyCompareStats
    Public Function Report(ByVal aUci As atcUCI.HspfUci, _
                           ByVal aCons As String, _
                           ByVal aSite As String, _
                           ByVal aSimTSer As atcTimeseries, _
                           ByVal aObsTSer As atcTimeseries, _
                           ByVal aRunMade As String, _
                  Optional ByVal aSDateJ As Double = 0, _
                  Optional ByVal aEDateJ As Double = 0) As String

        Dim lSDateJ As Double = aSDateJ
        If Math.Abs(lSDateJ) < 0.00001 Then lSDateJ = aUci.GlobalBlock.SDateJ
        Dim lEDateJ As Double = aEDateJ
        If Math.Abs(lEDateJ) < 0.00001 Then lEDateJ = aUci.GlobalBlock.EdateJ

        Dim lStr As String
        lStr = "Daily and Monthly " & aCons & " Statistics for '" & IO.Path.GetFileNameWithoutExtension(aUci.Name) & "' scenario." & vbCrLf
        lStr &= "   Run Made " & aRunMade & vbCrLf
        lStr &= "   " & aUci.GlobalBlock.RunInf.Value & vbCrLf
        lStr &= "   " & TimeSpanAsString(lSDateJ, lEDateJ)
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
End Module