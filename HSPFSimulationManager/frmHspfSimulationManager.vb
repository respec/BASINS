Public Class frmHspfSimulationManager
    'Dim pTreeBackground As Bitmap
    Private Sub frmHspfSimulationManager_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'If pTreeBackground IsNot Nothing Then pTreeBackground.Dispose()
        'pTreeBackground = New Bitmap(panelSchematic.Width, panelSchematic.Height, Drawing.Imaging.PixelFormat.Format32bppArgb)
        'Dim lGraphics As Graphics = Graphics.FromImage(pTreeBackground)
        'Dim lLinesPen As Pen = SystemPens.ControlDarkDark
        'lGraphics.DrawLine(lLinesPen, ButtonCenter(btnMedinaAboveLake), ButtonCenter(btnMedinaLake))
        'lGraphics.DrawLine(lLinesPen, ButtonCenter(btnMedinaLake), ButtonCenter(btnMedinaDiversionLake))
        'lGraphics.DrawLine(lLinesPen, ButtonCenter(btnMedio), ButtonCenter(btnMedinaBelowLake))
        'lGraphics.DrawLine(lLinesPen, ButtonCenter(btnMedinaDiversionLake), ButtonCenter(btnMedinaBelowLake))
        'lGraphics.DrawLine(lLinesPen, ButtonCenter(btnMedinaBelowLake), ButtonCenter(btnUSAR))
        'lGraphics.DrawLine(lLinesPen, ButtonCenter(btnLeon), ButtonCenter(btnMedinaBelowLake))
        'lGraphics.DrawLine(lLinesPen, ButtonCenter(btnSalado), ButtonCenter(btnUSAR))
        'lGraphics.DrawLine(lLinesPen, ButtonCenter(btnCibolo), ButtonCenter(btnLSAR))
        'lGraphics.DrawLine(lLinesPen, ButtonCenter(btnUSAR), ButtonCenter(btnLSAR))

        ''For Each lIcon As clsSchematicIcon In pIcons
        ''    With lIcon
        ''        Dim lIconCenter As Point = .Center
        ''        For Each lUpstreamIcon As clsIcon In lIcon.UpstreamIcons
        ''            lGraphics.DrawLine(lLinesPen, lIconCenter, lUpstreamIcon.Center)
        ''        Next
        ''        If .Selected Then
        ''            lGraphics.FillRectangle(HighlightBrush, .Left - pBorderWidth, .Top - pBorderWidth, .Width + pBorderWidth * 2, .Height + pBorderWidth * 2)
        ''        End If
        ''    End With
        ''Next
        'lGraphics.Dispose()
        'panelSchematic.BackgroundImage = pTreeBackground

    End Sub

    Private Function ButtonCenter(ByVal aButton As Button) As Drawing.Point
        With aButton
            Return New Drawing.Point(.Location.X + .Size.Width / 2, .Location.Y + .Size.Height / 2)
        End With
    End Function

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        End
    End Sub

    Private Sub OpenToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OpenToolStripMenuItem.Click
        Dim lFileName As String = String.Empty
        If BrowseOpen("Open Simulation Specification File", "*.txt", ".txt", Me, lFileName) Then
            Me.Text = "SARA HSPF Simulation Manager - " & lFileName
            Dim lIconSize As New Drawing.Size(SchematicDiagram.IconWidth, SchematicDiagram.IconHeight)
            Dim lNewIcons As IconCollection = clsSimulationManagerSpecFile.Open(Me.Size, lIconSize, lFileName)
            SchematicDiagram.IconWidth = lIconSize.Width
            SchematicDiagram.IconHeight = lIconSize.Height
            SchematicDiagram.BuildTree(lNewIcons)

            Dim lReport As String = ""
            Dim lReportLine As String

            For Each lIcon As clsIcon In lNewIcons
                Dim lUpstreamUCI As atcUCI.HspfUci = lIcon.UciFile
                If lUpstreamUCI Is Nothing Then
                    lReportLine = "UCI file not found: " & lIcon.UciFileName
                    If Not lReport.Contains(lReportLine) Then
                        lReport &= lReportLine & vbCrLf
                    End If
                ElseIf lIcon.DownstreamIcon IsNot Nothing Then
                    Dim lDownstreamUCI As atcUCI.HspfUci = lIcon.DownstreamIcon.UciFile
                    If lDownstreamUCI IsNot Nothing Then
                        Dim lConnCheck As List(Of String) = modUCI.ConnectionSummary(lUpstreamUCI, lDownstreamUCI)
                        If lConnCheck Is Nothing OrElse lConnCheck.Count = 0 Then
                            lReport &= "No datasets found connecting " & lIcon.UciFileName & " to " & lIcon.DownstreamIcon.UciFileName & vbCrLf
                        End If
                    End If
                End If
            Next

            If lReport.Length > 0 Then
                MsgBox(lReport, MsgBoxStyle.OkOnly, "Connection Report")
            End If
        End If
    End Sub

    Private Sub SaveToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveToolStripMenuItem.Click
        Dim lFileDialog As New Windows.Forms.SaveFileDialog()
        With lFileDialog
            .Title = "Save Simulation Specification File"
            '.Filter = aFilter
            '.FilterIndex = 0
            '.DefaultExt = aExtension
            '.CheckFileExists = False
            '.CheckPathExists = False
            If .ShowDialog(Me) = DialogResult.OK Then
                clsSimulationManagerSpecFile.Save(SchematicDiagram.AllIcons, Me.Size, New Drawing.Size(SchematicDiagram.IconWidth, SchematicDiagram.IconHeight), .FileName)
            End If
        End With
    End Sub

    Public Shared Function BrowseOpen(ByVal aTitle As String, ByVal aFilter As String, ByVal aExtension As String, ByVal aParentForm As Form, ByRef aFileName As String) As Boolean
        Dim lFileDialog As New Windows.Forms.OpenFileDialog()
        With lFileDialog
            .Title = aTitle
            If IO.File.Exists(aFileName) Then
                .FileName = aFileName
            End If
            If Not String.IsNullOrEmpty(aFilter) Then
                If Not aFilter.Contains("|") Then aFilter &= "|" & aFilter
                .Filter = aFilter
                .FilterIndex = 0
            End If
            .DefaultExt = aExtension
            .CheckFileExists = True
            If .ShowDialog(aParentForm) = DialogResult.OK Then
                aFileName = .FileName
                Return True
            End If
        End With
        Return False
    End Function

    Private Sub btnRunHSPF_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRunHSPF.Click
        Dim lRun As New frmRunHSPF
        lRun.Icon = Me.Icon
        lRun.SchematicDiagram = SchematicDiagram
        lRun.Show(Me)
        'frmModel.RunUCI()
    End Sub

    Private Sub AddWatershedToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AddWatershedToolStripMenuItem.Click
        Dim lModelForm As New frmModel
        lModelForm.Schematic = SchematicDiagram
        lModelForm.ModelIcon = SchematicDiagram.AllIcons.FindOrAddIcon("New Watershed")
        lModelForm.Show()
    End Sub
End Class
