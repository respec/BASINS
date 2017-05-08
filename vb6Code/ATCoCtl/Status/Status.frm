VERSION 5.00
Begin VB.Form frmStatus 
   Caption         =   "Status"
   ClientHeight    =   2580
   ClientLeft      =   1095
   ClientTop       =   1515
   ClientWidth     =   5550
   Icon            =   "Status.frx":0000
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   PaletteMode     =   1  'UseZOrder
   ScaleHeight     =   2580
   ScaleWidth      =   5550
   ShowInTaskbar   =   0   'False
   Begin VB.CommandButton cmdPictures 
      Caption         =   "Basins"
      Height          =   615
      Index           =   2
      Left            =   6120
      Picture         =   "Status.frx":0442
      TabIndex        =   12
      Top             =   1920
      Visible         =   0   'False
      Width           =   1095
   End
   Begin VB.Timer Timer2 
      Interval        =   500
      Left            =   4320
      Top             =   1920
   End
   Begin VB.CommandButton cmdPictures 
      Caption         =   "HSPF"
      Height          =   615
      Index           =   1
      Left            =   6120
      Picture         =   "Status.frx":0D0C
      TabIndex        =   11
      Top             =   1200
      Visible         =   0   'False
      Width           =   1095
   End
   Begin VB.CommandButton cmdPictures 
      Caption         =   "GenScn"
      Height          =   615
      Index           =   0
      Left            =   6120
      Picture         =   "Status.frx":15D6
      TabIndex        =   10
      Top             =   240
      Visible         =   0   'False
      Width           =   1095
   End
   Begin VB.TextBox txtDetails 
      BeginProperty Font 
         Name            =   "Courier New"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   3135
      Left            =   240
      MultiLine       =   -1  'True
      ScrollBars      =   2  'Vertical
      TabIndex        =   6
      Top             =   2760
      Width           =   8775
   End
   Begin VB.CommandButton cmdDetails 
      Caption         =   "&Output"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   495
      Left            =   2880
      TabIndex        =   5
      Top             =   1920
      Width           =   1095
   End
   Begin VB.Timer Timer1 
      Left            =   360
      Top             =   1920
   End
   Begin MSComctlLib.ProgressBar ProgressBar1 
      Height          =   372
      Left            =   240
      TabIndex        =   7
      Top             =   1200
      Visible         =   0   'False
      Width           =   5052
      _ExtentX        =   8916
      _ExtentY        =   661
      _Version        =   393216
      Appearance      =   1
   End
   Begin VB.CommandButton cmdCancel 
      Cancel          =   -1  'True
      Caption         =   "Cancel"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   495
      Left            =   1560
      TabIndex        =   4
      Top             =   1920
      Width           =   1095
   End
   Begin VB.CommandButton cmdPause 
      Caption         =   "&Pause"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   495
      Left            =   240
      TabIndex        =   3
      Top             =   1920
      Width           =   1095
   End
   Begin VB.Label lblMsg 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      BackStyle       =   0  'Transparent
      Caption         =   "MSG6"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H80000008&
      Height          =   315
      Index           =   5
      Left            =   240
      TabIndex        =   13
      Top             =   1200
      Width           =   5235
   End
   Begin VB.Label lblMsg 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      BackStyle       =   0  'Transparent
      Caption         =   "MSG0"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H80000008&
      Height          =   192
      Index           =   0
      Left            =   240
      TabIndex        =   9
      Top             =   1620
      Width           =   4992
      WordWrap        =   -1  'True
   End
   Begin VB.Label lblMsg 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      BackStyle       =   0  'Transparent
      Caption         =   "MSG1"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H80000008&
      Height          =   195
      Index           =   1
      Left            =   240
      TabIndex        =   0
      Top             =   240
      Width           =   5235
   End
   Begin VB.Label lblMsg 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      BackStyle       =   0  'Transparent
      Caption         =   "MSG2"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H80000008&
      Height          =   372
      Index           =   2
      Left            =   240
      TabIndex        =   1
      Top             =   720
      Width           =   5472
   End
   Begin VB.Label lblMsg 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      BackStyle       =   0  'Transparent
      Caption         =   "MSG4"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H80000008&
      Height          =   372
      Index           =   4
      Left            =   240
      TabIndex        =   8
      Top             =   720
      Width           =   5172
   End
   Begin VB.Label lblMsg 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      BackStyle       =   0  'Transparent
      Caption         =   "MSG3"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H80000008&
      Height          =   375
      Index           =   3
      Left            =   240
      TabIndex        =   2
      Top             =   720
      Width           =   5175
   End
