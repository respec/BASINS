VERSION 5.00
Begin VB.Form frmGEdit 
   BorderStyle     =   1  'Fixed Single
   Caption         =   "Graph Edit"
   ClientHeight    =   4455
   ClientLeft      =   2025
   ClientTop       =   1770
   ClientWidth     =   7065
   HelpContextID   =   602
   Icon            =   "frmGEdit.frx":0000
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   PaletteMode     =   1  'UseZOrder
   ScaleHeight     =   4455
   ScaleWidth      =   7065
   WhatsThisButton =   -1  'True
   WhatsThisHelp   =   -1  'True
   Begin VB.CommandButton cmdClose 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Appl&y"
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
      Height          =   372
      Index           =   2
      Left            =   3600
      TabIndex        =   63
      Top             =   3960
      Width           =   972
   End
   Begin VB.CommandButton cmdClose 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
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
      Height          =   372
      Index           =   1
      Left            =   6000
      TabIndex        =   65
      Top             =   3960
      Width           =   972
   End
   Begin VB.CommandButton cmdClose 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "&OK"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   372
      Index           =   0
      Left            =   4800
      TabIndex        =   64
      Top             =   3960
      Width           =   972
   End
   Begin TabDlg.SSTab sstGEdit 
      Height          =   3735
      Left            =   120
      TabIndex        =   0
      Top             =   120
      Width           =   6855
      _ExtentX        =   12091
      _ExtentY        =   6588
      _Version        =   393216
      Tabs            =   5
      Tab             =   4
      TabsPerRow      =   5
      TabHeight       =   423
      BeginProperty Font {0BE35203-8F91-11CE-9DE3-00AA004BB851} 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      TabCaption(0)   =   "&Axes"
      TabPicture(0)   =   "frmGEdit.frx":0442
      Tab(0).ControlEnabled=   0   'False
      Tab(0).Control(0)=   "chkGrid(4)"
      Tab(0).Control(1)=   "fraXAxisTime"
      Tab(0).Control(2)=   "fratab(0)"
      Tab(0).ControlCount=   3
      TabCaption(1)   =   "&Titles"
      TabPicture(1)   =   "frmGEdit.frx":045E
      Tab(1).ControlEnabled=   0   'False
      Tab(1).Control(0)=   "fratab(1)"
      Tab(1).ControlCount=   1
      TabCaption(2)   =   "&Curves"
      TabPicture(2)   =   "frmGEdit.frx":047A
      Tab(2).ControlEnabled=   0   'False
      Tab(2).Control(0)=   "fratab(2)"
      Tab(2).ControlCount=   1
      TabCaption(3)   =   "&Lines"
      TabPicture(3)   =   "frmGEdit.frx":0496
      Tab(3).ControlEnabled=   0   'False
      Tab(3).Control(0)=   "fratab(3)"
      Tab(3).ControlCount=   1
      TabCaption(4)   =   "&General"
      TabPicture(4)   =   "frmGEdit.frx":04B2
      Tab(4).ControlEnabled=   -1  'True
      Tab(4).Control(0)=   "fratab(4)"
      Tab(4).Control(0).Enabled=   0   'False
      Tab(4).ControlCount=   1
      Begin VB.CheckBox chkGrid 
         Caption         =   "G&rid"
         Height          =   255
         Index           =   4
         Left            =   -74760
         TabIndex        =   11
         Top             =   1180
         Width           =   615
      End
      Begin VB.Frame fraXAxisTime 
         Caption         =   "Time X-Axis"
         Height          =   3255
         Left            =   -74880
         TabIndex        =   66
         Top             =   360
         Width           =   1575
         Begin ATCoCtl.ATCoText txtIntvl 
            Height          =   252
            Left            =   360
            TabIndex        =   16
            Top             =   2640
            Width           =   852
            _ExtentX        =   1508
            _ExtentY        =   450
            InsideLimitsBackground=   16777215
            OutsideHardLimitBackground=   8421631
            OutsideSoftLimitBackground=   8454143
            HardMax         =   -999
            HardMin         =   -999
            SoftMax         =   -999
            SoftMin         =   -999
            MaxWidth        =   -999
            Alignment       =   1
            DataType        =   0
            DefaultValue    =   0
            Value           =   ""
            Enabled         =   -1  'True
         End
         Begin VB.OptionButton rdoTMAx 
            Caption         =   "Year"
            Height          =   255
            Index           =   0
            Left            =   120
            TabIndex        =   12
            Top             =   1200
            Width           =   975
         End
         Begin VB.OptionButton rdoTMAx 
            Caption         =   "&Month"
            Height          =   255
            Index           =   1
            Left            =   120
            TabIndex        =   13
            Top             =   1560
            Width           =   975
         End
         Begin VB.OptionButton rdoTMAx 
            Caption         =   "&Day"
            Height          =   255
            Index           =   2
            Left            =   120
            TabIndex        =   14
            Top             =   1920
            Width           =   1095
         End
         Begin VB.OptionButton rdoTMAx 
            Caption         =   "Hour/Minute"
            Height          =   255
            Index           =   3
            Left            =   120
            TabIndex        =   15
            Top             =   2280
            Width           =   1215
         End
      End
      Begin VB.Frame fratab 
         BorderStyle     =   0  'None
         Caption         =   "Frame1"
         Height          =   2655
         Index           =   4
         Left            =   240
         TabIndex        =   99
         Top             =   480
         Width           =   6495
         Begin VB.ComboBox ComboDataLabels 
            Height          =   315
            ItemData        =   "frmGEdit.frx":04CE
            Left            =   1080
            List            =   "frmGEdit.frx":04D0
            Style           =   2  'Dropdown List
            TabIndex        =   117
            Top             =   2040
            Width           =   1575
         End
         Begin VB.Frame frameAddText 
            Caption         =   "A&dditional Text"
            Height          =   1935
            Left            =   2760
            TabIndex        =   115
            Top             =   0
            Width           =   3615
            Begin ATCoCtl.ATCoText txtTextLoc 
               Height          =   255
               Index           =   0
               Left            =   1320
               TabIndex        =   112
               Top             =   1200
               Width           =   615
               _ExtentX        =   1085
               _ExtentY        =   450
               InsideLimitsBackground=   16777215
               OutsideHardLimitBackground=   8421631
               OutsideSoftLimitBackground=   8454143
               HardMax         =   -999
               HardMin         =   -999
               SoftMax         =   1.5
               SoftMin         =   -0.5
               MaxWidth        =   7
               Alignment       =   1
               DataType        =   2
               DefaultValue    =   "0.8"
               Value           =   "0.8"
               Enabled         =   -1  'True
            End
            Begin ATCoCtl.ATCoText txtTextLoc 
               Height          =   255
               Index           =   1
               Left            =   1320
               TabIndex        =   114
               Top             =   1440
               Width           =   615
               _ExtentX        =   1085
               _ExtentY        =   450
               InsideLimitsBackground=   16777215
               OutsideHardLimitBackground=   8421631
               OutsideSoftLimitBackground=   8454143
               HardMax         =   -999
               HardMin         =   -999
               SoftMax         =   1.5
               SoftMin         =   -0.5
               MaxWidth        =   7
               Alignment       =   1
               DataType        =   2
               DefaultValue    =   "1"
               Value           =   "1"
               Enabled         =   -1  'True
            End
            Begin ATCoCtl.ATCoText txtAddText 
               Height          =   255
               Index           =   1
               Left            =   240
               TabIndex        =   109
               Top             =   600
               Width           =   3135
               _ExtentX        =   5530
               _ExtentY        =   450
               InsideLimitsBackground=   16777215
               OutsideHardLimitBackground=   8421631
               OutsideSoftLimitBackground=   8454143
               HardMax         =   -999
               HardMin         =   -999
               SoftMax         =   -999
               SoftMin         =   -999
               MaxWidth        =   5
               Alignment       =   1
               DataType        =   0
               DefaultValue    =   ""
               Value           =   ""
               Enabled         =   -1  'True
            End
            Begin ATCoCtl.ATCoText txtAddText 
               Height          =   255
               Index           =   2
               Left            =   240
               TabIndex        =   110
               Top             =   840
               Width           =   3135
               _ExtentX        =   5530
               _ExtentY        =   450
               InsideLimitsBackground=   16777215
               OutsideHardLimitBackground=   8421631
               OutsideSoftLimitBackground=   8454143
               HardMax         =   -999
               HardMin         =   -999
               SoftMax         =   -999
               SoftMin         =   -999
               MaxWidth        =   5
               Alignment       =   1
               DataType        =   0
               DefaultValue    =   ""
               Value           =   ""
               Enabled         =   -1  'True
            End
            Begin ATCoCtl.ATCoText txtAddText 
               Height          =   255
               Index           =   0
               Left            =   240
               TabIndex        =   108
               Top             =   360
               Width           =   3135
               _ExtentX        =   5530
               _ExtentY        =   450
               InsideLimitsBackground=   16777215
               OutsideHardLimitBackground=   8421631
               OutsideSoftLimitBackground=   8454143
               HardMax         =   -999
               HardMin         =   -999
               SoftMax         =   -999
               SoftMin         =   -999
               MaxWidth        =   5
               Alignment       =   1
               DataType        =   0
               DefaultValue    =   ""
               Value           =   ""
               Enabled         =   -1  'True
            End
            Begin VB.Label lblTextLoc 
               Caption         =   "&Y Position:"
               Height          =   255
               Index           =   1
               Left            =   240
               TabIndex        =   113
               Top             =   1440
               WhatsThisHelpID =   19
               Width           =   855
            End
            Begin VB.Label lblTextLoc 
               Caption         =   "&X Position:"
               Height          =   255
               Index           =   0
               Left            =   255
               TabIndex        =   111
               Top             =   1200
               WhatsThisHelpID =   19
               Width           =   855
            End
         End
         Begin VB.Frame frameLegendLocation 
            Caption         =   "Legend Location"
            Height          =   1935
            Left            =   0
            TabIndex        =   100
            Top             =   0
            Width           =   2655
            Begin VB.OptionButton optLegend 
               Caption         =   "&None"
               Enabled         =   0   'False
               Height          =   252
               Index           =   0
               Left            =   240
               TabIndex        =   101
               Top             =   340
               Width           =   852
            End
            Begin VB.OptionButton optLegend 
               Caption         =   "&Upper left corner"
               Enabled         =   0   'False
               Height          =   252
               Index           =   1
               Left            =   240
               TabIndex        =   102
               Top             =   600
               Width           =   2295
            End
            Begin VB.OptionButton optLegend 
               Caption         =   "&Specify as fraction of axes:"
               Enabled         =   0   'False
               Height          =   252
               Index           =   2
               Left            =   240
               TabIndex        =   103
               Top             =   860
               Width           =   2295
            End
            Begin ATCoCtl.ATCoText txtLegLoc 
               Height          =   255
               Index           =   0
               Left            =   1440
               TabIndex        =   105
               Top             =   1200
               Width           =   615
               _ExtentX        =   1085
               _ExtentY        =   450
               InsideLimitsBackground=   16777215
               OutsideHardLimitBackground=   8421631
               OutsideSoftLimitBackground=   8454143
               HardMax         =   -999
               HardMin         =   -999
               SoftMax         =   1.5
               SoftMin         =   -0.5
               MaxWidth        =   7
               Alignment       =   1
               DataType        =   2
               DefaultValue    =   "0"
               Value           =   "0"
               Enabled         =   -1  'True
            End
            Begin ATCoCtl.ATCoText txtLegLoc 
               Height          =   255
               Index           =   1
               Left            =   1440
               TabIndex        =   107
               Top             =   1440
               Width           =   615
               _ExtentX        =   1085
               _ExtentY        =   450
               InsideLimitsBackground=   16777215
               OutsideHardLimitBackground=   8421631
               OutsideSoftLimitBackground=   8454143
               HardMax         =   -999
               HardMin         =   -999
               SoftMax         =   1.5
               SoftMin         =   -0.5
               MaxWidth        =   7
               Alignment       =   1
               DataType        =   2
               DefaultValue    =   "1"
               Value           =   "1"
               Enabled         =   -1  'True
            End
            Begin VB.Label lblLegLoc 
               Caption         =   "&X Position:"
               Enabled         =   0   'False
               Height          =   255
               Index           =   0
               Left            =   480
               TabIndex        =   104
               Top             =   1200
               WhatsThisHelpID =   17
               Width           =   975
            End
            Begin VB.Label lblLegLoc 
               Caption         =   "&Y Position:"
               Enabled         =   0   'False
               Height          =   255
               Index           =   1
               Left            =   480
               TabIndex        =   106
               Top             =   1440
               WhatsThisHelpID =   17
               Width           =   855
            End
         End
         Begin VB.Label Label1 
            Caption         =   "Data &Labels:"
            Height          =   255
            Left            =   0
            TabIndex        =   116
            Top             =   2080
            Width           =   1095
         End
      End
      Begin VB.Frame fratab 
         BorderStyle     =   0  'None
         Caption         =   "Frame2"
         Height          =   3255
         Index           =   0
         Left            =   -74880
         TabIndex        =   1
         Top             =   360
         Width           =   6615
         Begin VB.Frame fraXaxis 
            Caption         =   "X-axis"
            Height          =   3255
            Left            =   0
            TabIndex        =   2
            Top             =   0
            Width           =   1572
            Begin VB.OptionButton optXaxis 
               Caption         =   "Probability"
               Height          =   252
               Index           =   3
               Left            =   240
               TabIndex        =   68
               Top             =   4680
               WhatsThisHelpID =   1
               Width           =   1212
            End
            Begin VB.OptionButton optXaxis 
               Caption         =   "Logarit&hmic"
               Height          =   252
               Index           =   2
               Left            =   120
               TabIndex        =   4
               Top             =   480
               WhatsThisHelpID =   1
               Width           =   1212
            End
            Begin VB.OptionButton optXaxis 
               Caption         =   "Time"
               Height          =   252
               Index           =   0
               Left            =   240
               TabIndex        =   67
               Top             =   4440
               WhatsThisHelpID =   1
               Width           =   972
            End
            Begin VB.OptionButton optXaxis 
               Caption         =   "&Arithmetic"
               Height          =   255
               Index           =   1
               Left            =   120
               TabIndex        =   3
               Top             =   240
               WhatsThisHelpID =   1
               Width           =   1215
            End
            Begin ATCoCtl.ATCoText txtAMin 
               Height          =   255
               Index           =   4
               Left            =   600
               TabIndex        =   8
               Top             =   2520
               Width           =   735
               _ExtentX        =   1296
               _ExtentY        =   450
               InsideLimitsBackground=   16777215
               OutsideHardLimitBackground=   8421631
               OutsideSoftLimitBackground=   8454143
               HardMax         =   -999
               HardMin         =   -999
               SoftMax         =   -999
               SoftMin         =   -999
               MaxWidth        =   5
               Alignment       =   1
               DataType        =   2
               DefaultValue    =   "0"
               Value           =   "0"
               Enabled         =   -1  'True
            End
            Begin ATCoCtl.ATCoText txtTics 
               Height          =   285
               Index           =   4
               Left            =   600
               TabIndex        =   6
               Top             =   1110
               Width           =   495
               _ExtentX        =   873
               _ExtentY        =   503
               InsideLimitsBackground=   16777215
               OutsideHardLimitBackground=   8421631
               OutsideSoftLimitBackground=   8454143
               HardMax         =   -999
               HardMin         =   0
               SoftMax         =   20
               SoftMin         =   -999
               MaxWidth        =   5
               Alignment       =   1
               DataType        =   0
               DefaultValue    =   "0"
               Value           =   "0"
               Enabled         =   -1  'True
            End
            Begin ATCoCtl.ATCoText txtAMax 
               Height          =   255
               Index           =   4
               Left            =   600
               TabIndex        =   10
               Top             =   2760
               Width           =   735
               _ExtentX        =   1296
               _ExtentY        =   450
               InsideLimitsBackground=   16777215
               OutsideHardLimitBackground=   8421631
               OutsideSoftLimitBackground=   8454143
               HardMax         =   -999
               HardMin         =   -999
               SoftMax         =   -999
               SoftMin         =   -999
               MaxWidth        =   5
               Alignment       =   1
               DataType        =   2
               DefaultValue    =   "1"
               Value           =   "1"
               Enabled         =   -1  'True
            End
            Begin VB.Label lblTics 
               Caption         =   "Tic&s:"
               Height          =   255
               Index           =   4
               Left            =   120
               TabIndex        =   5
               Top             =   1150
               WhatsThisHelpID =   2
               Width           =   495
            End
            Begin VB.Label lblDataRange 
               Caption         =   "Data Range:"
               Height          =   255
               Index           =   4
               Left            =   120
               TabIndex        =   74
               Top             =   1560
               WhatsThisHelpID =   5
               Width           =   1095
            End
            Begin VB.Label lblDataMin 
               Caption         =   "Min:"
               Height          =   255
               Index           =   4
               Left            =   240
               TabIndex        =   73
               Top             =   1800
               WhatsThisHelpID =   5
               Width           =   375
            End
            Begin VB.Label lblDataMax 
               Caption         =   "Max:"
               Height          =   255
               Index           =   4
               Left            =   240
               TabIndex        =   72
               Top             =   2040
               WhatsThisHelpID =   5
               Width           =   375
            End
            Begin VB.Label lblAxisRange 
               Caption         =   "Axis Scale Range:"
               Height          =   255
               Index           =   4
               Left            =   120
               TabIndex        =   71
               Top             =   2280
               WhatsThisHelpID =   6
               Width           =   1335
            End
            Begin VB.Label lblDMin 
               Height          =   255
               Index           =   4
               Left            =   600
               TabIndex        =   70
               Top             =   1800
               WhatsThisHelpID =   5
               Width           =   735
            End
            Begin VB.Label lblDMax 
               Height          =   255
               Index           =   4
               Left            =   600
               TabIndex        =   69
               Top             =   2040
               WhatsThisHelpID =   5
               Width           =   735
            End
            Begin VB.Label lblAxisMin 
               Caption         =   "Mi&n:"
               Height          =   255
               Index           =   4
               Left            =   240
               TabIndex        =   7
               Top             =   2520
               WhatsThisHelpID =   6
               Width           =   375
            End
            Begin VB.Label lblAxisMax 
               Caption         =   "Ma&x:"
               Height          =   255
               Index           =   4
               Left            =   240
               TabIndex        =   9
               Top             =   2760
               WhatsThisHelpID =   6
               Width           =   375
            End
         End
         Begin VB.Frame fraAuxAxis 
            Caption         =   "Auxilliary axis"
            Height          =   3252
            Left            =   5040
            TabIndex        =   37
            Top             =   0
            Width           =   1572
            Begin VB.CheckBox chkGrid 
               Caption         =   "G&rid"
               Height          =   255
               Index           =   3
               Left            =   120
               TabIndex        =   40
               Top             =   840
               Visible         =   0   'False
               Width           =   615
            End
            Begin VB.ComboBox cboAuxSize 
               Height          =   315
               ItemData        =   "frmGEdit.frx":04D2
               Left            =   600
               List            =   "frmGEdit.frx":04F4
               TabIndex        =   39
               Top             =   480
               WhatsThisHelpID =   7
               Width           =   732
            End
            Begin ATCoCtl.ATCoText txtTics 
               Height          =   285
               Index           =   3
               Left            =   600
               TabIndex        =   42
               Top             =   1110
               Width           =   495
               _ExtentX        =   873
               _ExtentY        =   503
               InsideLimitsBackground=   16777215
               OutsideHardLimitBackground=   8421631
               OutsideSoftLimitBackground=   8454143
               HardMax         =   -999
               HardMin         =   0
               SoftMax         =   20
               SoftMin         =   -999
               MaxWidth        =   5
               Alignment       =   1
               DataType        =   0
               DefaultValue    =   "0"
               Value           =   "0"
               Enabled         =   -1  'True
            End
            Begin ATCoCtl.ATCoText txtAMin 
               Height          =   255
               Index           =   3
               Left            =   600
               TabIndex        =   44
               Top             =   2520
               Width           =   735
               _ExtentX        =   1296
               _ExtentY        =   450
               InsideLimitsBackground=   16777215
               OutsideHardLimitBackground=   8421631
               OutsideSoftLimitBackground=   8454143
               HardMax         =   -999
               HardMin         =   -999
               SoftMax         =   -999
               SoftMin         =   -999
               MaxWidth        =   7
               Alignment       =   1
               DataType        =   2
               DefaultValue    =   "0"
               Value           =   "0"
               Enabled         =   -1  'True
            End
            Begin ATCoCtl.ATCoText txtAMax 
               Height          =   255
               Index           =   3
               Left            =   600
               TabIndex        =   46
               Top             =   2760
               Width           =   735
               _ExtentX        =   1296
               _ExtentY        =   450
               InsideLimitsBackground=   16777215
               OutsideHardLimitBackground=   8421631
               OutsideSoftLimitBackground=   8454143
               HardMax         =   -999
               HardMin         =   -999
               SoftMax         =   -999
               SoftMin         =   -999
               MaxWidth        =   7
               Alignment       =   1
               DataType        =   2
               DefaultValue    =   "1"
               Value           =   "1"
               Enabled         =   -1  'True
            End
            Begin VB.Label lblAuxSize 
               Caption         =   "&Fraction of Y Axis:"
               Height          =   255
               Left            =   120
               TabIndex        =   38
               Top             =   240
               Width           =   1335
            End
            Begin VB.Label lblDataRange 
               Caption         =   "Data Range:"
               Height          =   255
               Index           =   3
               Left            =   120
               TabIndex        =   96
               Top             =   1560
               WhatsThisHelpID =   5
               Width           =   1095
            End
            Begin VB.Label lblDataMin 
               Caption         =   "Min:"
               Height          =   255
               Index           =   3
               Left            =   120
               TabIndex        =   95
               Top             =   1800
               WhatsThisHelpID =   5
               Width           =   375
            End
            Begin VB.Label lblDataMax 
               Caption         =   "Max:"
               Height          =   255
               Index           =   3
               Left            =   120
               TabIndex        =   94
               Top             =   2040
               WhatsThisHelpID =   5
               Width           =   375
            End
            Begin VB.Label lblAxisRange 
               Caption         =   "Axis Scale Range:"
               Height          =   255
               Index           =   3
               Left            =   120
               TabIndex        =   93
               Top             =   2280
               WhatsThisHelpID =   6
               Width           =   1335
            End
            Begin VB.Label lblDMin 
               Height          =   255
               Index           =   3
               Left            =   600
               TabIndex        =   92
               Top             =   1800
               WhatsThisHelpID =   5
               Width           =   735
            End
            Begin VB.Label lblDMax 
               Height          =   255
               Index           =   3
               Left            =   600
               TabIndex        =   91
               Top             =   2040
               WhatsThisHelpID =   5
               Width           =   735
            End
            Begin VB.Label lblAxisMin 
               Caption         =   "Mi&n:"
               Height          =   255
               Index           =   3
               Left            =   240
               TabIndex        =   43
               Top             =   2520
               WhatsThisHelpID =   6
               Width           =   375
            End
            Begin VB.Label lblAxisMax 
               Caption         =   "Ma&x:"
               Height          =   255
               Index           =   3
               Left            =   240
               TabIndex        =   45
               Top             =   2760
               WhatsThisHelpID =   6
               Width           =   375
            End
            Begin VB.Label lblTics 
               Caption         =   "Tic&s:"
               Height          =   255
               Index           =   3
               Left            =   120
               TabIndex        =   41
               Top             =   1150
               WhatsThisHelpID =   2
               Width           =   495
            End
         End
         Begin VB.Frame fraRYaxis 
            Caption         =   "Right Y-axis"
            Height          =   3252
            Left            =   3360
            TabIndex        =   27
            Top             =   0
            Width           =   1572
            Begin VB.OptionButton optRYaxis 
               Caption         =   "Logarit&hmic"
               Height          =   252
               Index           =   2
               Left            =   120
               TabIndex        =   29
               Top             =   480
               WhatsThisHelpID =   4
               Width           =   1212
            End
            Begin VB.OptionButton optRYaxis 
               Caption         =   "&Arithmetic"
               Height          =   252
               Index           =   1
               Left            =   120
               TabIndex        =   28
               Top             =   240
               WhatsThisHelpID =   4
               Width           =   1212
            End
            Begin VB.CheckBox chkGrid 
               Caption         =   "G&rid"
               Height          =   255
               Index           =   2
               Left            =   120
               TabIndex        =   30
               Top             =   820
               Width           =   615
            End
            Begin ATCoCtl.ATCoText txtTics 
               Height          =   285
               Index           =   2
               Left            =   600
               TabIndex        =   32
               Top             =   1110
               Width           =   495
               _ExtentX        =   873
               _ExtentY        =   503
               InsideLimitsBackground=   16777215
               OutsideHardLimitBackground=   8421631
               OutsideSoftLimitBackground=   8454143
               HardMax         =   -999
               HardMin         =   0
               SoftMax         =   20
               SoftMin         =   -999
               MaxWidth        =   5
               Alignment       =   1
               DataType        =   0
               DefaultValue    =   "0"
               Value           =   "0"
               Enabled         =   -1  'True
            End
            Begin ATCoCtl.ATCoText txtAMin 
               Height          =   255
               Index           =   2
               Left            =   600
               TabIndex        =   34
               Top             =   2520
               Width           =   735
               _ExtentX        =   1296
               _ExtentY        =   450
               InsideLimitsBackground=   16777215
               OutsideHardLimitBackground=   8421631
               OutsideSoftLimitBackground=   8454143
               HardMax         =   -999
               HardMin         =   -999
               SoftMax         =   -999
               SoftMin         =   -999
               MaxWidth        =   7
               Alignment       =   1
               DataType        =   2
               DefaultValue    =   "0"
               Value           =   "0"
               Enabled         =   -1  'True
            End
            Begin ATCoCtl.ATCoText txtAMax 
               Height          =   255
               Index           =   2
               Left            =   600
               TabIndex        =   36
               Top             =   2760
               Width           =   735
               _ExtentX        =   1296
               _ExtentY        =   450
               InsideLimitsBackground=   16777215
               OutsideHardLimitBackground=   8421631
               OutsideSoftLimitBackground=   8454143
               HardMax         =   -999
               HardMin         =   -999
               SoftMax         =   -999
               SoftMin         =   -999
               MaxWidth        =   7
               Alignment       =   1
               DataType        =   2
               DefaultValue    =   "1"
               Value           =   "1"
               Enabled         =   -1  'True
            End
            Begin VB.Label lblTics 
               Caption         =   "Tic&s:"
               Height          =   255
               Index           =   2
               Left            =   120
               TabIndex        =   31
               Top             =   1155
               WhatsThisHelpID =   2
               Width           =   495
            End
            Begin VB.Label lblDataRange 
               Caption         =   "Data Range:"
               Height          =   255
               Index           =   2
               Left            =   120
               TabIndex        =   90
               Top             =   1560
               WhatsThisHelpID =   5
               Width           =   1095
            End
            Begin VB.Label lblDataMin 
               Caption         =   "Min:"
               Height          =   255
               Index           =   2
               Left            =   120
               TabIndex        =   89
               Top             =   1800
               WhatsThisHelpID =   5
               Width           =   375
            End
            Begin VB.Label lblDataMax 
               Caption         =   "Max:"
               Height          =   255
               Index           =   2
               Left            =   120
               TabIndex        =   88
               Top             =   2040
               WhatsThisHelpID =   5
               Width           =   375
            End
            Begin VB.Label lblAxisRange 
               Caption         =   "Axis Scale Range:"
               Height          =   255
               Index           =   2
               Left            =   120
               TabIndex        =   87
               Top             =   2280
               WhatsThisHelpID =   6
               Width           =   1335
            End
            Begin VB.Label lblDMin 
               Height          =   255
               Index           =   2
               Left            =   600
               TabIndex        =   86
               Top             =   1800
               WhatsThisHelpID =   5
               Width           =   735
            End
            Begin VB.Label lblDMax 
               Height          =   255
               Index           =   2
               Left            =   600
               TabIndex        =   85
               Top             =   2040
               WhatsThisHelpID =   5
               Width           =   735
            End
            Begin VB.Label lblAxisMin 
               Caption         =   "Mi&n:"
               Height          =   255
               Index           =   2
               Left            =   240
               TabIndex        =   33
               Top             =   2520
               WhatsThisHelpID =   6
               Width           =   375
            End
            Begin VB.Label lblAxisMax 
               Caption         =   "Ma&x:"
               Height          =   255
               Index           =   2
               Left            =   240
               TabIndex        =   35
               Top             =   2760
               WhatsThisHelpID =   6
               Width           =   375
            End
         End
         Begin VB.Frame fraLYaxis 
            Caption         =   "Left Y-axis"
            Height          =   3252
            Left            =   1680
            TabIndex        =   17
            Top             =   0
            Width           =   1572
            Begin VB.ComboBox cboLYClip 
               Height          =   315
               Index           =   1
               ItemData        =   "frmGEdit.frx":0525
               Left            =   720
               List            =   "frmGEdit.frx":0535
               TabIndex        =   76
               Top             =   3720
               Width           =   852
            End
            Begin VB.ComboBox cboLYClip 
               Height          =   315
               Index           =   0
               ItemData        =   "frmGEdit.frx":0554
               Left            =   720
               List            =   "frmGEdit.frx":0564
               TabIndex        =   75
               Top             =   3480
               Width           =   852
            End
            Begin VB.OptionButton optLYaxis 
               Caption         =   "Logarit&hmic"
               Height          =   255
               Index           =   2
               Left            =   120
               TabIndex        =   19
               Top             =   480
               WhatsThisHelpID =   3
               Width           =   1215
            End
            Begin VB.OptionButton optLYaxis 
               Caption         =   "&Arithmetic"
               Height          =   255
               Index           =   1
               Left            =   120
               TabIndex        =   18
               Top             =   240
               WhatsThisHelpID =   3
               Width           =   1215
            End
            Begin VB.CheckBox chkGrid 
               Caption         =   "G&rid"
               Height          =   255
               Index           =   1
               Left            =   120
               TabIndex        =   20
               Top             =   840
               Width           =   615
            End
            Begin ATCoCtl.ATCoText txtTics 
               Height          =   285
               Index           =   1
               Left            =   600
               TabIndex        =   22
               Top             =   1110
               Width           =   495
               _ExtentX        =   873
               _ExtentY        =   503
               InsideLimitsBackground=   16777215
               OutsideHardLimitBackground=   8421631
               OutsideSoftLimitBackground=   8454143
               HardMax         =   -999
               HardMin         =   0
               SoftMax         =   20
               SoftMin         =   -999
               MaxWidth        =   5
               Alignment       =   1
               DataType        =   0
               DefaultValue    =   "0"
               Value           =   "0"
               Enabled         =   -1  'True
            End
            Begin ATCoCtl.ATCoText txtAMin 
               Height          =   255
               Index           =   1
               Left            =   600
               TabIndex        =   24
               Top             =   2520
               Width           =   735
               _ExtentX        =   1296
               _ExtentY        =   450
               InsideLimitsBackground=   16777215
               OutsideHardLimitBackground=   8421631
               OutsideSoftLimitBackground=   8454143
               HardMax         =   -999
               HardMin         =   -999
               SoftMax         =   -999
               SoftMin         =   -999
               MaxWidth        =   7
               Alignment       =   1
               DataType        =   2
               DefaultValue    =   0
               Value           =   "0"
               Enabled         =   -1  'True
            End
            Begin ATCoCtl.ATCoText txtAMax 
               Height          =   255
               Index           =   1
               Left            =   600
               TabIndex        =   26
               Top             =   2760
               Width           =   735
               _ExtentX        =   1296
               _ExtentY        =   450
               InsideLimitsBackground=   16777215
               OutsideHardLimitBackground=   8421631
               OutsideSoftLimitBackground=   8454143
               HardMax         =   -999
               HardMin         =   -999
               SoftMax         =   -999
               SoftMin         =   -999
               MaxWidth        =   7
               Alignment       =   1
               DataType        =   2
               DefaultValue    =   "1"
               Value           =   "1"
               Enabled         =   -1  'True
            End
            Begin VB.Label lblLYClip 
               Caption         =   "Bottom:"
               Height          =   252
               Index           =   1
               Left            =   120
               TabIndex        =   84
               Top             =   3720
               Width           =   612
            End
            Begin VB.Label lblLYClip 
               Caption         =   "Off Top:"
               Height          =   252
               Index           =   0
               Left            =   120
               TabIndex        =   83
               Top             =   3480
               Width           =   612
            End
            Begin VB.Label lblAxisMax 
               Caption         =   "Ma&x:"
               Height          =   255
               Index           =   1
               Left            =   240
               TabIndex        =   25
               Top             =   2760
               WhatsThisHelpID =   6
               Width           =   375
            End
            Begin VB.Label lblAxisMin 
               Caption         =   "Mi&n:"
               Height          =   255
               Index           =   1
               Left            =   240
               TabIndex        =   23
               Top             =   2520
               WhatsThisHelpID =   6
               Width           =   375
            End
            Begin VB.Label lblDMax 
               Height          =   255
               Index           =   1
               Left            =   600
               TabIndex        =   82
               Top             =   2040
               WhatsThisHelpID =   5
               Width           =   735
            End
            Begin VB.Label lblDMin 
               Height          =   255
               Index           =   1
               Left            =   600
               TabIndex        =   81
               Top             =   1800
               WhatsThisHelpID =   5
               Width           =   735
            End
            Begin VB.Label lblAxisRange 
               Caption         =   "Axis Scale Range:"
               Height          =   255
               Index           =   1
               Left            =   120
               TabIndex        =   80
               Top             =   2280
               WhatsThisHelpID =   6
               Width           =   1335
            End
            Begin VB.Label lblDataMax 
               Caption         =   "Max:"
               Height          =   255
               Index           =   1
               Left            =   240
               TabIndex        =   79
               Top             =   2040
               WhatsThisHelpID =   5
               Width           =   375
            End
            Begin VB.Label lblDataMin 
               Caption         =   "Min:"
               Height          =   255
               Index           =   1
               Left            =   240
               TabIndex        =   78
               Top             =   1800
               WhatsThisHelpID =   5
               Width           =   375
            End
            Begin VB.Label lblDataRange 
               Caption         =   "Data Range:"
               Height          =   255
               Index           =   1
               Left            =   120
               TabIndex        =   77
               Top             =   1560
               WhatsThisHelpID =   5
               Width           =   1095
            End
            Begin VB.Label lblTics 
               Caption         =   "Tic&s:"
               Height          =   255
               Index           =   1
               Left            =   120
               TabIndex        =   21
               Top             =   1150
               WhatsThisHelpID =   2
               Width           =   495
            End
         End
      End
      Begin VB.Frame fratab 
         BorderStyle     =   0  'None
         Caption         =   "Frame1"
         Height          =   3255
         Index           =   1
         Left            =   -74880
         TabIndex        =   47
         Top             =   360
         Width           =   6615
         Begin VB.TextBox txtTitles 
            Height          =   288
            Index           =   7
            Left            =   960
            TabIndex        =   61
            Top             =   2745
            WhatsThisHelpID =   8
            Width           =   5652
         End
         Begin VB.TextBox txtTitles 
            Height          =   288
            Index           =   6
            Left            =   960
            TabIndex        =   59
            Top             =   2400
            WhatsThisHelpID =   8
            Width           =   5652
         End
         Begin VB.TextBox txtTitles 
            Height          =   288
            Index           =   5
            Left            =   960
            TabIndex        =   57
            Top             =   2040
            WhatsThisHelpID =   8
            Width           =   5652
         End
         Begin VB.TextBox txtTitles 
            BackColor       =   &H80000016&
            Enabled         =   0   'False
            Height          =   288
            Index           =   3
            Left            =   960
            TabIndex        =   55
            Top             =   1560
            WhatsThisHelpID =   8
            Width           =   5652
         End
         Begin VB.TextBox txtTitles 
            BackColor       =   &H80000016&
            Enabled         =   0   'False
            Height          =   288
            Index           =   2
            Left            =   960
            TabIndex        =   53
            Top             =   1065
            WhatsThisHelpID =   8
            Width           =   5652
         End
         Begin VB.TextBox txtTitles 
            BackColor       =   &H80000016&
            Enabled         =   0   'False
            Height          =   288
            Index           =   1
            Left            =   960
            TabIndex        =   51
            Top             =   600
            WhatsThisHelpID =   8
            Width           =   5652
         End
         Begin VB.TextBox txtTitles 
            BackColor       =   &H80000016&
            Enabled         =   0   'False
            Height          =   288
            Index           =   4
            Left            =   960
            TabIndex        =   49
            Top             =   120
            WhatsThisHelpID =   8
            Width           =   5652
         End
         Begin VB.Label lblTitles 
            Alignment       =   1  'Right Justify
            Caption         =   "&3"
            Height          =   255
            Index           =   6
            Left            =   0
            TabIndex        =   60
            Top             =   2745
            Width           =   855
         End
         Begin VB.Label lblTitles 
            Alignment       =   1  'Right Justify
            Caption         =   "&2"
            Height          =   255
            Index           =   5
            Left            =   0
            TabIndex        =   58
            Top             =   2400
            Width           =   855
         End
         Begin VB.Label lblTitles 
            Alignment       =   1  'Right Justify
            Caption         =   "Main:        &1"
            Height          =   255
            Index           =   2
            Left            =   0
            TabIndex        =   56
            Top             =   2040
            Width           =   855
         End
         Begin VB.Label lblTitles 
            Caption         =   "A&ux axis:"
            Height          =   255
            Index           =   4
            Left            =   0
            TabIndex        =   54
            Top             =   1560
            Width           =   975
         End
         Begin VB.Label lblTitles 
            Caption         =   "&Right Y-axis:"
            Height          =   255
            Index           =   3
            Left            =   0
            TabIndex        =   52
            Top             =   1080
            Width           =   975
         End
         Begin VB.Label lblTitles 
            Caption         =   "&Left Y-axis:"
            Height          =   255
            Index           =   1
            Left            =   0
            TabIndex        =   50
            Top             =   600
            Width           =   855
         End
         Begin VB.Label lblTitles 
            Caption         =   "&X-axis:"
            Height          =   255
            Index           =   0
            Left            =   0
            TabIndex        =   48
            Top             =   120
            Width           =   855
         End
      End
      Begin VB.Frame fratab 
         BorderStyle     =   0  'None
         Caption         =   "Frame1"
         Height          =   3255
         Index           =   2
         Left            =   -74880
         TabIndex        =   62
         Top             =   360
         Width           =   6615
         Begin ATCoCtl.ATCoGrid agdCurves 
            Height          =   3255
            Left            =   0
            TabIndex        =   97
            Top             =   0
            Width           =   6615
            _ExtentX        =   11668
            _ExtentY        =   5741
            SelectionToggle =   0   'False
            AllowBigSelection=   0   'False
            AllowEditHeader =   0   'False
            AllowLoad       =   0   'False
            AllowSorting    =   -1  'True
            Rows            =   2
            Cols            =   2
            ColWidthMinimum =   300
            gridFontBold    =   0   'False
            gridFontItalic  =   0   'False
            gridFontName    =   "MS Sans Serif"
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
            ComboCheckValidValues=   -1  'True
         End
      End
      Begin VB.Frame fratab 
         BorderStyle     =   0  'None
         Caption         =   "Frame1"
         Height          =   3255
         Index           =   3
         Left            =   -74880
         TabIndex        =   118
         Top             =   360
         Width           =   6615
         Begin VB.CommandButton cmdCurve 
            Appearance      =   0  'Flat
            BackColor       =   &H80000005&
            Caption         =   "Remove"
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
            Index           =   1
            Left            =   1080
            TabIndex        =   119
            Top             =   3000
            Width           =   972
         End
         Begin VB.CommandButton cmdCurve 
            Appearance      =   0  'Flat
            BackColor       =   &H80000005&
            Caption         =   "Add"
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
            Left            =   0
            TabIndex        =   98
            Top             =   3000
            Width           =   972
         End
         Begin ATCoCtl.ATCoGrid agdLines 
            Height          =   2895
            Left            =   0
            TabIndex        =   120
            Top             =   0
            Width           =   6615
            _ExtentX        =   11668
            _ExtentY        =   5741
            SelectionToggle =   0   'False
            AllowBigSelection=   0   'False
            AllowEditHeader =   0   'False
            AllowLoad       =   0   'False
            AllowSorting    =   -1  'True
            Rows            =   2
            Cols            =   2
            ColWidthMinimum =   300
            gridFontBold    =   0   'False
            gridFontItalic  =   0   'False
            gridFontName    =   "MS Sans Serif"
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
            ComboCheckValidValues=   -1  'True
         End
      End
   End
