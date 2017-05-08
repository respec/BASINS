VERSION 5.00
Begin VB.Form frmGenScn 
   Caption         =   "GenScn"
   ClientHeight    =   7425
   ClientLeft      =   30
   ClientTop       =   645
   ClientWidth     =   12765
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
   HelpContextID   =   21
   Icon            =   "GenScn.frx":0000
   LinkTopic       =   "Form1"
   PaletteMode     =   1  'UseZOrder
   ScaleHeight     =   7425
   ScaleWidth      =   12765
   Begin VB.Frame fraSLC 
      Caption         =   "Constituents"
      Enabled         =   0   'False
      Height          =   3132
      HelpContextID   =   70
      Index           =   2
      Left            =   9240
      TabIndex        =   28
      Top             =   0
      Width           =   2412
      Begin VB.OptionButton optSLC 
         Caption         =   "Location"
         Enabled         =   0   'False
         Height          =   252
         Index           =   3
         Left            =   1080
         TabIndex        =   14
         Top             =   600
         Width           =   1092
      End
      Begin VB.OptionButton optSLC 
         Caption         =   "All"
         Enabled         =   0   'False
         Height          =   252
         Index           =   2
         Left            =   240
         TabIndex        =   13
         Top             =   600
         Value           =   -1  'True
         Width           =   852
      End
      Begin VB.CommandButton cmdSLC 
         Caption         =   "None"
         Enabled         =   0   'False
         Height          =   252
         Index           =   5
         Left            =   1680
         TabIndex        =   12
         ToolTipText     =   "Select NO Constituents"
         Top             =   240
         Width           =   612
      End
      Begin VB.CommandButton cmdSLC 
         Caption         =   "All"
         Enabled         =   0   'False
         Height          =   252
         Index           =   2
         Left            =   960
         TabIndex        =   11
         ToolTipText     =   "Select ALL Constituents"
         Top             =   240
         Width           =   612
      End
      Begin VB.ListBox lstSLC 
         BackColor       =   &H80000016&
         Enabled         =   0   'False
         Height          =   1815
         Index           =   2
         ItemData        =   "GenScn.frx":0442
         Left            =   120
         List            =   "GenScn.frx":0444
         MultiSelect     =   1  'Simple
         Sorted          =   -1  'True
         TabIndex        =   15
         Top             =   960
         WhatsThisHelpID =   5
         Width           =   2172
      End
      Begin VB.ListBox lstSLC 
         BackColor       =   &H80000016&
         Enabled         =   0   'False
         Height          =   1815
         Index           =   4
         Left            =   120
         MultiSelect     =   1  'Simple
         Sorted          =   -1  'True
         TabIndex        =   16
         Top             =   960
         Visible         =   0   'False
         Width           =   2172
      End
      Begin VB.Label lblSLC 
         Enabled         =   0   'False
         Height          =   252
         Index           =   2
         Left            =   120
         TabIndex        =   29
         Top             =   240
         Width           =   1092
      End
   End
   Begin VB.Frame fraSLC 
      Caption         =   "Scenarios"
      Enabled         =   0   'False
      Height          =   3132
      HelpContextID   =   52
      Index           =   0
      Left            =   6480
      TabIndex        =   0
      Top             =   0
      Width           =   2412
      Begin VB.OptionButton optSLC 
         Caption         =   "Location"
         Enabled         =   0   'False
         Height          =   252
         Index           =   1
         Left            =   1200
         TabIndex        =   5
         Top             =   600
         Width           =   1092
      End
      Begin VB.OptionButton optSLC 
         Caption         =   "All"
         Enabled         =   0   'False
         Height          =   252
         Index           =   0
         Left            =   240
         TabIndex        =   4
         Top             =   600
         Value           =   -1  'True
         Width           =   852
      End
      Begin VB.CommandButton cmdSen 
         Caption         =   "New"
         Enabled         =   0   'False
         Height          =   372
         HelpContextID   =   68
         Index           =   2
         Left            =   1745
         TabIndex        =   10
         ToolTipText     =   "Create Scenario"
         Top             =   2640
         Width           =   550
      End
      Begin VB.CommandButton cmdSen 
         Caption         =   "Delete"
         Enabled         =   0   'False
         Height          =   372
         HelpContextID   =   67
         Index           =   1
         Left            =   1025
         TabIndex        =   9
         ToolTipText     =   "Delete Selected Scenario"
         Top             =   2640
         Width           =   735
      End
      Begin VB.CommandButton cmdSen 
         Caption         =   "Activate"
         Enabled         =   0   'False
         Height          =   372
         Index           =   0
         Left            =   120
         TabIndex        =   8
         ToolTipText     =   "View/Edit/Execute Selected Scenario"
         Top             =   2640
         Width           =   900
      End
      Begin VB.CommandButton cmdSLC 
         Caption         =   "None"
         Enabled         =   0   'False
         Height          =   252
         Index           =   3
         Left            =   1680
         TabIndex        =   3
         ToolTipText     =   "Select NO Scenarios"
         Top             =   240
         Width           =   612
      End
      Begin VB.CommandButton cmdSLC 
         Caption         =   "All"
         Enabled         =   0   'False
         Height          =   252
         Index           =   0
         Left            =   960
         TabIndex        =   2
         ToolTipText     =   "Select ALL Scenarios"
         Top             =   240
         Width           =   612
      End
      Begin MSComDlg.CommonDialog CDNewScen 
         Left            =   840
         Top             =   600
         _ExtentX        =   688
         _ExtentY        =   688
         _Version        =   393216
         FontSize        =   4.873e-37
      End
      Begin VB.ListBox lstSLC 
         BackColor       =   &H80000016&
         Enabled         =   0   'False
         Height          =   1425
         Index           =   0
         ItemData        =   "GenScn.frx":0446
         Left            =   120
         List            =   "GenScn.frx":0448
         MultiSelect     =   1  'Simple
         Sorted          =   -1  'True
         TabIndex        =   7
         Top             =   960
         WhatsThisHelpID =   3
         Width           =   2172
      End
      Begin VB.ListBox lstSLC 
         BackColor       =   &H80000016&
         Enabled         =   0   'False
         Height          =   1425
         Index           =   3
         Left            =   120
         MultiSelect     =   1  'Simple
         Sorted          =   -1  'True
         TabIndex        =   6
         Top             =   960
         Visible         =   0   'False
         Width           =   2175
      End
      Begin VB.Label lblSLC 
         Enabled         =   0   'False
         Height          =   252
         Index           =   0
         Left            =   120
         TabIndex        =   27
         Top             =   240
         Width           =   1092
      End
   End
   Begin VB.Frame fraDumDates 
      Caption         =   "Dates"
      Enabled         =   0   'False
      Height          =   1335
      HelpContextID   =   89
      Left            =   6480
      TabIndex        =   32
      Top             =   5040
      Width           =   4932
      Begin VB.Label lblDumDates 
         Caption         =   "<not set>."
         Enabled         =   0   'False
         Height          =   252
         Left            =   120
         TabIndex        =   33
         Top             =   360
         Width           =   4692
      End
   End
   Begin VB.Frame sashV 
      BorderStyle     =   0  'None
      Height          =   7335
      Left            =   6360
      MousePointer    =   9  'Size W E
      TabIndex        =   35
      Top             =   0
      Width           =   60
   End
   Begin MSComDlg.CommonDialog cdl 
      Left            =   855
      Top             =   15
      _ExtentX        =   688
      _ExtentY        =   688
      _Version        =   393216
      FontSize        =   4.873e-37
   End
   Begin VB.Frame fraTimser 
      Caption         =   "Time Series"
      Enabled         =   0   'False
      Height          =   1812
      HelpContextID   =   71
      Left            =   6480
      TabIndex        =   31
      Top             =   3160
      Width           =   6255
      Begin GenScn.ATCTimserGrid TimserGrid 
         Height          =   1455
         Left            =   120
         TabIndex        =   36
         Top             =   240
         Width           =   5895
         _ExtentX        =   10398
         _ExtentY        =   2566
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
      Caption         =   "Analysis"
      Enabled         =   0   'False
      Height          =   865
      HelpContextID   =   90
      Left            =   6480
      TabIndex        =   30
      Top             =   6480
      Width           =   4920
      Begin VB.CommandButton cmdAna 
         Enabled         =   0   'False
         Height          =   550
         Index           =   8
         Left            =   4320
         Picture         =   "GenScn.frx":044A
         Style           =   1  'Graphical
         TabIndex        =   26
         Tag             =   "9"
         ToolTipText     =   "Estimate Loads"
         Top             =   240
         Visible         =   0   'False
         Width           =   520
      End
      Begin VB.CommandButton cmdAna 
         Enabled         =   0   'False
         Height          =   550
         HelpContextID   =   112
         Index           =   7
         Left            =   3795
         Picture         =   "GenScn.frx":0754
         Style           =   1  'Graphical
         TabIndex        =   25
         Tag             =   "8"
         ToolTipText     =   "Generate Profile Plots"
         Top             =   240
         Width           =   520
      End
      Begin VB.CommandButton cmdAna 
         Enabled         =   0   'False
         Height          =   550
         HelpContextID   =   111
         Index           =   6
         Left            =   3270
         Picture         =   "GenScn.frx":0A5E
         Style           =   1  'Graphical
         TabIndex        =   24
         Tag             =   "7"
         ToolTipText     =   "Perform Animation"
         Top             =   240
         Width           =   520
      End
      Begin VB.CommandButton cmdAna 
         Enabled         =   0   'False
         Height          =   550
         HelpContextID   =   110
         Index           =   5
         Left            =   2220
         Picture         =   "GenScn.frx":0EA0
         Style           =   1  'Graphical
         TabIndex        =   22
         Tag             =   "6"
         ToolTipText     =   "View a File"
         Top             =   240
         Width           =   520
      End
      Begin VB.CommandButton cmdAna 
         Enabled         =   0   'False
         Height          =   550
         HelpContextID   =   920
         Index           =   4
         Left            =   2745
         Picture         =   "GenScn.frx":11AA
         Style           =   1  'Graphical
         TabIndex        =   23
         Tag             =   "4"
         ToolTipText     =   "Generate Timeseries"
         Top             =   240
         Width           =   520
      End
      Begin VB.CommandButton cmdAna 
         Enabled         =   0   'False
         Height          =   550
         HelpContextID   =   108
         Index           =   3
         Left            =   1695
         Picture         =   "GenScn.frx":14B4
         Style           =   1  'Graphical
         TabIndex        =   21
         Tag             =   "3"
         ToolTipText     =   "Compare Two Timeseries"
         Top             =   240
         Width           =   520
      End
      Begin VB.CommandButton cmdAna 
         Enabled         =   0   'False
         Height          =   550
         HelpContextID   =   107
         Index           =   2
         Left            =   1170
         Picture         =   "GenScn.frx":15BB
         Style           =   1  'Graphical
         TabIndex        =   20
         Tag             =   "2"
         ToolTipText     =   "Perform Duration Analysis"
         Top             =   240
         Width           =   520
      End
      Begin VB.CommandButton cmdAna 
         Enabled         =   0   'False
         Height          =   550
         HelpContextID   =   910
         Index           =   1
         Left            =   645
         Picture         =   "GenScn.frx":19FD
         Style           =   1  'Graphical
         TabIndex        =   19
         Tag             =   "1"
         ToolTipText     =   "List Timeseries Values"
         Top             =   240
         Width           =   520
      End
      Begin VB.CommandButton cmdAna 
         Enabled         =   0   'False
         Height          =   550
         HelpContextID   =   601
         Index           =   0
         Left            =   120
         Picture         =   "GenScn.frx":1D07
         Style           =   1  'Graphical
         TabIndex        =   18
         Tag             =   "0"
         ToolTipText     =   "Generate Graphs"
         Top             =   240
         Width           =   520
      End
   End
   Begin VB.Frame fraSLC 
      Caption         =   "Locations"
      Enabled         =   0   'False
      Height          =   7335
      HelpContextID   =   40
      Index           =   1
      Left            =   0
      TabIndex        =   1
      Top             =   0
      Width           =   6255
      Begin VB.Frame fraLocations 
         BorderStyle     =   0  'None
         Caption         =   "Frame1"
         Height          =   3612
         Left            =   120
         TabIndex        =   37
         Top             =   300
         Visible         =   0   'False
         Width           =   2532
         Begin VB.CommandButton cmdSLC 
            Caption         =   "All"
            Enabled         =   0   'False
            Height          =   252
            Index           =   1
            Left            =   840
            TabIndex        =   40
            ToolTipText     =   "Select ALL Constituents"
            Top             =   0
            Width           =   612
         End
         Begin VB.CommandButton cmdSLC 
            Caption         =   "None"
            Enabled         =   0   'False
            Height          =   252
            Index           =   4
            Left            =   1560
            TabIndex        =   39
            ToolTipText     =   "Select NO Constituents"
            Top             =   0
            Width           =   612
         End
         Begin VB.ListBox lstSLC 
            BackColor       =   &H80000016&
            Enabled         =   0   'False
            Height          =   2790
            Index           =   1
            ItemData        =   "GenScn.frx":2011
            Left            =   0
            List            =   "GenScn.frx":2013
            MultiSelect     =   1  'Simple
            Sorted          =   -1  'True
            TabIndex        =   38
            Top             =   360
            WhatsThisHelpID =   5
            Width           =   2172
         End
         Begin VB.Label lblSLC 
            Enabled         =   0   'False
            Height          =   252
            Index           =   1
            Left            =   0
            TabIndex        =   41
            Top             =   0
            Width           =   1092
         End
      End
      Begin MSComDlg.CommonDialog cmdKey 
         Left            =   1680
         Top             =   0
         _ExtentX        =   688
         _ExtentY        =   688
         _Version        =   393216
      End
      Begin ATML2k.ATCoMap Map1 
         Height          =   6915
         HelpContextID   =   500
         Left            =   120
         TabIndex        =   34
         Top             =   240
         Width           =   5955
         _ExtentX        =   10504
         _ExtentY        =   12197
         RefreshMapLayer =   -1  'True
         ConfirmSelections=   0   'False
         Enabled         =   0   'False
         LegendVisible   =   -1  'True
         ToolbarVisible  =   -1  'True
      End
   End
   Begin ATCoCtl.ATCoDate ctlGenDate 
      Height          =   1350
      HelpContextID   =   69
      Left            =   6480
      TabIndex        =   17
      Top             =   5040
      Width           =   4920
      _ExtentX        =   8837
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
         NumListImages   =   24
         BeginProperty ListImage1 {2C247F27-8591-11D1-B16A-00C0F0283628} 
            Picture         =   "GenScn.frx":2015
            Key             =   "Zoom"
         EndProperty
         BeginProperty ListImage2 {2C247F27-8591-11D1-B16A-00C0F0283628} 
            Picture         =   "GenScn.frx":2567
            Key             =   "ZoomIn"
         EndProperty
         BeginProperty ListImage3 {2C247F27-8591-11D1-B16A-00C0F0283628} 
            Picture         =   "GenScn.frx":2AB9
            Key             =   "ZoomOut"
         EndProperty
         BeginProperty ListImage4 {2C247F27-8591-11D1-B16A-00C0F0283628} 
            Picture         =   "GenScn.frx":300B
            Key             =   "Pan"
         EndProperty
         BeginProperty ListImage5 {2C247F27-8591-11D1-B16A-00C0F0283628} 
            Picture         =   "GenScn.frx":355D
            Key             =   "FullExtent"
         EndProperty
         BeginProperty ListImage6 {2C247F27-8591-11D1-B16A-00C0F0283628} 
            Picture         =   "GenScn.frx":3AAF
            Key             =   "Identify"
         EndProperty
         BeginProperty ListImage7 {2C247F27-8591-11D1-B16A-00C0F0283628} 
            Picture         =   "GenScn.frx":4001
            Key             =   "Left"
         EndProperty
         BeginProperty ListImage8 {2C247F27-8591-11D1-B16A-00C0F0283628} 
            Picture         =   "GenScn.frx":431B
            Key             =   "Right"
         EndProperty
         BeginProperty ListImage9 {2C247F27-8591-11D1-B16A-00C0F0283628} 
            Picture         =   "GenScn.frx":4635
            Key             =   "Select"
         EndProperty
         BeginProperty ListImage10 {2C247F27-8591-11D1-B16A-00C0F0283628} 
            Picture         =   "GenScn.frx":494F
            Key             =   "Unselect"
         EndProperty
         BeginProperty ListImage11 {2C247F27-8591-11D1-B16A-00C0F0283628} 
            Picture         =   "GenScn.frx":4C69
            Key             =   "Get"
         EndProperty
         BeginProperty ListImage12 {2C247F27-8591-11D1-B16A-00C0F0283628} 
            Picture         =   "GenScn.frx":4F83
            Key             =   "Save"
         EndProperty
         BeginProperty ListImage13 {2C247F27-8591-11D1-B16A-00C0F0283628} 
            Picture         =   "GenScn.frx":529D
            Key             =   "Print"
         EndProperty
         BeginProperty ListImage14 {2C247F27-8591-11D1-B16A-00C0F0283628} 
            Picture         =   "GenScn.frx":55B7
            Key             =   "Add"
         EndProperty
         BeginProperty ListImage15 {2C247F27-8591-11D1-B16A-00C0F0283628} 
            Picture         =   "GenScn.frx":58D1
            Key             =   "Remove"
         EndProperty
         BeginProperty ListImage16 {2C247F27-8591-11D1-B16A-00C0F0283628} 
            Picture         =   "GenScn.frx":5BEB
            Key             =   "Properties"
         EndProperty
         BeginProperty ListImage17 {2C247F27-8591-11D1-B16A-00C0F0283628} 
            Picture         =   "GenScn.frx":5F05
            Key             =   "Clear"
         EndProperty
         BeginProperty ListImage18 {2C247F27-8591-11D1-B16A-00C0F0283628} 
            Picture         =   "GenScn.frx":621F
            Key             =   "Up"
         EndProperty
         BeginProperty ListImage19 {2C247F27-8591-11D1-B16A-00C0F0283628} 
            Picture         =   "GenScn.frx":6539
            Key             =   "Down"
         EndProperty
         BeginProperty ListImage20 {2C247F27-8591-11D1-B16A-00C0F0283628} 
            Picture         =   "GenScn.frx":6853
            Key             =   ""
         EndProperty
         BeginProperty ListImage21 {2C247F27-8591-11D1-B16A-00C0F0283628} 
            Picture         =   "GenScn.frx":6B6D
            Key             =   ""
         EndProperty
         BeginProperty ListImage22 {2C247F27-8591-11D1-B16A-00C0F0283628} 
            Picture         =   "GenScn.frx":6E87
            Key             =   ""
         EndProperty
         BeginProperty ListImage23 {2C247F27-8591-11D1-B16A-00C0F0283628} 
            Picture         =   "GenScn.frx":71A1
            Key             =   ""
         EndProperty
         BeginProperty ListImage24 {2C247F27-8591-11D1-B16A-00C0F0283628} 
            Picture         =   "GenScn.frx":74BB
            Key             =   ""
         EndProperty
      EndProperty
   End
   Begin VB.Image imgMovePoint 
      Height          =   480
      Left            =   2880
      Picture         =   "GenScn.frx":77D5
      Top             =   0
      Width           =   480
   End
   Begin VB.Image imgPan 
      Height          =   480
      Left            =   2040
      Picture         =   "GenScn.frx":7ADF
      Top             =   0
      Width           =   480
   End
   Begin VB.Image imgZoom 
      Height          =   480
      Left            =   1320
      Picture         =   "GenScn.frx":83A9
      Top             =   0
      Width           =   480
   End
   Begin VB.Menu mnuX 
      Caption         =   "&File"
      Index           =   0
      Begin VB.Menu mnuNew 
         Caption         =   "&New Project"
      End
      Begin VB.Menu mnuOpen 
         Caption         =   "&Open Project"
      End
      Begin VB.Menu mnuSepx 
         Caption         =   "-"
      End
      Begin VB.Menu mnuEdit 
         Caption         =   "&Edit Project"
         Enabled         =   0   'False
      End
      Begin VB.Menu mnuPreferredUnits 
         Caption         =   "Preferred &Units"
      End
      Begin VB.Menu mnuSep0 
         Caption         =   "-"
      End
      Begin VB.Menu mnuClose 
         Caption         =   "&Close Project"
         Enabled         =   0   'False
      End
      Begin VB.Menu mnuSep1 
         Caption         =   "-"
      End
      Begin VB.Menu mnuSave 
         Caption         =   "&Save Project"
         Enabled         =   0   'False
         Index           =   0
      End
      Begin VB.Menu mnuSave 
         Caption         =   "Save Project &As"
         Enabled         =   0   'False
         Index           =   1
      End
      Begin VB.Menu mnuSave 
         Caption         =   "Save Project Short&Cut"
         Enabled         =   0   'False
         Index           =   2
      End
      Begin VB.Menu mnuSep1B 
         Caption         =   "-"
      End
      Begin VB.Menu mnuRecordKeys 
         Caption         =   "&Record Keys"
      End
      Begin VB.Menu mnuPlayKeys 
         Caption         =   "&Playback Keys"
      End
      Begin VB.Menu mnuRecent 
         Caption         =   "-"
         Index           =   0
         Visible         =   0   'False
      End
      Begin VB.Menu mnuSep2 
         Caption         =   "-"
      End
      Begin VB.Menu mnuExit 
         Caption         =   "E&xit"
      End
   End
   Begin VB.Menu mnuX 
      Caption         =   "&Analysis"
      Index           =   1
      Visible         =   0   'False
      Begin VB.Menu mnuAnal 
         Caption         =   "&Graph"
         Index           =   0
      End
      Begin VB.Menu mnuAnal 
         Caption         =   "&List"
         Index           =   1
      End
      Begin VB.Menu mnuAnal 
         Caption         =   "&Duration"
         Index           =   2
      End
      Begin VB.Menu mnuAnal 
         Caption         =   "&Compare"
         Index           =   3
      End
      Begin VB.Menu mnuAnal 
         Caption         =   "Ge&nerate"
         Index           =   4
      End
      Begin VB.Menu mnuAnal 
         Caption         =   "Fre&quency"
         Index           =   5
      End
      Begin VB.Menu mnuAnal 
         Caption         =   "&File"
         Index           =   6
      End
      Begin VB.Menu mnuAnal 
         Caption         =   "&Animation"
         Index           =   7
      End
      Begin VB.Menu mnuAnal 
         Caption         =   "&Profile"
         Index           =   8
      End
      Begin VB.Menu mnuAnal 
         Caption         =   "&Estimator"
         Index           =   9
      End
      Begin VB.Menu mnuAnal 
         Caption         =   "&HSPF"
         Index           =   10
      End
      Begin VB.Menu mnuAnal 
         Caption         =   "Run Script"
         Index           =   11
      End
      Begin VB.Menu mnuAnalSep 
         Caption         =   "-"
      End
      Begin VB.Menu mnuGraphNative 
         Caption         =   "Graph Native"
         Visible         =   0   'False
      End
      Begin VB.Menu mnuGenerOld 
         Caption         =   "Old Generate"
      End
      Begin VB.Menu mnuListOld 
         Caption         =   "Old List"
      End
      Begin VB.Menu mnuAnalNew 
         Caption         =   "New..."
      End
      Begin VB.Menu mnuAnalPlugIn 
         Caption         =   "Report"
         Index           =   0
         Visible         =   0   'False
      End
   End
   Begin VB.Menu mnuX 
      Caption         =   "&Map"
      Index           =   2
      Visible         =   0   'False
      Begin VB.Menu mnuMap 
         Caption         =   "&Zoom"
         Index           =   0
      End
      Begin VB.Menu mnuMap 
         Caption         =   "In (&+)"
         Index           =   10
      End
      Begin VB.Menu mnuMap 
         Caption         =   "Out (&-)"
         Index           =   20
      End
      Begin VB.Menu mnuMap 
         Caption         =   "Pa&n"
         Index           =   30
      End
      Begin VB.Menu mnuMap 
         Caption         =   "&Whole Extent"
         Index           =   40
      End
      Begin VB.Menu mnuMap 
         Caption         =   "-"
         Index           =   50
      End
      Begin VB.Menu mnuMap 
         Caption         =   "&Identify"
         Index           =   60
      End
      Begin VB.Menu mnuMap 
         Caption         =   "-"
         Index           =   70
      End
      Begin VB.Menu mnuMap 
         Caption         =   "&Save"
         Index           =   80
      End
      Begin VB.Menu mnuMap 
         Caption         =   "&Get"
         Index           =   90
      End
      Begin VB.Menu mnuMap 
         Caption         =   "-"
         Index           =   100
      End
      Begin VB.Menu mnuMap 
         Caption         =   "&Print"
         Index           =   110
      End
      Begin VB.Menu mnuMap 
         Caption         =   "-"
         Index           =   120
      End
      Begin VB.Menu mnuMap 
         Caption         =   "&Add Point"
         Index           =   200
      End
      Begin VB.Menu mnuMap 
         Caption         =   "&Delete point"
         Index           =   210
      End
      Begin VB.Menu mnuMap 
         Caption         =   "&Move Point"
         Index           =   220
      End
      Begin VB.Menu mnuMap 
         Caption         =   "&Edit"
         Index           =   300
      End
      Begin VB.Menu mnuMap 
         Caption         =   "-"
         Index           =   310
      End
      Begin VB.Menu mnuMap 
         Caption         =   "&Branch Selection"
         Enabled         =   0   'False
         Index           =   400
      End
      Begin VB.Menu mnuMap 
         Caption         =   "&Downstream Selection"
         Enabled         =   0   'False
         Index           =   410
      End
      Begin VB.Menu mnuMap 
         Caption         =   "&Change to List"
         Index           =   500
      End
   End
   Begin VB.Menu mnuX 
      Caption         =   "&Locations"
      Index           =   3
      Visible         =   0   'False
      Begin VB.Menu mnuLoc 
         Caption         =   "&All"
         Index           =   0
      End
      Begin VB.Menu mnuLoc 
         Caption         =   "&None"
         Index           =   1
      End
      Begin VB.Menu mnuLoc 
         Caption         =   "&Focus"
         Index           =   2
      End
      Begin VB.Menu mnuLoc 
         Caption         =   "&Properties"
         Index           =   3
      End
      Begin VB.Menu mnuLoc 
         Caption         =   "All With &Data"
         Index           =   4
      End
      Begin VB.Menu mnuLoc 
         Caption         =   "-"
         Index           =   10
      End
      Begin VB.Menu mnuLoc 
         Caption         =   "&Change to List"
         Index           =   500
      End
   End
   Begin VB.Menu mnuX 
      Caption         =   "&Scenarios"
      Index           =   4
      Visible         =   0   'False
      Begin VB.Menu mnuScen 
         Caption         =   "&All"
         Index           =   0
      End
      Begin VB.Menu mnuScen 
         Caption         =   "&None"
         Index           =   1
      End
      Begin VB.Menu mnuScen 
         Caption         =   "&Focus"
         Index           =   2
      End
      Begin VB.Menu mnuScen 
         Caption         =   "&View"
         Index           =   3
      End
      Begin VB.Menu mnuScen 
         Caption         =   "&Delete"
         Index           =   4
      End
      Begin VB.Menu mnuScen 
         Caption         =   "&Create"
         Index           =   5
      End
      Begin VB.Menu mnuScen 
         Caption         =   "&Properties"
         Index           =   6
      End
   End
   Begin VB.Menu mnuX 
      Caption         =   "&Constituents"
      Index           =   5
      Visible         =   0   'False
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
      Index           =   6
      Visible         =   0   'False
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
      Begin VB.Menu mnuSepTime 
         Caption         =   "-"
      End
   End
   Begin VB.Menu mnuDate 
      Caption         =   "&Dates"
      Visible         =   0   'False
      Begin VB.Menu mnuDates 
         Caption         =   "&Reset"
         Index           =   0
      End
      Begin VB.Menu mnuDates 
         Caption         =   "&Start"
         Index           =   1
      End
      Begin VB.Menu mnuDates 
         Caption         =   "&End"
         Index           =   2
      End
      Begin VB.Menu mnuDates 
         Caption         =   "&Time Step"
         Index           =   3
      End
      Begin VB.Menu mnuDates 
         Caption         =   "&Units"
         Index           =   4
      End
      Begin VB.Menu mnuDates 
         Caption         =   "&Aggregation"
         Index           =   5
      End
   End
   Begin VB.Menu mnuHelp 
      Caption         =   "&Help"
      NegotiatePosition=   3  'Right
      Begin VB.Menu mnuHelpContents 
         Caption         =   "&About"
         Index           =   0
      End
      Begin VB.Menu mnuHelpContents 
         Caption         =   "&Contents"
         Index           =   1
      End
      Begin VB.Menu mnuHelpContents 
         Caption         =   "&HSPF Manual"
         Index           =   2
      End
      Begin VB.Menu mnuHelpContents 
         Caption         =   "&Debug"
         Index           =   3
      End
      Begin VB.Menu mnuHelpScript 
         Caption         =   "&Script Documentation"
      End
      Begin VB.Menu mnuHelpFeedback 
         Caption         =   "Send &Feedback"
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

