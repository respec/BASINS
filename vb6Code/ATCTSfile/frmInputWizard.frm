VERSION 5.00
Begin VB.Form frmInputWizard 
   Caption         =   "Script Creation Wizard"
   ClientHeight    =   7095
   ClientLeft      =   60
   ClientTop       =   345
   ClientWidth     =   14205
   BeginProperty Font 
      Name            =   "Courier New"
      Size            =   10.5
      Charset         =   0
      Weight          =   400
      Underline       =   0   'False
      Italic          =   0   'False
      Strikethrough   =   0   'False
   EndProperty
   HelpContextID   =   802
   LinkTopic       =   "Form1"
   ScaleHeight     =   7095
   ScaleWidth      =   14205
   Begin VB.Frame fraTab 
      BorderStyle     =   0  'None
      Caption         =   "Frame1"
      Height          =   3372
      Index           =   2
      Left            =   7920
      TabIndex        =   38
      Top             =   600
      Visible         =   0   'False
      Width           =   6852
      Begin ATCoCtl.ATCoGrid agdDataMapping 
         Height          =   3015
         Left            =   0
         TabIndex        =   23
         Top             =   0
         Width           =   7815
         _ExtentX        =   13785
         _ExtentY        =   5318
         SelectionToggle =   0   'False
         AllowBigSelection=   -1  'True
         AllowEditHeader =   0   'False
         AllowLoad       =   0   'False
         AllowSorting    =   -1  'True
         Rows            =   1
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
         BackColorBkg    =   8421504
         BackColorSel    =   -2147483635
         ForeColorSel    =   -2147483634
         BackColorFixed  =   -2147483633
         ForeColorFixed  =   -2147483630
         InsideLimitsBackground=   16777215
         OutsideHardLimitBackground=   8421631
         OutsideSoftLimitBackground=   8454143
         ComboCheckValidValues=   -1  'True
      End
   End
   Begin VB.Frame fraTab 
      BorderStyle     =   0  'None
      Caption         =   "Frame1"
      Height          =   2412
      Index           =   1
      Left            =   280
      TabIndex        =   39
      Top             =   600
      Width           =   7056
      Begin VB.TextBox txtScriptDesc 
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   285
         Left            =   1200
         TabIndex        =   5
         Text            =   "txtScriptDesc"
         Top             =   600
         Width           =   5052
      End
      Begin VB.Frame fraHeader 
         Caption         =   "Header"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   1332
         Left            =   0
         TabIndex        =   42
         Top             =   960
         Width           =   1932
         Begin VB.TextBox txtHeaderLines 
            BeginProperty Font 
               Name            =   "MS Sans Serif"
               Size            =   8.25
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   288
            Left            =   1440
            TabIndex        =   11
            Text            =   "1"
            ToolTipText     =   "Single printable character delimiter"
            Top             =   960
            Width           =   372
         End
         Begin VB.CheckBox chkSkipHeader 
            Caption         =   "Skip"
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
            TabIndex        =   6
            ToolTipText     =   "Do not search header for any information"
            Top             =   240
            Value           =   1  'Checked
            Width           =   1692
         End
         Begin VB.OptionButton optHeader 
            Caption         =   "Lines"
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
            Index           =   3
            Left            =   120
            TabIndex        =   10
            Top             =   960
            Width           =   1335
         End
         Begin VB.OptionButton optHeader 
            Caption         =   "Starts With"
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
            Index           =   2
            Left            =   120
            TabIndex        =   8
            Top             =   720
            Width           =   1332
         End
         Begin VB.OptionButton optHeader 
            Caption         =   "None"
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
            Left            =   120
            TabIndex        =   7
            Top             =   480
            Value           =   -1  'True
            Width           =   1692
         End
         Begin VB.TextBox txtHeaderStart 
            BeginProperty Font 
               Name            =   "MS Sans Serif"
               Size            =   8.25
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   285
            Left            =   1440
            TabIndex        =   9
            Text            =   "#"
            ToolTipText     =   "Single printable character delimiter"
            Top             =   720
            Width           =   372
         End
      End
      Begin VB.Frame fraLineEnd 
         Caption         =   "Line Ending"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   1332
         Left            =   4080
         TabIndex        =   41
         Top             =   960
         Width           =   1932
         Begin VB.TextBox txtLineLen 
            BeginProperty Font 
               Name            =   "MS Sans Serif"
               Size            =   8.25
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   288
            Left            =   1440
            TabIndex        =   22
            Text            =   "80"
            ToolTipText     =   "Single printable character delimiter"
            Top             =   960
            Width           =   372
         End
         Begin VB.TextBox txtLineEndChar 
            BeginProperty Font 
               Name            =   "MS Sans Serif"
               Size            =   8.25
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   285
            Left            =   1440
            TabIndex        =   20
            Text            =   "13"
            ToolTipText     =   "Single printable character delimiter"
            Top             =   720
            Width           =   372
         End
         Begin VB.OptionButton optLineEnd 
            Caption         =   "CR/LF or CR"
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
            Left            =   120
            TabIndex        =   17
            Top             =   240
            Value           =   -1  'True
            Width           =   1692
         End
         Begin VB.OptionButton optLineEnd 
            Caption         =   "ASCII Char:"
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
            Index           =   2
            Left            =   120
            TabIndex        =   19
            Top             =   720
            Width           =   1452
         End
         Begin VB.OptionButton optLineEnd 
            Caption         =   "LF"
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
            Left            =   120
            TabIndex        =   18
            Top             =   480
            Width           =   1452
         End
         Begin VB.OptionButton optLineEnd 
            Caption         =   "Line Length:"
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
            Index           =   3
            Left            =   120
            TabIndex        =   21
            Top             =   960
            Width           =   1452
         End
      End
      Begin VB.Frame fraColumns 
         Caption         =   "Column Format"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   1332
         Left            =   2040
         TabIndex        =   40
         Top             =   960
         Width           =   1932
         Begin VB.OptionButton optDelimiter 
            Caption         =   "Character:"
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
            Index           =   3
            Left            =   120
            TabIndex        =   15
            Top             =   960
            Width           =   1335
         End
         Begin VB.OptionButton optDelimiter 
            Caption         =   "Tab Delimited"
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
            Index           =   2
            Left            =   120
            TabIndex        =   13
            Top             =   480
            Width           =   1692
         End
         Begin VB.OptionButton optDelimiter 
            Caption         =   "Space Delimited"
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
            Left            =   120
            TabIndex        =   14
            Top             =   720
            Width           =   1750
         End
         Begin VB.TextBox txtDelimiter 
            BeginProperty Font 
               Name            =   "MS Sans Serif"
               Size            =   8.25
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   285
            Left            =   1440
            TabIndex        =   16
            Text            =   ","
            ToolTipText     =   "Single printable character delimiter"
            Top             =   960
            Width           =   255
         End
         Begin VB.OptionButton optDelimiter 
            Caption         =   "Fixed Width"
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
            Left            =   120
            TabIndex        =   12
            Top             =   240
            Value           =   -1  'True
            Width           =   1692
         End
      End
      Begin VB.CommandButton cmdBrowseDesc 
         Caption         =   "Browse"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   285
         Left            =   6360
         TabIndex        =   4
         Top             =   360
         Width           =   855
      End
      Begin VB.TextBox txtScriptFile 
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   285
         Left            =   1200
         TabIndex        =   3
         Text            =   "txtScriptFile"
         Top             =   360
         Width           =   5052
      End
      Begin VB.CommandButton cmdBrowseData 
         Caption         =   "Browse"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   285
         Left            =   6360
         TabIndex        =   2
         Top             =   0
         Width           =   855
      End
      Begin VB.TextBox txtDataFile 
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   285
         Left            =   1200
         TabIndex        =   1
         Text            =   "txtDataFile"
         ToolTipText     =   "Name of file containing data to import"
         Top             =   0
         Width           =   5052
      End
      Begin VB.Label lblScriptDesc 
         Alignment       =   1  'Right Justify
         Caption         =   "Description:"
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
         TabIndex        =   45
         Top             =   600
         Width           =   1092
      End
      Begin VB.Label lblDataDescFile 
         Caption         =   "Script File:"
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
         TabIndex        =   44
         Top             =   360
         Width           =   1212
      End
      Begin VB.Label lblDataFile 
         Caption         =   "Data File:"
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
         Left            =   0
         TabIndex        =   43
         Top             =   0
         Width           =   1212
      End
   End
   Begin ComctlLib.TabStrip tabTop 
      CausesValidation=   0   'False
      Height          =   3012
      Left            =   120
      TabIndex        =   0
      Top             =   120
      Width           =   7452
      _ExtentX        =   13150
      _ExtentY        =   5318
      _Version        =   327682
      BeginProperty Tabs {0713E432-850A-101B-AFC0-4210102A8DA7} 
         NumTabs         =   2
         BeginProperty Tab1 {0713F341-850A-101B-AFC0-4210102A8DA7} 
            Caption         =   "File Properties"
            Key             =   ""
            Object.Tag             =   ""
            ImageVarType    =   2
         EndProperty
         BeginProperty Tab2 {0713F341-850A-101B-AFC0-4210102A8DA7} 
            Caption         =   "Data Mapping"
            Key             =   ""
            Object.Tag             =   ""
            ImageVarType    =   2
         EndProperty
      EndProperty
      BeginProperty Font {0BE35203-8F91-11CE-9DE3-00AA004BB851} 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
   End
   Begin VB.Frame fraSash 
      BorderStyle     =   0  'None
      Caption         =   "Frame3"
      Height          =   130
      Left            =   0
      MousePointer    =   7  'Size N S
      TabIndex        =   37
      Top             =   3240
      Width           =   8052
   End
   Begin MSComDlg.CommonDialog dlgOpenFile 
      Left            =   120
      Top             =   0
      _ExtentX        =   847
      _ExtentY        =   847
      _Version        =   393216
   End
   Begin VB.Frame fraButtons 
      BorderStyle     =   0  'None
      Caption         =   "Frame3"
      Height          =   372
      Left            =   1440
      TabIndex        =   35
      Top             =   6600
      Width           =   4452
      Begin VB.CommandButton cmdSaveDesc 
         Caption         =   "&Save Script"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   375
         Left            =   1560
         TabIndex        =   25
         ToolTipText     =   "Save selections and data mapping information to a data descriptor file."
         Top             =   0
         Width           =   1335
      End
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
         Height          =   375
         Left            =   3120
         TabIndex        =   26
         Top             =   0
         Width           =   1335
      End
      Begin VB.CommandButton cmdOk 
         Caption         =   "&Read Data"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   375
         Left            =   0
         TabIndex        =   24
         Top             =   0
         Width           =   1335
      End
   End
   Begin VB.Frame fraTextSample 
      BorderStyle     =   0  'None
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   2175
      Left            =   120
      TabIndex        =   27
      Top             =   3480
      Width           =   7335
      Begin VB.VScrollBar VScrollSample 
         Height          =   1212
         LargeChange     =   5
         Left            =   7080
         TabIndex        =   36
         Top             =   480
         Width           =   200
      End
      Begin VB.HScrollBar HScrollSample 
         Height          =   200
         LargeChange     =   40
         Left            =   0
         Max             =   1000
         Min             =   1
         TabIndex        =   34
         Top             =   1972
         Value           =   1
         Width           =   7332
      End
      Begin VB.TextBox txtRuler2 
         BackColor       =   &H8000000F&
         BorderStyle     =   0  'None
         BeginProperty Font 
            Name            =   "Courier New"
            Size            =   9.75
            Charset         =   0
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   216
         HideSelection   =   0   'False
         Left            =   0
         Locked          =   -1  'True
         TabIndex        =   33
         Text            =   "1234567890"
         Top             =   240
         Width           =   7332
      End
      Begin VB.TextBox txtSample 
         BorderStyle     =   0  'None
         BeginProperty Font 
            Name            =   "Courier New"
            Size            =   9.75
            Charset         =   0
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   252
         HideSelection   =   0   'False
         Index           =   0
         Left            =   0
         Locked          =   -1  'True
         TabIndex        =   29
         Text            =   "Sample"
         Top             =   480
         Width           =   7092
      End
      Begin VB.TextBox txtRuler1 
         BackColor       =   &H8000000F&
         BorderStyle     =   0  'None
         BeginProperty Font 
            Name            =   "Courier New"
            Size            =   9.75
            Charset         =   0
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   216
         HideSelection   =   0   'False
         Left            =   0
         Locked          =   -1  'True
         TabIndex        =   32
         Text            =   "         1"
         Top             =   0
         Width           =   7332
      End
   End
   Begin VB.Frame fraColSample 
      BorderStyle     =   0  'None
      Caption         =   "fraColSample"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   2295
      Left            =   120
      TabIndex        =   28
      Top             =   3480
      Visible         =   0   'False
      Width           =   7335
      Begin ATCoCtl.ATCoGrid agdSample 
         Height          =   1932
         Left            =   0
         TabIndex        =   30
         Top             =   240
         Width           =   7332
         _ExtentX        =   12938
         _ExtentY        =   3413
         SelectionToggle =   0   'False
         AllowBigSelection=   -1  'True
         AllowEditHeader =   0   'False
         AllowLoad       =   0   'False
         AllowSorting    =   -1  'True
         Rows            =   1
         Cols            =   1
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
         SelectionMode   =   2
         BackColor       =   -2147483643
         ForeColor       =   -2147483640
         BackColorBkg    =   8421504
         BackColorSel    =   -2147483635
         ForeColorSel    =   -2147483634
         BackColorFixed  =   -2147483633
         ForeColorFixed  =   -2147483630
         InsideLimitsBackground=   0
         OutsideHardLimitBackground=   0
         OutsideSoftLimitBackground=   0
         ComboCheckValidValues=   -1  'True
      End
      Begin VB.Label lblInputColumns 
         Caption         =   "Column Number:"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   255
         Left            =   120
         TabIndex        =   31
         Top             =   0
         Width           =   2415
      End
   End
