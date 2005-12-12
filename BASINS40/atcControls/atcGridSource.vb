Public Enum atcAlignment
  HAlignLeft = 1
  HAlignRight = 2
  HAlignCenter = 4
  HAlignDecimal = 8
  HAlign = 1 + 2 + 4 + 8
  VAlign = 1024 + 2048 + 4096
  VAlignTop = 1024
  VAlignBottom = 2048
  VAlignCenter = 4096
End Enum

Public Class atcGridSource

  Private pRows As Integer = 0
  Private pColumns As Integer = 0
  Private pFixedRows As Integer = 0
  Private pFixedColumns As Integer = 0
  Private pValues(,) As String
  Private pColors(,) As Color
  Private pSelected(,) As Boolean
  Private pEditable(,) As Boolean
  Private pAlignment(,) As atcAlignment
  Private pColorCells As Boolean = False
  Private pSwapRowsColumns As Boolean = False

  Public Event ChangedRows(ByVal aRows As Integer) 'Number of Rows changed
  Public Event ChangedColumns(ByVal aColumns As Integer) 'Number of Columns changed

  'Value at the given row and column was changed
  Event ChangedValue(ByVal aRow As Integer, ByVal aColumn As Integer)

  'Value in each cell (not overridable to hide row/column swapping from inheritors)
  Property CellValue(ByVal aRow As Integer, ByVal aColumn As Integer) As String
    Get
      If pSwapRowsColumns Then
        Return ProtectedCellValue(aColumn, aRow)
      Else
        Return ProtectedCellValue(aRow, aColumn)
      End If
    End Get
    Set(ByVal newValue As String)
      If pSwapRowsColumns Then
        ProtectedCellValue(aColumn, aRow) = newValue
      Else
        ProtectedCellValue(aRow, aColumn) = newValue
      End If
      RaiseEvent ChangedValue(aRow, aColumn)
    End Set
  End Property

  'True to use CellColor, False to not color individual cells
  Property ColorCells() As Boolean
    Get
      Return pColorCells
    End Get
    Set(ByVal newValue As Boolean)
      pColorCells = newValue
    End Set
  End Property

  'Background color of each cell (not overridable to hide row/column swapping from inheritors)
  Property CellColor(ByVal aRow As Integer, ByVal aColumn As Integer) As Color
    Get
      If pSwapRowsColumns Then
        Return ProtectedCellColor(aColumn, aRow)
      Else
        Return ProtectedCellColor(aRow, aColumn)
      End If
    End Get
    Set(ByVal newValue As Color)
      If pSwapRowsColumns Then
        ProtectedCellColor(aColumn, aRow) = newValue
      Else
        ProtectedCellColor(aRow, aColumn) = newValue
      End If
      'RaiseEvent ChangedValue(aRow, aColumn)
    End Set
  End Property

  'True if cell is currently selected (not overridable to hide row/column swapping from inheritors)
  Property CellSelected(ByVal aRow As Integer, ByVal aColumn As Integer) As Boolean
    Get
      If pSwapRowsColumns Then
        Return ProtectedCellSelected(aColumn, aRow)
      Else
        Return ProtectedCellSelected(aRow, aColumn)
      End If
    End Get
    Set(ByVal newValue As Boolean)
      If pSwapRowsColumns Then
        ProtectedCellSelected(aColumn, aRow) = newValue
      Else
        ProtectedCellSelected(aRow, aColumn) = newValue
      End If
      'RaiseEvent ChangedValue(aRow, aColumn)
    End Set
  End Property

  'True if cell is user-editable (not overridable to hide row/column swapping from inheritors)
  Property CellEditable(ByVal aRow As Integer, ByVal aColumn As Integer) As Boolean
    Get
      If pSwapRowsColumns Then
        Return ProtectedCellEditable(aColumn, aRow)
      Else
        Return ProtectedCellEditable(aRow, aColumn)
      End If
    End Get
    Set(ByVal newValue As Boolean)
      If pSwapRowsColumns Then
        ProtectedCellEditable(aColumn, aRow) = newValue
      Else
        ProtectedCellEditable(aRow, aColumn) = newValue
      End If
      'RaiseEvent ChangedValue(aRow, aColumn)
    End Set
  End Property

  'Number of rows (not overridable to hide row/column swapping from inheritors)
  Property Rows() As Integer
    Get
      If pSwapRowsColumns Then
        Return ProtectedColumns
      Else
        Return ProtectedRows
      End If
    End Get
    Set(ByVal newValue As Integer)
      If pSwapRowsColumns Then
        ProtectedColumns = newValue
      Else
        ProtectedRows = newValue
      End If
      RaiseEvent ChangedRows(newValue)
    End Set
  End Property

  'Number of columns (not overridable to hide row/column swapping from inheritors)
  Property Columns() As Integer
    Get
      If pSwapRowsColumns Then
        Return ProtectedRows
      Else
        Return ProtectedColumns
      End If
    End Get
    Set(ByVal newValue As Integer)
      If pSwapRowsColumns Then
        ProtectedRows = newValue
      Else
        ProtectedColumns = newValue
      End If
      RaiseEvent ChangedColumns(newValue)
    End Set
  End Property

  'Number of header rows that do not scroll (not overridable to hide row/column swapping from inheritors)
  Property FixedRows() As Integer
    Get
      If pSwapRowsColumns Then
        Return ProtectedFixedColumns
      Else
        Return ProtectedFixedRows
      End If
    End Get
    Set(ByVal newValue As Integer)
      If pSwapRowsColumns Then
        ProtectedFixedColumns = newValue
      Else
        ProtectedFixedRows = newValue
      End If
    End Set
  End Property

  'Number of columns that do not scroll (not overridable to hide row/column swapping from inheritors)
  Property FixedColumns() As Integer
    Get
      If pSwapRowsColumns Then
        Return ProtectedFixedRows
      Else
        Return ProtectedFixedColumns
      End If
    End Get
    Set(ByVal newValue As Integer)
      If pSwapRowsColumns Then
        ProtectedFixedRows = newValue
      Else
        ProtectedFixedColumns = newValue
      End If
    End Set
  End Property

  'Alignment of the contents of each cell (not overridable to hide row/column swapping from inheritors)
  Property Alignment(ByVal aRow As Integer, ByVal aColumn As Integer) As atcAlignment
    Get
      If pSwapRowsColumns Then
        Return ProtectedAlignment(aColumn, aRow)
      Else
        Return ProtectedAlignment(aRow, aColumn)
      End If
    End Get
    Set(ByVal newValue As atcAlignment)
      If pSwapRowsColumns Then
        ProtectedAlignment(aColumn, aRow) = newValue
      Else
        ProtectedAlignment(aRow, aColumn) = newValue
      End If
    End Set
  End Property

  'True if aRow or aColumn does not exist
  Protected Function InvalidRowOrColumn(ByVal aRow As Integer, ByVal aColumn As Integer) As Boolean
    Return (aRow >= Rows _
     OrElse aColumn >= Columns _
     OrElse aRow < 0 _
     OrElse aColumn < 0)
  End Function

  'Expands Rows and/or Columns if needed to reach from 0,0 to aRow, aColumn
  Protected Sub ExpandRowsColumns(ByVal aRow As Integer, ByVal aColumn As Integer)
    If aRow > Rows + 1 Then Rows = aRow + 1
    If aColumn > Columns + 1 Then Columns = aColumn + 1
  End Sub

  'Override this instead of CellValue
  Protected Overridable Property ProtectedCellValue(ByVal aRow As Integer, ByVal aColumn As Integer) As String
    Get
      If pValues Is Nothing OrElse InvalidRowOrColumn(aRow, aColumn) Then
        Return ""
      Else
        Return pValues(aRow, aColumn)
      End If
    End Get
    Set(ByVal newValue As String)
      If pValues Is Nothing Then
        ReDim pValues(pRows, pColumns)
      End If
      ExpandRowsColumns(aRow, aColumn)
      If pValues(aRow, aColumn) <> newValue Then
        pValues(aRow, aColumn) = newValue
      End If
    End Set
  End Property

  'Override this instead of CellColor
  Protected Overridable Property ProtectedCellColor(ByVal aRow As Integer, ByVal aColumn As Integer) As Color
    Get
      If pColors Is Nothing OrElse InvalidRowOrColumn(aRow, aColumn) Then
        Return System.Drawing.SystemColors.Window
      Else
        Return pColors(aRow, aColumn)
      End If
    End Get
    Set(ByVal newValue As Color)
      If pColors Is Nothing Then
        ReDim pColors(pRows, pColumns)
      End If
      ExpandRowsColumns(aRow, aColumn)
      pColors(aRow, aColumn) = newValue
    End Set
  End Property

  'Override this instead of CellSelected
  Protected Overridable Property ProtectedCellSelected(ByVal aRow As Integer, ByVal aColumn As Integer) As Boolean
    Get
      If pSelected Is Nothing OrElse InvalidRowOrColumn(aRow, aColumn) Then
        Return False
      Else
        Return pSelected(aRow, aColumn)
      End If
    End Get
    Set(ByVal newValue As Boolean)
      ExpandRowsColumns(aRow, aColumn)
      If pSelected Is Nothing Then
        ReDim pSelected(pRows, pColumns)
      End If
      pSelected(aRow, aColumn) = newValue
    End Set
  End Property

  'Override this instead of CellEditable
  Protected Overridable Property ProtectedCellEditable(ByVal aRow As Integer, ByVal aColumn As Integer) As Boolean
    Get
      If pEditable Is Nothing OrElse InvalidRowOrColumn(aRow, aColumn) Then
        Return False
      Else
        Return pEditable(aRow, aColumn)
      End If
    End Get
    Set(ByVal newValue As Boolean)
      If newValue <> ProtectedCellEditable(aRow, aColumn) Then
        ExpandRowsColumns(aRow, aColumn)
        If pEditable Is Nothing Then
          ReDim pEditable(pRows, pColumns)
        End If
        pEditable(aRow, aColumn) = newValue
      End If
    End Set
  End Property

  'Override this instead of Columns
  Protected Overridable Property ProtectedColumns() As Integer
    Get
      Return pColumns
    End Get
    Set(ByVal newValue As Integer)
      pColumns = newValue
      ReDim Preserve pValues(pRows, pColumns)
      If Not pColors Is Nothing Then ReDim Preserve pColors(pRows, pColumns)
      If Not pSelected Is Nothing Then ReDim Preserve pSelected(pRows, pColumns)
      If Not pEditable Is Nothing Then ReDim Preserve pEditable(pRows, pColumns)
      If Not pAlignment Is Nothing Then ReDim Preserve pAlignment(pRows, pColumns)
    End Set
  End Property

  'Override this instead of FixedColumns
  Protected Overridable Property ProtectedFixedColumns() As Integer
    Get
      Return pFixedColumns
    End Get
    Set(ByVal newValue As Integer)
      pFixedColumns = newValue
    End Set
  End Property

  'Override this instead of FixedRows
  Protected Overridable Property ProtectedFixedRows() As Integer
    Get
      Return pFixedRows
    End Get
    Set(ByVal newValue As Integer)
      pFixedRows = newValue
    End Set
  End Property

  'Override this instead of Rows
  Protected Overridable Property ProtectedRows() As Integer
    Get
      Return pRows
    End Get
    Set(ByVal aNewRows As Integer)
      If aNewRows <> pRows Then
        Dim lastRowCopied As Integer
        If pRows > aNewRows Then
          lastRowCopied = aNewRows
        Else
          lastRowCopied = pRows
        End If

        Dim newValues(aNewRows, Columns) As String
        For iRow As Integer = 0 To lastRowCopied
          pRows = aNewRows
          For iColumn As Integer = 0 To pColumns
            newValues(iRow, iColumn) = pValues(iRow, iColumn)
          Next
        Next
        pRows = aNewRows
        pValues = newValues
        'TODO: copy these arrays, currently they all end up full of Nothing
        If Not pColors Is Nothing Then ReDim pColors(pRows, pColumns)
        If Not pSelected Is Nothing Then ReDim pSelected(pRows, pColumns)
        If Not pEditable Is Nothing Then ReDim pEditable(pRows, pColumns)
        If Not pAlignment Is Nothing Then ReDim pAlignment(pRows, pColumns)
      End If
    End Set
  End Property

  'Override this instead of Alignment
  Protected Overridable Property ProtectedAlignment(ByVal aRow As Integer, ByVal aColumn As Integer) As atcAlignment
    Get
      If pAlignment Is Nothing OrElse InvalidRowOrColumn(aRow, aColumn) Then
        Return atcAlignment.HAlignLeft + atcAlignment.VAlignCenter
      Else
        Return pAlignment(aRow, aColumn)
      End If
    End Get
    Set(ByVal newValue As atcAlignment)
      If pAlignment Is Nothing Then
        ReDim pAlignment(pRows, pColumns)
      End If
      ExpandRowsColumns(aRow, aColumn)
      pAlignment(aRow, aColumn) = newValue
    End Set
  End Property

  'True for rows and columns to be swapped, false for normal orientation
  Public Property SwapRowsColumns() As Boolean
    Get
      Return pSwapRowsColumns
    End Get
    Set(ByVal newValue As Boolean)
      pSwapRowsColumns = newValue
    End Set
  End Property

  Public Overrides Function ToString() As String
    Dim lCellValue As String
    Dim lAddTabs() As Boolean
    ReDim lAddTabs(Me.Columns)
    Dim lMaxCol As Integer = Columns - 1
    Dim lMaxRow As Integer = Rows - 1

    For iCol As Integer = 0 To lMaxCol
      For iRow As Integer = 0 To lMaxRow
        If CellValue(iRow, iCol).IndexOf(vbTab) > -1 Then
          lAddTabs(iCol) = True
          Exit For
        End If
      Next
    Next

    For iRow As Integer = 0 To lMaxRow
      For iCol As Integer = 0 To lMaxCol
        lCellValue = CellValue(iRow, iCol)
        ToString &= lCellValue
        'Some modified values contain "<tab>(+10%)", add a tab to those that don't
        If lAddTabs(iCol) AndAlso lCellValue.IndexOf(vbTab) < 0 Then ToString &= vbTab
        If iCol < lMaxCol Then ToString &= vbTab 'Add tab afer each column except for last 
      Next
      ToString &= vbCrLf
    Next
  End Function
End Class
