Imports System
Imports atcUtility
Imports atcData
Imports MapWinUtility
Imports atcGraph
Imports ZedGraph

Public Module Graph
    ' aTimeSeries keys: "Observed", "Simulated", "Precipitation",
    '                   "LZS", "UZS", "PotET", "ActET", 
    '                   "Baseflow", "Interflow", "Surface"
    Public Sub GraphAll(ByVal aSDateJ As Double, ByVal aEDateJ As Double, _
                        ByVal aCons As String, ByVal aSite As String, _
                        ByVal aTimeSeries As atcCollection, _
                        ByVal aGraphSaveFormat As String, ByVal aGraphAnnual As Boolean)
        Dim lDataGroup As New atcDataGroup
        lDataGroup.Add(Aggregate(SubsetByDate(aTimeSeries.ItemByKey("Observed"), _
                                              aSDateJ, _
                                              aEDateJ, Nothing), _
                                 atcTimeUnit.TUDay, 1, atcTran.TranAverSame, Nothing))
        lDataGroup.Add(Aggregate(SubsetByDate(aTimeSeries.ItemByKey("Simulated"), _
                                              aSDateJ, _
                                              aEDateJ, Nothing), _
                                 atcTimeUnit.TUDay, 1, atcTran.TranAverSame, Nothing))

        Dim lOutFileBase As String = "outfiles\" & aCons & "_" & aSite
        Dim lZgc As ZedGraphControl

        'duration plot
        lZgc = CreateZgc()
        Dim lGraphDur As New clsGraphProbability(lDataGroup, lZgc)
        'TODO: add title 
        lZgc.SaveIn(lOutFileBase & "_dur" & aGraphSaveFormat)
        lGraphDur.Dispose()
        lZgc.Dispose()

        'cummulative difference
        lZgc = CreateZgc()
        Dim lGraphCum As New clsGraphCumulativeDifference(lDataGroup, lZgc)
        'TODO: add title 
        lZgc.SaveIn(lOutFileBase & "_cumDif" & aGraphSaveFormat)
        lGraphCum.Dispose()
        lZgc.Dispose()

        'scatter
        lZgc = CreateZgc()
        Dim lGraphScatter As New clsGraphScatter(lDataGroup, lZgc)
        lGraphScatter.AddFitLine()

        lZgc.SaveIn(lOutFileBase & "_scatDay" & aGraphSaveFormat)
        With lZgc.MasterPane.PaneList(0)
            .YAxis.Type = AxisType.Log
            .XAxis.Type = AxisType.Log
        End With
        lZgc.SaveIn(lOutFileBase & "_scatDay_log" & aGraphSaveFormat)
        lGraphScatter.Dispose()
        lZgc.Dispose()

        If aTimeSeries.IndexFromKey("LZS") > -1 Then 'scatter - LZS vs Error(cfs)
            lZgc = CreateZgc()
            If GraphScatterError(lZgc, lDataGroup, aSDateJ, aEDateJ, aTimeSeries.ItemByKey("LZS"), "LZS (in)") Then
                lZgc.SaveIn(lOutFileBase & "_Error_LZS" & aGraphSaveFormat)
            End If
        End If

        If aTimeSeries.IndexFromKey("UZS") > -1 Then 'scatter - UZS vs Error(cfs)
            lZgc = CreateZgc()
            If GraphScatterError(lZgc, lDataGroup, aSDateJ, aEDateJ, aTimeSeries.ItemByKey("UZS"), "UZS (in)") Then
                lZgc.SaveIn(lOutFileBase & "_Error_UZS" & aGraphSaveFormat)
            End If
        End If

        'scatter - Observed vs Error(cfs)    
        lZgc = CreateZgc()
        If GraphScatterError(lZgc, lDataGroup, aSDateJ, aEDateJ, aTimeSeries.ItemByKey("Observed"), "Observed (cfs)", AxisType.Log) Then
            lZgc.SaveIn(lOutFileBase & "_Error_ObsFlow" & aGraphSaveFormat)
        End If

        'add precip to aux axis
        Dim lPaneCount As Integer = 1
        Dim lPrecTser As atcTimeseries = Nothing
        If aTimeSeries.IndexFromKey("Precipitation") > -1 Then
            lPrecTser = aTimeSeries.ItemByKey("Precipitation")
            lPrecTser.Attributes.SetValue("YAxis", "Aux")
            lDataGroup.Add(SubsetByDate(lPrecTser, _
                                        aSDateJ, _
                                        aEDateJ, Nothing))
            lPaneCount = 2
        End If

        'whole span
        GraphTimeseries(lDataGroup, lPaneCount, lOutFileBase, aGraphSaveFormat)

        If aGraphAnnual Then 'single year plots
            Dim lSDateJ As Double = aSDateJ
            Dim lDate(5) As Integer
            While lSDateJ < aEDateJ
                Dim lEDateJ As Double = TimAddJ(lSDateJ, 6, 1, 1)
                Dim lDataGroupYear As New atcDataGroup
                For Each lTimeseries As atcTimeseries In lDataGroup
                    lDataGroupYear.Add(SubsetByDate(lTimeseries, lSDateJ, lEDateJ, Nothing))
                Next
                J2Date(lSDateJ, lDate)
                If lDate(1) <> 1 OrElse lDate(2) <> 1 Then lDate(0) += 1 'non calendar years label with ending year
                GraphTimeseries(lDataGroupYear, lPaneCount, lOutFileBase & "_" & lDate(0), aGraphSaveFormat)
                lSDateJ = lEDateJ
            End While
        End If

        'monthly
        Dim lMonthDataGroup As New atcDataGroup
        lMonthDataGroup.Add(Aggregate(lDataGroup.Item(0), atcTimeUnit.TUMonth, 1, atcTran.TranAverSame))
        lMonthDataGroup.Add(Aggregate(lDataGroup.Item(1), atcTimeUnit.TUMonth, 1, atcTran.TranAverSame))
        If lPaneCount = 2 Then
            lMonthDataGroup.Add(Aggregate(lDataGroup.Item(2), atcTimeUnit.TUMonth, 1, atcTran.TranSumDiv))
        End If
        lZgc = CreateZgc()
        lZgc.Width *= 2
        Dim lGrapher As New clsGraphTime(lMonthDataGroup, lZgc)
        If lPaneCount = 2 Then lZgc.MasterPane.PaneList(0).YAxis.Title.Text = "Precip (in)"
        Dim lDualDateScale As Object = lZgc.MasterPane.PaneList(0).XAxis.Scale
        lDualDateScale.MaxDaysMonthLabeled = 1200
        lZgc.SaveIn(lOutFileBase & "_month" & aGraphSaveFormat)

        'monthly timeseries - log
        With lZgc.MasterPane.PaneList(lPaneCount - 1) 'main pane, not aux
            .YAxis.Type = ZedGraph.AxisType.Log
            .YAxis.Scale.Max *= 4 'wag!
            .YAxis.Scale.MaxAuto = False
            .YAxis.Scale.IsUseTenPower = False
        End With
        lZgc.SaveIn(lOutFileBase & "_month_log " & aGraphSaveFormat)
        lZgc.Dispose()
        lGrapher.Dispose()

        Dim lKeys As New Collection
        If aTimeSeries.IndexFromKey("PotET") > -1 AndAlso _
           aTimeSeries.IndexFromKey("PotET") Then
            'weekly ET - pet vs act
            lDataGroup.Clear()
            lKeys.Add("PotET")
            lKeys.Add("ActET")
            For Each lKey As String In lKeys
                Dim lTser As atcTimeseries = aTimeSeries.ItemByKey(lKey)
                lTser.Attributes.SetValue("Units", "ET (in)")
                lTser.Attributes.SetValue("YAxis", "Left")
                If lKey = "PotET" Then ' force pet to be observed
                    lTser.Attributes.SetValue("Scenario", "Observed")
                End If
                lDataGroup.Add(SubsetByDate(Aggregate(lTser, atcTimeUnit.TUDay, 7, atcTran.TranSumDiv), _
                                            aSDateJ, _
                                            aEDateJ, Nothing))
            Next
            lZgc = CreateZgc()
            lGrapher = New clsGraphTime(lDataGroup, lZgc)
            lZgc.Width *= 2
            lDualDateScale = lZgc.MasterPane.PaneList(0).XAxis.Scale
            lDualDateScale.MaxDaysMonthLabeled = 1200
            lZgc.SaveIn(lOutFileBase & "_ET" & aGraphSaveFormat)
            lZgc.Dispose()
            lGrapher.Dispose()
            lKeys.Clear()
        End If

        If aTimeSeries.IndexFromKey("Baseflow") > -1 AndAlso _
           aTimeSeries.IndexFromKey("Interflow") > -1 AndAlso _
           aTimeSeries.IndexFromKey("Surface") > -1 Then
            'flow components
            lDataGroup.Clear()
            lKeys.Add("Baseflow")
            lKeys.Add("Interflow")
            lKeys.Add("Surface")
            For Each lKey As String In lKeys
                Dim lTser As atcTimeseries = aTimeSeries.ItemByKey(lKey)
                lTser.Attributes.SetValue("Units", "Flow (in)")
                lTser.Attributes.SetValue("YAxis", "Left")
                lDataGroup.Add(SubsetByDate(lTser, _
                                            aSDateJ, _
                                            aEDateJ, Nothing))
            Next
            'precip
            lPrecTser = aTimeSeries.ItemByKey("Precipitation")
            lPrecTser.Attributes.SetValue("YAxis", "Aux")
            lDataGroup.Add(SubsetByDate(lPrecTser, _
                                        aSDateJ, _
                                        aEDateJ, Nothing))
            'actual et
            Dim lETTSer As atcTimeseries = aTimeSeries.ItemByKey("ActET")
            lETTSer.Attributes.SetValue("YAxis", "Left")
            lDataGroup.Add(SubsetByDate(lETTSer, _
                                        aSDateJ, _
                                        aEDateJ, Nothing))
            'do the graphs
            GraphFlowComponents(lDataGroup, lOutFileBase, aGraphSaveFormat)
        End If
    End Sub

    Private Sub GraphTimeseries(ByVal aDataGroup As atcDataGroup, _
                                ByVal aPaneCount As Integer, _
                                ByVal aOutFileBase As String, _
                                ByVal aGraphSaveFormat As String)
        'timeseries - arith
        Dim lZgc As ZedGraphControl
        lZgc = CreateZgc()
        Dim lGrapher As New clsGraphTime(aDataGroup, lZgc)
        If aPaneCount = 2 Then lZgc.MasterPane.PaneList(0).YAxis.Title.Text = "Precip (in)"
        lZgc.SaveIn(aOutFileBase & aGraphSaveFormat)
        'timeseries - log
        With lZgc.MasterPane.PaneList(aPaneCount - 1)
            .YAxis.Type = ZedGraph.AxisType.Log
            'ScaleAxis(lDataGroup, .YAxis)
            .YAxis.Scale.Max *= 4 'wag!
            .YAxis.Scale.MaxAuto = False
            .YAxis.Scale.IsUseTenPower = False
        End With
        lZgc.SaveIn(aOutFileBase & "_log " & aGraphSaveFormat)
        lGrapher.Dispose()
        lZgc.Dispose()
    End Sub

    Sub GraphFlowComponents(ByVal aDataGroup As atcDataGroup, _
                            ByVal aOutFileBase As String, _
                            ByVal aGraphSaveFormat As String)
        Dim lDataGroupOutput As New atcDataGroup

        'baseflow + interflow
        Dim lMathBaseInterTSer As atcTimeseries = atcTimeseriesMath.atcTimeseriesMath.Compute("add", aDataGroup.Item(0), aDataGroup.Item(1))
        lMathBaseInterTSer.Attributes.SetValue("Constituent", "Interflow+baseflow")
        lDataGroupOutput.Add(lMathBaseInterTSer)

        'total - add surface runoff
        Dim lMathTSer As atcTimeseries = atcTimeseriesMath.atcTimeseriesMath.Compute("add", lMathBaseInterTSer, aDataGroup.Item(2))
        lMathTSer.Attributes.SetValue("Constituent", "Simulated")
        lDataGroupOutput.Add(lMathTSer)

        'precip - actual et
        Dim lMathPrecEtTSer As atcTimeseries = _
            atcTimeseriesMath.atcTimeseriesMath.Compute("subtract", _
                                Aggregate(aDataGroup.Item(3), atcTimeUnit.TUDay, 1, atcTran.TranSumDiv), _
                                Aggregate(aDataGroup.Item(4), atcTimeUnit.TUDay, 1, atcTran.TranSumDiv))
        lMathPrecEtTSer.Attributes.SetValue("Constituent", "Precip-ActET")
        lMathPrecEtTSer.Attributes.SetValue("YAxis", "Left")

        aDataGroup.Item(0).Attributes.SetValue("Constituent", "Baseflow")
        lDataGroupOutput.Add(aDataGroup.Item(0)) 'baseflow

        lDataGroupOutput.Add(aDataGroup.Item(3)) 'precip

        Dim lZgc As ZedGraphControl = CreateZgc()
        lZgc.Width *= 3
        Dim lGrapher As New clsGraphTime(lDataGroupOutput, lZgc)
        Dim lDualDateScale As Object = lZgc.MasterPane.PaneList(1).XAxis.Scale
        lDualDateScale.MaxDaysMonthLabeled = 1200
        lZgc.MasterPane.PaneList(1).YAxis.Title.Text = "Flow (in)"
        lZgc.MasterPane.PaneList(0).YAxis.Title.Text = "Precip (in)"
        lZgc.SaveIn(aOutFileBase & "_Components" & aGraphSaveFormat)
        With lZgc.MasterPane.PaneList(1) 'main pane, not aux
            .YAxis.Type = ZedGraph.AxisType.Log
            .YAxis.Scale.Max = 4
            .YAxis.Scale.Min = 0.001
            .YAxis.Scale.MaxAuto = False
            .YAxis.Scale.IsUseTenPower = False
        End With
        lZgc.SaveIn(aOutFileBase & "_Components_Log" & aGraphSaveFormat)
        lZgc.Dispose()
        lGrapher.Dispose()

        'now monthly
        Dim lMonthDataGroup As New atcDataGroup
        'one axis test
        lDataGroupOutput.Item(3).Attributes.SetValue("YAxis", "Left")
        lMonthDataGroup.Add(Aggregate(lDataGroupOutput.Item(0), atcTimeUnit.TUMonth, 1, atcTran.TranSumDiv))
        lMonthDataGroup.Add(Aggregate(lDataGroupOutput.Item(1), atcTimeUnit.TUMonth, 1, atcTran.TranSumDiv))
        lMonthDataGroup.Add(Aggregate(lDataGroupOutput.Item(2), atcTimeUnit.TUMonth, 1, atcTran.TranSumDiv))
        lMonthDataGroup.Add(Aggregate(lDataGroupOutput.Item(3), atcTimeUnit.TUMonth, 1, atcTran.TranSumDiv)) 'prec
        lMonthDataGroup.Add(Aggregate(lMathPrecEtTSer, atcTimeUnit.TUMonth, 1, atcTran.TranSumDiv)) 'prec -act et
        lZgc = CreateZgc()
        lZgc.Width *= 3
        lGrapher = New clsGraphTime(lMonthDataGroup, lZgc)
        lDualDateScale = lZgc.MasterPane.PaneList(0).XAxis.Scale
        lDualDateScale.MaxDaysMonthLabeled = 1200
        lZgc.MasterPane.PaneList(0).YAxis.Scale.Max = 10
        lZgc.SaveIn(aOutFileBase & "_Components_month" & aGraphSaveFormat)
        'monthly timeseries - log
        lGrapher.Datasets.RemoveAt(4) 'has negative values, looks wierd
        With lZgc.MasterPane.PaneList(0) 'main pane, not aux
            'With lZgc.MasterPane.PaneList(1) 'main pane, not aux
            .YAxis.Type = ZedGraph.AxisType.Log
            .YAxis.Scale.Max = 10
            .YAxis.Scale.Min = 0.001
            .YAxis.Scale.MaxAuto = False
            .YAxis.Scale.IsUseTenPower = False
        End With
        lZgc.SaveIn(aOutFileBase & "_Components_month_log " & aGraphSaveFormat)
        lZgc.Dispose()
        lGrapher.Dispose()
    End Sub

    Function GraphScatterError(ByVal aZgc As ZedGraphControl, ByVal aDataGroup As atcDataGroup, _
                               ByVal aSDateJ As Double, ByVal aEDateJ As Double, _
                               ByVal aXAxisTser As atcTimeseries, ByVal aXAxisTitle As String, _
                               Optional ByVal aXAxisType As ZedGraph.AxisType = AxisType.Linear) As Boolean
        Dim lMath As New atcTimeseriesMath.atcTimeseriesMath
        Dim lMathArgs As New atcDataAttributes
        lMathArgs.SetValue("timeseries", aDataGroup)
        If lMath.Open("subtract", lMathArgs) Then
            Dim lDataGroupError As New atcDataGroup
            lDataGroupError.Add(SubsetByDate(aXAxisTser, aSDateJ, aEDateJ, Nothing))
            lDataGroupError.Add(SubsetByDate(lMath.DataSets(0), aSDateJ, aEDateJ, Nothing))
            Dim lGraphScatter As clsGraphScatter = New clsGraphScatter(lDataGroupError, aZgc)
            With aZgc.MasterPane.PaneList(0)
                .XAxis.Title.Text = aXAxisTitle
                .XAxis.Type = aXAxisType
                If aXAxisType = AxisType.Linear Then
                    Scalit(aXAxisTser.Attributes.GetValue("Minimum"), _
                           aXAxisTser.Attributes.GetValue("Maximum"), _
                           False, .XAxis.Scale.Min, .XAxis.Scale.Max)
                Else
                    .XAxis.Scale.Min = 1
                    .XAxis.Scale.Max = aXAxisTser.Attributes.GetDefinedValue("Maximum").Value * 2
                    .XAxis.Scale.IsUseTenPower = False
                End If
                .YAxis.Title.Text = "Error (cfs)"
                If Math.Abs(.YAxis.Scale.Min) > .YAxis.Scale.Max Then
                    .YAxis.Scale.Max = -.YAxis.Scale.Min
                Else
                    .YAxis.Scale.Min = -.YAxis.Scale.Max
                End If
            End With
            lGraphScatter.Dispose()
            Return True
        Else 'TODO:need error message
            Return False
        End If
    End Function
End Module
