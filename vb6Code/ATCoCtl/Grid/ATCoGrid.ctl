VERSION 5.00
Begin VB.UserControl ATCoGrid 
   ClientHeight    =   1860
   ClientLeft      =   0
   ClientTop       =   0
   ClientWidth     =   3990
   ScaleHeight     =   1860
   ScaleWidth      =   3990
   ToolboxBitmap   =   "ATCoGrid.ctx":0000
   Begin ATCoCtl.ATCoText txtEditHeader 
      Height          =   255
      Left            =   2040
      TabIndex        =   6
      Top             =   360
      Visible         =   0   'False
      Width           =   1455
      _ExtentX        =   2566
      _ExtentY        =   450
      InsideLimitsBackground=   16777215
      OutsideHardLimitBackground=   255
      OutsideSoftLimitBackground=   65535
      HardMax         =   -999
      HardMin         =   -999
      SoftMax         =   -999
      SoftMin         =   -999
      MaxWidth        =   5
      Alignment       =   1
      DataType        =   0
      DefaultValue    =   -999
      Value           =   "-999"
      Enabled         =   -1  'True
   End
   Begin MSComDlg.CommonDialog cdlg 
      Left            =   3360
      Top             =   1440
      _ExtentX        =   847
      _ExtentY        =   847
      _Version        =   393216
   End
   Begin ATCoCtl.ATCoText txtPopup 
      Height          =   252
      Left            =   120
      TabIndex        =   5
      Top             =   960
      Visible         =   0   'False
      Width           =   1452
      _ExtentX        =   2566
      _ExtentY        =   450
      InsideLimitsBackground=   16777215
      OutsideHardLimitBackground=   255
      OutsideSoftLimitBackground=   65535
      HardMax         =   -999
      HardMin         =   -999
      SoftMax         =   -999
      SoftMin         =   -999
      MaxWidth        =   5
      Alignment       =   1
      DataType        =   0
      DefaultValue    =   -999
      Value           =   "-999"
      Enabled         =   -1  'True
   End
   Begin VB.ComboBox comboPopup 
      Appearance      =   0  'Flat
      Height          =   315
      Left            =   120
      TabIndex        =   2
      Text            =   "comboPopup"
      Top             =   1320
      Visible         =   0   'False
      Width           =   1455
   End
   Begin VB.Frame framePopup 
      Height          =   615
      Left            =   1800
      TabIndex        =   3
      Top             =   720
      Visible         =   0   'False
      Width           =   2055
      Begin VB.Label lblPopup 
         Caption         =   "lblPopup"
         Height          =   255
         Left            =   120
         TabIndex        =   4
         Top             =   240
         Width           =   1815
      End
   End
   Begin MSFlexGridLib.MSFlexGrid grid 
      Height          =   1215
      Left            =   0
      TabIndex        =   0
      Top             =   360
      Width           =   3735
      _ExtentX        =   6588
      _ExtentY        =   2143
      _Version        =   393216
      FixedCols       =   0
      BackColorBkg    =   -2147483632
      GridColor       =   -2147483626
      GridColorFixed  =   -2147483627
      AllowBigSelection=   -1  'True
      ScrollTrack     =   -1  'True
      AllowUserResizing=   1
      BorderStyle     =   0
   End
   Begin VB.Label lblHeader 
      Caption         =   "lblHeader"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   255
      Left            =   0
      TabIndex        =   1
      Top             =   0
      Width           =   3735
      WordWrap        =   -1  'True
   End
   Begin VB.Menu mnuPopup 
      Caption         =   "grid popup menu"
      Begin VB.Menu mnuPopupRange 
         Caption         =   ""
         Visible         =   0   'False
      End
      Begin VB.Menu mnuSepRange 
         Caption         =   "-"
         Visible         =   0   'False
      End
      Begin VB.Menu mnuPopupCopy 
         Caption         =   "&Copy Selection"
      End
      Begin VB.Menu mnuPopupCopyAll 
         Caption         =   "Copy &All"
      End
      Begin VB.Menu mnuPopupPaste 
         Caption         =   "&Paste"
      End
      Begin VB.Menu mnuSep1 
         Caption         =   "-"
      End
      Begin VB.Menu mnuPopupColsSizeByContents 
         Caption         =   "Si&ze Columns by contents"
      End
      Begin VB.Menu mnuPopupColsSizeToWidth 
         Caption         =   "Fi&t Columns to width"
      End
      Begin VB.Menu mnuPopupBackColor 
         Caption         =   "&Background Color..."
      End
      Begin VB.Menu mnuPopupForeColor 
         Caption         =   "&Foreground Color..."
      End
      Begin VB.Menu mnuPopupFont 
         Caption         =   "F&ont..."
      End
      Begin VB.Menu mnuPopupSelectionToggle 
         Caption         =   "Selection To&ggle"
      End
      Begin VB.Menu mnuSep2 
         Caption         =   "-"
      End
      Begin VB.Menu mnuPopupPrint 
         Caption         =   "&Print..."
      End
      Begin VB.Menu mnuPopupSave 
         Caption         =   "&Save..."
      End
      Begin VB.Menu mnuPopupSaveNormalized 
         Caption         =   "Save &Normalized"
      End
      Begin VB.Menu mnuPopupLoad 
         Caption         =   "&Load from File..."
      End
   End
End
Attribute VB_Name = "ATCoGrid"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit
'Copyright 2001 by AQUA TERRA Consultants

'Public Enum ATCoDataType 'in ATCoText
'  ATCoTxt = 0
'  ATCoInt = 1
'  ATCoSng = 2
'  ATCoClr = 10
'  NONE = -999
'End Enum

Public Enum ATCoSelectionMode
  ASfree = 0
  ASbyRow = 1
  ASbyCol = 2
  ASdisjoint = 3
  ASdisjointByRow = 4
End Enum

Public Event RowColChange()
Public Event SelChange(row&, col&) 'Raised when cell is selected or deselected
Public Event TextChange(ChangeFromRow&, ChangeToRow&, ChangeFromCol&, ChangeToCol&)
Public Event CommitChange(ChangeFromRow&, ChangeToRow&, ChangeFromCol&, ChangeToCol&)
Public Event Click() 'user has changed which cell(s) selected with mouse or keyboard
Public Event DoubleClick(row&, col&)
Public Event ClickColTitle(col&, Button&)
Public Event Sorted(col&, Ascending As Boolean)
Public Event Error(code&, Description$)
'Public Event KeyDown(KeyCode As Integer, Shift As Integer)

Private progress1 As ATCoProgress

Private Type ColSpecs
  Editable As Boolean
  Selectable As Boolean
  CharWidth As Long
  DataType As ATCoDataType
  DecimalPos As Long
  MinVal As Single
  MaxVal As Single
  SoftMin As Single
  SoftMax As Single
  SetAlignment As Boolean
End Type

Type CellSpecs
  row As Long
  col As Long
End Type

Private Const sngSlop = 0.0000001 'floating point arithmetic is sloppy
Private Const scrollBarWidth = 270
Private Const PrintMargin = "        "
Private Const HiddenColumnTitle = "HIDE"
Private Const DefaultColCharWidthTxt = -999
Private Const DefaultColCharWidthInt = -999
Private Const DefaultColCharWidthSng = -999

Private ColSpec() As ColSpecs
Private ColWidthMin&

Private ValidValues$()
Private nValidValues&

Private ItemDataA() As Variant
Private nItemData As Long

Private DisjointSelections As Boolean, DisjointByRow As Boolean, pSelectionToggle As Boolean
Private SelectedCells As Collection  'Collection of (Row, Col) used when DisjointSelections = True
Private pSelStartCol&, pSelStartRow& 'Cell where current selection was started
Private pSelOtherCol&, pSelOtherRow& 'Cell at other corner of current selection
Private ChangeFromCol&, ChangeToCol&, ChangeFromRow&, ChangeToRow& 'in flexgrid coordinates

Private ForcePaste       As Boolean
Private pAllowEditHeader As Boolean
Private pAllowLoad       As Boolean
Private pAllowSorting    As Boolean

Private pComboCheckValidValues As Boolean
Private pAutoCompleting As Boolean 'Keeps the autocomplete functions from triggering the Change event
Private OkBg&, OutsideSoftBg&, OutsideHardBg& 'Background colors for range checking
Private MaxOccRow&                            'Maximum row number that contains data
Private ErrorHappened As Boolean
Private hdrLines&                             'number of lines of header above grid

'flexSortGenericAscending     1 Generic Ascending.  An ascending sort, which estimates whether text is string or number, is performed.
'flexSortGenericDescending    2 Generic Descending. A descending sort, which estimates whether text is string or number, is performed.
'flexSortNumericAscending     3 Numeric Ascending.  An ascending sort, which converts strings to numbers, is performed.
'flexSortNumericDescending    4 Numeric Descending. A descending sort, which converts strings to numbers, is performed.
'flexSortStringNoCaseAsending 5 String Ascending.   An ascending sort using case-insensitive string comparison is performed.
'flexSortNoCaseDescending     6 String Descending.  A descending sort using case-insensitive string comparison is performed.
'flexSortStringAscending      7 String Ascending.   An ascending sort using case-sensitive string comparison is performed.
'flexSortStringDescending     8 String Descending.  A descending sort using case-sensitive string comparison is performed.

Public Sub Sort(Column&, Ascending As Boolean)
  Dim sortMethod&
  Select Case ColSpec(Column).DataType
    Case ATCoTxt:          sortMethod = 7 'flexSortStringNoCaseAsending
    Case ATCoInt, ATCoSng: sortMethod = 3 'flexSortNumericAscending
    Case ATCoClr:          sortMethod = 0 'flexSortNone
    Case Else:             sortMethod = 1 'flexSortGenericAscending
  End Select
  If sortMethod > 0 Then
    If Not Ascending Then sortMethod = sortMethod + 1
    col = Column
    grid.Sort = sortMethod
  End If
End Sub

Public Property Get hwnd() As Long
  hwnd = UserControl.hwnd
End Property

Public Property Get DesiredHeight&()
  DesiredHeight = grid.rows * grid.RowHeight(0) + lblHeader.Height
End Property

Public Property Get DesiredWidth&()
  Dim r&, c&
  Dim thisWidth&, maxWidth&(), gridWidth&
  ReDim maxWidth(cols)
  
  UserControl.FontBold = gridFontBold
  UserControl.FontItalic = gridFontItalic
  UserControl.FontName = gridFontName
  UserControl.FontSize = gridFontSize
  UserControl.FontUnderline = gridFontUnderline
  
  For r = 0 To grid.rows - 1
    For c = 0 To grid.cols - 1
      thisWidth = UserControl.TextWidth(grid.TextMatrix(r, c))
      If thisWidth > maxWidth(c) Then maxWidth(c) = thisWidth
    Next c
  Next r
  gridWidth = 100
  For c = 0 To cols - 1
    gridWidth = gridWidth + maxWidth(c)
  Next c
  DesiredWidth = gridWidth
End Property

Public Property Let InsideLimitsBackground(ByVal NewValue&)
Attribute InsideLimitsBackground.VB_Description = "Background color for values that are inside hard and soft limits"
  OkBg = NewValue
End Property

Public Property Get InsideLimitsBackground&()
  InsideLimitsBackground = OkBg
End Property

Public Property Let OutsideSoftLimitBackground(NewValue&)
Attribute OutsideSoftLimitBackground.VB_Description = "Background color for values inside hard limits but outside soft limits"
  OutsideSoftBg = NewValue
End Property

Public Property Get OutsideSoftLimitBackground&()
  OutsideSoftLimitBackground = OutsideSoftBg
End Property

Public Property Let OutsideHardLimitBackground(NewValue&)
Attribute OutsideHardLimitBackground.VB_Description = "Background color for values outside hard limits"
  OutsideHardBg = NewValue
End Property

Public Property Get OutsideHardLimitBackground&()
  OutsideHardLimitBackground = OutsideHardBg
End Property

'True if typed value in a popup list must be one of the values in the list
'False if value may be a string not in the list (see also addValue)
Public Property Let ComboCheckValidValues(ByVal NewValue As Boolean)
  pComboCheckValidValues = NewValue
End Property

Public Property Get ComboCheckValidValues() As Boolean
  ComboCheckValidValues = pComboCheckValidValues
End Property

Public Property Let FixedCols(ByVal NewValue&)
  DebugMsg "Let FixedCols: " & NewValue, 5, "p"
'  Dim c&
'  c = grid.FixedCols + 1
  If NewValue < grid.cols Then grid.FixedCols = NewValue
'  Let Cols = Cols 'redimension ColSpec, stc.
'  While c <= grid.FixedCols
'    ColSpec(-(c - 1)).MinVal = NONE
'    ColSpec(-(c - 1)).MaxVal = NONE
'    c = c + 1
'  Wend
End Property

Public Property Get FixedCols() As Long
  FixedCols = grid.FixedCols
End Property

Public Property Let FixedRows(ByVal NewValue&)
'  Dim r&
'  For r = 0 To grid.FixedRows - 1
'    grid.MergeRow(r) = False
'  Next r
'  For r = 0 To newValue - 1
'    grid.MergeRow(r) = True
'  Next r
  DebugMsg "Let FixedRows: " & NewValue, 5, "p"
  If NewValue >= grid.rows Then rows = NewValue + 1
  grid.FixedRows = NewValue
  
End Property
Public Property Get FixedRows() As Long
  FixedRows = grid.FixedRows
End Property

Public Property Let gridFontBold(NewValue As Boolean)
  grid.Font.Bold = NewValue
End Property

Public Property Get gridFontBold() As Boolean
  gridFontBold = grid.Font.Bold
End Property

Public Property Let gridFontItalic(NewValue As Boolean)
  grid.Font.Italic = NewValue
End Property

Public Property Get gridFontItalic() As Boolean
  gridFontItalic = grid.Font.Italic
End Property

Public Property Let gridFontName(NewValue$)
  grid.Font.name = NewValue
End Property

Public Property Get gridFontName() As String
  gridFontName = grid.Font.name
End Property

Public Property Let gridFontSize(NewValue&)
  grid.Font.Size = NewValue
End Property

Public Property Get gridFontSize() As Long
  gridFontSize = grid.Font.Size
End Property

Public Property Let gridFontUnderline(NewValue As Boolean)
  grid.Font.Underline = NewValue
End Property

Public Property Get gridFontUnderline() As Boolean
  gridFontUnderline = grid.Font.Underline
End Property

Public Property Let gridFontWeight(NewValue&)
  grid.Font.Weight = NewValue
End Property

Public Property Get gridFontWeight() As Long
  gridFontWeight = grid.Font.Weight
End Property

Public Property Let gridFontWidth(NewValue&)
  grid.FontWidth = NewValue
End Property

Public Property Get gridFontWidth() As Long
  gridFontWidth = grid.FontWidth
