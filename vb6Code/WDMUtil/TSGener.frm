VERSION 5.00
Begin VB.Form frmTSGener 
   BorderStyle     =   3  'Fixed Dialog
   Caption         =   "Generate Timeseries"
   ClientHeight    =   6864
   ClientLeft      =   2328
   ClientTop       =   936
   ClientWidth     =   6888
   BeginProperty Font 
      Name            =   "MS Sans Serif"
      Size            =   7.8
      Charset         =   0
      Weight          =   700
      Underline       =   0   'False
      Italic          =   0   'False
      Strikethrough   =   0   'False
   EndProperty
   HelpContextID   =   105
   Icon            =   "TSGener.frx":0000
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   PaletteMode     =   1  'UseZOrder
   ScaleHeight     =   6864
   ScaleWidth      =   6888
   Begin VB.Frame fraDumGenerDates 
      Caption         =   "Dates"
      Height          =   1332
      Left            =   120
      TabIndex        =   64
      Top             =   4920
      Width           =   6612
      Begin VB.Label lblDumGenerDates 
         Caption         =   "Label1"
         Height          =   252
         Left            =   120
         TabIndex        =   65
         Top             =   360
         Width           =   6372
      End
   End
   Begin VB.CommandButton cmdClose 
      Cancel          =   -1  'True
      Caption         =   "Close"
      Height          =   372
      Left            =   3960
      TabIndex        =   47
      Top             =   6360
      Width           =   972
   End
   Begin VB.CommandButton cmdPerform 
      Caption         =   "&Perform Operation"
      Default         =   -1  'True
      Height          =   372
      Left            =   1800
      TabIndex        =   46
      Top             =   6360
      Width           =   1812
   End
   Begin VB.Frame fraFunction 
      Caption         =   "Function"
      Height          =   2052
      Left            =   120
      TabIndex        =   14
      Top             =   1080
      Width           =   6612
      Begin VB.OptionButton optManFill 
         Caption         =   "Fill New Data Set, Use this value for all"
         Height          =   252
         Index           =   2
         Left            =   480
         TabIndex        =   73
         Top             =   1080
         Width           =   3852
      End
      Begin ATCoCtl.ATCoText atxValue 
         Height          =   252
         Left            =   4440
         TabIndex        =   71
         Top             =   840
         Width           =   852
         _ExtentX        =   1503
         _ExtentY        =   445
         InsideLimitsBackground=   16777215
         OutsideHardLimitBackground=   8421631
         OutsideSoftLimitBackground=   8454143
         HardMax         =   -999
         HardMin         =   -999
         SoftMax         =   -999
         SoftMin         =   -999
         MaxWidth        =   -999
         Alignment       =   1
         DataType        =   2
         DefaultValue    =   -999
         Value           =   "-999"
         Enabled         =   -1  'True
      End
      Begin ATCoCtl.ATCoText atxYear 
         Height          =   252
         Left            =   2160
         TabIndex        =   68
         Top             =   360
         Width           =   732
         _ExtentX        =   1291
         _ExtentY        =   445
         InsideLimitsBackground=   16777215
         OutsideHardLimitBackground=   8421631
         OutsideSoftLimitBackground=   8454143
         HardMax         =   9999
         HardMin         =   1800
         SoftMax         =   9999
         SoftMin         =   1800
         MaxWidth        =   -999
         Alignment       =   1
         DataType        =   1
         DefaultValue    =   -999
         Value           =   "-999"
         Enabled         =   -1  'True
      End
      Begin ATCoCtl.ATCoText atxConst 
         Height          =   252
         Index           =   0
         Left            =   3000
         TabIndex        =   66
         Top             =   1680
         Width           =   852
         _ExtentX        =   1503
         _ExtentY        =   445
         InsideLimitsBackground=   16777215
         OutsideHardLimitBackground=   8421631
         OutsideSoftLimitBackground=   8454143
         HardMax         =   -999
         HardMin         =   -999
         SoftMax         =   -999
         SoftMin         =   -999
         MaxWidth        =   -999
         Alignment       =   1
         DataType        =   2
         DefaultValue    =   0
         Value           =   "0"
         Enabled         =   -1  'True
      End
      Begin VB.OptionButton optManFill 
         Caption         =   "Append values, Use this value for all"
         Height          =   252
         Index           =   1
         Left            =   480
         TabIndex        =   49
         Top             =   840
         Width           =   3852
      End
      Begin VB.OptionButton optManFill 
         Caption         =   "Append values, Interpolate to this value"
         Height          =   252
         Index           =   0
         Left            =   480
         TabIndex        =   48
         Top             =   600
         Value           =   -1  'True
         Width           =   3852
      End
      Begin VB.CommandButton cmdInputFile 
         Caption         =   "Input File"
         Height          =   252
         Left            =   120
         TabIndex        =   40
         Top             =   1680
         Width           =   972
      End
      Begin VB.OptionButton optFunction 
         Caption         =   "Table"
         Height          =   252
         Index           =   23
         Left            =   4920
         TabIndex        =   38
         Top             =   960
         Width           =   972
      End
      Begin VB.OptionButton optFunction 
         Caption         =   "Line"
         Height          =   252
         Index           =   22
         Left            =   4920
         TabIndex        =   37
         Top             =   720
         Width           =   972
      End
      Begin VB.OptionButton optFunction 
         Caption         =   "SigF"
         Height          =   252
         Index           =   21
         Left            =   4920
         TabIndex        =   36
         Top             =   480
         Width           =   972
      End
      Begin VB.OptionButton optFunction 
         Caption         =   "Sum"
         Height          =   252
         Index           =   20
         Left            =   4920
         TabIndex        =   35
         Top             =   240
         Width           =   972
      End
      Begin VB.OptionButton optFunction 
         Caption         =   "Max"
         Height          =   252
         Index           =   19
         Left            =   3960
         TabIndex        =   34
         Top             =   960
         Width           =   972
      End
      Begin VB.OptionButton optFunction 
         Caption         =   "Min"
         Height          =   252
         Index           =   18
         Left            =   3960
         TabIndex        =   33
         Top             =   720
         Width           =   972
      End
      Begin VB.OptionButton optFunction 
         Caption         =   "Z+"
         Height          =   252
         Index           =   17
         Left            =   3960
         TabIndex        =   32
         Top             =   480
         Width           =   972
      End
      Begin VB.OptionButton optFunction 
         Caption         =   "Z-"
         Height          =   252
         Index           =   16
         Left            =   3960
         TabIndex        =   31
         Top             =   240
         Width           =   972
      End
      Begin VB.OptionButton optFunction 
         Caption         =   "Abs"
         Height          =   252
         Index           =   15
         Left            =   3000
         TabIndex        =   30
         Top             =   960
         Width           =   972
      End
      Begin VB.OptionButton optFunction 
         Caption         =   "Lge"
         Height          =   252
         Index           =   14
         Left            =   3000
         TabIndex        =   29
         Top             =   720
         Width           =   972
      End
      Begin VB.OptionButton optFunction 
         Caption         =   "Lg10"
         Height          =   252
         Index           =   13
         Left            =   3000
         TabIndex        =   28
         Top             =   480
         Width           =   972
      End
      Begin VB.OptionButton optFunction 
         Caption         =   "E**"
         Height          =   252
         Index           =   12
         Left            =   3000
         TabIndex        =   27
         Top             =   240
         Width           =   972
      End
      Begin VB.OptionButton optFunction 
         Caption         =   "Pow"
         Height          =   252
         Index           =   11
         Left            =   2040
         TabIndex        =   26
         Top             =   960
         Width           =   972
      End
      Begin VB.OptionButton optFunction 
         Caption         =   "C**"
         Height          =   252
         Index           =   10
         Left            =   2040
         TabIndex        =   25
         Top             =   720
         Width           =   972
      End
      Begin VB.OptionButton optFunction 
         Caption         =   "**C"
         Height          =   252
         Index           =   9
         Left            =   2040
         TabIndex        =   24
         Top             =   480
         Width           =   972
      End
      Begin VB.OptionButton optFunction 
         Caption         =   "Wght"
         Height          =   252
         Index           =   8
         Left            =   2040
         TabIndex        =   23
         Top             =   240
         Width           =   972
      End
      Begin VB.OptionButton optFunction 
         Caption         =   "Mean"
         Height          =   252
         Index           =   7
         Left            =   1080
         TabIndex        =   22
         Top             =   960
         Width           =   972
      End
      Begin VB.OptionButton optFunction 
         Caption         =   "Div"
         Height          =   252
         Index           =   6
         Left            =   1080
         TabIndex        =   21
         Top             =   720
         Width           =   972
      End
      Begin VB.OptionButton optFunction 
         Caption         =   "Mult"
         Height          =   252
         Index           =   5
         Left            =   1080
         TabIndex        =   20
         Top             =   480
         Width           =   972
      End
      Begin VB.OptionButton optFunction 
         Caption         =   "Sub"
         Height          =   252
         Index           =   4
         Left            =   1080
         TabIndex        =   19
         Top             =   240
         Width           =   972
      End
      Begin VB.OptionButton optFunction 
         Caption         =   "Add"
         Height          =   252
         Index           =   3
         Left            =   120
         TabIndex        =   18
         Top             =   960
         Width           =   972
      End
      Begin VB.OptionButton optFunction 
         Caption         =   "*C"
         Height          =   252
         Index           =   2
         Left            =   120
         TabIndex        =   17
         Top             =   720
         Width           =   972
      End
      Begin VB.OptionButton optFunction 
         Caption         =   "+C"
         Height          =   252
         Index           =   1
         Left            =   120
         TabIndex        =   16
         Top             =   480
         Width           =   972
      End
      Begin VB.OptionButton optFunction 
         Caption         =   "None"
         Height          =   252
         Index           =   0
         Left            =   120
         TabIndex        =   15
         Top             =   240
         Width           =   972
      End
      Begin MSComDlg.CommonDialog CDInfile 
         Left            =   6120
         Top             =   1560
         _ExtentX        =   699
         _ExtentY        =   699
         _Version        =   393216
         FontSize        =   1.85008e-37
      End
      Begin ATCoCtl.ATCoText atxConst 
         Height          =   252
         Index           =   1
         Left            =   4680
         TabIndex        =   67
         Top             =   1680
         Width           =   852
         _ExtentX        =   1503
         _ExtentY        =   445
         InsideLimitsBackground=   16777215
         OutsideHardLimitBackground=   8421631
         OutsideSoftLimitBackground=   8454143
         HardMax         =   -999
         HardMin         =   -999
         SoftMax         =   -999
         SoftMin         =   -999
         MaxWidth        =   -999
         Alignment       =   1
         DataType        =   2
         DefaultValue    =   "0"
         Value           =   "0"
         Enabled         =   -1  'True
      End
      Begin ATCoCtl.ATCoText atxMonth 
         Height          =   252
         Left            =   3600
         TabIndex        =   69
         Top             =   360
         Width           =   732
         _ExtentX        =   1291
         _ExtentY        =   445
         InsideLimitsBackground=   16777215
         OutsideHardLimitBackground=   8421631
         OutsideSoftLimitBackground=   8454143
         HardMax         =   12
         HardMin         =   1
         SoftMax         =   12
         SoftMin         =   1
         MaxWidth        =   -999
         Alignment       =   1
         DataType        =   1
         DefaultValue    =   -999
         Value           =   "-999"
         Enabled         =   -1  'True
      End
      Begin ATCoCtl.ATCoText atxDay 
         Height          =   252
         Left            =   4920
         TabIndex        =   70
         Top             =   360
         Width           =   732
         _ExtentX        =   1291
         _ExtentY        =   445
         InsideLimitsBackground=   16777215
         OutsideHardLimitBackground=   8421631
         OutsideSoftLimitBackground=   8454143
         HardMax         =   31
         HardMin         =   1
         SoftMax         =   31
         SoftMin         =   1
         MaxWidth        =   -999
         Alignment       =   1
         DataType        =   1
         DefaultValue    =   -999
         Value           =   "-999"
         Enabled         =   -1  'True
      End
      Begin VB.Label lblDay 
         Caption         =   "Day:"
         Height          =   252
         Left            =   4440
         TabIndex        =   53
         Top             =   360
         Width           =   492
      End
      Begin VB.Label lblMonth 
         Caption         =   "Month:"
         Height          =   252
         Left            =   3000
         TabIndex        =   52
         Top             =   360
         Width           =   612
      End
      Begin VB.Label lblYear 
         Caption         =   "Output Starting Year:"
         Height          =   252
         Left            =   360
         TabIndex        =   51
         Top             =   360
         Width           =   1812
      End
      Begin VB.Label lblValue 
         Caption         =   "Value:"
         Height          =   252
         Left            =   3600
         TabIndex        =   50
         Top             =   720
         Width           =   612
      End
      Begin VB.Label lblSigFigs 
         Caption         =   "Sig Figs:"
         Height          =   252
         Left            =   1920
         TabIndex        =   45
         Top             =   1680
         Visible         =   0   'False
         Width           =   852
      End
      Begin VB.Label lblExponent 
         Caption         =   "Exponent:"
         Height          =   252
         Left            =   1800
         TabIndex        =   44
         Top             =   1680
         Visible         =   0   'False
         Width           =   852
      End
      Begin VB.Label lblC2 
         Caption         =   "C2"
         Height          =   252
         Left            =   4200
         TabIndex        =   43
         Top             =   1680
         Width           =   252
      End
      Begin VB.Label lblC1 
         Caption         =   "C1"
         Height          =   252
         Left            =   2400
         TabIndex        =   42
         Top             =   1680
         Width           =   252
      End
      Begin VB.Label lblConstants 
         Caption         =   "Constants:"
         Height          =   252
         Left            =   1320
         TabIndex        =   41
         Top             =   1680
         Width           =   972
      End
      Begin VB.Label lblFunction 
         Height          =   252
         Left            =   240
         TabIndex        =   39
         Top             =   1320
         Width           =   6132
      End
   End
   Begin VB.Frame fraOperation 
      Caption         =   "Operation"
      Height          =   852
      Left            =   120
      TabIndex        =   13
      Top             =   120
      Width           =   6612
      Begin VB.OptionButton optOperation 
         Caption         =   "&Manual"
         Height          =   252
         HelpContextID   =   108
         Index           =   2
         Left            =   4680
         TabIndex        =   2
         Top             =   360
         Width           =   1092
      End
      Begin VB.OptionButton optOperation 
         Caption         =   "&Transform"
         Height          =   252
         HelpContextID   =   107
         Index           =   1
         Left            =   2640
         TabIndex        =   1
         Top             =   360
         Width           =   1212
      End
      Begin VB.OptionButton optOperation 
         Caption         =   "&Compute"
         Height          =   252
         HelpContextID   =   106
         Index           =   0
         Left            =   720
         TabIndex        =   0
         Top             =   360
         Width           =   1092
      End
   End
   Begin VB.Frame fraTimeseries 
      Caption         =   "Timeseries"
      Height          =   1572
      Left            =   120
      TabIndex        =   6
      Top             =   3240
      Width           =   6612
      Begin ATCoCtl.ATCoText atxOutDsn 
         Height          =   252
         Left            =   5520
         TabIndex        =   72
         ToolTipText     =   "ID number for output data set"
         Top             =   480
         Width           =   732
         _ExtentX        =   1291
         _ExtentY        =   445
         InsideLimitsBackground=   16777215
         OutsideHardLimitBackground=   8421631
         OutsideSoftLimitBackground=   8454143
         HardMax         =   9999
         HardMin         =   1
         SoftMax         =   9999
         SoftMin         =   1
         MaxWidth        =   -999
         Alignment       =   1
         DataType        =   1
         DefaultValue    =   -999
         Value           =   "-999"
         Enabled         =   -1  'True
      End
      Begin VB.ComboBox cboInSen 
         Height          =   288
         Index           =   0
         Left            =   3960
         TabIndex        =   62
         Text            =   "Combo1"
         ToolTipText     =   "Available Scenarios for Input Timeseries 1"
         Top             =   840
         Width           =   1332
      End
      Begin VB.ComboBox cboInLoc 
         Height          =   288
         Index           =   0
         Left            =   2520
         TabIndex        =   61
         Text            =   "Combo1"
         ToolTipText     =   "Available Locations for Input Timeseries 1"
         Top             =   840
         Width           =   1332
      End
      Begin VB.ComboBox cboInCon 
         Height          =   288
         Index           =   0
         Left            =   1080
         TabIndex        =   60
         Text            =   "Combo1"
         ToolTipText     =   "Available Constituents for Input Timeseries 1"
         Top             =   840
         Width           =   1332
      End
      Begin VB.ComboBox cboInSen 
         Height          =   288
         Index           =   1
         Left            =   3960
         TabIndex        =   59
         Text            =   "Combo1"
         ToolTipText     =   "Available Locations for Input Timeseries 2"
         Top             =   1200
         Visible         =   0   'False
         Width           =   1332
      End
      Begin VB.ComboBox cboInLoc 
         Height          =   288
         Index           =   1
         Left            =   2520
         TabIndex        =   58
         Text            =   "Combo1"
         ToolTipText     =   "Available Locations for Input Timeseries 2"
         Top             =   1200
         Visible         =   0   'False
         Width           =   1332
      End
      Begin VB.ComboBox cboInCon 
         Height          =   288
         Index           =   1
         Left            =   1080
         TabIndex        =   57
         Text            =   "Combo1"
         ToolTipText     =   "Available Constituents for Input Timeseries 2"
         Top             =   1200
         Visible         =   0   'False
         Width           =   1332
      End
      Begin VB.ComboBox cboInDsn 
         Height          =   288
         Index           =   0
         Left            =   5520
         TabIndex        =   56
         Text            =   "Combo1"
         ToolTipText     =   "Available Data-set Numbers for Input Timeseries 1"
         Top             =   840
         Width           =   732
      End
      Begin VB.ComboBox cboInDsn 
         Height          =   288
         Index           =   1
         Left            =   5520
         TabIndex        =   55
         Text            =   "Combo1"
         ToolTipText     =   "Available Data-set Numbers for Input Timeseries 2"
         Top             =   1200
         Visible         =   0   'False
         Width           =   732
      End
      Begin VB.TextBox txtConsOut 
         Height          =   288
         Left            =   1080
         TabIndex        =   5
         ToolTipText     =   "Constituent name for output data set"
         Top             =   480
         Width           =   1332
      End
      Begin VB.TextBox txtLocOut 
         Height          =   288
         Left            =   2520
         TabIndex        =   4
         ToolTipText     =   "Location name for output data set"
         Top             =   480
         Width           =   1332
      End
      Begin VB.TextBox txtScenOut 
         Height          =   288
         Left            =   3960
         TabIndex        =   3
         ToolTipText     =   "Scenario name for output data set"
         Top             =   480
         Width           =   1332
      End
      Begin VB.Label lblInput 
         Caption         =   "Input 2:"
         Height          =   252
         Index           =   1
         Left            =   120
         TabIndex        =   63
         Top             =   1200
         Width           =   732
      End
      Begin VB.Label lblConstit 
         Caption         =   "Constituent"
         Height          =   252
         Left            =   1080
         TabIndex        =   12
         Top             =   240
         Width           =   972
      End
      Begin VB.Label lblLocation 
         Caption         =   "Location"
         Height          =   252
         Left            =   2520
         TabIndex        =   11
         Top             =   240
         Width           =   972
      End
      Begin VB.Label lblScenario 
         Caption         =   "Scenario"
         Height          =   252
         Left            =   3960
         TabIndex        =   10
         Top             =   240
         Width           =   972
      End
      Begin VB.Label lblDsn 
         Caption         =   "Dsn"
         Height          =   252
         Left            =   5520
         TabIndex        =   9
         Top             =   240
         Width           =   372
      End
      Begin VB.Label lblInput 
         Caption         =   "Input 1:"
         Height          =   255
         Index           =   0
         Left            =   120
         TabIndex        =   8
         Top             =   840
         Width           =   735
      End
      Begin VB.Label lblOutput 
         Caption         =   "Output:"
         Height          =   252
         Left            =   120
         TabIndex        =   7
         Top             =   480
         Width           =   612
      End
   End
   Begin ATCoCtl.ATCoDate ctlGenerDate 
      Height          =   1356
      Left            =   120
      TabIndex        =   54
      Top             =   4920
      Width           =   6612
      _ExtentX        =   11663
      _ExtentY        =   2392
      TUnit           =   4
      TAggr           =   1
      TStep           =   1
      CurrE           =   35064
      CurrS           =   33970
      LimtE           =   35795
      LimtS           =   33239
      DispL           =   1
      LabelCurrentRange=   "Current"
      TstepVisible    =   -1  'True
   End