End
Attribute VB_Name = "frmGEdit"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
'Copyright 2001 by AQUA TERRA Consultants

'Axes Info: 1-lY, 2-rY, 3-Aux, 4-X
Dim AType(1 To 4) As Long   'axis type 0TM,1AR,2LG,3-8PRB
Dim pmin(1 To 4) As Single  'min axis value
Dim pmax(1 To 4) As Single  'max axis value
Dim dmin(1 To 4) As Single  'min data value/axis
Dim dmax(1 To 4) As Single  'max data value/axis
Dim NTic(1 To 4) As Long    'number of tics
Dim grid(1 To 4) As Long    '1 to draw grid
Dim label(1 To 4) As String 'axis label
Dim xstype&                  'time axis supp type
Dim Xint&                   'time axis interval
'variable information
Dim xvar() As Long    'x variable for each curve
Dim yvar() As Long    'y variable for each curve
Dim vmin() As Single  'min data value/variable
Dim vmax() As Single  'max data value/variable
Dim which() As Long   'which axis code
Dim tran() As Long    'transformation, 1-AR, 2-LG
Dim vlab() As String  'label
'curve information
Dim ctyp() As Long
Dim ltyp() As Long
Dim lthk() As Long
Dim styp() As Long
Dim clr() As Long
Dim legend() As String
Dim ltstep() As Long
Dim ltunit() As Long
Dim ldtype() As Long
Dim lsdate(5) As Long
Dim ledate(5) As Long
Dim lncrv&, lnvar&
'Dim lnNonDataLines&
Dim lAuxLen!
Dim lTitle$, lCapt$
Dim LegLoc(1) As Single
Dim TxtLoc(1) As Single
Dim ExtraText As String
Dim CurHelpFile$
Dim AutoScale(4) As Boolean 'scale axis flags

