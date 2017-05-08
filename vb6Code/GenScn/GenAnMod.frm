VERSION 5.00
Begin VB.Form frmGenScnAnimMod 
   Caption         =   "GenScn Animation Modify"
   ClientHeight    =   2535
   ClientLeft      =   2040
   ClientTop       =   2085
   ClientWidth     =   5010
   BeginProperty Font 
      Name            =   "MS Sans Serif"
      Size            =   8.25
      Charset         =   0
      Weight          =   700
      Underline       =   0   'False
      Italic          =   0   'False
      Strikethrough   =   0   'False
   EndProperty
   HelpContextID   =   111
   Icon            =   "GenAnMod.frx":0000
   LinkTopic       =   "Form1"
   PaletteMode     =   1  'UseZOrder
   ScaleHeight     =   2535
   ScaleWidth      =   5010
   Begin VB.ComboBox cboLine 
      Height          =   288
      Index           =   2
      ItemData        =   "GenAnMod.frx":0442
      Left            =   3600
      List            =   "GenAnMod.frx":044F
      TabIndex        =   9
      Text            =   "Combo1"
      Top             =   1560
      Width           =   1092
   End
   Begin VB.ComboBox cboLine 
      Height          =   288
      Index           =   1
      ItemData        =   "GenAnMod.frx":0468
      Left            =   2400
      List            =   "GenAnMod.frx":0475
      TabIndex        =   8
      Text            =   "Combo1"
      Top             =   1560
      Width           =   1092
   End
   Begin VB.ComboBox cboLine 
      Height          =   288
      Index           =   0
      ItemData        =   "GenAnMod.frx":048E
      Left            =   1200
      List            =   "GenAnMod.frx":049B
      TabIndex        =   7
      Text            =   "Combo1"
      Top             =   1560
      Width           =   1092
   End
   Begin VB.ComboBox cboColor 
      Height          =   288
      Index           =   2
      ItemData        =   "GenAnMod.frx":04B4
      Left            =   3600
      List            =   "GenAnMod.frx":04CD
      TabIndex        =   6
      Text            =   "Combo1"
      Top             =   1200
      Width           =   1092
   End
   Begin VB.ComboBox cboColor 
      Height          =   288
      Index           =   1
      ItemData        =   "GenAnMod.frx":0501
      Left            =   2400
      List            =   "GenAnMod.frx":051A
      TabIndex        =   5
      Text            =   "Combo1"
      Top             =   1200
      Width           =   1092
   End
   Begin VB.ComboBox cboColor 
      Height          =   288
      Index           =   0
      ItemData        =   "GenAnMod.frx":054E
      Left            =   1200
      List            =   "GenAnMod.frx":0567
      TabIndex        =   4
      Text            =   "Combo1"
      Top             =   1200
      Width           =   1092
   End
   Begin VB.CommandButton cmdCancel 
      Cancel          =   -1  'True
      Caption         =   "Cancel"
      Height          =   372
      Left            =   3240
      TabIndex        =   11
      Top             =   2040
      Width           =   852
   End
   Begin VB.CommandButton cmdOkay 
      Caption         =   "&OK"
      Default         =   -1  'True
      Height          =   372
      Left            =   1800
      TabIndex        =   10
      Top             =   2040
      Width           =   852
   End
   Begin VB.TextBox txtUpper 
      Height          =   288
      Left            =   3000
      TabIndex        =   3
      Top             =   600
      Width           =   1092
   End
   Begin VB.TextBox txtLower 
      Height          =   288
      Left            =   1680
      TabIndex        =   1
      Top             =   600
      Width           =   1092
   End
   Begin VB.Label lblThick 
      Alignment       =   1  'Right Justify
      Caption         =   "Thickness"
      Height          =   252
      Left            =   120
      TabIndex        =   16
      Top             =   1560
      Width           =   972
   End
   Begin VB.Label lblColor 
      Alignment       =   1  'Right Justify
      Caption         =   "Color"
      Height          =   252
      Left            =   240
      TabIndex        =   15
      Top             =   1200
      Width           =   852
   End
   Begin VB.Label lblHigh 
      Alignment       =   2  'Center
      Caption         =   "High"
      Height          =   252
      Left            =   3600
      TabIndex        =   14
      Top             =   960
      Width           =   1092
   End
   Begin VB.Label lblMedium 
      Alignment       =   2  'Center
      Caption         =   "Medium"
      Height          =   252
      Left            =   2400
      TabIndex        =   13
      Top             =   960
      Width           =   1092
   End
   Begin VB.Label lblLow 
      Alignment       =   2  'Center
      Caption         =   "Low"
      Height          =   252
      Left            =   1200
      TabIndex        =   12
      Top             =   960
      Width           =   1092
   End
   Begin VB.Label lblUpper 
      Caption         =   "Upper Boundary"
      Height          =   372
      Left            =   3120
      TabIndex        =   2
      Top             =   120
      Width           =   852
   End
   Begin VB.Label lblLower 
      Caption         =   "Lower Boundary"
      Height          =   372
      Left            =   1800
      TabIndex        =   0
      Top             =   120
      Width           =   852
   End
