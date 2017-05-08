VERSION 5.00
Object = "{F9043C88-F6F2-101A-A3C9-08002B2F49FB}#1.2#0"; "COMDLG32.OCX"
Begin VB.Form frmWizard 
   Caption         =   "BASINS Web Data Download"
   ClientHeight    =   2055
   ClientLeft      =   165
   ClientTop       =   735
   ClientWidth     =   9600
   HelpContextID   =   11
   Icon            =   "frmWizard.frx":0000
   LinkTopic       =   "Form1"
   ScaleHeight     =   2055
   ScaleWidth      =   9600
   StartUpPosition =   3  'Windows Default
   Begin VB.Frame fraStep 
      BorderStyle     =   0  'None
      Caption         =   "fraStep"
      Height          =   1575
      Index           =   4
      Left            =   0
      TabIndex        =   30
      Top             =   6600
      Width           =   6255
      Begin VB.Label lblDownloadStatus 
         BackStyle       =   0  'Transparent
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   735
         Index           =   4
         Left            =   0
         TabIndex        =   32
         Top             =   480
         Width           =   6135
      End
      Begin VB.Label lblStep 
         Caption         =   "Checking downloaded data and projecting if needed"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   255
         Index           =   4
         Left            =   120
         TabIndex        =   31
         Top             =   120
         Width           =   6135
      End
   End
   Begin VB.Frame fraButtons 
      BorderStyle     =   0  'None
      Height          =   615
      HelpContextID   =   11
      Left            =   480
      TabIndex        =   13
      Top             =   1440
      Width           =   5295
      Begin VB.CommandButton cmdAllNone 
         Caption         =   "Select &None"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   375
         HelpContextID   =   11
         Index           =   1
         Left            =   1200
         TabIndex        =   15
         Top             =   120
         Width           =   1335
      End
      Begin VB.CommandButton cmdCancel 
         Cancel          =   -1  'True
         Caption         =   "Cancel"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   375
         HelpContextID   =   11
         Left            =   4080
         TabIndex        =   18
         Top             =   120
         Width           =   1095
      End
      Begin VB.CommandButton cmdNext 
         Caption         =   "&Next >"
         Default         =   -1  'True
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   375
         HelpContextID   =   11
         Left            =   2760
         TabIndex        =   17
         Top             =   120
         Width           =   1095
      End
      Begin VB.CommandButton cmdFinish 
         Caption         =   "&Finish"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   375
         HelpContextID   =   11
         Left            =   2760
         TabIndex        =   16
         Top             =   120
         Visible         =   0   'False
         Width           =   1095
      End
      Begin VB.CommandButton cmdDetails 
         Caption         =   "Details..."
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   375
         HelpContextID   =   11
         Left            =   0
         TabIndex        =   27
         Top             =   120
         Visible         =   0   'False
         Width           =   1095
      End
      Begin VB.CommandButton cmdAllNone 
         Caption         =   "Select &All"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   375
         HelpContextID   =   11
         Index           =   0
         Left            =   0
         TabIndex        =   14
         Top             =   120
         Width           =   1095
      End
   End
   Begin VB.Frame fraStep 
      BorderStyle     =   0  'None
      Caption         =   "fraStep"
      Height          =   1695
      Index           =   3
      Left            =   0
      TabIndex        =   11
      Top             =   4800
      Width           =   6255
      Begin VB.Label lblDownloadStatus 
         BackStyle       =   0  'Transparent
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   735
         Index           =   3
         Left            =   120
         TabIndex        =   25
         Top             =   960
         Width           =   6135
      End
      Begin VB.Label lblStep 
         Caption         =   "Downloading data..."
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   735
         Index           =   3
         Left            =   120
         TabIndex        =   12
         Top             =   120
         Width           =   6135
      End
   End
   Begin VB.Frame fraStep 
      BorderStyle     =   0  'None
      Caption         =   "fraStep"
      Height          =   1455
      Index           =   5
      Left            =   0
      TabIndex        =   9
      Top             =   8280
      Width           =   6255
      Begin VB.CommandButton cmdFeedback 
         Caption         =   "Send Feedback"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   375
         HelpContextID   =   22
         Left            =   1800
         TabIndex        =   33
         Top             =   1080
         Width           =   2055
      End
      Begin VB.CommandButton cmdViewLog 
         Caption         =   "View Log"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   375
         HelpContextID   =   11
         Left            =   120
         TabIndex        =   28
         Top             =   1080
         Width           =   1335
      End
      Begin VB.Label lblLogPath 
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   495
         Left            =   120
         TabIndex        =   29
         Top             =   480
         Width           =   6075
      End
      Begin VB.Label lblStep 
         Caption         =   "Finished downloading data"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   255
         Index           =   5
         Left            =   120
         TabIndex        =   10
         Top             =   120
         Width           =   6135
      End
   End
   Begin MSComDlg.CommonDialog cdlg 
      Left            =   0
      Top             =   2040
      _ExtentX        =   688
      _ExtentY        =   688
      _Version        =   393216
      CancelError     =   -1  'True
   End
   Begin VB.Frame fraStep 
      BorderStyle     =   0  'None
      Caption         =   "Finding Intermediate Data"
      Height          =   1455
      Index           =   1
      Left            =   0
      TabIndex        =   6
      Top             =   2160
      Width           =   6255
      Begin VB.Label lblDownloadStatus 
         BackStyle       =   0  'Transparent
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   735
         Index           =   1
         Left            =   120
         TabIndex        =   26
         Top             =   720
         Width           =   6135
      End
      Begin VB.Label lblStep 
         Caption         =   "Finding intermediate data..."
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   495
         Index           =   1
         Left            =   120
         TabIndex        =   7
         Top             =   120
         Width           =   6135
      End
   End
   Begin VB.Frame fraStep 
      BorderStyle     =   0  'None
      Caption         =   "Frame1"
      Height          =   975
      Index           =   2
      Left            =   0
      TabIndex        =   5
      Top             =   3720
      Width           =   6255
      Begin VB.CheckBox chkCriteria 
         Height          =   252
         HelpContextID   =   11
         Index           =   0
         Left            =   3840
         TabIndex        =   20
         Top             =   600
         Visible         =   0   'False
         Width           =   220
      End
      Begin VB.TextBox txtCriteria 
         Height          =   285
         HelpContextID   =   11
         Index           =   0
         Left            =   3840
         Locked          =   -1  'True
         TabIndex        =   21
         Top             =   600
         Visible         =   0   'False
         Width           =   2292
      End
      Begin VB.Label lblStep 
         Caption         =   "Specify details about data to download (bold items are required):"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   255
         Index           =   2
         Left            =   120
         TabIndex        =   8
         Top             =   120
         Width           =   6015
      End
      Begin VB.Label lblCriteria 
         Height          =   255
         Index           =   0
         Left            =   240
         TabIndex        =   19
         Top             =   600
         Visible         =   0   'False
         Width           =   3615
      End
   End
   Begin VB.Frame fraStep 
      BorderStyle     =   0  'None
      Caption         =   "Select Download Type"
      Height          =   1335
      HelpContextID   =   11
      Index           =   0
      Left            =   0
      TabIndex        =   4
      Top             =   0
      Width           =   6255
      Begin VB.Frame fraChk 
         BorderStyle     =   0  'None
         Height          =   375
         HelpContextID   =   11
         Index           =   0
         Left            =   360
         TabIndex        =   24
         Top             =   840
         Width           =   6255
         Begin VB.CheckBox chkDataType 
            Height          =   255
            HelpContextID   =   11
            Index           =   0
            Left            =   0
            TabIndex        =   22
            Top             =   60
            Width           =   4095
         End
         Begin VB.CommandButton cmdTypeDetails 
            Caption         =   "Details"
            Height          =   315
            HelpContextID   =   11
            Index           =   0
            Left            =   4680
            TabIndex        =   23
            Top             =   0
            Visible         =   0   'False
            Width           =   855
         End
      End
      Begin VB.TextBox txtProject 
         Height          =   288
         HelpContextID   =   11
         Left            =   1680
         TabIndex        =   1
         Top             =   120
         Width           =   3615
      End
      Begin VB.CommandButton cmdProject 
         Caption         =   "Browse"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   252
         HelpContextID   =   11
         Left            =   5400
         TabIndex        =   2
         Top             =   120
         Width           =   852
      End
      Begin VB.Label lblProject 
         Caption         =   "BASINS Project:"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
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
         Width           =   1455
      End
      Begin VB.Label lblStep 
         Caption         =   "Select Data Types to Download:"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   255
         Index           =   0
         Left            =   120
         TabIndex        =   3
         Top             =   480
         Width           =   6135
      End
   End
   Begin VB.Menu mnuTop 
      Caption         =   "&File"
      HelpContextID   =   13
      Index           =   0
      Begin VB.Menu mnuLoad 
         Caption         =   "&Load Status"
         HelpContextID   =   14
         Visible         =   0   'False
      End
      Begin VB.Menu mnuSave 
         Caption         =   "&Save Status"
         HelpContextID   =   15
         Visible         =   0   'False
      End
      Begin VB.Menu mnuFileSep1 
         Caption         =   "-"
         Visible         =   0   'False
      End
      Begin VB.Menu mnuOpenCache 
         Caption         =   "&Open Cache Folder"
         Visible         =   0   'False
      End
      Begin VB.Menu mnuClearCache 
         Caption         =   "Clear &Cache"
         Visible         =   0   'False
      End
      Begin VB.Menu mnuFileSep2 
         Caption         =   "-"
         Visible         =   0   'False
      End
      Begin VB.Menu mnuExit 
         Caption         =   "E&xit"
         HelpContextID   =   13
      End
   End
   Begin VB.Menu mnuTop 
      Caption         =   "&View"
      HelpContextID   =   17
      Index           =   1
      Visible         =   0   'False
      Begin VB.Menu mnuViewStatus 
         Caption         =   "&Status"
         HelpContextID   =   18
      End
   End
   Begin VB.Menu mnuTop 
      Caption         =   "&Help"
      HelpContextID   =   19
      Index           =   2
      Begin VB.Menu mnuContents 
         Caption         =   "&Contents and Index"
         HelpContextID   =   20
      End
      Begin VB.Menu mnuHelpAbout 
         Caption         =   "&About"
         HelpContextID   =   21
      End
      Begin VB.Menu mnuHelpFeedback 
         Caption         =   "Send &Feedback"
         HelpContextID   =   22
      End
   End
