VERSION 5.00
Object = "{F9043C88-F6F2-101A-A3C9-08002B2F49FB}#1.2#0"; "COMDLG32.OCX"
Object = "{831FDD16-0C5C-11D2-A9FC-0000F8754DA1}#2.0#0"; "MSCOMCTL.OCX"
Object = "{872F11D5-3322-11D4-9D23-00A0C9768F70}#1.10#0"; "ATCoCtl.ocx"
Begin VB.Form frmGenScn 
   Caption         =   "WDMUtil"
   ClientHeight    =   6255
   ClientLeft      =   2880
   ClientTop       =   780
   ClientWidth     =   8055
   BeginProperty Font 
      Name            =   "MS Sans Serif"
      Size            =   8.25
      Charset         =   0
      Weight          =   700
      Underline       =   0   'False
      Italic          =   0   'False
      Strikethrough   =   0   'False
   EndProperty
   ForeColor       =   &H00000000&
   HelpContextID   =   18
   Icon            =   "WDMUtil.frx":0000
   LinkTopic       =   "Form1"
   PaletteMode     =   1  'UseZOrder
   Picture         =   "WDMUtil.frx":0442
   ScaleHeight     =   6255
   ScaleWidth      =   8055
   Begin VB.Frame fraSLC 
      Caption         =   "Constituents"
      Enabled         =   0   'False
      Height          =   1932
      HelpContextID   =   34
      Index           =   2
      Left            =   5520
      TabIndex        =   22
      Top             =   0
      Width           =   2652
      Begin VB.ListBox lstSLC 
         BackColor       =   &H00C0C0C0&
         Enabled         =   0   'False
         Height          =   255
         Index           =   2
         ItemData        =   "WDMUtil.frx":1F84
         Left            =   120
         List            =   "WDMUtil.frx":1F86
         MultiSelect     =   2  'Extended
         Sorted          =   -1  'True
         TabIndex        =   8
         Top             =   600
         WhatsThisHelpID =   5
         Width           =   2412
      End
      Begin VB.CommandButton cmdSLC 
         Caption         =   "None"
         Enabled         =   0   'False
         Height          =   252
         Index           =   5
         Left            =   1920
         TabIndex        =   10
         ToolTipText     =   "Select NO Constituents"
         Top             =   240
         Width           =   612
      End
      Begin VB.CommandButton cmdSLC 
         Caption         =   "All"
         Enabled         =   0   'False
         Height          =   252
         Index           =   2
         Left            =   1200
         TabIndex        =   9
         ToolTipText     =   "Select ALL Constituents"
         Top             =   240
         Width           =   612
      End
      Begin VB.Label lblslc 
         Enabled         =   0   'False
         Height          =   252
         Index           =   2
         Left            =   120
         TabIndex        =   23
         Top             =   240
         Width           =   1092
      End
   End
   Begin VB.Frame fraSLC 
      Caption         =   "Scenarios"
      Enabled         =   0   'False
      Height          =   1932
      HelpContextID   =   34
      Index           =   0
      Left            =   0
      TabIndex        =   0
      Top             =   0
      Width           =   2652
      Begin VB.CommandButton cmdSLC 
         Caption         =   "None"
         Enabled         =   0   'False
         Height          =   252
         Index           =   3
         Left            =   1920
         TabIndex        =   4
         ToolTipText     =   "Select NO Scenarios"
         Top             =   240
         Width           =   612
      End
      Begin VB.CommandButton cmdSLC 
         Caption         =   "All"
         Enabled         =   0   'False
         Height          =   252
         Index           =   0
         Left            =   1200
         TabIndex        =   3
         ToolTipText     =   "Select ALL Scenarios"
         Top             =   240
         Width           =   612
      End
      Begin VB.ListBox lstSLC 
         BackColor       =   &H00C0C0C0&
         Enabled         =   0   'False
         Height          =   255
         Index           =   0
         Left            =   120
         MultiSelect     =   2  'Extended
         Sorted          =   -1  'True
         TabIndex        =   2
         Top             =   600
         WhatsThisHelpID =   3
         Width           =   2412
      End
      Begin VB.Label lblslc 
         Enabled         =   0   'False
         Height          =   252
         Index           =   0
         Left            =   120
         TabIndex        =   21
         Top             =   240
         Width           =   1092
      End
   End
   Begin VB.Frame fraSLC 
      Caption         =   "Locations"
      Enabled         =   0   'False
      Height          =   1932
      HelpContextID   =   34
      Index           =   1
      Left            =   2760
      TabIndex        =   1
      Top             =   0
      Width           =   2652
      Begin VB.CommandButton cmdSLC 
         Caption         =   "None"
         Enabled         =   0   'False
         Height          =   252
         Index           =   4
         Left            =   1920
         TabIndex        =   7
         ToolTipText     =   "Select NO Constituents"
         Top             =   240
         Width           =   612
      End
      Begin VB.CommandButton cmdSLC 
         Caption         =   "All"
         Enabled         =   0   'False
         Height          =   252
         Index           =   1
         Left            =   1200
         TabIndex        =   6
         ToolTipText     =   "Select ALL Constituents"
         Top             =   240
         Width           =   612
      End
      Begin VB.ListBox lstSLC 
         BackColor       =   &H00C0C0C0&
         Enabled         =   0   'False
         Height          =   255
         Index           =   1
         Left            =   120
         MultiSelect     =   2  'Extended
         Sorted          =   -1  'True
         TabIndex        =   5
         Top             =   600
         WhatsThisHelpID =   5
         Width           =   2412
      End
      Begin MSComDlg.CommonDialog cdl 
         Left            =   2040
         Top             =   1680
         _ExtentX        =   688
         _ExtentY        =   688
         _Version        =   393216
      End
      Begin VB.Label lblslc 
         Enabled         =   0   'False
         Height          =   255
         Index           =   1
         Left            =   120
         TabIndex        =   28
         Top             =   240
         Width           =   1095
      End
   End
   Begin VB.Frame hSash 
      BorderStyle     =   0  'None
      Caption         =   "hSash"
      Height          =   60
      Left            =   0
      MousePointer    =   7  'Size N S
      TabIndex        =   29
      Top             =   2040
      Width           =   7455
   End
   Begin MSComDlg.CommonDialog cdlFile 
      Left            =   2520
      Top             =   1680
      _ExtentX        =   688
      _ExtentY        =   688
      _Version        =   393216
      FontSize        =   4.873e-37
   End
   Begin VB.Frame fraTimser 
      Caption         =   "Time Series"
      Enabled         =   0   'False
      Height          =   2655
      HelpContextID   =   35
      Left            =   0
      TabIndex        =   25
      Top             =   2160
      Width           =   8172
      Begin WDMUtil.ATCTimserGrid TimserGrid 
         Height          =   2292
         HelpContextID   =   35
         Left            =   120
         TabIndex        =   11
         Top             =   240
         Width           =   7932
         _ExtentX        =   13996
         _ExtentY        =   4048
      End
      Begin MSComDlg.CommonDialog CDFileList 
         Left            =   8880
         Top             =   2640
         _ExtentX        =   688
         _ExtentY        =   688
         _Version        =   393216
         FontSize        =   4.873e-37
      End
   End
   Begin VB.Frame fraAnal 
      Caption         =   "Tools"
      Enabled         =   0   'False
      Height          =   1330
      HelpContextID   =   54
      Left            =   5280
      TabIndex        =   24
      Top             =   4920
      Width           =   2772
      Begin VB.CommandButton cmdAna 
         Enabled         =   0   'False
         Height          =   480
         Index           =   7
         Left            =   840
         Picture         =   "WDMUtil.frx":1F88
         Style           =   1  'Graphical
         TabIndex        =   18
         Tag             =   "5"
         ToolTipText     =   "Generate Time Series"
         Top             =   780
         Width           =   520
      End
      Begin VB.CommandButton cmdAna 
         Enabled         =   0   'False
         Height          =   480
         Index           =   5
         Left            =   2040
         Picture         =   "WDMUtil.frx":2292
         Style           =   1  'Graphical
         TabIndex        =   16
         Tag             =   "3"
         ToolTipText     =   "Write Time Series to WDM"
         Top             =   240
         Width           =   520
      End
      Begin VB.CommandButton cmdAna 
         Enabled         =   0   'False
         Height          =   480
         Index           =   4
         Left            =   1440
         Picture         =   "WDMUtil.frx":26D4
         Style           =   1  'Graphical
         TabIndex        =   19
         Tag             =   "6"
         ToolTipText     =   "Export Time Series"
         Top             =   780
         Width           =   520
      End
      Begin VB.CommandButton cmdAna 
         Enabled         =   0   'False
         Height          =   480
         Index           =   1
         Left            =   240
         Picture         =   "WDMUtil.frx":298E
         Style           =   1  'Graphical
         TabIndex        =   13
         Tag             =   "0"
         ToolTipText     =   "Summarize Data"
         Top             =   240
         Width           =   520
      End
      Begin VB.CommandButton cmdAna 
         Enabled         =   0   'False
         Height          =   480
         Index           =   2
         Left            =   840
         Picture         =   "WDMUtil.frx":2DD0
         Style           =   1  'Graphical
         TabIndex        =   14
         Tag             =   "1"
         ToolTipText     =   "List/Edit Time Series"
         Top             =   240
         Width           =   520
      End
      Begin VB.CommandButton cmdAna 
         Enabled         =   0   'False
         Height          =   480
         Index           =   6
         Left            =   2040
         Picture         =   "WDMUtil.frx":30DA
         Style           =   1  'Graphical
         TabIndex        =   20
         Tag             =   "8"
         ToolTipText     =   "View a File"
         Top             =   780
         Width           =   520
      End
      Begin VB.CommandButton cmdAna 
         Enabled         =   0   'False
         Height          =   480
         Index           =   0
         Left            =   240
         Picture         =   "WDMUtil.frx":33E4
         Style           =   1  'Graphical
         TabIndex        =   17
         Tag             =   "4"
         ToolTipText     =   "Compute/Disaggregate Meteorological Time Series"
         Top             =   780
         Width           =   520
      End
      Begin VB.CommandButton cmdAna 
         Enabled         =   0   'False
         Height          =   480
         Index           =   3
         Left            =   1440
         Picture         =   "WDMUtil.frx":36EE
         Style           =   1  'Graphical
         TabIndex        =   15
         Tag             =   "2"
         ToolTipText     =   "Generate Graphs"
         Top             =   240
         Width           =   520
      End
   End
   Begin ATCoCtl.ATCoDate ctlGenDate 
      Height          =   1356
      HelpContextID   =   53
      Left            =   0
      TabIndex        =   12
      Top             =   4920
      Width           =   5280
      _ExtentX        =   9313
      _ExtentY        =   2381
      TUnit           =   4
      TAggr           =   1
      TStep           =   1
      CurrE           =   0
      CurrS           =   0
      LimtE           =   35795
      LimtS           =   33239
      DispL           =   4
      LabelCurrentRange=   "Current"
      TstepVisible    =   -1  'True
   End
   Begin MSComctlLib.ImageList imgListTools 
      Left            =   120
      Top             =   0
      _ExtentX        =   794
      _ExtentY        =   794
      BackColor       =   -2147483643
      ImageWidth      =   16
      ImageHeight     =   16
      MaskColor       =   8421376
      _Version        =   393216
      BeginProperty Images {2C247F25-8591-11D1-B16A-00C0F0283628} 
         NumListImages   =   23
         BeginProperty ListImage1 {2C247F27-8591-11D1-B16A-00C0F0283628} 
            Picture         =   "WDMUtil.frx":39F8
            Key             =   "Zoom"
         EndProperty
         BeginProperty ListImage2 {2C247F27-8591-11D1-B16A-00C0F0283628} 
            Picture         =   "WDMUtil.frx":3F4A
            Key             =   "ZoomIn"
         EndProperty
         BeginProperty ListImage3 {2C247F27-8591-11D1-B16A-00C0F0283628} 
            Picture         =   "WDMUtil.frx":449C
            Key             =   "ZoomOut"
         EndProperty
         BeginProperty ListImage4 {2C247F27-8591-11D1-B16A-00C0F0283628} 
            Picture         =   "WDMUtil.frx":49EE
            Key             =   "Pan"
         EndProperty
         BeginProperty ListImage5 {2C247F27-8591-11D1-B16A-00C0F0283628} 
            Picture         =   "WDMUtil.frx":4F40
            Key             =   "FullExtent"
         EndProperty
         BeginProperty ListImage6 {2C247F27-8591-11D1-B16A-00C0F0283628} 
            Picture         =   "WDMUtil.frx":5492
            Key             =   "Identify"
         EndProperty
         BeginProperty ListImage7 {2C247F27-8591-11D1-B16A-00C0F0283628} 
            Picture         =   "WDMUtil.frx":59E4
            Key             =   "Left"
         EndProperty
         BeginProperty ListImage8 {2C247F27-8591-11D1-B16A-00C0F0283628} 
            Picture         =   "WDMUtil.frx":5CFE
            Key             =   "Right"
         EndProperty
         BeginProperty ListImage9 {2C247F27-8591-11D1-B16A-00C0F0283628} 
            Picture         =   "WDMUtil.frx":6018
            Key             =   "Select"
         EndProperty
         BeginProperty ListImage10 {2C247F27-8591-11D1-B16A-00C0F0283628} 
            Picture         =   "WDMUtil.frx":6332
            Key             =   "Unselect"
         EndProperty
         BeginProperty ListImage11 {2C247F27-8591-11D1-B16A-00C0F0283628} 
            Picture         =   "WDMUtil.frx":664C
            Key             =   "Get"
         EndProperty
         BeginProperty ListImage12 {2C247F27-8591-11D1-B16A-00C0F0283628} 
            Picture         =   "WDMUtil.frx":6966
            Key             =   "Save"
         EndProperty
         BeginProperty ListImage13 {2C247F27-8591-11D1-B16A-00C0F0283628} 
            Picture         =   "WDMUtil.frx":6C80
            Key             =   "Print"
         EndProperty
         BeginProperty ListImage14 {2C247F27-8591-11D1-B16A-00C0F0283628} 
            Picture         =   "WDMUtil.frx":6F9A
            Key             =   "Add"
         EndProperty
         BeginProperty ListImage15 {2C247F27-8591-11D1-B16A-00C0F0283628} 
            Picture         =   "WDMUtil.frx":72B4
            Key             =   "Remove"
         EndProperty
         BeginProperty ListImage16 {2C247F27-8591-11D1-B16A-00C0F0283628} 
            Picture         =   "WDMUtil.frx":75CE
            Key             =   "Properties"
         EndProperty
         BeginProperty ListImage17 {2C247F27-8591-11D1-B16A-00C0F0283628} 
            Picture         =   "WDMUtil.frx":78E8
            Key             =   "Clear"
         EndProperty
         BeginProperty ListImage18 {2C247F27-8591-11D1-B16A-00C0F0283628} 
            Picture         =   "WDMUtil.frx":7C02
            Key             =   "Up"
         EndProperty
         BeginProperty ListImage19 {2C247F27-8591-11D1-B16A-00C0F0283628} 
            Picture         =   "WDMUtil.frx":7F1C
            Key             =   "Down"
         EndProperty
         BeginProperty ListImage20 {2C247F27-8591-11D1-B16A-00C0F0283628} 
            Picture         =   "WDMUtil.frx":8236
            Key             =   "Headers"
         EndProperty
         BeginProperty ListImage21 {2C247F27-8591-11D1-B16A-00C0F0283628} 
            Picture         =   "WDMUtil.frx":8550
            Key             =   "Move"
         EndProperty
         BeginProperty ListImage22 {2C247F27-8591-11D1-B16A-00C0F0283628} 
            Picture         =   "WDMUtil.frx":886A
            Key             =   "All"
         EndProperty
         BeginProperty ListImage23 {2C247F27-8591-11D1-B16A-00C0F0283628} 
            Picture         =   "WDMUtil.frx":8B84
            Key             =   "None"
         EndProperty
      EndProperty
   End
   Begin VB.Frame fraDumDates 
      Caption         =   "Dates"
      Enabled         =   0   'False
      Height          =   1335
      Left            =   0
      TabIndex        =   26
      Top             =   4920
      Width           =   5172
      Begin VB.Label lblDumDates 
         Caption         =   "<not set>."
         Enabled         =   0   'False
         Height          =   252
         Left            =   120
         TabIndex        =   27
         Top             =   360
         Width           =   4692
      End
   End
   Begin VB.Menu mnuX 
      Caption         =   "&File"
      Index           =   0
      Begin VB.Menu mnuNew 
         Caption         =   "&New"
      End
      Begin VB.Menu mnuOpen 
         Caption         =   "&Open"
      End
      Begin VB.Menu mnuImport 
         Caption         =   "&Import"
         Enabled         =   0   'False
      End
      Begin VB.Menu mnuSepx 
         Caption         =   "-"
      End
      Begin VB.Menu mnuEdit 
         Caption         =   "&Edit Project"
         Enabled         =   0   'False
         Visible         =   0   'False
      End
      Begin VB.Menu mnuSep0 
         Caption         =   "-"
         Visible         =   0   'False
      End
      Begin VB.Menu mnuClose 
         Caption         =   "&Close"
         Enabled         =   0   'False
      End
      Begin VB.Menu mnuSep1 
         Caption         =   "-"
      End
      Begin VB.Menu mnuSave 
         Caption         =   "&Save Project"
         Enabled         =   0   'False
         Index           =   0
         Visible         =   0   'False
      End
      Begin VB.Menu mnuSave 
         Caption         =   "Save Project &As"
         Enabled         =   0   'False
         Index           =   1
         Visible         =   0   'False
      End
      Begin VB.Menu mnuSep1B 
         Caption         =   "-"
         Visible         =   0   'False
      End
      Begin VB.Menu mnuRecordKeys 
         Caption         =   "&Record Keys"
         Shortcut        =   ^R
         Visible         =   0   'False
      End
      Begin VB.Menu mnuPlayKeys 
         Caption         =   "&Playback Keys"
         Shortcut        =   ^P
         Visible         =   0   'False
      End
      Begin VB.Menu mnuSep2 
         Caption         =   "-"
         Visible         =   0   'False
      End
      Begin VB.Menu mnuExit 
         Caption         =   "E&xit"
      End
   End
   Begin VB.Menu mnuX 
      Caption         =   "T&ools"
      Enabled         =   0   'False
      Index           =   1
      Begin VB.Menu mnuAnal 
         Caption         =   "&Summarize"
         Index           =   0
      End
      Begin VB.Menu mnuAnal 
         Caption         =   "&View/Edit"
         Index           =   1
      End
      Begin VB.Menu mnuAnal 
         Caption         =   "&Graph"
         Index           =   2
      End
      Begin VB.Menu mnuAnal 
         Caption         =   "&Write"
         Index           =   3
      End
      Begin VB.Menu mnuAnal 
         Caption         =   "&Compute"
         Index           =   4
      End
      Begin VB.Menu mnuAnal 
         Caption         =   "G&enerate"
         Index           =   5
      End
      Begin VB.Menu mnuAnal 
         Caption         =   "E&xport"
         Index           =   6
      End
      Begin VB.Menu mnuAnal 
         Caption         =   "&File"
         Index           =   8
      End
   End
   Begin VB.Menu mnuX 
      Caption         =   "&Scenarios"
      Enabled         =   0   'False
      Index           =   2
      Begin VB.Menu mnuSen 
         Caption         =   "&All"
         Index           =   0
      End
      Begin VB.Menu mnuSen 
         Caption         =   "&None"
         Index           =   1
      End
      Begin VB.Menu mnuSen 
         Caption         =   "&Focus"
         Index           =   2
      End
   End
   Begin VB.Menu mnuX 
      Caption         =   "&Locations"
      Enabled         =   0   'False
      Index           =   3
      Begin VB.Menu mnuloc 
         Caption         =   "&All"
         Index           =   0
      End
      Begin VB.Menu mnuloc 
         Caption         =   "&None"
         Index           =   1
      End
      Begin VB.Menu mnuloc 
         Caption         =   "&Focus"
         Index           =   2
      End
      Begin VB.Menu mnuloc 
         Caption         =   "&Properties"
         Index           =   3
      End
   End
   Begin VB.Menu mnuX 
      Caption         =   "&Constituents"
      Enabled         =   0   'False
      Index           =   5
      Begin VB.Menu mnuCon 
         Caption         =   "&All"
         Index           =   0
      End
      Begin VB.Menu mnuCon 
         Caption         =   "&None"
         Index           =   1
      End
      Begin VB.Menu mnuCon 
         Caption         =   "&Focus"
         Index           =   2
      End
      Begin VB.Menu mnuCon 
         Caption         =   "&Properties"
         Index           =   3
      End
   End
   Begin VB.Menu mnuX 
      Caption         =   "&Time Series"
      Enabled         =   0   'False
      Index           =   6
      Begin VB.Menu mnuTime 
         Caption         =   "&Add"
         Index           =   0
      End
      Begin VB.Menu mnuTime 
         Caption         =   "&Remove"
         Index           =   1
      End
      Begin VB.Menu mnuTime 
         Caption         =   "&Clear"
         Index           =   2
      End
      Begin VB.Menu mnuTime 
         Caption         =   "Move &Top"
         Index           =   3
      End
      Begin VB.Menu mnuTime 
         Caption         =   "Move &Up"
         Index           =   4
      End
      Begin VB.Menu mnuTime 
         Caption         =   "Move &Down"
         Index           =   5
      End
      Begin VB.Menu mnuTime 
         Caption         =   "Se&lect Columns"
         Index           =   6
      End
      Begin VB.Menu mnuTime 
         Caption         =   "&Save List"
         Index           =   7
      End
      Begin VB.Menu mnuTime 
         Caption         =   "&Get List"
         Index           =   8
      End
      Begin VB.Menu mnuTime 
         Caption         =   "&Edit Attributes"
         Index           =   9
      End
      Begin VB.Menu mnuTime 
         Caption         =   "&Interpolate"
         Index           =   10
      End
      Begin VB.Menu mnuTime 
         Caption         =   "&Delete"
         Index           =   11
      End
      Begin VB.Menu mnuTime 
         Caption         =   "Sa&ve Time Series"
         Index           =   12
      End
      Begin VB.Menu mnuTime 
         Caption         =   "&New Time Series"
         Index           =   13
      End
      Begin VB.Menu mnuTime 
         Caption         =   "&Focus"
         Index           =   14
      End
   End
   Begin VB.Menu mnuHelp 
      Caption         =   "&Help"
      NegotiatePosition=   3  'Right
      Begin VB.Menu mnuHelpAbout 
         Caption         =   "&About"
      End
      Begin VB.Menu mnuHelpContents 
         Caption         =   "&Contents"
      End
      Begin VB.Menu mnuHelpFeedback 
         Caption         =   "Send &Feedback"
      End
      Begin VB.Menu mnuHelpDebug 
         Caption         =   "&Debug"
      End
   End
