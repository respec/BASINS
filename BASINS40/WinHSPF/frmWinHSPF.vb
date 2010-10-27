Imports atcUCIForms
Imports MapWinUtility
Imports atcUCI
Imports atcUtility
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
        If CheckForOpenUCI() Then
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
        If CheckForOpenUCI() Then
            ReachEditor()
        End If
    End Sub

    Private Sub cmdReach_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdReach.Click
        If CheckForOpenUCI() Then
            ReachEditor()
        End If
    End Sub

    Private Sub SimulationTimeAndMetDataEditorToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimulationTimeAndMetDataEditorToolStripMenuItem.Click
        If CheckForOpenUCI() Then
            SimulationTimeMetDataEditor()
        End If
    End Sub

    Private Sub cmdTime_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdTime.Click
        If CheckForOpenUCI() Then
            SimulationTimeMetDataEditor()
        End If
    End Sub

    Private Sub LandUseEditorToolStripMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LandUseEditorToolStripMenuItem.Click
        If CheckForOpenUCI() Then
            LandUseEditor()
        End If
    End Sub

    Private Sub cmdLandUse_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdLandUse.Click
        If CheckForOpenUCI() Then
            LandUseEditor()
        End If
    End Sub

    Private Sub TablesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TablesToolStripMenuItem.Click
        If CheckForOpenUCI() Then
            EditControlCardsWithTables()
        End If
    End Sub

    Private Sub cmdControlCardsTables_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdControlCardsTables.Click
        If CheckForOpenUCI() Then
            EditControlCardsWithTables()
        End If
    End Sub

    Private Sub DescriptionsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DescriptionsToolStripMenuItem.Click
        If CheckForOpenUCI() Then
            EditControlCardsWithDescriptions()
        End If
    End Sub

    Private Sub cmdControlCardsDescriptions_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdControlCardsDescriptions.Click
        If CheckForOpenUCI() Then
            EditControlCardsWithDescriptions()
        End If
    End Sub

    Private Sub PollutantToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PollutantToolStripMenuItem.Click
        If CheckForOpenUCI() Then
            PollutantSelector()
        End If
    End Sub

    Private Sub cmdPollutant_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdPollutant.Click
        If CheckForOpenUCI() Then
            PollutantSelector()
        End If
    End Sub

    Private Sub PointSourceEditorToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PointSourceEditorToolStripMenuItem.Click
        If CheckForOpenUCI() Then
            PointSourceEditor()
        End If
    End Sub

    Private Sub cmdPoint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdPoint.Click
        If CheckForOpenUCI() Then
            PointSourceEditor()
        End If
    End Sub

    Private Sub InputDataEditorToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles InputDataEditorToolStripMenuItem.Click
        If CheckForOpenUCI() Then
            InputDataEditor()
        End If
    End Sub

    Private Sub cmdToolStripInputEditor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdInputDataEditor.Click
        If CheckForOpenUCI() Then
            InputDataEditor()
        End If
    End Sub

    Private Sub cmdOutputManager_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOutputManager.Click
        If CheckForOpenUCI() Then
            OutputManager()
        End If
    End Sub

    Private Sub OutputManagerToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OutputManagerToolStripMenuItem.Click
        If CheckForOpenUCI() Then
            OutputManager()
        End If
    End Sub

    Private Sub cmdViewOutput_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdViewOutput.Click
        If CheckForOpenUCI() Then
            ViewOutput()
        End If
    End Sub

    Private Sub ViewOutputToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ViewOutputToolStripMenuItem.Click
        If CheckForOpenUCI() Then
            ViewOutput()
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

    Private Sub AQUATOXToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AQUATOXToolStripMenuItem.Click

        If CheckForOpenUCI() Then
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
        End If

    End Sub

    Private Sub BMPToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BMPToolStripMenuItem.Click

        If CheckForOpenUCI() Then
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
        End If

    End Sub

    Private Sub HSPFparmToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HSPFparmToolStripMenuItem.Click

        If CheckForOpenUCI() Then
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
        End If

    End Sub

    Private Sub SaveAsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveAsToolStripMenuItem.Click

        If CheckForOpenUCI() Then
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
        End If

    End Sub

    Private Sub StarterToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StarterToolStripMenuItem.Click

        If CheckForOpenUCI() Then
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
        Dim lCloseMe As Boolean = True
        If Not pUCI Is Nothing Then
            If pUCI.Edited Then
                If Logger.Msg("Changes have been made since your last Save." & vbCrLf & vbCrLf & _
                              "Are you sure you want to Exit?", _
                              MsgBoxStyle.OkCancel, "WinHSPF Confirm Exit") = MsgBoxResult.Cancel Then
                    lCloseMe = False
                End If
            End If
        End If
        If lCloseMe Then
            Me.Dispose()
        End If
    End Sub

    Private Sub SaveToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveToolStripMenuItem.Click
        If CheckForOpenUCI() Then
            SaveUCI()
        End If
    End Sub

    Private Sub SaveToolStripButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveToolStripButton.Click
        If CheckForOpenUCI() Then
            SaveUCI()
        End If
    End Sub

    Private Sub NewToolStripButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripButton.Click
        NewUCI()
    End Sub

    Private Sub RunHSPFToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RunHSPFToolStripMenuItem.Click
        If CheckForOpenUCI() Then
            RunHSPF()
        End If
    End Sub

    Private Sub cmdRunHSPF_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdRunHSPF.Click
        If CheckForOpenUCI() Then
            RunHSPF()
        End If
    End Sub

    Private Sub frmWinHSPF_DragDrop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles Me.DragDrop
        Dim lUciFilename As String = DraggingUCI(e)
        If IO.File.Exists(lUciFilename) Then
            Me.Activate()
            OpenUCI(lUciFilename)
        End If
    End Sub

    Private Sub frmWinHSPF_DragEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles Me.DragEnter
        If DraggingUCI(e).Length > 0 Then
            e.Effect = Windows.Forms.DragDropEffects.All
        End If
    End Sub

    Private Function DraggingUCI(ByVal e As System.Windows.Forms.DragEventArgs) As String
        If e.Data.GetDataPresent(Windows.Forms.DataFormats.FileDrop) Then
            Dim lFilenames() As String = e.Data.GetData(Windows.Forms.DataFormats.FileDrop)
            If lFilenames.Length = 1 AndAlso lFilenames(0).ToLower.EndsWith(".uci") Then
                Return lFilenames(0)
            End If
        End If
        Return ""
    End Function

    Private Sub frmWinHSPF_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyValue = Windows.Forms.Keys.F1 Then
            ShowHelp(pWinHSPFManualName)
            ShowHelp("User's Guide\Detailed Functions\Main WinHSPF Window.html")
        End If
    End Sub

    Private Sub frmWinHSPF_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
        ResizeDiagram()
    End Sub
    Public Sub ResizeDiagram()
        SchematicDiagram.Top = TopPanel.Height
        SchematicDiagram.Size = New Size(Me.ClientRectangle.Width, Me.ClientRectangle.Height - SchematicDiagram.Top)
        SchematicDiagram.RefreshDetails()
        SchematicDiagram.RefreshDetails()
    End Sub

    Private Sub ContentsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ContentsToolStripMenuItem.Click
        ShowHelp(pWinHSPFManualName)
        ShowHelp("")
    End Sub

    Private Sub HSPFManualToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HSPFManualToolStripMenuItem.Click
        ShowHelp(pHSPFManualName)
        ShowHelp("")
    End Sub

    Private Sub WebSupToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles WebSupToolStripMenuItem.Click
        OpenFile("http://www.epa.gov/waterscience/BASINS/")
    End Sub

    Private Function CheckForOpenUCI() As Boolean
        Dim lReturn As Boolean = True
        If pUCI Is Nothing OrElse pUCI.Name.Length = 0 Then
            Logger.Msg("No project is active." & vbCrLf & vbCrLf & _
                       "Open a project before using this feature.", _
                       MsgBoxStyle.OkOnly, "WinHSPF Problem")
            lReturn = False
        End If
        CheckForOpenUCI = lReturn
    End Function
End Class
