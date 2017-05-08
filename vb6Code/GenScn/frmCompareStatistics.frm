VERSION 5.00
Object = "*\A..\..\VBEXPE~1\ATCoCtl\ATCoCtl.vbp"
Begin VB.Form frmCompareStatistics 
   Caption         =   "Compare Timeseries"
   ClientHeight    =   4428
   ClientLeft      =   48
   ClientTop       =   336
   ClientWidth     =   6708
   LinkTopic       =   "Form1"
   ScaleHeight     =   4428
   ScaleWidth      =   6708
   StartUpPosition =   3  'Windows Default
   Begin VB.Frame fraButtons 
      BorderStyle     =   0  'None
      Caption         =   "Frame1"
      Height          =   252
      Left            =   120
      TabIndex        =   1
      Top             =   4080
      Width           =   6492
      Begin VB.CommandButton cmdSwap 
         Caption         =   "&Swap"
         Height          =   252
         Left            =   0
         TabIndex        =   3
         Top             =   0
         Width           =   732
      End
      Begin VB.CommandButton cmdClose 
         Cancel          =   -1  'True
         Caption         =   "&Close"
         Height          =   252
         Left            =   5760
         TabIndex        =   2
         Top             =   0
         Width           =   732
      End
   End
   Begin ATCoCtl.ATCoGrid agd 
      Height          =   3012
      Left            =   120
      TabIndex        =   0
      Top             =   960
      Width           =   6492
      _ExtentX        =   11451
      _ExtentY        =   5313
      SelectionToggle =   0   'False
      AllowBigSelection=   -1  'True
      AllowEditHeader =   0   'False
      AllowLoad       =   0   'False
      AllowSorting    =   0   'False
      Rows            =   1
      Cols            =   2
      ColWidthMinimum =   300
      gridFontBold    =   0   'False
      gridFontItalic  =   0   'False
      gridFontName    =   "MS Sans Serif"
      gridFontSize    =   8
      gridFontUnderline=   0   'False
      gridFontWeight  =   400
      gridFontWidth   =   0
      Header          =   ""
      FixedRows       =   1
      FixedCols       =   0
      ScrollBars      =   3
      SelectionMode   =   0
      BackColor       =   -2147483643
      ForeColor       =   -2147483640
      BackColorBkg    =   -2147483632
      BackColorSel    =   -2147483635
      ForeColorSel    =   -2147483634
      BackColorFixed  =   -2147483633
      ForeColorFixed  =   -2147483630
      InsideLimitsBackground=   -2147483643
      OutsideHardLimitBackground=   8421631
      OutsideSoftLimitBackground=   8454143
      ComboCheckValidValues=   0   'False
   End
   Begin VB.Label lblTS2 
      Caption         =   "timeseries 2"
      Height          =   252
      Left            =   240
      TabIndex        =   6
      Top             =   600
      Width           =   6252
   End
   Begin VB.Label lblVS 
      Caption         =   "vs"
      Height          =   252
      Left            =   240
      TabIndex        =   5
      Top             =   360
      Width           =   6252
   End
   Begin VB.Label lblTS1 
      Caption         =   "timeseries 1"
      Height          =   252
      Left            =   240
      TabIndex        =   4
      Top             =   120
      Width           =   6252
   End
End
Attribute VB_Name = "frmCompareStatistics"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit

Private pTScoll As Collection

Private Sub cmdClose_Click()
  Unload Me
End Sub

Private Sub cmdSwap_Click()
  Dim tmpTS As Object
  Set tmpTS = pTScoll(1)
  pTScoll.Remove 1
  pTScoll.Add tmpTS
  Recalculate
End Sub

Private Sub Form_Load()
  agd.ColTitle(0) = "Statistic"
  agd.ColTitle(1) = "Value"
End Sub

Public Sub Showcoll(TimeseriesColl As Collection)
  Set pTScoll = TimeseriesColl
  Me.Show
  Recalculate
End Sub

Private Sub Recalculate()
  Dim r As Long
  lblTS1.Caption = pTScoll(1).Header.desc
  lblTS2.Caption = pTScoll(2).Header.desc
  agd.ClearData
  r = 1
  agd.TextMatrix(r, 0) = "Correlation Coefficient (R)"
  agd.TextMatrix(r, 1) = Statistic("Correlation Coefficient", pTScoll)
  r = r + 1
  agd.TextMatrix(r, 0) = "Coefficient of Determination (R^2)"
  agd.TextMatrix(r, 1) = Statistic("Coefficient of Determination", pTScoll)
  r = r + 1
  agd.TextMatrix(r, 0) = "Mean Error"
  agd.TextMatrix(r, 1) = Statistic("Mean Error", pTScoll)
  r = r + 1
  agd.TextMatrix(r, 0) = "Mean Absolute Error"
  agd.TextMatrix(r, 1) = Statistic("Mean Absolute Error", pTScoll)
  r = r + 1
  agd.TextMatrix(r, 0) = "RMS Error"
  agd.TextMatrix(r, 1) = Statistic("rms error", pTScoll)
  r = r + 1
  agd.TextMatrix(r, 0) = "Nash Sutcliffe"
  agd.TextMatrix(r, 1) = Statistic("nash sutcliffe", pTScoll)
  r = r + 1
  agd.TextMatrix(r, 0) = "Model Fit Efficiency"
  agd.TextMatrix(r, 1) = Statistic("model fit efficiency", pTScoll)
  
End Sub
