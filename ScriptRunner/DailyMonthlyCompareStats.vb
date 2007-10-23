Imports atcUtility
Imports atcData
Imports atcWDM
Imports atcTimeseriesMath
Imports MapWindow.Interfaces

Module DailyMonthlyCompareStats
    Private Const pTestPath As String = "C:\test\EXP_CAL\hyd_man.net"
    Private Const pBaseName As String = "hyd_man"

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        ChDriveDir(pTestPath)
        'open uci file
        Dim lMsg As New atcUCI.HspfMsg
        lMsg.Open("hspfmsg.mdb")
        Dim lHspfUci As New atcUCI.HspfUci
        lHspfUci.FastReadUciForStarter(lMsg, pBaseName & ".uci")

        'open WDM file
        Dim lWdmFileName As String = pTestPath & "\" & pBaseName & ".wdm"
        Dim lWdmDataSource As New atcDataSourceWDM
        lWdmDataSource.Open(lWdmFileName)
        'TODO: get the following four parms from the exs file
        Dim lCons As String = "Flow"
        Dim lSites() As String = {"RCH5"}
        Dim lArea() As Double = {54831}
        Dim lSimDsnId() As Integer = {1001}
        Dim lObsDsnId() As Integer = {261}
        Dim lStr As String = DailyMonthlyCompareStats.Report(lHspfUci, lWdmDataSource, lCons, lSites, _
                                                             lArea, lSimDsnId, lObsDsnId)
        Dim lOutFileName As String = "outfiles\DailyMonthly" & lCons & "Stats"
        If lSites.GetUpperBound(0) = 0 Then lOutFileName &= "-" & lSites(0)
        lOutFileName &= ".txt"
        SaveFileString(lOutFileName, lStr)
    End Sub

    Friend Function Report(ByVal aUci As atcUCI.HspfUci, _
                           ByVal aDataSource As atcDataSource, _
                           ByVal aCons As String, _
                           ByVal aSites() As String, _
                           ByVal aArea() As Double, _
                           ByVal aSimDsnId() As Integer, _
                           ByVal aObsDsnId() As Integer) As String


        Dim lTsMath As atcDataSource = New atcTimeseriesMath.atcTimeseriesMath
        Dim lArgsMath As New atcDataAttributes

        Dim lStr As String
        lStr = "Daily and Monthly " & aCons & " Statistics for '" & FilenameOnly(aUci.Name) & "' scenario." & vbCrLf
        lStr &= "   Run Made " & FileDateTime(aUci.Name) & vbCrLf
        lStr &= "   " & aUci.GlobalBlock.RunInf.Value & vbCrLf
        lStr &= "   " & aUci.GlobalBlock.RunPeriod & vbCrLf
        lStr &= "   (Units:CFS days)" & vbCrLf & vbCrLf 'TODO: do this in inches too?

        For lSiteIndex As Integer = 0 To aSites.GetUpperBound(0)
            Dim lSite As String = aSites(lSiteIndex)
            Dim lSimTSer As atcTimeseries = aDataSource.DataSets(aDataSource.DataSets.IndexFromKey(aSimDsnId(lSiteIndex)))
            Dim lSDateJ As Double = aUci.GlobalBlock.SDateJ
            Dim lEDateJ As Double = aUci.GlobalBlock.EdateJ
            Dim lNewSimTSer As atcTimeseries = SubsetByDate(lSimTSer, lSDateJ, lEDateJ, Nothing)
            Dim lSimConv As Double = aArea(lSiteIndex) * 43560.0# / (12.0# * 24.0# * 3600.0#) 'inches to cfs days
            lTsMath.DataSets.Clear()
            lArgsMath.Clear()
            lArgsMath.SetValue("timeseries", lNewSimTSer)
            lArgsMath.SetValue("number", lSimConv)
            lTsMath.Open("multiply", lArgsMath)
            lNewSimTSer = lTsMath.DataSets(0)
            lSimTSer = Nothing
            Dim lObsTSer As atcTimeseries = aDataSource.DataSets(aDataSource.DataSets.IndexFromKey(aObsDsnId(lSiteIndex)))
            Dim lNewObsTSer As atcTimeseries = SubsetByDate(lObsTSer, lSDateJ, lEDateJ, Nothing)
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

        Dim lMeanError As Double = 0.0#
        Dim lMeanAbsoluteError As Double = 0.0#
        Dim lRmsError As Double = 0.0#
        Dim lValDiff As Double
        Dim lVal1 As Double
        Dim lVal2 As Double

        For lIndex As Integer = 1 To aTSer1.numValues
            lVal1 = aTSer1.Values(lIndex)
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
        Dim lMean2 As Double = aTSer2.Attributes.GetDefinedValue("Mean").Value
        For lIndex As Integer = 1 To aTSer1.numValues
            lVal1 = aTSer1.Values(lIndex)
            lVal2 = aTSer2.Values(lIndex)
            lCorrelationCoefficient += (lVal1 - lMean1) * (lVal2 - lMean2)
            lNashSutcliffe += (lVal2 - lMean2) ^ 2
        Next
        lCorrelationCoefficient /= (aTSer1.numValues - 1)
        Dim lSD1 As Double = aTSer1.Attributes.GetDefinedValue("Standard Deviation").Value
        Dim lSD2 As Double = aTSer2.Attributes.GetDefinedValue("Standard Deviation").Value
        If Math.Abs(lSD1 * lSD2) > 0.0001 Then
            lCorrelationCoefficient /= (lSD1 * lSD2)
        End If
        If lNashSutcliffe > 0 Then
            lNashSutcliffe = lNashSutcliffeNumerator / lNashSutcliffe
        End If

        Dim lStr As String = ""
        lStr &= "Correlation Coefficient".PadLeft(36) & DecimalAlign(lCorrelationCoefficient, 15) & vbCrLf
        lStr &= "Coefficient of Determination".PadLeft(36) & DecimalAlign(lCorrelationCoefficient ^ 2, 15) & vbCrLf
        lStr &= "Mean Error".PadLeft(36) & DecimalAlign(lMeanError, 15) & vbCrLf
        lStr &= "Mean Absolute Error".PadLeft(36) & DecimalAlign(lMeanAbsoluteError, 15) & vbCrLf
        lStr &= "RMS Error".PadLeft(36) & DecimalAlign(lRmsError, 15) & vbCrLf
        lStr &= "Model Fit Efficiency".PadLeft(36) & DecimalAlign(1 - lNashSutcliffe, 15) & vbCrLf

        Return lStr
    End Function
End Module
