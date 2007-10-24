Imports atcData
Imports atcData.atcDataGroup
Imports atcUtility
Imports atcUCI
Imports MapWindow.Interfaces
Imports MapWinUtility

Public Module WatershedSummary
    Private Const pTestPath As String = "C:\test\ScriptReports\"

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)

        'For each UCI file in the specified (pTestPath) folder, 
        'do a Watershed Summary (Loading) Report for the specified constituents;
        'ie total n summary

        'Requirements:
        'UCI and HBN must reside in the same folder,
        'base name of UCI = base name of HBN file

        'Output:
        'writes to the pTestPath folder, for each UCI and constituent,
        'a file named as follows:
        'UCI base name & "_" & constituent name & "_" & "WatershedSummary.txt"

        Logger.Dbg("Start")

        'change to the directory of the current project
        ChDriveDir(pTestPath)
        Logger.Dbg(" CurDir:" & CurDir())

        'build collection of constituents to report
        Dim lConstituents As New atcCollection
        lConstituents.Add("Water")
        lConstituents.Add("BOD")
        lConstituents.Add("DO")
        'lConstituents.Add("FColi")
        'lConstituents.Add("Lead")
        'lConstituents.Add("NH3")
        lConstituents.Add("NH4")
        lConstituents.Add("NO3")
        lConstituents.Add("OrganicN")
        lConstituents.Add("OrganicP")
        lConstituents.Add("PO4")
        lConstituents.Add("Sediment")
        lConstituents.Add("TotalN")
        lConstituents.Add("TotalP")
        'lConstituents.Add("WaterTemp")
        'lConstituents.Add("Zinc")

        'build collection of scenarios (uci base names) to report
        Dim lUcis As New System.Collections.Specialized.NameValueCollection
        AddFilesInDir(lUcis, pTestPath, False, "*.uci")
        Dim lScenarios As New atcCollection
        For Each lUci As String In lUcis
            lScenarios.Add(FilenameNoPath(FilenameNoExt(lUci)))
        Next

        'loop thru each scenario (uci name)
        For Each lScenario As String In lScenarios
            'open the corresponding hbn file
            Dim lHspfBinFile As atcDataSource = New atcHspfBinOut.atcTimeseriesFileHspfBinOut
            Dim lHspfBinFileName As String = lScenario & ".hbn"
            Logger.Dbg(" AboutToOpen " & lHspfBinFileName)
            If Not FileExists(lHspfBinFileName) Then
                'if hbn doesnt exist, make a guess at what the name might be
                lHspfBinFileName = lHspfBinFileName.Replace(".hbn", ".base.hbn")
                Logger.Dbg("  NameUpdated " & lHspfBinFileName)
            End If
            atcDataManager.OpenDataSource(lHspfBinFile, lHspfBinFileName, Nothing)
            Logger.Dbg(" DataSetCount " & lHspfBinFile.DataSets.Count)

            'make uci available 
            Dim lMsg As New atcUCI.HspfMsg
            lMsg.Open("hspfmsg.mdb")
            Dim lHspfUci As New atcUCI.HspfUci
            lHspfUci.FastReadUciForStarter(lMsg, lScenario & ".uci")

            'call main watershed summary routine
            Dim lHspfBinFileInfo As System.IO.FileInfo = New System.IO.FileInfo(lHspfBinFileName)
            HspfSupport.WatershedSummary.ReportsToFiles(lConstituents, lHspfUci, lHspfBinFile, lHspfBinFileInfo.LastWriteTime)

            'clean up 
            atcDataManager.DataSources.Remove(lHspfBinFile)
            lHspfBinFile.DataSets.Clear()
            lHspfBinFile = Nothing
        Next
    End Sub
End Module
