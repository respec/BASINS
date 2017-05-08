VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "ATCoIPC"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit
'Copyright 2001 by AQUA TERRA Consultants

Private pStatusMonitorAvailable As Boolean
Private pTriedToStartMonitor As Boolean

Private Const STARTF_USESTDHANDLES = &H100
Private Const STD_INPUT_HANDLE = -10&
Private Const STD_OUTPUT_HANDLE = -11&
Private Const STD_ERROR_HANDLE = -12&

Private Const INFINITE = &HFFFFFFFF       '  Infinite timeout

Private Const NORMAL_PRIORITY_CLASS = &H20
Private Const DETACHED_PROCESS = &H8

Private Const WAIT_TIMEOUT = &H102&

Private Type PROCESS_INFORMATION
  hProcess As Long
  hThread As Long
  dwProcessId As Long
  dwThreadId As Long
End Type

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
    
Private Declare Function WaitForInputIdle Lib "user32" (ByVal hProcess As Long, ByVal dwMilliseconds As Long) As Long

Private Declare Function SetStdHandle Lib "kernel32" (ByVal nStdHandle As Long, ByVal nHandle As Long) As Long
Private Declare Function GetStdHandle Lib "kernel32" (ByVal nStdHandle As Long) As Long
Private Declare Function PeekNamedPipe Lib "kernel32" (ByVal hNamedPipe As Long, lpBuffer As Any, ByVal nBufferSize As Long, lpBytesRead As Long, lpTotalBytesAvail As Long, lpBytesLeftThisMessage As Long) As Long

Private Declare Function GetCurrentProcessId Lib "kernel32" () As Long
Private Declare Function GetExitCodeProcess Lib "kernel32" (ByVal hProcess As Long, lpExitCode As Long) As Long
Private Declare Function GetLastError Lib "kernel32" () As Long
Private Declare Function TerminateProcess Lib "kernel32" (ByVal hProcess As Long, ByVal uexitcode As Long) As Long

Private sinfo As STARTUPINFO
Private sainfo As SECURITY_ATTRIBUTES
Private pinfoMonitor As PROCESS_INFORMATION
Private pinfoCompute As PROCESS_INFORMATION
Private pComputeStatus As String
Private myPID&

Private Type ProcessInfoType
  name As String
  Status As String
  pinfo As PROCESS_INFORMATION
  pipeBuffer As String
  hPipeReadFromProcess As Long
  hPipeWriteToProcess As Long
  hPipeReadFromParent As Long
  hPipeWriteToParent As Long
End Type
Private pProcess() As ProcessInfoType
Private pNumProcesses As Long

'-99 means process create failed, probably couldn't find exe
'Public Event Terminated(ExitCode&)
'Public Event Message(FirstWord$, Rest$, ParenLevel&)


Public Property Let MonitorEnabled(newValue As Boolean)
  If newValue Then
    StartMonitor
  Else
    If pStatusMonitorAvailable Then ExitProcessNumber 0
    pStatusMonitorAvailable = False
    pTriedToStartMonitor = True
  End If
End Property

'Call before using SendMonitorMessage to use pipes to an existing status monitor
'rather than creating a new one
Public Sub SetMonitorPipes(PipeReadFromStatus As Long, PipeWriteToStatus As Long)
  pProcess(0).hPipeReadFromProcess = PipeReadFromStatus
  pProcess(0).hPipeWriteToProcess = PipeWriteToStatus
  If pStatusMonitorAvailable Then
    ExitProcessNumber 0
    StartMonitor
  End If
End Sub

'Do not use pipe handles unless it is necessary (e.g. for a call to F90_SPIPH)
'Use SendProcessMessage and GetProcessMessage instead
Public Property Get hPipeReadFromProcess(ByVal ProcessName As String) As Long
  Dim pNum As Long
  pNum = ProcessIndex(ProcessName)
  If pNum >= 0 And pNum <= pNumProcesses Then
    hPipeReadFromProcess = pProcess(pNum).hPipeReadFromProcess
  End If
