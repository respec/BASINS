Imports atcData
Imports atcUtility
Imports atcSeasons

Imports Microsoft.VisualBasic
Imports System.Collections
Imports System.IO
Imports System.Windows.Forms

Public Module ScriptSeasons
  Public Sub Main(ByVal aDataManager As atcDataManager, ByVal aBasinsPlugIn As Object)
    Dim lGrapher As new atcGraph.atcGraphPlugin
    Dim lWDMfile As atcDataSource = New atcWdm.atcDataSourceWdm
    Dim lTsMath As atcDataSource = New atcTimeseriesMath.atcTimeseriesMath 
    Dim lSummary As New atcDebugTimser.atcDebugTimserPlugin
    Dim lHighLowSource As atcDataSource = New atcTimeseriesNdayHighLow.atcTimeseriesNdayHighLow

    Dim lOutFile as string = "SeasonSummer_output.txt"
    Dim lErr As String
    Dim lArgs() As Object 
    Dim lArgsMath As New atcDataAttributes
    Dim lMatch As New atcDataGroup
    Dim lSummerData As New atcDataGroup

    Dim lAllowExit As Boolean = False

    Dim lTestDir as string = "c:\test"
    If Curdir.ToLower.StartsWith(lTestDir) Then
      lAllowExit = True
      ChDriveDir(lTestDir & "\Seasons\")
    Else
      ChDriveDir(lTestDir & "\Seasons\current")
    End If

    SaveFileString(lOutFile, "Entry" & vbCrLf)

    Dim lWdmName As String = "jack.wdm"
    aDataManager.OpenDataSource(lWDMfile, lWdmName, Nothing)
    AppendFileString(lOutFile, " Opened: " & lWdmName & vbCrLf)
    AppendFileString(lOutFile, "   CountWDM: " & lWDMfile.DataSets.Count & vbCrLf)
    AppendFileString(lOutFile, "   CountManager: " & aDataManager.DataSets.Count & vbCrLf)

    lArgs = New Object() {aDataManager.DataSets, "ID", "3", 0}
    lMatch = aBasinsPlugIn.RunBasinsScript("vb", "subFindMatch.vb", lErr, lArgs)

    'subset to 1960-1970
    lTsMath.DataSets.Clear()
    lArgsMath.Clear()
    lArgsMath.SetValue("timeseries", lMatch)
    lArgsMath.SetValue("Start Date", "1960/1/1")
    lArgsMath.SetValue("End Date", "1971/1/1")
    aDataManager.OpenDataSource(lTsMath, "subset by date", lArgsMath)
    lSummary.Save(aDataManager, lTsMath.DataSets, "List1960to1970.txt", "Expand")

    Dim lSeasonsSummer As New atcSeasonsYearSubset("1900/6/15", "1900/9/15")
    lSummerData = lSeasonsSummer.Split(lTsMath.DataSets(0), aDataManager.DataSources(0))
    AppendFileString(lOutFile, "   SummerDataCount:" & lSummerData.Count & vbCrLf)
    lSummary.Save(aDataManager, lSummerData, "ListSummer.txt", "Expand")

    '7day low ts
    lHighLowSource.DataSets.Clear()
    lArgsMath.Clear()
    lArgsMath.SetValue("Timeseries", New atcDataGroup(lSummerData.ItemByIndex(1)))
    lArgsMath.SetValue("NDay", 7)
    aDataManager.OpenDataSource(lHighLowSource, "n-day low timeseries", lArgsMath)
    lSummary.Save(aDataManager, lHighLowSource.DataSets, "List7Low.txt", "Expand")

    AppendFileString(lOutFile, " Done" & vbCrLf)
    If lAllowExit Then
      Application.Exit()
    End If
  End Sub
End Module
