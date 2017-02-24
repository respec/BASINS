Imports atcData
Imports atcUtility

Public Class clsRecessionSegment
    Implements IComparable

    Public Shared RecessionCount As Integer
    Public Shared StreamFlowTS As atcTimeseries
    Public Shared MaxSegmentLengthInDays As Integer = 60

    '...FALL...
    Public Shared MaxSegmentLengthInDaysGW As Integer = 300
    Public HzeroDayIndex As Integer
    Public HzeroDayDate As Double
    Public HzeroDayValue As Double
    Public Recharges As atcCollection
    Public AntecedentGWLMethods As atcCollection 'This is a collection in the form of ant.Method vs H2
    '...FALL...

    Public PeakDayIndex As Integer
    Public PeakDayDate As Double
    Public SegmentLength As Integer
    Public Coefficient1 As Double
    Public Coefficient2 As Double
    Public DaysLogCycle As Double = -1 * Coefficient1

    Public IsExcluded As Boolean = False
    Public NeedtoReadData As Boolean = True
    Public MeanLogQ As Double
    Public MeanOrdinals As Double

    Private pMinDayOrdinal As Integer
    Private pMaxDayOrdinal As Integer
    Private pMinDayOrdinalChanged As Boolean = False
    Private pMaxDayOrdinalChanged As Boolean = False

    Private pBackTraceFlag As Dictionary(Of Integer, Boolean)
    Private pBFImaxEstimates As Dictionary(Of String, Double)

    Public Property MinDayOrdinal() As Integer
        Get
            Return pMinDayOrdinal
        End Get
        Set(ByVal value As Integer)
            If value <> pMinDayOrdinal Then
                pMinDayOrdinal = value
                pMinDayOrdinalChanged = True
            Else
                pMinDayOrdinalChanged = False
            End If
        End Set
    End Property
    Public Property MaxDayOrdinal() As Integer
        Get
            Return pMaxDayOrdinal
        End Get
        Set(ByVal value As Integer)
            If value <> pMaxDayOrdinal Then
                pMaxDayOrdinal = value
                pMaxDayOrdinalChanged = True
            Else
                pMaxDayOrdinalChanged = False
            End If
        End Set
    End Property

    Private pNeedToAnalyse As Boolean = True
    Public Property NeedToAnalyse() As Boolean
        Get
            If Coefficient1 = 0 AndAlso Coefficient1 = Coefficient2 AndAlso Coefficient1 = MeanLogQ AndAlso Coefficient1 = MeanOrdinals Then
                pNeedToAnalyse = True
            ElseIf pMinDayOrdinalChanged OrElse pMaxDayOrdinalChanged Then
                pNeedToAnalyse = True
            Else
                pNeedToAnalyse = False
            End If
            Return pNeedToAnalyse
        End Get
        Set(ByVal value As Boolean) 'set right after it is analysed
            If Not value Then
                pMinDayOrdinalChanged = False
                pMaxDayOrdinalChanged = False
            End If
        End Set
    End Property

    'for display
    Public ReadOnly Property BestFitEquation() As String
        Get
            Return "T = ( " & String.Format("{0:0.0000}", Coefficient1).PadLeft(8, " ") & " * LOGQ )  +  " & String.Format("{0:0.0000}", Coefficient2).PadLeft(8, " ")
        End Get
    End Property

    Public ReadOnly Property PeakDayDateToString() As String
        Get
            Dim lDate(5) As Integer
            J2Date(PeakDayDate, lDate)
            Return lDate(0).ToString & "/" & lDate(1).ToString.PadLeft(2, " ") & "/" & lDate(2).ToString.PadLeft(2, " ")
        End Get
    End Property

    Public ReadOnly Property EndDayDateToString(Optional ByVal aBFIEstimateEndDay As Boolean = False) As String
        Get
            Dim lastDayIndex As Integer = PeakDayIndex + SegmentLength - 1
            If aBFIEstimateEndDay Then
                lastDayIndex -= 1
            End If
            Dim lEndDate = StreamFlowTS.Dates.Value(lastDayIndex - 1)
            Dim lDate(5) As Integer
            J2Date(lEndDate, lDate)
            Return lDate(0).ToString & "/" & lDate(1).ToString.PadLeft(2, " ") & "/" & lDate(2).ToString.PadLeft(2, " ")
        End Get
    End Property

    Public ReadOnly Property HzeroDayDateToString() As String
        Get
            Dim lDate(5) As Integer
            J2Date(HzeroDayDate, lDate)
            Return lDate(0).ToString & "/" & lDate(1).ToString.PadLeft(2, " ") & "/" & lDate(2).ToString.PadLeft(2, " ")
        End Get
    End Property

    Public ReadOnly Property BackTraceContinuousFlag(ByVal aBackTraceDays As Integer) As Boolean
        Get
            If pBackTraceFlag.ContainsKey(aBackTraceDays) Then
                Return pBackTraceFlag(aBackTraceDays)
            Else
                Dim lastDayIndex As Integer = PeakDayIndex + SegmentLength - 1
                Dim lContinuous As Boolean = True
                If lastDayIndex - aBackTraceDays < 0 Then
                    lContinuous = False
                    pBackTraceFlag.Add(aBackTraceDays, lContinuous)
                    Return lContinuous
                End If
                Dim lflowVal As Double
                Dim lDayCount As Integer = 0
                For I As Integer = lastDayIndex To 0 Step -1
                    lflowVal = StreamFlowTS.Value(I)
                    If Double.IsNaN(lflowVal) AndAlso lDayCount < aBackTraceDays Then
                        lContinuous = False
                        Exit For
                    End If
                    If lDayCount < aBackTraceDays Then
                        lDayCount += 1
                    Else
                        Exit For
                    End If
                Next 'loop 220
                pBackTraceFlag.Add(aBackTraceDays, lContinuous)
                Return lContinuous
            End If
        End Get
    End Property

    Public ReadOnly Property BackTraceTimeSeries(ByVal aBackTraceDays As Integer) As atcTimeseries
        Get
            Dim lTsBacktrace As atcTimeseries = Nothing
            Dim lStartIndex As Integer = PeakDayIndex + SegmentLength - aBackTraceDays - 1
            Dim lEndIndex As Integer = PeakDayIndex + SegmentLength - 1
            'If lStartIndex < 1 Then Return lTsBacktrace
            If lStartIndex < 1 Then
                lStartIndex = 1
            End If

            Dim lStartDate As Double = StreamFlowTS.Dates.Value(lStartIndex - 1)
            Dim lEndDate As Double = StreamFlowTS.Dates.Value(lEndIndex - 1)
            Try
                lTsBacktrace = SubsetByDate(StreamFlowTS, lStartDate, lEndDate, Nothing)
            Catch ex As Exception
                lTsBacktrace = Nothing
            End Try
            Return lTsBacktrace
        End Get
    End Property

    Public Function EstimateBFImax_CF(ByVal aTSflow As atcTimeseries,
                                      ByRef aDF2P As clsRecess.DF2P,
                                      Optional ByRef aTS_b_prime As atcTimeseries = Nothing,
                                      Optional ByVal aRecalculate As Boolean = True) As String
        If aTSflow Is Nothing OrElse Double.IsNaN(aDF2P.RecessionConstant) Then
            Return "ERROR:Invalid Input"
        End If
        'If aTSflow.numValues < 366 Then
        '    Return "ERROR:Record too Short"
        'End If

        Dim lKey_attr As String = "BFImax_CF"
        Dim lKey_collection As String = aTSflow.numValues.ToString() & "-" & aDF2P.RecessionConstant.ToString()

        If Not aRecalculate Then
            If pBFImaxEstimates.ContainsKey(lKey_collection) Then
                aTSflow.Attributes.SetValue(lKey_attr, pBFImaxEstimates(lKey_collection))
                Return "SUCCESS," & lKey_attr
            End If
        End If

        Dim lTsWaterYearBound As atcTimeseries = Nothing
        'If aTSflow.numValues > 365 Then
        '    Try
        '        lTsWaterYearBound = SubsetByDateBoundary(aTSflow, 10, 1, Nothing)
        '    Catch ex As Exception
        '        lTsWaterYearBound = Nothing
        '    End Try
        '    If lTsWaterYearBound Is Nothing Then
        '        Return "ERROR:Extract water year data failed"
        '    End If
        'End If

        aTS_b_prime = aTSflow.Clone()
        For I As Integer = 0 To aTS_b_prime.numValues
            aTS_b_prime.Value(I) = Double.NaN
        Next
        aTS_b_prime.Attributes.SetValue("Constituent", "b'")

        Dim lflowVal As Double
        Dim lflowVal_prev As Double
        Dim lb_prime As Double
        lflowVal = aTSflow.Value(aTSflow.numValues)
        lflowVal_prev = aTSflow.Value(aTSflow.numValues - 1)
        If lflowVal / aDF2P.RecessionConstant < lflowVal_prev Then
            lb_prime = lflowVal / aDF2P.RecessionConstant
        Else
            lb_prime = lflowVal_prev
        End If
        aTS_b_prime.Value(aTSflow.numValues - 1) = lb_prime
        For I As Integer = aTS_b_prime.numValues - 2 To 1 Step -1
            If aTS_b_prime.Value(I + 1) / aDF2P.RecessionConstant < aTSflow.Value(I) Then
                aTS_b_prime.Value(I) = aTS_b_prime.Value(I + 1) / aDF2P.RecessionConstant
            Else
                aTS_b_prime.Value(I) = aTSflow.Value(I)
            End If
        Next

        Dim lSumFlow As Double = 0.0
        Dim lSum_b_prime As Double = 0.0
        'If aTSflow.numValues > 365 AndAlso lTsWaterYearBound.Values IsNot Nothing Then
        '    Dim lTs_b_prime_WY As atcTimeseries = SubsetByDateBoundary(aTS_b_prime, 10, 1, Nothing)
        '    lSumFlow = lTsWaterYearBound.Attributes.GetValue("Sum")
        '    lSum_b_prime = lTs_b_prime_WY.Attributes.GetValue("Sum")
        '    aTSflow.Attributes.SetValue("BFIEstimateStartDate", lTsWaterYearBound.Dates.Value(0))
        '    aTSflow.Attributes.SetValue("BFIEstimateEndDate", lTsWaterYearBound.Dates.Value(lTsWaterYearBound.numValues))
        '    aDF2P.BFIEstimateStartDate = lTsWaterYearBound.Dates.Value(0)
        '    aDF2P.BFIEstimateEndDate = lTsWaterYearBound.Dates.Value(lTsWaterYearBound.numValues)
        '    aDF2P.isBFIEstimateInWaterYear = True
        'Else
        lSumFlow = aTSflow.Attributes.GetValue("Sum")
        lSum_b_prime = aTS_b_prime.Attributes.GetValue("Sum")
        aTSflow.Attributes.SetValue("BFIEstimateStartDate", aTSflow.Dates.Value(0))
        aTSflow.Attributes.SetValue("BFIEstimateEndDate", aTSflow.Dates.Value(aTSflow.numValues - 1))
        aDF2P.BFIEstimateStartDate = aTSflow.Dates.Value(0)
        aDF2P.BFIEstimateEndDate = aTSflow.Dates.Value(aTSflow.numValues - 1)
        aDF2P.isBFIEstimateInWaterYear = False
        'End If
        aDF2P.BFImax = lSum_b_prime / lSumFlow
        aTSflow.Attributes.SetValue(lKey_attr, aDF2P.BFImax)
        If Not pBFImaxEstimates.ContainsKey(lKey_collection) Then
            pBFImaxEstimates.Add(lKey_collection, aDF2P.BFImax)
        End If
        Return "SUCCESS," & lKey_attr
    End Function

    Public Flow() As Double
    Public QLog() As Double
    Public Dates() As Double

    Public Sub New()
        'ReDim Flow(MaxSegmentLengthInDays)
        'ReDim QLog(MaxSegmentLengthInDays)
        'ReDim Dates(MaxSegmentLengthInDays)
        AntecedentGWLMethods = New atcCollection() 'FALL
        Recharges = New atcCollection() 'FALL
        pBackTraceFlag = New Dictionary(Of Integer, Boolean)
        pBFImaxEstimates = New Dictionary(Of String, Double)
    End Sub

    Public Sub GetData(Optional ByVal aSetGWPPElev As Boolean = False, Optional ByVal aPPElev As Double = Double.NaN)
        ReDim Flow(SegmentLength)
        ReDim QLog(SegmentLength)
        ReDim Dates(SegmentLength)

        For I As Integer = 1 To SegmentLength 'loop 215
            Flow(I) = 0.0
            QLog(I) = -99.9
        Next 'loop 215

        Dim lUseNaturalLog As Boolean = False
        Dim lDataType As String = StreamFlowTS.Attributes.GetValue("Constituent")
        If lDataType.ToUpper() = "GW LEVEL" Then
            lUseNaturalLog = True
        End If
        Dim lIndex As Integer
        For I As Integer = 1 To SegmentLength 'loop 220
            lIndex = I - 1 + PeakDayIndex
            Flow(I) = StreamFlowTS.Value(lIndex)
            If aSetGWPPElev AndAlso Not Double.IsNaN(aPPElev) Then
                Flow(I) -= aPPElev
            End If

            If Flow(I) = 0.0 Then
                QLog(I) = -88.8
            Else
                If Flow(I) > 0 Then
                    If lUseNaturalLog Then
                        QLog(I) = Math.Log(Flow(I))
                    Else
                        QLog(I) = Math.Log10(Flow(I))
                    End If
                Else
                    If lUseNaturalLog Then
                        QLog(I) = Math.Log(Flow(I) * -1)
                    Else
                        QLog(I) = Math.Log10(Flow(I) * -1) 'Actually, might need to raise objection in the interface for this
                    End If
                End If
            End If
            Dates(I) = StreamFlowTS.Dates.Value(lIndex - 1)
        Next 'loop 220
        NeedtoReadData = False
    End Sub


    Public Sub SetPourpointElevation(ByVal aPPElev As Double)
        Dim lUseNaturalLog As Boolean = False
        Dim lDataType As String = StreamFlowTS.Attributes.GetValue("Constituent")
        If lDataType.ToUpper() = "GW LEVEL" Then
            lUseNaturalLog = True
        End If
        'if aPPElev changes, needs to first GetData, then do this routine again
        For I As Integer = 1 To SegmentLength 'loop 220
            Flow(I) = StreamFlowTS.Value(I + PeakDayIndex)
            Flow(I) -= aPPElev
            If Flow(I) = 0.0 Then
                QLog(I) = -88.8
            Else
                If Flow(I) > 0 Then
                    If lUseNaturalLog Then
                        QLog(I) = Math.Log(Flow(I))
                    Else
                        QLog(I) = Math.Log10(Flow(I))
                    End If

                Else
                    If lUseNaturalLog Then
                        QLog(I) = Math.Log(Flow(I) * -1)
                    Else
                        QLog(I) = Math.Log10(Flow(I) * -1) 'Actually, might need to raise objection in the interface for this
                    End If
                End If
            End If
            Dates(I) = StreamFlowTS.Dates.Value(I + PeakDayIndex - 1)
        Next 'loop 220
    End Sub
    Public Sub GetDataSubset()
        If MinDayOrdinal = MaxDayOrdinal AndAlso MaxDayOrdinal = 0 Then
            Exit Sub
        End If

        Dim lnewLength As Integer = MaxDayOrdinal - MinDayOrdinal + 1

        Dim lFlow(lnewLength) As Double
        Dim lQLog(lnewLength) As Double
        Dim lDates(lnewLength) As Double
        For I As Integer = 1 To lnewLength 'loop 215
            lFlow(I) = 0.0
            lQLog(I) = -99.9
        Next 'loop 215

        Dim lNewIndex As Integer = 0
        For I As Integer = 1 To SegmentLength 'loop 220
            If I < MinDayOrdinal OrElse I > MaxDayOrdinal Then
                Continue For
            Else
                lFlow(lNewIndex) = Flow(I)
                lQLog(lNewIndex) = QLog(I)
                lDates(lNewIndex) = Dates(I)
            End If
        Next 'loop 220

        ReDim Flow(lnewLength)
        ReDim QLog(lnewLength)
        ReDim Dates(lnewLength)

        For I As Integer = 1 To lnewLength
            Flow(lNewIndex) = lFlow(I)
            QLog(lNewIndex) = lQLog(I)
            Dates(lNewIndex) = lDates(I)
        Next

        ReDim lFlow(0)
        ReDim lQLog(0)
        ReDim lDates(0)
    End Sub

    Public Sub ReadData()
        GetData()
    End Sub

    Public Sub Clear()
        ReDim Flow(0)
        ReDim QLog(0)
        ReDim Dates(0)
        pBackTraceFlag.Clear()
    End Sub

    Public Function CompareTo(ByVal obj As Object) As Integer Implements System.IComparable.CompareTo
        If TypeOf obj Is clsRecessionSegment Then
            Dim aSeg As clsRecessionSegment = CType(obj, clsRecessionSegment)
            Return MeanLogQ.CompareTo(aSeg.MeanLogQ)
        End If
        Throw New ArgumentException("object is not a clsRecessionSegment")
    End Function
End Class
