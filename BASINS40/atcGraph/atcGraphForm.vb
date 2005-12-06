Imports atcData
Imports atcUtility

Imports ZedGraph

Imports System
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Drawing.Imaging
Imports System.Collections
Imports System.ComponentModel
Imports System.IO
Imports System.Windows.Forms

Public Class atcGraphForm
  Inherits Form

  'Form object that contains graph(s)
  Private pMaster As ZedGraph.MasterPane

  'All the currently open files
  Private pDataManager As atcData.atcDataManager

  'Graph editing form
  Private WithEvents pEditor As atcGraphEdit

  'The group of atcData displayed
  Private WithEvents pDataGroup As atcDataGroup

  Private Shared SaveImageExtension As String = ".png"

  Private Sub InitMasterPane()
    pMaster = zgc.MasterPane
    pMaster.PaneList.Clear() 'remove default GraphPane
    'pMaster.PaneFill = New Fill(Color.White, Color.MediumSlateBlue, 45.0F)
    pMaster.Legend.IsVisible = False

    Dim myPane As GraphPane = New GraphPane

    'myPane.PaneFill = New Fill(Color.White, Color.LightYellow, 45.0F)
    With myPane
      With .XAxis
        .Type = ZedGraph.AxisType.Date
        '.MajorUnit = ZedGraph.DateUnit.Day
        '.MinorUnit = ZedGraph.DateUnit.Hour
        .Max = 0
        .Min = 100000
        .IsTic = False
        .IsMinorTic = False
        .IsInsideTic = True
        .IsMinorInsideTic = True
      End With
      With .YAxis
        .IsTic = False
        .IsMinorTic = False
        .IsInsideTic = True
        .IsMinorInsideTic = True
      End With
      With .Y2Axis
        .IsTic = False
        .IsMinorTic = False
        .IsInsideTic = True
        .IsMinorInsideTic = True
      End With
    End With
    pMaster.PaneList.Add(myPane)

    Dim g As Graphics = Me.CreateGraphics()
    pMaster.AutoPaneLayout(g, PaneLayout.SingleColumn)

    For Each ts As atcTimeseries In pDataGroup
      AddDatasetTimeseries(ts, ts.ToString)
      With Pane.XAxis
        If ts.Attributes.GetValue("point", False) Then
          If ts.Dates.Value(1) < .Min Then .Min = ts.Dates.Value(1)
        Else
          If ts.Dates.Value(0) < .Min Then .Min = ts.Dates.Value(0)
        End If
        If ts.Dates.Value(ts.Dates.numValues) > .Max Then .Max = ts.Dates.Value(ts.Dates.numValues)
      End With
    Next

    pMaster.AxisChange(g)
    Invalidate()
    g.Dispose()
  End Sub

