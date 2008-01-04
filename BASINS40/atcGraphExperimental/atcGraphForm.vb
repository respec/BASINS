Imports atcData
Imports atcUtility
Imports MapWinUtility

Imports ZedGraph

Imports System
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Drawing.Imaging
Imports System.Collections
Imports System.ComponentModel
Imports System.IO
Imports System.Windows.Forms
'Imports System.Runtime.InteropServices

Public Class atcGraphForm
    Inherits Form

    'Form object that contains graph(s)
    Private pMaster As ZedGraph.MasterPane
    Private pPaneMain As ZedGraph.GraphPane
    Private pPaneAux As ZedGraph.GraphPane
    Private pAuxEnabled As Boolean = True 'force change with value of false on init
    Public AuxFraction As Single = 0.2

    'Graph editing form
    Private WithEvents pEditor As frmGraphEditor ' ZedGraph.frmEdit 'frmGraphEdit

    Private WithEvents pZgc As New ZedGraphControl

    Private pGrapher As clsGraphBase

    Private Shared SaveImageExtension As String = ".png"
    Friend WithEvents mnuViewVerticalZoom As System.Windows.Forms.MenuItem
    Friend WithEvents mnuViewHorizontalZoom As System.Windows.Forms.MenuItem
    Friend WithEvents mnuEditCopyMetafile As System.Windows.Forms.MenuItem
    Friend WithEvents mnuCoordinates As System.Windows.Forms.MenuItem
    Friend WithEvents mnuCoordinatesOnMenuBar As System.Windows.Forms.MenuItem

    Public Property Grapher() As clsGraphBase
        Get
            Return pGrapher
        End Get
        Set(ByVal newValue As clsGraphBase)
            pGrapher = newValue
            RefreshGraph()
        End Set
    End Property

    Private Sub InitMasterPane()
        InitMatchingColors(FindFile("Find graph coloring rules", "GraphColors.txt"))

        pPaneMain = New GraphPane
        FormatPaneWithDefaults(pPaneMain)

        Me.Controls.Add(pZgc)
        With pZgc
            .Dock = DockStyle.Fill
            .Visible = True
            .IsSynchronizeXAxes = True
            .IsEnableHZoom = mnuViewHorizontalZoom.Checked
            .IsEnableVZoom = mnuViewVerticalZoom.Checked
            '.IsZoomOnMouseCenter = mnuViewZoomMouse.Checked
            pMaster = .MasterPane
        End With

        With pMaster
            .PaneList.Clear() 'remove default GraphPane
            .Border.IsVisible = False
            .Legend.IsVisible = False
            .Margin.All = 10
            .InnerPaneGap = 5
            .IsCommonScaleFactor = True
        End With

        pMaster.PaneList.Add(pPaneMain)
        AuxAxisEnabled = False

    End Sub

    Public Property AuxAxisEnabled() As Boolean
        Get
            Return pAuxEnabled
        End Get
        Set(ByVal aEnable As Boolean)
            If pAuxEnabled <> aEnable Then
                pAuxEnabled = aEnable
                pPaneAux = EnableAuxAxis(pMaster, aEnable, AuxFraction)
                'pMaster.PaneList.Clear()
                'Dim lGraphics As Graphics = Me.CreateGraphics()
                'If pAuxEnabled Then
                '    ' Main pane already exists, just needs to be shifted
                '    With pPaneMain
                '        .YAxis.MinSpace = 80
                '        .Y2Axis.MinSpace = 20
                '        .Margin.All = 0
                '        .Margin.Top = 10
                '        .Margin.Bottom = 10
                '    End With
                '    ' Create, format, position aux pane
                '    pPaneAux = New ZedGraph.GraphPane
                '    FormatPaneWithDefaults(pPaneAux)
                '    With pPaneAux
                '        .Margin.All = 0
                '        .Margin.Top = 10
                '        With .XAxis
                '            .Title.IsVisible = False
                '            .Scale.IsVisible = False
                '            .Scale.Max = pPaneMain.XAxis.Scale.Max
                '            .Scale.Min = pPaneMain.XAxis.Scale.Min
                '        End With
                '        .X2Axis.IsVisible = False
                '        With .YAxis
                '            .Type = AxisType.Linear
                '            .MinSpace = 80
                '        End With
                '        .Y2Axis.MinSpace = 20
                '    End With

                '    With pMaster
                '        .PaneList.Add(pPaneAux)
                '        .PaneList.Add(pPaneMain)
                '        .SetLayout(lGraphics, PaneLayout.SingleColumn)
                '        ResizePanes()
                '    End With
                'Else
                '    pMaster.PaneList.Add(pPaneMain)
                '    pMaster.SetLayout(lGraphics, PaneLayout.SingleColumn)
                'End If
                RefreshGraph()
            End If
        End Set
    End Property

