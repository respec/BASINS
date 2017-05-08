VERSION 5.00
Object = "{F9043C88-F6F2-101A-A3C9-08002B2F49FB}#1.2#0"; "COMDLG32.OCX"
Begin VB.Form frmConvert 
   Caption         =   "HSPF Documentation Hypertext Converter"
   ClientHeight    =   5400
   ClientLeft      =   2445
   ClientTop       =   690
   ClientWidth     =   2850
   Icon            =   "Convert.frx":0000
   LinkTopic       =   "Form1"
   PaletteMode     =   1  'UseZOrder
   ScaleHeight     =   5400
   ScaleWidth      =   2850
   Begin VB.TextBox Text1 
      BackColor       =   &H8000000F&
      Height          =   855
      Left            =   120
      MultiLine       =   -1  'True
      TabIndex        =   10
      Top             =   4440
      Width           =   2535
   End
   Begin VB.Frame frameConvertTo 
      Caption         =   "Convert to:"
      Height          =   4212
      Left            =   120
      TabIndex        =   0
      Top             =   120
      Width           =   2535
      Begin VB.CommandButton cmdConvert 
         Caption         =   "Preview"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   9.75
            Charset         =   0
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   375
         Index           =   1
         Left            =   1320
         TabIndex        =   13
         Top             =   3720
         Width           =   1095
      End
      Begin VB.OptionButton optTargetFormat 
         Caption         =   "From HSPF Manual"
         Height          =   255
         HelpContextID   =   2
         Index           =   0
         Left            =   120
         TabIndex        =   12
         Top             =   1440
         Width           =   2295
      End
      Begin VB.OptionButton optTargetFormat 
         Caption         =   "HTML Help"
         Height          =   255
         HelpContextID   =   2
         Index           =   4
         Left            =   120
         TabIndex        =   11
         Top             =   900
         Width           =   2295
      End
      Begin VB.CheckBox chkID 
         Caption         =   "HelpContextID File"
         Height          =   255
         Left            =   120
         TabIndex        =   9
         Top             =   1920
         Value           =   1  'Checked
         Width           =   2295
      End
      Begin VB.CheckBox UpNextCheck 
         Caption         =   "Up/Next Navigation"
         Height          =   255
         Left            =   120
         TabIndex        =   8
         Top             =   2640
         Width           =   2295
      End
      Begin VB.CheckBox TimestampCheck 
         Caption         =   "Footer Timestamps"
         Height          =   255
         Left            =   120
         TabIndex        =   7
         Top             =   3360
         Width           =   2295
      End
      Begin VB.CommandButton cmdConvert 
         Caption         =   "Convert"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   9.75
            Charset         =   0
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   375
         Index           =   0
         Left            =   120
         TabIndex        =   6
         Top             =   3720
         Width           =   1095
      End
      Begin VB.CheckBox ContentsCheck 
         Caption         =   "Contents"
         Height          =   255
         Left            =   120
         TabIndex        =   4
         Top             =   3000
         Value           =   1  'Checked
         Width           =   2295
      End
      Begin VB.OptionButton optTargetFormat 
         Caption         =   "Windows Help"
         Height          =   255
         HelpContextID   =   22
         Index           =   3
         Left            =   120
         TabIndex        =   3
         Top             =   630
         Width           =   2295
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
         Width           =   2295
      End
      Begin VB.OptionButton optTargetFormat 
         Caption         =   "HTML Pages"
         Height          =   255
         HelpContextID   =   2
         Index           =   1
         Left            =   120
         TabIndex        =   1
         Top             =   1170
         Width           =   1935
      End
      Begin VB.CheckBox ProjectCheck 
         Caption         =   "Project File"
         Height          =   255
         Left            =   120
         TabIndex        =   5
         Top             =   2280
         Width           =   2295
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
'Copyright 2000 by AQUA TERRA Consultants
Private TargetFormat%
Private Const SectionConvert = "Convert Window"

