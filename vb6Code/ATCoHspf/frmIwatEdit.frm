VERSION 5.00
Begin VB.Form frmIwatEdit 
   Caption         =   "IWATER Simulation and Input Options"
   ClientHeight    =   5052
   ClientLeft      =   48
   ClientTop       =   276
   ClientWidth     =   6156
   LinkTopic       =   "Form1"
   ScaleHeight     =   5052
   ScaleWidth      =   6156
   StartUpPosition =   3  'Windows Default
   Begin VB.CheckBox chkLateral 
      Caption         =   "Lateral surface inflow subject to retention (RTLIFG)"
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
      TabIndex        =   15
      Top             =   960
      Width           =   5892
   End
   Begin VB.CheckBox chkAssign 
      Caption         =   "Assign To All"
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
      Left            =   4440
      TabIndex        =   14
      Top             =   120
      Width           =   2172
   End
   Begin VB.CommandButton cmdCancel 
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
      Left            =   3240
      TabIndex        =   7
      Top             =   4440
      Width           =   1092
   End
   Begin VB.CommandButton cmdOk 
      Caption         =   "&OK"
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
      TabIndex        =   6
      Top             =   4440
      Width           =   1092
   End
   Begin VB.Frame fraOption 
      Caption         =   "Manning's n for overland flow plane (VMNFG)"
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
      Index           =   2
      Left            =   120
      TabIndex        =   5
      Top             =   3360
      Width           =   5892
      Begin VB.OptionButton opt2 
         Caption         =   "Vary monthly"
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
         Left            =   240
         TabIndex        =   13
         Top             =   480
         Width           =   3612
      End
      Begin VB.OptionButton opt1 
         Caption         =   "Constant"
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
         Left            =   240
         TabIndex        =   12
         Top             =   240
         Width           =   3612
      End
   End
   Begin VB.Frame fraOption 
      Caption         =   "Retention storage capacity (VRSFG)"
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
      Index           =   1
      Left            =   120
      TabIndex        =   4
      Top             =   2400
      Width           =   5892
      Begin VB.OptionButton opt2 
         Caption         =   "Vary monthly"
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
         Left            =   240
         TabIndex        =   11
         Top             =   480
         Width           =   3612
      End
      Begin VB.OptionButton opt1 
         Caption         =   "Constant"
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
         Left            =   240
         TabIndex        =   10
         Top             =   240
         Width           =   3612
      End
   End
   Begin VB.Frame fraOption 
      Caption         =   "Overland flow routing (RTOPFG)"
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
      Index           =   0
      Left            =   120
      TabIndex        =   3
      Top             =   1440
      Width           =   5892
      Begin VB.OptionButton opt2 
         Caption         =   "Alternative method"
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
         Left            =   240
         TabIndex        =   9
         Top             =   480
         Width           =   3612
      End
      Begin VB.OptionButton opt1 
         Caption         =   "Method in NPS"
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
         Left            =   240
         TabIndex        =   8
         Top             =   240
         Width           =   3612
      End
   End
   Begin VB.CheckBox chkSnow 
      Caption         =   "Use snow simulation data (CSNOFG)"
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
      TabIndex        =   2
      Top             =   600
      Width           =   5892
   End
   Begin VB.ComboBox cmbLand 
      Height          =   288
      Left            =   1200
      Style           =   2  'Dropdown List
      TabIndex        =   1
      Top             =   120
      Width           =   3012
   End
   Begin VB.Label lblLand 
      Caption         =   "Land Use"
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
      Width           =   972
   End
End
Attribute VB_Name = "frmIwatEdit"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
'Copyright 2002 AQUA TERRA Consultants - Royalty-free use permitted under open source license

Dim myTable As HspfTable
Dim myIcon As StdPicture
Dim cVals&()
Dim initing As Boolean

Private Sub chkLateral_Click()
  StoreChanges cmbLand.ListIndex
End Sub

Private Sub chkSnow_Click()
  StoreChanges cmbLand.ListIndex
End Sub

