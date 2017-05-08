VERSION 5.00
Begin VB.Form frmGenRDBInit 
   Caption         =   "RDB Data Initialization"
   ClientHeight    =   1608
   ClientLeft      =   3060
   ClientTop       =   2364
   ClientWidth     =   7308
   BeginProperty Font 
      Name            =   "MS Sans Serif"
      Size            =   7.8
      Charset         =   0
      Weight          =   700
      Underline       =   0   'False
      Italic          =   0   'False
      Strikethrough   =   0   'False
   EndProperty
   Icon            =   "GenRDBIn.frx":0000
   LinkTopic       =   "Form1"
   PaletteMode     =   1  'UseZOrder
   ScaleHeight     =   1608
   ScaleWidth      =   7308
   Tag             =   "0"
   Begin VB.CommandButton cmdClose 
      Cancel          =   -1  'True
      Caption         =   "&Cancel"
      Height          =   372
      Index           =   2
      Left            =   4200
      TabIndex        =   9
      Top             =   1080
      Width           =   972
   End
   Begin VB.CommandButton cmdClose 
      Caption         =   "&Skip"
      Height          =   372
      Index           =   1
      Left            =   2640
      TabIndex        =   8
      Top             =   1080
      Width           =   972
   End
   Begin VB.CommandButton cmdClose 
      Caption         =   "&OK"
      Default         =   -1  'True
      Height          =   372
      Index           =   0
      Left            =   1080
      TabIndex        =   6
      Top             =   1080
      Width           =   972
   End
   Begin VB.TextBox txtSLC 
      Height          =   288
      Index           =   2
      Left            =   6000
      TabIndex        =   5
      Top             =   600
      Width           =   1212
   End
   Begin VB.TextBox txtSLC 
      Height          =   288
      Index           =   1
      Left            =   3480
      TabIndex        =   3
      Top             =   600
      Width           =   1212
   End
   Begin VB.TextBox txtSLC 
      Height          =   288
      Index           =   0
      Left            =   1080
      TabIndex        =   1
      Top             =   600
      Width           =   1212
   End
   Begin VB.Label lblColHdr 
      Height          =   252
      Left            =   120
      TabIndex        =   7
      Top             =   120
      Width           =   6372
   End
   Begin VB.Label lblSLC 
      Caption         =   "Constituent:"
      Height          =   252
      Index           =   2
      Left            =   4920
      TabIndex        =   4
      Top             =   600
      Width           =   1092
   End
   Begin VB.Label lblSLC 
      Caption         =   "Location:"
      Height          =   252
      Index           =   1
      Left            =   2640
      TabIndex        =   2
      Top             =   600
      Width           =   972
   End
   Begin VB.Label lblSLC 
      Caption         =   "Scenario:"
      Height          =   252
      Index           =   0
      Left            =   120
      TabIndex        =   0
      Top             =   600
      Width           =   972
   End
End
Attribute VB_Name = "frmGenRDBInit"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
'Copyright 2002 by AQUA TERRA Consultants





Private Sub cmdClose_Click(index As Integer)

    If index = 0 Then 'OK, process this column
      frmGenRDBInit.Tag = 1
    ElseIf index = 1 Then 'skip this column
      frmGenRDBInit.Tag = 0
    ElseIf index = 2 Then 'cancel using this data
      frmGenRDBInit.Tag = -1
    End If
    frmGenRDBInit.Hide

End Sub


