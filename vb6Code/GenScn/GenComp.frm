VERSION 5.00
Begin VB.Form frmGenScnCompare 
   Caption         =   "GenScn Compare"
   ClientHeight    =   5280
   ClientLeft      =   168
   ClientTop       =   348
   ClientWidth     =   8424
   BeginProperty Font 
      Name            =   "MS Sans Serif"
      Size            =   7.8
      Charset         =   0
      Weight          =   700
      Underline       =   0   'False
      Italic          =   0   'False
      Strikethrough   =   0   'False
   EndProperty
   HelpContextID   =   108
   Icon            =   "GenComp.frx":0000
   LinkTopic       =   "Form1"
   PaletteMode     =   1  'UseZOrder
   ScaleHeight     =   5280
   ScaleWidth      =   8424
   Begin VB.TextBox txtTitle 
      BackColor       =   &H00FFFFFF&
      Height          =   492
      Left            =   120
      MultiLine       =   -1  'True
      TabIndex        =   0
      Text            =   "GenComp.frx":0442
      Top             =   120
      Width           =   8172
   End
   Begin TabDlg.SSTab SSTabCompare 
      Height          =   3972
      Left            =   120
      TabIndex        =   1
      Top             =   720
      Width           =   8172
      _ExtentX        =   14415
      _ExtentY        =   7006
      _Version        =   393216
      Tab             =   2
      TabHeight       =   423
      BeginProperty Font {0BE35203-8F91-11CE-9DE3-00AA004BB851} 
         Name            =   "MS Sans Serif"
         Size            =   7.8
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      TabCaption(0)   =   "Class &Limits"
      TabPicture(0)   =   "GenComp.frx":0447
      Tab(0).ControlEnabled=   0   'False
      Tab(0).Control(0)=   "fraTab(0)"
      Tab(0).ControlCount=   1
      TabCaption(1)   =   "&Select Statistics"
      TabPicture(1)   =   "GenComp.frx":0463
      Tab(1).ControlEnabled=   0   'False
      Tab(1).Control(0)=   "SelectResults"
      Tab(1).ControlCount=   1
      TabCaption(2)   =   "&Results"
      TabPicture(2)   =   "GenComp.frx":047F
      Tab(2).ControlEnabled=   -1  'True
      Tab(2).Control(0)=   "agd"
      Tab(2).Control(0).Enabled=   0   'False
      Tab(2).ControlCount=   1
      Begin VB.Frame fraTab 
         BorderStyle     =   0  'None
         Caption         =   "Frame1"
         Height          =   3492
         Index           =   0
         Left            =   -74880
         TabIndex        =   36
         Top             =   360
         Width           =   7932
         Begin VB.Frame fraMissing 
            Caption         =   "Missing Values"
            Height          =   1020
            Left            =   0
            TabIndex        =   20
            Top             =   2400
            Width           =   3252
            Begin VB.TextBox txtMissing2 
               Height          =   288
               Left            =   2160
               TabIndex        =   24
               Text            =   "txtMissing2"
               Top             =   600
               Width           =   972
            End
            Begin VB.TextBox txtMissing1 
               Height          =   288
               Left            =   2160
               TabIndex        =   22
               Text            =   "txtMissing1"
               Top             =   240
               Width           =   972
            End
            Begin VB.Label lblMissing2 
               Caption         =   "Secon&d Time series:"
               Height          =   252
               Left            =   240
               TabIndex        =   23
               Top             =   636
               Width           =   1932
            End
            Begin VB.Label lblMissing1 
               Caption         =   "&First Time series:"
               Height          =   252
               Left            =   240
               TabIndex        =   21
               Top             =   276
               Width           =   1932
            End
         End
         Begin VB.Frame frmCLimitsDef 
            Caption         =   "Defaults"
            Height          =   2172
            Left            =   0
            TabIndex        =   2
            Top             =   120
            Width           =   1812
            Begin VB.TextBox txtUpper 
               Height          =   288
               Left            =   840
               TabIndex        =   10
               Text            =   "UpperCLimits"
               Top             =   1680
               Width           =   852
            End
            Begin VB.TextBox txtLower 
               BackColor       =   &H00FFFFFF&
               Height          =   288
               Left            =   840
               TabIndex        =   8
               Text            =   "LowerCLimits"
               Top             =   1320
               Width           =   852
            End
            Begin VB.ComboBox cmbCLimits 
               Height          =   288
               Left            =   840
               Style           =   2  'Dropdown List
               TabIndex        =   6
               Top             =   840
               Width           =   852
            End
            Begin VB.OptionButton optCLimits 
               Caption         =   "Logarit&hmic"
               Height          =   252
               Index           =   1
               Left            =   120
               TabIndex        =   4
               Top             =   480
               Value           =   -1  'True
               Width           =   1332
            End
            Begin VB.OptionButton optCLimits 
               Caption         =   "&Arithmetic"
               Height          =   252
               Index           =   0
               Left            =   120
               TabIndex        =   3
               Top             =   240
               Width           =   1212
            End
            Begin VB.Label lblCLimitsnum 
               Caption         =   "&Number:"
               Height          =   252
               Left            =   120
               TabIndex        =   5
               Top             =   880
               Width           =   852
            End
            Begin VB.Label lblLower 
               Caption         =   "L&ower:"
               Height          =   252
               Left            =   120
               TabIndex        =   7
               Top             =   1360
               Width           =   732
            End
            Begin VB.Label lblUpper 
               Caption         =   "&Upper:"
               Height          =   252
               Left            =   120
               TabIndex        =   9
               Top             =   1720
               Width           =   732
            End
         End
         Begin VB.ListBox lstActCLimits 
            Height          =   1584
            ItemData        =   "GenComp.frx":049B
            Left            =   3960
            List            =   "GenComp.frx":04A2
            TabIndex        =   19
            Top             =   240
            Width           =   1092
         End
         Begin VB.TextBox txtCLimits 
            Height          =   288
            Left            =   1920
            TabIndex        =   13
            Top             =   2040
            Width           =   1092
         End
         Begin VB.CommandButton cmdCLimitsaddall 
            Caption         =   ">>"
            Height          =   252
            Left            =   3240
            TabIndex        =   14
            Top             =   240
            Width           =   492
         End
         Begin VB.CommandButton cmdCLimitsadd 
            Caption         =   "--&>"
            Height          =   252
            Left            =   3240
            TabIndex        =   15
            Top             =   600
            Width           =   492
         End
         Begin VB.CommandButton cmdCLimitsdrop 
            Caption         =   "&<--"
            Height          =   252
            Left            =   3240
            TabIndex        =   16
            Top             =   960
            Width           =   492
         End
         Begin VB.CommandButton cmdCLimitsdelall 
            Caption         =   "<<"
            Height          =   252
            Left            =   3240
            TabIndex        =   17
            Top             =   1320
            Width           =   492
         End
         Begin VB.ListBox lstCLimits 
            Height          =   1584
            ItemData        =   "GenComp.frx":04B5
            Left            =   1920
            List            =   "GenComp.frx":04BC
            TabIndex        =   12
            Top             =   240
            Width           =   1092
         End
         Begin VB.Frame fraFORTRAN 
            Caption         =   "FORTRAN Compare"
            Height          =   1932
            Left            =   5280
            TabIndex        =   25
            Top             =   120
            Width           =   2052
            Begin VB.CommandButton cmdOutfile 
               Caption         =   "F90 Output File"
               Height          =   372
               Left            =   240
               TabIndex        =   28
               ToolTipText     =   "Save FORTRAN results to a text file"
               Top             =   1320
               Width           =   1452
            End
            Begin VB.CommandButton cmdResults 
               Caption         =   "F90 Results"
               Height          =   372
               Left            =   240
               TabIndex        =   27
               ToolTipText     =   "View results of FORTRAN computation"
               Top             =   840
               Width           =   1452
            End
            Begin VB.CommandButton cmdAnalyze 
               Caption         =   "F90 Compute"
               Height          =   372
               Left            =   240
               TabIndex        =   26
               ToolTipText     =   "Use FORTRAN routines to compare these time series"
               Top             =   360
               Width           =   1452
            End
            Begin MSComDlg.CommonDialog CDOutfile 
               Left            =   1680
               Top             =   1320
               _ExtentX        =   699
               _ExtentY        =   699
               _Version        =   393216
               FontSize        =   3.14179e-37
            End
         End
         Begin VB.Label lblCLimitsAvail 
            Caption         =   "Availa&ble"
            Height          =   252
            Left            =   2040
            TabIndex        =   11
            Top             =   0
            Width           =   852
         End
         Begin VB.Label lblCLimitsAct 
            Caption         =   "A&ctive"
            Height          =   252
            Left            =   4200
            TabIndex        =   18
            Top             =   0
            Width           =   612
         End
      End
      Begin ATCoCtl.ATCoSelectList SelectResults 
         Height          =   3492
         Left            =   -74880
         TabIndex        =   37
         Top             =   360
         Width           =   7932
         _ExtentX        =   13991
         _ExtentY        =   6160
         RightLabel      =   "Selected:"
         LeftLabel       =   "Available:"
      End
      Begin ATCoCtl.ATCoGrid agd 
         Height          =   3492
         Left            =   120
         TabIndex        =   38
         Top             =   360
         Width           =   7932
         _ExtentX        =   13991
         _ExtentY        =   6160
         SelectionToggle =   0   'False
         AllowBigSelection=   -1  'True
         AllowEditHeader =   0   'False
         AllowLoad       =   0   'False
         AllowSorting    =   0   'False
         Rows            =   1
         Cols            =   2
         ColWidthMinimum =   300
         gridFontBold    =   0   'False
         gridFontItalic  =   0   'False
         gridFontName    =   "Courier New"
         gridFontSize    =   8
         gridFontUnderline=   0   'False
         gridFontWeight  =   400
         gridFontWidth   =   0
         Header          =   ""
         FixedRows       =   1
         FixedCols       =   0
         ScrollBars      =   3
         SelectionMode   =   0
         BackColor       =   -2147483643
         ForeColor       =   -2147483640
         BackColorBkg    =   -2147483632
         BackColorSel    =   -2147483635
         ForeColorSel    =   -2147483634
         BackColorFixed  =   -2147483633
         ForeColorFixed  =   -2147483630
         InsideLimitsBackground=   -2147483643
         OutsideHardLimitBackground=   8421631
         OutsideSoftLimitBackground=   8454143
         ComboCheckValidValues=   0   'False
      End
   End
   Begin VB.Frame fraButtons 
      BorderStyle     =   0  'None
      Height          =   372
      Left            =   120
      TabIndex        =   29
      Top             =   4800
      Width           =   8172
      Begin VB.CommandButton cmdSaveResults 
         Caption         =   "Sa&ve Results"
         Height          =   372
         Left            =   960
         TabIndex        =   31
         ToolTipText     =   "Save results to a text file"
         Top             =   0
         Width           =   1332
      End
      Begin VB.CommandButton cmdClose 
         Cancel          =   -1  'True
         Caption         =   "Close"
         Height          =   372
         Left            =   7200
         TabIndex        =   35
         Top             =   0
         Width           =   972
      End
      Begin VB.CommandButton cmdClear 
         Caption         =   "Cl&ear Specs"
         Height          =   372
         Left            =   5640
         TabIndex        =   34
         Top             =   0
         Width           =   1332
      End
      Begin VB.CommandButton cmdGet 
         Caption         =   "&Get Specs"
         Height          =   372
         Left            =   4080
         TabIndex        =   33
         Top             =   0
         Width           =   1332
      End
      Begin VB.CommandButton cmdSave 
         Caption         =   "Save S&pecs"
         Height          =   372
         Left            =   2520
         TabIndex        =   32
         ToolTipText     =   "Save class limits, missing values, and selected statistics"
         Top             =   0
         Width           =   1332
      End
      Begin MSComDlg.CommonDialog CDSave 
         Left            =   3840
         Top             =   0
         _ExtentX        =   699
         _ExtentY        =   699
         _Version        =   393216
         FontSize        =   3.14179e-37
      End
      Begin MSComDlg.CommonDialog CDGet 
         Left            =   5400
         Top             =   0
         _ExtentX        =   699
         _ExtentY        =   699
         _Version        =   393216
         FontSize        =   3.14179e-37
      End
      Begin VB.CommandButton cmdSwap 
         Caption         =   "S&wap"
         Height          =   372
         Left            =   0
         TabIndex        =   30
         ToolTipText     =   "Exchange first and second time series and recompute results"
         Top             =   0
         Width           =   732
      End
   End
