VERSION 5.00
Begin VB.Form frmWDMUComp 
   BorderStyle     =   3  'Fixed Dialog
   Caption         =   "WDMUtil Compute"
   ClientHeight    =   7296
   ClientLeft      =   4800
   ClientTop       =   804
   ClientWidth     =   7320
   BeginProperty Font 
      Name            =   "MS Sans Serif"
      Size            =   7.8
      Charset         =   0
      Weight          =   700
      Underline       =   0   'False
      Italic          =   0   'False
      Strikethrough   =   0   'False
   EndProperty
   HelpContextID   =   72
   Icon            =   "WDMUComp.frx":0000
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   PaletteMode     =   1  'UseZOrder
   ScaleHeight     =   7296
   ScaleWidth      =   7320
   Begin VB.Frame fraFunction 
      Caption         =   "Disaggregate Functions"
      Height          =   1572
      Index           =   1
      Left            =   120
      TabIndex        =   10
      Top             =   840
      Width           =   7092
      Begin VB.OptionButton optFunction 
         Caption         =   "Precipitation"
         Height          =   252
         HelpContextID   =   86
         Index           =   11
         Left            =   3480
         TabIndex        =   16
         Top             =   720
         Width           =   2292
      End
      Begin VB.OptionButton optFunction 
         Caption         =   "Solar Radiation"
         Height          =   252
         HelpContextID   =   81
         Index           =   6
         Left            =   240
         TabIndex        =   11
         Top             =   240
         Value           =   -1  'True
         Width           =   2532
      End
      Begin VB.OptionButton optFunction 
         Caption         =   "Temperature"
         Height          =   252
         HelpContextID   =   82
         Index           =   7
         Left            =   240
         TabIndex        =   12
         Top             =   480
         Width           =   2532
      End
      Begin VB.OptionButton optFunction 
         Caption         =   "Dewpoint Temperature"
         Height          =   252
         HelpContextID   =   83
         Index           =   8
         Left            =   240
         TabIndex        =   13
         Top             =   720
         Width           =   2532
      End
      Begin VB.OptionButton optFunction 
         Caption         =   "Evapotranspiration"
         Height          =   252
         HelpContextID   =   84
         Index           =   9
         Left            =   3480
         TabIndex        =   14
         Top             =   240
         Width           =   2652
      End
      Begin VB.OptionButton optFunction 
         Caption         =   "Wind Travel"
         Height          =   252
         HelpContextID   =   85
         Index           =   10
         Left            =   3480
         TabIndex        =   15
         TabStop         =   0   'False
         Top             =   480
         Width           =   2292
      End
      Begin VB.Label lblFunction 
         Height          =   396
         Index           =   1
         Left            =   120
         TabIndex        =   59
         Top             =   1080
         Width           =   6852
      End
   End
   Begin VB.Frame fraDumCompDates 
      Caption         =   "Dates"
      Height          =   1332
      Left            =   120
      TabIndex        =   93
      Top             =   6480
      Width           =   7092
      Begin VB.Label lblDumCompDates 
         Caption         =   "Label1"
         Height          =   252
         Left            =   120
         TabIndex        =   94
         Top             =   360
         Width           =   6492
      End
   End
   Begin ATCoCtl.ATCoDate ctlCompDate 
      Height          =   1356
      HelpContextID   =   53
      Left            =   120
      TabIndex        =   92
      Top             =   6480
      Width           =   7092
      _ExtentX        =   12510
      _ExtentY        =   2392
      TUnit           =   4
      TAggr           =   1
      TStep           =   1
      CurrE           =   0
      CurrS           =   0
      LimtE           =   35795
      LimtS           =   33239
      DispL           =   1
      LabelCurrentRange=   "Current"
      TstepVisible    =   0   'False
   End
   Begin VB.Frame fraExtras 
      Caption         =   "Additional Inputs"
      Height          =   1572
      Left            =   120
      TabIndex        =   60
      Top             =   4800
      Width           =   7092
      Begin VB.OptionButton optForC 
         Caption         =   "Celsius"
         Height          =   252
         Index           =   1
         Left            =   5400
         TabIndex        =   96
         Top             =   360
         Width           =   1332
      End
      Begin VB.OptionButton optForC 
         Caption         =   "Fahrenheit"
         Height          =   252
         Index           =   0
         Left            =   5400
         TabIndex        =   95
         Top             =   120
         Value           =   -1  'True
         Width           =   1332
      End
      Begin ATCoCtl.ATCoText atxConst 
         Height          =   252
         Left            =   3600
         TabIndex        =   66
         Top             =   240
         Visible         =   0   'False
         Width           =   492
         _ExtentX        =   868
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
         DefaultValue    =   "24"
         Value           =   "24"
         Enabled         =   -1  'True
      End
      Begin ATCoCtl.ATCoText atxLat 
         Height          =   252
         Index           =   0
         Left            =   2040
         TabIndex        =   61
         Top             =   240
         Width           =   372
         _ExtentX        =   656
         _ExtentY        =   445
         InsideLimitsBackground=   16777215
         OutsideHardLimitBackground=   8421631
         OutsideSoftLimitBackground=   8454143
         HardMax         =   90
         HardMin         =   0
         SoftMax         =   90
         SoftMin         =   0
         MaxWidth        =   -999
         Alignment       =   1
         DataType        =   1
         DefaultValue    =   ""
         Value           =   ""
         Enabled         =   -1  'True
      End
      Begin ATCoCtl.ATCoText atxLat 
         Height          =   252
         Index           =   1
         Left            =   2520
         TabIndex        =   62
         Top             =   240
         Width           =   372
         _ExtentX        =   656
         _ExtentY        =   445
         InsideLimitsBackground=   16777215
         OutsideHardLimitBackground=   8421631
         OutsideSoftLimitBackground=   8454143
         HardMax         =   60
         HardMin         =   0
         SoftMax         =   60
         SoftMin         =   0
         MaxWidth        =   -999
         Alignment       =   1
         DataType        =   1
         DefaultValue    =   ""
         Value           =   ""
         Enabled         =   -1  'True
      End
      Begin ATCoCtl.ATCoText atxLat 
         Height          =   252
         Index           =   2
         Left            =   3000
         TabIndex        =   63
         Top             =   240
         Width           =   372
         _ExtentX        =   656
         _ExtentY        =   445
         InsideLimitsBackground=   16777215
         OutsideHardLimitBackground=   8421631
         OutsideSoftLimitBackground=   8454143
         HardMax         =   60
         HardMin         =   0
         SoftMax         =   60
         SoftMin         =   0
         MaxWidth        =   -999
         Alignment       =   1
         DataType        =   1
         DefaultValue    =   ""
         Value           =   ""
         Enabled         =   -1  'True
      End
      Begin ATCoCtl.ATCoText atxCoef 
         Height          =   252
         Index           =   1
         Left            =   600
         TabIndex        =   80
         Top             =   960
         Visible         =   0   'False
         Width           =   492
         _ExtentX        =   868
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
         DefaultValue    =   ""
         Value           =   ""
         Enabled         =   -1  'True
      End
      Begin ATCoCtl.ATCoText atxCoef 
         Height          =   252
         Index           =   2
         Left            =   1080
         TabIndex        =   81
         Top             =   960
         Visible         =   0   'False
         Width           =   492
         _ExtentX        =   868
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
         DefaultValue    =   ""
         Value           =   ""
         Enabled         =   -1  'True
      End
      Begin ATCoCtl.ATCoText atxCoef 
         Height          =   252
         Index           =   3
         Left            =   1560
         TabIndex        =   82
         Top             =   960
         Visible         =   0   'False
         Width           =   492
         _ExtentX        =   868
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
         DefaultValue    =   ""
         Value           =   ""
         Enabled         =   -1  'True
      End
      Begin ATCoCtl.ATCoText atxCoef 
         Height          =   252
         Index           =   4
         Left            =   2040
         TabIndex        =   83
         Top             =   960
         Visible         =   0   'False
         Width           =   492
         _ExtentX        =   868
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
         DefaultValue    =   ""
         Value           =   ""
         Enabled         =   -1  'True
      End
      Begin ATCoCtl.ATCoText atxCoef 
         Height          =   252
         Index           =   5
         Left            =   2520
         TabIndex        =   84
         Top             =   960
         Visible         =   0   'False
         Width           =   492
         _ExtentX        =   868
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
         DefaultValue    =   ""
         Value           =   ""
         Enabled         =   -1  'True
      End
      Begin ATCoCtl.ATCoText atxCoef 
         Height          =   252
         Index           =   6
         Left            =   3000
         TabIndex        =   85
         Top             =   960
         Visible         =   0   'False
         Width           =   492
         _ExtentX        =   868
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
         DefaultValue    =   ""
         Value           =   ""
         Enabled         =   -1  'True
      End
      Begin ATCoCtl.ATCoText atxCoef 
         Height          =   252
         Index           =   7
         Left            =   3480
         TabIndex        =   86
         Top             =   960
         Visible         =   0   'False
         Width           =   492
         _ExtentX        =   868
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
         DefaultValue    =   ""
         Value           =   ""
         Enabled         =   -1  'True
      End
      Begin ATCoCtl.ATCoText atxCoef 
         Height          =   252
         Index           =   8
         Left            =   3960
         TabIndex        =   87
         Top             =   960
         Visible         =   0   'False
         Width           =   492
         _ExtentX        =   868
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
         DefaultValue    =   ""
         Value           =   ""
         Enabled         =   -1  'True
      End
      Begin ATCoCtl.ATCoText atxCoef 
         Height          =   252
         Index           =   9
         Left            =   4440
         TabIndex        =   88
         Top             =   960
         Visible         =   0   'False
         Width           =   492
         _ExtentX        =   868
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
         DefaultValue    =   ""
         Value           =   ""
         Enabled         =   -1  'True
      End
      Begin ATCoCtl.ATCoText atxCoef 
         Height          =   252
         Index           =   10
         Left            =   4920
         TabIndex        =   89
         Top             =   960
         Visible         =   0   'False
         Width           =   492
         _ExtentX        =   868
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
         DefaultValue    =   ""
         Value           =   ""
         Enabled         =   -1  'True
      End
      Begin ATCoCtl.ATCoText atxCoef 
         Height          =   252
         Index           =   11
         Left            =   5400
         TabIndex        =   90
         Top             =   960
         Visible         =   0   'False
         Width           =   492
         _ExtentX        =   868
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
         DefaultValue    =   ""
         Value           =   ""
         Enabled         =   -1  'True
      End
      Begin ATCoCtl.ATCoText atxCoef 
         Height          =   252
         Index           =   12
         Left            =   5880
         TabIndex        =   91
         Top             =   960
         Visible         =   0   'False
         Width           =   492
         _ExtentX        =   868
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
         DefaultValue    =   ""
         Value           =   ""
         Enabled         =   -1  'True
      End
      Begin ATCoCtl.ATCoText atxCoef 
         Height          =   252
         Index           =   13
         Left            =   600
         TabIndex        =   98
         Top             =   1200
         Visible         =   0   'False
         Width           =   492
         _ExtentX        =   868
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
         DefaultValue    =   ""
         Value           =   ""
         Enabled         =   -1  'True
      End
      Begin ATCoCtl.ATCoText atxCoef 
         Height          =   252
         Index           =   14
         Left            =   1080
         TabIndex        =   99
         Top             =   1200
         Visible         =   0   'False
         Width           =   492
         _ExtentX        =   868
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
         DefaultValue    =   ""
         Value           =   ""
         Enabled         =   -1  'True
      End
      Begin ATCoCtl.ATCoText atxCoef 
         Height          =   252
         Index           =   15
         Left            =   1560
         TabIndex        =   100
         Top             =   1200
         Visible         =   0   'False
         Width           =   492
         _ExtentX        =   868
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
         DefaultValue    =   ""
         Value           =   ""
         Enabled         =   -1  'True
      End
      Begin ATCoCtl.ATCoText atxCoef 
         Height          =   252
         Index           =   16
         Left            =   2040
         TabIndex        =   101
         Top             =   1200
         Visible         =   0   'False
         Width           =   492
         _ExtentX        =   868
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
         DefaultValue    =   ""
         Value           =   ""
         Enabled         =   -1  'True
      End
      Begin ATCoCtl.ATCoText atxCoef 
         Height          =   252
         Index           =   17
         Left            =   2520
         TabIndex        =   102
         Top             =   1200
         Visible         =   0   'False
         Width           =   492
         _ExtentX        =   868
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
         DefaultValue    =   ""
         Value           =   ""
         Enabled         =   -1  'True
      End
      Begin ATCoCtl.ATCoText atxCoef 
         Height          =   252
         Index           =   18
         Left            =   3000
         TabIndex        =   103
         Top             =   1200
         Visible         =   0   'False
         Width           =   492
         _ExtentX        =   868
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
         DefaultValue    =   ""
         Value           =   ""
         Enabled         =   -1  'True
      End
      Begin ATCoCtl.ATCoText atxCoef 
         Height          =   252
         Index           =   19
         Left            =   3480
         TabIndex        =   104
         Top             =   1200
         Visible         =   0   'False
         Width           =   492
         _ExtentX        =   868
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
         DefaultValue    =   ""
         Value           =   ""
         Enabled         =   -1  'True
      End
      Begin ATCoCtl.ATCoText atxCoef 
         Height          =   252
         Index           =   20
         Left            =   3960
         TabIndex        =   105
         Top             =   1200
         Visible         =   0   'False
         Width           =   492
         _ExtentX        =   868
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
         DefaultValue    =   ""
         Value           =   ""
         Enabled         =   -1  'True
      End
      Begin ATCoCtl.ATCoText atxCoef 
         Height          =   252
         Index           =   21
         Left            =   4440
         TabIndex        =   106
         Top             =   1200
         Visible         =   0   'False
         Width           =   492
         _ExtentX        =   868
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
         DefaultValue    =   ""
         Value           =   ""
         Enabled         =   -1  'True
      End
      Begin ATCoCtl.ATCoText atxCoef 
         Height          =   252
         Index           =   22
         Left            =   4920
         TabIndex        =   107
         Top             =   1200
         Visible         =   0   'False
         Width           =   492
         _ExtentX        =   868
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
         DefaultValue    =   ""
         Value           =   ""
         Enabled         =   -1  'True
      End
      Begin ATCoCtl.ATCoText atxCoef 
         Height          =   252
         Index           =   23
         Left            =   5400
         TabIndex        =   108
         Top             =   1200
         Visible         =   0   'False
         Width           =   492
         _ExtentX        =   868
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
         DefaultValue    =   ""
         Value           =   ""
         Enabled         =   -1  'True
      End
      Begin ATCoCtl.ATCoText atxCoef 
         Height          =   252
         Index           =   24
         Left            =   5880
         TabIndex        =   109
         Top             =   1200
         Visible         =   0   'False
         Width           =   492
         _ExtentX        =   868
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
         DefaultValue    =   ""
         Value           =   ""
         Enabled         =   -1  'True
      End
      Begin ATCoCtl.ATCoText atxTolerance 
         Height          =   252
         Left            =   5400
         TabIndex        =   114
         Top             =   480
         Visible         =   0   'False
         Width           =   492
         _ExtentX        =   868
         _ExtentY        =   445
         InsideLimitsBackground=   16777215
         OutsideHardLimitBackground=   8421631
         OutsideSoftLimitBackground=   8454143
         HardMax         =   100
         HardMin         =   1
         SoftMax         =   100
         SoftMin         =   1
         MaxWidth        =   -999
         Alignment       =   1
         DataType        =   2
         DefaultValue    =   ""
         Value           =   ""
         Enabled         =   -1  'True
      End
      Begin ATCoCtl.ATCoText atxSummFile 
         Height          =   252
         Left            =   2400
         TabIndex        =   116
         Top             =   600
         Visible         =   0   'False
         Width           =   3492
         _ExtentX        =   6160
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
         DataType        =   0
         DefaultValue    =   "Disagg.sum"
         Value           =   "Disagg.sum"
         Enabled         =   -1  'True
      End
      Begin VB.Label lblSummFile 
         Caption         =   "Summary Output File:"
         Height          =   252
         Left            =   600
         TabIndex        =   115
         Top             =   600
         Visible         =   0   'False
         Width           =   1932
      End
      Begin VB.Label lblTolerance 
         Caption         =   "Data Tolerance (%):"
         Height          =   252
         Left            =   3720
         TabIndex        =   113
         Top             =   480
         Visible         =   0   'False
         Width           =   1812
      End
      Begin VB.Label lblHour 
         Caption         =   "13-24"
         Height          =   252
         Index           =   1
         Left            =   120
         TabIndex        =   111
         Top             =   840
         Visible         =   0   'False
         Width           =   492
      End
      Begin VB.Label lblHour 
         Caption         =   "1-12"
         Height          =   252
         Index           =   0
         Left            =   120
         TabIndex        =   110
         Top             =   480
         Visible         =   0   'False
         Width           =   492
      End
      Begin VB.Label lblForC 
         Caption         =   "Temperature Units:"
         Height          =   252
         Left            =   3720
         TabIndex        =   97
         Top             =   240
         Visible         =   0   'False
         Width           =   1692
      End
      Begin VB.Label lblCoef 
         Caption         =   "Dec"
         Height          =   252
         Index           =   12
         Left            =   5880
         TabIndex        =   79
         Top             =   720
         Visible         =   0   'False
         Width           =   372
      End
      Begin VB.Label lblCoef 
         Caption         =   "Nov"
         Height          =   252
         Index           =   11
         Left            =   5400
         TabIndex        =   78
         Top             =   720
         Visible         =   0   'False
         Width           =   372
      End
      Begin VB.Label lblCoef 
         Caption         =   "Oct"
         Height          =   252
         Index           =   10
         Left            =   4920
         TabIndex        =   77
         Top             =   720
         Visible         =   0   'False
         Width           =   372
      End
      Begin VB.Label lblCoef 
         Caption         =   "Sep"
         Height          =   252
         Index           =   9
         Left            =   4440
         TabIndex        =   76
         Top             =   720
         Visible         =   0   'False
         Width           =   372
      End
      Begin VB.Label lblCoef 
         Caption         =   "Aug"
         Height          =   252
         Index           =   8
         Left            =   3960
         TabIndex        =   75
         Top             =   720
         Visible         =   0   'False
         Width           =   372
      End
      Begin VB.Label lblCoef 
         Caption         =   "Jul"
         Height          =   252
         Index           =   7
         Left            =   3480
         TabIndex        =   74
         Top             =   720
         Visible         =   0   'False
         Width           =   372
      End
      Begin VB.Label lblCoef 
         Caption         =   "Jun"
         Height          =   252
         Index           =   6
         Left            =   3000
         TabIndex        =   73
         Top             =   720
         Visible         =   0   'False
         Width           =   372
      End
      Begin VB.Label lblCoef 
         Caption         =   "May"
         Height          =   252
         Index           =   5
         Left            =   2520
         TabIndex        =   72
         Top             =   720
         Visible         =   0   'False
         Width           =   372
      End
      Begin VB.Label lblCoef 
         Caption         =   "Apr"
         Height          =   252
         Index           =   4
         Left            =   2040
         TabIndex        =   71
         Top             =   720
         Visible         =   0   'False
         Width           =   372
      End
      Begin VB.Label lblCoef 
         Caption         =   "Mar"
         Height          =   252
         Index           =   3
         Left            =   1560
         TabIndex        =   70
         Top             =   720
         Visible         =   0   'False
         Width           =   372
      End
      Begin VB.Label lblCoef 
         Caption         =   "Feb"
         Height          =   252
         Index           =   2
         Left            =   1080
         TabIndex        =   69
         Top             =   720
         Visible         =   0   'False
         Width           =   372
      End
      Begin VB.Label lblCoef 
         Caption         =   "Jan"
         Height          =   252
         Index           =   1
         Left            =   600
         TabIndex        =   68
         Top             =   720
         Visible         =   0   'False
         Width           =   372
      End
      Begin VB.Label lblCoef 
         Caption         =   "Monthly Coefficients:"
         Height          =   252
         Index           =   0
         Left            =   600
         TabIndex        =   67
         Top             =   480
         Visible         =   0   'False
         Width           =   2292
      End
      Begin VB.Label lblConst 
         Caption         =   "Constant Coefficient:"
         Height          =   252
         Left            =   600
         TabIndex        =   65
         Top             =   240
         Visible         =   0   'False
         Width           =   1812
      End
      Begin VB.Label lblLat 
         Caption         =   "Latitude (d,m,s):"
         Height          =   252
         Left            =   600
         TabIndex        =   64
         Top             =   240
         Width           =   1452
      End
   End
   Begin VB.CommandButton cmdClose 
      Cancel          =   -1  'True
      Caption         =   "Close"
      Height          =   372
      Left            =   3960
      TabIndex        =   30
      Top             =   6720
      Width           =   972
   End
   Begin VB.CommandButton cmdPerform 
      Caption         =   "&Perform Operation"
      Default         =   -1  'True
      Height          =   372
      Left            =   1800
      TabIndex        =   29
      Top             =   6720
      Width           =   1812
   End
   Begin VB.Frame fraFunction 
      Caption         =   "Compute Functions"
      Height          =   1572
      Index           =   0
      Left            =   120
      TabIndex        =   3
      Top             =   840
      Width           =   7092
      Begin VB.OptionButton optFunction 
         Caption         =   "Percent Cloud Cover"
         Height          =   252
         HelpContextID   =   79
         Index           =   5
         Left            =   3480
         TabIndex        =   9
         Top             =   720
         Width           =   2292
      End
      Begin VB.OptionButton optFunction 
         Caption         =   "Wind Travel"
         Height          =   252
         HelpContextID   =   78
         Index           =   4
         Left            =   3480
         TabIndex        =   8
         Top             =   480
         Width           =   2292
      End
      Begin VB.OptionButton optFunction 
         Caption         =   "Penman Pan Evaporation"
         Height          =   252
         HelpContextID   =   77
         Index           =   3
         Left            =   3480
         TabIndex        =   7
         Top             =   240
         Width           =   2652
      End
      Begin VB.OptionButton optFunction 
         Caption         =   "Hamon PET"
         Height          =   252
         HelpContextID   =   76
         Index           =   2
         Left            =   240
         TabIndex        =   6
         Top             =   720
         Width           =   2532
      End
      Begin VB.OptionButton optFunction 
         Caption         =   "Jensen PET"
         Height          =   252
         HelpContextID   =   75
         Index           =   1
         Left            =   240
         TabIndex        =   5
         Top             =   480
         Width           =   2532
      End
      Begin VB.OptionButton optFunction 
         Caption         =   "Solar Radiation"
         Height          =   252
         HelpContextID   =   74
         Index           =   0
         Left            =   240
         TabIndex        =   4
         Top             =   240
         Value           =   -1  'True
         Width           =   2532
      End
      Begin VB.Label lblFunction 
         Height          =   400
         Index           =   0
         Left            =   120
         TabIndex        =   28
         Top             =   1080
         Width           =   6612
      End
   End
   Begin VB.Frame frmOperation 
      Caption         =   "Operation"
      Height          =   612
      Left            =   120
      TabIndex        =   0
      Top             =   120
      Width           =   7092
      Begin VB.OptionButton optOperation 
         Caption         =   "&Disaggregate"
         Height          =   252
         HelpContextID   =   80
         Index           =   1
         Left            =   2880
         TabIndex        =   2
         Top             =   240
         Width           =   1452
      End
      Begin VB.OptionButton optOperation 
         Caption         =   "&Compute"
         Height          =   252
         HelpContextID   =   73
         Index           =   0
         Left            =   720
         TabIndex        =   1
         Top             =   240
         Width           =   1092
      End
   End
   Begin VB.Frame fraTimeseries 
      Caption         =   "Timeseries"
      Height          =   3492
      Left            =   120
      TabIndex        =   21
      Top             =   2520
      Width           =   7092
      Begin VB.ComboBox cboInDsn 
         Height          =   288
         Index           =   5
         Left            =   6240
         TabIndex        =   54
         Text            =   "Combo1"
         Top             =   3000
         Visible         =   0   'False
         Width           =   732
      End
      Begin VB.ComboBox cboInSen 
         Height          =   288
         Index           =   5
         Left            =   4680
         TabIndex        =   53
         Text            =   "Combo1"
         Top             =   3000
         Visible         =   0   'False
         Width           =   1332
      End
      Begin VB.ComboBox cboInLoc 
         Height          =   288
         Index           =   5
         Left            =   3240
         TabIndex        =   52
         Text            =   "Combo1"
         Top             =   3000
         Visible         =   0   'False
         Width           =   1332
      End
      Begin VB.ComboBox cboInCon 
         Height          =   288
         Index           =   5
         Left            =   1800
         TabIndex        =   51
         Text            =   "Combo1"
         Top             =   3000
         Visible         =   0   'False
         Width           =   1332
      End
      Begin VB.TextBox txtSenOut 
         Height          =   288
         Left            =   4680
         TabIndex        =   19
         Top             =   480
         Width           =   1332
      End
      Begin VB.TextBox txtLocOut 
         Height          =   288
         Left            =   3240
         TabIndex        =   18
         Top             =   480
         Width           =   1332
      End
      Begin VB.TextBox txtConOut 
         Height          =   288
         Left            =   1800
         TabIndex        =   17
         Top             =   480
         Width           =   1332
      End
      Begin ATCoCtl.ATCoText atxOutDsn 
         Height          =   288
         Left            =   6240
         TabIndex        =   20
         Top             =   480
         Width           =   732
         _ExtentX        =   1291
         _ExtentY        =   508
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
         DefaultValue    =   0
         Value           =   ""
         Enabled         =   -1  'True
      End
      Begin VB.ComboBox cboInDsn 
         Height          =   288
         Index           =   4
         Left            =   6240
         TabIndex        =   50
         Text            =   "Combo1"
         Top             =   2640
         Visible         =   0   'False
         Width           =   732
      End
      Begin VB.ComboBox cboInDsn 
         Height          =   288
         Index           =   3
         Left            =   6240
         TabIndex        =   46
         Text            =   "Combo1"
         Top             =   2280
         Visible         =   0   'False
         Width           =   732
      End
      Begin VB.ComboBox cboInDsn 
         Height          =   288
         Index           =   2
         Left            =   6240
         TabIndex        =   42
         Text            =   "Combo1"
         Top             =   1920
         Visible         =   0   'False
         Width           =   732
      End
      Begin VB.ComboBox cboInDsn 
         Height          =   288
         Index           =   1
         Left            =   6240
         TabIndex        =   38
         Text            =   "Combo1"
         Top             =   1560
         Visible         =   0   'False
         Width           =   732
      End
      Begin VB.ComboBox cboInDsn 
         Height          =   288
         Index           =   0
         Left            =   6240
         TabIndex        =   34
         Text            =   "Combo1"
         Top             =   1200
         Width           =   732
      End
      Begin VB.ComboBox cboInCon 
         Height          =   288
         Index           =   4
         Left            =   1800
         TabIndex        =   47
         Text            =   "Combo1"
         Top             =   2640
         Visible         =   0   'False
         Width           =   1332
      End
      Begin VB.ComboBox cboInCon 
         Height          =   288
         Index           =   3
         Left            =   1800
         TabIndex        =   43
         Text            =   "Combo1"
         Top             =   2280
         Visible         =   0   'False
         Width           =   1332
      End
      Begin VB.ComboBox cboInCon 
         Height          =   288
         Index           =   2
         Left            =   1800
         TabIndex        =   39
         Text            =   "Combo1"
         Top             =   1920
         Visible         =   0   'False
         Width           =   1332
      End
      Begin VB.ComboBox cboInCon 
         Height          =   288
         Index           =   1
         Left            =   1800
         TabIndex        =   35
         Text            =   "Combo1"
         Top             =   1560
         Visible         =   0   'False
         Width           =   1332
      End
      Begin VB.ComboBox cboInLoc 
         Height          =   288
         Index           =   4
         Left            =   3240
         TabIndex        =   48
         Text            =   "Combo1"
         Top             =   2640
         Visible         =   0   'False
         Width           =   1332
      End
      Begin VB.ComboBox cboInLoc 
         Height          =   288
         Index           =   3
         Left            =   3240
         TabIndex        =   44
         Text            =   "Combo1"
         Top             =   2280
         Visible         =   0   'False
         Width           =   1332
      End
      Begin VB.ComboBox cboInLoc 
         Height          =   288
         Index           =   2
         Left            =   3240
         TabIndex        =   40
         Text            =   "Combo1"
         Top             =   1920
         Visible         =   0   'False
         Width           =   1332
      End
      Begin VB.ComboBox cboInLoc 
         Height          =   288
         Index           =   1
         Left            =   3240
         TabIndex        =   36
         Text            =   "Combo1"
         Top             =   1560
         Visible         =   0   'False
         Width           =   1332
      End
      Begin VB.ComboBox cboInSen 
         Height          =   288
         Index           =   4
         Left            =   4680
         TabIndex        =   49
         Text            =   "Combo1"
         Top             =   2640
         Visible         =   0   'False
         Width           =   1332
      End
      Begin VB.ComboBox cboInSen 
         Height          =   288
         Index           =   3
         Left            =   4680
         TabIndex        =   45
         Text            =   "Combo1"
         Top             =   2280
         Visible         =   0   'False
         Width           =   1332
      End
      Begin VB.ComboBox cboInSen 
         Height          =   288
         Index           =   2
         Left            =   4680
         TabIndex        =   41
         Text            =   "Combo1"
         Top             =   1920
         Visible         =   0   'False
         Width           =   1332
      End
      Begin VB.ComboBox cboInSen 
         Height          =   288
         Index           =   1
         Left            =   4680
         TabIndex        =   37
         Text            =   "Combo1"
         Top             =   1560
         Visible         =   0   'False
         Width           =   1332
      End
      Begin VB.ComboBox cboInCon 
         Height          =   288
         Index           =   0
         Left            =   1800
         TabIndex        =   31
         Text            =   "Combo1"
         Top             =   1200
         Width           =   1332
      End
      Begin VB.ComboBox cboInLoc 
         Height          =   288
         Index           =   0
         Left            =   3240
         TabIndex        =   32
         Text            =   "Combo1"
         Top             =   1200
         Width           =   1332
      End
      Begin VB.ComboBox cboInSen 
         Height          =   288
         Index           =   0
         Left            =   4680
         TabIndex        =   33
         Text            =   "Combo1"
         Top             =   1200
         Width           =   1332
      End
      Begin VB.Label lblInTS 
         Caption         =   "Wind Movement:"
         Height          =   252
         Index           =   5
         Left            =   120
         TabIndex        =   117
         Top             =   3000
         Visible         =   0   'False
         Width           =   1692
      End
      Begin VB.Label lblInput 
         Caption         =   "Input(s):"
         Height          =   252
         Left            =   120
         TabIndex        =   112
         Top             =   840
         Width           =   1212
      End
      Begin VB.Label lblInTS 
         Caption         =   "Solar Radiation:"
         Height          =   252
         Index           =   4
         Left            =   120
         TabIndex        =   58
         Top             =   2640
         Visible         =   0   'False
         Width           =   1692
      End
      Begin VB.Label lblInTS 
         Caption         =   "Wind Movement:"
         Height          =   252
         Index           =   3
         Left            =   120
         TabIndex        =   57
         Top             =   2280
         Visible         =   0   'False
         Width           =   1692
      End
      Begin VB.Label lblInTS 
         Caption         =   "Dewpoint Temp:"
         Height          =   252
         Index           =   2
         Left            =   120
         TabIndex        =   56
         Top             =   1920
         Visible         =   0   'False
         Width           =   1692
      End
      Begin VB.Label lblInTS 
         Caption         =   "Input 1:"
         Height          =   252
         Index           =   1
         Left            =   120
         TabIndex        =   55
         Top             =   1560
         Visible         =   0   'False
         Width           =   1692
      End
      Begin VB.Label lblConstit 
         Caption         =   "Constituent"
         Height          =   252
         Left            =   1800
         TabIndex        =   27
         Top             =   240
         Width           =   972
      End
      Begin VB.Label lblLocation 
         Caption         =   "Location"
         Height          =   252
         Left            =   3240
         TabIndex        =   26
         Top             =   240
         Width           =   972
      End
      Begin VB.Label lblScenario 
         Caption         =   "Scenario"
         Height          =   252
         Left            =   4680
         TabIndex        =   25
         Top             =   240
         Width           =   972
      End
      Begin VB.Label lblDsn 
         Caption         =   "DSN"
         Height          =   252
         Left            =   6240
         TabIndex        =   24
         Top             =   240
         Width           =   372
      End
      Begin VB.Label lblInTS 
         Caption         =   "Input 1:"
         Height          =   252
         Index           =   0
         Left            =   120
         TabIndex        =   23
         Top             =   1200
         Width           =   1692
      End
      Begin VB.Label lblOutput 
         Caption         =   "Output:"
         Height          =   252
         Left            =   120
         TabIndex        =   22
         Top             =   480
         Width           =   1212
      End
   End