Dim MapRefresh As Boolean
Private SashVdragging As Boolean
Private Const SectionMainWin = "Main Window"

Private WithEvents tsList As ATCoTSDisp.ATCoTSlist
Attribute tsList.VB_VarHelpID = -1

Public Sub DispDates(OnFlg As Boolean)
  If (OnFlg) Then
    fraDumDates.Visible = False
    'ctlGenDate.Visible = False
    ctlGenDate.Visible = True
    mnuDate.Visible = True
  Else
    ctlGenDate.Visible = False
    mnuDate.Visible = False
    fraDumDates.Visible = True
  End If
End Sub

Private Sub GetCommonDates()
'  Dim Strt&(), Stp&(), TUnits&(), TSteps&()
  Dim lb&(6), le&(6)
'  Dim TUnit&, TSTEP&
'  Dim gpflg&, tdsfrc&, retcod&
  Dim j&, i&, k&, d$, jm1
  Dim FirstStart As Double, LastEnd As Double
  Dim LastStart As Double, FirstEnd As Double
  Dim ThisStart As Double, ThisEnd As Double
  
  FirstStart = 1E+30
  LastEnd = -1E+30
  
  FirstEnd = 1E+30
  LastStart = -1E+30
  
  If Tser.Count = 0 Then
    Call DispDates(False)
    lblDumDates.Caption = "No Dates are available until Timeseries are Selected"
  Else