End
Attribute VB_Name = "frmTSGener"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
'Copyright 2000 by AQUA TERRA Consultants
Dim SLCDLock$(5), lTs As Collection, OpType&
Dim oscen$, oloc$, ocons$, osdat&(6), oedat&(6), ogsiz&
Dim csdate&(6), cedate&(6), cts&, ctu&
Dim nin&, fnct&, fillval!, mants&

Sub GenerInit()
  Dim i&
  'default to compute operation
  optOperation(0) = True
  'set compute defaults for function frame
  optFunction(0) = True
  For i = 0 To 23
    optFunction(i).Visible = True
  Next i
  cmdInputFile.Visible = False
  atxConst(0).Visible = False
  atxConst(1).Visible = False
  lblC1.Visible = False
  lblC2.Visible = False
  lblSigFigs.Visible = False
  lblConstants.Visible = False
  lblExponent.Visible = False
  lblFunction.Visible = True
  lblFunction.Caption = ""
  'set compute defaults for timeseries frame
  lblInput(0).Visible = False
  lblInput(1).Visible = False
  txtScenOut.Text = ""
  txtLocOut.Text = ""
  txtConsOut.Text = ""
  'hide manual fields
  lblValue.Visible = False
  atxValue.Visible = False
  For i = 0 To optManFill.Count - 1
    optManFill(i).Visible = False
  Next i
  'hide transformation fields
  lblYear.Visible = False
  lblMonth.Visible = False
  lblDay.Visible = False
  atxYear.Visible = False
  atxMonth.Visible = False
  atxDay.Visible = False