Private AxisName$(4)
Private LineTypeName$(6)
Private ThicknessName$(3)
Private MarkerName$(6)
Private ValueName$(2)

Const AxisEmptyToolTip = "To use this axis," & vbCr & "move a curve to it" & vbCr & "in the Curve tab."

Const ColAxis = 0
Const ColLine = 1
Const ColMarker = 2
Const ColColor = 3
Const ColValue = 4
Const ColLegend = 5

Private Sub agdCurves_CommitChange(ChangeFromRow As Long, ChangeToRow As Long, ChangeFromCol As Long, ChangeToCol As Long)
  Dim r&, c&, Crv&, index&, txt$
  For r = ChangeFromRow To ChangeToRow
    Crv = r - 1
    For c = ChangeFromCol To ChangeToCol
      txt = agdCurves.TextMatrix(r, c)
      Select Case c
        Case ColLine:   For index = 1 To UBound(LineTypeName):  If txt = LineTypeName(index) Then ltyp(Crv) = index - 1
                        Next
                        For index = 1 To UBound(ThicknessName): If txt = ThicknessName(index) Then lthk(Crv) = index: ltyp(Crv) = 1
                        Next
        Case ColAxis:   For index = 1 To UBound(AxisName):      If txt = AxisName(index) Then AxisChange r, index
                        Next
        Case ColMarker: For index = 1 To UBound(MarkerName):    If txt = MarkerName(index) Then styp(Crv) = index - 1
                        Next
        Case ColValue:  For index = 1 To UBound(ValueName):     If txt = ValueName(index) Then ldtype(Crv) = index
                        Next
        Case ColColor:  clr(Crv) = TextOrNumericColor(txt)
        Case ColLegend: legend(Crv) = txt
      End Select
    Next c
  Next r