End
Attribute VB_Name = "frmStatus"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
'Copyright 2002 by AQUA TERRA Consultants
'##PROJECT_DESCRIPTION ATC Status - Received messages from related processes, displays them _
 in a formatted GUI.  User may try to send cancel message to listening _
 process (response depends on code in listening process)
'Copyright 2000 by AQUA TERRA Consultants

Private Declare Function TerminateProcess Lib "Kernel32" (ByVal hProcess As Long, ByVal uexitcode As Long) As Long
'Private Declare Function GetLastError Lib "kernel32" () As Long
Private Declare Function OpenProcess Lib "Kernel32" (ByVal dwDesiredAccess As Long, ByVal bInheritHandle As Long, ByVal dwProcessId As Long) As Long
Private Declare Function GetExitCodeProcess Lib "Kernel32" (ByVal hProcess As Long, lpExitCode As Long) As Long
Private Declare Function GetStdHandle Lib "Kernel32" (ByVal nStdHandle As Long) As Long
Private Declare Function WriteFile Lib "Kernel32" (ByVal hFile As Long, lpBuffer As Any, ByVal nNumberOfBytesToWrite As Long, lpNumberOfBytesWritten As Long, ByVal lpOverlapped As Long) As Long
Private Declare Function Sleep Lib "Kernel32" (ByVal dwMilliseconds As Long)
'Private Declare Function SetForegroundWindow Lib "user32" (ByVal hwnd As Long) As Long 'BringWindowToTop
Private Declare Function SetWindowPos Lib "user32" (ByVal hwnd As Long, ByVal hWndInsertAfter As Long, ByVal x As Long, ByVal y As Long, ByVal cx As Long, ByVal cy As Long, ByVal wFlags As Long) As Long
Private Const SWP_NOSIZE = &H1
Private Const SWP_NOMOVE = &H2
Private Const SWP_SHOWWINDOW = &H40

Private Const STD_INPUT_HANDLE = -10&
Private Const STD_OUTPUT_HANDLE = -11&
Private Const STD_ERROR_HANDLE = -12&

Private InPipeBuf As String
Private InPipeHandle&
Private OutPipeHandle& 'Writes C for cancel, P for pause, R for resume, E for exit
Private ParentID&
Private hParentProcess&
Private SleepMilliseconds&
Private NextRec As Long
Private FirstRec As Long
Private Records() As String, MaxRec As Long
Private curLabel(5) As String
Private LogToFile As Integer
Private LogFilename As String
Private IgnoringWindowCommands As Boolean

Private Sub ResizeRecords(newSize As Long)
  MaxRec = newSize
  ReDim Preserve Records(newSize)
  If NextRec > MaxRec Then NextRec = 0
  If FirstRec > MaxRec Then FirstRec = 0
End Sub

