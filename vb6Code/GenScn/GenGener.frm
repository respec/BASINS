VERSION 5.00
Begin VB.Form frmGenScnGener 
   BorderStyle     =   3  'Fixed Dialog
   Caption         =   "GenScn Gener"
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
   Icon            =   "GenGener.frx":0000
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   PaletteMode     =   1  'UseZOrder
   ScaleHeight     =   6864
   ScaleWidth      =   6888
   Begin VB.CommandButton cmdClose 
      Cancel          =   -1  'True
      Caption         =   "Close"
      Height          =   372
      Left            =   3960
      TabIndex        =   65
      Top             =   6360
      Width           =   972
   End
   Begin VB.CommandButton cmdPerform 
      Caption         =   "&Perform Operation"
      Default         =   -1  'True
      Height          =   372
      Left            =   1800
      TabIndex        =   64
      Top             =   6360
      Width           =   1812
   End
   Begin VB.Frame frmFunction 
      Caption         =   "Function"
      Height          =   2052
      Left            =   120
      TabIndex        =   30
      Top             =   2760
      Width           =   6612
      Begin VB.OptionButton optFunction 
         Caption         =   "Overwrite"
         Height          =   252
         Index           =   27
         Left            =   3480
         TabIndex        =   77
         Top             =   1680
         Width           =   1215
      End
      Begin VB.OptionButton optFunction 
         Caption         =   "Append"
         Height          =   252
         Index           =   26
         Left            =   2280
         TabIndex        =   76
         Top             =   1680
         Width           =   972
      End
      Begin VB.TextBox txtDay 
         Height          =   288
         Left            =   4920
         TabIndex        =   72
         Top             =   360
         Width           =   732
      End
      Begin VB.TextBox txtMonth 
         Height          =   288
         Left            =   3600
         TabIndex        =   71
         Top             =   360
         Width           =   732
      End
      Begin VB.TextBox txtYear 
         Height          =   288
         Left            =   2160
         TabIndex        =   70
         Top             =   360
         Width           =   732
      End
      Begin VB.TextBox txtValue 
         Height          =   288
         Left            =   4200
         TabIndex        =   68
         Top             =   720
         Width           =   852
      End
      Begin VB.OptionButton optFunction 
         Caption         =   "Use this value for all"
         Height          =   252
         Index           =   25
         Left            =   720
         TabIndex        =   67
         Top             =   840
         Width           =   2292
      End
      Begin VB.OptionButton optFunction 
         Caption         =   "Interpolate to this value"
         Height          =   252
         Index           =   24
         Left            =   720
         TabIndex        =   66
         Top             =   600
         Width           =   2415
      End
      Begin VB.TextBox txtC2 
         Height          =   288
         Left            =   4800
         TabIndex        =   63
         Top             =   1680
         Width           =   852
      End
      Begin VB.TextBox txtC1 
         Height          =   288
         Left            =   2880
         TabIndex        =   62
         Top             =   1680
         Width           =   852
      End
      Begin VB.CommandButton cmdInputFile 
         Caption         =   "Input File"
         Height          =   252
         Left            =   120
         TabIndex        =   56
         Top             =   1680
         Width           =   972
      End
      Begin VB.OptionButton optFunction 
         Caption         =   "Table"
         Height          =   252
         Index           =   23
         Left            =   4920
         TabIndex        =   54
         Top             =   960
         Width           =   972
      End
      Begin VB.OptionButton optFunction 
         Caption         =   "Line"
         Height          =   252
         Index           =   22
         Left            =   4920
         TabIndex        =   53
         Top             =   720
         Width           =   972
      End
      Begin VB.OptionButton optFunction 
         Caption         =   "SigF"
         Height          =   252
         Index           =   21
         Left            =   4920
         TabIndex        =   52
         Top             =   480
         Width           =   972
      End
      Begin VB.OptionButton optFunction 
         Caption         =   "Sum"
         Height          =   252
         Index           =   20
         Left            =   4920
         TabIndex        =   51
         Top             =   240
         Width           =   972
      End
      Begin VB.OptionButton optFunction 
         Caption         =   "Max"
         Height          =   252
         Index           =   19
         Left            =   3960
         TabIndex        =   50
         Top             =   960
         Width           =   972
      End
      Begin VB.OptionButton optFunction 
         Caption         =   "Min"
         Height          =   252
         Index           =   18
         Left            =   3960
         TabIndex        =   49
         Top             =   720
         Width           =   972
      End
      Begin VB.OptionButton optFunction 
         Caption         =   "Z+"
         Height          =   252
         Index           =   17
         Left            =   3960
         TabIndex        =   48
         Top             =   480
         Width           =   972
      End
      Begin VB.OptionButton optFunction 
         Caption         =   "Z-"
         Height          =   252
         Index           =   16
         Left            =   3960
         TabIndex        =   47
         Top             =   240
         Width           =   972
      End
      Begin VB.OptionButton optFunction 
         Caption         =   "Abs"
         Height          =   252
         Index           =   15
         Left            =   3000
         TabIndex        =   46
         Top             =   960
         Width           =   972
      End
      Begin VB.OptionButton optFunction 
         Caption         =   "Lge"
         Height          =   252
         Index           =   14
         Left            =   3000
         TabIndex        =   45
         Top             =   720
         Width           =   972
      End
      Begin VB.OptionButton optFunction 
         Caption         =   "Lg10"
         Height          =   252
         Index           =   13
         Left            =   3000
         TabIndex        =   44
         Top             =   480
         Width           =   972
      End
      Begin VB.OptionButton optFunction 
         Caption         =   "E**"
         Height          =   252
         Index           =   12
         Left            =   3000
         TabIndex        =   43
         Top             =   240
         Width           =   972
      End
      Begin VB.OptionButton optFunction 
         Caption         =   "Pow"
         Height          =   252
         Index           =   11
         Left            =   2040
         TabIndex        =   42
         Top             =   960
         Width           =   972
      End
      Begin VB.OptionButton optFunction 
         Caption         =   "C**"
         Height          =   252
         Index           =   10
         Left            =   2040
         TabIndex        =   41
         Top             =   720
         Width           =   972
      End
      Begin VB.OptionButton optFunction 
         Caption         =   "**C"
         Height          =   252
         Index           =   9
         Left            =   2040
         TabIndex        =   40
         Top             =   480
         Width           =   972
      End
      Begin VB.OptionButton optFunction 
         Caption         =   "Wght"
         Height          =   252
         Index           =   8
         Left            =   2040
         TabIndex        =   39
         Top             =   240
         Width           =   972
      End
      Begin VB.OptionButton optFunction 
         Caption         =   "Mean"
         Height          =   252
         Index           =   7
         Left            =   1080
         TabIndex        =   38
         Top             =   960
         Width           =   972
      End
      Begin VB.OptionButton optFunction 
         Caption         =   "Div"
         Height          =   252
         Index           =   6
         Left            =   1080
         TabIndex        =   37
         Top             =   720
         Width           =   972
      End
      Begin VB.OptionButton optFunction 
         Caption         =   "Mult"
         Height          =   252
         Index           =   5
         Left            =   1080
         TabIndex        =   36
         Top             =   480
         Width           =   972
      End
      Begin VB.OptionButton optFunction 
         Caption         =   "Sub"
         Height          =   252
         Index           =   4
         Left            =   1080
         TabIndex        =   35
         Top             =   240
         Width           =   972
      End
      Begin VB.OptionButton optFunction 
         Caption         =   "Add"
         Height          =   252
         Index           =   3
         Left            =   120
         TabIndex        =   34
         Top             =   960
         Width           =   972
      End
      Begin VB.OptionButton optFunction 
         Caption         =   "*C"
         Height          =   252
         Index           =   2
         Left            =   120
         TabIndex        =   33
         Top             =   720
         Width           =   972
      End
      Begin VB.OptionButton optFunction 
         Caption         =   "+C"
         Height          =   252
         Index           =   1
         Left            =   120
         TabIndex        =   32
         Top             =   480
         Width           =   972
      End
      Begin VB.OptionButton optFunction 
         Caption         =   "None"
         Height          =   252
         Index           =   0
         Left            =   120
         TabIndex        =   31
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
      Begin VB.Label lblDay 
         Caption         =   "Day:"
         Height          =   252
         Left            =   4440
         TabIndex        =   75
         Top             =   360
         Width           =   492
      End
      Begin VB.Label lblMonth 
         Caption         =   "Month:"
         Height          =   252
         Left            =   3000
         TabIndex        =   74
         Top             =   360
         Width           =   612
      End
      Begin VB.Label lblYear 
         Caption         =   "Output Starting Year:"
         Height          =   252
         Left            =   360
         TabIndex        =   73
         Top             =   360
         Width           =   1812
      End
      Begin VB.Label lblValue 
         Caption         =   "Value:"
         Height          =   252
         Left            =   3600
         TabIndex        =   69
         Top             =   720
         Width           =   612
      End
      Begin VB.Label lblSigFigs 
         Caption         =   "Sig Figs:"
         Height          =   252
         Left            =   1920
         TabIndex        =   61
         Top             =   1680
         Visible         =   0   'False
         Width           =   852
      End
      Begin VB.Label lblExponent 
         Caption         =   "Exponent:"
         Height          =   252
         Left            =   1800
         TabIndex        =   60
         Top             =   1680
         Visible         =   0   'False
         Width           =   852
      End
      Begin VB.Label lblC2 
         Caption         =   "C2"
         Height          =   252
         Left            =   4200
         TabIndex        =   59
         Top             =   1680
         Width           =   252
      End
      Begin VB.Label lblC1 
         Caption         =   "C1"
         Height          =   252
         Left            =   2400
         TabIndex        =   58
         Top             =   1680
         Width           =   252
      End
      Begin VB.Label lblConstants 
         Caption         =   "Constants:"
         Height          =   252
         Left            =   1320
         TabIndex        =   57
         Top             =   1680
         Width           =   972
      End
      Begin VB.Label lblFunction 
         Height          =   252
         Left            =   240
         TabIndex        =   55
         Top             =   1320
         Width           =   6132
      End
   End
   Begin VB.Frame frmOperation 
      Caption         =   "Operation"
      Height          =   852
      Left            =   120
      TabIndex        =   29
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
   Begin VB.Frame frmTimeseries 
      Caption         =   "Timeseries"
      Height          =   1572
      Left            =   120
      TabIndex        =   12
      Top             =   1080
      Width           =   6612
      Begin VB.TextBox txtConsOut 
         Height          =   288
         Left            =   5280
         TabIndex        =   7
         Top             =   480
         Width           =   1092
      End
      Begin VB.TextBox txtLocOut 
         Height          =   288
         Left            =   4080
         TabIndex        =   6
         Top             =   480
         Width           =   1092
      End
      Begin VB.TextBox txtScenOut 
         Height          =   288
         Left            =   2880
         TabIndex        =   5
         Top             =   480
         Width           =   1092
      End
      Begin VB.TextBox txtOutputDsn 
         Height          =   288
         Left            =   2040
         TabIndex        =   4
         Top             =   480
         Width           =   732
      End
      Begin VB.TextBox txtInputDsn1 
         Height          =   288
         Left            =   2040
         TabIndex        =   9
         Top             =   840
         Width           =   732
      End
      Begin VB.TextBox txtInputDsn2 
         Height          =   288
         Left            =   2040
         TabIndex        =   11
         Top             =   1200
         Width           =   732
      End
      Begin VB.CommandButton cmdMainOut 
         Caption         =   "From Main"
         Height          =   252
         Left            =   840
         TabIndex        =   3
         Top             =   480
         Width           =   1092
      End
      Begin VB.CommandButton cmdMainIn1 
         Caption         =   "From Main"
         Height          =   252
         Left            =   840
         TabIndex        =   8
         Top             =   840
         Width           =   1092
      End
      Begin VB.CommandButton cmdMainIn2 
         Caption         =   "From Main"
         Height          =   252
         Left            =   840
         TabIndex        =   10
         Top             =   1200
         Width           =   1092
      End
      Begin VB.Label lblConstit 
         Caption         =   "Constituent"
         Height          =   252
         Left            =   5280
         TabIndex        =   28
         Top             =   240
         Width           =   972
      End
      Begin VB.Label lblConsOut 
         Caption         =   "OutputCons"
         Height          =   252
         Left            =   5280
         TabIndex        =   27
         Top             =   480
         Width           =   1092
      End
      Begin VB.Label lblConsIn1 
         Caption         =   "InputCons1"
         Height          =   252
         Left            =   5280
         TabIndex        =   26
         Top             =   840
         Width           =   1092
      End
      Begin VB.Label lblConsIn2 
         Caption         =   "InputCons2"
         Height          =   252
         Left            =   5280
         TabIndex        =   25
         Top             =   1200
         Width           =   1092
      End
      Begin VB.Label lblLocation 
         Caption         =   "Location"
         Height          =   252
         Left            =   4080
         TabIndex        =   24
         Top             =   240
         Width           =   972
      End
      Begin VB.Label lblLocOut 
         Caption         =   "OutputLoc"
         Height          =   252
         Left            =   4080
         TabIndex        =   23
         Top             =   480
         Width           =   1092
      End
      Begin VB.Label lblLocIn1 
         Caption         =   "InputLoc1"
         Height          =   252
         Left            =   4080
         TabIndex        =   22
         Top             =   840
         Width           =   1092
      End
      Begin VB.Label lblLocIn2 
         Caption         =   "InputLoc2"
         Height          =   252
         Left            =   4080
         TabIndex        =   21
         Top             =   1200
         Width           =   1092
      End
      Begin VB.Label lblScenario 
         Caption         =   "Scenario"
         Height          =   252
         Left            =   2880
         TabIndex        =   20
         Top             =   240
         Width           =   972
      End
      Begin VB.Label lblScenOut 
         Caption         =   "OutputScen"
         Height          =   252
         Left            =   2880
         TabIndex        =   19
         Top             =   480
         Width           =   1092
      End
      Begin VB.Label lblScenIn1 
         Caption         =   "InputScen1"
         Height          =   252
         Left            =   2880
         TabIndex        =   18
         Top             =   840
         Width           =   1092
      End
      Begin VB.Label lblScenIn2 
         Caption         =   "InputScen2"
         Height          =   252
         Left            =   2880
         TabIndex        =   17
         Top             =   1200
         Width           =   1092
      End
      Begin VB.Label lblDsn 
         Caption         =   "Dsn"
         Height          =   252
         Left            =   2040
         TabIndex        =   16
         Top             =   240
         Width           =   372
      End
      Begin VB.Label lblInput2 
         Caption         =   "Input 2:"
         Height          =   255
         Left            =   120
         TabIndex        =   15
         Top             =   1200
         Width           =   735
      End
      Begin VB.Label lblInput1 
         Caption         =   "Input 1:"
         Height          =   255
         Left            =   120
         TabIndex        =   14
         Top             =   840
         Width           =   735
      End
      Begin VB.Label lblOutput 
         Caption         =   "Output:"
         Height          =   252
         Left            =   120
         TabIndex        =   13
         Top             =   480
         Width           =   612
      End
   End
   Begin ATCoCtl.ATCoDate ctlGenerDate 
      Height          =   1350
      Left            =   120
      TabIndex        =   78
      Top             =   4920
      Width           =   6615
      _ExtentX        =   11663
      _ExtentY        =   2392
      TUnit           =   4
      TAggr           =   1
      TStep           =   1
      CurrE           =   73415
      CurrS           =   2
      LimtE           =   73415
      LimtS           =   2
      DispL           =   1
      LabelCurrentRange=   "To Graph"
      TstepVisible    =   -1  'True
   End