End Property

Public Property Let header(NewValue$)
  Dim AmpersandPos&, LastAmpersandPos&
  lblHeader.Caption = ""
  DebugMsg "Let Header: " & NewValue, 5, "p"
  LastAmpersandPos = 0
  AmpersandPos = InStr(NewValue, "&")
  While AmpersandPos > 0
    lblHeader.Caption = lblHeader.Caption & Mid(NewValue, LastAmpersandPos + 1, AmpersandPos - LastAmpersandPos - 1) & vbCr
    LastAmpersandPos = AmpersandPos
    AmpersandPos = InStr(LastAmpersandPos + 1, NewValue, "&")
  Wend
  lblHeader.Caption = lblHeader.Caption & Mid(NewValue, LastAmpersandPos + 1)
  UserControl_Resize
End Property

Public Property Get header() As String
  header = lblHeader.Caption
End Property

Public Property Let ItemData(ByVal Index&, NewValue As Variant)
  Dim newsize&
  If nItemData < Index Then
    newsize = Index
    If newsize < rows Then newsize = rows
    ReDim Preserve ItemDataA(newsize)
    nItemData = newsize
  End If
  ItemDataA(Index) = NewValue
End Property

Public Property Get ItemData(ByVal Index&) As Variant
  If nItemData < Index Then ItemData = 0 Else ItemData = ItemDataA(Index)
End Property

Public Property Get ListIndex() As Long
  ListIndex = comboPopup.ListIndex
End Property

Public Property Let cols(ByVal NewValue&)
  Dim oldCols&, c&
  DebugMsg "Let Cols: " & NewValue, 5, "p"
  If NewValue < 1 Then NewValue = 1
  On Error GoTo AllocationError
  ReDim Preserve ColSpec(0 To NewValue - 1)    'ReDim Preserve ColSpec(-(FixedCols - 1) To newvalue)
  oldCols = cols
  
  For c = oldCols To NewValue - 1
    With ColSpec(c)
      .Editable = False
      .Selectable = False
      .MinVal = NONE
      .MaxVal = NONE
      .SoftMin = NONE
      .SoftMax = NONE
      .CharWidth = DefaultColCharWidthTxt
    End With
  Next c
  grid.cols = flexCol(NewValue) 'FixedCols + newvalue
  For c = oldCols To NewValue - 1
    grid.ColAlignment(flexCol(c)) = flexAlignLeftCenter 'default for text columns, will be changed for other types in Let ColType
    colWidth(c) = colWidth(0)
  Next c
  ColsSizeByContents
  If NewValue < oldCols Then CullSelectedCells
  Exit Property
  
AllocationError:
  RaiseEvent Error(1, "Let Cols: can't allocate " & NewValue & " columns with " & rows & " rows." & vbCr & Err.Description)
  ErrorHappened = True
End Property

Public Property Get cols() As Long
  cols = agdCol(grid.cols) 'grid.Cols - grid.FixedCols
End Property

Public Property Let col(ByVal NewValue&)
  DebugMsg "Let Col: " & NewValue, 7, "p"
  If NewValue >= 0 And NewValue < cols Then grid.col = flexCol(NewValue)
End Property

Public Property Get col() As Long
  col = agdCol(grid.col)
End Property

Public Property Get HadError() As Boolean
  HadError = ErrorHappened
  ErrorHappened = False
End Property

Public Property Let rows(ByVal NewValue&)
  DebugMsg "Let Rows: " & NewValue, 5, "p"
  Dim hadScrollbar As Boolean, haveScrollbar As Boolean, oldRows&
  hadScrollbar = (Not RowIsVisible(1) Or Not RowIsVisible(rows))
  If NewValue >= 0 Then
    oldRows = rows
    On Error GoTo AllocationError
    grid.rows = grid.FixedRows + NewValue
    If NewValue < MaxOccRow Then MaxOccRow = NewValue
    If NewValue < oldRows Then CullSelectedCells
  
    Select Case grid.ScrollBars
      Case flexScrollBarBoth, flexScrollBarVertical
        haveScrollbar = (Not RowIsVisible(1) Or Not RowIsVisible(rows))
        If hadScrollbar <> haveScrollbar Then ColsSizeToWidth
    End Select
  End If
  Exit Property
  
AllocationError:
  RaiseEvent Error(1, "Let Rows: can't allocate " & NewValue & " rows with " & cols & " columns." & vbCr & Err.Description)
  ErrorHappened = True
End Property

Public Property Get rows() As Long
  rows = grid.rows - grid.FixedRows
End Property

Public Property Let row(ByVal NewValue&)
  DebugMsg "Let Row: " & NewValue, 7, "p"
  If NewValue > 0 And NewValue <= rows Then grid.row = flexRow(NewValue)
End Property

Public Property Get row() As Long
  row = agdRow(grid.row)
End Property

Public Property Get MaxOccupiedRow() As Long
  MaxOccupiedRow = MaxOccRow
End Property

Public Property Let TopRow(ByVal NewValue&)
  DebugMsg "Let TopRow: " & NewValue, 7, "p"
  If NewValue > 0 Then grid.TopRow = flexRow(NewValue)
  'values that are too large will be converted to max val
End Property

Public Property Get TopRow() As Long
  TopRow = agdRow(grid.TopRow)
End Property

Public Property Get RowIsVisible(ByVal atrow&) As Boolean
  If atrow > 0 And atrow <= rows Then
    RowIsVisible = grid.RowIsVisible(flexRow(atrow))
  Else
    RowIsVisible = False
  End If
End Property

Public Property Get ScrollBars() As ScrollBarConstants
  ScrollBars = grid.ScrollBars
End Property

Public Property Let ScrollBars(NewValue As ScrollBarConstants)
  grid.ScrollBars = NewValue
End Property

Public Property Get SelStartRow() As Long
  If DisjointSelections Then
    If pSelStartRow < pSelOtherRow Or pSelOtherRow < 1 Then
      SelStartRow = pSelStartRow
    Else
      SelStartRow = pSelOtherRow
    End If
  Else
    If grid.RowSel < grid.row Then
      SelStartRow = agdRow(grid.RowSel)
    Else
      SelStartRow = agdRow(grid.row)
    End If
  End If
End Property

Public Property Get SelEndRow() As Long
  If DisjointSelections Then
    If pSelStartRow > pSelOtherRow Or pSelOtherRow < 1 Then
      SelEndRow = pSelStartRow
    Else
      SelEndRow = pSelOtherRow
    End If
  Else
    If grid.RowSel > grid.row Then
      SelEndRow = agdRow(grid.RowSel)
    Else
      SelEndRow = agdRow(grid.row)
    End If
  End If
End Property

Public Property Get SelStartCol() As Long
  If DisjointSelections Then
    If pSelStartCol < pSelOtherCol Then
      SelStartCol = pSelStartCol
    Else
      SelStartCol = pSelOtherCol
    End If
  Else
    If grid.ColSel < grid.col Then
      SelStartCol = agdCol(grid.ColSel)
    Else
      SelStartCol = agdCol(grid.col)
    End If
  End If
End Property

Public Property Get SelEndCol() As Long
  If grid.ColSel > grid.col Then
    SelEndCol = agdCol(grid.ColSel)
  Else
    SelEndCol = agdCol(grid.col)
  End If
End Property

Private Function InvalidCell(atrow&, atcol&)
  If atrow < 1 Then
    InvalidCell = True
  ElseIf atrow > rows Then
    InvalidCell = True
  ElseIf atcol < 0 Then
    InvalidCell = True
  ElseIf atcol >= cols Then
    InvalidCell = True
  Else
    InvalidCell = False
  End If

End Function

Public Property Let Selected(ByVal atrow&, ByVal atcol&, NewValue As Boolean)
  Dim minCol&, maxCol&
  Dim newCellInfo As CellSpecs
  Dim Index&
  Dim oldvalue As Boolean
  Static AlreadyRaisingClick As Boolean 'Raising the click event can trigger setting Selected, this flag avoids infinite recursion

  If InvalidCell(atrow, atcol) Then Exit Property

  If DisjointSelections Then
    Dim oldRow&, oldCol&, oldtoprow&, c
    oldRow = grid.row
    oldCol = grid.col
    oldtoprow = grid.TopRow
    If DisjointByRow Then
      minCol = flexCol(0)
      maxCol = flexCol(cols - 1)
      grid.Visible = False
    Else
      minCol = flexCol(atcol)
      maxCol = minCol
    End If
    grid.row = flexRow(atrow)
    newCellInfo.row = atrow
    For c = minCol To maxCol
      grid.col = c
      oldvalue = (CellForeColor = ForeColorSel) And (CellBackColor = BackColorSel)
      If oldvalue <> NewValue Then
        newCellInfo.col = agdCol(c)
        If NewValue Then
          CellForeColor = ForeColorSel
          CellBackColor = BackColorSel
          SelectedCells.Add newCellInfo
        Else
          CellForeColor = -1 'ForeColor return to normal
          CellBackColor = -1 'BackColor return to normal
          For Index = SelectedCells.Count To 1 Step -1
            If SelectedCells(Index).row = newCellInfo.row And SelectedCells(Index).col = newCellInfo.col Then
              SelectedCells.Remove Index
              Index = 0
            End If
          Next
        End If
        If Not AlreadyRaisingClick Then
          AlreadyRaisingClick = True
          If NewValue Then
            If newCellInfo.row < pSelStartRow Then pSelStartRow = newCellInfo.row
            If newCellInfo.col < pSelStartCol Then pSelStartCol = newCellInfo.col
            If newCellInfo.row > pSelOtherRow Then pSelOtherRow = newCellInfo.row
            If newCellInfo.col > pSelOtherCol Then pSelOtherCol = newCellInfo.col
          End If
          RaiseEvent SelChange(newCellInfo.row, newCellInfo.col)
          RaiseEvent Click
          AlreadyRaisingClick = False
        End If
      End If
    Next c
    grid.row = oldRow
    grid.col = oldCol
    grid.TopRow = oldtoprow
    grid.Visible = True
    CullSelectedCells
  Else
    If NewValue Then
      Select Case SelectionMode
        Case ASfree:
          grid.row = flexRow(atrow)
          grid.col = flexCol(atcol)
        Case ASbyRow:
          grid.row = flexRow(atrow)
          grid.col = 0
          grid.ColSel = grid.cols - 1
        Case ASbyCol:
          grid.col = flexCol(atcol)
          grid.row = flexRow(1)
          grid.RowSel = flexRow(rows)
      End Select
    End If
  End If
End Property

Public Property Get Selected(ByVal atrow&, ByVal atcol&) As Boolean
  Selected = False
  If DisjointSelections Then
    If SelectedCells.Count < 200 Then
      Dim Index&
      For Index = 1 To SelectedCells.Count
        If SelectedCells(Index).row = atrow And SelectedCells(Index).col = atcol Then
          Selected = True
          Index = SelectedCells.Count
        End If
      Next
    Else
      Dim oldR&, oldC&
      oldR = grid.row
      oldC = grid.col
      row = atrow
      col = atcol
      If grid.CellBackColor = BackColorSel Then Selected = True
      grid.row = oldR
      grid.col = oldC
    End If
  Else
    If atrow >= SelStartRow And atrow <= SelEndRow And atcol >= SelStartCol And atcol <= SelEndCol Then Selected = True
  End If
End Property

Public Sub DeselectAll()
  DeselectAllBut -999, -999

'  Dim oldRow&, oldCol&, oldtoprow&, oldRowSel&, oldColSel&
'  oldRow = grid.row
'  oldCol = grid.col
'  oldRowSel = grid.RowSel
'  oldColSel = grid.ColSel
'  oldtoprow = grid.TopRow
'
'  grid.row = flexRow(1)
'  grid.col = Me.FixedCols
'  grid.RowSel = flexRow(rows)
'  grid.ColSel = Me.cols - Me.FixedCols - 1
'  grid.CellForeColor = 0
'  grid.CellBackColor = 0
'
'  grid.row = oldRow
'  grid.col = oldCol
'  grid.RowSel = oldRowSel
'  grid.ColSel = oldColSel
'  grid.TopRow = oldtoprow
'
'  Set SelectedCells = Nothing
'  Set SelectedCells = New Collection
End Sub

Private Sub DeselectAllBut(selectRow&, selectCol&)
  Dim oldRow&, oldCol&, oldtoprow&, oldcount&
  Dim r&, c&
  Dim Index&
  
  oldRow = grid.row
  oldCol = grid.col
  oldtoprow = grid.TopRow
  Index = 1
  While SelectedCells.Count > Index - 1
    r = SelectedCells(Index).row
    c = SelectedCells(Index).col
    If r <> selectRow Or c <> selectCol Then
      oldcount = SelectedCells.Count
      Selected(r, c) = False
      If oldcount = SelectedCells.Count Then 'nothing got deselected
        'Sometimes we have something marked as selected that isn't
        If r = SelectedCells(Index).row And c = SelectedCells(Index).col Then
          SelectedCells.Remove Index
        End If
      End If
    Else
      Index = Index + 1
    End If
  Wend
  
  If grid.row <> oldRow Then grid.row = oldRow
  If grid.col <> oldCol Then grid.col = oldCol
  If grid.TopRow <> oldtoprow Then grid.TopRow = oldtoprow
'  Dim oldRow&, oldCol&, oldtoprow&
'  Dim r&, c&
'  Dim vCell As Variant
'  Dim newSelectedCells As Collection
'  Set newSelectedCells = New Collection
'
'  oldRow = grid.row
'  oldCol = grid.col
'  oldtoprow = grid.TopRow
'
'  For Each vCell In SelectedCells
'    r = vCell.row
'    c = vCell.col
'    If r <> selectRow Or c <> selectCol Then
'      grid.row = flexRow(r)
'      grid.col = flexCol(c)
'      grid.CellForeColor = 0
'      grid.CellBackColor = 0
'    Else
'      newSelectedCells.Add vCell
'    End If
'  Next
'  Set SelectedCells = Nothing
'  Set SelectedCells = newSelectedCells
'  RaiseEvent SelChange(selectRow, selectCol)
'
'  If grid.row <> oldRow Then grid.row = oldRow
'  If grid.col <> oldCol Then grid.col = oldCol
'  If grid.TopRow <> oldtoprow Then grid.TopRow = oldtoprow
End Sub

'Counts selected cells (rows x columns)
Public Property Get SelCount() As Long
  If DisjointSelections Then
    SelCount = SelectedCells.Count
  Else
    SelCount = (Abs(SelEndCol - SelStartCol) + 1) * (Abs(SelEndRow - SelStartRow) + 1)
  End If
End Property

