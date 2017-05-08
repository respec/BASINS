VERSION 5.00
Begin VB.Form frmGenScnSimplify 
   BorderStyle     =   3  'Fixed Dialog
   Caption         =   "GenScn Scenario New Simplify Network"
   ClientHeight    =   6540
   ClientLeft      =   1236
   ClientTop       =   1596
   ClientWidth     =   7584
   BeginProperty Font 
      Name            =   "MS Sans Serif"
      Size            =   7.8
      Charset         =   0
      Weight          =   700
      Underline       =   0   'False
      Italic          =   0   'False
      Strikethrough   =   0   'False
   EndProperty
   HelpContextID   =   69
   Icon            =   "GenSimp.frx":0000
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   PaletteMode     =   1  'UseZOrder
   ScaleHeight     =   6540
   ScaleWidth      =   7584
   Begin VB.CommandButton cmdClear 
      Caption         =   "C&lear Specs"
      Height          =   372
      Left            =   4800
      TabIndex        =   17
      Top             =   5400
      Width           =   1332
   End
   Begin VB.CommandButton cmdGet 
      Caption         =   "&Get Specs"
      Height          =   372
      Left            =   3120
      TabIndex        =   16
      Top             =   5400
      Width           =   1332
   End
   Begin VB.CommandButton cmdSave 
      Caption         =   "&Save Specs"
      Height          =   372
      Left            =   1440
      TabIndex        =   15
      Top             =   5400
      Width           =   1332
   End
   Begin VB.CommandButton cmdFile 
      Caption         =   "Select"
      Height          =   252
      Index           =   6
      Left            =   360
      TabIndex        =   14
      Top             =   4800
      Width           =   852
   End
   Begin VB.CommandButton cmdFile 
      Caption         =   "Select"
      Height          =   252
      Index           =   5
      Left            =   360
      TabIndex        =   13
      Top             =   4440
      Width           =   852
   End
   Begin VB.CommandButton cmdFile 
      Caption         =   "Select"
      Height          =   252
      Index           =   3
      Left            =   360
      TabIndex        =   10
      Top             =   3240
      Width           =   852
   End
   Begin VB.TextBox txtSmall 
      Height          =   288
      Left            =   4920
      TabIndex        =   5
      Top             =   1320
      Width           =   852
   End
   Begin VB.TextBox txtLarge 
      Height          =   288
      Left            =   4920
      TabIndex        =   3
      Top             =   960
      Width           =   852
   End
   Begin VB.TextBox txtRank 
      Height          =   288
      Left            =   4920
      TabIndex        =   1
      Top             =   600
      WhatsThisHelpID =   25
      Width           =   852
   End
   Begin VB.CommandButton cmdFile 
      Caption         =   "Select"
      Height          =   252
      Index           =   4
      Left            =   360
      TabIndex        =   12
      Top             =   4080
      Width           =   852
   End
   Begin VB.CommandButton cmdFile 
      Caption         =   "Select"
      Height          =   252
      Index           =   2
      Left            =   360
      TabIndex        =   9
      Top             =   2880
      Width           =   852
   End
   Begin VB.CommandButton cmdFile 
      Caption         =   "Select"
      Height          =   252
      Index           =   1
      Left            =   360
      TabIndex        =   8
      Top             =   2520
      Width           =   852
   End
   Begin VB.CommandButton cmdFile 
      Caption         =   "Select"
      Height          =   252
      Index           =   0
      Left            =   360
      TabIndex        =   7
      Top             =   2160
      Width           =   852
   End
   Begin VB.CommandButton cmdNew 
      Cancel          =   -1  'True
      Caption         =   "&Cancel"
      Height          =   375
      Index           =   2
      Left            =   3960
      TabIndex        =   19
      Top             =   6000
      Width           =   972
   End
   Begin VB.CommandButton cmdNew 
      Caption         =   "&OK"
      Height          =   375
      Index           =   1
      Left            =   2640
      TabIndex        =   18
      Top             =   6000
      Width           =   972
   End
   Begin MSComDlg.CommonDialog CDGet 
      Left            =   3000
      Top             =   5880
      _ExtentX        =   699
      _ExtentY        =   699
      _Version        =   393216
      FontSize        =   4.16602e-38
   End
   Begin MSComDlg.CommonDialog CDSave 
      Left            =   1200
      Top             =   5880
      _ExtentX        =   699
      _ExtentY        =   699
      _Version        =   393216
      FontSize        =   4.16602e-38
   End
   Begin MSComDlg.CommonDialog CDFile 
      Index           =   6
      Left            =   0
      Top             =   4680
      _ExtentX        =   699
      _ExtentY        =   699
      _Version        =   393216
      FontSize        =   4.16602e-38
   End
   Begin MSComDlg.CommonDialog CDFile 
      Index           =   5
      Left            =   0
      Top             =   4320
      _ExtentX        =   699
      _ExtentY        =   699
      _Version        =   393216
      FontSize        =   4.16602e-38
   End
   Begin MSComDlg.CommonDialog CDFile 
      Index           =   3
      Left            =   0
      Top             =   3120
      _ExtentX        =   699
      _ExtentY        =   699
      _Version        =   393216
      FontSize        =   4.16602e-38
   End
   Begin MSComDlg.CommonDialog CDFile 
      Index           =   4
      Left            =   0
      Top             =   3960
      _ExtentX        =   699
      _ExtentY        =   699
      _Version        =   393216
      FontSize        =   4.16602e-38
   End
   Begin MSComDlg.CommonDialog CDFile 
      Index           =   2
      Left            =   0
      Top             =   2760
      _ExtentX        =   699
      _ExtentY        =   699
      _Version        =   393216
      FontSize        =   4.16602e-38
   End
   Begin MSComDlg.CommonDialog CDFile 
      Index           =   1
      Left            =   0
      Top             =   2400
      _ExtentX        =   699
      _ExtentY        =   699
      _Version        =   393216
      FontSize        =   4.16602e-38
   End
   Begin MSComDlg.CommonDialog CDFile 
      Index           =   0
      Left            =   0
      Top             =   2040
      _ExtentX        =   699
      _ExtentY        =   699
      _Version        =   393216
      FontSize        =   4.16602e-38
   End
   Begin VB.Label lblOutput 
      Caption         =   "O&utput Files"
      Height          =   252
      Left            =   240
      TabIndex        =   11
      Top             =   3720
      Width           =   1092
   End
   Begin VB.Label lblFile 
      Caption         =   "<none>"
      Height          =   252
      Index           =   6
      Left            =   3360
      TabIndex        =   34
      Top             =   4800
      Width           =   3972
   End
   Begin VB.Label lblFile 
      Caption         =   "<none>"
      Height          =   252
      Index           =   5
      Left            =   3360
      TabIndex        =   33
      Top             =   4440
      Width           =   3972
   End
   Begin VB.Label lblFile 
      Caption         =   "<none>"
      Height          =   252
      Index           =   3
      Left            =   3360
      TabIndex        =   32
      Top             =   3240
      Width           =   3972
   End
   Begin VB.Label lblAreaout 
      Caption         =   "Areas"
      Height          =   252
      Left            =   1320
      TabIndex        =   31
      Top             =   4800
      Width           =   1932
   End
   Begin VB.Label lblConnout 
      Caption         =   "Connections"
      Height          =   252
      Left            =   1320
      TabIndex        =   30
      Top             =   4440
      Width           =   1932
   End
   Begin VB.Label lblReachout 
      Caption         =   "Reaches"
      Height          =   252
      Left            =   1320
      TabIndex        =   29
      Top             =   4080
      Width           =   1932
   End
   Begin VB.Label lblCriteria 
      Caption         =   "Criteria"
      Height          =   252
      Left            =   240
      TabIndex        =   28
      Top             =   240
      Width           =   1092
   End
   Begin VB.Label lblSmall 
      Caption         =   "Fraction of Basin at which to &Combine Segments:"
      Height          =   255
      Left            =   360
      TabIndex        =   4
      Top             =   1320
      Width           =   4455
   End
   Begin VB.Label lblLarge 
      Caption         =   "Fraction of Basin at which to &Break Segment:"
      Height          =   255
      Left            =   360
      TabIndex        =   2
      Top             =   960
      Width           =   4455
   End
   Begin VB.Label lblRank 
      Caption         =   "&Minimum Number of Upstream Segments:"
      Height          =   255
      Left            =   360
      TabIndex        =   0
      Top             =   600
      WhatsThisHelpID =   25
      Width           =   4455
   End
   Begin VB.Label lblFile 
      Caption         =   "<none>"
      Height          =   252
      Index           =   4
      Left            =   3360
      TabIndex        =   27
      Top             =   4080
      Width           =   3972
   End
   Begin VB.Label lblFile 
      Caption         =   "<none>"
      Height          =   252
      Index           =   2
      Left            =   3360
      TabIndex        =   26
      Top             =   2880
      Width           =   3972
   End
   Begin VB.Label lblFile 
      Caption         =   "<none>"
      Height          =   252
      Index           =   1
      Left            =   3360
      TabIndex        =   25
      Top             =   2520
      Width           =   3972
   End
   Begin VB.Label lblFile 
      Caption         =   "<none>"
      Height          =   252
      Index           =   0
      Left            =   3360
      TabIndex        =   24
      Top             =   2160
      Width           =   3972
   End
   Begin VB.Label lblFiles 
      Caption         =   "&Input Files"
      Height          =   252
      Left            =   240
      TabIndex        =   6
      Top             =   1800
      Width           =   972
   End
   Begin VB.Label lblLanduse 
      Caption         =   "Land Uses (optional)"
      Height          =   252
      Left            =   1320
      TabIndex        =   23
      Top             =   3240
      Width           =   1932
   End
   Begin VB.Label lblArea 
      Caption         =   "Areas"
      Height          =   252
      Left            =   1320
      TabIndex        =   22
      Top             =   2880
      Width           =   1932
   End
   Begin VB.Label lblConn 
      Caption         =   "Connections"
      Height          =   252
      Left            =   1320
      TabIndex        =   21
      Top             =   2520
      Width           =   1932
   End
   Begin VB.Label lblReach 
      Caption         =   "Reaches"
      Height          =   252
      Left            =   1320
      TabIndex        =   20
      Top             =   2160
      Width           =   1932
   End
