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

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        ChDriveDir("C:\test\R4Whitlock")
        Logger.StartToFile("ScriptImportPCStoWDM.log", , , True)

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
            End With

            'process the data into timeseries
            Dim lDataSource As New atcDataSource

            Dim lTSBuilders As New atcData.atcTimeseriesGroupBuilder(Nothing)
            Dim lTSBuilder As atcData.atcTimeseriesBuilder
            Dim lKey As String
            Dim lValueStr As String
            Dim lValue As Double
            Dim lDate As Double

            Dim lCSV As New atcTableDelimited
            With lCSV
                .Delimiter = ","c
                Logger.Dbg("Opening " & lTextFilename )
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
                            lTSBuilder.Attributes.SetValue("tu", atcTimeUnit.TUMonth)
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

            Dim lUpdateReportSB As New Text.StringBuilder
            lUpdateReportSB.AppendLine("Description" & vbTab & "Constituent" & vbTab & _
                                       "Index" & vbTab & "Month" & vbTab & "Prev" & vbTab & _
                                       "Intrp" & vbTab & "Next" & vbTab & "Mean" & vbTab & "MissCnt")
            Dim lNumTimeseries As Integer = lTSBuilders.Count
            Dim lProgressTimeseries As Integer = 1
            Dim lWdm As New atcDataSourceWDM
            If lWdm.Open(lWdmName) Then
                For Each lDataset As atcTimeseries In lTSBuilders.CreateTimeseriesGroup
                    Dim lNumValuesOrig As Integer = lDataset.numValues
                    lDataset = FillValues(lDataset, atcTimeUnit.TUMonth, 1, GetNaN, GetNaN)
                    Logger.Dbg("Fill " & lDSN & " NumValues " & lNumValuesOrig & " " & lDataset.numValues)
                    If lNumValuesOrig < lDataset.numValues Then 'some were filled, now be smarter
                        Dim lDesc As String = lDataset.Attributes.GetValue("Description")
                        Dim lCons As String = lDataset.Attributes.GetValue("Cons")
                        'options - interpolate, prev value, aver monthly value
                        Dim lSeasons As New atcSeasonsMonth
                        Dim lSeasonalAttributes As New atcDataAttributes
                        Dim lMonthlyAttributes As New atcDataAttributes
                        lSeasonalAttributes.SetValue("Mean", 0) 'fluxes are summed from daily, monthly or annual to annual
                        lSeasons.SetSeasonalAttributes(lDataset, lSeasonalAttributes, lMonthlyAttributes)
                        For lMonthIndex As Integer = 0 To 11
                            With lMonthlyAttributes(lMonthIndex)
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
                                    Do While Double.IsNaN(lNextValue) And lNextValueIndex < lDataset.numValues
                                        lNextValueIndex += 1
                                        lMissingCount += 1
                                        lNextValue = lDataset.Value(lNextValueIndex)
                                    Loop
                                    lUpdateReportSB.AppendLine(lDesc & vbTab & lCons & vbTab & _
                                                               lIndex & vbTab & _
                                                               lMonth & vbTab & _
                                                               lPrevVal & vbTab & _
                                                               DoubleToString(lDatasetFillByInterpolation.Value(lIndex), , "#,###,##0.00") & vbTab & _
                                                               lNextValue & vbTab & _
                                                               DoubleToString(lMonthlyAttributes(lMonth - 1).Value, , "#,###,##0.00") & vbTab & _
                                                               lMissingCount)
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
                    lWdm.AddDataset(lDataset, atcDataSource.EnumExistAction.ExistRenumber)
                Next
                SaveFileString("UpdateReport.txt", lUpdateReportSB.ToString)
                Logger.Status("Wrote " & lNumTimeseries & " to " & lWdmName, True)
            Else
                Logger.Msg("Unable to open WDM file '" & lWdmName & "' for writing", "PCS to WDM")
            End If
        End If
    End Sub

End Module
