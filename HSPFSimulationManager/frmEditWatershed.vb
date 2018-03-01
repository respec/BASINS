Imports atcUtility
Imports MapWinUtility

Public Class frmEditWatershed

    Private pIcon As clsIcon
    Friend Schematic As ctlSchematic

    Public Property ModelIcon() As clsIcon
        Set(ByVal value As clsIcon)
            pIcon = value

            txtName.Text = pIcon.WatershedName

            Dim lUciFileNames As String = ""
            For Each lScenario As clsUciScenario In value.Scenarios
                lstScenarios.Items.Add(lScenario.Clone())
                If lScenario.UciFileName = pIcon.UciFileName Then
                    lstScenarios.SelectedIndex = lstScenarios.Items.Count - 1
                End If
            Next

            cboDownstream.Items.Clear()
            cboDownstream.Items.Add("None")
            For Each lIcon In Schematic.AllIcons
                If lIcon.WatershedName <> pIcon.WatershedName Then
                    cboDownstream.Items.Add(lIcon.WatershedName)
                End If
            Next

            Dim lIndex As Integer = 0
            If pIcon.DownstreamIcon IsNot Nothing Then
                lIndex = cboDownstream.Items.IndexOf(pIcon.DownstreamIcon.WatershedName)
                If lIndex = -1 Then
                    lIndex = cboDownstream.Items.Count
                    cboDownstream.Items.Add(pIcon.DownstreamIcon.WatershedName)
                End If
            End If
            cboDownstream.SelectedIndex = lIndex
            btnImage.Text = pIcon.WatershedImageFilename
            btnImage.BackgroundImage = pIcon.WatershedImage
            btnImage.BackgroundImageLayout = ImageLayout.Zoom
        End Set
        Get
            pIcon.WatershedName = txtName.Text
            pIcon.Scenarios.Clear()
            If lstScenarios.SelectedItem IsNot Nothing Then
                pIcon.Scenario = lstScenarios.SelectedItem
            End If
            For Each lScenario As clsUciScenario In lstScenarios.Items
                pIcon.ScenariosAdd(lScenario)
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
        End If
    End Sub

    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
        Try
            Logger.Progress(1, 1)  'attempt to get rid of lingering progress messages
            Logger.Dbg("frmEditWatershed_clicked_ok")
            Try
                Schematic.AllIcons.Remove(pIcon)
            Catch
            End Try
            pIcon = Me.ModelIcon 'Apply changes to icon
            Schematic.AllIcons.Add(pIcon)
            Schematic.BuildTree(Schematic.AllIcons)
            Logger.Dbg("finished BuildTree after frmEditWatershed_clicked_ok")

            'do connection check to see if this newly added UCI is connected to the downstream UCI
            Dim lUCIs As New atcCollection
            Dim lUpstreamUCIs As New atcCollection
            Dim lDownstreamUCIs As New atcCollection
            lUCIs.Add(pIcon.UciFile)
            lUpstreamUCIs.Add(pIcon.UciFile)
            Dim lDownIcon As clsIcon = Nothing
            If cboDownstream.SelectedItem.Trim <> "None" Then
                lDownIcon = Schematic.AllIcons.FindOrAddIcon(cboDownstream.SelectedItem.Trim)
            End If
            If lDownIcon IsNot Nothing AndAlso lDownIcon.UciFile IsNot Nothing Then
                lUCIs.Add(lDownIcon.UciFile)
                lDownstreamUCIs.Add(lDownIcon.UciFile)
                Logger.Dbg("frmEditWatershed_clicked_ok added icon: " & lDownIcon.UciFileName)
                Logger.Dbg("frmEditWatershed_clicked_ok added icon name: " & lDownIcon.UciFile.Name)
            End If
            If lUCIs.Count = 2 Then
                'return blank if models are not connected
                'return name of transfer wdm if models are connected using a single transfer wdm
                'return 'MULTIPLE' if models are connected but connections use multiple wdms 
                Logger.Progress(1, 1)  'attempt to get rid of lingering progress messages
                Dim lTransferWDM As String = UsesTransfer(lUpstreamUCIs, lDownstreamUCIs)
                Logger.Dbg("frmEditWatershed_clicked_ok transfer wdm: " & lTransferWDM)
                If lTransferWDM.Length = 0 Then
                    'these models are not connected, ask about connecting them
                    Logger.Progress(1, 1)  'attempt to get rid of lingering progress messages
                    If Logger.Msg("This UCI has no connection to a downstream UCI." & vbCrLf & vbCrLf &
                           "Do you want to modify this UCI and the downstream UCI so that they connect?",
                              MsgBoxStyle.YesNo, "Add Connection?") = MsgBoxResult.Yes Then
                        'new form here for entering reach IDs and transfer WDM name
                        Logger.Dbg("frmEditWatershed_clicked_ok about to show AddConnectionForm")
                        Dim lAddConnectionForm As New frmAddConnection
                        lAddConnectionForm.SetUCIs(pIcon.UciFile, lDownIcon.UciFile)
                        lAddConnectionForm.ShowDialog()
                        Logger.Dbg("frmEditWatershed_clicked_ok finished AddConnectionForm")
                    End If
                End If
            End If

        Catch ex As Exception
        End Try
        Me.Close()
    End Sub

    Private Sub btnRemove_Click(sender As Object, e As EventArgs) Handles btnRemove.Click
        If Schematic.RemoveIconAfterAsking(pIcon) Then
            Me.Close()
        End If
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub btnAddScenario_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddScenario.Click
        Dim lFileName As String = String.Empty
        If frmHspfSimulationManager.BrowseOpen("Open UCI File", "UCI Files|*.uci|All Files|*.*", ".uci", Me, lFileName) Then
            'do more checking here?
            AddNewScenario(lFileName)
        End If
    End Sub

    Private Sub btnRemoveScenario_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRemoveScenario.Click
        Try
            Dim lSelectedIndex As Integer = lstScenarios.SelectedIndex
            If lSelectedIndex >= 0 Then
                lstScenarios.Items.RemoveAt(lSelectedIndex)
                If lSelectedIndex >= lstScenarios.Items.Count Then
                    lSelectedIndex -= 1
                End If
                If lSelectedIndex < 0 OrElse lstScenarios.Items.Count = 0 Then
                    lstScenarios.Text = ""
                Else
                    lstScenarios.SelectedIndex = lSelectedIndex
                End If
            End If
        Catch
        End Try
    End Sub

    Private Sub btnConnectionReport_Click(sender As Object, e As EventArgs) Handles btnConnectionReport.Click
        Try
            Me.Enabled = False
            Me.Cursor = Cursors.WaitCursor
            Dim lReport As String = g_AppNameLong & " Connection Report" & vbCrLf & vbCrLf & txtName.Text & ": "
            Logger.Dbg("frmEditWatershed_ConnectionReport starting")
            If lstScenarios.SelectedItem Is Nothing Then
                lReport &= "No Scenario"
            Else
                lReport &= lstScenarios.SelectedItem.ScenarioName & vbCrLf
                Dim lNewDownstreamIcon As clsIcon = CurrentlySelectedDownstreamIcon()

                Dim lUciFileName As String = lstScenarios.SelectedItem.UciFileName
                Logger.Dbg("frmEditWatershed_ConnectionReport  lUciFileName: " & lUciFileName)

                Dim lUpstreamUCI As atcUCI.HspfUci = Nothing
                If lUciFileName.ToLower.Equals(pIcon.UciFileName.ToLower) Then
                    lUpstreamUCI = pIcon.UciFile
                Else
                    lUpstreamUCI = OpenUCI(lUciFileName)
                End If
                If lUpstreamUCI Is Nothing Then
                    lReport &= "UCI file not found: " & lUciFileName & vbCrLf
                ElseIf lNewDownstreamIcon Is Nothing Then
                    lReport &= "No downstream watershed specified"
                Else
                    Dim lDownstreamUCI As atcUCI.HspfUci = lNewDownstreamIcon.UciFile
                    If lDownstreamUCI Is Nothing Then
                        lReport &= "Upstream UCI file: " & vbCrLf & lstScenarios.Text & vbCrLf
                        lReport &= "Downstream UCI file not found: " & lNewDownstreamIcon.UciFileName & vbCrLf
                    Else
                        lReport &= "To: " & lNewDownstreamIcon.WatershedName & ", " & lNewDownstreamIcon.Scenario.ScenarioName & vbCrLf & vbCrLf

                        Logger.Dbg("frmEditWatershed_ConnectionReport  about to write: ")
                        Logger.Dbg("frmEditWatershed_ConnectionReport  about to write: upstream: " & lUpstreamUCI.Name)
                        Logger.Dbg("frmEditWatershed_ConnectionReport  about to write: upstream: " & lDownstreamUCI.Name)
                        lReport &= ConnectionReport(lUpstreamUCI, lDownstreamUCI)
                    End If
                End If
            End If

            If lReport.Length > 0 Then
                Dim lText As New frmText
                lText.Icon = Me.Icon
                lText.Text = g_AppNameLong & " Connection Report"
                lText.txtMain.Text = lReport
                lText.Show()
            End If
            Me.Cursor = Cursors.Default
            Me.Enabled = True
            Logger.Dbg("frmEditWatershed_ConnectionReport finished")
        Catch ex As Exception
        End Try
    End Sub

    Private Sub AddNewScenario(aUciFileName As String)
        Dim lNewScenario As New clsUciScenario(aUciFileName, "")
        Dim lNameScenario As New frmNameScenario
        lNameScenario.Icon = Me.Icon
        If lNameScenario.AskUser(lNewScenario, Schematic.AllScenarioNames) Then
            lstScenarios.Items.Add(lNewScenario)
            lstScenarios.SelectedIndex = lstScenarios.Items.Count - 1
        End If
    End Sub

    Private Sub frmModel_DragDrop(sender As Object, e As DragEventArgs) Handles Me.DragDrop
        If e.Data.GetDataPresent(Windows.Forms.DataFormats.FileDrop) Then
            For Each lFileName As String In e.Data.GetData(Windows.Forms.DataFormats.FileDrop)

                Select Case IO.Path.GetExtension(lFileName).ToLower
                    Case ".uci"
                        'commented out to allow adding same UCI again and giving it a new scenario name
                        'For Each lExistingScenario As clsUciScenario In lstScenarios.Items
                        '    If lExistingScenario.UciFileName.ToLower.Equals(lFileName.ToLower) Then
                        '        lstScenarios.SelectedItem = lExistingScenario
                        '        GoTo NextFile
                        '    End If
                        'Next
                        AddNewScenario(lFileName)
                    Case ".png", ".jpg", ".gif"
                        btnImage.Text = lFileName
                        btnImage.BackgroundImage = Drawing.Image.FromFile(lFileName)

                End Select
NextFile:
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

    Private Sub btnRenameScenario_Click(sender As Object, e As EventArgs) Handles btnRenameScenario.Click
        Dim lScenario As clsUciScenario = lstScenarios.SelectedItem
        If lScenario Is Nothing Then
            Logger.Msg("Select a scenario UCI file before Renaming", MsgBoxStyle.OkOnly, g_AppNameLong)
        Else
            Dim lNameScenario As New frmNameScenario
            lNameScenario.Icon = Me.Icon
            If lNameScenario.AskUser(lScenario, Schematic.AllScenarioNames) Then
                Dim lSelectedIndex As Integer = lstScenarios.SelectedIndex
                lstScenarios.Items.RemoveAt(lSelectedIndex)
                lstScenarios.Items.Insert(lSelectedIndex, lScenario)
                lstScenarios.SelectedIndex = lSelectedIndex
                lstScenarios.Refresh()
            End If
        End If
    End Sub
End Class