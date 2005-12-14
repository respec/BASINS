Public Enum atcAlignment
  HAlignLeft = 1
  HAlignRight = 2
  HAlignCenter = 4
  HAlignDecimal = 8
  HAlign = 1 + 2 + 4 + 8
  VAlignTop = 1024
  VAlignBottom = 2048
  VAlignCenter = 4096
  VAlign = 1024 + 2048 + 4096
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

  Public Event ChangedRows(ByVal aRows As Integer) 'Number of Rows changed
  Public Event ChangedColumns(ByVal aColumns As Integer) 'Number of Columns changed

  'Value at the given row and column was changed
  Event ChangedValue(ByVal aRow As Integer, ByVal aColumn As Integer)

  'Value in each cell 
  Overridable Property CellValue(ByVal aRow As Integer, ByVal aColumn As Integer) As String
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

  'True to use CellColor, False to not color individual cells
  Overridable Property ColorCells() As Boolean
    Get
      Return pColorCells
    End Get
    Set(ByVal newValue As Boolean)
      pColorCells = newValue
    End Set
  End Property

  'Background color of each cell 
  Overridable Property CellColor(ByVal aRow As Integer, ByVal aColumn As Integer) As Color
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

  'True if cell is currently selected 
  Overridable Property CellSelected(ByVal aRow As Integer, ByVal aColumn As Integer) As Boolean
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

  'True if cell is user-editable 
  Overridable Property CellEditable(ByVal aRow As Integer, ByVal aColumn As Integer) As Boolean
    Get
      If pEditable Is Nothing OrElse InvalidRowOrColumn(aRow, aColumn) Then
        Return False
      Else
        Return pEditable(aRow, aColumn)
      End If
    End Get
    Set(ByVal newValue As Boolean)
      If newValue <> CellEditable(aRow, aColumn) Then
        ExpandRowsColumns(aRow, aColumn)
        If pEditable Is Nothing Then
          ReDim pEditable(pRows, pColumns)
        End If
        pEditable(aRow, aColumn) = newValue
      End If
    End Set
  End Property

  'Number of rows 
  Overridable Property Rows() As Integer
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
        Dim newColors(,) As Color
        Dim newSelected(,) As Boolean
        Dim newEditable(,) As Boolean
        Dim newAlignment(,) As atcAlignment

        If Not pColors Is Nothing Then ReDim newColors(pRows, pColumns)
        If Not pSelected Is Nothing Then ReDim newSelected(pRows, pColumns)
        If Not pEditable Is Nothing Then ReDim newEditable(pRows, pColumns)
        If Not pAlignment Is Nothing Then ReDim newAlignment(pRows, pColumns)

        For iRow As Integer = 0 To lastRowCopied
          pRows = aNewRows
          For iColumn As Integer = 0 To pColumns
            newValues(iRow, iColumn) = pValues(iRow, iColumn)
            If Not pColors Is Nothing Then newColors(iRow, iColumn) = pColors(iRow, iColumn)
            If Not pSelected Is Nothing Then newSelected(iRow, iColumn) = pSelected(iRow, iColumn)
            If Not pEditable Is Nothing Then newEditable(iRow, iColumn) = pEditable(iRow, iColumn)
            If Not pAlignment Is Nothing Then newAlignment(iRow, iColumn) = pAlignment(iRow, iColumn)
          Next
        Next
        pValues = newValues
        If Not pColors Is Nothing Then pColors = newColors
        If Not pSelected Is Nothing Then pSelected = newSelected
        If Not pEditable Is Nothing Then pEditable = newEditable
        If Not pAlignment Is Nothing Then pAlignment = newAlignment

        pRows = aNewRows
        RaiseEvent ChangedRows(aNewRows)
      End If
    End Set
  End Property

  'Number of columns 
  Overridable Property Columns() As Integer
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

  'Number of header rows that do not scroll 
  Overridable Property FixedRows() As Integer
    Get
      Return pFixedRows
    End Get
    Set(ByVal newValue As Integer)
      pFixedRows = newValue
    End Set
  End Property

  'Number of columns that do not scroll 
  Overridable Property FixedColumns() As Integer
    Get
      Return pFixedColumns
    End Get
    Set(ByVal newValue As Integer)
      pFixedColumns = newValue
    End Set
  End Property

  'Alignment of the contents of each cell 
  Overridable Property Alignment(ByVal aRow As Integer, ByVal aColumn As Integer) As atcAlignment
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
