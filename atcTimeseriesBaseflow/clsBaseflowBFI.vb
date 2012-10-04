Imports atcData
Imports atcUtility

Public Class clsBaseflowBFI
    Inherits clsBaseflow
    Private pPartitionLengthInDays As Integer = 5
    Public Property PartitionLengthInDays() As Integer
        Get
            Return pPartitionLengthInDays
        End Get
        Set(ByVal value As Integer)
            pPartitionLengthInDays = value
        End Set
    End Property

    'The method only effect the approach for finding turning point
    'rest of the work flow between the two methods are the same
    Private pMethod As BFMethods = BFMethods.BFIStandard
    Public Property Method() As BFMethods
        Get
            Return pMethod
        End Get
        Set(ByVal value As BFMethods)
            pMethod = value
        End Set
    End Property

    Private pUseSymbols As Boolean
    Public Property UseSymbols() As Boolean
        Get
            Return pUseSymbols
        End Get
        Set(ByVal value As Boolean)
            pUseSymbols = value
        End Set
    End Property

    'F,  ratio used in base-flow separation (default = 0.9)
    Private pTPTestFraction As Double = 0.9
    Public Property TPTestFraction() As Double
        Get
            Return pTPTestFraction
        End Get
        Set(ByVal value As Double)
            pTPTestFraction = value
        End Set
    End Property

    'K
    Private pOneDayRecessConstant As Double = 0.97915
    Public Property OneDayRecessConstant() As Double
        Get
            Return pOneDayRecessConstant
        End Get
        Set(ByVal value As Double)
            pOneDayRecessConstant = value
        End Set
    End Property

    Public gZSYM As String = "*" 'symbol for years with no-flow days (empty if symbols aren't used)
    Public gISYM As String = "^" 'symbol for interpolated turning points (empty if symbols aren't used)
    Public gXSYM As String = "!" 'the extrapolation symbol (empty if symbols aren't used)

    Private pYearBasis As String
    Public Property YearBasis() As String
        Get
            Return pYearBasis
        End Get
        Set(ByVal value As String)
            pYearBasis = value
        End Set
    End Property

    Public Overrides Function DoBaseFlowSeparation() As atcTimeseriesGroup
        If TargetTS Is Nothing Then
            Return Nothing
        End If

        'Make sure the ts is in daily timestep and adjust start and end date

        'Dim lTsDaily As atcTimeseries = SubsetByDate(TargetTS, lTsStart, lTsEnd, Nothing)
        'Dim lTsDaily As atcTimeseries = TargetTS.Clone()

        If StartDate = 0 Then StartDate = TargetTS.Attributes.GetValue("SJDay")
        If EndDate = 0 Then EndDate = TargetTS.Attributes.GetValue("EJDay")
        Dim lTsDaily As atcTimeseries = SubsetByDate(TargetTS, StartDate, EndDate, Nothing)
        If Not lTsDaily.Attributes.GetValue("Tu") = atcTimeUnit.TUDay Then
            lTsDaily = Aggregate(lTsDaily, atcTimeUnit.TUDay, 1, atcTran.TranAverSame)
        End If

        'set Year basis, water year vs calendar year
        Dim lDate(5) As Integer
        J2Date(lTsDaily.Dates.Value(0), lDate)
        If lDate(1) = 1 Then
            YearBasis = "Calendar"
        ElseIf lDate(1) = 10 Then
            YearBasis = "Water   "
        Else
            YearBasis = "Arbitrary"
        End If
        If YearBasis.StartsWith("Arbitrary") Then
            Return Nothing 'enforce either water year data or calendar year data
        End If

        'Find the mins
        Dim lTsQMINS As atcTimeseries = lTsDaily.Clone()
        Dim lTsDAYMIN As atcTimeseries = lTsDaily.Clone()
        For I As Integer = 0 To lTsDAYMIN.numValues
            lTsDAYMIN.Value(I) = -999 'important as later on will use this -999 as guide for detecting valud TP day
        Next
        GetMins0(lTsDaily, lTsQMINS, lTsDAYMIN)
        Dim lTsBF As atcTimeseries = DoBFI(lTsDaily, lTsQMINS, lTsDAYMIN)

        'Adjust for pre- and post- duration dates
        lTsBF = SubsetByDate(lTsBF, StartDate, EndDate, Nothing)
        lTsBF.Attributes.SetValue("Drainage Area", DrainageArea)
        Dim lTsBFgroup As New atcTimeseriesGroup
        lTsBFgroup.Add(lTsBF)

        Return lTsBFgroup
    End Function

    Public Sub GetMins0(ByVal aTsFlow As atcTimeseries, ByRef lTsQMINS As atcTimeseries, ByRef lTsDAYMIN As atcTimeseries)
        'Dim lTsQMINS As atcTimeseries = aTsFlow.Clone()
        'Dim lTsDAYMIN As atcTimeseries = aTsFlow.Clone()
        Dim lblZEROR As String = " "
        Dim lblZSYM As String = lblZEROR

        For I As Integer = 1 To aTsFlow.numValues
            lTsQMINS.Value(I) = -1
            lTsDAYMIN.Value(I) = -1
        Next

        Dim lNumIntervals As Integer = aTsFlow.numValues / PartitionLengthInDays
        Dim lQMIN As Double
        Dim lJJ As Integer
        Dim lDAYN As Integer

        For J As Integer = 1 To lNumIntervals 'loop 590
            lQMIN = 999999
            If J = lNumIntervals Then
                lJJ = aTsFlow.numValues - (J - 1) * PartitionLengthInDays
            Else
                lJJ = PartitionLengthInDays
            End If
            If J < lNumIntervals Then
                For I As Integer = 1 To lJJ 'loop 585
                    Dim lDayIndex As Integer = PartitionLengthInDays * (J - 1) + I
                    If aTsFlow.Value(lDayIndex) < lQMIN Then
                        lQMIN = aTsFlow.Value(lDayIndex)
                        lDAYN = lDayIndex
                    End If
                Next 'loop 585
            Else
                For I As Integer = 1 To lJJ 'loop 588
                    Dim lDayIndex As Integer = PartitionLengthInDays * (J - 1) + I
                    If aTsFlow.Value(lDayIndex) <= lQMIN Then
                        lQMIN = aTsFlow.Value(lDayIndex)
                        lDAYN = lDayIndex
                    End If
                Next 'loop 588
            End If

            If lQMIN = 0 Then lblZEROR = lblZSYM
            lTsQMINS.Value(J) = lQMIN
            lTsDAYMIN.Value(J) = lDAYN

        Next 'loop 590
    End Sub

    Public Function DoBFI(ByVal aTsFlow As atcTimeseries, ByVal aTsQMINS As atcTimeseries, ByVal aTsDAYMIN As atcTimeseries) As atcTimeseries
        '*---- DETERMINE TURNING POINTS AND COMPUTE VOLUME OF BASE FLOW BETWEEN
        '*---- SUCCESSIVE TURNING POINTS FOR DATA IN ARRAYS FOR YEAR 1

        Dim lVOLBF As Double = 0.0 'volume of base flow
        Dim lFIRSTTP As Double = aTsFlow.numValues '365 'day of first base-flow turning point of the year
        Dim lLASTTP As Double = 0 'day of the last base-flow turning point for the year

        Dim lComplete As Boolean = (aTsFlow.Attributes.GetValue("Count Missing") = 0)

        Dim lTsBaseflow As atcTimeseries = aTsFlow.Clone()
        With lTsBaseflow.Attributes
            Select Case Method
                Case BFMethods.BFIStandard
                    .SetValue("Scenario", "BFIStandard")
                    .SetValue("Method", BFMethods.BFIStandard)
                    .SetValue("BFIFrac", TPTestFraction)
                Case BFMethods.BFIModified
                    .SetValue("Scenario", "BFIModified")
                    .SetValue("Method", BFMethods.BFIModified)
                    .SetValue("BFIK1Day", OneDayRecessConstant)
            End Select
            .SetValue("BFINDay", PartitionLengthInDays)
        End With

        '*---- TACK YEAR 2's FIRST QMIN ONTO END OF YEAR 1
        'IF ((WATYR2.EQ.WATYR1+1).AND.(GAGE2.EQ.GAGE1)) THEN
        '      QMINS1(NDAY1YR + 1) = QMINS2(1)
        '      DAY1MIN(NDAY1YR + 1) = DAY2MIN(1)
        '  Else
        '      QMINS1(NDAY1YR + 1) = -99
        '  End If

        '*---- IF AN INTERPOLATION WAS PERFORMED THE LAST TIME THROUGH, RECALL THE
        '*---- RESULT AS THE FIRST TURNING POINT, AND RETRIEVE THE END-OF-YEAR Q
        '*---- AS THIS YEAR'S START-OF-YEAR Q
        Dim lTPQ1 As Double = 0 ' interpolated base flow on day 1 of water year (October 1); start out as 0
        Dim lTYPEF As String = " " ' holds interpolation marker for year's first turning point
        Dim lSOYQ As Double = -99 'start-of-year Q (instantaneous Q at t=0.5 days)
        Dim lSOYDAY As Double = -99 'start-of-year day (0.5 or -99)
        Dim lEOYQ As Double 'end-of-year base-flow (on day 365.5/366.5); starts out as 0
        Dim lSEFLAG As Integer = 0 'integer flag to tell whether the complete year of data
        'was used to calculate base flow, including SOY and EOY points

        Dim lPREVDAY As Integer 'day of year on which previous turning point occurred
        Dim lPREVQ As Double 'discharge at previous turning point
        Dim lFLAG As String = " " 'character variable to hold interpolation marker

        If (lTPQ1 >= 0) Then
            lFIRSTTP = 1
            lPREVDAY = 1
            lPREVQ = lTPQ1
            If UseSymbols Then
                lFLAG = gISYM
            Else
                lFLAG = " "
            End If

            lTYPEF = gISYM
            lSOYQ = lEOYQ
            lSOYDAY = 0.5
        End If

        Dim lDate(5) As Integer
        Dim lStrYear As String
        Dim lStrMonth As String
        Dim lStrDay As String
        Dim lStrFlow As String
        Dim lStrFlag As String
        Dim lBF2Print As Double

        '*---- LOCATE SUCCESSIVE TURNING POINTS THROUGHOUT THE YEAR
        Dim lTurnPt As Boolean
        Dim lADDVol As Double 'additional volume accumulated between successive turning points

        aTsQMINS.Value(0) = 0
        aTsDAYMIN.Value(0) = 0
        Dim lTsDayTp As atcTimeseries = aTsDAYMIN.Clone()
        For I As Integer = 1 To aTsFlow.numValues / PartitionLengthInDays 'loop 80
            lTurnPt = False
            lTurnPt = TestForTurningPoint(aTsQMINS.Value(I - 1), aTsQMINS.Value(I), aTsQMINS.Value(I + 1), _
                                aTsDAYMIN.Value(I - 1), aTsDAYMIN.Value(I), aTsDAYMIN.Value(I + 1), Method)

            If lTurnPt Then
                lFIRSTTP = Math.Min(lFIRSTTP, aTsDAYMIN.Value(I))
                lLASTTP = Math.Max(lLASTTP, aTsDAYMIN.Value(I))
                Dim lDAYOFTP As Integer = aTsDAYMIN.Value(I)
                Dim lQTP As Double = aTsQMINS.Value(I)
                '*-------- IF THE FIRST TURNING POINT WAS INTERPOLATED, AND A TURNING POINT
                '*-------- WAS NOT SUBSEQUENTLY FOUND ON DAY 1 OF THE YEAR, THEN OUTPUT THE
                '*-------- INTERPOLATED TURNING POINT WITH THE INTERPOLATION FLAG.  OTHERWISE
                '*-------- RESET TYPEF TO ' ' AND DON'T PRINT THE TURNING POINT.  IT WILL GET
                '*-------- PRINTED LATER.
                If lTPQ1 >= 0 Then
                    If aTsDAYMIN.Value(I) = 1 Then
                        lTYPEF = " "
                    Else
                        'IF (TPFILE.NE.0) WRITE (TPFILE,70)
                        '+            DATE1YR(PREVDAY),DATE1MTH(PREVDAY),
                        '+            DATE1DAY(PREVDAY),PREVQ,FLA
                        '70     FORMAT (I4,I6,I6,F10.2,A2)
                        J2Date(aTsFlow.Dates.Value(lPREVDAY - 1), lDate)
                        lStrYear = lDate(0).ToString
                        lStrMonth = lDate(1).ToString.PadLeft(6, " ")
                        lStrDay = lDate(2).ToString.PadLeft(6, " ")
                        lStrFlow = DoubleToString(aTsFlow.Value(lPREVDAY), 10, , , , ).PadLeft(10, " ")
                        lStrFlag = lFLAG.PadLeft(2, " ")
                        'lFHTP.WriteLine(lStrYear & lStrMonth & lStrDay & lStrFlow & lStrFlag)
                    End If
                    lTPQ1 = -99
                    lFLAG = " "
                End If 'lTPQ1 >= 0

                '*-------- WRITE THE TURNING POINT TO THE TURNING POINT OUTPUT FILE
                J2Date(aTsFlow.Dates.Value(lDAYOFTP - 1), lDate)
                lStrYear = lDate(0).ToString
                lStrMonth = lDate(1).ToString.PadLeft(6, " ")
                lStrDay = lDate(2).ToString.PadLeft(6, " ")
                lStrFlow = DoubleToString(aTsFlow.Value(lDAYOFTP), 10, , , , ).PadLeft(10, " ")
                lStrFlag = lFLAG.PadLeft(2, " ")
                'lFHTP.WriteLine(lStrYear & lStrMonth & lStrDay & lStrFlow & lStrFlag)

                '*-------- WRITE THE INDIVIDUAL DAILY BASE FLOWS AND TOTAL FLOWS
                '*-------- TO THE DAILY FLOW OUTPUT FILE.
                If lDAYOFTP <> lFIRSTTP Then
                    For II As Integer = lPREVDAY To lDAYOFTP - 1 'loop 75
                        lBF2Print = QINTERP(lPREVDAY, lPREVQ, lDAYOFTP, lQTP, II)
                        If lBF2Print > aTsFlow.Value(II) Then lBF2Print = aTsFlow.Value(II)

                        'original program writes out daily baseflow
                        'we are just saving into a timeseries
                        'WRITE (DAYFILE,77)DATE1YR(II),DATE1MTH(II),
                        '+          DATE1DAY(II),BF2PRINT,Q1(II)
                        '77 FORMAT (I4,I6,I6,F10.2,F12.2)
                        lTsBaseflow.Value(II) = lBF2Print
                    Next 'loop 75
                End If

                lPREVQ = lQTP
                lPREVDAY = lDAYOFTP
            Else
                lTsDayTp.Value(I) = -1
            End If 'lTurnPt
        Next 'loop 80

        'Reconstruct a QMINS timeseries with correct date-flowvalue alignment
        Dim lTsQMINSTp As atcTimeseries = aTsFlow.Clone()
        For I As Integer = 0 To lTsQMINSTp.numValues
            lTsQMINSTp.Value(I) = GetNaN()
        Next
        Dim lDayIndex As Integer
        For I As Integer = 0 To lTsDayTp.numValues
            lDayIndex = lTsDayTp.Value(I)
            If lDayIndex >= 0 Then
                lTsQMINSTp.Value(lDayIndex) = aTsQMINS.Value(I)
            End If
        Next

        'calculate annual stats
        Dim lAnnualSummary As New Text.StringBuilder()
        Dim lGroupTsQMin As atcTimeseriesGroup = Nothing
        'Dim lGroupTsDayMin As atcTimeseriesGroup = lSeasonWaterYear.Split(aTsDAYMIN, Nothing)
        Dim lGroupTsFlow As atcTimeseriesGroup = Nothing

        Dim lSeasonYear As Object = Nothing
        If YearBasis.StartsWith("Calendar") Then
            lSeasonYear = New atcSeasonsCalendarYear()
        ElseIf YearBasis.StartsWith("Water") Then
            lSeasonYear = New atcSeasonsWaterYear()
        End If
        lGroupTsQMin = lSeasonYear.Split(lTsQMINSTp, Nothing)
        lGroupTsFlow = lSeasonYear.Split(aTsFlow, Nothing)

        With lTsBaseflow.Attributes
            .SetValue("YearBasis", YearBasis)
            .SetValue("TsQMINSTp", lTsQMINSTp)
        End With

        Dim lBFISum As Double
        Dim lBFISqr As Double
        Dim lNYears As Integer
        Dim lVolQ As Double = 0

        Dim lQSum As Double
        Dim lQSqr As Double
        Dim lBFSum As Double
        Dim lBFSqr As Double

        'Writing annual results
        For Yc As Integer = 0 To lGroupTsQMin.Count - 1
            lPREVDAY = -99
            lPREVQ = 0
            lVOLBF = 0
            lFIRSTTP = aTsFlow.numValues
            lLASTTP = 0
            '*---- Determine annual baseflow sum in CFS
            Dim lTsQMin As atcTimeseries = lGroupTsQMin(Yc)
            Dim lTsFlow As atcTimeseries = lGroupTsFlow(Yc)

            For TPc As Integer = 1 To lTsQMin.numValues
                Dim lDAYOFTP As Integer = TPc
                Dim lQTP As Double = lTsQMin.Value(TPc)
                lFIRSTTP = Math.Min(lFIRSTTP, lDAYOFTP)
                lLASTTP = Math.Max(lLASTTP, lDAYOFTP)
                'If lDAYOFTP > 0 Then
                '    If lDAYOFTP <> lFIRSTTP And lDAYOFTP <> lPREVDAY Then '???Not sure this is needed???
                '        lADDVol = BASEFLOW(lPREVDAY, lDAYOFTP, lPREVQ, lQTP)
                '        lVOLBF += lADDVol
                '    End If
                'End If
                If lPREVDAY >= 0 AndAlso lDAYOFTP <> lPREVDAY AndAlso lPREVQ > 0 AndAlso Not Double.IsNaN(lQTP) Then
                    lADDVol = BASEFLOW(lPREVDAY, lDAYOFTP, lPREVQ, lQTP)
                    lVOLBF += lADDVol
                End If
                If Not Double.IsNaN(lQTP) Then
                    lPREVDAY = lDAYOFTP
                    lPREVQ = lQTP
                End If
            Next
            '*---- DETERMINE ANNUAL VOLUME OF TOTAL FLOW BETWEEN FIRST TURNING POINT AND
            '*---- LAST TURNING POINT (FIRSTTP,LASTTP) THIS VOLUME IS IN CFS-DAYS
            lVolQ = 0
            Dim lValue As Double
            For lDay As Integer = lFIRSTTP To lLASTTP 'loop 110
                lValue = lTsFlow.Value(lDay)
                If Not Double.IsNaN(lValue) AndAlso lValue >= 0 Then
                    If lDay = lFIRSTTP Or lDay = lLASTTP Then
                        lVolQ += lTsFlow.Value(lDay) / 2
                    Else
                        lVolQ += lTsFlow.Value(lDay)
                    End If
                End If
            Next 'loop 110

            '*---- CALCULATE Annual BASE-FLOW INDEX
            Dim lBFIndex As Double
            If lVolQ = 0 Then
                lBFIndex = 0
            Else
                lBFIndex = lVOLBF / lVolQ
            End If
            lBFISum += lBFIndex
            lBFISqr += lBFIndex * lBFIndex
            lNYears += 1

            '*---- COMPUTE VOLUME OF RUNOFF AND BASE FLOW FOR THE FULL YEAR
            Dim lblBFExtrap As String = " "
            If lTsFlow.Attributes.GetValue("Count Missing") > 0 Then
                lblBFExtrap = gXSYM
            End If
            lVolQ = 0
            For D As Integer = 1 To lTsFlow.numValues
                lValue = lTsFlow.Value(D)
                If Not Double.IsNaN(lValue) AndAlso lValue >= 0 Then 'by pass missing data and bad values
                    lVolQ += lTsFlow.Value(D)
                End If
            Next

            lVolQ *= 86400 / 43560 'acre-day 86400 seconds/day, 1 acre = 43560 square feet
            lVOLBF = lVolQ * lBFIndex 'baseflow in acre-day

            lQSum += lVolQ
            lQSqr += lVolQ * lVolQ
            lBFSum += lVOLBF
            lBFSqr += lVOLBF * lVOLBF

            J2Date(lTsFlow.Dates.Value(1), lDate)
            lStrYear = lDate(0).ToString.PadLeft(5, " ")
            If lblBFExtrap = " " Then 'IF (COMP1LETE) THEN
                Dim lStrBFIndex As String = String.Format("{0:0.000}", lBFIndex).PadLeft(14, " ") & " " & lblBFExtrap
                Dim lStrVolBF As String = String.Format("{0:0}.", lVOLBF).PadLeft(14, " ") & " " & lblBFExtrap
                lStrFlow = String.Format("{0:0}.", lVolQ).PadLeft(14, " ")
                Dim lStrFirstTP As String = lFIRSTTP.ToString.PadLeft(10, " ") & " " & " "
                Dim lStrLastTP As String = lLASTTP.ToString.PadLeft(10, " ") & " " & " "
                lAnnualSummary.AppendLine(lStrYear & lStrBFIndex & lStrVolBF & lStrFlow & lStrFirstTP & lStrLastTP)
                'lFHOut.WriteLine(lStrYear & lStrBFIndex & lStrVolBF & lStrFlow & lStrFirstTP & lStrLastTP)
            Else 'incomplete year
                lAnnualSummary.AppendLine(lStrYear & "     Incomplete year. Base flow cannot be determined.")
                'lFHOut.WriteLine(lStrYear & "     Incomplete year. Base flow cannot be determined.")
            End If
            'Write annual values

        Next 'Yc ((water) year count)

        If lNYears >= 5 Then
            '*---- CALL STATS AND RESET ACCUMULATORS
            Dim lBFIMean As Double : Dim lBFISDEV As Double : Dim lBFICV As Double
            STATS(lNYears, lBFISum, lBFISqr, lBFIMean, lBFISDEV, lBFICV)
            Dim lBFMean As Double : Dim lBFSDEV As Double : Dim lBFCV As Double
            STATS(lNYears, lBFSum, lBFSqr, lBFMean, lBFSDEV, lBFCV)
            Dim lQMean As Double : Dim lQSDEV As Double : Dim lQCV As Double
            STATS(lNYears, lQSum, lQSqr, lQMean, lQSDEV, lQCV)
            With lTsBaseflow.Attributes
                .SetValue("BFIMEAN", lBFIMean) : .SetValue("BFISDEV", lBFISDEV) : .SetValue("BFICV", lBFICV)
                .SetValue("BFMEAN", lBFMean) : .SetValue("BFSDEV", lBFSDEV) : .SetValue("BFCV", lBFCV)
                .SetValue("QMEAN", lQMean) : .SetValue("QSDEV", lQSDEV) : .SetValue("QCV", lQCV)
            End With
        End If

        With lTsBaseflow.Attributes
            lAnnualSummary.AppendLine("")
            lAnnualSummary.AppendLine(" Statistics for " & lNYears.ToString.PadLeft(3, " ") & " " & YearBasis & " years at gage 00055150")
            lAnnualSummary.AppendLine(" --------------------------------------------------")
            lAnnualSummary.AppendLine("                                         STANDARD       COEFFICIENT")
            lAnnualSummary.AppendLine("                            MEAN         DEVIATION      OF VARIATION")
            lAnnualSummary.AppendLine(" ---------------------------------------------------------------------")
            lAnnualSummary.AppendLine(" BASE FLOW (AC-FT)     " & String.Format("{0:0.0}", .GetValue("BFMEAN")).PadLeft(10, " ") & Space(6) & _
                             String.Format("{0:0.0}", .GetValue("BFSDEV")).PadLeft(10, " ") & Space(6) & _
                             String.Format("{0:0.000}", .GetValue("BFCV")).PadLeft(10, " "))
            lAnnualSummary.AppendLine(" TOTAL RUNOFF (AC-FT)  " & String.Format("{0:0.0}", .GetValue("QMEAN")).PadLeft(10, " ") & Space(6) & _
                                 String.Format("{0:0.0}", .GetValue("QSDEV")).PadLeft(10, " ") & Space(6) & _
                                 String.Format("{0:0.000}", .GetValue("QCV")).PadLeft(10, " "))
            lAnnualSummary.AppendLine(" BASE-FLOW INDEX       " & String.Format("{0:0.000}", .GetValue("BFIMEAN")).PadLeft(10, " ") & Space(6) & _
                                 String.Format("{0:0.000}", .GetValue("BFISDEV")).PadLeft(10, " ") & Space(6) & _
                                 String.Format("{0:0.000}", .GetValue("BFICV")).PadLeft(10, " "))
            lAnnualSummary.AppendLine("")
            lAnnualSummary.AppendLine(" * - Denotes years with one or more zero flow days")
            lAnnualSummary.AppendLine(" ^ - Indicates interpolated turning point")
            lAnnualSummary.AppendLine(" ! - Indicates whole year could not be analyzed, but")
            lAnnualSummary.AppendLine("     total annual base flow is extrapolated using the")
            lAnnualSummary.AppendLine("     calculated Base-Flow Index and total annual runoff")

        End With

        lTsBaseflow.Attributes.SetValue("BFIAnnualSummary", lAnnualSummary.ToString())
        lAnnualSummary.Length = 0

        Return lTsBaseflow

    End Function

    ''' <summary>
    ''' ****************************************************************************
    '''* SUBROUTINE TO INTEGRATE BASE FLOW FROM DAY1 TO DAY2.
    '''*
    '''* IF POSSIBLE, USE LOGARITHMIC BASE-FLOW RECESSION TO COMPUTE
    '''* VOLUME OF BASE FLOW.  IF NOT POSSIBLE, (Q1=Q2 OR Q1=0 OR Q2=0)
    '''* ASSUME LINEAR VARIATION.  UNITS OF BASE-FLOW VOLUME ARE CFS-DAYS.
    '''* NOTE THAT THE FORTRAN LOG FUNCTION IS THE NATURAL LOGARITHM.
    ''' ****************************************************************************
    ''' </summary>
    ''' <returns>base-flow volumne from day1 to day2 (cfs-days)</returns>
    ''' <remarks>
    ''' * VARIABLES
    '''*      BF - base-flow volume from DAY1 to DAY2 (cfs-days)
    '''*    DAY1 - day of water year for first turning point
    '''*    DAY2 - day of water year for second turning point
    '''*      Q1 - average daily base flow at first turning point
    '''*      Q2 - average daily base flow at second turning point
    '''*   SLOPE - slope of base-flow recession line (log cycles per day)
    ''' </remarks>
    Private Function BASEFLOW(ByVal aDay1 As Double, ByVal aDay2 As Double, ByVal aQ1 As Double, ByVal aQ2 As Double) As Double
        Dim lBF As Double
        If aQ1 = aQ2 Or aQ1 = 0 Or aQ2 = 0 Then
            lBF = (aQ2 + aQ1) / 2.0 * (aDay2 - aDay1)
        Else
            Dim lSlope As Double = Math.Log(aQ2 / aQ1) / (aDay2 - aDay1)
            lBF = (aQ1 / lSlope) * (Math.Exp(lSlope * (aDay2 - aDay1)) - 1.0)
        End If
        Return lBF
    End Function

    Private Function TestForTurningPoint(ByVal aQMin0 As Double, ByVal aQMin1 As Double, ByVal aQMin2 As Double, _
                               ByVal aDMin0 As Integer, ByVal aDMin1 As Integer, ByVal aDMin2 As Integer, _
                               ByVal aBFMethod As BFMethods) As Boolean

        Dim lTurnPT As Boolean = False
        If aQMin1 <= 0 Then
            lTurnPT = True
        Else
            Select Case aBFMethod
                Case BFMethods.BFIStandard
                    Dim lFrac As Double = aQMin1 * TPTestFraction
                    If aQMin0 * aQMin2 > 0 OrElse aQMin0 * aQMin2 < 0 Then
                        If lFrac <= aQMin0 And lFrac <= aQMin2 Then lTurnPT = True
                    ElseIf aQMin0 = 0 Then
                        If lFrac <= aQMin2 Then lTurnPT = True
                    Else
                        If lFrac <= aQMin0 Then lTurnPT = True
                    End If
                Case BFMethods.BFIModified
                    If aQMin0 * aQMin2 > 0 OrElse aQMin0 * aQMin2 < 0 Then
                        If aQMin1 * Math.Pow(OneDayRecessConstant, aDMin1 - aDMin0) <= aQMin0 And _
                           aQMin1 * Math.Pow(OneDayRecessConstant, aDMin2 - aDMin1) <= aQMin2 Then
                            lTurnPT = True
                        End If
                    ElseIf aQMin0 = 0 Then
                        If aQMin1 * Math.Pow(OneDayRecessConstant, aDMin2 - aDMin1) <= aQMin2 Then lTurnPT = True
                    Else
                        If aQMin1 * Math.Pow(OneDayRecessConstant, aDMin1 - aDMin0) <= aQMin0 Then lTurnPT = True
                    End If
            End Select
        End If

        Return lTurnPT
    End Function

    ''' <summary>
    ''' ****************************************************************************
    '''* PERFORM STRAIGHT-LINE INTERPOLATION BETWEEN TWO BASE-FLOW DATA
    '''* POINTS PLOTTED ON SEMI-LOG PAPER.  THE POINTS (D1,Q1) AND (D3,Q3)
    '''* SPECIFY THE LINE ON SEMI-LOG PAPER.  QINTERP IS COMPUTED AT DAY
    '''* D2.  IF Q1=Q3 OR IF Q1=0 OR IF Q3=0, THEN LOGARITHMIC INTERPOLATION
    '''* CAN NOT BE USED.  IN THESE CASES THE FLOW WILL BE ASSUMED TO VARY
    '''* LINEARLY FROM DAY D1 TO DAY D3.
    '''*
    '''* VARIABLES
    '''*        D1 - day 1
    '''*        D2 - day 2 (real)
    '''*        D3 - day 3
    '''*        Q1 - discharge on day 1
    '''*        Q3 - discharge on day 3
    '''****************************************************************************
    ''' </summary>
    ''' <returns>
    ''' QINTERP - discharge on day 2, determined by interpolation between
    '''* day 1 and day 3
    ''' </returns>
    ''' <remarks></remarks>
    Private Function QINTERP(ByVal D1 As Double, ByVal Q1 As Double, ByVal D3 As Double, ByVal Q3 As Double, ByVal D2 As Double) As Double
        Dim lInterpolatedFlow As Double
        If D2 - D1 < 0.01 Then
            lInterpolatedFlow = Q1
        ElseIf D3 - D2 < 0.01 Then
            lInterpolatedFlow = Q3
        ElseIf Q1 = -99 Or Q3 = -99 Then
            lInterpolatedFlow = -99
        ElseIf Q1 = Q3 Or Q1 = 0 Or Q3 = 0 Then
            lInterpolatedFlow = Q1 + (Q3 - Q1) / (D3 - D1) * (D2 - D1)
        Else
            lInterpolatedFlow = Math.Exp(Math.Log(Q1) + Math.Log(Q3 / Q1) / (D3 - D1) * (D2 - D1))
        End If
        Return lInterpolatedFlow
    End Function

    ''' <summary>
    '''*****************************************************************
    '''* COMPUTE MEAN, STANDARD DEVIATION, AND COEFFICIENT OF VARIATION
    '''*****************************************************************
    ''' </summary>
    ''' <param name="aN">number of data items (NYears)</param>
    ''' <param name="aSum">sum of x</param>
    ''' <param name="aSumSqr">sum of x**2</param>
    ''' <param name="aXBar">mean of x</param>
    ''' <param name="aSDev">standard deviation of x</param>
    ''' <param name="aCV">coefficient of variation of x</param>
    ''' <remarks></remarks>
    Private Sub STATS(ByVal aN As Integer, ByVal aSum As Double, ByVal aSumSqr As Double, _
                      ByRef aXBar As Double, ByRef aSDev As Double, ByRef aCV As Double)
        aXBar = aSum / aN
        aSDev = Math.Sqrt((aSumSqr - (Math.Pow(aSum, 2.0) / aN)) / (aN - 1))
        If aXBar = 0 Then
            aCV = 0
        Else
            aCV = aSDev / aXBar
        End If
    End Sub

    Public Sub WriteOutBFI(ByVal aFileStream As IO.StreamWriter)
        aFileStream.WriteLine("        Input(file = " & "excelbfi.txt")
        aFileStream.WriteLine("        File(Format = Web / rdb(NWIS - W)")
        aFileStream.WriteLine("                      Base-flow output file = " & "excelbfi.bfi")
        aFileStream.WriteLine("                  Turning point output file = " & "excelbfi.tp")
        aFileStream.WriteLine(" Daily base flow and total flow output file = " & "excelbfi.q")
        aFileStream.WriteLine("")
        aFileStream.WriteLine(" Program Version = BFI 4.15")
        aFileStream.WriteLine("")
        aFileStream.WriteLine(" AVAILABLE SEPARATION METHODS:")
        If Method = BFMethods.BFIStandard Then
            aFileStream.WriteLine(" * 1 = STANDARD Institute of Hydrology method")
            aFileStream.WriteLine("       (N-day avg. recession test; uses ""N"" and ""f"")")
            aFileStream.WriteLine("   2 = MODIFIED method")
            aFileStream.WriteLine("       (1-day recession constant adjusted for number of days")
            aFileStream.WriteLine("        between points; uses ""N"" and ""K"")")
        ElseIf Method = BFMethods.BFIModified Then
            aFileStream.WriteLine("   1 = STANDARD Institute of Hydrology method")
            aFileStream.WriteLine("       (N-day avg. recession test; uses ""N"" and ""f"")")
            aFileStream.WriteLine(" * 2 = MODIFIED method")
            aFileStream.WriteLine("       (1-day recession constant adjusted for number of days")
            aFileStream.WriteLine("        between points; uses ""N"" and ""K"")")
        End If
        aFileStream.WriteLine("")
        aFileStream.WriteLine(" BASE-FLOW SEPARATION PARAMETERS")
        If Method = BFMethods.BFIStandard Then
            aFileStream.WriteLine("   METHOD =   1")
        ElseIf Method = BFMethods.BFIModified Then
            aFileStream.WriteLine("   METHOD =   2")
        End If

        aFileStream.WriteLine("   N      =" & PartitionLengthInDays.ToString.PadLeft(4, " "))
        aFileStream.WriteLine("   f      =" & DoubleToString(OneDayRecessConstant, , , , , ))
        For I As Integer = 1 To 7
            aFileStream.WriteLine("")
        Next
        aFileStream.WriteLine(" =============================================================================")
        aFileStream.WriteLine(" Base-Flow Index for gage " & "00055150")
        aFileStream.WriteLine(" agency " & "00055150" & " sample data")
        aFileStream.WriteLine(" Calendar   Base-Flow      Base Flow     Total Runoff | Day of Turning Point |")
        aFileStream.WriteLine(" Year         Index        (acre-ft)       (acre-ft)  |  [First]     [Last]  |")
        aFileStream.WriteLine(" -----------------------------------------------------------------------------")

        aFileStream.WriteLine(" Statistics for   5 Calendar years at gage 00055150")
        aFileStream.WriteLine(" --------------------------------------------------")
        aFileStream.WriteLine("                                         STANDARD       COEFFICIENT")
        aFileStream.WriteLine("                            MEAN         DEVIATION      OF VARIATION")
        aFileStream.WriteLine(" ---------------------------------------------------------------------")
        aFileStream.WriteLine(" BASE FLOW (AC-FT)        36579.9         13028.1            .356")
        aFileStream.WriteLine(" TOTAL RUNOFF (AC-FT)    487202.7        152783.8            .314")
        aFileStream.WriteLine(" BASE-FLOW INDEX             .078            .029            .371")
        aFileStream.WriteLine("")
        aFileStream.WriteLine(" * - Denotes years with one or more zero flow days")
        aFileStream.WriteLine(" ^ - Indicates interpolated turning point")
        aFileStream.WriteLine(" ! - Indicates whole year could not be analyzed, but")
        aFileStream.WriteLine("     total annual base flow is extrapolated using the")
        aFileStream.WriteLine("     calculated Base-Flow Index and total annual runoff")
    End Sub

    Public Overrides Sub Clear()
        'do nothing
    End Sub
End Class
