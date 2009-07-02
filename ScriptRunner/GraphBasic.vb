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
    Private pbasename As String
    Private pWQGraphSpecification As Object
    Private Const pWorkingDirectory As String = "H:\"
    Private Const pTimeseries1Axis As String = "Aux"
    Private Const pTimeseries1IsPoint As Boolean = False
    Private Const pTimeseries2Axis As String = "Aux"
    Private Const pTimeseries2IsPoint As Boolean = False
    Private Const pTimeseries3Axis As String = "Left"
    Private Const pTimeseries3IsPoint As Boolean = False
    Private Const pObservedWQBaseFileName As String = "H:\FB_WQData.dbf"
    Private Const pTimeseries4Axis As String = "Left"
    Private Const pTimeseries4IsPoint As Boolean = True
    Private Const pTimeseries5Axis As String = "Left"
    Private Const pTimeseries5IsPoint As Boolean = True
    Private Const pTimeseries6Axis As String = "Left"
    Private Const pTimeseries6IsPoint As Boolean = True
    Private Const pTimeseries7Axis As String = "Right"
    Private Const pTimeseries7IsPoint As Boolean = False
    Private pTimeseriesConstituent, pLeftYAxisLabel, pLeftAuxAxisLabel, foldername As String


    Private pGraphSpecUpatoi(,) As Object = { _
        {"RCH35", "TSS", 1999, 10, 1, 2006, 5, 30, "D12", "D13"}, _
        {"RCH35", "TSS", 2006, 5, 9, 2006, 5, 13, "", ""}, _
        {"RCH46", "TSS", 2006, 5, 7, 2006, 5, 10, "", ""}, _
        {"RCH46", "TSS", 2006, 5, 10, 2006, 5, 14, "", ""}, _
        {"RCH46", "TSS", 2005, 12, 14, 2005, 12, 19, "", ""}, _
        {"RCH46", "TSS", 2005, 10, 31, 2005, 11, 6, "", ""}, _
        {"RCH639", "TSS", 2006, 6, 23, 2006, 6, 26, "", ""}, _
        {"RCH639", "TSS", 2006, 8, 22, 2006, 8, 25, "", ""}, _
        {"RCH614", "TSS", 2006, 5, 7, 2006, 5, 12, "", ""}, _
        {"RCH614", "TSS", 2005, 11, 1, 2005, 11, 5, "", ""}, _
        {"RCH662", "TSS", 2005, 12, 4, 2005, 12, 8, "", ""}, _
        {"RCH30", "TSS", 2006, 5, 10, 2006, 5, 12, "", ""}, _
        {"RCH30", "TSS", 2005, 11, 19, 2005, 11, 23, "", ""}, _
        {"RCH30", "TSS", 2006, 1, 1, 2006, 1, 4, "", ""}, _
        {"RCH33", "TSS", 2006, 5, 28, 2006, 5, 31, "", ""}, _
        {"RCH33", "TSS", 2006, 4, 21, 2006, 4, 24, "", ""}, _
        {"RCH33", "TSS", 2006, 5, 9, 2006, 5, 13, "", ""}, _
        {"RCH33", "TSS", 2006, 4, 26, 2006, 4, 29, "", ""}, _
        {"RCH33", "TSS", 2005, 12, 23, 2005, 12, 27, "", ""}, _
        {"RCH666", "TSS", 2006, 5, 9, 2006, 5, 12, "", ""}, _
        {"RCH666", "TSS", 2006, 6, 1, 2006, 6, 4, "", ""}, _
        {"RCH35", "DO", 2001, 7, 1, 2004, 6, 30, "", ""}, _
        {"RCH614", "DO", 2001, 7, 1, 2004, 6, 30, "", ""}, _
        {"RCH45", "DO", 2001, 7, 1, 2004, 6, 30, "", ""}, _
        {"RCH33", "DO", 2001, 7, 1, 2004, 6, 30, "", ""}, _
        {"RCH46", "DO", 2000, 1, 1, 2000, 12, 31, "", ""}, _
        {"RCH35", "TW", 1999, 10, 1, 2004, 6, 30, "D12", "D13"}, _
        {"RCH33", "TW", 2001, 7, 1, 2004, 6, 30, "", ""}, _
        {"RCH614", "TW", 2001, 7, 1, 2004, 6, 30, "", ""}, _
        {"RCH46", "TW", 1999, 10, 1, 2000, 12, 31, "", ""}, _
        {"RCH72", "TW", 2004, 11, 1, 2006, 9, 30, "", ""}, _
        {"RCH45", "TW", 1999, 12, 1, 2004, 6, 30, "O13", ""}, _
        {"RCH35", "NH4-N", 1999, 10, 1, 2006, 5, 31, "D12", "D13"}, _
        {"RCH35", "NO3-N", 1999, 10, 1, 2004, 9, 30, "D12", "D13"}, _
        {"RCH45", "NO3-N", 1999, 12, 1, 2003, 11, 30, "O13", ""}, _
        {"RCH46", "NO3-N", 2000, 1, 1, 2000, 12, 31, "", ""}, _
        {"RCH614", "NO3-N", 2001, 9, 1, 2003, 11, 30, "", ""}, _
        {"RCH35", "PO4-P", 2000, 1, 1, 2003, 12, 31, "D12", "D13"}, _
        {"RCH46", "PO4-P", 1999, 10, 1, 2000, 12, 31, "", ""} _
        }

    Private pGraphSpecNonUpatoi(,) As Object = { _
        {"RCH108", "DO", 1999, 10, 1, 2000, 12, 31, "", ""}, _
        {"RCH108", "NH4-N", 1999, 10, 1, 2000, 12, 31, "", ""}, _
        {"RCH108", "NO3-N", 1999, 10, 1, 2000, 12, 31, "", ""}, _
        {"RCH108", "PO4-P", 1999, 10, 1, 2000, 12, 31, "", ""}, _
        {"RCH108", "TW", 2000, 1, 1, 2000, 12, 31, "", ""}, _
        {"RCH311", "DO", 2001, 7, 1, 2004, 6, 30, "", ""}, _
        {"RCH311", "NO3-N", 2001, 9, 1, 2003, 9, 30, "", ""}, _
        {"RCH311", "TW", 2001, 7, 1, 2004, 6, 30, "", ""} _
        }

    Private Sub Initialize()

        Dim lTestName As String = "upatoi"
        'Dim lTestName As String = "nonupatoi"

        Select Case lTestName
            Case "upatoi"
                pbasename = "upatoi"
                pWQGraphSpecification = pGraphSpecUpatoi

            Case "nonupatoi"
                pbasename = "nonupatoi"
                pWQGraphSpecification = pGraphSpecNonUpatoi

        End Select
    End Sub

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        Initialize()
        Dim pTimeseries1FileName As String = "H:\" & pbasename & ".wdm"
        Dim pOutputWDMFileName As String = "H:\" & pbasename & ".wdm"
        Dim pLastIndex As Integer = pWQGraphSpecification.GetUpperBound(0)
        ChDriveDir(pWorkingDirectory)
        Dim wdmfileinfo As System.IO.FileInfo = New System.IO.FileInfo(pOutputWDMFileName)
        Dim lRunmade As String = wdmfileinfo.LastWriteTime.ToString

        foldername = "H:\WQGraphs_" & pbasename & "_" & Format(Month(lRunmade), "00") & Format(Day(lRunmade), "00") & _
                                  Format(Year(lRunmade), "00") & Format(Hour(lRunmade), "00") & Format(Minute(lRunmade), "00")


        For lGraphIndex As Integer = 0 To pLastIndex

            pTimeseriesConstituent = pWQGraphSpecification(lGraphIndex, 1)
            'If pTimeseriesConstituent = "DO" Then
            Select Case pTimeseriesConstituent
                Case "TW"
                    pLeftYAxisLabel = "Water Temperature (" & ChrW(186) & "F)"
                Case "NO3-N"
                    pLeftYAxisLabel = "Nitrate as Nitrogen (mg/l)"
                Case "TSS"
                    pLeftYAxisLabel = "Total Suspended Solids (mg/l)"
                Case "DO"
                    pLeftYAxisLabel = "Dissolved Oxygen (mg/l)"
                Case "NH4-N"
                    pLeftYAxisLabel = "Ammonia as Nitrogen (mg/l)"
                Case "PO4-P"
                    pLeftYAxisLabel = "Phosphate (mg/l)"
            End Select

            Dim lSDate(5) As Integer : lSDate(0) = pWQGraphSpecification(lGraphIndex, 2) : lSDate(1) _
                                       = pWQGraphSpecification(lGraphIndex, 3) : lSDate(2) = pWQGraphSpecification(lGraphIndex, 4)
            Dim lSDateJ As Double = Date2J(lSDate)
            Dim lEDate(5) As Integer : lEDate(0) = pWQGraphSpecification(lGraphIndex, 5) : lEDate(1) = _
                                        pWQGraphSpecification(lGraphIndex, 6) : lEDate(2) = pWQGraphSpecification(lGraphIndex, 7)
            Dim lEdatej As Double = Date2J(lEDate)
            pBaseName = pWQGraphSpecification(lGraphIndex, 0) & "_" & pTimeseriesConstituent & "_" & Format(pWQGraphSpecification(lGraphIndex, 3), "00") & _
                        Format(pWQGraphSpecification(lGraphIndex, 4), "00") & Right(pWQGraphSpecification(lGraphIndex, 2), 2) & "_to_" & _
                        Format(pWQGraphSpecification(lGraphIndex, 6), "00") & Format(pWQGraphSpecification(lGraphIndex, 7), "00") & Right(pWQGraphSpecification(lGraphIndex, 5), 2)
            Dim lTimeseriesGroup As New atcTimeseriesGroup

            'get timeseries 1
            Dim lDataSource1 As New atcDataSourceWDM
            Dim lTser1, lTser2, ltser3 As atcTimeseries

            If lDataSource1.Open(pTimeseries1FileName) Then

                If pTimeseriesConstituent = "TSS" Then


                    'lTser2 = lDataSource1.DataSets.FindData("Location", pWQGraphSpecification(lGraphIndex, 0)). _
                    'FindData("Constituent", "FLOW").FindData("Time Unit", 3).Finddata("Scenario", "SIMULATE")(0)
                    'lTser2 = lDataSource1.DataSets.FindData("Location", pWQGraphSpecification(lGraphIndex, 0)). _
                    '                    FindData("Constituent", "PREC").FindData("Time Unit", 3)(0)
                    lTser2 = lDataSource1.DataSets.FindData("Location", pWQGraphSpecification(lGraphIndex, 0)). _
                                        FindData("Constituent", "PREC")(0)
                    'lTser2 = Aggregate(lTser2, atcTimeUnit.TUDay, 1, atcTran.TranSumDiv)

                    lTser2.Attributes.SetValue("YAxis", pTimeseries2Axis)
                    lTser2.Attributes.SetValue("Point", pTimeseries2IsPoint)
                    lTimeseriesGroup.Add(SubsetByDate(lTser2, lSDateJ, lEdatej, Nothing))
                    pLeftAuxAxisLabel = "PREC (in)"


                Else
                    lTser1 = lDataSource1.DataSets.FindData("Location", pWQGraphSpecification(lGraphIndex, 0)). _
                    FindData("Constituent", "FLOW").FindData("Time Unit", 4)(0)
                    pLeftAuxAxisLabel = "Flow (cfs)"

                End If

                If lTser1 IsNot Nothing Then
                    lTser1.Attributes.SetValue("YAxis", pTimeseries1Axis)
                    lTser1.Attributes.SetValue("Point", pTimeseries1IsPoint)
                    lTimeseriesGroup.Add(SubsetByDate(lTser1, lSDateJ, lEdatej, Nothing))
                End If

            Else
                Logger.Msg("Unable to Open " & pTimeseries1FileName)
            End If

            Dim lDataSource4 As New atcDataSourceBasinsObsWQ
            If lDataSource4.Open(pObservedWQBaseFileName) Then
                Dim lTser4 As atcTimeseries
                lTser4 = Nothing
                lTser4 = lDataSource4.DataSets.FindData("Location", pWQGraphSpecification(lGraphIndex, 0)). _
                                    FindData("Constituent", pTimeseriesConstituent)(0)

                If lTser4 IsNot Nothing Then
                    lTser4.Attributes.SetValue("YAxis", pTimeseries4Axis)
                    lTser4.Attributes.SetValue("Point", pTimeseries4IsPoint)
                    lTimeseriesGroup.Add(SubsetByDate(lTser4, lSDateJ, lEdatej, Nothing))
                End If


                If (pWQGraphSpecification(lGraphIndex, 8) <> "") Then

                    Dim lTser5 As atcTimeseries = lDataSource4.DataSets.FindData("Location", pWQGraphSpecification(lGraphIndex, 8)). _
                        FindData("Constituent", pTimeseriesConstituent)(0)
                    lTser5.Attributes.SetValue("YAxis", pTimeseries5Axis)
                    lTser5.Attributes.SetValue("Point", pTimeseries5IsPoint)
                    lTimeseriesGroup.Add(SubsetByDate(lTser5, lSDateJ, lEdatej, Nothing))
                    pbasename = pbasename & "_" & pWQGraphSpecification(lGraphIndex, 8)

                    If (pWQGraphSpecification(lGraphIndex, 9) <> "") Then

                        Dim lTser6 As atcTimeseries = lDataSource4.DataSets.FindData("Location", pWQGraphSpecification(lGraphIndex, 9)). _
                            FindData("Constituent", pTimeseriesConstituent)(0)
                        lTser6.Attributes.SetValue("YAxis", pTimeseries6Axis)
                        lTser6.Attributes.SetValue("Point", pTimeseries6IsPoint)
                        lTimeseriesGroup.Add(SubsetByDate(lTser6, lSDateJ, lEdatej, Nothing))

                        pbasename = pbasename & "_" & pWQGraphSpecification(lGraphIndex, 9)
                    End If

                End If
                If pTimeseriesConstituent = "TSS" Then
                    Dim lTser7 As atcTimeseries
                    lTser7 = Nothing
                    lTser7 = lDataSource4.DataSets.FindData("Location", pWQGraphSpecification(lGraphIndex, 0)). _
                                        FindData("Constituent", "Stage_ft")(0)
                    lTser7 = SubsetByDate(lTser7, lSDateJ, lEdatej, Nothing)
                    If (lTser7 IsNot Nothing) Then
                        lTser7.Attributes.SetValue("YAxis", pTimeseries7Axis)
                        lTser7.Attributes.SetValue("Point", pTimeseries7IsPoint)
                        lTimeseriesGroup.Add(lTser7)
                    End If

                End If

            Else
                Logger.Msg("Unable to Open " & pObservedWQBaseFileName)
            End If

            Dim lDataSource3 As New atcDataSourceWDM

            If lDataSource3.Open(pOutputWDMFileName) Then

                If pTimeseriesConstituent = "TSS" Then
                    ltser3 = lDataSource3.DataSets.FindData("Location", pWQGraphSpecification(lGraphIndex, 0)). _
                    FindData("Constituent", pTimeseriesConstituent).FindData("Time Unit", atcTimeUnit.TUHour)(0)
                Else

                    ltser3 = lDataSource3.DataSets.FindData("Location", pWQGraphSpecification(lGraphIndex, 0)). _
                            FindData("Constituent", pTimeseriesConstituent)(0)

                End If
                lTser3.Attributes.SetValue("YAxis", pTimeseries3Axis)
                lTser3.Attributes.SetValue("Point", pTimeseries3IsPoint)
                lTimeseriesGroup.Add(SubsetByDate(lTser3, lSDateJ, lEdatej, Nothing))


            Else
                Logger.Msg("Unable to Open " & pOutputWDMFileName)
            End If

            GraphTimeseriesBatch(lTimeseriesGroup)
            'End If
        Next
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
            .XAxis.Title.Text = ""
            .YAxis.Scale.FontSpec.Size = 14
            .YAxis.Scale.FontSpec.IsBold = True
            .Border.IsVisible = False

            For Each lCurve As ZedGraph.LineItem In .CurveList
                lCurve.Label.Text = lCurve.Label.Text.Replace("SIMULATE", "SIMULATED")
                lCurve.Label.Text = lCurve.Label.Text.Replace("TW", "Water Temperature")
                lCurve.Label.Text = lCurve.Label.Text.Replace("RCH33", "Sally Br. (RCH 33)")
                lCurve.Label.Text = lCurve.Label.Text.Replace("DOX", "Dissolved Oxygen")
                lCurve.Label.Text = lCurve.Label.Text.Replace("RCH35", "Bonham Cr. (RCH 35)")
                lCurve.Label.Text = lCurve.Label.Text.Replace("RCH46", "Upatoi Cr./MCB Bridge (RCH 46)")
                lCurve.Label.Text = lCurve.Label.Text.Replace("RCH614", "Upper Upatoi Cr. (RCH 614)")
                lCurve.Label.Text = lCurve.Label.Text.Replace("RCH639", "Upper Randall Cr. (RCH 639)")
                lCurve.Label.Text = lCurve.Label.Text.Replace("RCH45", "Randall Cr. (RCH 45)")
                lCurve.Label.Text = lCurve.Label.Text.Replace("RCH626", "Pine Knot Cr. (RCH 626)")
                lCurve.Label.Text = lCurve.Label.Text.Replace("RCH30", "Pine Knot Cr. (RCH 30)")
                lCurve.Label.Text = lCurve.Label.Text.Replace("RCH662", "Ochille Cr. (RCH 662)")
                lCurve.Label.Text = lCurve.Label.Text.Replace("RCH666", "Tiger Cr. (RCH 666)")
                lCurve.Label.Text = lCurve.Label.Text.Replace("RCH72", "Upatoi Cr./WWTP (RCH 72)")
                lCurve.Label.Text = lCurve.Label.Text.Replace("RCH311", "Oswichee Cr. (RCH 311)")
                lCurve.Label.Text = lCurve.Label.Text.Replace("RCH108", "Bull Cr. (RCH 108)")
            Next
        End With

    End Sub

    Sub GraphTimeseriesBatch(ByVal aDataGroup As atcTimeseriesGroup)
        Dim lOutFileName As String = foldername & "\" & pBaseName
        Dim lZgc As ZedGraphControl = CreateZgc(, 1024, 768)
        Dim lGrapher As New clsGraphTime(aDataGroup, lZgc)
        Dim lPaneAux As GraphPane = lZgc.MasterPane.PaneList(0)
        Dim lPaneMain As GraphPane = lZgc.MasterPane.PaneList(1)
        Dim lCurve As ZedGraph.LineItem

        lCurve = lPaneAux.CurveList.Item(0)
        lCurve.Line.StepType = StepType.NonStep
        lCurve.Line.Width = 2
        lCurve.Color = Drawing.Color.Red
        If Not lCurve.Label.Text.Contains(" at ") Then
            lCurve.Label.Text &= " at " & aDataGroup(0).Attributes.GetValue("Location")
        End If

        If lPaneAux.CurveList.Count > 1 Then
            lCurve = lPaneAux.CurveList.Item(1)
            lCurve.Line.StepType = StepType.NonStep
            lCurve.Line.Width = 2
            lCurve.Color = Drawing.Color.Red
            lPaneAux.CurveList.Item(0).Color = Drawing.Color.Blue
            If Not lCurve.Label.Text.Contains(" at ") Then
                lCurve.Label.Text &= " at " & aDataGroup(0).Attributes.GetValue("Location")
            End If
        End If
        Dim lObserved As ZedGraph.LineItem

        For i As Integer = 0 To lPaneMain.CurveList.Count - 2

            lObserved = lPaneMain.CurveList.Item(i)

            If Not lObserved.Label.Text.Contains(" at ") Then
                lObserved.Label.Text &= " at " & aDataGroup(1 + i).Attributes.GetValue("Location")
            End If
            Dim constituent As String = aDataGroup(i + 1).Attributes.GetValue("Constituent")
            If constituent <> "Stage_ft" Then
                Select Case i
                    Case 0
                        lObserved.Symbol.Type = SymbolType.Circle
                        lObserved.Color = Drawing.Color.Blue
                        lObserved.Symbol.Fill.IsVisible = True
                        lObserved.Symbol.Size = 6
                    Case 1
                        lObserved.Symbol.Type = SymbolType.Diamond
                        lObserved.Color = Drawing.Color.Brown
                        lObserved.Symbol.Fill.IsVisible = True
                        lObserved.Symbol.Size = 8

                    Case 2
                        lObserved.Symbol.Type = SymbolType.Triangle
                        lObserved.Color = Drawing.Color.SeaGreen
                        lObserved.Symbol.Fill.IsVisible = True
                        lObserved.Symbol.Size = 8
                End Select
            Else

                lObserved.Color = Drawing.Color.Green
                lObserved.Line.StepType = StepType.NonStep
                lObserved.Line.Width = 2
                lPaneMain.Y2Axis.Title.Text = "Stage (ft)"

            End If

        Next
        'lCurve = lPaneMain.CurveList.Item(lPaneMain.CurveList.Count - 2)
        'lCurve.Line.StepType = StepType.NonStep
        'lCurve.Line.Width = 2
        'lCurve.Color = Drawing.Color.Green
        'If Not lCurve.Label.Text.Contains(" at ") Then
        '    lCurve.Label.Text &= " at " & aDataGroup(0).Attributes.GetValue("Location")
        'End If

        lCurve = lPaneMain.CurveList.Item(lPaneMain.CurveList.Count - 1)
        lCurve.Line.StepType = StepType.NonStep
        lCurve.Line.Width = 2
        lCurve.Color = Drawing.Color.Red
        If Not lCurve.Label.Text.Contains(" at ") Then
            lCurve.Label.Text &= " at " & aDataGroup(0).Attributes.GetValue("Location")
        End If

        FormatPanes(lZgc)
        lPaneMain.YAxis.Title.Text = pLeftYAxisLabel

        lPaneAux.YAxis.Title.Text = pLeftAuxAxisLabel

        If lOutFileName.EndsWith("RCH614_TSS_110105_to_110505") Then Stop
        'Make sure both graphs line up horizontally
        lPaneAux.AxisChange()
        lPaneMain.AxisChange()
        Dim lMaxX As Single = Math.Max(lPaneAux.Chart.Rect.X, lPaneMain.Chart.Rect.X)
        Dim lMinRight As Single = Math.Min(lPaneAux.Chart.Rect.Right, lPaneMain.Chart.Rect.Right)
        lPaneAux.Chart.Rect = New Drawing.RectangleF(lMaxX, lPaneAux.Chart.Rect.Y, lMinRight - lMaxX, lPaneAux.Chart.Rect.Height)
        lPaneMain.Chart.Rect = New Drawing.RectangleF(lMaxX, lPaneMain.Chart.Rect.Y, lMinRight - lMaxX, lPaneMain.Chart.Rect.Height)

        System.IO.Directory.CreateDirectory(foldername)
        lZgc.SaveIn(lOutFileName & ".png")
        If lOutFileName.EndsWith("RCH614_TSS_110105_to_110505") Then
            OpenFile(lOutFileName & ".png")
            Stop
        End If
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
