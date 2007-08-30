Public Class atcGridSourceTable
    Inherits atcGridSource

    Dim pTable As atcUtility.atcTable

    Public Property Table() As atcUtility.atcTable
        Get
            Return pTable
        End Get
        Set(ByVal newValue As atcUtility.atcTable)
            pTable = newValue
        End Set
    End Property

    'Value in each cell 
    Overrides Property CellValue(ByVal aRow As Integer, ByVal aColumn As Integer) As String
        Get
            If pTable Is Nothing OrElse InvalidRowOrColumn(aRow, aColumn) Then
                Return ""
            Else
                aColumn += 1
                If aRow = 0 Then
                    Return pTable.FieldName(aColumn)
                Else
                    If aRow <> pTable.CurrentRecord Then pTable.CurrentRecord = aRow
                    Return pTable.Value(aColumn)
                End If
            End If
        End Get
        Set(ByVal newValue As String)
            If pTable Is Nothing Then

            Else
                If InvalidRowOrColumn(aRow, aColumn) Then
                    'LogDbg("Could not set CellValue(" & aRow & ", " & aColumn & ") to " & newValue)
                Else
                    aColumn += 1
                    If aRow <> pTable.CurrentRecord Then pTable.CurrentRecord = aRow
                    pTable.Value(aColumn) = newValue
                End If
            End If
        End Set
    End Property

    'Background color of each cell 
    Overrides Property CellColor(ByVal aRow As Integer, ByVal aColumn As Integer) As Color
        Get
            If aRow = 0 Then
                Return System.Drawing.SystemColors.ControlLight
            Else
                Return System.Drawing.SystemColors.Window
            End If
        End Get
        Set(ByVal newValue As Color)
        End Set
    End Property

    'True if cell is currently selected 
    Overrides Property CellSelected(ByVal aRow As Integer, ByVal aColumn As Integer) As Boolean
        Get
            Return False
        End Get
        Set(ByVal newValue As Boolean)
        End Set
    End Property

    'True if cell is user-editable 
    Overrides Property CellEditable(ByVal aRow As Integer, ByVal aColumn As Integer) As Boolean
        Get
            Return (aRow > 0)
        End Get
        Set(ByVal newValue As Boolean)
        End Set
    End Property

    'Number of rows 
    Overrides Property Rows() As Integer
        Get
            Return pTable.NumRecords + 1
        End Get
        Set(ByVal aNewRows As Integer)
        End Set
    End Property

    'Number of columns 
    Overrides Property Columns() As Integer
        Get
            Return pTable.NumFields
        End Get
        Set(ByVal aNewColumns As Integer)
        End Set
    End Property

    'Number of header rows that do not scroll 
    Overrides Property FixedRows() As Integer
        Get
            Return 1
        End Get
        Set(ByVal newValue As Integer)
        End Set
    End Property

    'Number of columns that do not scroll 
    Overrides Property FixedColumns() As Integer
        Get
            Return 0
        End Get
        Set(ByVal newValue As Integer)
        End Set
    End Property

    'Alignment of the contents of each cell 
    Overrides Property Alignment(ByVal aRow As Integer, ByVal aColumn As Integer) As atcAlignment
        Get
            If aRow = 0 Then
                Return atcAlignment.HAlignCenter
            Else
                Return atcAlignment.HAlignDecimal
            End If
        End Get
        Set(ByVal newValue As atcAlignment)
        End Set
    End Property

    'Expands Rows and/or Columns if needed to reach from 0,0 to aRow, aColumn
    Protected Overrides Sub ExpandRowsColumns(ByVal aRow As Integer, ByVal aColumn As Integer)
        'Expanding table this way is not supported
    End Sub

End Class