End
Attribute VB_Name = "frmGenScnGener"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
'Copyright 2000 by AQUA TERRA Consultants
Dim outdsn&, oscen$, oloc$, ocons$, otu&, ots&, osdat&(6), oedat&(6), ogsiz&
Dim in1dsn&, i1scen$, i1loc$, i1cons$, i1tu&, i1ts&, i1sdat&(6), i1edat&(6), i1gsiz&
Dim in2dsn&, i2scen$, i2loc$, i2cons$, i2tu&, i2ts&, i2sdat&(6), i2edat&(6), i2gsiz&
Dim csdate&(6), cedate&(6), cts&, ctu&
Dim k1&, rc!(2), OutDsnExists
Dim nin&, fnct&, fillval!, mants&
Dim valueold$, c1old$, c2old$, olddsnin1$, olddsnin2$, olddsnout$
Dim oldyear$, oldmonth$, oldday$, iyear&, imonth&, iday&

Sub CheckInputDsn1()
    Dim SDate&(12), EDate&(12), Tu&(2), ts&(2), retcod&, dtype&
    
    If in1dsn > 0 Then
      dtype = F90_WDCKDT(p.WDMFiles(1).fileUnit, in1dsn)
      If dtype <> 0 Then
        'data set already exists
        'Call F90_TSDSPC(in1dsn, i1scen, i1loc, i1cons, i1tu, i1ts, i1sdat, i1edat, i1gsiz)
        Call GetInfoFromWDMTSer(in1dsn, i1scen, i1loc, i1cons, i1tu, i1ts, i1sdat, i1edat)
        If i1sdat(0) = 0 Then
          lblScenIn1.Visible = False
          lblLocIn1.Visible = False
          lblConsIn1.Visible = False
          lblScenIn1.Caption = ""
          lblLocIn1.Caption = ""
          lblConsIn1.Caption = ""
          MsgBox "The input data set must contain data.", vbExclamation, "GenScn Gener Problem"
          txtInputDsn1.SetFocus
        Else
          lblScenIn1.Visible = True
          lblLocIn1.Visible = True
          lblConsIn1.Visible = True
          lblScenIn1.Caption = i1scen
          lblLocIn1.Caption = i1loc
          lblConsIn1.Caption = i1cons
          'set dates
          If nin = 1 Then
            ctlGenerDate.CurrE = DateSerial(i1edat(0), i1edat(1), i1edat(2))
            ctlGenerDate.CurrS = DateSerial(i1sdat(0), i1sdat(1), i1sdat(2))
            ctlGenerDate.LimtE = DateSerial(i1edat(0), i1edat(1), i1edat(2))
            ctlGenerDate.LimtS = DateSerial(i1sdat(0), i1sdat(1), i1sdat(2))
            ctlGenerDate.CommE = ctlGenerDate.LimtE
            ctlGenerDate.CommS = ctlGenerDate.LimtS
            ctlGenerDate.TSTEP = i1ts
            ctlGenerDate.TUnit = i1tu
            cts = i1ts
            ctu = i1tu
            ctlGenerDate.Visible = False
            ctlGenerDate.Visible = True
            If optOperation(1) = True Then
              'set dates for target data set
              txtYear.text = i1sdat(0)
              txtMonth.text = i1sdat(1)
              txtDay.text = i1sdat(2)
              iyear = i1sdat(0)
              imonth = i1sdat(1)
              iday = i1sdat(2)
            End If
          ElseIf nin = 2 And in2dsn > 0 Then
            SDate(0) = i1sdat(0)
            SDate(1) = i1sdat(1)
            SDate(2) = i1sdat(2)
            SDate(3) = i1sdat(3)
            SDate(4) = i1sdat(4)
            SDate(5) = i1sdat(5)
            EDate(0) = i1edat(0)
            EDate(1) = i1edat(1)
            EDate(2) = i1edat(2)
            EDate(3) = i1edat(3)
            EDate(4) = i1edat(4)
            EDate(5) = i1edat(5)
            SDate(6) = i2sdat(0)
            SDate(7) = i2sdat(1)
            SDate(8) = i2sdat(2)
            SDate(9) = i2sdat(3)
            SDate(10) = i2sdat(4)
            SDate(11) = i2sdat(5)
            EDate(6) = i2edat(0)
            EDate(7) = i2edat(1)
            EDate(8) = i2edat(2)
            EDate(9) = i2edat(3)
            EDate(10) = i2edat(4)
            EDate(11) = i2edat(5)
            ts(0) = i1ts
            ts(1) = i2ts
            Tu(0) = i1tu
            Tu(1) = i2tu
            Call F90_DTMCMN(2, SDate(0), EDate(0), ts(0), Tu(0), _
                            csdate(0), cedate(0), cts, ctu, retcod)
            ctlGenerDate.CurrE = DateSerial(cedate(0), cedate(1), cedate(2))
            ctlGenerDate.CurrS = DateSerial(csdate(0), csdate(1), csdate(2))
            ctlGenerDate.LimtE = DateSerial(cedate(0), cedate(1), cedate(2))
            ctlGenerDate.LimtS = DateSerial(csdate(0), csdate(1), csdate(2))
            ctlGenerDate.CommE = ctlGenerDate.LimtE
            ctlGenerDate.CommS = ctlGenerDate.LimtS
            ctlGenerDate.Visible = False
            ctlGenerDate.Visible = True
          End If
        End If
      Else
        'data set does not exist, needs to exist
        lblScenIn1.Visible = False
        lblLocIn1.Visible = False
        lblConsIn1.Visible = False
        lblScenIn1.Caption = ""
        lblLocIn1.Caption = ""
        lblConsIn1.Caption = ""
        MsgBox "The input data set must already exist.", vbExclamation, "GenScn Gener Problem"
        txtInputDsn1.SetFocus
      End If
    End If
