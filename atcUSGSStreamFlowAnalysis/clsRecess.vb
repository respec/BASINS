Imports System.IO
Imports atcData
Imports atcUtility
Imports MapWinUtility

Public Class clsRecess

    'Assuming when pass in, the pData already has 
    'the desired starting and ending dates!!!
    Private pData As atcTimeseries
    Public Property FlowData() As atcTimeseries
        Get
            Return pData
        End Get
        Set(ByVal value As atcTimeseries)
            pData = value
        End Set
    End Property

    Private pRecessLengthInDays As Integer = 0
    Public Property RecessLengthInDays() As Integer
        Get
            Return pRecessLengthInDays
        End Get
        Set(ByVal value As Integer)
            If value <= 0 Then
                value = 5
            End If
            pRecessLengthInDays = value
        End Set
    End Property

    'must be integers, can only be within 1 - 12
    Private pRecessMonths As New ArrayList()
    Public Property RecessIncludeMonths() As ArrayList
        Get
            Return pRecessMonths
        End Get
        Set(ByVal value As ArrayList)
            If value IsNot Nothing Then
                pRecessMonths = value
            End If
            pRecessMonths.Sort()
        End Set
    End Property

    Private pRecessions As New atcCollection 'collection of clsRecessionSegment
    Public RecessionSegment As clsRecessionSegment = Nothing

#Region "IOManagement"
    Private pHasWritePermission As Boolean = False
    Private pOutputPath As String
    Public Property OutputPath() As String
        Get
            Return pOutputPath
        End Get
        Set(ByVal value As String)
            If IO.Directory.Exists(value) Then
                Dim lSW As StreamWriter = Nothing
                Try
                    lSW = New StreamWriter(IO.Path.Combine(value, "z.txt"))
                    lSW.WriteLine("1")
                    lSW.Close()
                    lSW = Nothing
                    pHasWritePermission = True
                Catch ex As Exception
                    If lSW IsNot Nothing Then
                        lSW.Close()
                        lSW = Nothing
                    End If
                    pHasWritePermission = False
                End Try

            End If
        End Set
    End Property

    Private pDataFilename As String = ""

    Dim pHeaderOutFile1 As String = ""
    Dim pHeaderOutFile2 As String = ""
#End Region

#Region "Size Limits"
    Private pMaxNumDaysInAllRecPeriods As Integer = 3000
    Public Property MaxNumDaysInAllRecPeriods() As Integer
        Get
            Return pMaxNumDaysInAllRecPeriods
        End Get
        Set(ByVal value As Integer)
            pMaxNumDaysInAllRecPeriods = value
        End Set
    End Property

    Private pMaxNumRecPeriods As Integer = 50
    Public Property MaxNumRecPeriods() As Integer
        Get
            Return pMaxNumRecPeriods
        End Get
        Set(ByVal value As Integer)
            pMaxNumRecPeriods = value
        End Set
    End Property
#End Region

#Region "Display Parameters"
    Private piDisplayLastDay As Integer = 20
    Public Property DisplayLastDay() As Integer
        Get
            Return piDisplayLastDay
        End Get
        Set(ByVal value As Integer)
            piDisplayLastDay = value
        End Set
    End Property

    Private pXLogQMin As Double
    Public Property XLogQMin() As Double
        Get
            Return pXLogQMin
        End Get
        Set(ByVal value As Double)
            pXLogQMin = value
        End Set
    End Property

    Private pXLogQMax As Double
    Public Property XLogQMax() As Double
        Get
            Return pXLogQMax
        End Get
        Set(ByVal value As Double)
            pXLogQMax = value
        End Set
    End Property

    Private pSeasonLabel As String = "n"
    Public Property SeasonLabel() As String
        Get
            Return pSeasonLabel
        End Get
        Set(ByVal value As String)
            pSeasonLabel = value
        End Set
    End Property

    'The following two are for Recess original Table and ascii Graph
    'can be changed during run time
    Private pPickStartingDayOrdinal As Integer = 1 'the only valid choices are 1, 11, 21
    Public Property DisplayStartDayOrdinal() As Integer
        Get
            Return pPickStartingDayOrdinal
        End Get
        Set(ByVal value As Integer)
            If value <> 1 And value <> 11 And value <> 21 Then
                value = 1
            End If
            pPickStartingDayOrdinal = value
        End Set
    End Property
    Private pPickInterval As Integer = 1 'the only valid choices: ENTER TIME INTERVAL (1=EVERY DAY, 2=EVERY OTHER DAY)'
    Public Property DisplayInterval() As Integer
        Get
            Return pPickInterval
        End Get
        Set(ByVal value As Integer)
            If value <> 1 And value <> 2 Then
                value = 1
            End If
            pPickInterval = value
        End Set
    End Property

    Public Tables As atcCollection = Nothing
    Public Graphs As atcCollection = Nothing

    Public Table As String 'current recession segment's table
    Public GraphTs As atcTimeseries 'current recession's timeseries
    'Public AskUserFirstDayofSegment As Integer 'ENTER THE FIRST DAY OF THE SEGMENT *********** enter a NUMBER ******'
    'Public AskUserLastDayofSegment As Integer  'ENTER THE LAST DAY OF THE SEGMENT ************ enter a NUMBER ******'

