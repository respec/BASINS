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
  Private pValues(,) As String
  Private pColors(,) As Color
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

  'Override this instead of CellValue
  Protected Overridable Property ProtectedCellValue(ByVal aRow As Integer, ByVal aColumn As Integer) As String
    Get
      If pValues Is Nothing OrElse aRow >= Rows OrElse aColumn >= Columns Then
        Return ""
      Else
        Return pValues(aRow, aColumn)
      End If
    End Get
    Set(ByVal newValue As String)
      If pValues Is Nothing Then
        ReDim pValues(Rows, Columns)
      End If
      If aRow > Rows + 1 Then Rows = aRow + 1
      If aColumn > Columns + 1 Then Columns = aRow + 1
      If pValues(aRow, aColumn) <> newValue Then
        pValues(aRow, aColumn) = newValue
      End If
    End Set
  End Property

  'Override this instead of CellColor
  Protected Overridable Property ProtectedCellColor(ByVal aRow As Integer, ByVal aColumn As Integer) As Color
    Get
      If pColors Is Nothing OrElse aRow >= Rows OrElse aColumn >= Columns Then
        Return Color.White
      Else
        Return pColors(aRow, aColumn)
      End If
    End Get
    Set(ByVal newValue As Color)
      If pColors Is Nothing Then
        ReDim pColors(Rows, Columns)
      End If
      If aRow > Rows + 1 Then Rows = aRow + 1
      If aColumn > Columns + 1 Then Columns = aRow + 1
      pColors(aRow, aColumn) = newValue
    End Set
  End Property

  'Override this instead of Columns
  Protected Overridable Property ProtectedColumns() As Integer
    Get
      Return pColumns
    End Get
    Set(ByVal newValue As Integer)
      pColumns = newValue
      ReDim Preserve pValues(Rows, pColumns)
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
      End If
    End Set
  End Property

  'Override this instead of Alignment
  Protected Overridable Property ProtectedAlignment(ByVal aRow As Integer, ByVal aColumn As Integer) As atcAlignment
    Get
      Return atcAlignment.HAlignLeft + atcAlignment.VAlignCenter
    End Get
    Set(ByVal newValue As atcAlignment)
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
