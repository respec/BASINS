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
                       Optional ByVal aLineStyle As Drawing.Drawing2D.DashStyle = Drawing.Drawing2D.DashStyle.Solid, _
                       Optional ByVal aTag As String = Nothing)
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
            lCurve.Tag = aTag
            '.CurveList.Add(lCurve)
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

    <CLSCompliant(False)> _
    Public Sub ScaleYAxis(ByVal aDataGroup As atcDataGroup, ByVal aYAxis As YAxis)
        Dim lDataMin As Double = 1.0E+30
        Dim lDataMax As Double = -1.0E+30
        Dim lLogFlag As Boolean = False
        If aYAxis.Type = ZedGraph.AxisType.Log Then
            lLogFlag = True
        End If

        For Each lTimeseries As atcTimeseries In aDataGroup
            Dim lValue As Double = lTimeseries.Attributes.GetValue("Minimum")
            If lValue < lDataMin Then lDataMin = lValue
            lValue = lTimeseries.Attributes.GetValue("Maximum")
            If lValue > lDataMax Then lDataMax = lValue
        Next
        Scalit(lDataMin, lDataMax, lLogFlag, aYAxis.Scale.Min, aYAxis.Scale.Max)
    End Sub

    ''' <summary>
    ''' Determines an appropriate scale based on the minimum and maximum values and 
    ''' whether an arithmetic, probability, or logarithmic scale is requested. 
    ''' Minimum and maximum for probability plots must be standard deviates. 
    ''' For log scales, the minimum and maximum must not be transformed.
    ''' </summary>
    ''' <param name="aDataMin"></param>
    ''' <param name="aDataMax"></param>
    ''' <param name="aLogScale"></param>
    ''' <param name="aScaleMin"></param>
    ''' <param name="aScaleMax"></param>
    ''' <remarks></remarks>
    Public Sub Scalit(ByVal aDataMin As Double, ByVal aDataMax As Double, ByVal aLogScale As Boolean, _
                      ByRef aScaleMin As Double, ByRef aScaleMax As Double)
        'TODO: should existing ScaleMin and ScaleMax be respected?
        If Not aLogScale Then 'arithmetic scale
            'get next lowest mult of 10
            Static lRange(15) As Double
            If lRange(1) < 0.09 Then 'need to initialze
                lRange(1) = 0.1
                lRange(2) = 0.15
                lRange(3) = 0.2
                lRange(4) = 0.4
                lRange(5) = 0.5
                lRange(6) = 0.6
                lRange(7) = 0.8
                lRange(8) = 1.0#
                lRange(9) = 1.5
                lRange(10) = 2.0#
                lRange(11) = 4.0#
                lRange(12) = 5.0#
                lRange(13) = 6.0#
                lRange(14) = 8.0#
                lRange(15) = 10.0#
            End If

            Dim lRangeIndex As Integer
            Dim lRangeInc As Integer
            Dim lDataRndlow As Double = Rndlow(aDataMax)
            If lDataRndlow > 0.0# Then
                lRangeInc = 1
                lRangeIndex = 1
            Else
                lRangeInc = -1
                lRangeIndex = 15
            End If
            Do
                aScaleMax = lRange(lRangeIndex) * lDataRndlow
                lRangeIndex += lRangeInc
            Loop While aDataMax > aScaleMax And lRangeIndex <= 15 And lRangeIndex >= 1

            If aDataMin < 0.5 * aDataMax And aDataMin >= 0.0# And aDataMin = 1 Then
                aScaleMin = 0.0#
            Else
                'get next lowest mult of 10
                lDataRndlow = Rndlow(aDataMin)
                If lDataRndlow >= 0.0# Then
                    lRangeInc = -1
                    lRangeIndex = 15
                Else
                    lRangeInc = 1
                    lRangeIndex = 1
                End If
                Do
                    aScaleMin = lRange(lRangeIndex) * lDataRndlow
                    lRangeIndex += lRangeInc
                Loop While aDataMin < aScaleMin And lRangeIndex >= 1 And lRangeIndex <= 15
            End If
        Else 'logarithmic scale
            Dim lLogMin As Integer
            If aDataMin > 0.000000001 Then
                lLogMin = Fix(Log10(aDataMin))
            Else
                'too small or neg value, set to -9
                lLogMin = -9
            End If
            If aDataMin < 1.0# Then
                lLogMin -= 1
            End If
            aScaleMin = 10.0# ^ lLogMin

            Dim lLogMax As Integer
            If aDataMax > 0.000000001 Then
                lLogMax = Fix(Log10(aDataMax))
            Else
                'too small or neg value, set to -8
                lLogMax = -8
            End If
            If aDataMax > 1.0# Then
                lLogMax += 1
            End If
            aScaleMax = 10.0# ^ lLogMax

            If aScaleMin * 10000000.0# < aScaleMax Then
                'limit range to 7 cycles
                aScaleMin = aScaleMax / 10000000.0
            End If
        End If
    End Sub
End Module
