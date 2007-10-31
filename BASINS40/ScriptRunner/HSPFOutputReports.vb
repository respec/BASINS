Imports atcUtility
Imports atcData
Imports atcWDM
Imports atcHspfBinOut
Imports HspfSupport
Imports MapWinUtility

Imports MapWindow.Interfaces

Module HSPFOutputReports


    Private Const pTestPath As String = "D:\MountainViewData\Calleguas\"
    Private Const pBaseName As String = "Calleg"
    'Private Const pTestPath As String = "C:\test\EXP_CAL\hyd_man.net"
    'Private Const pBaseName As String = "hyd_man"

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

        Dim lOutFileName As String
        Try
            Dim lExpertSystem As HspfSupport.ExpertSystem
            lExpertSystem = New HspfSupport.ExpertSystem(lHspfUci, lWdmDataSource)
            Dim lStr As String = lExpertSystem.Report
            SaveFileString("outfiles\ExpertSysStats.txt", lStr)

            lStr = lExpertSystem.AsString 'NOTE:just testing
            SaveFileString(FilenameOnly(lHspfUci.Name) & ".exx", lStr)

            Dim lCons As String = "Flow"
            For lSiteIndex As Integer = 1 To lExpertSystem.Sites.Count
                Dim lSite As String = lExpertSystem.Sites(lSiteIndex).Name
                Dim lArea As Double = lExpertSystem.Sites(lSiteIndex).Area
                Dim lSimDsnId As Integer = lExpertSystem.Sites(lSiteIndex).Dsn(0)
                Dim lObsDsnId As Integer = lExpertSystem.Sites(lSiteIndex).Dsn(1)
                lStr = HspfSupport.DailyMonthlyCompareStats.Report(lHspfUci, lWdmDataSource, lCons, lSite, lArea, lSimDsnId, lObsDsnId)
                lOutFileName = "outfiles\DailyMonthly" & lCons & "Stats" & "-" & lSite & ".txt"
                SaveFileString(lOutFileName, lStr)
            Next lSiteIndex
        Catch lEx As ApplicationException
            Logger.Dbg(lEx.Message)
        End Try

        'open HBN file
        Dim lHspfBinFileName As String = pTestPath & "\" & pBaseName & ".hbn"
        Dim lHspfBinDataSource As New atcTimeseriesFileHspfBinOut
        lHspfBinDataSource.Open(lHspfBinFileName)
        Dim lSummaryType As String = "Water"
        Dim lHspfBinFileInfo As System.IO.FileInfo = New System.IO.FileInfo(lHspfBinFileName)
        Dim lString As Text.StringBuilder = HspfSupport.WatershedSummary.Report(lHspfUci, lHspfBinDataSource, lHspfBinFileInfo.LastWriteTime, lSummaryType)
        lOutFileName = "outfiles\" & lSummaryType & "_" & "WatershedSummary.txt"
        SaveFileString(lOutFileName, lString.ToString)
        lString = Nothing

        'build collection of operation types to report
        Dim lOperations As New atcCollection
        lOperations.Add("P:", "PERLND")
        lOperations.Add("I:", "IMPLND")
        lOperations.Add("R:", "RCHRES")
        Dim lLocations As atcCollection = lHspfBinDataSource.DataSets.SortedAttributeValues("Location")

        lString = HspfSupport.ConstituentBalance.Report(lHspfUci, lSummaryType, lOperations, pBaseName, _
                                            lHspfBinDataSource, lLocations, lHspfBinFileInfo.LastWriteTime)
        lOutFileName = "outfiles\" & lSummaryType & "_" & "ConstituentBalance.txt"
        SaveFileString(lOutFileName, lString.ToString)

    End Sub
End Module
