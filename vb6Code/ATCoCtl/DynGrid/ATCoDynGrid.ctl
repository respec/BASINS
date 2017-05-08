VERSION 5.00
Begin VB.UserControl ATCoDynGrid 
   ClientHeight    =   2676
   ClientLeft      =   0
   ClientTop       =   0
   ClientWidth     =   3768
   ScaleHeight     =   223
   ScaleMode       =   3  'Pixel
   ScaleWidth      =   314
   Begin ATCoCtl.ATCoScrollbar vScroll 
      Height          =   2652
      Left            =   3480
      TabIndex        =   1
      Top             =   0
      Width           =   252
      _ExtentX        =   445
      _ExtentY        =   4678
      Enabled         =   -1  'True
      Min             =   1
      Max             =   100
      SmallChange     =   1
      LargeChange     =   10
      DragEvents      =   0   'False
      Value           =   1
   End
   Begin VB.PictureBox picGrid 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      BorderStyle     =   0  'None
      ForeColor       =   &H80000008&
      Height          =   2652
      Left            =   0
      ScaleHeight     =   221
      ScaleMode       =   3  'Pixel
      ScaleWidth      =   291
      TabIndex        =   0
      Top             =   0
      Width           =   3492
   End
End
Attribute VB_Name = "ATCoDynGrid"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit

Private pRow As Long
Private pCol As Long
Private pDataCols As Long
Private pFixedCols As Long
Private pDataRows As Long
Private pFixedRows As Long
Private pTextMatrix() As String
Private pRowHeight As Long

Private pGridLineColor As OLE_COLOR

Private Type ColSpecs
  Editable As Boolean
  Selectable As Boolean
  'CharWidth As Long
  Left As Long
  Width As Long
  DataType As ATCoDataType
  DecimalPos As Long
  MinVal As Single
  MaxVal As Single
  SoftMin As Single
  SoftMax As Single
  'Alignment As ATCoAlignment
End Type
Private pColSpec() As ColSpecs

Public Property Get cols() As Long
  cols = pDataCols
End Property
Public Property Let cols(ByVal NewValue As Long)
  pDataCols = NewValue
  RedimAll
End Property

Public Property Get rows() As Long
  rows = pDataRows
End Property
Public Property Let rows(ByVal NewValue As Long)
  pDataRows = NewValue
  ReDim pTextMatrix(pFixedRows + pDataRows, pFixedCols + pDataCols)
End Property

Public Property Get FixedCols() As Long
  FixedCols = pFixedCols
End Property
Public Property Let FixedCols(ByVal NewValue As Long)
  pFixedCols = NewValue
  RedimAll
End Property

Public Property Get FixedRows() As Long
  FixedRows = pFixedRows
End Property
Public Property Let FixedRows(ByVal NewValue As Long)
  pFixedRows = NewValue
  ReDim pTextMatrix(pFixedRows + pDataRows, pFixedCols + pDataCols)
End Property

Public Property Let TextMatrix(ByVal rowIndex&, ByVal colIndex&, NewValue$)
  Dim r As Long, c As Long
  r = rowIndex + pFixedRows
  c = colIndex + pFixedCols
  If r > 0 And r <= pFixedRows + pDataRows Then
    If c > 0 And c <= pFixedCols + pDataCols Then
      pTextMatrix(r, c) = NewValue
      DrawCell r, c
    End If
  End If
End Property
Public Property Get TextMatrix(ByVal rowIndex&, ByVal colIndex&) As String
  Dim r As Long, c As Long
  r = rowIndex + FixedRows
  c = colIndex + FixedCols
  If r > 0 And r < pFixedRows + pDataRows Then
    If c > 0 And c < pFixedCols + pDataCols Then
      TextMatrix = pTextMatrix(r, c)
    End If
  End If
End Property

Private Sub ClearCell(ByVal rowIndex&, ByVal colIndex&)
  Dim CellLeft&, CellWidth&, CellTop&
  CellLeft = pColSpec(colIndex).Left + 1
  CellWidth = pColSpec(colIndex).Width - 2
  CellTop = pRowHeight * (rowIndex - 1) + 1
  If rowIndex > pFixedRows And colIndex > pFixedCols Then 'Data cell
    picGrid.Line (CellLeft, CellTop)-Step(CellWidth, pRowHeight - 2), picGrid.BackColor, BF
  Else 'Fixed cell
    picGrid.Line (CellLeft, CellTop)-Step(CellWidth, pRowHeight - 2), vbButtonFace, BF
  End If
End Sub