End
Attribute VB_Name = "frmGenScnSimplify"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
'Copyright 2000 by AQUA TERRA Consultants
Dim oldrank$, oldlarge$, oldsmall$
Dim irank&, rlarge!, rsmall!

Private Sub SimplifyInit()
  Dim i&
  irank = 0
  txtRank.text = irank
  rlarge = 1#
  txtLarge.text = rlarge
  rsmall = 0#
  txtSmall.text = rsmall
  For i = 0 To 6
    lblFile(i).Caption = "<none>"
  Next i
End Sub

Private Sub GetSpecs(Filename)
  Dim ichr$, i&
  On Error GoTo 10
  Open Filename For Input As #1
  Call SimplifyInit
  Line Input #1, ichr
  txtRank = ichr
  irank = txtRank.text
  Line Input #1, ichr
  txtLarge = ichr
  rlarge = txtLarge.text
  Line Input #1, ichr
  txtSmall = ichr
  rsmall = txtSmall.text
  For i = 0 To 6
    Line Input #1, ichr
    lblFile(i).Caption = ichr
  Next i
  Close #1
  GoTo 20
10  Close #1
    Call SimplifyInit
20  'go here if everything read okay
End Sub

Private Sub PutSpecs(Filename)
  Dim i&
  Open Filename For Output As #1
  Print #1, txtRank
  Print #1, txtLarge
  Print #1, txtSmall
  For i = 0 To 6
    Print #1, lblFile(i).Caption
  Next i
  Close #1
