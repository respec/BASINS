Imports atcData
Imports atcUtility
Imports System.IO
Imports MapWinUtility

Public Class clsBaseflowPart
    Inherits clsBaseflow

    Private pTsBaseflow1 As atcTimeseries = Nothing
    Private pTsBaseflow2 As atcTimeseries = Nothing
    Private pTsBaseflow3 As atcTimeseries = Nothing

    Private pTsBaseflow1Monthly As atcTimeseries = Nothing
    Private pTsBaseflow2Monthly As atcTimeseries = Nothing
    Private pTsBaseflow3Monthly As atcTimeseries = Nothing

    Private pTsBaseflowMonthly As atcTimeseries = Nothing
    Private pTsBaseflowMonthlyDepth As atcTimeseries = Nothing

    Private pTsMonthlyFlowDepth As atcTimeseries = Nothing

    Private pTotalBaseflowDepth As Double = 0

    Private pThresholdLowLogQ As Double = 0.1
    Public Property ThresholdLowLogQ() As Double
        Get
            Return pThresholdLowLogQ
        End Get
        Set(ByVal value As Double)
            pThresholdLowLogQ = value
        End Set
    End Property

    Public Property TBase() As Double
        Get
            Dim lDblTbase As Double = DrainageArea ^ 0.2
            Dim lIntTbase As Integer = Math.Floor(lDblTbase)
            If lIntTbase > lDblTbase Then
                lIntTbase -= 1
            End If
            If lDblTbase - lIntTbase > 1.0 Then
                Return -999.0 'need to stop PART program
            End If
            Return lIntTbase
        End Get
        Set(ByVal value As Double)
        End Set
    End Property

    Public Sub New()
        MyBase.New()
    End Sub

    Private pMissingDataMonth As atcCollection

    Public Overrides Function DoBaseFlowSeparation() As atcTimeseriesGroup
        If TargetTS Is Nothing Then
            Return Nothing
        End If

        'checking the drainage area size
        Dim lMsgDrainageArea As String = ""
        If DrainageArea < 1.0 Then
            lMsgDrainageArea = _
            "*** DRAINAGE AREA IS SMALLER THAN RECOMMENDED.***" & vbCrLf & _
            "*** THIS CAUSES THE REQUIREMENT OF ANTECEDENT ***" & vbCrLf & _
            "*** RECESSION TO BE LESS THAN TIME INCREMENT  ***" & vbCrLf & _
            "*** OF THE DATA!  RESULTS WILL BE QUESTIONABLE.**"
        ElseIf DrainageArea > 500.0 Then
            lMsgDrainageArea = _
            "*** DRAINAGE AREA IS LARGER THAN RECOMMENDED ***" & vbCrLf & _
            "*********  USE RESULTS WITH CAUTION ************"
        End If

        If lMsgDrainageArea.Length > 0 Then
            Dim lYesNo() As String = {"Continue", "Quit"}
            If Logger.MsgCustom(lMsgDrainageArea, "Problematic Drainage Area", lYesNo) = "Quit" Then
                Return Nothing
            End If
        End If

        If TBase = -999.0 Then
            Logger.Msg("Problem with calculation of required antecedent recession.", MsgBoxStyle.Critical, "PART Method Stopped")
            Return Nothing
        End If

        If StartDate = 0 Then StartDate = TargetTS.Attributes.GetValue("SJDay")
        If EndDate = 0 Then EndDate = TargetTS.Attributes.GetValue("EJDay")
        Dim lTsDaily As atcTimeseries = SubsetByDate(TargetTS, StartDate, EndDate, Nothing)
        If Not lTsDaily.Attributes.GetValue("Tu") = atcTimeUnit.TUDay Then
            lTsDaily = Aggregate(lTsDaily, atcTimeUnit.TUDay, 1, atcTran.TranAverSame)
        End If

        Dim lTsBaseflow As atcTimeseries = Nothing
        PrintDataSummary(lTsDaily)
        Dim lNumMissing As Integer = lTsDaily.Attributes.GetValue("Count Missing")
        If lNumMissing <= 1 Then
            Logger.Dbg( _
                  "NUMBER OF DAYS (WITH DATA) COUNTED =            " & lTsDaily.numValues - lNumMissing & vbCrLf & _
                  "NUMBER OF DAYS THAT SHOULD BE IN THIS INTERVAL =" & lTsDaily.numValues, MsgBoxStyle.Information, "Perform PART")
            Part(lTsDaily)
        Else
            Logger.Dbg( _
                   "***************************************" & vbCrLf & _
                   "*** THERE IS A BREAK IN THE STREAM- ***" & vbCrLf & _
                   "*** FLOW RECORD WITHIN THE PERIOD OF **" & vbCrLf & _
                   "*** INTEREST.  PROGRAM TERMINATION. ***" & vbCrLf & _
                   "***************************************", MsgBoxStyle.Critical, "PART Method Stopped")
            Return Nothing
        End If

        Dim lTsBaseflowgroup As New atcTimeseriesGroup
        pTsBaseflow1.Attributes.SetValue("Scenario", "PartDaily1")
        pTsBaseflow1.Attributes.SetValue("Drainage Area", DrainageArea)
        pTsBaseflow1.Attributes.SetValue("TBase", TBase)
        pTsBaseflow2.Attributes.SetValue("Scenario", "PartDaily2")
        pTsBaseflow3.Attributes.SetValue("Scenario", "PartDaily3")
        pTsBaseflow1Monthly.Attributes.SetValue("Scenario", "PartMonthly1")
        pTsBaseflow1Monthly.Attributes.SetValue("Drainage Area", DrainageArea)
        pTsBaseflow2Monthly.Attributes.SetValue("Scenario", "PartMonthly2")
        pTsBaseflow3Monthly.Attributes.SetValue("Scenario", "PartMonthly3")
        pTsBaseflowMonthlyDepth.Attributes.SetValue("Scenario", "PartMonthlyDepth")
        pTsBaseflowMonthlyDepth.Attributes.SetValue("Drainage Area", DrainageArea)
        pTsBaseflowMonthlyDepth.Attributes.SetValue("SumDepth", pTotalBaseflowDepth)
        pTsBaseflowMonthlyDepth.Attributes.SetValue("MissingMonths", pMissingDataMonth)
        lTsBaseflowgroup.Add(pTsBaseflow1)
        lTsBaseflowgroup.Add(pTsBaseflow2)
        lTsBaseflowgroup.Add(pTsBaseflow3)
        lTsBaseflowgroup.Add(pTsBaseflow1Monthly)
        lTsBaseflowgroup.Add(pTsBaseflow2Monthly)
        lTsBaseflowgroup.Add(pTsBaseflow3Monthly)
        lTsBaseflowgroup.Add(pTsBaseflowMonthlyDepth)
        Return lTsBaseflowgroup
    End Function

    Public Function Part(ByVal aTS As atcTimeseries) As atcTimeseries

        Dim lCountDays As Integer = aTS.numValues
        Dim lALLGW(lCountDays) As Char
        For D As Integer = 0 To lCountDays
            lALLGW(D) = " "c
        Next
        Dim lB1D(lCountDays, 3) As Double
        For D As Integer = 0 To lCountDays
            For IBASE As Integer = 0 To 3
                lB1D(D, IBASE) = -888.0
            Next
        Next

        Dim I, J As Integer
        '--------  REPEAT FROM HERE TO LINE 500 FOR EACH OF THE 3 VALUES  ----------
        '--------  FOR THE REQUIREMENT OF ANTECEDENT RECESSION        --------------

        Dim lITbase As Integer = TBase - 1
        For IBASE As Integer = 1 To 3 'original loop 500
            lITbase += 1
            'Reset lALLGW flags
            For I = 0 To lCountDays
                lALLGW(I) = " "c
            Next

            'DESIGNATE BASEFLOW=STREAMFLOW ON DATES PRECEEDED BY A RECESSION PERIOD ---
            'AND ON DATES OF ZERO STREAMFLOW. SET VARIABLE ALLGW='*' ON THESE DATES.---
            For I = lITbase + 1 To lCountDays 'original loop 200
                Dim lIndicator As Integer = 1
                Dim lIBack As Integer = 0

                Do While True 'original loop 190
                    lIBack += 1
                    If aTS.Value(I - lIBack) < aTS.Value(I - lIBack + 1) Then lIndicator = 0
                    If lIBack < lITbase Then
                        Continue Do
                    Else
                        Exit Do
                    End If
                Loop
                If lIndicator = 1 Then
                    lB1D(I, IBASE) = aTS.Value(I)
                    lALLGW(I) = "*"c
                Else
                    lB1D(I, IBASE) = -888.0
                    lALLGW(I) = " "c
                End If
            Next ' lITbase + 1 To lCountDays

            For I = 1 To lITbase ' original loop 220
                If aTS.Value(I) = 0 Then
                    lB1D(I, IBASE) = 0
                    lALLGW(I) = "*"c
                End If
            Next

            '------ DURING THESE RECESSION PERIODS, SET ALLGW = ' ' IF THE DAILY  ------
            '----------  DECLINE OF LOG Q EXCEEDS THE THRESHOLD VALUE: -----------------
            I = 1
            Do While True 'original loop 221
                I += 1
                If I = lCountDays Then Exit Do
                If lALLGW(I) <> "*"c Then Continue Do
                If lALLGW(I + 1) <> "*"c Then Continue Do
                If aTS.Value(I) = 0 Or aTS.Value(I + 1) = 0 Then Continue Do
                If lALLGW(I + 1) <> "*"c Then Continue Do
                If aTS.Value(I) < 0 Or aTS.Value(I + 1) < 0 Then Continue Do
                Dim lDiff As Double = (Math.Log(aTS.Value(I)) - Math.Log(aTS.Value(I + 1))) / 2.3025851 'ln(x) = 2.3025851 * log(x)
                If lDiff > ThresholdLowLogQ Then lALLGW(I) = " "c
                If I + 1 <= lCountDays Then
                    Continue Do
                Else
                    Exit Do
                End If
            Loop

            While True 'Start of original loop 225
                '----EXTRAPOLATE BASEFLOW IN THE FIRST FEW DAYS OF THE PERIOD OF INTEREST:----

                J = 0
                Do While True
                    J += 1
                    If lALLGW(J) = " "c Then
                        Continue Do
                    Else
                        Exit Do
                    End If
                Loop
                Dim lStartBaseFlow As Double = lB1D(J, IBASE)
                For I = 1 To J
                    lB1D(I, IBASE) = lStartBaseFlow
                Next

                '---- EXTRAPOLATE BASEFLOW IN LAST FEW DAYS OF PERIOD OF INTEREST: ----------
                J = lCountDays + 1
                Do While True 'orignial loop 250
                    J -= 1
                    If lALLGW(J) = " "c Then
                        Continue Do
                    Else
                        Exit Do
                    End If
                Loop
                Dim lEndBaseFlow As Double = lB1D(J, IBASE)
                For I = J To lCountDays 'original loop 260
                    lB1D(I, IBASE) = lEndBaseFlow
                Next

                '---------  INTERPOLATE DAILY VALUES OF BASEFLOW FOR PERIODS WHEN   ----------
                '-------------------------  BASEFLOW IS NOT KNOWN:   -------------------------
                '
                '                  FIND VERY FIRST INCIDENT OF BASEFLOW DATA

                I = 0
                Do While True 'original loop 290
                    I += 1
                    If lALLGW(I) = " "c Then
                        Continue Do
                    Else
                        Exit Do
                    End If
                Loop

                '                 NOW THAT A DAILY BASEFLOW IS FOUND, MARCH
                '                        AHEAD TO FIRST GAP IN DATA

                While True 'original loop 300
                    Dim lBASE1 As Double
                    Dim lMarchFurther1 As Boolean = True
                    While True 'original loop 300 top portion
                        If lB1D(I, IBASE) <= 0 Then
                            lBASE1 = -999.0
                        Else
                            lBASE1 = Math.Log10(lB1D(I, IBASE))
                        End If
                        I += 1
                        If I > lCountDays Then
                            lMarchFurther1 = False
                            Exit While
                        End If
                        If lALLGW(I) <> " "c Then
                            Continue While
                        Else
                            Exit While
                        End If
                    End While
                    If Not lMarchFurther1 Then Exit While 'loop 300

                    '                  MARCH THROUGH GAP IN BASEFLOW DATA:
                    Dim lNewSTART As Integer = I - 1
                    Dim lMarchFurther2 As Boolean = True
                    Dim lITime As Integer = 1
                    While True 'original loop 320
                        lITime += 1
                        If lITime + lNewSTART > lCountDays Then
                            lMarchFurther2 = False
                            Exit While
                        End If
                        If lALLGW(lITime + lNewSTART) = " "c Then
                            Continue While
                        Else
                            Exit While
                        End If
                    End While 'original loop 320
                    If Not lMarchFurther2 Then Exit While 'loop 300

                    Dim lIDays As Integer = lITime - 1
                    'Dim lT2 As Integer = lITime
                    Dim lBASE2 As Double
                    If lB1D(I + lIDays, IBASE) <= 0 Then
                        lBASE2 = -999.0
                    Else
                        lBASE2 = Math.Log10(lB1D(I + lIDays, IBASE))
                    End If

                    '    FILL IN ESTIMATED BASEFLOW DATA:
                    If lBASE1 = -999.0 Or lBASE2 = -999.0 Then
                        For J = 1 To lIDays
                            lB1D(J + lNewSTART, IBASE) = 0.0
                        Next
                    Else
                        For J = 1 To lIDays
                            lB1D(J + lNewSTART, IBASE) = 10 ^ (lBASE1 + (lBASE2 - lBASE1) * J * 1.0 / lITime)
                        Next
                    End If

                    I += lIDays
                    If I <= lCountDays Then
                        Continue While
                    Else
                        Exit While
                    End If
                End While 'original loop 300

                '------ TEST TO FIND OUT IF BASE FLOW EXCEEDS STREAMFLOW ON ANY DAY: -----

                Dim lQLow1 As Integer = 0
                For I = 1 To lCountDays
                    If lB1D(I, IBASE) > aTS.Value(I) Then
                        lQLow1 = 1
                        Exit For
                    End If
                Next
                If lQLow1 = 0 Then Exit While 'loop 225

                ' ------- IF ANY DAYS EXIST WHEN BASE FLOW > STREAMFLOW AND SF=0, ASSIGN
                ' ------- BF=SF, THEN RUN THROUGH INTERPOLATION (ABOVE):

                Dim lQLow2 As Integer = 0
                For I = 1 To lCountDays
                    If lB1D(I, IBASE) > aTS.Value(I) And aTS.Value(I) = 0 Then
                        lQLow2 = 1
                        lB1D(I, IBASE) = 0
                        lALLGW(I) = "*"c
                    End If
                Next
                If lQLow2 = 1 Then Continue While 'loop 225

                ' --------  LOCATE INTERVALS OF INTERPOLATED BASEFLOW IN WHICH AT LEAST ------
                ' --------  ONE BASEFLOW EXCEEDS STREAMFLOW ON THE CORRESPONDING DAY. --------
                ' --------  LOCATE THE DAY ON THIS INTERVAL OF THE MAXIMUM "BF MINUS SF", ----
                ' --------  AND ASSIGN BASEFLOW=STREAMFLOW ON THIS DAY. THEN RUN THROUGH -----
                ' --------  THE INTERPOLATION SCHEME (ABOVE) AGAIN.  ------------------------

                I = 0
                While True 'original loop 400
                    I += 1
                    If lB1D(I, IBASE) > aTS.Value(I) Then

                        Dim lQMININT As Double = aTS.Value(I)
                        Dim lDELMAX As Double
                        If lB1D(I, IBASE) < 0 Or aTS.Value(I) < 0 Then
                            lDELMAX = -999.0
                        Else
                            lDELMAX = Math.Log(lB1D(I, IBASE)) - Math.Log(aTS.Value(I))
                        End If

                        Dim lISearch As Integer = I
                        Dim lIMin As Integer = I

                        While True 'original loop 402
                            lISearch += 1
                            If lALLGW(lISearch) <> " "c Or _
                                lB1D(lISearch, IBASE) <= aTS.Value(lISearch) Or _
                                lISearch > lCountDays Then
                                Exit While ' loop 402
                            End If

                            Dim lDEL As Double
                            If lB1D(lISearch, IBASE) < 0 Or aTS.Value(lISearch) < 0 Then
                                lDEL = -999.0
                            Else
                                lDEL = Math.Log(lB1D(lISearch, IBASE)) - Math.Log(aTS.Value(lISearch))
                            End If
                            If lDEL > lDELMAX Then
                                lDELMAX = lDEL
                                lQMININT = aTS.Value(lISearch)
                                lIMin = lISearch
                            End If

                            If lALLGW(lISearch) = " "c Then
                                Continue While 'loop 402
                            Else
                                Exit While 'loop 402
                            End If
                        End While 'original loop 402

                        lB1D(lIMin, IBASE) = lQMININT
                        lALLGW(lIMin) = "*"c
                        I = lISearch + 1
                    End If

                    If I < lCountDays Then
                        Continue While 'loop 400
                    Else
                        Exit While 'loop 400
                    End If
                End While 'original loop 400

                If lQLow1 = 1 Then
                    Continue While 'loop 225
                Else
                    Exit While 'loop 225
                End If
            End While 'end of original loop 225

            '---------- DETERMINE RANGE OF VALUES FOR STREAMFLOW AND BASEFLOW:  ----------
            Dim lQMINPI As Double = aTS.Value(1)
            Dim lQMAXPI As Double = aTS.Value(1)
            Dim lBMINPI As Double = lB1D(1, IBASE)
            Dim lBMAXPI As Double = lB1D(1, IBASE)
            For I = 1 To lCountDays
                If aTS.Value(I) < lQMINPI Then lQMINPI = aTS.Value(I)
                If aTS.Value(I) > lQMAXPI Then lQMAXPI = aTS.Value(I)
                If lB1D(I, IBASE) < lBMINPI Then lBMINPI = lB1D(I, IBASE)
                If lB1D(I, IBASE) > lBMAXPI Then lBMAXPI = lB1D(I, IBASE)
            Next

        Next 'IBASE value (line 500)

        'TODO: construct timeseries for baseflow

        'construct baseflow timeseries
        pTsBaseflow1 = aTS.Clone()
        pTsBaseflow2 = aTS.Clone()
        pTsBaseflow3 = aTS.Clone()

        pTsBaseflow1.Dates = aTS.Dates
        pTsBaseflow2.Dates = aTS.Dates
        pTsBaseflow3.Dates = aTS.Dates

        For I = 1 To aTS.numValues
            pTsBaseflow1.Value(I) = lB1D(I, 1)
        Next

        For I = 1 To aTS.numValues
            pTsBaseflow2.Value(I) = lB1D(I, 2)
        Next

        For I = 1 To aTS.numValues
            pTsBaseflow3.Value(I) = lB1D(I, 3)
        Next

        '  NOW DIVIDE EACH BY THE NUMBER OF DAYS IN THE MONTH, TO OBTAIN MEAN
        '  FLOW IN EACH MONTH IN CFS, FOR EACH OF THE THREE VALUES OF
        '  THE REQUIREMENT OF ANTECEDENT RECESSION....

        pTsBaseflow1Monthly = Aggregate(pTsBaseflow1, atcTimeUnit.TUMonth, 1, atcTran.TranAverSame)
        pTsBaseflow2Monthly = Aggregate(pTsBaseflow2, atcTimeUnit.TUMonth, 1, atcTran.TranAverSame)
        pTsBaseflow3Monthly = Aggregate(pTsBaseflow3, atcTimeUnit.TUMonth, 1, atcTran.TranAverSame)
        For I = 1 To pTsBaseflow1Monthly.numValues
            If pTsBaseflow1Monthly.Value(I) = 0 Then pTsBaseflow1Monthly.Value(I) = -99.99
            If pTsBaseflow2Monthly.Value(I) = 0 Then pTsBaseflow2Monthly.Value(I) = -99.99
            If pTsBaseflow3Monthly.Value(I) = 0 Then pTsBaseflow3Monthly.Value(I) = -99.99
        Next

        '  DETERMINE MONTHLY BASE FLOW (IN CFS) BY INTERPOLATION BETWEEN
        '  BASE FLOW FOR TWO DIFFERENT REQUIREMENTS OF ANT. RECESSION:
        Dim lDate(5) As Integer
        Dim lX As Double = DrainageArea ^ 0.2 - TBase
        pTsBaseflowMonthly = pTsBaseflow1Monthly.Clone()
        For I = 1 To pTsBaseflow1Monthly.numValues
            J2Date(pTsBaseflow1Monthly.Dates.Value(I - 1), lDate)
            If Not pMissingDataMonth.Keys.Contains(lDate(0).ToString & "_" & lDate(1).ToString.PadLeft(2, "0")) Then
                pTsBaseflowMonthly.Value(I) = pTsBaseflow1Monthly.Value(I) + lX * (pTsBaseflow2Monthly.Value(I) - pTsBaseflow1Monthly.Value(I))
            Else
                pTsBaseflowMonthly.Value(I) = -99.99
            End If
        Next

        '   DETERMINE MONTHLY BASE FLOW IN INCHES AND FLAG AS -99.99 IF
        '   MONTH IS INCOMPLETE.  ALSO DETERMINE TOTAL OF MONTHLY AMOUNTS:

        pTotalBaseflowDepth = 0
        pTsBaseflowMonthlyDepth = pTsBaseflowMonthly.Clone
        For I = 1 To pTsBaseflowMonthlyDepth.numValues
            J2Date(pTsBaseflow1Monthly.Dates.Value(I - 1), lDate)
            If pMissingDataMonth.Keys.Contains(lDate(0).ToString & "_" & lDate(1).ToString.PadLeft(2, "0")) Then
                pTsBaseflowMonthlyDepth.Value(I) = -99.99
            Else
                pTsBaseflowMonthlyDepth.Value(I) *= DayMon(lDate(0), lDate(1)) / (26.888889 * DrainageArea)
                pTotalBaseflowDepth += pTsBaseflowMonthlyDepth.Value(I)
            End If
        Next

        ReDim lB1D(3, 0)
        ReDim lB1D(2, 0)
        ReDim lB1D(1, 0)

        '------- WRITE STREAMFLOW AND BASEFLOW FOR ONE OR MORE YEARS: ---
        Dim lOutputDir As String = Path.GetDirectoryName(aTS.Attributes.GetValue("History 1"))
        lOutputDir = lOutputDir.ToLower.Substring("read from ".Length)
        Dim lPartDayFilename As String = Path.Combine(lOutputDir, "partday_vbNet.TXT")
        'WriteBFDaily(aTS, lPartDayFilename)

        Dim lPartMonthFilename As String = Path.Combine(lOutputDir, "partmon_vbNet.txt")
        WriteBFMonthly(aTS, lPartMonthFilename)

        Dim lPartQrtFilename As String = Path.Combine(lOutputDir, "partqrt_vbNet.txt")
        WriteBFQuarterly(aTS, lPartQrtFilename)

        Dim lPartWYFilename As String = Path.Combine(lOutputDir, "partWY_vbNet.txt")
        'WriteBFWaterYear(pTsBaseflowMonthlyDepth, lPartWYFilename)

        Dim lPartSumFilename As String = Path.Combine(lOutputDir, "partsum_vbNet.txt")
        'WriteBFSum(aTS, lPartSumFilename)

        Return pTsBaseflow1 'TODO: make sure which one is it
    End Function

    Public Sub PrintDataSummary(ByVal aTS As atcTimeseries)
        'print out data summary, usgs style
        If pMissingDataMonth Is Nothing Then
            pMissingDataMonth = New atcCollection()
        End If
        Dim lNeedToRecordMissingMonth As Boolean = (pMissingDataMonth.Count = 0)
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
                If lNeedToRecordMissingMonth Then
                    Dim lKeyToBeAdded As String = lDate(0).ToString & "_" & M.ToString.PadLeft(2, "0")
                    If Not pMissingDataMonth.Keys.Contains(lKeyToBeAdded) Then
                        If lMonthFlag = "X" Then
                            pMissingDataMonth.Add(lKeyToBeAdded, lMonthFlag)
                        End If
                    End If
                End If
            Next 'month
        Next 'day
        lStrBuilderDataSummary.AppendLine("")
        lStrBuilderDataSummary.AppendLine(" COMPLETE RECORD = .      INCOMPLETE = X")

        Dim lDataSummaryFilename As String = Path.GetDirectoryName(aTS.Attributes.GetValue("History 1"))
        lDataSummaryFilename = lDataSummaryFilename.Substring("Read from ".Length)
        lDataSummaryFilename = Path.Combine(lDataSummaryFilename, "DataSummary.txt")
        Dim lSW As New StreamWriter(lDataSummaryFilename, False)
        lSW.WriteLine(lStrBuilderDataSummary.ToString)
        lSW.Flush()
        lSW.Close()
        lSW = Nothing
        lStrBuilderDataSummary.Remove(0, lStrBuilderDataSummary.Length)
    End Sub

    Private Sub WriteBFDaily(ByVal aTS As atcTimeseries, ByVal aFilename As String)
        Dim lSW As New StreamWriter(aFilename, False)

        Dim lDate(5) As Integer
        J2Date(aTS.Dates.Value(0), lDate)
        Dim lStartingYear As String = lDate(0).ToString
        J2Date(aTS.Dates.Value(aTS.numValues - 1), lDate)
        Dim lEndingYear As String = lDate(0).ToString

        lSW.WriteLine(" THIS IS FILE PARTDAY.TXT WHICH GIVES DAILY OUTPUT OF PROGRAM PART. ")
        lSW.WriteLine(" NOTE -- RESULTS AT THIS SMALL TIME SCALE ARE PROVIDED FOR ")
        lSW.WriteLine(" THE PURPOSES OF PROGRAM SCREENING AND FOR GRAPHICS, BUT ")
        lSW.WriteLine(" SHOULD NOT BE REPORTED OR USED QUANTITATIVELY ")
        lSW.WriteLine("  INPUT FILE = " & Path.GetFileName(aTS.Attributes.GetValue("History 1")))
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

        For I As Integer = 0 To aTS.numValues - 1
            lDayCount = (I + 1).ToString.PadLeft(5, " ")
            lStreamFlow = String.Format("{0:0.00}", aTS.Value(I + 1)).PadLeft(11, " ")
            lBF1 = String.Format("{0:0.00}", pTsBaseflow1.Value(I + 1)).PadLeft(11, " ")
            lBF2 = String.Format("{0:0.00}", pTsBaseflow2.Value(I + 1)).PadLeft(11, " ")
            lBF3 = String.Format("{0:0.00}", pTsBaseflow3.Value(I + 1)).PadLeft(11, " ")
            J2Date(aTS.Dates.Value(I), lDate)
            lDateStr = lDate(0).ToString.PadLeft(9, " ") & _
                       lDate(1).ToString.PadLeft(4, " ") & _
                       lDate(2).ToString.PadLeft(4, " ")
            lSW.WriteLine(lDayCount & lStreamFlow & lBF1 & lBF2 & lBF3 & lDateStr)
        Next
        lSW.Flush()
        lSW.Close()
        lSW = Nothing
    End Sub

    Private Sub WriteBFMonthly(ByVal aTS As atcTimeseries, ByVal aFilename As String)

        Dim lDate(5) As Integer
        J2Date(aTS.Dates.Value(0), lDate)
        Dim lStartingYear As String = lDate(0).ToString
        J2Date(aTS.Dates.Value(aTS.numValues - 1), lDate)
        Dim lEndingYear As String = lDate(0).ToString

        'Monthly stream flow in cfs, then, turn into inches
        Dim lTotXX As Double = 0.0 ' in inches
        If pTsMonthlyFlowDepth Is Nothing Then
            pTsMonthlyFlowDepth = Aggregate(aTS, atcTimeUnit.TUMonth, 1, atcTran.TranAverSame, Nothing)
            For M As Integer = 1 To pTsMonthlyFlowDepth.numValues
                If pTsMonthlyFlowDepth.Value(M) = 0 Then
                    pTsMonthlyFlowDepth.Value(M) = -99.99
                End If
            Next

            'Monthly stream flow in inches and flag as -99.99 if month is incomplete
            'Also determine total of monthly amounts
            For M As Integer = 1 To pTsMonthlyFlowDepth.numValues
                J2Date(pTsMonthlyFlowDepth.Dates.Value(M - 1), lDate)
                If pMissingDataMonth.Keys.Contains(lDate(0).ToString & "_" & lDate(1).ToString.PadLeft(2, "0")) Then
                    pTsMonthlyFlowDepth.Value(M) = -99.99
                Else
                    pTsMonthlyFlowDepth.Value(M) *= DayMon(lDate(0), lDate(1)) / (26.888889 * DrainageArea)
                    lTotXX += pTsMonthlyFlowDepth.Value(M)
                End If
            Next
        End If

