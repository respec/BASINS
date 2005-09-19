Public Class atcGrid
  Inherits System.Windows.Forms.UserControl

  Event MouseDownCell(ByVal aRow As Integer, ByVal aColumn As Integer)
  Event UserResizedColumn(ByVal aColumn As Integer, ByVal aWidth As Integer)

  Private WithEvents pSource As atcGridSource

  Private Const COL_TOLERANCE As Integer = 2

  Private pFont As Font = New Font("Microsoft Sans Serif", 8.25, FontStyle.Regular, GraphicsUnit.Point)

  Private pAllowHorizontalScrolling As Boolean = True
  Private pLineColor As Color
  Private pLineWidth As Single
  Private pCellBackColor As Color
  Private pRowHeight As ArrayList = New ArrayList   'of Single
  Private pColumnWidth As ArrayList = New ArrayList 'of Single

  Private pTopRow As Integer
  Private pLeftColumn As Integer

  Private pRowBottom As ArrayList = New ArrayList
  Private pColRight As ArrayList = New ArrayList

  Private pColumnDragging As Integer = -1

#Region " Windows Form Designer generated code "

  Public Sub New()
    MyBase.New()
    'This call is required by the Windows Form Designer.
    InitializeComponent()
    Me.SetStyle(ControlStyles.DoubleBuffer Or ControlStyles.UserPaint Or ControlStyles.AllPaintingInWmPaint, True)
  End Sub

  'UserControl overrides dispose to clean up the component list.
  Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
    If disposing Then
      If Not (components Is Nothing) Then
        components.Dispose()
      End If
    End If
    MyBase.Dispose(disposing)
  End Sub

  'Required by the Windows Form Designer
  Private components As System.ComponentModel.IContainer

  'NOTE: The following procedure is required by the Windows Form Designer
  'It can be modified using the Windows Form Designer.  
  'Do not modify it using the code editor.
  Friend WithEvents scrollCorner As System.Windows.Forms.Panel
  Friend WithEvents VScroller As System.Windows.Forms.VScrollBar
  Friend WithEvents HScroller As System.Windows.Forms.HScrollBar
  <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
    Me.VScroller = New System.Windows.Forms.VScrollBar
    Me.HScroller = New System.Windows.Forms.HScrollBar
    Me.scrollCorner = New System.Windows.Forms.Panel
    Me.SuspendLayout()
    '
    'VScroller
    '
    Me.VScroller.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.VScroller.Location = New System.Drawing.Point(144, 0)
    Me.VScroller.Name = "VScroller"
    Me.VScroller.Size = New System.Drawing.Size(16, 72)
    Me.VScroller.TabIndex = 1
    Me.VScroller.Visible = False
    '
    'HScroller
    '
    Me.HScroller.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
    Me.HScroller.Location = New System.Drawing.Point(0, 144)
    Me.HScroller.Name = "HScroller"
    Me.HScroller.Size = New System.Drawing.Size(88, 16)
    Me.HScroller.TabIndex = 2
    Me.HScroller.Visible = False
    '
    'scrollCorner
    '
    Me.scrollCorner.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.scrollCorner.Location = New System.Drawing.Point(144, 144)
    Me.scrollCorner.Name = "scrollCorner"
    Me.scrollCorner.Size = New System.Drawing.Size(16, 16)
    Me.scrollCorner.TabIndex = 3
    Me.scrollCorner.Visible = False
    '
    'atcGrid
    '
    Me.Controls.Add(Me.scrollCorner)
    Me.Controls.Add(Me.HScroller)
    Me.Controls.Add(Me.VScroller)
    Me.Name = "atcGrid"
    Me.Size = New System.Drawing.Size(160, 160)
    Me.ResumeLayout(False)

  End Sub

