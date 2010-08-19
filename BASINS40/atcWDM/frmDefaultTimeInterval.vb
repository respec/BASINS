Public Class frmDefaultTimeInterval
    Private pTs As Integer
    Private pTu As Integer
    Private pAggr As Integer '0 aver, 1 sum, 2 min, 3 max, 4 first, 5 last
    Private pResult As Boolean

    Public Function AskUser(ByVal aDataSetDescription As String, ByRef aTu As Integer, ByRef aTs As Integer, ByRef aAggr As Integer) As Boolean
        If aTs > 0 Then atcTextTimeStep.ValueInteger = aTs
        If aTu > 0 Then
            lstTimeUnits.SelectedIndex = aTu - 2
        Else
            lstTimeUnits.SelectedIndex = 0 'minutes
        End If
        If aAggr >= 0 Then
            lstAggregation.SelectedIndex = aAggr
        Else
            lstAggregation.SelectedIndex = 0
        End If

        lblInstructions.Text &= aDataSetDescription

        Me.ShowDialog()

        aTu = pTu
        aTs = pTs

        Return pResult
    End Function

    Private Sub btnAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAll.Click
        pTu = lstTimeUnits.SelectedIndex + 2 ' minutes = 2, hours = 3, days = 4, months = 5
        If pTu = 1 Then 'nothing was selected
            pTu = 0
        End If
        pTs = atcTextTimeStep.ValueInteger
        pAggr = lstAggregation.SelectedIndex
        pResult = False  'dont ask
        Me.Close()
    End Sub

    Private Sub btnOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk.Click
        pTu = lstTimeUnits.SelectedIndex + 2 ' minutes = 2, hours = 3, days = 4, months = 5
        If pTu = 1 Then 'nothing was selected
            pTu = 0
        End If
        pTs = atcTextTimeStep.ValueInteger
        pAggr = lstAggregation.SelectedIndex
        pResult = True 'ask next time
        Me.Close()
    End Sub

    Private Sub btnSkip_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSkip.Click
        pTu = 0
        pTs = 0
        pAggr = lstAggregation.SelectedIndex
        pResult = True 'ask next time
        Me.Close()
    End Sub

    Private Sub btnSkipAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSkipAll.Click
        pTu = 0
        pTs = 0
        pAggr = lstAggregation.SelectedIndex
        pResult = False  'dont ask next time
        Me.Close()
    End Sub

    Private Sub frmDefaultTimeInterval_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyValue = Windows.Forms.Keys.F1 Then
            'TODO: link to documentation
            'atcUtility.ShowHelp("BASINS Details\?.html")
        End If
    End Sub
End Class