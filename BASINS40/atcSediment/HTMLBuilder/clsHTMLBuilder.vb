Imports System.Text
Imports System.Text.StringBuilder

Public Class clsHTMLBuilder


    Private Structure structColumn
        Dim Heading As String
        Dim Align As enumAlign
        Dim Width As Integer
        Dim WidthUnits As enumWidthUnits
    End Structure

    Public Enum enumAlign
        Auto
        Center
        Left
        Right
    End Enum

    Public Enum enumBorderStyle
        none
        dotted
        dashed
        solid
    End Enum

    Public Enum enumColors
        aqua
        black
        blue
        fuchsia
        gray
        green
        lime
        maroon
        navy
        olive
        purple
        red
        silver
        teal
        white
        yellow
    End Enum

    Public Enum enumDividerStyle
        All
        Cols
        Groups
        None
        Rows
    End Enum

    Public Enum enumFontSize
        Tiny = 1
        ExtraSmall
        Small
        Medium
        Large
        ExtraLarge
        Huge
    End Enum

    Public Enum enumFontStyle
        Bold = 1
        Italic = 2
        Underline = 4
        Subscript = 8
        Superscript = 16
    End Enum

    Public Enum enumHeading
        Level1 = 1
        Level2
        Level3
        Level4
        Level5
        Level6
    End Enum

    Public Enum enumListType
        Bulleted
        Numbered
    End Enum

    Public Enum enumWidthUnits
        None
        Percent
        Pixels
        Proportional
    End Enum

    Const _CellPadding = "padding: 2px 5px"
    Private _Columns As New Generic.List(Of structColumn)

    Private _HTMLColor() As String = {"aqua", "black", "blue", "fuchsia", "gray", "green", "lime", "maroon", "navy", "olive", "purple", "red", "silver", "teal", "white", "yellow"}
    Private _NextColumnNum As Integer

    Private sb As StringBuilder

    Public Sub New(ByVal Value As String)
        sb = New StringBuilder(Value)
    End Sub

    Public Sub New()
        sb = New StringBuilder()
    End Sub

    Private Function AlignTag(ByVal Align As enumAlign) As String
        Dim tag As String = ""
        Select Case Align
            Case enumAlign.Center : tag = "center"
            Case enumAlign.Left : tag = "left"
            Case enumAlign.Right : tag = "right"
        End Select
        Return String.Format("align='{0}'", tag)
    End Function

    Private Function AlignTag(ByVal Align As enumAlign, ByVal CellText As String) As String
        Dim tag As String = ""
        Select Case Align
            Case enumAlign.Auto : If IsNumeric(CellText) Then tag = "right" Else tag = "left"
            Case enumAlign.Center : tag = "center"
            Case enumAlign.Left : tag = "left"
            Case enumAlign.Right : tag = "right"
        End Select
        Return String.Format("align='{0}'", tag)
    End Function

    ''' <summary>
    ''' Append text to document
    ''' </summary>
    ''' <param name="Value">Text to append</param>
    Public Function Append(ByVal Value As String) As clsHTMLBuilder
        sb.Append(HTMLText(Value))
        Return Me
    End Function

    ''' <summary>
    ''' Append text to document
    ''' </summary>
    ''' <param name="Format">String format</param>
    ''' <param name="Args">Arguments</param>
    Public Function Append(ByVal Format As String, ByVal ParamArray Args() As String) As clsHTMLBuilder
        Return Append(String.Format(Format, Args))
    End Function

    ''' <summary>
    ''' Begin a new font size override
    ''' </summary>
    ''' <param name="FontSize">One of seven standard font sizes</param>
    Public Function AppendFont(ByVal FontSize As enumFontSize, ByVal FontColor As enumColors) As clsHTMLBuilder
        sb.AppendFormat("<font size={0} color='{1}'>", CInt(FontSize), HTMLColor(FontColor))
        Return Me
    End Function

    ''' <summary>
    ''' End prior font size override
    ''' </summary>
    Public Function AppendFontEnd() As clsHTMLBuilder
        sb.Append("</font>")
        Return Me
    End Function

    ''' <summary>
    ''' Append text to document
    ''' </summary>
    ''' <param name="Level">Heading level</param>
    ''' <param name="Value">Text to append</param>
    Public Function AppendHeading(ByVal Level As enumHeading, ByVal Align As enumAlign, ByVal Value As String) As clsHTMLBuilder
        sb.AppendFormat("<h{0} {1}>{2}</h{0}>", CInt(Level), AlignTag(Align), HTMLText(Value))
        Return Me
    End Function

    ''' <summary>
    ''' Append text to document
    ''' </summary>
    ''' <param name="Level">Heading level</param>
    ''' <param name="Format">String format</param>
    ''' <param name="Args">Arguments</param>
    Public Function AppendHeading(ByVal Level As enumHeading, ByVal Align As enumAlign, ByVal Format As String, ByVal ParamArray Args() As String) As clsHTMLBuilder
        Return AppendHeading(Level, Align, String.Format(Format, Args))
    End Function

    ''' <summary>
    ''' Append a horizontal line
    ''' </summary>
    Public Function AppendHorizLine() As clsHTMLBuilder
        sb.Append("<hr>")
        Return Me
    End Function

    ''' <summary>
    ''' Append raw HTML text
    ''' </summary>
    Public Function AppendHTML(ByVal HTMLText As String) As clsHTMLBuilder
        sb.AppendLine(HTMLText)
        Return Me
    End Function

    ''' <summary>
    ''' Append an image
    ''' </summary>
    ''' <param name="ImageFile">Name of image file (path is relative to HTML file)</param>
    ''' <param name="Width">Width of units (height is set using original image aspect ratio)</param>
    ''' <param name="WidthUnits">Width units</param>
    Public Function AppendImage(ByVal ImageFile As String, Optional ByVal Width As Integer = 100, Optional ByVal WidthUnits As enumWidthUnits = enumWidthUnits.Percent, Optional ByVal BorderWidth As Integer = 1) As clsHTMLBuilder
        sb.AppendFormat("<img src='{0}' {1} border='{2}'/>", ImageFile, WidthTag(Width, WidthUnits), BorderWidth)
        Return Me
    End Function

    ''' <summary>
    ''' Append a line break within a paragraph
    ''' </summary>
    Public Function AppendLineBreak() As clsHTMLBuilder
        sb.Append("<br>")
        Return Me
    End Function

    ''' <summary>
    ''' Append either a numbered or bulleted list
    ''' </summary>
    ''' <param name="ListType">Type of list</param>
    ''' <param name="Args">Items in list</param>
    Public Function AppendList(ByVal ListType As enumListType, ByVal ParamArray Args() As String)
        Dim tag As String
        If ListType = enumListType.Bulleted Then tag = "ul" Else tag = "ol"
        sb.AppendFormat("<{0}>", tag)
        For Each s As String In Args
            sb.AppendFormat("<li>{0}</li>", s)
        Next
        sb.AppendFormat("</{0}>", tag)
        Return Me
    End Function

    ''' <summary>
    ''' Append text as entire paragraph (end tag is applied)
    ''' </summary>
    ''' <param name="Align">Paragraph alignment</param>
    ''' <param name="Value">Text</param>
    Public Function AppendPara(ByVal Align As enumAlign, ByVal Value As String) As clsHTMLBuilder
        sb.AppendFormat("<p {0}>{1}</p>", AlignTag(Align), HTMLText(Value))
        sb.AppendLine()
        Return Me
    End Function

    ''' <summary>
    ''' Append text as entire paragraph (end tag is applied)
    ''' </summary>
    ''' <param name="Align">Paragraph alignment</param>
    ''' <param name="Format">Format string</param>
    ''' <param name="Args">Arguments</param>
    Public Function AppendPara(ByVal Align As enumAlign, ByVal Format As String, ByVal Args() As String) As clsHTMLBuilder
        Return AppendPara(Align, String.Format(Format, Args))
    End Function

    ''' <summary>
    ''' Begin a new paragraph
    ''' </summary>
    ''' <param name="Align">Paragraph alignment</param>
    Public Function AppendPara(ByVal Align As enumAlign) As clsHTMLBuilder
        sb.AppendFormat("<p {0}>", AlignTag(Align))
        Return Me
    End Function

    ''' <summary>
    ''' End prior paragraph
    ''' </summary>
    Public Function AppendParaEnd() As clsHTMLBuilder
        sb.AppendLine("</p>")
        Return Me
    End Function

    ''' <summary>
    ''' Append text formatted using specified font style (end tag is applied)
    ''' </summary>
    ''' <param name="Style">Text style</param>
    ''' <param name="Value">Text to format</param>
    Public Function AppendStyle(ByVal Style As enumFontStyle, ByVal Value As String) As clsHTMLBuilder
        Dim tag As String = ""
        If (Style And enumFontStyle.Bold) = enumFontStyle.Bold Then tag &= "<b>"
        If (Style And enumFontStyle.Italic) = enumFontStyle.Italic Then tag &= "<i>"
        If (Style And enumFontStyle.Underline) = enumFontStyle.Underline Then tag &= "<u>"
        If (Style And enumFontStyle.Subscript) = enumFontStyle.Subscript Then tag &= "<sub>"
        If (Style And enumFontStyle.Superscript) = enumFontStyle.Superscript Then tag &= "<sup>"
        sb.AppendFormat("{0}{1}{2}", tag, HTMLText(Value), tag.Replace("<", "</"))
        Return Me
    End Function

    ''' <summary>
    ''' Append text formatted using specified font style (end tag is applied)
    ''' </summary>
    ''' <param name="Style">Text style</param>
    ''' <param name="Format">String format</param>
    ''' <param name="Args">Arguements</param>
    Public Function AppendStyle(ByVal Style As enumFontStyle, ByVal Format As String, ByVal Args() As String) As clsHTMLBuilder
        Return AppendStyle(Style, String.Format(Format, Args))
    End Function

    ''' <summary>
    ''' Begin a new table; must be followed by calls to AppendRow and AppendTableEnd
    ''' </summary>
    ''' <param name="TableWidth">Width of table</param>
    ''' <param name="WidthUnits">Units for width</param>
    ''' <param name="BorderWidth">Width of border in pixels</param>
    ''' <param name="BorderStyle">Border style around table</param>
    ''' <param name="DividerStyle">Format for cell dividers</param>
    Public Function AppendTable(Optional ByVal TableWidth As Integer = 0, Optional ByVal WidthUnits As enumWidthUnits = enumWidthUnits.None, Optional ByVal BorderStyle As enumBorderStyle = enumBorderStyle.solid, Optional ByVal BorderWidth As Integer = 1, Optional ByVal DividerStyle As enumDividerStyle = enumDividerStyle.All) As clsHTMLBuilder
        _Columns.Clear()
        sb.AppendFormat("<table {0} rules={1} frame='void' style='font-family: sans-serif; font-size: 70%; border-collapse: collapse; border: {2} black {3}'>", WidthTag(TableWidth, WidthUnits), DividerStyle, BorderWidth, BorderStyle)
        sb.AppendLine()
        Return Me
    End Function

    ''' <summary>
    ''' Create a table and fill it with the contents of a datagridview
    ''' </summary>
    ''' <param name="dg">Datagridview to take data from</param>
    ''' <param name="Title">Left aligned, bold and underlined table title</param>
    ''' <param name="TableWidth">Width of table</param>
    ''' <param name="WidthUnits">Units for width</param>
    ''' <param name="BorderWidth">Width of border in pixels</param>
    ''' <param name="BorderStyle">Border style around table</param>
    ''' <param name="DividerStyle">Format for cell dividers</param>
    Public Function AppendTable(ByVal dg As DataGridView, Optional ByVal Title As String = "", Optional ByVal TableWidth As Integer = 0, Optional ByVal WidthUnits As enumWidthUnits = enumWidthUnits.None, Optional ByVal BorderStyle As enumBorderStyle = enumBorderStyle.solid, Optional ByVal BorderWidth As Integer = 1, Optional ByVal DividerStyle As enumDividerStyle = enumDividerStyle.All) As clsHTMLBuilder

        If Title <> "" Then
            AppendPara(clsHTMLBuilder.enumAlign.Left)
            AppendStyle(clsHTMLBuilder.enumFontStyle.Bold Or clsHTMLBuilder.enumFontStyle.Underline, Title)
            AppendParaEnd()
        End If

        AppendTable(TableWidth, WidthUnits, BorderStyle, BorderWidth, DividerStyle)
        For c As Integer = 0 To dg.ColumnCount - 1
            With dg.Columns(c)
                AppendTableColumn(.HeaderText)
            End With
        Next
        For r As Integer = 0 To dg.RowCount - IIf(dg.AllowUserToAddRows, 2, 1)
            AppendTableRow()
            For c As Integer = 0 To 2
                Dim Align As clsHTMLBuilder.enumAlign
                Select Case dg.Columns(c).DefaultCellStyle.Alignment
                    Case DataGridViewContentAlignment.BottomLeft, DataGridViewContentAlignment.MiddleLeft, DataGridViewContentAlignment.TopLeft
                        Align = enumAlign.Left
                    Case DataGridViewContentAlignment.BottomCenter, DataGridViewContentAlignment.MiddleCenter, DataGridViewContentAlignment.TopCenter
                        Align = enumAlign.Center
                    Case DataGridViewContentAlignment.BottomRight, DataGridViewContentAlignment.MiddleRight, DataGridViewContentAlignment.TopRight
                        Align = enumAlign.Right
                    Case Else
                        Align = enumAlign.Auto
                End Select
                AppendTableCell(Align, dg.Item(c, r).Value)
            Next
            AppendTableRowEnd()
        Next
        AppendTableEnd()
        Return Me
    End Function

    ''' <summary>
    ''' Add a new cell to the current row (started with AppendTableRow); will default to column alignment and width set with AppendTableColumn calls
    ''' </summary>
    ''' <param name="cell">Text in cell</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function AppendTableCell(ByVal cell As String) As clsHTMLBuilder
        With _Columns(_NextColumnNum)
            sb.AppendFormat("<td style='{0}' {1} {2}>{3}</td>", _CellPadding, AlignTag(.Align, cell), WidthTag(.Width, .WidthUnits), HTMLText(cell))
        End With
        sb.AppendLine()
        _NextColumnNum += 1
        Return Me
    End Function

    ''' <summary>
    ''' Start adding a new cell to the current row (started with AppendTableRow); will default to column alignment and width set with AppendTableColumn calls
    ''' Should be followed by Append, AppendFont, AppendHeading, etc., then AppendTableCellEnd
    ''' </summary>
    Public Function AppendTableCell() As clsHTMLBuilder
        If _Columns.Count = 0 Then
            sb.AppendFormat("<td>")
        Else
            With _Columns(_NextColumnNum)
                sb.AppendFormat("<td style='{0}' {1} {2}>", _CellPadding, AlignTag(.Align), WidthTag(.Width, .WidthUnits))
            End With
        End If
        sb.AppendLine()
        Return Me
    End Function

    ''' <summary>
    ''' Add a new cell to the current row (started with AppendTableRow); will default to column alignment and width set with AppendTableColumn calls
    ''' </summary>
    ''' <param name="format">Format string</param>
    ''' <param name="args">Arguments</param>
    Public Function AppendTableCell(ByVal format As String, ByVal ParamArray args() As String) As clsHTMLBuilder
        Dim cell As String = String.Format(format, args)
        If _Columns.Count = 0 Then
            sb.AppendFormat("<td style='{0}' {1} {2}>{3}</td>", _CellPadding, AlignTag(enumAlign.Auto, cell), WidthTag(0, enumWidthUnits.None), HTMLText(cell))
        Else
            With _Columns(_NextColumnNum)
                sb.AppendFormat("<td {0} {1}>{2}</td>", AlignTag(.Align, cell), WidthTag(.Width, .WidthUnits), HTMLText(cell))
            End With
        End If
        sb.AppendLine()
        _NextColumnNum += 1
        Return Me
    End Function

    ''' <summary>
    ''' Add a new cell to the current row (started with AppendTableRow); will default to column alignment and width set with AppendTableColumn calls
    ''' </summary>
    ''' <param name="cell">Text in cell</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function AppendTableCell(ByVal Align As enumAlign, ByVal cell As String) As clsHTMLBuilder
        With _Columns(_NextColumnNum)
            sb.AppendFormat("<td style='{0}' {1} {2}>{3}</td>", _CellPadding, AlignTag(Align, cell), WidthTag(.Width, .WidthUnits), HTMLText(cell))
            sb.AppendLine()
        End With
        _NextColumnNum += 1
        Return Me
    End Function

    ''' <summary>
    ''' Finish adding cell contents
    ''' </summary>
    Public Function AppendTableCellEnd() As clsHTMLBuilder
        sb.AppendLine("</td>")
        _NextColumnNum += 1
        Return Me
    End Function

    ''' <summary>
    ''' Define columns in the active table
    ''' </summary>
    ''' <param name="Heading">Column heading text</param>
    ''' <param name="HeadingAlign">Alignment of heading text</param>
    ''' <param name="DataAlign">Alignment of data in column</param>
    ''' <param name="Width">Width of column</param>
    ''' <param name="WidthUnits">Units for width</param>
    ''' <param name="ForeColor">Heading text color</param>
    ''' <param name="BackColor">Heading cell background color</param>
    ''' <remarks>Column settings are saved in list and applied to individual cells because "col" tag apparently not supported)</remarks>
    Public Function AppendTableColumn(ByVal Heading As String, Optional ByVal HeadingAlign As enumAlign = enumAlign.Center, Optional ByVal DataAlign As enumAlign = enumAlign.Auto, Optional ByVal Width As Integer = 0, Optional ByVal WidthUnits As enumWidthUnits = enumWidthUnits.None, Optional ByVal ForeColor As enumColors = enumColors.white, Optional ByVal BackColor As enumColors = enumColors.gray) As clsHTMLBuilder
        'If _Columns.Count = 0 Then sb.AppendLine("<head>")
        Dim col As structColumn
        With col
            .Heading = Heading
            .Align = DataAlign
            .Width = Width
            .WidthUnits = WidthUnits
            If .Heading <> "" Then
                sb.AppendFormat("<th style='{0}' {1} {2} bgcolor='{5}'><font color='{4}'>{3}</font></th>", _CellPadding, AlignTag(HeadingAlign, .Heading), WidthTag(.Width, .WidthUnits), HTMLText(.Heading), HTMLColor(ForeColor), HTMLColor(BackColor))
                sb.AppendLine()
            End If
        End With
        _Columns.Add(col)
        Return Me
    End Function

    ''' <summary>
    ''' End prior table
    ''' </summary>
    Public Function AppendTableEnd() As clsHTMLBuilder
        sb.AppendLine("</tbody>")
        sb.AppendLine("</table>")
        Return Me
    End Function

    ''' <summary>
    ''' Append a new row to the active table
    ''' </summary>
    ''' <param name="cells">Text for each cell in row</param>
    Public Function AppendTableRow(ByVal ParamArray cells() As String) As clsHTMLBuilder
        sb.Append("<tr>")
        For i As Integer = 0 To cells.Length - 1
            With _Columns(i)
                sb.AppendFormat("<td {0} {1}>{2}</td>", AlignTag(.Align), WidthTag(.Width, .WidthUnits), HTMLText(cells(i)))
            End With
        Next
        sb.AppendLine("</tr>")
        Return Me
    End Function

    ''' <summary>
    ''' Start new row (must follow with AppendTableCell calls and then AppendTableRowEnd)
    ''' </summary>
    Public Function AppendTableRow() As clsHTMLBuilder
        _NextColumnNum = 0
        sb.AppendLine("<tr>")
        Return Me
    End Function

    Public Function AppendTableRowEmpty() As clsHTMLBuilder
        sb.AppendFormat("<tr><td colspan={0}>&nbsp;</td></tr>", _Columns.Count)
        sb.AppendLine()
        Return Me
    End Function

    ''' <summary>
    ''' Finish prior added row
    ''' </summary>
    Public Function AppendTableRowEnd() As clsHTMLBuilder
        sb.AppendLine("</tr>")
        Return Me
    End Function

    Private Function HTMLColor(ByVal Color As enumColors) As String
        Return _HTMLColor(Color)
    End Function

    Private Function HTMLText(ByVal Text As String) As String
        If Text = "" Then
            Return "&nbsp;"
        Else
            Return Text.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("""", "&quot;").Replace("'", "&apos;").Replace("\n", "<br>")
        End If
    End Function

    ''' <summary>
    ''' Return HTML text, encapsulated with appropriate tags
    ''' </summary>
    Public Overrides Function ToString() As String
        Return String.Format("<html>{0}<head>{0}</head>{0}<body>{0}{1}{0}</body>{0}</html>", vbCrLf, sb.ToString)
    End Function

    Private Function WidthTag(ByVal Width As Integer, ByVal WidthUnits As enumWidthUnits) As String
        Dim tag As String
        Select Case WidthUnits
            Case enumWidthUnits.None : Return ""
            Case enumWidthUnits.Percent : tag = "%"
            Case enumWidthUnits.Proportional : tag = "*"
            Case Else
                tag = ""
        End Select
        Return String.Format("width='{0}{1}'", Width, tag)
    End Function

    ''' <summary>
    ''' Save the HTML string to the specified file
    ''' </summary>
    ''' <param name="Filename">Name of file to save to; will be overwritten without warning; if blank, will create a uniquely named temporary file</param>
    Public Sub Save(Optional ByRef Filename As String = "")
        If String.IsNullOrEmpty(Filename) Then
            Filename = IO.Path.ChangeExtension(IO.Path.GetTempFileName(), ".htm")
        End If
        Dim sw As New IO.StreamWriter(Filename, False)
        sw.Write(ToString)
        sw.Close()
        sw.Dispose()
    End Sub
End Class