End
Attribute VB_Name = "frmWDMUComp"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
'Copyright 2000 by AQUA TERRA Consultants
Dim nints&, SLCDLock$(5), lTs As Collection ' nts&, lts() As Timser
'Dim oldcntdsn&
Dim lDesc$
Dim LatDMS!(2), LatFg As Boolean, CoefFg As Boolean, ConstFg As Boolean
Dim DCurve!(1 To 24)
Dim CmpBuf!()
Const QualFg& = 0

Private Sub atxLat_CommitChange(Index As Integer)

    LatDMS(Index) = atxLat(Index).Value

End Sub

Private Sub cboInLoc_Change(Index As Integer)

    'update output location if unique input location specified
    If cboInLoc(Index).Text <> "mult" And cboInLoc(Index).Text <> "ALL" Then
      If optFunction(11).Value = False Or Index = 0 Then
        'don't update output location if selecting adjacent hourly locations for precip disagg
        txtLocOut.Text = cboInLoc(Index).Text
      End If
    End If

End Sub

Private Sub cboInCon_Click(Index As Integer)

    If InStr(SLCDLock(Index), "C") <= 0 Then
      'constituent selection is locked
      SLCDLock(Index) = SLCDLock(Index) & "C"
    End If
    Call UpdateInputTS(Index, "C")