End
Attribute VB_Name = "frmInputWizard"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
'Copyright 2002 by AQUA TERRA Consultants

Private Const conNumSampleLines = 50

Private Const conSashLimit = 2100  'Sash can't get closer than this to top or bottom
Private Const conMaxNumColumns = 50

Private Const ColMappingName = 0
Private Const ColMappingAttr = 1
Private Const ColMappingCol = 2
Private Const ColMappingConstant = 3
Private Const ColMappingSkip = 4
Private Const ColMappingLast = 4

Private FoundDDF As Boolean
Private FoundPas As Boolean
Private WriteHeader As Boolean

Private UnitDescFile As Integer
Private NameDescFile As String

Private CharWidth As Single
Private SettingSelFromGrid As Boolean

Private nFixedCols As Long
Private hSashDragging As Boolean
Private delim As String            ' currently selected delimiter
Private delimQ As Boolean          ' boolean for whether file is delimited
Private SiteDataMapped As Boolean
Private DidMap As Boolean
Private mbMoving As Boolean

Private NameDataFile As String
Private UnitDataFile As Integer  'file handle for reading data

Private pMonitor As Object
Private pMonitorSet As Boolean

Private pTserFile As ATCclsTserFile
Private Script As clsATCscriptExpression

Private RequiredFields As Variant 'Array of names
Private nRequiredFields As Long

Public Property Set Monitor(ByVal o As Object)
  Set pMonitor = o
  pMonitorSet = True
End Property

Public Property Set TserFile(newValue As ATCclsTserFile)
  Set pTserFile = newValue
End Property

Private Sub chkSkipHeader_Click()
  PopulateSample
End Sub

Private Sub cmdOk_Click()
  Me.MousePointer = vbHourglass
  Set Script = Nothing
  Set Script = New clsATCscriptExpression
  
  If pMonitorSet Then
     pMonitor.SendMonitorMessage "(OPEN Reading " & NameDataFile & ")"
     pMonitor.SendMonitorMessage "(BUTTOFF CANCEL)"
     pMonitor.SendMonitorMessage "(BUTTOFF PAUSE)"
     ScriptSetMonitor pMonitor
  End If
  
  TokenStringInit
  Script.ParseExpression ScriptStringFromWizard
  ScriptRun Script, NameDataFile, pTserFile  ' MsgBox ScriptRun(Script, NameDataFile, pTserFile), vbOKOnly, "Ran Import Data Script"

  If pMonitorSet Then
     pMonitor.SendMonitorMessage "(CLOSE)"
     pMonitor.SendMonitorMessage "(BUTTON CANCEL)"
     pMonitor.SendMonitorMessage "(BUTTON PAUSE)"
  End If
  Me.MousePointer = vbDefault
  Unload Me
End Sub

'Displays the Input Wizard form and initializes
'fields and objects used in the application.
Private Sub Form_Load()
  
  RequiredFields = Array("", "Value", "Year", "Month", "Day", "Hour", "Minute", _
                         "Scenario", "Location", "Constituent", "Description", "Repeating", "Repeats")
  nRequiredFields = UBound(RequiredFields)
  InitDataMapping
  
  CharWidth = Me.TextWidth("X")
  
  tabTop.SelectedItem = tabTop.Tabs(1)
    
  ' initialize Input File Property defaults.
  txtScriptFile.Text = ""
  txtDataFile.Text = ""
  DidMap = False
  delim = txtDelimiter.Text
  DisableFilePropertiesFields

  ' Left justify major display areas on form
  fraTextSample.Left = tabTop.Left
  fraColSample.Left = tabTop.Left
  fraTab(2).Top = fraTab(1).Top
  fraTab(2).Left = fraTab(1).Left
  'fraTab(3).Top = fraTab(1).Top
  'fraTab(3).Left = fraTab(1).Left
  SizeControls fraSash.Top
End Sub

Private Sub Form_Resize()
  SizeControls fraSash.Top
End Sub

'Resize all controls on the form
Sub SizeControls(SashTop As Single)
  Dim fraWidth As Long
  Dim tabWidth As Long
  Dim txtWidth As Long
  Dim SampleHeight As Long
  Dim EachSampleHeight As Long
  Dim sam As Long
  'Dim ButtonsTop As Long
  Dim BotTop As Long
  Dim TopHeight As Long
  Dim BottomHeight As Long
  On Error Resume Next

  fraSash.Top = SashTop

  'set the height of the top objects
  tabTop.Height = SashTop - 130
  TopHeight = SashTop - 700
  fraTab(1).Height = TopHeight
  fraTab(2).Height = TopHeight
  'fraTab(3).Height = TopHeight
  agdDataMapping.Height = TopHeight
  'agdTestMapping.Height = TopHeight

  ' Set the top of frame containers for sample display.
  fraTextSample.Top = SashTop + fraSash.Height
  fraColSample.Top = SashTop + fraSash.Height

  ' Set the height of the bottom objects, do the frames
  ' then the items contained - 900 for buttons
  ' at the bottom.
  BottomHeight = Me.Height - SashTop - 900 - fraSash.Height
  
  fraTextSample.Height = BottomHeight
  SampleHeight = BottomHeight - txtRuler1.Height - txtRuler2.Height - HScrollSample.Height
  VScrollSample.Top = txtSample(0).Top
  VScrollSample.Height = SampleHeight
  EachSampleHeight = txtSample(0).Height * 0.95
  sam = txtSample.Count - 1
  While sam > 0 And txtSample(sam).Top + EachSampleHeight > SampleHeight
    Unload txtSample(sam)
    sam = sam - 1
  Wend
  While txtSample(sam).Top < SampleHeight
    sam = sam + 1
    Load txtSample(sam)
    txtSample(sam).Top = txtSample(sam - 1).Top + EachSampleHeight
    txtSample(sam).Visible = True
  Wend
  PopulateTxtSample
  HScrollSample.Top = BottomHeight - HScrollSample.Height 'txtSample(txtSample.Count - 1).Top + txtSample(txtSample.Count - 1).Height
  HScrollSample.ZOrder
  fraColSample.Height = BottomHeight
  agdSample.Height = BottomHeight - 300

  fraButtons.Top = Me.Height - 800 ' Position the bottom buttons
  
  'set the width
  tabWidth = Me.Width - 400
  fraSash.Width = Me.Width
  tabTop.Width = tabWidth
  fraColSample.Width = tabWidth
  agdSample.Width = tabWidth
  fraTextSample.Width = tabWidth
  txtWidth = tabWidth - 200
  txtRuler1.Width = txtWidth
  txtRuler2.Width = txtWidth
  txtWidth = txtWidth - VScrollSample.Width
  HScrollSample.Width = txtWidth
  VScrollSample.Left = txtWidth
  For sam = 0 To txtSample.Count - 1
    txtSample(sam).Width = txtWidth
  Next sam
  
  SetRulers
  
  ' Expand both grids to slightly smaller than the tabTop width:
  fraWidth = tabWidth - 325
  fraTab(1).Width = fraWidth
  fraTab(2).Width = fraWidth
  'fraTab(3).Width = fraWidth
  agdDataMapping.Width = fraWidth
  'agdTestMapping.Width = fraWidth

End Sub

Private Sub SetRulers()
  Dim NumChars As Long
  Dim RulerCount As Long
  Dim RulerStringTemp As String
  Dim RulerString1 As String
  Dim RulerString2 As String
  
  RulerString1 = ""
  RulerString2 = ""
  NumChars = txtRuler2.Width / CharWidth
  
  'First, fill in possibly odd number of digits caused by scrolling
  For RulerCount = HScrollSample Mod 10 To 9
    RulerString2 = RulerString2 & RulerCount
  Next RulerCount
  RulerString2 = RulerString2 & "0"
  
  RulerCount = (HScrollSample + 10 - HScrollSample Mod 10) \ 10
  RulerStringTemp = CStr(RulerCount)
  If Len(RulerString2) > Len(RulerStringTemp) Then
    RulerString1 = Space(Len(RulerString2) - Len(RulerStringTemp)) & RulerStringTemp
  Else
    RulerString1 = Space(Len(RulerString2))
  End If
  
  'Then fill in ten digits at a time until we have enough
  While Len(RulerString2) < NumChars
    RulerCount = RulerCount + 1
    RulerStringTemp = CStr(RulerCount)
    RulerStringTemp = Space(10 - Len(RulerStringTemp)) & RulerStringTemp
    RulerString1 = RulerString1 & RulerStringTemp
    RulerString2 = RulerString2 & "1234567890"
  Wend
  RulerString2 = Left(RulerString2, NumChars)
  RulerString1 = Left(RulerString1, NumChars)
  txtRuler1.Text = RulerString1
  txtRuler2.Text = RulerString2
End Sub

Private Sub HScrollSample_Change()
  SetRulers
  PopulateTxtSample
