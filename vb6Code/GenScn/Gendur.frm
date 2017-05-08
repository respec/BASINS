VERSION 5.00
Begin VB.Form frmGenScnDuration 
   BorderStyle     =   3  'Fixed Dialog
   Caption         =   "GenScn Duration"
   ClientHeight    =   5460
   ClientLeft      =   3525
   ClientTop       =   2430
   ClientWidth     =   5895
   HelpContextID   =   107
   Icon            =   "Gendur.frx":0000
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   PaletteMode     =   1  'UseZOrder
   ScaleHeight     =   5460
   ScaleWidth      =   5895
   Begin VB.CommandButton cmdClear 
      Caption         =   "Cl&ear Specs"
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
      Left            =   3960
      TabIndex        =   53
      Top             =   4920
      Width           =   1332
   End
   Begin VB.CommandButton cmdGet 
      Caption         =   "&Get Specs"
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
      Left            =   2280
      TabIndex        =   52
      TabStop         =   0   'False
      Top             =   4920
      Width           =   1332
   End
   Begin VB.CommandButton cmdSave 
      Caption         =   "&Save Specs"
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
      Left            =   600
      TabIndex        =   51
      Top             =   4920
      Width           =   1332
   End
   Begin VB.CommandButton cmdOutfile 
      Caption         =   "Output &File"
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
      Left            =   240
      TabIndex        =   47
      Top             =   4320
      Width           =   1092
   End
   Begin VB.CommandButton cmdResults 
      Caption         =   "&Results"
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
      Left            =   3120
      TabIndex        =   49
      Top             =   4320
      Width           =   1092
   End
   Begin VB.CommandButton cmdAnalyze 
      Caption         =   "&Compute"
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
      Left            =   1680
      TabIndex        =   48
      Top             =   4320
      Width           =   1092
   End
   Begin VB.TextBox txtTitle 
      BackColor       =   &H00FFFFFF&
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   288
      Left            =   120
      TabIndex        =   0
      Text            =   "????"
      Top             =   120
      Width           =   5652
   End
   Begin VB.CommandButton cmdClose 
      Cancel          =   -1  'True
      Caption         =   "Close"
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
      Left            =   4560
      TabIndex        =   50
      Top             =   4320
      Width           =   1092
   End
   Begin TabDlg.SSTab SSTab1 
      Height          =   3612
      Left            =   120
      TabIndex        =   1
      Top             =   480
      Width           =   5652
      _ExtentX        =   9975
      _ExtentY        =   6376
      _Version        =   393216
      Tabs            =   4
      Tab             =   1
      TabsPerRow      =   4
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
      TabCaption(0)   =   "&Durations"
      TabPicture(0)   =   "Gendur.frx":0442
      Tab(0).ControlEnabled=   0   'False
      Tab(0).Control(0)=   "fraTab(0)"
      Tab(0).ControlCount=   1
      TabCaption(1)   =   "&Thresholds"
      TabPicture(1)   =   "Gendur.frx":045E
      Tab(1).ControlEnabled=   -1  'True
      Tab(1).Control(0)=   "fraTab(1)"
      Tab(1).Control(0).Enabled=   0   'False
      Tab(1).ControlCount=   1
      TabCaption(2)   =   "&Options"
      TabPicture(2)   =   "Gendur.frx":047A
      Tab(2).ControlEnabled=   0   'False
      Tab(2).Control(0)=   "fraTab(2)"
      Tab(2).ControlCount=   1
      TabCaption(3)   =   "&Lethality"
      TabPicture(3)   =   "Gendur.frx":0496
      Tab(3).ControlEnabled=   0   'False
      Tab(3).Control(0)=   "fraTab(3)"
      Tab(3).ControlCount=   1
      Begin VB.Frame fraTab 
         BorderStyle     =   0  'None
         Caption         =   "Frame1"
         Height          =   3135
         Index           =   3
         Left            =   -74880
         TabIndex        =   58
         Top             =   360
         Width           =   5295
         Begin VB.Frame fraLethOut 
            Caption         =   "Output Level"
            BeginProperty Font 
               Name            =   "MS Sans Serif"
               Size            =   8.25
               Charset         =   0
               Weight          =   700
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   1452
            Left            =   120
            TabIndex        =   60
            Top             =   1560
            Width           =   1455
            Begin VB.OptionButton optLethOut 
               Caption         =   "&None"
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
               Index           =   0
               Left            =   120
               TabIndex        =   43
               Top             =   360
               Value           =   -1  'True
               Width           =   1095
            End
            Begin VB.OptionButton optLethOut 
               Caption         =   "Lo&w"
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
               Index           =   1
               Left            =   120
               TabIndex        =   44
               Top             =   600
               Width           =   1095
            End
            Begin VB.OptionButton optLethOut 
               Caption         =   "&Middle"
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
               Index           =   2
               Left            =   120
               TabIndex        =   45
               Top             =   840
               Width           =   1095
            End
            Begin VB.OptionButton optLethOut 
               Caption         =   "&High"
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
               Index           =   3
               Left            =   120
               TabIndex        =   46
               Top             =   1080
               Width           =   1095
            End
         End
         Begin VB.OptionButton optLethoffon 
            Caption         =   "O&ff"
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
            Index           =   0
            Left            =   1800
            TabIndex        =   36
            Top             =   120
            Width           =   735
         End
         Begin VB.OptionButton optLethoffon 
            Caption         =   "On"
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
            Index           =   1
            Left            =   2520
            TabIndex        =   37
            Top             =   120
            Width           =   735
         End
         Begin VB.Frame fraGTLT 
            Caption         =   "Critical Level"
            BeginProperty Font 
               Name            =   "MS Sans Serif"
               Size            =   8.25
               Charset         =   0
               Weight          =   700
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   732
            Left            =   120
            TabIndex        =   59
            Top             =   480
            Width           =   1455
            Begin VB.OptionButton optGTLT 
               Caption         =   "&>"
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
               Index           =   0
               Left            =   120
               TabIndex        =   41
               Top             =   360
               Value           =   -1  'True
               Width           =   612
            End
            Begin VB.OptionButton optGTLT 
               Caption         =   "&<"
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
               Index           =   1
               Left            =   720
               TabIndex        =   42
               Top             =   360
               Width           =   492
            End
         End
         Begin VB.ComboBox cmbNoLethal 
            BackColor       =   &H00FFFFFF&
            BeginProperty Font 
               Name            =   "MS Sans Serif"
               Size            =   8.25
               Charset         =   0
               Weight          =   700
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   315
            Left            =   2640
            TabIndex        =   39
            Text            =   "1"
            Top             =   480
            Width           =   1212
         End
         Begin ATCoCtl.ATCoGrid grdLethal 
            Height          =   2175
            Left            =   1800
            TabIndex        =   40
            Top             =   840
            Width           =   3495
            _ExtentX        =   6165
            _ExtentY        =   3836
            SelectionToggle =   0   'False
            AllowBigSelection=   -1  'True
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
            FixedCols       =   1
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
         Begin VB.Label lblDurInt 
            Caption         =   "No duration intervals have been set."
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
            Left            =   1920
            TabIndex        =   62
            Top             =   1680
            Width           =   3135
         End
         Begin VB.Label Label4 
            Caption         =   "C&urves:"
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
            Left            =   1800
            TabIndex        =   38
            Top             =   480
            Width           =   975
         End
         Begin VB.Label Label3 
            Caption         =   "Lethality Analysis:"
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
            TabIndex        =   61
            Top             =   120
            Width           =   1575
         End
      End
      Begin VB.Frame fraTab 
         BorderStyle     =   0  'None
         Caption         =   "Frame1"
         Height          =   3015
         Index           =   1
         Left            =   120
         TabIndex        =   56
         Top             =   360
         Width           =   5415
         Begin VB.ListBox lstActThresh 
            BeginProperty Font 
               Name            =   "MS Sans Serif"
               Size            =   8.25
               Charset         =   0
               Weight          =   700
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   1230
            ItemData        =   "Gendur.frx":04B2
            Left            =   4200
            List            =   "Gendur.frx":04B9
            TabIndex        =   27
            Top             =   600
            Width           =   1092
         End
         Begin VB.ListBox lstThresholds 
            BeginProperty Font 
               Name            =   "MS Sans Serif"
               Size            =   8.25
               Charset         =   0
               Weight          =   700
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   1230
            ItemData        =   "Gendur.frx":04CB
            Left            =   2160
            List            =   "Gendur.frx":04D2
            TabIndex        =   20
            Top             =   600
            Width           =   1092
         End
         Begin VB.TextBox txtThresholds 
            BeginProperty Font 
               Name            =   "MS Sans Serif"
               Size            =   8.25
               Charset         =   0
               Weight          =   700
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   288
            Left            =   2160
            TabIndex        =   21
            Top             =   2400
            Width           =   1092
         End
         Begin VB.CommandButton cmdThreshaddall 
            Caption         =   ">>"
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
            Left            =   3480
            TabIndex        =   22
            Top             =   600
            Width           =   492
         End
         Begin VB.CommandButton cmdThreshadd 
            Caption         =   "--&>"
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
            Left            =   3480
            TabIndex        =   23
            Top             =   960
            Width           =   492
         End
         Begin VB.CommandButton cmdThreshdrop 
            Caption         =   "&<--"
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
            Left            =   3480
            TabIndex        =   24
            Top             =   1320
            Width           =   492
         End
         Begin VB.CommandButton cmdThreshdelall 
            Caption         =   "<<"
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
            Left            =   3480
            TabIndex        =   25
            Top             =   1680
            Width           =   492
         End
         Begin VB.Frame fraThresDef 
            Caption         =   "Defaults"
            BeginProperty Font 
               Name            =   "MS Sans Serif"
               Size            =   8.25
               Charset         =   0
               Weight          =   700
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   2172
            Left            =   120
            TabIndex        =   57
            Top             =   480
            Width           =   1812
            Begin VB.OptionButton optThres 
               Caption         =   "&Arithmetic"
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
               Index           =   0
               Left            =   120
               TabIndex        =   11
               Top             =   240
               Value           =   -1  'True
               Width           =   1212
            End
            Begin VB.OptionButton optThres 
               Caption         =   "Logarit&hmic"
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
               Index           =   1
               Left            =   120
               TabIndex        =   12
               Top             =   480
               Width           =   1332
            End
            Begin VB.ComboBox cmbThresh 
               BeginProperty Font 
                  Name            =   "MS Sans Serif"
                  Size            =   8.25
                  Charset         =   0
                  Weight          =   700
                  Underline       =   0   'False
                  Italic          =   0   'False
                  Strikethrough   =   0   'False
               EndProperty
               Height          =   315
               Left            =   840
               TabIndex        =   14
               Text            =   "Combo1"
               Top             =   840
               Width           =   852
            End
            Begin VB.TextBox txtLower 
               BackColor       =   &H00FFFFFF&
               BeginProperty Font 
                  Name            =   "MS Sans Serif"
                  Size            =   8.25
                  Charset         =   0
                  Weight          =   700
                  Underline       =   0   'False
                  Italic          =   0   'False
                  Strikethrough   =   0   'False
               EndProperty
               Height          =   288
               Left            =   840
               TabIndex        =   16
               Text            =   "LowerThres"
               Top             =   1320
               Width           =   852
            End
            Begin VB.TextBox txtUpper 
               BeginProperty Font 
                  Name            =   "MS Sans Serif"
                  Size            =   8.25
                  Charset         =   0
                  Weight          =   700
                  Underline       =   0   'False
                  Italic          =   0   'False
                  Strikethrough   =   0   'False
               EndProperty
               Height          =   288
               Left            =   840
               TabIndex        =   18
               Text            =   "UpperThres"
               Top             =   1680
               Width           =   852
            End
            Begin VB.Label lblUpper 
               Caption         =   "&Upper:"
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
               Left            =   120
               TabIndex        =   17
               Top             =   1680
               Width           =   732
            End
            Begin VB.Label lblLower 
               Caption         =   "Lo&wer:"
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
               Left            =   120
               TabIndex        =   15
               Top             =   1320
               Width           =   732
            End
            Begin VB.Label lblThreshnum 
               Caption         =   "&Number:"
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
               Left            =   120
               TabIndex        =   13
               Top             =   840
               Width           =   852
            End
         End
         Begin VB.Label lblThreshAvail 
            Caption         =   "A&vailable"
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
            Left            =   2280
            TabIndex        =   19
            Top             =   360
            Width           =   855
         End
         Begin VB.Label lblThreshAct 
            Caption         =   "Act&ive"
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
            Left            =   4440
            TabIndex        =   26
            Top             =   360
            Width           =   615
         End
      End
      Begin VB.Frame fraTab 
         BorderStyle     =   0  'None
         Caption         =   "Output Options"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   2415
         Index           =   2
         Left            =   -74640
         TabIndex        =   54
         Top             =   480
         Width           =   4932
         Begin VB.OptionButton optOutlev 
            Caption         =   "+ Fraction of events with duration '&n'"
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
            Index           =   7
            Left            =   240
            TabIndex        =   35
            Top             =   1920
            Width           =   4455
         End
         Begin VB.OptionButton optOutlev 
            Caption         =   "+ Standard devi&ation of events per level"
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
            Index           =   6
            Left            =   240
            TabIndex        =   34
            Top             =   1680
            Width           =   4455
         End
         Begin VB.OptionButton optOutlev 
            Caption         =   "+ Average d&uration of events at each level"
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
            Index           =   5
            Left            =   240
            TabIndex        =   33
            Top             =   1440
            Width           =   4455
         End
         Begin VB.OptionButton optOutlev 
            Caption         =   "+ Number of e&vents at each level"
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
            Index           =   4
            Left            =   240
            TabIndex        =   32
            Top             =   1200
            Width           =   4455
         End
         Begin VB.OptionButton optOutlev 
            Caption         =   "+ Time s&pent at each level"
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
            Index           =   3
            Left            =   240
            TabIndex        =   31
            Top             =   960
            Width           =   4455
         End
         Begin VB.OptionButton optOutlev 
            Caption         =   "+ Fraction of ti&me relative to time per level"
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
            Index           =   2
            Left            =   240
            TabIndex        =   30
            Top             =   720
            Width           =   4455
         End
         Begin VB.OptionButton optOutlev 
            Caption         =   "+ Fract&ion of time relative to total"
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
            Index           =   1
            Left            =   240
            TabIndex        =   29
            Top             =   480
            Width           =   4455
         End
         Begin VB.OptionButton optOutlev 
            Caption         =   "&Basic Table"
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
            Index           =   0
            Left            =   240
            TabIndex        =   28
            Top             =   240
            Value           =   -1  'True
            Width           =   1812
         End
      End
      Begin VB.Frame fraTab 
         BorderStyle     =   0  'None
         Caption         =   "Frame1"
         Height          =   2895
         Index           =   0
         Left            =   -74400
         TabIndex        =   55
         Top             =   480
         Width           =   4575
         Begin VB.ListBox lstDurations 
            BeginProperty Font 
               Name            =   "MS Sans Serif"
               Size            =   8.25
               Charset         =   0
               Weight          =   700
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   1230
            ItemData        =   "Gendur.frx":04E5
            Left            =   240
            List            =   "Gendur.frx":04EC
            TabIndex        =   3
            Top             =   480
            Width           =   1335
         End
         Begin VB.ListBox lstActDur 
            BeginProperty Font 
               Name            =   "MS Sans Serif"
               Size            =   8.25
               Charset         =   0
               Weight          =   700
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   1230
            ItemData        =   "Gendur.frx":04FE
            Left            =   2760
            List            =   "Gendur.frx":0505
            TabIndex        =   10
            Top             =   480
            Width           =   1332
         End
         Begin VB.CommandButton cmdDuradd 
            Caption         =   "--&>"
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
            Left            =   1920
            TabIndex        =   6
            Top             =   840
            Width           =   492
         End
         Begin VB.CommandButton cmdDurdrop 
            Caption         =   "&<--"
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
            Left            =   1920
            TabIndex        =   7
            Top             =   1200
            Width           =   492
         End
         Begin VB.TextBox txtUserdur 
            BeginProperty Font 
               Name            =   "MS Sans Serif"
               Size            =   8.25
               Charset         =   0
               Weight          =   700
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   288
            Left            =   240
            TabIndex        =   4
            Top             =   2280
            Width           =   1332
         End
         Begin VB.CommandButton cmdDuraddall 
            Caption         =   ">>"
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
            Left            =   1920
            TabIndex        =   5
            Top             =   480
            Width           =   492
         End
         Begin VB.CommandButton cmdDurdelall 
            Caption         =   "<<"
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
            Left            =   1920
            TabIndex        =   8
            Top             =   1560
            Width           =   492
         End
         Begin VB.Label Label1 
            Caption         =   "A&vailable"
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
            Left            =   480
            TabIndex        =   2
            Top             =   240
            Width           =   855
         End
         Begin VB.Label Label2 
            Caption         =   "Act&ive"
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
            Left            =   3120
            TabIndex        =   9
            Top             =   240
            Width           =   615
         End
      End
   End
   Begin MSComDlg.CommonDialog CDGet 
      Left            =   2160
      Top             =   4800
      _ExtentX        =   688
      _ExtentY        =   688
      _Version        =   393216
      FontSize        =   3.14179e-37
   End
   Begin MSComDlg.CommonDialog CDSave 
      Left            =   360
      Top             =   4800
      _ExtentX        =   688
      _ExtentY        =   688
      _Version        =   393216
      FontSize        =   3.14179e-37
   End
   Begin MSComDlg.CommonDialog CDOutfile 
      Left            =   240
      Top             =   4080
      _ExtentX        =   688
      _ExtentY        =   688
      _Version        =   393216
      FontSize        =   3.14179e-37
   End
