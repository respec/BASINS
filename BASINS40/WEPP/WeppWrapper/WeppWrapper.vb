Imports atcData
Imports atcUtility
Imports atcWDM
Imports atcSeasons
Imports MapWinUtility
Imports System.String
Imports System.IO

Public Module Main

    Sub Main()
        Try
            'begin set block (set things in here)
            '----------------------------------
            'give this particular run an ID
            Dim lRunId As String = "1"

            'set the WDM file path
            Dim lWDMFilePath As String = "C:\FTB\wdm\FBmet.wdm"

            'set the output file path
            Dim lOutputFilePath As String = "Z:\Documents\filecabinet\employment\aquaterra\active.projects\SERDP\Roads\WEPP\wepp.run\cli.met\" & lRunId & ".cli"

            'set the log file path
            Dim lLogFilePath As String = "Z:\Documents\filecabinet\employment\aquaterra\active.projects\SERDP\Roads\WEPP\wepp.run\cli.met\" & lRunId & "-log.txt"

            'set the DSNs for constituents
            Dim lDsnPREC As Integer = 106
            Dim lDsnATEM As Integer = 13
            Dim lDsnDEWP As Integer = 17
            Dim lDsnWIND As Integer = 14
            Dim lDsnSOLR As Integer = 15
            Dim lDsnPEVT As Integer = 16

            'Set the elevation in meters
            Dim lElevation As String = "150"

            'Set the flag for exporting the raw timeseries data as a comma-separated textfile (not in WEPP format). Next line is path to export to.
            Dim lRawTsFlag As Boolean = False
            Dim lRawTsFilePath As String = "Z:\Documents\filecabinet\employment\aquaterra\active.projects\SERDP\Roads\WEPP\wepp.run\cli.met\" & lRunId & "-RawTs.csv"

            'Set arrays of dates for begin/end of model
            'Important: Data must begin on hour "0" of first day and end on hour "24" of last day
            Dim lStrModelBegin() As Integer = {1999, 10, 1, 0, 0, 0}
            Dim lStrModelEnd() As Integer = {2006, 9, 30, 24, 0, 0}

            'set raw pre int file path
            Dim lPreInterpolatorRawTsFlag As Boolean = False
            Dim lPreInterpolatorRawTsFilePath As String = "Z:\Documents\filecabinet\employment\aquaterra\active.projects\SERDP\Roads\WEPP\wepp.run\cli.met\" & lRunId & "-PreInterpolatorRawTs.csv"
            'end set block
            '----------------------------------

            Logger.StartToFile(lLogFilePath)


            Dim lTimeseriesStatistics As New atcTimeseriesStatistics.atcTimeseriesStatistics
            For Each lOperation As atcDefinedValue In lTimeseriesStatistics.AvailableOperations
                atcDataAttributes.AddDefinition(lOperation.Definition)
            Next

            Dim lWDMDataSource As New atcWDM.atcDataSourceWDM
            If lWDMDataSource.Open(lWDMFilePath) Then    'use the WDM file name

                'export a raw csv timeseries before the subset by date and interpolator (if lPreInterpolatorRawTsFlag is True)
                If lPreInterpolatorRawTsFlag Then
                    Dim TTS As atcTimeseries = lWDMDataSource.DataSets.ItemByKey(lDsnPREC)
                    Dim lPreInterpolatoeRawTS As System.IO.StreamWriter = System.IO.File.CreateText(lPreInterpolatorRawTsFilePath)
                    For i = 1 To TTS.Values.Length - 1

                        Dim lDatePre(5) As Integer
                        J2Date(TTS.Dates.Values(i), lDatePre)
                        Dim lTempStringPre As String = ""
                        For j = 0 To 5
                            lTempStringPre &= lDatePre(j) & ","
                        Next
                        lPreInterpolatoeRawTS.WriteLine(lTempStringPre & TTS.Values(i) * 25.4)
                    Next
                    lPreInterpolatoeRawTS.Flush()
                    lPreInterpolatoeRawTS.Close()
                End If


                'successfully opened the wdm
                Logger.Dbg("Opened " & lWDMDataSource.Name)
                Logger.Dbg("DSNs for this run: " & "PREC=" & lDsnPREC & " ATEM=" & lDsnATEM & " DEWP=" & lDsnDEWP & " WIND=" & lDsnWIND & " SOLR=" & lDsnSOLR & " PEVT=" & lDsnPEVT)

                Dim lJulianDate As Double
                Dim lDate(5) As Integer

                Dim lTempString As String = ""
                For i = 0 To 5
                    lTempString &= lStrModelBegin(i).ToString & " "
                Next

                Logger.Dbg("Model Begin (yyyy mm dd hh mm ss): = " & lTempString)

                lTempString = ""
                For i = 0 To 5
                    lTempString &= lStrModelEnd(i).ToString & " "
                Next

                Logger.Dbg("Model End (yyyy mm dd hh mm ss): = " & lTempString)
                lTempString = ""

                Dim lTableDelimiter As String = " "
                Dim lTableCellWidth As Integer = 12
                Dim lD2SMax As Integer = 10
                Dim lD2SFormat As String = "###.#"
                Dim lD2SSig As Integer = 5

                'make timeseries

                'PREC
                Dim lTSPREC As atcTimeseries = modTimeseriesMath.SubsetByDate(lWDMDataSource.DataSets.ItemByKey(lDsnPREC), Date2J(lStrModelBegin), Date2J(lStrModelEnd), lWDMDataSource)

                'print all attributes in the debug log file for checking
                Dim lSeasons As atcSeasonBase = New atcSeasonsMonth
                Dim lSeasonalAttributes As New atcDataAttributes
                lSeasons.SetSeasonalAttributes(lTSPREC, lSeasonalAttributes, lTSPREC.Attributes)
                DumpAttributes(lTSPREC)

                'ATEM
                Dim lTSATEM As atcTimeseries = modTimeseriesMath.SubsetByDate(lWDMDataSource.DataSets.ItemByKey(lDsnATEM), Date2J(lStrModelBegin), Date2J(lStrModelEnd), lWDMDataSource)
                'DEWP
                Dim lTSDEWP As atcTimeseries = modTimeseriesMath.SubsetByDate(lWDMDataSource.DataSets.ItemByKey(lDsnDEWP), Date2J(lStrModelBegin), Date2J(lStrModelEnd), lWDMDataSource)
                'WIND
                Dim lTSWIND As atcTimeseries = modTimeseriesMath.SubsetByDate(lWDMDataSource.DataSets.ItemByKey(lDsnWIND), Date2J(lStrModelBegin), Date2J(lStrModelEnd), lWDMDataSource)
                'SOLR
                Dim lTSSOLR As atcTimeseries = modTimeseriesMath.SubsetByDate(lWDMDataSource.DataSets.ItemByKey(lDsnSOLR), Date2J(lStrModelBegin), Date2J(lStrModelEnd), lWDMDataSource)
                'PEVT
                Dim lTSPEVT As atcTimeseries = modTimeseriesMath.SubsetByDate(lWDMDataSource.DataSets.ItemByKey(lDsnPEVT), Date2J(lStrModelBegin), Date2J(lStrModelEnd), lWDMDataSource)

                ' Create a file to write WEPP output to
                Dim lWeppOutputStream As System.IO.StreamWriter = System.IO.File.CreateText(lOutputFilePath)

                'Create a file to write raw Timeseries values to (if lRawTsFlag is true)
                'constructed below to quiet the error debug messages due to declaration in If statement
                Dim lRawTsOutputStream As Object
                Dim lRawTsString As Object
                If lRawTsFlag Then
                    lRawTsOutputStream = System.IO.File.CreateText(lRawTsFilePath)
                    lRawTsString = ""
                End If

                'sring that grows recursively with each new hour. Gets written at the end of the stat computations.
                Dim lWriteBreakPointLines As String = ""

                Dim lWritePreamble(15) As String
                For i = 0 To 15
                    lWritePreamble(i) = ""
                Next

                lWritePreamble(1) = " 0.0"
                lWritePreamble(2) = "1".PadLeft(lTableCellWidth, lTableDelimiter) & "1".PadLeft(lTableCellWidth, lTableDelimiter) & "1".PadLeft(lTableCellWidth, lTableDelimiter)
                lWritePreamble(3) = lTSPREC.Attributes.GetDefinedValue("Parent Timeseries").Value.ToString & "RunID: " & lRunId
                lWritePreamble(4) = " Latitude Longitude Elevation (m) Obs. Years   Beginning year  Years simulated"
                lWritePreamble(5) = lTSPREC.Attributes.GetDefinedValue("Latitude").Value.ToString.PadLeft(10, lTableDelimiter) & " " & lTSPREC.Attributes.GetDefinedValue("Longitude").Value.ToString.PadLeft(10, lTableDelimiter) & " " & lElevation.PadLeft(10, lTableDelimiter) & " " & lStrModelEnd(0) - lStrModelBegin(0).ToString.PadLeft(lTableCellWidth, lTableDelimiter) & " " & 1 & " " & lStrModelEnd(0) - lStrModelBegin(0).ToString.PadLeft(lTableCellWidth, lTableDelimiter)
                lWritePreamble(6) = " Observed monthly ave max temperature (C)"
                lWritePreamble(7) = "##### Calculated #####"
                lWritePreamble(8) = " Observed monthly ave min temperature (C)"
                lWritePreamble(9) = "##### Calculated #####"
                lWritePreamble(10) = " Observed monthly ave solar radiation (Langleys/day)"
                lWritePreamble(11) = "##### Calculated #####"
                lWritePreamble(12) = " Observed monthly ave precipitation (mm)"
                lWritePreamble(13) = "##### Calculated #####"
                lWritePreamble(14) = " da mo  year  bpts  tmax  tmin   rad  w-vl w-dir  tdew"
                lWritePreamble(15) = "               (#)   (C)   (C) (1/d) (m/s) (Deg)   (C)"

                'TempDayStatResults indexes:
                '0: min temperature
                '1: max temperature
                '2: solar radiation
                '3: wind velocity
                '4: wind direction
                '5: dew point temp
                Dim lTempDayStatResults(5) As Double

                'Monthly statistics need to be computed for four things. We will create four collections of all monthly data, then compute stats after the master loop has run.
                Dim lMonthlyStats(3, 11) As Collection
                'initialize the collections
                For i = 0 To 3
                    For j = 0 To 11
                        lMonthlyStats(i, j) = New Collection
                    Next
                Next

                'Make array of collections to calculate stats at the end of every month.
                Dim lTempCurrentMonthStats(3) As Collection
                For i = 0 To 3
                    lTempCurrentMonthStats(i) = New Collection
                Next

                'variables for statistics
                Dim lTempRecord As Double
                Dim lDayOutString As String = ""
                Dim lMonthOutString As String = ""
                Dim lCurrentMonth As Integer
                Dim lHourIndexEnd As Integer = lTSPREC.Values.Length
                Dim lTempDayPrecipAccumulate As Double
                Dim lCurrentMonthStatCollection As Collection

                Dim lBigAccum As Double = 0
                Dim lCurrentYear As Integer
                Dim lYearCollection As New Collection

                'Loop through every record (hour) in the timeseries.
                '===================================================
                For i = 0 To lHourIndexEnd - 25

                    'Set dates for very first iteration
                    If i = 0 Then
                        lJulianDate = lTSPREC.Dates.Values(i)  'date associated with the first value 
                        J2Date(lJulianDate, lDate)
                    End If

                    'export current date to string for raw timeseries textfile (if lRawTsFlag is true). vbCrLf is added after we gather the precip value below
                    If lRawTsFlag Then

                        'loop through the date array and place a "-" between year, month, day hour will be added in the midnight loop (j)
                        lTempString = ""
                        For k = 0 To 2
                            lTempString &= lDate(k).ToString & ","
                        Next

                    End If

                    'Its midnight in the loop "i", so let us loop through 24 hours then calculate the stats.
                    If lDate(3) = 0 AndAlso i <> lHourIndexEnd - 1 Then

                        'set all averaged values equal to zero, wont affect mean sum because denominator used to calculate mean is set at 24.
                        lTempDayStatResults(2) = 0
                        lTempDayStatResults(3) = 0
                        lTempDayStatResults(4) = 0
                        lTempDayStatResults(5) = 0
                        lTempDayPrecipAccumulate = 0

                        For j = 1 To 24
                            'ATEM: min and max temperature for the current day
                            lTempRecord = lTSATEM.Values(i + j)
                            'convert from degrees F to C
                            lTempRecord = (lTempRecord - 32) * 5 / 9
                            If j = 1 Then
                                lTempDayStatResults(0) = lTempRecord
                                lTempDayStatResults(1) = lTempRecord
                            Else
                                If lTempRecord < lTempDayStatResults(0) Then lTempDayStatResults(0) = lTempRecord
                                If lTempRecord > lTempDayStatResults(1) Then lTempDayStatResults(1) = lTempRecord
                            End If

                            'SOLR: daily radiation
                            lTempRecord = lTSSOLR.Values(i + j)
                            lTempDayStatResults(2) += lTempRecord

                            'WIND: wind velocity
                            lTempRecord = lTSWIND.Values(i + j)
                            'convert MPH to meters/sec.
                            lTempRecord *= 0.44704
                            lTempDayStatResults(3) += lTempRecord

                            'wind direction (set to zero for now)
                            'DNE -- lWDMDataSetTemp = lWDMDataSource.DataSets.ItemByIndex(xx)
                            lTempDayStatResults(4) = 0

                            'DEWP: dewpoint temp
                            lTempRecord = lTSDEWP.Values(i + j)
                            'convert from degrees F to C
                            lTempRecord = (lTempRecord - 32) * 5 / 9
                            lTempDayStatResults(5) += lTempRecord

                            'write breakpoint line for this j hour
                            lTempRecord = lTSPREC.Values(i + j)
                            'Convert inches to millimeters
                            lTempRecord *= 25.4
                            lTempDayPrecipAccumulate += lTempRecord
                            lDayOutString = lDayOutString & " " & j.ToString.PadLeft(2, " ") & " " & WEPPformatMoreDetail(lTempDayPrecipAccumulate)

                            'export this hour's values to the debug raw timeseries textfile (if lRawTsFlag is true)
                            If lRawTsFlag Then
                                lRawTsOutputStream.WriteLine(lTempString & j & "," & lTempRecord)
                            End If

                            If j = 24 Then
                                'average things
                                lTempDayStatResults(3) = lTempDayStatResults(3) / 24
                                lTempDayStatResults(5) = lTempDayStatResults(5) / 24
                                lBigAccum += lTempDayPrecipAccumulate
                            Else
                                lDayOutString &= vbCrLf
                            End If
                        Next

                        lDayOutString = " " & lDate(2).ToString.PadLeft(2, lTableDelimiter) _
                        & " " & lDate(1).ToString.PadLeft(2, lTableDelimiter) _
                        & " " & lDate(0) - lStrModelBegin(0) + 1 _
                        & "24".PadLeft(lTableCellWidth, lTableDelimiter) _
                        & WEPPformat(lTempDayStatResults(1)).PadLeft(lTableCellWidth, lTableDelimiter) _
                        & WEPPformat(lTempDayStatResults(0)).PadLeft(lTableCellWidth, lTableDelimiter) _
                        & WEPPformat(lTempDayStatResults(2)).PadLeft(lTableCellWidth, lTableDelimiter) _
                        & WEPPformat(lTempDayStatResults(3)).PadLeft(lTableCellWidth, lTableDelimiter) _
                        & WEPPformat(lTempDayStatResults(4)).PadLeft(lTableCellWidth, lTableDelimiter) _
                        & WEPPformat(lTempDayStatResults(5)).PadLeft(lTableCellWidth, lTableDelimiter) _
                        & vbCrLf _
                        & lDayOutString

                        'Dont append a linefeed if this is the last hour. This prevents blank lines (which is bad, I think).
                        If i = 0 Then
                            lWriteBreakPointLines &= lDayOutString
                        Else
                            lWriteBreakPointLines &= vbCrLf & lDayOutString
                        End If

                        'reset the string to nothing
                        lDayOutString = ""

                        'add todays stats to the current month collections
                        lTempCurrentMonthStats(0).Add(lTempDayStatResults(1))
                        lTempCurrentMonthStats(1).Add(lTempDayStatResults(0))
                        lTempCurrentMonthStats(2).Add(lTempDayStatResults(2))
                        lTempCurrentMonthStats(3).Add(lTempDayPrecipAccumulate)

                    End If

                    'set the current month as an integer
                    lCurrentMonth = lDate(1)
                    lCurrentYear = lDate(0)

                    'change lDate for the next hour and check for a month change
                    J2Date(lTSPREC.Dates.Values(i + 1), lDate)

                    If lDate(0) <> lCurrentYear Or i = lHourIndexEnd - 25 Then
                        lYearCollection.Add(lBigAccum)
                        lBigAccum = 0
                    End If

                    If lDate(1) <> lCurrentMonth Or i = lHourIndexEnd - 25 Then
                        'Average all current month stats

                        'ATEM Max
                        lMonthlyStats(0, lCurrentMonth - 1).Add(MeanCollectionOfDoubles(lTempCurrentMonthStats(0)))

                        'ATEM Min
                        lMonthlyStats(1, lCurrentMonth - 1).Add(MeanCollectionOfDoubles(lTempCurrentMonthStats(1)))

                        'SOLR
                        lMonthlyStats(2, lCurrentMonth - 1).Add(MeanCollectionOfDoubles(lTempCurrentMonthStats(2)))

                        'PREC
                        lMonthlyStats(3, lCurrentMonth - 1).Add(SumCollectionOfDoubles(lTempCurrentMonthStats(3)))

                        lTempCurrentMonthStats(0).Clear()
                        lTempCurrentMonthStats(1).Clear()
                        lTempCurrentMonthStats(2).Clear()
                        lTempCurrentMonthStats(3).Clear()

                    End If

                Next


                'Prep lines for the text file write

                'average the (months)averages of the (month)average of the averages(day) for everything.
                For i = 0 To 3
                    For j = 0 To 11
                        lCurrentMonthStatCollection = lMonthlyStats(i, j)
                        If j = 0 Then
                            lMonthOutString = lMonthOutString & WEPPformat(MeanCollectionOfDoubles(lCurrentMonthStatCollection)).PadLeft(lTableCellWidth, lTableDelimiter)
                        Else
                            lMonthOutString = lMonthOutString & lTableDelimiter & WEPPformat(MeanCollectionOfDoubles(lCurrentMonthStatCollection)).PadLeft(lTableCellWidth, lTableDelimiter)
                        End If
                    Next

                    'write strings to respective lines
                    Select Case i
                        Case 0
                            lWritePreamble(7) = lMonthOutString
                        Case 1
                            lWritePreamble(9) = lMonthOutString
                        Case 2
                            lWritePreamble(11) = lMonthOutString
                        Case 3
                            lWritePreamble(13) = lMonthOutString
                    End Select

                    lMonthOutString = ""
                Next

                For k = 1 To 15
                    lWeppOutputStream.WriteLine(lWritePreamble(k))
                Next

                'WRITE LINES 16, 17 and subsequent lines.

                Dim lAnnualSummary As String = ""
                For i = 1 To lYearCollection.Count
                    lAnnualSummary &= WEPPformat(lYearCollection.Item(i)) & " "
                Next

                Logger.Dbg("For Stat Checking: Total precipitation for each year (mm): " & lAnnualSummary)
                Logger.Dbg("For Stat Checking: Mean of Total precipitation for each year (mm): " & WEPPformat(MeanCollectionOfDoubles(lYearCollection)))
                lWeppOutputStream.WriteLine(lWriteBreakPointLines)

                If lRawTsFlag Then
                    lRawTsOutputStream.Flush()
                    lRawTsOutputStream.Close()
                End If


                'clean up file stuff in memory.
                lWeppOutputStream.Flush()
                lWeppOutputStream.Close()
                lWDMDataSource = Nothing
            End If
            Logger.Dbg("Done with " & lRunId)







        Catch ex As Exception
            Logger.Dbg("Badness message: " & ex.ToString)
        End Try

    End Sub
    Private Function MeanCollectionOfDoubles(ByRef aCollection As Collection) As Double
        Dim lTempAccumulator As Double = 0

        If aCollection.Count > 0 Then
            For i = 1 To aCollection.Count
                If IsNumeric(aCollection.Item(i)) Then lTempAccumulator += aCollection.Item(i)
            Next
            lTempAccumulator /= aCollection.Count
            Return lTempAccumulator
        Else
            Return -9999999
        End If

    End Function
    Private Function SumCollectionOfDoubles(ByRef aCollection As Collection) As Double
        Dim lTempAccumulator As Double = 0

        If aCollection.Count > 0 Then
            For i = 1 To aCollection.Count
                If IsNumeric(aCollection.Item(i)) Then lTempAccumulator += aCollection.Item(i)
            Next
            Return lTempAccumulator
        Else
            Return -9999999
        End If
    End Function
    Private Sub DumpAttributes(ByVal aTimeSeries As atcTimeseries)
        Dim lAttributeValues As SortedList = aTimeSeries.Attributes.ValuesSortedByName
        For lAttributeIndex As Integer = 0 To lAttributeValues.Count - 1
            Dim lAttributeName As String = lAttributeValues.GetKey(lAttributeIndex)
            Dim lAttributeValue As String = aTimeSeries.Attributes.GetFormattedValue(lAttributeName)
            Logger.Dbg(lAttributeName & " = " & lAttributeValue)
        Next
    End Sub
    Function WEPPformat(ByVal aNumber As Double) As String
        Dim lStr As String = DoubleToString(aNumber, 10, , "0", , 3)
        If Not lStr.Contains(".") Then lStr &= "."
        Return lStr
    End Function
    Function WEPPformatMoreDetail(ByVal aNumber As Double) As String
        Dim lStr As String = DoubleToString(aNumber, 10, , "#.####", , 6)
        If Not lStr.Contains(".") Then lStr &= "."
        Return lStr
    End Function


End Module
