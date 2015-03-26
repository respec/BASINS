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

            Dim lNewDownstreamIcon As clsIcon = CurrentlySelectedDownstreamIcon()

            If pIcon.DownstreamIcon IsNot Nothing AndAlso pIcon.DownstreamIcon.UpstreamIcons.Contains(pIcon) Then
                'Remove old downstream icon connectivity
                pIcon.DownstreamIcon.UpstreamIcons.Remove(pIcon)
            End If
            pIcon.DownstreamIcon = lNewDownstreamIcon
            If lNewDownstreamIcon IsNot Nothing Then
                lNewDownstreamIcon.UpstreamIcons.Add(pIcon)
            End If
            pIcon.WatershedImageFilename = btnImage.Text
            pIcon.WatershedImage = btnImage.BackgroundImage

            Return pIcon
        End Get
    End Property

    Private Function CurrentlySelectedDownstreamIcon() As clsIcon
        Select Case cboDownstream.SelectedItem.Trim.ToLowerInvariant
            Case "", "none" 'No downstream model
                Return Nothing
            Case Else
                Return Schematic.AllIcons.FindOrAddIcon(cboDownstream.SelectedItem.Trim)
        End Select
    End Function

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

    Private Sub btnConnectionReport_Click(sender As Object, e As EventArgs) Handles btnConnectionReport.Click
        Me.Enabled = False
        Me.Cursor = Cursors.WaitCursor
        Dim lReport As String = frmHspfSimulationManager.g_AppNameLong & " Connection Report" & vbCrLf
        Dim lReportLine As String
        Dim lNewDownstreamIcon As clsIcon = CurrentlySelectedDownstreamIcon()

        Dim lUpstreamUCI As atcUCI.HspfUci = Nothing
        If pIcon.UciFileName = cboUciFiles.Text Then
            lUpstreamUCI = pIcon.UciFile
        Else
            lUpstreamUCI = OpenUCI(cboUciFiles.Text)
        End If
        If lUpstreamUCI Is Nothing Then
            lReport &= "UCI file not found: " & pIcon.UciFileName & vbCrLf
        ElseIf lNewDownstreamIcon Is Nothing Then
            lReport &= "No downstream watershed specified"
        Else
            Dim lDownstreamUCI As atcUCI.HspfUci = lNewDownstreamIcon.UciFile
            If lDownstreamUCI Is Nothing Then
                lReport &= "Downstream UCI file not found: " & lNewDownstreamIcon.UciFileName & vbCrLf
            Else
                lReport &= "Upstream UCI file: " & vbCrLf & cboUciFiles.Text & vbCrLf & vbCrLf & "Downstream UCI file:" & vbCrLf & lNewDownstreamIcon.UciFileName & vbCrLf & vbCrLf
                Dim lConnCheck As List(Of String) = modUCI.ConnectionSummary(lUpstreamUCI, lDownstreamUCI)
                If lConnCheck Is Nothing OrElse lConnCheck.Count = 0 Then
                    lReport &= "No connecting datasets found." & vbCrLf
                Else
                    Dim lWDMFileName As String = String.Empty
                    For Each lReportLine In lConnCheck
                        Dim lFields() As String = lReportLine.Split("|"c)
                        If lFields(0) <> lWDMFileName Then
                            lWDMFileName = lFields(0)
                            lReport &= "WDM file: " & vbCrLf & lWDMFileName & vbCrLf
                        End If
                        For lField = 1 To lFields.Length - 1
                            lReport &= vbTab & lFields(lField)
                        Next
                        lReport &= vbCrLf
                    Next
                End If
            End If
        End If

        If lReport.Length > 0 Then
            Dim lText As New frmText
            lText.Icon = Me.Icon
            lText.Text = frmHspfSimulationManager.g_AppNameLong & " Connection Report"
            lText.txtMain.Text = lReport
            lText.Show()
        End If
        Me.Cursor = Cursors.Default
        Me.Enabled = True
    End Sub

    Private Sub frmModel_DragDrop(sender As Object, e As DragEventArgs) Handles Me.DragDrop
        If e.Data.GetDataPresent(Windows.Forms.DataFormats.FileDrop) Then
            For Each lFileName As String In e.Data.GetData(Windows.Forms.DataFormats.FileDrop)

                Select Case IO.Path.GetExtension(lFileName).ToLower
                    Case ".uci"
                        If Not cboUciFiles.Items.Contains(lFileName) Then
                            cboUciFiles.Items.Add(lFileName)
                        End If
                        cboUciFiles.SelectedItem = lFileName

                    Case ".png", ".jpg", ".gif"
                        btnImage.Text = lFileName
                        btnImage.BackgroundImage = Drawing.Image.FromFile(lFileName)
                        Me.Height = btnImage.Height + 164

                End Select
            Next
        End If
    End Sub

    Private Sub frmModel_DragEnter(sender As Object, e As DragEventArgs) Handles Me.DragEnter
        If e.Data.GetDataPresent(Windows.Forms.DataFormats.FileDrop) Then
            Dim lFileNames() As String = e.Data.GetData(Windows.Forms.DataFormats.FileDrop)
            If lFileNames.Length > 0 Then
                Select Case IO.Path.GetExtension(lFileNames(0)).ToLower
                    Case ".uci", ".png", ".jpg", ".gif"
                        e.Effect = Windows.Forms.DragDropEffects.All
                End Select
            End If
        End If
    End Sub

End Class