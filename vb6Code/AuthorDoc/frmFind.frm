VERSION 5.00
Begin VB.Form frmFind 
   Caption         =   "Find"
   ClientHeight    =   1875
   ClientLeft      =   45
   ClientTop       =   270
   ClientWidth     =   4905
   Icon            =   "frmFind.frx":0000
   LinkTopic       =   "Form1"
   ScaleHeight     =   1875
   ScaleWidth      =   4905
   StartUpPosition =   3  'Windows Default
   Begin VB.CommandButton cmdClose 
      Cancel          =   -1  'True
      Caption         =   "Done"
      Height          =   252
      Left            =   3840
      TabIndex        =   9
      Top             =   1440
      Width           =   852
   End
   Begin VB.CheckBox chkCase 
      Caption         =   "Match Case"
      Height          =   252
      Left            =   1320
      TabIndex        =   8
      Top             =   1320
      Width           =   2052
   End
   Begin VB.CheckBox chkScope 
      Caption         =   "Search Whole Project"
      Height          =   252
      Left            =   1320
      TabIndex        =   7
      Top             =   960
      Width           =   1932
   End
   Begin VB.CommandButton cmdReplaceAll 
      Caption         =   "Replace All"
      Height          =   492
      Left            =   3840
      TabIndex        =   6
      Top             =   840
      Width           =   852
   End
   Begin VB.CommandButton cmdReplace 
      Caption         =   "Replace"
      Height          =   252
      Left            =   3840
      TabIndex        =   5
      Top             =   480
      Width           =   852
   End
   Begin VB.CommandButton cmdFind 
      Caption         =   "Find"
      Default         =   -1  'True
      Height          =   252
      Left            =   3840
      TabIndex        =   4
      Top             =   120
      Width           =   852
   End
   Begin VB.TextBox txtReplace 
      Height          =   288
      Left            =   1320
      TabIndex        =   3
      Top             =   480
      Width           =   2412
   End
   Begin VB.TextBox txtFind 
      Height          =   288
      Left            =   1320
      TabIndex        =   2
      Top             =   120
      Width           =   2412
   End
   Begin VB.Label Label1 
      Caption         =   "Replace With:"
      Height          =   252
      Left            =   120
      TabIndex        =   1
      Top             =   480
      Width           =   1212
   End
   Begin VB.Label lblFind 
      Caption         =   "Find Text:"
      Height          =   252
      Left            =   120
      TabIndex        =   0
      Top             =   120
      Width           =   1212
   End
End
Attribute VB_Name = "frmFind"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
'Copyright 2000 by AQUA TERRA Consultants

