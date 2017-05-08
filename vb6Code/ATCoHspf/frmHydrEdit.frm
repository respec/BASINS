VERSION 5.00
Begin VB.Form frmHydrEdit 
   Caption         =   "HYDR Simulation and Input Options"
   ClientHeight    =   6156
   ClientLeft      =   48
   ClientTop       =   276
   ClientWidth     =   7620
   LinkTopic       =   "Form1"
   ScaleHeight     =   6156
   ScaleWidth      =   7620
   StartUpPosition =   3  'Windows Default
   Begin VB.Frame fraExit 
      Caption         =   "Exit"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   7.8
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   2772
      Left            =   120
      TabIndex        =   11
      Top             =   2640
      Width           =   7332
      Begin ATCoCtl.ATCoText atxGt 
         Height          =   252
         Left            =   5160
         TabIndex        =   21
         Top             =   1080
         Width           =   1092
         _ExtentX        =   1926
         _ExtentY        =   445
         InsideLimitsBackground=   16777215
         OutsideHardLimitBackground=   8421631
         OutsideSoftLimitBackground=   8454143
         HardMax         =   5
         HardMin         =   0
         SoftMax         =   -999
         SoftMin         =   -999
         MaxWidth        =   -999
         Alignment       =   1
         DataType        =   1
         DefaultValue    =   -999
         Value           =   "-999"
         Enabled         =   -1  'True
      End
      Begin ATCoCtl.ATCoText atxFvol 
         Height          =   252
         Left            =   5160
         TabIndex        =   20
         Top             =   720
         Width           =   1092
         _ExtentX        =   1926
         _ExtentY        =   445
         InsideLimitsBackground=   16777215
         OutsideHardLimitBackground=   8421631
         OutsideSoftLimitBackground=   8454143
         HardMax         =   8
         HardMin         =   -5
         SoftMax         =   -999
         SoftMin         =   -999
         MaxWidth        =   -999
         Alignment       =   1
         DataType        =   1
         DefaultValue    =   -999
         Value           =   "-999"
         Enabled         =   -1  'True
      End
      Begin VB.CheckBox chkAssign 
         Caption         =   "Assign To All Exits"
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
         TabIndex        =   17
         Top             =   240
         Width           =   2172
      End
      Begin VB.Frame fraOption 
         Caption         =   "Function used to combine outflow demand components (FUNCT)"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   7.8
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   1092
         Index           =   1
         Left            =   240
         TabIndex        =   13
         Top             =   1440
         Width           =   6852
         Begin VB.OptionButton opt1 
            Caption         =   "Smaller of F(vol) and G(t)"
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
            Top             =   240
            Width           =   3612
         End
         Begin VB.OptionButton opt2 
            Caption         =   "Larger of F(vol) and G(t)"
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
            Top             =   480
            Width           =   3612
         End
         Begin VB.OptionButton opt3 
            Caption         =   "Sum of F(vol) and G(t)"
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
            TabIndex        =   14
            Top             =   720
            Width           =   2412
         End
      End
      Begin VB.ComboBox cmbExit 
         Height          =   288
         Left            =   240
         Style           =   2  'Dropdown List
         TabIndex        =   12
         Top             =   240
         Width           =   2892
      End
      Begin VB.Label lblF 
         Caption         =   "G(t) component of the outflow demand (ODGTFG)"
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
         TabIndex        =   19
         Top             =   1080
         Width           =   4812
      End
      Begin VB.Label lblF 
         Caption         =   "F(vol) component of the outflow demand (ODFVFG)"
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
         TabIndex        =   18
         Top             =   720
         Width           =   4812
      End
   End
   Begin VB.CheckBox chkAux 
      Caption         =   "Calculate shear velocity and bed shear stress (AUX3FG)"
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
      Left            =   120
      TabIndex        =   10
      Top             =   2280
      Width           =   7452
   End
   Begin VB.CheckBox chkAux 
      Caption         =   "Calculate average velocity and average cross-sectional area (AUX2FG)"
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
      TabIndex        =   9
      Top             =   1920
      Width           =   7452
   End
   Begin VB.CheckBox chkAssign 
      Caption         =   "Assign To All Rchres"
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
      Left            =   4440
      TabIndex        =   8
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
      Left            =   3720
      TabIndex        =   5
      Top             =   5640
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
      Left            =   2280
      TabIndex        =   4
      Top             =   5640
      Width           =   1092
   End
   Begin VB.Frame fraOption 
      Caption         =   "F(vol) outflow demand components (VCONFG)"
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
      Top             =   600
      Width           =   7332
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
         Index           =   0
         Left            =   240
         TabIndex        =   7
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
         Index           =   0
         Left            =   240
         TabIndex        =   6
         Top             =   240
         Width           =   3612
      End
   End
   Begin VB.CheckBox chkAux 
      Caption         =   "Calculate depth, stage, surface area, average depth, and top width (AUX1FG)"
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
      TabIndex        =   2
      Top             =   1560
      Width           =   7452
   End
   Begin VB.ComboBox cmbReach 
      Height          =   288
      Left            =   1200
      Style           =   2  'Dropdown List
      TabIndex        =   1
      Top             =   120
      Width           =   3012
   End
   Begin VB.Label lblLand 
      Caption         =   "Rchres"
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
Attribute VB_Name = "frmHydrEdit"
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
Dim parmcount&

Private Sub chkAux_Click(Index As Integer)
  StoreChanges cmbReach.ListIndex
End Sub