End
Attribute VB_Name = "frmGenScnDuration"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
'Copyright 2000 by AQUA TERRA Consultants

Public oldlower$, oldupper$, savcol&, savrow&, outfile$

Private Const lethalMin = 0.0000001
Private Const lethalMax = 10000000
Private pTs As Collection

Public Sub DurInit()
  Dim i&
  SetTitle
  'set up duration list
  lstDurations.clear
  lstActDur.clear
  lstActThresh.clear
  lstDurations.AddItem 1
  lstDurations.AddItem 2
  lstDurations.AddItem 4
  lstDurations.AddItem 6
  lstDurations.AddItem 8
  lstDurations.AddItem 10
  lstDurations.AddItem 12
  lstDurations.AddItem 18
  lstDurations.AddItem 24
  optThres(0) = True
  optThres(1) = False
  txtLower = "1"
  txtUpper = "10000"
  cmbThresh.clear
  For i = 1 To 35
    cmbThresh.AddItem i
  Next i
  cmbThresh.ListIndex = 4
  optOutlev(0).value = True
  optOutlev(1).value = False
  optOutlev(2).value = False
  optOutlev(3).value = False
  optOutlev(4).value = False
  optOutlev(5).value = False
  optOutlev(6).value = False
  optOutlev(7).value = False
  'set initial lethality parameters
  optLethoffon(0) = True
  optLethoffon(1) = False
  optLethOut(0) = True
  cmbNoLethal.clear
  For i = 0 To 4
    cmbNoLethal.AddItem i + 1
  Next i
  cmbNoLethal.ListIndex = 0
  optGTLT(0) = True
  optGTLT(1) = False
  optLethOut(0).Enabled = False
  optLethOut(1).Enabled = False
  optLethOut(2).Enabled = False
  optLethOut(3).Enabled = False
  optGTLT(0).Enabled = False
  optGTLT(1).Enabled = False
  cmbNoLethal.Enabled = False
  grdLethal.Visible = False
  lblDurInt.Enabled = False
  fraLethOut.Enabled = False
  cmdAnalyze.Enabled = False
  grdLethal.Visible = False
  lblDurInt.Visible = True
