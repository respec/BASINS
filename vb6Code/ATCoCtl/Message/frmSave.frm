VERSION 5.00
Object = "{831FDD16-0C5C-11D2-A9FC-0000F8754DA1}#2.0#0"; "MSCOMCTL.OCX"
Begin VB.Form frmSave 
   Caption         =   "Save"
   ClientHeight    =   8808
   ClientLeft      =   48
   ClientTop       =   276
   ClientWidth     =   11304
   LinkTopic       =   "Form1"
   ScaleHeight     =   8808
   ScaleWidth      =   11304
   StartUpPosition =   3  'Windows Default
   Begin VB.Frame fraSave 
      BorderStyle     =   0  'None
      Caption         =   "Frame1"
      Height          =   4212
      Index           =   1
      Left            =   5280
      TabIndex        =   12
      Top             =   4560
      Width           =   3132
      Begin VB.CheckBox chkCurve 
         Caption         =   "Curve Properties (style, color, label)"
         Height          =   255
         Left            =   240
         TabIndex        =   23
         Top             =   2160
         Value           =   1  'Checked
         Width           =   3132
      End
      Begin VB.CheckBox chkLegLoc 
         Caption         =   "Legend Location"
         Height          =   255
         Left            =   240
         TabIndex        =   22
         Top             =   2880
         Value           =   1  'Checked
         Width           =   3135
      End
      Begin VB.CheckBox chkMainTitle 
         Caption         =   "Main Title and Window Caption"
         Height          =   255
         Left            =   240
         TabIndex        =   21
         Top             =   3600
         Value           =   1  'Checked
         Width           =   3135
      End
      Begin VB.Frame fraGraphAxis 
         Caption         =   "Axis Properites"
         Height          =   1935
         Left            =   0
         TabIndex        =   16
         Top             =   0
         Width           =   3132
         Begin VB.CheckBox chkTitles 
            Caption         =   "Titles"
            Height          =   255
            Left            =   240
            TabIndex        =   20
            Top             =   360
            Value           =   1  'Checked
            Width           =   2535
         End
         Begin VB.CheckBox chkTypes 
            Caption         =   "Types (arithmetic, log)"
            Enabled         =   0   'False
            Height          =   255
            Left            =   240
            TabIndex        =   19
            Top             =   720
            Width           =   2535
         End
         Begin VB.CheckBox chkScales 
            Caption         =   "Scales and Tics"
            Height          =   255
            Left            =   240
            TabIndex        =   18
            Top             =   1080
            Value           =   1  'Checked
            Width           =   2655
         End
         Begin VB.CheckBox chkGrid 
            Caption         =   "Grid"
            Height          =   255
            Left            =   240
            TabIndex        =   17
            Top             =   1440
            Value           =   1  'Checked
            Width           =   2535
         End
      End
      Begin VB.CheckBox chkAddTxt 
         Caption         =   "Additional Text"
         Height          =   255
         Left            =   240
         TabIndex        =   15
         Top             =   3240
         Value           =   1  'Checked
         Width           =   3135
      End
      Begin VB.CheckBox chkDataLabels 
         Caption         =   "Data Labels"
         Height          =   255
         Left            =   240
         TabIndex        =   14
         Top             =   3960
         Value           =   1  'Checked
         Width           =   3135
      End
      Begin VB.CheckBox chkLine 
         Caption         =   "Formula Lines"
         Height          =   255
         Left            =   240
         TabIndex        =   13
         Top             =   2520
         Value           =   1  'Checked
         Width           =   3132
      End
   End
   Begin VB.Frame fraSave 
      BorderStyle     =   0  'None
      Height          =   2532
      Index           =   0
      Left            =   8400
      TabIndex        =   3
      Top             =   4680
      Width           =   2652
      Begin VB.TextBox txtHeader 
         Height          =   765
         Left            =   240
         MultiLine       =   -1  'True
         ScrollBars      =   3  'Both
         TabIndex        =   9
         Text            =   "frmSave.frx":0000
         ToolTipText     =   "Name of file containing data to import"
         Top             =   360
         Width           =   3495
      End
      Begin VB.OptionButton optDelimiter 
         Caption         =   "Fi&xed width space padded"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   7.8
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   255
         Index           =   3
         Left            =   240
         TabIndex        =   8
         Top             =   2160
         Width           =   2655
      End
      Begin VB.TextBox txtDelimiter 
         Height          =   285
         Left            =   2400
         TabIndex        =   7
         Text            =   ","
         ToolTipText     =   "Single printable character delimiter"
         Top             =   1872
         Width           =   255
      End
      Begin VB.OptionButton optDelimiter 
         Caption         =   "S&pace delimited"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   7.8
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   255
         Index           =   1
         Left            =   240
         TabIndex        =   6
         Top             =   1680
         Width           =   2055
      End
      Begin VB.OptionButton optDelimiter 
         Caption         =   "T&ab delimited"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   7.8
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   255
         Index           =   0
         Left            =   240
         TabIndex        =   5
         Top             =   1440
         Value           =   -1  'True
         Width           =   2175
      End
      Begin VB.OptionButton optDelimiter 
         Caption         =   "&Character delimited:"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   7.8
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   255
         Index           =   2
         Left            =   240
         TabIndex        =   4
         Top             =   1920
         Width           =   2175
      End
      Begin VB.Label lblFileType 
         Caption         =   "Column &Format:"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   7.8
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   252
         Left            =   0
         TabIndex        =   11
         Top             =   1200
         Width           =   1452
      End
      Begin VB.Label lblHeaderRecord 
         Caption         =   "&Header:"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   7.8
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   255
         Left            =   0
         TabIndex        =   10
         Top             =   45
         Width           =   1455
      End
   End
   Begin VB.CommandButton cmdSave 
      Caption         =   "&Save"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   7.8
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   375
      Left            =   3600
      TabIndex        =   2
      Top             =   3120
      Width           =   975
   End
   Begin VB.CommandButton cmdCancel 
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
      Height          =   375
      Left            =   4800
      TabIndex        =   1
      Top             =   3120
      Width           =   975
   End
   Begin MSComctlLib.TabStrip TabStrip1 
      Height          =   4692
      Left            =   120
      TabIndex        =   0
      Top             =   0
      Width           =   3372
      _ExtentX        =   5948
      _ExtentY        =   8276
      _Version        =   393216
      BeginProperty Tabs {1EFB6598-857C-11D1-B16A-00C0F0283628} 
         NumTabs         =   2
         BeginProperty Tab1 {1EFB659A-857C-11D1-B16A-00C0F0283628} 
            ImageVarType    =   2
         EndProperty
         BeginProperty Tab2 {1EFB659A-857C-11D1-B16A-00C0F0283628} 
            ImageVarType    =   2
         EndProperty
      EndProperty
   End
End
Attribute VB_Name = "frmSave"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit

Private Sub Form_Load()

End Sub
