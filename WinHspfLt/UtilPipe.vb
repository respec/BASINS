Option Strict Off
Option Explicit On

Imports System.Threading
Imports Microsoft.VisualBasic
Imports atcUtility

Module utilPipe
    '##MODULE_REMARKS Copyright 2001-3 AQUA TERRA Consultants - Royalty-free use permitted under open source license

    'UPGRADE_ISSUE: Declaring a parameter 'As Any' is not supported. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="FAE78A8D-8978-4FD4-8208-5B7324A8F795"'
    'Private Declare Function PeekNamedPipe Lib "Kernel32" (ByVal hPipe As Integer, ByRef lpBuffer As Any, ByVal nBufferSize As Integer, ByRef lpBytesRead As Integer, ByRef lpTotalBytesAvail As Integer, ByRef lpBytesLeftThisMessage As Integer) As Integer 'not used for *named* pipe here, function should be called PeekPipe
    'UPGRADE_ISSUE: Declaring a parameter 'As Any' is not supported. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="FAE78A8D-8978-4FD4-8208-5B7324A8F795"'
    'Private Declare Function ReadFile Lib "Kernel32" (ByVal hFile As Integer, ByRef lpBuffer As Any, ByVal nNumberOfBytesToRead As Integer, ByRef lpNumberOfBytesRead As Integer, ByVal lpOverlapped As Integer) As Integer

    Private pRecording As Boolean
    Private pRecordingFilename As String

    '    Public Sub RecordPipeTraffic(ByRef recording As Boolean, Optional ByRef Filename As String = "")
    '        pRecording = recording
    '        If recording Then pRecordingFilename = Filename
    '    End Sub

    '    Private Sub LogPipeMessage(ByRef writing As Boolean, ByRef pipename As String, ByRef Msg As String)
    '        Dim buf As String
    '        On Error GoTo ErrorWriting
    '        If writing Then buf = ": Wrote to  " Else buf = ": Read from "
    '        buf = TimeOfDay & buf & pipename & " " & Msg
    '        Dim outf As Short
    '        If Len(pRecordingFilename) > 0 Then
    '            outf = FreeFile()
    '            FileOpen(outf, pRecordingFilename, OpenMode.Append)
    '            PrintLine(outf, buf)
    '            FileClose(outf)
    '        Else
    '            Debug.Print(buf)
    '        End If

    '        Exit Sub
    'ErrorWriting:
    '        MsgBox("Error writing pipe logging file '" & pRecordingFilename & "'" & vbCr & Err.Description)
    '    End Sub

    '    'pipename is just used in logging, no particular value is necessary
    '    'Can use hProcess = 0 to skip testing whether a process is alive before sending message
    '    Public Function WriteTokenToPipe(ByRef hPipe As Integer, ByVal Msg As String, Optional ByRef pipename As String = "Unnamed", Optional ByRef aProcess As Process = Nothing) As Boolean
    '        Dim ch, written, Length As Integer
    '        Dim byt() As Byte
    '        Dim OpenParenEscape As String
    '        Dim CloseParenEscape As String
    '        Dim lpExitCode As Integer

    '        OpenParenEscape = Chr(6)
    '        CloseParenEscape = Chr(7)
    '        WriteTokenToPipe = False

    '        If Not IsNothing(aProcess) Then
    '            lpExitCode = aProcess.ExitCode
    '            If lpExitCode <> &H103S Then 'TODO: check to be sure codes have not changed
    '                Exit Function 'Process at other end of pipe is dead, stop talking to it
    '            End If
    '        End If


    '        If hPipe <> 0 Then
    '            If Left(Msg, 1) = "(" And Right(Msg, 1) = ")" Then Msg = Mid(Msg, 2, Len(Msg) - 2)
    '            Msg = ReplaceString(Msg, "(", OpenParenEscape)
    '            Msg = ReplaceString(Msg, ")", CloseParenEscape)
    '            If Len(Msg) > 0 Then
    '                If Asc(Right(Msg, 1)) > 31 Then Msg = "(" & Msg & ")"

    '                If pRecording Then LogPipeMessage(True, pipename, Msg)

    '                Length = Len(Msg)
    '                If Length > 0 Then
    '                    ReDim byt(Length - 1)
    '                    For ch = 1 To Length
    '                        byt(ch - 1) = Asc(Mid(Msg, ch, 1))
    '                    Next ch
    '                    WriteFile(hPipe, byt(0), Length, written, 0)
    '                End If
    '            End If
    '            WriteTokenToPipe = True
    '        End If
    '    End Function

    'pipename is just used in logging, no particular value is necessary
    'Can use hProcess = 0 to skip testing whether a process is alive before sending message
    '    Public Function ReadTokenFromPipe(ByRef hPipe As Integer, ByRef pipeBuffer As String, Optional ByVal Block As Single = 86400, Optional ByRef pipename As String = "Unnamed", Optional ByRef aProcess As Process = Nothing) As String
    '        Dim res, ExitCode As Integer
    '        Dim freshFromPipe As String
    '        Dim retval As String
    '        Dim ch As String
    '        Dim chindex As Integer
    '        Dim lavail, lread, lmessage As Integer
    '        Dim OpenParenEscape As String
    '        Dim CloseParenEscape As String
    '        Dim lpExitCode As Integer
    '        Dim startTime As Single
    '        Dim nowTime As Single

    '        If Block > 0 Then
    '            Dim lTimer As Timer
    '            'startTime = VB.Timer()
    '        End If

    '        OpenParenEscape = Chr(6)
    '        CloseParenEscape = Chr(7)

    '        'Debug.Print "waitingForPipe:" & hPipe

    'LookForMessageEnd:
    '        If Len(pipeBuffer) > 0 Then
    '            chindex = InStr(pipeBuffer, "(")
    '            If chindex > 0 Then 'Looks like nicely parenthesized tokens
    '                chindex = InStr(chindex, pipeBuffer, ")")
    '                If chindex > 0 Then
    '                    retval = Left(pipeBuffer, chindex - 1)
    '                    pipeBuffer = Mid(pipeBuffer, chindex + 1)
    '                    'retval = replacestring(retval, ")", "")
    '                    retval = ReplaceString(retval, "(", "")
    '                    retval = ReplaceString(retval, OpenParenEscape, "(")
    '                    retval = ReplaceString(retval, CloseParenEscape, ")")
    '                End If
    '            Else 'Looks like we have something, but not with parens around it
    '                retval = pipeBuffer
    '                retval = ReplaceString(retval, vbCr, "")
    '                retval = ReplaceString(retval, vbLf, "")
    '                pipeBuffer = ""
    '            End If
    '        End If
    '        If Len(retval) = 0 Then
    '            System.Windows.Forms.Application.DoEvents()
    '            res = PeekNamedPipe(hPipe, 0, 0, lread, lavail, lmessage)
    '            If res <> 0 And lavail > 0 Then
    '                Dim InBuf(lavail) As Byte
    '                res = ReadFile(hPipe, InBuf(0), lavail, lread, 0)
    '                freshFromPipe = BytesToString(InBuf, lavail)

    '                If pRecording Then LogPipeMessage(False, pipename, freshFromPipe)

    '                pipeBuffer = pipeBuffer & freshFromPipe
    '                GoTo LookForMessageEnd
    '            Else
    '                Windows.Forms.Application.DoEvents()
    '                System.Threading.Thread.Sleep(10)
    '                If Not aProcess Is Nothing > 0 Then
    '                    lpExitCode = aProcess.ExitCode
    '                    If lpExitCode <> &H103S Then
    '                        retval = pipename & " exited with code " & lpExitCode
    '                        GoTo StopReading
    '                    End If
    '                End If
    '                If Block > 0 Then
    '                    'nowTime = VB.Timer()
    '                    If nowTime < startTime Then nowTime = nowTime + 86400 'In case we are running at midnight
    '                    If nowTime - startTime < Block Then GoTo LookForMessageEnd
    '                    retval = "Time expired waiting for a message from '" & pipename & "' pipe for " & Block & " seconds"
    '                    'Debug.Print retval
    '                End If
    '            End If
    '        End If
    'StopReading:
    '        ReadTokenFromPipe = retval
    '        'Debug.Print "fromPipe:" & retval
    '    End Function

    'Remove nulls from double-byte ascii or leave normal ascii alone
    Private Function BytesToString(ByRef byt() As Byte, ByRef nBytes As Integer) As String
        Dim s As String
        Dim ch As Integer
        s = ""
        For ch = 0 To nBytes - 1
            If byt(ch) <> 0 Then s = s & Chr(byt(ch))
        Next ch
        BytesToString = s
    End Function