End
Attribute VB_Name = "frmGenScn"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
'Copyright 2000 by AQUA TERRA Consultants

'date widget stuff
Dim DateBase&, strendfg%, lftAdj&, widAdj&, inc#

'Dim ExePath$ - now defined in Gentry.bas

Dim MapRefresh As Boolean

Dim hSashDragging As Boolean
'Dim ListEditing As ATCoList
Private WithEvents tsList As ATCoTSDisp.ATCoTSlist
Attribute tsList.VB_VarHelpID = -1

Public Sub DispDates(OnFlg As Boolean)
    If (OnFlg) Then
      fraDumDates.Visible = False
      ctlGenDate.Visible = False
      ctlGenDate.Visible = True
      ctlGenDate.SetFocus
    Else
      ctlGenDate.Visible = False
      fraDumDates.Visible = True
    End If
End Sub

Private Sub GetCommonDates()
  Dim Strt&(), Stp&(), TUnits&(), TSteps&(), lb&(6), le&(6)
  Dim TUnit&, TSTEP&
  Dim gpflg&, tdsfrc&, retcod&
  Dim j&, i&, k&, d$, jm1
  Dim FirstStart As Double, LastEnd As Double, ThisStart As Double, ThisEnd As Double
  Dim STSer As Collection
  Set STSer = TSerSelected
  
  FirstStart = 1E+30
  LastEnd = -1E+30
  
  If STSer.Count = 0 Then
    Call DispDates(False)
    lblDumDates.Caption = "No Dates are available until Timeseries are Selected"
  Else
    ReDim TUnits(STSer.Count)
    ReDim TSteps(STSer.Count)
    ReDim Strt(6 * STSer.Count)
    ReDim Stp(6 * STSer.Count)
  
    gpflg = 1
    TUnit = 6
    For j = 1 To STSer.Count
      jm1 = j - 1 'index for FORTRAN arrays
      ThisStart = STSer(j).Dates.Summary.SJDay
      ThisEnd = STSer(j).Dates.Summary.EJDay
      
      If ThisStart < FirstStart Then FirstStart = ThisStart
      If ThisEnd > LastEnd Then LastEnd = ThisEnd
      
      Call J2Date(ThisStart, lb)
      Call J2Date(ThisEnd, le)
      For i = 0 To 5
        k = i + jm1 * 6
        Strt(k) = lb(i)
        Stp(k) = le(i)
      Next i
      TSteps(jm1) = STSer(j).Dates.Summary.ts
      TUnits(jm1) = STSer(j).Dates.Summary.Tu
    Next j
    Call DTMCMN(Strt, Stp, TSteps, TUnits, CSDat, CEDat, TSTEP, TUnit, retcod)
    Call J2Date(FirstStart, lb)
    Call J2Date(LastEnd, le)
    
    If retcod < 0 Then
      'lblDumDates.Caption = "No Common Period for Selected Timeseries"
      'Call DispDates(False)
      For i = 0 To 5
        CSDat(i) = lb(i)
        CEDat(i) = le(i)
      Next i
      retcod = 0
    End If
    If retcod = 0 Then
      If CSDat(0) = 0 And CEDat(0) = 0 Then
        'no data in these data sets
        Call DispDates(False)
        lblDumDates.Caption = "No Data Available in Selected Timeseries."
      Else
        Call timcnv(CEDat)
        Call timcnv(le)
        CSDat(3) = 0: CSDat(4) = 0: CSDat(5) = 0
        CEDat(3) = 0: CEDat(4) = 0: CEDat(5) = 0
    
        If TUnit < ctlGenDate.TUnit Then
          ctlGenDate.TUnit = TUnit
        End If
        ctlGenDate.LimtS = DateSerial(lb(0), lb(1), lb(2)) ' 1)
        ctlGenDate.LimtE = DateSerial(le(0), le(1), le(2)) ' daymon(CEDat(0), CEDat(1))) '
        ctlGenDate.CommS = DateSerial(CSDat(0), CSDat(1), CSDat(2)) ' 1)
        ctlGenDate.CommE = DateSerial(CEDat(0), CEDat(1), CEDat(2)) ' daymon(CEDat(0), CEDat(1))) '
        If ctlGenDate.CurrS < ctlGenDate.LimtS Then
          ctlGenDate.CurrS = ctlGenDate.LimtS
        End If
        If ctlGenDate.CurrE > ctlGenDate.LimtE Then
          ctlGenDate.CurrE = ctlGenDate.LimtE
        End If
        ctlGenDate.Width = fraDumDates.Width
        Call DispDates(True)
      End If
    End If
  End If
