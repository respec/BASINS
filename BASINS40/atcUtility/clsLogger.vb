Option Strict Off
Option Explicit On 

Imports System.IO
Imports System.Windows.Forms

Public Class Logger
  Private Shared pMapWin As Object 'MapWindow.Interfaces.IMapWin
  Private Shared pFile As Integer = -1 'File unit number - default is none
  Private Shared pTimeStamp As Boolean = True
  Private Shared pProgressStartTime As Double = Double.NaN
  Private Shared pProgressLastUpdate As Double = Double.NaN
  Private Shared pProgressRefresh As Double = 2 / 720000.0# ' 1 / 720000.0 = 0.1 second
  Private Shared pStatusMonitorPID As Integer = -1
  Private Shared pText As String = "" 'Buffer messages here if needed while log file is busy
  Private Shared pNumLogsQueued As Integer = 0 'count messages in Buffer
  Private Shared pFirstCall As Boolean = True
  Private Shared pFileName As String = "" 'file to write logs to 

  Public Shared Sub StartToFile(ByVal aLogFileName As String, _
                       Optional ByVal aAppend As Boolean = False, _
                       Optional ByVal aRenameExisting As Boolean = True, _
                       Optional ByVal aForceNameChange As Boolean = False)
    If aForceNameChange OrElse pFile = -1 Then 'log file name change is allowed
      If pFile >= 0 Then 'Close the already-open file
        Try
          FileClose(pFile)
        Catch ex As Exception
        End Try
        pFile = -1
      End If

      pFileName = aLogFileName

      If pFileName.Length > 0 Then
        MkDirPath(PathNameOnly(pFileName))
        pFile = FreeFile()

        If FileExists(pFileName) Then
          If aAppend Then
            FileOpen(pFile, pFileName, OpenMode.Append, OpenAccess.Write, OpenShare.LockWrite)
          Else
            If aRenameExisting Then
              Rename(pFileName, MakeLogName(pFileName))
            Else
              Kill(pFileName)
            End If
            FileOpen(pFile, pFileName, OpenMode.Output, OpenAccess.Write, OpenShare.LockWrite)
          End If
        Else
          FileOpen(pFile, pFileName, OpenMode.Output, OpenAccess.Write, OpenShare.LockWrite)
        End If
      End If
    End If
  End Sub

  ''' File name being logged to
  Public Shared ReadOnly Property FileName() As String
    Get
      Return pFileName
    End Get
  End Property

  ''' Append a number to a given file name if needed to make it unique
  Private Shared Function MakeLogName(ByVal aLogFileName As String) As String
    Dim lTryName As String
    Dim lTry As Integer = 1

    Do
      lTryName = FilenameNoExt(pFileName) & "#" & lTry & ".log"
      lTry += 1
    Loop While FileExists(lTryName)
    Return lTryName
  End Function

  ''' True to enable time stamping each log event
  Public Shared Property TimeStamping() As Boolean
    Set(ByVal aLogTimeStamp As Boolean)
      pTimeStamp = aLogTimeStamp
    End Set
    Get
      Return pTimeStamp
    End Get
  End Property

  ''' If MapWin is set to a MapWindow.Interfaces.IMapWin object, progress messages will affect pMapWin.StatusBar 
  Public Shared WriteOnly Property MapWin() As Object
    Set(ByVal aMapWin As Object) 'MapWindow.Interfaces.IMapWin)
      pMapWin = aMapWin
    End Set
  End Property

  ''' Write any pending log messages to the log file (if logging to a file)
  Public Shared Sub Flush()
    If pFile > 0 Then
      FileClose(pFile)
      FileOpen(pFile, pFileName, OpenMode.Append, OpenAccess.Write, OpenShare.LockWrite)
    End If
  End Sub

  ''' Add a "debug" message to the log. 
  ''' Debug messages are commonly used to indicate that execution has reached a certain line.
  ''' Important values just computed or about to be used are often included.
  Public Shared Sub Dbg(ByRef aMessage As String)
    Dim lText As String = pText

    If pTimeStamp Then
      With Now
        'TODO: add timestamp to each record?
        lText &= Format(.Hour, "00") & ":" & _
                 Format(.Minute, "00") & ":" & _
                 Format(.Second, "00") & "." & _
                 Format(.Millisecond, "000") & vbTab
      End With
    End If

    lText &= aMessage

    If pNumLogsQueued > 0 Then
      lText &= "Queued: " & pNumLogsQueued
    End If

    Try
      Debug.WriteLine(lText)
      If pFile >= 0 Then PrintLine(pFile, lText)
      pText = ""
      pNumLogsQueued = 0
    Catch ex As Exception
      pText = lText & vbCrLf
      pNumLogsQueued += 1
      Debug.WriteLine("LogsQueued:" & pNumLogsQueued)
    End Try
  End Sub

  ''' Log the use of a message box 
  Public Shared Function Msg(ByVal aMessage As String, _
                    Optional ByRef aMsgBoxStyle As MsgBoxStyle = MsgBoxStyle.OKOnly, _
                    Optional ByVal aTitle As String = "") As MsgBoxResult
    If aTitle.Length = 0 Then
      If aMessage.IndexOf(":") > 0 Then
        aTitle = StrSplit(aMessage, ":", "")
      End If
    End If

    Dbg("Msg:" & aMessage & ":Title:" & aTitle & ":Style:" & aMsgBoxStyle)
    Flush()
    Dim lResult As MsgBoxResult = MsgBox(aMessage, aMsgBoxStyle, aTitle)
    Dim lResultString As String = ""
    Select Case lResult
      Case MsgBoxResult.Abort : lResultString = "Abort"
      Case MsgBoxResult.Retry : lResultString = "Retry"
      Case MsgBoxResult.Ignore : lResultString = "Ignore"

      Case MsgBoxResult.Yes : lResultString = "Yes"
      Case MsgBoxResult.No : lResultString = "No"

      Case MsgBoxResult.OK : lResultString = "OK"
      Case MsgBoxResult.Cancel : lResultString = "Cancel"
    End Select
    Dbg("MsgResult:" & lResult & ":" & lResultString)
    Return lResult
  End Function

  Public Shared Sub StartMonitor()
    If pStatusMonitorPID = -1 Then
      Dim exe As String = FindFile("Please locate StatusMonitor.exe", "StatusMonitor.exe")
      If exe.Length > 0 Then
        Dim lStartCmdLine As String = """" & exe & """ " & Process.GetCurrentProcess.Id & " " & pFileName
        Dbg("StartMonitor " & lStartCmdLine)
        Flush()
        pStatusMonitorPID = Shell(lStartCmdLine, AppWinStyle.NormalNoFocus)
      Else
        Dbg("StartMonitor Could not find StatusMonitor.exe")
      End If
    Else
      Dbg("LogStartMonitor - Already started PID " & pStatusMonitorPID)
    End If
  End Sub

  Public Shared Sub StopMonitor()
    If pStatusMonitorPID > -1 Then
      Dbg("EXIT")
      pStatusMonitorPID = 0
    End If
  End Sub

  Public Shared Sub Progress(ByRef aMessage As String, ByRef aCurrent As Integer, ByRef aLast As Integer)
    Dbg("Progress " & aMessage & " " & aCurrent & " of " & aLast)
    Dim lCurTime As Double = Now.ToOADate
    If aCurrent = aLast Then
      pProgressStartTime = Double.NaN
      pProgressLastUpdate = Double.NaN
      If Not pMapWin Is Nothing Then
        pMapWin.StatusBar.ShowProgressBar = False
      End If
      Flush()
    ElseIf Double.IsNaN(pProgressStartTime) Then
      pProgressStartTime = lCurTime
      pProgressLastUpdate = lCurTime
      If Not pMapWin Is Nothing Then
        pMapWin.StatusBar.ProgressBarValue = 0
        pMapWin.StatusBar.ShowProgressBar = True
      End If
    Else
      If lCurTime - pProgressLastUpdate > pProgressRefresh Then
        Flush()
        If Not pMapWin Is Nothing Then
          pMapWin.StatusBar.ProgressBarValue = 100 * aCurrent / aLast
        End If
        'Logger.Msg(aMessage & vbCrLf & aCurrent & " of " & aLast & " (" & 100 * aCurrent / aLast & "%)")
        pProgressLastUpdate = lCurTime
      End If
    End If
  End Sub

  ''Send a command to the status monitor
  'Public Sub Cmd(ByRef aCommand As String)
  '  System.Diagnostics.Debug.WriteLine(aCommand)
  '  'If Not (gIPC Is Nothing) Then gIPC.SendMonitorMessage((aCommand))
  'End Sub

  Public Shared Sub SendFeedback()
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
  '    fileline(i).Substring(0, fileline(i).Length - 2)
  '    fs = File.AppendText(TRACE_LOG)
  '    fs.WriteLine(texttoadd)
  '    fs.Flush()
  '    fs.Close()
  '  Catch ex As Exception
  '    MsgBox(ex.ToString)
  '  End Try
  'End Sub

End Class
