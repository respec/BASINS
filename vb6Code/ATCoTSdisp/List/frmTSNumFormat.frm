VERSION 5.00
Object = "*\A..\..\..\VBEXPE~1\ATCoCtl\ATCoCtl.vbp"
Begin VB.Form frmTSNumFormat 
   Caption         =   "Number Format"
   ClientHeight    =   4296
   ClientLeft      =   48
   ClientTop       =   276
   ClientWidth     =   7044
   LinkTopic       =   "Form1"
   ScaleHeight     =   4296
   ScaleWidth      =   7044
   StartUpPosition =   3  'Windows Default
   Begin ATCoCtl.ATCoGrid agd 
      Height          =   4215
      Left            =   0
      TabIndex        =   0
      Top             =   0
      Width           =   6975
      _ExtentX        =   12298
      _ExtentY        =   7430
      SelectionToggle =   -1  'True
      AllowBigSelection=   -1  'True
      AllowEditHeader =   0   'False
      AllowLoad       =   0   'False
      AllowSorting    =   -1  'True
      Rows            =   3
      Cols            =   2
      ColWidthMinimum =   300
      gridFontBold    =   0   'False
      gridFontItalic  =   0   'False
      gridFontName    =   "Courier New"
      gridFontSize    =   8
      gridFontUnderline=   0   'False
      gridFontWeight  =   400
      gridFontWidth   =   0
      Header          =   ""
      FixedRows       =   1
      FixedCols       =   1
      ScrollBars      =   1
      SelectionMode   =   0
      BackColor       =   -2147483643
      ForeColor       =   -2147483640
      BackColorBkg    =   -2147483633
      BackColorSel    =   -2147483635
      ForeColorSel    =   -2147483634
      BackColorFixed  =   -2147483633
      ForeColorFixed  =   -2147483630
      InsideLimitsBackground=   16777215
      OutsideHardLimitBackground=   8421631
      OutsideSoftLimitBackground=   8454143
      ComboCheckValidValues=   -1  'True
   End
End
Attribute VB_Name = "frmTSNumFormat"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit

Private tsList As frmTSlist

Private Sub agd_CommitChange(ChangeFromRow As Long, ChangeToRow As Long, ChangeFromCol As Long, ChangeToCol As Long)
  Dim r&, c&, newValue As Long, changed As Boolean
  changed = False
  
  If Not tsList Is Nothing Then
    For r = ChangeFromRow To ChangeToRow
      For c = ChangeFromCol To ChangeToCol
        If IsNumeric(agd.TextMatrix(r, c)) Then
          newValue = CLng(agd.TextMatrix(r, c))
          If newValue >= 0 Then
            Select Case r
              Case 1
                    If tsList.FormatWidth(c) <> newValue Then
                      tsList.FormatWidth(c) = newValue
                      changed = True
                    End If
              Case 2
                    If tsList.FormatSignifDigits(c) <> newValue Then
                      tsList.FormatSignifDigits(c) = newValue
                      changed = True
                    End If
              Case 3
                    If tsList.FormatDecimalPlaces(c) <> newValue Then
                      tsList.FormatDecimalPlaces(c) = newValue
                      changed = True
                    End If
            End Select
          End If
        End If
      Next
    Next
    If changed Then tsList.PopulateGrid
  End If
End Sub

Public Sub SetHeaders(dataGrid As ATCoGrid)
  Dim r&, c&
  agd.FixedRows = dataGrid.FixedRows
  agd.rows = 3
  agd.cols = dataGrid.cols
  For r = 1 - dataGrid.FixedRows To 0
    For c = 0 To agd.cols - 1
      agd.TextMatrix(r, c) = dataGrid.TextMatrix(r, c)
    Next
  Next
  agd.TextMatrix(1, 0) = "Width"
  agd.TextMatrix(2, 0) = "Significant Digits"
  agd.TextMatrix(3, 0) = "Decimal Places"
End Sub

Public Sub SetTSlist(newTSlist As frmTSlist)
  Dim c&
  Set tsList = newTSlist
  For c = 1 To agd.cols - 1
    agd.TextMatrix(1, c) = tsList.FormatWidth(c)
    agd.TextMatrix(2, c) = tsList.FormatSignifDigits(c)
    agd.TextMatrix(3, c) = tsList.FormatDecimalPlaces(c)
    agd.ColEditable(c) = True
  Next
End Sub

Private Sub Form_Resize()
  agd.Width = Me.ScaleWidth
  agd.Height = Me.ScaleHeight
End Sub
