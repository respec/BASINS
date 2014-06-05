'Imports MapWindow.Interfaces
Imports System.Windows.Forms.DialogResult
Imports System.IO
Public Class MakeEXSFile
    'Private g_MapWin As Object
    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        WriteEXSFileMain()
        Me.Close()
    End Sub

    Private Sub cmbNumberOfSites_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbNumberOfSites.SelectedIndexChanged
        cmbNumberOfSites.Items.Add("1")
        cmbNumberOfSites.Items.Add("2")
        cmbNumberOfSites.Items.Add("3")
        cmbNumberOfSites.Items.Add("4")
        cmbNumberOfSites.Items.Add("5")

        Dim NumberOfSites As Integer
        NumberOfSites = cmbNumberOfSites.SelectedIndex.ToString
        If NumberOfSites > 1 Then
            lblReachNumber2.Visible = True
        End If

    End Sub

    Private Sub pLocations_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles pLocations.Paint


    End Sub

    Private Sub txtUCIFilename_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub chkAnalysisPeriod_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkAnalysisPeriod.CheckedChanged
        If chkAnalysisPeriod.Checked Then
            DTStartDate.Enabled = True
            DTEndDate.Enabled = True

        End If

    End Sub

    
    Private Sub lstBOXWDM_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstBOXWDM.SelectedIndexChanged

    End Sub
End Class