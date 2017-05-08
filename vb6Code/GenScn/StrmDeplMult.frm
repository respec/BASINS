VERSION 5.00
Begin VB.Form frmStrmDeplMult 
   BorderStyle     =   1  'Fixed Single
   Caption         =   "GenScn Stream Depletion Multipliers"
   ClientHeight    =   5292
   ClientLeft      =   48
   ClientTop       =   336
   ClientWidth     =   3708
   Icon            =   "StrmDeplMult.frx":0000
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   5292
   ScaleWidth      =   3708
   Begin ATCoCtl.ATCoText AtxMult 
      Height          =   255
      Index           =   0
      Left            =   1920
      TabIndex        =   1
      Top             =   240
      Width           =   1215
      _ExtentX        =   2138
      _ExtentY        =   445
      InsideLimitsBackground=   16777215
      OutsideHardLimitBackground=   8421631
      OutsideSoftLimitBackground=   8454143
      HardMax         =   -999
      HardMin         =   -999
      SoftMax         =   -999
      SoftMin         =   -999
      MaxWidth        =   5
      Alignment       =   1
      DataType        =   2
      DefaultValue    =   "1.0"
      Value           =   "1.0"
      Enabled         =   -1  'True
   End
   Begin VB.CommandButton cmdCancel 
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
      Left            =   1920
      TabIndex        =   25
      Top             =   4680
      Width           =   975
   End
   Begin VB.CommandButton cmdOK 
      Caption         =   "O&K"
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
      Left            =   720
      TabIndex        =   24
      Top             =   4680
      Width           =   975
   End
   Begin ATCoCtl.ATCoText AtxMult 
      Height          =   255
      Index           =   1
      Left            =   1920
      TabIndex        =   3
      Top             =   600
      Width           =   1215
      _ExtentX        =   2138
      _ExtentY        =   445
      InsideLimitsBackground=   16777215
      OutsideHardLimitBackground=   8421631
      OutsideSoftLimitBackground=   8454143
      HardMax         =   -999
      HardMin         =   -999
      SoftMax         =   -999
      SoftMin         =   -999
      MaxWidth        =   5
      Alignment       =   1
      DataType        =   2
      DefaultValue    =   "1.0"
      Value           =   "1.0"
      Enabled         =   -1  'True
   End
   Begin ATCoCtl.ATCoText AtxMult 
      Height          =   255
      Index           =   2
      Left            =   1920
      TabIndex        =   5
      Top             =   960
      Width           =   1215
      _ExtentX        =   2138
      _ExtentY        =   445
      InsideLimitsBackground=   16777215
      OutsideHardLimitBackground=   8421631
      OutsideSoftLimitBackground=   8454143
      HardMax         =   -999
      HardMin         =   -999
      SoftMax         =   -999
      SoftMin         =   -999
      MaxWidth        =   5
      Alignment       =   1
      DataType        =   2
      DefaultValue    =   "1.0"
      Value           =   "1.0"
      Enabled         =   -1  'True
   End
   Begin ATCoCtl.ATCoText AtxMult 
      Height          =   255
      Index           =   3
      Left            =   1920
      TabIndex        =   7
      Top             =   1320
      Width           =   1215
      _ExtentX        =   2138
      _ExtentY        =   445
      InsideLimitsBackground=   16777215
      OutsideHardLimitBackground=   8421631
      OutsideSoftLimitBackground=   8454143
      HardMax         =   -999
      HardMin         =   -999
      SoftMax         =   -999
      SoftMin         =   -999
      MaxWidth        =   5
      Alignment       =   1
      DataType        =   2
      DefaultValue    =   "1.0"
      Value           =   "1.0"
      Enabled         =   -1  'True
   End
   Begin ATCoCtl.ATCoText AtxMult 
      Height          =   255
      Index           =   4
      Left            =   1920
      TabIndex        =   9
      Top             =   1680
      Width           =   1215
      _ExtentX        =   2138
      _ExtentY        =   445
      InsideLimitsBackground=   16777215
      OutsideHardLimitBackground=   8421631
      OutsideSoftLimitBackground=   8454143
      HardMax         =   -999
      HardMin         =   -999
      SoftMax         =   -999
      SoftMin         =   -999
      MaxWidth        =   5
      Alignment       =   1
      DataType        =   2
      DefaultValue    =   "1.0"
      Value           =   "1.0"
      Enabled         =   -1  'True
   End
   Begin ATCoCtl.ATCoText AtxMult 
      Height          =   255
      Index           =   5
      Left            =   1920
      TabIndex        =   11
      Top             =   2040
      Width           =   1215
      _ExtentX        =   2138
      _ExtentY        =   445
      InsideLimitsBackground=   16777215
      OutsideHardLimitBackground=   8421631
      OutsideSoftLimitBackground=   8454143
      HardMax         =   -999
      HardMin         =   -999
      SoftMax         =   -999
      SoftMin         =   -999
      MaxWidth        =   5
      Alignment       =   1
      DataType        =   2
      DefaultValue    =   "1.0"
      Value           =   "1.0"
      Enabled         =   -1  'True
   End
   Begin ATCoCtl.ATCoText AtxMult 
      Height          =   255
      Index           =   6
      Left            =   1920
      TabIndex        =   13
      Top             =   2400
      Width           =   1215
      _ExtentX        =   2138
      _ExtentY        =   445
      InsideLimitsBackground=   16777215
      OutsideHardLimitBackground=   8421631
      OutsideSoftLimitBackground=   8454143
      HardMax         =   -999
      HardMin         =   -999
      SoftMax         =   -999
      SoftMin         =   -999
      MaxWidth        =   5
      Alignment       =   1
      DataType        =   2
      DefaultValue    =   "1.0"
      Value           =   "1.0"
      Enabled         =   -1  'True
   End
   Begin ATCoCtl.ATCoText AtxMult 
      Height          =   255
      Index           =   7
      Left            =   1920
      TabIndex        =   15
      Top             =   2760
      Width           =   1215
      _ExtentX        =   2138
      _ExtentY        =   445
      InsideLimitsBackground=   16777215
      OutsideHardLimitBackground=   8421631
      OutsideSoftLimitBackground=   8454143
      HardMax         =   -999
      HardMin         =   -999
      SoftMax         =   -999
      SoftMin         =   -999
      MaxWidth        =   5
      Alignment       =   1
      DataType        =   2
      DefaultValue    =   "1.0"
      Value           =   "1.0"
      Enabled         =   -1  'True
   End
   Begin ATCoCtl.ATCoText AtxMult 
      Height          =   255
      Index           =   8
      Left            =   1920
      TabIndex        =   17
      Top             =   3120
      Width           =   1215
      _ExtentX        =   2138
      _ExtentY        =   445
      InsideLimitsBackground=   16777215
      OutsideHardLimitBackground=   8421631
      OutsideSoftLimitBackground=   8454143
      HardMax         =   -999
      HardMin         =   -999
      SoftMax         =   -999
      SoftMin         =   -999
      MaxWidth        =   5
      Alignment       =   1
      DataType        =   2
      DefaultValue    =   "1.0"
      Value           =   "1.0"
      Enabled         =   -1  'True
   End
   Begin ATCoCtl.ATCoText AtxMult 
      Height          =   255
      Index           =   9
      Left            =   1920
      TabIndex        =   19
      Top             =   3480
      Width           =   1215
      _ExtentX        =   2138
      _ExtentY        =   445
      InsideLimitsBackground=   16777215
      OutsideHardLimitBackground=   8421631
      OutsideSoftLimitBackground=   8454143
      HardMax         =   -999
      HardMin         =   -999
      SoftMax         =   -999
      SoftMin         =   -999
      MaxWidth        =   5
      Alignment       =   1
      DataType        =   2
      DefaultValue    =   "1.0"
      Value           =   "1.0"
      Enabled         =   -1  'True
   End
   Begin ATCoCtl.ATCoText AtxMult 
      Height          =   255
      Index           =   10
      Left            =   1920
      TabIndex        =   21
      Top             =   3840
      Width           =   1215
      _ExtentX        =   2138
      _ExtentY        =   445
      InsideLimitsBackground=   16777215
      OutsideHardLimitBackground=   8421631
      OutsideSoftLimitBackground=   8454143
      HardMax         =   -999
      HardMin         =   -999
      SoftMax         =   -999
      SoftMin         =   -999
      MaxWidth        =   5
      Alignment       =   1
      DataType        =   2
      DefaultValue    =   "1.0"
      Value           =   "1.0"
      Enabled         =   -1  'True
   End
   Begin ATCoCtl.ATCoText AtxMult 
      Height          =   255
      Index           =   11
      Left            =   1920
      TabIndex        =   23
      Top             =   4200
      Width           =   1215
      _ExtentX        =   2138
      _ExtentY        =   445
      InsideLimitsBackground=   16777215
      OutsideHardLimitBackground=   8421631
      OutsideSoftLimitBackground=   8454143
      HardMax         =   -999
      HardMin         =   -999
      SoftMax         =   -999
      SoftMin         =   -999
      MaxWidth        =   5
      Alignment       =   1
      DataType        =   2
      DefaultValue    =   "1.0"
      Value           =   "1.0"
      Enabled         =   -1  'True
   End
   Begin VB.Label Label1 
      Caption         =   "&December"
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
      Index           =   11
      Left            =   600
      TabIndex        =   22
      Top             =   4200
      Width           =   1095
   End
   Begin VB.Label Label1 
      Caption         =   "&November"
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
      Index           =   10
      Left            =   600
      TabIndex        =   20
      Top             =   3840
      Width           =   1095
   End
   Begin VB.Label Label1 
      Caption         =   "&October"
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
      Index           =   9
      Left            =   600
      TabIndex        =   18
      Top             =   3480
      Width           =   1095
   End
   Begin VB.Label Label1 
      Caption         =   "&September"
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
      Index           =   8
      Left            =   600
      TabIndex        =   16
      Top             =   3120
      Width           =   1095
   End
   Begin VB.Label Label1 
      Caption         =   "Au&gust"
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
      Index           =   7
      Left            =   600
      TabIndex        =   14
      Top             =   2760
      Width           =   1095
   End
   Begin VB.Label Label1 
      Caption         =   "Ju&ly"
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
      Index           =   6
      Left            =   600
      TabIndex        =   12
      Top             =   2400
      Width           =   1095
   End
   Begin VB.Label Label1 
      Caption         =   "J&une"
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
      Index           =   5
      Left            =   600
      TabIndex        =   10
      Top             =   2040
      Width           =   1095
   End
   Begin VB.Label Label1 
      Caption         =   "Ma&y"
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
      Index           =   4
      Left            =   600
      TabIndex        =   8
      Top             =   1680
      Width           =   1095
   End
   Begin VB.Label Label1 
      Caption         =   "&April"
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
      Index           =   3
      Left            =   600
      TabIndex        =   6
      Top             =   1320
      Width           =   1095
   End
   Begin VB.Label Label1 
      Caption         =   "&March"
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
      Left            =   600
      TabIndex        =   4
      Top             =   960
      Width           =   1095
   End
   Begin VB.Label Label1 
      Caption         =   "&February"
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
      Index           =   1
      Left            =   600
      TabIndex        =   2
      Top             =   600
      Width           =   1095
   End
   Begin VB.Label Label1 
      Caption         =   "&January"
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
      Index           =   0
      Left            =   600
      TabIndex        =   0
      Top             =   240
      Width           =   1095
   End
End
Attribute VB_Name = "frmStrmDeplMult"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
'Copyright 2000 by AQUA TERRA Consultants
Private Sub cmdOk_Click()
  Dim i&, rval#(12)
  For i = 0 To 11
    rval(i) = AtxMult(i).value
  Next i
  Call frmStrmDeplInterface.PutMult(rval)
  Unload Me
End Sub
Private Sub cmdCancel_Click()
  Unload Me
End Sub
Private Sub Form_Load()
  Dim i&, rval#(12)
  Call frmStrmDeplInterface.GetMult(rval)
  For i = 0 To 11
    AtxMult(i).value = rval(i)
  Next i
End Sub
