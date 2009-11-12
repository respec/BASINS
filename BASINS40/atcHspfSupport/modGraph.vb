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
                        ByVal aTimeSeries As atcTimeseriesGroup, _
                        ByVal aGraphSaveFormat As String, _
                        ByVal aGraphSaveWidth As Integer, _
                        ByVal aGraphSaveHeight As Integer, _
                        ByVal aGraphAnnual As Boolean, _
                        ByVal aOutFolderName As String)
        Dim lDataGroup As New atcTimeseriesGroup
        lDataGroup.Add(Aggregate(SubsetByDate(aTimeSeries.ItemByKey("Observed"), _
                                              aSDateJ, _
                                              aEDateJ, Nothing), _
                                 atcTimeUnit.TUDay, 1, atcTran.TranAverSame, Nothing))
        lDataGroup.Add(Aggregate(SubsetByDate(aTimeSeries.ItemByKey("Simulated"), _
                                              aSDateJ, _
                                              aEDateJ, Nothing), _
                                 atcTimeUnit.TUDay, 1, atcTran.TranAverSame, Nothing))

        Dim lOutFileBase As String = aOutFolderName & aCons & "_" & aSite
        Dim lZgc As ZedGraphControl

        'duration plot
        lZgc = CreateZgc()
        lZgc.Width = aGraphSaveWidth
        lZgc.Height = aGraphSaveHeight
        Dim lGraphDur As New clsGraphProbability(lDataGroup, lZgc)
        With lGraphDur.ZedGraphCtrl.GraphPane
            'If .YAxis.Scale.Min < 1 Then
            .YAxis.Scale.MinAuto = False
            .YAxis.Scale.Min = 1
            '.YAxis.Scale.Max = 10000
            .XAxis.Scale.MinAuto = True
            .YAxis.Scale.MaxAuto = True
            '.XAxis.Scale.Min = 0.001
            .XAxis.Scale.MaxAuto = True
            '.XAxis.Scale.Max = 0.9998
            .AxisChange()
            'End If

            .YAxis.Title.FontSpec.Size += 2
            .YAxis.Title.Text = "Flow (cfs)"
            .XAxis.Scale.FontSpec.Size += 1
            .XAxis.Title.FontSpec.Size += 1
            .XAxis.Title.Text = "Percent chance daily flow exceeded (cfs)"
            .YAxis.Scale.FontSpec.IsBold = True
            .YAxis.Scale.FontSpec.Size += 1
            .CurveList(0).Label.Text = "Observed Flow"
            .CurveList(1).Label.Text = "Simulated Flow"

        End With

        
        'TODO: add title 
        lZgc.SaveIn(lOutFileBase & "_dur" & aGraphSaveFormat)
        lGraphDur.Dispose()
        lZgc.Dispose()

        'cummulative difference
        lZgc = CreateZgc()
        lZgc.Width = aGraphSaveWidth
        lZgc.Height = aGraphSaveHeight
        Dim lGraphCum As New clsGraphCumulativeDifference(lDataGroup, lZgc)
        'TODO: add title 
        lZgc.SaveIn(lOutFileBase & "_cumDif" & aGraphSaveFormat)
        lGraphCum.Dispose()
        lZgc.Dispose()

        'scatter
        lZgc = CreateZgc()
        lZgc.Width = aGraphSaveWidth
        lZgc.Height = aGraphSaveHeight
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

        If aTimeSeries.Keys.Contains("LZS") Then 'scatter - LZS vs Error(cfs)
            lZgc = CreateZgc()
            lZgc.Width = aGraphSaveWidth
            lZgc.Height = aGraphSaveHeight
            If GraphScatterError(lZgc, lDataGroup, aSDateJ, aEDateJ, aTimeSeries.ItemByKey("LZS"), "LZS (in)") Then
                lZgc.SaveIn(lOutFileBase & "_Error_LZS" & aGraphSaveFormat)
            End If
        End If

        If aTimeSeries.Keys.Contains("UZS") Then 'scatter - UZS vs Error(cfs)
            lZgc = CreateZgc()
            lZgc.Width = aGraphSaveWidth
            lZgc.Height = aGraphSaveHeight
            If GraphScatterError(lZgc, lDataGroup, aSDateJ, aEDateJ, aTimeSeries.ItemByKey("UZS"), "UZS (in)") Then
                lZgc.SaveIn(lOutFileBase & "_Error_UZS" & aGraphSaveFormat)
            End If
        End If

        'scatter - Observed vs Error(cfs)    
        lZgc = CreateZgc()
        lZgc.Width = aGraphSaveWidth
        lZgc.Height = aGraphSaveHeight
        If GraphScatterError(lZgc, lDataGroup, aSDateJ, aEDateJ, aTimeSeries.ItemByKey("Observed"), "Observed (cfs)", AxisType.Log) Then
            lZgc.SaveIn(lOutFileBase & "_Error_ObsFlow" & aGraphSaveFormat)
        End If

        'add precip to aux axis
        Dim lPaneCount As Integer = 1
        Dim lPrecTser As atcTimeseries = Nothing
        If aTimeSeries.Keys.Contains("Precipitation") Then
            lPrecTser = aTimeSeries.ItemByKey("Precipitation")
            lPrecTser.Attributes.SetValue("YAxis", "Aux")
            lDataGroup.Add(Aggregate(SubsetByDate(lPrecTser, _
                                                  aSDateJ, _
                                                  aEDateJ, Nothing), _
                                     atcTimeUnit.TUDay, 1, atcTran.TranSumDiv, Nothing))
            lPaneCount = 2
        End If

        'whole span
        GraphTimeseries(lDataGroup, lPaneCount, lOutFileBase, aGraphSaveFormat, aGraphSaveWidth, aGraphSaveHeight)

        If aGraphAnnual Then 'single year plots
            Dim lSDateJ As Double = aSDateJ
            Dim lDate(5) As Integer
            While lSDateJ < aEDateJ
                Dim lEDateJ As Double = TimAddJ(lSDateJ, 6, 1, 1)
                Dim lDataGroupYear As New atcTimeseriesGroup
                For Each lTimeseries As atcTimeseries In lDataGroup
                    lDataGroupYear.Add(SubsetByDate(lTimeseries, lSDateJ, lEDateJ, Nothing))
                Next
                J2Date(lSDateJ, lDate)
                If lDate(1) <> 1 OrElse lDate(2) <> 1 Then lDate(0) += 1 'non calendar years label with ending year
                GraphTimeseries(lDataGroupYear, lPaneCount, lOutFileBase & "_" & lDate(0), aGraphSaveFormat, aGraphSaveWidth, aGraphSaveHeight)
                lSDateJ = lEDateJ
            End While
        End If

        'monthly
        Dim lMonthDataGroup As New atcTimeseriesGroup
        lMonthDataGroup.Add(Aggregate(lDataGroup.Item(0), atcTimeUnit.TUMonth, 1, atcTran.TranAverSame))
        lMonthDataGroup.Add(Aggregate(lDataGroup.Item(1), atcTimeUnit.TUMonth, 1, atcTran.TranAverSame))
        If lPaneCount = 2 Then
            lMonthDataGroup.Add(Aggregate(lDataGroup.Item(2), atcTimeUnit.TUMonth, 1, atcTran.TranSumDiv))
        End If
        GraphTimeseries(lMonthDataGroup, lPaneCount, lOutFileBase & "_month", aGraphSaveFormat, aGraphSaveWidth, aGraphSaveHeight)

        'lZgc = CreateZgc()
        'lZgc.Width = aGraphSaveWidth * 2
        'lZgc.Height = aGraphSaveHeight
        Dim lGrapher As clsGraphTime 'New clsGraphTime(lMonthDataGroup, lZgc)
        'If lPaneCount = 2 Then lZgc.MasterPane.PaneList(0).YAxis.Title.Text = "Precip (in)"
        Dim lDualDateScale As Object '= lZgc.MasterPane.PaneList(0).XAxis.Scale
        'lDualDateScale.MaxDaysMonthLabeled = 1200
        'lZgc.SaveIn(lOutFileBase & "_month" & aGraphSaveFormat)

        'monthly timeseries - log
        'With lZgc.MasterPane.PaneList(lPaneCount - 1) 'main pane, not aux
        '    .YAxis.Type = ZedGraph.AxisType.Log
        '    .YAxis.Scale.Max *= 4 'wag!
        '    .YAxis.Scale.MaxAuto = False
        '    .YAxis.Scale.IsUseTenPower = False
        'End With
        'lZgc.SaveIn(lOutFileBase & "_month_log" & aGraphSaveFormat)
        'lZgc.Dispose()
        'lGrapher.Dispose()

        Dim lKeys As New Collection
        If aTimeSeries.Keys.Contains("PotET") AndAlso _
           aTimeSeries.Keys.Contains("ActET") Then
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
            lZgc.Width = aGraphSaveWidth * 2
            lZgc.Height = aGraphSaveHeight
            lDualDateScale = lZgc.MasterPane.PaneList(0).XAxis.Scale
            lDualDateScale.MaxDaysMonthLabeled = 1200
            lZgc.SaveIn(lOutFileBase & "_ET" & aGraphSaveFormat)
            lZgc.Dispose()
            lGrapher.Dispose()
            lKeys.Clear()
        End If

        If aTimeSeries.Keys.Contains("Baseflow") AndAlso _
           aTimeSeries.Keys.Contains("Interflow") AndAlso _
           aTimeSeries.Keys.Contains("Surface") Then
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
            GraphFlowComponents(lDataGroup, lOutFileBase, aGraphSaveFormat, aGraphSaveWidth, aGraphSaveHeight)
        End If
    End Sub

    Public Sub GraphStorms(ByVal aDataGroup As atcTimeseriesGroup, _
                           ByVal aPaneCount As Integer, _
                           ByVal aOutFileBase As String, _
                           ByVal aGraphSaveFormat As String, _
                           ByVal aGraphSaveWidth As Integer, _
                           ByVal aGraphSaveHeight As Integer, _
                           ByVal aExpSystem As HspfSupport.atcExpertSystem)
        For Each lStorm As HexStorm In aExpSystem.Storms
            Dim lDataGroupStorm As New atcTimeseriesGroup
            For Each lTimeseries As atcTimeseries In aDataGroup
                lDataGroupStorm.Add(SubsetByDate(lTimeseries, lStorm.SDateJ, lStorm.EDateJ, Nothing))
            Next
            Dim lDate(6) As Integer
            J2Date(lStorm.SDateJ, lDate)
            GraphTimeseries(lDataGroupStorm, aPaneCount, _
                            aOutFileBase & "_" & lDate(0) & "-" & lDate(1) & "-" & lDate(2), _
                            aGraphSaveFormat, aGraphSaveWidth, aGraphSaveHeight, True)
        Next
    End Sub

    Private Sub GraphTimeseries(ByVal aDataGroup As atcTimeseriesGroup, _
                                ByVal aPaneCount As Integer, _
                                ByVal aOutFileBase As String, _
                                ByVal aGraphSaveFormat As String, _
                                ByVal aGraphSaveWidth As Integer, _
                                ByVal aGraphSaveHeight As Integer, _
                       Optional ByVal aLogPrefix As Boolean = False)
        'timeseries - arith
        Dim lZgc As ZedGraphControl = CreateZgc()

        lZgc.Width = aGraphSaveWidth
        lZgc.Height = aGraphSaveHeight
        Dim lGrapher As New clsGraphTime(aDataGroup, lZgc)
        If aPaneCount = 2 Then
            EnableAuxAxis(lZgc.MasterPane, True, 0.2)

            'Move X axis label into legend labels
            Dim lXlabel As String = lZgc.MasterPane.PaneList(1).XAxis.Title.Text.Trim & " "
            Dim lBoldFont As New ZedGraph.FontSpec()
            lBoldFont.IsBold = True
            lBoldFont.Border.IsVisible = False
            lBoldFont.Size += 2
            lXlabel = CapitalizeFirstLetter(lXlabel)

            lZgc.MasterPane.PaneList(0).YAxis.Title.Text = "Precip (in)"
            lZgc.MasterPane.PaneList(0).YAxis.Title.FontSpec.IsBold = True
            lZgc.MasterPane.PaneList(0).YAxis.Title.FontSpec.Size += 2
            lZgc.MasterPane.PaneList(0).YAxis.Scale.FontSpec.IsBold = True
            lZgc.MasterPane.PaneList(0).YAxis.Scale.FontSpec.Size += 1
            lZgc.MasterPane.PaneList(0).YAxis.Scale.Max *= 1.1

            lZgc.MasterPane.PaneList(1).YAxis.Title.Text = "Flow (cfs)"
            lZgc.MasterPane.PaneList(1).YAxis.Title.FontSpec.IsBold = True
            lZgc.MasterPane.PaneList(1).YAxis.Title.FontSpec.Size += 2
            lZgc.MasterPane.PaneList(1).YAxis.Scale.Max *= 1.1

            lZgc.MasterPane.PaneList(1).YAxis.Scale.FontSpec.Size += 1
            lZgc.MasterPane.PaneList(1).YAxis.Scale.FontSpec.IsBold = True
            lZgc.MasterPane.PaneList(1).YAxis.Scale.Min = 0
            lZgc.MasterPane.PaneList(1).XAxis.Scale.FontSpec.IsBold = True
            lZgc.MasterPane.PaneList(1).XAxis.Scale.FontSpec.Size += 1


            'If lZgc.MasterPane.PaneList(0).CurveList(0).Label.Text.Contains(" Weighted Average ") Then
            '    lZgc.MasterPane.PaneList(0).CurveList(0).Label.Text = lXlabel & "Observed Precipitation at Upatoi Crk, MCB Bridge (RCH46)"
            '    lZgc.MasterPane.PaneList(0).CurveList(0).Label.FontSpec = lBoldFont
            'End If

            'If lZgc.MasterPane.PaneList(1).CurveList(0).Label.Text.Contains("OBSERVED FLOW at RCH46 ") Then
            '    lZgc.MasterPane.PaneList(1).CurveList(0).Label.Text = lXlabel & "Observed Flow at Upatoi Crk, MCB Bridge (RCH46)"
            '    lZgc.MasterPane.PaneList(1).CurveList(0).Label.FontSpec = lBoldFont
            '    lZgc.MasterPane.PaneList(1).XAxis.Title.Text = ""
            'End If

            'If lZgc.MasterPane.PaneList(1).CurveList(1).Label.Text.Contains("SIMULATE") Then
            '    lZgc.MasterPane.PaneList(1).CurveList(1).Label.Text = lXlabel & "Simulated Flow at Upatoi Crk, MCB Bridge (RCH46)"
            '    lZgc.MasterPane.PaneList(1).CurveList(1).Label.FontSpec = lBoldFont
            '    lZgc.MasterPane.PaneList(1).XAxis.Title.Text = ""
            'End If
        End If
        
        lZgc.SaveIn(aOutFileBase & aGraphSaveFormat)
        'timeseries - log
        With lZgc.MasterPane.PaneList(aPaneCount - 1)
            .YAxis.Type = ZedGraph.AxisType.Log
            ScaleAxis(aDataGroup, .YAxis)
            .YAxis.Scale.Max *= 4 'wag!
            .YAxis.Scale.MaxAuto = False
            .YAxis.Scale.Min = 1
            .YAxis.Scale.IsUseTenPower = False
        End With

        Dim lOutFileName As String = ""
        If aLogPrefix Then
            Dim lPathIndex As Integer = aOutFileBase.LastIndexOf("\")
            If lPathIndex > -1 Then
                lOutFileName = aOutFileBase.Substring(0, lPathIndex + 1) & "log_" & aOutFileBase.Substring(lPathIndex + 1) & aGraphSaveFormat
            Else
                lOutFileName = "log_" & aOutFileBase & aGraphSaveFormat
            End If
        Else
            lOutFileName = aOutFileBase & "_log" & aGraphSaveFormat
        End If
        lZgc.SaveIn(lOutFileName)
        lGrapher.Dispose()
        lZgc.Dispose()
    End Sub

    Sub GraphFlowComponents(ByVal aDataGroup As atcTimeseriesGroup, _
                            ByVal aOutFileBase As String, _
                            ByVal aGraphSaveFormat As String, _
                            ByVal aGraphSaveWidth As Integer, _
                            ByVal aGraphSaveHeight As Integer)
        Dim lDataGroupOutput As New atcTimeseriesGroup

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
        lZgc.Width = aGraphSaveWidth * 3
        lZgc.Height = aGraphSaveHeight
        Dim lGrapher As New clsGraphTime(lDataGroupOutput, lZgc)
        Dim lDualDateScale As Object = lZgc.MasterPane.PaneList(1).XAxis.Scale
        lDualDateScale.MaxDaysMonthLabeled = 1200
        lZgc.MasterPane.PaneList(1).YAxis.Title.Text = "Flow (in)"
        lZgc.MasterPane.PaneList(0).YAxis.Title.Text = "Precip (in)"
        lZgc.SaveIn(aOutFileBase & "_Components" & aGraphSaveFormat)
        With lZgc.MasterPane.PaneList(1) 'main pane, not aux
            .YAxis.Type = ZedGraph.AxisType.Log
            .YAxis.Scale.Max = 4
            .YAxis.Scale.Min = 1
            .YAxis.Scale.MaxAuto = False
            .YAxis.Scale.IsUseTenPower = False
        End With
        lZgc.SaveIn(aOutFileBase & "_Components_Log" & aGraphSaveFormat)
        lZgc.Dispose()
        lGrapher.Dispose()

        'now monthly
        Dim lMonthDataGroup As New atcTimeseriesGroup
        'one axis test
        lDataGroupOutput.Item(3).Attributes.SetValue("YAxis", "Left")
        lMonthDataGroup.Add(Aggregate(lDataGroupOutput.Item(0), atcTimeUnit.TUMonth, 1, atcTran.TranSumDiv))
        lMonthDataGroup.Add(Aggregate(lDataGroupOutput.Item(1), atcTimeUnit.TUMonth, 1, atcTran.TranSumDiv))
        lMonthDataGroup.Add(Aggregate(lDataGroupOutput.Item(2), atcTimeUnit.TUMonth, 1, atcTran.TranSumDiv))
        lMonthDataGroup.Add(Aggregate(lDataGroupOutput.Item(3), atcTimeUnit.TUMonth, 1, atcTran.TranSumDiv)) 'prec
        lMonthDataGroup.Add(Aggregate(lMathPrecEtTSer, atcTimeUnit.TUMonth, 1, atcTran.TranSumDiv)) 'prec -act et
        lZgc = CreateZgc()
        lZgc.Width = aGraphSaveWidth * 3
        lZgc.Height = aGraphSaveHeight
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
        lZgc.SaveIn(aOutFileBase & "_Components_month_log" & aGraphSaveFormat)
        lZgc.Dispose()
        lGrapher.Dispose()
    End Sub

    Function GraphScatterError(ByVal aZgc As ZedGraphControl, ByVal aDataGroup As atcTimeseriesGroup, _
                               ByVal aSDateJ As Double, ByVal aEDateJ As Double, _
                               ByVal aXAxisTser As atcTimeseries, ByVal aXAxisTitle As String, _
                               Optional ByVal aXAxisType As ZedGraph.AxisType = AxisType.Linear) As Boolean
        Dim lMath As New atcTimeseriesMath.atcTimeseriesMath
        Dim lMathArgs As New atcDataAttributes
        lMathArgs.SetValue("timeseries", aDataGroup)
        If lMath.Open("subtract", lMathArgs) Then
            Dim lDataGroupError As New atcTimeseriesGroup
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