End
Attribute VB_Name = "frmWizard"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit

Private Const pBaseChkWidth As Long = 375
Private Const pBaseCmdWidth As Long = 375
Private pMaxWidth As Long
Private pStep As Long
Private pManager As clsWebDataManager

Private pDataTypes As FastCollection 'of clsWebData, those selected for downloading
Private pValues() As FastCollection  'for keeping any multiple values for criteria
Private lastY As Long 'Keeping track of adding criteria to form

Private Sub chkCriteria_Click(Index As Integer)
  If chkCriteria(Index).Value = vbChecked Then
    txtCriteria(Index).Text = "True"
  Else
    txtCriteria(Index).Text = "False"
  End If
End Sub

Private Sub chkCriteria_GotFocus(Index As Integer)
  chkCriteria(Index).BackColor = vbButtonShadow
End Sub

Private Sub chkCriteria_LostFocus(Index As Integer)
  chkCriteria(Index).BackColor = vbButtonFace
End Sub

Private Sub chkDataType_Click(Index As Integer)
'  Dim RequiresPos As Integer
'  Dim RequiresStr As String
'  Dim RequiresStrPos As Integer
'  Dim SetChk As Integer
'  Dim ComponentName As String
'  Dim Desc As String
'  Dim SearchStr As String
'  Dim SkipAfterSearch As Integer
'  Dim OtherNameDirection As Integer
'
'  ComponentName = chkDataType(Index).Caption
'  Desc = cmdDetails(Index).Tag
'
'  If chkDataType(Index).Value = vbChecked Then 'Check everything this one requires
'    SearchStr = ComponentName & " requires "
'    SkipAfterSearch = Len(SearchStr)
'    OtherNameDirection = 1
'  Else 'If we un-selected this, uncheck whatever requires it, too
'    SearchStr = " requires " & ComponentName
'    SkipAfterSearch = -1
'    OtherNameDirection = -1
'  End If
'
'  RequiresPos = InStr(Desc, SearchStr)
'  While RequiresPos > 0
'    RequiresStrPos = RequiresPos + SkipAfterSearch
'    While Asc(Mid(Desc, RequiresStrPos)) > 32
'      RequiresStrPos = RequiresStrPos + OtherNameDirection
'    Wend
'    If RequiresStrPos < RequiresPos Then
'      RequiresStr = Mid(Desc, RequiresStrPos + 1, RequiresPos + SkipAfterSearch - RequiresStrPos)
'    Else
'      RequiresStr = Mid(Desc, RequiresPos + SkipAfterSearch, RequiresStrPos - (RequiresPos + SkipAfterSearch))
'    End If
'    For SetChk = chkDataType.LBound To chkDataType.UBound
'      If chkDataType(SetChk).Caption = RequiresStr Then
'        If chkDataType(SetChk).Value <> chkDataType(Index).Value Then
'          chkDataType(SetChk).Value = chkDataType(Index).Value
'        End If
'      End If
'    Next
'    RequiresPos = InStr(RequiresPos + 1, Desc, SearchStr)
'  Wend
    
End Sub

Private Sub cmdAllNone_Click(Index As Integer)
  Dim chkIndex As Long
  For chkIndex = chkDataType.LBound To chkDataType.UBound
    If Index = 0 Then
      chkDataType(chkIndex).Value = vbChecked
    Else
      chkDataType(chkIndex).Value = vbUnchecked
    End If
  Next
End Sub

Private Sub cmdCancel_Click()
  pManager.LogDbg "Cancel clicked in Wizard, skipping to Finished"
  lblStep(5) = "Cancelled"
  'pManager.State = pManager.State + 1000 'This is also done in form unload
  Step = 5
  'Unload Me
End Sub

'Private Sub cmdDetails_Click(Index As Integer)
'  LogMsg cmdDetails(Index).Tag, chkDataType(Index).Caption & " " & cmdDetails(Index).Caption
'End Sub

Private Sub cmdDetails_Click()
  pManager.ShowDownload
End Sub

Private Sub cmdFeedback_Click()
  SendFeedback
End Sub

Public Sub cmdFinish_Click()
  Dim instructionfile As String
  Dim endinstructions As String
  endinstructions = pManager.CurrentStatusGetString("endinstructions")
  If Len(endinstructions) > 0 Then
    instructionfile = FilenameNoExt(pManager.LogPath) & "instructions.txt"
    pManager.LogDbg "Saving instructions in " & instructionfile
    SaveFileString instructionfile, endinstructions & vbCrLf
    If pManager.Batch Then
      pManager.LogDbg "Skipping display of end instructions - saved to: " & instructionfile
    Else
      OpenFile instructionfile, cdlg
    End If
  End If
  pManager.State = 1000
  Unload Me
