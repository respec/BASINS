VERSION 5.00
Object = "{48E59290-9880-11CF-9754-00AA00C00908}#1.0#0"; "MSINET.OCX"
Object = "{F9043C88-F6F2-101A-A3C9-08002B2F49FB}#1.2#0"; "COMDLG32.OCX"
Begin VB.Form frmFeedback 
   Caption         =   "Feedback"
   ClientHeight    =   4680
   ClientLeft      =   45
   ClientTop       =   330
   ClientWidth     =   6360
   LinkTopic       =   "Form1"
   ScaleHeight     =   4680
   ScaleWidth      =   6360
   StartUpPosition =   3  'Windows Default
   Begin MSComDlg.CommonDialog cdlg 
      Left            =   720
      Top             =   4200
      _ExtentX        =   688
      _ExtentY        =   688
      _Version        =   393216
      CancelError     =   -1  'True
   End
   Begin VB.TextBox txtSysInfo 
      BackColor       =   &H8000000F&
      Height          =   1095
      Left            =   120
      Locked          =   -1  'True
      MultiLine       =   -1  'True
      ScrollBars      =   2  'Vertical
      TabIndex        =   13
      Top             =   2880
      Width           =   6132
   End
   Begin VB.Frame fraSash 
      BorderStyle     =   0  'None
      Caption         =   " "
      Height          =   495
      Left            =   0
      MousePointer    =   7  'Size N S
      TabIndex        =   11
      Top             =   2400
      Width           =   6375
      Begin VB.Label lblSysInfo 
         Caption         =   "The following system information will be sent with your message:"
         Height          =   255
         Left            =   120
         TabIndex        =   12
         Top             =   120
         Width           =   5055
      End
   End
   Begin InetCtlsObjects.Inet xfer 
      Left            =   120
      Top             =   4200
      _ExtentX        =   794
      _ExtentY        =   794
      _Version        =   393216
   End
   Begin VB.TextBox txtEmail 
      Height          =   288
      Left            =   2040
      TabIndex        =   3
      Top             =   400
      Width           =   2412
   End
   Begin VB.TextBox txtName 
      Height          =   288
      Left            =   2040
      TabIndex        =   1
      Top             =   120
      Width           =   2412
   End
   Begin VB.Frame fraMessage 
      BorderStyle     =   0  'None
      Caption         =   "Frame2"
      Height          =   1572
      Left            =   120
      TabIndex        =   10
      Top             =   840
      Width           =   6132
      Begin VB.TextBox txtMessage 
         Height          =   1212
         Left            =   0
         MultiLine       =   -1  'True
         ScrollBars      =   2  'Vertical
         TabIndex        =   5
         Top             =   360
         Width           =   6132
      End
      Begin VB.Label lblpleaseType 
         Caption         =   "Type a &message to the developers:"
         Height          =   252
         Left            =   0
         TabIndex        =   4
         Top             =   0
         Width           =   3972
      End
   End
   Begin VB.Frame fraButtons 
      BorderStyle     =   0  'None
      Caption         =   "Frame2"
      Height          =   372
      Left            =   2760
      TabIndex        =   9
      Top             =   4200
      Width           =   3492
      Begin VB.CommandButton cmdCopy 
         Caption         =   "&Copy"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   372
         Left            =   1200
         TabIndex        =   7
         ToolTipText     =   "Copy all to clipboard"
         Top             =   0
         Width           =   1092
      End
      Begin VB.CommandButton cmdSend 
         Caption         =   "&Send"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   372
         Left            =   0
         TabIndex        =   6
         ToolTipText     =   "Send this message to the developers now"
         Top             =   0
         Width           =   1092
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
         Height          =   372
         Left            =   2400
         TabIndex        =   8
         Top             =   0
         Width           =   1092
      End
   End
   Begin VB.Label lblEmail 
      Caption         =   "Your &Email Address:"
      Height          =   252
      Left            =   120
      TabIndex        =   2
      Top             =   480
      Width           =   1812
   End
   Begin VB.Label lblName 
      Caption         =   "Your &Name:"
      Height          =   252
      Left            =   120
      TabIndex        =   0
      Top             =   120
      Width           =   1092
   End
