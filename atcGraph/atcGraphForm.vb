Imports System
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports ZedGraph

Public Class atcGraphForm
  Inherits System.Windows.Forms.Form

  'Private pMaster As ZedGraph.MasterPane

  'Private pGraphPane As GraphPane
  Private pTimeseriesManager As atcData.atcTimeseriesManager
  Private WithEvents pEditor As atcGraphEdit

  Private Sub InitMasterPane()
    'pGraphPane = New GraphPane(New RectangleF(10, 10, 10, 10), "", "", "")

    'pGraphPane.XAxis.Type = ZedGraph.AxisType.Date
    'pGraphPane.XAxis.MajorUnit = ZedGraph.DateUnit.Day
    'pGraphPane.XAxis.MinorUnit = ZedGraph.DateUnit.Hour
    ''atcGraphTime.AddDatasetTimeseries(pGraphPane, t, "Test")
    'pGraphPane.AxisChange(Me.CreateGraphics)

    Dim pMaster As MasterPane = zgc.MasterPane
    pMaster.PaneList.Clear() 'remove default GraphPane
    'pMaster.PaneFill = New Fill(Color.White, Color.MediumSlateBlue, 45.0F)
    pMaster.Legend.IsVisible = False

    Dim myPane As GraphPane = New GraphPane

    'myPane.PaneFill = New Fill(Color.White, Color.LightYellow, 45.0F)
    myPane.XAxis.Type = ZedGraph.AxisType.Date
    myPane.XAxis.MajorUnit = ZedGraph.DateUnit.Day
    myPane.XAxis.MinorUnit = ZedGraph.DateUnit.Hour
    pMaster.PaneList.Add(myPane)

    Dim g As Graphics = Me.CreateGraphics()
    pMaster.AutoPaneLayout(g, PaneLayout.SingleColumn)
    pMaster.AxisChange(g)
    Invalidate()
    g.Dispose()
  End Sub

