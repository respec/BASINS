Option Strict Off
Option Explicit On

Imports System.Collections.Specialized
Imports MapWinUtility
Imports atcUtility

Module modWinHSPFLt
    Declare Sub F90_W99OPN Lib "hass_ent.dll" ()
    Declare Sub F90_WDBFIN Lib "hass_ent.dll" ()
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

    Private pMsgUnit As Integer
    Private pMsgName As String
    Private pWdmUnit(4) As Integer
    Private pIPC As Object 'ATCoIPC
    Private pUci As String = ""
    Private pFileName As String = ""
    Friend pPipeWriteToStatus As Integer = 0
    Friend pPipeReadFromStatus As Integer = 0

    Public Sub Main()
        Dim lRetcod As Integer
        Dim lInd As Integer
        Dim lOpt As Integer
        Dim lExeCmd As String 'command line
        Dim lErrLogName As String = "WinHspfLt.log"
        Dim lErrLogFlag As Boolean
        Dim lMsg As String

        Try
            lErrLogFlag = False
            lExeCmd = Environment.CommandLine

            lInd = InStr(LCase(lExeCmd), "/log")
            If lInd > 0 Then
                Logger.StartToFile(lErrLogName, False, False, True)
                Logger.Dbg("CommandLine '" & lExeCmd & "'")
                lErrLogFlag = True
                lExeCmd = Trim(Left(lExeCmd, lInd - 1) & Mid(lExeCmd, lInd + 4))
            End If
            Logger.Dbg("ExeName '" & StrRetRem(lExeCmd) & "'")
            Logger.Dbg("CommandLineArgs '" & lExeCmd & "'")

            lInd = InStr(LCase(lExeCmd), "/batch")
            If lInd > 0 Then
                Logger.DisplayMessageBoxes = False
                lExeCmd = Trim(Left(lExeCmd, lInd - 1) & Mid(lExeCmd, lInd + 6))
            End If
            Logger.Dbg(" AfterBatchCheck: '" & lExeCmd & "' MsgBoxDisplay:" & Logger.DisplayMessageBoxes)

            pPipeReadFromStatus = StrFirstInt(lExeCmd)
            pPipeWriteToStatus = StrFirstInt(lExeCmd)
            Logger.Dbg(" AftPipX: '" & lExeCmd & "' FromStatus " & pPipeReadFromStatus & " ToStatus " & pPipeWriteToStatus)

            If pPipeWriteToStatus = 0 Or pPipeReadFromStatus = 0 Then
                Logger.ProgressStatus = New StatusMonitor
                Logger.Status("Begin") 'set handles but don't open
            End If

            Logger.Dbg("StartPath:" & CurDir())
            If pPipeWriteToStatus = -1 Then pPipeWriteToStatus = 0
            If pPipeReadFromStatus = -1 Then pPipeReadFromStatus = 0
            Logger.Dbg(" PipeWriteToStatus:" & pPipeWriteToStatus)
            Logger.Dbg(" PipeReadFromStatus:" & pPipeReadFromStatus)

            Logger.Dbg("F90_W99OPN") : Call F90_W99OPN() 'open error file for fortan problems
            Logger.Dbg("F90_WDBFIN") : Call F90_WDBFIN() 'initialize WDM record buffer
            Logger.Dbg("F90_PUTOLV") : Call F90_PUTOLV(10)
            'Logger.Dbg("F90_SCNDBG") : Call F90_SCNDBG(10) 'lots of debugging
            Logger.Dbg("F90_SPIPH") : Call F90_SPIPH(pPipeReadFromStatus, pPipeWriteToStatus)

            pMsgName = FindFile("HSPF Message File", "hspfmsg.wdm")
            Logger.Dbg("MsgName = " & pMsgName)
            lOpt = 1
            F90_WDBOPNR(lOpt, pMsgName, pMsgUnit, lRetcod, Len(pMsgName))

            If pMsgUnit <> 0 Then
                If FileExists(lExeCmd) Then
                    pFileName = lExeCmd
                Else
                    pFileName = FindFile("WinHspf UCI File Selection", _
                                         lExeCmd, , "Uci Files(*.uci)|*.uci", _
                                         True, True)
                End If
                If pFileName.Length > 0 Then
                    pUci = FilenameOnly(pFileName)
                End If

                If pUci.Length > 0 Then
                    Logger.Status("Open")
                    lErrLogName = FilenameSetExt(pFileName, "log")
                    If lErrLogFlag Then
                        Logger.Dbg("ChangeLogFileTO:" & lErrLogName)
                    End If
                    Logger.StartToFile(lErrLogName, False, False, True)
                    Logger.Status("(LOGTOFILE " & lErrLogName & ")")
                    lOpt = -1

                    Logger.Dbg("Pre:  F90_ACTSCN (" & lOpt & ", " & pWdmUnit(1) & ", " & pMsgUnit & ", " & lRetcod & ", " & pUci & ", " & Len(pUci) & ")")
                    Call F90_ACTSCN(lOpt, pWdmUnit(1), pMsgUnit, lRetcod, pUci, Len(pUci))
                    Logger.Dbg("Post: F90_ACTSCN (" & lOpt & ", " & pWdmUnit(1) & ", " & pMsgUnit & ", " & lRetcod & ", " & pUci & ", " & Len(pUci) & ")")
                    If lRetcod = 0 Then
                        Logger.Dbg("Pre:  F90_SIMSCN (" & lRetcod & ")")
                        Call F90_SIMSCN(lRetcod)
                        Logger.Dbg("Post: F90_SIMSCN (" & lRetcod & ")")
                    End If
                    If lRetcod <> 0 Then
                        lMsg = "HSPF execution terminated with return code " & CStr(lRetcod) & "." & vbCrLf
                        Logger.Status(lMsg)
                        Throw New Exception(lMsg)
                    End If
                End If
            Else
                lMsg = "HSPF message file '" & pMsgName & "' is not valid. Code " & lRetcod & vbCrLf
                Throw New Exception(lMsg)
            End If
        Catch ex As Exception
            lMsg = "Fatal Error: " & ex.Message
            If Logger.Msg(lMsg & vbCrLf & vbCrLf & "Send a feedback message to the WinHSPFLt development team.", _
                          MsgBoxStyle.YesNo, _
                          MsgBoxResult.No, "HSPF Error") = MsgBoxResult.Yes Then
                Dim lStr As String = ""
                If Logger.FileName.Length > 0 Then lStr = WholeFileString(Logger.FileName)
                ShowFeedback(lStr)
            End If
        End Try

        Logger.Status("EXIT")
    End Sub

    Private Sub ShowFeedback(ByRef Progress As String)
        Dim lfrmFeedback As New frmFeedback

        Dim lName As String = ""
        Dim lEmail As String = ""
        Dim lMessage As String = ""
        Dim lFeedback As String = ""
        If lfrmFeedback.ShowFeedback(lName, lEmail, lMessage, lFeedback) Then
            Dim lFeedbackCollection As New NameValueCollection
            lFeedbackCollection.Add("name", Trim(lName))
            lFeedbackCollection.Add("email", Trim(lEmail))
            lFeedbackCollection.Add("message", Trim(lMessage))
            lFeedbackCollection.Add("sysinfo", lFeedback)
            Dim lClient As New System.Net.WebClient
            lClient.UploadValues("http://hspf.com/cgi-bin/feedback-basins4.cgi", "POST", lFeedbackCollection)
            Logger.Msg("Feedback successfully sent", "Send Feedback")
        End If
    End Sub