'        Exit Sub

        'Dim lSW As New StreamWriter(aFilename, False)
        'lSW.WriteLine("  ")
        'lSW.WriteLine("  THIS IS FILE PARTMON.TXT FOR INPUT FILE: " & Path.GetFileName(aTS.Attributes.GetValue("History 1")))
        'lSW.WriteLine(" ")
        'lSW.WriteLine("  PROGRAM VERSION DATE = JANUARY 2007  ")
        'lSW.WriteLine(" ")
        'lSW.WriteLine(" ")
        'lSW.WriteLine("                        MONTHLY STREAMFLOW (INCHES):")
        'lSW.WriteLine("          J     F     M     A     M     J     J     A     S     O     N     D   YEAR")
        'lSW.Flush()
        'Dim lFieldWidth As Integer = 6
        'Dim lTsYearly As atcTimeseries = Aggregate(pTsMonthlyFlowDepth, atcTimeUnit.TUYear, 1, atcTran.TranSumDiv)
        'Dim lYearCount As Integer = 1
        'Dim lYearHasMiss As Boolean = False
        'For I As Integer = 1 To pTsMonthlyFlowDepth.numValues
        '    J2Date(pTsMonthlyFlowDepth.Dates.Value(I - 1), lDate)
        '    lSW.Write(lDate(0).ToString.PadLeft(lFieldWidth, " ")) 'begining of a year
        '    Dim lCurrentYear As Integer = lDate(0)
        '    lYearHasMiss = False
        '    For M As Integer = 1 To 12
        '        If lDate(1) = M Then
        '            If lDate(0) = lCurrentYear Then
        '                If pTsMonthlyFlowDepth.Value(I) < -99.0 Then lYearHasMiss = True
        '                lSW.Write(String.Format("{0:0.00}", pTsMonthlyFlowDepth.Value(I)).PadLeft(lFieldWidth, " "))
        '                I += 1
        '                J2Date(pTsMonthlyFlowDepth.Dates.Value(I - 1), lDate)
        '            Else
        '                Exit For
        '            End If

        '        Else
        '            lSW.Write(Space(lFieldWidth))
        '        End If
        '    Next
        '    I -= 1

        '    'print yearly sum
        '    If lYearHasMiss Then
        '        lSW.WriteLine(String.Format("{0:0.00}", -99.99).PadLeft(lFieldWidth, " "))
        '    Else
        '        lSW.WriteLine(String.Format("{0:0.00}", lTsYearly.Value(lYearCount)).PadLeft(lFieldWidth, " "))
        '    End If
        '    lYearCount += 1

        'Next
        'lSW.WriteLine(" ")
        'lSW.WriteLine("                 TOTAL OF MONTHLY AMOUNTS = " & lTotXX)
        'lSW.Flush()



        ''print baseflow monthly values
        'lSW.WriteLine(" ")
        'lSW.WriteLine(" ")
        'lSW.WriteLine("                         MONTHLY BASE FLOW (INCHES):")
        'lSW.WriteLine("          J     F     M     A     M     J     J     A     S     O     N     D   YEAR")

        'Dim lTsBFYearly As atcTimeseries = Aggregate(pTsBaseflowMonthlyDepth, atcTimeUnit.TUYear, 1, atcTran.TranSumDiv)
        'lYearCount = 1
        'For I As Integer = 1 To pTsBaseflowMonthlyDepth.numValues
        '    J2Date(pTsBaseflowMonthlyDepth.Dates.Value(I - 1), lDate)
        '    lSW.Write(lDate(0).ToString.PadLeft(lFieldWidth, " ")) 'begining of a year
        '    Dim lCurrentYear As Integer = lDate(0)
        '    lYearHasMiss = False
        '    For M As Integer = 1 To 12
        '        If lDate(1) = M Then
        '            If lDate(0) = lCurrentYear Then
        '                If pTsBaseflowMonthlyDepth.Value(I) < -99.0 Then lYearHasMiss = True
        '                lSW.Write(String.Format("{0:0.00}", pTsBaseflowMonthlyDepth.Value(I)).PadLeft(lFieldWidth, " "))
        '                I += 1
        '                J2Date(pTsBaseflowMonthlyDepth.Dates.Value(I - 1), lDate)
        '            Else
        '                Exit For
        '            End If

        '        Else
        '            lSW.Write(Space(lFieldWidth))
        '        End If

        '    Next

        '    I -= 1
        '    'print yearly sum
        '    If lYearHasMiss Then
        '        lSW.WriteLine(String.Format("{0:0.00}", -99.99).PadLeft(lFieldWidth, " "))
        '    Else
        '        lSW.WriteLine(String.Format("{0:0.00}", lTsBFYearly.Value(lYearCount)).PadLeft(lFieldWidth, " "))
        '    End If
        '    lYearCount += 1
        'Next

        'lSW.WriteLine(" ")
        'lSW.WriteLine("                  TOTAL OF MONTHLY AMOUNTS = " & String.Format("{0:0.0000000}", pTotalBaseflowDepth))
        'lSW.WriteLine(" ")
        'lSW.WriteLine(" RESULTS ON THE MONTHLY TIME SCALE SHOULD BE USED WITH CAUTION. ")
        'lSW.WriteLine(" FILES PARTQRT.TXT AND PARTSUM.TXT GIVE RESULT AT THE")
        'lSW.WriteLine(" CORRECT TIME SCALES (QUARTER YEAR, YEAR, OR MORE). ")

        'lSW.Flush()
        'lSW.Close()
        'lSW = Nothing
        'lTsYearly.Clear() : lTsYearly = Nothing
    End Sub

    Private Sub WriteBFQuarterly(ByVal aTS As atcTimeseries, ByVal aFilename As String)

        Dim lDate(5) As Integer

        'Monthly stream flow in cfs
        Dim lTotXX As Double = 0.0
        If pTsMonthlyFlowDepth Is Nothing Then
            pTsMonthlyFlowDepth = Aggregate(aTS, atcTimeUnit.TUMonth, 1, atcTran.TranAverSame, Nothing)
            For M As Integer = 1 To pTsMonthlyFlowDepth.numValues
                If pTsMonthlyFlowDepth.Value(M) = 0 Then
                    pTsMonthlyFlowDepth.Value(M) = -99.99
                End If
            Next
            'Monthly stream flow in inches and flag as -99.99 if month is incomplete
            'Also determine total of monthly amounts
            For M As Integer = 1 To pTsMonthlyFlowDepth.numValues
                J2Date(pTsMonthlyFlowDepth.Dates.Value(M - 1), lDate)
                If pMissingDataMonth.Keys.Contains(lDate(0).ToString & "_" & lDate(1).ToString.PadLeft(2, "0")) Then
                    pTsMonthlyFlowDepth.Value(M) = -99.99
                Else
                    pTsMonthlyFlowDepth.Value(M) *= DayMon(lDate(0), lDate(1)) / (26.888889 * DrainageArea)
                    lTotXX += pTsMonthlyFlowDepth.Value(M)
                End If
            Next
        End If

        'Exit Sub

        'Dim lSW As New StreamWriter(aFilename, False)
        'lSW.WriteLine("  ")
        'lSW.WriteLine("  THIS IS FILE PARTQRT.TXT FOR INPUT FILE: " & Path.GetFileName(aTS.Attributes.GetValue("History 1")))
        'lSW.WriteLine(" ")
        'lSW.WriteLine("  PROGRAM VERSION DATE = JANUARY 2007  ")
        'lSW.WriteLine(" ")
        'lSW.WriteLine("  ")
        'lSW.WriteLine("        QUARTER-YEAR STREAMFLOW IN INCHES         ")
        'lSW.WriteLine("        --------------------------------          ")
        'lSW.WriteLine("          JAN-    APR-    JULY-   OCT-    YEAR    ")
        'lSW.WriteLine("          MAR     JUNE    SEPT    DEC     TOTAL   ")
        'lSW.Flush()

        '' 1053 FORMAT (1I6, 5F8.2)
        'Dim lFieldWidth1 As Integer = 6
        'Dim lFieldWidthO As Integer = 8
        'Dim lTsYearly As atcTimeseries = Aggregate(pTsMonthlyFlowDepth, atcTimeUnit.TUYear, 1, atcTran.TranSumDiv)
        'Dim lYearCount As Integer = 1
        'Dim lQuarter1 As Double = 0
        'Dim lQuarter2 As Double = 0
        'Dim lQuarter3 As Double = 0
        'Dim lQuarter4 As Double = 0

        'Dim lQuarter1Negative As Boolean = False
        'Dim lQuarter2Negative As Boolean = False
        'Dim lQuarter3Negative As Boolean = False
        'Dim lQuarter4Negative As Boolean = False

        'For I As Integer = 1 To pTsMonthlyFlowDepth.numValues
        '    J2Date(pTsMonthlyFlowDepth.Dates.Value(I - 1), lDate)
        '    Dim lCurrentYear As Integer = lDate(0)

        '    lQuarter1 = 0
        '    lQuarter2 = 0
        '    lQuarter3 = 0
        '    lQuarter4 = 0

        '    lQuarter1Negative = False
        '    lQuarter2Negative = False
        '    lQuarter3Negative = False
        '    lQuarter4Negative = False

        '    For M As Integer = 1 To 12
        '        If lDate(1) = M And lDate(0) = lCurrentYear Then 'within a year
        '            Select Case M
        '                Case 1, 2, 3
        '                    If pTsMonthlyFlowDepth.Value(I) < -99.0 Then
        '                        lQuarter1Negative = True
        '                    Else
        '                        lQuarter1 += pTsMonthlyFlowDepth.Value(I)
        '                    End If
        '                Case 4, 5, 6
        '                    If pTsMonthlyFlowDepth.Value(I) < -99.0 Then
        '                        lQuarter2Negative = True
        '                    Else
        '                        lQuarter2 += pTsMonthlyFlowDepth.Value(I)
        '                    End If
        '                Case 7, 8, 9
        '                    If pTsMonthlyFlowDepth.Value(I) < -99.0 Then
        '                        lQuarter3Negative = True
        '                    Else
        '                        lQuarter3 += pTsMonthlyFlowDepth.Value(I)
        '                    End If
        '                Case 10, 11, 12
        '                    If pTsMonthlyFlowDepth.Value(I) < -99.0 Then
        '                        lQuarter4Negative = True
        '                    Else
        '                        lQuarter4 += pTsMonthlyFlowDepth.Value(I)
        '                    End If
        '            End Select
        '            I += 1
        '            J2Date(pTsMonthlyFlowDepth.Dates.Value(I - 1), lDate)
        '        End If
        '    Next ' month

        '    I -= 1

        '    If lQuarter1Negative Then lQuarter1 = -99.99
        '    If lQuarter2Negative Then lQuarter2 = -99.99
        '    If lQuarter3Negative Then lQuarter3 = -99.99
        '    If lQuarter4Negative Then lQuarter4 = -99.99

        '    Dim lStrYear As String = lCurrentYear.ToString.PadLeft(lFieldWidth1, " ")
        '    Dim lStrQ1 As String = String.Format("{0:0.00}", lQuarter1).PadLeft(lFieldWidthO, " ")
        '    Dim lStrQ2 As String = String.Format("{0:0.00}", lQuarter2).PadLeft(lFieldWidthO, " ")
        '    Dim lStrQ3 As String = String.Format("{0:0.00}", lQuarter3).PadLeft(lFieldWidthO, " ")
        '    Dim lStrQ4 As String = String.Format("{0:0.00}", lQuarter4).PadLeft(lFieldWidthO, " ")
        '    Dim lStrQYear As String = String.Format("{0:0.00}", lTsYearly.Value(lYearCount)).PadLeft(lFieldWidthO, " ")
        '    lSW.WriteLine(lStrYear & lStrQ1 & lStrQ2 & lStrQ3 & lStrQ4 & lStrQYear)

        '    lYearCount += 1
        'Next 'monthly streamflow in inches

        ''print quarterly baseflow values
        'lSW.WriteLine("  ")
        'lSW.WriteLine("  ")
        'lSW.WriteLine("        QUARTER-YEAR BASE FLOW IN INCHES          ")
        'lSW.WriteLine("        --------------------------------          ")
        'lSW.WriteLine("          JAN-    APR-    JULY-   OCT-    YEAR    ")
        'lSW.WriteLine("          MAR     JUNE    SEPT    DEC     TOTAL   ")

        'Dim lTsBFYearly As atcTimeseries = Aggregate(pTsBaseflowMonthlyDepth, atcTimeUnit.TUYear, 1, atcTran.TranSumDiv)
        'lYearCount = 1
        'For I As Integer = 1 To pTsBaseflowMonthlyDepth.numValues
        '    J2Date(pTsBaseflowMonthlyDepth.Dates.Value(I - 1), lDate)
        '    Dim lCurrentYear As Integer = lDate(0)

        '    lQuarter1 = 0
        '    lQuarter2 = 0
        '    lQuarter3 = 0
        '    lQuarter4 = 0

        '    lQuarter1Negative = False
        '    lQuarter2Negative = False
        '    lQuarter3Negative = False
        '    lQuarter4Negative = False

        '    For M As Integer = 1 To 12
        '        If lDate(1) = M And lDate(0) = lCurrentYear Then 'within a year
        '            Select Case M
        '                Case 1, 2, 3
        '                    If pTsBaseflowMonthlyDepth.Value(I) < -99.0 Then
        '                        lQuarter1Negative = True
        '                    Else
        '                        lQuarter1 += pTsBaseflowMonthlyDepth.Value(I)
        '                    End If
        '                Case 4, 5, 6
        '                    If pTsBaseflowMonthlyDepth.Value(I) < -99.0 Then
        '                        lQuarter2Negative = True
        '                    Else
        '                        lQuarter2 += pTsBaseflowMonthlyDepth.Value(I)
        '                    End If
        '                Case 7, 8, 9
        '                    If pTsBaseflowMonthlyDepth.Value(I) < -99.0 Then
        '                        lQuarter3Negative = True
        '                    Else
        '                        lQuarter3 += pTsBaseflowMonthlyDepth.Value(I)
        '                    End If
        '                Case 10, 11, 12
        '                    If pTsBaseflowMonthlyDepth.Value(I) < -99.0 Then
        '                        lQuarter4Negative = True
        '                    Else
        '                        lQuarter4 += pTsBaseflowMonthlyDepth.Value(I)
        '                    End If
        '            End Select
        '            I += 1
        '            J2Date(pTsBaseflowMonthlyDepth.Dates.Value(I - 1), lDate)
        '        End If
        '    Next ' month

        '    I -= 1

        '    If lQuarter1Negative Then lQuarter1 = -99.99
        '    If lQuarter2Negative Then lQuarter2 = -99.99
        '    If lQuarter3Negative Then lQuarter3 = -99.99
        '    If lQuarter4Negative Then lQuarter4 = -99.99

        '    Dim lStrYear As String = lCurrentYear.ToString.PadLeft(lFieldWidth1, " ")
        '    Dim lStrQ1 As String = String.Format("{0:0.00}", lQuarter1).PadLeft(lFieldWidthO, " ")
        '    Dim lStrQ2 As String = String.Format("{0:0.00}", lQuarter2).PadLeft(lFieldWidthO, " ")
        '    Dim lStrQ3 As String = String.Format("{0:0.00}", lQuarter3).PadLeft(lFieldWidthO, " ")
        '    Dim lStrQ4 As String = String.Format("{0:0.00}", lQuarter4).PadLeft(lFieldWidthO, " ")
        '    Dim lStrQYear As String = String.Format("{0:0.00}", lTsBFYearly.Value(lYearCount)).PadLeft(lFieldWidthO, " ")
        '    lSW.WriteLine(lStrYear & lStrQ1 & lStrQ2 & lStrQ3 & lStrQ4 & lStrQYear)

        '    lYearCount += 1
        'Next 'monthly streamflow in inches

        'lSW.Flush()
        'lSW.Close()
        'lSW = Nothing
        'lTsYearly.Clear() : lTsYearly = Nothing
    End Sub

    Private Sub WriteBFSum(ByVal aTsSF As atcTimeseries, ByVal aFilename As String)
        Dim lWriteHeader As Boolean = False
        If Not File.Exists(aFilename) Then
            lWriteHeader = True
        End If

        Dim lSW As New StreamWriter(aFilename, True)
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

        Dim lDataFilename As String = Path.GetFileName(TargetTS.Attributes.GetValue("History 1")).Substring(0, 10)
        Dim lPadWidth As Integer = 19 - lDataFilename.Trim().Length
        Dim lDrainageArea As String = String.Format("{0:0.00}", DrainageArea).PadLeft(lPadWidth, " ")
        Dim lSFMean As Double = aTsSF.Attributes.GetValue("Mean")
        Dim lBFMean1 As Double = pTsBaseflow1.Attributes.GetValue("Mean")
        Dim lBFMean2 As Double = pTsBaseflow2.Attributes.GetValue("Mean")
        Dim lBFMean3 As Double = pTsBaseflow3.Attributes.GetValue("Mean")
        Dim lMsg As String = ""
        If lBFMean1 <> lBFMean2 Then
            lMsg &= "STREAMFLOW VARIES BETWEEN DIFFERENT " & vbCrLf
            lMsg &= "VALUES OF THE REQMT ANT. RECESSION !!!"
        End If
        Dim lBFMeanArithmetic As Double = (lBFMean1 + lBFMean2 + lBFMean3) / 3.0

        Dim lA As Double = (lBFMean1 - lBFMean2 - lBFMean2 + lBFMean3) / 2.0
        Dim lB As Double = lBFMean2 - lBFMean1 - 3.0 * lA
        Dim lC As Double = lBFMean1 - lA - lB
        Dim lX As Double = DrainageArea ^ 0.2 - TBase + 1
        Dim lBFInterpolatedCFS As Double = lA * lX ^ 2.0 + lB * lX + lC 'interpolated mean base flow (cfs)
        Dim lBFInterpolatedInch As Double = lBFInterpolatedCFS * 13.5837 / DrainageArea 'interpolated mean base flow (IN/YR)

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
        Dim lSFMeanInch As String = String.Format("{0:0.00}", lSFMean * 13.5837 / DrainageArea).PadLeft(8, " ")

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

    Private Sub WriteBFWaterYear(ByVal aTsBFDepth As atcTimeseries, ByVal aFilename As String)
        Dim lSW As New StreamWriter(aFilename, False)

        Dim lWaterYear As New atcSeasonsWaterYear

        Dim lWaterYearCollection As atcTimeseriesGroup = lWaterYear.Split(aTsBFDepth, Nothing)

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

    Public Overrides Sub Clear()
        If pTsBaseflow1 IsNot Nothing Then
            pTsBaseflow1.Clear()
            pTsBaseflow1 = Nothing
        End If
        If pTsBaseflow2 IsNot Nothing Then
            pTsBaseflow2.Clear()
            pTsBaseflow2 = Nothing
        End If
        If pTsBaseflow3 IsNot Nothing Then
            pTsBaseflow3.Clear()
            pTsBaseflow3 = Nothing
        End If
        If pTsBaseflow1Monthly IsNot Nothing Then
            pTsBaseflow1Monthly.Clear()
            pTsBaseflow1Monthly = Nothing
        End If
        If pTsBaseflow2Monthly IsNot Nothing Then
            pTsBaseflow2Monthly.Clear()
            pTsBaseflow2Monthly = Nothing
        End If
        If pTsBaseflow3Monthly IsNot Nothing Then
            pTsBaseflow3Monthly.Clear()
            pTsBaseflow3Monthly = Nothing
        End If
        If pTsBaseflowMonthly IsNot Nothing Then
            pTsBaseflowMonthly.Clear()
            pTsBaseflowMonthly = Nothing
        End If
        If pTsBaseflowMonthlyDepth IsNot Nothing Then
            pTsBaseflowMonthlyDepth.Clear()
            pTsBaseflowMonthlyDepth = Nothing
        End If
        If pTsMonthlyFlowDepth IsNot Nothing Then
            pTsMonthlyFlowDepth.Clear()
            pTsMonthlyFlowDepth = Nothing
        End If
    End Sub
End Class
