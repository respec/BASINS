VERSION 5.00
Object = "{F9043C88-F6F2-101A-A3C9-08002B2F49FB}#1.2#0"; "COMDLG32.OCX"
Begin VB.Form frmConvert 
   Caption         =   "HSPF Documentation Hypertext Converter"
   ClientHeight    =   5568
   ClientLeft      =   2448
   ClientTop       =   696
   ClientWidth     =   2436
   LinkTopic       =   "Form1"
   PaletteMode     =   1  'UseZOrder
   ScaleHeight     =   5568
   ScaleWidth      =   2436
   Begin VB.TextBox Text1 
      BackColor       =   &H8000000F&
      Height          =   855
      Left            =   120
      MultiLine       =   -1  'True
      TabIndex        =   10
      Top             =   4560
      Width           =   2175
   End
   Begin VB.Frame frameConvertTo 
      Caption         =   "Convert to:"
      Height          =   4332
      Left            =   120
      TabIndex        =   0
      Top             =   120
      Width           =   2175
      Begin VB.OptionButton optTargetFormat 
         Caption         =   "HTML Help"
         Height          =   255
         HelpContextID   =   2
         Index           =   4
         Left            =   120
         TabIndex        =   11
         Top             =   840
         Width           =   1935
      End
      Begin VB.CheckBox chkID 
         Caption         =   "HelpContextID File"
         Height          =   255
         Left            =   120
         TabIndex        =   9
         Top             =   1680
         Value           =   1  'Checked
         Width           =   1935
      End
      Begin VB.CheckBox UpNextCheck 
         Caption         =   "Up/Next Navigation"
         Height          =   255
         Left            =   120
         TabIndex        =   8
         Top             =   2400
         Width           =   1935
      End
      Begin VB.CheckBox TimestampCheck 
         Caption         =   "Footer Timestamps"
         Height          =   255
         Left            =   120
         TabIndex        =   7
         Top             =   3120
         Width           =   1815
      End
      Begin VB.CommandButton cmdConvert 
         Caption         =   "Convert"
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
         Left            =   480
         TabIndex        =   6
         Top             =   3720
         Width           =   1095
      End
      Begin VB.CheckBox ContentsCheck 
         Caption         =   "Contents"
         Height          =   255
         Left            =   120
         TabIndex        =   4
         Top             =   2760
         Value           =   1  'Checked
         Width           =   1935
      End
      Begin VB.OptionButton optTargetFormat 
         Caption         =   "Windows Help"
         Height          =   255
         HelpContextID   =   22
         Index           =   3
         Left            =   120
         TabIndex        =   3
         Top             =   600
         Width           =   1935
      End
      Begin VB.OptionButton optTargetFormat 
         Caption         =   "Printable Document"
         Height          =   255
         HelpContextID   =   21
         Index           =   2
         Left            =   120
         TabIndex        =   2
         Top             =   360
         Value           =   -1  'True
         Width           =   1935
      End
      Begin VB.OptionButton optTargetFormat 
         Caption         =   "HTML Pages"
         Height          =   255
         HelpContextID   =   2
         Index           =   1
         Left            =   120
         TabIndex        =   1
         Top             =   1080
         Width           =   1935
      End
      Begin VB.CheckBox ProjectCheck 
         Caption         =   "Project File"
         Height          =   255
         Left            =   120
         TabIndex        =   5
         Top             =   2040
         Width           =   1935
      End
   End
   Begin MSComDlg.CommonDialog CmDialog1 
      Left            =   0
      Top             =   0
      _ExtentX        =   847
      _ExtentY        =   847
      _Version        =   393216
   End
End
Attribute VB_Name = "frmConvert"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
Private TargetFormat%

Private Sub cmdConvert_Click()
  Dim contents As Boolean
  Dim list As Boolean
  Dim timestamps As Boolean
  Dim UpNext As Boolean
  Dim id As Boolean

  If ContentsCheck.Value = 1 Then contents = True Else contents = False
  If TimestampCheck.Value = 1 Then timestamps = True Else timestamps = False
  If UpNextCheck.Value = 1 Then UpNext = True Else UpNext = False
  If chkID.Value = 1 Then id = True Else id = False
  Convert TargetFormat, contents, timestamps, UpNext, id
  If TargetFormat = tHELP Then
    If ProjectCheck.Value = 1 Then CreateHelpProject True
  End If
  Beep
  Unload Me
End Sub

Private Sub Form_Load()
  SetUnInitialized
  Text1.Text = ""
  optTargetFormat_Click tPRINT
End Sub

Private Sub optTargetFormat_Click(Index As Integer)
  TargetFormat = Index
  ContentsCheck.Value = vbChecked
  ContentsCheck.Enabled = True
  Select Case Index
    Case tPRINT
      UpNextCheck.Value = vbUnchecked
      UpNextCheck.Enabled = False
      
      TimestampCheck.Enabled = True
      
      ProjectCheck.Value = vbUnchecked
      ProjectCheck.Enabled = False
      
      chkID.Value = vbUnchecked
      chkID.Enabled = False
    Case tHELP
      UpNextCheck.Enabled = True
      UpNextCheck.Value = vbChecked
      
      TimestampCheck.Value = vbUnchecked
      TimestampCheck.Enabled = False
      
      ProjectCheck.Enabled = True
      ProjectCheck.Value = vbUnchecked
      
      chkID.Enabled = True
      chkID.Value = vbUnchecked
    Case tHTMLHELP
      UpNextCheck.Enabled = True
      UpNextCheck.Value = vbChecked
      
      TimestampCheck.Value = vbUnchecked
      TimestampCheck.Enabled = False
      
      ProjectCheck.Enabled = True
      ProjectCheck.Value = vbUnchecked
      
      chkID.Enabled = True
      chkID.Value = vbUnchecked
    Case tHTML
      UpNextCheck.Enabled = True
      UpNextCheck.Value = vbChecked
      
      TimestampCheck.Value = vbUnchecked
      TimestampCheck.Enabled = False
      
      ProjectCheck.Value = vbUnchecked
      ProjectCheck.Enabled = False
      
      chkID.Value = vbUnchecked
      chkID.Enabled = False
  End Select
End Sub
