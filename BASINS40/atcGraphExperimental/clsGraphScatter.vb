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
                With lPane.XAxis
                    .Type = AxisType.Linear
                    .Scale.Min = lTimeseriesX.Attributes.GetValue("Min", 0)
                    .Scale.Min = lTimeseriesX.Attributes.GetValue("Max", 1000)
                End With

                With lPane.YAxis
                    .Type = AxisType.Linear
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
                    lCurve = lPane.AddCurve(clsGraphTime.TSCurveLabel(lTimeseriesY), lXValues, lYValues, lCurveColor, lSymbol)
                    If lNPts >= 1000 Then
                        lCurve.Symbol.Size = 1
                    ElseIf lNPts >= 100 Then
                        lCurve.Symbol.Size = 2
                    End If
                    lCurve.Line.IsVisible = False
                End With
            End If
        End Set
    End Property

End Class