End Property
Public Property Get hPipeWriteToProcess(ByVal ProcessName As String) As Long
  Dim pNum As Long
  pNum = ProcessIndex(ProcessName)
  If pNum >= 0 And pNum <= pNumProcesses Then
    hPipeWriteToProcess = pProcess(pNum).hPipeWriteToProcess
  End If
End Property
Public Property Get hPipeReadFromParent(ByVal ProcessName As String) As Long
  Dim pNum As Long
  pNum = ProcessIndex(ProcessName)
  If pNum >= 0 And pNum <= pNumProcesses Then
    hPipeReadFromParent = pProcess(pNum).hPipeReadFromParent
  End If
End Property
Public Property Get hPipeWriteToParent(ByVal ProcessName As String) As Long
  Dim pNum As Long
  pNum = ProcessIndex(ProcessName)
  If pNum >= 0 And pNum <= pNumProcesses Then
    hPipeWriteToParent = pProcess(pNum).hPipeWriteToParent
  End If
End Property

Private Function ExitProcessNumber(Index As Long) As Long
  Dim ExitCode As Long
  With pProcess(Index)
    If .pinfo.hProcess <> 0 Then
      TerminateProcess .pinfo.hProcess, ExitCode
      .pinfo.hProcess = 0
    End If
  End With
  ExitProcessNumber = ExitCode
End Function

'Terminates process and returns exit code
'ProcessName can be a name or an index from 0 to the number of processes started, 0 is the status monitor
'ProcessName "all" will exit all but the status monitor
Public Function ExitProcess(ByVal ProcessName As String) As Long
  Dim ExitCode As Long
  Dim pNum As Long
  
  If LCase(ProcessName) = "all" Then
    ExitProcess = ExitAllProcesses
    pNum = -1
  Else
    pNum = ProcessIndex(ProcessName)
  End If
  
  If pNum >= 0 And pNum <= pNumProcesses Then
    ExitProcess = ExitProcessNumber(pNum)
  Else
    ExitProcess = -999
  End If
End Function

Private Function ExitAllProcesses() As Long
  Dim ExitCode As Long
  Dim pNum As Long
  For pNum = 1 To pNumProcesses 'Don't close monitor
    ExitCode = ExitCode + ExitProcessNumber(pNum)
  Next
  ExitAllProcesses = ExitCode
End Function

'Send a debug message to the status monitor
'Msg will appear in the Output window without setting any of the labels
Public Sub dbg(Msg$)
  Dim CleanMsg As String
  CleanMsg = ReplaceString(Msg, "(", "[")
  CleanMsg = ReplaceString(CleanMsg, ")", "]")
  SendMonitorMessage "(DBG " & CleanMsg & ")"
End Sub

'Send a message to the status monitor
Public Sub SendMonitorMessage(Msg$)
  If Not pTriedToStartMonitor Then StartMonitor
  If pStatusMonitorAvailable Then
    With pProcess(0)
      If .hPipeWriteToProcess <> 0 Then
        WriteTokenToPipe .hPipeWriteToProcess, Msg, .name, .pinfo.hProcess
      End If
    End With
  Else
    Debug.Print "SendMonitorMessage: " & Msg
  End If
End Sub

'Send a message to any process started through StartProcess
Public Function SendProcessMessage(ByVal ProcessName As String, Msg As String) As Boolean
  Dim pNum As Long
  'Static MyMsgBox As New ATCoMessage
  pNum = ProcessIndex(ProcessName)
  If pNum = 0 Then
    SendMonitorMessage Msg
    SendProcessMessage = True 'Don't worry about failure to send status messages
  ElseIf pNum <= pNumProcesses Then
    With pProcess(pNum)
      SendProcessMessage = WriteTokenToPipe(.hPipeWriteToProcess, Msg, .name, .pinfo.hProcess)
    End With
  End If
  'If Not SendProcessMessage Then
  '  MyMsgBox.ShowNoWait "Couldn't write to process '" & ProcessName & "'", "ATCoIPC SendProcessMessage", "Ok"
  'End If
End Function

