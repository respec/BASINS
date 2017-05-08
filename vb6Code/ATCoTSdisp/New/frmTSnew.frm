VERSION 5.00
Begin VB.Form frmTSnew 
   Caption         =   "New Time Series"
   ClientHeight    =   6135
   ClientLeft      =   45
   ClientTop       =   270
   ClientWidth     =   10440
   HelpContextID   =   920
   Icon            =   "frmTSnew.frx":0000
   LinkTopic       =   "Form1"
   ScaleHeight     =   6135
   ScaleWidth      =   10440
   StartUpPosition =   3  'Windows Default
   Begin VB.Frame fraTab 
      BorderStyle     =   0  'None
      Height          =   3252
      Index           =   2
      Left            =   3000
      TabIndex        =   95
      Top             =   2880
      Width           =   5052
      Begin VB.ComboBox cboAddRemoveTser 
         Height          =   288
         Left            =   0
         TabIndex        =   7
         Top             =   360
         Width           =   5052
      End
      Begin VB.ComboBox cboAddRemoveTunits 
         Height          =   288
         ItemData        =   "frmTSnew.frx":0442
         Left            =   2100
         List            =   "frmTSnew.frx":0458
         Style           =   2  'Dropdown List
         TabIndex        =   10
         Top             =   720
         Width           =   1152
      End
      Begin VB.TextBox txtAddRemoveTstep 
         Alignment       =   1  'Right Justify
         Height          =   310
         Left            =   1680
         TabIndex        =   9
         Text            =   "1"
         Top             =   720
         Width           =   432
      End
      Begin VB.OptionButton optBaseTser 
         Caption         =   "Specify Interval"
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
         Left            =   0
         TabIndex        =   8
         Top             =   770
         Width           =   1812
      End
      Begin VB.OptionButton optBaseTser 
         Caption         =   "Base on existing time series"
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
         Left            =   0
         TabIndex        =   6
         Top             =   120
         Value           =   -1  'True
         Width           =   3492
      End
      Begin VB.Frame Frame1 
         Caption         =   "Added Values"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   1572
         Left            =   0
         TabIndex        =   106
         Top             =   1560
         Width           =   4932
         Begin VB.TextBox txtCloneDat 
            Alignment       =   1  'Right Justify
            Height          =   288
            Index           =   4
            Left            =   4416
            MaxLength       =   2
            TabIndex        =   31
            Text            =   "00"
            Top             =   1025
            Width           =   272
         End
         Begin VB.TextBox txtCloneDat 
            Alignment       =   1  'Right Justify
            Height          =   288
            Index           =   3
            Left            =   4152
            MaxLength       =   2
            TabIndex        =   30
            Text            =   "00"
            Top             =   1025
            Width           =   272
         End
         Begin VB.TextBox txtCloneDat 
            Alignment       =   1  'Right Justify
            Height          =   288
            Index           =   2
            Left            =   3900
            MaxLength       =   2
            TabIndex        =   29
            Text            =   "00"
            Top             =   1025
            Width           =   272
         End
         Begin VB.TextBox txtInterp 
            Alignment       =   1  'Right Justify
            Height          =   288
            Left            =   3960
            MaxLength       =   4
            TabIndex        =   25
            Text            =   "0"
            Top             =   720
            Width           =   452
         End
         Begin VB.OptionButton optNewValues 
            Caption         =   "Copy nearest existing value"
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
            TabIndex        =   23
            Top             =   516
            Width           =   2892
         End
         Begin VB.OptionButton optNewValues 
            Caption         =   "Interpolate from nearest existing value to"
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
            TabIndex        =   24
            Top             =   756
            Width           =   3852
         End
         Begin VB.TextBox txtCloneDat 
            Alignment       =   1  'Right Justify
            Height          =   288
            Index           =   0
            Left            =   3168
            MaxLength       =   4
            TabIndex        =   27
            Text            =   "1997"
            Top             =   1025
            Width           =   452
         End
         Begin VB.TextBox txtCloneDat 
            Alignment       =   1  'Right Justify
            Height          =   288
            Index           =   1
            Left            =   3636
            MaxLength       =   2
            TabIndex        =   28
            Text            =   "00"
            Top             =   1025
            Width           =   272
         End
         Begin VB.TextBox txtNewVal 
            Alignment       =   1  'Right Justify
            Height          =   288
            Left            =   2160
            MaxLength       =   4
            TabIndex        =   22
            Text            =   "0"
            Top             =   240
            Width           =   452
         End
         Begin VB.OptionButton optNewValues 
            Caption         =   "Set added values to"
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
            TabIndex        =   21
            Top             =   276
            Value           =   -1  'True
            Width           =   2532
         End
         Begin VB.OptionButton optNewValues 
            Caption         =   "Clone existing values starting at"
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
            TabIndex        =   26
            Top             =   996
            Width           =   3252
         End
      End
      Begin VB.TextBox txtStartDat 
         Alignment       =   1  'Right Justify
         Height          =   288
         Index           =   4
         Left            =   2184
         MaxLength       =   2
         TabIndex        =   15
         Text            =   "00"
         Top             =   1140
         Width           =   272
      End
      Begin VB.TextBox txtStartDat 
         Alignment       =   1  'Right Justify
         Height          =   288
         Index           =   3
         Left            =   1920
         MaxLength       =   2
         TabIndex        =   14
         Text            =   "00"
         Top             =   1140
         Width           =   272
      End
      Begin VB.TextBox txtStartDat 
         Alignment       =   1  'Right Justify
         Height          =   288
         Index           =   2
         Left            =   1666
         MaxLength       =   2
         TabIndex        =   13
         Text            =   "00"
         Top             =   1140
         Width           =   272
      End
      Begin VB.TextBox txtStartDat 
         Alignment       =   1  'Right Justify
         Height          =   288
         Index           =   1
         Left            =   1404
         MaxLength       =   2
         TabIndex        =   12
         Text            =   "00"
         Top             =   1140
         Width           =   272
      End
      Begin VB.TextBox txtEndDat 
         Alignment       =   1  'Right Justify
         Height          =   288
         Index           =   4
         Left            =   4704
         MaxLength       =   2
         TabIndex        =   20
         Text            =   "00"
         Top             =   1140
         Width           =   272
      End
      Begin VB.TextBox txtEndDat 
         Alignment       =   1  'Right Justify
         Height          =   288
         Index           =   3
         Left            =   4440
         MaxLength       =   2
         TabIndex        =   19
         Text            =   "00"
         Top             =   1140
         Width           =   272
      End
      Begin VB.TextBox txtStartDat 
         Alignment       =   1  'Right Justify
         Height          =   288
         Index           =   0
         Left            =   936
         MaxLength       =   4
         TabIndex        =   11
         Text            =   "1997"
         Top             =   1140
         Width           =   452
      End
      Begin VB.TextBox txtEndDat 
         Alignment       =   1  'Right Justify
         Height          =   288
         Index           =   2
         Left            =   4188
         MaxLength       =   2
         TabIndex        =   18
         Text            =   "00"
         Top             =   1140
         Width           =   272
      End
      Begin VB.TextBox txtEndDat 
         Alignment       =   1  'Right Justify
         Height          =   288
         Index           =   1
         Left            =   3924
         MaxLength       =   2
         TabIndex        =   17
         Text            =   "00"
         Top             =   1140
         Width           =   272
      End
      Begin VB.TextBox txtEndDat 
         Alignment       =   1  'Right Justify
         Height          =   288
         Index           =   0
         Left            =   3456
         MaxLength       =   4
         TabIndex        =   16
         Text            =   "1997"
         Top             =   1140
         Width           =   452
      End
      Begin VB.Label lblDate 
         BackStyle       =   0  'Transparent
         Caption         =   "New Start"
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
         Left            =   0
         TabIndex        =   85
         Top             =   1200
         WhatsThisHelpID =   16
         Width           =   852
         WordWrap        =   -1  'True
      End
      Begin VB.Label lblDate 
         BackStyle       =   0  'Transparent
         Caption         =   "New End"
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
         Left            =   2640
         TabIndex        =   86
         Top             =   1200
         WhatsThisHelpID =   16
         Width           =   972
         WordWrap        =   -1  'True
      End
   End
   Begin VB.Frame fraTab 
      BorderStyle     =   0  'None
      Caption         =   "Frame1"
      Height          =   1812
      Index           =   3
      Left            =   8280
      TabIndex        =   97
      Top             =   4200
      Width           =   3732
      Begin VB.TextBox txtOldDat 
         Alignment       =   1  'Right Justify
         BackColor       =   &H8000000F&
         Height          =   288
         Index           =   4
         Left            =   2184
         Locked          =   -1  'True
         MaxLength       =   2
         TabIndex        =   93
         Text            =   "00"
         Top             =   720
         Width           =   272
      End
      Begin VB.TextBox txtOldDat 
         Alignment       =   1  'Right Justify
         BackColor       =   &H8000000F&
         Height          =   288
         Index           =   3
         Left            =   1920
         Locked          =   -1  'True
         MaxLength       =   2
         TabIndex        =   92
         Text            =   "00"
         Top             =   720
         Width           =   272
      End
      Begin VB.TextBox txtOldDat 
         Alignment       =   1  'Right Justify
         BackColor       =   &H8000000F&
         Height          =   288
         Index           =   2
         Left            =   1666
         Locked          =   -1  'True
         MaxLength       =   2
         TabIndex        =   91
         Text            =   "00"
         Top             =   720
         Width           =   272
      End
      Begin VB.TextBox txtOldDat 
         Alignment       =   1  'Right Justify
         BackColor       =   &H8000000F&
         Height          =   288
         Index           =   0
         Left            =   936
         Locked          =   -1  'True
         MaxLength       =   4
         TabIndex        =   89
         Text            =   "1997"
         Top             =   720
         Width           =   452
      End
      Begin VB.TextBox txtOldDat 
         Alignment       =   1  'Right Justify
         BackColor       =   &H8000000F&
         Height          =   288
         Index           =   1
         Left            =   1404
         Locked          =   -1  'True
         MaxLength       =   2
         TabIndex        =   90
         Text            =   "00"
         Top             =   720
         Width           =   272
      End
      Begin VB.ComboBox cboShiftTser 
         Height          =   288
         Left            =   0
         TabIndex        =   32
         Top             =   360
         Width           =   3732
      End
      Begin VB.TextBox txtShiftDat 
         Alignment       =   1  'Right Justify
         Height          =   288
         Index           =   4
         Left            =   2184
         MaxLength       =   2
         TabIndex        =   37
         Text            =   "00"
         Top             =   1140
         Width           =   272
      End
      Begin VB.TextBox txtShiftDat 
         Alignment       =   1  'Right Justify
         Height          =   288
         Index           =   3
         Left            =   1920
         MaxLength       =   2
         TabIndex        =   36
         Text            =   "00"
         Top             =   1140
         Width           =   272
      End
      Begin VB.TextBox txtShiftDat 
         Alignment       =   1  'Right Justify
         Height          =   288
         Index           =   2
         Left            =   1666
         MaxLength       =   2
         TabIndex        =   35
         Text            =   "00"
         Top             =   1140
         Width           =   272
      End
      Begin VB.TextBox txtShiftDat 
         Alignment       =   1  'Right Justify
         Height          =   288
         Index           =   1
         Left            =   1404
         MaxLength       =   2
         TabIndex        =   34
         Text            =   "00"
         Top             =   1140
         Width           =   272
      End
      Begin VB.TextBox txtShiftDat 
         Alignment       =   1  'Right Justify
         Height          =   288
         Index           =   0
         Left            =   936
         MaxLength       =   4
         TabIndex        =   33
         Text            =   "1997"
         Top             =   1140
         Width           =   452
      End
      Begin VB.Label lblOldStart 
         BackStyle       =   0  'Transparent
         Caption         =   "Old Start"
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
         Left            =   0
         TabIndex        =   88
         Top             =   780
         WhatsThisHelpID =   16
         Width           =   852
         WordWrap        =   -1  'True
      End
      Begin VB.Label lblTimser 
         Caption         =   "Base on existing time series"
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
         Left            =   0
         TabIndex        =   87
         Top             =   120
         Width           =   2412
      End
      Begin VB.Label lblDate 
         BackStyle       =   0  'Transparent
         Caption         =   "New Start"
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
         Left            =   0
         TabIndex        =   101
         Top             =   1200
         WhatsThisHelpID =   16
         Width           =   852
         WordWrap        =   -1  'True
      End
   End
   Begin VB.Frame fraTab 
      BorderStyle     =   0  'None
      Height          =   3012
      Index           =   5
      Left            =   7800
      TabIndex        =   102
      Top             =   0
      Width           =   5772
      Begin VB.Frame fraTableButtons 
         BorderStyle     =   0  'None
         Caption         =   "Frame2"
         Height          =   252
         Left            =   3960
         TabIndex        =   107
         Top             =   960
         Width           =   1812
         Begin VB.CommandButton cmdTableSave 
            Caption         =   "&Save"
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
            Left            =   960
            TabIndex        =   66
            Top             =   0
            Width           =   852
         End
         Begin VB.CommandButton cmdTableLoad 
            Caption         =   "&Load"
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
            Left            =   0
            TabIndex        =   65
            Top             =   0
            Width           =   852
         End
      End
      Begin VB.ComboBox cboTableTser 
         Height          =   288
         Left            =   0
         TabIndex        =   62
         Top             =   360
         Width           =   5772
      End
      Begin VB.OptionButton optTable 
         Caption         =   "Interpolate between new values"
         Height          =   252
         Index           =   1
         Left            =   240
         TabIndex        =   64
         Top             =   960
         Width           =   3252
      End
      Begin VB.OptionButton optTable 
         Caption         =   "New values are constant in each range"
         Height          =   252
         Index           =   0
         Left            =   240
         TabIndex        =   63
         Top             =   720
         Value           =   -1  'True
         Width           =   3252
      End
      Begin ATCoCtl.ATCoGrid agdTable 
         Height          =   1572
         Left            =   0
         TabIndex        =   67
         Top             =   1320
         Width           =   5772
         _ExtentX        =   10186
         _ExtentY        =   2778
         SelectionToggle =   0   'False
         AllowBigSelection=   -1  'True
         AllowEditHeader =   0   'False
         AllowLoad       =   0   'False
         AllowSorting    =   0   'False
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
         Header          =   "Type a constant or select a behavior for each range"
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
      Begin VB.Label lblTimser 
         Caption         =   "Base on existing time series"
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
         Left            =   0
         TabIndex        =   94
         Top             =   120
         Width           =   2412
      End
   End
   Begin VB.Frame fraTab 
      BorderStyle     =   0  'None
      Caption         =   "Frame1"
      Height          =   3015
      Index           =   4
      Left            =   5760
      TabIndex        =   103
      Top             =   1560
      Width           =   4812
      Begin VB.OptionButton optFunction 
         Caption         =   "Min"
         Height          =   252
         Index           =   18
         Left            =   4200
         TabIndex        =   53
         Tag             =   "T = Min(arg1, arg2, ...)"
         Top             =   120
         Width           =   732
      End
      Begin VB.TextBox txtRunningAfter 
         Alignment       =   1  'Right Justify
         Height          =   288
         Left            =   2640
         TabIndex        =   58
         Text            =   "0"
         Top             =   1120
         Width           =   315
      End
      Begin VB.TextBox txtRunningBefore 
         Alignment       =   1  'Right Justify
         Height          =   288
         Left            =   840
         TabIndex        =   56
         Text            =   "all"
         Top             =   1120
         Width           =   315
      End
      Begin VB.CheckBox chkRunning 
         Caption         =   "Include"
         Height          =   255
         Left            =   0
         TabIndex        =   55
         Top             =   1120
         Width           =   975
      End
      Begin VB.OptionButton optFunction 
         Caption         =   "Geometric Mean"
         Height          =   255
         Index           =   20
         Left            =   2640
         TabIndex        =   50
         Tag             =   "T = Antilog( (log(arg1) + log(arg2) + ... + log(argN)) /N)"
         Top             =   120
         Width           =   1695
      End
      Begin VB.OptionButton optFunction 
         Caption         =   "Log e"
         Height          =   252
         Index           =   14
         Left            =   1560
         TabIndex        =   47
         Tag             =   "T = ln(arg); If arg <=0, then T=10E35"
         Top             =   360
         Width           =   852
      End
      Begin VB.OptionButton optFunction 
         Caption         =   "Log 10"
         Height          =   252
         Index           =   13
         Left            =   1560
         TabIndex        =   46
         Tag             =   "T = log(arg); If arg <=0, then T=10E35"
         Top             =   120
         Width           =   852
      End
      Begin VB.OptionButton optFunction 
         Caption         =   "e**"
         Height          =   252
         Index           =   12
         Left            =   1560
         TabIndex        =   48
         Tag             =   "T = e ** arg            (alternate form T =e^arg)"
         Top             =   600
         Width           =   732
      End
      Begin VB.OptionButton optFunction 
         Caption         =   "Exponent"
         Height          =   252
         Index           =   11
         Left            =   1560
         TabIndex        =   49
         Tag             =   "T = base ** exponent       (alternate form T = base^exponent)"
         Top             =   840
         Width           =   1092
      End
      Begin ATCoCtl.ATCoGrid agdMath 
         Height          =   1215
         Left            =   0
         TabIndex        =   61
         Top             =   1800
         Width           =   4815
         _ExtentX        =   8493
         _ExtentY        =   2143
         SelectionToggle =   0   'False
         AllowBigSelection=   -1  'True
         AllowEditHeader =   0   'False
         AllowLoad       =   0   'False
         AllowSorting    =   0   'False
         Rows            =   81
         Cols            =   2
         ColWidthMinimum =   300
         gridFontBold    =   0   'False
         gridFontItalic  =   0   'False
         gridFontName    =   "MS Sans Serif"
         gridFontSize    =   8
         gridFontUnderline=   0   'False
         gridFontWeight  =   400
         gridFontWidth   =   0
         Header          =   "Type a constant or select time series for each argument"
         FixedRows       =   0
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
      Begin VB.OptionButton optFunction 
         Caption         =   "Abs"
         Height          =   252
         Index           =   15
         Left            =   720
         TabIndex        =   45
         Tag             =   "T = Abs(arg)"
         Top             =   840
         Width           =   732
      End
      Begin VB.OptionButton optFunction 
         Caption         =   "Weight"
         Height          =   252
         Index           =   8
         Left            =   720
         TabIndex        =   43
         Tag             =   "T = (arg1*weight1)+(arg2*weight2)+...      (sum of weights should be 1)"
         Top             =   360
         Width           =   852
      End
      Begin VB.OptionButton optFunction 
         Caption         =   "Mean"
         Height          =   252
         Index           =   7
         Left            =   720
         TabIndex        =   42
         Tag             =   "T = (arg1 + arg2 + ... + argN) / N"
         Top             =   120
         Width           =   852
      End
      Begin VB.OptionButton optFunction 
         Caption         =   "Div"
         Height          =   252
         Index           =   6
         Left            =   0
         TabIndex        =   41
         Tag             =   "T = numerator / denominator"
         Top             =   840
         Width           =   732
      End
      Begin VB.OptionButton optFunction 
         Caption         =   "Mult"
         Height          =   252
         Index           =   5
         Left            =   0
         TabIndex        =   40
         Tag             =   "T = arg1 * arg2 * ..."
         Top             =   600
         Width           =   732
      End
      Begin VB.OptionButton optFunction 
         Caption         =   "Sub"
         Height          =   252
         Index           =   4
         Left            =   0
         TabIndex        =   39
         Tag             =   "T = arg1 - arg2"
         Top             =   360
         Width           =   732
      End
      Begin VB.OptionButton optFunction 
         Caption         =   "Add"
         Height          =   252
         Index           =   3
         Left            =   0
         TabIndex        =   38
         Tag             =   "T = arg1 + arg2 + ..."
         Top             =   120
         Value           =   -1  'True
         Width           =   732
      End
      Begin VB.OptionButton optFunction 
         Caption         =   "Max"
         Height          =   252
         Index           =   19
         Left            =   4200
         TabIndex        =   54
         Tag             =   "T = Max(arg1, arg2, ...)"
         Top             =   360
         Width           =   732
      End
      Begin VB.OptionButton optFunction 
         Caption         =   "Line"
         Height          =   252
         Index           =   22
         Left            =   720
         TabIndex        =   44
         Tag             =   "T = (arg1 * slope) + offset"
         Top             =   600
         Width           =   732
      End
      Begin VB.OptionButton optFunction 
         Caption         =   "Interpolate"
         Height          =   252
         Index           =   0
         Left            =   2640
         TabIndex        =   51
         Tag             =   "Values from first TS, constant args = missing values, dates from other TS args will be used if more than one TS arg"
         Top             =   360
         Width           =   1212
      End
      Begin VB.OptionButton optFunction 
         Caption         =   "Convert Units"
         Height          =   252
         Index           =   1
         Left            =   2640
         TabIndex        =   52
         Tag             =   "Values from TS, converted from current units to new units"
         Top             =   600
         Width           =   1335
      End
      Begin VB.Label lblRunning 
         Caption         =   "values after each value"
         Height          =   255
         Index           =   1
         Left            =   3000
         TabIndex        =   59
         Top             =   1125
         Width           =   1815
      End
      Begin VB.Label lblRunning 
         Caption         =   "values before and"
         Height          =   255
         Index           =   0
         Left            =   1200
         TabIndex        =   57
         Top             =   1125
         Width           =   1335
      End
      Begin VB.Label lblFunction 
         BackColor       =   &H80000016&
         Height          =   255
         Left            =   0
         TabIndex        =   60
         Top             =   1440
         Width           =   4815
      End
   End
   Begin VB.Frame fraNewTimeseries 
      Caption         =   "New Properties"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   1812
      Left            =   120
      TabIndex        =   68
      Top             =   3720
      Width           =   6612
      Begin VB.ComboBox cboUnits 
         Height          =   315
         Left            =   1080
         TabIndex        =   80
         Top             =   1320
         Width           =   1095
      End
      Begin VB.ComboBox cboFile 
         Height          =   315
         Left            =   3120
         TabIndex        =   82
         Top             =   1320
         Width           =   3255
      End
      Begin VB.TextBox txtDescOut 
         Height          =   288
         Left            =   3120
         TabIndex        =   78
         Top             =   840
         Width           =   3252
      End
      Begin VB.TextBox txtIdOut 
         Height          =   288
         Left            =   1080
         TabIndex        =   76
         ToolTipText     =   "ID = DSN for WDM"
         Top             =   840
         Width           =   732
      End
      Begin VB.TextBox txtScenOut 
         Height          =   288
         Left            =   1080
         TabIndex        =   70
         Top             =   360
         Width           =   1092
      End
      Begin VB.TextBox txtLocOut 
         Height          =   288
         Left            =   3120
         TabIndex        =   72
         Top             =   360
         Width           =   1092
      End
      Begin VB.TextBox txtConsOut 
         Height          =   288
         Left            =   5280
         TabIndex        =   74
         Top             =   360
         Width           =   1092
      End
      Begin VB.Label lblUnits 
         Caption         =   "Units"
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
         Left            =   240
         TabIndex        =   79
         Top             =   1320
         Width           =   615
      End
      Begin VB.Label lblFile 
         Caption         =   "Save in"
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
         TabIndex        =   81
         Top             =   1320
         Width           =   735
      End
      Begin VB.Label Label1 
         Caption         =   "Description"
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
         Left            =   2040
         TabIndex        =   77
         Top             =   900
         Width           =   1092
      End
      Begin VB.Label lblDsn 
         Caption         =   "ID"
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
         Left            =   240
         TabIndex        =   75
         Top             =   900
         Width           =   372
      End
      Begin VB.Label lblScenario 
         Caption         =   "Scenario"
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
         Left            =   240
         TabIndex        =   69
         Top             =   420
         Width           =   972
      End
      Begin VB.Label lblLocation 
         Caption         =   "Location"
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
         Left            =   2280
         TabIndex        =   71
         Top             =   420
         Width           =   972
      End
      Begin VB.Label lblConstit 
         Caption         =   "Constituent"
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
         Left            =   4320
         TabIndex        =   73
         Top             =   420
         Width           =   1092
      End
   End
   Begin VB.Frame fraTab 
      BorderStyle     =   0  'None
      Caption         =   "Frame1"
      Height          =   2772
      Index           =   1
      Left            =   360
      TabIndex        =   96
      Top             =   480
      Width           =   6252
      Begin VB.ComboBox cboIntervalTser 
         Height          =   288
         Left            =   0
         TabIndex        =   1
         Top             =   360
         Width           =   5772
      End
      Begin VB.ComboBox cboEndMon 
         Enabled         =   0   'False
         Height          =   288
         ItemData        =   "frmTSnew.frx":047E
         Left            =   3960
         List            =   "frmTSnew.frx":04A6
         Style           =   2  'Dropdown List
         TabIndex        =   4
         Top             =   888
         Width           =   735
      End
      Begin VB.TextBox txtTstep 
         Alignment       =   1  'Right Justify
         Height          =   310
         Left            =   1200
         TabIndex        =   2
         Text            =   "1"
         Top             =   888
         Width           =   432
      End
      Begin VB.ComboBox cboTunits 
         Height          =   288
         ItemData        =   "frmTSnew.frx":04E6
         Left            =   1620
         List            =   "frmTSnew.frx":04FC
         Style           =   2  'Dropdown List
         TabIndex        =   3
         ToolTipText     =   "Time Units"
         Top             =   888
         Width           =   1152
      End
      Begin VB.ComboBox cboAggr 
         Height          =   288
         ItemData        =   "frmTSnew.frx":0522
         Left            =   1200
         List            =   "frmTSnew.frx":0532
         Style           =   2  'Dropdown List
         TabIndex        =   5
         Top             =   1380
         Width           =   1212
      End
      Begin VB.Label lblTimser 
         Caption         =   "Base on existing time series"
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
         Left            =   0
         TabIndex        =   105
         Top             =   120
         Width           =   2412
      End
      Begin VB.Label lblYearEnd 
         Caption         =   "Year End"
         Enabled         =   0   'False
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
         Left            =   3000
         TabIndex        =   100
         Top             =   960
         Width           =   1092
      End
      Begin VB.Label lblAggr 
         BackStyle       =   0  'Transparent
         Caption         =   "Aggregation"
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
         Left            =   0
         TabIndex        =   99
         Top             =   1440
         WhatsThisHelpID =   16
         Width           =   1092
         WordWrap        =   -1  'True
      End
      Begin VB.Label lblTStep 
         BackStyle       =   0  'Transparent
         Caption         =   "Time Step"
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
         Left            =   0
         TabIndex        =   98
         Top             =   960
         WhatsThisHelpID =   16
         Width           =   1092
         WordWrap        =   -1  'True
      End
   End
   Begin VB.Frame fraButtons 
      BorderStyle     =   0  'None
      Caption         =   "Frame1"
      Height          =   372
      Left            =   2280
      TabIndex        =   104
      Top             =   5640
      Width           =   2292
      Begin VB.CommandButton cmdCancel 
         Cancel          =   -1  'True
         Caption         =   "&Cancel"
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
         Left            =   1200
         TabIndex        =   84
         Top             =   0
         Width           =   1092
      End
      Begin VB.CommandButton cmdOk 
         Caption         =   "&Ok"
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
         Left            =   0
         TabIndex        =   83
         Top             =   0
         Width           =   1092
      End
   End
   Begin MSComctlLib.TabStrip Tabs 
      Height          =   3492
      Left            =   120
      TabIndex        =   0
      Top             =   120
      Width           =   6852
      _ExtentX        =   12091
      _ExtentY        =   6165
      _Version        =   393216
      BeginProperty Tabs {1EFB6598-857C-11D1-B16A-00C0F0283628} 
         NumTabs         =   5
         BeginProperty Tab1 {1EFB659A-857C-11D1-B16A-00C0F0283628} 
            Caption         =   "Change Interval"
            ImageVarType    =   2
         EndProperty
         BeginProperty Tab2 {1EFB659A-857C-11D1-B16A-00C0F0283628} 
            Caption         =   "Add/Remove Dates"
            ImageVarType    =   2
         EndProperty
         BeginProperty Tab3 {1EFB659A-857C-11D1-B16A-00C0F0283628} 
            Caption         =   "Shift Dates"
            ImageVarType    =   2
         EndProperty
         BeginProperty Tab4 {1EFB659A-857C-11D1-B16A-00C0F0283628} 
            Caption         =   "Math"
            ImageVarType    =   2
         EndProperty
         BeginProperty Tab5 {1EFB659A-857C-11D1-B16A-00C0F0283628} 
            Caption         =   "Table Filter Values"
            ImageVarType    =   2
         EndProperty
      EndProperty
   End
