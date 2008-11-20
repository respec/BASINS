Imports atcMwGisUtility
Imports atcWASP
Imports atcData
Imports MapWinUtility
Imports atcUtility

Module modWASPFromMW
    Friend Sub BuildListofValidStationNames(ByRef aConstituent As String, _
                                            ByVal aStations As atcCollection)
        aStations.Clear()

        Dim lTotalCount As Integer = 0
        For Each lDataSource As atcDataSource In atcDataManager.DataSources
            lTotalCount += lDataSource.DataSets.Count()
        Next

        Dim lCounter As Integer = 0
        For Each lDataSource As atcDataSource In atcDataManager.DataSources
            For Each lDataSet As atcData.atcTimeseries In lDataSource.DataSets
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
                    Dim lStationDetails As New StationDetails
                    lStationDetails.Name = lLoc
                    lStationDetails.StartJDate = lSJDay
                    lStationDetails.EndJDate = lEJDay
                    lStationDetails.Description = lLoc & ":" & lStanam & " " & lDateString
                    Try
                        aStations.Add(lStationDetails.Description, lStationDetails)
                    Catch
                        Logger.Dbg("Problem adding " & lStationDetails.Description)
                    End Try
                End If
                'set valuesneedtoberead so that the dates and values will be forgotten, to free up memory
                lDataSet.ValuesNeedToBeRead = True
            Next
            lDataSource = Nothing
        Next

        Logger.Dbg("Found " & aStations.Count & " Stations")
    End Sub

    Friend Class StationDetails
        Public Name As String
        Public StartJDate As Double
        Public EndJDate As Double
        Public Description As String
    End Class
End Module
