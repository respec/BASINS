Imports atcUtility
Imports MapWindow.Interfaces
Imports MapWinUtility
Imports System.IO
Imports System.Text

Imports System.Collections.Specialized
Imports atcMetCmp
Imports atcData

Public Enum HySepMethod
    FIXED = 1
    SLIDE = 2
    LOCMIN = 3
End Enum

Public Class clsBaseflow

    Private pDataSource As atcDataSource = Nothing
    Private pDataGroup As atcTimeseriesGroup = Nothing

    Private pTargetTS As atcTimeseries
    Public Property TargetTS() As atcTimeseries
        Get
            Return pTargetTS
        End Get
        Set(ByVal value As atcTimeseries)
            pTargetTS = value
        End Set
    End Property

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

    Private pStations As atcCollection
    Public Property Stations() As atcCollection
        Get
            If pStations Is Nothing Then
                pStations = New atcCollection()
            End If
            Return pStations
        End Get
        Set(ByVal value As atcCollection)
            pStations = value
        End Set
    End Property

    Public Function GetStations() As Integer
        Stations.Clear()
        Dim lSR As New StreamReader(StationInfoFile)
        Dim lOneLine As String
        Dim lCount As Integer = 0
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
    End Function

    Private pStationInfoFile As String = "Station.txt"
    Public Property StationInfoFile() As String
        Get
            If Not File.Exists(pStationInfoFile) Then
                pStationInfoFile = FindFile("Please locate Station.txt", pStationInfoFile, "txt")
            End If
            Return pStationInfoFile
        End Get
        Set(ByVal value As String)
            If File.Exists(value) Then
                pStationInfoFile = value
            Else
                pStationInfoFile = FindFile("Please locate Station.txt", value, "txt")
            End If
        End Set
    End Property

    Public Sub New()
    End Sub

    Public Sub New(ByVal aDataGroup As atcTimeseriesGroup)
        pDataGroup = aDataGroup
        'TargetTS = GetTargetTS()
    End Sub

    Public Sub New(ByVal aDataSource As atcDataSource)
        pDataSource = aDataSource
        If pDataSource.Open(pDataSource.Specification) Then
            pDataGroup = pDataSource.DataSets
        End If
    End Sub

    'if metric unit (UnitFlag=2), then convert drainage area from square mile to square kilometer by a factor of 2.59
    Private pDrainageArea As Double = 0.0
    Public Property DrainageArea() As Double
        Get
            Return pDrainageArea
        End Get
        Set(ByVal value As Double)
            pDrainageArea = value
        End Set
    End Property

    Private pStartDate As Double = 0.0
    Public Property StartDate() As Double
        Get
            Return pStartDate
        End Get
        Set(ByVal value As Double)
            pStartDate = value
        End Set
    End Property

    Private pEndDate As Double = 0.0
    Public Property EndDate() As Double
        Get
            Return pEndDate
        End Get
        Set(ByVal value As Double)
            pEndDate = value
        End Set
    End Property

    'unit flag 1 for English, 2 for metric
    'if metric, sq mi => sq km
    'if metric, cfs => cms
    Private pUnitFlag As Integer
    Public Property UnitFlag() As Integer
        Get
            Return pUnitFlag
        End Get
        Set(ByVal value As Integer)
            pUnitFlag = value
        End Set
    End Property

End Class
