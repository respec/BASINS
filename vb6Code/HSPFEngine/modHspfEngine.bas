Attribute VB_Name = "modHspfEngine"
Option Explicit
'Copyright 2001 by AQUA TERRA Consultants

Dim pParentInPipeHandle As Long
Dim pParentOutPipeHandle As Long
Dim pStatusInPipeHandle As Long
Dim pStatusOutPipeHandle As Long
Dim ParentProcess As Long

Private Sub Main()
  Dim desiredAccess&, inheritHandle&, ParentID&
  Dim ExitCode As Long, res As Long
  Dim curToken As String
  Dim NoTokenCount As Long
  Static pipeBuffer$
  
  On Error GoTo ErrHand
  
  ParentProcess = 0
  ParentID = 0
  If Len(Command) > 0 Then
    If IsNumeric(Command) Then
      ParentID = CLng(Command)
    End If
  End If
  'MsgBox "Parent ID=" & ParentID
  desiredAccess = &H400 'PROCESS_QUERY_INFORMATION = &H400
  inheritHandle = False
  If ParentID = 0 Then
    MsgBox "HSPF Engine must be called by an appropriate HSPF Front End like WinHSPF", vbExclamation, "HSPF Engine Error"
    End
  Else
    ParentProcess = OpenProcess(desiredAccess, inheritHandle, ParentID)
  End If
  pStatusInPipeHandle = GetStdHandle(STD_INPUT_HANDLE)
  pStatusOutPipeHandle = GetStdHandle(STD_OUTPUT_HANDLE)
  Call F90_SPIPH(pStatusInPipeHandle, pStatusOutPipeHandle)
  
'  frmDummy.timerPipe.Enabled = True
  
  res = GetExitCodeProcess(ParentProcess, ExitCode)
  'process messages from parent
  While res <> 0 And ExitCode = &H103 'front end STILL_ACTIVE
    DoEvents
    curToken = ReadTokenFromPipe(pStatusInPipeHandle, pipeBuffer, False)
    If Len(curToken) > 0 Then
      NoTokenCount = 0
      GotToken curToken
    Else 'If there wasn't a message, maybe wait a moment before checking again
      If NoTokenCount < 1000 Then
        NoTokenCount = NoTokenCount + 1
      Else
        Sleep 50
      End If
    End If
    res = GetExitCodeProcess(ParentProcess, ExitCode)
  Wend

  Exit Sub
  
ErrHand:
  MsgBox Err.Description & vbCr _
       & "Main: " & vbCr _
       & "  curToken = " & curToken & vbCr _
       & "  res = " & res & vbCr _
       & "  ExitCode = " & ExitCode & vbCr _
       & "  NoTokenCount = " & NoTokenCount & vbCr _
       & "  ParentID = " & ParentID & vbCr _
       & "  pipeBuffer = " & pipeBuffer$ & vbCr, vbCritical, "HspfEngine"

End Sub

Private Sub GotToken(str$)
  Dim FirstWord$, Rest$, spcindex&

  On Error GoTo ErrHand
  
  FirstWord = Trim(str)
  If Len(FirstWord) > 0 Then
    spcindex = InStr(1, FirstWord, " ")
    If spcindex > 0 Then
      Rest = Mid(FirstWord, spcindex + 1)
      FirstWord = Left(FirstWord, spcindex - 1)
    Else
      Rest = ""
    End If
    ReceiveMessage FirstWord, Rest
  End If
  Exit Sub
ErrHand:
  MsgBox Err.Description & vbCr & "GotToken(" & str & ")", vbCritical, "HspfEngine"
End Sub

