Imports HTMLBuilder

Public Class Form1

    Private Sub Form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim hb As New clsHTMLBuilder
        With hb
            .AppendPara(clsHTMLBuilder.enumAlign.Left, "Now is the time")

            .AppendTable(, , clsHTMLBuilder.enumBorderStyle.solid, 2, clsHTMLBuilder.enumDividerStyle.All)

            For r As Integer = 1 To 2
                .AppendTableRow()
                For c As Integer = 1 To 3
                    .AppendTableCell("Col {0} Row {1}", c, r)
                Next
                .AppendTableRowEnd()
            Next
            .AppendTableEnd()

            .AppendPara(clsHTMLBuilder.enumAlign.Center, "ANOTHER SECTION")

            .AppendTable(50, clsHTMLBuilder.enumWidthUnits.None, clsHTMLBuilder.enumBorderStyle.dashed, 2, clsHTMLBuilder.enumDividerStyle.All)
            For c As Integer = 1 To 3
                .AppendTableColumn("Col " & c, clsHTMLBuilder.enumAlign.Center, clsHTMLBuilder.enumAlign.Center, c * 100, clsHTMLBuilder.enumWidthUnits.Pixels, clsHTMLBuilder.enumColors.maroon, clsHTMLBuilder.enumColors.teal)
            Next
            For r As Integer = 1 To 10
                .AppendTableRow("Col 1, Row " & r, "Col 2, Row " & r, "Col 3, Row " & r)
                If r = 4 Then .AppendTableRowEmpty()
            Next
            .AppendTableRow()
            .AppendTableCell.AppendFont(clsHTMLBuilder.enumFontSize.Large, clsHTMLBuilder.enumColors.lime).AppendPara(clsHTMLBuilder.enumAlign.Center, "Now is the time").AppendFontEnd.AppendTableCellEnd()
            .AppendTableEnd()

            .AppendImage("wilson.jpg", 100, clsHTMLBuilder.enumWidthUnits.Percent)
            Dim f As String = My.Computer.FileSystem.SpecialDirectories.MyPictures & "\Test.htm"
            .Save(f)
            WebBrowser1.Navigate(f)
            TextBox1.Text = hb.ToString
        End With
    End Sub
End Class