End Sub

Private Sub HScrollSample_Scroll()
  HScrollSample_Change
End Sub

Private Sub fraSash_MouseDown(Button As Integer, Shift As Integer, X As Single, Y As Single)
  mbMoving = True
  fraSash.BackColor = vbButtonShadow
End Sub

Private Sub fraSash_MouseMove(Button As Integer, Shift As Integer, X As Single, Y As Single)
  Dim sglPos As Single
  
  If mbMoving Then
    sglPos = Y + fraSash.Top
    If sglPos < conSashLimit Then
      fraSash.Top = conSashLimit
    ElseIf sglPos > Me.Height - conSashLimit Then
      fraSash.Top = Me.Height - conSashLimit
    Else
      fraSash.Top = sglPos
    End If
  End If
End Sub

'time to move the controls
Private Sub fraSash_MouseUp(Button As Integer, Shift As Integer, X As Single, Y As Single)
  mbMoving = False
  fraSash.BackColor = vbButtonFace
  SizeControls fraSash.Top
End Sub

Private Sub optHeader_Click(index As Integer)
  PopulateSample
End Sub

Private Sub agdDataMapping_CommitChange( _
            ChangeFromRow As Long, ChangeToRow As Long, _
            ChangeFromCol As Long, ChangeToCol As Long)
  If ChangeFromCol = ColMappingCol Then SetSelFromGrid ChangeFromRow
  With agdDataMapping
    If .MaxOccupiedRow = .rows Then .rows = .rows + 1
  End With
End Sub

Private Sub agdDataMapping_RowColChange()
  Static lastrow As Long, lastcol As Long
  Static InRowColChange As Boolean
  
  Dim ics As String
  Dim icl As Long
  Dim newrow As Long
  Dim newcol As Long
  If InRowColChange Then Exit Sub
  InRowColChange = True
  With agdDataMapping

    If .row <> lastrow Or .Col <> lastcol Then
      newrow = .row
      newcol = .Col
      If lastrow > nRequiredFields Or lastcol <> ColMappingName Then
        .row = lastrow
        .Col = lastcol
        .CellBackColor = .BackColor
        .CellForeColor = .ForeColor
        .row = newrow
        .Col = newcol
      End If
      If newrow > nRequiredFields Or newcol <> ColMappingName Then
        .CellBackColor = .BackColorSel
        .CellForeColor = .ForeColorSel
      End If
      .clearvalues
      If newrow = ColMappingAttr Then
        .addValue "yes"
        .addValue "no"
      End If
      lastrow = newrow
      lastcol = newcol
      SetSelFromGrid .row
    End If
    If .row > nRequiredFields Or .Col <> ColMappingName Then .ColEditable(.Col) = True
    .clearvalues
  End With
  InRowColChange = False
End Sub

Private Sub agdDataMapping_TextChange( _
            ChangeFromRow As Long, ChangeToRow As Long, _
            ChangeFromCol As Long, ChangeToCol As Long)
  EnableButtons
End Sub

Private Function FixedColLeft(index&) As Long
  FixedColLeft = ReadIntLeaveRest(Trim( _
                 agdDataMapping.TextMatrix(index, ColMappingCol)))
End Function

Private Function FixedColRight(index&) As Long
  Dim str$, pos&
  str = Trim(agdDataMapping.TextMatrix(index, ColMappingCol))
  pos = InStr(str, "-")
  
  If pos > 0 Then
    FixedColRight = CLng(Mid(str, pos + 1))
  Else
    pos = InStr(str, "+")
    If pos > 0 Then
      FixedColRight = CLng(Mid(str, pos + 1))
    ElseIf IsNumeric(str) Then
      FixedColRight = CLng(str)
    Else
      FixedColRight = 0
    End If
  End If
End Function

Private Sub SetSelFromGrid(row&)
  
  Dim SelStart&, SelLength&
  SelStart = 0
  SelLength = 0
  
  If Not SettingSelFromGrid Then
    SettingSelFromGrid = True
    
    If delimQ Then 'Select column in agdSample
    
      SelStart = FixedColLeft(row)
      If SelStart > 0 Then
        If SelStart <> agdSample.SelStartCol Then
          agdSample.Selected(1, SelStart - 1) = True
        End If
      End If
    
    Else           'Select column in txtSample
      SelStart = FixedColLeft(row) - 1
      If SelStart >= 0 Then
        SelLength = FixedColRight(row) - SelStart
        If SelStart < HScrollSample.Value Then
          SelLength = SelLength - (HScrollSample.Value - SelStart) + 1
          SelStart = HScrollSample.Value - 1
        End If
        If SelLength > 0 Then
          txtRuler1.SelStart = SelStart - HScrollSample.Value + 1
          txtRuler1.SelLength = SelLength
        Else
          txtRuler1.SelLength = 0
        End If
        txtSampleAnyChange -1
      End If
    End If
    
    SettingSelFromGrid = False
  End If
End Sub


' Subroutine ===============================================
' Name:      EnableButtons
' Purpose:   Enables/Disables 3 command buttons at bottom
'            of the form when fields are mapped.
'
' Notes: This is the only place where DidMap can get set
'        to true. DidMap true means all mapping enteries
'        have been made and its OK for the program to
'        proceed with data processing.
'
Private Sub EnableButtons()
  Dim Idx As Long
  
  cmdCancel.Enabled = True
    
  With agdDataMapping
    For Idx = 1 To agdDataMapping.rows
      'If Trim(Len(.TextMatrix(Idx, 3))) = 0 And _
      '   Trim(Len(.TextMatrix(Idx, 4))) = 0 _
      'Then
      '  DidMap = False
      '  cmdSaveDesc.Enabled = False
      '  cmdProcessData.Enabled = False
      '  cmdMakeGwsiTransactions.Enabled = False
      '  Exit Sub
      'End If
    Next
    DidMap = True

    cmdSaveDesc.Enabled = True
  End With

End Sub
' Subroutine ===============================================
' Name:      DisableButtons
' Purpose:   Disables 3 command buttons at bottom
'            of the form, used to turn off
'            buttons as data is processed.
'
Private Sub DisableButtons()
  cmdSaveDesc.Enabled = False
  cmdCancel.Enabled = True
End Sub

' Subroutine ===============================================
' Name:      SelectFieldsDone
' Purpose:   Signal passed from the SelectFields control
'            to the form
'
Public Sub SelectFieldsDone()
  Dim m_Prompt As String
  Dim m_LenPro As Integer
  
  EnableButtons
  
  ' Reset and move the user prompt:
  '
  m_Prompt = " To map " & agdDataMapping.TextMatrix(1, ColMappingName) & _
             " , click or enter a column number"
  m_LenPro = BoxWidth(m_Prompt)
  If m_LenPro > 7500 Then m_LenPro = 7500
  
  'ctlUserPrompt1.StackLines
  'ctlUserPrompt1.Width = m_LenPro
  'ctlUserPrompt1.Top = 0
  'ctlUserPrompt1.Left = cmdSelectFields.Left + _
  '                      cmdSelectFields.Width + 35
  'ctlUserPrompt1.PromptLine1 m_Prompt
  'ctlUserPrompt1.Line2Off
  'ctlUserPrompt1.Visible = True
  
End Sub

' Event     <><><><><><><><><><><><><><><><><><><><><><><><>
' Name:     agdSample_Click
' Purpose:  Responds to click on sample grid
'
Private Sub agdSample_Click()
  Dim m_Prompt As String
  Dim m_LenPro As Integer
'
' Don't change the mapping unless you are on the data mapping
' tab.
'
  If tabTop.SelectedItem.index <> 2 Then
    Exit Sub
  End If
  
  ' Select column means put this column number in current row af mapping
  If agdSample.SelStartCol = agdSample.SelEndCol _
    And agdSample.SelStartRow = 1 _
    And agdSample.SelEndRow = agdSample.rows Then
    
      agdDataMapping.TextMatrix(agdDataMapping.row, ColMappingCol) = agdSample.SelStartCol + 1
                                                'agdSample.ColTitle(agdSample.SelStartCol)
  End If
'
' This is called to enable or disable add and mod buttons
' it will only enable buttons when all data mapping is done.
'
  EnableButtons
End Sub

' Event     <><><><><><><><><><><><><><><><><><><><><><><><>
' Name:     cmdBrowseDesc_Click
' Purpose:  Responds to press of lower "Browse" button
'
Private Sub cmdBrowseDesc_Click()
  dlgOpenFile.Filter = "Wizard Script Files (*.ws)|*.ws|All Files (*.*)|*.*"
  dlgOpenFile.DefaultExt = "ws"
  dlgOpenFile.DialogTitle = "Open Script File"
  dlgOpenFile.ShowOpen
  If dlgOpenFile.Filename <> "" Then
    NameDescFile = dlgOpenFile.Filename
    txtScriptFile.Text = NameDescFile
    ReadScript
  End If
End Sub

Public Sub ReadScript()
  If Len(txtScriptFile.Text) > 0 Then
    If Len(Dir(txtScriptFile.Text)) > 0 Then
      Set Script = ScriptFromString(WholeFileString(txtScriptFile.Text))
      If Script Is Nothing Then
        MsgBox "Could not parse " & txtScriptFile.Text & vbCr & err.Description, vbExclamation, "Read Script"
      Else
        FoundDDF = True
        SetWizardFromScript Script
      End If
    End If
  End If
End Sub

Private Sub InitDataMapping()
  Dim r As Long
  With agdDataMapping
    .ClearData
    .Cols = ColMappingLast + 1
    .rows = nRequiredFields + 1
    .ColTitle(ColMappingName) = "Name"
    .ColTitle(ColMappingAttr) = "Attribute"
    .ColTitle(ColMappingCol) = "Input Column"
    .ColTitle(ColMappingConstant) = "Constant"
    .ColTitle(ColMappingSkip) = "Skip Values"
    .Col = ColMappingName
    For r = 1 To nRequiredFields
      .row = r
      .Text = RequiredFields(r)
      .CellBackColor = vbMenuBar
      Select Case RequiredFields(r)
        Case "Year":                     .TextMatrix(r, ColMappingConstant) = "1900"
        Case "Day":                      .TextMatrix(r, ColMappingConstant) = "1"
        Case "Hour", "Minute", "Second": .TextMatrix(r, ColMappingConstant) = "0"
        
        Case "Scenario", "Location", "Constituent", "Description"
                                         .TextMatrix(r, ColMappingAttr) = "yes"
        Case Else:                       .TextMatrix(r, ColMappingAttr) = "no"
      End Select
    Next r
  End With
End Sub

Private Sub SetColumnFormatFromScript(scr As clsATCscriptExpression)
  Dim rule As String, lrule As String, ColIndex As Long, SubExpIndex As Long, SubExpMax As Long
  Dim tmpstr As String, ColName$, ColCol$
  Dim StartCol As Long, ColWidth As Long
  Dim caretPos&, dollarPos&, colonPos&, r&
  ColIndex = 1
  rule = scr.SubExpression(1).Printable
  InitDataMapping
  
  FixedColumns = False
  ColumnDelimiter = ""
  If IsNumeric(rule) Then
    ColumnDelimiter = Chr(CInt(rule))
  Else
    lrule = Trim(LCase(rule))
    If lrule = "fixed" Then
      FixedColumns = True
    Else
      If InStr(lrule, "tab") Then ColumnDelimiter = ColumnDelimiter & vbTab
      If InStr(lrule, "space") Then ColumnDelimiter = ColumnDelimiter & " "
      For StartCol = 33 To 126
        Select Case StartCol
          Case 48: StartCol = 58
          Case 65: StartCol = 91
          Case 97: StartCol = 123
        End Select
        If InStr(lrule, Chr(StartCol)) > 0 Then ColumnDelimiter = ColumnDelimiter & Chr(StartCol)
      Next StartCol
      NumColumnDelimiters = Len(ColumnDelimiter)
    End If
  End If
  
  SubExpMax = scr.SubExpressionCount
  SubExpIndex = 2
  While SubExpIndex <= SubExpMax
    rule = scr.SubExpression(SubExpIndex).Printable
    ColCol = ""
    If FixedColumns Then ' start-end:name or start+len:name