Private Sub cmbLand_Click()
  Dim i&, j&
  
  initing = True
  'after user changed combo list, use to refresh values
  If chkAssign.Value <> 1 Then
    'assign to all is not checked
    j = cmbLand.ListIndex * myTable.Parms.Count
    chkSnow.Value = cVals(j + 1)
    If cVals(j + 2) = 1 Then
      opt1(0).Value = True
    Else
      opt2(0).Value = True
    End If
    For i = 3 To 4
      If cVals(j + i) = 1 Then
        opt2(i - 2).Value = True
      Else
        opt1(i - 2).Value = True
      End If
    Next i
    chkLateral.Value = cVals(j + 5)
  End If
  initing = False
End Sub

Private Sub cmdCancel_Click()
  Unload Me
End Sub

Private Sub cmdOK_Click()
  Dim i&, istart&, iend&, j&
  
  If chkAssign.Value = 1 Then
    For j = 0 To cmbLand.ListCount - 1
      StoreChanges j
    Next j
  Else
    StoreChanges cmbLand.ListIndex
  End If
  
  For j = 0 To cmbLand.ListCount - 1
    SaveChanges j
  Next j
  Unload Me
End Sub

Private Sub Form_Load()
  Dim vOpn As Variant
  Dim lOpn As HspfOperation
  Dim i As Long
  Dim j As Long
  Dim tempTable As HspfTable
  
  Me.icon = myIcon
  cmbLand.Clear
  i = 0
  initing = True
  For Each vOpn In myTable.Opn.Uci.OpnBlks("IMPLND").Ids
    Set lOpn = vOpn
    cmbLand.AddItem lOpn.Description & " (" & lOpn.Id & ")"
    cmbLand.ItemData(i) = lOpn.Id
    i = i + 1
    'save a local copy of the values
    Set tempTable = lOpn.Tables(myTable.Name)
    ReDim Preserve cVals(i * tempTable.Parms.Count)
    For j = 1 To tempTable.Parms.Count
      cVals(((i - 1) * tempTable.Parms.Count) + j) = tempTable.Parms(j)
    Next j
  Next vOpn
  cmbLand.ListIndex = 0
  initing = False
End Sub

Public Sub init(O As Object, icon As StdPicture)
  Set myIcon = icon
  Set myTable = O
End Sub

Private Sub Form_Resize()
  Dim i As Long
  If Width > 1200 Then
    For i = 0 To 2
      fraOption(i).Width = Width - 360
    Next i
    chkSnow.Width = Width - 360
    chkLateral.Width = Width - 360
    For i = 0 To 2
      opt1(i).Width = fraOption(0).Width - 500
      opt2(i).Width = fraOption(0).Width - 500
    Next i
    cmdOk.Left = Width / 2 - cmdOk.Width - 200
    cmdCancel.Left = Width / 2 + 100
  End If
End Sub

Private Sub SaveChanges(ioper&)
  Dim i As Long
  Dim j As Long
  Dim lOpn As HspfOperation
  Dim ltable As HspfTable
  
  j = ioper * myTable.Parms.Count
  Set lOpn = myTable.Opn.Uci.OpnBlks("IMPLND").OperFromID(cmbLand.ItemData(ioper))
  Set ltable = lOpn.Tables(myTable.Name)
  
  For i = 1 To myTable.Parms.Count
    ltable.Parms(i).Value = cVals(j + i)
  Next i
  
End Sub

Private Sub StoreChanges(ioper&)
  Dim i As Long
  Dim j As Long
  
  If Not initing Then
    j = ioper * myTable.Parms.Count
    
    cVals(j + 1) = chkSnow.Value
    
    If opt1(0).Value = True Then
      cVals(j + 2) = 1
    Else
      cVals(j + 2) = 0
    End If
    For i = 3 To 4
      If opt2(i - 2).Value = True Then
        cVals(j + i) = 1
      Else
        cVals(j + i) = 0
      End If
    Next i
    cVals(j + 5) = chkLateral.Value
  End If
End Sub

Private Sub opt1_Click(Index As Integer)
  StoreChanges cmbLand.ListIndex
End Sub

Private Sub opt2_Click(Index As Integer)
  StoreChanges cmbLand.ListIndex
End Sub
