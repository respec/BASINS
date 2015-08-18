Imports atcData
Imports atcUtility
Imports MapWinUtility
Module modUtil
    Public Function BuildStationsInfo(ByVal aDataGroup As atcTimeseriesGroup) As ArrayList
        Dim lStationsInfo As New atcUtility.atcCollection()
        Dim lStationInfo As String = ""
        Dim loc As String
        Dim lDA As String
        Dim lFrom As String
        For Each lTser As atcTimeseries In aDataGroup
            With lTser.Attributes
                loc = .GetValue("Location")
                lDA = .GetValue("Drainage Area", "")
                lFrom = .GetValue("History 1")
                If Not String.IsNullOrEmpty(lFrom) Then
                    lFrom = lFrom.Substring("read from ".Length)
                End If

                lStationInfo = "Station " & loc & "," & lDA & "," & lFrom
                If Not lStationsInfo.Keys.Contains(loc) Then
                    lStationsInfo.Add(loc, lStationInfo)
                End If
            End With
        Next
        Return lStationsInfo
    End Function
End Module
