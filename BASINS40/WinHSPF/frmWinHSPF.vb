Imports atcUCIForms
Imports MapWinUtility
Imports atcUCI
Imports System
Imports atcControls

Public Class frmWinHSPF

    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.

        'set edit menu
        SetEditMenu()

    End Sub

    Private Sub ReachEditorToolStripMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ReachEditorToolStripMenuItem.Click
        ReachEditor()
    End Sub

    Private Sub LandUseEditorToolStripMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LandUseEditorToolStripMenuItem.Click

        If IsNothing(pfrmLand) Then
            pfrmLand = New frmLand
            pfrmLand.Show()
        Else
            If pfrmLand.IsDisposed Then
                pfrmLand = New frmLand
                pfrmLand.Show()
            Else
                pfrmLand.WindowState = FormWindowState.Normal
                pfrmLand.BringToFront()
            End If
        End If

    End Sub

    Private Sub InputDataEditorToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles InputDataEditorToolStripMenuItem.Click

        If IsNothing(pfrmInputDataEditor) Then
            pfrmInputDataEditor = New frmInputDataEditor
            pfrmInputDataEditor.Show()
        Else
            If pfrmInputDataEditor.IsDisposed Then
                pfrmInputDataEditor = New frmInputDataEditor
                pfrmInputDataEditor.Show()
            Else
                pfrmInputDataEditor.WindowState = FormWindowState.Normal
                pfrmInputDataEditor.BringToFront()
            End If
        End If

    End Sub

    Private Sub PollutantToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PollutantToolStripMenuItem.Click

        If IsNothing(pfrmPollutant) Then
            pfrmPollutant = New frmPollutant
            pfrmPollutant.Show()
        Else
            If pfrmPollutant.IsDisposed Then
                pfrmPollutant = New frmPollutant
                pfrmPollutant.Show()
            Else
                pfrmPollutant.WindowState = FormWindowState.Normal
                pfrmPollutant.BringToFront()
            End If
        End If

    End Sub

    Private Sub SetEditMenu()
        Dim lAddFlag As Boolean
        For Each lBlock As HspfBlockDef In pMsg.BlockDefs
            lAddFlag = False
            If lBlock.SectionDefs(0).TableDefs.Count = 0 Then
                lAddFlag = True
            ElseIf lBlock.SectionDefs(0).TableDefs.Count = 1 Then
                If lBlock.SectionDefs(0).TableDefs(0).Name = "<NONE>" Then
                    lAddFlag = True
                End If
            End If
            If lAddFlag Then
                Me.EditToolStripMenuItem.DropDownItems.Add(lBlock.Name)
            End If
        Next lBlock
    End Sub

    Private Sub EditToolStripMenuItem_DropDownItemClicked(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolStripItemClickedEventArgs) Handles EditToolStripMenuItem.DropDownItemClicked

        If pUCI.Name.Length < 1 Then
            Logger.Msg("No Project is active." & vbCrLf & vbCrLf & _
                       "Open or Create a Project before selecting this menu item.", MsgBoxStyle.OkOnly, _
                       "WinHSPF: Edit Problem")
        Else
            If pUCI.MetSegs.Count > 0 Then
                pUCI.MetSeg2Source()
            End If
            pUCI.Point2Source()

            EditBlock(Me, e.ClickedItem.Text)

            pUCI.Source2MetSeg()
            pUCI.Source2Point()

            With SchematicDiagram
                .UpdateLegend()
                .BuildTree()
            End With
        End If
    End Sub


    Private Sub EditControlCardsWithTablesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EditControlCardsWithTablesToolStripMenuItem.Click

        If IsNothing(pfrmActivityAll) Then
            pfrmActivityAll = New frmActivityAll
            pfrmActivityAll.Show()
        Else
            If pfrmActivityAll.IsDisposed Then
                pfrmActivityAll = New frmActivityAll
                pfrmActivityAll.Show()
            Else
                pfrmActivityAll.WindowState = FormWindowState.Normal
                pfrmActivityAll.BringToFront()
            End If
        End If

    End Sub

    Private Sub EditWithToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EditWithToolStripMenuItem.Click

        If IsNothing(pfrmControl) Then
            pfrmControl = New frmControl
            pfrmControl.Show()
        Else
            If pfrmControl.IsDisposed Then
                pfrmControl = New frmControl
                pfrmControl.Show()
            Else
                pfrmControl.WindowState = FormWindowState.Normal
                pfrmControl.BringToFront()
            End If
        End If

    End Sub

    Private Sub ToolStripButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdToolStripOutput.Click

        If IsNothing(pfrmOutput) Then
            pfrmOutput = New frmOutput
            pfrmOutput.Show()
        Else
            If pfrmOutput.IsDisposed Then
                pfrmOutput = New frmOutput
                pfrmOutput.Show()
            Else
                pfrmOutput.WindowState = FormWindowState.Normal
                pfrmOutput.BringToFront()
            End If
        End If

    End Sub

    Private Sub AboutToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AboutToolStripMenuItem.Click

        If IsNothing(pfrmAbout) Then
            pfrmAbout = New frmAbout
            pfrmAbout.Show()
        Else
            If pfrmAbout.IsDisposed Then
                pfrmAbout = New frmAbout
                pfrmAbout.Show()
            Else
                pfrmAbout.WindowState = FormWindowState.Normal
                pfrmAbout.BringToFront()
            End If
        End If

    End Sub

    Private Sub cmdToolStripInputEditor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdToolStripInputEditor.Click

        If IsNothing(pfrmInputDataEditor) Then
            pfrmInputDataEditor = New frmInputDataEditor
            pfrmInputDataEditor.Show()
        Else
            If pfrmInputDataEditor.IsDisposed Then
                pfrmInputDataEditor = New frmInputDataEditor
                pfrmInputDataEditor.Show()
            Else
                pfrmInputDataEditor.WindowState = FormWindowState.Normal
                pfrmInputDataEditor.BringToFront()
            End If
        End If

    End Sub

    Private Sub cmdTime_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdTime.Click

        If IsNothing(pfrmTime) Then
            pfrmTime = New frmTime
            pfrmTime.Show()
        Else
            If pfrmTime.IsDisposed Then
                pfrmTime = New frmTime
                pfrmTime.Show()
            Else
                pfrmTime.WindowState = FormWindowState.Normal
                pfrmTime.BringToFront()
            End If
        End If

    End Sub

    Private Sub cmdPoint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdPoint.Click

        If IsNothing(pfrmPoint) Then
            pfrmPoint = New frmPoint
            pfrmPoint.Show()
        Else
            If pfrmPoint.IsDisposed Then
                pfrmPoint = New frmPoint
                pfrmPoint.Show()
            Else
                pfrmPoint.WindowState = FormWindowState.Normal
                pfrmPoint.BringToFront()
            End If
        End If

    End Sub

    Private Sub AQUATOXToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AQUATOXToolStripMenuItem.Click

        If IsNothing(pfrmAQUATOX) Then
            pfrmAQUATOX = New frmAQUATOX
            pfrmAQUATOX.Show()
        Else
            If pfrmAQUATOX.IsDisposed Then
                pfrmAQUATOX = New frmAQUATOX
                pfrmAQUATOX.Show()
            Else
                pfrmAQUATOX.WindowState = FormWindowState.Normal
                pfrmAQUATOX.BringToFront()
            End If
        End If

    End Sub

    Private Sub BMPToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BMPToolStripMenuItem.Click

        If IsNothing(pfrmBMP) Then
            pfrmBMP = New frmBMP
            pfrmBMP.Show()
        Else
            If pfrmBMP.IsDisposed Then
                pfrmBMP = New frmBMP
                pfrmBMP.Show()
            Else
                pfrmBMP.WindowState = FormWindowState.Normal
                pfrmBMP.BringToFront()
            End If
        End If

    End Sub

    Private Sub HSPFparmToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HSPFparmToolStripMenuItem.Click

        If IsNothing(pfrmHspfParm) Then
            pfrmHspfParm = New frmHspfParm
            pfrmHspfParm.Show()
        Else
            If pfrmHspfParm.IsDisposed Then
                pfrmHspfParm = New frmHspfParm
                pfrmHspfParm.Show()
            Else
                pfrmHspfParm.WindowState = FormWindowState.Normal
                pfrmHspfParm.BringToFront()
            End If
        End If

    End Sub

    Private Sub SaveAsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveAsToolStripMenuItem.Click

        If IsNothing(pfrmSaveAs) Then
            pfrmSaveAs = New frmSaveAs
            pfrmSaveAs.Show()
        Else
            If pfrmSaveAs.IsDisposed Then
                pfrmSaveAs = New frmSaveAs
                pfrmSaveAs.Show()
            Else
                pfrmSaveAs.WindowState = FormWindowState.Normal
                pfrmSaveAs.BringToFront()
            End If
        End If

    End Sub

    Private Sub StarterToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StarterToolStripMenuItem.Click

        If IsNothing(pfrmStarter) Then
            pfrmStarter = New frmStarter
            pfrmStarter.Show()
        Else
            If pfrmStarter.IsDisposed Then
                pfrmStarter = New frmStarter
                pfrmStarter.Show()
            Else
                pfrmStarter.WindowState = FormWindowState.Normal
                pfrmStarter.BringToFront()
            End If
        End If

    End Sub

    Private Sub cmdLandUse_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdLandUse.Click

        If IsNothing(pfrmLand) Then
            pfrmLand = New frmLand
            pfrmLand.Show()
        Else
            If pfrmLand.IsDisposed Then
                pfrmLand = New frmLand
                pfrmLand.Show()
            Else
                pfrmLand.WindowState = FormWindowState.Normal
                pfrmLand.BringToFront()
            End If
        End If

    End Sub

    Private Sub cmdPollutant_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdPollutant.Click

        If IsNothing(pfrmPollutant) Then
            pfrmPollutant = New frmPollutant
            pfrmPollutant.Show()
        Else
            If pfrmPollutant.IsDisposed Then
                pfrmPollutant = New frmPollutant
                pfrmPollutant.Show()
            Else
                pfrmPollutant.WindowState = FormWindowState.Normal
                pfrmPollutant.BringToFront()
            End If
        End If

    End Sub

    Private Sub cmdReach_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdReach.Click
        ReachEditor()
    End Sub

    Private Sub OpenToolStripButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OpenToolStripButton.Click
        OpenUCI()
    End Sub

    Private Sub OpenToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OpenToolStripMenuItem.Click
        OpenUCI()
    End Sub

    Private Sub CloseToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CloseToolStripMenuItem.Click
        CloseUCI()
    End Sub
End Class