End Sub

Private Sub cboInLoc_Click(Index As Integer)

    If InStr(SLCDLock(Index), "L") <= 0 Then
      'location selection is locked
      SLCDLock(Index) = SLCDLock(Index) & "L"
    End If
    Call UpdateInputTS(Index, "L")
    'update output location if unique input location specified
    If cboInLoc(Index).Text <> "mult" And cboInLoc(Index).Text <> "ALL" Then
      If optFunction(11).Value = False Or Index = 0 Then
        'don't update output location if selecting adjacent hourly locations for precip disagg
        txtLocOut.Text = cboInLoc(Index).Text
      End If
    End If

End Sub

Private Sub cboInSen_Click(Index As Integer)

    If InStr(SLCDLock(Index), "S") <= 0 Then
      'scenario selection is locked
      SLCDLock(Index) = SLCDLock(Index) & "S"
    End If
    Call UpdateInputTS(Index, "S")

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

Private Sub cboInDsn_Change(Index As Integer)

    Call UpdateLatDef
    Call UpdateDates

End Sub

Private Sub cboInDsn_Click(Index As Integer)

  Dim i&, Ind&, cdsn&, saind&, salen&, StrSen$, StrLoc$, StrCon$

  If IsNumeric(cboInDsn(Index).Text) Then
    'find matching TSer
    Ind = 0
    i = 1
    Do While i < lTs.Count
      If lTs(i).Header.id = cboInDsn(Index).Text Then
        'found a match
        Ind = i
        i = lTs.Count
      End If
      i = i + 1
    Loop
    If Ind > 0 Then
      'get S,L,C for selected dsn
      cboInSen(Index).Text = lTs(Ind).Header.Sen
      cboInLoc(Index).Text = lTs(Ind).Header.Loc
      cboInCon(Index).Text = lTs(Ind).Header.Con
      Call UpdateDates
    End If
  End If