End Sub

Sub ManageProject(i%)

    If i = 0 Then 'new project
'      Unload frmWDMUManageProject
'      frmWDMUManageProject.Show
    ElseIf i = 1 Then 'open existing project
      Call OpenStatusFile
      p.EditFlg = False
      frmGenScn.SetFocus
    ElseIf i = 2 Then 'edit project
'      Unload frmWDMUManageProject
'      frmWDMUManageProject.Show
    End If

End Sub

Public Sub RefreshMain()
  Dim ObservedFlag As Boolean, i&
  On Error Resume Next 'some controls no longer exist, just ignore that
  lstSLC(2).Clear 'constituents
  For i = 0 To p.ConsCount - 1
    lstSLC(2).AddItem p.Cons(i).Name
  Next i
  lstSLC_GotFocus (2)
  lstSLC(1).Clear 'locations
  For i = 0 To p.LocnCount - 1
    lstSLC(1).AddItem p.Locn(i).Name
  Next i
  lstSLC_GotFocus (1)
  lstSLC(0).Clear 'scenarios
  For i = 0 To p.ScenCount - 1
    lstSLC(0).AddItem p.Scen(i).Name
  Next i
  lstSLC_GotFocus (0)
  TimserGrid.Enabled = True
  agdDSNUpdate
  For i = 0 To cmdSLC.Count - 1 'slc all none
    cmdSLC(i).Enabled = True
  Next i
  For i = 0 To 2
    lstSLC(i).Enabled = True
    lstSLC(i).BackColor = &HFFFFFF
    If i < 3 Then lblslc(i).Enabled = True
  Next i
  fraAnal.Enabled = True
  fraDumDates.Enabled = True
  For i = 0 To fraSLC.Count 'scenario, location and constituent
    fraSLC(i).Enabled = True
  Next i
  fraTimser.Enabled = True
'  For i = 1 To tbrTimser.Buttons.Count
'    tbrTimser.Buttons(i).Enabled = True
'  Next i
  For i = 0 To cmdAna.Count - 1
    cmdAna(i).Enabled = True
  Next i
  lblDumDates.Enabled = True
  mnuClose.Enabled = True
  mnuSave(0).Enabled = True
  mnuSave(1).Enabled = True
  mnuEdit.Enabled = True
'  mnuOpen.Enabled = False
'  mnuNew.Enabled = False
'  mnuX(1).Visible = True
'  mnuX(2).Visible = True
'  mnuX(3).Visible = True
'  mnuX(5).Visible = True
'  mnuX(6).Visible = True
  mnuX(1).Enabled = True
  mnuX(2).Enabled = True
  mnuX(3).Enabled = True
  mnuX(5).Enabled = True
  mnuX(6).Enabled = True
  On Error GoTo 0
End Sub

Private Function RepAll(S$) As String
               
   If Len(S) > 0 Then
     RepAll = S
   Else
     RepAll = "ALL"
   End If

End Function

Sub SyncSLC(S$, l$, c$)
    Dim i%, j%, t$
    
    For j = 0 To 2 Step 2
      If j = 0 Then
        t = S
      ElseIf j = 1 Then
        t = l
      Else
        t = c
      End If
      For i = 0 To lstSLC(j).ListCount - 1
        If t = lstSLC(j).List(i) Then
          If Not (lstSLC(j).Selected(i)) Then
            lstSLC(j).Selected(i) = True
            i = lstSLC(j).ListCount
          End If
        End If
      Next i
    Next j
    
End Sub

Public Sub RefreshTSList()
  agdDSNUpdate
End Sub
Public Function OpenHspfMsg(ByVal S$) As Boolean
  Dim n$, i&, fno$, try&, sPath$, parsePos&
  OpenHspfMsg = True
  If p.HSPFMsg.Unit = 0 Then
    try = 1
    
    n = S
    If Len(Dir(n)) > 0 Then GoTo OpenN
    
    fno = FilenameOnly(S) & ".wdm"
TryNext:
    Select Case try
      Case 1: fno = FilenameOnly(S) & ".wdm"
      Case 2: fno = "hspfmsg.wdm"
      Case 3: fno = "message.wdm"
      Case 4:
        If MsgBox("Could not find HSPF message file indicated (" & S & ")" & vbCr & "Do you want to browse for this file?", vbYesNo) = vbYes Then
          With cdl
            .DialogTitle = "Open HSPF message file"
            .CancelError = False
            .flags = &H1804& 'not read only
            .Filter = "WDM files (*.wdm)|*.wdm"
            .Filename = fno
            .ShowOpen
            n = .Filename
          End With
        End If
        If Len(Dir(n)) > 0 Then GoTo OpenN
        OpenHspfMsg = False
        Exit Function
      Case Else: 'Tried everything and still didn't find it
        OpenHspfMsg = False
        Exit Function
    End Select
    
    n = fno
    If Len(Dir(n)) > 0 Then GoTo OpenN
    
    sPath = PathNameOnly(S)
    GoSub SearchParentDirs
    
    sPath = ExePath
    GoSub SearchParentDirs
    
    n = ExePath & "bin\" & fno
    If Len(Dir(n)) > 0 Then GoTo OpenN
    
    n = ExePath & "bin_data\" & fno
    If Len(Dir(n)) > 0 Then GoTo OpenN
    
    n = "C:\WINNT\System32\" & fno
    If Len(Dir(n)) > 0 Then GoTo OpenN
    
    try = try + 1
    GoTo TryNext
    
