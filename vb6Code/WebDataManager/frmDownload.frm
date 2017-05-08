VERSION 5.00
Begin VB.Form frmDownload 
   Caption         =   "Downloading"
   ClientHeight    =   3555
   ClientLeft      =   60
   ClientTop       =   345
   ClientWidth     =   12000
   Icon            =   "frmDownload.frx":0000
   LinkTopic       =   "Form1"
   ScaleHeight     =   3555
   ScaleWidth      =   12000
   StartUpPosition =   3  'Windows Default
   Begin VB.Timer timProgress 
      Interval        =   1000
      Left            =   120
      Top             =   600
   End
   Begin VB.TextBox txtTo 
      Height          =   855
      Left            =   960
      Locked          =   -1  'True
      MultiLine       =   -1  'True
      TabIndex        =   3
      Top             =   1080
      Width           =   10932
   End
   Begin VB.TextBox txtFrom 
      Height          =   855
      Left            =   960
      Locked          =   -1  'True
      MultiLine       =   -1  'True
      TabIndex        =   1
      Top             =   120
      Width           =   10932
   End
   Begin VB.Frame fraStatus 
      BorderStyle     =   0  'None
      Height          =   1335
      Left            =   120
      TabIndex        =   4
      Top             =   2040
      Width           =   11772
      Begin VB.TextBox txtStatus 
         Height          =   1335
         Left            =   840
         Locked          =   -1  'True
         MultiLine       =   -1  'True
         TabIndex        =   9
         Top             =   0
         Width           =   7815
      End
      Begin VB.CommandButton cmdOkCancel 
         Caption         =   "Ok"
         Default         =   -1  'True
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
         Index           =   1
         Left            =   10560
         TabIndex        =   8
         Top             =   360
         Visible         =   0   'False
         Width           =   1212
      End
      Begin VB.CommandButton cmdOkCancel 
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
         Index           =   0
         Left            =   10560
         TabIndex        =   6
         ToolTipText     =   "Abort the current download"
         Top             =   360
         Width           =   1212
      End
      Begin VB.Label lblTime 
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   255
         Left            =   9000
         TabIndex        =   7
         Top             =   0
         Width           =   2775
      End
      Begin VB.Label lblStatusStatic 
         Caption         =   "Status:"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   252
         Left            =   0
         TabIndex        =   5
         Top             =   120
         Width           =   732
      End
   End
   Begin VB.Label lblTo 
      Caption         =   "To:"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   252
      Left            =   120
      TabIndex        =   2
      Top             =   1200
      Width           =   732
   End
   Begin VB.Label lblFrom 
      Caption         =   "From:"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   252
      Left            =   120
      TabIndex        =   0
      Top             =   240
      Width           =   732
   End
End
Attribute VB_Name = "frmDownload"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit

Private pStartTime As Date

Private Sub cmdOkCancel_Click(Index As Integer)
  If Index = 0 Then
    Me.MousePointer = vbHourglass
    Me.Caption = "Cancelled"
  End If
  Me.Hide
End Sub

Private Sub Form_Resize()
  Dim h As Long
  Dim w As Long
  h = Me.ScaleHeight
  If h > 1000 Then
    txtFrom.Height = (h - 800) / 3
    txtTo.Height = txtFrom.Height
    txtTo.Top = txtFrom.Top + txtFrom.Height + 120
    lblTo.Top = txtTo.Top
    txtStatus.Height = txtFrom.Height
    fraStatus.Height = txtStatus.Height
  End If
  w = Me.ScaleWidth
  If w > txtFrom.Left + 200 Then
    txtFrom.Width = w - txtFrom.Left - 120
    txtTo.Width = txtFrom.Width
    fraStatus.Width = w - fraStatus.Left - 120
    cmdOkCancel(0).Left = fraStatus.Width - cmdOkCancel(0).Width
    cmdOkCancel(1).Left = cmdOkCancel(0).Left
  End If
  fraStatus.Top = h - 120 - fraStatus.Height
End Sub

Public Sub Clear(Optional RestartTimer As Boolean = True)
  Dim stepname As String
  
  On Error GoTo ErrHand
  
  stepname = "txtFrom.Text"
  txtFrom.Text = ""
  
  stepname = "txtTo.Text"
  txtTo.Text = ""
  
  stepname = "txtFrom.BackColor"
  txtFrom.BackColor = vbWindowBackground
  
  stepname = "txtTo.BackColor"
  txtTo.BackColor = vbWindowBackground
  
  stepname = "txtStatus.BackColor"
  txtStatus.BackColor = vbWindowBackground

  stepname = "txtStatus"
  txtStatus.Text = ""
  
  stepname = "cmdOkCancel(0).Visible"
  cmdOkCancel(0).Visible = True
  
  stepname = "cmdOkCancel(1).Visible"
  cmdOkCancel(1).Visible = False
  
  stepname = "RestartTimer"
  If RestartTimer Then
    stepname = "pStartTime"
    pStartTime = Time
  End If
  
  stepname = "timProgress.Enabled"
  timProgress.Enabled = True
  
  stepname = "Exit"
  Exit Sub
  
ErrHand:
  Err.Raise Err.Number, Err.Description & " in frmDownload.Clear, step " & stepname
End Sub

Private Sub timProgress_Timer()
  Dim hours As Long
  Dim minutes As Long
  Dim seconds As Long
  timProgress.Enabled = False
  seconds = DateDiff("s", pStartTime, Time)
  minutes = seconds \ 60
  seconds = seconds - 60 * minutes
  lblTime = "Elapsed Time: " & minutes & ":" & Format(seconds, "00")
  timProgress.Enabled = True
End Sub

Public Sub Deactivate(Optional newStatus As String)
  If Len(newStatus) > 0 Then txtStatus.Text = newStatus
  timProgress.Enabled = False
  cmdOkCancel(0).Visible = False
  cmdOkCancel(1).Visible = True
  txtFrom.BackColor = Me.BackColor
  txtTo.BackColor = Me.BackColor
  txtStatus.BackColor = Me.BackColor
End Sub
