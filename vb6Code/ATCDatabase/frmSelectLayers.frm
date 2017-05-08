VERSION 5.00
Begin VB.Form frmSelectLayers 
   Caption         =   "Select layers to save"
   ClientHeight    =   6450
   ClientLeft      =   60
   ClientTop       =   345
   ClientWidth     =   4680
   Icon            =   "frmSelectLayers.frx":0000
   LinkTopic       =   "Form1"
   ScaleHeight     =   6450
   ScaleWidth      =   4680
   StartUpPosition =   3  'Windows Default
   Begin VB.ListBox lstTables 
      Height          =   5715
      Left            =   120
      MultiSelect     =   2  'Extended
      TabIndex        =   0
      Top             =   120
      Width           =   4455
   End
   Begin VB.Frame Frame1 
      BorderStyle     =   0  'None
      Caption         =   "Frame1"
      Height          =   375
      Left            =   960
      TabIndex        =   1
      Top             =   6000
      Width           =   2775
      Begin VB.CommandButton cmdCancel 
         Cancel          =   -1  'True
         Caption         =   "Cancel"
         Height          =   375
         Left            =   1560
         TabIndex        =   3
         Top             =   0
         Width           =   1215
      End
      Begin VB.CommandButton cmdOk 
         Caption         =   "Ok"
         Default         =   -1  'True
         Height          =   375
         Left            =   0
         TabIndex        =   2
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
  Dim retval As FastCollection
  Dim i As Long
  lstTables.Clear
  Set LayerNames = gdb.GeoLayers
  For i = 1 To LayerNames.Count
    lstTables.AddItem LayerNames.ItemByIndex(i)
  Next
  
  Me.Show
  
  Me.Visible = True
  Me.ZOrder
  
  Do
    DoEvents
    Sleep 100
  Loop While Me.Visible
  
  Set SelectLayers = New FastCollection
  For i = 0 To lstTables.ListCount - 1
    If lstTables.Selected(i) Then SelectLayers.Add lstTables.List(i), lstTables.List(i)
  Next
End Function

Private Sub cmdCancel_Click()
  Dim i As Long
  For i = 0 To lstTables.ListCount - 1
    lstTables.Selected(i) = False
  Next
  cmdOk_Click
End Sub

Private Sub cmdOk_Click()
  Me.Hide
  Me.Visible = False
End Sub
