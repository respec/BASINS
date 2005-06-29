Public Class atcGrid
  Inherits System.Windows.Forms.UserControl

  Event MouseDownCell(ByVal aRow As Integer, ByVal aColumn As Integer)

  Private WithEvents pSource As atcGridSource

  Private pFont As Font = New Font("Microsoft Sans Serif", 8.25, FontStyle.Regular, GraphicsUnit.Point)

  Private pLineColor As Color
  Private pLineWidth As Single
  Private pCellBackColor As Color
  Private pRowHeight As ArrayList   'of Single
  Private pColumnWidth As ArrayList 'of Single

  Private pTopRow As Integer
  Private pLeftColumn As Integer

  Private pRowBottom As ArrayList
  Private pColRight As ArrayList

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
  Friend WithEvents VScroll As System.Windows.Forms.VScrollBar
  Friend WithEvents HScroll As System.Windows.Forms.HScrollBar
  Friend WithEvents scrollCorner As System.Windows.Forms.Panel
  <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
    Me.VScroll = New System.Windows.Forms.VScrollBar
    Me.HScroll = New System.Windows.Forms.HScrollBar
    Me.scrollCorner = New System.Windows.Forms.Panel
    Me.SuspendLayout()
    '
    'VScroll
    '
    Me.VScroll.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.VScroll.Location = New System.Drawing.Point(144, 0)
    Me.VScroll.Name = "VScroll"
    Me.VScroll.Size = New System.Drawing.Size(16, 72)
    Me.VScroll.TabIndex = 1
    Me.VScroll.Visible = False
    '
    'HScroll
    '
    Me.HScroll.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
    Me.HScroll.Location = New System.Drawing.Point(0, 144)
    Me.HScroll.Name = "HScroll"
    Me.HScroll.Size = New System.Drawing.Size(88, 16)
    Me.HScroll.TabIndex = 2
    Me.HScroll.Visible = False
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
    Me.Controls.Add(Me.HScroll)
    Me.Controls.Add(Me.VScroll)
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
  End Sub

  Public Sub Initialize(ByVal aSource As atcGridSource)
    pSource = aSource
    Clear()
  End Sub

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
      Dim visibleHeight As Single = Me.Height
      Dim visibleWidth As Single = Me.Width
      Dim lLinePen As New Pen(pLineColor, pLineWidth)
      Dim lOutsideBrush As New SolidBrush(pLineColor)
      Dim lCellBrush As New SolidBrush(pCellBackColor)

      Dim iRow As Integer
      Dim iColumn As Integer
      Dim lRows As Integer = pSource.Rows
      Dim lColumns As Integer = pSource.Columns
      Dim lCellValue As String
      Dim lCellAlignment As Integer
      Dim lCellValueSize As SizeF

      'Clear whole area to default cell background color
      g.FillRectangle(lCellBrush, 0, 0, visibleWidth, visibleHeight)

      'Draw Row Lines
      pRowBottom = New ArrayList
      If pTopRow = 0 Then VScroll.Visible = False
      For iRow = pTopRow To lRows - 1
        y += RowHeight(iRow)
        g.DrawLine(lLinePen, 0, y, visibleWidth, y)
        pRowBottom.Add(y)
        If y > visibleHeight Then
          visibleWidth -= VScroll.Width
          VScroll.Visible = True
          If iRow - pTopRow > 2 Then
            VScroll.LargeChange = iRow - pTopRow - 1
          Else
            VScroll.LargeChange = 1
          End If
          VScroll.Maximum = pSource.Rows '- VScroll.LargeChange + 2
          'Debug.WriteLine("Rows = " & lRows & ", TopRow = " & pTopRow & ", LargeChange = " & VScroll.LargeChange & ", Maximum = " & VScroll.Maximum)
          Exit For
        End If
      Next
      'Fill unused space below bottom line
      g.FillRectangle(lOutsideBrush, 0, y, visibleWidth, visibleHeight - y)

      'Draw Column Lines
      pColRight = New ArrayList
      If pLeftColumn = 0 Then HScroll.Visible = False
      For iColumn = pLeftColumn To lColumns - 1
        x += ColumnWidth(iColumn)
        g.DrawLine(lLinePen, x, 0, x, visibleHeight)
        pColRight.Add(x)
        If x > visibleWidth Then
          visibleHeight -= HScroll.Height
          If Not VScroll.Visible AndAlso y > visibleHeight Then
            'TODO: add VScroll because last line got hidden by HScroll
          End If
          HScroll.Visible = True
          If iColumn - pLeftColumn > 2 Then
            HScroll.LargeChange = iColumn - pLeftColumn - 1
          Else
            HScroll.LargeChange = 1
          End If
          HScroll.Maximum = pSource.Columns '- HScroll.LargeChange + 1
          Exit For
        End If
      Next
      'Fill unused space right of rightmost column
      g.FillRectangle(lOutsideBrush, x, 0, visibleWidth - x, visibleHeight)

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
              Select Case lCellAlignment And atcAlignment.HAlign
                Case atcAlignment.HAlignLeft
                  x = lCellLeft
                Case atcAlignment.HAlignRight
                  x = pColRight(iColumn - pLeftColumn) - lCellValueSize.Width
                Case atcAlignment.HAlignCenter
                  x = lCellLeft + (pColRight(iColumn - pLeftColumn) - lCellLeft - lCellValueSize.Width) / 2
                  'Case atcAlignment.HAlignDecimal 'TODO: implement decimal alignment
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
              g.DrawString(lCellValue, pFont, Brushes.Black, x, y) 'TODO: allow flexibility of brush
            End If
            If iColumn < lColumns - 1 Then lCellLeft = pColRight(iColumn - pLeftColumn)
          End If
        Next
        If iRow < lRows - 1 Then lCellTop = pRowBottom(iRow - pTopRow) 'Top of next row is bottom of this one
      Next
    End If
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
    Dim lRow As Integer = 0
    Dim lColumn As Integer = 0
    While lRow < pRowBottom.Count AndAlso e.Y > pRowBottom(lRow)
      lRow += 1
    End While
    While lColumn < pColRight.Count AndAlso e.X > pColRight(lColumn)
      lColumn += 1
    End While
    If lRow < pSource.Rows And lColumn < pSource.Columns Then
      RaiseEvent MouseDownCell(lRow + pTopRow, lColumn + pLeftColumn)
    End If
  End Sub

  Private Sub atcGrid_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Resize
    If HScroll.Visible Then
      VScroll.Height = Me.Height - HScroll.Height
    Else
      VScroll.Height = Me.Height
    End If

    If VScroll.Visible Then
      HScroll.Width = Me.Width - VScroll.Width
    Else
      HScroll.Width = Me.Width
    End If

    scrollCorner.Visible = VScroll.Visible And HScroll.Visible

    Me.Refresh()
  End Sub

  Private Sub VScroll_ValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles VScroll.ValueChanged
    pTopRow = VScroll.Value
    Refresh()
  End Sub

  Private Sub HScroll_ValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles HScroll.ValueChanged
    pLeftColumn = HScroll.Value
    Refresh()
  End Sub
End Class
