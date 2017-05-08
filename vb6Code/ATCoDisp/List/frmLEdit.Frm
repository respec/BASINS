VERSION 5.00
Begin VB.Form frmLEdit 
   BorderStyle     =   1  'Fixed Single
   Caption         =   "List Edit"
   ClientHeight    =   4272
   ClientLeft      =   1056
   ClientTop       =   1548
   ClientWidth     =   4692
   Icon            =   "frmLEdit.frx":0000
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   PaletteMode     =   1  'UseZOrder
   ScaleHeight     =   4272
   ScaleWidth      =   4692
   WhatsThisButton =   -1  'True
   WhatsThisHelp   =   -1  'True
   Begin VB.CommandButton cmdEdList 
      Cancel          =   -1  'True
      Caption         =   "Cancel"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   7.8
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   372
      Index           =   1
      Left            =   3480
      TabIndex        =   49
      Top             =   3720
      Width           =   972
   End
   Begin VB.CommandButton cmdEdList 
      Caption         =   "&OK"
      Default         =   -1  'True
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   7.8
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   372
      Index           =   0
      Left            =   2280
      TabIndex        =   48
      Top             =   3720
      Width           =   972
   End
   Begin TabDlg.SSTab sstEd 
      Height          =   3495
      Left            =   120
      TabIndex        =   0
      Top             =   120
      Width           =   4455
      _ExtentX        =   7853
      _ExtentY        =   6160
      _Version        =   393216
      Tabs            =   4
      Tab             =   1
      TabsPerRow      =   4
      TabHeight       =   423
      BeginProperty Font {0BE35203-8F91-11CE-9DE3-00AA004BB851} 
         Name            =   "MS Sans Serif"
         Size            =   7.8
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      TabCaption(0)   =   "&General"
      TabPicture(0)   =   "frmLEdit.frx":0442
      Tab(0).ControlEnabled=   0   'False
      Tab(0).Control(0)=   "chkRange(2)"
      Tab(0).Control(0).Enabled=   0   'False
      Tab(0).Control(1)=   "chkRange(1)"
      Tab(0).Control(1).Enabled=   0   'False
      Tab(0).Control(2)=   "chkRange(0)"
      Tab(0).Control(2).Enabled=   0   'False
      Tab(0).Control(3)=   "txtTitle(2)"
      Tab(0).Control(3).Enabled=   0   'False
      Tab(0).Control(4)=   "txtTitle(1)"
      Tab(0).Control(4).Enabled=   0   'False
      Tab(0).Control(5)=   "txtTitle(0)"
      Tab(0).Control(5).Enabled=   0   'False
      Tab(0).Control(6)=   "Label6"
      Tab(0).Control(6).Enabled=   0   'False
      Tab(0).Control(7)=   "lblTitle"
      Tab(0).Control(7).Enabled=   0   'False
      Tab(0).ControlCount=   8
      TabCaption(1)   =   "&Fields"
      TabPicture(1)   =   "frmLEdit.frx":045E
      Tab(1).ControlEnabled=   -1  'True
      Tab(1).Control(0)=   "lblField"
      Tab(1).Control(0).Enabled=   0   'False
      Tab(1).Control(1)=   "lblFldNum"
      Tab(1).Control(1).Enabled=   0   'False
      Tab(1).Control(2)=   "lblLabel"
      Tab(1).Control(2).Enabled=   0   'False
      Tab(1).Control(3)=   "lblWidth"
      Tab(1).Control(3).Enabled=   0   'False
      Tab(1).Control(4)=   "Label1"
      Tab(1).Control(4).Enabled=   0   'False
      Tab(1).Control(5)=   "Label2"
      Tab(1).Control(5).Enabled=   0   'False
      Tab(1).Control(6)=   "Label3"
      Tab(1).Control(6).Enabled=   0   'False
      Tab(1).Control(7)=   "Label5"
      Tab(1).Control(7).Enabled=   0   'False
      Tab(1).Control(8)=   "txtWid"
      Tab(1).Control(8).Enabled=   0   'False
      Tab(1).Control(9)=   "txtfMax"
      Tab(1).Control(9).Enabled=   0   'False
      Tab(1).Control(10)=   "txtfMin"
      Tab(1).Control(10).Enabled=   0   'False
      Tab(1).Control(11)=   "txtDpl"
      Tab(1).Control(11).Enabled=   0   'False
      Tab(1).Control(12)=   "txtSdg"
      Tab(1).Control(12).Enabled=   0   'False
      Tab(1).Control(13)=   "cmdUpDn(0)"
      Tab(1).Control(13).Enabled=   0   'False
      Tab(1).Control(14)=   "cmdUpDn(1)"
      Tab(1).Control(14).Enabled=   0   'False
      Tab(1).Control(15)=   "txtLabel"
      Tab(1).Control(15).Enabled=   0   'False
      Tab(1).Control(16)=   "optListScreen(0)"
      Tab(1).Control(16).Enabled=   0   'False
      Tab(1).Control(17)=   "optListScreen(1)"
      Tab(1).Control(17).Enabled=   0   'False
      Tab(1).ControlCount=   18
      TabCaption(2)   =   "&Summaries"
      TabPicture(2)   =   "frmLEdit.frx":047A
      Tab(2).ControlEnabled=   0   'False
      Tab(2).Control(0)=   "lstEndMon"
      Tab(2).Control(0).Enabled=   0   'False
      Tab(2).Control(1)=   "chkCnt(1)"
      Tab(2).Control(1).Enabled=   0   'False
      Tab(2).Control(2)=   "chkCnt(2)"
      Tab(2).Control(2).Enabled=   0   'False
      Tab(2).Control(3)=   "chkCnt(3)"
      Tab(2).Control(3).Enabled=   0   'False
      Tab(2).Control(4)=   "chkCnt(4)"
      Tab(2).Control(4).Enabled=   0   'False
      Tab(2).Control(5)=   "chkCnt(5)"
      Tab(2).Control(5).Enabled=   0   'False
      Tab(2).Control(6)=   "chkCnt(6)"
      Tab(2).Control(6).Enabled=   0   'False
      Tab(2).Control(7)=   "chkCnt(7)"
      Tab(2).Control(7).Enabled=   0   'False
      Tab(2).Control(8)=   "chkMax(1)"
      Tab(2).Control(8).Enabled=   0   'False
      Tab(2).Control(9)=   "chkMin(1)"
      Tab(2).Control(9).Enabled=   0   'False
      Tab(2).Control(10)=   "chkSum(1)"
      Tab(2).Control(10).Enabled=   0   'False
      Tab(2).Control(11)=   "chkAve(1)"
      Tab(2).Control(11).Enabled=   0   'False
      Tab(2).Control(12)=   "chkMax(2)"
      Tab(2).Control(12).Enabled=   0   'False
      Tab(2).Control(13)=   "chkMax(3)"
      Tab(2).Control(13).Enabled=   0   'False
      Tab(2).Control(14)=   "chkAve(2)"
      Tab(2).Control(14).Enabled=   0   'False
      Tab(2).Control(15)=   "chkMin(2)"
      Tab(2).Control(15).Enabled=   0   'False
      Tab(2).Control(16)=   "chkSum(2)"
      Tab(2).Control(16).Enabled=   0   'False
      Tab(2).Control(17)=   "chkMin(3)"
      Tab(2).Control(17).Enabled=   0   'False
      Tab(2).Control(18)=   "chkSum(3)"
      Tab(2).Control(18).Enabled=   0   'False
      Tab(2).Control(19)=   "chkAve(3)"
      Tab(2).Control(19).Enabled=   0   'False
      Tab(2).Control(20)=   "chkMax(4)"
      Tab(2).Control(20).Enabled=   0   'False
      Tab(2).Control(21)=   "chkMin(4)"
      Tab(2).Control(21).Enabled=   0   'False
      Tab(2).Control(22)=   "chkSum(4)"
      Tab(2).Control(22).Enabled=   0   'False
      Tab(2).Control(23)=   "chkAve(4)"
      Tab(2).Control(23).Enabled=   0   'False
      Tab(2).Control(24)=   "chkMax(5)"
      Tab(2).Control(24).Enabled=   0   'False
      Tab(2).Control(25)=   "chkMin(5)"
      Tab(2).Control(25).Enabled=   0   'False
      Tab(2).Control(26)=   "chkSum(5)"
      Tab(2).Control(26).Enabled=   0   'False
      Tab(2).Control(27)=   "chkAve(5)"
      Tab(2).Control(27).Enabled=   0   'False
      Tab(2).Control(28)=   "chkMax(6)"
      Tab(2).Control(28).Enabled=   0   'False
      Tab(2).Control(29)=   "chkMin(6)"
      Tab(2).Control(29).Enabled=   0   'False
      Tab(2).Control(30)=   "chkSum(6)"
      Tab(2).Control(30).Enabled=   0   'False
      Tab(2).Control(31)=   "chkAve(6)"
      Tab(2).Control(31).Enabled=   0   'False
      Tab(2).Control(32)=   "chkMax(7)"
      Tab(2).Control(32).Enabled=   0   'False
      Tab(2).Control(33)=   "chkMin(7)"
      Tab(2).Control(33).Enabled=   0   'False
      Tab(2).Control(34)=   "chkSum(7)"
      Tab(2).Control(34).Enabled=   0   'False
      Tab(2).Control(35)=   "chkAve(7)"
      Tab(2).Control(35).Enabled=   0   'False
      Tab(2).Control(36)=   "lblSumHdr(4)"
      Tab(2).Control(36).Enabled=   0   'False
      Tab(2).Control(37)=   "lblSumHdr(3)"
      Tab(2).Control(37).Enabled=   0   'False
      Tab(2).Control(38)=   "lblSumHdr(2)"
      Tab(2).Control(38).Enabled=   0   'False
      Tab(2).Control(39)=   "lblSumHdr(1)"
      Tab(2).Control(39).Enabled=   0   'False
      Tab(2).Control(40)=   "lblIntrvl(6)"
      Tab(2).Control(40).Enabled=   0   'False
      Tab(2).Control(41)=   "lblIntrvl(5)"
      Tab(2).Control(41).Enabled=   0   'False
      Tab(2).Control(42)=   "lblIntrvl(4)"
      Tab(2).Control(42).Enabled=   0   'False
      Tab(2).Control(43)=   "lblIntrvl(3)"
      Tab(2).Control(43).Enabled=   0   'False
      Tab(2).Control(44)=   "lblIntrvl(2)"
      Tab(2).Control(44).Enabled=   0   'False
      Tab(2).Control(45)=   "lblIntrvl(1)"
      Tab(2).Control(45).Enabled=   0   'False
      Tab(2).Control(46)=   "lblIntrvl(0)"
      Tab(2).Control(46).Enabled=   0   'False
      Tab(2).Control(47)=   "lblSumHdr(0)"
      Tab(2).Control(47).Enabled=   0   'False
      Tab(2).Control(48)=   "lblSum"
      Tab(2).Control(48).Enabled=   0   'False
      Tab(2).ControlCount=   49
      TabCaption(3)   =   "&Dates"
      TabPicture(3)   =   "frmLEdit.frx":0496
      Tab(3).ControlEnabled=   0   'False
      Tab(3).Control(0)=   "Frame1"
      Tab(3).Control(0).Enabled=   0   'False
      Tab(3).ControlCount=   1
      Begin VB.ComboBox lstEndMon 
         Height          =   288
         ItemData        =   "frmLEdit.frx":04B2
         Left            =   -74400
         List            =   "frmLEdit.frx":04DA
         Style           =   2  'Dropdown List
         TabIndex        =   83
         Top             =   1260
         Width           =   735
      End
      Begin VB.Frame Frame1 
         BorderStyle     =   0  'None
         Caption         =   "Frame1"
         Height          =   2775
         Left            =   -74880
         TabIndex        =   78
         Top             =   360
         Width           =   4095
         Begin VB.OptionButton optDateFmt 
            Caption         =   "yyyy MON dd hh:mm:ss - 1987 SEP 12 15:00:00"
            Height          =   252
            Index           =   0
            Left            =   240
            TabIndex        =   81
            Top             =   600
            Value           =   -1  'True
            Width           =   3810
         End
         Begin VB.OptionButton optDateFmt 
            Caption         =   "yyyy.mm.dd.hh.mm.ss - 1987.09.12.15.00.00"
            Height          =   252
            Index           =   1
            Left            =   240
            TabIndex        =   80
            Top             =   960
            Width           =   3780
         End
         Begin VB.OptionButton optDateFmt 
            Caption         =   "mm/dd/yy hh:mm:ss - 9/12/87 15:00:00"
            Height          =   252
            Index           =   2
            Left            =   240
            TabIndex        =   79
            Top             =   1320
            Width           =   3825
         End
         Begin VB.Label lblFormat 
            Caption         =   "Available Formats:"
            Height          =   255
            Left            =   120
            TabIndex        =   82
            Top             =   240
            Width           =   1695
         End
      End
      Begin VB.CheckBox chkRange 
         Caption         =   "All Outside"
         Height          =   252
         Index           =   2
         Left            =   -73440
         TabIndex        =   6
         Top             =   2640
         Value           =   1  'Checked
         Width           =   1932
      End
      Begin VB.CheckBox chkRange 
         Caption         =   "Some In, Some Out"
         Height          =   252
         Index           =   1
         Left            =   -73440
         TabIndex        =   5
         Top             =   2400
         Value           =   1  'Checked
         Width           =   1932
      End
      Begin VB.CheckBox chkRange 
         Caption         =   "All Inside"
         Height          =   252
         Index           =   0
         Left            =   -73440
         TabIndex        =   4
         Top             =   2160
         Value           =   1  'Checked
         Width           =   1932
      End
      Begin VB.OptionButton optListScreen 
         Caption         =   "Screen"
         Height          =   252
         Index           =   1
         Left            =   3240
         TabIndex        =   12
         Top             =   2415
         Width           =   972
      End
      Begin VB.OptionButton optListScreen 
         Caption         =   "List"
         Height          =   252
         Index           =   0
         Left            =   3240
         TabIndex        =   11
         Top             =   2175
         Value           =   -1  'True
         Width           =   972
      End
      Begin VB.TextBox txtTitle 
         Height          =   285
         Index           =   2
         Left            =   -74760
         TabIndex        =   3
         Top             =   1680
         WhatsThisHelpID =   20
         Width           =   3975
      End
      Begin VB.TextBox txtTitle 
         Height          =   285
         Index           =   1
         Left            =   -74760
         TabIndex        =   2
         Top             =   1200
         WhatsThisHelpID =   20
         Width           =   3975
      End
      Begin VB.TextBox txtLabel 
         Height          =   288
         Left            =   1440
         TabIndex        =   10
         Top             =   1095
         WhatsThisHelpID =   23
         Width           =   2772
      End
      Begin VB.CommandButton cmdUpDn 
         Caption         =   ">"
         Height          =   252
         Index           =   1
         Left            =   1920
         TabIndex        =   9
         Top             =   615
         WhatsThisHelpID =   22
         Width           =   252
      End
      Begin VB.CommandButton cmdUpDn 
         Caption         =   "<"
         Height          =   252
         Index           =   0
         Left            =   1680
         TabIndex        =   8
         Top             =   615
         WhatsThisHelpID =   22
         Width           =   252
      End
      Begin VB.TextBox txtTitle 
         Height          =   285
         Index           =   0
         Left            =   -74760
         TabIndex        =   1
         Top             =   720
         WhatsThisHelpID =   20
         Width           =   3975
      End
      Begin VB.CheckBox chkCnt 
         Height          =   252
         Index           =   1
         Left            =   -71250
         TabIndex        =   47
         Top             =   3105
         WhatsThisHelpID =   31
         Width           =   252
      End
      Begin VB.CheckBox chkCnt 
         Height          =   252
         Index           =   2
         Left            =   -71250
         TabIndex        =   42
         Top             =   2745
         WhatsThisHelpID =   31
         Width           =   252
      End
      Begin VB.CheckBox chkCnt 
         Height          =   252
         Index           =   3
         Left            =   -71250
         TabIndex        =   37
         Top             =   2385
         WhatsThisHelpID =   31
         Width           =   252
      End
      Begin VB.CheckBox chkCnt 
         Height          =   252
         Index           =   4
         Left            =   -71250
         TabIndex        =   32
         Top             =   2025
         WhatsThisHelpID =   31
         Width           =   252
      End
      Begin VB.CheckBox chkCnt 
         Height          =   252
         Index           =   5
         Left            =   -71250
         TabIndex        =   27
         Top             =   1665
         WhatsThisHelpID =   31
         Width           =   252
      End
      Begin VB.CheckBox chkCnt 
         Height          =   252
         Index           =   6
         Left            =   -71250
         TabIndex        =   22
         Top             =   1305
         WhatsThisHelpID =   31
         Width           =   252
      End
      Begin VB.CheckBox chkCnt 
         Height          =   252
         Index           =   7
         Left            =   -71250
         TabIndex        =   17
         Top             =   945
         WhatsThisHelpID =   31
         Width           =   252
      End
      Begin VB.CheckBox chkMax 
         Height          =   255
         Index           =   1
         Left            =   -71850
         TabIndex        =   46
         Top             =   3105
         WhatsThisHelpID =   30
         Width           =   255
      End
      Begin VB.CheckBox chkMin 
         Height          =   255
         Index           =   1
         Left            =   -72450
         TabIndex        =   45
         Top             =   3105
         WhatsThisHelpID =   29
         Width           =   255
      End
      Begin VB.CheckBox chkSum 
         Height          =   255
         Index           =   1
         Left            =   -73050
         TabIndex        =   44
         Top             =   3105
         WhatsThisHelpID =   28
         Width           =   255
      End
      Begin VB.CheckBox chkAve 
         Height          =   255
         Index           =   1
         Left            =   -73650
         TabIndex        =   43
         Top             =   3105
         WhatsThisHelpID =   27
         Width           =   255
      End
      Begin VB.CheckBox chkMax 
         Height          =   255
         Index           =   2
         Left            =   -71850
         TabIndex        =   41
         Top             =   2745
         WhatsThisHelpID =   30
         Width           =   255
      End
      Begin VB.CheckBox chkMax 
         Height          =   255
         Index           =   3
         Left            =   -71850
         TabIndex        =   36
         Top             =   2385
         WhatsThisHelpID =   30
         Width           =   255
      End
      Begin VB.CheckBox chkAve 
         Height          =   255
         Index           =   2
         Left            =   -73650
         TabIndex        =   38
         Top             =   2745
         WhatsThisHelpID =   27
         Width           =   255
      End
      Begin VB.CheckBox chkMin 
         Height          =   255
         Index           =   2
         Left            =   -72450
         TabIndex        =   40
         Top             =   2745
         WhatsThisHelpID =   29
         Width           =   255
      End
      Begin VB.CheckBox chkSum 
         Height          =   255
         Index           =   2
         Left            =   -73050
         TabIndex        =   39
         Top             =   2745
         WhatsThisHelpID =   28
         Width           =   255
      End
      Begin VB.CheckBox chkMin 
         Height          =   255
         Index           =   3
         Left            =   -72450
         TabIndex        =   35
         Top             =   2385
         WhatsThisHelpID =   29
         Width           =   255
      End
      Begin VB.CheckBox chkSum 
         Height          =   255
         Index           =   3
         Left            =   -73050
         TabIndex        =   34
         Top             =   2385
         WhatsThisHelpID =   28
         Width           =   255
      End
      Begin VB.CheckBox chkAve 
         Height          =   255
         Index           =   3
         Left            =   -73650
         TabIndex        =   33
         Top             =   2385
         WhatsThisHelpID =   27
         Width           =   255
      End
      Begin VB.CheckBox chkMax 
         Height          =   255
         Index           =   4
         Left            =   -71850
         TabIndex        =   31
         Top             =   2025
         WhatsThisHelpID =   30
         Width           =   255
      End
      Begin VB.CheckBox chkMin 
         Height          =   255
         Index           =   4
         Left            =   -72450
         TabIndex        =   30
         Top             =   2025
         WhatsThisHelpID =   29
         Width           =   255
      End
      Begin VB.CheckBox chkSum 
         Height          =   255
         Index           =   4
         Left            =   -73050
         TabIndex        =   29
         Top             =   2025
         WhatsThisHelpID =   28
         Width           =   255
      End
      Begin VB.CheckBox chkAve 
         Height          =   255
         Index           =   4
         Left            =   -73650
         TabIndex        =   28
         Top             =   2025
         WhatsThisHelpID =   27
         Width           =   255
      End
      Begin VB.CheckBox chkMax 
         Height          =   255
         Index           =   5
         Left            =   -71850
         TabIndex        =   26
         Top             =   1665
         WhatsThisHelpID =   30
         Width           =   255
      End
      Begin VB.CheckBox chkMin 
         Height          =   255
         Index           =   5
         Left            =   -72450
         TabIndex        =   25
         Top             =   1665
         WhatsThisHelpID =   29
         Width           =   255
      End
      Begin VB.CheckBox chkSum 
         Height          =   255
         Index           =   5
         Left            =   -73050
         TabIndex        =   24
         Top             =   1665
         WhatsThisHelpID =   28
         Width           =   255
      End
      Begin VB.CheckBox chkAve 
         Height          =   255
         Index           =   5
         Left            =   -73650
         TabIndex        =   23
         Top             =   1665
         WhatsThisHelpID =   27
         Width           =   255
      End
      Begin VB.CheckBox chkMax 
         Height          =   255
         Index           =   6
         Left            =   -71850
         TabIndex        =   21
         Top             =   1305
         WhatsThisHelpID =   30
         Width           =   255
      End
      Begin VB.CheckBox chkMin 
         Height          =   255
         Index           =   6
         Left            =   -72450
         TabIndex        =   20
         Top             =   1305
         WhatsThisHelpID =   29
         Width           =   255
      End
      Begin VB.CheckBox chkSum 
         Height          =   255
         Index           =   6
         Left            =   -73050
         TabIndex        =   19
         Top             =   1305
         WhatsThisHelpID =   28
         Width           =   255
      End
      Begin VB.CheckBox chkAve 
         Height          =   255
         Index           =   6
         Left            =   -73650
         TabIndex        =   18
         Top             =   1305
         WhatsThisHelpID =   27
         Width           =   255
      End
      Begin VB.CheckBox chkMax 
         Height          =   255
         Index           =   7
         Left            =   -71850
         TabIndex        =   16
         Top             =   945
         WhatsThisHelpID =   30
         Width           =   255
      End
      Begin VB.CheckBox chkMin 
         Height          =   255
         Index           =   7
         Left            =   -72450
         TabIndex        =   15
         Top             =   945
         WhatsThisHelpID =   29
         Width           =   255
      End
      Begin VB.CheckBox chkSum 
         Height          =   255
         Index           =   7
         Left            =   -73050
         TabIndex        =   14
         Top             =   945
         WhatsThisHelpID =   28
         Width           =   255
      End
      Begin VB.CheckBox chkAve 
         Height          =   255
         Index           =   7
         Left            =   -73650
         TabIndex        =   13
         Top             =   945
         WhatsThisHelpID =   27
         Width           =   255
      End
      Begin ATCoCtl.ATCoText txtSdg 
         Height          =   285
         Left            =   1440
         TabIndex        =   74
         Top             =   1815
         Width           =   495
         _ExtentX        =   868
         _ExtentY        =   508
         InsideLimitsBackground=   16777215
         OutsideHardLimitBackground=   8421631
         OutsideSoftLimitBackground=   8454143
         HardMax         =   -999
         HardMin         =   1
         SoftMax         =   12
         SoftMin         =   -999
         MaxWidth        =   5
         Alignment       =   1
         DataType        =   1
         DefaultValue    =   -999
         Value           =   "-999"
         Enabled         =   -1  'True
      End
      Begin ATCoCtl.ATCoText txtDpl 
         Height          =   285
         Left            =   1440
         TabIndex        =   75
         Top             =   2160
         Width           =   495
         _ExtentX        =   868
         _ExtentY        =   508
         InsideLimitsBackground=   16777215
         OutsideHardLimitBackground=   8421631
         OutsideSoftLimitBackground=   8454143
         HardMax         =   -999
         HardMin         =   0
         SoftMax         =   6
         SoftMin         =   -999
         MaxWidth        =   5
         Alignment       =   1
         DataType        =   1
         DefaultValue    =   -999
         Value           =   "-999"
         Enabled         =   -1  'True
      End
      Begin ATCoCtl.ATCoText txtfMin 
         Height          =   285
         Left            =   3240
         TabIndex        =   76
         Top             =   1440
         Width           =   975
         _ExtentX        =   868
         _ExtentY        =   508
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
         DefaultValue    =   -999
         Value           =   "-999"
         Enabled         =   -1  'True
      End
      Begin ATCoCtl.ATCoText txtfMax 
         Height          =   285
         Left            =   3240
         TabIndex        =   77
         Top             =   1800
         Width           =   975
         _ExtentX        =   868
         _ExtentY        =   508
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
         DefaultValue    =   -999
         Value           =   "-999"
         Enabled         =   -1  'True
      End
      Begin ATCoCtl.ATCoText txtWid 
         Height          =   285
         Left            =   1440
         TabIndex        =   73
         Top             =   1455
         Width           =   495
         _ExtentX        =   868
         _ExtentY        =   508
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
      Begin VB.Label lblSumHdr 
         Caption         =   "Count"
         Height          =   255
         Index           =   4
         Left            =   -71400
         TabIndex        =   72
         Top             =   705
         Width           =   615
      End
      Begin VB.Label lblSumHdr 
         Caption         =   "Max"
         Height          =   255
         Index           =   3
         Left            =   -71955
         TabIndex        =   71
         Top             =   705
         Width           =   375
      End
      Begin VB.Label lblSumHdr 
         Caption         =   "Min"
         Height          =   255
         Index           =   2
         Left            =   -72525
         TabIndex        =   70
         Top             =   705
         Width           =   360
      End
      Begin VB.Label lblSumHdr 
         Caption         =   "Sum"
         Height          =   255
         Index           =   1
         Left            =   -73155
         TabIndex        =   69
         Top             =   705
         Width           =   405
      End
      Begin VB.Label Label6 
         Caption         =   "Range Filters:"
         Height          =   255
         Left            =   -74760
         TabIndex        =   68
         Top             =   2160
         Width           =   1095
      End
      Begin VB.Label Label5 
         Caption         =   "Max:"
         Height          =   255
         Left            =   2520
         TabIndex        =   67
         Top             =   1815
         WhatsThisHelpID =   24
         Width           =   615
      End
      Begin VB.Label Label3 
         Caption         =   "Min:"
         Height          =   255
         Left            =   2520
         TabIndex        =   65
         Top             =   1455
         WhatsThisHelpID =   24
         Width           =   615
      End
      Begin VB.Label Label2 
         Caption         =   "Decimal Places:"
         Height          =   255
         Left            =   240
         TabIndex        =   64
         Top             =   2175
         WhatsThisHelpID =   26
         Width           =   1215
      End
      Begin VB.Label Label1 
         Caption         =   "Signif. Digits:"
         Height          =   255
         Left            =   240
         TabIndex        =   63
         Top             =   1815
         WhatsThisHelpID =   25
         Width           =   975
      End
      Begin VB.Label lblWidth 
         Caption         =   "Width:"
         Height          =   255
         Left            =   240
         TabIndex        =   62
         Top             =   1455
         WhatsThisHelpID =   24
         Width           =   975
      End
      Begin VB.Label lblLabel 
         Caption         =   "Label:"
         Height          =   255
         Left            =   240
         TabIndex        =   61
         Top             =   1095
         WhatsThisHelpID =   23
         Width           =   975
      End
      Begin VB.Label lblFldNum 
         BorderStyle     =   1  'Fixed Single
         Height          =   255
         Left            =   1200
         TabIndex        =   7
         Top             =   615
         WhatsThisHelpID =   21
         Width           =   375
      End
      Begin VB.Label lblField 
         Caption         =   "Current field:"
         Height          =   255
         Left            =   240
         TabIndex        =   60
         Top             =   615
         WhatsThisHelpID =   21
         Width           =   975
      End
      Begin VB.Label lblTitle 
         Caption         =   "Title:"
         Height          =   255
         Left            =   -74760
         TabIndex        =   59
         Top             =   480
         Width           =   495
      End
      Begin VB.Label lblIntrvl 
         Caption         =   "Second"
         Height          =   255
         Index           =   6
         Left            =   -74850
         TabIndex        =   58
         Top             =   3105
         Width           =   615
      End
      Begin VB.Label lblIntrvl 
         Caption         =   "Minute"
         Height          =   255
         Index           =   5
         Left            =   -74850
         TabIndex        =   57
         Top             =   2745
         Width           =   495
      End
      Begin VB.Label lblIntrvl 
         Caption         =   "Hour"
         Height          =   255
         Index           =   4
         Left            =   -74850
         TabIndex        =   56
         Top             =   2385
         Width           =   495
      End
      Begin VB.Label lblIntrvl 
         Caption         =   "Day"
         Height          =   255
         Index           =   3
         Left            =   -74850
         TabIndex        =   55
         Top             =   2025
         Width           =   495
      End
      Begin VB.Label lblIntrvl 
         Caption         =   "Month"
         Height          =   255
         Index           =   2
         Left            =   -74850
         TabIndex        =   54
         Top             =   1665
         Width           =   495
      End
      Begin VB.Label lblIntrvl 
         Caption         =   "Year"
         Height          =   255
         Index           =   1
         Left            =   -74850
         TabIndex        =   53
         Top             =   1320
         Width           =   495
      End
      Begin VB.Label lblIntrvl 
         Caption         =   "Grand Total"
         Height          =   255
         Index           =   0
         Left            =   -74850
         TabIndex        =   52
         Top             =   945
         Width           =   975
      End
      Begin VB.Label lblSumHdr 
         Caption         =   "Ave"
         Height          =   255
         Index           =   0
         Left            =   -73680
         TabIndex        =   51
         Top             =   720
         Width           =   435
      End
      Begin VB.Label lblSum 
         Caption         =   "Aggregations/Summaries:"
         Height          =   255
         Left            =   -74760
         TabIndex        =   50
         Top             =   360
         Width           =   1935
      End
   End
   Begin VB.Label Label4 
      Caption         =   "Field width:"
      Height          =   252
      Left            =   2640
      TabIndex        =   66
      Top             =   1920
      WhatsThisHelpID =   24
      Width           =   972
   End
