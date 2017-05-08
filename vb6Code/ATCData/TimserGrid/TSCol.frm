VERSION 5.00
Begin VB.Form TSCol 
   Caption         =   "Time Series Attributes"
   ClientHeight    =   3108
   ClientLeft      =   1236
   ClientTop       =   5448
   ClientWidth     =   7356
   HelpContextID   =   950
   Icon            =   "TSCol.frx":0000
   LinkTopic       =   "Form1"
   PaletteMode     =   1  'UseZOrder
   ScaleHeight     =   3108
   ScaleWidth      =   7356
   Begin VB.Frame fraButtons 
      BorderStyle     =   0  'None
      Caption         =   "Frame1"
      Height          =   372
      Left            =   120
      TabIndex        =   5
      Top             =   2520
      Width           =   7212
      Begin VB.CommandButton cmdDefault 
         Caption         =   "&Default"
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
         Left            =   2880
         TabIndex        =   4
         Top             =   0
         Width           =   852
      End
      Begin VB.CommandButton cmdOk 
         Caption         =   "&OK"
         Default         =   -1  'True
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
         Left            =   0
         TabIndex        =   1
         Top             =   0
         Width           =   852
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
         Left            =   960
         TabIndex        =   2
         Top             =   0
         Width           =   852
      End
      Begin VB.CommandButton cmdReset 
         Caption         =   "&Reset"
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
         Left            =   1920
         TabIndex        =   3
         Top             =   0
         Width           =   852
      End
      Begin VB.Label lblShow 
         Caption         =   "Attributes displayed left to right"
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
         Left            =   4200
         TabIndex        =   6
         Top             =   120
         Width           =   2895
      End
   End
   Begin ATCoCtl.ATCoSelectList asl 
      Height          =   2295
      Left            =   120
      TabIndex        =   0
      Top             =   0
      Width           =   7095
      _ExtentX        =   12510
      _ExtentY        =   4043
      RightLabel      =   "Show These Attributes:"
      LeftLabel       =   "Available Attributes:"
   End
End
Attribute VB_Name = "TSCol"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
'Copyright 2000 by AQUA TERRA Consultants

Private Const MXTSCOL = 20
Private NtsCol&
Private pGrid As Object

Public Sub clear()
  asl.ClearLeft
  asl.ClearRight
End Sub

Public Property Set Grid(newvalue As Object)
  Dim i&
  Dim colName As Variant
  Set pGrid = Nothing
  Set pGrid = newvalue
  clear
  On Error Resume Next
  
  i = 0
  For Each colName In pGrid.AvailAttributes
    asl.LeftItem(i) = colName
    i = i + 1
  Next
  MoveCurrentlyVisibleRight
End Property

Private Sub cmdCancel_Click()
  Unload Me
End Sub

Private Sub MoveCurrentlyVisibleRight()
  Dim colName As Variant
  Dim i&
  For Each colName In pGrid.VisibleAttributes
    i = 0
    While i < asl.LeftCount
      If asl.LeftItem(i) = colName Then
        asl.MoveRight i
        i = asl.LeftCount
      End If
      i = i + 1
    Wend
  Next
End Sub

Private Sub cmdDefault_Click()
  asl.MoveAllLeft
  MoveCurrentlyVisibleRight
End Sub

Private Sub cmdOk_Click()
  Dim i&
  Dim nowVisible As Collection
  If asl.RightCount < 1 Then
    MsgBox "You must specify at least one column to show for the list of timeseries data.", vbOKOnly, "Timeseries Columns"
  Else
    Set nowVisible = New Collection
    For i = 0 To asl.RightCount - 1
      nowVisible.Add asl.RightItem(i)
    Next i
    Set pGrid.VisibleAttributes = nowVisible
    pGrid.NewVisibleAttributes
    Unload Me
  End If
End Sub

Private Sub cmdReset_Click()
  Set Grid = pGrid
End Sub

Private Sub Form_Resize()
  Dim margin%
  margin = 225
  If Width > 4000 Then
    asl.Left = margin
    asl.Width = Width - margin * 2
    lblShow.Left = Width - margin - lblShow.Width
    If lblShow.Left - margin > cmdReset.Left + cmdReset.Width Then
      lblShow.Visible = True
    Else
      lblShow.Visible = False
    End If
  End If
  If Height > 3000 Then
    fraButtons.Top = Height - 900
    asl.Height = fraButtons.Top - margin
  End If
End Sub
