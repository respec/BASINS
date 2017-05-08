VERSION 5.00
Begin VB.Form frmMapCols 
   Caption         =   "Table Columns"
   ClientHeight    =   5892
   ClientLeft      =   60
   ClientTop       =   348
   ClientWidth     =   5100
   HelpContextID   =   521
   LinkTopic       =   "Form1"
   ScaleHeight     =   5892
   ScaleWidth      =   5100
   StartUpPosition =   3  'Windows Default
   Visible         =   0   'False
   Begin VB.Frame fraButtons 
      BorderStyle     =   0  'None
      Caption         =   "Frame1"
      Height          =   372
      Left            =   120
      TabIndex        =   20
      Top             =   2040
      Width           =   4572
      Begin VB.CommandButton cmdOption 
         Caption         =   "Dow&n"
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
         Index           =   3
         Left            =   3600
         TabIndex        =   4
         Top             =   0
         Width           =   855
      End
      Begin VB.CommandButton cmdOption 
         Caption         =   "&Up"
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
         Left            =   2640
         TabIndex        =   3
         Top             =   0
         Width           =   855
      End
      Begin VB.CommandButton cmdClose 
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
         Index           =   1
         Left            =   960
         TabIndex        =   2
         TabStop         =   0   'False
         Top             =   0
         Width           =   855
      End
      Begin VB.CommandButton cmdClose 
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
         Index           =   0
         Left            =   0
         TabIndex        =   1
         Top             =   0
         Width           =   855
      End
   End
   Begin VB.VScrollBar VScroll 
      Height          =   1935
      Left            =   4800
      TabIndex        =   0
      Top             =   360
      Visible         =   0   'False
      Width           =   255
   End
   Begin VB.Frame fraColumn 
      BorderStyle     =   0  'None
      Height          =   475
      Index           =   0
      Left            =   120
      TabIndex        =   14
      Top             =   360
      Width           =   4635
      Begin VB.TextBox txtAlias 
         Height          =   312
         Index           =   0
         Left            =   2640
         TabIndex        =   11
         Text            =   "?"
         Top             =   120
         Width           =   1272
      End
      Begin VB.CheckBox chkTable 
         Caption         =   "Check1"
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
         Left            =   4260
         TabIndex        =   13
         Top             =   150
         Width           =   192
      End
      Begin VB.ComboBox comboStyle 
         Height          =   315
         Index           =   0
         Left            =   6000
         Style           =   2  'Dropdown List
         TabIndex        =   15
         Top             =   120
         Visible         =   0   'False
         Width           =   1815
      End
      Begin VB.CheckBox chkMapCov 
         Caption         =   "Check1"
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
         Left            =   4080
         TabIndex        =   12
         Top             =   150
         Visible         =   0   'False
         Width           =   192
      End
      Begin VB.Label lblColumn 
         Caption         =   "?"
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
         TabIndex        =   9
         Top             =   165
         Width           =   1455
      End
      Begin VB.Label lblType 
         Caption         =   "?"
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
         Left            =   1680
         TabIndex        =   10
         Top             =   165
         Width           =   915
      End
   End
   Begin VB.Label lblCol 
      Caption         =   "Column"
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
      Top             =   120
      Width           =   1155
   End
   Begin VB.Label lblCol 
      Alignment       =   2  'Center
      Caption         =   " lblCol"
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
      Left            =   5160
      TabIndex        =   19
      Top             =   120
      Visible         =   0   'False
      Width           =   615
   End
   Begin VB.Label lblCol 
      Caption         =   " lblCol"
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
      Left            =   5580
      TabIndex        =   18
      Top             =   120
      Visible         =   0   'False
      Width           =   615
   End
   Begin VB.Label lblCol 
      Caption         =   "Style"
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
      Left            =   6180
      TabIndex        =   17
      Top             =   120
      Visible         =   0   'False
      Width           =   735
   End
   Begin VB.Label lblCol 
      Caption         =   "Type"
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
      Index           =   5
      Left            =   1680
      TabIndex        =   6
      Top             =   120
      Width           =   495
   End
   Begin VB.Label lblCol 
      Caption         =   "Label"
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
      Left            =   2760
      TabIndex        =   7
      Top             =   120
      Width           =   1155
   End
   Begin VB.Label lblCol 
      Caption         =   "Table"
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
      Index           =   8
      Left            =   4260
      TabIndex        =   8
      Top             =   120
      Width           =   495
   End
   Begin VB.Label lblCol 
      Caption         =   " "
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
      Left            =   4080
      TabIndex        =   16
      Top             =   120
      Width           =   495
   End
End
Attribute VB_Name = "frmMapCols"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
'Copyright 2000 by AQUA TERRA Consultants

Public CurColumn&
Public TopFrameTop&, TopFrameLeft&

Public Sub AddField(ByVal fiName$, ByVal fiCaption$, ByVal fiColumn&)  'fi As MapFieldInfo)
  MapColAddField fiName, fiCaption, fiColumn, Me
End Sub

Public Sub SetRecordset(rs As Recordset)
  MapColSetRecordset rs, Me
End Sub

Private Sub chkTable_Click(Index As Integer)
  Dim sel As Boolean
  SelectColumn Index
  If chkTable(Index).Value = 1 Then sel = True Else sel = False
  MapColSelectField Index, sel
End Sub

Private Sub cmdClose_Click(Index As Integer)
  If Index = 0 Then 'Ok was pressed
    MapColsOk = True
  Else
    MapColsOk = False
    ClearFrmMapCols
  End If
  CurColumn = 0
  Unload Me
  'Me.Hide
End Sub

Private Sub cmdOption_Click(Index As Integer)
  If CurColumn < 0 Then SelectColumn 0
  MapColOption_Click Index, Me
End Sub

Private Sub Form_Load()
  MapColsOk = False
  CurColumn = 0
  TopFrameTop = fraColumn(0).Top
  TopFrameLeft = fraColumn(0).Left
End Sub

Private Sub Form_Resize()
  MapColsResize Me
End Sub

Private Sub fraColumn_Click(Index As Integer)
  SelectColumn Index
End Sub

Private Sub lblColumn_Click(Index As Integer)
  SelectColumn Index
End Sub

Private Sub lblType_Click(Index As Integer)
  SelectColumn Index
End Sub

Public Sub SelectColumn(ByVal Index As Integer)
  If (CurColumn >= 0) Then fraColumn(CurColumn).BorderStyle = 0
  CurColumn = Index
  fraColumn(CurColumn).BorderStyle = 1
End Sub

Private Sub txtAlias_Click(Index As Integer)
  SelectColumn Index
End Sub

Private Sub VScroll_Change()
  VScroll_Scroll
End Sub

Private Sub VScroll_Scroll()
  If VScroll.Visible Then
    MapColsResize Me
  End If
End Sub
