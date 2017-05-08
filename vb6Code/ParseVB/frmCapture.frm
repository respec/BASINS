VERSION 5.00
Begin VB.Form frmCapture 
   Caption         =   "Capture"
   ClientHeight    =   1848
   ClientLeft      =   48
   ClientTop       =   312
   ClientWidth     =   1716
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   ScaleHeight     =   1848
   ScaleWidth      =   1716
   StartUpPosition =   3  'Windows Default
   Begin VB.PictureBox pictCapture 
      AutoSize        =   -1  'True
      Height          =   252
      Left            =   1320
      ScaleHeight     =   204
      ScaleWidth      =   324
      TabIndex        =   5
      Top             =   120
      Visible         =   0   'False
      Width           =   372
   End
   Begin VB.Timer TimerDelay 
      Left            =   1320
      Top             =   840
   End
   Begin VB.CommandButton cmdCapture 
      Caption         =   "Capture"
      Default         =   -1  'True
      Height          =   372
      Left            =   240
      TabIndex        =   4
      Top             =   1320
      Width           =   972
   End
   Begin VB.TextBox txtDelay 
      Height          =   288
      Left            =   240
      TabIndex        =   2
      Text            =   "5"
      Top             =   840
      Width           =   252
   End
   Begin VB.OptionButton optScreen 
      Caption         =   "Screen"
      Height          =   192
      Left            =   240
      TabIndex        =   1
      Top             =   480
      Width           =   1092
   End
   Begin VB.OptionButton optWindow 
      Caption         =   "Window"
      Height          =   192
      Left            =   240
      TabIndex        =   0
      Top             =   240
      Value           =   -1  'True
      Width           =   1092
   End
   Begin VB.Label lblDelay 
      Caption         =   "sec. delay"
      Height          =   252
      Left            =   600
      TabIndex        =   3
      Top             =   900
      Width           =   852
   End
End
Attribute VB_Name = "frmCapture"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit

Public filename As String

Private Sub cmdCapture_Click()
  Dim seconds&
  If IsNumeric(txtDelay) Then
    seconds = txtDelay
    TimerDelay.Interval = 1000 * seconds
    TimerDelay.Enabled = True
    Me.Hide
    frmMain.Hide
    frmSample.Hide
  Else
    MsgBox "Seconds of delay must be a number"
  End If
End Sub

Private Sub TimerDelay_Timer()
  TimerDelay.Enabled = False
  If optWindow.value Then
    Set pictCapture.Picture = CaptureActiveWindow()
  Else
    Set pictCapture.Picture = CaptureScreen()
  End If
  If Len(filename) < 1 Then
    frmMain.cdlg.ShowSave
    filename = frmMain.cdlg.filename
  End If
  If Len(filename) > 0 Then
    If LCase(Right(filename, 4)) <> ".bmp" Then filename = filename & ".bmp"
    SavePicture pictCapture.Image, filename
    frmSample.SetImage filename
  End If
  Beep
  frmMain.Show
End Sub
