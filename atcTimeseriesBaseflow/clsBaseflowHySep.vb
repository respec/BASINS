Imports atcData
Imports atcUtility

Public Class clsBaseflowHySep
    Inherits clsBaseflow
    Private pIntervalInDay As Integer
    Public Property IntervalInDays() As Integer
        Get
            Dim lRINTR As Double = DrainageArea ^ 0.2
            lRINTR *= 2.0
            If (lRINTR <= 4.0) Then
                pIntervalInDay = 3
            ElseIf (lRINTR <= 6.0 And lRINTR > 4.0) Then
                pIntervalInDay = 5
            ElseIf (lRINTR <= 8.0 And lRINTR > 6.0) Then
                pIntervalInDay = 7
            ElseIf (lRINTR <= 10 And lRINTR > 8.0) Then
                pIntervalInDay = 9
            Else
                pIntervalInDay = 11
            End If
            Return pIntervalInDay
        End Get
        Set(ByVal value As Integer)
            pIntervalInDay = value
        End Set
    End Property

    Private pMethod As HySepMethod
    Public Property Method() As HySepMethod
        Get
            Return pMethod
        End Get
        Set(ByVal value As HySepMethod)
            pMethod = value
        End Set
    End Property

    Private pDaysInBuffer As Integer = 11
    Public Property DaysInBuffer() As Integer
        Get
            Return pDaysInBuffer
        End Get
        Set(ByVal value As Integer)
            pDaysInBuffer = value
        End Set
    End Property

    'Private pFlagLast As Boolean
    'Public Property FlagLast() As Boolean
    '    Get
    '        Return pFlagLast
    '    End Get
    '    Set(ByVal value As Boolean)
    '        pFlagLast = value
    '    End Set
    'End Property

    Public Function DoBaseFlowSeparation() As atcTimeseries
        If TargetTS Is Nothing Then
            Return Nothing
        End If

        'Start organizing here using the original HYSEP scheme
        'that is to do one year at a time
        '
        'Seems key is to have up to 11-day before and after the duration of interest
        Dim lTsStart As Double = TargetTS.Attributes.GetValue("SJDay")
        Dim lTsEnd As Double = TargetTS.Attributes.GetValue("EJDay")

        Dim lDaysPrev As Integer = timdifJ(lTsStart, StartDate, atcTimeUnit.TUDay, 1)
        Dim lDaysPost As Integer = timdifJ(EndDate, lTsEnd, atcTimeUnit.TUDay, 1)

        If lDaysPrev >= 11 Then lDaysPrev = 11
        lTsStart = TimAddJ(StartDate, atcTimeUnit.TUDay, 1, -1 * lDaysPrev)

        If lDaysPost >= 11 Then lDaysPost = 11
        lTsEnd = TimAddJ(EndDate, atcTimeUnit.TUDay, 1, lDaysPost)

        'Make sure the ts is in daily timestep
        Dim lTsDaily As atcTimeseries = SubsetByDate(TargetTS, lTsStart, lTsEnd, Nothing)
        If Not lTsDaily.Attributes.GetValue("Tu") = atcTimeUnit.TUDay Then
            lTsDaily = Aggregate(lTsDaily, atcTimeUnit.TUDay, 1, atcTran.TranAverSame, Nothing)
        End If

        'Get drainage area
        'if metric units chosen, convert drainage area (square miles2) to square km2
        'unitflag 2 is to use metric units
        If UnitFlag = 2 Then DrainageArea = DrainageArea * 2.59

        Dim lTsBF As atcTimeseries = Nothing
        Select Case Method
            Case HySepMethod.FIXED
                lTsBF = HySepFixed(lTsDaily)
            Case HySepMethod.SLIDE
                lTsBF = HySepSlide(lTsDaily)
            Case HySepMethod.LOCMIN
                lTsBF = HySepLocMin(lTsDaily)
        End Select

        'Adjust for pre- and post- duration dates
        lTsBF = SubsetByDate(lTsBF, StartDate, EndDate, Nothing)

        Return lTsBF
    End Function

    Public Function HySepFixed(ByVal aTS As atcTimeseries) As atcTimeseries
        Dim lInterv As Integer = IntervalInDays
        Dim lValueBaseflow() As Double
        ReDim lValueBaseflow(aTS.numValues)

        Dim lGoodValue As Double = 0
        Dim lStartInd As Integer = 0
        For I As Integer = 0 To aTS.numValues
            If aTS.Value(I) >= 0 Then
                lValueBaseflow(I) = 0
                lGoodValue = 1
            Else
                lValueBaseflow(I) = aTS.Value(I)
                If lGoodValue = 0 And I > lStartInd Then lStartInd = I
            End If
        Next

        Dim lFxDays As Integer = aTS.numValues
        'last year to process: reduce total days for processing by 
        'the number of missing values within extra 11 days at end
        'If FlagLast Then
        'End If
        For I As Integer = aTS.numValues To aTS.numValues - (DaysInBuffer - 1) Step -1
            If aTS.Value(I) < 0 Then
                lFxDays = lFxDays - 1
            End If
        Next

        Dim lK As Integer = Math.Floor((lFxDays - lStartInd) / lInterv)
        Dim lPMIN As Double
        Dim L1 As Integer
        Dim L2 As Integer
        Dim J As Integer 'would-be last index in the timeseries array for baseflow
        If lK >= 1 Then 'at least have 1 interval to analyse
            For I As Integer = 1 To lK
                lPMIN = 100000.0
                L1 = (I - 1) * lInterv + lStartInd + 1
                L2 = I * lInterv + lStartInd
                For J = L1 To L2
                    If aTS.Value(J) < lPMIN Then lPMIN = aTS.Value(J)
                Next
                For J = L1 To L2
                    lValueBaseflow(J) = lPMIN
                Next
                If I = 220 Then
                    Dim Checkpoint1 As String = "Checkpoint1"
                End If
            Next
        End If

        'last year to process
        If L2 < lFxDays Then
            'extra days left over after process K intervals
            'do baseflow separation on those days by themselves
            lPMIN = 100000.0
            For J = L2 + 1 To lFxDays
                If aTS.Value(J) < lPMIN Then lPMIN = aTS.Value(J)
            Next
            For J = L2 + 1 To lFxDays
                lValueBaseflow(J) = lPMIN
            Next
        End If

        'TODO: further determine if below now becomes unnecessary
        'If FlagLast Then
        '    'moved outside this branch as it has to be done regardless
        '    'as there always going to be an end
        'Else
        '    'not last year to process
        '    'set START for next year so that first few days of next year
        '    'use some of last few days of this year to determin baseflow

        '    'find last day processed
        '    Dim lD As Integer = lStartInd + lK * lInterv
        '    'find last day of year in array
        '    Dim lA As Integer = aTS.numValues - DaysInBuffer
        '    'move START back in interval increments
        '    Do While (lD > lA)
        '        lD -= lInterv
        '        lStartInd = DaysInBuffer - (lA - lD)
        '    Loop
        'End If

        Dim lTsBaseflow As atcTimeseries = aTS.Clone()
        lTsBaseflow.Values = lValueBaseflow

        'Dim lBFStart As Double = aTS.Dates.Value(lStartInd - 1)
        'Dim lBFEnd As Double = aTS.Dates.Value(J)
        'Dim lNewDates As New atcTimeseries(Nothing)
        'lNewDates.Values = NewDates(lBFStart, lBFEnd, atcTimeUnit.TUDay, 1)

        'Dim lValueBaseflowFinal() As Double
        'ReDim lValueBaseflowFinal(J - lStartInd + 1)
        'lValueBaseflowFinal(0) = GetNaN()
        'For I As Integer = lStartInd To J
        '    lValueBaseflowFinal(I - lStartInd + 1) = lValueBaseflow(I)
        'Next
        'lTsBaseflow.Dates = lNewDates
        'lTsBaseflow.Values = lValueBaseflowFinal
        Return lTsBaseflow
    End Function

    Public Function HySepSlide(ByVal aTS As atcTimeseries) As atcTimeseries
        Dim lInterv As Integer = IntervalInDays
        Dim lValueBaseflow() As Double
        ReDim lValueBaseflow(aTS.numValues)

        Dim lGoodValue As Double = 0
        Dim lStartInd As Integer = 0
        For I As Integer = 0 To aTS.numValues
            If aTS.Value(I) >= 0 Then
                lValueBaseflow(I) = 0
                lGoodValue = 1
            Else
                lValueBaseflow(I) = aTS.Value(I)
                If lGoodValue = 0 And I >= 0 Then lStartInd = I
            End If
        Next

        Dim lmidInterv As Integer = (lInterv - 1) / 2
        Dim S1 As Integer = lStartInd + 1
        Dim lDay As Integer
        Dim lPMIN As Double
        Dim lK2 As Integer 'end of the interval index
        Dim lK1 As Integer 'beg of the interval index
        Dim J As Integer
        For I As Integer = S1 To aTS.numValues
            If aTS.Value(I) >= 0 Then
                lPMIN = 100000.0
                'set DAY equal to index of current day in year
                lDay = I - lStartInd
                If lDay <= lmidInterv Then
                    'when day near beginning
                    lK2 = I + lmidInterv
                    For J = S1 To lK2
                        If aTS.Value(J) < lPMIN Then lPMIN = aTS.Value(J)
                    Next
                    lValueBaseflow(I) = lPMIN
                ElseIf aTS.numValues - I <= lmidInterv Then
                    'when day near end
                    lK1 = I - lmidInterv
                    For J = lK1 To aTS.numValues
                        If aTS.Value(J) < lPMIN Then lPMIN = aTS.Value(J)
                    Next
                    lValueBaseflow(I) = lPMIN
                Else
                    'when day not near beg or end
                    lK1 = I - lmidInterv
                    lK2 = I + lmidInterv
                    For J = lK1 To lK2
                        If aTS.Value(J) < lPMIN Then lPMIN = aTS.Value(J)
                    Next
                    lValueBaseflow(I) = lPMIN
                End If
            End If
        Next

        Dim lTsBaseflow As atcTimeseries = aTS.Clone()
        lTsBaseflow.Values = lValueBaseflow

        Return lTsBaseflow
    End Function

    Public Function HySepLocMin(ByVal aTS As atcTimeseries) As atcTimeseries
        Dim lInterv As Integer = IntervalInDays
        Dim lValueBaseflow() As Double
        ReDim lValueBaseflow(aTS.numValues)

        Dim lDaysWithGoodValue As Integer = 0
        Dim lStartInd As Integer = 0
        Dim lIndex As Integer = 0 'ID in the original program
        Dim lendIndex As Integer = 0 'END in the original program
        Dim lPFLAG As Boolean = False 'signify finding good period to process
        Dim lIPoint(aTS.numValues) As Integer 'IPOINT(400) in original code
        Dim lNumPt As Integer 'NUMPT in original code

        Do While lendIndex < aTS.numValues And lIndex < aTS.numValues
            'loop for periods of good data
            lNumPt = 0
            lDaysWithGoodValue = 0
            lPFLAG = False
            Do While lIndex < aTS.numValues And Not lPFLAG
                'find start and end of good values
                lIndex += 1
                If aTS.Value(lIndex) >= 0 Then 'good value
                    lValueBaseflow(lIndex) = 0
                    If lDaysWithGoodValue = 0 Then lStartInd = lIndex
                    lDaysWithGoodValue += 1
                    lendIndex = lIndex
                Else ' bad value
                    If lDaysWithGoodValue = 0 Then
                        'no good values yet
                        lValueBaseflow(lIndex) = aTS.Value(lIndex)
                    ElseIf lDaysWithGoodValue < lInterv Then
                        'not enough good values to process
                        For J As Integer = 1 To lDaysWithGoodValue
                            lValueBaseflow(lIndex - J) = -999.0
                        Next
                        lDaysWithGoodValue = 0
                    Else
                        'found good period to process
                        lPFLAG = True
                        lValueBaseflow(lIndex) = -999.0
                    End If
                End If
            Loop

            If Not lDaysWithGoodValue >= lInterv Then
                Continue Do
            End If

            'if have good period to process, then continue down
            Dim lEnd As Integer 'L in the original code
            Dim lS As Integer 'S in the original code

            Dim lSearchRadius As Integer = (lInterv - (lInterv Mod 2)) / 2
            lEnd = lendIndex - lSearchRadius
            lS = lStartInd + lSearchRadius
            Dim lFoundLocMin As Boolean
            For I As Integer = lS To lEnd
                lFoundLocMin = False
                For J As Integer = 1 To lSearchRadius
                    If aTS.Value(I) <= aTS.Value(I + J) And aTS.Value(I) <= aTS.Value(I - J) Then
                        lFoundLocMin = True
                    Else
                        lFoundLocMin = False
                        Exit For
                    End If
                Next
                If lFoundLocMin Then
                    lNumPt += 1
                    lIPoint(lNumPt) = I
                End If
            Next

            If lNumPt > 0 Then
                'at least one local minimum found in good period
                'being analyzed

                'set beginning values to first local minimum
                For I As Integer = lStartInd To lIPoint(1)
                    lValueBaseflow(I) = aTS.Value(lIPoint(1))
                Next

                'set ending values to last local minimum
                For I As Integer = lIPoint(lNumPt) To lendIndex
                    lValueBaseflow(I) = aTS.Value(lIPoint(lNumPt))
                Next

                'set all values in the middle
                For I As Integer = 1 To lNumPt - 1
                    Dim lP1 As Integer = lIPoint(I)
                    Dim lP2 As Integer = lIPoint(I + 1)
                    lValueBaseflow(lP1) = aTS.Value(lP1)
                    lValueBaseflow(lP2) = aTS.Value(lP2)
                    For J As Integer = lP1 To lP2
                        If lValueBaseflow(lP1) <= 0 Then lValueBaseflow(lP1) = 0.01
                        If lValueBaseflow(lP2) <= 0 Then lValueBaseflow(lP2) = 0.01
                        Dim lV As Double = (J - lP1) / (lP2 - lP1) * 1.0
                        Dim lExp As Double = lV * (Math.Log10(lValueBaseflow(lP2)) - Math.Log10(lValueBaseflow(lP1))) + Math.Log10(lValueBaseflow(lP1))
                        lValueBaseflow(J) = 10.0 ^ lExp
                    Next
                Next

            Else 'no local minimum found in period analyzed
                For I As Integer = lStartInd To lendIndex
                    lValueBaseflow(I) = -999.0
                Next
            End If
        Loop 'next value's index

        For I As Integer = 1 To aTS.numValues
            If lValueBaseflow(I) > aTS.Value(I) Then lValueBaseflow(I) = aTS.Value(I)
        Next

        Dim lTsBaseflow As atcTimeseries = aTS.Clone
        With lTsBaseflow
            .Attributes.SetValue("Constituent", "Baseflow")
            .Attributes.SetValue("Scenario", "HySEPLocMin")
            .Values = lValueBaseflow
        End With
        Return lTsBaseflow
    End Function
End Class
