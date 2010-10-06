Imports atcUtility
Imports atcData
Imports atcWDM
Imports atcGraph
Imports atcDurationCompare
Imports HspfSupport
Imports MapWindow.Interfaces
Imports ZedGraph
Imports MapWinUtility
Imports System

Module CompareStatsTest
    Private Const pTestPath As String = "C:\test\USGS SWSTAT\input"
    Private Const pBaseName As String = "test"

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        ChDriveDir(pTestPath)
        'open WDM file
        Dim lWdmFileName As String = pTestPath & "\" & pBaseName & ".wdm"
        Dim lWdmDataSource As New atcDataSourceWDM
        If lWdmDataSource.Open(lWdmFileName) Then
            Dim lDataGroup As New atcTimeseriesGroup
            ChDriveDir(IO.Directory.GetCurrentDirectory & "\outfiles")

            Dim lSDate(5) As Integer : lSDate(0) = 1950 : lSDate(1) = 10 : lSDate(2) = 1
            Dim lSDateJ As Double = Date2J(lSDate)
            Dim lEDate(5) As Integer : lEDate(0) = 1980 : lEDate(1) = 10 : lEDate(2) = 1
            Dim lEdatej As Double = Date2J(lEDate)

            Dim lTser1 As atcTimeseries = lWdmDataSource.DataSets.ItemByKey(3) 'Shasta (considered as obs)
            lTser1.Attributes.SetValue("YAxis", "Left")
            lDataGroup.Add(SubsetByDate(lTser1, _
                                        lSDateJ, _
                                        lEdatej, Nothing))

            Dim lTser2 As atcTimeseries = lWdmDataSource.DataSets.ItemByKey(4) 'Scott (considered as sim)
            lTser2.Attributes.SetValue("YAxis", "Left")
            lDataGroup.Add(SubsetByDate(lTser2, _
                                        lSDateJ, _
                                        lEdatej, Nothing))

            Dim lStr As String = ""
            lStr = IntervalReport("USGS Test", atcTimeUnit.TUDay, lDataGroup(0), lDataGroup(1))
            SaveFileString("DailyReport.txt", lStr)
            'lStr = IntervalReport("USGS Test", atcTimeUnit.TUMonth, lDataGroup(0), lDataGroup(1))
            'SaveFileString("MonthlyReport.txt", lStr)
            'lStr = IntervalReport("USGS Test", atcTimeUnit.TUYear, lDataGroup(0), lDataGroup(1))
            'SaveFileString("WaterYearReport.txt", lStr)

            'GraphScatterBatch(lDataGroup)
            'GraphDurationBatch(lDataGroup)
            'GraphTimeseriesBatch(lDataGroup)
            'GraphResidualBatch(lDataGroup)
            'GraphCumDifBatch(lDataGroup)
        Else
            Logger.Msg("Unable to Open " & lWdmFileName)
        End If
    End Sub

    Sub GraphTimeseriesBatch(ByVal aDataGroup As atcTimeseriesGroup)
        Dim lOutFileName As String = pBaseName
        Dim lZgc As ZedGraphControl = CreateZgc()
        Dim lGrapher As New clsGraphTime(aDataGroup, lZgc)
        Dim lPane As GraphPane = lZgc.MasterPane.PaneList(0)

        lZgc.SaveIn(lOutFileName & ".png")
        lZgc.SaveIn(lOutFileName & ".emf")
        With lPane
            .YAxis.Type = ZedGraph.AxisType.Log
            ScaleAxis(aDataGroup, .YAxis)
            .YAxis.Scale.MaxAuto = False
            .YAxis.Scale.IsUseTenPower = False
        End With
        lOutFileName = pBaseName & "_log "
        lZgc.SaveIn(lOutFileName & ".png")
        lZgc.SaveIn(lOutFileName & ".emf")
        lZgc.Dispose()
    End Sub

    Sub GraphScatterBatch(ByVal aDataGroup As atcTimeseriesGroup)
        Dim lOutFileName As String = pBaseName & "_scat"
        Dim lACoef As Double
        Dim lBCoef As Double
        Dim lRSquare As Double
        Dim lZgc As ZedGraphControl = CreateZgc()
        Dim lGrapher As New clsGraphScatter(aDataGroup, lZgc)
        Dim lPane As GraphPane = lZgc.MasterPane.PaneList(0)
        With lPane
            ScaleAxis(aDataGroup, .YAxis)
            '45 degree line
            AddLine(lPane, 1, 0, Drawing.Drawing2D.DashStyle.Dot, "45DegLine")
            'regression line 
            FitLine(aDataGroup.ItemByIndex(1), aDataGroup.ItemByIndex(0), lACoef, lBCoef, lRSquare)
            Dim lCorrCoef As Double = Math.Sqrt(lRSquare)
            AddLine(lPane, lACoef, lBCoef, Drawing.Drawing2D.DashStyle.Solid, "RegLine")
            Dim lReport As New atcDurationCompare.DurationReport
            SaveFileString("CompareStats.txt", CompareStats(aDataGroup.ItemByIndex(0), _
                                                            aDataGroup.ItemByIndex(1), _
                                                            lReport.ClassLimits))

            Dim lText As New TextObj
            Dim lFmt As String = "###,##0.###"
            lText.Text = "Y = " & DoubleToString(lACoef, , lFmt) & " X + " & DoubleToString(lBCoef, , lFmt) & _
                         Environment.NewLine & _
                         "Corr Coef = " & DoubleToString(lCorrCoef, , lFmt)
            'TODO: turn off border
            lText.Location = New Location(0.05, 0.05, CoordType.ChartFraction, AlignH.Left, AlignV.Top)
            .GraphObjList.Add(lText)
            .CurveList(0).Label.IsVisible = False

            lZgc.SaveIn(lOutFileName & ".png")
            lZgc.SaveIn(lOutFileName & ".emf")

            .YAxis.Type = ZedGraph.AxisType.Log
            ScaleAxis(aDataGroup, .YAxis)
            .XAxis.Type = ZedGraph.AxisType.Log
            .XAxis.Scale.Min = .YAxis.Scale.Min
            .XAxis.Scale.Max = .YAxis.Scale.Max
            .CurveList.RemoveAt(2)
            .CurveList.RemoveAt(1)
            AddLine(lPane, 1, 0, Drawing.Drawing2D.DashStyle.Dot, "New45DegLine")
            AddLine(lPane, lACoef, lBCoef, Drawing.Drawing2D.DashStyle.Solid, "NewRegLine")
        End With
        lOutFileName = pBaseName & "_scat_log"
        lZgc.SaveIn(lOutFileName & ".png")
        lZgc.SaveIn(lOutFileName & ".emf")
        lZgc.Dispose()
    End Sub

    Sub GraphDurationBatch(ByVal aDataGroup As atcTimeseriesGroup)
        Dim lOutFileName As String = pBaseName & "_dur"
        Dim lZgc As ZedGraphControl = CreateZgc()
        Dim lGrapher As New clsGraphProbability(aDataGroup, lZgc)
        Dim lPane As GraphPane = lZgc.MasterPane.PaneList(0)
        lZgc.SaveIn(lOutFileName & ".png")
        lZgc.SaveIn(lOutFileName & ".emf")
        lZgc.Dispose()
    End Sub

    Sub GraphResidualBatch(ByVal aDataGroup As atcTimeseriesGroup)
        Dim lZgc As ZedGraphControl = CreateZgc()
        Dim lGrapher As New clsGraphResidual(aDataGroup, lZgc)
        Dim lOutFileName As String = pBaseName & "_residual"
        lZgc.SaveIn(lOutFileName & ".png")
        lZgc.SaveIn(lOutFileName & ".emf")
        lZgc.Dispose()
    End Sub

    Sub GraphCumDifBatch(ByVal aDataGroup As atcTimeseriesGroup)
        Dim lZgc As ZedGraphControl = CreateZgc()
        Dim lGrapher As New clsGraphCumulativeDifference(aDataGroup, lZgc)
        Dim lOutFileName As String = pBaseName & "_cumDif"
        lZgc.SaveIn(lOutFileName & ".png")
        lZgc.SaveIn(lOutFileName & ".emf")
    End Sub
End Module