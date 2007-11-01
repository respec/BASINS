Imports atcData
Imports atcUtility
Imports HspfSupport
Imports MapWindow.Interfaces
Imports MapWinUtility

Public Module ConstituentBalanceScript
    Private Const pTestPath As String = "C:\test\ScriptReports\"
    Private Const pDebug As Boolean = False

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)

        'For each UCI file in the specified (pTestPath) folder, 
        'do a Constituent Balance Report for the specified constituents;
        'ie water balance, total n balance, etc.

        'Requirements:
        'UCI and HBN must reside in the same folder,
        'base name of UCI = base name of HBN file

        'Output:
        'writes to the pTestPath folder, for each UCI and constituent,
        'a file named as follows:
        'UCI base name & "_" & constituent name & "_" & "Balance.txt"

        Logger.Dbg("Start")

        'change to the directory of the current project
        ChDriveDir(pTestPath)
        Logger.Dbg(" CurDir:" & CurDir())

        'build collection of constituents to report
        Dim lConstituents As New atcCollection
        lConstituents.Add("Water")
        'lConstituents.Add("NfromPQUAL")
        'lConstituents.Add("TotalN")
        'lConstituents.Add("SedimentCopper")  'implemented but not in use
        'lConstituents.Add("TotalP")          'not yet implemented

        'build collection of scenarios (uci base names) to report
        Dim lUcis As New System.Collections.Specialized.NameValueCollection
        AddFilesInDir(lUcis, pTestPath, False, "*.uci")
        Dim lScenarios As New atcCollection
        'lScenarios.Add("USANFRAN")
        For Each lUci As String In lUcis
            lScenarios.Add(FilenameNoPath(FilenameNoExt(lUci)))
        Next

        'build collection of operation types to report
        Dim lOperations As New atcCollection
        lOperations.Add("P:", "PERLND")
        lOperations.Add("I:", "IMPLND")
        lOperations.Add("R:", "RCHRES")

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

            'build collection of locations to report (hspf operations)
            Dim lLocations As atcCollection = atcDataManager.DataSets.SortedAttributeValues("Location")
            Logger.Dbg(" LocationCount " & lLocations.Count)

            'call main constituent balances routine
            Dim lHspfBinFileInfo As System.IO.FileInfo = New System.IO.FileInfo(lHspfBinFileName)
            Dim lMsg As New atcUCI.HspfMsg
            lMsg.Open("hspfmsg.mdb")
            Dim lHspfUci As New atcUCI.HspfUci
            lHspfUci.FastReadUciForStarter(lMsg, lScenario & ".uci")
            ConstituentBalance.ReportsToFiles(lHspfUci, lOperations, lConstituents, lScenario, lHspfBinFile, lLocations, lHspfBinFileInfo.LastWriteTime)

            'clean up 
            atcDataManager.DataSources.Remove(lHspfBinFile)
            lHspfBinFile.DataSets.Clear()
            lHspfBinFile = Nothing
        Next lScenario
    End Sub
End Module
