VERSION 5.00
Begin VB.Form frmGenProfPlot 
   Caption         =   "Profile Plot"
   ClientHeight    =   6924
   ClientLeft      =   5088
   ClientTop       =   1476
   ClientWidth     =   6060
   HelpContextID   =   112
   Icon            =   "GenPPlt.frx":0000
   LinkTopic       =   "Form1"
   PaletteMode     =   1  'UseZOrder
   ScaleHeight     =   6924
   ScaleWidth      =   6060
   Begin VB.Frame Frame1 
      Caption         =   "Labels"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   7.8
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   975
      Left            =   0
      TabIndex        =   30
      Top             =   5400
      Width           =   6015
      Begin VB.CheckBox chkLabelPoints 
         Caption         =   "&Label Points"
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
         Left            =   3240
         TabIndex        =   33
         Top             =   240
         Width           =   2052
      End
      Begin VB.OptionButton OptRelative 
         Caption         =   "A&bsolute"
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
         Index           =   1
         Left            =   4440
         TabIndex        =   37
         Top             =   480
         Width           =   1095
      End
      Begin VB.OptionButton OptRelative 
         Caption         =   "Relative"
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
         Index           =   0
         Left            =   3240
         TabIndex        =   36
         Top             =   480
         Value           =   -1  'True
         Width           =   1032
      End
      Begin VB.ComboBox ComboLabelData 
         Height          =   315
         Left            =   1320
         Style           =   2  'Dropdown List
         TabIndex        =   32
         Top             =   200
         Width           =   1695
      End
      Begin VB.ComboBox ComboLabelX 
         Height          =   315
         Left            =   1320
         Style           =   2  'Dropdown List
         TabIndex        =   35
         Top             =   520
         Width           =   1695
      End
      Begin VB.Label Label3 
         Caption         =   "&Data Points:"
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
         Left            =   120
         TabIndex        =   31
         Top             =   280
         Width           =   1335
      End
      Begin VB.Label Label2 
         Caption         =   "&X axis:"
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
         Left            =   120
         TabIndex        =   34
         Top             =   600
         Width           =   735
      End
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
      Height          =   372
      Left            =   3360
      TabIndex        =   54
      Top             =   6480
      Width           =   972
   End
   Begin VB.CommandButton cmdPlot 
      Caption         =   "&Plot"
      Enabled         =   0   'False
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
      Left            =   1800
      TabIndex        =   38
      Top             =   6480
      Width           =   972
   End
   Begin VB.Frame fraStage 
      Caption         =   "Stage Specification"
      Enabled         =   0   'False
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   7.8
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   852
      Left            =   0
      TabIndex        =   26
      Top             =   4440
      Width           =   6012
      Begin VB.OptionButton optStage 
         Caption         =   "Inver&t + Stage Values"
         Enabled         =   0   'False
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
         Index           =   1
         Left            =   120
         TabIndex        =   28
         Top             =   480
         Width           =   2292
      End
      Begin VB.OptionButton optStage 
         Caption         =   "Stage Values &Only"
         Enabled         =   0   'False
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
         Index           =   0
         Left            =   120
         TabIndex        =   27
         Top             =   240
         Value           =   -1  'True
         Width           =   2292
      End
      Begin VB.CheckBox chkInvert 
         Caption         =   "Dra&w Invert on Plot"
         Enabled         =   0   'False
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
         Left            =   3240
         TabIndex        =   29
         Top             =   480
         Width           =   2052
      End
   End
   Begin VB.Frame fraTime 
      Caption         =   "Time Specification"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   7.8
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   1815
      Left            =   0
      TabIndex        =   16
      Top             =   2520
      Width           =   6012
      Begin VB.CommandButton cmdTime 
         Caption         =   "&<--"
         Enabled         =   0   'False
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
         Index           =   1
         Left            =   3360
         TabIndex        =   23
         Top             =   1020
         Width           =   372
      End
      Begin VB.CommandButton cmdTime 
         Caption         =   "--&>"
         Enabled         =   0   'False
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
         Index           =   0
         Left            =   3360
         TabIndex        =   22
         Top             =   720
         Width           =   372
      End
      Begin VB.ListBox lstTime 
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   7.8
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   1200
         Index           =   1
         Left            =   3840
         TabIndex        =   25
         Top             =   480
         Width           =   2055
      End
      Begin VB.ListBox lstTime 
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   7.8
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   1200
         Index           =   0
         Left            =   1200
         Sorted          =   -1  'True
         TabIndex        =   21
         Top             =   480
         Width           =   2055
      End
      Begin VB.OptionButton optTime 
         Caption         =   "Pea&ks"
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
         Left            =   120
         TabIndex        =   18
         Top             =   840
         Width           =   1085
      End
      Begin VB.OptionButton optTime 
         Caption         =   "Inter&vals"
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
         Left            =   120
         TabIndex        =   17
         Top             =   480
         Value           =   -1  'True
         Width           =   1085
      End
      Begin VB.OptionButton optTime 
         Caption         =   "&Animate"
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
         Index           =   2
         Left            =   120
         TabIndex        =   19
         Top             =   1200
         Width           =   1085
      End
      Begin VB.Label lblTime 
         Caption         =   "Selected"
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
         Index           =   1
         Left            =   3840
         TabIndex        =   24
         Top             =   240
         Width           =   1452
      End
      Begin VB.Label lblTime 
         Caption         =   "Available"
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
         Index           =   0
         Left            =   1220
         TabIndex        =   20
         Top             =   240
         Width           =   1692
      End
   End
   Begin VB.Frame fraBranch 
      Caption         =   "Branch/Node Specification"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   7.8
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   1815
      Left            =   0
      TabIndex        =   2
      Top             =   600
      Width           =   6012
      Begin VB.ListBox lstBranch 
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   7.8
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   240
         Index           =   1
         Left            =   3000
         TabIndex        =   12
         Top             =   1380
         Width           =   1572
      End
      Begin VB.ListBox lstBranch 
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   7.8
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   240
         Index           =   0
         Left            =   3000
         TabIndex        =   10
         Top             =   600
         Width           =   1572
      End
      Begin VB.ComboBox cboNode 
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   7.8
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   288
         Index           =   1
         Left            =   4680
         Style           =   2  'Dropdown List
         TabIndex        =   15
         Top             =   1380
         Width           =   1212
      End
      Begin VB.ComboBox cboNode 
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   7.8
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   288
         Index           =   0
         ItemData        =   "GenPPlt.frx":0442
         Left            =   4680
         List            =   "GenPPlt.frx":0444
         Style           =   2  'Dropdown List
         TabIndex        =   14
         Top             =   600
         Width           =   1212
      End
      Begin VB.ListBox lstBranch 
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   7.8
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   432
         Index           =   2
         Left            =   3000
         TabIndex        =   11
         Top             =   900
         Width           =   1572
      End
      Begin VB.CommandButton cmdBran 
         Caption         =   "<-- &Remove"
         Enabled         =   0   'False
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
         Index           =   3
         Left            =   1680
         TabIndex        =   5
         Top             =   300
         Width           =   1212
      End
      Begin VB.CommandButton cmdBran 
         Caption         =   "&End -->"
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
         Index           =   1
         Left            =   1680
         TabIndex        =   8
         Top             =   1380
         Width           =   1212
      End
      Begin VB.CommandButton cmdBran 
         Caption         =   "&Intermed -->"
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
         Index           =   2
         Left            =   1680
         TabIndex        =   7
         Top             =   960
         Width           =   1212
      End
      Begin VB.CommandButton cmdBran 
         Caption         =   "&Start -->"
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
         Index           =   0
         Left            =   1680
         TabIndex        =   6
         Top             =   600
         Width           =   1212
      End
      Begin VB.ListBox lstAvBr 
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   7.8
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   1008
         Left            =   120
         TabIndex        =   4
         Top             =   600
         Width           =   1452
      End
      Begin VB.Label Label1 
         Caption         =   "&Node"
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
         Left            =   4680
         TabIndex        =   13
         Top             =   360
         Width           =   1092
      End
      Begin VB.Label lblBran 
         Caption         =   "Branch"
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
         Left            =   3000
         TabIndex        =   9
         Top             =   360
         Width           =   1092
      End
      Begin VB.Label lblAvail 
         Caption         =   "Available"
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
         Left            =   120
         TabIndex        =   3
         Top             =   360
         Width           =   1452
      End
   End
   Begin VB.Frame fraAnimButtons 
      BorderStyle     =   0  'None
      Height          =   375
      Left            =   600
      TabIndex        =   39
      Top             =   6480
      Visible         =   0   'False
      Width           =   2295
      Begin VB.CommandButton cmdAnimate 
         Height          =   300
         Index           =   0
         Left            =   1800
         Picture         =   "GenPPlt.frx":0446
         Style           =   1  'Graphical
         TabIndex        =   53
         Top             =   0
         Width           =   300
      End
      Begin VB.CommandButton cmdAnimate 
         Height          =   300
         Index           =   1
         Left            =   0
         Picture         =   "GenPPlt.frx":0750
         Style           =   1  'Graphical
         TabIndex        =   42
         Top             =   0
         Width           =   300
      End
      Begin VB.CommandButton cmdAnimate 
         Height          =   300
         Index           =   4
         Left            =   1080
         Picture         =   "GenPPlt.frx":0A5A
         Style           =   1  'Graphical
         TabIndex        =   48
         Top             =   0
         Width           =   300
      End
      Begin VB.CommandButton cmdAnimate 
         Height          =   300
         Index           =   5
         Left            =   720
         Picture         =   "GenPPlt.frx":0D64
         Style           =   1  'Graphical
         TabIndex        =   46
         Top             =   0
         Width           =   300
      End
      Begin VB.CommandButton cmdAnimate 
         Height          =   300
         Index           =   3
         Left            =   360
         Picture         =   "GenPPlt.frx":162E
         Style           =   1  'Graphical
         TabIndex        =   44
         Top             =   0
         Width           =   300
      End
      Begin VB.CommandButton cmdAnimate 
         Height          =   300
         Index           =   2
         Left            =   1440
         Picture         =   "GenPPlt.frx":1938
         Style           =   1  'Graphical
         TabIndex        =   50
         Top             =   0
         Width           =   300
      End
      Begin VB.Label lblAnimate 
         Caption         =   "&>"
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
         Index           =   7
         Left            =   1920
         TabIndex        =   52
         Top             =   480
         Width           =   135
      End
      Begin VB.Label lblAnimate 
         Caption         =   "&<"
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
         Left            =   120
         TabIndex        =   40
         Top             =   480
         Width           =   135
      End
      Begin VB.Label lblAnimate 
         Caption         =   "&6"
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
         Index           =   6
         Left            =   1800
         TabIndex        =   51
         Top             =   480
         Width           =   135
      End
      Begin VB.Label lblAnimate 
         Caption         =   "&5"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   7.8
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   495
         Index           =   5
         Left            =   1440
         TabIndex        =   49
         Top             =   480
         Width           =   135
      End
      Begin VB.Label lblAnimate 
         Caption         =   "&4"
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
         Index           =   4
         Left            =   1080
         TabIndex        =   47
         Top             =   480
         Width           =   135
      End
      Begin VB.Label lblAnimate 
         Caption         =   "&3"
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
         Left            =   720
         TabIndex        =   45
         Top             =   480
         Width           =   135
      End
      Begin VB.Label lblAnimate 
         Caption         =   "&2"
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
         Left            =   360
         TabIndex        =   43
         Top             =   480
         Width           =   135
      End
      Begin VB.Label lblAnimate 
         Caption         =   "&1"
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
         Left            =   0
         TabIndex        =   41
         Top             =   480
         Width           =   135
      End
   End
   Begin VB.Label lblCon 
      Caption         =   "Constituent:"
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
      Left            =   120
      TabIndex        =   1
      Top             =   360
      Width           =   5892
   End
   Begin VB.Label lblScen 
      Caption         =   "Scenario(s):"
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
      Left            =   120
      TabIndex        =   0
      Top             =   120
      Width           =   5892
   End
