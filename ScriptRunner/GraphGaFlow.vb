Imports atcUtility
Imports atcData
Imports atcWDM
Imports atcGraph
Imports HspfSupport
Imports MapWindow.Interfaces
Imports ZedGraph
Imports MapWinUtility

Module GraphGaFlow
    Private Const pTestPath As String = "D:\Basins\data\03130001\flow"
    Private Const pBaseName As String = "flow"

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        ChDriveDir(pTestPath)
        'open WDM file
        Dim lWdmFileName As String = pTestPath & "\" & pBaseName & ".wdm"
        Dim lWdmDataSource As New atcDataSourceWDM
        If lWdmDataSource.Open(lWdmFileName) Then
            Dim lDataGroup As New atcDataGroup
            ChDriveDir(CurDir() & "\outfiles")

            Dim lSDate(5) As Integer : lSDate(0) = 2000 : lSDate(1) = 1 : lSDate(2) = 1
            Dim lSDateJ As Double = Date2J(lSDate)
            Dim lEDate(5) As Integer : lEDate(0) = 2006 : lEDate(1) = 1 : lEDate(2) = 1
            Dim lEdatej As Double = Date2J(lEDate)

            Dim lTser1 As atcTimeseries = lWdmDataSource.DataSets.ItemByKey(9) 'Chattahoochee at Buford
            lTser1.Attributes.SetValue("YAxis", "Left")
            lDataGroup.Add(SubsetByDate(lTser1, _
                                        lSDateJ, _
                                        lEdatej, Nothing))

            Dim lTser2 As atcTimeseries = lWdmDataSource.DataSets.ItemByKey(15) 'Chattahoochee at Norcross
            lTser2.Attributes.SetValue("YAxis", "Left")
            lDataGroup.Add(SubsetByDate(lTser2, _
                                        lSDateJ, _
                                        lEdatej, Nothing))

            GraphScatterBatch(lDataGroup)
            GraphDurationBatch(lDataGroup)
            GraphTimeseriesBatch(lDataGroup, "Stanam")
            GraphResidualBatch(lDataGroup)
            GraphCumDifBatch(lDataGroup)
        Else
            Logger.Msg("Unable to Open " & lWdmFileName)
        End If
    End Sub

    Sub GraphTimeseriesBatch(ByVal aDataGroup As atcDataGroup, ByVal aLabelAttribute As String)
        Dim lGraphForm As New atcGraph.atcGraphForm()
        Dim lGrapher As New clsGraphTime(aDataGroup, lGraphForm.ZedGraphCtrl)
        ScaleYAxis(aDataGroup, lGraphForm.Pane.YAxis)
        With lGraphForm.Pane
            .YAxis.Title.Text = pBaseName & " (cfs)"
            .YAxis.MajorGrid.IsVisible = True
            .YAxis.MinorGrid.IsVisible = False
            .XAxis.Title.Text = "Daily Mean Flow"
            .XAxis.MajorTic.IsOutside = True
            .XAxis.MajorGrid.IsVisible = True
            Dim lIndex As Integer = 0
            For Each lDataSet As atcDataSet In aDataGroup
                'TODO: need better default label, old GenScn?
                Dim lLabel As String = lDataSet.Attributes.GetDefinedValue(aLabelAttribute).Value
                Dim lScenario As String = lDataSet.Attributes.GetDefinedValue("Scenario").Value
                With .CurveList(lIndex)
                    .Label.Text = lLabel
                    .Color = GetMatchingColor()
                End With
                lIndex += 1
            Next
        End With
        Dim lOutFileName As String = pBaseName
        lGraphForm.ZedGraphCtrl.SaveIn(lOutFileName & ".png")
        lGraphForm.ZedGraphCtrl.SaveIn(lOutFileName & ".emf")

        With lGraphForm.Pane
            .YAxis.Type = ZedGraph.AxisType.Log
            ScaleYAxis(aDataGroup, lGraphForm.Pane.YAxis)
            '.YAxis.Scale.Min = 100
            '.YAxis.Scale.Max = pYMax
            .YAxis.Scale.MaxAuto = False
            .YAxis.Scale.IsUseTenPower = False
        End With
        lOutFileName = pBaseName & "_log "
        lGraphForm.ZedGraphCtrl.SaveIn(lOutFileName & ".png")
        lGraphForm.ZedGraphCtrl.SaveIn(lOutFileName & ".emf")
        lGraphForm.Dispose()
    End Sub

    Sub GraphScatterBatch(ByVal aDataGroup As atcDataGroup)
        Dim lACoef As Double
        Dim lBCoef As Double
        Dim lRSquare As Double
        Dim lGraphForm As New atcGraph.atcGraphForm()
        Dim lGrapher As New clsGraphScatter(aDataGroup, lGraphForm.ZedGraphCtrl)
        With lGraphForm.Pane
            ScaleYAxis(aDataGroup, .YAxis)
            With .YAxis
                .Title.Text = "Norcross"
                .Scale.IsUseTenPower = False
                .Scale.MaxAuto = False
                .MajorGrid.IsVisible = True
                .MinorGrid.IsVisible = True
            End With
            .XAxis.Scale.Max = .YAxis.Scale.Max
            .XAxis.Scale.Min = .YAxis.Scale.Min
            With .XAxis
                'TODO: figures out how to make whole title go below XAxis title
                .Title.Text = "Buford" & vbCrLf & vbCrLf & _
                              "Scatter Plot" & vbCrLf & _
                              "Chattahoochee Flow (cfs)"
                .Scale.IsUseTenPower = False
                .Scale.MaxAuto = False
                .MajorGrid.IsVisible = True
                .MinorGrid.IsVisible = True
            End With
            '45 degree line
            AddLine(lGraphForm.Pane, 1, 0, Drawing.Drawing2D.DashStyle.Dot, "45DegLine")
            'regression line 
            'TODO: figure out why this seems backwards!
            FitLine(aDataGroup.ItemByIndex(1), aDataGroup.ItemByIndex(0), lACoef, lBCoef, lRSquare)
            Dim lCorrCoef = Math.Sqrt(lRSquare)
            AddLine(lGraphForm.Pane, lACoef, lBCoef, Drawing.Drawing2D.DashStyle.Solid, "RegLine")
            SaveFileString("CompareStats.txt", CompareStats(aDataGroup.ItemByIndex(0), aDataGroup.ItemByIndex(1)))

            Dim lText As New TextObj
            Dim lFmt As String = "###,##0.###"
            lText.Text = "Y = " & DoubleToString(lACoef, , lFmt) & " X + " & DoubleToString(lBCoef, , lFmt) & vbLf & _
                         "Corr Coef = " & DoubleToString(lCorrCoef, , lFmt)
            'TODO: turn off border
            lText.Location = New Location(0.05, 0.05, CoordType.ChartFraction, AlignH.Left, AlignV.Top)
            .GraphObjList.Add(lText)
            .CurveList(0).Label.IsVisible = False
        End With
        Dim lOutFileName As String = pBaseName & "_scat"
        lGraphForm.ZedGraphCtrl.SaveIn(lOutFileName & ".png")
        lGraphForm.ZedGraphCtrl.SaveIn(lOutFileName & ".emf")

        With lGraphForm.Pane
            .YAxis.Type = ZedGraph.AxisType.Log
            ScaleYAxis(aDataGroup, .YAxis)
            .XAxis.Type = ZedGraph.AxisType.Log
            .XAxis.Scale.Min = .YAxis.Scale.Min
            .XAxis.Scale.Max = .YAxis.Scale.Max
            .CurveList.RemoveAt(2)
            .CurveList.RemoveAt(1)
            AddLine(lGraphForm.Pane, 1, 0, Drawing.Drawing2D.DashStyle.Dot, "New45DegLine")
            AddLine(lGraphForm.Pane, lACoef, lBCoef, Drawing.Drawing2D.DashStyle.Solid, "NewRegLine")
        End With
        lOutFileName = pBaseName & "_scat_log"
        lGraphForm.ZedGraphCtrl.SaveIn(lOutFileName & ".png")
        lGraphForm.ZedGraphCtrl.SaveIn(lOutFileName & ".emf")
        lGraphForm.Dispose()
    End Sub

    Sub GraphDurationBatch(ByVal aDataGroup As atcDataGroup)
        Dim lGraphForm As New atcGraph.atcGraphForm()
        Dim lGrapher As New clsGraphProbability(aDataGroup, lGraphForm.ZedGraphCtrl)
        SetGraphSpecs(lGraphForm, "Buford", "Norcross")
        With lGraphForm.Pane
            .YAxis.Title.Text = pBaseName & " (cfs)"
            .YAxis.Type = ZedGraph.AxisType.Log
            ScaleYAxis(aDataGroup, .YAxis)
            .YAxis.Scale.MaxAuto = False
            .YAxis.Scale.IsUseTenPower = False
            .XAxis.Title.Text = "Percent of Time " & pBaseName & " exceeded"
        End With
        Dim lOutFileName As String = pBaseName & "_dur"
        lGraphForm.ZedGraphCtrl.SaveIn(lOutFileName & ".png")
        lGraphForm.ZedGraphCtrl.SaveIn(lOutFileName & ".emf")
        lGraphForm.Dispose()
    End Sub

    Sub GraphResidualBatch(ByVal aDataGroup As atcDataGroup)
        Dim lArgsMath As New atcDataAttributes
        Dim lTsMath As atcDataSource = New atcTimeseriesMath.atcTimeseriesMath
        lArgsMath.SetValue("timeseries", aDataGroup)
        If lTsMath.Open("subtract", lArgsMath) Then
            Dim lGraphForm As New atcGraph.atcGraphForm()
            Dim lGrapher As New clsGraphTime(lTsMath.DataSets, lGraphForm.ZedGraphCtrl)
            lGraphForm.Pane.CurveList(0).Label.Text = "Residual"
            Dim lOutFileName As String = pBaseName & "_residual"
            lGraphForm.ZedGraphCtrl.SaveIn(lOutFileName & ".png")
            lGraphForm.ZedGraphCtrl.SaveIn(lOutFileName & ".emf")
            lGraphForm.Dispose()
        Else
            Logger.Dbg("ResidualGraph Calculation Failed")
        End If
    End Sub

    Sub GraphCumDifBatch(ByVal aDataGroup As atcDataGroup)
        Dim lArgsMath As New atcDataAttributes
        Dim lTsMath As atcDataSource = New atcTimeseriesMath.atcTimeseriesMath
        lArgsMath.SetValue("timeseries", aDataGroup)
        If lTsMath.Open("subtract", lArgsMath) Then
            lArgsMath.Clear()
            lArgsMath.SetValue("timeseries", lTsMath.DataSets)
            Dim lTsRunSum As atcDataSource = New atcTimeseriesMath.atcTimeseriesMath
            If lTsRunSum.Open("running sum", lArgsMath) Then
                Dim lGraphForm As New atcGraph.atcGraphForm()
                Dim lGrapher As New clsGraphTime(lTsRunSum.DataSets, lGraphForm.ZedGraphCtrl)
                lGraphForm.Pane.CurveList(0).Label.Text = "Cummulative Difference"
                Dim lOutFileName As String = pBaseName & "_cumDif"
                lGraphForm.ZedGraphCtrl.SaveIn(lOutFileName & ".png")
                lGraphForm.ZedGraphCtrl.SaveIn(lOutFileName & ".emf")
                lGraphForm.Dispose()
            Else
                Logger.Dbg("CumulativeDifferenceGraph Accumulation Calculation Failed")
            End If
        Else
            Logger.Dbg("CumulativeDifferenceGraph Difference Calculation Failed")
        End If
    End Sub
End Module
