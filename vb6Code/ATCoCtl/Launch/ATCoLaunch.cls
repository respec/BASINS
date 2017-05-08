VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "AtCoLaunch"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit
'Copyright 2001 by AQUA TERRA Consultants

Private pNconvertPath As String
Private pStatusMonitorAvailable As Boolean

Private Type ProcessInfoType
  name As String
  pinfo As PROCESS_INFORMATION
  pipeBuffer As String
  hPipeParentRead As Long
  hPipeParentWrite As Long
  hPipeReadFromParent As Long
  hPipeWriteToParent As Long
End Type
Private pProcess() As ProcessInfoType
Private pNumProcesses As Long

Private pBlockWhenCompute As Boolean

Private Const STARTF_USESTDHANDLES = &H100
Private Const STD_INPUT_HANDLE = -10&
Private Const STD_OUTPUT_HANDLE = -11&
Private Const STD_ERROR_HANDLE = -12&

Private Const INFINITE = &HFFFFFFFF       '  Infinite timeout

Private Const NORMAL_PRIORITY_CLASS = &H20
Private Const DETACHED_PROCESS = &H8

Private Const WAIT_TIMEOUT = &H102&

Private Type STARTUPINFO
  cb As Long
  lpReserved As String
  lpDesktop As String
  lpTitle As String
  dwX As Long
  dwY As Long
  dwXSize As Long
  dwYSize As Long
  dwXCountChars As Long
  dwYCountChars As Long
  dwFillAttribute As Long
  dwFlags As Long
  wShowWindow As Integer
  cbReserved2 As Integer
  lpReserved2 As Long
  hStdInput As Long
  hStdOutput As Long
  hStdError As Long
End Type

Private Type SECURITY_ATTRIBUTES
  nLength As Long
  lpSecurityDescriptor As Long
  bInheritHandle As Long
End Type

Private Declare Function CreatePipe Lib "kernel32" (phReadPipe As Long, phWritePipe As Long, lpPipeAttributes As SECURITY_ATTRIBUTES, ByVal nSize As Long) As Long
Private Declare Function CloseHandle Lib "kernel32" (ByVal hObject As Long) As Long
Private Declare Function WaitForSingleObject Lib "kernel32" (ByVal hHandle As Long, ByVal dwMilliseconds As Long) As Long
Private Declare Function CreateProcessBynum Lib "kernel32" Alias "CreateProcessA" (ByVal lpApplicationName As String, ByVal lpCommandLine As String, ByVal lpProcessAttributes As Long, ByVal lpThreadAttributes As Long, ByVal bInheritHandles As Long, ByVal dwCreationFlags As Long, lpEnvironment As Any, ByVal lpCurrentDirectory As String, lpStartupInfo As STARTUPINFO, lpProcessInformation As PROCESS_INFORMATION) As Long

Private Declare Function CreateProcessA Lib "kernel32" _
   (ByVal lpAppName As Long, _
    ByVal lpCommandLine As String, _
    ByVal lpProcessAttributes As Long, _
    ByVal lpThreadAttributes As Long, _
    ByVal bInheritHandles As Long, _
    ByVal dwCreationFlags As Long, _
    ByVal lpEnvironment As Long, _
    ByVal lpCurrentDirectory As Long, _
    lpStartupInfo As STARTUPINFO, _
    lpProcessInformation As PROCESS_INFORMATION) As Long
    
Private Declare Function TerminateProcess Lib "kernel32" (ByVal hProcess As Long, ByVal uexitcode As Long) As Long
Private Declare Function WaitForInputIdle Lib "user32" (ByVal hProcess As Long, ByVal dwMilliseconds As Long) As Long
Private Declare Function SetStdHandle Lib "kernel32" (ByVal nStdHandle As Long, ByVal nHandle As Long) As Long
Private Declare Function GetStdHandle Lib "kernel32" (ByVal nStdHandle As Long) As Long
Private Declare Function PeekNamedPipe Lib "kernel32" (ByVal hNamedPipe As Long, lpBuffer As Any, ByVal nBufferSize As Long, lpBytesRead As Long, lpTotalBytesAvail As Long, lpBytesLeftThisMessage As Long) As Long

