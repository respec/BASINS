Attribute VB_Name = "utilPipe"
Option Explicit
'##MODULE_REMARKS Copyright 2001-3 AQUA TERRA Consultants - Royalty-free use permitted under open source license

Private Declare Function PeekNamedPipe Lib "Kernel32" (ByVal hPipe As Long, lpBuffer As Any, ByVal nBufferSize As Long, lpBytesRead As Long, lpTotalBytesAvail As Long, lpBytesLeftThisMessage As Long) As Long 'not used for *named* pipe here, function should be called PeekPipe
Private Declare Function ReadFile Lib "Kernel32" (ByVal hFile As Long, lpBuffer As Any, ByVal nNumberOfBytesToRead As Long, lpNumberOfBytesRead As Long, ByVal lpOverlapped As Long) As Long

Private pRecording As Boolean
Private pRecordingFilename As String

Public Sub RecordPipeTraffic(recording As Boolean, Optional Filename As String = "")
  pRecording = recording
  If recording Then pRecordingFilename = Filename
End Sub

Private Sub LogPipeMessage(writing As Boolean, pipename As String, Msg As String)
  Dim buf As String
  On Error GoTo ErrorWriting
  If writing Then buf = ": Wrote to  " Else buf = ": Read from "
  buf = Time & buf & pipename & " " & Msg
  If Len(pRecordingFilename) > 0 Then
    Dim outf As Integer
    outf = FreeFile
    Open pRecordingFilename For Append As outf
    Print #outf, buf
    Close #outf
  Else
    Debug.Print buf
  End If
  
  Exit Sub
ErrorWriting:
  MsgBox "Error writing pipe logging file '" & pRecordingFilename & "'" & vbCr & Err.Description
End Sub

'pipename is just used in logging, no particular value is necessary
'Can use hProcess = 0 to skip testing whether a process is alive before sending message
Public Function WriteTokenToPipe(hPipe As Long, ByVal Msg As String, Optional pipename As String = "Unnamed", Optional hProcess As Long = 0) As Boolean
  Dim written&, byt() As Byte, ch&, Length&
  Dim OpenParenEscape As String
  Dim CloseParenEscape As String
  Dim lpExitCode As Long
  
  OpenParenEscape = Chr(6)
  CloseParenEscape = Chr(7)
  WriteTokenToPipe = False
  
  If hProcess > 0 Then
    GetExitCodeProcess hProcess, lpExitCode
    If lpExitCode <> &H103 Then
      Exit Function 'Process at other end of pipe is dead, stop talking to it
    End If
  End If
  
  
  If hPipe <> 0 Then
    If Left(Msg, 1) = "(" And Right(Msg, 1) = ")" Then Msg = Mid(Msg, 2, Len(Msg) - 2)
    Msg = ReplaceString(Msg, "(", OpenParenEscape)
    Msg = ReplaceString(Msg, ")", CloseParenEscape)
    If Len(Msg) > 0 Then
      If Asc(Right(Msg, 1)) > 31 Then Msg = "(" & Msg & ")"
      
      If pRecording Then LogPipeMessage True, pipename, Msg
      
      Length = Len(Msg)
      If Length > 0 Then
        ReDim byt(Length - 1)
        For ch = 1 To Length
          byt(ch - 1) = Asc(Mid(Msg, ch, 1))
        Next ch
        WriteFile hPipe, byt(0), Length, written, 0
      End If
    End If
    WriteTokenToPipe = True
  End If
End Function

'pipename is just used in logging, no particular value is necessary
'Can use hProcess = 0 to skip testing whether a process is alive before sending message
Public Function ReadTokenFromPipe(hPipe As Long, ByRef pipeBuffer As String, Optional ByVal Block As Single = 86400, Optional pipename As String = "Unnamed", Optional hProcess As Long = 0) As String
  Dim res As Long, ExitCode As Long
  Dim freshFromPipe As String
  Dim retval As String
  Dim ch As String, chindex As Long
  Dim lread As Long, lavail As Long, lmessage As Long
  Dim OpenParenEscape As String
  Dim CloseParenEscape As String
  Dim lpExitCode As Long
  Dim startTime As Single
  Dim nowTime As Single
  
  If Block > 0 Then startTime = Timer
  
  OpenParenEscape = Chr(6)
  CloseParenEscape = Chr(7)
   
  'Debug.Print "waitingForPipe:" & hPipe
  
LookForMessageEnd:
  If Len(pipeBuffer) > 0 Then
    chindex = InStr(pipeBuffer, "(")
    If chindex > 0 Then 'Looks like nicely parenthesized tokens
      chindex = InStr(chindex, pipeBuffer, ")")
      If chindex > 0 Then
        retval = Left(pipeBuffer, chindex - 1)
        pipeBuffer = Mid(pipeBuffer, chindex + 1)
        'retval = replacestring(retval, ")", "")
        retval = ReplaceString(retval, "(", "")
        retval = ReplaceString(retval, OpenParenEscape, "(")
        retval = ReplaceString(retval, CloseParenEscape, ")")
      End If
    Else 'Looks like we have something, but not with parens around it
      retval = pipeBuffer
      retval = ReplaceString(retval, vbCr, "")
      retval = ReplaceString(retval, vbLf, "")
      pipeBuffer = ""
    End If
  End If
  If Len(retval) = 0 Then
    DoEvents
    res = PeekNamedPipe(hPipe, ByVal 0&, 0, lread, lavail, lmessage)
    If res <> 0 And lavail > 0 Then
      ReDim InBuf(lavail) As Byte
      res = ReadFile(hPipe, InBuf(0), lavail, lread, 0)
      freshFromPipe = BytesToString(InBuf, lavail)
      
      If pRecording Then LogPipeMessage False, pipename, freshFromPipe
      
      pipeBuffer = pipeBuffer & freshFromPipe
      GoTo LookForMessageEnd
    Else
      Sleep 10
      If hProcess > 0 Then
        GetExitCodeProcess hProcess, lpExitCode
        If lpExitCode <> &H103 Then
          retval = pipename & " exited with code " & lpExitCode
          GoTo StopReading
        End If
      End If
      If Block > 0 Then
        nowTime = Timer
        If nowTime < startTime Then nowTime = nowTime + 86400 'In case we are running at midnight
        If nowTime - startTime < Block Then GoTo LookForMessageEnd
        retval = "Time expired waiting for a message from '" & pipename & "' pipe for " & Block & " seconds"
        'Debug.Print retval
      End If
    End If
  End If
StopReading:
  ReadTokenFromPipe = retval
  'Debug.Print "fromPipe:" & retval
End Function

'Remove nulls from double-byte ascii or leave normal ascii alone
Private Function BytesToString(byt() As Byte, nBytes As Long) As String
  Dim s$, ch&
  s = ""
  For ch = 0 To nBytes - 1
    If byt(ch) <> 0 Then s = s & Chr$(byt(ch))
  Next ch
  BytesToString = s
End Function