End
Attribute VB_Name = "frmGenScnCompare"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
'Copyright 2000 by AQUA TERRA Consultants
Private oldlower$, oldupper$, savcol&, savrow&, OutFile$, oldmiss1$, oldmiss2$
Private pTScoll As Collection

Public Sub Showcoll(TimeseriesColl As Collection)
  Set pTScoll = TimeseriesColl
  Me.Show
  With pTScoll
    txtTitle.text = "Compare Analysis for " & _
      .item(1).Header.sen & " " & _
      .item(1).Header.con & " at " & _
      .item(1).Header.loc & vbCrLf & " and " & _
      .item(2).Header.sen & " " & _
      .item(2).Header.con & " at " & _
      .item(2).Header.loc
  End With
  If SSTabCompare.Tab = 2 Then Recalculate
End Sub

Private Sub CompInit()
  Dim i&
  optCLimits(0) = False
  optCLimits(1) = True
  lstActCLimits.clear
  txtLower = "1"
  txtUpper = "10000"
  txtMissing1 = -99999#
  txtMissing2 = -99999#
  cmbCLimits.clear
  For i = 1 To 35
    cmbCLimits.AddItem i
  Next i
  cmbCLimits.ListIndex = 34
  cmdAnalyze.Enabled = False
  With SelectResults
    .ClearLeft
    .ClearRight
    .LeftItemFastAdd "Count 1"
    .LeftItemFastAdd "Count 2"
    .LeftItemFastAdd "Percent 1"
    .LeftItemFastAdd "Percent 2"
    .LeftItemFastAdd "Mean 1"
    .LeftItemFastAdd "Mean 2"
    .LeftItemFastAdd "Geometric Mean 1"
    .LeftItemFastAdd "Geometric Mean 2"
    .LeftItemFastAdd "Correlation Coefficient"
    .LeftItemFastAdd "Coefficient of Determination"
    .LeftItemFastAdd "Mean Error"
    .LeftItemFastAdd "Percent Mean Error"
    .LeftItemFastAdd "Mean Absolute Error"
    .LeftItemFastAdd "Percent Mean Absolute Error"
    .LeftItemFastAdd "RMS Error"
    '.LeftItemFastAdd "Nash Sutcliffe"
    .LeftItemFastAdd "Model Fit Efficiency (NS)"
    .MoveAllRight
    .LeftItemFastAdd "Count >= 1"
    .LeftItemFastAdd "Count >= 2"
    .LeftItemFastAdd "Count Compare"
    .LeftItemFastAdd "Percent >= 1"
    .LeftItemFastAdd "Percent >= 2"
    .LeftItemFastAdd "Std Deviation 1"
    .LeftItemFastAdd "Std Deviation 2"
  End With
