Attribute VB_Name = "modWinHSPFLt"
Option Explicit
'Copyright 2000 by AQUA TERRA Consultants

Dim pMsgUnit As Long, pMsgName As String
Dim pWdmUnit(4) As Long
Dim AtCoRegistry1 As ATCoRegistry
Dim pStatus As clsStatus, pStatusName As String
Dim pUci As String, pFileName As String
Dim pAppName As String
Dim pStartPath As String

Sub Main()
  Dim hin&, hout&, hdle&, i&, r&, l&
  Dim s As String * 80
  Dim ExeCmd As String 'command line
  Dim ExeName As String, ExePath As String
  
  pStartPath = CurDir
  pAppName = "WinHspfLt.exe"
  hdle = GetModuleHandle(pAppName)
  'MsgBox "got handle " & hdle
  i = GetModuleFileName(hdle, s, 80)
  'MsgBox "got name " & s, i
  ExeName = UCase(Left(s, InStr(s, Chr(0)) - 1))
  'MsgBox "exename " & ExeName
  If InStr(ExeName, "VB6.EXE") Then 'running in development environment
    ShowWin "Microsoft Visual Basic", SW_MINIMIZE, 0 'get vb out of the way
    ExePath = "c:\vbapps6\WinHspfLt\bin\" 'may vary by machine
    ExeCmd = "" 'no default for command line
  Else
    ExeCmd = Command$ 'command line
    'MsgBox "Command: " & ExeCmd & ":" & s & ":" & pAppName
    i = InStr(s, pAppName)
    If i > 0 Then
      ExePath = Left(s, InStr(s, pAppName) - 1)
    Else
      ExePath = ""
    End If
    'MsgBox "Exepath: " & ExePath
    'MsgBox Len(ExePath) & ExePath & vbCrLf & _
    '       Len(ExeName) & ExeName & vbCrLf & _
    '       Len(ExeCmd) & ExeCmd & vbCrLf & _
    '       Len(s) & s & vbCrLf
    ChDrive (Left(ExePath, 2))
    ChDir ExePath
  End If
  
  Set AtCoRegistry1 = New ATCoRegistry
  AtCoRegistry1.AppName = "WinHspfLt"
  
  If InStr(UCase(Command), "/REGPATH") > 0 Then
    AtCoRegistry1.GlobalValue("", "ExePath") = ExePath
  ElseIf InStr(UCase(Command), "/UNREGPATH") > 0 Then
    MsgBox "You may manually remove the key" & vbCr _
    & "HKEY_LOCAL_MACHINE\Software\AQUA TERRA Consultants\WinHspfLt\ExePath", vbOKOnly, "WinHspfLt Registry"
  Else
    Set pStatus = New clsStatus
    pStatusName = FindFileIfMissing(ExePath & "status.exe")
    If Len(pStatusName) > 0 Then
      pStatus.StartMonitor (pStatusName)
      hin = pStatus.ComputeRead
      hout = pStatus.ComputeWrite
          
      Call F90_W99OPN  'open error file for fortan problems
      Call F90_WDBFIN  'initialize WDM record buffer
      Call F90_PUTOLV(10)
    
      Call F90_SPIPH(hin, hout)
      
      pMsgName = FindFileIfMissing(ExePath & "hspfmsg.wdm")
      If Len(pMsgName) > 0 Then
        i = 1
        pMsgUnit = F90_WDBOPN(i, pMsgName, Len(pMsgName))
        If pMsgUnit <> 0 Then
          If Len(ExeCmd) > 0 Then 'a command was tried
            pFileName = FindFileIfMissing(ExeCmd)
            If Len(pFileName) > 0 Then
              If Mid(pFileName, 2, 1) = ":" Then
                ChDrive (Left(pFileName, 1))
              End If
              ChDir PathNameOnly(pFileName)
              pUci = FilenameOnly(pFileName)
              If LCase(Right(pUci, 4)) = ".uci" Then
                pUci = Left(pUci, Len(pUci) - 4)
              End If
            Else
              pUci = ""
            End If
          Else
            With frmDummy.cmdFile
              .Filename = ""
              If InStr(pStartPath, "VB") = 0 Then
                .InitDir = pStartPath
              Else
                .InitDir = ExePath
              End If
              .DialogTitle = "WinHspf UCI File Selection"
              .Filter = "Uci Files(*.uci)|*.uci"
              .CancelError = True
              On Error GoTo X:
              .ShowOpen
              pFileName = .Filename
              pUci = Left(.FileTitle, Len(.FileTitle) - 4)
            End With
          End If
          
          If Len(pUci) > 0 Then
            i = -1
            Call F90_ACTSCN(i, pWdmUnit(1), pMsgUnit, r, pUci, Len(pUci))
            
            If r = 0 Then
              Call F90_SIMSCN(r)
            End If
            If r <> 0 Then
              MsgBox "HSPF execution terminated with return code " & r & "."
            End If
          End If
        Else
          MsgBox "HSPF message file '" & pMsgName & "' is not valid."
        End If
      End If
    End If
X:
    Unload frmDummy
    pStatus.ExitMonitor
  End If
End Sub

Function FindFileIfMissing(s$) As String
  Dim lExt$
  
  lExt = Right(s, Len(s) - InStrRev(s, "."))
  
  If Len(Dir(s)) > 0 Then
    FindFileIfMissing = s
  Else
    With frmDummy.cmdFile
      On Error Resume Next
      .Filename = FilenameOnly(s) & "." & lExt
      .InitDir = PathNameOnly(s)
      .DialogTitle = "Find Missing File " & s
      .CancelError = True
      On Error GoTo Y:
      .ShowOpen
      FindFileIfMissing = .Filename
    End With
  End If
  Exit Function
Y:
  FindFileIfMissing = ""
End Function
