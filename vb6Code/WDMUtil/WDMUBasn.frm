VERSION 5.00
Begin VB.Form frmBasInfUpd 
   Caption         =   "BASINS Information Update"
   ClientHeight    =   2292
   ClientLeft      =   1608
   ClientTop       =   1560
   ClientWidth     =   8664
   BeginProperty Font 
      Name            =   "MS Sans Serif"
      Size            =   7.8
      Charset         =   0
      Weight          =   700
      Underline       =   0   'False
      Italic          =   0   'False
      Strikethrough   =   0   'False
   EndProperty
   HelpContextID   =   90
   Icon            =   "WDMUBasn.frx":0000
   LinkTopic       =   "Form1"
   ScaleHeight     =   2292
   ScaleWidth      =   8664
   Begin VB.CommandButton cmdClose 
      Caption         =   "Ignore"
      Height          =   372
      Index           =   2
      Left            =   4920
      TabIndex        =   5
      Top             =   1800
      Width           =   972
   End
   Begin ATCoCtl.ATCoGrid agdBasInf 
      Height          =   732
      Left            =   0
      TabIndex        =   4
      Top             =   1080
      Width           =   8412
      _ExtentX        =   14838
      _ExtentY        =   1291
      SelectionToggle =   0   'False
      AllowBigSelection=   -1  'True
      AllowEditHeader =   0   'False
      AllowLoad       =   0   'False
      AllowSorting    =   -1  'True
      Rows            =   1
      Cols            =   12
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
      BackColorBkg    =   -2147483633
      BackColorSel    =   -2147483635
      ForeColorSel    =   -2147483634
      BackColorFixed  =   -2147483633
      ForeColorFixed  =   -2147483630
      InsideLimitsBackground=   -2147483643
      OutsideHardLimitBackground=   8421631
      OutsideSoftLimitBackground=   8454143
      ComboCheckValidValues=   -1  'True
   End
   Begin VB.CommandButton cmdDelete 
      Caption         =   "Delete Row"
      Height          =   372
      Left            =   240
      TabIndex        =   3
      Top             =   1800
      Width           =   1212
   End
   Begin VB.CommandButton cmdClose 
      Caption         =   "Cancel"
      Height          =   372
      Index           =   1
      Left            =   3600
      TabIndex        =   2
      Top             =   1800
      Width           =   972
   End
   Begin VB.CommandButton cmdClose 
      Caption         =   "OK"
      Height          =   372
      Index           =   0
      Left            =   2280
      TabIndex        =   1
      Top             =   1800
      Width           =   972
   End
   Begin VB.Label lblBasUpdate 
      Caption         =   "Update BASINS information as needed.  Use OK to update BASINS information file, Cancel to not update."
      Height          =   732
      Left            =   120
      TabIndex        =   0
      Top             =   120
      Width           =   3612
   End
End
Attribute VB_Name = "frmBasInfUpd"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
'Copyright 2000 by AQUA TERRA Consultants

Private Sub cmdClose_Click(index As Integer)

  Dim i&, j&, iloc&, lnts&, lts() As Timser

  If index = 0 Then 'update basins info
    With agdBasInf
      For j = 1 To .Rows
        BasInf(j - 1).Nam = .TextMatrix(j, 0)
        BasInf(j - 1).Desc = .TextMatrix(j, 1)
        BasInf(j - 1).Elev = .TextMatrix(j, 2)
'        BasInf(j - 1).EvapCoef = .TextMatrix(j, 3)
        For i = 0 To 7
          BasInf(j - 1).ds(i).dsn = .TextMatrix(j, i + 3)
        Next i
      Next j
      NewLoc = .Rows - NLoc
    End With
    frmBasInfUpd.Tag = -1 'close down form
  ElseIf index = 1 Then 'cancel
    frmBasInfUpd.Tag = 1 'signal for main form to not shut down
  ElseIf index = 2 Then 'ignore, shut down, but don't write to BASINS inf file
    frmBasInfUpd.Tag = 0
  End If
  frmBasInfUpd.Hide

End Sub

Private Sub cmdDelete_Click()

  Dim i&

  'delete current row from grid
  Call agdBasInf.DeleteRow(agdBasInf.row)
  For i = agdBasInf.row To agdBasInf.Rows - 1
    BasInf(i - 1) = BasInf(i)
  Next i
'  With agdBasInf
'    For i = .row To .Rows - 1
'      For j = 0 To 11
'        .TextMatrix(i, j) = .TextMatrix(i + 1, j)
'      Next j
'    Next i
'    .Rows = .Rows - 1
'  End With

End Sub

Private Sub Form_Load()

  Dim i&, j&

  lblBasUpdate.Caption = "Update BASINS information as needed." & vbCrLf & _
                         "Use OK to update BASINS information file." & vbCrLf & _
                         "Use Cancel to return to the main form." & vbCrLf & _
                         "Use Ignore to Close/Exit and not update BASINS information file." & vbCrLf & _
                         "Use Delete Row to remove any locations not to be saved on BASINS information file."

  With agdBasInf
    .Rows = NLoc + NewLoc
    .ColTitle(0) = "Station ID"
    .colWidth(0) = 800
    .ColTitle(1) = "Description"
    .colWidth(1) = 1000
    .ColTitle(2) = "Elevation (ft)"
    .colWidth(2) = 900
    .ColType(2) = ATCoSng
    .ColMin(2) = -1000#
    .ColMax(2) = 25000#
'    .ColTitle(3) = "Evap. Coef."
'    .colWidth(3) = 900
'    .ColType(3) = ATCoSng
'    .ColMin(3) = 0#
'    .ColMax(3) = 1#
    .ColTitle(3) = "PREC"
    .ColTitle(4) = "EVAP"
    .ColTitle(5) = "ATEM"
    .ColTitle(6) = "WIND"
    .ColTitle(7) = "SOLR"
    .ColTitle(8) = "PEVT"
    .ColTitle(9) = "DEWP"
    .ColTitle(10) = "CLOU"
    For i = 3 To 10
      .ColType(i) = ATCoInt
      .ColMin(i) = 1
      .ColMax(i) = 9999
      .colWidth(i) = 600
    Next i
    For i = 0 To 10
      .ColEditable(i) = True
    Next i
    For i = 1 To .Rows
      .TextMatrix(i, 0) = BasInf(i - 1).Nam
      .TextMatrix(i, 1) = BasInf(i - 1).Desc
      .TextMatrix(i, 2) = BasInf(i - 1).Elev
      .TextMatrix(i, 3) = BasInf(i - 1).EvapCoef
      For j = 0 To 7
        .TextMatrix(i, j + 3) = BasInf(i - 1).ds(j).dsn
      Next j
    Next i
    .Height = (.Rows - 1) * 240 + 732
  End With
  cmdDelete.Top = agdBasInf.Top + agdBasInf.Height + 228
  cmdClose(0).Top = cmdDelete.Top
  cmdClose(1).Top = cmdDelete.Top
  cmdClose(2).Top = cmdDelete.Top
  frmBasInfUpd.Height = cmdDelete.Top + cmdDelete.Height + 500

End Sub
