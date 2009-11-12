Imports atcUtility
Imports atcData
Imports atcWDM
Imports atcHspfBinOut
Imports atcGraph
Imports MapWindow.Interfaces
Imports ZedGraph
Imports MapWinUtility
Imports System

Module TauGraphs
    Private Const pWorkingDirectory As String = "H:\"   'all plot files will be written to this folder
    Private pBaseName As String
    Private pLeftYAxisLabel As String

    Private Const pTimeseriesAuxAxis As String = "Aux"
    Private Const pTimeseriesAuxIsPoint As Boolean = False

    Private pOutputHBNFileName As String = "H:\NonUpatoiTau.hbn"
    Private Const pTimeseries1Axis As String = "Left"
    Private Const pTimeseries1IsPoint As Boolean = False


    Public Sub ScriptMain(ByRef aMapWin As IMapWin)

        ChDriveDir(pWorkingDirectory)
        Dim TauPercentOutput As String = "Sediment\TauPercent.txt"
        'Dim lTser1 As atcTimeseries
        Dim lTser2 As atcTimeseries
        

        'Dim pOutputHBNFileName As String = "H:\Upatoitau" & i & ".hbn"

        Dim lHspfBinDataSource As New atcTimeseriesFileHspfBinOut()
        lHspfBinDataSource.Open(pOutputHBNFileName)
        Dim lLocations As atcCollection = lHspfBinDataSource.DataSets.SortedAttributeValues("Location")


        For Each lLocation As String In lHspfBinDataSource.DataSets.SortedAttributeValues("Location", "unknown")
            lTser2 = lHspfBinDataSource.DataSets.FindData("Location", lLocation). _
                                FindData("Constituent", "TAU")(0)
            'lTser1 = lHspfBinDataSource.DataSets.FindData("Location", lLocation). _
            'FindData("Constituent", "AVEDP")(0)
            'lTser2 = Aggregate(lTser1, atcTimeUnit.TUDay, 1, atcTran.TranMax)
            'lTser2.Attributes.GetValue("Start Date")

            lTser2.Attributes.SetValue("YAxis", pTimeseries1Axis)
            lTser2.Attributes.SetValue("Point", pTimeseries1IsPoint)

            'lTser1 = lTser1.Values.GetUpperBound()
            'lTser1 = lHspfBinDataSource.DataSets.FindData("Location", lLocation). _
            'FindData("Constituent", "Flow").FindData("Time Unit", 3)(0)
            Dim lTimeseriesGroup As New atcTimeseriesGroup
            lTimeseriesGroup.Add(lTser2)

            pBaseName = "TAU_Calc" & lLocation
            Dim EightyPercentTau As String = Format(lTser2.Attributes.GetValue("%80"), "##.##")
            Dim OnePercentTau As String = Format(lTser2.Attributes.GetValue("%05"), "##.##")
            'Dim Minimum As String = Format(lTser2.Attributes.GetValue("Minimum"), "##.##")
            'Dim Average As String = Format(lTser2.Attributes.GetValue("Mean"), "##.##")
            'Dim Maximum As String = Format(lTser2.Attributes.GetValue("Maximum"), "##.##")
            Dim LocationAndPercent As String = lLocation & "," & EightyPercentTau & "," & OnePercentTau & vbCrLf
            'Dim LocationAndPercent As String = lLocation & "," & Minimum & "," & Average & "," & Maximum & vbCrLf

            IO.File.AppendAllText(TauPercentOutput, LocationAndPercent)

            GraphTimeseriesBatch(lTimeseriesGroup)
            'GraphDurationBatch(lTimeseriesGroup)

        Next



        'Next
    End Sub

    Sub GraphTimeseriesBatch(ByVal aDataGroup As atcTimeseriesGroup)
        pBaseName = SafeFilename(pBaseName)
        Dim lOutFileName As String = "Sediment\" & pBaseName
        Dim lZgc As ZedGraphControl = CreateZgc(, 1024, 768)
        Dim lGrapher As New clsGraphTime(aDataGroup, lZgc)
        'Dim lPaneAux As GraphPane = lZgc.MasterPane.PaneList(0)
        Dim lPaneMain As GraphPane = lZgc.MasterPane.PaneList(0)
        Dim lCurve As ZedGraph.LineItem

        lCurve = lZgc.MasterPane.PaneList(0).CurveList.Item(0)
        lCurve.Line.Color = Drawing.Color.Blue
        lCurve.Line.StepType = StepType.NonStep

        'If Not lCurve.Label.Text.Contains(" at ") Then
        '    lCurve.Label.Text &= " at " & aDataGroup(0).Attributes.GetValue("Location")
        'End If

        FormatPanes(lZgc)
        'lPaneMain.YAxis.Title.Text = "TAU (lbs/ft-square)"
        'lPaneAux.YAxis.Title.Text = "Flow (cfs)"
        lZgc.SaveIn(lOutFileName & ".png")
        'lZgc.SaveIn(lOutFileName & ".emf")
        'With lPaneAux
        '    .YAxis.Type = ZedGraph.AxisType.Log
        '    ScaleAxis(aDataGroup, .YAxis)
        '    .YAxis.Scale.MaxAuto = False
        '    .YAxis.Scale.IsUseTenPower = False
        'End With

        'lOutFileName = lOutFileName & "_log "
        'lZgc.SaveIn(lOutFileName & ".png")
        'lZgc.SaveIn(lOutFileName & ".emf")
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
        Dim lOutFileName As String = "Sediment\" & "_dur" & pBaseName
        Dim lZgc As ZedGraphControl = CreateZgc()
        Dim lGrapher As New clsGraphProbability(aDataGroup, lZgc)
        Dim lPane As GraphPane = lZgc.MasterPane.PaneList(0)
        lZgc.SaveIn(lOutFileName & ".png")
        'lZgc.SaveIn(lOutFileName & ".emf")
        lZgc.Dispose()
    End Sub
    <CLSCompliant(False)> _
      Public Sub FormatPanes(ByVal aZgc As ZedGraph.ZedGraphControl)

        For Each lPane As ZedGraph.GraphPane In aZgc.MasterPane.PaneList()
            FormatPanePrintable(lPane)
        Next
    End Sub

    Public Sub FormatPanePrintable(ByVal aPane As ZedGraph.GraphPane)
        FormatPaneWithDefaults(aPane)
        With aPane

            With .Legend
                .Position = LegendPos.Float
                .Location = New Location(0.05, 0.05, CoordType.ChartFraction, AlignH.Left, AlignV.Top) 'You can change the legend position here
                .FontSpec.Size = 12
                .FontSpec.Border.IsVisible = False
            End With
            .XAxis.Scale.FontSpec.Size = 14
            .XAxis.Scale.FontSpec.IsBold = True
            '.XAxis.Title.Text = ""
            .YAxis.Scale.FontSpec.Size = 14
            .YAxis.Scale.FontSpec.IsBold = True
            .Border.IsVisible = False

            'For Each lCurve As ZedGraph.LineItem In .CurveList
            '    lCurve.Label.Text = lCurve.Label.Text.Replace("SIMULATE", "SIMULATED")
            '    lCurve.Label.Text = lCurve.Label.Text.Replace("TW", "Water Temperature")
            '    lCurve.Label.Text = lCurve.Label.Text.Replace("RCH33", "Sally Branch (Reach 33)")
            '    lCurve.Label.Text = lCurve.Label.Text.Replace("DOX", "Dissolved Oxygen")
            '    lCurve.Label.Text = lCurve.Label.Text.Replace("RCH35", "Bonham Creek (Reach 35)")
            '    lCurve.Label.Text = lCurve.Label.Text.Replace("RCH46", "Upatoi Creek at McBride Bridge (Reach 46)")
            'Next
        End With

    End Sub

End Module