End Sub

Public Sub SetThresDef()
  'set up threshold defaults
  lstThresholds.clear
  Dim i&, j&, rval!, inc!, diff!, InList As Boolean
  If optThres(0) = True Then
    'fill arithmetic
    If val(txtLower.text) = 1 Then
      'special case
      diff = val(txtUpper.text) - val(txtLower.text) + 1
      If cmbThresh.ListIndex <> 0 Then
        inc = diff / cmbThresh.ListIndex
      End If
      InList = False
      For i = 0 To lstActThresh.ListCount - 1
        'check to see if this number is already in active list
        If val(lstActThresh.List(i)) = val(txtLower.text) Then
          'already in list
          InList = True
        End If
      Next i
      If InList = False Then
        lstThresholds.AddItem txtLower.text
      End If
      rval = val(txtLower.text) - 1
      For i = 1 To cmbThresh.ListIndex
        rval = rval + inc
        InList = False
        For j = 0 To lstActThresh.ListCount - 1
          'check to see if this number is already in active list
          If val(lstActThresh.List(j)) = rval Then
            'already in list
            InList = True
          End If
        Next j
        If InList = False Then
          lstThresholds.AddItem rval
        End If
      Next i
    Else
      diff = val(txtUpper.text) - val(txtLower.text)
      If cmbThresh.ListIndex <> 0 Then
        inc = diff / cmbThresh.ListIndex
      End If
      InList = False
      For i = 0 To lstActThresh.ListCount - 1
        'check to see if this number is already in active list
        If val(lstActThresh.List(i)) = val(txtLower.text) Then
          'already in list
          InList = True
        End If
      Next i
      If InList = False Then
        lstThresholds.AddItem txtLower.text
      End If
      rval = val(txtLower.text)
      For i = 1 To cmbThresh.ListIndex
        rval = rval + inc
        InList = False
        For j = 0 To lstActThresh.ListCount - 1
          'check to see if this number is already in active list
          If val(lstActThresh.List(j)) = rval Then
            'already in list
            InList = True
          End If
        Next j
        If InList = False Then
          lstThresholds.AddItem rval
        End If
      Next i
    End If
  Else
    'fill logarithmic
    diff = Log10(val(txtUpper.text)) - Log10(val(txtLower.text))
    If cmbThresh.ListIndex <> 0 Then
      inc = diff / cmbThresh.ListIndex
    End If
    InList = False
    For i = 0 To lstActThresh.ListCount - 1
      'check to see if this number is already in active list
      If val(lstActThresh.List(i)) = val(txtLower.text) Then
        'already in list
        InList = True
      End If
    Next i
    If InList = False Then
      lstThresholds.AddItem txtLower.text
    End If
    rval = Log10(val(txtLower.text))
    For i = 1 To cmbThresh.ListIndex
      rval = rval + inc
      InList = False
      For j = 0 To lstActThresh.ListCount - 1
        'check to see if this number is already in active list
        If val(lstActThresh.List(j)) = 10 ^ rval Then
          'already in list
          InList = True
        End If
      Next j
      If InList = False Then
        lstThresholds.AddItem 10 ^ rval
      End If
    Next i
  End If
