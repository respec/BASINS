Imports System.Drawing

Imports atcData
Imports atcUtility

Imports ZedGraph

Public Class atcGraphTime

  Public Shared Sub AddDatasetTimeseries(ByVal pane As GraphPane, ByVal t As atcTimeseries, ByVal CurveLabel As String)
    'Dim x As Double() = t.Dates.Values
    'Dim y As Double() = t.Values
    Dim curveColor As Color = GetMatchingColor(t.Attributes.GetValue("scenario"))
    Dim curve As LineItem

    curve = pane.AddCurve(CurveLabel, t.Dates.Values, t.Values, curveColor, SymbolType.None)
    curve.Line.Width = 1
    curve.Line.StepType = StepType.ForwardStep
    If pane.CurveList.Count > 1 Then curve.IsY2Axis = True
    'curve.Line.Fill = New Fill(Color.White, Color.FromArgb(60, 190, 50), 90.0F)
    'curve.Line.IsSmooth = True
    'curve.Line.SmoothTension = 0.6F
    'curve.Symbol.Fill = New Fill(Color.White)
    'curve.Symbol.Size = 10
    'curve.Symbol.IsVisible = False

  End Sub

  'Public Sub Demo(ByVal pane As GraphPane)
  '  pane.Title = "Wacky Widget Company" & vbCr & "Production Report"
  '  pane.XAxis.Title = "Time, Days" & vbCr & "(Since Plant Construction Startup)"
  '  pane.YAxis.Title = "Widget Production" & vbCr & "(units/hour)"
  '  Dim x As Double() = {100, 200, 300, 400, 500, 600, 700, 800, 900, 1000}
  '  Dim y As Double() = {20, 10, 50, 25, 35, 75, 90, 40, 33, 50}
  '  Dim curve As LineItem
  '  curve = pane.AddCurve("Larry", x, y, Color.Green, SymbolType.Circle)
  '  curve.Line.Width = 1.5F
  '  curve.Line.Fill = New Fill(Color.White, Color.FromArgb(60, 190, 50), 90.0F)
  '  curve.Line.IsSmooth = True
  '  curve.Line.SmoothTension = 0.6F
  '  curve.Symbol.Fill = New Fill(Color.White)
  '  curve.Symbol.Size = 10
  '  Dim x3 As Double() = {150, 250, 400, 520, 780, 940}
  '  Dim y3 As Double() = {5.2, 49, 33.8, 88.57, 99.9, 36.8}
  '  curve = pane.AddCurve("Moe", x3, y3, Color.FromArgb(200, 55, 135), SymbolType.Triangle)
  '  curve.Line.Width = 1.5F
  '  curve.Symbol.Fill = New Fill(Color.White)
  '  curve.Line.Fill = New Fill(Color.White, Color.FromArgb(160, 230, 145, 205), 90.0F)
  '  curve.Symbol.Size = 10
  '  Dim x4 As Double() = {100, 200, 300, 400, 500, 600, 700, 800, 900, 1000}
  '  Dim y4 As Double() = {30, 45, 53, 60, 75, 83, 84, 79, 71, 57}
  '  Dim bar As BarItem = pane.AddBar("Wheezy", x4, y4, Color.SteelBlue)
  '  bar.Bar.Fill = New Fill(Color.RosyBrown, Color.White, Color.RosyBrown)
  '  pane.ClusterScaleWidth = 100
  '  pane.BarType = BarType.Stack
  '  Dim x2 As Double() = {100, 200, 300, 400, 500, 600, 700, 800, 900, 1000}
  '  Dim y2 As Double() = {10, 15, 17, 20, 25, 27, 29, 26, 24, 18}
  '  bar = pane.AddBar("Curly", x2, y2, Color.RoyalBlue)
  '  bar.Bar.Fill = New Fill(Color.RoyalBlue, Color.White, Color.RoyalBlue)
  '  pane.ClusterScaleWidth = 100
  '  pane.PaneFill = New Fill(Color.WhiteSmoke, Color.Lavender, 0.0F)
  '  pane.AxisFill = New Fill(Color.FromArgb(255, 255, 245), Color.FromArgb(255, 255, 190), 90.0F)
  '  pane.XAxis.IsShowGrid = True
  '  pane.YAxis.IsShowGrid = True
  '  pane.YAxis.Max = 120
  '  Dim text As TextItem = New TextItem("First Prod" & Microsoft.VisualBasic.Chr(10) & "21-Oct-93", 175.0F, 80.0F)
  '  text.Location.AlignH = AlignH.Center
  '  text.Location.AlignV = AlignV.Bottom
  '  text.FontSpec.Fill = New Fill(Color.White, Color.PowderBlue, 45.0F)
  '  pane.GraphItemList.Add(text)
  '  Dim arrow As ArrowItem = New ArrowItem(Color.Black, 12.0F, 175.0F, 77.0F, 100.0F, 45.0F)
  '  arrow.Location.CoordinateFrame = CoordType.AxisXYScale
  '  pane.GraphItemList.Add(arrow)
  '  text = New TextItem("Upgrade", 700.0F, 50.0F)
  '  text.FontSpec.Angle = 90
  '  text.FontSpec.FontColor = Color.Black
  '  text.Location.AlignH = AlignH.Right
  '  text.Location.AlignV = AlignV.Center
  '  text.FontSpec.Fill.IsVisible = False
  '  text.FontSpec.Border.IsVisible = False
  '  pane.GraphItemList.Add(text)
  '  arrow = New ArrowItem(Color.Black, 15, 700, 53, 700, 80)
  '  arrow.Location.CoordinateFrame = CoordType.AxisXYScale
  '  arrow.PenWidth = 2.0F
  '  pane.GraphItemList.Add(arrow)
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
  '  pane.GraphItemList.Add(text)
  '  Dim box As BoxItem = New BoxItem(New RectangleF(0, 110, 1200, 10), Color.Empty, Color.FromArgb(225, 245, 225))
  '  box.Location.CoordinateFrame = CoordType.AxisXYScale
  '  box.Location.AlignH = AlignH.Left
  '  box.Location.AlignV = AlignV.Top
  '  box.ZOrder = ZOrder.E_BehindAxis
  '  pane.GraphItemList.Add(box)
  '  text = New TextItem("Peak Range", 1170, 105)
  '  text.Location.CoordinateFrame = CoordType.AxisXYScale
  '  text.Location.AlignH = AlignH.Right
  '  text.Location.AlignV = AlignV.Center
  '  text.FontSpec.IsItalic = True
  '  text.FontSpec.IsBold = False
  '  text.FontSpec.Fill.IsVisible = False
  '  text.FontSpec.Border.IsVisible = False
  '  pane.GraphItemList.Add(text)
  'End Sub

End Class
