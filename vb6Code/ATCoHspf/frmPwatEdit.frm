VERSION 5.00
Begin VB.Form frmPwatEdit 
   Caption         =   "PWATER Simulation and Input Options"
   ClientHeight    =   6864
   ClientLeft      =   48
   ClientTop       =   276
   ClientWidth     =   8544
   LinkTopic       =   "Form1"
   ScaleHeight     =   6864
   ScaleWidth      =   8544
   StartUpPosition =   3  'Windows Default
   Begin VB.CheckBox chkHigh 
      Caption         =   "High Water Table Conditions (HWTFG)"
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
      Left            =   4320
      TabIndex        =   40
      Top             =   600
      Width           =   4092
   End
   Begin VB.Frame fraOption 
      Caption         =   "Irrigation demand method (IRRGFG)"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   7.8
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   1332
      Index           =   9
      Left            =   4320
      TabIndex        =   33
      Top             =   4800
      Width           =   4092
      Begin VB.OptionButton opt4 
         Caption         =   "Schedule defined by user"
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
         TabIndex        =   39
         Top             =   960
         Width           =   2412
      End
      Begin VB.OptionButton opt3 
         Caption         =   "Allowable water depletion basis"
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
         TabIndex        =   38
         Top             =   720
         Width           =   3612
      End
      Begin VB.OptionButton opt2 
         Caption         =   "From external time series"
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
         Index           =   10
         Left            =   240
         TabIndex        =   35
         Top             =   480
         Width           =   3612
      End
      Begin VB.OptionButton opt1 
         Caption         =   "No demands"
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
         Index           =   10
         Left            =   240
         TabIndex        =   34
         Top             =   240
         Width           =   3612
      End
   End
   Begin VB.Frame fraOption 
      Caption         =   "Frozen ground infiltration computation (IFFCFG)"
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
      Index           =   8
      Left            =   4320
      TabIndex        =   30
      Top             =   3840
      Width           =   4092
      Begin VB.OptionButton opt1 
         Caption         =   "Ice in the snow pack basis"
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
         Index           =   8
         Left            =   240
         TabIndex        =   32
         Top             =   240
         Width           =   3612
      End
      Begin VB.OptionButton opt2 
         Caption         =   "Soil temperature basis"
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
         Index           =   8
         Left            =   240
         TabIndex        =   31
         Top             =   480
         Width           =   3612
      End
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
      TabIndex        =   29
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
      Left            =   4440
      TabIndex        =   12
      Top             =   6360
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
      Left            =   3000
      TabIndex        =   11
      Top             =   6360
      Width           =   1092
   End
   Begin VB.Frame fraOption 
      Caption         =   "Lower Zone ET parameter (VLEFG)"
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
      Index           =   7
      Left            =   4320
      TabIndex        =   10
      Top             =   2880
      Width           =   4092
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
         Index           =   7
         Left            =   240
         TabIndex        =   28
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
         Index           =   7
         Left            =   240
         TabIndex        =   27
         Top             =   240
         Width           =   3612
      End
   End
   Begin VB.Frame fraOption 
      Caption         =   "Interflow recession const (VIRCFG)"
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
      Index           =   6
      Left            =   4320
      TabIndex        =   9
      Top             =   1920
      Width           =   4092
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
         Index           =   6
         Left            =   240
         TabIndex        =   26
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
         Index           =   6
         Left            =   240
         TabIndex        =   25
         Top             =   240
         Width           =   3612
      End
   End
   Begin VB.Frame fraOption 
      Caption         =   "Interflow inflow parameter (VIFWFG)"
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
      Index           =   5
      Left            =   4320
      TabIndex        =   8
      Top             =   960
      Width           =   4092
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
         Index           =   5
         Left            =   240
         TabIndex        =   24
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
         Index           =   5
         Left            =   240
         TabIndex        =   23
         Top             =   240
         Width           =   3612
      End
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
      Index           =   4
      Left            =   120
      TabIndex        =   7
      Top             =   5280
      Width           =   4092
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
         Index           =   4
         Left            =   240
         TabIndex        =   22
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
         Index           =   4
         Left            =   240
         TabIndex        =   21
         Top             =   240
         Width           =   3612
      End
   End
   Begin VB.Frame fraOption 
      Caption         =   "Upper Zone nominal storage (VUZFG)"
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
      Index           =   3
      Left            =   120
      TabIndex        =   6
      Top             =   4320
      Width           =   4092
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
         Index           =   3
         Left            =   240
         TabIndex        =   20
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
         Index           =   3
         Left            =   240
         TabIndex        =   19
         Top             =   240
         Width           =   3612
      End
   End
   Begin VB.Frame fraOption 
      Caption         =   "Interception storage capacity (VCSFG)"
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
      Width           =   4092
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
         TabIndex        =   18
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
         TabIndex        =   17
         Top             =   240
         Width           =   3612
      End
   End
   Begin VB.Frame fraOption 
      Caption         =   "Upper Zone inflow computation (UZFG)"
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
      Width           =   4092
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
         Index           =   1
         Left            =   240
         TabIndex        =   16
         Top             =   480
         Width           =   3612
      End
      Begin VB.OptionButton opt1 
         Caption         =   "Method in HSPX, ARM and NPS"
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
         TabIndex        =   15
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
      Height          =   1332
      Index           =   0
      Left            =   120
      TabIndex        =   3
      Top             =   960
      Width           =   4092
      Begin VB.OptionButton opt4 
         Caption         =   "FTable method"
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
         TabIndex        =   37
         Top             =   960
         Width           =   2412
      End
      Begin VB.OptionButton opt3 
         Caption         =   "Power function method"
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
         TabIndex        =   36
         Top             =   720
         Width           =   2412
      End
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
         TabIndex        =   14
         Top             =   480
         Width           =   3612
      End
      Begin VB.OptionButton opt1 
         Caption         =   "Method in HSPX, ARM and NPS"
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
         TabIndex        =   13
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
      Width           =   3732
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
Attribute VB_Name = "frmPwatEdit"
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