#Region " Windows Form Designer generated code "

  Public Sub New(ByVal aTimeseriesManager As atcData.atcTimeseriesManager)
    MyBase.New()
    pTimeseriesManager = aTimeseriesManager
    InitializeComponent() 'required by Windows Form Designer
    InitMasterPane()
    Me.SetStyle(ControlStyles.DoubleBuffer Or ControlStyles.UserPaint Or ControlStyles.AllPaintingInWmPaint, True)
  End Sub

  'Required by the Windows Form Designer
  Private components As System.ComponentModel.IContainer

  'NOTE: The following procedure is required by the Windows Form Designer
  'It can be modified using the Windows Form Designer.  
  'Do not modify it using the code editor.
  Friend WithEvents MainMenu1 As System.Windows.Forms.MainMenu
  Friend WithEvents mnuFile As System.Windows.Forms.MenuItem
  Friend WithEvents mnuFileAdd As System.Windows.Forms.MenuItem
  Friend WithEvents mnuFileSave As System.Windows.Forms.MenuItem
  Friend WithEvents mnuFilePrint As System.Windows.Forms.MenuItem
  Friend WithEvents mnuEdit As System.Windows.Forms.MenuItem
  Friend WithEvents mnuEditTitles As System.Windows.Forms.MenuItem
  Friend WithEvents mnuEditCurves As System.Windows.Forms.MenuItem
  Friend WithEvents mnuEditFont As System.Windows.Forms.MenuItem
  Friend WithEvents mnuAnalysis As System.Windows.Forms.MenuItem
  Friend WithEvents mnuAnalysisGraph As System.Windows.Forms.MenuItem
  Friend WithEvents mnuAnalysisList As System.Windows.Forms.MenuItem
  Friend WithEvents mnuAnalysisGenerate As System.Windows.Forms.MenuItem
  Friend WithEvents zgc As ZedGraph.ZedGraphControl
  Friend WithEvents mnuEditY As System.Windows.Forms.MenuItem
  Friend WithEvents mnuEditX As System.Windows.Forms.MenuItem
  <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
    Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(atcGraphForm))
    Me.MainMenu1 = New System.Windows.Forms.MainMenu
    Me.mnuFile = New System.Windows.Forms.MenuItem
    Me.mnuFileAdd = New System.Windows.Forms.MenuItem
    Me.mnuFileSave = New System.Windows.Forms.MenuItem
    Me.mnuFilePrint = New System.Windows.Forms.MenuItem
    Me.mnuEdit = New System.Windows.Forms.MenuItem
    Me.mnuEditY = New System.Windows.Forms.MenuItem
    Me.mnuEditTitles = New System.Windows.Forms.MenuItem
    Me.mnuEditCurves = New System.Windows.Forms.MenuItem
    Me.mnuEditFont = New System.Windows.Forms.MenuItem
    Me.mnuAnalysis = New System.Windows.Forms.MenuItem
    Me.mnuAnalysisGraph = New System.Windows.Forms.MenuItem
    Me.mnuAnalysisList = New System.Windows.Forms.MenuItem
    Me.mnuAnalysisGenerate = New System.Windows.Forms.MenuItem
    Me.zgc = New ZedGraph.ZedGraphControl
    Me.mnuEditX = New System.Windows.Forms.MenuItem
    Me.SuspendLayout()
    '
    'MainMenu1
    '
    Me.MainMenu1.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuFile, Me.mnuEdit, Me.mnuAnalysis})
    '
    'mnuFile
    '
    Me.mnuFile.Index = 0
    Me.mnuFile.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuFileAdd, Me.mnuFileSave, Me.mnuFilePrint})
    Me.mnuFile.Text = "&File"
    '
    'mnuFileAdd
    '
    Me.mnuFileAdd.Index = 0
    Me.mnuFileAdd.Text = "&Add Timeseries"
    '
    'mnuFileSave
    '
    Me.mnuFileSave.Index = 1
    Me.mnuFileSave.Text = "&Save"
    '
    'mnuFilePrint
    '
    Me.mnuFilePrint.Index = 2
    Me.mnuFilePrint.Text = "&Print"
    '
    'mnuEdit
    '
    Me.mnuEdit.Index = 1
    Me.mnuEdit.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuEditX, Me.mnuEditY, Me.mnuEditTitles, Me.mnuEditCurves, Me.mnuEditFont})
    Me.mnuEdit.Text = "&Edit"
    '
    'mnuEditY
    '
    Me.mnuEditY.Index = 1
    Me.mnuEditY.Text = "&Y Axis"
    '
    'mnuEditTitles
    '
    Me.mnuEditTitles.Index = 2
    Me.mnuEditTitles.Text = "&Titles"
    '
    'mnuEditCurves
    '
    Me.mnuEditCurves.Index = 3
    Me.mnuEditCurves.Text = "&Curves"
    '
    'mnuEditFont
    '
    Me.mnuEditFont.Index = 4
    Me.mnuEditFont.Text = "&Font"
    '
    'mnuAnalysis
    '
    Me.mnuAnalysis.Index = 2
    Me.mnuAnalysis.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuAnalysisGraph, Me.mnuAnalysisList, Me.mnuAnalysisGenerate})
    Me.mnuAnalysis.Text = "&Analysis"
    '
    'mnuAnalysisGraph
    '
    Me.mnuAnalysisGraph.Index = 0
    Me.mnuAnalysisGraph.Text = "&Graph"
    '
    'mnuAnalysisList
    '
    Me.mnuAnalysisList.Index = 1
    Me.mnuAnalysisList.Text = "&List"
    '
    'mnuAnalysisGenerate
    '
    Me.mnuAnalysisGenerate.Index = 2
    Me.mnuAnalysisGenerate.Text = "&Generate"
    '
    'zgc
    '
    Me.zgc.Dock = System.Windows.Forms.DockStyle.Fill
    Me.zgc.IsEnablePan = True
    Me.zgc.IsEnableZoom = True
    Me.zgc.IsShowContextMenu = True
    Me.zgc.IsShowPointValues = False
    Me.zgc.Location = New System.Drawing.Point(0, 0)
    Me.zgc.Name = "zgc"
    Me.zgc.PointDateFormat = "g"
    Me.zgc.PointValueFormat = "G"
    Me.zgc.Size = New System.Drawing.Size(544, 497)
    Me.zgc.TabIndex = 0
    '
    'mnuEditX
    '
    Me.mnuEditX.Index = 0
    Me.mnuEditX.Text = "&X Axis"
    '
    'atcGraphForm
    '
    Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
    Me.ClientSize = New System.Drawing.Size(544, 497)
    Me.Controls.Add(Me.zgc)
    Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
    Me.Menu = Me.MainMenu1
    Me.Name = "atcGraphForm"
    Me.Text = "Graph"
    Me.ResumeLayout(False)

  End Sub

