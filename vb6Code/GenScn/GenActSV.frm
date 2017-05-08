VERSION 5.00
Begin VB.Form frmGenActSV 
   BorderStyle     =   1  'Fixed Single
   Caption         =   "GenScn Activate SaveAs"
   ClientHeight    =   2436
   ClientLeft      =   36
   ClientTop       =   276
   ClientWidth     =   5304
   HelpContextID   =   53
   Icon            =   "GenActSV.frx":0000
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   2436
   ScaleWidth      =   5304
   StartUpPosition =   3  'Windows Default
   Begin MSComDlg.CommonDialog cmdFile 
      Left            =   4320
      Top             =   1920
      _ExtentX        =   847
      _ExtentY        =   847
      _Version        =   393216
      DefaultExt      =   "uci"
      DialogTitle     =   "Select Scenario Path"
      Filter          =   "*.uci"
   End
   Begin VB.TextBox txtPath 
      BackColor       =   &H80000004&
      Height          =   285
      Left            =   720
      Locked          =   -1  'True
      TabIndex        =   4
      Text            =   "curPath"
      Top             =   480
      Width           =   4455
   End
   Begin VB.CommandButton cmdBrowse 
      Caption         =   "&Browse"
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
      Left            =   4080
      TabIndex        =   2
      Top             =   120
      Width           =   1095
   End
   Begin ATCoCtl.ATCoText ATCoTextBase 
      Height          =   252
      Left            =   1560
      TabIndex        =   7
      Top             =   1320
      Width           =   1092
      _ExtentX        =   1926
      _ExtentY        =   445
      InsideLimitsBackground=   16777215
      OutsideHardLimitBackground=   8421631
      OutsideSoftLimitBackground=   8454143
      HardMax         =   10000
      HardMin         =   0
      SoftMax         =   -999
      SoftMin         =   -999
      MaxWidth        =   5
      Alignment       =   1
      DataType        =   1
      DefaultValue    =   "1000"
      Value           =   "1000"
      Enabled         =   -1  'True
   End
   Begin VB.OptionButton optRelAbs 
      Caption         =   "Absolute"
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
      Left            =   3120
      TabIndex        =   9
      Top             =   1440
      Value           =   -1  'True
      Width           =   1812
   End
   Begin VB.OptionButton optRelAbs 
      Caption         =   "Relative"
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
      Left            =   3120
      TabIndex        =   8
      Top             =   1200
      Width           =   1812
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
      Height          =   372
      Left            =   2520
      TabIndex        =   11
      Top             =   1920
      Width           =   1092
   End
   Begin VB.CommandButton cmdOkay 
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
      Height          =   372
      Left            =   1320
      TabIndex        =   10
      Top             =   1920
      Width           =   1092
   End
   Begin VB.TextBox txtName 
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   7.8
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   288
      Left            =   2760
      TabIndex        =   1
      Top             =   120
      Width           =   1095
   End
   Begin VB.Label lblPath 
      Caption         =   "Path:"
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
      Left            =   120
      TabIndex        =   3
      Top             =   480
      Width           =   615
   End
   Begin VB.Label lblBase 
      Caption         =   "&Base DSN:"
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
      Left            =   360
      TabIndex        =   6
      Top             =   1320
      Width           =   1092
   End
   Begin VB.Label lblDsn 
      Caption         =   "Number Data Sets As Follows:"
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
      Left            =   120
      TabIndex        =   5
      Top             =   960
      Width           =   4812
   End
   Begin VB.Label lblEnter 
      Caption         =   "New &Scenario (8 chars max):"
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
      Left            =   120
      TabIndex        =   0
      Top             =   120
      Width           =   2655
   End
End
Attribute VB_Name = "frmGenActSV"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
Dim lCurDir$
'Copyright 2000 by AQUA TERRA Consultants

Private Sub cmdBrowse_Click()
  cmdFile.Filename = txtName & ".uci"
  cmdFile.InitDir = CurDir
  cmdFile.CancelError = True
  On Error GoTo UserCancel:
  cmdFile.ShowSave
  lCurDir = PathNameOnly(cmdFile.Filename)
  txtName = FilenameOnly(cmdFile.Filename)
  txtName_Change
  Exit Sub
UserCancel:
End Sub

Private Sub cmdCancel_Click()
  frmGenScnActivate.newName = ""
  Unload Me
End Sub

Private Sub cmdOkay_Click()
  frmGenScnActivate.newName = txtName.text
  If optRelAbs(0).value Then
    frmGenScnActivate.RelAbs = 0
  Else
    frmGenScnActivate.RelAbs = 1
  End If
  frmGenScnActivate.BaseDSN = ATCoTextBase.value
  Unload Me
End Sub

Private Sub Form_Load()
  lCurDir = PathNameOnly(AbsolutePath(frmGenScnActivate.ScenFile, CurDir))
  txtName.text = FilenameOnly(frmGenScnActivate.ScenFile)
  frmGenScnActivate.newName = ""
End Sub

Private Sub txtName_Change()
  If Len(txtName) > 8 Then txtName = Left(txtName, 8)
  txtPath = lCurDir & "\" & txtName & ".uci"
End Sub

