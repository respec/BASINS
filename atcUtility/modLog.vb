Option Strict Off
Option Explicit On 

Imports System.IO
Imports System.Windows.Forms

Public Module modLog
  Private pMapWin As Object 'MapWindow.Interfaces.IMapWin
  Private pFile As Integer = -1
  Private pFileName As String = ""
  Private pTimeStamp As Boolean = True
  Private pProgressStartTime As Double = Double.NaN
  Private pProgressLastUpdate As Double = Double.NaN
  Private pProgressRefresh As Double = 2 / 720000.0# ' 1 / 720000.0 = 0.1 second
  Private pStatusMonitorPID As Integer = -1
  Private pText As String = "" 'Buffer messages here if needed while log file is busy
  Private pNumLogsQueued As Integer = 0

  Public Sub LogSetFileName(ByVal aLogFileName As String, Optional ByVal aAppend As Boolean = False)
    If Not aLogFileName.Equals(pFileName) Then
      If pFile >= 0 Then 'Close the already-open file
        Try
          FileClose(pFile)
        Catch ex As Exception
        End Try
        pFile = -1
      End If
      If Not aAppend AndAlso FileExists(aLogFileName) Then Kill(aLogFileName)
      pFileName = aLogFileName
    End If
  End Sub

  Public Sub LogSetTimeStamp(ByVal aLogTimeStamp As Boolean)
    pTimeStamp = aLogTimeStamp
  End Sub

  Public Sub LogSetMapWin(ByVal aMapWin As Object) 'MapWindow.Interfaces.IMapWin)
    pMapWin = aMapWin
  End Sub

  Public Sub LogFlush()
    If pFile = -1 AndAlso pFileName.Length > 0 Then
      MkDirPath(PathNameOnly(pFileName))
      pFile = FreeFile()
      FileOpen(pFile, pFileName, OpenMode.Append, OpenAccess.Write, OpenShare.LockWrite)
    End If
    FileClose(pFile)
    FileOpen(pFile, pFileName, OpenMode.Append, OpenAccess.Write, OpenShare.LockWrite)
  End Sub

  Public Sub LogDbg(ByRef aMessage As String)  'Log a debugging trace message
    Dim lText As String = pText
    If pTimeStamp Then
      With Now
        lText &= Format(.Hour, "00") & ":" & _
                 Format(.Minute, "00") & ":" & _
                 Format(.Second, "00") & "." & _
                 Format(.Millisecond, "000") & vbTab
      End With
    End If
    lText &= aMessage
    If pNumLogsQueued > 0 Then lText &= "Queued: " & pNumLogsQueued
    Try
      If pFileName.Length = 0 Then
        Debug.WriteLine(lText)
      Else
        If pFile = -1 Then LogFlush()
        PrintLine(pFile, lText)
      End If
      pText = ""
      pNumLogsQueued = 0
    Catch ex As Exception
      pText = lText & vbCrLf
      pNumLogsQueued += 1
      Debug.WriteLine(pNumLogsQueued)
    End Try
  End Sub

  'Log the use of a message box (using pMsg As ATCoMessage)
  Public Function LogMsg(ByRef aMessage As String, ByRef aTitle As String, ByVal ParamArray aButtonName() As Object) As Integer
    'Dim lButtonName() As Object

    LogDbg("LogMsg:" & aTitle & ":" & aMessage & ":" & UBound(aButtonName) + 1)
    MsgBox(aMessage, MsgBoxStyle.OKOnly, aTitle)
    Return 1
    'If UBound(aButtonName) < 0 Then
    '    ReDim lButtonName(0)
    '    lButtonName(0) = "&OK"
    'Else
    '    lButtonName = VB6.CopyArray(aButtonName)
    'End If

    'LogMsg = pMsg.ShowArray(aMessage, aTitle, lButtonName)
    'If UBound(lButtonName) > 0 Then
    '    If (LogMsg > 0) Then
    '        LogDbg("LogMsg:UserSelectedButton:" & lButtonName(LogMsg - 1))
    '    Else
    '        LogDbg("LogMsg:UserEscaped")
    '    End If
    'End If
  End Function

  Public Sub LogStartMonitor()
    If pStatusMonitorPID = -1 Then
      Dim exe As String = FindFile("Please locate StatusMonitor.exe", "StatusMonitor.exe")
      If exe.Length > 0 Then
        Dim lStartCmdLine As String = """" & exe & """ " & Process.GetCurrentProcess.Id & " " & pFileName
        LogDbg("StartMonitor " & lStartCmdLine)
        LogFlush()
        pStatusMonitorPID = Shell(lStartCmdLine, AppWinStyle.NormalNoFocus)
      Else
        LogDbg("StartMonitor Could not find StatusMonitor.exe")
      End If
    Else
      LogDbg("StartMonitor - Already started PID " & pStatusMonitorPID)
    End If
  End Sub

  Public Sub LogStopMonitor()
    If pStatusMonitorPID > -1 Then
      LogDbg("EXIT")
      pStatusMonitorPID = 0
    End If
  End Sub

  Public Sub LogProgress(ByRef aMessage As String, ByRef aCurrent As Integer, ByRef aLast As Integer)
    LogDbg("Progress " & aMessage & " " & aCurrent & " of " & aLast)
    Dim lCurTime As Double = Now.ToOADate
    If aCurrent = aLast Then
      pProgressStartTime = Double.NaN
      pProgressLastUpdate = Double.NaN
      If Not pMapWin Is Nothing Then pMapWin.StatusBar.ShowProgressBar = False
      LogFlush()
    ElseIf Double.IsNaN(pProgressStartTime) Then
      pProgressStartTime = lCurTime
      pProgressLastUpdate = lCurTime
      If Not pMapWin Is Nothing Then
        pMapWin.StatusBar.ProgressBarValue = 0
        pMapWin.StatusBar.ShowProgressBar = True
      End If
    Else
      If lCurTime - pProgressLastUpdate > pProgressRefresh Then
        LogFlush()
        If Not pMapWin Is Nothing Then
          pMapWin.StatusBar.ProgressBarValue = 100 * aCurrent / aLast
        End If
        'MsgBox(aMessage & vbCrLf & aCurrent & " of " & aLast & " (" & 100 * aCurrent / aLast & "%)")
        pProgressLastUpdate = lCurTime
      End If
    End If
  End Sub

  ''Send a command to the status monitor
  'Public Sub LogCmd(ByRef aCommand As String)
  '  System.Diagnostics.Debug.WriteLine(aCommand)
  '  'If Not (gIPC Is Nothing) Then gIPC.SendMonitorMessage((aCommand))
  'End Sub

  Public Sub LogSendFeedback()
    'Dim iDrive As Integer
    'Dim allFiles As New FColl.FastCollection()
    'Dim vFilename As Object
    'Dim lFeedback As ATCoFeedback.clsATCoFeedback

    'lFeedback = New ATCoFeedback.clsATCoFeedback()

    'For iDrive = 1 To Len(pBasinsDrives)
    '    AddFilesInDir(allFiles, Mid(pBasinsDrives, iDrive, 1) & ":\Basins\", True, "unins*.dat")
    'Next

    'For Each vFilename In allFiles
    '    lFeedback.AddFile(CStr(vFilename))
    'Next vFilename

    'lFeedback.AddText(WholeFileString(pFileName))
    ''lFeedback.Show(App, frmMain.DefInstance.Icon)
  End Sub

  'Friend Sub Tracer()
  '  Dim texttoadd As String
  '  Dim logtext() As String
  '  Dim fileline() As String
  '  Dim fs As StreamWriter
  '  Dim strace As New StackTrace(True)
  '  Try
  '    If Not File.Exists(TRACE_LOG) Then
  '      fs = File.CreateText(TRACE_LOG)
  '      fs.Write("Trace Log " & Format(Now) & vbCr & vbCr)
  '      fs.Flush()
  '      fs.Close()
  '    End If
  '    logtext = strace.GetFrame(1).ToString.Split(" ")
  '    fileline = logtext(6).Split("\")
  '    Dim i As Integer = fileline.GetUpperBound(0)
  '    texttoadd = logtext(0) & ": " & _
  '         fileline(i).Substring(0, fileline(i).Length - 2)
  '    fs = File.AppendText(TRACE_LOG)
  '    fs.WriteLine(texttoadd)
  '    fs.Flush()
  '    fs.Close()
  '  Catch ex As Exception
  '    MsgBox(ex.ToString)
  '  End Try
  'End Sub

End Module