#Region " Windows Form Designer generated code "

    <CLSCompliant(False)> _
    Public Sub New()
        MyBase.New()
        InitializeComponent() 'required by Windows Form Designer

        SetStyle(ControlStyles.DoubleBuffer Or ControlStyles.UserPaint Or ControlStyles.AllPaintingInWmPaint, True)

        InitMasterPane()

        Dim DisplayPlugins As ICollection = atcDataManager.GetPlugins(GetType(atcDataDisplay))
        For Each lDisp As atcDataDisplay In DisplayPlugins
            Dim lMenuText As String = lDisp.Name
            If lMenuText.StartsWith("Analysis::") Then lMenuText = lMenuText.Substring(10)
            mnuAnalysis.MenuItems.Add(lMenuText, New EventHandler(AddressOf mnuAnalysis_Click))
        Next
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents MainMenu1 As System.Windows.Forms.MainMenu
    Friend WithEvents mnuFile As System.Windows.Forms.MenuItem
    Friend WithEvents mnuFileSave As System.Windows.Forms.MenuItem
    Friend WithEvents mnuFilePrint As System.Windows.Forms.MenuItem
    Friend WithEvents mnuFileSelectData As System.Windows.Forms.MenuItem
    Friend WithEvents mnuFileSep1 As System.Windows.Forms.MenuItem
    Friend WithEvents mnuEdit As System.Windows.Forms.MenuItem
    Friend WithEvents mnuEditGraph As System.Windows.Forms.MenuItem
    Friend WithEvents mnuEditSep1 As System.Windows.Forms.MenuItem
    Friend WithEvents mnuEditCopy As System.Windows.Forms.MenuItem
    Friend WithEvents mnuView As System.Windows.Forms.MenuItem
    Friend WithEvents mnuAnalysis As System.Windows.Forms.MenuItem

    Friend WithEvents mnuHelp As System.Windows.Forms.MenuItem
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(atcGraphForm))
        Me.MainMenu1 = New System.Windows.Forms.MainMenu(Me.components)
        Me.mnuFile = New System.Windows.Forms.MenuItem
        Me.mnuFileSelectData = New System.Windows.Forms.MenuItem
        Me.mnuFileSep1 = New System.Windows.Forms.MenuItem
        Me.mnuFileSave = New System.Windows.Forms.MenuItem
        Me.mnuFilePrint = New System.Windows.Forms.MenuItem
        Me.mnuEdit = New System.Windows.Forms.MenuItem
        Me.mnuEditGraph = New System.Windows.Forms.MenuItem
        Me.mnuEditSep1 = New System.Windows.Forms.MenuItem
        Me.mnuEditCopy = New System.Windows.Forms.MenuItem
        Me.mnuEditCopyMetafile = New System.Windows.Forms.MenuItem
        Me.mnuView = New System.Windows.Forms.MenuItem
        Me.mnuViewVerticalZoom = New System.Windows.Forms.MenuItem
        Me.mnuViewHorizontalZoom = New System.Windows.Forms.MenuItem
        Me.mnuAnalysis = New System.Windows.Forms.MenuItem
        Me.mnuHelp = New System.Windows.Forms.MenuItem
        Me.mnuCoordinates = New System.Windows.Forms.MenuItem
        Me.mnuCoordinatesOnMenuBar = New System.Windows.Forms.MenuItem
        Me.SuspendLayout()
        '
        'MainMenu1
        '
        Me.MainMenu1.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuFile, Me.mnuEdit, Me.mnuView, Me.mnuAnalysis, Me.mnuCoordinates, Me.mnuHelp})
        '
        'mnuFile
        '
        Me.mnuFile.Index = 0
        Me.mnuFile.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuFileSelectData, Me.mnuFileSep1, Me.mnuFileSave, Me.mnuFilePrint})
        Me.mnuFile.Text = "File"
        '
        'mnuFileSelectData
        '
        Me.mnuFileSelectData.Index = 0
        Me.mnuFileSelectData.Text = "Select Data"
        '
        'mnuFileSep1
        '
        Me.mnuFileSep1.Index = 1
        Me.mnuFileSep1.Text = "-"
        '
        'mnuFileSave
        '
        Me.mnuFileSave.Index = 2
        Me.mnuFileSave.Text = "Save As..."
        '
        'mnuFilePrint
        '
        Me.mnuFilePrint.Index = 3
        Me.mnuFilePrint.Text = "Print"
        '
        'mnuEdit
        '
        Me.mnuEdit.Index = 1
        Me.mnuEdit.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuEditGraph, Me.mnuEditSep1, Me.mnuEditCopy, Me.mnuEditCopyMetafile})
        Me.mnuEdit.Text = "Edit"
        '
        'mnuEditGraph
        '
        Me.mnuEditGraph.Index = 0
        Me.mnuEditGraph.Text = "Graph"
        '
        'mnuEditSep1
        '
        Me.mnuEditSep1.Index = 1
        Me.mnuEditSep1.Text = "-"
        '
        'mnuEditCopy
        '
        Me.mnuEditCopy.Index = 2
        Me.mnuEditCopy.Shortcut = System.Windows.Forms.Shortcut.CtrlC
        Me.mnuEditCopy.Text = "Copy"
        '
        'mnuEditCopyMetafile
        '
        Me.mnuEditCopyMetafile.Index = 3
        Me.mnuEditCopyMetafile.Text = "Copy Metafile"
        Me.mnuEditCopyMetafile.Visible = False
        '
        'mnuView
        '
        Me.mnuView.Index = 2
        Me.mnuView.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuViewVerticalZoom, Me.mnuViewHorizontalZoom})
        Me.mnuView.Text = "View"
        '
        'mnuViewVerticalZoom
        '
        Me.mnuViewVerticalZoom.Index = 0
        Me.mnuViewVerticalZoom.Text = "Vertical Zoom"
        '
        'mnuViewHorizontalZoom
        '
        Me.mnuViewHorizontalZoom.Checked = True
        Me.mnuViewHorizontalZoom.Index = 1
        Me.mnuViewHorizontalZoom.Text = "Horizontal Zoom"
        '
        'mnuAnalysis
        '
        Me.mnuAnalysis.Index = 3
        Me.mnuAnalysis.Text = "Analysis"
        '
        'mnuHelp
        '
        Me.mnuHelp.Index = 5
        Me.mnuHelp.Shortcut = System.Windows.Forms.Shortcut.F1
        Me.mnuHelp.ShowShortcut = False
        Me.mnuHelp.Text = "Help"
        '
        'mnuCoordinates
        '
        Me.mnuCoordinates.Index = 4
        Me.mnuCoordinates.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuCoordinatesOnMenuBar})
        Me.mnuCoordinates.Text = "Coordinates"
        '
        'mnuCoordinatesOnMenuBar
        '
        Me.mnuCoordinatesOnMenuBar.Checked = True
        Me.mnuCoordinatesOnMenuBar.Index = 0
        Me.mnuCoordinatesOnMenuBar.Text = "On Menu Bar"
        '
        'atcGraphForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(543, 496)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Menu = Me.MainMenu1
        Me.Name = "atcGraphForm"
        Me.Text = "Graph"
        Me.ResumeLayout(False)

    End Sub

