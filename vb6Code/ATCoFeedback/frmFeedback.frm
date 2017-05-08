VERSION 5.00
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
         Height          =   255
         Left            =   0
         TabIndex        =   4
         Top             =   0
         Width           =   6135
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
   End
   Begin VB.Label lblStatus 
      Caption         =   "Label1"
      Height          =   375
      Left            =   120
      TabIndex        =   14
      Top             =   4200
      Width           =   4935
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

Private pSashDragging As Boolean
Private pFeedback As clsATCoFeedback

Public Sub setFeedback(aFeedback As clsATCoFeedback)
  Set pFeedback = aFeedback
  If Len(pFeedback.Text) > 64000 Then
    txtSysInfo.Text = Left(pFeedback.Text, 64000) & vbCrLf & vbCrLf & "<truncated, use Copy and then Paste into a text editor to view all text> " & vbCrLf & vbCrLf
  Else
    txtSysInfo.Text = pFeedback.Text
  End If
End Sub

Private Sub cmdCancel_Click()
  UnloadMe
End Sub

Private Sub cmdCopy_Click()
  Clipboard.Clear
  Clipboard.SetText "Report From: " & txtName.Text & vbCrLf & txtMessage.Text & vbCrLf & vbCrLf & pFeedback.Text
End Sub

Private Sub cmdSend_Click()
  Dim URL As String
  Dim postData As String
  Dim startTime As Long, lastDot As Long
  Dim spacePos As Long
  Dim exeName As String
  
  On Error GoTo ErrHand
  Me.MousePointer = vbHourglass
  
  URL = "http://genscn.com/cgi-bin/feedback.cgi"
  spacePos = InStr(Me.Caption, " ")
  If spacePos > 1 Then
    exeName = Left(Me.Caption, spacePos - 1)
    postData = "appname=" & exeName
  Else
    postData = "appname=Unspecified"
  End If
  postData = postData & "&name=" & Trim(txtName.Text)
  postData = postData & "&email=" & Trim(txtEmail.Text)
  postData = postData & "&message=" & Trim(txtMessage.Text)
  postData = postData & "&sysinfo=" & Trim(pFeedback.Text)
  
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
    MsgBox "Thank you, " & exeName & " feedback successfully sent", vbOKOnly, "Feedback"
  Else
    Err.Description = xfer.ResponseInfo
    GoTo ErrHand
  End If
  UnloadMe
  
  Exit Sub

ErrHand:
  MsgBox "Error submitting feedback" & vbCr _
        & "Please click Copy, then paste the message into an email to: feedback@genscn.com" & vbCr & vbCr _
        & "Error Details:" & vbCr & vbCr _
        & Err.Description, vbOKOnly, "Feedback"
  Me.MousePointer = vbDefault
  Me.Caption = exeName & " Feedback"
        
End Sub

'Try to make sure parent has a chance to realize I am not visible before unloading
Private Sub UnloadMe()
  pFeedback.Wait = False
  Unload Me
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
        lblpleaseType.Width = txtWidth
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

Private Sub fraSash_MouseDown(Button As Integer, Shift As Integer, x As Single, y As Single)
  pSashDragging = True
End Sub

Private Sub fraSash_MouseUp(Button As Integer, Shift As Integer, x As Single, y As Single)
  pSashDragging = False
End Sub

Private Sub fraSash_MouseMove(Button As Integer, Shift As Integer, x As Single, y As Single)
  Dim newHeight&
  
  newHeight = fraButtons.Top - (fraSash.Top + y + fraSash.Height + 400)
  If pSashDragging And (fraSash.Top + y) > fraMessage.Top + 600 And (newHeight > 600) Then
    fraSash.Top = fraSash.Top + y
    If newHeight > 0 Then txtSysInfo.Height = newHeight
    If Me.WindowState = vbNormal Then Form_Resize
  End If
End Sub

Private Sub lblSysInfo_MouseDown(Button As Integer, Shift As Integer, x As Single, y As Single)
  pSashDragging = True
End Sub

Private Sub lblSysInfo_MouseMove(Button As Integer, Shift As Integer, x As Single, y As Single)
  fraSash_MouseMove Button, Shift, x, y
End Sub

Private Sub lblSysInfo_MouseUp(Button As Integer, Shift As Integer, x As Single, y As Single)
  pSashDragging = False
End Sub
