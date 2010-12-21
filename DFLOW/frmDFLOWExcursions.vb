Imports System.Windows.Forms
Public Class frmDFLOWExcursions


    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        Me.Close()

    End Sub

    Private Sub btnCopy2Clipbrd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCopy2Clipbrd.Click
        Dim lBuffer As String
        lBuffer = Me.Text & vbCrLf
        Dim lRow As Integer
        Dim lCol As Integer
        With (AtcGrid1.Source)
            For lRow = 0 To .Rows - 1
                For lCol = 0 To .Columns - 2
                    lBuffer = lBuffer + .CellValue(lRow, lCol) + vbTab
                Next
                lBuffer = lBuffer + .CellValue(lRow, .Columns - 1) + vbCrLf
            Next
        End With
        clipboard.settext(lBuffer)

    End Sub
End Class