End Sub

Private Sub cmdClose_Click()

    Unload frmWDMUComp

End Sub

Private Sub cmdPerform_Click()

  Dim i&, j&, ltu&, nv&, lid&, TmpTyp$, failfg As Boolean
  Dim s$, lMsg$, outdsn&, addedDsn As Boolean
  Dim retc&, Sdt&(5), Edt&(5), JSdt#, JEdt#
  Dim lscen$, llocn$, lcons$, LatDeg!, Coefs!(24), Cnst!, Tolr!
  Dim lints As Collection, lTs As Collection
  Dim CmpTs As ATCclsTserData, DayTs As ATCclsTserData
  Dim lTsFile As ATCclsTserFile

  addedDsn = False
  failfg = False
  MousePointer = ccHourglass
  If Len(txtConOut.Text) = 0 Then 'no constituent name specified
    MsgBox "No Constituent name has been specified." & "Enter a uniqued combination of Scenario, Location, and Constituent to identify this data set.", 48, "WDMUtil Compute Problem"
    txtConOut.SetFocus
    failfg = True
  End If
  If Len(txtLocOut.Text) = 0 And Not failfg Then 'no location name specified
    MsgBox "No Location name has been specified." & "Enter a uniqued combination of Scenario, Location, and Constituent to identify this data set.", 48, "WDMUtil Compute Problem"
    txtLocOut.SetFocus
    failfg = True
  End If
  If Len(txtSenOut.Text) = 0 And Not failfg Then 'no scenario name specified
    MsgBox "No Scenario name has been specified." & "Enter a uniqued combination of Scenario, Location, and Constituent to identify this data set.", 48, "WDMUtil Compute Problem"
    txtSenOut.SetFocus
    failfg = True
  End If
  If atxOutDsn.Value > 0 And atxOutDsn.Value < 10000 Then
    outdsn = atxOutDsn.Value
  ElseIf Not failfg Then 'invalid output data-set number
    MsgBox "A valid output data-set number has not been specified." & vbCrLf & "Enter the number of a data set in the DSN field.", 48, "WDMUtil Compute Problem"
    atxOutDsn.SetFocus
    failfg = True
  End If
  If LatFg And Not failfg Then 'make sure valid latitude entries
    If Not IsNumeric(atxLat(0).Value) Or _
       Not IsNumeric(atxLat(1).Value) Or _
       Not IsNumeric(atxLat(2).Value) Then
      MsgBox "Valid latitude values have not been specified." & vbCrLf & "Enter values for latitude in the fields provided.", 48, "WDMUtil Compute Problem"
      atxLat(0).SetFocus
      failfg = True
    Else
      LatDeg = LatDMS(0) + LatDMS(1) / 60# + LatDMS(2) / 3600#
    End If
  End If
  If CoefFg And Not failfg Then 'make sure coefficients are entered
    For i = 1 To 12
      If Not IsNumeric(atxCoef(i).Value) Then
        failfg = True
      Else
        Coefs(i) = CSng(atxCoef(i).Value)
      End If
    Next i
    If atxCoef(13).Visible Then 'check remaining hourly coefs
      For i = 13 To 24
        If Not IsNumeric(atxCoef(i).Value) Then
          failfg = True
        Else
          Coefs(i) = CSng(atxCoef(i).Value)
        End If
      Next i
    End If
    If failfg Then 'problem w/coefficients
      If atxCoef(13).Visible Then
        MsgBox "Valid Hourly Coefficient values have not been specified." & vbCrLf & "Enter values for each Hourly Coefficient in the fields provided.", 48, "WDMUtil Compute Problem"
      Else
        MsgBox "Valid Monthly Coefficient values have not been specified." & vbCrLf & "Enter values for each Monthly Coefficient in the fields provided.", 48, "WDMUtil Compute Problem"
      End If
      atxCoef(i).SetFocus
    End If
  End If
  If ConstFg And Not failfg Then 'check for valid constant value
    If Not IsNumeric(atxConst.Value) Then
      If optFunction(7).Value = True Then
        MsgBox "A valid Constant Coefficient value has not been specified." & vbCrLf & "Enter a value for the constant in the field provided.", 48, "WDMUtil Compute Problem"
      Else
        MsgBox "A valid Observation Hour has not been specified." & vbCrLf & "Enter a value for the Observation Hour in the field provided.", 48, "WDMUtil Compute Problem"
      End If
      atxConst.SetFocus
      failfg = True
    Else
      Cnst = CSng(atxConst.Value)
    End If
    If Not failfg And atxTolerance.Visible Then
      'check other precip disaggregation fields
      If Not IsNumeric(atxTolerance.Value) Then
        MsgBox "A valid Data Tolerance value has not been specified." & vbCrLf & "Enter a value for the Data Tolerance in the field provided.", 48, "WDMUtil Compute Problem"
        atxTolerance.SetFocus
        failfg = True
      Else
        Tolr = atxTolerance.Value / 100
        If Len(atxSummFile.Value) = 0 Then
          MsgBox "A valid Summary Output File has not been specified." & vbCrLf & "Enter a valid file name in the Summary Output File field provided.", 48, "WDMUtil Compute Problem"
          atxSummFile.SetFocus
          failfg = True
        End If
      End If
    End If
  End If
  If Not failfg Then 'get the data
    Sdt(0) = Year(ctlCompDate.CurrS)
    Sdt(1) = Month(ctlCompDate.CurrS)
    Sdt(2) = Day(ctlCompDate.CurrS)
    Edt(0) = Year(ctlCompDate.CurrE)
    Edt(1) = Month(ctlCompDate.CurrE)
    Edt(2) = Day(ctlCompDate.CurrE)
    Edt(3) = 24
    JSdt = Date2J(Sdt())
    JEdt = Date2J(Edt())
    Set lints = Nothing
    Set lints = New Collection
    i = 0
    Do While i < nints
      If Not IsNumeric(cboInDsn(i).Text) Then 'unique dataset not specified
        'if doing precip disagg, it's ok to have some undefined data sets
        If optFunction(11).Value = False Then
          MsgBox "A unique data set has not been specified for " & lblInTS(i).Caption & "." & vbCrLf & _
                 "Make more detailed selections from the Scenario, Location, or Constituent lists or " & vbCrLf & _
                 "Select a specific data-set number from the DSN list.", 48, "WDMUtil Compute Problem"
          failfg = True
          cboInDsn(i).SetFocus
          i = nints
        Else
          i = i + 1
        End If
      Else
        lscen = cboInSen(i).Text
        llocn = cboInLoc(i).Text
        lcons = cboInCon(i).Text
        Call FindTimSer(lscen, llocn, lcons, lTs)
        For j = 1 To lTs.Count 'this will usually just be one
          If lTs(j).Header.id = CLng(cboInDsn(i).Text) Then
            'need extra day(s) at beginning and/or end for temp disagg
            If optFunction(7).Value = True Then
              If i = 0 Then 'min temp, may need 2 extra days at end
                JEdt = JEdt + 2
              Else 'max temp, may need extra day at beginning and end
                JSdt = JSdt - 1
                JEdt = JEdt - 1
              End If
            End If
            lints.Add lTs(j).SubSetByDate(JSdt, JEdt)
          End If
        Next j
        i = i + 1
        If lints.Count < i Then
          MsgBox "Could not find data set specified for " & lblInTS(i - 1).Caption & ".", 48, "WDMUtil Compute Problem"
          failfg = True
        End If
      End If
    Loop
  End If
  If Not failfg Then
    lscen = UCase(txtSenOut.Text)
    llocn = UCase(txtLocOut.Text)
    lcons = UCase(txtConOut.Text)
    If optForC(0).Visible Then
      If optForC(0).Value = True Then
        TmpTyp = "F"
      ElseIf optForC(1).Value = True Then
        TmpTyp = "C"
      End If
    End If
    If optOperation(0).Value = True Then 'daily compute operation
      ltu = 4
      If optFunction(0).Value = True Then 'solar radiation
        Set CmpTs = CmpSol(lints, LatDeg)
      ElseIf optFunction(1).Value = True Then 'Jensen PET
        Set CmpTs = CmpJen(lints, Coefs(), Cnst, TmpTyp)
      ElseIf optFunction(2).Value = True Then 'Hamon PET
        Set CmpTs = CmpHam(lints, LatDeg, Coefs(), TmpTyp)
      ElseIf optFunction(3).Value = True Then 'Penman Pan Evap
        Set CmpTs = CmpPen(lints)
      ElseIf optFunction(4).Value = True Then 'Wind
        Set CmpTs = CmpWnd(lints)
      ElseIf optFunction(5).Value = True Then 'cloud cover
        Set CmpTs = CmpCld(lints)
      End If
    ElseIf optOperation(1) = True Then 'daily-->hourly disaggregation operation
      ltu = 3
      If optFunction(6).Value = True Then 'solar
        Set CmpTs = DisSolPet(lints, 1, LatDeg)
      ElseIf optFunction(7).Value = True Then 'temperature
        JSdt = Date2J(Sdt())
        JEdt = Date2J(Edt())
        Set CmpTs = DisTemp(lints, CLng(Cnst), JSdt, JEdt)
      ElseIf optFunction(8).Value = True Then 'dewpoint temperature
        Set CmpTs = DisDwpnt(lints)
      ElseIf optFunction(9).Value = True Then 'evap
        Set CmpTs = DisSolPet(lints, 2, LatDeg)
      ElseIf optFunction(10).Value = True Then 'wind
        Set CmpTs = DisWnd(lints, Coefs())
      ElseIf optFunction(11).Value = True Then 'precip
        Set DayTs = lints(1)
        lints.Remove (1)
        Set CmpTs = DisPrecip(DayTs, lints, CLng(Cnst), Tolr, atxSummFile.Value)
      End If
    End If
    s = CmpTs.ErrorDescription
    If Len(s) = 0 Or InStr(s, "WARNING") > 0 Then 'compute/disagg went ok
      If optOperation(0) = True Then
        lMsg = "Compute operation successfully performed." & vbCrLf & "Results will be saved to the specified data set on the WDM file."
      Else
        lMsg = "Disaggregate operation successfully performed." & vbCrLf & "Results will be saved to the specified data set on the WDM file."
      End If
      If Len(s) > 0 Then 'add warning message
        lMsg = lMsg & vbCrLf & vbCrLf & s
      End If
      j = MsgBox(lMsg, vbQuestion + vbOKCancel + vbDefaultButton1, "WDMUtil Compute")
      If j = vbOK Then 'write new data set
        Set lTsFile = TserFiles.Active(2).obj 'WDM assumed to be in 2nd position
        With CmpTs.Header
          .id = outdsn
          .Sen = lscen
          .Con = lcons
          .Loc = llocn
          .Desc = lDesc
        End With
        If lTsFile.addtimser(CmpTs, TsIdReplAsk + TsIdAppendAsk + TsIdRenumAsk) Then
          'output to data set ok
          addedDsn = True
        Else
          MsgBox "Data not saved to WDM file.", vbInformation, "WDMUtil Compute"
        End If
      Else 'don't write it, delete it
        MsgBox "Data not saved to WDM file.", vbInformation, "WDMUtil Compute"
      End If
    ElseIf optOperation(0) = True Then
      MsgBox "A problem occurred during this operation." & vbCrLf & s, 48, "WDMUtil Compute Problem"
    ElseIf optOperation(1) = True Then
      MsgBox "A problem occurred during this operation." & vbCrLf & s, 48, "WDMUtil Disaggregate Problem"
    End If
    If addedDsn = True Then 'update available timeseries
      Call RefreshSLC
      Call frmGenScn.RefreshMain
      Call frmGenScn.SelectAll
    End If
  End If
  MousePointer = ccDefault