End
Attribute VB_Name = "frmFeedback"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit

Private SashDragging As Boolean
Private pEXEName As String
Private pAppDir As String
Private pWindowsSysDir As String
Private pWindowsDir As String
Private pLenWinSys As Long
Private pLenWin As Long
Private pLenApp As Long

Public Sub AddFile(newValue As String)
  Dim b() As Byte, i As Long, l As Long, f As String, s As String
  Dim chkFile As Long
  
  If Len(newValue) > 0 Then
    If Len(Dir(newValue)) > 0 Then
      If FileExt(newValue) = "dat" And Left(FilenameOnly(newValue), 4) = "unin" Then
        b = WholeFileBytes(newValue)
        l = UBound(b)
        i = 0
        s = ""
        While i < l
          If b(i) > 0 Then
            If b(i) = 58 Then 'colon, reset string
              'Debug.Print "colon at " & i
              s = Chr(b(i - 1))
            End If
            If b(i) >= 32 And b(i) < 128 Then
              s = s & Chr(b(i))
            End If
          Else
            If Len(s) > 3 Then
              If Mid(s, 2, 1) = ":" Then
                'For chkFile = 1 To pFilesCount
                '  If pFiles(chkFile) = s Then GoTo DontAdd
                'Next
                'pFilesCount = pFilesCount + 1
                AddFileInfo s
DontAdd:
              End If
            End If
            s = ""
          End If
          i = i + 1
        Wend
      Else
        AddFileInfo newValue
      End If
    End If
  End If
End Sub

Public Function BrowseForFile() As String
  On Error GoTo Cancelled
  
  cdlg.Filter = "Uninstall data (*.dat)|*.dat|All files|*.*"
  cdlg.FilterIndex = 1
  cdlg.ShowOpen
  BrowseForFile = cdlg.Filename

Cancelled:
End Function

Public Sub AddFileInfo(Filename As String, Optional isAppDir As Boolean = False)
  Dim thisFileInfo As String
  Dim tmpstr As String
  On Error GoTo SomeError
  
  If isAppDir Then
    pAppDir = Filename
    If Right(pAppDir, 1) = "\" Then pAppDir = Left(pAppDir, Len(pAppDir) - 1)
    pLenApp = Len(pAppDir)
    thisFileInfo = "Application directory {app}: " & pAppDir & vbCrLf
  Else
    tmpstr = PathNameOnly(Filename)
    If pLenWinSys > 0 And LCase(Left(tmpstr, pLenWinSys)) = LCase(pWindowsSysDir) Then
      tmpstr = "{sys}" & Mid(tmpstr, pLenWinSys + 1)
    ElseIf pLenWin > 0 And LCase(Left(tmpstr, pLenWin)) = LCase(pWindowsDir) Then
      tmpstr = "{win}" & Mid(tmpstr, pLenWin + 1)
    ElseIf pLenApp > 0 And LCase(Left(tmpstr, pLenApp)) = LCase(pAppDir) Then
      tmpstr = "{app}" & Mid(tmpstr, pLenApp + 1)
    End If
    thisFileInfo = FilenameNoPath(Filename) & " in " & tmpstr & vbCrLf
    tmpstr = GetFileVerString(Filename)
    If Len(tmpstr) > 0 Then thisFileInfo = thisFileInfo & "  Version: " & tmpstr & vbCrLf
    tmpstr = Format(FileDateTime(Filename), "MM/DD/YYYY hh:mm:ss am/pm")
    If Len(tmpstr) > 0 Then thisFileInfo = thisFileInfo & "  Date: " & tmpstr & vbCrLf
    tmpstr = FileLen(Filename)
    thisFileInfo = thisFileInfo & "  Size: " & FileLen(Filename) & vbCrLf
  End If
  txtSysInfo.Text = txtSysInfo.Text & thisFileInfo & vbCrLf
  Exit Sub
