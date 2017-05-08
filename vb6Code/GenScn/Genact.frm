VERSION 5.00
Begin VB.Form frmGenScnActivate 
   Caption         =   "GenScn Activate"
   ClientHeight    =   5565
   ClientLeft      =   270
   ClientTop       =   1335
   ClientWidth     =   8280
   HelpContextID   =   53
   Icon            =   "Genact.frx":0000
   LinkTopic       =   "Form1"
   PaletteMode     =   1  'UseZOrder
   ScaleHeight     =   5565
   ScaleWidth      =   8280
   WhatsThisButton =   -1  'True
   WhatsThisHelp   =   -1  'True
   Begin VB.ListBox lstFiles 
      Height          =   645
      Left            =   1200
      TabIndex        =   7
      Top             =   1920
      Width           =   1695
   End
   Begin VB.CommandButton cmdTable 
      Caption         =   "Delete"
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
      Left            =   5640
      TabIndex        =   19
      Top             =   3480
      Visible         =   0   'False
      Width           =   732
   End
   Begin VB.CommandButton cmdTable 
      Caption         =   "Insert"
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
      Left            =   4800
      TabIndex        =   18
      Top             =   3480
      Visible         =   0   'False
      Width           =   735
   End
   Begin VB.CommandButton cmdTable 
      Caption         =   "Dbg"
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
      Left            =   7560
      TabIndex        =   21
      Top             =   3480
      Visible         =   0   'False
      Width           =   492
   End
   Begin VB.CommandButton cmdTable 
      Caption         =   "Text/Grid"
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
      Left            =   6480
      TabIndex        =   20
      Top             =   3480
      Visible         =   0   'False
      Width           =   972
   End
   Begin VB.PictureBox picGlobal 
      BorderStyle     =   0  'None
      Height          =   1335
      Left            =   240
      ScaleHeight     =   1335
      ScaleWidth      =   7935
      TabIndex        =   43
      Top             =   3840
      Visible         =   0   'False
      Width           =   7935
      Begin VB.TextBox txtStart 
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
         Left            =   720
         TabIndex        =   24
         Text            =   "????"
         Top             =   360
         WhatsThisHelpID =   25
         Width           =   612
      End
      Begin VB.TextBox txtEnd 
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
         Left            =   720
         TabIndex        =   29
         Text            =   "????"
         Top             =   720
         WhatsThisHelpID =   26
         Width           =   612
      End
      Begin VB.ComboBox comboUnits 
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
         Left            =   6840
         Style           =   2  'Dropdown List
         TabIndex        =   37
         Top             =   720
         WhatsThisHelpID =   30
         Width           =   975
      End
      Begin VB.ComboBox comboOutput 
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
         Left            =   4920
         Style           =   2  'Dropdown List
         TabIndex        =   34
         Top             =   360
         WhatsThisHelpID =   27
         Width           =   852
      End
      Begin VB.ComboBox comboSpecial 
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
         Left            =   4920
         Style           =   2  'Dropdown List
         TabIndex        =   35
         Top             =   720
         WhatsThisHelpID =   28
         Width           =   852
      End
      Begin VB.ComboBox comboRunflag 
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
         Left            =   6840
         Style           =   2  'Dropdown List
         TabIndex        =   36
         Top             =   360
         WhatsThisHelpID =   29
         Width           =   972
      End
      Begin VB.TextBox txtSmon 
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
         Left            =   1440
         TabIndex        =   25
         Text            =   "??"
         Top             =   360
         Width           =   492
      End
      Begin VB.TextBox txtEmon 
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
         Left            =   1440
         TabIndex        =   30
         Text            =   "??"
         Top             =   720
         Width           =   492
      End
      Begin VB.TextBox txtSday 
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
         Left            =   2040
         TabIndex        =   26
         Text            =   "??"
         Top             =   360
         Width           =   492
      End
      Begin VB.TextBox txtEday 
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
         Left            =   2040
         TabIndex        =   31
         Text            =   "??"
         Top             =   720
         Width           =   492
      End
      Begin VB.TextBox txtShour 
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
         Left            =   2640
         TabIndex        =   27
         Text            =   "??"
         Top             =   360
         Width           =   492
      End
      Begin VB.TextBox txtEhour 
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
         Left            =   2640
         TabIndex        =   32
         Text            =   "??"
         Top             =   720
         Width           =   492
      End
      Begin VB.TextBox txtSmin 
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
         Left            =   3240
         TabIndex        =   28
         Text            =   "??"
         Top             =   360
         Width           =   492
      End
      Begin VB.TextBox txtEmin 
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
         Left            =   3240
         TabIndex        =   33
         Text            =   "??"
         Top             =   720
         Width           =   492
      End
      Begin VB.Label Label1 
         Alignment       =   1  'Right Justify
         Caption         =   "Start:"
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
         TabIndex        =   56
         Top             =   360
         WhatsThisHelpID =   25
         Width           =   492
      End
      Begin VB.Label Label2 
         Alignment       =   1  'Right Justify
         Caption         =   "End:"
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
         TabIndex        =   55
         Top             =   720
         WhatsThisHelpID =   26
         Width           =   492
      End
      Begin VB.Label Label3 
         Alignment       =   1  'Right Justify
         Caption         =   "General:"
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
         Left            =   3840
         TabIndex        =   54
         Top             =   360
         WhatsThisHelpID =   27
         Width           =   972
      End
      Begin VB.Label Label4 
         Alignment       =   1  'Right Justify
         Caption         =   "Special Actions:"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   492
         Left            =   3960
         TabIndex        =   53
         Top             =   720
         WhatsThisHelpID =   28
         Width           =   852
      End
      Begin VB.Label Label5 
         Alignment       =   1  'Right Justify
         Caption         =   "Run Flag:"
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
         Left            =   5880
         TabIndex        =   52
         Top             =   360
         WhatsThisHelpID =   29
         Width           =   852
      End
      Begin VB.Label Label6 
         Alignment       =   1  'Right Justify
         Caption         =   "Units:"
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
         Left            =   6240
         TabIndex        =   51
         Top             =   720
         WhatsThisHelpID =   30
         Width           =   492
      End
      Begin VB.Label Label7 
         Alignment       =   2  'Center
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
         Height          =   252
         Left            =   3840
         TabIndex        =   50
         Top             =   0
         Width           =   1212
      End
      Begin VB.Label Label8 
         Caption         =   "Span"
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
         TabIndex        =   49
         Top             =   0
         Width           =   612
      End
      Begin VB.Label Label9 
         Alignment       =   2  'Center
         Caption         =   "Year"
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
         Left            =   720
         TabIndex        =   48
         Top             =   120
         Width           =   612
      End
      Begin VB.Label Label10 
         Alignment       =   2  'Center
         Caption         =   "Mo"
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
         Left            =   1440
         TabIndex        =   47
         Top             =   120
         Width           =   492
      End
      Begin VB.Label Label11 
         Alignment       =   2  'Center
         Caption         =   "Day"
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
         TabIndex        =   46
         Top             =   120
         Width           =   492
      End
      Begin VB.Label Label12 
         Alignment       =   2  'Center
         Caption         =   "Hr"
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
         Left            =   2640
         TabIndex        =   45
         Top             =   120
         Width           =   492
      End
      Begin VB.Label Label13 
         Alignment       =   2  'Center
         Caption         =   "Min"
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
         Left            =   3240
         TabIndex        =   44
         Top             =   120
         Width           =   492
      End
   End
   Begin VB.TextBox txtPath 
      BackColor       =   &H80000004&
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
      Left            =   1080
      Locked          =   -1  'True
      TabIndex        =   0
      Text            =   "????"
      ToolTipText     =   "Edit Path in GenScn:Scenarios:Properties"
      Top             =   120
      WhatsThisHelpID =   24
      Width           =   6972
   End
   Begin VB.CommandButton cmdOpt 
      Caption         =   "&Modify"
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
      Index           =   5
      Left            =   1680
      TabIndex        =   3
      Top             =   960
      WhatsThisHelpID =   34
      Width           =   1212
   End
   Begin VB.CommandButton cmdOpt 
      Caption         =   "&View"
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
      Index           =   4
      Left            =   240
      TabIndex        =   6
      Top             =   2040
      WhatsThisHelpID =   33
      Width           =   855
   End
   Begin VB.CommandButton cmdTable 
      Caption         =   "Help"
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
      Left            =   3960
      TabIndex        =   17
      Top             =   3480
      Visible         =   0   'False
      Width           =   732
   End
   Begin VB.CommandButton cmdOpt 
      Caption         =   "&Close"
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
      Index           =   3
      Left            =   960
      TabIndex        =   8
      Top             =   2760
      WhatsThisHelpID =   32
      Width           =   1212
   End
   Begin VB.CommandButton cmdTable 
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
      Height          =   252
      Index           =   1
      Left            =   3000
      TabIndex        =   16
      Top             =   3480
      Visible         =   0   'False
      Width           =   855
   End
   Begin VB.CommandButton cmdTable 
      Caption         =   "OK"
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
      Left            =   2280
      TabIndex        =   15
      Top             =   3480
      Visible         =   0   'False
      Width           =   612
   End
   Begin VB.Frame fraTname 
      Caption         =   "Table"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   2295
      Left            =   5760
      TabIndex        =   40
      Top             =   840
      Visible         =   0   'False
      Width           =   2292
      Begin VB.OptionButton optOpnTname 
         Caption         =   "Active"
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
         TabIndex        =   14
         Top             =   1920
         Value           =   -1  'True
         WhatsThisHelpID =   40
         Width           =   975
      End
      Begin VB.OptionButton optOpnTname 
         Caption         =   "All"
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
         Left            =   240
         TabIndex        =   13
         Top             =   1920
         WhatsThisHelpID =   40
         Width           =   735
      End
      Begin VB.ListBox lstTname 
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   1035
         Left            =   120
         TabIndex        =   12
         Top             =   360
         WhatsThisHelpID =   39
         Width           =   2052
      End
   End
   Begin VB.Frame Frame1 
      Caption         =   "Block"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   2295
      Left            =   3240
      TabIndex        =   39
      Top             =   840
      Width           =   2292
      Begin VB.OptionButton optOpnKwd 
         Caption         =   "Active"
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
         TabIndex        =   11
         Top             =   1920
         Value           =   -1  'True
         WhatsThisHelpID =   38
         Width           =   975
      End
      Begin VB.OptionButton optOpnKwd 
         Caption         =   "All"
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
         TabIndex        =   10
         Top             =   1920
         WhatsThisHelpID =   38
         Width           =   615
      End
      Begin VB.ListBox lstKwd 
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   1035
         ItemData        =   "Genact.frx":0442
         Left            =   120
         List            =   "Genact.frx":0444
         TabIndex        =   9
         Top             =   360
         WhatsThisHelpID =   37
         Width           =   2055
      End
   End
   Begin VB.CommandButton cmdOpt 
      Caption         =   "Save &As"
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
      Left            =   1680
      TabIndex        =   5
      Top             =   1440
      WhatsThisHelpID =   35
      Width           =   1212
   End
   Begin VB.TextBox txtInfo 
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
      Left            =   1080
      TabIndex        =   1
      Text            =   "????"
      Top             =   480
      WhatsThisHelpID =   24
      Width           =   6972
   End
   Begin VB.CommandButton cmdOpt 
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
      Height          =   372
      Index           =   1
      Left            =   240
      TabIndex        =   4
      Top             =   1440
      WhatsThisHelpID =   34
      Width           =   1212
   End
   Begin VB.CommandButton cmdOpt 
      Caption         =   "Simulate(&R)"
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
      Left            =   240
      TabIndex        =   2
      Top             =   960
      WhatsThisHelpID =   31
      Width           =   1212
   End
   Begin VB.TextBox txtRec 
      BeginProperty Font 
         Name            =   "Courier New"
         Size            =   7.5
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   1455
      Left            =   240
      MultiLine       =   -1  'True
      ScrollBars      =   2  'Vertical
      TabIndex        =   23
      Text            =   "Genact.frx":0446
      Top             =   3840
      Visible         =   0   'False
      Width           =   7800
   End
   Begin ATCoCtl.ATCoGrid gridctrl1 
      Height          =   1335
      Left            =   120
      TabIndex        =   22
      Top             =   3840
      Width           =   7935
      _ExtentX        =   13996
      _ExtentY        =   2355
      SelectionToggle =   -1  'True
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
      Header          =   "lblHeader"
      FixedRows       =   1
      FixedCols       =   0
      ScrollBars      =   3
      SelectionMode   =   0
      BackColor       =   -2147483643
      ForeColor       =   -2147483640
      BackColorBkg    =   -2147483633
      BackColorSel    =   -2147483635
      ForeColorSel    =   -2147483634
      BackColorFixed  =   -2147483633
      ForeColorFixed  =   -2147483630
      InsideLimitsBackground=   16777215
      OutsideHardLimitBackground=   8421631
      OutsideSoftLimitBackground=   8454143
      ComboCheckValidValues=   -1  'True
   End
   Begin VB.Label lblPath 
      Alignment       =   1  'Right Justify
      Caption         =   "Path:"
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
      TabIndex        =   42
      Top             =   120
      WhatsThisHelpID =   24
      Width           =   852
   End
   Begin VB.Label lblBlock 
      Caption         =   "????"
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
      TabIndex        =   41
      Top             =   3480
      Visible         =   0   'False
      Width           =   1935
   End
   Begin VB.Label lblScen 
      Alignment       =   1  'Right Justify
      Caption         =   "Run Info:"
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
      TabIndex        =   38
      Top             =   480
      WhatsThisHelpID =   24
      Width           =   852
   End