End Sub

Sub CheckInputDsn2()
  Dim SDate&(12), EDate&(12), ts&(2), Tu&(2), retcod&, dtype&
    If in2dsn > 0 Then
      dtype = F90_WDCKDT(p.WDMFiles(1).fileUnit, in2dsn)
      If dtype <> 0 Then
        'data set already exists
        'Call F90_TSDSPC(in2dsn, i2scen, i2loc, i2cons, i2tu, i2ts, i2sdat, i2edat, i2gsiz)
        Call GetInfoFromWDMTSer(in2dsn, i2scen, i2loc, i2cons, i2tu, i2ts, i2sdat, i2edat)
        If i2sdat(0) = 0 Then
          lblScenIn2.Visible = False
          lblLocIn2.Visible = False
          lblConsIn2.Visible = False
          lblScenIn2.Caption = ""
          lblLocIn2.Caption = ""
          lblConsIn2.Caption = ""
          MsgBox "The input data set must contain data.", vbExclamation, "GenScn Gener Problem"
          txtInputDsn2.SetFocus
        Else
          lblScenIn2.Visible = True
          lblLocIn2.Visible = True
          lblConsIn2.Visible = True
          lblScenIn2.Caption = i2scen
          lblLocIn2.Caption = i2loc
          lblConsIn2.Caption = i2cons
          If in1dsn > 0 Then
            'set dates
            SDate(0) = i1sdat(0)
            SDate(1) = i1sdat(1)
            SDate(2) = i1sdat(2)
            SDate(3) = i1sdat(3)
            SDate(4) = i1sdat(4)
            SDate(5) = i1sdat(5)
            EDate(0) = i1edat(0)
            EDate(1) = i1edat(1)
            EDate(2) = i1edat(2)
            EDate(3) = i1edat(3)
            EDate(4) = i1edat(4)
            EDate(5) = i1edat(5)
            SDate(6) = i2sdat(0)
            SDate(7) = i2sdat(1)
            SDate(8) = i2sdat(2)
            SDate(9) = i2sdat(3)
            SDate(10) = i2sdat(4)
            SDate(11) = i2sdat(5)
            EDate(6) = i2edat(0)
            EDate(7) = i2edat(1)
            EDate(8) = i2edat(2)
            EDate(9) = i2edat(3)
            EDate(10) = i2edat(4)
            EDate(11) = i2edat(5)
            ts(0) = i1ts
            ts(1) = i2ts
            Tu(0) = i1tu
            Tu(1) = i2tu
            Call F90_DTMCMN(2, SDate(0), EDate(0), ts(0), Tu(0), _
                             csdate(0), cedate(0), cts, ctu, retcod)
            ctlGenerDate.CurrE = DateSerial(cedate(0), cedate(1), cedate(2))
            ctlGenerDate.CurrS = DateSerial(csdate(0), csdate(1), csdate(2))
            ctlGenerDate.LimtE = DateSerial(cedate(0), cedate(1), cedate(2))
            ctlGenerDate.LimtS = DateSerial(csdate(0), csdate(1), csdate(2))
            ctlGenerDate.CommE = ctlGenerDate.LimtE
            ctlGenerDate.CommS = ctlGenerDate.LimtS
            ctlGenerDate.Visible = False
            ctlGenerDate.Visible = True
          End If
        End If
      Else
        'data set does not exist, needs to exist
        lblScenIn2.Visible = False
        lblLocIn2.Visible = False
        lblConsIn2.Visible = False
        lblScenIn2.Caption = ""
        lblLocIn2.Caption = ""
        lblConsIn2.Caption = ""
        MsgBox "The input data set must already exist.", vbExclamation, "GenScn Gener Problem"
        txtInputDsn2.SetFocus
      End If
    End If
End Sub

