Attribute VB_Name = "modUpdate"
Option Explicit

Sub Main()
  Dim sParentID As String
  Dim ParentID As Long
  Dim hParentProcess As Long
  Dim desiredAccess As Long
  Dim inheritHandle As Long
  Dim ExitCode As Long
  Dim WaitStart As Single
  Dim e As Long
  
  hParentProcess = 0
  'Wait for parent to exit, if it is running
  sParentID = GetSetting("Update", "WaitFor", "PID", Command)
  If Len(sParentID) > 0 Then
    If IsNumeric(sParentID) Then
      ParentID = CLng(sParentID)
      If ParentID <> 0 Then
        desiredAccess = &H400 'PROCESS_QUERY_INFORMATION = &H400
        inheritHandle = False
        hParentProcess = OpenProcess(desiredAccess, inheritHandle, ParentID)
        Do
          WaitStart = Timer
          Do
            DoEvents
            e = GetExitCodeProcess(hParentProcess, ExitCode)
            If ExitCode <> &H103 Then 'still active
              GoTo ParentFinished
            End If
            Sleep 100
          Loop While Timer - WaitStart < 20
        Loop While MsgBox("Parent process (" & ParentID & ") has not yet exited. " _
                 & vbCr & "Keep trying to start new one?", vbYesNo, "Download Starter") = vbYes
      End If
    End If
  End If
ParentFinished:
  On Error Resume Next
  DeleteSetting "Update", "WaitFor"
  'Check for things to do before starting main program
  UpdateComponents
  'Start main program
  'If MsgBox("Update finished, start Download?" & vbCr & Timer - WaitStart, vbYesNo) = vbYes Then
  '  Shell GetSetting("Download", "Files", "Download.exe", "Download.exe")
  'End If
End Sub

Private Sub UpdateComponents()
  Dim vToDoList As Variant
  Dim ToDoList() As String
  Dim ToDoIndex As Long
  Dim ToDoValue As String
  Dim LastToDo As Long
  Dim WhatToDo As String
  Dim filename As String
  Dim Destination As String
  Dim ErrMsg As String
  Dim LogString As String
  Dim WaitStart As Single
  
  On Error GoTo ErrHand
'  DeleteSetting "Update", "ToDo"
'  SaveSetting "Update", "ToDo", "1", "Copy ""c:\vbExperimental\Update\Pending\Old foo.dll"" ""c:\vbExperimental\Update\Bad foo.dll"""
'  SaveSetting "Update", "ToDo", "2", "CopyRegister ""c:\vbExperimental\Update\Pending\New foo.dll"" ""c:\vbExperimental\Update\foo.dll"""
'  SaveSetting "Update", "ToDo", "3", "Delete ""c:\vbExperimental\Update\Bad foo.dll"""
'  SaveSetting "Update", "ToDo", "4", "SaveSetting ""Update"" ""ToDo"" ""5"" ""Delete foo.dll"""

  vToDoList = GetAllSettings("Update", "ToDo")
  If IsArray(vToDoList) Then 'something is in the list
    ToDoList = vToDoList
    LastToDo = UBound(ToDoList)
    For ToDoIndex = 0 To LastToDo
