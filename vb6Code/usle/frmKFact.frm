VERSION 5.00
Begin VB.Form frmKFact 
   Caption         =   "K Factor"
   ClientHeight    =   8280
   ClientLeft      =   132
   ClientTop       =   360
   ClientWidth     =   9120
   HelpContextID   =   19
   Icon            =   "frmKFact.frx":0000
   LinkTopic       =   "Form1"
   ScaleHeight     =   8280
   ScaleWidth      =   9120
   StartUpPosition =   3  'Windows Default
   Begin VB.TextBox txtKFactor 
      Height          =   1092
      Left            =   120
      MultiLine       =   -1  'True
      ScrollBars      =   2  'Vertical
      TabIndex        =   1
      Text            =   "frmKFact.frx":0CFA
      Top             =   120
      Width           =   8892
   End
   Begin VB.CommandButton cmdMore 
      Caption         =   "More on K"
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
      Left            =   7680
      TabIndex        =   5
      ToolTipText     =   "Access help on K Factor"
      Top             =   1320
      Width           =   1332
   End
   Begin VB.CommandButton cmdOK 
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
      Left            =   5640
      TabIndex        =   3
      ToolTipText     =   "Insert K Factor value in Main Table"
      Top             =   1320
      Width           =   852
   End
   Begin VB.CommandButton cmdCancel 
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
      Left            =   6600
      TabIndex        =   4
      ToolTipText     =   "Return to table without accepting K Factor value"
      Top             =   1320
      Width           =   972
   End
   Begin VB.PictureBox Picture1 
      AutoSize        =   -1  'True
      Height          =   6360
      Left            =   120
      Picture         =   "frmKFact.frx":0D07
      ScaleHeight     =   6312
      ScaleWidth      =   8856
      TabIndex        =   0
      TabStop         =   0   'False
      Top             =   1800
      Width           =   8904
      Begin VB.Label lblPerm 
         BackColor       =   &H80000009&
         BeginProperty Font 
            Name            =   "Courier New"
            Size            =   6
            Charset         =   0
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   1452
         Left            =   6960
         TabIndex        =   8
         Top             =   4800
         Width           =   1812
      End
      Begin VB.Label lblStructure 
         BackColor       =   &H80000009&
         BeginProperty Font 
            Name            =   "Courier New"
            Size            =   6
            Charset         =   0
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   852
         Left            =   5400
         TabIndex        =   6
         Top             =   120
         Width           =   2412
      End
   End
   Begin ATCoCtl.ATCoText txtKVal 
      Height          =   252
      Left            =   2880
      TabIndex        =   2
      Top             =   1320
      Width           =   732
      _ExtentX        =   1291
      _ExtentY        =   445
      InsideLimitsBackground=   16777215
      OutsideHardLimitBackground=   8421631
      OutsideSoftLimitBackground=   8454143
      HardMax         =   -999
      HardMin         =   -999
      SoftMax         =   -999
      SoftMin         =   -999
      MaxWidth        =   -999
      Alignment       =   1
      DataType        =   0
      DefaultValue    =   ""
      Value           =   ""
      Enabled         =   -1  'True
   End
   Begin VB.Label lblKValue 
      Alignment       =   1  'Right Justify
      Caption         =   "K Factor to insert in Main Table: "
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
      TabIndex        =   7
      Top             =   1320
      Width           =   2772
   End
End
Attribute VB_Name = "frmKFact"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
Public TableRow As Long

Private Sub cmdCancel_Click()
  Me.Hide
End Sub

Private Sub cmdMore_Click()
  SendKeys "{F1}"
End Sub

Private Sub cmdOk_Click()
  Me.Hide
  frmTable.agdTable.TextMatrix(TableRow, KCOL) = txtKVal.Value
  frmTable.txtTotal = DoSedCalcs(frmTable.agdTable, TableRow)
End Sub

Private Sub Form_Load()
  txtKFactor.Text = "The main table automatically references an extensive database of K-factor values for soil types all across the U.S.  If, however, the user decides that the database is not appropriate for the application at hand, the following nomograph can be used as an alternative approach in obtaining K-factor values.  Enter the graph at the left edge with the cumulative percentage of silt and very fine sand.  Move right to the point representing the percent sand, then vertically to the point representing the percent organic matter.  Move right again to the soil structure, as defined at the top of the second graph.  Next, move down to the permeability, as defined at the bottom of the graph, and, finally, left to the K factor on the left axis of the second graph.  An example is shown for a soil with 65% silt and very fine sand, 5% sand, 2.8% organic matter, fine granular soil structure, and slow to moderate permeability."
  lblStructure.Caption = _
      "*  1 - very fine granular" & vbCrLf & _
      "    2 - fine granular" & vbCrLf & _
      "    3 - medium or course granular" & vbCrLf & _
      "    4 - block, platy, or massive"
  lblPerm.Caption = _
      "6 - silty clay, clay" & vbCrLf & _
      "5 - silty clay loam, sandy clay" & vbCrLf & _
      "4 - sandy clay loam, clay loam" & vbCrLf & _
      "3 - loam, silt loam" & vbCrLf & _
      "2 - loamy sand, sandy loam" & vbCrLf & _
      "1 - sand"
End Sub
