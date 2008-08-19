Imports atcData
Imports atcUtility

Public Class frmCompare

    Private pObserved As atcTimeseries
    Private pSimulated As atcTimeseries

    Private Sub btnCompare_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCompare.Click
        Dim cdlg As New Windows.Forms.SaveFileDialog
        With cdlg
            .Title = "Save Comparison Report As..."
            .DefaultExt = "txt"
            .Filter = "Text Files|*.txt|All Files|*.*"
            .FilterIndex = 0
            If .ShowDialog = Windows.Forms.DialogResult.OK Then
                Dim lDataGroup As atcDataGroup = DateChooser.CreateSelectedDataGroupSubset

                Dim lTU As atcTimeUnit
                If radioTUDaily.Checked Then
                    lTU = atcTimeUnit.TUDay
                ElseIf radioTUMonthly.Checked Then
                    lTU = atcTimeUnit.TUMonth
                Else
                    lTU = atcTimeUnit.TUYear
                End If

                Dim lTran As atcTran = atcTran.TranAverSame

                pObserved = Aggregate(lDataGroup(0), lTU, 1, lTran)
                pSimulated = Aggregate(lDataGroup(1), lTU, 1, lTran)
                IO.File.WriteAllText(.FileName, CompareStats(pObserved, pSimulated, GetClassLimits(pObserved)))
                OpenFile(.FileName)
            End If
        End With
    End Sub

    Private Sub btnObserved_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnObserved.Click
        Dim lHad As New atcDataGroup
        If pObserved IsNot Nothing Then lHad.Add(pObserved)
        Dim lUserSelected As atcDataGroup = atcDataManager.UserSelectData("Select Observed Dataset", lHad)
        If lUserSelected.Count > 0 Then
            pObserved = lUserSelected(0)
            txtObserved.Text = pObserved.ToString
        Else
            pObserved = Nothing
            txtObserved.Text = ""
        End If
        SetDates()
    End Sub

    Private Sub btnSimulated_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSimulated.Click
        Dim lHad As New atcDataGroup
        If pSimulated IsNot Nothing Then lHad.Add(pSimulated)
        Dim lUserSelected As atcDataGroup = atcDataManager.UserSelectData("Select Simulated Dataset", lHad)
        If lUserSelected.Count > 0 Then
            pSimulated = lUserSelected(0)
            txtSimulated.Text = pSimulated.ToString
        Else
            pSimulated = Nothing
            txtSimulated.Text = ""
        End If
        SetDates()
    End Sub

    Private Sub txtObserved_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtObserved.Click
        btnObserved_Click(sender, e)
    End Sub

    Private Sub txtSimulated_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtSimulated.Click
        btnSimulated_Click(sender, e)
    End Sub

    Private Sub SetDates()
        Dim lDataGroup As New atcDataGroup
        If pObserved IsNot Nothing Then lDataGroup.Add(pObserved)
        If pSimulated IsNot Nothing Then lDataGroup.Add(pSimulated)

        DateChooser.DataGroup = lDataGroup
    End Sub
End Class