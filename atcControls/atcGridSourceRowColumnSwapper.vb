Public Class atcGridSourceRowColumnSwapper
    Inherits atcGridSource

    Private WithEvents pSource As atcGridSource

    Private pSwapRowsColumns As Boolean = False

    Public Shadows Event ChangedRows(ByVal aRows As Integer) 'Number of Rows changed
    Public Shadows Event ChangedColumns(ByVal aColumns As Integer) 'Number of Columns changed

    'Value at the given row and column was changed
    Public Shadows Event ChangedValue(ByVal aRow As Integer, ByVal aColumn As Integer)

    'RowColumnSwapper swaps references to rows and columns and passes all calls through to aSource.
    'aSource needs to return all the appropriate values for the default (not swapped) orientation.
    Sub New(ByVal aSource As atcGridSource)
        pSource = aSource
    End Sub

    'Value in each cell (not overridable to hide row/column swapping from inheritors)
    Overrides Property CellValue(ByVal aRow As Integer, ByVal aColumn As Integer) As String
        Get
            If pSwapRowsColumns Then
                Return pSource.CellValue(aColumn, aRow)
            Else
                Return pSource.CellValue(aRow, aColumn)
            End If
        End Get
        Set(ByVal newValue As String)
            If pSwapRowsColumns Then
                pSource.CellValue(aColumn, aRow) = newValue
            Else
                pSource.CellValue(aRow, aColumn) = newValue
            End If
            RaiseEvent ChangedValue(aRow, aColumn)
        End Set
    End Property

    'Background color of each cell (not overridable to hide row/column swapping from inheritors)
    Overrides Property CellColor(ByVal aRow As Integer, ByVal aColumn As Integer) As Color
        Get
            If pSwapRowsColumns Then
                Return pSource.CellColor(aColumn, aRow)
            Else
                Return pSource.CellColor(aRow, aColumn)
            End If
        End Get
        Set(ByVal newValue As Color)
            If pSwapRowsColumns Then
                pSource.CellColor(aColumn, aRow) = newValue
            Else
                pSource.CellColor(aRow, aColumn) = newValue
            End If
            'RaiseEvent ChangedValue(aRow, aColumn)
        End Set
    End Property

    'True if cell is currently selected (not overridable to hide row/column swapping from inheritors)
    Overrides Property CellSelected(ByVal aRow As Integer, ByVal aColumn As Integer) As Boolean
        Get
            If pSwapRowsColumns Then
                Return pSource.CellSelected(aColumn, aRow)
            Else
                Return pSource.CellSelected(aRow, aColumn)
            End If
        End Get
        Set(ByVal newValue As Boolean)
            If pSwapRowsColumns Then
                pSource.CellSelected(aColumn, aRow) = newValue
            Else
                pSource.CellSelected(aRow, aColumn) = newValue
            End If
            'RaiseEvent ChangedValue(aRow, aColumn)
        End Set
    End Property

    'True if cell is user-editable (not overridable to hide row/column swapping from inheritors)
    Overrides Property CellEditable(ByVal aRow As Integer, ByVal aColumn As Integer) As Boolean
        Get
            If pSwapRowsColumns Then
                Return pSource.CellEditable(aColumn, aRow)
            Else
                Return pSource.CellEditable(aRow, aColumn)
            End If
        End Get
        Set(ByVal newValue As Boolean)
            If pSwapRowsColumns Then
                pSource.CellEditable(aColumn, aRow) = newValue
            Else
                pSource.CellEditable(aRow, aColumn) = newValue
            End If
            'RaiseEvent ChangedValue(aRow, aColumn)
        End Set
    End Property

    'Number of rows (not overridable to hide row/column swapping from inheritors)
    Overrides Property Rows() As Integer
        Get
            If pSwapRowsColumns Then
                Return pSource.Columns
            Else
                Return pSource.Rows
            End If
        End Get
        Set(ByVal newValue As Integer)
            If pSwapRowsColumns Then
                pSource.Columns = newValue
            Else
                pSource.Rows = newValue
            End If
            RaiseEvent ChangedRows(newValue)
        End Set
    End Property

    'Number of columns (not overridable to hide row/column swapping from inheritors)
    Overrides Property Columns() As Integer
        Get
            If pSwapRowsColumns Then
                Return pSource.Rows
            Else
                Return pSource.Columns
            End If
        End Get
        Set(ByVal newValue As Integer)
            If pSwapRowsColumns Then
                pSource.Rows = newValue
            Else
                pSource.Columns = newValue
            End If
            RaiseEvent ChangedColumns(newValue)
        End Set
    End Property

    'Number of header rows that do not scroll (not overridable to hide row/column swapping from inheritors)
    Overrides Property FixedRows() As Integer
        Get
            If pSwapRowsColumns Then
                Return pSource.FixedColumns
            Else
                Return pSource.FixedRows
            End If
        End Get
        Set(ByVal newValue As Integer)
            If pSwapRowsColumns Then
                pSource.FixedColumns = newValue
            Else
                pSource.FixedRows = newValue
            End If
        End Set
    End Property

    'Number of columns that do not scroll (not overridable to hide row/column swapping from inheritors)
    Overrides Property FixedColumns() As Integer
        Get
            If pSwapRowsColumns Then
                Return pSource.FixedRows
            Else
                Return pSource.FixedColumns
            End If
        End Get
        Set(ByVal newValue As Integer)
            If pSwapRowsColumns Then
                pSource.FixedRows = newValue
            Else
                pSource.FixedColumns = newValue
            End If
        End Set
    End Property

    'Alignment of the contents of each cell (not overridable to hide row/column swapping from inheritors)
    Overrides Property Alignment(ByVal aRow As Integer, ByVal aColumn As Integer) As atcAlignment
        Get
            If pSwapRowsColumns Then
                Return pSource.Alignment(aColumn, aRow)
            Else
                Return pSource.Alignment(aRow, aColumn)
            End If
        End Get
        Set(ByVal newValue As atcAlignment)
            If pSwapRowsColumns Then
                pSource.Alignment(aColumn, aRow) = newValue
            Else
                pSource.Alignment(aRow, aColumn) = newValue
            End If
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

End Class