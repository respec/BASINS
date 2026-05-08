Option Strict Off
Option Explicit On

Imports atcUtility
Imports atcData
Imports atcGraph
Imports HspfSupport
Imports atcHspfBinOut
Imports atcUCI
Imports MapWinUtility 'for Logger
Imports MapWinUtility.Strings
Imports System.IO

Module modHSPEXP

    ''' <summary>
    ''' Entry point for HSPEXP+ command line arguments
    ''' </summary>
    ''' <remarks></remarks>
    Public Function CommandLine() As Integer

        Dim lTempCL As String = Environment.CommandLine
        Dim lExeName As String = StrSplit(lTempCL, " ", """")

        'lTempCL = "/QAQC C:\temp\ian\RedLake_222\RedLake_222.uci"  'for testing

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

                If StringFindAndRemove(lTempCL, "/QAQC") Then
                    'do QAQC report from command line

                    Dim lOutFolder As String = PathNameOnly(lTempCL)
                    ChDriveDir(lOutFolder)
                    Dim lUciName As String = lTempCL

                    If IO.File.Exists(lUciName) Then
                        Dim lUci As New HspfUci
                        Dim lHspfMsg = New HspfMsg

                        Dim lWinHspfLtDir As String = PathNameOnly(Reflection.Assembly.GetEntryAssembly.Location) & g_PathChar & "WinHSPFLt"
                        atcWDM.atcDataSourceWDM.HSPFMsgFilename = IO.Path.Combine(lWinHspfLtDir, "hspfmsg.wdm")
                        lHspfMsg.Open(atcWDM.atcDataSourceWDM.HSPFMsgFilename)
                        lUci.FastReadUciForStarter(lHspfMsg, lUciName)

                        atcData.atcDataManager.Clear()
                        With atcData.atcDataManager.DataPlugins
                            .Add(New atcHspfBinOut.atcTimeseriesFileHspfBinOut)
                            .Add(New atcBasinsObsWQ.atcDataSourceBasinsObsWQ)
                            .Add(New atcWDM.atcDataSourceWDM)
                            .Add(New atcTimeseriesWaterQualUS.atcTimeseriesWaterQualUS)
                            .Add(New atcGraph.atcGraphPlugin)
                        End With

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
                        'get echo file name from files block
                        Dim lHspfEchoFileName As String = ""
                        For i As Integer = 0 To lUci.FilesBlock.Count
                            If lUci.FilesBlock.Value(i).Typ = "MESSU" Then
                                lHspfEchoFileName = AbsolutePath(lUci.FilesBlock.Value(i).Name.Trim, CurDir()) 'Update echo file name if it is referenced in the Files block
                                Exit For
                            End If
                        Next

                        Dim lQAQCReportFile As New Text.StringBuilder
                        Logger.Status("Beginning the QAQC Report")
                        lQAQCReportFile.AppendLine("<html>")
                        lQAQCReportFile.AppendLine(QAReportStyle())
                        lQAQCReportFile.AppendLine("<body>")
                        Dim lRunMade As String = CheckEchoFile(lHspfEchoFileName)
                        lQAQCReportFile.AppendLine(QAGeneralModelInfo(lUci, lRunMade))
                        lQAQCReportFile.AppendLine(QAModelAreaReport(lUci, lOperationTypes))
                        'lQAQCReportFile.AppendLine(QACheckHSPFParmValues(lUci, lRunMade))
                        lQAQCReportFile.AppendLine(QACheckDiurnalPattern(lUci, "DO"))
                        lQAQCReportFile.AppendLine(QACheckDiurnalPattern(lUci, "Water Temperature"))
                        'If pConstituents.Count > 0 Then  'consider adding constituents to command line
                        '    DoWaterQualityReports(lUci, lRunMade, lDateString, lOperationTypes, lQAQCReportFile)
                        'End If
                        Logger.Status("Closing the QAQC Report")
                        lQAQCReportFile.AppendLine("</body>")
                        lQAQCReportFile.AppendLine("</html>")
                        File.WriteAllText(lOutFolder & "\ModelQAQC.htm", lQAQCReportFile.ToString())
                        OpenFile(lOutFolder & "\ModelQAQC.htm")
                    End If
                End If

                'close monitor if unhandled command line instruction
                pStatusMonitor.StopMonitor()
                pStatusMonitor = Nothing
                End 'force exit if command line handled instructions

            Catch ex As Exception
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
End Module

''' <summary>
''' Sends messages to VB6 Status Monitor. 
''' Passes messages received from Status Monitor to file handle pPipeReadFromStatus
''' </summary>
''' <remarks></remarks>
'Friend Class StatusMonitor
'    Implements MapWinUtility.IProgressStatus

'    Dim pInit As Boolean = False
'    Dim pMonitorProcess As Process

'    Public Sub Progress(ByVal aCurrentPosition As Integer, ByVal aLastPosition As Integer) Implements MapWinUtility.IProgressStatus.Progress
'        WriteStatus("PROGRESS " & aCurrentPosition & " of " & aLastPosition)
'    End Sub

'    Public Sub Status(ByVal aStatusMessage As String) Implements MapWinUtility.IProgressStatus.Status
'        If Not pInit Then
'            Try
'                Dim lProcessId As Integer = Process.GetCurrentProcess.Id
'                pMonitorProcess = New Process
'                With pMonitorProcess.StartInfo
'                    .FileName = FindFile("Status Monitor", "statusMonitor.exe")
'                    .Arguments = lProcessId
'                    .CreateNoWindow = True
'                    .UseShellExecute = False
'                    .RedirectStandardInput = True
'                    .RedirectStandardOutput = True
'                    'AddHandler pMonitorProcess.OutputDataReceived, AddressOf MonitorMessageHandler
'                    .RedirectStandardError = True
'                    'AddHandler pMonitorProcess.ErrorDataReceived, AddressOf MonitorMessageHandler
'                End With
'                pMonitorProcess.Start()
'                '
'                'NOTE: to debug pMonitorProcess, in VS2005 (not Express) - choose Tools:AttachToProcess - StatusMonitor
'                '
'                'pMonitorProcess.StandardInput.WriteLine("Show")
'                'pMonitorProcess.BeginErrorReadLine()
'                'pMonitorProcess.BeginOutputReadLine()
'                Logger.Dbg("MonitorLaunched")
'                Dim lStreamMonitorInputFromMyOutput As IO.FileStream = pMonitorProcess.StandardInput.BaseStream
'                pPipeWriteToStatus = lStreamMonitorInputFromMyOutput.SafeFileHandle.DangerousGetHandle
'                Dim lStreamMonitorOutputToMyInput As IO.FileStream = pMonitorProcess.StandardOutput.BaseStream
'                pPipeReadFromStatus = lStreamMonitorOutputToMyInput.SafeFileHandle.DangerousGetHandle
'            Catch ex As Exception
'                Logger.Msg("StatusProcessStartError:" & ex.Message)
'            End Try
'            pInit = True
'        End If

'        WriteStatus(aStatusMessage)

'        If aStatusMessage.ToLower = "exit" Then
'            If Not pMonitorProcess.HasExited Then
'                pMonitorProcess.StandardInput.WriteLine("Exit")
'            End If
'        End If
'    End Sub

'Private Function WriteStatus(ByVal aMsg As String) As Boolean
'        If Not IsNothing(pMonitorProcess) Then
'            If pMonitorProcess.HasExited Then
'                If pMonitorProcess.ExitCode <> &H103S Then 'TODO: check to be sure codes have not changed
'                    Return False  'Process at other end of pipe is dead, stop talking to it
'                End If
'            End If
'        End If

'        If aMsg.StartsWith("(") AndAlso aMsg.EndsWith(")") Then
'            aMsg = aMsg.Substring(1, aMsg.Length - 2)
'        End If

'        If aMsg.Length > 0 Then
'            Dim OpenParenEscape As String = Chr(6)
'            aMsg = aMsg.Replace("(", OpenParenEscape)
'            Dim CloseParenEscape As String = Chr(7)
'            aMsg = aMsg.Replace(")", CloseParenEscape)
'            If Asc(Right(aMsg, 1)) > 31 Then
'                aMsg = "(" & aMsg & ")"
'            End If
'            Logger.Dbg(aMsg)
'            pMonitorProcess.StandardInput.WriteLine(aMsg)
'        End If
'        Return True
'    End Function
'End Class