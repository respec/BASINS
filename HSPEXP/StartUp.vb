Imports System.Windows.Forms.DialogResult
Imports System.IO
Public Class StartUp
    Private Sub cmdStart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdStart.Click
        ScriptMain(Nothing)
    End Sub

    Private Sub cmdBrowse_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdBrowse.Click
        Dim lOpenDialog As New Windows.Forms.OpenFileDialog

        With lOpenDialog
            .Title = "Select UCI File from Current Calibration Run"
            .Filter = "UCI files|*.uci"
            .FilterIndex = 0
            .Multiselect = False
            If .ShowDialog = Windows.Forms.DialogResult.OK Then
                txtUCIPath.Text = .FileName ' lPathName
                txtUCIPath.Visible = True
                txtRCH.Visible = True
                lblRCH.Visible = True
                lblOutReach2.Visible = True
                pnlHighlight.Visible = True
                cmdStart.Enabled = True
            End If
        End With
        Me.Focus()
    End Sub

    Private Sub cmdEnd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdEnd.Click
        Me.Close()
    End Sub

    Private Sub chkRunHSPF_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkRunHSPF.CheckedChanged
        
    End Sub

    Private Sub chkConstituentReportChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkWaterBalance.CheckedChanged, chkSedimentBalance.CheckedChanged, chkTotalNitrogen.CheckedChanged, chkTotalPhosphorus.CheckedChanged, chkBODBalance.CheckedChanged, chkFecalColiform.CheckedChanged
        If chkWaterBalance.Checked OrElse chkSedimentBalance.Checked OrElse chkTotalNitrogen.Checked OrElse chkTotalPhosphorus.Checked OrElse chkBODBalance.Checked OrElse chkFecalColiform.Checked Then
            lblRCH.Enabled = True
            lblOutReach2.Enabled = True
            txtRCH.Enabled = True
            pnlHighlight.Enabled = True
        Else
            lblRCH.Enabled = False
            lblOutReach2.Enabled = False
            txtRCH.Enabled = False
            pnlHighlight.Enabled = False
        End If

    End Sub

    Private Sub chkExpertStats_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkExpertStats.CheckedChanged
        If chkExpertStats.Checked Then
            chkGraphStandard.Checked = True
        Else
            chkGraphStandard.Checked = False

        End If
    End Sub


    Private Sub btnMakeEXSFile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMakeEXSFile.Click
        If txtUCIPath.Text.Length > 0 Then
            MakeEXSFile.Show()
            MakeEXSFile.lblUCIFileName.Text = txtUCIPath.Text
            Dim lWDM As New atcWDM.atcDataSourceWDM
            atcData.atcDataManager.OpenDataSource(lWDM, IO.Path.ChangeExtension(MakeEXSFile.lblUCIFileName.Text, ".wdm"), Nothing)
            Dim lSIMQ As atcData.atcTimeseriesGroup = lWDM.DataSets.FindData("Constituent", "SIMQ")
            With MakeEXSFile.lstBOXWDM
                For Each lTs As atcData.atcTimeseries In lSIMQ
                    .Items.Add(lTs.Attributes.GetValue("Location"))
                Next
            End With
        End If
    End Sub

    Private Sub StartUp_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        atcData.atcDataManager.Clear()
        With atcData.atcDataManager.DataPlugins
            .Add(New atcHspfBinOut.atcTimeseriesFileHspfBinOut)
            .Add(New atcBasinsObsWQ.atcDataSourceBasinsObsWQ)
            .Add(New atcWDM.atcDataSourceWDM)
            '.Add(New atcTimeseriesNCDC.atcTimeseriesNCDC)
            '.Add(New atcTimeseriesRDB.atcTimeseriesRDB)
            '.Add(New atcTimeseriesScript.atcTimeseriesScriptPlugin)
            '.Add(New atcTimeseriesSUSTAIN.atcTimeseriesSUSTAIN)

            '.Add(New atcList.atcListPlugin)
            .Add(New atcGraph.atcGraphPlugin)
            '.Add(New atcDataTree.atcDataTreePlugin)
        End With

    End Sub

    Private Sub btn_help_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_help.Click
        Help.ShowHelp(Me, Application.StartupPath & "\HSPEXP+.chm")
    End Sub

    Private Sub chkHydrologySensitivity_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkHydrologySensitivity.CheckedChanged

    End Sub

   
End Class