Private Sub DrawCell(ByVal rowIndex&, ByVal colIndex&, Optional ClearFirst As Boolean = False)
  Dim txtWid As Long
  If ClearFirst Then ClearCell rowIndex, colIndex
  txtWid = picGrid.TextWidth(pTextMatrix(rowIndex, colIndex))
  'Select case pColSpec(colIndex).Alignment ...
  picGrid.CurrentX = pColSpec(colIndex).Left + pColSpec(colIndex).Width - txtWid
  picGrid.CurrentY = pRowHeight * (rowIndex - 1) + 5
  picGrid.Print pTextMatrix(rowIndex, colIndex)
End Sub

Public Sub RefreshGrid()
  Dim r&, c&
  picGrid.BackColor = picGrid.BackColor
  If pFixedRows > 0 Then picGrid.Line (0, 0)-(ScaleWidth, pFixedRows * pRowHeight), vbButtonFace, BF
  If pFixedCols > 0 Then picGrid.Line (0, 0)-(pColSpec(pFixedCols + 1).Left, ScaleHeight), vbButtonFace, BF
  For r = 1 To pFixedRows + pDataRows
    For c = 1 To pFixedCols + pDataCols
      DrawCell r, c
    Next
  Next
  For r = 1 To pFixedRows
    picGrid.Line (0, (r - 1) * pRowHeight)-Step(ScaleWidth, 0), vb3DLight
    picGrid.Line (0, r * pRowHeight - 1)-Step(ScaleWidth, 0), vb3DDKShadow
  Next r
  For r = pFixedRows + 1 To pFixedRows + pDataRows
    picGrid.Line (0, r * pRowHeight - 1)-Step(ScaleWidth, 0), pGridLineColor
  Next r
  For c = 1 To pFixedCols + pDataCols - 1
    picGrid.Line (pColSpec(c).Left, 0)-Step(0, ScaleHeight), pGridLineColor
  Next
End Sub

Private Sub RedimAll()
  Dim c As Long, colWidth As Long
  ReDim pColSpec(pFixedCols + pDataCols)
  ReDim pTextMatrix(pFixedRows + pDataRows, pFixedCols + pDataCols)
  colWidth = (ScaleWidth - vScroll.Width) / (pFixedCols + pDataCols)
  For c = 1 To pFixedCols + pDataCols
    With pColSpec(c)
      .Editable = False
      .Selectable = False
      .MinVal = NONE
      .MaxVal = NONE
      .SoftMin = NONE
      .SoftMax = NONE
      .Left = colWidth * (c - 1)
      .Width = colWidth
    End With
  Next
End Sub

Private Sub UserControl_Initialize()
  Dim r As Long
  pGridLineColor = vbWhite 'vb3DLight
  pFixedCols = 1
  pDataCols = 1
  pFixedRows = 1
  pDataRows = 1
  pRowHeight = picGrid.TextHeight("X") * 1.5
  RedimAll
End Sub

'Public Event RowColChange()
'Public Event SelChange(row&, col&) 'Raised when cell is selected or deselected
'Public Event TextChange(ChangeFromRow&, ChangeToRow&, ChangeFromCol&, ChangeToCol&)
'Public Event CommitChange(ChangeFromRow&, ChangeToRow&, ChangeFromCol&, ChangeToCol&)
'Public Event Click() 'user has changed which cell(s) selected with mouse or keyboard
'Public Event DoubleClick(row&, col&)
'Public Event ClickColTitle(col&, Button&)
'Public Event Sorted(col&, Ascending As Boolean)
'Public Event Error(code&, Description$)

Private Sub UserControl_ReadProperties(PropBag As PropertyBag)
'  SelectionToggle = PropBag.ReadProperty("SelectionToggle", False)
'  AllowBigSelection = PropBag.ReadProperty("AllowBigSelection", False)
'  AllowEditHeader = PropBag.ReadProperty("AllowEditHeader", False)
'  AllowLoad = PropBag.ReadProperty("AllowLoad", False)
'  AllowSorting = PropBag.ReadProperty("AllowSorting", True)
  rows = PropBag.ReadProperty("Rows", 2)
  cols = PropBag.ReadProperty("Cols", 2)
'  gridFontBold = PropBag.ReadProperty("gridFontBold", False)
'  gridFontItalic = PropBag.ReadProperty("gridFontItalic", False)
'  gridFontName = PropBag.ReadProperty("gridFontName", "MS Sans Serif")
'  gridFontSize = PropBag.ReadProperty("gridFontSize", "8.25")
'  gridFontUnderline = PropBag.ReadProperty("gridFontUnderline", False)
'  gridFontWeight = PropBag.ReadProperty("gridFontWeight", 400)
'  gridFontWidth = PropBag.ReadProperty("gridFontWidth", 0)
'  header = PropBag.ReadProperty("Header", "")
  FixedRows = PropBag.ReadProperty("FixedRows", 1)
  FixedCols = PropBag.ReadProperty("FixedCols", 0)