End Sub

Private Sub ctlCompDate_LostFocus()

    CSDat(0) = Year(ctlCompDate.CurrS)
    CSDat(1) = Month(ctlCompDate.CurrS)
    CSDat(2) = Day(ctlCompDate.CurrS)
    CEDat(0) = Year(ctlCompDate.CurrE)
    CEDat(1) = Month(ctlCompDate.CurrE)
    CEDat(2) = Day(ctlCompDate.CurrE)
    'tu,ts,dt need to stay constant for daily data retrieval
'    Ctunit = ctlCompDate.TUnit
'    CTStep = ctlCompDate.TStep
'    CDTran = ctlCompDate.TAggr

End Sub

Private Sub Form_Load()

    Dim i&

    If Not (HelpCheck) Then
      optOperation(0).Value = True
'      Call InitInputTS(5)
      txtSenOut.Text = "COMPUTED"
      ctlCompDate.Visible = False
      fraDumCompDates.Visible = True
      lblDumCompDates.Caption = "No Input Data Sets Specified."
      'oldcntdsn = CntDsn 'save original count of data sets
      'set tu,ts,dt for daily data retrieval
      ctunit = 4
      CTStep = 1
      CDTran = 1
      'set initial wind disaggregation coefficients
      DCurve(1) = 0.034
      DCurve(2) = 0.034
      DCurve(3) = 0.034
      DCurve(4) = 0.034
      DCurve(5) = 0.034
      DCurve(6) = 0.034
      DCurve(7) = 0.034
      DCurve(8) = 0.035
      DCurve(9) = 0.037
      DCurve(10) = 0.041
      DCurve(11) = 0.046
      DCurve(12) = 0.05
      DCurve(13) = 0.053
      DCurve(14) = 0.054
      DCurve(15) = 0.058
      DCurve(16) = 0.057
      DCurve(17) = 0.056
      DCurve(18) = 0.05
      DCurve(19) = 0.043
      DCurve(20) = 0.04
      DCurve(21) = 0.038
      DCurve(22) = 0.036
      DCurve(23) = 0.036
      DCurve(24) = 0.035
    End If

