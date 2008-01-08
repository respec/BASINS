Imports System.Drawing

Imports atcData
Imports atcUtility
Imports MapWinUtility
Imports ZedGraph

Public Class clsGraphCumulativeDifference
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
            If aDataGroup.Count = 2 Then
                Dim lArgsMath As New atcDataAttributes
                Dim lTsMath As atcDataSource = New atcTimeseriesMath.atcTimeseriesMath
                lTsMath.DataSets.Clear()
                lArgsMath.Clear()
                lArgsMath.SetValue("timeseries", aDataGroup)
                If lTsMath.Open("subtract", lArgsMath) Then
                    lArgsMath.Clear()
                    lArgsMath.SetValue("timeseries", lTsMath.DataSets)
                    Dim lTsRunSum As atcDataSource = New atcTimeseriesMath.atcTimeseriesMath
                    If lTsRunSum.Open("running sum", lArgsMath) Then
                        MyBase.Datasets = lTsRunSum.DataSets
                        For Each lTimeseries As atcTimeseries In Datasets
                            Dim lCurve As ZedGraph.CurveItem = AddTimeseriesCurve(lTimeseries, pZgc, FindYAxis(lTimeseries, pZgc, Datasets))
                            lCurve.Label.Text = "Cummulative Difference"
                        Next
                    Else
                        Logger.Msg("CumulativeDifferenceGraph Accumulation Calculation Failed")
                    End If
                Else
                    Logger.Msg("CumulativeDifferenceGraph Difference Calculation Failed")
                End If
            Else
                Logger.Msg("Cumulative Difference requires 2 timeseries, " & aDataGroup.Count & " specified")
            End If
        End Set
    End Property
End Class