Public Property Let SelectionMode(ByVal NewValue As ATCoSelectionMode)
  '0 ASfree      Allows selections to be made normally, spreadsheet-style. (default)
  '1 ASbyRow     Forces selections to span entire rows, as in a multi-column list-box or record-based display.
  '2 ASbyColumn  Forces selections to span entire columns, as if selecting ranges for a chart or fields for sorting.
  '3 ASdisjoint  not using flexGrid's spreadsheet selection mechanism at all, (de)select individual cells by clicking
  '3 ASdisjointByRow (de)select entire rows by clicking (columns must be selectable)
  If NewValue = ASdisjointByRow Then DisjointByRow = True Else DisjointByRow = False
  If NewValue = ASdisjoint Or NewValue = ASdisjointByRow Then
    DisjointSelections = True
    grid.SelectionMode = 0
    grid.HighLight = flexHighlightNever
  Else
    DisjointSelections = False
    grid.SelectionMode = NewValue
    grid.HighLight = flexHighlightAlways
  End If
End Property

Public Property Get SelectionMode() As ATCoSelectionMode
  If DisjointSelections Then
    If DisjointByRow Then SelectionMode = ASdisjointByRow Else SelectionMode = ASdisjoint
  Else
    SelectionMode = grid.SelectionMode
  End If
End Property

Public Property Let SelectionToggle(ByVal NewValue As Boolean)
  pSelectionToggle = NewValue
End Property

Public Property Get SelectionToggle() As Boolean
  SelectionToggle = pSelectionToggle
End Property

Public Property Let AllowBigSelection(ByVal NewValue As Boolean)
  grid.AllowBigSelection = NewValue
End Property

Public Property Get AllowBigSelection() As Boolean
  AllowBigSelection = grid.AllowBigSelection
End Property

Public Property Let AllowEditHeader(ByVal NewValue As Boolean)
  pAllowEditHeader = NewValue
End Property

Public Property Get AllowEditHeader() As Boolean
  AllowEditHeader = pAllowEditHeader
End Property

Public Property Let AllowLoad(ByVal NewValue As Boolean)
  pAllowLoad = NewValue
  mnuPopupLoad.Visible = NewValue
End Property

Public Property Get AllowLoad() As Boolean
  AllowLoad = pAllowLoad
End Property

Public Property Let AllowSorting(ByVal NewValue As Boolean)
  pAllowSorting = NewValue
End Property

Public Property Get AllowSorting() As Boolean
  AllowSorting = pAllowSorting
End Property

Public Property Let ForeColor(ByVal NewValue As OLE_COLOR)
  DebugMsg "Let ForeColor: " & NewValue, 5, "p"
  grid.ForeColor = NewValue
  'If DisjointSelections Then grid.ForeColorSel = newvalue
End Property

Public Property Get ForeColor() As OLE_COLOR
  ForeColor = grid.ForeColor
End Property

Public Property Let ForeColorFixed(ByVal NewValue As OLE_COLOR)
  DebugMsg "Let ForeColorFixed: " & NewValue, 5, "p"
  grid.ForeColorFixed = NewValue
End Property

Public Property Get ForeColorFixed() As OLE_COLOR
  ForeColorFixed = grid.ForeColorFixed
End Property

Public Property Let ForeColorSel(ByVal NewValue As OLE_COLOR)
  DebugMsg "Let ForeColorSel: " & NewValue, 5, "p"
  grid.ForeColorSel = NewValue
'  ForeColorSelp = newValue
End Property

Public Property Get ForeColorSel() As OLE_COLOR
  ForeColorSel = grid.ForeColorSel
'  ForeColorSel = ForeColorSelp
End Property

Public Property Get gridTop&()
  gridTop = grid.Top
End Property

'Width of visible plus scrolling area occupied by columns
Public Property Get gridWidth&()
  If cols < 1 Then
    gridWidth = 0
  Else
    gridWidth = grid.colPos(grid.cols - 1) + grid.colWidth(grid.cols - 1)
  End If
End Property

Public Property Let BackColor(ByVal NewValue As OLE_COLOR)
  DebugMsg "Let BackColor: " & NewValue, 5, "p"
  grid.BackColor = NewValue
  'If DisjointSelections Then grid.BackColorSel = newvalue
End Property

Public Property Get BackColor() As OLE_COLOR
  BackColor = grid.BackColor
End Property

Public Property Let BackColorBkg(ByVal NewValue As OLE_COLOR)
  DebugMsg "Let BackColorBkg: " & NewValue, 5, "p"
  grid.BackColorBkg = NewValue
End Property

Public Property Get BackColorBkg() As OLE_COLOR
  BackColorBkg = grid.BackColorBkg
End Property

Public Property Let BackColorFixed(ByVal NewValue As OLE_COLOR)
  DebugMsg "Let BackColorFixed: " & NewValue, 5, "p"
  grid.BackColorFixed = NewValue
End Property

Public Property Get BackColorFixed() As OLE_COLOR
  BackColorFixed = grid.BackColorFixed
End Property

Public Property Let BackColorSel(ByVal NewValue As OLE_COLOR)
  DebugMsg "Let BackColorSel: " & NewValue, 5, "p"
  grid.BackColorSel = NewValue
  'BackColorSelp = newValue
End Property

Public Property Get BackColorSel() As OLE_COLOR
  BackColorSel = grid.BackColorSel 'p
End Property

Public Property Let CellForeColor(ByVal NewValue As OLE_COLOR)
  DebugMsg "Let CellForeColor: " & NewValue, 5, "p"
  If NewValue = -1 Then
    grid.CellForeColor = 0
  Else
    grid.CellForeColor = NewValue
  End If
End Property

Public Property Get CellForeColor() As OLE_COLOR
  CellForeColor = grid.CellForeColor
End Property

Public Property Let CellBackColor(ByVal NewValue As OLE_COLOR)
  DebugMsg "Let CellBackColor: " & NewValue, 5, "p"
  If NewValue = 0 Then     'flexGrid: CellBackColor set to 0 means return it to normal backcolor
    grid.CellBackColor = 1 '1 is still black
  ElseIf NewValue = -1 Then
    grid.CellBackColor = 0
  Else
    grid.CellBackColor = NewValue
  End If
End Property

Public Property Get CellBackColor() As OLE_COLOR
  CellBackColor = grid.CellBackColor
End Property

Public Property Let TextForeColor(NewValue As OLE_COLOR)
  DebugMsg "Let TextForeColor: " & NewValue, 5, "p"
  txtPopup.ForeColor = NewValue
End Property

Public Property Get TextForeColor() As OLE_COLOR
  TextForeColor = txtPopup.ForeColor
End Property

Public Property Let TextBackColor(NewValue As OLE_COLOR)
  DebugMsg "Let TextBackColor: " & NewValue, 5, "p"
  txtPopup.BackColor = NewValue
End Property

Public Property Get TextBackColor() As OLE_COLOR
  TextBackColor = txtPopup.BackColor
End Property

Public Property Let Text(NewValue$)
  DebugMsg "Let Text: " & NewValue, 6, "p"
  If txtPopup.Visible Then
    txtPopup.Value = NewValue
  Else
    grid.Text = NewValue
  End If
End Property

Public Property Get Text() As String
  If txtPopup.Visible Then
    Text = txtPopup.Value
  Else
    Text = grid.Text
  End If
End Property

Private Sub TestValueSetCellBackColor(ByVal Value!, ByVal r&, ByVal c&)
  With ColSpec(c)
    If r > 0 And col >= FixedCols Then
      row = r
      col = c
      If .MinVal <> NONE And (.MinVal - Value) > sngSlop Or .MaxVal <> NONE And (Value - .MaxVal) > sngSlop Then
        CellBackColor = OutsideHardBg
      ElseIf .SoftMin <> NONE And (.SoftMin - Value) > sngSlop Or .SoftMax <> NONE And (Value - .SoftMax) > sngSlop Then
        CellBackColor = OutsideSoftBg
      Else
        'If OkBg = BackColor Then CellBackColor = 0 Else CellBackColor = OkBg
        CellBackColor = -1 'unset special cell back color
      End If
    End If
  End With
End Sub

Public Property Let TextMatrix(ByVal rowIndex&, ByVal colIndex&, NewValue$)
  Dim thisDecimalPos As Long
  Dim formattedValue As String
  DebugMsg "Let TextMatrix(" & rowIndex & ", " & colIndex & "): " & NewValue, 6, "p"
  If flexRow(rowIndex) < 0 Then Exit Property
  If flexCol(colIndex) < 0 Then Exit Property
  If rowIndex > rows Then rows = rowIndex: DebugMsg "Let TextMatrix increased rows to " & rows, 4, "p"
  If colIndex >= cols Then cols = colIndex + 1: DebugMsg "Let TextMatrix increased cols to " & cols, 4, "p"
  If MaxOccRow < agdRow(rowIndex) Then MaxOccRow = agdRow(rowIndex)
  If Len(NewValue) > 0 Then
    Select Case ColType(colIndex)
    Case ATCoSng:
      Dim tmpSng!
      If IsNumeric(NewValue) Then
        tmpSng = CSng(NewValue)
        TestValueSetCellBackColor tmpSng, rowIndex, colIndex
        If ColSpec(colIndex).DecimalPos > 0 And rowIndex > 0 Then
          thisDecimalPos = InStr(NewValue, ".")
          If thisDecimalPos = 0 Then thisDecimalPos = Len(NewValue) + 1
          If thisDecimalPos < ColSpec(colIndex).DecimalPos Then
            formattedValue = Space(ColSpec(colIndex).DecimalPos - thisDecimalPos) & NewValue
          Else
            formattedValue = NewValue
          End If
          grid.TextMatrix(flexRow(rowIndex), flexCol(colIndex)) = formattedValue
        Else
          grid.TextMatrix(flexRow(rowIndex), flexCol(colIndex)) = tmpSng
        End If
      Else
        If rowIndex > 0 Then
          row = rowIndex
          col = colIndex
          CellBackColor = OutsideHardBg
        End If
        grid.TextMatrix(flexRow(rowIndex), flexCol(colIndex)) = NewValue
      End If
    Case ATCoInt:
      Dim tmpLng&
      If IsNumeric(NewValue) Then
        tmpLng = CLng(NewValue)
        grid.TextMatrix(flexRow(rowIndex), flexCol(colIndex)) = tmpLng
        TestValueSetCellBackColor tmpLng, rowIndex, colIndex
      Else
        row = rowIndex
        col = colIndex
        CellBackColor = OutsideHardBg
        grid.TextMatrix(flexRow(rowIndex), flexCol(colIndex)) = NewValue
      End If
    Case ATCoClr:
      row = rowIndex
      col = colIndex
      If IsNumeric(NewValue) Then
        tmpLng = CLng(NewValue)
        grid.Text = ""
      Else
        tmpLng = TextOrNumericColor(NewValue)
        grid.Text = NewValue
      End If
      CellBackColor = tmpLng
      If Brightness(tmpLng) > 0.3 Then CellForeColor = vbBlack Else CellForeColor = vbWhite
    Case Else:
      If ColSpec(colIndex).DecimalPos > 0 And rowIndex > 0 Then
        thisDecimalPos = InStr(NewValue, ".")
        If thisDecimalPos = 0 Then thisDecimalPos = Len(NewValue) + 1
        If thisDecimalPos < ColSpec(colIndex).DecimalPos Then
          formattedValue = Space(ColSpec(colIndex).DecimalPos - thisDecimalPos) & NewValue
        Else
          formattedValue = NewValue
        End If
        grid.TextMatrix(flexRow(rowIndex), flexCol(colIndex)) = formattedValue
      Else
        grid.TextMatrix(flexRow(rowIndex), flexCol(colIndex)) = NewValue
      End If
    End Select
  Else
    grid.TextMatrix(flexRow(rowIndex), flexCol(colIndex)) = NewValue
  End If
End Property

Public Property Get TextMatrix(ByVal rowIndex&, ByVal colIndex&) As String
  If flexRow(rowIndex) < 0 Then
    TextMatrix = ""
  ElseIf flexCol(colIndex) < 0 Then
    TextMatrix = ""
  ElseIf rowIndex > rows Then
    TextMatrix = ""
  ElseIf colIndex >= cols Then
    TextMatrix = ""
  Else
    TextMatrix = grid.TextMatrix(flexRow(rowIndex), flexCol(colIndex))
  End If
End Property

'flexAlignLeftTop      0 The cell content is aligned left, top.
'flexAlignLeftCenter   1 The cell content is aligned left, center. This is the default for strings.
'flexAlignLeftBottom   2 The cell content is aligned left, bottom.
'flexAlignCenterTop    3 The cell content is aligned center, top.
'flexAlignCenterCenter 4 The cell content is aligned center, center.
'flexAlignCenterBottom 5 The cell content is aligned center, bottom.
'flexAlignRightTop     6 The cell content is aligned right, top.
'flexAlignRightCenter  7 The cell content is aligned right, center. This is the default for numbers.
'flexAlignRightBottom  8 The cell content is aligned right, bottom.
'flexAlignGeneral      9 The cell content is of general alignment. This is "left, center" for strings and "right, center" for numbers.
'Public Property Let CellAlignment(row As Long, col As Long, newvalue As Long)
'  Dim oldFixed As Long, oldRow As Long, oldCol As Long
'  'If row < 1 Then oldFixed = FixedRows: FixedRows = 0
'  oldRow = grid.row
'  oldCol = grid.col
'  grid.row = flexRow(row)
'  grid.col = flexCol(col)
'  grid.CellAlignment = newvalue
'  grid.row = oldRow
'  grid.col = oldCol
'  'If row < 1 Then FixedRows = oldFixed
'End Property

Public Property Let ColAlignment(col As Long, NewValue As Long)
  grid.ColAlignment(flexCol(col)) = NewValue
  ColSpec(col).SetAlignment = True
End Property

Public Property Get ColTitle(ByVal Column&) As String
  If Column >= LBound(ColSpec) And Column <= UBound(ColSpec) And grid.rows > 0 Then
    ColTitle = grid.TextMatrix(0, flexCol(Column))
  Else
    ColTitle = "No Column #" & CStr(Column)
  End If
End Property

Public Property Let ColTitle(ByVal Column&, newTitle$)
  Dim assignTitle$
  DebugMsg "Let ColTitle(" & Column & "): " & newTitle, 5, "p"
  If cols <= Column Then cols = Column + 1
  If grid.rows < 1 Then grid.rows = 1
  If Column >= LBound(ColSpec) And Column <= UBound(ColSpec) Then
    grid.TextMatrix(0, flexCol(Column)) = newTitle
    If newTitle = HiddenColumnTitle Then colWidth(Column) = 0
  End If
End Property

Public Property Get ColEditable(ByVal Column&) As Boolean
  If Column >= LBound(ColSpec) And Column <= UBound(ColSpec) Then
    ColEditable = ColSpec(Column).Editable
  Else
    ColEditable = False
  End If
End Property

