Imports atcData
Imports atcUtility

Public Class clsRora
    
    Public PARAMAnteRecessDaysMin As Integer ' originally ITBASE
    Public PARAMAnteRecessDays As Integer
    Public FatalError As Boolean = False
    Public RangeOfAnteRecessionDays As ArrayList
    Public OutputFilenameRoot As String
    Public OutputDir As String

    Public TsStreamFlow As atcTimeseries
    Private pTsRecharge As atcTimeseries
    Public ReadOnly Property TsRecharge() As atcTimeseries
        Get
            Return pTsRecharge
        End Get
    End Property

    Public RechargeTotal As Double
    'Tc
    Public ReadOnly Property TimeCritical() As Double
        Get
            Return 0.2144 * PARAMRecessIndex
        End Get
    End Property

    Private PARAMRecessIndex As Double
    Private pDrainageArea As Double
    Private piPeak As Integer
    Private pUADepth As Double = 0.0371901
    Private pLog10ToNaturalLog As Double = 2.3025851 'ln(x) = 2.3025851 * log(x)
    Private pStartDate As Double
    Private pEndDate As Double

    Private pFileRoraSum10 As String = "rorasum.txt"
    Private pFileRoraPek12 As String = "rorapek.txt"
    Private pFileRoraMon14 As String = "roramon.txt"
    Private pFileRoraQrt15 As String = "roraqrt.txt"
    Private pFileRoraWY16 As String = "roraWY.txt"

    Public RecessionSegment As clsRecessionSegment = Nothing
    Public listOfSegments As atcCollection

    Public Bulletin As String = ""

    Public Sub ASCIICommon()

        'Set output files' names
        Dim lFileRoraSum10 As String = IO.Path.Combine(OutputDir, OutputFilenameRoot & "_" & pFileRoraSum10)
        Dim lFileRoraPek12 As String = IO.Path.Combine(OutputDir, OutputFilenameRoot & "_" & pFileRoraPek12)
        Dim lFileRoraMon14 As String = IO.Path.Combine(OutputDir, OutputFilenameRoot & "_" & pFileRoraMon14)
        Dim lFileRoraQrt15 As String = IO.Path.Combine(OutputDir, OutputFilenameRoot & "_" & pFileRoraQrt15)
        Dim lFileRoraWY16 As String = IO.Path.Combine(OutputDir, OutputFilenameRoot & "_" & pFileRoraWY16)
        Dim lFileRoraMonCSV As String = IO.Path.Combine(OutputDir, OutputFilenameRoot & "_" & IO.Path.ChangeExtension(pFileRoraMon14, "csv"))
        Dim lFileRoraAnnCSV As String = IO.Path.Combine(OutputDir, OutputFilenameRoot & "_" & "roraAnn.csv")
        Dim lFlowDataFilename As String = TsStreamFlow.Attributes.GetValue("History 1").replace("read from", "")
        Dim lStationID As String = TsStreamFlow.Attributes.GetValue("Location")
        Dim lStationName As String = TsStreamFlow.Attributes.GetValue("STANAM")
        Dim lDA As String = String.Format("{0:0.0}", TsStreamFlow.Attributes.GetValue("Drainage Area"))

        Dim lSeg As clsRecessionSegment = Nothing
        Dim lSW As IO.StreamWriter = Nothing

        '--  WRITE CALCULATIONS FOR EACH RECHARGE EVENT, TO FILE "rorapek.txt"  --
        lSW = New IO.StreamWriter(lFileRoraPek12, False)
        lSW.WriteLine("  THIS IS FILE rorapek.txt: PEAK BY PEAK CALCULATIONS OF PROGRAM rora.")
        lSW.WriteLine("  ")
        lSW.WriteLine("  NOTE: RESULTS AT THIS SMALL TIME SCALE ARE FOR SCREENING PURPOSES")
        lSW.WriteLine("  AND SHOULD NOT BE REPORTED OR USED QUANTITATIVELY. USE FILES")
        lSW.WriteLine("  OUTRORA.SUM OR OUTRORA.QRT INSTEAD.")
        lSW.WriteLine("  ")
        lSW.WriteLine(" INPUT FILE = " & lFlowDataFilename)
        Dim lDate(5) As Integer
        J2Date(pStartDate, lDate)
        Dim lYearStart As Integer = lDate(0)
        J2Date(pEndDate - JulianHour, lDate)
        Dim lYearEnd As Integer = lDate(0)
        lSW.WriteLine(" TIME PERIOD = " & lYearStart & " to " & lYearEnd)
        lSW.WriteLine(" K =" & PARAMRecessIndex)
        lSW.WriteLine(" CRITICAL TIME = " & DoubleToString(TimeCritical))
        lSW.WriteLine(" REQMT OF ANT. RECESSION : " & PARAMAnteRecessDaysMin)
        lSW.WriteLine(" ..TP...TS...TE.....TA.....TBC.....QP....QA.....QB....QC.......C.....DELQ....RECH..YEAR..MON")
        '855 FORMAT (3I5, 2F8.2, 1F7.1, 3F7.3, 1F8.3, 2F7.3, 1I6, 1I4)
        Dim lIndexFieldLength As Integer = listOfSegments(listOfSegments.Count - 1).PeakDayIndex.ToString.Length
        Dim lOneLine As New Text.StringBuilder
        For Each lSeg In listOfSegments
            lOneLine.Length = 0
            With lSeg
                lOneLine.Append(.PeakDayIndex.ToString.PadLeft(lIndexFieldLength + 1, " "))
                lOneLine.Append(String.Format("{0:0}", .TS).PadLeft(lIndexFieldLength + 1, " "))
                lOneLine.Append(String.Format("{0:0}", .TE).PadLeft(lIndexFieldLength + 1, " "))
                lOneLine.Append(String.Format("{0:0.00}", .TA).PadLeft(lIndexFieldLength + 4, " ")) '8
                lOneLine.Append(String.Format("{0:0.00}", .IndexAfterTimeCritical).PadLeft(lIndexFieldLength + 4, " ")) '8
                lOneLine.Append(String.Format("{0:0.0}", .PeakDayFlow).PadLeft(7, " "))
                lOneLine.Append(String.Format("{0:0.000}", .QA).PadLeft(7, " "))
                lOneLine.Append(String.Format("{0:0.000}", .QB).PadLeft(7, " "))
                lOneLine.Append(String.Format("{0:0.000}", .QC).PadLeft(7, " "))
                lOneLine.Append(String.Format("{0:0.000}", .C).PadLeft(8, " "))
                lOneLine.Append(String.Format("{0:0.000}", .DeltaQ).PadLeft(7, " "))
                lOneLine.Append(String.Format("{0:0.000}", .Recharge).PadLeft(7, " "))
                J2Date(.PeakDayDate, lDate)
                lOneLine.Append(lDate(0).ToString.PadLeft(6, " ").PadLeft(6, " "))
                lOneLine.Append(lDate(1).ToString.PadLeft(4, " ").PadLeft(4, " "))
            End With
            lSW.WriteLine(lOneLine.ToString)
        Next
        lSW.Flush() : lSW.Close() : lSW = Nothing

        '---  LONG-TERM RESULTS ARE DETERMINED AND WRITTEN TO FILE "RORASUM.TXT" ---
        lSW = New IO.StreamWriter(lFileRoraSum10, True)
        lOneLine.Length = 0
        If lFlowDataFilename.Length > 11 Then
            Dim lNewName As String = lFlowDataFilename.Substring(lFlowDataFilename.LastIndexOf("\") + 1)
            If lNewName.Length > 11 Then
                lNewName = lNewName.Substring(0, 11)
            End If

            lOneLine.Append(lNewName)
        Else
            lOneLine.Append(lFlowDataFilename.PadLeft(12, " "))
        End If
        Dim lDrainageArea As Double = TsStreamFlow.Attributes.GetValue("Drainage Area", -99.9)
        If lDrainageArea < 0 And pDrainageArea <= 0 Then
            lOneLine.Append(Space(7) & "  ")
        ElseIf pDrainageArea > 0 Then
            lOneLine.Append(String.Format("{0:0.00}", pDrainageArea).PadLeft(7, " ") & "  ")
        Else
            lOneLine.Append(String.Format("{0:0.00}", lDrainageArea).PadLeft(7, " ") & "  ")
        End If
        lOneLine.Append(lYearStart.ToString & "-" & lYearEnd.ToString)

        Dim liiMaxDay As Integer = 0
        For Y As Integer = lYearStart To lYearEnd
            If Date.IsLeapYear(Y) Then
                liiMaxDay += 366
            Else
                liiMaxDay += 365
            End If
        Next
        Dim lActualDaysInAnalysis As Integer = timdifJ(pStartDate, pEndDate, atcTimeUnit.TUDay, 1)
        If liiMaxDay > lActualDaysInAnalysis Then
            '858 FORMAT ( A12,1X,1F6.2,2X,1I4,'-',1I4, A32)
            lOneLine.Append(" *** record is not complete *** ")
        Else
            '16 FORMAT (A12,1X,1F6.2,2X,1I4,'-',1I4,1I6,3X,1I8,3X,1F7.1,1F11.3)
            lOneLine.Append(PARAMAnteRecessDaysMin.ToString.PadLeft(6, " ") & "   ")
            lOneLine.Append(listOfSegments.Count.ToString.PadLeft(8, " ") & "   ")
            lOneLine.Append(String.Format("{0:0.0}", PARAMRecessIndex).PadLeft(7, " "))
            Dim lAverageAnnualRecharge As Double = RechargeTotal / (lYearEnd - lYearStart + 1)
            lOneLine.Append(String.Format("{0:0.000}", lAverageAnnualRecharge).PadLeft(11, " "))
        End If
        lSW.WriteLine(lOneLine.ToString)
        lSW.Flush() : lSW.Close() : lSW = Nothing

        '---MONTHLY RESULTS ARE DETERMINED AND WRITTEN TO FILE "RORAMON.TXT" ---
        '---WRITE TO "RORAQRT.TXT" ---
        '---WRITE TO "RORAWY.TXT" ---
        lOneLine.Length = 0

        Dim lTextQrt As New Text.StringBuilder
        Dim lSWQrt As New IO.StreamWriter(lFileRoraQrt15, False)
        lTextQrt.AppendLine("   ")
        lTextQrt.AppendLine(" STREAMFLOW FILE = " & lFlowDataFilename)
        lTextQrt.AppendLine(" RECESSION INDEX (DAYS/LOG CYCLE) = " & PARAMRecessIndex)
        lTextQrt.AppendLine(" REQUIREMENT OF ANTEC. RECESSION =  " & PARAMAnteRecessDaysMin)
        lTextQrt.AppendLine(" PROGRAM VERSION = JANUARY 2007 ")
        lTextQrt.AppendLine("   ")
        lTextQrt.AppendLine("        QUARTER-YEAR RECHARGE IN INCHES:")
        lTextQrt.AppendLine("        --------------------------------")
        lTextQrt.AppendLine("          JAN-    APR-    JULY-   OCT-    YEAR")
        lTextQrt.AppendLine("          MAR     JUNE    SEPT    DEC     TOTAL")

        Dim lTextWY As New Text.StringBuilder
        Dim lSWWY As New IO.StreamWriter(lFileRoraWY16, False)
        lTextWY.AppendLine("  ")
        lTextWY.AppendLine(" Results on the basis of the")
        lTextWY.AppendLine(" water year (Oct 1 to Sept 30)")
        lTextWY.AppendLine("  ")
        lTextWY.AppendLine("       Year                Total")
        lTextWY.AppendLine(" -------------------       -----")

        Dim lTextMonthly As New Text.StringBuilder
        lSW = New IO.StreamWriter(lFileRoraMon14, False)
        lTextMonthly.AppendLine("   ")
        lTextMonthly.AppendLine(" STREAMFLOW FILE =" & lFlowDataFilename)
        lTextMonthly.AppendLine(" RECESSION INDEX (DAYS/LOG CYCLE) = " & PARAMRecessIndex)
        lTextMonthly.AppendLine(" REQUIREMENT OF ANT. RECESSION  =   " & PARAMAnteRecessDaysMin)
        lTextMonthly.AppendLine(" PROGRAM VERSION = JANUARY 2007 ")
        lTextMonthly.AppendLine("   ")
        lTextMonthly.AppendLine("                         MONTHLY RECHARGE IN INCHES:")
        lTextMonthly.Append("          J     F     M     A     M     J     J     A     S     O     N     D   YEAR")
        lSW.WriteLine(lTextMonthly.ToString) : lSW.Flush()
        lTextMonthly.Length = 0

        Dim lTextMonCSV As New Text.StringBuilder
        lTextMonCSV.AppendLine("Groundwater Toolbox monthly output for recharge estimates using RORA approach.")
        lTextMonCSV.AppendLine("Station: USGS " & lStationID & " " & lStationName)
        lTextMonCSV.AppendLine("Drainage area: " & lDA & " square miles")
        lTextMonCSV.AppendLine(" ")
        lTextMonCSV.AppendLine("(IN: monthly recharge for drainage area, in inches)")
        lTextMonCSV.AppendLine(" ")
        lTextMonCSV.AppendLine("Month,Date,IN ")
        Dim lSWMonCSV As New IO.StreamWriter(lFileRoraMonCSV, False)
        lSWMonCSV.WriteLine(lTextMonCSV) : lSWMonCSV.Flush()
        lTextMonCSV.Length = 0

        Dim lTextAnnCSV As New Text.StringBuilder
        lTextAnnCSV.AppendLine("Groundwater Toolbox monthly output for recharge estimates using RORA approach.")
        lTextAnnCSV.AppendLine("Station: USGS " & lStationID & " " & lStationName)
        lTextAnnCSV.AppendLine("Drainage area: " & lDA & " square miles")
        lTextAnnCSV.AppendLine(" ")
        lTextAnnCSV.AppendLine("(IN: annual recharge for drainage area, in inches)")
        lTextAnnCSV.AppendLine(" ")
        lTextAnnCSV.AppendLine("Year,Date,IN ")
        Dim lSWAnnCSV As New IO.StreamWriter(lFileRoraAnnCSV, False)
        lSWAnnCSV.WriteLine(lTextAnnCSV) : lSWAnnCSV.Flush()
        lTextAnnCSV.Length = 0

        'Get monthly sum
        Dim lNumYears As Integer = lYearEnd - lYearStart + 1
        Dim lYearByMonthRecharge(lNumYears, 12) As Double
        Dim laDateMonthStart As Double
        Dim laDateMonthEnd As Double
        Dim lMonthComplete As Boolean
        For I As Integer = 0 To listOfSegments.Count - 1
            lSeg = listOfSegments.ItemByIndex(I)
            lMonthComplete = True
            laDateMonthStart = Date2J(lSeg.PeakDayYear, lSeg.PeakDayMonth, 1, 0, 0, 0)
            laDateMonthEnd = Date2J(lSeg.PeakDayYear, lSeg.PeakDayMonth, DayMon(lSeg.PeakDayYear, lSeg.PeakDayMonth), 24, 0, 0)
            If pStartDate > laDateMonthStart OrElse pEndDate < laDateMonthEnd Then
                lMonthComplete = False
            End If
            If lMonthComplete Then
                lYearByMonthRecharge(lSeg.PeakDayYear - lYearStart + 1, lSeg.PeakDayMonth) += lSeg.Recharge
            Else
                lYearByMonthRecharge(lSeg.PeakDayYear - lYearStart + 1, lSeg.PeakDayMonth) = 0
            End If
        Next

        Dim lSumYear As Double = 0
        Dim liMiss As Integer = 0
        Dim lTotXX As Double = 0
        Dim lAA As Double
        Dim lBB As Double
        Dim lCC As Double
        Dim lDD As Double
        Dim lMonthCount As Integer = 1
        For liYear As Integer = 1 To lNumYears
            lSumYear = 0
            liMiss = 0
            Dim liMonth As Integer
            For liMonth = 1 To 12
                lSumYear += lYearByMonthRecharge(liYear, liMonth)
                If lYearByMonthRecharge(liYear, liMonth) = 0 Then
                    liMiss = 1
                Else
                    lTotXX += lYearByMonthRecharge(liYear, liMonth)
                End If
            Next
            If liMiss = 1 Then lSumYear = -99.99

            lTextAnnCSV.AppendLine(liYear & "," & liYear + lYearStart - 1 & "," & String.Format("{0:0.00}", lSumYear))

            ' 943 FORMAT (1I6, 13F6.2) - write to 14 monthly
            lTextMonthly.Append((liYear + lYearStart - 1).ToString.PadLeft(6, " "))

            Dim lMonthlyRechargeValue As Double
            For liMonth = 1 To 12
                lMonthlyRechargeValue = lYearByMonthRecharge(liYear, liMonth)
                If lMonthlyRechargeValue = 0 Then lMonthlyRechargeValue = -99.99
                lTextMonthly.Append(String.Format("{0:0.00}", lMonthlyRechargeValue).PadLeft(6, " "))

                lTextMonCSV.AppendLine(lMonthCount & "," & liMonth & "-" & liYear + lYearStart - 1 & "," & String.Format("{0:0.00}", lMonthlyRechargeValue))
                lMonthCount += 1
            Next
            lTextMonthly.AppendLine(String.Format("{0:0.00}", lSumYear).PadLeft(6, " "))

            lAA = lYearByMonthRecharge(liYear, 1) + lYearByMonthRecharge(liYear, 2) + lYearByMonthRecharge(liYear, 3)
            If lYearByMonthRecharge(liYear, 1) = 0 OrElse _
               lYearByMonthRecharge(liYear, 2) = 0 OrElse _
               lYearByMonthRecharge(liYear, 3) = 0 Then
                lAA = -99.99
            End If
            lBB = lYearByMonthRecharge(liYear, 4) + lYearByMonthRecharge(liYear, 5) + lYearByMonthRecharge(liYear, 6)
            If lYearByMonthRecharge(liYear, 4) = 0 OrElse _
               lYearByMonthRecharge(liYear, 5) = 0 OrElse _
               lYearByMonthRecharge(liYear, 6) = 0 Then
                lBB = -99.99
            End If
            lCC = lYearByMonthRecharge(liYear, 7) + lYearByMonthRecharge(liYear, 8) + lYearByMonthRecharge(liYear, 9)
            If lYearByMonthRecharge(liYear, 7) = 0 OrElse _
               lYearByMonthRecharge(liYear, 8) = 0 OrElse _
               lYearByMonthRecharge(liYear, 9) = 0 Then
                lCC = -99.99
            End If

            If liYear > 1 Then
                Dim lWY As Double = lDD + lAA + lBB + lCC
                If lAA < 0 OrElse lBB < 0 OrElse lCC < 0 OrElse lDD < 0 Then lWY = -99.99
                '945 format ('Oct ',1i4,' to Sept ',1i4,3x,5f8.2)
                lTextWY.AppendLine("Oct " & lYearStart + liYear - 2 & " to Sept " & lYearStart + liYear - 1 & String.Format("{0:0.00}", lWY).PadLeft(11, " "))
            End If

            lDD = lYearByMonthRecharge(liYear, 10) + lYearByMonthRecharge(liYear, 11) + lYearByMonthRecharge(liYear, 12)
            If lYearByMonthRecharge(liYear, 10) = 0 OrElse _
               lYearByMonthRecharge(liYear, 11) = 0 OrElse _
               lYearByMonthRecharge(liYear, 12) = 0 Then
                lDD = -99.99
            End If
            '944 FORMAT (1I6, 5F8.2) Qrt15
            lTextQrt.Append((liYear + lYearStart - 1).ToString.PadLeft(6, " "))
            lTextQrt.Append(String.Format("{0:0.00}", lAA).PadLeft(8, " "))
            lTextQrt.Append(String.Format("{0:0.00}", lBB).PadLeft(8, " "))
            lTextQrt.Append(String.Format("{0:0.00}", lCC).PadLeft(8, " "))
            lTextQrt.Append(String.Format("{0:0.00}", lDD).PadLeft(8, " "))
            lTextQrt.AppendLine(String.Format("{0:0.00}", lSumYear).PadLeft(8, " "))

        Next 'liYear

        lTextMonthly.AppendLine("  ")
        lTextMonthly.AppendLine("                 TOTAL OF MONTHLY AMOUNTS  = " & String.Format("{0:0.00}", lTotXX).PadLeft(8, " "))
        lTextMonthly.AppendLine("  ")
        lTextMonthly.AppendLine("  RESULTS ON THE MONTHLY TIME SCALE SHOULD BE USED WITH CAUTION.")
        lTextMonthly.AppendLine("  FILES RORAQRT.TXT AND RORASUM.TXT GIVE RESULTS AT THE CORRECT")
        lTextMonthly.AppendLine("  TIME SCALES (QUARTER YEAR, YEAR, OR MORE).")

        lSW.WriteLine(lTextMonthly.ToString) : lSW.Flush() : lSW.Close() : lSW = Nothing
        lSWQrt.WriteLine(lTextQrt.ToString) : lSWQrt.Flush() : lSWQrt.Close() : lSWQrt = Nothing
        lSWWY.WriteLine(lTextWY.ToString) : lSWWY.Flush() : lSWWY.Close() : lSWWY = Nothing
        lSWMonCSV.WriteLine(lTextMonCSV.ToString) : lSWMonCSV.Flush() : lSWMonCSV.Close() : lSWMonCSV = Nothing
        lSWAnnCSV.WriteLine(lTextAnnCSV.ToString) : lSWAnnCSV.Flush() : lSWAnnCSV.Close() : lSWAnnCSV = Nothing

        Bulletin = "NUMBER OF PEAKS= " & listOfSegments.Count & vbCrLf
        Bulletin &= "SUMMARY RESULTS WRITTEN TO FILE ""rorasum.txt""" & vbCrLf
        Bulletin &= "MONTHLY RESULTS ARE WRITTEN TO FILE ""roramon.txt""" & vbCrLf
        Bulletin &= "...AND QUARTER-YEAR RESULTS TO FILE ""roraqrt.txt"""
    End Sub

    ''' <summary>
    ''' Set the monthly or yearly recharge timeseries
    ''' </summary>
    ''' <param name="aTimeStep">atcTimeUnit</param>
    ''' <remarks>Needs to be either for monthly or yearly</remarks>
    Public Sub GetRechargeTimeseries(ByVal aTimeStep As atcTimeUnit)
        If listOfSegments Is Nothing OrElse listOfSegments.Count = 0 Then
            Exit Sub
        End If

        Dim lSeg As clsRecessionSegment = Nothing
        Dim lDates As New List(Of Double)
        Dim lRechargeValues As New List(Of Double)

        'For I As Integer = 0 To listOfSegments.Count - 1
        '    lSeg = listOfSegments.ItemByIndex(I)
        '    lDates.Add(lSeg.PeakDayDate)
        '    lRechargeValues.Add(lSeg.Recharge)
        'Next

        'Get monthly sum
        Dim lDate(5) As Integer
        J2Date(pStartDate, lDate)
        Dim lYearStart As Integer = lDate(0)
        J2Date(pEndDate - JulianHour, lDate)
        Dim lYearEnd As Integer = lDate(0)
        Dim lNumYears As Integer = lYearEnd - lYearStart + 1
        Dim lYearByMonthRecharge(lNumYears, 12) As Double
        Dim laDateMonthStart As Double
        Dim laDateMonthEnd As Double
        Dim lMonthComplete As Boolean
        For I As Integer = 0 To listOfSegments.Count - 1
            lSeg = listOfSegments.ItemByIndex(I)
            lMonthComplete = True
            laDateMonthStart = Date2J(lSeg.PeakDayYear, lSeg.PeakDayMonth, 1, 0, 0, 0)
            laDateMonthEnd = Date2J(lSeg.PeakDayYear, lSeg.PeakDayMonth, DayMon(lSeg.PeakDayYear, lSeg.PeakDayMonth), 24, 0, 0)
            If pStartDate > laDateMonthStart OrElse pEndDate < laDateMonthEnd Then
                lMonthComplete = False
            End If
            If lMonthComplete Then
                lYearByMonthRecharge(lSeg.PeakDayYear - lYearStart + 1, lSeg.PeakDayMonth) += lSeg.Recharge
            Else
                lYearByMonthRecharge(lSeg.PeakDayYear - lYearStart + 1, lSeg.PeakDayMonth) = 0
            End If
        Next

        Dim lYear As Integer
        Dim lDateDouble As Double
        Dim lValue As Double
        Dim lBeginningTimeAdjusted As Boolean = False
        For lYearCount As Integer = 1 To lNumYears
            lYear = lYearCount + lYearStart - 1
            For lMonth As Integer = 1 To 12
                If Not lBeginningTimeAdjusted Then
                    Dim lPrevMonth As Integer
                    Dim lPrevYear As Integer
                    If lMonth = 1 Then
                        lPrevMonth = 12
                        lPrevYear = lYear - 1
                    Else
                        lPrevMonth = lMonth - 1
                        lPrevYear = lYear
                    End If
                    lDateDouble = Date2J(lPrevYear, lPrevMonth, DayMon(lPrevYear, lPrevMonth), 24, 0, 0)
                    lValue = Double.NaN
                    lDates.Add(lDateDouble)
                    lRechargeValues.Add(lValue)
                    lBeginningTimeAdjusted = True
                End If
                lDateDouble = Date2J(lYear, lMonth, DayMon(lYear, lMonth), 24, 0, 0)
                lValue = lYearByMonthRecharge(lYearCount, lMonth)
                If lValue > 0 Then
                    lDates.Add(lDateDouble)
                    lRechargeValues.Add(lValue)
                End If
            Next
        Next

        pTsRecharge = New atcTimeseries(Nothing)
        Dim lTsDates As New atcTimeseries(Nothing)
        lTsDates.Values = lDates.ToArray()
        pTsRecharge.Values = lRechargeValues.ToArray()
        pTsRecharge.Dates = lTsDates

        If aTimeStep = atcTimeUnit.TUYear Then
            pTsRecharge = Aggregate(pTsRecharge, aTimeStep, 1, atcTran.TranSumDiv)
            pTsRecharge.SetInterval(aTimeStep, 1)
        Else
            pTsRecharge.SetInterval(atcTimeUnit.TUMonth, 1)
        End If
        
        'pTsRecharge.Attributes.SetValue("point", True)

    End Sub

    Public Sub Rora(ByVal Args As atcDataAttributes)

        If Args Is Nothing Then Exit Sub
        'Get parameters
        pDrainageArea = Args.GetValue("Drainage Area")
        PARAMRecessIndex = Args.GetValue("Recession Index")
        PARAMAnteRecessDays = Args.GetValue("Antecedent Recession")
        Dim lStart As Double = Args.GetValue("Start Date")
        Dim lEnd As Double = Args.GetValue("End Date")
        TsStreamFlow = Args.GetValue("Streamflow")(0)
        Dim lTsStreamFlow As atcTimeseries = Nothing
        If lStart > 0 AndAlso lEnd > 0 AndAlso lEnd > lStart Then
            lTsStreamFlow = SubsetByDate(TsStreamFlow, lStart, lEnd, Nothing)
            pStartDate = lStart
            pEndDate = lEnd
        Else
            Exit Sub
        End If

        Dim lEnglishUnit As Boolean = Args.GetValue("EnglishUnit")
        If listOfSegments Is Nothing Then
            listOfSegments = New atcCollection()
        Else
            listOfSegments.Clear()
        End If

        Dim lDate(5) As Integer
        Dim liRecMax As Integer = MaxNumDaysAfterPeak(PARAMRecessIndex)
        If liRecMax < PARAMAnteRecessDaysMin Then liRecMax = PARAMAnteRecessDaysMin

        Dim lTotalDayCount As Integer = lTsStreamFlow.numValues
        ' FINDING DAYS OF SUFFICIENT ANTECEDENT RECESSION'
        Dim lALLGW() As String : ReDim lALLGW(lTotalDayCount)
        For Z As Integer = 1 To lTotalDayCount
            lALLGW(Z) = " "
        Next
        Dim lDayCount As Integer
        For lDayCount = PARAMAnteRecessDaysMin + 1 To lTotalDayCount 'loop 270
            Dim lIndicator As Integer = 1
            Dim liBack As Integer = 0
            While True 'loop 260
                liBack += 1
                If lTsStreamFlow.Value(lDayCount - liBack) < lTsStreamFlow.Value(lDayCount - liBack + 1) Then lIndicator = 0
                If liBack >= PARAMAnteRecessDaysMin Then Exit While
            End While 'loop 260
            If lIndicator = 1 Then
                lALLGW(lDayCount) = "*"
            Else
                lALLGW(lDayCount) = " "
            End If
        Next 'loop 270

        ' Determine the location (in time) of peaks and recession periods

        ' Locate end of first recession
        lDayCount = 0
        While True 'loop 280
            lDayCount += 1
            If lDayCount > lTotalDayCount Then
                Bulletin = "No Recession segments found." & vbCrLf & "Rora Ends."
                Exit Sub
            End If
            If lALLGW(lDayCount) = "*" Then Exit While
        End While 'loop 280

        While True 'loop 290
            lDayCount += 1
            If lDayCount > lTotalDayCount Then
                Bulletin = "Cannot find end of the first recession." & vbCrLf & "Rora Ends."
                Exit Sub
            End If
            If lALLGW(lDayCount) = " " Then Exit While
        End While 'loop 290

        lDayCount -= 1
        Dim lAnteDay As Integer = lDayCount
        Dim lAnteDayFlow As Double = lTsStreamFlow.Value(lDayCount)
        piPeak = 0
        ' LOCATING PEAKS + REMAINING RECESSION PERIODS '
        Dim liLook As Integer
        While True 'loop 330
            piPeak += 1
            If piPeak > 6000 Then
                Exit While 'exit sub Too many peaks
            End If
            RecessionSegment = New clsRecessionSegment()
            With RecessionSegment
                .PeakDayDate = lTsStreamFlow.Dates.Value(lDayCount - 1)
                .PeakDayFlow = lTsStreamFlow.Value(lDayCount) 'QP(IPEAK) = Q1D(I)
                .PeakDayIndex = lDayCount 'TP(IPEAK) = I
                If listOfSegments.Count = 0 Then
                    .TA = lAnteDay
                    .QA = lAnteDayFlow
                End If
            End With

            liLook = lDayCount + 1
            Dim lDoneLookingForPeaks As Boolean = False
            While True 'loop 340
                If liLook >= lTotalDayCount Then
                    lDoneLookingForPeaks = True
                    Exit While 'loop 340
                End If
                If lALLGW(liLook) = "*" Then Exit While 'loop 340

                If lTsStreamFlow.Value(liLook) >= RecessionSegment.PeakDayFlow Then
                    RecessionSegment.PeakDayDate = lTsStreamFlow.Dates.Value(liLook - 1)
                    RecessionSegment.PeakDayFlow = lTsStreamFlow.Value(liLook)
                    RecessionSegment.PeakDayIndex = liLook
                End If
                If lALLGW(liLook) = " " Then
                    liLook += 1
                Else
                    Exit While 'loop 340
                End If
            End While 'loop 340
            If lDoneLookingForPeaks Then Exit While 'loop 330

            RecessionSegment.IndexAfterTimeCritical = RecessionSegment.PeakDayIndex + TimeCritical

            '-----   FIND FIRST AND LAST DAYS OF RECESSION FOLLOWING THE PEAK: ----

            lDayCount = liLook
            RecessionSegment.TS = lDayCount

            While True 'loop 370
                lDayCount += 1
                If lDayCount > lTotalDayCount Then Exit While 'loop 370
                If lALLGW(lDayCount) = " " Then Exit While 'loop 370
            End While 'loop 370

            lDayCount -= 1
            RecessionSegment.TE = lDayCount
            If RecessionSegment.TE - RecessionSegment.PeakDayIndex > liRecMax Then
                RecessionSegment.TE = RecessionSegment.PeakDayIndex + liRecMax
            End If
            If RecessionSegment.TE < RecessionSegment.TS Then
                RecessionSegment.TE = RecessionSegment.TS
            End If

            J2Date(RecessionSegment.PeakDayDate, lDate)
            Dim lKey As String = lDate(0).ToString & "/" & lDate(1).ToString.PadLeft(2, " ") & "/" & lDate(2).ToString.PadLeft(2, " ")
            listOfSegments.Add(lKey, RecessionSegment)

            lDayCount += 1
            If lDayCount >= lTotalDayCount Then Exit While 'loop 330
        End While 'loop 330

        '---- By now, ALL RECESSION PERIODS AND PEAKS HAVE BEEN LOCATED: ----

        'remove the last segment
        Dim lNPeaks As Integer = piPeak - 1
        RecessionSegment = listOfSegments(listOfSegments.Count - 1)
        Dim lEndDateofLastSegTE As Double = TimAddJ(RecessionSegment.PeakDayDate, atcTimeUnit.TUDay, 1, RecessionSegment.TE - RecessionSegment.TS + 1)
        If lEndDateofLastSegTE > pEndDate Then
            listOfSegments.RemoveAt(listOfSegments.Count - 1)
        End If

        Bulletin = "Number of Peaks=" & listOfSegments.Count

        RechargeTotal = 0
        ' --- EXTRAPOLATE STREAMFLOW, DETERMINE RECESSION CURVE DISPLACEMENT, AND --
        ' ----------------- CALCULATE RECHARGE FOR FIRST PEAK:  --------------------
        RecessionSegment = listOfSegments(0)
        With RecessionSegment
            .QB = .QA * (10 ^ (-1 * (.IndexAfterTimeCritical - .TA) / PARAMRecessIndex))
            Dim lSum As Double = 0.0
            For I As Integer = .TS To .TE 'loop 810
                Dim lDQ As Double = lTsStreamFlow.Value(I) - .QA * (10 ^ (-1 * (I - .TA) / PARAMRecessIndex))
                lSum += lDQ * Math.Sqrt(I - .PeakDayIndex)
            Next 'loop 810

            Dim lNRecess As Double = .TE - .TS + 1
            .C = lSum / lNRecess
            .DeltaQ = .C / Math.Sqrt(.IndexAfterTimeCritical - .PeakDayIndex)
            .Recharge = 2.0 * pUADepth * .DeltaQ * PARAMRecessIndex / (pLog10ToNaturalLog * pDrainageArea)
            RechargeTotal += .Recharge
            .QC = .QB + .DeltaQ
        End With

        ' -- EXTRAPOLATE STREAMFLOW, DETERMINE RECESSION CURVE DISPLACEMENT, AND --
        ' --------------- CALCULATE RECHARGE FOR ALL OTHER PEAKS: -----------------
        For iPeakCount As Integer = 1 To listOfSegments.Count - 1
            RecessionSegment = listOfSegments(iPeakCount)
            Dim lprevRecess As clsRecessionSegment = listOfSegments(iPeakCount - 1)
            With RecessionSegment
                .QA = lprevRecess.QC
                .TA = lprevRecess.IndexAfterTimeCritical
                .QB = .QA * (10 ^ (-1 * (.IndexAfterTimeCritical - .TA) / PARAMRecessIndex))
                Dim lSum As Double = 0.0
                Dim lBaseLine As Double
                For I As Integer = .TS To .TE 'loop 820
                    If I > .TA Then
                        lBaseLine = .QA * 10 ^ (-1 * (I - .TA) / PARAMRecessIndex)
                    Else
                        With lprevRecess
                            lBaseLine = .C / Math.Sqrt(I - .PeakDayIndex) + .QA * 10 ^ (-1 * (I - .TA) / PARAMRecessIndex)
                        End With
                    End If
                    Dim lDQ As Double = lTsStreamFlow.Value(I) - lBaseLine
                    lSum += lDQ * Math.Sqrt(I - .PeakDayIndex)
                Next 'loop 820

                Dim lNRecess As Double = .TE - .TS + 1
                .C = lSum / lNRecess
                .DeltaQ = .C / Math.Sqrt(.IndexAfterTimeCritical - .PeakDayIndex)
                .Recharge = 2.0 * pUADepth * .DeltaQ * PARAMRecessIndex / (pLog10ToNaturalLog * pDrainageArea)
                .QC = .QB + .DeltaQ
                RechargeTotal += .Recharge
            End With
        Next
    End Sub

    ''' <summary>
    '''C  ---  DETERMINE THE MINIMUM NUMBER OF DAYS OF ANTECEDENT RECESSION  --
    '''C  ---  TO INDICATE THAT STREAMFLOW IS TO BE CONSIDERED GROUND-WATER  --
    '''C  ---  DISCHARGE.  OBTAIN FROM THE EQUATION DA**0.2, ROUNDED UPWARD  -
    ''' </summary>
    ''' <param name="aDA">Drainage Area in Sq mi</param>

    Public Sub ITBase(ByVal aDA As Double)
        PARAMAnteRecessDaysMin = 0
        FatalError = False
        While True 'loop 210
            PARAMAnteRecessDaysMin += 1
            If PARAMAnteRecessDaysMin > 10 Then
                FatalError = True
                Exit While
            End If
            If PARAMAnteRecessDaysMin > aDA ^ 0.2 Then Exit While
        End While 'loop 210

        'set the range of allowable antecedent recession days
        If RangeOfAnteRecessionDays IsNot Nothing Then
            RangeOfAnteRecessionDays.Clear()
        Else
            RangeOfAnteRecessionDays = New ArrayList()
        End If

        If Not FatalError Then
            With RangeOfAnteRecessionDays
                .Add(PARAMAnteRecessDaysMin)
                .Add(PARAMAnteRecessDaysMin + 1)
                .Add(PARAMAnteRecessDaysMin + 2)
                .Add(PARAMAnteRecessDaysMin + 3)
            End With
        End If
    End Sub

    ''' <summary>
    '''C ----- SPECIFY THE MAXIMUM ALLOWABLE NUMBER OF DAYS THAT CAN BE USED
    '''C ----- AFTER A PEAK, TO DETERMINE THE GROUND-WATER DISCHARGE AFTER
    '''C ----- THE PEAK:
    ''' </summary>
    ''' <param name="aRecessIndex">Recession Index</param>
    ''' <returns>Integer (days)</returns>
    ''' <remarks></remarks>
    Public Function MaxNumDaysAfterPeak(ByVal aRecessIndex As Double) As Integer
        Return Math.Floor(0.2144 * aRecessIndex)
    End Function
End Class
