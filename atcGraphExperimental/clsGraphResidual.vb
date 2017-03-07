Imports System.Drawing

Imports atcData
Imports atcUtility
Imports MapWinUtility
Imports ZedGraph

Public Class clsGraphResidual
    Inherits clsGraphBase

    <CLSCompliant(False)> _
    Public Sub New(ByVal aDataGroup As atcTimeseriesGroup, ByVal aZedGraphControl As ZedGraphControl)
        MyBase.New(aDataGroup, aZedGraphControl)
    End Sub

    Public Overrides Property Datasets() As atcTimeseriesGroup
        Get
            Return MyBase.Datasets
        End Get
        Set(ByVal aDataGroup As atcTimeseriesGroup)
            If aDataGroup.Count <> 2 Then
                Logger.Msg("Residual Graph requires 2 timeseries, " & aDataGroup.Count & " specified")
            Else
                Dim lArgsMath As New atcDataAttributes
                lArgsMath.SetValue("timeseries", aDataGroup)
                Dim lTsMath As atcTimeseriesSource = New atcTimeseriesMath.atcTimeseriesMath
                If lTsMath.Open("subtract", lArgsMath) Then
                    MyBase.Datasets = lTsMath.DataSets
                    Dim lCommonScenario As String = aDataGroup.CommonAttributeValue("Scenario", "")
                    Dim lCommonLocation As String = aDataGroup.CommonAttributeValue("Location", "")
                    Dim lCommonConstituent As String = aDataGroup.CommonAttributeValue("Constituent", "")
                    Dim lCommonUnits As String = aDataGroup.CommonAttributeValue("Units", "")
                    Dim lCommonTimeUnit As Integer = aDataGroup.CommonAttributeValue("Time Unit", 0)
                    Dim lCommonTimeStep As Integer = aDataGroup.CommonAttributeValue("Time Step", 0)
                    Dim lCommonTimeUnitName As String = TimeUnitName(lCommonTimeUnit, lCommonTimeStep)
                    For Each lTimeseries As atcTimeseries In Datasets
                        'Dim lCurve As ZedGraph.CurveItem = AddTimeseriesCurve(lTimeseries, pZgc, FindYAxis(lTimeseries, pZgc, Datasets))
                        'lCurve.Label.Text = TSCurveLabel(aDataGroup(0), lCommonTimeUnitName, lCommonScenario, lCommonConstituent, lCommonLocation, lCommonTimeUnit) _
                        '          & " - " & TSCurveLabel(aDataGroup(1), lCommonTimeUnitName, lCommonScenario, lCommonConstituent, lCommonLocation, lCommonTimeUnit)

                        Dim lCurveDict As Generic.Dictionary(Of String, ZedGraph.CurveItem) = AddTimeseriesCurve(lTimeseries, pZgc, FindYAxis(lTimeseries, pZgc, Datasets))
                        Dim lMiscText As String = ""
                        For Each lKey As String In lCurveDict.Keys
                            If lKey = "provisional" Then
                                lMiscText = "Provisional"
                            ElseIf lKey = "nonprovisional" Then
                                lMiscText = ""
                            End If
                            lCurveDict.Item(lKey).Label.Text = TSCurveLabel(aDataGroup(0), lCommonTimeUnitName, lCommonScenario, lCommonConstituent, lCommonLocation, lCommonTimeUnit) _
                                      & " - " & TSCurveLabel(aDataGroup(1), lCommonTimeUnitName, lCommonScenario, lCommonConstituent, lCommonLocation, lCommonTimeUnit, lMiscText)
                        Next
                        lCurveDict.Clear()
                        lCurveDict = Nothing
                    Next
                    ScaleAxis(Datasets, pZgc.MasterPane.PaneList(0).YAxis)
                    pZgc.MasterPane.PaneList(0).XAxis.Title.Text = "Residual"

                    Dim lSourceFile1 As String = ""
                    If aDataGroup(0).Attributes.ContainsAttribute("Data Source") Then
                        lSourceFile1 = aDataGroup(0).Attributes.GetValue("Data Source")
                    End If
                    If Not IO.File.Exists(lSourceFile1) And aDataGroup(0).Attributes.GetValue("History 1").Length > 9 Then
                        'see if the history attribute contains a file name
                        lSourceFile1 = aDataGroup(0).Attributes.GetValue("History 1").Substring(10)
                    End If
                    Dim lSourceFile2 As String = ""
                    If aDataGroup(1).Attributes.ContainsAttribute("Data Source") Then
                        lSourceFile2 = aDataGroup(1).Attributes.GetValue("Data Source")
                    End If
                    If Not IO.File.Exists(lSourceFile2) And aDataGroup(1).Attributes.GetValue("History 1").Length > 9 Then
                        'see if the history attribute contains a file name
                        lSourceFile2 = aDataGroup(1).Attributes.GetValue("History 1").Substring(10)
                    End If
                    pZgc.MasterPane.PaneList(0).CurveList(0).Tag = aDataGroup(0).Serial & "|" & aDataGroup(0).Attributes.GetValue("ID") & "|" & lSourceFile1 & "||" &
                                                                   aDataGroup(1).Serial & "|" & aDataGroup(1).Attributes.GetValue("ID") & "|" & lSourceFile2

                    AxisTitlesFromCommonAttributes(pZgc.MasterPane.PaneList(0), lCommonTimeUnitName, lCommonScenario, lCommonConstituent, lCommonLocation, lCommonUnits)
                    pZgc.Refresh()
                Else
                    Logger.Msg("Residual Graph Calculation Failed")
                End If
            End If
        End Set
    End Property
End Class
