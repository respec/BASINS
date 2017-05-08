Attribute VB_Name = "utilPipe"
Option Explicit
'Copyright 2001 by AQUA TERRA Consultants

Private Declare Function PeekNamedPipe Lib "Kernel32" (ByVal hPipe As Long, lpBuffer As Any, ByVal nBufferSize As Long, lpBytesRead As Long, lpTotalBytesAvail As Long, lpBytesLeftThisMessage As Long) As Long 'not used for *named* pipe here, function should be called PeekPipe
Private Declare Function ReadFile Lib "Kernel32" (ByVal hFile As Long, lpBuffer As Any, ByVal nNumberOfBytesToRead As Long, lpNumberOfBytesRead As Long, ByVal lpOverlapped As Long) As Long

Public Sub WriteTokenToPipe(hPipe As Long, ByVal Msg As String, _
                            Optional startDelim As String = "(", _
                            Optional endDelim As String = ")")
  Dim written&, byt() As Byte, ch&, Length&
  Dim OpenParenEscape As String
  Dim CloseParenEscape As String
  
  OpenParenEscape = Chr(6)
  CloseParenEscape = Chr(7)
  
'  If asc(Right(msg, 1)) < 32 Then
'    startDelim = ""
'    endDelim = Right(msg, 1)
'    If asc(Right(msg, 2)) < 32 Then
'      endDelim = Right(msg, 2)
'    End If
'  End If
  If hPipe <> 0 Then
    If Left(Msg, Len(startDelim)) = startDelim And Right(Msg, Len(endDelim)) = endDelim Then
      Msg = Mid(Msg, Len(startDelim) + 1, Len(Msg) - Len(startDelim) - Len(endDelim))
    End If
    If Len(startDelim) > 0 Then
      Msg = ReplaceString(Msg, startDelim, OpenParenEscape)
    End If
    If Len(endDelim) > 0 Then
      Msg = ReplaceString(Msg, endDelim, CloseParenEscape)
    End If
    Msg = startDelim & Msg & endDelim
    Length = Len(Msg)
    If Length > 0 Then
      ReDim byt(Length - 1)
      For ch = 1 To Length
        byt(ch - 1) = asc(Mid(Msg, ch, 1))
      Next ch
      WriteFile hPipe, byt(0), Length, written, 0
    End If
  End If
End Sub

Public Function ReadTokenFromPipe(hPipe As Long, ByRef pipeBuffer As String, _
                            Optional Block As Boolean = True, _
                            Optional startDelim As String = "(", _
                            Optional endDelim As String = ")") As String
  Dim res As Long, ExitCode As Long
  Dim freshFromPipe As String
  Dim retval As String
  Dim ch As String, chindex As Long
  Dim lread As Long, lavail As Long, lmessage As Long
  Dim OpenParenEscape As String
  Dim CloseParenEscape As String
  
  OpenParenEscape = Chr(6)
  CloseParenEscape = Chr(7)
  
LookForMessageEnd:
  If Len(endDelim) = 0 Then
    chindex = Len(pipeBuffer) + 1
  Else
    chindex = InStr(pipeBuffer, endDelim)
  End If
  If chindex > 0 And Len(pipeBuffer) > 0 Then
    retval = Left(pipeBuffer, chindex - 1)
    pipeBuffer = Mid(pipeBuffer, chindex + 1)
    'retval = replacestring(retval, ")", "")
    retval = ReplaceString(retval, startDelim, "")
    retval = ReplaceString(retval, OpenParenEscape, startDelim)
    retval = ReplaceString(retval, CloseParenEscape, endDelim)
  Else
    DoEvents
    Sleep 10
    res = PeekNamedPipe(hPipe, ByVal 0&, 0, lread, lavail, lmessage)
    If res <> 0 And lavail > 0 Then
      ReDim InBuf(lavail) As Byte
      res = ReadFile(hPipe, InBuf(0), lavail, lread, 0)
      pipeBuffer = pipeBuffer & BytesToString(InBuf, lavail)
      GoTo LookForMessageEnd
    Else
      If Block Then GoTo LookForMessageEnd
    End If
  End If
  
  ReadTokenFromPipe = retval
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

Private Function ReplaceString(Source$, find$, replace$) As String
  Dim retval$, findpos&, replen&, finlen&
  replen = Len(replace)
  finlen = Len(find)
  If finlen > 0 Then
    retval = Source
    findpos = InStr(retval, find)
    While findpos > 0
      retval = Left(retval, findpos - 1) & replace & Mid(retval, findpos + finlen)
      findpos = InStr(findpos + replen, retval, find)
    Wend
    ReplaceString = retval
  Else
    ReplaceString = Source
  End If
End Function