End Sub

Private Sub agdCurves_RowColChange()
  Dim index&
  With agdCurves
    .ClearValues
    Select Case .col
      Case ColLine:   .addValue LineTypeName(1)
                      For index = 1 To UBound(ThicknessName): .addValue ThicknessName(index): Next
                      For index = 3 To UBound(LineTypeName):  .addValue LineTypeName(index):  Next
      Case ColAxis:   For index = 1 To UBound(AxisName):      .addValue AxisName(index):      Next
      Case ColMarker: For index = 1 To UBound(MarkerName):    .addValue MarkerName(index):    Next
      Case ColValue:  For index = 1 To UBound(ValueName):     .addValue ValueName(index):     Next
      Case ColColor:
      Case ColLegend:
    End Select
  End With
End Sub

Private Sub agdLines_RowColChange()
  Dim index
  With agdLines
    .ClearValues
    Select Case .col
      Case ColLine:   .addValue LineTypeName(1)
                      For index = 1 To UBound(ThicknessName): .addValue ThicknessName(index): Next
                      For index = 3 To UBound(LineTypeName):  .addValue LineTypeName(index):  Next
      Case ColAxis:   For index = 1 To UBound(AxisName):      .addValue AxisName(index):      Next
      Case ColMarker: For index = 1 To UBound(MarkerName):    .addValue MarkerName(index):    Next
      
      Case ColValue:  index = AxisNumFromName(.TextMatrix(.row, ColAxis))
                      If txtAMin(index).Enabled Then
                        .ColSoftMin(.col) = txtAMin(index).Value
                        .ColSoftMax(.col) = txtAMax(index).Value
                      Else
                        .ColSoftMin(.col) = 0
                        .ColSoftMax(.col) = 1
                      End If
      Case ColColor:
      Case ColLegend:
    End Select
  End With
