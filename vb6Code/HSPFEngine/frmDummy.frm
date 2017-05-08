VERSION 5.00
Begin VB.Form frmDummy 
   Caption         =   "Form1"
   ClientHeight    =   2556
   ClientLeft      =   48
   ClientTop       =   276
   ClientWidth     =   3744
   Icon            =   "frmDummy.frx":0000
   LinkTopic       =   "Form1"
   ScaleHeight     =   2556
   ScaleWidth      =   3744
   StartUpPosition =   3  'Windows Default
   Begin VB.Timer timerPipe 
      Enabled         =   0   'False
      Interval        =   100
      Left            =   1080
      Top             =   1080
   End
End
Attribute VB_Name = "frmDummy"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Private Sub timerPipe_Timer()
  'process messages from parent
  timerPipe.Enabled = False
  ProcessMessages
  timerPipe.Enabled = True

End Sub