End
Attribute VB_Name = "frmTSnew"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
'Copyright 2000 by AQUA TERRA Consultants

Private pTser As Collection
Private nts As Long
Private pTserLabel As Collection
Private pMathFunction As String
Private pMathExpandable As Boolean
Private pCreationType As String
Private pOpenFiles As Collection
Private pAddedValueOption As Long
Private pcboEndMonListIndex As Long

Public Event CreatedTser(newTS As ATCclsTserData)

Public Property Get OpenFiles() As Collection
  Set OpenFiles = pOpenFiles
End Property
Public Property Set OpenFiles(newvalue As Collection)
  Dim vTSF As Variant, tsf As ATCclsTserFile
  Set pOpenFiles = newvalue
  If Not pOpenFiles Is Nothing Then
    cboFile.clear
    For Each vTSF In pOpenFiles
      Set tsf = vTSF
      'If tsf.Label = "In-Memory" Then
      '  cboFile.AddItem "<in memory>"
      'Else
        cboFile.AddItem tsf.Filename
      'End If
    Next
    If cboFile.ListCount > 0 Then cboFile.ListIndex = 0
    'cboFile.AddItem "Other file..."
'    If Not pTser Is Nothing Then
'      If pTser.Count = 1 Then
'        If Not pTser(1).File Is Nothing Then
'          cboFile.text = pTser(1).File.Filename
'        End If
'      End If
'    End If
  End If
