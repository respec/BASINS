Imports atcUtility
Imports atcData
Imports atcTimeseriesCSV
Imports atcWDM
Imports atcGraph
Imports atcList
Imports HspfSupport
Imports MapWindow.Interfaces
Imports ZedGraph
Imports MapWinUtility
Imports System

Module ERGGraph
    'black
    'Private Const pTestPath As String = "C:\ERG_SteamElectric\Black"
    'Private pLocation As Integer = 39 '1 '97 '39
    'Private pStartYearForBaselinePlot As Integer = 1999
    'Private pStartYearForSimulation As Integer = 1982
    'Private pComplianceDate As Integer = 2019
    'Private pSiteName As String = "Black"
    'Private pBenthicSegmentStart As Integer = 59
    'etowah
    'Private Const pTestPath As String = "C:\ERG_SteamElectric\Etowah"
    ' ''Private Const pTestPath As String = "C:\ERG_SteamElectric\Etowah\RevisedBackground"
    'Private pLocation As Integer = 18 '1 '50 '18
    'Private pStartYearForBaselinePlot As Integer = 2004
    'Private pStartYearForSimulation As Integer = 1982
    'Private pComplianceDate As Integer = 2021
    'Private pSiteName As String = "Etowah"
    'Private pBenthicSegmentStart As Integer = 33
    'white
    'Private Const pTestPath As String = "C:\ERG_SteamElectric\White"
    'Private pLocation As Integer = 76 '16  '76  '17  changed to 76 with addition of lick creek
    'Private pStartYearForBaselinePlot As Integer = 1999
    'Private pStartYearForSimulation As Integer = 1986
    'Private pComplianceDate As Integer = 2019
    'Private pSiteName As String = "White"
    'Private pBenthicSegmentStart As Integer = 26
    'lake sinclair
    Private Const pTestPath As String = "C:\ERG_SteamElectric\LakeSinclair"
    Private pLocation As Integer = 276 '699 
    Private pStartYearForBaselinePlot As Integer = 2012
    Private pStartYearForSimulation As Integer = 2012
    Private pComplianceDate As Integer = 2019
    Private pSiteName As String = "LakeSinclair"
    Private pBenthicSegmentStart As Integer = 99999
    'mississippi
    'Private Const pTestPath As String = "C:\ERG_SteamElectric\MississippiMO"
    'Private pLocation As Integer = 17  '9 is rush island, 17 is meramec
    'Private pStartYearForBaselinePlot As Integer = 1999
    'Private pStartYearForSimulation As Integer = 1982
    'Private pComplianceDate As Integer = 2019
    'Private pSiteName As String = "MississippiMO"
    'Private pBenthicSegmentStart As Integer = 31
    'ohio
    'Private Const pTestPath As String = "C:\ERG_SteamElectric\Ohio"
    'Private pLocation As Integer = 9 '9 is sammis, 13 is mansfield
    'Private pStartYearForBaselinePlot As Integer = 1999
    'Private pStartYearForSimulation As Integer = 1982
    'Private pComplianceDate As Integer = 2019
    'Private pSiteName As String = "Ohio"
    'Private pBenthicSegmentStart As Integer = 29


    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        'DoERGGraphs()                          'core
        'ComputeThreeMonthRollingAverages()     'core
        'DoERGCompositeGraph()                  'core  
        'DoERGCompositeGraphRefined()           'core  
        'DoExceedanceSummary()                  'core
        '      DoBaselineHistoricGraphsMultipleLocations()  
        '      DoGraphAtMaxConcLocation()
        'ComputeAnnualAverageAcrossAllSegments()     'specific to lake sinclair
        'ComputeAverageConcentrationInEachSegment()  'specific to lake sinclair?
        'DoERGCompositeGraphAveraged()               'specific to lake sinclair 
        DoERGCompositeGraphAveragedRefined()        'specific to lake sinclair
        'OutputInitialConditions()    'specific to lake sinclair
    End Sub

    Private Sub DoERGGraphs()

        Dim lCsvName As String = ""

        Dim pRunNames As New atcCollection
        pRunNames.Add("AsBaselineHistoric")
        pRunNames.Add("CdBaselineHistoric")
        pRunNames.Add("CuBaselineHistoric")
        pRunNames.Add("NiBaselineHistoric")
        pRunNames.Add("PbBaselineHistoric")
        pRunNames.Add("SeBaselineHistoric")
        pRunNames.Add("TlBaselineHistoric")
        pRunNames.Add("ZnBaselineHistoric")

        lCsvName = "Total_Concentration.csv"

        Dim lTimeseriesGroup As New atcTimeseriesGroup
        Dim lTimeseriesCsv As New atcTimeseriesCSV.atcTimeseriesCSV

        ChDriveDir(pTestPath & "\OutputPlots")
        'ChDriveDir(pTestPath & "\OutputPlotsBenthic")
        'ChDriveDir(pTestPath & "\OutputPlotsDownstream")


        Dim lSDate(5) As Integer : lSDate(0) = pStartYearForBaselinePlot : lSDate(1) = 1 : lSDate(2) = 1
        Dim lSDateJ As Double = Date2J(lSDate)
        Dim lEDate(5) As Integer : lEDate(0) = 2020 : lEDate(1) = 12 : lEDate(2) = 31
        Dim lEdatej As Double = Date2J(lEDate)

        'do baseline historic plots
        For Each lRunName As String In pRunNames

            Dim lOutFileName As String = lRunName & ".png"
            If pSiteName = "White" And pLocation = 16 Then
                lOutFileName = lRunName & "Seg" & pLocation.ToString & ".png"
            End If
            If pSiteName = "MississippiMO" Or pSiteName = "Ohio" Then
                lOutFileName = lRunName & "Seg" & pLocation.tostring & ".png"
            End If

            If Not FileExists(lOutFileName) Then

                'if start date file doesn't exist, copy from the first run name folder 
                If Not FileExists(pTestPath & "\" & lRunName & "\" & "Total_Concentration.csv.start") Then
                    FileCopy(pTestPath & "\" & pRunNames(0) & "\" & "Total_Concentration.csv.start", pTestPath & "\" & lRunName & "\" & "Total_Concentration.csv.start")
                End If

                Dim lCsvFileName As String = pTestPath & "\" & lRunName & "\" & lCsvName
                If lTimeseriesCsv.Open(lCsvFileName) Then

                    Dim lTimSer1 As atcTimeseries = lTimeseriesCsv.DataSets.ItemByKey(pLocation)

                    lTimSer1.Attributes.SetValue("YAxis", "Left")
                    lTimeseriesGroup.Add(SubsetByDate(lTimSer1, _
                                                lSDateJ, _
                                                lEdatej, Nothing))

                    GraphTimeseriesBaselineHistoric(lTimeseriesGroup, lRunName, lOutFileName)

                    'while we've got it open, also output average total conc for each year
                    Dim lAverageAnnualFileName As String = lRunName & "AnnualAverage.txt"
                    If pSiteName = "White" And pLocation = 16 Then
                        lAverageAnnualFileName = lRunName & "Seg" & pLocation.ToString & "AnnualAverage.txt"
                    End If
                    If pSiteName = "MississippiMO" Or pSiteName = "Ohio" Then
                        lAverageAnnualFileName = lRunName & "Seg" & pLocation.tostring & "AnnualAverage.txt"
                    End If
                    Dim lHeader As String = lRunName & " Annual Average from " & lCsvName & " at Seg " & pLocation
                    WriteAverageAnnualFile(lAverageAnnualFileName, lTimSer1, pStartYearForSimulation, lHeader)

                    lTimeseriesCsv.Clear()
                    lTimeseriesGroup.Clear()
                Else
                    Logger.Msg("Unable to Open " & lCsvFileName)
                End If
            End If
        Next

        'patrick says these option plots are not needed, but the tables are
        Dim pMetalNames As New atcCollection
        pMetalNames.Add("As")
        pMetalNames.Add("Cd")
        pMetalNames.Add("Cu")
        pMetalNames.Add("Ni")
        pMetalNames.Add("Pb")
        pMetalNames.Add("Se")
        pMetalNames.Add("Tl")
        pMetalNames.Add("Zn")

        lSDate(0) = pComplianceDate : lSDate(1) = 1 : lSDate(2) = 1
        lSDateJ = Date2J(lSDate)
        lEDate(0) = pComplianceDate + 9 : lEDate(1) = 12 : lEDate(2) = 31
        lEdatej = Date2J(lEDate)

        Dim lTimeseriesOptionsGroup As New atcTimeseriesGroup
        Dim lTimeseriesCsvOption As New atcTimeseriesCSV.atcTimeseriesCSV

        'do post-compliance option plots
        For Each lMetalName As String In pMetalNames
            Dim lOutFileName As String = lMetalName & "Options.png"
            If pSiteName = "White" And pLocation = 16 Then
                lOutFileName = lMetalName & "Seg" & pLocation.ToString & "Options.png"
            End If
            If pSiteName = "MississippiMO" Or pSiteName = "Ohio" Then
                lOutFileName = lMetalName & "Seg" & pLocation.ToString & "Options.png"
            End If

            If Not FileExists(lOutFileName) Then
                Dim lOptionName As String = ""

                lOptionName = lMetalName & "Baseline"
                If FileExists(pTestPath & "\" & lOptionName & "\", True) Then
                    'baseline folder exists

                    'if start date file doesn't exist, copy from the first run name folder 
                    If Not FileExists(pTestPath & "\" & lOptionName & "\" & "Total_Concentration.csv.start") Then
                        FileCopy(pTestPath & "\SeBaseline\" & "Total_Concentration.csv.start", pTestPath & "\" & lOptionName & "\" & "Total_Concentration.csv.start")
                    End If

                    Dim lCsvFileName As String = pTestPath & "\" & lOptionName & "\" & lCsvName
                    If lTimeseriesCsvOption.Open(lCsvFileName) Then
                        Dim lTimSerX As atcTimeseries = lTimeseriesCsvOption.DataSets.ItemByKey(pLocation)
                        lTimSerX.Attributes.SetValue("YAxis", "Left")
                        lTimSerX.Attributes.SetValue("Constituent", "Baseline")
                        lTimeseriesGroup.Add(SubsetByDate(lTimSerX, _
                                                    lSDateJ, _
                                                    lEdatej, Nothing))

                        'while we've got it open, also output average total conc for each year
                        Dim lAverageAnnualFileName As String = lOptionName & "AnnualAverage.txt"
                        If pSiteName = "White" And pLocation = 16 Then
                            lAverageAnnualFileName = lOptionName & "Seg" & pLocation.ToString & "AnnualAverage.txt"
                        End If
                        If pSiteName = "MississippiMO" Or pSiteName = "Ohio" Then
                            lAverageAnnualFileName = lOptionName & "Seg" & pLocation.ToString & "AnnualAverage.txt"
                        End If
                        Dim lHeader As String = lOptionName & " Annual Average from " & lCsvName & " at Seg " & pLocation
                        WriteAverageAnnualFile(lAverageAnnualFileName, lTimSerX, pComplianceDate, lHeader)

                        lTimeseriesCsvOption.Clear()
                    Else
                        Logger.Msg("Unable to Open " & lCsvFileName)
                    End If
                End If

                lOptionName = lMetalName & "OptionA"
                If FileExists(pTestPath & "\" & lOptionName & "\", True) Then
                    'folder exists

                    'if start date file doesn't exist, copy from the first run name folder 
                    If Not FileExists(pTestPath & "\" & lOptionName & "\" & "Total_Concentration.csv.start") Then
                        FileCopy(pTestPath & "\SeBaseline\" & "Total_Concentration.csv.start", pTestPath & "\" & lOptionName & "\" & "Total_Concentration.csv.start")
                    End If

                    Dim lCsvFileName As String = pTestPath & "\" & lOptionName & "\" & lCsvName
                    If lTimeseriesCsvOption.Open(lCsvFileName) Then
                        Dim lTimSerA As atcTimeseries = lTimeseriesCsvOption.DataSets.ItemByKey(pLocation)
                        lTimSerA.Attributes.SetValue("YAxis", "Left")
                        lTimSerA.Attributes.SetValue("Constituent", "Option A")
                        lTimeseriesGroup.Add(SubsetByDate(lTimSerA, _
                                                    lSDateJ, _
                                                    lEdatej, Nothing))

                        'while we've got it open, also output average total conc for each year
                        Dim lAverageAnnualFileName As String = lOptionName & "AnnualAverage.txt"
                        If pSiteName = "White" And pLocation = 16 Then
                            lAverageAnnualFileName = lOptionName & "Seg" & pLocation.ToString & "AnnualAverage.txt"
                        End If
                        If pSiteName = "MississippiMO" Or pSiteName = "Ohio" Then
                            lAverageAnnualFileName = lOptionName & "Seg" & pLocation.ToString & "AnnualAverage.txt"
                        End If
                        Dim lHeader As String = lOptionName & " Annual Average from " & lCsvName & " at Seg " & pLocation
                        WriteAverageAnnualFile(lAverageAnnualFileName, lTimSerA, pComplianceDate, lHeader)

                        lTimeseriesCsvOption.Clear()
                    Else
                        Logger.Msg("Unable to Open " & lCsvFileName)
                    End If
                End If

                lOptionName = lMetalName & "OptionB"
                If FileExists(pTestPath & "\" & lOptionName & "\", True) Then
                    'folder exists

                    'if start date file doesn't exist, copy from the first run name folder 
                    If Not FileExists(pTestPath & "\" & lOptionName & "\" & "Total_Concentration.csv.start") Then
                        FileCopy(pTestPath & "\SeBaseline\" & "Total_Concentration.csv.start", pTestPath & "\" & lOptionName & "\" & "Total_Concentration.csv.start")
                    End If

                    Dim lCsvFileName As String = pTestPath & "\" & lOptionName & "\" & lCsvName
                    If lTimeseriesCsvOption.Open(lCsvFileName) Then
                        Dim lTimSerB As atcTimeseries = lTimeseriesCsvOption.DataSets.ItemByKey(pLocation)
                        lTimSerB.Attributes.SetValue("YAxis", "Left")
                        lTimSerB.Attributes.SetValue("Constituent", "Option B")
                        lTimeseriesGroup.Add(SubsetByDate(lTimSerB, _
                                                    lSDateJ, _
                                                    lEdatej, Nothing))

                        'while we've got it open, also output average total conc for each year
                        Dim lAverageAnnualFileName As String = lOptionName & "AnnualAverage.txt"
                        If pSiteName = "White" And pLocation = 16 Then
                            lAverageAnnualFileName = lOptionName & "Seg" & pLocation.ToString & "AnnualAverage.txt"
                        End If
                        If pSiteName = "MississippiMO" Or pSiteName = "Ohio" Then
                            lAverageAnnualFileName = lOptionName & "Seg" & pLocation.ToString & "AnnualAverage.txt"
                        End If
                        Dim lHeader As String = lOptionName & " Annual Average from " & lCsvName & " at Seg " & pLocation
                        WriteAverageAnnualFile(lAverageAnnualFileName, lTimSerB, pComplianceDate, lHeader)

                        lTimeseriesCsvOption.Clear()
                    Else
                        Logger.Msg("Unable to Open " & lCsvFileName)
                    End If
                End If

                lOptionName = lMetalName & "OptionC"
                If FileExists(pTestPath & "\" & lOptionName & "\", True) Then
                    'folder exists

                    'if start date file doesn't exist, copy from the first run name folder 
                    If Not FileExists(pTestPath & "\" & lOptionName & "\" & "Total_Concentration.csv.start") Then
                        FileCopy(pTestPath & "\SeBaseline\" & "Total_Concentration.csv.start", pTestPath & "\" & lOptionName & "\" & "Total_Concentration.csv.start")
                    End If

                    Dim lCsvFileName As String = pTestPath & "\" & lOptionName & "\" & lCsvName
                    If lTimeseriesCsvOption.Open(lCsvFileName) Then
                        Dim lTimSerC As atcTimeseries = lTimeseriesCsvOption.DataSets.ItemByKey(pLocation)
                        lTimSerC.Attributes.SetValue("YAxis", "Left")
                        lTimSerC.Attributes.SetValue("Constituent", "Option C")
                        lTimeseriesGroup.Add(SubsetByDate(lTimSerC, _
                                                    lSDateJ, _
                                                    lEdatej, Nothing))

                        'while we've got it open, also output average total conc for each year
                        Dim lAverageAnnualFileName As String = lOptionName & "AnnualAverage.txt"
                        If pSiteName = "White" And pLocation = 16 Then
                            lAverageAnnualFileName = lOptionName & "Seg" & pLocation.ToString & "AnnualAverage.txt"
                        End If
                        If pSiteName = "MississippiMO" Or pSiteName = "Ohio" Then
                            lAverageAnnualFileName = lOptionName & "Seg" & pLocation.ToString & "AnnualAverage.txt"
                        End If
                        Dim lHeader As String = lOptionName & " Annual Average from " & lCsvName & " at Seg " & pLocation
                        WriteAverageAnnualFile(lAverageAnnualFileName, lTimSerC, pComplianceDate, lHeader)

                        lTimeseriesCsvOption.Clear()
                    Else
                        Logger.Msg("Unable to Open " & lCsvFileName)
                    End If
                End If

                lOptionName = lMetalName & "OptionD"
                If FileExists(pTestPath & "\" & lOptionName & "\", True) Then
                    'folder exists

                    'if start date file doesn't exist, copy from the first run name folder 
                    If Not FileExists(pTestPath & "\" & lOptionName & "\" & "Total_Concentration.csv.start") Then
                        FileCopy(pTestPath & "\SeBaseline\" & "Total_Concentration.csv.start", pTestPath & "\" & lOptionName & "\" & "Total_Concentration.csv.start")
                    End If

                    Dim lCsvFileName As String = pTestPath & "\" & lOptionName & "\" & lCsvName
                    If lTimeseriesCsvOption.Open(lCsvFileName) Then
                        Dim lTimSerD As atcTimeseries = lTimeseriesCsvOption.DataSets.ItemByKey(pLocation)
                        lTimSerD.Attributes.SetValue("YAxis", "Left")
                        lTimSerD.Attributes.SetValue("Constituent", "Option D")
                        lTimeseriesGroup.Add(SubsetByDate(lTimSerD, _
                                                    lSDateJ, _
                                                    lEdatej, Nothing))

                        'while we've got it open, also output average total conc for each year
                        Dim lAverageAnnualFileName As String = lOptionName & "AnnualAverage.txt"
                        If pSiteName = "White" And pLocation = 16 Then
                            lAverageAnnualFileName = lOptionName & "Seg" & pLocation.ToString & "AnnualAverage.txt"
                        End If
                        If pSiteName = "MississippiMO" Or pSiteName = "Ohio" Then
                            lAverageAnnualFileName = lOptionName & "Seg" & pLocation.ToString & "AnnualAverage.txt"
                        End If
                        Dim lHeader As String = lOptionName & " Annual Average from " & lCsvName & " at Seg " & pLocation
                        WriteAverageAnnualFile(lAverageAnnualFileName, lTimSerD, pComplianceDate, lHeader)

                        lTimeseriesCsvOption.Clear()
                    Else
                        Logger.Msg("Unable to Open " & lCsvFileName)
                    End If
                End If

                GraphTimeseriesOptions(lTimeseriesGroup, lMetalName, lOutFileName, "Computed Tot Conc Ug/L", "")
                lTimeseriesGroup.Clear()
            End If
        Next

    End Sub

    Sub GraphTimeseriesBaselineHistoric(ByVal aDataGroup As atcTimeseriesGroup, ByVal aRunName As String, ByVal aOutFile As String)
        Dim lZgc As ZedGraphControl = CreateZgc()
        Dim lGrapher As New clsGraphTime(aDataGroup, lZgc)
        Dim lPane As GraphPane = lZgc.MasterPane.PaneList(0)
        Dim lCurve As ZedGraph.LineItem

        For i As Integer = 0 To lPane.CurveList.Count - 1
            lCurve = lPane.CurveList.Item(i)
            If i = 0 Then
                lCurve.Color = Drawing.Color.Blue
            ElseIf i = 1 Then
                lCurve.Color = Drawing.Color.Green
            End If
        Next i

        lPane.YAxis.Scale.Min = 0

        If lPane.CurveList.Count = 1 Then
            lPane.XAxis.Title.Text = aRunName.Substring(0, 2) & " at Segment " & pLocation
        Else
            lPane.XAxis.Title.Text = aRunName.Substring(0, 2) & " at Segments " & pLocation & " and " & (pLocation - 1).ToString
        End If
        lZgc.Width = 1200

        lZgc.SaveIn(aOutFile)
        lZgc.Dispose()
    End Sub

    Sub GraphTimeseriesOptions(ByVal aDataGroup As atcTimeseriesGroup, ByVal aRunName As String, ByVal aOutFile As String, ByVal aUnits As String, ByVal aSiteName As String)
        Dim lZgc As ZedGraphControl = CreateZgc()
        Dim lGrapher As New clsGraphTime(aDataGroup, lZgc)
        Dim lPane As GraphPane = lZgc.MasterPane.PaneList(0)
        Dim lCurve As ZedGraph.LineItem

        For i As Integer = 0 To lPane.CurveList.Count - 1
            lCurve = lPane.CurveList.Item(i)
            If lCurve.Label.Text = "Computed Baseline" Then
                lCurve.Color = Drawing.Color.Blue
                lCurve.Label.Text = "Baseline"
            ElseIf lCurve.Label.Text = "Computed Option A" Then
                lCurve.Color = Drawing.Color.Green
                lCurve.Label.Text = "Option A"
            ElseIf lCurve.Label.Text = "Computed Option B" Then
                lCurve.Color = Drawing.Color.Red
                lCurve.Label.Text = "Option B"
            ElseIf lCurve.Label.Text = "Computed Option C" Then
                lCurve.Color = Drawing.Color.Cyan
                lCurve.Label.Text = "Option C"
            ElseIf lCurve.Label.Text = "Computed Option D" Then
                lCurve.Color = Drawing.Color.Magenta
                lCurve.Label.Text = "Option D"
            End If
        Next i

        If aOutFile.Contains("Composite") Then
            lCurve = lPane.CurveList.Item(0)
            lCurve.Color = Drawing.Color.Blue
            lCurve = lPane.CurveList.Item(1)
            lCurve.Color = Drawing.Color.Purple
            If Not aOutFile.Contains("CompositeRev") Then
                lCurve = lPane.CurveList.Item(2)
                lCurve.Color = Drawing.Color.LightGray
                lCurve.Color = Drawing.Color.FromKnownColor(Drawing.KnownColor.Gray)
                lCurve.Color = Drawing.Color.LightBlue
            End If

            'lZgc.Width = 2400
            lZgc.Width = 1440 'may sacrifice too much resolution doing this?
            lZgc.Height = 300

            If aOutFile.Contains("CompositeRev") Then
                lZgc.Width = 1440
                lZgc.Height = 600
            End If

            lPane.Legend.IsVisible = False
            lPane.XAxis.Title.Text = ""
            lPane.YAxis.Type = ZedGraph.AxisType.Log
            'lPane.YAxis.Scale.Min = 0.001

            'add up to seven additional solid lines
            Dim lNationalModelBaseline As Double = 0.0
            Dim lNationalModelOptionD As Double = 0.0
            Dim lAquaBenchmarkAcute As Double = 0.0
            Dim lAquaBenchmarkChronic As Double = 0.0
            Dim lHealthBenchmarkWO As Double = 0.0
            Dim lHealthBenchmarkO As Double = 0.0
            Dim lMCL As Double = 0.0

            Dim lLabel As String = ""
            GetBenchmarks(aSiteName, aUnits, aRunName, _
                          lNationalModelBaseline, _
                          lNationalModelOptionD, _
                          lAquaBenchmarkAcute, _
                          lAquaBenchmarkChronic, _
                          lHealthBenchmarkWO, _
                          lHealthBenchmarkO, _
                          lMCL, _
                          lLabel)
            lPane.YAxis.Title.Text = lLabel

            'now add them
            If lNationalModelBaseline > 0.0 Then
                Dim lLine1 As ZedGraph.LineItem = AddLineMine(lPane, 0.0, lNationalModelBaseline * 1000, Drawing.Drawing2D.DashStyle.Dash)
                lLine1.Color = Drawing.Color.Blue
                lLine1.Line.Width = 2
            End If
            If lNationalModelOptionD > 0.0 Then
                Dim lLine2 As ZedGraph.LineItem = AddLineMine(lPane, 0.0, lNationalModelOptionD * 1000, Drawing.Drawing2D.DashStyle.Dash)
                lLine2.Color = Drawing.Color.Purple
                lLine2.Line.Width = 2
            End If
            If lAquaBenchmarkAcute > 0.0 Then
                Dim lLine3 As ZedGraph.LineItem = AddLineMine(lPane, 0.0, lAquaBenchmarkAcute * 1000, Drawing.Drawing2D.DashStyle.Dot)
                lLine3.Color = Drawing.Color.Brown
                lLine3.Line.Width = 2
            End If
            If lAquaBenchmarkChronic > 0.0 Then
                Dim lLine4 As ZedGraph.LineItem = AddLineMine(lPane, 0.0, lAquaBenchmarkChronic * 1000, Drawing.Drawing2D.DashStyle.Dot)
                lLine4.Color = Drawing.Color.Green
                lLine4.Line.Width = 2
            End If
            If lHealthBenchmarkWO > 0.0 Then
                Dim lLine5 As ZedGraph.LineItem = AddLineMine(lPane, 0.0, lHealthBenchmarkWO * 1000, Drawing.Drawing2D.DashStyle.DashDot)
                lLine5.Color = Drawing.Color.Orange
                lLine5.Line.Width = 2
            End If
            If lHealthBenchmarkO > 0.0 Then
                Dim lLine6 As ZedGraph.LineItem = AddLineMine(lPane, 0.0, lHealthBenchmarkO * 1000, Drawing.Drawing2D.DashStyle.DashDot)
                lLine6.Color = Drawing.Color.Red
                lLine6.Line.Width = 2
            End If
            If lMCL > 0.0 Then
                Dim lLine7 As ZedGraph.LineItem = AddLineMine(lPane, 0.0, lMCL * 1000, Drawing.Drawing2D.DashStyle.DashDotDot)
                lLine7.Color = Drawing.Color.Pink
                lLine7.Line.Width = 2
            End If

            If lNationalModelBaseline > 0 Then
                If (lNationalModelBaseline * 1000 < lPane.YAxis.Scale.Min) Then
                    lPane.YAxis.Scale.Min = lNationalModelBaseline * 1000
                End If
            End If
            If lNationalModelOptionD > 0 Then
                If (lNationalModelOptionD * 1000 < lPane.YAxis.Scale.Min) Then
                    lPane.YAxis.Scale.Min = lNationalModelOptionD * 1000
                End If
            End If

            'If lPane.YAxis.Scale.Min >= 1 And lPane.YAxis.Scale.Min <= 10 And _
            '   lPane.YAxis.Scale.Max >= 1 And lPane.YAxis.Scale.Max <= 10 Then
            '    'special case, use 1 log cycle
            '    lPane.YAxis.Scale.Min = 1
            '    lPane.YAxis.Scale.Max = 10
            'ElseIf lPane.YAxis.Scale.Min >= 0.1 And lPane.YAxis.Scale.Min <= 1 And _
            '       lPane.YAxis.Scale.Max >= 0.1 And lPane.YAxis.Scale.Max <= 1 Then
            '    'special case, use 1 log cycle
            '    lPane.YAxis.Scale.Min = 0.1
            '    lPane.YAxis.Scale.Max = 1
            'ElseIf lPane.YAxis.Scale.Min >= 0.01 And lPane.YAxis.Scale.Min <= 0.1 And _
            '   lPane.YAxis.Scale.Max >= 0.01 And lPane.YAxis.Scale.Max <= 0.1 Then
            '    'special case, use 1 log cycle
            '    lPane.YAxis.Scale.Min = 0.01
            '    lPane.YAxis.Scale.Max = 0.1
            'ElseIf lPane.YAxis.Scale.Min >= 10 And lPane.YAxis.Scale.Min <= 100 And _
            '    lPane.YAxis.Scale.Max >= 10 And lPane.YAxis.Scale.Max <= 100 Then
            '    'special case, use 1 log cycle
            '    lPane.YAxis.Scale.Min = 10
            '    lPane.YAxis.Scale.Max = 100
            'End If

            If lPane.YAxis.Scale.Min = 0.0 Then
                'last ditch effort to not have zero at the bottom
                lPane.YAxis.Scale.Min = 10
                For Each lCurveX As ZedGraph.LineItem In lPane.CurveList
                    Dim lxMin As Double
                    Dim lxMax As Double
                    Dim lyMin As Double
                    Dim lyMax As Double
                    lCurveX.GetRange(lxMin, lxMax, lyMin, lyMax, True, False, lPane)
                    If lyMin < lPane.YAxis.Scale.Min Then
                        If lyMin > 0.000001 Then
                            lPane.YAxis.Scale.Min = lyMin
                        Else
                            lPane.YAxis.Scale.Min = lPane.YAxis.Scale.Min / 10
                        End If
                    End If
                Next
            End If

            If lPane.YAxis.Scale.Max < lNationalModelBaseline * 1000 Then
                lPane.YAxis.Scale.Max = lNationalModelBaseline * 1000 * 1.1  'make max 10 percent higher 
            End If

        Else
            lZgc.Width = 1200
            lPane.XAxis.Title.Text = aRunName.Substring(0, 2) & " at Segment " & pLocation
            lPane.YAxis.Scale.Min = 0
            lPane.YAxis.Title.Text = aUnits
        End If

        lZgc.SaveIn(aOutFile)
        lZgc.Dispose()
    End Sub

    Sub WriteAverageAnnualFile(ByVal aAverageAnnualFileName As String, ByVal aTimSer As atcTimeseries, ByVal aStartYear As Integer, ByVal aHeader As String)
        Dim aAnnualTimSer As atcTimeseries = atcData.modTimeseriesMath.Aggregate(aTimSer, atcTimeUnit.TUYear, 1, atcTran.TranAverSame)
        Dim lWrite As New IO.StreamWriter(aAverageAnnualFileName, False)
        Dim lDate(5) As Integer

        Dim lStr As String = aHeader
        lWrite.WriteLine(lStr)

        For lIndex As Integer = 1 To aAnnualTimSer.numValues
            J2Date(aAnnualTimSer.Dates.Value(lIndex), lDate)
            If lDate(0) >= aStartYear Then
                lStr = lDate(0).ToString & " " & DoubleToString(aAnnualTimSer.Value(lIndex))
                lWrite.WriteLine(lStr)
            End If
        Next
        lWrite.Close()
    End Sub

    Private Sub ComputeThreeMonthRollingAverages()
        ChDriveDir(pTestPath & "\OutputPlots")
        Dim lCsvName As String = "Dissolved_Concentration.csv"

        Dim lSDate(5) As Integer : lSDate(0) = pComplianceDate : lSDate(1) = 1 : lSDate(2) = 1
        Dim lSDateJ As Double = Date2J(lSDate)

        Dim pMetalNames As New atcCollection
        'pMetalNames.Add("As")
        'pMetalNames.Add("Cd")
        'pMetalNames.Add("Cu")
        'pMetalNames.Add("Ni")
        'pMetalNames.Add("Pb")
        pMetalNames.Add("Se")    'patrick says only selenium for production runs
        'pMetalNames.Add("Tl")
        'pMetalNames.Add("Zn")

        Dim pOptions As New atcCollection
        pOptions.Add("Baseline")
        pOptions.Add("OptionA")
        pOptions.Add("OptionB")
        pOptions.Add("OptionC")
        pOptions.Add("OptionD")

        Dim lTimeseriesCsvOption As New atcTimeseriesCSV.atcTimeseriesCSV
        Dim lTimeseriesGroup As New atcTimeseriesGroup

        'do post-compliance three month rolling averages
        For Each lMetalName As String In pMetalNames
            For Each lOption As String In pOptions
                Dim lOptionName As String = ""
                lOptionName = lMetalName & lOption
                Dim lThreeMonthAverageFileName As String = lOptionName & "ThreeMonthRollingAverage.csv"

                If Not FileExists(lThreeMonthAverageFileName) Then
                    If FileExists(pTestPath & "\" & lOptionName & "\", True) Then
                        'option folder exists

                        'if start date file doesn't exist, copy from the total conc 
                        If Not FileExists(pTestPath & "\" & lOptionName & "\" & "Dissolved_Concentration.csv.start") Then
                            FileCopy(pTestPath & "\" & lOptionName & "\" & "Total_Concentration.csv.start", pTestPath & "\" & lOptionName & "\" & "Dissolved_Concentration.csv.start")
                        End If

                        Dim lCsvFileName As String = pTestPath & "\" & lOptionName & "\" & lCsvName
                        If lTimeseriesCsvOption.Open(lCsvFileName) Then

                            Dim lHeader As String = lOptionName & " Three Month Rolling Average from " & lCsvName

                            For lLoc As Integer = 1 To pLocation
                                Dim lTimSerX As atcTimeseries = lTimeseriesCsvOption.DataSets.ItemByKey(lLoc)
                                lTimeseriesGroup.Add(lTimSerX)
                            Next

                            WriteThreeMonthAverageFile(lThreeMonthAverageFileName, lTimeseriesGroup, pComplianceDate, lHeader)

                            lTimeseriesCsvOption.Clear()
                            lTimeseriesGroup.Clear()
                        Else
                            Logger.Msg("Unable to Open " & lCsvFileName)
                        End If
                    End If

                End If
            Next
        Next
    End Sub

    Sub WriteThreeMonthAverageFile(ByVal aThreeMonthFileName As String, ByVal aTimSerGroup As atcTimeseriesGroup, ByVal aStartYear As Integer, ByVal aHeader As String)
        Dim aTimSer1 As atcTimeseries = aTimSerGroup(0)
        Dim lWrite As New IO.StreamWriter(aThreeMonthFileName, False)
        Dim lDate(5) As Integer

        Dim lStr As String = aHeader
        lWrite.WriteLine(lStr)
        lStr = "Year, Month "
        For lIndex As Integer = 1 To aTimSerGroup.Count
            lStr &= ", " & lIndex.ToString
        Next
        lWrite.WriteLine(lStr)

        'make a first pass to find the start of each desired month
        Dim lMonthStarts As New atcCollection
        For lIndex As Integer = 1 To aTimSer1.numValues
            J2Date(aTimSer1.Dates.Value(lIndex), lDate)
            If lDate(0) >= aStartYear Then
                If lDate(2) = 1 Then
                    lMonthStarts.Add(lIndex)
                End If
            End If
        Next

        Dim lStartingIndex As Integer = 0
        Dim lEndingIndex As Integer = 0
        For lMonth As Integer = 0 To lMonthStarts.Count - 3
            lStartingIndex = lMonthStarts(lMonth)
            If lMonth = lMonthStarts.Count - 3 Then
                'last month
                lEndingIndex = aTimSer1.numValues
            Else
                lEndingIndex = lMonthStarts(lMonth + 3) - 1
            End If
            'get average of values between start and ending index and write
            J2Date(aTimSer1.Dates.Value(lEndingIndex), lDate)
            Dim lYr As Integer = lDate(0)
            Dim lMo As Integer = lDate(1)
            lStr = lYr.ToString & ", " & lMo.ToString
            For Each lTimSer As atcTimeseries In aTimSerGroup
                Dim lThreeMonthSum As Double = 0.0
                For lIndexX As Integer = lStartingIndex To lEndingIndex
                    lThreeMonthSum += lTimSer.Value(lIndexX)
                Next
                Dim lValue As String = Format((lThreeMonthSum / (lEndingIndex - lStartingIndex + 1)), "0.####")
                lStr = lStr & ", " & lValue
            Next
            lWrite.WriteLine(lStr)
        Next
        lWrite.Close()
    End Sub

    Private Sub DoBaselineHistoricGraphsMultipleLocations()

        Dim lCsvName As String = ""

        Dim pRunNames As New atcCollection
        pRunNames.Add("AsBaselineHistoric")
        pRunNames.Add("CdBaselineHistoric")
        pRunNames.Add("CuBaselineHistoric")
        pRunNames.Add("NiBaselineHistoric")
        pRunNames.Add("PbBaselineHistoric")
        pRunNames.Add("SeBaselineHistoric")
        pRunNames.Add("TlBaselineHistoric")
        pRunNames.Add("ZnBaselineHistoric")

        lCsvName = "Total_Concentration.csv"

        Dim lTimeseriesGroup As New atcTimeseriesGroup
        Dim lTimeseriesCsv As New atcTimeseriesCSV.atcTimeseriesCSV

        ChDriveDir(pTestPath & "\OutputPlots")

        Dim lSDate(5) As Integer : lSDate(0) = pStartYearForBaselinePlot : lSDate(1) = 1 : lSDate(2) = 1
        Dim lSDateJ As Double = Date2J(lSDate)
        Dim lEDate(5) As Integer : lEDate(0) = 2020 : lEDate(1) = 12 : lEDate(2) = 31
        Dim lEdatej As Double = Date2J(lEDate)

        'do baseline historic plots
        For Each lRunName As String In pRunNames

            Dim lOutFileName As String = lRunName & "IRWandNextDownstream.png"

            If Not FileExists(lOutFileName) Then

                'if start date file doesn't exist, copy from the first run name folder 
                If Not FileExists(pTestPath & "\" & lRunName & "\" & "Total_Concentration.csv.start") Then
                    FileCopy(pTestPath & "\" & pRunNames(0) & "\" & "Total_Concentration.csv.start", pTestPath & "\" & lRunName & "\" & "Total_Concentration.csv.start")
                End If

                Dim lCsvFileName As String = pTestPath & "\" & lRunName & "\" & lCsvName
                If lTimeseriesCsv.Open(lCsvFileName) Then

                    Dim lTimSer1 As atcTimeseries = lTimeseriesCsv.DataSets.ItemByKey(pLocation)

                    lTimSer1.Attributes.SetValue("YAxis", "Left")
                    lTimeseriesGroup.Add(SubsetByDate(lTimSer1, _
                                                lSDateJ, _
                                                lEdatej, Nothing))

                    Dim lTimSer2 As atcTimeseries = lTimeseriesCsv.DataSets.ItemByKey(pLocation - 1)

                    lTimSer2.Attributes.SetValue("YAxis", "Left")
                    lTimeseriesGroup.Add(SubsetByDate(lTimSer2, _
                                                lSDateJ, _
                                                lEdatej, Nothing))

                    GraphTimeseriesBaselineHistoric(lTimeseriesGroup, lRunName, lOutFileName)

                    lTimeseriesCsv.Clear()
                    lTimeseriesGroup.Clear()
                Else
                    Logger.Msg("Unable to Open " & lCsvFileName)
                End If
            End If
        Next
    End Sub

    Private Sub DoERGCompositeGraph()

        Dim lCsvName As String = ""

        Dim pMetalNames As New atcCollection
        pMetalNames.Add("As")
        pMetalNames.Add("Cd")
        pMetalNames.Add("Cu")
        pMetalNames.Add("Ni")
        pMetalNames.Add("Pb")
        pMetalNames.Add("Se")
        pMetalNames.Add("Tl")
        pMetalNames.Add("Zn")

        Dim pTypes As New atcCollection
        pTypes.Add("Total")
        pTypes.Add("Dissolved")

        For Each lType As String In pTypes
            lCsvName = lType & "_Concentration.csv"

            Dim lTimeseriesGroup As New atcTimeseriesGroup

            ChDriveDir(pTestPath & "\OutputPlots")

            For Each lMetal As String In pMetalNames

                Dim lOutFileName As String = pSiteName & lType & lMetal.Substring(0, 2) & "Composite" & "New.png"
                If pSiteName = "MississippiMO" Or pSiteName = "Ohio" Then
                    lOutFileName = pSiteName & lType & lMetal.Substring(0, 2) & "CompositeSeg" & pLocation.ToString & "New.png"
                End If

                If Not FileExists(lOutFileName) Then
                    'get baseline historic data
                    Dim lTimeseriesCsv1 As New atcTimeseriesCSV.atcTimeseriesCSV
                    Dim lRunName As String = lMetal & "BaselineHistoric"
                    Dim lCsvFileName As String = pTestPath & "\" & lRunName & "\" & lCsvName

                    If Not FileExists(lCsvFileName & ".start") Then
                        FileCopy(pTestPath & "\" & lRunName & "\" & "Total_Concentration.csv.start", lCsvFileName & ".start")
                    End If

                    If lTimeseriesCsv1.Open(lCsvFileName) Then

                        Dim lTimSer1 As atcTimeseries = lTimeseriesCsv1.DataSets.ItemByKey(pLocation)

                        lTimSer1.Attributes.SetValue("YAxis", "Left")

                        Dim lSDate(5) As Integer : lSDate(0) = pComplianceDate - 10 : lSDate(1) = 1 : lSDate(2) = 1
                        If pSiteName = "LakeSinclair" Then
                            'special case -- limited data
                            lSDate(0) = pComplianceDate - 7 : lSDate(1) = 2 : lSDate(2) = 2
                        End If
                        Dim lSDateJ As Double = Date2J(lSDate)
                        Dim lEDate(5) As Integer : lEDate(0) = pComplianceDate - 1 : lEDate(1) = 12 : lEDate(2) = 31
                        Dim lEdatej As Double = Date2J(lEDate)

                        lTimeseriesGroup.Add(SubsetByDate(lTimSer1, _
                                                    lSDateJ, _
                                                    lEdatej, Nothing))
                        lTimeseriesCsv1.Clear()
                    Else
                        Logger.Msg("Unable to Open " & lCsvFileName)
                    End If

                    'get option d post-compliance data
                    Dim lTimeseriesCsv3 As New atcTimeseriesCSV.atcTimeseriesCSV
                    lRunName = lMetal & "OptionD"
                    lCsvFileName = pTestPath & "\" & lRunName & "\" & lCsvName
                    If Not FileExists(lCsvFileName) Then
                        lRunName = lMetal & "OptionC"
                        lCsvFileName = pTestPath & "\" & lRunName & "\" & lCsvName
                    End If

                    If Not FileExists(lCsvFileName & ".start") Then
                        FileCopy(pTestPath & "\" & lRunName & "\" & "Total_Concentration.csv.start", lCsvFileName & ".start")
                    End If

                    If lTimeseriesCsv3.Open(lCsvFileName) Then

                        Dim lTimSerD As atcTimeseries = lTimeseriesCsv3.DataSets.ItemByKey(pLocation)

                        lTimSerD.Attributes.SetValue("YAxis", "Left")

                        Dim lSDate(5) As Integer : lSDate(0) = pComplianceDate : lSDate(1) = 1 : lSDate(2) = 1
                        If pSiteName = "LakeSinclair" Then
                            'special case -- limited data
                            lSDate(0) = pComplianceDate : lSDate(1) = 1 : lSDate(2) = 2
                        End If
                        Dim lSDateJ As Double = Date2J(lSDate)
                        Dim lEDate(5) As Integer : lEDate(0) = pComplianceDate + 9 : lEDate(1) = 12 : lEDate(2) = 31
                        If pSiteName = "LakeSinclair" Then
                            'special case -- limited data
                            lEDate(0) = pComplianceDate + 6 : lEDate(1) = 11 : lEDate(2) = 30
                        End If
                        Dim lEdatej As Double = Date2J(lEDate)

                        lTimeseriesGroup.Add(SubsetByDate(lTimSerD, _
                                                    lSDateJ, _
                                                    lEdatej, Nothing))
                        lTimeseriesCsv3.Clear()
                    Else
                        Logger.Msg("Unable to Open " & lCsvFileName)
                    End If

                    'get baseline post-compliance data
                    Dim lTimeseriesCsv2 As New atcTimeseriesCSV.atcTimeseriesCSV
                    lRunName = lMetal & "Baseline"
                    lCsvFileName = pTestPath & "\" & lRunName & "\" & lCsvName

                    If Not FileExists(lCsvFileName & ".start") Then
                        FileCopy(pTestPath & "\" & lRunName & "\" & "Total_Concentration.csv.start", lCsvFileName & ".start")
                    End If

                    If lTimeseriesCsv2.Open(lCsvFileName) Then

                        Dim lTimSer2 As atcTimeseries = lTimeseriesCsv2.DataSets.ItemByKey(pLocation)

                        lTimSer2.Attributes.SetValue("YAxis", "Left")

                        Dim lSDate(5) As Integer : lSDate(0) = pComplianceDate : lSDate(1) = 1 : lSDate(2) = 1
                        If pSiteName = "LakeSinclair" Then
                            'special case -- limited data
                            lSDate(0) = pComplianceDate : lSDate(1) = 1 : lSDate(2) = 2
                        End If
                        Dim lSDateJ As Double = Date2J(lSDate)
                        Dim lEDate(5) As Integer : lEDate(0) = pComplianceDate + 9 : lEDate(1) = 12 : lEDate(2) = 31
                        If pSiteName = "LakeSinclair" Then
                            'special case -- limited data
                            lEDate(0) = pComplianceDate + 6 : lEDate(1) = 11 : lEDate(2) = 30
                        End If
                        Dim lEdatej As Double = Date2J(lEDate)

                        lTimeseriesGroup.Add(SubsetByDate(lTimSer2, _
                                                    lSDateJ, _
                                                    lEdatej, Nothing))
                        lTimeseriesCsv2.Clear()
                    Else
                        Logger.Msg("Unable to Open " & lCsvFileName)
                    End If

                    GraphTimeseriesOptions(lTimeseriesGroup, lMetal, lOutFileName, lType, pSiteName)

                    lTimeseriesGroup.Clear()
                End If

            Next
        Next

    End Sub

    Public Function AddLineMine(ByRef aPane As ZedGraph.GraphPane, _
                            ByVal aACoef As Double, _
                            ByVal aBCoef As Double, _
                   Optional ByVal aLineStyle As Drawing.Drawing2D.DashStyle = Drawing.Drawing2D.DashStyle.Solid, _
                   Optional ByVal aTag As String = Nothing) As LineItem
        With aPane
            Dim lXValues(2) As Double
            Dim lYValues(2) As Double
            Dim lStep As Double = (.XAxis.Scale.Max - .XAxis.Scale.Min) / lXValues.GetUpperBound(0)
            For lIndex As Integer = 0 To lXValues.GetUpperBound(0)
                lXValues(lIndex) = .XAxis.Scale.Min + (lStep * lIndex)
                lYValues(lIndex) = (aACoef * lXValues(lIndex)) + aBCoef
            Next
            Dim lCurve As LineItem = .AddCurve("", lXValues, lYValues, Drawing.Color.Blue, SymbolType.None)
            lCurve.Line.Style = aLineStyle
            lCurve.Tag = aTag
            Return lCurve
        End With
    End Function

    Private Sub DoGraphAtMaxConcLocation()
        Dim lCsvName As String = ""

        Dim pMetalNames As New atcCollection
        pMetalNames.Add("As")
        pMetalNames.Add("Cd")
        pMetalNames.Add("Cu")
        pMetalNames.Add("Ni")
        pMetalNames.Add("Pb")
        pMetalNames.Add("Se")
        pMetalNames.Add("Tl")
        pMetalNames.Add("Zn")

        Dim lRun As String = "BaselineHistoric"

        Dim pTypes As New atcCollection
        pTypes.Add("Total")
        'pTypes.Add("Dissolved")

        For Each lType As String In pTypes
            lCsvName = lType & "_Concentration.csv"

            Dim lTimeseriesGroup As New atcTimeseriesGroup

            ChDriveDir(pTestPath & "\OutputPlots")

            For Each lMetal As String In pMetalNames

                Dim lOutFileName As String = pSiteName & lType & lMetal.Substring(0, 2) & "MaxConc" & ".png"

                If Not FileExists(lOutFileName) Then
                    'get baseline historic data
                    Dim lTimeseriesCsv1 As New atcTimeseriesCSV.atcTimeseriesCSV
                    Dim lRunName As String = lMetal & lRun
                    Dim lCsvFileName As String = pTestPath & "\" & lRunName & "\" & lCsvName

                    If Not FileExists(lCsvFileName & ".start") Then
                        FileCopy(pTestPath & "\" & lRunName & "\" & "Total_Concentration.csv.start", lCsvFileName & ".start")
                    End If

                    If lTimeseriesCsv1.Open(lCsvFileName) Then

                        'determine location of maximum concentration
                        Dim lMaxVal As Double = -1.0
                        Dim lMaxLoc As Integer = 0

                        For lIndex As Integer = 1 To lTimeseriesCsv1.DataSets.Count
                            Dim lTimeseries As atcTimeseries = lTimeseriesCsv1.DataSets.ItemByKey(lIndex)
                            If lTimeseries.Attributes.GetValue("Max") > lMaxVal Then
                                lMaxVal = lTimeseries.Attributes.GetValue("Max")
                                lMaxLoc = lIndex
                            End If
                        Next

                        Dim lTimSer1 As atcTimeseries = lTimeseriesCsv1.DataSets.ItemByKey(lMaxLoc)
                        pLocation = lMaxLoc

                        lTimSer1.Attributes.SetValue("YAxis", "Left")

                        Dim lSDate(5) As Integer : lSDate(0) = pComplianceDate - 10 : lSDate(1) = 1 : lSDate(2) = 1
                        If pSiteName = "LakeSinclair" Then
                            'special case -- limited data
                            lSDate(0) = pComplianceDate - 7 : lSDate(1) = 2 : lSDate(2) = 2
                        End If
                        Dim lSDateJ As Double = Date2J(lSDate)
                        Dim lEDate(5) As Integer : lEDate(0) = pComplianceDate - 1 : lEDate(1) = 12 : lEDate(2) = 31
                        Dim lEdatej As Double = Date2J(lEDate)

                        lTimeseriesGroup.Add(SubsetByDate(lTimSer1, _
                                                    lSDateJ, _
                                                    lEdatej, Nothing))
                        lTimeseriesCsv1.Clear()
                    Else
                        Logger.Msg("Unable to Open " & lCsvFileName)
                    End If

                    GraphTimeseriesBaselineHistoric(lTimeseriesGroup, lRunName, lOutFileName)

                    lTimeseriesGroup.Clear()
                End If

            Next
        Next
    End Sub

    Private Sub DoExceedanceSummary()
        Dim lCsvName As String = ""

        Dim pMetalNames As New atcCollection
        pMetalNames.Add("As")
        pMetalNames.Add("Cd")
        pMetalNames.Add("Cu")
        pMetalNames.Add("Ni")
        pMetalNames.Add("Pb")
        pMetalNames.Add("Se")
        pMetalNames.Add("Tl")
        pMetalNames.Add("Zn")

        Dim pRunNames As New atcCollection
        pRunNames.Add("Baseline")
        pRunNames.Add("OptionD")

        Dim pTypes As New atcCollection
        pTypes.Add("Total")
        pTypes.Add("Dissolved")

        ChDriveDir(pTestPath & "\OutputPlots")
        Dim lOutFileName As String = pSiteName & "ExceedanceSummary.txt"

        If Not FileExists(lOutFileName) Then
            Dim lWrite As New IO.StreamWriter(lOutFileName, False)
            Dim lStr As String = ""
            Dim lDayPercent As Double = 0.0

            For Each lType As String In pTypes
                lCsvName = lType & "_Concentration.csv"

                Dim lTimeseriesGroup As New atcTimeseriesGroup

                For Each lMetal As String In pMetalNames

                    Dim lNationalModelBaseline As Double = 0.0
                    Dim lNationalModelOptionD As Double = 0.0
                    Dim lAquaBenchmarkAcute As Double = 0.0
                    Dim lAquaBenchmarkChronic As Double = 0.0
                    Dim lHealthBenchmarkWO As Double = 0.0
                    Dim lHealthBenchmarkO As Double = 0.0
                    Dim lMCL As Double = 0.0

                    Dim lLabel As String = ""
                    GetBenchmarks(pSiteName, lType, lMetal, _
                                  lNationalModelBaseline, _
                                  lNationalModelOptionD, _
                                  lAquaBenchmarkAcute, _
                                  lAquaBenchmarkChronic, _
                                  lHealthBenchmarkWO, _
                                  lHealthBenchmarkO, _
                                  lMCL, _
                                  lLabel)

                    Dim lBenthicSedimentCriterion As Double = 0.0
                    If lMetal = "As" Then
                        lBenthicSedimentCriterion = 5.9
                    ElseIf lMetal = "Cd" Then
                        lBenthicSedimentCriterion = 0.596
                    ElseIf lMetal = "Cu" Then
                        lBenthicSedimentCriterion = 35.7
                    ElseIf lMetal = "Pb" Then
                        lBenthicSedimentCriterion = 35
                    ElseIf lMetal = "Ni" Then
                        lBenthicSedimentCriterion = 18
                    ElseIf lMetal = "Zn" Then
                        lBenthicSedimentCriterion = 123
                    End If

                    For Each lRun As String In pRunNames
                        'get data
                        Dim lTimeseriesCsv1 As New atcTimeseriesCSV.atcTimeseriesCSV
                        Dim lRunName As String = lMetal & lRun
                        Dim lCsvFileName As String = pTestPath & "\" & lRunName & "\" & lCsvName

                        If Not FileExists(lCsvFileName) Then
                            'try option c instead
                            lCsvFileName = pTestPath & "\" & lMetal & "OptionC" & "\" & lCsvName
                        End If

                        If lTimeseriesCsv1.Open(lCsvFileName) Then

                            'Dim lAquaBenchmarkAcute As Double = 0.0
                            'Dim lAquaBenchmarkChronic As Double = 0.0
                            'Dim lHealthBenchmarkWO As Double = 0.0
                            'Dim lHealthBenchmarkO As Double = 0.0
                            'Dim lMCL As Double = 0.0

                            Dim lStandard As Double = 0.0
                            Dim lStandardName As String = ""

                            'freshwater acute
                            If lAquaBenchmarkAcute > 0.0 Then
                                lStandard = lAquaBenchmarkAcute * 1000
                                lStandardName = "FreshwaterAcute"
                                For lIndex As Integer = 1 To lTimeseriesCsv1.DataSets.Count
                                    Dim lWholeTimeseries As atcTimeseries = lTimeseriesCsv1.DataSets.ItemByKey(lIndex)
                                    'get only subset of data
                                    Dim lSDate(5) As Integer : lSDate(0) = pComplianceDate : lSDate(1) = 1 : lSDate(2) = 1
                                    If pSiteName = "LakeSinclair" Then
                                        'special case -- limited data
                                        lSDate(0) = pComplianceDate : lSDate(1) = 1 : lSDate(2) = 2
                                    End If
                                    Dim lSDateJ As Double = Date2J(lSDate)
                                    Dim lEDate(5) As Integer : lEDate(0) = pComplianceDate + 9 : lEDate(1) = 12 : lEDate(2) = 31
                                    If pSiteName = "LakeSinclair" Then
                                        'special case -- limited data
                                        lEDate(0) = pComplianceDate + 6 : lEDate(1) = 11 : lEDate(2) = 30
                                    End If
                                    Dim lEdatej As Double = Date2J(lEDate)
                                    Dim lTimeseries As atcTimeseries = SubsetByDate(lWholeTimeseries, lSDateJ, lEdatej, Nothing)

                                    Dim lViolationCount As Integer = 0
                                    For lVal As Integer = 1 To lTimeseries.numValues
                                        If lTimeseries.Value(lVal) > lStandard Then
                                            lViolationCount += 1
                                        End If
                                    Next
                                    If lViolationCount > 0 Then
                                        'found some violations of this standard at this location
                                        lDayPercent = (lViolationCount / lTimeseries.numValues) * 100.0
                                        lStr = lRun & " " & lType & " " & lMetal & " violated " & lStandardName & " standard " & Format(lDayPercent, "0.##") & " percent of days in model segment " & lIndex
                                        If (lIndex = pLocation) Then
                                            lStr = lStr & " IRW"
                                        End If
                                        If lIndex < pBenthicSegmentStart Or lIndex = pLocation Then
                                            lWrite.WriteLine(lStr)
                                        End If
                                    End If
                                Next
                            End If

                            'freshwater chronic
                            If lAquaBenchmarkChronic > 0.0 Then
                                lStandard = lAquaBenchmarkChronic * 1000
                                lStandardName = "FreshwaterChronic"
                                For lIndex As Integer = 1 To lTimeseriesCsv1.DataSets.Count
                                    Dim lWholeTimeseries As atcTimeseries = lTimeseriesCsv1.DataSets.ItemByKey(lIndex)
                                    'get only subset of data
                                    Dim lSDate(5) As Integer : lSDate(0) = pComplianceDate : lSDate(1) = 1 : lSDate(2) = 1
                                    If pSiteName = "LakeSinclair" Then
                                        'special case -- limited data
                                        lSDate(0) = pComplianceDate : lSDate(1) = 1 : lSDate(2) = 2
                                    End If
                                    Dim lSDateJ As Double = Date2J(lSDate)
                                    Dim lEDate(5) As Integer : lEDate(0) = pComplianceDate + 9 : lEDate(1) = 12 : lEDate(2) = 31
                                    If pSiteName = "LakeSinclair" Then
                                        'special case -- limited data
                                        lEDate(0) = pComplianceDate + 6 : lEDate(1) = 11 : lEDate(2) = 30
                                    End If
                                    Dim lEdatej As Double = Date2J(lEDate)
                                    Dim lTimeseries As atcTimeseries = SubsetByDate(lWholeTimeseries, lSDateJ, lEdatej, Nothing)

                                    Dim lViolationCount As Integer = 0
                                    For lVal As Integer = 1 To lTimeseries.numValues
                                        If lTimeseries.Value(lVal) > lStandard Then
                                            lViolationCount += 1
                                        End If
                                    Next
                                    If lViolationCount > 0 Then
                                        'found some violations of this standard at this location
                                        lDayPercent = (lViolationCount / lTimeseries.numValues) * 100.0
                                        lStr = lRun & " " & lType & " " & lMetal & " violated " & lStandardName & " standard " & Format(lDayPercent, "0.##") & " percent of days in model segment " & lIndex
                                        If (lIndex = pLocation) Then
                                            lStr = lStr & " IRW"
                                        End If
                                        If lIndex < pBenthicSegmentStart Or lIndex = pLocation Then
                                            lWrite.WriteLine(lStr)
                                        End If
                                    End If
                                Next
                            End If

                            'human health water and organism
                            If lHealthBenchmarkWO > 0.0 Then
                                lStandard = lHealthBenchmarkWO * 1000
                                lStandardName = "HumanHealthWaterAndOrganism"
                                For lIndex As Integer = 1 To lTimeseriesCsv1.DataSets.Count
                                    Dim lWholeTimeseries As atcTimeseries = lTimeseriesCsv1.DataSets.ItemByKey(lIndex)
                                    'get only subset of data
                                    Dim lSDate(5) As Integer : lSDate(0) = pComplianceDate : lSDate(1) = 1 : lSDate(2) = 1
                                    If pSiteName = "LakeSinclair" Then
                                        'special case -- limited data
                                        lSDate(0) = pComplianceDate : lSDate(1) = 1 : lSDate(2) = 2
                                    End If
                                    Dim lSDateJ As Double = Date2J(lSDate)
                                    Dim lEDate(5) As Integer : lEDate(0) = pComplianceDate + 9 : lEDate(1) = 12 : lEDate(2) = 31
                                    If pSiteName = "LakeSinclair" Then
                                        'special case -- limited data
                                        lEDate(0) = pComplianceDate + 6 : lEDate(1) = 11 : lEDate(2) = 30
                                    End If
                                    Dim lEdatej As Double = Date2J(lEDate)
                                    Dim lTimeseries As atcTimeseries = SubsetByDate(lWholeTimeseries, lSDateJ, lEdatej, Nothing)

                                    Dim lViolationCount As Integer = 0
                                    For lVal As Integer = 1 To lTimeseries.numValues
                                        If lTimeseries.Value(lVal) > lStandard Then
                                            lViolationCount += 1
                                        End If
                                    Next
                                    If lViolationCount > 0 Then
                                        'found some violations of this standard at this location
                                        lDayPercent = (lViolationCount / lTimeseries.numValues) * 100.0
                                        lStr = lRun & " " & lType & " " & lMetal & " violated " & lStandardName & " standard " & Format(lDayPercent, "0.##") & " percent of days in model segment " & lIndex
                                        If (lIndex = pLocation) Then
                                            lStr = lStr & " IRW"
                                        End If
                                        If lIndex < pBenthicSegmentStart Or lIndex = pLocation Then
                                            lWrite.WriteLine(lStr)
                                        End If
                                    End If
                                Next
                            End If

                            'human health organism only
                            If lHealthBenchmarkO > 0.0 Then
                                lStandard = lHealthBenchmarkO * 1000
                                lStandardName = "HumanHealthOrganismOnly"
                                For lIndex As Integer = 1 To lTimeseriesCsv1.DataSets.Count
                                    Dim lWholeTimeseries As atcTimeseries = lTimeseriesCsv1.DataSets.ItemByKey(lIndex)
                                    'get only subset of data
                                    Dim lSDate(5) As Integer : lSDate(0) = pComplianceDate : lSDate(1) = 1 : lSDate(2) = 1
                                    If pSiteName = "LakeSinclair" Then
                                        'special case -- limited data
                                        lSDate(0) = pComplianceDate : lSDate(1) = 1 : lSDate(2) = 2
                                    End If
                                    Dim lSDateJ As Double = Date2J(lSDate)
                                    Dim lEDate(5) As Integer : lEDate(0) = pComplianceDate + 9 : lEDate(1) = 12 : lEDate(2) = 31
                                    If pSiteName = "LakeSinclair" Then
                                        'special case -- limited data
                                        lEDate(0) = pComplianceDate + 6 : lEDate(1) = 11 : lEDate(2) = 30
                                    End If
                                    Dim lEdatej As Double = Date2J(lEDate)
                                    Dim lTimeseries As atcTimeseries = SubsetByDate(lWholeTimeseries, lSDateJ, lEdatej, Nothing)

                                    Dim lViolationCount As Integer = 0
                                    For lVal As Integer = 1 To lTimeseries.numValues
                                        If lTimeseries.Value(lVal) > lStandard Then
                                            lViolationCount += 1
                                        End If
                                    Next
                                    If lViolationCount > 0 Then
                                        'found some violations of this standard at this location
                                        lDayPercent = (lViolationCount / lTimeseries.numValues) * 100.0
                                        lStr = lRun & " " & lType & " " & lMetal & " violated " & lStandardName & " standard " & Format(lDayPercent, "0.##") & " percent of days in model segment " & lIndex
                                        If (lIndex = pLocation) Then
                                            lStr = lStr & " IRW"
                                        End If
                                        If lIndex < pBenthicSegmentStart Or lIndex = pLocation Then
                                            lWrite.WriteLine(lStr)
                                        End If
                                    End If
                                Next
                            End If

                            'drinking water mcl
                            If lMCL > 0.0 Then
                                lStandard = lMCL * 1000
                                lStandardName = "DrinkingWaterMCL"
                                For lIndex As Integer = 1 To lTimeseriesCsv1.DataSets.Count
                                    Dim lWholeTimeseries As atcTimeseries = lTimeseriesCsv1.DataSets.ItemByKey(lIndex)
                                    'get only subset of data
                                    Dim lSDate(5) As Integer : lSDate(0) = pComplianceDate : lSDate(1) = 1 : lSDate(2) = 1
                                    If pSiteName = "LakeSinclair" Then
                                        'special case -- limited data
                                        lSDate(0) = pComplianceDate : lSDate(1) = 1 : lSDate(2) = 2
                                    End If
                                    Dim lSDateJ As Double = Date2J(lSDate)
                                    Dim lEDate(5) As Integer : lEDate(0) = pComplianceDate + 9 : lEDate(1) = 12 : lEDate(2) = 31
                                    If pSiteName = "LakeSinclair" Then
                                        'special case -- limited data
                                        lEDate(0) = pComplianceDate + 6 : lEDate(1) = 11 : lEDate(2) = 30
                                    End If
                                    Dim lEdatej As Double = Date2J(lEDate)
                                    Dim lTimeseries As atcTimeseries = SubsetByDate(lWholeTimeseries, lSDateJ, lEdatej, Nothing)

                                    Dim lViolationCount As Integer = 0
                                    For lVal As Integer = 1 To lTimeseries.numValues
                                        If lTimeseries.Value(lVal) > lStandard Then
                                            lViolationCount += 1
                                        End If
                                    Next
                                    If lViolationCount > 0 Then
                                        'found some violations of this standard at this location
                                        lDayPercent = (lViolationCount / lTimeseries.numValues) * 100.0
                                        lStr = lRun & " " & lType & " " & lMetal & " violated " & lStandardName & " standard " & Format(lDayPercent, "0.##") & " percent of days in model segment " & lIndex
                                        If (lIndex = pLocation) Then
                                            lStr = lStr & " IRW"
                                        End If
                                        If lIndex < pBenthicSegmentStart Or lIndex = pLocation Then
                                            lWrite.WriteLine(lStr)
                                        End If
                                    End If
                                Next
                            End If

                            'benthic sediment criterion
                            Dim lBenthicExceedanceFound As Boolean = False
                            If lBenthicSedimentCriterion > 0.0 Then
                                lStandard = lBenthicSedimentCriterion * 1000
                                lStandardName = "BenthicSedimentCriterion"
                                For lIndex As Integer = 1 To lTimeseriesCsv1.DataSets.Count
                                    Dim lWholeTimeseries As atcTimeseries = lTimeseriesCsv1.DataSets.ItemByKey(lIndex)
                                    'get only subset of data
                                    Dim lSDate(5) As Integer : lSDate(0) = pComplianceDate : lSDate(1) = 1 : lSDate(2) = 1
                                    If pSiteName = "LakeSinclair" Then
                                        'special case -- limited data
                                        lSDate(0) = pComplianceDate : lSDate(1) = 1 : lSDate(2) = 2
                                    End If
                                    Dim lSDateJ As Double = Date2J(lSDate)
                                    Dim lEDate(5) As Integer : lEDate(0) = pComplianceDate + 9 : lEDate(1) = 12 : lEDate(2) = 31
                                    If pSiteName = "LakeSinclair" Then
                                        'special case -- limited data
                                        lEDate(0) = pComplianceDate + 6 : lEDate(1) = 11 : lEDate(2) = 30
                                    End If
                                    Dim lEdatej As Double = Date2J(lEDate)
                                    Dim lTimeseries As atcTimeseries = SubsetByDate(lWholeTimeseries, lSDateJ, lEdatej, Nothing)

                                    Dim lViolationCount As Integer = 0
                                    For lVal As Integer = 1 To lTimeseries.numValues
                                        If lTimeseries.Value(lVal) > lStandard Then
                                            lViolationCount += 1
                                        End If
                                    Next
                                    If lViolationCount > 0 Then
                                        'found some violations of this standard at this location
                                        lDayPercent = (lViolationCount / lTimeseries.numValues) * 100.0
                                        lStr = lRun & " " & lType & " " & lMetal & " violated " & lStandardName & " standard " & Format(lDayPercent, "0.##") & " percent of days in model segment " & lIndex
                                        If (lIndex = pLocation) Then
                                            lStr = lStr & " IRW"
                                        End If
                                        lWrite.WriteLine(lStr)
                                        lBenthicExceedanceFound = True
                                    End If
                                Next
                                If lBenthicExceedanceFound = False Then
                                    lWrite.WriteLine("No exceedance of benthic sediment criterion for " & lRun & " " & lType & " " & lMetal)
                                End If
                            End If

                        Else
                            Logger.Msg("Unable to Open " & lCsvFileName)
                        End If
                    Next
                Next
            Next

            lWrite.Close()
        End If
    End Sub

    Private Sub GetBenchmarks(ByVal aSiteName As String, ByVal aUnits As String, ByVal aMetal As String, _
                              ByRef aNationalModelBaseline As Double, _
                              ByRef aNationalModelOptionD As Double, _
                              ByRef aAquaBenchmarkAcute As Double, _
                              ByRef aAquaBenchmarkChronic As Double, _
                              ByRef aHealthBenchmarkWO As Double, _
                              ByRef aHealthBenchmarkO As Double, _
                              ByRef aMCL As Double, _
                              ByRef aLabel As String)
        'Dim lNationalModelBaseline As Double = 0.0
        'Dim lNationalModelOptionD As Double = 0.0
        'Dim lAquaBenchmarkAcute As Double = 0.0
        'Dim lAquaBenchmarkChronic As Double = 0.0
        'Dim lHealthBenchmarkWO As Double = 0.0
        'Dim lHealthBenchmarkO As Double = 0.0
        'Dim lMCL As Double = 0.0

        'site specific data 
        If aSiteName = "Black" Then
            If aUnits = "Total" Then
                If aMetal = "As" Then  'total
                    aNationalModelBaseline = 0.00002846
                    aNationalModelOptionD = 0.00001955
                ElseIf aMetal = "Cd" Then
                    aNationalModelBaseline = 0.0001767
                    aNationalModelOptionD = 0.000009237
                ElseIf aMetal = "Cu" Then
                    aNationalModelBaseline = 0.00004296
                    aNationalModelOptionD = 0.000007874
                ElseIf aMetal = "Pb" Then
                    aNationalModelBaseline = 0.00001285
                    aNationalModelOptionD = 0.000005185
                ElseIf aMetal = "Ni" Then
                    aNationalModelBaseline = 0.001384
                    aNationalModelOptionD = 0.0000225
                ElseIf aMetal = "Se" Then
                    aNationalModelBaseline = 0.001821
                    aNationalModelOptionD = 0.00003949
                ElseIf aMetal = "Tl" Then
                    aNationalModelBaseline = 0.00005049
                    aNationalModelOptionD = 0.00001532
                ElseIf aMetal = "Zn" Then
                    aNationalModelBaseline = 0.002211
                    aNationalModelOptionD = 0.00008888
                End If
            Else
                'for dissolved
                If aMetal = "As" Then
                    aNationalModelBaseline = 0.00002376
                    aNationalModelOptionD = 0.00001633
                ElseIf aMetal = "Cd" Then
                    aNationalModelBaseline = 0.00005939
                    aNationalModelOptionD = 0.000003105
                ElseIf aMetal = "Cu" Then
                    aNationalModelBaseline = 0.00001909
                    aNationalModelOptionD = 0.000003499
                ElseIf aMetal = "Pb" Then
                    aNationalModelBaseline = 0.000000952
                    aNationalModelOptionD = 0.0000003841
                ElseIf aMetal = "Ni" Then
                    aNationalModelBaseline = 0.0009228
                    aNationalModelOptionD = 0.000015
                ElseIf aMetal = "Se" Then
                    aNationalModelBaseline = 0.00112
                    aNationalModelOptionD = 0.0000243
                ElseIf aMetal = "Tl" Then
                    aNationalModelBaseline = 0.00003811
                    aNationalModelOptionD = 0.00001156
                ElseIf aMetal = "Zn" Then
                    aNationalModelBaseline = 0.0006317
                    aNationalModelOptionD = 0.00002539
                End If
            End If
        ElseIf aSiteName = "Etowah" Then
            If aUnits = "Total" Then
                If aMetal = "As" Then
                    aNationalModelBaseline = 0.000007059
                    aNationalModelOptionD = 0.000003615
                ElseIf aMetal = "Cd" Then
                    aNationalModelBaseline = 0.00007042
                    aNationalModelOptionD = 0.000002609
                ElseIf aMetal = "Cu" Then
                    aNationalModelBaseline = 0.00001636
                    aNationalModelOptionD = 0.000002341
                ElseIf aMetal = "Pb" Then
                    aNationalModelBaseline = 0.000005056
                    aNationalModelOptionD = 0.000002101
                ElseIf aMetal = "Ni" Then
                    aNationalModelBaseline = 0.0005549
                    aNationalModelOptionD = 0.000003907
                ElseIf aMetal = "Se" Then
                    aNationalModelBaseline = 0.0007252
                    aNationalModelOptionD = 0.000003546
                ElseIf aMetal = "Tl" Then
                    aNationalModelBaseline = 0.00001956
                    aNationalModelOptionD = 0.000006077
                ElseIf aMetal = "Zn" Then
                    aNationalModelBaseline = 0.0008717
                    aNationalModelOptionD = 0.00001238
                End If
            Else
                'for dissolved
                If aMetal = "As" Then
                    aNationalModelBaseline = 0.000005895
                    aNationalModelOptionD = 0.000003019
                ElseIf aMetal = "Cd" Then
                    aNationalModelBaseline = 0.00002367
                    aNationalModelOptionD = 0.0000008768
                ElseIf aMetal = "Cu" Then
                    aNationalModelBaseline = 0.00000727
                    aNationalModelOptionD = 0.00000104
                ElseIf aMetal = "Pb" Then
                    aNationalModelBaseline = 0.0000003745
                    aNationalModelOptionD = 0.0000001556
                ElseIf aMetal = "Ni" Then
                    aNationalModelBaseline = 0.00037
                    aNationalModelOptionD = 0.000002604
                ElseIf aMetal = "Se" Then
                    aNationalModelBaseline = 0.0004463
                    aNationalModelOptionD = 0.000002182
                ElseIf aMetal = "Tl" Then
                    aNationalModelBaseline = 0.00001476
                    aNationalModelOptionD = 0.000004587
                ElseIf aMetal = "Zn" Then
                    aNationalModelBaseline = 0.0002491
                    aNationalModelOptionD = 0.000003536
                End If
            End If
        ElseIf aSiteName = "LakeSinclair" Then
            If aUnits = "Total" Then
                If aMetal = "As" Then
                    aNationalModelBaseline = 0.00001307
                    aNationalModelOptionD = 0.000003471
                ElseIf aMetal = "Cd" Then
                    aNationalModelBaseline = 0.00006934
                    aNationalModelOptionD = 0.000002505
                ElseIf aMetal = "Cu" Then
                    aNationalModelBaseline = 0.00002902
                    aNationalModelOptionD = 0.000002248
                ElseIf aMetal = "Pb" Then
                    aNationalModelBaseline = 0.00001052
                    aNationalModelOptionD = 0.000002018
                ElseIf aMetal = "Ni" Then
                    aNationalModelBaseline = 0.0005428
                    aNationalModelOptionD = 0.000003752
                ElseIf aMetal = "Se" Then
                    aNationalModelBaseline = 0.0006987
                    aNationalModelOptionD = 0.000003405
                ElseIf aMetal = "Tl" Then
                    aNationalModelBaseline = 0.00002335
                    aNationalModelOptionD = 0.000005836
                ElseIf aMetal = "Zn" Then
                    aNationalModelBaseline = 0.0008857
                    aNationalModelOptionD = 0.00001189
                End If
            Else
                'for dissolved
                If aMetal = "As" Then
                    aNationalModelBaseline = 0.00001091
                    aNationalModelOptionD = 0.000002899
                ElseIf aMetal = "Cd" Then
                    aNationalModelBaseline = 0.00002331
                    aNationalModelOptionD = 0.0000008421
                ElseIf aMetal = "Cu" Then
                    aNationalModelBaseline = 0.0000129
                    aNationalModelOptionD = 0.0000009992
                ElseIf aMetal = "Pb" Then
                    aNationalModelBaseline = 0.0000007792
                    aNationalModelOptionD = 0.0000001495
                ElseIf aMetal = "Ni" Then
                    aNationalModelBaseline = 0.0003619
                    aNationalModelOptionD = 0.000002501
                ElseIf aMetal = "Se" Then
                    aNationalModelBaseline = 0.00043
                    aNationalModelOptionD = 0.000002096
                ElseIf aMetal = "Tl" Then
                    aNationalModelBaseline = 0.00001762
                    aNationalModelOptionD = 0.000004405
                ElseIf aMetal = "Zn" Then
                    aNationalModelBaseline = 0.0002531
                    aNationalModelOptionD = 0.000003396
                End If
            End If
        ElseIf aSiteName = "MississippiMO" Then
            If pLocation = 9 Then
                If aUnits = "Total" Then
                    If aMetal = "As" Then
                        aNationalModelBaseline = 0.000006317
                    ElseIf aMetal = "Cd" Then
                        aNationalModelBaseline = 0.0000009122
                    ElseIf aMetal = "Cu" Then
                        aNationalModelBaseline = 0.000004338
                    ElseIf aMetal = "Pb" Then
                        aNationalModelBaseline = 0.000002844
                    ElseIf aMetal = "Ni" Then
                        aNationalModelBaseline = 0.000003436
                    ElseIf aMetal = "Se" Then
                        aNationalModelBaseline = 0.000002741
                    ElseIf aMetal = "Tl" Then
                        aNationalModelBaseline = 0.000003293
                    ElseIf aMetal = "Zn" Then
                        aNationalModelBaseline = 0.000007985
                    End If
                Else
                    'for dissolved
                    If aMetal = "As" Then
                        aNationalModelBaseline = 0.000004218
                    ElseIf aMetal = "Cd" Then
                        aNationalModelBaseline = 0.0000001526
                    ElseIf aMetal = "Cu" Then
                        aNationalModelBaseline = 0.000001045
                    ElseIf aMetal = "Pb" Then
                        aNationalModelBaseline = 0.00000008751
                    ElseIf aMetal = "Ni" Then
                        aNationalModelBaseline = 0.00000152
                    ElseIf aMetal = "Se" Then
                        aNationalModelBaseline = 0.000001065
                    ElseIf aMetal = "Tl" Then
                        aNationalModelBaseline = 0.00000181
                    ElseIf aMetal = "Zn" Then
                        aNationalModelBaseline = 0.000001094
                    End If
                End If
            End If
        ElseIf aSiteName = "Ohio" Then
            If pLocation = 13 Then
                If aUnits = "Total" Then  'for mansfield plant
                    If aMetal = "As" Then
                        aNationalModelBaseline = 0.0000009613
                        aNationalModelOptionD = 0.000000271
                    ElseIf aMetal = "Cd" Then
                        aNationalModelBaseline = 0.000005398
                        aNationalModelOptionD = 0.0000001955
                    ElseIf aMetal = "Cu" Then
                        aNationalModelBaseline = 0.000001754
                        aNationalModelOptionD = 0.0000001755
                    ElseIf aMetal = "Pb" Then
                        aNationalModelBaseline = 0.0000007768
                        aNationalModelOptionD = 0.0000001575
                    ElseIf aMetal = "Ni" Then
                        aNationalModelBaseline = 0.00004359
                        aNationalModelOptionD = 0.0000002928
                    ElseIf aMetal = "Se" Then
                        aNationalModelBaseline = 0.00005458
                        aNationalModelOptionD = 0.0000002658
                    ElseIf aMetal = "Tl" Then
                        aNationalModelBaseline = 0.000003493
                        aNationalModelOptionD = 0.0000004555
                    ElseIf aMetal = "Zn" Then
                        aNationalModelBaseline = 0.00006697
                        aNationalModelOptionD = 0.0000009278
                    End If
                Else
                    'for dissolved
                    If aMetal = "As" Then
                        aNationalModelBaseline = 0.0000008028
                        aNationalModelOptionD = 0.0000002263
                    ElseIf aMetal = "Cd" Then
                        aNationalModelBaseline = 0.000001814
                        aNationalModelOptionD = 0.00000006573
                    ElseIf aMetal = "Cu" Then
                        aNationalModelBaseline = 0.0000007798
                        aNationalModelOptionD = 0.00000007799
                    ElseIf aMetal = "Pb" Then
                        aNationalModelBaseline = 0.00000005754
                        aNationalModelOptionD = 0.00000001167
                    ElseIf aMetal = "Ni" Then
                        aNationalModelBaseline = 0.00002906
                        aNationalModelOptionD = 0.0000001952
                    ElseIf aMetal = "Se" Then
                        aNationalModelBaseline = 0.00003359
                        aNationalModelOptionD = 0.0000001636
                    ElseIf aMetal = "Tl" Then
                        aNationalModelBaseline = 0.000002636
                        aNationalModelOptionD = 0.0000003438
                    ElseIf aMetal = "Zn" Then
                        aNationalModelBaseline = 0.00001913
                        aNationalModelOptionD = 0.0000002651
                    End If
                End If
            ElseIf pLocation = 9 Then
                If aUnits = "Total" Then  'for sammis plant
                    If aMetal = "As" Then
                        aNationalModelBaseline = 0.000004356
                        aNationalModelOptionD = 0.00000009819
                    ElseIf aMetal = "Cd" Then
                        aNationalModelBaseline = 0.000001233
                        aNationalModelOptionD = 0.00000005797
                    ElseIf aMetal = "Cu" Then
                        aNationalModelBaseline = 0.000005257
                        aNationalModelOptionD = 0.00000005093
                    ElseIf aMetal = "Pb" Then
                        aNationalModelBaseline = 0.00000396
                        aNationalModelOptionD = 0.00000004073
                    ElseIf aMetal = "Ni" Then
                        aNationalModelBaseline = 0.00001974
                        aNationalModelOptionD = 0.0000001098
                    ElseIf aMetal = "Se" Then
                        aNationalModelBaseline = 0.00001338
                        aNationalModelOptionD = 0.0000001501
                    ElseIf aMetal = "Tl" Then
                        aNationalModelBaseline = 0.00002009
                        aNationalModelOptionD = 0.0000001187
                    ElseIf aMetal = "Zn" Then
                        aNationalModelBaseline = 0.00001641
                        aNationalModelOptionD = 0.0000003943
                    End If
                Else
                    'for dissolved
                    If aMetal = "As" Then
                        aNationalModelBaseline = 0.000003638
                        aNationalModelOptionD = 0.000000082
                    ElseIf aMetal = "Cd" Then
                        aNationalModelBaseline = 0.0000004145
                        aNationalModelOptionD = 0.00000001949
                    ElseIf aMetal = "Cu" Then
                        aNationalModelBaseline = 0.000002336
                        aNationalModelOptionD = 0.00000002263
                    ElseIf aMetal = "Pb" Then
                        aNationalModelBaseline = 0.0000002933
                        aNationalModelOptionD = 0.000000003017
                    ElseIf aMetal = "Ni" Then
                        aNationalModelBaseline = 0.00001316
                        aNationalModelOptionD = 0.00000007317
                    ElseIf aMetal = "Se" Then
                        aNationalModelBaseline = 0.000008237
                        aNationalModelOptionD = 0.00000009234
                    ElseIf aMetal = "Tl" Then
                        aNationalModelBaseline = 0.00001516
                        aNationalModelOptionD = 0.00000008955
                    ElseIf aMetal = "Zn" Then
                        aNationalModelBaseline = 0.000004687
                        aNationalModelOptionD = 0.0000001126
                    End If
                End If
            End If
        ElseIf aSiteName = "White" Then
            If aUnits = "Total" Then
                If aMetal = "As" Then
                    aNationalModelBaseline = 0.004619
                    aNationalModelOptionD = 0.0002511
                ElseIf aMetal = "Cd" Then
                    aNationalModelBaseline = 0.002405
                    aNationalModelOptionD = 0.0001812
                ElseIf aMetal = "Cu" Then
                    aNationalModelBaseline = 0.01546
                    aNationalModelOptionD = 0.0001626
                ElseIf aMetal = "Pb" Then
                    aNationalModelBaseline = 0.007078
                    aNationalModelOptionD = 0.000146
                ElseIf aMetal = "Ni" Then
                    aNationalModelBaseline = 0.01364
                    aNationalModelOptionD = 0.0002714
                ElseIf aMetal = "Se" Then
                    aNationalModelBaseline = 0.04041
                    aNationalModelOptionD = 0.0002463
                ElseIf aMetal = "Tl" Then
                    aNationalModelBaseline = 0.008447
                    aNationalModelOptionD = 0.0004222
                ElseIf aMetal = "Zn" Then
                    aNationalModelBaseline = 0.01425
                    aNationalModelOptionD = 0.0008598
                End If
            Else
                'for dissolved
                If aMetal = "As" Then
                    aNationalModelBaseline = 0.003857
                    aNationalModelOptionD = 0.0002097
                ElseIf aMetal = "Cd" Then
                    aNationalModelBaseline = 0.0008083
                    aNationalModelOptionD = 0.00006091
                ElseIf aMetal = "Cu" Then
                    aNationalModelBaseline = 0.006871
                    aNationalModelOptionD = 0.00007228
                ElseIf aMetal = "Pb" Then
                    aNationalModelBaseline = 0.0005243
                    aNationalModelOptionD = 0.00001081
                ElseIf aMetal = "Ni" Then
                    aNationalModelBaseline = 0.009092
                    aNationalModelOptionD = 0.0001809
                ElseIf aMetal = "Se" Then
                    aNationalModelBaseline = 0.02487
                    aNationalModelOptionD = 0.0001516
                ElseIf aMetal = "Tl" Then
                    aNationalModelBaseline = 0.006375
                    aNationalModelOptionD = 0.0003186
                ElseIf aMetal = "Zn" Then
                    aNationalModelBaseline = 0.004071
                    aNationalModelOptionD = 0.0002457
                End If
            End If
        End If

        If aUnits = "Total" Then
            If aMetal = "As" Then
                aLabel = "Total Arsenic (ug/L)"
                aAquaBenchmarkAcute = 0.0
                aAquaBenchmarkChronic = 0.0
                aHealthBenchmarkWO = 0.000018
                aHealthBenchmarkO = 0.00014
                aMCL = 0.01
            ElseIf aMetal = "Cd" Then
                aLabel = "Total Cadmium (ug/L)"
                aAquaBenchmarkAcute = 0.0
                aAquaBenchmarkChronic = 0.0
                aHealthBenchmarkWO = 0.0
                aHealthBenchmarkO = 0.0
                aMCL = 0.005
            ElseIf aMetal = "Cu" Then
                aLabel = "Total Copper (ug/L)"
                aAquaBenchmarkAcute = 0.0
                aAquaBenchmarkChronic = 0.0
                aHealthBenchmarkWO = 1.3
                aHealthBenchmarkO = 0.0
                aMCL = 1.3
            ElseIf aMetal = "Ni" Then
                aLabel = "Total Nickel (ug/L)"
                aAquaBenchmarkAcute = 0.0
                aAquaBenchmarkChronic = 0.0
                aHealthBenchmarkWO = 0.61
                aHealthBenchmarkO = 4.6
                aMCL = 0.0
            ElseIf aMetal = "Pb" Then
                aLabel = "Total Lead (ug/L)"
                aAquaBenchmarkAcute = 0.0
                aAquaBenchmarkChronic = 0.0
                aHealthBenchmarkWO = 0.0
                aHealthBenchmarkO = 0.0
                aMCL = 0.015
            ElseIf aMetal = "Se" Then
                aLabel = "Total Selenium (ug/L)"
                aAquaBenchmarkAcute = 0.0
                aAquaBenchmarkChronic = 0.005
                aHealthBenchmarkWO = 0.17
                aHealthBenchmarkO = 4.2
                aMCL = 0.05
            ElseIf aMetal = "Tl" Then
                aLabel = "Total Thallium (ug/L)"
                aAquaBenchmarkAcute = 0.0
                aAquaBenchmarkChronic = 0.0
                aHealthBenchmarkWO = 0.00024
                aHealthBenchmarkO = 0.00047
                aMCL = 0.002
            ElseIf aMetal = "Zn" Then
                aLabel = "Total Zinc (ug/L)"
                aAquaBenchmarkAcute = 0.0
                aAquaBenchmarkChronic = 0.0
                aHealthBenchmarkWO = 7.4
                aHealthBenchmarkO = 26
                aMCL = 0.0
            End If
        Else
            'dissolved
            If aMetal = "As" Then
                aLabel = "Dissolved Arsenic (ug/L)"
                aAquaBenchmarkAcute = 0.34
                aAquaBenchmarkChronic = 0.15
                aHealthBenchmarkWO = 0.0
                aHealthBenchmarkO = 0.0
                aMCL = 0.0
            ElseIf aMetal = "Cd" Then
                aLabel = "Dissolved Cadmium (ug/L)"
                aAquaBenchmarkAcute = 0.002
                aAquaBenchmarkChronic = 0.00025
                aHealthBenchmarkWO = 0.0
                aHealthBenchmarkO = 0.0
                aMCL = 0.0
            ElseIf aMetal = "Cu" Then
                aLabel = "Dissolved Copper (ug/L)"
                aAquaBenchmarkAcute = 0.013
                aAquaBenchmarkChronic = 0.009
                aHealthBenchmarkWO = 0.0
                aHealthBenchmarkO = 0.0
                aMCL = 0.0
            ElseIf aMetal = "Ni" Then
                aLabel = "Dissolved Nickel (ug/L)"
                aAquaBenchmarkAcute = 0.47
                aAquaBenchmarkChronic = 0.052
                aHealthBenchmarkWO = 0.0
                aHealthBenchmarkO = 0.0
                aMCL = 0.0
            ElseIf aMetal = "Pb" Then
                aLabel = "Dissolved Lead (ug/L)"
                aAquaBenchmarkAcute = 0.065
                aAquaBenchmarkChronic = 0.0025
                aHealthBenchmarkWO = 0.0
                aHealthBenchmarkO = 0.0
                aMCL = 0.0
            ElseIf aMetal = "Se" Then
                aLabel = "Dissolved Selenium (ug/L)"
                aAquaBenchmarkAcute = 0.0
                aAquaBenchmarkChronic = 0.0
                aHealthBenchmarkWO = 0.0
                aHealthBenchmarkO = 0.0
                aMCL = 0.0
            ElseIf aMetal = "Tl" Then
                aLabel = "Dissolved Thallium (ug/L)"
                aAquaBenchmarkAcute = 0.0
                aAquaBenchmarkChronic = 0.0
                aHealthBenchmarkWO = 0.0
                aHealthBenchmarkO = 0.0
                aMCL = 0.0
            ElseIf aMetal = "Zn" Then
                aLabel = "Dissolved Zinc (ug/L)"
                aAquaBenchmarkAcute = 0.12
                aAquaBenchmarkChronic = 0.12
                aHealthBenchmarkWO = 0.0
                aHealthBenchmarkO = 0.0
                aMCL = 0.0
            End If
        End If
    End Sub

    Private Sub ComputeAnnualAverageAcrossAllSegments()

        Dim lCsvName As String = ""

        Dim pRunNames As New atcCollection
        pRunNames.Add("AsBaselineHistoric")
        pRunNames.Add("CdBaselineHistoric")
        pRunNames.Add("CuBaselineHistoric")
        pRunNames.Add("NiBaselineHistoric")
        pRunNames.Add("PbBaselineHistoric")
        pRunNames.Add("SeBaselineHistoric")
        pRunNames.Add("TlBaselineHistoric")
        pRunNames.Add("ZnBaselineHistoric")

        lCsvName = "Total_Concentration.csv"

        Dim lTimeseriesGroup As New atcTimeseriesGroup
        Dim lTimeseriesCsv As New atcTimeseriesCSV.atcTimeseriesCSV

        ChDriveDir(pTestPath & "\OutputPlots")

        'Dim lSDate(5) As Integer : lSDate(0) = pStartYearForBaselinePlot : lSDate(1) = 1 : lSDate(2) = 1
        'Dim lSDateJ As Double = Date2J(lSDate)
        'Dim lEDate(5) As Integer : lEDate(0) = 2020 : lEDate(1) = 12 : lEDate(2) = 31
        'Dim lEdatej As Double = Date2J(lEDate)

        'do baseline historic 
        For Each lRunName As String In pRunNames

            Dim lAverageAnnualFileName As String = lRunName & "AnnualAverageForAllSegments.txt"

            If Not FileExists(lAverageAnnualFileName) Then

                Dim lCsvFileName As String = pTestPath & "\" & lRunName & "\" & lCsvName
                If lTimeseriesCsv.Open(lCsvFileName) Then

                    For Each lTimSerX As atcTimeseries In lTimeseriesCsv.DataSets
                        lTimeseriesGroup.Add(lTimSerX)
                    Next

                    'while we've got it open, also output average total conc for each year
                    Dim lHeader As String = lRunName & " Annual Average from " & lCsvName & " over all segments"

                    Dim lTimSerAverage As atcTimeseries = lTimeseriesCsv.DataSets.ItemByKey(pLocation)

                    Dim lDaySum As Double = 0.0
                    For lIndex As Integer = 1 To lTimSerAverage.numValues
                        lDaySum = 0.0
                        For Each lTimSer As atcTimeseries In lTimeseriesGroup
                            If lTimSer.Values(1) > -1 Then
                                lDaySum = lDaySum + lTimSer.Value(lIndex)
                            End If
                        Next
                        lTimSerAverage.Value(lIndex) = lDaySum / (lTimeseriesGroup.Count - 1)
                    Next

                    WriteAverageAnnualFile(lAverageAnnualFileName, lTimSerAverage, pStartYearForSimulation, lHeader)

                    lTimeseriesCsv.Clear()
                    lTimeseriesGroup.Clear()
                Else
                    Logger.Msg("Unable to Open " & lCsvFileName)
                End If
            End If
        Next

        Dim pMetalNames As New atcCollection
        pMetalNames.Add("As")
        pMetalNames.Add("Cd")
        pMetalNames.Add("Cu")
        pMetalNames.Add("Ni")
        pMetalNames.Add("Pb")
        pMetalNames.Add("Se")
        pMetalNames.Add("Tl")
        pMetalNames.Add("Zn")

        'lSDate(0) = pComplianceDate : lSDate(1) = 1 : lSDate(2) = 1
        'lSDateJ = Date2J(lSDate)
        'lEDate(0) = pComplianceDate + 9 : lEDate(1) = 12 : lEDate(2) = 31
        'lEdatej = Date2J(lEDate)

        Dim lTimeseriesOptionsGroup As New atcTimeseriesGroup
        Dim lTimeseriesCsvOption As New atcTimeseriesCSV.atcTimeseriesCSV

        'do post-compliance options
        For Each lMetalName As String In pMetalNames

            Dim lAverageAnnualFileName As String = lMetalName & "Baseline" & "AnnualAverageForAllSegments.txt"

            If Not FileExists(lAverageAnnualFileName) Then
                Dim lOptionName As String = ""

                lOptionName = lMetalName & "Baseline"
                If FileExists(pTestPath & "\" & lOptionName & "\", True) Then
                    'baseline folder exists

                    Dim lCsvFileName As String = pTestPath & "\" & lOptionName & "\" & lCsvName
                    If lTimeseriesCsvOption.Open(lCsvFileName) Then

                        For Each lTimSerX As atcTimeseries In lTimeseriesCsvOption.DataSets
                            lTimeseriesGroup.Add(lTimSerX)
                        Next

                        'while we've got it open, also output average total conc for each year
                        Dim lHeader As String = lOptionName & " Annual Average from " & lCsvName & " over all segments"

                        Dim lTimSerAverage As atcTimeseries = lTimeseriesCsvOption.DataSets.ItemByKey(pLocation)

                        Dim lDaySum As Double = 0.0
                        For lIndex As Integer = 1 To lTimSerAverage.numValues
                            lDaySum = 0.0
                            For Each lTimSer As atcTimeseries In lTimeseriesGroup
                                If lTimSer.Values(1) > -1 Then
                                    lDaySum = lDaySum + lTimSer.Value(lIndex)
                                End If
                            Next
                            lTimSerAverage.Value(lIndex) = lDaySum / (lTimeseriesGroup.Count - 1)
                        Next
                        WriteAverageAnnualFile(lAverageAnnualFileName, lTimSerAverage, pComplianceDate, lHeader)

                        lTimeseriesGroup.Clear()
                        lTimeseriesCsvOption.Clear()
                    Else
                        Logger.Msg("Unable to Open " & lCsvFileName)
                    End If
                End If
            End If

            lAverageAnnualFileName = lMetalName & "OptionA" & "AnnualAverageForAllSegments.txt"
            If Not FileExists(lAverageAnnualFileName) Then
                Dim lOptionName As String = ""

                lOptionName = lMetalName & "OptionA"
                If FileExists(pTestPath & "\" & lOptionName & "\", True) Then
                    'folder exists

                    Dim lCsvFileName As String = pTestPath & "\" & lOptionName & "\" & lCsvName
                    If lTimeseriesCsvOption.Open(lCsvFileName) Then
                        For Each lTimSerX As atcTimeseries In lTimeseriesCsvOption.DataSets
                            lTimeseriesGroup.Add(lTimSerX)
                        Next

                        'while we've got it open, also output average total conc for each year
                        Dim lHeader As String = lOptionName & " Annual Average from " & lCsvName & " over all segments"
                        Dim lTimSerAverage As atcTimeseries = lTimeseriesCsvOption.DataSets.ItemByKey(pLocation)
                        Dim lDaySum As Double = 0.0
                        For lIndex As Integer = 1 To lTimSerAverage.numValues
                            lDaySum = 0.0
                            For Each lTimSer As atcTimeseries In lTimeseriesGroup
                                If lTimSer.Values(1) > -1 Then
                                    lDaySum = lDaySum + lTimSer.Value(lIndex)
                                End If
                            Next
                            lTimSerAverage.Value(lIndex) = lDaySum / (lTimeseriesGroup.Count - 1)
                        Next
                        WriteAverageAnnualFile(lAverageAnnualFileName, lTimSerAverage, pComplianceDate, lHeader)

                        lTimeseriesGroup.Clear()
                        lTimeseriesCsvOption.Clear()
                    Else
                        Logger.Msg("Unable to Open " & lCsvFileName)
                    End If
                End If
            End If

            lAverageAnnualFileName = lMetalName & "OptionB" & "AnnualAverageForAllSegments.txt"
            If Not FileExists(lAverageAnnualFileName) Then
                Dim lOptionName As String = ""

                lOptionName = lMetalName & "OptionB"
                If FileExists(pTestPath & "\" & lOptionName & "\", True) Then
                    'folder exists

                    Dim lCsvFileName As String = pTestPath & "\" & lOptionName & "\" & lCsvName
                    If lTimeseriesCsvOption.Open(lCsvFileName) Then
                        For Each lTimSerX As atcTimeseries In lTimeseriesCsvOption.DataSets
                            lTimeseriesGroup.Add(lTimSerX)
                        Next

                        'while we've got it open, also output average total conc for each year
                        Dim lHeader As String = lOptionName & " Annual Average from " & lCsvName & " over all segments"
                        Dim lTimSerAverage As atcTimeseries = lTimeseriesCsvOption.DataSets.ItemByKey(pLocation)
                        Dim lDaySum As Double = 0.0
                        For lIndex As Integer = 1 To lTimSerAverage.numValues
                            lDaySum = 0.0
                            For Each lTimSer As atcTimeseries In lTimeseriesGroup
                                If lTimSer.Values(1) > -1 Then
                                    lDaySum = lDaySum + lTimSer.Value(lIndex)
                                End If
                            Next
                            lTimSerAverage.Value(lIndex) = lDaySum / (lTimeseriesGroup.Count - 1)
                        Next
                        WriteAverageAnnualFile(lAverageAnnualFileName, lTimSerAverage, pComplianceDate, lHeader)

                        lTimeseriesGroup.Clear()
                        lTimeseriesCsvOption.Clear()
                    Else
                        Logger.Msg("Unable to Open " & lCsvFileName)
                    End If
                End If
            End If

            lAverageAnnualFileName = lMetalName & "OptionC" & "AnnualAverageForAllSegments.txt"
            If Not FileExists(lAverageAnnualFileName) Then
                Dim lOptionName As String = ""

                lOptionName = lMetalName & "OptionC"
                If FileExists(pTestPath & "\" & lOptionName & "\", True) Then
                    'folder exists

                    Dim lCsvFileName As String = pTestPath & "\" & lOptionName & "\" & lCsvName
                    If lTimeseriesCsvOption.Open(lCsvFileName) Then
                        For Each lTimSerX As atcTimeseries In lTimeseriesCsvOption.DataSets
                            lTimeseriesGroup.Add(lTimSerX)
                        Next

                        'while we've got it open, also output average total conc for each year
                        Dim lHeader As String = lOptionName & " Annual Average from " & lCsvName & " over all segments"
                        Dim lTimSerAverage As atcTimeseries = lTimeseriesCsvOption.DataSets.ItemByKey(pLocation)
                        Dim lDaySum As Double = 0.0
                        For lIndex As Integer = 1 To lTimSerAverage.numValues
                            lDaySum = 0.0
                            For Each lTimSer As atcTimeseries In lTimeseriesGroup
                                If lTimSer.Values(1) > -1 Then
                                    lDaySum = lDaySum + lTimSer.Value(lIndex)
                                End If
                            Next
                            lTimSerAverage.Value(lIndex) = lDaySum / (lTimeseriesGroup.Count - 1)
                        Next
                        WriteAverageAnnualFile(lAverageAnnualFileName, lTimSerAverage, pComplianceDate, lHeader)

                        lTimeseriesGroup.Clear()
                        lTimeseriesCsvOption.Clear()
                    Else
                        Logger.Msg("Unable to Open " & lCsvFileName)
                    End If
                End If
            End If

            lAverageAnnualFileName = lMetalName & "OptionD" & "AnnualAverageForAllSegments.txt"
            If Not FileExists(lAverageAnnualFileName) Then
                Dim lOptionName As String = ""

                lOptionName = lMetalName & "OptionD"
                If FileExists(pTestPath & "\" & lOptionName & "\", True) Then
                    'folder exists

                    Dim lCsvFileName As String = pTestPath & "\" & lOptionName & "\" & lCsvName
                    If lTimeseriesCsvOption.Open(lCsvFileName) Then
                        For Each lTimSerX As atcTimeseries In lTimeseriesCsvOption.DataSets
                            lTimeseriesGroup.Add(lTimSerX)
                        Next
                        'while we've got it open, also output average total conc for each year
                        Dim lHeader As String = lOptionName & " Annual Average from " & lCsvName & " over all segments"
                        Dim lTimSerAverage As atcTimeseries = lTimeseriesCsvOption.DataSets.ItemByKey(pLocation)
                        Dim lDaySum As Double = 0.0
                        For lIndex As Integer = 1 To lTimSerAverage.numValues
                            lDaySum = 0.0
                            For Each lTimSer As atcTimeseries In lTimeseriesGroup
                                If lTimSer.Values(1) > -1 Then
                                    lDaySum = lDaySum + lTimSer.Value(lIndex)
                                End If
                            Next
                            lTimSerAverage.Value(lIndex) = lDaySum / (lTimeseriesGroup.Count - 1)
                        Next
                        WriteAverageAnnualFile(lAverageAnnualFileName, lTimSerAverage, pComplianceDate, lHeader)

                        lTimeseriesGroup.Clear()
                        lTimeseriesCsvOption.Clear()
                    Else
                        Logger.Msg("Unable to Open " & lCsvFileName)
                    End If
                End If
            End If

        Next

    End Sub

    Private Sub DoERGCompositeGraphAveraged()

        Dim lCsvName As String = ""

        Dim pMetalNames As New atcCollection
        pMetalNames.Add("As")
        pMetalNames.Add("Cd")
        pMetalNames.Add("Cu")
        pMetalNames.Add("Ni")
        pMetalNames.Add("Pb")
        pMetalNames.Add("Se")
        pMetalNames.Add("Tl")
        pMetalNames.Add("Zn")

        Dim pTypes As New atcCollection
        pTypes.Add("Total")
        pTypes.Add("Dissolved")

        For Each lType As String In pTypes
            lCsvName = lType & "_Concentration.csv"

            Dim lTimeseriesGroup As New atcTimeseriesGroup

            ChDriveDir(pTestPath & "\OutputPlots")

            For Each lMetal As String In pMetalNames

                Dim lOutFileName As String = pSiteName & lType & lMetal.Substring(0, 2) & "Composite" & "AveragedNew.png"
                If pSiteName = "MississippiMO" Or pSiteName = "Ohio" Then
                    lOutFileName = pSiteName & lType & lMetal.Substring(0, 2) & "CompositeSeg" & pLocation.ToString & "AveragedNew.png"
                End If

                If Not FileExists(lOutFileName) Then
                    'get baseline historic data
                    Dim lTimeseriesCsv1 As New atcTimeseriesCSV.atcTimeseriesCSV
                    Dim lRunName As String = lMetal & "BaselineHistoric"
                    Dim lCsvFileName As String = pTestPath & "\" & lRunName & "\" & lCsvName

                    If Not FileExists(lCsvFileName & ".start") Then
                        FileCopy(pTestPath & "\" & lRunName & "\" & "Total_Concentration.csv.start", lCsvFileName & ".start")
                    End If

                    If lTimeseriesCsv1.Open(lCsvFileName) Then

                        'calc average value across all cells
                        Dim lTimeseriesGroupToAverage As New atcTimeseriesGroup
                        For Each lTimSerX As atcTimeseries In lTimeseriesCsv1.DataSets
                            lTimeseriesGroupToAverage.Add(lTimSerX)
                        Next
                        Dim lTimSer1 As atcTimeseries = lTimeseriesCsv1.DataSets.ItemByKey(pLocation)
                        Dim lDaySum As Double = 0.0
                        For lIndex As Integer = 1 To lTimSer1.numValues
                            lDaySum = 0.0
                            For Each lTimSer As atcTimeseries In lTimeseriesGroupToAverage
                                If lTimSer.Values(1) > -1 Then
                                    lDaySum = lDaySum + lTimSer.Value(lIndex)
                                End If
                            Next
                            lTimSer1.Value(lIndex) = lDaySum / (lTimeseriesGroupToAverage.Count - 1)
                        Next
                        lTimeseriesGroupToAverage.Clear()

                        lTimSer1.Attributes.SetValue("YAxis", "Left")

                        Dim lSDate(5) As Integer : lSDate(0) = pComplianceDate - 10 : lSDate(1) = 1 : lSDate(2) = 1
                        If pSiteName = "LakeSinclair" Then
                            'special case -- limited data
                            lSDate(0) = pComplianceDate - 7 : lSDate(1) = 2 : lSDate(2) = 2
                        End If
                        Dim lSDateJ As Double = Date2J(lSDate)
                        Dim lEDate(5) As Integer : lEDate(0) = pComplianceDate - 1 : lEDate(1) = 12 : lEDate(2) = 31
                        Dim lEdatej As Double = Date2J(lEDate)

                        lTimeseriesGroup.Add(SubsetByDate(lTimSer1, _
                                                    lSDateJ, _
                                                    lEdatej, Nothing))
                        lTimeseriesCsv1.Clear()
                    Else
                        Logger.Msg("Unable to Open " & lCsvFileName)
                    End If

                    'get option d post-compliance data
                    Dim lTimeseriesCsv3 As New atcTimeseriesCSV.atcTimeseriesCSV
                    lRunName = lMetal & "OptionD"
                    lCsvFileName = pTestPath & "\" & lRunName & "\" & lCsvName
                    If Not FileExists(lCsvFileName) Then
                        lRunName = lMetal & "OptionC"
                        lCsvFileName = pTestPath & "\" & lRunName & "\" & lCsvName
                    End If

                    If Not FileExists(lCsvFileName & ".start") Then
                        FileCopy(pTestPath & "\" & lRunName & "\" & "Total_Concentration.csv.start", lCsvFileName & ".start")
                    End If

                    If lTimeseriesCsv3.Open(lCsvFileName) Then

                        'calc average value across all cells
                        Dim lTimeseriesGroupToAverage As New atcTimeseriesGroup
                        For Each lTimSerX As atcTimeseries In lTimeseriesCsv3.DataSets
                            lTimeseriesGroupToAverage.Add(lTimSerX)
                        Next
                        Dim lTimSerD As atcTimeseries = lTimeseriesCsv3.DataSets.ItemByKey(pLocation)
                        Dim lDaySum As Double = 0.0
                        For lIndex As Integer = 1 To lTimSerD.numValues
                            lDaySum = 0.0
                            For Each lTimSer As atcTimeseries In lTimeseriesGroupToAverage
                                If lTimSer.Values(1) > -1 Then
                                    lDaySum = lDaySum + lTimSer.Value(lIndex)
                                End If
                            Next
                            lTimSerD.Value(lIndex) = lDaySum / (lTimeseriesGroupToAverage.Count - 1)
                        Next
                        lTimeseriesGroupToAverage.Clear()

                        lTimSerD.Attributes.SetValue("YAxis", "Left")

                        Dim lSDate(5) As Integer : lSDate(0) = pComplianceDate : lSDate(1) = 1 : lSDate(2) = 1
                        If pSiteName = "LakeSinclair" Then
                            'special case -- limited data
                            lSDate(0) = pComplianceDate : lSDate(1) = 1 : lSDate(2) = 2
                        End If
                        Dim lSDateJ As Double = Date2J(lSDate)
                        Dim lEDate(5) As Integer : lEDate(0) = pComplianceDate + 9 : lEDate(1) = 12 : lEDate(2) = 31
                        If pSiteName = "LakeSinclair" Then
                            'special case -- limited data
                            lEDate(0) = pComplianceDate + 6 : lEDate(1) = 11 : lEDate(2) = 30
                        End If
                        Dim lEdatej As Double = Date2J(lEDate)

                        lTimeseriesGroup.Add(SubsetByDate(lTimSerD, _
                                                    lSDateJ, _
                                                    lEdatej, Nothing))
                        lTimeseriesCsv3.Clear()
                    Else
                        Logger.Msg("Unable to Open " & lCsvFileName)
                    End If

                    'get baseline post-compliance data
                    Dim lTimeseriesCsv2 As New atcTimeseriesCSV.atcTimeseriesCSV
                    lRunName = lMetal & "Baseline"
                    lCsvFileName = pTestPath & "\" & lRunName & "\" & lCsvName

                    If Not FileExists(lCsvFileName & ".start") Then
                        FileCopy(pTestPath & "\" & lRunName & "\" & "Total_Concentration.csv.start", lCsvFileName & ".start")
                    End If

                    If lTimeseriesCsv2.Open(lCsvFileName) Then

                        'calc average value across all cells
                        Dim lTimeseriesGroupToAverage As New atcTimeseriesGroup
                        For Each lTimSerX As atcTimeseries In lTimeseriesCsv2.DataSets
                            lTimeseriesGroupToAverage.Add(lTimSerX)
                        Next
                        Dim lTimSer2 As atcTimeseries = lTimeseriesCsv2.DataSets.ItemByKey(pLocation)
                        Dim lDaySum As Double = 0.0
                        For lIndex As Integer = 1 To lTimSer2.numValues
                            lDaySum = 0.0
                            For Each lTimSer As atcTimeseries In lTimeseriesGroupToAverage
                                If lTimSer.Values(1) > -1 Then
                                    lDaySum = lDaySum + lTimSer.Value(lIndex)
                                End If
                            Next
                            lTimSer2.Value(lIndex) = lDaySum / (lTimeseriesGroupToAverage.Count - 1)
                        Next
                        lTimeseriesGroupToAverage.Clear()

                        lTimSer2.Attributes.SetValue("YAxis", "Left")

                        Dim lSDate(5) As Integer : lSDate(0) = pComplianceDate : lSDate(1) = 1 : lSDate(2) = 1
                        If pSiteName = "LakeSinclair" Then
                            'special case -- limited data
                            lSDate(0) = pComplianceDate : lSDate(1) = 1 : lSDate(2) = 2
                        End If
                        Dim lSDateJ As Double = Date2J(lSDate)
                        Dim lEDate(5) As Integer : lEDate(0) = pComplianceDate + 9 : lEDate(1) = 12 : lEDate(2) = 31
                        If pSiteName = "LakeSinclair" Then
                            'special case -- limited data
                            lEDate(0) = pComplianceDate + 6 : lEDate(1) = 11 : lEDate(2) = 30
                        End If
                        Dim lEdatej As Double = Date2J(lEDate)

                        lTimeseriesGroup.Add(SubsetByDate(lTimSer2, _
                                                    lSDateJ, _
                                                    lEdatej, Nothing))
                        lTimeseriesCsv2.Clear()
                    Else
                        Logger.Msg("Unable to Open " & lCsvFileName)
                    End If

                    GraphTimeseriesOptions(lTimeseriesGroup, lMetal, lOutFileName, lType, pSiteName)

                    lTimeseriesGroup.Clear()
                End If

            Next
        Next

    End Sub

    Private Sub OutputInitialConditions()

        Dim lCsvName As String = ""

        Dim pRunNames As New atcCollection
        pRunNames.Add("AsBaselineHistoric")
        pRunNames.Add("CdBaselineHistoric")
        pRunNames.Add("CuBaselineHistoric")
        pRunNames.Add("NiBaselineHistoric")
        pRunNames.Add("PbBaselineHistoric")
        pRunNames.Add("SeBaselineHistoric")
        pRunNames.Add("TlBaselineHistoric")
        pRunNames.Add("ZnBaselineHistoric")

        lCsvName = "Total_Concentration.csv"

        Dim lTimeseriesGroup As New atcTimeseriesGroup
        Dim lTimeseriesCsv As New atcTimeseriesCSV.atcTimeseriesCSV

        ChDriveDir(pTestPath & "\InitialConditions")
        Dim lInitialConditionsFileName As String = ""

        'Dim lSDate(5) As Integer : lSDate(0) = pStartYearForBaselinePlot : lSDate(1) = 1 : lSDate(2) = 1
        'Dim lSDateJ As Double = Date2J(lSDate)
        'Dim lEDate(5) As Integer : lEDate(0) = 2020 : lEDate(1) = 12 : lEDate(2) = 31
        'Dim lEdatej As Double = Date2J(lEDate)

        'do baseline historic 
        For Each lRunName As String In pRunNames

            lInitialConditionsFileName = lRunName.Substring(0, 2) & "InitialConditions.txt"

            If Not FileExists(lInitialConditionsFileName) Then

                Dim lCsvFileName As String = pTestPath & "\" & lRunName & "\" & lCsvName
                If lTimeseriesCsv.Open(lCsvFileName) Then

                    For Each lTimSerX As atcTimeseries In lTimeseriesCsv.DataSets
                        lTimeseriesGroup.Add(lTimSerX)
                    Next

                    Dim lWrite As New IO.StreamWriter(lInitialConditionsFileName, False)
                    Dim lStr As String = ""
                    Dim lCnt As Integer = 0

                    For Each lTimSer As atcTimeseries In lTimeseriesGroup
                        If lTimSer.Values(1) > -1 Then
                            lCnt += 1
                            '   1  0.000000  1.00000
                            lStr = "   " & lCnt.ToString & "  " & lTimSer.Value(lTimSer.numValues).ToString & "  1.00000"
                            lWrite.WriteLine(lStr)
                        End If
                    Next

                    lWrite.Close()

                    lTimeseriesCsv.Clear()
                    lTimeseriesGroup.Clear()
                Else
                    Logger.Msg("Unable to Open " & lCsvFileName)
                End If
            End If
        Next

        'Organic_Solids.csv
        lInitialConditionsFileName = "Organic_SolidsInitialConditions.txt"
        lCsvName = "Organic_Solids.csv"
        If Not FileExists(lInitialConditionsFileName) Then

            Dim lCsvFileName As String = pTestPath & "\" & "AsBaselineHistoric" & "\" & lCsvName
            If lTimeseriesCsv.Open(lCsvFileName) Then

                For Each lTimSerX As atcTimeseries In lTimeseriesCsv.DataSets
                    lTimeseriesGroup.Add(lTimSerX)
                Next

                Dim lWrite As New IO.StreamWriter(lInitialConditionsFileName, False)
                Dim lStr As String = ""
                Dim lCnt As Integer = 0

                For Each lTimSer As atcTimeseries In lTimeseriesGroup
                    If lTimSer.Values(1) > -1 Then
                        lCnt += 1
                        '   1  0.000000  1.00000
                        lStr = "   " & lCnt.ToString & "  " & lTimSer.Value(lTimSer.numValues).ToString & "  1.00000"
                        lWrite.WriteLine(lStr)
                    End If
                Next

                lWrite.Close()

                lTimeseriesCsv.Clear()
                lTimeseriesGroup.Clear()
            Else
                Logger.Msg("Unable to Open " & lCsvFileName)
            End If
        End If
        'Sand.csv
        lInitialConditionsFileName = "SandInitialConditions.txt"
        lCsvName = "Sand.csv"
        If Not FileExists(lInitialConditionsFileName) Then

            Dim lCsvFileName As String = pTestPath & "\" & "AsBaselineHistoric" & "\" & lCsvName
            If lTimeseriesCsv.Open(lCsvFileName) Then

                For Each lTimSerX As atcTimeseries In lTimeseriesCsv.DataSets
                    lTimeseriesGroup.Add(lTimSerX)
                Next

                Dim lWrite As New IO.StreamWriter(lInitialConditionsFileName, False)
                Dim lStr As String = ""
                Dim lCnt As Integer = 0

                For Each lTimSer As atcTimeseries In lTimeseriesGroup
                    If lTimSer.Values(1) > -1 Then
                        lCnt += 1
                        '   1  0.000000  1.00000
                        lStr = "   " & lCnt.ToString & "  " & lTimSer.Value(lTimSer.numValues).ToString & "  1.00000"
                        lWrite.WriteLine(lStr)
                    End If
                Next

                lWrite.Close()

                lTimeseriesCsv.Clear()
                lTimeseriesGroup.Clear()
            Else
                Logger.Msg("Unable to Open " & lCsvFileName)
            End If
        End If
        'Silts_Fines.csv
        lInitialConditionsFileName = "Silts_FinesInitialConditions.txt"
        lCsvName = "Silts_Fines.csv"
        If Not FileExists(lInitialConditionsFileName) Then

            Dim lCsvFileName As String = pTestPath & "\" & "AsBaselineHistoric" & "\" & lCsvName
            If lTimeseriesCsv.Open(lCsvFileName) Then

                For Each lTimSerX As atcTimeseries In lTimeseriesCsv.DataSets
                    lTimeseriesGroup.Add(lTimSerX)
                Next

                Dim lWrite As New IO.StreamWriter(lInitialConditionsFileName, False)
                Dim lStr As String = ""
                Dim lCnt As Integer = 0

                For Each lTimSer As atcTimeseries In lTimeseriesGroup
                    If lTimSer.Values(1) > -1 Then
                        lCnt += 1
                        '   1  0.000000  1.00000
                        lStr = "   " & lCnt.ToString & "  " & lTimSer.Value(lTimSer.numValues).ToString & "  1.00000"
                        lWrite.WriteLine(lStr)
                    End If
                Next

                lWrite.Close()

                lTimeseriesCsv.Clear()
                lTimeseriesGroup.Clear()
            Else
                Logger.Msg("Unable to Open " & lCsvFileName)
            End If
        End If

    End Sub

    Private Sub DoERGCompositeGraphRefined()

        Dim lCsvName As String = ""

        Dim pMetalNames As New atcCollection
        pMetalNames.Add("As")
        pMetalNames.Add("Cd")
        pMetalNames.Add("Cu")
        pMetalNames.Add("Ni")
        pMetalNames.Add("Pb")
        pMetalNames.Add("Se")
        pMetalNames.Add("Tl")
        pMetalNames.Add("Zn")

        Dim pTypes As New atcCollection
        pTypes.Add("Total")
        pTypes.Add("Dissolved")

        For Each lType As String In pTypes
            lCsvName = lType & "_Concentration.csv"

            Dim lTimeseriesGroup As New atcTimeseriesGroup

            ChDriveDir(pTestPath & "\OutputPlots")

            For Each lMetal As String In pMetalNames

                Dim lOutFileName As String = pSiteName & lType & lMetal.Substring(0, 2) & "CompositeRev" & "New.png"
                If pSiteName = "MississippiMO" Or pSiteName = "Ohio" Then
                    lOutFileName = pSiteName & lType & lMetal.Substring(0, 2) & "CompositeRevSeg" & pLocation.ToString & "New.png"
                End If

                If Not FileExists(lOutFileName) Then
                    'get baseline historic data
                    Dim lTimeseriesCsv1 As New atcTimeseriesCSV.atcTimeseriesCSV
                    Dim lRunName As String = lMetal & "BaselineHistoric"
                    Dim lCsvFileName As String = pTestPath & "\" & lRunName & "\" & lCsvName

                    If Not FileExists(lCsvFileName & ".start") Then
                        FileCopy(pTestPath & "\" & lRunName & "\" & "Total_Concentration.csv.start", lCsvFileName & ".start")
                    End If

                    If lTimeseriesCsv1.Open(lCsvFileName) Then

                        Dim lTimSer1 As atcTimeseries = lTimeseriesCsv1.DataSets.ItemByKey(pLocation)

                        lTimSer1.Attributes.SetValue("YAxis", "Left")

                        Dim lSDate(5) As Integer : lSDate(0) = pComplianceDate - 5 : lSDate(1) = 1 : lSDate(2) = 1
                        If pSiteName = "LakeSinclair" Then
                            'special case -- limited data
                            lSDate(0) = pComplianceDate - 7 : lSDate(1) = 2 : lSDate(2) = 2
                        End If
                        Dim lSDateJ As Double = Date2J(lSDate)
                        Dim lEDate(5) As Integer : lEDate(0) = pComplianceDate - 1 : lEDate(1) = 12 : lEDate(2) = 30
                        Dim lEdatej As Double = Date2J(lEDate)

                        lTimeseriesGroup.Add(SubsetByDate(lTimSer1, _
                                                    lSDateJ, _
                                                    lEdatej, Nothing))
                        lTimeseriesCsv1.Clear()
                    Else
                        Logger.Msg("Unable to Open " & lCsvFileName)
                    End If

                    'get option d post-compliance data
                    Dim lTimeseriesCsv3 As New atcTimeseriesCSV.atcTimeseriesCSV
                    lRunName = lMetal & "OptionD"
                    lCsvFileName = pTestPath & "\" & lRunName & "\" & lCsvName
                    If Not FileExists(lCsvFileName) Then
                        lRunName = lMetal & "OptionC"
                        lCsvFileName = pTestPath & "\" & lRunName & "\" & lCsvName
                    End If

                    If Not FileExists(lCsvFileName & ".start") Then
                        FileCopy(pTestPath & "\" & lRunName & "\" & "Total_Concentration.csv.start", lCsvFileName & ".start")
                    End If

                    If lTimeseriesCsv3.Open(lCsvFileName) Then

                        Dim lTimSerD As atcTimeseries = lTimeseriesCsv3.DataSets.ItemByKey(pLocation)

                        lTimSerD.Attributes.SetValue("YAxis", "Left")

                        Dim lSDate(5) As Integer : lSDate(0) = pComplianceDate - 1 : lSDate(1) = 12 : lSDate(2) = 31
                        If pSiteName = "LakeSinclair" Then
                            'special case -- limited data
                            lSDate(0) = pComplianceDate : lSDate(1) = 1 : lSDate(2) = 2
                        End If
                        Dim lSDateJ As Double = Date2J(lSDate)
                        Dim lEDate(5) As Integer : lEDate(0) = pComplianceDate + 4 : lEDate(1) = 12 : lEDate(2) = 31
                        If pSiteName = "MississippiMO" Then
                            'special case -- two compliance dates
                            lEDate(0) = pComplianceDate + 8 : lEDate(1) = 12 : lEDate(2) = 31
                        End If
                        If pSiteName = "LakeSinclair" Then
                            'special case -- limited data
                            lEDate(0) = pComplianceDate + 6 : lEDate(1) = 11 : lEDate(2) = 30
                        End If
                        Dim lEdatej As Double = Date2J(lEDate)

                        'lag dates by a day to account for wasp output convention
                        For lindex As Integer = lTimSerD.numValues - 1 To 1 Step -1
                            lTimSerD.Values(lindex + 1) = lTimSerD.Values(lindex)
                        Next

                        lTimeseriesGroup.Add(SubsetByDate(lTimSerD, _
                                                    lSDateJ, _
                                                    lEdatej, Nothing))
                        lTimeseriesCsv3.Clear()
                    Else
                        Logger.Msg("Unable to Open " & lCsvFileName)
                    End If

                    GraphTimeseriesOptions(lTimeseriesGroup, lMetal, lOutFileName, lType, pSiteName)

                    lTimeseriesGroup.Clear()
                End If

            Next
        Next

    End Sub

    Private Sub DoERGCompositeGraphAveragedRefined()

        Dim lCsvName As String = ""

        Dim pMetalNames As New atcCollection
        pMetalNames.Add("As")
        pMetalNames.Add("Cd")
        pMetalNames.Add("Cu")
        pMetalNames.Add("Ni")
        pMetalNames.Add("Pb")
        pMetalNames.Add("Se")
        pMetalNames.Add("Tl")
        pMetalNames.Add("Zn")

        Dim pTypes As New atcCollection
        pTypes.Add("Total")
        pTypes.Add("Dissolved")

        For Each lType As String In pTypes
            lCsvName = lType & "_Concentration.csv"

            Dim lTimeseriesGroup As New atcTimeseriesGroup

            ChDriveDir(pTestPath & "\OutputPlots")

            For Each lMetal As String In pMetalNames

                Dim lOutFileName As String = pSiteName & lType & lMetal.Substring(0, 2) & "CompositeRev" & "AveragedNew.png"
                If pSiteName = "MississippiMO" Or pSiteName = "Ohio" Then
                    lOutFileName = pSiteName & lType & lMetal.Substring(0, 2) & "CompositeRevSeg" & pLocation.ToString & "AveragedNew.png"
                End If

                If Not FileExists(lOutFileName) Then
                    'get baseline historic data
                    Dim lTimeseriesCsv1 As New atcTimeseriesCSV.atcTimeseriesCSV
                    Dim lRunName As String = lMetal & "BaselineHistoric"
                    Dim lCsvFileName As String = pTestPath & "\" & lRunName & "\" & lCsvName

                    If Not FileExists(lCsvFileName & ".start") Then
                        FileCopy(pTestPath & "\" & lRunName & "\" & "Total_Concentration.csv.start", lCsvFileName & ".start")
                    End If

                    If lTimeseriesCsv1.Open(lCsvFileName) Then

                        'calc average value across all cells
                        Dim lTimeseriesGroupToAverage As New atcTimeseriesGroup
                        For Each lTimSerX As atcTimeseries In lTimeseriesCsv1.DataSets
                            lTimeseriesGroupToAverage.Add(lTimSerX)
                        Next
                        Dim lTimSer1 As atcTimeseries = lTimeseriesCsv1.DataSets.ItemByKey(pLocation)
                        Dim lDaySum As Double = 0.0
                        For lIndex As Integer = 1 To lTimSer1.numValues
                            lDaySum = 0.0
                            For Each lTimSer As atcTimeseries In lTimeseriesGroupToAverage
                                If lTimSer.Values(1) > -1 Then
                                    lDaySum = lDaySum + lTimSer.Value(lIndex)
                                End If
                            Next
                            lTimSer1.Value(lIndex) = lDaySum / (lTimeseriesGroupToAverage.Count - 1)
                        Next
                        lTimeseriesGroupToAverage.Clear()

                        lTimSer1.Attributes.SetValue("YAxis", "Left")

                        Dim lSDate(5) As Integer : lSDate(0) = pComplianceDate - 5 : lSDate(1) = 1 : lSDate(2) = 1
                        If pSiteName = "LakeSinclair" Then
                            'special case -- limited data
                            lSDate(0) = pComplianceDate - 7 : lSDate(1) = 2 : lSDate(2) = 2
                        End If
                        Dim lSDateJ As Double = Date2J(lSDate)
                        Dim lEDate(5) As Integer : lEDate(0) = pComplianceDate - 1 : lEDate(1) = 12 : lEDate(2) = 30
                        Dim lEdatej As Double = Date2J(lEDate)

                        lTimeseriesGroup.Add(SubsetByDate(lTimSer1, _
                                                    lSDateJ, _
                                                    lEdatej, Nothing))
                        lTimeseriesCsv1.Clear()
                    Else
                        Logger.Msg("Unable to Open " & lCsvFileName)
                    End If

                    'get option d post-compliance data
                    Dim lTimeseriesCsv3 As New atcTimeseriesCSV.atcTimeseriesCSV
                    lRunName = lMetal & "OptionD"
                    lCsvFileName = pTestPath & "\" & lRunName & "\" & lCsvName
                    If Not FileExists(lCsvFileName) Then
                        lRunName = lMetal & "OptionC"
                        lCsvFileName = pTestPath & "\" & lRunName & "\" & lCsvName
                    End If

                    If Not FileExists(lCsvFileName & ".start") Then
                        FileCopy(pTestPath & "\" & lRunName & "\" & "Total_Concentration.csv.start", lCsvFileName & ".start")
                    End If

                    If lTimeseriesCsv3.Open(lCsvFileName) Then

                        'calc average value across all cells
                        Dim lTimeseriesGroupToAverage As New atcTimeseriesGroup
                        For Each lTimSerX As atcTimeseries In lTimeseriesCsv3.DataSets
                            lTimeseriesGroupToAverage.Add(lTimSerX)
                        Next
                        Dim lTimSerD As atcTimeseries = lTimeseriesCsv3.DataSets.ItemByKey(pLocation)
                        Dim lDaySum As Double = 0.0
                        For lIndex As Integer = 1 To lTimSerD.numValues
                            lDaySum = 0.0
                            For Each lTimSer As atcTimeseries In lTimeseriesGroupToAverage
                                If lTimSer.Values(1) > -1 Then
                                    lDaySum = lDaySum + lTimSer.Value(lIndex)
                                End If
                            Next
                            lTimSerD.Value(lIndex) = lDaySum / (lTimeseriesGroupToAverage.Count - 1)
                        Next
                        lTimeseriesGroupToAverage.Clear()

                        lTimSerD.Attributes.SetValue("YAxis", "Left")

                        Dim lSDate(5) As Integer : lSDate(0) = pComplianceDate - 1 : lSDate(1) = 12 : lSDate(2) = 31
                        If pSiteName = "LakeSinclair" Then
                            'special case -- limited data
                            lSDate(0) = pComplianceDate : lSDate(1) = 1 : lSDate(2) = 2
                        End If
                        Dim lSDateJ As Double = Date2J(lSDate)
                        Dim lEDate(5) As Integer : lEDate(0) = pComplianceDate + 4 : lEDate(1) = 12 : lEDate(2) = 31
                        If pSiteName = "MississippiMO" Then
                            'special case -- two compliance dates
                            lEDate(0) = pComplianceDate + 8 : lEDate(1) = 12 : lEDate(2) = 31
                        End If
                        If pSiteName = "LakeSinclair" Then
                            'special case -- limited data
                            lEDate(0) = pComplianceDate + 6 : lEDate(1) = 11 : lEDate(2) = 30
                        End If
                        Dim lEdatej As Double = Date2J(lEDate)

                        'lag dates by a day to account for wasp output convention
                        For lindex As Integer = lTimSerD.numValues - 1 To 1 Step -1
                            lTimSerD.Values(lindex + 1) = lTimSerD.Values(lindex)
                        Next

                        lTimeseriesGroup.Add(SubsetByDate(lTimSerD, _
                                                    lSDateJ, _
                                                    lEdatej, Nothing))
                        lTimeseriesCsv3.Clear()
                    Else
                        Logger.Msg("Unable to Open " & lCsvFileName)
                    End If

                    GraphTimeseriesOptions(lTimeseriesGroup, lMetal, lOutFileName, lType, pSiteName)

                    lTimeseriesGroup.Clear()
                End If

            Next
        Next

    End Sub

    Private Sub ComputeAverageConcentrationInEachSegment()

        Dim lConcCsvName As String = "Total_Concentration.csv"
        Dim lFlowCsvName As String = "Segment_Outflow.csv"

        Dim lTimeseriesGroupConc As New atcTimeseriesGroup
        Dim lTimeseriesGroupFlow As New atcTimeseriesGroup
        Dim lTimeseriesCsv As New atcTimeseriesCSV.atcTimeseriesCSV

        ChDriveDir(pTestPath & "\OutputPlots")

        Dim pMetalNames As New atcCollection
        'pMetalNames.Add("As")
        'pMetalNames.Add("Cd")
        'pMetalNames.Add("Cu")
        'pMetalNames.Add("Ni")
        pMetalNames.Add("Pb")
        'pMetalNames.Add("Se")
        'pMetalNames.Add("Tl")
        'pMetalNames.Add("Zn")

        Dim lOldOrNew As String = ""
        'lOldOrNew = "\PriorToKdCorrection"

        Dim lSDate(5) As Integer : lSDate(0) = pComplianceDate : lSDate(1) = 1 : lSDate(2) = 1
        Dim lSDateJ As Double = Date2J(lSDate)
        Dim lEDate(5) As Integer : lEDate(0) = pComplianceDate + 9 : lEDate(1) = 12 : lEDate(2) = 31
        Dim lEdatej As Double = Date2J(lEDate)

        Dim lTimeseriesCsvOptionConc As New atcTimeseriesCSV.atcTimeseriesCSV
        Dim lTimeseriesCsvOptionFlow As New atcTimeseriesCSV.atcTimeseriesCSV

        For Each lMetalName As String In pMetalNames

            Dim lOutputFileName As String = lMetalName & "Baseline" & "AverageConcentrationForAllSegments.txt"
            If lOldOrNew.Length > 0 Then
                lOutputFileName = lMetalName & "Baseline" & "AverageConcentrationForAllSegmentsPriorToKdCorrections.txt"
            End If

            If Not FileExists(lOutputFileName) Then
                Dim lOptionName As String = ""

                lOptionName = lMetalName & "Baseline"
                lOptionName = "AAATrial" & lMetalName & "Baseline"

                If FileExists(pTestPath & lOldOrNew & "\" & lOptionName & "\", True) Then
                    'baseline folder exists

                    Dim lCsvFileName As String = pTestPath & lOldOrNew & "\" & lOptionName & "\" & lConcCsvName
                    If lTimeseriesCsvOptionConc.Open(lCsvFileName) Then

                        For Each lTimSerX As atcTimeseries In lTimeseriesCsvOptionConc.DataSets
                            'lTimeseriesGroupConc.Add(SubsetByDate(lTimSerX, _
                            '                         lSDateJ, _
                            '                         lEdatej, Nothing))
                            lTimeseriesGroupConc.Add(lTimSerX)
                        Next

                        'lTimeseriesCsvOption.Clear()
                    Else
                        Logger.Msg("Unable to Open " & lCsvFileName)
                    End If

                    'if start date file doesn't exist, copy from total conc 
                    If Not FileExists(pTestPath & lOldOrNew & "\" & lOptionName & "\" & lFlowCsvName & ".start") Then
                        FileCopy(pTestPath & lOldOrNew & "\" & lOptionName & "\" & lConcCsvName & ".start", pTestPath & lOldOrNew & "\" & lOptionName & "\" & lFlowCsvName & ".start")
                    End If

                    lCsvFileName = pTestPath & lOldOrNew & "\" & lOptionName & "\" & lFlowCsvName
                    If lTimeseriesCsvOptionFlow.Open(lCsvFileName) Then

                        For Each lTimSerX As atcTimeseries In lTimeseriesCsvOptionFlow.DataSets
                            'lTimeseriesGroupFlow.Add(SubsetByDate(lTimSerX, _
                            '                         lSDateJ, _
                            '                         lEdatej, Nothing))
                            lTimeseriesGroupFlow.Add(lTimSerX)
                        Next

                        'lTimeseriesCsvOption.Clear()
                    Else
                        Logger.Msg("Unable to Open " & lCsvFileName)
                    End If

                    'now we have all the conc and flow timeseries available 
                    Dim lHeader As String = lOptionName & " Average Total Concentration and Outflow for All Segments"

                    WriteAverageConcAndFlowFile(lOutputFileName, lTimeseriesGroupConc, lTimeseriesGroupFlow, lHeader)

                    lTimeseriesGroupConc.Clear()
                    lTimeseriesGroupFlow.Clear()
                End If
            End If

        Next

    End Sub

    Sub WriteAverageConcAndFlowFile(ByVal aOutputFileName As String, ByVal aTimSerGroupConc As atcTimeseriesGroup, ByVal aTimSerGroupFlow As atcTimeseriesGroup, ByVal aHeader As String)

        Dim lWrite As New IO.StreamWriter(aOutputFileName, False)
        Dim lDate(5) As Integer

        Dim lStr As String = aHeader
        lWrite.WriteLine(lStr)

        lStr = "Segment, TotalConc, Outflow"
        lWrite.WriteLine(lStr)
        lStr = "  , ug/L , cms"
        lWrite.WriteLine(lStr)
        For lIndex As Integer = 1 To aTimSerGroupConc.Count - 1
            Dim aTimSerConc As atcTimeseries = aTimSerGroupConc(lIndex - 1)
            Dim aTimSerFlow As atcTimeseries = aTimSerGroupFlow(lIndex - 1)
            lStr = lIndex.ToString & " , " & Format(aTimSerConc.Attributes.GetValue("Mean"), "0.########") & " , " & Format(aTimSerFlow.Attributes.GetValue("Mean"), "0.####")
            lWrite.WriteLine(lStr)
        Next

        lWrite.Close()
    End Sub
End Module
