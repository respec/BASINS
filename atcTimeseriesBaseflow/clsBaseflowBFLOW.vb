Imports atcData
Imports atcUtility
Imports System.IO
Imports MapWinUtility

Public Class clsBaseflowBFLOW
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

    '10 !NDMIN: minimum Number of days for alpha calculation
    '10 !NDMAX: maximum Number of days for alpha calculation
    '0 !IPRINT: daily Print option (0-no; 1-yes)
    Private pNDMIN As Integer = 10
    Public Property NDMIN() As Double
        Get
            Return pNDMIN
        End Get
        Set(value As Double)
            pNDMIN = value
        End Set
    End Property

    Private pNDMAX As Integer = 10
    Public Property NDMAX() As Double
        Get
            Return pNDMAX
        End Get
        Set(value As Double)
            pNDMAX = value
        End Set
    End Property

    Public Property IPRINT As Integer = 0

    Private pNREGMX As Integer = 300
    Public Property NREGMX() As Double
        Get
            Return pNREGMX
        End Get
        Set(value As Double)
            pNREGMX = value
        End Set
    End Property

    Public Property FP1 As Double = 0.925 'original variable name: lf1

    Public Sub New()
        MyBase.New()
    End Sub

    Private pMissingDataMonth As atcCollection

    Public Overrides Function DoBaseFlowSeparation() As atcTimeseriesGroup
        If TargetTS Is Nothing Then
            Return Nothing
        End If

        If StartDate = 0 Then StartDate = TargetTS.Dates.Value(0)
        If EndDate = 0 Then EndDate = TargetTS.Dates.Value(TargetTS.Dates.numValues)
        Dim lTsDaily As atcTimeseries = SubsetByDate(TargetTS, StartDate, EndDate, Nothing)
        If Not lTsDaily.Attributes.GetValue("Tu") = atcTimeUnit.TUDay Then
            lTsDaily = Aggregate(lTsDaily, atcTimeUnit.TUDay, 1, atcTran.TranAverSame)
        End If

        Dim lTsBaseflow As atcTimeseries = Nothing
        PrintDataSummary(lTsDaily)
        Dim lNumMissing As Integer = lTsDaily.Attributes.GetValue("Count Missing")
        If lNumMissing <= 1 Then
            Logger.Dbg(
                  "NUMBER OF DAYS (WITH DATA) COUNTED =            " & lTsDaily.numValues - lNumMissing & vbCrLf &
                  "NUMBER OF DAYS THAT SHOULD BE IN THIS INTERVAL =" & lTsDaily.numValues, MsgBoxStyle.Information, "Perform BFLOW")
            BFLOW(lTsDaily)
        Else
            Logger.Dbg(
                   "***************************************" & vbCrLf &
                   "*** THERE IS A BREAK IN THE STREAM- ***" & vbCrLf &
                   "*** FLOW RECORD WITHIN THE PERIOD OF **" & vbCrLf &
                   "*** INTEREST.  PROGRAM TERMINATION. ***" & vbCrLf &
                   "***************************************", MsgBoxStyle.Critical, "BFLOW Method Stopped")
            If gBatchRun Then
                gError &= vbCrLf & "Error:BFLOW:Flow Data Has Gap."
            End If
            Return Nothing
        End If

        Dim lTsBaseflowgroup As New atcTimeseriesGroup
        With pTsBaseflow1.Attributes
            .SetValue("Scenario", "BFLOWDaily1")
            .SetValue("Drainage Area", DrainageArea)
            .SetValue("Method", BFMethods.BFLOW)
            .SetValue("FP1", FP1)
            .SetValue("Constituent", "BF_BFLOWDaily1")
            .SetValue("AnalysisStart", StartDate)
            .SetValue("AnalysisEnd", EndDate)
        End With
        With pTsBaseflow2.Attributes
            .SetValue("Scenario", "BFLOWDaily2")
            .SetValue("Drainage Area", DrainageArea)
            .SetValue("Method", BFMethods.BFLOW)
            .SetValue("FP1", FP1)
            .SetValue("Constituent", "BF_BFLOWDaily2")
            .SetValue("AnalysisStart", StartDate)
            .SetValue("AnalysisEnd", EndDate)
        End With
        With pTsBaseflow3.Attributes
            .SetValue("Scenario", "BFLOWDaily3")
            .SetValue("Method", BFMethods.BFLOW)
            .SetValue("Constituent", "BF_BFLOWDaily3")
        End With

        With pTsBaseflow1Monthly.Attributes
            .SetValue("Scenario", "BFLOWMonthly1")
            .SetValue("Method", BFMethods.BFLOW)
            .SetValue("Drainage Area", DrainageArea)
            .SetValue("Constituent", "BF_BFLOWMonthly1")
            .SetValue("AnalysisStart", StartDate)
            .SetValue("AnalysisEnd", EndDate)
        End With
        With pTsBaseflow2Monthly.Attributes
            .SetValue("Scenario", "BFLOWMonthly2")
            .SetValue("Constituent", "BF_BFLOWMonthly2")
        End With

        With pTsBaseflow3Monthly.Attributes
            .SetValue("Scenario", "BFLOWMonthly3")
            .SetValue("Constituent", "BF_BFLOWMonthly3")
        End With

        With pTsBaseflowMonthly.Attributes
            .SetValue("Scenario", "BFLOWMonthlyInterpolated")
            .SetValue("Method", BFMethods.BFLOW)
            .SetValue("Drainage Area", DrainageArea)
            'this one has a 'LinearSlope' attribute already
            .SetValue("Constituent", "BF_BFLOWMonthlyInterpolated")
            .SetValue("AnalysisStart", StartDate)
            .SetValue("AnalysisEnd", EndDate)
        End With
        With pTsBaseflowMonthlyDepth.Attributes ' a linear interpolation of the first two bf
            .SetValue("Scenario", "BFLOWMonthlyDepth")
            .SetValue("Method", BFMethods.BFLOW)
            .SetValue("Drainage Area", DrainageArea)
            .SetValue("SumDepth", pTotalBaseflowDepth)
            .SetValue("MissingMonths", pMissingDataMonth)
            .SetValue("Constituent", "BF_BFLOWMonthlyDepth")
            .SetValue("AnalysisStart", StartDate)
            .SetValue("AnalysisEnd", EndDate)
        End With

        With lTsBaseflowgroup
            .Add(pTsBaseflow1)
            .Add(pTsBaseflow2)
            .Add(pTsBaseflow3)
            .Add(pTsBaseflowMonthly)
            '.Add(pTsBaseflow1Monthly)
            '.Add(pTsBaseflow2Monthly)
            '.Add(pTsBaseflow3Monthly)
            .Add(pTsBaseflowMonthlyDepth)
        End With

        Return lTsBaseflowgroup
    End Function

    Public Function BFLOW(ByVal aTS As atcTimeseries) As atcTimeseries
        'construct baseflow timeseries
        Dim lsurfq() As Double
        ReDim lsurfq(aTS.numValues)
        pTsBaseflow1 = aTS.Clone()
        pTsBaseflow2 = aTS.Clone()
        pTsBaseflow3 = aTS.Clone()

        'So 3 pass share dates
        pTsBaseflow1.Dates.Clear()
        pTsBaseflow2.Dates.Clear()
        pTsBaseflow3.Dates.Clear()
        pTsBaseflow1.Dates = aTS.Dates
        pTsBaseflow2.Dates = aTS.Dates
        pTsBaseflow3.Dates = aTS.Dates

        'perform passes to calculate base flow
        'Dim lf1 As Double = 0.925
        Dim lf2 As Double = (1.0 + FP1) / 2.0
        lsurfq(1) = aTS.Value(1) / 2.0
        pTsBaseflow1.Value(1) = aTS.Value(1) - lsurfq(1)
        pTsBaseflow2.Value(1) = pTsBaseflow1.Value(1)
        pTsBaseflow3.Value(1) = pTsBaseflow1.Value(1)

        'first pass, forward
        For I = 2 To aTS.numValues
            lsurfq(I) = FP1 * lsurfq(I - 1) + lf2 * (aTS.Value(I) - aTS.Value(I - 1))
            If lsurfq(I) < 0 Then
                lsurfq(I) = 0.0
            End If
            pTsBaseflow1.Value(I) = aTS.Value(I) - lsurfq(I)
            If pTsBaseflow1.Value(I) < 0 Then
                pTsBaseflow1.Value(I) = 0
            End If
            If pTsBaseflow1.Value(I) > aTS.Value(I) Then
                pTsBaseflow1.Value(I) = aTS.Value(I)
            End If
        Next

        'second pass, backward
        pTsBaseflow2.Value(pTsBaseflow2.numValues) = pTsBaseflow1.Value(pTsBaseflow1.numValues)
        For I = aTS.numValues - 1 To 1 Step -1
            lsurfq(I) = FP1 * lsurfq(I + 1) + lf2 * (pTsBaseflow1.Value(I) - pTsBaseflow1.Value(I + 1))
            If lsurfq(I) < 0 Then
                lsurfq(I) = 0.0
            End If
            pTsBaseflow2.Value(I) = pTsBaseflow1.Value(I) - lsurfq(I)
            If pTsBaseflow2.Value(I) < 0 Then
                pTsBaseflow2.Value(I) = 0
            End If
            If pTsBaseflow2.Value(I) > pTsBaseflow1.Value(I) Then
                pTsBaseflow2.Value(I) = pTsBaseflow1.Value(I)
            End If
        Next

        'third pass, forward
        pTsBaseflow3.Value(pTsBaseflow3.numValues) = pTsBaseflow1.Value(pTsBaseflow1.numValues)
        For I = 2 To aTS.numValues
            lsurfq(I) = FP1 * lsurfq(I - 1) + lf2 * (pTsBaseflow2.Value(I) - pTsBaseflow2.Value(I - 1))
            If lsurfq(I) < 0 Then
                lsurfq(I) = 0.0
            End If
            pTsBaseflow3.Value(I) = pTsBaseflow2.Value(I) - lsurfq(I)
            If pTsBaseflow3.Value(I) < 0 Then
                pTsBaseflow3.Value(I) = 0
            End If
            If pTsBaseflow3.Value(I) > pTsBaseflow2.Value(I) Then
                pTsBaseflow3.Value(I) = pTsBaseflow2.Value(I)
            End If
        Next

        'perform summary calculations
        Dim lsumbf1 As Double = 0
        Dim lsumbf2 As Double = 0
        Dim lsumbf3 As Double = 0
        Dim lsumstrf As Double = 0
        Dim lVal As Double = 0
        For I As Integer = 1 To aTS.numValues
            lVal = aTS.Value(I)
            If Not Double.IsNaN(lVal) Then lsumstrf += lVal
            lVal = pTsBaseflow1.Value(I)
            If Not Double.IsNaN(lVal) Then lsumbf1 += lVal
            lVal = pTsBaseflow2.Value(I)
            If Not Double.IsNaN(lVal) Then lsumbf2 += lVal
            lVal = pTsBaseflow3.Value(I)
            If Not Double.IsNaN(lVal) Then lsumbf3 += lVal
        Next

        'calculate base flow fractions
        Dim lbflw_fr1 As Double = lsumbf1 / lsumstrf
        Dim lbflw_fr2 As Double = lsumbf2 / lsumstrf
        Dim lbflw_fr3 As Double = lsumbf3 / lsumstrf

        'compute streamflow recession constant, alpha
        Dim alpha() As Double
        Dim ndreg() As Integer
        Dim q0() As Double
        Dim q10() As Double

        'initialize variables
        ReDim alpha(aTS.numValues)
        ReDim ndreg(aTS.numValues)
        ReDim q0(aTS.numValues)
        ReDim q10(aTS.numValues)

        'Dim nregmx As Integer = 300
        'real, dimension(nregmx, 200) : florec
        'real, dimension(nregmx) : aveflo, bfdd, qaveln
        'Integer, dimension(200) : icount
        'Integer, dimension(nregmx) : npreg, idone
        Dim florec(NREGMX, 200) As Double
        Dim aveflo(NREGMX) As Double
        Dim bfdd(NREGMX) As Double
        Dim qaveln(NREGMX) As Double
        Dim npreg(NREGMX) As Integer
        Dim idone(NREGMX) As Integer
        Dim icount(200) As Integer

        Dim nd As Integer = 0
        '10 !NDMIN: minimum Number of days for alpha calculation
        '10 !NDMAX: maximum Number of days for alpha calculation
        '0 !IPRINT: daily Print option (0-no; 1-yes)
        'Dim ndmin As Integer = 10
        'Dim ndmax As Integer = 10
        'Dim iprint As Integer = 0

        For I As Integer = 1 To aTS.numValues
            If aTS.Value(I) > 0 Then
                If pTsBaseflow1.Value(I) / aTS.Value(I) < 0.99 Then
                    If nd >= NDMIN Then
                        alpha(I) = Math.Log(aTS.Value(I - nd) / aTS.Value(I - 1)) / nd
                        ndreg(I) = nd
                    End If
                    nd = 0
                Else
                    nd += 1
                    If nd >= NDMAX Then
                        alpha(I) = Math.Log(aTS.Value(I - nd + 1) / aTS.Value(I)) / nd
                        ndreg(I) = nd
                        nd = 0
                    End If
                End If
            Else
                nd = 0
            End If
        Next

        'compute x and y coords for alpha regress analysis
        Dim npr As Integer = 0
        Dim kk As Integer = 0
        Dim k As Integer = 0
        Dim x As Double = 0.0
        Dim lDates(5) As Integer
        For I As Integer = 1 To aTS.numValues
            If alpha(I) > 0 Then
                J2Date(aTS.Dates.Value(I - 1), lDates)
                If lDates(1) <= 2 OrElse lDates(1) >= 11 Then
                    npr += 1
                    q10(npr) = aTS.Value(I - 1)
                    q0(npr) = aTS.Value(I - ndreg(I))
                    If q0(npr) - q10(npr) > 0.001 Then
                        bfdd(npr) = ndreg(I) / (Math.Log(q0(npr)) - Math.Log(q10(npr)))
                        qaveln(npr) = Math.Log((q0(npr) + q10(npr)) / 2.0)
                        kk = 0
                        For k = 1 To ndreg(I)
                            x = Math.Log(aTS.Value(I - k))
                            If x > 0 Then
                                kk += 1
                                florec(kk, npr) = x
                            End If
                        Next
                        If kk = 0 Then npr -= 1
                    End If 'flow difference >0.001
                End If 'within month 2 ~ 11
            End If 'alpha_i > 0
        Next 'flow value

        'estimate master recession curve
        'real:   ssxx, ssxy, sumx, sumy, sumxy, sumx2, alf, bfd, x, slope
        'real: yint, amn
        'Integer : nd, npr, np, j, k, Now, igap

        Dim ssxx As Double
        Dim ssxy As Double
        Dim sumx As Double
        Dim sumy As Double
        Dim sumxy As Double
        Dim sumx2 As Double
        Dim alf As Double
        Dim bfd As Double
        Dim slope As Double
        Dim yint As Double
        Dim amn As Double
        Dim np As Integer
        Dim j As Integer
        Dim Now As Integer
        Dim igap As Integer

        If npr > 1 Then
            np = 0
            sumx = 0
            sumy = 0
            sumxy = 0
            sumx2 = 0
            For I As Integer = 1 To npr
                np += 1
                x = qaveln(I)
                sumx += x
                sumy += bfdd(I)
                sumxy += x * bfdd(I)
                sumx2 += x * x
            Next

            ssxx = 0
            ssxx = 0
            slope = 0
            yint = 0

            ssxy = sumxy - (sumx * sumy) / np
            ssxx = sumx2 - (sumx * sumx) / np
            slope = ssxy / ssxx
            yint = sumy / np - slope * sumx / np

            'find the recession curve with the lowest point on it
            For j = 1 To npr
                amn = 1.0E+20
                For I As Integer = 1 To npr
                    If idone(I) = 0 Then
                        If florec(1, I) < amn Then
                            amn = florec(1, I)
                            Now = I
                        End If
                    End If
                Next
                idone(Now) = 1

                'now Is the number In array florec Of the current smallest flow
                'icount keeps track Of where the now recession curve falls On the
                'x axis(Day line)
                igap = 0
                If j = 1 Then
                    icount(Now) = 1
                    igap = 1
                Else
                    For I As Integer = 1 To NREGMX
                        If florec(1, Now) <= aveflo(I) Then
                            icount(Now) = I
                            igap = 1
                            Exit For
                        End If
                    Next
                End If

                'if there is a gap, run linear regression on the average flow
                If igap = 0 Then
                    np = 0
                    sumx = 0
                    sumy = 0
                    sumxy = 0
                    sumx2 = 0
                    For I As Integer = 1 To NREGMX
                        If aveflo(I) > 0 Then
                            np += 1
                            x *= 1.0
                            sumx += x
                            sumy += aveflo(I)
                            sumxy += x * aveflo(I)
                            sumx2 += x * x
                        End If
                    Next
                    ssxy = 0
                    ssxx = 0
                    slope = 0
                    yint = 0
                    If sumx > 1 Then
                        ssxy = sumxy - (sumx * sumy) / np
                        ssxx = sumx2 - (sumx * sumx) / np
                        slope = ssxy / ssxx
                        yint = sumy / np - slope * sumx / np
                        icount(Now) = (florec(1, Now) - yint) / slope
                    Else
                        slope = 0
                        yint = 0
                        icount(Now) = 0
                    End If
                End If

                'update average flow array
                For I As Integer = 1 To NDMAX
                    If florec(1, Now) > 0.0001 Then
                        k = icount(Now) + I - 1
                        aveflo(k) = (aveflo(k) * npreg(k) + florec(I, Now)) / (npreg(k) + 1)
                        If aveflo(k) <= 0 Then aveflo(k) = slope * I + yint
                        npreg(k) += 1
                    Else
                        Exit For
                    End If
                Next
            Next

            'run alpha regression on all adjusted points
            'calcuate alpha factor for groundwater
            np = 0
            sumx = 0.0
            sumy = 0.0
            sumxy = 0.0
            sumx2 = 0.0
            For j = 1 To npr
                For I As Integer = 1 To NDMAX
                    If (florec(I, j) > 0.0) Then
                        np = np + 1
                        x = (icount(j) + I) * 1.0
                        sumx += x
                        sumy += florec(I, j)
                        sumxy += x * florec(I, j)
                        sumx2 += x * x
                    Else
                        Exit For
                    End If
                Next
            Next
            ssxy = sumxy - (sumx * sumy) / np
            ssxx = sumx2 - (sumx * sumx) / np
            alf = ssxy / ssxx
            bfd = 2.3 / alf
            With pTsBaseflow1.Attributes
                .Add("DatasetName", aTS.Attributes.GetValue("History 1").Replace("Read from ", ""))
                .Add("fr1", lbflw_fr1)
                .Add("fr2", lbflw_fr2)
                .Add("fr3", lbflw_fr3)
                .Add("npr", npr)
                .Add("alf", alf)
                .Add("bfd", bfd)
            End With
            If IPRINT = 1 Then
                'WriteOutputDat("", pTsBaseflow1.Attributes)
            End If
            'Write(3, 5002) flwfile, bflw_fr1, bflw_fr2, bflw_fr3, npr, alf, bfd
        Else
            With pTsBaseflow1.Attributes
                .Add("DatasetName", aTS.Attributes.GetValue("History 1").Replace("Read from ", ""))
                .Add("fr1", lbflw_fr1)
                .Add("fr2", lbflw_fr2)
                .Add("fr3", lbflw_fr3)
                .Add("npr", npr)
                .Add("alf", -99)
                .Add("bfd", -99)
            End With
            If IPRINT = 1 Then
                Dim lOutputDir As String = Path.GetDirectoryName(aTS.Attributes.GetValue("History 1"))
                lOutputDir = lOutputDir.ToLower.Substring("read from ".Length)
                Dim bflowDatFile As String = Path.Combine(lOutputDir, "BFLOW_" & aTS.Attributes.GetValue("Location") & ".dat")
                WriteOutputDat(bflowDatFile, pTsBaseflow1.Attributes)
                'write(3,5002) flwfile, bflw_fr1, bflw_fr2, bflw_fr3
            End If
        End If

        'if daily baseflow values are wanted
        If IPRINT = 1 Then
            Dim lOutputDir As String = Path.GetDirectoryName(aTS.Attributes.GetValue("History 1"))
            lOutputDir = lOutputDir.ToLower.Substring("read from ".Length)
            Dim flwfileo As String = Path.Combine(lOutputDir, "BFLOW_" & aTS.Attributes.GetValue("Location") & ".out")
            Dim lSW As System.IO.StreamWriter = Nothing
            Try
                lSW = New StreamWriter(flwfileo, False)
                lSW.WriteLine("Daily baseflow filters values for data from: " + aTS.Attributes.GetValue("History 1"))
                lSW.WriteLine("YEARMNDY" & " " & "  Streamflow" & " " & " Bflow Pass1" & " " & " Bflow Pass2" & " " & " Bflow Pass3")

                Dim lstrDate As String
                Dim lstrflow As String
                Dim lstrbaseq1 As String
                Dim lstrbaseq2 As String
                Dim lstrbaseq3 As String
                For I As Integer = 1 To aTS.numValues
                    '6002 format (i4,i2,i2,1x,e12.6,1x,e12.6,1x,e12.6,1x,e12.6)
                    'Write(4, 6002) iyr(i), mon(i), iday(i), strflow(i), baseq(1,i), baseq(2,i), baseq(3,i)
                    '               4       2       2      x f12.6       f12.6       f12.6       f12.6
                    J2Date(aTS.Dates.Value(I - 1), lDates)
                    lstrDate = lDates(0) & "/" & lDates(1) & "/" & lDates(2) & " "
                    lstrflow = DoubleToString(aTS.Value(I), 12, "#.000000").PadLeft(12, " ")
                    lstrbaseq1 = DoubleToString(pTsBaseflow1.Value(I), 12, "#.000000").PadLeft(12, " ")
                    lstrbaseq2 = DoubleToString(pTsBaseflow2.Value(I), 12, "#.000000").PadLeft(12, " ")
                    lstrbaseq3 = DoubleToString(pTsBaseflow3.Value(I), 12, "#.000000").PadLeft(12, " ")

                    lSW.WriteLine(lstrDate & lstrflow & lstrbaseq1 & lstrbaseq2 & lstrbaseq3)
                Next
            Catch ex As Exception
            Finally
                If lSW IsNot Nothing Then
                    lSW.Close()
                    lSW = Nothing
                End If
            End Try
        End If

        'deallocate arrays
        ReDim lsurfq(0) : lsurfq = Nothing
        ReDim alpha(0) : alpha = Nothing
        ReDim ndreg(0) : ndreg = Nothing
        ReDim q0(0) : q0 = Nothing
        ReDim q10(0) : q10 = Nothing

        ReDim aveflo(0) : aveflo = Nothing
        ReDim bfdd(0) : bfdd = Nothing
        ReDim qaveln(0) : qaveln = Nothing
        ReDim npreg(0) : npreg = Nothing
        ReDim idone(0) : idone = Nothing
        ReDim icount(0) : icount = Nothing

        For I As Integer = 0 To NREGMX
            ReDim florec(I, 0)
        Next
        ReDim florec(0, 0) : florec = Nothing


        '  NOW DIVIDE EACH BY THE NUMBER OF DAYS IN THE MONTH, TO OBTAIN MEAN
        '  FLOW IN EACH MONTH IN CFS, FOR EACH OF THE THREE VALUES OF
        '  THE REQUIREMENT OF ANTECEDENT RECESSION....

        pTsBaseflow1Monthly = Aggregate(pTsBaseflow1, atcTimeUnit.TUMonth, 1, atcTran.TranAverSame)
        pTsBaseflow2Monthly = Aggregate(pTsBaseflow2, atcTimeUnit.TUMonth, 1, atcTran.TranAverSame)
        pTsBaseflow3Monthly = Aggregate(pTsBaseflow3, atcTimeUnit.TUMonth, 1, atcTran.TranAverSame)
        'For I = 1 To pTsBaseflow1Monthly.numValues
        '    If pTsBaseflow1Monthly.Value(I) = 0 Then pTsBaseflow1Monthly.Value(I) = -99.99
        '    If pTsBaseflow2Monthly.Value(I) = 0 Then pTsBaseflow2Monthly.Value(I) = -99.99
        '    If pTsBaseflow3Monthly.Value(I) = 0 Then pTsBaseflow3Monthly.Value(I) = -99.99
        'Next

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
        pTsBaseflowMonthly.Attributes.SetValue("LinearSlope", lX)

        Return pTsBaseflow1 'TODO: make sure which one is it
    End Function

    Private Function WriteOutputDat(ByVal aFilename As String, ByVal aResults As atcDataAttributes) As Boolean
        If aResults Is Nothing Then Return False
        Dim lOutputGood As Boolean = True
        Dim lSW As System.IO.StreamWriter = Nothing
        Try
            lSW = New StreamWriter(aFilename, False)
            Dim lDatasetName As String = aResults.GetValue("DatasetName", "")
            lSW.WriteLine("Baseflow data file: this file summarizes the fraction " &
                          "of streamflow that is contributed by baseflow for each " &
                          "of the 3 passes made by the program")
            lSW.WriteLine("Gage file      " & " Baseflow Fr1" & " Baseflow Fr2" &
                          " Baseflow Fr3" & "    NPR" & " Alpha Factor" &
                          " Baseflow Days")

            '5002 format(a15,1x,f12.2,1x,f12.2,1x,f12.2,1x,i6,1x,f12.4,1x,f13.4)

            Dim lstrfwfile As String = " ".PadLeft(15, " ")
            If Not String.IsNullOrEmpty(lDatasetName) Then
                Dim lFilenameOnly As String = IO.Path.GetFileName(lDatasetName)
                If lFilenameOnly.Length >= 15 Then
                    lstrfwfile = lFilenameOnly.Substring(0, 15)
                Else
                    lstrfwfile = lFilenameOnly.PadLeft(15, " ")
                End If
            End If
            Dim lstrbflw_fr1 As String = DoubleToString(aResults.GetValue("fr1", -99), 12, "#.00").PadLeft(12, " ")
            Dim lstrbflw_fr2 As String = DoubleToString(aResults.GetValue("fr2", -99), 12, "#.00").PadLeft(12, " ")
            Dim lstrbflw_fr3 As String = DoubleToString(aResults.GetValue("fr3", -99), 12, "#.00").PadLeft(12, " ")
            Dim lstrnpr As String = ""
            Dim lstralf As String = ""
            Dim lstrbfd As String = ""
            If aResults.GetValue("npr", -1) > 1 Then
                lstrnpr = DoubleToString(aResults.GetValue("npr", -99), 0, "0").PadLeft(12, " ")
                lstralf = DoubleToString(aResults.GetValue("alf", -99), 12, "#.0000").PadLeft(12, " ")
                lstrbfd = DoubleToString(aResults.GetValue("bfd", -99), 13, "#.0000").PadLeft(13, " ")
            End If
            If String.IsNullOrEmpty(lstrnpr) Then
                lSW.WriteLine(lstrfwfile & " " & lstrbflw_fr1 & " " & lstrbflw_fr2 & " " & lstrbflw_fr3)
            Else
                lSW.WriteLine(lstrfwfile & " " & lstrbflw_fr1 & " " & lstrbflw_fr2 & " " & lstrbflw_fr3 & " " &
                              lstrnpr & " " & lstralf & " " & lstrbfd)
            End If
        Catch ex As Exception
            lOutputGood = False
        Finally
            If lSW IsNot Nothing Then
                lSW.Close()
                lSW = Nothing
            End If
        End Try
        If Not lOutputGood Then

        End If
        Return lOutputGood
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
        lStrBuilderDataSummary.AppendLine(
            "                 MONTH        " & vbCrLf &
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
                        J2Date(aTS.Dates.Value(I), lDate) 'need to re-read the year that is just being examined of its December
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

        lSW.WriteLine(" THIS IS FILE BFLOWDAY.TXT WHICH GIVES DAILY OUTPUT OF PROGRAM BFLOW. ")
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
            lDateStr = lDate(0).ToString.PadLeft(9, " ") &
                       lDate(1).ToString.PadLeft(4, " ") &
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
