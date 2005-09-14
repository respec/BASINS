Imports atcData
Imports atcUtility
Imports atcSeasons

Imports Microsoft.VisualBasic
Imports System.Collections
Imports System.IO
Imports System.Windows.Forms

Public Module ScriptSeasonAttributes
  Public Sub Main(ByVal aDataManager As atcDataManager, ByVal aBasinsPlugIn As Object)
    Dim lGrapher As new atcGraph.atcGraphPlugin
    Dim lWDMfile As atcDataSource = New atcWdm.atcDataSourceWdm
    Dim lSummary As New atcDebugTimser.atcDebugTimserPlugin

    Dim lOutFile as string = "SeasonAttributes_output.txt"
    Dim lErr As String
    Dim lArgs() As Object 
    Dim lMatch As New atcDataGroup
    Dim lMonthData As New atcDataGroup

    'ChDriveDir("C:\test\Seasons\current")
    'Dim lS As String = "subFindMatch.vb"
    'If Not FileExists(lS) Then
    '  FileCopy("..\scripts\" & lS, lS)
    'End If

    SaveFileString(lOutFile, "Entry" & vbCrLf)

    Dim lWdmName As String = "jack.wdm"
    aDataManager.OpenDataSource(lWDMfile, lWdmName, Nothing)
    AppendFileString(lOutFile, " Opened: " & lWdmName & vbCrLf)
    AppendFileString(lOutFile, "   CountWDM: " & lWDMfile.DataSets.Count & vbCrLf)
    AppendFileString(lOutFile, "   CountManager: " & aDataManager.DataSets.Count & vbCrLf)

    lArgs = New Object() {aDataManager.DataSets, "ID", "3", 0}
    lMatch = aBasinsPlugIn.RunBasinsScript("vb", "subFindMatch.vb", lErr, lArgs)

    Dim lAttributes As New atcDataAttributes
    lAttributes.SetValue("Mean", 0)
    lAttributes.SetValue("Max", 0)
    lAttributes.SetValue("Min", 0)

    Dim lSeasonsMonth As New atcSeasonsMonth
    lSeasonsMonth.SetSeasonalAttributes(lMatch.ItemByIndex(0), lAttributes)
    lSummary.Save(aDataManager, lMatch, "ListMonthAttributes.txt", "Expand")

    Dim lSeasonsDayOfWeek As New atcSeasonsDayOfWeek
    Dim lAttributesTemp As New atcDataAttributes
    lSeasonsDayOfWeek.SetSeasonalAttributes(lMatch.ItemByIndex(0), lAttributes, lAttributesTemp)
    SaveFileString("ListDayOfWeekAttributes.txt", lAttributesTemp.ToString)

    lAttributesTemp.Clear()
    Dim lSeasonsCalendarYear As New atcSeasonsCalendarYear
    lSeasonsCalendarYear.SetSeasonalAttributes(lMatch.ItemByIndex(0), lAttributes, lAttributesTemp)
    SaveFileString("ListCalendarYearAttributes.txt", lAttributesTemp.ToString)

    AppendFileString(lOutFile, " Done" & vbCrLf)
    Application.Exit()
  End Sub
End Module
