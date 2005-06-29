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
  <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
    components = New System.ComponentModel.Container
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
      g.FillRectangle(lCellBrush, 0, 0, g.VisibleClipBounds.Width, g.VisibleClipBounds.Height)

      'Draw Row Lines
      pRowBottom = New ArrayList
      For iRow = pTopRow To lRows - 1
        y += RowHeight(iRow)
        g.DrawLine(lLinePen, g.VisibleClipBounds.Left, y, g.VisibleClipBounds.Right, y)
        pRowBottom.Add(y)
        If y > g.VisibleClipBounds.Bottom Then Exit For
      Next
      'Fill unused space below bottom line
      g.FillRectangle(lOutsideBrush, 0, y, g.VisibleClipBounds.Width, g.VisibleClipBounds.Height - y)

      'Draw Column Lines
      pColRight = New ArrayList
      For iColumn = pLeftColumn To lColumns - 1
        x += ColumnWidth(iColumn)
        g.DrawLine(lLinePen, x, g.VisibleClipBounds.Top, x, g.VisibleClipBounds.Bottom)
        pColRight.Add(x)
      Next
      'Fill unused space right of rightmost column
      g.FillRectangle(lOutsideBrush, x, 0, g.VisibleClipBounds.Width - x, g.VisibleClipBounds.Height)

      Dim lCellLeft As Single
      Dim lCellTop As Single = 0
      Dim lastRowDrawn As Integer = pTopRow + pRowBottom.Count - 1

      For iRow = pTopRow To lastRowDrawn
        lCellLeft = 0
        For iColumn = pLeftColumn To lColumns - 1
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
                  x = pColRight(iColumn) - lCellValueSize.Width
                Case atcAlignment.HAlignCenter
                  x = lCellLeft + (pColRight(iColumn) - lCellLeft - lCellValueSize.Width) / 2
                  'Case atcAlignment.HAlignDecimal 'TODO: implement decimal alignment
                Case Else 'Default to left alignment 
                  x = lCellLeft
              End Select
              Select Case lCellAlignment And atcAlignment.VAlign
                Case atcAlignment.VAlignTop
                  y = lCellTop
                Case atcAlignment.VAlignBottom
                  y = pRowBottom(iRow) - lCellValueSize.Height
                Case Else 'atcAlignment.VAlignCenter 'Default to centering vertically 
                  y = lCellTop + (pRowBottom(iRow) - lCellTop - lCellValueSize.Height) / 2
              End Select
              g.DrawString(lCellValue, pFont, Brushes.Black, x, y) 'TODO: allow flexibility of brush
            End If
            If iColumn < lColumns - 1 Then lCellLeft = pColRight(iColumn)
          End If
        Next
        If iRow < lRows - 1 Then lCellTop = pRowBottom(iRow)
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
      RaiseEvent MouseDownCell(lRow, lColumn)
    End If
  End Sub

  Private Sub atcGrid_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Resize
    Me.Refresh()
  End Sub
End Class