#End Region

#Region "Runtime trackers"
    'internal use
    Private pCountDay As Integer = 0
    Private pCountRecession As Integer = 0
    Private pIndexPeakDay As Integer = 0

    'user interaction
    Private pFirstDayofSegment As Integer = 0
    Public Property AskUserFirstDayofSegment() As Integer
        Get
            Return pFirstDayofSegment
        End Get
        Set(ByVal value As Integer)
            pFirstDayofSegment = value
        End Set
    End Property
    Private pLastDayofSegment As Integer = 0
    Public Property AskUserLastDayofSegment() As Integer
        Get
            Return pLastDayofSegment
        End Get
        Set(ByVal value As Integer)
            pLastDayofSegment = value
        End Set
    End Property

    'must be integers
    Private pListRecessDelete As New ArrayList()
    Public Property ListOfRecessDelete() As ArrayList
        Get
            Return pListRecessDelete
        End Get
        Set(ByVal value As ArrayList)
            If value IsNot Nothing Then
                pListRecessDelete = value
            End If
        End Set
    End Property

    Private pIndicator() As Integer
    Private pXX() As Double
    Private pYY() As Double
    Private pZZ() As Double
    Private pX() As Double
    Private pY() As Double
    Private pK() As Double
    Private pDUMMY() As Double

    Private pXMeanAR() As Double
    Private pYMeanAR() As Double
    Private pCoef1AR() As Double
    Private pCoef2AR() As Double
    Private pMinAR() As Double
    Private pMaxAR() As Double
    Private pPickAR() As Integer
    Private pOrigNoAR() As Integer
    Private pDatesAR() As Double
    Private pXMNArray() As Double
    Private pCoefArray() As Double

    Private pQLogMax As Double = 0.0
    Private pQLogMin As Double = 10.0

