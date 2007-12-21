Imports System.Drawing

Imports atcData
Imports atcUtility
Imports ZedGraph

Public Module modGraph
    Friend Const DefaultAxisLabelFormat As String = "#,##0.###"
    Friend DefaultMajorGridColor As Color = Color.FromArgb(255, 225, 225, 225)
    Friend DefaultMinorGridColor As Color = Color.FromArgb(255, 245, 245, 245)

    <CLSCompliant(False)> _
    Public Sub AddLine(ByRef aPane As ZedGraph.GraphPane, _
                       ByVal aACoef As Double, ByVal aBCoef As Double, _
                       Optional ByVal aLineStyle As Drawing.Drawing2D.DashStyle = Drawing.Drawing2D.DashStyle.Solid)
        With aPane
            Dim lXValues(1) As Double
            Dim lYValues(1) As Double
            If aBCoef > 0 Then
                lXValues(0) = .XAxis.Scale.Min
                lYValues(0) = (aACoef * lXValues(0)) + aBCoef
            Else
                lYValues(0) = .YAxis.Scale.Min
                lXValues(0) = (lYValues(0) - aBCoef) / aACoef
            End If
            lXValues(1) = .XAxis.Scale.Max
            lYValues(1) = (aACoef * lXValues(1)) + aBCoef
            Dim lCurve As LineItem = .AddCurve("", lXValues, lYValues, Drawing.Color.Blue, SymbolType.None)
            lCurve.Line.Style = aLineStyle
            .CurveList.Add(lCurve)
        End With
    End Sub

    Public Sub SetGraphSpecs(ByRef aGraphForm As atcGraph.atcGraphForm, _
                             Optional ByRef aLabel1 As String = "Simulated", _
                             Optional ByRef aLabel2 As String = "Observed")
        aGraphForm.WindowState = Windows.Forms.FormWindowState.Maximized
        With aGraphForm.Pane
            .YAxis.MajorGrid.IsVisible = True
            .YAxis.MinorGrid.IsVisible = False
            .XAxis.MajorGrid.IsVisible = True
            With .CurveList(0)
                .Label.Text = aLabel1
                .Color = System.Drawing.Color.Red
            End With
            With .CurveList(1)
                .Label.Text = aLabel2
                .Color = System.Drawing.Color.Blue
            End With
        End With
        Windows.Forms.Application.DoEvents()
    End Sub

    <CLSCompliant(False)> _
    Public Sub FormatPaneWithDefaults(ByVal aPane As ZedGraph.GraphPane)
        With aPane
            .IsAlignGrids = True
            .IsFontsScaled = False
            .IsPenWidthScaled = False
            With .XAxis
                .MajorTic.IsOutside = False
                .MajorTic.IsInside = True
                .MajorTic.IsOpposite = True
                .MinorTic.IsOutside = False
                .MinorTic.IsInside = True
                .MinorTic.IsOpposite = True
                .Scale.Format = DefaultAxisLabelFormat
                With .MajorGrid
                    .Color = DefaultMajorGridColor
                    .DashOn = 0
                    .DashOff = 0
                    .IsVisible = True
                End With
                With .MinorGrid
                    .Color = DefaultMinorGridColor
                    .DashOn = 0
                    .DashOff = 0
                    .IsVisible = True
                End With
            End With
            With .X2Axis
                .IsVisible = False
            End With
            With .YAxis
                .MajorGrid.IsVisible = True
                .MajorTic.IsOutside = False
                .MajorTic.IsInside = True
                .MinorTic.IsOutside = False
                .MinorTic.IsInside = True
                .Scale.Format = DefaultAxisLabelFormat
                .Scale.Align = AlignP.Inside
                With .MajorGrid
                    .Color = DefaultMajorGridColor
                    .DashOn = 0
                    .DashOff = 0
                    .IsVisible = True
                End With
                With .MinorGrid
                    .Color = DefaultMinorGridColor
                    .DashOn = 0
                    .DashOff = 0
                    .IsVisible = True
                End With
            End With
            With .Y2Axis
                .MajorTic.IsOutside = False
                .MajorTic.IsInside = True
                .MinorTic.IsOutside = False
                .MinorTic.IsInside = True
                .Scale.Format = DefaultAxisLabelFormat
                .Scale.Align = AlignP.Inside
                With .MajorGrid
                    .Color = DefaultMajorGridColor
                    .DashOn = 0
                    .DashOff = 0
                    .IsVisible = True
                End With
                With .MinorGrid
                    .Color = DefaultMinorGridColor
                    .DashOn = 0
                    .DashOff = 0
                    .IsVisible = True
                End With
            End With
            With .Legend
                .Position = LegendPos.Float
                .Location = New Location(0.05, 0.05, CoordType.ChartFraction, AlignH.Left, AlignV.Top)
                .IsHStack = False
                .Border.IsVisible = False
            End With
            .Border.IsVisible = False
        End With
    End Sub
End Module