Private Sub SetProgress(args As String)
  Static TimeStart As Single, PercentLast As Single
  Dim percent As Single
  Dim TimeCurr As Single, TimeLeft As Single
  Dim ElapsedSeconds As Single, hours As Long, minutes As Long, seconds As Long
  Dim str As String, spcindex&
  
  If IsNumeric(args) Then
    percent = CLng(args)
    TimeCurr = Timer
  Else
    spcindex = InStr(1, args, " ")
    If spcindex > 0 Then
      str = Left(args, spcindex - 1)
      If IsNumeric(str) Then percent = CSng(str)
      str = Mid(args, spcindex + 1)
      If IsNumeric(str) Then TimeCurr = CSng(str)
    End If
  End If

  If percent > 0 And percent < 1 Then percent = percent * 100
      
  If percent <= 1 Or percent < PercentLast Or TimeCurr < TimeStart Then
    TimeStart = TimeCurr
    lblMsg(0) = "" '"Unknown Estimate for Completion"
  ElseIf percent <> PercentLast Then
    Dim Countdown As String
    
    ElapsedSeconds = TimeCurr - TimeStart
    TimeLeft = ElapsedSeconds * (100 - percent) / percent
    'TimeDone = TimeCurr + TimeLeft
    If TimeLeft < 60 Then
      lblMsg(0) = "Estimated time left " & CInt(TimeLeft) & " seconds"
    Else
      lblMsg(0) = "Estimate Done at: " & DateAdd("s", TimeLeft, Time)
      hours = Int(TimeLeft / 3600)
      If hours > 0 Then
        Countdown = hours & " hours "
        TimeLeft = TimeLeft - hours * 3600
      End If
      minutes = Int(TimeLeft / 60)
      If minutes > 0 Then
        Countdown = Countdown & minutes & " minute" & Switch(minutes > 1, "s ", True, " ")
        TimeLeft = TimeLeft - minutes * 60
      End If
      If hours = 0 And TimeLeft > 0 Then Countdown = Countdown & CInt(TimeLeft) & " seconds"
      Countdown = Trim(Countdown)
      If Countdown <> "" Then lblMsg(0) = lblMsg(0) & "  (" & Countdown & ") "
    End If
  End If
        
  If percent >= 0 And percent <= 100 Then
    ProgressBar1.Value = percent
    If frmStatus.Visible Then ProgressBar1.Visible = True
  Else
    ProgressBar1.Visible = False
  End If
  PercentLast = percent

End Sub

Private Sub CloseLogFile()
  If LogToFile > 0 Then Close LogToFile: LogToFile = 0
End Sub
Private Sub FlushLogFile()
  If LogToFile > 0 Then
    Close LogToFile
    Open LogFilename For Append Access Write As LogToFile
  End If
End Sub

Private Sub ReceiveMessage(FirstWord As String, Rest As String)
  Dim str$, pos&, l&, e&, t&, i&, iMax&
  Dim lpExitCode&
  
  Select Case UCase(FirstWord)
  Case "EXIT", "QUIT", "END":
    CloseLogFile
    WriteOut "E"
    End '******************************************************
  Case "CLEAR"
    Clear
    Caption = ""
  Case "IGNOREWINDOWCOMMANDS"
    IgnoringWindowCommands = True
  Case "FOLLOWWINDOWCOMMANDS"
    IgnoringWindowCommands = False
  Case "REDIM"
    If IsNumeric(Rest) Then ResizeRecords CLng(Rest)
  Case "TERMINATE"
    CloseLogFile
    i = 0
    iMax = 1000
    Do While i < iMax
      i = i + 1
      lblMsg(1) = "Terminate, Wait for AutoClose " & i
      ShowMe
      DoEvents
      
      e = GetExitCodeProcess(hParentProcess, lpExitCode)
      If lpExitCode <> &H103 Then 'still active
        Exit Do
      End If
    Loop
    If i = iMax Then
      lblMsg(1) = "Terminate, AutoClose Failed"
      DoEvents
      t = TerminateProcess(hParentProcess, 0)
      If t = 0 Then
        MsgBox "Error Terminating Process" & vbCrLf & _
               "Use Task Manager to End Process" & vbCrLf & _
               CStr(e) & ":" & CStr(t) & ":" & CStr(lpExitCode)
      Else
        MsgBox "GenScn Terminated by Status Monitor" & vbCrLf & _
               CStr(e) & ":" & CStr(t) & ":" & CStr(lpExitCode)
      End If
    End If
    End '******************************************************
  Case "MSG99", "CLOSE", "HIDE"
    If Not IgnoringWindowCommands Then HideMe
  Case "MSG0": 'don't do anything
  Case "MSG1": curLabel(1) = Rest
  Case "MSG2": curLabel(2) = Rest
  Case "MSG3": curLabel(3) = Rest
  Case "MSG4": curLabel(4) = Rest
  Case "MSG5", "PROGRESS": SetProgress Rest
  Case "MSG6": curLabel(5) = Rest
  Case "MSG7", "DBG"
    AddRecord Rest
    FirstWord = ""
    Rest = ""
  Case "SHOW": GoTo ShowMe
  Case "MSG10", "OPEN", "INIT"
    Clear
    If Len(Trim(Rest)) > 0 Then
      SetCaption Rest
    End If