End
Attribute VB_Name = "frmGenProfPlot"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
'Copyright 2000 by AQUA TERRA Consultants

Dim lTs As Collection
Dim lt As Collection
Dim lnsen&, lsen$(), lncon&, lcon$()
'module variables for profile plot parameters
Dim tcnt&, tval$(), peakfg&
Dim stagefg&, invrtfg&, rangefg&

Dim initfg As Boolean, failfg As Boolean

'animation variables
Dim stopfg As Boolean, tsinc&, ssfg&
Dim curDate$

Dim lKeyFld$, lKeyFldType&, lLenFld$

Dim dataLabelAttribute As String

Sub PPAnim()
  Dim lMin&, lMax&
  If Not initfg Then
    On Error GoTo setInitFg
    If IsNull(GenScnEntry.gProfPlot) Then initfg = True
    If Not initfg And Not GenScnEntry.gProfPlot.GraphOpen Then
setInitFg:
      initfg = True
    End If
    On Error GoTo 0
  End If
  If initfg Then
    cmdPlot_Click
    If failfg Then
      failfg = False
      Exit Sub
    End If
  End If
  If tsinc = 1 Then
    lMin = 0
    lMax = lstTime(0).ListCount - 1
  Else
    lMin = 1
    lMax = lstTime(0).ListCount
  End If
  Do
    If lstTime(0).ListIndex >= lMin And _
       lstTime(0).ListIndex < lMax Then
      lstTime(0).ListIndex = lstTime(0).ListIndex + tsinc
      DoEvents
      tval(0) = curDate 'lblDate.Caption
      Call GenScnEntry.DoProfPlot(lt, tcnt, tval(), lnsen, lsen(), lncon, lcon(), peakfg, stagefg, invrtfg, rangefg, 0, 1, ComboLabelX.text, "", dataLabelAttribute)
      DoEvents
      Sleep 100 'It might be nice to have user control of speed. This slows it down to 10 frames per second
    Else
      stopfg = True
    End If
  Loop While Not stopfg And GenScnEntry.gProfPlot.GraphOpen

