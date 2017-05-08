VERSION 5.00
Begin VB.Form frmSpecifyUnits 
   Caption         =   "Specify Units"
   ClientHeight    =   3795
   ClientLeft      =   60
   ClientTop       =   345
   ClientWidth     =   4680
   Icon            =   "frmSpecifyUnits.frx":0000
   LinkTopic       =   "Form1"
   ScaleHeight     =   3795
   ScaleWidth      =   4680
   StartUpPosition =   3  'Windows Default
   Begin VB.ListBox lstUnits 
      Height          =   2340
      IntegralHeight  =   0   'False
      Left            =   2400
      TabIndex        =   5
      Top             =   600
      Width           =   2175
   End
   Begin VB.ListBox lstCategory 
      Height          =   2340
      IntegralHeight  =   0   'False
      Left            =   120
      TabIndex        =   3
      Top             =   600
      Width           =   2175
   End
   Begin VB.Frame fraButtons 
      BorderStyle     =   0  'None
      Height          =   375
      Left            =   1200
      TabIndex        =   0
      Top             =   3120
      Width           =   2412
      Begin VB.CommandButton cmdCancel 
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
         Height          =   372
         Left            =   1440
         TabIndex        =   2
         Top             =   0
         Width           =   972
      End
      Begin VB.CommandButton cmdOk 
         Caption         =   "Ok"
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
         TabIndex        =   1
         Top             =   0
         Width           =   972
      End
   End
   Begin VB.Label lblUnits 
      BackStyle       =   0  'Transparent
      Caption         =   "Select Units"
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
      Left            =   2400
      TabIndex        =   6
      Top             =   240
      Width           =   1815
   End
   Begin VB.Label lblCategory 
      BackStyle       =   0  'Transparent
      Caption         =   "Select Unit Category"
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
      TabIndex        =   4
      Top             =   240
      Width           =   2175
   End
End
Attribute VB_Name = "frmSpecifyUnits"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit

Private LastEnteredUnits As String

Public Function GetUnitsFromUser()
  Dim UnitIndex As Long
  Dim UnitCategories As FastCollection
  Set UnitCategories = GetAllUnitCategories
  For UnitIndex = 1 To UnitCategories.Count
    lstCategory.AddItem UnitCategories.ItemByIndex(UnitIndex)
  Next
  lstUnits.clear
  Me.Show
  While Me.Visible
    DoEvents
  Wend
  GetUnitsFromUser = LastEnteredUnits
  Unload Me
End Function

Private Sub cmdCancel_Click()
  LastEnteredUnits = ""
  Me.Visible = False
End Sub

Private Sub cmdOk_Click()
  Me.Visible = False
End Sub

Private Sub lstCategory_Click()
  Dim UnitsOfType As FastCollection
  Dim vUnits As Variant
  On Error GoTo EndSub
  Me.MousePointer = vbHourglass
  lstUnits.clear
  lstUnits.AddItem "<none>"
  Set UnitsOfType = GetAllUnitsInCategory(lstCategory.text)
  For Each vUnits In UnitsOfType
    lstUnits.AddItem CStr(vUnits)
  Next
EndSub:
  Me.MousePointer = vbDefault
End Sub

Private Sub lstUnits_Click()
  LastEnteredUnits = lstUnits.text
End Sub

Private Sub Form_Resize()
  Dim w As Long
  Dim h As Long
  w = Me.ScaleWidth
  h = Me.ScaleHeight
  If w > 1000 And h > 1000 Then
    fraButtons.Left = (w - fraButtons.Width) / 2
    fraButtons.Top = h - 468
    lstCategory.Height = fraButtons.Top - lstCategory.Top - 108
    lstUnits.Height = lstCategory.Height
    lstCategory.Width = (w - 330) / 2
    lstUnits.Width = lstCategory.Width
    lblUnits.Left = lstCategory.Width + lstCategory.Left + 120
    lstUnits.Left = lblUnits.Left
  End If
End Sub