Sub CheckOutDsn()
  Dim dtype&, newedat&(6), newsdat&(6)
    If outdsn > 0 Then
      dtype = F90_WDCKDT(p.WDMFiles(1).fileUnit, outdsn)
      If dtype = 0 Then
        'new data set
        txtScenOut.Visible = True
        txtLocOut.Visible = True
        txtConsOut.Visible = True
        txtScenOut.SetFocus
        OutDsnExists = False
      Else
        'data set already exists
        'Call F90_TSDSPC(outdsn, oscen, oloc, ocons, otu, ots, osdat, oedat, ogsiz)
        Call GetInfoFromWDMTSer(outdsn, oscen, oloc, ocons, otu, ots, osdat, oedat)
        lblScenOut.Visible = True
        lblLocOut.Visible = True
        lblConsOut.Visible = True
        lblScenOut.Caption = oscen
        lblLocOut.Caption = oloc
        lblConsOut.Caption = ocons
        txtScenOut.Visible = False
        txtLocOut.Visible = False
        txtConsOut.Visible = False
        OutDsnExists = True
      End If
      If optOperation(0) = True And dtype <> 0 Then
        'doing compute, needs to be new data set
        MsgBox "For a compute operation, the output data set must not already exist.", vbExclamation, "GenScn Gener Compute Problem"
      ElseIf optOperation(1) = True And dtype <> 0 Then
        'doing transform with existing data set
        optFunction(26).Visible = True
        optFunction(27).Visible = True
        optFunction(26) = True
      ElseIf optOperation(1) = True And dtype = 0 Then
        'doing transform with new data set
        optFunction(26).Visible = False
        optFunction(27).Visible = False
      ElseIf optOperation(2) = True And dtype <> 0 Then
        'doing manual with existing data set
        ctlGenerDate.TUnit = otu
        ctlGenerDate.TSTEP = ots
        If oedat(0) = 0 Then
          oedat(0) = 1900
          oedat(1) = 1
          oedat(2) = 1
        End If
        Call F90_TIMADD(oedat(0), 4, 1, 1, newsdat(0)) 'need to lag by 1 time step
        ctlGenerDate.CurrS = DateSerial(newsdat(0), newsdat(1), newsdat(2))
        ctlGenerDate.LimtS = DateSerial(oedat(0), oedat(1), oedat(2))
        ctlGenerDate.CommS = ctlGenerDate.LimtS
        Call F90_TIMADD(newsdat(0), otu, ots, 1, newedat(0))
        ctlGenerDate.CurrE = DateSerial(newedat(0), newedat(1), newedat(2))
        ctlGenerDate.LimtE = DateSerial(2100, newedat(1), newedat(2))
        ctlGenerDate.CommE = ctlGenerDate.LimtE
        ctlGenerDate.Visible = False
        ctlGenerDate.Visible = True
      ElseIf optOperation(2) = True And dtype = 0 Then
        'doing manual with new data set
        ctlGenerDate.CurrS = DateSerial(1900, 1, 1)
        ctlGenerDate.CurrE = DateSerial(2100, 12, 31)
        ctlGenerDate.LimtS = DateSerial(1900, 1, 1)
        ctlGenerDate.LimtE = DateSerial(2100, 12, 31)
        ctlGenerDate.CommE = ctlGenerDate.LimtE
        ctlGenerDate.CommS = ctlGenerDate.LimtS
        ctlGenerDate.Visible = False
        ctlGenerDate.Visible = True
      End If
    End If
End Sub

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
  txtC1.Visible = False
  txtC2.Visible = False
  txtC1.text = ""
  txtC2.text = ""
  lblC1.Visible = False
  lblC2.Visible = False
  lblSigFigs.Visible = False
  lblConstants.Visible = False
  lblExponent.Visible = False
  lblFunction.Visible = True
  lblFunction.Caption = ""
  'set compute defaults for timeseries frame
  lblInput1.Visible = False
  cmdMainIn1.Visible = False
  txtInputDsn1.Visible = False
  lblScenIn1.Visible = False
  lblLocIn1.Visible = False
  lblConsIn1.Visible = False
  lblInput2.Visible = False
  cmdMainIn2.Visible = False
  txtInputDsn2.Visible = False
  lblScenIn2.Visible = False
  lblLocIn2.Visible = False
  lblConsIn2.Visible = False
  lblScenOut.Visible = False
  lblLocOut.Visible = False
  lblConsOut.Visible = False
  txtOutputDsn.text = ""
  txtInputDsn1.text = ""
  txtInputDsn2.text = ""
  txtScenOut.text = ""
  txtLocOut.text = ""
  txtConsOut.text = ""
  lblScenOut.Caption = ""
  lblLocOut.Caption = ""
  lblConsOut.Caption = ""
  lblScenIn1.Caption = ""
  lblLocIn1.Caption = ""
  lblConsIn1.Caption = ""
  lblScenIn2.Caption = ""
  lblLocIn2.Caption = ""
  lblConsIn2.Caption = ""
  cmdMainOut.Visible = False
  'set default dsns
  outdsn = 0
  'hide manual fields
  lblValue.Visible = False
  txtValue.Visible = False
  optFunction(24).Visible = False
  optFunction(25).Visible = False
  'hide transformation fields
  lblYear.Visible = False
  lblMonth.Visible = False
  lblDay.Visible = False
  txtYear.Visible = False
  txtMonth.Visible = False
  txtDay.Visible = False
  optFunction(26).Visible = False
  optFunction(27).Visible = False
End Sub



Private Sub cmdClose_Click()
  Unload frmGenScnGener
End Sub


Private Sub cmdInputFile_Click()
  CDInfile.flags = &H1804&
  CDInfile.Filename = "gener.inp"
  CDInfile.DialogTitle = "GenScn Gener Compute Input File"
  CDInfile.Action = 1
End Sub

Private Sub cmdMainIn1_Click()
  Dim i&
  i = frmGenScn.grdDsnSelected
  If i > 0 Then
    txtInputDsn1.text = i
    in1dsn = txtInputDsn1.text
    Call CheckInputDsn1
  Else
    'nothing selected on main
    MsgBox "No timeseries is selected in the GenScn Timeseries list.", vbExclamation, "GenScn Gener Problem"
  End If
End Sub

Private Sub cmdMainIn2_Click()
  Dim i&
  i = frmGenScn.grdDsnSelected
  If i > 0 Then
    txtInputDsn2.text = i
    in2dsn = txtInputDsn2.text
    Call CheckInputDsn2
  Else
    'nothing selected on main
    MsgBox "No timeseries is selected in the GenScn Timeseries list.", vbExclamation, "GenScn Gener Problem"
  End If
End Sub

Private Sub cmdMainOut_Click()
  Dim i&
  i = frmGenScn.grdDsnSelected
  If i > 0 Then
    txtOutputDsn.text = i
    outdsn = txtOutputDsn.text
    Call CheckOutDsn
  Else
    'nothing selected on main
    MsgBox "No timeseries is selected in the GenScn Timeseries list.", vbExclamation, "GenScn Gener Problem"
  End If
End Sub