End Sub


Private Sub cmdClear_Click()
    Call SimplifyInit
End Sub


Private Sub cmdFile_Click(Index As Integer)
    CDFile(Index).flags = &H8806&
    If Index >= 0 And Index <= 3 Then
      CDFile(Index).filter = "ASCII files (*.inp)|*.inp|RDB files (*.rdb)|*.rdb|All files (*.*)|*.*"
    Else
      CDFile(Index).filter = "ASCII files (*.inp)|*.inp|All files (*.*)|*.*"
    End If
    If Index = 0 Then
      CDFile(Index).Filename = "reach.inp"
      CDFile(Index).DialogTitle = "GenScn Scenario New Simplify Network Reach File"
    ElseIf Index = 1 Then
      CDFile(Index).Filename = "rconn.inp"
      CDFile(Index).DialogTitle = "GenScn Scenario New Simplify Network Connection File"
    ElseIf Index = 2 Then
      CDFile(Index).Filename = "area.inp"
      CDFile(Index).DialogTitle = "GenScn Scenario New Simplify Network Area File"
    ElseIf Index = 3 Then
      CDFile(Index).Filename = "landuse.inp"
      CDFile(Index).DialogTitle = "GenScn Scenario New Simplify Network Land Use File"
    ElseIf Index = 4 Then
      CDFile(Index).Filename = "reachnew.inp"
      CDFile(Index).DialogTitle = "GenScn Scenario New Simplify Network Reach File"
    ElseIf Index = 5 Then
      CDFile(Index).Filename = "rconnnew.inp"
      CDFile(Index).DialogTitle = "GenScn Scenario New Simplify Network Connection File"
    ElseIf Index = 6 Then
      CDFile(Index).Filename = "areanew.inp"
      CDFile(Index).DialogTitle = "GenScn Scenario New Simplify Network Area File"
    End If
    On Error GoTo 10
    CDFile(Index).CancelError = True
    If Index = 4 Or Index = 5 Or Index = 6 Then
      CDFile(Index).Action = 2
    Else
      CDFile(Index).Action = 1
    End If
    lblFile(Index).Caption = CDFile(Index).Filename
