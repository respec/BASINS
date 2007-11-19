Imports atcUtility
Imports atcData
Imports atcWDM
Imports atcHspfBinOut
Imports HspfSupport
Imports MapWinUtility

Imports MapWindow.Interfaces
Imports System.Collections.Specialized

Module HSPFOutputReports
    Private pTestPath As String
    Private pBaseName As String
    Private pOutputLocations As New atcCollection

    Private Sub Initialize()
        pOutputLocations.Clear()
        'Dim lTestName As String = "hyd_man"
        'Dim lTestName As String = "calleguas_cat"
        'Dim lTestName As String = "calleguas_nocat"
        Dim lTestName As String = "SantaClara"
        Select Case lTestName
            Case "calleguas_cat"
                pTestPath = "D:\MountainViewData\Calleguas\cat"
                pBaseName = "Calleg"
                pOutputLocations.Add("R:408")
                pOutputLocations.Add("R:10")
                pOutputLocations.Add("R:307")
            Case "calleguas_nocat"
                pTestPath = "D:\MountainViewData\Calleguas\nocat"
                pBaseName = "Calleg"
                pOutputLocations.Add("R:408")
                pOutputLocations.Add("R:10")
                pOutputLocations.Add("R:307")
            Case "hyd_man"
                pTestPath = "C:\test\EXP_CAL\hyd_man.net_cat"
                pBaseName = "hyd_man"
                pOutputLocations.Add("R:5")
                pOutputLocations.Add("R:4")
            Case "SantaClara"
                pTestPath = "D:\MountainViewData\SantaClara"
                pBaseName = "SCR10"
                pOutputLocations.Add("R:70")
                pOutputLocations.Add("R:180")
                pOutputLocations.Add("R:526")
                pOutputLocations.Add("R:410")
                pOutputLocations.Add("R:880")
        End Select
    End Sub

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        Initialize()
        ChDriveDir(pTestPath)

        'open uci file
        Dim lMsg As New atcUCI.HspfMsg
        lMsg.Open("hspfmsg.mdb")
        Dim lHspfUci As New atcUCI.HspfUci
        lHspfUci.FastReadUciForStarter(lMsg, pBaseName & ".uci")

        'open WDM file
        Dim lWdmFileName As String = pTestPath & "\" & pBaseName & ".wdm"
        Dim lWdmDataSource As New atcDataSourceWDM()
        lWdmDataSource.Open(lWdmFileName)

        Dim lOutFileName As String
        Dim lExpertSystemFileNames As New NameValueCollection
        AddFilesInDir(lExpertSystemFileNames, CurDir, False, "*.exs")
        Dim lExpertSystem As HspfSupport.ExpertSystem
        For Each lExpertSystemFileName As String In lExpertSystemFileNames
            Try
                Dim lFileCopied As Boolean = False
                If FilenameOnly(lExpertSystemFileName) <> pBaseName Then
                    FileCopy(lExpertSystemFileName, pBaseName & ".exs")
                    lFileCopied = True
                End If
                lExpertSystem = New HspfSupport.ExpertSystem(lHspfUci, lWdmDataSource)
                Dim lStr As String = lExpertSystem.Report
                SaveFileString("outfiles\ExpertSysStats-" & FilenameOnly(lExpertSystemFileName) & ".txt", lStr)

                'lStr = lExpertSystem.AsString 'NOTE:just testing
                'SaveFileString(FilenameOnly(lHspfUci.Name) & ".exx", lStr)

                Dim lCons As String = "Flow"
                For lSiteIndex As Integer = 1 To lExpertSystem.Sites.Count
                    Dim lSite As String = lExpertSystem.Sites(lSiteIndex).Name
                    Dim lArea As Double = lExpertSystem.Sites(lSiteIndex).Area
                    Dim lSimDsnId As Integer = lExpertSystem.Sites(lSiteIndex).Dsn(0)
                    Dim lObsDsnId As Integer = lExpertSystem.Sites(lSiteIndex).Dsn(1)
                    lStr = HspfSupport.DailyMonthlyCompareStats.Report(lHspfUci, lWdmDataSource, _
                                                                       lCons, lSite, lArea, _
                                                                       lSimDsnId, lObsDsnId, _
                                                                       lExpertSystem.SDateJ, _
                                                                       lExpertSystem.EDateJ)
                    lOutFileName = "outfiles\DailyMonthly" & lCons & "Stats" & "-" & lSite & ".txt"
                    SaveFileString(lOutFileName, lStr)
                Next lSiteIndex
                lExpertSystem = Nothing
                If lFileCopied Then
                    Kill(pBaseName & ".exs")
                End If
            Catch lEx As ApplicationException
                Logger.Dbg(lEx.Message)
            End Try
        Next lExpertSystemFileName

        'open HBN file
        Dim lHspfBinFileName As String = pTestPath & "\" & pBaseName & ".hbn"
        Dim lHspfBinDataSource As New atcTimeseriesFileHspfBinOut()
        lHspfBinDataSource.Open(lHspfBinFileName)

        Dim lSummaryType As String = "Water"

        'watershed summary
        Dim lHspfBinFileInfo As System.IO.FileInfo = New System.IO.FileInfo(lHspfBinFileName)
        Dim lString As Text.StringBuilder = HspfSupport.WatershedSummary.Report(lHspfUci, lHspfBinDataSource, lHspfBinFileInfo.LastWriteTime, lSummaryType)
        lOutFileName = "outfiles\" & lSummaryType & "_" & "WatershedSummary.txt"
        SaveFileString(lOutFileName, lString.ToString)
        lString = Nothing

        'build collection of operation types to report
        Dim lOperationTypes As New atcCollection
        lOperationTypes.Add("P:", "PERLND")
        lOperationTypes.Add("I:", "IMPLND")
        lOperationTypes.Add("R:", "RCHRES")
        Dim lLocations As atcCollection = lHspfBinDataSource.DataSets.SortedAttributeValues("Location")

        'constituent balance
        lString = HspfSupport.ConstituentBalance.Report _
           (lHspfUci, lSummaryType, lOperationTypes, pBaseName, _
            lHspfBinDataSource, lLocations, lHspfBinFileInfo.LastWriteTime)
        lOutFileName = "outfiles\" & lSummaryType & "_" & "ConstituentBalance.txt"
        SaveFileString(lOutFileName, lString.ToString)

        'watershed constituent balance 
        lString = HspfSupport.WatershedConstituentBalance.Report _
           (lHspfUci, lSummaryType, lOperationTypes, pBaseName, _
            lHspfBinDataSource, lHspfBinFileInfo.LastWriteTime)
        lOutFileName = "outfiles\" & lSummaryType & "_" & "WatershedConstituentBalance.txt"
        SaveFileString(lOutFileName, lString.ToString)

        If pOutputLocations.Count > 0 Then 'subwatershed constituent balance 
            HspfSupport.WatershedConstituentBalance.ReportsToFiles _
               (lHspfUci, lSummaryType, lOperationTypes, pBaseName, _
                lHspfBinDataSource, pOutputLocations, lHspfBinFileInfo.LastWriteTime, _
                "outfiles\")
        End If
    End Sub
End Module
