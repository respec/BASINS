VERSION 5.00
Begin VB.Form frmGenActObs 
   Caption         =   "GenScn Activate OBSERVED"
   ClientHeight    =   2244
   ClientLeft      =   4320
   ClientTop       =   2988
   ClientWidth     =   2952
   BeginProperty Font 
      Name            =   "MS Sans Serif"
      Size            =   7.8
      Charset         =   0
      Weight          =   700
      Underline       =   0   'False
      Italic          =   0   'False
      Strikethrough   =   0   'False
   EndProperty
   HelpContextID   =   53
   Icon            =   "GenObs.frx":0000
   LinkTopic       =   "Form1"
   PaletteMode     =   1  'UseZOrder
   ScaleHeight     =   2244
   ScaleWidth      =   2952
   Begin VB.OptionButton optFormat 
      Caption         =   "&RDB"
      Enabled         =   0   'False
      Height          =   252
      Index           =   2
      Left            =   360
      TabIndex        =   5
      Top             =   1200
      Width           =   2535
   End
   Begin VB.OptionButton optFormat 
      Caption         =   "Watstore &Unit Values"
      Enabled         =   0   'False
      Height          =   252
      Index           =   1
      Left            =   360
      TabIndex        =   4
      Top             =   840
      Width           =   2535
   End
   Begin VB.OptionButton optFormat 
      Caption         =   "Watstore &Daily Values"
      Height          =   252
      Index           =   0
      Left            =   360
      TabIndex        =   3
      Top             =   480
      Value           =   -1  'True
      Width           =   2535
   End
   Begin VB.CommandButton cmdOkCancel 
      Cancel          =   -1  'True
      Caption         =   "Cancel"
      Height          =   375
      Index           =   1
      Left            =   1560
      TabIndex        =   1
      Top             =   1680
      Width           =   972
   End
   Begin VB.CommandButton cmdOkCancel 
      Caption         =   "&OK"
      Default         =   -1  'True
      Height          =   375
      Index           =   0
      Left            =   360
      TabIndex        =   0
      Top             =   1680
      Width           =   972
   End
   Begin VB.Label lblFormat 
      Caption         =   "Input Data Format:"
      Height          =   255
      Left            =   120
      TabIndex        =   2
      Top             =   120
      Width           =   2415
   End
End
Attribute VB_Name = "frmGenActObs"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
'Copyright 2000 by AQUA TERRA Consultants
Private Sub cmdOkCancel_Click(index As Integer)
  If index = 0 Then
    'okay
    If optFormat(0) = True Then
      'watstore daily values
      Unload frmGenScnImportWD
      frmGenScnImportWD.Show
      Unload frmGenActObs
    ElseIf optFormat(1) = True Then
      MsgBox "Importing Watstore Unit Values format not yet supported.", , "GenScn Activate OBSERVED"
'    ElseIf optFormat(2) = True Then 'RDB data
'      frmGenScnImportRDB.Show
'      Unload frmGenActObs
    End If
  ElseIf index = 1 Then
    'cancel
    Unload frmGenActObs
    frmGenScn.SetFocus
    'p.StatusFilePath = CurDir
    Call frmGenScn.RefreshMain
  End If
End Sub


