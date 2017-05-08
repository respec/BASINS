VERSION 5.00
Begin VB.Form frmStatusInprocess 
   Caption         =   "Status"
   ClientHeight    =   2676
   ClientLeft      =   1092
   ClientTop       =   1512
   ClientWidth     =   5700
   Icon            =   "StatusInprocess.frx":0000
   LinkTopic       =   "Form1"
   PaletteMode     =   1  'UseZOrder
   ScaleHeight     =   2676
   ScaleWidth      =   5700
   Begin VB.TextBox txtDetails 
      BeginProperty Font 
         Name            =   "Courier New"
         Size            =   8.4
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
      Caption         =   "&Details..."
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   7.8
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   495
      Left            =   4080
      TabIndex        =   5
      Top             =   1920
      Width           =   1215
   End
   Begin MSComctlLib.ProgressBar ProgressBar1 
      Height          =   375
      Left            =   240
      TabIndex        =   7
      Top             =   1200
      Visible         =   0   'False
      Width           =   5175
      _ExtentX        =   9123
      _ExtentY        =   656
      _Version        =   393216
      Appearance      =   0
      Max             =   1000
   End
   Begin VB.CommandButton cmdCancel 
      Cancel          =   -1  'True
      Caption         =   "Cancel"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   7.8
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   495
      Left            =   2280
      TabIndex        =   4
      Top             =   1920
      Width           =   1095
   End
   Begin VB.CommandButton cmdPause 
      Caption         =   "&Pause"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   7.8
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   495
      Left            =   360
      TabIndex        =   3
      Top             =   1920
      Width           =   1215
   End
   Begin VB.Label lblMsg 
      Appearance      =   0  'Flat
      AutoSize        =   -1  'True
      BackColor       =   &H80000005&
      BackStyle       =   0  'Transparent
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   7.8
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H80000008&
      Height          =   192
      Index           =   0
      Left            =   480
      TabIndex        =   9
      Top             =   1620
      Width           =   4992
      WordWrap        =   -1  'True
   End
   Begin VB.Label lblMsg 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      BackStyle       =   0  'Transparent
      Caption         =   "lblMsg1"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   7.8
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
      Caption         =   "lblMsg2"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   7.8
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H80000008&
      Height          =   375
      Index           =   2
      Left            =   240
      TabIndex        =   1
      Top             =   720
      Width           =   5235
   End
   Begin VB.Label lblMsg 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      BackStyle       =   0  'Transparent
      Caption         =   "lblMsg4"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   7.8
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H80000008&
      Height          =   375
      Index           =   4
      Left            =   240
      TabIndex        =   8
      Top             =   720
      Width           =   5175
   End
   Begin VB.Label lblMsg 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      BackStyle       =   0  'Transparent
      Caption         =   "lblMsg3"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   7.8
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
Attribute VB_Name = "frmStatusInprocess"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
'Copyright 2002 by AQUA TERRA Consultants

'This form is used by ATCoProgress.
'Looking at ATCoProgress.ctl will probably be more useful than looking here

Private mvarStatus$ 'R = running or resume, P = pause, C = cancel

Public Property Get Status() As String
  Status = mvarStatus
End Property

Public Property Let Status(ByVal NewValue As String)
  mvarStatus = NewValue
End Property

