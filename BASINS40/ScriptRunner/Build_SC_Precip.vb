Imports System.Collections.Specialized
Imports MapWindow.Interfaces
Imports MapWinUtility
Imports atcMetCmp
Imports atcWDM
Imports atcData
Imports atcUtility

Module Build_SC_Precip
    Private pDataPath As String = "G:\Projects\Gerry\HSPFMetData"

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        Logger.Dbg("BuildSCPrecip Start")
        ChDriveDir(pDataPath)

        Dim lObsTimes As New List(Of Integer) From {7, 24}
        For Each lObsTime As Integer In lObsTimes
            Dim lDailyTimeseriesBuilder As New atcTimeseriesBuilder(Nothing)
            Dim lObsTimeTimeseriesBuilder As New atcTimeseriesBuilder(Nothing)

            Dim lPrecipTable As New atcTableDelimited
            With lPrecipTable
                .Delimiter = ","
                .OpenFile("Obs_McTier_Prec.csv")
                Logger.Dbg("PrecipTable has " & .NumRecords & " records.")
                Dim lDailySJDate As Double = Date2J(2000, 1, 1, 24)
                Dim lDailyEJDate As Double = Date2J(2009, 12, 31, 24)
                Dim lFirst As Boolean = True
                Dim lDate(5) As Integer
                lDate(3) = 24
                Dim lJDateNow As Double
                While Not (.EOF)
                    Dim lDateString As String = .Value(1).Replace("/", ",")
                    lDate(1) = StrRetRem(lDateString)
                    lDate(2) = StrRetRem(lDateString)
                    lDate(0) = lDateString
                    lJDateNow = Date2J(lDate)
                    If lFirst AndAlso lJDateNow > lDailySJDate Then
                        For lJDate As Double = Math.Floor(lDailySJDate) To lJDateNow - 1 Step 1
                            lDailyTimeseriesBuilder.AddValue(lJDate, 0.0)
                            lObsTimeTimeseriesBuilder.AddValue(lJDate, lObsTime)
                        Next
                        lFirst = False
                    End If
                    lDailyTimeseriesBuilder.AddValue(lJDateNow, .Value(2) / 2.54)
                    lObsTimeTimeseriesBuilder.AddValue(lJDateNow, lObsTime)
                    .MoveNext()
                End While
                If lJDateNow < lDailyEJDate Then
                    For lJDate As Double = lJDateNow + 1 To lDailyEJDate Step 1
                        lDailyTimeseriesBuilder.AddValue(lJDate, 0.0)
                        lObsTimeTimeseriesBuilder.AddValue(lJDate, lObsTime)
                    Next
                End If
            End With

            Dim lNewWdmFileName As String = "McTier_" & lObsTime & ".wdm"
            If IO.File.Exists(lNewWdmFileName) Then
                IO.File.Delete(lNewWdmFileName)
            End If
            IO.File.Copy("SC381939.wdm", lNewWdmFileName)
            Dim lNewWdmFile As New atcDataSourceWDM
            lNewWdmFile.Open(lNewWdmFileName)

            Dim lBaseHourlyTimeseries As atcTimeseries = SubsetByDate(lNewWdmFile.DataSets.FindData("Id", 1).Item(0), _
                                                         Date2J(2000, 1, 1),
                                                         Date2J(2009, 12, 31, 24), Nothing)

            Dim lDailyTimeseries As atcTimeseries = lDailyTimeseriesBuilder.CreateTimeseries
            With lDailyTimeseries
                '.Attributes.SetValue("Stanam", "McTier")
                '.Attributes.SetValue("Location", "McTier")
                .Attributes.SetValue("Stanam", "SC381939")
                .Attributes.SetValue("Location", "SC381939")
                .Attributes.SetValue("Constituent", "PREC")
                .Attributes.SetValue("Scenario", "OBSERVED")
                .Attributes.SetValue("TU", atcTimeUnit.TUDay)
                .Attributes.SetValue("TS", 1)
                .Attributes.SetValue("Id", 3001)
                .Dates.Value(0) = .Dates.Value(1) - 1
            End With
            Dim lResult As Boolean = lNewWdmFile.AddDataset(lDailyTimeseries, atcDataSource.EnumExistAction.ExistReplace)

            Dim lDisaggHourlyTimeseries As atcTimeseries =
                atcMetCmp.DisaggPrecip(lDailyTimeseries,
                                       lNewWdmFile,
                                       New atcTimeseriesGroup(lBaseHourlyTimeseries),
                                       lObsTimeTimeseriesBuilder.CreateTimeseries, 90, "dissagReport_" & lObsTime & ".txt")

            Dim lFinalHourlyTimeseries As atcTimeseries = SubsetByDate(lDisaggHourlyTimeseries, Date2J(2000, 1, 1), Date2J(2009, 12, 31, 24), Nothing)

            Dim lDateHourlyIndex As Integer = 0
            For lDateIndexJ As Double = Date2J(2000, 1, 1, 1) To Date2J(2000, 2, 7, 7) Step (1 / 24)
                lDateHourlyIndex = FindDateAtOrAfter(lBaseHourlyTimeseries.Dates.Values, lDateIndexJ, lDateHourlyIndex)
                lFinalHourlyTimeseries.Value(lDateHourlyIndex) = lBaseHourlyTimeseries.Value(lDateHourlyIndex)
            Next
            For lDateIndexJ As Double = Date2J(2009, 9, 30, 8) To Date2J(2009, 12, 31, 24) Step (1 / 24)
                lDateHourlyIndex = FindDateAtOrAfter(lBaseHourlyTimeseries.Dates.Values, lDateIndexJ, lDateHourlyIndex)
                lFinalHourlyTimeseries.Value(lDateHourlyIndex) = lBaseHourlyTimeseries.Value(lDateHourlyIndex)
            Next

            With lFinalHourlyTimeseries
                '.Attributes.SetValue("Stanam", "McTier")
                '.Attributes.SetValue("Location", "McTier")
                .Attributes.SetValue("Id", 1)
                .Attributes.DiscardCalculated()
                lResult = lNewWdmFile.AddDataset(lFinalHourlyTimeseries, atcDataSource.EnumExistAction.ExistReplace)
                .Attributes.SetValue("Id", 10)
                lResult = lNewWdmFile.AddDataset(lFinalHourlyTimeseries, atcDataSource.EnumExistAction.ExistReplace)
            End With
            lDailyTimeseries = Aggregate(lFinalHourlyTimeseries, modDate.atcTimeUnit.TUDay, 1, modDate.atcTran.TranSumDiv)
            lDailyTimeseries.Attributes.SetValue("Id", 3002)
            lResult = lNewWdmFile.AddDataset(lDailyTimeseries, atcDataSource.EnumExistAction.ExistReplace)
            lNewWdmFile.Clear()
            lNewWdmFile = Nothing
        Next
    End Sub
End Module