ShowMe:
    If Not IgnoringWindowCommands Then
      ShowMe
      'frmStatus.ZOrder 0 'bring to front (broken in Win98)
      'BringWindowToTop frmStatus.hwnd
      'SetForegroundWindow frmStatus.hwnd
      SetWindowPos frmStatus.hwnd, -1, 100, 100, 1000, 500, SWP_NOSIZE + SWP_NOMOVE + SWP_SHOWWINDOW
    End If
  Case "CAPTION"
    SetCaption Rest
  Case "BUTTON"
    Select Case UCase(Rest)
      Case "CANCEL": cmdCancel.Visible = True
      Case "PAUSE":  cmdPause.Visible = True
      Case "DETAILS", "OUTPUT": cmdDetails.Visible = True
    End Select
  Case "BUTTOFF"
    Select Case UCase(Rest)
      Case "CANCEL": cmdCancel.Visible = False
      Case "PAUSE":  cmdPause.Visible = False
      Case "DETAILS", "OUTPUT": cmdDetails.Visible = False
    End Select
  Case "TEXT"
    If WindowState = vbNormal Then
      If UCase(Rest) = "ON" Then
        Me.Height = txtDetails.Top * 2.5
        Me.Width = 10000
      ElseIf UCase(Rest) = "OFF" Then
        Me.Height = cmdDetails.Top + 1200
        Me.Width = 6000
      Else
        txtDetails.Text = Rest
      End If
    End If
  Case "LOGTOFILE" ' rest is either off or a file name to log to
    CloseLogFile
    LogFilename = Rest
    If LCase(Rest) = "off" Then
      LogToFile = 0
    Else
      LogToFile = FreeFile
      MkDirPath PathNameOnly(LogFilename)
      Open LogFilename For Output Access Write As LogToFile
      AddRecord Format(Now, "yyyy/mm/dd") & "  Log from " & Caption
    End If
  Case Else
    str = Trim(FirstWord & " " & Rest)
    l = Len(str)
    pos = InStrRev(str, vbCr)
    While pos > 1 And pos >= l - 1
      str = Left(str, pos - 1)
      l = Len(str)
      pos = InStrRev(str, vbCr)
    Wend
    If pos = 0 Then
      lblMsg(1).Caption = str
    ElseIf pos < l Then
      lblMsg(1).Caption = Mid(str, pos + 1)
    End If
  End Select
  
  If Len(FirstWord) > 0 Then
    str = FirstWord
    If Len(Rest) > 0 Then str = str & " " & Rest
    'Sometimes (e.g. SWMM) output contains only carriage returns
    'Here, we insert linefeeds so txtDetails will recognize them
    pos = InStr(str, vbCr)
    l = Len(str)
    While pos > 0
      If pos < l Then
        AddRecord Left(str, pos - 1)
        If Mid(str, pos + 1, 1) <> vbLf Then
          str = Mid(str, pos + 1)
        Else
          str = Mid(str, pos + 2)
        End If
      End If
      If pos > 1 Then pos = InStr(str, vbCr) Else pos = 0
    Wend
    If Len(str) > 1 Then AddRecord (str)
  End If
  If txtDetails.Visible Then RefreshTextDetails
  DoEvents 'Allow display to update
  Exit Sub
  
ErrHandler:
  MsgBox "Error in ReceiveMessage: " & vbCr & FirstWord & vbCr & Rest & vbCr & Err.Description, vbOKOnly, App.EXEName
End Sub

Private Sub SetCaption(aCaption As String)
  Dim vCmd As Variant, lCmd As CommandButton
  
  Caption = aCaption
  'Look through the pictures we have for one that matches new caption
  For Each vCmd In cmdPictures
    Set lCmd = vCmd
    If InStr(UCase(Caption), UCase(lCmd.Caption)) Then
      Set Icon = lCmd.Picture
      Exit For
    End If
  Next vCmd
