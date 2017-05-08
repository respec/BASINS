VERSION 5.00
Object = "*\A..\..\VBEXPE~1\ATCoCtl\ATCoCtl.vbp"
Begin VB.Form frmLS 
   Caption         =   "LS Factor Definition"
   ClientHeight    =   2904
   ClientLeft      =   48
   ClientTop       =   276
   ClientWidth     =   6408
   HelpContextID   =   21
   Icon            =   "frmLS.frx":0000
   LinkTopic       =   "Form1"
   ScaleHeight     =   2904
   ScaleWidth      =   6408
   StartUpPosition =   3  'Windows Default
   Begin ATCoCtl.ATCoText txtIncline 
      Height          =   252
      Left            =   2040
      TabIndex        =   5
      Top             =   1920
      Width           =   732
      _ExtentX        =   1291
      _ExtentY        =   445
      InsideLimitsBackground=   16777215
      OutsideHardLimitBackground=   8421631
      OutsideSoftLimitBackground=   8454143
      HardMax         =   -999
      HardMin         =   0
      SoftMax         =   -999
      SoftMin         =   -999
      MaxWidth        =   -999
      Alignment       =   1
      DataType        =   0
      DefaultValue    =   "0"
      Value           =   "0"
      Enabled         =   -1  'True
   End
   Begin ATCoCtl.ATCoText txtLength 
      Height          =   252
      Left            =   2040
      TabIndex        =   4
      Top             =   1560
      Width           =   732
      _ExtentX        =   1291
      _ExtentY        =   445
      InsideLimitsBackground=   16777215
      OutsideHardLimitBackground=   8421631
      OutsideSoftLimitBackground=   8454143
      HardMax         =   -999
      HardMin         =   0
      SoftMax         =   -999
      SoftMin         =   -999
      MaxWidth        =   -999
      Alignment       =   1
      DataType        =   0
      DefaultValue    =   "0"
      Value           =   "0"
      Enabled         =   -1  'True
   End
   Begin VB.Frame fraRill 
      Caption         =   "Rill Susceptibility"
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
      Left            =   120
      TabIndex        =   0
      Top             =   120
      Width           =   3852
      Begin VB.OptionButton optRill 
         Caption         =   "Low (pasture, forest)"
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
         TabIndex        =   1
         Tag             =   "0.5"
         Top             =   240
         Width           =   3252
      End
      Begin VB.OptionButton optRill 
         Caption         =   "Moderate (agriculture, disturbed forest)"
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
         TabIndex        =   2
         Tag             =   "1"
         Top             =   480
         Value           =   -1  'True
         Width           =   3672
      End
      Begin VB.OptionButton optRill 
         Caption         =   "High (construction, landfill)"
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
         TabIndex        =   3
         Tag             =   "2"
         Top             =   720
         Width           =   3492
      End
   End
   Begin VB.CommandButton cmdOk 
      Caption         =   "OK"
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
      TabIndex        =   7
      ToolTipText     =   "Insert LS Factor Value in Main Table"
      Top             =   2400
      Width           =   732
   End
   Begin VB.CommandButton cmdCancel 
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
      Left            =   3960
      TabIndex        =   8
      ToolTipText     =   "Return to Main Table without inserting LS Factor value"
      Top             =   2400
      Width           =   852
   End
   Begin VB.CommandButton cmdHelp 
      Caption         =   "More on LS"
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
      Left            =   5040
      TabIndex        =   10
      ToolTipText     =   "Access help on LS Factor"
      Top             =   2400
      Width           =   1332
   End
   Begin ATCoCtl.ATCoText txtLS 
      Height          =   252
      Left            =   2040
      TabIndex        =   6
      Top             =   2472
      Width           =   732
      _ExtentX        =   1291
      _ExtentY        =   445
      InsideLimitsBackground=   16777215
      OutsideHardLimitBackground=   8421631
      OutsideSoftLimitBackground=   8454143
      HardMax         =   -999
      HardMin         =   0
      SoftMax         =   -999
      SoftMin         =   -999
      MaxWidth        =   -999
      Alignment       =   1
      DataType        =   0
      DefaultValue    =   "0"
      Value           =   "0"
      Enabled         =   -1  'True
   End
   Begin VB.Label lblRill 
      Caption         =   "Label4"
      Height          =   1092
      Left            =   4080
      TabIndex        =   13
      Top             =   240
      Width           =   2172
   End
   Begin VB.Label Label3 
      Alignment       =   1  'Right Justify
      Caption         =   "LS Factor"
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
      Left            =   360
      TabIndex        =   12
      Top             =   2470
      Width           =   1572
   End
   Begin VB.Label Label2 
      Alignment       =   1  'Right Justify
      Caption         =   "Slope Incline (%)"
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
      Left            =   240
      TabIndex        =   11
      Top             =   1920
      Width           =   1692
   End
   Begin VB.Label Label1 
      Alignment       =   1  'Right Justify
      Caption         =   "Slope Length (ft)"
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
      Left            =   240
      TabIndex        =   9
      Top             =   1560
      Width           =   1692
   End
End
Attribute VB_Name = "frmLS"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
Dim Coeff As Single

Private Sub cmdCancel_Click()
  Unload Me
End Sub

Private Sub cmdHelp_Click()
  SendKeys "{F1}"
End Sub

Private Sub cmdOk_Click()
  With frmTable.agdTable
    .TextMatrix(.row, InclineCOL) = txtIncline.Value
    .TextMatrix(.row, LengthCOL) = txtLength.Value
    .TextMatrix(.row, LSCOL) = txtLS.Value
    frmTable.txtTotal = DoSedCalcs(frmTable.agdTable, .row)
  End With
  Unload Me
End Sub

Private Sub Form_Load()
  Dim RillIndex As Long
  lblRill.Caption = "Land use has an effect on the susceptibility of " & _
      "soil to rill erosion.  The more stable the soil soil surface, " & _
      "the less rilling will occur."

  Select Case frmTable.LandUsefromRow
    Case "Permanent Grass", "Rangeland", "Forest":                             RillIndex = 0
    Case "Agriculture", "Disturbed Forest", "Parks", "Urban Areas":            RillIndex = 1
    Case "Construction Sites", "Mining Sites", "Landfills":                    RillIndex = 2
    Case Else:                                                                 RillIndex = 1
  End Select
  optRill(RillIndex).Value = True
  Coeff = optRill(RillIndex).Tag
  With frmTable.agdTable
    txtIncline.Value = .TextMatrix(.row, InclineCOL)
    If Len(.TextMatrix(.row, LengthCOL)) > 0 Then
      txtLength.Value = .TextMatrix(.row, LengthCOL)
    Else
      txtLength.Value = 72.6
    End If
    txtLS.Value = .TextMatrix(.row, LSCOL)
  End With
End Sub

Private Sub optRill_Click(index As Integer)
  Coeff = optRill(index).Tag
  ComputeLS
End Sub

Private Sub txtIncline_Change()
  ComputeLS

End Sub

Private Sub txtLength_Change()
  ComputeLS

End Sub

Private Sub txtLength_CommitChange()
  ComputeLS
End Sub

Private Sub txtIncline_CommitChange()
  ComputeLS
End Sub

Private Sub ComputeLS()
  If IsNumeric(txtIncline.Value) And IsNumeric(txtLength.Value) Then
    Debug.Print "ComputeLS: Incline=" & txtIncline.Value & " Len=" & txtLength.Value
    txtLS.Value = NumFmted(TopoFactor(txtIncline.Value, txtLength.Value, Coeff), 6, 2)
  End If
End Sub