End Sub

Private Sub AxisChange(curveChanged&, newAxis&)

    Dim i&, mcrv&
    Dim lY As Boolean, RY As Boolean, Aux As Boolean

    which(yvar(curveChanged - 1)) = newAxis
    lY = False
    RY = False
    Aux = False
    'count number of curves on main plot (left or right y)
    mcrv = 0
    For i = 0 To lncrv - 1
      'check curves to see which axes are in use
      If which(yvar(i)) = 1 Then 'left in use
        lY = True
        mcrv = mcrv + 1
        If AType(1) = 0 Then 'wasn't in use, default to arithmetic
          AType(1) = 1
          AutoScale(1) = True
        End If
      ElseIf which(yvar(i)) = 2 Then 'right in use
        RY = True
        mcrv = mcrv + 1
        If AType(2) = 0 Then 'wasn't in use, default to arithmetic
          AType(2) = 1
          AutoScale(2) = True
        End If
      ElseIf which(yvar(i)) = 3 Then 'aux in use
        Aux = True
        If lAuxLen = 0 Then 'wasn't in use, default length of aux
          lAuxLen = 0.2
          AutoScale(3) = True
        End If
      End If
    Next i
    If Not lY Then AType(1) = 0
    If Not RY Then AType(2) = 0
    If Not Aux Then lAuxLen = 0
    If mcrv < 2 Then '1 or less curves on main plot,
      'no legend needed
      optLegend(0).Value = True
    End If
    Call UpdateYAxes