'    ReDim TUnits(Tser.Count)
'    ReDim TSteps(Tser.Count)
'    ReDim Strt(6 * Tser.Count)
'    ReDim Stp(6 * Tser.Count)
'
'    gpflg = 1
'    TUnit = 6
'    jm1 = -1
    For j = 1 To Tser.Count
      ThisStart = Tser(j).dates.Summary.SJDay
      ThisEnd = Tser(j).dates.Summary.EJDay
      If Abs(ThisStart) > 2 And Abs(ThisEnd) > 2 Then
'        jm1 = jm1 + 1 'index for FORTRAN arrays
        
        If ThisStart < FirstStart Then FirstStart = ThisStart
        If ThisEnd > LastEnd Then LastEnd = ThisEnd
        
        If ThisStart > LastStart Then LastStart = ThisStart
        If ThisEnd < FirstEnd Then FirstEnd = ThisEnd
        
'        Call J2Date(ThisStart, lb)
'        Call J2Date(ThisEnd, le)
'        For i = 0 To 5
'          k = i + jm1 * 6
'          Strt(k) = lb(i)
'          Stp(k) = le(i)
'        Next i
'        TSteps(jm1) = Tser(j).dates.Summary.ts
'        TUnits(jm1) = Tser(j).dates.Summary.Tu
      End If
    Next j
    If LastStart >= FirstEnd Then 'No common dates - just use all dates
      LastStart = FirstStart
      FirstEnd = LastEnd
    End If
'    If jm1 < 0 Then
'      retcod = 0
'      CSDat(0) = 0
'      CEDat(0) = 0
'    Else
'      Call DTMCMN(Strt, Stp, TSteps, TUnits, CSDat, CEDat, TSTEP, TUnit, retcod)
    If LastEnd > FirstStart Then
      Call J2Date(LastStart, CSDat)
      Call J2Date(FirstEnd, CEDat)
      Call J2Date(FirstStart, lb)
      Call J2Date(LastEnd, le)
    End If
'    End If
'    If retcod < 0 Then
'      'lblDumDates.Caption = "No Common Period for Selected Timeseries"
'      'Call DispDates(False)
'      For i = 0 To 5
'        CSDat(i) = lb(i)
'        CEDat(i) = le(i)
'      Next i
'      retcod = 0
'    End If
'    If retcod = 0 Then
      If CSDat(0) = 0 And CEDat(0) = 0 Then
        'no data in these data sets
        Call DispDates(False)
        lblDumDates.Caption = "No Data Available in Selected Timeseries."
      Else
        Call timcnv(CEDat)
        Call timcnv(le)
        CSDat(3) = 0: CSDat(4) = 0: CSDat(5) = 0
        CEDat(3) = 0: CEDat(4) = 0: CEDat(5) = 0
    
'        If TUnit < ctlGenDate.TUnit Then
'          ctlGenDate.TUnit = TUnit
'        End If
        ctlGenDate.LimtS = DateSerial(lb(0), lb(1), lb(2)) ' 1)
        ctlGenDate.LimtE = DateSerial(le(0), le(1), le(2)) ' daymon(CEDat(0), CEDat(1))) '
        ctlGenDate.CommS = DateSerial(CSDat(0), CSDat(1), CSDat(2)) ' 1)
        ctlGenDate.CommE = DateSerial(CEDat(0), CEDat(1), CEDat(2)) ' daymon(CEDat(0), CEDat(1))) '
'        If ctlGenDate.CurrS < ctlGenDate.LimtS Then
'          ctlGenDate.CurrS = ctlGenDate.LimtS
'        End If
'        If ctlGenDate.CurrE > ctlGenDate.LimtE Then
'          ctlGenDate.CurrE = ctlGenDate.LimtE
'        End If
        ctlGenDate.Width = fraDumDates.Width
        Call DispDates(True)
      End If
'    End If
  End If
End Sub

Sub ManageProject(i%)

  If i = 0 Then 'new project
    If p.StatusFileName <> "" Then mnuClose_Click
    If p.StatusFileName <> "" Then Exit Sub
    InitATCoTSer
    Unload frmGenScnManageProject
    p.StatusFilePath = ""
    frmGenScnManageProject.Show '(vbModal)
  ElseIf i = 1 Then 'open existing project
    Call OpenStatusFile("")
  ElseIf i = 2 Then 'edit project
    frmGenScn.Enabled = False
    frmGenScnManageProject.Show '(vbModal)
  End If

End Sub

'Public Sub grdDsnRemove(i&)
'  If i > 0 And i <= grdDsn.Rows Then
'    DbgMsg "TSLstMan:Remove:Row " & i + 1 & " " & TSer(i).Header.id, 6, "frmGenScn", "t"
'    grdDsn.DeleteRow i
'    ResyncTSerToList
'    Call GetCommonDates
'  End If
'End Sub
'
'Public Function grdDsnSelected(i&) As Boolean
'  If i > 0 And i <= grdDsn.Rows Then 'LstDsn.ListItems.Count Then
'    grdDsnSelected = grdDsn.Selected(i, 0)
'  Else
'    grdDsnSelected = False
'  End If
'End Function

Public Function grdDsnSelected() As Long
  Dim tempds As ATCclsTserData
  Set tempds = TimserGrid.FirstSelected
  If tempds Is Nothing Then
    grdDsnSelected = 0
  Else
    grdDsnSelected = tempds.Header.ID
  End If
End Function

Public Sub RefreshMain()
  Dim ObservedFlag As Boolean, i&, vName As Variant
  On Error Resume Next 'some controls no longer exist, just ignore that
  lstSLC(0).Clear 'scenarios
  ObservedFlag = False
  For Each vName In p.ScenName
    lstSLC(0).AddItem vName
    If vName = "OBSERVED" Then ObservedFlag = True
  Next
  If Not ObservedFlag Then lstSLC(0).AddItem "OBSERVED"
  lstSLC_GotFocus (0)
  MapRefresh = True
  
  lstSLC(1).Clear 'locations
  ObservedFlag = False
  For Each vName In p.LocnName
    lstSLC(1).AddItem vName
  Next
  lstSLC_GotFocus (1)
  
  lstSLC(2).Clear 'constituents
  For Each vName In p.ConsName
    lstSLC(2).AddItem vName
  Next vName
  lstSLC_GotFocus (2)
  TimserGrid.Enabled = True 'grdDsn.Rows = 0 'LstDsn.ListItems.Clear
  Set Tser = Nothing
  Set Tser = New Collection
  'lstDsn_GotFocus
  For i = 0 To cmdSLC.Count + 1 'slc all none
    cmdSLC(i).Enabled = True
  Next i
  For i = 0 To cmdSen.Count - 1
    cmdSen(i).Enabled = True 'act,del,new
  Next i
  For i = 0 To lstSLC.Count
    lstSLC(i).Enabled = True
    lstSLC(i).BackColor = vbWhite
    If i < 3 Then lblSLC(i).Enabled = True
  Next i
  fraAnal.Enabled = True
  fraDumDates.Enabled = True
  For i = 0 To fraSLC.Count ' includes map which includes loc
    fraSLC(i).Enabled = True
  Next i
  fraTimser.Enabled = True
  
  For i = 0 To cmdAna.Count - 1
    cmdAna(i).Enabled = True
  Next i
  lblDumDates.Enabled = True
  UpdateLblDsn
  mnuClose.Enabled = True
  mnuSave(0).Enabled = True
  mnuSave(1).Enabled = True
  mnuSave(2).Enabled = True
  mnuEdit.Enabled = True
  Map1.Visible = True
  For i = 0 To optSLC.Count - 1
    optSLC(i).Enabled = True
  Next i
  For i = 1 To mnuX.Count - 1
    mnuX(i).Visible = True
  Next i
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
  UpdateLblDsn
  Call GetCommonDates
  ctunit = ctlGenDate.TUnit
  CTStep = ctlGenDate.TSTEP
End Sub