End Property

Public Property Get AllTSer() As Collection
  Set AllTSer = pTser
End Property
Public Property Set AllTSer(newvalue As Collection)
  Dim tsIndex As Long, tsLabel As String
  
  Set pTser = Nothing
  Set pTser = newvalue
  If pTser Is Nothing Then Set pTser = New Collection
  nts = pTser.Count

  Set pTserLabel = Nothing
  Set pTserLabel = New Collection
  
  cboIntervalTser.clear
  cboAddRemoveTser.clear
  cboShiftTser.clear
  cboTableTser.clear
  'cboInterpolateTser.clear
  'cboInterpolateDates.clear
  If nts > 0 Then
    For tsIndex = 1 To nts
      With pTser(tsIndex).Header
        tsLabel = .sen & " " & .Loc & " " & .con & "  #" & .ID & " " & pTser(tsIndex).File.Filename
      End With
      pTserLabel.Add tsLabel
      agdMath.addValue tsLabel
      cboIntervalTser.AddItem tsLabel
      cboAddRemoveTser.AddItem tsLabel
      cboShiftTser.AddItem tsLabel
      cboTableTser.AddItem tsLabel
      'cboInterpolateTser.AddItem tsLabel
      'cboInterpolateDates.AddItem tsLabel
    Next
    SetDefaultTS 1
    EnablePartsRequiringTser True
  Else
    EnablePartsRequiringTser False
  End If