End Sub

Private Sub chkGrid_Click(index As Integer)
  If chkGrid(index).Value = 1 Then grid(index) = 1 Else grid(index) = 0
End Sub

Private Sub cmdClose_Click(index As Integer)

  Dim i&, j&, minv!, maxv!
  
  'allow grids to update currently open cell
  cmdClose(index).SetFocus
  DoEvents
  
  If index = 0 Or index = 2 Then 'ok or apply clicked
    MousePointer = vbHourglass
    'save axes titles
    For i = 1 To 4
      label(i) = txtTitles(i).Text
    Next i
    'save axes types
    If cboAuxSize.Enabled And _
       cboAuxSize.Text <> "" Then
      lAuxLen = CSng(cboAuxSize.Text)
    End If
    'Call frmG.SetNumVars(lncrv, lnvar, lnNonDataLines)
    frmG.NumCurves = lncrv
    frmG.NumVars = lnvar
    If Xint < 4 Then
      Call frmG.SetAxesInfo(AType(4), xstype, Xint, AType(1), AType(2), lAuxLen, label(4), label(1), label(2), label(3))
    Else 'day or larger, interval is always 1
      Call frmG.SetAxesInfo(AType(4), xstype, 1, AType(1), AType(2), lAuxLen, label(4), label(1), label(2), label(3))
    End If
    Call frmG.SetScale(pmin, pmax, NTic)
    Call frmG.SetGrid(grid(4), grid(1), grid(2))
    Call frmG.SetVarInfo(vmin, vmax, which, tran, vlab)
    'update title
    lTitle = txtTitles(5).Text
    If txtTitles(6).Text <> "" Then
      lTitle = lTitle & "&" & txtTitles(6).Text
    End If
    If txtTitles(7).Text <> "" Then
      lTitle = lTitle & "&" & txtTitles(7).Text
    End If
    Call frmG.SetTitles(lTitle, lCapt)
    'set legend location
    If optLegend(0).Value = True Then 'none
      LegLoc(0) = NONE
      LegLoc(1) = NONE
    ElseIf optLegend(1).Value = True Then 'default
      LegLoc(0) = 0
      LegLoc(1) = 1
    End If
    Call frmG.SetLegLoc(LegLoc(0), LegLoc(1))
    'set additional text
    ExtraText = txtAddText(0).Value
    If txtAddText(1).Value <> "" Then
      ExtraText = ExtraText & "&" & txtAddText(1).Value
    End If
    If txtAddText(2).Value <> "" Then
      ExtraText = ExtraText & "&" & txtAddText(2).Value
    End If
    Call frmG.SetAddText(TxtLoc(0), TxtLoc(1), ExtraText)
    'update curve info
    Call frmG.SetCurveInfo(ctyp, ltyp, lthk, styp, clr, legend)
    'set time info
    Call frmG.SetTime(ltstep, ltunit, lsdate, ledate, ldtype)
    Call frmG.SetDataLabelPosition(ComboDataLabels.ListIndex)
    Set curG = frmG
    SetFrmGlines
    Call frmG.ReDrawGraph(-1)
    MousePointer = vbDefault
  End If
  
  If index <> 2 Then 'close window if Apply was not pressed (Ok or Cancel)
    'restore application help file
    App.HelpFile = CurHelpFile
    'close form
    Unload frmGEdit
  End If
End Sub

Private Function AxisNumFromName(thisName$) As Long
  Dim index&, retval&
  index = 0
  retval = -1
  While retval = -1 And index <= UBound(AxisName)
    If thisName = AxisName(index) Then retval = index Else index = index + 1
  Wend
  If retval = 0 Then retval = 4
  If retval = -1 Then retval = 1
  AxisNumFromName = retval
End Function

Private Sub SetFrmGlines()
  Dim WchAx&, LType&, LThck&, SType&, Color&, LegLbl$, Value$
  Dim txt$, r&, index&
  frmG.NumLines = 0 'lnNonDataLines
  With agdLines
    For r = 1 To .rows 'lnNonDataLines
      LType = -1
      txt = .TextMatrix(r, ColLine)
      index = 1
      While LType = -1 And index <= UBound(LineTypeName)
        If txt = LineTypeName(index) Then LType = index - 1 Else index = index + 1
      Wend
      index = 1
      While LType = -1 And index <= UBound(ThicknessName)
        If txt = ThicknessName(index) Then LThck = index: LType = 1 Else index = index + 1
      Wend
      
      WchAx = AxisNumFromName(.TextMatrix(r, ColAxis))

      SType = -2
      txt = .TextMatrix(r, ColMarker)
      index = 0
      While SType = -2 And index <= UBound(MarkerName)
        If txt = MarkerName(index) Then SType = index - 1 Else index = index + 1
      Wend
            
      Color = TextOrNumericColor(.TextMatrix(r, ColColor))
      LegLbl = .TextMatrix(r, ColLegend)
      Value = .TextMatrix(r, ColValue)
      frmG.AddLine WchAx&, LType&, LThck&, SType&, Color&, LegLbl$, Value$
    Next r
  End With
End Sub

Private Sub cmdCurve_Click(index As Integer)
  Dim dimension&
  With agdLines
    Select Case index
      Case 0 'lnNonDataLines = lnNonDataLines + 1
        .rows = .rows + 1
        .TextMatrix(.rows, ColAxis) = AxisName(1)
        .TextMatrix(.rows, ColLine) = ThicknessName(1)
        .TextMatrix(.rows, ColMarker) = MarkerName(1)
        .TextMatrix(.rows, ColColor) = "black"
      Case 1 'lnNonDataLines = lnNonDataLines - 1
        If agdLines.rows > 0 Then agdLines.DeleteRows
    End Select
  End With
  'agdLines.rows = lnNonDataLines
  'dimension = lncrv + lnNonDataLines - 1
  'ReDim Preserve ctyp(dimension)
  'ReDim Preserve yvar(dimension)
  'ReDim Preserve ltyp(dimension)
  'ReDim Preserve lthk(dimension)
  'ReDim Preserve styp(dimension)
  'ReDim Preserve clr(dimension)
  'ReDim Preserve legend(dimension)
  'ReDim Preserve ldtype(dimension)
  'ReDim Preserve legend(dimension)
  'ReDim Preserve which(dimension)
  'If index = 0 Then
  '  lthk(dimension) = 1
  '
  'End If
  LoadCurveGrid
End Sub

