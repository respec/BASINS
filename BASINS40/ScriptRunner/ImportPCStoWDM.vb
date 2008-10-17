Imports atcUtility
Imports atcData
Imports atcWDM
Imports MapWindow.Interfaces
Imports MapWinUtility
Imports MapWinGeoProc
Imports atcMwGisUtility
Imports System.Collections.Specialized

Module ScriptImportPCStoWDM
    Private pDebug As Boolean = False

    Private Structure SnodasData
        Dim DateObs As Double
        Dim Constituent As String
        Dim Id As Integer
        Dim FileName As String
    End Structure

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
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
                If .OpenFile(lTextFilename) Then

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
                Else
                    Logger.Msg("Unable to open text file: '" & lTextFilename & "'", "PCS to WDM")
                End If
            End With

            Dim lNumTimeseries As Integer = lTSBuilders.Count
            Dim lProgressTimeseries As Integer = 1
            Dim lWdm As New atcDataSourceWDM
            If lWdm.Open(lWdmName) Then
                For Each lDataset As atcDataSet In lTSBuilders.CreateTimeseriesGroup
                    lDataset = FillValues(lDataset, atcTimeUnit.TUMonth, 1, -999)
                    Logger.Progress("Writing " & lProgressTimeseries & " of " & lNumTimeseries, lProgressTimeseries, lNumTimeseries)
                    lProgressTimeseries += 1
                    lDataset.Attributes.SetValue("ID", lDSN)
                    lDSN += 1
                    lWdm.AddDataset(lDataset, atcDataSource.EnumExistAction.ExistRenumber)
                Next
                Logger.Status("Wrote " & lNumTimeseries & " to " & lWdmName, True)
            Else
                Logger.Msg("Unable to open WDM file '" & lWdmName & "' for writing", "PCS to WDM")
            End If
        End If
    End Sub

End Module