End
Attribute VB_Name = "frmGenScnActivate"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
'Copyright 2000 by AQUA TERRA Consultants

Private Type HSPFOper
   Name As String
   Exists As Boolean
   OpF As Long
   OpL As Long
   cnt As Long
End Type
Dim oper(11) As HSPFOper

Dim S&(5), e&(5), ou&, sp&, ru&, em&, inf$
Dim i&, k$
Dim akey&(), nkey&
Dim oldsyear$, oldsmonth$, oldsday$, oldshour$, oldsminute$
Dim oldeyear$, oldemonth$, oldeday$, oldehour$, oldeminute$

Dim ousav&, spsav&, rusav&, emsav&
Dim sysav$, eysav$, smosav$, emosav$, sdsav$, edsav$
Dim shsav$, ehsav$, smisav$, emisav$

'Retrieved in a call to F90_XTINFO to get column info for grid
Dim lnflds&, lscol&(30), lflen&(30), lftyp$, lapos&(30)
Dim limin&(30), limax&(30), lidef&(30)
Dim lrmin!(30), lrmax!(30), lrdef!(30)
Dim lnmhdr&, hdrbuf$(10), lfdnam$(30)
Dim lScenFile$

Public newName$, RelAbs&, BaseDSN&

Public Function OperationExists(Name$) As Boolean

   OperationExists = False
   For i = 0 To lstKwd.ListCount
     If lstKwd.List(i) = Name Then
       OperationExists = True
       Exit For
     End If
   Next i
   
End Function

Private Sub Edit_Global()
  txtRec.Visible = False
  lblBlock.Visible = True
  gridctrl1.Visible = False
  cmdTable(0).Visible = True
  cmdTable(1).Visible = True
  cmdTable(2).Visible = True
  picGlobal.Visible = True
  lblBlock.Caption = lstKwd.List(lstKwd.ListIndex)
  frmGenScnActivate.Height = picGlobal.Height + picGlobal.Top + 250
  For i = 0 To 5
    cmdOpt(i).Enabled = False
  Next i
  lstTname.Enabled = False
  lstKwd.Enabled = False
  optOpnKwd(0).Enabled = False
  optOpnKwd(1).Enabled = False
  optOpnTname(0).Enabled = False
  optOpnTname(1).Enabled = False
  'save values we came in with
  ousav = comboOutput.ListIndex
  spsav = comboSpecial.ListIndex
  rusav = comboRunflag.ListIndex
  emsav = comboUnits.ListIndex
  sysav = txtStart.text
  eysav = txtEnd.text
  smosav = txtSmon.text
  emosav = txtEmon.text
  sdsav = txtSday.text
  edsav = txtEday.text
  shsav = txtShour.text
  ehsav = txtEhour.text
  smisav = txtSmin.text
  emisav = txtEmin.text
End Sub

Private Sub Edit_ExtSources(g As Control, etype%)
  'etype - 1:text change, 2:RowCol change
  Dim S$, dsn&, i&, wdmindex&
  Static TarGrpInd&, TarOperTyp&, LastRow&
    
  If g.row <> LastRow Then
    'update info on current state of this row
    TarOperTyp = GetOperType(g.TextMatrix(g.row, 8))
    TarGrpInd = GetGrpInd(g.TextMatrix(g.row, 11), TarOperTyp)
    LastRow = g.row
  End If
  
  If etype = 1 Then
    If g.col = 1 Then
      S = g.text
      If IsNumeric(S) Then
        dsn = CLng(S)
        If Len(g.TextMatrix(g.row, 0)) > 3 And _
           IsNumeric(Mid(g.TextMatrix(g.row, 0), 4, 1)) Then
          wdmindex = CInt(Mid(g.TextMatrix(g.row, 0), 4, 1))
        Else
          wdmindex = 1
        End If
        For i = 1 To p.WDMFiles(wdmindex).DataCount
          If p.WDMFiles(wdmindex).Data(i).Header.id = dsn Then
            S = p.WDMFiles(wdmindex).Data(i).Attrib("TSTYPE")
            Exit For
          End If
        Next i
        If Len(S) > 0 Then
          g.TextBackColor = vbWhite
        Else
          g.TextBackColor = vbRed
        End If
      Else
        g.TextBackColor = vbRed
        S = ""
      End If
      g.TextMatrix(g.row, 2) = S
    ElseIf g.col = 8 Then
      TarOperTyp = GetOperType(g.text)
    ElseIf g.col = 11 Then
      TarGrpInd = g.ListIndex + 1
    End If
  ElseIf etype = 2 Then
    g.ClearValues
    If g.col = 0 Then 'svol:wdm and what else?
      g.addValue "WDM"
      For i = 1 To p.WDMFiles.Count
        g.addValue "WDM" & CStr(i)
      Next i
    ElseIf g.col = 1 Then 'svolno
    ElseIf g.col = 2 Then 'smemn
      If IsNumeric(g.TextMatrix(g.row, 1)) Then
        dsn = g.TextMatrix(g.row, 1)
        If Len(g.TextMatrix(g.row, 0)) > 3 And _
           IsNumeric(Mid(g.TextMatrix(g.row, 0), 4, 1)) Then
          wdmindex = CInt(Mid(g.TextMatrix(g.row, 0), 4, 1))
        Else
          wdmindex = 1
        End If
        For i = 1 To p.WDMFiles(wdmindex).DataCount
          If p.WDMFiles(wdmindex).Data(i).Header.id = dsn Then
            S = p.WDMFiles(wdmindex).Data(i).Attrib("TSTYPE")
            Exit For
          End If
        Next i
        g.addValue S
      End If
    ElseIf g.col = 3 Then 'qflg
    ElseIf g.col = 4 Then 'ssyst
      Call SetTableKwds(g, -(g.col + 1))
    ElseIf g.col = 5 Then 'sgapst
      g.addValue " " 'allow default blank
      Call SetTableKwds(g, -(g.col + 1))
    ElseIf g.col = 6 Then 'mfactr
    ElseIf g.col = 7 Then 'tran
      g.addValue " " 'allow default blank
      Call SetTableKwds(g, -(g.col + 1))
    ElseIf g.col = 8 Then 'tvol
      Call SetActiveOpn(g)
    ElseIf g.col = 9 Then 'topfst
      g.SetColRange g.col, (oper(TarOperTyp).OpF), (oper(TarOperTyp).OpL)
    ElseIf g.col = 10 Then 'toplst
      g.SetColRange g.col, (oper(TarOperTyp).OpF), (oper(TarOperTyp).OpL)
    ElseIf g.col = 11 Then 'tmemnm
      Call SetTableKwds(g, CLng(TarOperTyp + 140))
    ElseIf g.col = 12 Then 'tgrpn
      i = (TarOperTyp + 140) * 1000 + TarGrpInd + 1
      Call SetTableKwds(g, i)
    ElseIf g.col = 13 Then 'tmems1
    ElseIf g.col = 14 Then 'tmems2
    End If
  End If
