Imports atcUtility
Imports atcData
Imports atcData.atcDataGroup
Imports atcList
Imports atcHspfBinOut
Imports MapWindow.Interfaces
Imports MapWinUtility
Imports System

Module ListTimeseries
    Private Const pTestPath As String = "D:\mono_70"
    Private Const pBaseName As String = "base"

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        ChDriveDir(pTestPath)
        'open BinaryOutputFile file
        Dim lHbnFileName As String = pTestPath & "\" & pBaseName & ".hbn"
        Dim lHbnDataSource As New atcTimeseriesFileHspfBinOut
        lHbnDataSource.Open(lHbnFileName)
        Dim lDataSource As atcDataSource = lHbnDataSource
        'locations to List
        Logger.Dbg("BeginLocationSelection")
        Dim lLocationData As atcDataGroup = lDataSource.DataSets.FindData("Location", "R:9")
        'constituents to List
        Logger.Dbg("BeginConstituentSelection")
        Dim lConstituents As New atcCollection
        lConstituents.Add("RO")
        lConstituents.Add("ROSED-TOT")
        lConstituents.Add("N-TOT-OUT")
        Dim lSelectedData As atcDataGroup = lLocationData.FindData("Constituent", lConstituents)
        'aggregate data to annual
        Logger.Dbg("BeginAnnualAggregation")
        Dim lAnnualData As New atcDataGroup
        For Each lDataSet As atcTimeseries In lSelectedData
            Dim lTran As atcTran = atcTran.TranSumDiv
            If lDataSet.Attributes.GetDefinedValue("Constituent").Value.ToString.ToLower = "ro" Then
                lTran = atcTran.TranAverSame
            End If
            lAnnualData.Add(Aggregate(lDataSet, atcTimeUnit.TUYear, 1, lTran))
        Next
        Logger.Dbg("BeginListing")
        Dim lList As New atcListPlugin
        lList.Save(lAnnualData, pBaseName & "Output.txt")
        Logger.Dbg("CompleteListing")
    End Sub
End Module
