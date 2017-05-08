VERSION 5.00
Begin VB.Form frmShapeUtil 
   Caption         =   "Shape Util"
   ClientHeight    =   7080
   ClientLeft      =   60
   ClientTop       =   345
   ClientWidth     =   8190
   Icon            =   "frmShapeUtil.frx":0000
   LinkTopic       =   "Form1"
   ScaleHeight     =   7080
   ScaleWidth      =   8190
   StartUpPosition =   3  'Windows Default
   Begin VB.Timer timerShow 
      Interval        =   2000
      Left            =   0
      Top             =   5280
   End
   Begin VB.TextBox txtLicense 
      Height          =   5055
      Left            =   120
      MultiLine       =   -1  'True
      ScrollBars      =   3  'Both
      TabIndex        =   0
      Text            =   "frmShapeUtil.frx":08CA
      Top             =   120
      Width           =   7935
   End
   Begin VB.Label lblStatus 
      Height          =   1335
      Left            =   240
      TabIndex        =   1
      Top             =   5520
      Width           =   7695
   End
End
Attribute VB_Name = "frmShapeUtil"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit

Private Sub timerShow_Timer()
  Me.Show
  timerShow.Enabled = False
End Sub