Public Sub OpenStatusFile(ByVal Filename As String)
  Dim i&, j&, x%, S$, istr$, d$, h$, StrTyp$, fun%, k$
  Dim exeIndex&, label$, path$, Value$, filter$
  Dim isFile As Boolean, isOutput As Boolean, isOnCommandline As Boolean
  
  If p.StatusFileName <> "" Then mnuClose_Click
  If p.StatusFileName <> "" Then Exit Sub
  
  On Error GoTo errhandler
  DbgMsg "OpenStatusFile:", 7, "frmGenScn", "i"
  Call InitATCoTSer
  
  If Len(Filename) = 0 Then 'no command line file
    x = 1
  
    cdl.Filename = GetSetting("GenScn", "Open", "LastStatus", "")
    If Len(cdl.Filename) = 0 Then
      cdl.Filename = ExePath
      i = InStr(UCase(ExePath), "BASINS\")
      If i > 0 Then
        cdl.Filename = Left(cdl.Filename, i + 6) & "data\*.sta"
      Else
        cdl.Filename = PathNameOnly(cdl.Filename) & "\*.sta"
      End If
    End If
    ChDriveDir PathNameOnly(cdl.Filename)
    cdl.DialogTitle = "GenScn File Open Project"
    cdl.CancelError = True
    cdl.flags = &H1804& 'not read only
    cdl.filter = "Project files (*.sta)|*.sta"
    cdl.ShowOpen
    p.StatusFileName = cdl.Filename
    p.StatusFilePath = PathNameOnly(p.StatusFileName)
    SaveSetting "GenScn", "Open", "LastStatus", p.StatusFileName
  Else
    p.StatusFileName = Filename 'name from command line
    p.StatusFilePath = PathNameOnly(Filename)
    ChDriveDir p.StatusFilePath
  End If
  AddRecentFile mnuRecent, p.StatusFileName
  
  DbgMsg "OpenStatusFile:name:" & p.StatusFileName, 3, "frmGenScn", "t"
      
  MousePointer = vbHourglass
  
  If p.HSPFMsg.Unit > 0 Then
    'message file already open
    DbgMsg "OpenStatusFile:Close open HSPFMsg on " & p.HSPFMsg.Unit, 7, "frmGenScn", "t"
    i = F90_WDFLCL(p.HSPFMsg.Unit)
    If i <> -87 Then 'cant close last time
      'closed properly
      p.HSPFMsg.Unit = 0
      p.HSPFMsg.Name = ""
    Else
      DbgMsg "OpenStatusFile:Close open HSPFMsg PROBLEM:" & i, 2, "frmGenScn", "e"
    End If
  End If

  MyRchAvail = False

  InitSLCCollections

  AllowScenarioModify True
  x = 2
  fun = FreeFile(0)
  Open p.StatusFileName For Input As #fun
  'p.StatusFilePath = CurDir
  p.EditFlg = False
  p.PreferredUnits.Clear
  p.UnitsRequired = False
  
  DbgMsg "OpenStatusFile:open file and set CurDir to " & CurDir, 3, "frmGenScn", "t"

  While Not EOF(fun)
    x = 6
    Line Input #fun, istr
    DbgMsg "OpenStatusFile:read:" & istr, 8, "frmGenScn", "t"
    istr = Trim(istr)
    StrTyp = Trim(UCase(StrRetRem(istr)))
    Select Case StrTyp
    Case "HID"
      S = UCase(StrRetRem(istr))
      If S = "SCENARIOMODIFY" Then AllowScenarioModify False
    Case "MES"      'message file name
      'DbgMsg "OpenStatusFile:HSPFMsg:" & istr, 5, "frmGenScn", "t"
      x = 3
      S = istr
      If Not Batch.OpenHspfMsg(S) Then
        MsgBox "Could not open HSPF message file " & S & vbCr & p.HSPFMsg.Name & vbCr & "Aborting reading of status file", vbOKOnly
        Exit Sub
      End If
    Case "MAP"      'map status file name
      'DbgMsg "OpenStatusFile:MapStatus:" & istr, 5, "frmGenScn", "t"
      x = 4
      p.MapName = istr
      'get reach info from database
      S = ""
      If Len(Dir(p.MapName)) > 0 Then
        Map1.SetMapData PathNameOnly(p.MapName), FilenameNoPath(p.MapName), S
      ElseIf Len(Dir(p.StatusFilePath & "\" & p.MapName)) > 0 Then
        Map1.SetMapData p.StatusFilePath, p.MapName, S
      End If
      p.MapName = Map1.MapFilePath & Map1.MapFileName
      'sstLocation.TabCaption(1) = Map1.Layers(0).Name
      'DbgMsg "OpenStatusFile:MapDataSet", 5, "frmGenScn", "t"
      x = 5
      If Len(S) > 0 Then
        'we have a map layer to animate
        'DbgMsg "OpenStatusFile:Animate:" & s, 5, "frmGenScn", "t"
        Call ReadShapeLine(S, MyRch, Map1)
        MyRchAvail = True
      End If
    Case "SCN"
      'DbgMsg "OpenStatusFile:SCN Data: " & p.ScenCount + 1 & " " & istr, 5, "frmGenScn", "t"
      k = StrRetRem(istr)
      'If Len(k) > 0 Then DbgMsg "    SCN Name: " & k, 6, "frmGenScn", "t"
      p.ScenName.Add k, k
      S = StrRetRem(istr)
      'If Len(s) > 0 Then DbgMsg "    SCN Type: " & s, 6, "frmGenScn", "t"
      p.ScenType.Add S, k
      S = StrRetRem(istr)
      'If Len(s) > 0 Then DbgMsg "    SCN File: " & s, 6, "frmGenScn", "t"
      p.ScenFile.Add S, k
      S = StrRetRem(istr)
      'If Len(s) > 0 Then DbgMsg "    SCN Desc: " & s, 6, "frmGenScn", "t"
      p.ScenDesc.Add S, k
    Case "CON"
      'DbgMsg "OpenStatusFile:CON Data: " & p.ConsCount + 1 & " " & istr, 5, "frmGenScn", "t"
      k = StrRetRem(istr)
      'If Len(k) > 0 Then DbgMsg "    CON Name: " & k, 6, "frmGenScn", "t"
      p.ConsName.Add k, k
      S = StrRetRem(istr)
      'If Len(s) > 0 Then DbgMsg "    CON Desc: " & s, 6, "frmGenScn", "t"
      p.ConsDesc.Add S, k
    Case "LOC"
      'DbgMsg "OpenStatusFile:LOC Data: " & p.LocnCount + 1 & " " & istr, 5, "frmGenScn", "t"
      'ReDim Preserve p.Locn(p.LocnCount)
      k = StrRetRem(istr)
      'If Len(k) > 0 Then DbgMsg "    LOC Name: " & k, 6, "frmGenScn", "t"
      p.LocnName.Add k, k
      S = StrRetRem(istr)
      'If Len(s) > 0 Then DbgMsg "    LOC Desc: " & s, 6, "frmGenScn", "t"
      p.LocnDesc.Add S, k
      'lblLocationsSelected.Caption = map1.PointsSelected " of " & p.LocnCount
    Case "SWX"  'swmm executable name
      DbgMsg "OpenStatusFile:SWMM executable: " & istr, 5, "frmGenScn", "t"
      Load mnuAnal(mnuAnal.Count)
      mnuAnal(mnuAnal.Count - 1).Caption = "&SWMM"
      mnuAnal(mnuAnal.Count - 1).Tag = 0
      SWExeName = istr
    Case "EXE"  'external executable name
      DbgMsg "OpenStatusFile:executable: " & istr, 5, "frmGenScn", "t"
      label = StrRetRem(istr)
      path = StrRetRem(istr)
      exeIndex = atEXE.AddEXE(label, path)
      Load mnuAnal(mnuAnal.Count)
      mnuAnal(mnuAnal.Count - 1).Caption = label
      mnuAnal(mnuAnal.Count - 1).Tag = exeIndex
      
      If InStr(UCase(label), "SWMM") > 0 Then SWExeName = path
      If InStr(UCase(label), "STRMDEPL") > 0 Then SDExeName = path
    Case "EXO", "EXI"  'external executable input or output file (refers to recent EXE in file)
      isFile = True
      label = StrRetRem(istr)
      Value = StrRetRem(istr)
      filter = StrRetRem(istr)
      If StrTyp = "EXO" Then isOutput = True Else isOutput = False
      If InStr(UCase(istr), "NOT") Then isOnCommandline = False Else isOnCommandline = True
      atEXE.AddDetail label, Value, filter, isFile, isOutput, isOnCommandline
    Case "EXA"  'external executable argument (refers to recent EXE in file)
      isFile = False
      label = StrRetRem(istr)
      Value = StrRetRem(istr)
      filter = ""
      If InStr(1, UCase(istr), "NOT") Then isOnCommandline = False Else isOnCommandline = True
      atEXE.AddDetail label, Value, filter, isFile, False, isOnCommandline
    Case "TSC" 'Timeseries Columns
      With TimserGrid
        Value = StrRetRem(istr)
        If IsNumeric(Value) Then
          '.NtsCol = CLng(value)
          i = 0
          Value = StrRetRem(istr)
          While Value <> ""
            On Error Resume Next
            .VisibleAttributes.Add Value
            i = i + 1
            Value = StrRetRem(istr)
            On Error GoTo errhandler
          Wend
          .PopulateGrid
        End If
      End With
    Case "UNI" 'Preferred Units. Example: UNI Length ft
      S = StrRetRem(istr)
      Value = StrRetRem(istr)
      If LCase(S) = "required" Then
        Select Case LCase(Left(Value, 1))
          Case "t", "y":  p.UnitsRequired = True  'True or Yes, else leave it false
        End Select
      Else
        p.PreferredUnits.Add Value, S
      End If
    Case ""
      'ignore blank lines
    Case Else 'atcotimser
      If StrTyp = "WDM" Then StrRetRem istr
      
      If FileExists(istr) Then
        Batch.OpenFile StrTyp, istr
      ElseIf FileExists(p.StatusFilePath & "\" & istr) Then
        Batch.OpenFile StrTyp, p.StatusFilePath & "\" & istr
      Else 'Try opening it anyway, maybe it isn't a file
        Batch.OpenFile StrTyp, istr
      End If
    End Select
  Wend
  x = 7
  'On Error GoTo 0
  Call RefreshSLC
  'Call RefreshMain
  frmGenScn.Caption = "GenScn: " & FilenameOnly(p.StatusFileName)
  Close #fun
  Map1.UnsavedChanges = False
  If Map1.LayerCount < 1 Then ChangeToList
  On Error Resume Next
  frmGenScn.SetFocus
  On Error GoTo 0
  MousePointer = vbDefault
  
Exit Sub

errhandler:
  MousePointer = vbDefault
  h = "GenScn Initialization Problem"
  DbgMsg "OpenStatusFile:Problem: " & x & " " & istr, 2, "frmGenScn", "e"
  Select Case x
    Case 1: 'user pressed Cancel of open of status 'MsgBox "You must choose the name of a status file.", vbExclamation, h
    Case 2: MsgBox "Error opening status file" & vbCr & err.Description, vbExclamation, h
            Exit Sub
    Case 3: MsgBox "Could not find WDM file " & S, vbExclamation, h 'bad wdm file
            Exit Sub
    Case 4: MsgBox "Could not find Map file " & p.MapName, vbExclamation, h 'problem with map database
    Case 5: MsgBox "Map:" & p.MapName & vbCr & "  Error:" & err.Description, vbExclamation, h: Resume Next
    Case 6: MsgBox "Error:" & err.Description & vbCrLf & "near status file line: " & istr, vbExclamation, h
    Case 7: MsgBox "Error after loading status file" & vbCr & err.Description, vbExclamation, h
  End Select
  On Error Resume Next
  Close #fun
End Sub

Public Sub SaveStatusFile()
    Dim sp$              'path to status file
    Dim S$               'string to be written to status file
    Dim i&
    Dim fl%              'file pointer of status file being written
    Dim vTserFile As Variant 'plugin we iterate through
    Dim detail As Variant 'command line for external executable to store in status file
    Dim exeNum&           'iterate through external executables
    Dim tsColNum&
    Dim curClsTserFile As ATCclsTserFile
    
    On Error GoTo errhandler
   
    fl = FreeFile(0)
    Open p.StatusFileName For Output As #fl
    sp = PathNameOnly(p.StatusFileName)
    'message file name
    If p.HSPFMsg.Name <> "" Then Print #fl, "MES " & RelativeFilename(p.HSPFMsg.Name, sp)
    'map database file name
    If Map1.MapFileName <> "" Then
      p.MapName = RelativeFilename(Map1.MapFileName, sp)
      Print #fl, "MAP " & p.MapName
    End If
    i = 0
    For Each vTserFile In TserFiles.Active
      Set curClsTserFile = vTserFile.obj
      If curClsTserFile.label = "WDM" Then
        i = i + 1
        Print #fl, curClsTserFile.label & " " & _
                   curClsTserFile.label & i & " " & _
                   RelativeFilename(curClsTserFile.Filename, sp)
      ElseIf curClsTserFile.label = "In-Memory" Then
        'doesn't make sense to save a reference to in-memory data
      Else
        Print #fl, curClsTserFile.label & " " & RelativeFilename(curClsTserFile.Filename, sp)
      End If
    Next vTserFile
    
    If p.UnitsRequired Then Print #fl, "UNI Required True"
    
    If Not p.PreferredUnits Is Nothing Then
      For i = 1 To p.PreferredUnits.Count
        Print #fl, "UNI " & p.PreferredUnits.Key(i) & " " & p.PreferredUnits.ItemByIndex(i)
      Next
    End If
    
    On Error Resume Next
    Dim vName As Variant
    For Each vName In p.ScenName           'scenarios
      S = "SCN " & vName
      S = S & "," & p.ScenType(vName)
      S = S & "," & RelativeFilename(p.ScenFile(vName), sp)
      S = S & ",'" & p.ScenDesc(vName) & "'"
      Print #fl, S
    Next
    
    For Each vName In p.ConsName           'constituents
      If Len(p.ConsDesc(vName)) > 0 Then
        Print #fl, "CON " & vName & ",'" & p.ConsDesc(vName) & "'"
      End If
    Next

    For Each vName In p.LocnName           'locations
      If Len(p.LocnDesc(vName)) > 0 Then
        Print #fl, "LOC " & vName & ",'" & p.LocnDesc(vName) & "'"
      End If
    Next
    
    If SWExeName <> "" Then Print #fl, "SWX " & SWExeName   'swmm executable name
    
    For exeNum = 1 To atEXE.nEXE                        'Other executable names
      S = "EXE '" & atEXE.EXElabel(exeNum) & "','" & atEXE.ExePath(exeNum) & "'"
      Print #fl, S
      For Each detail In atEXE.EXEdetails(exeNum)
        If detail.isFile Then
          If detail.isOutput Then S = "EXO '" Else S = "EXI '"
          S = S & detail.label & "','" & detail.Value & "','" & detail.filter & "'"
        Else
          S = "EXA '" & detail.label & "','" & detail.Value & "'"
        End If
        If Not detail.isOnCommandline Then S = S & ",NotCommandline"
        Print #fl, S
      Next detail
    Next exeNum
    
    With TimserGrid
      S = "TSC " & .VisibleAttributes.Count
      For tsColNum = 1 To .VisibleAttributes.Count
        S = S & ",'" & .VisibleAttributes(tsColNum) & "'"
      Next
      Print #fl, S
    End With
    
    Close #fl
    
    p.EditFlg = False
    frmGenScn.Caption = "GenScn: " & FilenameOnly(p.StatusFileName)
    
Exit Sub
errhandler:
    MsgBox "Error opening project file " & p.StatusFileName & vbCr & err.Description, vbExclamation, "GenScn Project File Save Problem"
End Sub

Private Sub SaveStatusFileAs()
    On Error GoTo 10
    cdl.DialogTitle = "GenScn File Save Project As"
    cdl.CancelError = True
    cdl.flags = &H3804& 'create & not read only
    cdl.filter = "Project files (*.sta)|*.sta"
    cdl.Filename = p.StatusFileName
    cdl.ShowSave
    p.StatusFileName = cdl.Filename
    Call SaveStatusFile
    AddRecentFile mnuRecent, p.StatusFileName
10 'continue
End Sub

Sub SyncLists(l As ListBox, M As ListBox)
   Dim i%, j%
   If l.ListCount > 0 Then
     For i = 0 To l.ListCount - 1
       For j = 0 To M.ListCount - 1
         If l.List(i) = M.List(j) Then
           M.Selected(j) = l.Selected(i)
         End If
       Next j
     Next i
   End If
End Sub

Sub TimeseriesAdd()
  Dim crit As ATCclsCriterion
  Dim i&, Msg$
  
  TimserGrid.ClearRules
  
  If fraLocations.Visible Then
    If lstSLC(1).SelCount = 0 Then
      Msg = Msg & "No Location specified." & vbCrLf
      scntLoc = -1
    End If
  Else
    If Map1.SelCount = 0 Then
      Msg = Msg & "No Location specified." & vbCrLf
      scntLoc = -1
    Else
      'Map1.GetSelectedKeys specLoc
      'scntLoc = UBound(specLoc) - 1
    End If
  End If
  If scntLoc >= 0 Then
    Set crit = New ATCclsCriterion
    crit.Field = "Loc"
    For i = 0 To scntLoc
      crit.Values.Add specLoc(i)
    Next
    TimserGrid.AddRule crit
    Set crit = Nothing
  End If
  
  If lstSLC(0).SelCount = 0 Then 'scenarios
    Msg = Msg & "No scenarios specified." & vbCrLf
  ElseIf Len(specSen(0)) > 0 Then
    Set crit = New ATCclsCriterion
    crit.Field = "Sen"
    For i = 0 To scntSen
      crit.Values.Add specSen(i)
    Next
    TimserGrid.AddRule crit
    Set crit = Nothing
  End If
  
  If lstSLC(2).SelCount = 0 Then 'constituents
    Msg = Msg & "No constituents specified." & vbCrLf
  ElseIf Len(specCon(0)) > 0 Then
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

Sub UpdateCount(l As ListBox, t As label, i%)
    Dim S$
    
    S = l.SelCount & " of " & l.ListCount
    t.Caption = S
    DbgMsg "UpdateCount:" & sSLC(i) & ": " & S, 5, "frmGenScn", "t"

End Sub

Sub UpdateSLC(i As Integer, opt As Integer)
  Dim S$, j%, k%, lA As ListBox, lb As ListBox
' Debug.Print "UpdateSLC ", i, opt
  If MapRefresh Then MousePointer = vbHourglass
  If lstSLC(i).SelCount = lstSLC(i).ListCount Then
    k = 0
  Else
    k = lstSLC(i).SelCount
  End If
  DbgMsg "UpdateSLC: " & sSLC(i) & ":" & i & " " & opt & " " & k, 8, "frmGenScn", "t"
  Select Case i
    Case 0, 3: ReDim specSen(k): scntSen = k - 1: If k = 0 Then specSen(0) = ""
    Case 1:    ReDim specLoc(k): scntLoc = k - 1: If k = 0 Then specLoc(0) = ""
    Case 2, 4: ReDim specCon(k): scntCon = k - 1: If k = 0 Then specCon(0) = ""
    Case Else 'site?
  End Select
 
  If k > 0 Then
    k = -1
    For j = 0 To lstSLC(i).ListCount - 1
      If lstSLC(i).Selected(j) Then
        k = k + 1
        Select Case i
          Case 0, 3: specSen(k) = lstSLC(i).List(j)
          Case 1:    specLoc(k) = lstSLC(i).List(j)
          Case 2, 4: specCon(k) = lstSLC(i).List(j)
        End Select
      End If
    Next j
  End If

  Set lA = lstSLC(i)
  If i < 4 Then
    j = i Mod 3
  Else
    j = 2
  End If
  Call UpdateCount(lA, lblSLC(j), j)
  If i > 2 Then
    Set lb = lstSLC(j)
    Call SyncLists(lA, lb)
  End If
  
  If i = 1 Then LocationSelChange

  If MapRefresh Then MousePointer = vbDefault
End Sub

Public Sub UpdateLblDsn()
  fraTimser.Caption = "Time Series (" & TimserGrid.VisibleList.Count & " of " & CntDsn & ")"
End Sub

Private Sub cmdAna_Click(index As Integer)
  Call DoAnal(cmdAna(index).Tag)
End Sub

Private Sub cmdSen_Click(index As Integer)

  Dim i&, j&, k&
  Dim S$, f$, n$
  Dim lTs As Collection, ts As ATCclsTserData
  Dim vWDMFile As Variant

  DbgMsg "cmdSen:Click: " & index, 3, "frmGenScn", "m"
  If index = 2 Then 'new
    If p.HSPFMsg.Unit = 0 Then   'only works if we know about hspf 'Or p.WDMData.Unit = 0
      MsgBox "No HSPF Message or Data WDM File is Available", _
             vbExclamation, "GenScn " & cmdSen(index).Caption & " Problem"
    Else
      Unload frmGenScnNewScen
      frmGenScnNewScen.Show
    End If
  Else
    If lstSLC(0).SelCount = 1 Then
      If p.HSPFMsg.Unit = 0 Then  'only works if we know about hspf 'Or p.WDMData.Unit = 0
        MsgBox "No HSPF Message or Data WDM File is Available", _
               vbExclamation, "GenScn " & cmdSen(index).Caption & " Problem"
      Else
        On Error GoTo nofile
        For i = 1 To lstSLC(0).ListCount
          If lstSLC(0).Selected(i - 1) Then
            lstSLC(0).ListIndex = i - 1
          End If
        Next i
        n = lstSLC(0).List(lstSLC(0).ListIndex)
        If index = 0 Then ' activate
          If UCase(n) = "OBSERVED" Then
            'activate observed, enter more observed data
            Unload frmGenActObs
            frmGenActObs.Show
          Else 'hspf only supported
            ChDriveDir p.StatusFilePath 'assume default location in statusfilepath (or relative to it)
            f = p.ScenFile(n)
            ChDriveDir PathNameOnly(f)
            f = n & ".uci"
            If Len(Dir(f)) = 0 Then
LenDirZero:
              cdl.DialogTitle = "Locate Scenario UCI"
              cdl.CancelError = True
              cdl.filter = "User Control Input (*.uci)|*.uci"
              cdl.Filename = f
              cdl.ShowOpen
              f = RelativeFilename(cdl.Filename, CurDir)
              If Len(Dir(f)) = 0 Then GoTo LenDirZero
              p.ScenFile.Remove n
              p.ScenFile.Add f, n
              p.EditFlg = True
            End If
            Open f For Input As #1
            Close #1
            'Me.Enabled = False
            'MousePointer = vbHourglass
            'ScenFile = f
            cmdSen(0).Enabled = False
            cmdSen(1).Enabled = False
            cmdSen(2).Enabled = False
            Load frmGenScnActivate
            frmGenScnActivate.ScenFile = f
          End If
        ElseIf index = 1 Then 'delete
          If UCase(n) = "OBSERVED" Then
            'Cannot delete observed
            MsgBox "Cannot delete 'Observed' scenario", _
                   vbExclamation, "GenScn " & cmdSen(index).Caption & " Problem"
          ElseIf lstSLC(0).ListCount < 2 Then
            MsgBox "Cannot delete the only scenario", _
                   vbExclamation, "GenScn " & cmdSen(index).Caption
          Else
            Dim rsp%, icnt&
            'count the number of wdm datasets to be deleted
            Call FindTimSer(n, "", "", lTs)
            icnt = 0
            For i = 1 To lTs.Count
              If TypeOf lTs(i).File Is clsTSerWDM Then
                icnt = icnt + 1
              End If
            Next
            If icnt = 0 Then
              rsp = MsgBox("Do you want to Delete Scenario '" & n & "'?", _
                           vbYesNo, "GenScn Delete Query")
            Else
              rsp = MsgBox("Do you want to Delete Scenario '" & n & "'?" & vbCrLf & _
                         vbCrLf & CStr(icnt) & " WDM Time Series will be permanently deleted.", vbYesNo, "GenScn Delete Query")
            End If
            If rsp = vbYes Then
              'get count of number of data sets to be deleted
              MousePointer = vbHourglass
              For Each ts In lTs
                ts.File.RemoveTimSer ts
              Next ts
              'delete this scenario
              'Call F90_DELSCN(Left(n, Len(n)), Len(n))
              'reset scenario list and number of data sets
              On Error Resume Next
              p.ScenDesc.Remove n
              p.ScenFile.Remove n
              p.ScenType.Remove n
              p.ScenName.Remove n
              'lstSLC(0).RemoveItem lstSLC(0).ListIndex
              For Each vWDMFile In p.WDMFiles
                vWDMFile.Refresh
              Next
              RefreshSLC

              CntDsn = CountAllTimser ' CntDsn - lts.Count
              UpdateLblDsn
              'lstDsn_GotFocus
              S = lstSLC(0).SelCount & " of " & lstSLC(0).ListCount
              lblSLC(0).Caption = S
              p.EditFlg = True
              'Need to clear non-existent data sets from list
              MousePointer = vbDefault
              DoEvents
            End If
          End If
        End If
      End If
    Else
      MsgBox "Scenario " & cmdSen(index).Caption & " requires one and only one Scenario to be Selected." & vbCrLf _
        & "You have selected " & lstSLC(0).SelCount & ".", _
        vbExclamation, "GenScn " & cmdSen(index).Caption
    End If
  End If
  Exit Sub
nofile:
  If err.Number <> 55 Then
    MsgBox "Scenario " & cmdSen(index).Caption & " data file " & f & " not found", _
        vbExclamation, "GenScn " & cmdSen(index).Caption
    Exit Sub
  Else
    Resume Next
  End If
End Sub

Private Sub cmdSLC_Click(index As Integer)
  Dim i%, j%, lA As ListBox, lb As ListBox
  Dim n As Boolean
  
  DbgMsg "cmdSLC:Click: " & index, 3, "frmGenScn", "m"
  MapRefresh = False
  MousePointer = vbHourglass
  If index < 3 Then
    n = True
  Else
    n = False
  End If
  i = index Mod 3
  If i = 0 Then 'scenario
    If optSLC(0).Value = True Then
      Set lA = lstSLC(0)
      Set lb = lstSLC(3)
    Else
      Set lA = lstSLC(3)
      Set lb = lstSLC(0)
    End If
  ElseIf i = 2 Then 'cons
    If optSLC(2).Value = True Then
      Set lA = lstSLC(2)
      Set lb = lstSLC(4)
    Else
      Set lA = lstSLC(4)
      Set lb = lstSLC(2)
    End If
  Else 'loc
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
  Call UpdateCount(lA, lblSLC(i), i)
  If i = 0 Then
    Call SyncLists(lA, lb) 'scen
  ElseIf i = 2 Then
    Call SyncLists(lA, lb) 'con
  End If
  MousePointer = vbDefault
  MapRefresh = True
  'If i = 1 Then 'loc
    Call UpdateSLC(i, 0)
  'End If
End Sub

'Private Sub ctlGenDate_Change()
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
  ctunit = ctlGenDate.TUnit
  CTStep = ctlGenDate.TSTEP
  CDTran = ctlGenDate.TAggr
  If CDTran < 2 Then ' kludge for diff convention in ATCoDate
    CDTran = Abs((CDTran - 1)) Mod 2
  End If
End Sub

Private Sub Form_Load()
  Dim i&, colstr$
  Dim vAnalysis As Variant
  
  IPC.dbg "frmGenScn Load RetrieveWindowSettings"
  
  RetrieveRecentFiles mnuRecent, AppName
  RetrieveWindowSettings Me, AppName, SectionMainWin
  colstr = GetSetting(AppName, SectionMainWin, "MapWidth")
  If IsNumeric(colstr) Then
    If colstr > 50 And colstr < Me.ScaleWidth * 0.9 Then sashV.Left = colstr
  End If
  SashVdragging = True
  sashV_MouseMove 1, 0, 0, 0
  SashVdragging = False
  
  ctlGenDate.TAggr = 4 'default to Native mode in date control
  
  DbgMsg "Form_Load:ExeName:" & ExeName, 3, "frmGenScn", "L"
  'If Len(Command$) > 0 Then
  '  DbgMsg "Form_Load:CmdLin:" & ExCmd, 3, "frmGenScn", "L"
  'End If
  'IPC.dbg "frmGenScn Load Set Map1.DebugControl"
  'Set Map1.DebugControl = dbg
  
  Set Map1.IPC = IPC 'FIXME
  
  On Error Resume Next
  'With tbrAnal.Buttons
  '  For i = 1 To .Count
  '    If .Item(i).Image = 0 Then .Item(i).Image = .Item(i).key
  '  Next i
  'End With
  
  'Try setting pathnames of cursor files
  
  'Launch.dbg "frmGenScn Load lstSLC0"
  
  'For i = 0 To p.ScenCount - 1
  '  lstSLC(0).AddItem p.Scen(i).Name
  'Next i
  'lstSLC_GotFocus (0)
  
  'Launch.dbg "frmGenScn Load lstSLC2"
  
  'For i = 0 To p.ConsCount - 1
  '  lstSLC(2).AddItem p.Cons(i).Name
  'Next i
  'lstSLC_GotFocus (2)
  
  'Launch.dbg "frmGenScn Load GetCommonDates"
  'Call GetCommonDates
  
  'LstDsn.SortKey = 1
  
  ctlGenDate.Caption = "Dates"
  ctlGenDate.Visible = False
  mnuDate.Visible = False
  Set tsList = New ATCoTSlist
  tsList.HelpFileName = App.HelpFile
  
'  For Each vAnalysis In Analyses.Avail
'    Load mnuAnalPlugIn(mnuAnalPlugIn.Count)
'    With mnuAnalPlugIn(mnuAnalPlugIn.Count - 1)
'      .Caption = vAnalysis.label
'      .Visible = True
'    End With
'  Next

'  If Len(ExCmd) > 0 Then
'    If Len(Dir(ExCmd)) > 0 Then 'might be a status file
'      Call OpenStatusFile(ExCmd)
'    End If
'  End If
End Sub

Private Sub Form_Resize()
  
  Dim fh%, fw%, fhMinusMenus%, vSpaceLeft%
  fh = frmGenScn.ScaleHeight
  fw = frmGenScn.ScaleWidth
  fhMinusMenus = fh '- 600 '753
  If fhMinusMenus < 10 Then Exit Sub 'fhMinusMenus = 10
  fraSLC(1).Height = fhMinusMenus
  sashV.Height = fraSLC(1).Height
  sashV.Left = fw - fraAnal.Width - sashV.Width '- 150
  If sashV.Left < 11 Then
    sashV.Left = 11
    SashVdragging = True
    sashV_MouseMove 0, 0, 0, 0
    SashVdragging = False
  Else
    If fraSLC(1).Height > 3120 Then Map1.Height = fraSLC(1).Height - 360
    'If fw > 5300 Then fraSLC(1).Width = fw - 5223
    If sashV.Left > 0 Then fraSLC(1).Width = sashV.Left
    If fraSLC(1).Width > 250 Then Map1.Width = fraSLC(1).Width - 240
    fraLocations.Height = Map1.Height
    fraLocations.Width = Map1.Width
    lstSLC(1).Height = fraLocations.Height - 372
    lstSLC(1).Width = fraLocations.Width
    'If fw > 5200 Then fraAnal.Left = fw - 5115
      fraAnal.Left = sashV.Left + sashV.Width
      fraSLC(0).Left = fraAnal.Left
      fraTimser.Left = fraAnal.Left
      fraDumDates.Left = fraAnal.Left
      ctlGenDate.Left = fraAnal.Left
    'End If
    'If fw > 2600 Then fraSLC(2).Left = fw - 2595
    fraSLC(2).Left = fraSLC(0).Left + fraSLC(0).Width + 100
    If fhMinusMenus > fraAnal.Height + 50 Then fraAnal.Top = fhMinusMenus - fraAnal.Height
    If fraAnal.Top - fraDumDates.Height > 0 Then
      fraDumDates.Top = fraAnal.Top - fraDumDates.Height
      ctlGenDate.Top = fraDumDates.Top
    End If
    vSpaceLeft = fraDumDates.Top
    fraSLC(0).Height = vSpaceLeft * 0.555
    fraSLC(2).Height = fraSLC(0).Height
    fraTimser.Top = fraSLC(0).Height + 50
    If (vSpaceLeft - fraTimser.Top - 50) > 0 Then fraTimser.Height = vSpaceLeft - fraTimser.Top - 50
    If fraTimser.Height > 360 Then TimserGrid.Height = fraTimser.Height - 360
    If fraSLC(0).Height > 500 Then
      cmdSen(0).Top = fraSLC(0).Height - 492
      cmdSen(1).Top = cmdSen(0).Top
      cmdSen(2).Top = cmdSen(0).Top
    End If
    If fraSLC(2).Height > 1200 Then
      lstSLC(2).Height = fraSLC(2).Height - 1122
      lstSLC(4).Height = lstSLC(2).Height
    End If
    If fraSLC(0).Height > 1600 Then
      If cmdSen(0).Visible = True Then
        lstSLC(0).Height = fraSLC(0).Height - 1511
      Else
        lstSLC(0).Height = lstSLC(2).Height
      End If
      lstSLC(3).Height = lstSLC(0).Height
    End If
  End If
End Sub

Private Sub Form_Terminate()
  'MsgBox "gottoterminate"
End Sub

Private Sub Form_Unload(Cancel As Integer)
  Dim frm As Object
  'Static InUnload As Boolean 'Don't want to unload this form again while unloading other forms
  'If InUnload Then
  '  Cancel = 1
  'Else
  '  InUnload = True
    If Map1.UnsavedChanges Then
      Select Case MsgBox("Save changes to map?", vbYesNoCancel, "GenScn Exit")
        Case vbYes:    Map1.MapSave
        Case vbCancel: Cancel = 1
      End Select
    End If
    If p.EditFlg Or p.MapName <> Map1.MapFilePath & Map1.MapFileName Then
      Select Case MsgBox("Save changed status file?", vbYesNoCancel, "GenScn Exit")
        Case vbYes:    SaveStatusFileAs
        Case vbCancel: Cancel = 1
      End Select
    End If
    
    If Cancel = 0 Then
      On Error Resume Next
      UnhookKeyboard
      SaveRecentFiles mnuRecent, AppName
      If SaveWindowSettings(Me, AppName, SectionMainWin) Then
        SaveSetting AppName, SectionMainWin, "MapWidth", sashV.Left
      End If
      
      'If p.WDMData.Unit > 0 Then F90_WDFLCL p.WDMData.Unit
      If p.HSPFMsg.Unit > 0 Then F90_WDFLCL p.HSPFMsg.Unit
      Call F90_W99CLO
      TserFiles.Clear
      Set TserFiles = Nothing
      
      On Error Resume Next
      'MsgBox "Unloading " & p.ExternalForms.Count & " External forms", vbOKOnly, "GenScn Unload"
      For Each frm In p.ExternalForms
        'MsgBox frm.GraphForm.Name & " - " & frm.GraphForm.Caption
        Unload frm
        Set frm = Nothing
      Next
      'MsgBox "Unloading " & Forms.Count & " internal forms", vbOKOnly, "GenScn Unload"
      For Each frm In Forms
        If frm.Name <> Me.Name Then
          'MsgBox frm.Name & " - " & frm.Caption
          Unload frm
          Set frm = Nothing
        End If
      Next frm
      If RunningVB Then
        IPC.ExitProcess 0 'End status monitor
        End
      Else
        IPC.SendMonitorMessage "TERMINATE"
      End If
    End If
  '  InUnload = False
  'End If
  'MsgBox "end of unload #" & Forms.Count & " Cancel=" & Cancel
End Sub

Private Sub lstSLC_Click(index As Integer)
  DbgMsg "lstSLC:Click: " & index & ":" & sSLC(index), 4, "frmGenScn", "m"
  If lstSLC(index).Visible Then
    lstSLC_GotFocus (index)
  End If
End Sub

Private Sub lstSLC_DblClick(index As Integer)
  DbgMsg "lstSLC:DblClick " & index, 3, "frmGenScn", "m"
  If index = 0 Then
    If cmdSen(0).Visible Then
      If lstSLC(index).SelCount = 1 Then
        cmdSen_Click (0)
      End If
    End If
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
  If index = 2 Or index = 4 Then lstSLC(index).ToolTipText = GetDescription("CON", lstSLC(index).List(itemnum))
End Sub

Private Sub Map1_SelectionChange(FeatureID As String, layer As Long, state As Boolean)
  DbgMsg "Map1_SelectionChange:" & FeatureID & " on " & layer & " to " & state, 3, "frmGenScn", "t"
  scntLoc = Map1.SelCount - 1
  If scntLoc > -1 Then
    Map1.GetSelectedKeys specLoc
  Else
    ReDim specLoc(0)
  End If
  LocationSelChange
End Sub

Private Sub LocationSelChange()
  Dim sen As String, curSen&, maxSen&
  Dim con As String, curCon&, maxCon&
  Dim curLoc&, otherLoc&, maxLoc&
  Dim lTs As Collection
  Set lTs = New Collection
  
  If scntSen > 0 Then maxSen = scntSen - 1 Else maxSen = 0
  If scntCon > 0 Then maxCon = scntCon - 1 Else maxCon = 0
  sen = ""
  con = ""
  lstSLC(3).Clear
  lstSLC(4).Clear
  For curSen = 0 To maxSen
    If scntSen > 0 Then sen = specSen(curSen)
    For curCon = 0 To maxCon
      If scntCon > 0 Then con = specCon(curCon)
      For curLoc = 0 To scntLoc
        If Left(specLoc(curLoc), 1) = "<" Then 'special case - other points
          For otherLoc = 0 To p.LocnName.Count - 1
            If Not (p.LocnGeoCode(p.LocnName(curLoc))) Then 'not on map
              Call FindTimSer(sen, p.LocnName(otherLoc), con, lTs)
            End If
          Next
        Else 'defined point
          Call FindTimSer(sen, specLoc(curLoc), con, lTs)
        End If
        If lTs.Count > 0 Then
          Call UpdateSite(0, lTs, lstSLC(3))
          Call UpdateSite(1, lTs, lstSLC(4))
        End If
      Next
    Next
  Next
  Call frmGenScn.SyncLists(frmGenScn.lstSLC(0), lstSLC(3))
  Call frmGenScn.SyncLists(frmGenScn.lstSLC(2), lstSLC(4))
'    If lts.Count > 0 Then  'match something
'      Call Map1.SetSelectedByKey(layer, FeatureID, True)
      'location/scenario list
      'Call UpdateSite(0, lts, lstSLC(3))
      'location/constituent list
      'Call UpdateSite(1, lts, lstSLC(4))
'    Else
'      Call Map1.SetSelectedByKey(layer, FeatureID, False)
'    End If
'    Set lts = Nothing
'  Else 'always turn off
'    Call Map1.SetSelectedByKey(layer, FeatureID, False)
'  End If
End Sub

Private Sub SelectLocationsWithData()
  Dim sen As String, curSen&, maxSen&
  Dim con As String, curCon&, maxCon&
  Dim vLocnName As Variant
  Dim vTs As Variant
  Dim ts As ATCclsTserData

  Dim curLoc&, otherLoc&, maxLoc&
  'Dim lTs As Collection
  'Set lTs = New Collection
  
  Me.MousePointer = vbHourglass
  
  If scntSen > 0 Then maxSen = scntSen - 1 Else maxSen = 0
  If scntCon > 0 Then maxCon = scntCon - 1 Else maxCon = 0
  sen = ""
  con = ""
  lstSLC(3).Clear
  lstSLC(4).Clear

  For Each vLocnName In p.LocnName
    For Each vTs In TimserGrid.WholeList
      Set ts = vTs
      If ts.Header.loc = vLocnName Then
        If scntSen >= 0 Then
          For curSen = 0 To maxSen
            If specSen(curSen) = ts.Header.sen Then GoTo MatchedSen
          Next
          GoTo NoMatch
        End If
MatchedSen:
        If scntCon >= 0 Then
          For curCon = 0 To maxCon
            If specCon(curCon) = ts.Header.con Then GoTo MatchedCon
          Next
          GoTo NoMatch
        End If
MatchedCon:
        If fraLocations.Visible Then
          For otherLoc = 0 To lstSLC(1).ListCount
            If lstSLC(1).List(otherLoc) = vLocnName Then
              lstSLC(1).Selected(otherLoc) = True
              GoTo SelectedLoc
            End If
          Next
        Else
          Map1.SetSelectedByKey Map1.CurLayer, vLocnName, True
        End If
        GoTo SelectedLoc
      End If
NoMatch:
    Next
SelectedLoc:
  Next

'  For curLoc = 1 To p.LocnName.Count
'    For curSen = 0 To maxSen
'      If scntSen >= 0 Then sen = specSen(curSen)
'      For curCon = 0 To maxCon
'        If scntCon >= 0 Then con = specCon(curCon)
'        Call FindTimSer(sen, p.LocnName(curLoc), con, lTs)
'        If lTs.Count > 0 Then
'          If fraLocations.Visible Then
'            For otherLoc = 0 To lstSLC(1).ListCount
'              If lstSLC(1).List(otherLoc) = p.LocnName(curLoc) Then
'                lstSLC(1).Selected(otherLoc) = True
'                GoTo SelectedLoc
'              End If
'            Next
'          Else
'            Map1.SetSelectedByKey Map1.CurLayer, p.LocnName(curLoc), True
'          End If
'          GoTo SelectedLoc
'        End If
'      Next
'    Next
'SelectedLoc:
'  Next
  If fraLocations.Visible Then
    ReDim specLoc(0)
    ReDim specLoc(lstSLC(1).SelCount)
    curLoc = 0
    scntLoc = lstSLC(1).ListCount - 1
    For otherLoc = 0 To scntLoc
      If lstSLC(1).Selected(otherLoc) Then
        specLoc(curLoc) = lstSLC(1).List(otherLoc)
        curLoc = curLoc + 1
      End If
    Next
  Else
    Map1_SelectionChange "", Map1.CurLayer, False
  End If
  
  Me.MousePointer = vbDefault

End Sub

Private Sub mnuAnal_Click(index As Integer)
  DbgMsg "mnuAnal:Click:" & mnuAnal(index).Caption, 3, "frmGenScn", "m"
  Call DoAnal(index)
End Sub

Private Sub mnuAnalNew_Click()
  Unload frmRun
  With frmRun
    .lblArg(0).Caption = "Program"
    .txtArg(0).text = "path"
    .txtArg(0).Locked = True
    .cmdView(0).Visible = False
    .Caption = "Defining new external program for GenScn"
    .cmdRun.Caption = "OK"
    .cmdAddArg.Visible = True
    .Show
  End With
End Sub

Private Sub CheckForRequiredUnits(tscoll As Collection)
  Dim i&, k&
  Dim units As String, lUnits As String
  Dim otherTS As ATCclsTserData
  Dim SimilarTs As Long
  Dim UpdatedAny As Boolean
  Dim SaveSelected As Collection
  
  For i = 1 To tscoll.Count
    With TSerSelected(i)
      units = .Attrib("Units")
      If Len(units) = 0 Or LCase(units) = "unknown" Then 'use none for no units available
        frmSpecifyUnits.Caption = "Specify Units for " & .Header.con & " at " & .Header.loc & " in " & .Header.sen
        frmSpecifyUnits.Icon = Me.Icon
        units = frmSpecifyUnits.GetUnitsFromUser
        If Len(units) > 0 Then
          .AttribSet "Units", units
          .File.WriteDataHeader TSerSelected(i)
          UpdatedAny = True
          SimilarTs = 0
          For k = 1 To .File.DataCount
            Set otherTS = .File.Data(k)
            If otherTS.Header.con = .Header.con Then
              lUnits = otherTS.Attrib("Units")
              If Len(lUnits) = 0 Or LCase(lUnits) = "unknown" Then
                SimilarTs = SimilarTs + 1
              End If
            End If
          Next
          If SimilarTs > 0 Then
            If MsgBox("Assign these units to " & SimilarTs _
                    & " other data sets in the same file with the same constituent (" _
                    & .Header.con & ")", vbYesNo) = vbYes Then
              For k = 1 To .File.DataCount
                Set otherTS = .File.Data(k)
                If otherTS.Header.con = .Header.con Then
                  lUnits = otherTS.Attrib("Units")
                  If Len(lUnits) = 0 Or LCase(lUnits) = "unknown" Then
                    otherTS.AttribSet "Units", units
                    .File.WriteDataHeader otherTS
                  End If
                End If
              Next
            End If
          End If
        End If
      End If
    End With
  Next
  
  If UpdatedAny Then TimserGrid.PopulateGrid
  
End Sub
Private Sub DoAnal(index As Integer)
  Dim i&, animfg%, ok As Boolean
  Dim htab$, rtab$, Msg$
  Dim lTs As Collection
  Dim ConstInt As Boolean

  ctlGenDate_LostFocus
  ok = True
  If lstSLC(0).Enabled = False Then
    MsgBox "No project file is active.", vbExclamation, "Genscn Analysis Problem"
    ok = False
  ElseIf Tser.Count < 1 And index < 4 Then
    MsgBox "No time series available to perform selected Analysis option." & vbCr _
         & "Add some data to the list above.", vbExclamation, "Genscn Analysis Problem"
    ok = False
  ElseIf ctlGenDate.Visible = False And (index < 4 Or index = 7) Then
    MsgBox "No dates are available to perform selected Analysis option", vbExclamation, "Genscn Analysis Problem"
    ok = False
  End If
  If ok Then
    Select Case index
      Case 0, 1, 2, 7 'Graph, List, Duration, Animate
        If TSerSelected.Count = 0 Then
          If Tser.Count = 1 Then
            TimserGrid.SelectRow 1, True
          Else
            If MsgBox("At least one time series must be selected." & vbCr & "Use all time series in the list?", vbYesNo, "GenScn Analysis") = vbYes Then
              For i = 1 To TimserGrid.VisibleList.Count
                TimserGrid.SelectRow i, True
              Next
            End If
          End If
        End If
      Case 3 'Compare
        Select Case TSerSelected.Count
          Case 0, 1
            If Tser.Count > 1 Then
              TimserGrid.SelectRow 1, True
              TimserGrid.SelectRow 2, True
            Else
              MsgBox "Exactly two time series must be selected for this analysis.", vbOKOnly, "GenScn Analysis"
            End If
          Case 2: 'We hope for exactly two
          Case Is > 2:
            MsgBox "Only two time series can be selected for this analysis.", vbOKOnly, "GenScn Analysis"
        End Select
    End Select
    
    If p.UnitsRequired Then CheckForRequiredUnits TSerSelected
    
    Select Case index
      Case 0 'Graph
        If TSerSelected.Count > 0 Then
          MousePointer = vbHourglass
          Unload frmGenScnPlot                 ' make sure frmGenScnPlot.Form_Load is run when we show it
          frmGenScnPlot.Icon = cmdAna(0).Picture
          frmGenScnPlot.Show
          MousePointer = vbDefault
        End If
      Case 1 'List
        If TSerSelected.Count > 0 Then
          MousePointer = vbHourglass
          Set lTs = FillTimSerExt(TSerSelected, ConstInt)
          Set tsList.OpenFiles = TimserGrid.OpenFiles
          tsList.Showcoll lTs
          MousePointer = vbDefault
        End If
      Case 2 'Duration
        Unload frmGenScnDuration
        frmGenScnDuration.Show
      Case 3 'Compare
        If TSerSelected.Count = 2 Then
          Set lTs = FillTimSerExt(TSerSelected, ConstInt)
          Unload frmGenScnCompare
          frmGenScnCompare.Showcoll lTs
        End If
      Case 4 'Generate
        Call TimserGrid.ManageTimeSeriesList(13, "new time series")
      Case 5
        MsgBox "Frequency Analysis is Under Construction.", 64, "GenScn Frequency Analysis"
      Case 6 'Display File
        On Error GoTo 10
        CDFileList.flags = &H1804&
        CDFileList.Filename = " "
        CDFileList.CancelError = True
        CDFileList.DialogTitle = "GenScn File View"
        CDFileList.Action = 1
        Dim inputfile$
        inputfile = CDFileList.Filename
        Dim cap$
        cap = CDFileList.DialogTitle
        If InStr(UCase(inputfile), "UCI") Or InStr(UCase(inputfile), "STS") Then
          DispFile.OpenFile inputfile, cap, Me.Icon, True
        Else
          DispFile.OpenFile inputfile, cap, Me.Icon, False
        End If
10        'continue here on cancel
      Case 7  'Animate
        'Set lTs = TSerSelected
        Set Tser = FillTimSerExt(TSerSelected, ConstInt) 'Added so that selected dates are used instead of native dates
        Set lTs = Tser
        If lTs.Count > 0 Then
          'make sure timseries contain 1 scen/1 loc
          Msg = ""
          For i = 2 To lTs.Count
            If lTs(i).Header.sen <> lTs(1).Header.sen Then
              If InStr(Msg, lTs(i).Header.sen) = 0 Then
                Msg = Msg & "Scenarios: " & lTs(1).Header.sen & " and " & lTs(i).Header.sen & " do not match" & vbCr
              End If
            ElseIf lTs(i).Header.con <> lTs(1).Header.con Then
              If InStr(Msg, lTs(i).Header.con) = 0 Then
                Msg = Msg & "Constituents: " & lTs(1).Header.con & " and " & lTs(i).Header.con & " do not match" & vbCr
              End If
            End If
          Next i
          If Len(Msg) = 0 Then 'do animation
            MousePointer = vbHourglass
            frmGenScnAnimate.Show
            MousePointer = vbDefault
          Else
            MsgBox "Animation requires all time series being animated " & vbCrLf _
              & "to have the same Scenario and Constituent." & vbCrLf & Msg, _
              vbExclamation, "GenScn Animate Problem"
          End If
        End If
      Case 8 'Profile Plot
        If lstSLC(0).SelCount = 0 Then
          MsgBox "You must have at least one Scenario selected to generate a Profile Plot", vbExclamation, "GenScn Profile Plot Problem"
        ElseIf lstSLC(2).SelCount = 0 Then
          MsgBox "You must have at least one Constituent selected to generate a Profile Plot", vbExclamation, "GenScn Profile Plot Problem"
        Else
          MousePointer = vbHourglass
          frmGenProfPlot.Show
          MousePointer = vbDefault
        End If
      Case 9 'Estimator
        Dim really&
        really = MsgBox("Estimator is not currently a supported part of GenScn." & vbCr & "Continue anyway?", vbOKCancel, "Estimator Warning")
        If really = vbOK Then
          Unload frmGenScnEstimator
          frmGenScnEstimator.Show
        End If
      Case 10
        Unload frmHSPFSimulate
        frmHSPFSimulate.Show
      'Case 11 Then 'SWMM
      '  frmSWMMInterface.Show
      Case Is > 10: 'external EXE
        'the menu item's tag was set to refer to the corresponding EXE in OpenStatusFile
        If InStr(UCase(mnuAnal(index).Caption), "SWMM") > 0 Then
          frmSWMMInterface.Show
        ElseIf InStr(UCase(mnuAnal(index).Caption), "STRMDEPL") > 0 Then
          If lstSLC(0).SelCount = 1 Then
            Call frmStrmDeplInterface.LoadStrmDeplData(lstSLC(0).List(lstSLC(0).ListIndex))
          Else
            MsgBox "Stream Depletion requires one and only one Scenario to be Selected." & vbCrLf _
            & "You have selected " & lstSLC(0).SelCount & ".", _
            vbExclamation, "GenScn Stream Depletion"
          End If
        ElseIf InStr(UCase(mnuAnal(index).Caption), "SCRIPT") > 0 Then
          RunScript
        Else
          frmRun.ConfigureForm mnuAnal(index).Tag
          frmRun.Show
        End If
    End Select
  End If

End Sub

'Private Sub mnuAnalPlugIn_Click(Index As Integer)
'  Dim curactive As ATCclsAnalysis, ConstInt As Boolean, e As String, i As Long
'
'  If TSerSelected.Count = 0 Then
'    If Tser.Count = 1 Then
'      TimserGrid.SelectRow 1, True
'    Else
'      If MsgBox("At least one time series must be selected." & vbCr & "Use all time series in the list?", vbYesNo, "GenScn Analysis") = vbYes Then
'        For i = 1 To TimserGrid.VisibleList.Count
'          TimserGrid.SelectRow i, True
'        Next
'      End If
'    End If
'  End If
'
'  If TSerSelected.Count > 0 Then
'
'    Call Analyses.Create((Index))
'    Set curactive = Analyses.CurrentActive.obj
'    Set curactive.Monitor = IPC
'    'curactive.HelpFileName = App.HelpFile
'
'    '!!!!
'    'there's probably a cleaner way to do this, but need to get date information
'    ctlGenDate_LostFocus
'    '!!!!
'    Set curactive.DataCollection = FillTimSerExt(TSerSelected, ConstInt)
'    If curactive.EditSpecification Then
'      curactive.Go
'    End If
'
'    e = curactive.ErrorDescription
'    If Len(e) > 0 Then
'      MsgBox e, vbOKOnly, Me.Caption
'    End If
'
'  End If
'
'End Sub

Private Sub mnuClose_Click()
  Dim i%, j&, lp As GenScnProject
  
  DbgMsg "mnuClose:Click", 3, "frmGenScn", "m"
  i = vbNo
  If p.EditFlg Then
    i = MsgBox("Do you want to save your edited status file?", vbYesNoCancel, "GenScn Close")
    If i = vbYes Then
      If Len(p.StatusFileName) > 0 Then
        Call SaveStatusFile
      Else
        Call SaveStatusFileAs
      End If
    End If
  End If
  If i <> vbCancel Then
    If Map1.UnsavedChanges Then
      i = MsgBox("Save changes to map?", vbYesNoCancel, "GenScn Close")
      If i = vbYes Then Map1.MapSave
    End If
    If i <> vbCancel Then
      For j = TserFiles.Active.Count To 1 Step -1
        TserFiles.Delete (j)
      Next j
      'close files if already open
      'If p.WDMData.Unit > 0 Then
        'wdm file already open
      '  i = F90_WDFLCL(p.WDMData.Unit)
      '  If i <> -87 Then ' cant close last time
      '    p.WDMData.Unit = 0
      '    p.WDMData.Name = ""
      '  End If
      'End If
      If p.HSPFMsg.Unit > 0 Then
        'message file already open
        i = F90_WDFLCL(p.HSPFMsg.Unit)
        If i <> -87 Then ' cant close last time
          p.HSPFMsg.Unit = 0
          p.HSPFMsg.Name = ""
        End If
      End If
      CntDsn = 0
      Set Tser = Nothing
      Set Tser = New Collection
      Set TimserGrid.WholeList = Tser
      Set TimserGrid.AvailAttributes = New Collection
      Set TimserGrid.VisibleAttributes = New Collection

      TimserGrid.ClearRules
      TimserGrid.PopulateGrid
      TimserGrid.Enabled = False 'grdDsn.Rows = 0 'LstDsn.ListItems.Clear
      Set p.WDMFiles = Nothing
      Set p.WDMFiles = New Collection
      'Call lstDsn_GotFocus
      Call GetCommonDates
      
      On Error Resume Next
      
  '    map1.MapAction(tbrMap1.Buttons, tbrMap1.Buttons("FullExtent").Tag, Map1)  'full extent
  '    agdMapLocationDetails.Clear
  '    agdMapLocationDetails.Rows = 1
      
      For i = 0 To 5
        cmdSLC(i).Enabled = False
      Next i
      cmdSen(0).Enabled = False
      cmdSen(1).Enabled = False
      cmdSen(2).Enabled = False
      For i = 0 To cmdAna.Count - 1
        cmdAna(i).Enabled = False
      Next i
      For i = 0 To lstSLC.Count
        lstSLC(i).Clear
        lstSLC(i).Enabled = False
        lstSLC(i).BackColor = vbButtonFace
      Next i
      fraAnal.Enabled = False
      fraDumDates.Enabled = False
      fraSLC(0).Enabled = False
      fraSLC(1).Enabled = False
      fraSLC(2).Enabled = False
      fraTimser.Enabled = False
      lblDumDates.Enabled = False
      For i = 0 To lblSLC.Count
        lblSLC(i).Caption = "" '"0 of 0"
        lblSLC(i).Enabled = False
      Next i
      mnuClose.Enabled = False
      mnuSave(0).Enabled = False
      mnuSave(1).Enabled = False
      mnuSave(2).Enabled = False
      mnuEdit.Enabled = False
      'mnuOpen.Enabled = True
      'mnuNew.Enabled = True
      UpdateLblDsn
      p.StatusFileName = ""
      p.MapName = ""
      For i = 0 To optSLC.Count - 1
        optSLC(i).Enabled = False
      Next i
      
      Map1.Clear
      Map1.UnsavedChanges = False
      Map1.Enabled = False
      For i = 1 To mnuX.Count - 1
        mnuX(i).Visible = False
      Next i
      On Error GoTo 0
      
      Call InitATCoTSer 'init timeseries classes to none
      
      frmGenScn.Caption = AppName
    End If
  End If
End Sub

Private Sub mnuCon_Click(index As Integer)
    Dim i%
    
    DbgMsg "mnuCon:Click:" & mnuCon(index).Caption, 3, "frmGenScn", "m"
    If index = 3 Then 'properties
      Unload frmGenProp
      Load frmGenProp
      With frmGenProp
        If Left(.Caption, 1) = "P" Then .Caption = "Constituent " & .Caption
        .Icon = Me.Icon
        .atcGrid.Clear
        .atcGrid.ColTitle(0) = "Name"
        .atcGrid.ColType(0) = ATCoCtl.ATCoTxt
        .atcGrid.colWidth(0) = 1200
        .atcGrid.ColTitle(1) = "Desciption"
        .atcGrid.ColType(1) = ATCoCtl.ATCoTxt
        .atcGrid.ColEditable(1) = True
        .atcGrid.colWidth(1) = .atcGrid.Width - .atcGrid.colWidth(0) - 200
        For i = 1 To p.ConsName.Count
          .atcGrid.TextMatrix((i), 0) = p.ConsName(i)
          .atcGrid.TextMatrix((i), 1) = p.ConsDesc(p.ConsName(i))
        Next i
      End With
      frmGenProp.Show
    ElseIf index = 2 Then 'set focus
      If lstSLC(2).Visible Then
        lstSLC(2).SetFocus
      ElseIf lstSLC(4).Visible Then
        lstSLC(4).SetFocus
      End If
    Else
      If index = 0 Then
        i = 2
      Else
        i = 5
      End If
      Call cmdSLC_Click(i)
    End If

End Sub

Private Sub mnuDates_Click(index As Integer)
  With ctlGenDate
    Select Case index
      Case 0: .refreshDateAll     'Reset
      Case 1: .SetControlFocus 0  'Start Year
      Case 2: .SetControlFocus 6  'End Year
      Case 3: .SetControlFocus -3 'Time Step
      Case 4: .SetControlFocus -4 'Units
      Case 5: .SetControlFocus -5 'Aggregation
    End Select
  End With
End Sub

Private Sub mnuEdit_Click()

    DbgMsg "mnuEdit:Click", 3, "frmGenScn", "m"
    Call ManageProject(2)
    
End Sub

Private Sub mnuExit_Click()
    
  DbgMsg "mnuExit:Click", 3, "frmGenScn", "m"
  Unload Me

End Sub

Private Sub mnuGenerOld_Click()
  Unload frmGenScnGener
  Call frmGenScnGener.GenerInit
  frmGenScnGener.Show
End Sub

Private Sub mnuHelpContents_Click(index As Integer)
  'Dim retval As Long
  'Dim command As Long
  'retval = 0
  'command = 0
  DbgMsg "mnuHelpContents:Click:" & CStr(index), 3, "frmGenScn", "m"
  If index = 0 Then
    frmGenScnAbout.Show
  ElseIf index = 1 Then
    'HtmlHelp Me.hwnd, App.HelpFile, command, ByVal retval
    cdl.Filename = App.HelpFile
    OpenFile App.HelpFile, cdl
    If App.HelpFile <> cdl.Filename Then App.HelpFile = cdl.Filename
  ElseIf index = 2 Then
    'HtmlHelp Me.hwnd, ExePath & "doc\HSPF.chm", command, ByVal retval
    OpenFile PathNameOnly(App.HelpFile) & "\HSPF.chm", cdl
  ElseIf index = 3 Then
    IPC.SendMonitorMessage "(Open GenScn Status Monitor)"
    IPC.SendMonitorMessage "(TEXT ON)"
  Else
    'Call HelpMissing
  End If
End Sub

Private Sub mnuGraphNative_Click()
  Dim lTs As Collection, ConstInt As Boolean
  Dim tsg As New ATCoTSgraph
  Dim i As Long
  If TSerSelected.Count = 0 Then
    If Tser.Count = 1 Then
      TimserGrid.SelectRow 1, True
    Else
      If MsgBox("At least one time series must be selected." & vbCr & "Use all time series in the list?", vbYesNo, "GenScn Analysis") = vbYes Then
        For i = 1 To TimserGrid.VisibleList.Count
          TimserGrid.SelectRow i, True
        Next
      End If
    End If
  End If
  ctlGenDate_LostFocus
  Set lTs = FillTimSerExt(TSerSelected, ConstInt)
  tsg.Show lTs
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
  feedback.AddText StatusString(False)
  feedback.AddText WholeFileString(LogFileName)
  stepname = "5: feedback.Show"
  feedback.Show App, Me.Icon
  
  Exit Sub
  
errmsg:
  MsgBox "Error opening feedback in step " & stepname & vbCr & err.Description
End Sub

Private Sub mnuHelpScript_Click()
  ShowScriptDocumentation
End Sub

Private Sub mnuListOld_Click()
  Dim ok As Boolean, i As Long
  ctlGenDate_LostFocus
  ok = True
  If lstSLC(0).Enabled = False Then
    MsgBox "No project file is active.", vbExclamation, "Genscn Analysis Problem"
    ok = False
  ElseIf Tser.Count < 1 Then
    MsgBox "No time series available to perform selected Analysis option." & vbCr _
         & "Add some data to the list above.", vbExclamation, "Genscn Analysis Problem"
    ok = False
  ElseIf ctlGenDate.Visible = False Then
    MsgBox "No dates are available to perform selected Analysis option", vbExclamation, "Genscn Analysis Problem"
    ok = False
  End If
  If ok Then
    If TSerSelected.Count = 0 Then
      If Tser.Count = 1 Then
        TimserGrid.SelectRow 1, True
      Else
        If MsgBox("At least one time series must be selected." & vbCr & "Use all time series in the list?", vbYesNo, "GenScn Analysis") = vbYes Then
          For i = 1 To TimserGrid.VisibleList.Count
            TimserGrid.SelectRow i, True
          Next
        End If
      End If
    End If
    
    Me.MousePointer = vbHourglass
    Call GenList
    Me.MousePointer = vbDefault
  End If
'  Dim lts As Collection, tsv As Variant, Ts As ATCclsTserData
'  Dim tsl As New ATCoTSlist, ConstInt As Boolean
'
'  ctlGenDate_LostFocus
'  Set lts = FillTimSerExt(TSer, ConstInt)
'
'  For Each tsv In lts
'    Set Ts = tsv
'    tsl.Show Ts
'  Next
End Sub

Private Sub mnuPlayKeys_Click()
  cmdKey.DialogTitle = "Open Keystroke File"
  cmdKey.flags = &H1804& 'not read only
  cmdKey.filter = "Keystroke Files (*.atk)|*.atk|All Files (*.*)|*.*"
  
  On Error GoTo NeverMind
  cmdKey.CancelError = True
  cmdKey.ShowOpen
    
  If Len(cmdKey.Filename) > 0 Then
    'launch.StartMacroPlay ExePath & "bin\winkeydriver.exe " & cmdKey.Filename
    IPC.StartProcess "PlayMacro", ExePath & "bin\winkeydriver.exe " & cmdKey.Filename
  End If
NeverMind:
End Sub

Private Sub mnuPreferredUnits_Click()
  frmPreferredUnits.Show
  frmPreferredUnits.Icon = Me.Icon
  frmPreferredUnits.SetFromProject
End Sub

Private Sub mnuRecent_Click(index As Integer)
  Dim newFilePath$, tmpFilePath$
  newFilePath = mnuRecent(index).Tag
  If index > 0 Then
    If UCase(p.StatusFileName) = UCase(newFilePath) Then
      If p.EditFlg Then
        If MsgBox("Discard changes and reload this project?", vbOKCancel, "Load Project") = vbCancel Then Exit Sub
        p.EditFlg = False
      End If
    End If
    If Len(Dir(newFilePath)) > 0 Then
      OpenStatusFile newFilePath
    Else 'status file not currently available, remove from menu
      While index < mnuRecent.Count - 1
        tmpFilePath = mnuRecent.item(index + 1).Tag
        mnuRecent.item(index).Caption = "&" & index & " " & FilenameOnly(tmpFilePath)
        mnuRecent.item(index).Tag = tmpFilePath
        index = index + 1
      Wend
      Unload mnuRecent.item(index)
      If MsgBox("Project " & newFilePath & " not found. Look for it?", vbOKCancel, "GenScn Recent Error") = vbOK Then
        mnuOpen_Click
      End If
    End If
  End If
End Sub

Private Sub mnuRecordKeys_Click()
  Dim S$, i&, t$
  
  If mnuRecordKeys.Caption = "&Record Keys" Then
    HookKeyboard
    DbgMsg "StartRecordingKeys:", 7, "frmGenScn", "i"
    mnuRecordKeys.Caption = "Stop &Recording"
  Else
    UnhookKeyboard
    mnuRecordKeys.Caption = "&Record Keys"
    S = GetRecordedKeystrokes
    If Len(S) > 0 Then
      cmdKey.DialogTitle = "Save Keystroke File"
      cmdKey.flags = &H1804& 'not read only
      cmdKey.filter = "Keystroke Files (*.atk)|*.atk|All Files (*.*)|*.*"
      On Error GoTo NeverMind
      cmdKey.CancelError = True
      cmdKey.ShowSave
      
      If Len(cmdKey.Filename) > 0 Then

        i = FreeFile(0)
        Open cmdKey.Filename For Output As i
        Print #i, S
        Close #i
      End If
    End If
  End If
NeverMind:
End Sub

Private Sub mnuLoc_Click(index As Integer)
  Dim i%
  DbgMsg "mnuLoc:Click:" & mnuLoc(index).Caption, 3, "frmGenScn", "m"
  
  If index = 3 Then
    Unload frmGenProp
    Load frmGenProp
    With frmGenProp
      If Left(.Caption, 1) = "P" Then .Caption = "Location " & .Caption
      .Icon = Me.Icon
      .atcGrid.Clear
      .atcGrid.ColTitle(0) = "Name"
      .atcGrid.ColType(0) = ATCoCtl.ATCoTxt
      .atcGrid.colWidth(0) = 1200
      .atcGrid.ColTitle(1) = "Desciption"
      .atcGrid.ColType(1) = ATCoCtl.ATCoTxt
      .atcGrid.ColEditable(1) = True
      .atcGrid.colWidth(1) = .atcGrid.Width - .atcGrid.colWidth(0) - 200
      For i = 1 To p.LocnName.Count
        .atcGrid.TextMatrix((i), 0) = p.LocnName(i)
        .atcGrid.TextMatrix((i), 1) = p.LocnDesc(p.LocnName(i))
      Next i
    End With
    frmGenProp.Show
  ElseIf index = 4 Then
    SelectLocationsWithData
  ElseIf fraLocations.Visible Then
    Select Case index
      Case 0: cmdSLC_Click 1     'Select All
      Case 1: cmdSLC_Click 4     'Select None
      Case 2: lstSLC(1).SetFocus 'set focus to location list
      Case 500: ChangeToMap
    End Select
  Else
    Select Case index
      Case 0: Map1.SelectAll Map1.CurLayer, True  'Select All
      Case 1: Map1.SelectAll Map1.CurLayer, False 'Select None
      Case 2: Map1.SetControlFocus 1      'set focus to point details
      Case 500: ChangeToList
    End Select
  End If
End Sub

Private Sub mnuMap_Click(index As Integer)
  DbgMsg "mnuMap:Click:" & mnuMap(index).Caption, 3, "frmGenScn", "m"
  Select Case index
    Case 400, 410 'Branch, Downstream checkboxes
      mnuMap(index).Checked = Not mnuMap(index).Checked
      If index = 400 Then
          mnuMap(410).Checked = False
      Else
          mnuMap(400).Checked = False
      End If
      Map1.MapAction index
    Case 500
      If fraLocations.Visible Then ChangeToMap Else ChangeToList
    Case Else
      Map1.MapAction index
  End Select
End Sub

Private Sub ChangeToList()
  Select Case scntLoc
    Case -1:                      cmdSLC_Click 4 'Select None
    Case lstSLC(1).ListCount - 1: cmdSLC_Click 1 'Select All
    Case Else                                    'Select Some
      Dim i&, j&, listLoc As String
      For i = 0 To lstSLC(1).ListCount - 1
        listLoc = lstSLC(1).List(i)
        For j = 0 To scntLoc
          If specLoc(j) = listLoc Then lstSLC(1).Selected(i) = True: GoTo FoundIt
        Next
        lstSLC(1).Selected(i) = False
FoundIt:
      Next
  End Select
  fraLocations.Visible = True 'Switch from map to list
  Map1.Visible = False
  mnuLoc(500).Caption = "&Change to Map"
  mnuMap(500).Caption = "&Change to Map"
End Sub
Private Sub ChangeToMap()
'  We would like to make the selections from the list show up on the map
'  It would go something like the following, but with which layer?
'  Dim layer as long
'  layer = ?
'  Select Case scntLoc
'    Case -1:                      Map1.SelectAll layer, False
'    Case lstSLC(1).ListCount - 1: Map1.SelectAll layer, True
'    Case Else                                    'Select Some
'      Map1.SelectAll layer, False
'      For j = 0 To scntLoc
'        Call Map1.SetSelectedByKey(layer, specLoc(j), True)
'      Next
'  End Select
  fraLocations.Visible = False 'Switch from list to map
  Map1.Visible = True
  mnuLoc(500).Caption = "&Change to List"
  mnuMap(500).Caption = "&Change to List"
End Sub

Private Sub mnuNew_Click()
  DbgMsg "mnuNew:Click", 3, "frmGenScn", "m"
  Call ManageProject(0)
  If Map1.LayerCount < 1 Then ChangeToList
End Sub

Private Sub mnuOpen_Click()
  DbgMsg "mnuOpen:Click", 3, "frmGenScn", "m"
  DbgMsg "%FO", 1, "frmGenScn", "k"
  Call ManageProject(1)
End Sub

Private Sub mnuSave_Click(index As Integer)
    Dim i&, S$, f$, g$, e$
    
    DbgMsg "mnuSave:Click:" & mnuSave(index).Caption, 3, "frmGenScn", "m"
    If lstSLC(0).Enabled = False Then
      MsgBox "No project is active to save.", _
        vbExclamation, "GenScn Save Project File"
    Else
      'have active project
      If index = 1 Or p.StatusFileName = "" Then
        'do save as
        Call SaveStatusFileAs
      ElseIf index = 0 Then
        'just save
        Call SaveStatusFile
      End If
      If index = 2 Then
        'save shortcut
        Dim gsSTARTMENUKEY$, gsPROGMENUKEY$
        'gsSTARTMENUKEY = "$(Start Menu)"
        'gsPROGMENUKEY = "$(Programs)"
        gsPROGMENUKEY = vbNullChar
        Dim fPrivate As Boolean
        fPrivate = True
        g = "..\..\desktop" & vbNullChar
        S = "GenScn " & FilenameOnly(p.StatusFileName) & vbNullChar
        e = ExeName & vbNullChar
        f = p.StatusFileName & vbNullChar
        CreateLink GetSpecialFolder(CSIDL_DESKTOPDIRECTORY), S, ExePath, FilenameOnly(ExeName) & " " & p.StatusFileName
        'i = fCreateShellLink(g, s, e, f, fPrivate, gsPROGMENUKEY)
        'i = Shell("rundll32.exe AppWiz.Cpl,NewLinkHere " & ExePath, 1)
        'i = Shell("rundll32.exe AppWiz.Cpl, NewLinkHere " & ExeName, vbNormalFocus)
      End If
    End If
End Sub

Private Sub mnuScen_Click(index As Integer)
    Dim i&, n$
    
    DbgMsg "mnuScen:Click:" & mnuScen(index).Caption, 3, "frmGenScn", "m"
    If index = 6 Then 'properties
      Unload frmGenProp
      Load frmGenProp
      With frmGenProp
        If Left(.Caption, 1) = "P" Then .Caption = "Scenario " & .Caption
        .Icon = Me.Icon
        .atcGrid.Clear
        .atcGrid.ColTitle(0) = "Name"
        .atcGrid.ColType(0) = ATCoCtl.ATCoTxt
        .atcGrid.colWidth(0) = 1200
        .atcGrid.ColTitle(1) = "Desciption"
        .atcGrid.ColType(1) = ATCoCtl.ATCoTxt
        .atcGrid.ColEditable(1) = True
        .atcGrid.colWidth(1) = 200
        .atcGrid.ColTitle(2) = "Type"
        .atcGrid.ColType(2) = ATCoCtl.ATCoTxt
        .atcGrid.ColEditable(2) = True
        .atcGrid.colWidth(2) = 200
        .atcGrid.ColTitle(3) = "Path"
        .atcGrid.ColType(3) = ATCoCtl.ATCoTxt
        .atcGrid.colWidth(3) = 400
        .atcGrid.ColEditable(3) = True
        For i = 1 To p.ScenName.Count
          n = p.ScenName(i)
          .atcGrid.TextMatrix(i, 0) = n
          .atcGrid.TextMatrix(i, 1) = p.ScenDesc(n)
          .atcGrid.TextMatrix(i, 2) = p.ScenType(n)
          .atcGrid.TextMatrix(i, 3) = p.ScenFile(n)
        Next i
        .atcGrid.ColsSizeByContents
      End With
      frmGenProp.Show
    ElseIf index = 2 Then 'set focus
      If lstSLC(0).Visible Then
        lstSLC(0).SetFocus
      ElseIf lstSLC(3).Visible Then
        lstSLC(3).SetFocus
      End If
    ElseIf index < 2 Then
      If index = 0 Then
        i = 0
      Else
        i = 3
      End If
      Call cmdSLC_Click((i))
    Else
      i = index - 3
      If cmdSen(i).Visible Then cmdSen_Click (i)
    End If
End Sub

Private Sub mnuTime_Click(index As Integer)
  DbgMsg "mnuTime:Click:" & mnuTime(index).Caption, 3, "frmGenScn", "m"
  If index = 14 Then
    If TimserGrid.Visible And TimserGrid.Enabled Then TimserGrid.SetFocus
  Else
    Call TimserGrid.ManageTimeSeriesList(index, mnuTime(index).Caption)
  End If
End Sub

Private Sub optSLC_Click(index As Integer)
  DbgMsg "optSLC:Click: " & index, 3, "frmGenScn", "m"
  optSLC(index).Value = True 'need this now, not later
  If index = 0 Then ' all scen
    lstSLC(3).Visible = False
    lstSLC(0).Visible = True
    Call SyncLists(lstSLC(3), lstSLC(0))
    Call UpdateCount(lstSLC(0), lblSLC(0), 0)
  ElseIf index = 1 Then ' site scen
    lstSLC(3).Visible = True
    lstSLC(0).Visible = False
    Call SyncLists(lstSLC(0), lstSLC(3))
    Call UpdateCount(lstSLC(3), lblSLC(0), 0)
  ElseIf index = 2 Then 'all con
    lstSLC(4).Visible = False
    lstSLC(2).Visible = True
    Call SyncLists(lstSLC(4), lstSLC(2))
    Call UpdateCount(lstSLC(2), lblSLC(2), 2)
  ElseIf index = 3 Then 'site con
    lstSLC(4).Visible = True
    lstSLC(2).Visible = False
    Call SyncLists(lstSLC(2), lstSLC(4))
    Call UpdateCount(lstSLC(4), lblSLC(2), 2)
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

'  If TSer.Count > 10 Then  'not doing more than 10 listings
'    lnts = 10
'  Else
'    lnts = TSer.Count
'  End If
  'get the data values for selected data sets
  ConstInt = True
  Set lTs = FillTimSerExt(TSerSelected, ConstInt)
  lnts = lTs.Count
  'labels
  ReDim CScenm(lnts - 1)
  ReDim clocnm(lnts - 1)
  ReDim cconnm(lnts - 1)
  ReDim which(lnts - 1)
  ReDim typind(lnts - 1)
  ReDim clab(lnts - 1)
  For i = 1 To lnts
    CScenm(i - 1) = lTs(i).Header.sen
    clocnm(i - 1) = lTs(i).Header.loc
    cconnm(i - 1) = lTs(i).Header.con
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
  
  capt = "Genscn List"
  If ConstInt Then 'constant interval
    Call GLInit(2, l, lnts, lnts)
  Else
    Call GLInit(2, l, lnts, 2 * lnts)
  End If
  Call GLTitl(Ctitl, capt)
  Call GLVarLab(clab())
  Call GLAxLab("", Cyllab, Cyrlab, Calab)
  Call GLDate(CSDat, CEDat, ConstInt)
  Call GLDoTS(l, 2, lTs)
End Sub

Private Sub sashV_MouseDown(Button As Integer, Shift As Integer, x As Single, y As Single)
  SashVdragging = True
End Sub

Private Sub sashV_MouseMove(Button As Integer, Shift As Integer, x As Single, y As Single)
  If SashVdragging And (sashV.Left + x) > 10 And (sashV.Left + x < Width - 10) Then
    Dim newRightWidth&
    sashV.Left = sashV.Left + x
    newRightWidth = Width - (sashV.Left + sashV.Width + 150)
    If newRightWidth > 1000 Then
      fraTimser.Width = newRightWidth
      ctlGenDate.Width = newRightWidth
      fraDumDates.Width = newRightWidth
      fraAnal.Width = newRightWidth
      fraSLC(0).Width = newRightWidth / 2 - 50
      fraSLC(2).Width = newRightWidth / 2 - 50
      If fraTimser.Width > 300 Then TimserGrid.Width = newRightWidth - 300 'LstDsn.Width = newRightWidth - 240
      If fraSLC(0).Width > 250 Then
        lstSLC(0).Width = fraSLC(0).Width - 240
        lstSLC(3).Width = lstSLC(0).Width
      End If
      If fraSLC(2).Width > 250 Then
        lstSLC(2).Width = fraSLC(2).Width - 240
        lstSLC(4).Width = lstSLC(2).Width
      End If
    End If
    Form_Resize
  End If
End Sub

Private Sub sashV_MouseUp(Button As Integer, Shift As Integer, x As Single, y As Single)
  SashVdragging = False
End Sub

Private Sub TimserGrid_add()
  TimeseriesAdd
  TimserGrid_Change
End Sub

Private Sub TimserGrid_Change()
  Set Tser = Nothing
  Set Tser = TimserGrid.VisibleList
  RefreshTSList
  TimserGrid.PopulateGrid
End Sub

Private Sub TimserGrid_Edit()
  RaiseEdit
End Sub

Private Sub tsList_CreatedTser(newTS As ATCData.ATCclsTserData)
  RaiseEdit
End Sub

Private Sub tsList_Edit()
  RaiseEdit
End Sub

Public Sub RaiseEdit()
  RefreshSLC
  'RefreshMain
  TimserGrid_Change
End Sub

Private Sub AllowScenarioModify(ScenModFlag As Boolean)
  cmdSen(0).Visible = ScenModFlag
  cmdSen(1).Visible = ScenModFlag
  cmdSen(2).Visible = ScenModFlag
  Form_Resize
End Sub

Public Property Get TSerSelected() As Collection
  Set TSerSelected = TimserGrid.SelectedList
End Property