End Sub

Private Sub setGrdLethalColProperties()
  Dim c&
  With grdLethal
    For c = 1 To .cols - 1
      .ColEditable(c) = True
      .ColMax(c) = lethalMax
      .ColMin(c) = lethalMin
      .ColSelectable(c) = True
      .ColEditable(c) = True
      .ColType(c) = ATCoCtl.ATCoSng
    Next c
  End With
End Sub

Private Sub GetSpecs(Filename)
  Dim i&, r&, c&, ichr$, iint&, isng!
  On Error GoTo 10
  Open Filename For Input As #1
  lstDurations.clear
  lstActDur.clear
  lstThresholds.clear
  lstActThresh.clear
  Line Input #1, ichr
  iint = CLng(ichr)
  For i = 0 To iint - 1
    Line Input #1, ichr
    lstDurations.AddItem ichr
  Next i
  Line Input #1, ichr
  iint = CLng(ichr)
  For i = 0 To iint - 1
    Line Input #1, ichr
    lstActDur.AddItem ichr
  Next i
  Line Input #1, ichr
  iint = CLng(ichr)
  For i = 0 To iint - 1
    Line Input #1, ichr
    lstThresholds.AddItem ichr
  Next i
  Line Input #1, ichr
  iint = CLng(ichr)
  For i = 0 To iint - 1
    Line Input #1, ichr
    lstActThresh.AddItem ichr
  Next i
  cmbThresh.clear
  For i = 1 To 35
    cmbThresh.AddItem i
  Next i
  Line Input #1, ichr:  txtLower = ichr
  Line Input #1, ichr:  txtUpper = ichr
  Line Input #1, ichr:  optThres(0) = ichr
  Line Input #1, ichr:  optThres(1) = ichr
  Line Input #1, ichr:  cmbThresh.ListIndex = CLng(ichr)
  Line Input #1, ichr:  optOutlev(0).value = CLng(ichr)
  Line Input #1, ichr:  optOutlev(1).value = CLng(ichr)
  Line Input #1, ichr:  optOutlev(2).value = CLng(ichr)
  Line Input #1, ichr:  optOutlev(3).value = CLng(ichr)
  Line Input #1, ichr:  optOutlev(4).value = CLng(ichr)
  Line Input #1, ichr:  optOutlev(5).value = CLng(ichr)
  Line Input #1, ichr:  optOutlev(6).value = CLng(ichr)
  Line Input #1, ichr:  optOutlev(7).value = CLng(ichr)
  Line Input #1, ichr:  optLethoffon(0) = CLng(ichr)
  Line Input #1, ichr:  optLethoffon(1) = CLng(ichr)
  Line Input #1, ichr:  optLethOut(0) = CLng(ichr)
  Line Input #1, ichr:  optLethOut(1) = CLng(ichr)
  Line Input #1, ichr:  optLethOut(2) = CLng(ichr)
  Line Input #1, ichr:  optLethOut(3) = CLng(ichr)
  cmbNoLethal.clear
  For i = 0 To 4
    cmbNoLethal.AddItem i + 1
  Next i
  Line Input #1, ichr:  cmbNoLethal.ListIndex = CLng(ichr)
  Line Input #1, ichr:  optGTLT(0) = CLng(ichr)
  Line Input #1, ichr:  optGTLT(1) = CLng(ichr)
  Line Input #1, ichr:  grdLethal.Rows = CLng(ichr)
  Line Input #1, ichr:  grdLethal.cols = CLng(ichr)
  setGrdLethalColProperties
  
  If grdLethal.Rows > 0 And grdLethal.cols > 0 Then
    For r = 0 To grdLethal.Rows
      For c = 0 To grdLethal.cols - 1
        Line Input #1, ichr
        grdLethal.TextMatrix(r, c) = ichr
      Next c
    Next r
  End If
  If lstActDur.ListCount > 0 And lstActThresh.ListCount > 0 Then
    cmdAnalyze.Enabled = True
  Else
    cmdAnalyze.Enabled = False
  End If
  Close #1
  GoTo 20
