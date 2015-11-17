Imports atcUtility
Imports atcHspfBinOut
Imports HspfSupport
Imports MapWindow.Interfaces
Imports MapWinUtility
Imports System.IO
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Collections.Specialized
Imports System.Xml
Imports System.Xml.Linq
Imports atcMetCmp
Imports atcData
Imports atcWDM
Imports atcGraph
Imports ZedGraph
Imports Microsoft.Office.Interop

Module Util_SERDP
    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        Dim lTask As Integer = 1
        Select Case lTask
            Case 1 : ExtractDailySedLoadPutIntoHourlyTser()
            Case 2 : QATask1RainReductionImpact()
                'Case 3 : ProcessEFDCOutputsTest()
            Case 3 : ProcessEFDCOutputs()
        End Select
    End Sub

    Private Sub ProcessEFDCOutputs()
        'Dim lPath As String = "G:\Admin\SERDP\EFDC\Stock_SED\"
        Dim lScen As String = "BRAC"
        'lScen = "EB"

        Dim lPath As String = "G:\Admin\SERDP\EFDC\"
        'lPath = "G:\Admin\SERDP\EFDC\" & lScen & "\"
        lPath = "D:\temp\efdcRev2\"

        Dim lPathDischarge As String = lPath & "out_discharge_" & lScen & "\"
        Dim lPathSsc As String = lPath & "out_ssc_" & lScen & "\"
        Dim lRchRes As New atcCollection
        Dim lOutSegment1() As String = {"02", "03", "04", "05", "06", "07", "08"}
        Dim lOutSegment2() As String = {"09", "10", "11", "12", "13", "14", "15"}
        Dim lOutSegment3() As String = {"16", "17", "18", "19", "20", "21", "22"}
        Dim lOutSegment4() As String = {"23", "24", "25", "26", "27", "28", "29"}

        lRchRes.Add("1", lOutSegment1)
        lRchRes.Add("2", lOutSegment2)
        lRchRes.Add("3", lOutSegment3)
        lRchRes.Add("4", lOutSegment4)

        'Dim lOutSegments() As String = {"05", "12", "19", "26"}
        Dim lStartDate As Double = Date2J(1999, 10, 1, 0, 0, 0)
        Dim lTimeUnit As atcTimeUnit = atcTimeUnit.TUHour
        Dim lTimeStep As Integer = 1
        Dim lDebug As Boolean = False
        Dim lWDM As String = lPath & "EFDC_" & lScen & ".wdm"
        Dim lWDMH As New atcWDM.atcDataSourceWDM()
        If Not lWDMH.Open(lWDM) Then Exit Sub

        'Dim lxlApp As New Excel.Application
        'Dim lxlWorkbook As Excel.Workbook = Nothing
        'Dim lxlSheet As Excel.Worksheet = Nothing
        'Dim lExceFilename As String = lPath & "EFDCOut.xls"
        'If File.Exists(lExceFilename) Then TryDelete(lExceFilename)
        'Dim lMissingValue As Object = System.Reflection.Missing.Value
        'lxlWorkbook = lxlApp.Workbooks.Add(lMissingValue)
        'lxlSheet = lxlWorkbook.Sheets("Sheet1")
        'lxlWorkbook.SaveAs(lExceFilename)

        Dim lSW As StreamWriter = Nothing
        Dim lPrevRchID As Integer = 0
        Dim lxSectionCounter As Integer
        For Each lRch() As String In lRchRes
            Dim lTsGroupSed As New atcTimeseriesGroup
            Dim lTsGroupFlow As New atcTimeseriesGroup
            For Each lOutSeg As String In lRch
                Dim lFileSedConc As String = lPathSsc & "SEDTS" & lOutSeg & ".OUT"
                Dim lFileFlow As String = lPathDischarge & "UVTTS" & lOutSeg & ".OUT"

                TimeseriesSourceEFDC.Debug = False
                Dim lTsSrcSed As New TimeseriesSourceEFDC(lStartDate, lFileSedConc, "Sediment")
                lTsSrcSed.TimeUnit = lTimeUnit
                lTsSrcSed.TimeStep = lTimeStep

                Dim lTsSrcFlow As New TimeseriesSourceEFDC(lStartDate, lFileFlow, "Flow")
                lTsSrcFlow.TimeUnit = lTimeUnit
                lTsSrcFlow.TimeStep = lTimeStep

                If Not lTsSrcSed.Open() Then
                    Continue For
                Else
                    lTsGroupSed.Add(lTsSrcSed.Timeseries)
                End If

                If Not lTsSrcFlow.Open() Then
                    Continue For
                Else
                    lTsGroupFlow.Add(lTsSrcFlow.Timeseries)
                End If
            Next

            Dim lLocation As String = lTsGroupSed(0).Attributes.GetValue("Location")
            If lLocation.ToLower.Contains("downstream end") Then lLocation = "Reach 74"
            Dim lReachID As Integer = Integer.Parse(lLocation.Substring("Reach ".Length))
            'If lReachID <> lPrevRchID Then
            '    lxSectionCounter = 1
            '    lPrevRchID = lReachID
            'End If
            'lReachID = lReachID * 10 + lxSectionCounter
            'lxsectioncounter += 1

            'sum of flow
            Dim lTsFlow As atcTimeseries = lTsGroupFlow(0).Clone()
            For J As Integer = 1 To lTsGroupFlow(0).numValues
                lTsFlow.Value(J) = 0
                For I As Integer = 0 To lTsGroupFlow.Count - 1
                    lTsFlow.Value(J) += lTsGroupFlow(I).Value(J)
                Next
            Next

            'flow-weighted average sediment concentration
            Dim lTsSed As atcTimeseries = lTsGroupSed(0).Clone()
            For J As Integer = 1 To lTsGroupSed(0).numValues
                lTsSed.Value(J) = 0
                For I As Integer = 0 To lTsGroupSed.Count - 1
                    lTsSed.Value(J) += lTsGroupSed(I).Value(J) * lTsGroupFlow(I).Value(J)
                Next
                lTsSed.Value(J) /= lTsFlow.Value(J)
            Next

            lTsSed.Attributes.SetValue("ID", lReachID)
            lTsFlow.Attributes.SetValue("ID", 100 + lReachID)

            'convert flow from cms to cfs
            '1 cms = 35.3146667 cfs
            For J As Integer = 1 To lTsFlow.numValues
                lTsFlow.Value(J) *= 35.3146667
            Next

            'Dim lTsDaily As atcTimeseries = Aggregate(lTsSed.Timeseries, atcTimeUnit.TUDay, 1, atcTran.TranAverSame)
            'Can't write 144 minutes timestep data; WDTPUT error RetCode = -20
            'resort to excel
            If Not lWDMH.AddDataset(lTsSed, atcDataSource.EnumExistAction.ExistReplace) Then
                Logger.Msg("Failed added Sed @" & lLocation)
            End If

            If Not lWDMH.AddDataset(lTsFlow, atcDataSource.EnumExistAction.ExistReplace) Then
                Logger.Msg("Failed added discharge @" & lLocation)
            End If

            If lDebug Then
                lSW = New StreamWriter(lPath & "zDebugSed.txt", True)
                lSW.WriteLine("----------SED," & lTsSed.Attributes.GetValue("Location") & "------------")
                With lTsSed
                    For I As Integer = 0 To 100
                        lSW.WriteLine(.Dates.Value(I) & vbTab & .Value(I + 1))
                    Next

                    lSW.WriteLine("11111" & vbTab & "22222")
                    lSW.WriteLine("11111" & vbTab & "22222")
                    lSW.WriteLine("11111" & vbTab & "22222")
                    lSW.WriteLine("11111" & vbTab & "22222")
                    lSW.WriteLine("11111" & vbTab & "22222")

                    For I As Integer = .Dates.numValues - 100 To .Dates.numValues - 1
                        lSW.WriteLine(.Dates.Value(I) & vbTab & .Value(I + 1))
                    Next
                End With
                lSW.Flush()
                lSW.Close()
                lSW = Nothing
            End If
            'lxlSheet = lxlWorkbook.Worksheets.Add(lxlSheet)
            'With lxlSheet
            '    .Name = "SEDTS" & lOutSeg
            '    .Cells(1, 1).Value = "Date"
            '    .Cells(1, 2).Value = "SEDCmg/L"
            '    For I As Integer = 0 To lTsSed.Timeseries.Dates.numValues - 1
            '        '.Cells(I + 2, 1).Value = DumpDate(lTsSed.Timeseries.Dates.Value(I))
            '        .Cells(I + 2, 1).Value = lTsSed.Timeseries.Dates.Value(I)
            '        .Cells(I + 2, 2).Value = lTsSed.Timeseries.Value(I + 1)
            '    Next
            'End With
            'lxlWorkbook.Save()

            lTsGroupFlow.Clear()
            lTsGroupSed.Clear()
            lTsFlow.Clear()
            lTsSed.Clear()
        Next
        lWDMH.Clear()
        lWDMH = Nothing

        'lxlWorkbook.Save()
        'lxlWorkbook.Close()

        'lxlApp.Quit()
        'System.Runtime.InteropServices.Marshal.ReleaseComObject(lxlSheet)
        'System.Runtime.InteropServices.Marshal.ReleaseComObject(lxlWorkbook)
        'System.Runtime.InteropServices.Marshal.ReleaseComObject(lxlApp)

        'lxlSheet = Nothing
        'lxlWorkbook = Nothing
        'lxlApp = Nothing

        Logger.Msg("Done Processing EFDC Model Outputs.")
    End Sub

    Private Sub ProcessEFDCOutputsTest()
        Dim lPath As String = "G:\Admin\SERDP\EFDC\"
        Dim lFileSedConc As String = lPath & "SEDTS02.OUT"
        Dim lFileFlow As String = lPath & "UVTTS02.OUT"
        Dim lStartDate As Double = Date2J(1999, 1, 1, 0, 0, 0)
        Dim lTsSed As New TimeseriesSourceEFDC(lStartDate, lFileSedConc, "Sediment")
        Dim lTsFlow As New TimeseriesSourceEFDC(lStartDate, lFileFlow, "Flow")

        Dim lSW As StreamWriter

        If lTsSed.Open() Then
            lSW = New StreamWriter(lPath & "zDebugSed.txt", False)
            With lTsSed.Timeseries
                For I As Integer = 0 To 100
                    lSW.WriteLine(.Dates.Value(I) & vbTab & .Value(I + 1))
                Next

                lSW.WriteLine("11111" & vbTab & "22222")
                lSW.WriteLine("11111" & vbTab & "22222")
                lSW.WriteLine("11111" & vbTab & "22222")
                lSW.WriteLine("11111" & vbTab & "22222")
                lSW.WriteLine("11111" & vbTab & "22222")

                For I As Integer = .Dates.numValues - 100 To .Dates.numValues - 1
                    lSW.WriteLine(.Dates.Value(I) & vbTab & .Value(I + 1))
                Next
            End With
            lSW.Flush()
            lSW.Close()
            lSW = Nothing
        End If

        If lTsFlow.Open() Then
            lSW = New StreamWriter(lPath & "zDebugflow.txt", False)
            With lTsFlow.Timeseries
                For I As Integer = 0 To 100
                    lSW.WriteLine(.Dates.Value(I) & vbTab & .Value(I + 1))
                Next

                lSW.WriteLine("11111" & vbTab & "22222")
                lSW.WriteLine("11111" & vbTab & "22222")
                lSW.WriteLine("11111" & vbTab & "22222")
                lSW.WriteLine("11111" & vbTab & "22222")
                lSW.WriteLine("11111" & vbTab & "22222")

                For I As Integer = .Dates.numValues - 100 To .Dates.numValues - 1
                    lSW.WriteLine(.Dates.Value(I) & vbTab & .Value(I + 1))
                Next
            End With
            lSW.Flush()
            lSW.Close()
            lSW = Nothing
        End If

        lTsFlow.Timeseries.Attributes.SetValue("ID", 1)
        lTsSed.Timeseries.Attributes.SetValue("ID", 2)

        Dim lWDM As String = lPath & "EFDCOut.wdm"
        Dim lWDMH As New atcWDM.atcDataSourceWDM()
        If lWDMH.Open(lWDM) Then
            If Not lWDMH.AddDataset(lTsFlow.Timeseries, atcDataSource.EnumExistAction.ExistReplace) Then
                Logger.Msg("Failed added flow")
            End If
            If Not lWDMH.AddDataset(lTsSed.Timeseries, atcDataSource.EnumExistAction.ExistReplace) Then
                Logger.Msg("Failed added Sed")
            End If
            lWDMH.Clear()
            lWDMH = Nothing
        End If


        Logger.Msg("Done Processing EFDC Model Outputs.")
    End Sub

    Private Sub QATask1RainReductionImpact()
        Dim l9YearRunPath As String = "G:\Admin\SERDP\9YearRuns\"
        Dim l9YearRunWDMName As String = l9YearRunPath & "NineYearRun.wdm"
        Dim l9YearRunWDM As New atcWDM.atcDataSourceWDM
        If Not l9YearRunWDM.Open(l9YearRunWDMName) Then Exit Sub
        Dim lSedBegYear As Integer = 1998
        Dim lRuns() As Integer = {1, 2, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15}
        Dim lSW As New StreamWriter(l9YearRunPath & "QATask1RainReductionImpact.txt", False)
        For Each lRun As Integer In lRuns
            Dim lTsSEDHourly As atcTimeseries = l9YearRunWDM.DataSets.FindData("ID", 200 + lRun)(0)
            Dim lTsSEDDaily As atcTimeseries = l9YearRunWDM.DataSets.FindData("ID", 300 + lRun)(0)

            lTsSEDHourly = Aggregate(lTsSEDHourly, atcTimeUnit.TUYear, 1, atcTran.TranSumDiv, Nothing)
            lTsSEDDaily = Aggregate(lTsSEDDaily, atcTimeUnit.TUYear, 1, atcTran.TranSumDiv, Nothing)

            With lSW
                Dim lLocation As String = lTsSEDDaily.Attributes.GetValue("Location")

                Dim lDate(5) As Integer
                For I As Integer = 0 To lTsSEDDaily.Dates.numValues - 1
                    J2Date(lTsSEDDaily.Dates.Value(I), lDate)
                    .WriteLine(lRun & vbTab & _
                               lLocation & vbTab & _
                               lDate(0) & vbTab & _
                               lTsSEDDaily.Value(I + 1) & vbTab & _
                               lTsSEDHourly.Value(I + 1))
                Next
                .Flush()
            End With
        Next
        lSW.Close()
        lSW = Nothing
        l9YearRunWDM.Clear()
        l9YearRunWDM = Nothing
    End Sub

    Private Sub ExtractDailySedLoadPutIntoHourlyTser()
        'Dim l9YearRunPath As String = "D:\Projects\SERDP\WEPP\9YearRuns\"
        'Dim l9YearRunWDMName As String = l9YearRunPath & "NineYearRun.wdm"
        'Dim l9YearRunWDM As New atcWDM.atcDataSourceWDM
        Dim l13YearRunPath As String = "D:\Projects\SERDP\WEPP\13YearRuns\"
        Dim l13YearRunWDMName As String = l13YearRunPath & "13YearRun.wdm"
        Dim l13YearRunWDM As New atcWDM.atcDataSourceWDM
        If Not l13YearRunWDM.Open(l13YearRunWDMName) Then Exit Sub
        Dim lSedBegYear As Integer = 1998
        Dim lRuns() As Integer = {1, 2, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15}
        For Each lRun As Integer In lRuns
            Dim lRunPath As String = l13YearRunPath & lRun.ToString & "\"

            Dim lCliFile As New TimeseriesSourceWEPPCli(lSedBegYear, lRunPath & "in.cli")
            With lCliFile
                If Not .Open() Then Continue For
                .DailyTimeseries.Attributes.SetValue("ID", lRun)
                .DailyFractionTimeseries.Attributes.SetValue("ID", 100 + lRun)

                Dim lDataAdded As Boolean = True
                If Not l13YearRunWDM.AddDataset(.DailyTimeseries, atcDataSource.EnumExistAction.ExistReplace) Then
                    Logger.Dbg("Failed to add run " & lRun & " precip to WDM.")
                    lDataAdded = False
                End If

                If Not l13YearRunWDM.AddDataset(.DailyFractionTimeseries, atcDataSource.EnumExistAction.ExistReplace) Then
                    Logger.Dbg("Failed to add run " & lRun & " fraction to WDM.")
                    lDataAdded = False
                End If

                If Not lDataAdded Then Continue For
            End With

            Dim lOutPlotFile As New TimeseriesSourceWEPPOutPlot(lSedBegYear, lRunPath & "out-plot.txt")
            With lOutPlotFile
                .BegDate = lCliFile.DailyTimeseries.Dates.Value(0)
                .EndDate = lCliFile.DailyTimeseries.Dates.Value(lCliFile.DailyTimeseries.Dates.numValues)
                If Not .Open() Then Continue For
                'For-Debug----
                'Dim lSW As New StreamWriter(Path.Combine(lRunPath, "out-plot-log-tslisting.txt"), False)
                'For I As Integer = 0 To .PointTs.numValues
                '    lSW.WriteLine(.PointTs.Dates.Value(I) & vbTab & .PointTs.Value(I))
                'Next
                'lSW.Flush() : lSW.Close() : lSW = Nothing
                'End-Debug----
            End With

            'Clone the hourly timeseries for the new sediment timeseries
            Dim lSedTs As atcTimeseries = lCliFile.DailyFractionTimeseries.Clone()
            With lSedTs
                'Reset some attributes
                .Attributes.SetValue("Constituent", "SED")
                .Attributes.SetValue("TSTYP", "SED")
                .Attributes.SetValue("Scenario", "WEPP")
                .Attributes.SetValue("Units", "tpa")
                .Attributes.SetValue("ID", 200 + lRun)
            End With 'lSedTs

            'create a new set of values as some of the days have rain, but no sediment loss
            Dim lSedValues(lSedTs.numValues) As Double
            lSedValues(0) = GetNaN()
            'Distribute sed daily values into lSedTs's hourly timestep
            With lOutPlotFile.PointTs
                Dim lLookedTillHere As Integer = 0
                For I As Integer = 0 To .Values.Length - 2 'by pass the last value, which is the sum annual value
                    Dim lSedDailyTotal As Double = .Value(I)
                    Dim lSedOutputDate As Double = .Dates.Value(I) 'set at the beginning moment of the day
                    'If lRun = 15 AndAlso lSedOutputDate = Date2J(2007, 4, 14, 0, 0, 0) Then
                    ''this is a good test for edge of day problem
                    '    Dim lfound As String = ""
                    'End If
                    Dim lD As Integer
                    For lD = lLookedTillHere To lSedTs.Dates.numValues
                        If lSedTs.Dates.Value(lD) >= lSedOutputDate AndAlso lSedTs.Dates.Value(lD) < lSedOutputDate + JulianHour * 24 Then
                            'can't use <= end of day moment as that is actually the beginning of the next day
                            If lSedTs.Value(lD + 1) > 0 Then
                                lSedValues(lD + 1) = lSedTs.Value(lD + 1) * lSedDailyTotal
                            End If
                        ElseIf lSedTs.Dates.Value(lD) >= lSedOutputDate + JulianHour * 24 Then
                            Exit For 'lSed.Dates loop
                        End If
                    Next
                    lLookedTillHere = lD - 1
                    If lLookedTillHere < 0 Then lLookedTillHere = 0
                Next
            End With
            'swap in the new values
            'For I As Integer = 1 To lSedTs.numValues
            '    lSedTs.Value(I) = lSedValues(I)
            'Next
            lSedTs.Values = lSedValues

            'Write the sed ts to WDM
            If Not l13YearRunWDM.AddDataset(lSedTs, atcDataSource.EnumExistAction.ExistReplace) Then
                Logger.Dbg("Failed to add run " & lRun & " sediment to WDM.")
            End If

            Try
                lOutPlotFile.ContinuousTs.Attributes.SetValue("ID", 300 + lRun)
                lOutPlotFile.ContinuousTs.Attributes.SetValue("Location", lSedTs.Attributes.GetValue("Location", "<unk>"))
                lOutPlotFile.ContinuousTs.Attributes.SetValue("Scenario", lSedTs.Attributes.GetValue("Scenario"))
                If Not l13YearRunWDM.AddDataset(lOutPlotFile.ContinuousTs, atcDataSource.EnumExistAction.ExistReplace) Then
                    Logger.Dbg("Failed to add run " & lRun & " Point sediment to WDM.")
                End If
            Catch ex As Exception
                'do nothing
            End Try

            lCliFile.Clear() : lCliFile = Nothing
            lOutPlotFile.Clear() : lOutPlotFile = Nothing
            lSedTs.Clear() : lSedTs = Nothing
            System.GC.Collect()
        Next 'lRun
        l13YearRunWDM.Clear()
        l13YearRunWDM = Nothing
    End Sub