'Look for a message coming back from a process
'Wait specifies how many seconds to wait for a message before returning
' "Time expired waiting for a message ..." Default wait is one day
Public Function GetProcessMessage(ByVal ProcessName As String, Optional ByVal Wait As Single = 86400) As String
  Dim pNum As Long, lpExitCode As Long
  pNum = ProcessIndex(ProcessName)
  If pNum >= 0 And pNum <= pNumProcesses Then
    With pProcess(pNum)
      GetProcessMessage = ReadTokenFromPipe(.hPipeReadFromProcess, .pipeBuffer, Wait, .name, .pinfo.hProcess)
    End With
  Else
    GetProcessMessage = "GetProcessMessage: no such process '" & ProcessName & "'"
  End If
End Function

'ProcessName - Name that will be used in later calls to such routines as GetProcessMessage, SendProcessMessage and ExitProcess (must not be numeric)
'CmdLine     - command line to run, including path and file name of executable to launch and any arguments
'AskAfter    - number of seconds to wait before asking the user if they want to terminate the process. 0 = never ask
'KillAfter   - number of seconds to wait before killing the process. 0 = never automatically kill the process
'If AskAfter and KillAfter are both omitted or zero, StartProcess will exit immediately, leaving the new process running
'If AskAfter or KillAfter is greater than zero, StartProcess will wait until the new process ends or is killed
Public Function StartProcess(ByVal ProcessName As String, _
                             CmdLine As String, _
                             Optional AskAfter As Single = 0, _
                             Optional KillAfter As Single = 0) As Boolean
  Dim pNum As Long
  Dim lpExitCode As Long
  Dim res As Long
  Dim newCmdLine As String
    
  'Make sure process name is not a number
  If IsNumeric(ProcessName) Then
    Debug.Print "Warning: numeric process name '" & ProcessName & "' changed in ATCoIPC:StartProcess";
    ProcessName = "P" & ProcessName
    Debug.Print " to '" & ProcessName & "'"
  End If
  
  newCmdLine = ReplaceString(CmdLine, "ParentPID", CStr(myPID))
  
  If LCase(ProcessName) = "status" Then
    pNum = 0
  Else
    pNum = ProcessIndex(ProcessName)
    If pNum <= pNumProcesses Then
      'Already have one by this name started -- see if it is finished
      res = GetExitCodeProcess(pProcess(pNum).pinfo.hProcess, lpExitCode)
      If lpExitCode <> &H103 Then 'Not still running
        Call CloseHandle(pProcess(pNum).pinfo.hThread) ' Kill the last handle of the thread
        Call CloseHandle(pProcess(pNum).pinfo.hProcess) ' Kill the last handle of the process
      Else 'Still running, so we can't re-use the name yet
        MsgBox "Cannot start a process with same name as one already running: '" & ProcessName & "'" _
         & " (" & pProcess(pNum).pinfo.dwProcessId & ") " & res, vbOKOnly, "ATCoIPC StartProcess"
        pNum = pNumProcesses + 1
      End If
    Else
      pNumProcesses = pNumProcesses + 1
      ReDim Preserve pProcess(pNumProcesses)
      pNum = pNumProcesses
    End If
  End If
  If pNum <= pNumProcesses Then
    With pProcess(pNum)
      .name = ProcessName
      .Status = "Launching " & ProcessName
      If .hPipeReadFromParent = 0 Then 'Only create new pipes if not already created
        res = CreatePipe(.hPipeReadFromProcess, .hPipeWriteToParent, sainfo, 0)
        res = CreatePipe(.hPipeReadFromParent, .hPipeWriteToProcess, sainfo, 0)
      End If
            
      sinfo.hStdOutput = .hPipeWriteToParent
      sinfo.hStdError = .hPipeWriteToParent
      sinfo.hStdInput = .hPipeReadFromParent
      sinfo.dwFlags = STARTF_USESTDHANDLES
      
      res = CreateProcessBynum(vbNullString, newCmdLine, 0, 0, True, NORMAL_PRIORITY_CLASS Or DETACHED_PROCESS, ByVal 0&, vbNullString, sinfo, .pinfo)
        
      If res Then
        .Status = "Launched '" & ProcessName & "' " & newCmdLine
        'MsgBox .Status & " as " & .pinfo.dwProcessId, vbOKOnly, "StartProcess"
        StartProcess = True
        If pNum = 0 Then 'we have just started the status monitor
          'Set standard in/out to go to/from monitor
          'for computation that occurs without benefit of a separate process via StartCompute
          Call SetStdHandle(STD_OUTPUT_HANDLE, .hPipeWriteToProcess)
          Call SetStdHandle(STD_ERROR_HANDLE, .hPipeWriteToProcess)
          Call SetStdHandle(STD_INPUT_HANDLE, .hPipeReadFromProcess)
          pStatusMonitorAvailable = True
        ElseIf AskAfter + KillAfter > 0 Then
          lpExitCode = WaitForTerm(pNum, AskAfter, KillAfter)
          .Status = "Terminated"
          'RaiseEvent Terminated(lpExitCode)
        End If
      Else
        .Status = "StartProcess failed for '" & newCmdLine & "'" & vbCr & _
                  "CreateProcessBynum set error code: " & GetLastError
        MsgBox .Status, vbExclamation, "Could Not Start Process"
        .hPipeWriteToProcess = 0
        .hPipeReadFromProcess = 0
        'RaiseEvent Terminated(-99) 'create failed
      End If
    End With
  End If
  'Print "(OPEN)"
  'Print "MSG1 Writing to stdout from vb works (2)"