End Sub
Private Sub Edit_ExtTargets(g As Control, etype%)
  'etype - 1:text change, 2:RowCol change
  Dim S$, dsn&, i&, wdmindex&
  Static SouGrpInd&, SouOperTyp&, LastRow&
    
  If g.row <> LastRow Then
    'update info on current state of this row
    SouOperTyp = GetOperType(g.TextMatrix(g.row, 0))
    SouGrpInd = GetGrpInd(g.TextMatrix(g.row, 2), SouOperTyp)
    LastRow = g.row
  End If
  
  If etype = 1 Then 'text change
    If g.col = 0 Then 'svol
      SouOperTyp = GetOperType(g.text)
    ElseIf g.col = 2 Then 'sgrpn
      SouGrpInd = g.ListIndex + 1
    ElseIf g.col = 9 Then
      S = g.text
      If IsNumeric(S) Then
        dsn = CLng(S)
        If Len(g.TextMatrix(g.row, g.col - 1)) > 3 And _
          IsNumeric(Mid(g.TextMatrix(g.row, g.col - 1), 4, 1)) Then
          wdmindex = CInt(Mid(g.TextMatrix(g.row, g.col - 1), 4, 1))
        Else
          wdmindex = 1
        End If
        For i = 1 To p.WDMFiles(wdmindex).DataCount
          If p.WDMFiles(wdmindex).Data(i).Header.id = dsn Then
            S = p.WDMFiles(wdmindex).Data(i).Attrib("TSTYPE")
            Exit For
          End If
        Next i
        If Len(S) > 0 Then
          g.TextBackColor = vbWhite
        Else
          g.TextBackColor = vbRed
        End If
      Else
        g.TextBackColor = vbRed
        S = ""
      End If
      g.TextMatrix(g.row, 10) = S
    End If
  Else 'RowCol change
    g.ClearValues
    If g.col = 0 Then 'svol
      g.SetColRange g.col, (oper(SouOperTyp).OpF), (oper(SouOperTyp).OpL)
    ElseIf g.col = 2 Then 'sgrpn
      Call SetTableKwds(g, CLng(SouOperTyp + 140))
    ElseIf g.col = 3 Then 'smemn
      i = (SouOperTyp + 140) * 1000 + SouGrpInd + 1
      Call SetTableKwds(g, i)
    ElseIf g.col = 4 Then 'smems1
    ElseIf g.col = 5 Then 'smems2
    ElseIf g.col = 6 Then 'mfactr
    ElseIf g.col = 7 Then 'tran
      g.addValue " " 'allow default blank
      Call SetTableKwds(g, -(g.col + 1))
    ElseIf g.col = 8 Then 'tvol:wdm and what else
      g.addValue "WDM"
      For i = 1 To p.WDMFiles.Count
        g.addValue "WDM" & CStr(i)
      Next i
    ElseIf g.col = 9 Then 'tvolno
    ElseIf g.col = 10 Then 'tmemn
      If IsNumeric(g.TextMatrix(g.row, 9)) Then
        dsn = g.TextMatrix(g.row, 9)
        If Len(g.TextMatrix(g.row, 8)) > 3 And _
          IsNumeric(Mid(g.TextMatrix(g.row, 8), 4, 1)) Then
          wdmindex = CInt(Mid(g.TextMatrix(g.row, 8), 4, 1))
        Else
          wdmindex = 1
        End If
        For i = 1 To p.WDMFiles(wdmindex).DataCount
          If p.WDMFiles(wdmindex).Data(i).Header.id = dsn Then
            S = p.WDMFiles(wdmindex).Data(i).Attrib("TSTYPE")
            Exit For
          End If
        Next i
        g.addValue S
      End If
    ElseIf g.col = 11 Then 'qflg
    ElseIf g.col = 12 Then 'tsyst
      g.addValue " " 'allow default blank
      Call SetTableKwds(g, -(g.col + 1))
    ElseIf g.col = 13 Then 'aggst
      g.addValue " " 'allow default blank
      Call SetTableKwds(g, -(g.col + 1))
    ElseIf g.col = 14 Then 'amdst
      g.addValue " " 'allow default blank
      Call SetTableKwds(g, -(g.col + 1))
    End If
  End If
End Sub
Private Sub Edit_Network(g As Control, etype%)
  'etype - 1:text change, 2:RowCol change
  Static SouGrpInd&, SouOperTyp&, TarGrpInd&, TarOperTyp&, LastRow&
    
  If g.row <> LastRow Then
    'update info on current state of this row
    SouOperTyp = GetOperType(g.TextMatrix(g.row, 0))
    SouGrpInd = GetGrpInd(g.TextMatrix(g.row, 2), SouOperTyp)
    TarOperTyp = GetOperType(g.TextMatrix(g.row, 8))
    TarGrpInd = GetGrpInd(g.TextMatrix(g.row, 11), TarOperTyp)
    LastRow = g.row
  End If
  
  If etype = 1 Then 'text change
    If g.col = 0 Then 'svol
      SouOperTyp = GetOperType(g.text)
    ElseIf g.col = 2 Then 'sgrpn
      SouGrpInd = g.ListIndex + 1
    ElseIf g.col = 8 Then
      TarOperTyp = GetOperType(g.text)
    ElseIf g.col = 11 Then
      TarGrpInd = g.ListIndex + 1
    End If
  Else 'RowCol change
    g.ClearValues
    If g.col = 0 Then 'svol
      Call SetActiveOpn(g)
    ElseIf g.col = 1 Then 'svolno
      g.SetColRange g.col, (oper(SouOperTyp).OpF), (oper(SouOperTyp).OpL)
    ElseIf g.col = 2 Then 'sgrpn
      Call SetTableKwds(g, CLng(SouOperTyp + 140))
    ElseIf g.col = 3 Then 'smemn
      i = (SouOperTyp + 140) * 1000 + SouGrpInd + 1
      Call SetTableKwds(g, i)
    ElseIf g.col = 4 Then 'smems1
    ElseIf g.col = 5 Then 'smems2
    ElseIf g.col = 6 Then 'mfactr
    ElseIf g.col = 7 Then 'tran
      g.addValue " " 'allow default blank
      Call SetTableKwds(g, -(g.col + 1))
    ElseIf g.col = 8 Then 'tvol
      Call SetActiveOpn(g)
    ElseIf g.col = 9 Then 'topfst
      g.SetColRange g.col, (oper(TarOperTyp).OpF), (oper(TarOperTyp).OpL)
    ElseIf g.col = 10 Then 'toplst
      g.SetColRange g.col, (oper(TarOperTyp).OpF), (oper(TarOperTyp).OpL)
    ElseIf g.col = 11 Then 'tmemnm
      Call SetTableKwds(g, CLng(TarOperTyp + 140))
    ElseIf g.col = 12 Then 'tgrpn
      i = (TarOperTyp + 140) * 1000 + TarGrpInd + 1
      Call SetTableKwds(g, i)
    ElseIf g.col = 13 Then 'tmems1
    ElseIf g.col = 14 Then 'tmems2
    End If
  End If
End Sub
Private Sub Edit_MassLink(g As Control, etype%)
  'etype - 1:text change, 2:RowCol change
  Static SouGrpInd&, SouOperTyp&, TarGrpInd&, TarOperTyp&, LastRow&
    
  If g.row <> LastRow Then
    'update info on current state of this row
    SouOperTyp = GetOperType(g.TextMatrix(g.row, 0))
    SouGrpInd = GetGrpInd(g.TextMatrix(g.row, 1), SouOperTyp)
    TarOperTyp = GetOperType(g.TextMatrix(g.row, 6))
    TarGrpInd = GetGrpInd(g.TextMatrix(g.row, 7), TarOperTyp)
    LastRow = g.row
  End If
  
  If etype = 1 Then 'text change
    If g.col = 0 Then 'svol
      SouOperTyp = GetOperType(g.text)
    ElseIf g.col = 1 Then 'sgrpn
      SouGrpInd = g.ListIndex + 1
    ElseIf g.col = 6 Then 'tvol
      TarOperTyp = GetOperType(g.text)
    ElseIf g.col = 7 Then 'tgrpn
      TarGrpInd = g.ListIndex + 1
    End If
  Else 'RowCol change
    g.ClearValues
    If g.col = 0 Then 'svol
      Call SetActiveOpn(g)
    ElseIf g.col = 1 Then 'sgrpn
      Call SetTableKwds(g, CLng(SouOperTyp + 140))
    ElseIf g.col = 2 Then 'smemn
      i = (SouOperTyp + 140) * 1000 + SouGrpInd + 1
      Call SetTableKwds(g, i)
    ElseIf g.col = 3 Then 'smems1
    ElseIf g.col = 4 Then 'smems2
    ElseIf g.col = 5 Then 'mfactr
    ElseIf g.col = 6 Then 'tvol
      Call SetActiveOpn(g)
    ElseIf g.col = 7 Then 'tmemnm
      Call SetTableKwds(g, CLng(TarOperTyp + 140))
    ElseIf g.col = 8 Then 'tgrpn
      i = (TarOperTyp + 140) * 1000 + TarGrpInd + 1
      Call SetTableKwds(g, i)
    ElseIf g.col = 9 Then 'tmems1
    ElseIf g.col = 10 Then 'tmems2
    End If
  End If
