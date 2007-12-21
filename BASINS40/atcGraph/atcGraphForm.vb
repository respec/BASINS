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
    Private WithEvents pEditor As ZedGraph.frmEdit 'frmGraphEdit

    'The group of atcData displayed
    Private WithEvents pDataGroup As atcDataGroup
    Private pNumProbabilityPoints As Integer = 200

    Private pXAxisType As AxisType = AxisType.DateDual
    Private pYAxisType As AxisType = AxisType.Linear

    Private Shared SaveImageExtension As String = ".png"
    Friend WithEvents mnuViewScatter As System.Windows.Forms.MenuItem
    Friend WithEvents mnuViewSep1 As System.Windows.Forms.MenuItem
    Friend WithEvents mnuViewVerticalZoom As System.Windows.Forms.MenuItem
    Friend WithEvents mnuViewHorizontalZoom As System.Windows.Forms.MenuItem
    Friend WithEvents mnuViewZoomMouse As System.Windows.Forms.MenuItem
    Friend WithEvents mnuEditCopyMetafile As System.Windows.Forms.MenuItem
    Friend WithEvents mnuCoordinates As System.Windows.Forms.MenuItem
    Friend WithEvents mnuCoordinatesOnMenuBar As System.Windows.Forms.MenuItem

    Private WithEvents pZgc As New ZedGraphControl

    Private Sub InitMasterPane()
        InitMatchingColors(FindFile("Find graph coloring rules", "GraphColors.txt"))

        pPaneMain = New GraphPane
        FormatPaneWithDefaults(pPaneMain, pXAxisType, pYAxisType)

        Me.Controls.Add(pZgc)
        With pZgc
            .Dock = DockStyle.Fill
            .Visible = True
            .IsSynchronizeXAxes = True
            .IsEnableHZoom = mnuViewHorizontalZoom.Checked
            .IsEnableVZoom = mnuViewVerticalZoom.Checked
            .IsZoomOnMouseCenter = mnuViewZoomMouse.Checked
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

        AuxAxisEnabled = False

        SetDatasets(pDataGroup)
    End Sub

    Private Property AuxAxisEnabled() As Boolean
        Get
            Return pAuxEnabled
        End Get
        Set(ByVal aValue As Boolean)
            If pAuxEnabled <> aValue Then
                pMaster.PaneList.Clear()
                Dim lGraphics As Graphics = Me.CreateGraphics()
                pAuxEnabled = aValue
                If pAuxEnabled Then
                    With pPaneMain
                        .YAxis.MinSpace = 80
                        .Y2Axis.MinSpace = 20
                        .Margin.All = 0
                        .Margin.Top = 10
                        .Margin.Bottom = 10
                    End With
                    pPaneAux = New ZedGraph.GraphPane
                    FormatPaneWithDefaults(pPaneAux, pXAxisType, pYAxisType)
                    With pPaneAux
                        .Margin.All = 0
                        .Margin.Top = 10
                        With .XAxis
                            .Title.IsVisible = False
                            .Scale.IsVisible = False
                            .Scale.Max = pPaneMain.XAxis.Scale.Max
                            .Scale.Min = pPaneMain.XAxis.Scale.Min
                        End With
                        .X2Axis.IsVisible = False
                        With .YAxis
                            .Type = AxisType.Linear
                            .MinSpace = 80
                        End With
                        .Y2Axis.MinSpace = 20
                    End With

                    With pMaster
                        .PaneList.Add(pPaneAux)
                        .PaneList.Add(pPaneMain)
                        .SetLayout(lGraphics, PaneLayout.SingleColumn)
                        ResizePanes()
                    End With
                Else
                    pMaster.PaneList.Add(pPaneMain)
                    pMaster.SetLayout(lGraphics, PaneLayout.SingleColumn)
                End If
                pMaster.AxisChange(lGraphics)
                Invalidate()
                lGraphics.Dispose()
                Me.Refresh()
            End If
        End Set
    End Property

    Private Sub SetDatasets(ByVal aDataGroup As atcDataGroup)
        Dim lGraphics As Graphics = Me.CreateGraphics()

        If Not pPaneMain Is Nothing Then pPaneMain.CurveList.Clear()
        If Not pPaneAux Is Nothing Then pPaneAux.CurveList.Clear()

        If mnuViewScatter.Checked Then
            AddDatasetsScatter(aDataGroup)
        Else
            For Each lTimeseries As atcTimeseries In aDataGroup
                AddDatasetTimeseries(lTimeseries)
            Next

            If mnuViewTime.Checked Then
                Dim lMin As Double = GetNaN()
                Dim lMax As Double = GetNaN()
                For Each lTimeseries As atcTimeseries In aDataGroup
                    If lTimeseries.numValues > 0 Then
                        If Double.IsNaN(lMin) OrElse lTimeseries.Attributes.GetValue("SJDay") < lMin Then
                            lMin = lTimeseries.Attributes.GetValue("SJDay")
                        End If
                        If Double.IsNaN(lMax) OrElse lTimeseries.Attributes.GetValue("EJDay") > lMax Then
                            lMax = lTimeseries.Attributes.GetValue("EJDay")
                        End If
                    End If
                Next
                If Not Double.IsNaN(lMin) Then Pane.XAxis.Scale.Min = lMin
                If Not Double.IsNaN(lMax) Then Pane.XAxis.Scale.Max = lMax
            End If
        End If
        pMaster.AxisChange(lGraphics)
        Invalidate()
        lGraphics.Dispose()
        Me.Refresh()
    End Sub