End Sub

Private Sub atxValue_CommitChange()

  fillval = atxValue.Value

End Sub

Private Sub cmdClose_Click()
  Unload frmTSGener
End Sub


Private Sub cmdInputFile_Click()
  CDInfile.flags = &H1804&
  CDInfile.Filename = "gener.inp"
  CDInfile.DialogTitle = "Timeseries Generate Compute Input File"
  CDInfile.Action = 1
End Sub

Private Sub cmdPerform_Click()
  Dim i&, j&, dsn&(3), retc&, EDate&(6), SDate&(6), dates&(18)
  Dim interp&, mantu&, dexist&, crecod&
  Dim crtflg&, cmpflg&, flag1&, addedDsn
  Dim c1#, c2#, JSdt#, JEdt#, DateDiff#
  Dim lscen$, lcons$, llocn$, lErrDesc$
  Dim outdsn&, failfg As Boolean
  Dim DataVals!(), DateVals#(), nAppend&, IntrpInc!
  Dim TsDate As ATCclsTserDate, TsDateSum As ATTimSerDateSummary
  Dim InTs1 As ATCclsTserData, InTs2 As ATCclsTserData
  Dim GenTs As ATCclsTserData, lTsFile As ATCclsTserFile

  addedDsn = False
  MousePointer = vbHourglass
  If Len(txtConsOut.Text) = 0 And txtConsOut.Enabled Then
    'no constituent name specified
    MsgBox "No Constituent name has been specified." & "Enter a uniqued combination of Scenario, Location, and Constituent to identify this data set.", 48, "Timeseries Generate Problem"
    txtConsOut.SetFocus
    failfg = True
  End If
  If Len(txtLocOut.Text) = 0 And txtLocOut.Enabled And _
     Not failfg Then 'no location name specified
    MsgBox "No Location name has been specified." & "Enter a uniqued combination of Scenario, Location, and Constituent to identify this data set.", 48, "Timeseries Generate Problem"
    txtLocOut.SetFocus
    failfg = True
  End If
  If Len(txtScenOut.Text) = 0 And txtScenOut.Enabled And _
     Not failfg Then 'no scenario name specified
    MsgBox "No Scenario name has been specified." & "Enter a uniqued combination of Scenario, Location, and Constituent to identify this data set.", 48, "Timeseries Generate Problem"
    txtScenOut.SetFocus
    failfg = True
  End If
  If atxOutDsn.Value > 0 And atxOutDsn.Value < 10000 And _
     atxOutDsn.Enabled Then
    outdsn = atxOutDsn.Value
  ElseIf Not failfg And atxOutDsn.Enabled Then 'invalid output data-set number
    MsgBox "A valid output data-set number has not been specified." & vbCrLf & "Enter the number of a data set in the DSN field.", 48, "Timeseries Generate Problem"
    atxOutDsn.SetFocus
    failfg = True
  End If
  If cboInDsn(0).Visible And Not failfg Then 'get the data
    SDate(0) = Year(ctlGenerDate.CurrS)
    SDate(1) = Month(ctlGenerDate.CurrS)
    SDate(2) = Day(ctlGenerDate.CurrS)
    EDate(0) = Year(ctlGenerDate.CurrE)
    EDate(1) = Month(ctlGenerDate.CurrE)
    EDate(2) = Day(ctlGenerDate.CurrE)
    EDate(3) = 24
    JSdt = Date2J(SDate())
    JEdt = Date2J(EDate())
    Set InTs1 = New ATCclsTserData
    Set InTs2 = New ATCclsTserData
    i = 0
    Do While i < nin
      If Not IsNumeric(cboInDsn(i).Text) Then 'unique dataset not specified
        MsgBox "A unique data set has not been specified for " & lblInput(i).Caption & "." & vbCrLf & _
               "Make more detailed selections from the Scenario, Location, or Constituent lists or " & vbCrLf & _
               "Select a specific data-set number from the Dsn list.", 48, "Timeseries Generate Problem"
        failfg = True
        cboInDsn(i).SetFocus
        i = nin
      Else
        lscen = cboInSen(i).Text
        llocn = cboInLoc(i).Text
        lcons = cboInCon(i).Text
        Call FindTimSer(lscen, llocn, lcons, lTs)
        For j = 1 To lTs.Count 'this will usually just be one
          If lTs(j).Header.id = CLng(cboInDsn(i).Text) Then
            If i = 0 Then
              If optOperation(2) Then 'append to whole tser
                Set InTs1 = lTs(j)
              Else
                Set InTs1 = lTs(j).SubSetByDate(JSdt, JEdt)
              End If
            Else
              Set InTs2 = lTs(j).SubSetByDate(JSdt, JEdt)
            End If
          End If
        Next j
        If (i = 0 And InTs1.Header.id <= 0) Or _
           (i = 1 And InTs2.Header.id <= 0) Then
          MsgBox "Could not find data set specified for " & lblInput(i).Caption & ".", 48, "Timeseries Generate Problem"
          failfg = True
        End If
        i = i + 1
      End If
    Loop
  End If
  If Not failfg Then
    If optOperation(0) = True Then 'compute operations
      If atxConst(0).Visible Then
        If IsNumeric(atxConst(0).Value) Then
          c1 = atxConst(0).Value
        Else
          MsgBox "A valid constant value has not been entered." & vbCrLf & "Enter a valid numeric value for this constant.", vbExclamation, "Timeseries Generate Problem"
          atxConst(0).SetFocus
          failfg = True
        End If
      End If
      If atxConst(1).Visible And Not failfg Then
        If IsNumeric(atxConst(1).Value) Then
          c2 = atxConst(1).Value
        Else
          MsgBox "A valid constant value has not been entered." & vbCrLf & "Enter a valid numeric value for this constant.", vbExclamation, "Timeseries Generate Problem"
          atxConst(1).SetFocus
          failfg = True
        End If
      End If
      If Not failfg Then
        Set GenTs = InTs1.DoMath(OpType, InTs2, c1)
      End If
    ElseIf optOperation(1) = True Then
      'do transform operation
      If atxYear.Value < 0 Or atxYear.Value > 9999 Or _
         atxMonth.Value < 1 Or atxMonth.Value > 12 Or _
         atxDay.Value < 1 Or atxDay.Value > 31 Then
        MsgBox "Invalid entry in output date specification.", vbExclamation, "Timeseries Generate Problem"
        atxYear.SetFocus
      Else 'make copy of input TSer
        Set GenTs = InTs1.Copy
        SDate(0) = atxYear.Value
        SDate(1) = atxMonth.Value
        SDate(2) = atxDay.Value
        SDate(3) = 0
        SDate(4) = 0
        SDate(5) = 0
        JSdt = Date2J(SDate)
        DateDiff = JSdt - InTs1.dates.Summary.SJDay
        'make a copy of dates, then modify them
        Set TsDate = GenTs.dates.Copy
        TsDateSum = TsDate.Summary
        TsDateSum.SJDay = TsDateSum.SJDay + DateDiff
        TsDateSum.EJDay = TsDateSum.EJDay + DateDiff
        If Not TsDateSum.CIntvl Then
          ReDim DateVals(TsDateSum.NVALS)
          For i = 1 To TsDateSum.NVALS
            DateVals(i) = TsDate.Value(i) + DateDiff
          Next i
        Else
          ReDim DateVals(0)
        End If
        TsDate.Summary = TsDateSum
        TsDate.Values = DateVals
        Set GenTs.dates = Nothing 'try to clean out old dates
        Set GenTs.dates = TsDate
      End If
    ElseIf optOperation(2) = True Then
      'do manual manipulation operation
      c1 = atxValue.Value
      If optManFill(2).Value Then 'fill new data set
        Set GenTs = New ATCclsTserData
        Set TsDate = New ATCclsTserDate
        TsDateSum = TsDate.Summary
        SDate(0) = Year(ctlGenerDate.CurrS)
        SDate(1) = Month(ctlGenerDate.CurrS)
        SDate(2) = Day(ctlGenerDate.CurrS)
        EDate(0) = Year(ctlGenerDate.CurrE)
        EDate(1) = Month(ctlGenerDate.CurrE)
        EDate(2) = Day(ctlGenerDate.CurrE)
        EDate(3) = 24
        With TsDateSum
          .CIntvl = True
          .SJDay = Date2J(SDate())
          .EJDay = Date2J(EDate())
          .ts = ctlGenerDate.TSTEP
          .Tu = ctlGenerDate.TUnit
          Select Case .Tu
            Case TUSecond:  .Intvl = .ts / 86400#
            Case TUMinute:  .Intvl = .ts / 1440#
            Case TUHour:    .Intvl = .ts / 24#
            Case TUDay:     .Intvl = .ts
            Case TUMonth:   .Intvl = .ts * 30
            Case TUYear:    .Intvl = .ts * 365.25
            Case TUCentury: .Intvl = .ts * 36525
            Case Else: .Tu = TUCentury: .Intvl = 36525 'should not happen
          End Select
          .NVALS = (.EJDay - .SJDay) / .Intvl
        End With
        ReDim DataVals(TsDateSum.NVALS)
        For i = 1 To TsDateSum.NVALS
          DataVals(i) = c1
        Next i
        TsDate.Summary = TsDateSum
        Set GenTs.dates = TsDate
        GenTs.Values = DataVals
      Else 'append to existing data set
        Set GenTs = InTs1
        Set TsDate = GenTs.dates.Copy
        TsDateSum = TsDate.Summary
        EDate(0) = Year(ctlGenerDate.CurrE)
        EDate(1) = Month(ctlGenerDate.CurrE)
        EDate(2) = Day(ctlGenerDate.CurrE)
        JEdt = Date2J(EDate)
        nAppend = (JEdt - TsDateSum.EJDay) / TsDateSum.Intvl
        TsDateSum.NVALS = TsDateSum.NVALS + nAppend
        TsDateSum.EJDay = JEdt
        DataVals = GenTs.Values
        ReDim Preserve DataVals(TsDateSum.NVALS)
        If optManFill(0).Value Then 'interpolate
          IntrpInc = (c1 - DataVals(GenTs.dates.Summary.NVALS)) / nAppend
          For i = GenTs.dates.Summary.NVALS + 1 To TsDateSum.NVALS
            DataVals(i) = DataVals(GenTs.dates.Summary.NVALS) + _
                          (i - GenTs.dates.Summary.NVALS) * IntrpInc
          Next i
        Else 'fill with constant value
          For i = GenTs.dates.Summary.NVALS + 1 To TsDateSum.NVALS
            DataVals(i) = c1
          Next i
        End If
        TsDate.Summary = TsDateSum
        Set GenTs.dates = TsDate
        GenTs.Values = DataVals
        outdsn = GenTs.Header.id
      End If
