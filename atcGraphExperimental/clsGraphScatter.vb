Imports System.Drawing

Imports atcData
Imports atcUtility
Imports ZedGraph

Public Class clsGraphScatter
    Inherits clsGraphBase

    <CLSCompliant(False)> _
    Public Sub New(ByVal aDataGroup As atcDataGroup, ByVal aZedGraphControl As ZedGraphControl)
        MyBase.New(aDataGroup, aZedGraphControl)
    End Sub

    Public Overrides Property Datasets() As atcDataGroup
        Get
            Return MyBase.Datasets
        End Get
        Set(ByVal newValue As atcDataGroup)
            MyBase.Datasets = newValue
            If newValue.Count > 1 Then
                Dim lTimeseriesX As atcTimeseries = newValue.ItemByIndex(0)
                Dim lTimeseriesY As atcTimeseries = newValue.ItemByIndex(1)
                Dim lPane As GraphPane = MyBase.pZgc.MasterPane.PaneList(0)
                lPane.Legend.IsVisible = False
                With lPane.XAxis
                    .Type = AxisType.Linear
                    .Scale.MaxAuto = False
                    .Title.Text = lTimeseriesX.ToString
                End With

                With lPane.YAxis
                    .Type = AxisType.Linear
                    .Scale.MaxAuto = False
                    .Title.Text = lTimeseriesY.ToString
                End With

                With lTimeseriesY.Attributes
                    'Dim lScen As String = .GetValue("scenario")
                    'Dim lLoc As String = .GetValue("location")
                    'Dim lCons As String = .GetValue("constituent")
                    'Dim lCurveColor As Color = GetMatchingColor(lScen & ":" & lLoc & ":" & lCons)
                    Dim lCurveColor As Color = Color.Blue
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
                    lCurve = lPane.AddCurve("", lXValues, lYValues, lCurveColor, lSymbol)
                    If lNPts >= 1000 Then
                        lCurve.Symbol.Size = 1
                    ElseIf lNPts >= 100 Then
                        lCurve.Symbol.Size = 2
                    End If
                    lCurve.Line.IsVisible = False
                    lCurve.Tag = lTimeseriesY.ToString & " vs " & lTimeseriesX.ToString
                End With
                ScaleAxis(newValue, lPane.YAxis)
                lPane.XAxis.Scale.Min = lPane.YAxis.Scale.Min
                lPane.XAxis.Scale.Max = lPane.YAxis.Scale.Max
            End If
            pZgc.Refresh()
        End Set
    End Property

    Public Sub AddFitLine()
        '45 degree line
        Dim lPane As ZedGraph.GraphPane = pZgc.MasterPane.PaneList(0)
        AddLine(lPane, 1, 0, Drawing.Drawing2D.DashStyle.Dot, "45DegLine")
        'regression line 
        Dim lACoef As Double
        Dim lBCoef As Double
        Dim lRSquare As Double
        FitLine(Datasets(1), Datasets(0), lACoef, lBCoef, lRSquare)
        AddLine(lPane, lACoef, lBCoef, Drawing.Drawing2D.DashStyle.Solid, "RegLine")
        Dim lText As New TextObj
        Dim lFmt As String = "###,##0.###"
        lText.Text = "Y = " & DoubleToString(lACoef, , lFmt) & " X + " & DoubleToString(lBCoef, , lFmt) & Environment.NewLine & _
                     "R = " & DoubleToString(Math.Sqrt(lRSquare), , lFmt) & vbCrLf & _
                     "R Squared = " & DoubleToString(lRSquare, , lFmt)
        lText.FontSpec.StringAlignment = Drawing.StringAlignment.Near
        lText.Location = New Location(0.05, 0.05, CoordType.ChartFraction, AlignH.Left, AlignV.Top)
        lText.FontSpec.Border.IsVisible = False
        lPane.GraphObjList.Add(lText)
        lPane.XAxis.Title.Text &= vbCrLf & vbCrLf & "Scatter Plot"
    End Sub
End Class