End Property

Private Sub agdMath_CommitChange(ChangeFromRow As Long, ChangeToRow As Long, ChangeFromCol As Long, ChangeToCol As Long)
  Dim tsIndex As Long
  Dim tsLabel As String
  
  With agdMath
    If pMathExpandable And .MaxOccupiedRow >= .Rows Then
      If .TextMatrix(1, 0) = "arg1" Then
        If Len(Trim(.TextMatrix(.Rows, 1))) > 0 Then
          .Rows = .Rows + 1
          .TextMatrix(.Rows, 0) = "arg" & .Rows
        End If
      Else
        .Rows = .Rows + 1
      End If
    End If
    tsLabel = .TextMatrix(ChangeFromRow, ChangeFromCol)
    If Not IsNumeric(tsLabel) Then
      If .TextMatrix(ChangeFromRow, 0) = "to units" Then
        If cboUnits.Text <> tsLabel Then cboUnits.Text = tsLabel
      Else
        For tsIndex = 1 To nts
          If pTserLabel(tsIndex) = tsLabel Then
            SetDefaultTS tsIndex
            Exit For
          End If
        Next
      End If
    End If
  End With
End Sub

Private Sub agdMath_RowColChange()
  On Error GoTo errHand
  Select Case LCase(pMathFunction)
    Case "convert units"
      Dim fromUnits As String
      Dim vUnitCategory As Variant 'String
      Dim vUnits As Variant
      Dim CategoryColl As FastCollection
      Dim UnitsColl As FastCollection
      agdMath.ClearValues
      Select Case agdMath.row
        Case 1: AddTsersToGridValues agdMath
        Case 2, 3
          With agdMath
            If Len(.TextMatrix(1, 1)) > 0 Then 'If a timeseries has been specified
              If Len(.TextMatrix(2, 1)) = 0 Then 'and from units have not
                On Error Resume Next             'try to fill in default from units
                .TextMatrix(2, 1) = TserMatching(.TextMatrix(1, 1)).Attrib("Units")
                On Error GoTo errHand
              End If
            End If
            .ClearValues
            fromUnits = .TextMatrix(2, 1)
            If Len(fromUnits) > 0 Then
              .addValue fromUnits
              vUnitCategory = GetUnitCategory(fromUnits)
            Else
              vUnitCategory = ""
            End If
            If Len(vUnitCategory) > 0 Then
              Set UnitsColl = GetAllUnitsInCategory(vUnitCategory)
              For Each vUnits In UnitsColl
                If vUnits <> fromUnits Then .addValue CStr(vUnits)
              Next
            Else
              Set CategoryColl = GetAllUnitCategories
              For Each vUnitCategory In CategoryColl
                Set UnitsColl = GetAllUnitsInCategory(vUnitCategory)
                For Each vUnits In UnitsColl
                  If vUnits <> fromUnits Then .addValue CStr(vUnits)
                Next
              Next
            End If
          End With
      End Select
    Case ""
  End Select
  Exit Sub
errHand:
  MsgBox Err.Description, vbCritical, "Math Row/Col Change"
End Sub

