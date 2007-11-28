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

    'Graph editing form
    'Private WithEvents pEditor As ZedGraph.frmEdit 'frmGraphEdit

    'The group of atcData displayed
    Private WithEvents pDataGroup As atcDataGroup
    Private pNumProbabilityPoints As Integer = 98

    Private pXAxisType As AxisType = AxisType.DateMulti
    Private pYAxisType As AxisType = AxisType.Linear

    Private Shared SaveImageExtension As String = ".png"
    Friend WithEvents mnuViewScatter As System.Windows.Forms.MenuItem
    Friend WithEvents mnuViewSep1 As System.Windows.Forms.MenuItem
    Friend WithEvents mnuViewVerticalZoom As System.Windows.Forms.MenuItem
    Friend WithEvents mnuViewHorizontalZoom As System.Windows.Forms.MenuItem
    Friend WithEvents mnuViewZoomMouse As System.Windows.Forms.MenuItem

    Private WithEvents pZgc As New ZedGraphControl

    Private Sub InitMasterPane()
        Me.Controls.Add(pZgc)
        pZgc.Dock = DockStyle.Fill
        pZgc.Visible = True
        pMaster = pZgc.MasterPane
        pMaster.PaneList.Clear() 'remove default GraphPane
        'pMaster.PaneFill = New Fill(Color.White, Color.MediumSlateBlue, 45.0F)
        pMaster.Legend.IsVisible = False

        Dim lPane As GraphPane = New GraphPane

        'myPane.PaneFill = New Fill(Color.White, Color.LightYellow, 45.0F)
        With lPane
            With .XAxis
                .Type = pXAxisType
                .Scale.Max = Double.NegativeInfinity
                .Scale.Min = Double.PositiveInfinity
                .MajorTic.IsOutside = False
                .MajorTic.IsInside = True
                .MinorTic.IsOutside = False
                .MinorTic.IsInside = True
            End With
            With .X2Axis
                .IsVisible = False
            End With
            With .YAxis
                .Type = pYAxisType
                .MajorTic.IsOutside = False
                .MajorTic.IsInside = True
                .MinorTic.IsOutside = False
                .MinorTic.IsInside = True
            End With
            With .Y2Axis
                .MajorTic.IsOutside = False
                .MajorTic.IsInside = True
                .MinorTic.IsOutside = False
                .MinorTic.IsInside = True
            End With
        End With

        pZgc.IsEnableHZoom = mnuViewHorizontalZoom.Checked
        pZgc.IsEnableVZoom = mnuViewVerticalZoom.Checked
        pZgc.IsZoomOnMouseCenter = mnuViewZoomMouse.Checked

        pMaster.PaneList.Add(lPane)

        SetDatasets(pDataGroup)
    End Sub

    Private Sub SetDatasets(ByVal aDataGroup As atcDataGroup)
        Dim lGraphics As Graphics = Me.CreateGraphics()
        pMaster.SetLayout(lGraphics, PaneLayout.SingleColumn)
        Pane.CurveList.Clear()
        If mnuViewScatter.Checked Then
            AddDatasetsScatter(aDataGroup)
        Else
            For Each lTimeseries As atcTimeseries In aDataGroup
                AddDatasetTimeseries(lTimeseries, lTimeseries.ToString)
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
    End Sub

