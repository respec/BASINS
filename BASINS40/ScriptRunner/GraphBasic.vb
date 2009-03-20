Imports atcUtility
Imports atcData
Imports atcWDM
Imports atcBasinsObsWQ
Imports atcGraph
Imports MapWindow.Interfaces
Imports ZedGraph
Imports MapWinUtility
Imports System

Module GraphBasic
    'This script provides an example of how to build a basic timeseries plot from a script.
    'The data to be displayed is set in these constants, prior to the Sub ScriptMain.

    Private Const pWorkingDirectory As String = "c:\Basins\modelout\"   'all plot files will be written to this folder
    Private Const pBaseName As String = "basic_sample"                  'used in the name of each output plot file

    Private Const pTimeseries1FileName As String = "c:\Basins\modelout\upatoi.wdm"
    Private Const pTimeseries1Id As Integer = 1   'same thing as data set number for a wdm file
    Private Const pTimeseries1Axis As String = "Aux"
    Private Const pTimeseries1IsPoint As Boolean = False

    Private Const pTimeseries2FileName As String = "c:\Basins\modelout\upatoi.wdm"
    Private Const pTimeseries2Id As Integer = 1043
    Private Const pTimeseries2Axis As String = "Left"
    Private Const pTimeseries2IsPoint As Boolean = False

    Private Const pTimeseries3FileName As String = "c:\Basins\modelout\UpatoiMCB.dbf"
    Private Const pTimeseries3Id As Integer = 1
    Private Const pTimeseries3Axis As String = "Left"
    Private Const pTimeseries3IsPoint As Boolean = True

    Private Const pStartingYear As Integer = 2006   'starting date of plot
    Private Const pStartingMonth As Integer = 5
    Private Const pStartingDay As Integer = 1
    Private Const pEndingYear As Integer = 2006     'ending date of plot
    Private Const pEndingMonth As Integer = 5
    Private Const pEndingDay As Integer = 30

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)

        ChDriveDir(pWorkingDirectory)

        Dim lSDate(5) As Integer : lSDate(0) = pStartingYear : lSDate(1) = pStartingMonth : lSDate(2) = pStartingDay
        Dim lSDateJ As Double = Date2J(lSDate)
        Dim lEDate(5) As Integer : lEDate(0) = pEndingYear : lEDate(1) = pEndingMonth : lEDate(2) = pEndingDay
        Dim lEdatej As Double = Date2J(lEDate)

        Dim lTimeseriesGroup As New atcTimeseriesGroup

        'get timeseries 1
        Dim lDataSource1 As New atcDataSourceWDM
        If lDataSource1.Open(pTimeseries1FileName) Then
            Dim lTser1 As atcTimeseries = lDataSource1.DataSets.ItemByKey(pTimeseries1Id)
            lTser1.Attributes.SetValue("YAxis", pTimeseries1Axis)
            lTser1.Attributes.SetValue("Point", pTimeseries1IsPoint)
            lTimeseriesGroup.Add(SubsetByDate(lTser1, _
                                        lSDateJ, _
                                        lEdatej, Nothing))

        Else
            Logger.Msg("Unable to Open " & pTimeseries1FileName)
        End If

        'get timeseries 2
        Dim lDataSource2 As New atcDataSourceWDM
        If lDataSource2.Open(pTimeseries2FileName) Then
            Dim lTser2 As atcTimeseries = lDataSource2.DataSets.ItemByKey(pTimeseries2Id)
            lTser2.Attributes.SetValue("YAxis", pTimeseries2Axis)
            lTser2.Attributes.SetValue("Point", pTimeseries2IsPoint)
            lTimeseriesGroup.Add(SubsetByDate(lTser2, _
                                        lSDateJ, _
                                        lEdatej, Nothing))

        Else
            Logger.Msg("Unable to Open " & pTimeseries2FileName)
        End If

        'get timeseries 3
        Dim lDataSource3 As New atcDataSourceBasinsObsWQ
        If lDataSource3.Open(pTimeseries3FileName) Then
            Dim lTser3 As atcTimeseries = lDataSource3.DataSets(pTimeseries3Id)
            lTser3.Attributes.SetValue("YAxis", pTimeseries3Axis)
            lTser3.Attributes.SetValue("Point", pTimeseries3IsPoint)
            lTimeseriesGroup.Add(SubsetByDate(lTser3, _
                                        lSDateJ, _
                                        lEdatej, Nothing))

        Else
            Logger.Msg("Unable to Open " & pTimeseries3FileName)
        End If

        GraphTimeseriesBatch(lTimeseriesGroup)
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