#End Region

    <CLSCompliant(False)> _
    Public ReadOnly Property ZedGraphCtrl() As ZedGraphControl
        Get
            Return pZgc
        End Get
    End Property

    <CLSCompliant(False)> _
    Public ReadOnly Property PaneAux() As GraphPane
        Get
            Return pPaneAux
        End Get
    End Property

    <CLSCompliant(False)> _
    Public ReadOnly Property Pane() As GraphPane
        Get
            Return pPaneMain
        End Get
    End Property

    Private Sub mnuFileSelectData_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFileSelectData.Click
        atcDataManager.UserSelectData(, pGrapher.Datasets, False)
    End Sub

    Private Sub mnuFileSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnuFileSave.Click
        RefreshGraph()
        Dim lSavedAs As String
        lSavedAs = pZgc.SaveAs(SaveImageExtension)
        If lSavedAs.Length > 0 Then
            SaveImageExtension = System.IO.Path.GetExtension(lSavedAs)
        End If
    End Sub

    Private Sub mnuEditCopy_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnuEditCopy.Click
        Clipboard.SetDataObject(pZgc.MasterPane.GetImage)
    End Sub

    Private Sub mnuEditCopyMetafile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuEditCopyMetafile.Click
        'MetafileHelper.PutEnhMetafileOnClipboard(Me.Handle, PaneAsMetafile(Pane))
    End Sub

    Private Sub mnuFilePrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnuFilePrint.Click
        Dim printdlg As New PrintDialog
        Dim printdoc As New Printing.PrintDocument
        AddHandler printdoc.PrintPage, AddressOf Me.PrintPage

        printdlg.Document = printdoc
        printdlg.AllowSelection = False
        printdlg.ShowHelp = True

        ' If the result is OK then print the document.
        If (printdlg.ShowDialog = Windows.Forms.DialogResult.OK) Then
            Dim saveRect As RectangleF = pMaster.Rect
            printdoc.Print()
            ' Restore graph size to fit form's bounds. 
            pMaster.ReSize(Me.CreateGraphics, saveRect)
        End If
    End Sub

    '' <summary> Prints the displayed graph. </summary> 
    '' <param name="sender"> Object raising this event. </param> 
    '' <param name="e"> Event arguments passing graphics context to print to. </param> 
    Private Sub PrintPage(ByVal sender As System.Object, ByVal e As Printing.PrintPageEventArgs)
        ' Validate. 
        If (e Is Nothing) Then Return
        If (e.Graphics Is Nothing) Then Return

        ' Resize the graph to fit the printout. 
        With e.MarginBounds
            pMaster.ReSize(e.Graphics, New RectangleF(.X, .Y, .Width, .Height))
        End With

        ' Print the graph. 
        pMaster.Draw(e.Graphics)

        e.HasMorePages = False 'ends the print job
    End Sub

    Private Sub pEditor_Apply() Handles pEditor.Apply
        RefreshGraph()
    End Sub

    Public Sub RefreshGraph()
        pZgc.AxisChange()
        Invalidate()
        Me.Refresh()
    End Sub

    Private Sub mnuEditGraph_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnuEditGraph.Click
        pEditor = New frmGraphEditor ' ZedGraph.frmEdit
        pEditor.Edit(Pane)

        'pEditor = New frmGraphEdit
        'pEditor.Initialize(zgc.GraphPane)
        'pEditor.Show()
    End Sub

    Private Sub mnuAnalysis_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnuAnalysis.Click
        atcDataManager.ShowDisplay(sender.Text, pGrapher.Datasets)
    End Sub

    Private Sub mnuHelp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuHelp.Click
        ShowHelp("BASINS Details\Analysis\Time Series Functions\Graph.html")
    End Sub

    Private Sub mnuCoordinatesOnMenuBar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuCoordinatesOnMenuBar.Click
        mnuCoordinatesOnMenuBar.Checked = Not mnuCoordinatesOnMenuBar.Checked
        If Not Not mnuCoordinatesOnMenuBar.Checked Then mnuCoordinates.Text = "Coordinates"
    End Sub

    Private Sub mnuViewHorizontalZoom_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuViewHorizontalZoom.Click
        mnuViewHorizontalZoom.Checked = Not mnuViewHorizontalZoom.Checked
        pZgc.IsEnableHZoom = mnuViewHorizontalZoom.Checked
    End Sub

    Private Sub mnuViewVerticalZoom_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnuViewVerticalZoom.Click
        mnuViewVerticalZoom.Checked = Not mnuViewVerticalZoom.Checked
        pZgc.IsEnableVZoom = mnuViewVerticalZoom.Checked
    End Sub

    'Private Sub mnuViewZoomMouse_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuViewZoomMouse.Click
    '    mnuViewZoomMouse.Checked = Not mnuViewZoomMouse.Checked
    '    pZgc.IsZoomOnMouseCenter = mnuViewZoomMouse.Checked
    'End Sub

    Private Sub pZgc_ZoomEvent(ByVal sender As ZedGraph.ZedGraphControl, ByVal oldState As ZedGraph.ZoomState, ByVal newState As ZedGraph.ZoomState) Handles pZgc.ZoomEvent

    End Sub

    Private Function pZgc_MouseMoveEvent(ByVal sender As ZedGraph.ZedGraphControl, ByVal e As System.Windows.Forms.MouseEventArgs) As System.Boolean Handles pZgc.MouseMoveEvent
        If mnuCoordinatesOnMenuBar.Checked Then
            ' Save the mouse location
            Dim mousePt As New PointF(e.X, e.Y)
            Dim lPositionText As String = "Coordinates"
            ' Find the pane that contains the current mouse location
            Dim lPane As GraphPane = sender.MasterPane.FindChartRect(mousePt)
            ' If pane is non-null, we have a valid location.  Otherwise, the mouse is not within any chart rect.
            If Not lPane Is Nothing Then
                Dim x, y As Double
                ' Convert the mouse location to X, Y scale values
                lPane.ReverseTransform(mousePt, x, y)
                ' Format the status label text
                If lPane.XAxis.Type = AxisType.DateDual Then
                    Dim lDate As Date = Date.FromOADate(x)
                    If lPane.XAxis.Scale.Max - lPane.XAxis.Scale.Min > 10 Then
                        lPositionText = lDate.ToString("yyyy MMM d")
                    Else
                        lPositionText = lDate.ToString("yyyy MMM d HH:mm")
                    End If
                Else
                    lPositionText = DoubleToString(x)
                End If
                lPositionText = "(" & lPositionText & ", " & DoubleToString(y) & ")"
            End If
            mnuCoordinates.Text = lPositionText
        End If
        ' Return false to indicate we have not processed the MouseMoveEvent
        ' ZedGraphControl should still go ahead and handle it
        Return False
    End Function

    Private Sub atcGraphForm_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
        ResizePanesWithAux(pMaster, AuxFraction)
    End Sub

    Private Sub FreeResources()
        If Not pEditor Is Nothing Then
            pEditor.Dispose()
            pEditor = Nothing
        End If
        pMaster = Nothing
        pZgc = Nothing
        If Not pGrapher Is Nothing Then
            pGrapher.Dispose()
            pGrapher = Nothing
        End If
    End Sub

    Protected Overrides Sub OnClosing(ByVal e As System.ComponentModel.CancelEventArgs)
        FreeResources()
    End Sub

    Private Sub atcGraphForm_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed
        FreeResources()
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
        FreeResources()
    End Sub

End Class

