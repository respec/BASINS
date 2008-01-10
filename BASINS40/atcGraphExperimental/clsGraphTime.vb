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
            Dim lYaxisName As String

            Dim lYaxisDataSets As New atcDataGroup
            Dim lY2axisDataSets As New atcDataGroup
            Dim lAuxAxisDataSets As New atcDataGroup

            For Each lTimeseries As atcTimeseries In newValue
                'AddDatasetCurve(lTimeseries)
                lYaxisName = FindYAxis(lTimeseries, pZgc, Datasets)
                Select Case lYaxisName.ToUpper
                    Case "AUX"
                        lAuxAxisDataSets.Add(lTimeseries)
                    Case "RIGHT"
                        lY2axisDataSets.Add(lTimeseries)
                    Case Else
                        lYaxisDataSets.Add(lTimeseries)
                End Select
                AddTimeseriesCurve(lTimeseries, pZgc, lYaxisName)
            Next
            If lYaxisDataSets.Count > 0 Then
                ScaleYAxis(lYaxisDataSets, pZgc.MasterPane.PaneList(pZgc.MasterPane.PaneList.Count - 1).YAxis)
            End If
            If lY2axisDataSets.Count > 0 Then
                ScaleYAxis(lYaxisDataSets, pZgc.MasterPane.PaneList(pZgc.MasterPane.PaneList.Count - 1).Y2Axis)
            End If
            If lAuxAxisDataSets.Count > 0 Then
                ScaleYAxis(lAuxAxisDataSets, pZgc.MasterPane.PaneList(0).YAxis)
            End If
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
            Dim lCurve As ZedGraph.CurveItem
            For Each lTs As atcTimeseries In aRemoved
                For Each lPane As GraphPane In MyBase.pZgc.MasterPane.PaneList
                    lCurve = lPane.CurveList.Item(TSCurveLabel(lTs))
                    If Not lCurve Is Nothing Then
                        lPane.CurveList.Remove(lCurve)
                    End If
                Next
            Next
            pZgc.Refresh()
        End If
    End Sub
End Class