SomeError:
  tmpstr = "(" & Err.Description & ")"
  Err.Clear
  Resume Next
End Sub

Private Sub cmdCancel_Click()
  Unload Me
End Sub

Private Sub cmdCopy_Click()
  Clipboard.Clear
  Clipboard.SetText "Report From: " & txtName.Text & vbCrLf & txtMessage.Text & vbCrLf & vbCrLf & txtSysInfo.Text
End Sub

Public Sub InitInfo(AppObject As Object)
  Dim SystemInfo As String
  On Error Resume Next
  
  pWindowsSysDir = GetWindowsSysDir
  pWindowsDir = GetWindowsDir
  'remove trailing \
  If Right(pWindowsSysDir, 1) = "\" Then pWindowsSysDir = Left(pWindowsSysDir, Len(pWindowsSysDir) - 1)
  If Right(pWindowsDir, 1) = "\" Then pWindowsDir = Left(pWindowsDir, Len(pWindowsDir) - 1)
  pLenWinSys = Len(pWindowsSysDir)
  pLenWin = Len(pWindowsDir)
  
  SystemInfo = ""
  SystemInfo = SystemInfo & "System Information:"
  SystemInfo = SystemInfo & vbCrLf & "  " & GetWinPlatform
  SystemInfo = SystemInfo & vbCrLf & "  Current User: " & APIUserName
  SystemInfo = SystemInfo & vbCrLf & "  Windows Directory {win}: " & pWindowsDir
  SystemInfo = SystemInfo & vbCrLf & "  Windows System {sys}: " & pWindowsSysDir
  SystemInfo = SystemInfo & vbCrLf
  
  If IsNull(AppObject) Or AppObject Is Nothing Then Set AppObject = App
  '  SystemInfo = SystemInfo & vbCrLf & "App Object not set"
  'Else
    pEXEName = AppObject.EXEName
    lblpleaseType.Caption = "Type a message to the " & pEXEName & " developers"
    SystemInfo = SystemInfo & vbCrLf & "App.EXEName: " & pEXEName
    SystemInfo = SystemInfo & vbCrLf & "  Path: " & AppObject.path
    SystemInfo = SystemInfo & vbCrLf & "  Title: " & AppObject.Title
    SystemInfo = SystemInfo & vbCrLf & "  ProductName: " & AppObject.ProductName
    SystemInfo = SystemInfo & vbCrLf & "  Version: " & AppObject.Major & "." & AppObject.Minor & "." & AppObject.Revision
    SystemInfo = SystemInfo & vbCrLf
  'End If
  
  SystemInfo = SystemInfo & vbCrLf & "Application Status:" & vbCrLf & vbCrLf
  
  txtSysInfo.Text = SystemInfo
  If pEXEName <> "Feedback" Then Me.Caption = pEXEName & " Feedback"
  
End Sub

Private Sub cmdSend_Click()
  Dim URL As String
  Dim postData As String
  Dim startTime As Long, lastDot As Long
  
  On Error GoTo ErrHand
  Me.MousePointer = vbHourglass
  
  URL = "http://genscn.com/cgi-bin/feedback-private.cgi"
  If Len(pEXEName) > 0 Then
    postData = "appname=" & pEXEName
  Else
    postData = "appname=Unspecified"
  End If
  postData = postData & "&name=" & Trim(txtName.Text)
  postData = postData & "&email=" & Trim(txtEmail.Text)
  postData = postData & "&message=" & Trim(txtMessage.Text)
  postData = postData & "&sysinfo=" & Trim(txtSysInfo.Text)
  
  xfer.Execute URL, "POST", postData
  Me.Caption = "Feedback - Sending"
  startTime = Timer
  lastDot = startTime
  While xfer.StillExecuting
    If Timer - lastDot > 1 Then
      Me.Caption = Me.Caption & "."
      lastDot = Timer
    End If
    DoEvents
    If Timer - startTime > 30 Then
      Err.Description = "Timed out"
      GoTo ErrHand
    End If
  Wend
  If xfer.ResponseCode = 0 Then
    MsgBox "Thank you, " & pEXEName & " feedback successfully sent", vbOKOnly, "Feedback"
  Else
    Err.Description = xfer.ResponseInfo
    GoTo ErrHand
  End If
  Unload Me
  
  Exit Sub