10  Close #1
    Call DurInit
20  'go here if everything read okay
End Sub

Private Sub PutSpecs(Filename)
  Dim i&, r&, c&
  Open Filename For Output As #1
  Print #1, lstDurations.ListCount
  For i = 0 To lstDurations.ListCount - 1
    Print #1, lstDurations.List(i)
  Next i
  Print #1, lstActDur.ListCount
  For i = 0 To lstActDur.ListCount - 1
    Print #1, lstActDur.List(i)
  Next i
  Print #1, lstThresholds.ListCount
  For i = 0 To lstThresholds.ListCount - 1
    Print #1, lstThresholds.List(i)
  Next i
  Print #1, lstActThresh.ListCount
  For i = 0 To lstActThresh.ListCount - 1
    Print #1, lstActThresh.List(i)
  Next i
  Print #1, txtLower
  Print #1, txtUpper
  Print #1, optThres(0)
  Print #1, optThres(1)
  Print #1, cmbThresh.ListIndex
  Print #1, optOutlev(0).value
  Print #1, optOutlev(1).value
  Print #1, optOutlev(2).value
  Print #1, optOutlev(3).value
  Print #1, optOutlev(4).value
  Print #1, optOutlev(5).value
  Print #1, optOutlev(6).value
  Print #1, optOutlev(7).value
  Print #1, optLethoffon(0)
  Print #1, optLethoffon(1)
  Print #1, optLethOut(0)
  Print #1, optLethOut(1)
  Print #1, optLethOut(2)
  Print #1, optLethOut(3)
  Print #1, cmbNoLethal.ListIndex
  Print #1, optGTLT(0)
  Print #1, optGTLT(1)
  Print #1, grdLethal.Rows
  Print #1, grdLethal.cols
  If grdLethal.Rows > 0 And grdLethal.cols > 0 Then
    For r = 0 To grdLethal.Rows
      For c = 0 To grdLethal.cols - 1
        Print #1, grdLethal.TextMatrix(r, c)
      Next c
    Next r
  End If
  Close #1
End Sub

Public Sub SetLethalGrid()
  'set up grid
  Dim r&, c&
  grdLethal.Rows = lstActDur.ListCount
  For r = 1 To grdLethal.Rows
    grdLethal.TextMatrix(r, 0) = lstActDur.List(r - 1)
  Next r
  grdLethal.cols = 1 + cmbNoLethal
  setGrdLethalColProperties
  For c = 1 To grdLethal.cols - 1
    grdLethal.ColTitle(c) = "Curve " & c
  Next c
  grdLethal.row = 0
  grdLethal.col = 0
  grdLethal.TextMatrix(0, 0) = "Duration"
  grdLethal.Visible = True
End Sub

Private Sub cmbNoLethal_Click()
  Call SetLethalGrid
End Sub

Private Sub cmbThresh_Click()
    Call SetThresDef
End Sub