End Sub

Private Sub cmdNext_Click()
  Step = Step + 1
End Sub

Public Property Let DownloadStatus(newValue As String)
  lblDownloadStatus(1).Caption = newValue
  lblDownloadStatus(3).Caption = newValue
  lblDownloadStatus(4).Caption = newValue
  lblDownloadStatus(1).Refresh
  lblDownloadStatus(3).Refresh
  lblDownloadStatus(4).Refresh
End Property

Public Property Get Step() As Long
  Step = pStep
End Property
Public Property Let Step(newValue As Long)
  Dim nSelected As Long
  Dim i As Long
  Dim prevStep As Long
  prevStep = pStep
  If newValue < 0 Or newValue > fraStep.UBound Then
    pManager.LogDbg "Wizard can't move to step " & newValue & " (valid range is 0 to " & fraStep.UBound & ")"
  Else
    pManager.LogDbg "Wizard moving to step " & newValue & ", '" & lblStep(newValue) & "'"
    Me.DownloadStatus = ""
    'Before moving from step 0 to 1, make sure user has selected a good number of data types
    nSelected = 0
    If newValue = 1 And prevStep = 0 Then
      For i = chkDataType.LBound To chkDataType.UBound
        If chkDataType(i).Value = vbChecked Then nSelected = nSelected + 1
      Next
      
      If nSelected < 1 Then
        If pManager.LogMsg("No data was selected to download. Do you wish to download data?", _
                                   App.Title, "+&Yes", "-&No") = 2 Then 'no
          Unload Me
        End If
        Exit Property
      ElseIf (nSelected > 3) Then
        If pManager.LogMsg("Downloading multiple types of data may take a long time." & vbCr & vbCr & _
                                   nSelected & " types of data selected for download", _
                                   App.Title, "+&Ok", "-&Cancel") = 2 Then 'cancel
          Exit Property
        End If
      End If
    End If
    
    For i = 0 To fraStep.UBound
      If i = newValue Then
        fraStep(i).Visible = True
      Else
        fraStep(i).Visible = False
      End If
    Next
    pStep = newValue
    Me.Refresh
    Select Case pStep
      Case 0
        cmdAllNone(0).Visible = True
        cmdAllNone(1).Visible = True
        cmdNext.Visible = True
        cmdDetails.Visible = False
        SizeFromContents
      
      Case 1
        If prevStep <> 0 Then 'If we are backing up, skip this step
          Step = 0
        Else
          cmdAllNone(0).Visible = False
          cmdAllNone(1).Visible = False
          cmdNext.Visible = False
          cmdDetails.Visible = False
          
          Me.MousePointer = vbHourglass
          For i = chkDataType.LBound To chkDataType.UBound
            If chkDataType(i).Value = vbChecked Then
              lblStep(pStep).Caption = "Finding intermediate data for " & vbCr & chkDataType(i).Caption
              lblStep(pStep).Refresh
              pManager.DataTypeFromLabel(chkDataType(i).Caption).Specify
            End If
          Next
          lblStep(pStep).Caption = "Finding intermediate data..."
          Me.MousePointer = vbDefault
          Step = Step + 1 'Automatically move on the next step
        End If
      
      Case 2
        cmdAllNone(0).Visible = False
        cmdAllNone(1).Visible = False
        cmdNext.Visible = True
        cmdDetails.Visible = False
        SizeFromContents
        If lblCriteria.UBound = 0 Then Step = Step + 1 'move on the next step if there is nothing to specify
        
      Case 3
        cmdAllNone(0).Visible = False
        cmdAllNone(1).Visible = False
        cmdNext.Visible = False
        cmdDetails.Visible = True
        For i = 1 To txtCriteria.UBound
          pManager.LogDbg "      criteria " & lblCriteria(i).Caption & " = '" & txtCriteria(i).Text & "'"
        Next
        If GetData Then Step = Step + 1 Else Step = Step - 1
      
      Case 4
        pManager.ProjectDownloadedData
        Step = Step + 1
      Case 5
        If pManager.State < 1000 Then
          pManager.State = 999 'Finished downloading, but waiting for user
        End If
        lblLogPath.Caption = pManager.LogPath
        
        cmdAllNone(0).Visible = False
        cmdAllNone(1).Visible = False
        cmdNext.Visible = False
        cmdDetails.Visible = False
        cmdFinish.Visible = True
        cmdCancel.Visible = False
        'txtLog.Text = pManager.Logger.CurrentLog
        Form_Resize
    End Select
  End If
  If Step <= 5 Then BringFormToTop Me Else pManager.LogDbg "***Wizard Reached step " & Step
End Property

Private Sub cmdViewLog_Click()
  Dim OpenMessage As String
  OpenMessage = OpenFileQuiet(lblLogPath.Caption)
  If (OpenMessage <> lblLogPath.Caption) Then
    pManager.LogMsg "Error opening log file '" & lblLogPath.Caption & "'" & vbCr & OpenMessage, _
                           "View Log Error"
  End If
End Sub

Private Sub Form_Load()
  For pStep = 0 To fraStep.UBound
    With fraStep(pStep)
      .Visible = False
      .Left = 0
      .Top = 0
    End With
  Next
  'Step = 0 (done in loadList)
End Sub

Private Sub Form_Resize()
  Dim w As Long
  Dim h As Long
  Dim i As Long
  Dim margin As Long
  margin = 120
  w = ScaleWidth
  h = ScaleHeight
  If h > 1000 Then
    fraStep(pStep).Height = h
    fraButtons.Top = h - fraButtons.Height
    'txtLog.Height = fraButtons.Top - txtLog.Top
  End If
  If w > fraButtons.Width Then
    For i = fraStep.LBound To fraStep.UBound
      fraStep(i).Width = w
      lblStep(i).Width = w
    Next
    lblDownloadStatus(1).Width = w
    lblDownloadStatus(3).Width = w
    lblDownloadStatus(4).Width = w
    lblLogPath.Width = w
    fraButtons.Left = (w - fraButtons.Width) / 2
    
    If pStep = 0 Then 'First step is selecting types of data to download
        cmdProject.Left = w - cmdProject.Width - margin
        If cmdProject.Left - txtProject.Left - lblProject.Left > lblProject.Width Then
          txtProject.Width = cmdProject.Left - txtProject.Left - margin
        End If
        For i = 0 To fraChk.UBound
          fraChk(i).Width = w - 480
          'cmdDetails(i).Left = fraChk(i).Width '- cmdDetails(i).Width
          chkDataType(i).Width = fraChk(i).Width 'cmdDetails(i).Left
        Next
    End If

    If pStep = 2 Then 'Third step is specifying criteria (selecting sites to get data from, choosing destination, etc)
      For i = 0 To txtCriteria.Count - 1
        'cmdEdit(Index).Left = w - cmdEdit(0).Width - 120
        'txtCriteria(Index).Width = cmdEdit(Index).Left - txtCriteria(Index).Left - 120
        txtCriteria(i).Width = w - txtCriteria(i).Left - margin
        'chkCriteria(i).Width = txtCriteria(i).Width
      Next
    End If
    'txtLog.Width = w - txtLog.Left * 2
  End If
End Sub

