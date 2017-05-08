Attribute VB_Name = "modWinHSPFLt"
Option Explicit
'Copyright 2002 by AQUA TERRA Consultants

Dim pMsgUnit As Long, pMsgName As String
Dim pWdmUnit(4) As Long
Dim pIPC As Object 'ATCoIPC
Dim pUci As String, pFileName As String
Dim pAppName As String
Dim pStartPath As String

Sub Main()
  Dim hin&, hout&, i&, r&, l&
  Dim ExeCmd As String 'command line
  Dim ExeName As String
  Dim ExePath As String
  Dim Progress As String
  Dim PipeReadFromStatus As Long
  Dim PipeWriteToStatus As Long
  Dim ErrLogString As String
  Dim ErrLogName As String
  Dim ErrLogFlag As Boolean
  Dim s As String
  
  On Error GoTo ErrHand
  ErrLogFlag = False
  ErrLogString = ""
  
  ExeCmd = Command$ 'command line
  PipeReadFromStatus = StrFirstInt(ExeCmd)
  PipeWriteToStatus = StrFirstInt(ExeCmd)
  
  Progress = Progress & vbCrLf & "Command: " & ExeCmd
  
  pStartPath = CurDir
  
  pAppName = "WinHspfLt.exe"
  ExeName = UCase(GetExeFullPath)
  
  Progress = Progress & vbCrLf & "exename " & ExeName
    
  If InStr(ExeName, "VB6.EXE") > 0 Then 'running in development environment
    ShowWin "Microsoft Visual Basic", SW_MINIMIZE, 0 'get vb out of the way
    ExePath = App.path
  Else
    ExePath = PathNameOnly(ExeName)
    Progress = Progress & vbCrLf & "Exepath: " & ExePath & " (" & Len(ExePath) & ")"
  End If
    
  If PipeWriteToStatus = 0 Or PipeReadFromStatus = 0 Then
    Progress = Progress & vbCrLf & "Set pIPC"
    Set pIPC = CreateObject("ATCoCtl.ATCoIPC")
    PipeReadFromStatus = pIPC.hPipeReadFromProcess(0)
    PipeWriteToStatus = pIPC.hPipeWriteToProcess(0)
  End If
  If PipeWriteToStatus = -1 Then PipeWriteToStatus = 0
  If PipeReadFromStatus = -1 Then PipeReadFromStatus = 0
  
  Progress = Progress & vbCrLf & "F90_W99OPN": Call F90_W99OPN  'open error file for fortan problems
  Progress = Progress & vbCrLf & "F90_WDBFIN": Call F90_WDBFIN  'initialize WDM record buffer
  Progress = Progress & vbCrLf & "F90_PUTOLV": Call F90_PUTOLV(10)
  Progress = Progress & vbCrLf & "F90_SPIPH":  Call F90_SPIPH(PipeReadFromStatus, PipeWriteToStatus)
  
  pMsgName = FindFileIfMissing("hspfmsg.wdm")
  Progress = Progress & vbCrLf & "pMsgName = " & pMsgName
  
  If Len(pMsgName) > 0 Then
    i = 1
    Progress = Progress & vbCrLf & "F90_WDBOPN"
    pMsgUnit = F90_WDBOPN(i, pMsgName, Len(pMsgName))
    If pMsgUnit <> 0 Then
      If Len(ExeCmd) > 0 Then 'a command was tried
        Progress = Progress & vbCrLf & "a command was tried" & vbCrLf & "FindFileIfMissing(" & ExeCmd & ")"
        pFileName = FindFileIfMissing(ExeCmd)
        If Len(pFileName) > 0 Then
          Progress = Progress & vbCrLf & "ChDriveDir " & PathNameOnly(pFileName)
          ChDriveDir PathNameOnly(pFileName)
          Progress = Progress & vbCrLf & "pUci = " & FilenameOnly(pFileName)
          pUci = FilenameOnly(pFileName)
        Else
          pUci = ""
        End If
      Else
        Progress = Progress & vbCrLf & "With frmDummy.cmdFile"
        With frmDummy.cmdFile
          .filename = ""
          If InStr(pStartPath, "VB") = 0 Then
            .InitDir = GetDataPath(pStartPath)
          Else
            .InitDir = GetDataPath(ExePath)
          End If
          Progress = Progress & vbCrLf & "InitDir = " & .InitDir
          .DialogTitle = "WinHspf UCI File Selection"
          .Filter = "Uci Files(*.uci)|*.uci"
          .CancelError = True
          On Error GoTo Cancelled
          Progress = Progress & vbCrLf & ".ShowOpen"
          .ShowOpen
          On Error GoTo ErrHand

          pFileName = .filename
          pUci = .FileTitle
        End With
      End If
      If LCase(Right(pUci, 4)) = ".uci" Then
        pUci = Left(pUci, Len(pUci) - 4)
      End If
      
      If Len(pUci) > 0 Then
      
        'If a file by the name 'WinHSPFLtError.Log' exists alongside
        'the UCI, send error messages to it instead of to message boxes.
        If Len(PathNameOnly(pFileName)) > 0 Then
          ErrLogName = PathNameOnly(pFileName) & "\WinHSPFLtError.Log"
        Else
          ErrLogName = "WinHSPFLtError.Log"
        End If
        ErrLogFlag = FileExists(ErrLogName)
      
        i = -1
        Progress = Progress & vbCrLf & "Pre:  F90_ACTSCN (" & i & ", " & pWdmUnit(1) & ", " & pMsgUnit & ", " & r & ", " & pUci & ", " & Len(pUci) & ")"
        Call F90_ACTSCN(i, pWdmUnit(1), pMsgUnit, r, pUci, Len(pUci))
        Progress = Progress & vbCrLf & "Post: F90_ACTSCN (" & i & ", " & pWdmUnit(1) & ", " & pMsgUnit & ", " & r & ", " & pUci & ", " & Len(pUci) & ")"
        If r = 0 Then
          Progress = Progress & vbCrLf & "Pre:  F90_SIMSCN (" & r & ")"
          Call F90_SIMSCN(r)
          Progress = Progress & vbCrLf & "Post: F90_SIMSCN (" & r & ")"
        End If
        If r <> 0 Then
          s = "HSPF execution terminated with return code " & CStr(r) & "."
          If ErrLogFlag Then
            ErrLogString = ErrLogString & vbCrLf & s
          Else
            MsgBox s
          End If
        End If
      End If
    Else
      s = "HSPF message file '" & pMsgName & "' is not valid."
      If ErrLogFlag Then
        ErrLogString = ErrLogString & vbCrLf & s
      Else
        MsgBox s
      End If
    End If
  End If
