Public Class frmWDM

    Public Function AskUser(ByVal aIcon As Drawing.Icon, ByVal aType As String, ByVal aSaveFolder As String) As String
        Dim lSaveDir As String = CurDir()
        atcUtility.ChDriveDir(aSaveFolder)
        Me.Icon = aIcon
        Me.Text = aType & Me.Text
        lblMessage.Text &= aType & " data,"

        Dim lFilename As String = IO.Path.Combine(aSaveFolder, aType.ToLower & ".wdm")
        If IO.File.Exists(lFilename) Then
            txtFilenameExisting.Text = lFilename
            txtFilenameNew.Text = atcUtility.GetTemporaryFileName(IO.Path.Combine(aSaveFolder, aType.ToLower), "wdm")
        Else
            txtFilenameNew.Text = lFilename
        End If

        If Me.ShowDialog() = Windows.Forms.DialogResult.OK Then
            atcUtility.ChDriveDir(lSaveDir)
            If RadioIndividual.Checked Then
                Return ""
            ElseIf RadioAddNew.Checked Then
                Return "<SaveWDM>" & txtFilenameNew.Text & "</SaveWDM>" & vbCrLf
            ElseIf RadioAddExisting.Checked Then
                atcData.atcDataManager.RemoveDataSource(txtFilenameExisting.Text)
                Return "<SaveWDM>" & txtFilenameExisting.Text & "</SaveWDM>" & vbCrLf
            ElseIf RadioDontAdd.Checked Then
                Return "<CacheOnly>True</CacheOnly>"
            End If
        End If
        Return Nothing
    End Function

    Private Sub btnBrowseNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowseNew.Click
        Dim lDialog As New Windows.Forms.SaveFileDialog
        With lDialog
            .Title = "Save new WDM file as..."
            .FileName = txtFilenameNew.Text
            .Filter = "WDM files|*.wdm"
            .DefaultExt = ".wdm"
            .CheckFileExists = False
            .OverwritePrompt = False
            .ValidateNames = True
            .CreatePrompt = False
            If .ShowDialog = Windows.Forms.DialogResult.OK Then
                txtFilenameNew.Text = .FileName
            End If
        End With
    End Sub

    Private Sub btnBrowseExisting_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowseExisting.Click
        Dim lDialog As New Windows.Forms.OpenFileDialog
        With lDialog
            .Title = "Save data in..."
            .FileName = txtFilenameExisting.Text
            .Filter = "WDM files|*.wdm"
            .DefaultExt = ".wdm"
            .CheckFileExists = False
            .ValidateNames = True
            If .ShowDialog = Windows.Forms.DialogResult.OK Then
                txtFilenameExisting.Text = .FileName
            End If
        End With
    End Sub

    Private Sub txtFilenameNew_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtFilenameNew.TextChanged
        RadioAddNew.Checked = True
    End Sub

    Private Sub txtFilenameExisting_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtFilenameExisting.TextChanged
        RadioAddExisting.Checked = True
    End Sub
End Class