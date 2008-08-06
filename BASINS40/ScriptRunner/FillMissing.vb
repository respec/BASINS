Imports atcUtility
Imports atcData
Imports atcWDM
Imports MapWindow.Interfaces
Imports MapWinUtility

Module FillMissing
    Private Const pTestPath As String = "D:\Basins\Data\"
    Private Const pBaseName As String = "Upatoi"

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        ChDriveDir(pTestPath)
        'open WDM file
        Dim lWdmFileName As String = IO.Path.Combine(pTestPath, pBaseName & ".wdm")
        Dim lWdmDataSource As New atcDataSourceWDM
        lWdmDataSource.Open(lWdmFileName)

        Dim lFlowTser As atcTimeseries = lWdmDataSource.DataSets.ItemByKey(4)
        Dim lFilledTser As atcTimeseries = FillMissingByInterpolation(lFlowTser, 1)
        Dim lDailyTser As atcTimeseries = lWdmDataSource.DataSets.ItemByKey(10)
        Dim lDisaggregatedDailyTo15 As atcTimeseries = Disaggregate(lDailyTser, lFilledTser.Dates)
        Dim lFilledFromDaily As atcTimeseries = FillMissingFromOther(lFilledTser, lDisaggregatedDailyTo15)

        Dim lDebugging As Boolean = True
        If lDebugging Then
            Dim lDataSets As New atcDataGroup

            lFlowTser.Attributes.SetValue("Description", "1FlowTser")
            lDataSets.Add(lFlowTser)

            lFilledTser.Attributes.SetValue("Description", "2FilledTser")
            lDataSets.Add(lFilledTser)

            lDailyTser.Attributes.SetValue("Description", "3DailyTser")
            lDataSets.Add(lDailyTser)

            lDisaggregatedDailyTo15.Attributes.SetValue("Description", "4DisaggregatedDailyTo15")
            lDataSets.Add(lDisaggregatedDailyTo15)

            lFilledFromDaily.Attributes.SetValue("Description", "5FilledFromDaily")
            lDataSets.Add(lFilledFromDaily)

            atcDataManager.UserSelectDisplay("Show intermediate data", lDataSets)
        Else
            lWdmDataSource.AddDataset(lFilledFromDaily)
        End If
    End Sub

    Public Function Disaggregate(ByVal aOldTSer As atcTimeseries, _
                                 ByVal aNewDates As atcTimeseries) As atcTimeseries
        Dim lNewTSer As New atcTimeseries(Nothing)
        lNewTSer.Dates = aNewDates
        lNewTSer.numValues = aNewDates.numValues
        Dim lOldIndex As Integer = 1
        For lInd As Integer = 1 To lNewTSer.numValues
            If lNewTSer.Dates.Value(lInd) > aOldTSer.Dates.Value(lOldIndex) Then
                lOldIndex += 1
            End If
            lNewTSer.Value(lInd) = aOldTSer.Value(lOldIndex)
        Next
        Return lNewTSer
    End Function

    ''' <summary>
    ''' Fill missing periods in a timeseries using another timeseries
    ''' </summary>
    ''' <param name="aOldTSer">Timeseries containing missing values</param>
    ''' <param name="aFillFrom">Timeseries containing values to replace missing values with</param>
    ''' <returns>atcTimeseries clone of original timeseries containing filled values</returns>
    ''' <remarks></remarks>
    Public Function FillMissingFromOther(ByVal aOldTSer As atcTimeseries, _
                                         ByVal aFillFrom As atcTimeseries) As atcTimeseries

        'calc date offset to account for the fact that the starting dates are different
        '  the 96 represents the number of 15 minute periods in a day
        Dim lDateOffset As Integer = (CInt(aOldTSer.Dates.Values(0) - aFillFrom.Dates.Values(0))) * 96

        Dim lNewTSer As atcTimeseries = aOldTSer.Clone
        For lInd As Integer = 1 To lNewTSer.numValues
            If Double.IsNaN(lNewTSer.Values(lInd)) Then
                'need to fill this value
                lNewTSer.Values(lInd) = aFillFrom.Values(lInd + lDateOffset)
            End If
        Next
        Return lNewTSer
    End Function


End Module
