Imports atcUtility
Imports atcData
Imports atcTimeseriesMath

Public Module AnnualCompareStats
    Public Function Report(ByVal aUci As atcUCI.HspfUci, _
                           ByVal aCons As String, _
                           ByVal aSite As String, _
                           ByVal aUnits As String, _
                           ByVal aPrecTSer As atcTimeseries, _
                           ByVal aSimTSer As atcTimeseries, _
                           ByVal aObsTSer As atcTimeseries, _
                           Optional ByVal aSDateJ As Double = 0, _
                           Optional ByVal aEDateJ As Double = 0) As String

        Dim lStr As String
        lStr = "Annual Simulated and Observed " & aCons & " Statistics for '" & IO.Path.GetFileNameWithoutExtension(aUci.Name) & "' scenario." & vbCrLf
        lStr &= "   Run Made " & FileDateTime(aUci.Name) & vbCrLf
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
        Dim lNewPrecTSer As atcTimeseries = SubsetByDate(aPrecTSer, lSDateJ, lEDateJ, Nothing)

        If lNewSimTSer.numValues <> lNewObsTSer.numValues Then
            lStr &= "   SimCount " & lNewSimTSer.numValues & " ObsCount " & lNewObsTSer.numValues & vbCrLf & vbCrLf
        End If

        lStr &= IntervalReport(aSite, atcTimeUnit.TUYear, lNewSimTSer, lNewObsTSer, True, lNewPrecTSer)

        Return lStr
    End Function
End Module