End Sub

'set up class limit defaults
Private Sub SetCLimitsDef()
  Dim i&, j&
  lstCLimits.clear
  Dim rval!, inc!, diff!, r10!, InList As Boolean
  If optCLimits(0) = True Then
    'fill arithmetic
    If val(txtLower.text) = 1 Then
      'special case
      diff = val(txtUpper.text) - val(txtLower.text) + 1
      If cmbCLimits.ListIndex > 1 Then
        inc = diff / (cmbCLimits.ListIndex - 1)
      End If
      InList = False
      For i = 0 To lstActCLimits.ListCount - 1
        'check to see if this number is already in active list
        If val(lstActCLimits.List(i)) = 0 Then
          'already in list
          InList = True
        End If
      Next i
      If InList = False Then
        lstCLimits.AddItem 0
      End If
      InList = False
      For i = 0 To lstActCLimits.ListCount - 1
        'check to see if this number is already in active list
        If val(lstActCLimits.List(i)) = val(txtLower.text) Then
          'already in list
          InList = True
        End If
      Next i
      If InList = False Then
        lstCLimits.AddItem txtLower.text
      End If
      rval = val(txtLower.text) - 1
      If cmbCLimits.ListIndex > 1 Then
        For i = 1 To cmbCLimits.ListIndex - 1
          rval = rval + inc
          Call F90_DECPRC(3, 2, rval)
          InList = False
          For j = 0 To lstActCLimits.ListCount - 1
            'check to see if this number is already in active list
            If val(lstActCLimits.List(j)) = rval Then
              'already in list
              InList = True
            End If
          Next j
          If InList = False Then
            lstCLimits.AddItem rval
          End If
        Next i
      End If
    Else
      diff = val(txtUpper.text) - val(txtLower.text)
      If cmbCLimits.ListIndex <> 0 Then
        inc = diff / (cmbCLimits.ListIndex - 1)
      End If
      InList = False
      For i = 0 To lstActCLimits.ListCount - 1
        'check to see if this number is already in active list
        If val(lstActCLimits.List(i)) = 0 Then
          'already in list
          InList = True
        End If
      Next i
      If InList = False Then
        lstCLimits.AddItem 0
      End If
      InList = False
      For i = 0 To lstActCLimits.ListCount - 1
        'check to see if this number is already in active list
        If val(lstActCLimits.List(i)) = val(txtLower.text) Then
          'already in list
          InList = True
        End If
      Next i
      If InList = False Then
        lstCLimits.AddItem txtLower.text
      End If
      rval = val(txtLower.text)
      If cmbCLimits.ListIndex > 1 Then
        For i = 1 To cmbCLimits.ListIndex - 1
          rval = rval + inc
          Call F90_DECPRC(3, 2, rval)
          InList = False
          For j = 0 To lstActCLimits.ListCount - 1
            'check to see if this number is already in active list
            If val(lstActCLimits.List(j)) = rval Then
              'already in list
              InList = True
            End If
          Next j
          If InList = False Then
            lstCLimits.AddItem rval
          End If
        Next i
      End If
    End If
  Else
    'fill logarithmic
    diff = Log10(val(txtUpper.text)) - Log10(val(txtLower.text))
    If cmbCLimits.ListIndex > 1 Then
      inc = diff / (cmbCLimits.ListIndex - 1)
    End If
    InList = False
    For i = 0 To lstActCLimits.ListCount - 1
      'check to see if this number is already in active list
      If val(lstActCLimits.List(i)) = 0 Then
        'already in list
        InList = True
      End If
    Next i
    If InList = False Then
      lstCLimits.AddItem 0
    End If
    InList = False
    For i = 0 To lstActCLimits.ListCount - 1
      'check to see if this number is already in active list
      If val(lstActCLimits.List(i)) = val(txtLower.text) Then
        'already in list
        InList = True
      End If
    Next i
    If InList = False Then
      lstCLimits.AddItem txtLower.text
    End If
    rval = Log10(val(txtLower.text))
    If cmbCLimits.ListIndex > 1 Then
      For i = 1 To cmbCLimits.ListIndex - 1
        rval = rval + inc
        r10 = 10 ^ rval
        Call F90_DECPRC(2, 2, r10)
        InList = False
        For j = 0 To lstActCLimits.ListCount - 1
          'check to see if this number is already in active list
          If val(lstActCLimits.List(j)) = r10 Then
            'already in list
            InList = True
          End If
        Next j
        If InList = False Then
          lstCLimits.AddItem r10
        End If
      Next i
    End If
  End If