'      If outdsn <= 0 Then
'        MsgBox "The output data set is not specified.", 48, "GenScn Gener Manual Problem"
'      Else
'        oscen = UCase(oscen)
'        oloc = UCase(oloc)
'        ocons = UCase(ocons)
'        EDate(0) = Year(ctlGenerDate.CurrE)
'        EDate(1) = Month(ctlGenerDate.CurrE)
'        EDate(2) = Day(ctlGenerDate.CurrE)
'        EDate(3) = 0
'        EDate(4) = 0
'        EDate(5) = 0
'        SDate(0) = Year(ctlGenerDate.CurrS)
'        SDate(1) = Month(ctlGenerDate.CurrS)
'        SDate(2) = Day(ctlGenerDate.CurrS)
'        SDate(3) = 0
'        SDate(4) = 0
'        SDate(5) = 0
'        mantu = ctlGenerDate.TUnit
'        mants = ctlGenerDate.TStep
'        If optFunction(24) = True Then
'          interp = 2
'        Else
'          interp = 1
'        End If
'        If OutDsnExists = True Then
'          dexist = 1
'        Else
'          dexist = 0
'        End If
'        Call F90_GMANEX(p.HSPFMsg.Unit, p.WDMFiles(1).FileUnit, outdsn, mants, mantu, _
'                        SDate(0), EDate(0), interp, fillval, dexist, _
'                        retc, crecod, oscen, oloc, ocons, _
'                        Len(oscen), Len(oloc), Len(ocons))
'        If (crecod = 1 Or crecod = 0) And retc = 0 Then
'          MsgBox "Manual operation successfully performed.", 0, "GenScn Gener Manual"
'          If OutDsnExists = False Then
'            addedDsn = True
'          End If
'        ElseIf (crecod = 1 Or crecod = 0) And retc <> 0 Then
'          MsgBox "A problem occurred during this operation.", 48, "GenScn Gener Manual Problem"
'        ElseIf crecod <> 0 Then
'          MsgBox "A problem occurred creating this data set.", 48, "GenScn Gener Manual Problem"
'        End If
'      End If
    End If
  End If
  If Not failfg Then
        lErrDesc = GenTs.ErrorDescription
        If Len(lErrDesc) = 0 Then 'gener went ok
          j = MsgBox("Timeseries Generate operation successfully performed." & vbCrLf & "Results will be saved to the specified data set on the WDM file.", vbQuestion + vbOKCancel + vbDefaultButton1, "Timseries Generate")
          If j = vbOK Then 'write new data set
            Set lTsFile = TserFiles.Active(2).obj 'WDM assumed to be in 2nd position
            With GenTs.Header
              .id = outdsn
              .Sen = UCase(txtScenOut.Text)
              .Con = UCase(txtConsOut.Text)
              .Loc = UCase(txtLocOut.Text)
            End With
            GenTs.calcSummary
            If lTsFile.addtimser(GenTs, TsIdReplAsk + TsIdAppendAsk + TsIdRenumAsk) Then
              'output to data set ok
              addedDsn = True
            Else
              MsgBox "Data not saved to WDM file.", vbInformation, "Timeseries Generate"
            End If
          Else 'don't write it, delete it
            MsgBox "Data not saved to WDM file.", vbInformation, "Timeseries Generate"
          End If
        Else
          MsgBox "A problem occurred during this Timeseries Generate operation." & vbCrLf & lErrDesc, 48, "Timeseries Generate Problem"
        End If
  End If
  If addedDsn = True Then 'update available timeseries
    Call RefreshSLC
    Call frmGenScn.RefreshMain
    Call frmGenScn.SelectAll
  End If
  MousePointer = vbDefault