Cancelled:
  On Error Resume Next
  
  If ErrLogFlag And Len(ErrLogString) > 0 Then
    SaveFileString ErrLogName, ErrLogString
  End If
  Unload frmDummy
  If Not (pIPC Is Nothing) Then
    pIPC.SendMonitorMessage "(Exit)"
  End If
  Exit Sub

ErrHand:
  s = "Fatal Error: " & Err.Description
  If ErrLogFlag Then
    ErrLogString = ErrLogString & vbCrLf & s & vbCrLf & Progress
  Else
    Dim myMsgBox As ATCoMessage
    Set myMsgBox = New ATCoMessage
    Set myMsgBox.Icon = frmDummy.Icon
    If myMsgBox.Show(s & vbCrLf & vbCrLf & "Click 'Send' to send a feedback message to the WinHSPFLt development team.", "HSPF Error", "&Send", "+-&Quit") = 1 Then
      ShowFeedback Progress, ExePath
    End If
  End If
  Resume Cancelled
End Sub

Private Sub ShowFeedback(Progress As String, Optional ExePath As String = "")
  'Dim StartTimer As Double
  Dim feedback As clsATCoFeedback
  Set feedback = New clsATCoFeedback
  If Len(ExePath) > 0 Then feedback.AddFile ExePath & pAppName
  feedback.AddText Progress & vbCrLf & vbCrLf & Err.Description
  feedback.Wait = True
  feedback.Show App, frmDummy.Icon
  'StartTimer = Timer
  'While Timer < StartTimer + 60
  '  DoEvents
  'Wend
  'Set feedback = Nothing
End Sub

Public Function FindFileIfMissing(ByVal s As String) As String
  Dim basename As String
  Dim Progress As String
  
  On Error GoTo ErrHand
    
  Progress = "If IsMissing(" & s & ")"
  If Not FileExists(s) Then
    basename = FilenameNoPath(s)
    s = App.path & "\" & basename
    If Not FileExists(s) Then
      s = GetWindowsSysDir & basename
      If Not FileExists(s) Then
        With frmDummy.cmdFile
          On Error Resume Next
          .filename = basename
          Progress = Progress & vbCrLf & ".InitDir = PathNameOnly(" & s & ")"
          .InitDir = PathNameOnly(s)
          .DialogTitle = "Find Missing File " & s
          .CancelError = True
          On Error GoTo Cancelled
          .ShowOpen
          On Error GoTo ErrHand
          s = .filename
        End With
      End If
    End If
  End If
  If FileExists(s) Then
    FindFileIfMissing = s
  Else
    FindFileIfMissing = ""
  End If
  Exit Function
Cancelled:
  FindFileIfMissing = ""
  Exit Function

ErrHand:
  MsgBox Progress & vbCrLf & vbCrLf & Err.Description, vbCritical, "WniHspfLt Error in FindFileIfMissing"
  Resume Cancelled
End Function

Private Function GetDataPath(Optional ByVal aDefaultPath As String = "") As String
  Dim BasinsPos As Long
  
  If Len(aDefaultPath) = 0 Then aDefaultPath = CurDir
  If Right(aDefaultPath, 1) <> "\" Then aDefaultPath = aDefaultPath & "\"
  
  BasinsPos = InStr(UCase(aDefaultPath), "BASINS\")
  If BasinsPos > 0 Then aDefaultPath = Left(aDefaultPath, BasinsPos + 6) & "data"
  
  GetDataPath = aDefaultPath
End Function