End
Attribute VB_Name = "frmLEdit"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
'Copyright 2001 by AQUA TERRA Consultants

Dim lnlis&, lnvar&
Dim cfld&
'local field info
Dim titl$
Dim capt$
Dim FldLab$()
Dim wid&()
Dim sdg&()
Dim dpl&()
Dim fmin!()
Dim fmax!()
Dim FInRan() As Boolean
'local summation info
Dim nsums&, mxsumcnt&
Dim sumtype&(34)
Dim sumint&(34)
Dim yrend&
'local time info
Dim sdt&(5), edt&(5)
Dim tc&(0), ts&(0), dtr&(0)
Dim ldfmt&
'local help file
Dim CurHelpFile$


Private Sub cmdEdList_Click(index As Integer)

    Dim i&, c(3) As Boolean

    If index = 0 Then 'ok clicked
      MousePointer = vbHourglass
      For i = 0 To lnvar - 1
        Call frmL.SetVarLabel(i, FldLab(i))
        Call frmL.SetFldPrms(i, wid(i), sdg(i), dpl(i))
        Call frmL.SetFldRange(i, fmin(i), fmax(i), FInRan(i))
      Next i
      'update title
      titl = txtTitle(0).Text
      If txtTitle(1).Text <> "" Then
        titl = titl & "&" & txtTitle(1).Text
      End If
      If txtTitle(2).Text <> "" Then
        titl = titl & "&" & txtTitle(2).Text
      End If
      Call frmL.SetTitles(titl, capt)
      'view check boxes to set sums/aggs
      nsums = 0
      For i = 1 To 7
        If chkAve(i).Value = 1 Then
          sumtype(nsums) = 0
          sumint(nsums) = i
          nsums = nsums + 1
        End If
        If chkSum(i).Value = 1 Then
          sumtype(nsums) = 1
          sumint(nsums) = i
          nsums = nsums + 1
        End If
        If chkMin(i).Value = 1 Then
          sumtype(nsums) = 3
          sumint(nsums) = i
          nsums = nsums + 1
        End If
        If chkMax(i).Value = 1 Then
          sumtype(nsums) = 2
          sumint(nsums) = i
          nsums = nsums + 1
        End If
        If chkCnt(i).Value = 1 Then
          sumtype(nsums) = 4
          sumint(nsums) = i
          nsums = nsums + 1
        End If
      Next i
      Call frmL.SetSums(nsums, sumtype(), sumint(), lstEndMon.ListIndex + 1, 0)

      'set range information
      For i = 0 To 2
        If chkRange(i).Value = 1 Then
          c(i) = False
        Else
          c(i) = True
        End If
      Next i
      Call frmL.SetXRange(c(0), c(1), c(2))
      'set date format
      Call frmL.SetDateFmt(ldfmt)

      Set curL = frmL
      Call frmL.DispList(True)
      MousePointer = vbDefault
    End If
    'restore application help file
    App.HelpFile = CurHelpFile
    Unload frmLEdit