#Region "UtilityClasses"
    Public Class TimeseriesSourceWEPPCli
        Public ReadOnly StartingYear As Integer 'must be set
        Private pBegDate As Double
        Public Property BegDate() As Double
            Get
                Return pBegDate
            End Get
            Set(ByVal value As Double)
                pBegDate = value
            End Set
        End Property

        Private pEndDate As Double
        Public Property EndDate() As Double
            Get
                Return pEndDate
            End Get
            Set(ByVal value As Double)
                pEndDate = value
            End Set
        End Property

        Private pSpecification As String
        Public Property Specification() As String
            Get
                Return pSpecification
            End Get
            Set(ByVal value As String)
                pSpecification = value
            End Set
        End Property

        Private pWeatherStatsInfoBlock As String
        Public Property WeatherStatsInfoBlock() As String
            Get
                Return pWeatherStatsInfoBlock
            End Get
            Set(ByVal value As String)
                pWeatherStatsInfoBlock = value
            End Set
        End Property

        Private pDailyTimeseries As atcTimeseries
        Public Property DailyTimeseries() As atcTimeseries
            Get
                If pDailyTimeseries Is Nothing Then
                    Return New atcTimeseries(Nothing)
                Else
                    Return pDailyTimeseries
                End If
            End Get
            Set(ByVal value As atcTimeseries)
                pDailyTimeseries = value
            End Set
        End Property

        Private pDailyFractionTimeseries As atcTimeseries
        Public Property DailyFractionTimeseries() As atcTimeseries
            Get
                If pDailyFractionTimeseries Is Nothing Then
                    Return New atcTimeseries(Nothing)
                Else
                    Return pDailyFractionTimeseries
                End If
            End Get
            Set(ByVal value As atcTimeseries)
                pDailyFractionTimeseries = value
            End Set
        End Property

        Public Sub New(ByVal aStartingYear As Integer, Optional ByVal aSpec As String = "")
            StartingYear = aStartingYear
            If File.Exists(aSpec) Then Specification = aSpec
        End Sub

        Public Function Open() As Boolean
            Dim lOpened As Boolean = True

            If Not File.Exists(Specification) Then Return False
            Dim lMatchDailyValue As String = "^\s+(\d+) (\d+\.\d*)$"
            'Dim lMatchDailyLeaderLine As String = "^\s+(\d+ ){2}(\d+)\s+(\d+)\s+(\d+\.\d*\s+){5}(\d+\.\d*)$"
            Dim lMatchDailyLeaderLine As String = "^\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(-*\d+\.\d*\s*){6}$"
            '   32.5161   -84.9422        150 9 1 9
            Dim lMatchLocationInfo As String = "^\s+(\d+\.\d+)\s+(-\d+\.\d+)\s+(\d+) (\d+) (\d+) (\d+)"
            Dim lValues As New List(Of Double)
            lValues.Add(Double.NaN)
            Dim lFractions As New List(Of Double)
            lFractions.Add(Double.NaN)
            Dim lArrValue() As Double = Nothing
            Dim lArrFraction() As Double = Nothing

            Dim lSW As New StreamWriter(IO.Path.Combine(IO.Path.GetDirectoryName(Specification), "LogDates.txt"), False)

            Dim lSR As New StreamReader(Specification)
            Dim lOneLine As String
            Dim lWeatherInfoBlock As New StringBuilder()
            Dim lTotalTimeWithinDay As Integer
            Dim lTimeWithinDay As Integer
            Dim lDateValue1 As Integer
            Dim lDateValue2 As Integer
            Dim lDateValue3 As Integer
            Dim lLocation As String = ""
            Dim lScenario As String = ""
            Dim lConstituent As String = ""
            Dim lLongitude As Double
            Dim lLatitude As Double
            Dim lElevation As Double

            While Not lSR.EndOfStream
                lOneLine = lSR.ReadLine()
                Dim mDailyValue As Match = Regex.Match(lOneLine, lMatchDailyValue, RegexOptions.IgnoreCase)
                Dim mDailyLeaderLine As Match = Regex.Match(lOneLine, lMatchDailyLeaderLine, RegexOptions.IgnoreCase)
                If mDailyLeaderLine.Success Then 'at least in this case, the leader line comes first
                    lSW.WriteLine(lOneLine)
                    lDateValue1 = Integer.Parse(mDailyLeaderLine.Groups(1).Value.Trim) 'day
                    lDateValue2 = Integer.Parse(mDailyLeaderLine.Groups(2).Value.Trim) 'month
                    lDateValue3 = Integer.Parse(mDailyLeaderLine.Groups(3).Value.Trim) 'ordinal year starting from 1
                    lTotalTimeWithinDay = Integer.Parse(mDailyLeaderLine.Groups(4).Value.Trim) 'total timesteps in this period, for within a day, it is 24
                    If lDateValue3 = 10 Then
                        Dim lFound As String = ""
                    End If
                    ReDim lArrValue(lTotalTimeWithinDay - 1)
                    ReDim lArrFraction(lTotalTimeWithinDay - 1)
                    If Not BegDate > 0 Then
                        BegDate = Date2J(StartingYear + lDateValue3, lDateValue2, lDateValue1, 0, 0, 0)
                    End If
                ElseIf mDailyValue.Success Then
                    lTimeWithinDay = Integer.Parse(mDailyValue.Groups(1).Value.Trim) 'hour
                    Dim lValue2 As Double 'daily value
                    If Not Double.TryParse(mDailyValue.Groups(2).Value.Trim, lValue2) Then
                        lValue2 = 0.0
                    End If
                    lArrValue(lTimeWithinDay - 1) = lValue2

                    If lTimeWithinDay = lTotalTimeWithinDay Then 'do the summary when it reach the last time devision in side a period (day)
                        'summarize this day's worth of rain
                        Dim lTotalValue As Double = lArrValue(lArrValue.Length - 1)
                        If lTotalValue > 0 Then
                            'First, convert from mm to inch
                            '1 millimeter = 0.0393700787 inch
                            For I As Integer = 0 To lArrValue.Length - 1
                                lArrValue(I) *= 0.0393700787
                            Next
                            '2nd, discretize into each step from the original cumulative values
                            Dim lNewArrValue(lArrValue.Length - 1) As Double
                            For I As Integer = 0 To lArrValue.Length - 1
                                If I = 0 Then
                                    lNewArrValue(I) = lArrValue(I)
                                Else
                                    lNewArrValue(I) = lArrValue(I) - lArrValue(I - 1)
                                End If
                            Next
                            lValues.AddRange(lNewArrValue) 'add the original discretized rain data
                            '3rd, remove first 0.2 inch of precip for erosion delay
                            Dim lInitialPrecipStore As Double = 0.2 'assume first 0.2 inch of rain don't generate erosion
                            For I As Integer = 0 To lNewArrValue.Length - 1
                                If lNewArrValue(I) > 0 Then
                                    If lNewArrValue(I) - lInitialPrecipStore >= 0 Then
                                        lNewArrValue(I) -= lInitialPrecipStore
                                        Exit For 'all initial storage is gone
                                    Else
                                        lInitialPrecipStore -= lNewArrValue(I)
                                        lNewArrValue(I) = 0
                                    End If
                                End If
                            Next
                            '4th, now get the new total for this period, ie day
                            lTotalValue = 0
                            For I As Integer = 0 To lNewArrValue.Length - 1
                                lTotalValue += lNewArrValue(I)
                            Next
                            '4th, calculate fraction of each step out of the total of this period
                            If lTotalValue > 0 Then
                                For I As Integer = 0 To lArrFraction.Length - 1
                                    lArrFraction(I) = lNewArrValue(I) / lTotalValue
                                Next
                            End If
                            ReDim lNewArrValue(0)
                        Else
                            'if all zeros, then simply add it to the value collection
                            lValues.AddRange(lArrValue) 'add the original discretized rain data
                        End If 'lTotalValue > 0

                        '5th, add these to their own collection before moving on to next day
                        lFractions.AddRange(lArrFraction)
                    End If
                Else
                    If lOneLine.Contains("OBSERVED") Then
                        Dim lArr() As String = Regex.Split(lOneLine, "\s+")
                        If lArr(0) = "" Then
                            lScenario = lArr(1)
                            lLocation = lArr(2)
                            lConstituent = lArr(3)
                        Else
                            lScenario = lArr(0).Trim
                            lLocation = lArr(1).Trim
                            lConstituent = lArr(2).Trim
                        End If
                    Else
                        Dim mLocationInfo As Match = Regex.Match(lOneLine, lMatchLocationInfo, RegexOptions.IgnoreCase)
                        If mLocationInfo.Success Then
                            lLatitude = Double.Parse(mLocationInfo.Groups(1).Value.Trim)
                            lLongitude = Double.Parse(mLocationInfo.Groups(2).Value.Trim)
                            lElevation = Double.Parse(mLocationInfo.Groups(3).Value.Trim)
                        End If
                    End If
                    lWeatherInfoBlock.AppendLine(lOneLine)
                End If
            End While
            lSR.Close()
            lSR = Nothing
            lSW.Flush()
            lSW.Close()
            lSW = Nothing

            If Not EndDate > 0 Then
                EndDate = Date2J(StartingYear + lDateValue3, lDateValue2, lDateValue1, lTimeWithinDay, 0, 0)
            End If

            Dim lTsDates As New atcTimeseries(Nothing)
            lTsDates.Values = NewDates(BegDate, EndDate, atcTimeUnit.TUHour, 1)
            DailyTimeseries = New atcTimeseries(Nothing)
            With DailyTimeseries
                .Dates = lTsDates
                .numValues = .Dates.numValues
                
                .SetInterval(atcTimeUnit.TUHour, 1)

                .Attributes.SetValue("Location", lLocation)
                .Attributes.SetValue("Scenario", lScenario)
                .Attributes.SetValue("Constituent", lConstituent)
                .Attributes.SetValue("TSTYP", lConstituent)
                .Attributes.SetValue("Latitude", lLatitude)
                .Attributes.SetValue("Longitude", lLongitude)
                .Attributes.SetValue("Elev", lElevation)
                .Attributes.SetValue("Units", "in")

                .Values = lValues.ToArray()

            End With

            DailyFractionTimeseries = New atcTimeseries(Nothing)
            With DailyFractionTimeseries
                .Dates = lTsDates
                .numValues = lTsDates.numValues
                .SetInterval(atcTimeUnit.TUHour, 1)
                .Values = lFractions.ToArray()
                .Attributes.SetValue("Location", lLocation)
                .Attributes.SetValue("Scenario", lScenario)
                .Attributes.SetValue("Constituent", "Frac")
                .Attributes.SetValue("TSTYP", "Frac")
                .Attributes.SetValue("Latitude", lLatitude)
                .Attributes.SetValue("Longitude", lLongitude)
                .Attributes.SetValue("Elev", lElevation)
                .Attributes.SetValue("Units", "Dless")
            End With

            lValues.Clear() : lValues = Nothing
            lFractions.Clear() : lFractions = Nothing
            Return lOpened
        End Function

        Public Sub Clear()
            DailyTimeseries.Clear() : DailyTimeseries = Nothing
            DailyFractionTimeseries.Clear() : DailyFractionTimeseries = Nothing
        End Sub

    End Class

    Public Class TimeseriesSourceWEPPOutPlot
        Public ReadOnly StartingYear As Integer 'must be set
        Public PointTs As atcTimeseries
        Public ContinuousTs As atcTimeseries
        Public Specification As String
        Public BegDate As Double
        Public EndDate As Double

        Public Sub New(ByVal aStartingYear As Integer, Optional ByVal aSpec As String = "")
            StartingYear = aStartingYear
            If File.Exists(aSpec) Then Specification = aSpec
        End Sub

        Public Function Open() As Boolean
            Dim lOpened As Boolean = True
            'Dim lSW As New StreamWriter(IO.Path.Combine(IO.Path.GetDirectoryName(Specification), "out-plot-log.txt"), False)
            If Not File.Exists(Specification) Then Return False
            Dim lValues As New List(Of Double)
            Dim lDates As New List(Of Double)
            Dim lMatchPatternOutPlot As String = "^DATE\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+\.\d+E*-*\d+)\s*$"
            'DATE           10          10           1   0.226857E-01
            Dim lSedYear As Integer
            Dim lSedMonth As Integer
            Dim lSedDay As Integer
            Dim lDate As Double
            Dim lSR As New StreamReader(Specification)
            While Not lSR.EndOfStream
                Dim lOneLine As String = lSR.ReadLine()
                Dim mDailySedLoad As Match = Regex.Match(lOneLine, lMatchPatternOutPlot, RegexOptions.IgnoreCase)
                If mDailySedLoad.Success Then
                    lSedDay = Integer.Parse(mDailySedLoad.Groups(1).Value.Trim)
                    lSedMonth = Integer.Parse(mDailySedLoad.Groups(2).Value.Trim)
                    lSedYear = StartingYear + Integer.Parse(mDailySedLoad.Groups(3).Value.Trim)
                    lDate = Date2J(lSedYear, lSedMonth, lSedDay, 0, 0, 0)
                    Dim lValueDailySedLoad As Double = Double.Parse(mDailySedLoad.Groups(4).Value.Trim) 'kg/m2
                    If lValueDailySedLoad > 0 Then
                        'lSW.WriteLine(lOneLine) -FOR DEBUG
                        '(1 kilogram) per (square meter) = 4.46089561 short ton per acre
                        lValueDailySedLoad *= 4.46 'convert into short ton per acre
                        lDates.Add(lDate)
                        lValues.Add(lValueDailySedLoad)
                    End If 'only record non-zero sediment events
                End If 'mDailySedLoad.Success
            End While
            lSR.Close() : lSR = Nothing
            'lSW.Flush() : lSW.Close() : lSW = Nothing -FOR DEBUG
            'build timeseries
            Dim lTsDates As New atcTimeseries(Nothing)
            lTsDates.Attributes.SetValue("Point", True)
            lTsDates.Values = lDates.ToArray()

            If PointTs Is Nothing Then PointTs = New atcTimeseries(Nothing)
            With PointTs
                .Attributes.SetValue("Point", True)
                .Attributes.SetValue("Constituent", "PtSED")
                .Attributes.SetValue("TSTYP", "PtSED")
                .Attributes.SetValue("Units", "tpa")
                .Dates = lTsDates
                .numValues = .Dates.numValues
                .Values = lValues.ToArray()
            End With

            If BegDate > 0 AndAlso EndDate > 0 AndAlso EndDate > BegDate Then
                Dim lFixedDatesTs As New atcTimeseries(Nothing)
                lFixedDatesTs.Values = NewDates(BegDate, EndDate, atcTimeUnit.TUDay, 1)
                If ContinuousTs Is Nothing Then ContinuousTs = New atcTimeseries(Nothing)
                With ContinuousTs
                    .Dates = lFixedDatesTs
                    .numValues = .Dates.numValues
                    .Attributes.SetValue("Constituent", "PtSED")
                    .Attributes.SetValue("TSTYP", "PtSED")
                    .SetInterval(atcTimeUnit.TUDay, 1)
                    Dim lLastDate As Double = PointTs.Dates.Value(PointTs.Dates.Values.Length - 2)
                    For I As Integer = 0 To .Dates.numValues
                        If .Dates.Value(I) > lLastDate Then
                            Exit For
                        End If
                        For J As Integer = 0 To PointTs.Dates.Values.Length - 2 'bypass the last value
                            If .Dates.Value(I) = PointTs.Dates.Value(J) Then
                                .Value(I + 1) = PointTs.Value(J)
                                Exit For
                            End If
                        Next
                    Next
                End With
            End If

            lDates.Clear() : lDates = Nothing
            lValues.Clear() : lValues = Nothing
            Return lOpened
        End Function

        Public Sub Clear()
            PointTs.Clear()
            PointTs = Nothing
            ContinuousTs.Clear()
            ContinuousTs = Nothing
        End Sub
    End Class

    Public Class TimeseriesSourceEFDC
        Public Specification As String
        Public StartDate As Double

        Public DataColumn As Integer

        Public DataType As String

        Public Timeseries As atcTimeseries

        Public Shared Debug As Boolean

        Public TimeUnit As atcTimeUnit = atcTimeUnit.TUUnknown
        Public TimeStep As Integer

        Public Sub New(ByVal aStartingDate As Integer, _
                       Optional ByVal aSpec As String = "", _
                       Optional ByVal aDataType As String = "", _
                       Optional ByVal aDataColumn As Integer = 0)
            StartDate = aStartingDate
            If File.Exists(aSpec) Then Specification = aSpec
            DataColumn = aDataColumn
            DataType = aDataType
        End Sub

        Public Function Open() As Boolean
            Dim lOpened As Boolean = True
            Dim lDebug As Boolean = True

            Dim lSW As StreamWriter = Nothing
            Dim lLogFile As String = IO.Path.ChangeExtension(Specification, "Log")

            If Not File.Exists(Specification) Then Return False
            Dim lValues As New List(Of Double) : lValues.Add(GetNaN())
            Dim lDates As New List(Of Double) : lDates.Add(StartDate)
            Dim lUnits As String = ""
            Dim lCons As String = ""
            Dim lMatchPatternOut As String = ""
            Dim lMultiplier As Double = 1.0
            If DataType.ToLower.Contains("sediment") Then
                lMatchPatternOut = "^\s+([0-9]+\.*[0-9]+)\s+([^\s]+)\s+([^\s]+)\s*$"
                '  2192.02075  0.2008E+03  0.1942E+01
                DataColumn = 3
                lUnits = "mg/LITER"
                lCons = "SEDC"
            ElseIf DataType.ToLower.Contains("flow") Then
                lMatchPatternOut = "^\s+([0-9]+\.*[0-9]+)\s+([^\s]+)\s+([^\s]+)\s+(.+)\s*$"
                '  2192.02075  0.0000E+00 -0.1730E+01  0.0000E+00  0.0000E+00  0.0000E+00  0.0000E+00
                DataColumn = 3
                lUnits = "cms"
                lCons = "FLOW"
                lMultiplier = -1.0
            End If

            Dim lMatchPatternTimestep As String = "^\s+TIME IN FIRST COLUMN HAS UNITS OF ([A-Z]+)\s*$"
            Dim lMatchPatternLocation As String = "^\s+AT LOCATION\s+(.+)$"

            Dim lDateNominal As Double
            Dim lDate As Double
            Dim lDateArr(5) As Integer
            Dim lSR As New StreamReader(Specification)
            Dim location As String = ""
            Dim lTimeDivision As String = ""
            Dim lValue As Double

            Dim lEDFCYearCount As Integer = 0
            Dim lEDFCFirstNominalYear As Integer = -99

            If Debug Then
                lSW = New StreamWriter(lLogFile, False)
                lSW.WriteLine(StartDate & vbTab & "-->" & DumpDate(StartDate))
            End If

            Dim lLineCount As Integer = 0
            While Not lSR.EndOfStream
                Dim lOneLine As String = lSR.ReadLine()
                Dim mOutputValue As Match = Regex.Match(lOneLine, lMatchPatternOut, RegexOptions.IgnoreCase)
                If lLineCount < 5 Then
                    Dim mLocation As Match = Regex.Match(lOneLine, lMatchPatternLocation, RegexOptions.IgnoreCase)
                    Dim mTimeStep As Match = Regex.Match(lOneLine, lMatchPatternTimestep, RegexOptions.IgnoreCase)
                    If mLocation.Success Then
                        location = mLocation.Groups(1).Value.Trim()
                    ElseIf mTimeStep.Success Then
                        lTimeDivision = mTimeStep.Groups(1).Value.ToUpper.Trim()
                    End If
                    lLineCount += 1
                End If
                If mOutputValue.Success Then
                    lDateNominal = Double.Parse(mOutputValue.Groups(1).Value.Trim)
                    If lEDFCFirstNominalYear < 0 Then
                        lEDFCFirstNominalYear = CInt(lDateNominal)
                    End If

                    lDate = StartDate + (lDateNominal - lEDFCFirstNominalYear)
                    'J2Date(lDate, lDateArr)

                    lValue = Double.Parse(mOutputValue.Groups(DataColumn).Value.Trim)
                    If lValue < 0 Then lValue *= lMultiplier
                    'lSW.WriteLine(lOneLine) -FOR DEBUG
                    '(1 kilogram) per (square meter) = 4.46089561 short ton per acre
                    'lValueSedConc *= 4.46 'convert into short ton per acre
                    lDates.Add(lDate)
                    lValues.Add(lValue)

                    If Debug Then
                        lSW.WriteLine(lDateNominal & vbTab & lDate & vbTab & DumpDate(lDate))
                    End If
                End If 'mDailySedLoad.Success
            End While
            lSR.Close() : lSR = Nothing
            If Debug Then
                lSW.Flush() : lSW.Close() : lSW = Nothing '-FOR DEBUG
                Debug = False 'only do it once
            End If

            'build timeseries
            Dim lTsDates As New atcTimeseries(Nothing)
            lTsDates.Values = lDates.ToArray()

            If Timeseries Is Nothing Then Timeseries = New atcTimeseries(Nothing)

            With Timeseries
                .Attributes.SetValue("Constituent", lCons)
                .Attributes.SetValue("TSTYP", lCons)
                .Attributes.SetValue("Location", location)
                .Attributes.SetValue("Scenario", "EFDC")
                If lUnits.Length > 5 Then lUnits = lUnits.Substring(0, 5)
                .Attributes.SetValue("Units", lUnits)
                .Dates = lTsDates
                .numValues = .Dates.numValues
                .Values = lValues.ToArray()


                If TimeUnit = atcTimeUnit.TUUnknown OrElse TimeStep = 0 Then
                    Dim lTimeStep As Integer
                    Dim lTimeUnit As atcTimeUnit
                    CalcTimeUnitStep(lDates(0), lDates(1), lTimeUnit, lTimeStep)
                    lTimeStep = 30 'TODO: hack here
                    .SetInterval(lTimeUnit, lTimeStep)
                Else
                    'Dim lNewDates() As Double = NewDates(lDates(0), lDates(lDates.Count - 1), TimeUnit, TimeStep)
                    '.Dates.Values = lNewDates
                    .SetInterval(TimeUnit, TimeStep)
                End If
            End With

            lDates.Clear() : lDates = Nothing
            lValues.Clear() : lValues = Nothing
            Return lOpened
        End Function
    End Class
#End Region
End Module
