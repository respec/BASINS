Imports atcData
Imports atcData.atcDataSource.EnumExistAction
Imports atcUtility

Imports Microsoft.VisualBasic
Imports System.Collections
Imports System.IO
Imports System.Windows.Forms

Public Module ScriptStatTest
  Public Sub Main(ByVal aDataManager As atcDataManager, ByVal aBasinsPlugIn As Object)
    Dim lWDMfile As atcDataSource = New atcWdm.atcDataSourceWdm
    Dim lTsMath As atcDataSource = New atcTimeseriesMath.atcTimeseriesMath 
    Dim lSummary As atcDebugTimser.atcDebugTimserPlugin = New atcDebugTimser.atcDebugTimserPlugin 
    Dim lHighLowSource As atcDataSource = New atcTimeseriesNdayHighLow.atcTimeseriesNdayHighLow 

    Dim lArgs as Object()
    Dim lMatch As New atcDataGroup
    Dim lErr As String
    Dim lArgsMath As New atcDataAttributes
    Dim lReturnPeriod() As Double = {1.25, 2, 3, 5, 10, 20, 25, 100}
    Dim lNDays() As Double = {1, 2, 4, 7, 15, 30}

    Dim lAllowExit As Boolean = aBasinsPlugIn.RunBasinsScript("vb", "subSetBaseDir.vb", lErr, New Object() {"StatsAndNDay",""})

    SetLogFileName("StatTest.log")
    SetLogTimeStamp(False)
    LogDbg("Entry")

    aDataManager.OpenDataSource(lWDMfile, "jack.wdm", Nothing)

    'group containing dsn 3 from jack.wdm
    lArgs = New Object() {aDataManager.DataSets, "ID", "3", 0}
    lMatch = aBasinsPlugIn.RunBasinsScript("vb", "subFindMatch.vb", lErr, lArgs)

    'Summarize original data
    lSummary.Save(aDataManager, lMatch, "ShastaOrig.txt", "Expand")

    '7Q10 & 1Hi100 of original data
    lHighLowSource.DataSets.Clear()
    lArgsMath.Clear()
    lArgsMath.SetValue("Timeseries", lMatch)
    aDataManager.OpenDataSource(lHighLowSource, "7Q10", lArgsMath)
    aDataManager.OpenDataSource(lHighLowSource, "1Hi100", lArgsMath)
    lSummary.Save(aDataManager, lMatch, "ShastaFreq.txt", "Expand")

    '1Hi(lots) of original data
    lArgsMath.Clear()
    lArgsMath.SetValue("Timeseries", lMatch)
    lArgsMath.SetValue("Return Period", lReturnPeriod)
    aDataManager.OpenDataSource(lHighLowSource, "n-day high value", lArgsMath)
    lSummary.Save(aDataManager, lMatch, "ShastaFreqLots.txt", "Expand")

    '1day high ts 
    lHighLowSource.DataSets.Clear()
    lArgsMath.Clear()
    lArgsMath.SetValue("Timeseries", lMatch)
    lArgsMath.SetValue("NDay", 1)
    aDataManager.OpenDataSource(lHighLowSource, "n-day high timeseries", lArgsMath)
    lSummary.Save(aDataManager, lHighLowSource.DataSets, "Shasta1DyHigh.txt", "Display 40", "Expand")
    '1Hi100 from 1day high ts
    Dim lAnnualTimeseries As atcTimeseries = lHighLowSource.DataSets(0)
    lArgsMath.Clear()
    lArgsMath.SetValue("Timeseries", New atcDataGroup(lAnnualTimeseries))
    lArgsMath.SetValue("NDay", 1)
    lArgsMath.SetValue("Return Period", 100)
    aDataManager.OpenDataSource(lHighLowSource, "n-day high value", lArgsMath)
    lSummary.Save(aDataManager, New atcDataGroup(lAnnualTimeseries), "Shasta1DyHigh100yr.txt", "Display 40", "Expand")

    'subset to year Apr-Mar 
    lTsMath.DataSets.Clear()
    lArgsMath.Clear()
    lArgsMath.SetValue("timeseries", lMatch)
    lArgsMath.SetValue("Start Date", "1934/4/1")
    lArgsMath.SetValue("End Date", "1982/4/1")
    aDataManager.OpenDataSource(lTsMath, "subset by date", lArgsMath)
    lSummary.Save(aDataManager, lTsMath.DataSets, "ShastaWyAprMar.txt", "Expand")

    '7Q10 & 1Hi100 of drought year subset
    lHighLowSource.DataSets.Clear()
    lArgsMath.Clear()
    lArgsMath.SetValue("Timeseries", lTsMath.DataSets)
    aDataManager.OpenDataSource(lHighLowSource, "7Q10", lArgsMath)
    aDataManager.OpenDataSource(lHighLowSource, "1Hi100", lArgsMath)
    lSummary.Save(aDataManager, lTsMath.DataSets, "ShastaWyAprMarFreq.txt", "Expand")

    'more return periods of drought year subset
    lHighLowSource.DataSets.Clear()
    lArgsMath.Clear()
    lArgsMath.SetValue("Return Period", lReturnPeriod)
    lArgsMath.SetValue("NDay", lNDays)
    lArgsMath.SetValue("Timeseries", lTsMath.DataSets)
    aDataManager.OpenDataSource(lHighLowSource, "n-day low value", lArgsMath)
    lSummary.Save(aDataManager, lTsMath.DataSets, "ShastaWyAprMarFreqLots.txt", "Expand")

    '7day low ts
    lHighLowSource.DataSets.Clear()
    lArgsMath.Clear()
    lArgsMath.SetValue("Timeseries", lTsMath.DataSets)
    lArgsMath.SetValue("NDay", 7)
    aDataManager.OpenDataSource(lHighLowSource, "n-day low timeseries", lArgsMath)
    lSummary.Save(aDataManager, lHighLowSource.DataSets, "ShastaWyAprMar7DyLow.txt", "Display 40", "Expand")

    If lAllowExit Then
      Application.Exit()
    End If
  End Sub
End Module
