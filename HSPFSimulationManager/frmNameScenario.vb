Public Class frmNameScenario
    Dim pScenario As clsUciScenario
    Dim ScenarioName As String = ""

    Public Function AskUser(aScenario As clsUciScenario, aAllScenarioNames As IEnumerable(Of String)) As Boolean
        pScenario = aScenario
        txtNewScenarioName.Text = aScenario.ScenarioName
        lstExistingScenarioNames.Items.AddRange(aAllScenarioNames.ToArray())
        Return Me.ShowDialog() = Windows.Forms.DialogResult.OK
    End Function

    Private Sub lstExistingScenarioNames_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lstExistingScenarioNames.SelectedIndexChanged
        ScenarioName = lstExistingScenarioNames.SelectedItem
        txtNewScenarioName.Text = ScenarioName
    End Sub

    Private Sub txtNewScenarioName_TextChanged(sender As Object, e As EventArgs) Handles txtNewScenarioName.TextChanged
        ScenarioName = txtNewScenarioName.Text
    End Sub

    Private Sub btnOk_Click(sender As Object, e As EventArgs) Handles btnOk.Click
        pScenario.ScenarioName = ScenarioName
        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub
End Class