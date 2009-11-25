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
                .UpdateDetails()
            End With
        End If
    End Sub

    Private Sub ReachEditorToolStripMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ReachEditorToolStripMenuItem.Click
        ReachEditor()
    End Sub

    Private Sub cmdReach_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdReach.Click
        ReachEditor()
    End Sub

    Private Sub SimulationTimeAndMetDataEditorToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimulationTimeAndMetDataEditorToolStripMenuItem.Click
        SimulationTimeMetDataEditor()
    End Sub

    Private Sub cmdTime_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdTime.Click
        SimulationTimeMetDataEditor()
    End Sub

    Private Sub LandUseEditorToolStripMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LandUseEditorToolStripMenuItem.Click
        LandUseEditor()
    End Sub

    Private Sub cmdLandUse_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdLandUse.Click
        LandUseEditor()
    End Sub

    Private Sub TablesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TablesToolStripMenuItem.Click
        EditControlCardsWithTables()
    End Sub

    Private Sub cmdControlCardsTables_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdControlCardsTables.Click
        EditControlCardsWithTables()
    End Sub

    Private Sub DescriptionsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DescriptionsToolStripMenuItem.Click
        EditControlCardsWithDescriptions()
    End Sub

    Private Sub cmdControlCardsDescriptions_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdControlCardsDescriptions.Click
        EditControlCardsWithDescriptions()
    End Sub

    Private Sub PollutantToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PollutantToolStripMenuItem.Click
        PollutantSelector()
    End Sub

    Private Sub cmdPollutant_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdPollutant.Click
        PollutantSelector()
    End Sub

    Private Sub PointSourceEditorToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PointSourceEditorToolStripMenuItem.Click
        PointSourceEditor()
    End Sub

    Private Sub cmdPoint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdPoint.Click
        PointSourceEditor()
    End Sub

    Private Sub InputDataEditorToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles InputDataEditorToolStripMenuItem.Click
        InputDataEditor()
    End Sub

    Private Sub cmdToolStripInputEditor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdInputDataEditor.Click
        InputDataEditor()
    End Sub

    Private Sub cmdOutputManager_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOutputManager.Click
        OutputManager()
    End Sub

    Private Sub OutputManagerToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OutputManagerToolStripMenuItem.Click
        OutputManager()
    End Sub

    Private Sub cmdViewOutput_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdViewOutput.Click
        Logger.Msg("'View Output' Feature not yet implemented.", vbOKOnly, "WinHSPF Problem")
    End Sub

    Private Sub ViewOutputToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ViewOutputToolStripMenuItem.Click
        Logger.Msg("'View Output' Feature not yet implemented.", vbOKOnly, "WinHSPF Problem")
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

    Private Sub AQUATOXToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AQUATOXToolStripMenuItem.Click

        Dim lAQlocCnt As Integer = 0

        For Each lOper As HspfOperation In pUCI.OpnSeqBlock.Opns
            If lOper.Name = "RCHRES" Then
                If frmOutput.IsAQUATOXLocation(lOper.Name, lOper.Id) Then
                    'this is an aquatox output location
                    lAQlocCnt = lAQlocCnt + 1
                End If
            End If
        Next

        If lAQlocCnt = 0 Then
            Logger.Msg("At least one AQUATOX output location must be specified " & vbCrLf & "in the Output Manager " & _
                       "before linking to AQUATOX.", "WinHSPF-AQUATOX Problem")
        Else
            If IsNothing(pfrmAQUATOX) Then
                pfrmAQUATOX = New frmAQUATOX
                pfrmAQUATOX.Init()
                pfrmAQUATOX.Show()
            Else
                If pfrmAQUATOX.IsDisposed Then
                    pfrmAQUATOX = New frmAQUATOX
                    pfrmAQUATOX.Init()
                    pfrmAQUATOX.Show()
                Else
                    pfrmAQUATOX.WindowState = FormWindowState.Normal
                    pfrmAQUATOX.BringToFront()
                End If
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

    Private Sub PESTToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PESTToolStripMenuItem.Click
        Logger.Msg("'PEST' Feature not yet implemented.", vbOKOnly, "WinHSPF Problem")
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

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        Me.Dispose()
    End Sub

    Private Sub SaveToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveToolStripMenuItem.Click
        SaveUCI()
    End Sub

    Private Sub SaveToolStripButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveToolStripButton.Click
        SaveUCI()
    End Sub

    Private Sub NewToolStripButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripButton.Click
        NewUCI()
    End Sub

    Private Sub RunHSPFToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RunHSPFToolStripMenuItem.Click
        RunHSPF()
    End Sub

    Private Sub cmdRunHSPF_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdRunHSPF.Click
        RunHSPF()
    End Sub
End Class