Retry:
      ToDoValue = ExpandWinSysNames(ToDoList(ToDoIndex, 1))

      LogString = LogString & ToDoValue & vbCrLf & "  "
      
      WhatToDo = StrRetRem(ToDoValue)
      filename = StrSplit(ToDoValue, " ", """")
      Destination = StrSplit(ToDoValue, " ", """")
      ErrMsg = ""
      'MsgBox "WhatToDo: " & WhatToDo & vbCr _
      '     & "Filename: " & Filename & vbCr _
      '     & "Destination: " & Destination, vbOKOnly, "UpdateComponents"
      Select Case LCase(WhatToDo)
        Case "copy"
          On Error Resume Next
          Kill Destination
          On Error GoTo ErrHand
          FileCopy filename, Destination
          LogString = LogString & "Copied to " & Destination & vbCrLf
        Case "copyregister", "moveregister"
          On Error Resume Next
          UnRegister Destination
          Kill Destination
          On Error GoTo ErrHand
          FileCopy filename, Destination
          If LCase(WhatToDo) = "moveregister" Then Kill filename
          ErrMsg = Register(Destination)
          If InStr(ErrMsg, "Successful") = 0 Then Err.Raise vbObjectError + 1000, "UpdateComponents", "Unsuccessful Register"
          LogString = LogString & "Copied to " & Destination & " and registered" & vbCrLf
        Case "delete"
          Kill filename
          LogString = LogString & "Deleted " & filename & vbCrLf
        Case "deletesetting"
          If Len(ToDoValue) > 0 Then
            SaveSetting filename, Destination, ToDoValue, ""
            DeleteSetting filename, Destination, ToDoValue
          ElseIf Len(Destination) > 0 Then
            SaveSetting filename, Destination, "dummy", ""
            DeleteSetting filename, Destination
          Else
            SaveSetting filename, "dummy", "dummy", ""
            DeleteSetting filename
          End If
          LogString = LogString & "Deleted setting " & filename & " " & Destination & " " & ToDoValue & vbCrLf
        Case "savesetting"
          If Len(ToDoValue) > 0 Then
            SaveSetting filename, Destination, StrSplit(ToDoValue, " ", """"), StrSplit(ToDoValue, " ", """")
            LogString = LogString & "Saved setting in " & filename & ", " & Destination & vbCrLf
          End If
        Case "register"
          ErrMsg = Register(filename)
          If InStr(ErrMsg, "Successful") = 0 Then Err.Raise vbObjectError + 1000, "UpdateComponents", "Unsuccessful Unregister"
          LogString = LogString & "Registered " & filename & vbCrLf
        Case "unregister"
          On Error Resume Next
          UnRegister filename
          On Error GoTo ErrHand
          LogString = LogString & "Unregistered " & filename & vbCrLf
        Case "unregisterdelete"
          On Error Resume Next
          UnRegister filename
          Kill filename
          On Error GoTo ErrHand
          LogString = LogString & "Unregistered and deleted " & filename & vbCrLf
        Case "run"
          LogString = LogString & "running " & filename & vbCrLf
          Shell filename
        Case Else
          Err.Raise vbObjectError + 1000, "UpdateComponents", "Unknown ToDo Command"
      End Select
Skip:
    Next
    SaveSetting "Update", "ToDo", "dummy", ""
    DeleteSetting "Update", "ToDo"
  End If

  On Error GoTo ErrAtEnd

SaveLog:
  SaveFileString GetLogFilename(, "update"), LogString
  
  frmUpdate.txtLog.Text = LogString
  frmUpdate.Show
  frmUpdate.ZOrder
  WaitStart = Timer
  While frmUpdate.Visible And Timer - WaitStart < 20
    DoEvents
    Sleep 100
  Wend
  Unload frmUpdate
  Exit Sub

ErrHand:
  LogString = LogString & "Error: " & WhatToDo & vbCrLf _
                   & """" & filename & """" & vbCrLf _
                   & """" & Destination & """" & vbCrLf _
                   & ErrMsg & vbCrLf _
                   & Err.Description
                   
  Select Case MsgBox(WhatToDo & vbCr _
                   & """" & filename & """" & vbCr _
                   & """" & Destination & """" & vbCr _
                   & ErrMsg & vbCr _
                   & Err.Description, vbAbortRetryIgnore, "Update Problem")
    Case vbRetry:  Resume Retry
    Case vbIgnore: Resume Skip
    Case vbAbort:  SaveSetting "Update", "ToDo", "dummy", ""
                   DeleteSetting "Update", "ToDo"
                   Resume SaveLog
  End Select
  
ErrAtEnd:
  MsgBox "Error at end: " & Err.Description, vbOKOnly, "Update Problem"
End Sub

Private Function GetLogFilename(Optional ByVal aDefaultPath As String = "", _
                                Optional ByRef aDefaultSuffix As String = "") As String
  Dim BasinsPos As Long
  If Len(aDefaultPath) = 0 Then aDefaultPath = CurDir
  If Right(aDefaultPath, 1) <> "\" Then aDefaultPath = aDefaultPath & "\"
  
  BasinsPos = InStr(UCase(aDefaultPath), "BASINS\")
  If BasinsPos > 0 Then aDefaultPath = Left(aDefaultPath, BasinsPos + 6) & "cache\"
  
  aDefaultPath = aDefaultPath & Format(Date, "log\\yyyy-mm-dd") & Format(Time, "atHH-MM")
  If Len(aDefaultSuffix) > 0 Then aDefaultPath = aDefaultPath & "_" & aDefaultSuffix
  GetLogFilename = aDefaultPath & ".txt"
End Function


