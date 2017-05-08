Attribute VB_Name = "modUpdateCheck"
Option Explicit

Public Const cAppLabel As String = "Check for Updates"
Public Const cDefaultServerURL = "http://hspf.com/cgi-bin/update?app=BASINS&version=3.1&release=7"
Private Const cTmpFileTag As String = "{tmpfile}"

Public gCancelled As Boolean

Public gAppName As String  'Used for registry settings
Public gUpdateURL As String

Private pNextUpdateIndex As Long

Private pDownloadedString As String
Private pAppPath As String  '"C:\BASINS" (no trailing slash)
Private pCacheDir As String '"C:\BASINS\cache\"
Private pLogFilename As String

Private pQuiet As Boolean

Sub Main()
  Dim errmsg As String
  Dim findPos As String
  Dim updateXML As ChilkatXml
  Dim FollowedAnyInstructions As Boolean
    
  On Error GoTo ErrHand
  
  findPos = InStr(LCase(Command), "/app=""")
  If findPos > 0 Then
    gAppName = Mid(Command, findPos + 6)
    findPos = InStr(gAppName, """")
    If findPos > 0 Then gAppName = Left(gAppName, findPos - 1)
  Else
    gAppName = "DataDownload"
  End If
  
  'Command switch /quiet is used to automatically check for updates
  '/quiet disables most interaction unless there are available updates
  If InStr(LCase(Command), "/quiet") > 0 Then
    pQuiet = True
    'If we already ran quietly today, we don't need to do it again
    If GetSetting(gAppName, "Update", "LastCheck", "Never") = Date Then Exit Sub
  End If
  SaveSetting gAppName, "Update", "LastCheck", Date

  gUpdateURL = cDefaultServerURL
  'findPos = InStr(LCase(Command), "/url=""")
  'If findPos > 0 Then
  '  gUpdateURL = Mid(Command, findPos + 6)
  '  findPos = InStr(gUpdateURL, """")
  '  If findPos > 0 Then gUpdateURL = Left(gUpdateURL, findPos - 1)
  'Else
  '  gUpdateURL = GetSetting(gAppName, "Update", "URL3", cDefaultServerURL)
  'End If
    
  findPos = InStr(LCase(Command), "/apppath=""")
  If findPos > 0 Then
    pAppPath = Mid(Command, findPos + 10)
    findPos = InStr(pAppPath, """")
    If findPos > 0 Then pAppPath = Left(pAppPath, findPos - 1)
  Else
    pAppPath = GetSetting(gAppName, "files", "AppPath", "")
    If Len(pAppPath) = 0 Then pAppPath = RegGetString(HKEY_LOCAL_MACHINE, "SOFTWARE\AQUA TERRA Consultants\" & gAppName, "Base Directory")
    If Len(pAppPath) = 0 Then pAppPath = RegGetString(HKEY_LOCAL_MACHINE, "SOFTWARE\AQUA TERRA Consultants\BASINS", "Base Directory")
    If Len(pAppPath) = 0 Then pAppPath = CurDir
    AddDirSep pAppPath
  End If
  
  pLogFilename = GetLogFilename(pAppPath, "updatecheck")
  
  If Not pQuiet Then frmUpdates.Show vbModal
  
  If Not gCancelled Then
    Dim AvailableUpdates As ChilkatXml
    Set AvailableUpdates = GetUpdateXML(gUpdateURL)
    If Not AvailableUpdates Is Nothing Then
      Dim ComponentsToUpdate() As String
      ComponentsToUpdate = CheckForUpdates(AvailableUpdates, pAppPath)
      
      If UBound(ComponentsToUpdate) > 0 Then
        Dim ComponentName As String
        Dim ComponentIndex As Long
        For ComponentIndex = 1 To UBound(ComponentsToUpdate)
          ComponentName = ComponentsToUpdate(ComponentIndex)
          'OnlyUpdateXML was: ComponentsOnlyNewVariables.KeyExists(ComponentName)
          errmsg = FollowInstructions(AvailableUpdates, _
                                      ComponentName, _
                                      False, _
                                      pAppPath)
          If Len(errmsg) > 0 Then
            LogMsg errmsg, cAppLabel & " - " & ComponentName
          End If
          FollowedAnyInstructions = True
        Next
      End If
      
      
      
      If pNextUpdateIndex > 0 Then
        'we should now launch Update.exe and exit
        'If Len(postUpdateCommand) > 0 Then SaveSetting "Update", "ToDo", pNextUpdateIndex, postUpdateCommand
        SaveSetting "Update", "WaitFor", "PID", GetCurrentProcessId
        Dim f As String
        f = "Update.exe"
        If Not FileExists(f) Then
          LogDbg "FindFile " & f
          Dim ff As Object
          On Error GoTo CouldNotCreateFindFile
          Set ff = CreateObject("ATCoFindFile")
          On Error GoTo ErrHand
          ff.SetDialogProperties "Please locate Update.exe", pAppPath & "\etc\DataDownload\Update.exe"
          ff.SetRegistryInfo gAppName, "Files", "Update.exe"
          f = ff.GetName
        End If
        LogDbg "Got Update.exe name: " & f
        If FileExists(f) Then
          LogDbg "Launching " & f
          Shell f, vbNormalFocus
          'errmsg = "Updates found and downloaded. " & vbCr _
          '        & "This program must exit to finish updating."
          'If LogMsg(errmsg, cAppLabel, "Ok", "Cancel") = 1 Then 'ok
          '  pState = state_finish
          'End If
        Else
CouldNotCreateFindFile:
          errmsg = "Could not find Update.exe, so updates will not be installed."
          LogMsg "Error " & errmsg, cAppLabel
          SaveSetting "Update", "ToDo", "dumm", ""
          DeleteSetting "Update", "ToDo"
        End If
      ElseIf FollowedAnyInstructions Then
        'we did all updating internally
        LogDbg "Update complete"
      End If
    
    End If
  End If
  
  Exit Sub
  
ErrHand:
  LogMsg Err.Description, "Error in UpdateCheck main"
  Exit Sub
  
End Sub

'Private Sub MakeBinaryXML(aSourceFilename As String, aXMLfilename As String)
'  Dim X As New ChilkatXml
'  X.Tag = FilenameNoPath(aSourceFilename)
'  X.SetBinaryContent 1, 0, "", WholeFileBytes(aSourceFilename)
'  SaveFileString aXMLfilename, X.GetXml
'  SaveFileString aXMLfilename & ".binzip", X.GetBinaryContent(0, 0, "")
'End Sub

'Private Sub ExtractBinaryFromFile(aSourceFilename As String, aSaveFilename As String)
'  Dim X As New ChilkatXml
'  X.LoadXmlFile aSourceFilename
'  SaveFileBytes aSaveFilename, X.GetBinaryContent(1, 0, "")
'End Sub

'Private Sub ExtractBinaryXML(aURL As String, aFilename As String)
'  Dim X As New ChilkatXml
'  SaveFileBytes aFilename, X.GetURL(aURL, "", "", "")
'  X.LoadXmlFile aFilename
'  Kill aFilename
'  SaveFileBytes aFilename, X.GetBinaryContent(1, 0, "")
'End Sub

'Private Sub GetURL(aURL As String, aFilename As String)
'  Dim X As New ChilkatXml
'  Set X = X.HttpGet(aURL)
'  SaveFileBytes aFilename, X.GetBinaryContent(1, 0, "")
'End Sub

'Private Sub WaitForParentExit()
'  Dim sParentID As String
'  Dim ParentID As Long
'  Dim hParentProcess As Long
'  Dim DesiredAccess As Long
'  Dim inheritHandle As Long
'  Dim ExitCode As Long
'  Dim WaitStart As Single
'  Dim e As Long
'
'  hParentProcess = 0
'  'Wait for parent to exit, if it is running
'  sParentID = GetSetting("Update", "WaitFor", "PID", Command)
'  If Len(sParentID) > 0 Then
'    If IsNumeric(sParentID) Then
'      ParentID = CLng(sParentID)
'      If ParentID <> 0 Then
'        DesiredAccess = &H400 'PROCESS_QUERY_INFORMATION = &H400
'        inheritHandle = False
'        hParentProcess = OpenProcess(DesiredAccess, inheritHandle, ParentID)
'        Do
'          WaitStart = Timer
'          Do
'            DoEvents
'            e = GetExitCodeProcess(hParentProcess, ExitCode)
'            If ExitCode <> &H103 Then 'still active
'              GoTo ParentFinished
'            End If
'            Sleep 100
'          Loop While Timer - WaitStart < 20
'        Loop While MsgBox("Parent process (" & ParentID & ") has not yet exited. " _
'                 & vbCr & "Keep trying to start new one?", vbYesNo, "Download Starter") = vbYes
'      End If
'    End If
'  End If
'ParentFinished:
'  SaveSetting "Update", "WaitFor", "PID", ""
'  DeleteSetting "Update", "WaitFor"
'End Sub

'Private Sub UpdateComponents()
'  Dim vToDoList As Variant
'  Dim ToDoList() As String
'  Dim ToDoIndex As Long
'  Dim ToDoValue As String
'  Dim LastToDo As Long
'  Dim WhatToDo As String
'  Dim filename As String
'  Dim Destination As String
'  Dim errmsg As String
'  Dim LogString As String
'  Dim WaitStart As Single
'
'  On Error GoTo ErrHand
''  DeleteSetting "Update", "ToDo"
''  SaveSetting "Update", "ToDo", "1", "Copy ""c:\vbExperimental\Update\Pending\Old foo.dll"" ""c:\vbExperimental\Update\Bad foo.dll"""
''  SaveSetting "Update", "ToDo", "2", "CopyRegister ""c:\vbExperimental\Update\Pending\New foo.dll"" ""c:\vbExperimental\Update\foo.dll"""
''  SaveSetting "Update", "ToDo", "3", "Delete ""c:\vbExperimental\Update\Bad foo.dll"""
''  SaveSetting "Update", "ToDo", "4", "SaveSetting ""Update"" ""ToDo"" ""5"" ""Delete foo.dll"""
'
'  vToDoList = GetAllSettings("Update", "ToDo")
'  If IsArray(vToDoList) Then 'something is in the list
'    ToDoList = vToDoList
'    LastToDo = UBound(ToDoList)
'    For ToDoIndex = 0 To LastToDo
'Retry:
'      ToDoValue = ExpandWinSysNames(ToDoList(ToDoIndex, 1))
'
'      LogString = LogString & ToDoValue & vbCrLf & "  "
'
'      WhatToDo = StrRetRem(ToDoValue)
'      filename = StrSplit(ToDoValue, " ", """")
'      Destination = StrSplit(ToDoValue, " ", """")
'      errmsg = ""
'      'MsgBox "WhatToDo: " & WhatToDo & vbCr _
'      '     & "Filename: " & Filename & vbCr _
'      '     & "Destination: " & Destination, vbOKOnly, "UpdateComponents"
'      Select Case LCase(WhatToDo)
'        Case "copy"
'          On Error Resume Next
'          Kill Destination
'          On Error GoTo ErrHand
'          FileCopy filename, Destination
'          LogString = LogString & "Copied to " & Destination & vbCrLf
'        Case "copyregister", "moveregister"
'          On Error Resume Next
'          UnRegister Destination
'          Kill Destination
'          On Error GoTo ErrHand
'          FileCopy filename, Destination
'          If LCase(WhatToDo) = "moveregister" Then Kill filename
'          errmsg = Register(Destination)
'          If InStr(errmsg, "Successful") = 0 Then Err.Raise vbObjectError + 1000, "UpdateComponents", "Unsuccessful Register"
'          LogString = LogString & "Copied to " & Destination & " and registered" & vbCrLf
'        Case "delete"
'          Kill filename
'          LogString = LogString & "Deleted " & filename & vbCrLf
'        Case "deletesetting"
'          If Len(ToDoValue) > 0 Then
'            SaveSetting filename, Destination, ToDoValue, ""
'            DeleteSetting filename, Destination, ToDoValue
'          ElseIf Len(Destination) > 0 Then
'            SaveSetting filename, Destination, "dummy", ""
'            DeleteSetting filename, Destination
'          Else
'            SaveSetting filename, "dummy", "dummy", ""
'            DeleteSetting filename
'          End If
'          LogString = LogString & "Deleted setting " & filename & " " & Destination & " " & ToDoValue & vbCrLf
'        Case "savesetting"
'          If Len(ToDoValue) > 0 Then
'            SaveSetting filename, Destination, StrSplit(ToDoValue, " ", """"), StrSplit(ToDoValue, " ", """")
'            LogString = LogString & "Saved setting in " & filename & ", " & Destination & vbCrLf
'          End If
'        Case "register"
'          errmsg = Register(filename)
'          If InStr(errmsg, "Successful") = 0 Then Err.Raise vbObjectError + 1000, "UpdateComponents", "Unsuccessful Unregister"
'          LogString = LogString & "Registered " & filename & vbCrLf
'        Case "unregister"
'          On Error Resume Next
'          UnRegister filename
'          On Error GoTo ErrHand
'          LogString = LogString & "Unregistered " & filename & vbCrLf
'        Case "unregisterdelete"
'          On Error Resume Next
'          UnRegister filename
'          Kill filename
'          On Error GoTo ErrHand
'          LogString = LogString & "Unregistered and deleted " & filename & vbCrLf
'        Case "run"
'          LogString = LogString & "running " & filename & vbCrLf
'          Shell filename
'        Case Else
'          Err.Raise vbObjectError + 1000, "UpdateComponents", "Unknown ToDo Command"
'      End Select
'Skip:
'    Next
'    SaveSetting "Update", "ToDo", "dummy", ""
'    DeleteSetting "Update", "ToDo"
'  End If
'
'  On Error GoTo ErrAtEnd
'
'SaveLog:
'  SaveFileString GetLogFilename(, "update"), LogString
'
'  'frmUpdate.txtLog.Text = LogString
'  'frmUpdate.Show
'  'frmUpdate.ZOrder
'  WaitStart = Timer
'  'While frmUpdate.Visible And Timer - WaitStart < 20
'    DoEvents
'    Sleep 100
'  'Wend
'  'Unload frmUpdate
'  Exit Sub
'
'ErrHand:
'  LogString = LogString & "Error: " & WhatToDo & vbCrLf _
'                   & """" & filename & """" & vbCrLf _
'                   & """" & Destination & """" & vbCrLf _
'                   & errmsg & vbCrLf _
'                   & Err.Description
'
'  Select Case MsgBox(WhatToDo & vbCr _
'                   & """" & filename & """" & vbCr _
'                   & """" & Destination & """" & vbCr _
'                   & errmsg & vbCr _
'                   & Err.Description, vbAbortRetryIgnore, "Update Problem")
'    Case vbRetry:  Resume Retry
'    Case vbIgnore: Resume Skip
'    Case vbAbort:  SaveSetting "Update", "ToDo", "dummy", ""
'                   DeleteSetting "Update", "ToDo"
'                   Resume SaveLog
'  End Select
'
'ErrAtEnd:
'  MsgBox "Error at end: " & Err.Description, vbOKOnly, "Update Problem"
'End Sub

Private Function GetLogFilename(Optional ByVal aDefaultPath As String = "", _
                                Optional ByRef aDefaultSuffix As String = "") As String
  Dim BasinsPos As Long
  If Len(aDefaultPath) = 0 Then aDefaultPath = CurDir
  AddDirSep aDefaultPath
  
  BasinsPos = InStr(UCase(aDefaultPath), "BASINS\")
  If BasinsPos > 0 Then aDefaultPath = Left(aDefaultPath, BasinsPos + 6) & "cache\"
  
  aDefaultPath = aDefaultPath & Format(Date, "log\\yyyy-mm-dd") & Format(Time, "atHH-MM")
  If Len(aDefaultSuffix) > 0 Then aDefaultPath = aDefaultPath & "_" & aDefaultSuffix
  GetLogFilename = aDefaultPath & ".txt"
End Function

Private Function GetUpdateXML(URL As String) As ChilkatXml
  Download URL, "", True, "Checking for updates"
  
  If Len(pDownloadedString) = 0 Then
    LogMsg "Could not find update information at " & URL, "Get Update"
  Else
    Dim AvailableUpdates As ChilkatXml
    Set AvailableUpdates = New ChilkatXml
    AvailableUpdates.LoadXmlV pDownloadedString
    If Len(AvailableUpdates.ErrorLogText) > 0 Then
      LogMsg "Error parsing update information from " & URL & vbCr _
              & AvailableUpdates.ErrorLogText, cAppLabel
      Set AvailableUpdates = Nothing
    Else
      Set GetUpdateXML = AvailableUpdates
    End If
  End If
End Function

'CheckForUpdates returns array of names of components to update
Public Function CheckForUpdates(AvailableUpdates As ChilkatXml, appPath As String) As String()
  Dim nNewComponents As Long
  Dim Msg As String
  Dim ComponentIndex As Long
  Dim ComponentName As String
  Dim ReleaseNotes As String
  Dim Destination As String
  Dim AvailableComponents As FastCollection
  Dim availNode As ChilkatXml
  Dim lNode As ChilkatXml
  Dim lLocalDoc As ChilkatXml
  Dim tmpNode As ChilkatXml
  Dim filename As String 'File name of local xml or file to be updated
  Dim CompareResult As String
  Dim frmAskWhich As New frmCheckBoxes
  Dim ComponentsOnlyNewVariables As New FastCollection
  Dim HadMessage As Boolean
  Dim saveQuiet As Boolean
  'Dim Requires As New FastCollection
  'Dim vRequirement As Variant 'a string for iterating through Requires
  
  On Error GoTo ErrHand
  
  LogDbg "CheckForUpdates:Entry "
      
  ReDim CheckForUpdates(0)
      
  For ComponentIndex = 0 To AvailableUpdates.NumChildren - 1
    Set availNode = AvailableUpdates.GetChild(ComponentIndex)
    Select Case LCase(availNode.Tag)
    Case "usermessage"
      HadMessage = True
      If GetSetting(gAppName, "Update", availNode.GetAttrValue("MessageID"), "show") = "show" Then
        saveQuiet = pQuiet
        If availNode.GetAttrValue("BreakQuiet") = "yes" Then pQuiet = False
        If Len(availNode.GetAttrValue("URL")) > 0 Then
          Msg = availNode.GetAttrValue("Message") & vbCrLf & vbCrLf & "Open the web page at:" & vbCrLf & availNode.GetAttrValue("URL") & vbCrLf & vbCrLf & "now?"
          LogDbg availNode.GetAttrValue("Title") & vbCrLf & Msg
          If MsgBox(Msg, vbYesNo, availNode.GetAttrValue("Title")) = vbYes Then
            LogDbg "Opening URL: " & availNode.GetAttrValue("URL")
            OpenURL availNode.GetAttrValue("URL")
          End If
        Else
          LogMsg availNode.GetAttrValue("Message"), availNode.GetAttrValue("Title")
        End If
        pQuiet = saveQuiet
      End If
    Case "component"
      'AddRequires availNode, Requires
      ComponentName = availNode.GetAttrValue("Name")
      Destination = availNode.GetAttrValue("Destination")
      filename = ExpandWinSysNames(Destination, appPath) & "\" & ComponentName & ".xml"
      
      Set lLocalDoc = New ChilkatXml
      If FileExists(filename) Then  'Load saved XML describing existing component
        lLocalDoc.LoadXml WholeFileString(filename)
      Else 'Look for existing file without XML and try to generate some XML
        filename = ExpandWinSysNames(Destination, appPath) & "\" _
          & availNode.GetAttrValue("File")
        If FileExists(filename) Then
          lLocalDoc.LoadXml MakeComponentXML(filename, Destination, , , ComponentName)
        End If
      End If
      Set lNode = lLocalDoc.SearchForAttribute(Nothing, "Component", "Name", ComponentName)
      CompareResult = CompareComponents(lNode, availNode)
      If Len(CompareResult) > 0 Then
        nNewComponents = nNewComponents + 1
        ReleaseNotes = FindNodeText(availNode, "ReleaseNote")
        'For Each vRequirement In Requires
        '  If InStr(vRequirement, ComponentName & " ") > 0 Then
        '    ReleaseNotes = ReleaseNotes & vbCr & vRequirement
        '  End If
        'Next
        frmAskWhich.Add ReplaceString(ComponentName, "ATCWebData", ""), _
                        ComponentName, _
                        ReplaceString(ReleaseNotes & vbCrLf & CompareResult, "ATCWebData", ""), True
        If InStr(CompareResult, " version ") = 0 Then 'Not a new version, only need new variables
          ComponentsOnlyNewVariables.Add ComponentName, ComponentName
        End If
      End If
    End Select
  Next
        
  If nNewComponents = 0 Then
    If Not HadMessage Then LogMsg "All available updates are already installed.", cAppLabel
    ReDim CheckForUpdates(0)
  Else
    CheckForUpdates = frmAskWhich.AskUserToSelect("Select Updates to Install", "Available Updates")
  End If
  
  Unload frmAskWhich
  Set frmAskWhich = Nothing
  
  LogDbg "CheckForUpdates Exit"
  
  Exit Function

ErrHand:
  LogMsg "Error checking for updates:" & vbCr & Err.Description, cAppLabel
End Function

Private Function FollowInstructions(AvailableUpdates As ChilkatXml, _
                              ByVal ComponentName As String, _
                           Optional OnlyUpdateXML As Boolean = False, _
                           Optional appPath As String = "") As String
  Dim lNode As ChilkatXml
  Dim Instructions As String
  Dim Destination As String
  Dim xml As String
  Dim DownloadCaption As String

  Dim args As String
  Dim nArgs As Integer
  Dim arg(10) As String
  Dim iArg As Integer
  Dim thisLine As String
  Dim thisCommand As String
  Dim Dest As String
  Dim tmpFilename As String
  Dim retval As String
  Dim SendRestToUpdate As Boolean
  Dim registering As Boolean
  
  LogDbg "FollowInstructions for " & ComponentName
  Set lNode = AvailableUpdates.SearchForAttribute(Nothing, "Component", "Name", ComponentName)
  If lNode Is Nothing Then
    retval = "lNode Is Nothing"
  Else
    xml = AvailableUpdates.GetXml
    xml = Left(xml, InStr(xml, "<Component") - 1) & lNode.GetXml & vbLf & "</ATCCompMl>"
    
    Destination = ExpandWinSysNames(lNode.GetAttrValue("Destination"), appPath) '& "\" & ComponentName & ".xml"
    
    If Not OnlyUpdateXML Then
      Instructions = ExpandWinSysNames(FindNodeText(lNode, "Instructions"), appPath)
      
      DownloadCaption = "Downloading " & ComponentName
    
      thisLine = Trim(StrSplit(Instructions, vbLf, """"))
      If Right(thisLine, 1) = vbCr Then thisLine = Left(thisLine, Len(thisLine) - 1)
      While Len(thisLine) > 0
        LogDbg "FollowInstructions: " & thisLine
        
        args = thisLine
        nArgs = 0
        
        For iArg = 0 To 10
          If Len(args) > 0 Then
            nArgs = iArg
            arg(iArg) = StrSplit(args, " ", """")
          Else
            arg(iArg) = ""
          End If
        Next
        
        thisCommand = arg(0)
        
        'Keep processing "get" commands even if we are sending rest to update
        'This lets us have more than one "get" to different temp files
        If SendRestToUpdate And Not LCase(thisCommand) = "get" Then GoTo GiveCommandToUpdate
        
        Select Case LCase(thisCommand)
          
          Case "get" 'Download URL to file
            If nArgs > 2 Then LogDbg "Warning: more than 2 arguments to " & thisCommand
            If nArgs < 1 Then
              LogDbg "Warning: no arguments, skipped " & thisLine
            Else
              tmpFilename = arg(2)
              'If destination for download was not specified or was {tmpfile}, generate a temp name
              If tmpFilename = cTmpFileTag Or Len(tmpFilename) = 0 Then tmpFilename = GetTmpFileName
              
              If Not Download(arg(1), tmpFilename, DownloadCaption) Then
                retval = "Could not download " & arg(1) & vbCrLf & pDownloadedString
                Kill tmpFilename
                GoTo ExitFun
              End If
              LogDbg "Got """ & arg(1) & """ as """ & tmpFilename
            End If
          
          Case "copy", "move", "copyregister", "moveregister" 'Copy or move a file
            If nArgs > 2 Then LogDbg "Warning: more than 2 arguments to " & thisCommand
            If nArgs < 2 Then
              LogDbg "Warning: fewer than two args, skipped " & thisLine
            Else
              If InStr(LCase(thisCommand), "register") > 0 Then registering = True
              arg(1) = ReplaceString(arg(1), cTmpFileTag, tmpFilename)
              arg(2) = ReplaceString(arg(2), cTmpFileTag, tmpFilename)
              If FileExists(arg(2)) Then
                If registering Then UnRegister arg(2)
                Kill arg(2)
              Else
                MkDirPath PathNameOnly(arg(2))
              End If
              FileCopy arg(1), arg(2)
              If InStr(LCase(thisCommand), "move") > 0 Then
                Kill arg(1)
                LogDbg "Moved """ & arg(1) & """ to """ & arg(2) & """"
              Else
                LogDbg "Copied """ & arg(1) & """ to """ & arg(2) & """"
              End If
              If registering Then
                retval = Register(arg(2))
                If InStr(retval, "Successful") = 0 Then
                  retval = "Could not register """ & arg(2) & """: " & vbCrLf & retval
                Else
                  LogDbg "Registered new """ & arg(2) & """"
                  retval = ""
                End If
              End If
            End If
          
          Case "register"
            If nArgs > 1 Then LogDbg "Warning: more than 1 argument to " & thisCommand
            arg(1) = ReplaceString(arg(1), cTmpFileTag, tmpFilename)
            retval = Register(arg(1))
            If InStr(retval, "Successful") = 0 Then
              retval = "Could not register """ & arg(1) & """: " & vbCrLf & retval
            Else
              LogDbg "Registered new """ & arg(1) & """"
              retval = ""
            End If
          
          Case "unregister"
            If nArgs > 1 Then LogDbg "Warning: more than 1 argument to " & thisCommand
            arg(1) = ReplaceString(arg(1), cTmpFileTag, tmpFilename)
            retval = UnRegister(arg(1))
            If InStr(retval, "Successful") = 0 Then
              retval = "Could not unregister """ & arg(1) & """: " & vbCrLf & retval
            Else
              LogDbg "Unregistered """ & arg(1) & """"
              retval = ""
            End If
          
          Case "unregisterdelete"
            If nArgs > 1 Then LogDbg "Warning: more than 1 argument to " & thisCommand
            arg(1) = ReplaceString(arg(1), cTmpFileTag, tmpFilename)
            retval = UnRegister(arg(1))
            If InStr(retval, "Successful") = 0 Then
              retval = "Could not unregister """ & arg(1) & """: " & vbCrLf & retval
            Else
              LogDbg "Unregistered """ & arg(1) & """"
              retval = ""
            End If
            
            Kill arg(1)
            LogDbg "Deleted " & arg(1) & vbCrLf
          
          Case "delete" 'Delete a file
            If nArgs > 1 Then LogDbg "Warning: more than 1 argument to " & thisCommand
            arg(1) = ReplaceString(arg(1), cTmpFileTag, tmpFilename)
            Kill arg(1)
            LogDbg "Deleted " & arg(1) & vbCrLf
          
          Case "deletesetting" 'Delete a user registry setting
            Select Case nArgs 'DeleteSetting is an error if nothing to delete so we SaveSetting first
              Case Is > 2: SaveSetting arg(1), arg(2), arg(3), "": DeleteSetting arg(1), arg(2), arg(3)
              Case 2:      SaveSetting arg(1), arg(2), "dumm", "": DeleteSetting arg(1), arg(2)
              Case 1:      SaveSetting arg(1), "dumm", "dumm", "": DeleteSetting arg(1)
              Case Else:   LogDbg "Unexpected " & nArgs & " args for " & thisCommand
            End Select
          
          Case "savesetting" 'Create a user registry setting
            If nArgs > 4 Then LogDbg "Warning: more than 4 arguments to " & thisCommand
            If nArgs < 4 Then
              LogDbg "Warning: fewer than 4 arguments, skipped " & thisLine
            Else
              SaveSetting arg(1), arg(2), arg(3), arg(4)
              LogDbg "Saved setting: " & arg(1) & " " & arg(2) & " " & arg(3) & " " & arg(4)
            End If
          
          Case "run"
            If nArgs < 1 Then
              LogDbg "Warning: no arguments, skipping " & thisCommand
            Else
              Dest = ReplaceString(Mid(thisLine, Len(thisCommand) + 2), cTmpFileTag, tmpFilename)
              LogDbg "Running " & Dest
              Shell Dest
            End If
          
          Case "quiet"
            If nArgs > 1 Then LogDbg "Warning: arguments to " & thisCommand
            Select Case LCase(arg(1))
              Case "", "yes", "true": pQuiet = True
              Case Else: pQuiet = False
            End Select
            LogDbg "Quiet = " & pQuiet
          
          Case "requireadmin" 'administrator required before further steps can be taken
            If Not IsAdmin Then
              pQuiet = False
              If nArgs > 0 Then
                retval = Mid(thisLine, Len(thisCommand) + 2)
                retval = ReplaceString(retval, cTmpFileTag, tmpFilename)
                retval = ReplaceString(retval, "crlf", vbCrLf)
              Else
                retval = "ADMINISTRATIVE PRIVILEGES REQUIRED" & vbCrLf _
                       & "To finish installing this update," & vbCrLf _
                       & "a system administrator is needed" & vbCrLf _
                       & "to log in and check for updates."
              End If
              GoTo ExitFun 'GiveCommandToUpdate
            End If
          Case "message" 'arg(1) is title, rest is message
CaseMessage:
            args = thisLine
            StrSplit args, " ", """" 'Remove thisCommand
            StrSplit args, " ", """" 'Remove arg(1)
            args = ReplaceString(args, "crlf", vbCrLf)
            LogMsg ReplaceString(args, cTmpFileTag, tmpFilename), arg(1)
                      
          Case "wait" 'Stop carrying out instructions - leave the rest for Update.exe
            If nArgs > 0 Then LogDbg "Warning: arguments to " & thisCommand
            SendRestToUpdate = True
            LogDbg "Waiting for Update.exe before further instructions are followed."
          
          Case Else   'Not a recognized instruction, so leave it and the rest for Update.exe
            LogDbg "Instruction not known here, sending to Update.exe: " & thisCommand
GiveCommandToUpdate:
            SendRestToUpdate = True
            If pNextUpdateIndex = 0 Then
              pNextUpdateIndex = 1 'GetNextUpdateIndex
              'TODO: check for stale entries and log that they were deleted or leave them to be done
              SaveSetting "Update", "ToDo", "dumm", ""
              DeleteSetting "Update", "ToDo"
            End If
            thisCommand = ReplaceString(thisLine, cTmpFileTag, """" & tmpFilename & """")
            SaveSetting "Update", "ToDo", pNextUpdateIndex, thisCommand
            pNextUpdateIndex = pNextUpdateIndex + 1
        End Select
        thisLine = Trim(StrSplit(Instructions, vbLf, """"))
        If Right(thisLine, 1) = vbCr Then thisLine = Left(thisLine, Len(thisLine) - 1)
      Wend
    End If
    Dest = Destination & "\" & ComponentName & ".xml"
    If SendRestToUpdate Then
      tmpFilename = GetTmpFileName
      SaveSetting "Update", "ToDo", pNextUpdateIndex, "Copy """ & tmpFilename & """ """ & Dest
      pNextUpdateIndex = pNextUpdateIndex + 1
    Else
      tmpFilename = Dest
    End If
    SaveFileString tmpFilename, xml
  End If
ExitFun:
  If Len(retval) > 0 Then LogDbg "FollowInstructions: " & retval
  FollowInstructions = retval
End Function

'Returns True if download was successful
'If filename is blank, downloads to pDownloadedString instead of a file
Public Function Download(ByVal URL As String, _
                         ByVal filename As String, _
                         Optional newCaption As String = "Downloading", _
                         Optional prependToOutput As String = "", _
                         Optional cacheFilename As String = "") As Boolean
  Dim URLfirstpart As String
  Dim URLdatapart As String
  Dim logFilename As String
  Dim URLfileName As String
  Dim TemporaryDownload As Boolean
  Dim Msg As String
  Dim CmdLine As String
  Dim cacheFullPath As String
  Dim dispStatus As String
  Dim lastStatusUpdate As Single
  Dim curlExitCode As Long
  Dim curlExitCodePos As Long
  Dim useActiveMode As Boolean
  
  pDownloadedString = ""
  
  If Len(filename) > 0 Then
    If FileExists(filename) Then 'existing file should be the one we were about to download
      If FileLen(filename) > 0 Then
        Download = True
        LogDbg "  Download: File '" & filename & "' found so downloading was skipped"
        GoTo WrapUp
      
      End If
    End If
  End If
  
  If Len(cacheFilename) > 0 Then
    cacheFullPath = pCacheDir & cacheFilename
    If Not FileExists(cacheFullPath) Then
      MkDirPath PathNameOnly(cacheFullPath)
    Else
      If FileLen(cacheFullPath) > 0 Then
        Download = True
        If Len(filename) > 0 Then
          FileCopy cacheFullPath, filename
          LogDbg "  Download: Cached file '" & cacheFullPath & "' copied to '" & filename & "'"
        Else
          pDownloadedString = WholeFileString(cacheFullPath)
          LogDbg "  Download: Cached file '" & cacheFullPath & "' found and used"
        End If
        
        GoTo WrapUp
      
      End If
    End If
  End If
  
  LogDbg "  Download '" & URL & "' --> '" & filename & "'"
  
  Dim X As ChilkatXml
  Set X = New ChilkatXml
  If Len(filename) = 0 Then
    pDownloadedString = X.GetURL(URL, "", "", "")
    If Len(cacheFilename) > 0 Then
      SaveFileString pCacheDir & cacheFilename, pDownloadedString
    End If
  Else
    SaveFileBytes filename, X.GetURL(URL, "", "", "")
    If Len(cacheFilename) > 0 Then
      MkDirPath (PathNameOnly(pCacheDir & cacheFilename))
      FileCopy filename, pCacheDir & cacheFilename
    End If
  End If
  Download = True

WrapUp:
  
  Exit Function
ErrGettingUpdateXML:
  Select Case Err.Number
    Case -2147467259: LogMsg "Could not find update description at" & vbCrLf & gUpdateURL, "Error searching for update"
    Case Else: LogMsg Err.Description, "Error searching for update " & Err.Number
  End Select
'  If gUpdateURL <> cDefaultServerURL Then 'If we had an error with a non-default URL, try the default one
'    gUpdateURL = cDefaultServerURL
'    Resume
'  End If
  Exit Function
End Function

Public Sub LogDbg(Message As String)
  Debug.Print Message
  
  If Len(pLogFilename) > 0 Then
    AppendFileString pLogFilename, Format(Now, "yyyy/mm/dd hh:mm:ss  ") & Message & vbCrLf
  End If
End Sub

Public Function LogMsg(ByVal aMessage As String, ByVal aTitle As String, ParamArray aButtonName()) As Long
'  Dim lButtonName() As Variant
  
  If Len(aTitle) = 0 Then aTitle = cAppLabel
  
  LogDbg "LogMsg:" & aTitle & ":" & aMessage & ":" & UBound(aButtonName) + 1
  
  If Not pQuiet Then
    aMessage = ReplaceString(aMessage, "{br}", vbCrLf)
    aMessage = ReplaceString(aMessage, "{cr}", vbCr)
    aMessage = ReplaceString(aMessage, "{lf}", vbLf)
    aMessage = ReplaceString(aMessage, "{crlf}", vbCrLf)
    MsgBox aMessage, vbOKOnly, aTitle
  End If
  'TODO: use ATCoMessage as below in place of MsgBox?
  
'  If UBound(aButtonName) < 0 Then
'    ReDim lButtonName(0)
'    lButtonName(0) = "&OK"
'  Else
'    lButtonName = aButtonName
'  End If
'
'  LogMsg = pMsg.ShowArray(aMessage, title, lButtonName)
'  If UBound(lButtonName) > 0 Then
'    If (LogMsg > 0) Then
'      LogDbg "LogMsg:UserSelectedButton:" & lButtonName(LogMsg - 1)
'    Else
'      LogDbg "LogMsg:UserEscaped"
'    End If
'  End If
End Function