'Sets whether a column can be selected if SelectionMode = ASdisjoint (default false for all cols)
Public Property Let ColEditable(ByVal Column&, Editable As Boolean)
  DebugMsg "Let ColEditable(" & Column & "): " & Editable, 5, "p"
  If cols <= Column Then cols = Column + 1
  If Column >= LBound(ColSpec) And Column <= UBound(ColSpec) Then
    ColSpec(Column).Editable = Editable
  End If
End Property

Public Property Get ColSelectable(ByVal Column&) As Boolean
  If Column >= LBound(ColSpec) And Column <= UBound(ColSpec) Then
    ColSelectable = ColSpec(Column).Selectable
  Else
    ColSelectable = False
  End If
End Property

Public Property Let ColSelectable(ByVal Column&, Selectable As Boolean)
  DebugMsg "Let ColSelectable(" & Column & "): " & Selectable, 5, "p"
  If cols <= Column Then cols = Column + 1
  If Column >= LBound(ColSpec) And Column <= UBound(ColSpec) Then
    ColSpec(Column).Selectable = Selectable
  End If
End Property

Public Property Get colWidth&(ByVal Column&)
  If Column >= LBound(ColSpec) And Column <= UBound(ColSpec) Then
    colWidth = grid.colWidth(flexCol(Column))
  Else
    colWidth = -1
  End If
End Property

'Call with colWidth = -1 for default width
Public Property Let colWidth(ByVal Column&, ByVal newWidth&)
  DebugMsg "Let ColWidth(" & Column & "): " & newWidth, 5, "p"
  If cols <= Column Then cols = Column + 1
  If Column >= LBound(ColSpec) And Column <= UBound(ColSpec) Then
    If ColTitle(Column) = HiddenColumnTitle Then
      grid.colWidth(flexCol(Column)) = 0
    ElseIf newWidth >= 0 Then
      grid.colWidth(flexCol(Column)) = newWidth
    End If
  End If
End Property

Public Property Get colCharWidth&(ByVal Column&)
  If Column >= LBound(ColSpec) And Column <= UBound(ColSpec) Then
    colCharWidth = ColSpec(Column).CharWidth
  Else
    colCharWidth = -1
  End If
End Property

'Call with colWidth = -1 to use default width
Public Property Let colCharWidth(ByVal Column&, ByVal newWidth&)
  DebugMsg "Let ColCharWidth(" & Column & "): " & newWidth, 5, "p"
  If cols <= Column Then cols = Column + 1
  If Column >= LBound(ColSpec) And Column <= UBound(ColSpec) Then
    ColSpec(Column).CharWidth = newWidth
  End If
End Property

Public Property Get colDecimalPos&(ByVal Column&)
  If Column >= LBound(ColSpec) And Column <= UBound(ColSpec) Then
    colDecimalPos = ColSpec(Column).DecimalPos
  Else
    colDecimalPos = 0
  End If
End Property

'Call with newDecimal = 0 to disable decimal positioning
'This scheme only works when the grid is using a fixed-width font like Courier
'Example use from TSDisp:
'  .colDecimalPos(c) = Fix(Log10(Fix(MaxValue))) + 2
'  'space for negative - sign
'  If MinValue < 0 Then .colDecimalPos(c) = .colDecimalPos(c) + 1
Public Property Let colDecimalPos(ByVal Column&, ByVal newDecimal&)
  DebugMsg "Let colDecimalPos(" & Column & "): " & newDecimal, 5, "p"
  If cols <= Column Then cols = Column + 1
  If Column >= LBound(ColSpec) And Column <= UBound(ColSpec) Then
    ColSpec(Column).DecimalPos = newDecimal
  End If
End Property

Public Property Get ColWidthMinimum&()
  ColWidthMinimum = ColWidthMin
End Property

Public Property Let ColWidthMinimum(ByVal newWidth&)
  ColWidthMin = newWidth
End Property

Public Property Get ColType(ByVal Column&) As ATCoDataType
  If Column >= LBound(ColSpec) And Column <= UBound(ColSpec) Then
    ColType = ColSpec(Column).DataType
  Else
    ColType = NONE
  End If
End Property

Public Property Let ColType(ByVal Column&, DataType As ATCoDataType)
  DebugMsg "Let ColType(" & Column & "): " & txtPopup.ATCoTypeString(DataType), 5, "p"
  If cols <= Column Then cols = Column + 1
  If Column >= LBound(ColSpec) And Column <= UBound(ColSpec) Then
    ColSpec(Column).DataType = DataType
    If DataType = ATCoTxt Then
      If Not ColSpec(Column).SetAlignment Then grid.ColAlignment(flexCol(Column)) = flexAlignLeftCenter
      ColSpec(Column).CharWidth = DefaultColCharWidthTxt
    Else
      If Not ColSpec(Column).SetAlignment Then grid.ColAlignment(flexCol(Column)) = flexAlignRightCenter
      Select Case DataType
        Case ATCoInt: ColSpec(Column).CharWidth = DefaultColCharWidthInt
        Case ATCoSng: ColSpec(Column).CharWidth = DefaultColCharWidthSng
      End Select
    End If
  End If
End Property

'Column value limits:
'If a column is numeric (ATCoInt or ATCoSng), these are the minimum and maximum valid values for data in each column
'If a column is ATCoTxt, these are limits on the length if the string
Public Property Let ColMax(ByVal Column&, ByVal NewValue!)
  DebugMsg "Let ColMax(" & Column & "): " & NewValue, 5, "p"
  If cols <= Column Then cols = Column + 1
  If Column >= LBound(ColSpec) And Column <= UBound(ColSpec) Then
    ColSpec(Column).MaxVal = NewValue
  End If
End Property

Public Property Let ColMin(ByVal Column&, ByVal NewValue!)
  DebugMsg "Let ColMin(" & Column & "): " & NewValue, 5, "p"
  If cols <= Column Then cols = Column + 1
  If Column >= LBound(ColSpec) And Column <= UBound(ColSpec) Then
    ColSpec(Column).MinVal = NewValue
  End If
End Property

Public Property Let ColSoftMin(ByVal Column&, ByVal NewValue!)
  DebugMsg "Let ColSoftMin(" & Column & "): " & NewValue, 5, "p"
  If cols <= Column Then cols = Column + 1
  If Column >= LBound(ColSpec) And Column <= UBound(ColSpec) Then
    ColSpec(Column).SoftMin = NewValue
  End If
End Property

Public Property Let ColSoftMax(ByVal Column&, ByVal NewValue!)
  DebugMsg "Let ColSoftMax(" & Column & "): " & NewValue, 5, "p"
  If cols <= Column Then cols = Column + 1
  If Column >= LBound(ColSpec) And Column <= UBound(ColSpec) Then
    ColSpec(Column).SoftMax = NewValue
  End If
End Property

Public Sub SetColRange(ByVal Column&, ByVal MinVal!, ByVal MaxVal!)
  'Set ColMin and ColMax in one call
  DebugMsg "SetColRange(" & Column & "): " & MinVal & ".." & MaxVal, 5, "p"
  If cols <= Column Then cols = Column + 1
  If Column >= LBound(ColSpec) And Column <= UBound(ColSpec) Then
    ColMin(Column) = MinVal
    ColMax(Column) = MaxVal
  End If
End Sub

Public Sub Clear() 'Clear all titles and cells and valid values
  DebugMsg "Clear", 5, "p"
  closePopup
  ClearValues
  ReDim ItemDataA(1)
  Set SelectedCells = Nothing
  Set SelectedCells = New Collection
  nItemData = 0
  MaxOccRow = 0
  grid.Clear
  header = ""
  UserControl_Resize
End Sub
  
Public Sub ClearData() 'Clear data in cells but leave column titles intact
  DebugMsg "ClearData", 5, "p"
  
  closePopup
  ClearValues
  ReDim ItemDataA(1)
  Set SelectedCells = Nothing
  Set SelectedCells = New Collection
  nItemData = 0
  MaxOccRow = 0

  If FixedRows < 1 Then
    grid.Clear
  Else
    Dim title$(), c&, r&
    ReDim title$(FixedRows, grid.cols)
    For r = 0 To FixedRows - 1
      For c = 0 To grid.cols - 1
        title(r, c) = grid.TextMatrix(r, c)
      Next c
    Next r
    grid.Clear
    For r = 0 To FixedRows - 1
      For c = 0 To grid.cols - 1
        grid.TextMatrix(r, c) = title(r, c)
      Next c
    Next r
  End If
  UserControl_Resize
End Sub

Public Sub ClearValues() 'Clear Valid Values (remove list of values user must choose from)
  DebugMsg "ClearValues", 5, "p"
  nValidValues = 0
  ReDim ValidValues(nValidValues)
End Sub

Public Sub addValue(NewValue$) 'Specify one of a limited set of valid values to be entered as data in the grid
Attribute addValue.VB_Description = "Add a valid value to the list of valid values"
  DebugMsg "addValue: " & NewValue, 5, "p"
  nValidValues = nValidValues + 1
  ReDim Preserve ValidValues(nValidValues)
  ValidValues(nValidValues - 1) = NewValue
End Sub

Public Sub DeleteRow(delRow&)
  grid.row = flexRow(delRow)
  DeleteRows
End Sub

Public Sub DeleteRows() 'Remove the selected row(s) from the grid, making the grid have fewer rows
  Dim fromRow&, toRow&, rows_copied&, rows_deleted&, r&
  On Error GoTo ErrDelRow
  With grid
    If .row <= .RowSel Then fromRow = .row: toRow = .RowSel Else fromRow = .RowSel: toRow = .row
    DebugMsg "DeleteRows: " & agdRow(fromRow) & " to " & agdRow(toRow), 5, "p"
    .col = flexCol(0)
    .ColSel = flexCol(cols - 1)
    rows_deleted = toRow - fromRow + 1
    
    If toRow = rows Then
      rows = fromRow - 1
    Else
      DeselectAll 'Maybe we should move all the current selections at >= fromRow, but for now we just clear selections
      If toRow < .rows - 1 And MaxOccRow > agdRow(toRow) Then 'need to move rows below those being deleted
        '.row = toRow + 1
        '.ColSel = .Cols - 1
        '.RowSel = .Rows - 1
        'rows_copied = .RowSel - .row + 1
        'grid_KeyPress 3 'copy
  
        '.row = fromRow
        '.ColSel = .Cols - 1
        '.RowSel = fromRow + rows_copied - 1
  
        'ForcePaste = True
        'grid_KeyPress 22 'paste
        'ForcePaste = False
        
        r = fromRow
        While r + rows_deleted < nItemData
          ItemData(r) = ItemData(r + rows_deleted - 1)
          r = r + 1
        Wend
        
      End If
      For r = toRow To fromRow Step -1
        If grid.rows > r Then grid.RemoveItem r
      Next r
      '.Rows = .Rows - rows_deleted
      If MaxOccRow >= agdRow(toRow) Then
        MaxOccRow = MaxOccRow - rows_deleted
      ElseIf MaxOccRow >= agdRow(fromRow) Then
        MaxOccRow = agdRow(fromRow) - 1
      End If
      If fromRow < .rows Then .row = fromRow
      .col = 0
    End If
  End With
  Exit Sub
  
ErrDelRow:
  MsgBox Err.Description, vbOKOnly, "Grid"
End Sub

Public Sub InsertRow(ByVal InsertRow&)  'Inserts a copy of InsertRow after InsertRow
  DebugMsg "InsertRow: " & InsertRow, 5, "p"
  Dim InsertAfter&, r&
  If rows = 0 Then
    rows = 1
  Else
    If InsertRow < 1 Then InsertRow = 1
    If InsertRow > rows Then InsertRow = rows
    If InsertRow > MaxOccRow Then 'don't need to shift any content, just add an empty row
      rows = rows + 1
    Else
      With grid
        InsertAfter = flexRow(InsertRow)
        .col = 0
        .row = InsertAfter
        .ColSel = .cols - 1
        .RowSel = flexRow(MaxOccRow)
  
        grid_KeyPress 3 'copy
  
        MaxOccRow = MaxOccRow + 1
        If flexRow(MaxOccRow) >= .rows Then .rows = .rows + 1
        If .row < .rows - 1 Then .row = .row + 1
        .col = 0
        .ColSel = .cols - 1
        .RowSel = flexRow(MaxOccRow)
  
        ForcePaste = True
        grid_KeyPress 22 'paste
        ForcePaste = False
  
        .row = InsertAfter
        .col = 0
      End With
      r = nItemData
      While r >= InsertAfter
        ItemData(r + 1) = ItemData(r)
        r = r - 1
      Wend
    End If
  End If
End Sub

'Search a column for a string, or search ItemData if column < 0
'returns -1 if not found
Public Function RowContaining&(ByVal search As Variant, ByVal Column&)
  Dim r&, maxr&, found As Boolean, lcaseSearch As Boolean 'not doing lcaseSearch for itemdata, but could
  Static lastPos& 'Usually helps speed to check near the last one first
  maxr = MaxOccRow
  If lastPos > maxr Then lastPos = maxr
  r = lastPos - 1
  If r < 1 Then r = 1
  If search = LCase(search) Then lcaseSearch = True
  found = False
  If Column < 0 Then
    If nItemData < MaxOccRow Then maxr = nItemData
    While r <= maxr And Not found
      If ItemDataA(r) = search Then found = True Else r = r + 1
    Wend
    If Not found Then
      r = 1
      While r < lastPos And Not found
        If ItemDataA(r) = search Then found = True Else r = r + 1
      Wend
    End If
  ElseIf Column < cols Then
    While r <= maxr And Not found
      If lcaseSearch Then
        If LCase(TextMatrix(r, Column)) = search Then found = True Else r = r + 1
      Else
        If TextMatrix(r, Column) = search Then found = True Else r = r + 1
      End If
    Wend
    If Not found Then
      r = 1
      While r < lastPos And Not found
        If lcaseSearch Then
          If LCase(TextMatrix(r, Column)) = search Then found = True Else r = r + 1
        Else
          If TextMatrix(r, Column) = search Then found = True Else r = r + 1
        End If
      Wend
    End If
  End If
  If found Then
    RowContaining = r
    lastPos = r
  Else
    RowContaining = -1
    lastPos = 0
  End If
End Function

'copies entire grid, including fixed rows and cols
Public Sub copyAll()
  Dim r&, c&, CopyString$
  CopyString = ""
  'First, read info from fixed rows (column titles)
  If hdrLines > 0 Then
    Dim CRpos&, lastCrPos&
    lastCrPos = 0
    For r = 1 To hdrLines
      CRpos = InStr(lastCrPos + 1, lblHeader.Caption, vbCr)
      If CRpos = 0 Then CRpos = Len(lblHeader.Caption) + 1
      CopyString = CopyString & Mid(lblHeader.Caption, lastCrPos + 1, CRpos - lastCrPos - 1) & vbCr
      lastCrPos = CRpos
    Next r
  End If
  If FixedRows > 0 Then
    For r = 0 To FixedRows - 1
      For c = 0 To cols - 1
        If colWidth(c) > 0 Then
          CopyString = CopyString & grid.TextMatrix(r, c)
          If Right(CopyString, 1) = "-" Then
            CopyString = Left(CopyString, Len(CopyString) - 1)
          Else
            CopyString = CopyString & " "
          End If
        End If
        CopyString = CopyString & vbTab
      Next
      CopyString = Left(CopyString, Len(CopyString) - 1) & vbCr
    Next
  End If