Private Sub Form_Activate()

  Dim i&, tpos&, tp2&

  'save current application help file
  CurHelpFile = App.HelpFile
  App.HelpFile = GraphHelpFileName
  'get number of curves/variables
  'Call frmG.GetNumVars(lncrv, lnvar, lnNonDataLines)
  lncrv = frmG.NumCurves
  lnvar = frmG.NumVars
  If lncrv > 0 And lnvar > 0 Then
    'get variables assigned to each curve
    ReDim xvar(lncrv - 1)
    ReDim yvar(lncrv - 1)
    For i = 0 To lncrv - 1
      Call frmG.GetVars(i, yvar(i), xvar(i))
    Next i
    'get current variable info
    ReDim vmin(lnvar - 1)
    ReDim vmax(lnvar - 1)
    ReDim which(lnvar - 1)
    ReDim tran(lnvar - 1)
    ReDim vlab(lnvar - 1)
    Call frmG.GetVarInfo(vmin, vmax, which, tran, vlab)
    'get current curve info
    ReDim ctyp(lncrv - 1)
    ReDim ltyp(lncrv - 1)
    ReDim lthk(lncrv - 1)
    ReDim styp(lncrv - 1)
    ReDim clr(lncrv - 1)
    ReDim legend(lncrv - 1)
    Call frmG.GetCurveInfo(ctyp, ltyp, lthk, styp, clr, legend)
    'get time info
    ReDim ltstep(lncrv - 1)
    ReDim ltunit(lncrv - 1)
    ReDim ldtype(lncrv - 1)
    Call frmG.GetTime(ltstep, ltunit, lsdate, ledate, ldtype)
    'get current axes info
    Call frmG.GetAxesInfo(AType(4), xstype, Xint, AType(1), AType(2), lAuxLen, label(4), label(1), label(2), label(3))
    'get axes min/max/tics
    Call frmG.GetScale(pmin, pmax, NTic)
    'default to not automatically scale axes
    For i = 1 To 4
      AutoScale(i) = False
    Next i
    'Set grid checkboxes for left, right, bottom axes
    Call frmG.GetGrid(grid(4), grid(1), grid(2))
    chkGrid(1).Value = grid(1)
    chkGrid(2).Value = grid(2)
    chkGrid(4).Value = grid(4)
    
    If AType(4) <> -1 Then
      'not doing boxplots
      If Abs(AType(4)) < 3 Then
        optXaxis(Abs(AType(4))).Value = True
      Else
        optXaxis(3).Value = True
      End If
    End If
    If AType(4) = 0 Then 'can't change from time on X-axis
      fraXaxis.Visible = False
      'For I = 1 To 3 'disable other options
      '  optXaxis(I).Enabled = False
      'Next I
      'Call EnableAxis(4, False) 'disable range fields
      fraXAxisTime.Visible = True
      If xstype = 6 Then 'year
        rdoTMAx(0) = True
      ElseIf xstype = 5 Then 'month
        rdoTMAx(1) = True
      ElseIf xstype = 4 Then 'day
        rdoTMAx(2) = True
      Else  'hour
        rdoTMAx(3) = True
      End If
    Else 'x axis label valid
      fraXaxis.Visible = True
      fraXAxisTime.Visible = False
      txtTitles(4).Text = label(4)
      txtTitles(4).Enabled = True
      txtTitles(4).BackColor = QBColor(15)
      If Abs(AType(4)) > 2 Then 'can't change probability on X-axis
        For i = 0 To 2 'disable other options
          optXaxis(i).Enabled = False
        Next i
        Call EnableAxis(4, False) 'disable range fields
      Else 'arithmetic or log
        'can't switch to time or probability
        optXaxis(0).Enabled = False
        optXaxis(3).Enabled = False
        txtTics(4).Value = NTic(4)
        txtAMin(4).Value = pmin(4)
        txtAMax(4).Value = pmax(4)
        dmin(4) = 1000000
        dmax(4) = -1000000
        For i = 0 To lnvar - 1
          If which(i) = 4 Then
            If vmin(i) < dmin(4) Then dmin(4) = vmin(i)
            If vmax(i) > dmax(4) Then dmax(4) = vmax(i)
          End If
        Next i
        lblDMin(4).Caption = CStr(dmin(4))
        lblDMax(4).Caption = CStr(dmax(4))
      End If
    End If
    Call UpdateYAxes
    If AType(4) = -1 Then
      'disable most features for boxplot
      fraXaxis.Enabled = False
      Call EnableAxis(4, False) 'disable range fields
      txtAMin(4).Value = pmin(4)
      txtAMax(4).Value = pmax(4)
      optXaxis(1).Enabled = False
      optXaxis(2).Enabled = False
      txtAddText(0).Enabled = False
      txtAddText(1).Enabled = False
      txtAddText(2).Enabled = False
      txtTextLoc(0).Enabled = False
      txtTextLoc(1).Enabled = False
    End If
    'get current titles
    Call frmG.GetTitles(lTitle, lCapt)
    tpos = InStr(lTitle, "&")
    If tpos > 0 Then
      txtTitles(5).Text = Left(lTitle, tpos - 1)
      tp2 = InStr(tpos + 1, lTitle, "&")
      If tp2 > 0 Then
        txtTitles(6).Text = Mid(lTitle, tpos + 1, tp2 - tpos - 1)
        txtTitles(7).Text = Mid(lTitle, tp2 + 1)
      Else
        txtTitles(6).Text = Mid(lTitle, tpos + 1)
      End If
    Else
      txtTitles(5).Text = lTitle
    End If
    If lncrv > 1 And AType(4) <> -1 Then
      'get legend location unless doing box plots
      Call frmG.GetLegLoc(LegLoc(0), LegLoc(1))
      frameLegendLocation.Enabled = True
      txtLegLoc(0).Value = LegLoc(0)
      txtLegLoc(1).Value = LegLoc(1)
      For i = 0 To 2
        optLegend(i).Enabled = True
      Next i
      If LegLoc(0) = NONE Then 'none for start
        optLegend(0).Value = True
      ElseIf LegLoc(0) = 0 And LegLoc(1) = 1 Then 'upper left corner
        optLegend(1).Value = True
      Else 'specific location defined
        optLegend(2).Value = True
      End If
    End If
    'get any extra text
    Call frmG.GetAddText(TxtLoc(0), TxtLoc(1), ExtraText)
    txtTextLoc(0).Value = TxtLoc(0)
    txtTextLoc(1).Value = TxtLoc(1)
    tpos = InStr(ExtraText, "&")
    If tpos > 0 Then
      txtAddText(0).Value = Left(ExtraText, tpos - 1)
      tp2 = InStr(tpos + 1, ExtraText, "&")
      If tp2 > 0 Then
        txtAddText(1).Value = Mid(ExtraText, tpos + 1, tp2 - tpos - 1)
        txtAddText(2).Value = Mid(ExtraText, tp2 + 1)
      Else
        txtAddText(1).Value = Mid(ExtraText, tpos + 1)
      End If
    Else
      txtAddText(0).Value = ExtraText
    End If
    'Setup combo box for choosing data label behavior
    ComboDataLabels.Clear
    ComboDataLabels.AddItem "None"
    ComboDataLabels.AddItem "Horizontal"
    ComboDataLabels.AddItem "Vertical"
    ComboDataLabels.AddItem "Pop-up"
    If frmG.GetNdataLabels > 0 Then
      ComboDataLabels.Enabled = True
      ComboDataLabels.ListIndex = frmG.GetDataLabelPosition
    Else
      ComboDataLabels.ListIndex = 0
      ComboDataLabels.Enabled = False
    End If
    LoadCurveGrid
    LoadLineGrid
  End If
End Sub

Private Sub Form_Load()
  Dim c&
  AxisName(1) = "Left"
  AxisName(2) = "Right"
  AxisName(3) = "Auxillary"
  AxisName(4) = "Bottom"
  LineTypeName(1) = "None"
  LineTypeName(2) = "Solid"
  LineTypeName(3) = "Dash"
  LineTypeName(4) = "Dot"
  LineTypeName(5) = "Dash-Dot"
  LineTypeName(6) = "Dash-Dot-Dot"
  ThicknessName(1) = "Thin"
  ThicknessName(2) = "Medium"
  ThicknessName(3) = "Thick"
  MarkerName(1) = "None"
  MarkerName(2) = "."
  MarkerName(3) = "+"
  MarkerName(4) = "*"
  MarkerName(5) = "o"
  MarkerName(6) = "x"
  ValueName(1) = "Mean"
  ValueName(2) = "Point"
  With agdCurves
    .ColTitle(ColAxis) = "Axis"
    .ColTitle(ColLine) = "Line Type"    '.ColTitle(ColThick) = "Thickness"
    .ColTitle(ColMarker) = "Marker"
    .ColTitle(ColValue) = "Value Type"
    .ColTitle(ColColor) = "Color"
    .ColTitle(ColLegend) = "Legend Entry"
    
    .ColType(ColColor) = ATCoClr
    
    For c = 0 To 5
      .ColEditable(c) = True
    Next c
  End With
  With agdLines
    .ColTitle(ColAxis) = "Axis"
    .ColTitle(ColLine) = "Line Type"    '.ColTitle(ColThick) = "Thickness"
    .ColTitle(ColMarker) = "HIDE"
    .ColTitle(ColValue) = "Value"
    .ColTitle(ColColor) = "Color"
    .ColTitle(ColLegend) = "Legend Entry"
    
    .ColType(ColColor) = ATCoClr
    
    For c = 0 To 5
      .ColEditable(c) = True
    Next c
  End With
  sstGEdit_Click 0
End Sub

Private Sub LoadCurveGrid()
  Dim Crv&, rows&
  With agdCurves
    rows = lncrv '+ lnNonDataLines
    .rows = rows
    For Crv = 0 To rows - 1
      If ltyp(Crv) = 1 Then
        .TextMatrix(Crv + 1, ColLine) = ThicknessName(lthk(Crv))
      Else
        .TextMatrix(Crv + 1, ColLine) = LineTypeName(ltyp(Crv) + 1)
      End If
      .TextMatrix(Crv + 1, ColAxis) = AxisName(which(yvar(Crv)))
      .TextMatrix(Crv + 1, ColMarker) = MarkerName(styp(Crv) + 1)
      .TextMatrix(Crv + 1, ColValue) = ValueName(ldtype(Crv))
      .TextMatrix(Crv + 1, ColColor) = colorName(clr(Crv))
      .TextMatrix(Crv + 1, ColLegend) = Trim(legend(Crv))
    Next Crv
    .ColsSizeByContents
  End With
End Sub

Private Sub LoadLineGrid()
  agdLines.rows = frmG.NumLines
  Dim WchAx&, LType&, LThck&, SType&, Color&, LegLbl$, Value$
  Dim txt$, r&, index&
  With agdLines
    For r = 1 To .rows 'lnNonDataLines
      frmG.GetLine r, WchAx&, LType&, LThck&, SType&, Color&, LegLbl$, Value$
      .TextMatrix(r, ColAxis) = AxisName(WchAx)
      If LType = 1 Then
        .TextMatrix(r, ColLine) = ThicknessName(LThck)
      Else
        .TextMatrix(r, ColLine) = LineTypeName(LType + 1)
      End If
      If SType < 1 Then .TextMatrix(r, ColMarker) = MarkerName(1) Else .TextMatrix(r, ColMarker) = MarkerName(SType)
      .TextMatrix(r, ColColor) = colorName(Color)
      .TextMatrix(r, ColLegend) = LegLbl
      .TextMatrix(r, ColValue) = Value
    Next r
  End With
End Sub

Private Sub optLegend_Click(index As Integer)

    If index = 2 Then 'specifying location
      If LegLoc(0) < -0.5 Then
        'was none or default, set to default position
        LegLoc(0) = 0
        LegLoc(1) = 1
        txtLegLoc(0).Value = LegLoc(0)
        txtLegLoc(1).Value = LegLoc(1)
      End If
      lblLegLoc(0).Enabled = True
      txtLegLoc(0).Enabled = True
      lblLegLoc(1).Enabled = True
      txtLegLoc(1).Enabled = True
    Else
      If index = 1 Then 'Upper left corner
        txtLegLoc(0).Value = 0
        txtLegLoc(1).Value = 1
      Else
        txtLegLoc(0).Value = NONE
        txtLegLoc(1).Value = NONE
      End If
      lblLegLoc(0).Enabled = False
      txtLegLoc(0).Enabled = False
      lblLegLoc(1).Enabled = False
      txtLegLoc(1).Enabled = False
    End If
    'Just in case these were not triggered above
    txtLegLoc_Change 0
    txtLegLoc_Change 1
