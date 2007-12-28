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
    Private pYMax As Double = 10000

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
            GraphTimeseriesBatch(lDataGroup)
            GraphResidualBatch(lDataGroup)
            GraphCumDifBatch(lDataGroup)
        Else
            Logger.Msg("Unable to Open " & lWdmFileName)
        End If
    End Sub

    Sub GraphTimeseriesBatch(ByVal aDataGRoup As atcDataGroup)
        Dim lGraphForm As New atcGraph.atcGraphForm()
        Dim lGrapher As New clsGraphTime(aDataGRoup, lGraphForm.ZedGraphCtrl)
        With lGraphForm.Pane
            .YAxis.Scale.Min = 0
            .YAxis.Scale.Max = pYMax
            .YAxis.Title.Text = pBaseName & " (cfs)"
            .XAxis.Title.Text = "Daily Mean Flow"
            .XAxis.MajorTic.IsOutside = True
        End With
        SetGraphSpecs(lGraphForm, "Buford", "Norcross")
        Dim lOutFileName As String = pBaseName
        lGraphForm.SaveBitmapToFile(lOutFileName & ".png")
        lGraphForm.ZedGraphCtrl.SaveIn(lOutFileName & ".emf")

        With lGraphForm.Pane
            .YAxis.Type = ZedGraph.AxisType.Log
            .YAxis.Scale.Min = 100
            .YAxis.Scale.Max = pYMax
            .YAxis.Scale.MaxAuto = False
            .YAxis.Scale.IsUseTenPower = False
        End With
        lOutFileName = pBaseName & "_log "
        lGraphForm.SaveBitmapToFile(lOutFileName & ".png")
        lGraphForm.ZedGraphCtrl.SaveIn(lOutFileName & ".emf")
        lGraphForm.Dispose()
    End Sub

    Sub GraphScatterBatch(ByVal aDataGroup As atcDataGroup)
        Dim lACoef As Double
        Dim lBCoef As Double
        Dim lRSquare As Double
        Dim lGraphForm As New atcGraph.atcGraphForm()
        Dim lGrapher As New clsGraphScatter(aDataGRoup, lGraphForm.ZedGraphCtrl)
        With lGraphForm.Pane
            With .XAxis
                'TODO: figures out how to make whole title go below XAxis title
                .Title.Text = "Buford" & vbCrLf & vbCrLf & _
                              "Scatter Plot" & vbCrLf & _
                              "Chattahoochee Flow (cfs)"
                .Scale.Min = 0
                .Scale.Max = pYMax
                .Scale.IsUseTenPower = False
                .Scale.MaxAuto = False
                .MajorGrid.IsVisible = True
                .MinorGrid.IsVisible = True
            End With
            .YAxis.Scale.Max = .XAxis.Scale.Max
            .YAxis.Scale.Min = .XAxis.Scale.Min
            With .YAxis
                .Title.Text = "Norcross"
                .Scale.IsUseTenPower = False
                .Scale.MaxAuto = False
                .MajorGrid.IsVisible = True
                .MinorGrid.IsVisible = True
            End With

            '45 degree line
            AddLine(lGraphForm.Pane, 1, 0, Drawing.Drawing2D.DashStyle.Dot)
            'regression line 
            'TODO: figure out why this seems backwards!
            FitLine(aDataGRoup.ItemByIndex(1), aDataGRoup.ItemByIndex(0), lACoef, lBCoef, lRSquare)
            Dim lCorrCoef = Math.Sqrt(lRSquare)
            AddLine(lGraphForm.Pane, lACoef, lBCoef)
            SaveFileString("CompareStats.txt", CompareStats(aDataGRoup.ItemByIndex(0), aDataGRoup.ItemByIndex(1)))

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
        lGraphForm.SaveBitmapToFile(lOutFileName & ".png")
        lGraphForm.ZedGraphCtrl.SaveIn(lOutFileName & ".emf")

        With lGraphForm.Pane
            .YAxis.Type = ZedGraph.AxisType.Log
            .YAxis.Scale.Min = 100
            .XAxis.Type = ZedGraph.AxisType.Log
            .XAxis.Scale.Min = 100
            .CurveList.RemoveAt(2)
            .CurveList.RemoveAt(1)
            AddLine(lGraphForm.Pane, 1, 0, Drawing.Drawing2D.DashStyle.Dot)
            AddLine(lGraphForm.Pane, lACoef, lBCoef)
        End With
        lOutFileName = pBaseName & "_scat_log"
        lGraphForm.SaveBitmapToFile(lOutFileName & ".png")
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
            .YAxis.Scale.Min = 100
            .YAxis.Scale.Max = pYMax
            .YAxis.Scale.MaxAuto = False
            .YAxis.Scale.IsUseTenPower = False
            .XAxis.Title.Text = "Percent of Time " & pBaseName & " exceeded"
        End With
        Dim lOutFileName As String = pBaseName & "_dur"
        lGraphForm.SaveBitmapToFile(lOutFileName & ".png")
        lGraphForm.ZedGraphCtrl.SaveIn(lOutFileName & ".emf")
        lGraphForm.Dispose()
    End Sub

    Sub GraphResidualBatch(ByVal aDataGroup As atcDataGroup)
        Dim lArgsMath As New atcDataAttributes
        Dim lTsMath As atcDataSource = New atcTimeseriesMath.atcTimeseriesMath
        lArgsMath.SetValue("timeseries", aDataGroup)
        lArgsMath.SetValue("number", Double.NaN)  'TODO: kludge, find a better way!
        If lTsMath.Open("subtract", lArgsMath) Then
            Dim lGraphForm As New atcGraph.atcGraphForm()
            Dim lGrapher As New clsGraphTime(lTsMath.DataSets, lGraphForm.ZedGraphCtrl)
            lGraphForm.Pane.CurveList(0).Label.Text = "Residual"
            Dim lOutFileName As String = pBaseName & "_residual"
            lGraphForm.SaveBitmapToFile(lOutFileName & ".png")
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
        lArgsMath.SetValue("number", Double.NaN)  'TODO: kludge, find a better way!
        If lTsMath.Open("subtract", lArgsMath) Then
            lArgsMath.Clear()
            lArgsMath.SetValue("timeseries", lTsMath.DataSets)
            Dim lTsRunSum As atcDataSource = New atcTimeseriesMath.atcTimeseriesMath
            If lTsRunSum.Open("running sum", lArgsMath) Then
                Dim lGraphForm As New atcGraph.atcGraphForm()
                Dim lGrapher As New clsGraphTime(lTsRunSum.DataSets, lGraphForm.ZedGraphCtrl)
                lGraphForm.Pane.CurveList(0).Label.Text = "Cummulative Difference"
                Dim lOutFileName As String = pBaseName & "_cumDif"
                lGraphForm.SaveBitmapToFile(lOutFileName & ".png")
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