End Sub
Private Sub Edit_Schematic(g As Control, etype%)
  'etype - 1:text change, 2:RowCol change
  Static SouOperTyp&, TarOperTyp&, LastRow&
    
  If g.row <> LastRow Then
    'update info on current state of this row
    SouOperTyp = GetOperType(g.TextMatrix(g.row, 0))
    TarOperTyp = GetOperType(g.TextMatrix(g.row, 3))
    LastRow = g.row
  End If
  
  If etype = 1 Then 'text change
    If g.col = 0 Then 'svol
      SouOperTyp = GetOperType(g.text)
    ElseIf g.col = 3 Then 'tvol
      TarOperTyp = GetOperType(g.text)
    End If
  Else 'RowCol change
    g.ClearValues
    If g.col = 0 Then 'svol
      Call SetActiveOpn(g)
    ElseIf g.col = 1 Then 'svolno
      g.SetColRange g.col, (oper(SouOperTyp).OpF), (oper(SouOperTyp).OpL)
    ElseIf g.col = 2 Then 'afacter
    ElseIf g.col = 3 Then 'tvol
      Call SetActiveOpn(g)
    ElseIf g.col = 4 Then 'tvolno
      g.SetColRange g.col, (oper(TarOperTyp).OpF), (oper(TarOperTyp).OpL)
    ElseIf g.col = 5 Then 'mslkno
    End If
  End If
End Sub
Private Function GetGrpInd(CurGrp$, OperTyp&) As Long
  Dim Init&, kwd$, kflg&, retid&, contfg&, Ind&, cnt
  
  Ind = OperTyp + 140
  Init = 1
  cnt = 1
  GetGrpInd = 0
  Do
    Call F90_GTNXKW(Init, Ind, kwd, kflg, contfg, retid)
    Init = 0
    If kwd = CurGrp Then
      GetGrpInd = cnt
    Else
      cnt = cnt + 1
    End If
  Loop While contfg = 1
  
End Function
Private Sub GetOperInfo(o As HSPFOper)
  'find information about hspf operation type
  Dim cbuff$, OmCode&, Init&, retcod&, retkey&, i%

  OmCode = 3
  Init = 1
  Call F90_XBLOCK(OmCode, Init, retkey, cbuff, retcod)
  Init = 0
  o.cnt = 0
  Do
    Call F90_XBLOCK(OmCode, Init, retkey, cbuff, retcod)
    If Mid(cbuff, 7, 6) = o.Name Then
      o.cnt = o.cnt + 1
      i = Mid(cbuff, 18, 3)
      If o.OpF = 0 Or i < o.OpF Then
        o.OpF = i
      End If
      If o.OpL = 0 Or i > o.OpL Then
        o.OpL = i
      End If
    End If
  Loop While retcod = 1 Or retcod = 2
  
End Sub
Private Function GetOperType(S$) As Long
    Dim i%
    
    GetOperType = 0
    i = 1
    Do
      If oper(i).Name = S Then
        GetOperType = i
        Exit Do
      Else
        i = i + 1
      End If
    Loop While i < UBound(oper)
End Function

Private Sub SetActiveOpn(g As Object)
  Dim i%, pos%
  
  pos = 0
  Do
    i = lstKwd.ItemData(pos)
    If i > 120 Then
      If oper(i - 120).Exists Then
        g.addValue oper(i - 120).Name
      End If
    End If
    pos = pos + 1
  Loop While pos < lstKwd.ListCount - 1
End Sub
Private Sub SetTableKwds(g As Control, Ind&)
  Dim Init&, kwd$, kflg&, retid&, contfg&
  
  Init = 1
  Do
    Call F90_GTNXKW(Init, Ind, kwd, kflg, contfg, retid)
    g.addValue kwd
    Init = 0
  Loop While contfg = 1

End Sub
Private Sub open_table()
  lblBlock.Visible = True
  For i = 0 To 4
    cmdTable(i).Visible = True
  Next i
  txtRec.Visible = True
  'lblBlock.Caption = lstKwd.List(lstKwd.ListIndex)
  On Error Resume Next ' kludge for vb prob?
  frmGenScnActivate.Height = 6252
  'frmGenScnActivate.Height = 6252
  On Error GoTo 0
  'frmGenScnActivate.BorderStyle = 2 'resizable
  For i = 0 To 5
    cmdOpt(i).Enabled = False
  Next i
  lstTname.Enabled = False
  lstKwd.Enabled = False
  optOpnKwd(0).Enabled = False
  optOpnKwd(1).Enabled = False
  optOpnTname(0).Enabled = False
  optOpnTname(1).Enabled = False
End Sub

Private Sub Put_Table_Back()
  Dim ucibf$, r&, c&, k&, replaceRows&
  'Call F90_SCNDBG(-2)
  With gridctrl1
    If nkey < gridctrl1.Rows Then replaceRows = nkey Else replaceRows = gridctrl1.Rows
    For r = 1 To replaceRows
      GoSub buildRow
      If akey(r) > 0 Then
        Call F90_REPUCI(akey(r), ucibf, Len(ucibf))
      Else
        MsgBox "PutTableBack:repl" & akey(r)
      End If
    Next r
    For k = replaceRows + 1 To nkey
      If akey(k) > 0 Then
        Call F90_DELUCI(akey(k))
      Else
        MsgBox "PutTableBack:dele" & akey(k)
      End If
    Next k
    For r = replaceRows + 1 To gridctrl1.Rows
      GoSub buildRow
      If akey(nkey) > 0 Then
        Call F90_PUTUCI(akey(nkey), 0, ucibf, Len(ucibf))
      Else
        MsgBox "PutTableBack:add" & akey(nkey)
      End If
    Next r
  End With
  Exit Sub
  
buildRow:
  Dim txt$, lentxt%, pos%
  ucibf = ""
  pos = 1
  For c = 0 To gridctrl1.cols - 1
    If pos < lscol(c) Then
      ucibf = ucibf & Space(lscol(c) - pos)
      pos = lscol(c)
    End If
    txt = gridctrl1.TextMatrix(r, c)
    lentxt = Len(txt)
    If lentxt > lflen(c) Then
      ucibf = ucibf & Left(txt, lflen(c))
    Else
      If IsNumeric(txt) Then
        ucibf = ucibf & Space(lflen(c) - lentxt) & txt
      Else
        ucibf = ucibf & txt & Space(lflen(c) - lentxt)
      End If
    End If
    pos = pos + lflen(c)
  Next c
  Return
End Sub

Private Sub Put_TextBox_Back()
  Dim ucibf$, istart&, iend&, ilen&, moretext
  istart = 1
  moretext = True
  For i = 1 To nkey
    If moretext = True Then
      iend = InStr(istart, txtRec.text, Chr(13) & Chr(10))
      If iend = 0 Then
        'last line of text
        ilen = 80
        If akey(i) > 0 Then
          moretext = False
        End If
      Else
        ilen = iend - istart
      End If
      ucibf = Mid(txtRec.text, istart, ilen)
      If akey(i) > 0 Then
        Call F90_REPUCI(akey(i), ucibf, Len(ucibf))
      End If
      istart = iend + 2
    Else
      'remove this record
      Call F90_DELUCI(akey(i))
    End If
  Next i
  While moretext = True
    'still have more lines to put
    iend = InStr(istart, txtRec.text, Chr(13) & Chr(10))
    If iend = 0 Then
      'last line of text
      ilen = 80
      moretext = False
    Else
      ilen = iend - istart
    End If
    ucibf = Mid(txtRec.text, istart, ilen)
    Call F90_PUTUCI(akey(nkey), 0, ucibf, Len(ucibf))
    istart = iend + 2
  Wend
End Sub

Private Sub unfill_lstRec()
  txtRec.Visible = False
  lblBlock.Visible = False
  gridctrl1.Visible = False
  For i = 0 To 6
    cmdTable(i).Visible = False
  Next i
  picGlobal.Visible = False
  nkey = 0
  ReDim akey(nkey)
  txtRec.text = ""
  If frmGenScnActivate.WindowState = vbNormal Then
    frmGenScnActivate.Height = txtRec.Top - 20
    frmGenScnActivate.Width = fraTname.Left + fraTname.Width + 300
  End If
  'frmGenScnActivate.BorderStyle = 3 'Fixed Dialog, no resize
  For i = 0 To 5
    cmdOpt(i).Enabled = True
  Next i
  lstTname.Enabled = True
  lstKwd.Enabled = True
  optOpnKwd(0).Enabled = True
  optOpnKwd(1).Enabled = True
  optOpnTname(0).Enabled = True
  optOpnTname(1).Enabled = True
