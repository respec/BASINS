Imports atcUtility
Imports atcData
Imports atcWDM
Imports atcSeasons
Imports atcMwGisUtility

Imports MapWindow.Interfaces
Imports MapWinUtility
Imports MapWinGeoProc
Imports System.Collections.Specialized

Module ScriptImportPCStoWDM
    Private pDebug As Boolean = False

    Public Function Write(ByVal aWDMfilename As String, ByVal Scenario As String, ByVal StationID As String, ByVal PCode As String, ByVal DateTimes() As DateTime, ByVal Values() As Double) As Boolean
        Try
            Dim lTS As New atcTimeseries(Nothing)
            lTS.Dates = New atcTimeseries(Nothing)
            lTS.numValues = Values.Length
            Dim tu As atcUtility.atcTimeUnit
            With lTS.Attributes
                .SetValue("Scenario", Scenario)
                .SetValue("Description", "Created Using WRDB")
                .SetValue("Location", StationID)
                .SetValue("Constituent", PCode)
                .SetValue("Time Step", 1)
                Dim lDateSpan As TimeSpan = DateTimes(1).Subtract(DateTimes(0))
                If lDateSpan.TotalSeconds = 1 Then
                    tu = atcUtility.atcTimeUnit.TUSecond
                ElseIf lDateSpan.TotalMinutes = 1 Then
                    tu = atcUtility.atcTimeUnit.TUMinute
                ElseIf lDateSpan.TotalHours = 1 Then
                    tu = atcUtility.atcTimeUnit.TUHour
                ElseIf lDateSpan.TotalDays = 1 Then
                    tu = atcUtility.atcTimeUnit.TUDay
                ElseIf lDateSpan.TotalDays = 30 Then
                    tu = atcUtility.atcTimeUnit.TUMonth
                ElseIf lDateSpan.TotalDays = 365 Then
                    tu = atcUtility.atcTimeUnit.TUYear
                ElseIf lDateSpan.TotalDays = 3650 Then
                    tu = atcUtility.atcTimeUnit.TUCentury
                End If
                .SetValue("tu", tu)
                .SetValue("SJDay", DateTimes(0).ToOADate)
                .SetValue("EJDay", DateTimes(lTS.numValues - 1).ToOADate)
            End With
            lTS.Value(0) = GetNaN()
            For i As Integer = 1 To lTS.numValues
                lTS.Dates.Value(i - 1) = DateTimes(i - 1).ToOADate
                lTS.Value(i) = Values(i - 1)
            Next
            lTS.Dates.Value(lTS.numValues) = TimAddJ(lTS.Dates.Value(lTS.numValues - 1), tu, 1, 1)

            Dim Wdm As New atcWDM.atcDataSourceWDM
            If Wdm.Open(aWDMfilename) Then
                For Each ds As atcData.atcDataSet In Wdm.DataSets
                    If ds.Attributes.GetValue("Scenario", "") = Scenario And ds.Attributes.GetValue("Location", "") = StationID And ds.Attributes.GetValue("Constituent", "") = PCode Then
                        Wdm.RemoveDataset(ds)
                        Exit For
                    End If
                Next

                Dim dsn As Integer = 0
                dsn += 1
                lTS.Attributes.SetValue("ID", dsn)
                If Not Wdm.AddDataset(lTS, atcData.atcDataSource.EnumExistAction.ExistRenumber) Then Return False
            Else
                'WarningMsgFmt("Unable to open WDM file '{0}' for writing.", mFilename)
                Return False
            End If
            Return True
        Catch ex As Exception
            'ErrorMsg(, ex)
            Return False
        End Try
    End Function


    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        'Dim dates(5) As DateTime
        'Dim values(5) As Double
        'dates(0) = New Date(2000, 1, 1, 0, 0, 0)
        'For i As Integer = 1 To 5
        '    dates(i) = dates(0).AddDays(i)
        '    values(i) = i
        'Next
        'Write("c:\test\cwtest.wdm", "Scen", "Loc", "Pcod", dates, values)
        'Exit Sub

        'ChDriveDir("C:\test")
        'Logger.StartToFile("ScriptImportPCStoWDM.log", , , True)

        Dim lTextFilename As String = "qryWDMoutput.txt"
        Dim lWdmName As String = "PCS.wdm"
        Dim lDSN As Integer = 1
        Dim lScenario As String = "Observed"
        Dim lConstituentColumn As Integer = 1
        Dim lDescriptionColumn As Integer = 2
        Dim lYearColumn As Integer = 3
        Dim lMonthColumn As Integer = 4
        Dim lDayColumn As Integer = 5
        Dim lValueColumn As Integer = 6
        Dim lLocationColumn As Integer = 8
        Dim lFill As Boolean = False

        Dim lUserParms As New atcCollection
        With lUserParms
            .Add("Text File", lTextFilename)
            .Add("WDM file name", lWdmName)
            .Add("First DSN", lDSN)
            .Add("Scenario", lScenario)
            .Add("Constituent Column", lConstituentColumn)
            .Add("Description Column", lDescriptionColumn)
            .Add("Year Column", lYearColumn)
            .Add("Month Column", lMonthColumn)
            .Add("Day Column", lDayColumn)
            .Add("Value Column", lValueColumn)
            .Add("Location Column", lLocationColumn)
            .Add("Fill Monthly", lFill)
        End With
        Dim lAsk As New frmArgs
        If lAsk.AskUser("Specify full path of text file to import and WDM file to write into", lUserParms) Then
            With lUserParms
                lTextFilename = .ItemByKey("Text File")
                lWdmName = .ItemByKey("WDM file name")
                lDSN = .ItemByKey("First DSN")
                lScenario = .ItemByKey("Scenario")
                lConstituentColumn = .ItemByKey("Constituent Column")
                lDescriptionColumn = .ItemByKey("Description Column")
                lYearColumn = .ItemByKey("Year Column")
                lMonthColumn = .ItemByKey("Month Column")
                lDayColumn = .ItemByKey("Day Column")
                lValueColumn = .ItemByKey("Value Column")
                lLocationColumn = .ItemByKey("Location Column")
                lFill = .ItemByKey("Fill Monthly")
            End With

            'process the data into timeseries
            Dim lDataSource As New atcTimeseriesSource

            Dim lTSBuilders As New atcData.atcTimeseriesGroupBuilder(Nothing)
            Dim lTSBuilder As atcData.atcTimeseriesBuilder
            Dim lKey As String
            Dim lValueStr As String
            Dim lValue As Double
            Dim lDate As Double

            Dim lCSV As New atcTableDelimited
            With lCSV
                .Delimiter = ","c
                Logger.Dbg("Opening " & lTextFilename)
                If .OpenFile(lTextFilename) Then
                    Logger.Dbg("Reading " & lTextFilename & " with " & .NumRecords & " records")
                    For lRecord As Integer = 1 To .NumRecords
                        .CurrentRecord = lRecord
                        lKey = .Value(lConstituentColumn) & ":" & .Value(lLocationColumn) & ":" & .Value(lDescriptionColumn)

                        lTSBuilder = lTSBuilders.Builder(lKey)
                        If lTSBuilder.NumValues = 0 Then 'Set attributes of new builder
                            lTSBuilder.Attributes.SetValue("Scenario", lScenario)
                            lTSBuilder.Attributes.SetValue("Constituent", .Value(lConstituentColumn).Replace("""", ""))
                            lTSBuilder.Attributes.SetValue("Location", .Value(lLocationColumn).Replace("""", ""))
                            lTSBuilder.Attributes.SetValue("Description", .Value(lDescriptionColumn).Replace("""", ""))
                            If lFill Then
                                lTSBuilder.Attributes.SetValue("tu", atcTimeUnit.TUMonth)
                            Else
                                lTSBuilder.Attributes.SetValue("tu", atcTimeUnit.TUUnknown)
                            End If
                            lTSBuilder.Attributes.SetValue("ts", 1)
                        End If

                        Try
                            lDate = Jday(.Value(lYearColumn), .Value(lMonthColumn), .Value(lDayColumn), 24, 0, 0)
                            lValueStr = .Value(lValueColumn)
                            lValue = GetNaN()
                            If Not Double.TryParse(lValueStr, lValue) Then
                                If lValueStr.Length = 0 Then
                                    Logger.Dbg("No value found at date " & DumpDate(lDate) & " for " & lKey & " in " & lTextFilename)
                                Else
                                    Logger.Dbg("Could not parse value '" & lValueStr & "' at date " & DumpDate(lDate) & " for " & lKey & " in " & lTextFilename)
                                End If
                            End If
                        Catch
                            Logger.Dbg("Error adding value at date " & DumpDate(lDate) & " for " & lKey & " in " & lTextFilename)
                        End Try
                        lTSBuilder.AddValue(lDate, lValue)
                    Next lRecord
                    Logger.Dbg("EndOfRead " & lTSBuilders.Count & " TimeseriesCreated")
                Else
                    Logger.Msg("Unable to open text file: '" & lTextFilename & "'", "PCS to WDM")
                End If
            End With

            'Dim lUpdateReportSB As New Text.StringBuilder
            'lUpdateReportSB.AppendLine("Description" & vbTab & "Constituent" & vbTab & _
            '                           "Index" & vbTab & "Month" & vbTab & "Prev" & vbTab & _
            '                           "Intrp" & vbTab & "Next" & vbTab & "Mean" & vbTab & "MissCnt")
            Dim lNumTimeseries As Integer = lTSBuilders.Count
            Dim lProgressTimeseries As Integer = 1
            Dim lWdm As New atcDataSourceWDM
            If lWdm.Open(lWdmName) Then
                For Each lDataset As atcTimeseries In lTSBuilders.CreateTimeseriesGroup
                    Dim lNumValuesOrig As Integer = lDataset.numValues
                    If lFill Then
                        lDataset = FillValues(lDataset, atcTimeUnit.TUMonth, 1, GetNaN, GetNaN)
                        Logger.Dbg("Fill " & lDSN & " NumValues " & lNumValuesOrig & " " & lDataset.numValues)
                    End If
                    If lNumValuesOrig < lDataset.numValues Then 'some were filled, now be smarter
                        Dim lDesc As String = lDataset.Attributes.GetValue("Description")
                        Dim lCons As String = lDataset.Attributes.GetValue("Cons")
                        'options - interpolate, prev value, aver monthly value
                        Dim lSeasons As New atcSeasonsMonth
                        Dim lSeasonalAttributes As New atcDataAttributes
                        Dim lMonthlyAttributes As New atcDataAttributes
                        lSeasonalAttributes.SetValue("Mean", 0) 'fluxes are summed from daily, monthly or annual to annual
                        lSeasons.SetSeasonalAttributes(lDataset, lSeasonalAttributes, lMonthlyAttributes)
                        For Each lAttribute As atcDefinedValue In lMonthlyAttributes
                            With lAttribute
                                Logger.Dbg(lDesc & " " & lCons & " " & .Arguments(1).Value & " " & .Arguments(2).Value & " " & DoubleToString(.Value, , "#,###,##0.00"))
                            End With
                        Next
                        Dim lDatasetFillByInterpolation As atcTimeseries = FillMissingByInterpolation(lDataset)
                        Dim lPrevVal As Double = lDataset.Value(0)
                        Dim lDateI(5) As Integer
                        For lIndex As Integer = 1 To lDataset.numValues
                            Dim lVal As Double = lDataset.Value(lIndex)
                            If Double.IsNaN(lVal) Then 'need to fill
                                Try
                                    J2Date(lDataset.Dates.Values(lIndex), lDateI)
                                    Dim lMonth As Integer = lDateI(1) - 1
                                    If lMonth = 0 Then lMonth = 12
                                    Dim lNextValueIndex As Integer = lIndex + 1
                                    Dim lMissingCount As Integer = 1
                                    Dim lNextValue As Double = Double.NaN
                                    Do While Double.IsNaN(lNextValue) AndAlso lNextValueIndex < lDataset.numValues
                                        lNextValueIndex += 1
                                        lMissingCount += 1
                                        lNextValue = lDataset.Value(lNextValueIndex)
                                    Loop
                                    'lUpdateReportSB.AppendLine(lDesc & vbTab & lCons & vbTab & _
                                    '                           lIndex & vbTab & _
                                    '                           lMonth & vbTab & _
                                    '                           lPrevVal & vbTab & _
                                    '                           DoubleToString(lDatasetFillByInterpolation.Value(lIndex), , "#,###,##0.00") & vbTab & _
                                    '                           lNextValue & vbTab & _
                                    '                           DoubleToString(lMonthlyAttributes(lMonth - 1).Value, , "#,###,##0.00") & vbTab & _
                                    '                           lMissingCount)
                                    lDataset.Value(lIndex) = lDatasetFillByInterpolation.Value(lIndex)
                                Catch lEx As Exception
                                    Logger.Dbg("Fill Problem " & lEx.Message)
                                End Try
                            End If
                            If Not Double.IsNaN(lVal) Then lPrevVal = lVal
                        Next
                    End If
                    Logger.Progress("Writing " & lProgressTimeseries & " of " & lNumTimeseries, lProgressTimeseries, lNumTimeseries)
                    lProgressTimeseries += 1
                    lDataset.Attributes.SetValue("ID", lDSN)
                    lDSN += 1
                    lWdm.AddDataset(lDataset, atcTimeseriesSource.EnumExistAction.ExistRenumber)
                Next
                'SaveFileString("UpdateReport.txt", lUpdateReportSB.ToString)
                Logger.Status("Wrote " & lNumTimeseries & " to " & lWdmName, True)
            Else
                Logger.Msg("Unable to open WDM file '" & lWdmName & "' for writing", "PCS to WDM")
            End If
        End If
    End Sub

End Module