ParseFixedDef:
      StartCol = ReadIntLeaveRest(rule)
      tmpstr = Left(rule, 1)
      If tmpstr = ":" Then
        ColWidth = 1
      Else
        rule = Mid(rule, 2)
        ColWidth = ReadIntLeaveRest(rule)
        If tmpstr = "-" Then ColWidth = ColWidth - StartCol + 1
      End If
      If ColWidth < 2 Then
        ColCol = StartCol
      Else
        ColCol = StartCol & "-" & ColWidth + StartCol - 1
      End If
      rule = Mid(rule, 2)
    Else 'delimited definition - expect colNum:name or name
      colonPos = InStr(rule, ":")
      If colonPos > 0 Then
        tmpstr = Left(rule, colonPos - 1)
        If IsNumeric(tmpstr) Then
          ColCol = tmpstr
          rule = Mid(rule, colonPos + 1)
        End If
      End If
    End If
    ColName = rule
    r = RowNamed(ColName)
    With agdDataMapping
      .TextMatrix(r, ColMappingName) = ColName
      .TextMatrix(r, ColMappingCol) = ColCol
      .TextMatrix(r, ColMappingConstant) = ""
    End With
    SubExpIndex = SubExpIndex + 1
  Wend
  If FixedColumns Then
    optDelimiter_Click 0
    agdDataMapping.row = 1
    SetSelFromGrid 1
  ElseIf ColumnDelimiter = " " Then
    optDelimiter_Click 1
  ElseIf ColumnDelimiter = vbTab Then
    optDelimiter_Click 2
  Else
    txtDelimiter = ColumnDelimiter
    optDelimiter_Click 3
  End If
  agdDataMapping.ColsSizeByContents
End Sub

'Finds named row in grid by non-case-sensitive comparison ignoring whitespace
'If a blank row is found at the end of the grid, that row is returned
'If no row matches, .rows + 1 is returned
Private Function RowNamed(FieldName As String) As Long
  Dim srchName As String, thisName As String, r As Long, maxRow As Long
  
  With agdDataMapping
    maxRow = .rows
    srchName = Trim(LCase(FieldName))
    For r = 1 To maxRow
      If Trim(LCase(.TextMatrix(r, ColMappingName))) = srchName Then Exit For
    Next r
    If r > maxRow Then
      If Trim(.TextMatrix(maxRow, ColMappingName)) = "" Then r = maxRow
    End If
  End With
  RowNamed = r
End Function

Private Sub SetDatePortion(scr As clsATCscriptExpression, subexpName$)
  Dim r&, otherRow&, scp As String
  r = RowNamed(subexpName)
  If r <= agdDataMapping.rows Then
    scp = TrimQuotes(scr.Printable)
    If LCase(scp) <> LCase(subexpName) Then
      agdDataMapping.TextMatrix(r, ColMappingConstant) = scp
    End If
  End If
End Sub

Private Sub SetWizardFromDate(scr As clsATCscriptExpression)
  Dim cnt&, SubExp&, subexpName$, r&
  cnt = scr.SubExpressionCount
  If cnt > 0 Then SetDatePortion scr.SubExpression(1), "Year"
  If cnt > 1 Then SetDatePortion scr.SubExpression(2), "Month"
  If cnt > 2 Then SetDatePortion scr.SubExpression(3), "Day"
  If cnt > 3 Then SetDatePortion scr.SubExpression(4), "Hour"
  If cnt > 4 Then SetDatePortion scr.SubExpression(5), "Minute"
  If cnt > 5 Then SetDatePortion scr.SubExpression(6), "Second"
End Sub

Public Sub SetWizardFromScript(scr As clsATCscriptExpression)
  Static ReverseLogic As Boolean
  Dim ForMax As Long, SubExp As Long, str As String, str2 As String, r As Long
  Select Case scr.Token
    Case tok_And
      ForMax = scr.SubExpressionCount
      For SubExp = 1 To ForMax
        SetWizardFromScript scr.SubExpression(SubExp)
      Next
    Case tok_ATCScript
      ForMax = scr.SubExpressionCount
      txtScriptDesc.Text = TrimQuotes(scr.SubExpression(1).Printable)
      For SubExp = 2 To ForMax
        SetWizardFromScript scr.SubExpression(SubExp)
      Next
    Case tok_Attribute
      str = scr.SubExpression(1).Printable
      r = RowNamed(str)
      agdDataMapping.TextMatrix(r, ColMappingName) = str
      agdDataMapping.TextMatrix(r, ColMappingAttr) = "yes"
      str2 = Trim(scr.SubExpression(2).Printable)
      If Left(str2, 1) = """" Then str2 = Mid(str2, 2)
      If Right(str2, 1) = """" Then str2 = Left(str2, Len(str2) - 1)
      agdDataMapping.TextMatrix(r, ColMappingConstant) = str2
    Case tok_ColumnFormat:  SetColumnFormatFromScript scr
    Case tok_Dataset:
      ForMax = scr.SubExpressionCount
      SubExp = 1
      While SubExp < ForMax
        str = scr.SubExpression(SubExp).Printable
        r = RowNamed(str)
        agdDataMapping.TextMatrix(r, ColMappingName) = str
        agdDataMapping.TextMatrix(r, ColMappingAttr) = "yes"
        SubExp = SubExp + 1
        If scr.SubExpression(SubExp).Token = tok_Literal Then
          str2 = TrimQuotes(scr.SubExpression(SubExp).Printable)
          agdDataMapping.TextMatrix(r, ColMappingConstant) = str2
        End If
        SubExp = SubExp + 1
      Wend
    Case tok_Date:       SetWizardFromDate scr
    Case tok_FatalError:
    Case tok_For
      If LCase(Trim(scr.SubExpression(1).Printable)) = "repeat" _
         And scr.SubExpression(3).Token = tok_Literal Then
        r = RowNamed("Repeats")
        agdDataMapping.TextMatrix(r, ColMappingConstant) = TrimQuotes(scr.SubExpression(3).Printable)
      End If
      ForMax = scr.SubExpressionCount
      For SubExp = 4 To ForMax
        SetWizardFromScript scr.SubExpression(SubExp)
      Next
    Case tok_If
      ForMax = scr.SubExpressionCount
      For SubExp = 1 To ForMax
        SetWizardFromScript scr.SubExpression(SubExp)
      Next
    Case tok_In
      r = RowNamed(scr.SubExpression(1).Printable)
      If r <= agdDataMapping.rows Then
        ForMax = scr.SubExpressionCount
        SubExp = 2
        str = ""
        While SubExp <= ForMax
          str = str & scr.SubExpression(SubExp).Printable
          SubExp = SubExp + 1
          If SubExp <= ForMax Then str = str & ","
        Wend
        If ReverseLogic Then
          agdDataMapping.TextMatrix(r, ColMappingSkip) = str
        End If
      End If
    Case tok_Increment:
    Case tok_LineEnd:
      str = UCase(scr.SubExpression(1).Printable)
      If IsNumeric(str) Then
        txtLineLen.Text = str
        optLineEnd(3).Value = True
      ElseIf Left(str, 1) = "A" And IsNumeric(Mid(str, 2)) Then
        txtLineEndChar.Text = Mid(str, 2)
        optLineEnd(2).Value = True
      ElseIf str = "CR" Then optLineEnd(0).Value = True
      ElseIf str = "LF" Then optLineEnd(1).Value = True
      Else: MsgBox "Unknown LineEnd '" & str & "' in SetWizardFromScript"
      End If
    Case tok_Literal:
    Case tok_Mid
    Case tok_Not:
      ReverseLogic = Not ReverseLogic
      SetWizardFromScript scr.SubExpression(1)
      ReverseLogic = Not ReverseLogic
    Case tok_NextLine:
      If scr.SubExpressionCount = 1 Then
        chkSkipHeader.Value = vbChecked
        optHeader(3).Value = True
        txtHeaderLines.Text = scr.SubExpression(1).Printable
      End If
    Case tok_Set:
    Case tok_Value:
    Case tok_Variable:
    Case tok_Warn:
    Case tok_While
      If scr.SubExpressionCount = 2 Then
        If scr.SubExpression(2).Token = tok_NextLine Then
          str = scr.SubExpression(1).Printable
          SubExp = Len("(= HeaderStart ")
          If Left(str, SubExp) = "(= HeaderStart " Then
            chkSkipHeader.Value = vbChecked
            optHeader(2).Value = True
            txtHeaderStart.Text = Mid(str, SubExp + 1, Len(str) - SubExp - 1)
            Exit Sub
          End If
        End If
      End If
      ForMax = scr.SubExpressionCount
      For SubExp = 2 To ForMax
        SetWizardFromScript scr.SubExpression(SubExp)
      Next
    Case tok_GT:
    Case tok_GE:
    Case tok_LT:
    Case tok_LE:
    Case tok_NE
      r = RowNamed(scr.SubExpression(1).Printable)
      If r <= agdDataMapping.rows Then
        If Not ReverseLogic Then
          agdDataMapping.TextMatrix(r, ColMappingSkip) = scr.SubExpression(2).Printable
        End If
      End If
    Case tok_EQ
      r = RowNamed(scr.SubExpression(1).Printable)
      If r <= agdDataMapping.rows Then
        If ReverseLogic Then
          agdDataMapping.TextMatrix(r, ColMappingSkip) = scr.SubExpression(2).Printable
        End If
      End If
    Case Else
  End Select
  
End Sub

Private Function TrimQuotes(str$) As String
  Dim retval As String
  retval = Trim(str)
  If Left(retval, 1) = """" Then retval = Mid(retval, 2)
  If Right(retval, 1) = """" Then retval = Left(retval, Len(retval) - 1)
  TrimQuotes = retval
End Function

