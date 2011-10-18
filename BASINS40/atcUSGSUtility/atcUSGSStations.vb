Imports atcData
Imports atcUtility

Public Class atcUSGSStations
    ''' <summary>
    ''' This is a file (usually called 'Station.txt') that is read by programs PREP, RECESS, RORA, and PART, 
    ''' it contains the drainage area values for each station data file downloaded from NWIS
    ''' Note: This file should have ten header lines.  
    ''' The streamflow file name should be 12 characters or less (for the original fortran program). 
    ''' </summary>
    ''' <remarks></remarks>
    Public Structure USGSGWStation
        Dim Filename As String
        Dim DrainageArea As Double
        Dim ExtraInfo As String
    End Structure

    'Private pMissingDataMonth As atcCollection

    Public Shared pStations As atcCollection
    Public Shared Property Stations() As atcCollection
        Get
            If pStations Is Nothing Then
                pStations = New atcCollection()
            End If
            Return pStations
        End Get
        Set(ByVal value As atcCollection)
            If pStations IsNot Nothing Then
                pStations.Clear()
                pStations = Nothing
            End If
            pStations = value
        End Set
    End Property

    Public Shared pStationInfoFile As String = "Station.txt"
    Public Shared Property StationInfoFile() As String
        Get
            If Not IO.File.Exists(pStationInfoFile) Then
                pStationInfoFile = FindFile("Please locate Station.txt", pStationInfoFile, "txt")
            End If
            Return pStationInfoFile
        End Get
        Set(ByVal value As String)
            If IO.File.Exists(value) Then
                pStationInfoFile = value
            Else
                pStationInfoFile = FindFile("Please locate Station.txt", value, "txt")
            End If
        End Set
    End Property

    Public Shared StationHeaderText As String = "File ""station.txt""" & vbCrLf & _
"This file is read by programs PREP, RECESS, RORA, and PART, to" & vbCrLf & _
"obtain the drainage area.  Note: This file should have ten header" & vbCrLf & _
"lines.  The streamflow file name should be 12 characters or less." & vbCrLf & _
"----------------------------------------------------------------" & vbCrLf & _
"              Drainage" & vbCrLf & _
" Name of       area     The space below, after drainage area, is" & vbCrLf & _
" streamflow   (Square   for optional information that is not read" & vbCrLf & _
" file          miles)   by the programs.  This is free-form." & vbCrLf & _
"---------------------- ------------------------------------------"

    Public Shared Function GetStations() As Integer
        Stations.Clear()
        Dim lSR As New IO.StreamReader(StationInfoFile)

        Dim lOneLine As String
        Dim lCount As Integer = 0
        Try
            While Not lSR.EndOfStream
                'bypass the first 10 header lines
                If lCount = 10 Then Exit While
                lOneLine = lSR.ReadLine()
                lCount += 1
            End While
            Dim lOneStation As USGSGWStation
            While Not lSR.EndOfStream
                lOneLine = lSR.ReadLine()
                If lOneLine.Trim().Length >= 20 Then
                    lOneStation = New USGSGWStation
                    lOneStation.Filename = lOneLine.Substring(0, 12).Trim()
                    lOneStation.DrainageArea = Double.Parse(lOneLine.Substring(12, 8))
                    lOneStation.ExtraInfo = lOneLine.Substring(20).Trim()
                    Stations.Add(lOneStation.Filename, lOneStation)
                End If
            End While
            lSR.Close()
            lSR = Nothing
            Return Stations.Count
        Catch ex As Exception
            Return 0
        End Try
    End Function

    Public Shared Function SaveStations(ByVal aStationList As atcCollection, ByVal aSpecification As String) As Integer
        're-write the stations.txt file
        Dim lSW As New IO.StreamWriter(StationInfoFile, False)
        lSW.WriteLine(StationHeaderText)
        For Each lStation As USGSGWStation In aStationList
            lSW.Write(lStation.Filename.Trim.PadRight(12, " "))
            lSW.Write(String.Format("{0:0.00}", lStation.DrainageArea).PadLeft(8, " "))
            lSW.WriteLine(lStation.ExtraInfo)
        Next
        lSW.Flush()
        lSW.Close()
        lSW = Nothing
    End Function
End Class