Private Sub ReceiveMessage(FirstWord As String, Rest As String)
  Dim s As String, m As String
  Static WdmName As String, msgUnit As Long
  Static UciName As String
  Static WdmUnit(4) As Long
  Static Status As Long
  
  Dim i As Long, r As Long
  
  On Error GoTo ErrHand
  
  s = Rest
  
  Select Case UCase(FirstWord)
  Case "W99OPN"
    WriteStatus "DBG F90_W99OPN"
    Call F90_W99OPN  'open error file for fortan problems
  Case "WDBFIN"
    WriteStatus "DBG F90_WDBFIN"
    Call F90_WDBFIN  'initialize WDM record buffer
  Case "PUTOLV"
    WriteStatus "DBG F90_PUTOLV"
    i = StrRetRem(s)
    Call F90_PUTOLV(i)
  Case "SPIPH"
    pParentInPipeHandle = StrRetRem(s)
    pParentOutPipeHandle = StrRetRem(s)
    WriteStatus "(DBG F90_SPIPH)"
    Call F90_SPIPH(pStatusInPipeHandle, pStatusOutPipeHandle)
  Case "WDBOPN"
    WdmName = StrRetRem(s)
    i = StrRetRem(s)
    WriteStatus "(DBG F90_WDBOPN(" & i & ", " & WdmName & ", " & Len(WdmName) & "))"
    r = F90_WDBOPN(i, WdmName, Len(WdmName))
    If InStr(LCase(WdmName), "hspfmsg.wdm") Then msgUnit = r
    WriteStatus "(DBG wdmUnit " & CStr(r) & ")"
    WriteParent "WDBOPN complete, unit " & r
  Case "ACTIVATE"
    m = "begin activate"
    F90_FILSTA m, Len(m)
    UciName = StrRetRem(s)
    'MsgBox "HSPFEngine ACTIVATE entry:" & UciName & ":" & s
    If Len(s) > 0 Then
      i = s '-1:skip interp
    Else
      i = 0
    End If
    WriteStatus "DBG F90_ACTSCN(" & i & ", " & WdmUnit(1) & ", " & msgUnit & ", " & Status & ", " & UciName & ", " & Len(UciName)
    Call F90_ACTSCN(i, WdmUnit(1), msgUnit, Status, UciName, Len(UciName))
    'MsgBox "HSPFEngine ACTIVATE exit:" & Status
    m = "after activate, retcod:" & Status & " i:" & i & vbCrLf
    F90_FILSTA m, Len(m)
    m = "ACTIVATE complete " & Status & vbCrLf
    WriteParent m
    DoEvents
    'MsgBox M, vbOKOnly, "HSPF Engine"
  Case "SIMULATE"
    'MsgBox "HSPFEngine SIMULATE status:" & Status
    If Status = 0 Then 'no previous problems
      frmDummy.timerPipe.Enabled = True
      WriteStatus "DBG F90_SIMSCN"
      F90_SIMSCN Status
      WriteParent "SIMULATE complete " & Status
      DoEvents
    End If
  Case "XBLOCK"
    Dim blk&, Init&, retkey&, cbuff$, retcod&
    blk = StrRetRem(Rest)
    Init = StrRetRem(Rest)
    F90_XBLOCK blk, Init, retkey, cbuff, retcod
    WriteParent blk & " " & Init & " " & retkey & " " & " " & retcod & " " & cbuff
  Case "XBLOCKEX"
    Dim rectyp& ',blk&, init&, retkey&, cbuff$, retcod&
    blk = StrRetRem(Rest)
    Init = StrRetRem(Rest)
    retkey = StrRetRem(Rest)
    F90_XBLOCKEX blk, Init, retkey, cbuff, rectyp, retcod
    WriteParent blk & " " & Init & " " & retkey & " " & " " & rectyp & " " & retcod & " " & cbuff
  Case "GTNXKW"
    Dim id&, ckwd$, kwdfg&, contfg&, retid&  ',init&
    Init = StrRetRem(Rest)
    id = StrRetRem(Rest)
    F90_GTNXKW Init, id, ckwd, kwdfg, contfg, retid
    WriteParent Init & " " & id & " " & kwdfg & " " & contfg & " " & retid & " " & ckwd
  Case "GLOBLK"
    Dim sdatim&(5), edatim&(5), outlev&, spout&, runfg&, emfg&, rninfo$
    F90_GLOBLK sdatim, edatim, outlev, spout, runfg, emfg, rninfo
    m = ""
    For i = 0 To 5
      m = m & sdatim(i) & " "
    Next
    For i = 0 To 5
      m = m & edatim(i) & " "
    Next
    WriteParent m & outlev & " " & spout & " " & runfg & " " & emfg & " " & rninfo
  Case "GLOPRMI"
    Dim iVal&, parmname$
    parmname = StrRetRem(s)
    F90_GLOPRMI iVal, parmname, Len(parmname)
    WriteParent CStr(iVal)
  Case "GETOCR"
    Dim itype&, noccur&
    itype = Rest
    F90_GETOCR itype, noccur
    WriteParent itype & " " & noccur
  Case "XTABLEEX"
    Dim omcode&, tabno&, uunits&, addfg&, Occur& ' , Init&, retkey&, cbuff$, rectyp&, retcod&
    'MsgBox "XTABLEEX " & Rest
    omcode = StrRetRem(Rest)
    tabno = StrRetRem(Rest)
    uunits = StrRetRem(Rest)
    Init = StrRetRem(Rest)
    addfg = StrRetRem(Rest)
    Occur = StrRetRem(Rest)
    retkey = StrRetRem(Rest)
    'MsgBox "XTABLEEX " & OmCode & " " & tabno & " " & uunits & " " & Init & " " & addfg & " " & Occur & " " & retkey, vbOKOnly, "HSPF Engine"
    F90_XTABLEEX omcode, tabno, uunits, Init, addfg, Occur, retkey, cbuff, rectyp, retcod
    WriteParent retkey & " " & rectyp & " " & retcod & " " & cbuff
  Case "CURDIR"
    ChDrive Left(Rest, 2)
    ChDir Rest
    WriteStatus "CURDIR:" & CurDir
  Case "SET_INTEG_DRIVER"
    Dim integfg&, integts!, integFile$
    integfg = StrRetRem(Rest) ' 1 = do only integts rather than whole run
    integts = StrRetRem(Rest)  'in days
    integFile = StrRetRem(Rest)
    F90_SET_DRIVER integfg, integts, integFile, Len(integFile)
  Case "SETDBG"
    Dim debuglev&
    debuglev = StrRetRem(Rest)
    F90_SCNDBG debuglev
  Case "EXIT":
    End '******************************************************
  Case Else
    MsgBox "Unknown Command:" & FirstWord, vbExclamation, "HSPF Engine"
    WriteParent FirstWord & ":Unknown"
  End Select
  DoEvents 'Allow display to update
  Exit Sub

ErrHand:
  MsgBox Err.Description & vbCr & "ReceiveMessage(" & FirstWord & " " & Rest & ")", vbCritical, "HspfEngine"
End Sub

Private Sub WriteStatus(s$)
  WriteTokenToPipe pStatusOutPipeHandle, s
End Sub
Private Sub WriteParent(s$)
  WriteTokenToPipe pParentOutPipeHandle, s
End Sub

Public Sub ProcessMessages()
  Dim ExitCode As Long, res As Long
  Static pipeBuffer$
  
  res = GetExitCodeProcess(ParentProcess, ExitCode)
  If ExitCode = &H103 Then 'front end STILL_ACTIVE
    GotToken ReadTokenFromPipe(pStatusInPipeHandle, pipeBuffer, False)
    DoEvents
  End If
End Sub
