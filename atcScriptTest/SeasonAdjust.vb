Imports atcData
Imports atcSeasons
Imports atcUtility

Imports Microsoft.VisualBasic
Imports System.Collections
Imports System.IO
Imports System.Windows.Forms

Public Module ScriptSeasonAdjust
  Public Sub Main(ByVal aDataManager As atcDataManager, ByVal aBasinsPlugIn As Object)
    Dim lWDMfile As atcDataSource = New atcWdm.atcDataSourceWdm
    Dim lSummary As New atcDebugTimser.atcDebugTimserPlugin
    Dim lTsMath As atcDataSource = New atcTimeseriesMath.atcTimeseriesMath 
    Dim lArgsMath As New atcDataAttributes

    Dim lOutFile as string = "SeasonAdjust_output.txt"
    Dim lErr As String
    Dim lArgs() As Object 
    Dim lMatch As New atcDataGroup
    Dim lMonthData As New atcDataGroup
    Dim lDataAdj As New atcDataGroup

    ChDriveDir("C:\test\Seasons\current")
    Dim lS As String = "subFindMatch.vb"
    If Not FileExists(lS) Then
      FileCopy("..\scripts\" & lS, lS)
    End If

    SaveFileString(lOutFile, "Entry" & vbCrLf)

    Dim lWdmName As String = "jack.wdm"
    aDataManager.OpenDataSource(lWDMfile, lWdmName, Nothing)
    AppendFileString(lOutFile, " Opened: " & lWdmName & vbCrLf)
    AppendFileString(lOutFile, "   CountWDM: " & lWDMfile.DataSets.Count & vbCrLf)
    AppendFileString(lOutFile, "   CountManager: " & aDataManager.DataSets.Count & vbCrLf)

    lArgs = New Object() {aDataManager.DataSets, "ID", "3", 0}
    lMatch = aBasinsPlugIn.RunBasinsScript("vb", "subFindMatch.vb", lErr, lArgs)
    AppendFileString(lOutFile, "   MatchCount:" & lMatch.Count & vbCrLf)
    lSummary.Save(aDataManager, lMatch, "ListOrig.txt", "Expand")

    Dim lSeasonsMonth As New atcSeasonsMonth
    lMonthData = lSeasonsMonth.Split(lMatch.ItemByIndex(0), aDataManager.DataSources(0))

    AppendFileString(lOutFile, "   MonthCount:" & lMonthData.Count & vbCrLf)
    lSummary.Save(aDataManager, lMonthData, "ListMonth.txt", "Expand")

    For Each lMonthTs As atcTimeseries In lMonthData
      lArgsMath.Clear()
      lArgsMath.SetValue("timeseries", New atcDataGroup(lMonthTs))
      lArgsMath.SetValue("Number", "1.1")
      aDataManager.OpenDataSource(lTsMath, "multiply", lArgsMath)
    Next
    AppendFileString(lOutFile, "   MathCount:" & lTsMath.DataSets.Count & vbCrLf)
    lSummary.Save(aDataManager, lTsMath.DataSets, "ListMonthAfterMult.txt", "Expand")

    lMonthData = lTsMath.DataSets.Clone
    lTsMath.DataSets.Clear()
    AppendFileString(lOutFile, "   MonthCount:" & lMonthData.Count & vbCrLf)
    lSummary.Save(aDataManager, lMonthData, "ListMonthAfterClone.txt", "Expand")

    lArgsMath.Clear()
    lArgsMath.SetValue("timeseries", lMonthData)
    aDataManager.OpenDataSource(lTsMath, "merge", lArgsMath)
    AppendFileString(lOutFile, "   MathAddCount:" & lTsMath.DataSets.Count & vbCrLf)
    lSummary.Save(aDataManager, lTsMath.DataSets, "ListMonthAfterAdd.txt", "Expand")

    AppendFileString(lOutFile, " Done" & vbCrLf)
    Application.Exit()
  End Sub
End Module