End Sub

Private Sub ctlGenerDate_LostFocus()
  If optOperation(1) = True Then
    'set dates for transform operation
    atxYear.Value = Year(ctlGenerDate.CurrS)
    atxMonth.Value = Month(ctlGenerDate.CurrS)
    atxDay.Value = Day(ctlGenerDate.CurrS)
  End If
End Sub

Private Sub Form_Load()
  ctlGenerDate.Caption = "Dates"
  txtScenOut.Text = ""
  txtLocOut.Text = ""
  txtConsOut.Text = ""
End Sub

Private Sub optFunction_Click(index As Integer)

  fnct = index 'save function number for use elsewhere
  'these are rarely used, assume hidden
  lblSigFigs.Visible = False
  lblExponent.Visible = False
  cmdInputFile.Visible = False
  If fnct = 0 Then
    lblFunction.Caption = ""
  ElseIf fnct = 1 Then
    lblFunction.Caption = "  T = T1 + C                C can be any number"
    OpType = ATCAdd
  ElseIf fnct = 2 Then
    lblFunction.Caption = "  T = T1 * C                C can be any non-zero number"
    OpType = ATCMultiply
  ElseIf fnct = 3 Then
    lblFunction.Caption = "  T = T1 + T2"
    OpType = ATCAdd
  ElseIf fnct = 4 Then
    lblFunction.Caption = "  T = T1 - T2"
    OpType = ATCSubtract
  ElseIf fnct = 5 Then
    lblFunction.Caption = "  T = T1 * T2"
    OpType = ATCMultiply
  ElseIf fnct = 6 Then
    lblFunction.Caption = "  T = T1 / T2               If T2=0, then T=10.0E35"
    OpType = ATCDivide
  ElseIf fnct = 7 Then
    lblFunction.Caption = "  T = (T1+T2) / 2."
    OpType = ATCAve
  ElseIf fnct = 8 Then
    lblFunction.Caption = "  T = (C1*T1)+(C2*T2)       Must have C1+C2=1.0"
    OpType = ATCWeight
  ElseIf fnct = 9 Then
    lblFunction.Caption = "  T = T ** C                C is any non-zero number"
    lblExponent.Visible = True
    OpType = ATCTSPowC
  ElseIf fnct = 10 Then
    lblFunction.Caption = "  T = C ** T1               C is any non-zero number"
    OpType = ATCCPowTS
  ElseIf fnct = 11 Then
    lblFunction.Caption = "  T = T1 ** T2              If T1=0 and T2<0, then T=0"
    OpType = ATCTSPowTS
  ElseIf fnct = 12 Then
    lblFunction.Caption = "  T = e ** T1               If T1>80.5, then T=10.0E35"
    OpType = ATCExp
  ElseIf fnct = 13 Then
    lblFunction.Caption = "  T = log(T1)               IF T1<=0, then T=10.0E35"
    OpType = ATCLog
  ElseIf fnct = 14 Then
    lblFunction.Caption = "  T = ln(T1)                If T1<=0, then T=10.0E35"
    OpType = ATCLn
  ElseIf fnct = 15 Then
    lblFunction.Caption = "  T = abs(T1)"
    OpType = ATCAbs
  ElseIf fnct = 16 Then
    lblFunction.Caption = "  T = T1 if T1 > C1; T = C2 if T1 <= C1"
    OpType = ATCTSgtC
  ElseIf fnct = 17 Then
    lblFunction.Caption = "  T = T1 if T1 < C1; T = C2 if T1 >= C1"
    OpType = ATCTSltC
  ElseIf fnct = 18 Then
    lblFunction.Caption = "  T = min(T1,T2)"
    OpType = ATCMin
  ElseIf fnct = 19 Then
    lblFunction.Caption = "  T = max(T1,T2)"
    OpType = ATCMax
  ElseIf fnct = 20 Then
    lblFunction.Caption = "  T(n) = T1(1)+T1(2)+...+T1(n)"
    OpType = ATCCummDiff
  ElseIf fnct = 21 Then
    lblFunction.Caption = "  T = n significant figures of T1"
    lblSigFigs.Visible = True
  ElseIf fnct = 22 Then
    lblFunction.Caption = "  T = (C1*T1) + C2    C1 and C2 can be any number"
    OpType = ATCMultiply
  ElseIf fnct = 23 Then
    lblFunction.Caption = "  T = value looked up in table"
    cmdInputFile.Visible = True
  End If
  If (fnct > 0 And fnct < 3) Or (fnct > 7 And fnct < 11) Or _
     fnct = 16 Or fnct = 17 Or fnct = 21 Or fnct = 22 Then
    atxConst(0).Visible = True
    If fnct = 9 Or fnct = 21 Then
      lblC1.Visible = False 'use other labels for this constant
      lblConstants.Visible = False
    Else
      lblC1.Visible = True
      lblConstants.Visible = True
    End If
    If fnct = 21 Then
      atxConst(0).Value = 1#
    Else
      atxConst(0).Value = 0#
    End If
  Else
    atxConst(0).Visible = False
    lblC1.Visible = False
    lblConstants.Visible = False
  End If
  If fnct = 8 Or fnct = 16 Or fnct = 17 Or fnct = 22 Then
    atxConst(1).Visible = True
    atxConst(1).Value = 0#
    lblC2.Visible = True
  Else
    atxConst(1).Visible = False
    lblC2.Visible = False
  End If
  If fnct = 0 Or fnct = 23 Then
    If fnct = 0 Then
      nin = 0
    Else
      nin = 1
    End If
  ElseIf (fnct > 2 And fnct < 9) Or fnct = 11 _
          Or fnct = 18 Or fnct = 19 Then
    nin = 2
  Else
    nin = 1
  End If
  Call InitInputTS(nin)