End Sub
Private Sub refresh_optKwd()
     Dim kwd$, kflg&, Init&, id&, contfg&, retid&, Ind%
     
     lstKwd.Clear
     Init = 1
     id = 0
     Do
       Call F90_GTNXKW(Init, id, kwd, kflg, contfg, retid)
       If (kflg > 0 Or optOpnKwd(0)) Then 'And retid <> 2 Then
         lstKwd.AddItem kwd
         lstKwd.ItemData(lstKwd.newIndex) = retid
         If kflg > 0 And (retid > 120 And retid < 132) Then
           'this operation exists, mark it
           Ind = retid - 120
           oper(Ind).Exists = True
           oper(Ind).Name = kwd
           Call GetOperInfo(oper(Ind))
         End If
       End If
       Init = 0
     Loop While contfg = 1
     fraTname.Visible = False

End Sub

Private Sub refresh_lstTname(id As Integer)
  Dim kwd$, kflg&, Init&, contfg&, retid&, lid&
  Dim ctxt$, i&, noccur&
  
  lstTname.Clear
  If id > 0 Then
    Init = 1
    Do
      Call F90_GTNXKW(Init, CLng(id), kwd, kflg, contfg, retid)
      If (kflg > 0 Or optOpnTname(0)) And retid <> 0 Then
        'check for multiple occurances
        Call F90_GETOCR(retid, noccur)
        If noccur > 1 Then
          'multiple occurances
          For i = 1 To noccur
            ctxt = kwd & " (" & i & ")"
            lstTname.AddItem ctxt
            lstTname.ItemData(lstTname.newIndex) = retid
          Next i
        Else
          'single occurance
          lstTname.AddItem kwd
          lstTname.ItemData(lstTname.newIndex) = retid
        End If
      End If
      Init = 0
    Loop While contfg = 1
    optOpnTname(0).Visible = True
    optOpnTname(1).Visible = True
    lstTname.Height = lstKwd.Height
    fraTname.Caption = "Table"
  Else
    optOpnTname(0).Visible = False
    optOpnTname(1).Visible = False
    lstTname.Height = optOpnTname(0).Top + optOpnTname(0).Height - lstTname.Top
    If id = -1 Then 'MassLink
      fraTname.Caption = "MassLink"
    ElseIf id = -2 Then 'Ftable
      fraTname.Caption = "Ftable"
    End If
    lid = id - 100
    Init = 1
    Do
      Call F90_GTNXKW(Init, lid, kwd, kflg, contfg, retid)
      If retid <> 0 Then
        lstTname.AddItem fraTname.Caption & " " & LTrim(kwd)
        lstTname.ItemData(lstTname.newIndex) = retid
      End If
      Init = 0
    Loop While contfg = 1
  End If
  fraTname.Visible = True
End Sub

Private Sub open_lstRecTab()
  Dim cbuff$, OmCode&, tabno&, Init&, retcod&, uunits&, retkey&, temptext$
  Dim lastUnbroken%, breakAfter% 'for deciding which cols headers go in
  Dim i&, j&, c&, ch$, thisoccur&, noccur&, ctmp$, isect&, irept&
  Dim lRow&, lCol&
  
  hdrbuf(0) = ""
  open_table
  Call F90_PUTOLV(6)
  nkey = 0
  ReDim akey(nkey)
  txtRec.text = ""
  temptext = ""
  With gridctrl1
    .Clear
    .cols = 2
    .Rows = 2
  
    Init = 1
    uunits = comboUnits.ListIndex + 1
    OmCode = lstKwd.ItemData(lstKwd.ListIndex)
    tabno = lstTname.ItemData(lstTname.ListIndex)
    tabno = tabno - ((OmCode - 120) * 1000)
    
    Call F90_XTINFO(OmCode, tabno, uunits, 1, _
      lnflds, lscol, lflen, lftyp, lapos, _
      limin, limax, lidef, lrmin, lrmax, lrdef, _
      lnmhdr, hdrbuf, lfdnam, isect, irept, retcod)
    If retcod <> 0 Then MsgBox "lstRecTab:XTINFO:ret: " & retcod
    If lnflds < 1 Then
      .Header = "This table does not exist in this project."
      Exit Sub
    End If
    
    .Rows = 1000
    .FixedRows = 1
    .cols = lnflds
    
    If lflen(0) = 8 Then
      For c = 1 To lnflds - 1
        lscol(c) = lscol(c) + 2
      Next c
    End If
  
    Dim tlen%
    For c = 0 To lnflds - 1
      .ColTitle(c) = lfdnam(c)
      tlen = 0
      If lfdnam(c) = "OPNID" Then
        tlen = Len("OPNID")
        lflen(0) = 5
        .ColEditable(c) = False
      Else
        .ColEditable(c) = True
      End If
      ch = Mid(lftyp, c + 1, 1)
      If ch = "C" Then
        tlen = lflen(c) + 2
      ElseIf tlen < lflen(c) Then
        tlen = lflen(c)
      End If
      If Len(lfdnam(c)) > tlen Then
        tlen = Len(lfdnam(c))
      End If
      .colWidth(c) = Me.TextWidth(String(tlen, "R"))
     
      If ch = "C" Then
        .ColType(c) = ATCoCtl.ATCoTxt
        .ColMin(c) = 0
        .ColMax(c) = (lflen(c))
      ElseIf ch = "I" Then
        .ColType(c) = ATCoCtl.ATCoInt
        .ColMin(c) = (limin(lapos(c) - 1))
        .ColMax(c) = (limax(lapos(c) - 1))
      ElseIf ch = "R" Then
        .ColType(c) = ATCoCtl.ATCoSng
        .ColMin(c) = lrmin(lapos(c) - 1)
        .ColMax(c) = lrmax(lapos(c) - 1)
      End If
    Next c
    
    .Header = "" 'hdrbuf(0)
    Dim lenHdr
    .Header = hdrbuf(0)
    For i = 1 To lnmhdr - 2
      .Header = .Header & vbCr & hdrbuf(i)
    Next i
    
    lRow = 1
    txtRec.Visible = False
    thisoccur = 1
    'check for multiple occurances
    Call F90_GETOCR(lstTname.ItemData(lstTname.ListIndex), noccur)
    If noccur > 1 Then
      'which occurance is wanted
      ctmp = lstTname.List(lstTname.ListIndex)
      thisoccur = CInt(Mid(ctmp, Len(ctmp) - 1, 1))
    End If
    
    Do
      Call F90_XTABLE(OmCode, tabno, uunits, Init, CLng(1), thisoccur, _
                      retkey, cbuff, retcod)
      Init = 0
      'If (retcod = 1 Or retcod = 2) And nkey < 1000 Then
      If nkey < 1000 Then
        If Len(temptext) = 0 Then
          temptext = cbuff
        Else
          temptext = temptext & vbCrLf & cbuff
        End If
        
        If retcod = 2 Then
          ' **** may need add multiple records with opn range (something in col 6/10)
          nkey = nkey + 1
          ReDim Preserve akey(nkey)
          akey(nkey) = retkey
          For lCol = 0 To .cols - 1
            If Len(cbuff) > lscol(lCol) Then
              .TextMatrix(lRow, lCol) = Trim(Mid(cbuff, lscol(lCol), lflen(lCol)))
            Else
              .TextMatrix(lRow, lCol) = ""
            End If
          Next lCol
          lRow = lRow + 1
        End If
      End If
    Loop While retcod = 1 Or retcod = 2
    .Visible = True
    txtRec.Visible = Not .Visible
    txtRec.text = temptext
  End With
End Sub
Public Sub SaveUci()
  If Len(txtInfo.text) = 0 Then
    inf = "<none>"
  Else
    inf = txtInfo.text
  End If
  ou = comboOutput.ListIndex
  sp = comboSpecial.ListIndex
  ru = comboRunflag.ListIndex
  em = comboUnits.ListIndex + 1
  Me.MousePointer = vbHourglass
  Call F90_PUTGLO(S(0), e(0), ou, sp, ru, em, inf, Len(inf))
  Call F90_UCISAV
  Me.MousePointer = vbDefault
End Sub


