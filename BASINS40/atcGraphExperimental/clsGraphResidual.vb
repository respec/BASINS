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
                    Dim lCommonScenario As String = aDataGroup.CommonAttributeValue("Scenario", "")
                    Dim lCommonLocation As String = aDataGroup.CommonAttributeValue("Location", "")
                    Dim lCommonConstituent As String = aDataGroup.CommonAttributeValue("Constituent", "")
                    Dim lCommonUnits As String = aDataGroup.CommonAttributeValue("Units", "")
                    Dim lCommonTimeUnits As Integer = aDataGroup.CommonAttributeValue("Time Units", 0)
                    Dim lCommonTimeStep As Integer = aDataGroup.CommonAttributeValue("Time Step", 0)
                    Dim lCommonTimeUnitName As String = TimeUnitName(lCommonTimeUnits, lCommonTimeStep)
                    For Each lTimeseries As atcTimeseries In Datasets
                        Dim lCurve As ZedGraph.CurveItem = AddTimeseriesCurve(lTimeseries, pZgc, FindYAxis(lTimeseries, pZgc, Datasets))
                        lCurve.Label.Text = TSCurveLabel(aDataGroup(0), lCommonTimeUnitName, lCommonScenario, lCommonConstituent, lCommonLocation, lCommonTimeUnits) _
                                  & " - " & TSCurveLabel(aDataGroup(1), lCommonTimeUnitName, lCommonScenario, lCommonConstituent, lCommonLocation, lCommonTimeUnits)
                    Next
                    ScaleAxis(Datasets, pZgc.MasterPane.PaneList(0).YAxis)
                    pZgc.MasterPane.PaneList(0).XAxis.Title.Text = "Residual"
                    AxisTitlesFromCommonAttributes(pZgc.MasterPane.PaneList(0), lCommonTimeUnitName, lCommonScenario, lCommonConstituent, lCommonLocation, lCommonUnits)
                    pZgc.Refresh()
                Else
                    Logger.Msg("Residual Graph Calculation Failed")
                End If
            End If
        End Set
    End Property
End Class