End Sub

Private Sub optFunction_Click(Index As Integer)

  Dim i&, yp&, caps$(5), MonCoefDef!

  MousePointer = ccHourglass
  If Index = 0 Then 'compute solar radiation
    nints = 1
    caps(0) = "Cloud Cover:"
    lblFunction(0).Caption = "Compute Daily Solar Radiation (langleys) from cloud cover time series (tenths, i.e. 0 - 10) and latitude (d, m, s)."
    txtConOut.Text = "DSOL"
    lDesc = "computed daily solar radiation"
  ElseIf Index = 1 Then 'compute Jensen PET
    nints = 3
    caps(0) = "Min Air Temp:"
    caps(1) = "Max Air Temp:"
    caps(2) = "Solar Radiation:"
    lblFunction(0).Caption = "Compute Daily PET (in) using a constant and monthly coefficients and time series for min and max air temperature (F) and solar radiation (langleys)."
    txtConOut.Text = "DEVT"
    lDesc = "computed daily PET (in)"
  ElseIf Index = 2 Then 'compute Hamon PET
    nints = 2
    caps(0) = "Min Air Temp:"
    caps(1) = "Max Air Temp:"
    lblFunction(0).Caption = "Compute Daily PET (in) using monthly coefficients, latitude (d,m,s) and time series for min and max air temperature (F or C)."
    txtConOut.Text = "DEVT"
    lDesc = "computed daily PET (in)"
  ElseIf Index = 3 Then 'compute Penman Pan Evap
    nints = 5
    caps(0) = "Min Air Temp:"
    caps(1) = "Max Air Temp:"
    caps(2) = "Dewpoint Temp:"
    caps(3) = "Wind Movement:"
    caps(4) = "Solar Radiation:"
    lblFunction(0).Caption = "Compute Daily Pan Evap (in) using time series for min/max air temp. (F), dewpoint temp. (F), wind movement (miles), and solar radiation (langleys)."
    txtConOut.Text = "DEVP"
    lDesc = "computed daily pan evaporation (in)"
  ElseIf Index = 4 Then 'compute wind travel
    nints = 1
    caps(0) = "Wind Speed:"
    lblFunction(0).Caption = "Compute Total Daily Wind Travel (miles) from time series of average daily wind speed (miles/hr)."
    txtConOut.Text = "TWND"
    lDesc = "computed total daily wind travel"
  ElseIf Index = 5 Then 'compute % cloud cover
    nints = 1
    caps(0) = "Percent Sun:"
    lblFunction(0).Caption = "Compute Daily Percent Cloud Cover from time series of percent sun."
    txtConOut.Text = "DCLO"
    lDesc = "computed daily percent cloud cover"
  ElseIf Index = 6 Then 'disagg solar radiation
    nints = 1
    caps(0) = "Solar Radiation:"
    lblFunction(1).Caption = "Disaggregate Daily Solar Radiation to Hourly using an empirical distribution based on latitude (recommended for latitudes from 25 deg N to 50 deg N)."
    txtConOut.Text = "SOLR"
    lDesc = "disaggregated solar radiation (daily to hourly)"
  ElseIf Index = 7 Then 'disagg temperature
    nints = 2
    caps(0) = "Min Air Temp:"
    caps(1) = "Max Air Temp:"
    lblFunction(1).Caption = "Disaggregate Daily Min and Max Air Temperature to Hourly Air Temperature (assumes min temp at 6 AM and max temp at 4 PM)."
    txtConOut.Text = "ATEM"
    lDesc = "disaggregated air temperature (daily to hourly)"
  ElseIf Index = 8 Then 'disagg dewpoint temperature
    nints = 1
    caps(0) = "Dewpoint Temp:"
    lblFunction(1).Caption = "Disaggregate Daily Dewpoint Temperature (F or C) to Hourly (assumes daily average is constant for 24 hours)."
    txtConOut.Text = "DEWP"
    lDesc = "disaggregated dewpoint temperature (daily to hourly)"
  ElseIf Index = 9 Then 'disagg evap
    nints = 1
    caps(0) = "Potential ET:"
    lblFunction(1).Caption = "Disaggregate Daily PET (in or cm) to Hourly (assumes a distribution based on latitude (d,m,s) and time of year)."
    txtConOut.Text = "PEVT"
    lDesc = "disaggregated PET (daily to hourly)"
  ElseIf Index = 10 Then 'disagg wind travel
    nints = 1
    caps(0) = "Wind Movement:"
    lblFunction(1).Caption = "Disaggregate Daily Wind Movement (any units) to Hourly Wind Speed (same units as daily) using an empirical hourly distribution."
    txtConOut.Text = "WIND"
    lDesc = "disaggregated wind movement (daily to hourly)"
  ElseIf Index = 11 Then 'disagg precipitation
    nints = 6
    caps(0) = "Daily Precip:"
    caps(1) = "Hourly Precip:"
    caps(2) = "Hourly Precip:"
    caps(3) = "Hourly Precip:"
    caps(4) = "Hourly Precip:"
    caps(5) = "Hourly Precip:"
    lblFunction(1).Caption = "Disaggregate Daily Precipitation using anywhere from 1 to 5 hourly precipitation data sets."
    txtConOut.Text = "PREC"
    lDesc = "disaggregated precipitation (daily to hourly)"
  End If
  Call InitInputTS(nints)
  Call EnableInputTS(nints, caps)
  If (Index >= 3 And Index <= 5) Or Index = 8 Then
    'don't need any extra inputs (just time series)
    fraExtras.Visible = False
    ctlCompDate.Top = fraTimeseries.Top + fraTimeseries.Height + 108
    LatFg = False
  Else
    'need extra inputs
    fraExtras.Top = fraTimeseries.Top + fraTimeseries.Height + 108
    If Index = 0 Or Index = 2 Or Index = 6 Or Index = 9 Then
      'latitude needed
      LatFg = True
      Call UpdateLatDef
    Else
      LatFg = False
    End If
    lblLat.Visible = LatFg
    For i = 0 To 2
      atxLat(i).Visible = LatFg
    Next i
    'assume not needing constant controls
    lblConst.Visible = False
    atxConst.Visible = False
    lblTolerance.Visible = False
    atxTolerance.Visible = False
    lblSummFile.Visible = False
    atxSummFile.Visible = False
    'assume don't need units for temp data
    lblForC.Visible = False
    optForC(0).Visible = False
    optForC(1).Visible = False
    'set position of controls
    yp = 480
    If Index = 1 Or Index = 2 Then 'need monthly coeffs
      CoefFg = True
      If Index = 1 Then 'need constant also
        MonCoefDef = 0.012
      Else 'put under latitude control
        MonCoefDef = 0.0055
      End If
      'also need units for temp data
      lblForC.Visible = True
      optForC(0).Visible = True
      optForC(1).Visible = True
    ElseIf Index = 10 Then 'need hourly coefficients
      CoefFg = True
      yp = 200
    Else 'don't need monthly coeffs
      CoefFg = False
    End If
    If CoefFg Then 'set position
      lblCoef(0).Top = yp + 40
      yp = yp + 480
    End If
    lblCoef(0).Visible = CoefFg
    For i = 1 To 12
      If CoefFg Then 'set position
        lblCoef(i).Top = yp - 240
        atxCoef(i).Top = yp
        atxCoef(i).DefaultValue = MonCoefDef
      End If
      lblCoef(i).Visible = CoefFg
      atxCoef(i).Visible = CoefFg
    Next i
    If Index = 10 Then 'need hourly coefficient values
      For i = 1 To 12
        atxCoef(i).Top = lblCoef(i).Top
        atxCoef(i).Value = DCurve(i)
      Next i
      For i = 13 To 24
        atxCoef(i).Top = atxCoef(1).Top + 360
        atxCoef(i).Visible = True
        atxCoef(i).Value = DCurve(i)
      Next i
      yp = yp + 160
      lblCoef(0).Caption = "Hourly distribution fractions:"
      lblHour(0).Visible = True
      lblHour(1).Visible = True
    ElseIf lblHour(0).Visible = True Then 'hide hourly coeffs
      For i = 13 To 24
        atxCoef(i).Visible = False
      Next i
      lblHour(0).Visible = False
      lblHour(1).Visible = False
      lblCoef(0).Caption = "Monthly Coefficients:"
    End If
    If Index = 1 Or Index = 7 Or Index = 11 Then 'need constant entry
      ConstFg = True
      lblConst.Visible = True
      atxConst.Visible = True
      atxConst.Left = lblConst.Left + lblConst.Width
      If Index = 1 Then 'Jensen coefficient
        lblConst.Caption = "Constant Coefficient:"
        atxConst.HardMin = -999
        atxConst.HardMax = -999
      Else 'obs hour
        lblConst.Caption = "Observation Hour:"
        atxConst.HardMin = 1
        atxConst.HardMax = 24
      End If
    Else
      ConstFg = False
    End If
    If Index = 11 Then 'precip disaggregation
      lblTolerance.Visible = True
      atxTolerance.Visible = True
      lblTolerance.Top = atxConst.Top
      atxTolerance.Top = atxConst.Top
      lblSummFile.Visible = True
      atxSummFile.Visible = True
      yp = yp + 160
    End If
    fraExtras.Height = yp + 320
    fraExtras.Visible = True
    ctlCompDate.Top = fraExtras.Top + fraExtras.Height + 108
  End If
  fraDumCompDates.Top = ctlCompDate.Top
  cmdPerform.Top = ctlCompDate.Top + ctlCompDate.Height + 108
  cmdClose.Top = cmdPerform.Top
  frmWDMUComp.Height = cmdClose.Top + cmdClose.Height + 450
  MousePointer = ccDefault

