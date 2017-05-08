VERSION 5.00
Begin VB.Form frmMap 
   Caption         =   "Select Areas of Interest"
   ClientHeight    =   6600
   ClientLeft      =   48
   ClientTop       =   276
   ClientWidth     =   7068
   HelpContextID   =   10
   Icon            =   "frmMap.frx":0000
   LinkTopic       =   "Form1"
   ScaleHeight     =   6600
   ScaleWidth      =   7068
   StartUpPosition =   3  'Windows Default
   Begin VB.CommandButton cmdMore 
      Caption         =   "Help"
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
      Left            =   5400
      TabIndex        =   3
      ToolTipText     =   "Access help on Map"
      Top             =   6120
      Width           =   1092
   End
   Begin VB.CommandButton cmdCancel 
      Cancel          =   -1  'True
      Caption         =   "&Cancel"
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
      Left            =   4200
      TabIndex        =   2
      ToolTipText     =   "Return to main table ignoring selection changes"
      Top             =   6120
      Width           =   1092
   End
   Begin VB.CommandButton cmdNext 
      Caption         =   "&Ok"
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
      Left            =   3000
      TabIndex        =   1
      ToolTipText     =   "Set these selected HUCs in main table"
      Top             =   6120
      Width           =   1092
   End
   Begin ATML2k.ATCoMap Map1 
      Height          =   5892
      Left            =   120
      TabIndex        =   0
      Top             =   120
      Width           =   6852
      _ExtentX        =   12086
      _ExtentY        =   10393
      RefreshMapLayer =   -1  'True
      ConfirmSelections=   0   'False
      Enabled         =   -1  'True
      LegendVisible   =   -1  'True
      ToolbarVisible  =   -1  'True
   End
End
Attribute VB_Name = "frmMap"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
Private NewHUCs() As String

Private Sub cmdCancel_Click()
  Unload Me
End Sub

Private Sub cmdMore_Click()
  SendKeys "{F1}"
End Sub

Private Sub cmdNext_Click()
  Dim i&

  Me.MousePointer = vbHourglass
  HUCsFromMap Map1
  HUCsToTable frmTable.agdTable
  frmTable.Show
  Me.MousePointer = vbDefault
  Unload Me

End Sub

Private Sub Form_Load()
  Dim i&
  With Map1
    .SetMapData MapDir, MapName, ""
    For i = 1 To 8
      .ButtonVisible(i) = True
    Next
    .ButtonVisible("Add") = False
    .ButtonVisible("Remove") = False
    .ButtonVisible("Move") = False
    '.ButtonVisible("Edit") = False
  End With
  If Map1.LayerCount > 0 And Map1.CurLayer <> 1 Then Map1.CurLayer = 1
End Sub

Private Sub Form_Resize()
  If Width > 350 Then Map1.Width = Width - 320
  If Height > 1032 Then Map1.Height = Height - 1024
  cmdCancel.Left = (Width - cmdCancel.Width) / 2
  cmdCancel.Top = Map1.Height + 228
  cmdNext.Top = cmdCancel.Top
  cmdMore.Top = cmdCancel.Top
  cmdNext.Left = cmdCancel.Left - 1200
  cmdMore.Left = cmdCancel.Left + 1200
End Sub

Private Sub Form_Unload(Cancel As Integer)
  'Map1.SaveMapFile MapName
  Set MapExtent = Map1.Map.Extent
  MapExtentSet = True
End Sub