End Function

'Public Sub StartMacroPlay(ByVal CmdLine As String)
'  Dim res&, args$
'  args = CmdLine & " " & CStr(myPID)
'  Shell args, 1
'End Sub

' This wait routine allows other application events
' to be processed while waiting for the process to complete.
Private Function WaitForTerm(pNum As Long, _
                             ByVal AskAfter As Single, _
                             ByVal KillAfter As Single, _
                             Optional SendOutputToStatus As Boolean = True) As Long
  Dim res&
  Dim lpExitCode&
  Dim FlushedStatusMessage As String
  Dim proc As ProcessInfoType
  Dim pinfo As PROCESS_INFORMATION
  Dim startTime As Single
  Dim nowTime As Single
  Dim elapsedTime As Long
  Dim askFrm As frmAsk
  Static Running As Boolean
    
  WaitForTerm = -99
  If Running Then
    MsgBox "Already waiting for a process from this control."
  Else
    proc = pProcess(pNum)
    pinfo = proc.pinfo
    Running = True
    If KillAfter > 0 Then
      If AskAfter < 0.01 Or AskAfter > KillAfter Then AskAfter = KillAfter
    End If
    If AskAfter > 0 Then startTime = Timer
    ' Let the process initialize
    Call WaitForInputIdle(pinfo.hProcess, INFINITE)
    ' We don't need the thread handle
    Call CloseHandle(pinfo.hThread)
    While WaitForSingleObject(pinfo.hProcess, 0) = WAIT_TIMEOUT
      DoEvents
      
      If SendOutputToStatus Or Not (pStatusMonitorAvailable) Then
        ReadTokenFromPipe proc.hPipeReadFromProcess, FlushedStatusMessage, False, , pinfo.hProcess
        If Len(FlushedStatusMessage) > 0 Then
          Debug.Print proc.name & ": " & FlushedStatusMessage & vbCrLf
          If pStatusMonitorAvailable Then
            Me.SendMonitorMessage FlushedStatusMessage
          End If
        End If
      End If
      
      If AskAfter > 0 Then
        nowTime = Timer
        If nowTime < startTime Then nowTime = nowTime + 86400 'In case we are running at midnight
        If nowTime - startTime > AskAfter Then
          elapsedTime = elapsedTime + (nowTime - startTime)
          If KillAfter > 0 And elapsedTime > KillAfter Then
            ExitProcess pNum
          Else
            If askFrm Is Nothing Then
              Set askFrm = New frmAsk
              Set askFrm.IPC = Me
              askFrm.AskAbout proc.name
            End If
            askFrm.lbl2 = "(elapsed time " & elapsedTime & " seconds)"
          End If
          startTime = nowTime
        End If
      End If
    Wend
    If Not askFrm Is Nothing Then
      askFrm.Hide
      Unload askFrm
    End If
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
  
  If IsNumeric(ProcessName) Then
    ProcessIndex = CLng(ProcessName)
  ElseIf LCase(ProcessName) = "status" Then
    ProcessIndex = 0
  Else
    While retval <= pNumProcesses
      If LCase(pProcess(retval).name) = ProcessName Then
        ProcessIndex = retval
        retval = pNumProcesses + 1
      Else
        retval = retval + 1
      End If
    Wend
  End If
  If ProcessIndex = 0 And Not pTriedToStartMonitor Then StartMonitor