Private Function ScriptStringFromWizard() As String
  Dim str As String, tmpstr As String, tmpstr2 As String
  Dim ParsePos As Long
  Dim indent As Long, indentIncrement As Long
  Dim RepeatStart As Long, RepeatEnd As Long
  Dim r As Long, commaPos As Long, NestedIfs As Long, CurrentIf As Long
  Dim SomeAttribVaries As Boolean       'True if at least one attribute is not constant
  Dim SomeAttribVariesRepeat As Boolean 'True if an attribute varies within a line
  
  indent = 2
  indentIncrement = 2
  RepeatStart = 0
  RepeatEnd = 0
  str = "(ATCScript "
  tmpstr = Trim(txtScriptDesc.Text)
  If tmpstr = "" Then tmpstr = FilenameOnly(txtScriptFile.Text)
  If tmpstr = "" Then tmpstr = "ReadData"
  str = str & """" & tmpstr & """"
  str = str & PrintEOL & Space(indent) & "(LineEnd "
  If optLineEnd(0).Value = True Then
    str = str & "CR"
  ElseIf optLineEnd(1).Value = True Then
    str = str & "LF"
  ElseIf optLineEnd(2).Value = True Then
    str = str & "A" & Trim(txtLineEndChar.Text)
  ElseIf optLineEnd(3).Value = True Then
    str = str & Trim(txtLineLen.Text)
  End If
  str = str & ")" & PrintEOL
  
  If chkSkipHeader.Value = vbChecked Then
    If optHeader(2).Value Then 'Starts With
      str = str & Space(indent) & "(While (= HeaderStart """ & txtHeaderStart.Text & """)" & PrintEOL
      str = str & Space(indent) & "       (NextLine))" & PrintEOL
    ElseIf optHeader(3).Value Then 'Number of lines
      str = str & Space(indent) & "(NextLine " & txtHeaderLines.Text & ")" & PrintEOL
    End If
  End If
  
  str = str & Space(indent) & "(ColumnFormat "
  indent = indent + Len("(ColumnFormat ")
  If Not delimQ Then
    str = str & "Fixed"
  Else 'delimited
    For ParsePos = 1 To Len(delim)
      tmpstr = Mid(delim, ParsePos, 1)
      Select Case tmpstr
        Case vbTab:        str = str & "tab"
        Case " ":          str = str & "space"
        Case Else:         str = str & tmpstr
      End Select
    Next
  End If
  If optHeader(2).Value Then 'Starts With
    str = str & PrintEOL & Space(indent) & "1"
    If Len(txtHeaderStart.Text) > 1 Then str = str & "-" & Len(txtHeaderStart.Text) - 1
    str = str & ":" & "HeaderStart"
  End If
  For r = 1 To agdDataMapping.rows
    tmpstr = Trim(agdDataMapping.TextMatrix(r, ColMappingCol))
    If tmpstr <> "" Then
      tmpstr2 = Trim(agdDataMapping.TextMatrix(r, ColMappingName))
      str = str & PrintEOL & Space(indent) & tmpstr
      str = str & ":" & tmpstr2
      
      If LCase(tmpstr2) = "repeating" Then
        RepeatStart = FixedColLeft(r)
        RepeatEnd = FixedColRight(r)
      End If
    End If
  Next
  str = str & ")"
  indent = indent - Len("(ColumnFormat ")
  
  'Figure out whether there is more than one dataset and if so whether it may change within a line
  SomeAttribVaries = False
  SomeAttribVariesRepeat = False
  For r = 1 To agdDataMapping.rows
    tmpstr = LCase(Trim(agdDataMapping.TextMatrix(r, ColMappingAttr)))
    tmpstr2 = Trim(agdDataMapping.TextMatrix(r, ColMappingCol))
    If Left(tmpstr, 1) = "y" And tmpstr2 <> "" Then
      SomeAttribVaries = True
      If RepeatStart <= FixedColLeft(r) And RepeatEnd >= FixedColRight(r) Then
        SomeAttribVariesRepeat = True
      End If
    End If
  Next
  
  If Not SomeAttribVaries Then
    For r = 1 To agdDataMapping.rows
      tmpstr = Trim(agdDataMapping.TextMatrix(r, ColMappingConstant))
      If tmpstr <> "" Then
        tmpstr2 = Trim(agdDataMapping.TextMatrix(r, ColMappingCol))
        If tmpstr2 = "" Then
          If LCase(Left(Trim(agdDataMapping.TextMatrix(r, ColMappingAttr)), 1)) = "y" Then
            str = str & PrintEOL & Space(indent) & "(Attribute "
          Else
            str = str & PrintEOL & Space(indent) & "(Set "
          End If
          str = str & Trim(agdDataMapping.TextMatrix(r, ColMappingName))
          str = str & " """ & tmpstr & """)"
        End If
      End If
    Next
  End If
  
  str = str & PrintEOL & Space(indent) & "(While (Not EOF)"
  indent = indent + Len("(While ")
  
  'Have to make sure this datum goes into the right dataset
  If SomeAttribVaries And Not SomeAttribVariesRepeat Then GoSub SelectDataset
  
  If RepeatStart > 0 And RepeatEnd >= RepeatStart Then
'    str = str & printeol & Space(indent) & "(Set Repeat 0)"
'    str = str & printeol & Space(indent) & "(While (Not EOL)"
'    indent = indent + Len("(While ")
'    str = str & printeol & Space(indent) & "(Increment Repeat)"
    str = str & PrintEOL & Space(indent) & "(For Repeat = 1 to " & ConstOrCol("Repeats")
    indent = indent + Len("(For ")
  End If
  
  NestedIfs = 0
  For r = 1 To agdDataMapping.rows
    tmpstr = Trim(agdDataMapping.TextMatrix(r, ColMappingSkip))
    If tmpstr <> "" Then NestedIfs = NestedIfs + 1
  Next r
  
  If NestedIfs > 0 Then
    str = str & PrintEOL & Space(indent) & "(If "
    indent = indent + Len("(If ")
    CurrentIf = 1
    If NestedIfs > 1 Then
      str = str & "(And "
      indent = indent + Len("(And ")
    End If
    For r = 1 To agdDataMapping.rows
      tmpstr = Trim(agdDataMapping.TextMatrix(r, ColMappingSkip))
      If tmpstr <> "" Then
        commaPos = InStr(tmpstr, ",")
        If commaPos = 0 Then
          str = str & "(<> " & ConstOrCol(Trim(agdDataMapping.TextMatrix(r, ColMappingName))) & " " & tmpstr & ")"
        Else
          str = str & "(Not (In " & ConstOrCol(Trim(agdDataMapping.TextMatrix(r, ColMappingName)))
          While commaPos > 0
            str = str & " " & Left(tmpstr, commaPos - 1)
            If commaPos > Len(tmpstr) Then
              commaPos = 0
            Else
              tmpstr = Mid(tmpstr, commaPos + 1)
              commaPos = InStr(tmpstr, ",")
              If commaPos = 0 Then commaPos = Len(tmpstr) + 1
            End If
          Wend
          str = str & "))"
        End If
        If CurrentIf < NestedIfs Then str = str & PrintEOL & Space(indent)
        CurrentIf = CurrentIf + 1
      End If
    Next r
    If NestedIfs > 1 Then
      indent = indent - Len("(And ")
      str = str & PrintEOL & Space(indent) & ")"
    End If
  End If
  
  'Have to make sure this datum goes into the right dataset
  If SomeAttribVariesRepeat Then GoSub SelectDataset
    
  str = str & PrintEOL & Space(indent) & "(Date "
  indent = indent + Len("(Date ")
  str = str & ConstOrCol("Year")
  str = str & PrintEOL & Space(indent) & ConstOrCol("Month")
  str = str & PrintEOL & Space(indent) & ConstOrCol("Day")
  str = str & PrintEOL & Space(indent) & ConstOrCol("Hour")
  str = str & PrintEOL & Space(indent) & ConstOrCol("Minute") & ")"
  'str = str & printeol & Space(indent) & ConstOrCol("Second") & ")"
  indent = indent - Len("(Date ")
  
  str = str & PrintEOL & Space(indent) & "(Value " & ConstOrCol("Value") & ")"
  
  'For r = 1 To NestedIfs
  If NestedIfs > 0 Then
    indent = indent - Len("(If ")
    str = str & PrintEOL & Space(indent) & ")"
  End If
  'Next r
  
  If RepeatStart > 0 And RepeatEnd >= RepeatStart Then
    indent = indent - Len("(For ")
    str = str & PrintEOL & Space(indent) & ")"
  End If
  
  str = str & PrintEOL & Space(indent) & "(NextLine)"
  indent = indent - Len("(While ")
  str = str & PrintEOL & Space(indent) & ")"
  
  str = str & PrintEOL & ")"
  
  ScriptStringFromWizard = str
  Exit Function

SelectDataset:
  str = str & PrintEOL & Space(indent) & "(Dataset "
  indent = indent + Len("(Dataset ")
  
  For r = 1 To agdDataMapping.rows
    tmpstr = LCase(Trim(agdDataMapping.TextMatrix(r, ColMappingAttr)))
    If Left(tmpstr, 1) = "y" Then
      tmpstr2 = Trim(agdDataMapping.TextMatrix(r, ColMappingName))
      tmpstr = ConstOrCol(tmpstr2)
      If tmpstr <> "" Then
        If Right(str, Len("(Dataset ")) <> "(Dataset " Then str = str & PrintEOL & Space(indent)
        str = str & tmpstr2 & " " & tmpstr
      End If
    End If
  Next
  str = str & ")"
  indent = indent - Len("(Dataset ")
  Return

End Function

Private Function ConstOrCol(FieldName As String)
  Dim r&, str As String, searchstr As String, colStr As String, constStr As String, retval As String
  retval = ""
  If LCase(FieldName) = "repeat" Then
    retval = FieldName
  Else
    r = RowNamed(FieldName)
    If r > agdDataMapping.rows Then
      ConstOrCol = """" & FieldName & """"
    Else
      colStr = Trim(agdDataMapping.TextMatrix(r, ColMappingCol))
      If Len(colStr) > 0 Then colStr = FieldName
      retval = colStr
      constStr = Trim(agdDataMapping.TextMatrix(r, ColMappingConstant))
      If Len(constStr) > 0 Then
        If LCase(constStr) = "repeat" Then
          retval = constStr
        Else
          Select Case Left(constStr, 1)
            Case "+", "-", "*", "/", "^"
              retval = "(" & Left(constStr, 1)
              If colStr <> "" Then retval = retval & " " & colStr
              retval = retval & " " & Trim(Mid(constStr, 2)) & ")"
            Case Else
              If retval = "" Then retval = """" & constStr & """"
          End Select
        End If
      End If
    End If
  End If
  ConstOrCol = retval
End Function

' Subroutine ===============================================
' Name:      DisableFilePropertiesFields
' Purpose:   Disable most fields on the "File-Properties"
'            tab until the data file has been successfully
'            opened.
'
' Notes: this routine is called to initialize the program
'        prior to the entry of an input data file name.
'        once a valid name is entered the routine
'        EnableFilePropertiesFields is called to allow the
'        user to continue.
'
'        Disable "Data-Mapping" and "Test-Mapping" tabs
'        until the data file has been successfully opened.
'
Private Sub DisableFilePropertiesFields()
  'FoundData = False
  FoundDDF = False
  FoundPas = False
  ' +------------------------------------------------------------+
  ' | "sstInputWizard.Enabled = False"  <-  Disables everything!  |
  ' | Alternative: Handle this in "sstInputWizard_Click" event    |
  ' +------------------------------------------------------------+
  'lblDataDescFile.Enabled = False
  'txtScriptFile.Enabled = False
  'cmdBrowseDesc.Enabled = False
  'lblFileType.Enabled = False
  'lblDelimiter.Enabled = False
  'txtDelimiter.Enabled = False
  'chkCollapseDelim.Enabled = False
  'optDelimiter.Item(0).Enabled = False
  'optDelimiter.Item(1).Enabled = False
  'optDelimiter.Item(2).Enabled = False
  'lblLinesToSkip.Enabled = False
  'txtLinesToSkip.Enabled = False
  'lblHeaderRecord.Enabled = False
  'chkHeaderRecord.Enabled = False
  'lblQuoteChar.Enabled = False
  'txtQuoteChar.Enabled = False
  'lblNullChar.Enabled = False
  'txtNullChar.Enabled = False
  'cmdSaveDesc.Enabled = False
End Sub

' Subroutine ===============================================
' Name:      EnableFilePropertiesFields
' Purpose:   Enable fields on the File-Properties tab
'            when data file has been opened
'
Private Sub EnableFilePropertiesFields()
  Dim m_Prompt As String
  Dim m_LenPro As Integer

'zzz this is not needed ?  PopulateGridSample
  lblDataDescFile.Enabled = True
  txtScriptFile.Enabled = True
  cmdBrowseDesc.Enabled = True
  'lblFileType.Enabled = True
  'lblDelimiter.Enabled = True
  'txtDelimiter.Enabled = True
  'chkCollapseDelim.Enabled = True
  'optDelimiter.Item(0).Enabled = True
  'optDelimiter.Item(1).Enabled = True
  'optDelimiter.Item(2).Enabled = True
  'lblLinesToSkip.Enabled = True
  'txtLinesToSkip.Enabled = True
  'lblHeaderRecord.Enabled = True
  'chkHeaderRecord.Enabled = True
  '
  ' Note: Leave these fields disabled until the functionality
  ' has been programmed.
  '
  ' lblQuoteChar.Enabled = True
  ' txtQuoteChar.Enabled = True
  ' lblNullChar.Enabled = True
  ' txtNullChar.Enabled = True

End Sub

' Event     <><><><><><><><><><><><><><><><><><><><><><><><>
' Name:     cmdCancel_Click
' Purpose:  Responds to press of "Cancel" button
'
Private Sub cmdCancel_Click()
  Unload frmInputWizard
End Sub

' Event     <><><><><><><><><><><><><><><><><><><><><><><><>
' Name:     cmdSaveDesc_Click
' Purpose:  Responds to press of "Save Description" button
'
Private Sub cmdSaveDesc_Click()
  Dim ScriptString As String, ScriptFilename As String
  'If MsgBox(ScriptString, vbYesNo, "Save this script?") = vbYes Then
    dlgOpenFile.Filter = "Wizard Script Files (*.ws)|*.ws|All Files|*.*"
    dlgOpenFile.DefaultExt = "ws"
    dlgOpenFile.DialogTitle = "Save Script As"
    dlgOpenFile.Filename = txtScriptFile.Text
    dlgOpenFile.ShowSave
    ScriptFilename = dlgOpenFile.Filename
    If ScriptFilename <> "" Then
      Dim OutFile As Integer
      If InStr(ScriptFilename, ".") = 0 Then ScriptFilename = ScriptFilename & ".ws"
      If Trim(txtScriptDesc.Text) = "" Then txtScriptDesc.Text = FilenameOnly(ScriptFilename)
      ScriptString = ScriptStringFromWizard
      OutFile = FreeFile
      Open ScriptFilename For Output As OutFile
      Print #OutFile, ScriptString
      Close OutFile
      SaveSetting "ATCTimeseriesImport", "Scripts", dlgOpenFile.Filename, txtScriptDesc.Text
    End If
  'End If
  
  'ctlUserPrompt1.Visible = False
  
  'modFileIO.OpenFile UnitDescFile, NameDescFile, "Output", _
  '    "Data Description Files (*.ddf)|*.ddf", "Save", dlgOpenFile
  'If UnitDescFile > 0 Then
  '  WriteDescFile UnitDescFile
  '  txtScriptFile.Text = NameDescFile
  '  Close UnitDescFile
  'Else
  '  MsgBox "ERROR: CANNOT OPEN FILE:" & vbCrLf _
  '         & Chr(34) & NameDescFile & Chr(34)
  'End If
End Sub

'Select valid column names from list of field names in active memory
Private Sub SetValidInputColNames()
  'If Not fraColSample.Visible Then
  '  tabSample.SelectedItem = tabSample.Tabs.Item(2)
  '  tabSample_Click
  'End If
  Dim c As Long
  Dim t As String
  For c = 0 To agdSample.Cols - 1
    t = agdSample.ColTitle(c)
    If delimQ Then
      If t <> CStr(c + 1) Then t = t & " (" & c + 1 & ")"
    End If
    agdDataMapping.addValue t
  Next c
End Sub

'Returns position of next character from chars in str
'Returns len(str) + 1 if none were found
Private Function FirstCharPos(start&, str$, chars$) As Long
  Dim retval&, curval&, CharPos&, LenChars&
  retval = Len(str) + 1
  LenChars = Len(chars)
  For CharPos = 1 To LenChars
    curval = InStr(start, str, Mid(chars, CharPos, 1))
    If curval > 0 And curval < retval Then retval = curval
  Next CharPos
  FirstCharPos = retval
End Function

' Function  - - - - - - - - - - - - - - - - - - - - - - - - -
' Name:     ParseInputLine (3 arguments)
' Purpose:  Returns number of columns parsed from buffer into array
'           Populates parsed array from element 1 to index returned
'
Private Function ParseInputLine( _
                 ByVal InBuf As String, _
                 parsed() As String) As Long

  Dim parseCol As Long
  Dim fromCol As Long
  Dim toCol As Long
  parseCol = 0
  fromCol = 1
  If delimQ Then 'parse delimited text
    While fromCol <= Len(InBuf) And parseCol < UBound(parsed)
      toCol = FirstCharPos(fromCol, InBuf, delim)
      'If chkCollapseDelim.Value = 1 Then 'treat multiple contiguous delimiters as one
       ' While toCol = fromCol And toCol < Len(inBuf)
        '  toCol = fromCol + 1
         ' toCol = InStr(fromCol, inBuf, delim)
       ' Wend
      'End If
      If toCol < fromCol Then toCol = Len(InBuf) + 1
      parseCol = parseCol + 1
      parsed(parseCol) = Mid(InBuf, fromCol, toCol - fromCol)
      fromCol = toCol + 1
    Wend
  Else 'fixed columns
    While parseCol < nFixedCols
      parseCol = parseCol + 1
      toCol = FixedColRight(parseCol)
      If toCol > 0 Then
        fromCol = FixedColLeft(parseCol)
        parsed(parseCol) = Mid(InBuf, fromCol, toCol - fromCol + 1)
      Else
        parsed(parseCol) = ""
      End If
    Wend
  End If
  If parseCol > UBound(parsed) Then parseCol = UBound(parsed)
  ParseInputLine = parseCol
End Function

' Subroutine ===============================================
' Name:      PopulateGridTest
' Purpose:   Populates the Test-Mapping grid with data
'            fields selected on the Data-Mapping tab
'
Private Sub PopulateGridTest()
  Dim lines As Long
  Dim linecnt As Long
  Dim cbuff As String
  Dim parsed(conMaxNumColumns) As String
  Dim pcols As Long
  Dim cout As Long
  Dim cin As Long   ' column out (agdTestMapping), in (agdDataMapping)
  Dim icl As Long
  Dim ics As String ' input column long, string
  
  'Call objParser.AssignProperties( _
  '     cboFileType.text, _
  '     delim, _
  '     txtQuoteChar.text)
  
'  Seek UnitDataFile, 1
'  If IsNumeric(txtLinesToSkip.Text) Then
'    linecnt = 0
'    lines = CLng(txtLinesToSkip.Text)
'    While linecnt < lines And Not ScriptEndOfData
'      Line Input #UnitDataFile, cbuff
'      linecnt = linecnt + 1
'    Wend
'  End If
'  With agdTestMapping
'    .Clear
'    .Cols = agdDataMapping.MaxOccupiedRow
'    .rows = 1
'    lines = 50  'txtSample.Height / TextHeight("X")
'    linecnt = 0
'    For cout = 0 To .Cols - 1
'      .ColTitle(cout) = agdDataMapping.TextMatrix(cout + 1, ColMappingCol) '& _
'                        '", " & agdDataMapping.TextMatrix(cout + 1, ColMappingLookup)
'    Next cout
'    'If chkHeaderRecord.Value = 1 Then Line Input #UnitDataFile, cbuff
'    While Not ScriptEndOfData And linecnt < lines
'      Line Input #UnitDataFile, cbuff
'      pcols = ParseInputLine(cbuff, parsed)
'      linecnt = linecnt + 1
'      For cout = 0 To .Cols - 1
'        ics = agdDataMapping.TextMatrix(cout + 1, ColMappingCol)
'        If Len(ics) > 0 Then
'          If delimQ Then
'            If IsNumeric(ics) Then
'              icl = CLng(ics)
'              If icl <= pcols Then .TextMatrix(linecnt, cout) = parsed(icl)
'            End If
'          Else
'            .TextMatrix(linecnt, cout) = parsed(cout + 1)
'          End If
'        Else
'          .TextMatrix(linecnt, cout) = _
'           agdDataMapping.TextMatrix(cout + 1, ColMappingConstant)
'        End If
'      Next cout
'    Wend
'    Seek UnitDataFile, 1
'  End With
End Sub

' Subroutine ===============================================
' Name:      PopulateGridSample
' Purpose:   For delimited text files, populates the sample
'            grid with conNumSampleLines of sample lines of
'            data.
'
Private Sub PopulateGridSample()
  Dim lines As Long
  Dim linecnt As Long
  Dim cbuff As String
  Dim parsed(conMaxNumColumns) As String
  Dim pcols As Long
  Dim c As Long
  If UnitDataFile > 0 Then
    Seek UnitDataFile, 1
  '  If IsNumeric(txtLinesToSkip.Text) Then
  '    linecnt = 0
  '    lines = CLng(txtLinesToSkip.Text)
  '    While linecnt < lines And Not ScriptEndOfData
  '      Line Input #UnitDataFile, cbuff
  '      linecnt = linecnt + 1
  '    Wend
  '  End If
    With agdSample
      .Clear
      .Cols = 1
      .rows = 1
      lines = conNumSampleLines
      linecnt = 0
  '    If chkHeaderRecord.Value = 1 Then
  '      Line Input #UnitDataFile, cbuff
  '      pcols = ParseInputLine(cbuff, parsed)
  '      .Cols = pcols
  '      For c = 1 To pcols
  '        .ColTitle(c - 1) = parsed(c)
  '        .ColEditable(c - 1) = True
  '      Next c
  '    End If
      While Not EOF(UnitDataFile) And linecnt < lines
        Line Input #UnitDataFile, cbuff
        pcols = ParseInputLine(cbuff, parsed)
        linecnt = linecnt + 1
        For c = 1 To pcols
          .TextMatrix(linecnt, c - 1) = parsed(c)
        Next c
      Wend
      'If chkHeaderRecord.Value <> 1 Then 'number columns
      For c = 0 To .Cols - 1
        If delimQ _
          Then .ColTitle(c) = c + 1 _
          Else .ColTitle(c) = FixedColLeft(c + 1) & "-" & FixedColRight(c + 1)
      Next c
      'End If
      Seek UnitDataFile, 1
    End With
  End If
End Sub

' Subroutine ===============================================
' Name:      PopulateTxtSample
' Purpose:   For fixed format files, populates the Sample
'            text box with sample data
Private Sub PopulateTxtSample()
  Dim linecnt As Long
  Dim cbuff As String
  Dim nChars As Long
  
  InputLineLen = 0
  If optLineEnd(0).Value = True Then
    InputEOL = vbCr
  ElseIf optLineEnd(1).Value = True Then
    InputEOL = vbLf
  ElseIf optLineEnd(2).Value = True Then
    If IsNumeric(txtLineEndChar.Text) Then InputEOL = Chr(txtLineEndChar.Text)
  ElseIf optLineEnd(3).Value = True Then
    If IsNumeric(txtLineLen.Text) Then InputLineLen = Trim(txtLineLen.Text)
  End If
  LenInputEOL = Len(InputEOL)
  
  linecnt = 0
  NextLineStart = 1
  If LenDataFile > 0 Then
  
    'On Error GoTo exitsub
    'Debug.Print "PopulateTxtSample " & Time
    'Skip lines vertical scroll has scrolled past
    
    If chkSkipHeader.Value = vbChecked Then
      If optHeader(2).Value = True And Len(txtHeaderStart) > 0 Then 'skip header lines starting with string
        CurrentLine = txtHeaderStart
        While Not ScriptEndOfData And Left(CurrentLine, Len(txtHeaderStart)) = txtHeaderStart
          ScriptNextLine
        Wend
      ElseIf optHeader(3).Value = True And IsNumeric(txtHeaderLines) Then 'Skip number of header lines
        linecnt = txtHeaderLines
        While Not ScriptEndOfData And linecnt > 0
          ScriptNextLine
          linecnt = linecnt - 1
        Wend
      End If
    End If
    
    While Not ScriptEndOfData And linecnt < VScrollSample.Value
      ScriptNextLine
      linecnt = linecnt + 1
    Wend

    'Read portion of lines right of horizontal scroll position
    nChars = txtSample(0).Width / CharWidth - 1
    linecnt = 0
    While Not ScriptEndOfData And linecnt < txtSample.Count
      ScriptNextLine
      txtSample(linecnt).Text = Mid(CurrentLine, HScrollSample, nChars)
      txtSample(linecnt).SelStart = txtRuler1.SelStart
      txtSample(linecnt).SelLength = txtRuler1.SelLength
      linecnt = linecnt + 1
    Wend
    EnableFilePropertiesFields
  End If
  While linecnt < txtSample.Count
    txtSample(linecnt).Text = ""
    linecnt = linecnt + 1
  Wend
exitsub:
Exit Sub

End Sub


' Event     <><><><><><><><><><><><><><><><><><><><><><><><>
' Name:     optDelimiter_Click (1 argument)
' Purpose:  Responds to press of a "Delimiter-option" button
'
Private Sub optDelimiter_Click(index As Integer)
  Select Case index
    Case 0: delimQ = False
    Case 1: delimQ = True: delim = " "
    Case 2: delimQ = True: delim = Chr$(9)
    Case 3: delimQ = True: delim = txtDelimiter.Text
  End Select
  
  If delimQ Then
    fraTextSample.Visible = False
    fraColSample.Visible = True
    agdDataMapping.ColTitle(ColMappingCol) = "Input Column"
  Else
    fraColSample.Visible = False
    fraTextSample.Visible = True
    txtSample(0).Top = txtRuler2.Top + txtRuler2.Height
    agdDataMapping.ColTitle(ColMappingCol) = "Beg-End Column"
  End If
  PopulateSample
End Sub

Private Sub PopulateSample()
  If fraColSample.Visible Then PopulateGridSample
  If fraTextSample.Visible Then PopulateTxtSample
End Sub

Private Sub optLineEnd_Click(index As Integer)
  PopulateSample
End Sub

Private Sub tabTop_Click()
  If tabTop.SelectedItem.index = 1 Then fraTab(1).Visible = True Else fraTab(1).Visible = False
  If tabTop.SelectedItem.index = 2 Then fraTab(2).Visible = True Else fraTab(2).Visible = False
  'If tabTop.SelectedItem.index = 3 Then fraTab(3).Visible = True Else fraTab(3).Visible = False
  'If fraTab(3).Visible Then PopulateGridTest
End Sub

Private Sub txtScriptFile_GotFocus()

'-----------------------------------
'highlight upon selection so that the
'next user input key begins a new entry,
'clearing the entire previous entry.
'-----------------------------------
'In the mytextbox gotfocus event:
   txtScriptFile.SelStart = 0
   txtScriptFile.SelLength = Len(txtDataFile.Text)
    
End Sub

Private Sub txtHeaderLines_Change()
  optHeader(3).Value = True
  PopulateSample
End Sub

Private Sub txtHeaderStart_Change()
  optHeader(2).Value = True
  PopulateSample
End Sub

Private Sub txtLineEndChar_Change()
  optLineEnd(2).Value = True
  PopulateSample
End Sub

Private Sub txtLineLen_Change()
  optLineEnd(3).Value = True
  PopulateSample
End Sub

Private Sub txtRuler1_MouseDown(Button As Integer, Shift As Integer, X As Single, Y As Single)
  If Button = 1 Then txtSampleAnyChange -1
End Sub

Private Sub txtRuler1_MouseMove(Button As Integer, Shift As Integer, X As Single, Y As Single)
  If Button = 1 Then txtSampleAnyChange -1
End Sub

Private Sub txtRuler1_MouseUp(Button As Integer, Shift As Integer, X As Single, Y As Single)
  If Button = 1 Then txtSampleAnyChange -1
End Sub

Private Sub txtRuler2_MouseDown(Button As Integer, Shift As Integer, X As Single, Y As Single)
  If Button = 1 Then txtSampleAnyChange -2
End Sub

Private Sub txtRuler2_MouseMove(Button As Integer, Shift As Integer, X As Single, Y As Single)
  If Button = 1 Then txtSampleAnyChange -2
End Sub

Private Sub txtRuler2_MouseUp(Button As Integer, Shift As Integer, X As Single, Y As Single)
  If Button = 1 Then txtSampleAnyChange -2
End Sub

Private Sub txtSample_MouseDown(index As Integer, Button As Integer, Shift As Integer, X As Single, Y As Single)
  If Button = 1 Then txtSampleAnyChange index
End Sub

Private Sub txtSample_MouseMove(index As Integer, Button As Integer, Shift As Integer, X As Single, Y As Single)
  If Button = 1 Then txtSampleAnyChange index
End Sub

Private Sub txtSample_MouseUp(index As Integer, Button As Integer, Shift As Integer, X As Single, Y As Single)
  If Button = 1 Then txtSampleAnyChange index
End Sub

Private Sub txtSampleAnyChange(whichChanged As Integer)
  Dim SelStart&, SelLength&, sam&
  Select Case whichChanged
    Case -1:   SelStart = txtRuler1.SelStart: SelLength = txtRuler1.SelLength
    Case -2:   SelStart = txtRuler2.SelStart: SelLength = txtRuler2.SelLength
    Case Else: SelStart = txtSample(whichChanged).SelStart
              SelLength = txtSample(whichChanged).SelLength
  End Select
  
  If txtRuler1.SelStart <> SelStart Then txtRuler1.SelStart = SelStart
  If txtRuler2.SelStart <> SelStart Then txtRuler2.SelStart = SelStart
  
  If txtRuler1.SelLength <> SelLength Then txtRuler1.SelLength = SelLength
  If txtRuler2.SelLength <> SelLength Then txtRuler2.SelLength = SelLength
  
  For sam = 0 To txtSample.Count - 1
    If txtSample(sam).SelStart <> SelStart Then txtSample(sam).SelStart = SelStart
    If txtSample(sam).SelLength <> SelLength Then txtSample(sam).SelLength = SelLength
  Next sam
  
  SelStart = SelStart + HScrollSample.Value
  If SelLength > 0 And Not SettingSelFromGrid Then
    If SelLength < 2 Then
      agdDataMapping.TextMatrix(agdDataMapping.row, ColMappingCol) = SelStart
    Else
      agdDataMapping.TextMatrix(agdDataMapping.row, ColMappingCol) = SelStart & "-" & SelStart + SelLength - 1
    End If
'    SetFixedWidthsFromDataMapping
  End If
End Sub

Private Sub txtDataFile_GotFocus()
  txtDataFile.SelStart = 0
  txtDataFile.SelLength = Len(txtDataFile.Text)
End Sub

Private Sub txtDataFile_Change()
  
  ' If a filename was not entered, exit the subroutine:
  ' If a filename not changed, then exit the subroutine:
  '
  If txtDataFile.Text = "" Then
    UnitDataFile = 0
    PopulateSample
  ElseIf txtDataFile.Text <> NameDataFile Or UnitDataFile = 0 Then
    OpenDataFile
  End If
  
End Sub

'Start with text in txtDataFile, try to open the file and set NameDataFile
Private Sub OpenDataFile()
  Dim n As Long ', msg As String
  ' Try to find a \ in the name. If so then assume the user
  ' entered a full pathname. If not, then append the cur directory
  ' to get a fullpathname.
  '
  n = InStr(1, txtDataFile.Text, "\", vbTextCompare)
  If n > 0 Then
    NameDataFile = Trim(txtDataFile.Text)
  Else
    NameDataFile = CurDir & "\" & Trim(txtDataFile.Text)
  End If
  txtDataFile.Text = NameDataFile
'
' Open the input file.
'
'  If UnitDataFile > 0 Then Close UnitDataFile
'  UnitDataFile = FreeFile
'  On Error GoTo FileNotOpened
'  Open NameDataFile For Input As UnitDataFile
  
  OpenUnitDataFile
'  Msg = ScriptOpenDataFile(NameDataFile)
'  If Msg <> "OK" Then
'    MsgBox Msg, vbOKOnly, "Data Import"
'  Else
'    PopulateSample
'  End If
'  Exit Sub
'
'FileNotOpened:
'  UnitDataFile = 0
End Sub

' Event     <><><><><><><><><><><><><><><><><><><><><><><><>
' Name:     cmdBrowseData_Click
' Purpose:  Responds to press of the "Browse" button
'           to obtain the input file name.
'
Private Sub cmdBrowseData_Click()
  Dim m_FileName As String

  If UnitDataFile > 0 Then Close UnitDataFile
  dlgOpenFile.Filter = "All Files (*.*)|*.*"
  dlgOpenFile.DefaultExt = ""
  dlgOpenFile.DialogTitle = "Open Data File"
  dlgOpenFile.ShowOpen
  NameDataFile = dlgOpenFile.Filename
  OpenUnitDataFile
End Sub
Private Sub OpenUnitDataFile()
  Dim Msg As String
  ScriptOpenDataFile NameDataFile
  Msg = ScriptOpenDataFile(NameDataFile)
  If Msg <> "OK" Then
    MsgBox Msg, vbOKOnly, "Data Import"
  Else
    UnitDataFile = FreeFile
    Open NameDataFile For Input As UnitDataFile
  
    txtDataFile.Text = NameDataFile
  
    PopulateSample
  End If
End Sub

' Function  - - - - - - - - - - - - - - - - - - - - - - - - -
' Name:     BoxWidth (1 argument)
' Purpose:  Returns an optimum width for user-prompt box
'           based on the length of the input text string
'
Private Function BoxWidth(TextStr As String) As Integer
  BoxWidth = Len(Trim(TextStr)) * 100
End Function

' Event     <><><><><><><><><><><><><><><><><><><><><><><><>
' Name:     txtScriptFile_Click
' Purpose:  Responds to click in "txtScriptFile" text box
'
Private Sub txtScriptFile_Click()
'
' Get default file name and check for its existance. If there
' ask user if the want to use it, otherwise prompt for a different
' file.
'
'  NameDescFile = Mid(Trim(NameDataFile), 1, Len(NameDataFile) - 4) & ".ddf"
'  FoundDDF = modFileIO.DoesFileExist(NameDescFile)
'
'  If FoundDDF = False Then
'    PromptForFileName (2)
'    Exit Sub
'  End If
'
' An existing ddf exists ask user "do you want to delete it ?"
'
'  Select Case MsgBox("Data-descriptor file exists:" & vbCrLf _
'              & NameDescFile & vbCrLf & "delete it?", vbYesNoCancel)
'    Case vbYes
'       modFileIO.DeleteFile NameDescFile
  '    txtScriptFile.Text = Dir(NameDescFile)
  '    modFileIO.OpenFile UnitDescFile, NameDescFile, "Input"
  '    If UnitDescFile > 0 Then
  '      txtScriptFile.Text = NameDescFile
  '      ReadDescFile
  '      txtLinesToSkip.SetFocus
  '    Else
  '      MsgBox "ERROR: CANNOT OPEN FILE:" & vbCrLf _
  '             & Chr(34) & NameDescFile & Chr(34)
  '      txtScriptFile.SetFocus
  '    End If

'    Case vbNo
'    Case vbCancel
'      txtScriptFile.Text = ""
'      txtLinesToSkip.SetFocus
'      Exit Sub
'    End Select
'
'    txtScriptFile.Text = ""
'    PromptForFileName (2)
'    txtScriptFile.SetFocus
'    Exit Sub
End Sub

' Event     <><><><><><><><><><><><><><><><><><><><><><><><>
' Name:     txtScriptFile_KeyPress (1 argument)
' Purpose:  Responds to press of any key in txtScriptFile text box
'
Private Sub txtScriptFile_KeyPress(KeyAscii As Integer)
  'ctlUserPrompt1.PromptLine2 "  TAB TO END"
  'ctlUserPrompt1.Line2Red
End Sub

' Event     <><><><><><><><><><><><><><><><><><><><><><><><>
' Name:     txtScriptFile_LostFocus
' Purpose:  Responds to loss of focus in txtScriptFile text box
'
Private Sub txtScriptFile_LostFocus()
  Dim m_FilePath As String
  Dim m_Prompt As String
  Dim m_LenPro As Integer
  Dim n As Long
  
  ' If a filename was not entered, reset found flags and
  ' exit the subroutine:
  '
  If Len(Trim(txtScriptFile.Text)) = 0 Then
'    FoundData = False
    FoundDDF = False
'    txtLinesToSkip.SetFocus
'    FoundPas = False
    Exit Sub
  End If

  ' Something was entered in the data-description file text box:
  '
  ' Try to find a \ in the name. If so then assume the user
  ' entered a full pathname. If not, then append the cur directory
  ' to get a fullpathname.
  '
  n = InStr(1, txtScriptFile.Text, "\", vbTextCompare)
  If n > 0 Then
    NameDescFile = Trim(txtScriptFile.Text)
  Else
    m_FilePath = CurDir
    NameDescFile = m_FilePath & "\" & Trim(txtScriptFile.Text)
  End If

'  FoundDDF = DoesFileExist(NameDescFile)
'
'  If FoundDDF = True Then
'    If UnitDescFile > 0 Then Close UnitDescFile
'    modFileIO.OpenFile UnitDescFile, NameDescFile, "Input"
'    If UnitDescFile > 0 Then
'        txtScriptFile.Text = NameDescFile
'        ReadDescFile
'    End If
'
'    PromptForDataMapping
'  Else
'    MsgBox "ERROR: FILE NOT FOUND:" & vbCrLf _
'           & Chr(34) & txtScriptFile.Text & Chr(34)
'    txtScriptFile.Text = ""
'    txtScriptFile.SetFocus
'
'  End If
End Sub

' Event     <><><><><><><><><><><><><><><><><><><><><><><><>
' Name:     txtDelimiter_Change
' Purpose:  Responds to any change in the "txtDelimiter" text box
'
Private Sub txtDelimiter_Change()
  optDelimiter(3).Value = True
  delim = txtDelimiter.Text
  If fraColSample.Visible Then PopulateGridSample
End Sub

' Event     <><><><><><><><><><><><><><><><><><><><><><><><>
' Name:     txtQuoteChar_KeyPress
' Purpose:  Responds to a key pressed in "txtQuoteChar" text box
'
'Private Sub txtQuoteChar_KeyPress(KeyAscii As Integer)
'  If KeyAscii = Asc("n") Or KeyAscii = Asc("N") Then
'    txtQuoteChar.Text = "none"
'  ElseIf KeyAscii > 32 And KeyAscii < 127 Then
'    txtQuoteChar.Text = Chr(KeyAscii)
'  Else
'    txtQuoteChar.Text = "none"
'  End If
'  KeyAscii = 0
'  If fraColSample.Visible Then PopulateGridSample
'End Sub

' Event     <><><><><><><><><><><><><><><><><><><><><><><><>
' Name:     txtNullChar_KeyPress
' Purpose:  Responds to a key pressed in "txtNullChar" text box
'
'Private Sub txtNullChar_KeyPress(KeyAscii As Integer)
'  If KeyAscii = Asc("n") Or KeyAscii = Asc("N") Then
'    txtNullChar.Text = "none"
'  ElseIf KeyAscii > 32 And KeyAscii < 127 Then
'    txtNullChar.Text = Chr(KeyAscii)
'  Else
'    txtNullChar.Text = "none"
'  End If
'  KeyAscii = 0
'  If fraColSample.Visible Then PopulateGridSample
'End Sub

' Event     <><><><><><><><><><><><><><><><><><><><><><><><>
' Name:     txtLinesToSkip_Change
' Purpose:  Responds to any change in the "txtLinesToSkip" text box
'
Private Sub txtLinesToSkip_Change()
  If fraColSample.Visible Then PopulateGridSample
End Sub




' Subroutine ===============================================
' Name:      AskForLookupFilename
' Purpose:   Queries user for name of lookup file
'
Private Sub AskForLookupFilename()
  dlgOpenFile.Filter = "All Files (*.*)|*.*"
  dlgOpenFile.DefaultExt = ""
  dlgOpenFile.DialogTitle = "Open lookup file"
  dlgOpenFile.ShowOpen
  With agdDataMapping
    .TextMatrix(.row, 0) = dlgOpenFile.Filename
    If Len(dlgOpenFile.Filename) > 0 _
     Then .TextMatrix(.row, 4) = ""
  End With
End Sub

' Subroutine ===============================================
' Name:      SetFixedWidthsFromDataMapping
' Purpose:   Sets widths of fields for fixed-format files
'
'Public Sub SetFixedWidthsFromDataMapping()
'  Dim r As Long
'  Dim colspec As String
'  Dim dashpos As Long
' 'Dim existing As Long
'
'  nFixedCols = 0
'  For r = 1 To agdDataMapping.MaxOccupiedRow
'    colspec = agdDataMapping.TextMatrix(r, ColMappingCol)
'    nFixedCols = nFixedCols + 1
'    If Len(colspec) > 0 Then
'      dashpos = InStr(colspec, "-")
'      If dashpos > 0 Then   'range (left-right)
'        FixedColLeft(nFixedCols) = Left(colspec, dashpos - 1)
'        FixedColRight(nFixedCols) = Mid(colspec, dashpos + 1)
'      Else
'        dashpos = InStr(colspec, "+")
'        If dashpos > 0 Then 'range (left+length)
'          FixedColLeft(nFixedCols) = Left(colspec, dashpos - 1)
'          FixedColRight(nFixedCols) = Mid(colspec, dashpos + 1) + _
'          FixedColLeft(nFixedCols)
'        Else                'Single number, assume single character column
'          FixedColLeft(nFixedCols) = colspec
'          FixedColRight(nFixedCols) = colspec
'        End If
'      End If
'    Else
'      FixedColLeft(nFixedCols) = 0
'      FixedColRight(nFixedCols) = 0
'    End If
'  Next r
'  'Debug.Print "SetFixedWidthsFromDataMapping: nFixedCols=" & nFixedCols
'End Sub

' Subroutine ===============================================
' Name:      WriteDescFile (1 argument)
' Purpose:   Saves the mapping information to a data-definition/
'            descriptor file
' Modified:
'
Private Sub WriteDescFile(UnitOutfile As Integer)
  Dim r As Long, index As Long
  Dim view As String
  Dim Field As String
  Dim tmp As String
  
  If UnitOutfile < 0 Then
    MsgBox "Cannot write to output file.", vbOKOnly, "Data Import"
  Else
    Print #UnitOutfile, "#National Water Information System"
  End If
End Sub

' Event     <><><><><><><><><><><><><><><><><><><><><><><><>
' Name:     objEditEngine_JobStatus
' Purpose:  Handles updating jobstatus and interupts.
'
' Notes:
' Whenever the JobStatus event is raised in the objEditEngine,
' this event procedure displays the jobs current status. The
' DoEvents statement allows the GUI to repaint, and
' also gives the user the opportunity to click the
' StopJob (StopJob) button.
'
'Private Sub objEditEngine_JobStatus( _
'            ByVal NumRecProcessed As Long, _
'            ByVal NumErrWarning As Long, _
'            ByVal NumErrFatal As Long, _
'            StopJob As Boolean)
'
' Display status in GUI ...
'
  'lblWarn.caption = "Warn: " & CStr(NumErrWarning)
  'lblFatal.caption = "Fatal: " & CStr(NumErrFatal)
  'lblRead.caption = "Read: " & CStr(NumRecProcessed + CLng(txtLinesToSkip.text))
'
' StopJob the job if the user has requested the process to StopJob.
' StopJob it by passing StopJob = true back to the objeditengine.
'
  'If m_StopJob Then
  '  StopJob = True
  'End If
'
' Allow the user to access to GUI to StopJob the processing.
'
  'DoEvents
'End Sub

' Subroutine ===============================================
' Name:      UpdateDataMap
' Purpose:   Updates the data-mapping grid (?)
'
Sub UpdateDataMap()
  Dim i As Integer
'
' Load views and columns if not already loaded.
'
'  If g_colView.Count = 0 Then
'    LoadViewsAndColumns
'  End If
'
' Clear out any existing data map in the engine.
'
  'objEditEngine.ClearDataMap
'
' Pass in the current data map from the GUI.
'
' Note: in real batch processing mode the data map information
' could be read in directly from a ddf file.
'

  For i = 1 To agdDataMapping.rows
     'Call objEditEngine.AddDataMapEntry( _
     '     agdDataMapping.TextMatrix(i, 5), _
     '     agdDataMapping.TextMatrix(i, 2), _
     '     agdDataMapping.TextMatrix(i, 3), _
     '     agdDataMapping.TextMatrix(i, 4), _
     '     agdDataMapping.TextMatrix(i, 0), _
     '     g_colVWCol.item(Trim(agdDataMapping.TextMatrix(i, 5)) & _
     '      "_" & Trim(agdDataMapping.TextMatrix(i, 2))).EditCriteria, _
     '     g_colVWCol.item(Trim(agdDataMapping.TextMatrix(i, 5)) & _
     '      "_" & Trim(agdDataMapping.TextMatrix(i, 2))))
  Next
End Sub

Private Sub VScrollSample_Change()
  PopulateTxtSample
End Sub

Private Sub VScrollSample_Scroll()
  VScrollSample_Change
End Sub
