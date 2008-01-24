Imports System.Drawing

Imports atcData
Imports atcUtility
Imports MapWinUtility
Imports ZedGraph

Public Class clsGraphRunningSum
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
            Dim lRunningSums As New atcDataGroup
            Dim lTsMath As atcDataSource = New atcTimeseriesMath.atcTimeseriesMath
            For Each lTimeseries As atcTimeseries In aDataGroup
                Dim lArgsMath As New atcDataAttributes
                lArgsMath.SetValue("timeseries", lTimeseries)
                If lTsMath.Open("running sum", lArgsMath) Then
                    lRunningSums.Add(lTsMath.DataSets.ItemByIndex(lTsMath.DataSets.Count - 1))
                Else
                    Logger.Msg("Running Sum Calculation Failed")
                End If
            Next
            MyBase.Datasets = lRunningSums
            pZgc.MasterPane.PaneList(0).XAxis.Title.Text = "Running Sums"
            AddTimeseriesCurves(Datasets, pZgc)
        End Set
    End Property
End Class
