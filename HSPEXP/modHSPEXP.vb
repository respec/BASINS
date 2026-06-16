Option Strict Off
Option Explicit On

Imports atcUtility
Imports atcData
Imports atcGraph
Imports HspfSupport
Imports atcHspfBinOut
Imports atcTimeseriesRDB
Imports atcUCI
Imports MapWinUtility 'for Logger
Imports MapWinUtility.Strings
Imports System.IO
Imports System.Runtime.InteropServices

Public Class clsCLI
    <DllImport("kernel32.dll", SetLastError:=True)>
    Friend Shared Function AttachConsole(dwProcessId As Integer) As Boolean
    End Function

    <DllImport("kernel32.dll", SetLastError:=True)>
    Friend Shared Function FreeConsole() As Boolean
    End Function
End Class

Module modHSPEXP

    ''' <summary>
    ''' Entry point for HSPEXP+ command line arguments
    ''' </summary>
    ''' <remarks></remarks>

    Public Function CommandLine() As Integer

        Dim lTempCL As String = Environment.CommandLine
        Dim lExeName As String = StrSplit(lTempCL, " ", """")

        'lTempCL = "-h"
        'lTempCL = "/QAQC C:\temp\ian\RedLake_222\RedLake_222.uci"  'for testing
        'lTempCL = "/QAQC C:\Talon\HSPF_Model\v10RSAftables\TalonRSA.uci"  'for testing
        'lTempCL = "/IMPORT 'C:\USGSDocs\TO2\json\NWIS_discharge_02225500.rdb' 'C:\USGSDocs\TO2\json\temp.wdm' 500"
        'lTempCL = "/IMPORT 'C:\USGSDocs\TO2\json\discharge_02225500.csv' 'C:\USGSDocs\TO2\json\temp.wdm' 500"
        'lTempCL = "/HYDRO 'C:\LAN-Cibolo\CiboloU125\CiboloU125.uci'"
        'lTempCL = "/STATS 'C:\LAN-Cibolo\CiboloU125\CiboloU125.uci'"

        If Len(lTempCL) > 0 Then
            'If Logger.ProgressStatus Is Nothing OrElse Not (TypeOf (Logger.ProgressStatus) Is MonitorProgressStatus) Then
            'Start running status monitor to give better progress and status indication during long-running processes
            Dim pStatusMonitor As New MonitorProgressStatus
            If pStatusMonitor.StartMonitor(FindFile("Find Status Monitor", "StatusMonitor.exe"),
                                                IO.Directory.GetCurrentDirectory,
                                                System.Diagnostics.Process.GetCurrentProcess.Id) Then
                'put our status monitor (StatusMonitor.exe) between the Logger and the default MW status monitor
                pStatusMonitor.InnerProgressStatus = Logger.ProgressStatus
                Logger.ProgressStatus = pStatusMonitor
                Logger.Status("LABEL TITLE HSPEXP+")
                Logger.Status("PROGRESS TIME OFF") 'Disable time-to-completion estimation
                Logger.Status("")
            Else
                pStatusMonitor.StopMonitor()
                pStatusMonitor = Nothing
            End If

            Try

                If lTempCL = "-h" Or lTempCL = "--help" Then
                    clsCLI.AttachConsole(-1)
                    Console.WriteLine(" ")
                    Console.WriteLine("usage: HSPEXP+.exe /QAQC uciName")
                    Console.WriteLine("usage: HSPEXP+.exe /IMPORT rdbName wdmName newDSN")
                    Console.WriteLine("usage: HSPEXP+.exe /HYDRO uciName")
                    Console.WriteLine("usage: HSPEXP+.exe /STATS uciName")
                    Console.WriteLine(" ")
                    Console.WriteLine("switch /QAQC creates QAQC report for the specified UCI")
                    Console.WriteLine("switch /IMPORT imports USGS rdb/csv file to wdm file with preferred DSN")
                    Console.WriteLine("switch /HYDRO creates hydrology calibration reports and graphs for the specified UCI")
                    Console.WriteLine("switch /STATS computes hydrology calibration statistics for the specified UCI")
                    SendKeys.SendWait("{ENTER}")
                    clsCLI.FreeConsole()

                Else
                    'general scripting setup
                    atcData.atcDataManager.Clear()
                    With atcData.atcDataManager.DataPlugins
                        .Add(New atcHspfBinOut.atcTimeseriesFileHspfBinOut)
                        .Add(New atcBasinsObsWQ.atcDataSourceBasinsObsWQ)
                        .Add(New atcWDM.atcDataSourceWDM)
                        .Add(New atcTimeseriesWaterQualUS.atcTimeseriesWaterQualUS)
                        .Add(New atcGraph.atcGraphPlugin)
                    End With
                    'set up the timeseries attributes for statistics
                    atcTimeseriesStatistics.atcTimeseriesStatistics.InitializeShared()
                    'init graph specs
                    pGraphSaveFormat = ".png"
                    pGraphSaveWidth = 1300
                    pGraphSaveHeight = 768

                    If StringFindAndRemove(lTempCL, "/HYDRO") Then
                        'do hydro calibration report from command line
                        Dim lUciName As String = StrRetRem(lTempCL)
                        Dim lOutFolder As String = PathNameOnly(lUciName)
                        ChDriveDir(lOutFolder)

                        If IO.File.Exists(lUciName) Then
                            Dim lUci As New HspfUci
                            Dim lHspfMsg = New HspfMsg
                            Dim lWinHspfLtDir As String = PathNameOnly(Reflection.Assembly.GetEntryAssembly.Location) & g_PathChar & "WinHSPFLt"
                            atcWDM.atcDataSourceWDM.HSPFMsgFilename = IO.Path.Combine(lWinHspfLtDir, "hspfmsg.wdm")
                            lHspfMsg.Open(atcWDM.atcDataSourceWDM.HSPFMsgFilename)
                            lUci.FastReadUciForStarter(lHspfMsg, lUciName)
                            Dim lRunMade As String = DateTimeFolder(lUciName, lUci)
                            DoExpertSystemStats(lUci, lRunMade)
                        End If

                    ElseIf StringFindAndRemove(lTempCL, "/STATS") Then
                        'do expert system stats from command line
                        Dim lUciName As String = StrRetRem(lTempCL)
                        Dim lOutFolder As String = PathNameOnly(lUciName)
                        ChDriveDir(lOutFolder)

                        If IO.File.Exists(lUciName) Then
                            Dim lUci As New HspfUci
                            Dim lHspfMsg = New HspfMsg
                            Dim lWinHspfLtDir As String = PathNameOnly(Reflection.Assembly.GetEntryAssembly.Location) & g_PathChar & "WinHSPFLt"
                            atcWDM.atcDataSourceWDM.HSPFMsgFilename = IO.Path.Combine(lWinHspfLtDir, "hspfmsg.wdm")
                            lHspfMsg.Open(atcWDM.atcDataSourceWDM.HSPFMsgFilename)
                            lUci.FastReadUciForStarter(lHspfMsg, lUciName)
                            Dim lRunMade As String = DateTimeFolder(lUciName, lUci)
                            DoExpertSystemStats(lUci, lRunMade, True)
                        End If

                    ElseIf StringFindAndRemove(lTempCL, "/QAQC") Then
                        'do QAQC report from command line

                        Dim lUciName As String = StrRetRem(lTempCL)
                        Dim lOutFolder As String = PathNameOnly(lUciName)
                        ChDriveDir(lOutFolder)

                        If IO.File.Exists(lUciName) Then
                            Dim lUci As New HspfUci
                            Dim lHspfMsg = New HspfMsg

                            Dim lWinHspfLtDir As String = PathNameOnly(Reflection.Assembly.GetEntryAssembly.Location) & g_PathChar & "WinHSPFLt"
                            atcWDM.atcDataSourceWDM.HSPFMsgFilename = IO.Path.Combine(lWinHspfLtDir, "hspfmsg.wdm")
                            lHspfMsg.Open(atcWDM.atcDataSourceWDM.HSPFMsgFilename)
                            lUci.FastReadUciForStarter(lHspfMsg, lUciName)

                            Dim lOpenHspfBinDataSource As New atcDataSource
                            Logger.Dbg(Now & " Opening the binary output files.")
                            For i As Integer = 0 To lUci.FilesBlock.Count
                                If lUci.FilesBlock.Value(i).Typ = "BINO" Then
                                    Dim lHspfBinFileName As String = AbsolutePath(lUci.FilesBlock.Value(i).Name.Trim, CurDir())
                                    lOpenHspfBinDataSource = atcDataManager.DataSourceBySpecification(lHspfBinFileName)
                                    If lOpenHspfBinDataSource Is Nothing Then
                                        If atcDataManager.OpenDataSource(lHspfBinFileName) Then
                                            lOpenHspfBinDataSource = atcDataManager.DataSourceBySpecification(lHspfBinFileName)
                                        End If
                                    End If
                                End If
                            Next i

                            'build collection of operation types to report
                            Dim lOperationTypes As New atcCollection
                            lOperationTypes.Add("P:", "PERLND")
                            lOperationTypes.Add("I:", "IMPLND")
                            lOperationTypes.Add("R:", "RCHRES")
                            lOperationTypes.Add("B:", "BMPRAC")

                            Dim lRunMade As String = DateTimeFolder(lUciName, lUci)

                            Dim lQAQCReportFile As New Text.StringBuilder
                            Logger.Status("Beginning the QAQC Report")
                            lQAQCReportFile.AppendLine("<html>")
                            lQAQCReportFile.AppendLine(QAReportStyle())
                            lQAQCReportFile.AppendLine("<body>")
                            lQAQCReportFile.AppendLine(QAGeneralModelInfo(lUci, lRunMade))
                            lQAQCReportFile.AppendLine(QAModelAreaReport(lUci, lOperationTypes))
                            lQAQCReportFile.AppendLine(QACheckHSPFParmValues(lUci, lRunMade))
                            lQAQCReportFile.AppendLine(QACheckDiurnalPattern(lUci, "DO"))
                            lQAQCReportFile.AppendLine(QACheckDiurnalPattern(lUci, "Water Temperature"))
                            'If pConstituents.Count > 0 Then  'consider adding constituents to command line
                            '    DoWaterQualityReports(lUci, lRunMade, lDateString, lOperationTypes, lQAQCReportFile)
                            'End If
                            Logger.Status("Closing the QAQC Report")
                            lQAQCReportFile.AppendLine("</body>")
                            lQAQCReportFile.AppendLine("</html>")
                            'Logger.Msg("about to write qaqc")
                            File.WriteAllText(lOutFolder & "\ModelQAQC.htm", lQAQCReportFile.ToString())
                            'Logger.Msg("finished writing qaqc")
                            OpenFile(lOutFolder & "\ModelQAQC.htm")
                        End If

                    ElseIf StringFindAndRemove(lTempCL, "/IMPORT") Then
                        'import specified timeseries file to WDM
                        Dim lInputFile As String = StrRetRem(lTempCL)
                        Dim lOutputFile As String = StrRetRem(lTempCL)
                        Dim lDsn As Integer = Int(lTempCL)
                        If UCase(FileExt(lInputFile)) = "RDB" Then
                            Dim lRDBReader As New atcTimeseriesRDB.atcTimeseriesRDB()
                            Dim lWDMfile As New atcWDM.atcDataSourceWDM
                            If lRDBReader.Open(lInputFile) Then
                                Dim lTS As atcTimeseries = lRDBReader.DataSets(0)
                                lTS.Attributes.SetValue("ID", lDsn)
                                lWDMfile.Open(lOutputFile)
                                lWDMfile.AddDataset(lTS, atcData.atcDataSource.EnumExistAction.ExistRenumber)
                            End If
                        ElseIf UCase(FileExt(lInputFile)) = "CSV" Then
                            Dim lCSVReader As New atcTimeseriesCSV_USGS.atcTimeseriesCSV_USGS
                            Dim lWDMfile As New atcWDM.atcDataSourceWDM
                            If lCSVReader.Open(lInputFile) Then
                                Dim lTS As atcTimeseries = lCSVReader.DataSets(0)
                                lTS.Attributes.SetValue("ID", lDsn)
                                lWDMfile.Open(lOutputFile)
                                lWDMfile.AddDataset(lTS, atcData.atcDataSource.EnumExistAction.ExistRenumber)
                            End If
                        End If

                    End If
                End If

                'close monitor if unhandled command line instruction
                pStatusMonitor.StopMonitor()
                pStatusMonitor = Nothing
                End 'force exit if command line handled instructions

            Catch ex As Exception
                Logger.Msg(ex.ToString)
                pStatusMonitor.StopMonitor()
            End Try
        End If

    End Function

    Friend Function StringFindAndRemove(ByRef aStr As String, ByVal aFindStr As String) As Boolean
        Dim lInd As Integer = InStr(aStr, aFindStr)
        If lInd > 0 Then 'found string, remove it
            aStr = Trim(Left(aStr, lInd - 1) & Mid(aStr, lInd + aFindStr.Length))
            Return True
        Else
            Return False
        End If
    End Function

    Private Function DateTimeFolder(ByRef aUciName As String, ByRef aUci As HspfUci) As String
        'get echo file name from files block
        Dim lHspfEchoFileName As String = ""
        For i As Integer = 0 To aUci.FilesBlock.Count
            If aUci.FilesBlock.Value(i).Typ = "MESSU" Then
                lHspfEchoFileName = AbsolutePath(aUci.FilesBlock.Value(i).Name.Trim, CurDir()) 'Update echo file name if it is referenced in the Files block
                Exit For
            End If
        Next

        'check echo file to be sure the model ran last time
        Dim lRunMade As String = CheckEchoFile(lHspfEchoFileName)
        'craete a folder name that has the basename and the time when the run was made.
        Dim lDateString As String = Format(Year(lRunMade), "00") & Format(Month(lRunMade), "00") &
                    Format(Microsoft.VisualBasic.DateAndTime.Day(lRunMade), "00") & Format(Hour(lRunMade), "00") & Format(Minute(lRunMade), "00")
        pTestPath = aUciName
        Dim lTestName As String = IO.Path.GetFileNameWithoutExtension(aUciName)
        pBaseName = lTestName
        pTestPath = Mid(pTestPath, 1, Len(pTestPath) - Len(pBaseName) - 4)
        pOutFolderName = pTestPath & "Reports_" & lDateString & "\"
        Directory.CreateDirectory(pOutFolderName)
        File.Copy(pTestPath & pBaseName & ".uci", pOutFolderName & pBaseName & ".uci", overwrite:=True)

        Return lRunMade
    End Function

End Module
