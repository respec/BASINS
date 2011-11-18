Imports atcData
Imports atcUtility
Imports MapWinUtility

Module modBaseflowUtil

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
    ''' this is the HySEP monthly ASCII output
    ''' </summary>
    ''' <param name="aTs">Streamflow timeseries with baseflow group as an attribute</param>
    ''' <param name="aFilename">.BSF output filename</param>
    ''' <remarks></remarks>
    Public Sub ASCIIHySepMonthly(ByVal aTs As atcTimeseries, ByVal aFilename As String, Optional ByVal aBFName As String = "")
        Dim lSW As IO.StreamWriter = Nothing
        Try
            lSW = New IO.StreamWriter(aFilename, False)
        Catch ex As Exception
            Logger.Dbg("Problem opening file: " & aFilename)
            Exit Sub
        End Try

        Dim lTsBF As atcTimeseries = Nothing
        Dim lTsFlow As atcTimeseries = Nothing
        If aBFName = "" Then aBFName = "HySep"
        For Each lTs As atcTimeseries In aTs.Attributes.GetDefinedValue("Baseflow").Value
            If lTs.Attributes.GetValue("Scenario").ToString.StartsWith(aBFName) Then
                lTsBF = lTs
                lTsFlow = SubsetByDate(aTs, lTs.Dates.Value(0), lTs.Dates.Value(lTs.numValues), Nothing)
                Exit For
            End If
        Next

        If lTsBF Is Nothing Then
            Logger.Dbg("No baseflow data found.")
            Exit Sub
        End If

        Dim lEnglishUnit As Boolean = True
        Dim lDate(5) As Integer
        'English Unit: flow in cfs, depth in inch, drainage area in square miles
        'Metric Unit: flow in m3/s, depth in centimeter, drainage area in square km
        '1 second-foot for one day covers 1 square mile 0.03719 inch deep (Water Supply Paper by USGS)
        '1 cfs = 0.6462 M gal/d (flow rate conversion)
        Dim lDA As Double = lTsBF.Attributes.GetValue("DrainageArea", 1.0)
        Dim lTsFlowDepthPUA As atcTimeseries = Nothing
        Dim lTsFlowVolumePUA As atcTimeseries = Nothing
        Dim lTsFlowRatePUA As atcTimeseries = Nothing
        'Dim lTsFlowVolume As atcTimeseries = Nothing

        If lEnglishUnit Then
            lTsFlowDepthPUA = lTsFlow * (0.03719 / lDA) '-> inch
            lTsFlowVolumePUA = lTsFlow * (0.6462 / lDA) '-> Mgal/d/mi2
            'lTsFlowVolume = aTs * 0.6462
        Else
            lTsFlowDepthPUA = lTsFlow * (8.64 / lDA) '-> centimeter
        End If
        lTsFlowRatePUA = lTsFlow / lDA

        Dim lTsBFDepthPUA As atcTimeseries = Nothing
        Dim lTsBFVolumePUA As atcTimeseries = Nothing
        Dim lTsBFRatePUA As atcTimeseries = Nothing
        Dim lTsBFRateVolume As atcTimeseries = Nothing
        If lEnglishUnit Then
            lTsBFDepthPUA = lTsBF * (0.03719 / lDA) '-> inch
            lTsBFVolumePUA = lTsBF * (0.6462 / lDA) '-> Mgal/d/mi2
            lTsBFRateVolume = lTsBF * 0.6462
        Else
            lTsBFDepthPUA = lTsBF * (8.64 / lDA) '-> centimeter
        End If
        lTsBFRatePUA = lTsBF / lDA

        'Dim lTsBFPct As atcTimeseries = (lTsBF * 100.0) / aTs

        'aggregate into monthly values
        Dim lMonthlyCo1RateFlow As atcTimeseries = Aggregate(lTsFlow, atcTimeUnit.TUMonth, 1, atcTran.TranAverSame)
        Dim lMonthlyCo2RateBF As atcTimeseries = Aggregate(lTsBF, atcTimeUnit.TUMonth, 1, atcTran.TranAverSame)
        Dim lMonthlyCo3RateRO As atcTimeseries = Aggregate(lTsFlow - lTsBF, atcTimeUnit.TUMonth, 1, atcTran.TranAverSame)
        Dim lMonthlyCo4DepthFlowPUA As atcTimeseries = Aggregate(lTsFlowDepthPUA, atcTimeUnit.TUMonth, 1, atcTran.TranSumDiv)
        Dim lMonthlyCo5DepthBFPUA As atcTimeseries = Aggregate(lTsBFDepthPUA, atcTimeUnit.TUMonth, 1, atcTran.TranSumDiv)
        Dim lMonthlyCo6DepthROPUA As atcTimeseries = Aggregate(lTsFlowDepthPUA - lTsBFDepthPUA, atcTimeUnit.TUMonth, 1, atcTran.TranSumDiv)
        'Dim lMonthlyVolumeFlowPUA As atcTimeseries = Aggregate(lTsFlowVolumePUA, atcTimeUnit.TUMonth, 1, atcTran.TranSumDiv)
        Dim lMonthlyCo8RateBFPUA As atcTimeseries = Aggregate(lTsBFRatePUA, atcTimeUnit.TUMonth, 1, atcTran.TranAverSame)
        Dim lMonthlyCo9RateBFVolumePUA As atcTimeseries = Aggregate(lTsBFVolumePUA, atcTimeUnit.TUMonth, 1, atcTran.TranSumDiv)
        Dim lMonthlyCo10RateBFVolume As atcTimeseries = Aggregate(lTsBFRateVolume, atcTimeUnit.TUMonth, 1, atcTran.TranSumDiv)

        Dim lSnMonth As New atcSeasonsMonth
        Dim lSnCo1RateFlow As atcTimeseriesGroup = lSnMonth.Split(lMonthlyCo1RateFlow, Nothing)
        Dim lSnCo2RateBF As atcTimeseriesGroup = lSnMonth.Split(lMonthlyCo2RateBF, Nothing)
        Dim lSnCo3RateRO As atcTimeseriesGroup = lSnMonth.Split(lMonthlyCo3RateRO, Nothing)
        Dim lSnCo4DepthFlowPUA As atcTimeseriesGroup = lSnMonth.Split(lMonthlyCo4DepthFlowPUA, Nothing)
        Dim lSnCo5DepthBFPUA As atcTimeseriesGroup = lSnMonth.Split(lMonthlyCo5DepthBFPUA, Nothing)
        Dim lSnCo6DepthROPUA As atcTimeseriesGroup = lSnMonth.Split(lMonthlyCo6DepthROPUA, Nothing)
        Dim lSnCo8RateBFPUA As atcTimeseriesGroup = lSnMonth.Split(lMonthlyCo8RateBFPUA, Nothing)
        Dim lSnCo9RateBFVolumePUA As atcTimeseriesGroup = lSnMonth.Split(lMonthlyCo9RateBFVolumePUA, Nothing)
        Dim lSnCo10RateBFVolume As atcTimeseriesGroup = lSnMonth.Split(lMonthlyCo10RateBFVolume, Nothing)

        '2100
        Dim lHeader2100 As String = "" & _
     Space(6) & "   Mean       Mean       Mean     Total   Total   Total    BF/     Base" & _
     Space(6) & "  stream-     base      surface  stream-   base  surface stream-   flow" & _
     Space(6) & "   flow       flow      runoff    flow     flow   runoff  flow   (Mgal/d/" & _
     Space(6) & "  (ft3/s)    (ft3/s)    (ft3/s)    (in)    (in)    (in)    (%)      mi2)" & _
     Space(6) & "---------- ---------- ---------- ------- ------- -------  ----- ----------"

        '2110
        Dim lHeader2110 As String = "" & _
     Space(6) & "   Mean       Mean       Mean     Total   Total   Total    BF/" & _
     Space(6) & "  stream-     base      surface  stream-   base  surface stream-   Base" & _
     Space(6) & "   flow       flow      runoff    flow     flow   runoff  flow     flow" & _
     Space(6) & "  (ft3/s)    (ft3/s)    (ft3/s)    (in)    (in)    (in)    (%)   (Mgal/d)" & _
     Space(6) & "---------- ---------- ---------- ------- ------- -------  ----- ----------"

        '2120
        Dim lHeader2120 As String = "" & _
     Space(6) & "   Mean       Mean       Mean     Total   Total   Total    BF/     Base" & _
     Space(6) & "  stream-     base      surface  stream-   base  surface stream-   flow" & _
     Space(6) & "   flow       flow      runoff    flow     flow   runoff  flow    (ft3/s/" & _
     Space(6) & "  (ft3/s)    (ft3/s)    (ft3/s)    (in)    (in)    (in)    (%)      mi2)" & _
     Space(6) & "---------- ---------- ---------- ------- ------- -------  ----- ----------"

        '2130
        Dim lHeader2130 As String = "" & _
     Space(6) & "   Mean       Mean       Mean     Total   Total   Total    BF/     Base" & _
     Space(6) & "  stream-     base      surface  stream-   base  surface stream-   flow" & _
     Space(6) & "   flow       flow      runoff    flow     flow   runoff  flow    (m3/s/" & _
     Space(6) & "  (m3/s)     (m3/s)     (m3/s)     (cm)    (cm)    (cm)    (%)      km2)" & _
     Space(6) & "---------- ---------- ---------- ------- ------- -------  ----- ----------"

        Dim lHeaderEngAll As String = "" & _
     Space(6) & "   Mean       Mean       Mean     Total   Total   Total    BF/     Base       Base" & _
     Space(6) & "  stream-     base      surface  stream-   base  surface stream-   flow       flow       Base" & _
     Space(6) & "   flow       flow      runoff    flow     flow   runoff  flow    (ft3/s/   (Mgal/d/     flow" & _
     Space(6) & "  (ft3/s)    (ft3/s)    (ft3/s)    (in)    (in)    (in)    (%)      mi2)       mi2)    (Mgal/d)" & _
     Space(6) & "---------- ---------- ---------- ------- ------- -------  ----- ---------- ---------- ----------"

        '2150 FORMAT (A5,3F11.2,3F8.3,F7.2,F11.3)
        Dim lMonthNames() As String = {"Dummy", "Jan. ", "Feb. ", "Mar. ", "Apr. ", _
                                                "May  ", "June ", "July ", "Aug. ", _
                                                "Sept.", "Oct. ", "Nov. ", "Dec. "}

        'construct table
        Dim lCo0MonthName As String = ""
        Dim lCo1RateFlow As String = ""
        Dim lCo2RateBF As String = ""
        Dim lCo3RateRO As String = ""
        Dim lCo4DepthTotalflow As String = ""
        Dim lCo5DepthTotalBF As String = ""
        Dim lCo6DepthTotalRO As String = ""
        Dim lCo7BFPct As String = ""
        Dim lCo8RateBFPUA As String = ""
        Dim lCo9RateBFVolumePUA As String = ""
        Dim lCo10RateBFVolume As String = ""
        Dim lTable As New atcTableDelimited
        With lTable
            .Delimiter = ","
            .NumFields = 11
            .FieldLength(1) = 5
            .FieldName(1) = "Month"
            For I As Integer = 2 To 4
                .FieldLength(I) = 11
                Select Case I
                    Case 2 : .FieldName(I) = "FlowRate"
                    Case 3 : .FieldName(I) = "BaseflowRate"
                    Case 4 : .FieldName(I) = "RunoffRate"
                End Select
            Next
            For I As Integer = 5 To 7
                .FieldLength(I) = 8
                Select Case I
                    Case 5 : .FieldName(I) = "TotalFlowDepth"
                    Case 6 : .FieldName(I) = "TotalBaseflowDepth"
                    Case 7 : .FieldName(I) = "TotalRunoffDepth"
                End Select
            Next
            .FieldLength(8) = 7
            .FieldName(8) = "Baseflow%"

            For I As Integer = 9 To 11
                .FieldLength(I) = 11
                Select Case I
                    Case 9 : .FieldName(I) = "BaseflowRatePUA"
                    Case 10 : .FieldName(I) = "BaseflowRateVolumePUA"
                    Case 11 : .FieldName(I) = "BaseflowRateVolume"
                End Select
            Next

            'Start write out

            Dim lCo1Sum12MonthFlow As Double
            Dim lCo2Sum12MonthBF As Double
            Dim lCo3Sum12MonthRO As Double
            Dim lCo4Sum12MonthDepthFlow As Double
            Dim lCo5Sum12MonthDepthBF As Double
            Dim lCo6Sum12MonthDepthRO As Double
            Dim lCo8Sum12MonthRateBFPUA As Double
            Dim lCo9Sum12MonthRateBFVolumePUA As Double
            Dim lCo10Sum12MonthRateBFVolume As Double

            .CurrentRecord = 1
            For I As Integer = 0 To lMonthlyCo1RateFlow.numValues - 1

                If I + 1 Mod 12 = 0 Then
                    lCo1Sum12MonthFlow /= 12.0
                    lCo2Sum12MonthBF /= 12.0
                    lCo3Sum12MonthRO /= 12.0
                    Dim lBFPct As Double = lCo5Sum12MonthDepthBF / lCo4Sum12MonthDepthFlow * 100.0
                    lCo8Sum12MonthRateBFPUA /= 12.0
                    lCo9Sum12MonthRateBFVolumePUA /= 12.0
                    lCo10Sum12MonthRateBFVolume /= 12.0
                    J2Date(lMonthlyCo1RateFlow.Dates.Value(I), lDate)

                    .Value(1) = "E" & lDate(0)
                    .Value(2) = String.Format("{0:0.00}", lCo1Sum12MonthFlow)
                    .Value(3) = String.Format("{0:0.00}", lCo2Sum12MonthBF)
                    .Value(4) = String.Format("{0:0.00}", lCo3Sum12MonthRO)
                    .Value(5) = String.Format("{0:0.000}", lCo4Sum12MonthDepthFlow)
                    .Value(6) = String.Format("{0:0.000}", lCo5Sum12MonthDepthBF)
                    .Value(7) = String.Format("{0:0.000}", lCo6Sum12MonthDepthRO)
                    .Value(8) = String.Format("{0:0.00}", lBFPct)
                    .Value(9) = String.Format("{0:0.000}", lCo8Sum12MonthRateBFPUA)
                    .Value(10) = String.Format("{0:0.000}", lCo9Sum12MonthRateBFVolumePUA)
                    .Value(11) = String.Format("{0:0.000}", lCo10Sum12MonthRateBFVolume)

                    lCo1Sum12MonthFlow = 0.0
                    lCo2Sum12MonthBF = 0.0
                    lCo3Sum12MonthRO = 0.0
                    lCo4Sum12MonthDepthFlow = 0.0
                    lCo5Sum12MonthDepthBF = 0.0
                    lCo6Sum12MonthDepthRO = 0.0
                    lCo8Sum12MonthRateBFPUA = 0.0
                    lCo9Sum12MonthRateBFVolumePUA = 0.0
                    lCo10Sum12MonthRateBFVolume = 0.0

                    .CurrentRecord += 1
                End If

                J2Date(lMonthlyCo1RateFlow.Dates.Value(I), lDate)
                .Value(1) = String.Format("{0:0.00}", lMonthNames(lDate(1)))
                .Value(2) = String.Format("{0:0.00}", lMonthlyCo1RateFlow.Value(I + 1))
                .Value(3) = String.Format("{0:0.00}", lMonthlyCo2RateBF.Value(I + 1))
                .Value(4) = String.Format("{0:0.00}", lMonthlyCo3RateRO.Value(I + 1))
                .Value(5) = String.Format("{0:0.000}", lMonthlyCo4DepthFlowPUA.Value(I + 1))
                .Value(6) = String.Format("{0:0.000}", lMonthlyCo5DepthBFPUA.Value(I + 1))
                .Value(7) = String.Format("{0:0.000}", lMonthlyCo6DepthROPUA.Value(I + 1))
                .Value(8) = String.Format("{0:0.00}", CDbl(.Value(3)) / CDbl(.Value(2)) * 100.0)
                .Value(9) = String.Format("{0:0.000}", lMonthlyCo8RateBFPUA.Value(I + 1))
                .Value(10) = String.Format("{0:0.000}", lMonthlyCo9RateBFVolumePUA.Value(I + 1))
                .Value(11) = String.Format("{0:0.000}", lMonthlyCo10RateBFVolume.Value(I + 1))

                lCo1Sum12MonthFlow += lMonthlyCo1RateFlow.Value(I + 1)
                lCo2Sum12MonthBF += lMonthlyCo2RateBF.Value(I + 1)
                lCo3Sum12MonthRO += lMonthlyCo3RateRO.Value(I + 1)
                lCo4Sum12MonthDepthFlow += lMonthlyCo4DepthFlowPUA.Value(I + 1)
                lCo5Sum12MonthDepthBF += lMonthlyCo5DepthBFPUA.Value(I + 1)
                lCo6Sum12MonthDepthRO += lMonthlyCo6DepthROPUA.Value(I + 1)
                lCo8Sum12MonthRateBFPUA += lMonthlyCo8RateBFPUA.Value(I + 1)
                lCo9Sum12MonthRateBFVolumePUA += lMonthlyCo9RateBFVolumePUA.Value(I + 1)
                lCo10Sum12MonthRateBFVolume += lMonthlyCo10RateBFVolume.Value(I + 1)

                .CurrentRecord += 1
            Next
            'For I As Integer = 1 To 12
            '    .CurrentRecord = I
            '    .Value(1) = lMonthNames(I)
            '    .Value(2) = lSnCo1RateFlow(I - 1).Attributes.GetValue("Mean")
            '    .Value(3) = lSnCo2RateBF(I - 1).Attributes.GetValue("Mean")
            '    .Value(4) = lSnCo3RateRO(I - 1).Attributes.GetValue("Mean")
            '    .Value(5) = lSnCo4DepthFlowPUA(I - 1).Attributes.GetValue("Mean")
            '    .Value(6) = lSnCo5DepthBFPUA(I - 1).Attributes.GetValue("Mean")
            '    .Value(7) = lSnCo6DepthROPUA(I - 1).Attributes.GetValue("Mean")
            '    .Value(8) = CDbl(.Value(3)) / CDbl(.Value(2)) * 100.0
            '    .Value(9) = lSnCo8RateBFPUA(I - 1).Attributes.GetValue("Mean")
            '    .Value(10) = lSnCo9RateBFVolumePUA(I - 1).Attributes.GetValue("Mean")
            '    .Value(11) = lSnCo10RateBFVolume(I - 1).Attributes.GetValue("Mean")
            'Next
        End With

        Dim lStation As String = aTs.Attributes.GetValue("STAID", "")
        If lStation = "" Then lStation = aTs.Attributes.GetValue("Location", "")
        Dim lUnitArea As String = "square miles"
        If Not lEnglishUnit Then lUnitArea = "square kilometers"

        J2Date(lTsBF.Dates.Value(0), lDate)
        Dim lStartYear As String = lDate(0)
        Dim lStartMonth As Integer = lDate(1)
        J2Date(lTsBF.Dates.Value(lTsBF.numValues - 1), lDate)
        Dim lEndYear As String = lDate(0)
        Dim lEndMonth As Integer = lDate(1)
        Dim lBFInterval As Double = lTsBF.Attributes.GetValue("BFInterval", 0.0)

        Dim lOneLine As String = lTsBF.Attributes.GetValue("Scenario", "")
        If lOneLine.Length > 0 Then lOneLine = lOneLine.Substring("HySep".Length)
        lSW.WriteLine(Space(80).Replace(" ", "-"))
        lSW.WriteLine("Hydrograph separation by the " & lOneLine.PadRight(23, " "))
        lSW.WriteLine("Station ID = " & lStation & " Drainage Area = " & String.Format("{0:0.00}", lDA) & " " & lUnitArea)
        lSW.WriteLine("Period from " & lStartYear & " to " & lEndYear & "   interval = " & String.Format("{0:0.0}", lBFInterval) & " days")
        lSW.WriteLine(Space(80).Replace(" ", "-"))

        lSW.WriteLine(vbCrLf & vbCrLf)
        lSW.WriteLine(lHeaderEngAll)
        lSW.WriteLine(lTable.ToString.Replace(",", ""))

        'Seasonal-distribution table
        lSW.WriteLine(vbCrLf & vbCrLf)
        lSW.WriteLine(Space(80).Replace(" ", "-"))
        lSW.WriteLine("                 Seasonal-distribution table " & vbCrLf)
        lSW.WriteLine("Hydrograph separation by the " & lOneLine.PadRight(23, " "))
        lSW.WriteLine("Station ID = " & lStation & " Drainage Area = " & String.Format("{0:0.00}", lDA) & " " & lUnitArea)
        lSW.WriteLine("Period from " & lStartYear & " to " & lEndYear & "   interval = " & String.Format("{0:0.0}", lBFInterval) & " days")
        lSW.WriteLine(Space(80).Replace(" ", "-"))
        lSW.WriteLine("                Year starts in " & lMonthNames(lStartMonth))
        lSW.WriteLine("                 Year ends in " & lMonthNames(lEndMonth))

        lSW.WriteLine("                        Base flow     Runoff")
        lSW.WriteLine("           Month           (in)         (in)")
        lSW.WriteLine("           ---------     ---------     ------")
        Dim lBFinch As String
        Dim lROinch As String
        For I As Integer = lStartMonth To lEndMonth
            lBFinch = String.Format("{0:0.000}", lSnCo5DepthBFPUA(I - 1).Attributes.GetValue("Mean")).PadLeft(13, " ")
            lROinch = String.Format("{0:0.000}", lSnCo6DepthROPUA(I - 1).Attributes.GetValue("Mean")).PadLeft(13, " ")
            lSW.Write(Space(11) & lMonthNames(I).Trim().PadRight(9, " ") & lBFinch & lROinch)
        Next

        lSW.Flush()
        lSW.Close()
        lSW = Nothing
        lTable.Clear()
        lTable = Nothing

        lTsFlowDepthPUA.Clear()
        lTsFlowVolumePUA.Clear()
        lTsFlowRatePUA.Clear()
        lTsBFDepthPUA.Clear()
        lTsBFVolumePUA.Clear()
        lTsBFRatePUA.Clear()
        lTsBFRateVolume.Clear()

        lMonthlyCo1RateFlow.Clear()
        lMonthlyCo2RateBF.Clear()
        lMonthlyCo3RateRO.Clear()
        lMonthlyCo4DepthFlowPUA.Clear()
        lMonthlyCo5DepthBFPUA.Clear()
        lMonthlyCo6DepthROPUA.Clear()

        lMonthlyCo8RateBFPUA.Clear()
        lMonthlyCo9RateBFVolumePUA.Clear()
        lMonthlyCo10RateBFVolume.Clear()

    End Sub

    Public Sub ASCIIHySepDelimited(ByVal aTs As atcTimeseries, ByVal aFilename As String, Optional ByVal aDelim As String = vbTab)
        Dim lSTAID As String = aTs.Attributes.GetValue("STAID", "12345678")
        Dim lColumnId1 As String = "2" & lSTAID & "   60    3"
        Dim lColumnId2 As String = ("3" & lSTAID).PadRight(16, " ")

        Dim lTsBF As atcTimeseries = Nothing
        Dim lBFName As String = ""
        If lBFName = "" Then lBFName = "HySep"
        For Each lTs As atcTimeseries In aTs.Attributes.GetDefinedValue("Baseflow").Value
            If lTs.Attributes.GetValue("Scenario").ToString.StartsWith(lBFName) Then
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
        Dim lStarting As Boolean = True
        Dim lEnded As Boolean = False
        lSW.WriteLine("Baseflow at lSTAID")
        For I As Integer = 0 To lTsBF.numValues - 1
            J2Date(lTsBF.Dates.Value(I), lDate)
            lSW.WriteLine(lDate(0) & "/" & lDate(1) & "/" & lDate(2) & aDelim & String.Format("{0:#.00}", lTsBF.Value(I + 1)))
        Next
        lSW.Flush()
        lSW.Close()
        lSW = Nothing
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

    Public Sub ASCIIPartDailyDelimited(ByVal aTS As atcTimeseries, ByVal aFilename As String, Optional ByVal aDelim As String = vbTab)
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
            Logger.Dbg("ASCIIPartDailyTabDelimited: no baseflow data found.")
            Exit Sub
        End If

        If lTsBaseflow1 Is Nothing OrElse lTsBaseflow2 Is Nothing OrElse lTsBaseflow3 Is Nothing Then
            Logger.Dbg("ASCIIPartDailyTabDelimited: no baseflow data found.")
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
        lSW.WriteLine("DAY #" & aDelim & "FLOW" & aDelim & "#1" & aDelim & "#2" & aDelim & "#3" & aDelim & "DATE")
        Dim lDayCount As String
        Dim lStreamFlow As String
        Dim lBF1 As String
        Dim lBF2 As String
        Dim lBF3 As String
        Dim lDateStr As String

        For I As Integer = 0 To lTsFlow.numValues - 1
            lDayCount = (I + 1).ToString & aDelim
            lStreamFlow = String.Format("{0:0.00}", lTsFlow.Value(I + 1)) & aDelim
            lBF1 = String.Format("{0:0.00}", lTsBaseflow1.Value(I + 1)) & aDelim
            lBF2 = String.Format("{0:0.00}", lTsBaseflow2.Value(I + 1)) & aDelim
            lBF3 = String.Format("{0:0.00}", lTsBaseflow3.Value(I + 1)) & aDelim
            J2Date(lTsFlow.Dates.Value(I), lDate)
            lDateStr = lDate(0) & "/" & lDate(1) & "/" & lDate(2)
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

    Public Sub ASCIIPartMonthlyDelimited(ByVal aTS As atcTimeseries, ByVal aFilename As String, Optional ByVal aDelim As String = vbTab)
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

        lSW.WriteLine(" ")
        lSW.WriteLine("  THIS IS FILE PARTMON.TXT FOR INPUT FILE: " & IO.Path.GetFileName(aTS.Attributes.GetValue("History 1")))
        lSW.WriteLine(" ")
        lSW.WriteLine("  PROGRAM VERSION DATE = JANUARY 2007  ")
        lSW.WriteLine(" ")
        lSW.WriteLine(" ")
        lSW.WriteLine("  MONTHLY STREAMFLOW AND BASEFLOW (INCHES):")
        lSW.WriteLine("Date" & aDelim & "Flow" & aDelim & "Baseflow")
        lSW.Flush()

        For I As Integer = 1 To lTsMonthlyFlowDepth.numValues
            J2Date(lTsMonthlyFlowDepth.Dates.Value(I - 1), lDate)
            lSW.Write(lDate(0) & "/" & lDate(1) & aDelim)
            lSW.Write(String.Format("{0:0.00}", lTsMonthlyFlowDepth.Value(I)) & aDelim)
            lSW.WriteLine(String.Format("{0:0.00}", lTsBaseflowMonthlyDepth.Value(I)))
        Next
        lSW.WriteLine(" ")
        lSW.WriteLine("     TOTAL OF MONTHLY Flow AMOUNTS = " & lTotXX)
        lSW.WriteLine("     TOTAL OF MONTHLY Baseflow AMOUNTS = " & String.Format("{0:0.0000000}", lTotalBaseflowDepth))
        lSW.WriteLine(" ")
        lSW.WriteLine(" RESULTS ON THE MONTHLY TIME SCALE SHOULD BE USED WITH CAUTION. ")
        lSW.WriteLine(" FILES PARTQRT.TXT AND PARTSUM.TXT GIVE RESULT AT THE")
        lSW.WriteLine(" CORRECT TIME SCALES (QUARTER YEAR, YEAR, OR MORE). ")
        lSW.Flush()
        lSW.Close()
        lSW = Nothing
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

    Public Sub ASCIIPartQuarterlyDelimited(ByVal aTS As atcTimeseries, ByVal aFilename As String, Optional ByVal aDelim As String = vbTab)
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

        lSW.WriteLine(" ")
        lSW.WriteLine("  THIS IS FILE PARTQRT.TXT FOR INPUT FILE: " & IO.Path.GetFileName(aTS.Attributes.GetValue("History 1")))
        lSW.WriteLine(" ")
        lSW.WriteLine("  PROGRAM VERSION DATE = JANUARY 2007  ")
        lSW.WriteLine(" ")
        lSW.WriteLine(" ")
        lSW.WriteLine("    " & aDelim & "QUARTER-YEAR STREAMFLOW IN INCHES         ")
        lSW.WriteLine("    " & aDelim & "---------------------------------          ")
        lSW.WriteLine("    " & aDelim & "JAN-" & aDelim & "APR-" & aDelim & "JUL-" & aDelim & "OCT-" & aDelim & "YEAR")
        lSW.WriteLine("Year" & aDelim & "MAR " & aDelim & "JUN " & aDelim & "SEP " & aDelim & "DEC " & aDelim & "TOTAL")
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

            Dim lStrYear As String = lCurrentYear.ToString & aDelim
            Dim lStrQ1 As String = String.Format("{0:0.00}", lQuarter1) & aDelim
            Dim lStrQ2 As String = String.Format("{0:0.00}", lQuarter2) & aDelim
            Dim lStrQ3 As String = String.Format("{0:0.00}", lQuarter3) & aDelim
            Dim lStrQ4 As String = String.Format("{0:0.00}", lQuarter4) & aDelim
            Dim lStrQYear As String = String.Format("{0:0.00}", lTsYearly.Value(lYearCount))
            lSW.WriteLine(lStrYear & lStrQ1 & lStrQ2 & lStrQ3 & lStrQ4 & lStrQYear)

            lYearCount += 1
        Next 'monthly streamflow in inches

        'print quarterly baseflow values
        lSW.WriteLine(" ")
        lSW.WriteLine(" ")
        lSW.WriteLine("    " & aDelim & "QUARTER-YEAR BASE FLOW IN INCHES          ")
        lSW.WriteLine("    " & aDelim & "--------------------------------          ")
        lSW.WriteLine("    " & aDelim & " JAN-" & aDelim & "APR-" & aDelim & "JUL-" & aDelim & "OCT-" & aDelim & "YEAR")
        lSW.WriteLine("Year" & aDelim & " MAR " & aDelim & "JUN " & aDelim & "SEP " & aDelim & "DEC " & aDelim & "TOTAL")

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

            Dim lStrYear As String = lCurrentYear.ToString & aDelim
            Dim lStrQ1 As String = String.Format("{0:0.00}", lQuarter1) & aDelim
            Dim lStrQ2 As String = String.Format("{0:0.00}", lQuarter2) & aDelim
            Dim lStrQ3 As String = String.Format("{0:0.00}", lQuarter3) & aDelim
            Dim lStrQ4 As String = String.Format("{0:0.00}", lQuarter4) & aDelim
            Dim lStrQYear As String = String.Format("{0:0.00}", lTsBFYearly.Value(lYearCount))
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

    Public Sub ASCIIPartWaterYearDelim(ByVal aTs As atcTimeseries, ByVal aFilename As String, Optional ByVal aDelim As String = vbTab)
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
        lSW.WriteLine(" ")
        lSW.WriteLine("  Year " & aDelim & " Total ")
        lSW.WriteLine(" ------" & aDelim & "----- ")

        'write results
        Dim lDate(5) As Integer
        For Each lTsWaterYear As atcTimeseries In lWaterYearCollection
            If lTsWaterYear.Attributes.GetValue("Count") = 12 Then
                'a full water year, then write out
                J2Date(lTsWaterYear.Dates.Value(0), lDate)
                lSW.Write("Oct " & lDate(0))
                J2Date(lTsWaterYear.Dates.Value(lTsWaterYear.numValues), lDate)
                lSW.Write(" to Sept " & lDate(0) & aDelim)
                lSW.WriteLine(String.Format("{0:0.00}", lTsWaterYear.Attributes.GetValue("Sum")))
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