End
Attribute VB_Name = "frmGenScnAnimMod"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Dim itmx As ListItem


Private Sub cmdCancel_Click()
    Unload frmGenScnAnimMod
End Sub

Private Sub cmdOkay_Click()
    For i = 1 To frmGenScnAnimate.lvwSegments.ListItems.Count
      If frmGenScnAnimate.lvwSegments.ListItems(i).Selected Then
        Set itmx = frmGenScnAnimate.lvwSegments.ListItems(i)
        itmx.SubItems(1) = txtLower.Text
        itmx.SubItems(2) = txtUpper.Text
        itmx.SubItems(3) = cboColor(0).List(cboColor(0).ListIndex)
        itmx.SubItems(4) = cboColor(1).List(cboColor(1).ListIndex)
        itmx.SubItems(5) = cboColor(2).List(cboColor(2).ListIndex)
        itmx.SubItems(6) = cboLine(0).List(cboLine(0).ListIndex)
        itmx.SubItems(7) = cboLine(1).List(cboLine(1).ListIndex)
        itmx.SubItems(8) = cboLine(2).List(cboLine(2).ListIndex)
      End If
    Next i

    Unload frmGenScnAnimMod
    Call frmGenScnAnimate.SetAnimInfo
    
End Sub


Private Sub Form_Load()
    'figure out what is selected
    isel = 0
    For i = 1 To frmGenScnAnimate.lvwSegments.ListItems.Count
      If frmGenScnAnimate.lvwSegments.ListItems(i).Selected And isel = 0 Then
        isel = i
      End If
    Next i
    Set itmx = frmGenScnAnimate.lvwSegments.ListItems(isel)
    
    For i = 0 To cboColor(0).ListCount - 1
      If cboColor(0).List(i) = itmx.SubItems(3) Then
        cboColor(0).ListIndex = i
      End If
      If cboColor(1).List(i) = itmx.SubItems(4) Then
        cboColor(1).ListIndex = i
      End If
      If cboColor(2).List(i) = itmx.SubItems(5) Then
        cboColor(2).ListIndex = i
      End If
    Next i
    
    For i = 0 To cboLine(0).ListCount - 1
      If cboLine(0).List(i) = itmx.SubItems(6) Then
        cboLine(0).ListIndex = i
      End If
      If cboLine(1).List(i) = itmx.SubItems(7) Then
        cboLine(1).ListIndex = i
      End If
      If cboLine(2).List(i) = itmx.SubItems(8) Then
        cboLine(2).ListIndex = i
      End If
    Next i
    
    txtLower.Text = itmx.SubItems(1)
    txtUpper.Text = itmx.SubItems(2)

End Sub

Private Sub Form_Unload(Cancel As Integer)
    Call frmGenScnAnimate.SetFocus
End Sub