End Sub
Private Sub AddRecord(str As String)
  Records(NextRec) = Format(Now, "hh:mm:ss  ") & str
  If LogToFile > 0 Then Print #LogToFile, Records(NextRec): FlushLogFile
  NextRec = NextRec + 1
  If NextRec > MaxRec Then NextRec = 0
  If NextRec = FirstRec Then
    FirstRec = FirstRec + 1
    If FirstRec > MaxRec Then FirstRec = 0
  End If
  'If txtDetails.Visible Then RefreshTextDetails
End Sub

Private Sub RefreshTextDetails()
  Dim newText As String, i As Long
  'newText = "FirstRec = " & FirstRec & ",  NextRec = " & NextRec & vbCrLf
  If FirstRec < NextRec Then
    For i = FirstRec To NextRec - 1
      newText = newText & Records(i) & vbCrLf
    Next
  Else
    For i = FirstRec To MaxRec
      newText = newText & Records(i) & vbCrLf
    Next
    For i = 0 To NextRec - 1
      newText = newText & Records(i) & vbCrLf
    Next
  End If
  txtDetails.Text = newText
  txtDetails.SelStart = Len(newText)
End Sub

Private Sub cmdCancel_Click()
  If SleepMilliseconds = 0 Then
    'Parent process must have exited, so close
    Unload Me
  Else
    WriteOut "C"
    HideMe
  End If
End Sub

Private Sub cmdDetails_Click()
  On Error GoTo NeverMind
  If WindowState = vbNormal Then
    If Me.Height < 3000 Then
      Me.Height = 6444
      Me.Width = 10000
    Else
      Me.Height = 2900
      Me.Width = 5652
    End If
  End If
NeverMind:
End Sub

Private Sub cmdPause_Click()
  If cmdPause.Caption = "&Pause" Then
    WriteOut "P"
    cmdPause.Caption = "&Resume"
  ElseIf cmdPause.Caption = "&Resume" Then
    WriteOut "R"
    cmdPause.Caption = "&Pause"
  Else
    OpenFile LogFilename
  End If
End Sub

'Private Sub cmdSave_Click()
'  Dim outfile As Integer, i As Long
'
'  cdlg.FileName = Me.Caption & ".txt"
'  cdlg.DialogTitle = "Save Trace File as"
'  cdlg.ShowSave
'
'  outfile = FreeFile
'  Open cdlg.FileName For Output As outfile
'  If FirstRec < NextRec Then
'    For i = FirstRec To NextRec
'      Print #outfile, Records(i)
'    Next
'  Else
'    For i = FirstRec To MaxRec
'      Print #outfile, Records(i)
'    Next
'    For i = 0 To NextRec - 1
'      Print #outfile, Records(i)
'    Next
'  End If
'  Close outfile
'
'End Sub

Private Sub Form_Initialize()
  SleepMilliseconds = 100
  hParentProcess = 0
  Dim desiredAccess&, inheritHandle&, ExitCode&
  If Len(Command) > 0 Then
    If IsNumeric(Command) Then
      ParentID = CLng(Command)
      'MsgBox "Parent ID=" & ParentID
      desiredAccess = &H400 'PROCESS_QUERY_INFORMATION = &H400
      inheritHandle = False
      If ParentID <> 0 Then hParentProcess = OpenProcess(desiredAccess, inheritHandle, ParentID)
    End If
  End If
  InPipeHandle = GetStdHandle(STD_INPUT_HANDLE)
  OutPipeHandle = GetStdHandle(STD_OUTPUT_HANDLE)
  ResizeRecords 100
  Clear
  Timer1.Interval = SleepMilliseconds
  Timer2.Interval = 1000 ' 1 second for label update
  HideMe
End Sub

Private Sub HideMe()
  'WindowState = vbMinimized
  Visible = False
End Sub

Private Sub ShowMe()
  Show
  'WindowState = vbNormal
  Visible = True
End Sub