ErrHand:
  MsgBox "Error submitting feedback" & vbCr _
        & "Please click Copy, then paste the message into an email to: feedback@genscn.com" & vbCr & vbCr _
        & "Error Details:" & vbCr & vbCr _
        & Err.Description, vbOKOnly, "Feedback"
  Me.MousePointer = vbDefault
  Me.Caption = pEXEName & " Feedback"
        
End Sub

Private Sub Form_Initialize()
  On Error GoTo errmsg
    
  Me.InitInfo App
  AddFile BrowseForFile
  Me.Show
  
  Exit Sub
errmsg:
  MsgBox "Error opening feedback" & vbCr & Err.Description
End Sub

Private Sub Form_Resize()
  Dim w As Long, h As Long, margin As Long
  Dim txtWidth As Long, fraHeight As Long
  Static Resizing As Boolean
  
  If Not Resizing Then
    Resizing = True
    If WindowState <> vbMinimized Then
      margin = fraMessage.Left
      w = Me.ScaleWidth
      txtWidth = w - 2 * margin
      If txtWidth > fraButtons.Width Then
        fraButtons.Left = w - fraButtons.Width - margin
        fraMessage.Width = txtWidth
        fraSash.Width = txtWidth
        txtMessage.Width = txtWidth
        txtSysInfo.Width = txtWidth
        txtName.Width = w - txtName.Left - margin
        txtEmail.Width = txtName.Width
      End If
      
      h = Me.ScaleHeight
      fraButtons.Top = h - fraButtons.Height - margin
      fraHeight = fraSash.Top - fraMessage.Top
      If fraHeight > 360 Then
        fraMessage.Height = fraHeight
        txtMessage.Height = fraHeight - 360
      End If
      fraHeight = fraButtons.Top - fraSash.Top - fraSash.Height - margin
      If fraHeight > 360 Then
        txtSysInfo.Top = fraSash.Top + fraSash.Height
        txtSysInfo.Height = fraHeight
      End If
    End If
    Resizing = False
  End If
End Sub

Private Sub fraSash_MouseDown(Button As Integer, Shift As Integer, X As Single, Y As Single)
  SashDragging = True
End Sub

Private Sub fraSash_MouseUp(Button As Integer, Shift As Integer, X As Single, Y As Single)
  SashDragging = False
End Sub

Private Sub fraSash_MouseMove(Button As Integer, Shift As Integer, X As Single, Y As Single)
  Dim newHeight&
  
  newHeight = fraButtons.Top - (fraSash.Top + Y + fraSash.Height + 400)
  If SashDragging And (fraSash.Top + Y) > fraMessage.Top + 600 And (newHeight > 600) Then
    fraSash.Top = fraSash.Top + Y
    If newHeight > 0 Then txtSysInfo.Height = newHeight
    If Me.WindowState = vbNormal Then Form_Resize
  End If
End Sub

Private Sub lblSysInfo_MouseDown(Button As Integer, Shift As Integer, X As Single, Y As Single)
  SashDragging = True
End Sub

Private Sub lblSysInfo_MouseMove(Button As Integer, Shift As Integer, X As Single, Y As Single)
  fraSash_MouseMove Button, Shift, X, Y
End Sub

Private Sub lblSysInfo_MouseUp(Button As Integer, Shift As Integer, X As Single, Y As Single)
  SashDragging = False
End Sub
