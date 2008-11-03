Option Strict Off
Option Explicit On

Imports MapWinUtility 'for Logger
Imports atcUtility 'for StrRetRem, StrFirstInt, FindFile, FilenameOnly, FilenameSetExt, WholdFileString, frmFeedBack

''' <summary>
''' Windows wrapper for HSPF 
''' </summary>
''' <remarks></remarks>
Module modWinHSPFLt
    ''' <summary>
    ''' Open FORTRAN unit 99
    ''' </summary>
    ''' <remarks></remarks>
    Declare Sub F90_W99OPN Lib "hass_ent.dll" ()
    ''' <summary>
    ''' Initialize wdm buffers
    ''' </summary>
    ''' <remarks></remarks>
    Declare Sub F90_WDBFIN Lib "hass_ent.dll" ()
    ''' <summary>
    ''' Open a WDM file
    ''' </summary>
    ''' <param name="aRwflg">ReadWrite flag - 0:RW, 1:RO, 2:CreateNew</param>
    ''' <param name="aName">Name of WDM file</param>
    ''' <param name="aUnit">Fortran unit number of WDM file</param>
    ''' <param name="aRetcod">Return code, see WDM programmer's guide for definitions</param>
    ''' <param name="aNameLen">Length of aName</param>
    ''' <remarks></remarks>
    Declare Sub F90_WDBOPNR Lib "hass_ent.dll" _
        (ByRef aRwflg As Integer, _
         ByVal aName As String, _
         ByRef aUnit As Integer, _
         ByRef aRetcod As Integer, _
         ByVal aNameLen As Short)
    Declare Sub F90_SPIPH Lib "hass_ent.dll" _
        (ByRef aHin As Integer, _
         ByRef aHout As Integer)
    Declare Sub F90_PUTOLV Lib "hass_ent.dll" _
        (ByRef aOutLevel As Integer)
    Declare Sub F90_SCNDBG Lib "hass_ent.dll" _
        (ByRef aDbgLevel As Integer)
    Declare Sub F90_ACTSCN Lib "hass_ent.dll" _
        (ByRef aMkfiles As Integer, _
         ByRef aWdmFl As Integer, _
         ByRef aMsgFl As Integer, _
         ByRef aRetcod As Integer, _
         ByVal aScen As String, _
         ByVal aScenLen As Short)
    Declare Sub F90_SIMSCN Lib "hass_ent.dll" _
        (ByRef aRetcod As Integer)

    Friend pPipeWriteToStatus As Integer = 0
    Friend pPipeReadFromStatus As Integer = 0

    ''' <summary>
    ''' Entry point for WinHspfLt
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Main()
        Try
            Dim lErrLogName As String = "WinHspfLt.log"
            Dim lErrLogFlag As Boolean = False

            Dim lExeCmd As String = Environment.CommandLine

            If StringFindAndRemove(lExeCmd, "/log") Then
                Logger.StartToFile(lErrLogName, False, False, True)
                Logger.Dbg("Early Logging On")
                lErrLogFlag = True
            End If

            Logger.Dbg("ExeName '" & StrRetRem(lExeCmd) & "'")
            Logger.Dbg("CommandLineArgs '" & lExeCmd & "'")

            If StringFindAndRemove(lExeCmd, "/batch") Then
                Logger.DisplayMessageBoxes = False
            End If
            Logger.Dbg("AfterBatchCheck: '" & lExeCmd & "' DisplayMessagegBoxes:" & Logger.DisplayMessageBoxes)

            Dim lSendFeedbackAlways As Boolean = False
            If StringFindAndRemove(lExeCmd, "/feedback") Then
                lSendFeedbackAlways = True
            End If
            Logger.Dbg("AfterFeedbackCheck: '" & lExeCmd & "' SendFeedbackAlways:" & lSendFeedbackAlways)

            pPipeReadFromStatus = StrFirstInt(lExeCmd)
            pPipeWriteToStatus = StrFirstInt(lExeCmd)
            Logger.Dbg("AfterPipesFromCommandLine: '" & lExeCmd & "' FromStatus " & pPipeReadFromStatus & " ToStatus " & pPipeWriteToStatus)

            If pPipeWriteToStatus = 0 Or pPipeReadFromStatus = 0 Then
                Logger.ProgressStatus = New StatusMonitor 'MonitorProgressStatus
                Logger.Status("Begin") 'set handles but don't open
            End If

            If pPipeWriteToStatus = -1 Then pPipeWriteToStatus = 0
            If pPipeReadFromStatus = -1 Then pPipeReadFromStatus = 0
            Logger.Dbg("PipeWriteToStatus:" & pPipeWriteToStatus)
            Logger.Dbg("PipeReadFromStatus:" & pPipeReadFromStatus)

            Logger.Dbg("F90_W99OPN") : Call F90_W99OPN() 'open error file for fortan problems
            Logger.Dbg("F90_WDBFIN") : Call F90_WDBFIN() 'initialize WDM record buffer
            Logger.Dbg("F90_PUTOLV") : Call F90_PUTOLV(10)
            'Logger.Dbg("F90_SCNDBG") : Call F90_SCNDBG(10) 'lots of debugging
            Logger.Dbg("F90_SPIPH") : Call F90_SPIPH(pPipeReadFromStatus, pPipeWriteToStatus)

            Dim lMsgName As String = FindFile("HSPF Message File", "hspfmsg.wdm")
            Logger.Dbg("MsgName = " & lMsgName)

            Dim lOpt As Integer = 1
            Dim lMsgUnit As Integer
            Dim lRetcod As Integer
            F90_WDBOPNR(lOpt, lMsgName, lMsgUnit, lRetcod, Len(lMsgName))

            If lMsgUnit <> 0 Then
                Dim lFileName As String = ""
                If FileExists(lExeCmd) Then
                    lFileName = lExeCmd
                    Logger.Dbg("OriginalPath '" & CurDir() & "'")
                    ChDriveDir(PathNameOnly(lFileName))
                    Logger.Dbg("NewPath '" & CurDir() & "'")
                Else
                    lFileName = FindFile("WinHspf UCI File Selection", _
                                         lExeCmd, , "Uci Files(*.uci)|*.uci", _
                                         True, True)
                    Logger.Dbg("Path '" & CurDir() & "'")
                End If

                Dim lUci As String = ""
                If lFileName.Length > 0 Then
                    lUci = IO.Path.GetFileNameWithoutExtension(lFileName)
                End If

                If lUci.Length > 0 Then
                    lErrLogName = FilenameSetExt(lFileName, "log")
                    Dim lLoggerString As String = "Begin Logging"
                    If lErrLogFlag Then
                        lLoggerString = WholeFileString(Logger.FileName)
                        Logger.Dbg("ChangeLogFileTO:" & lErrLogName)
                    End If
                    Logger.Status("Show")
                    Logger.StartToFile(lErrLogName, False, False, True)
                    Logger.Dbg(lLoggerString)
                    Logger.Status("(LOGTOFILE " & lErrLogName & ")")

                    Dim lWdmUnit(4) As Integer
                    lOpt = -1
                    Logger.Dbg("Pre:  F90_ACTSCN (" & lOpt & ", " & lWdmUnit(1) & ", " & lMsgUnit & ", " & lRetcod & ", " & lUci & ", " & Len(lUci) & ")")
                    Call F90_ACTSCN(lOpt, lWdmUnit(1), lMsgUnit, lRetcod, lUci, Len(lUci))
                    Logger.Dbg("Post: F90_ACTSCN (" & lOpt & ", " & lWdmUnit(1) & ", " & lMsgUnit & ", " & lRetcod & ", " & lUci & ", " & Len(lUci) & ")")

                    If lRetcod = 0 Then
                        Logger.Dbg("Pre:  F90_SIMSCN (" & lRetcod & ")")
                        Call F90_SIMSCN(lRetcod)
                        Logger.Dbg("Post: F90_SIMSCN (" & lRetcod & ")")
                    End If

                    If lRetcod <> 0 Then
                        Throw New Exception("ERROR - HSPF execution terminated with return code " & CStr(lRetcod) & "." & vbCrLf)
                    End If
                End If
            Else
                Throw New Exception("ERROR - HSPF message file '" & lMsgName & "' is not valid. Code " & lRetcod & vbCrLf)
            End If

            If lSendFeedbackAlways Then
                Throw New Exception("Feedback reqested on command line")
            End If

        Catch ex As Exception
            If Logger.Msg(ex.Message & vbCrLf & "Send a feedback message to the WinHspfLt development team?", _
                          MsgBoxStyle.YesNo, _
                          MsgBoxResult.No, "WinHspfLt Feedback") = MsgBoxResult.Yes Then
                Dim lStr As String = ""
                If Logger.FileName.Length > 0 Then
                    lStr = vbCrLf & "------------ Logfile '" & Logger.FileName & "' Contents ---------------" & _
                           vbCrLf & WholeFileString(Logger.FileName)
                End If
                ShowFeedback(lStr)
            End If
        End Try

        Logger.Status("EXIT")
    End Sub

    Private Sub ShowFeedback(ByRef aProgress As String)
        Dim lfrmFeedback As New frmFeedback

        Dim lName As String = ""
        Dim lEmail As String = ""
        Dim lMessage As String = ""
        Dim lFeedback As String = MapWinUtility.MiscUtils.GetDebugInfo & aProgress
        If lfrmFeedback.ShowFeedback(lName, lEmail, lMessage, lFeedback) Then
            Dim lFeedbackCollection As New System.Collections.Specialized.NameValueCollection
            lFeedbackCollection.Add("name", Trim(lName))
            lFeedbackCollection.Add("email", Trim(lEmail))
            lFeedbackCollection.Add("message", Trim(lMessage))
            lFeedbackCollection.Add("sysinfo", lFeedback)
            Dim lClient As New System.Net.WebClient
            lClient.UploadValues("http://hspf.com/cgi-bin/feedback-basins4.cgi", "POST", lFeedbackCollection)
            Logger.Msg("Feedback successfully sent", "Send Feedback")
        End If
    End Sub

    Private Function StringFindAndRemove(ByRef aStr As String, ByVal aFindStr As String) As Boolean
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
Friend Class StatusMonitor
    Implements MapWinUtility.IProgressStatus

    Dim pInit As Boolean = False
    Dim pMonitorProcess As Process

    Public Sub Progress(ByVal aCurrentPosition As Integer, ByVal aLastPosition As Integer) Implements MapWinUtility.IProgressStatus.Progress
        'TODO: send progress messages to status monitor
    End Sub

    Public Sub Status(ByVal aStatusMessage As String) Implements MapWinUtility.IProgressStatus.Status
        If Not pInit Then
            Try
                Dim lProcessId As Integer = Process.GetCurrentProcess.Id
                pMonitorProcess = New Process
                With pMonitorProcess.StartInfo
                    .FileName = FindFile("Status Monitor", "statusMonitor.exe")
                    .Arguments = lProcessId
                    .CreateNoWindow = True
                    .UseShellExecute = False
                    .RedirectStandardInput = True
                    .RedirectStandardOutput = True
                    'AddHandler pMonitorProcess.OutputDataReceived, AddressOf MonitorMessageHandler
                    .RedirectStandardError = True
                    'AddHandler pMonitorProcess.ErrorDataReceived, AddressOf MonitorMessageHandler
                End With
                pMonitorProcess.Start()
                pMonitorProcess.StandardInput.WriteLine("Show")
                'pMonitorProcess.BeginErrorReadLine()
                'pMonitorProcess.BeginOutputReadLine()
                Logger.Dbg("MonitorLaunched")
                Dim lStream As IO.FileStream = pMonitorProcess.StandardInput.BaseStream
                pPipeWriteToStatus = lStream.Handle 'lStream.SafeFileHandle 
                lStream = pMonitorProcess.StandardOutput.BaseStream
                pPipeReadFromStatus = lStream.Handle
                pInit = True
            Catch ex As Exception
                Logger.Msg("StatusProcessStartError:" & ex.Message)
            End Try
        End If

        WriteStatus(aStatusMessage)

        If aStatusMessage.ToLower = "exit" Then
            If Not pMonitorProcess.HasExited Then
                pMonitorProcess.Kill()
            End If
        End If
    End Sub

    Private Function WriteStatus(ByVal aMsg As String) As Boolean
        If Not IsNothing(pMonitorProcess) Then
            If pMonitorProcess.HasExited Then
                If pMonitorProcess.ExitCode <> &H103S Then 'TODO: check to be sure codes have not changed
                    Return False  'Process at other end of pipe is dead, stop talking to it
                End If
            End If
        End If

        If Left(aMsg, 1) = "(" And Right(aMsg, 1) = ")" Then
            aMsg = Mid(aMsg, 2, Len(aMsg) - 2)
        End If

        If aMsg.Length > 0 Then
            Dim OpenParenEscape As String = Chr(6)
            aMsg = ReplaceString(aMsg, "(", OpenParenEscape)
            Dim CloseParenEscape As String = Chr(7)
            aMsg = ReplaceString(aMsg, ")", CloseParenEscape)
            If Asc(Right(aMsg, 1)) > 31 Then
                aMsg = "(" & aMsg & ")"
            End If
            Logger.Dbg(aMsg)
            pMonitorProcess.StandardInput.WriteLine(aMsg)
        End If
        Return True
    End Function

    Private Sub MonitorMessageHandler(ByVal aSendingProcess As Object, _
                                      ByVal aOutLine As DataReceivedEventArgs)
        If Not String.IsNullOrEmpty(aOutLine.Data) Then
            Logger.Dbg(aOutLine.Data.ToString)
            Logger.Flush()
        End If
    End Sub
End Class