End Sub
Private Sub EnableInputTS(nints&, caps$())

    Dim i%, j&, lcon$
    Static Init&, TopPos&

    fraTimeseries.Visible = True
    If Init = 0 Then 'set default start pos for input specs
      TopPos = lblInTS(0).Top
      Init = 1
    End If
    For i = 0 To nints - 1
      lblInTS(i).Visible = True
      lblInTS(i).Caption = caps(i)
      cboInSen(i).Visible = True
      cboInLoc(i).Visible = True
      cboInCon(i).Visible = True
      cboInDsn(i).Visible = True
      If i = 0 Then 'check positioning for 1st input specs
        If caps(i) = "Daily Precip:" Then
          'adjust daily precip input specs up to isolate them
          lblInTS(i).Top = TopPos - 120
        Else 'use default position
          lblInTS(i).Top = TopPos
        End If
        lblInTS(i).Top = lblInTS(i).Top
        cboInCon(i).Top = lblInTS(i).Top
        cboInSen(i).Top = lblInTS(i).Top
        cboInLoc(i).Top = lblInTS(i).Top
        cboInDsn(i).Top = lblInTS(i).Top
      End If
      Select Case caps(i)
        Case "Cloud Cover:": lcon = "DCLO"
        Case "Min Air Temp:": lcon = "TMIN"
        Case "Max Air Temp:": lcon = "TMAX"
        Case "Solar Radiation:": lcon = "DSOL"
        Case "Dewpoint Temp:": lcon = "DPTP"
        Case "Wind Movement:": lcon = "DWND"
        Case "Potential ET:": lcon = "DEVT"
        Case "Daily Precip:": lcon = "DPRC"
        Case "Hourly Precip:": lcon = "PREC"
      End Select
      If InList(lcon, cboInCon(i)) Then
        For j = 0 To cboInCon(i).ListCount - 1
          If cboInCon(i).List(j) = lcon Then
            cboInCon(i).ListIndex = j
            Exit For
          End If
        Next j
      End If
    Next i
    For i = nints To 4
      lblInTS(i).Visible = False
      cboInSen(i).Visible = False
      cboInLoc(i).Visible = False
      cboInCon(i).Visible = False
      cboInDsn(i).Visible = False
    Next i
    fraTimeseries.Height = cboInSen(nints - 1).Top + cboInSen(nints - 1).Height + 80

