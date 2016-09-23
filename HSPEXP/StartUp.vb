Imports System.Windows.Forms.DialogResult
Imports System.IO
Imports atcUtility
Imports MapWinUtility

Public Class StartUp

    Private pHspfMsg As atcUCI.HspfMsg
    Private pUci As atcUCI.HspfUci

    Private Sub cmdStart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdStart.Click

        If pUci Is Nothing OrElse pUci.Name <> cmbUCIPath.Text Then
            UciChanged()
        End If
        If pUci IsNot Nothing AndAlso IO.File.Exists(pUci.Name) Then
            ScriptMain(Nothing, pUci)
        Else
            Logger.Msg("The UCI file " & cmbUCIPath.Text & " does not exist", MsgBoxStyle.Critical, "HSPEXP+")
        End If

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
        chkRunHSPF.Enabled = lExists
        chkAreaReports.Enabled = lExists
        DateTimePicker1.Enabled = lExists
        DateTimePicker2.Enabled = lExists
        chkExpertStats.Enabled = lExists
        chkGraphStandard.Enabled = lExists
        chkWaterBalance.Enabled = lExists
        chkAdditionalgraphs.Enabled = lExists
        chkSedimentBalance.Enabled = lExists
        chkTotalNitrogen.Enabled = lExists
        chkTotalPhosphorus.Enabled = lExists
        chkBODBalance.Enabled = lExists
        txtRCH.Enabled = lExists
        lblRCH.Enabled = lExists
        lblOutReach2.Enabled = lExists
        pnlHighlight.Enabled = lExists
        cmdStart.Enabled = lExists


        Dim lUCI As String = cmbUCIPath.Text
        If IO.File.Exists(lUCI) Then
            'Logger.Status(Now & " Opening " & lUCI, True)
            Me.Cursor = Cursors.WaitCursor
            pUci = New atcUCI.HspfUci
            pUci.FastReadUciForStarter(pHspfMsg, lUCI)
            Me.Cursor = Cursors.Default
            Dim lSDateJ = pUci.GlobalBlock.SDateJ
            Dim lEDateJ = pUci.GlobalBlock.EdateJ


            DateTimePicker1.Value = System.DateTime.FromOADate(lSDateJ)
            DateTimePicker2.Value = System.DateTime.FromOADate(lEDateJ - 1)
        Else
            pUci = Nothing
        End If
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


    Private Sub StartUp_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim WinHspfLtDir As String = PathNameOnly(Reflection.Assembly.GetEntryAssembly.Location) & g_PathChar & "WinHSPFLt"
        Logger.Dbg("Located WinHSPFLt at " & WinHspfLtDir)
        Try
            'Set Environmental Variable
            Dim lEnvPath As String = Environment.GetEnvironmentVariable("PATH")
            If Not lEnvPath.ToLowerInvariant.Contains(WinHspfLtDir.ToLowerInvariant) Then
                System.Environment.SetEnvironmentVariable("PATH", WinHspfLtDir & ";" & lEnvPath)
            End If
        Catch exSetEnv As Exception
            Logger.Dbg("Could not add WinHspfLtDir to PATH " & exSetEnv.Message)
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

        Logger.Dbg(Now & " Attempting to open hspfmsg.wdm")
        pHspfMsg = New atcUCI.HspfMsg
        pHspfMsg.Open(atcWDM.atcDataSourceWDM.HSPFMsgFilename) 'Becky: this can be found at C:\BASINS\models\HSPF\bin if you did the typical BASINS install


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
        UciChanged()
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

    Private Sub cmbUCIPath_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbUCIPath.SelectedIndexChanged
        UciChanged()
    End Sub

    Private Sub cmbUCIPath_TextChanged(sender As Object, e As EventArgs) Handles cmbUCIPath.TextChanged
        UciChanged()
    End Sub
End Class