10      'continue here on cancel
End Sub

Private Sub cmdGet_Click()
    CDGet.flags = &H1806&
    CDGet.filter = "GenScn Simplify Network Files (*.gsn)|*.gsn"
    CDGet.Filename = "*.gsn"
    CDGet.DialogTitle = "GenScn Simplify Network Get Specs File"
    On Error GoTo 10
    CDGet.CancelError = True
    CDGet.Action = 1
    Call GetSpecs(CDGet.Filename)
10 'continue here on cancel
End Sub

Private Sub cmdNew_Click(Index As Integer)
  Dim rchnam$, connam$, arenam$, lunam$, orchnam$, oconnam$, oarenam$, defname$
  If Index = 1 Then
    'simplify network
    Dim retcod&
    If lblFile(0).Caption = "<none>" Or _
       lblFile(1).Caption = "<none>" Or _
       lblFile(2).Caption = "<none>" Or _
       lblFile(4).Caption = "<none>" Or _
       lblFile(5).Caption = "<none>" Or _
       lblFile(6).Caption = "<none>" Then
       'this file is not optional
       MsgBox "One or more required files have not been selected.", _
         vbExclamation, "GenScn Scenario New Simplify Network"
    Else
      'okay to run
      rchnam = lblFile(0).Caption
      connam = lblFile(1).Caption
      arenam = lblFile(2).Caption
      lunam = lblFile(3).Caption
      orchnam = lblFile(4).Caption
      oconnam = lblFile(5).Caption
      oarenam = lblFile(6).Caption
      Call F90_SIMNET(irank, rlarge, rsmall, retcod, _
                      rchnam, connam, arenam, lunam, _
                      orchnam, oconnam, oarenam, _
                      Len(rchnam), Len(connam), Len(arenam), Len(lunam), _
                      Len(orchnam), Len(oconnam), Len(oarenam))
      If retcod = 3 Then
        'triple junction problem
        MsgBox "This network contains one or more triple junctions.", _
          vbExclamation, "GenScn Scenario New Simplify Network"
      ElseIf retcod = 2 Then
        'multiple outlets problem
        MsgBox "This network contains multiple outlets.", _
          vbExclamation, "GenScn Scenario New Simplify Network"
      ElseIf retcod = 1 Then
        'misordered reaches problem
        MsgBox "Reaches are not numbered sequentially.", _
          vbExclamation, "GenScn Scenario New Simplify Network"
      ElseIf retcod = -1 Then
        'file read error
        MsgBox "Error reading files.", _
          vbExclamation, "GenScn Scenario New Simplify Network"
      ElseIf retcod = -2 Then
        'file open error
        MsgBox "Error opening files.", _
          vbExclamation, "GenScn Scenario New Simplify Network"
      ElseIf retcod = 0 Then
        'successful creation
        MsgBox "Successful simplification of network.", 0, _
          "GenScn Scenario New Simplify Network"
        'default file names to new scen screen
        frmGenScnNewScen.lblFile(0).Caption = lblFile(4).Caption
        frmGenScnNewScen.lblFile(1).Caption = lblFile(5).Caption
        frmGenScnNewScen.lblFile(2).Caption = lblFile(6).Caption
        frmGenScnNewScen.cmdNew(1).Enabled = False
        Call frmGenScnNewScen.ClearSummary
        frmGenScnNewScen.SSNewTab.Tab = 0
        'clear summary here
        If frmGenScnNewScen.lblFile(3).Caption <> "<none>" Then
          frmGenScnNewScen.cmdNew(3).Enabled = True
        Else
          frmGenScnNewScen.cmdNew(3).Enabled = False
        End If
      End If
    End If
  ElseIf Index = 2 Then
    'close
    'defname = UciPath & "\default.gsn"
    defname = "default.gsn"
    Call PutSpecs(defname)
    Unload frmGenScnSimplify
  End If