#Region " Windows Form Designer generated code "

  Public Sub New(ByVal aDataManager As atcData.atcDataManager, _
        Optional ByVal aDataGroup As atcData.atcDataGroup = Nothing)
    MyBase.New()
    InitializeComponent() 'required by Windows Form Designer

    SetStyle(ControlStyles.DoubleBuffer Or ControlStyles.UserPaint Or ControlStyles.AllPaintingInWmPaint, True)

    pDataManager = aDataManager

    Dim lTempDataGroup As atcDataGroup = aDataGroup
    If aDataGroup Is Nothing Then lTempDataGroup = New atcDataGroup

    If lTempDataGroup.Count = 0 Then 'ask user to specify some Data
      pDataManager.UserSelectData(, lTempDataGroup, True)
    End If

    If lTempDataGroup.Count > 0 Then
      pDataGroup = lTempDataGroup 'Don't assign to pDataGroup too soon or it may slow down UserSelectData
      InitMasterPane()

      Dim DisplayPlugins As ICollection = pDataManager.GetPlugins(GetType(atcDataDisplay))
      For Each lDisp As atcDataDisplay In DisplayPlugins
        Dim lMenuText As String = lDisp.Name
        If lMenuText.StartsWith("Tools::") Then lMenuText = lMenuText.Substring(7)
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
  Friend WithEvents mnuFileAdd As System.Windows.Forms.MenuItem
  Friend WithEvents mnuFileSave As System.Windows.Forms.MenuItem
  Friend WithEvents mnuFilePrint As System.Windows.Forms.MenuItem
  Friend WithEvents mnuEdit As System.Windows.Forms.MenuItem
  Friend WithEvents mnuEditTitles As System.Windows.Forms.MenuItem
  Friend WithEvents mnuEditCurves As System.Windows.Forms.MenuItem
  Friend WithEvents mnuEditFont As System.Windows.Forms.MenuItem
  Friend WithEvents mnuAnalysis As System.Windows.Forms.MenuItem
  Public WithEvents zgc As ZedGraph.ZedGraphControl
  Friend WithEvents mnuEditY As System.Windows.Forms.MenuItem
  Friend WithEvents mnuEditX As System.Windows.Forms.MenuItem
  Friend WithEvents mnuEditY2 As System.Windows.Forms.MenuItem
  <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
    Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(atcGraphForm))
    Me.MainMenu1 = New System.Windows.Forms.MainMenu
    Me.mnuFile = New System.Windows.Forms.MenuItem
    Me.mnuFileAdd = New System.Windows.Forms.MenuItem
    Me.mnuFileSave = New System.Windows.Forms.MenuItem
    Me.mnuFilePrint = New System.Windows.Forms.MenuItem
    Me.mnuEdit = New System.Windows.Forms.MenuItem
    Me.mnuEditX = New System.Windows.Forms.MenuItem
    Me.mnuEditY = New System.Windows.Forms.MenuItem
    Me.mnuEditY2 = New System.Windows.Forms.MenuItem
    Me.mnuEditTitles = New System.Windows.Forms.MenuItem
    Me.mnuEditCurves = New System.Windows.Forms.MenuItem
    Me.mnuEditFont = New System.Windows.Forms.MenuItem
    Me.mnuAnalysis = New System.Windows.Forms.MenuItem
    Me.zgc = New ZedGraph.ZedGraphControl
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
    Me.mnuFileAdd.Text = "&Add Data"
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
    Me.mnuEdit.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuEditX, Me.mnuEditY, Me.mnuEditY2, Me.mnuEditTitles, Me.mnuEditCurves, Me.mnuEditFont})
    Me.mnuEdit.Text = "&Edit"
    '
    'mnuEditX
    '
    Me.mnuEditX.Index = 0
    Me.mnuEditX.Text = "&X Axis"
    '
    'mnuEditY
    '
    Me.mnuEditY.Index = 1
    Me.mnuEditY.Text = "&Y Axis"
    '
    'mnuEditY2
    '
    Me.mnuEditY2.Index = 2
    Me.mnuEditY2.Text = "Y&2 Axis"
    '
    'mnuEditTitles
    '
    Me.mnuEditTitles.Index = 3
    Me.mnuEditTitles.Text = "&Titles"
    '
    'mnuEditCurves
    '
    Me.mnuEditCurves.Index = 4
    Me.mnuEditCurves.Text = "&Curves"
    '
    'mnuEditFont
    '
    Me.mnuEditFont.Index = 5
    Me.mnuEditFont.Text = "&Font"
    '
    'mnuAnalysis
    '
    Me.mnuAnalysis.Index = 2
    Me.mnuAnalysis.Text = "&Analysis"
    '
    'zgc
    '
    Me.zgc.Dock = System.Windows.Forms.DockStyle.Fill
    Me.zgc.IsEnableHPan = True
    Me.zgc.IsEnableVPan = True
    Me.zgc.IsEnableZoom = True
    Me.zgc.IsShowContextMenu = True
    Me.zgc.IsShowHScrollBar = False
    Me.zgc.IsShowPointValues = False
    Me.zgc.IsShowVScrollBar = False
    Me.zgc.IsZoomOnMouseCenter = False
    Me.zgc.Location = New System.Drawing.Point(0, 0)
    Me.zgc.Name = "zgc"
    Me.zgc.PointDateFormat = "g"
    Me.zgc.PointValueFormat = "G"
    Me.zgc.ScrollMaxX = 0
    Me.zgc.ScrollMaxY = 0
    Me.zgc.ScrollMinX = 0
    Me.zgc.ScrollMinY = 0
    Me.zgc.Size = New System.Drawing.Size(543, 496)
    Me.zgc.TabIndex = 0
    Me.zgc.ZoomStepFraction = 0.1
    '
    'atcGraphForm
    '
    Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
    Me.ClientSize = New System.Drawing.Size(543, 496)
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

  Private Sub mnuFileAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFileAdd.Click
    pDataManager.UserSelectData(, pDataGroup, False)
  End Sub

  Private Sub mnuFileSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnuFileSave.Click
    SaveBitmapToFile()
  End Sub

  Public Sub SaveBitmapToFile(Optional ByVal aFileName As String = "")
    If aFileName.Length = 0 Then 'No file name specified - ask user
      Dim lSavedAs As String
      lSavedAs = zgc.SaveAs(SaveImageExtension)
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
      zgc.MasterPane.Image.Save(lStream.BaseStream, lFormat)
      lStream.Close()

    End If
  End Sub

  Private Sub mnuFilePrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnuFilePrint.Click
    Dim printdlg As New PrintDialog
    Dim printdoc As New Printing.PrintDocument
    AddHandler printdoc.PrintPage, AddressOf Me.PrintPage

    printdlg.Document = printdoc
    printdlg.AllowSelection = False
    printdlg.ShowHelp = True

    ' If the result is OK then print the document.
    If (printdlg.ShowDialog = DialogResult.OK) Then
      Dim saveRect As RectangleF = Pane.PaneRect
      printdoc.Print()
      ' Restore graph size to fit form's bounds. 
      Pane.ReSize(Me.CreateGraphics, saveRect)
    End If
  End Sub

  '' <summary> Prints the displayed graph. </summary> 
  '' <param name="sender"> Object raising this event. </param> 
  '' <param name="e"> Event arguments passing graphics context to print to. </param> 
  Private Sub PrintPage(ByVal sender As System.Object, ByVal e As Printing.PrintPageEventArgs)
    Dim sBuf As String

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

  Private Sub mnuEditY2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuEditY2.Click
    pEditor = New atcGraphEdit
    pEditor.Edit(zgc.GraphPane.Y2Axis)
  End Sub

  Public Sub AddDatasetTimeseries(ByVal t As atcTimeseries, ByVal CurveLabel As String)
    Dim lCons As String = t.Attributes.GetValue("constituent")
    Dim lOldCons As String

    Dim curveColor As Color = GetMatchingColor(t.Attributes.GetValue("scenario"))
    Dim curve As LineItem
    Dim lOldCurve As LineItem

    Dim y() As Double = t.Values
    Dim x() As Double = t.Dates.Values

    If t.Attributes.GetValue("point", False) Then
      curve = Pane.AddCurve(CurveLabel, x, y, curveColor, SymbolType.Star)
      curve.Line.IsVisible = False
    Else
      If UBound(y) > 0 Then
        y(0) = y(1)
        curve = Pane.AddCurve(CurveLabel, x, y, curveColor, SymbolType.None)
        curve.Line.Width = 1
        curve.Line.StepType = StepType.RearwardStep
      End If
    End If

    'TODO: label Y Axis

    'TODO: 3rd Y Axis above (for PREC)

    If Pane.CurveList.Count > 1 Then
      For Each ts As atcTimeseries In pDataGroup
        lOldCurve = Pane.CurveList.Item(ts.ToString)
        If Not lOldCurve Is Nothing Then
          lOldCons = ts.Attributes.GetValue("constituent")
          If lOldCons = lCons Then
            curve.IsY2Axis = lOldCurve.IsY2Axis
          Else
            curve.IsY2Axis = Not (lOldCurve.IsY2Axis)
          End If
          If curve.IsY2Axis Then
            With Pane.Y2Axis
              .IsOppositeTic = False
              .IsMinorOppositeTic = False
              .IsVisible = True
              .IsShowTitle = True
            End With
            With Pane.YAxis
              .IsOppositeTic = False
              .IsMinorOppositeTic = False
            End With
          End If
        End If
      Next
    End If

    'curve.Line.Fill = New Fill(Color.White, Color.FromArgb(60, 190, 50), 90.0F)
    'curve.Line.IsSmooth = True
    'curve.Line.SmoothTension = 0.6F
    'curve.Symbol.Fill = New Fill(Color.White)
    'curve.Symbol.Size = 10
    'curve.Symbol.IsVisible = False

  End Sub

  Private Sub pTimeseriesGroup_Added(ByVal aAdded As atcCollection) Handles pDataGroup.Added
    For Each ts As atcTimeseries In aAdded
      AddDatasetTimeseries(ts, ts.ToString)
    Next
    zgc.AxisChange()
    Invalidate()
    Me.Refresh()
  End Sub

  Private Sub pTimeseriesGroup_Removed(ByVal aRemoved As atcCollection) Handles pDataGroup.Removed
    For Each ts As atcTimeseries In aRemoved
      Pane.CurveList.Remove(Pane.CurveList.Item(ts.ToString))
    Next
    zgc.AxisChange()
    Invalidate()
    Me.Refresh()
  End Sub

  Private Sub mnuAnalysis_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnuAnalysis.Click
    pDataManager.ShowDisplay(sender.Text, pDataGroup)
  End Sub

  Protected Overrides Sub OnClosing(ByVal e As System.ComponentModel.CancelEventArgs)
    If Not pEditor Is Nothing Then
      pEditor.Close()
      pEditor = Nothing
    End If
    pMaster = Nothing
    pDataManager = Nothing
    pDataGroup = Nothing
  End Sub
End Class
