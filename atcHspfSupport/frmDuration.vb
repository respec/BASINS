Imports atcData
Imports atcUtility

Public Class frmDuration
    Private pTSer As atcTimeseries

    Private Sub btnDuration_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDuration.Click
        Dim cdlg As New Windows.Forms.SaveFileDialog
        With cdlg
            .Title = "Save Duration Report As..."
            .DefaultExt = "txt"
            .Filter = "Text Files|*.txt|All Files|*.*"
            .FilterIndex = 0
            If .ShowDialog = Windows.Forms.DialogResult.OK Then
                Dim lDataGroup As atcDataGroup = DateChooser.CreateSelectedDataGroupSubset

                'Dim lTU As atcTimeUnit
                'If radioTUDaily.Checked Then
                '    lTU = atcTimeUnit.TUDay
                'ElseIf radioTUMonthly.Checked Then
                '    lTU = atcTimeUnit.TUMonth
                'Else
                '    lTU = atcTimeUnit.TUYear
                'End If

                Dim lTran As atcTran = atcTran.TranAverSame

                pTSer = Aggregate(lDataGroup(0), atcTimeUnit.TUDay, 1, lTran)
                'pSimulated = Aggregate(lDataGroup(1), lTU, 1, lTran)
                IO.File.WriteAllText(.FileName, DurationStats(pTSer, GetClassLimits(pTSer)))
                OpenFile(.FileName)
            End If
        End With

    End Sub

    Private Sub btnTSDir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTSDir.Click
        Dim lHad As New atcDataGroup
        If pTSer IsNot Nothing Then lHad.Add(pTSer)
        Dim lUserSelected As atcDataGroup = atcDataManager.UserSelectData("Select Dataset", lHad)
        If lUserSelected.Count > 0 Then
            pTSer = lUserSelected(0)
            txtTSDir.Text = pTSer.ToString
        Else
            pTSer = Nothing
            txtTSDir.Text = ""
        End If
        SetDates()

    End Sub

    Private Sub TextBox1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtTSDir.TextChanged
        btnTSDir_Click(sender, e)
    End Sub

    Private Sub SetDates()
        Dim lDataGroup As New atcDataGroup
        If pTSer IsNot Nothing Then lDataGroup.Add(pTSer)
        DateChooser.DataGroup = lDataGroup
    End Sub

End Class