'This routine is a leftover from Status.exe where we needed to
'communicate status messages only via text strings coming through a pipe.
'This functionality is now mostly done by methods and properties of ATCoProgress
'
'Message recieves messages from the application that change
'labels, progress indicator, button visibility, window visibility
'See select statement for possible values of Comand and uses of Arg
'Comand & " " & Arg is also entered in Details box
Public Sub Message(ByVal Comand As String, ByVal Arg As String)
  Static TimeStart As Date, PercentLast As Single
  Dim TimeCurr As Date, TimeDone As Date
  Dim TimeDiff As Variant
  Dim str$, pos&, l&
  Dim doCR As Boolean
  doCR = True
    
  Do
    DoEvents
  Loop While mvarStatus = "P"
  
  Select Case UCase(Comand)
  Case "EXIT", "QUIT", "END":    Unload Me
  Case "MSG99", "CLOSE", "HIDE": Me.WindowState = vbMinimized
  Case "MSG0": 'don't do anything
  Case "MSG1": lblMsg(1) = Arg
  Case "MSG2": lblMsg(2) = Arg
  Case "MSG3": lblMsg(3) = Arg
  Case "MSG4": lblMsg(4) = Arg
  Case "MSG5", "PROGRESS":
    If IsNumeric(Arg) Then
      Dim percent As Single
      percent = CSng(Arg)
      If percent > 0 And percent < 1 Then percent = percent * 100
      
      If percent <= 1 Then
        TimeStart = Time
        lblMsg(0) = "Unknown Estimate for Completion"
      ElseIf percent <> PercentLast Then
        TimeCurr = Time
        TimeDiff = DateDiff("s", TimeStart, TimeCurr)
        TimeDone = DateAdd("s", TimeDiff / (percent / 100#), TimeStart)
        lblMsg(0) = "Estimate Done at: " & TimeDone
      End If
            
      If percent >= 0 And percent <= 100 Then
        ProgressBar1.Value = percent
        If Me.Visible Then ProgressBar1.Visible = True
      Else
        ProgressBar1.Visible = False
      End If
      PercentLast = percent
    End If
  Case "MSG10", "OPEN", "INIT"
    Clear
    If Len(Arg) > 0 Then Caption = Arg
    Show
    WindowState = vbNormal
    Me.ZOrder 0 'bring to front
  Case "BUTTON"
    Select Case Arg
      Case "CANCEL":  cmdCancel.Visible = True
      Case "PAUSE":   cmdPause.Visible = True
      Case "DETAILS": cmdDetails.Visible = True
    End Select
  Case "BUTTOFF"
    Select Case Arg
      Case "CANCEL":  cmdCancel.Visible = False
      Case "PAUSE":   cmdPause.Visible = False
      Case "DETAILS": cmdDetails.Visible = False
    End Select
  Case "TEXT"
    If WindowState = vbNormal Then
      If Arg = "ON" Then
        Me.Height = txtDetails.Top * 2.5
        Me.Width = 10000
      ElseIf Arg = "OFF" Then
        Me.Height = cmdDetails.Top + 1200
        Me.Width = 6000
      Else
        txtDetails.Text = Arg
      End If
    End If
  Case Else
    str = Trim(Comand & " " & Arg)
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
    doCR = False
  End Select
  
  If txtDetails.Visible Then
    str = Comand & " " & Arg
    
    'Sometimes (e.g. SWMM) output contains only carriage returns
    'Here, we insert linefeeds so txtDetails will recognize them
    pos = InStrRev(str, vbCr)
    l = Len(str)
    While pos > 0
      If pos < l Then
        If Mid(str, pos + 1, 1) <> vbLf Then
          str = Left(str, pos) & vbLf & Mid(str, pos + 1)
        End If
      Else
        str = str & vbLf
      End If
      If pos > 1 Then pos = InStrRev(str, vbCr, pos - 1) Else pos = 0
    Wend
        
    str = txtDetails.Text & str
    If Len(str) > 10000 Then
      str = Right(str, 10000)
    End If
    If doCR Then str = str & vbCrLf
    txtDetails.Text = str
    txtDetails.SelStart = Len(str)
  End If
End Sub

Private Sub cmdCancel_Click()
  'If mvarStatus = "C" Then
  '  Unload Me
  'Else
    mvarStatus = "C"
  '  Me.WindowState = vbMinimized
  'End If
End Sub

Private Sub cmdDetails_Click()
  On Error GoTo NeverMind
  If WindowState = vbNormal Then
    If Me.Height - cmdDetails.Top < 2000 Then
      Me.Height = txtDetails.Top * 2.5
      Me.Width = 10000
    Else
      Me.Height = cmdDetails.Top + 1200
      Me.Width = 6000
    End If
  End If
NeverMind:
End Sub

Private Sub cmdPause_Click()
  If cmdPause.Caption = "&Pause" Then
    mvarStatus = "P"
    cmdPause.Caption = "&Resume"
  Else
    mvarStatus = "R"
    cmdPause.Caption = "&Pause"
  End If
End Sub

Private Sub Form_Initialize()
  Clear
End Sub
  
Public Sub Clear()
  Caption = ""
  lblMsg(0) = ""
  lblMsg(1) = ""
  lblMsg(2) = ""
  lblMsg(3) = ""
  lblMsg(4) = ""
  ProgressBar1.Value = 0
  ProgressBar1.Visible = False
  mvarStatus = "R"
End Sub

Private Sub Form_QueryUnload(Cancel As Integer, UnloadMode As Integer)
  If UnloadMode = vbFormControlMenu Then
    WindowState = vbMinimized
    mvarStatus = "C"
    Cancel = 1
  End If
End Sub

Private Sub Form_Resize()
  Dim lbl&, barWidth&
  If Width > 700 Then
    barWidth = Width - 660
    ProgressBar1.Width = barWidth
    txtDetails.Width = barWidth
    For lbl = 1 To 4
      lblMsg(lbl).Width = barWidth
    Next lbl
    cmdCancel.Left = Width / 2 - cmdCancel.Width / 2
    cmdDetails.Left = Width - 1770
  End If
  If Height > 4000 Then
    txtDetails.Visible = Me.Visible
    txtDetails.Height = Height - 3300
  Else
    txtDetails.Visible = False
  End If
End Sub