End Sub

Private Sub cmdUpDn_Click(index As Integer)

    Dim inc&

    If index = 0 Then
      inc = -1
    Else
      inc = 1
    End If
    cfld = cfld + inc
    If cfld > lnvar - 1 Then
      cfld = 0
    ElseIf cfld < 0 Then
      cfld = lnvar - 1
    End If
    lblFldNum.Caption = CStr(cfld + 1)

End Sub


Private Sub Form_Load()

    Dim i&, tpos&, tp2&, c(3) As Boolean

    'save current application help file
    CurHelpFile = App.HelpFile
    App.HelpFile = GraphHelpFileName
    Caption = frmL.Caption & " Edit"
    cfld = 0
    Call frmL.GetNumVars(lnlis, lnvar)
    ReDim FldLab(lnvar)
    ReDim wid(lnvar)
    ReDim sdg(lnvar)
    ReDim dpl(lnvar)
    ReDim fmin(lnvar)
    ReDim fmax(lnvar)
    ReDim FInRan(lnvar)
    For i = 0 To lnvar - 1
      FldLab(i) = frmL.GetVarLabel(i)
      Call frmL.GetFldPrms(i, wid(i), sdg(i), dpl(i))
      Call frmL.GetFldRange(i, fmin(i), fmax(i), FInRan(i))
    Next i
    Call frmL.GetTitles(titl, capt)
    tpos = InStr(titl, "&")
    If tpos > 0 Then
      txtTitle(0).Text = Left(titl, tpos - 1)
      tp2 = InStr(tpos + 1, titl, "&")
      If tp2 > 0 Then
        txtTitle(1).Text = Mid(titl, tpos + 1, tp2 - tpos - 1)
        txtTitle(2).Text = Mid(titl, tp2 + 1)
      Else
        txtTitle(1).Text = Mid(titl, tpos + 1)
      End If
    Else
      txtTitle(0).Text = titl
    End If
    lblFldNum.Caption = CStr(cfld + 1)
    Call frmL.GetSums(nsums, sumtype, sumint, yrend, mxsumcnt)
    For i = 0 To nsums - 1
      If sumtype(i) = 1 Then 'sum
        chkSum(sumint(i)).Value = 1
      ElseIf sumtype(i) = 0 Then 'average
        chkAve(sumint(i)).Value = 1
      ElseIf sumtype(i) = 3 Then 'minimum
        chkMin(sumint(i)).Value = 1
      ElseIf sumtype(i) = 2 Then 'maximum
        chkMax(sumint(i)).Value = 1
      ElseIf sumtype(i) = 4 Then 'count
        chkCnt(sumint(i)).Value = 1
      End If
    Next i
    lstEndMon.ListIndex = yrend - 1
    'get time info
    Call frmL.GetTime(ts(), tc(), sdt(), edt(), dtr())
    For i = tc(0) - 1 To 1 Step -1
      'disable any time intervals less than tcode
      chkSum(i).Enabled = False
      chkAve(i).Enabled = False
      chkMin(i).Enabled = False
      chkMax(i).Enabled = False
      chkCnt(i).Enabled = False
    Next i
    Call frmL.GetXRange(c(0), c(1), c(2))
    For i = 0 To 2
      If c(i) Then
        chkRange(i).Value = 0
      Else
        chkRange(i).Value = 1
      End If
    Next i
    'get date format
    Call frmL.GetDateFmt(ldfmt)
    optDateFmt(ldfmt).Value = True