Private sinfo As STARTUPINFO
Private sainfo As SECURITY_ATTRIBUTES
Private pinfoMonitor As PROCESS_INFORMATION
Private pinfoCompute As PROCESS_INFORMATION
Private pComputeStatus As String
Private myPID&

'-99 means process create failed, probably couldn't find exe
Public Event Terminated(ExitCode&)

Public Event Message(FirstWord$, Rest$, ParenLevel&)

'Remove nulls from double-byte ascii or leave normal ascii alone
Private Function BytesToString$(byt() As Byte, nBytes&)
  Dim s$, ch&
  s = ""
  For ch = 0 To nBytes - 1
    If byt(ch) <> 0 Then s = s & Chr$(byt(ch))
  Next ch
  BytesToString = s
End Function

'"","Launching", "Launched", "Terminated", "Process Create Failed"
Public Property Get ComputeStatus$()
  ComputeStatus = pComputeStatus
End Property

Public Property Get hComputeProcess() As Long
  hComputeProcess = pProcess(1).pinfo.hProcess
End Property

Public Property Get MonitorRead() As Long
  MonitorRead = pProcess(0).hPipeReadFromParent
End Property

Public Property Get MonitorWrite() As Long
  MonitorWrite = pProcess(0).hPipeWriteToParent
End Property

Public Property Get ComputeRead() As Long
  ComputeRead = pProcess(0).hPipeParentRead  'pProcess(1).hPipeReadFromParent
End Property

Public Property Get ComputeWrite() As Long
  ComputeWrite = pProcess(0).hPipeParentWrite
End Property

Public Property Get ParentRead() As Long
  ParentRead = pProcess(0).hPipeParentRead
End Property

Public Property Get ParentWrite() As Long
  ParentWrite = pProcess(0).hPipeParentWrite
End Property

Public Property Get ComputeReadFromParent() As Long
  ComputeReadFromParent = pProcess(1).hPipeReadFromParent
End Property

Public Property Get ComputeWriteToParent() As Long
  ComputeWriteToParent = pProcess(1).hPipeWriteToParent
End Property

Public Sub ExitMonitor()
  ExitProcessNumber 0
End Sub

Public Sub ExitProcessNumber(Index As Long)
  Dim ExitCode&
  With pProcess(Index)
    If .pinfo.hProcess > 0 Then
      TerminateProcess .pinfo.hProcess, ExitCode
      .pinfo.hProcess = 0
      .name = ""
    End If
  End With
End Sub

Public Sub ExitProcess(ProcessName As String)
  Dim ExitCode As Long
  Dim pIndex As Long
  If LCase(ProcessName) = "all" Then
    ExitAllProcesses
  Else
    pIndex = ProcessIndex(ProcessName)
    If pIndex <= pNumProcesses Then
      ExitProcessNumber pIndex
    End If
  End If
End Sub

Public Sub ExitAllProcesses()
  Dim pIndex As Long
  For pIndex = 1 To pNumProcesses 'Don't close monitor
    ExitProcessNumber pIndex
  Next
End Sub

Public Sub dbg(Msg$)
  Dim CleanMsg As String
  CleanMsg = ReplaceString(Msg, "(", "[")
  CleanMsg = ReplaceString(CleanMsg, ")", "]")
  SendMonitorMessage "(DBG " & CleanMsg & ")"
End Sub

Public Sub SendMonitorMessage(Msg$)
  With pProcess(0)
    If .hPipeParentWrite <> 0 Then
      WriteTokenToPipe .hPipeParentWrite, Msg, .name, .pinfo.hProcess
    End If
  End With
End Sub

Public Sub SendComputeMessage(Msg$)
  WriteTokenToPipe pProcess(1).hPipeParentWrite, Msg
End Sub

