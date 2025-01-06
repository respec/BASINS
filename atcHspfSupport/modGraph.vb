Imports System
Imports atcUtility
Imports atcData
Imports MapWinUtility
Imports atcGraph
Imports ZedGraph
Imports System.Drawing

Public Module Graph
    ' aTimeSeries keys: "Observed", "Simulated", "SimulatedBroken", "Precipitation",
    '                   "LZS", "UZS", "PotET", "ActET", 
    '                   "Baseflow", "Interflow", "Surface"
    Public Sub GraphAll(ByVal aSDateJ As Double, ByVal aEDateJ As Double,
                        ByVal aCons As String, ByVal aSite As String,
                        ByVal aTimeSeries As atcTimeseriesGroup,
                        ByVal aGraphSaveFormat As String,
                        ByVal aGraphSaveWidth As Integer,
                        ByVal aGraphSaveHeight As Integer,
                        ByVal aGraphAnnual As Boolean,
                        ByVal aOutFolderName As String,
                        Optional ByVal aMakeStd As Boolean = True,
                        Optional ByVal aMakeLog As Boolean = True,
                        Optional ByVal aMakeSup As Boolean = True,
                        Optional ByVal aPercentMissingData As Double = 0.0)
        'Becky added the last two booleans to indicate which
        'graphs to print, default them to true so if by some chance this is called from somewhere else the code will
        'still work
        Dim lDataGroup As New atcTimeseriesGroup
        lDataGroup.Add(Aggregate(SubsetByDate(aTimeSeries.ItemByKey("Observed"),
                                              aSDateJ,
                                              aEDateJ, Nothing),
                                 atcTimeUnit.TUDay, 1, atcTran.TranAverSame, Nothing))
        If aPercentMissingData > 0 Then
            lDataGroup.Add(Aggregate(SubsetByDate(aTimeSeries.ItemByKey("SimulatedBroken"),
                                              aSDateJ,
                                              aEDateJ, Nothing),
                                 atcTimeUnit.TUDay, 1, atcTran.TranAverSame, Nothing))


        Else
            lDataGroup.Add(Aggregate(SubsetByDate(aTimeSeries.ItemByKey("Simulated"),
                                              aSDateJ,
                                              aEDateJ, Nothing),
                                 atcTimeUnit.TUDay, 1, atcTran.TranAverSame, Nothing))


        End If

        Dim lOutFileBase As String = aOutFolderName & aCons & "_" & aSite
        Dim lOutFileName As String = lOutFileBase

        Dim lZgc As ZedGraphControl

        InitMatchingColors(FindFile("Please locate GraphColors.txt", "GraphColors.txt")) 'Becky moved this here from atcGraph10/CreateZgc so that
        'the file is found ONCE instead of a gazillion times and the colors are initialized ONCE rather than a gazillion
        'times

        Dim CumulativeGraph As Boolean = True

        If CumulativeGraph Then
            lZgc = CreateZgc()
            lZgc.Width = aGraphSaveWidth
            lZgc.Height = aGraphSaveHeight
            Dim lGraphDur As New clsGraphRunningSum(lDataGroup, lZgc)
            With lGraphDur.ZedGraphCtrl.GraphPane
                .YAxis.Title.Text = "Cumulative Flow (cfs)"
                .XAxis.Title.Text = ""
                .YAxis.Title.FontSpec.Size += 8
                .YAxis.Scale.FontSpec.IsBold = False
                .YAxis.Scale.FontSpec.Size += 4
                .XAxis.Scale.FontSpec.Size += 4
                .XAxis.Scale.FontSpec.IsBold = False
                .XAxis.Title.FontSpec.Size += 10
                .Legend.FontSpec.Size += 4
                .Title.FontSpec.Size -= 2
                .Title.FontSpec.IsBold = False
                lOutFileName &= "_cum" & aGraphSaveFormat
                If aPercentMissingData > 0 Then
                    .Title.Text = lOutFileName & " (" & FormatNumber(aPercentMissingData, 1) & "% observed data is missing)"
                Else
                    .Title.Text = lOutFileName
                End If

            End With
            lZgc.SaveIn(lOutFileName)
            lGraphDur.Dispose()
            lZgc.Dispose()

        End If

        If aMakeStd Then 'Becky added this if-then so duration plot only generates if the user wants standard plots
            'duration plot
            lZgc = CreateZgc()
            lZgc.Width = aGraphSaveWidth
            lZgc.Height = aGraphSaveHeight
            lOutFileName = lOutFileBase
            Dim lGraphDur As New clsGraphProbability(lDataGroup, lZgc)
            With lGraphDur.ZedGraphCtrl.GraphPane

                .YAxis.Scale.MinAuto = False
                'This makes sure that the Minimum of YAxis is 0.1, 1, 10, or 100.
                Dim YMin As Double = Math.Min(lDataGroup(0).Attributes.GetDefinedValue("min").Value,
                                              lDataGroup(1).Attributes.GetDefinedValue("min").Value)

                .YAxis.Scale.Min = Math.Pow(10, Math.Floor(Math.Log10(YMin)))


                '.YAxis.Scale.Max = 10000
                .XAxis.Scale.MinAuto = True
                '.YAxis.Scale.MaxAuto = True
                '.XAxis.Scale.Min = 0.001
                .XAxis.Scale.MaxAuto = True
                '.XAxis.Scale.Max = 0.9998
                .AxisChange()
                'End If
                .YAxis.Title.Text = "Flow (cfs)"
                .XAxis.Title.Text = "Percent Chance Daily Flow Exceeded"
                .YAxis.Title.FontSpec.Size += 8
                .YAxis.Scale.FontSpec.IsBold = False
                .YAxis.Scale.FontSpec.Size += 4
                .XAxis.Scale.FontSpec.Size += 4
                .XAxis.Scale.FontSpec.IsBold = False
                .XAxis.Title.FontSpec.Size += 10
                .Legend.FontSpec.Size += 4
                .Title.FontSpec.Size -= 2
                .Title.FontSpec.IsBold = False
                lOutFileName &= "_dur" & aGraphSaveFormat

                If aPercentMissingData > 0 Then
                    .Title.Text = lOutFileName & " (" & FormatNumber(aPercentMissingData, 1) & "% observed data is missing)"
                Else
                    .Title.Text = lOutFileName
                End If

            End With

            lZgc.SaveIn(lOutFileName)
            lGraphDur.Dispose()
            lZgc.Dispose()
        End If

        lZgc = CreateZgc()
        lZgc.Width = aGraphSaveWidth
        lZgc.Height = aGraphSaveHeight
        lOutFileName = lOutFileBase
        Dim lGraphCum As New clsGraphCumulativeDifference(lDataGroup, lZgc)
        With lGraphCum.ZedGraphCtrl.GraphPane
            .YAxis.Title.Text = "Cumulative Difference Daily Flow (cfs)"
            .XAxis.Title.Text = ""
            .YAxis.Title.FontSpec.Size += 8
            .YAxis.Scale.FontSpec.IsBold = False
            .YAxis.Scale.FontSpec.Size += 4
            .XAxis.Scale.FontSpec.Size += 4
            .XAxis.Scale.FontSpec.IsBold = False
            .XAxis.Title.FontSpec.Size += 10
            .Legend.FontSpec.Size += 4
            .Title.FontSpec.Size -= 2
            .Title.FontSpec.IsBold = False
            lOutFileName &= "_cumDif" & aGraphSaveFormat
            If aPercentMissingData > 0 Then
                .Title.Text = lOutFileName & " (" & FormatNumber(aPercentMissingData, 1) & "% observed data is missing)"
            Else
                .Title.Text = lOutFileName
            End If
        End With

        lZgc.SaveIn(lOutFileName)
        lGraphCum.Dispose()
        lZgc.Dispose()

        'scatter
        lZgc = CreateZgc()
        lZgc.Width = aGraphSaveWidth
        lZgc.Height = aGraphSaveHeight
        If Not aPercentMissingData > 0 Then
            Dim lGraphScatter As New clsGraphScatter(lDataGroup, lZgc)
            lOutFileName = lOutFileBase
            lGraphScatter.AddFitLine()
            With lGraphScatter.ZedGraphCtrl.GraphPane
                '.YAxis.Title.Text = "Cumulative Difference Daily Flow (cfs)"
                .YAxis.Title.FontSpec.Size += 8
                .YAxis.Scale.FontSpec.IsBold = False
                .YAxis.Scale.FontSpec.Size += 4
                .XAxis.Scale.FontSpec.Size += 4
                .XAxis.Scale.FontSpec.IsBold = False
                .XAxis.Title.FontSpec.Size += 10

                .Legend.FontSpec.Size += 4
                .Title.FontSpec.Size -= 2
                .Title.FontSpec.IsBold = False
                lOutFileName &= "_scatDay" & aGraphSaveFormat
                .Title.Text = lOutFileName
            End With
            lZgc.SaveIn(lOutFileName)
            lOutFileName = lOutFileBase
            With lZgc.MasterPane.PaneList(0)
                .YAxis.Type = AxisType.Log
                .XAxis.Type = AxisType.Log
                lOutFileName &= "_scatDay_log" & aGraphSaveFormat
                .Title.Text = lOutFileName
            End With
            lZgc.SaveIn(lOutFileName)

            lGraphScatter.Dispose()
            lZgc.Dispose()

            'scatter - Observed vs Error(cfs)    
            lZgc = CreateZgc()
            lZgc.Width = aGraphSaveWidth
            lZgc.Height = aGraphSaveHeight
            lOutFileName = lOutFileBase
            If GraphScatterError(lZgc, lDataGroup, aSDateJ, aEDateJ, aTimeSeries.ItemByKey("Observed"), "Observed (cfs)", AxisType.Log) Then
                lOutFileName &= "_Error_ObsFlow" & aGraphSaveFormat

                Dim lPane0 As GraphPane = lZgc.MasterPane.PaneList(0)
                lPane0.YAxis.Title.FontSpec.IsBold = True
                lPane0.YAxis.Title.FontSpec.Size += 8
                lPane0.YAxis.Scale.FontSpec.Size += 4
                lPane0.YAxis.Scale.FontSpec.IsBold = False
                lPane0.YAxis.Scale.Max *= 1.0
                lPane0.Legend.FontSpec.Size += 4
                lPane0.Legend.FontSpec.IsBold = True

                lPane0.XAxis.Title.FontSpec.IsBold = True
                lPane0.XAxis.Title.FontSpec.Size += 8
                lPane0.XAxis.Scale.FontSpec.Size += 4
                lPane0.XAxis.Scale.FontSpec.IsBold = False
                lZgc.GraphPane.Title.FontSpec.IsBold = False
                lZgc.GraphPane.Title.FontSpec.Size -= 2
                lZgc.GraphPane.Title.Text = lOutFileName



                lZgc.SaveIn(lOutFileName)
            End If
        End If


        lDataGroup.Clear()
        lDataGroup.Add(Aggregate(SubsetByDate(aTimeSeries.ItemByKey("Observed"),
                                              aSDateJ,
                                              aEDateJ, Nothing),
                                 atcTimeUnit.TUDay, 1, atcTran.TranAverSame, Nothing))

        lDataGroup.Add(Aggregate(SubsetByDate(aTimeSeries.ItemByKey("Simulated"),
                                              aSDateJ,
                                              aEDateJ, Nothing),
                                 atcTimeUnit.TUDay, 1, atcTran.TranAverSame, Nothing))




        If aTimeSeries.Keys.Contains("LZS") Then 'scatter - LZS vs Error(cfs)
            lZgc = CreateZgc()
            lOutFileName = lOutFileBase
            lZgc.Width = aGraphSaveWidth
            lZgc.Height = aGraphSaveHeight

            If GraphScatterError(lZgc, lDataGroup, aSDateJ, aEDateJ, aTimeSeries.ItemByKey("LZS"), "LZS (in)") Then
                lOutFileName &= "_Error_LZS" & aGraphSaveFormat

                With lZgc.GraphPane
                    .YAxis.Title.FontSpec.Size += 8
                    .YAxis.Scale.FontSpec.IsBold = False
                    .YAxis.Scale.FontSpec.Size += 4
                    .XAxis.Scale.FontSpec.Size += 4
                    .XAxis.Scale.FontSpec.IsBold = False
                    .XAxis.Title.FontSpec.Size += 10
                    .Legend.FontSpec.Size += 4
                    .Title.FontSpec.Size -= 2
                    .Title.FontSpec.IsBold = False
                    .Title.Text = lOutFileName
                End With

                lZgc.SaveIn(lOutFileName)
            End If
        End If

        If aTimeSeries.Keys.Contains("UZS") Then 'scatter - UZS vs Error(cfs)
            lZgc = CreateZgc()
            lOutFileName = lOutFileBase
            lZgc.Width = aGraphSaveWidth
            lZgc.Height = aGraphSaveHeight
            If GraphScatterError(lZgc, lDataGroup, aSDateJ, aEDateJ, aTimeSeries.ItemByKey("UZS"), "UZS (in)") Then
                lOutFileName &= "_Error_UZS" & aGraphSaveFormat

                With lZgc.GraphPane
                    .YAxis.Title.FontSpec.Size += 8
                    .YAxis.Scale.FontSpec.IsBold = False
                    .YAxis.Scale.FontSpec.Size += 4
                    .XAxis.Scale.FontSpec.Size += 4
                    .XAxis.Scale.FontSpec.IsBold = False
                    .XAxis.Title.FontSpec.Size += 10
                    .Legend.FontSpec.Size += 4
                    .Title.FontSpec.Size -= 2
                    .Title.FontSpec.IsBold = False
                    .Title.Text = lOutFileName
                End With
                lZgc.SaveIn(lOutFileName)
            End If
        End If

        'add precip to aux axis
        Dim lPaneCount As Integer = 1
        Dim lPrecTser As atcTimeseries = Nothing
        If aTimeSeries.Keys.Contains("Precipitation") Then
            lPrecTser = aTimeSeries.ItemByKey("Precipitation")
            lPrecTser.Attributes.SetValue("YAxis", "Aux")
            lDataGroup.Add(Aggregate(SubsetByDate(lPrecTser,
                                                  aSDateJ,
                                                  aEDateJ, Nothing),
                                     atcTimeUnit.TUDay, 1, atcTran.TranSumDiv, Nothing))
            lPaneCount = 2
        End If

        GraphTimeseries(lDataGroup, lPaneCount, lOutFileBase, aGraphSaveFormat, aGraphSaveWidth, aGraphSaveHeight, , aMakeLog)

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
                lSDateJ = lEDateJ
                If lDataGroupYear(0).numValues = 0 Then Continue While
                GraphTimeseries(lDataGroupYear, lPaneCount, lOutFileBase & "_" & lDate(0), aGraphSaveFormat, aGraphSaveWidth, aGraphSaveHeight, , aMakeLog)

            End While
        End If

        'monthly
        Dim lMonthDataGroup As New atcTimeseriesGroup
        lMonthDataGroup.Add(Aggregate(lDataGroup.Item(0), atcTimeUnit.TUMonth, 1, atcTran.TranAverSame))
        lMonthDataGroup.Add(Aggregate(lDataGroup.Item(1), atcTimeUnit.TUMonth, 1, atcTran.TranAverSame))
        If lPaneCount = 2 Then
            lMonthDataGroup.Add(Aggregate(lDataGroup.Item(2), atcTimeUnit.TUMonth, 1, atcTran.TranSumDiv))
        End If
        GraphTimeseries(lMonthDataGroup, lPaneCount, lOutFileBase & "_month", aGraphSaveFormat, aGraphSaveWidth, aGraphSaveHeight, , aMakeLog)


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
        If aTimeSeries.Keys.Contains("PotET") AndAlso
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
                lDataGroup.Add(SubsetByDate(Aggregate(lTser, atcTimeUnit.TUDay, 7, atcTran.TranSumDiv),
                                                aSDateJ,
                                                aEDateJ, Nothing))
            Next
            lZgc = CreateZgc()
            lGrapher = New clsGraphTime(lDataGroup, lZgc)
            lZgc.Width = aGraphSaveWidth
            lZgc.Height = aGraphSaveHeight
            lDualDateScale = lZgc.MasterPane.PaneList(0).XAxis.Scale
            lDualDateScale.MaxDaysMonthLabeled = 1200
            Dim lPane0 As GraphPane = lZgc.MasterPane.PaneList(0)
            lPane0.YAxis.Title.FontSpec.IsBold = True
            lPane0.YAxis.Title.FontSpec.Size += 8
            lPane0.YAxis.Scale.FontSpec.Size += 4
            lPane0.YAxis.Scale.FontSpec.IsBold = False
            lPane0.YAxis.Scale.Max *= 1.0
            lPane0.Legend.FontSpec.Size += 4
            lPane0.Legend.FontSpec.IsBold = True

            lPane0.XAxis.Title.FontSpec.IsBold = True
            lPane0.XAxis.Title.FontSpec.Size += 8
            lPane0.XAxis.Scale.FontSpec.Size += 4
            lPane0.XAxis.Scale.FontSpec.IsBold = False

            lZgc.SaveIn(lOutFileBase & "_ET" & aGraphSaveFormat)
            lZgc.Dispose()
            lGrapher.Dispose()
            lKeys.Clear()
        End If

        If aTimeSeries.Keys.Contains("Baseflow") AndAlso
              aTimeSeries.Keys.Contains("Interflow") AndAlso
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
                lDataGroup.Add(SubsetByDate(lTser,
                                                aSDateJ,
                                                aEDateJ, Nothing))
            Next
            'precip
            lPrecTser = aTimeSeries.ItemByKey("Precipitation")
            lPrecTser.Attributes.SetValue("YAxis", "Aux")
            lDataGroup.Add(SubsetByDate(lPrecTser,
                                            aSDateJ,
                                            aEDateJ, Nothing))
            'actual et
            Dim lETTSer As atcTimeseries = aTimeSeries.ItemByKey("ActET")
            lETTSer.Attributes.SetValue("YAxis", "Left")
            lDataGroup.Add(SubsetByDate(lETTSer,
                                            aSDateJ,
                                            aEDateJ, Nothing))
            'do the graphs
            GraphFlowComponents(lDataGroup, lOutFileBase, aGraphSaveFormat, aGraphSaveWidth, aGraphSaveHeight, aMakeLog)
        End If

    End Sub

    Public Sub GraphStorms(ByVal aDataGroup As atcTimeseriesGroup,
                           ByVal aPaneCount As Integer,
                           ByVal aOutFileBase As String,
                           ByVal aGraphSaveFormat As String,
                           ByVal aGraphSaveWidth As Integer,
                           ByVal aGraphSaveHeight As Integer,
                           ByVal aExpSystem As HspfSupport.atcExpertSystem,
                           Optional ByVal aMakeLog As Boolean = True)
        'Becky added aMakeLog so user can specify whether or not to print the log charts; default to true so I don't break anything preexisting
        For Each lStorm As HexStorm In aExpSystem.Storms
            Dim lDataGroupStorm As New atcTimeseriesGroup
            Dim SkipGraph As Boolean = False
            For Each lTimeseries As atcTimeseries In aDataGroup
                Dim lSubset As atcTimeseries = SubsetByDate(lTimeseries, lStorm.SDateJ, lStorm.EDateJ, Nothing)
                If lSubset.numValues > 0 Then
                    lDataGroupStorm.Add(lSubset)
                Else
                    SkipGraph = True
                End If
            Next
            Dim lDate(6) As Integer
            J2Date(lStorm.SDateJ, lDate)
            If SkipGraph = False Then
                GraphTimeseries(lDataGroupStorm, aPaneCount,
                                aOutFileBase & "_" & lDate(0) & "-" & lDate(1) & "-" & lDate(2),
                                aGraphSaveFormat, aGraphSaveWidth, aGraphSaveHeight, True, aMakeLog)
            Else
                Logger.Dbg("Skipped graph" & aOutFileBase & "_" & lDate(0) & "-" & lDate(1) & "-" & lDate(2) & ". No datasets in this period.")
            End If

        Next
    End Sub

    Private Sub GraphTimeseries(ByVal aDataGroup As atcTimeseriesGroup,
                                ByVal aPaneCount As Integer,
                                ByVal aOutFileBase As String,
                                ByVal aGraphSaveFormat As String,
                                ByVal aGraphSaveWidth As Integer,
                                ByVal aGraphSaveHeight As Integer,
                       Optional ByVal aLogPrefix As Boolean = False,
                       Optional ByVal aMakeLog As Boolean = True) 'Becky added MakeLog, default true so as not to break other things, 
        'so user can specify whether or not to print log graphs

        Dim lZgc As ZedGraphControl = CreateZgc()
        lZgc.Width = aGraphSaveWidth
        lZgc.Height = aGraphSaveHeight

        Dim lGrapher As New clsGraphTime(aDataGroup, lZgc)

        If aPaneCount = 2 Then
            EnableAuxAxis(lZgc.MasterPane, True, 0.25)
            Dim lPane0 As GraphPane = lZgc.MasterPane.PaneList(0)
            Dim lpane1 As GraphPane = lZgc.MasterPane.PaneList(1)

            'Move X axis label into legend labels
            Dim lXlabel As String = lpane1.XAxis.Title.Text.Trim & " "
            Dim lBoldFont As New ZedGraph.FontSpec()
            lBoldFont.IsBold = True
            lBoldFont.Border.IsVisible = False
            lBoldFont.Size += 2
            lXlabel = CapitalizeFirstLetter(lXlabel)


            lPane0.YAxis.Title.Text = "Precip (in)"

            lPane0.YAxis.Title.FontSpec.IsBold = True
            lPane0.YAxis.Title.FontSpec.Size += 8
            lPane0.YAxis.Scale.FontSpec.Size += 4
            lPane0.YAxis.Scale.FontSpec.IsBold = False
            lPane0.YAxis.Scale.Max *= 1.0
            lPane0.Legend.FontSpec.Size += 4
            lPane0.Legend.FontSpec.IsBold = True

            lpane1.YAxis.Title.Text = "Flow (cfs)"
            lpane1.YAxis.Title.FontSpec.IsBold = True
            lpane1.YAxis.Title.FontSpec.Size += 8
            lpane1.YAxis.Scale.FontSpec.Size += 4
            lpane1.YAxis.Scale.FontSpec.IsBold = False
            lpane1.YAxis.Scale.Max *= 1.0
            lpane1.Legend.FontSpec.Size += 4
            lpane1.Legend.FontSpec.IsBold = True

            lpane1.XAxis.Title.Text = ""
            lpane1.XAxis.Scale.FontSpec.IsBold = False
            lpane1.XAxis.Scale.FontSpec.Size += 4

            If lpane1.YAxis.Scale.Max > 9999 Then
                lpane1.YAxis.Scale.IsUseTenPower = True
            End If

            lpane1.YAxis.Scale.Min = 0


            If aDataGroup(2).Attributes.GetValue("Time Unit") = 4 Then
                lPane0.CurveList(0).Label.Text = "Daily Weighted Precipitation"
            Else
                lPane0.CurveList(0).Label.Text = "Weighted Precipitation"
            End If
            If aDataGroup(0).Attributes.GetValue("Time Unit") = 4 Then
                lpane1.CurveList(0).Label.Text = "Daily Observed Flow"
            Else
                lpane1.CurveList(0).Label.Text = "Observed Flow"
            End If
            If aDataGroup(1).Attributes.GetValue("Time Unit") = 4 Then
                lpane1.CurveList(1).Label.Text = "Daily Simulated Flow"
            Else
                lpane1.CurveList(1).Label.Text = "Simulated Flow"
            End If
            lZgc.GraphPane.Title.Text = aOutFileBase & aGraphSaveFormat
            lZgc.GraphPane.Title.FontSpec.Size -= 2
            lZgc.GraphPane.Title.FontSpec.IsBold = False
            'The code below aligns the auxiliary and main panes. Alignement may cause the labels to 
            'go beyond the graph, so Anurag adjusted the X and chartwidth additionally. 
            Dim lChartWidth As Single = Math.Min(lpane1.Chart.Rect.Width, lPane0.Chart.Rect.Width)
            Dim lChartX As Single = Math.Max(lpane1.Chart.Rect.X, lPane0.Chart.Rect.X)
            lpane1.Chart.Rect = New RectangleF(lChartX + 15, lpane1.Chart.Rect.Y, lChartWidth - 15, lpane1.Chart.Rect.Height)
            lPane0.Chart.Rect = New RectangleF(lChartX + 15, lPane0.Chart.Rect.Y, lChartWidth - 15, lPane0.Chart.Rect.Height)

        End If


        lZgc.SaveIn(aOutFileBase & aGraphSaveFormat)

        If aMakeLog Then
            With lZgc.MasterPane.PaneList(aPaneCount - 1)
                .YAxis.Type = AxisType.Log
                Dim MainPainDataGroup As New atcTimeseriesGroup
                MainPainDataGroup.Add(aDataGroup.FindData("Constituent", "FLOW"))
                MainPainDataGroup.Add(aDataGroup.FindData("Constituent", "SIMQ"))

                ScaleAxis(MainPainDataGroup, .YAxis)
                .YAxis.Type = AxisType.Log
                MainPainDataGroup.Clear()
                .YAxis.Scale.Max *= 4 'wag!
                .YAxis.Scale.MaxAuto = False
                '.YAxis.Scale.Min = 0.1
                .YAxis.Scale.MinAuto = True
                If .YAxis.Scale.Min < 0.01 Then
                    .YAxis.Scale.MinAuto = False
                    .YAxis.Scale.Min = 0.01
                End If
                .YAxis.Scale.IsUseTenPower = False

                If .YAxis.Scale.Max > 9999 Then
                    .YAxis.Scale.IsUseTenPower = True
                End If
                .AxisChange()
            End With

            Dim lOutFileName As String = ""
            If aLogPrefix Then
                Dim lPathIndex As Integer = aOutFileBase.LastIndexOf("\")
                If lPathIndex > -1 Then
                    lOutFileName = aOutFileBase.Substring(0, lPathIndex + 1) & "log_" & aOutFileBase.Substring(lPathIndex + 1) & aGraphSaveFormat
                Else
                    lOutFileName = "_log" & aOutFileBase & aGraphSaveFormat
                End If
            Else
                lOutFileName = aOutFileBase & "_log" & aGraphSaveFormat
            End If
            lZgc.GraphPane.Title.Text = lOutFileName
            lZgc.SaveIn(lOutFileName)
        End If
        lGrapher.Dispose()
        lZgc.Dispose()
    End Sub

    Sub GraphFlowComponents(ByVal aDataGroup As atcTimeseriesGroup, _
                            ByVal aOutFileBase As String, _
                            ByVal aGraphSaveFormat As String, _
                            ByVal aGraphSaveWidth As Integer, _
                            ByVal aGraphSaveHeight As Integer, _
                            Optional ByVal aMakeLog As Boolean = True) 'Becky added last boolean so user can specify whether to make the log graphs
        'defaulting aMakeLog to true makes it so any other random references to this method won't break
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

        'InitMatchingColors(FindFile("", "GraphColors.txt")) 'Becky moved this here from atcGraph10/CreateZgc so that
        'the file is found ONCE instead of a gazillion times and the colors are initialized ONCE rather than a gazillion
        'times

        Dim lZgc As ZedGraphControl = CreateZgc()
        lZgc.Width = aGraphSaveWidth
        lZgc.Height = aGraphSaveHeight
        Dim lGrapher As New clsGraphTime(lDataGroupOutput, lZgc)
        Dim lDualDateScale As Object = lZgc.MasterPane.PaneList(1).XAxis.Scale
        lDualDateScale.MaxDaysMonthLabeled = 1200

        Dim lPane0 As GraphPane = lZgc.MasterPane.PaneList(0)
        Dim lpane1 As GraphPane = lZgc.MasterPane.PaneList(1)

        'Move X axis label into legend labels
        Dim lXlabel As String = lpane1.XAxis.Title.Text.Trim & " "
        Dim lBoldFont As New ZedGraph.FontSpec()
        lBoldFont.IsBold = True
        lBoldFont.Border.IsVisible = False
        lBoldFont.Size += 2
        lXlabel = CapitalizeFirstLetter(lXlabel)

        lPane0.YAxis.Title.Text = "Precip (in)"

        lPane0.YAxis.Title.FontSpec.IsBold = True
        lPane0.YAxis.Title.FontSpec.Size += 8
        lPane0.YAxis.Scale.FontSpec.Size += 4
        lPane0.YAxis.Scale.FontSpec.IsBold = False
        lPane0.YAxis.Scale.Max *= 1.0
        lPane0.Legend.FontSpec.Size += 4
        lPane0.Legend.FontSpec.IsBold = True

        lpane1.YAxis.Title.Text = "Flow (in)"
        lpane1.YAxis.Title.FontSpec.IsBold = True
        lpane1.YAxis.Title.FontSpec.Size += 8
        lpane1.YAxis.Scale.FontSpec.Size += 4
        lpane1.YAxis.Scale.FontSpec.IsBold = False
        lpane1.YAxis.Scale.Max *= 1.0
        lpane1.Legend.FontSpec.Size += 4
        lpane1.Legend.FontSpec.IsBold = True

        'lpane1.XAxis.Title.Text = ""
        lpane1.XAxis.Scale.FontSpec.IsBold = False
        lpane1.XAxis.Scale.FontSpec.Size += 4
        lpane1.XAxis.Title.FontSpec.IsBold = True
        lpane1.XAxis.Title.FontSpec.Size += 8

        If lpane1.YAxis.Scale.Max > 9999 Then
            lpane1.YAxis.Scale.IsUseTenPower = True
        End If

        lpane1.YAxis.Scale.Min = 0

        lZgc.GraphPane.Title.Text = aOutFileBase & "_Components" & aGraphSaveFormat
        lZgc.GraphPane.Title.FontSpec.Size -= 2
        lZgc.GraphPane.Title.FontSpec.IsBold = False

        'The code below aligns the auxiliary and main panes. Alignement may cause the labels to 
        'go beyond the graph, so Anurag adjusted the X and chartwidth additionally. 
        Dim lChartWidth As Single = Math.Min(lpane1.Chart.Rect.Width, lPane0.Chart.Rect.Width)
        Dim lChartX As Single = Math.Max(lpane1.Chart.Rect.X, lPane0.Chart.Rect.X)
        lpane1.Chart.Rect = New RectangleF(lChartX + 15, lpane1.Chart.Rect.Y, lChartWidth - 15, lpane1.Chart.Rect.Height)
        lPane0.Chart.Rect = New RectangleF(lChartX + 15, lPane0.Chart.Rect.Y, lChartWidth - 15, lPane0.Chart.Rect.Height)


        lZgc.SaveIn(aOutFileBase & "_Components" & aGraphSaveFormat)

        'If aMakeLog Then 'Becky added, only do this if the user wants log graphs
        '    With lZgc.MasterPane.PaneList(1) 'main pane, not aux
        '        .YAxis.Type = ZedGraph.AxisType.Log
        '        .YAxis.Scale.Max = 4
        '        .YAxis.Scale.Min = 1
        '        .YAxis.Scale.MaxAuto = False
        '        .YAxis.Scale.IsUseTenPower = False
        '    End With
        '    lZgc.SaveIn(aOutFileBase & "_Components_Log" & aGraphSaveFormat)
        'End If
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
        lZgc.Width = aGraphSaveWidth
        lZgc.Height = aGraphSaveHeight
        lGrapher = New clsGraphTime(lMonthDataGroup, lZgc)
        lDualDateScale = lZgc.MasterPane.PaneList(0).XAxis.Scale
        lDualDateScale.MaxDaysMonthLabeled = 1200
        'lZgc.MasterPane.PaneList(0).YAxis.Scale.Max = 10
        lPane0 = lZgc.MasterPane.PaneList(0)


        lPane0.YAxis.Title.Text = "Flow (in)"

        lPane0.YAxis.Title.FontSpec.IsBold = True
        lPane0.YAxis.Title.FontSpec.Size += 8
        lPane0.YAxis.Scale.FontSpec.Size += 4
        lPane0.YAxis.Scale.FontSpec.IsBold = False
        lPane0.YAxis.Scale.Max *= 1.0
        lPane0.Legend.FontSpec.Size += 4
        lPane0.Legend.FontSpec.IsBold = True

        lPane0.XAxis.Scale.FontSpec.IsBold = False
        lPane0.XAxis.Scale.FontSpec.Size += 4
        lPane0.XAxis.Title.FontSpec.IsBold = True
        lPane0.XAxis.Title.FontSpec.Size += 8

        lZgc.GraphPane.Title.Text = aOutFileBase & "_Components_month" & aGraphSaveFormat
        lZgc.GraphPane.Title.FontSpec.Size -= 2
        lZgc.GraphPane.Title.FontSpec.IsBold = False

        lZgc.SaveIn(aOutFileBase & "_Components_month" & aGraphSaveFormat)
        'If aMakeLog Then 'Becky added, only make log graphs if user wants them
        '    'monthly timeseries - log
        '    lGrapher.Datasets.RemoveAt(4) 'has negative values, looks wierd
        '    With lZgc.MasterPane.PaneList(0) 'main pane, not aux
        '        'With lZgc.MasterPane.PaneList(1) 'main pane, not aux
        '        .YAxis.Type = ZedGraph.AxisType.Log
        '        .YAxis.Scale.Max = 10
        '        .YAxis.Scale.Min = 0.001
        '        .YAxis.Scale.MaxAuto = False
        '        .YAxis.Scale.IsUseTenPower = False
        '    End With
        '    lZgc.SaveIn(aOutFileBase & "_Components_month_log" & aGraphSaveFormat)
        'End If
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