Private Sub agdTable_CommitChange(ChangeFromRow As Long, ChangeToRow As Long, ChangeFromCol As Long, ChangeToCol As Long)
  With agdTable
    If .MaxOccupiedRow = .Rows Then .Rows = .Rows + 1
  End With
End Sub

Private Sub agdTable_RowColChange()
  With agdTable
    .ClearValues
    If .col = 1 Then
      .addValue "Leave Unchanged"
      .addValue "Delete"
    End If
  End With
End Sub

Private Sub cboAddRemoveTser_Click()
  SetDefaultTS cboAddRemoveTser.ListIndex + 1
  optBaseTser(0).Value = True
End Sub

'Private Sub cboInterpolateTser_Change()
'  SetDefaultTS cboInterpolateTser.ListIndex + 1
'  cboInterpolateDates.ListIndex = cboInterpolateTser.ListIndex
'End Sub

Private Sub cboIntervalTser_Click()
  SetDefaultTS cboIntervalTser.ListIndex + 1
End Sub

Private Sub cboShiftTser_Click()
  SetDefaultTS cboShiftTser.ListIndex + 1
End Sub

Private Sub cboTableTser_Click()
  SetDefaultTS cboTableTser.ListIndex + 1
End Sub

Private Sub cboUnits_Change()
  If agdMath.TextMatrix(3, 0) = "to units" Then
    If agdMath.TextMatrix(3, 1) <> cboUnits.Text Then
      agdMath.TextMatrix(3, 1) = cboUnits.Text
    End If
  End If
End Sub

Private Sub cboUnits_Click()
  cboUnits_Change
End Sub

Private Sub chkRunning_Click()
  Dim SelectedOpt As Integer
  Dim opt As Integer
  On Error GoTo errHandLoop
  SelectedOpt = -1
  For opt = optFunction.LBound To optFunction.UBound
    With optFunction(opt)
      If chkRunning.Value = vbChecked Then
        Select Case .Caption
          Case "Mean", "Geometric Mean", "Add", "Min", "Max"
            .Enabled = True
            If .Value = True Then SelectedOpt = opt
          Case Else
            .Value = False
            .Enabled = False
        End Select
      Else
        .Enabled = True
      End If
    End With
NextOpt:
  Next
On Error GoTo errHand
  If SelectedOpt < 0 Then
    optFunction(3).Value = True
  Else
    optFunction_Click SelectedOpt
  End If
errHand:
  Exit Sub

errHandLoop:
  Resume NextOpt
End Sub

Private Sub cmdCancel_Click()
  Unload Me
End Sub

Private Sub cmdOk_Click()
  Dim tsf As ATCclsTserFile, tsv As Variant
  Dim Filename As String
  Dim ts As ATCclsTserData
  Dim Msg As String
  Msg = ""
  If Len(Trim(txtScenOut.Text)) = 0 Then
    Msg = Msg & "Scenario must be specified for new time series" & vbCr
  End If
  
  If Len(Trim(txtLocOut.Text)) = 0 Then
    Msg = Msg & "Location must be specified for new time series" & vbCr
  End If
  
  If Len(Trim(txtConsOut.Text)) = 0 Then
    Msg = Msg & "Constituent must be specified for new time series" & vbCr
  End If
  
  If Len(Trim(txtIdOut.Text)) = 0 Then
    Msg = Msg & "ID number must be specified for new time series" & vbCr & "For WDM, ID = data set number (DSN)" & vbCr
  End If
    
  If Len(Msg) > 0 Then
    MsgBox Msg, vbExclamation, "Cannot create new time series"
  Else
    Me.MousePointer = vbHourglass
    Select Case pCreationType
  
      Case "Change Interval":     Set ts = ChangeInterval
      Case "Add/Remove Dates":    Set ts = AddRemoveDates
      Case "Shift Dates":         Set ts = ShiftDates
      Case "Math":                Set ts = Math
      Case "Table Filter Values": Set ts = TableFilter
      'Case "Interpolate":         Set ts = Interpolate
      
    End Select
    Me.MousePointer = vbDefault
    
    If Not ts Is Nothing Then
      With ts.Header
        .sen = txtScenOut
        .Loc = txtLocOut
        .con = txtConsOut
        .Desc = txtDescOut
        .ID = txtIdOut
      End With
      ts.AttribSet "Units", cboUnits.Text
      Filename = cboFile.Text
      If Len(Filename) > 0 And Not pOpenFiles Is Nothing Then
        For Each tsv In pOpenFiles
          Set tsf = tsv
          If tsf.Filename = Filename Then GoTo FoundFile
        Next
        
        MsgBox "Could not find file '" & Filename & "'", vbExclamation, "New Timeseries"
        Exit Sub
        
FoundFile:
        If tsf.addtimser(ts, TsIdReplAsk + TsIdAppendAsk + TsIdRenumAsk) Then 'Add successful
          'confimation messagebox is opened in addtimser, at least in WDM it is.
        Else
          MsgBox "Failed to save timeseries." & vbCr & tsf.ErrorDescription, vbExclamation, "Save Timeseries"
          Exit Sub
        End If
      End If
      RaiseEvent CreatedTser(ts)
      'If MsgBox("Created a time series using " & pCreationType & vbCr & "Exit this window?", vbYesNo, "TSNew") = vbYes Then
        Unload Me
      'End If
    End If
  End If
End Sub

'Private Sub SetTsSLCD(ts As ATCclsTserData)
'End Sub

Private Function ChangeInterval() As ATCclsTserData
  Dim tsIn As ATCclsTserData
  Dim tsOut As ATCclsTserData
  Dim dsOut As ATCclsTserDate, dsS As ATTimSerDateSummary
  Dim Sdt&(6), Edt&(6), lMo&
    
  If cboTunits.ListIndex < 0 Then
    MsgBox pCreationType & " needs Time Units to be specified"
  ElseIf cboAggr.ListIndex < 0 Then
    MsgBox pCreationType & " needs Aggregation to be specified"
  Else
    Set tsIn = TserMatching(cboIntervalTser.Text)
    If tsIn Is Nothing Then
      MsgBox pCreationType & " needs to be based on an existing time series"
    Else
      Set dsOut = New ATCclsTserDate
      With dsS
        If cboTunits.Text = "Whole Span" Then
          .CIntvl = True
          .SJDay = tsIn.dates.Summary.SJDay
          .EJDay = tsIn.dates.Summary.EJDay
          .Intvl = .EJDay - .SJDay
          .NVALS = 1
          .ts = 1
          .Tu = TUCentury
        Else
          If Not IsNumeric(txtTstep.Text) Then
            MsgBox "Time step must be a number"
            Exit Function
          End If
          .CIntvl = True
          Call J2Date(tsIn.dates.Summary.SJDay, Sdt)
          '.SJDay = tsIn.dates.Summary.SJDay
          .EJDay = tsIn.dates.Summary.EJDay
          .ts = CLng(txtTstep.Text)
          .Tu = cboTunits.ListIndex + 1
          If .Tu > TUSecond Then Sdt(5) = 0
          If .Tu > TUMinute Then Sdt(4) = 0
          If .Tu > TUHour Then Sdt(3) = 0
          If .Tu > TUDay Then Sdt(2) = 1
          If .Tu > TUMonth Then
            If .Tu = TUYear Then
              lMo = pcboEndMonListIndex + 2
              If lMo = 13 Then lMo = 1
              If lMo > Sdt(1) Then Sdt(0) = Sdt(0) - 1
              Sdt(1) = lMo
            End If
          End If
          .SJDay = Date2J(Sdt)
          Select Case .Tu
            Case TUSecond:  .Intvl = .ts * JulianSecond
            Case TUMinute:  .Intvl = .ts * JulianMinute
            Case TUHour:    .Intvl = .ts * JulianHour
            Case TUDay:     .Intvl = .ts
            Case TUMonth:   .Intvl = .ts * JulianMonth
            Case TUYear:    .Intvl = .ts * JulianYear
            Case TUCentury: .Intvl = .ts * JulianYear * 100
            Case Else: .Tu = TUCentury: .Intvl = JulianYear * 100 'should not happen
          End Select
          If .Intvl <= 1 Then
            .NVALS = (.EJDay - .SJDay) / .Intvl '+ 1
            .EJDay = .SJDay + .NVALS * .Intvl
          Else 'special case for long intervals
            Call J2Date(.SJDay, Sdt)
            Call J2Date(.EJDay, Edt)
            Call timdif(Sdt, Edt, .Tu, .ts, .NVALS)
            .EJDay = TimAddJ(.SJDay, .Tu, .ts, .NVALS)
          End If
        End If
      End With
      dsOut.Summary = dsS
      Set ChangeInterval = tsIn.Aggregate(dsOut, cboAggr.ItemData(cboAggr.ListIndex))
    End If
  End If
End Function

