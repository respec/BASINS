Imports atcUtility
Imports MapWinUtility

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
    Private pVisibleWidth As Integer = 0

    Private pTopRow As Integer

    Private pRowBottom As New atcCollection
    Private pColumnRight As New atcCollection

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
    Friend WithEvents CellComboBox As System.Windows.Forms.ComboBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.VScroller = New System.Windows.Forms.VScrollBar
        Me.HScroller = New System.Windows.Forms.HScrollBar
        Me.scrollCorner = New System.Windows.Forms.Panel
        Me.CellEditBox = New System.Windows.Forms.TextBox
        Me.CellComboBox = New System.Windows.Forms.ComboBox
        Me.SuspendLayout()
        '
        'VScroller
        '
        Me.VScroller.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.VScroller.Location = New System.Drawing.Point(134, 0)
        Me.VScroller.Name = "VScroller"
        Me.VScroller.Size = New System.Drawing.Size(16, 72)
        Me.VScroller.TabIndex = 1
        Me.VScroller.Visible = False
        '
        'HScroller
        '
        Me.HScroller.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.HScroller.Location = New System.Drawing.Point(0, 134)
        Me.HScroller.Name = "HScroller"
        Me.HScroller.Size = New System.Drawing.Size(88, 16)
        Me.HScroller.TabIndex = 2
        Me.HScroller.Visible = False
        '
        'scrollCorner
        '
        Me.scrollCorner.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.scrollCorner.Location = New System.Drawing.Point(134, 134)
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
        'CellComboBox
        '
        Me.CellComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CellComboBox.Location = New System.Drawing.Point(32, 40)
        Me.CellComboBox.Name = "CellComboBox"
        Me.CellComboBox.Size = New System.Drawing.Size(104, 21)
        Me.CellComboBox.TabIndex = 5
        Me.CellComboBox.Visible = False
        '
        'atcGrid
        '
        Me.Controls.Add(Me.CellComboBox)
        Me.Controls.Add(Me.CellEditBox)
        Me.Controls.Add(Me.scrollCorner)
        Me.Controls.Add(Me.HScroller)
        Me.Controls.Add(Me.VScroller)
        Me.Name = "atcGrid"
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

        CellComboBox.Items.Clear()
    End Sub

    Public Property ValidValues() As ICollection
        Get
            Return CellComboBox.Items
        End Get
        Set(ByVal newValues As ICollection)
            CellComboBox.Items.Clear()
            For Each lValue As Object In newValues
                If Not lValue Is Nothing Then CellComboBox.Items.Add(lValue)
            Next
        End Set
    End Property

    Public Property AllowNewValidValues() As Boolean
        Get
            Return CellComboBox.DropDownStyle.Equals(ComboBoxStyle.DropDown)
        End Get
        Set(ByVal newValue As Boolean)
            If newValue Then
                CellComboBox.DropDownStyle = ComboBoxStyle.DropDown
            Else
                CellComboBox.DropDownStyle = ComboBoxStyle.DropDownList
            End If
        End Set
    End Property

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
        If pSource Is Nothing Then
            Return ""
        Else
            Return pSource.ToString
        End If
    End Function

    Public Property ColumnWidth(ByVal aColumn As Integer) As Integer
        Get
            If pColumnWidth.Count = 0 Then
                'Logger.Msg("atcGrid:ColumnWidth:Get: Unexpected pColumnWidth.Count = 0")
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
            If aRow < pRowHeight.Count Then 'Change existing width of this row
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

    ''' <summary>
    ''' Set HScroller.Minimum and HScroller.Maximum from current column widths
    ''' </summary>
    ''' <returns>
    ''' True if HScroller.Value had to be changed to fit between new Minimum and Maximum
    ''' </returns>
    Private Function SetHScroller() As Boolean
        If AllowHorizontalScrolling Then
            Dim lColumns As Integer = pSource.Columns
            Dim lFixedColumns As Integer = pSource.FixedColumns
            Dim lColumn As Integer
            Dim x As Integer = 0
            With HScroller
                For lColumn = 0 To lFixedColumns - 1
                    x += ColumnWidth(lColumn)
                Next
                .Minimum = x
                For lColumn = lFixedColumns To lColumns - 1
                    x += ColumnWidth(lColumn)
                Next
                If x > pVisibleWidth Then
                    .Maximum = x '- Me.Width + .Minimum + VScroller.Width
                    If (lColumns - lFixedColumns) > 0 Then
                        .LargeChange = (.Maximum - .Minimum) / (lColumns - lFixedColumns)
                    Else
                        .LargeChange = 1
                    End If
                    If .Value < .Minimum Then
                        .Value = .Minimum
                        Return True
                    End If
                    If .Value > .Maximum Then
                        .Value = .Maximum
                        Return True
                    End If
                    .Visible = True
                Else
                    .Visible = False
                    If .Value <> .Minimum Then
                        .Value = .Minimum
                        Return True
                    End If
                    End If
                    Return False
            End With
        Else
            HScroller.Visible = False
        End If
    End Function

    Private Sub Render(ByVal g As Graphics)
        Try
            If Me.Visible And Not pSource Is Nothing Then
                If SetHScroller() Then Exit Sub 'Changed HScroller.Value will create new Render

                Dim x As Integer = 0
                Dim y As Integer = 0
                Dim lx As Integer

                Dim lRows As Integer = pSource.Rows
                Dim lRowsVisible As Integer = 0
                Dim lFixedRows As Integer = pSource.FixedRows

                Dim lColumns As Integer = pSource.Columns
                Dim lFixedColumns As Integer = pSource.FixedColumns
                Dim lColumnWidth As Integer

                Dim lRow As Integer
                Dim lColumn As Integer

                Dim lRowIndex As Integer
                Dim lColumnIndex As Integer

                Dim visibleHeight As Integer = Me.Height
                If HScroller.Visible Then visibleHeight -= HScroller.Height

                pVisibleWidth = Me.Width

                If pTopRow > 0 Then        'Scrolled down at least one row
                    If lRows < pTopRow Then  'Scrolled past last row
                        pTopRow = 0            'Reset scrollbar to top row
                        Me.VScroller.Value = 0
                    Else                     'Check to see if all rows could fit
                        y = 0
                        For lRow = lRows - 1 To 0 Step -1
                            y += RowHeight(lRow)
                            If y > visibleHeight Then Exit For
                        Next
                        lRow += 2
                        If lRow < pTopRow Then
                            pTopRow = lRow
                            Me.VScroller.Value = pTopRow
                        End If
                        'If y <= visibleHeight Then 'If all rows can fit
                        '    pTopRow = 0              'Reset scrollbar to top row
                        '    Me.VScroller.Value = 0
                        'End If
                    End If
                End If

                Dim lLinePen As New Pen(pLineColor, pLineWidth)
                Dim lOutsideBrush As New SolidBrush(pLineColor)
                Dim lCellBrush As New SolidBrush(pCellBackColor)

                Dim lEachCellBackColor As Color = pCellBackColor
                Dim lEachCellBackBrush As New SolidBrush(lEachCellBackColor)
                Dim lEachCellTextBrush As New SolidBrush(SystemColors.WindowText)

                'Dim lColorCells As Boolean = pSource.ColorCells

                Dim lCellValue As String
                Dim lCellAlignment As Integer
                Dim lCellValueSize As SizeF
                Dim lVscrollInUse As Boolean = False

                'Clear whole area to default cell background color
                g.FillRectangle(lCellBrush, 0, 0, pVisibleWidth, visibleHeight)

                'Draw Row Lines
                pRowBottom = New atcCollection
                If pTopRow > 0 Then
                    lVscrollInUse = True
                Else
                    VScroller.Visible = False
                End If
                y = -1

                For lRow = 0 To lRows - 1
                    If lRow < lFixedRows OrElse lRow >= pTopRow Then
                        lRowsVisible += 1
                        y += RowHeight(lRow)
                        g.DrawLine(lLinePen, 0, y, pVisibleWidth, y)
                        pRowBottom.Add(lRow, y)
                        If y > visibleHeight Then
                            lVscrollInUse = True
                            If lRowsVisible > 1 Then
                                VScroller.LargeChange = lRowsVisible - 1
                            Else
                                VScroller.LargeChange = 1
                            End If
                            Exit For
                        End If
                    ElseIf lRow < pTopRow - 1 Then 'skip rows we can't see
                        lRow = pTopRow - 1
                    End If
                Next

                If lVscrollInUse Then
                    VScroller.Visible = True
                    pVisibleWidth -= VScroller.Width
                    If lRows > 1 Then VScroller.Maximum = lRows - 1
                End If

                'Fill unused space below bottom line
                If y < visibleHeight Then
                    g.FillRectangle(lOutsideBrush, 0, y, pVisibleWidth, visibleHeight - y)
                End If

                'Draw Column Lines
                pColumnRight = New atcCollection

                x = -1
                For lColumn = 0 To lColumns - 1
                    'ColumnWidth(lColumn) = 100
                    lColumnWidth = ColumnWidth(lColumn)
                    x += lColumnWidth
                    'If lColumn < lFixedColumns Then
                    '    lx = x
                    '    g.DrawLine(lLinePen, lx, 0, lx, visibleHeight)
                    '    pColumnRight.Add(lColumn, lx)
                    'Else
                    If Not HScroller.Visible OrElse x > HScroller.Minimum OrElse lColumn < lFixedColumns Then
                        If HScroller.Visible AndAlso lColumn >= lFixedColumns Then
                            lx = x + HScroller.Minimum - HScroller.Value
                        Else
                            lx = x
                        End If
                        If Not AllowHorizontalScrolling AndAlso lx < pVisibleWidth Then
                            'See if this is the last non-hidden column to expand to fit the available width
                            Dim lScanHiddenColumns As Integer = lColumn + 1
                            While lScanHiddenColumns < lColumns AndAlso ColumnWidth(lScanHiddenColumns) = 0
                                lScanHiddenColumns += 1
                            End While
                            If lScanHiddenColumns = lColumns Then 'Any columns right of this one are hidden
                                'ColumnWidth(lColumn) += pVisibleWidth - x 'Expand this one
                                lx = pVisibleWidth
                            End If
                        End If
                        'If lx > pVisibleWidth Then
                        'lx = pVisibleWidth
                        'End If
                        'Debug.Print("Column Line (" & lColumn & ") " & x & " -> " & lx)
                        g.DrawLine(lLinePen, lx, 0, lx, visibleHeight)
                        pColumnRight.Add(lColumn, lx)
                    End If
                    'End If
                Next

                SizeScrollers()

                'Fill unused space right of rightmost column
                If lx < pVisibleWidth Then
                    g.FillRectangle(lOutsideBrush, lx, 0, pVisibleWidth - lx, visibleHeight)
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
                            If lColumnIndex >= lFixedColumns AndAlso HScroller.Visible AndAlso lCellLeft < HScroller.Minimum Then
                                'Change left edge for clipping cell contents
                                lCellLeft = HScroller.Minimum
                            End If
                            Dim lClipRect As New System.Drawing.Rectangle(lCellLeft, lCellTop, lCellRight - lCellLeft, lCellBottom - lCellTop)
                            g.SetClip(lClipRect, Drawing2D.CombineMode.Replace)
                            If lCellLeft = 0 OrElse lCellLeft = HScroller.Minimum Then
                                'Change left edge for measuring where to put cell contents
                                lCellLeft = lCellRight - ColumnWidth(lColumn)
                            End If
                            If lColumnIndex < lFixedColumns OrElse Not HScroller.Visible OrElse lCellRight > HScroller.Minimum Then
                                If pSource.CellSelected(lRow, lColumn) Then
                                    lEachCellBackColor = SystemColors.Highlight
                                    lEachCellTextBrush.Color = SystemColors.HighlightText
                                Else 'If lColorCells Then
                                    lEachCellBackColor = pSource.CellColor(lRow, lColumn)
                                    lEachCellTextBrush.Color = SystemColors.WindowText
                                    'Else
                                    '    lEachCellBackColor = pCellBackColor
                                    '    lEachCellTextBrush.Color = SystemColors.WindowText
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
                                    While lCellValueSize.Width > ColumnWidth(lColumn) '(lCellRight - lCellLeft) '
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
                                            'Draw debugging line down center of cell to check for correct decimal alignment
                                            'x = lCellLeft + (lCellRight - lCellLeft) / 2
                                            'g.DrawLine(lLinePen, x, lCellTop, x, lCellBottom)
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
                                        g.ResetClip()
                                    Catch winErr As Exception
                                    End Try
                                End If
                            End If
                            lCellLeft = lCellRight + 1 'Left of next column is right of this one
                        End If
                    Next
                Next
            End If
        Catch e As Exception
            Logger.Dbg(e.Message)
        End Try
    End Sub

    'Private Function MeasureDisplayStringWidth(ByVal aText As String, ByVal g As Graphics)
    '  Dim lFormat As New System.Drawing.StringFormat
    '  Dim lRect As RectangleF = New System.Drawing.RectangleF(0, 0, 1000, 1000)
    '  Dim lRanges() As CharacterRange = {New System.Drawing.CharacterRange(0, aText.Length)}
    '  Dim lRegions() As Region

    '  lFormat.SetMeasurableCharacterRanges(lRanges)

    '  lRegions = g.MeasureCharacterRanges(aText, pFont, lRect, lFormat)
    '  lRect = lRegions(0).GetBounds(g)

    '  Return CInt(lRect.Right + 1.0)
    'End Function

    Private Function WidthLeftOfDecimal(ByVal aText As String, ByVal g As Graphics) As Integer
        Dim lCellValueSize As SizeF
        If IsNumeric(aText) Then
            Dim lDecimalPos As Integer = aText.IndexOf(".")
            'If lDecimalPos = -1 Then
            '  Return MeasureDisplayStringWidth(aText & ".", g)
            'Else
            '  Return MeasureDisplayStringWidth(aText.Substring(0, lDecimalPos + 1), g)
            'End If
            If lDecimalPos = -1 Then 'TODO: use something more accurate than .net 1.1 MeasureString
                lCellValueSize = g.MeasureString(aText, pFont)
            Else
                lCellValueSize = g.MeasureString(aText.Substring(0, lDecimalPos), pFont)
            End If
            Return lCellValueSize.Width
        Else 'Center non-numeric values
            Return g.MeasureString(aText, pFont).Width / 2
        End If
    End Function

    Private Function WidthRightOfDecimal(ByVal aText As String, ByVal g As Graphics) As Integer
        If IsNumeric(aText) Then
            Dim lDecimalPos As Integer = aText.IndexOf(".")
            If lDecimalPos = -1 Then
                Return 0
            Else
                Return g.MeasureString(aText.Substring(lDecimalPos + 1), pFont).Width
            End If
        Else 'Center non-numeric values
            Return g.MeasureString(aText, pFont).Width / 2
        End If
    End Function

    ' aTotalWidth = desired final total width of all columns
    ' if aShrinkToTotalWidth is false, columns will not be resized smaller to match aTotalWidth
    Public Sub SizeAllColumnsToContents(Optional ByVal aTotalWidth As Integer = 0, _
                                        Optional ByVal aShrinkToTotalWidth As Boolean = False)
        If Not pSource Is Nothing Then
            Dim lMaxColumn As Integer = pSource.Columns - 1
            Dim lContentsWidth As Integer = 0
            For lCol As Integer = 0 To lMaxColumn
                SizeColumnToContents(lCol)
                lContentsWidth += ColumnWidth(lCol)
            Next
            If aTotalWidth > 0 AndAlso (aShrinkToTotalWidth OrElse aTotalWidth > lContentsWidth) Then
                For lCol As Integer = 0 To lMaxColumn
                    ColumnWidth(lCol) = ColumnWidth(lCol) * aTotalWidth / lContentsWidth
                Next
            End If
        End If
    End Sub

    Public Sub SizeColumnToContents(ByVal aColumn As Integer)
        If Not pSource Is Nothing Then
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
        End If
    End Sub

    Public Sub SizeColumnToString(ByVal aColumn As Integer, ByVal aString As String)
        If Not aString Is Nothing AndAlso aString.Length > 0 Then
            Dim lCellWidth As Integer
            Dim g As Graphics = Me.CreateGraphics
            If (pSource.Alignment(0, aColumn) And atcAlignment.HAlign) = atcAlignment.HAlignDecimal Then
                Dim lDecimalWidth As Integer = g.MeasureString(".", pFont).Width
                lCellWidth = lDecimalWidth + 2 * Math.Max(WidthLeftOfDecimal(aString, g), WidthRightOfDecimal(aString, g))
            Else
                lCellWidth = g.MeasureString(aString, pFont).Width
            End If
            ColumnWidth(aColumn) = lCellWidth + DRAG_TOLERANCE * 2
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

    Private Function CellBounds(ByVal aRow As Integer, ByVal aColumn As Integer) As Rectangle
        Dim lX As Integer = 0
        Dim lY As Integer = 0
        Dim lWidth As Integer = 0
        Dim lFixedRows As Integer = pSource.FixedRows
        Dim lFixedColumns As Integer = pSource.FixedColumns

        For lRow As Integer = 0 To aRow - 1
            If lRow < lFixedRows OrElse lRow >= pTopRow Then
                lY += RowHeight(lRow)
            ElseIf lRow < pTopRow - 1 Then 'skip rows we can't see
                lRow = pTopRow - 1
            End If
        Next

        If aColumn = 0 Then
            lX = 0
        Else
            lX = pColumnRight(aColumn - 1)
        End If

        lWidth = pColumnRight(aColumn) - lX
        If lWidth < 0 Then lWidth = 0

        Return New Rectangle(lX, lY, lWidth, RowHeight(aRow))
    End Function

    Public Sub EditCell(ByVal aRow As Integer, ByVal aColumn As Integer, Optional ByVal aOverrideEditable As Boolean = False)
        If Not pSource Is Nothing Then
            EditCellFinished()
            If aOverrideEditable OrElse pSource.CellEditable(aRow, aColumn) Then
                Dim EditCellBounds As Rectangle = CellBounds(aRow, aColumn)
                pColumnEditing = aColumn
                pRowEditing = aRow
                If CellComboBox.Items.Count > 0 Then
                    CellComboBox.Font = pFont
                    CellComboBox.Text = pSource.CellValue(aRow, aColumn)
                    CellComboBox.BackColor = pSource.CellColor(aRow, aColumn)
                    CellComboBox.SetBounds(EditCellBounds.Left, EditCellBounds.Top, EditCellBounds.Width, EditCellBounds.Height)
                    CellComboBox.Visible = True
                    CellComboBox.Focus()
                Else
                    CellEditBox.Font = pFont
                    CellEditBox.Text = pSource.CellValue(aRow, aColumn)
                    CellEditBox.BackColor = pSource.CellColor(aRow, aColumn)
                    CellEditBox.SetBounds(EditCellBounds.Left, EditCellBounds.Top, EditCellBounds.Width, EditCellBounds.Height)
                    CellEditBox.Visible = True
                    CellEditBox.Focus()
                End If
            End If
        End If
    End Sub

    Public Sub EditCellFinished()
        If CellComboBox.Visible Then
            ChangeEditingValues(CellComboBox.Text)
            CellComboBox.Visible = False
        End If
        If CellEditBox.Visible Then
            ChangeEditingValues(CellEditBox.Text)
            CellEditBox.Visible = False
        End If
    End Sub

    Private Sub ChangeEditingValues(ByVal aNewValue As String)
        If aNewValue <> pSource.CellValue(pRowEditing, pColumnEditing) Then
            pSource.CellValue(pRowEditing, pColumnEditing) = aNewValue
            RaiseEvent CellEdited(Me, pRowEditing, pColumnEditing)
        End If
    End Sub

    Protected Overrides Sub OnMouseDown(ByVal e As System.Windows.Forms.MouseEventArgs)
        If Not Me.Focused Then Me.Focus()
        If pRowBottom.Count > 0 Then
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
                    RaiseEvent MouseDownCell(Me, lRow, lColumn)
                    If pSource.CellEditable(lRow, lColumn) Then
                        EditCell(lRow, lColumn)
                    End If
                End If
            End If
        End If
    End Sub

    Protected Overrides Sub OnMouseMove(ByVal e As System.Windows.Forms.MouseEventArgs)
        'Debug.Print(e.X & " " & e.Y)
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
        CellComboBox.Visible = False
        CellEditBox.Visible = False
        SizeScrollers()
        Refresh()
    End Sub

    Private Sub SizeScrollers()
        If HScroller.Visible Then
            VScroller.Height = Me.Height - HScroller.Height
        Else
            VScroller.Height = Me.Height
            'If VScroller.Focused Then 'Work around buggy scrollbar focus effect
            '    Me.Focus()
            '    VScroller.Focus()
            'End If
        End If

        If VScroller.Visible Then
            HScroller.Width = Me.Width - VScroller.Width
            'If HScroller.Focused Then 'Work around buggy scrollbar focus effect
            '    Me.Focus()
            '    HScroller.Focus()
            'End If
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
        'Debug.Print("HscrollChanged to " & HScroller.Value)
        HScroller.Focus()
        Refresh()
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

    Private Sub CellComboBox_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles CellComboBox.KeyDown
        Select Case e.KeyCode
            Case Keys.Escape : CellComboBox.Visible = False : EditCellFinished()
            Case Keys.Enter : EditCellFinished()
            Case Keys.Tab : EditCellFinished() 'TODO: shift editing right one column
        End Select
    End Sub

    Private Sub CellComboBox_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles CellComboBox.LostFocus
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

    Private Sub VScroller_MouseEnter(ByVal sender As Object, ByVal e As System.EventArgs) Handles VScroller.MouseEnter
        Me.Cursor = Cursors.Arrow
    End Sub

    Private Sub HScroller_MouseEnter(ByVal sender As Object, ByVal e As System.EventArgs) Handles HScroller.MouseEnter
        Me.Cursor = Cursors.Arrow
    End Sub
End Class
