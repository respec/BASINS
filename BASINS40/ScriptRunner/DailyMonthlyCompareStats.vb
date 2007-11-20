Imports atcUtility
Imports atcData
Imports atcWDM
Imports HspfSupport.Utility
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
        'open expert system
        Dim lExpertSystem As HspfSupport.ExpertSystem
        lExpertSystem = New HspfSupport.ExpertSystem(lHspfUci, lWdmDataSource)
        Dim lCons As String = "Flow"
        For lSiteIndex As Integer = 1 To lExpertSystem.Sites.Count
            Dim lSite As String = lExpertSystem.Sites(lSiteIndex).Name
            Dim lArea As Double = lExpertSystem.Sites(lSiteIndex).Area
            Dim lSimTser As atcTimeseries = lWdmDataSource.DataSets.ItemByKey(lExpertSystem.Sites(lSiteIndex).Dsn(0))
            lSimTser = InchesToCfs(lSimTser, lArea)
            lSimTser = SubsetByDate(lSimTser, _
                                    lExpertSystem.SDateJ, _
                                    lExpertSystem.EDateJ, Nothing)
            Dim lObsTser As atcTimeseries = lWdmDataSource.DataSets.ItemByKey(lExpertSystem.Sites(lSiteIndex).Dsn(1))
            lObsTser = SubsetByDate(lObsTser, _
                                    lExpertSystem.SDateJ, _
                                    lExpertSystem.EDateJ, Nothing)
            Dim lStr As String = HspfSupport.DailyMonthlyCompareStats.Report(lHspfUci, lCons, lSite, _
                                                                             lSimTser, lObsTser)
            Dim lOutFileName As String = "outfiles\DailyMonthly" & lCons & "Stats" & "-" & lSite & ".txt"
            SaveFileString(lOutFileName, lStr)
        Next lSiteIndex
    End Sub
End Module