End Sub

Private Sub UpdateDates()

    Dim i&, j&, k&, l&, d$, NoDates As Boolean, Sdt&(5), Edt&(5)
    Dim Strt&(), Stp&(), retcod&, lnts&

    ReDim Strt(6 * nints)
    ReDim Stp(6 * nints)

'    NoDates = False
    lnts = 0
    NoDates = True
    For i = 0 To nints - 1
      If IsNumeric(cboInDsn(i).Text) Then
        If CLng(cboInDsn(i).Text) > 0 Then 'valid dsn
          j = 1
          Do While j <= lTs.Count
            If lTs(j).Header.id = CLng(cboInDsn(i).Text) Then
              Call J2Date(lTs(j).dates.Summary.SJDay, Sdt())
              Call J2Date(lTs(j).dates.Summary.EJDay, Edt())
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
      If lnts > 1 Then 'nints > 1 Then
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
      ctlCompDate.CurrS = DateSerial(CSDat(0), CSDat(1), CSDat(2))
      ctlCompDate.CurrE = DateSerial(CEDat(0), CEDat(1), CEDat(2))
      If optFunction(11).Value = True And _
         IsNumeric(cboInDsn(0).Text) Then 'use daily TSer for available period
        ctlCompDate.LimtS = DateSerial(Strt(0), Strt(1), Strt(2))
        ctlCompDate.LimtE = DateSerial(Stp(0), Stp(1), Stp(2))
      Else
        ctlCompDate.LimtS = DateSerial(CSDat(0), CSDat(1), CSDat(2))
        ctlCompDate.LimtE = DateSerial(CEDat(0), CEDat(1), CEDat(2))
      End If
      ctlCompDate.CommS = ctlCompDate.LimtS
      ctlCompDate.CommE = ctlCompDate.LimtE
      If retcod < 0 Then 'no common period found
        lblDumCompDates.Caption = "No Common Period for Selected Timeseries"
        NoDates = True
      ElseIf CSDat(0) = 0 And CEDat(0) = 0 Then
        'no data in these data sets
        lblDumCompDates.Caption = "No Data Available in Selected Timeseries."
        NoDates = True
      End If
    Else 'dsns not specified, no dates to display
      lblDumCompDates.Caption = "No Input Data Sets Specified."
    End If
    ctlCompDate.Width = fraDumCompDates.Width
    'assume no dates being displayed;
    'making them visible will refresh dates if needed
    ctlCompDate.Visible = False
    If NoDates Then
      fraDumCompDates.Visible = True
    Else
      ctlCompDate.Visible = True
      fraDumCompDates.Visible = False
    End If

End Sub

Private Sub UpdateLatDef()

    Dim i%, j%, cdsn&, ldsn&(4), ILat&(1 To 3)

    j = 0
    For i = 0 To 4
      ldsn(j) = 0
      If cboInDsn(i).Visible And IsNumeric(cboInDsn(i).Text) > 0 Then
        cdsn = CLng(cboInDsn(i).Text)
        If F90_WDCKDT(p.WDMFiles(1).FileUnit, cdsn) = 1 Then 'valid ts data set
          ldsn(j) = cdsn
          j = j + 1
        End If
      End If
    Next i
    If j > 0 Then 'check input ts dsns for latitude attribute values
      Call GetLatDef(ldsn(), ILat())
      If ILat(1) > -999 And ILat(2) > -999 And ILat(3) > -999 Then
        For i = 0 To 2
          atxLat(i).Value = ILat(i + 1)
        Next i
      End If
    End If

End Sub

Public Sub GetLatDef(ldsn&(), ILat&())

    'determine if latitude exists in target dsn
    'in either LATDMS or LATDEG format.

    Dim i%, saind&, salen&, retcod&, LatDMS&, LatDeg!

    ILat(1) = -999
    ILat(2) = -999
    ILat(3) = -999
    i = 0
    Do While i <= UBound(ldsn)
      If ldsn(i) > 0 Then
        salen = 1
        'get latitude (LATDMS) from input dsn if available
        saind = 54
        Call F90_WDBSGI(p.WDMFiles(1).FileUnit, ldsn(i), saind, salen, LatDMS, retcod)
        If retcod = 0 Then
          'we got LATDMS
          LatDeg = CNVTDG(LatDMS)
          Call CNVTLT(LatDeg, ILat)
        Else
          'LATDMS not available,
          'get latitude (LATDEG) from input dsn if available
          saind = 8
          Call F90_WDBSGR(p.WDMFiles(1).FileUnit, ldsn(i), saind, salen, LatDeg, retcod)
          If retcod = 0 Then
            'we got LATDEG
            Call CNVTLT(LatDeg, ILat)
          End If
        End If
        If ILat(1) >= 0 Then 'got a valid latitude value, use it
          i = UBound(ldsn)
        End If
      Else 'no more data sets in ldsn
        i = UBound(ldsn)
      End If
      i = i + 1
    Loop

End Sub

Public Function CNVTDG(LATLNG&)

    'Computes latitude or longitude in degrees from
    'an integer representation of degrees-minutes-seconds.
    'Examples:
               '593000 is converted to 59.5
               '-723015 is converted to -72.5042

    Dim DEG&, Min&, SEC&

    DEG = LATLNG / 10000
    Min = (LATLNG - DEG * 10000) / 100
    SEC = LATLNG - DEG * 10000 - Min * 100

    CNVTDG = CSng(DEG) + CSng(Min) / 60# + CSng(SEC) / 3600#

End Function

Public Sub CNVTLT(LatDeg!, ILat&())

    'routine to convert decimal degrees to degrees-minutes-seconds
    'and return the results in the ILAT(3) integer array

    Dim x!

    ILat(1) = LatDeg
    ILat(2) = (LatDeg - CSng(ILat(1))) * 60
    x = LatDeg - (CSng(ILat(1)) + (CSng(ILat(2)) / 60#))
    ILat(3) = Fix(x * 3600)

End Sub

Private Sub optOperation_Click(Index As Integer)

    Dim i&

    fraFunction(Index).Visible = True
    If Index = 0 Then 'compute
      'which function?
      i = 0
      Do While i < 6
        If optFunction(i).Value = True Then
          optFunction_Click (i)
          i = 6
        Else
          i = i + 1
        End If
      Loop
      fraFunction(1).Visible = False
      'set time units to days
      ctlCompDate.TUnit = 4
      ctlCompDate.TSTEP = 1
    ElseIf Index = 1 Then 'disaggregate
      'which function?
      i = 6
      Do While i < optFunction.Count
        If optFunction(i).Value = True Then
          optFunction_Click (i)
          i = optFunction.Count
        Else
          i = i + 1
        End If
      Loop
      fraFunction(0).Visible = False
      'set time units to hourly
      ctlCompDate.TUnit = 3
      ctlCompDate.TSTEP = 1
    End If

End Sub

Private Sub InitInputTS(lnt&)
'init input time series specification lists

  Dim i&, j&

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
    cboInSen(i).Text = ""
    cboInLoc(i).Text = ""
    cboInCon(i).Text = ""
    cboInDsn(i).Text = ""
    SLCDLock(i) = ""
  Next i

End Sub
