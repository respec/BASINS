Imports MapWindow.Interfaces
Imports MapWinUtility

Imports atcUtility
Imports atcData

Module SummarizeTimeseriesMultiple
    Private pBaseFolder As String = "D:\Mono_70\"

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        ChDriveDir(pBaseFolder)
        Logger.Dbg("SummarizeTimeseriesMultipleStart:CurDir:" & CurDir())

        Dim lSeasonNames As New ArrayList
        lSeasonNames.Add("DJF")
        lSeasonNames.Add("MAM")
        lSeasonNames.Add("JJA")
        lSeasonNames.Add("SON")

        Dim lSeasonalAttributeNames As New ArrayList
        lSeasonalAttributeNames.Add("Mean")
        lSeasonalAttributeNames.Add("SumAnnual")
        lSeasonalAttributeNames.Add("Standard Deviation")

        Dim lAttributes As New atcCollection
        With lAttributes
            .Add("Data Source", True)
            .Add("ID", True)
            .Add("Location", True)
            .Add("Constituent", True)
            .Add("Start Date", True)
            .Add("End Date", True)
            .Add("Count", True)
            .Add("Mean", True)
            .Add("Standard Deviation", True)
            .Add("SumAnnual", True)
            .Add("Geometric Mean", True)
            .Add("Minimum", True)
            .Add("Maximum", True)
        End With

        Dim lConstituents As New ArrayList
        lConstituents.Add("HPRC")
        lConstituents.Add("ATMP")
        lConstituents.Add("EVAP")

        Dim lWdmNames As New System.Collections.Specialized.NameValueCollection
        Dim lBaseWdmName As String = pBaseFolder & "base.wdm"
        lWdmNames.Add(lBaseWdmName.ToLower, lBaseWdmName)
        AddFilesInDir(lWdmNames, pBaseFolder, False, "*_m.base.wdm")

        Dim lStringBuilder As New Text.StringBuilder
        Dim lInit As Boolean = True
        For Each lWdmName As String In lWdmNames
            Dim lWdm As New atcWDM.atcDataSourceWDM
            lWdm.Open(lWdmName)
            lStringBuilder.Append(SummarizeTimeseries(lAttributes, lWdm.DataSets, lInit, _
                                  lSeasonalAttributeNames, lSeasonNames, lConstituents))
            lInit = False
            lWdm.Clear()
            lWdm = Nothing
        Next

        SaveFileString("SummarizeTimeseries.txt", lStringBuilder.ToString)
        Logger.Dbg("SummarizeTimeseriesMultipleDone")
    End Sub
End Module
