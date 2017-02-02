Imports System.IO
Imports atcData
Imports atcUtility
Imports MapWinUtility

Public Enum WTFAnalysis
    FindRecession
    FindRecharge
End Enum
Public Class clsFall
    Inherits clsRecess

    'Private pFileFallDat As String = "falldat.txt"
    Private pFileOutSum As String = "fallsum.txt"

    'In this class, missing data is allowed, and simply marked as -999 in the original FORTRAN code
    Public GraphTsGroup As atcTimeseriesGroup

    Public ApplyDatum As Boolean = False
    Private pDatum As Double = 0 'usually 0 for depth to water and arbitrary for water level altitude as long as it is less than the minimum water level
    Public Property Datum() As Double
        Get
            Return pDatum
        End Get
        Set(ByVal value As Double)
            pDatum = value
        End Set
    End Property

    Private pDataType As Integer '1, depth to water table; 2, water level altitude
    Public Property DataType() As Integer
        Get
            Return pDataType
        End Get
        Set(ByVal value As Integer)
            pDataType = value
        End Set
    End Property

    Public MinimumRise As Double
    Public Phase As WTFAnalysis = WTFAnalysis.FindRecession
    Public PeakAheadDays As Integer = 14
    Public PeakRisePct As Double = 0.75

    Private pFallD As Double = Double.NaN
    Public Property FallD(Optional ByVal aRefreshData As Boolean = False) As Double
        Get
            Return pFallD
        End Get
        Set(ByVal value As Double)
            'could enforce some rules here
            If FlowData Is Nothing Then
                Exit Property
            End If
            Dim lMin As Double = FlowData.Attributes.GetValue("Min")
            If value > lMin Then
                Throw New ApplicationException("The value of d specified is greater than the minimum groundwater level (" & lMin & ")" & vbCrLf & _
                           "measured at the site. Re-enter another value of d.")
                Exit Property
            End If
            pFallD = value
            If aRefreshData Then
                For Each lRecessSeg As clsRecessionSegment In listOfSegments
                    lRecessSeg.GetData(True, pFallD)
                Next
            End If
        End Set
    End Property

    ''' <summary>
    ''' Find either a falling (true) or rising (false) limb
    ''' </summary>
    ''' <param name="aFall">True (default): find falling limb; False: find rising limb</param>
    ''' <remarks></remarks>
    Public Overrides Sub RecessGetAllSegments() 'Optional ByVal aFall As Boolean = True
        'If DataType = 1 AndAlso ApplyDatum Then
        '    For I As Integer = 1 To FlowData.numValues
        '        'If FlowData.Value(I) > 0 Then FlowData.Value(I) *= -1
        '        If Not Double.IsNaN(FlowData.Value(I)) Then
        '            FlowData.Value(I) = Datum - FlowData.Value(I)
        '        End If
        '    Next
        'End If

        Dim lDate(5) As Integer

        ' ------------- LOCATE a PEAK ---------------------
        Dim lOK As Integer = 0
        'CountRecession  
        Dim lNum As Integer
        Dim II As Integer = 1
        Dim liCount As Integer
        Dim lKey As String
        Dim liHowFar As Integer
        For liCount = 2 To FlowData.numValues 'original loop 200
            lOK = 0
            J2Date(FlowData.Dates.Value(liCount - 1), lDate)
            If Not RecessIncludeMonths.Contains(lDate(1)) Then
                Continue For 'loop 200
            End If

            If liCount >= FlowData.numValues Then Continue For
            Dim lCurrentValue As Double = FlowData.Value(liCount)
            If Double.IsNaN(lCurrentValue) Then Continue For

            If Phase = WTFAnalysis.FindRecession Then 'aFall = True, Find fall limb
                'need to find a local peak as a starting point of a falling limb
                If liCount < FlowData.numValues AndAlso (lCurrentValue <= FlowData.Value(liCount - 1) Or lCurrentValue <= FlowData.Value(liCount + 1)) Then
                    Continue For 'loop 200
                Else
                    IndexPeakDay = liCount
                    lOK = 1
                End If
            Else 'Find rise limb
                'need to find a local min as a starting point of a rising limb
                If liCount < FlowData.numValues AndAlso (lCurrentValue >= FlowData.Value(liCount - 1) Or lCurrentValue >= FlowData.Value(liCount + 1)) Then
                    Continue For 'loop 200
                Else
                    IndexPeakDay = liCount
                    lOK = 1
                End If
            End If

            '-------------- ANALYZE THE RECESSION AFTER THE PEAK: -----------------
            liHowFar = 0
            While True 'loop 210
                liCount += 1
                If liCount > FlowData.numValues Then Exit For 'loop 200

                If Phase = WTFAnalysis.FindRecession Then 'aFall = True, Find fall limb
                    'if no longer falling, then reaches the end of a falling limb
                    If FlowData.Value(liCount) > FlowData.Value(liCount - 1) Then lOK = 0
                Else 'Find rise limb
                    'if no longer rising, then reaches the end of a rising limb
                    If FlowData.Value(liCount) < FlowData.Value(liCount - 1) Then lOK = 0
                End If
                'c............. next line, different from recess.............
                If Double.IsNaN(FlowData.Value(liCount)) Then lOK = 0

                liHowFar = liCount - IndexPeakDay - 1
                If lOK = 1 Then
                    Continue While 'loop 210
                Else
                    Exit While 'loop 210
                End If
            End While 'loop 210
            If liHowFar < RecessMinLengthInDays And lOK = 0 Then
                liCount -= 1
                Continue For 'loop 200
            End If
            If liHowFar >= RecessMinLengthInDays And lOK = 0 Then 'This is a long if branch
                RecessionSegment = New clsRecessionSegment()

                lNum = liCount - IndexPeakDay - 1
                If lNum > clsRecessionSegment.MaxSegmentLengthInDaysGW Then
                    lNum = clsRecessionSegment.MaxSegmentLengthInDaysGW
                    liCount = IndexPeakDay + clsRecessionSegment.MaxSegmentLengthInDaysGW
                    'write (*,*) 'Recession period more than 300 days.'
                    'write (*,*) 'Removed data past 300.'
                End If
                'Dim liMin As Integer = 1
                'Dim liMax As Integer = lNum
                'For I As Integer = 1 To lNum 'loop 220
                '    lFlow(I) = FlowData.Value(I + IndexPeakDay)
                '    If lFlow(I) = 0.0 Then
                '        lQLog(I) = -88.8
                '    Else
                '        lQLog(I) = Math.Log10(lFlow(I))
                '    End If
                '    lDates(I) = FlowData.Dates.Value(I + IndexPeakDay - 1)
                'Next 'loop 220
                With RecessionSegment
                    .PeakDayIndex = IndexPeakDay
                    .PeakDayDate = FlowData.Dates.Value(IndexPeakDay - 1)
                    .SegmentLength = lNum
                    .MinDayOrdinal = 1
                    .MaxDayOrdinal = lNum
                    .IsExcluded = True
                    '.GetData() get data later to save memory

                    If Phase = WTFAnalysis.FindRecharge Then 'Not aFall
                        Dim lH0Index As Integer = IndexPeakDay - 1
                        While FlowData.Value(lH0Index) <= FlowData.Value(IndexPeakDay)
                            If lH0Index <= 1 Then
                                Exit While
                            End If
                            lH0Index -= 1
                        End While
                        .HzeroDayIndex = lH0Index
                        .HzeroDayDate = FlowData.Dates.Value(lH0Index - 1)
                        .HzeroDayValue = FlowData.Value(lH0Index)
                    End If
                End With
                CountRecession += 1
                J2Date(FlowData.Dates.Value(IndexPeakDay - 1), lDate)
                lKey = lDate(0).ToString & "/" & lDate(1).ToString.PadLeft(2, " ") & "/" & lDate(2).ToString.PadLeft(2, " ")
                listOfSegments.Add(lKey, RecessionSegment)
            End If 'end of long if branch
        Next 'original loop 200
    End Sub

    Public Sub RiseGetAllSegments() 'Optional ByVal aFall As Boolean = True
        Dim lDate(5) As Integer

        ' ------------- LOCATE a PEAK ---------------------
        Dim lOK As Integer = 0
        'CountRecession  
        Dim lNum As Integer
        Dim II As Integer = 1
        Dim liCount As Integer
        Dim lKey As String
        Dim liHowFar As Integer

        'Find some guiding gap values
        Dim lTsGaps As atcTimeseries = FlowData.Clone()
        Dim lGap As Double = 0
        Dim lVal1 As Double
        Dim lVal2 As Double
        Dim lGapTotal As Double = 0.0
        Dim lGapCount As Integer = 0
        Dim lGapMinAboveZero As Double = Double.MaxValue
        Dim lGapMax As Double = Double.MinValue
        For I As Integer = 2 To lTsGaps.numValues
            lVal2 = FlowData.Value(I)
            lVal1 = FlowData.Value(I - 1)
            If Double.IsNaN(lVal1) OrElse Double.IsNaN(lVal2) Then
                lGap = Double.NaN
            Else
                lGap = lVal2 - lVal1
                lGapTotal += Math.Abs(lGap)
                lGapCount += 1
            End If
            lTsGaps.Value(I) = lGap
            If Not Double.IsNaN(lGap) Then
                Dim lGapAbs As Double = Math.Abs(lGap)
                If lGapMinAboveZero > lGapAbs AndAlso lGapAbs > 0 Then
                    'Get the smallest gap above zero
                    lGapMinAboveZero = lGapAbs
                End If
                If lGapMax < lGapAbs Then
                    lGapMax = lGapAbs
                End If
            End If
        Next
        lTsGaps.Value(1) = Double.NaN
        Dim lGapMean As Double = lGapTotal / lGapCount

        Dim lGapsRunning As New atcCollection()
        For liCount = 2 To FlowData.numValues 'original loop 200
            lOK = 0
            J2Date(FlowData.Dates.Value(liCount - 1), lDate)
            If Not RecessIncludeMonths.Contains(lDate(1)) Then
                Continue For 'loop 200
            End If

            If liCount >= FlowData.numValues Then Continue For
            Dim lCurrentValue As Double = FlowData.Value(liCount)
            If Double.IsNaN(lCurrentValue) Then Continue For

            If Phase = WTFAnalysis.FindRecession Then 'aFall = True, Find fall limb
                'need to find a local peak as a starting point of a falling limb
                If liCount < FlowData.numValues AndAlso (lCurrentValue <= FlowData.Value(liCount - 1) Or lCurrentValue <= FlowData.Value(liCount + 1)) Then
                    Continue For 'loop 200
                Else
                    IndexPeakDay = liCount
                    lOK = 1
                End If
            Else 'Find rise limb
                'need to find a local min as a starting point of a rising limb
                If liCount < FlowData.numValues AndAlso (lCurrentValue >= FlowData.Value(liCount - 1) Or lCurrentValue >= FlowData.Value(liCount + 1)) Then
                    Continue For 'loop 200
                Else
                    lGapsRunning.Clear()
                    If Not Double.IsNaN(lTsGaps.Value(liCount)) Then
                        lGapsRunning.Add(lTsGaps.Value(liCount))
                    End If
                    Dim lTotalCount As Double = 0
                    Dim lRiseCount As Double = 0
                    For I As Integer = 1 To 4
                        If liCount + I > FlowData.numValues Then
                            Exit For
                        End If
                        Dim lValue As Double = lTsGaps.Value(liCount + I)
                        If Not Double.IsNaN(lValue) Then
                            lTotalCount += 1
                            If lValue > 0 Then
                                lRiseCount += 1
                            End If
                        End If
                    Next
                    Dim lRisePct As Double = lRiseCount / lTotalCount
                    'has to continue on to rise 50% of times in the short future
                    If lRisePct < 0.5 Then
                        Continue For 'loop 200
                    End If
                    IndexPeakDay = liCount
                    lOK = 1
                End If
            End If

            '-------------- ANALYZE THE RECESSION AFTER THE PEAK: -----------------
            liHowFar = 0
            While True 'loop 210
                liCount += 1
                If liCount > FlowData.numValues Then Exit For 'loop 200

                If Phase = WTFAnalysis.FindRecession Then 'aFall = True, Find fall limb
                    'if no longer falling, then reaches the end of a falling limb
                    If FlowData.Value(liCount) > FlowData.Value(liCount - 1) Then lOK = 0
                Else 'Find rise limb
                    'if no longer rising, then reaches the end of a rising limb
                    If FlowData.Value(liCount) < FlowData.Value(liCount - 1) Then
                        'lOK = 0
                        Dim lGapNowIsSignificant As Boolean = True
                        'Dim lGapNow As Double = FlowData.Value(liCount - 1) - FlowData.Value(liCount)
                        'If lGapNow < lGapMean Then
                        '    lGapNowIsSignificant = False
                        'End If
                        'Dim lLargeGapCount As Integer = 0
                        'For Each lGapVal As Double In lGapsRunning
                        '    If lGapNow < lGapVal Then
                        '        lLargeGapCount += 1
                        '    End If
                        'Next
                        'If lLargeGapCount / lGapsRunning.Count * 1.0 > 2 / 3 Then
                        '    lGapNowIsSignificant = False
                        'End If

                        'Scout ahead a window of 30
                        Dim I As Integer
                        Dim lRiseCount As Double = 0
                        Dim lTotalCount As Double = 0
                        Dim lFlatCount As Double = 0
                        Dim lFirstDip As Integer = -99
                        Dim lwindowSize As Integer = PeakAheadDays
                        For I = 1 To lwindowSize
                            'If liCount + I <= FlowData.numValues AndAlso FlowData.Value(liCount + I) > FlowData.Value(liCount) Then
                            '    lGapNowIsSignificant = False
                            '    Exit For
                            'End If
                            If liCount + I > FlowData.numValues Then
                                Exit For
                            End If
                            Dim lGapVal As Double = lTsGaps.Value(liCount + I)
                            If Not Double.IsNaN(lGapVal) Then
                                lTotalCount += 1
                                If lGapVal > 0 Then
                                    lRiseCount += 1
                                Else
                                    If lFirstDip < 0 Then
                                        lFirstDip = I
                                    End If
                                End If
                                If Math.Abs(lGapVal) - lGapMinAboveZero > 0.001 Then

                                Else
                                    lFlatCount += 1
                                End If
                            End If
                        Next
                        For I = 1 To Int(lFlatCount)
                            If liCount + lwindowSize + I > FlowData.numValues Then
                                Exit For
                            End If
                            Dim lGapVal As Double = lTsGaps.Value(liCount + lwindowSize + I)
                            If Not Double.IsNaN(lGapVal) Then
                                lTotalCount += 1
                                If lGapVal > 0 Then
                                    lRiseCount += 1
                                Else
                                    If lFirstDip < 0 Then
                                        lFirstDip = lwindowSize + I
                                    End If
                                End If
                                If Math.Abs(lGapVal) - lGapMinAboveZero > 0.001 Then

                                End If
                            End If
                        Next

                        Dim lRisePct As Double = lRiseCount / lTotalCount
                        If lRisePct > PeakRisePct Then
                            lGapNowIsSignificant = False
                        End If

                        If lGapNowIsSignificant Then
                            lOK = 0
                        Else
                            If lFirstDip > 0 Then
                                liCount += lFirstDip 'I
                            End If
                            lOK = 1
                        End If
                    End If
                End If
                'c............. next line, different from recess.............
                If Double.IsNaN(FlowData.Value(liCount)) Then lOK = 0

                liHowFar = liCount - IndexPeakDay - 1
                If lOK = 1 Then
                    If Not Double.IsNaN(lTsGaps.Value(liCount)) Then
                        lGapsRunning.Add(lTsGaps.Value(liCount))
                    End If
                    Continue While 'loop 210
                Else
                    Exit While 'loop 210
                End If
            End While 'loop 210
            'abondon the current rise segment and start over at this local minimum
            'If liHowFar < RecessMinLengthInDays And lOK = 0 Then
            '    liCount -= 1
            '    Continue For 'loop 200
            'End If
            If liHowFar >= RecessMinLengthInDays And lOK = 0 Then 'This is a long if branch
                RecessionSegment = New clsRecessionSegment()

                lNum = liCount - IndexPeakDay - 1
                If lNum > clsRecessionSegment.MaxSegmentLengthInDaysGW Then
                    lNum = clsRecessionSegment.MaxSegmentLengthInDaysGW
                    liCount = IndexPeakDay + clsRecessionSegment.MaxSegmentLengthInDaysGW
                    'write (*,*) 'Recession period more than 300 days.'
                    'write (*,*) 'Removed data past 300.'
                End If
                'Dim liMin As Integer = 1
                'Dim liMax As Integer = lNum
                'For I As Integer = 1 To lNum 'loop 220
                '    lFlow(I) = FlowData.Value(I + IndexPeakDay)
                '    If lFlow(I) = 0.0 Then
                '        lQLog(I) = -88.8
                '    Else
                '        lQLog(I) = Math.Log10(lFlow(I))
                '    End If
                '    lDates(I) = FlowData.Dates.Value(I + IndexPeakDay - 1)
                'Next 'loop 220
                With RecessionSegment
                    .PeakDayIndex = IndexPeakDay
                    .PeakDayDate = FlowData.Dates.Value(IndexPeakDay - 1)
                    .SegmentLength = lNum
                    .MinDayOrdinal = 1
                    .MaxDayOrdinal = lNum
                    .IsExcluded = True
                    '.GetData() get data later to save memory

                    If Phase = WTFAnalysis.FindRecharge Then 'Not aFall
                        'might not need to do this here as user might change 
                        'the begining of a rising limb
                        Dim lH0Index As Integer = IndexPeakDay
                        Dim lGapVal As Double = lTsGaps.Value(IndexPeakDay)
                        While True
                            If lH0Index <= 1 Then
                                Exit While
                            End If
                            If Not Double.IsNaN(lGapVal) AndAlso lGapVal > 0 Then
                                lH0Index -= 1
                                lGapVal = lTsGaps.Value(lH0Index)
                            Else
                                lH0Index += 1
                                Exit While
                            End If
                        End While
                        If lH0Index > IndexPeakDay Then
                            lH0Index = IndexPeakDay
                        End If
                        .HzeroDayIndex = lH0Index
                        .HzeroDayDate = FlowData.Dates.Value(lH0Index - 1)
                        .HzeroDayValue = FlowData.Value(lH0Index)
                    End If
                End With
                CountRecession += 1
                J2Date(FlowData.Dates.Value(IndexPeakDay - 1), lDate)
                lKey = lDate(0).ToString & "/" & lDate(1).ToString.PadLeft(2, " ") & "/" & lDate(2).ToString.PadLeft(2, " ")
                listOfSegments.Add(lKey, RecessionSegment)
            Else
                liCount -= 1
            End If 'end of long if branch
        Next 'original loop 200
    End Sub

    Public Overrides Sub SetOutputFiles()
        'pFileFallDat = IO.Path.Combine(OutputPath, pFileFallDat)
        'pFileRecSum = IO.Path.Combine(OutputPath, pFileRecSum)
        'pFileRecData = IO.Path.Combine(OutputPath, pFileRecData)
        'pFileIndex = IO.Path.Combine(OutputPath, pFileIndex)

        pFileOutSum = IO.Path.Combine(OutputPath, "fallsum.txt")
        pFileOut1 = IO.Path.Combine(OutputPath, "fallout1.txt")
        pFileOut2 = IO.Path.Combine(OutputPath, "fallout2extended.txt")

    End Sub

    Public Overrides Function DoOperation(ByVal aOperation As String, ByVal aRecessKey As String) As Boolean
        If aRecessKey <> "" Then
            RecessionSegment = listOfSegments.ItemByKey(aRecessKey)
            If RecessionSegment.NeedtoReadData Then
                If Not Double.IsNaN(pFallD) Then
                    RecessionSegment.GetData(True, pFallD)
                Else
                    RecessionSegment.GetData() 'should not allow to proceed!
                End If
            End If
        End If
        Select Case aOperation.ToLower
            Case "d"
                FallDisplay(RecessionSegment)
            Case "r"
                'If RecessionSegment.NeedToAnalyse Then
                'End If
                MyBase.RecessAnalyse(RecessionSegment, "GW LEVEL")
            Case "select"
                RecessionSegment.IsExcluded = False
            Case "unselect"
                RecessionSegment.IsExcluded = True
            Case "summary"
                'Dim lOutDir As String = IO.Path.GetDirectoryName(pFileOut1)
                'pFileOut1 = IO.Path.Combine(lOutDir, IO.Path.GetFileNameWithoutExtension(pFileOut1) & "_summary" & IO.Path.GetExtension(pFileOut1))
                'pFileOut2 = IO.Path.Combine(lOutDir, IO.Path.GetFileNameWithoutExtension(pFileOut2) & "_summary" & IO.Path.GetExtension(pFileOut2))
                MyBase.RecessSummary()
                'pFileOut1 = pFileOut1.Replace("_summary", "")
                'pFileOut2 = pFileOut2.Replace("_summary", "")
                FallSummary()
            Case "q"

        End Select
    End Function

    Private Sub FallDisplay(ByVal aSegment As clsRecessionSegment)
        'This branch only for display tabular recession data so far
        'doesn't involve any real calculation, needs to be factored out
        Dim lThisTable As String = ""
        Dim lMsg As New Text.StringBuilder
        lMsg.Length = 0
        With aSegment
            'can save the table entries into some file
            lMsg.AppendLine(TableFall(.Flow, .Dates, .PeakDayIndex, .MinDayOrdinal, .MaxDayOrdinal, .MinDayOrdinal, .MaxDayOrdinal))
        End With

        Table = lMsg.ToString
        Dim lSDate As Double = aSegment.Dates(aSegment.MinDayOrdinal)
        Dim lDate(5) As Integer
        J2Date(aSegment.Dates(aSegment.MaxDayOrdinal), lDate)
        Dim lEDate As Double = Date2J(lDate(0), lDate(1), lDate(2), 24, 0, 0)

        GraphTs = SubsetByDate(FlowData, lSDate, lEDate, Nothing)
        With GraphTs
            '.numValues = aSegment.SegmentLength - 1
            '.Dates.Values = aSegment.Dates

            Select Case Phase
                Case WTFAnalysis.FindRecession
                    Dim lSubsetQLog(aSegment.MaxDayOrdinal - aSegment.MinDayOrdinal + 1) As Double
                    For I As Integer = 1 To aSegment.QLog.Length - 1
                        If I >= aSegment.MinDayOrdinal AndAlso I <= aSegment.MaxDayOrdinal Then
                            lSubsetQLog(I - aSegment.MinDayOrdinal + 1) = aSegment.QLog(I)
                        End If
                    Next
                    .Values = lSubsetQLog
                Case WTFAnalysis.FindRecharge
                    Dim lSubsetGWL(aSegment.MaxDayOrdinal - aSegment.MinDayOrdinal + 1) As Double
                    For I As Integer = 1 To aSegment.Flow.Length - 1
                        If I >= aSegment.MinDayOrdinal AndAlso I <= aSegment.MaxDayOrdinal Then
                            lSubsetGWL(I - aSegment.MinDayOrdinal + 1) = aSegment.Flow(I)
                        End If
                    Next
                    .Values = lSubsetGWL
                Case Else
                    Exit Sub
            End Select

            .Value(0) = GetNaN()
            '.Dates.Value(0) = aSegment.PeakDayDate
            .Dates.Value(0) = lSDate - JulianHour * 24.0
            .Attributes.SetValue("YAxis", "LEFT")
            .Attributes.SetValue("point", True)
            .Attributes.SetValue("Constituent", "")
            .Attributes.SetValue("Scenario", "")

            Select Case Phase
                Case WTFAnalysis.FindRecession
                    If DataType = 1 Then
                        .Attributes.SetValue("Units", "ln(GWL,ft)") 'This is converted however by ground_Elev - depth_to_watertable data
                    ElseIf DataType = 2 Then
                        .Attributes.SetValue("Units", "ln(GWL,ft)")
                    End If
                    If Not Double.IsNaN(pFallD) Then
                        .Attributes.SetValue("Units", "ln(GWL,(Ht - d))")
                    End If
                Case WTFAnalysis.FindRecharge
                    .Attributes.SetValue("Units", "GWL, ft")
            End Select
        End With
    End Sub

    Private Sub ConstructGraphTsGroup()
        If listOfSegments Is Nothing OrElse listOfSegments.Count = 0 Then Exit Sub
        If GraphTsGroup Is Nothing Then GraphTsGroup = New atcTimeseriesGroup
        For Each lGraphTs As atcTimeseries In GraphTsGroup
            lGraphTs.Clear()
        Next
        GraphTsGroup.Clear()

        'Dim lSeg As clsRecessionSegment = Nothing
        'For Each lSeg In listOfSegments
        '    With lSeg
        '        Dim lNewGraphTs As New atcTimeseries(Nothing)
        '        Dim lSubsetGWL(.MaxDayOrdinal - .MinDayOrdinal + 1) As Double
        '        For I As Integer = 1 To .Flow.Length - 1
        '            If I >= .MinDayOrdinal AndAlso I <= .MaxDayOrdinal Then
        '                lSubsetGWL(I - .MinDayOrdinal + 1) = .Flow(I)
        '            End If
        '        Next
        '        .Values = lSubsetGWL
        '        .Value(0) = GetNaN()
        '        '.Dates.Value(0) = aSegment.PeakDayDate
        '        .Dates.Value(0) = lSDate - JulianHour * 24.0
        '        .Attributes.SetValue("YAxis", "LEFT")
        '        .Attributes.SetValue("point", True)
        '        .Attributes.SetValue("Constituent", "")
        '        .Attributes.SetValue("Scenario", "")
        '        If DataType = 1 Then
        '            .Attributes.SetValue("Units", "GWL Depth")
        '        ElseIf DataType = 2 Then
        '            .Attributes.SetValue("Units", "GWL Altitude")
        '        End If
        '    End With
        'Next

    End Sub
    '--------- THIS SUBROUTINE MAKES TABULAR OUTPUT OF RECESSION DATA: -----
    Public Shared Function TableFall(ByVal aFlow() As Double, _
                                       ByVal aDates() As Double, _
                                       ByVal aiPeak As Integer, _
                                       ByVal aiMin As Integer, _
                                       ByVal aiMax As Integer, _
                                       ByVal aPickStartingDay As Integer, _
                                       ByVal aiDisplayLastDay As Integer) As String

        'Logger.Dbg("ENTER STARTING DAY (1, 11, OR 21)") 'originally dynamically read in
        Dim liStart As Integer = aPickStartingDay
        'Dim liEnd As Integer = liStart + aiDisplayLastDay - 1
        Dim liEnd As Integer = aiDisplayLastDay
        If liStart > 150 Then liStart = 150
        If liEnd > 150 Then liEnd = 150

        'Dim lDelQLog(60) As Double
        'For I As Integer = liStart To liEnd 'loop 20
        '    If I = 1 Then
        '        lDelQLog(I) = 999.0
        '    Else
        '        lDelQLog(I) = aQLog(I) - aQLog(I - 1)
        '    End If
        'Next 'loop 20

        Dim lDate(5) As Integer
        Dim lStr As New System.Text.StringBuilder
        lStr.AppendLine("TIME AFTER                           TIME AFTER")
        lStr.AppendLine("   PEAK                      GWL       START   YEAR  .  MONTH    DAY")
        For I As Integer = liStart To liEnd 'loop 230
            If I > aiMax Or I < aiMin Then
                lStr.AppendLine("---")
            Else
                '   20 FORMAT (1I6, 1F30.4, 1I8, 3I8)
                Dim liCountStr As String = I.ToString.PadLeft(6, " ")
                'Dim lQLogStr As String = String.Format("{0:0.0000}", aQLog(I)).ToString.PadLeft(10, " ")
                'Dim lDelQLogStr As String = String.Format("{0:0.0000}", lDelQLog(I)).ToString.PadLeft(10, " ")
                ''Dim lFlowStr As String = String.Format("{0:0.0000}", aFlow(I)).ToString.PadLeft(10, " ")
                Dim lFlowStr As String = String.Format("{0:0.0000}", aFlow(I)).ToString.PadLeft(30, " ")
                Dim liCountPeakStr As String = (I + aiPeak).ToString.PadLeft(8, " ")
                J2Date(aDates(I), lDate)
                Dim lYearStr As String = lDate(0).ToString.PadLeft(8, " ")
                Dim lMonStr As String = lDate(1).ToString.PadLeft(8, " ")
                Dim lDayStr As String = lDate(2).ToString.PadLeft(8, " ")

                Dim lDateStr As String = lYearStr & lMonStr & lDayStr
                'lStr.AppendLine(liCountStr & lQLogStr & lDelQLogStr & lFlowStr & liCountPeakStr & lDateStr)
                lStr.AppendLine(liCountStr & lFlowStr & liCountPeakStr & lDateStr)
            End If
        Next 'loop 230
        Return lStr.ToString
    End Function

    Public Sub FallSummary()

        If SaveInterimResults Then
            SetOutputFiles()
        End If

        Dim lSW As IO.StreamWriter = Nothing
        Dim lMsg As Text.StringBuilder = Nothing
        Dim lSeg As clsRecessionSegment = Nothing
        Dim lDiff As Double = 0
        Dim lYGraph As Double
        Dim lOrdinal As Integer

        If SaveInterimResults AndAlso fHasWritePermission Then
            ''Write the fallout1.txt
            'lSW = New StreamWriter(pFileOut1, False)
            'lSW.WriteLine("   TIME       GWL   ")
            'lSW.WriteLine("")
            'For Each lSeg In listOfSegments
            '    With lSeg
            '        If .NeedtoReadData Then
            '            .GetData()
            '        End If
            '        'DoOperation("r", lSeg.PeakDayDateToString)
            '        For I As Integer = .MinDayOrdinal To .MaxDayOrdinal
            '            lOrdinal = I - .MinDayOrdinal
            '            lYGraph = .Flow(I)
            '            If lYGraph < -80.0 Then lYGraph = -2
            '            lSW.WriteLine(lOrdinal.ToString.PadLeft(7, " ") & String.Format("{0:0.000}", lYGraph).PadLeft(10, " "))
            '        Next
            '        lSW.WriteLine("")
            '    End With
            'Next
            'lSW.Flush()
            'lSW.Close()
            'lSW = Nothing

            'Write the fallout2.txt
            Dim lDate(5) As Integer
            'lSW = New StreamWriter(pFileOut2, False)
            lSW = New StreamWriter(pFileOutSum, False)
            lSW.WriteLine("  FILE  """ & IO.Path.GetFileName(pFileOutSum) & """ --  OUTPUT OF FALL program")
            lSW.WriteLine("  INPUT FILE = " & FlowData.Attributes.GetValue("History 1"))
            J2Date(FlowData.Dates.Value(0), lDate)
            lSW.WriteLine("  START =  " & lDate(0))
            J2Date(FlowData.Dates.Value(FlowData.numValues), lDate)
            lSW.WriteLine("  END =    " & lDate(0))
            lSW.WriteLine("  DAYS OF RECESSION REQUIRED FOR DETECTION= " & RecessMinLengthInDays)
            lSW.WriteLine("  MONTHS SELECTED:")
            For Each lMonth As Integer In RecessIncludeMonths
                lSW.WriteLine(Space(4) & lMonth)
            Next
            If DataType = 1 Then
                lSW.WriteLine(" WATER-LEVEL DATA FROM FILE IS ""DEPTH TO. (negative values)"" ")
            ElseIf DataType = 2 Then
                lSW.WriteLine(" WATER-LEVEL DATA FROM FILE IS ""Water Level Altitude""")
            End If

            lSW.WriteLine(" GWL GIVEN BELOW IS THE WATER LEVEL ABOVE ")
            lSW.WriteLine(" A VERTICAL REFERENCE.  THE DEPTH TO THE  ")
            lSW.WriteLine(" REFERENCE IS " & Datum)
            lSW.WriteLine(" ----------------------------------------------------------------------")
            lSW.WriteLine("  Seq # is the sequence number in which the segment ")
            lSW.WriteLine("  was selected")
            lSW.WriteLine("  Begin date is the first day of recession")
            lSW.WriteLine("  End date is the last day of recession")
            lSW.WriteLine("  Peak is the ground-water level of the peak")
            lSW.WriteLine("  TIME is the number of days since the last peak  ")
            lSW.WriteLine("  GWL = Ground-water level ")
            lSW.WriteLine(" -----------------------------------------------------------------------")
            lSW.WriteLine("")
            Dim lSeqNumber As Integer = 1
            For Each lSeg In listOfSegments
                With lSeg
                    'If .NeedtoReadData Then
                    '    .GetData()
                    'End If
                    'DoOperation("r", lSeg.PeakDayDateToString)
                    lSW.WriteLine("Seq # =  " & lSeqNumber)
                    J2Date(.Dates(1), lDate)
                    lSW.WriteLine("Begin date = " & lDate(0) & lDate(1).ToString.PadLeft(2, " ") & lDate(2).ToString.PadLeft(2, " "))
                    J2Date(.Dates(UBound(.Dates)), lDate)
                    lSW.WriteLine("  End date = " & lDate(0) & lDate(1).ToString.PadLeft(2, " ") & lDate(2).ToString.PadLeft(2, " "))
                    lSW.WriteLine("   TIME       GWL")
                    For I As Integer = .MinDayOrdinal To .MaxDayOrdinal
                        lOrdinal = I - .MinDayOrdinal
                        lYGraph = .Flow(I)
                        If lYGraph < -80.0 Then lYGraph = -2
                        lSW.WriteLine(lOrdinal.ToString.PadLeft(7, " ") & String.Format("{0:0.000}", lYGraph).PadLeft(10, " "))
                    Next
                    lSW.WriteLine("")
                End With
                lSeqNumber += 1
            Next
            lSW.Flush()
            lSW.Close()
            lSW = Nothing
        End If
    End Sub
End Class
