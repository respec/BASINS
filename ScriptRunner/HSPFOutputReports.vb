Imports atcUtility
Imports atcData
Imports atcWDM
Imports atcHspfBinOut
Imports MapWindow.Interfaces

Module HSPFOutputReports
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

        Dim lStr As String
        lStr = ExpertSystemStatistics.Report(lHspfUci, lWdmDataSource)
        SaveFileString("outfiles\ExpertSysStats.txt", lStr)

        'TODO: get the following four parms from the exs file
        Dim lCons As String = "Flow"
        Dim lSites() As String = {"RCH5"}
        Dim lArea() As Double = {54831}
        Dim lSimDsnId() As Integer = {1001}
        Dim lObsDsnId() As Integer = {261}
        lStr = DailyMonthlyCompareStats.Report(lHspfUci, lWdmDataSource, lCons, lSites, lArea, lSimDsnId, lObsDsnId)
        Dim lOutFileName As String = "outfiles\DailyMonthly" & lCons & "Stats"
        If lSites.GetUpperBound(0) = 0 Then lOutFileName &= "-" & lSites(0)
        lOutFileName &= ".txt"
        SaveFileString(lOutFileName, lStr)

        'open HBN file
        Dim lHspfBinFileName As String = pTestPath & "\" & pBaseName & ".hbn"
        Dim lHspfBinDataSource As New atcTimeseriesFileHspfBinOut
        lHspfBinDataSource.Open(lHspfBinFileName)
        Dim lSummaryType As String = "Water"
        Dim lHspfBinFileInfo As System.IO.FileInfo = New System.IO.FileInfo(lHspfBinFileName)
        Dim lString As Text.StringBuilder = WatershedSummary.Report(lHspfUci, lHspfBinDataSource, lHspfBinFileInfo.LastWriteTime, lSummaryType)
        lOutFileName = "outfiles\" & lSummaryType & "_" & "WatershedSummary.txt"
        SaveFileString(lOutFileName, lString.ToString)
        lString = Nothing

        'build collection of operation types to report
        Dim lOperations As New atcCollection
        lOperations.Add("P:", "PERLND")
        lOperations.Add("I:", "IMPLND")
        lOperations.Add("R:", "RCHRES")
        Dim lLocations As atcCollection = lHspfBinDataSource.DataSets.SortedAttributeValues("Location")

        lString = ConstituentBalance.Report(lHspfUci, lSummaryType, lOperations, pBaseName, _
                                            lHspfBinDataSource, lLocations, lHspfBinFileInfo.LastWriteTime)
        lOutFileName = "outfiles\" & lSummaryType & "_" & "ConstituentBalance.txt"
        SaveFileString(lOutFileName, lString.ToString)

    End Sub
End Module
