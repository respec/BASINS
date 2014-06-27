
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

    Private pFilter As String = "GSSHA Timeseries Files (*.ts, *.gag)|*.ts;*.gag|All Files|*.*"
    Private pName As String = "Timeseries::GSSHA"
    Private Shared pNaN As Double = GetNaN()

    Private Const pUnknownUnits As String = "<unknown>"
    Private Const TimeseriesStart As String = "GSSHA_TS"
    Private Const TimeseriesEnd As String = "END_TS"
    Private Const TimeseriesAbsolute As String = "ABSOLUTE"
    Private Const TimeseriesRelative As String = "RELATIVE"

    Private Const EventStart As String = "EVENT" 'description string of the event follows in double quotes
    Private Const EventNRGAG As String = "NRGAG" 'number of rainfall gages
    Private Const EventNRPDS As String = "NRPDS" 'number of rainfall data points
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
            If Not IO.File.Exists(Specification) Then
                Return True 'Report success on opening file that does not exist yet to write to
            End If
            Logger.Dbg("Opening atcTimeseriesGSSHA file: " & Specification)
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
        Dim lStatusFilename As String = Specification
        If lStatusFilename.Length > 40 Then
            lStatusFilename = IO.Path.GetFileName(Specification)
        End If
        Logger.Status("Opening '" & lStatusFilename & "'", True)
        Dim lTsGB As atcTimeseriesGroupBuilder = Nothing
        Dim lEventDescription As String = ""
        Dim lEventCount As Integer = 0
        Dim lKeys As New List(Of String)
        Dim lNextTimeseriesID As Integer = 1
        Dim lNRGAG As Integer = 0
        Dim lGridDates As List(Of Double) = Nothing
        Dim lGridFilenames As List(Of String) = Nothing
        For Each lLine As String In LinesInFile(Specification)
            Select Case SafeSubstring(lLine, 0, 5).ToUpper
                Case EventStart
                    If lNRGAG > 0 Then
                        MakeEventTimeseries(lTsGB, lEventDescription)
                    Else
                        If lGridDates IsNot Nothing Then
                            atcAsciiGridArcInfo.ReadTimeseries(lGridDates, lGridFilenames, Me)
                            lGridDates.Clear() : lGridDates = Nothing
                            lGridFilenames.Clear() : lGridFilenames = Nothing
                        End If
                    End If
                    lEventCount += 1
                    lEventDescription = SafeSubstring(lLine, EventStart.Length + 1).Trim("'"c, """"c, " "c, vbTab(0))
                    If lEventDescription.Length = 0 Then lEventDescription = "Event " & lEventCount
                    lKeys.Clear()
                    lNRGAG = 0
                Case EventNRGAG
                    Integer.TryParse(SafeSubstring(lLine, 5).Trim, lNRGAG)
                    'If Integer.TryParse(SafeSubstring(lLine, 5).Trim, lNRGAG) Then
                    '    If lNRGAG < 1 Then
                    '        Logger.Dbg("NRGAG < 0 not supported")
                    '    End If
                    'End If
                Case EventCOORD
                    lKeys.Add(SafeSubstring(lLine, 5).Trim)
                Case EventACCUM, EventGAGES, EventRADAR, EventRATES
                    If lTsGB Is Nothing Then
                        lTsGB = New atcTimeseriesGroupBuilder(Me)
                        If lKeys.Count > 0 Then
                            Dim lBuilderIDs(lKeys.Count - 1) As String
                            Dim lBuilderIndex As Integer = 0
                            For lBuilderIndex = 0 To lBuilderIDs.GetUpperBound(0)
                                lBuilderIDs(lBuilderIndex) = lNextTimeseriesID
                                lNextTimeseriesID += 1
                            Next
                            lTsGB.CreateBuilders(lBuilderIDs)
                            lBuilderIndex = 0
                            For Each lKey As String In lKeys
                                lTsGB.Builder(lBuilderIDs(lBuilderIndex)).Attributes.SetValue("Location", lKey)
                                lBuilderIndex += 1
                            Next
                        End If
                    End If
                    If lNRGAG > 0 Then
                        AddDateAndValuesFromEventLine(lLine, lTsGB)
                    Else
                        Dim lFieldValues() As String = SplitLine(lLine)
                        If lGridDates Is Nothing Then
                            lGridDates = New List(Of Double)
                            lGridFilenames = New List(Of String)
                        End If
                        lGridDates.Add(atcUtility.modDate.Date2J(lFieldValues(1), lFieldValues(2), lFieldValues(3), _
                                                                 lFieldValues(4), lFieldValues(5)))
                        lGridFilenames.Add(AbsolutePath(lFieldValues(6), IO.Path.GetDirectoryName(Specification)))
                    End If
            End Select
        Next
        If lNRGAG > 0 Then
            MakeEventTimeseries(lTsGB, lEventDescription)
        Else
            atcAsciiGridArcInfo.ReadTimeseries(lGridDates, lGridFilenames, Me)
        End If
    End Sub

    Private Sub MakeEventTimeseries(ByRef aTsGB As atcTimeseriesGroupBuilder, aDescription As String)
        If aTsGB IsNot Nothing AndAlso aTsGB.Count > 0 Then
            aTsGB.SetAttributeForAll("Description", aDescription)
            aTsGB.SetAttributeForAll("Scenario", "GSSHA")
            aTsGB.SetAttributeForAll("Constituent", "Precipitation")
            aTsGB.CreateTimeseriesAddToGroup(Me.DataSets)
            aTsGB = Nothing
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
        Try
            Dim lFieldValues() As String = SplitLine(aLine)
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
        Try
            Dim lFieldValues() As String = SplitLine(aLine)
            Dim lSplitBound As Integer = lFieldValues.GetUpperBound(0)
            If lSplitBound > 5 Then
                Dim lDateDouble As Double = atcUtility.modDate.Date2J(lFieldValues(1), lFieldValues(2), lFieldValues(3), _
                                                                      lFieldValues(4), lFieldValues(5))
                Dim lDate As Date = Date.FromOADate(lDateDouble)
                Dim lLastIndex As Integer = Math.Min(lSplitBound - 6, aTsGB.Count - 1)
                Dim lValues(lLastIndex) As Double
                Dim lFoundValue As Boolean = False
                For lValueIndex As Integer = 0 To lLastIndex
                    Dim lValueString As String = lFieldValues(lValueIndex + 6)
                    If Double.TryParse(lValueString, lValues(lValueIndex)) Then
                        lFoundValue = True
                    Else
                        ''non-numeric value may be an ASCII grid file name
                        'Dim lFileName As String = AbsolutePath(lValueString, IO.Path.GetDirectoryName(Specification))
                        'If IO.File.Exists(lFileName) Then
                        '    atcAsciiGridArcInfo.AddTimeStep(lFileName, lDate, aTsGB)
                        'Else
                        Logger.Dbg("Unable to parse value '" & lFieldValues(lValueIndex + 6) & "'" & vbCrLf & " line: '" & aLine & "'")
                        lValues(lValueIndex) = pNaN
                        'End If
                    End If
                Next
                If lFoundValue Then aTsGB.AddValues(lDate, lValues)
            End If
            aTsGB.SetAttributeForAll("Event Type", lFieldValues(0))
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

            If Me.DataSets(0).Attributes.ContainsAttribute("Event Type") Then
                SaveAsEvents(aSaveFileName)
            Else
                SaveAsTimeseries(aSaveFileName)
            End If
            Return True
        Catch e As Exception
            Logger.Msg("Error writing '" & aSaveFileName & "': " & e.ToString, MsgBoxStyle.OkOnly, "Did not write file")
            Return False
        End Try
    End Function

    Public Sub SaveAsTimeseries(aSaveFileName As String)
        'Dim lTimeseries As atcTimeseries = Me.DataSets(0)
        Dim lWriter As New System.IO.StreamWriter(aSaveFileName)

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
                modDate.J2Date(lTimeseries.Dates.Value(lTimeStep) - lInterval, lDateArray)


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

    Public Sub SaveAsEvents(aSaveFileName As String)
        Dim lWriter As New System.IO.StreamWriter(aSaveFileName)

        Dim lInterval As Double = 0
        'Dim lInterval As Double = lTimeseries.Attributes.GetValue("Interval", JulianHour)
        Dim lDatasetsToWrite As atcTimeseriesGroup = Me.DataSets ' New atcTimeseriesGroup(lTimeseries)

        'TODO: figure out which timeseries should be grouped into the same event (matching Description and Dates?)
        'Currently we write a separate event for each timeseries

        'For Each lTimeseries In Me.DataSets
        '    If lTimeseries.Attributes.GetValue("Interval", JulianHour) <> lInterval Then
        '        Logger.Msg("Different interval data cannot be written to same file, skipping " & lTimeseries.ToString & " - " & DoubleToString(lTimeseries.Attributes.GetValue("Interval", JulianHour) * 24) & " hours <> " & DoubleToString(lInterval * 24))
        '    ElseIf lTimeseries.Dates.numValues < lLastTimeStep Then
        '        Logger.Msg("Different number of values cannot be written to same file, skipping " & lTimeseries.ToString & " which contains " & lTimeseries.Dates.numValues & " values instead of " & lLastTimeStep)
        '    Else
        '        lDatasetsToWrite.Add(lTimeseries)
        '    End If
        'Next
        Dim lDelimiter As String = vbTab
        Dim lEventCount As Integer = 0
        For Each lTimeseries As atcTimeseries In lDatasetsToWrite
            lEventCount += 1
            Dim lLastTimeStep As Integer = lTimeseries.Dates.numValues
            'Backdoor way to allow run-time specification of quote character, do not expect to use this
            Dim lQuote As String = lTimeseries.Attributes.GetValue("Quote Character", """")
            Dim lEventDescription As String = lTimeseries.Attributes.GetValue("Description", "Event " & lEventCount).Trim
            If Not lEventDescription.StartsWith(lQuote) Then lEventDescription = lQuote & lEventDescription
            If Not lEventDescription.EndsWith(lQuote) Then lEventDescription &= lQuote
            lWriter.WriteLine(EventStart & lDelimiter & lEventDescription)
            lWriter.WriteLine(EventNRPDS & lDelimiter & lTimeseries.numValues)
            lWriter.WriteLine(EventNRGAG & lDelimiter & "1")
            lWriter.WriteLine(EventCOORD & lDelimiter & lTimeseries.Attributes.GetValue("Location", lTimeseries.Attributes.GetValue("ID", "")))
            Dim lEventType As String = lTimeseries.Attributes.GetValue("Event Type", "GAGES")

            For lTimeStep As Integer = 1 To lLastTimeStep
                Dim lDateArray(5) As Integer
                modDate.J2Date(lTimeseries.Dates.Value(lTimeStep) - lInterval, lDateArray)


                lWriter.Write(lEventType & _
                              lDelimiter & Format(lDateArray(0), "0000") & _
                              lDelimiter & lDateArray(1) & _
                              lDelimiter & lDateArray(2) & _
                              lDelimiter & lDateArray(3) & _
                              lDelimiter & lDateArray(4))

                lWriter.Write(lDelimiter & DoubleToString(lTimeseries.Value(lTimeStep)))
                lWriter.WriteLine()
            Next
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