SearchParentDirs:
    parsePos = InStrRev(sPath, "\")
    While parsePos > 0
      sPath = Left(sPath, parsePos - 1)
      n = sPath & "\" & fno
      If Len(Dir(n)) > 0 Then GoTo OpenN
      parsePos = InStrRev(sPath, "\")
    Wend
    Return
    
OpenN:
    i = 1
    p.HSPFMsg.Unit = F90_WDBOPN(i, n, Len(n))
    If p.HSPFMsg.Unit > 0 Then
      p.HSPFMsg.Name = n
    End If
  End If
End Function

Public Sub OpenStatusFile()
    Dim i&, x%, S$, istr$, d$, h$, StrTyp$, fun%
    On Error GoTo errhandler
    dbg "OpenStatusFile:", 7, "WDMUtil", "i"
    x = 1
    Call InitATCoTSer

'    Status_Name.DialogTitle = "WDMUtil File Open Project"
'    Status_Name.CancelError = True
'    Status_Name.Flags = &H1804& 'not read only
'    Status_Name.Filter = "Project files (*.sta)|*.sta"
'    Status_Name.ShowOpen
'    p.StatusFileName = Status_Name.filename
    dbg "OpenStatusFile:name:" & p.StatusFileName, 3, "WDMUtil", "t"
        
    MousePointer = ccHourglass
    'may need to close files if some already open
    'If p.WDMFiles(1).FileUnit > 0 Then
    '  dbg "OpenStatusFile:Close open WDMData on " & p.WDMFiles(1).FileUnit, 7, "WDMUtil", "t"
      'wdm file already open
    '  i = F90_WDFLCL(p.WDMFiles(1).FileUnit)
    'End If

    'dbg "OpenStatusFile:Reset RDB data to none", 7, "WDMUtil", "t"
    ReDim p.Scen(0)
    p.ScenCount = 0
    ReDim p.Cons(0)
    p.ConsCount = 0
    ReDim p.Locn(0)
    p.LocnCount = 0

    x = 2
    fun = FreeFile(0)
    Open p.StatusFileName For Input As #fun
    p.StatusFilePath = CurDir
    dbg "OpenStatusFile:open file and set CurDir to " & CurDir, 3, "WDMUtil", "t"

    While Not EOF(fun)
      Line Input #fun, istr
      dbg "OpenStatusFile:read:" & istr, 8, "WDMUtil", "t"
      istr = Trim(istr)
      StrTyp = StrRetRem(istr)
'      If StrTyp = "WDM" Then
'        'WDM timeseries data file
'        dbg "OpenStatusFile:WDMData:" & istr, 5, "WDMUtil", "t"
'        x = 3
'        'skip wdm id for now
'        s = StrRetRem(istr)
'        p.WDMFiles(1).filename = istr
'        i = 0
'        p.WDMFiles(1).FileUnit = F90_WDBOPN(i, p.WDMFiles(1).filename, Len(p.WDMFiles(1).filename))
'        Call F90_WDIINI
'        Call F90_WIDADD(p.WDMFiles(1).FileUnit, 10000, "WDM1", Len("WDM1"))
'        If p.WDMFiles(1).FileUnit = 0 Then
'          s = p.WDMFiles(1).FileUnit
'          Err.Raise 103
'        End If
'        On Error Resume Next
'        Call F90_TSDRRE(p.WDMFiles(1).FileUnit, 0&, 0&)
'        On Error GoTo errhandler
'      ElseIf StrTyp = "RDB" Then
'        p.RDBCount = p.RDBCount + 1
'        dbg "OpenStatusFile:RDBData: " & p.RDBCount & " " & istr, 5, "WDMUtil", "t"
'        i = UBound(RDBData)
'        RDBData(i).NameRDB = istr
'        ReDim Preserve RDBData(i + 1)
''        Call RDBRead(RDBData(i))
'      Else
      If StrTyp = "CON" Then
        dbg "OpenStatusFile:CON Data: " & p.ConsCount + 1 & " " & istr, 5, "WDMUtil", "t"
        ReDim Preserve p.Cons(p.ConsCount)
        S = StrRetRem(istr)
        If Len(S) > 0 Then dbg "    CON Name: " & S, 6, "WDMUtil", "t"
        p.Cons(p.ConsCount).Name = S
        S = StrRetRem(istr)
        If Len(S) > 0 Then dbg "    CON Desc: " & S, 6, "WDMUtil", "t"
        p.Cons(p.ConsCount).desc = S
        p.ConsCount = p.ConsCount + 1
      ElseIf StrTyp = "LOC" Then
        dbg "OpenStatusFile:LOC Data: " & p.LocnCount + 1 & " " & istr, 5, "WDMUtil", "t"
        ReDim Preserve p.Locn(p.LocnCount)
        S = StrRetRem(istr)
        If Len(S) > 0 Then dbg "    LOC Name: " & S, 6, "WDMUtil", "t"
        p.Locn(p.LocnCount).Name = S
        S = StrRetRem(istr)
        If Len(S) > 0 Then dbg "    LOC Desc: " & S, 6, "WDMUtil", "t"
        p.Locn(p.LocnCount).desc = S
        p.LocnCount = p.LocnCount + 1
'        lblLocationsSelected.Caption = map1.PointsSelected " of " & p.LocnCount
      ElseIf StrTyp = "SCN" Then
        dbg "OpenStatusFile:SCN Data: " & p.ScenCount + 1 & " " & istr, 5, "GenScn", "t"
        ReDim Preserve p.Scen(p.ScenCount)
        S = StrRetRem(istr)
        If Len(S) > 0 Then dbg "    SCN Name: " & S, 6, "GenScn", "t"
        p.Scen(p.ScenCount).Name = S
        S = StrRetRem(istr)
        If Len(S) > 0 Then dbg "    SCN Type: " & S, 6, "GenScn", "t"
        p.Scen(p.ScenCount).Type = S
        S = StrRetRem(istr)
        If Len(S) > 0 Then dbg "    SCN File: " & S, 6, "GenScn", "t"
        p.Scen(p.ScenCount).File = S
        S = StrRetRem(istr)
        If Len(S) > 0 Then dbg "    SCN Desc: " & S, 6, "GenScn", "t"
        p.Scen(p.ScenCount).desc = S
        p.ScenCount = p.ScenCount + 1
      ElseIf StrTyp = "" Then
         'ignore blank lines
      Else 'atcotimser
        Dim TserIndex&, newClsTserFile As ATCclsTserFile
        dbg "OpenStatusFile:ATType" & StrTyp, 3, "WDMUtil", "t"
        TserIndex = TserFiles.AvailIndexByName("clsTSer" & StrTyp)
        If TserIndex = 0 Then 'not a valid type
          MsgBox StrTyp & " Read Error: " & TserFiles.ErrorDescription & vbCrLf & "Invalid Type " & StrTyp, vbExclamation
        Else 'a valid type, create an instance
          Dim wdmFlg As Boolean
          If Left(istr, 3) = "WDM" Then
            S = StrRetRem(istr)
          End If
          If InStr(UCase(istr), "WDM") Then
            wdmFlg = True
          Else
            wdmFlg = False
          End If
          TserFiles.Create TserIndex
          'make monitor available
          Set newClsTserFile = TserFiles.CurrentActive.obj
          Set newClsTserFile.Monitor = IPC
          newClsTserFile.HelpFileName = App.HelpFile
          If wdmFlg Then TserFiles.CurrentActive.obj.MsgUnit = p.HSPFMsg.Unit
          'read the file
          newClsTserFile.Filename = istr
          'check read errors
          S = newClsTserFile.ErrorDescription
          If Len(S) > 0 Then 'had a problem
            MsgBox StrTyp & " Read Error: " & S, vbExclamation
            Call TserFiles.Delete(TserFiles.CurrentActiveIndex)
          ElseIf wdmFlg Then
            p.WDMFiles.Add newClsTserFile
          End If
        End If
      End If
    Wend
    On Error GoTo 0
    Call RefreshSLC
    Call RefreshMain
    Call SelectAll
    If TimserGrid.VisibleList.Count > 0 Then
      TimserGrid.SelectRow 1, True
      agdDSNUpdate
    End If
    Me.Caption = Me.Caption & ": " & FilenameOnly(p.StatusFileName)
    Close #fun
    MousePointer = ccDefault
Exit Sub
errhandler:
    MousePointer = ccDefault
    h = "WDMUtil Initialization Problem"
    dbg "OpenStatusFile:Problem: " & x & " " & istr, 2, "WDMUtil", "e"
    If x = 1 Then 'user pressed Cancel of open of status
      'MsgBox "You must choose the name of a status file.", 48, h
    ElseIf x = 2 Then
      MsgBox "Need to go to Create new project file here.", 48, h
      End
    ElseIf x = 3 Then 'bad wdm file
      MsgBox "Could not find WDM file " & S, 48, h
      End
    ElseIf x = 4 Then 'problem with map database
      MsgBox "Could not find Map file " & p.MapName, 48, h
    ElseIf x = 5 Then
      MsgBox "Map:" & p.MapName & "  Error:" & Err.Description, 48, h
    End If
    Close #1
End Sub
Public Sub SaveStatusFile()
    Dim S$, i%, fl%
    On Error GoTo errhandler
   
    fl = FreeFile(0)
    Open p.StatusFileName For Output As #fl
    'WDM data file name
    S = "WDM WDM1 " & p.WDMFiles(1).Filename
    Print #fl, S
    'rdb files
'    For i = 0 To p.RDBCount - 1
'      s = "RDB " & RDBData(i).NameRDB
'      Print #fl, s
'    Next i
    'constituents
    For i = 0 To p.ConsCount - 1
      S = "CON " & p.Cons(i).Name
      S = S & ",'" & p.Cons(i).desc & "'"
      Print #fl, S
    Next i
    'locations
    For i = 0 To p.LocnCount - 1
      S = "LOC " & p.Locn(i).Name
      S = S & ",'" & p.Locn(i).desc & "'"
      Print #fl, S
    Next i
    
    Close #fl
    
    p.EditFlg = False
    
Exit Sub
errhandler:
    MsgBox "Error opening project file " & p.StatusFileName, 48, "WDMUtil Project File Save Problem"
End Sub
Private Sub SaveStatusFileAs()
    On Error GoTo 10
    cdlFile.DialogTitle = "WDMUtil File Save Project As"
    cdlFile.CancelError = True
    cdlFile.flags = &H3804& 'create & not read only
    cdlFile.Filter = "Project files (*.sta)|*.sta"
    cdlFile.Filename = p.StatusFileName
    cdlFile.ShowSave
    p.StatusFileName = cdlFile.Filename
    Call SaveStatusFile
    Me.Caption = "WDMUtil: " & FilenameOnly(p.StatusFileName)
10 'continue
End Sub
Public Sub SelectAll()

    'force selection of all time series
    Dim i%

    For i = 3 To 5 '3 to 5 for None buttons, 0 To 2 for All buttons
      cmdSLC_Click (i)
    Next i
'    Call TimeSeriesListManage(0, "Add")
    Call TimserGrid.ManageTimeSeriesList(0, "Add")

End Sub
Sub SyncLists(l As ListBox, m As ListBox)
   Dim i%, j%
   If l.ListCount > 0 Then
     For i = 0 To l.ListCount - 1
       For j = 0 To m.ListCount - 1
         If l.List(i) = m.List(j) Then
           m.Selected(j) = l.Selected(i)
         End If
       Next j
     Next i
   End If
End Sub


Sub UpdateCount(l As ListBox, t As Label, i%)
    Dim S$
    
    S = l.SelCount & " of " & l.ListCount
    t.Caption = S
    dbg "UpdateCount:" & sSLC(i) & ": " & S, 5, "WDMUtil", "t"

End Sub

Sub UpdateSLC(i As Integer, opt As Integer)
  Dim S$, j%, k%, lA As ListBox, lb As ListBox

  If MapRefresh Then MousePointer = ccHourglass
'  If lstSLC(i).SelCount = lstSLC(i).ListCount Then
'    k = -1
'  Else
    k = lstSLC(i).SelCount
'  End If
  dbg "UpdateSLC: " & sSLC(i) & ":" & i & " " & opt & " " & k, 8, "WDMUtil", "t"
  If i = 0 Or i = 3 Then
    ReDim specSen(Abs(k))
    scntSen = Abs(k) - 1
    If k < 0 Then
      specSen(0) = ""
    End If
  ElseIf i = 1 Then
    ReDim specLoc(Abs(k))
    scntLoc = Abs(k) - 1
    If k < 0 Then
      specLoc(0) = ""
    End If
  ElseIf i = 2 Or i = 4 Then
    ReDim specCon(Abs(k))
    scntCon = Abs(k) - 1
    If k < 0 Then
      specCon(0) = ""
    End If
  Else 'site
    '?
  End If
 
  If k > 0 Then
    k = -1
    For j = 0 To lstSLC(i).ListCount - 1
      If lstSLC(i).Selected(j) Then
        k = k + 1
        If i = 0 Then
          specSen(k) = lstSLC(i).List(j)
        ElseIf i = 1 Then
          specLoc(k) = lstSLC(i).List(j)
        ElseIf i = 2 Then
          specCon(k) = lstSLC(i).List(j)
        End If
      End If
    Next j
  End If

  Set lA = lstSLC(i)
  If i < 4 Then
    j = i Mod 3
  Else
    j = 2
  End If
  Call UpdateCount(lA, lblslc(j), j)
  If i > 2 Then
    Set lb = lstSLC(j)
    Call SyncLists(lA, lb)
  End If
  If MapRefresh Then MousePointer = ccDefault
End Sub

Private Sub agdDSNUpdate()
    Dim S$, i%
    i = TimserGrid.VisibleList.Count 'agdDSN.MaxOccupiedRow
    If i = 0 Then
      'agdDSN.BackColor = 12632256 '.Enabled = False
    Else
      'agdDSN.BackColor = 16777215 '.Enabled = True
    End If
    S = i & " of " & TSerAll.Count & " available time series in list (" & InMemFile.DataCount & " not on WDM file);  "
    fraTimser.Caption = "Time Series  -  " & S & TSerSelected.Count & " selected."
    Call GetCommonDates

End Sub

Private Sub cmdAna_Click(index As Integer)
  Call DoAnal(cmdAna(index).Tag)
End Sub

Private Sub cmdSLC_Click(index As Integer)
   Dim i%, j%, lA As ListBox, lb As ListBox
   Dim n As Boolean
   
   dbg "cmdSLC:Click: " & index, 3, "WDMUtil", "m"
   MapRefresh = False
   MousePointer = ccHourglass
   If index < 3 Then
     n = True
   Else
     n = False
   End If
   i = index Mod 3
   If i = 0 Then 'scenario
'     If optSLC(0).Value = True Then
       Set lA = lstSLC(0)
'       Set lb = lstSLC(3)
'     Else
'       Set la = lstSLC(3)
'       Set lb = lstSLC(0)
'     End If
   ElseIf i = 2 Then 'cons
'     If optSLC(2).Value = True Then
       Set lA = lstSLC(2)
'       Set lb = lstSLC(4)
'     Else
'       Set la = lstSLC(4)
'       Set lb = lstSLC(2)
'     End If
   Else 'loc
'     Exit Sub     'Set la = lstSLC(1)
     Set lA = lstSLC(1)
   End If
   lA.Visible = False
   For j = 0 To lA.ListCount - 1
     lA.Selected(j) = n
   Next j
   lA.Visible = True
   If n = False Then
     lA.TopIndex = 0
   End If
   Call UpdateCount(lA, lblslc(i), i)
'   If i = 0 Then
'     Call SyncLists(la, lb) 'scen
'   ElseIf i = 2 Then
'     Call SyncLists(la, lb) 'con
'   End If
   MousePointer = ccDefault
'   MapRefresh = True
   'If i = 1 Then 'loc
     Call UpdateSLC(i, 0)
   'End If
End Sub

Private Sub ctlGenDate_LostFocus()
    CSDat(0) = Year(ctlGenDate.CurrS)
    CSDat(1) = Month(ctlGenDate.CurrS)
    CSDat(2) = Day(ctlGenDate.CurrS)
    CSDat(3) = 0
    CSDat(4) = 0
    CSDat(5) = 0
    CEDat(0) = Year(ctlGenDate.CurrE)
    CEDat(1) = Month(ctlGenDate.CurrE)
    CEDat(2) = Day(ctlGenDate.CurrE)
    CEDat(3) = 24
    CEDat(4) = 0
    CEDat(5) = 0
'    If CSDat(0) > ComSDat(0) Or CSDat(1) > ComSDat(1) Or CSDat(2) > ComSDat(2) Then
'      'if beyond start yr, mo, or day, make sure hour is set to 0
'      CSDat(3) = 0
'    Else 'at start yr, mo, day, set hour to common start
'      CSDat(3) = ComSDat(3)
'    End If
    ctunit = ctlGenDate.TUnit
    CTStep = ctlGenDate.TSTEP
    CDTran = ctlGenDate.TAggr
    If CDTran < 2 Then ' kludge for diff convention in ATCoDate
      CDTran = Abs((CDTran - 1)) Mod 2
    End If
End Sub

Private Sub Form_Resize()
  Dim fh&, fw&, fra&, lstwid&, lsthgt&, Left&
  fh = frmGenScn.ScaleHeight
  fw = frmGenScn.ScaleWidth

  'Widths
  lstwid = (fw - 216) / 3
  If lstwid > 250 Then
    For fra = 0 To 2
      fraSLC(fra).Width = lstwid
      lstSLC(fra).Width = lstwid - 240
      If fra > 0 Then fraSLC(fra).Left = fraSLC(fra - 1).Left + lstwid + 105
    Next fra
  End If
  fraTimser.Width = fw
  'If fw > 150 Then fraTimser.Width = fw - 96
  If fraTimser.Width > 300 Then TimserGrid.Width = fraTimser.Width - 240
  
  'Heights
  lsthgt = fraSLC(0).Height - 700
  If lsthgt > 100 Then
    For fra = 0 To 2
      lstSLC(fra).Height = lsthgt
    Next fra
  End If
  ctlGenDate.Top = fh - 1332
  fraAnal.Top = ctlGenDate.Top
  fraDumDates.Top = ctlGenDate.Top
  If (ctlGenDate.Top - 200) > fraTimser.Top Then fraTimser.Height = ctlGenDate.Top - fraTimser.Top - 105
  'If fraTimser.Height > 900 Then LstDsn.Height = fraTimser.Height - 850) / 2
  'If fraTimser.Height > 900 Then agdDSN.Height = fraTimser.Height - 850 'LstDsn.Height: agdDSN.Top = LstDsn.Top + LstDsn.Height
  If fraTimser.Height > 360 Then TimserGrid.Height = fraTimser.Height - 360

End Sub

Private Sub Form_Unload(Cancel As Integer)

  Dim i&, j&

  If InMemFile.DataCount > 0 Then
    i = MsgBox("You have read data into the system, but not saved it to the WDM file." & vbCrLf & "Are you sure you want to exit WDMUtil?", 324, "WDMUtil Exit")
  Else
    i = vbYes
  End If
  If i = vbYes Then 'ok to exit program
    For j = TserFiles.Active.Count To 1 Step -1
      TserFiles.Delete (j)
    Next j
    i = F90_WDFLCL(p.HSPFMsg.Unit)
    Call F90_W99CLO
    TserFiles.Clear
    Set TserFiles = Nothing
    If RunningVB Then
      IPC.ExitProcess "Status"
      End
    Else
      IPC.SendMonitorMessage "TERMINATE"
      End
    End If
  Else 'don't exit yet
    Cancel = 1
  End If

End Sub

Private Sub hSash_MouseDown(Button As Integer, Shift As Integer, x As Single, y As Single)
  hSashDragging = True
End Sub

Private Sub hSash_MouseMove(Button As Integer, Shift As Integer, x As Single, y As Single)
  If hSashDragging And (hSash.Top + y) > 0 And (hSash.Top + y < Height) Then
    hSash.Top = hSash.Top + y
    fraTimser.Top = hSash.Top + hSash.Height
    If hSash.Top > 100 Then
      Dim fra&, frahgt&
      frahgt = hSash.Top
      For fra = 0 To 2
        fraSLC(fra).Height = frahgt
      Next fra
    End If
    Form_Resize
  End If
End Sub

Private Sub hSash_MouseUp(Button As Integer, Shift As Integer, x As Single, y As Single)
  hSashDragging = False
End Sub

Private Sub lstSLC_Click(index As Integer)
  dbg "lstSLC:Click: " & index & ":" & sSLC(index), 4, "WDMUtil", "m"
  If lstSLC(index).Visible Then
    lstSLC_GotFocus (index)
  End If
End Sub

Public Sub lstSLC_GotFocus(i As Integer)
  Call UpdateSLC(i, 0)
End Sub

Private Sub lstSLC_MouseMove(index As Integer, Button As Integer, Shift As Integer, x As Single, y As Single)
  Dim topitem&, itemheight!, itemnum&
  topitem = lstSLC(index).TopIndex 'SendMessage(lstSLC(Index).hwnd, LB_GETTOPINDEX, 0, 0)
  itemheight = SendMessage(lstSLC(index).hwnd, LB_GETITEMHEIGHT, 0, 0)
  itemnum = topitem - 0.4 + y / (Screen.TwipsPerPixelY * itemheight)
  If index = 0 Or index = 3 Then lstSLC(index).ToolTipText = GetDescription("SCN", lstSLC(index).List(itemnum))
  If index = 1 Then lstSLC(index).ToolTipText = GetDescription("LOC", lstSLC(index).List(itemnum))
  If index = 2 Or index = 4 Then lstSLC(index).ToolTipText = GetDescription("CON", lstSLC(index).List(itemnum))
End Sub

Private Sub mnuAnal_Click(index As Integer)
  dbg "mnuAnal:Click:" & mnuAnal(index).Caption, 3, "WDMUtil", "m"
  Call DoAnal(index)
End Sub

Private Sub DoAnal(index As Integer)
  Dim i&, animfg%, ok As Boolean
  Dim htab$, rtab$
  Dim lTs As Collection
  Dim ConstInt As Boolean

  ctlGenDate_LostFocus
  ok = True
  
  If index = 8 Then 'view a file, don't need to check for valid timeseries selection
    On Error GoTo 10
    CDFileList.flags = &H1804&
    CDFileList.Filename = " "
    CDFileList.CancelError = True
    CDFileList.DialogTitle = "WDMUtil File View"
    CDFileList.Action = 1
    Dim inputfile$
    inputfile = CDFileList.Filename
    Dim cap$
    cap = CDFileList.DialogTitle
    ATCoDispFile1.OpenFile inputfile, cap, Me.Icon, False
10      'continue here on cancel
  Else
  
    If lstSLC(1).Enabled = False Then
      MsgBox "No project file is active.", 48, "WDMUtil Tools Problem"
      ok = False
    ElseIf TSerSelected.Count < 1 And (index < 4 Or (index > 5 And index < 9)) Then
      MsgBox "No timeseries selected on which to perform this action.", 48, "WDMUtil Tools Problem"
      ok = False
    ElseIf ctlGenDate.Visible = False And index < 4 Then
      MsgBox "No dates are available to perform selected action.", 48, "WDMUtil Tools Problem"
      ok = False
    End If
    If ok Then
      If index = 0 Then 'summarize missing data
        frmWDMUSumm.Show
      ElseIf index = 1 Then 'view/edit data
        'MousePointer = ccHourglass
        'Call GenList
        'MousePointer = ccDefault
        
        If TSerSelected.Count = 0 Then
          If TSerVisible.Count = 1 Then
            TimserGrid.SelectRow 1, True
          Else
            If MsgBox("At least one time series must be selected." & vbCr & "Use all time series in the list?", vbYesNo, "GenScn Analysis") = vbYes Then
              For i = 1 To TSerVisible.Count
                TimserGrid.SelectRow i, True
              Next
            End If
          End If
        End If
        
        If TSerSelected.Count > 0 Then
          MousePointer = vbHourglass
          Set lTs = FillTimSerExt(TSerSelected, ConstInt)
          Set tsList.OpenFiles = TimserGrid.OpenFiles
          tsList.ShowColl lTs
          MousePointer = vbDefault
        End If
      ElseIf index = 2 Then 'plot data
        frmGenScnPlot.Show
        frmGenScnPlot.Icon = cmdAna(3).Picture
      ElseIf index = 3 Then 'write time series to wdm
        frmWDMUWrite.Show
        frmWDMUWrite.Icon = cmdAna(5).Picture
      ElseIf index = 4 Then 'compute new data
        MousePointer = ccHourglass
        frmWDMUComp.Show
        MousePointer = ccDefault
      ElseIf index = 5 Then 'generate
        'frmTSGener.Show
        TimserGrid.ManageTimeSeriesList 13, "&New Time Series"
      ElseIf index = 6 Then 'export data
        Call ExportData
  '      ElseIf index = 7 Then 'delete time series
  '        i = MsgBox("Are you sure you want to delete the " & STSer.Count & " selected Time Series?" & vbCrLf & _
  '                  "(WDM time-series data sets will be deleted from WDM file)", vbYesNo, "WDMUtil Delete Time Series")
  '        If i = vbYes Then 'do the delete
  '          For i = 1 To STSer.Count
  '            If Not STSer(i).File.RemoveTimSer(STSer(i)) Then
  '              MsgBox "Could not delete Time Series (ID=" & STSer(i).Header.id & ").", vbExclamation, "WDMUtil Delete Time Series Problem"
  '            End If
  '          Next i
  '        End If
  '        Call RefreshSLC
  '        Call RefreshMain
  '        Call SelectAll
      End If
    End If
  End If
End Sub

Private Sub mnuClose_Click()
  Dim i%
  dbg "mnuClose:Click", 3, "WDMUtil", "m"
  If InMemFile.DataCount > 0 Then
    i = MsgBox("You have read data into the system, but not saved it to the WDM file." & vbCrLf & "Are you sure you want to close the WDM file?", 324, "WDMUtil Close")
  Else
    i = vbYes
  End If
  If i = vbYes Then 'ok to close file
    'close files if already open
    For i = TserFiles.Active.Count To 1 Step -1
      TserFiles.Delete (i)
    Next i
    p.WDMFiles.Remove (1) 'assumes only one WDM file open at a time
    If p.HSPFMsg.Unit > 0 Then
      'message file already open
      i = F90_WDFLCL(p.HSPFMsg.Unit)
      If i <> -87 Then ' cant close last time
        p.HSPFMsg.Unit = 0
        p.HSPFMsg.Name = ""
      End If
    End If
    'CntDsn = 0
    Set TimserGrid.WholeList = New Collection
    TimserGrid.ClearRules
    TimserGrid.PopulateGrid
    TimserGrid.Enabled = False 'agdDsn.Rows = 0
    agdDSNUpdate
    
    On Error Resume Next
    
    For i = 0 To 5
      cmdSLC(i).Enabled = False
    Next i
    For i = 0 To cmdAna.Count - 1
      cmdAna(i).Enabled = False
    Next i
    For i = 0 To lstSLC.Count
      lstSLC(i).Clear
      lstSLC(i).Enabled = False
      lstSLC(i).BackColor = &HC0C0C0
    Next i
    fraAnal.Enabled = False
    fraDumDates.Enabled = False
    fraSLC(0).Enabled = False
    fraSLC(1).Enabled = False
    fraSLC(2).Enabled = False
    fraTimser.Enabled = False
    lblDumDates.Enabled = False
    For i = 0 To lblslc.Count
      lblslc(i).Caption = "0 of 0"
      lblslc(i).Enabled = False
    Next i
    mnuClose.Enabled = False
    mnuSave(0).Enabled = False
    mnuSave(1).Enabled = False
    mnuEdit.Enabled = False
    mnuOpen.Enabled = True
    mnuImport.Enabled = False
    mnuNew.Enabled = True
    p.StatusFileName = ""
    p.MapName = ""
    'p.ExtCount = 0
    p.EditFlg = False
    
    For i = 1 To mnuX.Count '- 1
      mnuX(i).Enabled = False
    Next i
    p.LocnCount = 0
    ReDim p.Locn(0)
    p.ConsCount = 0
    ReDim p.Cons(0)
    p.ScenCount = 0
    ReDim p.Scen(0)
    frmGenScn.Caption = "WDMUtil"
    On Error GoTo 0
  End If

End Sub

Private Sub mnuCon_Click(index As Integer)
    Dim i%
    
    dbg "mnuCon:Click:" & mnuCon(index).Caption, 3, "WDMUtil", "m"
    If index = 3 Then 'properties
      Unload frmGenProp
      Load frmGenProp
      With frmGenProp
        If Left(.Caption, 1) = "P" Then .Caption = "Constituent " & .Caption
        .Icon = Me.Icon
        .atcGrid.Clear
        .atcGrid.ColTitle(0) = "Name"
        .atcGrid.ColType(0) = ATCoTxt
        .atcGrid.ColWidth(0) = 1200
        .atcGrid.ColTitle(1) = "Description"
        .atcGrid.ColType(1) = ATCoTxt
        .atcGrid.ColEditable(1) = True
        .atcGrid.ColWidth(1) = .atcGrid.Width - .atcGrid.ColWidth(0) - 200
        For i = 0 To p.ConsCount - 1
            If i >= 1 Then .atcGrid.InsertRow (i)
            .atcGrid.TextMatrix((i + 1), 0) = p.Cons(i).Name
            .atcGrid.TextMatrix((i + 1), 1) = p.Cons(i).desc
        Next i
      End With
      frmGenProp.Show
    ElseIf index = 2 Then
      lstSLC(2).SetFocus
    Else
      If index = 0 Then
        i = 2
      Else
        i = 5
      End If
      Call cmdSLC_Click(i)
    End If

End Sub

Private Sub mnuEdit_Click()

    dbg "mnuEdit:Click", 3, "WDMUtil", "m"
    Call ManageProject(2)
    
End Sub

Private Sub mnuExit_Click()
    Dim i%
    
    dbg "mnuExit:Click", 3, "WDMUtil", "m"
'    If p.EditFlg And lstSLC(0).Enabled Then
'      i = MsgBox("Do you want to save your edited status file?", 3, "WDMUtil Exit")
'      If i = vbYes Then
'        'do save as
'        Call SaveStatusFileAs
'      Else 'abandon ship
'        p.StatusFileName = "XXXX"
'      End If
'    End If
    If p.StatusFileName <> "" Or lstSLC(0).Enabled = False Then
      Call Form_Unload(0)
    End If
End Sub
Private Sub Form_Load()
  If Not (HelpCheck) Then
    Dim i&, colstr$
    Dim clmX As ColumnHeader

    'App.HelpFile = "WDMUtil.hlp"
    'ExePath = App.Path

    'open message file
    dbg "OpenMessageFile:message.wdm:", 5, "WDMUtil", "t"
    Call OpenHspfMsg("message.wdm")
    If p.HSPFMsg.Unit = 0 Then
      MsgBox "Unable to open WDMUtil message file (named: " & p.HSPFMsg.Name & ")", vbCritical
    End If

    'tbrAnal.Left = 24

    On Error Resume Next
'    With tbrTimser.Buttons
'      For i = 1 To .Count
'        If .Item(i).Image = 0 Then .Item(i).Image = .Item(i).key
'      Next i
'    End With
    'With tbrAnal.Buttons
    '  For i = 1 To .Count
    '    If .Item(i).Image = 0 Then .Item(i).Image = .Item(i).key
    '  Next i
    'End With
    
    'Try setting pathnames of cursor files
    
    For i = 0 To p.ScenCount - 1
      lstSLC(0).AddItem p.Scen(i).Name
    Next i
    lstSLC_GotFocus (0)
    For i = 0 To p.LocnCount - 1
      lstSLC(1).AddItem p.Locn(i).Name
    Next i
    lstSLC_GotFocus (1)
    For i = 0 To p.ConsCount - 1
      lstSLC(2).AddItem p.Cons(i).Name
    Next i
    lstSLC_GotFocus (2)
    agdDSNUpdate
    TimserGrid.ButtonVisible(11) = False
    TimserGrid.ButtonVisible(12) = False
    
    ctlGenDate.Caption = "Dates"
    ctlGenDate.Visible = False
    
    Set tsList = New ATCoTSlist
    tsList.HelpFileName = App.HelpFile
    
    ctlGenDate.TAggr = 4 'default to Native mode in date control
  
  End If
End Sub

Private Sub mnuHelpAbout_Click()
  dbg "mnuHelpAbout:Click", 3, "WDMUtil", "m"
  frmAbout.lblInfo.Caption = StatusString
  frmAbout.Show
End Sub

Private Sub mnuHelpContents_Click()
  dbg "mnuHelpContents_Click", 3, "WDMUtil", "m"
  OpenFile App.HelpFile, cdl
End Sub

Private Sub mnuHelpDebug_Click()
  dbg "mnuHelpDebug_Click", 3, "WDMUtil", "m"
  IPC.SendMonitorMessage "(SHOW)"
End Sub

Private Sub mnuHelpFeedback_Click()
  Dim stepname As String
  On Error GoTo errmsg
  stepname = "1: Dim feedback As clsATCoFeedback"
  Dim feedback As clsATCoFeedback
  stepname = "2: Set feedback = New clsATCoFeedback"
  Set feedback = New clsATCoFeedback
  stepname = "3: feedback.AddFile"
  feedback.AddFile Left(App.path, InStr(4, App.path, "\")) & "unins000.dat"
  stepname = "4: feedback.AddText"
  feedback.AddText StatusString
  stepname = "5: feedback.Show"
  feedback.Show App, Me.Icon
  
  Exit Sub
  
errmsg:
  MsgBox "Error opening feedback in step " & stepname & vbCr & Err.Description
End Sub

Private Function StatusString() As String
  Dim S As String
  S = "WDMUtil" & vbCrLf
  S = S & "Version 2.27" & vbCrLf
  S = S & "-------------" & vbCrLf & vbCrLf
  S = S & "Contacts:" & vbCrLf
  S = S & "Marjorie Wellman, tel. (202)260-9821, email wellman.marjorie@epa.gov" & vbCrLf
  S = S & "Paul Cocca, tel. (202)260-8614, email cocca.paul@epa.gov"
  S = S & vbCrLf & vbCrLf
  If p.WDMFiles.Count = 0 Then
    S = S & "No WDM File Active" & vbCrLf
  Else
    S = S & "WDM File:  " & p.WDMFiles(1).Filename & vbCrLf
  End If
  StatusString = S
End Function

Private Sub mnuImport_Click()
  Dim e As String
  Dim OldCount As Long

  On Error GoTo ImportErrHndlr
  cdlFile.DialogTitle = "WDMUtil File Import"
  cdlFile.CancelError = True
  cdlFile.flags = &H1804& 'not read only
  cdlFile.Filename = ""
  cdlFile.Filter = "All files (*.*)|*.*"
  cdlFile.ShowOpen
  If Len(Dir(cdlFile.Filename)) > 0 Then
    Set InMemFile.Monitor = IPC
    InMemFile.HelpFileName = App.HelpFile
    OldCount = InMemFile.DataCount
    InMemFile.Filename = cdlFile.Filename
    e = InMemFile.ErrorDescription
    If Len(e) > 0 Then
      MsgBox e, vbExclamation, Me.Caption & " Problem"
    ElseIf InMemFile.DataCount <= OldCount Then 'Did not actually find any datasets
      MsgBox "No datasets added from '" & cdlFile.Filename & "'", vbOKOnly, Me.Caption
    Else 'data brought in
      RefreshSLC
      RefreshMain
      SelectAll
      TimserGrid.TopRow = TSerVisible.Count
    End If
  End If
ImportErrHndlr:
End Sub

Private Sub mnuLoc_Click(index As Integer)
    Dim i%
    dbg "mnuLoc:Click:" & mnuloc(index).Caption, 3, "WDMUtil", "m"
'    If Index = 0 Then 'All
'      Map1.SelectAll Map1.CurLayer, True
'    ElseIf Index = 1 Then 'None
'      Map1.SelectAll Map1.CurLayer, False
'    ElseIf Index = 2 Then 'set focus to point details
'      'sstLocation.Tab = 1
''      agdMapLocationDetails.SetFocus
    If index = 3 Then 'properties
      Unload frmGenProp
      Load frmGenProp
      With frmGenProp
        If Left(.Caption, 1) = "P" Then .Caption = "Location " & .Caption
        .Icon = Me.Icon
        .atcGrid.Clear
        .atcGrid.ColTitle(0) = "Name"
        .atcGrid.ColType(0) = ATCoTxt
        .atcGrid.ColWidth(0) = 1200
        .atcGrid.ColTitle(1) = "Desciption"
        .atcGrid.ColType(1) = ATCoTxt
        .atcGrid.ColEditable(1) = True
        .atcGrid.ColWidth(1) = .atcGrid.Width - .atcGrid.ColWidth(0) - 200
        For i = 0 To p.LocnCount - 1
            If i >= 1 Then .atcGrid.InsertRow (i)
            .atcGrid.TextMatrix((i + 1), 0) = p.Locn(i).Name
            .atcGrid.TextMatrix((i + 1), 1) = p.Locn(i).desc
        Next i
      End With
      frmGenProp.Show
    ElseIf index = 2 Then
      lstSLC(1).SetFocus
    Else
      If index = 0 Then
        i = 1
      Else
        i = 4
      End If
      Call cmdSLC_Click(i)
    End If

End Sub

Private Sub mnuNew_Click()

  Dim i&, fun&, fname$
  dbg "mnuNew:Click", 3, "WDMUtil", "m"

  On Error GoTo errhandler
  dbg "mnuOpen:Click", 3, "WDMUtil", "m"
  dbg "%FO", 1, "WDMUtil", "k"

  cdlFile.DialogTitle = "WDMUtil File New"
  cdlFile.CancelError = True
  cdlFile.flags = &H2000& 'create new file
  cdlFile.Filename = ""
  'set filter for WDM as default
  cdlFile.Filter = "WDM files (*.wdm)|*.wdm" '|All files (*.*)|*.*" 'Project files (*.sta)|*.sta"
  cdlFile.ShowOpen
  fname = cdlFile.Filename
  If Len(Dir(fname)) > 0 Then 'file exists
    MsgBox "Unable to create WDM file.  Specified file already exists.", vbExclamation, "WDMUtil File:New Problem"
  Else
    If Len(fname) > 0 Then 'valid name entered
      'create WDM file
      i = 2
      fun = F90_WDBOPN(i, fname, Len(fname))
      If fun > 0 Then 'successful build
        'close it up (will be opened within status file later)
        i = F90_WDFLCL(fun)
        'build status file for new WDM file
        p.StatusFileName = FilenameOnly(fname) & ".wdu"
        If Len(Dir(p.StatusFileName)) > 0 Then 'rebuild working project file
          Kill p.StatusFileName
        End If
        'build working project file
        fun = FreeFile(0)
        Open p.StatusFileName For Output As #fun
        'put WDM file on working project file
        Print #fun, "WDM WDM1 " & fname
        Close #fun
        Call frmGenScn.ManageProject(1)
        If p.WDMFiles.Count > 0 Then 'wdm open, disable open menu option
          mnuOpen.Enabled = False
          mnuImport.Enabled = True
        End If
        mnuNew.Enabled = False
      Else 'problem building WDM file
        MsgBox "Problem building WDM file: " & fname, vbExclamation, "WDMUtil New WDM File Problem"
      End If
    End If
  End If

errhandler:

End Sub
Private Sub mnuOpen_Click()

  Dim i&, j&, fun%, bfun%, fname$, istr$, lstr$, StaFName$
  Dim Find&, OldFiltInd%, attr&

  On Error GoTo errhandler
  dbg "mnuOpen:Click", 3, "WDMUtil", "m"
  dbg "%FO", 1, "WDMUtil", "k"

  With cdlFile
    .Filename = GetSetting("WDMUtil", "Open", "LastWDM", "")
    If Not FileExists(.Filename) Then
      .Filename = ExePath
      i = InStr(UCase(ExePath), "BASINS\")
      If i > 0 Then
        .Filename = Left(.Filename, i + 6) & "data\*.wdm"
      Else
        .Filename = PathNameOnly(.Filename) & "\*.wdm"
      End If
    End If
    ChDriveDir PathNameOnly(.Filename)
    .DialogTitle = "WDMUtil File Open"
    .CancelError = True
    .flags = &H1804& 'not read only
    .Filename = ""
    .Filter = "WDM files (*.wdm)|*.wdm|All files (*.*)|*.*" 'Project files (*.sta)|*.sta"
    If OldFiltInd = 0 Then
      .FilterIndex = OldFiltInd
    Else
      .FilterIndex = 1
    End If
    .ShowOpen
    dbg "OpenFile:name:" & .Filename, 3, "WDMUtil", "t"

    i = vbYes
    If InStr(UCase(.Filename), "MESSAGE.WDM") > 0 Then 'can't open message file
      MsgBox "This WDM file (message.wdm) is WDMUtil's message file and may not be opened.", vbExclamation, "WDMUtil Open Problem"
      i = vbNo
    Else 'check to make sure it's a WDM file
      If ValidWDMFile(.Filename) Then 'passed VB WDM check
        'make sure it's not readonly
        attr = GetAttr(.Filename)
        If (attr And vbReadOnly) <> 0 Then
          i = MsgBox("The specified WDM file is Read Only and may not be opened in that state." & vbCrLf & "Change file to not Read Only so that it may be opened?", vbExclamation + vbYesNo + vbDefaultButton1, "WDMUtil File Open Problem")
          If i = vbYes Then 'change to not readonly
            attr = attr - vbReadOnly
            SetAttr .Filename, attr
            fname = FilenameOnly(.Filename) & ".inf"
            If Len(Dir(fname)) > 0 Then
              SetAttr fname, attr
            End If
          Else 'don't open
            p.WDMFiles(1).FileUnit = -1
          End If
        End If
        If i = vbYes Then
          Dim plgIndex&, newClsTserFile As ATCclsTserFile
          plgIndex = TserFiles.AvailIndexByName("clsTSerWDM")
          TserFiles.Create plgIndex
          'make monitor available
          Set newClsTserFile = TserFiles.CurrentActive.obj
          TserFiles.CurrentActive.obj.MsgUnit = p.HSPFMsg.Unit
          'read the file
          newClsTserFile.Filename = .Filename
          p.WDMFiles.Add newClsTserFile
        End If
      Else 'failed VB WDM check
        p.WDMFiles(1).FileUnit = 0
      End If
      If p.WDMFiles(1).FileUnit <> 0 Then 'valid WDM file,
        SaveSetting "WDMUtil", "Open", "LastWDM", .Filename
        If p.WDMFiles(1).FileUnit > 0 Then
          'close it for now (will be opened in OpenStatusFile)
          j = F90_WDMOPN(p.WDMFiles(1).FileUnit, p.WDMFiles(1).Filename, Len(p.WDMFiles(1).Filename))
          j = F90_WDFLCL(p.WDMFiles(1).FileUnit)
          p.WDMFiles.Remove (1) 'memory leak?
        End If
      Else 'not a valid WDM file
        MsgBox "The selected file (" & .Filename & ") is not a valid WDM file and may not be opened.", vbExclamation, "WDMUtil Open Problem"
        i = vbNo
      End If
    End If
    If i = vbYes Then 'ok to open wdm file
      StaFName = FilenameOnly(.Filename) & ".wdu"
      If p.StatusFileName = StaFName Then 'rebuild working project file
        Kill p.StatusFileName
      End If
      'build working project file
      p.StatusFileName = StaFName
      fun = FreeFile(0)
      Open p.StatusFileName For Output As #fun
      'put WDM file on working project file
      Print #fun, "WDM WDM1 " & .Filename
      Close #fun
      Call ManageProject(1)
      
    End If
    If p.WDMFiles.Count > 0 Then 'wdm open, disable open menu option
      mnuOpen.Enabled = False
      mnuImport.Enabled = True
    End If
    mnuNew.Enabled = False
  End With
errhandler:

End Sub

Private Function ValidWDMFile(WDMName$) As Boolean

  Dim fun&, itmp As Long

  On Error GoTo WDMerrhnd
  fun = FreeFile(0)
  Open WDMName For Binary As #fun 'Len = 2048
  'Input #fun, itmp
  Get #fun, , itmp
  If itmp = -999 Or itmp = -998 Then
    ValidWDMFile = True
  Else
    ValidWDMFile = False
  End If
  Close #fun
  Exit Function

WDMerrhnd:
  ValidWDMFile = False

End Function

'Private Sub mnuPlayKeys_Click()
'
'  cmdKey.DialogTitle = "Open Keystroke File"
'  cmdKey.flags = &H1804& 'not read only
'  cmdKey.Filter = "Keystroke Files (*.atk)|*.atk|All Files (*.*)|*.*"
'
'  On Error GoTo NeverMind
'  cmdKey.CancelError = True
'  cmdKey.ShowOpen
'
'  If Len(cmdKey.Filename) > 0 Then
'    Launch.StartMacroPlay ExePath & "winkeydriver.exe " & cmdKey.Filename
'  End If
'NeverMind:
'End Sub

'Private Sub mnuRecordKeys_Click()
'  Dim s$, i&, t$
'
'  If mnuRecordKeys.Caption = "&Record Keys" Then
'    HookKeyboard
'    dbg "StartRecordingKeys:", 7, "WDMUtil", "i"
'    mnuRecordKeys.Caption = "Stop &Recording"
'  Else
'    UnhookKeyboard
'    mnuRecordKeys.Caption = "&Record Keys"
'    s = GetRecordedKeystrokes
'    If Len(s) > 0 Then
'      cmdKey.DialogTitle = "Save Keystroke File"
'      cmdKey.flags = &H1804& 'not read only
'      cmdKey.Filter = "Keystroke Files (*.atk)|*.atk|All Files (*.*)|*.*"
'      On Error GoTo NeverMind
'      cmdKey.CancelError = True
'      cmdKey.ShowSave
'
'      If Len(cmdKey.Filename) > 0 Then
'
'        i = FreeFile(0)
'        Open cmdKey.Filename For Output As i
'        Print #i, s
'        Close #i
'      End If
'    End If
'  End If
'NeverMind:
'End Sub

Private Sub mnuSave_Click(index As Integer)
    dbg "mnuSave:Click:" & mnuSave(index).Caption, 3, "WDMUtil", "m"
    If lstSLC(0).Enabled = False Then
      MsgBox "No project is active to save.", _
        vbExclamation, "WDMUtil Save Project File"
    Else
      'have active project
      If index = 1 Or p.StatusFileName = "" Then
        'do save as
        Call SaveStatusFileAs
      Else
        'just save
        Call SaveStatusFile
      End If
    End If
End Sub

Private Sub mnuSen_Click(index As Integer)
    
    Dim i%

    If index = 2 Then
      lstSLC(0).SetFocus
    Else
      If index = 0 Then
        i = 0
      Else
        i = 3
      End If
      Call cmdSLC_Click(i)
    End If

End Sub

Private Sub mnuTime_Click(index As Integer)

    dbg "mnuTime:Click:" & mnuTime(index).Caption, 3, "WDMUtil", "m"
    If index = 14 Then
      If TimserGrid.Visible And TimserGrid.Enabled Then TimserGrid.SetFocus
      'agdDSN.SetFocus
    Else
      Call TimserGrid.ManageTimeSeriesList(index, mnuTime(index).Caption)
      'Call TimeSeriesListManage(Index, mnuTime(Index).Caption)
    End If
End Sub

Private Sub GenList()

    Dim i&, j&, ipos&, lnts&, ldt&(5)
    Dim mxpts&, ConstInt As Boolean
    Dim lqfg&, retcod&
    Dim CScenm$(), clocnm$(), cconnm$()
    Dim ltu&(), which&(), typind&()
    Dim Lcntcon&, Lcntsen&, Lcntloc&
    Dim Calab$, Cyrlab$, Cyllab$, Ctitl$, capt$
    Dim ctran$, strCTunit$, clab$()
    Dim l As Object
    Dim lTs As Collection
    Dim STSer As Collection

    Set STSer = TSerSelected
    
    If STSer.Count > 10 Then 'not doing more than 10 listings
      lnts = 10
    Else
      lnts = STSer.Count
    End If
    'get the data values for selected data sets
    ConstInt = True
    Set lTs = FillTimSerExt(STSer, ConstInt)

    'labels
    ReDim CScenm(lnts - 1)
    ReDim clocnm(lnts - 1)
    ReDim cconnm(lnts - 1)
    ReDim which(lnts - 1)
    ReDim typind(lnts - 1)
    ReDim clab(lnts - 1)
    For i = 1 To lnts
      CScenm(i - 1) = STSer(i).Header.Sen
      clocnm(i - 1) = STSer(i).Header.Loc
      cconnm(i - 1) = STSer(i).Header.Con
    Next i
    Call DefaultLabels(CDTran, ConstInt, _
                       lnts, CScenm(), clocnm(), cconnm(), _
                       ctunit, 0, which(), typind(), Lcntcon, Lcntsen, Lcntloc, _
                       Calab, Cyrlab, Cyllab, Ctitl, clab(), ctran, strCTunit)

    If Not ConstInt Then
      'adjust variable labels
      ReDim Preserve clab(2 * lnts)
      ReDim VLab(2 * lnts)
      For i = lnts - 1 To 1 Step -1
        VLab(2 * i) = clab(i)
        VLab(2 * i + 1) = "Time"
      Next i
      VLab(1) = "Time"
    End If
    capt = "WDMUtil List"
    If ConstInt Then 'constant interval
      Call GLInit(2, l, lnts, lnts)
'      For i = 0 To lnts - 1
'        Call GLSetEditable(l, i)
'      Next i
    Else
      Call GLInit(2, l, lnts, 2 * lnts)
'      For i = 0 To 2 * lnts - 1 Step 2
'        j = i / 2
'        Call GLSetEditable(l, j)
'      Next i
    End If
    Call GLTitl(Ctitl, capt)
    Call GLIcon(cmdAna(2).Picture)
    Call GLVarLab(clab())
    Call GLAxLab("", Cyllab, Cyrlab, Calab)
    Call GLDate(CSDat, CEDat, ConstInt)
    Call GLDoTS(l, 2, lTs)
    If ConstInt Then 'constant interval
      For i = 0 To lnts - 1
        Call GLSetEditable(l, i)
      Next i
    Else
      For i = 0 To 2 * lnts - 1 Step 2
        j = i / 2
        Call GLSetEditable(l, j)
      Next i
    End If

End Sub

Private Sub TimserGrid_add()
  TimeseriesAdd
  TimserGrid_Change
End Sub

Private Sub TimserGrid_Change()
  RefreshTSList
  TimserGrid.PopulateGrid
  SetGridColors
End Sub

Private Sub TimserGrid_Edit()
  
  RefreshSLC
  agdDSNUpdate
  SetGridColors
  
'  Dim i&, j&, FirstSel&
'  Dim vTserFile As Variant, lSTSer As Collection
'  Dim ATserF As ATCclsTserFile, Atser As ATCclsTserData

'  ReDim p.Scen(0)
'  p.ScenCount = 0
'  ReDim p.Cons(0)
'  p.ConsCount = 0
'  ReDim p.Locn(0)
'  p.LocnCount = 0
'
'  For Each vTserFile In TserFiles.Active
'    Set ATserF = vTserFile.obj
'    For i = 1 To ATserF.DataCount
'      Set Atser = ATserF.Data(i)
'      With Atser.Header
'        AddUniqueSLC .Sen, "Scen"  'scenario
'        AddUniqueSLC .Loc, "Locn"  'location
'        AddUniqueSLC .Con, "Cons"  'constituent
'      End With
'    Next i
'  Next vTserFile
'  lstSLC(2).clear 'constituents
'  For i = 0 To p.ConsCount - 1
'    lstSLC(2).AddItem p.Cons(i).Name
'  Next i
'  lstSLC_GotFocus (2)
'  lstSLC(1).clear 'locations
'  For i = 0 To p.LocnCount - 1
'    lstSLC(1).AddItem p.Locn(i).Name
'  Next i
'  lstSLC_GotFocus (1)
'  lstSLC(0).clear 'scenarios
'  For i = 0 To p.ScenCount - 1
'    lstSLC(0).AddItem p.Scen(i).Name
'  Next i
'  lstSLC_GotFocus (0)

'  If STSer.Count > 0 Then
'    Set lSTSer = New Collection
'    Set lSTSer = STSer
'    FirstSel = 0
'    For i = 1 To Tser.Count
'      For j = 1 To lSTSer.Count
'        If Tser(i).serial = lSTSer(j).serial Then
'          TimserGrid.SelectRow i, True
'          If FirstSel = 0 Then FirstSel = i
'        End If
'      Next j
'    Next i
'    TimserGrid.TopRow = FirstSel
'  End If
End Sub

Private Sub TimserGrid_SelChange()
  'Debug.Print "TimserGrid_SelChange "
  agdDSNUpdate
  SetGridColors
End Sub

Sub TimeseriesAdd()
  Dim crit As ATCclsCriterion
  Dim i&, msg$
  
  TimserGrid.ClearRules
  
  If lstSLC(1).SelCount = 0 Then 'locations
    msg = msg & "No Location specified." & vbCrLf
  Else
    Set crit = New ATCclsCriterion
    crit.Field = "Loc"
    For i = 0 To scntLoc
      crit.Values.Add specLoc(i)
    Next
    TimserGrid.AddRule crit
    Set crit = Nothing
  End If
  
  If lstSLC(0).SelCount = 0 Then 'scenarios
    msg = msg & "No scenarios specified." & vbCrLf
  Else
    Set crit = New ATCclsCriterion
    crit.Field = "Sen"
    For i = 0 To scntSen
      crit.Values.Add specSen(i)
    Next
    TimserGrid.AddRule crit
    Set crit = Nothing
  End If
  
  If lstSLC(2).SelCount = 0 Then 'constituents
    msg = msg & "No constituents specified." & vbCrLf
  Else
    Set crit = New ATCclsCriterion
    crit.Field = "Con"
    For i = 0 To scntCon
      crit.Values.Add specCon(i)
    Next
    TimserGrid.AddRule crit
    Set crit = Nothing
  End If
  TimserGrid.AddTimeseriesMatchingCurrentRules
  TimserGrid.Enabled = True
  MousePointer = vbDefault
End Sub

Private Sub SetGridColors()

  Dim i&
  For i = 1 To TSerVisible.Count
    If TSerVisible(i).File.Label <> "WDM" Then
      TimserGrid.RowBackColor(i) = vbYellow
'    Else
'      TimserGrid.RowBackColor(i) = vbWhite
    End If
  Next i
End Sub

Private Sub ExportData()

  Dim i&, j&, S$, sdsn$, DateVals#(), DateWidth&, ldt&(5), ltu&
  Dim lTs As Collection, llTs As Collection
  Dim ExpFileName$, Funit&, ConstInt As Boolean
  Dim STSer As Collection

  Set STSer = TSerSelected
  On Error GoTo ExpErrHandler
  If STSer.Count > 1 Then
    MsgBox "Exporting is performed one time series per file." & vbCr & "Only the first selected time series will be exported.", vbInformation, "WDMUtil Export"
  End If
  'get name of file to export data to
  cdlFile.DialogTitle = "WDMUtil Export Data"
  cdlFile.CancelError = True
  cdlFile.flags = &H3804& 'create & not read only
  cdlFile.Filename = ""
  cdlFile.Filter = "Export files (*.exp)|*.exp|All files (*.*)|*.*"
  cdlFile.ShowSave
  ExpFileName = cdlFile.Filename
  MousePointer = ccHourglass
  Funit = FreeFile(0)
  Open ExpFileName For Output As #Funit
  'get the data values for selected data sets
  ConstInt = True
  Set llTs = Nothing
  Set llTs = New Collection
  llTs.Add STSer(1) 'only take the first selected tser
  Set lTs = FillTimSerExt(llTs, ConstInt)
  Print #Funit, lTs(1).Header.Sen
  Print #Funit, lTs(1).Header.Loc
  Print #Funit, lTs(1).Header.Con
  Print #Funit, lTs(1).Header.desc
  If CDTran < 4 Then
    ltu = ctunit
  Else
    ltu = lTs(1).Dates.Summary.Tu
  End If
  Select Case ltu
    Case TUYear:    DateWidth = 4
    Case TUMonth:   DateWidth = 7
    Case TUDay:     DateWidth = 10
    Case TUHour:    DateWidth = 13
    Case TUMinute:  DateWidth = 16
    Case Else:      DateWidth = 19
  End Select
  S = "Date" & Space(DateWidth - 4)
  For i = 1 To lTs.Count
    sdsn = "DSN " & lTs(i).Header.id
    S = S & Space(12 - Len(sdsn)) & sdsn
  Next i
  Print #Funit, S
  ReDim DateVals(lTs(1).Dates.Summary.NVALS)
  DateVals = lTs(1).Dates.Values
  For i = 1 To lTs(1).Dates.Summary.NVALS
    If lTs(1).Dates.Summary.Tu = 4 Then 'back up one interval for start of day
      Call J2Date(DateVals(i - 1), ldt)
    Else
      Call J2Date(DateVals(i), ldt)
    End If
    If ldt(3) = 0 And DateWidth > 10 Then 'adjust to midnight hr=24
      Call timcnv(ldt)
    End If
    S = ldt(0) & "/" & NumFmtI(ldt(1), 2) & "/" & NumFmtI(ldt(2), 2)
    If DateWidth > 10 Then 'do hms too
      S = S & " " & NumFmtI(ldt(3), 2) & ":" & NumFmtI(ldt(4), 2) & ":" & NumFmtI(ldt(5), 2)
    End If
    S = Left(S, DateWidth) 'trim as needed
    For j = 1 To lTs.Count
      S = S & NumFmted(lTs(j).Value(i), 12, 4)
    Next j
    Print #Funit, S
  Next i
  MousePointer = ccDefault
  MsgBox "Successfully Exported Time Series to file " & ExpFileName, vbInformation, "WDMUtil Export"
  Close #Funit

ExpErrHandler:

End Sub

Public Property Get TSerAll() As Collection
  Set TSerAll = TimserGrid.WholeList
End Property

Public Property Get TSerSelected() As Collection
  Set TSerSelected = TimserGrid.SelectedList
End Property

Public Property Get TSerVisible() As Collection
  Set TSerVisible = TimserGrid.VisibleList
End Property

Private Sub tsList_CreatedTser(newTS As ATCData.ATCclsTserData)
  RaiseEdit
End Sub

Private Sub tsList_Edit()
  RaiseEdit
End Sub

Public Sub RaiseEdit()
  RefreshSLC
  RefreshMain
  TimserGrid_Change
End Sub