End Sub

Private Sub cboNode_Click(index As Integer)

  If lstBranch(0).List(0) = lstBranch(1).List(0) Then
    'same start and end branch,
    'make sure ending node after starting node
    If cboNode(0).ListIndex > cboNode(1).ListIndex Then
      'end node same as or earlier than start
      MsgBox "For the same branch, you have specified an ending node which preceeds the starting node." & vbCrLf & "The ending node must follow the starting node", vbExclamation, "GenScn Profile Plot Problem"
      If index = 0 Then
        If cboNode(1).ListIndex > 0 Then
          cboNode(0).ListIndex = cboNode(1).ListIndex - 1
        Else
          cboNode(0).ListIndex = -1
        End If
      Else
        If cboNode(0).ListIndex < cboNode(0).ListCount - 1 Then
          cboNode(1).ListIndex = cboNode(0).ListIndex + 1
        Else
          cboNode(1).ListIndex = -1
        End If
      End If
    End If
  End If
End Sub

Private Sub SelectBranch(index As Integer)
  Dim i As Long, j As Long
  Dim bran As String, CNode As Long
  Dim searchfor As String
  
  Me.MousePointer = vbHourglass
  
  bran = lstAvBr.List(lstAvBr.ListIndex)
  lstBranch(index).AddItem bran
  lstBranch(index).ListIndex = lstBranch(index).ListCount - 1
  For i = 0 To 2
    'unselect items in other lists
    If i <> index Then
      lstBranch(i).ListIndex = -1
    End If
  Next i
  If index < 2 Then
    'add available nodes for start or end branch
    'first find the top node (the one w/out
    'anyone else in the branch pointing to it)
    CNode = -1
    i = 1
    Do While i <= lTs.Count
      If lTs(i).attrib("BRANCH") = bran Then
        'see if anyone points to this node
        j = 1
        searchfor = lTs(i).attrib("NODE")
        Do While j <= lTs.Count
          If lTs(j).attrib("DSNODE") = searchfor Then
            If lTs(j).attrib("BRANCH") = bran Then GoTo FoundUpstream
          End If
          j = j + 1
        Loop
        'no one points to this node, put first in node list
        cboNode(index).AddItem lTs(i).attrib("NODE")
        CNode = i
        i = lTs.Count + 1
FoundUpstream:
      End If
      i = i + 1
    Loop
    If CNode < 0 Then
      'for some reason didn't find top node of branch,
      'use first node encountered in ts buffer
      i = 1
      Do While i <= lTs.Count
        If lTs(i).attrib("BRANCH") = bran Then
          cboNode(index).AddItem lTs(i).attrib("NODE")
          CNode = i
          i = lTs.Count + 1
        End If
        i = i + 1
      Loop
    End If
    Do While CNode >= 0
      i = 1
      searchfor = lTs(CNode).attrib("DSNODE")
      Do While i <= lTs.Count
        If lTs(i).attrib("NODE") = searchfor Then   'right node
          If lTs(i).attrib("BRANCH") = bran Then 'right branch
            'add node to list
            cboNode(index).AddItem lTs(i).attrib("NODE")
            CNode = i
            i = lTs.Count + 1
          End If
        End If
        i = i + 1
      Loop
      If i = lTs.Count + 1 Then 'no downstream node found
        CNode = -1
      End If
    Loop
    If index = 0 Then
      'make 1st node default for start branch
      cboNode(0).ListIndex = 0
    Else
      'make last node default for end branch
      cboNode(1).ListIndex = cboNode(1).ListCount - 1
    End If
  Else 'intermediate branch,
    'remove from available list
    lstAvBr.RemoveItem lstAvBr.ListIndex
  End If
  
  Me.MousePointer = vbDefault

End Sub