'  If FixedCols = 0 Then
'    r = row
'    c = col
'    row = 1
'    col = 0
'    grid.RowSel = flexRow(rows)
'    grid.ColSel = flexCol(cols - 1)
'    CopyString = CopyString & grid.Clip
'    row = r
'    col = c
'  Else
    For r = flexRow(1) To grid.rows - 1
      For c = 0 To grid.cols - 1
        If colWidth(c) > 0 Then CopyString = CopyString & grid.TextMatrix(r, c) & vbTab
      Next c
      CopyString = Left(CopyString, Len(CopyString) - 1) & vbCr
    Next r
'  End If
  PutOnClipboard CopyString
End Sub

'Copy selected cells of grid to clipboard
'If DisjointSelections or DisjointByRow is true, copies entire grid
'grid_KeyPress 3 'ctrl-c = copy
Public Sub copy()
  If DisjointSelections Or DisjointByRow Then 'copy whole table
    copyAll
  Else                                        'copy selected portion of table
    PutOnClipboard grid.Clip
  End If
End Sub

Private Sub PutOnClipboard(CopyString$)
  On Error GoTo StupidWindozeClipboard
  Clipboard.Clear
  Clipboard.SetText CopyString, vbCFText
  On Error GoTo 0
  Exit Sub
StupidWindozeClipboard:
  'Sleep 500 'vb keeps forgetting how to sleep, so we use the following loop:
  Dim zzz&
  For zzz = 1 To 200
    DoEvents
  Next zzz
  Clipboard.SetText CopyString, vbCFText
  Resume Next
End Sub

Private Function agdRow&(ByVal flexRo&)
  agdRow = flexRo - grid.FixedRows + 1
End Function

Private Function flexRow&(ByVal agdRo&)
  flexRow = agdRo + grid.FixedRows - 1
End Function

Private Function agdCol&(ByVal flexCo&)
  agdCol = flexCo 'agdCol = flexCo - grid.FixedCols + 1
End Function

Private Function flexCol&(ByVal agdCo&)
  flexCol = agdCo 'flexCol = agdCo + grid.FixedCols - 1
End Function

Private Sub DebugMsg(s$, Optional l& = 9, Optional t$ = "?")
  Dim typ$, lev&
  lev = l
  typ = t
  DbgMsg s$, lev, "ATCoGrid", typ
End Sub

Private Sub comboPopup_Change()
  'autocomplete code by Dan Redding, Blue Knot Software http://www.blueknot.com/
  Dim strPart As String, iLoop As Integer, iStart As Integer, strItem As String
  'don't do if no text or if change was made by autocomplete coding
  If Not pAutoCompleting And comboPopup.Text <> "" Then
    'save the selection start point (cursor position)
    iStart = comboPopup.SelStart
    'get the part the user has typed (not selected)
    strPart = UCase(Left(comboPopup.Text, iStart))
    For iLoop = 0 To comboPopup.ListCount - 1
      'compare each item to the part the user has typed,
      '"complete" with the first good match
      strItem = UCase$(comboPopup.List(iLoop))
      If Left(strItem, iStart) = strPart And strItem <> UCase(comboPopup.Text) Then
        'partial match but not the whole thing. (if whole thing, nothing to complete!)
        pAutoCompleting = True
        comboPopup.SelText = Mid(comboPopup.List(iLoop), iStart + 1) 'add on the new ending
        comboPopup.SelStart = iStart   'reset the selection
        comboPopup.SelLength = Len(comboPopup.Text) - iStart
        pAutoCompleting = False
        Exit For
      End If
    Next iLoop
    comboPopup_Click
  End If
End Sub

Private Sub comboPopup_Click()
  If grid.Text <> comboPopup.Text Then
    If ValidPopupValue(comboPopup.Text) Then
      ChangeSelectedValues comboPopup.Text
      RaiseEvent CommitChange(agdRow(ChangeFromRow), agdRow(ChangeToRow), agdCol(ChangeFromCol), agdCol(ChangeToCol))
    End If
  End If
End Sub

Private Function ValidPopupValue(testValue$) As Boolean
  If ComboCheckValidValues Then
    Dim v&, found As Boolean
    v = 0
    found = False
    While v < nValidValues And Not found
      If comboPopup.Text = ValidValues(v) Then found = True
      v = v + 1
    Wend
    ValidPopupValue = found
  Else
    ValidPopupValue = True
  End If

End Function

Private Sub comboPopup_KeyDown(KeyCode As Integer, Shift As Integer)
  'RaiseEvent KeyDown(KeyCode, Shift)
  'Unless we watch out for it, backspace or delete will just delete
  'the selected text (the autocomplete part), so we delete it here
  'first so it doesn't interfere with what the user expects
  Select Case KeyCode
    Case vbKeyBack, vbKeyDelete
      pAutoCompleting = True
      comboPopup.SelText = ""
      pAutoCompleting = False
    Case vbKeyReturn 'Accept autocomplete on 'Enter' keypress
      comboPopup_LostFocus
      'the following causes the item to be selected and
      'the cursor placed at the end:
      comboPopup.SelStart = Len(comboPopup.Text)
      'This would select the whole thing instead:
      'comboPopup.SelLength = Len(comboPopup.Text)
      comboPopup.Visible = False
    Case vbKeyEscape
      comboPopup.Visible = False
  End Select
End Sub

Private Sub comboPopup_LostFocus()
  closePopup
End Sub

Private Sub CullSelectedCells()
  Dim Index&, r&, c&, toRow&, toCol&
  toRow = rows
  toCol = cols - 1
  pSelStartRow = rows
  pSelStartCol = cols
  pSelOtherRow = 0
  pSelOtherCol = 0
  For Index = SelectedCells.Count To 1 Step -1
    r = SelectedCells(Index).row
    c = SelectedCells(Index).col
    If r > toRow Then
      SelectedCells.Remove Index
    ElseIf c > toCol Then
      SelectedCells.Remove Index
    Else 'cell still exists and is selected
      If r < pSelStartRow Then pSelStartRow = r
      If c < pSelStartCol Then pSelStartCol = c
      
      If r > pSelOtherRow Then pSelOtherRow = r
      If c > pSelOtherCol Then pSelOtherCol = c
    End If
  Next
  If pSelOtherRow = 0 Then pSelStartRow = 0: pSelStartCol = 0 ' no cells selected
End Sub

Private Sub grid_Click()
  'RaiseEvent Click
  'grid.SetFocus
End Sub

Private Sub grid_DblClick()
  DebugMsg "grid:DblClick", 5, "m"
  If ColSpec(col).Editable Then
    OpenPopup
  Else
    RaiseEvent DoubleClick(row, col)
  End If
End Sub

'Private Sub grid_GotFocus()
  'Debug.Print "Grid got focus"
'End Sub

Private Sub grid_KeyDown(KeyCode As Integer, Shift As Integer)
  DebugMsg "grid:KeyDown:" & KeyCode & "," & Shift, 6, "k"
  'RaiseEvent KeyDown(KeyCode, Shift)
  Select Case KeyCode
    Case vbKeySpace: OpenPopup
    Case 93:         OpenPopupMenu 'Windows menu key next to right ctrl
    Case vbKeyPause
      Dim c&
      For c = 0 To cols - 1
        If ColTitle(c) = HiddenColumnTitle Then
          ColTitle(c) = HiddenColumnTitle & "-"
          colWidth(c) = -1
        ElseIf ColTitle(c) = HiddenColumnTitle & "-" Then
          ColTitle(c) = HiddenColumnTitle
        End If
      Next c
  End Select

End Sub

Private Sub grid_KeyPress(KeyAscii As Integer)
  Dim r&, c&
  DebugMsg "grid:KeyPress:" & KeyAscii, 5, "k"
    
  If ColSpec(col).DataType = ATCoClr Then
    OpenPopup
  Else
    If KeyAscii = 22 And Clipboard.GetFormat(vbCFText) Then
      PasteFromClipboard grid
    ElseIf KeyAscii = 3 Then 'Control-C (copy)
      copy
    ElseIf KeyAscii = 10 Then 'Control-Enter = LF = toggle select
      grid_MouseDown vbLeftButton, vbCtrlMask, 0, 0
    ElseIf KeyAscii = 13 Then
      grid_MouseDown vbLeftButton, 0, 0, 0 'Enter = select
    ElseIf KeyAscii = 32 Then 'Spacebar = select (moved to KeyDown)

    ElseIf grid.row >= grid.FixedRows And ColSpec(grid.col).Editable Then
      'they are probably trying to type a value, so open the control to edit this cell
      OpenPopup
      Dim DataType As ATCoDataType
      DataType = ColSpec(col).DataType
      If DataType = ATCoTxt And KeyAscii >= 32 And KeyAscii < 127 Then
        SendKeys "{" & Chr$(KeyAscii) & "}"
      ElseIf DataType = ATCoInt And KeyAscii > 47 And KeyAscii < 58 Or KeyAscii = 45 Then
        SendKeys "{" & Chr$(KeyAscii) & "}"
      ElseIf DataType = ATCoSng And (KeyAscii > 47 And KeyAscii < 58 Or KeyAscii = 45 Or KeyAscii = 46) Then
        SendKeys "{" & Chr$(KeyAscii) & "}"
      End If
    ElseIf KeyAscii > 32 And KeyAscii < 127 Then
      'search for next row starting with this character in non-editable column
      Dim SearchString$
      SearchString = UCase(Chr(KeyAscii))
      r = row + 1
      c = col
      While r <= rows
        If UCase(Left(TextMatrix(r, c), 1)) = SearchString Then
          row = r
          If Not RowIsVisible(r) Then
            If r > 1 Then TopRow = r - 1 Else TopRow = r
          End If
          r = rows 'cause loop to exit
        End If
        r = r + 1
      Wend
    Else
      Debug.Print "KeyAscii=" & KeyAscii & " (Unknown)"
    End If
  End If
  DebugMsg "grid:KeyPress", 8, "o"
End Sub

Private Sub grid_KeyUp(KeyCode As Integer, Shift As Integer)
  DebugMsg "grid:KeyUp:", 8, "k"
'  openPopup
End Sub

Private Sub grid_LeaveCell()
  closePopup
End Sub

'Private Sub grid_LostFocus()
  'Debug.Print "Grid lost focus"
'End Sub

Private Sub grid_MouseDown(Button As Integer, Shift As Integer, X As Single, Y As Single)
  Dim r&, c&
  Dim ShiftDown As Boolean
  Dim CtrlDown As Boolean
  DebugMsg "grid:MouseDown", 5, "m"
  
  ShiftDown = (Shift And vbShiftMask) > 0
  CtrlDown = (Shift And vbCtrlMask) > 0
  
  If X = 0 And Y = 0 Then 'this is not really a mouse event
    c = col
    r = row
  Else
    c = agdCol(grid.MouseCol)
    r = agdRow(grid.MouseRow)
  End If
  If r < 1 Then 'clicked in a fixed row
    RaiseEvent ClickColTitle(c, (Button))
    
    If AllowSorting And nItemData = 0 Then 'Sorting can happen only when not using ItemData
      Dim Ascending As Boolean             'Use a column of zero width instead to do both
      
      'sort ascending by default or descending if right click
      If Button = vbRightButton Then Ascending = False Else Ascending = True
      Sort c, Ascending
      RaiseEvent Sorted(c, Ascending)
    End If
    
  ElseIf Button = vbRightButton Then
    col = c
    row = r
    OpenPopupMenu
  ElseIf Button = vbLeftButton And DisjointSelections Then
    'If Not ShiftDown Or pSelStartRow < 1 Or pSelStartCol < 0 Then
    '  pSelStartRow = r
    '  pSelStartCol = c
    'End If
    If ShiftDown Then
      ExtendRectangularSelection r, c
    ElseIf (ColSpec(col).Selectable Or DisjointByRow) Then
      If pSelectionToggle Or CtrlDown Then
        Selected(r, c) = Not Selected(r, c)
      Else
        DeselectAllBut r, c
        Selected(r, c) = True
      End If
      grid.SetFocus
    End If
  ElseIf Not (DisjointSelections) Then 'jlk 4/21/99 for hspfparm
    RaiseEvent Click
    grid.SetFocus
  End If
End Sub

Private Function RangeText$(ByVal col&)
  Dim txt$
  txt = ""
  If ColSpec(col).MinVal <> ColSpec(col).MaxVal Then
    If ColSpec(col).MinVal <> NONE Then txt = txt & "Min: " & ColSpec(col).MinVal & " "
    If ColSpec(col).MaxVal <> NONE Then txt = txt & "Max: " & ColSpec(col).MaxVal & " "
  End If
  If ColSpec(col).SoftMin <> ColSpec(col).SoftMax Then
    If Len(txt) > 0 Then
      If ColSpec(col).SoftMin <> NONE Then txt = txt & "Soft Min: " & ColSpec(col).SoftMin & " "
      If ColSpec(col).SoftMax <> NONE Then txt = txt & "Soft Max: " & ColSpec(col).SoftMax
    Else
      If ColSpec(col).SoftMin <> NONE Then txt = txt & "Min: " & ColSpec(col).SoftMin & " "
      If ColSpec(col).SoftMax <> NONE Then txt = txt & "Max: " & ColSpec(col).SoftMax
    End If
  End If
  RangeText = txt
End Function

Private Sub grid_MouseMove(Button As Integer, Shift As Integer, X As Single, Y As Single)
  
  'First, get rid of annoying repeated events
  Static lastX As Single, lastY As Single
  If X = lastX And Y = lastY Then Exit Sub
  lastX = X
  lastY = Y
  
  'Set tooltip text
  Dim txt$
  txt = ""
  If grid.MouseCol = grid.col And grid.MouseRow = grid.row And nValidValues = 0 Then
    txt = RangeText(grid.col)
  End If
  If txt <> grid.ToolTipText Then grid.ToolTipText = txt

  Dim LeftDown As Boolean
  'Dim CtrlDown As Boolean
  
  LeftDown = (Button And vbLeftButton) > 0
  'CtrlDown = (Shift And vbCtrlMask) > 0
  
  'Extend selection
  Dim r&, c&
  If LeftDown And DisjointSelections Then
    r = agdRow(grid.MouseRow)
    c = agdCol(grid.MouseCol)
    ExtendRectangularSelection r, c
  End If
End Sub

