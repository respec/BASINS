﻿Option Strict Off
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

    Private Shared pFilter As String = "NASA GDS Files (*.gds, *.txt)|*.gds;*.txt|All Files (*.*)|(*.*)"
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
                Dim lCurLine() As String

                Dim lInputStream As New FileStream(aFileName, FileMode.Open, FileAccess.Read)
                Dim lInputBuffer As New BufferedStream(lInputStream)
                Dim lInputReader As New BinaryReader(lInputBuffer)

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
                Dim lTimestep As Integer, lX As Integer, lY As Integer
                For lTimestep = 0 To lNumTimesteps - 1
                    For lY = 0 To lNumY - 1
                        lCurLine = NextLine(lInputReader).Split(",")
                        lExpectedLineHeader = "[" & lTimestep & "][" & lY & "]"
                        If lCurLine(0) = lExpectedLineHeader Then
                            For lX = 1 To lNumX
                                lTs = lData(lX, lY)
                                If lTs Is Nothing Then
                                    lTs = New atcTimeseries(Me)
                                    lTs.Attributes.SetValue("Constituent", lConstituent)
                                    lTs.Attributes.AddHistory("Read from " & Specification)
                                    lTs.numValues = lNumTimesteps
                                    lData(lX, lY) = lTs
                                    pData.Add(lTs)
                                End If
                                lTs.Value(lTimestep + 1) = Double.Parse(lCurLine(lX).Trim)
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
                            For lDateIndex As Integer = 1 To lNumTimesteps
                                lDates.Value(lDateIndex) = Double.Parse(lCurLine(lDateIndex - 1).Trim) - pJulianOffset
                            Next
                            For Each lTs In pData
                                lTs.Dates = lDates
                            Next
                        Case "lat"
                            lCurLine = NextLine(lInputReader).Split(",")
                            For lY = 0 To lNumY - 1
                                For lX = 1 To lNumX
                                    lData(lX, lY).Attributes.SetValue("lat", lCurLine(lY))
                                Next
                            Next
                        Case "lon"
                            lCurLine = NextLine(lInputReader).Split(",")
                            For lY = 0 To lNumY - 1
                                For lX = 1 To lNumX
                                    lData(lX, lY).Attributes.SetValue("lat", lCurLine(lX - 1))
                                Next
                            Next
                            Exit Do
                        Case ""
                        Case Else
                            Logger.Dbg("Unexpected line in GDS data '" & String.Join(", ", lCurLine))
                    End Select
                Loop
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