End Sub

Private Sub optManFill_Click(index As Integer)

  If index < 2 Then 'need input dsn
    nin = 1
  Else 'only need output dsn
    nin = 0
  End If
  'set manual defaults for timeseries frame
  Call InitInputTS(nin)
  If index < 2 Then 'don't need output dsn
    lblOutput.Enabled = False
    txtConsOut.Enabled = False
    txtLocOut.Enabled = False
    txtScenOut.Enabled = False
    atxOutDsn.Enabled = False
  Else 'need output dsn
    lblOutput.Enabled = True
    txtConsOut.Enabled = True
    txtLocOut.Enabled = True
    txtScenOut.Enabled = True
    atxOutDsn.Enabled = True
  End If
  txtConsOut.BackColor = atxOutDsn.BackColor
  txtLocOut.BackColor = atxOutDsn.BackColor
  txtScenOut.BackColor = atxOutDsn.BackColor

End Sub

Private Sub optOperation_Click(index As Integer)
  Dim i&, newedat&(6)
  If index = 0 Then
    'compute
    Call GenerInit
  Else 'turn off compute controls
    For i = 0 To 23
      optFunction(i).Visible = False
    Next i
    cmdInputFile.Visible = False
    atxConst(0).Visible = False
    atxConst(1).Visible = False
    lblC1.Visible = False
    lblC2.Visible = False
    lblSigFigs.Visible = False
    lblConstants.Visible = False
    lblExponent.Visible = False
    lblFunction.Visible = False
    If index = 1 Then 'set transform defaults for function frame
      nin = 1
      'hide manual fields
      lblValue.Visible = False
      atxValue.Visible = False
      For i = 0 To optManFill.Count - 1
        optManFill(i).Visible = False
      Next i
      'show fields for transformation
      ctlGenerDate.TUnit = 4
      ctlGenerDate.TSTEP = 1
      mants = 1
      lblYear.Visible = True
      lblMonth.Visible = True
      lblDay.Visible = True
      atxYear.Visible = True
      atxMonth.Visible = True
      atxDay.Visible = True
      ctlGenerDate.TAggr = 1
      'set transform defaults for timeseries frame
      Call InitInputTS(nin)
    ElseIf index = 2 Then 'set manual defaults for function frame
      'hide fields for transformation functions
      lblYear.Visible = False
      lblMonth.Visible = False
      lblDay.Visible = False
      atxYear.Visible = False
      atxMonth.Visible = False
      atxDay.Visible = False
      'show manual fields
      lblValue.Visible = True
      atxValue.Visible = True
      atxValue.Value = 0#
      For i = 0 To optManFill.Count - 1
        optManFill(i).Visible = True
      Next i
      optManFill(0).Value = True
      fillval = 0#
      ctlGenerDate.TUnit = 4
      ctlGenerDate.TSTEP = 1
      mants = 1
    End If
  End If
End Sub

Private Sub txtConsOut_lostfocus()
  ocons = UCase(txtConsOut.Text)
  txtConsOut.Text = ocons
End Sub

Private Sub txtLocOut_lostfocus()
  oloc = UCase(txtLocOut.Text)
  txtLocOut.Text = oloc
End Sub

Private Sub txtScenOut_LostFocus()
  oscen = UCase(txtScenOut.Text)
  txtScenOut.Text = oscen
End Sub

