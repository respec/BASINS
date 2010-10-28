Public Class frmWDM

    Public Function AskUser(ByVal aIcon As Drawing.Icon, ByVal aType As String, ByVal aSaveFolder As String, Optional ByVal aWindowTitle As String = Nothing) As String
        Dim lSaveDir As String = CurDir()
        atcUtility.ChDriveDir(aSaveFolder)
        Me.Icon = aIcon
        If aWindowTitle Is Nothing Then
            Me.Text = aType & Me.Text
        Else
            Me.Text = aWindowTitle
        End If
        lblMessage.Text &= aType & " data,"

        Dim lFilename As String = IO.Path.Combine(aSaveFolder, aType.ToLower & ".wdm")
        If IO.File.Exists(lFilename) Then
            txtFilenameNew.Text = atcUtility.GetTemporaryFileName(IO.Path.Combine(aSaveFolder, aType.ToLower), "wdm")
            txtFilenameExisting.Text = lFilename
            RadioAddExisting.Checked = True
        Else
            txtFilenameNew.Text = lFilename
            Dim lExistingWdmFiles() As String = IO.Directory.GetFiles(aSaveFolder, "*.wdm")
            If lExistingWdmFiles.Length > 0 Then
                txtFilenameExisting.Text = lExistingWdmFiles(0)
                RadioAddExisting.Checked = True
            Else
                txtFilenameExisting.Text = ""
                RadioAddNew.Checked = True
            End If
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
        If IO.File.Exists(txtFilenameExisting.Text) Then RadioAddExisting.Checked = True
    End Sub

    Private Sub frmWDM_DragDrop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles Me.DragDrop
        Me.Activate()
        If e.Data.GetDataPresent(Windows.Forms.DataFormats.FileDrop) AndAlso IsWdmFileName(e.Data.GetData(Windows.Forms.DataFormats.FileDrop)) Then
            txtFilenameExisting.Text = e.Data.GetData(Windows.Forms.DataFormats.FileDrop)(0)
            RadioAddExisting.Checked = True
        End If
    End Sub

    Private Function IsWdmFileName(ByVal aFilenames() As String) As Boolean
        Return aFilenames.Length = 1 AndAlso IO.Path.GetExtension(aFilenames(0)).ToLower = ".wdm"
    End Function

    Private Sub frmWDM_DragEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles Me.DragEnter
        If e.Data.GetDataPresent(Windows.Forms.DataFormats.FileDrop) AndAlso IsWdmFileName(e.Data.GetData(Windows.Forms.DataFormats.FileDrop)) Then
            e.Effect = Windows.Forms.DragDropEffects.All
        End If
    End Sub
End Class