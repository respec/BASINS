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
    Private Const pTestPath As String = "C:\ERG_SteamElectric\Black"
    Private pLocation As Integer = 39
    Private pStartYear As Integer = 1999
    Private pComplianceDate As Integer = 2019
    'etowah
    'Private Const pTestPath As String = "C:\ERG_SteamElectric\Etowah"
    'Private pLocation As Integer = 18
    'Private pStartYear As Integer = 2004
    'Private pComplianceDate As Integer = 2021

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)

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

        Dim lSDate(5) As Integer : lSDate(0) = pStartYear : lSDate(1) = 1 : lSDate(2) = 1
        Dim lSDateJ As Double = Date2J(lSDate)
        Dim lEDate(5) As Integer : lEDate(0) = 2020 : lEDate(1) = 12 : lEDate(2) = 31
        Dim lEdatej As Double = Date2J(lEDate)

        'do baseline historic plots
        For Each lRunName As String In pRunNames

            Dim lOutFileName As String = lRunName & ".png"

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

        lSDate(0) = pComplianceDate : lSDate(1) = 1 : lSDate(2) = 1
        lSDateJ = Date2J(lSDate)
        lEDate(0) = pComplianceDate + 9 : lEDate(1) = 12 : lEDate(2) = 31
        lEdatej = Date2J(lEDate)

        Dim lTimeseriesOptionsGroup As New atcTimeseriesGroup
        Dim lTimeseriesCsvOption As New atcTimeseriesCSV.atcTimeseriesCSV

        'do post-compliance option plots
        For Each lMetalName As String In pMetalNames
            Dim lOutFileName As String = lMetalName & "Options.png"

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
                        lTimeseriesCsvOption.Clear()
                    Else
                        Logger.Msg("Unable to Open " & lCsvFileName)
                    End If
                End If

                GraphTimeseriesOptions(lTimeseriesGroup, lMetalName, lOutFileName, "Computed Tot Conc Ug/L")
                lTimeseriesGroup.Clear()
            End If
        Next

    End Sub

    Sub GraphTimeseriesBaselineHistoric(ByVal aDataGroup As atcTimeseriesGroup, ByVal aRunName As String, ByVal aOutFile As String)
        Dim lZgc As ZedGraphControl = CreateZgc()
        Dim lGrapher As New clsGraphTime(aDataGroup, lZgc)
        Dim lPane As GraphPane = lZgc.MasterPane.PaneList(0)
        Dim lCurve As ZedGraph.LineItem

        lCurve = lPane.CurveList.Item(0)
        lCurve.Color = Drawing.Color.Blue
        lPane.YAxis.Scale.Min = 0

        lPane.XAxis.Title.Text = aRunName.Substring(0, 2) & " at Segment " & pLocation
        lZgc.Width = 1200

        lZgc.SaveIn(aOutFile)
        lZgc.Dispose()
    End Sub

    Sub GraphTimeseriesOptions(ByVal aDataGroup As atcTimeseriesGroup, ByVal aRunName As String, ByVal aOutFile As String, ByVal aUnits As String)
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
                lCurve.Color = Drawing.Color.Yellow
                lCurve.Label.Text = "Option D"
            End If
        Next i

        lPane.YAxis.Scale.Min = 0
        lPane.YAxis.Title.Text = aUnits

        lPane.XAxis.Title.Text = aRunName.Substring(0, 2) & " at Segment " & pLocation
        lZgc.Width = 1200

        lZgc.SaveIn(aOutFile)
        lZgc.Dispose()
    End Sub

End Module