Private Sub cmdBran_Click(index As Integer)
  Dim i&, imatch%, bran$

  If index < 3 Then 'adding to start,end,or intermediate
    If lstAvBr.ListIndex >= 0 Then
      bran = lstAvBr.List(lstAvBr.ListIndex)
      If index = 0 And lstBranch(0).ListCount > 0 Then
        MsgBox "The Starting Branch has already been specified." & vbCrLf & _
               "To specify a different Starting Branch, remove the existing one with the Remove button.", _
               vbExclamation, "GenScn Profile Plot Problem"
      ElseIf index = 1 And lstBranch(1).ListCount > 0 Then
        MsgBox "The Ending Branch has already been specified." & vbCrLf & _
               "To specify a different Ending Branch, remove the existing one with the Remove button.", _
               vbExclamation, "GenScn Profile Plot Problem"
      ElseIf index = 2 And lstBranch(0).List(0) = bran Then
        MsgBox "This branch has already been selected as the starting branch." & vbCrLf & _
               "It may not be selected as an intermediate branch.", _
               vbExclamation, "GenScn Profile Plot Problem"
      ElseIf index = 2 And lstBranch(1).List(0) = bran Then
        MsgBox "This branch has already been selected as the ending branch." & vbCrLf & _
               "It may not be selected as an intermediate branch.", _
               vbExclamation, "GenScn Profile Plot Problem"
      Else 'ok to add branch to specified list
        SelectBranch index
      End If
    Else
      MsgBox "No Branch has been selected from the Available list to add to the specified Branch List.", vbExclamation, "GenScn Profile Plot Problem"
    End If
  Else 'remove item from one of the branch lists
    If lstBranch(0).ListIndex >= 0 Then
      imatch = 0
      For i = 0 To lstAvBr.ListCount - 1
        If lstAvBr.List(i) = lstBranch(0).List(0) Then
          imatch = 1
        End If
      Next i
      If imatch = 0 Then 'not already in list. add to available
        lstAvBr.AddItem lstBranch(0).List(0)
      End If
      lstBranch(0).clear
      cboNode(0).clear
    End If
    If lstBranch(1).ListIndex >= 0 Then
      imatch = 0
      For i = 0 To lstAvBr.ListCount - 1
        If lstAvBr.List(i) = lstBranch(1).List(0) Then
          imatch = 1
        End If
      Next i
      If imatch = 0 Then 'not already in list. add to available
        lstAvBr.AddItem lstBranch(1).List(0)
      End If
      lstBranch(1).clear
      cboNode(1).clear
    End If
    If lstBranch(2).ListIndex >= 0 Then
      lstAvBr.AddItem lstBranch(2).List(lstBranch(2).ListIndex)
      lstBranch(2).RemoveItem lstBranch(2).ListIndex
    End If
    If lstBranch(0).ListIndex < 0 And _
       lstBranch(1).ListIndex < 0 And _
       lstBranch(2).ListIndex < 0 Then
      cmdBran(3).Enabled = False
    End If
  End If
  Call CheckPlotEnabled
End Sub

Private Sub cmdCancel_Click()

    Unload frmGenProfPlot
End Sub

Private Sub cmdPlot_Click()
  Dim i&, j&, k&, l&, ic&
  Dim adfg%, cnctfg%, relativefg%
  Dim lMyRchAvail As Boolean
  
  Me.MousePointer = vbHourglass
  
  If chkLabelPoints.value = vbChecked Then
    dataLabelAttribute = ComboLabelData.text
  Else
    dataLabelAttribute = ""
  End If
  
  'put needed timeseries into global ts buffer
  If MyRchAvail Then
    'look through local array to confirm
    'valid connectivity, assume negative
    lMyRchAvail = False
    For i = 1 To lTs.Count
      If Len(lTs(i).attrib("DSNODE")) > 0 Then
        'connectivity data exists
        lMyRchAvail = True
        Exit For
      End If
    Next i
  Else
    lMyRchAvail = MyRchAvail
  End If
  
  failfg = False
  Set lt = Nothing
  Set lt = New Collection
  For j = 0 To lnsen - 1
    For ic = 0 To lncon - 1
      'find start of timeseries to use
      adfg = 0
      i = 1
      While i <= lTs.Count
        If lTs(i).attrib("BRANCH") = lstBranch(0).List(0) And _
           lTs(i).Header.sen = lsen(j) And lTs(i).Header.con = lcon(ic) Then
          If lTs(i).attrib("NODE") = cboNode(0).text Or adfg = 1 Then
            'first node or beyond, add to main ts buffer
            lt.Add lTs(i)
            If lMyRchAvail Then 'reach info exists,
              'find next downstream node
              k = 1
              While k <= lTs.Count
                If lTs(k).attrib("BRANCH") = lTs(i).attrib("BRANCH") And _
                   lTs(k).Header.sen = lsen(j) And _
                   lTs(k).Header.con = lcon(ic) And _
                   lTs(k).attrib("NODE") = lTs(i).attrib("DSNODE") Then
                  'downstream node found
                  i = k - 1
                  k = lTs.Count + 1
                  adfg = 1
                End If
                k = k + 1
              Wend
              If k = lTs.Count + 1 Then 'no more downstream nodes
                i = lTs.Count
              End If
            Else 'assume remaining nodes in order
              adfg = 1
            End If
          End If
        End If
        i = i + 1
      Wend
      If lt.Count > 0 Then  'timeseries found from start branch
        If Not lMyRchAvail Or lt(lt.Count).attrib("DSNODE") <> "" Then
          'no reach info or downstream node
          'exists from last node in starting branch
          'add any intermediate branches
          For k = 0 To lstBranch(2).ListCount - 1
            cnctfg = 0
            i = 1
            While i <= lTs.Count
              If lTs(i).attrib("BRANCH") = lstBranch(2).List(k) And _
                 lTs(i).Header.sen = lsen(j) And lTs(i).Header.con = lcon(ic) Then
                'matching branch, scenario, and constituent
                'if reach info exists, need to match downstream
                'node from last node of starting branch
                If Not lMyRchAvail Or _
                   lTs(i).attrib("NODE") = lt(lt.Count).attrib("DSNODE") Then
                  'add to ts buffer
                  lt.Add lTs(i)
                  If lMyRchAvail Then
                    'look through all available timeseries
                    'for next downstream node
                    i = 0
                  End If
                  cnctfg = 1 'branches connect ok
                End If
              End If
              i = i + 1
            Wend
            If lMyRchAvail And cnctfg = 0 Then
              'branches not connected
              failfg = True
              If k = 0 Then
                MsgBox "Unable to find connecting node between starting branch and first intermediate branch.", vbExclamation, "GenScn Profile Plot Problem"
              Else
                MsgBox "Unable to find connecting node between specified intermediate branches.", vbExclamation, "GenScn Profile Plot Problem"
              End If
            End If
          Next k
          If lstBranch(0).List(0) <> lstBranch(1).List(0) Then
            'different start/end branch, add timeseries up to ending node
            cnctfg = 0
            i = 1
            While i <= lTs.Count
              If lTs(i).attrib("BRANCH") = lstBranch(1).List(0) And _
                 lTs(i).Header.sen = lsen(j) And lTs(i).Header.con = lcon(ic) Then
                'matching branch, scenario, and constituent
                'if reach info exists, need to match downstream
                'node from last node of previous branch
                If Not lMyRchAvail Or _
                   lTs(i).attrib("NODE") = lt(lt.Count).attrib("DSNODE") Then
                  lt.Add lTs(i)
                  If lTs(i).attrib("NODE") = cboNode(1).text Then
                    'last node, don't add after this
                    i = lTs.Count + 1
                  ElseIf lMyRchAvail Then
                    'look through all available timeseries
                    'for next downstream node
                    i = 0
                  End If
                  cnctfg = 1
                End If
              End If
              i = i + 1
            Wend
            If lMyRchAvail And cnctfg = 0 Then
              failfg = True
              MsgBox "Unable to find connecting node between starting branch and ending branch.", vbExclamation, "GenScn Profile Plot Problem"
            End If
          End If
        ElseIf lstBranch(2).ListCount > 0 Then
          failfg = True
          MsgBox "The last node of the Starting branch has no downstream node," & vbCrLf & "therefore it can not be connected to the specified intermediate branch(es).", vbExclamation, "GenScn Profile Plot Problem"
        ElseIf lstBranch(0).List(0) <> lstBranch(1).List(0) Then
          failfg = True
          MsgBox "The last node of the Starting branch has no downstream node," & vbCrLf & "therefore it can not be connected to the specified ending branch.", vbExclamation, "GenScn Profile Plot Problem"
        End If
        If lstBranch(0).List(0) = lstBranch(1).List(0) Then
          'same start and end branch,
          'may need to remove some timeseries
          i = lt.Count + 1
          While i > 0
            i = i - 1
            If lTs(i).Header.sen = lsen(j) And lTs(i).Header.con = lcon(ic) Then
              If lt(i).attrib("NODE") = cboNode(1).text Then
                'don't remove any more
                i = 0
              Else 'keep removing
                lt.Remove (lt.Count)
              End If
            End If
          Wend
        End If
      Else 'no timeseries found in start branch
        failfg = True
        MsgBox "No time series were found for the starting branch and node specified for " & vbCrLf & "Scenario: " & lsen(j) & ", Constituent: " & lcon(ic) & vbCrLf & "Therefore, no plot can be made for the specified profile.", vbExclamation, "GenScn Profile Plot Problem"
      End If
    Next ic
  Next j
  If Not failfg Then
    'fill timseries arrays
    Call GenScnEntry.FillTimSer(lt)
    'determine profile plot parameters
    peakfg = 0
    rangefg = 0
    initfg = False
    If optTime(0).value = True Then
      tcnt = lstTime(1).ListCount
      ReDim tval(tcnt - 1)
      For i = 0 To tcnt - 1
        tval(i) = lstTime(1).List(i)
      Next i
    Else
      tcnt = 1
      ReDim tval(0)
      If optTime(1).value = True Then 'peaks
        tval(0) = ""
        peakfg = 1
      Else 'animation, use label caption for date
        tval(0) = curDate
        'use whole period for min/max range
        rangefg = 1
      End If
    End If
    If optStage(1).value = True Then
      stagefg = 1
    Else
      stagefg = 0
    End If
    If chkInvert.value = True Then
      invrtfg = 1
    Else
      invrtfg = 0
    End If
      
