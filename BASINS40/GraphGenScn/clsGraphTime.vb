Imports System.Drawing

Imports atcData
Imports atcUtility
Imports ZedGraph

Public Class clsGraphTime
    Inherits clsGraphBase

    <CLSCompliant(False)> _
    Public Sub New(ByVal aDataGroup As atcTimeseriesGroup, ByVal aZedGraphControl As ZedGraphControl)
        MyBase.New(aDataGroup, aZedGraphControl)
    End Sub

    Public Overrides Property Datasets() As atcTimeseriesGroup
        Get
            Return MyBase.Datasets
        End Get
        Set(ByVal newValue As atcTimeseriesGroup)
            MyBase.Datasets = newValue
            AddTimeseriesCurves(newValue, pZgc)
            pZgc.Refresh()
        End Set
    End Property

    'Overrides Sub pDataGroup_Added(ByVal aAdded As atcUtility.atcCollection)
    '    For Each lTimeseries As atcTimeseries In aAdded
    '        'AddDatasetCurve(lTimeseries)
    '        AddTimeseriesCurve(lTimeseries, pZgc, FindYAxis(lTimeseries, pZgc, Datasets))
    '    Next
    '    pZgc.Refresh()
    'End Sub

    'Overrides Sub pDataGroup_Removed(ByVal aRemoved As atcUtility.atcCollection)
    '    If Not MyBase.ZedGraphCtrl Is Nothing AndAlso _
    '       Not MyBase.ZedGraphCtrl.MasterPane Is Nothing Then
    '        Dim lCurve As ZedGraph.CurveItem
    '        For Each lTs As atcTimeseries In aRemoved
    '            For Each lPane As GraphPane In MyBase.pZgc.MasterPane.PaneList
    '                lCurve = lPane.CurveList.Item(TSCurveLabel(lTs))
    '                If Not lCurve Is Nothing Then
    '                    lPane.CurveList.Remove(lCurve)
    '                End If
    '            Next
    '        Next
    '        pZgc.Refresh()
    '    End If
    'End Sub
End Class
