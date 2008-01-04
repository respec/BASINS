Imports System.Drawing

Imports atcData
Imports atcUtility
Imports ZedGraph

Public Class clsGraphTime
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
            For Each lTimeseries As atcTimeseries In newValue
                'AddDatasetCurve(lTimeseries)
                AddTimeseriesCurve(lTimeseries, pZgc, FindYAxis(lTimeseries, pZgc, Datasets))
            Next
            'TODO: is this the spot to do this?
            ScaleYAxis(newValue, pZgc.MasterPane.PaneList(0).YAxis)
            pZgc.Refresh()
        End Set
    End Property

    Overrides Sub pDataGroup_Added(ByVal aAdded As atcUtility.atcCollection)
        For Each lTimeseries As atcTimeseries In aAdded
            'AddDatasetCurve(lTimeseries)
            AddTimeseriesCurve(lTimeseries, pZgc, FindYAxis(lTimeseries, pZgc, Datasets))
        Next
        pZgc.Refresh()
    End Sub

    Overrides Sub pDataGroup_Removed(ByVal aRemoved As atcUtility.atcCollection)
        If Not MyBase.ZedGraphCtrl Is Nothing AndAlso _
           Not MyBase.ZedGraphCtrl.MasterPane Is Nothing Then
            Dim lCurveList As ZedGraph.CurveList = MyBase.pZgc.MasterPane.PaneList.Item(0).CurveList
            Dim lCurveListAux As ZedGraph.CurveList = MyBase.pZgc.MasterPane.PaneList.Item(1).CurveList
            If MyBase.pZgc.MasterPane.PaneList.Count > 1 Then
                lCurveListAux = MyBase.pZgc.MasterPane.PaneList.Item(1).CurveList
            End If
            Dim lCurve As ZedGraph.CurveItem
            For Each lTs As atcTimeseries In aRemoved
                lCurve = lCurveList.Item(clsGraphTime.TSCurveLabel(lTs))
                If lCurve Is Nothing Then
                    lCurve = lCurveListAux.Item(clsGraphTime.TSCurveLabel(lTs))
                    If Not lCurve Is Nothing Then
                        lCurveListAux.Remove(lCurve)
                    End If
                Else
                    lCurveList.Remove(lCurve)
                End If
            Next
            pZgc.Refresh()
        End If
    End Sub

    'Moved to modGraph.AddTimeseriesCurve + FindYAxis 
    'Protected Sub AddDatasetCurve(ByVal aTimeseries As atcTimeseries)
    '    Dim lScen As String = aTimeseries.Attributes.GetValue("scenario")
    '    Dim lLoc As String = aTimeseries.Attributes.GetValue("location")
    '    Dim lCons As String = aTimeseries.Attributes.GetValue("constituent")
    '    Dim lCurveLabel As String = TSCurveLabel(aTimeseries)
    '    Dim lCurveColor As Color = GetMatchingColor(lScen & ":" & lLoc & ":" & lCons)

    '    Dim lCurve As LineItem = Nothing
    '    Dim lOldCons As String
    '    Dim lOldCurve As LineItem
    '    Dim lPane As GraphPane = MyBase.pZgc.MasterPane.PaneList(0)
    '    Dim lYAxis As Axis = lPane.YAxis
    '    Dim lYAxisName As String = aTimeseries.Attributes.GetValue("YAxis", "")
    '    If lYAxisName.Length = 0 Then 'Does not have a pre-assigned axis
    '        'Use the same Y axis as existing curve with this constituent
    '        Dim lFoundMatchingCons As Boolean = False
    '        For Each lTs As atcTimeseries In pDataGroup
    '            lOldCurve = lPane.CurveList.Item(TSCurveLabel(lTs))
    '            If Not lOldCurve Is Nothing Then
    '                lOldCons = lTs.Attributes.GetValue("constituent")
    '                If lOldCons = lCons Then
    '                    If lOldCurve.IsY2Axis Then lYAxisName = "RIGHT" Else lYAxisName = "LEFT"
    '                    lFoundMatchingCons = True
    '                    Exit For
    '                End If
    '            End If
    '        Next
    '        If Not lFoundMatchingCons AndAlso lPane.CurveList.Count > 0 Then
    '            'Put new curve on right axis if we already have a non-matching curve
    '            lYAxisName = "Right"
    '        End If
    '    End If
    '    Select Case lYAxisName.ToUpper
    '        'Case "AUX"
    '        '    aGraphForm.AuxAxisEnabled = True
    '        '    lPane = aGraphForm.PaneAux
    '        '    lYAxis = aGraphForm.PaneAux.YAxis
    '        Case "RIGHT"
    '            lYAxis = lPane.Y2Axis
    '            With lPane.YAxis
    '                .MajorTic.IsOpposite = False
    '                .MinorTic.IsOpposite = False
    '            End With
    '            With lYAxis
    '                .MajorTic.IsOpposite = False
    '                .MinorTic.IsOpposite = False
    '            End With
    '    End Select

    '    lYAxis.IsVisible = True

    '    With lPane
    '        If .XAxis.Type <> AxisType.DateDual Then .XAxis.Type = AxisType.DateDual
    '        If aTimeseries.Attributes.GetValue("point", False) Then
    '            lCurve = .AddCurve(lCurveLabel, New atcTimeseriesPointList(aTimeseries), lCurveColor, SymbolType.Plus)
    '            lCurve.Line.IsVisible = False
    '        Else
    '            lCurve = .AddCurve(lCurveLabel, New atcTimeseriesPointList(aTimeseries), lCurveColor, SymbolType.None)
    '            lCurve.Line.Width = 1
    '            lCurve.Line.StepType = StepType.RearwardStep
    '        End If

    '        If lYAxisName.ToUpper.Equals("RIGHT") Then
    '            lCurve.IsY2Axis = True
    '        End If

    '        'Use units as Y axis title (if this data has units and Y axis title is not set)
    '        If aTimeseries.Attributes.ContainsAttribute("Units") AndAlso _
    '           (lYAxis.Title Is Nothing OrElse lYAxis.Title.Text Is Nothing OrElse lYAxis.Title.Text.Length = 0) Then
    '            lYAxis.Title.Text = aTimeseries.Attributes.GetValue("Units")
    '            lYAxis.Title.IsVisible = True
    '        End If

    '        Dim lSJDay As Double = aTimeseries.Attributes.GetValue("SJDay")
    '        Dim lEJDay As Double = aTimeseries.Attributes.GetValue("EJDay")
    '        If .CurveList.Count = 1 Then
    '            If aTimeseries.numValues > 0 Then 'Set X axis to contain this date range
    '                .XAxis.Scale.Min = lSJDay
    '                .XAxis.Scale.Max = lEJDay
    '            End If
    '        ElseIf .CurveList.Count > 1 AndAlso Not lCurve Is Nothing Then
    '            'Expand time scale if needed to include all dates in new curve
    '            If aTimeseries.numValues > 0 Then
    '                If lSJDay < .XAxis.Scale.Min Then
    '                    .XAxis.Scale.Min = lSJDay
    '                End If
    '                If lEJDay > .XAxis.Scale.Max Then
    '                    .XAxis.Scale.Max = lEJDay
    '                End If
    '            End If
    '        End If
    '    End With
    'End Sub

    Public Shared Function TSCurveLabel(ByVal aTimeseries As atcTimeseries) As String
        With aTimeseries.Attributes
            Return .GetValue("scenario") & " " & .GetValue("constituent") & " at " & .GetValue("location")
        End With
    End Function

End Class
