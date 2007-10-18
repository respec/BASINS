Imports atcUtility.modFile
Imports atcData
Imports atcWDM
Imports MapWindow.Interfaces

Module HSPFOutputReports
    Private Const pTestPath As String = "C:\test\EXP_CAL\hyd_man.net"
    Private Const pBaseName As String = "hyd_man"

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        ChDriveDir(pTestPath)

        Dim lMsg As New atcUCI.HspfMsg
        lMsg.Open("hspfmsg.mdb")
        Dim lHspfUci As New atcUCI.HspfUci
        lHspfUci.FastReadUciForStarter(lMsg, pBaseName & ".uci")

        'open WDM file
        Dim lWdmFileName As String = pTestPath & "\" & pBaseName & ".wdm"
        Dim lDataSource As New atcDataSourceWDM
        lDataSource.Open(lWdmFileName)

        Dim lStr As String
        lStr = ExpertSystemStatistics.Report(lHspfUci, lDataSource)
        SaveFileString("outfiles\ExpertSysStats.txt", lStr)

        Dim lSites() As String = {"RCH5"}
        lStr = DailyMonthlyFlowStats.Report(lHspfUci, lDataSource, lSites)
        SaveFileString("outfiles\DailyMontlyFlowStats.txt", lStr)
    End Sub
End Module