Private Sub cmdAnalyze_Click()
  Dim i&, j&, mxlev&, mxdur&, mxlc&
  mxlev = 35
  mxdur = 10
  mxlc = 5
  Dim dinfo$, ncint&, rclint!(35)
  Dim ndura&, durat&(10)
  Dim ncrit&, rlclev!(5, 35), NVALS&, rvals!()
  Dim SDate&(6), EDate&(6), ssdate&(2), sedate&(2), Tu&, ts&, Dtran&
  Dim prfg&, lcout&, lcgtlt&
  Dim num&, frqnw!(10), snw!(10), sqnw!(10)
  Dim frqpos!(10, 35), spos!(10, 35), sqpos!(10, 35)
  Dim frqneg!(10, 35), sneg!(10, 35), sqneg!(10, 35)
  Dim mnw!(10), mpos!(10, 35), mneg!(10, 35)
  Dim ptnw!(10), ptpos!(10, 35), ptneg!(10, 35)
  Dim pt1nw!(10), pt1pos!(10, 35), pt1neg!(10, 35)
  Dim frevnw!(10), frevps!(10, 35), frevng!(10, 35)
  Dim delt!, Max!, minim!, mean!, sumsq!, lctsto&(5)

  dinfo = txtTitle.text
  ndura = lstActDur.ListCount
  If ndura = 0 Then
    'problem, no durations defined
    MsgBox "No Durations have been Set", vbExclamation, "GenScn Duration Compute Problem"
  Else
    If ndura > 9 Then
      'force to be no greater than 9 since that's all hspf can handle
      ndura = 9
    End If
    For i = 1 To ndura
      durat(i - 1) = CInt(lstActDur.List(i - 1))
    Next i
    ncint = lstActThresh.ListCount
    If ncint = 0 Then
      'problem, no thresholds defined
      MsgBox "No Thresholds have been Set", vbExclamation, "GenScn Duration Compute Problem"
    Else
      If ncint > 35 Then
        ncint = 35
      End If
      For i = 1 To ncint
        rclint(i - 1) = val(lstActThresh.List(i - 1))
      Next i
      If optLethoffon(0) = True Then
        'lethality off
        ncrit = 0
      Else
        'lethality on
        ncrit = cmbNoLethal
        For i = 1 To ndura
          For j = 1 To ncrit
            grdLethal.row = i
            grdLethal.col = j
            If Len(grdLethal.text) > 0 Then
              rlclev(j - 1, i - 1) = val(grdLethal.text)
            Else
              rlclev(j - 1, i - 1) = 0
            End If
          Next j
        Next i
      End If
      SDate(0) = CSDat(0)
      SDate(1) = CSDat(1)
      SDate(2) = CSDat(2)
      SDate(3) = 0
      SDate(4) = 0
      SDate(5) = 0
      ssdate(0) = SDate(1)
      ssdate(1) = SDate(2)
      EDate(0) = CEDat(0)
      EDate(1) = CEDat(1)
      EDate(2) = CEDat(2)
      EDate(3) = 24
      EDate(4) = 0
      EDate(5) = 0
      sedate(0) = EDate(1)
      sedate(1) = EDate(2)
      Set pTs = GenScnEntry.FillTimSer(frmGenScn.TSerSelected)
      With pTs(1).dates.Summary
        Tu = .Tu
        ts = .ts
        Dtran = 1
        NVALS = .NVALS
      End With
      If NVALS > 30000 Then
        'problem, too many data points
        MsgBox "The specified timeseries contains too many data points." & vbCrLf & _
               "The maximum number of data points is 30,000.", vbExclamation, "GenScn Duration Compute Problem"
      Else
        ReDim rvals(NVALS)
        With pTs(1)
          For i = 1 To NVALS
            rvals(i) = .value(i)
          Next i
        End With
        'set duration analysis data values
        Call F90_DAANST(NVALS, rvals(1))
        For i = 0 To 7
          If optOutlev(i) = True Then
            prfg = i
          End If
        Next i
        For i = 0 To 3
          If optLethOut(i) = True Then
            lcout = i
          End If
        Next i
        If optGTLT(0) = True Then
          lcgtlt = 1
        Else
          lcgtlt = 2
        End If
        'set output file
        outfile = CDOutfile.Filename
        If Len(outfile) = 0 Then
          outfile = "durani.out"
        End If
        'all set to do duration analysis
        Call F90_DAANWV(mxlev, ncint, rclint(0), mxdur, ndura, durat(0), _
                        mxlc, ncrit, rlclev(0, 0), SDate(0), EDate(0), ssdate(0), _
                        sedate(0), Tu, ts, NVALS, prfg, _
                        lcout, lcgtlt, num, frqnw(0), snw(0), sqnw(0), _
                        frqpos(0, 0), spos(0, 0), sqpos(0, 0), frqneg(0, 0), sneg(0, 0), sqneg(0, 0), _
                        mnw(0), mpos(0, 0), mneg(0, 0), ptnw(0), ptpos(0, 0), ptneg(0, 0), _
                        pt1nw(0), pt1pos(0, 0), pt1neg(0, 0), frevnw(0), frevps(0, 0), frevng(0, 0), _
                        delt, Max, minim, mean, sumsq, lctsto(0), dinfo, outfile, Len(dinfo), Len(outfile))
        If Len(outfile) > 0 Then
          cmdResults.Enabled = True
        Else
          cmdResults.Enabled = False
        End If
      End If
    End If
  End If
End Sub

Private Sub cmdClear_Click()
  Call DurInit
End Sub

Private Sub cmdClose_Click()
  Dim defname$
  defname = p.StatusFilePath & "\default.gdu"
  Call PutSpecs(defname)
  Unload frmGenScnDuration
End Sub


Private Sub cmdDuradd_Click()
  Dim i&, ipos&, newdur$, ival&, chgflg&
  newdur = ""
  If lstDurations.ListIndex >= 0 Then
    newdur = lstDurations.List(lstDurations.ListIndex)
  Else
    If Len(txtUserdur.text) > 0 Then
      newdur = txtUserdur.text
      Call ChkTxtI("Duration", 1, 1000, newdur, ival, chgflg)
      txtUserdur.text = ""
      If chgflg <> 1 Then
        newdur = ""
      Else
        'make sure not one of the above choices
        i = 0
        Do Until i = lstDurations.ListCount
          If CInt(lstDurations.List(i)) = CInt(newdur) Then
            'already in list, remove
            lstDurations.RemoveItem i
            i = lstDurations.ListCount
          Else
            i = i + 1
          End If
        Loop
        i = 0
        Do Until i = lstActDur.ListCount
          If CInt(lstActDur.List(i)) = CInt(newdur) Then
            'already in list, remove
            lstActDur.RemoveItem i
            i = lstActDur.ListCount
          Else
            i = i + 1
          End If
        Loop
      End If
    End If
  End If
  If Len(newdur) > 0 Then
    'add this duration to active list, figure where
    If lstActDur.ListCount < 1 Then
      ipos = 0
    Else
      'find where to put it in list
      ipos = -1
      i = 0
      Do Until ipos > -1 Or i = lstActDur.ListCount
        If CInt(lstActDur.List(i)) > CInt(newdur) Then
          ipos = i
        End If
        i = i + 1
      Loop
      If ipos = -1 Then
        ipos = lstActDur.ListCount
      End If
    End If
    lstActDur.AddItem newdur, ipos
  End If
  If lstDurations.ListIndex >= 0 Then
    lstDurations.RemoveItem lstDurations.ListIndex
  End If
  If lstActDur.ListCount > 0 And lstActThresh.ListCount > 0 Then
    cmdAnalyze.Enabled = True
  Else
    cmdAnalyze.Enabled = False
  End If
End Sub

Private Sub cmdDuraddall_Click()
  Dim newdur$, ipos&, i&
  Do Until lstDurations.ListCount = 0
    newdur = lstDurations.List(0)
    'add this duration to active list, figure where
    If lstActDur.ListCount < 1 Then
      ipos = 0
    Else
      'find where to put it in list
      ipos = -1
      i = 0
      Do Until ipos > -1 Or i = lstActDur.ListCount
        If CInt(lstActDur.List(i)) > CInt(newdur) Then
          ipos = i
        End If
        i = i + 1
      Loop
      If ipos = -1 Then
        ipos = lstActDur.ListCount
      End If
    End If
    lstActDur.AddItem newdur, ipos
    lstDurations.RemoveItem 0
  Loop
  If lstActDur.ListCount > 0 And lstActThresh.ListCount > 0 Then
    cmdAnalyze.Enabled = True
  Else
    cmdAnalyze.Enabled = False
  End If
