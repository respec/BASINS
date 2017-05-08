VERSION 5.00
Begin VB.Form frmCapture 
   Caption         =   "Capture"
   ClientHeight    =   1845
   ClientLeft      =   45
   ClientTop       =   315
   ClientWidth     =   1725
   Icon            =   "frmCapture.frx":0000
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   ScaleHeight     =   1845
   ScaleWidth      =   1725
   StartUpPosition =   3  'Windows Default
   Begin VB.PictureBox pictCapture 
      AutoSize        =   -1  'True
      Height          =   252
      Left            =   1320
      ScaleHeight     =   195
      ScaleWidth      =   315
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
'Copyright 2000 by AQUA TERRA Consultants

Public Filename As String

Private Sub cmdCapture_Click()
  Dim seconds As Single
  If IsNumeric(txtDelay) Then
    seconds = txtDelay
  Else
    seconds = 0.1
  End If
  TimerDelay.Interval = 1000 * seconds
  TimerDelay.Enabled = True
  Me.Hide
  frmMain.Hide
  frmSample.Hide
End Sub

Private Sub TimerDelay_Timer()

  Dim tempFilename As String
  Dim cmdline As String
  Dim taskID As Integer
  Dim startt As Single
  
  TimerDelay.Enabled = False
  If optWindow.Value Then
    Set pictCapture.Picture = CaptureActiveWindow()
  Else
    Set pictCapture.Picture = CaptureScreen()
  End If
  If Len(Filename) < 1 Then
    With frmMain.cdlg
      .DialogTitle = "Save As..."
      .ShowSave
      Filename = .Filename
    End With
  End If
  If Len(Filename) > 0 Then
    If LCase(Right(Filename, 4)) = ".bmp" Then
      SavePicture pictCapture.Image, Filename
      frmSample.SetImage Filename
    Else
      tempFilename = GetTmpPath & FilenameOnly(Filename) & ".bmp"
      SavePicture pictCapture.Image, tempFilename
      frmSample.SetImage tempFilename
      If Len(Dir(Filename)) > 0 Then Kill Filename
      ' -D = delete original, -quiet = no output, -o = output filename
      cmdline = "-D -o """ & Filename & """ -out " & Right(Filename, 3) & " """ & tempFilename & """"
      RunNconvert cmdline
      'Kill tempFilename
    End If
  End If
  Beep
  frmMain.Show
End Sub