Private Function TranslateHelpContextID(ByVal ClassName As String, ByVal id As Integer) As Integer
  'Fix HelpContextIds that were not updated until Basins31update3
  TranslateHelpContextID = id
  Select Case id
    Case 11, 12, 13:
      Select Case ClassName
        Case "clsBasins":       TranslateHelpContextID = 24
        Case "clsBasinsMet":    TranslateHelpContextID = 25
        Case "clsBasinsSWAT":   TranslateHelpContextID = 26
        Case "clsNHD":          TranslateHelpContextID = 27
        Case "clsNHDinGEO":     TranslateHelpContextID = 28
        Case "clsNLCD":         TranslateHelpContextID = 29
        Case "clsPCSDischarge": TranslateHelpContextID = 30
        Case "clsPCSFacility":  TranslateHelpContextID = 30
        Case "clsStoretStatn":  TranslateHelpContextID = 31
        Case "clsStoretVisit":  TranslateHelpContextID = 31
        Case "clsUSGSdaily":    TranslateHelpContextID = 32
        Case "clsUSGSWQ":       TranslateHelpContextID = 33
      End Select
  End Select
  Debug.Print "HelpID for " & ClassName & " = " & TranslateHelpContextID
End Function

Private Sub AddDataType(ByVal aWebData As clsWebData, _
                       ByVal key As String, _
              Optional ByVal startChecked As Boolean = False)
  Dim fraWidth As Long
  Dim i As Long
  
  i = fraChk.UBound
  If Len(chkDataType(i).Caption) > 0 Then
    i = i + 1
    Load fraChk(i)
    Load chkDataType(i): Set chkDataType(i).Container = fraChk(i)
   'Load cmdDetails(i):  Set cmdDetails(i).Container = fraChk(i)
    fraChk(i).Top = fraChk(i - 1).Top + fraChk(i - 1).Height + 50
  End If
  fraChk(i).Visible = True
  chkDataType(i).Visible = True
  
  chkDataType(i).Tag = key
  chkDataType(i).Caption = aWebData.Label
  chkDataType(i).HelpContextId = TranslateHelpContextID(aWebData.Name, aWebData.HelpId)
  fraWidth = Me.TextWidth(chkDataType(i).Caption) + pBaseChkWidth '+ cmdDetails(i).Width
  If fraWidth > pMaxWidth Then pMaxWidth = fraWidth

  If startChecked Then chkDataType(i).Value = vbChecked Else chkDataType(i).Value = vbUnchecked
  
End Sub

Public Sub AddCriteriaFor(newDataType As clsWebData)
  Dim lCategoryNode As ChilkatXml
  Dim nod As ChilkatXml
  Dim dbgStep As String
  
  On Error GoTo ErrHand
  
  dbgStep = "Log"
  pManager.LogDbg "  frmWizard: AddCriteriaFor: " & newDataType.Label
  
  dbgStep = "New FastCollection"
  If pDataTypes Is Nothing Then Set pDataTypes = New FastCollection
  
  dbgStep = "pDataTypes.Add"
  pDataTypes.Add newDataType
    
  dbgStep = "Set lCategoryNode = newDataType.Provides.FirstChild"
  Set lCategoryNode = newDataType.Provides.FirstChild
  While Not lCategoryNode Is Nothing
    dbgStep = "Select Case"
    Select Case LCase(lCategoryNode.Tag)
      Case "criteria"
        dbgStep = "Set nod = lCategoryNode.FirstChild"
        pManager.LogDbg "    DataType: " & newDataType.Name _
                   & ", HelpID = " & newDataType.HelpId
        Set nod = lCategoryNode.FirstChild
        While Not nod Is Nothing
          dbgStep = "AddNode"
          AddNode nod, newDataType.Name, newDataType.HelpId
          pManager.LogDbg "      criteria " & nod.Tag & " = '" & txtCriteria(txtCriteria.Count - 1).Text & "'"
          pManager.LogDbg "      XML: " & nod.GetXml
          dbgStep = "Next"
          If nod.NextSibling2 = 0 Then Set nod = Nothing
        Wend
    End Select
    If lCategoryNode.NextSibling2 = 0 Then Set lCategoryNode = Nothing
  Wend
  dbgStep = "Me.Height"
  If lastY + 1280 > Me.Height Then Me.Height = lastY + 1280

  dbgStep = "pManager.State = 2"
  pManager.State = 2
  Exit Sub
ErrHand:
  pManager.LogMsg Err.Description & vbCr & "at step " & dbgStep, _
                         "Wizard AddCriteriaFor"
End Sub

Private Sub AddNode(nod As ChilkatXml, DataType As String, ByVal HelpContextId As Integer)
  Dim lblIndex As Long
  Dim cboIndex As Long
  Dim grpIndex As Long
  Dim ctlTop As Single
  Dim c As FastCollection
'  Dim attrGroup As String
  Dim attrMultiple As Boolean
  Dim attrSelected As Boolean
  Dim attrFormat As String
  Dim attrLabel As String
  Dim attrType As String
  Dim attrUser As Boolean
  Dim attrOptional As Boolean
  Dim NodeValues As FastCollection
  Dim dbgStep As String
    
  On Error GoTo ErrHand
  
  HelpContextId = TranslateHelpContextID(DataType, HelpContextId)
  
  dbgStep = "frmWizard: AddNode "
  dbgStep = nod.Tag & " " & DataType
  
  On Error Resume Next
'  attrGroup = LCase(nod.getAttrValue("group"))
  attrFormat = nod.GetAttrValue("format")
  attrLabel = nod.GetAttrValue("label")
  attrType = nod.GetAttrValue("type")
  If LCase(nod.GetAttrValue("multiple")) = "true" Then attrMultiple = True
  If LCase(nod.GetAttrValue("selected")) = "true" Then attrSelected = True
  'Default user-editable to true
  If LCase(nod.GetAttrValue("user")) = "false" Then attrUser = False Else attrUser = True
  'Everything outside a group defaults to required, inside a group defaults to optional
  attrOptional = False
'  If Len(attrGroup) > 0 Then attrOptional = True
  If LCase(nod.GetAttrValue("optional")) = "true" Then attrOptional = True
  
  On Error GoTo ErrHand
  dbgStep = "attrLabel"
  If Len(attrLabel) = 0 Then attrLabel = nod.Tag
  
  If LCase(nod.Tag) = "group" Then