End Sub

Private Sub GetSpecs(Filename)
  Dim i&, inum&, ichr$
  
  CompInit
  
  On Error GoTo ErrorGettingFile
  
  Open Filename For Input As #1
  lstCLimits.clear
  lstActCLimits.clear
  Line Input #1, ichr
  inum = CLng(ichr)
  For i = 0 To inum - 1
    Line Input #1, ichr
    lstCLimits.AddItem ichr
  Next i
  Line Input #1, ichr
  inum = CLng(ichr)
  For i = 0 To inum - 1
    Line Input #1, ichr
    lstActCLimits.AddItem ichr
  Next i
  cmbCLimits.clear
  For i = 1 To 35
    cmbCLimits.AddItem i
  Next i
  Line Input #1, ichr:  txtLower = ichr
  Line Input #1, ichr:  txtUpper = ichr
  Line Input #1, ichr:  optCLimits(0) = ichr
  Line Input #1, ichr:  optCLimits(1) = ichr
  Line Input #1, ichr:  cmbCLimits.ListIndex = CLng(ichr)
  Line Input #1, ichr:  txtMissing1 = ichr
  Line Input #1, ichr:  txtMissing2 = ichr
  
  If lstActCLimits.ListCount > 0 Then
    cmdAnalyze.Enabled = True
  Else
    cmdAnalyze.Enabled = False
  End If
  
  Line Input #1, ichr
  If Len(ichr) > 0 Then
    SelectResults.MoveAllLeft
    While Len(ichr) > 0
      ichr = LCase(ichr)
      For i = 0 To SelectResults.LeftCount
        If LCase(SelectResults.LeftItem(i)) = ichr Then
          SelectResults.MoveRight i
          Exit For
        End If
      Next
      If EOF(1) Then ichr = "" Else Line Input #1, ichr
    Wend
  End If
  
ErrorGettingFile:
  err.clear
  On Error Resume Next
  Close #1

End Sub

Private Sub PutSpecs(Filename)
  Dim i&
  Open Filename For Output As #1
  Print #1, lstCLimits.ListCount
  For i = 0 To lstCLimits.ListCount - 1
    Print #1, lstCLimits.List(i)
  Next i
  Print #1, lstActCLimits.ListCount
  For i = 0 To lstActCLimits.ListCount - 1
    Print #1, lstActCLimits.List(i)
  Next i
  Print #1, txtLower
  Print #1, txtUpper
  Print #1, optCLimits(0)
  Print #1, optCLimits(1)
  Print #1, cmbCLimits.ListIndex
  Print #1, txtMissing1
  Print #1, txtMissing2
  For i = 0 To SelectResults.RightCount - 1
    Print #1, SelectResults.RightItem(i)
  Next
  
  Close #1
End Sub

Private Sub cmbCLimits_Change()
  SetCLimitsDef
End Sub

Private Sub cmbCLimits_Click()
  SetCLimitsDef
End Sub

Private Sub cmdAnalyze_Click()
  Dim mxlev&, i&
  Dim ncint&, rclint!(35), rvals!(), NVALS&
  Dim dsn&(2), rmiss!(2), retcod&
  Dim SDate&(6), EDate&(6), Tu&, ts&, Dtran&
  Dim zanb&, zb&, za&, zbna&, zab&, tnum&, etot&(8)
  Dim tsdif!, tpdif!, tsdif2!, tpdif2!, tbias!, tpbias!, stest!
  Dim cpcta!(35), cpctb!(35)
  Dim Label1$, Label2$
  Dim col As Long
  
  Me.MousePointer = vbHourglass
  
  mxlev = 35
  ncint = lstActCLimits.ListCount
  If ncint = 0 Then
    'problem, no class limits defined
    MsgBox "No Class Limits have been Set", vbExclamation, "GenScn Compare Compute Problem"
    SSTabCompare.Tab = 0
  Else
    If ncint > 35 Then
      ncint = 35
    End If
    For i = 0 To ncint - 1
      rclint(i) = val(lstActCLimits.List(i))
    Next i
    rclint(i) = 1E+30
    SDate(0) = CSDat(0)
    SDate(1) = CSDat(1)
    SDate(2) = CSDat(2)
    SDate(3) = 0
    SDate(4) = 0
    SDate(5) = 0
    EDate(0) = CEDat(0)
    EDate(1) = CEDat(1)
    EDate(2) = CEDat(2)
    EDate(3) = 24
    EDate(4) = 0
    EDate(5) = 0
    
    Tu = pTScoll(1).dates.Summary.Tu
    ts = pTScoll(1).dates.Summary.ts
    Dtran = 1
    NVALS = pTScoll(1).dates.Summary.NVALS
    ReDim rvals(NVALS * 2)
    For i = 1 To NVALS
      rvals(i) = pTScoll(1).value(i)
      rvals(NVALS + i) = pTScoll(2).value(i)
    Next i
    rmiss(0) = txtMissing1
    rmiss(1) = txtMissing2
    Label1 = pTScoll(1).Header.sen & " " & _
             pTScoll(1).Header.con & " at " & _
             pTScoll(1).Header.loc
    Label2 = pTScoll(2).Header.sen & " " & _
             pTScoll(2).Header.con & " at " & _
             pTScoll(2).Header.loc
            
    'set output file
    OutFile = CDOutfile.Filename
    If Len(OutFile) = 0 Then
      OutFile = "compare.out"
    End If
    'all set to do compare analysis
    Call F90_TSCBAT(NVALS, rvals(0), ncint, rclint(0), rmiss(0), _
                    SDate(0), EDate(0), Tu, ts, Dtran, _
                    zanb, zb, za, zbna, zab, tnum, tsdif, tpdif, _
                    tsdif2, tpdif2, tbias, tpbias, stest, etot(0), _
                    cpcta(0), cpctb(0), _
                    Label1, Label2, OutFile, Len(Label1), Len(Label2), Len(OutFile))
    cmdResults.Enabled = True
    cmdResults.SetFocus
  End If
  
  Me.MousePointer = vbDefault

  Exit Sub

'---------------------------------------------------------------------------------
'
'Recalculate:
'  agd.ClearData
'
'  For i = 1 To ncint
'    agd.TextMatrix(i, 0) = rclint(i - 1)
'  Next
'  agd.TextMatrix(i + 1, 0) = "Total"
'  For col = 1 To pStatistics.count
'    agd.ColTitle(col) = pStatistics(col)
'    For i = 1 To ncint
'      agd.TextMatrix(i, col) = Statistic(pStatistics(col), pTScoll, rclint(i - 1), rclint(i))
'    Next
'    agd.TextMatrix(i + 1, col) = Statistic(pStatistics(col), pTScoll)
'    DoEvents
'  Next
'
  Return