'    If lMyRchAvail And (ComboLabelData.text <> lKeyFld Or ComboLabelX.text <> lLenFld) Then
'      Dim Filename$, s$, fldnum%, cdist#
'      Dim ShpDB As Database, myRec As Recordset, fld&
'      Filename = MyRch(0).Filename
'      s = GetDatabase(Filename)
'      Set ShpDB = OpenDatabase(s, False, False, "DBASE IV")
'      s = FilenameOnly(Filename) & ".dbf"
'      Set myRec = ShpDB.OpenRecordset(s, dbOpenDynaset)
'
'      If ComboLabelX.text <> lLenFld Then 'change length entries
'        fldnum = ComboLabelX.ItemData(ComboLabelX.ListIndex)
'        cdist = 0
'        For i = 1 To lt.Count
'          If lKeyFldType = dbText Then
'            s = lKeyFld & " = '" & lt(i).Header.loc & "'"
'          Else
'            s = lKeyFld & " = " & lt(i).Header.loc
'          End If
'          myRec.FindFirst s
'          If Not myRec.NoMatch Then
'            If Not IsNull(myRec.Fields(fldnum).value) Then lt(i).Dist = myRec.Fields(fldnum).value
'          End If
'        Next i
'      End If
      
'      If ComboLabelData.text <> lKeyFld Then 'change data labels
'        fldnum = ComboLabelData.ItemData(ComboLabelData.ListIndex)
'        For i = 1 To lt.Count
'          myRec.FindFirst lKeyFld & " = '" & lt(i).Header.loc & "'"
'          If Not myRec.NoMatch Then
'            If Not IsNull(myRec.Fields(fldnum).value) Then lt(i).Header.loc = myRec.Fields(fldnum).value
'          End If
'        Next i
'      End If
'
'      ShpDB.Close
'    End If

'    If chkLabelPoints.value = False Then 'Remove labels
'      For i = 1 To lt.Count
'        lt(i).Header.loc = ""
'      Next i
'    End If
      
    If OptRelative(0).value Then relativefg = 1 Else relativefg = 0
    'generate profile plot
    Call GenScnEntry.DoProfPlot(lt, tcnt, tval(), lnsen, lsen(), lncon, lcon(), peakfg, stagefg, invrtfg, rangefg, 1, relativefg, ComboLabelX.text, "", dataLabelAttribute)
  End If
  Me.MousePointer = vbDefault
End Sub

Private Sub cmdTime_Click(index As Integer)
  Dim lind%

  If lstTime(index).ListIndex >= 0 Then
    lind = (index + 1) Mod 2
    lstTime(lind).AddItem lstTime(index).List(lstTime(index).ListIndex)
    If index = 0 Then 'save time interval index
      lstTime(lind).ItemData(lstTime(lind).ListCount - 1) = lstTime(index).ListIndex
    End If
    lstTime(index).RemoveItem lstTime(index).ListIndex
    cmdTime(index).Enabled = False
    Call CheckPlotEnabled
  Else
    MsgBox "No item selected from the " & lblTime(index).Caption & " list to move to the other list.", vbExclamation, "GenScn Profile Plot Problem"
  End If