'    If Len(attrGroup) > 0 Then LoadGroup attrGroup, attrLabel, attrOptional
  Else
    'Don't add this one if we already have it
    dbgStep = "For lblIndex"
    For lblIndex = lblCriteria.LBound To lblCriteria.UBound
      If lblCriteria(lblIndex).Tag = nod.Tag Then Exit Sub
    Next
    dbgStep = "lblIndex"
    lblIndex = lblCriteria.Count
    ReDim Preserve pValues(lblIndex)
    dbgStep = "Load lblCriteria"
    Load lblCriteria(lblIndex)
    Load chkCriteria(lblIndex)
    Load txtCriteria(lblIndex)
    lastY = lastY + lblCriteria(lblIndex).Height * 1.4
    ctlTop = lastY

    dbgStep = "txtCriteria"
    With txtCriteria(lblIndex)
      Select Case attrFormat
        Case "openpath", "openfile", "savepath", "savefile"
            .BackColor = vbButtonFace
            .Locked = True
        Case Else
          If attrMultiple Then
            .BackColor = vbButtonFace
            .Locked = True
          Else
            .BackColor = vbWindowBackground
            .Locked = False
          End If
      End Select
      dbgStep = "getAttrValue(filter)"
      .Tag = nod.GetAttrValue("filter") 'for file types
      .Top = ctlTop
      '.ToolTipText = attrFormat
      .HelpContextId = HelpContextId
      If attrFormat = "boolean" Then
        .Visible = False
      Else
        .Visible = True
      End If
    End With
    dbgStep = "With chkCriteria"
    With chkCriteria(lblIndex)
      .Tag = attrFormat 'A handy place for it, not necessarily logical
      .HelpContextId = HelpContextId
      .Top = ctlTop
      .Left = txtCriteria(lblIndex).Left
      If attrFormat = "boolean" Then
        .Visible = True
      Else
        .Visible = False
      End If
      '.caption = lblCriteria(lblIndex).caption
      If attrSelected Then .Value = vbChecked Else .Value = vbUnchecked
    End With
    dbgStep = "With lblCriteria"
    With lblCriteria(lblIndex)
      .Top = ctlTop
      .Caption = attrLabel
      .Tag = nod.Tag
      .Visible = True
      .FontBold = Not attrOptional
      '.ToolTipText = DataType
    End With
    
    Set NodeValues = FindNodeValues(nod.Tag)

    If NodeValues.Count > 0 Then 'have a current value
      dbgStep = "Set pValues(lblIndex)"
      Set pValues(lblIndex) = NodeValues
      txtCriteria(lblIndex).Text = NodeValues.ItemByIndex(1).content
      If NodeValues.Count > 1 Then
        txtCriteria(lblIndex).Text = txtCriteria(lblIndex).Text & " and " & NodeValues.Count - 1 & " more"
      End If
      dbgStep = "If chkCriteria(lblIndex).Visible"
      If chkCriteria(lblIndex).Visible Then
        If LCase(txtCriteria(lblIndex).Text) = "true" Then
          chkCriteria(lblIndex).Value = vbChecked
        Else
          chkCriteria(lblIndex).Value = vbUnchecked
        End If
      End If
    End If

'    Set c = pManager.Provides(nod.Tag)
'    For cboIndex = 1 To c.Count
'      If c(cboIndex) <> DataType Then
'        cbo(lblIndex).AddItem c(cboIndex)
'      End If
'    Next
'    Automatically get value from BASINS project if possible
'    If Len(txtCriteria(lblIndex).Text) = 0 And cbo(lblIndex).Text = "BASINS Project" Then
'      'GetValueFromProject lblIndex
'      If Len(pManager.CurrentStatusGetString("project_dir")) > 0 Then
'        ShowCriteria lblIndex, False
'      End If
'    End If
  End If
  dbgStep = "fraButtons.Top"
  fraButtons.Top = lastY + 300
  Exit Sub
ErrHand:
  pManager.LogMsg Err.Description & vbCr & "at step " & dbgStep, _
                         "Wizard AddNode"
End Sub

Private Function FindNodeValues(aTag As String) As FastCollection
  Dim lQuery As ChilkatXml
  Dim lResult As ChilkatXml
  Dim lStatusVar
  Dim lStatus As Boolean
  Dim lProvider As clsWebData
  Dim providers As FastCollection
  Dim providerIndex As Long
  Dim lMatchNodeList As FastCollection
  Dim dbgStep As String
  
  On Error GoTo ErrHand
  
  dbgStep = "Set lMatchNodeList"
  Set lMatchNodeList = pManager.CurrentStatusGetList(aTag)
  dbgStep = "If lMatchNodeList.Count = 0"
  If lMatchNodeList.Count = 0 Then 'no current value, look for one
    dbgStep = "Set providers"
    Set providers = pManager.Provides(aTag)
    dbgStep = "For providerIndex"
    For providerIndex = 1 To providers.Count
      Debug.Print providers.ItemByIndex(providerIndex) & " could provide " & aTag
      dbgStep = "Set lProvider"
      Set lProvider = pManager.DataTypeFromLabel(providers.ItemByIndex(providerIndex))
      dbgStep = "CurrentStatusUpdateString"
      pManager.CurrentStatusUpdateString aTag, "", "requested from " & lProvider.Name
      dbgStep = "BuildQueryFromStatus"
      Set lQuery = BuildQueryFromStatus(lProvider)
      If lQuery Is Nothing Then GoTo NextProvider 'Not all required criteria specified
      dbgStep = "Set lResult"
      Set lResult = New ChilkatXml
      
      dbgStep = "lStatus = lProvider.GetData (" & lProvider.Name & ".GetData" & vbCrLf & vbCrLf & lQuery.GetXml & vbCrLf & "---" & vbCrLf & lResult.GetXml
      lStatus = lProvider.GetData(lQuery, lResult)
      dbgStep = "If lStatus Then (" & lStatus & ")"
      If lStatus Then
        dbgStep = "CurrentStatusUpdateList"
        pManager.CurrentStatusUpdateList aTag, _
                                         GetChildrenWithTag(lResult, aTag), _
                                         "set by " & providers(providerIndex)
        dbgStep = "Set FindNodeValues"
        Set FindNodeValues = GetChildrenWithTag(pManager.CurrentStatus.GetChildWithTag("status_variables"), aTag)
        dbgStep = "FindNodeValues exit "
        Exit Function
      End If
NextProvider:
    Next
    Set lMatchNodeList = GetChildrenWithTag(pManager.CurrentStatus, aTag)
  End If
  dbgStep = "Set FindNodeValues"
  Set FindNodeValues = lMatchNodeList
  dbgStep = "FindNodeValues end "
  Exit Function
ErrHand:
  pManager.LogMsg Err.Description & vbCr & " at step " & dbgStep, _
                         "Wizard FindNodeValues " & aTag
End Function

