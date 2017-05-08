VERSION 5.00
Begin VB.Form frmMapLabel 
   Caption         =   "Fields"
   ClientHeight    =   3480
   ClientLeft      =   60
   ClientTop       =   348
   ClientWidth     =   4500
   HelpContextID   =   523
   LinkTopic       =   "Form1"
   ScaleHeight     =   3480
   ScaleWidth      =   4500
   StartUpPosition =   3  'Windows Default
   Begin VB.ComboBox comboLength 
      Height          =   288
      Left            =   2400
      Style           =   2  'Dropdown List
      TabIndex        =   15
      Top             =   2520
      Width           =   1935
   End
   Begin VB.ComboBox comboDownstream 
      Height          =   288
      Left            =   2400
      Style           =   2  'Dropdown List
      TabIndex        =   13
      Top             =   2160
      Width           =   1935
   End
   Begin VB.ComboBox comboBranch 
      Height          =   288
      Left            =   2400
      Style           =   2  'Dropdown List
      TabIndex        =   11
      Top             =   1800
      Width           =   1935
   End
   Begin MSComDlg.CommonDialog cdlFont 
      Left            =   3840
      Top             =   2280
      _ExtentX        =   847
      _ExtentY        =   847
      _Version        =   393216
   End
   Begin VB.CommandButton cmdClose 
      Caption         =   "&Ok"
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
      Index           =   1
      Left            =   240
      TabIndex        =   8
      Top             =   3000
      Width           =   1095
   End
   Begin VB.CommandButton cmdClose 
      Cancel          =   -1  'True
      Caption         =   "Cancel"
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
      Left            =   1740
      TabIndex        =   9
      Top             =   3000
      Width           =   1095
   End
   Begin VB.CommandButton cmdFont 
      Caption         =   "&Font..."
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
      Left            =   3240
      TabIndex        =   10
      Top             =   3000
      Width           =   1095
   End
   Begin VB.ComboBox comboKeyField 
      Height          =   288
      Left            =   2400
      Style           =   2  'Dropdown List
      TabIndex        =   3
      Top             =   1440
      Width           =   1935
   End
   Begin VB.ComboBox comboLabelField 
      Height          =   288
      Left            =   2400
      Style           =   2  'Dropdown List
      TabIndex        =   1
      Top             =   240
      Width           =   1935
   End
   Begin VB.ComboBox comboHorizAlign 
      Height          =   288
      Left            =   2400
      Style           =   2  'Dropdown List
      TabIndex        =   7
      Top             =   960
      Width           =   1935
   End
   Begin VB.ComboBox comboVertAlign 
      Height          =   288
      ItemData        =   "MapLabel.frx":0000
      Left            =   2400
      List            =   "MapLabel.frx":0002
      Style           =   2  'Dropdown List
      TabIndex        =   5
      Top             =   600
      Width           =   1935
   End
   Begin VB.Label Label1 
      Caption         =   "&Length field in DBF:"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   7.8
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   252
      Index           =   6
      Left            =   240
      TabIndex        =   16
      Top             =   2592
      Width           =   2172
   End
   Begin VB.Label Label1 
      Caption         =   "&Downstream field in DBF:"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   7.8
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   252
      Index           =   5
      Left            =   240
      TabIndex        =   14
      Top             =   2232
      Width           =   2172
   End
   Begin VB.Label Label1 
      Caption         =   "&Branch field in DBF:"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   7.8
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   252
      Index           =   4
      Left            =   240
      TabIndex        =   12
      Top             =   1872
      Width           =   2052
   End
   Begin VB.Label Label1 
      Caption         =   "Location &Key in DBF:"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   7.8
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   252
      Index           =   3
      Left            =   240
      TabIndex        =   2
      Top             =   1512
      Width           =   2052
   End
   Begin VB.Label Label1 
      Caption         =   "Label &Field in DBF:"
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
      Index           =   2
      Left            =   240
      TabIndex        =   0
      Top             =   285
      Width           =   1815
   End
   Begin VB.Label Label1 
      Caption         =   "&Horizontal Alignment:"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   7.8
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   252
      Index           =   1
      Left            =   480
      TabIndex        =   6
      Top             =   1032
      Width           =   1812
   End
   Begin VB.Label Label1 
      Caption         =   "&Vertical Alignment:"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   7.8
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   252
      Index           =   0
      Left            =   480
      TabIndex        =   4
      Top             =   648
      Width           =   1812
   End
End
Attribute VB_Name = "frmMapLabel"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
'Copyright 2000 by AQUA TERRA Consultants

'This form's controls are initialized and interpreted by code in frmMapCov

Public MapLabelOk As Boolean

Private Sub cmdClose_Click(Index As Integer)
  If Index = 1 Then MapLabelOk = True Else MapLabelOk = False
  Me.Hide 'Me.unload
End Sub

Private Sub cmdFont_Click()
  cdlFont.ShowFont
End Sub

Private Sub Form_Load()
  MapLabelOk = False
End Sub
