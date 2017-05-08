VERSION 5.00
Object = "*\A..\ATCoCtl\ATCoCtl.vbp"
Object = "{831FDD16-0C5C-11D2-A9FC-0000F8754DA1}#2.0#0"; "MSCOMCTL.OCX"
Begin VB.Form frmDR 
   Caption         =   "Delivery Ratio Calculations"
   ClientHeight    =   6948
   ClientLeft      =   48
   ClientTop       =   336
   ClientWidth     =   9924
   HelpContextID   =   26
   Icon            =   "frmDR.frx":0000
   LinkTopic       =   "Form1"
   ScaleHeight     =   6948
   ScaleWidth      =   9924
   StartUpPosition =   3  'Windows Default
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
      Left            =   3600
      TabIndex        =   6
      ToolTipText     =   "Insert DR value in Main Table"
      Top             =   6480
      Width           =   852
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
      Left            =   4680
      TabIndex        =   7
      ToolTipText     =   "Return to Main Table without accepting DR value"
      Top             =   6480
      Width           =   852
   End
   Begin VB.CommandButton cmdHelp 
      Caption         =   "More on DR"
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
      Left            =   5760
      TabIndex        =   8
      ToolTipText     =   "Access help on Delivery Ratio"
      Top             =   6480
      Width           =   1332
   End
   Begin ATCoCtl.ATCoText txtDR 
      Height          =   252
      Left            =   2400
      TabIndex        =   5
      Top             =   6540
      Width           =   972
      _ExtentX        =   1715
      _ExtentY        =   445
      InsideLimitsBackground=   16777215
      OutsideHardLimitBackground=   8421631
      OutsideSoftLimitBackground=   8454143
      HardMax         =   1
      HardMin         =   0
      SoftMax         =   -999
      SoftMin         =   -999
      MaxWidth        =   -999
      Alignment       =   1
      DataType        =   2
      DefaultValue    =   ""
      Value           =   ""
      Enabled         =   -1  'True
   End
   Begin VB.Frame fraTab 
      BorderStyle     =   0  'None
      Caption         =   "Calculation"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   9.6
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   3132
      Index           =   1
      Left            =   240
      TabIndex        =   10
      Top             =   1080
      Width           =   9252
      Begin ATCoCtl.ATCoText txtArea 
         Height          =   252
         Left            =   8280
         TabIndex        =   1
         Top             =   1440
         Width           =   732
         _ExtentX        =   1291
         _ExtentY        =   445
         InsideLimitsBackground=   16777215
         OutsideHardLimitBackground=   8421631
         OutsideSoftLimitBackground=   8454143
         HardMax         =   -999
         HardMin         =   0.01
         SoftMax         =   -999
         SoftMin         =   -999
         MaxWidth        =   -999
         Alignment       =   1
         DataType        =   2
         DefaultValue    =   ""
         Value           =   ""
         Enabled         =   -1  'True
      End
      Begin ATCoCtl.ATCoText txtRelief 
         Height          =   252
         Left            =   8280
         TabIndex        =   2
         Top             =   1800
         Width           =   732
         _ExtentX        =   1291
         _ExtentY        =   445
         InsideLimitsBackground=   16777215
         OutsideHardLimitBackground=   8421631
         OutsideSoftLimitBackground=   8454143
         HardMax         =   -999
         HardMin         =   0.01
         SoftMax         =   -999
         SoftMin         =   -999
         MaxWidth        =   -999
         Alignment       =   1
         DataType        =   2
         DefaultValue    =   ""
         Value           =   ""
         Enabled         =   -1  'True
      End
      Begin ATCoCtl.ATCoText txtLength 
         Height          =   252
         Left            =   8280
         TabIndex        =   3
         Top             =   2160
         Width           =   732
         _ExtentX        =   1291
         _ExtentY        =   445
         InsideLimitsBackground=   16777215
         OutsideHardLimitBackground=   8421631
         OutsideSoftLimitBackground=   8454143
         HardMax         =   -999
         HardMin         =   0.01
         SoftMax         =   -999
         SoftMin         =   -999
         MaxWidth        =   -999
         Alignment       =   1
         DataType        =   2
         DefaultValue    =   ""
         Value           =   ""
         Enabled         =   -1  'True
      End
      Begin ATCoCtl.ATCoText txtBR 
         Height          =   252
         Left            =   8280
         TabIndex        =   4
         Top             =   2520
         Width           =   732
         _ExtentX        =   1291
         _ExtentY        =   445
         InsideLimitsBackground=   16777215
         OutsideHardLimitBackground=   8421631
         OutsideSoftLimitBackground=   8454143
         HardMax         =   -999
         HardMin         =   1
         SoftMax         =   -999
         SoftMin         =   -999
         MaxWidth        =   -999
         Alignment       =   1
         DataType        =   2
         DefaultValue    =   ""
         Value           =   ""
         Enabled         =   -1  'True
      End
      Begin VB.Label lblDRCalc 
         Caption         =   $"frmDR.frx":0CFA
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   9.6
            Charset         =   0
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   1212
         Left            =   120
         TabIndex        =   16
         Top             =   0
         Width           =   9132
      End
      Begin VB.Label lblFormula 
         Caption         =   $"frmDR.frx":0E69
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   9.6
            Charset         =   0
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   1452
         Left            =   120
         TabIndex        =   15
         Top             =   1440
         Width           =   6252
      End
      Begin VB.Label Label4 
         Alignment       =   1  'Right Justify
         Caption         =   "Bifurcation Ratio: "
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   9.6
            Charset         =   0
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   252
         Left            =   6600
         TabIndex        =   14
         Top             =   2520
         Width           =   1692
      End
      Begin VB.Label lblLength 
         Alignment       =   1  'Right Justify
         Caption         =   "Length (ft): "
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   9.6
            Charset         =   0
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   252
         Left            =   7200
         TabIndex        =   13
         Top             =   2160
         Width           =   1092
      End
      Begin VB.Label lblRelief 
         Alignment       =   1  'Right Justify
         Caption         =   "Relief (ft): "
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   9.6
            Charset         =   0
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   252
         Left            =   7200
         TabIndex        =   12
         Top             =   1800
         Width           =   1092
      End
      Begin VB.Label lblArea 
         Alignment       =   1  'Right Justify
         Caption         =   "Area (acres): "
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   9.6
            Charset         =   0
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   252
         Left            =   6720
         TabIndex        =   11
         Top             =   1440
         Width           =   1572
      End
   End
   Begin VB.Frame fraTab 
      BorderStyle     =   0  'None
      Caption         =   "Chart"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   9.6
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   4332
      Index           =   3
      Left            =   360
      TabIndex        =   20
      Top             =   960
      Width           =   6852
      Begin VB.Image imgTable 
         BorderStyle     =   1  'Fixed Single
         Height          =   3000
         Left            =   120
         Picture         =   "frmDR.frx":0F6A
         Top             =   960
         Width           =   6588
      End
      Begin VB.Label lblChart 
         Caption         =   $"frmDR.frx":2623
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   9.6
            Charset         =   0
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   972
         Left            =   120
         TabIndex        =   21
         Top             =   120
         Width           =   6612
      End
   End
   Begin VB.Frame fraTab 
      BorderStyle     =   0  'None
      Caption         =   "Graph"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   9.6
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   5172
      Index           =   2
      Left            =   240
      TabIndex        =   18
      Top             =   960
      Width           =   9372
      Begin VB.PictureBox pctGraph 
         Height          =   5052
         Left            =   2640
         Picture         =   "frmDR.frx":26F3
         ScaleHeight     =   5004
         ScaleWidth      =   6564
         TabIndex        =   22
         Top             =   120
         Width           =   6612
      End
      Begin VB.Label lblDRGraph 
         Caption         =   $"frmDR.frx":21875
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   9.6
            Charset         =   0
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   3732
         Left            =   120
         TabIndex        =   19
         Top             =   120
         Width           =   2412
      End
   End
   Begin MSComctlLib.TabStrip TabDR 
      Height          =   5772
      Left            =   120
      TabIndex        =   0
      Top             =   600
      Width           =   9612
      _ExtentX        =   16955
      _ExtentY        =   10181
      _Version        =   393216
      BeginProperty Tabs {1EFB6598-857C-11D1-B16A-00C0F0283628} 
         NumTabs         =   3
         BeginProperty Tab1 {1EFB659A-857C-11D1-B16A-00C0F0283628} 
            Caption         =   "Calculation"
            ImageVarType    =   2
         EndProperty
         BeginProperty Tab2 {1EFB659A-857C-11D1-B16A-00C0F0283628} 
            Caption         =   "Graph"
            ImageVarType    =   2
         EndProperty
         BeginProperty Tab3 {1EFB659A-857C-11D1-B16A-00C0F0283628} 
            Caption         =   "Table"
            ImageVarType    =   2
         EndProperty
      EndProperty
      BeginProperty Font {0BE35203-8F91-11CE-9DE3-00AA004BB851} 
         Name            =   "MS Sans Serif"
         Size            =   7.8
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
   End
   Begin VB.Label Label5 
      Caption         =   "Delivery Ratio (decimal):"
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
      TabIndex        =   17
      Top             =   6540
      Width           =   2292
   End
   Begin VB.Label lblTop 
      Caption         =   "lblTop"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   7.8
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   612
      Left            =   120
      TabIndex        =   9
      Top             =   120
      Width           =   10572
   End