End Sub

Private Sub Recalculate()
  Dim StatisticName As Variant
  Dim StatisticValue As String
  Dim agdrow As Long
  Dim agdcol As Long
  
  Dim classIndex As Long
  Dim nClasses As Long
  Dim ClassLimit() As Single
  Dim ClassMin As Single
  Dim ClassMax As Single
  
  Dim Count1 As Long
  Dim Count2 As Long
  Dim CountGE1 As Long
  Dim CountGE2 As Long
  Dim CountCompare As Long
  
  Dim valIndex As Long
  Dim NVALS As Long
  
  Dim val1 As Double
  Dim val2 As Double
  Dim valDiff As Double
  Dim Sum1 As Double
  Dim Sum2 As Double
  Dim SD1 As Double 'Standard Deviation
  Dim SD2 As Double
  Dim Mean1 As Double
  Dim Mean2 As Double
  Dim GeomMean1 As Double
  Dim GeomMean2 As Double
  Dim ComputingGeomMean1 As Boolean
  Dim ComputingGeomMean2 As Boolean
  Dim MeanError As Double
  Dim MeanAbsoluteError As Double
  Dim RMSerror As Double
  
  Dim Pass2Done As Boolean
  Dim NashSutcliffeNumerator As Double
  Dim NashSutcliffe As Double
  Dim CorrelationCoefficient As Double

  Dim pStatistics As New Collection
  
  Dim checkmissing1 As Boolean
  Dim checkmissing2 As Boolean
  Dim missing1 As Single
  Dim missing2 As Single
  Dim curMissing As Boolean
  Dim curProcessed As Boolean
  
  SSTabCompare.Enabled = False
  Me.MousePointer = vbHourglass
  
  If IsNumeric(txtMissing1) Then
    checkmissing1 = True
    missing1 = CSng(txtMissing1)
  Else
    checkmissing1 = False
  End If
  If IsNumeric(txtMissing2) Then
    checkmissing2 = True
    missing2 = CSng(txtMissing2)
  Else
    checkmissing2 = False
  End If
  
  For agdrow = 0 To SelectResults.RightCount - 1
    pStatistics.Add SelectResults.RightItem(agdrow)
  Next
  
  agd.Rows = 1
  agd.cols = 2
  agdcol = 1
  For Each StatisticName In pStatistics
    agd.ColTitle(agdcol) = StatisticName
    agdcol = agdcol + 1
  Next
  NVALS = pTScoll(1).dates.Summary.NVALS
  nClasses = lstActCLimits.ListCount
  ReDim ClassLimit(nClasses + 1)
  For classIndex = 1 To nClasses
    ClassLimit(classIndex) = val(lstActCLimits.List(classIndex - 1))
    StatisticValue = NumFmted(ClassLimit(classIndex), 9, 2)
    If Right(StatisticValue, 3) = ".00" Then StatisticValue = Left(StatisticValue, Len(StatisticValue) - 3)
    agd.TextMatrix(classIndex, 0) = StatisticValue
  Next
  ClassLimit(nClasses + 1) = 1E+30
  agd.TextMatrix(nClasses + 2, 0) = "Total"
  
  For classIndex = 1 To nClasses + 1
    DoEvents
    If classIndex > nClasses Then
      ClassMin = -1E+30
      ClassMax = 1E+30
    Else
      ClassMin = ClassLimit(classIndex)
      ClassMax = ClassLimit(classIndex + 1)
    End If
    Count1 = 0
    Count2 = 0
    CountCompare = 0
    CountGE1 = 0
    CountGE2 = 0
    Sum1 = 0
    Sum2 = 0
    SD1 = 0
    SD2 = 0
    Mean1 = 0
    Mean2 = 0
    GeomMean1 = 0
    GeomMean2 = 0
    ComputingGeomMean1 = True
    ComputingGeomMean2 = True
    MeanError = 0
    MeanAbsoluteError = 0
    RMSerror = 0
    
    Pass2Done = False
    NashSutcliffe = 0
    CorrelationCoefficient = 0
    
    For valIndex = 1 To NVALS
      curMissing = False
      curProcessed = False
      val1 = pTScoll(1).value(valIndex)
      val2 = pTScoll(2).value(valIndex)
      If val1 >= ClassMin Then
        If checkmissing1 Then
          If Abs(val1 - missing1) < 0.01 Then
            curMissing = True
            GoTo val1Missing
          End If
        End If
        CountGE1 = CountGE1 + 1
        If val1 < ClassMax Then
          'curProcessed = True (only count simulated (2nd timser))
          Count1 = Count1 + 1
          Sum1 = Sum1 + val1
          SD1 = SD1 + val1 * val1
          If ComputingGeomMean1 Then
            If val1 > 0 Then
              GeomMean1 = GeomMean1 + Log(val1)
            Else
              ComputingGeomMean1 = False
            End If
          End If
        End If
      End If
val1Missing:
      If val2 >= ClassMin Then
        If checkmissing2 Then
          If Abs(val2 - missing2) < 0.01 Then
            curMissing = True
            GoTo val2Missing
          End If
        End If
        CountGE2 = CountGE2 + 1
        If val2 < ClassMax Then
          curProcessed = True
          Count2 = Count2 + 1
          Sum2 = Sum2 + val2
          SD2 = SD2 + val2 * val2
          If ComputingGeomMean2 Then
            If val2 > 0 Then
              GeomMean2 = GeomMean2 + Log(val2)
            Else
              ComputingGeomMean2 = False
            End If
          End If
        End If
      End If
