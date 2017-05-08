VERSION 5.00
Object = "{872F11D5-3322-11D4-9D23-00A0C9768F70}#1.4#0"; "ATCoCtl.ocx"
Begin VB.Form frmPFact 
   Caption         =   "P Factor Definition"
   ClientHeight    =   4584
   ClientLeft      =   48
   ClientTop       =   336
   ClientWidth     =   7332
   BeginProperty Font 
      Name            =   "MS Sans Serif"
      Size            =   7.8
      Charset         =   0
      Weight          =   700
      Underline       =   0   'False
      Italic          =   0   'False
      Strikethrough   =   0   'False
   EndProperty
   HelpContextID   =   23
   Icon            =   "frmPFact.frx":0000
   LinkTopic       =   "Form1"
   ScaleHeight     =   4584
   ScaleWidth      =   7332
   StartUpPosition =   3  'Windows Default
   Begin VB.CommandButton cmcPFact 
      Caption         =   "More on P"
      Height          =   372
      Index           =   2
      Left            =   4080
      TabIndex        =   22
      ToolTipText     =   "Access help on P Factor"
      Top             =   4080
      Width           =   1092
   End
   Begin VB.CommandButton cmcPFact 
      Cancel          =   -1  'True
      Caption         =   "Cancel"
      Height          =   372
      Index           =   1
      Left            =   3120
      TabIndex        =   21
      ToolTipText     =   "Return to Main Table without inserting P Factor value"
      Top             =   4080
      Width           =   732
   End
   Begin VB.CommandButton cmcPFact 
      Caption         =   "OK"
      Height          =   372
      Index           =   0
      Left            =   2160
      TabIndex        =   20
      ToolTipText     =   "Insert P Factor Value in Main Table"
      Top             =   4080
      Width           =   732
   End
   Begin VB.TextBox txtPFact 
      BackColor       =   &H8000000B&
      Height          =   288
      Left            =   4320
      Locked          =   -1  'True
      TabIndex        =   18
      Text            =   "1.0"
      Top             =   3360
      Width           =   852
   End
   Begin VB.TextBox txtPSub 
      BackColor       =   &H8000000B&
      Height          =   288
      Index           =   1
      Left            =   4320
      Locked          =   -1  'True
      TabIndex        =   14
      Text            =   "1.0"
      Top             =   2400
      Width           =   852
   End
   Begin VB.TextBox txtPSub 
      BackColor       =   &H8000000B&
      Height          =   288
      Index           =   0
      Left            =   4320
      Locked          =   -1  'True
      TabIndex        =   12
      Text            =   "1.0"
      Top             =   1164
      Width           =   852
   End
   Begin VB.Frame fraTerraces 
      Caption         =   "Terraces Used"
      Height          =   1332
      Left            =   360
      TabIndex        =   4
      Top             =   1920
      Width           =   3372
      Begin ATCoCtl.ATCoText atxPctGrade 
         Height          =   230
         Left            =   2400
         TabIndex        =   8
         Top             =   960
         Width           =   852
         _ExtentX        =   1503
         _ExtentY        =   402
         InsideLimitsBackground=   16777215
         OutsideHardLimitBackground=   8421631
         OutsideSoftLimitBackground=   8454143
         HardMax         =   1
         HardMin         =   0
         SoftMax         =   1
         SoftMin         =   0
         MaxWidth        =   -999
         Alignment       =   1
         DataType        =   0
         DefaultValue    =   ""
         Value           =   ""
         Enabled         =   -1  'True
      End
      Begin VB.OptionButton optTerraces 
         Caption         =   "Open Outlet Terraces"
         Height          =   252
         Index           =   2
         Left            =   120
         TabIndex        =   7
         Top             =   720
         Width           =   2412
      End
      Begin VB.OptionButton optTerraces 
         Caption         =   "Closed Outlet Terraces"
         Height          =   252
         Index           =   1
         Left            =   120
         TabIndex        =   6
         Top             =   480
         Width           =   2412
      End
      Begin VB.OptionButton optTerraces 
         Caption         =   "None"
         Height          =   252
         Index           =   0
         Left            =   120
         TabIndex        =   5
         Top             =   240
         Value           =   -1  'True
         Width           =   2412
      End
      Begin VB.Label lblPctGrade 
         Caption         =   "Enter Percent Grade:"
         Height          =   230
         Left            =   600
         TabIndex        =   23
         Top             =   960
         Width           =   1812
      End
   End
   Begin VB.Frame fraConservation 
      Caption         =   "Conservation Practices"
      Height          =   1092
      Left            =   360
      TabIndex        =   0
      Top             =   720
      Width           =   3372
      Begin VB.OptionButton optCons 
         Caption         =   "Strip Cropping (most effective)"
         Height          =   252
         Index           =   2
         Left            =   120
         TabIndex        =   3
         Top             =   720
         Width           =   3012
      End
      Begin VB.OptionButton optCons 
         Caption         =   "Contouring"
         Height          =   252
         Index           =   1
         Left            =   120
         TabIndex        =   2
         Top             =   480
         Width           =   3012
      End
      Begin VB.OptionButton optCons 
         Caption         =   "None"
         Height          =   252
         Index           =   0
         Left            =   120
         TabIndex        =   1
         Top             =   240
         Value           =   -1  'True
         Width           =   3012
      End
   End
   Begin VB.Label lblPFact 
      Caption         =   "Label6"
      Height          =   492
      Left            =   360
      TabIndex        =   24
      Top             =   120
      Width           =   6972
   End
   Begin VB.Line Line1 
      X1              =   4320
      X2              =   7320
      Y1              =   3120
      Y2              =   3120
   End
   Begin VB.Label Label1 
      Caption         =   "Multiplication of the above two P Sub-Factors determines the resulting P-Factor Value"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   7.8
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   852
      Left            =   5280
      TabIndex        =   19
      Top             =   3240
      Width           =   1932
   End
   Begin VB.Label lblArrows 
      Caption         =   "=====>"
      Height          =   252
      Index           =   2
      Left            =   3720
      TabIndex        =   17
      Top             =   3360
      Width           =   732
   End
   Begin VB.Label Label5 
      Alignment       =   1  'Right Justify
      Caption         =   "Resulting P-Factor Value "
      Height          =   252
      Left            =   1320
      TabIndex        =   16
      Top             =   3360
      Width           =   2412
   End
   Begin VB.Label Label4 
      Caption         =   "The combination of Terrace Type and Terrace Interval (Slope Length) determine this sub-factor"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   7.8
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   972
      Left            =   5280
      TabIndex        =   15
      Top             =   2160
      Width           =   1932
   End
   Begin VB.Label Label3 
      Caption         =   "The combination of Conservation Practice and Slope for this land segment determine this sub-factor"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   7.8
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   972
      Left            =   5280
      TabIndex        =   13
      Top             =   960
      Width           =   1932
   End
   Begin VB.Label Label2 
      Caption         =   "P Sub-Factors"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   7.8
         Charset         =   0
         Weight          =   700
         Underline       =   -1  'True
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   252
      Left            =   4200
      TabIndex        =   11
      Top             =   600
      Width           =   1212
   End
   Begin VB.Label lblArrows 
      Caption         =   "=====>"
      Height          =   252
      Index           =   1
      Left            =   3720
      TabIndex        =   10
      Top             =   2400
      Width           =   732
   End
   Begin VB.Label lblArrows 
      Caption         =   "=====>"
      Height          =   252
      Index           =   0
      Left            =   3720
      TabIndex        =   9
      Top             =   1200
      Width           =   732
   End