Private Sub Clear()
  'Caption = ""
  lblMsg(0) = ""
  lblMsg(1) = ""
  lblMsg(2) = ""
  lblMsg(3) = ""
  lblMsg(4) = ""
  lblMsg(5) = ""
  ProgressBar1.Value = 0
  ProgressBar1.Visible = False
  'FirstRec = 0
  'NextRec = 0
End Sub

Private Sub Form_Resize()
  Dim lbl&, newWidth&
  If Width > 700 Then
    newWidth = Width - 660
    ProgressBar1.Width = newWidth
    txtDetails.Width = newWidth
    For lbl = 0 To 5
      lblMsg(lbl).Width = newWidth
    Next lbl
    'cmdCancel.Left = Width / 2 - cmdCancel.Width / 2
    'cmdDetails.Left = Width - 1770
  End If
  If Height > 4000 Then
    txtDetails.Height = Height - 3300
    'txtDetails.Visible = True
  'Else
    'txtDetails.Visible = False
  End If
End Sub

Private Sub WriteOut(S$)
  Dim written&, buf() As Byte
  If Len(S) > 0 Then
    ReDim buf(Len(S)) As Byte
    buf() = S
    WriteFile OutPipeHandle, buf(0), 2 * Len(S), written, 0
  End If
End Sub

Private Sub GotToken(str$)
  Dim FirstWord$, Rest$, spcindex&
  FirstWord = Trim(str)
  If Len(FirstWord) > 0 Then
    spcindex = InStr(1, FirstWord, " ")
    If spcindex > 0 Then
      Rest = Mid(FirstWord, spcindex + 1)
      FirstWord = Left(FirstWord, spcindex - 1)
    Else
      Rest = ""
    End If
    ReceiveMessage FirstWord, Rest
  End If
End Sub

Private Sub Form_Unload(Cancel As Integer)
  If SleepMilliseconds = 0 Then
    End
  Else
    HideMe
    Cancel = 1
  End If
End Sub

Private Sub Timer1_Timer()
  Dim ExitCode As Long, token As String
  Timer1.Interval = 0
  
  Do
    token = ReadTokenFromPipe(InPipeHandle, InPipeBuf, False)
    GotToken token
  Loop Until Len(token) = 0
  
  If hParentProcess > 0 Then
    GetExitCodeProcess hParentProcess, ExitCode
    If ExitCode <> &H103 Then  'STILL_ACTIVE = &H0103&
      'Close hDebugFile
      ShowMe
      Caption = "Parent process exited."
      SleepMilliseconds = 0
      If Len(LogFilename) > 0 Then
        cmdPause.Visible = True
        cmdPause.Caption = "&View Log"
      Else
        cmdPause.Visible = False
      End If
      cmdCancel.Visible = True
      cmdCancel.Caption = "Close"
      cmdDetails.Visible = True
      'MsgBox "Status monitor exiting because parent exited."
      'End '*************************************************
    End If
  End If
  Timer1.Interval = SleepMilliseconds
End Sub

Private Sub Timer2_timer()
  Dim i As Long
  
  For i = 1 To 5
    With lblMsg(i)
      If curLabel(i) <> .Caption Then
        .Caption = curLabel(i)
        .Refresh
      End If
    End With
  Next i
End Sub

Private Sub txtDetails_KeyDown(KeyCode As Integer, Shift As Integer)
  Dim FirstWord As String, Rest As String, spcpos As Long
  If KeyCode = vbKeyF10 Then
    If Left(txtDetails.Text, 1) = "(" And Right(txtDetails.Text, 1) = ")" Then
      spcpos = InStr(txtDetails.Text, " ")
      If spcpos > 0 Then
        FirstWord = Mid(txtDetails.Text, 1, spcpos - 1)
        Rest = Mid(txtDetails.Text, spcpos + 1, Len(txtDetails.Text) - spcpos - 1)
      Else
        FirstWord = Mid(txtDetails.Text, 1, Len(txtDetails.Text) - 2)
        Rest = ""
      End If
      ReceiveMessage FirstWord, Rest
    End If
  End If
End Sub