val2Missing:
      If Not (curMissing) And curProcessed Then
        CountCompare = CountCompare + 1
        valDiff = val1 - val2   'timeseries must be ordered as                                'are ordered as observed(1) and simulated(2)
                                'simulated(1), then observed(1)
        MeanError = MeanError + valDiff
        MeanAbsoluteError = MeanAbsoluteError + Abs(valDiff)
        RMSerror = RMSerror + valDiff * valDiff
      End If
    Next
    NashSutcliffeNumerator = RMSerror
    If Count1 > 0 Then
      Mean1 = Sum1 / Count1
      If ComputingGeomMean1 Then
        GeomMean1 = Exp(GeomMean1 / Count1)
      Else
        GeomMean1 = 0
      End If
      SD1 = (SD1 - Sum1 * Mean1)
      If Count1 > 1 Then SD1 = SD1 / (Count1 - 1)
      If SD1 > 0 Then SD1 = Sqr(SD1)
    End If
    If Count2 > 0 Then
      Mean2 = Sum2 / Count2
      If ComputingGeomMean2 Then
        GeomMean2 = Exp(GeomMean2 / Count2)
      Else
        GeomMean2 = 0
      End If
      SD2 = (SD2 - Sum2 * Mean2)
      If Count2 > 1 Then SD2 = SD2 / (Count2 - 1)
      If SD2 > 0 Then SD2 = Sqr(SD2)
      
      MeanError = MeanError / CountCompare
      MeanAbsoluteError = MeanAbsoluteError / CountCompare
      RMSerror = RMSerror / CountCompare
    End If
    If RMSerror > 0 Then RMSerror = Sqr(RMSerror)
    
    'Now we have computed all the one-pass stats for this class
    agdcol = 1
    If classIndex > nClasses Then
      agdrow = classIndex + 1
    Else
      agdrow = classIndex
    End If
    For Each StatisticName In pStatistics
      StatisticValue = ""
      Select Case StatisticName
        Case "Count 1":              StatisticValue = NumFmtI(Count1, 6)
        Case "Count 2":              StatisticValue = NumFmtI(Count2, 6)
        Case "Count >= 1":           StatisticValue = NumFmtI(CountGE1, 6)
        Case "Count >= 2":           StatisticValue = NumFmtI(CountGE2, 6)
        Case "Count Compare":        StatisticValue = NumFmtI(CountCompare, 6)
        Case "Percent 1"
          If NVALS > 0 Then StatisticValue = NumFmted(100 * Count1 / NVALS, 8, 2)
        Case "Percent 2"
          If NVALS > 0 Then StatisticValue = NumFmted(100 * Count2 / NVALS, 8, 2)
        Case "Percent >= 1"
          If NVALS > 0 Then StatisticValue = NumFmted(100 * CountGE1 / NVALS, 8, 2)
        Case "Percent >= 2"
          If NVALS > 0 Then StatisticValue = NumFmted(100 * CountGE2 / NVALS, 8, 2)
        Case "Mean 1":               StatisticValue = NumFmted(Mean1, 8, 2)
        Case "Mean 2":               StatisticValue = NumFmted(Mean2, 8, 2)
        Case "Geometric Mean 1":     StatisticValue = NumFmted(GeomMean1, 8, 2)
        Case "Geometric Mean 2":     StatisticValue = NumFmted(GeomMean2, 8, 2)
        Case "Std Deviation 1":      StatisticValue = NumFmted(SD1, 8, 2)
        Case "Std Deviation 2":      StatisticValue = NumFmted(SD2, 8, 2)
        Case Else
          If classIndex > nClasses Then ' more stats available here
            Select Case StatisticName
              Case "Correlation Coefficient"
                If Not Pass2Done Then GoSub ComputePass2
                                     StatisticValue = NumFmted(CorrelationCoefficient, 8, 2)
              Case "Coefficient of Determination"
                If Not Pass2Done Then GoSub ComputePass2
                                     StatisticValue = NumFmted(CorrelationCoefficient ^ 2, 8, 2)
              Case "Mean Error":     StatisticValue = NumFmted(MeanError, 9, 3)
              Case "Percent Mean Error"
                If Mean1 <> 0 Then
                                     StatisticValue = NumFmted(100 * (MeanError / Mean1), 9, 2) 'assumes observed is first timeser
                End If
              Case "Mean Absolute Error":  StatisticValue = NumFmted(MeanAbsoluteError, 9, 3)
              Case "Percent Mean Absolute Error"
                If Mean1 <> 0 Then
                                     StatisticValue = NumFmted(100 * (MeanAbsoluteError / Mean1), 9, 2) 'assumes observed is first timeser
                End If
              Case "RMS Error":      StatisticValue = NumFmted(RMSerror, 8, 2)
              'Case "Nash Sutcliffe"
              '  If Not Pass2Done Then GoSub ComputePass2
              '                       StatisticValue = NumFmted(NashSutcliffe, 8, 2)
              Case "Model Fit Efficiency (NS)"
                If Not Pass2Done Then GoSub ComputePass2
                                     StatisticValue = NumFmted(1 - NashSutcliffe, 8, 2)
            End Select
          End If
      End Select
      agd.TextMatrix(agdrow, agdcol) = StatisticValue
      agdcol = agdcol + 1
    Next
  Next
  
  agd.ColsSizeByContents
  
  SSTabCompare.Enabled = True
  Me.MousePointer = vbDefault

  Exit Sub
'---------------------------------------------------------------------------------------------

ComputePass2:

  CorrelationCoefficient = 0
  NashSutcliffe = 0
  
  'compute only where 'both' timeseries values are present.
  'ie cant include points where one timeseries has a value
  'but the other has a missing value
  Dim BothCount&, BothMean1 As Double, BothMean2 As Double
  Dim BothSum1 As Double, BothSum2 As Double
  Dim BothSD1 As Double, BothSD2 As Double
  Dim BothValuesPresent As Boolean
  BothCount = 0
  BothMean1 = 0
  BothMean2 = 0
  BothSum1 = 0
  BothSum2 = 0
  BothSD1 = 0
  BothSD2 = 0
  
  For valIndex = 1 To NVALS
    val2 = pTScoll(2).value(valIndex)
    val1 = pTScoll(1).value(valIndex)
    BothValuesPresent = True
    If checkmissing1 Then
      If Abs(val1 - missing1) < 0.01 Then
        'missing value, skip it
        BothValuesPresent = False
      Else
        If checkmissing2 Then
          If Abs(val2 - missing2) < 0.01 Then
            'missing value, skip it
            BothValuesPresent = False
          End If
        End If
      End If
    End If
    If BothValuesPresent And val2 >= ClassMin And val2 < ClassMax _
       And val1 >= ClassMin And val1 < ClassMax Then
      'compute means and standard deviations for timesteps where both values present
      BothSum1 = BothSum1 + val1
      BothSum2 = BothSum2 + val2
      BothSD1 = BothSD1 + val1 * val1
      BothSD2 = BothSD2 + val2 * val2
      BothCount = BothCount + 1
    End If
  Next
  If BothCount > 0 Then
    BothMean1 = BothSum1 / BothCount
    BothMean2 = BothSum2 / BothCount
    BothSD1 = (BothSD1 - BothSum1 * BothMean1)
    If BothCount > 1 Then BothSD1 = BothSD1 / (BothCount - 1)
    If BothSD1 > 0 Then BothSD1 = Sqr(BothSD1)
    BothSD2 = (BothSD2 - BothSum2 * BothMean2)
    If BothCount > 1 Then BothSD2 = BothSD2 / (BothCount - 1)
    If BothSD2 > 0 Then BothSD2 = Sqr(BothSD2)
  End If
  
  For valIndex = 1 To NVALS
    val2 = pTScoll(2).value(valIndex)
    val1 = pTScoll(1).value(valIndex)
    BothValuesPresent = True
    If checkmissing1 Then
      If Abs(val1 - missing1) < 0.01 Then
        'missing value, skip it
        BothValuesPresent = False
      Else
        If checkmissing2 Then
          If Abs(val2 - missing2) < 0.01 Then
            'missing value, skip it
            BothValuesPresent = False
          End If
        End If
      End If
    End If
    If BothValuesPresent And val2 >= ClassMin And val2 < ClassMax _
       And val1 >= ClassMin And val1 < ClassMax Then
      CorrelationCoefficient = CorrelationCoefficient + (val1 - BothMean1) * (val2 - BothMean2)
      NashSutcliffe = NashSutcliffe + (val2 - BothMean2) ^ 2   'observed in denominator
    End If
  Next
  If BothCount > 1 Then CorrelationCoefficient = CorrelationCoefficient / (BothCount - 1)
  If Abs(BothSD1 * BothSD2) > 0.0001 Then CorrelationCoefficient = CorrelationCoefficient / (BothSD1 * BothSD2)
  If NashSutcliffe > 0 Then NashSutcliffe = NashSutcliffeNumerator / NashSutcliffe
  
  Pass2Done = True
  Return
  