'When using shift-click or drag, selection expands and contracts as a rectangle
Public Sub ExtendRectangularSelection(newrow&, newcol&)
  Dim r&, c&, Index&
  Dim fromRow&, toRow&, fromCol&, toCol&
  Dim newCellInfo As CellSpecs
  Dim newSelect As Collection     'building collection of cells that were selected this time
  Static toDeselect As Collection 'newSelect from last call, may need to deselect some of them
  Static lastStartRow&, lastStartCol&
  
  If lastStartRow <> pSelStartRow Or _
     lastStartCol <> pSelStartCol Or lastStartRow < 1 Then
    Set toDeselect = Nothing
    Set toDeselect = New Collection
    lastStartRow = pSelStartRow
    lastStartCol = pSelStartCol
  End If
  
  If DisjointByRow Or grid.SelectionMode = flexSelectionByRow Then
    fromCol = pSelStartCol: toCol = pSelStartCol
  ElseIf newcol <= pSelStartCol Then
    fromCol = newcol: toCol = pSelStartCol
  Else
    fromCol = pSelStartCol: toCol = newcol
  End If
      
  If newrow <= pSelStartRow Then
    fromRow = newrow: toRow = pSelStartRow
  Else
    fromRow = pSelStartRow: toRow = newrow
  End If
      
  Set newSelect = New Collection
  If fromRow = 0 Then Exit Sub
  For r = fromRow To toRow
    For c = fromCol To toCol
      If ColSpec(c).Selectable Or DisjointByRow Then
                
        If Not Selected(r, c) Then
          Selected(r, c) = True
        End If
        
        If Selected(r, c) Then
          newCellInfo.row = r
          newCellInfo.col = c
          newSelect.Add newCellInfo
        End If
        
        'Remove this cell from deselect list if it is there
        For Index = toDeselect.Count To 1 Step -1
          If toDeselect(Index).row = r And toDeselect(Index).col = c Then
            toDeselect.Remove Index
            Index = 0
          End If
        Next
      
      End If
    Next c
  Next r
  For Index = toDeselect.Count To 1 Step -1
    r = toDeselect(Index).row
    c = toDeselect(Index).col
    Selected(r, c) = False
  Next
  Set toDeselect = Nothing
  Set toDeselect = newSelect
  grid.SetFocus

End Sub


Private Sub grid_MouseUp(Button As Integer, Shift As Integer, X As Single, Y As Single)
  DebugMsg "grid:MouseUp", 8, "m"
  'framePopup.Visible = False
  'openPopup
End Sub

Private Sub grid_RowColChange()
  Dim tmp$
  DebugMsg "grid:RowColChange", 6, "t"
  'closePopup instance in grid_LeaveCell should be enough
  'tmp = grid.ToolTipText
  grid.ToolTipText = ""
  RaiseEvent RowColChange
  'give users a chance to set their own tooltip on RowColChange event
  'If tmp = grid.ToolTipText Then grid.ToolTipText = RangeText(grid.col)
  If grid.ToolTipText = "" Then grid.ToolTipText = RangeText(grid.col)
End Sub

Private Sub grid_Scroll()
  DebugMsg "grid:Scroll", 7, "t"
  closePopup
End Sub

Private Sub grid_SelChange()
  DebugMsg "grid:SelChange", 7, "t"
  closePopup
End Sub

Private Sub lblHeader_Click()
  If AllowEditHeader Then
    With txtEditHeader
      .Move 0, 0, lblHeader.Width, lblHeader.Height
      .Value = lblHeader.Caption
      .Visible = True
    End With
  End If
End Sub

Private Sub lblPopup_MouseUp(Button As Integer, Shift As Integer, X As Single, Y As Single)
  'framePopup.Visible = False
End Sub

Private Sub mnuPopupBackColor_Click()
  Dim clr As OLE_COLOR
  clr = ColorFromDialog(BackColor)
  If clr <> -1 Then Me.BackColor = clr
End Sub

Private Sub mnuPopupColsSizeByContents_Click()
  ColsSizeByContents
End Sub

Private Sub mnuPopupColsSizeToWidth_Click()
  ColsSizeToWidth
End Sub

Private Sub mnuPopupCopy_Click()
  copy
End Sub

Private Sub mnuPopupCopyAll_Click()
  copyAll
End Sub

Private Sub mnuPopupFont_Click()
  SetFontFromDialog
End Sub

Private Sub mnuPopupForeColor_Click()
  Dim clr As OLE_COLOR
  clr = ColorFromDialog(ForeColor)
  If clr <> -1 Then Me.ForeColor = clr
End Sub

Private Sub mnuPopupLoad_Click()
  LoadGridInteractive 'frmGridPrintSaveLoad.LoadGrid Me 'LoadFile "#", False, False, "", ","
End Sub

Private Sub mnuPopupPaste_Click()
  paste
End Sub

Private Sub mnuPopupPrint_Click()
  PrintGridInteractive 'frmGridPrintSaveLoad.PrintGrid Me
End Sub

Private Sub mnuPopupSave_Click()
  SaveGridInteractive
End Sub

Private Sub mnuPopupSaveNormalized_Click()
  SaveNormalized
End Sub

Private Sub mnuPopupSelectionToggle_Click()
  mnuPopupSelectionToggle.Checked = Not mnuPopupSelectionToggle.Checked
  SelectionToggle = mnuPopupSelectionToggle.Checked
End Sub

Private Sub txtEditHeader_CommitChange()
  header = ReplaceString(txtEditHeader.Value, "&", vbCrLf)
  txtEditHeader.Visible = False
End Sub

Private Sub txtEditHeader_LostFocus()
  txtEditHeader.Visible = False
End Sub

Private Sub txtPopup_Change()
  DebugMsg "txtPopup:Change", 7, "t"
  If txtPopup.Visible Then
    ChangeSelectedValues txtPopup.Value 'Was commented out - put it back to support getting the value that was being edited when user was finished with the form
    RaiseEvent TextChange(agdRow(ChangeFromRow), agdRow(ChangeToRow), agdCol(ChangeFromCol), agdCol(ChangeToCol))
  End If
End Sub

Private Sub txtPopup_CommitChange()
  DebugMsg "txtPopup:CommitChange", 7, "t"
  If txtPopup.Visible Then closePopup
  RaiseEvent CommitChange(agdRow(ChangeFromRow), agdRow(ChangeToRow), agdCol(ChangeFromCol), agdCol(ChangeToCol))
End Sub

