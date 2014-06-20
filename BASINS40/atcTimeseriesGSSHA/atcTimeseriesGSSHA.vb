
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

    Private Const EventStart As String = "EVENT" 'description string of the event follows in double quotes
    Private Const EventNRGAG As String = "NRGAG" 'number of rainfall gages
    Private Const EventNRPDS As String = "NRDPS" 'number of rainfall data points
    Private Const EventCOORD As String = "COORD" 'coordinate, easting and northing of gage, one card for each gage (NRGAG), must have an identifying string in double quotations
    Private Const EventGAGES As String = "GAGES" 'rainfall accumulation (mm) over the last time period
    Private Const EventRADAR As String = "RADAR" 'rainfall rate (mm×hr-1) for the last time interval
    Private Const EventRATES As String = "RATES" 'rainfall rate (mm×hr-1) for the next time interval
    Private Const EventACCUM As String = "ACCUM" 'cumulative amount of rainfall up until that time period (mm)

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
            Dim lIsTimeseriesFile As Boolean = False
            Dim lIsEventFile As Boolean = False
            Dim lTsB As atcTimeseriesBuilder = Nothing
            Dim lTsGB As atcTimeseriesGroupBuilder = Nothing
            Dim lFirstLine As String = FirstLineInFile(Specification)

            If lFirstLine.StartsWith(atcTimeseriesGSSHA.TimeseriesStart) Then
                ReadTimeseries()
            ElseIf lFirstLine.StartsWith(atcTimeseriesGSSHA.EventStart) Then
                ReadEvents()
            Else
                'TODO: test for hmt format
                Throw New ApplicationException("Unable to read GSSHA file as either Event or Time Series: " & Specification)
            End If

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

    Private Sub ReadTimeseries()
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
        Dim lIsEventFile As Boolean = False
        Dim lTsB As atcTimeseriesBuilder = Nothing
        For Each lLine As String In LinesInFile(Specification)
            Select Case lLine.Trim.ToUpper
                Case TimeseriesStart
                    If lTsB IsNot Nothing Then
                        Logger.Dbg("Found " & TimeseriesStart & " without preceding " & TimeseriesEnd)
                        Me.DataSets.Add(lTsB.CreateTimeseries)
                        lTsB = Nothing
                    End If
                Case "" 'Skip blank line
                Case TimeseriesAbsolute
                    'Ignore whether ts is absolute or relative
                Case TimeseriesRelative
                    'Relative simply starts at year zero as listed
                Case TimeseriesEnd
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
        Next
    End Sub

    Private Sub ReadEvents()
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
        Dim lTsGB As atcTimeseriesGroupBuilder = Nothing
        Dim lAttributes As New atcDataAttributes
        Dim lEventDescription As String = ""
        Dim lKeys As New Generic.List(Of String)
        For Each lLine As String In LinesInFile(Specification)
            Select Case SafeSubstring(lLine, 0, 5)
                Case EventStart
                    If lTsGB IsNot Nothing Then
                        lTsGB.CreateTimeseriesAddToGroup(Me.DataSets)
                        lTsGB = Nothing
                    End If
                    lEventDescription = lLine.Substring(EventStart.Length + 2)
                    lKeys.Clear()
                Case EventCOORD
                    lKeys.Add(SafeSubstring(lLine, 5).Trim)
                Case EventACCUM, EventGAGES, EventRADAR, EventRATES
                    If lTsGB Is Nothing Then
                        lTsGB = New atcTimeseriesGroupBuilder(Me)
                        If lKeys.Count = 0 Then lKeys.Add("1")
                        lTsGB.CreateBuilders(lKeys.ToArray)
                        lTsGB.SetAttributeForAll("Description", lEventDescription)
                    End If
                    AddDateAndValuesFromEventLine(lLine, lTsGB)
            End Select
        Next
        If lTsGB IsNot Nothing Then
            lTsGB.CreateTimeseriesAddToGroup(Me.DataSets)
            lTsGB = Nothing
        End If
    End Sub

    ''' <summary>
    ''' Split parts of a line delimited by tabs and/or spaces
    ''' </summary>
    Private Function SplitLine(aLine As String) As String()
        aLine = aLine.Replace(vbTab, " ")
        While aLine.Contains("  ")
            aLine = aLine.Replace("  ", " ")
        End While
        Return aLine.Split(" ")
    End Function

    Private Sub AddDateAndValueFromTsLine(ByVal aLine As String, ByVal aTsB As atcTimeseriesBuilder)
        Dim lDate As Date
        Dim lDateDouble As Double
        Dim lValueDouble As Double
        Dim lFieldValues As New Generic.List(Of String)
        Try
            Dim lSplit() As String = SplitLine(aLine)
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

    Private Sub AddDateAndValuesFromEventLine(ByVal aLine As String, ByVal aTsGB As atcTimeseriesGroupBuilder)
        Dim lDate As Date
        Dim lDateDouble As Double
        Dim lFieldValues As New Generic.List(Of String)
        Try
            Dim lSplit() As String = SplitLine(aLine)
            Dim lSplitBound As Integer = lSplit.GetUpperBound(0)
            If lSplitBound > 5 Then
                lDateDouble = atcUtility.modDate.Date2J(lFieldValues(1), lFieldValues(2), lFieldValues(3), lFieldValues(4), lFieldValues(5))
                lDate = Date.FromOADate(lDateDouble)

                Dim lValues(lSplitBound) As Double
                For lValueIndex As Integer = 0 To lSplitBound
                    If Not Double.TryParse(lFieldValues(lValueIndex + 5), lValues(lValueIndex)) Then
                        Logger.Dbg("Unable to parse value '" & lFieldValues(5) & "'" & vbCrLf & " line: '" & aLine & "'")
                        lValues(lValueIndex) = pNaN
                    End If
                Next
                aTsGB.AddValues(lDate, lValues)
                aTsGB.SetAttributeForAll("Measurement", lFieldValues(0))
            End If
        Catch exParse As Exception
            Logger.Dbg("Unable to find date/value in line: '" & aLine & "' " & vbCrLf & exParse.Message)
        End Try
    End Sub

    'Public Function RemoveTimSer(ByVal aTimeseries As atcTimeseries) As Boolean
    '    Throw New ApplicationException("Unable to Remove Time Series " & aTimeseries.ToString & vbCrLf & "From:" & Specification)
    'End Function

    'Public Function RewriteTimSer(ByVal aTimeseries As atcTimeseries) As Boolean
    '    Throw New ApplicationException("Unable to Rewrite Time Series " & aTimeseries.ToString & vbCrLf & "From:" & Specification)
    'End Function

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

    Public Overrides ReadOnly Property CanSave As Boolean
        Get
            Return True
        End Get
    End Property

    Public Overrides Function Save(aSaveFileName As String, Optional aExistAction As atcData.atcDataSource.EnumExistAction = atcData.atcDataSource.EnumExistAction.ExistReplace) As Boolean
        Try
            Logger.Dbg("Save " & Me.Name & " in " & aSaveFileName)
            If IO.File.Exists(aSaveFileName) Then
                Dim lExtension As String = IO.Path.GetExtension(aSaveFileName)
                Dim lRenamedFilename As String = GetTemporaryFileName(aSaveFileName.Substring(0, aSaveFileName.Length - lExtension.Length), lExtension)
                Select Case aExistAction
                    Case EnumExistAction.ExistAppend
                        Logger.Dbg("Save: File already exists and aExistAction = ExistAppend, not implemented.")
                        Throw New ApplicationException("Append not implemented for " & Me.Name)
                    Case EnumExistAction.ExistAskUser
                        Select Case Logger.MsgCustom("Attempting to save but file already exists: " & vbCrLf & aSaveFileName, "File already exists", _
                                                     "Overwrite", "Do not write", "Save as " & IO.Path.GetFileName(lRenamedFilename))
                            Case "Overwrite"
                                IO.File.Delete(aSaveFileName)
                            Case "Do not write"
                                Return False
                            Case Else
                                aSaveFileName = lRenamedFilename
                        End Select
                    Case EnumExistAction.ExistNoAction
                        Logger.Dbg("Save: File already exists and aExistAction = ExistNoAction, not saving " & aSaveFileName)
                    Case EnumExistAction.ExistReplace
                        Logger.Dbg("Save: File already exists, deleting old " & aSaveFileName)
                        IO.File.Delete(aSaveFileName)
                    Case EnumExistAction.ExistRenumber
                        Logger.Dbg("Save: File already exists and aExistAction = ExistRenumber, saving as " & lRenamedFilename)
                        aSaveFileName = lRenamedFilename
                End Select
            End If
            'TODO: check Description attribute, if it starts with two numbers (northing easting) then save as event
            SaveAsTimeseries(aSaveFileName)
            Return True
        Catch e As Exception
            Logger.Msg("Error writing '" & aSaveFileName & "': " & e.ToString, MsgBoxStyle.OkOnly, "Did not write file")
            Return False
        End Try
    End Function

    Public Sub SaveAsTimeseries(aSaveFileName As String)
        'Dim lTimeseries As atcTimeseries = Me.DataSets(0)
        Dim lWriter As New System.IO.StreamWriter(aSaveFileName)
        lWriter.WriteLine("-----------------------------------------------------------------------------------------")

        Dim lInterval As Double = 0
        'Dim lInterval As Double = lTimeseries.Attributes.GetValue("Interval", JulianHour)
        Dim lDatasetsToWrite As atcTimeseriesGroup = Me.DataSets ' New atcTimeseriesGroup(lTimeseries)
        'For Each lTimeseries In Me.DataSets
        '    If lTimeseries.Attributes.GetValue("Interval", JulianHour) <> lInterval Then
        '        Logger.Msg("Different interval data cannot be written to same file, skipping " & lTimeseries.ToString & " - " & DoubleToString(lTimeseries.Attributes.GetValue("Interval", JulianHour) * 24) & " hours <> " & DoubleToString(lInterval * 24))
        '    ElseIf lTimeseries.Dates.numValues < lLastTimeStep Then
        '        Logger.Msg("Different number of values cannot be written to same file, skipping " & lTimeseries.ToString & " which contains " & lTimeseries.Dates.numValues & " values instead of " & lLastTimeStep)
        '    Else
        '        lDatasetsToWrite.Add(lTimeseries)
        '    End If
        'Next

        Dim lDelimiter As String = " "

        For Each lTimeseries As atcTimeseries In lDatasetsToWrite
            Dim lLastTimeStep As Integer = lTimeseries.Dates.numValues
            lWriter.WriteLine(TimeseriesStart)
            If lTimeseries.Dates.Value(1) > JulianYear Then
                lWriter.WriteLine(TimeseriesAbsolute)
            Else
                lWriter.WriteLine(TimeseriesRelative)
            End If


            For lTimeStep As Integer = 1 To lLastTimeStep
                Dim lDateArray(5) As Integer
                modDate.J2Date(lDatasetsToWrite(0).Dates.Value(lTimeStep) - lInterval, lDateArray)


                lWriter.Write(Format(lDateArray(0), "0000") & _
                              lDelimiter & lDateArray(1) & _
                              lDelimiter & lDateArray(2) & _
                              lDelimiter & lDateArray(3) & _
                              lDelimiter & lDateArray(4))

                lWriter.Write(lDelimiter & DoubleToString(lTimeseries.Value(lTimeStep)))
                lWriter.WriteLine()
            Next
            lWriter.WriteLine(TimeseriesEnd)
        Next
        lWriter.Close()
    End Sub

    Public Sub New()
        Filter = pFilter
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub
End Class