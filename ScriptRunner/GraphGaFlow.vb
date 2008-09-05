Imports atcUtility
Imports atcData
Imports atcWDM
Imports atcGraph
Imports atcList
Imports HspfSupport
Imports MapWindow.Interfaces
Imports ZedGraph
Imports MapWinUtility
Imports System

Module GraphGaFlow
    Private Const pTestPath As String = "C:\Basins\data\03130001\flow"
    Private Const pBaseName As String = "flow"

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        ChDriveDir(pTestPath)
        'open WDM file
        Dim lWdmFileName As String = pTestPath & "\" & pBaseName & ".wdm"
        Dim lWdmDataSource As New atcDataSourceWDM
        If lWdmDataSource.Open(lWdmFileName) Then
            Dim lDataGroup As New atcDataGroup
            ChDriveDir(IO.Directory.GetCurrentDirectory & "\outfiles")

            Dim lTserDaily As atcTimeseries = lWdmDataSource.DataSets.ItemByKey(30) 'Peachtree Ck
            Dim lHighLowSource As atcDataSource = New atcTimeseriesNdayHighLow.atcTimeseriesNdayHighLow
            Dim lArgsMath As New atcDataAttributes
            lArgsMath.SetValue("timeseries", lTserDaily)
            lArgsMath.SetValue("LogFlg", True)
            lArgsMath.SetValue("NDay", 1)
            lArgsMath.SetValue("Return Period", 100)
            lArgsMath.SetValue("HighFlag", True)
            lHighLowSource.Open("n-day high timeseries", lArgsMath)
            Dim lTserNDay As atcTimeseries = lHighLowSource.DataSets(0)
            Dim lList As New atcListPlugin
            lList.Save(lHighLowSource.DataSets, IO.Path.Combine(CurDir, "Hi1Day.txt"), "DateFormatIncludeYears")

            Dim lOutFileName As String = pBaseName & "_freq"
            Dim lZgc As ZedGraphControl = CreateZgc()
            Dim lGrapher As New clsGraphFrequency(lHighLowSource.DataSets, lZgc)
            lZgc.SaveIn(lOutFileName & ".png")
            lZgc.Dispose()

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

    Sub GraphTimeseriesBatch(ByVal aDataGroup As atcDataGroup)
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

    Sub GraphScatterBatch(ByVal aDataGroup As atcDataGroup)
        Dim lOutFileName As String = pBaseName & "_scat"
        Dim lZgc As ZedGraphControl = CreateZgc()
        Dim lGrapher As New clsGraphScatter(aDataGroup, lZgc)
        lGrapher.AddFitLine()
        Dim lPane As GraphPane = lZgc.MasterPane.PaneList(0)
        With lPane
            ScaleAxis(aDataGroup, .YAxis)
            lZgc.SaveIn(lOutFileName & ".png")
            lZgc.SaveIn(lOutFileName & ".emf")

            .YAxis.Type = ZedGraph.AxisType.Log
            ScaleAxis(aDataGroup, .YAxis)
            .XAxis.Type = ZedGraph.AxisType.Log
            .XAxis.Scale.Min = .YAxis.Scale.Min
            .XAxis.Scale.Max = .YAxis.Scale.Max
        End With
        lOutFileName = pBaseName & "_scat_log"
        lZgc.SaveIn(lOutFileName & ".png")
        lZgc.SaveIn(lOutFileName & ".emf")
        lZgc.Dispose()
    End Sub

    Sub GraphDurationBatch(ByVal aDataGroup As atcDataGroup)
        Dim lOutFileName As String = pBaseName & "_dur"
        Dim lZgc As ZedGraphControl = CreateZgc()
        Dim lGrapher As New clsGraphProbability(aDataGroup, lZgc)
        Dim lPane As GraphPane = lZgc.MasterPane.PaneList(0)
        lZgc.SaveIn(lOutFileName & ".png")
        lZgc.SaveIn(lOutFileName & ".emf")
        lZgc.Dispose()
    End Sub

    Sub GraphResidualBatch(ByVal aDataGroup As atcDataGroup)
        Dim lZgc As ZedGraphControl = CreateZgc()
        Dim lGrapher As New clsGraphResidual(aDataGroup, lZgc)
        Dim lOutFileName As String = pBaseName & "_residual"
        lZgc.SaveIn(lOutFileName & ".png")
        lZgc.SaveIn(lOutFileName & ".emf")
        lZgc.Dispose()
    End Sub

    Sub GraphCumDifBatch(ByVal aDataGroup As atcDataGroup)
        Dim lZgc As ZedGraphControl = CreateZgc()
        Dim lGrapher As New clsGraphCumulativeDifference(aDataGroup, lZgc)
        Dim lOutFileName As String = pBaseName & "_cumDif"
        lZgc.SaveIn(lOutFileName & ".png")
        lZgc.SaveIn(lOutFileName & ".emf")
    End Sub
End Module
