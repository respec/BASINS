Option Strict Off
Option Explicit On 

Imports System.IO

Public Module modLog

    'Log a debugging trace message
  Public Sub LogDbg(ByRef msg As String)
    'System.Diagnostics.Debug.WriteLine(msg)

    'MsgBox(CurDir() & vbCrLf & msg)

    Dim lT As String
    Dim lW As StreamWriter = File.AppendText("c:\test\errorVb.fil") 'todo:don't hard code this!
    With Now
      lT = Format(.Hour, "00") & ":" & _
           Format(.Minute, "00") & ":" & _
           Format(.Second, "00") & "." & _
           Format(.Millisecond, "000") & " : "
    End With
    lW.WriteLine(lT & msg)
    lW.Close()

    'F90_MSG(msg, Len(msg)) 'dont leave this in production version, remove modHass_Ent.vb

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

  'Send a command to the status monitor
  Public Sub LogCmd(ByRef aCommand As String)
    System.Diagnostics.Debug.WriteLine(aCommand)
    'If Not (gIPC Is Nothing) Then gIPC.SendMonitorMessage((aCommand))
  End Sub

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