Private Function AddRemoveDates() As ATCclsTserData
  Dim index As Long
  Dim tsIn As ATCclsTserData
  Dim Sdt&(6), Edt&(6), Clonedt&(6), Testdt&(6)
  Dim sjdate As Double, ejdate As Double
  Dim newvalue As Single
  Dim ClonedtInvalid As Boolean
  
  For index = 0 To 4
    
    If IsNumeric(txtStartDat(index)) Then
      Sdt(index) = txtStartDat(index)
    ElseIf Len(Trim(txtStartDat(index))) = 0 Then
      Sdt(index) = 0
    Else
      MsgBox "Start date fields must be numeric", vbExclamation, "Cannot create time series"
      txtStartDat(index).SetFocus
      Exit Function
    End If
    
    If IsNumeric(txtEndDat(index)) Then
      Edt(index) = txtEndDat(index)
    ElseIf Len(Trim(txtEndDat(index))) = 0 Then
      Edt(index) = 0
    Else
      MsgBox "End date fields must be numeric", vbExclamation, "Cannot create time series"
      txtEndDat(index).SetFocus
      Exit Function
    End If
  
    If IsNumeric(txtCloneDat(index)) Then
      Clonedt(index) = txtCloneDat(index)
    ElseIf Len(Trim(txtCloneDat(index))) = 0 Then
      Clonedt(index) = 0
    Else
      ClonedtInvalid = True
    End If
  
  Next
  sjdate = Date2J(Sdt)
  ejdate = Date2J(Edt)
  
  If optBaseTser(0).Value Then 'Base on existing time series
    Set tsIn = TserMatching(cboAddRemoveTser.Text)
    If tsIn Is Nothing Then
      MsgBox "Base on an existing times series was specified, " & vbCr _
           & "but no existing time series was selected.", vbExclamation, "Cannot create time series"
      Exit Function
    End If
    
    Select Case pAddedValueOption
      Case 0:
        If Not IsNumeric(txtNewVal.Text) Then
          MsgBox "New value must be numeric", vbExclamation, "Cannot create time series"
          Exit Function
        End If
        newvalue = txtNewVal.Text
      'Case 1 Use nearest existing value
      Case 2 'Interpolate from nearest existing value to specified value
        If Not IsNumeric(txtInterp.Text) Then
          MsgBox "Interpolate value must be numeric", vbExclamation, "Cannot create time series"
          Exit Function
        End If
        newvalue = CSng(txtInterp.Text)
      Case 3:
        If ClonedtInvalid Then
          MsgBox "Clone date fields must be numeric", vbExclamation, "Cannot create time series"
          Exit Function
        End If
        newvalue = tsIn.dates.IndexAtOrAfter(Date2J(Clonedt))
    End Select
    
    Set AddRemoveDates = tsIn.AddRemoveDates(sjdate, ejdate, pAddedValueOption, newvalue)
  
  Else 'not based on existing time series, must be pAddedValueOption=0: Set added values to txtNewVal
    If Not IsNumeric(txtAddRemoveTstep) Then
      MsgBox "Time steps must be numeric", vbExclamation, "Cannot create time series"
    ElseIf Not IsNumeric(txtNewVal.Text) Then
      MsgBox "New value must be numeric", vbExclamation, "Cannot create time series"
    Else
      Set AddRemoveDates = NewTser(sjdate, ejdate, (txtAddRemoveTstep), (cboAddRemoveTunits.ListIndex + 1), (txtNewVal.Text))
    End If
  End If
  
End Function

Private Function ShiftDates() As ATCclsTserData
  Dim index As Long
  Dim tsIn As ATCclsTserData
  Dim tsOut As ATCclsTserData
  Dim dsOut As ATCclsTserDate, dsS As ATTimSerDateSummary
  Dim Sdt(6) As Long, tmpdt(6) As Long
  Dim sjdate As Double, ejdate As Double
  Dim NewDateVal() As Double, DateShiftDist As Double
  
  Set tsIn = TserMatching(cboShiftTser.Text)
  If tsIn Is Nothing Then
    MsgBox pCreationType & " needs to be based on an existing time series"
  Else
    For index = 0 To 4
      Sdt(index) = txtShiftDat(index)
    Next
    Sdt(5) = 0
    Set tsOut = tsIn.Copy
    dsS = tsIn.dates.Summary
    If dsS.CIntvl Then
      'TIMADD Sdt, dsS.Tu, 1, 1, tmpdt
      'sjdate = Date2J(tmpdt)
      sjdate = Date2J(Sdt)
      
      DateShiftDist = sjdate - dsS.SJDay
      dsS.SJDay = sjdate
      dsS.EJDay = dsS.EJDay + DateShiftDist
      
      Set tsOut.dates = tsIn.dates.Copy
      tsOut.dates.Summary = dsS
    Else
      sjdate = Date2J(Sdt)
      
      DateShiftDist = sjdate - dsS.SJDay
      dsS.SJDay = sjdate
      dsS.EJDay = dsS.EJDay + DateShiftDist
      
      ReDim NewDateVal(dsS.NVALS)
      For index = 1 To dsS.NVALS
        NewDateVal(index) = tsOut.Value(index) + DateShiftDist
      Next
      Set dsOut = New ATCclsTserDate
      dsOut.File = tsIn.dates.File
      'dsOut.flags = tsIn.flags fixme
      dsOut.Values = NewDateVal
      dsOut.Summary = dsS
      Set tsOut.dates = dsOut
    End If
    Set ShiftDates = tsOut
  End If
End Function

Private Function Math() As ATCclsTserData
  Dim row As Long, col As Long, arg As String
  Dim constant As Single
  'Dim nConst As Long
  Dim nts As Long
  Dim tsIn As ATCclsTserData
  Dim args As Collection
  Dim tsOut As ATCclsTserData
  Dim PrependFunction As String
  Dim fromUnits As String
  Dim toUnits As String

  On Error GoTo errHand

  If Len(pMathFunction) < 1 Then
    MsgBox pCreationType & " needs a function to be selected"
    Exit Function
  End If
  
  Set args = New Collection
  
  If chkRunning.Value = vbChecked Then
    PrependFunction = "Running "
    arg = agdMath.TextMatrix(1, 1): GoSub AddArg
    args.Add txtRunningBefore.Text
    args.Add txtRunningAfter.Text
  Else
    Select Case pMathFunction
      Case "Convert Units"
        arg = agdMath.TextMatrix(1, 1):       GoSub AddArg
        If tsIn Is Nothing Then
          MsgBox "First argument to " & pMathFunction & " must be a time series"
        Else
          fromUnits = Trim(agdMath.TextMatrix(2, 1))
          toUnits = Trim(agdMath.TextMatrix(3, 1))
          If Len(fromUnits) = 0 Or Len(toUnits) = 0 Then
            MsgBox "From units and to units must both be specified"
          Else
            If fromUnits = "degF" And toUnits = "degC" Then
              pMathFunction = "F2C"
            ElseIf fromUnits = "degC" And toUnits = "degF" Then
              pMathFunction = "C2F"
            Else
              pMathFunction = "Mult"
              args.Add GetConversionFactor(fromUnits, toUnits)
            End If
          End If
        End If
      Case "Add", "Mult", "Mean", "Geometric Mean", "Min", "Max", "Interpolate" 'One constant, N time series
        For row = 1 To agdMath.Rows
          arg = agdMath.TextMatrix(row, 1)
          If Len(Trim(arg)) > 0 Then GoSub AddArg
        Next
  
      Case "Sub", "Div", "Exponent" '2 constant or time series"
        
        arg = agdMath.TextMatrix(1, 1):       GoSub AddArg
        arg = agdMath.TextMatrix(2, 1):       GoSub AddArg
  
      Case "Line"                              '3 constant or time series
        
        arg = agdMath.TextMatrix(1, 1):       GoSub AddArg
        arg = agdMath.TextMatrix(2, 1):       GoSub AddArg
        arg = agdMath.TextMatrix(3, 1):       GoSub AddArg
  
      Case "Weight"                            'N constants, N time series
        
        For row = 1 To agdMath.Rows
          arg = agdMath.TextMatrix(row, 0)
          If Len(Trim(arg)) > 0 Then
                                              GoSub AddArg
            arg = agdMath.TextMatrix(row, 1): GoSub AddArg
          End If
        Next
      
      Case "Abs", "Log 10", "Log e", "e**", "Running Sum" 'One time series
        
        arg = agdMath.TextMatrix(1, 1):       GoSub AddArg
    
    End Select
  End If
  If nts = 0 Then
    MsgBox pCreationType & " requires at least one time series."
  Else
    Set Math = MathColl(PrependFunction & pMathFunction, args)
  End If
  
  Exit Function
  
