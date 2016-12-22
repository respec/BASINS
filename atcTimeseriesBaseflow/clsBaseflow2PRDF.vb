Imports atcData
Imports atcUtility
Imports System.IO
Imports MapWinUtility

Public Class clsBaseflow2PRDF
    Inherits clsBaseflow

    Public Enum ETWOPARAMESTIMATION
        CUSTOM
        ECKHARDT
        CF
        NONE
    End Enum

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

    Private pParamEstimationMethod As ETWOPARAMESTIMATION = ETWOPARAMESTIMATION.ECKHARDT
    Public Property ParamEstimationMethod() As ETWOPARAMESTIMATION
        Get
            Return pParamEstimationMethod
        End Get
        Set(value As ETWOPARAMESTIMATION)
            pParamEstimationMethod = value
        End Set
    End Property

    Private pBFImax As Double = Double.NaN '0.5
    Public Property BFImax() As Double
        Get
            Return pBFImax
        End Get
        Set(ByVal value As Double)
            pBFImax = value
        End Set
    End Property

    Private pBFI As Double = Double.NaN
    Public ReadOnly Property BFI() As Double
        Get
            Return pBFI
        End Get
    End Property

    Private pRC As Double = Double.NaN
    Public Property RC() As Double
        Get
            Return pRC
        End Get
        Set(ByVal value As Double)
            pRC = value
        End Set
    End Property

    Private pSRC As Double = Double.NaN
    Public ReadOnly Property SRC() As Double
        Get
            Return pSRC
        End Get
    End Property

    Private pSBFImax As Double = Double.NaN
    Public ReadOnly Property SBFImax() As Double
        Get
            Return pSBFImax
        End Get
    End Property

    Public Property IPRINT() As Integer = 0

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
        Logger.Dbg(
                  "NUMBER OF DAYS (WITH DATA) COUNTED =            " & lTsDaily.numValues - lNumMissing & vbCrLf &
                  "NUMBER OF DAYS THAT SHOULD BE IN THIS INTERVAL =" & lTsDaily.numValues, MsgBoxStyle.Information, "Perform TwoPRDF")
        TwoPRDF(lTsDaily)

        Dim lTsBaseflowgroup As New atcTimeseriesGroup
        With pTsBaseflow1.Attributes
            .SetValue("Scenario", "TwoPRDFDaily")
            .SetValue("Drainage Area", DrainageArea)
            .SetValue("Method", BFMethods.TwoPRDF)
            .SetValue("BFImax", BFImax)
            .SetValue("SBFImax", SBFImax)
            .SetValue("RC", RC)
            .SetValue("BFI", BFI)
            .SetValue("SRC", SRC)
            .SetValue("Constituent", "BF_TwoPRDFDaily")
            .SetValue("AnalysisStart", StartDate)
            .SetValue("AnalysisEnd", EndDate)
        End With

        With pTsBaseflow1Monthly.Attributes
            .SetValue("Scenario", "TwoPRDFMonthly")
            .SetValue("Method", BFMethods.TwoPRDF)
            .SetValue("Drainage Area", DrainageArea)
            .SetValue("Constituent", "BF_TwoPRDFMonthly")
            .SetValue("AnalysisStart", StartDate)
            .SetValue("AnalysisEnd", EndDate)
        End With

        With pTsBaseflowMonthlyDepth.Attributes
            .SetValue("Scenario", "TwoPRDFMonthlyDepth")
            .SetValue("Method", BFMethods.TwoPRDF)
            .SetValue("Drainage Area", DrainageArea)
            .SetValue("SumDepth", pTotalBaseflowDepth)
            .SetValue("MissingMonths", pMissingDataMonth)
            .SetValue("Constituent", "BF_TwoPRDFMonthlyDepth")
            .SetValue("AnalysisStart", StartDate)
            .SetValue("AnalysisEnd", EndDate)
        End With

        With lTsBaseflowgroup
            .Add(pTsBaseflow1)
            '.Add(pTsBaseflow2)
            '.Add(pTsBaseflow3)
            .Add(pTsBaseflowMonthly)
            .Add(pTsBaseflowMonthlyDepth)
        End With

        Return lTsBaseflowgroup
    End Function

    ''' <summary>
    ''' This is the original BFImax and RC estimate methods from 2prdf_instructions.pdf
    ''' that comes with the Eckhardt Fortran program
    ''' </summary>
    ''' <param name="aTS"></param>
    ''' <param name="aConditions"></param>
    Public Sub CalculateBFImax_RC(ByVal aTS As atcTimeseries, Optional ByVal aConditions As atcDataAttributes = Nothing)
        '     Determine if the stream Is perennial Or ephemeral. It Is
        '     considered perennial, If it Is waterless during less than
        '     10% of all time steps with measured streamflow.
        Dim leval(aTS.numValues) As Boolean
        Dim lndry As Integer = 0
        Dim lnval As Integer = 0
        Dim lVali As Double
        Dim lVali_3 As Double
        Dim lVali_2 As Double
        Dim lVali_1 As Double
        Dim lVali1 As Double
        Dim lVali2 As Double
        For I As Integer = 1 To aTS.numValues
            lVali = aTS.Value(I)
            leval(I) = False
            If Not Double.IsNaN(lVali) Then
                lnval += 1
                If lVali < 0 Then

                Else
                    If lVali < 0.001 Then
                        lndry += 1
                    End If
                End If

                '     Only those values are evaluated, which are part of a
                '     recession period of at least five time steps. The streamflow
                '     has to recess since three time steps And to continue to recess
                '     for another two time steps 
                '     y(I - 3) > y(I - 2) > y(I - 1) > y(I) > y(I + 1) > y(I + 2).
                If I >= 4 AndAlso I <= aTS.numValues - 2 Then
                    lVali_3 = aTS.Value(I - 3)
                    lVali_2 = aTS.Value(I - 2)
                    lVali_1 = aTS.Value(I - 1)
                    lVali1 = aTS.Value(I + 1)
                    lVali2 = aTS.Value(I + 2)
                    If lVali_3 > 0 AndAlso lVali_2 > 0 AndAlso lVali_1 > 0 AndAlso lVali > 0 AndAlso lVali1 > 0 AndAlso lVali2 > 0 Then
                        If lVali_3 > lVali_2 AndAlso
                           lVali_2 > lVali_1 AndAlso
                           lVali_1 > lVali AndAlso
                           lVali > lVali1 AndAlso
                           lVali1 > lVali2 Then
                            leval(I) = True
                        End If
                    End If
                End If
            End If
        Next
        Dim ldryFraction As Double = 1.0 * lndry / (1.0 * lnval)
        If ldryFraction < 0.1 Then
            BFImax = 0.8
        Else
            BFImax = 0.5
        End If

        Dim lMonthsToSkip() As Integer = Nothing
        If aConditions IsNot Nothing Then
            lMonthsToSkip = aConditions.GetValue("MonthsToSkip", Nothing)
            If lMonthsToSkip IsNot Nothing Then
                Dim lDates(5) As Integer
                For I As Integer = 4 To aTS.numValues
                    J2Date(aTS.Dates.Value(I - 1), lDates)
                    If lMonthsToSkip.Contains(lDates(1)) Then
                        leval(I) = False
                    End If
                Next
            End If
        End If
        RC = getRecessionConstant(aTS, leval)
        ReDim leval(0) : leval = Nothing
    End Sub

    ''' <summary>
    ''' Consider only streamflow values y, which are part of a recession
    '''     period, and assume you draw a scatter plot Of y(i+1) against y(i).
    '''     If all assumptions were perfectly fullfilled, then all points in
    '''     the scatter plot would lie On a straight line With slope a. Yet, 
    '''     in reality this Is Not the case. Such a scatter plot shows that 
    '''     the recession between y(i) And y(i+1) can assume quite different 
    '''     speeds.
    '''     Baseflow, however, can be characterised As the most slowly recessing
    '''     streamflow component. This means that the baseflow recession Is 
    '''     described by those points, which form the upper bound Of the scatter
    '''     plot. Indeed, it Is found that these points lie almost On a straight 
    '''     line. Therefore, the recession constant can be found As the slope Of
    '''     a straight line, which Is fitted To the upper bound Of the scatter
    '''     plot.
    ''' </summary>
    ''' <param name="aTS"></param>
    ''' <returns>Recession Constant, a</returns>
    Public Function getRecessionConstant(ByVal aTS As atcTimeseries, ByVal aEvalFlags() As Boolean) As Double
        Dim a As Double
        '     In order to find an optimal value of the recession constant, the
        '     recession constant Is varied
        For J As Integer = 1 To 1000
            a = 1.0 - J * 0.001
            '        The recession constant represents the theoretical value of the
            '        ratio y(i+1)/y(i) during recession periods. The variable a Is
            '        considered the correct value Of the recession constant, If none 
            '        of the measured values y(i+1) Is greater than 1.02*a*y(i).
            For I As Integer = 3 To aTS.numValues - 1
                If aEvalFlags(I) AndAlso (aTS.Value(I + 1) > 1.02 * a * aTS.Value(I)) Then
                    Return a
                End If
            Next
        Next
        Return a
    End Function

    Public Function TwoPRDF(ByVal aTS As atcTimeseries) As atcTimeseries
        '     nheader  number of heading lines
        '     ndat     number of time steps
        '     nval     number of time steps with measured streamflow
        '     gageno   gage number
        '     ndry     number of time steps with streamflow = 0
        '     ysum     sum of all streamflow values
        '     a        recession constant
        '     bfimax   maximum baseflow index (filter parameter)
        '     bsum     sum of all baseflow values
        '     bfi      baseflow index
        '     sa       sensitivity index S(BFI|a)
        '     sbfimax  sensitivity index S(BFI|BFImax)
        '     y        streamflow
        '     b        baseflow
        '     name1    name of the input file with streamflow values
        '     name2    name of the output file with baseflow values
        '     eval     logical variable indicating if the data of the 
        '              respective time step Is evaluated

        'Calculate parameters, a (recession constant) and BFImax
        Select Case ParamEstimationMethod
            Case ETWOPARAMESTIMATION.CUSTOM
                If Double.IsNaN(RC) OrElse Double.IsNaN(BFImax) Then
                    Logger.Dbg("Custom RC or BFImax is invalid.")
                    Return Nothing
                End If
            Case ETWOPARAMESTIMATION.ECKHARDT, ETWOPARAMESTIMATION.NONE
                CalculateBFImax_RC(aTS)
            Case ETWOPARAMESTIMATION.CF
                'CalculateBFImaxWithUserSpecified_a(aTS)
        End Select

        'Calculate baseflow
        Dim lbsum As Double = 0
        Dim lysum As Double = aTS.Value(1)
        If Double.IsNaN(lysum) Then
            lysum = 0.0
        Else
            If lysum < 0 Then
                lysum = 0.0
            End If
        End If
        pTsBaseflow1 = aTS.Clone()
        pTsBaseflow1.Dates.Clear()
        pTsBaseflow1.Dates = aTS.Dates
        pTsBaseflow1.Value(1) = 0.9 * BFImax * aTS.Value(1)
        Dim lFlowVal As Double = 0.0
        For I As Integer = 2 To aTS.numValues
            pTsBaseflow1.Value(I) = -99.0
            lFlowVal = aTS.Value(I)
            If Not Double.IsNaN(lFlowVal) AndAlso lFlowVal > 0 Then
                lysum += lFlowVal
                If pTsBaseflow1.Value(I - 1) < 0 Then
                    pTsBaseflow1.Value(I) = 0.9 * BFImax * lFlowVal
                Else
                    pTsBaseflow1.Value(I) = ((1.0 - BFImax) * RC * pTsBaseflow1.Value(I - 1) + (1.0 - RC) * BFImax * lFlowVal) / (1.0 - RC * BFImax)
                    pTsBaseflow1.Value(I) = Math.Min(pTsBaseflow1.Value(I), lFlowVal)
                    lbsum += pTsBaseflow1.Value(I)
                End If
            End If
        Next

        pBFI = lbsum / lysum

        'Calculate the sensitivity indices
        pRC = Math.Round(RC * 1000.0, 0, MidpointRounding.AwayFromZero) / 1000.0
        Dim lBFIadj As Double = Math.Round(BFI * 100.0, 0, MidpointRounding.AwayFromZero) / 100.0

        pSRC = (1.0 - BFImax) * (lBFIadj - BFImax) / (1.0 - RC * BFImax) ^ 2 * RC / lBFIadj
        pSBFImax = (RC - 1.0) * (RC * lBFIadj - 1.0) / (1.0 - RC * BFImax) ^ 2 * BFImax / lBFIadj

        If IPRINT = 1 Then
            Dim lOutputDir As String = Path.GetDirectoryName(aTS.Attributes.GetValue("History 1"))
            lOutputDir = lOutputDir.ToLower.Substring("read from ".Length)
            Dim lBFITXTFile As String = Path.Combine(lOutputDir, "bfi_TwoPRDF.txt")
            WriteOutputDat(lBFITXTFile)
            Dim loc As String = aTS.Attributes.GetValue("Location")
            Dim lbaseflowTXTFile As String = Path.Combine(lOutputDir, "baseflow_TwoPRDF_" & loc & ".txt")
            WriteOutputBaseflow(lbaseflowTXTFile, aTS, pTsBaseflow1)
        End If

        pTsBaseflow1Monthly = Aggregate(pTsBaseflow1, atcTimeUnit.TUMonth, 1, atcTran.TranAverSame)

        '   DETERMINE MONTHLY BASE FLOW IN INCHES AND FLAG AS -99.99 IF
        '   MONTH IS INCOMPLETE.  ALSO DETERMINE TOTAL OF MONTHLY AMOUNTS:
        pTotalBaseflowDepth = 0
        pTsBaseflowMonthlyDepth = pTsBaseflow1Monthly.Clone()
        Dim lDates(5) As Integer
        For I = 1 To pTsBaseflowMonthlyDepth.numValues
            J2Date(pTsBaseflow1Monthly.Dates.Value(I - 1), lDates)
            If pMissingDataMonth.Keys.Contains(lDates(0).ToString & "_" & lDates(1).ToString.PadLeft(2, "0")) Then
                pTsBaseflowMonthlyDepth.Value(I) = -99.99
            Else
                pTsBaseflowMonthlyDepth.Value(I) *= DayMon(lDates(0), lDates(1)) / (26.888889 * DrainageArea)
                pTotalBaseflowDepth += pTsBaseflowMonthlyDepth.Value(I)
            End If
        Next

        Return pTsBaseflow1 'TODO: make sure which one is it
    End Function

    Private Function WriteOutputDat(ByVal aFilename As String) As Boolean
        'aFilename = "bfi.txt" -- unit 12
        'write(12,'(I11,2X,F5.3,3X,F4.2,5X,F5.2,10X,F5.2)') gageno, a, BFI, sa, SBFImax
        Dim lOutputGood As Boolean = True
        Dim lSW As System.IO.StreamWriter = Nothing
        Try
            Dim lWriteHeader As Boolean = True
            If IO.File.Exists(aFilename) Then
                lWriteHeader = False
            End If
            lSW = New StreamWriter(aFilename, True)
            If lWriteHeader Then
                lSW.WriteLine("gage number      a    BFI  S(BFI|a)  S(BFI|BFImax)")
                lSW.WriteLine("--------------------------------------------------")
            End If
            Dim lstrgageno As String = TargetTS.Attributes.GetValue("Location", "").ToString().PadLeft(11, " ")
            'Dim lstrRC As String = DoubleToString(RC, 5, "0.000").PadLeft(5, " ")
            'Dim lstrBFI As String = DoubleToString(BFI, 4, "0.00").PadLeft(4, " ")
            'Dim lstrSRC As String = DoubleToString(SRC, 5, "0.00").PadLeft(5, " ")
            'Dim lstrSBFI As String = DoubleToString(SBFImax, 5, "0.00").PadLeft(5, " ")
            Dim lstrRC As String = String.Format("{0:0.000}", RC).PadLeft(5, " ")
            Dim lstrBFI As String = String.Format("{0:0.00}", BFI).PadLeft(4, " ")
            Dim lstrSRC As String = String.Format("{0:0.00}", SRC).PadLeft(5, " ")
            Dim lstrSBFI As String = String.Format("{0:0.00}", SBFImax).PadLeft(5, " ")
            lSW.WriteLine(lstrgageno & "  " & lstrRC & "   " & lstrBFI & "     " & lstrSRC & "          " & lstrSBFI)
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

    Private Function WriteOutputBaseflow(ByVal aFilename As String, ByVal aTS As atcTimeseries, ByVal aBFTS As atcTimeseries) As Boolean
        'aFilename = "baseflow_" & TargetTS.Attribute.GetValue("Location") & ".txt"
        Dim lOutputGood As Boolean = True
        Dim lSW As System.IO.StreamWriter = Nothing
        Try
            lSW = New StreamWriter(aFilename, False)
            Dim lstrgageno As String = TargetTS.Attributes.GetValue("Location", "")
            lSW.WriteLine("Stream- and base-flow for gage number " & lstrgageno)
            lSW.WriteLine("recession constant= " & DoubleToString(RC, 5, "0.000") & ", BFI= " & DoubleToString(BFI, 4, "0.00"))
            Dim lDateFormat As New atcDateFormat()
            With lDateFormat
                .IncludeHours = False
                .IncludeMinutes = False
                .IncludeSeconds = False
            End With
            Dim lstrDate As String = ""
            Dim lstrFlow As String = ""
            Dim lstrBaseflow As String = ""
            For I As Integer = 1 To aTS.numValues
                lstrDate = lDateFormat.JDateToString(aTS.Dates.Value(I - 1)).PadLeft(10, " ")
                lstrFlow = DoubleToString(aTS.Value(I), 8, "0.0").PadLeft(8, " ")
                lstrBaseflow = DoubleToString(aBFTS.Value(I), 8, "0.0").PadLeft(8, " ")
                lSW.WriteLine(lstrDate & lstrFlow & lstrBaseflow)
            Next
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
        Dim lX As Double = 0.0 'DrainageArea ^ 0.2 - TBase + 1
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
