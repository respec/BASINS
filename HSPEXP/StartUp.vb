Imports MapWindow.Interfaces
Imports System.Windows.Forms.DialogResult
Imports System.IO
Public Class StartUp
    Private g_MapWin As IMapWin
    Private Sub cmdStart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdStart.Click
        'Try
        ScriptMain(g_MapWin)
        'Catch ex As Exception
        '    MapWinUtility.Logger.Msg(ex.Message)
        'End Try
    End Sub

    Private Sub cmdBrowse_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdBrowse.Click
        Dim lOpenDialog As New Windows.Forms.OpenFileDialog
        Dim lFileName As String
        Dim lPathName As String
        Dim lRunFolder As String
        Dim lPath() As String

        With lOpenDialog
            .Title = "Select UCI File from Current Calibration Run"
            .Filter = "UCI files|*.uci"
            .FilterIndex = 0
            .Multiselect = False
            If .ShowDialog = Windows.Forms.DialogResult.OK Then
                lFileName = .SafeFileName 'returns just file name and extension
                lPathName = Strings.Left(.FileName, Len(.FileName) - Len(lFileName)) 'gets path name from file name, removing file name and extension
                txtUCIPath.Text = lPathName
                'check for run number
                lPath = Split(lPathName, "\")
                lRunFolder = lPath(lPath.GetUpperBound(0) - 1) 'for some reason there is a blank in the top dimension
                If IsNumeric(Strings.Right(lRunFolder, Len(lRunFolder) - 3)) Then
                    'the user has run folders in the format Run##, where ## is the run number.      
                    'Fill out the run number and use it later to create a new run folder
                    txtRunNo.Text = CInt(Strings.Right(lRunFolder, Len(lRunFolder) - 3))
                    txtRootPath.Text = Strings.Left(lPathName, Strings.Len(lPathName) - Strings.Len(lRunFolder) - 1) 'take off run folder name & "\"
                End If
                Dim UCIFilePrefix As String = Strings.Left(lFileName, Len(lFileName) - 4)
                'takes the file name minus the 4 character extension
                txtPrefix.Text = UCIFilePrefix
                txtUCIPath.Visible = True
                txtPrefix.Visible = True
                txtRunNo.Visible = True
                txtRootPath.Visible = True
                txtRCH.Visible = True
                lblPrefix.Visible = True
                lblPrefixWarning.Visible = True
                lblRunNo.Visible = True
                lblRunNoInfo.Visible = True
                lblRootDirectory.Visible = True
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

    Private Sub chkConstituentReportChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkWaterBalance.CheckedChanged, chkSedimentBalance.CheckedChanged, chkNitrogenBalance.CheckedChanged, chkPhosphorusBalance.CheckedChanged, chkTotalNitrogen.CheckedChanged, chkTotalPhosphorus.CheckedChanged, chkBODBalance.CheckedChanged
        If chkWaterBalance.Checked OrElse chkSedimentBalance.Checked OrElse chkNitrogenBalance.Checked OrElse chkTotalNitrogen.Checked OrElse chkPhosphorusBalance.Checked OrElse chkTotalPhosphorus.Checked OrElse chkBODBalance.Checked Then
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

    Private Sub chkSupportingGraphs_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkGraphStandard.CheckedChanged, chkSupportingGraphs.CheckedChanged
        chkLogGraphs.Enabled = chkSupportingGraphs.Checked OrElse chkGraphStandard.Checked
        If Not chkLogGraphs.Enabled Then
            chkLogGraphs.Checked = False
        End If
    End Sub

    Private Sub chkExpertStats_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkExpertStats.CheckedChanged
        If chkExpertStats.Checked Then

            grpGraphs.Enabled = True
        Else
            chkAreaReports.Enabled = False
            grpGraphs.Enabled = False

            chkGraphStandard.Checked = False
            chkSupportingGraphs.Checked = False
        End If
    End Sub


    Private Sub chkMakeEXSFile_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkMakeEXSFile.CheckedChanged
        If chkMakeEXSFile.Checked = True Then
            MakeEXSFile.Show()
        End If

    End Sub
End Class
