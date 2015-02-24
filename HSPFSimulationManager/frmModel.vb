Imports atcUtility
Imports MapWinUtility

Public Class frmModel

    Private pIcon As clsIcon
    Friend Schematic As ctlSchematic

    Public Property ModelIcon() As clsIcon
        Set(ByVal value As clsIcon)
            pIcon = value
            txtUCI.Text = pIcon.UciFileName
            txtName.Text = pIcon.Label
            txtDownstream.Text = ""
            If pIcon.DownstreamIcon IsNot Nothing Then
                txtDownstream.Text = pIcon.DownstreamIcon.UciFileName
            End If
            btnImage.Text = pIcon.WatershedImageFilename
            btnImage.BackgroundImage = pIcon.OrigImage
            btnImage.BackgroundImageLayout = ImageLayout.Zoom
        End Set
        Get
            pIcon.UciFileName = txtUCI.Text
            pIcon.Label = txtName.Text

            Dim lNewDownstreamIcon As clsIcon
            Select Case txtDownstream.Text.Trim.ToLowerInvariant
                Case "", "none" 'No downstream model
                    lNewDownstreamIcon = Nothing
                Case Else
                    lNewDownstreamIcon = Schematic.AllIcons.FindOrAddIcon(txtDownstream.Text.Trim)
            End Select

            If pIcon.DownstreamIcon IsNot Nothing AndAlso pIcon.DownstreamIcon.UpstreamIcons.Contains(pIcon) Then
                'Remove old downstream icon connectivity
                pIcon.DownstreamIcon.UpstreamIcons.Remove(pIcon)
            End If
            pIcon.DownstreamIcon = lNewDownstreamIcon
            pIcon.DownstreamIcon.UpstreamIcons.Add(pIcon)

            pIcon.WatershedImageFilename = btnImage.Text
            pIcon.OrigImage = btnImage.BackgroundImage

            Return pIcon
        End Get
    End Property

    Private Sub btnImage_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImage.Click
        Dim lFileDialog As New Windows.Forms.OpenFileDialog()
        With lFileDialog
            .Title = "Open Watershed Image File"
            .Filter = "PNG Files|*.png|All Files|*.*"
            .FilterIndex = 0
            .CheckFileExists = True
            If .ShowDialog(Me) = DialogResult.OK Then
                btnImage.Text = .FileName
                btnImage.BackgroundImage = Drawing.Image.FromFile(.FileName)
                Me.Height = btnImage.Height + 164
            End If
        End With
    End Sub

    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
        pIcon = Me.ModelIcon 'Apply changes to icon
        If Not Schematic.AllIcons.Contains(pIcon) Then
            'Make sure this icon is on the schematic
            Schematic.AllIcons.Add(pIcon)
        End If
        Schematic.BuildTree(Schematic.AllIcons)
        Me.Close()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub btnWinHSPF_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnWinHSPF.Click
        RunUCI("WinHSPF.exe", txtUCI.Text)
    End Sub

    Private Sub btnRunHSPF_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRunHSPF.Click
        RunUCI("WinHSPFlt.exe", txtUCI.Text)
    End Sub

    Private Sub RunUCI(ByVal aExeName As String, ByVal aUCIFilename As String)
        Dim lHspfExe As String = atcUtility.FindFile("Please locate " & aExeName, aExeName)
        If Not IO.File.Exists(lHspfExe) OrElse Not lHspfExe.ToLower.EndsWith(aExeName.ToLower) Then
            lHspfExe = atcUtility.FindFile("Please locate " & aExeName, aExeName, , , True)
            If Not IO.File.Exists(lHspfExe) OrElse Not lHspfExe.ToLower.EndsWith(aExeName.ToLower) Then
                Logger.Msg("Unable to locate " & aExeName & ", not running.", aExeName)
                Exit Sub
            End If
        End If

        Logger.Status("Running " & aUCIFilename & " (" & lHspfExe & ")")
        MapWinUtility.LaunchProgram(lHspfExe, IO.Path.GetDirectoryName(aUCIFilename), aUCIFilename)
    End Sub
End Class