End Sub

Private Sub optLYaxis_Click(index As Integer)

    Dim j&

    If index = 1 Then
      'changing to arithmetic, allow tic mod
      lblTics(1).Enabled = True
      txtTics(1).Enabled = True
      If txtTics(1).Value < 1 Then txtTics(1).Value = 5
      'txtTics(1).BackColor = QBColor(15)
    Else
      'changing to log, disable tic mod
      lblTics(1).Enabled = False
      txtTics(1).Enabled = False
      'txtTics(1).BackColor = QBColor(7)
    End If
    If Abs(AType(1)) <> index Then
      'changing left axis type
      AType(1) = index
      'redo axis scale
      For j = 0 To lnvar - 1
        If which(j) = 1 Then
          tran(j) = AType(1)
        End If
      Next j
      Call Scalit((AType(1)), dmin(1), dmax(1), pmin(1), pmax(1))
      txtAMin(1).Value = pmin(1)
      txtAMax(1).Value = pmax(1)
    End If

End Sub

Private Sub optRYaxis_Click(index As Integer)

    Dim j&

    If index = 1 Then
      'changing to arithmetic, allow tic mod
      lblTics(2).Enabled = True
      txtTics(2).Enabled = True
      'txtTics(2).BackColor = QBColor(15)
    Else
      'changing to log, disable tic mod
      lblTics(2).Enabled = False
      txtTics(2).Enabled = False
      'txtTics(2).BackColor = QBColor(7)
    End If
    If Abs(AType(2)) <> index Then
      'changing right axis type
      AType(2) = index
      'redo axis scale
      For j = 0 To lnvar - 1
        If which(j) = 2 Then
          tran(j) = AType(2)
        End If
      Next j
      Call Scalit((AType(2)), dmin(2), dmax(2), pmin(2), pmax(2))
      txtAMin(2).Value = pmin(2)
      txtAMax(2).Value = pmax(2)
    End If

End Sub


Private Sub optXaxis_Click(index As Integer)

  Dim j&

  If index = 1 Then
    'changing to arithmetic, allow tic mod
    lblTics(4).Enabled = True
    txtTics(4).Enabled = True
    'txtTics(4).BackColor = QBColor(15)
  Else
    'changing to time/log/prob, disable tic mod
    lblTics(4).Enabled = False
    txtTics(4).Enabled = False
    'txtTics(4).BackColor = QBColor(7)
  End If
  If AType(4) <> index And (index = 1 Or index = 2) Then
    'changing x axis type from arith to log or vice versa
    AType(4) = index
    'redo axis scale
    For j = 0 To lnvar - 1
      If which(j) = 4 Then
        tran(j) = AType(4)
      End If
    Next j
    Call Scalit((AType(4)), dmin(4), dmax(4), pmin(4), pmax(4))
    txtAMin(4).Value = pmin(4)
    txtAMax(4).Value = pmax(4)
  End If
End Sub

Private Sub rdoTMAx_Click(index As Integer)
  txtIntvl.Visible = False
  If rdoTMAx(0) Then 'year
    xstype = 6
  ElseIf rdoTMAx(1) Then 'month
    xstype = 5
  ElseIf rdoTMAx(2) Then 'day
    xstype = 4
  ElseIf rdoTMAx(3) Then
    xstype = 2
    txtIntvl.Visible = True
    txtIntvl.Value = Xint
  End If
End Sub

Private Sub sstGEdit_Click(PreviousTab As Integer)
  Dim t&
  For t = 0 To 3
    If t = sstGEdit.Tab Then fratab(t).Visible = True Else fratab(t).Visible = False
  Next t
End Sub

Private Sub txtAMax_Change(index As Integer)
  pmax(index) = txtAMax(index).Value
End Sub

Private Sub txtAMin_Change(index As Integer)
  If txtAMin(index).Value < 0.00001 And Abs(AType(index)) = 2 Then
    txtAMin(index).Value = 0.00001
  End If
  pmin(index) = txtAMin(index).Value
End Sub

Private Sub txtIntvl_Change()
  Xint = txtIntvl.Value
End Sub

Private Sub txtLegLoc_Change(index As Integer)
  LegLoc(index) = txtLegLoc(index).Value
End Sub

Private Sub txtTextLoc_Change(index As Integer)
  TxtLoc(index) = txtTextLoc(index).Value
End Sub

Private Sub txtTics_Change(index As Integer)
  NTic(index) = txtTics(index).Value
End Sub

Private Sub UpdateYAxes()
  Dim i&
  If Abs(AType(1)) > 0 Then 'left y in use
    Call EnableAxis(1, True)
    If AType(1) <> -1 Then
      'do not do this for boxplots which have no y axis
      optLYaxis(Abs(AType(1))).Value = True
    End If
    txtTics(1).Value = NTic(1)
    Call UpdateAxisRange(1)
  Else 'disable left axis frame
    Call EnableAxis(1, False)
  End If
  If Abs(AType(2)) > 0 Then 'right Y-axis in use
    Call EnableAxis(2, True)
    optRYaxis(Abs(AType(2))).Value = True
    txtTics(2).Value = NTic(2)
    Call UpdateAxisRange(2)
  Else 'disable right axis frame
    Call EnableAxis(2, False)
  End If
  If lAuxLen > 0 Then
    AType(3) = 1
    Call EnableAxis(3, True)
    cboAuxSize.Text = CStr(lAuxLen)
    txtTics(3).Value = NTic(3)
    Call UpdateAxisRange(3)
  Else 'disable aux axis
    AType(3) = 0
    Call EnableAxis(3, False)
  End If
End Sub

Private Sub EnableAxis(iax&, TorF As Boolean)

  chkGrid(iax).Enabled = TorF
  lblTics(iax).Enabled = TorF
  txtTics(iax).Enabled = TorF
  If iax = 1 Then 'left y
    fraLYaxis.Enabled = TorF
    If TorF Then
      fraLYaxis.ToolTipText = ""
      optLYaxis(1).ToolTipText = ""
      optLYaxis(2).ToolTipText = ""
    Else
      fraLYaxis.ToolTipText = AxisEmptyToolTip
      optLYaxis(1).ToolTipText = AxisEmptyToolTip
      optLYaxis(2).ToolTipText = AxisEmptyToolTip
    End If
    optLYaxis(1).Enabled = TorF
    optLYaxis(2).Enabled = TorF
  ElseIf iax = 2 Then 'right y
    fraRYaxis.Enabled = TorF
    optRYaxis(1).Enabled = TorF
    optRYaxis(2).Enabled = TorF
  ElseIf iax = 3 Then 'aux
    fraAuxAxis.Enabled = TorF
    lblAuxSize.Enabled = TorF
    cboAuxSize.Enabled = TorF
    If TorF Then 'enable, use white background
      cboAuxSize.BackColor = QBColor(15)
    Else 'disable, use grey background
      cboAuxSize.BackColor = QBColor(7)
    End If
  ElseIf iax = 4 Then
    fraXaxis.Enabled = TorF
    chkGrid(iax).Enabled = True
  End If
  'If TorF Then 'enable, use white background
  '  txtTics(iax).BackColor = QBColor(15)
  'Else 'disable, use grey background
  '  txtTics(iax).BackColor = QBColor(7)
  'End If
  lblDataRange(iax).Enabled = TorF
  lblDataMin(iax).Enabled = TorF
  lblDataMax(iax).Enabled = TorF
  lblDMin(iax).Enabled = TorF
  lblDMax(iax).Enabled = TorF
  lblAxisRange(iax).Enabled = TorF
  lblAxisMin(iax).Enabled = TorF
  lblAxisMax(iax).Enabled = TorF
  txtAMin(iax).Enabled = TorF
  txtAMax(iax).Enabled = TorF
  txtTitles(iax).Enabled = TorF
  txtTitles(iax).Text = label(iax)
  If TorF Then 'enable, use white background
    txtTitles(iax).BackColor = QBColor(15)
  Else 'disable, use grey background
    txtTitles(iax).BackColor = QBColor(7)
  End If
End Sub

Private Sub UpdateAxisRange(iax&)

    Dim i&

    'determine data range
    dmin(iax) = 1000000
    dmax(iax) = -1000000
    For i = 0 To lnvar - 1
      If which(i) = iax Then
        If vmin(i) < dmin(iax) Then dmin(iax) = vmin(i)
        If vmax(i) > dmax(iax) Then dmax(iax) = vmax(i)
      End If
    Next i
    'display data range
    lblDMin(iax).Caption = CStr(dmin(iax))
    lblDMax(iax).Caption = CStr(dmax(iax))
    'update (if needed) axis scale
    If AutoScale(iax) Then 'scale axis based on data range
      Call Scalit((AType(iax)), dmin(iax), dmax(iax), pmin(iax), pmax(iax))
    ElseIf AType(iax) = -2 Then
      'transformed log, transform back for editing
      AType(iax) = 2 'reset
      pmin(iax) = 10 ^ pmin(iax)
      pmax(iax) = 10 ^ pmax(iax)
    End If
    txtAMin(iax).Value = pmin(iax)
    txtAMax(iax).Value = pmax(iax)
    AutoScale(iax) = False

End Sub

Private Sub UpdateTitles(tind&, EnabFg As Boolean)

End Sub
