Imports atcMwGisUtility
Imports atcWASP
Imports atcData
Imports MapWinUtility
Imports atcUtility

Module modWASPFromMW
    Friend Sub BuildListofValidStationNames(ByRef aConstituent As String, _
                                            ByVal aStationCandidates As WASPTimeseriesCollection)
        aStationCandidates.Clear()

        For Each lDataSource As atcTimeseriesSource In atcDataManager.DataSources
            BuildListofValidStationNamesFromDataSource(lDataSource, aConstituent, aStationCandidates)
        Next

    End Sub

    Friend Sub BuildListofValidStationNamesFromDataSource(ByVal aDataSource As atcTimeseriesSource, _
                                                          ByRef aConstituent As String, _
                                                          ByVal aStationCandidates As WASPTimeseriesCollection)

        Dim lTotalCount As Integer = aDataSource.DataSets.Count
        Dim lCounter As Integer = 0

        For Each lDataSet As atcData.atcTimeseries In aDataSource.DataSets
            lCounter += 1
            Logger.Progress(lCounter, lTotalCount)

            If lDataSet.Attributes.GetValue("Constituent") = aConstituent Then
                Dim lLoc As String = lDataSet.Attributes.GetValue("Location")
                Dim lStanam As String = lDataSet.Attributes.GetValue("Stanam")
                Dim lDsn As Integer = lDataSet.Attributes.GetValue("Id")
                Dim lSJDay As Double
                Dim lEJDay As Double
                lSJDay = lDataSet.Attributes.GetValue("Start Date", 0)
                lEJDay = lDataSet.Attributes.GetValue("End Date", 0)
                If lSJDay = 0 Then
                    lSJDay = lDataSet.Dates.Value(0)
                End If
                If lEJDay = 0 Then
                    lEJDay = lDataSet.Dates.Value(lDataSet.Dates.numValues)
                End If

                Dim lSdate(6) As Integer
                Dim lEdate(6) As Integer
                J2Date(lSJDay, lSdate)
                J2Date(lEJDay, lEdate)
                Dim lDateString As String = "(" & lSdate(0) & "/" & lSdate(1) & "/" & lSdate(2) & "-" & lEdate(0) & "/" & lEdate(1) & "/" & lEdate(2) & ")"
                Dim lCandidateTimeseries As New WASPTimeseries
                lCandidateTimeseries.Identifier = lLoc & ":" & lStanam
                lCandidateTimeseries.SDate = lSJDay
                lCandidateTimeseries.EDate = lEJDay
                lCandidateTimeseries.Description = lLoc & ":" & lStanam & " " & lDateString
                lCandidateTimeseries.Type = aConstituent
                Try
                    aStationCandidates.Add(lCandidateTimeseries)
                Catch
                    Logger.Dbg("Problem adding " & lCandidateTimeseries.Description)
                End Try
            End If
            'set valuesneedtoberead so that the dates and values will be forgotten, to free up memory
            lDataSet.ValuesNeedToBeRead = True
        Next

        Logger.Dbg("Found " & aStationCandidates.Count & " Stations")
    End Sub

End Module