Private Sub cmdPerform_Click()
  Dim dsn&(3), retc&, EDate&(6), SDate&(6), dates&(18)
  Dim interp&, mantu&, dexist&, crecod&, naoro&, tranf&
  Dim crtflg&, cmpflg&, flag1&, addedOrModifiedData As Boolean, ifound As Boolean
  Dim filnam As String, i&, ctxt As String
  
  addedOrModifiedData = False
  MousePointer = vbHourglass
  If optOperation(0) = True Then
    'do compute operation
    If OutDsnExists = True Then
      'cant do if output dsn already exists
      MsgBox "The output data set already exists.", vbExclamation, "GenScn Gener Compute Problem"
    Else
      dsn(0) = outdsn
      dsn(1) = in1dsn
      dsn(2) = in2dsn
      ocons = UCase(txtConsOut.text)
      txtConsOut.text = ocons
      oloc = UCase(txtLocOut.text)
      txtLocOut.text = oloc
      oscen = UCase(txtScenOut.text)
      txtScenOut.text = oscen
      If fnct = 24 Then
        filnam = CDInfile.Filename
      Else
        filnam = ""
      End If
      If dsn(0) <= 0 Or (nin > 0 And dsn(1) <= 0) Or (nin > 1 And dsn(2) <= 0) Then
        MsgBox "At least one data set is not specified.", vbExclamation, "GenScn Gener Compute Problem"
      ElseIf fnct <> 1 Then
        oscen = UCase(oscen)
        oloc = UCase(oloc)
        ocons = UCase(ocons)
        EDate(0) = Year(ctlGenerDate.CurrE)
        EDate(1) = Month(ctlGenerDate.CurrE)
        EDate(2) = Day(ctlGenerDate.CurrE)
        EDate(3) = 0
        EDate(4) = 0
        EDate(5) = 0
        SDate(0) = Year(ctlGenerDate.CurrS)
        SDate(1) = Month(ctlGenerDate.CurrS)
        SDate(2) = Day(ctlGenerDate.CurrS)
        SDate(3) = 0
        SDate(4) = 0
        SDate(5) = 0
        cts = ctlGenerDate.TSTEP
        ctu = ctlGenerDate.TUnit
        Call F90_GNMEXE(p.WDMFiles(1).fileUnit, dsn(0), fnct, nin, cts, _
                        ctu, SDate(0), EDate(0), rc(0), k1, _
                        p.HSPFMsg.Unit, retc, filnam, oscen, oloc, ocons, _
                        Len(filnam), Len(oscen), Len(oloc), Len(ocons))
        If retc = 0 Then
          MsgBox "Compute operation successfully performed.", 0, "GenScn Gener Compute"
          OutDsnExists = True
          addedOrModifiedData = True
        Else
          MsgBox "A problem occurred during this operation.", vbExclamation, "GenScn Gener Compute Problem"
        End If
      End If
    End If
  ElseIf optOperation(1) = True Then
    'do transform operation
    If outdsn <= 0 Then
      MsgBox "The output data set is not specified.", vbExclamation, "GenScn Gener Transform Problem"
    ElseIf in1dsn <= 0 Then
      MsgBox "The input data set is not specified.", vbExclamation, "GenScn Gener Transform Problem"
    Else
      oscen = UCase(oscen)
      oloc = UCase(oloc)
      ocons = UCase(ocons)
      dsn(0) = in1dsn
      dsn(1) = outdsn
      If OutDsnExists = False Then
        naoro = 1
      ElseIf optFunction(26) = True Then
        naoro = 2
      Else
        naoro = 3
      End If
      dates(0) = Year(ctlGenerDate.CurrS)
      dates(1) = Month(ctlGenerDate.CurrS)
      dates(2) = Day(ctlGenerDate.CurrS)
      dates(3) = 0
      dates(4) = 0
      dates(5) = 0
      dates(6) = Year(ctlGenerDate.CurrE)
      dates(7) = Month(ctlGenerDate.CurrE)
      dates(8) = Day(ctlGenerDate.CurrE)
      dates(9) = 0
      dates(10) = 0
      dates(11) = 0
      dates(12) = iyear
      dates(13) = imonth
      dates(14) = iday
      dates(15) = 0
      dates(16) = 0
      dates(17) = 0
      tranf = ctlGenerDate.TAggr
      mantu = ctlGenerDate.TUnit
      mants = ctlGenerDate.TSTEP
      Call F90_GNTTRN(p.HSPFMsg.Unit, p.WDMFiles(1).fileUnit, dsn(0), naoro, _
                      dates(0), tranf, mants, mantu, _
                      crtflg, cmpflg, retc, flag1, _
                      oscen, oloc, ocons, _
                      Len(oscen), Len(oloc), Len(ocons))
      If flag1 = 1 Then
        MsgBox "Date is not compatible with time step.", vbExclamation, "GenScn Gener Transform Problem"
      ElseIf flag1 = 2 Then
        MsgBox "Time steps are not compatible.", vbExclamation, "GenScn Gener Transform Problem"
      ElseIf crtflg > 1 Then
        MsgBox "Problem creating new data set.", vbExclamation, "GenScn Gener Transform Problem"
      ElseIf cmpflg > 1 Then
        MsgBox "A problem occurred during this operation.", vbExclamation, "GenScn Gener Transform Problem"
      Else
        MsgBox "Transform operation successfully performed.", 0, "GenScn Gener Transform"
        'If OutDsnExists = False Then
          addedOrModifiedData = True
        'End If
      End If
    End If
  ElseIf optOperation(2) = True Then
    'do manual manipulation operation
    If outdsn <= 0 Then
      MsgBox "The output data set is not specified.", vbExclamation, "GenScn Gener Manual Problem"
    Else
      oscen = UCase(oscen)
      oloc = UCase(oloc)
      ocons = UCase(ocons)
      EDate(0) = Year(ctlGenerDate.CurrE)
      EDate(1) = Month(ctlGenerDate.CurrE)
      EDate(2) = Day(ctlGenerDate.CurrE)
      EDate(3) = 0
      EDate(4) = 0
      EDate(5) = 0
      SDate(0) = Year(ctlGenerDate.CurrS)
      SDate(1) = Month(ctlGenerDate.CurrS)
      SDate(2) = Day(ctlGenerDate.CurrS)
      SDate(3) = 0
      SDate(4) = 0
      SDate(5) = 0
      mantu = ctlGenerDate.TUnit
      mants = ctlGenerDate.TSTEP
      If optFunction(24) = True Then
        interp = 2
      Else
        interp = 1
      End If
      If OutDsnExists = True Then
        dexist = 1
      Else
        dexist = 0
      End If
      Call F90_GMANEX(p.HSPFMsg.Unit, p.WDMFiles(1).fileUnit, outdsn, mants, mantu, _
                      SDate(0), EDate(0), interp, fillval, dexist, _
                      retc, crecod, oscen, oloc, ocons, _
                      Len(oscen), Len(oloc), Len(ocons))
      If (crecod = 1 Or crecod = 0) And retc = 0 Then
        MsgBox "Manual operation successfully performed.", 0, "GenScn Gener Manual"
        'If OutDsnExists = False Then
          addedOrModifiedData = True
        'End If
      ElseIf (crecod = 1 Or crecod = 0) And retc <> 0 Then
        MsgBox "A problem occurred during this operation.", vbExclamation, "GenScn Gener Manual Problem"
      ElseIf crecod <> 0 Then
        MsgBox "A problem occurred creating this data set.", vbExclamation, "GenScn Gener Manual Problem"
      End If
    End If
  End If
  If addedOrModifiedData Then
    p.WDMFiles(1).Refresh
    RefreshSLC
  End If
  MousePointer = vbDefault
End Sub

Private Sub ctlGenerDate_LostFocus()
  If optOperation(1) = True Then
    'set dates for transform operation
    With ctlGenerDate
      txtYear.text = Year(.CurrS)
      txtMonth.text = Month(.CurrS)
      txtDay.text = Day(.CurrS)
      iyear = Year(.CurrS)
      imonth = Month(.CurrS)
      iday = Day(.CurrS)
    End With
  End If
End Sub

Private Sub Form_Load()
    ctlGenerDate.Caption = "Dates"
    txtOutputDsn.text = ""
    txtInputDsn1.text = ""
    txtInputDsn2.text = ""
    txtScenOut.text = ""
    txtLocOut.text = ""
    txtConsOut.text = ""
    lblScenOut.Caption = ""
    lblLocOut.Caption = ""
    lblConsOut.Caption = ""
    lblScenIn1.Caption = ""
    lblLocIn1.Caption = ""
    lblConsIn1.Caption = ""
    lblScenIn2.Caption = ""
    lblLocIn2.Caption = ""
    lblConsIn2.Caption = ""
    rc(0) = 0
    rc(1) = 0
End Sub