Private Sub cmbExit_Click()
  Dim i, nexit&, j&

  initing = True
  'after user changed combo list, use to refresh values
  j = cmbReach.ListIndex * parmcount
  nexit = cmbExit.ListIndex + 1

  atxFvol.Value = cVals(j + 4 + nexit)
  atxGt.Value = cVals(j + 9 + nexit)

  If cVals(j + 14 + nexit) = 1 Then
    opt1(1).Value = True
  ElseIf cVals(j + 14 + nexit) = 2 Then
    opt2(1).Value = True
  ElseIf cVals(j + 14 + nexit) = 3 Then
    opt3(1).Value = True
  End If
  initing = False
End Sub

Private Sub cmbReach_Click()
  Dim i, nexit&, j&
  
  initing = True
  'after user changed combo list, use to refresh values
  If chkAssign(0).Value <> 1 Then
    'assign to all is not checked
    j = cmbReach.ListIndex * parmcount
    If cVals(j + 1) = 1 Then
      opt2(0).Value = True
    Else
      opt1(0).Value = True
    End If
    For i = 0 To 2
      chkAux(i).Value = cVals(j + i + 2)
    Next i
    
    nexit = cVals(j + parmcount)
    cmbExit.Clear
    For i = 1 To nexit
      cmbExit.AddItem "Exit " & i
    Next i
    cmbExit.ListIndex = 0
  End If
  initing = False
End Sub

Private Sub cmdCancel_Click()
  Unload Me
End Sub

Private Sub cmdOK_Click()
  Dim i&, istart&, iend&, j&, estart&, eend&, k&
  
  If chkAssign(0).Value = 1 Then
    For j = 0 To cmbReach.ListCount - 1
      StoreChanges j
    Next j
  Else
    StoreChanges cmbReach.ListIndex
  End If
  
  For j = 0 To cmbReach.ListCount - 1
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
  cmbReach.Clear
  i = 0
  initing = True
  For Each vOpn In myTable.Opn.Uci.OpnBlks("RCHRES").Ids
    Set lOpn = vOpn
    cmbReach.AddItem lOpn.Description & " (" & lOpn.Id & ")"
    cmbReach.ItemData(i) = lOpn.Id
    i = i + 1
    'save a local copy of the values
    Set tempTable = lOpn.Tables(myTable.Name)
    parmcount = tempTable.Parms.Count + 1
    ReDim Preserve cVals(i * parmcount)
    For j = 1 To tempTable.Parms.Count
      cVals(((i - 1) * parmcount) + j) = tempTable.Parms(j)
    Next j
    cVals(i * parmcount) = lOpn.Tables("GEN-INFO").Parms("NEXITS").Value
  Next vOpn
  cmbReach.ListIndex = 0
  initing = False
End Sub

Public Sub init(O As Object, icon As StdPicture)
  Set myIcon = icon
  Set myTable = O
End Sub

Private Sub Form_Resize()
  If Width > 1200 Then
    fraOption(0).Width = Width - 400
    opt1(0).Width = fraOption(0).Width - 500
    opt2(0).Width = fraOption(0).Width - 500
    chkAux(0).Width = Width - 500
    chkAux(1).Width = Width - 500
    chkAux(2).Width = Width - 500
    fraExit.Width = Width - 400
    fraOption(1).Width = fraExit.Width - 500
    opt1(1).Width = fraOption(1).Width - 500
    opt2(1).Width = fraOption(1).Width - 500
    opt3(1).Width = fraOption(1).Width - 500

    cmdOk.Left = Width / 2 - cmdOk.Width - 200
    cmdCancel.Left = Width / 2 + 100
  End If
End Sub

Private Sub SaveChanges(ioper&)
  Dim i As Long
  Dim j As Long
  Dim lOpn As HspfOperation
  Dim ltable As HspfTable
  
  j = ioper * parmcount
  Set lOpn = myTable.Opn.Uci.OpnBlks("RCHRES").OperFromID(cmbReach.ItemData(ioper))
  Set ltable = lOpn.Tables(myTable.Name)
  
  For i = 1 To myTable.Parms.Count
    ltable.Parms(i).Value = cVals(j + i)
  Next i
  
End Sub

Private Sub StoreChanges(ioper&)
  Dim i As Long
  Dim j As Long
  Dim k As Long
  Dim estart As Long
  Dim eend As Long
  
  If Not initing Then
    j = ioper * parmcount
    
    If opt1(0).Value = True Then
      cVals(j + 1) = 0
    Else
      cVals(j + 1) = 1
    End If
    For i = 0 To 2
      cVals(j + i + 2) = chkAux(i).Value
    Next i
    
    If chkAssign(1).Value = 0 Then
      estart = cmbExit.ListIndex + 1
      eend = estart
    Else
      estart = 1
      eend = cmbExit.ListCount
    End If

    For k = estart To eend
      cVals(j + 4 + k) = atxFvol.Value
      cVals(j + 9 + k) = atxGt.Value
      If opt1(1).Value = True Then
        cVals(j + 14 + k) = 1
      ElseIf opt2(1).Value = True Then
        cVals(j + 14 + k) = 2
      ElseIf opt3(1).Value = True Then
        cVals(j + 14 + k) = 3
      End If
    Next k
  End If
End Sub

Private Sub opt1_Click(Index As Integer)
  StoreChanges cmbReach.ListIndex
End Sub

Private Sub opt2_Click(Index As Integer)
  StoreChanges cmbReach.ListIndex
End Sub

Private Sub opt3_Click(Index As Integer)
  StoreChanges cmbReach.ListIndex
End Sub