Public Function SendProcessMessage(ProcessName As String, Msg As String) As Boolean
  Dim pNum As Long
  Static MyMsgBox As New ATCoMessage
  If Len(ProcessName) = 0 Then 'default to sending to first process
    If pNumProcesses > 0 Then
      With pProcess(1)
        SendProcessMessage = WriteTokenToPipe(.hPipeParentWrite, Msg, .name, .pinfo.hProcess)
      End With
    End If
    If Not SendProcessMessage Then
      MyMsgBox.ShowNoWait "Error - no process to send message to", "Launch SendProcessMessage", "Ok"
    End If
  Else
    pNum = ProcessIndex(ProcessName)
    If pNum <= pNumProcesses Then
      With pProcess(pNum)
        SendProcessMessage = WriteTokenToPipe(.hPipeParentWrite, Msg, .name, .pinfo.hProcess)
      End With
    End If
    If LCase(ProcessName) = "status" Then SendProcessMessage = True 'Don't worry about failure to send status messages
    If Not SendProcessMessage Then
      MyMsgBox.ShowNoWait "Couldn't write to process '" & ProcessName & "'", "Launch SendProcessMessage", "Ok"
    End If
  End If
End Function

Public Function GetProcessMessage(ProcessName As String) As String
  Dim pNum As Long, lpExitCode As Long
  pNum = ProcessIndex(ProcessName)
  If pNum <= pNumProcesses Then
    With pProcess(pNum)
      GetProcessMessage = ReadTokenFromPipe(.hPipeParentRead, .pipeBuffer, True, .name, .pinfo.hProcess)
    End With
  End If
End Function

Public Function StartProcess(ProcessName As String, CmdLine As String) As Boolean
  Dim pIndex As Long
  Dim lpExitCode As Long
  Dim res As Long
  Dim newCmdLine As String
  
  pComputeStatus = "Launching " & ProcessName
  
  newCmdLine = ReplaceString(CmdLine, "ParentPID", CStr(myPID))
  
  If ProcessName = "Status" Then
    pIndex = 0
  Else
    pIndex = ProcessIndex(ProcessName)
    If pIndex <= pNumProcesses Then
      'Already have this one started -- what should we do?
      MsgBox "Attempted to start a new process with the same name as an old one", vbOKOnly, "Launch StartProcess"
      pIndex = pNumProcesses + 1
    Else
      pNumProcesses = pNumProcesses + 1
      pIndex = pNumProcesses
      ReDim Preserve pProcess(pNumProcesses)
    End If
  End If
  If pIndex <= pNumProcesses Then
    With pProcess(pIndex)
      .name = ProcessName
      res = CreatePipe(.hPipeParentRead, .hPipeWriteToParent, sainfo, 0)
      res = CreatePipe(.hPipeReadFromParent, .hPipeParentWrite, sainfo, 0)
      
'      Call SetStdHandle(STD_OUTPUT_HANDLE, .hPipeWriteToParent)
'      Call SetStdHandle(STD_ERROR_HANDLE, .hPipeWriteToParent)
'      Call SetStdHandle(STD_INPUT_HANDLE, .hPipeReadFromParent)
'      sinfo.dwFlags = 0 'STARTF_USESTDHANDLES
      
      sinfo.hStdOutput = .hPipeWriteToParent
      sinfo.hStdError = .hPipeWriteToParent
      sinfo.hStdInput = .hPipeReadFromParent
      sinfo.dwFlags = STARTF_USESTDHANDLES
      
      res = CreateProcessBynum(vbNullString, newCmdLine, 0, 0, True, NORMAL_PRIORITY_CLASS Or DETACHED_PROCESS, ByVal 0&, vbNullString, sinfo, .pinfo)
        
      If res Then
        'Set standard in/out to go to/from launched monitor for computation that occurs without benefit of a separate process via StartCompute
        pComputeStatus = "Launched " & ProcessName
        StartProcess = True
        If pIndex = 0 Then
          Call SetStdHandle(STD_OUTPUT_HANDLE, .hPipeParentWrite)
          Call SetStdHandle(STD_ERROR_HANDLE, .hPipeParentWrite)
          Call SetStdHandle(STD_INPUT_HANDLE, .hPipeParentRead)
          pStatusMonitorAvailable = True
        ElseIf pBlockWhenCompute Then
          lpExitCode = WaitForTerm(pProcess(pIndex))
          pComputeStatus = "Terminated"
          'RaiseEvent Terminated(lpExitCode)
        End If
      Else
        pComputeStatus = "StartProcess failed for '" & newCmdLine & "'" & vbCr & _
                           "CreateProcessBynum set error code: " & GetLastError
        MsgBox pComputeStatus, vbExclamation, "Could Not Start Process"
        .hPipeParentWrite = 0
        .hPipeParentRead = 0
        'RaiseEvent Terminated(-99) 'create failed
      End If
    End With
  End If
  'Print "(OPEN)"
  'Print "MSG1 Writing to stdout from vb works (2)"

