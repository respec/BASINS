Imports MapWinUtility.Strings

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
                ReDim pValues(pRows - 1, pColumns - 1)
            End If
            ExpandRowsColumns(aRow, aColumn)
            If InvalidRowOrColumn(aRow, aColumn) Then
                'LogDbg("Could not set CellValue(" & aRow & ", " & aColumn & ") to " & newValue)
            ElseIf pValues(aRow, aColumn) <> newValue Then
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
                ReDim pColors(pRows - 1, pColumns - 1)
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
                ReDim pSelected(pRows - 1, pColumns - 1)
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
                    ReDim pEditable(pRows - 1, pColumns - 1)
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
                    lastRowCopied = aNewRows - 1
                Else
                    lastRowCopied = pRows - 1
                End If

                Dim newValues(aNewRows - 1, pColumns - 1) As String
                Dim newColors(,) As Color = Nothing
                Dim newSelected(,) As Boolean = Nothing
                Dim newEditable(,) As Boolean = Nothing
                Dim newAlignment(,) As atcAlignment = Nothing

                If Not pColors Is Nothing Then ReDim newColors(aNewRows - 1, pColumns - 1)
                If Not pSelected Is Nothing Then ReDim newSelected(aNewRows - 1, pColumns - 1)
                If Not pEditable Is Nothing Then ReDim newEditable(aNewRows - 1, pColumns - 1)
                If Not pAlignment Is Nothing Then ReDim newAlignment(aNewRows - 1, pColumns - 1)

                For iRow As Integer = 0 To lastRowCopied
                    For iColumn As Integer = 0 To pColumns - 1
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
        Set(ByVal aNewColumns As Integer)
            If aNewColumns <> pColumns Then
                pColumns = aNewColumns
                ReDim Preserve pValues(pRows - 1, pColumns - 1)
                If Not pColors Is Nothing Then ReDim Preserve pColors(pRows - 1, pColumns - 1)
                If Not pSelected Is Nothing Then ReDim Preserve pSelected(pRows - 1, pColumns - 1)
                If Not pEditable Is Nothing Then ReDim Preserve pEditable(pRows - 1, pColumns - 1)
                If Not pAlignment Is Nothing Then ReDim Preserve pAlignment(pRows - 1, pColumns - 1)
            End If
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
                ReDim pAlignment(pRows - 1, pColumns - 1)
            End If
            ExpandRowsColumns(aRow, aColumn)
            pAlignment(aRow, aColumn) = newValue
        End Set
    End Property

    'True if aRow or aColumn does not exist
    Protected Overridable Function InvalidRowOrColumn(ByVal aRow As Integer, ByVal aColumn As Integer) As Boolean
        Return (aRow >= Rows _
         OrElse aColumn >= Columns _
         OrElse aRow < 0 _
         OrElse aColumn < 0)
    End Function

    'Expands Rows and/or Columns if needed to reach from 0,0 to aRow, aColumn
    Protected Overridable Sub ExpandRowsColumns(ByVal aRow As Integer, ByVal aColumn As Integer)
        If aRow >= Rows Then Rows = aRow + 1
        If aColumn >= Columns Then Columns = aColumn + 1
    End Sub

    Public Overridable Sub FromString(ByVal aString As String)
        Dim lString As String = aString
        Dim curLine As String
        Dim curRow As Integer = -1
        Dim curCol As Integer
        Dim maxCol As Integer = 0
        While lString.Length > 0
            curRow += 1
            curCol = -1
            curLine = StrSplit(lString, vbCrLf, "")
            While curLine.Length > 0
                curCol += 1
                StrSplit(curLine, vbTab, "")
            End While
            If curCol > maxCol Then maxCol = curCol
        End While

        Me.Rows = curRow + 1
        Me.Columns = maxCol + 1
        lString = aString
        curRow = -1
        While lString.Length > 0
            curRow += 1
            curCol = -1
            curLine = StrSplit(lString, vbCrLf, "")
            While curLine.Length > 0
                curCol += 1
                CellValue(curRow, curCol) = StrSplit(curLine, vbTab, "")
            End While
            If curCol > maxCol Then maxCol = curCol
        End While
    End Sub

    Public Overrides Function ToString() As String
        Try
            Dim lCellValue As String
            Dim lAddTabs() As Boolean
            ReDim lAddTabs(Me.Columns - 1)
            Dim lMaxCol As Integer = Columns - 1
            Dim lMaxRow As Integer = Rows - 1
            Dim lCheckRowsForTab As Integer = Math.Min(lMaxRow, 100) 'Check at most this many rows for tabs
            MapWinUtility.Logger.Status("Reading " & Format(lMaxRow, "#,###") & " Rows")
            Dim lBuilder As New System.Text.StringBuilder(lMaxCol * lMaxRow * 5)
            Dim lRow As Integer
            Dim lCol As Integer
            For lCol = 0 To lMaxCol
                For lRow = 0 To lCheckRowsForTab
                    lCellValue = CellValue(lRow, lCol)
                    If lCellValue IsNot Nothing AndAlso lCellValue.Contains(vbTab) Then
                        lAddTabs(lCol) = True
                        Exit For
                    End If
                Next
            Next

            For lRow = 0 To lMaxRow
                For lCol = 0 To lMaxCol
                    lCellValue = CellValue(lRow, lCol)
                    If lCellValue Is Nothing Then lCellValue = ""
                    lBuilder.Append(lCellValue)
                    'Some modified values contain "<tab>(+10%)", add a tab to those that don't
                    If lAddTabs(lCol) AndAlso lCellValue.IndexOf(vbTab) < 0 Then lBuilder.Append(vbTab)
                    If lCol < lMaxCol Then lBuilder.Append(vbTab) 'Add tab afer each column except for last 
                Next
                lBuilder.Append(vbCrLf)
                MapWinUtility.Logger.Progress(lRow, lMaxRow)
            Next
            MapWinUtility.Logger.Status("")
            Return lBuilder.ToString
        Catch e As Exception
            MapWinUtility.Logger.Progress("", 0, 0)
            Return e.Message & vbCrLf & e.StackTrace
        End Try
    End Function

    Public Overridable Function AppendColumns(ByVal aRightColumns As atcGridSource) As atcGridSource
        Dim lNewSource As New atcGridSource
        With lNewSource
            .FixedRows = Math.Max(FixedRows, aRightColumns.FixedRows)
            .Rows = Math.Max(Rows, aRightColumns.Rows)
            .Columns = Columns + aRightColumns.Columns
            .ColorCells = ColorCells Or aRightColumns.ColorCells

            Dim lMaxCol As Integer = .Columns - 1
            Dim lMaxRow As Integer = .Rows - 1

            For iCol As Integer = 0 To lMaxCol
                For iRow As Integer = 0 To lMaxRow
                    If iCol < pColumns Then
                        CopyCell(Me, iRow, iCol, lNewSource, iRow, iCol)
                    Else
                        CopyCell(aRightColumns, iRow, iCol - pColumns, lNewSource, iRow, iCol)
                    End If
                Next
            Next
        End With
        Return lNewSource
    End Function

    Private Shared Sub CopyCell(ByVal aFromSource As atcGridSource, ByVal aFromRow As Integer, ByVal aFromColumn As Integer, _
                                ByVal aToSource As atcGridSource, ByVal aToRow As Integer, ByVal aToColumn As Integer)
        If Not aFromSource.InvalidRowOrColumn(aFromRow, aFromColumn) Then
            With aToSource
                .Alignment(aToRow, aToColumn) = aFromSource.Alignment(aFromRow, aFromColumn)
                .CellEditable(aToRow, aToColumn) = aFromSource.CellEditable(aFromRow, aFromColumn)
                .CellValue(aToRow, aToColumn) = aFromSource.CellValue(aFromRow, aFromColumn)
                If .ColorCells AndAlso aFromSource.ColorCells Then
                    .CellColor(aToRow, aToColumn) = aFromSource.CellColor(aFromRow, aFromColumn)
                End If
            End With
        End If
    End Sub
End Class