Private Function BuildQueryFromStatus(DataType As clsWebData) As ChilkatXml
  Dim lCategory As ChilkatXml
  Dim lCatItem As ChilkatXml
  Dim critName As String
  Dim lQuery As ChilkatXml
  Dim Requested As ChilkatXml
  Dim lVariable As ChilkatXml
  Dim lStatus As String
  Dim s As String
  Dim s2 As String
  Dim i As Long
  Dim missing As String
  Dim found As Boolean
  Dim NodeValues As FastCollection
  Dim dbgStep As String
  
  On Error GoTo ErrHand
  
  dbgStep = "Set Requested = DataType.Provides"
  Set Requested = DataType.Provides
  
  dbgStep = "DataType.Name"
  s = "<" & DataType.Name & "><criteria>"
  
  dbgStep = "Set lCategory = Requested.FirstChild"
  Set lCategory = Requested.FirstChild
  While Not lCategory Is Nothing
    dbgStep = "Select Case LCase(lCategory.Tag)"
    dbgStep = "Select Case LCase(" & lCategory.Tag & ")"
    Select Case LCase(lCategory.Tag)
      Case "criteria"
        dbgStep = "Set lCatItem = lCategory.FirstChild"
        Set lCatItem = lCategory.FirstChild
        While Not lCatItem Is Nothing
          dbgStep = "lCatItem.Tag"
          critName = lCatItem.Tag
          If critName <> "group" Then
            dbgStep = "FindNodeValues(" & critName & ")"
            Set NodeValues = FindNodeValues(critName)
            
            If NodeValues.Count > 0 Then 'have a current value
              dbgStep = "NodeValues.Count > 0"
              For i = 1 To NodeValues.Count
                s = s & "<" & critName & ">" & NodeValues.ItemByIndex(i).content & "</" & critName & ">"
              Next
            Else
              dbgStep = "NodeValues.Length <= 0"
              dbgStep = dbgStep & " DataType.Name = " & DataType.Name
              dbgStep = dbgStep & " DataType.HelpId = " & DataType.HelpId
              If Len(lCatItem.GetAttrValue("optional")) = 0 Then
                missing = missing & vbCr & "   " & critName
                dbgStep = "NodeValues.Length <= 0, optional Is Nothing"
                dbgStep = dbgStep & vbCr & " DataType.Name = " & DataType.Name
                dbgStep = dbgStep & vbCr & " DataType.HelpId = " & DataType.HelpId
                dbgStep = dbgStep & vbCr & " lCatItem.getxml = " & lCatItem.GetXml
                AddNode lCatItem, DataType.Name, DataType.HelpId
              ElseIf LCase(lCatItem.GetAttrValue("optional")) <> "true" Then
                missing = missing & vbCr & "   " & critName
                dbgStep = "NodeValues.Length <= 0, optional <> true, AddNode"
                AddNode lCatItem, DataType.Name, DataType.HelpId
              End If
            End If
          End If
          If lCatItem.NextSibling2 = 0 Then Set lCatItem = Nothing
        Wend
    End Select
    If lCategory.NextSibling2 = 0 Then Set lCategory = Nothing
  Wend
    
  If Len(missing) > 0 Then
    pManager.LogMsg "Required Criteria Not Set:" & vbCr & missing & vbCr & "Cannot get data", _
                           "Wizard BuildQueryFromStatus"
  Else
    s = s & "</criteria><requested>"
    dbgStep = "With pManager.CurrentStatus.getElementsByTagName"
    Set lVariable = pManager.CurrentStatus.GetChildWithTag("status_variables")
    lVariable.FirstChild2
    While Not lVariable Is Nothing
      lStatus = lVariable.GetAttrValue("status")
      If InStr(lStatus, DataType.Name) And Left(lStatus, 4) = "requ" Then
        s2 = Requested.SearchForTag(Nothing, lVariable.Tag).GetParent.Tag
        s = s & "<" & s2 & "><" & lVariable.Tag & "/></" & s2 & ">"
      End If
      If lVariable.NextSibling2 = 0 Then Set lVariable = Nothing
    Wend
    s = s & "</requested></" & DataType.Name & ">"
    dbgStep = "Set lQuery = New ChilkatXml"
    Set lQuery = New ChilkatXml
    dbgStep = "lQuery.loadXML s"
    lQuery.LoadXml s
    dbgStep = "Set BuildQueryFromStatus = lQuery"
    Set BuildQueryFromStatus = lQuery
  End If
  Exit Function
ErrHand:
  pManager.LogMsg Err.Description & vbCr & " at step " & dbgStep & vbCr & "missing = " & missing, _
                         "Wizard BuildQueryFromStatus"
End Function

Private Sub SizeFromContents()
  Dim MinWidth As Long
  Dim newWidth As Long
  Dim newHeight As Long
  
  'Find the best available width for window. (Width - ScaleWidth) = width of border
  newWidth = pMaxWidth + 480 + Width - ScaleWidth
  'MinWidth = fraButtons.Width + 300 + Width - ScaleWidth
  MinWidth = Width
  Select Case pStep
    Case 0
      'Find best available height for window. (Height - ScaleHeight) is the height of the title bar and border
      With fraChk(fraChk.UBound)
        newHeight = .Top + .Height + fraButtons.Height + Height - ScaleHeight
      End With
    Case 2
      With txtCriteria(txtCriteria.UBound)
        newHeight = .Top + .Height + fraButtons.Height + Height - ScaleHeight
      End With
    Case Else
      newHeight = Height
      newWidth = Width
  End Select
  
  If newWidth < MinWidth Then newWidth = MinWidth
  If newWidth > Screen.Width * 0.9 Then newWidth = Screen.Width * 0.9
  
  If newHeight < Me.Height Then newHeight = Me.Height
  If newHeight > Screen.Height * 0.9 Then newHeight = Screen.Height * 0.9
  If newHeight <> Me.Height Or newWidth <> Me.Width Then
    If Me.WindowState = vbNormal Then
      Me.Move Me.Left, Me.Top, newWidth, newHeight
    End If
  Else
    Form_Resize
  End If
End Sub

Private Sub Form_Unload(Cancel As Integer)
  'Move manager into finished or cancelled state
  Select Case pManager.State
    Case 999: pManager.State = 1000 'We were finished, just waiting while user views log
    Case 0:   pManager.State = 1001 'We were not finished yet, so cancel from current state
    Case Is < 999: pManager.State = pManager.State + 1000
  End Select
  'If pManager.State < 1000 Then pManager.State = pManager.State + 1000
End Sub

Private Sub mnuClearCache_Click()
'  Me.MousePointer = vbHourglass
'  Kill GetTmpPath & "WebDataCache_*"
'  Me.MousePointer = vbDefault

'  Dim Filename As String
'  Filename = Dir(GetTmpPath & "WebDataCache_*")
'  While Len(Filename) > 0
'    Kill Filename
'    Filename = Dir
'  Wend
End Sub

Private Sub mnuOpenCache_Click()
  OpenFile gCacheDir
End Sub

Private Sub txtCriteria_Click(Index As Integer)
  If txtCriteria(Index).Locked Then Edit Index
End Sub

Private Sub txtCriteria_KeyPress(Index As Integer, KeyAscii As Integer)
  If txtCriteria(Index).Locked And KeyAscii <> 9 Then Edit Index
End Sub

Private Sub txtCriteria_LostFocus(Index As Integer)
  With txtCriteria(Index)
    If Not .Locked Then
      If Right(.Tag, 4) = "_dir" Then
        If Len(.Text) > 0 Then
          If Right(.Text, 1) <> "\" Then .Text = .Text & "\"
        End If
      End If
      pManager.CurrentStatusUpdateString lblCriteria(Index).Tag, .Text, "set by " & lblCriteria(Index).ToolTipText
      Set pValues(Index) = Nothing
    End If
  End With
End Sub