End Function

Public Sub StartMonitor(ByVal MonitorCmdLine As String)
  StartProcess "Status", MonitorCmdLine
End Sub

Public Sub StartMacroPlay(ByVal CmdLine As String)
  Dim res&, args$
  args = CmdLine & " " & CStr(myPID)
  Shell args, 1
End Sub

Public Property Let BlockWhenCompute(NewValue As Boolean)
  pBlockWhenCompute = NewValue
End Property

Public Sub StartCompute(ComputeCmdLine As String)
  StartProcess "Compute", ComputeCmdLine
End Sub

' This wait routine allows other application events
' to be processed while waiting for the process to
' complete.
Private Function WaitForTerm(proc As ProcessInfoType) As Long
  Dim res&
  Dim lpExitCode&
  Dim FlushedStatusMessage As String
  Dim pinfo As PROCESS_INFORMATION
  Static Running As Boolean
  
  pinfo = proc.pinfo
  
  WaitForTerm = -99
  If Running Then
    MsgBox "Already waiting for a process from this control."
  Else
    Running = True
    
    ' Let the process initialize
    Call WaitForInputIdle(pinfo.hProcess, INFINITE)
    ' We don't need the thread handle
    Call CloseHandle(pinfo.hThread)
    While WaitForSingleObject(pinfo.hProcess, 0) = WAIT_TIMEOUT
      DoEvents
      If Not (pStatusMonitorAvailable) Then
        ReadTokenFromPipe proc.hPipeParentRead, FlushedStatusMessage, False, , pinfo.hProcess
        If Len(FlushedStatusMessage) > 0 Then Debug.Print proc.name & ": " & FlushedStatusMessage & vbCrLf
      End If
    Wend
    
    res = GetExitCodeProcess(pinfo.hProcess, lpExitCode)
    WaitForTerm = lpExitCode
    ' Kill the last handle of the process
    Call CloseHandle(pinfo.hProcess)
    Running = False
  End If
End Function

'Returns pNumProcesses + 1 if there isn't a process by that name yet
Private Function ProcessIndex(ByVal ProcessName As String) As Long
  Dim retval As Long
  ProcessIndex = pNumProcesses + 1
  ProcessName = LCase(ProcessName)
  retval = 0
  While retval <= pNumProcesses
    If LCase(pProcess(retval).name) = ProcessName Then
      ProcessIndex = retval
      retval = pNumProcesses + 1
    Else
      retval = retval + 1
    End If
  Wend
End Function

Private Sub Class_Initialize()
  ReDim pProcess(0)
  
  sinfo.cb = Len(sinfo)
  sinfo.lpReserved = vbNullString
  sinfo.lpDesktop = vbNullString
  sinfo.lpTitle = vbNullString
  sinfo.dwFlags = 0 'STARTF_USESTDHANDLES
  
  sainfo.nLength = Len(sainfo)
  sainfo.bInheritHandle = True
  myPID = GetCurrentProcessId
  pBlockWhenCompute = False
  myPID = GetCurrentProcessId
  pBlockWhenCompute = True
  pStatusMonitorAvailable = False
End Sub

'Used by SaveFileAs below
Public Property Get NconvertPath() As String
  NconvertPath = pNconvertPath
