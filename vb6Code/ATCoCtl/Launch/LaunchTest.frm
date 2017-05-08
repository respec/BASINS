VERSION 5.00
Begin VB.Form frmLaunchTest 
   Caption         =   "Launch Test"
   ClientHeight    =   825
   ClientLeft      =   60
   ClientTop       =   345
   ClientWidth     =   5115
   LinkTopic       =   "Form1"
   ScaleHeight     =   825
   ScaleWidth      =   5115
   StartUpPosition =   3  'Windows Default
   Begin VB.CommandButton Command1 
      Caption         =   "Errors"
      Height          =   375
      Index           =   0
      Left            =   4200
      TabIndex        =   2
      Top             =   240
      Width           =   735
   End
   Begin LaunchTest.AtCoLaunch AtCoLaunch1 
      Height          =   375
      Left            =   120
      TabIndex        =   1
      Top             =   240
      Width           =   255
      _ExtentX        =   450
      _ExtentY        =   661
   End
   Begin VB.TextBox Text1 
      Height          =   375
      Left            =   480
      TabIndex        =   0
      Text            =   "s:\vb\Status\Status.exe"
      Top             =   240
      Width           =   3495
   End
End
Attribute VB_Name = "frmLaunchTest"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Private Sub Command1_Click(Index As Integer)
  frmErrors.Show
End Sub

Private Sub Form_Load()
  AtCoLaunch1.StartMonitor Text1.Text
End Sub

Private Sub Text1_KeyPress(KeyAscii As Integer)
  If KeyAscii = 13 Then AtCoLaunch1.SendMonitorMessage Text1.Text
End Sub
