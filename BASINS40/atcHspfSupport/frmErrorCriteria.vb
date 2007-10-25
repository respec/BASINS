Public Class frmErrorCriteria
    Private pErrorCriteria As ErrorCriteria

    Friend Sub Edit(ByVal aErrorCriteria As ErrorCriteria)
        pErrorCriteria = aErrorCriteria
        Dim lSource As New atcControls.atcGridSource
        lSource.FixedRows = 1
        lSource.FixedColumns = 1
        lSource.CellValue(0, 0) = "Name"
        lSource.CellValue(0, 1) = "Value"
        For lIndex As Integer = 1 To aErrorCriteria.Count
            lSource.CellValue(lIndex, 0) = aErrorCriteria(lIndex).Name
            lSource.CellValue(lIndex, 1) = aErrorCriteria(lIndex).Value
            lSource.CellEditable(lIndex, 1) = True
        Next
        atcGridCriterion.Initialize(lSource)
        atcGridCriterion.SizeAllColumnsToContents()
        'TODO:make non editable cells grey background
    End Sub

    Private Sub cmdOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOK.Click
        For lIndex As Integer = 1 To pErrorCriteria.Count
            pErrorCriteria(lIndex).Value = Me.atcGridCriterion.Source.CellValue(lIndex, 1)
        Next
    End Sub
End Class