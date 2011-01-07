Option Strict Off
Option Explicit On

Imports MapWinUtility 'for Logger
Imports MapWinUtility.Strings

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
    Public Function Main() As Integer
        Dim lRetcod As Integer = 1234
        Dim lSendFeedBack As Boolean = True
        Dim lErrLogFlag As Boolean = False
        Try
            Dim lErrLogName As String = "WinHspfLt.log"

            Dim lExeCmd As String = Environment.CommandLine

            If StringFindAndRemove(lExeCmd, "/log") Then
                Logger.StartToFile(lErrLogName, False, False, True)
                Logger.Dbg("Early Logging On")
                lErrLogFlag = True
            End If

            If StringFindAndRemove(lExeCmd, "/nosendfeedback") Then
                lSendFeedBack = False
            End If

            If StringFindAndRemove(lExeCmd, "/debugpause") Then
                'Logger.Msg("Pause to attach to process")
            End If

            Logger.Dbg("ExeName '" & StrSplit(lExeCmd, " ", """") & "'")
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
                        Logger.Dbg("ChangeLogFileTo:" & lErrLogName)
                    End If
                    Logger.Status("Show")
                    Logger.Status("Label Title WinHSPFLt " & lUci)
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
                        Logger.Status("Progress Percent Off")
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
            If lRetcod = 0 Then lRetcod = 4321
            Logger.Dbg(ex.Message & vbCrLf & ex.StackTrace)
            If Not lErrLogFlag Then
                Logger.Msg(ex.Message & vbCrLf & ex.StackTrace, "HSPF Error")
            End If
            If lSendFeedBack Then
                ShowFeedback()
            End If
        End Try

        Logger.Status("EXIT")
        Application.DoEvents()
        Return lRetcod
    End Function

    Private Sub ShowFeedback()
        Dim lName As String = ""
        Dim lEmail As String = ""
        Dim lMessage As String = ""
        Dim lSysInfo As String = ""
        Dim lfrmFeedback As New frmFeedback
        lfrmFeedback.Text = "HSPF Error Report"
        If lfrmFeedback.ShowFeedback(lName, lEmail, lMessage, lSysInfo, True, True, True, Nothing) Then
            Dim lFeedbackCollection As New System.Collections.Specialized.NameValueCollection
            lFeedbackCollection.Add("name", Trim(lName))
            lFeedbackCollection.Add("email", Trim(lEmail))
            lFeedbackCollection.Add("message", Trim(lMessage))
            lFeedbackCollection.Add("sysinfo", lSysInfo)
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
        WriteStatus("PROGRESS " & aCurrentPosition & " of " & aLastPosition)
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
                '
                'NOTE: to debug pMonitorProcess, in VS2005 (not Express) - choose Tools:AttachToProcess - StatusMonitor
                '
                'pMonitorProcess.StandardInput.WriteLine("Show")
                'pMonitorProcess.BeginErrorReadLine()
                'pMonitorProcess.BeginOutputReadLine()
                Logger.Dbg("MonitorLaunched")
                Dim lStreamMonitorInputFromMyOutput As IO.FileStream = pMonitorProcess.StandardInput.BaseStream
                pPipeWriteToStatus = lStreamMonitorInputFromMyOutput.SafeFileHandle.DangerousGetHandle
                Dim lStreamMonitorOutputToMyInput As IO.FileStream = pMonitorProcess.StandardOutput.BaseStream
                pPipeReadFromStatus = lStreamMonitorOutputToMyInput.SafeFileHandle.DangerousGetHandle
            Catch ex As Exception
                Logger.Msg("StatusProcessStartError:" & ex.Message)
            End Try
            pInit = True
        End If

        WriteStatus(aStatusMessage)

        If aStatusMessage.ToLower = "exit" Then
            If Not pMonitorProcess.HasExited Then
                pMonitorProcess.StandardInput.WriteLine("Exit")
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

        If aMsg.StartsWith("(") AndAlso aMsg.EndsWith(")") Then
            aMsg = aMsg.Substring(1, aMsg.Length - 2)
        End If

        If aMsg.Length > 0 Then
            Dim OpenParenEscape As String = Chr(6)
            aMsg = aMsg.Replace("(", OpenParenEscape)
            Dim CloseParenEscape As String = Chr(7)
            aMsg = aMsg.Replace(")", CloseParenEscape)
            If Asc(Right(aMsg, 1)) > 31 Then
                aMsg = "(" & aMsg & ")"
            End If
            Logger.Dbg(aMsg)
            pMonitorProcess.StandardInput.WriteLine(aMsg)
        End If
        Return True
    End Function
End Class