Private Sub chkHigh_Click()
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
    ElseIf cVals(j + 2) = 0 Then
      opt2(0).Value = True
    ElseIf cVals(j + 2) = 2 Then
      opt3(0).Value = True
    ElseIf cVals(j + 2) = 3 Then
      opt4(0).Value = True
    End If
    If cVals(j + 3) = 1 Then
      opt1(1).Value = True
    Else
      opt2(1).Value = True
    End If
    For i = 4 To 9
      If cVals(j + i) = 1 Then
        opt2(i - 2).Value = True
      Else
        opt1(i - 2).Value = True
      End If
    Next i
    If cVals(j + 10) = 1 Then
      opt1(8).Value = True
    Else
      opt2(8).Value = True
    End If
    chkHigh.Value = cVals(j + 11)
    If cVals(j + 12) = 0 Then
      opt1(10).Value = True
    ElseIf cVals(j + 12) = 1 Then
      opt2(10).Value = True
    ElseIf cVals(j + 12) = 2 Then
      opt3(1).Value = True
    ElseIf cVals(j + 12) = 3 Then
      opt4(1).Value = True
    End If
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
  For Each vOpn In myTable.Opn.Uci.OpnBlks("PERLND").Ids
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
    For i = 0 To 9
      fraOption(i).Width = Width / 2 - 228
    Next i
    chkSnow.Width = Width / 2 - 228
    chkHigh.Width = Width / 2 - 228
    For i = 0 To 10
      If i <> 9 Then
        opt1(i).Width = fraOption(0).Width - 500
        opt2(i).Width = fraOption(0).Width - 500
      End If
    Next i
    For i = 0 To 1
      opt3(i).Width = fraOption(0).Width - 500
      opt4(i).Width = fraOption(0).Width - 500
    Next i
    For i = 5 To 9
      fraOption(i).Left = Width / 2
    Next i
    chkHigh.Left = Width / 2
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
  Set lOpn = myTable.Opn.Uci.OpnBlks("PERLND").OperFromID(cmbLand.ItemData(ioper))
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
    ElseIf opt2(0).Value = True Then
      cVals(j + 2) = 0
    ElseIf opt3(0).Value = True Then
      cVals(j + 2) = 2
    ElseIf opt4(0).Value = True Then
      cVals(j + 2) = 3
    End If
    If opt1(1).Value = True Then
      cVals(j + 3) = 1
    Else
      cVals(j + 3) = 0
    End If
    For i = 4 To 9
      If opt2(i - 2).Value = True Then
        cVals(j + i) = 1
      Else
        cVals(j + i) = 0
      End If
    Next i
    If opt1(8).Value = True Then
      cVals(j + 10) = 1
    Else
      cVals(j + 10) = 2
    End If
    cVals(j + 11) = chkHigh.Value
    If opt1(10).Value = True Then
      cVals(j + 12) = 0
    ElseIf opt2(10).Value = True Then
      cVals(j + 12) = 1
    ElseIf opt3(1).Value = True Then
      cVals(j + 12) = 2
    ElseIf opt4(1).Value = True Then
      cVals(j + 12) = 3
    End If
  End If
End Sub

Private Sub opt1_Click(Index As Integer)
  StoreChanges cmbLand.ListIndex
End Sub

Private Sub opt2_Click(Index As Integer)
  StoreChanges cmbLand.ListIndex
End Sub

Private Sub opt3_Click(Index As Integer)
  StoreChanges cmbLand.ListIndex
End Sub

Private Sub opt4_Click(Index As Integer)
  StoreChanges cmbLand.ListIndex
End Sub
