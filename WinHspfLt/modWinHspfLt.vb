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
    Private pPipeWriteToStatus As Integer
    Private pPipeReadFromStatus As Integer

    Public Sub Main()
        Dim lRetcod As Integer
        Dim lInd As Integer
        Dim lOpt As Integer
        Dim lExeCmd As String 'command line
        Dim lExeName As String
        Dim lErrLogName As String = "WinHspfLt.log"
        Dim lErrLogFlag As Boolean
        Dim lMsg As String

        Try
            lErrLogFlag = False
            lExeCmd = Environment.CommandLine
            lExeName = StrRetRem(lExeCmd)
            Logger.Dbg("CommandLineArgs: '" & lExeCmd & "'")

            lInd = InStr(LCase(lExeCmd), "/log")
            If lInd > 0 Then
                Logger.StartToFile(lErrLogName, False, False, True)
                lErrLogFlag = True
                lExeCmd = Trim(Left(lExeCmd, lInd - 1) & Mid(lExeCmd, lInd + 4))
            End If
            Logger.Dbg(" AftLogX: '" & lExeCmd & "' ErrLogFlag:" & lErrLogFlag)

            lInd = InStr(LCase(lExeCmd), "/batch")
            If lInd > 0 Then
                Logger.DisplayMessageBoxes = False
                lExeCmd = Trim(Left(lExeCmd, lInd - 1) & Mid(lExeCmd, lInd + 4))
            End If
            Logger.Dbg(" AfBatch: '" & lExeCmd & "' MsgBoxDisplay:" & Logger.DisplayMessageBoxes)

            pPipeReadFromStatus = StrFirstInt(lExeCmd)
            pPipeWriteToStatus = StrFirstInt(lExeCmd)
            Logger.Dbg(" AftPipX: '" & lExeCmd & "'")
            If pPipeWriteToStatus = 0 Or pPipeReadFromStatus = 0 Then
                Logger.Dbg(" Set pIPC")
                pIPC = CreateObject("ATCoCtl.ATCoIPC")
                pPipeReadFromStatus = pIPC.hPipeReadFromProcess(0)
                pPipeWriteToStatus = pIPC.hPipeWriteToProcess(0)
            End If
            If pPipeWriteToStatus = -1 Then pPipeWriteToStatus = 0
            If pPipeReadFromStatus = -1 Then pPipeReadFromStatus = 0
            Logger.Dbg(" PipeWriteToStatus:" & pPipeWriteToStatus)
            Logger.Dbg(" PipeReadFromStatus:" & pPipeReadFromStatus)

            writeStatus("StartPath:" & CurDir())
            writeStatus("exename " & lExeName)

            writeStatus("F90_W99OPN") : Call F90_W99OPN() 'open error file for fortan problems
            'writeStatus("F90_WDBFIN") : Call F90_WDBFIN() 'initialize WDM record buffer
            writeStatus("F90_PUTOLV") : Call F90_PUTOLV(10)
            'writeStatus("F90_SCNDBG") : Call F90_SCNDBG(10)
            writeStatus("F90_SPIPH") : Call F90_SPIPH(pPipeReadFromStatus, pPipeWriteToStatus)

            pMsgName = FindFile("hspfmsg.wdm")
            writeStatus("pMsgName = " & pMsgName)
            lOpt = 1
            F90_WDBOPNR(lOpt, pMsgName, pMsgUnit, lRetcod, Len(pMsgName))

            If pMsgUnit <> 0 Then
                pFileName = FindFile("WinHspf UCI File Selection", _
                                     lExeCmd, , "Uci Files(*.uci)|*.uci", _
                                     False, True)
                If pFileName.Length > 0 Then
                    pUci = FilenameOnly(pFileName)
                End If

                If pUci.Length > 0 Then
                    'If lErrLogFlag Then writeStatus("(Open)")
                    lErrLogName = FilenameSetExt(pFileName, "log")
                    If Not lErrLogFlag Then
                        Logger.Dbg("ChangeLogFileTO:" & lErrLogName)
                        Logger.StartToFile(lErrLogName, False, False, True)
                    End If
                End If
                If lErrLogFlag Then
                    writeStatus("(LOGTOFILE " & lErrLogName & ")")
                End If

                lOpt = -1
                writeStatus("Pre:  F90_ACTSCN (" & lOpt & ", " & pWdmUnit(1) & ", " & pMsgUnit & ", " & lRetcod & ", " & pUci & ", " & Len(pUci) & ")")
                Call F90_ACTSCN(lOpt, pWdmUnit(1), pMsgUnit, lRetcod, pUci, Len(pUci))
                writeStatus("Post: F90_ACTSCN (" & lOpt & ", " & pWdmUnit(1) & ", " & pMsgUnit & ", " & lRetcod & ", " & pUci & ", " & Len(pUci) & ")")
                If lRetcod = 0 Then
                    writeStatus("Pre:  F90_SIMSCN (" & lRetcod & ")")
                    Call F90_SIMSCN(lRetcod)
                    writeStatus("Post: F90_SIMSCN (" & lRetcod & ")")
                End If
                If lRetcod <> 0 Then
                    lMsg = "HSPF execution terminated with return code " & CStr(lRetcod) & "." & vbCrLf
                    Throw New Exception(lMsg)
                End If
            Else
                lMsg = "HSPF message file '" & pMsgName & "' is not valid. Code " & lRetcod & vbCrLf
                Throw New Exception(lMsg)
            End If
        Catch ex As Exception
            lMsg = "Fatal Error: " & ex.Message
            If Logger.Message(lMsg & vbCrLf & vbCrLf & "Send a feedback message to the WinHSPFLt development team.", _
                                  "HSPF Error", _
                                  MessageBoxButtons.YesNo, _
                                  MessageBoxIcon.Error, _
                                  MessageBoxDefaultButton.Button2) = MsgBoxResult.Yes Then
                ShowFeedback(WholeFileString(Logger.FileName))
            End If
        End Try

        If Not (pIPC Is Nothing) Then
            pIPC.SendMonitorMessage("(Exit)")
        End If
    End Sub

    Private Sub ShowFeedback(ByRef Progress As String)
        'Dim StartTimer As Double
        Dim lfrmFeedback As New frmFeedback

        'feedback.AddText(Progress & vbCrLf & vbCrLf & Err.Description)
        'feedback.Wait = True
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

        'StartTimer = Timer
        'While Timer < StartTimer + 60
        '  DoEvents
        'Wend
        'Set feedback = Nothing
    End Sub

    'Private Function GetDataPath(Optional ByVal aDefaultPath As String = "") As String
    '    Dim BasinsPos As Integer

    '    If Len(aDefaultPath) = 0 Then aDefaultPath = CurDir()
    '    If Right(aDefaultPath, 1) <> "\" Then aDefaultPath = aDefaultPath & "\"

    '    BasinsPos = InStr(UCase(aDefaultPath), "BASINS\")
    '    If BasinsPos > 0 Then
    '        aDefaultPath = Left(aDefaultPath, BasinsPos + 6) & "data"
    '    End If

    '    GetDataPath = aDefaultPath
    'End Function

    Private Function writeStatus(ByRef aMsg As String) As String
        Logger.Dbg(aMsg)
        If pIPC Is Nothing Then
            'WriteTokenToPipe(pPipeWriteToStatus, s)
        Else
            pIPC.SendMonitorMessage(aMsg)
        End If
        Return aMsg 'return the message
    End Function
End Module