End Sub

Private Sub cmdClear_Click()
  Call CompInit
End Sub

Private Sub cmdClose_Click()
  Unload Me
End Sub

Private Sub cmdGet_Click()
    CDGet.flags = &H1806&
    CDGet.filter = "GenScn Compare Files (*.gco)|*.gco"
    CDGet.Filename = "*.gco"
    CDGet.DialogTitle = "GenScn Compare Get Specs File"
    On Error GoTo 10
    CDGet.CancelError = True
    CDGet.Action = 1
    Call GetSpecs(CDGet.Filename)
10 'continue here on cancel
End Sub

Private Sub cmdOutfile_Click()
  CDOutfile.flags = &H8806&
  CDOutfile.Filename = "compare.out"
  CDOutfile.DialogTitle = "GenScn Compare Output File"
  CDOutfile.Action = 2
End Sub

Private Sub cmdResults_Click()
  Dim cap$
  cap = "GenScn Compare Results"
  Call DispFile.OpenFile(OutFile, cap, frmGenScnCompare.Icon, False)
End Sub

Private Sub cmdCLimitsadd_Click()
  Dim newCLimits$, rval!, chgflg&, i&, ipos&
  newCLimits = ""
  If lstCLimits.ListIndex >= 0 Then
    newCLimits = lstCLimits.List(lstCLimits.ListIndex)
  Else
    If Len(txtCLimits.text) > 0 Then
      newCLimits = txtCLimits.text
      Call ChkTxtR("Class Limits", -10000000#, 10000000#, newCLimits, rval, chgflg)
      txtCLimits.text = ""
      If chgflg <> 1 Then
        newCLimits = ""
      Else
        'make sure not one of the above choices
        i = 0
        Do Until i = lstCLimits.ListCount
          If val(lstCLimits.List(i)) = val(newCLimits) Then
            'already in list, remove
            lstCLimits.RemoveItem i
            i = lstCLimits.ListCount
          Else
            i = i + 1
          End If
        Loop
        i = 0
        Do Until i = lstActCLimits.ListCount
          If val(lstActCLimits.List(i)) = val(newCLimits) Then
            'already in list, remove
            lstActCLimits.RemoveItem i
            i = lstActCLimits.ListCount
          Else
            i = i + 1
          End If
        Loop
      End If
    End If
  End If
  If Len(newCLimits) > 0 Then
    'make sure not already in active list
    i = 0
    Do Until i = lstActCLimits.ListCount
      If val(lstActCLimits.List(i)) = val(newCLimits) Then
        'already in list, remove
        lstActCLimits.RemoveItem i
        i = lstActCLimits.ListCount
      Else
        i = i + 1
      End If
    Loop
    'add this class limit to active list, figure where
    If lstActCLimits.ListCount < 1 Then
      ipos = 0
    Else
      'find where to put it in list
      ipos = -1
      i = 0
      Do Until ipos > -1 Or i = lstActCLimits.ListCount
        If val(lstActCLimits.List(i)) > val(newCLimits) Then
          ipos = i
        End If
        i = i + 1
      Loop
      If ipos = -1 Then
        ipos = lstActCLimits.ListCount
      End If
    End If
    lstActCLimits.AddItem newCLimits, ipos
  End If
  If lstCLimits.ListIndex >= 0 Then
    lstCLimits.RemoveItem lstCLimits.ListIndex
  End If
  If lstActCLimits.ListCount > 0 Then
    cmdAnalyze.Enabled = True
  Else
    cmdAnalyze.Enabled = False
  End If
End Sub

Private Sub cmdCLimitsaddall_Click()
  Dim newCLimits$, i&, ipos&
  Do Until lstCLimits.ListCount = 0
    newCLimits = lstCLimits.List(0)
    'add this class limit to active list, figure where
    If lstActCLimits.ListCount < 1 Then
      ipos = 0
    Else
      'make sure not one of the above choices
      i = 0
      Do Until i = lstActCLimits.ListCount
        If val(lstActCLimits.List(i)) = val(newCLimits) Then
          'already in list, remove
          lstActCLimits.RemoveItem i
          i = lstActCLimits.ListCount
        Else
          i = i + 1
        End If
      Loop
      'find where to put it in list
      ipos = -1
      i = 0
      Do Until ipos > -1 Or i = lstActCLimits.ListCount
        If val(lstActCLimits.List(i)) > val(newCLimits) Then
          ipos = i
        End If
        i = i + 1
      Loop
      If ipos = -1 Then
        ipos = lstActCLimits.ListCount
      End If
    End If
    lstActCLimits.AddItem newCLimits, ipos
    lstCLimits.RemoveItem 0
  Loop
  If lstActCLimits.ListCount > 0 Then
    cmdAnalyze.Enabled = True
  Else
    cmdAnalyze.Enabled = False
  End If
End Sub

Private Sub cmdCLimitsdelall_Click()
  Dim newCLimits$, i&, ipos&
  Do Until lstActCLimits.ListCount = 0
    newCLimits = lstActCLimits.List(0)
    'add this class limit to available list, figure where
    If lstCLimits.ListCount < 1 Then
      ipos = 0
    Else
      'find where to put it in list
      ipos = -1
      i = 0
      Do Until ipos > -1 Or i = lstCLimits.ListCount
        If val(lstCLimits.List(i)) > val(newCLimits) Then
          ipos = i
        End If
        i = i + 1
      Loop
      If ipos = -1 Then
        ipos = lstCLimits.ListCount
      End If
    End If
    lstCLimits.AddItem newCLimits, ipos
    lstActCLimits.RemoveItem 0
  Loop
  If lstActCLimits.ListCount > 0 Then
    cmdAnalyze.Enabled = True
  Else
    cmdAnalyze.Enabled = False
  End If
End Sub

Private Sub cmdCLimitsdrop_Click()
  Dim i&, ipos&
  If lstActCLimits.ListIndex > -1 Then
    'something is selected
    If lstCLimits.ListCount < 1 Then
      'put this class limit back to available list
      ipos = 0
    Else
      'figure out where to put it
      ipos = -1
      i = 0
      Do Until ipos > -1 Or i = lstCLimits.ListCount
        If val(lstCLimits.List(i)) > val(lstActCLimits.List(lstActCLimits.ListIndex)) Then
          ipos = i
        End If
        i = i + 1
      Loop
      If ipos = -1 Then
        ipos = lstCLimits.ListCount
      End If
    End If
    lstCLimits.AddItem lstActCLimits.List(lstActCLimits.ListIndex), ipos
    lstActCLimits.RemoveItem lstActCLimits.ListIndex
  End If
  If lstActCLimits.ListCount > 0 Then
    cmdAnalyze.Enabled = True
  Else
    cmdAnalyze.Enabled = False
  End If
End Sub

Private Sub cmdSave_Click()
    CDSave.flags = &H8806&
    CDSave.filter = "GenScn Compare Files (*.gco)|*.gco"
    CDSave.Filename = "*.gco"
    CDSave.DialogTitle = "GenScn Compare Save Specs File"
    On Error GoTo 10
    CDSave.CancelError = True
    CDSave.Action = 2
    Call PutSpecs(CDSave.Filename)
10 'continue here on cancel
End Sub

Private Sub cmdSaveResults_Click()
  SSTabCompare.Tab = 2
  SSTabCompare_Click 2
  agd.SaveGridInteractive
End Sub

Private Sub cmdSwap_Click()

  Dim tmpTS As Object
  Dim tmpMissing As String
  
  Set tmpTS = pTScoll(1)
  pTScoll.Remove 1
  pTScoll.Add tmpTS
  
  tmpMissing = txtMissing1.text
  txtMissing1.text = txtMissing2.text
  txtMissing2.text = tmpMissing
  
  Showcoll pTScoll

End Sub

Private Sub Form_Load()
  cmdResults.Enabled = False
  Call GetSpecs(p.StatusFilePath & "\default.gco")
  SSTabCompare.Tab = 0
  SSTabCompare_Click 0
  agd.ColTitle(0) = "Class"
End Sub

Private Sub Form_Resize()
  Dim w As Long, h As Long
  w = Me.ScaleWidth
  h = Me.ScaleHeight
  If w > 1000 Then
    SSTabCompare.Width = w - 252
    txtTitle.Width = SSTabCompare.Width
    SelectResults.Width = SSTabCompare.Width - 240
    agd.Width = SelectResults.Width
  End If
  fraButtons.Top = h - fraButtons.Height - 180
  If fraButtons.Top - SSTabCompare.Top > 1000 Then
    SSTabCompare.Height = fraButtons.Top - SSTabCompare.Top - 108
    SelectResults.Height = SSTabCompare.Height - 480
    agd.Height = SelectResults.Height
  End If

'  If h > 4200 Then
'    lstCLimits.Height = h - 4140
'    lstActCLimits.Height = lstCLimits.Height
'    txtCLimits.Top = lstCLimits.Top + lstCLimits.Height + 40
'  End If
End Sub

Private Sub Form_Unload(Cancel As Integer)
  Call PutSpecs(p.StatusFilePath & "\default.gco")
End Sub

Private Sub optCLimits_Click(index As Integer)
    Call SetCLimitsDef
End Sub

Private Sub SSTabCompare_Click(PreviousTab As Integer)
  Select Case SSTabCompare.Tab
    Case 0:
      fraTab(0).Visible = True
      SelectResults.Visible = False
      agd.Visible = False
    Case 1:
      fraTab(0).Visible = False
      SelectResults.Visible = True
      agd.Visible = False
    Case 2:
      fraTab(0).Visible = False
      SelectResults.Visible = False
      agd.Visible = True
      Recalculate
  End Select
End Sub

Private Sub txtLower_GotFocus()
  oldlower = txtLower.text
End Sub

Private Sub txtLower_LostFocus()
  Dim newlower$, chgflg&, rval!
  newlower = txtLower.text
  Call ChkTxtR("Class Limits", 0.000001, 1000#, newlower, rval, chgflg)
  If chgflg <> 1 Then
    txtLower.text = oldlower
  End If
  If chgflg = 1 Then
    Call SetCLimitsDef
  End If
End Sub


Private Sub txtCLimits_Click()
  lstCLimits.ListIndex = -1
  lstActCLimits.ListIndex = -1
End Sub


Private Sub txtMissing1_GotFocus()
  oldmiss1 = txtMissing1.text
End Sub

Private Sub txtMissing1_LostFocus()
  Dim newmiss1$, chgflg&, rval!
  newmiss1 = txtMissing1.text
  Call ChkTxtR("Missing Value", -10000000#, 10000000#, newmiss1, rval, chgflg)
  If chgflg <> 1 Then
    txtMissing1.text = oldmiss1
  End If
End Sub

Private Sub txtMissing2_GotFocus()
  oldmiss2 = txtMissing2.text
End Sub

Private Sub txtMissing2_LostFocus()
  Dim newmiss2$, chgflg&, rval!
  newmiss2 = txtMissing2.text
  Call ChkTxtR("Missing Value", -10000000#, 10000000#, newmiss2, rval, chgflg)
  If chgflg <> 1 Then
    txtMissing2.text = oldmiss2
  End If
End Sub

Private Sub txtUpper_GotFocus()
  oldupper = txtUpper.text
End Sub

Private Sub txtUpper_LostFocus()
  Dim newupper$, chgflg&, rval!
  newupper = txtUpper.text
  Call ChkTxtR("Class Limits", 1#, 10000000#, newupper, rval, chgflg)
  If chgflg <> 1 Then
    txtUpper.text = oldupper
  End If
  If chgflg = 1 Then
    Call SetCLimitsDef
  End If
End Sub