End Sub

Private Sub PopulateFieldCombos()
  Dim Attr As Variant
  Dim DefaultX As Integer
  Dim DefaultData As Integer
  If lTs.Count > 0 Then
    ComboLabelX.clear
    ComboLabelData.clear
    For Each Attr In lTs(1).Attribs
      ComboLabelX.AddItem Attr.Name
      ComboLabelData.AddItem Attr.Name
      If UCase(Attr.Name) = "DISTANCE" Then DefaultX = ComboLabelX.newIndex
      If UCase(Attr.Name) = "NODE" Then DefaultData = ComboLabelX.newIndex
    Next
    ComboLabelX.ListIndex = DefaultX
    ComboLabelData.ListIndex = DefaultData
  End If
'  Dim Filename$, s$, fldname$, i&
'  Dim ShpDB As Database, myRec As Recordset, fld&
'
'  If MyRchAvail Then
'    Filename = MyRch(0).Filename
'
'    lKeyFld = ""
'    lLenFld = ""
'    For i = 0 To frmGenScn.Map1.LayerCount - 1
'      If FilenameOnly(Filename) = frmGenScn.Map1.LayerFilename(i) Then
'        lKeyFld = frmGenScn.Map1.LayerKeyField(i)
'        lLenFld = frmGenScn.Map1.LayerLengthField(i)
'        Exit For
'      End If
'    Next i
'
'    s = GetDatabase(Filename)
'    Set ShpDB = OpenDatabase(s, False, False, "DBASE IV")
'    s = FilenameOnly(Filename) & ".dbf"
'    Set myRec = ShpDB.OpenRecordset(s, dbOpenDynaset)
'    For fld = 0 To myRec.Fields.Count - 1
'      fldname = myRec.Fields(fld).Name
'      Select Case myRec.Fields(fld).Type
'      Case dbBigInt, dbByte, dbDate, dbCurrency, dbDecimal, dbDouble, dbFloat, dbInteger, dbLong
'        ComboLabelX.AddItem fldname
'        ComboLabelX.ItemData(ComboLabelX.newIndex) = fld
'      End Select
'      ComboLabelData.AddItem fldname
'      ComboLabelData.ItemData(ComboLabelData.newIndex) = fld
'      If fldname = lKeyFld Then
'        lKeyFldType = myRec.Fields(fld).Type
'      End If
'    Next fld
'    ShpDB.Close
'    On Error Resume Next 'Set defaults if they are there, but don't die if they are not
'    ComboLabelData.ListIndex = 0
'    ComboLabelX.ListIndex = 0
'    ComboLabelData.text = lKeyFld
'    ComboLabelX.text = lLenFld
'  End If
End Sub

Private Sub DefaultBranchSelection()
  'lstBranch(0).List(0) = upstream branch ID
  'lstBranch(1).List(0) = downstream branch ID
  'cboNode(0).text = upstream node ID
  'cboNode(1).text = downstream node ID
  Dim NotTop() As Boolean
  Dim OnStream() As Boolean
  Dim SelBranch() As String
  Dim SelDS() As String
  Dim SelLoc As String
  Dim SelLocIndex As Long
  Dim OtherLocIndex As Long
  Dim tsIndex As Long
  Dim TopNode As Long
  Dim BottomNode As Long
  Dim FoundBottom As Boolean
  Dim FoundTop As Boolean
  
  If scntLoc >= 0 Then
    ReDim SelBranch(scntLoc)
    ReDim SelDS(scntLoc)
    ReDim NotTop(scntLoc)
    ReDim OnStream(scntLoc)
    For SelLocIndex = 0 To scntLoc
      SelLoc = specLoc(SelLocIndex)
      For tsIndex = 1 To lTs.Count
        If lTs(tsIndex).attrib("NODE") = SelLoc Then
          'Set DebugTS = lTs(tsIndex)
          SelBranch(SelLocIndex) = lTs(tsIndex).attrib("BRANCH")
          SelDS(SelLocIndex) = lTs(tsIndex).attrib("DSNODE")
          GoTo FoundInfo
        End If
      Next
      Exit Sub 'No connectivity info for this selected node, give up
FoundInfo:
    Next
    
    For SelLocIndex = 0 To scntLoc
      SelLoc = specLoc(SelLocIndex)
      For OtherLocIndex = 0 To scntLoc
        If SelDS(OtherLocIndex) = SelLoc Then
          NotTop(SelLocIndex) = True
          Exit For
        End If
      Next
    Next
    For SelLocIndex = 0 To scntLoc
      If Not NotTop(SelLocIndex) Then
        If FoundTop Then Exit Sub 'Found more than one top node, give up
        FoundTop = True
        TopNode = SelLocIndex
      End If
    Next
    
    FoundBottom = False
    BottomNode = TopNode
    While Not FoundBottom
      OnStream(BottomNode) = True
      SelLoc = SelDS(BottomNode)
      For SelLocIndex = 0 To scntLoc
        If specLoc(SelLocIndex) = SelLoc Then
          BottomNode = SelLocIndex
          GoTo FoundNextDS
        End If
      Next
      FoundBottom = True
FoundNextDS:
    Wend
    
    For SelLocIndex = 0 To scntLoc
      If Not OnStream(SelLocIndex) Then Exit Sub 'Not contiguous from top to bottom, give up
    Next
    
    For SelLocIndex = 1 To lstAvBr.ListCount
      If lstAvBr.List(SelLocIndex) = SelBranch(TopNode) Then
        lstAvBr.ListIndex = SelLocIndex
        SelectBranch 0
        Exit For
      End If
    Next
    'cboNode(0).text = specLoc(TopNode)
    For SelLocIndex = 0 To cboNode(0).ListCount
      If cboNode(0).List(SelLocIndex) = specLoc(TopNode) Then
        cboNode(0).ListIndex = SelLocIndex
        Exit For
      End If
    Next
    
    For SelLocIndex = 1 To lstAvBr.ListCount
      If lstAvBr.List(SelLocIndex) = SelBranch(BottomNode) Then
        lstAvBr.ListIndex = SelLocIndex
        SelectBranch 1
        Exit For
      End If
    Next
    'cboNode(1).text = specLoc(BottomNode)
    For SelLocIndex = 0 To cboNode(1).ListCount
      If cboNode(1).List(SelLocIndex) = specLoc(BottomNode) Then
        cboNode(1).ListIndex = SelLocIndex
        Exit For
      End If
    Next
  End If