End Property
Public Property Let NconvertPath(NewValue As String)
  pNconvertPath = NewValue
End Property

Public Function SavePictureAvailableTypes() As String
  Dim CanFindNconvert As Boolean
  CanFindNconvert = False
  If Len(NconvertPath) > 0 Then
    If Len(Dir(NconvertPath)) > 0 Then
      CanFindNconvert = True
    End If
  End If
  If CanFindNconvert Then
    'SavePictureAvailableTypes = "CompuServe GIF (*.gif)|*.gif|Portable Network Graphics (*.png)|*.png|Windows Bitmap (*.bmp)|*.bmp|Windows Icon (*.ico)|*.ico|X Pixmap (*.xpm)|*.xpm|Copy to Clipboard|*.clb"
    SavePictureAvailableTypes = "Copy to Clipboard|*.clb|Portable Network Graphics (*.png)|*.png|Windows Bitmap (*.bmp)|*.bmp|X Pixmap (*.xpm)|*.xpm"
  Else
    SavePictureAvailableTypes = "Copy to Clipboard|*.clb|Windows Bitmap (*.bmp)|*.bmp"
  End If
End Function

Public Function SavePictureDialog(defaultFilename As String, ParamArray Customize()) As String
  Static LastFilterIndex As Long '+ 100 to tell the difference between initial 0 value and 0 as result of Added Filters
  Dim filter As String, NumAddedFilters As Long
  Dim Param As Variant
  Dim retval As String
  Dim ext As String
  filter = SavePictureAvailableTypes & "|All Files (*.*)|*.*"
  With frmDebug.cdlg
    .CancelError = True
    .DialogTitle = "Save Picture As"
    For Each Param In Customize
      Select Case Left(Param, 1)
        Case "+": filter = Mid(Param, 2) & "|" & filter
                  NumAddedFilters = NumAddedFilters + 1
        Case "=": .DialogTitle = Mid(Param, 2)
      End Select
    Next
    .filter = filter
    If LastFilterIndex > 0 Then .FilterIndex = LastFilterIndex + NumAddedFilters - 100
    If defaultFilename = "" Then
      .filename = "Untitled"
    Else
      .filename = defaultFilename
    End If
    .flags = cdlOFNHideReadOnly Or cdlOFNNoReadOnlyReturn
    On Error GoTo errhand:
    .ShowSave
    retval = .filename
    ext = FilterExt(filter, .FilterIndex)
    If LCase(Right(retval, 3)) <> LCase(Right(ext, 3)) Then
      retval = retval & Right(ext, 4)
    End If
'    Select Case .FilterIndex
'      Case NumAddedFilters + 1: SavePictureDialog = "CLB"
      'Case NumAddedFilters + 2: SavePictureDialog = "CLM"
'      Case Else:                SavePictureDialog = .Filename
'    End Select
    SavePictureDialog = retval
    LastFilterIndex = .FilterIndex - NumAddedFilters + 100
  End With
  Exit Function
errhand:
  SavePictureDialog = ""
End Function

'Requires free external program nconvert from http://www.xnview.com
'Assign NconvertPath = "c:\entire\path\to\nconvert.exe"
'Returns True if it thinks the picture was saved successfully
'FileName ends with a three-character extension of target format:
'  CompuServe GIF (gif)
'  Joint Bi-level Image experts Group (jbig)
'  Joint Photographic Experts Group (jpeg)
'  Portable Network Graphics (png)
'  Windows Bitmap (bmp)
'  Windows Icon (ico)|ico
'  Windows Metafile (wmf)
'  X Pixmap (xpm)
'  For a full list of supported formats, see the documentation included with nconvert.
'If pic = Nothing, this will attempt to convert Filename.bmp to Filename
Public Function SavePictureAs(pic As Object, filename As String) As Boolean
  Dim BitmapFilename As String
  Dim FileType As String
  Dim ConvertedFileName As String
  Dim CmdLine As String
  Dim proc As PROCESS_INFORMATION
  Dim start As STARTUPINFO
  Dim r As Long
  Dim retval As Boolean
  
  If filename = "" Then SavePictureAs = False: Exit Function 'Cancel from save dialog
  
  retval = False
  FileType = FileExt(filename)
  If FileType = "" Then FileType = filename
  
  If pic Is Nothing Then BitmapFilename = filename & ".bmp": GoTo RunConvert
  
  Select Case LCase(FileType)
    Case "clb": Clipboard.Clear: Clipboard.SetData pic.Image, vbCFBitmap:     retval = True
    'Case "clm": Clipboard.Clear: Clipboard.SetData pic.Picture, vbCFMetafile: retval = True
    Case "bmp": SavePicture pic.Image, filename:                              retval = True
    'Case "wmf": SavePicture pic.Picture, Filename:                            retval = True
    'Case "jpg": FileType = "jpeg": GoTo RunConvert
    'Case "tif": FileType = "tiff": GoTo RunConvert
    Case Else:
RunConvert:
      If Len(NconvertPath) = 0 Then
        dbg "Nconvert not found, asking user to locate it"
        NconvertPath = AskUserForFilename("Please locate 'Nconvert.exe'", _
                                          "EXE files (*.exe)|*.exe|All files (*.*)|*.*", _
                                          "exe", _
                                          "Nconvert.exe")
      End If
      If Len(Dir(NconvertPath)) = 0 Then
        dbg "Nconvert not found at '" & NconvertPath & "' - asking user to locate it"
        NconvertPath = AskUserForFilename("Please locate 'Nconvert.exe'", _
                                          "EXE files (*.exe)|*.exe|All files (*.*)|*.*", _
                                          "exe", _
                                          "Nconvert.exe")
      End If
      If BitmapFilename = "" Then
        BitmapFilename = GetTmpPath & FilenameOnly(filename) & ".bmp"
        SavePicture pic.Image, BitmapFilename
      End If
      On Error GoTo ShellError
      ' -D = delete original, -quiet = no output, -o = output filename
      CmdLine = NconvertPath & " -D -o " & filename & " -out " & FileType & " " & BitmapFilename
      StartProcess "Nconvert", CmdLine
      'Shell cmdline, vbHide
      retval = True
  End Select
  SavePictureAs = retval
  If Not retval Then GoTo ShellError
  Exit Function

ShellError:
  If NconvertPath = "" Then
    MsgBox "NconvertPath must be set when calling SavePictureAs." & vbCr & Err.Description, vbOKOnly, "Save Picture As"
  Else
    MsgBox "Could not convert to '" & filename & "'" & vbCr & "using converter '" & NconvertPath & "'" & vbCr & Err.Description, vbOKOnly, "Save Picture As"
  End If
End Function

Private Function FilterExt(ByVal filter As String, ByVal FilterIndex As Long) As String
  Dim parsePos As Long
  Dim parsePos2 As Long
  Dim Index As Long
  parsePos = InStr(filter, "|")
  While parsePos > 0
    parsePos2 = InStr(parsePos + 1, filter, "|")
    If parsePos2 = 0 Then parsePos2 = Len(filter)
    Index = Index + 1
    If Index = FilterIndex Then
      FilterExt = Mid(filter, parsePos + 1, parsePos2 - parsePos - 1)
      Exit Function
    End If
    parsePos = InStr(parsePos2 + 1, filter, "|")
  Wend
End Function

Private Sub Class_Terminate()
  ExitAllProcesses
End Sub

Private Function AskUserForFilename(DialogTitle As String, _
                                    Optional filter As String = "All files (*.*)|*.*", _
                                    Optional DefaultExt As String = "", _
                                    Optional defaultFilename As String = "") As String
  On Error GoTo CancelledDialog
  With frmDummy.cdlg
    .DialogTitle = DialogTitle
    .filename = defaultFilename
    .filter = filter
    .FilterIndex = 1
    .CancelError = True
    .DefaultExt = DefaultExt
    .ShowOpen
    AskUserForFilename = .filename
  End With

CancelledDialog: 'Return empty string if user clicked Cancel

End Function