'  ScrollBars = PropBag.ReadProperty("ScrollBars", flexScrollBarBoth)
'  SelectionMode = PropBag.ReadProperty("SelectionMode", ASfree)
'  ColWidthMinimum = PropBag.ReadProperty("ColWidthMinimum", 300)
'
'  BackColor = PropBag.ReadProperty("BackColor", vbWindowBackground)
'  ForeColor = PropBag.ReadProperty("ForeColor", vbWindowText)
'  BackColorBkg = PropBag.ReadProperty("BackColorBkg", vb3DShadow)
'  BackColorSel = PropBag.ReadProperty("BackColorSel", vbHighlight)
'  ForeColorSel = PropBag.ReadProperty("ForeColorSel", vbHighlightText)
'  BackColorFixed = PropBag.ReadProperty("BackColorFixed", vbButtonFace)
'  ForeColorFixed = PropBag.ReadProperty("ForeColorFixed", vbButtonText)
'  InsideLimitsBackground = PropBag.ReadProperty("InsideLimitsBackground", vbWindowBackground)
'  OutsideHardLimitBackground = PropBag.ReadProperty("OutsideHardLimitBackground", 8421631) 'light red
'  OutsideSoftLimitBackground = PropBag.ReadProperty("OutsideSoftLimitBackground", 8454143) 'light yellow
'  ComboCheckValidValues = PropBag.ReadProperty("ComboCheckValidValues", True)
End Sub

Private Sub UserControl_Resize()
  Dim H As Long, W As Long
  H = Height
  W = Width
  If vScroll.Visible Then
    vScroll.Left = W - vScroll.Width
    vScroll.Height = Height
    W = W - vScroll.Width
  End If
  'If hScroll.Visible Then H = H - hScroll.Height
  picGrid.Width = Width
  picGrid.Height = Height
End Sub

Private Sub UserControl_WriteProperties(PropBag As PropertyBag)
'  PropBag.WriteProperty "SelectionToggle", SelectionToggle
'  PropBag.WriteProperty "AllowBigSelection", AllowBigSelection
'  PropBag.WriteProperty "AllowEditHeader", AllowEditHeader
'  PropBag.WriteProperty "AllowLoad", AllowLoad
'  PropBag.WriteProperty "AllowSorting", AllowSorting
'  PropBag.WriteProperty "Rows", rows
'  PropBag.WriteProperty "Cols", cols
'  PropBag.WriteProperty "ColWidthMinimum", ColWidthMinimum
'  PropBag.WriteProperty "gridFontBold", gridFontBold
'  PropBag.WriteProperty "gridFontItalic", gridFontItalic
'  PropBag.WriteProperty "gridFontName", gridFontName
'  PropBag.WriteProperty "gridFontSize", gridFontSize
'  PropBag.WriteProperty "gridFontUnderline", gridFontUnderline
'  PropBag.WriteProperty "gridFontWeight", gridFontWeight
'  PropBag.WriteProperty "gridFontWidth", gridFontWidth
'  PropBag.WriteProperty "Header", lblHeader.Caption
'  PropBag.WriteProperty "FixedRows", FixedRows
'  PropBag.WriteProperty "FixedCols", FixedCols
'  PropBag.WriteProperty "ScrollBars", ScrollBars
'  If DisjointSelections Then
'    If DisjointByRow Then
'      PropBag.WriteProperty "SelectionMode", ASdisjointByRow
'    Else
'      PropBag.WriteProperty "SelectionMode", ASdisjoint
'    End If
'  Else
'    PropBag.WriteProperty "SelectionMode", grid.SelectionMode
'  End If
'
'  PropBag.WriteProperty "BackColor", BackColor
'  PropBag.WriteProperty "ForeColor", ForeColor
'  PropBag.WriteProperty "BackColorBkg", BackColorBkg
'  PropBag.WriteProperty "BackColorSel", BackColorSel
'  PropBag.WriteProperty "ForeColorSel", ForeColorSel
'  PropBag.WriteProperty "BackColorFixed", BackColorFixed
'  PropBag.WriteProperty "ForeColorFixed", ForeColorFixed
'  PropBag.WriteProperty "InsideLimitsBackground", InsideLimitsBackground
'  PropBag.WriteProperty "OutsideHardLimitBackground", OutsideHardLimitBackground
'  PropBag.WriteProperty "OutsideSoftLimitBackground", OutsideSoftLimitBackground
'  PropBag.WriteProperty "ComboCheckValidValues", ComboCheckValidValues
End Sub