End Sub

Private Sub Form_Load()
  Dim i&, j&, k&, l&, ic&, tfg%, iwarn%
  Dim Strt&(), Stp&(), TUnits&(), TSTEP&()
  Dim nint&, retcod&, nbran&, bran$(), branfg&()
  Dim ldt&(5), lstr$, sdtstr$, edtstr$, lBran$
  Dim ctmp As Collection
  Dim lsdat&(6), lEdat&(6)

  Set lTs = Nothing
  Set lTs = New Collection
  iwarn = 0
  
  lnsen = frmGenScn.lstSLC(0).SelCount
  If lnsen = 1 Then
    lblScen.Caption = "Scenario: "
  Else
    lblScen.Caption = "Scenarios: "
  End If
  j = 0
  ReDim lsen(lnsen - 1)
  For i = 0 To frmGenScn.lstSLC(0).ListCount - 1
    If frmGenScn.lstSLC(0).Selected(i) Then
      lsen(j) = frmGenScn.lstSLC(0).List(i)
      If j > 0 Then lblScen.Caption = lblScen.Caption & ", "
      lblScen.Caption = lblScen.Caption & lsen(j)
      j = j + 1
    End If
  Next i
  
  lncon = frmGenScn.lstSLC(2).SelCount
  If lncon = 1 Then
    lblCon.Caption = "Constituent: "
  Else
    lblCon.Caption = "Constituents: "
  End If
  j = 0
  ReDim lcon(lncon - 1)
  For i = 0 To frmGenScn.lstSLC(2).ListCount - 1
    If frmGenScn.lstSLC(2).Selected(i) Then
      lcon(j) = frmGenScn.lstSLC(2).List(i)
      If j > 0 Then lblCon.Caption = lblCon.Caption & ", "
      lblCon.Caption = lblCon.Caption & lcon(j)
      j = j + 1
    End If
  Next i
  
  For i = 0 To lncon - 1
    If lcon(i) = "WSELEV" Or lcon(i) = "STAGE" Or _
       lcon(i) = "ELEV" Then
      fraStage.Enabled = True
      optStage(0).Enabled = True
      optStage(1).Enabled = True
      chkInvert.Enabled = True
    End If
  Next i
  
  i = 0
  Do While i < lnsen ' And iwarn = 0
    ic = 0
    Do While ic < lncon ' And iwarn = 0
      Call GenScnEntry.FindTimSer(lsen(i), "", lcon(ic), lt)
      If lt.Count > 0 Then
        'Call GenScnEntry.FillReachDetails(lt)
        j = 1
        Do While j <= lt.Count
          lBran = lt(j).attrib("BRANCH")
          If Len(lBran) > 0 Then
            If Not InList(lBran, lstAvBr) Then 'branch not found in list
              If i = 0 And ic = 0 Then
                'first time through, add to branch list
                lstAvBr.AddItem lBran
                ReDim Preserve bran(nbran)
                bran(nbran) = lBran
                nbran = nbran + 1
              Else 'scenarios don't have matching branches
                If iwarn = 0 Then MsgBox "Selected Scenarios and/or Constituents do not have matching branches." & vbCrLf & "Only branches common to selected Scenarios and Constituents will be available.", vbExclamation, "GenScn Profile Plot Warning"
                iwarn = 1
              End If
            ElseIf i > 0 Or ic > 0 Then 'found an existing branch
              k = 0
              Do While k < lstAvBr.ListCount
                If lBran = lstAvBr.List(k) Then
                  branfg(k) = 1
                  k = lstAvBr.ListCount
                End If
                k = k + 1
              Loop
            End If
            'add this scenario's timseries to local buffer
            lTs.Add lt(j)
          Else 'no shape file info for this time series, remove it from buffer
            lt.Remove j
          End If
          If j = lt.Count Then
            'update count of timseries in local buffer
            ReDim TUnits(lTs.Count)
            ReDim TSTEP(lTs.Count)
            ReDim Strt(6 * lTs.Count)
            ReDim Stp(6 * lTs.Count)

            For k = 1 To lTs.Count
              Call J2Date(lTs(k).dates.Summary.SJDay, lsdat)
              Call J2Date(lTs(k).dates.Summary.EJDay, lEdat)
              For l = 0 To 5
                Strt((k - 1) * 6 + l) = lsdat(l)
                Stp((k - 1) * 6 + l) = lEdat(l)
              Next l
              TSTEP(k - 1) = lTs(k).dates.Summary.ts
              TUnits(k - 1) = lTs(k).dates.Summary.Tu
            Next k
            'find common period for timeseries
            Call F90_DTMCMN(lTs.Count, Strt(0), Stp(0), TSTEP(0), TUnits(0), CSDat(0), CEDat(0), TSTEP(0), TUnits(0), retcod)
            sdtstr = CSDat(0) & "/" & Format$(CSDat(1), "00") & "/" & Format$(CSDat(2), "00") & " " & Format$(CSDat(3), "00") & ":" & Format$(CSDat(4), "00") & ":" & Format$(CSDat(5), "00")
            edtstr = CEDat(0) & "/" & Format$(CEDat(1), "00") & "/" & Format$(CEDat(2), "00") & " " & Format$(CEDat(3), "00") & ":" & Format$(CEDat(4), "00") & ":" & Format$(CEDat(5), "00")
            If lstTime(0).ListCount > 0 Then
              While lstTime(0).List(0) < sdtstr
                'this interval before start of common period
                lstTime(0).RemoveItem 0
              Wend
              While lstTime(0).List(lstTime(0).ListCount - 1) > edtstr
                lstTime(0).RemoveItem lstTime(0).ListCount - 1
              Wend
            End If
            'add scenario's time intervals
            'If lt(j).Type = "WDM" Then
            '  Call timdif(CSDat(0), CEDat(0), TUnits(0), TStep(0), nint)
            'Else
              nint = lt(j).dates.Summary.NVALS
            'End If
            l = 0
            For k = 1 To nint
              'If lt(j).Type = "WDM" Then
              '  Call F90_TIMADD(CSDat(0), TUnits(0), TStep(0), k, ldt(0))
              'ElseIf lt(j).Type = "FEO" Then
              Call J2Date(lt(j).dates.value(k), ldt())
              'Else
              '  Call F90_TIMADD(CSDat(0), TUnits(0), TStep(0), k, ldt(0))
              'End If
              lstr = ldt(0) & "/" & Format$(ldt(1), "00") & "/" & Format$(ldt(2), "00") & " " & Format$(ldt(3), "00") & ":" & Format$(ldt(4), "00") & ":" & Format$(ldt(5), "00")
              If i > 0 Or ic > 0 Then 'ensuing scenarios or constituents,
                'look through existing time intervals
                tfg = 0
                While tfg = 0
                  If lstr <= lstTime(0).List(l) Or _
                     l = lstTime(0).ListCount - 1 Then
                    tfg = 1
                  Else
                    l = l + 1
                  End If
                Wend
                If lstr <> lstTime(0).List(l) Then
                  'new interval, add to list
                  lstTime(0).AddItem lstr
                End If
              Else '1st scenario, add all time intervals
                lstTime(0).AddItem lstr
              End If
            Next k
          End If
          j = j + 1
        Loop
        If i > 0 Or ic > 0 Then 'see if all previous branches found for this Scen/Con combo
          k = 0
          Do While k < nbran
            If branfg(k) < 1 Then 'this one was not found
              lstAvBr.RemoveItem k
              nbran = nbran - 1
              For l = k To nbran - 1
                bran(l) = bran(l + 1)
                branfg(l) = branfg(l + 1)
              Next l
              If iwarn = 0 Then MsgBox "Selected Scenarios and/or Constituents do not have matching branches." & vbCrLf & "Only branches common to selected Scenarios and Constituents will be available.", vbExclamation, "GenScn Profile Plot Warning"
              iwarn = 1
            Else
              k = k + 1
            End If
          Loop
        End If
        If nbran > 0 Then ReDim branfg(nbran - 1)
      Else
        MsgBox "No time series to plot for Scenario: " & lsen(i) & " and Constituent: " & lcon(ic), vbExclamation, "GenScn Profile Plot Warning"
      End If
      ic = ic + 1
    Loop
    i = i + 1
  Loop

  If failfg Then 'can't do plot, clear lists
    lstAvBr.clear
    lstTime(0).clear
    Unload Me
  Else
    DefaultBranchSelection
    If lstAvBr.ListCount > 0 And lstAvBr.ListIndex < 0 Then lstAvBr.ListIndex = 0
    PopulateFieldCombos
  End If
