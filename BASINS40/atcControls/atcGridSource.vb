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

  Public Event ChangedRows(ByVal aRows As Integer) 'Number of Rows changed
  Public Event ChangedColumns(ByVal aColumns As Integer) 'Number of Columns changed

  'Value at the given row and column was changed
  Event ChangedValue(ByVal aRow As Integer, ByVal aColumn As Integer)

  'Value in each cell
  Overridable Property CellValue(ByVal aRow As Integer, ByVal aColumn As Integer) As String
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
        RaiseEvent ChangedValue(aRow, aColumn)
      End If
    End Set
  End Property

  'Number of rows
  Overridable Property Rows() As Integer
    Get
      Return pRows
    End Get
    Set(ByVal newRows As Integer)
      If newRows <> pRows Then
        Dim lastRowCopied As Integer
        If pRows > newRows Then
          lastRowCopied = newRows
        Else
          lastRowCopied = pRows
        End If

        Dim newValues(newRows, Columns) As String
        For iRow As Integer = 0 To lastRowCopied
          pRows = newRows
          For iColumn As Integer = 0 To pColumns
            newValues(iRow, iColumn) = pValues(iRow, iColumn)
          Next
        Next
        pRows = newRows
        pValues = newValues
        RaiseEvent ChangedRows(newRows)
      End If
    End Set
  End Property

  'Number of columns
  Overridable Property Columns() As Integer
    Get
      Return pColumns
    End Get
    Set(ByVal newColumns As Integer)
      If newColumns <> pColumns Then
        pColumns = newColumns
        ReDim Preserve pValues(Rows, pColumns)
        RaiseEvent ChangedColumns(newColumns)
      End If
    End Set
  End Property

  'Alignment of the contents of each cell
  Overridable Property Alignment(ByVal aRow As Integer, ByVal aColumn As Integer) As atcAlignment
    Get
      Return atcAlignment.HAlignLeft + atcAlignment.VAlignCenter
    End Get
    Set(ByVal Value As atcAlignment)

    End Set
  End Property

  Public Overrides Function ToString() As String
    Dim lCellValue As String
    Dim lAddTabs() As Boolean
    ReDim lAddTabs(Me.Columns)

    For iCol As Integer = 0 To Me.Columns - 1
      For iRow As Integer = 0 To Me.Rows - 1
        If Me.CellValue(iRow, iCol).IndexOf(vbTab) > -1 Then
          lAddTabs(iCol) = True
          Exit For
        End If
      Next
    Next

    For iRow As Integer = 0 To Me.Rows - 1
      For iCol As Integer = 0 To Me.Columns - 1
        lCellValue = Me.CellValue(iRow, iCol)
        ToString &= lCellValue & vbTab
        'Some modified values contain vbTab(+10%), add a tab to those that don't
        If lAddTabs(iCol) AndAlso lCellValue.IndexOf(vbTab) < 0 Then ToString &= vbTab
      Next
      ToString &= vbCrLf
    Next
  End Function
End Class