Private Sub optFunction_Click(index As Integer)
  If index = 0 Then
    cmdInputFile.Visible = False
    txtC1.Visible = False
    txtC2.Visible = False
    lblC1.Visible = False
    lblC2.Visible = False
    lblSigFigs.Visible = False
    lblConstants.Visible = False
    lblExponent.Visible = False
    lblFunction.Caption = ""
    lblInput1.Visible = False
    cmdMainIn1.Visible = False
    txtInputDsn1.Visible = False
    lblScenIn1.Visible = False
    lblLocIn1.Visible = False
    lblConsIn1.Visible = False
    lblInput2.Visible = False
    cmdMainIn2.Visible = False
    txtInputDsn2.Visible = False
    lblScenIn2.Visible = False
    lblLocIn2.Visible = False
    lblConsIn2.Visible = False
  ElseIf index = 1 Then
    cmdInputFile.Visible = False
    txtC1.Visible = True
    txtC1.text = 0#
    txtC2.Visible = False
    lblC1.Visible = True
    lblC2.Visible = False
    lblSigFigs.Visible = False
    lblConstants.Visible = True
    lblExponent.Visible = False
    lblFunction.Caption = "  T = T1 + C                C can be any number"
    lblInput1.Visible = True
    cmdMainIn1.Visible = True
    txtInputDsn1.Visible = True
    lblScenIn1.Visible = True
    lblLocIn1.Visible = True
    lblConsIn1.Visible = True
    lblInput2.Visible = False
    cmdMainIn2.Visible = False
    txtInputDsn2.Visible = False
    lblScenIn2.Visible = False
    lblLocIn2.Visible = False
    lblConsIn2.Visible = False
  ElseIf index = 2 Then
    cmdInputFile.Visible = False
    txtC1.Visible = True
    txtC1.text = 0#
    txtC2.Visible = False
    lblC1.Visible = True
    lblC2.Visible = False
    lblSigFigs.Visible = False
    lblConstants.Visible = True
    lblExponent.Visible = False
    lblFunction.Caption = "  T = T1 * C                C can be any non-zero number"
    lblInput1.Visible = True
    cmdMainIn1.Visible = True
    txtInputDsn1.Visible = True
    lblScenIn1.Visible = True
    lblLocIn1.Visible = True
    lblConsIn1.Visible = True
    lblInput2.Visible = False
    cmdMainIn2.Visible = False
    txtInputDsn2.Visible = False
    lblScenIn2.Visible = False
    lblLocIn2.Visible = False
    lblConsIn2.Visible = False
  ElseIf index = 3 Then
    cmdInputFile.Visible = False
    txtC1.Visible = False
    txtC2.Visible = False
    lblC1.Visible = False
    lblC2.Visible = False
    lblSigFigs.Visible = False
    lblConstants.Visible = False
    lblExponent.Visible = False
    lblFunction.Caption = "  T = T1 + T2"
    lblInput1.Visible = True
    cmdMainIn1.Visible = True
    txtInputDsn1.Visible = True
    lblScenIn1.Visible = True
    lblLocIn1.Visible = True
    lblConsIn1.Visible = True
    lblInput2.Visible = True
    cmdMainIn2.Visible = True
    txtInputDsn2.Visible = True
    lblScenIn2.Visible = True
    lblLocIn2.Visible = True
    lblConsIn2.Visible = True
  ElseIf index = 4 Then
    cmdInputFile.Visible = False
    txtC1.Visible = False
    txtC2.Visible = False
    lblC1.Visible = False
    lblC2.Visible = False
    lblSigFigs.Visible = False
    lblConstants.Visible = False
    lblExponent.Visible = False
    lblFunction.Caption = "  T = T1 - T2"
    lblInput1.Visible = True
    cmdMainIn1.Visible = True
    txtInputDsn1.Visible = True
    lblScenIn1.Visible = True
    lblLocIn1.Visible = True
    lblConsIn1.Visible = True
    lblInput2.Visible = True
    cmdMainIn2.Visible = True
    txtInputDsn2.Visible = True
    lblScenIn2.Visible = True
    lblLocIn2.Visible = True
    lblConsIn2.Visible = True
  ElseIf index = 5 Then
    cmdInputFile.Visible = False
    txtC1.Visible = False
    txtC2.Visible = False
    lblC1.Visible = False
    lblC2.Visible = False
    lblSigFigs.Visible = False
    lblConstants.Visible = False
    lblExponent.Visible = False
    lblFunction.Caption = "  T = T1 * T2"
    lblInput1.Visible = True
    cmdMainIn1.Visible = True
    txtInputDsn1.Visible = True
    lblScenIn1.Visible = True
    lblLocIn1.Visible = True
    lblConsIn1.Visible = True
    lblInput2.Visible = True
    cmdMainIn2.Visible = True
    txtInputDsn2.Visible = True
    lblScenIn2.Visible = True
    lblLocIn2.Visible = True
    lblConsIn2.Visible = True
  ElseIf index = 6 Then
    cmdInputFile.Visible = False
    txtC1.Visible = False
    txtC2.Visible = False
    lblC1.Visible = False
    lblC2.Visible = False
    lblSigFigs.Visible = False
    lblConstants.Visible = False
    lblExponent.Visible = False
    lblFunction.Caption = "  T = T1 / T2               If T2=0, then T=10.0E35"
    lblInput1.Visible = True
    cmdMainIn1.Visible = True
    txtInputDsn1.Visible = True
    lblScenIn1.Visible = True
    lblLocIn1.Visible = True
    lblConsIn1.Visible = True
    lblInput2.Visible = True
    cmdMainIn2.Visible = True
    txtInputDsn2.Visible = True
    lblScenIn2.Visible = True
    lblLocIn2.Visible = True
    lblConsIn2.Visible = True
  ElseIf index = 7 Then
    cmdInputFile.Visible = False
    txtC1.Visible = False
    txtC2.Visible = False
    lblC1.Visible = False
    lblC2.Visible = False
    lblSigFigs.Visible = False
    lblConstants.Visible = False
    lblExponent.Visible = False
    lblFunction.Caption = "  T = (T1+T2) / 2."
    lblInput1.Visible = True
    cmdMainIn1.Visible = True
    txtInputDsn1.Visible = True
    lblScenIn1.Visible = True
    lblLocIn1.Visible = True
    lblConsIn1.Visible = True
    lblInput2.Visible = True
    cmdMainIn2.Visible = True
    txtInputDsn2.Visible = True
    lblScenIn2.Visible = True
    lblLocIn2.Visible = True
    lblConsIn2.Visible = True
  ElseIf index = 8 Then
    cmdInputFile.Visible = False
    txtC1.Visible = True
    txtC2.Visible = True
    txtC1.text = 0#
    txtC2.text = 0#
    lblC1.Visible = True
    lblC2.Visible = True
    lblSigFigs.Visible = False
    lblConstants.Visible = True
    lblExponent.Visible = False
    lblFunction.Caption = "  T = (C1*T1)+(C2*T2)       Must have C1+C2=1.0"
    lblInput1.Visible = True
    cmdMainIn1.Visible = True
    txtInputDsn1.Visible = True
    lblScenIn1.Visible = True
    lblLocIn1.Visible = True
    lblConsIn1.Visible = True
    lblInput2.Visible = True
    cmdMainIn2.Visible = True
    txtInputDsn2.Visible = True
    lblScenIn2.Visible = True
    lblLocIn2.Visible = True
    lblConsIn2.Visible = True
  ElseIf index = 9 Then
    cmdInputFile.Visible = False
    txtC1.Visible = True
    txtC1.text = 0#
    txtC2.Visible = False
    lblC1.Visible = False
    lblC2.Visible = False
    lblSigFigs.Visible = False
    lblConstants.Visible = False
    lblExponent.Visible = True
    lblFunction.Caption = "  T = T ** C                C is any non-zero number"
    lblInput1.Visible = True
    cmdMainIn1.Visible = True
    txtInputDsn1.Visible = True
    lblScenIn1.Visible = True
    lblLocIn1.Visible = True
    lblConsIn1.Visible = True
    lblInput2.Visible = False
    cmdMainIn2.Visible = False
    txtInputDsn2.Visible = False
    lblScenIn2.Visible = False
    lblLocIn2.Visible = False
    lblConsIn2.Visible = False
  ElseIf index = 10 Then
    cmdInputFile.Visible = False
    txtC1.Visible = True
    txtC1.text = 0#
    txtC2.Visible = False
    lblC1.Visible = True
    lblC2.Visible = False
    lblSigFigs.Visible = False
    lblConstants.Visible = True
    lblExponent.Visible = False
    lblFunction.Caption = "  T = C ** T1               C is any non-zero number"
    lblInput1.Visible = True
    cmdMainIn1.Visible = True
    txtInputDsn1.Visible = True
    lblScenIn1.Visible = True
    lblLocIn1.Visible = True
    lblConsIn1.Visible = True
    lblInput2.Visible = False
    cmdMainIn2.Visible = False
    txtInputDsn2.Visible = False
    lblScenIn2.Visible = False
    lblLocIn2.Visible = False
    lblConsIn2.Visible = False
  ElseIf index = 11 Then
    cmdInputFile.Visible = False
    txtC1.Visible = False
    txtC2.Visible = False
    lblC1.Visible = False
    lblC2.Visible = False
    lblSigFigs.Visible = False
    lblConstants.Visible = False
    lblExponent.Visible = False
    lblFunction.Caption = "  T = T1 ** T2              If T1=0 and T2<0, then T=0"
    lblInput1.Visible = True
    cmdMainIn1.Visible = True
    txtInputDsn1.Visible = True
    lblScenIn1.Visible = True
    lblLocIn1.Visible = True
    lblConsIn1.Visible = True
    lblInput2.Visible = True
    cmdMainIn2.Visible = True
    txtInputDsn2.Visible = True
    lblScenIn2.Visible = True
    lblLocIn2.Visible = True
    lblConsIn2.Visible = True
  ElseIf index = 12 Then
    cmdInputFile.Visible = False
    txtC1.Visible = False
    txtC2.Visible = False
    lblC1.Visible = False
    lblC2.Visible = False
    lblSigFigs.Visible = False
    lblConstants.Visible = False
    lblExponent.Visible = False
    lblFunction.Caption = "  T = e ** T1               If T1>80.5, then T=10.0E35"
    lblInput1.Visible = True
    cmdMainIn1.Visible = True
    txtInputDsn1.Visible = True
    lblScenIn1.Visible = True
    lblLocIn1.Visible = True
    lblConsIn1.Visible = True
    lblInput2.Visible = False
    cmdMainIn2.Visible = False
    txtInputDsn2.Visible = False
    lblScenIn2.Visible = False
    lblLocIn2.Visible = False
    lblConsIn2.Visible = False
  ElseIf index = 13 Then
    cmdInputFile.Visible = False
    txtC1.Visible = False
    txtC2.Visible = False
    lblC1.Visible = False
    lblC2.Visible = False
    lblSigFigs.Visible = False
    lblConstants.Visible = False
    lblExponent.Visible = False
    lblFunction.Caption = "  T = log(T1)               IF T1<=0, then T=10.0E35"
    lblInput1.Visible = True
    cmdMainIn1.Visible = True
    txtInputDsn1.Visible = True
    lblScenIn1.Visible = True
    lblLocIn1.Visible = True
    lblConsIn1.Visible = True
    lblInput2.Visible = False
    cmdMainIn2.Visible = False
    txtInputDsn2.Visible = False
    lblScenIn2.Visible = False
    lblLocIn2.Visible = False
    lblConsIn2.Visible = False
  ElseIf index = 14 Then
    cmdInputFile.Visible = False
    txtC1.Visible = False
    txtC2.Visible = False
    lblC1.Visible = False
    lblC2.Visible = False
    lblSigFigs.Visible = False
    lblConstants.Visible = False
    lblExponent.Visible = False
    lblFunction.Caption = "  T = ln(T1)                If T1<=0, then T=10.0E35"
    lblInput1.Visible = True
    cmdMainIn1.Visible = True
    txtInputDsn1.Visible = True
    lblScenIn1.Visible = True
    lblLocIn1.Visible = True
    lblConsIn1.Visible = True
    lblInput2.Visible = False
    cmdMainIn2.Visible = False
    txtInputDsn2.Visible = False
    lblScenIn2.Visible = False
    lblLocIn2.Visible = False
    lblConsIn2.Visible = False
  ElseIf index = 15 Then
    cmdInputFile.Visible = False
    txtC1.Visible = False
    txtC2.Visible = False
    lblC1.Visible = False
    lblC2.Visible = False
    lblSigFigs.Visible = False
    lblConstants.Visible = False
    lblExponent.Visible = False
    lblFunction.Caption = "  T = abs(T1)"
    lblInput1.Visible = True
    cmdMainIn1.Visible = True
    txtInputDsn1.Visible = True
    lblScenIn1.Visible = True
    lblLocIn1.Visible = True
    lblConsIn1.Visible = True
    lblInput2.Visible = False
    cmdMainIn2.Visible = False
    txtInputDsn2.Visible = False
    lblScenIn2.Visible = False
    lblLocIn2.Visible = False
    lblConsIn2.Visible = False
  ElseIf index = 16 Then
    cmdInputFile.Visible = False
    txtC1.Visible = True
    txtC2.Visible = True
    txtC1.text = 0#
    txtC2.text = 0#
    lblC1.Visible = True
    lblC2.Visible = True
    lblSigFigs.Visible = False
    lblConstants.Visible = True
    lblExponent.Visible = False
    lblFunction.Caption = "  T = T1 if T1 >= C1; T = C2 if T1 < C1"
    lblInput1.Visible = True
    cmdMainIn1.Visible = True
    txtInputDsn1.Visible = True
    lblScenIn1.Visible = True
    lblLocIn1.Visible = True
    lblConsIn1.Visible = True
    lblInput2.Visible = False
    cmdMainIn2.Visible = False
    txtInputDsn2.Visible = False
    lblScenIn2.Visible = False
    lblLocIn2.Visible = False
    lblConsIn2.Visible = False
  ElseIf index = 17 Then
    cmdInputFile.Visible = False
    txtC1.Visible = True
    txtC2.Visible = True
    txtC1.text = 0#
    txtC2.text = 0#
    lblC1.Visible = True
    lblC2.Visible = True
    lblSigFigs.Visible = False
    lblConstants.Visible = True
    lblExponent.Visible = False
    lblFunction.Caption = "  T = T1 if T1 <= C1; T = C2 if T1 > C1"
    lblInput1.Visible = True
    cmdMainIn1.Visible = True
    txtInputDsn1.Visible = True
    lblScenIn1.Visible = True
    lblLocIn1.Visible = True
    lblConsIn1.Visible = True
    lblInput2.Visible = False
    cmdMainIn2.Visible = False
    txtInputDsn2.Visible = False
    lblScenIn2.Visible = False
    lblLocIn2.Visible = False
    lblConsIn2.Visible = False
  ElseIf index = 18 Then
    cmdInputFile.Visible = False
    txtC1.Visible = False
    txtC2.Visible = False
    lblC1.Visible = False
    lblC2.Visible = False
    lblSigFigs.Visible = False
    lblConstants.Visible = False
    lblExponent.Visible = False
    lblFunction.Caption = "  T = min(T1,T2)"
    lblInput1.Visible = True
    cmdMainIn1.Visible = True
    txtInputDsn1.Visible = True
    lblScenIn1.Visible = True
    lblLocIn1.Visible = True
    lblConsIn1.Visible = True
    lblInput2.Visible = True
    cmdMainIn2.Visible = True
    txtInputDsn2.Visible = True
    lblScenIn2.Visible = True
    lblLocIn2.Visible = True
    lblConsIn2.Visible = True
  ElseIf index = 19 Then
    cmdInputFile.Visible = False
    txtC1.Visible = False
    txtC2.Visible = False
    lblC1.Visible = False
    lblC2.Visible = False
    lblSigFigs.Visible = False
    lblConstants.Visible = False
    lblExponent.Visible = False
    lblFunction.Caption = "  T = max(T1,T2)"
    lblInput1.Visible = True
    cmdMainIn1.Visible = True
    txtInputDsn1.Visible = True
    lblScenIn1.Visible = True
    lblLocIn1.Visible = True
    lblConsIn1.Visible = True
    lblInput2.Visible = True
    cmdMainIn2.Visible = True
    txtInputDsn2.Visible = True
    lblScenIn2.Visible = True
    lblLocIn2.Visible = True
    lblConsIn2.Visible = True
  ElseIf index = 20 Then
    cmdInputFile.Visible = False
    txtC1.Visible = False
    txtC2.Visible = False
    lblC1.Visible = False
    lblC2.Visible = False
    lblSigFigs.Visible = False
    lblConstants.Visible = False
    lblExponent.Visible = False
    lblFunction.Caption = "  T(n) = T1(1)+T1(2)+...+T1(n)"
    lblInput1.Visible = True
    cmdMainIn1.Visible = True
    txtInputDsn1.Visible = True
    lblScenIn1.Visible = True
    lblLocIn1.Visible = True
    lblConsIn1.Visible = True
    lblInput2.Visible = False
    cmdMainIn2.Visible = False
    txtInputDsn2.Visible = False
    lblScenIn2.Visible = False
    lblLocIn2.Visible = False
    lblConsIn2.Visible = False
  ElseIf index = 21 Then
    cmdInputFile.Visible = False
    txtC1.Visible = True
    txtC1.text = 1
    txtC2.Visible = False
    lblC1.Visible = False
    lblC2.Visible = False
    lblSigFigs.Visible = True
    lblConstants.Visible = False
    lblExponent.Visible = False
    lblFunction.Caption = "  T = n significant figures of T1"
    lblInput1.Visible = True
    cmdMainIn1.Visible = True
    txtInputDsn1.Visible = True
    lblScenIn1.Visible = True
    lblLocIn1.Visible = True
    lblConsIn1.Visible = True
    lblInput2.Visible = False
    cmdMainIn2.Visible = False
    txtInputDsn2.Visible = False
    lblScenIn2.Visible = False
    lblLocIn2.Visible = False
    lblConsIn2.Visible = False
  ElseIf index = 22 Then
    cmdInputFile.Visible = False
    txtC1.Visible = True
    txtC2.Visible = True
    txtC1.text = 0#
    txtC2.text = 0#
    lblC1.Visible = True
    lblC2.Visible = True
    lblSigFigs.Visible = False
    lblConstants.Visible = True
    lblExponent.Visible = False
    lblFunction.Caption = "  T = (C1*T1) + C2    C1 and C2 can be any number"
    lblInput1.Visible = True
    cmdMainIn1.Visible = True
    txtInputDsn1.Visible = True
    lblScenIn1.Visible = True
    lblLocIn1.Visible = True
    lblConsIn1.Visible = True
    lblInput2.Visible = False
    cmdMainIn2.Visible = False
    txtInputDsn2.Visible = False
    lblScenIn2.Visible = False
    lblLocIn2.Visible = False
    lblConsIn2.Visible = False
  ElseIf index = 23 Then
    cmdInputFile.Visible = True
    txtC1.Visible = False
    txtC2.Visible = False
    lblC1.Visible = False
    lblC2.Visible = False
    lblSigFigs.Visible = False
    lblConstants.Visible = False
    lblExponent.Visible = False
    lblFunction.Caption = "  T = value looked up in table"
    lblInput1.Visible = True
    cmdMainIn1.Visible = True
    txtInputDsn1.Visible = True
    lblScenIn1.Visible = True
    lblLocIn1.Visible = True
    lblConsIn1.Visible = False
    lblInput2.Visible = False
    cmdMainIn2.Visible = False
    txtInputDsn2.Visible = False
    lblScenIn2.Visible = False
    lblLocIn2.Visible = False
    lblConsIn2.Visible = False
  End If
  fnct = index + 1
  If fnct = 1 Then
    nin = 0
  ElseIf fnct = 2 Or fnct = 3 Then
    nin = 1
  ElseIf fnct > 3 And fnct < 10 Then
    nin = 2
  ElseIf fnct = 10 Or fnct = 11 Then
    nin = 1
  ElseIf fnct = 12 Then
    nin = 2
  ElseIf fnct > 12 And fnct < 19 Then
    nin = 1
  ElseIf fnct = 19 Or fnct = 20 Then
    nin = 2
  Else
    nin = 1
  End If
