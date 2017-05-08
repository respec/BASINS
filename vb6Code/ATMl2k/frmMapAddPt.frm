VERSION 5.00
Begin VB.Form frmMapAddPt 
   BorderStyle     =   3  'Fixed Dialog
   Caption         =   "Add Point"
   ClientHeight    =   1404
   ClientLeft      =   1296
   ClientTop       =   1452
   ClientWidth     =   5880
   HelpContextID   =   520
   Icon            =   "frmMapAddPt.frx":0000
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   1404
   ScaleWidth      =   5880
   ShowInTaskbar   =   0   'False
   Begin VB.CommandButton Command1 
      Cancel          =   -1  'True
      Caption         =   "&Cancel"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   7.8
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   375
      Index           =   1
      Left            =   3098
      TabIndex        =   3
      Top             =   840
      Width           =   1095
   End
   Begin VB.CommandButton Command1 
      Caption         =   "&Add"
      Default         =   -1  'True
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   7.8
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   375
      Index           =   0
      Left            =   1778
      TabIndex        =   2
      Top             =   840
      Width           =   1095
   End
   Begin VB.TextBox Text1 
      Height          =   375
      Left            =   2520
      TabIndex        =   1
      Top             =   240
      Width           =   3135
   End
   Begin VB.Label Label1 
      Caption         =   "Name of new location:"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   7.8
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   255
      Left            =   240
      TabIndex        =   0
      Top             =   300
      Width           =   2055
   End
End
Attribute VB_Name = "frmMapAddPt"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
'Copyright 2000 by AQUA TERRA Consultants

Public ok As Boolean
Public ptname$

Private Sub Command1_Click(Index As Integer)
  If Index = 0 Then ok = True Else ok = False
  Unload Me
End Sub

Private Sub Form_Load()
  ok = False
End Sub

Private Sub Text1_Change()
  ptname = Text1.Text
End Sub
