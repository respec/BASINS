
Imports atcData
Imports atcUtility
Imports MapWinUtility
Imports System.Xml

''' <summary>
''' 
''' </summary>
''' <remarks>
''' Copyright 2014 AQUA TERRA Consultants - Royalty-free use permitted under open source license
''' Formats documented at:
''' http://gsshawiki.com/File_Formats:Time_and_Elevation_Series_Files
''' http://gsshawiki.com/Precipitation:Spatially_and_Temporally_Varied_Precipitation
''' </remarks>
Public Class atcTimeseriesGSSHA
    Inherits atcData.atcTimeseriesSource

    Private pFilter As String = "GSSHA Timeseries Files (*.ts)|*.ts|All Files|*.*"
    Private pName As String = "Timeseries::GSSHA"
    Private Shared pNaN As Double = GetNaN()

    Private Const pUnknownUnits As String = "<unknown>"
    Private Const TimeseriesStart As String = "GSSHA_TS"
    Private Const TimeseriesEnd As String = "END_TS"
    Private Const TimeseriesAbsolute As String = "ABSOLUTE"
    Private Const TimeseriesRelative As String = "RELATIVE"
    Private Const EventStart As String = "EVENT"

    Public ReadOnly Property AvailableAttributes() As Collection
        Get
            Return New Collection
        End Get
    End Property

    Public Overrides ReadOnly Property Category() As String
        Get
            Return "File"
        End Get
    End Property

    Public Overrides ReadOnly Property Description() As String
        Get
            Return "GSSHA Timeseries"
        End Get
    End Property

    Public WriteOnly Property HelpFilename() As String
        Set(ByVal newValue As String)
            'TODO:how do we handle helpfiles?
            'App.HelpFile = newvalue
        End Set
    End Property

    Public ReadOnly Property Label() As String
        Get
            Return "GSSHA"
        End Get
    End Property

    Public Overrides Function Open(ByVal aSpecification As String, Optional ByVal aAttributes As atcData.atcDataAttributes = Nothing) As Boolean
        If MyBase.Open(aSpecification, aAttributes) Then
            Dim lInCurveLabels As Boolean = False
            Dim lLabelStart As Integer = 0
            Dim lLabelEnd As Integer = 0
            Dim lDescStart As Integer = 0
            Dim lTRANStart As Integer = 0
            'Dim lTRANCODStart As Integer = 0
            Dim lArea As Double = 0
            Dim lSpaceSeparator(0) As Char
            lSpaceSeparator(0) = " "c
            Logger.Dbg("Opening atcTimeseriesGSSHA file: " & Specification)
            Dim lGroupBuilder As New atcTimeseriesGroupBuilder(Me)
            Dim lSkipBlank As Integer = 0
            Dim lFirstLine As Boolean = True
            Dim lIsTimeseriesFile As Boolean = False
            Dim lIsEventFile As Boolean = False
            Dim lTsB As atcTimeseriesBuilder = Nothing
            For Each lLine As String In LinesInFile(Specification)
                If lFirstLine Then
                    If lLine.StartsWith(atcTimeseriesGSSHA.TimeseriesStart) Then
                        lIsTimeseriesFile = True
                        lTsB = Nothing
                    ElseIf lLine.StartsWith(atcTimeseriesGSSHA.TimeseriesStart) Then
                        lIsEventFile = True
                    Else
                        Throw New ApplicationException("Unable to read GSSHA file as either Event or Time Series: " & Specification)
                    End If
                Else
                    If lIsTimeseriesFile Then
                        Select Case lLine.Trim.ToUpper
                            Case "" 'Skip blank line
                            Case atcTimeseriesGSSHA.TimeseriesAbsolute
                                'Ignore whether ts is absolute or relative
                            Case atcTimeseriesGSSHA.TimeseriesRelative
                                'Relative simply starts at year zero as listed
                            Case atcTimeseriesGSSHA.TimeseriesEnd
                                Me.DataSets.Add(lTsB.CreateTimeseries)
                                lTsB = Nothing
                            Case Else
                                If lTsB Is Nothing Then 'Must be the line naming the timeseries
                                    lTsB = New atcTimeseriesBuilder(Me)
                                    lTsB.Attributes.SetValue("Description", lLine)
                                Else
                                    AddDateAndValueFromTsLine(lLine, lTsB)
                                End If
                        End Select
                    ElseIf lIsEventFile Then
                        'TODO
                    End If
                End If
            Next
            Logger.Dbg("Read " & Me.DataSets.Count & " Timeseries")
            Logger.Status("")

            'Shift all dates one time step, set all timeseries to refer to same Dates
            'Dim lDates As atcTimeseries = Me.DataSets(0).Dates
            'Dim lLastInterval As Double = lDates.Value(lDates.numValues) - lDates.Value(lDates.numValues - 1)

            'For lIndex As Integer = 0 To lDates.numValues - 1
            '    lDates.Value(lIndex) = lDates.Value(lIndex + 1)
            'Next
            'lDates.Value(lDates.numValues) = lDates.Value(lDates.numValues - 1) + lLastInterval
            'lDates.Attributes.DiscardCalculated()

            'For Each lTs As atcTimeseries In Me.DataSets
            '    If lTs.Dates.Serial <> lDates.Serial Then
            '        lTs.Dates.Clear()
            '        lTs.Dates = lDates
            '    End If
            'Next

            Return True
        End If
        Return False
    End Function

    Private Sub AddDateAndValueFromTsLine(ByVal aLine As String, ByVal aTsB As atcTimeseriesBuilder)
        Dim lDate As Date
        Dim lDateDouble As Double
        Dim lValueDouble As Double
        Dim lFieldValues As New Generic.List(Of String)
        Try
            aLine = aLine.Replace(vbTab, " ")
            While aLine.Contains("  ")
                aLine = aLine.Replace("  ", " ")
            End While
            Dim lSplit() As String = aLine.Split(" ")
            lDateDouble = atcUtility.modDate.Date2J(lFieldValues(0), lFieldValues(1), lFieldValues(2), lFieldValues(3), lFieldValues(4), 0)
            lDate = Date.FromOADate(lDateDouble)
            If Not Double.TryParse(lFieldValues(5), lValueDouble) Then
                Logger.Dbg("Unable to parse value '" & lFieldValues(5) & "'" & vbCrLf & " line: '" & aLine & "'")
                lValueDouble = pNaN
            End If
            aTsB.AddValue(lDate, lValueDouble)
        Catch exParse As Exception
            Logger.Dbg("Unable to find date/value in line: '" & aLine & "' " & vbCrLf & exParse.Message)
        End Try
    End Sub

    Public Function RemoveTimSer(ByVal aTimeseries As atcTimeseries) As Boolean
        Throw New ApplicationException("Unable to Remove Time Series " & aTimeseries.ToString & vbCrLf & "From:" & Specification)
    End Function

    Public Function RewriteTimSer(ByVal aTimeseries As atcTimeseries) As Boolean
        Throw New ApplicationException("Unable to Rewrite Time Series " & aTimeseries.ToString & vbCrLf & "From:" & Specification)
    End Function

    Public Overrides ReadOnly Property Name() As String
        Get
            Return pName
        End Get
    End Property

    Public Overrides ReadOnly Property CanOpen() As Boolean
        Get
            Return True
        End Get
    End Property

    Public Sub New()
        Filter = pFilter
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub
End Class