End Sub

Private Sub optOperation_Click(index As Integer)
  Dim newedat&(6), i&
  If index = 0 Then
    'compute
    Call GenerInit
  ElseIf index = 1 Then
    'set transform defaults for function frame
    nin = 1
    For i = 0 To 23
      optFunction(i).Visible = False
    Next i
    cmdInputFile.Visible = False
    txtC1.Visible = False
    txtC2.Visible = False
    lblC1.Visible = False
    lblC2.Visible = False
    lblSigFigs.Visible = False
    lblConstants.Visible = False
    lblExponent.Visible = False
    lblFunction.Visible = False
    'hide manual fields
    lblValue.Visible = False
    txtValue.Visible = False
    optFunction(24).Visible = False
    optFunction(25).Visible = False
    'show fields for transformation
    ctlGenerDate.TUnit = 4
    ctlGenerDate.TSTEP = 1
    mants = 1
    lblYear.Visible = True
    lblMonth.Visible = True
    lblDay.Visible = True
    txtYear.Visible = True
    txtMonth.Visible = True
    txtDay.Visible = True
    ctlGenerDate.TAggr = 1
    optFunction(24).Visible = False
    optFunction(25).Visible = False
    'set transform defaults for timeseries frame
    lblInput1.Visible = True
    cmdMainIn1.Visible = True
    txtInputDsn1.Visible = True
    lblScenIn1.Visible = True
    lblLocIn1.Visible = True
    lblConsIn1.Visible = True
    lblInput2.Visible = False
    cmdMainIn2.Visible = False
    txtInputDsn2.Visible = False
    lblScenIn2.Visible = False
    lblLocIn2.Visible = False
    lblConsIn2.Visible = False
    lblScenOut.Visible = True
    lblLocOut.Visible = True
    lblConsOut.Visible = True
    cmdMainOut.Visible = True
    txtOutputDsn.text = ""
    txtInputDsn1.text = ""
    txtInputDsn2.text = ""
    txtScenOut.text = ""
    txtLocOut.text = ""
    txtConsOut.text = ""
    lblScenOut.Caption = ""
    lblLocOut.Caption = ""
    lblConsOut.Caption = ""
    lblScenIn1.Caption = ""
    lblLocIn1.Caption = ""
    lblConsIn1.Caption = ""
    lblScenIn2.Caption = ""
    lblLocIn2.Caption = ""
    lblConsIn2.Caption = ""
    outdsn = 0
  ElseIf index = 2 Then
    'set manual defaults for function frame
    For i = 0 To 23
      optFunction(i).Visible = False
    Next i
    cmdInputFile.Visible = False
    txtC1.Visible = False
    txtC2.Visible = False
    lblC1.Visible = False
    lblC2.Visible = False
    lblSigFigs.Visible = False
    lblConstants.Visible = False
    lblExponent.Visible = False
    lblFunction.Visible = False
    'hide fields for transformation functions
    lblYear.Visible = False
    lblMonth.Visible = False
    lblDay.Visible = False
    txtYear.Visible = False
    txtMonth.Visible = False
    txtDay.Visible = False
    'show manual fields
    lblValue.Visible = True
    txtValue.Visible = True
    txtValue.text = "0.0"
    optFunction(24).Visible = True
    optFunction(25).Visible = True
    optFunction(24) = True
    optFunction(26).Visible = False
    optFunction(27).Visible = False
    fillval = 0#
    ctlGenerDate.TUnit = 4
    ctlGenerDate.TSTEP = 1
    mants = 1
    'set manual defaults for timeseries frame
    lblInput1.Visible = False
    cmdMainIn1.Visible = False
    txtInputDsn1.Visible = False
    lblScenIn1.Visible = False
    lblLocIn1.Visible = False
    lblConsIn1.Visible = False
    lblInput2.Visible = False
    cmdMainIn2.Visible = False
    txtInputDsn2.Visible = False
    lblScenIn2.Visible = False
    lblLocIn2.Visible = False
    lblConsIn2.Visible = False
    lblScenOut.Visible = True
    lblLocOut.Visible = True
    lblConsOut.Visible = True
    cmdMainOut.Visible = True
    txtOutputDsn.text = ""
    txtInputDsn1.text = ""
    txtInputDsn2.text = ""
    txtScenOut.text = ""
    txtLocOut.text = ""
    txtConsOut.text = ""
    lblScenOut.Caption = ""
    lblLocOut.Caption = ""
    lblConsOut.Caption = ""
    lblScenIn1.Caption = ""
    lblLocIn1.Caption = ""
    lblConsIn1.Caption = ""
    lblScenIn2.Caption = ""
    lblLocIn2.Caption = ""
    lblConsIn2.Caption = ""
    outdsn = 0
  End If
