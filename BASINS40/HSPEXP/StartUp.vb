Imports MapWindow.Interfaces
Public Class StartUp
    Private g_MapWin As IMapWin
    Private Sub cmdStart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdStart.Click
        ScriptMain(g_MapWin)
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
                txtPrefix.Text = Strings.Left(lFileName, Len(lFileName) - 4) 'takes the file name minus the 4 character extension
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
            End If
        End With
        Me.Focus()
    End Sub

    Private Sub txtRCH_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtRCH.TextChanged
        Dim lOK As Boolean = False
        cmdStart.Enabled = False
        If IsNumeric(txtRCH.Text) Then
            'one reach, entered as numeric
            lOK = True
        ElseIf InStr(1, txtRCH.Text, ",") > 0 Then
            'multiple reaches separated by commas
            Dim lRCHRES() As String
            lRCHRES = Split(txtRCH.Text, ",")
            For Each lRCH As String In lRCHRES
                lOK = True
                If Not (IsNumeric(lRCH)) Then
                    'one or more of the entries is not numeric, do not allow start button
                    lOK = False
                    Exit For
                End If
            Next
        Else
            lOK = False
            txtRCH.Clear()
        End If
        If lOK Then cmdStart.Enabled = True
    End Sub

    Private Sub cmdEnd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdEnd.Click
        Me.Close()
    End Sub

    Private Sub chkGraphs_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkGraphs.CheckedChanged
        If chkGraphs.Checked Then
            chkGraphStandard.Checked = True
            chkGraphStandard.Visible = True
            chkLogGraphs.Visible = True
            chkSupportingGraphs.Checked = True
            chkSupportingGraphs.Visible = True
        Else 'user unchecked
            chkGraphStandard.Checked = False
            chkLogGraphs.Checked = False
            chkSupportingGraphs.Checked = False
        End If
    End Sub
 
    Private Sub chkLogGraphs_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkLogGraphs.CheckedChanged
        If chkLogGraphs.Checked Then
            If Not chkGraphStandard.Checked And Not chkSupportingGraphs.Checked Then
                chkLogGraphs.Checked = False
            End If
        End If
    End Sub
End Class
