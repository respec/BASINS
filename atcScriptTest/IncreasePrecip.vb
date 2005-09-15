Imports atcData
Imports atcUtility
Imports atcDebugTimser
Imports atcGraph
imports atcScenarioBuilder

Imports Microsoft.VisualBasic
Imports System.Collections
Imports System.IO
Imports System.Windows.Forms

Public Module IncreasePrecip
  Public Sub Main(ByVal aDataManager As atcDataManager, ByVal aBasinsPlugIn As Object)
    Dim lTsMath As atcDataSource = New atcTimeseriesMath.atcTimeseriesMath
    Dim lSummary As atcDebugTimserPlugin = New atcDebugTimser.atcDebugTimserPlugin
    Dim lGrapher As atcGraphPlugin = New atcGraph.atcGraphPlugin

    Dim lArgsMath As New atcDataAttributes
    Dim lMatch As New atcDataGroup
    Dim lArgs as Object()
    Dim lErr As String

    Dim lAllowExit As Boolean = aBasinsPlugIn.RunBasinsScript("vb", "subSetBaseDir.vb", lErr, New Object() {"Climate", ""})

    Dim lWDMfile As atcDataSource = New atcWDM.atcDataSourceWDM
    Dim lWDMFileName As String = "base.wdm"
    aDataManager.OpenDataSource(lWDMfile, lWDMFileName, Nothing)
    'group containing dsn 105 from base.wdm
    lArgs = New Object() {aDataManager.DataSets, "ID", "105", 0}
    lMatch = aBasinsPlugIn.RunBasinsScript("vb", "subFindMatch.vb", lErr, lArgs)

    lTsMath.DataSets.Clear()
    lArgsMath.Clear()
    lArgsMath.SetValue("timeseries", lMatch)
    lArgsMath.SetValue("Number", "1.0") 'Adjust to change all values
    aDataManager.OpenDataSource(lTsMath, "multiply", lArgsMath)

    Dim lNewTS As atcTimeseries = lTsMath.DataSets(0)
    For iValue As Integer = 1 To lNewTS.numValues
      Dim lDate As Date = Date.FromOADate(lNewTS.Dates.Value(iValue))
      Select Case lDate.Month
        Case 8, 9 : lNewTS.Value(iValue) *= 1.25
      End Select
    Next
    lNewTS.Attributes.CalculateAll()

    lSummary.Save(aDataManager, lMatch, "DebugTimser_OrigTS.txt")
    lSummary.Save(aDataManager, new atcDataGroup(lNewTS), "DebugTimser_NewTS.txt")

    Dim lScenarioResults = New atcDataSource
    lScenarioResults = ScenarioRun(lWDMFileName, "modified", New atcDataGroup(lNewTS))
    aDataManager.DataSources.Add(lScenarioResults)

    lArgs = New Object() {aDataManager.DataSets, "ID", "1004", 0}
    lMatch = aBasinsPlugIn.RunBasinsScript("vb", "subFindMatch.vb", lErr, lArgs)

    lSummary.Save(aDataManager, lMatch, "DebugTimser_FlowB&M.txt")

    If lAllowExit Then
      Application.Exit()
    End If
    
  End Sub
End Module
