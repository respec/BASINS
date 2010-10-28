Option Strict Off
Option Explicit On

Imports atcData
Imports atcUtility
Imports MapWinUtility
Imports System.Collections
Imports System.IO

''' <summary>
''' Reads GrADS Data Server ASCII files containing time-series values
''' </summary>
''' <remarks>
''' http://www.iges.org/grads/gds/
''' http://disc.sci.gsfc.nasa.gov/additional/faq/hydrology_disc_faq.shtml#GDS_retrieve
''' </remarks>
Public Class atcTimeseriesGDS
    Inherits atcTimeseriesSource

    Private Shared pFilter As String = "NASA GDS Files (*.gds)|*.gds|Text Files (*.txt)|*.txt|All Files (*.*)|*.*"
    Private pErrorDescription As String
    Private pJulianOffset As Double = New Date(1900, 1, 1).Subtract(New Date(1, 1, 1)).TotalDays

    Public Overrides ReadOnly Property Description() As String
        Get
            Return "NASA GDS File"
        End Get
    End Property

    Public Overrides ReadOnly Property Name() As String
        Get
            Return "Timeseries::NASA GDS"
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
        If aFileName Is Nothing OrElse aFileName.Length = 0 OrElse Not FileExists(aFileName) Then
            aFileName = FindFile("Select " & Name & " file to open", , , Filter, True, , 1)
        End If

        If Not FileExists(aFileName) Then
            pErrorDescription = "File '" & aFileName & "' not found"
        Else
            Me.Specification = aFileName

            Try
                Dim lNaN As Double = GetNaN()
                Dim lBadDates As Boolean = False

                Using lInputStream As New FileStream(aFileName, FileMode.Open, FileAccess.Read)
                    Dim lInputBuffer As New BufferedStream(lInputStream)
                    Dim lInputReader As New BinaryReader(lInputBuffer)

                    Dim lCurLine() As String
                    Dim lHeader() As String = NextLine(lInputReader).Split(",")
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
                                               & ") Assuming hourly values starting " & DumpDate(lStartDate) & " for " & aFileName)
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
                            lTs.Attributes.SetValue("Time Unit", lTimeUnit)
                            lTs.Attributes.SetValue("Time Step", lTimeStep)
                        Next
                    End If
                End Using
                Return True
            Catch e As Exception
                Logger.Dbg("Exception reading '" & aFileName & "': " & e.Message, e.StackTrace)
                Return False
            End Try
        End If
    End Function

    Public Sub New()
        Filter = pFilter
    End Sub
End Class