End Sub


Private Sub lblFldNum_Change()

  'display label for this field
  txtLabel.Text = FldLab(cfld)
  'display field format parameters
  txtWid.Value = wid(cfld)
  txtSdg.Value = sdg(cfld)
  txtDpl.Value = dpl(cfld)
  txtfMin.Value = fmin(cfld)
  txtfMax.Value = fmax(cfld)
  If FInRan(cfld) Then
    optListScreen(0) = True
  Else
    optListScreen(1) = True
  End If

End Sub

Private Sub optDateFmt_Click(index As Integer)

    ldfmt = index

End Sub

Private Sub optListScreen_Click(index As Integer)
    If index = 0 Then
       FInRan(cfld) = True
    Else
       FInRan(cfld) = False
    End If
End Sub

Private Sub txtDpl_CommitChange()
  dpl(cfld) = txtDpl.Value
End Sub

Private Sub txtfMax_CommitChange()
  fmax(cfld) = txtfMax.Value
End Sub

Private Sub txtfMin_CommitChange()
  fmin(cfld) = txtfMin.Value
End Sub

Private Sub txtLabel_LostFocus()

    FldLab(cfld) = txtLabel.Text

End Sub

Private Sub txtSdg_CommitChange()
  sdg(cfld) = txtSdg.Value
End Sub

Private Sub txtWid_CommitChange()
  wid(cfld) = txtWid.Value
End Sub