End Sub


Private Sub txtC1_GotFocus()
  c1old = txtC1.text
End Sub


Private Sub txtC1_LostFocus()
  Dim c1new$, chgflg&, outdsn&, dtype&
  If Len(txtC1.text) > 0 Then
    c1new = txtC1.text
    If optFunction(21) = True Then
      Call ChkTxtI("Sig Figs", 1, 10, c1new, k1, chgflg)
    Else
      Call ChkTxtR("Function Value", -100000000, 100000000, c1new, rc(0), chgflg)
    End If
    If chgflg <> 1 Then
      txtC1.text = c1old
    End If
  Else
    txtC1.text = c1old
  End If
End Sub


Private Sub txtC2_GotFocus()
  c2old = txtC2.text
End Sub

Private Sub txtC2_LostFocus()
  Dim c2new$, chgflg&, outdsn&, dtype&
  If Len(txtC2.text) > 0 Then
    c2new = txtC2.text
    Call ChkTxtR("Function Value", -100000000, 100000000, c2new, rc(1), chgflg)
    If chgflg <> 1 Then
      txtC2.text = c2old
    End If
  Else
    txtC2.text = c2old
  End If
End Sub


Private Sub txtConsOut_lostfocus()
  ocons = UCase(txtConsOut.text)
  txtConsOut.text = ocons
End Sub

Private Sub txtDay_GotFocus()
  oldday = txtDay.text
End Sub


Private Sub txtDay_LostFocus()
  Dim daynew$, chgflg&
  If Len(txtDay.text) > 0 Then
    daynew = txtDay.text
    Call ChkTxtI("Day", 1, 31, daynew, iday, chgflg)
    If chgflg <> 1 Then
      txtDay.text = oldday
    End If
  Else
    txtDay.text = oldday
  End If
End Sub


Private Sub txtInputDsn1_GotFocus()
  olddsnin1 = txtInputDsn1.text
End Sub


Private Sub txtInputDsn1_LostFocus()
  Dim newdsnin1$, ival&, chgflg&
  If Len(txtInputDsn1.text) > 0 Then
    newdsnin1 = txtInputDsn1.text
    Call ChkTxtI("Dsn", 1, 10000, newdsnin1, ival, chgflg)
    If chgflg <> 1 Then
      txtInputDsn1.text = olddsnin1
    Else
      in1dsn = ival
    End If
    Call CheckInputDsn1
  End If
End Sub


Private Sub txtInputDsn2_GotFocus()
  olddsnin2 = txtInputDsn2.text
End Sub


Private Sub txtInputDsn2_LostFocus()
  Dim newdsnin2$, ival&, chgflg&
  If Len(txtInputDsn2.text) > 0 Then
    newdsnin2 = txtInputDsn2.text
    Call ChkTxtI("Dsn", 1, 10000, newdsnin2, ival, chgflg)
    If chgflg <> 1 Then
      txtInputDsn2.text = olddsnin2
    Else
      in2dsn = ival
    End If
    Call CheckInputDsn2
  End If
End Sub


Private Sub txtLocOut_lostfocus()
  oloc = UCase(txtLocOut.text)
  txtLocOut.text = oloc
End Sub

Private Sub txtMonth_GotFocus()
    oldmonth = txtMonth.text
End Sub


Private Sub txtMonth_LostFocus()
  Dim monthnew$, chgflg&
  If Len(txtMonth.text) > 0 Then
    monthnew = txtMonth.text
    Call ChkTxtI("Month", 1, 12, monthnew, imonth, chgflg)
    If chgflg <> 1 Then
      txtMonth.text = oldmonth
    End If
  Else
    txtMonth.text = oldmonth
  End If
End Sub


Private Sub txtOutputDsn_GotFocus()
  olddsnout = txtOutputDsn.text
End Sub


Private Sub txtOutputDsn_LostFocus()
  Dim newdsnout$, ival&, chgflg&
  If Len(txtOutputDsn.text) > 0 Then
    newdsnout = txtOutputDsn.text
    Call ChkTxtI("Dsn", 1, 10000, newdsnout, ival, chgflg)
    If chgflg <> 1 Then
      txtOutputDsn.text = olddsnout
    Else
      outdsn = ival
    End If
    Call CheckOutDsn
  End If
End Sub


Private Sub txtScenOut_LostFocus()
  oscen = UCase(txtScenOut.text)
  txtScenOut.text = oscen
End Sub


Private Sub txtValue_GotFocus()
  valueold = txtValue.text
End Sub


Private Sub txtValue_LostFocus()
  Dim valuenew$, chgflg&, dtype&
  If Len(txtValue.text) > 0 Then
    valuenew = txtValue.text
    Call ChkTxtR("Function Value", -100000000, 100000000, valuenew, fillval, chgflg)
    If chgflg <> 1 Then
      txtValue.text = valueold
    End If
  Else
    txtValue.text = valueold
  End If
End Sub


Private Sub txtYear_GotFocus()
  oldyear = txtYear.text
End Sub


Private Sub txtYear_LostFocus()
  Dim yearnew$, chgflg&
  If Len(txtYear.text) > 0 Then
    yearnew = txtYear.text
    Call ChkTxtI("Year", 1900, 2100, yearnew, iyear, chgflg)
    If chgflg <> 1 Then
      txtYear.text = oldyear
    End If
  Else
    txtYear.text = oldyear
  End If
End Sub
