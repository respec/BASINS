Imports System.Drawing

Imports atcData
Imports atcUtility
Imports MapWinUtility
Imports ZedGraph

Public Class clsGraphResidual
    Inherits clsGraphBase

    <CLSCompliant(False)> _
    Public Sub New(ByVal aDataGroup As atcDataGroup, ByVal aZedGraphControl As ZedGraphControl)
        MyBase.New(aDataGroup, aZedGraphControl)
    End Sub

    Public Overrides Property Datasets() As atcDataGroup
        Get
            Return MyBase.Datasets
        End Get
        Set(ByVal aDataGroup As atcDataGroup)
            If aDataGroup.Count <> 2 Then
                Logger.Msg("Residual Graph requires 2 timeseries, " & aDataGroup.Count & " specified")
            Else
                Dim lArgsMath As New atcDataAttributes
                lArgsMath.SetValue("timeseries", aDataGroup)
                Dim lTsMath As atcDataSource = New atcTimeseriesMath.atcTimeseriesMath
                If lTsMath.Open("subtract", lArgsMath) Then
                    MyBase.Datasets = lTsMath.DataSets
                    For Each lTimeseries As atcTimeseries In Datasets
                        Dim lCurve As ZedGraph.CurveItem = AddTimeseriesCurve(lTimeseries, pZgc, FindYAxis(lTimeseries, pZgc, Datasets))
                        lCurve.Label.Text = "Residual"
                    Next
                    ScaleYAxis(Datasets, pZgc.MasterPane.PaneList(0).YAxis)
                    pZgc.Refresh()
                Else
                    Logger.Msg("Residual Graph Calculation Failed")
                End If
            End If
        End Set
    End Property
End Class
