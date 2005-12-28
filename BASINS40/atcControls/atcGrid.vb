Imports atcUtility

Public Class atcGrid
  Inherits System.Windows.Forms.UserControl

  Event CellEdited(ByVal aGrid As atcGrid, ByVal aRow As Integer, ByVal aColumn As Integer)
  Event MouseDownCell(ByVal aGrid As atcGrid, ByVal aRow As Integer, ByVal aColumn As Integer)
  Event UserResizedColumn(ByVal aGrid As atcGrid, ByVal aColumn As Integer, ByVal aWidth As Integer)

  Private WithEvents pSource As atcGridSource

  Private Const DRAG_TOLERANCE As Integer = 2

  Private pFont As Font = New Font("Microsoft San Serif", 8.25, FontStyle.Regular, GraphicsUnit.Point) ' MyBase.Font

  Private pAllowHorizontalScrolling As Boolean = True
  Private pLineColor As Color
  Private pLineWidth As Single
  Private pCellBackColor As Color
  Private pCellTextColor As Color
  Private pRowHeight As ArrayList = New ArrayList   'of Integer
  Private pColumnWidth As ArrayList = New ArrayList 'of Integer

  Private pTopRow As Integer
  Private pLeftColumn As Integer

  Private pRowBottom As atcCollection = New atcCollection
  Private pColumnRight As atcCollection = New atcCollection

  Private pColumnDragging As Integer = -1

  Private pColumnEditing As Integer = -1
  Private pRowEditing As Integer = -1

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
  Friend WithEvents CellEditBox As System.Windows.Forms.TextBox
  <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
    Me.VScroller = New System.Windows.Forms.VScrollBar
    Me.HScroller = New System.Windows.Forms.HScrollBar
    Me.scrollCorner = New System.Windows.Forms.Panel
    Me.CellEditBox = New System.Windows.Forms.TextBox
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
    'CellEditBox
    '
    Me.CellEditBox.Location = New System.Drawing.Point(32, 96)
    Me.CellEditBox.Name = "CellEditBox"
    Me.CellEditBox.Size = New System.Drawing.Size(104, 20)
    Me.CellEditBox.TabIndex = 4
    Me.CellEditBox.Text = ""
    Me.CellEditBox.Visible = False
    '
    'atcGrid
    '
    Me.Controls.Add(Me.CellEditBox)
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

    pCellBackColor = SystemColors.Window
    pCellTextColor = SystemColors.WindowText

    pLineColor = SystemColors.Control
    pLineWidth = 1
    pLineWidth = 1

    pTopRow = 0
    pLeftColumn = 0

    pRowHeight = New ArrayList
    pColumnWidth = New ArrayList

    pRowBottom = New atcCollection
    pColumnRight = New atcCollection

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
      ColumnWidth(0) = Me.Width
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

  Public Property ColumnWidth(ByVal aColumn As Integer) As Integer
    Get
      If pColumnWidth.Count = 0 Then
        atcUtility.LogDbg("atcGrid:ColumnWidth:Get: Unexpected pColumnWidth.Count = 0")
        Return 0
      ElseIf aColumn < pColumnWidth.Count Then 'Return last defined width
        Return pColumnWidth(aColumn)
      Else
        Return pColumnWidth(pColumnWidth.Count - 1)
      End If
    End Get
    Set(ByVal newValue As Integer)
      If aColumn < pColumnWidth.Count Then 'Change existing width of this column
        If aColumn = pColumnWidth.Count - 1 AndAlso _
              Not pSource Is Nothing AndAlso _
              pSource.Columns > pColumnWidth.Count Then 'Preserve implied width of later columns
          pColumnWidth.Add(pColumnWidth(aColumn))
        End If
        pColumnWidth(aColumn) = newValue
      Else 'Need to add one or more column widths to include this one

        If aColumn > pColumnWidth.Count Then 'Preserve implied width of earlier columns
          Dim lOldWidth As Integer
          If pColumnWidth.Count > 0 Then
            lOldWidth = pColumnWidth(pColumnWidth.Count - 1)
          Else
            lOldWidth = newValue
          End If
          For newColumn As Integer = pColumnWidth.Count To aColumn - 1
            pColumnWidth.Add(lOldWidth)
          Next
        End If

        pColumnWidth.Add(newValue)
      End If
    End Set
  End Property

  Public Property CellBackColor() As Color
    Get
      Return pCellBackColor
    End Get
    Set(ByVal newValue As Color)
      pCellBackColor = newValue
    End Set
  End Property

  Public Property RowHeight(ByVal aRow As Integer) As Integer
    Get
      If pRowHeight.Count = 0 Then
        Return 0 'should not happen
      ElseIf aRow >= pRowHeight.Count Then 'Return last defined height
        Return pRowHeight(pRowHeight.Count - 1)
      Else
        Return pRowHeight(aRow)
      End If
    End Get
    Set(ByVal newValue As Integer)
      If aRow < pRowHeight.Count Then 'Change existing width of this column
        pRowHeight(aRow) = newValue
      Else 'Need to add one or more row heights to include this one
        For newRow As Integer = pRowHeight.Count To aRow
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

  Public Overrides Property Font() As Font
    Get
      Return pFont
    End Get
    Set(ByVal newValue As Font)
      If Not newValue Is Nothing Then
        pFont = newValue
        MyBase.Font = pFont
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
      Dim x As Integer = 0
      Dim y As Integer = 0

      Dim lRows As Integer = pSource.Rows
      Dim lFixedRows As Integer = pSource.FixedRows

      Dim lColumns As Integer = pSource.Columns
      Dim lFixedColumns As Integer = pSource.FixedColumns
      Dim lColumnWidth As Integer

      Dim lRow As Integer
      Dim lColumn As Integer

      Dim lRowIndex As Integer
      Dim lColumnIndex As Integer

      Dim visibleHeight As Integer = Me.Height
      Dim visibleWidth As Integer = Me.Width

      If pTopRow > 0 Then        'Scrolled down at least one row
        If lRows < pTopRow Then  'Scrolled past last row
          pTopRow = 0            'Reset scrollbar to top row
          Me.VScroller.Value = 0
        Else                     'Check to see if all rows could fit
          y = 0
          For lRow = 0 To lRows - 1
            y += RowHeight(lRow)
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

      Dim lEachCellBackColor As Color = pCellBackColor
      Dim lEachCellBackBrush As New SolidBrush(lEachCellBackColor)
      Dim lEachCellTextBrush As New SolidBrush(SystemColors.WindowText)

      Dim lColorCells As Boolean = pSource.ColorCells

      Dim lCellValue As String
      Dim lCellAlignment As Integer
      Dim lCellValueSize As SizeF
      Dim lCellValueLeftSize As SizeF

      'Clear whole area to default cell background color
      g.FillRectangle(lCellBrush, 0, 0, visibleWidth, visibleHeight)

      'Draw Row Lines
      pRowBottom = New atcCollection
      If pTopRow = 0 Then VScroller.Visible = False
      y = -1

      For lRow = 0 To lRows - 1
        If lRow < lFixedRows OrElse lRow >= pTopRow Then
          y += RowHeight(lRow)
          g.DrawLine(lLinePen, 0, y, visibleWidth, y)
          pRowBottom.Add(lRow, y)
          If y > visibleHeight Then
            visibleWidth -= VScroller.Width
            VScroller.Visible = True
            If lRow - pTopRow > 2 Then
              VScroller.LargeChange = lRow - pTopRow - 1
            Else
              VScroller.LargeChange = 1
            End If
            VScroller.Maximum = lRows '- VScroller.LargeChange + 2
            'Debug.WriteLine("Rows = " & lRows & ", TopRow = " & pTopRow & ", LargeChange = " & VScroller.LargeChange & ", Maximum = " & VScroller.Maximum)
            Exit For
          End If
        ElseIf lRow < pTopRow - 1 Then 'skip rows we can't see
          lRow = pTopRow - 1
        End If
      Next

      'Fill unused space below bottom line
      If y < visibleHeight Then
        g.FillRectangle(lOutsideBrush, 0, y, visibleWidth, visibleHeight - y)
      End If

      'Draw Column Lines
      pColumnRight = New atcCollection
      If pLeftColumn = 0 Then
        HScroller.Visible = False
      ElseIf Not AllowHorizontalScrolling Then
        pLeftColumn = 0
      End If

      x = -1
      For lColumn = 0 To lColumns - 1
        If lColumn < lFixedColumns OrElse lColumn >= pLeftColumn Then
          lColumnWidth = ColumnWidth(lColumn)
          If lColumnWidth > 0 Then
            x += lColumnWidth
            If Not AllowHorizontalScrolling AndAlso x < visibleWidth Then
              'See if this is the last non-hidden column to expand to fit the available width
              Dim lScanHiddenColumns As Integer = lColumn + 1
              While lScanHiddenColumns < lColumns AndAlso ColumnWidth(lScanHiddenColumns) = 0
                lScanHiddenColumns += 1
              End While
              If lScanHiddenColumns = lColumns Then 'Any columns right of this one are hidden
                ColumnWidth(lColumn) += visibleWidth - x 'Expand this one
                x = visibleWidth
              End If
            End If
            If x > visibleWidth Then
              If AllowHorizontalScrolling Then
                visibleHeight -= HScroller.Height
                If Not VScroller.Visible AndAlso y > visibleHeight Then
                  VScroller.Visible = True
                End If
                HScroller.Visible = True
                If lColumn - pLeftColumn > 2 Then
                  HScroller.LargeChange = lColumn - pLeftColumn - 1
                Else
                  HScroller.LargeChange = 1
                End If
                HScroller.Maximum = lColumns
                pColumnRight.Add(lColumn, x)
                Exit For
              Else
                x = visibleWidth
              End If
            End If
            g.DrawLine(lLinePen, x, 0, x, visibleHeight)
            pColumnRight.Add(lColumn, x)
          End If
        ElseIf lColumn < pLeftColumn - 1 Then
          lColumn = pLeftColumn - 1
        End If
      Next

      SizeScrollers()

      'Fill unused space right of rightmost column
      If x < visibleWidth Then
        g.FillRectangle(lOutsideBrush, x, 0, visibleWidth - x, visibleHeight)
      End If

      Dim lCellLeft As Integer
      Dim lCellRight As Integer
      Dim lCellTop As Integer = 0
      Dim lCellBottom As Integer = -1

      For lRowIndex = 0 To pRowBottom.Count - 1
        lRow = pRowBottom.Keys(lRowIndex)
        lCellLeft = 0
        lCellTop = lCellBottom + 1 'Top of next row is bottom of this one
        lCellBottom = pRowBottom.ItemByIndex(lRowIndex)
        For lColumnIndex = 0 To pColumnRight.Count - 1
          lColumn = pColumnRight.Keys(lColumnIndex)
          If ColumnWidth(lColumn) > 0 Then
            lCellRight = pColumnRight.ItemByIndex(lColumnIndex)
            If pSource.CellSelected(lRow, lColumn) Then
              lEachCellBackColor = SystemColors.Highlight
              lEachCellTextBrush.Color = SystemColors.HighlightText
            ElseIf lColorCells Then
              lEachCellBackColor = pSource.CellColor(lRow, lColumn)
              lEachCellTextBrush.Color = SystemColors.WindowText
            Else
              lEachCellBackColor = pCellBackColor
              lEachCellTextBrush.Color = SystemColors.WindowText
            End If
            If Not lEachCellBackColor.Equals(pCellBackColor) Then
              lEachCellBackBrush.Color = lEachCellBackColor
              g.FillRectangle(lEachCellBackBrush, lCellLeft, lCellTop, lCellRight - lCellLeft, lCellBottom - lCellTop)
              g.DrawRectangle(lLinePen, lCellLeft - 1, lCellTop - 1, lCellRight - lCellLeft + 1, lCellBottom - lCellTop + 1)
            End If
            lCellValue = pSource.CellValue(lRow, lColumn)
            If Not lCellValue Is Nothing AndAlso lCellValue.Length > 0 Then
              lCellAlignment = pSource.Alignment(lRow, lColumn)
              lCellValueSize = g.MeasureString(lCellValue, pFont)
              While lCellValueSize.Width > ColumnWidth(lColumn)
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
                  x = lCellRight - lCellValueSize.Width
                Case atcAlignment.HAlignCenter
                  lCellValueSize = g.MeasureString(lMainValue, pFont)
                  x = lCellLeft + (lCellRight - lCellLeft - lCellValueSize.Width) / 2
                Case atcAlignment.HAlignDecimal
                  x = lCellLeft + (lCellRight - lCellLeft) / 2 - WidthLeftOfDecimal(lMainValue, g)
                Case Else 'Default to left alignment 
                  x = lCellLeft
              End Select
              Select Case lCellAlignment And atcAlignment.VAlign
                Case atcAlignment.VAlignTop
                  y = lCellTop
                Case atcAlignment.VAlignBottom
                  y = lCellBottom - lCellValueSize.Height
                Case Else 'atcAlignment.VAlignCenter 'Default to centering vertically 
                  y = lCellTop + (lCellBottom - lCellTop - lCellValueSize.Height) / 2
              End Select
              Try
                If lTabPos >= 0 Then 'Right-align part of text after tab
                  g.DrawString(lMainValue, pFont, lEachCellTextBrush, x, y)
                  x = lCellRight - g.MeasureString(lCellValue.Substring(lTabPos + 1), pFont).Width
                  g.DrawString(lCellValue.Substring(lTabPos + 1), pFont, lEachCellTextBrush, x, y)
                Else
                  g.DrawString(lCellValue, pFont, lEachCellTextBrush, x, y) 'TODO: allow flexibility of brush
                End If
              Catch winErr As Exception
              End Try
            End If
            lCellLeft = lCellRight + 1 'Left of next column is right of this one
          End If
        Next
      Next
    End If
  End Sub

  Private Function WidthLeftOfDecimal(ByVal aCellValue As String, ByVal g As Graphics) As Integer
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

  Private Function WidthRightOfDecimal(ByVal aCellValue As String, ByVal g As Graphics) As Integer
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
    Dim lMaxColumn As Integer = pSource.Columns - 1
    For lCol As Integer = 0 To lMaxColumn
      SizeColumnToContents(lCol)
    Next
  End Sub

  Public Sub SizeColumnToContents(ByVal aColumn As Integer)
    Dim lCellValue As String
    Dim lCellWidth As Integer
    Dim lastRow As Integer = pSource.Rows - 1
    Dim g As Graphics = Me.CreateGraphics
    Dim lDecimalWidth As Integer = g.MeasureString(".", pFont).Width
    Dim lMaxWidth As Integer = 0

    'TODO: would be faster to check just length of string [before/after decimal] then do width of "XXXXwidthXXXX"
    If lastRow > pTopRow + 150 Then lastRow = pTopRow + 100 'Limit how much time we spend finding the widest cell
    For lRow As Integer = 0 To lastRow
      lCellValue = pSource.CellValue(lRow, aColumn)
      If Not lCellValue Is Nothing AndAlso lCellValue.Length > 0 Then
        If (pSource.Alignment(lRow, aColumn) And atcAlignment.HAlign) = atcAlignment.HAlignDecimal Then
          lCellWidth = lDecimalWidth + 2 * Math.Max(WidthLeftOfDecimal(lCellValue, g), WidthRightOfDecimal(lCellValue, g))
        Else
          lCellWidth = g.MeasureString(lCellValue, pFont).Width
        End If
        If lCellWidth > lMaxWidth Then
          lMaxWidth = lCellWidth
        End If
      End If
    Next
    ColumnWidth(aColumn) = lMaxWidth + DRAG_TOLERANCE * 2
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

  Private Function CellBounds(ByVal aRow As Integer, ByVal aColumn As Integer) As Rectangle
    Dim lX As Integer = 0
    Dim lY As Integer = 0
    Dim lFixedRows As Integer = pSource.FixedRows
    Dim lFixedColumns As Integer = pSource.FixedColumns

    For lRow As Integer = 0 To aRow - 1
      If lRow < lFixedRows OrElse lRow >= pTopRow Then
        lY += RowHeight(lRow)
      ElseIf lRow < pTopRow - 1 Then 'skip rows we can't see
        lRow = pTopRow - 1
      End If
    Next
    For lColumn As Integer = pLeftColumn To aColumn - 1
      If lColumn < lFixedColumns OrElse lColumn >= pLeftColumn Then
        lX += ColumnWidth(lColumn)
      Else
        lColumn = pLeftColumn - 1
      End If
    Next
    Return New Rectangle(lX, lY, ColumnWidth(aColumn), RowHeight(aRow))
  End Function

  Public Sub EditCell(ByVal aRow As Integer, ByVal aColumn As Integer, Optional ByVal aOverrideEditable As Boolean = False)
    EditCellFinished()
    If aOverrideEditable OrElse pSource.CellEditable(aRow, aColumn) Then
      Dim EditCellBounds As Rectangle = CellBounds(aRow, aColumn)
      pColumnEditing = aColumn
      pRowEditing = aRow
      CellEditBox.Font = pFont
      CellEditBox.Text = pSource.CellValue(aRow, aColumn)
      CellEditBox.BackColor = pSource.CellColor(aRow, aColumn)
      CellEditBox.SetBounds(EditCellBounds.Left, EditCellBounds.Top, EditCellBounds.Width, EditCellBounds.Height)
      CellEditBox.Visible = True
      CellEditBox.Focus()
    End If
  End Sub

  Public Sub EditCellFinished()
    If CellEditBox.Visible Then ChangeEditingValues(CellEditBox.Text)
    CellEditBox.Visible = False
  End Sub

  Private Sub ChangeEditingValues(ByVal aNewValue As String)
    If aNewValue <> pSource.CellValue(pRowEditing, pColumnEditing) Then
      pSource.CellValue(pRowEditing, pColumnEditing) = aNewValue
      RaiseEvent CellEdited(Me, pRowEditing, pColumnEditing)
    End If
  End Sub

  Protected Overrides Sub OnMouseDown(ByVal e As System.Windows.Forms.MouseEventArgs)
    Dim lLastRowDisplayed As Integer = pRowBottom.Keys.Item(pRowBottom.Count - 1)
    Dim lRowIndex As Integer = 0
    Dim lRow As Integer = 0
    Dim lColumn As Integer = ColumnEdgeToDrag(e.X)
    Dim lColumnIndex As Integer = 0
    If lColumn >= 0 Then
      pColumnDragging = lColumn
    Else
      While lRowIndex < pRowBottom.Count AndAlso e.Y > pRowBottom.ItemByIndex(lRowIndex)
        lRowIndex += 1
      End While

      While lColumnIndex < pColumnRight.Count AndAlso e.X > pColumnRight.ItemByIndex(lColumnIndex)
        lColumnIndex += 1
      End While

      If lRowIndex < pRowBottom.Count AndAlso lColumnIndex < pColumnRight.Count Then
        lRow = pRowBottom.Keys(lRowIndex)
        lColumn = pColumnRight.Keys(lColumnIndex)
        If pSource.CellEditable(lRow, lColumn) Then
          EditCell(lRow, lColumn)
        End If
        RaiseEvent MouseDownCell(Me, lRow, lColumn)
      End If
    End If
  End Sub

  Protected Overrides Sub OnMouseMove(ByVal e As System.Windows.Forms.MouseEventArgs)
    Dim newCursor As Windows.Forms.Cursor = Cursors.Default
    Select Case e.Button
      Case Windows.Forms.MouseButtons.None
        If ColumnEdgeToDrag(e.X) >= 0 Then
          newCursor = Cursors.SizeWE
        ElseIf ColumnDecimalToDrag(e.X, e.Y) >= 0 Then
          newCursor = Cursors.SizeWE
        End If
        If Not Me.Cursor Is newCursor Then Me.Cursor = newCursor
      Case Windows.Forms.MouseButtons.Left
        If pColumnDragging >= 0 Then
          ColumnWidth(pColumnDragging) += (e.X - pColumnRight.ItemByKey(pColumnDragging))
          If ColumnWidth(pColumnDragging) < DRAG_TOLERANCE * 2 Then 'it got too small
            ColumnWidth(pColumnDragging) = DRAG_TOLERANCE * 2       'enforce small minimun size
          End If
          RaiseEvent UserResizedColumn(Me, pColumnDragging, ColumnWidth(pColumnDragging))
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
        RaiseEvent UserResizedColumn(Me, pColumnDragging, ColumnWidth(pColumnDragging))
      End If
    End If
  End Sub

  Protected Overrides Sub OnMouseUp(ByVal e As System.Windows.Forms.MouseEventArgs)
    pColumnDragging = -1
  End Sub

  Private Function ColumnEdgeToDrag(ByVal X As Integer) As Integer
    Dim lColumn As Integer = 0
    Dim lColumnIndex As Integer = 0
    While lColumnIndex < pColumnRight.Count
      'If within tolerance of column edge
      If Math.Abs(X - pColumnRight.ItemByIndex(lColumnIndex)) <= DRAG_TOLERANCE Then
        lColumn = pColumnRight.Keys(lColumnIndex)
        If ColumnWidth(lColumn) <= 0 Then
          'ColumnWidth of zero means we are not displaying this column, so it is not resizable
        ElseIf Not AllowHorizontalScrolling AndAlso Not pSource Is Nothing AndAlso lColumn >= pSource.Columns - 1 Then
          'If we are not allowing horizontal scrolling, last column is not resizable
        Else
          Return lColumn
        End If
      End If
      lColumnIndex += 1
    End While
    Return -1
  End Function

  'This routine works, but is not in use until we decide to allow dragging the decimal position
  Private Function ColumnDecimalToDrag(ByVal X As Integer, ByVal Y As Integer) As Integer
    'Dim lRow As Integer = 0
    'Dim lColumn As Integer = pLeftColumn
    'Dim lColLeft As Integer = 0
    'While lRow < pRowBottom.Count AndAlso Y > pRowBottom(lRow)
    '  lRow += 1
    'End While
    'While lColumn < pColumnRight.Count AndAlso X > pColumnRight(lColumn)
    '  lColLeft = pColumnRight(lColumn)
    '  lColumn += 1
    'End While

    'If lColumn < pColumnRight.Count AndAlso pSource.Alignment(lRow, lColumn) = atcAlignment.HAlignDecimal Then
    '  'If within tolerance of column edge and column is not being hidden by a zero width
    '  If Math.Abs(X - (pColumnRight(lColumn) + lColLeft) / 2) <= DRAG_TOLERANCE AndAlso ColumnWidth(lColumn + pLeftColumn) > 0 Then
    '    Return lColumn + pLeftColumn
    '  End If
    'End If
    Return -1
  End Function

  Private Sub atcGrid_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Resize
    SizeScrollers()
    Refresh()
  End Sub

  Private Sub SizeScrollers()
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
  End Sub

  Private Sub VScroll_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles VScroller.ValueChanged
    VScroller.Focus()
    If pTopRow <> VScroller.Value Then
      pTopRow = VScroller.Value
      Refresh()
    End If
  End Sub

  Private Sub HScroll_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HScroller.ValueChanged
    HScroller.Focus()
    If pLeftColumn <> HScroller.Value Then
      pLeftColumn = HScroller.Value
      Refresh()
    End If
  End Sub

  Private Sub CellEditBox_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles CellEditBox.KeyDown
    Select Case e.KeyCode
      Case Keys.Escape : CellEditBox.Visible = False : EditCellFinished()
      Case Keys.Enter : EditCellFinished()
      Case Keys.Tab : EditCellFinished() 'TODO: shift editing right one column
      Case Keys.Up : EditCellFinished() 'TODO: shift editing 
      Case Keys.Down : EditCellFinished() 'TODO: shift editing 
    End Select
  End Sub

  Private Sub CellEditBox_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles CellEditBox.LostFocus
    EditCellFinished()
  End Sub

  Private Sub VScroller_MouseWheel(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles VScroller.MouseWheel
    Dim lNewValue As Integer = VScroller.Value
    If e.Delta > 0 Then lNewValue -= 1 Else lNewValue += 1
    If lNewValue < VScroller.Minimum Then
      VScroller.Value = VScroller.Minimum
    ElseIf lNewValue > VScroller.Maximum Then
      VScroller.Value = VScroller.Maximum
    Else
      VScroller.Value = lNewValue
    End If
  End Sub

  Protected Overrides Sub OnMouseWheel(ByVal e As System.Windows.Forms.MouseEventArgs)
    VScroller_MouseWheel(Me, e)
  End Sub
End Class