#End Region

  Public Sub Clear()
    Dim lRows As Integer
    Dim lColumns As Integer

    pCellBackColor = Color.White

    pLineColor = Color.FromKnownColor(KnownColor.ControlLight)
    pLineWidth = 1
    pLineWidth = 1

    pTopRow = 0
    pLeftColumn = 0

    pRowHeight = New ArrayList
    pColumnWidth = New ArrayList

    pRowBottom = New ArrayList
    pColRight = New ArrayList

    If pSource Is Nothing Then
      lRows = 0
      lColumns = 0
    Else
      lRows = pSource.Rows
      lColumns = pSource.Columns
    End If

    RowHeight(0) = 20
    If lColumns > 0 Then
      ColumnWidth(0) = Me.Width / lColumns
    Else
      ColumnWidth(0) = Me.Width / 2
    End If

    pColumnDragging = -1
  End Sub

  Public Property Source() As atcGridSource
    Get
      Return pSource
    End Get
    Set(ByVal newValue As atcGridSource)
      pSource = newValue
    End Set
  End Property

  Public Sub Initialize(ByVal aSource As atcGridSource)
    pSource = aSource
    Clear()
  End Sub

  Public Overrides Function ToString() As String
    Return pSource.ToString
  End Function

  Public Property ColumnWidth(ByVal iColumn) As Integer
    Get
      If pColumnWidth.Count = 0 Then
        Return 0 'should not happen
      ElseIf iColumn < pColumnWidth.Count Then 'Return last defined width
        Return pColumnWidth(iColumn)
      Else
        Return pColumnWidth(pColumnWidth.Count - 1)
      End If
    End Get
    Set(ByVal newValue As Integer)
      If iColumn < pColumnWidth.Count Then 'Change existing width of this column
        pColumnWidth(iColumn) = newValue
      Else 'Need to add one or more column widths to include this one
        For newColumn As Integer = pColumnWidth.Count To iColumn
          pColumnWidth.Add(newValue)
        Next
      End If
    End Set
  End Property

  Public Property RowHeight(ByVal iRow) As Single
    Get
      If pRowHeight.Count = 0 Then
        Return 0 'should not happen
      ElseIf iRow >= pRowHeight.Count Then 'Return last defined height
        Return pRowHeight(pRowHeight.Count - 1)
      Else
        Return pRowHeight(iRow)
      End If
    End Get
    Set(ByVal newValue As Single)
      If iRow < pRowHeight.Count Then 'Change existing width of this column
        pRowHeight(iRow) = newValue
      Else 'Need to add one or more row heights to include this one
        For newRow As Integer = pRowHeight.Count To iRow
          pRowHeight.Add(newValue)
        Next
      End If
    End Set
  End Property

  Public Property AllowHorizontalScrolling() As Boolean
    Get
      Return pAllowHorizontalScrolling
    End Get
    Set(ByVal newValue As Boolean)
      If newValue <> pAllowHorizontalScrolling Then
        pAllowHorizontalScrolling = newValue
        Me.Refresh()
      End If
    End Set
  End Property

  Public Property LineColor() As Color
    Get
      Return pLineColor
    End Get
    Set(ByVal newValue As Color)
      pLineColor = newValue
    End Set
  End Property

  Public Property LineWidth() As Single
    Get
      Return pLineWidth
    End Get
    Set(ByVal newValue As Single)
      pLineWidth = newValue
    End Set
  End Property

  Private Sub ATCgrid_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles MyBase.Paint
    Me.Render(e.Graphics)
  End Sub

  Private Sub Render(ByVal g As Graphics)
    If Me.Visible And Not pSource Is Nothing Then
      Dim x As Single = 0
      Dim y As Single = 0

      Dim lRows As Integer = pSource.Rows
      Dim lColumns As Integer = pSource.Columns

      Dim iRow As Integer
      Dim iColumn As Integer

      Dim visibleHeight As Single = Me.Height
      Dim visibleWidth As Single = Me.Width

      If pTopRow > 0 Then        'Scrolled down at least one row
        If lRows < pTopRow Then  'Scrolled past last row
          pTopRow = 0            'Reset scrollbar to top row
          Me.VScroller.Value = 0
        Else                     'Check to see if all rows could fit
          y = 0
          For iRow = 0 To lRows - 1
            y += RowHeight(iRow)
            If y > visibleHeight Then Exit For
          Next
          If y <= visibleHeight Then 'If all rows can fit
            pTopRow = 0              'Reset scrollbar to top row
            Me.VScroller.Value = 0
          End If
        End If
      End If
      If lColumns < pLeftColumn Then 'Scrolled past rightmost column
        Me.HScroller.Value = 0       'Reset scrollbar to leftmost column
      End If
      Dim lLinePen As New Pen(pLineColor, pLineWidth)
      Dim lOutsideBrush As New SolidBrush(pLineColor)
      Dim lCellBrush As New SolidBrush(pCellBackColor)

      Dim lCellValue As String
      Dim lCellAlignment As Integer
      Dim lCellValueSize As SizeF
      Dim lCellValueLeftSize As SizeF

      'Clear whole area to default cell background color
      g.FillRectangle(lCellBrush, 0, 0, visibleWidth, visibleHeight)

      'Draw Row Lines
      pRowBottom = New ArrayList
      If pTopRow = 0 Then VScroller.Visible = False
      y = 0
      For iRow = pTopRow To lRows - 1
        y += RowHeight(iRow)
        g.DrawLine(lLinePen, 0, y, visibleWidth, y)
        pRowBottom.Add(y)
        If y > visibleHeight Then
          visibleWidth -= VScroller.Width
          VScroller.Visible = True
          If iRow - pTopRow > 2 Then
            VScroller.LargeChange = iRow - pTopRow - 1
          Else
            VScroller.LargeChange = 1
          End If
          VScroller.Maximum = lRows '- VScroller.LargeChange + 2
          'Debug.WriteLine("Rows = " & lRows & ", TopRow = " & pTopRow & ", LargeChange = " & VScroller.LargeChange & ", Maximum = " & VScroller.Maximum)
          Exit For
        End If
      Next
      'Fill unused space below bottom line
      g.FillRectangle(lOutsideBrush, 0, y, visibleWidth, visibleHeight - y)

      'Draw Column Lines
      pColRight = New ArrayList
      If pLeftColumn = 0 Then
        HScroller.Visible = False
      ElseIf Not AllowHorizontalScrolling Then
        pLeftColumn = 0
      End If
      'If Not AllowHorizontalScrolling Then
      '  x = 0
      '  For iColumn = pLeftColumn To lColumns - 1
      '    x += ColumnWidth(iColumn)
      '  Next
      '  If x > visibleWidth Then
      '    For iColumn = pLeftColumn To lColumns - 1
      '      If ColumnWidth(iColumn) > -1 Then
      '      End If
      '      ColumnWidth(iColumn) *= visibleWidth / x
      '    Next
      '  End If
      'End If
      x = 0
      For iColumn = pLeftColumn To lColumns - 1
        x += ColumnWidth(iColumn)
        If iColumn = lColumns - 1 AndAlso Not AllowHorizontalScrolling AndAlso x <> visibleWidth Then
          ColumnWidth(iColumn) += visibleWidth - x
          x = visibleWidth
          'RaiseEvent UserResizedColumn(iColumn, ColumnWidth(iColumn))
        End If
        If x > visibleWidth Then
          If AllowHorizontalScrolling Then
            visibleHeight -= HScroller.Height
            If Not VScroller.Visible AndAlso y > visibleHeight Then
              'TODO: add VScroll because last line got hidden by HScroll
            End If
            HScroller.Visible = True
            If iColumn - pLeftColumn > 2 Then
              HScroller.LargeChange = iColumn - pLeftColumn - 1
            Else
              HScroller.LargeChange = 1
            End If
            HScroller.Maximum = lColumns '- HScroller.LargeChange + 1
            Exit For
          Else
            x = visibleWidth
          End If
        End If
        g.DrawLine(lLinePen, x, 0, x, visibleHeight)
        pColRight.Add(x)
      Next
      If AllowHorizontalScrolling Then
        'Fill unused space right of rightmost column
        g.FillRectangle(lOutsideBrush, x, 0, visibleWidth - x, visibleHeight)
      End If

      Dim lCellLeft As Single
      Dim lCellTop As Single = 0
      Dim lastRowDrawn As Integer = pTopRow + pRowBottom.Count - 1
      Dim lastColDrawn As Integer = pLeftColumn + pColRight.Count - 1

      For iRow = pTopRow To lastRowDrawn
        lCellLeft = 0
        For iColumn = pLeftColumn To lastColDrawn
          If ColumnWidth(iColumn) > 0 Then
            lCellValue = pSource.CellValue(iRow, iColumn)
            If Not lCellValue Is Nothing AndAlso lCellValue.Length > 0 Then
              lCellAlignment = pSource.Alignment(iRow, iColumn)
              lCellValueSize = g.MeasureString(lCellValue, pFont)
              While lCellValueSize.Width > ColumnWidth(iColumn)
                Select Case lCellValue.Length
                  Case 0 : Exit While
                  Case 1 : lCellValue = ""
                  Case Else
                    lCellValue = lCellValue.Substring(0, lCellValue.Length - 2) & "+"
                End Select
                lCellValueSize = g.MeasureString(lCellValue, pFont)
              End While

              Dim lTabPos As Integer = lCellValue.IndexOf(vbTab)
              Dim lMainValue As String
              If lTabPos >= 0 Then
                lMainValue = lCellValue.Substring(0, lTabPos)
              Else
                lMainValue = lCellValue
              End If

              Select Case lCellAlignment And atcAlignment.HAlign
                Case atcAlignment.HAlignLeft
                  x = lCellLeft
                Case atcAlignment.HAlignRight
                  x = pColRight(iColumn - pLeftColumn) - lCellValueSize.Width
                Case atcAlignment.HAlignCenter
                  lCellValueSize = g.MeasureString(lMainValue, pFont)
                  x = lCellLeft + (pColRight(iColumn - pLeftColumn) - lCellLeft - lCellValueSize.Width) / 2
                Case atcAlignment.HAlignDecimal
                  x = lCellLeft + (pColRight(iColumn - pLeftColumn) - lCellLeft) / 2 - WidthLeftOfDecimal(lMainValue, g)
                Case Else 'Default to left alignment 
                  x = lCellLeft
              End Select
              Select Case lCellAlignment And atcAlignment.VAlign
                Case atcAlignment.VAlignTop
                  y = lCellTop
                Case atcAlignment.VAlignBottom
                  y = pRowBottom(iRow - pTopRow) - lCellValueSize.Height
                Case Else 'atcAlignment.VAlignCenter 'Default to centering vertically 
                  y = lCellTop + (pRowBottom(iRow - pTopRow) - lCellTop - lCellValueSize.Height) / 2
              End Select
              Try
                If lTabPos >= 0 Then 'Right-align part of text after tab
                  g.DrawString(lMainValue, pFont, Brushes.Black, x, y)
                  x = pColRight(iColumn - pLeftColumn) - g.MeasureString(lCellValue.Substring(lTabPos + 1), pFont).Width
                  g.DrawString(lCellValue.Substring(lTabPos + 1), pFont, Brushes.Black, x, y)
                Else
                  g.DrawString(lCellValue, pFont, Brushes.Black, x, y) 'TODO: allow flexibility of brush
                End If
              Catch winErr As Exception
              End Try
            End If
            If iColumn < lColumns - 1 Then lCellLeft = pColRight(iColumn - pLeftColumn)
          End If
        Next
        If iRow < lRows - 1 Then lCellTop = pRowBottom(iRow - pTopRow) 'Top of next row is bottom of this one
      Next
    End If
  End Sub

  Private Function WidthLeftOfDecimal(ByVal aCellValue As String, ByVal g As Graphics) As Single
    Dim lCellValueSize As SizeF
    If IsNumeric(aCellValue) Then
      Dim lDecimalPos As Integer = aCellValue.IndexOf(".")
      If lDecimalPos = -1 Then
        lCellValueSize = g.MeasureString(aCellValue, pFont)
      Else
        lCellValueSize = g.MeasureString(aCellValue.Substring(0, lDecimalPos), pFont)
      End If
      Return lCellValueSize.Width
    Else 'Center non-numeric values
      Return g.MeasureString(aCellValue, pFont).Width / 2
    End If
  End Function

  Private Function WidthRightOfDecimal(ByVal aCellValue As String, ByVal g As Graphics) As Single
    If IsNumeric(aCellValue) Then
      Dim lDecimalPos As Integer = aCellValue.IndexOf(".")
      If lDecimalPos = -1 Then
        Return 0
      Else
        Return g.MeasureString(aCellValue.Substring(lDecimalPos + 1), pFont).Width
      End If
    Else 'Center non-numeric values
      Return g.MeasureString(aCellValue, pFont).Width / 2
    End If
  End Function

  Public Sub SizeAllColumnsToContents()
    Dim lMaxColumn = pSource.Columns - 1
    For lCol As Integer = 0 To lMaxColumn
      SizeColumnToContents(lCol)
    Next
  End Sub

  Public Sub SizeColumnToContents(ByVal aColumn As Integer)
    Dim lCellValue As String
    Dim lCellWidth As Single
    Dim lastRow As Integer = pSource.Rows - 1 'pTopRow + pRowBottom.Count - 1
    Dim g As Graphics = Me.CreateGraphics
    Dim lDecimalWidth As Single = g.MeasureString(".", pFont).Width
    Dim lMaxWidth As Integer = 0

    'TODO: would be faster to check just length of string [before/after decimal] then do width of "XXXXwidthXXXX"
    If lastRow > 150 Then lastRow = 100 'Limit how much time we spend finding the widest cell
    For iRow As Integer = pTopRow To lastRow
      lCellValue = pSource.CellValue(iRow, aColumn)
      If Not lCellValue Is Nothing AndAlso lCellValue.Length > 0 Then
        If (pSource.Alignment(iRow, aColumn) And atcAlignment.HAlign) = atcAlignment.HAlignDecimal Then
          lCellWidth = lDecimalWidth + 2 * Math.Max(WidthLeftOfDecimal(lCellValue, g), WidthRightOfDecimal(lCellValue, g))
        Else
          lCellWidth = g.MeasureString(lCellValue, pFont).Width
        End If
        If lCellWidth > lMaxWidth Then
          lMaxWidth = lCellWidth
        End If
      End If
    Next
    ColumnWidth(aColumn) = lMaxWidth + COL_TOLERANCE * 2
    g.Dispose()
  End Sub

  Private Sub pSource_ChangedColumns(ByVal aColumns As Integer) Handles pSource.ChangedColumns
    Me.Refresh()
  End Sub

  Private Sub pSource_ChangedRows(ByVal aRows As Integer) Handles pSource.ChangedRows
    Me.Refresh()
  End Sub

  Private Sub pSource_ChangedValue(ByVal aRow As Integer, ByVal aColumn As Integer) Handles pSource.ChangedValue
    Me.Refresh()
  End Sub

  Protected Overrides Sub OnMouseDown(ByVal e As System.Windows.Forms.MouseEventArgs)
    Dim lRow As Integer
    Dim lColumn As Integer = ColumnEdgeToDrag(e.X)
    If lColumn >= 0 Then
      pColumnDragging = lColumn
    Else
      lRow = 0
      lColumn = 0
      While lRow < pRowBottom.Count AndAlso e.Y > pRowBottom(lRow)
        lRow += 1
      End While
      While lColumn < pColRight.Count AndAlso e.X > pColRight(lColumn)
        lColumn += 1
      End While
      If lRow + pTopRow < pSource.Rows And lColumn + pLeftColumn < pSource.Columns Then
        RaiseEvent MouseDownCell(lRow + pTopRow, lColumn + pLeftColumn)
      End If
    End If
  End Sub

  Protected Overrides Sub OnMouseMove(ByVal e As System.Windows.Forms.MouseEventArgs)
    Dim newCursor As Windows.Forms.Cursor = Cursors.Default
    Select Case e.Button
      Case MouseButtons.None
        If ColumnEdgeToDrag(e.X) >= 0 Then
          newCursor = Cursors.SizeWE
        ElseIf ColumnDecimalToDrag(e.X, e.Y) >= 0 Then
          newCursor = Cursors.SizeWE
        End If
        If Not Me.Cursor Is newCursor Then Me.Cursor = newCursor
      Case MouseButtons.Left
        If pColumnDragging >= 0 Then
          ColumnWidth(pColumnDragging) += (e.X - pColRight(pColumnDragging - pLeftColumn))
          If ColumnWidth(pColumnDragging) < COL_TOLERANCE * 2 Then 'it got too small
            ColumnWidth(pColumnDragging) = COL_TOLERANCE * 2       'enforce small minimun size
          End If
          RaiseEvent UserResizedColumn(pColumnDragging, ColumnWidth(pColumnDragging))
          Refresh()
        End If
    End Select
  End Sub

  Protected Overrides Sub OnDoubleClick(ByVal e As System.EventArgs)
    If pColumnDragging >= 0 Then
      Dim lOldWidth As Integer = ColumnWidth(pColumnDragging)
      SizeColumnToContents(pColumnDragging)
      If ColumnWidth(pColumnDragging) <> lOldWidth Then
        Refresh()
        RaiseEvent UserResizedColumn(pColumnDragging, ColumnWidth(pColumnDragging))
      End If
    End If
  End Sub

  Protected Overrides Sub OnMouseUp(ByVal e As System.Windows.Forms.MouseEventArgs)
    pColumnDragging = -1
  End Sub

  Private Function ColumnEdgeToDrag(ByVal X As Integer) As Integer
    Dim lColumn As Integer = 0
    While lColumn < pColRight.Count
      'If within tolerance of column edge and column is not being hidden by a zero width
      If Math.Abs(X - pColRight(lColumn)) <= COL_TOLERANCE AndAlso ColumnWidth(lColumn + pLeftColumn) > 0 Then
        'If we are not allowing horizontal scrolling, last column is not resizable
        If AllowHorizontalScrolling OrElse lColumn < pColRight.Count - 1 Then
          Return lColumn + pLeftColumn
        End If
      End If
      lColumn += 1
    End While
    Return -1
  End Function

  Private Function ColumnDecimalToDrag(ByVal X As Integer, ByVal Y As Integer) As Integer
    Dim lRow As Integer = 0
    Dim lColumn As Integer = pLeftColumn
    Dim lColLeft As Integer = 0
    While lRow < pRowBottom.Count AndAlso Y > pRowBottom(lRow)
      lRow += 1
    End While
    While lColumn < pColRight.Count AndAlso X > pColRight(lColumn)
      lColLeft = pColRight(lColumn)
      lColumn += 1
    End While

    If lColumn < pColRight.Count AndAlso pSource.Alignment(lRow, lColumn) = atcAlignment.HAlignDecimal Then
      'If within tolerance of column edge and column is not being hidden by a zero width
      If Math.Abs(X - (pColRight(lColumn) + lColLeft) / 2) <= COL_TOLERANCE AndAlso ColumnWidth(lColumn + pLeftColumn) > 0 Then
        Return lColumn + pLeftColumn
      End If
    End If
    Return -1
  End Function

  Private Sub atcGrid_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Resize
    If HScroller.Visible Then
      VScroller.Height = Me.Height - HScroller.Height
    Else
      VScroller.Height = Me.Height
    End If

    If VScroller.Visible Then
      HScroller.Width = Me.Width - VScroller.Width
    Else
      HScroller.Width = Me.Width
    End If

    scrollCorner.Visible = VScroller.Visible And HScroller.Visible

    Me.Refresh()
  End Sub

  Private Sub VScroll_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles VScroller.ValueChanged
    pTopRow = VScroller.Value
    Refresh()
  End Sub

  Private Sub HScroll_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HScroller.ValueChanged
    pLeftColumn = HScroller.Value
    Refresh()
  End Sub

End Class
