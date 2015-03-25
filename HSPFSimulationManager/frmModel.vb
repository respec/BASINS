Imports atcUtility
Imports MapWinUtility

Public Class frmModel

    Private pIcon As clsIcon
    Friend Schematic As ctlSchematic

    Public Property ModelIcon() As clsIcon
        Set(ByVal value As clsIcon)
            pIcon = value

            txtName.Text = pIcon.WatershedName

            Dim lUciFileNames As String = ""
            For Each lUciFileName As String In value.UciFileNames
                cboUciFiles.Items.Add(lUciFileName)
                If lUciFileName = pIcon.UciFileName Then
                    cboUciFiles.SelectedIndex = cboUciFiles.Items.Count - 1
                End If
            Next

            cboDownstream.Items.Clear()
            cboDownstream.Items.Add("None")
            For Each lIcon In Schematic.AllIcons
                cboDownstream.Items.Add(lIcon.WatershedName)
            Next
            If pIcon.DownstreamIcon Is Nothing Then
                cboDownstream.SelectedIndex = 0
            Else
                Dim lIndex As Integer = cboDownstream.Items.IndexOf(pIcon.DownstreamIcon.WatershedName)
                If lIndex = -1 Then
                    lIndex = cboDownstream.Items.Count
                    cboDownstream.Items.Add(pIcon.DownstreamIcon.WatershedName)
                End If
                cboDownstream.SelectedIndex = lIndex
            End If
            btnImage.Text = pIcon.WatershedImageFilename
            btnImage.BackgroundImage = pIcon.WatershedImage
            btnImage.BackgroundImageLayout = ImageLayout.Zoom
        End Set
        Get
            pIcon.WatershedName = txtName.Text
            pIcon.UciFileName = cboUciFiles.Text

            pIcon.UciFileNames.Clear()
            For Each lUciFileName As String In cboUciFiles.Items
                pIcon.UciFileNames.Add(lUciFileName)
            Next

            Dim lNewDownstreamIcon As clsIcon
            Select Case cboDownstream.SelectedItem.Trim.ToLowerInvariant
                Case "", "none" 'No downstream model
                    lNewDownstreamIcon = Nothing
                Case Else
                    lNewDownstreamIcon = Schematic.AllIcons.FindOrAddIcon(cboDownstream.SelectedItem.Trim)
            End Select

            If pIcon.DownstreamIcon IsNot Nothing AndAlso pIcon.DownstreamIcon.UpstreamIcons.Contains(pIcon) Then
                'Remove old downstream icon connectivity
                pIcon.DownstreamIcon.UpstreamIcons.Remove(pIcon)
            End If
            pIcon.DownstreamIcon = lNewDownstreamIcon
            If lNewDownstreamIcon IsNot Nothing Then
                pIcon.DownstreamIcon.UpstreamIcons.Add(pIcon)
            End If
            pIcon.WatershedImageFilename = btnImage.Text
            pIcon.WatershedImage = btnImage.BackgroundImage

            Return pIcon
        End Get
    End Property

    Private Sub btnImage_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImage.Click
        Dim lFileName As String = String.Empty
        If frmHspfSimulationManager.BrowseOpen("Open Watershed Image File", "PNG Files|*.png|All Files|*.*", ".png", Me, lFileName) Then
            btnImage.Text = lFileName
            btnImage.BackgroundImage = Drawing.Image.FromFile(lFileName)
            Me.Height = btnImage.Height + 164
        End If
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

    Private Sub btnBrowseUCIFile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowseUCIFile.Click
        Dim lFileName As String = String.Empty
        If frmHspfSimulationManager.BrowseOpen("Open UCI File", "UCI Files|*.uci|All Files|*.*", ".uci", Me, lFileName) Then
            cboUciFiles.Items.Add(lFileName)
            cboUciFiles.SelectedIndex = cboUciFiles.Items.Count - 1
        End If
    End Sub

    Private Sub btnRemove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRemove.Click
        Try
            Dim lSelectedIndex As Integer = cboUciFiles.SelectedIndex
            If lSelectedIndex >= 0 Then
                cboUciFiles.Items.RemoveAt(lSelectedIndex)
                If lSelectedIndex >= cboUciFiles.Items.Count Then
                    lSelectedIndex -= 1
                End If
                If lSelectedIndex < 0 OrElse cboUciFiles.Items.Count = 0 Then
                    cboUciFiles.Text = ""
                Else
                    cboUciFiles.SelectedIndex = lSelectedIndex
                End If
            End If
        Catch
        End Try
    End Sub
End Class