End
Attribute VB_Name = "frmPFact"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
Dim PCons(1 To 6, 1 To 2) As Single
Dim PTerr(1 To 6, 1 To 4) As Single
Dim Slopes(1 To 6) As Single
Dim TerInts(1 To 6) As Single
Public LSlope As Single, LTerInt As Single, TableRow As Long

Public Property Let SegmentID(newValue As String)
  lblPFact.Caption = newValue
End Property

Private Sub cmcPFact_Click(index As Integer)

  If index = 2 Then
    SendKeys "{F1}"
  Else
    If index = 0 Then 'save P-Factor value in table
      frmTable.agdTable.TextMatrix(TableRow, PCOL) = txtPFact.Text
      frmTable.txtTotal = DoSedCalcs(frmTable.agdTable, TableRow)
    End If
    Unload Me
  End If
End Sub

Private Sub Form_Load()
  PCons(1, 1) = 0.6: PCons(2, 1) = 0.5: PCons(3, 1) = 0.6
  PCons(4, 1) = 0.7: PCons(5, 1) = 0.8: PCons(6, 1) = 0.9
  PCons(1, 2) = 0.3: PCons(2, 2) = 0.25: PCons(3, 2) = 0.3
  PCons(4, 2) = 0.35: PCons(5, 2) = 0.4: PCons(6, 2) = 0.45
  PTerr(1, 1) = 0.5: PTerr(2, 1) = 0.6: PTerr(3, 1) = 0.7
  PTerr(4, 1) = 0.8: PTerr(5, 1) = 0.9: PTerr(6, 1) = 1#
  PTerr(1, 2) = 0.6: PTerr(2, 2) = 0.7: PTerr(3, 2) = 0.8
  PTerr(4, 2) = 0.8: PTerr(5, 2) = 0.9: PTerr(6, 2) = 1#
  PTerr(1, 3) = 0.7: PTerr(2, 3) = 0.8: PTerr(3, 3) = 0.9
  PTerr(4, 3) = 0.9: PTerr(5, 3) = 1#: PTerr(6, 3) = 1#
  PTerr(1, 4) = 1#: PTerr(2, 4) = 1#: PTerr(3, 4) = 1#
  PTerr(4, 4) = 1#: PTerr(5, 4) = 1#: PTerr(6, 4) = 1#
  Slopes(1) = 2.5: Slopes(2) = 8.5: Slopes(3) = 12.5
  Slopes(4) = 16.5: Slopes(5) = 20.5: Slopes(6) = 100#
  TerInts(1) = 110: TerInts(2) = 140: TerInts(3) = 180
  TerInts(4) = 225: TerInts(5) = 300: TerInts(6) = 100000
  lblPFact.Caption = "This form contains a method for estimating P Factors for Agricultural lands." & vbCrLf & "For other land uses (or for further P Factor information), click the More on P button."