End Function

'Return a filter string specifying which types of picture file we can save
'Some types can only be saved if the external program Nconvert is available
Public Function SavePictureAvailableTypes() As String
'  Dim CanFindNconvert As Boolean
'  CanFindNconvert = False
'  If Len(NconvertPath) > 0 Then
'    If Len(Dir(NconvertPath)) > 0 Then
'      CanFindNconvert = True
'    End If
'  End If
'  If CanFindNconvert Then
    'SavePictureAvailableTypes = "CompuServe GIF (*.gif)|*.gif|Portable Network Graphics (*.png)|*.png|Windows Bitmap (*.bmp)|*.bmp|Windows Icon (*.ico)|*.ico|X Pixmap (*.xpm)|*.xpm|Copy to Clipboard|*.clb"
    SavePictureAvailableTypes = "Compuserv GIF|*.gif|Copy to Clipboard|*.clb|Portable Network Graphics (*.png)|*.png|Tagged Image Format (*.tif)|*.tif|Windows Bitmap (*.bmp)|*.bmp|X Pixmap (*.xpm)|*.xpm"
'  Else
'    SavePictureAvailableTypes = "Copy to Clipboard|*.clb|Windows Bitmap (*.bmp)|*.bmp"
'  End If
End Function

'Display a Save As... dialog for a picture file and return the selected filename
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
      .Filename = "Untitled"
    Else
      .Filename = defaultFilename
    End If
    .flags = cdlOFNHideReadOnly Or cdlOFNNoReadOnlyReturn
    On Error GoTo ErrHand:
    .ShowSave
    retval = .Filename
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
ErrHand:
  SavePictureDialog = ""
End Function

'Requires free external program nconvert from http://www.xnview.com
'Returns True if it thinks the picture was saved successfully
'FileName ends with extension of target format:
'  CompuServe GIF (gif)
'  Joint Bi-level Image experts Group (jbig)
'  Joint Photographic Experts Group (jpeg)
'  Portable Network Graphics (png)
'  Windows Bitmap (bmp)
'  Windows Icon (ico)
'  X Pixmap (xpm)
'  For a full list of supported formats, see the documentation included with nconvert.
'If pic = Nothing, this will attempt to convert Filename.bmp to Filename
Public Function SavePictureAs(pic As Object, Filename As String) As Boolean
  Dim BitmapFilename As String
  Dim FileType As String
  Dim ConvertedFileName As String
  Dim CmdLine As String
  Dim proc As PROCESS_INFORMATION
  Dim start As STARTUPINFO
  Dim r As Long
  Dim retval As Boolean
  Dim ctrl As GflAx.GflAx
  Dim SaveFormat As AX_SaveFormats
    
  If Filename = "" Then SavePictureAs = False: Exit Function 'Cancel from save dialog
  
  On Error GoTo ErrHandler
  
  retval = False
  FileType = LCase(FileExt(Filename))
  If FileType = "" Then FileType = Filename
  
  BitmapFilename = Filename & ".bmp"

  If Not pic Is Nothing Then
    Select Case FileType
      Case "clb": Clipboard.Clear: Clipboard.SetData pic.Image, vbCFBitmap: retval = True
      Case "bmp": SavePicture pic.Image, Filename:                          retval = True
    End Select
  End If
  If Not retval Then
    Select Case FileType
      Case "jpg", "jpeg": SaveFormat = AX_JPEG
      Case "tif", "tiff": SaveFormat = AX_TIFF
      Case "gif": SaveFormat = AX_GIF
      Case "png": SaveFormat = AX_PNG
      Case "tga": SaveFormat = AX_TGA
      Case "pcx": SaveFormat = AX_PCX
      Case "xpm": SaveFormat = AX_XPM
      Case "iff": SaveFormat = AX_IFF
      Case "dcx": SaveFormat = AX_DCX
      Case "pbm": SaveFormat = AX_PBM
      Case "pgm": SaveFormat = AX_PGM
      Case "ppm": SaveFormat = AX_PPM
      Case "pnm": SaveFormat = AX_PNM
      Case "xbm": SaveFormat = AX_XBM
      Case Else: Err.Description = "Unknown type '" & FileType & "'": GoTo ErrHandler
    End Select
  
    If Not pic Is Nothing Then SavePicture pic.Image, BitmapFilename
    
    Set ctrl = New GflAx.GflAx
    ctrl.LoadBitmap BitmapFilename
    If SaveFormat = AX_GIF Then ctrl.EnableLZW = True
    ctrl.SaveFormat = SaveFormat
    ctrl.SaveBitmap Filename
    retval = True
    Kill BitmapFilename
  End If