Private Sub open_lstRecOpn()
  Dim cbuff$, OmCode&, Init&, retcod&, retkey&, temptext$
  Dim i&, c&, ch$, isect&, irept&
  
  'Call F90_SCNDBG(2)
  open_table
  hdrbuf(0) = ""
  nkey = 0
  ReDim akey(nkey)
  txtRec.text = ""
  temptext = ""
  With gridctrl1
    .Clear
    gridctrl1.cols = 2
    .Rows = 2
    OmCode = lstKwd.ItemData(lstKwd.ListIndex)
    If OmCode = 4 Or OmCode = 11 Then
      Init = lstTname.ItemData(lstTname.ListIndex)
    Else
      Init = 1
    End If
    
    Call F90_XTINFO(OmCode, 0, 0, 1, _
        lnflds, lscol, lflen, lftyp, lapos, _
        limin, limax, lidef, lrmin, lrmax, lrdef, _
        lnmhdr, hdrbuf, lfdnam, isect, irept, retcod)
    If retcod <> 0 Then
      MsgBox "lstRecTab:XTINFO:ret: " & retcod & " " & OmCode
    Else
      If lnflds < 1 Then
        .Header = "This table does not exist in this project."
        Exit Sub
      End If
      If OmCode <> 4 Then ' not ftable
        lftyp = Mid(lftyp, 2, lnflds - 1) 'left shift types
        For c = 1 To lnflds - 1
          lscol(c - 1) = lscol(c) - 3
          lflen(c - 1) = lflen(c)
          lfdnam(c - 1) = lfdnam(c)
          lapos(c - 1) = lapos(c)
          If Mid(lftyp, c, 1) = "I" Then
            limin(lapos(c - 1)) = limin(lapos(c))
            limax(lapos(c - 1)) = limax(lapos(c))
            lidef(lapos(c - 1)) = lidef(lapos(c))
          ElseIf Mid(lftyp, c, 1) = "R" Then
            lrmin(lapos(c - 1)) = lrmin(lapos(c))
            lrmax(lapos(c - 1)) = lrmax(lapos(c))
            lrdef(lapos(c - 1)) = lrdef(lapos(c))
          End If
        Next c
        lnflds = lnflds - 1
      End If
      .cols = lnflds
      .Rows = 1000
      .Header = hdrbuf(0)
      For i = 1 To lnmhdr - 1
        .Header = .Header & vbCr & hdrbuf(i)
      Next i
      
      Dim tlen%
      For c = 0 To lnflds - 1
        .ColTitle(c) = lfdnam(c)
        .ColEditable(c) = True
        ch = Mid(lftyp, c + 1, 1)
        If ch = "C" Then
          tlen = lflen(c) + 2
        Else
          tlen = lflen(c)
        End If
        If Len(lfdnam(c)) > tlen Then
          tlen = Len(lfdnam(c))
        End If
        .colWidth(c) = Me.TextWidth(String(tlen, "R"))
        If ch = "C" Then
          .ColType(c) = ATCoCtl.ATCoTxt
          .ColMin(c) = 0
          .ColMax(c) = (lflen(c))
        ElseIf ch = "I" Then
          .ColType(c) = ATCoCtl.ATCoInt
          .ColMin(c) = (limin(lapos(c) - 1))
          .ColMax(c) = (limax(lapos(c) - 1))
        ElseIf ch = "R" Then
          .ColType(c) = ATCoCtl.ATCoSng
          .ColMin(c) = lrmin(lapos(c) - 1)
          .ColMax(c) = lrmax(lapos(c) - 1)
        End If
      Next c
    
      .ClearValues
      .row = 1
      txtRec.Visible = False
      Do
        Call F90_XBLOCK(OmCode, Init, retkey, cbuff, retcod)
        'If (retcod = 1 Or retcod = 2) And nkey < 500 Then
        If nkey < 500 Then
          If Len(temptext) = 0 Then
            temptext = cbuff
          Else
            temptext = temptext & vbCrLf & cbuff
          End If
         
          If OmCode = 4 And Init > 0 Then
            'skip row col part of ftable
          ElseIf retcod = 2 Or (OmCode = 4 And retcod = 10) Or (OmCode = 11 And retcod = 10) Then
            nkey = nkey + 1
            ReDim Preserve akey(nkey)
            akey(nkey) = retkey
            For c = 0 To .cols - 1
              .col = c
              If Len(cbuff) > lscol(c) Then
                If OmCode <> 9 Then
                  'not specact block, can trim
                  .text = Trim(Mid(cbuff, lscol(c), lflen(c)))
                Else
                  'need indentation
                  .text = Mid(cbuff, lscol(c), lflen(c))
                End If
              Else
                .text = ""
              End If
            Next c
            .row = .row + 1
          End If
       
        End If
        Init = 0
      Loop While retcod = 1 Or retcod = 2
      .row = .row - 1
      .Rows = .row
      .row = 1
      .col = 0
      .Visible = True
      txtRec.Visible = Not .Visible
      txtRec.text = temptext
    End If
  End With
  'Call F90_SCNDBG(0)
End Sub