Private Sub Edit(Index As Integer) ', Optional TryJustOne As Boolean = False)
  Dim lStatusSet As Boolean
  Dim lvalue As Variant ', i As Long
  Dim Filename As String
  Dim Label As String
  
  On Error GoTo NeverMind 'Probably a result of user clicking Cancel
  
  Filename = txtCriteria(Index).Text
  If Len(Filename) = 0 Then
    Filename = pManager.CurrentStatusGetString("project_dir")
    If Len(Filename) = 0 Then
      Filename = pManager.CurrentStatusGetString("save_dir", CurDir)
    End If
    Filename = Filename & lblCriteria(Index).Tag '"Untitled"
  End If
  
  Label = lblCriteria(Index).Caption
  pManager.LogDbg "    Edit:" & Label & " " & chkCriteria(Index).Tag
  lStatusSet = False
  With txtCriteria(Index)
    Select Case chkCriteria(Index).Tag
      Case "openpath"
        Filename = BrowseFolder(Me, "Choose a '" & Label & "' folder", Filename, False)
        If Len(Filename) > 0 Then txtCriteria(Index).Text = Filename: Set pValues(Index) = Nothing
      Case "savepath"
        Filename = BrowseFolder(Me, "Choose a '" & Label & "' folder", Filename, True)
        If Len(Filename) > 0 Then txtCriteria(Index).Text = Filename: Set pValues(Index) = Nothing
      Case "openfile"
        cdlg.DialogTitle = "Open " & Label
        cdlg.Filename = Filename
        If Len(txtCriteria(Index).Tag) = 0 Then
          cdlg.Filter = "All files|*.*"
        Else
          cdlg.Filter = txtCriteria(Index).Tag
        End If
        cdlg.ShowOpen
        txtCriteria(Index).Text = cdlg.Filename
        Set pValues(Index) = Nothing
      Case "savefile"
        cdlg.DialogTitle = "Save " & Label
        cdlg.Filename = Filename
        If Len(txtCriteria(Index).Tag) = 0 Then
          cdlg.Filter = "All files|*.*"
        Else
          cdlg.Filter = txtCriteria(Index).Tag
        End If
        cdlg.ShowSave
        txtCriteria(Index).Text = cdlg.Filename
        Set pValues(Index) = Nothing
        If LCase(FileExt(txtCriteria(Index).Text)) = "shp" Then
          If Len(FilenameOnly(txtCriteria(Index).Text)) > 7 Then
            pManager.LogMsg "Warning: Base name of shape file exceeds seven characters." & vbCr _
                 & "You may need to shorten it for use by some programs." & vbCr & vbCr & _
                 "Shape file '" & FilenameOnly(txtCriteria(Index).Text) & "' " & Len(FilenameOnly(txtCriteria(Index).Text)) & " characters long", _
                 "Wizard Edit"
          End If
        End If
      Case Else 'allow editing multiple values
        If pValues(Index) Is Nothing Then
          txtCriteria(Index).BackColor = vbWindowBackground
          txtCriteria(Index).Locked = False
          txtCriteria(Index).SetFocus
        ElseIf pValues(Index).Count < 2 Then
          txtCriteria(Index).BackColor = vbWindowBackground
          txtCriteria(Index).Locked = False
          txtCriteria(Index).SetFocus
        Else
        'If Right(cmdEdit(Index).caption, 3) = "..." And (Not pValues(Index) Is Nothing Or Not TryJustOne) Then
          Me.MousePointer = vbHourglass
          frmMultiple.Tag = lblCriteria(Index).Tag
          Set frmMultiple.coll = pValues(Index)
          frmMultiple.Caption = Label
          frmMultiple.Show vbModal
          If Not frmMultiple.coll Is Nothing Then
            Set pValues(Index) = Nothing
            Set pValues(Index) = frmMultiple.coll
            Select Case pValues(Index).Count
              Case 0:    txtCriteria(Index).Text = ""
              Case 1:    txtCriteria(Index).Text = pValues(Index).ItemByIndex(1).content
              Case Else: txtCriteria(Index).Text = pValues(Index).ItemByIndex(1).content _
                                         & " and " & pValues(Index).Count - 1 & " more"
                'i = 0
                'For Each lvalue In pValues(index)
                pManager.CurrentStatusUpdateList lblCriteria(Index).Tag, pValues(Index), "set by user"
                '  i = 1
                'Next lvalue
                lStatusSet = True
            End Select
          End If
          Unload frmMultiple
          Me.MousePointer = vbDefault
        End If
    End Select
  End With
  If Not (lStatusSet) Then
    pManager.CurrentStatusUpdateString lblCriteria(Index).Tag, txtCriteria(Index), "set by user"
  End If
NeverMind:
End Sub

Private Sub txtProject_Change()
  If pManager.CurrentStatusGetString("project_dir") <> txtProject.Text Then
    pManager.CurrentStatusUpdateString "project_dir", txtProject.Text, "set from SelectDataType"
  End If
End Sub

Private Sub txtProject_LostFocus()
  If Len(txtProject.Text) = 8 Then
    If IsNumeric(txtProject.Text) Then
      pManager.CurrentStatusUpdateString "huc_cd", txtProject.Text, "set from SelectDataType"
    End If
  End If
End Sub

Public Property Set Manager(newValue As clsWebDataManager)
  Set pManager = newValue
End Property

Public Sub LoadList()
  Dim lWebType As clsWebData
  Dim WebTypeIndex As Integer
  Dim itemIndex As Integer
  Dim childIndex As Integer
  Dim collOutput As FastCollection
  Dim collFile As FastCollection
  
  txtProject.Text = pManager.CurrentStatusGetString("project_dir")
  If Len(txtProject.Text) = 9 Then
    If IsNumeric(Left(txtProject.Text, 8)) Then
      txtProject.Text = Left(txtProject.Text, 8)
    End If
  End If
  
  For WebTypeIndex = fraChk.UBound To 1 Step -1
    Unload chkDataType(WebTypeIndex)
    Unload fraChk(WebTypeIndex)
  Next
  chkDataType(0).Caption = ""
  
  For WebTypeIndex = 1 To pManager.DataTypes.Count
    Set lWebType = pManager.DataTypes(WebTypeIndex)
    Set collOutput = GetChildrenWithTag(lWebType.Provides, "output")
    For itemIndex = 1 To collOutput.Count
      Set collFile = GetChildrenWithTag(collOutput.ItemByIndex(itemIndex), "file")
      If collFile.Count > 0 Then AddDataType lWebType, (WebTypeIndex), False
      Set collFile = Nothing
    Next
    Set collOutput = Nothing
  Next WebTypeIndex
  
  Step = 0
  SizeFromContents
  Me.Show
End Sub

Private Sub mnuContents_Click()
  Dim Filename As String
  Filename = OpenFile(App.HelpFile, Me.cdlg)
  If FileExists(Filename) Then
    If Filename <> App.HelpFile Then
      App.HelpFile = Filename
      SaveSetting cAppName, "files", "WebDataManager.chm", Filename
    End If
  End If
End Sub

Private Sub mnuExit_Click()
  Unload Me
End Sub

Private Sub mnuHelpFeedback_Click()
  SendFeedback
End Sub