AddArg:
  If IsNumeric(arg) Then
    constant = arg
    args.Add constant
    'nConst = nConst + 1
  Else
    Set tsIn = Nothing
    Set tsIn = TserMatching(arg)
    If tsIn Is Nothing Then
      MsgBox pCreationType & " does not recognize '" & arg & "' as numeric or as a time series."
      Exit Function
    End If
    args.Add tsIn
    nts = nts + 1
  End If
  Return
  
errHand:
  MsgBox Err.Description, vbCritical, "New Time Series Math"
End Function

'Private Function Interpolate() As ATCclsTserData
'  Dim tsIn As ATCclsTserData
'  Dim tsDates As ATCclsTserData
'  Set tsIn = TserMatching(cboInterpolateTser.Text)
'  If tsIn Is Nothing Then
'    MsgBox pCreationType & " needs to be based on an existing time series"
'  Else
'    Set tsDates = TserMatching(cboInterpolateDates.Text)
'    If tsDates Is Nothing Then
'      MsgBox pCreationType & " needs to be based on existing dates"
'    Else
'      If chkInterpReplace.Value = vbChecked Then
'        If IsNumeric(txtInterpReplace.Text) Then
'          Set Interpolate = InterpolateMissing(tsIn, tsDates.Dates, CSng(txtInterpReplace.Text))
'        Else
'          MsgBox "Data value to replace must be numeric"
'        End If
'      Else
'        If tsDates.Dates.Serial = tsIn.Dates.Serial Then
'          MsgBox "Interpolating to the same dates does not change anything"
'        Else
'          Set Interpolate = tsIn.Interpolate(tsDates.Dates)
'        End If
'      End If
'    End If
'  End If
'End Function

Private Function TableFilter() As ATCclsTserData
  Dim row As Long
  Dim rangeTop() As Single, newvalue() As Single
  Dim nRange As Long
  Dim Interpolate As Boolean
  Dim tsIn As ATCclsTserData
  Dim rangeStr As String, valueStr As String

  Set tsIn = TserMatching(cboTableTser.Text)
  If tsIn Is Nothing Then
    MsgBox pCreationType & " needs to be based on an existing time series"
  Else
    If optTable(1).Value Then Interpolate = True Else Interpolate = False
    
    With agdTable
      nRange = .Rows
      ReDim rangeTop(nRange)
      ReDim newvalue(nRange)
      nRange = 0
      For row = 1 To agdTable.Rows
        rangeStr = .TextMatrix(row, 0)
        valueStr = .TextMatrix(row, 1)
        If Len(rangeStr) > 0 And Len(valueStr) > 0 Then
          If IsNumeric(rangeStr) Then
            If IsNumeric(valueStr) Then
              nRange = nRange + 1
              rangeTop(nRange) = CSng(rangeStr)
              newvalue(nRange) = CSng(valueStr)
            ElseIf valueStr = "Delete" Then
              nRange = nRange + 1
              rangeTop(nRange) = CSng(rangeStr)
              newvalue(nRange) = -911
            ElseIf valueStr = "Leave Unchanged" Then
              nRange = nRange + 1
              rangeTop(nRange) = CSng(rangeStr)
              newvalue(nRange) = -912
            End If
          End If
        End If
      Next
      ReDim Preserve rangeTop(nRange)
      ReDim Preserve newvalue(nRange)
    End With
  End If
  If nRange > 0 Then
    Set TableFilter = tsIn.doTable(rangeTop, newvalue, Interpolate)
  End If
End Function

Private Sub cmdTableLoad_Click()
  agdTable.LoadGridInteractive
End Sub

Private Sub cmdTableSave_Click()
  agdTable.SaveGridInteractive
End Sub

Private Sub Form_Load()
  Dim vUnits As Variant
  Width = 7000
  agdMath.Rows = 2
  txtRunningBefore = GetSetting("Aqua Terra Units", "New Time Series", "Running Before", "all")
  txtRunningAfter = GetSetting("Aqua Terra Units", "New Time Series", "Running After", "0")
  
  With cboUnits
    .clear
    For Each vUnits In GetAllUnitsInCategory("all")
      .AddItem vUnits
    Next
  End With
  With cboTunits
    .clear
    .AddItem "Second"
    .AddItem "Minute"
    .AddItem "Hour"
    .AddItem "Day"
    .AddItem "Month"
    .AddItem "Year"
    .AddItem "Whole Span"
  End With
  pcboEndMonListIndex = 11
  
  With cboAddRemoveTunits
    .clear
    .AddItem "Second"
    .AddItem "Minute"
    .AddItem "Hour"
    .AddItem "Day"
    .AddItem "Month"
    .AddItem "Year"
  End With
  With cboAggr
    .clear
'  Date control does this differently??
'  lstAggr.AddItem "Sum/Div":   lstAggr.ItemData(0) = 0
'  lstAggr.AddItem "Aver/Same": lstAggr.ItemData(1) = 1
'  lstAggr.AddItem "Max":       lstAggr.ItemData(2) = 2
'  lstAggr.AddItem "Min":       lstAggr.ItemData(3) = 3
'  lstAggr.AddItem "Native":    lstAggr.ItemData(4) = 4
    .AddItem "Aver/Same":   .ItemData(0) = 0
    .AddItem "Sum/Div":     .ItemData(1) = 1
    .AddItem "Max":         .ItemData(2) = 2
    .AddItem "Min":         .ItemData(3) = 3
'    .AddItem "Last":        .ItemData(4) = 4 'would like this!
    .ListIndex = 0
  End With
  With agdTable
    .ColTitle(0) = "Values <="
    .ColTitle(1) = "New value"
    .ColEditable(0) = True
    .ColEditable(1) = True
  End With
  
  optFunction_Click 3

  If pTser Is Nothing Then
    EnablePartsRequiringTser False
  ElseIf pTser.Count = 0 Then
    EnablePartsRequiringTser False
  Else
    EnablePartsRequiringTser True
  End If
End Sub
  
Private Sub EnablePartsRequiringTser(Ena As Boolean)
  Tabs.Enabled = True
  If Ena Then
    optBaseTser(0).Value = True
    Tabs.SelectedItem = Tabs.Tabs(1)
  Else
    Tabs.SelectedItem = Tabs.Tabs(2)
    optBaseTser(1).Value = True
  End If
  Tabs_Click
  Tabs.Enabled = Ena
  cboAddRemoveTser.Enabled = Ena
  optBaseTser(0).Enabled = Ena
  SelectedExistingOrInterval
End Sub

Private Sub SelectedExistingOrInterval()
  Dim Ena As Boolean
  Ena = optBaseTser(0).Value
  
  cboAddRemoveTser.Enabled = Ena
  txtAddRemoveTstep.Enabled = Not Ena
  cboAddRemoveTunits.Enabled = Not Ena
  
  If Not Ena Then optNewValues(0).Value = True
  optNewValues(1).Enabled = Ena
  optNewValues(2).Enabled = Ena
  optNewValues(3).Enabled = Ena
  txtInterp.Enabled = Ena
  txtCloneDat(0).Enabled = Ena
  txtCloneDat(1).Enabled = Ena
  txtCloneDat(2).Enabled = Ena
  txtCloneDat(3).Enabled = Ena
  txtCloneDat(4).Enabled = Ena
End Sub

Private Sub Form_Resize()
  Dim t As Long, fraWidth As Long, fraHeight As Long
  If Width > 800 And Height > 3000 Then
    fraButtons.Top = Height - 816
    fraButtons.Left = (Width - fraButtons.Width) / 2
    fraNewTimeseries.Top = fraButtons.Top - fraNewTimeseries.Height - 144
    fraNewTimeseries.Width = Width - 360
    fraWidth = fraNewTimeseries.Width
    If fraWidth > 3500 Then
      cboFile.Width = fraWidth - cboFile.Left - 240
      txtDescOut.Width = fraWidth - txtDescOut.Left - 240
      txtScenOut.Width = (fraWidth - 3336) / 3
      txtLocOut.Width = txtScenOut.Width
      txtConsOut.Width = txtScenOut.Width
      lblLocation.Left = txtScenOut.Left + txtScenOut.Width + 108
      txtLocOut.Left = lblLocation.Left + 840
      lblConstit.Left = txtLocOut.Left + txtLocOut.Width + 108
      txtConsOut.Left = lblConstit.Left + 1050
    End If
    Tabs.Width = fraWidth
    Tabs.Height = fraNewTimeseries.Top - 228
    fraWidth = Tabs.Width - 360
    fraHeight = Tabs.Height - 480
    If fraHeight < 100 Then fraHeight = 100
    For t = 1 To Tabs.Tabs.Count
      fraTab(t).Width = fraWidth
      fraTab(t).Height = fraHeight
    Next
    lblFunction.Width = fraWidth
    agdMath.Width = fraWidth
    agdTable.Width = fraWidth
    
    cboIntervalTser.Width = fraWidth
    cboAddRemoveTser.Width = fraWidth
    cboShiftTser.Width = fraWidth
    cboTableTser.Width = fraWidth
    'cboInterpolateTser.Width = fraWidth
    'cboInterpolateDates.Width = fraWidth
    fraTableButtons.Left = fraWidth - fraTableButtons.Width
    
    If fraHeight > 1600 Then
      agdMath.Height = fraHeight - 1560
      agdTable.Height = fraHeight - 600
    End If
  End If