End Sub

Private Sub lstBranch_Click(index As Integer)

  If lstBranch(0).ListIndex < 0 And _
     lstBranch(1).ListIndex < 0 And _
     lstBranch(2).ListIndex < 0 Then
    cmdBran(3).Enabled = False
  Else
    cmdBran(3).Enabled = True
  End If
End Sub

Private Sub lstBranch_GotFocus(index As Integer)

  If lstBranch(index).ListIndex >= 0 Then
    lstBranch(index).ListIndex = -1
  End If
End Sub

Private Sub lstTime_Click(index As Integer)

  If index = 0 And optTime(2).value = True Then
    'display selected date in date field for
    'time step option
    If lstTime(0).ListIndex >= 0 Then
      curDate = lstTime(0).List(lstTime(0).ListIndex) ' lblDate.Caption =
    Else
      curDate = "" ' lblDate.Caption = ""
    End If
    Call CheckPlotEnabled
  ElseIf optTime(0).value = True Then
    cmdTime(index).Enabled = True
  End If
End Sub

Private Sub optTime_Click(index As Integer)
  Dim i%
  
  If index = 0 Then 'selected time intervals
    For i = 0 To 1
      lstTime(i).Enabled = True
      lstTime(i).BackColor = vbWhite
      lblTime(i).Enabled = True
      If lstTime(i).ListIndex >= 0 Then
        cmdTime(i).Enabled = True
      End If
    Next i
    lblTime(0).Caption = "Available"
  Else 'disable time lists and buttons
    For i = 0 To 1
      lstTime(i).Enabled = False
      lstTime(i).BackColor = vbButtonFace
      lblTime(i).Enabled = False
      cmdTime(i).Enabled = False
    Next i
  End If
  If index = 2 Then 'time step through data
    lblTime(0).Enabled = True
    lblTime(0).Caption = "Start Date"
    lstTime(0).Enabled = True
    lstTime(0).BackColor = vbWhite
    fraAnimButtons.Visible = True
    cmdPlot.Visible = False
  Else
    fraAnimButtons.Visible = False
    cmdPlot.Visible = True
  End If
  Call CheckPlotEnabled
End Sub

Private Sub CheckPlotEnabled()

  If cboNode(0).text <> "" And _
     cboNode(1).text <> "" And _
     ((optTime(0).value = True And lstTime(1).ListCount > 0) Or _
      optTime(1).value = True Or _
      (optTime(2).value = True And curDate <> "")) Then  ' lblDate.Caption
    cmdPlot.Enabled = True
  Else
    cmdPlot.Enabled = False
  End If
End Sub

Private Sub cmdAnimate_Click(index As Integer)
  Dim i&

  If lstTime(0).ListIndex < 0 Then lstTime(0).ListIndex = 0
  
  If index = 0 Then 'run forward in time
    tsinc = 1
    stopfg = False
    PPAnim
  ElseIf index = 1 Then 'run backward in time
    tsinc = -1
    stopfg = False
    PPAnim
  ElseIf index = 2 Then 'forward 1 step
    tsinc = 1
    stopfg = True
    PPAnim
  ElseIf index = 3 Then 'back 1 step
    tsinc = -1
    stopfg = True
    PPAnim
  ElseIf index = 4 Then 'pause
    stopfg = True
  ElseIf index = 5 Then 'stop
    stopfg = True
    lstTime(0).ListIndex = -1
    'disable all animation buttons
    'For i = 0 To 5
    '  cmdAnimate(i).Enabled = False
    'Next i
  End If
End Sub
