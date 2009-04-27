Public Class frmSelectR
    Private Structure structR
        Dim Min As Double
        Dim Max As Double
        Dim Avg As Double
    End Structure

    Private dictState As New Generic.SortedDictionary(Of String, Generic.SortedDictionary(Of String, structR))

    Private Sub frmSelectR_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim arLine() As String = My.Resources.R_Factors.Replace(vbCrLf, vbCr).Split(vbCr)
        For r As Integer = 1 To arLine.Length - 1 'skip header line
            If arLine(r).Trim = "" Then Exit For
            Dim arItem() As String = arLine(r).Split(vbTab)
            Dim dictItem As Generic.SortedDictionary(Of String, structR) = Nothing
            If Not dictState.TryGetValue(arItem(0), dictItem) Then
                dictItem = New Generic.SortedDictionary(Of String, structR)
                dictState.Add(arItem(0), dictItem)
            End If
            Dim R_Factor As structR
            With R_Factor
                .Min = arItem(2)
                .Avg = arItem(3)
                .Max = arItem(4)
            End With
            dictItem.Add(arItem(1), R_Factor)
        Next
        For Each s As String In dictState.Keys
            cboState.Items.Add(s)
        Next
        cboState.SelectedIndex = 0
    End Sub

    Private Sub cboState_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboState.SelectedIndexChanged
        cboCounty.Items.Clear()
        For Each s As String In dictState(cboState.Text).Keys
            cboCounty.Items.Add(s)
        Next
        cboCounty.SelectedIndex = 0
    End Sub

    Private Sub cboCounty_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboCounty.SelectedIndexChanged
        With dictState(cboState.Text)(cboCounty.Text)
            txtAvg.Text = .Avg
            txtMin.Text = .Min
            txtMax.Text = .Max
        End With
    End Sub

    Private Sub btn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click, btnCancel.Click
        Close()
    End Sub
End Class