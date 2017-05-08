VERSION 5.00
Begin VB.UserControl ctlGlobalBlkEdit 
   ClientHeight    =   1830
   ClientLeft      =   0
   ClientTop       =   0
   ClientWidth     =   8310
   ScaleHeight     =   1830
   ScaleWidth      =   8310
   Begin VB.PictureBox picGlobal 
      BorderStyle     =   0  'None
      Height          =   1215
      Left            =   120
      ScaleHeight     =   1215
      ScaleWidth      =   8055
      TabIndex        =   2
      Top             =   480
      Width           =   8055
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
         Index           =   4
         Left            =   3240
         TabIndex        =   16
         Text            =   "??"
         Top             =   720
         Width           =   492
      End
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
         Index           =   4
         Left            =   3240
         TabIndex        =   15
         Text            =   "??"
         Top             =   360
         Width           =   492
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
         Index           =   3
         Left            =   2640
         TabIndex        =   14
         Text            =   "??"
         Top             =   720
         Width           =   492
      End
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
         Index           =   3
         Left            =   2640
         TabIndex        =   13
         Text            =   "??"
         Top             =   360
         Width           =   492
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
         Index           =   2
         Left            =   2040
         TabIndex        =   12
         Text            =   "??"
         Top             =   720
         Width           =   492
      End
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
         Index           =   2
         Left            =   2040
         TabIndex        =   11
         Text            =   "??"
         Top             =   360
         Width           =   492
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
         Index           =   1
         Left            =   1440
         TabIndex        =   10
         Text            =   "??"
         Top             =   720
         Width           =   492
      End
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
         Index           =   1
         Left            =   1440
         TabIndex        =   9
         Text            =   "??"
         Top             =   360
         Width           =   492
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
         TabIndex        =   8
         Top             =   360
         WhatsThisHelpID =   29
         Width           =   972
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
         TabIndex        =   7
         Top             =   720
         WhatsThisHelpID =   28
         Width           =   852
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
         TabIndex        =   6
         Top             =   360
         WhatsThisHelpID =   27
         Width           =   852
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
         TabIndex        =   5
         Top             =   720
         WhatsThisHelpID =   30
         Width           =   975
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
         Index           =   0
         Left            =   720
         TabIndex        =   4
         Text            =   "????"
         Top             =   720
         WhatsThisHelpID =   26
         Width           =   612
      End
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
         Index           =   0
         Left            =   720
         TabIndex        =   3
         Text            =   "????"
         Top             =   360
         WhatsThisHelpID =   25
         Width           =   612
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
         TabIndex        =   29
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
         TabIndex        =   28
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
         TabIndex        =   27
         Top             =   120
         Width           =   492
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
         TabIndex        =   26
         Top             =   120
         Width           =   492
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
         TabIndex        =   25
         Top             =   120
         Width           =   612
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
         TabIndex        =   24
         Top             =   0
         Width           =   612
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
         TabIndex        =   23
         Top             =   0
         Width           =   1212
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
         TabIndex        =   22
         Top             =   720
         WhatsThisHelpID =   30
         Width           =   492
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
         TabIndex        =   21
         Top             =   360
         WhatsThisHelpID =   29
         Width           =   852
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
         TabIndex        =   20
         Top             =   720
         WhatsThisHelpID =   28
         Width           =   852
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
         TabIndex        =   19
         Top             =   360
         WhatsThisHelpID =   27
         Width           =   972
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
         TabIndex        =   18
         Top             =   720
         WhatsThisHelpID =   26
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
         TabIndex        =   17
         Top             =   360
         WhatsThisHelpID =   25
         Width           =   492
      End
   End
   Begin VB.TextBox txtRunInf 
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   285
      Left            =   1680
      TabIndex        =   0
      Text            =   "Run Information"
      Top             =   120
      Width           =   6495
   End
   Begin VB.Label lblRunInf 
      Caption         =   "Run Information:"
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
      TabIndex        =   1
      Top             =   120
      Width           =   1455
   End
End
Attribute VB_Name = "ctlGlobalBlkEdit"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = False
Option Explicit
'Copyright 2002 AQUA TERRA Consultants - Royalty-free use permitted under open source license
Private pGlobalBlk As HspfGlobalBlk
Event Change()
Private pFrm As Form

Public Property Set frm(newFrm As Form)
  Set pFrm = newFrm
End Property

Public Property Get Owner() As HspfGlobalBlk
  Set Owner = pGlobalBlk
End Property
Public Property Set Owner(newGlobalBlk As HspfGlobalBlk)
  Dim i&
  
  Set pGlobalBlk = newGlobalBlk
  
  With pGlobalBlk
    txtRunInf = .RunInf
    For i = 0 To 4
      txtStart(i) = .SDate(i)
      txtEnd(i) = .EDate(i)
    Next i
    comboOutput.ListIndex = .Outlev
    comboSpecial.ListIndex = .Spout
    comboRunflag.ListIndex = .Runfg
    comboUnits.ListIndex = .Emfg - 1
  End With

End Property

Public Sub Help()
  'just do contents for now
  HtmlHelp pFrm.hwnd, App.HelpFile, HH_DISPLAY_TOC, 0
End Sub

Public Sub Save()
  Dim i&
  
  pGlobalBlk.RunInf.Value = txtRunInf
  For i = 0 To 4
    pGlobalBlk.EDate(i) = txtEnd(i)
    pGlobalBlk.SDate(i) = txtStart(i)
  Next i
  pGlobalBlk.Runfg = comboRunflag.ListIndex
  pGlobalBlk.Spout = comboSpecial.ListIndex
  pGlobalBlk.Outlev.Value = comboOutput.ListIndex
  pGlobalBlk.Emfg = comboUnits.ListIndex + 1
End Sub

Private Sub comboOutput_Click()
  RaiseEvent Change
End Sub

Private Sub comboRunflag_Click()
  RaiseEvent Change
End Sub

Private Sub comboSpecial_Click()
  RaiseEvent Change
End Sub

Private Sub comboUnits_Click()
  RaiseEvent Change
End Sub

Private Sub txtEnd_Change(Index As Integer)
  RaiseEvent Change
End Sub

Private Sub txtRunInf_Change()
  RaiseEvent Change
End Sub

Private Sub txtStart_Change(Index As Integer)
  RaiseEvent Change
End Sub

Private Sub UserControl_Initialize()
  Dim i&
  For i = 0 To 10
    comboOutput.AddItem i
    comboSpecial.AddItem i
  Next i
  comboRunflag.AddItem "Interp"
  comboRunflag.AddItem "Run"
  comboUnits.AddItem "English"
  comboUnits.AddItem "Metric"
End Sub
