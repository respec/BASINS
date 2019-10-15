Public Class frmOptions

    Friend List As atcListForm

    Private pSaveOptions As frmOptions

    ''' <summary>
    ''' Save the current state of this form in a private hidden copy of this form
    ''' </summary>
    ''' <remarks>Useful if the user presses Cancel so we can go back to the original options</remarks>
    Public Sub SaveState()
        pSaveOptions = New frmOptions
        atcUtility.CopyControlState(Me, pSaveOptions)
    End Sub

    Private Sub btnOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk.Click
        List.SetOptions(Me)
        Me.Close()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        If pSaveOptions IsNot Nothing Then List.SetOptions(pSaveOptions)
        Me.Close()
    End Sub

    Private Sub btnApply_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnApply.Click
        List.SetOptions(Me)
    End Sub

    Private Sub ShowHelpForListOptions()
        If System.Reflection.Assembly.GetEntryAssembly.Location.EndsWith("TimeseriesUtility.exe") Then
            atcUtility.ShowHelp("View\List.html")
        Else
            atcUtility.ShowHelp("BASINS Details\Analysis\Time Series Functions\List.html")
        End If
    End Sub

    Private Sub btnHelp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnHelp.Click
        ShowHelpForListOptions()
    End Sub
    Private Sub atcGraphForm_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyValue = System.Windows.Forms.Keys.F1 Then
            ShowHelpForListOptions()
        End If
    End Sub

End Class