End Sub

Private Sub cboAggr_Change()
  If cboAggr.Text = "Native" Then
    txtTstep.Enabled = False
    cboTunits.Enabled = False
    'lblDate(4).Enabled = False
  Else
    txtTstep.Visible = cboAggr.Visible
    cboTunits.Visible = cboAggr.Visible
    'lblDate(4).Enabled = True
  End If
End Sub

Private Sub cboTunits_Click()
  If cboTunits.Text = "Year" Then
    lblYearEnd.Enabled = True
    cboEndMon.Enabled = True
    cboEndMon.ListIndex = pcboEndMonListIndex
  Else
    lblYearEnd.Enabled = False
    cboEndMon.Enabled = False
    If cboEndMon.ListIndex <> -1 Then
      pcboEndMonListIndex = cboEndMon.ListIndex
    End If
    cboEndMon.ListIndex = -1
  End If
End Sub

Private Sub optBaseTser_Click(index As Integer)
  SelectedExistingOrInterval
End Sub

Private Sub optFunction_Click(index As Integer)
  Dim r As Long, c As Long
  pMathFunction = optFunction(index).Caption
  lblFunction.Caption = " " & optFunction(index).Tag
  With agdMath
    .clear
    .cols = 2
    If chkRunning.Value = vbChecked Then
      .FixedRows = 0
      .Rows = 1
      .FixedCols = 1
      pMathExpandable = False
      .TextMatrix(1, 0) = "timeseries"
    Else
      Select Case pMathFunction
        Case "Convert Units"
          .FixedRows = 0
          .Rows = 3
          .cols = 2
          .FixedCols = 1
          .TextMatrix(1, 0) = "timeseries"
          .TextMatrix(2, 0) = "from units"
          .TextMatrix(3, 0) = "to units"
          pMathExpandable = False
        
        'One constant, N time series
        Case "Add", "Mult", "Mean", "Geometric Mean", "Min", "Max", "Interpolate"
          .FixedRows = 0
          If .Rows < 2 Then .Rows = 2
          If .MaxOccupiedRow = .Rows Then .Rows = .Rows + 1
          .FixedCols = 1
          pMathExpandable = True
          For r = 1 To .Rows
            .TextMatrix(r, 0) = "arg" & r
          Next
        
        Case "Sub", "Div", "Exponent" '2 constant or time series
          .FixedRows = 0
          .Rows = 2
          .cols = 2
          .FixedCols = 1
          pMathExpandable = False
          Select Case pMathFunction
            Case "Div"
              .TextMatrix(1, 0) = "numerator"
              .TextMatrix(2, 0) = "denominator"
            Case "Sub"
              .TextMatrix(1, 0) = "arg1"
              .TextMatrix(2, 0) = "arg2"
            Case "Exponent"
              .TextMatrix(1, 0) = "base"
              .TextMatrix(2, 0) = "exponent"
          End Select
        
        Case "Line", "Z+", "Z-"                  '3 constant or time series
          .FixedRows = 0
          .Rows = 3
          .FixedCols = 1
          pMathExpandable = False
          Select Case pMathFunction
            Case "Line"
              .TextMatrix(1, 0) = "arg1"
              .TextMatrix(2, 0) = "slope"
              .TextMatrix(3, 0) = "offset"
            Case "Z+", "Z-"
              .TextMatrix(1, 0) = "lowval"
              .TextMatrix(2, 0) = "threshold"
              .TextMatrix(3, 0) = "hival"
          End Select
        
        Case "Weight"                            'N constants, N time series
          If .FixedCols > 0 Then
            .clear
            .FixedCols = 0
          End If
          If .Rows < 3 Then .Rows = 3
          If .MaxOccupiedRow = .Rows Then .Rows = .Rows + 1
          .FixedRows = 1
          pMathExpandable = True
          .ColTitle(0) = "arg"
          .ColTitle(1) = "weight"
        
        Case "Abs", "Log 10", "Log e", "e**", "Sum" 'One time series
          .FixedRows = 0
          .Rows = 1
          .FixedCols = 1
          pMathExpandable = False
          .TextMatrix(1, 0) = "arg"
      End Select
    End If
    For c = 0 To .cols - 1
      .colWidth(c) = .Width / .cols
      .ColEditable(c) = True
    Next c
    AddTsersToGridValues agdMath
  End With
End Sub

Private Sub AddTsersToGridValues(agd As ATCoGrid, Optional ClearGrid As Boolean = True)
  Dim c As Integer
  
  If ClearGrid Then agd.ClearValues
  
  For c = 1 To nts
    With pTser(c).Header
      agd.addValue .sen & " " & .Loc & " " & .con & "  #" & .ID & " " & pTser(c).File.Filename
    End With
  Next
End Sub

Private Sub optNewValues_Click(index As Integer)
  pAddedValueOption = index
End Sub

Private Sub Tabs_Click()
  Dim t As Long
  
  pCreationType = Tabs.SelectedItem.Caption

  For t = 1 To Tabs.Tabs.Count
    fraTab(t).Left = 20000
  Next

  With fraTab(Tabs.SelectedItem.index)
    .Move 360, 480
    .ZOrder
  End With
End Sub

Public Sub SetDefaultTS(tsIndex As Long)
  Dim dt(6) As Long, i As Long, Filename As String
  If tsIndex > 0 And tsIndex <= nts Then
    
'    If Not pTser(tsIndex).File Is Nothing Then
'      Filename = pTser(tsIndex).File.Filename
'      For i = 0 To cboFile.ListCount
'        If cboFile.List(i) = Filename Then cboFile.text = Filename
'      Next
'    End If
    
    With pTser(tsIndex).Header
      txtConsOut.Text = .con
      txtScenOut.Text = .sen
      txtLocOut.Text = .Loc
      txtDescOut.Text = .Desc
    End With
    cboUnits.Text = pTser(tsIndex).Attrib("Units")
    txtIdOut.Text = NextID(pTser(tsIndex).File)
    cboIntervalTser.Text = cboIntervalTser.List(tsIndex - 1)
    cboAddRemoveTser.Text = cboAddRemoveTser.List(tsIndex - 1)
    cboShiftTser.Text = cboShiftTser.List(tsIndex - 1)
    cboTableTser.Text = cboTableTser.List(tsIndex - 1)
    'cboInterpolateTser.Text = cboInterpolateTser.List(tsIndex - 1)
    'cboInterpolateDates.Text = cboInterpolateDates.List(tsIndex - 1)
    With pTser(tsIndex).dates.Summary
      txtTstep.Text = .ts
      cboTunits.ListIndex = .Tu 'Default to aggregating one level of time units
      
      txtAddRemoveTstep.Text = .ts
      cboAddRemoveTunits.ListIndex = .Tu - 1
    End With
  
    J2Date pTser(tsIndex).dates.Summary.SJDay, dt
    For i = 0 To 4
      txtStartDat(i) = dt(i)
      txtCloneDat(i) = dt(i)
      txtOldDat(i) = dt(i)
      txtShiftDat(i) = dt(i)
    Next
    J2Date pTser(tsIndex).dates.Summary.EJDay, dt
    timcnv dt
    For i = 0 To 4
      txtEndDat(i) = dt(i)
    Next
  End If
End Sub

Private Function TserMatching(tsLabel As String) As ATCclsTserData
  Dim tsIndex As Long
  For tsIndex = 1 To nts
    If pTserLabel(tsIndex) = tsLabel Then
      Set TserMatching = pTser(tsIndex)
      Exit Function
    End If
  Next
End Function

Private Function NextID(tsf As ATCclsTserFile)
  Dim TakenIDs() As Boolean
  Dim retval As Long, curTs As Long
  retval = 1
  ReDim TakenIDs(tsf.DataCount)
  For curTs = 1 To tsf.DataCount
    retval = tsf.Data(curTs).Header.ID
    If retval <= tsf.DataCount Then TakenIDs(retval) = True
  Next
  For retval = 1 To tsf.DataCount
    If TakenIDs(retval) = False Then GoTo FoundIt
  Next
FoundIt:
  NextID = retval
End Function

Private Sub txtRunningAfter_Change()
  SaveSetting "Aqua Terra Units", "New Time Series", "Running After", txtRunningAfter.Text
End Sub

Private Sub txtRunningBefore_Change()
  SaveSetting "Aqua Terra Units", "New Time Series", "Running Before", txtRunningBefore.Text
End Sub