Private Sub cmdConvert_Click(Index As Integer)
  Dim RememberProjectFileName As String
  Dim RememberBaseName As String
  Dim PreviewProjectFile As Integer
  
  Dim contents As Boolean
  Dim list As Boolean
  Dim timestamps As Boolean
  Dim UpNext As Boolean
  Dim id As Boolean
  Dim makeProject As Boolean

  If ContentsCheck.Value = 1 Then contents = True Else contents = False
  If TimestampCheck.Value = 1 Then timestamps = True Else timestamps = False
  If UpNextCheck.Value = 1 Then UpNext = True Else UpNext = False
  If chkID.Value = 1 Then id = True Else id = False
  If ProjectCheck.Value = 1 Then makeProject = True Else makeProject = False
  
  SaveSetting App.Title, SectionConvert, "Contents", contents
  SaveSetting App.Title, SectionConvert, "Timestamps", timestamps
  SaveSetting App.Title, SectionConvert, "UpNext", UpNext
  SaveSetting App.Title, SectionConvert, "ID", id
  SaveSetting App.Title, SectionConvert, "Project", makeProject
  SaveSetting App.Title, SectionConvert, "TargetFormat", TargetFormat
  Me.MousePointer = vbHourglass
  
  If Index = 1 Then 'Preview
    PreviewProjectFile = FreeFile
    RememberBaseName = BaseName
    RememberProjectFileName = ProjectFileName
    ProjectFileName = PathNameOnly(CurrentFilename) & "\PreviewProject.txt"
    BaseName = FilenameOnly(ProjectFileName)
    Open ProjectFileName For Output As PreviewProjectFile
    Print #PreviewProjectFile, FilenameOnly(CurrentFilename)
    Close PreviewProjectFile
  End If
  Convert TargetFormat, contents, timestamps, UpNext, id, makeProject
  If Index = 1 Then 'Preview
    Kill ProjectFileName
    BaseName = RememberBaseName
    ProjectFileName = RememberProjectFileName
  End If
  Beep
  Unload Me
End Sub

Private Sub Form_Load()
  Dim setting As Variant
  SetUnInitialized
  Text1.Text = ""
  setting = GetSetting(App.Title, SectionConvert, "TargetFormat", tPRINT)
  If IsNumeric(setting) Then
    optTargetFormat(setting).Value = True
    optTargetFormat_Click CLng(setting)
  End If
  Select Case GetSetting(App.Title, SectionConvert, "Contents", 0)
    Case True:    ContentsCheck.Value = vbChecked
    Case False:   ContentsCheck.Value = vbUnchecked
  End Select
  
  Select Case GetSetting(App.Title, SectionConvert, "Timestamps", 0)
    Case True:    TimestampCheck.Value = vbChecked
    Case False:   TimestampCheck.Value = vbUnchecked
  End Select
  
  Select Case GetSetting(App.Title, SectionConvert, "UpNext", 0)
    Case True:    UpNextCheck.Value = vbChecked
    Case False:   UpNextCheck.Value = vbUnchecked
  End Select
  
  Select Case GetSetting(App.Title, SectionConvert, "ID", 0)
    Case True:    chkID.Value = vbChecked
    Case False:   chkID.Value = vbUnchecked
  End Select
      
  Select Case GetSetting(App.Title, SectionConvert, "Project", 0)
    Case True:    ProjectCheck.Value = vbChecked
    Case False:   ProjectCheck.Value = vbUnchecked
  End Select
  
End Sub

Private Sub optTargetFormat_Click(Index As Integer)
  TargetFormat = Index
  ContentsCheck.Value = vbChecked
  ContentsCheck.Enabled = True
  Select Case Index
    Case tASCII
      UpNextCheck.Value = vbUnchecked
      UpNextCheck.Enabled = False
      TimestampCheck.Value = vbUnchecked
      TimestampCheck.Enabled = False
      ProjectCheck.Enabled = True
      ProjectCheck.Value = vbChecked
      chkID.Value = vbUnchecked
      chkID.Enabled = False
    
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
      UpNextCheck.Value = vbUnchecked
      
      TimestampCheck.Value = vbUnchecked
      TimestampCheck.Enabled = False
      
      ProjectCheck.Enabled = True
      ProjectCheck.Value = vbChecked
      
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