End Module


Public Class TimerExample

    <MTAThread()> _
    Shared Sub Main()

        Dim autoEvent As New AutoResetEvent(False)
        Dim statusChecker As New StatusChecker(10)

        ' Create the delegate that invokes methods for the timer.
        Dim timerDelegate As TimerCallback = _
            AddressOf statusChecker.CheckStatus

        ' Create a timer that signals the delegate to invoke 
        ' CheckStatus after one second, and every 1/4 second 
        ' thereafter.
        Console.WriteLine("{0} Creating timer." & vbCrLf, _
            DateTime.Now.ToString("h:mm:ss.fff"))
        Dim stateTimer As Timer = _
                New Timer(timerDelegate, autoEvent, 1000, 250)

        ' When autoEvent signals, change the period to every 
        ' 1/2 second.
        autoEvent.WaitOne(5000, False)
        stateTimer.Change(0, 500)
        Console.WriteLine(vbCrLf & "Changing period." & vbCrLf)

        ' When autoEvent signals the second time, dispose of 
        ' the timer.
        autoEvent.WaitOne(5000, False)
        stateTimer.Dispose()
        Console.WriteLine(vbCrLf & "Destroying timer.")

    End Sub
End Class

Public Class StatusChecker

    Dim invokeCount, maxCount As Integer

    Sub New(ByVal count As Integer)
        invokeCount = 0
        maxCount = count
    End Sub

    ' This method is called by the timer delegate.
    Sub CheckStatus(ByVal stateInfo As Object)
        Dim autoEvent As AutoResetEvent = _
            DirectCast(stateInfo, AutoResetEvent)
        invokeCount += 1
        Console.WriteLine("{0} Checking status {1,2}.", _
            DateTime.Now.ToString("h:mm:ss.fff"), _
            invokeCount.ToString())

        If invokeCount = maxCount Then

            ' Reset the counter and signal to stop the timer.
            invokeCount = 0
            autoEvent.Set()
        End If
    End Sub

End Class