Private Sub cmdOpt_Click(index As Integer)
  Dim cap$, Filename$, retcod&, Init&, OmCode&, retkey&, cbuff$, vWDMFile As Variant
  Dim wdmUnits&(4), l&, i&
  
  If index = 0 Then 'simulate
    Dim d&, r&, rsp%, lena&, lenn&, newcap$
    d = 3
    Me.MousePointer = vbHourglass
    On Error Resume Next
    
    For i = 1 To 4
      wdmUnits(i) = 0
      If p.WDMFiles.Count >= i Then
        If p.WDMFiles(i).FileUnit > 0 Then
          wdmUnits(i) = p.WDMFiles(i).FileUnit
          l = F90_WDMOPN(wdmUnits(i), p.WDMFiles(i).Filename, Len(p.WDMFiles(i).Filename))
        End If
      End If
    Next i
    
    Call F90_SIMSCN(r)
    
    For i = 1 To 4    'keep wdms closed
      If wdmUnits(i) >= 0 Then
        l = F90_WDMCLO(wdmUnits(i))
      End If
    Next i
    
    'we might have just added or changed data
    For Each vWDMFile In p.WDMFiles
      vWDMFile.Refresh
    Next
    RefreshSLC
    Me.MousePointer = vbDefault
    On Error GoTo 0
  ElseIf index = 1 Then 'save
    rsp = MsgBox("Do you want to Overwrite Scenario '" & Left(lScenFile, Len(lScenFile) - 4) & "'?", _
      4, "GenScnActivateSave Overwrite Query")
    If rsp = vbYes Then
      Call SaveUci
    End If
  ElseIf index = 2 Then 'save as
    frmGenActSV.Show 1, frmGenScnActivate
    'newname = InputBox("Enter new scenario name (eight characters maximum):", "GenScnActivate SaveAs")
    If newName <> "" Then
      newName = UCase(newName)
      Dim invalid&
      invalid = 0
      For i = 0 To frmGenScn.lstSLC(0).ListCount - 1
        If frmGenScn.lstSLC(0).List(i) = newName Then
          'invalid, name already used
          invalid = 1
        End If
      Next i
      If invalid = 1 Then
        MsgBox "This scenario name is already in use.", _
        vbExclamation, "GenScnActivate SaveAs"
      Else
        Me.MousePointer = vbHourglass
        lena = Len(lScenFile) - 4
        lenn = Len(newName)
        'Call F90_COPSCN(BaseDSN, RelAbs, Left(lScenFile, lena), Left(newname, lenn), lena, lenn)
        Call newOutputDsns(BaseDSN, RelAbs, Left(lScenFile, lena), newName$)
        'change the file names in the files block in memory
        Call F90_NEWFIL(Left(lScenFile, lena), Left(newName, lenn), lena, lenn)
        
        lScenFile = Left(newName, lenn) & ".uci"
        'reset files list
        lstFiles.Clear
        retcod = 0
        Init = 1
        OmCode = 12
        Do
          Call F90_XBLOCK(OmCode, Init, retkey, cbuff, retcod)
          If retcod <> 2 Then Exit Do
          Init = 0
          If InStr(UCase(cbuff), "WDM") = 0 Then
            lstFiles.AddItem Right(cbuff, Len(cbuff) - 16)
          End If
        Loop
        lstFiles.ListIndex = 0
        If Len(txtInfo.text) = 0 Then
          inf = "<none>"
        Else
          inf = txtInfo.text
        End If
        ou = comboOutput.ListIndex
        sp = comboSpecial.ListIndex
        ru = comboRunflag.ListIndex
        em = comboUnits.ListIndex + 1
        Call F90_PUTGLO(S(0), e(0), ou, sp, ru, em, inf, Len(inf))
        Call F90_UCISAV
        newcap = Mid(Caption, 1, 16) + Left(newName, lenn) + "        "
        Caption = newcap
        'reset scenario list and number of data sets
        'p.ScenCount = p.ScenCount + 1
        'ReDim Preserve p.Scen(p.ScenCount)
        'p.Scen(p.ScenCount - 1).Name = newName
        p.ScenName.Add newName, newName
        p.ScenFile.Add RelativeFilename(CurDir & "\" & lScenFile, p.StatusFilePath), newName
        txtPath = p.ScenFile(newName)
        p.ScenType.Add "HSPF", newName
        p.ScenDesc.Add inf, newName
        frmGenScn.lstSLC(0).AddItem newName
        p.EditFlg = True
        Dim Scen$, newcnt&
        'scen = "Scenarios:" & Chr(13) & Chr(10)
        Scen = frmGenScn.lstSLC(0).SelCount & " of " & frmGenScn.lstSLC(0).ListCount
        frmGenScn.lblSLC(0).Caption = Scen
        'refresh wdm
        For Each vWDMFile In p.WDMFiles
          vWDMFile.Refresh
        Next
        RefreshSLC
        CntDsn = CountAllTimser
        'Call frmGenScn.UpdateLblDsn
        Me.MousePointer = vbDefault
      End If
    End If
  ElseIf index = 3 Then 'close
    Unload frmGenScnActivate
  ElseIf index = 4 Then 'view file from files block
    cap = "GenScn Activate View"
    Filename = lstFiles.List(lstFiles.ListIndex)
    Call DispFile.OpenFile(Filename, cap, frmGenScnActivate.Icon, False)
  ElseIf index = 5 Then 'modify
    frmGenActMod.Show 1
    Call refresh_optKwd
  End If
End Sub

Private Sub newOutputDsns(BaseDSN&, RelAbs&, oldn$, newn$)
  'build new output dsns on saveas
  Dim lTs As Collection 'of atcotimser
  Dim addedDsn As Boolean, update As Boolean
  Dim wdmsfl&, wdmid&, i&, ndsn&, cwdm$, tstype$, testWdmid&
  Dim GenTs As ATCclsTserData
  Dim TsDate As ATCclsTserDate
  Dim myDateSummary As ATTimSerDateSummary
  
  'look for output wdm
'  For i = p.WDMFiles.Count To 1 Step -1
'    If p.WDMFiles(i).fileUnit > 0 Then
'      'use this as the output wdm
'      wdmsfl = p.WDMFiles(i).fileUnit
'      wdmid = i
'    End If
'  Next i
  
  'look for matching WDM datasets
  Call FindTimSer(UCase(oldn), "", "", lTs)
  'return the names of the data sets from this wdm file
  For i = 1 To lTs.Count
    If lTs(i).File.label = "WDM" Then 'this is a wdm data set
      
      wdmid = 0
      testWdmid = 1
      Do While testWdmid <= p.WDMFiles.Count And wdmid = 0
        If p.WDMFiles(testWdmid).FileUnit = lTs(i).File.FileUnit Then
          wdmid = testWdmid
          wdmsfl = p.WDMFiles(wdmid).FileUnit
        Else
          testWdmid = testWdmid + 1
        End If
      Loop
      
      If wdmid > 0 Then
        'find a free dsn
        If RelAbs = 1 Then
          ndsn = BaseDSN
        Else
          ndsn = lTs(i).Header.id + BaseDSN
        End If
        Do 'look in wdmx for this data set
          Set GenTs = GetDataSetFromDsn(wdmid, ndsn)
          If GenTs Is Nothing Then
            'found unused data set number
            Set GenTs = New ATCclsTserData
            Exit Do
          Else 'try next dsn number
            ndsn = ndsn + 1
          End If
        Loop
        
        'set attribs to the old version
        With GenTs.Header
          .id = ndsn
          .sen = newn
          .con = lTs(i).Header.con
          .loc = lTs(i).Header.loc
        End With
        Set TsDate = New ATCclsTserDate
        With myDateSummary
          .CIntvl = lTs(i).dates.Summary.CIntvl
          .ts = lTs(i).dates.Summary.ts
          .Tu = lTs(i).dates.Summary.Tu
          .Intvl = lTs(i).dates.Summary.Intvl
        End With
        TsDate.Summary = myDateSummary
        Set GenTs.dates = TsDate
      
        'now add the timser
        addedDsn = AddWDMDataSet(wdmid, ndsn, newn, lTs(i).Header.loc, _
                      lTs(i).Header.con, lTs(i).dates.Summary.Tu, _
                      lTs(i).dates.Summary.ts)
                      
        'update tstype attribute
        Set GenTs = GetDataSetFromDsn(wdmid, ndsn)
        If Not GenTs Is Nothing Then
          tstype = lTs(i).Attrib("TSTYPE")
          GenTs.AttribSet "TSTYPE", tstype
          update = GenTs.File.WriteDataHeader(GenTs)
        End If
                      
        'change the appropriate ext targets record
        Call F90_NEWDSN(wdmid, lTs(i).Header.id, ndsn)
      End If
    End If
  Next i
End Sub

Public Function AddWDMDataSet(wdmid&, dsn&, Scen$, locn$, Cons$, Tu&, ts&) As Boolean
  Dim TsDate As ATCclsTserDate
  Dim GenTs As New ATCclsTserData
  Dim myDateSummary As ATTimSerDateSummary
  
  With GenTs.Header
    .id = dsn
    .sen = Scen
    .con = Cons
    .loc = locn
  End With
  Set TsDate = New ATCclsTserDate
  With myDateSummary
    .CIntvl = True
    .ts = ts
    .Tu = Tu
    .Intvl = 1
  End With
  TsDate.Summary = myDateSummary
  Set GenTs.dates = TsDate
  AddWDMDataSet = p.WDMFiles(wdmid).addtimser(GenTs, 0)
  
End Function

Public Function GetDataSetFromDsn(lWdmInd&, lDsn&) As ATCclsTserData
  Dim i&, lWdmObj As ATCclsTserFile
  
  Set lWdmObj = p.WDMFiles(lWdmInd)
  With lWdmObj
    For i = 1 To .DataCount
      If lDsn = .Data(i).Header.id Then
        Set GetDataSetFromDsn = .Data(i)
        Exit Function
      End If
    Next i
    Set GetDataSetFromDsn = Nothing
    'MsgBox "DSN " & lDsn & " does not exist.", vbOKOnly
  End With
End Function

Private Sub cmdTable_Click(index As Integer)
    If index = 0 Then 'ok, put table back
      If lstKwd.ItemData(lstKwd.ListIndex) <> 2 Then
        Call Put_Table_Back
      End If
      Call unfill_lstRec
    ElseIf index = 1 Then 'cancel
      If lstKwd.ListIndex < 0 Then 'nothing selected
        'MsgBox "Bad lstkwd.listindex (missing table)"
      ElseIf lstKwd.ItemData(lstKwd.ListIndex) = 2 Then
        'put global block settings back to the way they were
        comboOutput.ListIndex = ousav
        comboSpecial.ListIndex = spsav
        comboRunflag.ListIndex = rusav
        comboUnits.ListIndex = emsav
        txtStart.text = sysav
        txtEnd.text = eysav
        txtSmon.text = smosav
        txtEmon.text = emosav
        txtSday.text = sdsav
        txtEday.text = edsav
        txtShour.text = shsav
        txtEhour.text = ehsav
        txtSmin.text = smisav
        txtEmin.text = emisav
      End If
      Call unfill_lstRec
    ElseIf index = 2 Then 'help
      Dim d As HH_AKLINK, h$
      k = lblBlock.Caption & lstKwd.text
      d.pszKeywords = k
      d.fReserved = vbFalse
      d.cbStruct = LenB(d)
      h = Trim(PathNameOnly(App.HelpFile)) & "\HSPF.chm"
      HtmlHelp Me.hwnd, h, HH_ALINK_LOOKUP, d
      'OpenFile h, frmGenScn.cdl
      'i = WinHelp(CLng(frmGenScnActivate.hwnd), ExePath & "doc\hspfhelp.chm", _
      '            CLng(HELP_KEY), k)
    ElseIf index = 3 Then 'insert
      gridctrl1.InsertRow (1)
    ElseIf index = 4 Then 'delete
      If gridctrl1.Rows > 1 Then
        gridctrl1.DeleteRows
      Else
        MsgBox "Can't delete last row"
      End If
    ElseIf index = 5 Then 'toggle text/grid
      gridctrl1.Visible = Not gridctrl1.Visible
      txtRec.Visible = Not gridctrl1.Visible
    ElseIf index = 6 Then 'debug
      Dim S$, t$
      If lstKwd.ListIndex < 0 Then
        S = lstKwd.ListIndex
      Else
        S = "Name: " & lstKwd.List(lstKwd.ListIndex) & vbCrLf
        S = S & "Type: " & lstKwd.ItemData(lstKwd.ListIndex) & vbCrLf
        S = S & "NFld: " & lnflds & vbCrLf
        For i = 0 To lnflds - 1
          S = S & i & vbTab & "FNam: " & lfdnam(i) & vbTab
          t = Mid(lftyp, i + 1, 1)
          S = S & " T:" & t & vbTab & "P:" & lapos(i) & vbTab & " S:" & lscol(i) & vbTab & "W:" & lflen(i) & vbTab
          If t = "I" Then
            S = S & "Mn: " & limin(lapos(i) - 1) & vbTab & "Mx: " & limax(lapos(i) - 1) & vbCrLf
          ElseIf t = "R" Then
            S = S & "Mn: " & lrmin(lapos(i) - 1) & vbTab & "Mx: " & lrmax(lapos(i) - 1) & vbCrLf
          ElseIf t = "C" Then
            S = S & vbCrLf
          End If
        Next i
      End If
      MsgBox S
    End If
End Sub

Private Sub Form_Resize()
  With frmGenScnActivate
    If .Width > 1500 Then
      txtPath.Width = .Width - 1500
      txtInfo.Width = txtPath.Width
      gridctrl1.Width = .Width - 675
      txtRec.Width = gridctrl1.Width
    End If
    If .Height > 5000 Then
      gridctrl1.Height = .Height - gridctrl1.Top - 500
      txtRec.Height = gridctrl1.Height
    End If
  End With
End Sub

Private Sub Form_Unload(Cancel As Integer)
  'frmGenScn.Enabled = True
  'frmGenScn.MousePointer = vbDefault
  frmGenScn.cmdSen(0).Enabled = True
  frmGenScn.cmdSen(1).Enabled = True
  frmGenScn.cmdSen(2).Enabled = True
  
  i = WinHelp(CLng(frmGenScnActivate.hwnd), "", CLng(HELP_QUIT), CLng(0))
End Sub


Private Sub GridCtrl1_RowColChange()
  With gridctrl1
    If .Visible Then
      If lblBlock.Caption = "EXT SOURCES" Then
        Call Edit_ExtSources(gridctrl1, 2)
      ElseIf lblBlock.Caption = "EXT TARGETS" Then
        Call Edit_ExtTargets(gridctrl1, 2)
      ElseIf lblBlock.Caption = "NETWORK" Then
        Call Edit_Network(gridctrl1, 2)
      ElseIf lblBlock.Caption = "MASS-LINK" Then
        Call Edit_MassLink(gridctrl1, 2)
      ElseIf lblBlock.Caption = "SCHEMATIC" Then
        Call Edit_Schematic(gridctrl1, 2)
      End If
    End If
  End With
End Sub

Private Sub gridctrl1_TextChange(ChangeFromRow As Long, ChangeToRow As Long, ChangeFromCol As Long, ChangeToCol As Long)
  With gridctrl1
    If .Visible Then
      If lblBlock.Caption = "EXT SOURCES" Then
        Call Edit_ExtSources(gridctrl1, 1)
      ElseIf lblBlock.Caption = "EXT TARGETS" Then
        Call Edit_ExtTargets(gridctrl1, 1)
      ElseIf lblBlock.Caption = "NETWORK" Then
        Call Edit_Network(gridctrl1, 1)
      ElseIf lblBlock.Caption = "MASS-LINK" Then
        Call Edit_MassLink(gridctrl1, 1)
      ElseIf lblBlock.Caption = "SCHEMATIC" Then
        Call Edit_Schematic(gridctrl1, 1)
      End If
    End If
  End With
End Sub

Private Sub lstFiles_DblClick()
  cmdOpt_Click (4)
End Sub

Private Sub lstKwd_Click()
    Dim S$
    
    Me.MousePointer = vbHourglass
    If lstKwd.ItemData(lstKwd.ListIndex) = 2 Then
      'global block
      fraTname.Visible = False
      Call Edit_Global
    ElseIf lstKwd.ItemData(lstKwd.ListIndex) < 100 Then
      fraTname.Visible = False
      lstTname.Clear
      S = lstKwd.List(lstKwd.ListIndex)
      lblBlock.Caption = S
      If S = "MASS-LINK" Then 'which one
        Call refresh_lstTname(-1)
      ElseIf S = "FTABLES" Then 'which one
        Call refresh_lstTname(-2)
      Else
        Call open_lstRecOpn
      End If
    Else
      Call refresh_lstTname(lstKwd.ItemData(lstKwd.ListIndex))
    End If
    Me.MousePointer = vbDefault

End Sub

Private Sub lstTname_Click()
  lblBlock.Caption = lstTname.List(lstTname.ListIndex)
  If fraTname.Caption = "Table" Then
    'need to check to see if this table exists
    i = lstKwd.ItemData(lstKwd.ListIndex)
    If oper(i - 120).Exists Then
      Call open_lstRecTab
    Else
      MsgBox "Adding a new operation type block is not yet supported.", 64, "GenScn Activate"
      lblBlock.Caption = ""
    End If
  ElseIf fraTname.Caption = "Ftable" Then
    Call open_lstRecOpn
  ElseIf fraTname.Caption = "MassLink" Then
    Call open_lstRecOpn
  End If
End Sub

Private Sub optOpnKwd_Click(index As Integer)
    Call refresh_optKwd
    lstTname.Clear
End Sub

Private Sub optOpnTname_Click(index As Integer)
    Dim i%
    If lstKwd.ListIndex >= 0 Then
      i = lstKwd.ItemData(lstKwd.ListIndex)
      If i >= 100 Then
        Call refresh_lstTname(i)
      End If
    End If
End Sub


Private Sub txtEDay_GotFocus()
    oldeday = txtEday.text
End Sub
Private Sub txtEDay_LostFocus()
    Dim snew$, chgflg&
    If Len(txtEday.text) > 0 Then
      snew = txtEday.text
      Call ChkTxtI("Day", 1, 31, snew, e(2), chgflg)
      If chgflg <> 1 Then
        txtEday.text = oldeday
      End If
    Else
      txtEday.text = oldeday
    End If
End Sub
Private Sub txtEhour_GotFocus()
    oldehour = txtEhour.text
End Sub
Private Sub txtEhour_LostFocus()
    Dim snew$, chgflg&
    If Len(txtEhour.text) > 0 Then
      snew = txtEhour.text
      Call ChkTxtI("Hour", 0, 24, snew, e(3), chgflg)
      If chgflg <> 1 Then
        txtEhour.text = oldehour
      End If
    Else
      txtEhour.text = oldehour
    End If
End Sub


Private Sub txtEmin_GotFocus()
    oldeminute = txtEmin.text
End Sub


Private Sub txtEmin_LostFocus()
    Dim snew$, chgflg&
    If Len(txtEmin.text) > 0 Then
      snew = txtEmin.text
      Call ChkTxtI("Minute", 0, 60, snew, e(4), chgflg)
      If chgflg <> 1 Then
        txtEmin.text = oldeminute
      End If
    Else
      txtEmin.text = oldeminute
    End If
End Sub


Private Sub txtEmon_GotFocus()
    oldemonth = txtEmon.text
End Sub


Private Sub txtEmon_LostFocus()
    Dim snew$, chgflg&
    If Len(txtEmon.text) > 0 Then
      snew = txtEmon.text
      Call ChkTxtI("Month", 1, 12, snew, e(1), chgflg)
      If chgflg <> 1 Then
        txtEmon.text = oldemonth
      End If
    Else
      txtEmon.text = oldemonth
    End If
End Sub


Private Sub txtEnd_GotFocus()
    oldeyear = txtEnd.text
End Sub


Private Sub txtEnd_LostFocus()
    Dim snew$, chgflg&
    If Len(txtEnd.text) > 0 Then
      snew = txtEnd.text
      Call ChkTxtI("Year", 1900, 2100, snew, e(0), chgflg)
      If chgflg <> 1 Then
        txtEnd.text = oldeyear
      End If
    Else
      txtEnd.text = oldeyear
    End If
End Sub

Private Sub txtSDay_GotFocus()
    oldsday = txtSday.text
End Sub


Private Sub txtSDay_LostFocus()
    Dim snew$, chgflg&
    If Len(txtSday.text) > 0 Then
      snew = txtSday.text
      Call ChkTxtI("Day", 1, 31, snew, S(2), chgflg)
      If chgflg <> 1 Then
        txtSday.text = oldsday
      End If
    Else
      txtSday.text = oldsday
    End If
End Sub

Private Sub txtShour_GotFocus()
    oldshour = txtShour.text
End Sub

Private Sub txtShour_LostFocus()
    Dim snew$, chgflg&
    If Len(txtShour.text) > 0 Then
      snew = txtShour.text
      Call ChkTxtI("Hour", 0, 24, snew, S(3), chgflg)
      If chgflg <> 1 Then
        txtShour.text = oldshour
      End If
    Else
      txtShour.text = oldshour
    End If
End Sub

Private Sub txtSmin_GotFocus()
    oldsminute = txtSmin.text
End Sub

Private Sub txtSmin_LostFocus()
    Dim snew$, chgflg&
    If Len(txtSmin.text) > 0 Then
      snew = txtSmin.text
      Call ChkTxtI("Minute", 0, 60, snew, S(4), chgflg)
      If chgflg <> 1 Then
        txtSmin.text = oldsminute
      End If
    Else
      txtSmin.text = oldsminute
    End If
End Sub

Private Sub txtSmon_GotFocus()
    oldsmonth = txtSmon.text
End Sub

Private Sub txtSmon_LostFocus()
    Dim snew$, chgflg&
    If Len(txtSmon.text) > 0 Then
      snew = txtSmon.text
      Call ChkTxtI("Month", 1, 12, snew, S(1), chgflg)
      If chgflg <> 1 Then
        txtSmon.text = oldsmonth
      End If
    Else
      txtSmon.text = oldsmonth
    End If
End Sub

Private Sub txtStart_GotFocus()
    oldsyear = txtStart.text
End Sub

Private Sub txtStart_LostFocus()
    Dim snew$, chgflg&
    If Len(txtStart.text) > 0 Then
      snew = txtStart.text
      Call ChkTxtI("Year", 1900, 2100, snew, S(0), chgflg)
      If chgflg <> 1 Then
        txtStart.text = oldsyear
      End If
    Else
      txtStart.text = oldsyear
    End If
End Sub

Public Property Get ScenFile() As String
  ScenFile = lScenFile
End Property

Public Property Let ScenFile(ByVal newValue As String)
    Dim r&, u$, l&
    Dim OmCode&, retcod&, Init&, retkey&, cbuff$, wdmUnits&(4), i&
          
    lScenFile = newValue
    
    'Call F90_SCNDBG(-1) '1:msg from Fortran dll to console, -1:msg and pause
    For i = 0 To 11
      oper(i).Exists = False
    Next i
    u = Left(lScenFile, Len(lScenFile) - 4)
    wdmUnits(0) = 0
    For i = 1 To 4
      wdmUnits(i) = 0
      If p.WDMFiles.Count >= i Then
        If p.WDMFiles(i).FileUnit > 0 Then
          wdmUnits(i) = p.WDMFiles(i).FileUnit
          l = F90_WDMOPN(wdmUnits(i), p.WDMFiles(i).Filename, Len(p.WDMFiles(i).Filename))
        End If
      End If
    Next i
    Call F90_ACTSCN(CLng(0), wdmUnits(1), p.HSPFMsg.Unit, r&, u, Len(u))
    Caption = Caption & " " & u
    txtPath.text = ScenPath(u)
    For i = 1 To 4    'keep closed
      If wdmUnits(i) >= 0 Then
        l = F90_WDMCLO(wdmUnits(i))
      End If
    Next i
    
    Call F90_GLOBLK(S(), e(), ou, sp, ru, em, inf)
    
    txtInfo.text = inf
    txtStart.text = S(0)
    txtSmon.text = S(1)
    txtSday.text = S(2)
    txtShour.text = S(3)
    txtSmin.text = S(4)
    txtEnd.text = e(0)
    txtEmon.text = e(1)
    txtEday.text = e(2)
    txtEhour.text = e(3)
    txtEmin.text = e(4)
     
    For i = 0 To 10
      comboOutput.AddItem i
      comboSpecial.AddItem i
    Next i
    comboOutput.ListIndex = ou
    comboSpecial.ListIndex = sp
     
    comboRunflag.AddItem "Interp"
    comboRunflag.AddItem "Run"
    comboRunflag.ListIndex = ru
    comboUnits.AddItem "English"
    comboUnits.AddItem "Metric"
    comboUnits.ListIndex = em - 1
     
    lstFiles.Clear
    retcod = 0
    Init = 1
    OmCode = 12
    Do
      Call F90_XBLOCK(OmCode, Init, retkey, cbuff, retcod)
      If retcod <> 2 Then Exit Do
      Init = 0
      If InStr(UCase(cbuff), "WDM") = 0 Then
        lstFiles.AddItem Right(cbuff, Len(cbuff) - 16)
      End If
    Loop
    lstFiles.ListIndex = 0
    
    Call refresh_optKwd
     
    lstTname.Clear
    On Error Resume Next ' kludge for vb prob?
    frmGenScnActivate.Height = txtRec.Top - 20
    On Error GoTo 0
    Show
    Me.MousePointer = vbDefault
     
End Property