End Sub

Private Sub optCons_Click(index As Integer)
  Dim i&
  If index = 0 Then 'no conservation practice in use
    txtPSub(0).Text = "1.0"
  ElseIf LSlope >= 0 Then
    i = 1
    While i < UBound(PCons)
      If LSlope < Slopes(i) Then
        txtPSub(0).Text = PCons(i, index)
        i = UBound(PCons)
      End If
      i = i + 1
    Wend
  Else 'slope not defined for this segment
    MsgBox "A Slope value has not been defined for this Land Segment." & _
           "In order to estimate the effects of Conservation Practices, it is necessary to define the Slope." & _
           "Define the Slope for this Land Segment on the Main Form before continuing with P-Factor estimation.", _
           vbExclamation, "P-Factor Definition Problem"
  End If
End Sub

Private Sub optTerraces_Click(index As Integer)
  Dim i&
  If index = 0 Then 'no terraces in use
    txtPSub(1).Text = "1.0"
  ElseIf LTerInt >= 0 Then
    If index = 2 Then
      If atxPctGrade.Value >= 0 And atxPctGrade.Value <= 1 Then
        i = 1
        While i < UBound(PTerr)
          If LTerInt < TerInts(i) Then
            txtPSub(1).Text = PTerr(i, index)
            i = UBound(PTerr)
          End If
          i = i + 1
        Wend
      Else
        MsgBox "A Percent Grade value must be entered in order to determine P-Factor adjustment for Open Outlet Terracing." & _
               "Please enter a value for the Percent Grade of the Open Outlets.", vbExclamation, "P-Factor Definition Problem"
      End If
    End If
  Else 'terrace interval (field length) not defined for this segment
    MsgBox "A Terrace Interval (equivalent to Field Length) value has not been defined for this Land Segment." & _
           "In order to estimate the effects of Terracing, it is necessary to define the Terrace Interval." & _
           "Define the Field Length for this Land Segment on the Main Form before continuing with P-Factor estimation.", _
           vbExclamation, "P-Factor Definition Problem"
  End If
End Sub

Private Sub txtPSub_Change(index As Integer)
  txtPFact.Text = NumFmted(CSng(txtPSub(0).Text) * CSng(txtPSub(1).Text), 6, 2)
End Sub