#End Region

    Public Sub New(ByVal aTS As atcTimeseries)
        FlowData = aTS
        Dim lInputfile As String = IO.Path.GetFileName(aTS.Attributes.GetValue("history 1").substring("read from ".Length))
        pDataFilename = IO.Path.GetFileName(lInputfile)

        'Initialize arrays
        ReDim pIndicator(MaxNumDaysInAllRecPeriods)
        ReDim pXX(MaxNumDaysInAllRecPeriods)
        ReDim pYY(MaxNumDaysInAllRecPeriods)
        ReDim pZZ(MaxNumDaysInAllRecPeriods)
        ReDim pX(MaxNumRecPeriods)
        ReDim pY(MaxNumRecPeriods)
        ReDim pK(MaxNumRecPeriods)
        ReDim pDUMMY(MaxNumRecPeriods)

        ReDim pXMeanAR(MaxNumRecPeriods)
        ReDim pYMeanAR(MaxNumRecPeriods)
        ReDim pCoef1AR(MaxNumRecPeriods)
        ReDim pCoef2AR(MaxNumRecPeriods)
        ReDim pMinAR(MaxNumRecPeriods)
        ReDim pMaxAR(MaxNumRecPeriods)
        ReDim pPickAR(MaxNumRecPeriods)
        ReDim pOrigNoAR(MaxNumRecPeriods)
        ReDim pDatesAR(MaxNumRecPeriods)
        ReDim pXMNArray(MaxNumRecPeriods)
        ReDim pCoefArray(MaxNumRecPeriods)
        pXX(1) = -99
        pYY(1) = -99

        'Start off a recession search
        pCountDay = 2
        pIndexPeakDay = 0
        clsRecessionSegment.StreamFlowTS = pData
        clsRecessionSegment.RecessionCount = 0
        Tables = New atcCollection()
        Graphs = New atcCollection()
        RecessAnalysis()

    End Sub

    Public Function RecessAnalysis(Optional ByVal aOperation As String = "") As String

        Dim lMsg As New Text.StringBuilder
        Dim lFileRecSum As String = "recsum.txt"
        Dim lFileIndex As String = "index.txt"
        Dim lFileRecData As String = "recdata.txt"
        Dim lFileRanges As String = "ranges.txt"
        Dim lFileOut1 As String = ""
        Dim lFileOut2 As String = ""

        lFileRecSum = IO.Path.Combine(pOutputPath, lFileRecSum)
        lFileRecData = IO.Path.Combine(pOutputPath, lFileRecData)
        lFileIndex = IO.Path.Combine(pOutputPath, lFileIndex)
        lFileRanges = IO.Path.Combine(pOutputPath, lFileRanges)
        lFileOut1 = IO.Path.Combine(pOutputPath, "x" & SeasonLabel & pDataFilename)
        lFileOut2 = IO.Path.Combine(pOutputPath, "y" & SeasonLabel & pDataFilename)

        Dim lDate(5) As Integer
        J2Date(pData.Dates.Value(0), lDate)
        Dim lYearStart As Integer = lDate(0)
        J2Date(pData.Dates.Value(pData.numValues - 1), lDate)
        Dim lYearEnd As Integer = lDate(0)
        Dim lStrMonths As String = String.Join(",", RecessIncludeMonths.ToArray())

        pHeaderOutFile1 &= _
        " FILE " & lFileOut1 & "--  UNIT 10 OUTPUT OF RECESS.F " & vbCrLf & _
        " INPUT FILE = " & pDataFilename & vbCrLf & _
        " START = " & lYearStart & vbCrLf & _
        " END =   " & lYearEnd & vbCrLf & _
        " DAYS OF RECESSION REQUIRED FOR DETECTION=" & pRecessLengthInDays.ToString & vbCrLf & _
        " MONTHS SELECTED:" & lStrMonths & vbCrLf & _
        " " & vbCrLf & _
        "-----------------------------------------------------------------------" & vbCrLf & _
        "              RECESSION PERIODS INITIALLY SELECTED: " & vbCrLf & _
        "   LOG Q       RECESS.INDEX     TIME SINCE PEAK    .       DATE OF PEAK " & vbCrLf & _
        "   (MEAN)    ( -dT/d(LogQ) ) (START)(MIDDLE)(END)  .        (yr, mo, d) " & vbCrLf

        pHeaderOutFile2 &= _
        " FILE " & lFileOut2 & "--  UNIT 11 OUTPUT OF RECESS.F: " & vbCrLf & _
        " INPUT DATA FILE FOR THIS SESSION: ', INFILE" & vbCrLf & _
        " Tpeak is the time since the last peak  " & vbCrLf & _
        " Tmrc is the time on the Master Recession Curve " & vbCrLf & _
        " LogQ is the log of flow " & vbCrLf & _
        " Q is the flow " & vbCrLf & _
        " Seq# is the sequence number in which the segment " & vbCrLf & _
        " was selected.  " & vbCrLf & _
        "-----------------------------------------------------------------------"

        Dim lSW As IO.StreamWriter = Nothing
        ' ------------- LOCATE a PEAK ---------------------
        Dim lOK As Integer = 0

        'pCountRecession  'originally NMRECES
        Dim lDiff As Double = 0.0
        Dim lNumBlanks As Integer = 0
        Dim lNum As Integer
        Dim II As Integer = 1
        Dim liCount As Integer

        For liCount = pCountDay To pData.numValues 'original loop 200
            lOK = 0
            J2Date(pData.Dates.Value(liCount - 1), lDate)
            If Not RecessIncludeMonths.Contains(lDate(1)) Then
                Continue For 'loop 200
            End If
            Dim lCurrentValue As Double = pData.Value(liCount)
            If lCurrentValue <= pData.Value(liCount - 1) Or lCurrentValue <= pData.Value(liCount + 1) Then
                Continue For 'loop 200
            Else
                pIndexPeakDay = liCount
                lOK = 1
            End If

            '-------------- ANALYZE THE RECESSION AFTER THE PEAK: -----------------
            Dim liHowFar As Integer
            While True 'loop 210
                liCount += 1
                If liCount > pData.numValues Then Exit For 'loop 200
                If Math.Floor(100 * pData.Value(liCount)) > Math.Floor(100 * pData.Value(liCount - 1)) Then lOK = 0
                liHowFar = liCount - pIndexPeakDay - 1
                If lOK = 1 Then
                    Continue While 'loop 210
                Else
                    Exit While 'loop 210
                End If
            End While 'loop 210
            If liHowFar < RecessLengthInDays And lOK = 0 Then
                liCount -= 1
                Continue For 'loop 200
            End If
            If liHowFar >= RecessLengthInDays And lOK = 0 Then 'This is a long if branch
                RecessionSegment = New clsRecessionSegment()
                'Dim lFlow(60) As Double
                'Dim lQLog(60) As Double
                'Dim lDates(60) As Double
                'For I As Integer = 1 To 60 'loop 215
                '    lFlow(I) = 0.0
                '    lQLog(I) = -99.9
                'Next 'loop 215
                lNum = liCount - pIndexPeakDay - 1
                If lNum > clsRecessionSegment.MaxSegmentLengthInDays Then
                    lNum = clsRecessionSegment.MaxSegmentLengthInDays
                    liCount = pIndexPeakDay + clsRecessionSegment.MaxSegmentLengthInDays
                End If
                'Dim liMin As Integer = 1
                'Dim liMax As Integer = lNum
                'For I As Integer = 1 To lNum 'loop 220
                '    lFlow(I) = pData.Value(I + pIndexPeakDay)
                '    If lFlow(I) = 0.0 Then
                '        lQLog(I) = -88.8
                '    Else
                '        lQLog(I) = Math.Log10(lFlow(I))
                '    End If
                '    lDates(I) = pData.Dates.Value(I + pIndexPeakDay - 1)
                'Next 'loop 220
                With RecessionSegment
                    .PeakDayIndex = pIndexPeakDay
                    .PeakDayDate = pData.Dates.Value(pIndexPeakDay - 1)
                    .SegmentLength = lNum
                    .MinDayOrdinal = 1
                    .MaxDayOrdinal = lNum
                    .IsExcluded = True
                    .GetData()
                End With

                J2Date(pData.Dates.Value(pIndexPeakDay - 1), lDate)
                lMsg.AppendLine("Number of recession periods used so far = " & pCountRecession)
                lMsg.AppendLine("Date of New Peak = " & lDate(0) & "/" & lDate(1) & "/" & lDate(2))
                lMsg.AppendLine("Period of subsequent recession (days) = " & lNum)

                'Logger.Dbg("Number of recession periods used so far = " & pCountRecession)
                'Logger.Dbg("Date of New Peak = " & lDate(0) & "/" & lDate(1) & "/" & lDate(2))
                'Logger.Dbg("Period of subsequent recession (days) = " & lNum)

                If aOperation = "" Then
                    pCountDay = liCount
                    Return lMsg.ToString()
                End If

                'Whole bunch of user prompts
                If pHasWritePermission Then
                    lSW = New IO.StreamWriter(lFileRecData, True) 'keep appending to the file
                    lSW.WriteLine(1 + RecessionSegment.MaxDayOrdinal - RecessionSegment.MinDayOrdinal)
                    For I As Integer = RecessionSegment.MinDayOrdinal To RecessionSegment.MaxDayOrdinal 'loop 231
                        Dim lYGraph As Double = RecessionSegment.QLog(I)
                        If lYGraph < -80 Then lYGraph = -2
                        lSW.WriteLine(I.ToString.PadLeft(10, " ") & String.Format("{0:0.00000}", lYGraph).ToString.PadLeft(15, " "))
                    Next 'loop 231
                    lSW.Close()
                    lSW = Nothing
                End If

                'OPTIONS FOR DISPLAY USING THIS WINDOW --      '
                ' g  -- graphical display                      '
                ' t  -- tabular display of data                '
                'Recession data, on file "recdata.txt" can be displayed outside this '
                'window using the "recplot" application.  Data display ranges can be '
                'changed in file "ranges.txt".  Save and exit ranges.txt before continuing.'
                '                                              '
                'OPTIONS FOR PROGRAM ACTION --                 '
                ' c  -- choose first + last days of the        '
                '       recession segment.                     '
                ' b  -- un-do the effect of option c.          '
                ' a  -- advance to next recession period       '
                '       (do not use this one)                  '
                ' r  -- perform regression, store results in   '
                '       memory, and advance to next recession  '
                ' q --  quit.  This option is used to exit the '
                '       selection of recession segments.       '

                '230 marker



                Dim lPick As String = ""
                'The only purpose here seems to retrieve the three
                'parameters from the ranges.txt, could be set as user inputs
                'in the interface
                'Dim lSR As New IO.StreamReader(lFileRanges)
                'Dim liDisplayLastDay As Integer
                'Dim lXLogQMin As Double
                'Dim lXLogQMax As Double
                'Dim Line As String = lSR.ReadLine() 'title line
                'Line = lSR.ReadLine().Trim() '(Last day plotted)
                'Dim lArr() As String = Regex.Split(Line, "\s+")
                'liDisplayLastDay = CInt(lArr(0))
                'Line = lSR.ReadLine().Trim() '(Minimum value of LogQ plotted)
                'lArr = Regex.Split(Line, "\s+")
                'lXLogQMin = CDbl(lArr(0))
                'Line = lSR.ReadLine().Trim() '(Maximum value of LogQ plotted)
                'lArr = Regex.Split(Line, "\s+")
                'lXLogQMax = CDbl(lArr(0))
                'lSR.Close()
                'lSR = Nothing

                'This pickstartingday thing can be a user interface element
                'Dim liPickStartingDay As Integer = 1 'the only valid choices are 1, 11, 21
                'Dim liPickInterval As Integer = 1 'the only valid choices: ENTER TIME INTERVAL (1=EVERY DAY, 2=EVERY OTHER DAY)'

                If aOperation = "d" Then
                    'This branch only for display tabular recession data so far
                    'doesn't involve any real calculation, needs to be factored out
                    Dim lThisTable As String = ""
                    lMsg.Length = 0
                    If DisplayStartDayOrdinal <> 1 And DisplayStartDayOrdinal <> 11 And DisplayStartDayOrdinal <> 21 Then
                        'problem, can't do TableRecess
                    Else
                        'can save the table entries into some file
                        lMsg.AppendLine(TableRecess(RecessionSegment.QLog, RecessionSegment.Flow, RecessionSegment.Dates, pIndexPeakDay, RecessionSegment.MinDayOrdinal, RecessionSegment.MaxDayOrdinal, DisplayStartDayOrdinal, piDisplayLastDay))
                    End If
                    lMsg.AppendLine("                                              ")
                    lMsg.AppendLine("THIS --- INDICATES A DAY OUTSIDE THE PERIOD OF RECESSION, OR A DAY OUTSIDE ")
                    lMsg.AppendLine("OF THE SEGMENT THAT HAS BEEN SELECTED.")

                    Table = lMsg.ToString
                    GraphTs = New atcTimeseries(Nothing)
                    With GraphTs
                        .numValues = RecessionSegment.SegmentLength
                        .Dates.Values = RecessionSegment.Dates
                        .Values = RecessionSegment.QLog
                        .Value(0) = GetNaN()
                        .Dates.Value(0) = RecessionSegment.PeakDayDate
                        .Attributes.SetValue("YAxis", "LEFT")
                    End With
                    Return ""
                    'lSW = New IO.StreamWriter(IO.Path.Combine(pOutputPath, "Table.txt"), False)
                    'lSW.WriteLine(lThisTable)
                    'lSW.Flush() : lSW.Close() : lSW = Nothing
                    'Go To 230 marker to display again

                    'ElseIf lPick = "g" Or lPick = "g2" Then
                    '    'This branch only for display graph recession data so far
                    '    'doesn't involve any real calculation, needs to be factored out

                    '    Dim lThisGraph As String = ""
                    '    If DisplayInterval <> 1 And DisplayInterval <> 2 Then
                    '        'problem, can't do GraphRecess
                    '    Else
                    '        lThisGraph = GraphRecess(RecessionSegment.QLog, RecessionSegment.Flow, RecessionSegment.Dates, pIndexPeakDay, RecessionSegment.MinDayOrdinal, RecessionSegment.MaxDayOrdinal, pXLogQMin, pXLogQMax, DisplayStartDayOrdinal, DisplayInterval, piDisplayLastDay)
                    '    End If
                    '    lThisGraph &= vbCrLf & "                                              " & vbCrLf
                    '    lThisGraph &= "THIS * REPRESENTS FLOW.  THIS --- INDICATES A DAY OUTSIDE THE PERIOD  " & vbCrLf
                    '    lThisGraph &= "OF RECESSION, OR A DAY OUTSIDE OF THE SEGMENT THAT HAS BEEN SELECTED. " & vbCrLf
                    '    lThisGraph &= "THIS ::::: INDICATES FLOW OUTSIDE OF PLOTTING RANGE.                  "
                    '    lSW = New IO.StreamWriter(IO.Path.Combine(pOutputPath, "Graph.txt"), False)
                    '    lSW.WriteLine(lThisGraph)
                    '    lSW.Flush() : lSW.Close() : lSW = Nothing

                    'Go To 230 marker to display again
                ElseIf aOperation = "c" Then
                    RecessionSegment.MinDayOrdinal = AskUserFirstDayofSegment
                    RecessionSegment.MaxDayOrdinal = AskUserLastDayofSegment
                    RecessionSegment.GetDataSubset()
                    GraphTs.Clear()
                    With GraphTs
                        .numValues = RecessionSegment.Flow.Length - 1
                        .Dates.Values = RecessionSegment.Dates
                        .Values = RecessionSegment.QLog
                        .Value(0) = GetNaN()
                        .Dates.Value(0) = RecessionSegment.PeakDayDate 'need to modify here
                        .Attributes.SetValue("YAxis", "LEFT")
                    End With
                    'Dim liAskUserFirstDayofSegment As Integer = 2 'ENTER THE FIRST DAY OF THE SEGMENT *********** enter a NUMBER ******'
                    'Dim liAskUserLastDayofSegment As Integer = 8 'ENTER THE LAST DAY OF THE SEGMENT ************ enter a NUMBER ******'
                    'liMin = liAskUserFirstDayofSegment
                    'liMax = liAskUserLastDayofSegment
                    'Go To 230 marker to display again
                ElseIf lPick = "b" Then
                    'liMin = 1
                    'liMax = liCount - pIndexPeakDay - 1
                    'Go To 230 marker to display again

                ElseIf lPick = "a" Then
                    liCount -= 1
                    'Go To very beginning of loop 200
                    Continue For 'loop 200
                ElseIf lPick = "q" Then
                    Exit For
                ElseIf lPick = "r" Then
                    '    If lQLog(liMin) = lQLog(liMax) Then
                    '        liCount -= 1
                    '        'lMsg = ""
                    '        lMsg.AppendLine("THE PROGRAM SKIPPED THIS RECESSION")
                    '        lMsg.AppendLine("PERIOD BECAUSE FLOW DID NOT CHANGE")
                    '        lMsg.AppendLine("(RECESSION INDEX UNDEFINED).")
                    '        Logger.Dbg(lMsg.ToString)
                    '        Continue For 'loop 200
                    '    End If
                    '    If liMax - liMin > 49 Then
                    '        'lMsg = ""
                    '        lMsg.AppendLine("THE NUMBER OF DAYS SELECTED SHOULD BE")
                    '        lMsg.AppendLine("LESS THAN " & (pMaxNumRecPeriods + 1).ToString & " BEFORE PICKING OPTION r")
                    '        Logger.Dbg(lMsg.ToString)
                    '        'Go To 230 marker to display again
                    '    End If

                    '    pCountRecession += 1
                    '    If pCountRecession > pMaxNumRecPeriods Then
                    '        'lMsg = ""
                    '        lMsg.AppendLine("YOU HAVE ANALYZED THE MAXIMUM NUMBER OF RECESSION PERIODS.")
                    '        pCountRecession -= 1
                    '        Exit For 'loop 200
                    '    End If

                    '    Dim I As Integer = 0
                    '    For IT As Integer = liMin To liMax 'loop 240
                    '        I += 1
                    '        II += 1
                    '        If II > pMaxNumDaysInAllRecPeriods Then
                    '            'lMsg = ""
                    '            lMsg.AppendLine("THE TOTAL NUMBER OF DAYS IN ALL")
                    '            lMsg.AppendLine("SELECTED RECESSION PERIODS EXCEEDS")
                    '            lMsg.AppendLine("THE LIMIT OF " & pMaxNumDaysInAllRecPeriods & ".")
                    '            Logger.Dbg(lMsg.ToString)
                    '            Exit For 'loop 200
                    '        End If
                    '        pIndicator(II) = pCountRecession
                    '        pX(I) = lQLog(IT)
                    '        pY(I) = IT
                    '        pXX(II) = lQLog(IT)
                    '        pYY(II) = IT
                    '        If lQLog(IT) > pQLogMax Then pQLogMax = lQLog(IT)
                    '        If lQLog(IT) < pQLogMin Then pQLogMin = lQLog(IT)
                    '    Next 'loop 240

                    '    II += 1
                    '    pXX(II) = 0.0
                    '    pYY(II) = 0.0
                    '    Dim lXTotal As Double = 0
                    '    Dim lYTotal As Double = 0
                    '    For P As Integer = 1 To I 'loop 245
                    '        lXTotal += pX(P)
                    '        lYTotal += pY(P)
                    '    Next 'loop 245

                    '    Dim lXMean As Double = lXTotal / I
                    '    Dim lYMean As Double = lYTotal / I

                    '    Dim lCoeff1 As Double = 0
                    '    Dim lCoeff2 As Double = 0

                    '    Dim lResults As String = ""
                    '    lResults &= "     DAYS         LOG Q                  " & vbCrLf
                    '    lResults &= "   ( Y(I) )      ( X(I) )               I" & vbCrLf
                    '    For P As Integer = 1 To I
                    '        lResults &= pY(P).ToString.PadLeft(9, " ") & String.Format("{0:0.000000}", pX(P)).PadLeft(15, " ") & P.ToString.PadLeft(10, " ") & vbCrLf
                    '    Next
                    '    lResults &= vbCrLf
                    '    DoRegression2(pX, pY, I, lCoeff1, lCoeff2)
                    '    lResults &= " BEST-FIT EQUATION:" & vbCrLf
                    '    lResults &= " T = ( " & String.Format("{0:0.0000}", lCoeff1).PadLeft(12, " ") & "* LOGQ )  +  " & String.Format("{0:0.0000}", lCoeff2).PadLeft(12, " ") & vbCrLf
                    '    lResults &= " DAYS/LOG CYCLE=" & -1 * lCoeff1 & vbCrLf
                    '    lResults &= " MEAN LOG Q = " & lXMean & vbCrLf
                    '    lResults &= " " & vbCrLf
                    '    lSW = New IO.StreamWriter(lFileOut1)
                    '    lSW.WriteLine("*************** Regression Results *****************")
                    '    lSW.WriteLine(lResults)
                    '    lSW.WriteLine("****************************************************" & vbCrLf & vbCrLf)

                    '    lSW.WriteLine(pHeaderOutFile1)
                    '    '   19 FORMAT (1F10.5,1F15.3,3F8.1,10X,1I6,2I3)
                    '    J2Date(pData.Dates.Value(pIndexPeakDay - 1), lDate)
                    '    Dim lStrxMean As String = String.Format("{0:0.00000}", lXMean).PadLeft(10, " ")
                    '    Dim lStrCoeff1 As String = String.Format("{0:0.000}", -1 * lCoeff1).PadLeft(15, " ")
                    '    Dim lStriMin As String = String.Format("{0:0.0}", liMin).PadLeft(8, " ")
                    '    Dim lStryMean As String = String.Format("{0:0.0}", lYMean).PadLeft(8, " ")
                    '    Dim lStriMax As String = String.Format("{0:0.0}", liMax).PadLeft(8, " ")
                    '    Dim lStrBlnk As String = Space(10)
                    '    Dim lStrYear As String = lDate(0).ToString.PadLeft(6, " ")
                    '    Dim lStrMonth As String = lDate(1).ToString.PadLeft(3, " ")
                    '    Dim lStrDay As String = lDate(2).ToString.PadLeft(3, " ")

                    '    lSW.WriteLine(lStrxMean & lStrCoeff1 & lStriMin & lStryMean & lStriMax & lStrBlnk & lStrYear & lStrMonth & lStrDay)
                    '    lSW.Flush()
                    '    lSW.Close()
                    '    lSW = Nothing

                    '    pXMeanAR(pCountRecession) = lXMean
                    '    pYMeanAR(pCountRecession) = lYMean
                    '    pCoef1AR(pCountRecession) = lCoeff1
                    '    pCoef2AR(pCountRecession) = lCoeff2
                    '    pMinAR(pCountRecession) = liMin
                    '    pMaxAR(pCountRecession) = liMax
                    '    pDatesAR(pCountRecession) = pData.Dates.Value(pIndexPeakDay - 1)
                    '    If pCountRecession = pMaxNumRecPeriods Then
                    '        'lMsg = ""
                    '        lMsg.AppendLine("YOU HAVE ANALYZED " & pMaxNumRecPeriods & " RECESSIONS.")
                    '        lMsg.AppendLine("THIS IS THE MAXIMUM ALLOWABLE.")
                    '        Logger.Dbg(lMsg.ToString)
                    '    End If
                    '    pCountDay -= 1
                    '    Continue For 'loop 200
                    'Else
                    '    Logger.Dbg("OPTION NOT RECOGNIZED. CHOOSE AGAIN.")
                    '    'Go To 230 marker to display again
                End If
            End If 'end of long if branch
        Next 'original loop 200

    End Function

    Public Shared Function DoOperation(ByVal aOperation As String) As Boolean
        Select Case aOperation.ToLower
            Case "t"
            Case "g"
            Case "d"

            Case "c"
            Case "b"
            Case "a"
            Case "r"
            Case "q"

        End Select
    End Function


    '--------- THIS SUBROUTINE MAKES TABULAR OUTPUT OF RECESSION DATA: -----
    Public Shared Function TableRecess(ByVal aQLog() As Double, _
                            ByVal aFlow() As Double, _
                            ByVal aDates() As Double, _
                            ByVal aiPeak As Integer, _
                            ByVal aiMin As Integer, _
                            ByVal aiMax As Integer, _
                            ByVal aPickStartingDay As Integer, _
                            ByVal aiDisplayLastDay As Integer) As String

        'Logger.Dbg("ENTER STARTING DAY (1, 11, OR 21)") 'originally dynamically read in
        Dim liStart As Integer = aPickStartingDay
        Dim liEnd As Integer = liStart + aiDisplayLastDay - 1
        If liStart > 60 Then liStart = 60
        If liEnd > 60 Then liEnd = 60

        Dim lDelQLog(60) As Double
        For I As Integer = liStart To liEnd 'loop 20
            If I = 1 Then
                lDelQLog(I) = 999.0
            Else
                lDelQLog(I) = aQLog(I) - aQLog(I - 1)
            End If
        Next 'loop 20

        Dim lDate(5) As Integer
        Dim lStr As New System.Text.StringBuilder
        lStr.AppendLine("TIME AFTER         DELTA             TIME AFTER")
        lStr.AppendLine("   PEAK   LOG Q    LOG Q      Q        START   YEAR    MONTH    DAY")
        For I As Integer = liStart To liEnd 'loop 230
            If I > aiMax Or I < aiMin Then
                lStr.AppendLine("---")
            ElseIf aQLog(I) = -99.9 Then
                lStr.AppendLine("---")
            ElseIf aQLog(I) = -88.8 Then
                lStr.AppendLine("STREAMFLOW = ZERO")
            Else
                Dim liCountStr As String = I.ToString.PadLeft(6, " ")
                Dim lQLogStr As String = String.Format("{0:0.0000}", aQLog(I)).ToString.PadLeft(10, " ")
                Dim lDelQLogStr As String = String.Format("{0:0.0000}", lDelQLog(I)).ToString.PadLeft(10, " ")
                Dim lFlowStr As String = String.Format("{0:0.0000}", aFlow(I)).ToString.PadLeft(10, " ")
                Dim liCountPeakStr As String = (I + aiPeak).ToString.PadLeft(8, " ")
                J2Date(aDates(I), lDate)
                Dim lYearStr As String = lDate(0).ToString.PadLeft(8, " ")
                Dim lMonStr As String = lDate(1).ToString.PadLeft(8, " ")
                Dim lDayStr As String = lDate(2).ToString.PadLeft(8, " ")
                lStr.AppendLine(liCountStr & lQLogStr & lDelQLogStr & lFlowStr & liCountPeakStr & lYearStr & lMonStr & lDayStr)
            End If
        Next 'loop 230
        Return lStr.ToString
    End Function

    '------ THIS SUBROUTINE MAKES GRAPHICAL OUTPUT OF RECESSION DATA: ----
    Public Shared Function GraphRecess(ByVal aQLog() As Double, _
                            ByVal aFlow() As Double, _
                            ByVal aDates() As Double, _
                            ByVal aiPeak As Integer, _
                            ByVal aiMin As Integer, _
                            ByVal aiMax As Integer, _
                            ByVal aXLogQMin As Double, _
                            ByVal aXLogQMax As Double, _
                            ByVal aPickStartingDay As Integer, _
                            ByVal aPickInterval As Integer, _
                            ByVal aiDisplayLastDay As Integer) As String

        Dim liStart As Integer = aPickStartingDay
        Dim linterval As Integer = aPickInterval
        Dim liEnd As Integer = liStart + (aiDisplayLastDay - 1) * aPickInterval
        If liStart > 60 Then liStart = 60
        If liEnd > 60 Then liEnd = 60

        Dim liiEnd As Integer = liStart
        While True 'loop 8
            liiEnd += aPickInterval
            If liiEnd < 60 And liiEnd < liEnd Then
                Continue While
            Else
                Exit While
            End If
        End While 'loop 8
        If liiEnd > liEnd Then
            liEnd -= aPickInterval
        Else
            liEnd = liiEnd
        End If

        Dim lStr As New Text.StringBuilder
        '9 FORMAT (A8, 6X, 1F6.2, 55X, 1F6.2)
        lStr.AppendLine("LOG Q =       " & String.Format("{0:0.00}", aXLogQMin).PadLeft(6, " ") & Space(55) & String.Format("{0:0.00}", aXLogQMax).PadLeft(6, " "))
        Dim lDate(5) As Integer
        For I As Integer = liStart To liEnd Step aPickInterval
            If aDates(I) = 0 Then
                lDate(0) = 0 : lDate(1) = 0 : lDate(2) = 0
            Else
                J2Date(aDates(I), lDate)
            End If
            Dim liStr As String = I.ToString.PadLeft(3, " ")
            Dim lYearStr As String = lDate(0).ToString.PadLeft(5, " ")
            Dim lMonStr As String = lDate(1).ToString.PadLeft(3, " ")
            Dim lDayStr As String = lDate(2).ToString.PadLeft(3, " ")
            Dim lSymbol As String = ""
            If I > aiMax Or I < aiMin Then
                lSymbol = Space(52) & "---"
            ElseIf aQLog(I) = -99.9 Then
                lSymbol = Space(52) & "---"
            ElseIf aQLog(I) = -88.8 Then
                lSymbol = "STREAMFLOW = ZERO "
            ElseIf aQLog(I) > aXLogQMax Or aQLog(I) < aXLogQMin Then
                lSymbol = Space(52) & ":::::"
            Else
                Dim lDiff As Double = aQLog(I) - aXLogQMin
                Dim lNumBlank As Integer = Math.Ceiling(lDiff * 60 / (aXLogQMax - aXLogQMin))
                lSymbol = Space(lNumBlank) & "*"
            End If

            lStr.AppendLine(liStr & lYearStr & lMonStr & lDayStr & lSymbol)

        Next
        Return lStr.ToString
    End Function

    ' -----THIS SUBROUTINE PERFORMS LEAST-SQUARES REGRESSION TO FIND BEST-FIT ---
    ' ---------------- EQUATION OF LINEAR BASIS ( Y = A*X + B ) -----------------
    Public Shared Sub DoRegression2(ByVal aX() As Double, ByVal aY() As Double, ByVal aNumOfDaysInPeriod As Integer, ByRef aCoeff1 As Double, ByRef aCoeff2 As Double)
        Dim lA(2, 4) As Double
        Dim lRoot1 As Double = 0.0
        Dim lRoot2 As Double = 0.0
        For I As Integer = 1 To aNumOfDaysInPeriod
            lA(1, 1) += aX(I) ^ 2
            lA(1, 2) += aX(I)
            lA(2, 1) += aX(I)
            lRoot1 += aX(I) * aY(I)
            lRoot2 += aY(I)
        Next

        lA(2, 2) = aNumOfDaysInPeriod
        Dim lN As Integer = 2

        Inverse(lA, lN)

        aCoeff1 = lA(1, 1) * lRoot1 + lA(1, 2) * lRoot2
        aCoeff2 = lA(2, 1) * lRoot1 + lA(2, 2) * lRoot2

    End Sub

    ' -----  THIS SUBROUTINE CHANGES AN N*N MATRIX [A] TO ITS INVERSE -----
    ' ---------  (Note: The matrix is actually N*(2N) internally) ---------
    Public Shared Sub Inverse(ByRef A(,) As Double, ByVal N As Integer)
        For I As Integer = 1 To N
            For J As Integer = N + 1 To 2 * N
                A(I, J) = 0.0
            Next
        Next

        For I As Integer = 1 To N
            A(I, N + I) = 1.0
        Next

        For K As Integer = 1 To N
            For I As Integer = 1 To N
                Dim lTemp As Double = A(I, K)
                For J As Integer = 1 To 2 * N
                    A(I, J) /= lTemp
                Next
            Next

            For I As Integer = 1 To N
                If I <> K Then
                    For J As Integer = 1 To 2 * N
                        A(I, J) -= A(K, J)
                    Next
                End If
            Next
        Next

        For I As Integer = 1 To N
            Dim lAP As Double = A(I, I)
            For J As Integer = 1 To 2 * N
                A(I, J) /= lAP
            Next
        Next

        For I As Integer = 1 To N
            For J As Integer = 1 To N
                A(I, J) = A(I, J + N)
            Next
        Next
    End Sub

    '  INPUT IS 3 LISTS OF NUMBERS. ALL LISTS (LIST1,LIST2,AND LIST3) HAVE
    '  M NUMBERS. OUTPUT IS SAME, IN ASCENDING ORDER, SORTED BY VALUES
    '  IN LIST1. NOTE: LIST3 IS MADE UP OF INTEGERS.
    Public Shared Sub Order(ByVal aNumOfRecessPeriods As Integer, ByRef aList1XMean() As Double, ByRef aList2Coef1() As Double, ByRef aList3OriginalOrder() As Integer)
        Dim lSwaped As String = "YES"
        Dim lPasses As Integer = 0
        Dim lTempValue As Double
        While True
            lSwaped = "NO"
            lPasses += 1
            Dim I As Integer = 1
            While True
                If aList1XMean(I) > aList1XMean(I + 1) Then
                    lTempValue = aList1XMean(I + 1)
                    aList1XMean(I + 1) = aList1XMean(I)
                    aList1XMean(I) = lTempValue

                    lTempValue = aList2Coef1(I + 1)
                    aList2Coef1(I + 1) = aList2Coef1(I)
                    aList2Coef1(I) = lTempValue

                    lTempValue = aList3OriginalOrder(I + 1)
                    aList3OriginalOrder(I + 1) = aList3OriginalOrder(I)
                    aList3OriginalOrder(I) = lTempValue

                    lSwaped = "YES"
                End If
                I += 1
                If I <= aNumOfRecessPeriods - lPasses Then
                    Continue While
                Else
                    Exit While
                End If
            End While
            If lSwaped = "YES" Then
                Continue While
            Else
                Exit While
            End If
        End While
    End Sub
End Class
