VERSION 5.00
Object = "{872F11D5-3322-11D4-9D23-00A0C9768F70}#1.10#0"; "ATCoCtl.ocx"
Begin VB.Form frmSelectLayers 
   Caption         =   "Select layers to add to BASINS project"
   ClientHeight    =   3285
   ClientLeft      =   60
   ClientTop       =   345
   ClientWidth     =   6120
   Icon            =   "frmSelectLayers.frx":0000
   LinkTopic       =   "Form1"
   ScaleHeight     =   3285
   ScaleWidth      =   6120
   StartUpPosition =   3  'Windows Default
   Begin ATCoCtl.ATCoSelectList SelectListTables 
      Height          =   2535
      Left            =   120
      TabIndex        =   3
      Top             =   120
      Width           =   5895
      _ExtentX        =   10398
      _ExtentY        =   4471
      RightLabel      =   "Selected:"
      LeftLabel       =   "Available:"
   End
   Begin VB.Frame fraButtons 
      BorderStyle     =   0  'None
      Caption         =   "Frame1"
      Height          =   375
      Left            =   1680
      TabIndex        =   0
      Top             =   2760
      Width           =   2775
      Begin VB.CommandButton cmdCancel 
         Cancel          =   -1  'True
         Caption         =   "Cancel"
         Height          =   375
         Left            =   1560
         TabIndex        =   2
         Top             =   0
         Width           =   1215
      End
      Begin VB.CommandButton cmdOk 
         Caption         =   "Ok"
         Default         =   -1  'True
         Height          =   375
         Left            =   0
         TabIndex        =   1
         Top             =   0
         Width           =   1215
      End
   End
End
Attribute VB_Name = "frmSelectLayers"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit

Public Function SelectLayers(gdb As clsGeoDatabase) As FastCollection
  Dim LayerNames As FastCollection
  Dim LayerName As String
  Dim retval As FastCollection
  Dim i As Long
  
  SelectListTables.ClearLeft
  SelectListTables.ClearRight
  
  Set LayerNames = gdb.GeoLayers
  For i = 1 To LayerNames.Count
    LayerName = LayerNames.ItemByIndex(i)
    SelectListTables.LeftItemFastAdd LayerName
    If LCase(LayerName) = "nhdflowline" Then
      SelectListTables.MoveRight i - 1
    End If
  Next
  
  Me.Show
  
  Me.Visible = True
  Me.ZOrder
  
  Do
    DoEvents
    Sleep 100
  Loop While Me.Visible
  
  Set SelectLayers = New FastCollection
  For i = 0 To SelectListTables.RightCount - 1
    LayerName = SelectListTables.RightItem(i)
    SelectLayers.Add LayerName, LayerName
  Next
End Function

Private Sub cmdCancel_Click()
  SelectListTables.ClearRight
  cmdOk_Click
End Sub

Private Sub cmdOk_Click()
  Me.Hide
  Me.Visible = False
End Sub

Private Sub Form_Resize()
  Dim w As Long
  Dim h As Long
  w = Me.ScaleWidth
  h = Me.ScaleHeight
  If w > fraButtons.Width + 240 And h > fraButtons.Height * 5 Then
    fraButtons.Left = (w - fraButtons.Width) / 2
    fraButtons.Top = h - fraButtons.Height - 120
    SelectListTables.Width = w - 240
    SelectListTables.Height = fraButtons.Top - 240
  End If
End Sub