#Region " Windows Form Designer generated code "

    <CLSCompliant(False)> _
    Public Sub New(Optional ByVal aDataGroup As atcData.atcDataGroup = Nothing, _
                   Optional ByVal aXAxisType As ZedGraph.AxisType = AxisType.DateMulti, _
                   Optional ByVal aYAxisType As ZedGraph.AxisType = AxisType.Linear)
        MyBase.New()
        InitializeComponent() 'required by Windows Form Designer

        pXAxisType = aXAxisType
        pYAxisType = aYAxisType

        Select Case pXAxisType
            Case AxisType.Probability
                mnuViewProbability.Checked = True
                mnuViewTime.Checked = False
            Case AxisType.Date, AxisType.DateMulti
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
        Me.mnuView = New System.Windows.Forms.MenuItem
        Me.mnuViewTime = New System.Windows.Forms.MenuItem
        Me.mnuViewProbability = New System.Windows.Forms.MenuItem
        Me.mnuViewScatter = New System.Windows.Forms.MenuItem
        Me.mnuViewSep1 = New System.Windows.Forms.MenuItem
        Me.mnuViewVerticalZoom = New System.Windows.Forms.MenuItem
        Me.mnuViewHorizontalZoom = New System.Windows.Forms.MenuItem
        Me.mnuAnalysis = New System.Windows.Forms.MenuItem
        Me.mnuHelp = New System.Windows.Forms.MenuItem
        Me.mnuViewZoomMouse = New System.Windows.Forms.MenuItem
        Me.SuspendLayout()
        '
        'MainMenu1
        '
        Me.MainMenu1.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuFile, Me.mnuEdit, Me.mnuView, Me.mnuAnalysis, Me.mnuHelp})
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
        Me.mnuEdit.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuEditGraph, Me.mnuEditSep1, Me.mnuEditCopy})
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
        'mnuViewZoomMouse
        '
        Me.mnuViewZoomMouse.Checked = True
        Me.mnuViewZoomMouse.Index = 6
        Me.mnuViewZoomMouse.Text = "Center Zoom on Mouse"
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
    Public ReadOnly Property Pane() As GraphPane
        Get
            Return pZgc.MasterPane.PaneList.Item(0) ' pGraphPane
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

    Private Sub mnuEditCopy_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnuEditCopy.Click
        Clipboard.SetDataObject(pZgc.MasterPane.GetImage)
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

    'Private Sub pEditor_Apply(ByVal sender As Object, ByVal e As System.EventArgs) Handles pEditor.Apply
    '    zgc.AxisChange()
    '    Invalidate()
    '    Me.Refresh()
    'End Sub

    Private Sub mnuEditGraph_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnuEditGraph.Click
        'pEditor = New ZedGraph.frmEdit
        'pEditor.Edit(zgc.GraphPane)

        'pEditor = New frmGraphEdit
        'pEditor.Initialize(zgc.GraphPane)
        'pEditor.Show()
    End Sub

    Private Sub AddDatasetsScatter(ByVal aDataGroup As atcDataGroup)
        If aDataGroup.Count > 1 Then
            Dim lTimeseriesX As atcTimeseries = aDataGroup.ItemByIndex(0)
            Dim lTimeseriesY As atcTimeseries = aDataGroup.ItemByIndex(1)
            With Pane.XAxis
                .Type = AxisType.Linear
                .Scale.Min = lTimeseriesX.Attributes.GetValue("Min", 0)
                .Scale.Min = lTimeseriesX.Attributes.GetValue("Max", 1000)
            End With

            With Pane.YAxis
                .Type = AxisType.Linear
                .Scale.Min = lTimeseriesY.Attributes.GetValue("Min", 0)
                .Scale.Min = lTimeseriesY.Attributes.GetValue("Max", 1000)
            End With

            Dim lCurveColor As Color = GetMatchingColor(lTimeseriesX.Attributes.GetValue("scenario"))
            Dim lCurve As LineItem = Nothing
            Dim lXValues() As Double = lTimeseriesX.Values
            Dim lYValues() As Double = lTimeseriesY.Values
            lCurve = Pane.AddCurve(lTimeseriesX.ToString, lXValues, lYValues, lCurveColor, SymbolType.Star)
            lCurve.Line.IsVisible = False
        End If
    End Sub

    Public Sub AddDatasetTimeseries(ByVal aTimeseries As atcTimeseries, ByVal CurveLabel As String)
        Dim lCons As String = aTimeseries.Attributes.GetValue("constituent")
        Dim lOldCons As String

        Dim lCurveColor As Color = GetMatchingColor(aTimeseries.Attributes.GetValue("scenario"))
        Dim lCurve As LineItem = Nothing
        Dim lOldCurve As LineItem

        If mnuViewProbability.Checked Then
            Dim lX(200) As Double
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
            'percent greater than values
            'Dim lX() As Double = lProbScale.Percentages
            'Dim lXSd() As Double
            Dim lXFracExceed() As Double
            Dim lY() As Double
            'Dim lDivideBy As Double = pNumProbabilityPoints + 2

            'ReDim x(pNumProbabilityPoints - 1)
            'For lIndex = 0 To pNumProbabilityPoints - 1
            '    x(lIndex) = (lIndex + 1) / lDivideBy
            'Next
            ReDim lY(lLastIndex)
            'ReDim lXSd(lLastIndex)
            ReDim lXFracExceed(lLastIndex)

            For lIndex = 0 To lLastIndex
                'lXSd(lIndex) = Gausex(lX(lIndex) / 100)
                lXFracExceed(lIndex) = (100 - lX(lIndex)) / 100
                lAttributeName = "%" & Format(lX(lIndex), "00.####")
                lY(lIndex) = aTimeseries.Attributes.GetValue(lAttributeName)
                Logger.Dbg(lAttributeName & " = " & lY(lIndex) & _
                                            " : " & lX(lIndex) & _
                                            " : " & lXFracExceed(lIndex))
            Next
            With Pane.XAxis
                .Scale.Min = lXFracExceed(0)
                .Scale.Max = lXFracExceed(lLastIndex)
                AddHandler .ScaleFormatEvent, AddressOf XScaleFormatEvent
                .Scale.BaseTic = lXFracExceed(0)
            End With

            lCurve = Pane.AddCurve(CurveLabel, lXFracExceed, lY, lCurveColor, SymbolType.None)
            lCurve.Line.Width = 1
            lCurve.Line.StepType = StepType.NonStep
            Me.Refresh()
        ElseIf mnuViewTime.Checked Then
            If Pane.XAxis.Type <> AxisType.DateMulti Then Pane.XAxis.Type = AxisType.DateMulti
            If aTimeseries.Attributes.GetValue("point", False) Then
                lCurve = Pane.AddCurve(CurveLabel, New atcTimeseriesPointList(aTimeseries), lCurveColor, SymbolType.Plus)
                lCurve.Line.IsVisible = False
            Else
                lCurve = Pane.AddCurve(CurveLabel, New atcTimeseriesPointList(aTimeseries), lCurveColor, SymbolType.None)
                lCurve.Line.Width = 1
                lCurve.Line.StepType = StepType.RearwardStep
            End If
        End If
        'TODO: label Y Axis

        'TODO: 3rd Y Axis above (for PREC)

        If Pane.CurveList.Count = 1 Then
            If aTimeseries.numValues > 0 Then
                Pane.XAxis.Scale.Min = aTimeseries.Attributes.GetValue("SJDay")
                Pane.XAxis.Scale.Max = aTimeseries.Attributes.GetValue("EJDay")
            End If
        ElseIf Pane.CurveList.Count > 1 AndAlso Not lCurve Is Nothing Then
            'Expand time scale if needed to include all dates in new curve
            If aTimeseries.numValues > 0 Then
                If aTimeseries.Attributes.GetValue("SJDay") < Pane.XAxis.Scale.Min Then
                    Pane.XAxis.Scale.Min = aTimeseries.Attributes.GetValue("SJDay")
                End If
                If aTimeseries.Attributes.GetValue("EJDay") > Pane.XAxis.Scale.Max Then
                    Pane.XAxis.Scale.Max = aTimeseries.Attributes.GetValue("EJDay")
                End If
            End If

            'Use the same Y axis as other curves with this constituent
            Dim lFoundMatchingCons As Boolean = False
            For Each ts As atcTimeseries In pDataGroup
                lOldCurve = Pane.CurveList.Item(ts.ToString)
                If Not lOldCurve Is Nothing Then
                    lOldCons = ts.Attributes.GetValue("constituent")
                    If lOldCons = lCons Then
                        lCurve.IsY2Axis = lOldCurve.IsY2Axis
                        lFoundMatchingCons = True
                        Exit For
                    End If
                End If
            Next
            If Not lFoundMatchingCons Then
                lCurve.IsY2Axis = True
            End If
            If lCurve.IsY2Axis Then 'make sure second Y axis is visible and tics are not shown on other Y axis
                With Pane.Y2Axis
                    .MajorTic.IsOpposite = False
                    .MinorTic.IsOpposite = False
                    .IsVisible = True
                    .Title.IsVisible = True
                End With
                With Pane.YAxis
                    .MajorTic.IsOpposite = False
                    .MinorTic.IsOpposite = False
                End With
            End If
        End If

        'curve.Line.Fill = New Fill(Color.White, Color.FromArgb(60, 190, 50), 90.0F)
        'lCurve.Line.IsSmooth = False
        'curve.Line.SmoothTension = 0.6F
        'curve.Symbol.Fill = New Fill(Color.White)
        'lCurve.Symbol.Size = 10
        'lCurve.Symbol.IsVisible = True

    End Sub

    <CLSCompliant(False)> _
    Public Function XScaleFormatEvent(ByVal aGraphPane As GraphPane, _
                                      ByVal aAxis As ZedGraph.Axis, _
                                      ByVal aVal As Double, _
                                      ByVal aIndex As Integer) As String
        Dim lStr As String = Format(aVal * 100, "#0.##")
        'Logger.Dbg("LookingForXScaleTicValue:" & aIndex & ":" & aVal & ":" & lVal & ":" & lStr)
        Return lStr
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
            AddDatasetTimeseries(ts, ts.ToString)
        Next
        pZgc.AxisChange()
        Invalidate()
        Me.Refresh()
    End Sub

    Private Sub pTimeseriesGroup_Removed(ByVal aRemoved As atcCollection) Handles pDataGroup.Removed
        For Each ts As atcTimeseries In aRemoved
            Pane.CurveList.Remove(Pane.CurveList.Item(ts.ToString))
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

    'Private Sub CopyToClip(ByVal aPane As GraphPane)

    '    Dim g As Graphics = Me.CreateGraphics()
    '    Dim hdc As IntPtr = g.GetHdc()
    '    Dim Metafile As New Metafile(hdc, EmfType.EmfOnly)
    '    g.ReleaseHdc(hdc)
    '    g.Dispose()

    '    Dim gMeta As Graphics = Graphics.FromImage(Metafile)
    '    aPane.Draw(gMeta)
    '    gMeta.Dispose()

    '    ClipboardMetafileHelper.PutEnhMetafileOnClipboard(Me.Handle, Metafile)

    '    MessageBox.Show("Copied to ClipBoard")
    'End Sub

    'Private Class ClipboardMetafileHelper
    '    '<DllImport("user32.dll")> Public Shared Function OpenClipboard(ByVal hWndNewOwner As IntPtr) As Boolean
    '    'End Function

    '    '<DllImport("user32.dll")> Public Shared Function EmptyClipboard() As Boolean
    '    'End Function

    '    '<DllImport("user32.dll")> Public Shared Function SetClipboardData(ByVal uFormat As uint, ByVal hMem As IntPtr) As IntPtr
    '    'End Function

    '    '<DllImport("user32.dll")> Public Shared Function CloseClipboard() As Boolean
    '    'End Function

    '    '<DllImport("gdi32.dll")> Public Shared Function CopyEnhMetaFile(ByVal hemfSrc As IntPtr, ByVal hNULL As IntPtr) As IntPtr
    '    'End Function

    '    '<DllImport("gdi32.dll")> Public Shared Function DeleteEnhMetaFile(ByVal hemf As IntPtr) As Boolean
    '    'End Function

    '    ' Metafile mf is set to a state that is not valid inside this function.
    '    Public Shared Function PutEnhMetafileOnClipboard(ByVal hWnd As IntPtr, ByVal mf As Metafile) As Boolean
    '        Dim bResult As Boolean = False
    '        '    IntPtr(hEMF, hEMF2)
    '        '    hEMF = mf.GetHenhmetafile() ' invalidates mf
    '        '    If (not hEMF.Equals(New IntPtr(0))) Then
    '        '    hEMF2 = CopyEnhMetaFile( hEMF, new IntPtr(0) )
    '        '        If (!hEMF2.Equals(New IntPtr(0))) Then
    '        '            If (OpenClipboard(hWnd)) Then
    '        '                If (EmptyClipboard()) Then
    '        '                IntPtr hRes = SetClipboardData( 14 /*CF_ENHMETAFILE*/, hEMF2 )
    '        '                bResult = hRes.Equals( hEMF2 )
    '        '                CloseClipboard()
    '        '                End If
    '        '            End If
    '        '        End If
    '        '    DeleteEnhMetaFile( hEMF );
    '        Return bResult
    '    End Function
    'End Class

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

    Private Sub zgc_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles pZgc.MouseDown
        System.Console.WriteLine("Mouse Down " & e.Button)
    End Sub

    Private Sub zgc_MouseWheel(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles pZgc.MouseWheel
        System.Console.WriteLine("Mouse Wheel " & e.Button)
    End Sub

    Private Sub zgc_ZoomEvent(ByVal sender As ZedGraph.ZedGraphControl, ByVal oldState As ZedGraph.ZoomState, ByVal newState As ZedGraph.ZoomState) Handles pZgc.ZoomEvent
        System.Console.WriteLine("zgc_ZoomEvent " & newState.ToString)
    End Sub
End Class


