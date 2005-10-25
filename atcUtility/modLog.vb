Option Strict Off
Option Explicit On 

Imports System.IO
Imports System.Windows.Forms


Public Module modLog
  Private pLogFileName As String = PathNameOnly(Application.ExecutablePath) & "\Basins.Log"
  Private pLogTimeStamp As Boolean = True
  Private pProgressStartTime As Double = Double.NaN
  Private pProgressLastUpdate As Double = Double.NaN
  Private pProgressRefresh As Double = 2 / 72000.0# ' 1 / 72000.0 = 1 second

  Public Sub SetLogFileName(ByVal aLogFileName As String, Optional ByVal aAppend As Boolean = False)
    If Not aAppend AndAlso FileExists(aLogFileName) Then Kill(aLogFileName)
    pLogFileName = aLogFileName
  End Sub

  Public Sub SetLogTimeStamp(ByVal aLogTimeStamp As Boolean)
    pLogTimeStamp = aLogTimeStamp
  End Sub

  Public Sub LogDbg(ByRef aMsg As String)  'Log a debugging trace message
    Dim lT As String
    If pLogTimeStamp Then
      With Now
        lT = Format(.Hour, "00") & ":" & _
             Format(.Minute, "00") & ":" & _
             Format(.Second, "00") & "." & _
             Format(.Millisecond, "000") & " : "
      End With
    End If
    Dim lW As StreamWriter = File.AppendText(pLogFileName)
    lW.WriteLine(lT & aMsg)
    lW.Close()

    'If Not (gIPC Is Nothing) Then gIPC.dbg(msg)

    'If LCase(Left(msg, 6)) = "status" Then frmMain.DefInstance.Status = Trim(Mid(msg, 8))
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

  Public Sub LogProgress(ByRef aMessage As String, ByRef aCurrent As Integer, ByRef aLast As Integer)
    LogDbg("Progress: " & aMessage & " " & aCurrent & " of " & aLast)
    Dim lCurTime As Double = Now.ToOADate
    If aCurrent = aLast Then
      pProgressStartTime = Double.NaN
      pProgressLastUpdate = Double.NaN
    ElseIf Double.IsNaN(pProgressStartTime) Then
      pProgressStartTime = lCurTime
      pProgressLastUpdate = lCurTime
    Else
      If lCurTime - pProgressLastUpdate > pProgressRefresh Then
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

  Public Sub SendFeedback()
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

    'lFeedback.AddText(WholeFileString(pLogFilename))
    ''lFeedback.Show(App, frmMain.DefInstance.Icon)
  End Sub

End Module
