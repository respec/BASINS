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
    Dim lAllowExit As Boolean = False

    Dim lTestDir As String = "c:\test"
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

    Dim lAttributes As New atcDataAttributes 'last attribute comes up first in grid
    lAttributes.SetValue("7Q10", 0)
    lAttributes.SetValue("1Hi100", 0)
    lAttributes.SetValue("Geometric Mean", 0)
    lAttributes.SetValue("Mean", 0)
    lAttributes.SetValue("Max", 0)
    lAttributes.SetValue("Min", 0)

    Dim lSeasonsMonth As New atcSeasonsMonth
    lSeasonsMonth.SetSeasonalAttributes(lMatch.ItemByIndex(0), lAttributes)
    lSummary.Save(aDataManager, lMatch, "ListMonthAttributes.txt", "Expand")

    Dim lDisp As atcDataDisplay = New atcSeasonalAttributes.atcSeasonalAttributesPlugin
    lDisp.Show(aDataManager, lMatch)
    lDisp.Save(aDataManager, lMatch, "ListSeasonalAttributesFromGrid.txt", Nothing)

    Dim lSeasonsDayOfWeek As New atcSeasonsDayOfWeek
    Dim lAttributesTemp As New atcDataAttributes
    lSeasonsDayOfWeek.SetSeasonalAttributes(lMatch.ItemByIndex(0), lAttributes, lAttributesTemp)
    SaveFileString("ListDayOfWeekAttributes.txt", lAttributesTemp.ToString)

    lAttributesTemp.Clear()
    Dim lSeasonsCalendarYear As New atcSeasonsCalendarYear
    lSeasonsCalendarYear.SetSeasonalAttributes(lMatch.ItemByIndex(0), lAttributes, lAttributesTemp)
    SaveFileString("ListCalendarYearAttributes.txt", lAttributesTemp.ToString)

    AppendFileString(lOutFile, " Done" & vbCrLf)
    If lAllowExit Then
      Application.Exit()
    End If
  End Sub
End Module