End Sub


Private Sub cmdDurdelall_Click()
  Dim newdur$, ipos&, i&
  Do Until lstActDur.ListCount = 0
    newdur = lstActDur.List(0)
    'add this duration to available list, figure where
    If lstDurations.ListCount < 1 Then
      ipos = 0
    Else
      'find where to put it in list
      ipos = -1
      i = 0
      Do Until ipos > -1 Or i = lstDurations.ListCount
        If CInt(lstDurations.List(i)) > CInt(newdur) Then
          ipos = i
        End If
        i = i + 1
      Loop
      If ipos = -1 Then
        ipos = lstDurations.ListCount
      End If
    End If
    lstDurations.AddItem newdur, ipos
    lstActDur.RemoveItem 0
  Loop
  If lstActDur.ListCount > 0 And lstActThresh.ListCount > 0 Then
    cmdAnalyze.Enabled = True
  Else
    cmdAnalyze.Enabled = False
  End If
End Sub


Private Sub cmdDurdrop_Click()
  Dim newdur$, ipos&, i&
  If lstActDur.ListIndex > -1 Then
    'something is selected
    If lstDurations.ListCount < 1 Then
      'put this duration back to available list
      ipos = 0
    Else
      'figure out where to put it
      ipos = -1
      i = 0
      Do Until ipos > -1 Or i = lstDurations.ListCount
        If CInt(lstDurations.List(i)) > CInt(lstActDur.List(lstActDur.ListIndex)) Then
          ipos = i
        End If
        i = i + 1
      Loop
      If ipos = -1 Then
        ipos = lstDurations.ListCount
      End If
    End If
    lstDurations.AddItem lstActDur.List(lstActDur.ListIndex), ipos
    lstActDur.RemoveItem lstActDur.ListIndex
  End If
  If lstActDur.ListCount > 0 And lstActThresh.ListCount > 0 Then
    cmdAnalyze.Enabled = True
  Else
    cmdAnalyze.Enabled = False
  End If
End Sub



Private Sub cmdGet_Click()
    CDGet.flags = &H1806&
    CDGet.filter = "GenScn Duration Files (*.gdu)|*.gdu"
    CDGet.Filename = "*.gdu"
    CDGet.DialogTitle = "GenScn Duration Get Specs File"
    On Error GoTo 10
    CDGet.CancelError = True
    CDGet.Action = 1
    Call GetSpecs(CDGet.Filename)
10 'continue here on cancel
End Sub

Private Sub cmdOutfile_Click()
  CDOutfile.flags = &H8806&
  CDOutfile.Filename = "duranl.out"
  CDOutfile.DialogTitle = "GenScn Duration Output File"
  CDOutfile.Action = 2
End Sub

Private Sub cmdResults_Click()
  Dim cap$
  cap = "GenScn Duration Results"
  Call DispFile.OpenFile(outfile, cap, frmGenScnDuration.Icon, False)
End Sub

Private Sub cmdSave_Click()
    CDSave.flags = &H8806&
    CDSave.filter = "GenScn Duration Files (*.gdu)|*.gdu"
    CDSave.Filename = "*.gdu"
    CDSave.DialogTitle = "GenScn Duration Save Specs File"
    On Error GoTo 10
    CDSave.CancelError = True
    CDSave.Action = 2
    Call PutSpecs(CDSave.Filename)
10 'continue here on cancel
End Sub

Private Sub cmdThreshadd_Click()
  Dim i&, ipos&, newthresh$, rval!, chgflg&
  newthresh = ""
  If lstThresholds.ListIndex >= 0 Then
    newthresh = lstThresholds.List(lstThresholds.ListIndex)
  Else
    If Len(txtThresholds.text) > 0 Then
      newthresh = txtThresholds.text
      Call ChkTxtR("Thresholds", 0.000001, 10000000, newthresh, rval, chgflg)
      txtThresholds.text = ""
      If chgflg <> 1 Then
        newthresh = ""
      Else
        'make sure not one of the above choices
        i = 0
        Do Until i = lstThresholds.ListCount
          If val(lstThresholds.List(i)) = val(newthresh) Then
            'already in list, remove
            lstThresholds.RemoveItem i
            i = lstThresholds.ListCount
          Else
            i = i + 1
          End If
        Loop
        i = 0
        Do Until i = lstActThresh.ListCount
          If val(lstActThresh.List(i)) = val(newthresh) Then
            'already in list, remove
            lstActThresh.RemoveItem i
            i = lstActThresh.ListCount
          Else
            i = i + 1
          End If
        Loop
      End If
    End If
  End If
  If Len(newthresh) > 0 Then
    'make sure not already in active list
    i = 0
    Do Until i = lstActThresh.ListCount
      If val(lstActThresh.List(i)) = val(newthresh) Then
        'already in list, remove
        lstActThresh.RemoveItem i
        i = lstActThresh.ListCount
      Else
        i = i + 1
      End If
    Loop
    'add this threshold to active list, figure where
    If lstActThresh.ListCount < 1 Then
      ipos = 0
    Else
      'find where to put it in list
      ipos = -1
      i = 0
      Do Until ipos > -1 Or i = lstActThresh.ListCount
        If val(lstActThresh.List(i)) > val(newthresh) Then
          ipos = i
        End If
        i = i + 1
      Loop
      If ipos = -1 Then
        ipos = lstActThresh.ListCount
      End If
    End If
    lstActThresh.AddItem newthresh, ipos
  End If
  If lstThresholds.ListIndex >= 0 Then
    lstThresholds.RemoveItem lstThresholds.ListIndex
  End If
  If lstActDur.ListCount > 0 And lstActThresh.ListCount > 0 Then
    cmdAnalyze.Enabled = True
  Else
    cmdAnalyze.Enabled = False
  End If
End Sub

Private Sub cmdThreshaddall_Click()
  Dim newthresh$, ipos&, i&
  Do Until lstThresholds.ListCount = 0
    newthresh = lstThresholds.List(0)
    'add this threshold to active list, figure where
    If lstActThresh.ListCount < 1 Then
      ipos = 0
    Else
      'make sure not one of the above choices
      i = 0
      Do Until i = lstActThresh.ListCount
        If val(lstActThresh.List(i)) = val(newthresh) Then
          'already in list, remove
          lstActThresh.RemoveItem i
          i = lstActThresh.ListCount
        Else
          i = i + 1
        End If
      Loop
      'find where to put it in list
      ipos = -1
      i = 0
      Do Until ipos > -1 Or i = lstActThresh.ListCount
        If val(lstActThresh.List(i)) > val(newthresh) Then
          ipos = i
        End If
        i = i + 1
      Loop
      If ipos = -1 Then
        ipos = lstActThresh.ListCount
      End If
    End If
    lstActThresh.AddItem newthresh, ipos
    lstThresholds.RemoveItem 0
  Loop
  If lstActDur.ListCount > 0 And lstActThresh.ListCount > 0 Then
    cmdAnalyze.Enabled = True
  Else
    cmdAnalyze.Enabled = False
  End If