'      If Len(NconvertPath) = 0 Then
'        GoTo ShellError
'      Else
'        On Error GoTo ShellError
'        If BitmapFilename = "" Then
'          BitmapFilename = GetTmpPath & FilenameOnly(Filename) & ".bmp"
'          SavePicture pic.Image, BitmapFilename
'        End If
'        ' -D = delete original, -quiet = no output, -o = output filename
'        CmdLine = NconvertPath & " -D -o " & Filename & " -out " & FileType & " " & BitmapFilename
'        StartProcess "Nconvert", CmdLine, 10, 30
'        'Shell cmdline, vbHide
'        retval = True
'      End If
'  End Select
  SavePictureAs = retval
  If Not retval Then GoTo ErrHandler
  
  Exit Function

ErrHandler:
'  If Len(NconvertPath) = 0 Then
'    MsgBox "Nconvert.exe needed but not found in ATCoIPC SavePictureAs." & vbCr & Err.Description, vbOKOnly, "Save Picture As"
'  Else
    MsgBox "Could not convert to '" & Filename & "'" & vbCr & Err.Description & vbCr & "Image may be avaliable as " & BitmapFilename, vbOKOnly, "Save Picture As" '& vbCr & "using converter '" & NconvertPath & "'" & vbCr & Err.Description, vbOKOnly, "Save Picture As"
'  End If
End Function

'Take a filter string as used in common dialogs and return the extension of a particular filter
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

'Find the external program Nconvert
'We have changed to using Nconvert's libraries directly, so we don't need to find the EXE any more
'Private Function NconvertPath() As String
'  Static FoundPath As String
'  If Len(FoundPath) = 0 Then
'    Dim lFindFile As New ATCoFindFile
'    Dim r As New ATCoRegistry
'    FoundPath = r.RegGetString(HKEY_LOCAL_MACHINE, "SOFTWARE\AQUA TERRA Consultants\NconvertPath", "") & "\Nconvert.exe"
'    If Len(Dir(FoundPath)) = 0 Then
'      lFindFile.SetRegistryInfo "Nconvert", "Paths", "EXEPath"
'      lFindFile.SetDialogProperties "Please locate 'Nconvert.exe'", "Nconvert.exe", "exe", "EXE files (*.exe)|*.exe|All files|*.*"
'      FoundPath = lFindFile.GetName
'      If Len(FoundPath) > 0 Then
'        If Len(Dir(FoundPath)) = 0 Then FoundPath = ""
'      End If
'    End If
'  End If
'  NconvertPath = FoundPath
'End Function

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
  
End Sub

Private Sub StartMonitor()
  Dim MonitorCmdLine As String
  Dim lFindFile As New ATCoFindFile
  If Not pTriedToStartMonitor Then
    pTriedToStartMonitor = True
    lFindFile.SetRegistryInfo "Status", "Paths", "EXEPath"
    lFindFile.SetDialogProperties "Please locate 'Status.exe'", "Status.exe", "exe", "EXE files (*.exe)|*.exe|All files|*.*"
    MonitorCmdLine = lFindFile.GetName
    If Len(MonitorCmdLine) > 0 Then
      StartProcess "Status", MonitorCmdLine & " " & myPID
    End If
  End If
End Sub

Private Sub Class_Terminate()
  ExitAllProcesses
  ExitProcessNumber 0
End Sub