End
Attribute VB_Name = "frmDR"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
Public TableRow As Long

Private Sub cmdCancel_Click()
  Me.Hide
End Sub

Private Sub cmdHelp_Click()
  SendKeys "{F1}"
End Sub

Private Sub cmdOk_Click()
  Me.Hide
  frmTable.agdTable.TextMatrix(TableRow, DRCOL) = NumFmted(txtDR.Value, 6, 2)
  frmTable.txtTotal = DoSedCalcs(frmTable.agdTable, TableRow)
  'save Delivery Ratio calculation elements
  If TableRow > UBound(DRElems, 2) Then
    ReDim Preserve DRElems(4, TableRow)
  End If
  DRElems(1, TableRow) = txtArea.Value
  DRElems(2, TableRow) = txtRelief.Value
  DRElems(3, TableRow) = txtLength.Value
  DRElems(4, TableRow) = txtBR.Value
End Sub

Private Sub Form_Load()
  lblTop.Caption = "Three methods are offered for arriving at a Delivery Ratio: a formula, a graph, and a table"
  lblDRCalc.Caption = "Several empirical relationships have been proposed to estimate delivery ratios based on " & _
      "different controlling factors.  The following formula was developed from a study in the Piedmont region " & _
      "of the Carolinas and Georgia.  It accounts for the dominant watershed characteristics as they pertain to " & _
      "sediment transport.  Complete the data below to obtain a DR value.  The data should pertain to the most " & _
      "specific subwatershed of the subject HUC-8 as is possible."
      
  lblFormula.Caption = "log DR = 3.59253 - 0.23043 log W + 0.51022 log R/L - 2.78594 log BR" & vbCrLf & vbCrLf & _
      "          where:  W = subwatershed area (acres)" & vbCrLf & _
      "                         R/L = relief to length ratio" & vbCrLf & _
      "                         BR = bifurcation ratio" & vbCrLf & _
      "                         DR = delivery ratio"
  lblDRGraph.Caption = "This graph illustrates the proposed relationship between the delivery ratio and the drainage density for different soil textures.  Drainage density is defined as the ratio of total stream length within the system divided by the total area of the system."
  lblChart.Caption = "A study was done for the Ryan Gulch basin of Northern Colorado.  The results may be most appropriate for basins in that area of the U.S.  The following table lists the DR values resulting from this study."
  TabDR_Click
