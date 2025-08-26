Option Strict Off
Option Explicit On

Imports atcData
Imports atcUtility
Imports MapWinUtility
Imports System.Collections
Imports System.IO

''' <summary>
''' Reads GrADS Data Server ASCII or NLDAS timeseries.cgi files containing time-series values
''' </summary>
''' <remarks>
''' http://hydro1.sci.gsfc.nasa.gov/daac-bin/access/timeseries.cgi
''' http://www.iges.org/grads/gds/
''' http://disc.sci.gsfc.nasa.gov/additional/faq/hydrology_disc_faq.shtml#GDS_retrieve
''' </remarks>
Public Class atcTimeseriesGDS
    Inherits atcTimeseriesSource

    Private Shared pFilter As String = "NASA NLDAS (*.nldas.txt)|*.nldas.txt|NASA GDS (*.gds)|*.gds|Text Files (*.txt)|*.txt|All Files (*.*)|*.*"
    Private Shared pASC2FirstLine As String = "Metadata of the Time Series file:"
    Private Shared pASC2FirstLine2 As String = "Metadata for Requested Time Series:"
    Private Shared pASC2DataHeader As String = "           Date&Time       Data"
    Private Shared pASC2DataHeader2 As String = "Date&Time               Data"
    Private pErrorDescription As String
    Private pJulianOffset As Double = New Date(1900, 1, 1).Subtract(New Date(1, 1, 1)).TotalDays

    Public Overrides ReadOnly Property Description() As String
        Get
            Return "NASA NLDAS File"
        End Get
    End Property

    Public Overrides ReadOnly Property Name() As String
        Get
            Return "Timeseries::NASA NLDAS"
        End Get
    End Property

    Public Overrides ReadOnly Property Category() As String
        Get
            Return "File"
        End Get
    End Property

    Public Overrides ReadOnly Property CanOpen() As Boolean
        Get
            Return True
        End Get
    End Property

    Public Overrides Function Open(ByVal aFileName As String, _
                          Optional ByVal aAttributes As atcData.atcDataAttributes = Nothing) As Boolean
        Dim lOpened As Boolean = False
        If aFileName Is Nothing OrElse aFileName.Length = 0 OrElse Not FileExists(aFileName) Then
            aFileName = FindFile("Select " & Name & " file to open", , , Filter, True, , 1)
        End If

        If Not FileExists(aFileName) Then
            pErrorDescription = "File '" & aFileName & "' not found"
        Else
            Me.Specification = aFileName

            Try
                Using lInputStream As New FileStream(aFileName, FileMode.Open, FileAccess.Read)
                    Dim lInputBuffer As New BufferedStream(lInputStream)
                    Dim lInputReader As New BinaryReader(lInputBuffer)
                    Dim lFirstLine As String = NextLine(lInputReader)
                    If lFirstLine.StartsWith("ERROR") Then
                        While lFirstLine.StartsWith("ERROR")
                            Logger.Dbg(lFirstLine)
                            lFirstLine = NextLine(lInputReader)
                        End While
                        Return False
                    ElseIf lFirstLine.Equals(pASC2FirstLine) Then
                        lOpened = ReadASC2(lInputReader)
                    ElseIf lFirstLine.Equals(pASC2FirstLine2) Then
                        lOpened = ReadASC2r(lInputReader)
                    Else
                        lOpened = ReadGDS(lFirstLine, lInputReader)
                    End If
                End Using
            Catch e As Exception
                Logger.Dbg("Exception reading '" & aFileName & "': " & e.Message, e.StackTrace)
                Return False
            End Try
        End If
        Return lOpened
    End Function

    Private Function ReadASC2(ByVal lInputReader As BinaryReader) As Boolean
        Dim lNaN As Double = GetNaN()
        Dim lUndef As String = ""
        Dim lBuilder As New atcTimeseriesBuilder(Me)

        Dim lCurLineString As String = NextLine(lInputReader)
        Dim lCurLine() As String

        While lCurLineString <> pASC2DataHeader
            lCurLine = lCurLineString.Split("=")
            If lCurLine.Length = 2 Then
                Dim lAttName As String = lCurLine(0).Trim
                Dim lAttValue As String = lCurLine(1).Trim
                Select Case lAttName
                    Case "time_interval(hour)", "tot_record", "start_lat", "start_lon", "dlat", "dlon"
                        'Skip these
                    Case "param_short_name"
                        lBuilder.Attributes.SetValue("Constituent", lAttValue)
                    Case "param_name"
                        lBuilder.Attributes.SetValue("Description", lAttValue)
                    Case "undef"
                        lUndef = lAttValue
                    Case "unit"
                        lBuilder.Attributes.SetValue("Units", lAttValue)
                    Case "lat"
                        lBuilder.Attributes.SetValue("Latitude", lAttValue)
                    Case "lon"
                        lBuilder.Attributes.SetValue("Longitude", lAttValue)
                    Case Else
                        lBuilder.Attributes.SetValue(lAttName, lAttValue)
                End Select
            End If
            lCurLineString = NextLine(lInputReader)
        End While

        Dim lDate As Date, lValue As Double
        Do
            Try
                lCurLineString = NextLine(lInputReader)
                If Date.TryParse(SafeSubstring(lCurLineString, 0, 20).Trim.Replace("Z", ":00"), lDate) AndAlso
                    Double.TryParse(SafeSubstring(lCurLineString, 21), lValue) Then
                    lBuilder.AddValue(lDate, lValue)
                End If
            Catch ex As EndOfStreamException
                Exit Do
            End Try
        Loop
        Logger.Dbg("Read " & DoubleToString(lBuilder.NumValues) & " values from " & Specification)
        If lBuilder.NumValues > 0 Then
            Dim lTimeseries As atcTimeseries = lBuilder.CreateTimeseries()
            With lTimeseries.Attributes
                .AddHistory("Read from " & Specification)
                .SetValue("Scenario", "NLDAS")
                .SetValue("Location", "X" & .GetValue("grid_x", "").ToString.PadLeft(3, "0"c) _
                                    & "Y" & .GetValue("grid_y", "").ToString.PadLeft(3, "0"c))
            End With
            lTimeseries.SetInterval(atcTimeUnit.TUHour, 1)
            DataSets.Add(lTimeseries)
            Return True
        Else
            Return False
        End If
    End Function

    Private Function ReadASC2r(ByVal lInputReader As BinaryReader) As Boolean
        'pbd 2025 revised format
        Dim lNaN As Double = GetNaN()
        Dim lUndef As String = ""
        Dim lBuilder As New atcTimeseriesBuilder(Me)

        Dim lCurLineString As String = NextLine(lInputReader)
        Dim lCurLine() As String

        While lCurLineString <> pASC2DataHeader2
            lCurLine = lCurLineString.Split("=")
            If lCurLine.Length = 2 Then
                Dim lAttName As String = lCurLine(0).Trim
                Dim lAttValue As String = lCurLine(1).Trim
                Select Case lAttName
                    Case "time_interval(hour)", "tot_record", "start_lat", "start_lon", "dlat", "dlon"
                        'Skip these
                    Case "param_short_name"
                        lBuilder.Attributes.SetValue("Constituent", lAttValue)
                    Case "param_name"
                        lBuilder.Attributes.SetValue("Description", lAttValue)
                    Case "undef"
                        lUndef = lAttValue
                    Case "unit"
                        lBuilder.Attributes.SetValue("Units", lAttValue)
                    Case "lat"
                        lBuilder.Attributes.SetValue("Latitude", lAttValue)
                    Case "lon"
                        lBuilder.Attributes.SetValue("Longitude", lAttValue)
                    Case Else
                        lBuilder.Attributes.SetValue(lAttName, lAttValue)
                End Select
            End If
            lCurLineString = NextLine(lInputReader)
        End While

        Dim lDate As Date, lValue As Double
        Do
            Try
                lCurLineString = NextLine(lInputReader)
                If Date.TryParse(SafeSubstring(lCurLineString, 0, 20).Trim.Replace("Z", ":00"), lDate) AndAlso
                    Double.TryParse(SafeSubstring(lCurLineString, 20), lValue) Then
                    lBuilder.AddValue(lDate, lValue)
                End If
            Catch ex As EndOfStreamException
                Exit Do
            End Try
        Loop
        Logger.Dbg("Read " & DoubleToString(lBuilder.NumValues) & " values from " & Specification)
        If lBuilder.NumValues > 0 Then
            Dim lTimeseries As atcTimeseries = lBuilder.CreateTimeseries()
            With lTimeseries.Attributes
                .AddHistory("Read from " & Specification)
                .SetValue("Scenario", "NLDAS")
                Dim lDegreesPerGridCell As Double = 1 / 8
                Dim lWestmostGridEdge As Double = -125
                Dim lSouthmostGridEdge As Double = 25
                Dim lWestmostGridCenter As Double = lWestmostGridEdge + lDegreesPerGridCell / 2
                Dim lSouthmostGridCenter As Double = lSouthmostGridEdge + lDegreesPerGridCell / 2
                Dim lLongitude As Double = .GetValue("Longitude", 0.0)
                Dim lLatitude As Double = .GetValue("Latitude", 0.0)
                Dim lX As Integer = ((lLongitude - lWestmostGridCenter) / lDegreesPerGridCell)
                Dim lY As Integer = ((lLatitude - lSouthmostGridCenter) / lDegreesPerGridCell)
                .SetValue("Location", "X" & lX.ToString.PadLeft(3, "0"c) _
                                    & "Y" & lY.ToString.PadLeft(3, "0"c))
            End With
            lTimeseries.SetInterval(atcTimeUnit.TUHour, 1)
            DataSets.Add(lTimeseries)
            Return True
        Else
            Return False
        End If
    End Function

    Private Function ReadGDS(ByVal aFirstLine As String, ByVal lInputReader As BinaryReader) As Boolean
        Dim lNaN As Double = GetNaN()
        Dim lBadDates As Boolean = False
        Dim lCurLine() As String
        Dim lHeader() As String = aFirstLine.Split(",")
        Dim lConstituent As String = lHeader(0).Trim
        Dim lBounds() As String = lHeader(1).Trim.Replace("[", "").Split("]")
        Dim lNumTimesteps As Integer = Integer.Parse(lBounds(0))
        Dim lNumY As Integer = Integer.Parse(lBounds(1))
        Dim lNumX As Integer = Integer.Parse(lBounds(2))

        pData.Clear()
        Dim lData(lNumX, lNumY) As atcTimeseries
        Dim lTs As atcTimeseries
        Dim lExpectedLineHeader As String
        Dim lTimeIndex As Integer, lX As Integer, lY As Integer
        For lTimeIndex = 0 To lNumTimesteps - 1
            For lY = 0 To lNumY - 1
                lCurLine = NextLine(lInputReader).Split(",")
                lExpectedLineHeader = "[" & lTimeIndex & "][" & lY & "]"
                If lCurLine(0) = lExpectedLineHeader Then
                    For lX = 1 To lNumX
                        lTs = lData(lX, lY)
                        If lTs Is Nothing Then
                            lTs = New atcTimeseries(Me)
                            lTs.Attributes.SetValue("Constituent", lConstituent)
                            lTs.Attributes.AddHistory("Read from " & Specification)
                            lTs.Attributes.SetValue("TMZONE", 0) 'UTC offset = 0
                            lTs.numValues = lNumTimesteps
                            lTs.Value(0) = lNaN
                            lData(lX, lY) = lTs
                            pData.Add(lTs)
                        End If
                        lTs.Value(lTimeIndex + 1) = Double.Parse(lCurLine(lX).Trim)
                    Next
                    NextLine(lInputReader) 'Skip blank line
                Else
                    Throw New ApplicationException("Error reading '" & String.Join(", ", lCurLine) _
                                                 & "' expected to start with '" & lExpectedLineHeader & "'")
                End If
            Next
        Next

        Do
            lCurLine = NextLine(lInputReader).Split(",")
            Select Case lCurLine(0)
                Case "time"
                    lCurLine = NextLine(lInputReader).Split(",")
                    Dim lDates As New atcTimeseries(Me) 'Common dates for all data in file
                    lDates.numValues = lNumTimesteps
                    Dim lDateIndex As Integer
                    For lDateIndex = 1 To lNumTimesteps
                        lDates.Value(lDateIndex) = Double.Parse(lCurLine(lDateIndex - 1).Trim) - pJulianOffset
                        If lDateIndex > 1 AndAlso lDates.Value(lDateIndex - 1) > lDates.Value(lDateIndex) Then
                            lBadDates = True
                            Exit For
                        End If
                    Next
                    If lBadDates Then
                        Dim lStartDate As Double = 722451.5 - pJulianOffset
                        Logger.Dbg("Error: Bad time, index " & lDateIndex - 1 & " > " & lDateIndex & " (" _
                                   & DoubleToString(lDates.Value(lDateIndex - 1) + pJulianOffset, , , , , 11) & " > " _
                                   & DoubleToString(lDates.Value(lDateIndex) + pJulianOffset, , , , , 11) _
                                   & ") Assuming hourly values starting " & DumpDate(lStartDate))
                        lDates.Value(0) = lStartDate
                        For lDateIndex = 1 To lNumTimesteps
                            lDates.Value(lDateIndex) = lStartDate + JulianHour * lDateIndex
                        Next
                    Else
                        If lNumTimesteps > 1 Then
                            lDates.Value(0) = lDates.Value(1) - (lDates.Value(2) - lDates.Value(1))
                        Else
                            lDates.Value(0) = lNaN
                        End If
                    End If
                    For Each lTs In pData
                        lTs.Dates = lDates
                    Next
                Case "lat"
                    lCurLine = NextLine(lInputReader).Split(",")
                    For lY = 0 To lNumY - 1
                        For lX = 1 To lNumX
                            lData(lX, lY).Attributes.SetValue("Latitude", lCurLine(lY))
                        Next
                    Next
                Case "lon"
                    lCurLine = NextLine(lInputReader).Split(",")
                    For lY = 0 To lNumY - 1
                        For lX = 1 To lNumX
                            lData(lX, lY).Attributes.SetValue("Longitude", lCurLine(lX - 1))
                        Next
                    Next
                    Exit Do
                Case ""
                Case Else
                    Logger.Dbg("Unexpected line in GDS data '" & String.Join(", ", lCurLine))
            End Select
        Loop

        For lY = 0 To lNumY - 1
            For lX = 1 To lNumX
                With lData(lX, lY).Attributes
                    Dim lLocation As String = "Lat=" & .GetValue("Latitude", "") & " Long=" & .GetValue("Longitude", "")
                    .SetValue("STANAM", lLocation)
                    .SetValue("Location", lLocation)
                End With
            Next
        Next

        If lNumTimesteps > 1 AndAlso pData.Count > 0 Then
            Dim lTimeUnit As atcTimeUnit = atcTimeUnit.TUUnknown
            Dim lTimeStep As Integer = 1
            lTs = pData(0)
            CalcTimeUnitStep(lTs.Dates.Value(1), lTs.Dates.Value(2), lTimeUnit, lTimeStep)
            For Each lTs In pData
                lTs.SetInterval(lTimeUnit, lTimeStep)
            Next
        End If

    End Function

    Public Sub New()
        Filter = pFilter
    End Sub
End Class
