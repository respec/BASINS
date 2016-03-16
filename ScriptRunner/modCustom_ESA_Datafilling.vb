Imports System.Collections.Specialized
Imports atcUtility
Imports atcData
Imports atcWDM
Imports MapWinUtility
Imports System.Text.RegularExpressions
Imports Microsoft.Office.Interop.Excel

Module modCustom
    Private pMissingValue As Integer = -999
    Private pAccumulValue As Integer = -998
    Private pTimeStep As Integer = 15
    Private Enum EVQualALERT
        Good1 = 1
        Good2 = 2
        Good3 = 70
        Accum = 80
        Accum1 = 81
        Miss = 151
        Miss1 = 255
        Zero = 153
    End Enum
    Public Structure DataFile
        Public Location As String
        Public BeginYear As Integer
        Public EndYear As Integer
    End Structure

    Private Function GetDate(ByVal aTimeText As String) As Integer()
        Dim lNewTime As DateTime
        If DateTime.TryParse(aTimeText, lNewTime) Then
            Dim lDate(5) As Integer
            lDate(0) = lNewTime.Year
            lDate(1) = lNewTime.Month
            lDate(2) = lNewTime.Day
            lDate(3) = lNewTime.Hour
            lDate(4) = lNewTime.Minute
            lDate(5) = lNewTime.Second
            Return lDate
        Else
            Return Nothing
        End If
    End Function

    Public Sub P2Adjust15minRawWithRoundingFactor()
        Dim lRaw15minDataFilename As String = "C:\Projects\DataUpdate\Deliver\Met_Haines_15min_Precip\Met_Haines.wdm"
        Dim lRawDataAdjRndFilename As String = "C:\Projects\DataUpdate\Deliver\Met_Haines_15min_Precip\Met_Haines_Adj4Rnd.wdm"
        Dim lRawData15minWDM As New atcWDM.atcDataSourceWDM()
        If Not lRawData15minWDM.Open(lRaw15minDataFilename) Then Exit Sub
        Dim lRawDataAdjRndWDM As New atcWDM.atcDataSourceWDM()
        If Not lRawDataAdjRndWDM.Open(lRawDataAdjRndFilename) Then Exit Sub

        Dim lRoundingAdjFactor As Double = 0.03937 / 0.04
        Dim lDatasetIDs As New Generic.List(Of Integer)
        lDatasetIDs.Add(26)
        lDatasetIDs.Add(27)
        lDatasetIDs.Add(28)
        lDatasetIDs.Add(29)

        Dim lID As Integer = 0
        Dim lDoneCtr As Integer = 1
        Dim lVal As Double = 0.0
        Logger.Status("Adjusting 15-min rainfall with rounding factor ...")
        For Each lTser As atcTimeseries In lRawData15minWDM.DataSets
            lID = lTser.Attributes.GetValue("ID")
            If Not lDatasetIDs.Contains(lID) Then Continue For
            Logger.Progress(lDoneCtr, lDatasetIDs.Count)
            lTser.EnsureValuesRead()
            For I As Integer = 0 To lTser.numValues
                lVal = lTser.Value(I)
                If Double.IsNaN(lVal) OrElse lVal < 0 Then
                    'bypass
                Else
                    'adjust with rounding factor
                    lTser.Value(I) *= lRoundingAdjFactor
                End If
            Next
            If Not lRawDataAdjRndWDM.AddDataSet(lTser) Then
                Logger.Dbg("Station: " & lTser.Attributes.GetValue("Location") & " failed saving adjusted time serie.")
            End If
            lDoneCtr += 1
        Next
        lRawData15minWDM.Clear()
        lRawData15minWDM = Nothing
        lRawDataAdjRndWDM.Clear()
        lRawDataAdjRndWDM = Nothing
        Logger.Msg("Done adjusting 15-min raw precip data.", MsgBoxStyle.Information, "Phase 2 Adjusting 15 min data with rounding factor")
    End Sub

    Public Sub CreateBlankRDBStreamflow()

        Dim lRDBFilename As String = "C:\Projects\GW Toolbox\IntermittenData\NWIS_discharge_01118500.rdb"
        Dim lRDBFilenameNew As String = IO.Path.GetFileNameWithoutExtension(lRDBFilename) & "-999.rdb"
        lRDBFilenameNew = IO.Path.Combine(IO.Path.GetDirectoryName(lRDBFilename), lRDBFilenameNew)
        Dim lRDB As New atcTimeseriesRDB.atcTimeseriesRDB()
        If lRDB.Open(lRDBFilename) Then
            Dim lDataTser As atcTimeseries = lRDB.DataSets(0)
            For i As Integer = 1 To lDataTser.numValues
                lDataTser.Value(i) = -999
            Next
        End If

        If Not lRDB.Save(lRDBFilenameNew) Then
            Logger.Msg("Saving new RDB file: " & lRDBFilenameNew & " failed.", MsgBoxStyle.Critical, "Create Blank RDB Streamflow")
        Else
            Logger.Msg("Saved new RDB file: " & lRDBFilenameNew, MsgBoxStyle.Information, "Create Blank RDB Streamflow")
        End If
    End Sub

    Public Sub P2CompareRawAggregateVsCountyDailySum()
        'Dim lRawDailyDataFilename As String = "C:\Projects\DataUpdate\Deliver\Met_Haines_15min_Precip\Met_Haines_daily.wdm"
        Dim lRawDailyDataFilename As String = "C:\Projects\DataUpdate\Deliver\Met_Haines_15min_Precip\Met_Haines_dailyAdj.wdm"
        Dim lCountyDailyDataFilename As String = "C:\Projects\DataUpdate\Phase2\DailyPrecData\dailyrain.wdm"
        Dim lRawDailyAggWDM As New atcWDM.atcDataSourceWDM()
        Dim lCountyDailyWDM As New atcWDM.atcDataSourceWDM()
        If Not lRawDailyAggWDM.Open(lRawDailyDataFilename) Then Exit Sub
        If Not lCountyDailyWDM.Open(lCountyDailyDataFilename) Then Exit Sub

        Dim lCountyDailyTser As atcTimeseries = Nothing
        'Dim lMsg As New Text.StringBuilder()
        'For Each lTser As atcTimeseries In lRawDailyAggWDM.DataSets
        '    lCountyDailyTser = lCountyDailyWDM.DataSets.FindData("Location", lTser.Attributes.GetValue("Location"))(0)
        '    lMsg.Append(lTser.Attributes.GetValue("Location") & "->Raw: " & DumpDate(lTser.Attributes.GetValue("SJDay")) & " ~ ")
        '    lMsg.Append(DumpDate(lTser.Attributes.GetValue("EJDay")) & " | County: ")
        '    lMsg.Append(DumpDate(lCountyDailyTser.Attributes.GetValue("SJDay")) & " ~ ")
        '    lMsg.AppendLine(DumpDate(lCountyDailyTser.Attributes.GetValue("EJDay")))
        'Next
        'Logger.Msg(lMsg.ToString())
        'Dim lInfoFileDateRange As String = IO.Path.Combine(IO.Path.GetDirectoryName(lCountyDailyDataFilename), "CompareToRaw\Raw_vs_County_Dates.txt")
        'Dim lSW As New System.IO.StreamWriter(lInfoFileDateRange, False)
        'lSW.WriteLine(lMsg.ToString())
        'lSW.Flush()
        'lSW.Close()
        'lSW = Nothing

        Dim lSW As System.IO.StreamWriter = Nothing
        Dim lCompTable As New atcTableDelimited()
        'Dim lTableDirName As String = "C:\Projects\DataUpdate\Phase2\DailyPrecData\CompareToRaw\"
        Dim lTableDirName As String = "C:\Projects\DataUpdate\Phase2\DailyPrecData\CompareToRawAdj\"
        Dim location As String = ""
        Dim lTserCtr As Integer = 1
        For Each lTser As atcTimeseries In lRawDailyAggWDM.DataSets
            location = lTser.Attributes.GetValue("Location")
            Logger.Progress(location, lTserCtr, lRawDailyAggWDM.DataSets.Count)
            lTser.EnsureValuesRead()
            With lCompTable
                .Delimiter = ","
                .NumFields = 3
                .FieldName(1) = "Date"
                .FieldName(2) = "RawAgg"
                .FieldName(3) = "County"
                .CurrentRecord = 1
            End With

            lCountyDailyTser = lCountyDailyWDM.DataSets.FindData("Location", location)(0)
            lCountyDailyTser.EnsureValuesRead()
            'Only care about the dates of the raw 15-minute data
            Dim lRawVal As Double
            Dim lCountyVal As Double
            Dim lRawVal_Rd As Double
            Dim lCountyVal_Rd As Double
            Dim lIsSameValue As Boolean

            Dim lRawDate As Double
            Dim lCountyDate As Double
            Dim lDates(5) As Integer
            With lCompTable
                Dim lSearchIndex As Integer = 1
                For I As Integer = 1 To lTser.numValues
                    'lRawDate = lTser.Dates.Value(I - 1)
                    lRawDate = lTser.Dates.Value(I)
                    lRawVal = lTser.Value(I)

                    If lRawVal < 0 Then Continue For

                    For J As Integer = lSearchIndex To lCountyDailyTser.numValues
                        'lCountyDate = lCountyDailyTser.Dates.Value(J - 1)
                        lCountyDate = lCountyDailyTser.Dates.Value(J)
                        lCountyVal = lCountyDailyTser.Value(J)

                        If lRawDate > lCountyDate Then
                            'need to let county data continue search forward
                        ElseIf lRawDate < lCountyDate Then
                            Exit For 'county time loop and let raw time loop continue down forward
                        Else 'Dates match, then compare values
                            'Comparison should be done by first round up to 2-sig digit after decimal point
                            lIsSameValue = True
                            If Double.IsNaN(lRawVal) AndAlso Double.IsNaN(lCountyVal) Then
                                'both are missing --> pass
                            ElseIf Double.IsNaN(lRawVal) AndAlso Not Double.IsNaN(lCountyVal) Then
                                lIsSameValue = False
                            ElseIf lRawVal < 0 AndAlso lCountyVal < 0 Then
                                'both are missing--> pass
                            ElseIf lRawVal < 0 AndAlso Not (lCountyVal < 0) Then
                                lIsSameValue = False
                            Else
                                lRawVal_Rd = Math.Round(lRawVal, 2)
                                lCountyVal_Rd = Math.Round(lCountyVal, 2)
                                If Math.Abs(lRawVal_Rd - lCountyVal_Rd) > 0.001 Then
                                    lIsSameValue = False
                                End If
                            End If

                            'If Math.Abs(lRawVal - lCountyVal) < 0.001 Then
                            ''Matching dates and matching value, no need to report
                            'Else
                            ''mismatch value on matching date, need to report
                            If Not lIsSameValue Then
                                '.Value(1) = DumpDate(lRawDate)
                                J2Date(lRawDate, (lDates))
                                timcnv((lDates))
                                .Value(1) = lDates(0) & "/" & lDates(1).ToString().PadLeft(2, "0") & "/" & lDates(2).ToString().PadLeft(2, "0") &
                                            " " & lDates(3).ToString().PadLeft(2, "0") & ":" & lDates(4).ToString().PadLeft(2, "0") & ":" & lDates(5).ToString().PadLeft(2, "0")
                                .Value(2) = lRawVal
                                .Value(3) = lCountyVal
                                .CurrentRecord += 1
                            End If
                            'End If

                            lSearchIndex = J
                            Exit For 'county time loop
                        End If
                    Next
                Next
            End With
            lSW = New IO.StreamWriter(IO.Path.Combine(lTableDirName, "Compare_" & location.Replace(" ", "_") & ".txt"), False)
            lSW.WriteLine(lCompTable.ToString())
            lSW.Flush()
            lSW.Close()
            lSW = Nothing
            lCompTable.Clear()
            'lTser.Clear()
            'lCountyDailyTser.Clear()
            lTserCtr += 1
        Next

        lRawDailyAggWDM.Clear()
        lRawDailyAggWDM = Nothing
        lCountyDailyWDM.Clear()
        lCountyDailyWDM = Nothing
    End Sub

    Public Sub P2UpdateRaw15minWithCountyDaily()

        Dim SpecialRawDataValueRelabel As Integer = -997

        Dim lPattern As String = "Compare_(\w+).txt"

        Dim lRoutineTitle As String = "P2: Update Raw With County Daily"
        Dim lRawDataDir As String = "C:\Projects\DataUpdate\Deliver\Met_Haines_15min_Precip"
        Dim lRawDataFilename As String = "Met_Haines_Adj4Rnd.wdm"
        Dim lRawDataFilename_Bfc As String = "Met_Haines_Adj4RndBfc.wdm" 'back filled with county daily total for missing and rounding error
        Dim lRawDataWDM As New atcWDM.atcDataSourceWDM()
        If Not lRawDataWDM.Open(IO.Path.Combine(lRawDataDir, lRawDataFilename)) Then
            Logger.Msg("Open raw 15-min data WDM failed.", MsgBoxStyle.Critical, lRoutineTitle)
            Exit Sub
        End If
        Dim lRawDataWDM_Bfc As New atcWDM.atcDataSourceWDM()
        If Not lRawDataWDM_Bfc.Open(IO.Path.Combine(lRawDataDir, lRawDataFilename_Bfc)) Then
            Logger.Msg("Open update raw 15-min data WDM failed.", MsgBoxStyle.Critical, lRoutineTitle)
            Exit Sub
        End If

        Dim lRawVsCountyReportDir As String = "C:\Projects\DataUpdate\Phase2\DailyPrecData\CompareToRawAdj"
        Dim lDir As New IO.DirectoryInfo(lRawVsCountyReportDir)
        Dim lFileInfos As IO.FileInfo() = lDir.GetFiles("*.txt")
        Dim lFileInfo As IO.FileInfo
        Dim lReportListing As New Generic.Dictionary(Of String, String)
        Dim lFileTotalCount As Integer = lFileInfos.Length
        Dim lFileCount As Integer = 1
        For Each lFileInfo In lFileInfos
            Dim lFilename As String = IO.Path.GetFileName(lFileInfo.FullName)
            Dim lMatches As MatchCollection = Nothing
            Dim lFoundMatch As Boolean = True
            Try
                lMatches = Regex.Matches(lFilename, lPattern, RegexOptions.IgnoreCase)
            Catch ex As Exception
                lFoundMatch = False
            End Try
            If Not lFoundMatch Then Continue For
            Dim lStationName As String = lMatches(0).Groups(1).Value
            System.Diagnostics.Debug.Print("Accessing file " & lFileCount & " out of " & lFileTotalCount)
            If Not String.IsNullOrEmpty(lStationName) AndAlso Not String.IsNullOrEmpty(lStationName.Trim()) Then
                lStationName = lStationName.Trim().Replace("_", " ")
                lReportListing.Add(lStationName, lFileInfo.FullName)
                lFileCount += 1
            End If
        Next

        Dim lRawVal As Double
        Dim lCountyVal As Double
        Dim lNewValues(1) As Double
        Dim lRawDate As Double = 0
        Dim lRawValue0 As Double = 0
        Dim lTimeStepsToPeak As Integer = 96

        Dim lCompTable As atcTableDelimited
        lFileTotalCount = lReportListing.Count
        lFileCount = 1
        For Each lStnName As String In lReportListing.Keys
            Logger.Progress("Updating " & lStnName, lFileCount, lFileTotalCount)
            Logger.Dbg("Stn-" & lStnName & ": update starts...")

            Dim lRawTser As atcTimeseries = lRawDataWDM.DataSets.FindData("Location", lStnName)(0)

            lCompTable = New atcTableDelimited()
            With lCompTable
                .Delimiter = ","
            End With
            If Not lCompTable.OpenFile(lReportListing(lStnName)) Then
                Logger.Dbg("Stn-" & lStnName & ": open failed.")
                lCompTable.Clear()
                lCompTable = Nothing
                Continue For
            End If

            Dim lSearchIndex As Integer = 0
            With lCompTable
                .CurrentRecord = 1
                While Not .EOF
                    If String.IsNullOrEmpty(.Value(1)) Then
                        .CurrentRecord += 1
                        Continue While
                    End If
                    If Not Double.TryParse(.Value(2), lRawVal) Then
                        lRawVal = Double.NaN
                    End If
                    If Not Double.TryParse(.Value(3), lCountyVal) Then
                        lCountyVal = Double.NaN
                        'Logger.Msg("Bad County Data!", MsgBoxStyle.Critical, lRoutineTitle)
                        .CurrentRecord += 1
                        Continue While
                    End If

                    Dim lWinDate As DateTime = Nothing
                    DateTime.TryParse(.Value(1).Split(" ")(0), lWinDate)
                    lRawTser.EnsureValuesRead()
                    Dim lDayEnd As Double = Date2J(lWinDate.Year, lWinDate.Month, lWinDate.Day, 24, 0, 0)
                    Dim lDayBeg As Double = Date2J(lWinDate.Year, lWinDate.Month, lWinDate.Day - 1, 24, 0, 0)
                    'lDayBeg = lDayEnd - JulianHour * 24
                    Dim lDayLast15MinStart As Double = lDayEnd - JulianMinute * 15

                    lRawDate = 0
                    Dim lRoundingScenarioIsChecked As Boolean = False
                    Dim lRoundingScenarioDay As Boolean = True
                    For I As Integer = lSearchIndex To lRawTser.numValues - 1
                        lRawDate = lRawTser.Dates.Value(I)
                        lRawValue0 = lRawTser.Value(I + 1)
                        If lRawDate < lDayBeg Then
                            Continue For
                        ElseIf lRawDate > lDayEnd Then
                            lSearchIndex = I
                            Exit For
                        End If

                        'determine if the next 96 timesteps are of the same value
                        If Not lRoundingScenarioIsChecked Then
                            Dim J As Integer = I
                            'Dim lpeekValue As Double = lRawTser.Value(I + 1)
                            For peek As Integer = J + 1 To J + 95
                                If Math.Abs(lRawValue0 - lRawTser.Value(peek)) > 0.01 Then
                                    'Not all values are the same, so skip this day
                                    lRoundingScenarioDay = False
                                    Exit For
                                End If
                            Next
                            lRoundingScenarioIsChecked = True
                            If Not lRoundingScenarioDay Then
                                Exit For
                            End If
                        End If

                        If lRawDate < lDayLast15MinStart Then
                            If lCountyVal > 0 Then
                                lRawTser.Value(I + 1) = SpecialRawDataValueRelabel
                                Logger.Dbg("Stn-" & lStnName & ": update (raw)" & lRawValue0 & "->(label)" & SpecialRawDataValueRelabel & " on " & DumpDate(lRawDate))
                            ElseIf lCountyVal < 0 Then
                                Logger.Dbg("Stn-" & lStnName & ": update (raw)" & lRawValue0 & "->(county)bad_value:" & lCountyVal & " on " & DumpDate(lRawDate))
                            Else
                                lRawTser.Value(I + 1) = lCountyVal
                                Logger.Dbg("Stn-" & lStnName & ": update (raw)" & lRawValue0 & "->(county)" & lCountyVal & " on " & DumpDate(lRawDate))
                            End If
                        Else
                            lRawTser.Value(I + 1) = lCountyVal
                            Logger.Dbg("Stn-" & lStnName & ": update (raw)" & lRawValue0 & "->(county)" & lCountyVal & " on " & DumpDate(lRawDate))
                            lSearchIndex = I
                            Exit For
                        End If
                    Next
                    .CurrentRecord += 1
                End While
                .Clear()
                Logger.Dbg("Stn-" & lStnName & ": update END.")
            End With
            If Not lRawDataWDM_Bfc.AddDataset(lRawTser) Then
                Logger.Dbg("Stn-" & lStnName & ": save update failed.")
            End If
            lRawTser.Dates.Clear()
            lRawTser.Values = lNewValues
            lFileCount += 1
        Next
        Logger.Progress("Done.", lFileTotalCount, lFileTotalCount)
        lRawDataWDM.Clear()
        lRawDataWDM = Nothing
        lRawDataWDM_Bfc.Clear()
        lRawDataWDM_Bfc = Nothing
        Logger.Msg("Done updating raw 15-min with county data.", MsgBoxStyle.Information, lRoutineTitle)
    End Sub

    Public Sub P2SumRaw15minToDaily()
        'Dim lRaw15minDataFilename As String = "C:\Projects\DataUpdate\Deliver\Met_Haines_15min_Precip\Met_Haines.wdm"
        'Dim lRawDailyDataFilename As String = "C:\Projects\DataUpdate\Deliver\Met_Haines_15min_Precip\Met_Haines_daily.wdm"
        Dim lRaw15minDataFilename As String = "C:\Projects\DataUpdate\Deliver\Met_Haines_15min_Precip\Met_Haines_Adj4Rnd.wdm"
        Dim lRawDailyDataFilename As String = "C:\Projects\DataUpdate\Deliver\Met_Haines_15min_Precip\Met_Haines_dailyAdj.wdm"
        Dim lRawData15minWDM As New atcWDM.atcDataSourceWDM()
        If Not lRawData15minWDM.Open(lRaw15minDataFilename) Then Exit Sub
        Dim lRawDataDailyWDM As New atcWDM.atcDataSourceWDM()
        If Not lRawDataDailyWDM.Open(lRawDailyDataFilename) Then Exit Sub

        Dim lDatasetIDs As New Generic.List(Of Integer)
        lDatasetIDs.Add(21)
        lDatasetIDs.Add(22)
        lDatasetIDs.Add(23)
        lDatasetIDs.Add(24)
        lDatasetIDs.Add(25)
        lDatasetIDs.Add(26)
        lDatasetIDs.Add(27)
        lDatasetIDs.Add(28)
        lDatasetIDs.Add(29)

        Dim lDailyAggTser As atcTimeseries = Nothing
        Dim lID As Integer = 0
        Dim lDoneCtr As Integer = 1
        Logger.Status("Aggregating 15-min rainfall into daily...")
        For Each lTser As atcTimeseries In lRawData15minWDM.DataSets
            lID = lTser.Attributes.GetValue("ID")
            If Not lDatasetIDs.Contains(lID) Then Continue For
            Logger.Progress(lDoneCtr, lDatasetIDs.Count)
            lTser.EnsureValuesRead()
            lDailyAggTser = Aggregate(lTser, atcTimeUnit.TUDay, 1, atcTran.TranSumDiv)
            For I As Integer = 0 To lDailyAggTser.numValues
                If lDailyAggTser.Value(I) < 0 Then
                    lDailyAggTser.Value(I) = pMissingValue
                End If
            Next
            If Not lRawDataDailyWDM.AddDataset(lDailyAggTser) Then
                Logger.Dbg("Station: " & lTser.Attributes.GetValue("Location") & " failed saving daily aggregates.")
            End If
            lDailyAggTser.Clear()
            lDailyAggTser = Nothing
            lDoneCtr += 1
        Next
        lRawData15minWDM.Clear()
        lRawData15minWDM = Nothing
        lRawDataDailyWDM.Clear()
        lRawDataDailyWDM = Nothing
        Logger.Msg("Done aggregating 15-min raw precip data.", MsgBoxStyle.Information, "Phase 2 Aggregate 15 min data into daily")
    End Sub

    Public Sub P2ReadDailyToWDMs()
        Dim lPattern As String = "(.*)\s24-hour.csv"
        Dim lDataDir As String = "C:\Projects\DataUpdate\Phase2\DailyPrecData\"
        Dim lDir As New IO.DirectoryInfo(lDataDir)
        Dim lFileInfos As IO.FileInfo() = lDir.GetFiles("*.csv")
        Dim lFileInfo As IO.FileInfo
        Dim lDataListing As New Generic.Dictionary(Of String, SortedDictionary(Of Integer, DataFile))
        Dim lFileTotalCount As Integer = lFileInfos.Length
        Dim lFileCount As Integer = 1
        Dim lDailyRainFiles As New atcCollection()
        For Each lFileInfo In lFileInfos
            Dim lFilename As String = IO.Path.GetFileName(lFileInfo.FullName)
            Dim lMatches As MatchCollection = Nothing
            Dim lFoundMatch As Boolean = True
            Try
                lMatches = Regex.Matches(lFilename, lPattern, RegexOptions.IgnoreCase)
            Catch ex As Exception
                lFoundMatch = False
            End Try
            If Not lFoundMatch Then Continue For
            Dim lStationName As String = lMatches(0).Groups(1).Value
            lDailyRainFiles.Add(lStationName, lFileInfo.FullName)
            lFileCount += 1
        Next

        Dim lWDMFilename As String = lDataDir & "dailyrain.wdm"
        Dim lWDMFile As atcWDM.atcDataSourceWDM = New atcWDM.atcDataSourceWDM()
        If Not lWDMFile.Open(lWDMFilename) Then Exit Sub
        Dim lprojectFolder As String = IO.Path.GetDirectoryName(lWDMFilename)
        Dim lsaveInFolder As String = lprojectFolder

        Dim lCountMissing As Integer = 0
        Dim lValueD As Double = 0
        Dim lVBTime As DateTime = Nothing
        Dim lStartDate As Double
        Dim lEndDate As Double
        Dim lDates(5) As Integer
        Dim lcsvTable As New atcTableDelimited()
        With lcsvTable
            Dim lDatasetID As Integer = 1
            Dim lTser As atcTimeseries = Nothing
            For Each lStationKey As String In lDailyRainFiles.Keys
                Logger.Dbg("Start reading station: " & lStationKey & "...")
                lCountMissing = 0
                .NumHeaderRows = 2
                .Delimiter = ","
                .NumFields = 2
                Logger.Status("Reading daily rain from station: " & lStationKey)
                If .OpenFile(lDailyRainFiles.ItemByKey(lStationKey)) Then
                    'Construct a new timeseries
                    .CurrentRecord = 1
                    'it is said the daily values are suppose to be at the end of the previous day
                    'e.g.
                    '7/20/1982 00:00 --> 7/19/1982 24:00
                    DateTime.TryParse(.Value(1), lVBTime)
                    lStartDate = Date2J(lVBTime.Year, lVBTime.Month, lVBTime.Day - 1, 0, 0, 0)

                    .CurrentRecord = .NumRecords
                    DateTime.TryParse(.Value(1), lVBTime)
                    lEndDate = Date2J(lVBTime.Year, lVBTime.Month, lVBTime.Day - 1, 24, 0, 0)

                    lTser = New atcTimeseries(Nothing)
                    lTser.Dates = New atcTimeseries(Nothing)
                    lTser.Dates.Values = NewDates(lStartDate, lEndDate, atcTimeUnit.TUDay, 1)
                    lTser.numValues = lTser.Dates.numValues
                    lTser.Value(0) = GetNaN()
                    With lTser.Attributes
                        .SetValue("Constituent", "PREC")
                        .SetValue("TSTYPE", "PREC")
                        .SetValue("ID", lDatasetID)
                        .SetValue("Location", lStationKey)
                        .SetValue("Scenario", "OBSERVED")
                        .SetValue("Description", "Rand Daily")
                        '.SetValue("History 1", "C:\Projects\DataUpdate\Phase2\DailyPrecData\" & lStationKey)
                    End With
                    lTser.SetInterval(atcTimeUnit.TUDay, 1)
                    Dim lDateSearchIndex As Integer = 1
                    Dim lDateCount As Integer = lTser.Dates.numValues
                    Dim lRowSearch As Integer
                    Dim lEOFReached As Boolean = False
                    Dim lcsvTime As Double
                    Dim lTserTime As Double
                    Dim lValueIsValid As Boolean
                    .CurrentRecord = 1
                    lRowSearch = .CurrentRecord
                    lCountMissing = 0
                    For I As Integer = lDateSearchIndex To lDateCount
                        If I Mod 500 = 0 Then
                            Logger.Progress(I, lDateCount)
                        End If
                        lTserTime = lTser.Dates.Value(I)
                        'try to find a matching date in csvTable
                        .CurrentRecord = lRowSearch
                        While Not .EOF
                            If DateTime.TryParse(.Value(1), lVBTime) Then
                                lcsvTime = Date2J(lVBTime.Year, lVBTime.Month, lVBTime.Day, 0, 0, 0)
                                If Math.Abs(lTserTime - lcsvTime) < 0.0001 Then
                                    'found the matching time
                                    If Double.TryParse(.Value(2), lValueD) Then
                                        lValueIsValid = True
                                    Else
                                        lValueIsValid = False
                                        lValueD = pMissingValue
                                        lCountMissing += 1
                                        Logger.Dbg("Station: " & lStationKey & " missing data on " & .Value(1))
                                    End If
                                    lTser.Value(I) = lValueD
                                    lRowSearch = .CurrentRecord + 1 'assuming csvTable doesn't have duplicate dates rows
                                    Exit While 'jump out of the csvTable row loop
                                ElseIf lcsvTime > lTserTime Then
                                    lTser.Value(I) = pMissingValue
                                    lCountMissing += 1
                                    lRowSearch = .CurrentRecord
                                    Logger.Dbg("Station: " & lStationKey & " missing data on " & .Value(1))
                                    Exit While 'jump out of the csvTable row loop
                                ElseIf lcsvTime < lTserTime Then
                                    'Do nothing
                                    Dim lStop As String = "Stope"
                                End If
                            Else
                                'Do nothing, just go on the next line for the next table date
                                Logger.Dbg("Station: " & lStationKey & " has bad date @ " & .Value(1))
                            End If
                            .CurrentRecord += 1
                        End While
                    Next 'Tser timestep
                End If 'csvTable Opened

                If lCountMissing > 0 Then
                    Logger.Dbg("Done reading station: " & lStationKey & " Missing Count: " & lCountMissing)
                Else
                    Logger.Dbg("Done reading station: " & lStationKey)
                End If
                Logger.Progress(100, 100)

                'Save to WDM
                If lWDMFile.AddDataset(lTser, atcDataSource.EnumExistAction.ExistReplace) Then
                    'Dim lSumAnnualPREC As Double = lNewTser.Attributes.GetValue("SumAnnual")
                    'If lSW IsNot Nothing Then lSW.WriteLine(lThisLocation & " Penman-Monteith PET at DSN " & lID & " in file " & lWDMFile.Specification & ", SumAnnual " & lSumAnnualPET & " inches")
                    'Logger.Dbg("Wrote Penman-Monteith PET to DSN " & lID & " SumAnnual " & lSumAnnualPET)
                Else
                    'If lSW IsNot Nothing Then lSW.WriteLine(lThisLocation & " failed writing Penman-Montheith PET to DSN " & lID & " in file " & lWDMFile.Specification)
                    'Logger.Dbg("**** Problem Writing Penman-Monteith PET to DSN " & lID)
                End If
                lTser.Clear()
                lTser = Nothing
                lDatasetID += 1
                .Clear()
            Next 'station
        End With

        lcsvTable.Clear()
        lcsvTable = Nothing
        Logger.Msg("All daily stations are process.", MsgBoxStyle.Information, "Phase 2: Reading Daily Rainfall Data")
    End Sub

    Public Sub ReadRawPrecipData()
        Dim locations() As String = {"Barona", "Bonita", "El Capitan Reservoir", "Encinitas", "Escondido", "Fallbrook", "Fashion Valley", "Flinn Springs", "Granite Hills", "La Mesa", "Lake Henshaw", "Lake Wohlford", "Los Coches", "Oceanside", "Poway", "Ramona", "San Onofre", "Santee", "Sutherland Reservoir", "Descanso", "Dulzura", "Kearny Mesa", "lake Cuyamaca", "Lower Otay Reservoir", "Morena Lake", "San Vicente Reservoir", "San Ysidro", "Santa Ysabel", "Tierra del Sol"}
        Dim lPattern As String = "(\w+)\s-\s?(.+)15-min\s(\d{4})-(\d{4})\.xlsx"

        Dim lDataDir As String = "E:\Projects\DataUpdate\Reading\"
        Dim lDir As New IO.DirectoryInfo(lDataDir)
        Dim lFileInfos As IO.FileInfo() = lDir.GetFiles("*.xlsx")
        Dim lFileInfo As IO.FileInfo
        Dim lDataListing As New Generic.Dictionary(Of String, SortedDictionary(Of Integer, DataFile))
        Dim lFileTotalCount As Integer = lFileInfos.Length
        Dim lFileCount As Integer = 1
        For Each lFileInfo In lFileInfos
            Dim lFilename As String = IO.Path.GetFileName(lFileInfo.FullName)
            Dim lMatches As MatchCollection = Nothing
            Dim lFoundMatch As Boolean = True
            Try
                lMatches = Regex.Matches(lFilename, lPattern, RegexOptions.IgnoreCase)
            Catch ex As Exception
                lFoundMatch = False
            End Try
            If Not lFoundMatch Then Continue For
            Dim lStationName As String = lMatches(0).Groups(2).Value
            System.Diagnostics.Debug.Print("Accessing file " & lFileCount & " out of " & lFileTotalCount)
            If Not String.IsNullOrEmpty(lStationName) AndAlso Not String.IsNullOrEmpty(lStationName.Trim()) Then
                lStationName = lStationName.Trim()
                Dim lBeginYear As String = lMatches(0).Groups(3).Value
                Dim lEndYear As String = lMatches(0).Groups(4).Value
                Dim lintBeginYear As Integer
                Dim lintEndYear As Integer
                Integer.TryParse(lBeginYear, lintBeginYear)
                Integer.TryParse(lEndYear, lintEndYear)

                Dim lDataFiles As Generic.SortedDictionary(Of Integer, DataFile) = Nothing
                If lDataListing.TryGetValue(lStationName, lDataFiles) Then
                    Dim lDataFile As DataFile = Nothing
                    If lDataFiles.TryGetValue(lintBeginYear, lDataFile) Then
                        'already exists, do nothing
                        Dim lExists As String = "Yes"
                    Else
                        lDataFile = New DataFile()
                        With lDataFile
                            .BeginYear = lintBeginYear
                            .EndYear = lintEndYear
                            .Location = lFileInfo.FullName
                        End With
                        lDataFiles.Add(lintBeginYear, lDataFile)
                    End If
                Else
                    Dim lDict As New SortedDictionary(Of Integer, DataFile)
                    Dim lDataFile As DataFile = New DataFile()
                    With lDataFile
                        .BeginYear = lintBeginYear
                        .EndYear = lintEndYear
                        .Location = lFileInfo.FullName
                    End With
                    lDict.Add(lintBeginYear, lDataFile)
                    lDataListing.Add(lStationName, lDict)
                End If

            End If
            lFileCount += 1
        Next

        Dim lWDMFilename As String = "E:\Projects\DataUpdate\Met_Haines.wdm"
        Dim lWDMFile As atcWDM.atcDataSourceWDM = New atcWDM.atcDataSourceWDM()
        If Not lWDMFile.Open(lWDMFilename) Then Exit Sub
        Dim lprojectFolder As String = IO.Path.GetDirectoryName(lWDMFilename)
        Dim lsaveInFolder As String = lprojectFolder

        Dim lXLApp As New Application()
        Dim lXLBook As Workbook = Nothing
        Dim lXLSheet As Worksheet = Nothing
        Dim lDataSpan As New SortedDictionary(Of Integer, DataFile)
        Dim lDatasetID As Integer = lWDMFile.DataSets.Count + 1 '1 <-- do this one at a time, so change this to 2, 3, so on each run

        Dim lSW As IO.StreamWriter = Nothing
        For Each Loc As String In lDataListing.Keys
            Logger.Status("Reading data from """ & Loc & """ [" & lDatasetID & " out of " & lDataListing.Count & "]")
            Dim lDatafiles As SortedDictionary(Of Integer, DataFile) = lDataListing.Item(Loc)

            'lSW = New IO.StreamWriter(IO.Path.Combine(lDataDir, Loc & ".txt"), False)

            Dim lStartDate As Double
            Dim lEndDate As Double
            Dim lCount As Integer = 0
            Dim lDateSearchIndex As Integer = 3
            For Each lKeyBeginYear As Integer In lDatafiles.Keys
                If lCount = 1 Then lDateSearchIndex = -99
                Dim lFile As DataFile = lDatafiles.Item(lKeyBeginYear)
                If String.IsNullOrEmpty(lFile.Location) Then Continue For
                lXLBook = lXLApp.Workbooks.Open(lFile.Location)
                lXLSheet = lXLBook.Worksheets(1)
                Dim lDateValue As String = ""
                If lCount = 0 Then
                    lDateValue = lXLSheet.Cells(4, 1).Value
                    Dim lDate() As Integer = GetDate(lDateValue)
                    lStartDate = Date2J(lDate)
                    lStartDate -= JulianMinute * pTimeStep
                End If

                If lCount = 1 OrElse lDatafiles.Keys.Count = 1 Then
                    Dim lastRow As Integer = lXLSheet.UsedRange.Rows.Count
                    lDateValue = lXLSheet.Cells(lastRow, 1).Value
                    Dim lDate() As Integer = GetDate(lDateValue)
                    While lDate Is Nothing
                        lastRow -= 1
                        lDateValue = lXLSheet.Cells(lastRow, 1).Value
                        lDate = GetDate(lDateValue)
                    End While
                    lEndDate = Date2J(lDate)
                End If
                'GoTo CLOSEEXCEL
                lXLBook.Close()
                lCount += 1
            Next

            Dim lNewTser As New atcTimeseries(Nothing)
            lNewTser.Dates = New atcTimeseries(Nothing)
            lNewTser.Dates.Values = NewDates(lStartDate, lEndDate, atcTimeUnit.TUMinute, pTimeStep)
            With lNewTser.Attributes
                .SetValue("Constituent", "PREC")
                .SetValue("TSTYPE", "PREC")
                .SetValue("ID", lDatasetID)
                .SetValue("Location", Loc)
                .SetValue("Scenario", "OBSERVED")
                .SetValue("Description", "Haines " & Loc & " 15-min")
                '.SetValue("History 1", "C:\Zhai\Projects\DataUpdate\2015-08-27_FromRand_15-min rain gauge data")
            End With
            lNewTser.numValues = lNewTser.Dates.numValues
            lNewTser.Value(0) = GetNaN()
            lNewTser.SetInterval(atcTimeUnit.TUMinute, pTimeStep)

            lCount = 0
            Dim lTserSearchIndex As Integer = 1
            For Each lKeyBeginYear As Integer In lDatafiles.Keys
                Dim lFile As DataFile = lDatafiles.Item(lKeyBeginYear)
                lXLBook = lXLApp.Workbooks.Open(lFile.Location)
                lXLSheet = lXLBook.Worksheets(1)

                Dim lastRow As Integer = lXLSheet.UsedRange.Rows.Count
                Dim lTime As DateTime
                Dim lQualFlag As String
                Dim lValue As String

                'Find the true last row
                While Not DateTime.TryParse(lXLSheet.Cells(lastRow, 1).Value, lTime)
                    lastRow -= 1
                End While

                Dim lDates(5) As Integer : Dim lXLTime As Double
                Dim lRowSearch As Integer = 1
                Dim lAccumulating As Boolean = False
                Dim lValueD As Double
                Dim lValueIsValid As Boolean
                Dim lQFlagIsValid As Boolean
                Dim lQualFlgInt As Integer
                Dim lDateCount As Integer = lNewTser.Dates.numValues
                Dim lEOFReached As Boolean = False
                For I As Integer = lTserSearchIndex To lDateCount
                    If I Mod 500 = 0 Then
                        Logger.Progress(I, lDateCount)
                    End If
                    Dim lTserTime As Double = lNewTser.Dates.Value(I)
                    With lXLSheet
                        For lRow As Integer = lRowSearch To lastRow
                            If lRow = lastRow Then lEOFReached = True
                            If DateTime.TryParse(lXLSheet.Cells(lRow, 1).Value, lTime) Then
                                lDates(0) = lTime.Year : lDates(1) = lTime.Month : lDates(2) = lTime.Day
                                lDates(3) = lTime.Hour : lDates(4) = lTime.Minute : lDates(5) = lTime.Second
                                lXLTime = Date2J(lDates)
                                If Math.Abs(lTserTime - lXLTime) < 0.0001 Then
                                    'found the matching time
                                    lValue = .Cells(lRow, 2).Value
                                    lQualFlag = .Cells(lRow, 3).Value
                                    If Double.TryParse(lValue, lValueD) Then
                                        lValueIsValid = True
                                    Else
                                        lValueIsValid = False
                                    End If
                                    If Integer.TryParse(lQualFlag, lQualFlgInt) Then
                                        lQFlagIsValid = True
                                    Else
                                        lQFlagIsValid = False
                                    End If

                                    If lQualFlgInt = EVQualALERT.Good1 OrElse _
                                       lQualFlgInt = EVQualALERT.Good2 OrElse _
                                       lQualFlgInt = EVQualALERT.Good3 Then
                                        'lSW.WriteLine(lTserTime & vbTab & lValue)
                                        lNewTser.Value(I) = lValueD
                                    ElseIf lQualFlgInt = EVQualALERT.Miss OrElse lQualFlgInt = EVQualALERT.Miss1 Then
                                        'lSW.WriteLine(lTserTime & vbTab & pMissingValue)
                                        lNewTser.Value(I) = pMissingValue
                                    ElseIf (lQualFlgInt = EVQualALERT.Accum OrElse lQualFlgInt = EVQualALERT.Accum1) AndAlso _
                                        lValueIsValid AndAlso lValueD = 0 Then
                                        lAccumulating = True
                                        'lSW.WriteLine(lTserTime & vbTab & pAccumulValue)
                                        lNewTser.Value(I) = pAccumulValue
                                    ElseIf (lQualFlgInt = EVQualALERT.Accum OrElse lQualFlgInt = EVQualALERT.Accum1) AndAlso _
                                        lValueIsValid AndAlso lValueD > 0 Then
                                        lAccumulating = False
                                        'lSW.WriteLine(lTserTime & vbTab & lValue)
                                        lNewTser.Value(I) = lValueD
                                    ElseIf lQualFlgInt = EVQualALERT.Zero Then
                                        lNewTser.Value(I) = 0.0
                                    ElseIf lAccumulating Then
                                        'lSW.WriteLine(lTserTime & vbTab & pAccumulValue)
                                        lNewTser.Value(I) = pAccumulValue
                                        Logger.Dbg("Station " & Loc & " accumulating without flag on " & lXLSheet.Cells(lRow, 1).Value & ", value=" & lValue)
                                    Else
                                        'when qual flag is not recognizable (no flag, undocumented flag), set data to missing
                                        Dim Idonotthinkso As String = "I donot think so"
                                        lNewTser.Value(I) = pMissingValue
                                        If lQFlagIsValid Then
                                            Logger.Dbg("Station " & Loc & " has new QualFlag= " & lQualFlag & " on " & lXLSheet.Cells(lRow, 1).Value & ", value=" & lValue)
                                        Else
                                            Logger.Dbg("Station " & Loc & " has invalide QualFlag= " & lQualFlag & " on " & lXLSheet.Cells(lRow, 1).Value & ", value=" & lValue)
                                        End If
                                    End If
                                    lRowSearch = lRow + 1 'assuming Excel doesn't have duplicate dates rows
                                    Exit For 'jump out of the Excel row loop
                                ElseIf lXLTime > lTserTime Then
                                    'lSW.WriteLine(lTserTime & vbTab & pMissingValue)
                                    lNewTser.Value(I) = pMissingValue
                                    lRowSearch = lRow
                                    Exit For 'jump out of the Excel row loop
                                ElseIf lXLTime < lTserTime Then
                                    'Do nothing
                                End If

                            End If
                        Next 'Excel row
                        If lEOFReached Then
                            'done with this Excel file
                            lTserSearchIndex = I + 1
                            Exit For 'jump out of Tser date loop to go open the next Excel file
                        End If
                    End With
                Next 'Tser Time

                'GoTo CLOSEEXCEL
                lXLBook.Close()
                lCount += 1
            Next 'datafile
            'lSW.Flush() : lSW.Close()
            Logger.Progress(100, 100)
            'Start reading the files and put them into WDM
            Logger.Dbg("Write Met_Heines input: " & lWDMFilename & ", DatasetID: " & lDatasetID & ", Location: " & Loc)
            Try
                'first delete the existing dataset with the same location
                Dim locExisting As String = lNewTser.Attributes.GetValue("Location")
                Dim lTserExisting As atcTimeseries = Nothing
                For Each lTs As atcTimeseries In lWDMFile.DataSets
                    If locExisting.StartsWith(lTs.Attributes.GetValue("Location")) Then
                        lTserExisting = lTs
                        Exit For
                    End If
                Next
                Dim lIdToUse As Integer = lNewTser.Attributes.GetValue("ID")
                If lTserExisting IsNot Nothing Then
                    lIdToUse = lTserExisting.Attributes.GetValue("ID")
                    lNewTser.Attributes.SetValue("ID", lIdToUse)
                    If Not lWDMFile.RemoveDataset(lTserExisting) Then
                        Logger.Msg("Delete existing dataset (Location=" & locExisting & ") failed.")
                    End If
                End If
                'Add the newly calculated hourly PMET timeseries back into the current WDM, overwrite if already exists.
                If lWDMFile.AddDataset(lNewTser, atcDataSource.EnumExistAction.ExistReplace) Then
                    'Dim lSumAnnualPREC As Double = lNewTser.Attributes.GetValue("SumAnnual")
                    'If lSW IsNot Nothing Then lSW.WriteLine(lThisLocation & " Penman-Monteith PET at DSN " & lID & " in file " & lWDMFile.Specification & ", SumAnnual " & lSumAnnualPET & " inches")
                    'Logger.Dbg("Wrote Penman-Monteith PET to DSN " & lID & " SumAnnual " & lSumAnnualPET)
                Else
                    'If lSW IsNot Nothing Then lSW.WriteLine(lThisLocation & " failed writing Penman-Montheith PET to DSN " & lID & " in file " & lWDMFile.Specification)
                    'Logger.Dbg("**** Problem Writing Penman-Monteith PET to DSN " & lID)
                End If
            Catch lEx As Exception
                Logger.Dbg(lEx.InnerException.Message & vbCrLf & lEx.InnerException.StackTrace)
                Logger.Flush()
            End Try
            lNewTser.Clear()
            lNewTser.Dates.Clear()
            lNewTser = Nothing
            lDatasetID += 1
        Next

        lWDMFile.Clear()
        lWDMFile = Nothing

CLOSEEXCEL:

        lXLApp.Quit()
        System.Runtime.InteropServices.Marshal.ReleaseComObject(lXLSheet)
        System.Runtime.InteropServices.Marshal.ReleaseComObject(lXLBook)
        System.Runtime.InteropServices.Marshal.ReleaseComObject(lXLApp)
        lXLBook = Nothing
        lXLApp = Nothing
        GC.Collect()
        System.Threading.Thread.Sleep(30)
        Debug.Print("Finished Heines Met input file creation.")
    End Sub

    Public Sub GetTempAndPET()
        'Dim lPattern As String = "PRISM_(tm\w{2})_early_4kmD1_19810101_20151209_(\d+\.?\d+)_(-\d+\.?\d+)_Interp_((\w+\s?)+)\.csv"
        Dim lPattern As String = "PRISM_(tm\w{2})_early_4kmD1_(\d{8})_(\d{8})_(\d+\.?\d+)_(-\d+\.?\d+)_Interp_((\w+\s?)+)\.csv"
        Dim lDataDir As String = "C:\Projects\DataUpdate\PRISM\Temperature\"
        Dim lWDMName As String = IO.Path.Combine(lDataDir, "Met_PRISM_new.wdm")
        Dim lWDM As New atcWDM.atcDataSourceWDM()
        If Not lWDM.Open(lWDMName) Then Exit Sub
        Dim lDir As New IO.DirectoryInfo(lDataDir)
        Dim lFileInfos As IO.FileInfo() = lDir.GetFiles("*.csv")
        Dim lFileInfo As IO.FileInfo
        Dim lDataListing As New Generic.Dictionary(Of String, SortedDictionary(Of Integer, DataFile))
        Dim lTmaxFiles As New atcCollection()
        Dim lTminFiles As New atcCollection()
        Dim lFileTotalCount As Integer = lFileInfos.Length
        Dim lFileCount As Integer = 1
        For Each lFileInfo In lFileInfos
            Dim lFilename As String = IO.Path.GetFileName(lFileInfo.FullName)
            Dim lMatches As MatchCollection = Nothing
            Dim lFoundMatch As Boolean = True
            Try
                lMatches = Regex.Matches(lFilename, lPattern, RegexOptions.IgnoreCase)
            Catch ex As Exception
                lFoundMatch = False
            End Try
            If Not lFoundMatch Then Continue For
            Dim lStationName As String = lMatches(0).Groups(6).Value
            If Not String.IsNullOrEmpty(lStationName) AndAlso Not String.IsNullOrEmpty(lStationName.Trim()) Then
                Dim ltype As String = lMatches(0).Groups(1).Value
                If ltype = "tmax" Then
                    lTmaxFiles.Add(lStationName, lFilename)
                ElseIf ltype = "tmin" Then
                    lTminFiles.Add(lStationName, lFilename)
                End If
            End If
        Next
        Dim lDefStartDate As Double = Date2J(1981, 1, 1, 0, 0, 0)
        Dim lDefEndDate As Double = Date2J(2015, 12, 9, 24, 0, 0)
        Dim lStartDate As Double = -1
        Dim lEndDate As Double = -1
        Dim lID As Integer = lWDM.DataSets.Count + 1 '1
        Dim lMetCmpSrc As New atcMetCmp.atcMetCmpPlugin()
        Dim lCTS() As Double = {0.0055, 0.0055, 0.0055, 0.0055, 0.0055, 0.0055, 0.0055, 0.0055, 0.0055, 0.0055, 0.0055, 0.0055, 0.0055}
        For Each lStnName As String In lTmaxFiles.Keys
            Logger.Progress("Process Station: " & lStnName, lID, lTmaxFiles.Count * 3)
            Dim lTmaxFName As String = lTmaxFiles.ItemByKey(lStnName)
            Dim lTminFName As String = lTminFiles.ItemByKey(lStnName)
            If String.IsNullOrEmpty(lTminFName) Then Continue For

            'Construct a new timeseries
            Dim lTsMax As atcTimeseries = ReadPRISMTemp(IO.Path.Combine(lDataDir, lTmaxFName), lDefStartDate, lDefEndDate, lID)
            lTsMax.Attributes.SetValue("Location", lStnName)
            lID += 1
            Dim lTsMin As atcTimeseries = ReadPRISMTemp(IO.Path.Combine(lDataDir, lTminFName), lDefStartDate, lDefEndDate, lID)
            lTsMin.Attributes.SetValue("Location", lStnName)
            lID += 1
            lWDM.AddDataset(lTsMax)
            lWDM.AddDataset(lTsMin)

            Dim lat As Double = lTsMax.Attributes.GetValue("Latitude")
            Dim lArg As New atcDataAttributes()
            With lArg
                .SetValue("TMIN", lTsMin)
                .SetValue("TMAX", lTsMax)
                .SetValue("Degrees F", True)
                .SetValue("Latitude", lat)
                .SetValue("Hamon Monthly Coefficients", lCTS)
            End With
            If lMetCmpSrc.Open("Hamon PET", lArg) Then
                Dim lTsPET As atcTimeseries = lMetCmpSrc.DataSets(0)
                Dim lTsPETHr As atcTimeseries = atcMetCmp.DisSolPet(lTsPET, Nothing, 2, lat)
                With lTsPETHr.Attributes
                    .SetValue("Location", lStnName)
                    .SetValue("ID", lID)
                    .SetValue("Constituent", "PET")
                    .SetValue("Description", "Hamon PET from PRISM Temp")
                End With
                lWDM.AddDataset(lTsPETHr)
                lID += 1
                lTsPET.Clear() : lTsPET = Nothing
                lTsPETHr.Clear() : lTsPETHr = Nothing
            Else
                Logger.Msg("Problem calculating PET for station: " & lStnName)
            End If
            lTsMax.Clear() : lTsMax = Nothing
            lTsMin.Clear() : lTsMin = Nothing
        Next
        Logger.Progress(lID, lTmaxFiles.Count * 3)
        Logger.Msg("Done reading PRISM temperature and PET generation.")
    End Sub

    Public Function ReadPRISMTemp(ByVal aFName As String, ByVal aStart As Double, ByVal aEnd As Double, ByVal aID As Integer) As atcTimeseries
        Dim lSR As New IO.StreamReader(aFName)
        Dim lHeaderRowCount As Integer = 0
        Do
            If Not lSR.EndOfStream Then
                Dim line As String = lSR.ReadLine()
                If Not line.StartsWith("Date,") Then
                    lHeaderRowCount += 1
                Else
                    Exit Do
                End If
            Else
                Exit Do
            End If
        Loop
        lSR.Close() : lSR = Nothing
        Dim lTable As New atcTableDelimited()
        lTable.NumHeaderRows = lHeaderRowCount
        lTable.NumFields = 2
        If Not lTable.OpenFile(aFName) Then
            Return Nothing
        End If

        Dim lHeaders() As String = lTable.Header.Split(Environment.NewLine)
        Dim lat As Double
        Dim longi As Double
        Dim lelev As Double
        Dim lCons As String = ""
        Dim lStart As Double
        Dim lEnd As Double
        Dim lArr() As String = Nothing
        For Each line As String In lHeaders
            lArr = line.Trim().Split(":")
            If line.Trim().StartsWith("Location") Then
                lat = Double.Parse(lArr(2).Substring(0, lArr(2).LastIndexOf(" ")))
                longi = Double.Parse(lArr(3).Substring(0, lArr(3).LastIndexOf(" ")))
                lelev = Double.Parse(lArr(4).Substring(0, lArr(4).LastIndexOf("f")))
            ElseIf line.Trim().StartsWith("Climate variable") Then
                lCons = lArr(1).Trim()
            ElseIf line.Trim().StartsWith("Period") Then
                Dim lDateStr() As String = lArr(1).Split("-")
                'Dim lPattern0 As String = "1981-01-01 - 2015-12-09"
                Dim lPattern As String = "(\d{4})-(\d{2})-(\d{2}) - (\d{4})-(\d{2})-(\d{2})"
                Dim lMatches As MatchCollection = Nothing
                Dim lFoundMatch As Boolean = True
                Try
                    lMatches = Regex.Matches(lArr(1), lPattern, RegexOptions.IgnoreCase)
                Catch ex As Exception
                    lFoundMatch = False
                End Try
                If lFoundMatch Then
                    Dim lcStartYear As Integer = Integer.Parse(lMatches(0).Groups(1).Value)
                    Dim lcStartMonth As Integer = Integer.Parse(lMatches(0).Groups(2).Value)
                    Dim lcStartDay As Integer = Integer.Parse(lMatches(0).Groups(3).Value)
                    Dim lcEndYear As Integer = Integer.Parse(lMatches(0).Groups(4).Value)
                    Dim lcEndMonth As Integer = Integer.Parse(lMatches(0).Groups(5).Value)
                    Dim lcEndDay As Integer = Integer.Parse(lMatches(0).Groups(6).Value)

                    lStart = Date2J(lcStartYear, lcStartMonth, lcStartDay, 0, 0, 0)
                    lEnd = Date2J(lcEndYear, lcEndMonth, lcEndDay, 24, 0, 0)
                End If
            End If
        Next

        Dim lTs As atcTimeseries = Nothing
        If Math.Abs(aStart - lStart) > 0.001 OrElse Math.Abs(aEnd - lEnd) > 0.001 Then
            lTs = NewTimeseries(lStart, lEnd, atcTimeUnit.TUDay, 1)
        Else
            lTs = NewTimeseries(aStart, aEnd, atcTimeUnit.TUDay, 1)
        End If

        With lTs.Attributes
            .SetValue("ID", aID)
            .SetValue("Constituent", lCons)
            .SetValue("Scenario", "PRISM")
            .SetValue("latitude", lat)
            .SetValue("longitude", longi)
            .SetValue("Elev", lelev)
            .SetValue("Description", "PRISM Daily 4km Interpolated degF")
        End With

        Dim lYear, lMonth, lDay As Integer
        Dim lValue As Double
        lTable.CurrentRecord = 1
        For I As Integer = 1 To lTs.numValues
            With lTable
                .CurrentRecord = I
                lArr = .Value(1).Split("-")
                Try
                    lYear = Integer.Parse(lArr(0))
                    lMonth = Integer.Parse(lArr(1))
                    lDay = Integer.Parse(lArr(2))
                    Dim lDatePrism As Double = Date2J(lYear, lMonth, lDay, 24, 0, 0)
                    If Math.Abs(lDatePrism - lTs.Dates.Value(I)) < 0.001 Then
                        lValue = Double.Parse(.Value(2))
                        lTs.Value(I) = lValue
                    Else
                        Logger.Msg("Found a missing data @date: " & lYear & "-" & lMonth & "-" & lDay)
                    End If
                Catch ex As Exception
                    Logger.Msg("Parsing Data Problem.")
                End Try
            End With
        Next
        lTs.Value(0) = GetNaN()
        lTable.Clear()
        lTable = Nothing
        Return lTs
    End Function

    ''' <summary>
    ''' As new temperature station data are read in and PET generated, they are saved in the Temperature folder
    ''' in the Met_PRISM_new.wdm file
    ''' These newly created datasets needs to be incorporated into the master overall temperature wdm file.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub MergeTempIntoOneWDM()
        'new data
        Dim lWDMnewDir As String = "C:\Projects\DataUpdate\PRISM\Temperature\"
        Dim lWDMnew As String = "Met_PRISM_new.wdm"
        Dim lWDMFilenew As New atcWDM.atcDataSourceWDM()
        If Not lWDMFilenew.Open(IO.Path.Combine(lWDMnewDir, lWDMnew)) Then Exit Sub

        'master datasets
        Dim lWDMDir As String = "C:\Projects\DataUpdate\PRISM\Temperature_Done\"
        Dim lWDM As String = "Met_PRISM.wdm"
        Dim lWDMFile As New atcWDM.atcDataSourceWDM()
        If Not lWDMFile.Open(IO.Path.Combine(lWDMDir, lWDM)) Then Exit Sub

        'append to the end of the list of datasets sequentially
        Dim lID As Integer = lWDMFile.DataSets.Count + 1
        For Each lTserNew As atcTimeseries In lWDMFilenew.DataSets
            Dim loc As String = lTserNew.Attributes.GetValue("Location")
            If Not loc.StartsWith("Tierra") Then Continue For

            lTserNew.EnsureValuesRead()
            lTserNew.Attributes.SetValue("ID", lID)
            If Not lWDMFile.AddDataset(lTserNew) Then
                Logger.Dbg("Failed writing " & lTserNew.Attributes.GetValue("Location") & " into the 1 WDM.")
            End If
            lID += 1
        Next
    End Sub

    Public Sub MergeDatasetsIntoOneWDM(ByVal aMode As Integer)
        Dim lWDMFile2 As New atcWDM.atcDataSourceWDM()
        Dim lWDMFile3 As New atcWDM.atcDataSourceWDM()
        Dim lWDMFileAll As New atcWDM.atcDataSourceWDM()
        If aMode = 1 Then
            'merge original
            Dim lWDMDir As String = "C:\Projects\DataUpdate\Deliver\"
            Dim lWDM2 As String = "Met_Haines2.wdm"
            Dim lWDM3 As String = "Met_Haines3.wdm"
            Dim lWDMAll As String = "Met_Haines.wdm"
            If Not lWDMFile2.Open(IO.Path.Combine(lWDMDir, lWDM2)) Then Exit Sub
            If Not lWDMFile3.Open(IO.Path.Combine(lWDMDir, lWDM3)) Then Exit Sub
            If Not lWDMFileAll.Open(IO.Path.Combine(lWDMDir, lWDMAll)) Then Exit Sub
        ElseIf aMode = 2 Then
            'merge filled
            Dim lWDMFilledDir As String = "C:\Projects\DataUpdate\Deliver\Filled\"
            Dim lWDM2filled As String = "Met_Haines2_Filled.wdm"
            Dim lWDM3filled As String = "Met_Haines3_Filled.wdm"
            Dim lWDMAll As String = "Met_Haines_Filled.wdm"
            If Not lWDMFile2.Open(IO.Path.Combine(lWDMFilledDir, lWDM2filled)) Then Exit Sub
            If Not lWDMFile3.Open(IO.Path.Combine(lWDMFilledDir, lWDM3filled)) Then Exit Sub
            If Not lWDMFileAll.Open(IO.Path.Combine(lWDMFilledDir, lWDMAll)) Then Exit Sub
        End If
        Dim lDatasetID As Integer = 1
        For Each lDataset As atcTimeseries In lWDMFile2.DataSets
            lDataset.EnsureValuesRead()
            lDataset.Attributes.SetValue("ID", lDatasetID)
            If Not lWDMFileAll.AddDataset(lDataset) Then
                Logger.Dbg("Failed writing " & lDataset.Attributes.GetValue("Location") & " into the 1 WDM.")
            End If
            'lDataset.Clear()
            lDatasetID += 1
        Next

        For Each lDataset As atcTimeseries In lWDMFile3.DataSets
            lDataset.EnsureValuesRead()
            lDataset.Attributes.SetValue("ID", lDatasetID)
            If Not lWDMFileAll.AddDataset(lDataset) Then
                Logger.Dbg("Failed writing " & lDataset.Attributes.GetValue("Location") & " into the 1 WDM.")
            End If
            'lDataset.Clear()
            lDatasetID += 1
        Next
        'lWDMFile2.Clear()
        'lWDMFile3.Clear()
        'lWDMFileAll.Clear()
        'lWDMFile2 = Nothing
        'lWDMFile3 = Nothing
        'lWDMFileAll = Nothing
        Logger.Msg("Merge Precip Data Done.")
    End Sub

    Public Sub DataFill_SetPriority()
        Dim lSelectedStations As New atcCollection()
        With lSelectedStations
            '.Add("Bonita")
            '.Add("Descanso")
            '.Add("Encinitas")
            '.Add("Fallbrook AP")
            '.Add("Fashion Valley")
            '.Add("Granite Hills HS")
            '.Add("Kearny Mesa")
            '.Add("Lake Henshaw")
            '.Add("Lake Wohlford")
            '.Add("Oceanside")
            '.Add("Poway")
            '.Add("Ramona")
            '.Add("Morena Lake")
            '.Add("Santa Ysabel")
            .Add("Tierra del Sol")
        End With

        'Dim lXLApp As Microsoft.Office.Interop.Excel.Application = New Microsoft.Office.Interop.Excel.Application()
        'Dim lXLBook As Workbook = Nothing
        'Dim lXLSheet As Worksheet = Nothing

        'lXLBook = lXLApp.Workbooks.Open("C:\Projects\DataUpdate\PriorityTable_11302015.xlsx")
        'lXLSheet = lXLBook.Worksheets(1)
        'lXLBook = lXLApp.Workbooks.Open("C:\Projects\DataUpdate\PriorityTable_01152016.xlsx")
        'lXLBook = lXLApp.Workbooks.Open("C:\Projects\DataUpdate\Book1.xls")


        'To save memory, do these data fill for one station at a time, 
        'hence, we are testing only the first elements in the selected stations
        'If lSelectedStations(0) = "Morena Lake" OrElse lSelectedStations(0) = "Santa Ysabel" OrElse lSelectedStations(0) = "Tierra del Sol" Then
        '    lXLSheet = lXLBook.Sheets("AdditionalStations")
        'Else
        '    lXLSheet = lXLBook.Sheets("PriorityTable_11302015")
        'End If

        Dim lRows As Integer = 1 'lXLSheet.UsedRange.Rows.Count
        Dim lRow As Integer = 1
        Dim lColStaName As Integer = 1
        Dim lColStaPriority As Integer = 4
        Dim lColRank As Integer = 8
        Dim lColMultiplier As Integer = 9
        Dim lColHistParamTarget As Integer = 2
        Dim lColHistParamNeighbor As Integer = 5

        Dim lStaName As String = ""
        Dim lStaNeighbor As String = ""
        Dim lRank As Integer = 0
        Dim lMultiplier As Double = 1.0
        Dim lHistParamTarget As Double = -999
        Dim lHistParamNeighbor As Double = -999

        Dim lRankTable As New atcTableDelimited()
        lRankTable.Delimiter = ","
        Dim lRankTableFilename As String = "C:\Projects\DataUpdate\PriorityTable_03152016_xLC.csv"
        If Not lRankTable.OpenFile(lRankTableFilename) Then Exit Sub

        Logger.Status("Setup Neighboring Gage Network...")
        Dim lTargetStations As New atcCollection()
        'For lRow As Integer = 2 To lRows
        'Logger.Progress(lRow, lRows)
        With lRankTable 'lXLSheet
            .CurrentRecord = 1
            While Not .EOF
                lStaName = .Value(lColStaName)
                If Not lSelectedStations.Contains(lStaName) Then
                    .CurrentRecord += 1
                    Continue While
                End If
                lStaNeighbor = .Value(lColStaPriority)
                lRank = Integer.Parse(.Value(lColRank))
                lMultiplier = Double.Parse(.Value(lColMultiplier))
                lHistParamTarget = Double.Parse(.Value(lColHistParamTarget))
                lHistParamNeighbor = Double.Parse(.Value(lColHistParamNeighbor))
                Dim lTargetStation As Gage = lTargetStations.ItemByKey(lStaName)
                If lTargetStation Is Nothing Then
                    lTargetStation = New Gage()
                    lTargetStation.Name = lStaName
                    lTargetStation.DataPrecipHistParamValue = lHistParamTarget
                    lTargetStation.Neighbors = New SortedList(Of Integer, Gage)
                    lTargetStations.Add(lStaName, lTargetStation)
                End If
                If Not lTargetStation.Neighbors.ContainsKey(lRank) Then
                    Dim lnewNeighborGage As New Gage()
                    lnewNeighborGage.Name = lStaNeighbor
                    lnewNeighborGage.RankPrecip = lRank
                    lnewNeighborGage.DataPrecipMultiplier = lMultiplier
                    lnewNeighborGage.DataPrecipHistParamValue = lHistParamNeighbor
                    lTargetStation.Neighbors.Add(lRank, lnewNeighborGage)
                End If
                .CurrentRecord += 1
            End While
            .Clear()
        End With
        lRankTable = Nothing

        'Next
        'lXLBook.Close()
        'lXLApp.Quit()
        'System.Runtime.InteropServices.Marshal.ReleaseComObject(lXLSheet)
        'System.Runtime.InteropServices.Marshal.ReleaseComObject(lXLBook)
        'System.Runtime.InteropServices.Marshal.ReleaseComObject(lXLApp)
        'GC.Collect()

        Logger.Status("Locate Rainfall Data for gaging stations...")
        'Dim lWDMDir As String = "C:\Projects\DataUpdate\Deliver\"
        'Dim lWDMFilledDir As String = "C:\Projects\DataUpdate\Deliver\Filled\"
        'Dim lWDMRawFilename As String = "Met_Haines.wdm"
        'Dim lWDMFilledFilename As String = "Met_Haines_Filled.wdm"

        Dim lWDMDir As String = "C:\Projects\DataUpdate\Deliver\Met_Haines_15min_Precip\"
        'Dim lWDMFilledDir As String = "C:\Projects\DataUpdate\Deliver\Met_Haines_15min_Precip\Filled\"
        Dim lWDMRawFilename As String = "Met_Haines_Adj4RndBfc.wdm"
        Dim lWDMFilledFilename As String = "Met_Haines_Adj4RndBfc_Fd.wdm"

        'Dim lWDM2 As String = "Met_Haines2.wdm"
        'Dim lWDM3 As String = "Met_Haines3.wdm"
        'Dim lWDM2filled As String = "Met_Haines2_Filled.wdm"
        'Dim lWDM3filled As String = "Met_Haines3_Filled.wdm"
        'Dim lWDMFile2 As atcWDM.atcDataSourceWDM = New atcWDM.atcDataSourceWDM()
        'If Not lWDMFile2.Open(IO.Path.Combine(lWDMDir, lWDM2)) Then Exit Sub
        'Dim lWDMFile3 As atcWDM.atcDataSourceWDM = New atcWDM.atcDataSourceWDM()
        'If Not lWDMFile3.Open(IO.Path.Combine(lWDMDir, lWDM3)) Then Exit Sub

        Dim lWDMRawFile As atcWDM.atcDataSourceWDM = New atcWDM.atcDataSourceWDM()
        If Not lWDMRawFile.Open(IO.Path.Combine(lWDMDir, lWDMRawFilename)) Then Exit Sub
        Dim lWDMFilledFile As atcWDM.atcDataSourceWDM = New atcWDM.atcDataSourceWDM()
        If Not lWDMFilledFile.Open(IO.Path.Combine(lWDMDir, lWDMFilledFilename)) Then Exit Sub

        For Each lGageTarget As Gage In lTargetStations
            If SetDataSpec(lGageTarget, lWDMRawFile, Nothing) Then
                lGageTarget.DataPrecipWDMFilled = IO.Path.Combine(lWDMDir, lWDMFilledFilename)
                'If lGageTarget.DataPrecipWDM.Contains("2") Then
                '    lGageTarget.DataPrecipWDMFilled = IO.Path.Combine(lWDMFilledDir, lWDM2filled)
                'Else
                '    lGageTarget.DataPrecipWDMFilled = IO.Path.Combine(lWDMFilledDir, lWDM3filled)
                'End If
            End If
            For Each lGageNeighbor As Gage In lGageTarget.Neighbors.Values
                SetDataSpec(lGageNeighbor, lWDMRawFile, Nothing)
            Next
        Next

        DataFill_Fill(lTargetStations)
        Logger.Status("HIDE")
    End Sub

    Private Function SetDataSpec(ByRef aGage As Gage, ByVal aWDM1 As atcWDM.atcDataSourceWDM, ByVal aWDM2 As atcWDM.atcDataSourceWDM)
        If aWDM1 Is Nothing And aWDM2 Is Nothing Then
            Throw New Exception("Cannot set data source to null WDM files.")
            Return False
        End If
        Dim gageLoc As String = aGage.Name
        Dim lFound As Boolean = False
        If aWDM1 IsNot Nothing Then
            For Each lTs As atcTimeseries In aWDM1.DataSets
                Dim loc As String = lTs.Attributes.GetValue("Location")
                If gageLoc.ToLower.StartsWith(loc.ToLower) Then
                    aGage.DataPrecipWDM = aWDM1.Specification
                    aGage.DataPrecipDatasetID = lTs.Attributes.GetValue("ID")
                    lFound = True
                    Exit For
                End If
            Next
        End If

        If aWDM2 IsNot Nothing Then
            If Not lFound Then
                For Each lTs As atcTimeseries In aWDM2.DataSets
                    Dim loc As String = lTs.Attributes.GetValue("Location")
                    If gageLoc.ToLower.StartsWith(loc.ToLower) Then
                        aGage.DataPrecipWDM = aWDM2.Specification
                        aGage.DataPrecipDatasetID = lTs.Attributes.GetValue("ID")
                        lFound = True
                        Exit For
                    End If
                Next
            End If
        End If

        Return lFound
    End Function

    Private Sub DataFill_Fill(ByVal aTargetStations As atcCollection)
        Logger.Dbg("TongZhai: ESA Fill Data Starts")
        Logger.Status("Filling Station Precipitation Data...")

        Dim lMVal As Double = -999.0
        Dim lMAcc As Double = -998.0
        Dim lFSpecial As Double = -997.0 '24-hour redistribution flag, this could be the bad value in the missing summary
        Dim lFMin As Double = -100.0
        Dim lFMax As Double = 10000.0
        Dim lRepType As Integer = 1 'DBF parsing output format

        Dim pMaxNearStas As Integer = 27
        Dim pMaxFillLength As Integer = 11 * 4 'any span < max time shift (10 hrs for HI)
        'Dim pMinNumHrly As Integer = 43830 '5 years of hourly values
        'Dim pMinNumDly As Integer = 1830 '5 years of daily
        Dim pMinNum As Integer = 1830 * 24 * 4 '5 years of daily
        Dim pMaxPctMiss As Integer = 50 '20

        Dim lStationCount As Integer = 1
        For Each lGage As Gage In aTargetStations
            Logger.Progress("Fill Station " & lGage.Name, lStationCount, aTargetStations.Count)
            Dim lTargetTser As atcTimeseries = lGage.GetData("PREC")
            If lTargetTser Is Nothing Then
                Continue For
            Else
                lTargetTser.Attributes.SetValue(lGage.DataPrecipHistParam, lGage.DataPrecipHistParamValue)
            End If
            Dim lAddMe As Boolean = False
            Dim lStr As String = MissingDataSummary(lTargetTser, lMVal, lMAcc, lFMin, lFMax, lRepType)
            Dim lPctMiss As Double = CDbl(lStr.Substring(lStr.LastIndexOf(",") + 1))
            If Not (lPctMiss > 0) Then Continue For
            If lPctMiss < pMaxPctMiss Then '% missing OK
                If lTargetTser.numValues > pMinNum Then
                    Logger.Dbg("Filling data for " & lGage.Name)
                    lAddMe = True
                Else
                    Logger.Dbg("Not enough values (" & lTargetTser.numValues & ") for " & lGage.Name & " - need at least " & pMinNum)
                End If
            Else
                Logger.Dbg("For " & lGage.Name & ", percent Missing (" & lPctMiss & ") too large (> " & pMaxPctMiss & ")")
            End If
            If Not lAddMe Then Continue For

            'Actual filling
            Dim lFillers As New atcCollection()
            Dim lNeighborStnCount As Integer = 0
            For Each i As Integer In lGage.Neighbors.Keys
                Dim lNGage As Gage = lGage.Neighbors.Item(i)
                Dim lNTser As atcTimeseries = lNGage.GetData("PREC")
                If lNTser IsNot Nothing Then
                    lNTser.Attributes.SetValue(lNGage.DataPrecipHistParam, lNGage.DataPrecipHistParamValue)
                    'lNTser.Attributes.SetValue("ModifierAttributeName", lNGage.DataPrecipModifierAttributeName)
                    'lNTser.Attributes.SetValue(lNGage.DataPrecipModifierAttributeName, lNGage.DataPrecipMultiplier)
                    lFillers.Add(lNGage.RankPrecip, lNTser)
                    lNeighborStnCount += 1
                End If
                If lNeighborStnCount = pMaxNearStas Then
                    Exit For
                End If
            Next

            Dim lFilledTS As atcTimeseries = Nothing
            If lFillers.Count > 0 Then
                Logger.Dbg("FillMissing:  Found " & lFillers.Count & " nearby stations for filling")
                Logger.Dbg("FillMissing:  Before Filling, % Missing:  " & lPctMiss)
                'lFilledTS = lGage.GetData("PREC", True) 'lTargetTser.Clone()
                'If lFilledTS IsNot Nothing Then
                '    lFilledTS.Attributes.SetValue(lGage.DataPrecipHistParam, lGage.DataPrecipHistParamValue)
                'End If
                lFilledTS = lTargetTser.Clone()
                If lTargetTser.Attributes.GetValue("TU") = atcTimeUnit.TUHour OrElse
                   lTargetTser.Attributes.GetValue("TU") = atcTimeUnit.TUMinute Then
                    If lPctMiss > 0 Then
                        Dim lArgs As New atcDataAttributes()
                        lArgs.Add("AvailableRanked", True)
                        lArgs.Add("Roundoff", 0.01)
                        lArgs.SetValue("UseTriagDistribution", True)
                        lArgs.SetValue("AdditionalValueTarget", lFSpecial)
                        'lArgs.Add("ModifierAttributeName", CType(lFillers.ItemByIndex(0), atcTimeseries).Attributes.GetValue("ModifierAttributeName", ""))
                        FillHourlyTser(lFilledTS, lFillers, lMVal, lMAcc, 90, lArgs)
                    Else
                        Logger.Dbg("FillMissing:  All Missing periods filled via interpolation")
                    End If
                Else 'daily tser
                    FillDailyTser(lFilledTS, Nothing, lFillers, Nothing, lMVal, lMAcc, 90)
                End If
                lStr = MissingDataSummary(lFilledTS, lMVal, lMAcc, lFMin, lFMax, lRepType)
                lPctMiss = CDbl(lStr.Substring(lStr.LastIndexOf(",") + 1))
                Logger.Dbg("FillMissing:  After Filling, % Missing:  " & lPctMiss)
            Else
                Logger.Dbg("FillMissing:  PROBLEM - Could not find any nearby stations for filling")
            End If

            'write filled data set to new WDM file
            lGage.SaveData(lFilledTS)
            lFilledTS.Clear()
            lTargetTser.Clear()
            For Each lTs As atcTimeseries In lFillers
                lTs.Clear()
            Next
            'If lFilledTS IsNot Nothing Then
            '    If lNewWDMfile.AddDataset(lFilledTS) Then
            '        Logger.Dbg("FillMissing:  Added " & lCons & " dataset to WDM file for station " & lStation)
            '    Else
            '        Logger.Dbg("FillMissing:  PROBLEM adding " & lCons & " dataset to WDM file for station " & lStation)
            '    End If
            '    lFilledTS.Clear()
            'End If
            'lts.ValuesNeedToBeRead = True
            lStationCount += 1
        Next
        Logger.Status("HIDE")
        Logger.Msg("Finished Fill Missing Data.")
    End Sub

    Public Class Gage
        Public Name As String = ""
        Public Latitude As Double = 0.0
        Public Longitude As Double = 0.0
        Public RankPrecip As Integer = 1
        Public DataPrecip As atcTimeseries = Nothing
        Public Neighbors As SortedList(Of Integer, Gage)
        Public DataPrecipWDM As String = ""
        Public DataPrecipDatasetID As Integer = -99
        Public DataPrecipWDMFilled As String = ""
        Public DataPrecipHistParam As String = "PRECIP"
        Public DataPrecipHistParamValue As Double = -999
        Public DataPrecipModifierAttributeName As String = "Multiplier"
        Public DataPrecipMultiplier As Double = 1.0

        Public Function GetData(ByVal aCons As String, Optional ByVal aGetFromFilled As Boolean = False) As atcTimeseries
            If String.IsNullOrEmpty(aCons) Then Return Nothing

            Dim lDataset As atcTimeseries = Nothing
            Dim lWDMFilename As String = DataPrecipWDM
            If aGetFromFilled Then lWDMFilename = DataPrecipWDMFilled
            Select Case aCons
                Case "PREC"
                    If Not String.IsNullOrEmpty(lWDMFilename) AndAlso DataPrecipDatasetID > 0 Then
                        Dim lWDM As atcWDM.atcDataSourceWDM = atcDataManager.DataSourceBySpecification(lWDMFilename)
                        Dim lNeedToReopen As Boolean = False
                        If lWDM Is Nothing OrElse lWDM.DataSets Is Nothing Then
                            lNeedToReopen = True
                        Else
                            With lWDM
                                For I As Integer = 0 To .DataSets.Count - 1
                                    If .DataSets(I).Attributes.GetValue("ID") Is Nothing Then
                                        lWDM.Clear()
                                        atcDataManager.RemoveDataSource(lWDM)
                                        lNeedToReopen = True
                                        Exit For
                                    End If
                                Next
                            End With
                        End If
                        If lNeedToReopen Then
                            lWDM = New atcWDM.atcDataSourceWDM()
                            If lWDM.Open(lWDMFilename) Then
                                atcDataManager.DataSources.Add(lWDM)
                            Else
                                Return Nothing
                            End If
                        End If
                        lDataset = lWDM.DataSets.FindData("ID", DataPrecipDatasetID)(0)
                    End If
            End Select
            Return lDataset
        End Function

        Public Function SaveData(ByVal aTser As atcTimeseries, Optional ByVal aSaveToFilled As Boolean = True) As Boolean
            If aTser Is Nothing Then Return False

            Dim lDataset As atcTimeseries = Nothing
            Dim lWDMFilename As String = DataPrecipWDMFilled
            If Not aSaveToFilled Then lWDMFilename = DataPrecipWDM
            If Not String.IsNullOrEmpty(lWDMFilename) AndAlso DataPrecipDatasetID > 0 Then
                Dim lWDM As atcWDM.atcDataSourceWDM = atcDataManager.DataSourceBySpecification(lWDMFilename)
                If lWDM Is Nothing Then
                    lWDM = New atcWDM.atcDataSourceWDM()
                    If lWDM.Open(lWDMFilename) Then
                        atcDataManager.DataSources.Add(lWDM)
                    Else
                        Return False
                    End If
                End If
                Dim lMsg As String = ""
                If lWDM.AddDataset(aTser) Then
                    If aSaveToFilled Then
                        lMsg = "FillMissing: Done for station: " & Name
                    Else
                        lMsg = "Saved Original Data for station: " & Name
                    End If
                    Logger.Dbg(lMsg)
                    Return True
                Else
                    If aSaveToFilled Then
                        lMsg = "FillMissing: failed for station: " & Name
                    Else
                        lMsg = "Saved Original Data failed for station: " & Name
                    End If
                    Logger.Dbg(lMsg)
                    Return False
                End If
                'aTser.Clear()
            Else
                Return False
            End If
        End Function
    End Class
End Module
