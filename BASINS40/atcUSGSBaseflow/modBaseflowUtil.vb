Imports atcData
Imports atcUtility
Imports MapWinUtility

Module modBaseflowUtil

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

    Private pStations As atcCollection
    Public Property Stations() As atcCollection
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

    Private pStationInfoFile As String = "Station.txt"
    Public Property StationInfoFile() As String
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

    Public StationHeaderText As String = "File ""station.txt""" & vbCrLf & _
"This file is read by programs PREP, RECESS, RORA, and PART, to" & vbCrLf & _
"obtain the drainage area.  Note: This file should have ten header" & vbCrLf & _
"lines.  The streamflow file name should be 12 characters or less." & vbCrLf & _
"----------------------------------------------------------------" & vbCrLf & _
"              Drainage" & vbCrLf & _
" Name of       area     The space below, after drainage area, is" & vbCrLf & _
" streamflow   (Square   for optional information that is not read" & vbCrLf & _
" file          miles)   by the programs.  This is free-form." & vbCrLf & _
"---------------------- ------------------------------------------"

    Public Function GetStations() As Integer
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

    Public Function SaveStations(ByVal aStationList As atcCollection, ByVal aSpecification As String) As Integer
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

    Public Function PrintDataSummary(ByVal aTS As atcTimeseries) As String
        'print out data summary, usgs style
        'If pMissingDataMonth Is Nothing Then
        '    pMissingDataMonth = New atcCollection()
        'Else
        '    pMissingDataMonth.Clear()
        'End If
        'Dim lNeedToRecordMissingMonth As Boolean = (pMissingDataMonth.Count = 0)
        'Dim lNeedToRecordMissingMonth As Boolean = True
        Dim lFileName As String = IO.Path.GetFileName(aTS.Attributes.GetValue("History 1"))
        Dim lDate(5) As Integer
        Dim lStrBuilderDataSummary As New System.Text.StringBuilder
        lStrBuilderDataSummary.AppendLine("READING FILE NAMED " & lFileName)
        J2Date(aTS.Attributes.GetValue("SJDay"), lDate)
        lStrBuilderDataSummary.AppendLine("FIRST YEAR IN RECORD =  " & lDate(0))
        J2Date(aTS.Dates.Value(aTS.numValues - 1), lDate)
        lStrBuilderDataSummary.AppendLine(" LAST YEAR IN RECORD =  " & lDate(0))
        lStrBuilderDataSummary.AppendLine( _
            "                 MONTH        " & vbCrLf & _
            " YEAR   J F M A M J J A S O N D")

        For I As Integer = 0 To aTS.numValues - 1
            J2Date(aTS.Dates.Value(I), lDate)
            lStrBuilderDataSummary.Append(lDate(0).ToString.PadLeft(5, " "))
            Dim lDaysInMonth As Integer = DayMon(lDate(0), lDate(1))

            For M As Integer = 1 To 12
                Dim lMonthFlag As String = "."
                If lDate(1) = M Then
                    lDaysInMonth = DayMon(lDate(0), M)
                    Dim lDayInMonthDone As Integer = 0
                    While lDate(2) <= lDaysInMonth And lDate(1) = M
                        lDayInMonthDone += 1
                        If I = aTS.numValues Then Exit While
                        If Double.IsNaN(aTS.Value(I + 1)) OrElse aTS.Value(I + 1) < 0 Then
                            lMonthFlag = "X"
                        End If
                        I += 1
                        J2Date(aTS.Dates.Value(I), lDate)
                    End While
                    If lDayInMonthDone < lDaysInMonth Then
                        If lMonthFlag = "." Then lMonthFlag = "X"
                    End If
                    If M = 1 Then
                        lStrBuilderDataSummary.Append("   " & lMonthFlag)
                    ElseIf M = 12 Then
                        'End of one year
                        lStrBuilderDataSummary.AppendLine(" " & lMonthFlag)
                        I -= 1
                    Else
                        lStrBuilderDataSummary.Append(" " & lMonthFlag)
                    End If
                Else
                    If M = 1 Then
                        lStrBuilderDataSummary.Append("X".PadLeft(4, " "))
                    ElseIf M = 12 Then
                        lStrBuilderDataSummary.AppendLine(" X")
                    Else
                        lStrBuilderDataSummary.Append(" X")
                    End If
                End If
                'If lNeedToRecordMissingMonth Then
                '    Dim lKeyToBeAdded As String = lDate(0).ToString & "_" & M.ToString.PadLeft(2, "0")
                '    If Not pMissingDataMonth.Keys.Contains(lKeyToBeAdded) Then
                '        If lMonthFlag = "X" Then
                '            pMissingDataMonth.Add(lKeyToBeAdded, lMonthFlag)
                '        End If
                '    End If
                'End If
            Next 'month
        Next 'day
        lStrBuilderDataSummary.AppendLine("")
        lStrBuilderDataSummary.AppendLine(" COMPLETE RECORD = .      INCOMPLETE = X")

        'Dim lDataSummaryFilename As String = IO.Path.GetDirectoryName(aTS.Attributes.GetValue("History 1"))
        'lDataSummaryFilename = lDataSummaryFilename.Substring("Read from ".Length)
        'lDataSummaryFilename = IO.Path.Combine(lDataSummaryFilename, "DataSummary.txt")
        'Dim lSW As New StreamWriter(lDataSummaryFilename, False)
        'lSW.WriteLine(lStrBuilderDataSummary.ToString)
        'lSW.Flush()
        'lSW.Close()
        'lSW = Nothing
        'lStrBuilderDataSummary.Remove(0, lStrBuilderDataSummary.Length)

        Return lStrBuilderDataSummary.ToString()
    End Function

    ''' <summary>
    ''' this is the .BSF WatStore format ASCII output
    ''' </summary>
    ''' <param name="aTs">Streamflow timeseries with baseflow group as an attribute</param>
    ''' <param name="aFilename">.BSF output filename</param>
    ''' <remarks></remarks>
    Public Sub ASCIIHySepBSF(ByVal aTs As atcTimeseries, ByVal aFilename As String, Optional ByVal aBFName As String = "")

        Dim lSTAID As String = aTs.Attributes.GetValue("STAID", "12345678")
        Dim lColumnId1 As String = "2" & lSTAID & "   60    3"
        Dim lColumnId2 As String = ("3" & lSTAID).PadRight(16, " ")

        Dim lTsBF As atcTimeseries = Nothing
        If aBFName = "" Then aBFName = "HySep"
        For Each lTs As atcTimeseries In aTs.Attributes.GetDefinedValue("Baseflow").Value
            If lTs.Attributes.GetValue("Scenario").ToString.StartsWith(aBFName) Then
                lTsBF = lTs
                Exit For
            End If
        Next

        If lTsBF Is Nothing Then
            Logger.Dbg("No baseflow data found.")
            Exit Sub
        End If

        Dim lOutputFile As String = aFilename
        Dim lSW As New IO.StreamWriter(lOutputFile, False)
        lSW.WriteLine(lColumnId1)

        Dim lDate(5) As Integer
        Dim lMonthQuarter As Integer = 1
        J2Date(lTsBF.Attributes.GetValue("SJDay"), lDate)
        Dim lStartYear As Integer = lDate(0)
        Dim lStartMonth As Integer = lDate(1)
        Dim lStartDay As Integer = lDate(2)
        'J2Date(lTsBF.Dates.Value(lTsBF.numValues - 1), lDate)
        J2Date(lTsBF.Attributes.GetValue("EJDay"), lDate)
        Dim lEndYear As Integer = lDate(0)
        Dim lEndMonth As Integer = lDate(1)
        Dim lEndDay As Integer = lDate(2)
        Dim lBFTsEndDate As Double = lTsBF.Dates.Value(lTsBF.numValues)

        Dim lStarting As Boolean = True
        Dim lEnded As Boolean = False

        For I As Integer = 0 To lTsBF.numValues - 1
            J2Date(lTsBF.Dates.Value(I), lDate)
            lMonthQuarter = 1
            'write one month at a time
            Dim lcurrentMonth As Integer = lDate(1)
            Dim lcurrentYear As Integer = lDate(0)
            Dim lfinalIndexInMonth As Integer = 0
            Dim lmonthQuarterStart As Integer = 0
            While lDate(2) <= DayMon(lcurrentYear, lcurrentMonth)

                If lDate(2) <= 8 Then
                    lMonthQuarter = 1
                    lmonthQuarterStart = lDate(2) - 1
                ElseIf lDate(2) <= 16 Then
                    lMonthQuarter = 2
                    lmonthQuarterStart = lDate(2) - 8 - 1
                ElseIf lDate(2) <= 24 Then
                    lMonthQuarter = 3
                    lmonthQuarterStart = lDate(2) - 16 - 1
                Else
                    lMonthQuarter = 4
                    lmonthQuarterStart = lDate(2) - 24 - 1
                End If

                lSW.Write(lColumnId2)
                lSW.Write(lDate(0).ToString) 'year
                lSW.Write(lDate(1).ToString.PadLeft(2, " ")) 'month
                lSW.Write(lMonthQuarter.ToString.PadLeft(2, " ")) 'month quarter

                For J As Integer = 0 To 7

                    If lStarting And lDate(2) < lStartDay Then
                        lSW.Write("       ")
                    ElseIf J < lmonthQuarterStart Then
                        lSW.Write("       ")
                    Else
                        'need to determine end of month
                        J2Date(lTsBF.Dates.Value(I + J - lmonthQuarterStart), lDate)

                        'check if reaching the end of the BF timeseries
                        If I + J - lmonthQuarterStart >= lTsBF.numValues Then
                            lEnded = True
                            lSW.WriteLine()
                            Exit While
                        End If
                        'check if within the current month
                        If lDate(2) <= DayMon(lDate(0), lcurrentMonth) Then
                            lSW.Write(String.Format("{0:#.00}", lTsBF.Value(I + J - lmonthQuarterStart + 1)).PadLeft(7, " "))
                            lStarting = False
                            If lDate(2) = DayMon(lDate(0), lcurrentMonth) Then
                                lSW.WriteLine()
                                lfinalIndexInMonth = I + J
                                Exit While
                            End If
                        Else
                            'don't think need to fill out the blanks with the blanks,
                            'so just quit the loop here
                            lSW.WriteLine()
                            lfinalIndexInMonth = I + J
                            Exit While
                        End If

                    End If

                Next
                lSW.WriteLine()
                I += 8 - lmonthQuarterStart
                J2Date(lTsBF.Dates.Value(I), lDate)
            End While

            'end it if already reached the end of the timeseries
            If lEnded Then
                Exit For
            Else
                I = lfinalIndexInMonth
            End If
            'move I forward to the end of the month

        Next

        lSW.Flush()
        lSW.Close()
        lSW = Nothing
        ReDim lDate(0)
        lDate = Nothing
    End Sub

    ''' <summary>
    ''' PART ASCII output, partday.txt
    ''' </summary>
    ''' <param name="aTS">Daily streamflow Tser with baseflow group</param>
    ''' <param name="aFilename">partday file name</param>
    ''' <remarks>This summary file is supposed to be overwritten</remarks>
    Public Sub ASCIIPartDaily(ByVal aTS As atcTimeseries, ByVal aFilename As String)

        Dim lTsBaseflow1 As atcTimeseries = Nothing
        Dim lTsBaseflow2 As atcTimeseries = Nothing
        Dim lTsBaseflow3 As atcTimeseries = Nothing

        Dim lBFDatagroup As atcTimeseriesGroup = aTS.Attributes.GetDefinedValue("Baseflow").Value
        If lBFDatagroup IsNot Nothing Then
            For Each lTsBF As atcTimeseries In lBFDatagroup
                Select Case lTsBF.Attributes.GetValue("Scenario")
                    Case "PartDaily1"
                        lTsBaseflow1 = lTsBF
                    Case "PartDaily2"
                        lTsBaseflow2 = lTsBF
                    Case "PartDaily3"
                        lTsBaseflow3 = lTsBF
                End Select
            Next
        Else
            Logger.Dbg("ASCIIPartDaily: no baseflow data found.")
            Exit Sub
        End If

        If lTsBaseflow1 Is Nothing OrElse lTsBaseflow2 Is Nothing OrElse lTsBaseflow3 Is Nothing Then
            Logger.Dbg("ASCIIPartDaily: no baseflow data found.")
            Exit Sub
        End If

        Dim lstart As Double = lTsBaseflow1.Attributes.GetValue("SJDay")
        Dim lend As Double = lTsBaseflow1.Attributes.GetValue("EJDay")
        Dim lTsFlow As atcTimeseries = SubsetByDate(aTS, lstart, lend, Nothing)

        Dim lSW As New IO.StreamWriter(aFilename, False)

        Dim lDate(5) As Integer
        J2Date(aTS.Dates.Value(0), lDate)
        Dim lStartingYear As String = lDate(0).ToString
        J2Date(aTS.Dates.Value(aTS.numValues - 1), lDate)
        Dim lEndingYear As String = lDate(0).ToString

        lSW.WriteLine(" THIS IS FILE PARTDAY.TXT WHICH GIVES DAILY OUTPUT OF PROGRAM PART. ")
        lSW.WriteLine(" NOTE -- RESULTS AT THIS SMALL TIME SCALE ARE PROVIDED FOR ")
        lSW.WriteLine(" THE PURPOSES OF PROGRAM SCREENING AND FOR GRAPHICS, BUT ")
        lSW.WriteLine(" SHOULD NOT BE REPORTED OR USED QUANTITATIVELY ")
        lSW.WriteLine("  INPUT FILE = " & IO.Path.GetFileName(aTS.Attributes.GetValue("History 1")))
        lSW.WriteLine("  STARTING YEAR =" & lStartingYear.PadLeft(6, " "))
        lSW.WriteLine("  ENDING YEAR =" & lEndingYear.PadLeft(8, " "))
        lSW.WriteLine("                          BASE FLOW FOR EACH")
        lSW.WriteLine("                             REQUIREMENT OF  ")
        lSW.WriteLine("           STREAM         ANTECEDENT RECESSION ")
        lSW.WriteLine("  DAY #     FLOW        #1         #2         #3          DATE ")
        Dim lDayCount As String
        Dim lStreamFlow As String
        Dim lBF1 As String
        Dim lBF2 As String
        Dim lBF3 As String
        Dim lDateStr As String

        For I As Integer = 0 To lTsFlow.numValues - 1
            lDayCount = (I + 1).ToString.PadLeft(5, " ")
            lStreamFlow = String.Format("{0:0.00}", lTsFlow.Value(I + 1)).PadLeft(11, " ")
            lBF1 = String.Format("{0:0.00}", lTsBaseflow1.Value(I + 1)).PadLeft(11, " ")
            lBF2 = String.Format("{0:0.00}", lTsBaseflow2.Value(I + 1)).PadLeft(11, " ")
            lBF3 = String.Format("{0:0.00}", lTsBaseflow3.Value(I + 1)).PadLeft(11, " ")
            J2Date(lTsFlow.Dates.Value(I), lDate)
            lDateStr = lDate(0).ToString.PadLeft(9, " ") & _
                       lDate(1).ToString.PadLeft(4, " ") & _
                       lDate(2).ToString.PadLeft(4, " ")
            lSW.WriteLine(lDayCount & lStreamFlow & lBF1 & lBF2 & lBF3 & lDateStr)
        Next
        lSW.Flush()
        lSW.Close()
        lSW = Nothing
    End Sub

    ''' <summary>
    ''' PART ASCII output, partmon.txt
    ''' </summary>
    ''' <param name="aTS">Daily streamflow Tser with baseflow group</param>
    ''' <param name="aFilename">partmon filename</param>
    ''' <remarks>This summary file is supposed to be overwritten</remarks>
    Public Sub ASCIIPartMonthly(ByVal aTS As atcTimeseries, ByVal aFilename As String)

        Dim lTsBaseflowMonthlyDepth As atcTimeseries = Nothing
        Dim lTsBaseflowDaily As atcTimeseries = Nothing
        Dim lTotalBaseflowDepth As Double = 0.0
        Dim lMissingDataMonth As atcCollection = Nothing
        Dim lDrainageArea As Double = 0.0
        Dim lBFDatagroup As atcTimeseriesGroup = aTS.Attributes.GetDefinedValue("Baseflow").Value
        If lBFDatagroup IsNot Nothing Then
            For Each lTsBF As atcTimeseries In lBFDatagroup
                If lTsBF.Attributes.GetValue("Scenario") = "PartMonthlyDepth" Then
                    lTsBaseflowMonthlyDepth = lTsBF
                    lDrainageArea = lTsBF.Attributes.GetValue("Drainage Area")
                    lTotalBaseflowDepth = lTsBF.Attributes.GetValue("SumDepth")
                    lMissingDataMonth = lTsBF.Attributes.GetValue("MissingMonths")
                    If lTsBaseflowDaily IsNot Nothing Then Exit For
                ElseIf lTsBF.Attributes.GetValue("Scenario") = "PartDaily1" Then
                    lTsBaseflowDaily = lTsBF
                    If lTsBaseflowMonthlyDepth IsNot Nothing Then Exit For
                End If
            Next
        Else
            Logger.Dbg("ASCIIPartMonthly: no baseflow data found.")
            Exit Sub
        End If

        If lTsBaseflowMonthlyDepth Is Nothing Then
            Logger.Dbg("ASCIIPartMonthly: no baseflow data found.")
            Exit Sub
        End If

        'PrintDataSummary(aTS) 'repopulate the missing-month collection

        Dim lstart As Double = lTsBaseflowDaily.Attributes.GetValue("SJDay")
        Dim lend As Double = lTsBaseflowDaily.Attributes.GetValue("EJday")
        Dim lTsFlow As atcTimeseries = SubsetByDate(aTS, lstart, lend, Nothing)

        Dim lSW As New IO.StreamWriter(aFilename, False)

        Dim lDate(5) As Integer
        J2Date(aTS.Dates.Value(0), lDate)
        Dim lStartingYear As String = lDate(0).ToString
        J2Date(aTS.Dates.Value(aTS.numValues - 1), lDate)
        Dim lEndingYear As String = lDate(0).ToString

        'Monthly stream flow in cfs, then, turn into inches
        Dim lTotXX As Double = 0.0 ' in inches
        Dim lTsMonthlyFlowDepth As atcTimeseries = Nothing
        If lTsMonthlyFlowDepth Is Nothing Then
            lTsMonthlyFlowDepth = Aggregate(lTsFlow, atcTimeUnit.TUMonth, 1, atcTran.TranAverSame, Nothing)
            For M As Integer = 1 To lTsMonthlyFlowDepth.numValues
                If lTsMonthlyFlowDepth.Value(M) = 0 Then
                    lTsMonthlyFlowDepth.Value(M) = -99.99
                End If
            Next

            'Monthly stream flow in inches and flag as -99.99 if month is incomplete
            'Also determine total of monthly amounts
            For M As Integer = 1 To lTsMonthlyFlowDepth.numValues
                J2Date(lTsMonthlyFlowDepth.Dates.Value(M - 1), lDate)
                If lMissingDataMonth.Keys.Contains(lDate(0).ToString & "_" & lDate(1).ToString.PadLeft(2, "0")) Then
                    lTsMonthlyFlowDepth.Value(M) = -99.99
                Else
                    lTsMonthlyFlowDepth.Value(M) *= DayMon(lDate(0), lDate(1)) / (26.888889 * lDrainageArea)
                    lTotXX += lTsMonthlyFlowDepth.Value(M)
                End If
            Next
        End If

        lSW.WriteLine("  ")
        lSW.WriteLine("  THIS IS FILE PARTMON.TXT FOR INPUT FILE: " & IO.Path.GetFileName(aTS.Attributes.GetValue("History 1")))
        lSW.WriteLine(" ")
        lSW.WriteLine("  PROGRAM VERSION DATE = JANUARY 2007  ")
        lSW.WriteLine(" ")
        lSW.WriteLine(" ")
        lSW.WriteLine("                        MONTHLY STREAMFLOW (INCHES):")
        lSW.WriteLine("          J     F     M     A     M     J     J     A     S     O     N     D   YEAR")
        lSW.Flush()
        Dim lFieldWidth As Integer = 6
        Dim lTsYearly As atcTimeseries = Aggregate(lTsMonthlyFlowDepth, atcTimeUnit.TUYear, 1, atcTran.TranSumDiv)
        Dim lYearCount As Integer = 1
        Dim lYearHasMiss As Boolean = False
        For I As Integer = 1 To lTsMonthlyFlowDepth.numValues
            J2Date(lTsMonthlyFlowDepth.Dates.Value(I - 1), lDate)
            lSW.Write(lDate(0).ToString.PadLeft(lFieldWidth, " ")) 'begining of a year
            Dim lCurrentYear As Integer = lDate(0)
            lYearHasMiss = False
            For M As Integer = 1 To 12
                If lDate(1) = M Then
                    If lDate(0) = lCurrentYear Then
                        If lTsMonthlyFlowDepth.Value(I) < -99.0 Then lYearHasMiss = True
                        lSW.Write(String.Format("{0:0.00}", lTsMonthlyFlowDepth.Value(I)).PadLeft(lFieldWidth, " "))
                        I += 1
                        J2Date(lTsMonthlyFlowDepth.Dates.Value(I - 1), lDate)
                    Else
                        Exit For
                    End If

                Else
                    lSW.Write(Space(lFieldWidth))
                End If
            Next
            I -= 1

            'print yearly sum
            If lYearHasMiss Then
                lSW.WriteLine(String.Format("{0:0.00}", -99.99).PadLeft(lFieldWidth, " "))
            Else
                lSW.WriteLine(String.Format("{0:0.00}", lTsYearly.Value(lYearCount)).PadLeft(lFieldWidth, " "))
            End If
            lYearCount += 1

        Next
        lSW.WriteLine(" ")
        lSW.WriteLine("                 TOTAL OF MONTHLY AMOUNTS = " & lTotXX)
        lSW.Flush()

        'print baseflow monthly values
        lSW.WriteLine(" ")
        lSW.WriteLine(" ")
        lSW.WriteLine("                         MONTHLY BASE FLOW (INCHES):")
        lSW.WriteLine("          J     F     M     A     M     J     J     A     S     O     N     D   YEAR")

        Dim lTsBFYearly As atcTimeseries = Aggregate(lTsBaseflowMonthlyDepth, atcTimeUnit.TUYear, 1, atcTran.TranSumDiv)
        lYearCount = 1
        For I As Integer = 1 To lTsBaseflowMonthlyDepth.numValues
            J2Date(lTsBaseflowMonthlyDepth.Dates.Value(I - 1), lDate)
            lSW.Write(lDate(0).ToString.PadLeft(lFieldWidth, " ")) 'begining of a year
            Dim lCurrentYear As Integer = lDate(0)
            lYearHasMiss = False
            For M As Integer = 1 To 12
                If lDate(1) = M Then
                    If lDate(0) = lCurrentYear Then
                        If lTsBaseflowMonthlyDepth.Value(I) < -99.0 Then lYearHasMiss = True
                        lSW.Write(String.Format("{0:0.00}", lTsBaseflowMonthlyDepth.Value(I)).PadLeft(lFieldWidth, " "))
                        I += 1
                        J2Date(lTsBaseflowMonthlyDepth.Dates.Value(I - 1), lDate)
                    Else
                        Exit For
                    End If

                Else
                    lSW.Write(Space(lFieldWidth))
                End If

            Next

            I -= 1
            'print yearly sum
            If lYearHasMiss Then
                lSW.WriteLine(String.Format("{0:0.00}", -99.99).PadLeft(lFieldWidth, " "))
            Else
                lSW.WriteLine(String.Format("{0:0.00}", lTsBFYearly.Value(lYearCount)).PadLeft(lFieldWidth, " "))
            End If
            lYearCount += 1
        Next

        lSW.WriteLine(" ")
        lSW.WriteLine("                  TOTAL OF MONTHLY AMOUNTS = " & String.Format("{0:0.0000000}", lTotalBaseflowDepth))
        lSW.WriteLine(" ")
        lSW.WriteLine(" RESULTS ON THE MONTHLY TIME SCALE SHOULD BE USED WITH CAUTION. ")
        lSW.WriteLine(" FILES PARTQRT.TXT AND PARTSUM.TXT GIVE RESULT AT THE")
        lSW.WriteLine(" CORRECT TIME SCALES (QUARTER YEAR, YEAR, OR MORE). ")

        lSW.Flush()
        lSW.Close()
        lSW = Nothing
        lTsYearly.Clear() : lTsYearly = Nothing
    End Sub

    ''' <summary>
    ''' PART ASCII output, partqrt.txt
    ''' </summary>
    ''' <param name="aTS">Daily streamflow Tser with baseflow group</param>
    ''' <param name="aFilename">partqrt filename</param>
    ''' <remarks>This summary file is supposed to be overwritten</remarks>
    Public Sub ASCIIPartQuarterly(ByVal aTS As atcTimeseries, ByVal aFilename As String)

        Dim lTsBaseflowMonthlyDepth As atcTimeseries = Nothing
        Dim lTsBaseflowDaily As atcTimeseries = Nothing
        Dim lTotalBaseflowDepth As Double = 0.0
        Dim lMissingDataMonth As atcCollection = Nothing
        Dim lDrainageArea As Double = 0.0
        Dim lBFDatagroup As atcTimeseriesGroup = aTS.Attributes.GetDefinedValue("Baseflow").Value
        If lBFDatagroup IsNot Nothing Then
            For Each lTsBF As atcTimeseries In lBFDatagroup
                If lTsBF.Attributes.GetValue("Scenario") = "PartMonthlyDepth" Then
                    lTsBaseflowMonthlyDepth = lTsBF
                    lDrainageArea = lTsBF.Attributes.GetValue("Drainage Area")
                    lTotalBaseflowDepth = lTsBF.Attributes.GetValue("SumDepth")
                    lMissingDataMonth = lTsBF.Attributes.GetValue("MissingMonths")
                    If lTsBaseflowDaily IsNot Nothing Then Exit For
                ElseIf lTsBF.Attributes.GetValue("Scenario") = "PartDaily1" Then
                    lTsBaseflowDaily = lTsBF
                    If lTsBaseflowMonthlyDepth IsNot Nothing Then Exit For
                End If
            Next
        Else
            Logger.Dbg("ASCIIPartQuarterly: no baseflow data found.")
            Exit Sub
        End If

        If lTsBaseflowMonthlyDepth Is Nothing Then
            Logger.Dbg("ASCIIPartQuarterly: no baseflow data found.")
            Exit Sub
        End If

        Dim lstart As Double = lTsBaseflowDaily.Attributes.GetValue("SJDay")
        Dim lend As Double = lTsBaseflowDaily.Attributes.GetValue("EJday")
        Dim lTsFlow As atcTimeseries = SubsetByDate(aTS, lstart, lend, Nothing)


        Dim lSW As New IO.StreamWriter(aFilename, False)

        Dim lDate(5) As Integer

        'Monthly stream flow in cfs
        Dim lTsMonthlyFlowDepth As atcTimeseries = Nothing
        Dim lTotXX As Double = 0.0
        If lTsMonthlyFlowDepth Is Nothing Then
            lTsMonthlyFlowDepth = Aggregate(lTsFlow, atcTimeUnit.TUMonth, 1, atcTran.TranAverSame, Nothing)
            For M As Integer = 1 To lTsMonthlyFlowDepth.numValues
                If lTsMonthlyFlowDepth.Value(M) = 0 Then
                    lTsMonthlyFlowDepth.Value(M) = -99.99
                End If
            Next
            'Monthly stream flow in inches and flag as -99.99 if month is incomplete
            'Also determine total of monthly amounts
            For M As Integer = 1 To lTsMonthlyFlowDepth.numValues
                J2Date(lTsMonthlyFlowDepth.Dates.Value(M - 1), lDate)
                If lMissingDataMonth.Keys.Contains(lDate(0).ToString & "_" & lDate(1).ToString.PadLeft(2, "0")) Then
                    lTsMonthlyFlowDepth.Value(M) = -99.99
                Else
                    lTsMonthlyFlowDepth.Value(M) *= DayMon(lDate(0), lDate(1)) / (26.888889 * lDrainageArea)
                    lTotXX += lTsMonthlyFlowDepth.Value(M)
                End If
            Next
        End If

        lSW.WriteLine("  ")
        lSW.WriteLine("  THIS IS FILE PARTQRT.TXT FOR INPUT FILE: " & IO.Path.GetFileName(aTS.Attributes.GetValue("History 1")))
        lSW.WriteLine(" ")
        lSW.WriteLine("  PROGRAM VERSION DATE = JANUARY 2007  ")
        lSW.WriteLine(" ")
        lSW.WriteLine("  ")
        lSW.WriteLine("        QUARTER-YEAR STREAMFLOW IN INCHES         ")
        lSW.WriteLine("        --------------------------------          ")
        lSW.WriteLine("          JAN-    APR-    JULY-   OCT-    YEAR    ")
        lSW.WriteLine("          MAR     JUNE    SEPT    DEC     TOTAL   ")
        lSW.Flush()

        ' 1053 FORMAT (1I6, 5F8.2)
        Dim lFieldWidth1 As Integer = 6
        Dim lFieldWidthO As Integer = 8
        Dim lTsYearly As atcTimeseries = Aggregate(lTsMonthlyFlowDepth, atcTimeUnit.TUYear, 1, atcTran.TranSumDiv)
        Dim lYearCount As Integer = 1
        Dim lQuarter1 As Double = 0
        Dim lQuarter2 As Double = 0
        Dim lQuarter3 As Double = 0
        Dim lQuarter4 As Double = 0

        Dim lQuarter1Negative As Boolean = False
        Dim lQuarter2Negative As Boolean = False
        Dim lQuarter3Negative As Boolean = False
        Dim lQuarter4Negative As Boolean = False

        For I As Integer = 1 To lTsMonthlyFlowDepth.numValues
            J2Date(lTsMonthlyFlowDepth.Dates.Value(I - 1), lDate)
            Dim lCurrentYear As Integer = lDate(0)

            lQuarter1 = 0
            lQuarter2 = 0
            lQuarter3 = 0
            lQuarter4 = 0

            lQuarter1Negative = False
            lQuarter2Negative = False
            lQuarter3Negative = False
            lQuarter4Negative = False

            For M As Integer = 1 To 12
                If lDate(1) = M And lDate(0) = lCurrentYear Then 'within a year
                    Select Case M
                        Case 1, 2, 3
                            If lTsMonthlyFlowDepth.Value(I) < -99.0 Then
                                lQuarter1Negative = True
                            Else
                                lQuarter1 += lTsMonthlyFlowDepth.Value(I)
                            End If
                        Case 4, 5, 6
                            If lTsMonthlyFlowDepth.Value(I) < -99.0 Then
                                lQuarter2Negative = True
                            Else
                                lQuarter2 += lTsMonthlyFlowDepth.Value(I)
                            End If
                        Case 7, 8, 9
                            If lTsMonthlyFlowDepth.Value(I) < -99.0 Then
                                lQuarter3Negative = True
                            Else
                                lQuarter3 += lTsMonthlyFlowDepth.Value(I)
                            End If
                        Case 10, 11, 12
                            If lTsMonthlyFlowDepth.Value(I) < -99.0 Then
                                lQuarter4Negative = True
                            Else
                                lQuarter4 += lTsMonthlyFlowDepth.Value(I)
                            End If
                    End Select
                    I += 1
                    J2Date(lTsMonthlyFlowDepth.Dates.Value(I - 1), lDate)
                End If
            Next ' month

            I -= 1

            If lQuarter1Negative Then lQuarter1 = -99.99
            If lQuarter2Negative Then lQuarter2 = -99.99
            If lQuarter3Negative Then lQuarter3 = -99.99
            If lQuarter4Negative Then lQuarter4 = -99.99

            Dim lStrYear As String = lCurrentYear.ToString.PadLeft(lFieldWidth1, " ")
            Dim lStrQ1 As String = String.Format("{0:0.00}", lQuarter1).PadLeft(lFieldWidthO, " ")
            Dim lStrQ2 As String = String.Format("{0:0.00}", lQuarter2).PadLeft(lFieldWidthO, " ")
            Dim lStrQ3 As String = String.Format("{0:0.00}", lQuarter3).PadLeft(lFieldWidthO, " ")
            Dim lStrQ4 As String = String.Format("{0:0.00}", lQuarter4).PadLeft(lFieldWidthO, " ")
            Dim lStrQYear As String = String.Format("{0:0.00}", lTsYearly.Value(lYearCount)).PadLeft(lFieldWidthO, " ")
            lSW.WriteLine(lStrYear & lStrQ1 & lStrQ2 & lStrQ3 & lStrQ4 & lStrQYear)

            lYearCount += 1
        Next 'monthly streamflow in inches

        'print quarterly baseflow values
        lSW.WriteLine("  ")
        lSW.WriteLine("  ")
        lSW.WriteLine("        QUARTER-YEAR BASE FLOW IN INCHES          ")
        lSW.WriteLine("        --------------------------------          ")
        lSW.WriteLine("          JAN-    APR-    JULY-   OCT-    YEAR    ")
        lSW.WriteLine("          MAR     JUNE    SEPT    DEC     TOTAL   ")

        Dim lTsBFYearly As atcTimeseries = Aggregate(lTsBaseflowMonthlyDepth, atcTimeUnit.TUYear, 1, atcTran.TranSumDiv)
        lYearCount = 1
        For I As Integer = 1 To lTsBaseflowMonthlyDepth.numValues
            J2Date(lTsBaseflowMonthlyDepth.Dates.Value(I - 1), lDate)
            Dim lCurrentYear As Integer = lDate(0)

            lQuarter1 = 0
            lQuarter2 = 0
            lQuarter3 = 0
            lQuarter4 = 0

            lQuarter1Negative = False
            lQuarter2Negative = False
            lQuarter3Negative = False
            lQuarter4Negative = False

            For M As Integer = 1 To 12
                If lDate(1) = M And lDate(0) = lCurrentYear Then 'within a year
                    Select Case M
                        Case 1, 2, 3
                            If lTsBaseflowMonthlyDepth.Value(I) < -99.0 Then
                                lQuarter1Negative = True
                            Else
                                lQuarter1 += lTsBaseflowMonthlyDepth.Value(I)
                            End If
                        Case 4, 5, 6
                            If lTsBaseflowMonthlyDepth.Value(I) < -99.0 Then
                                lQuarter2Negative = True
                            Else
                                lQuarter2 += lTsBaseflowMonthlyDepth.Value(I)
                            End If
                        Case 7, 8, 9
                            If lTsBaseflowMonthlyDepth.Value(I) < -99.0 Then
                                lQuarter3Negative = True
                            Else
                                lQuarter3 += lTsBaseflowMonthlyDepth.Value(I)
                            End If
                        Case 10, 11, 12
                            If lTsBaseflowMonthlyDepth.Value(I) < -99.0 Then
                                lQuarter4Negative = True
                            Else
                                lQuarter4 += lTsBaseflowMonthlyDepth.Value(I)
                            End If
                    End Select
                    I += 1
                    J2Date(lTsBaseflowMonthlyDepth.Dates.Value(I - 1), lDate)
                End If
            Next ' month

            I -= 1

            If lQuarter1Negative Then lQuarter1 = -99.99
            If lQuarter2Negative Then lQuarter2 = -99.99
            If lQuarter3Negative Then lQuarter3 = -99.99
            If lQuarter4Negative Then lQuarter4 = -99.99

            Dim lStrYear As String = lCurrentYear.ToString.PadLeft(lFieldWidth1, " ")
            Dim lStrQ1 As String = String.Format("{0:0.00}", lQuarter1).PadLeft(lFieldWidthO, " ")
            Dim lStrQ2 As String = String.Format("{0:0.00}", lQuarter2).PadLeft(lFieldWidthO, " ")
            Dim lStrQ3 As String = String.Format("{0:0.00}", lQuarter3).PadLeft(lFieldWidthO, " ")
            Dim lStrQ4 As String = String.Format("{0:0.00}", lQuarter4).PadLeft(lFieldWidthO, " ")
            Dim lStrQYear As String = String.Format("{0:0.00}", lTsBFYearly.Value(lYearCount)).PadLeft(lFieldWidthO, " ")
            lSW.WriteLine(lStrYear & lStrQ1 & lStrQ2 & lStrQ3 & lStrQ4 & lStrQYear)

            lYearCount += 1
        Next 'monthly streamflow in inches

        lSW.Flush()
        lSW.Close()
        lSW = Nothing
        lTsYearly.Clear() : lTsYearly = Nothing

    End Sub

    ''' <summary>
    ''' PART ASCII output, partsum.txt
    ''' </summary>
    ''' <param name="aTsSF">Daily streamflow Tser with baseflow group</param>
    ''' <param name="aFilename">partsum filename</param>
    ''' <remarks>This summary file is supposed to be appended</remarks>
    Public Sub ASCIIPartBFSum(ByVal aTsSF As atcTimeseries, ByVal aFilename As String)

        Dim lTsBaseflow1 As atcTimeseries = Nothing
        Dim lTsBaseflow2 As atcTimeseries = Nothing
        Dim lTsBaseflow3 As atcTimeseries = Nothing
        Dim lDrainageArea As Double = 0.0
        Dim lTBase As Double = 0.0
        Dim lBFDatagroup As atcTimeseriesGroup = aTsSF.Attributes.GetDefinedValue("Baseflow").Value
        If lBFDatagroup IsNot Nothing Then
            For Each lTsBF As atcTimeseries In lBFDatagroup
                Select Case lTsBF.Attributes.GetValue("Scenario")
                    Case "PartDaily1"
                        lTsBaseflow1 = lTsBF
                        lDrainageArea = lTsBF.Attributes.GetValue("Drainage Area")
                        lTBase = lTsBF.Attributes.GetValue("TBase")
                    Case "PartDaily2"
                        lTsBaseflow2 = lTsBF
                    Case "PartDaily3"
                        lTsBaseflow3 = lTsBF
                End Select
            Next
        Else
            Logger.Dbg("ASCIIPartBFSum: no baseflow data found.")
            Exit Sub
        End If
        
        If lTsBaseflow1 Is Nothing OrElse lTsBaseflow2 Is Nothing OrElse lTsBaseflow3 Is Nothing Then
            Logger.Dbg("ASCIIPartBFSum: no baseflow data found.")
            Exit Sub
        End If

        Dim lstart As Double = lTsBaseflow1.Attributes.GetValue("SJDay")
        Dim lend As Double = lTsBaseflow1.Attributes.GetValue("EJDay")
        Dim lTsFlow As atcTimeseries = SubsetByDate(aTsSF, lstart, lend, Nothing)

        Dim lWriteHeader As Boolean = False
        If Not IO.File.Exists(aFilename) Then
            lWriteHeader = True
        End If

        Dim lSW As New IO.StreamWriter(aFilename, True)
        Dim lDate(5) As Integer

        If lWriteHeader Then
            lSW.WriteLine("File ""partsum.txt""                    Program version -- Jan 2007")
            lSW.WriteLine("-------------------------------------------------------------------")
            lSW.WriteLine("Each time the PART program is run, a new line is written to the end")
            lSW.WriteLine("of this file.")
            lSW.WriteLine(" ")
            lSW.WriteLine("            Drainage                                           Base-")
            lSW.WriteLine("              area                  Mean           Mean        flow")
            lSW.WriteLine("File name     (Sq.   Time         streamflow      baseflow     index")
            lSW.WriteLine("             miles)  period     (cfs)  (in/yr)  (cfs)  (in/yr)  (%)")
            lSW.WriteLine("--------------------------------------------------------------------")
        End If

        Dim lDataFilename As String = IO.Path.GetFileName(aTsSF.Attributes.GetValue("History 1")).Substring(0, 10)
        Dim lPadWidth As Integer = 19 - lDataFilename.Trim().Length
        Dim lDrainageAreaStr As String = String.Format("{0:0.00}", lDrainageArea).PadLeft(lPadWidth, " ")
        Dim lSFMean As Double = aTsSF.Attributes.GetValue("Mean")
        Dim lBFMean1 As Double = lTsBaseflow1.Attributes.GetValue("Mean")
        Dim lBFMean2 As Double = lTsBaseflow2.Attributes.GetValue("Mean")
        Dim lBFMean3 As Double = lTsBaseflow3.Attributes.GetValue("Mean")
        Dim lMsg As String = ""
        If lBFMean1 <> lBFMean2 Then
            lMsg &= "STREAMFLOW VARIES BETWEEN DIFFERENT " & vbCrLf
            lMsg &= "VALUES OF THE REQMT ANT. RECESSION !!!"
        End If
        Dim lBFMeanArithmetic As Double = (lBFMean1 + lBFMean2 + lBFMean3) / 3.0

        Dim lA As Double = (lBFMean1 - lBFMean2 - lBFMean2 + lBFMean3) / 2.0
        Dim lB As Double = lBFMean2 - lBFMean1 - 3.0 * lA
        Dim lC As Double = lBFMean1 - lA - lB
        Dim lX As Double = lDrainageArea ^ 0.2 - lTBase + 1
        Dim lBFInterpolatedCFS As Double = lA * lX ^ 2.0 + lB * lX + lC 'interpolated mean base flow (cfs)
        Dim lBFInterpolatedInch As Double = lBFInterpolatedCFS * 13.5837 / lDrainageArea 'interpolated mean base flow (IN/YR)

        '   LINEAR INTERPOLATION BETWEEN RESULTS FOR THE FIRST AND SECOND VALUES
        '   OF THE REQUIREMENT OF ANTECEDENT RECESSION.....
        'Dim lBFLine As Double = lBFMean1 + (lX - 1) * (lBFMean2 - lBFMean1)
        J2Date(aTsSF.Dates.Value(0), lDate)
        Dim lYearStart As Integer = lDate(0)
        J2Date(aTsSF.Dates.Value(aTsSF.numValues - 1), lDate)
        Dim lYearEnd As Integer = lDate(0)
        Dim lDurationString As String = (lYearStart.ToString & "-" & lYearEnd.ToString).PadLeft(11, " ")
        If aTsSF.Attributes.GetValue("Count Missing") > 1 Then
            lMsg = " ******** record incomplete ********"
        Else
            lMsg = ""
        End If
        Dim lSFMeanCfs As String = String.Format("{0:0.00}", lSFMean).PadLeft(8, " ")
        Dim lSFMeanInch As String = String.Format("{0:0.00}", lSFMean * 13.5837 / lDrainageArea).PadLeft(8, " ")

        Dim lBFMeanCfs As String = String.Format("{0:0.00}", lBFInterpolatedCFS).PadLeft(8, " ")
        Dim lBFMeanInch As String = String.Format("{0:0.00}", lBFInterpolatedInch).PadLeft(8, " ")

        Dim lBFIndex As String = String.Format("{0:0.00}", 100 * lBFInterpolatedCFS / lSFMean).PadLeft(8, " ")

        lSW.Write(lDataFilename & lDrainageArea & lDurationString)
        If lMsg.Length = 0 Then
            lSW.WriteLine(lSFMeanCfs & lSFMeanInch & lBFMeanCfs & lBFMeanInch & lBFIndex)
        Else
            lSW.WriteLine(lMsg)
        End If

        lSW.Flush()
        lSW.Close()
        lSW = Nothing
    End Sub

    ''' <summary>
    ''' PART ASCII output, partWY.txt
    ''' </summary>
    ''' <param name="aTS">Daily streamflow Tser with baseflow group</param>
    ''' <param name="aFilename">partWY filename</param>
    ''' <remarks>This summary file is supposed to be overwritten</remarks>
    Public Sub ASCIIPartWaterYear(ByVal aTs As atcTimeseries, ByVal aFilename As String)
        Dim lTsBaseflowMonthlyDepth As atcTimeseries = Nothing
        Dim lBFDatagroup As atcTimeseriesGroup = aTs.Attributes.GetDefinedValue("Baseflow").Value
        If lBFDatagroup IsNot Nothing Then
            For Each lTsBF As atcTimeseries In lBFDatagroup
                If lTsBF.Attributes.GetValue("Scenario") = "PartMonthlyDepth" Then
                    lTsBaseflowMonthlyDepth = lTsBF
                    Exit For
                End If
            Next
        Else
            Logger.Dbg("ASCIIPartWaterYear: no baseflow data found.")
            Exit Sub
        End If

        If lTsBaseflowMonthlyDepth Is Nothing Then
            Logger.Dbg("ASCIIPartWaterYear: no baseflow data found.")
            Exit Sub
        End If

        Dim lSW As New IO.StreamWriter(aFilename, False)
        Dim lWaterYear As New atcSeasonsWaterYear
        Dim lWaterYearCollection As atcTimeseriesGroup = lWaterYear.Split(lTsBaseflowMonthlyDepth, Nothing)

        'write file header
        lSW.WriteLine(" Results on the basis of the ")
        lSW.WriteLine(" water year (Oct 1 to Sept 30) ")
        lSW.WriteLine("  ")
        lSW.WriteLine("         Year              Total ")
        lSW.WriteLine(" --------------------      ----- ")

        'write results
        Dim lDate(5) As Integer
        For Each lTsWaterYear As atcTimeseries In lWaterYearCollection
            If lTsWaterYear.Attributes.GetValue("Count") = 12 Then
                'a full water year, then write out
                J2Date(lTsWaterYear.Dates.Value(0), lDate)
                lSW.Write("Oct " & lDate(0))
                J2Date(lTsWaterYear.Dates.Value(lTsWaterYear.numValues), lDate)
                lSW.Write(" to Sept " & lDate(0))
                lSW.WriteLine(String.Format("{0:0.00}", lTsWaterYear.Attributes.GetValue("Sum")).PadLeft(11, " "))
            Else
                'not a full water year, ignore
            End If
        Next
        lSW.Flush()
        lSW.Close()
        lSW = Nothing
        lWaterYearCollection.Clear() : lWaterYearCollection = Nothing
        lWaterYear = Nothing
    End Sub

End Module