#Region " Windows Form Designer generated code "

    <CLSCompliant(False)> _
    Public Sub New(Optional ByVal aDataGroup As atcData.atcDataGroup = Nothing, _
                   Optional ByVal aXAxisType As ZedGraph.AxisType = AxisType.DateDual, _
                   Optional ByVal aYAxisType As ZedGraph.AxisType = AxisType.Linear)
        MyBase.New()
        InitializeComponent() 'required by Windows Form Designer

        pXAxisType = aXAxisType
        pYAxisType = aYAxisType

        Select Case pXAxisType
            Case AxisType.Probability
                mnuViewProbability.Checked = True
                mnuViewTime.Checked = False
            Case AxisType.Date, AxisType.DateDual
                mnuViewTime.Checked = True
            Case AxisType.Linear
                mnuViewScatter.Checked = True
                mnuViewTime.Checked = False
        End Select

        SetStyle(ControlStyles.DoubleBuffer Or ControlStyles.UserPaint Or ControlStyles.AllPaintingInWmPaint, True)

        Dim lTempDataGroup As atcDataGroup = aDataGroup
        If aDataGroup Is Nothing Then lTempDataGroup = New atcDataGroup

        If lTempDataGroup.Count = 0 Then 'ask user to specify some Data
            atcDataManager.UserSelectData(, lTempDataGroup, True)
        End If

        If lTempDataGroup.Count > 0 Then
            pDataGroup = lTempDataGroup 'Don't assign to pDataGroup too soon or it may slow down UserSelectData
            InitMasterPane()

            Dim DisplayPlugins As ICollection = atcDataManager.GetPlugins(GetType(atcDataDisplay))
            For Each lDisp As atcDataDisplay In DisplayPlugins
                Dim lMenuText As String = lDisp.Name
                If lMenuText.StartsWith("Analysis::") Then lMenuText = lMenuText.Substring(10)
                mnuAnalysis.MenuItems.Add(lMenuText, New EventHandler(AddressOf mnuAnalysis_Click))
            Next
        End If
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
    Friend WithEvents mnuViewTime As System.Windows.Forms.MenuItem
    Friend WithEvents mnuViewProbability As System.Windows.Forms.MenuItem
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
        Me.mnuViewTime = New System.Windows.Forms.MenuItem
        Me.mnuViewProbability = New System.Windows.Forms.MenuItem
        Me.mnuViewScatter = New System.Windows.Forms.MenuItem
        Me.mnuViewSep1 = New System.Windows.Forms.MenuItem
        Me.mnuViewVerticalZoom = New System.Windows.Forms.MenuItem
        Me.mnuViewHorizontalZoom = New System.Windows.Forms.MenuItem
        Me.mnuViewZoomMouse = New System.Windows.Forms.MenuItem
        Me.mnuAnalysis = New System.Windows.Forms.MenuItem
        Me.mnuHelp = New System.Windows.Forms.MenuItem
        Me.mnuCoordinates = New System.Windows.Forms.MenuItem
        Me.mnuCoordinatesOnMenuBar = New System.Windows.Forms.MenuItem
        Me.SuspendLayout()
        '
        'MainMenu1
        '
        Me.MainMenu1.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuFile, Me.mnuEdit, Me.mnuView, Me.mnuAnalysis, Me.mnuHelp, Me.mnuCoordinates})
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
        Me.mnuFileSave.Text = "Save"
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
        '
        'mnuView
        '
        Me.mnuView.Index = 2
        Me.mnuView.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuViewTime, Me.mnuViewProbability, Me.mnuViewScatter, Me.mnuViewSep1, Me.mnuViewVerticalZoom, Me.mnuViewHorizontalZoom, Me.mnuViewZoomMouse})
        Me.mnuView.Text = "View"
        '
        'mnuViewTime
        '
        Me.mnuViewTime.Checked = True
        Me.mnuViewTime.Index = 0
        Me.mnuViewTime.Text = "Time"
        '
        'mnuViewProbability
        '
        Me.mnuViewProbability.Index = 1
        Me.mnuViewProbability.Text = "Probability"
        '
        'mnuViewScatter
        '
        Me.mnuViewScatter.Index = 2
        Me.mnuViewScatter.Text = "Scatter"
        '
        'mnuViewSep1
        '
        Me.mnuViewSep1.Index = 3
        Me.mnuViewSep1.Text = "-"
        '
        'mnuViewVerticalZoom
        '
        Me.mnuViewVerticalZoom.Index = 4
        Me.mnuViewVerticalZoom.Text = "Vertical Zoom"
        '
        'mnuViewHorizontalZoom
        '
        Me.mnuViewHorizontalZoom.Checked = True
        Me.mnuViewHorizontalZoom.Index = 5
        Me.mnuViewHorizontalZoom.Text = "Horizontal Zoom"
        '
        'mnuViewZoomMouse
        '
        Me.mnuViewZoomMouse.Checked = True
        Me.mnuViewZoomMouse.Index = 6
        Me.mnuViewZoomMouse.Text = "Center Zoom on Mouse"
        '
        'mnuAnalysis
        '
        Me.mnuAnalysis.Index = 3
        Me.mnuAnalysis.Text = "Analysis"
        '
        'mnuHelp
        '
        Me.mnuHelp.Index = 4
        Me.mnuHelp.Shortcut = System.Windows.Forms.Shortcut.F1
        Me.mnuHelp.ShowShortcut = False
        Me.mnuHelp.Text = "Help"
        '
        'mnuCoordinates
        '
        Me.mnuCoordinates.Index = 5
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
        atcDataManager.UserSelectData(, pDataGroup, False)
    End Sub

    Private Sub mnuFileSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnuFileSave.Click
        SaveBitmapToFile()
    End Sub

    Public Sub SaveBitmapToFile(Optional ByVal aFileName As String = "")
        pZgc.AxisChange()
        If aFileName.Length = 0 Then 'No file name specified - ask user
            Dim lSavedAs As String
            lSavedAs = pZgc.SaveAs(SaveImageExtension)
            If lSavedAs.Length > 0 Then
                SaveImageExtension = System.IO.Path.GetExtension(lSavedAs)
            End If
        Else
            Dim lFormat As ImageFormat
            Select Case FileExt(aFileName).ToLower
                Case "bmp" : lFormat = ImageFormat.Bmp
                Case "png" : lFormat = ImageFormat.Png
                Case "gif" : lFormat = ImageFormat.Gif
                Case "jpg", _
                    "jpeg" : lFormat = ImageFormat.Jpeg
                Case "tif", _
                    "tiff" : lFormat = ImageFormat.Tiff
                Case Else : lFormat = ImageFormat.Png
            End Select

            MkDirPath(PathNameOnly(aFileName))
            Dim lStream As New StreamWriter(aFileName)
            pZgc.MasterPane.GetImage.Save(lStream.BaseStream, lFormat)
            lStream.Close()
        End If
    End Sub

    'Private Sub mnuFileSaveMetafile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFileSaveMetafile.Click
    '    Dim cdlg As New SaveFileDialog
    '    With cdlg
    '        .Title = "Save Enhanced Windows Metafile As..."
    '        .DefaultExt = ".emf"
    '        If .ShowDialog = Windows.Forms.DialogResult.OK Then
    '            SaveAsMetafile(.FileName)
    '        End If
    '    End With
    'End Sub

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
            Dim saveRect As RectangleF = Pane.Rect
            printdoc.Print()
            ' Restore graph size to fit form's bounds. 
            Pane.ReSize(Me.CreateGraphics, saveRect)
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
            Pane.ReSize(e.Graphics, New RectangleF(.X, .Y, .Width, .Height))
        End With

        ' Print the graph. 
        Pane.Draw(e.Graphics)

        e.HasMorePages = False 'ends the print job
    End Sub

    Private Sub pEditor_Apply(ByVal sender As Object, ByVal e As System.EventArgs) Handles pEditor.Apply
        pZgc.AxisChange()
        Invalidate()
        Me.Refresh()
    End Sub

    Private Sub mnuEditGraph_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnuEditGraph.Click
        pEditor = New ZedGraph.frmEdit
        pEditor.Edit(Pane)

        'pEditor = New frmGraphEdit
        'pEditor.Initialize(zgc.GraphPane)
        'pEditor.Show()
    End Sub

    Private Sub AddDatasetsScatter(ByVal aDataGroup As atcDataGroup)
        If aDataGroup.Count > 1 Then
            Dim lTimeseriesX As atcTimeseries = aDataGroup.ItemByIndex(0)
            Dim lTimeseriesY As atcTimeseries = aDataGroup.ItemByIndex(1)
            With Pane.XAxis
                pXAxisType = AxisType.Linear
                .Type = pXAxisType
                .Scale.Min = lTimeseriesX.Attributes.GetValue("Min", 0)
                .Scale.Min = lTimeseriesX.Attributes.GetValue("Max", 1000)
            End With

            With Pane.YAxis
                pYAxisType = AxisType.Linear
                .Type = pYAxisType
                .Scale.Min = lTimeseriesY.Attributes.GetValue("Min", 0)
                .Scale.Min = lTimeseriesY.Attributes.GetValue("Max", 1000)
            End With

            With lTimeseriesY.Attributes
                Dim lScen As String = .GetValue("scenario")
                Dim lLoc As String = .GetValue("location")
                Dim lCons As String = .GetValue("constituent")
                Dim lCurveColor As Color = GetMatchingColor(lScen & ":" & lLoc & ":" & lCons)
                Dim lCurve As LineItem = Nothing
                Dim lXValues() As Double = lTimeseriesX.Values
                Dim lYValues() As Double = lTimeseriesY.Values
                Dim lSymbol As SymbolType
                Dim lNPts As Integer = lXValues.GetUpperBound(0)
                If lNPts < 100 Then
                    lSymbol = SymbolType.Star
                Else
                    lSymbol = SymbolType.Circle
                End If
                lCurve = Pane.AddCurve(TSCurveLabel(lTimeseriesY), lXValues, lYValues, lCurveColor, lSymbol)
                If lNPts >= 1000 Then
                    lCurve.Symbol.Size = 1
                ElseIf lNPts >= 100 Then
                    lCurve.Symbol.Size = 2
                End If
                lCurve.Line.IsVisible = False
            End With
        End If
    End Sub

    Public Sub AddDatasetTimeseries(ByVal aTimeseries As atcTimeseries)
        If mnuViewProbability.Checked Then
            AddToProbabilityGraph(aTimeseries)
        ElseIf mnuViewTime.Checked Then
            AddToTimeGraph(aTimeseries)
        End If
    End Sub

    Private Sub AddToProbabilityGraph(ByVal aTimeseries As atcTimeseries)
        Dim lScen As String = aTimeseries.Attributes.GetValue("scenario")
        Dim lLoc As String = aTimeseries.Attributes.GetValue("location")
        Dim lCons As String = aTimeseries.Attributes.GetValue("constituent")
        Dim lCurveLabel As String = TSCurveLabel(aTimeseries)
        Dim lCurveColor As Color = GetMatchingColor(lScen & ":" & lLoc & ":" & lCons)
        Dim lCurve As LineItem = Nothing
        Dim lX(pNumProbabilityPoints) As Double
        Dim lLastIndex As Integer = lX.GetUpperBound(0)
        With Pane.XAxis
            If .Type <> AxisType.Probability Then
                '.Type = AxisType.Linear 'for debugging 
                .Type = AxisType.Probability
                With .MajorTic
                    .IsInside = True
                    .IsCrossInside = True
                    .IsOutside = False
                    .IsCrossOutside = False
                End With
                Dim lGraphics As Graphics = Me.CreateGraphics()
                pMaster.AxisChange(lGraphics)
                lGraphics.Dispose()
            End If
            For lXindex As Integer = 0 To lLastIndex
                lX(lXindex) = 100 * .Scale.DeLinearize(lXindex / CDbl(lLastIndex))
            Next
        End With
        Dim lProbScale As ZedGraph.ProbabilityScale = Pane.XAxis.Scale
        Dim lAttributeName As String
        Dim lIndex As Integer
        Dim lXFracExceed() As Double
        Dim lY() As Double

        ReDim lY(lLastIndex)
        'Dim lXSd() As Double
        'ReDim lXSd(lLastIndex)
        ReDim lXFracExceed(lLastIndex)

        For lIndex = 0 To lLastIndex
            'lXSd(lIndex) = Gausex(lX(lIndex) / 100)
            lXFracExceed(lIndex) = (100 - lX(lIndex)) / 100
            lAttributeName = "%" & Format(lX(lIndex), "00.####")
            lY(lIndex) = aTimeseries.Attributes.GetValue(lAttributeName)
            'Logger.Dbg(lAttributeName & " = " & lY(lIndex) & _
            '                            " : " & lX(lIndex) & _
            '                            " : " & lXFracExceed(lIndex))
        Next
        With Pane.XAxis
            .Scale.Min = lXFracExceed(0)
            .Scale.Max = lXFracExceed(lLastIndex)
            .Scale.BaseTic = lXFracExceed(0)
            .Title.Text = "Percent chance exceeded"
        End With
        With Pane.YAxis
            .Type = AxisType.Log
            .Scale.IsUseTenPower = False
            If aTimeseries.Attributes.ContainsAttribute("Units") Then
                .Title.Text = aTimeseries.Attributes.GetValue("Units")
                .Title.IsVisible = True
            End If
        End With

        'Upper right corner of chart is better for this graph type
        Pane.Legend.Location = New Location(0.95, 0.05, CoordType.ChartFraction, AlignH.Right, AlignV.Top)

        lCurve = Pane.AddCurve(lCurveLabel, lXFracExceed, lY, lCurveColor, SymbolType.None)
        lCurve.Line.Width = 1
        lCurve.Line.StepType = StepType.NonStep
        Me.Refresh()
    End Sub

    Private Sub AddToTimeGraph(ByVal aTimeseries As atcTimeseries)
        Dim lScen As String = aTimeseries.Attributes.GetValue("scenario")
        Dim lLoc As String = aTimeseries.Attributes.GetValue("location")
        Dim lCons As String = aTimeseries.Attributes.GetValue("constituent")
        Dim lCurveLabel As String = TSCurveLabel(aTimeseries)
        Dim lCurveColor As Color = GetMatchingColor(lScen & ":" & lLoc & ":" & lCons)

        Dim lCurve As LineItem = Nothing
        Dim lOldCons As String
        Dim lOldCurve As LineItem
        Dim lPane As GraphPane = pPaneMain
        Dim lYAxis As Axis = pPaneMain.YAxis
        Dim lYAxisName As String = aTimeseries.Attributes.GetValue("YAxis", "")
        If lYAxisName.Length = 0 Then 'Does not have a pre-assigned axis
            'Use the same Y axis as existing curve with this constituent
            Dim lFoundMatchingCons As Boolean = False
            For Each lTs As atcTimeseries In pDataGroup
                lOldCurve = pPaneMain.CurveList.Item(TSCurveLabel(lTs))
                If Not lOldCurve Is Nothing Then
                    lOldCons = lTs.Attributes.GetValue("constituent")
                    If lOldCons = lCons Then
                        If lOldCurve.IsY2Axis Then lYAxisName = "RIGHT" Else lYAxisName = "LEFT"
                        lFoundMatchingCons = True
                        Exit For
                    End If
                End If
            Next
            If Not lFoundMatchingCons AndAlso Pane.CurveList.Count > 0 Then
                'Put new curve on right axis if we already have a non-matching curve
                lYAxisName = "Right"
            End If
        End If
        Select Case lYAxisName.ToUpper
            Case "AUX"
                AuxAxisEnabled = True
                lPane = pPaneAux
                lYAxis = pPaneAux.YAxis
            Case "RIGHT"
                lPane = pPaneMain
                lYAxis = lPane.Y2Axis
                With lPane.YAxis
                    .MajorTic.IsOpposite = False
                    .MinorTic.IsOpposite = False
                End With
                With lYAxis
                    .MajorTic.IsOpposite = False
                    .MinorTic.IsOpposite = False
                End With
        End Select

        lYAxis.IsVisible = True

        With lPane
            If .XAxis.Type <> AxisType.DateDual Then .XAxis.Type = AxisType.DateDual
            .XAxis.Title.Text = "" 'TODO: remove this when spacing works for title on date axis
            If aTimeseries.Attributes.GetValue("point", False) Then
                lCurve = .AddCurve(lCurveLabel, New atcTimeseriesPointList(aTimeseries), lCurveColor, SymbolType.Plus)
                lCurve.Line.IsVisible = False
            Else
                lCurve = .AddCurve(lCurveLabel, New atcTimeseriesPointList(aTimeseries), lCurveColor, SymbolType.None)
                lCurve.Line.Width = 1
                lCurve.Line.StepType = StepType.RearwardStep
            End If

            If lYAxisName.ToUpper.Equals("RIGHT") Then
                lCurve.IsY2Axis = True
            End If

            'Use units as Y axis title (if this data has units and Y axis title is not set)
            If aTimeseries.Attributes.ContainsAttribute("Units") AndAlso _
               (lYAxis.Title Is Nothing OrElse lYAxis.Title.Text Is Nothing OrElse lYAxis.Title.Text.Length = 0) Then
                lYAxis.Title.Text = aTimeseries.Attributes.GetValue("Units")
                lYAxis.Title.IsVisible = True
            End If

            Dim lSJDay As Double = aTimeseries.Attributes.GetValue("SJDay")
            Dim lEJDay As Double = aTimeseries.Attributes.GetValue("EJDay")
            If .CurveList.Count = 1 Then
                If aTimeseries.numValues > 0 Then 'Set X axis to contain this date range
                    .XAxis.Scale.Min = lSJDay
                    .XAxis.Scale.Max = lEJDay
                End If
            ElseIf .CurveList.Count > 1 AndAlso Not lCurve Is Nothing Then
                'Expand time scale if needed to include all dates in new curve
                If aTimeseries.numValues > 0 Then
                    If lSJDay < .XAxis.Scale.Min Then
                        .XAxis.Scale.Min = lSJDay
                    End If
                    If lEJDay > .XAxis.Scale.Max Then
                        .XAxis.Scale.Max = lEJDay
                    End If
                End If
            End If
        End With
    End Sub

    Private Function TSCurveLabel(ByVal aTimeseries As atcTimeseries) As String
        With aTimeseries.Attributes
            Return .GetValue("scenario") & " " & .GetValue("constituent") & " at " & .GetValue("location")
        End With
    End Function

    Private Function Gausex(ByVal aExprob As Double) As Double
        'GAUSSIAN PROBABILITY FUNCTIONS   W.KIRBY  JUNE 71
        ' GAUSEX=VALUE EXCEEDED WITH PROB EXPROB
        ' rev 8/96 by PRH for VB
        ' rev 11/2006 by MHG for c#
        ' rev 11/2007 by JLK for VB.NET
        Static c0 As Double = 2.515517
        Static c1 As Double = 0.802853
        Static c2 As Double = 0.010328
        Static d1 As Double = 1.432788
        Static d2 As Double = 0.189269
        Static d3 As Double = 0.001308
        Static StandardDeviations As Integer = 3
        Dim pr, rtmp, p, t, numerat, Denom As Double

        Try
            p = aExprob
            If (p >= 1) Then
                rtmp = -StandardDeviations 'set to minimum
            ElseIf (p <= 0) Then
                rtmp = StandardDeviations 'set at maximum
            Else          'compute value
                pr = p
                If (p > 0.5) Then pr = 1 - pr
                t = Math.Sqrt(-2 * Math.Log(pr))
                numerat = (c0 + t * (c1 + t * c2))
                Denom = (1 + t * (d1 + t * (d2 + t * d3)))
                rtmp = t - numerat / Denom
                If (p > 0.5) Then rtmp = -rtmp
                If (rtmp > StandardDeviations) Then rtmp = StandardDeviations
                If (rtmp < -StandardDeviations) Then rtmp = -StandardDeviations
                Return rtmp
            End If
        Catch e As Exception
            Return 0
        End Try
    End Function

    Private Sub pTimeseriesGroup_Added(ByVal aAdded As atcCollection) Handles pDataGroup.Added
        For Each ts As atcTimeseries In aAdded
            AddDatasetTimeseries(ts)
        Next
        pZgc.AxisChange()
        Invalidate()
        Me.Refresh()
    End Sub

    Private Sub pTimeseriesGroup_Removed(ByVal aRemoved As atcCollection) Handles pDataGroup.Removed
        For Each lTs As atcTimeseries In aRemoved
            Pane.CurveList.Remove(Pane.CurveList.Item(TSCurveLabel(lTs)))
        Next
        pZgc.AxisChange()
        Invalidate()
        Me.Refresh()
    End Sub

    Private Sub mnuAnalysis_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnuAnalysis.Click
        atcDataManager.ShowDisplay(sender.Text, pDataGroup)
    End Sub

    Protected Overrides Sub OnClosing(ByVal e As System.ComponentModel.CancelEventArgs)
        'If Not pEditor Is Nothing Then
        '    pEditor.Close()
        '    pEditor = Nothing
        'End If
        pMaster = Nothing
        pDataGroup = Nothing
    End Sub

    Private Sub mnuHelp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuHelp.Click
        ShowHelp("BASINS Details\Analysis\Time Series Functions\Graph.html")
    End Sub

    Private Sub mnuCoordinatesOnMenuBar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuCoordinatesOnMenuBar.Click
        mnuCoordinatesOnMenuBar.Checked = Not mnuCoordinatesOnMenuBar.Checked
        If Not Not mnuCoordinatesOnMenuBar.Checked Then mnuCoordinates.Text = "Coordinates"
    End Sub

    Private Sub mnuViewTime_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuViewTime.Click
        mnuViewTime.Checked = True
        mnuViewProbability.Checked = False
        mnuViewScatter.Checked = False
        SetDatasets(pDataGroup)
    End Sub

    Private Sub mnuViewProbability_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnuViewProbability.Click
        mnuViewProbability.Checked = True
        mnuViewTime.Checked = False
        mnuViewScatter.Checked = False
        SetDatasets(pDataGroup)
    End Sub

    Private Sub mnuViewScatter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuViewScatter.Click
        mnuViewScatter.Checked = True
        mnuViewTime.Checked = False
        mnuViewProbability.Checked = False
        SetDatasets(pDataGroup)
    End Sub

    Private Sub mnuViewHorizontalZoom_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuViewHorizontalZoom.Click
        mnuViewHorizontalZoom.Checked = Not mnuViewHorizontalZoom.Checked
        pZgc.IsEnableHZoom = mnuViewHorizontalZoom.Checked
    End Sub

    Private Sub mnuViewVerticalZoom_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnuViewVerticalZoom.Click
        mnuViewVerticalZoom.Checked = Not mnuViewVerticalZoom.Checked
        pZgc.IsEnableVZoom = mnuViewVerticalZoom.Checked
    End Sub

    Private Sub mnuViewZoomMouse_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuViewZoomMouse.Click
        mnuViewZoomMouse.Checked = Not mnuViewZoomMouse.Checked
        pZgc.IsZoomOnMouseCenter = mnuViewZoomMouse.Checked
    End Sub

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
                    lPositionText = DumpDate(x)
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
        ResizePanes()
    End Sub

    Private Sub ResizePanes()
        If AuxAxisEnabled And Not pPaneAux Is Nothing Then
            Dim lOrigAuxHeight As Single = pPaneAux.Rect.Height
            Dim lTotalPaneHeight As Single = lOrigAuxHeight + pPaneMain.Rect.Height
            pPaneAux.Rect = New System.Drawing.Rectangle( _
                    pPaneAux.Rect.X, pPaneAux.Rect.Y, _
                    pPaneAux.Rect.Width, lTotalPaneHeight * AuxFraction)
            pPaneMain.Rect = New System.Drawing.Rectangle( _
                    pPaneMain.Rect.X, pPaneMain.Rect.Y - lOrigAuxHeight + pPaneAux.Rect.Height, _
                    pPaneMain.Rect.Width, lTotalPaneHeight - pPaneAux.Rect.Height)
        End If
    End Sub

    'Private Sub SaveAsMetafile(ByVal aFilename As String)
    '    Dim g As Graphics = Me.CreateGraphics()
    '    Dim hdc As IntPtr = g.GetHdc()
    '    Dim lMetafile As New Metafile(aFilename, hdc, EmfType.EmfPlusDual)
    '    g.ReleaseHdc(hdc)
    '    g.Dispose()

    '    Dim gMeta As Graphics = Graphics.FromImage(lMetafile)
    '    pZgc.MasterPane.Draw(gMeta)
    '    gMeta.Dispose()
    '    lMetafile.Dispose()
    'End Sub

    'Private Function PaneAsMetafile(ByVal aPane As GraphPane) As Metafile
    '    Dim g As Graphics = Me.CreateGraphics()
    '    Dim hdc As IntPtr = g.GetHdc()
    '    Dim lMetafile As New Metafile(hdc, EmfType.EmfOnly)
    '    g.ReleaseHdc(hdc)
    '    g.Dispose()

    '    Dim gMeta As Graphics = Graphics.FromImage(lMetafile)
    '    aPane.Draw(gMeta)
    '    gMeta.Dispose()
    '    Return lMetafile
    'End Function

    'Private Class MetafileHelper
    '    Private Declare Function GetClipboardData Lib "user32" (ByVal wFormat As Long) As Long
    '    Private Declare Function CloseClipboard Lib "user32" () As Boolean
    '    Private Declare Function EmptyClipboard Lib "user32" () As Boolean
    '    Private Declare Function OpenClipboard Lib "user32" (ByVal hWndNewOwner As IntPtr) As Boolean
    '    Private Declare Function SetClipboardData Lib "user32" (ByVal uFormat As UInt32, ByVal hMem As IntPtr) As IntPtr
    '    Private Declare Function DeleteEnhMetaFile Lib "gdi32" (ByVal hemf As IntPtr) As Boolean
    '    Private Declare Function CopyEnhMetaFile Lib "gdi32" Alias "CopyEnhMetaFileA" (ByVal hemfSrc As IntPtr, ByVal hNULL As IntPtr) As IntPtr
    '    Private Declare Function CopyEnhMetaFile Lib "gdi32" Alias "CopyEnhMetaFileA" (ByVal hemfSrc As Integer, ByVal lpszFile As String) As Integer
    '    ' Metafile mf is set to a state that is not valid inside this function.
    '    Public Shared Function PutEnhMetafileOnClipboard(ByVal hWnd As IntPtr, ByVal mf As Metafile) As Boolean
    '        Dim bResult As Boolean = False
    '        Dim hEMF, hEMF2, hRes As IntPtr
    '        hEMF = mf.GetHenhmetafile() ' invalidates mf
    '        If (Not hEMF.Equals(New IntPtr(0))) Then
    '            hEMF2 = CopyEnhMetaFile(hEMF, New IntPtr(0))
    '            If (Not hEMF2.Equals(New IntPtr(0))) Then
    '                If (OpenClipboard(hWnd)) Then
    '                    If (EmptyClipboard()) Then
    '                        hRes = SetClipboardData(14, hEMF2) '14 /*CF_ENHMETAFILE*/
    '                        bResult = hRes.Equals(hEMF2)
    '                        CloseClipboard()
    '                    End If
    '                End If
    '            End If
    '            DeleteEnhMetaFile(hEMF)
    '            Return bResult
    '        End If
    '    End Function
    'End Class
End Class