Private Sub InitInputTS(lnt&)
'init input time series specification lists

  Dim i&, j&

  If lnt > 0 Then
    For i = 0 To lnt - 1
      cboInSen(i).clear
      cboInLoc(i).clear
      cboInCon(i).clear
      cboInDsn(i).clear
    Next i
    Set lTs = Nothing
    Set lTs = New Collection
    Call FindTimSer("", "", "", lTs)
    For j = 1 To lTs.Count
      If Not InList(lTs(j).Header.Sen, cboInSen(0)) Then
        For i = 0 To lnt - 1
          cboInSen(i).AddItem lTs(j).Header.Sen
        Next i
      End If
      If Not InList(lTs(j).Header.Loc, cboInLoc(0)) Then
        For i = 0 To lnt - 1
          cboInLoc(i).AddItem lTs(j).Header.Loc
        Next i
      End If
      If Not InList(lTs(j).Header.Con, cboInCon(0)) Then
        For i = 0 To lnt - 1
          cboInCon(i).AddItem lTs(j).Header.Con
        Next i
      End If
      For i = 0 To lnt - 1
        cboInDsn(i).AddItem lTs(j).Header.id
      Next i
    Next j
    For i = 0 To lnt - 1
      cboInSen(i).Visible = True
      cboInLoc(i).Visible = True
      cboInCon(i).Visible = True
      cboInDsn(i).Visible = True
      cboInSen(i).Text = ""
      cboInLoc(i).Text = ""
      cboInCon(i).Text = ""
      cboInDsn(i).Text = ""
      lblInput(i).Visible = True
      SLCDLock(i) = ""
    Next i
    lblOutput.Enabled = True
    txtConsOut.Enabled = True
    txtLocOut.Enabled = True
    txtScenOut.Enabled = True
    atxOutDsn.Enabled = True
    txtScenOut.Text = ""
    txtLocOut.Text = ""
    txtConsOut.Text = ""
    fraTimeseries.Height = cboInSen(lnt - 1).Top + cboInSen(lnt - 1).Height + 80
  Else
    lblOutput.Enabled = False
    txtConsOut.Enabled = False
    txtLocOut.Enabled = False
    txtScenOut.Enabled = False
    atxOutDsn.Enabled = False
    fraTimeseries.Height = cboInSen(0).Top + cboInSen(0).Height + 80
  End If
  For i = lnt To 1 'turn off any unneeded labels
    cboInSen(i).Visible = False
    cboInLoc(i).Visible = False
    cboInCon(i).Visible = False
    cboInDsn(i).Visible = False
    lblInput(i).Visible = False
  Next i
  ctlGenerDate.Top = fraTimeseries.Top + fraTimeseries.Height + 108
  fraDumGenerDates.Top = ctlGenerDate.Top
  cmdPerform.Top = ctlGenerDate.Top + ctlGenerDate.Height + 108
  cmdClose.Top = cmdPerform.Top
  frmTSGener.Height = cmdClose.Top + cmdClose.Height + 450
  Call UpdateDates

End Sub

Private Sub cboInLoc_Change(index As Integer)

'  'update output location if unique input location specified
'  If cboInLoc(Index).Text <> "mult" And cboInLoc(Index).Text <> "ALL" Then
'    If optFunction(11).Value = False Or Index = 0 Then
'      'don't update output location if selecting adjacent hourly locations for precip disagg
'      txtLocOut.Text = cboInLoc(Index).Text
'    End If
'  End If

End Sub

Private Sub cboInCon_Click(index As Integer)

  If InStr(SLCDLock(index), "C") <= 0 Then
    'constituent selection is locked
    SLCDLock(index) = SLCDLock(index) & "C"
  End If
  Call UpdateInputTS(index, "C")
  'update output constituent if unique input constituent specified
  If cboInCon(index).Text <> "mult" And _
     cboInCon(index).Text <> "ALL" And _
     txtConsOut.Enabled Then
    txtConsOut.Text = cboInCon(index).Text
  End If

End Sub

Private Sub cboInLoc_Click(index As Integer)

  If InStr(SLCDLock(index), "L") <= 0 Then
    'location selection is locked
    SLCDLock(index) = SLCDLock(index) & "L"
  End If
  Call UpdateInputTS(index, "L")
  'update output location if unique input location specified
  If cboInLoc(index).Text <> "mult" And _
     cboInLoc(index).Text <> "ALL" And _
     txtLocOut.Enabled Then
    txtLocOut.Text = cboInLoc(index).Text
  End If

End Sub

Private Sub cboInSen_Click(index As Integer)

  If InStr(SLCDLock(index), "S") <= 0 Then
    'scenario selection is locked
    SLCDLock(index) = SLCDLock(index) & "S"
  End If
  Call UpdateInputTS(index, "S")
  'update output scenario if unique input scenario specified
  If cboInSen(index).Text <> "mult" And _
     cboInSen(index).Text <> "ALL" And _
     txtScenOut.Enabled Then
    txtScenOut.Text = cboInSen(index).Text
  End If

End Sub

Private Sub UpdateInputTS(ByVal Ind&, CurSel$)

  Dim i&, ip&, lt As Collection
  Dim lsen$, lloc$, lcon$
  Dim SLock As Boolean, LLock As Boolean
  Dim CLock As Boolean ', DLock As Boolean

  If cboInSen(Ind).Text = "ALL" Or cboInSen(Ind).Text = "mult" Then
    'look for all scenarios
    lsen = ""
    ip = InStr(SLCDLock(Ind), "S")
    If ip > 0 Then 'remove scenario lock
      SLCDLock(Ind) = Left(SLCDLock(Ind), ip - 1) & Mid(SLCDLock(Ind), ip + 1)
    End If
  Else 'look for selected scenario
    lsen = cboInSen(Ind).Text
  End If
  If cboInLoc(Ind).Text = "ALL" Or cboInLoc(Ind).Text = "mult" Then
    'look for all locations
    lloc = ""
    ip = InStr(SLCDLock(Ind), "L")
    If ip > 0 Then 'remove scenario lock
      SLCDLock(Ind) = Left(SLCDLock(Ind), ip - 1) & Mid(SLCDLock(Ind), ip + 1)
    End If
  Else 'look for selected location
    lloc = cboInLoc(Ind).Text
  End If
  If cboInCon(Ind).Text = "ALL" Or cboInCon(Ind).Text = "mult" Then
    'look for all constituents
    lcon = ""
    ip = InStr(SLCDLock(Ind), "C")
    If ip > 0 Then 'remove scenario lock
      SLCDLock(Ind) = Left(SLCDLock(Ind), ip - 1) & Mid(SLCDLock(Ind), ip + 1)
    End If
  Else 'look for selected location
    lcon = cboInCon(Ind).Text
  End If
  Call FindTimSer(lsen, lloc, lcon, lt)
  'clear lists if they're not current selection
  'also add "ALL" option
  If CurSel <> "S" Then
    cboInSen(Ind).clear
    cboInSen(Ind).AddItem "ALL"
  End If
  If CurSel <> "L" Then
    cboInLoc(Ind).clear
    cboInLoc(Ind).AddItem "ALL"
  End If
  If CurSel <> "C" Then
    cboInCon(Ind).clear
    cboInCon(Ind).AddItem "ALL"
  End If
  If InStr(SLCDLock(Ind), "S") Then 'don't change scenario
    SLock = True
    If CurSel <> "S" Then
      'locked but not current selection,
      'keep locked selection in list
      cboInSen(Ind).AddItem lsen
      cboInSen(Ind).Text = lsen
    End If
  Else 'ok to update scenario list
    SLock = False
  End If
  If InStr(SLCDLock(Ind), "L") Then  'don't change location
    LLock = True
    If CurSel <> "L" Then
      'locked but not current selection,
      'keep locked selection in list
      cboInLoc(Ind).AddItem lloc
      cboInLoc(Ind).Text = lloc
    End If
  Else 'ok to update location list
    LLock = False
  End If
  If InStr(SLCDLock(Ind), "C") Then  'don't change constituent
    CLock = True
    If CurSel <> "C" Then
      'locked but not current selection,
      'keep locked selection in list
      cboInCon(Ind).AddItem lcon
      cboInCon(Ind).Text = lcon
    End If
  Else 'ok to update constituent list
    CLock = False
  End If
  'always clear dataset number list
  cboInDsn(Ind).clear
  For i = 1 To lt.Count
    If Not SLock Then 'update scenario list
      If Not InList(lt(i).Header.Sen, cboInSen(Ind)) Then
        cboInSen(Ind).AddItem lt(i).Header.Sen
      End If
    End If
    If Not LLock Then 'update location list
      If Not InList(lt(i).Header.Loc, cboInLoc(Ind)) Then
        cboInLoc(Ind).AddItem lt(i).Header.Loc
      End If
    End If
    If Not CLock Then 'update constituent list
      If Not InList(lt(i).Header.Con, cboInCon(Ind)) Then
        cboInCon(Ind).AddItem lt(i).Header.Con
      End If
    End If
    'always update data-set list
    cboInDsn(Ind).AddItem lt(i).Header.id
  Next i
  'if list not locked, select an item
  If Not SLock Then
    If cboInSen(Ind).ListCount > 2 Then
      'multiple items matched, default to mult
      cboInSen(Ind).Text = "mult" 'cboInSen(ind).List(0)
    ElseIf cboInSen(Ind).ListCount = 2 Then
      'only one item matched (besides ALL)
      cboInSen(Ind).Text = cboInSen(Ind).List(1)
    Else 'no items matched
      cboInSen(Ind).Text = "none"
    End If
  End If
  If Not LLock Then
    If cboInLoc(Ind).ListCount > 2 Then
      'multiple items matched, default to mult
      cboInLoc(Ind).Text = "mult" 'cboInLoc(ind).List(0)
    ElseIf cboInLoc(Ind).ListCount = 2 Then
      'only one item matched (besides ALL)
      cboInLoc(Ind).Text = cboInLoc(Ind).List(1)
    Else 'no items matched
      cboInLoc(Ind).Text = "none"
    End If
  End If
  If Not CLock Then
    If cboInCon(Ind).ListCount > 2 Then
      'multiple items matched, default to mult
      cboInCon(Ind).Text = "mult" 'cboInCon(ind).List(0)
    ElseIf cboInCon(Ind).ListCount = 2 Then
      'only one item matched (besides ALL)
      cboInCon(Ind).Text = cboInCon(Ind).List(1)
    Else 'no items matched
      cboInCon(Ind).Text = "none"
    End If
  End If
  If cboInDsn(Ind).ListCount > 1 Then 'multiple time series match
    cboInDsn(Ind).Text = "mult"
  ElseIf cboInDsn(Ind).ListCount = 1 Then
    'one timeseries matched, display it
    cboInDsn(Ind).Text = cboInDsn(Ind).List(0)
  Else 'no timeseries matched
    cboInDsn(Ind).Text = "none"
  End If