Private Sub txtPopup_KeyDown(KeyCode As Integer, Shift As Integer)
  Dim ch$
  'RaiseEvent KeyDown(KeyCode, Shift)
  If KeyCode > 31 And KeyCode < 127 Then ch = Chr$(KeyCode)
  DebugMsg "txtPopup:KeyDown " & KeyCode & " """ & ch & """", 5, "k"
  Dim SendStr, ShiftDown, ShiftUp As Boolean
  SendStr = Null
  If (Shift And vbShiftMask) > 0 Then
    ShiftDown = True: ShiftUp = False
  Else
    ShiftDown = False: ShiftUp = True
  End If
  With txtPopup
    Select Case KeyCode
      Case vbKeyReturn:   closePopup
      'Case vbKeyReturn:   SendStr = "{DOWN}"
      Case vbKeyUp:       SendStr = "{UP}"
      Case vbKeyDown:     SendStr = "{DOWN}"
      Case vbKeyPageUp:   SendStr = "{PGUP}"
      Case vbKeyPageDown: SendStr = "{PGDN}"
      Case vbKeyRight:    If ShiftUp And .SelStart = Len(.Value) Then SendStr = "{RIGHT}"
      Case vbKeyLeft:     If ShiftUp And .SelStart = 0 And .SelLength = 0 Then SendStr = "{LEFT}"
      Case vbKeyTab:      If ShiftUp Then SendStr = "{RIGHT}"
      Case vbKeyTab:      If ShiftDown Then SendStr = "{LEFT}"
    End Select
    If Not IsNull(SendStr) Then
      grid.SetFocus
      SendKeys SendStr
    End If
  End With
End Sub

Private Sub UserControl_GotFocus()
  'Debug.Print "AGD UserControl_GotFocus"
End Sub

Private Sub UserControl_LostFocus()
  closePopup
End Sub

Private Sub UserControl_Show()
  txtPopup.Visible = False
  comboPopup.Visible = False
  grid.Visible = True
  RaiseEvent RowColChange
End Sub

Private Sub UserControl_Hide()
  closePopup
End Sub

Private Sub UserControl_Initialize()
  Dim c&
  Set SelectedCells = New Collection
  Set progress1 = New ATCoProgress
  ForcePaste = False
  nValidValues = 0
  ColWidthMin = 300
  If grid.cols > 0 Then
    ReDim ColSpec(0 To grid.cols - 1)
    For c = 0 To grid.cols - 1
      With ColSpec(c)
        .Editable = False
        .Selectable = False
        .MinVal = NONE
        .MaxVal = NONE
        .SoftMin = NONE
        .SoftMax = NONE
        .CharWidth = DefaultColCharWidthTxt
      End With
'         flexAlignLeftTop
'         flexAlignLeftCenter
'         flexAlignLeftBottom
'         flexAlignCenterTop
'         flexAlignCenterCenter
'         flexAlignCenterBottom
'         flexAlignRightTop
'         flexAlignRightCenter
'         flexAlignRightBottom
'         flexAlignGeneral
      grid.ColAlignment(flexCol(c)) = flexAlignLeftCenter ' default for text columns, changed for other types in Let ColType
    Next c
  End If
End Sub

Private Sub UserControl_InitProperties()
  grid.rows = 2
  grid.cols = 2
  grid.ScrollBars = flexScrollBarBoth
  InsideLimitsBackground = vbWindowBackground
  OutsideHardLimitBackground = 8421631 'light red
  OutsideSoftLimitBackground = 8454143 'light yellow

End Sub

Private Sub UserControl_ReadProperties(PropBag As PropertyBag)
  SelectionToggle = PropBag.ReadProperty("SelectionToggle", False)
  AllowBigSelection = PropBag.ReadProperty("AllowBigSelection", False)
  AllowEditHeader = PropBag.ReadProperty("AllowEditHeader", False)
  AllowLoad = PropBag.ReadProperty("AllowLoad", False)
  AllowSorting = PropBag.ReadProperty("AllowSorting", True)
  rows = PropBag.ReadProperty("Rows", 2)
  cols = PropBag.ReadProperty("Cols", 2)
  gridFontBold = PropBag.ReadProperty("gridFontBold", False)
  gridFontItalic = PropBag.ReadProperty("gridFontItalic", False)
  gridFontName = PropBag.ReadProperty("gridFontName", "MS Sans Serif")
  gridFontSize = PropBag.ReadProperty("gridFontSize", "8.25")
  gridFontUnderline = PropBag.ReadProperty("gridFontUnderline", False)
  gridFontWeight = PropBag.ReadProperty("gridFontWeight", 400)
  gridFontWidth = PropBag.ReadProperty("gridFontWidth", 0)
  header = PropBag.ReadProperty("Header", "")
  FixedRows = PropBag.ReadProperty("FixedRows", 1)
  FixedCols = PropBag.ReadProperty("FixedCols", 0)
  ScrollBars = PropBag.ReadProperty("ScrollBars", flexScrollBarBoth)
  SelectionMode = PropBag.ReadProperty("SelectionMode", ASfree)
  ColWidthMinimum = PropBag.ReadProperty("ColWidthMinimum", 300)
  
  BackColor = PropBag.ReadProperty("BackColor", vbWindowBackground)
  ForeColor = PropBag.ReadProperty("ForeColor", vbWindowText)
  BackColorBkg = PropBag.ReadProperty("BackColorBkg", vb3DShadow)
  BackColorSel = PropBag.ReadProperty("BackColorSel", vbHighlight)
  ForeColorSel = PropBag.ReadProperty("ForeColorSel", vbHighlightText)
  BackColorFixed = PropBag.ReadProperty("BackColorFixed", vbButtonFace)
  ForeColorFixed = PropBag.ReadProperty("ForeColorFixed", vbButtonText)
  InsideLimitsBackground = PropBag.ReadProperty("InsideLimitsBackground", vbWindowBackground)
  OutsideHardLimitBackground = PropBag.ReadProperty("OutsideHardLimitBackground", 8421631) 'light red
  OutsideSoftLimitBackground = PropBag.ReadProperty("OutsideSoftLimitBackground", 8454143) 'light yellow
  ComboCheckValidValues = PropBag.ReadProperty("ComboCheckValidValues", True)
End Sub

Private Sub UserControl_WriteProperties(PropBag As PropertyBag)
  PropBag.WriteProperty "SelectionToggle", SelectionToggle
  PropBag.WriteProperty "AllowBigSelection", AllowBigSelection
  PropBag.WriteProperty "AllowEditHeader", AllowEditHeader
  PropBag.WriteProperty "AllowLoad", AllowLoad
  PropBag.WriteProperty "AllowSorting", AllowSorting
  PropBag.WriteProperty "Rows", rows
  PropBag.WriteProperty "Cols", cols
  PropBag.WriteProperty "ColWidthMinimum", ColWidthMinimum
  PropBag.WriteProperty "gridFontBold", gridFontBold
  PropBag.WriteProperty "gridFontItalic", gridFontItalic
  PropBag.WriteProperty "gridFontName", gridFontName
  PropBag.WriteProperty "gridFontSize", gridFontSize
  PropBag.WriteProperty "gridFontUnderline", gridFontUnderline
  PropBag.WriteProperty "gridFontWeight", gridFontWeight
  PropBag.WriteProperty "gridFontWidth", gridFontWidth
  PropBag.WriteProperty "Header", lblHeader.Caption
  PropBag.WriteProperty "FixedRows", FixedRows
  PropBag.WriteProperty "FixedCols", FixedCols
  PropBag.WriteProperty "ScrollBars", ScrollBars
  If DisjointSelections Then
    If DisjointByRow Then
      PropBag.WriteProperty "SelectionMode", ASdisjointByRow
    Else
      PropBag.WriteProperty "SelectionMode", ASdisjoint
    End If
  Else
    PropBag.WriteProperty "SelectionMode", grid.SelectionMode
  End If

  PropBag.WriteProperty "BackColor", BackColor
  PropBag.WriteProperty "ForeColor", ForeColor
  PropBag.WriteProperty "BackColorBkg", BackColorBkg
  PropBag.WriteProperty "BackColorSel", BackColorSel
  PropBag.WriteProperty "ForeColorSel", ForeColorSel
  PropBag.WriteProperty "BackColorFixed", BackColorFixed
  PropBag.WriteProperty "ForeColorFixed", ForeColorFixed
  PropBag.WriteProperty "InsideLimitsBackground", InsideLimitsBackground
  PropBag.WriteProperty "OutsideHardLimitBackground", OutsideHardLimitBackground
  PropBag.WriteProperty "OutsideSoftLimitBackground", OutsideSoftLimitBackground
  PropBag.WriteProperty "ComboCheckValidValues", ComboCheckValidValues
End Sub

Private Sub UserControl_Resize()
  DebugMsg "Resize", 6, "w"
  lblHeader.Width = UserControl.Width
  grid.Width = UserControl.Width

  If Len(lblHeader.Caption) < 1 Then
    hdrLines = 0
    lblHeader.Height = 0
  Else
    Dim start As Variant, lastStart As Variant
    hdrLines = 1
    start = InStr(1, lblHeader.Caption, vbCr)
    lastStart = 1
    If start > 0 Then
      While start > 0
        hdrLines = hdrLines + 1
        If TextWidth(Mid(lblHeader.Caption, lastStart, start - lastStart)) > lblHeader.Width Then hdrLines = hdrLines + 1
        lastStart = start
        start = InStr(start + 1, lblHeader.Caption, vbCr)
      Wend
    Else
      If TextWidth(lblHeader.Caption) > lblHeader.Width Then hdrLines = hdrLines + 1
    End If
    lblHeader.Height = 55 + hdrLines * 200
  End If
  grid.Top = lblHeader.Top + lblHeader.Height
  If grid.Top < UserControl.Height Then grid.Height = UserControl.Height - grid.Top
  If grid.ColIsVisible(grid.cols - 1) Then ColsSizeToWidth
End Sub

Public Sub ColsSizeByContents()
  Dim r&, c&
  Dim thisWidth&, maxWidth&(), gridWidth&
  ReDim maxWidth(cols)
  
  UserControl.FontBold = gridFontBold
  UserControl.FontItalic = gridFontItalic
  UserControl.FontName = gridFontName
  UserControl.FontSize = gridFontSize
  UserControl.FontUnderline = gridFontUnderline
  
  For r = 0 To grid.rows - 1
    For c = 0 To grid.cols - 1
      thisWidth = UserControl.TextWidth(grid.TextMatrix(r, c))
      If thisWidth > maxWidth(c) Then maxWidth(c) = thisWidth
    Next c
  Next r
  For c = 0 To cols - 1
    colWidth(agdCol(c)) = maxWidth(c) + 100  '* 31.1
  Next c
  
  thisWidth = grid.colPos(grid.cols - 1) + grid.colWidth(grid.cols - 1)
  gridWidth = grid.Width - 10 * grid.cols
  If rows > 1 Then
    If ScrollbarVisible Then gridWidth = gridWidth - scrollBarWidth
  End If
  If thisWidth < gridWidth Then ColsSizeToWidth
  
End Sub

Private Function ScrollbarVisible() As Boolean
  If rows < 2 Then
    ScrollbarVisible = False
  Else
    ScrollbarVisible = (grid.ScrollBars = flexScrollBarBoth Or grid.ScrollBars = flexScrollBarVertical) And (Not RowIsVisible(1) Or Not RowIsVisible(rows))
  End If
End Function

Public Sub ColsSizeToWidth()
  Dim oldwidth&, newWidth&, thisWidth&, c&
  If grid.Width > 300 And grid.cols > 0 Then
    oldwidth = grid.colPos(grid.cols - 1) + grid.colWidth(grid.cols - 1)
    newWidth = grid.Width - 10 * grid.cols
    If oldwidth < 1 Then oldwidth = newWidth
    If oldwidth < 1 Then oldwidth = 100
    If rows > 1 Then
      If ScrollbarVisible Then newWidth = newWidth - scrollBarWidth
    End If
    For c = 0 To grid.cols - 1
      thisWidth = newWidth * grid.colWidth(c) / oldwidth
      If thisWidth < ColWidthMin Then thisWidth = ColWidthMin
      colWidth(c) = thisWidth
    Next c
  End If
End Sub

Private Function browseSaveFilename(Optional DialogTitle$ = "Save As...", _
                                    Optional defaultFilename$ = "") As String
  browseSaveFilename = ""
  On Error GoTo NeverMind
  cdlg.DialogTitle = DialogTitle
  If defaultFilename <> "" Then cdlg.Filename = defaultFilename
  If Left(cdlg.DialogTitle, 4) = "Load" Or Left(cdlg.DialogTitle, 4) = "Open" Then
    cdlg.ShowOpen
  Else
    cdlg.ShowSave
  End If
  browseSaveFilename = cdlg.Filename
NeverMind:
End Function

Private Function ColorFromDialog(Optional default As OLE_COLOR) As OLE_COLOR
  
  If IsMissing(default) Then
    ColorFromDialog = -1
  Else
    ColorFromDialog = default
    cdlg.Color = default
  End If
  cdlg.DialogTitle = "Select a color"
  cdlg.CancelError = True
  On Error GoTo DontSetColor
  cdlg.ShowColor
  ColorFromDialog = cdlg.Color
DontSetColor:
End Function

Public Sub SetFocus()
  grid.SetFocus
End Sub

Private Sub ChangeSelectedValues(NewValue$)
  Dim r&, c&
  DebugMsg "ChangeSelectedValues: " & NewValue, 5, "p"
  r = grid.row
  c = grid.col
  If DisjointSelections Then
    Dim Index&
    For Index = 1 To SelectedCells.Count
      If Not DisjointByRow Or SelectedCells(Index).col = col Then
        SetTextIfEditable SelectedCells(Index).row, SelectedCells(Index).col, NewValue
      End If
    Next
  ElseIf grid.SelectionMode = flexSelectionFree Then
    For c = ChangeFromCol To ChangeToCol
      If ColEditable(agdCol(c)) Then
        For r = ChangeFromRow To ChangeToRow
          TextMatrix(agdRow(r), agdCol(c)) = NewValue
        Next r
      End If
    Next c
  ElseIf grid.SelectionMode = flexSelectionByRow Then
    If ColEditable(agdCol(c)) Then
      For r = ChangeFromRow To ChangeToRow
        SetTextIfEditable agdRow(r), agdCol(c), NewValue
      Next r
    End If
  ElseIf grid.SelectionMode = flexSelectionByColumn Then
    For c = ChangeFromCol To ChangeToCol
      SetTextIfEditable agdRow(r), agdCol(c), NewValue
    Next c
  End If
  If agdRow(ChangeToRow) > MaxOccRow Then MaxOccRow = agdRow(ChangeToRow)
End Sub

Private Sub SetTextIfEditable(r&, c%, NewValue$)
  If ColEditable(c) Then TextMatrix(r, c) = NewValue
End Sub

Private Sub closePopup()
  If txtPopup.Visible Then
    DebugMsg "closePopup: txtPopup", 7, "w"
    txtPopup.Visible = False
    txtPopup.Value = txtPopup.Value
    ChangeSelectedValues txtPopup.Value
  End If
  If comboPopup.Visible Then
    DebugMsg "closePopup: comboPopup", 7, "w"
    comboPopup.Visible = False
    If ValidPopupValue(comboPopup.Text) Then
      ChangeSelectedValues comboPopup.Text
      RaiseEvent CommitChange(agdRow(ChangeFromRow), agdRow(ChangeToRow), agdCol(ChangeFromCol), agdCol(ChangeToCol))
    End If
  End If
End Sub

Private Sub OpenPopup()
  closePopup
  grid.ToolTipText = ""
  If grid.Visible = False Then Exit Sub
  If grid.row >= grid.FixedRows And ColSpec(col).Editable Then
    With grid
      If .col <= .ColSel Then ChangeFromCol = .col: ChangeToCol = .ColSel Else ChangeFromCol = .ColSel: ChangeToCol = .col
      If .row <= .RowSel Then ChangeFromRow = .row: ChangeToRow = .RowSel Else ChangeFromRow = .RowSel: ChangeToRow = .row
      If Not Selected(agdRow(.row), agdCol(.col)) Then Selected(agdRow(.row), agdCol(.col)) = True
    End With
    Dim DataType As ATCoDataType
    DataType = ColSpec(grid.col).DataType
    If nValidValues > 0 Then
      Dim thisValueWidth&, maxValueWidth&
      With comboPopup
        DebugMsg "openPopup: comboPopup", 7, "w"
        .Visible = False
        .Clear
        Dim v&
        For v = 0 To nValidValues - 1
          .AddItem ValidValues(v)
          If grid.Text = ValidValues(v) Then .ListIndex = v
          thisValueWidth = TextWidth(ValidValues(v))
          If thisValueWidth > maxValueWidth Then maxValueWidth = thisValueWidth
        Next v
        
        If maxValueWidth > colWidth(col) Then 'Make sure column is wide enough to display options
          Dim curGridWidth&, curColWidth&, IntermedColWidth&
          curGridWidth = grid.colPos(grid.cols - 1) + grid.colWidth(grid.cols - 1)
          If curGridWidth > grid.Width Or maxValueWidth * 1.1 >= curGridWidth Then
            colWidth(col) = maxValueWidth * 1.1
          Else
            curColWidth = colWidth(col)
            colWidth(col) = (curGridWidth - curColWidth) / (1 - maxValueWidth * 1.1 / curGridWidth) - curGridWidth + curColWidth
            ColsSizeToWidth
          End If
        End If
        
        .Move grid.CellLeft, grid.CellTop + grid.Top - 10, grid.CellWidth
        .Visible = True
        .SetFocus
        SendKeys "{F4}"
      End With
    ElseIf DataType = ATCoClr Then
      SetCellColorFromDialog
    Else
      With txtPopup
        DebugMsg "openPopup: txtPopup", 7, "w"
        .InsideLimitsBackground = Me.InsideLimitsBackground
        .OutsideHardLimitBackground = Me.OutsideHardLimitBackground
        .OutsideSoftLimitBackground = Me.OutsideSoftLimitBackground
        .Visible = False
        .Move grid.CellLeft, grid.CellTop + grid.Top - 10, grid.CellWidth, grid.CellHeight
  
        .DataType = ColSpec(grid.col).DataType
        .maxWidth = ColSpec(grid.col).CharWidth
        If ColSpec(col).MaxVal = NONE Or ColSpec(col).MaxVal > ColSpec(col).MinVal Then
          .HardMin = ColSpec(col).MinVal
          .HardMax = ColSpec(col).MaxVal
        Else
          .HardMin = NONE
          .HardMax = NONE
        End If
        
        If ColSpec(col).SoftMin = NONE Or ColSpec(col).SoftMax > ColSpec(col).SoftMin Then
          .SoftMin = ColSpec(col).SoftMin
          .SoftMax = ColSpec(col).SoftMax
        Else
          .SoftMin = NONE
          .SoftMax = NONE
        End If
        
        .DefaultValue = NONE
        If DataType = ATCoTxt Then
          .Value = grid.Text
        ElseIf IsNumeric(grid.Text) Then
          Dim sngval!
          sngval = CSng(grid.Text)
          If (.HardMin = NONE Or sngval >= .HardMin) And (.HardMax = NONE Or sngval <= .HardMax) Then
            .Value = sngval
          Else
            .Value = NONE
          End If
        Else
          .Value = .HardMin
        End If
        
        If grid.ColAlignment(flexCol(col)) = flexAlignLeftCenter Then
          .Alignment = vbLeftJustify
        Else
          .Alignment = vbRightJustify
        End If
        
        If grid.Visible Then
          .Visible = True
          .SetFocus
        End If
      End With
    End If
  End If
  DebugMsg "openPopup", 8, "o"
End Sub

'Loads an ASCII file into the grid
'Lines at the beginning of the file that start with commentChar will be skipped
'includeColTitles = true means read the first line of data into the column titles
'if includeColTitles is missing, it is assumed to be false
'If filename is omitted or is "" then a file dialog is used
'if delimiter is omitted or "" then a tab delimited file is assummed
Public Sub LoadFile(Optional commentChar As String = "", _
                    Optional includeColTitles As Boolean = False, _
                    Optional includeRowTitles As Boolean = False, _
                    Optional Filename$ = "", _
                    Optional delimiter$, _
                    Optional emptyCell$ = "", _
                    Optional quote$ = "")
  Dim InFile%, buf$, bytesInFile&, BytesRead&
  Dim r&, c&, minRow&, minCol&
  If IsMissing(delimiter) Then delimiter = vbTab 'would be nice to automatically parse file and figure out what the delimiter should be
SetFilename:
  If Filename = "" Then Filename = browseSaveFilename("Load text file")
  If Filename = "" Then Exit Sub
  If Len(Dir(Filename)) = 0 Then
    If MsgBox("File not found: " & Filename, vbRetryCancel) = vbCancel Then Exit Sub
    Filename = ""
    GoTo SetFilename
  End If
  
  InFile = FreeFile(0)
  Open Filename For Input As #InFile
  If Not EOF(InFile) Then Line Input #InFile, buf
  bytesInFile = LOF(InFile)
  If bytesInFile > 0 Then
    'skip commented lines at top of file
    If commentChar <> "" Then
      While Left(buf, Len(commentChar)) = commentChar And Not EOF(InFile)
        Line Input #InFile, buf
      Wend
    End If
    If includeColTitles Then
      minRow = 1 - FixedRows
      Clear
    Else
      minRow = 1
      ClearData
    End If
    If includeRowTitles Then minCol = 0 Else minCol = FixedCols
    r = minRow
    With progress1
      .Clear
      .Caption = "Loading grid"
      .Progress = 0
      .WindowOpen
    End With
parseLine:
    BytesRead = (Loc(InFile) - 1) * 128
    With progress1
      .Progress = BytesRead / bytesInFile
      .LabelText(1) = "Row " & r
      .LabelText(2) = buf
    End With
    c = minCol
    While buf <> ""
      TextMatrix(r, c) = StrSplit(buf, delimiter, quote)
      If TextMatrix(r, c) = emptyCell Then TextMatrix(r, c) = ""
      c = c + 1
    Wend
    If progress1.Status = "C" Then
      'stop reading file
    ElseIf Not EOF(InFile) Then
      r = r + 1
      Line Input #InFile, buf
      GoTo parseLine
    End If
  End If
  Close InFile
  progress1.WindowClose
  RaiseEvent CommitChange(minRow, r, minCol, cols - minCol - 1)
  
End Sub

Private Sub CopyFont(src As Object, dst As Object)
  On Error Resume Next 'Some objects have only some of the font attributes
  dst.Bold = src.Bold
  dst.Italic = src.Italic
  dst.Charset = src.Charset
  dst.name = src.name
  dst.Size = src.Size
  dst.Strikethru = src.Strikethru
  dst.Underline = src.Underline
  dst.Weight = src.Weight
End Sub

'Non-interactively save or print
'if filenum is zero, print
'if filenum is a valid file handle as returned by FreeFile(0), save
'   filename is prompted for if not specified
'includeColTitles - if true, column titles are written, default is true
'
'headerText may be specified if desired. If grid Header is in use, that value may be desirable
'commentChar is used to mark beginning of header lines in output file, default is "#"
'delimiter is character to separate columns. delimiter = "" to space-pad columns instead
'quote is added before and after each value written
Public Sub SavePrintGridBatch(fileNum%, _
                              Optional includeColTitles As Boolean = True, _
                              Optional includeRowTitles As Boolean = True, _
                              Optional headerText$ = "", _
                              Optional commentChar$ = "#", _
                              Optional Filename$ = "", _
                              Optional delimiter$ = "", _
                              Optional emptyCell$ = "", _
                              Optional quote$ = "")

  Dim r&, c&, ip&, startCol&, endCol&, startRow&, endRow&
  Dim colChars&(), thisChars&
  Dim rdb As Boolean, PadColumns As Boolean
  Dim lstr$, colstr$, ltitl$, rdbfmt$, tstr$
  Dim testDelay&
  
  If delimiter = "" Then
    PadColumns = True
  Else
    PadColumns = False
  End If
retry:
  rdb = False
  
  'On Error GoTo errhandler
  If fileNum = 0 Then 'print to printer
    Dim fp&, tp&
    Dim tFont As New StdFont
    fp = 1
    tp = 1
    Call ShowPrinterX(frmGridPrintSaveLoad, fp, tp, 1, 1, PD_NOSELECTION + PD_NOPAGENUMS + PD_DISABLEPRINTTOFILE)

    If tp < 0 Then Exit Sub 'Cancel selected in print dialog

    CopyFont grid.Font, Printer.Font
    Printer.FontBold = False
    If headerText <> "" Then
      ltitl = ReplaceString(headerText, vbLf, vbLf & PrintMargin)
      Debug.Print "Printer initial coordinates (" & Printer.CurrentX & ", " & Printer.CurrentY & ")"
      Printer.Print
      Printer.Print
      Printer.Print
      Printer.Print PrintMargin & ltitl
      Printer.Print
    End If
    progress1.Caption = "Printing grid"
  Else 'save to file
    If Filename = "" Then Filename = browseSaveFilename
    If Filename = "" Then Exit Sub
    If LCase(Right(Filename, 4)) = ".rdb" Then rdb = True 'output to RDB file
    If Len(Dir(Filename)) > 0 Then Kill Filename
    Open Filename For Output As #fileNum
    If headerText <> "" Then
      ltitl = ReplaceString(headerText, vbLf, vbLf & commentChar & " ")
      Print #fileNum, commentChar & " " & ltitl
      Print #fileNum, commentChar
    End If
    progress1.Caption = "Saving grid"
  End If
  progress1.WindowOpen

  lstr = ""
  rdbfmt = ""
  
  If includeColTitles Then
    startRow = 1 - FixedRows 'output column titles
  Else
    startRow = 1             'skip column titles
  End If
  endRow = rows
  
  If includeRowTitles Then
    startCol = 0             'include all columns
  Else
    startCol = FixedCols     'skip fixed columns (if any)
  End If
  endCol = cols - 1
  
  
  If PadColumns Then
    ReDim colChars(endCol)
    Dim TotalCells&, CellsSoFar&
    TotalCells = (endCol - startCol + 1) * (endRow - startRow + 1)
    CellsSoFar = 0
    progress1.LabelText(1) = "Determining Column Widths"
    progress1.LabelText(2) = "(" & startCol & ", " & startRow & ")"
    progress1.LabelText(4) = "(" & endCol & ", " & endRow & ")"
    For c = startCol To endCol
      colChars(c) = Len(emptyCell)
      For r = startRow To endRow
        thisChars = Len(TextMatrix(r, c))
        If thisChars > colChars(c) Then colChars(c) = thisChars
        
        CellsSoFar = CellsSoFar + 1
        progress1.LabelText(3) = "Column " & c & ", Row " & r
        progress1.Progress = CellsSoFar / TotalCells
        If progress1.Status = "C" Then GoTo CloseAndExit
      Next r
      colChars(c) = colChars(c) + 1 + 2 * Len(quote) 'make sure column is one space wider than longest string
    Next c
  End If
  
  progress1.LabelText(1) = ""
  progress1.LabelText(2) = startRow
  progress1.LabelText(4) = endRow
  For r = startRow To endRow
    progress1.LabelText(3) = "Row " & r
    If endRow > startRow Then progress1.Progress = (r - startRow) / (endRow - startRow)
    lstr = ""
    For c = 0 To endCol
      colstr = TextMatrix(r, c)
      If colstr = "" Then colstr = emptyCell
      colstr = quote & colstr & quote
      If PadColumns Then
        thisChars = Len(colstr)
        If thisChars < colChars(c) Then colstr = colstr & Space(colChars(c) - thisChars)
      Else 'If rdb Then 'include delimiter
        If c < endCol Then colstr = colstr & delimiter
      End If
      lstr = lstr & colstr
    Next c
    If fileNum = 0 Then
      Printer.Print PrintMargin & lstr
    Else
      Print #fileNum, lstr
    End If
    If progress1.Status = "C" Then GoTo CloseAndExit
  Next r
CloseAndExit:
  If fileNum = 0 Then
    Printer.EndDoc
  Else
    Close #fileNum
  End If
  progress1.WindowClose
  Exit Sub
ErrHandler:
  Dim Msg$
  If fileNum = 0 Then
    Msg = "Error printing grid:" & vbCr & Err.Description
  Else
    Msg = "Error saving grid to file '" & Filename & "':" & vbCr & Err.Description
  End If
  Select Case MsgBox(Msg, vbAbortRetryIgnore)
    Case vbAbort:   Exit Sub 'progress1.Visible = False:
    Case vbRetry:   GoTo retry 'progress1.Visible = False:
    Case vbIgnore:  Resume Next
  End Select
End Sub

Public Sub LoadGridInteractive()
  frmGridPrintSaveLoad.LoadGrid Me
End Sub

Public Sub PrintGridInteractive()
  frmGridPrintSaveLoad.PrintGrid Me
End Sub

Public Sub SaveGridInteractive()
  frmGridPrintSaveLoad.SaveGrid Me
End Sub

Public Sub SaveNormalized()

  Dim r&, c&, endCol&
  Dim fname$
  Dim delim$
  Dim fileNum%     'file handle being saved to
  
retry:
  On Error GoTo LeaveSub
  delim = Chr(9)
  fname = browseSaveFilename("Save normalized table as...")
  If fname = "" Then Exit Sub
  If Len(Dir(fname)) > 0 Then Kill fname
  fileNum = FreeFile(0)
  Open fname For Output As #fileNum
  On Error GoTo CloseLeaveSub

  'With progress1
  '  .Min = 1 - FixedRows
  '  .Max = rows
  '  .value = 0
  '  .Visible = True
  'End With
  
  endCol = cols - 1
  
  For r = 1 To rows
    'progress1.value = r
    For c = 1 To endCol
      Print #fileNum, TextMatrix(r, 0) & delim & _
                      TextMatrix(0, c) & delim & _
                      TextMatrix(r, c)
    Next c
  Next r
  
CloseLeaveSub:
  Close #fileNum
LeaveSub:
  'progress1.Visible = False
  Exit Sub

ErrHandler:
  Dim Msg$
  Msg = "Error saving grid to file '" & fname & "':" & vbCr & Err.Description
  Select Case MsgBox(Msg, vbAbortRetryIgnore)
    Case vbAbort:   Exit Sub 'progress1.Visible = False:
    Case vbRetry:   GoTo retry 'progress1.Visible = False:
    Case vbIgnore:  Resume Next
  End Select
End Sub

Private Sub SetCellColorFromDialog()
  Dim r&, c&, bgColor&, fgColor&, txt$
  bgColor = ColorFromDialog(CellBackColor)
  If bgColor <> -1 Then
    txt = colorName(bgColor)
    If IsNumeric(txt) Then txt = "" 'aesthetic: Only put color names in grid, not numbers
    If Brightness(bgColor) > 0.4 Then fgColor = vbBlack Else fgColor = vbWhite
    For r = ChangeFromRow To ChangeToRow
      grid.row = r
      For c = ChangeFromCol To ChangeToCol
        grid.col = c
        grid.CellBackColor = bgColor
        grid.CellForeColor = fgColor
        grid.Text = txt
      Next c
    Next r
    RaiseEvent CommitChange(agdRow(ChangeFromRow), agdRow(ChangeToRow), agdCol(ChangeFromCol), agdCol(ChangeToCol))
  End If
End Sub

Private Sub SetFontFromDialog()
  cdlg.FontBold = gridFontBold
  cdlg.FontItalic = gridFontItalic
  cdlg.FontName = gridFontName
  cdlg.FontSize = gridFontSize
  cdlg.FontStrikethru = False
  cdlg.FontUnderline = gridFontUnderline
  cdlg.DialogTitle = "Select a font"
  cdlg.CancelError = True
  On Error GoTo DontSetFont
  cdlg.flags = cdlCFBoth + cdlCFScalableOnly + cdlCFWYSIWYG 'cdlCFScreenFonts  'cdlCFWYSIWYG, cdlCFTTOnly don't work
  cdlg.ShowFont
  gridFontBold = cdlg.FontBold
  gridFontItalic = cdlg.FontItalic
  gridFontName = cdlg.FontName
  gridFontSize = cdlg.FontSize
  gridFontUnderline = cdlg.FontUnderline
DontSetFont:
End Sub

Private Sub OpenPopupMenu()
  mnuPopupSelectionToggle.Checked = SelectionToggle
  mnuPopupRange.Caption = RangeText(col)
  If mnuPopupRange.Caption = "" Then
    mnuPopupRange.Visible = False
  Else
    mnuPopupRange.Visible = True
  End If
  mnuSepRange.Visible = mnuPopupRange.Visible
  mnuPopupSelectionToggle.Visible = DisjointSelections
  PopupMenu mnuPopup
    'If grid.row <> grid.MouseRow Then grid.row = grid.MouseRow
    'If grid.col <> grid.MouseCol Then grid.col = grid.MouseCol
    'Dim spec As ColSpecs
    'spec = ColSpec(col)
    'If Not spec.Editable Then Exit Sub 'don't do anything
    'openPopup
    'If nValidValues > 0 Then Exit Sub 'popup combobox will display valid values
    'If txtPopup.Visible Then txtPopup.ShowRange
End Sub

Public Sub paste()
  grid_KeyPress 22 'ctrl-V = paste
End Sub

Private Sub PasteFromClipboard(gridControl As MSFlexGrid)
  Dim c&, r&, fromCol&, toCol&, fromRow&, toRow&, txt$
  With gridControl
    If .col <= .ColSel Then fromCol = .col: toCol = .ColSel Else fromCol = .ColSel: toCol = .col: .col = toCol: .ColSel = fromCol
    If .row <= .RowSel Then fromRow = .row: toRow = .RowSel Else fromRow = .RowSel: toRow = .row: .row = toRow: .RowSel = fromRow
    If Not ForcePaste Then  'Check whether selected cols are editable
      For c = fromCol To toCol
        If ColSpec(c).Editable = False Then
          MsgBox "Column " & c & " of this table is not editable. " & vbCr & "Select only editable columns before pasting."
          Exit Sub
        End If
      Next c
    End If
    
    Dim nTargetRows&, pasteTxt$
    Dim nPasteRows&, rowPos&
    Dim nPasteCols&, colPos&
    Dim SetRowCol As Boolean
    
    nTargetRows = toRow - fromRow + 1
    On Error GoTo StupidWindozeClipboard
    pasteTxt = Clipboard.GetText(vbCFText)
    On Error GoTo 0
    
    nPasteRows = 1
    nPasteCols = 1
    rowPos = InStr(1, pasteTxt, vbCr)
    colPos = InStr(1, pasteTxt, vbTab)
    While colPos > 0 And colPos < rowPos
      nPasteCols = nPasteCols + 1
      colPos = InStr(colPos + 1, pasteTxt, vbTab)
    Wend
    While rowPos > 0
      nPasteRows = nPasteRows + 1
      rowPos = InStr(rowPos + 1, pasteTxt, vbCr)
    Wend
    If Right(pasteTxt, 1) = vbCr Or Right(pasteTxt, 2) = vbCrLf Then
      nPasteRows = nPasteRows - 1
    End If
    DebugMsg "nPasteRows=" & nPasteRows, 5, "t"
    DebugMsg "nPasteCols=" & nPasteCols, 5, "t"
    
    If nPasteCols > (toCol - fromCol + 1) Then 'Try to expand area to paste all
      For c = toCol + 1 To nPasteCols + fromCol - 1
        If c < .cols Then
          If ColSpec(c).Editable Then
            toCol = toCol + 1
          Else
            c = nPasteCols
          End If
        Else
          c = nPasteCols
        End If
      Next
      SetRowCol = True
    End If
    
    If nPasteRows > (toRow - fromRow + 1) Then 'Try to expand area to paste all
      toRow = fromRow + nPasteRows - 1
      SetRowCol = True
    End If
    If toRow >= .rows - .FixedRows Then .rows = toRow + .FixedRows
    If SetRowCol Then
      .row = fromRow
      .col = fromCol
      .ColSel = toCol
      .RowSel = toRow
    End If
    .Clip = pasteTxt
    If MaxOccRow < agdRow(toRow) Then MaxOccRow = agdRow(toRow)
    
    'Paste multiple copies if paste area is larger than copied text
    If nTargetRows >= nPasteRows * 2 Then
      'If Int(nTargetRows / nPasteRows) * nPasteRows = nTargetRows Then
      For rowPos = fromRow + nPasteRows To toRow - nPasteRows + 1 Step nPasteRows
        .row = rowPos
        .RowSel = rowPos + nPasteRows - 1
        .ColSel = toCol
        .Clip = pasteTxt
      Next rowPos
      .row = fromRow
      .RowSel = toRow
      'End If
    End If
    'Now clean up extra linefeeds (the above assignment _should_ have done this)
    'and go through TextMatrix routine which will call TestValueSetCellBackColor
    For c = fromCol To toCol
      For r = fromRow To toRow
        txt = grid.TextMatrix(r, c)
CheckForControlChars:
        If Len(txt) = 0 Then GoTo txtFinished
        If Asc(txt) >= 32 Then GoTo txtFinished
        txt = Mid(txt, 2)
        GoTo CheckForControlChars
txtFinished:
        TextMatrix(agdRow(r), agdCol(c)) = txt
      Next r
    Next c
  End With
  RaiseEvent CommitChange(agdRow(fromRow), agdRow(toRow), agdCol(fromCol), agdCol(toCol))
  Exit Sub
StupidWindozeClipboard:
  'Sleep 500 'vb keeps forgetting how to sleep, so we use the following loop:
  Dim zzz&
  For zzz = 1 To 200
    DoEvents
  Next zzz
  pasteTxt = Clipboard.GetText(vbCFText)
  Resume Next
End Sub