End Module

Friend Class StatusMonitor
    Implements MapWinUtility.IProgressStatus

    Dim pInit As Boolean = False
    Dim pProcess As Process

    Public Sub Progress(ByVal CurrentPosition As Integer, ByVal LastPosition As Integer) Implements MapWinUtility.IProgressStatus.Progress

    End Sub

    Public Sub Status(ByVal aStatusMessage As String) Implements MapWinUtility.IProgressStatus.Status
        If Not pInit Then
            Try
                Dim lProcessId As Integer = Process.GetCurrentProcess.Id
                pProcess = New Process
                pProcess.StartInfo.FileName = FindFile("Status Monitor", "status.exe")
                pProcess.StartInfo.Arguments = lProcessId
                pProcess.StartInfo.UseShellExecute = False
                pProcess.StartInfo.RedirectStandardInput = True
                pProcess.StartInfo.RedirectStandardOutput = True
                pProcess.Start()
                Dim lStream As IO.FileStream = pProcess.StandardInput.BaseStream
                pPipeWriteToStatus = lStream.Handle ' SafeFileHandle
                lStream = pProcess.StandardOutput.BaseStream
                pPipeReadFromStatus =lStream.Handle 
                pInit = True
            Catch ex As Exception
                Logger.Msg("StatusProcessStartError:" & ex.Message)
            End Try
        End If
        WriteTokenToPipe(aStatusMessage)
        If aStatusMessage.ToLower = "exit" Then
            If Not pProcess.HasExited Then
                pProcess.Kill()
            End If
        End If
    End Sub

    Private Function WriteTokenToPipe(ByVal aMsg As String) As Boolean
        Dim OpenParenEscape As String = Chr(6)
        Dim CloseParenEscape As String = Chr(7)
        Dim lpExitCode As Integer

        If Not IsNothing(pProcess) Then
            If pProcess.HasExited Then
                lpExitCode = pProcess.ExitCode
                If lpExitCode <> &H103S Then 'TODO: check to be sure codes have not changed
                    Return False  'Process at other end of pipe is dead, stop talking to it
                End If
            End If
        End If

        If Left(aMsg, 1) = "(" And Right(aMsg, 1) = ")" Then
            aMsg = Mid(aMsg, 2, Len(aMsg) - 2)
        End If
        aMsg = ReplaceString(aMsg, "(", OpenParenEscape)
        aMsg = ReplaceString(aMsg, ")", CloseParenEscape)
        If aMsg.Length > 0 Then
            If Asc(Right(aMsg, 1)) > 31 Then
                aMsg = "(" & aMsg & ")"
            End If
            pProcess.StandardInput.WriteLine(aMsg)
        End If
        Return True
    End Function
End Class