Imports System.Windows.Forms.DialogResult
Imports System.IO
Imports atcUtility
Imports MapWinUtility

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
                cmbUCIPath.Text = .FileName ' lPathName
                UciChanged()
                SaveSetting("HSPEXP+", "Defaults", "UCI", .FileName)
            End If
        End With
        Me.Focus()
    End Sub

    Sub UciChanged()
        Dim lExists As Boolean = IO.File.Exists(cmbUCIPath.Text)
        'cmbUCIPath.Visible = lExists
        txtRCH.Visible = lExists
        lblRCH.Visible = lExists
        lblOutReach2.Visible = lExists
        pnlHighlight.Visible = lExists
        cmdStart.Enabled = lExists
    End Sub

    Private Sub cmdEnd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdEnd.Click
        Me.Close()
        Call Application.Exit()
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
        If cmbUCIPath.Text.Length > 0 Then
            MakeEXSFile.Show()
            MakeEXSFile.lblUCIFileName.Text = cmbUCIPath.Text
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

        Dim WinHspfLtDir As String = PathNameOnly(Reflection.Assembly.GetEntryAssembly.Location) & g_PathChar & "WinHSPFLt"
        Logger.Dbg("Located WinHSPFLt at " & WinHspfLtDir)
        Try
            'Set Environmental Variable
            System.Environment.SetEnvironmentVariable("PATH", WinHspfLtDir & ";" & Environment.GetEnvironmentVariable("PATH"))
        Catch
        End Try
        Try
            Dim lHassentPath As String = IO.Path.Combine(WinHspfLtDir, "hass_ent.dll")
            If LoadLibraryEx(lHassentPath, IntPtr.Zero, LoadLibraryFlags.LOAD_WITH_ALTERED_SEARCH_PATH) = 0 Then
                Logger.Msg("Missing HSPF Library at install location:" & vbCrLf & lHassentPath)
                End
            End If
        Catch ex As Exception
        End Try

        HSPFOutputReports.pHSPFExe = IO.Path.Combine(WinHspfLtDir, "WinHspfLt.exe")
        atcWDM.atcDataSourceWDM.HSPFMsgFilename = IO.Path.Combine(WinHspfLtDir, "hspfmsg.wdm")

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
        Dim lUCI As String = GetSetting("HSPEXP+", "Defaults", "UCI", "")
        If IO.File.Exists(lUCI) Then
            cmbUCIPath.Items.Add(lUCI)
            cmbUCIPath.SelectedIndex = 0
        End If
        UciChanged
    End Sub

    Private Sub btn_help_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_help.Click
        Help.ShowHelp(Me, Application.StartupPath & "\HSPEXP+.chm")
    End Sub

    Private Sub chkHydrologySensitivity_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkHydrologySensitivity.CheckedChanged

    End Sub

    ' Reset all the controls to the user's default Control color.  
    Private Sub ResetAllControlsBackColor(ByVal control As Control)
        control.BackColor = SystemColors.Control
        control.ForeColor = SystemColors.ControlText
        If control.HasChildren Then
            ' Recursively call this method for each child control. 
            Dim childControl As Control
            For Each childControl In control.Controls
                ResetAllControlsBackColor(childControl)
            Next childControl
        End If
    End Sub

    Private Sub cmbUCIPath_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cmbUCIPath.SelectedIndexChanged

    End Sub
End Class
