Imports atcUtility
Imports atcData
Imports atcWDM
Imports MapWindow.Interfaces

Module DailyMonthlyCompareStats
    Private Const pTestPath As String = "C:\test\EXP_CAL\hyd_man.net"
    Private Const pBaseName As String = "hyd_man"

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        ChDriveDir(pTestPath)
        'open uci file
        Dim lMsg As New atcUCI.HspfMsg
        lMsg.Open("hspfmsg.mdb")
        Dim lHspfUci As New atcUCI.HspfUci
        lHspfUci.FastReadUciForStarter(lMsg, pBaseName & ".uci")

        'open WDM file
        Dim lWdmFileName As String = pTestPath & "\" & pBaseName & ".wdm"
        Dim lWdmDataSource As New atcDataSourceWDM
        lWdmDataSource.Open(lWdmFileName)
        'TODO: get the following four parms from the exs file
        Dim lCons As String = "Flow"
        Dim lSite As String = "RCH5"
        Dim lArea As Double = 54831
        Dim lSimTSer As atcTimeseries = lWdmDataSource.DataSets.ItemByKey(1001)
        Dim lObsTSer As atcTimeseries = lWdmDataSource.DataSets.ItemByKey(261)
        Dim lStr As String = HspfSupport.DailyMonthlyCompareStats.Report(lHspfUci, lCons, lSite, _
                                                                         lArea, lSimTSer, lObsTSer)
        Dim lOutFileName As String = "outfiles\DailyMonthly" & lCons & "Stats" & "-" & lSite & ".txt"
        SaveFileString(lOutFileName, lStr)
    End Sub
End Module