#End Region

  Public ReadOnly Property Pane() As GraphPane
    Get
      Return zgc.MasterPane.PaneList.Item(0) ' pGraphPane
    End Get
  End Property

  'Public Sub Demo()
  '  Me.Show()
  '  pGraphPane.Title = "Wacky Widget Company" & vbCr & "Production Report"
  '  pGraphPane.XAxis.Title = "Time, Days" & vbCr & "(Since Plant Construction Startup)"
  '  pGraphPane.YAxis.Title = "Widget Production" & vbCr & "(units/hour)"
  '  Dim x As Double() = {100, 200, 300, 400, 500, 600, 700, 800, 900, 1000}
  '  Dim y As Double() = {20, 10, 50, 25, 35, 75, 90, 40, 33, 50}
  '  Dim curve As LineItem
  '  curve = pGraphPane.AddCurve("Larry", x, y, Color.Green, SymbolType.Circle)
  '  curve.Line.Width = 1.5F
  '  curve.Line.Fill = New Fill(Color.White, Color.FromArgb(60, 190, 50), 90.0F)
  '  curve.Line.IsSmooth = True
  '  curve.Line.SmoothTension = 0.6F
  '  curve.Symbol.Fill = New Fill(Color.White)
  '  curve.Symbol.Size = 10
  '  Dim x3 As Double() = {150, 250, 400, 520, 780, 940}
  '  Dim y3 As Double() = {5.2, 49, 33.8, 88.57, 99.9, 36.8}
  '  curve = pGraphPane.AddCurve("Moe", x3, y3, Color.FromArgb(200, 55, 135), SymbolType.Triangle)
  '  curve.Line.Width = 1.5F
  '  curve.Symbol.Fill = New Fill(Color.White)
  '  curve.Line.Fill = New Fill(Color.White, Color.FromArgb(160, 230, 145, 205), 90.0F)
  '  curve.Symbol.Size = 10
  '  Dim x4 As Double() = {100, 200, 300, 400, 500, 600, 700, 800, 900, 1000}
  '  Dim y4 As Double() = {30, 45, 53, 60, 75, 83, 84, 79, 71, 57}
  '  Dim bar As BarItem = pGraphPane.AddBar("Wheezy", x4, y4, Color.SteelBlue)
  '  bar.Bar.Fill = New Fill(Color.RosyBrown, Color.White, Color.RosyBrown)
  '  pGraphPane.ClusterScaleWidth = 100
  '  pGraphPane.BarType = BarType.Stack
  '  Dim x2 As Double() = {100, 200, 300, 400, 500, 600, 700, 800, 900, 1000}
  '  Dim y2 As Double() = {10, 15, 17, 20, 25, 27, 29, 26, 24, 18}
  '  bar = pGraphPane.AddBar("Curly", x2, y2, Color.RoyalBlue)
  '  bar.Bar.Fill = New Fill(Color.RoyalBlue, Color.White, Color.RoyalBlue)
  '  pGraphPane.ClusterScaleWidth = 100
  '  pGraphPane.PaneFill = New Fill(Color.WhiteSmoke, Color.Lavender, 0.0F)
  '  pGraphPane.AxisFill = New Fill(Color.FromArgb(255, 255, 245), Color.FromArgb(255, 255, 190), 90.0F)
  '  pGraphPane.XAxis.IsShowGrid = True
  '  pGraphPane.YAxis.IsShowGrid = True
  '  pGraphPane.YAxis.Max = 120
  '  Dim text As TextItem = New TextItem("First Prod" & Microsoft.VisualBasic.Chr(10) & "21-Oct-93", 175.0F, 80.0F)
  '  text.Location.AlignH = AlignH.Center
  '  text.Location.AlignV = AlignV.Bottom
  '  text.FontSpec.Fill = New Fill(Color.White, Color.PowderBlue, 45.0F)
  '  pGraphPane.GraphItemList.Add(text)
  '  Dim arrow As ArrowItem = New ArrowItem(Color.Black, 12.0F, 175.0F, 77.0F, 100.0F, 45.0F)
  '  arrow.Location.CoordinateFrame = CoordType.AxisXYScale
  '  pGraphPane.GraphItemList.Add(arrow)
  '  text = New TextItem("Upgrade", 700.0F, 50.0F)
  '  text.FontSpec.Angle = 90
  '  text.FontSpec.FontColor = Color.Black
  '  text.Location.AlignH = AlignH.Right
  '  text.Location.AlignV = AlignV.Center
  '  text.FontSpec.Fill.IsVisible = False
  '  text.FontSpec.Border.IsVisible = False
  '  pGraphPane.GraphItemList.Add(text)
  '  arrow = New ArrowItem(Color.Black, 15, 700, 53, 700, 80)
  '  arrow.Location.CoordinateFrame = CoordType.AxisXYScale
  '  arrow.PenWidth = 2.0F
  '  pGraphPane.GraphItemList.Add(arrow)
  '  text = New TextItem("Confidential", 0.85F, -0.03F)
  '  text.Location.CoordinateFrame = CoordType.AxisFraction
  '  text.FontSpec.Angle = 15.0F
  '  text.FontSpec.FontColor = Color.Red
  '  text.FontSpec.IsBold = True
  '  text.FontSpec.Size = 16
  '  text.FontSpec.Border.IsVisible = False
  '  text.FontSpec.Border.Color = Color.Red
  '  text.FontSpec.Fill.IsVisible = False
  '  text.Location.AlignH = AlignH.Left
  '  text.Location.AlignV = AlignV.Bottom
  '  pGraphPane.GraphItemList.Add(text)
  '  Dim box As BoxItem = New BoxItem(New RectangleF(0, 110, 1200, 10), Color.Empty, Color.FromArgb(225, 245, 225))
  '  box.Location.CoordinateFrame = CoordType.AxisXYScale
  '  box.Location.AlignH = AlignH.Left
  '  box.Location.AlignV = AlignV.Top
  '  box.ZOrder = ZOrder.E_BehindAxis
  '  pGraphPane.GraphItemList.Add(box)
  '  text = New TextItem("Peak Range", 1170, 105)
  '  text.Location.CoordinateFrame = CoordType.AxisXYScale
  '  text.Location.AlignH = AlignH.Right
  '  text.Location.AlignV = AlignV.Center
  '  text.FontSpec.IsItalic = True
  '  text.FontSpec.IsBold = False
  '  text.FontSpec.Fill.IsVisible = False
  '  text.FontSpec.Border.IsVisible = False
  '  pGraphPane.GraphItemList.Add(text)
  '  pGraphPane.AxisChange(Me.CreateGraphics)
  'End Sub

  'Private Sub Graph_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
  '  SetSize()
  'End Sub

  'Private Sub Graph_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles MyBase.Paint
  '  'pGraphPane.Draw(e.Graphics)
  '  pMaster.Draw(e.Graphics)
  'End Sub

  'Private Sub Graph_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Resize
  '  SetSize()
  '  Invalidate()
  'End Sub

  'Private Sub SetSize()
  '  Dim paneRect As RectangleF = New RectangleF(Me.ClientRectangle.X, Me.ClientRectangle.Y, Me.ClientRectangle.Width, Me.ClientRectangle.Height)
  '  'Me.pGraphPane.PaneRect = paneRect
  '  zgc.MasterPane.PaneRect = paneRect
  '  zgc.MasterPane.AutoPaneLayout(Me.CreateGraphics, PaneLayout.SingleColumn)

  'End Sub

  Private Sub mnuFileAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFileAdd.Click
    Dim frmSelect As New atcData.frmSelectTimeseries
    Dim SelectedTS As ICollection = frmSelect.AskUser(pTimeseriesManager)
    For Each t As atcData.atcTimeseries In SelectedTS
      atcGraphTime.AddDatasetTimeseries(Me.Pane, t, t.ToString())
    Next
    zgc.AxisChange()
    Invalidate()
    Me.Refresh()
  End Sub

  Private Sub mnuFilePrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnuFilePrint.Click

  End Sub

  ''' <summary> Prints the displayed graph. </summary> 
  ''' <param name="sender"> Object raising this event. </param> 
  ''' <param name="e"> Event arguments passing graphics context to print to. </param> 
  'Private Sub docPrinter_PrintPage(ByVal sender As System.Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles docPrinter.PrintPage
  '  Dim sBuf As String

  '  ' Validate. 
  '  If (e Is Nothing) Then Return
  '  If (e.Graphics Is Nothing) Then Return

  '  ' Resize the graph to fit the printout. 
  '  ' Store current graph size into Tag so as to change it back at end of print job. 
  '  pGraphPane.Tag = pGraphPane.PaneRect
  '  With e.MarginBounds
  '    pGraphPane.ReSize(e.Graphics, New RectangleF(.X, .Y, .Width, .Height))
  '  End With

  '  ' Print the graph. 
  '  pGraphPane.Draw(e.Graphics)

  '  ' Setting this FALSE ends the print job. 
  '  e.HasMorePages = False
  'End Sub

  '''' <summary> Called at end of print job. Restores graph pane size. </summary> 
  '''' <param name="sender"> Object raising this event. </param> 
  '''' <param name="e"> Event arguments passing printer information. </param> 
  'Private Sub docPrinter_EndPrint()
  '  ' Restore graph size to fit form's bounds. 
  '  If (Not (pGraphPane.Tag Is Nothing)) Then
  '    pGraphPane.ReSize(chtMain.CreateGraphics, _
  '    DirectCast(pGraphPane.Tag, RectangleF))
  '    pGraphPane.Tag = Nothing
  '  End If
  'End Sub

  Private Sub pEditor_Apply() Handles pEditor.Apply
    zgc.AxisChange()
    Invalidate()
    Me.Refresh()
  End Sub

  Private Sub mnuEditCurves_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnuEditCurves.Click
    pEditor = New atcGraphEdit
    pEditor.Edit(zgc.GraphPane.CurveList(0))
  End Sub

  Private Sub mnuEditTitles_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnuEditTitles.Click
    pEditor = New atcGraphEdit
    pEditor.Edit(zgc.GraphPane)
  End Sub

  Private Sub mnuEditX_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnuEditX.Click
    pEditor = New atcGraphEdit
    pEditor.Edit(zgc.GraphPane.XAxis)
  End Sub

  Private Sub mnuEditY_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuEditY.Click
    pEditor = New atcGraphEdit
    pEditor.Edit(zgc.GraphPane.YAxis)
  End Sub
End Class