End Sub

Private Sub cmdSave_Click()
    CDSave.flags = &H8806&
    CDSave.filter = "GenScn Simplify Network Files (*.gsn)|*.gsn"
    CDSave.Filename = "*.gsn"
    CDSave.DialogTitle = "GenScn Simplify Network Save Specs File"
    On Error GoTo 10
    CDSave.CancelError = True
    CDSave.Action = 2
    Call PutSpecs(CDSave.Filename)
10 'continue here on cancel
End Sub

Private Sub Form_Load()
  Dim defname$
  'defname = UciPath & "\default.gsn"
  defname = "default.gsn"
  Call GetSpecs(defname)
  'default file names from new scen screen
  If lblFile(0).Caption = "<none>" And _
    frmGenScnNewScen.lblFile(0).Caption <> "<none>" Then
    lblFile(0).Caption = frmGenScnNewScen.lblFile(0).Caption
  End If
  If lblFile(1).Caption = "<none>" And _
    frmGenScnNewScen.lblFile(1).Caption <> "<none>" Then
    lblFile(1).Caption = frmGenScnNewScen.lblFile(1).Caption
  End If
  If lblFile(2).Caption = "<none>" And _
    frmGenScnNewScen.lblFile(2).Caption <> "<none>" Then
    lblFile(2).Caption = frmGenScnNewScen.lblFile(2).Caption
  End If
  If lblFile(3).Caption = "<none>" And _
    frmGenScnNewScen.lblFile(4).Caption <> "<none>" Then
    lblFile(3).Caption = frmGenScnNewScen.lblFile(4).Caption
  End If
End Sub

Private Sub txtLarge_GotFocus()
    oldlarge = txtLarge.text
End Sub

Private Sub txtLarge_LostFocus()
    Dim newlarge$, chgflg&
    If Len(txtLarge.text) > 0 Then
      newlarge = txtLarge.text
      Call ChkTxtR("Fraction", 0#, 1#, newlarge, rlarge, chgflg)
      If chgflg <> 1 Then
        txtLarge.text = oldlarge
      End If
    Else
      txtLarge.text = oldlarge
    End If
End Sub

Private Sub txtRank_GotFocus()
    oldrank = txtRank.text
End Sub

Private Sub txtRank_LostFocus()
    Dim newrank$, chgflg&
    If Len(txtRank.text) > 0 Then
      newrank = txtRank.text
      Call ChkTxtI("Rank", 0, 20, newrank, irank, chgflg)
      If chgflg <> 1 Then
        txtRank.text = oldrank
      End If
    Else
      txtRank.text = oldrank
    End If
End Sub


Private Sub txtSmall_GotFocus()
    oldsmall = txtSmall.text
End Sub

Private Sub txtSmall_LostFocus()
    Dim newsmall$, chgflg&
    If Len(txtSmall.text) > 0 Then
      newsmall = txtSmall.text
      Call ChkTxtR("Fraction", 0#, 1#, newsmall, rsmall, chgflg)
      If chgflg <> 1 Then
        txtSmall.text = oldsmall
      End If
    Else
      txtSmall.text = oldsmall
    End If
End Sub
