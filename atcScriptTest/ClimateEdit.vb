Imports atcData
Imports atcData.atcDataSource.EnumExistAction
Imports atcUtility

Imports Microsoft.VisualBasic
Imports System.Collections
Imports System.IO
Imports System.Windows.Forms

Public Module ScriptClimateEdit
  Public Sub Main(ByVal aDataManager As atcDataManager, ByVal aBasinsPlugIn As Object)
    Dim lWDMfile As atcDataSource = New atcWdm.atcDataSourceWdm
    Dim lTsMath As atcDataSource = New atcTimeseriesMath.atcTimeseriesMath 
    Dim lSummary As atcDebugTimser.atcDebugTimserPlugin = New atcDebugTimser.atcDebugTimserPlugin 

    Dim lArgs as Object()
    Dim lMatch As New atcDataGroup
    Dim lErr As String
    dim lId as string
    Dim lArgsMath As New atcDataAttributes

    ChDriveDir("C:\test\Climate\current")
    Dim lS As String = "subFindMatch.vb"
    If Not FileExists(lS) Then
      FileCopy("..\scripts\" & lS, lS)
    End If

    aDataManager.OpenDataSource(lWDMfile, "base.wdm", Nothing)

    'group containing observed data
    'lArgs = New Object() {aDataManager.DataSets, "Scenario", "Observed", 0}
    lArgs = New Object() {aDataManager.DataSets, "id", "262", 0}
    lMatch = aBasinsPlugIn.RunBasinsScript("vb", "subFindMatch.vb", lErr, lArgs)
    LogDbg("Match Count " & lMatch.Count)

    'Summarize original data
    For Each lCurrentMatch As atcTimeseries In lMatch
      lId = lCurrentMatch.Attributes.GetValue("id")
      LogDbg("Summarize " & lId)
      lSummary.Save(aDataManager, New atcDataGroup(lCurrentMatch), "Observed" & lId & ".txt")
    Next

    'subset of data to simultation span
    lTsMath.DataSets.Clear()
    lArgsMath.Clear()
    lArgsMath.SetValue("Start Date", "1985/10/1")
    lArgsMath.SetValue("End Date", "1988/10/1")
    For Each lCurrentMatch As atcTimeseries In lMatch
      lArgsMath.SetValue("timeseries", New atcDataGroup(lCurrentMatch))
      aDataManager.OpenDataSource(lTsMath, "subset by date", lArgsMath)
    Next
    lSummary.Save(aDataManager, lTsMath.DataSets, "ObservedSubset.txt")

    LogDbg("About to write " & lTsMath.DataSets.Count & " datasets")
    For Each lCurrentTimeseries As atcTimeseries In lTsMath.DataSets
      LogDbg("Write DSN " & lCurrentTimeseries.ToString)
      lWDMfile.AddDataSet(lCurrentTimeseries, ExistReplace)
      'lWDMfile.AddDataSet(lCurrentTimeseries, ExistRenumber)
    Next

    Application.Exit()
  End Sub
End Module