Private Sub SendFeedback()
  Dim stepname As String
  On Error GoTo errmsg
                                       stepname = "1: Dim feedback As clsATCoFeedback"
  Dim feedback As clsATCoFeedback
                                       stepname = "2: Set feedback = New clsATCoFeedback"
  Set feedback = New clsATCoFeedback
                                       stepname = "3: feedback.AddText"
  feedback.AddText AboutString(False)
                                       stepname = "4: feedback.AddFile"
  feedback.AddFile Left(App.path, InStr(4, App.path, "\")) & "unins000.dat"
                                       stepname = "5: pManager.LogPath"
  If FileExists(pManager.LogPath) Then
                                       stepname = "6: WholeFileString"
    feedback.AddText WholeFileString(pManager.LogPath)
  End If
                                       stepname = "7: feedback.Show"
  feedback.Show App, Me.Icon
  
  Exit Sub
  
errmsg:
  pManager.LogMsg "Error opening feedback in step " & stepname & vbCr & Err.Description, _
                         "Wizard Feedback"
End Sub

Private Sub mnuHelpAbout_Click()
  frmAbout.ShowVersions AboutString(True)
End Sub

Private Sub mnuLoad_Click()
  StatusLoad
End Sub

Private Sub mnuSave_Click()
  StatusSave
End Sub

Private Sub StatusSave()
  Dim s As String
  s = GetSetting(cAppName, "files", "StatusFile", "WebDataManagerStatus.xml")
    
  On Error GoTo ErrHand
  With cdlg
    .DialogTitle = "Save Status As..."
    .DefaultExt = ".xml"
    .Filter = "XML Files (*.xml)|*.xml|All Files|*.*"
    .FilterIndex = 1
    .Filename = s
    .ShowSave
    s = .Filename
  End With
  SaveSetting cAppName, "files", "StatusFile", s
  SaveFileString s, pManager.CurrentStatus.GetXml
  
  Exit Sub

ErrHand:
  Select Case Err.Number
    Case 0, 32755: 'Ignore no error or user cancelled file dialog
    Case Else: pManager.LogMsg "Error " & Err.Description & vbCr & "Saving Status File", _
                                      "Wizard StatusSave"
  End Select
'  Dim lCur As String
'
'  'dont hard code the names!
'  lCur = CurDir
'  ChDrive "C:"
'  ChDir "\vbexperimental\datafinder\data"
'  SaveFileString "DataFinderStatus.txt", pManager.CurrentStatus.getxml
'  ChDrive Left(lCur, 2)
'  ChDir Right(lCur, Len(lCur) - 2)
End Sub

Private Sub StatusLoad()
  Dim s As String
  s = GetSetting(cAppName, "files", "StatusFile", "DataFinderStatus.xml")

  On Error GoTo ErrHand
  With cdlg
    .DialogTitle = "Load Status"
    .DefaultExt = ".xml"
    .Filter = "XML Files (*.xml)|*.xml|All Files|*.*"
    .FilterIndex = 1
    .Filename = s
    .ShowOpen
    s = .Filename
  End With
  SaveSetting cAppName, "files", "StatusFile", s
  pManager.CurrentStatusFromFile s
  
  Exit Sub

ErrHand:
  Select Case Err.Number
    Case 0, 32755: 'Ignore no error or user cancelled file dialog
    Case Else: pManager.LogMsg "Error " & Err.Description & vbCr & "Loading Status File", _
                                      "Wizard StatusLoad"
  End Select

End Sub

'Private Sub mnuViewStatus_Click()
'  pManager.ShowTree pManager.CurrentStatus, "Current Status"
'End Sub

Private Sub cmdProject_Click()
  Dim Filename As String
  On Error GoTo NeverMind
  Filename = BrowseFolder(Me, "Choose a BASINS Project folder", txtProject.Text)
  If Len(Filename) > 0 Then txtProject.Text = Filename
NeverMind:
End Sub

Function AboutString(Optional AboutFlag As Boolean = True) As String
  Dim vTserFile As Variant
  Dim s$, i&
  
  If AboutFlag Then
    s = App.Title & " " & App.Major & "." & App.Minor & "." & App.Revision & vbCrLf _
    's = s & "FOR TESTING AND EVALUATION USE ONLY!" & vbCrLf
    s = s & "-----------" & vbCrLf
    s = s & "Inquiries about this software should be directed to" & vbCrLf
    s = s & "the organization which supplied you this software." & vbCrLf
    s = s & "-----------" & vbCrLf
    i = 0
  Else
    i = 2
  End If
  s = s & Space(i) & "Current Directory: " & CurDir & vbCrLf
  s = s & Space(i) & "Cache " & GetTmpPath & "WebDataCache_*" & vbCrLf & vbCrLf
  s = s & Space(i) & "Manager " & pManager.Version(i) & vbCrLf
  AboutString = s
End Function

Private Sub BuildQuery(ByRef aQuery As ChilkatXml, _
                       ByRef StatusVarsToGet As FastCollection, _
                       ByRef DataType As clsWebData)
  Dim Requested As ChilkatXml
  Dim s As String, s2 As String, n As String, i As Long, Val As Variant, fraIndex As Long
  Dim missing As String
  Dim found As Boolean
  Dim lVariable As ChilkatXml
  Dim lVarStatus As String
  
  Set StatusVarsToGet = New FastCollection
  Set Requested = DataType.Provides
  
  s = "<" & DataType.Name & "><criteria>"
  For i = 1 To lblCriteria.Count - 1
    If lblCriteria(i).Visible Then
      If Len(txtCriteria(i).Text) > 0 Then
        If pValues(i) Is Nothing Then
          s2 = "<" & lblCriteria(i).Tag & ">" & txtCriteria(i).Text & "</" & lblCriteria(i).Tag & ">"
        Else
          If pValues(i).Count > 0 Then
            's2 = ""
            Dim TempFileName As String
            TempFileName = GetTmpFileName
            For Each Val In pValues(i)
              's2 = s2 & Val.GetXml
              AppendFileString TempFileName, Val.GetXml
            Next
            s2 = WholeFileString(TempFileName)
            Kill TempFileName
          End If
        End If
        s = s & s2
'        If opt(i).Value Then SaveSetting cAppName, DataType.Name, fra(CInt(opt(i).Tag)).Tag, s2

      ElseIf lblCriteria(i).FontBold = True Then
        missing = missing & vbCr & "   " & lblCriteria(i).Caption
      End If
    End If
  Next
    
  If Len(missing) > 0 Then
    pManager.LogMsg "Required Criteria Not Set:" & vbCr & missing & vbCr & "Cannot get data", _
                           "Wizard BuildQuery"
  Else
    s = s & "</criteria><requested>"
    Set lVariable = pManager.CurrentStatus.GetChildWithTag("status_variables")
    Set lVariable = lVariable.FirstChild
    While Not lVariable Is Nothing
      If InStr(lVariable.Tag, DataType.Name) Then
        lVarStatus = lVariable.GetAttrValue("status")
        If LCase(Left(lVarStatus, 4)) = "requ" Then
          StatusVarsToGet.Add lVariable.Tag
          s2 = Requested.SearchForTag(Nothing, lVariable.Tag).GetParent.Tag
          s = s & "<" & s2 & "><" & lVariable.Tag & "/></" & s2 & ">"
        End If
      End If
      If lVariable.NextSibling2 = 0 Then Set lVariable = Nothing
    Wend
    s = s & "</requested></" & DataType.Name & ">"
    Set aQuery = New ChilkatXml
    aQuery.LoadXml s
  End If
End Sub

'Returns False if not enough criteria were specified to try retrieval
Private Function GetData() As Boolean
  Dim lQuery As ChilkatXml
  Dim lResult As ChilkatXml
  Dim lStatus As Boolean
  Dim vStatusVar As Variant
  Dim lStatusVarsToGet As FastCollection
  Dim vDataType As Variant
  Dim lDataType As clsWebData
  Dim dataIndex As Long
  
  GetData = False 'Default to unsuccessful in case we have to Exit
  Me.MousePointer = vbHourglass
  
  If pDataTypes Is Nothing Then
    pManager.LogDbg "Wizard GetData has no data types to download"
    Step = Step + 1
  Else
    'Now build the queries and download the data
    For Each vDataType In pDataTypes
      If pManager.State >= 999 Then Exit Function
      Set lDataType = vDataType
      BuildQuery lQuery, lStatusVarsToGet, lDataType
      If lQuery Is Nothing Then     'Not all required criteria was specified
        Me.MousePointer = vbDefault 'Should be noticed on first vDataType, so this error
        Exit Function               'will happen before we download any data
      End If
      dataIndex = dataIndex + 1
      lblStep(3).Caption = "Downloading " & lDataType.Label
      If pDataTypes.Count > 1 Then
        lblStep(3).Caption = lblStep(3).Caption & vbCr _
                           & "(data type " & dataIndex & " of " & pDataTypes.Count & ")"
      End If
      lblStep(3).Refresh
      Set lResult = New ChilkatXml
      lStatus = lDataType.GetData(lQuery, lResult)
      If lStatus Then 'update Status variables
        For Each vStatusVar In lStatusVarsToGet
          pManager.CurrentStatusUpdateList (vStatusVar), _
               GetChildrenWithTag(lResult, (vStatusVar)), "set by " & lDataType.Name
        Next
      End If
    Next
  End If
  GetData = True
  Me.MousePointer = vbDefault
End Function