End Sub

Private Sub RecalcDR()
  Dim lDR!
  If IsNumeric(txtArea.Value) And _
     IsNumeric(txtRelief.Value) And _
     IsNumeric(txtLength.Value) And _
     IsNumeric(txtBR.Value) Then
    If txtArea.Value > 0 And _
       txtRelief.Value > 0 And _
       txtLength.Value > 0 And _
       txtBR.Value > 0 Then
    
      lDR = (10 ^ (3.59253 - 0.23043 * Log10(txtArea.Value / 246.9) _
         + 0.51022 * Log10(txtRelief.Value / txtLength.Value) _
         - 2.78594 * Log10(txtBR.Value))) / 100
      If lDR >= 0# And lDR <= 1# Then
        txtDR.Value = lDR
      Else 'DR value is out of valid range
        MsgBox "The Delivery Ratio value (" & lDR & ") calculated using the input parameters " & vbCrLf & "is outside the acceptable range (0 < DR < 1)." & vbCrLf & "Check your input parameter values and update them to generate a valid Delivery Ratio value.", vbExclamation, "TMDL USLE Delivery Ratio Problem"
      End If
    End If
  End If

End Sub

Private Sub TabDR_Click()
  Dim t As Long
  For t = 1 To TabDR.Tabs.Count
    If t = TabDR.SelectedItem.index Then
      fraTab(t).Visible = True
    Else
      fraTab(t).Visible = False
    End If
  Next
End Sub

Private Sub txtArea_CommitChange()
  RecalcDR
End Sub
Private Sub txtBR_CommitChange()
  RecalcDR
End Sub
Private Sub txtLength_CommitChange()
  RecalcDR
End Sub
Private Sub txtRelief_CommitChange()
  RecalcDR
End Sub