End Sub

Private Sub cmdThreshdelall_Click()
  Dim newthresh$, ipos&, i&
  Do Until lstActThresh.ListCount = 0
    newthresh = lstActThresh.List(0)
    'add this threshold to available list, figure where
    If lstThresholds.ListCount < 1 Then
      ipos = 0
    Else
      'find where to put it in list
      ipos = -1
      i = 0
      Do Until ipos > -1 Or i = lstThresholds.ListCount
        If val(lstThresholds.List(i)) > val(newthresh) Then
          ipos = i
        End If
        i = i + 1
      Loop
      If ipos = -1 Then
        ipos = lstThresholds.ListCount
      End If
    End If
    lstThresholds.AddItem newthresh, ipos
    lstActThresh.RemoveItem 0
  Loop
  If lstActDur.ListCount > 0 And lstActThresh.ListCount > 0 Then
    cmdAnalyze.Enabled = True
  Else
    cmdAnalyze.Enabled = False
  End If
End Sub

Private Sub cmdThreshdrop_Click()
  Dim ipos&, i&
  If lstActThresh.ListIndex > -1 Then
    'something is selected
    If lstThresholds.ListCount < 1 Then
      'put this threshold back to available list
      ipos = 0
    Else
      'figure out where to put it
      ipos = -1
      i = 0
      Do Until ipos > -1 Or i = lstThresholds.ListCount
        If val(lstThresholds.List(i)) > val(lstActThresh.List(lstActThresh.ListIndex)) Then
          ipos = i
        End If
        i = i + 1
      Loop
      If ipos = -1 Then
        ipos = lstThresholds.ListCount
      End If
    End If
    lstThresholds.AddItem lstActThresh.List(lstActThresh.ListIndex), ipos
    lstActThresh.RemoveItem lstActThresh.ListIndex
  End If
  If lstActDur.ListCount > 0 And lstActThresh.ListCount > 0 Then
    cmdAnalyze.Enabled = True
  Else
    cmdAnalyze.Enabled = False
  End If
End Sub

Private Sub SetTitle()
  If Not pTs Is Nothing Then
    If pTs.Count > 0 Then
      txtTitle.text = "Duration Analysis for " & pTs(1).Header.sen & " " & _
                                                 pTs(1).Header.con & " at " & _
                                                 pTs(1).Header.loc
    End If
  End If
End Sub

Private Sub Form_Load()
  Dim defname$
  cmdResults.Enabled = False
  defname = p.StatusFilePath & "\default.gdu"
  Set pTs = GenScnEntry.FillTimSer(frmGenScn.TSerSelected)
  SetTitle
  Call GetSpecs(defname)
  SSTab1.Tab = 0
  SSTab1_Click 0
End Sub

Private Sub optlethoffon_Click(index As Integer)
  If lstActDur.ListCount = 0 Then
    grdLethal.Visible = False
    lblDurInt.Visible = True
  Else
    If optLethoffon(0) = True Then
      grdLethal.Visible = False
      lblDurInt.Visible = False
    Else
      Call SetLethalGrid
      grdLethal.Visible = True
    End If
  End If
  If optLethoffon(0) = True Then
    optLethOut(0).Enabled = False
    optLethOut(1).Enabled = False
    optLethOut(2).Enabled = False
    optLethOut(3).Enabled = False
    optGTLT(0).Enabled = False
    optGTLT(1).Enabled = False
    cmbNoLethal.Enabled = False
    grdLethal.Visible = False
    lblDurInt.Enabled = False
    fraLethOut.Enabled = False
    fraGTLT.Enabled = False
  Else
    optLethOut(0).Enabled = True
    optLethOut(1).Enabled = True
    optLethOut(2).Enabled = True
    optLethOut(3).Enabled = True
    optGTLT(0).Enabled = True
    optGTLT(1).Enabled = True
    cmbNoLethal.Enabled = True
    grdLethal.Visible = True
    lblDurInt.Enabled = True
    fraLethOut.Enabled = True
    fraGTLT.Enabled = True
  End If
End Sub

Private Sub optThres_Click(index As Integer)
    Call SetThresDef
End Sub

Private Sub SSTab1_Click(PreviousTab As Integer)
  Dim t&
  For t = 0 To 3
    If t = SSTab1.Tab Then fraTab(t).Visible = True Else fraTab(t).Visible = False
  Next t
  
  If SSTab1.Tab = 3 Then
    If lstActDur.ListCount = 0 Then
      grdLethal.Visible = False
      lblDurInt.Visible = True
    Else
      If optLethoffon(0) = True Then
        grdLethal.Visible = False
        lblDurInt.Visible = False
      Else
        'set up grid
        Call SetLethalGrid
      End If
    End If
  End If
End Sub

Private Sub txtLower_GotFocus()
  oldlower = txtLower.text
End Sub

Private Sub txtLower_LostFocus()
  Dim newlower$, chgflg&, rval!
  newlower = txtLower.text
  Call ChkTxtR("Thresholds", 0.000001, 10000000, newlower, rval, chgflg)
  If chgflg <> 1 Then
    txtLower.text = oldlower
  End If
  If chgflg = 1 Then
    Call SetThresDef
  End If
End Sub


Private Sub txtThresholds_Click()
  lstThresholds.ListIndex = -1
  lstActThresh.ListIndex = -1
End Sub


Private Sub txtUpper_GotFocus()
  oldupper = txtUpper.text
End Sub

Private Sub txtUpper_LostFocus()
  Dim newupper$, chgflg&, rval!
  newupper = txtUpper.text
  Call ChkTxtR("Thresholds", 0.000001, 10000000, newupper, rval, chgflg)
  If chgflg <> 1 Then
    txtUpper.text = oldupper
  End If
  If chgflg = 1 Then
    Call SetThresDef
  End If
End Sub


Private Sub txtUserdur_Click()
  lstDurations.ListIndex = -1
  lstActDur.ListIndex = -1
End Sub
