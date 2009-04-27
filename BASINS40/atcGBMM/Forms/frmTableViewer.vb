Public Class frmTableViewer

    Private Sub frmTableViewer_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        SaveWindowPos(REGAPP, Me)
    End Sub

    Private Sub frmTableViewer_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        GetWindowPos(REGAPP, Me)
    End Sub

    'Private Sub dgTable_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles dgTable.DataError
    '    With dgTable.Item(e.ColumnIndex, e.RowIndex)
    '        '.Value = "ERROR!"

    '        e.Cancel = True
    '    End With
    'End Sub

    Private Sub dgTable_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles dgTable.CellFormatting
        Dim dg As DataGridView = sender
        'If Project.DB.DatabaseType = clsDB.enumDBType.SQL_Server Then 'for some reason, does not obey datagridview nullvalue setting
        '    If dg.Item(e.ColumnIndex, e.RowIndex).Value.isnull Then e.Value = "<Null>" : e.FormattingApplied = True
        'End If
        If dg.Columns(e.ColumnIndex).GetType Is GetType(DataGridViewLinkColumn) Then
            Dim blobcol As Integer = -1
            For i As Integer = 0 To dg.Columns.Count - 1
                If dg.Columns(i).ValueType Is GetType(Byte()) Then
                    blobcol = i
                End If
            Next
            If blobcol <> -1 Then
                If Not IsDBNull(dg.Rows(e.RowIndex).Cells(blobcol)) Then
                    e.Value = "BLOB NOT NULL"
                    e.FormattingApplied = True
                End If
            End If
        End If
    End Sub
End Class