End Sub

Private Sub cboInDsn_Change(index As Integer)

  If IsNumeric(cboInDsn(index).Text) Then
    Call InDsnUpdate(index)
  End If

End Sub

Private Sub cboInDsn_Click(index As Integer)

  If IsNumeric(cboInDsn(index).Text) Then
    Call InDsnUpdate(index)
  End If

End Sub

Private Sub InDsnUpdate(index As Integer)
  Dim i&, cdsn&, saind&, salen&, StrSen$, StrLoc$, StrCon$
  Dim InTs As ATCclsTserData

  'find matching TSer
  i = 1
  Do While i <= lTs.Count
    If lTs(i).Header.id = cboInDsn(index).Text Then
      'found a match
      Set InTs = lTs(i)
      i = lTs.Count
    End If
    i = i + 1
  Loop
  If InTs.Header.id > 0 Then
    'get S,L,C for selected dsn
    cboInSen(index).Text = InTs.Header.Sen
    cboInLoc(index).Text = InTs.Header.Loc
    cboInCon(index).Text = InTs.Header.Con
    Call UpdateDates
    If optOperation(1) Then 'set defaults for transform start date
      If atxYear.Value <= 0 Then
        atxYear.Value = Year(ctlGenerDate.CurrS)
        atxMonth.Value = Month(ctlGenerDate.CurrS)
        atxDay.Value = Day(ctlGenerDate.CurrS)
      End If
    End If
  End If

End Sub

Private Sub UpdateDates()

  Dim i&, j&, k&, l&, d$, NoDates As Boolean, Sdt&(5), Edt&(5)
  Dim Strt&(), Stp&(), retcod&, lnts&, otu&, ots&

  ReDim Strt(6 * nin)
  ReDim Stp(6 * nin)

'    NoDates = False
  lnts = 0
  NoDates = True
  For i = 0 To nin - 1
    If IsNumeric(cboInDsn(i).Text) Then
      If CLng(cboInDsn(i).Text) > 0 Then 'valid dsn
        j = 1
        Do While j <= lTs.Count
          If lTs(j).Header.id = CLng(cboInDsn(i).Text) Then
            Call J2Date(lTs(j).dates.Summary.SJDay, Sdt())
            Call J2Date(lTs(j).dates.Summary.EJDay, Edt())
            otu = lTs(j).dates.Summary.Tu
            ots = lTs(j).dates.Summary.ts
            Call timcnv(Edt())
            For l = 0 To 5
              k = i * 6 + l
              Strt(k) = Sdt(l)
              Stp(k) = Edt(l)
            Next l
            j = lTs.Count
            lnts = lnts + 1
            NoDates = False
          End If
          j = j + 1
        Loop
      Else
'          NoDates = True
      End If
    Else
'        NoDates = True
    End If
  Next i

  If Not NoDates Then
    retcod = 0
    If lnts > 1 Then 'nin > 1 Then
      ReDim Preserve Strt(6 * lnts)
      ReDim Preserve Stp(6 * lnts)
      Call datcmn(Strt(), Stp(), CSDat(), CEDat(), retcod)
      Call timcnv(CEDat())
    Else
      For i = 0 To 5
        CSDat(i) = Strt(i)
        CEDat(i) = Stp(i)
      Next i
    End If
    If optOperation(2).Value Then 'appending to existing data set
      Call F90_TIMADD(CEDat(0), otu, ots, 1, Sdt(0)) 'need to lag by 1 time step
      ctlGenerDate.CurrS = DateSerial(Sdt(0), Sdt(1), Sdt(2)) + TimeSerial(Sdt(3), Sdt(4), Sdt(5))
      ctlGenerDate.LimtS = ctlGenerDate.CurrS
      Call F90_TIMADD(Sdt(0), otu, ots, 1, Edt(0))
      ctlGenerDate.CurrE = DateSerial(Edt(0), Edt(1), Edt(2)) + TimeSerial(Edt(3), Edt(4), Edt(5))
      ctlGenerDate.LimtE = DateSerial(2200, Edt(1), Edt(2)) + TimeSerial(Edt(3), Edt(4), Edt(5))
    Else 'just use common period for input datasets
      ctlGenerDate.CurrS = DateSerial(CSDat(0), CSDat(1), CSDat(2))
      ctlGenerDate.LimtS = ctlGenerDate.CurrS
      ctlGenerDate.CurrE = DateSerial(CEDat(0), CEDat(1), CEDat(2))
      ctlGenerDate.LimtE = ctlGenerDate.CurrE
    End If
    ctlGenerDate.CommS = ctlGenerDate.LimtS
    ctlGenerDate.CommE = ctlGenerDate.LimtE
    If retcod < 0 Then 'no common period found
      lblDumGenerDates.Caption = "No Common Period for Selected Timeseries"
      NoDates = True
    ElseIf CSDat(0) = 0 And CEDat(0) = 0 Then
      'no data in these data sets
      lblDumGenerDates.Caption = "No Data Available in Selected Timeseries."
      NoDates = True
    End If
  ElseIf optOperation(2).Value And optManFill(2).Value Then
    'filling new dataset
    ctlGenerDate.CurrS = DateSerial(1900, 1, 1)
    ctlGenerDate.CurrE = DateSerial(2000, 12, 31)
    ctlGenerDate.LimtS = DateSerial(1900, 1, 1)
    ctlGenerDate.LimtE = DateSerial(2200, 12, 31)
    NoDates = False
  Else 'dsns not specified, no dates to display
    lblDumGenerDates.Caption = "No Input Data Sets Specified."
  End If
  ctlGenerDate.Width = fraDumGenerDates.Width
  'assume no dates being displayed;
  'making them visible will refresh dates if needed
  ctlGenerDate.Visible = False
  If NoDates Then
    fraDumGenerDates.Visible = True
  Else
    ctlGenerDate.Visible = True
    fraDumGenerDates.Visible = False
  End If

End Sub


