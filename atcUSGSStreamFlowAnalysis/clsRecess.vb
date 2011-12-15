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

    Private pRecessMinLengthInDays As Integer = 0
    Public Property RecessMinLengthInDays() As Integer
        Get
            Return pRecessMinLengthInDays
        End Get
        Set(ByVal value As Integer)
            If value <= 0 Then
                value = 5
            End If
            pRecessMinLengthInDays = value
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

    Private pRecessionIndex As Double
    Public ReadOnly Property RecessionIndex() As Double
        Get
            Return pRecessionIndex
        End Get
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
                    lSW = New StreamWriter(IO.Path.Combine(value, "z.txt"), False)
                    lSW.WriteLine("1")
                    lSW.Close()
                    lSW = Nothing
                    pOutputPath = value
                    pHasWritePermission = True
                Catch ex As Exception
                    If lSW IsNot Nothing Then
                        lSW.Close()
                        lSW = Nothing
                    End If
                    pHasWritePermission = False
                End Try
            Else
                pHasWritePermission = False
            End If
        End Set
    End Property

    Private pDataFilename As String = ""
    Public SaveInterimResults As Boolean = False
    Private pFileRecSum As String = "recsum.txt"
    Private pFileIndex As String = "index.txt"
    Private pFileRecData As String = "recdata.txt"
    Private pFileRanges As String = "ranges.txt"
    Private pHeaderOutFile1 As String = ""
    Private pHeaderOutFile2 As String = ""
    Private pHeaderIndexFile As String = ""
    Private pHeaderRecSumFile As String = ""

    Private pFileOut1 As String = ""
    Private pFileOut2 As String = ""
    Public FileOut1Created As Boolean = False
    Public FileOut2Created As Boolean = False

    Private Sub SetOutputFiles()
        pFileRecSum = IO.Path.Combine(OutputPath, pFileRecSum)
        pFileRecData = IO.Path.Combine(OutputPath, pFileRecData)
        pFileIndex = IO.Path.Combine(OutputPath, pFileIndex)
        pFileRanges = IO.Path.Combine(OutputPath, pFileRanges)

        pFileOut1 = IO.Path.Combine(OutputPath, "x" & SeasonLabel & "." & pDataFilename)
        pFileOut2 = IO.Path.Combine(OutputPath, "y" & SeasonLabel & "." & pDataFilename)
    End Sub
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

    'Public Tables As atcCollection = Nothing
    'Public Graphs As atcCollection = Nothing

    Public Table As String 'current recession segment's table
    Public GraphTs As atcTimeseries 'current recession's timeseries

    Public Bulletin As String = ""

    Public listOfSegments As atcCollection

#End Region

#Region "Runtime trackers"
    'internal use
    Private pCountDay As Integer = 0
    Private pCountRecession As Integer = 0 'originally NMRECES
    Private pIndexPeakDay As Integer = 0

    'user interaction
    'FIRST DAY OF THE SEGMENT (a NUMBER)
    Private pFirstDayofSegment As Integer = 0
    Public Property AskUserFirstDayofSegment() As Integer
        Get
            Return pFirstDayofSegment
        End Get
        Set(ByVal value As Integer)
            pFirstDayofSegment = value
        End Set
    End Property
    'LAST DAY OF THE SEGMENT (a NUMBER)
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

    'Private pIndicator() As Integer
    'Private pXX() As Double
    'Private pYY() As Double
    'Private pZZ() As Double
    'Private pX() As Double
    'Private pY() As Double
    'Private pK() As Double
    'Private pDUMMY() As Double

    'Private pXMeanAR() As Double
    'Private pYMeanAR() As Double
    'Private pCoef1AR() As Double
    'Private pCoef2AR() As Double
    'Private pMinAR() As Double
    'Private pMaxAR() As Double
    'Private pPickAR() As Integer
    'Private pOrigNoAR() As Integer
    'Private pDatesAR() As Double
    'Private pXMNArray() As Double
    'Private pCoefArray() As Double

    Private pQLogMax As Double = 0.0
    Private pQLogMin As Double = 10.0

#End Region

    Public Sub New()

    End Sub

    Public Sub Initialize(ByVal aTS As atcTimeseries, Optional ByVal aArgs As atcDataAttributes = Nothing)

        Dim lSdate As Double
        Dim lEdate As Double
        Dim lDate(5) As Integer
        If aArgs IsNot Nothing Then
            lSdate = aArgs.GetValue("Start Date", aTS.Dates.Value(0))
            lEdate = aArgs.GetValue("End Date", aTS.Dates.Value(aTS.numValues))
            RecessIncludeMonths = aArgs.GetValue("SelectedMonths", New ArrayList())
            SeasonLabel = aArgs.GetValue("Season", "NoSeason")
            OutputPath = aArgs.GetValue("Output Path", "")
            RecessMinLengthInDays = aArgs.GetValue("MinSegmentLength", 5)
            SaveInterimResults = aArgs.GetValue("SaveInterimResults", False)
        End If

        Dim lTs As atcTimeseries = SubsetByDate(aTS, lSdate, lEdate, Nothing)
        FlowData = lTs

        '***** File IO *****
        Dim lInputfile As String = IO.Path.GetFileName(aTS.Attributes.GetValue("history 1").substring("read from ".Length))
        pDataFilename = IO.Path.GetFileName(lInputfile)
        If SaveInterimResults Then
            SetOutputFiles()
            If pHasWritePermission Then
                Dim lSW As StreamWriter = Nothing
                lSW = New StreamWriter(pFileOut1, False)
                lSW.Close() : lSW = Nothing
                lSW = New StreamWriter(pFileOut2, False)
                lSW.Close() : lSW = Nothing
                FileOut1Created = True
                FileOut1Created = True
            End If
        End If

        Dim lYearStart As Integer
        J2Date(lSdate, lDate) : lYearStart = lDate(0)
        Dim lYearEnd As Integer
        J2Date(lEdate - JulianHour * 24, lDate) : lYearEnd = lDate(0)

        Dim lStrMonths As String = ""
        For Each lMonth As Integer In RecessIncludeMonths
            lStrMonths &= lMonth & ", "
        Next
        lStrMonths = lStrMonths.Substring(0, lStrMonths.Length - 2)

        pHeaderOutFile1 &= _
" FILE " & pFileOut1 & "--  UNIT 10 OUTPUT OF RECESS.F " & vbCrLf & _
" INPUT FILE = " & pDataFilename & vbCrLf & _
" START = " & lYearStart & vbCrLf & _
" END =   " & lYearEnd & vbCrLf & _
" DAYS OF RECESSION REQUIRED FOR DETECTION=" & pRecessMinLengthInDays.ToString & vbCrLf & _
" MONTHS SELECTED:" & lStrMonths & vbCrLf & _
" " & vbCrLf & _
"-----------------------------------------------------------------------" & vbCrLf & _
"              RECESSION PERIODS INITIALLY SELECTED: " & vbCrLf & _
"   LOG Q       RECESS.INDEX     TIME SINCE PEAK    .       DATE OF PEAK " & vbCrLf & _
"   (MEAN)    ( -dT/d(LogQ) ) (START)(MIDDLE)(END)  .        (yr, mo, d) " & vbCrLf

        pHeaderOutFile2 &= _
" FILE " & pFileOut2 & "--  UNIT 11 OUTPUT OF RECESS.F: " & vbCrLf & _
" INPUT DATA FILE FOR THIS SESSION: ', INFILE" & vbCrLf & _
" Tpeak is the time since the last peak  " & vbCrLf & _
" Tmrc is the time on the Master Recession Curve " & vbCrLf & _
" LogQ is the log of flow " & vbCrLf & _
" Q is the flow " & vbCrLf & _
" Seq# is the sequence number in which the segment " & vbCrLf & _
" was selected.  " & vbCrLf & _
"-----------------------------------------------------------------------"

        pHeaderRecSumFile = "File ""recsum.txt""      Program version -- Jan 2007" & vbCrLf & _
"--------------------------------------------------------------" & vbCrLf & _
"Each time the recess program is run, a new line can be written" & vbCrLf & _
"to the end of this file.  Guide to columns --" & vbCrLf & vbCrLf & _
"  File = streamflow file analyzed" & vbCrLf & _
"     S = Season analyzed" & vbCrLf & _
"     P = Period analyzed (years)" & vbCrLf & _
"     # = Number of segments analyzed" & vbCrLf & vbCrLf & _
"  Kmin = Minimum recession index, among segments selected" & vbCrLf & _
"  Kmed = Median recession index, among segments selected" & vbCrLf & _
"  Kmax = Maximum recession index, among segments selected" & vbCrLf & vbCrLf & _
"LogQmn = The minimum value of the log of the streamflow" & vbCrLf & _
"LogQmx = The maximum value of the log of the streamflow" & vbCrLf & _
" A,B,C = Coefficients of the master recession index" & vbCrLf & vbCrLf & _
"    File    S     P      #  Kmin  Kmed  Kmax  LogQmn  LogQmx     A         B         C" & vbCrLf & _
"-----------------------------------------------------------------------------------------" & vbCrLf & vbCrLf

        pHeaderIndexFile = "File ""index.txt"" -- The recession index is entered on this file" & vbCrLf & _
"manually or by running the RECESS program.  This file ""index.txt""" & vbCrLf & _
"can be read by the PREP program (before running PULSE) and it can" & vbCrLf & _
"be read by RORA.  Note -- this file should have ten header lines." & vbCrLf & _
"----------------------------------------------------------------" & vbCrLf & _
"              Recession" & vbCrLf & _
"Name of         index" & vbCrLf & _
"streamflow    (days per" & vbCrLf & _
"file          log cycle)" & vbCrLf & _
"------------------------"
        'Indian.txt    100.00  (string12, f8.2)
        'Indian.txt     22.92

        ''Initialize arrays
        'ReDim pIndicator(MaxNumDaysInAllRecPeriods)
        'ReDim pXX(MaxNumDaysInAllRecPeriods)
        'ReDim pYY(MaxNumDaysInAllRecPeriods)
        'ReDim pZZ(MaxNumDaysInAllRecPeriods)
        'ReDim pX(MaxNumRecPeriods)
        'ReDim pY(MaxNumRecPeriods)
        'ReDim pK(MaxNumRecPeriods)
        'ReDim pDUMMY(MaxNumRecPeriods)

        'ReDim pXMeanAR(MaxNumRecPeriods)
        'ReDim pYMeanAR(MaxNumRecPeriods)
        'ReDim pCoef1AR(MaxNumRecPeriods)
        'ReDim pCoef2AR(MaxNumRecPeriods)
        'ReDim pMinAR(MaxNumRecPeriods)
        'ReDim pMaxAR(MaxNumRecPeriods)
        'ReDim pPickAR(MaxNumRecPeriods)
        'ReDim pOrigNoAR(MaxNumRecPeriods)
        'ReDim pDatesAR(MaxNumRecPeriods)
        'ReDim pXMNArray(MaxNumRecPeriods)
        'ReDim pCoefArray(MaxNumRecPeriods)
        'pXX(1) = -99
        'pYY(1) = -99

        'Start off a recession search
        pCountDay = 2
        pIndexPeakDay = 0
        clsRecessionSegment.StreamFlowTS = FlowData
        clsRecessionSegment.RecessionCount = 0
        'Tables = New atcCollection()
        'Graphs = New atcCollection()
        If listOfSegments IsNot Nothing AndAlso listOfSegments.Count > 0 Then
            For Each lSeg As clsRecessionSegment In listOfSegments
                lSeg.Clear()
            Next
            listOfSegments = Nothing
        End If
        listOfSegments = New atcCollection()
        'RecessAnalysis()

    End Sub

    Public Sub Clear()
        'in case when user changed the streamflow duration
        'then has to find everything again, need to clear existing
        If listOfSegments IsNot Nothing AndAlso listOfSegments.Count > 0 Then
            For Each lSeg As clsRecessionSegment In listOfSegments
                lSeg.Clear()
            Next
            listOfSegments.Clear()
        End If

        FlowData = Nothing
        FileOut1Created = False
        FileOut2Created = False
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
        lFileOut1 = IO.Path.Combine(pOutputPath, "x" & SeasonLabel & "." & pDataFilename)
        lFileOut2 = IO.Path.Combine(pOutputPath, "y" & SeasonLabel & "." & pDataFilename)

        Dim lDate(5) As Integer
        J2Date(pData.Dates.Value(0), lDate)
        Dim lYearStart As Integer = lDate(0)
        J2Date(pData.Dates.Value(pData.numValues - 1), lDate)
        Dim lYearEnd As Integer = lDate(0)
        Dim lStrMonths As String = String.Join(",", RecessIncludeMonths.ToArray())

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
            If liHowFar < RecessMinLengthInDays And lOK = 0 Then
                liCount -= 1
                Continue For 'loop 200
            End If
            If liHowFar >= RecessMinLengthInDays And lOK = 0 Then 'This is a long if branch
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

                Else
                    Logger.Dbg("OPTION NOT RECOGNIZED. CHOOSE AGAIN.")
                    'Go To 230 marker to display again
                End If
            End If 'end of long if branch
        Next 'original loop 200
        Return Nothing
    End Function

    Public Sub RecessGetAllSegments()
        Dim lDate(5) As Integer

        ' ------------- LOCATE a PEAK ---------------------
        Dim lOK As Integer = 0
        'pCountRecession  
        Dim lNum As Integer
        Dim II As Integer = 1
        Dim liCount As Integer
        Dim lKey As String
        Dim liHowFar As Integer
        For liCount = 2 To pData.numValues 'original loop 200
            lOK = 0
            J2Date(pData.Dates.Value(liCount - 1), lDate)
            If Not RecessIncludeMonths.Contains(lDate(1)) Then
                Continue For 'loop 200
            End If

            If liCount >= pData.numValues Then Continue For
            Dim lCurrentValue As Double = pData.Value(liCount)
            If liCount < pData.numValues AndAlso (lCurrentValue <= pData.Value(liCount - 1) Or lCurrentValue <= pData.Value(liCount + 1)) Then
                Continue For 'loop 200
            Else
                pIndexPeakDay = liCount
                lOK = 1
            End If

            '-------------- ANALYZE THE RECESSION AFTER THE PEAK: -----------------
            liHowFar = 0
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
            If liHowFar < RecessMinLengthInDays And lOK = 0 Then
                liCount -= 1
                Continue For 'loop 200
            End If
            If liHowFar >= RecessMinLengthInDays And lOK = 0 Then 'This is a long if branch
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
                    '.GetData() get data later to save memory
                End With
                pCountRecession += 1
                J2Date(pData.Dates.Value(pIndexPeakDay - 1), lDate)
                lKey = lDate(0).ToString & "/" & lDate(1).ToString.PadLeft(2, " ") & "/" & lDate(2).ToString.PadLeft(2, " ")
                listOfSegments.Add(lKey, RecessionSegment)
            End If 'end of long if branch
        Next 'original loop 200
    End Sub

    Public Function DoOperation(ByVal aOperation As String, ByVal aRecessKey As String) As Boolean
        If aRecessKey <> "" Then
            RecessionSegment = listOfSegments.ItemByKey(aRecessKey)
            If RecessionSegment.NeedtoReadData Then
                RecessionSegment.GetData()
            End If
        End If
        Select Case aOperation.ToLower
            Case "d"
                RecessDisplay(RecessionSegment)
            Case "r"
                'If RecessionSegment.NeedToAnalyse Then
                'End If
                RecessAnalyse(RecessionSegment)
            Case "select"
                RecessionSegment.IsExcluded = False
            Case "unselect"
                RecessionSegment.IsExcluded = True
            Case "summary"
                RecessSummary()
            Case "q"

        End Select
    End Function

    Private Sub RecessDisplay(ByVal aSegment As clsRecessionSegment)
        'This branch only for display tabular recession data so far
        'doesn't involve any real calculation, needs to be factored out
        Dim lThisTable As String = ""
        Dim lMsg As New Text.StringBuilder
        lMsg.Length = 0
        With aSegment
            'If .MinDayOrdinal <> 1 And .MinDayOrdinal <> 11 And .MinDayOrdinal <> 21 Then
            '    'problem, can't do TableRecess
            'Else
            '    'can save the table entries into some file
            '    lMsg.AppendLine(TableRecess(.QLog, .Flow, .Dates, .PeakDayIndex, .MinDayOrdinal, .MaxDayOrdinal, .MinDayOrdinal, .MaxDayOrdinal))
            'End If
            'can save the table entries into some file
            lMsg.AppendLine(TableRecess(.QLog, .Flow, .Dates, .PeakDayIndex, .MinDayOrdinal, .MaxDayOrdinal, .MinDayOrdinal, .MaxDayOrdinal))
            'lMsg.AppendLine("                                              ")
            'lMsg.AppendLine("THIS --- INDICATES A DAY OUTSIDE THE PERIOD OF RECESSION, OR A DAY OUTSIDE ")
            'lMsg.AppendLine("OF THE SEGMENT THAT HAS BEEN SELECTED.")
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
            Dim lSubsetQLog(aSegment.MaxDayOrdinal - aSegment.MinDayOrdinal + 1) As Double
            For I As Integer = 1 To aSegment.QLog.Length - 1
                If I >= aSegment.MinDayOrdinal AndAlso I <= aSegment.MaxDayOrdinal Then
                    lSubsetQLog(I - aSegment.MinDayOrdinal + 1) = aSegment.QLog(I)
                End If
            Next
            .Values = lSubsetQLog
            .Value(0) = GetNaN()
            '.Dates.Value(0) = aSegment.PeakDayDate
            .Dates.Value(0) = lSDate - JulianHour * 24.0
            .Attributes.SetValue("YAxis", "LEFT")
            .Attributes.SetValue("point", True)
            .Attributes.SetValue("Constituent", "")
            .Attributes.SetValue("Scenario", "")
            .Attributes.SetValue("Units", "Log(Flow, cfs)")
        End With
    End Sub

    Private Function RecessAnalyse(ByVal aSegment As clsRecessionSegment) As String
        Dim lMsg As New Text.StringBuilder
        With aSegment
            If .QLog(.MinDayOrdinal) = .QLog(.MaxDayOrdinal) Then
                lMsg.AppendLine("Recession period flow did not change. Skipped")
                'Go To 230 marker to display again
                Return lMsg.ToString
            End If
            'If liMax - liMin > 49 Then
            If .MaxDayOrdinal - .MinDayOrdinal > clsRecessionSegment.MaxSegmentLengthInDays Then
                lMsg.AppendLine("Recession period is too long (> " & clsRecessionSegment.MaxSegmentLengthInDays & " days). Skipped.")
                'Go To 230 marker to display again
                Return lMsg.ToString
            End If

            'TODO: don't need to restrict total number of recession periods???
            'pCountRecession += 1
            'If pCountRecession > pMaxNumRecPeriods Then
            '    'lMsg = ""
            '    lMsg.AppendLine("YOU HAVE ANALYZED THE MAXIMUM NUMBER OF RECESSION PERIODS.")
            '    pCountRecession -= 1
            '    Exit For 'loop 200
            'End If
            Dim lTotalLogQ As Double = 0
            Dim lTotalOrdinal As Double = 0
            For I As Integer = .MinDayOrdinal To .MaxDayOrdinal
                lTotalLogQ += .QLog(I)
                lTotalOrdinal += I
            Next
            .MeanLogQ = lTotalLogQ / (.MaxDayOrdinal - .MinDayOrdinal + 1)
            .MeanOrdinals = lTotalOrdinal / (.MaxDayOrdinal - .MinDayOrdinal + 1)

            DoRegression2(aSegment)
            'set analysed done
            aSegment.NeedToAnalyse = False

            lMsg.AppendLine("BEST-FIT EQUATION:")
            lMsg.AppendLine(.BestFitEquation)
            lMsg.AppendLine(" DAYS/LOG CYCLE= " & String.Format("{0:0.000000}", -1 * .Coefficient1))
            lMsg.AppendLine(" MEAN LOG Q = " & String.Format("{0:0.000000}", .MeanLogQ))

            If SaveInterimResults AndAlso pHasWritePermission AndAlso aSegment.NeedToAnalyse Then
                Dim lSW As IO.StreamWriter = New IO.StreamWriter(pFileOut1, FileOut1Created)
                Dim lDate(5) As Integer
                lSW.WriteLine(pHeaderOutFile1)
                '   19 FORMAT (1F10.5,1F15.3,3F8.1,10X,1I6,2I3)

                J2Date(.PeakDayDate, lDate)
                Dim lStrxMean As String = String.Format("{0:0.00000}", .MeanLogQ).PadLeft(10, " ")
                Dim lStrCoeff1 As String = String.Format("{0:0.000}", -1 * .Coefficient1).PadLeft(15, " ")
                Dim lStriMin As String = String.Format("{0:0.0}", .MinDayOrdinal).PadLeft(8, " ")
                Dim lStryMean As String = String.Format("{0:0.0}", .MeanOrdinals).PadLeft(8, " ")
                Dim lStriMax As String = String.Format("{0:0.0}", .MaxDayOrdinal).PadLeft(8, " ")
                Dim lStrBlnk As String = Space(10)
                Dim lStrYear As String = lDate(0).ToString.PadLeft(6, " ")
                Dim lStrMonth As String = lDate(1).ToString.PadLeft(3, " ")
                Dim lStrDay As String = lDate(2).ToString.PadLeft(3, " ")

                lSW.WriteLine(lStrxMean & lStrCoeff1 & lStriMin & lStryMean & lStriMax & lStrBlnk & lStrYear & lStrMonth & lStrDay)

                lSW.WriteLine("*************** Regression Results *****************")
                lSW.WriteLine(lMsg.ToString)
                lSW.WriteLine("****************************************************" & vbCrLf & vbCrLf)
                lSW.Flush()
                lSW.Close()
                lSW = Nothing
            End If
        End With
        'pXMeanAR(pCountRecession) = lXMean
        'pYMeanAR(pCountRecession) = lYMean
        'pCoef1AR(pCountRecession) = lCoeff1
        'pCoef2AR(pCountRecession) = lCoeff2
        'pMinAR(pCountRecession) = liMin
        'pMaxAR(pCountRecession) = liMax
        'pDatesAR(pCountRecession) = pData.Dates.Value(pIndexPeakDay - 1)
        'If pCountRecession = pMaxNumRecPeriods Then
        '    'lMsg = ""
        '    lMsg.AppendLine("YOU HAVE ANALYZED " & pMaxNumRecPeriods & " RECESSIONS.")
        '    lMsg.AppendLine("THIS IS THE MAXIMUM ALLOWABLE.")
        '    Logger.Dbg(lMsg.ToString)
        'End If
        'pCountDay -= 1
        'Continue For 'loop 200
        Bulletin = lMsg.ToString
        Return lMsg.ToString
    End Function

    Public Sub RecessSummary()

        If SaveInterimResults Then
            SetOutputFiles()
        End If

        Dim lSW As IO.StreamWriter = Nothing
        Dim lMsg As Text.StringBuilder = Nothing
        Dim lSeg As clsRecessionSegment = Nothing
        Dim lDiff As Double = 0
        '----------CONTINUE AFTER RECESSION PERIODS HAVE BEEN SELECTED:-------
        ' ----- COUNT TOTAL NUMBER OF DAYS INVOLVED ------------
        ' ----- DETERMINE MAX AND MIN K AND TRANSFER LOGQ AND K TO OTHER ----
        ' -------- VARIABLES FOR LISTING THEM BY DECREASING LOGQ:  --------
        Dim liiDV As Integer = 0
        Dim lSlopeMx As Double = 0
        Dim lSlopeMn As Double = 2000.0
        Dim lSlope As Double
        XLogQMax = 0 'max among all available peaks
        XLogQMin = 3000 'min among all available peaks
        For Each lSeg In listOfSegments
            With lSeg
                If .NeedtoReadData Then
                    .GetData()
                End If
                DoOperation("r", lSeg.PeakDayDateToString)
                If Not .IsExcluded Then
                    liiDV += .MaxDayOrdinal - .MinDayOrdinal + 1
                End If
                lSlope = -1 * .Coefficient1
                If lSlope > lSlopeMx Then lSlopeMx = lSlope
                If lSlope < lSlopeMn Then lSlopeMn = lSlope

                For I As Integer = .MinDayOrdinal To .MaxDayOrdinal
                    If .QLog(I) > XLogQMax Then XLogQMax = .QLog(I)
                    If .QLog(I) < XLogQMin Then XLogQMin = .QLog(I)
                Next
            End With
        Next

        If SaveInterimResults And pHasWritePermission Then
            lSW = New IO.StreamWriter(pFileOut1, FileOut1Created)
            lSW.WriteLine("TOTAL NUMBER OF DAILY VALUES OF STREAMFLOW THAT WERE USED, FOR ALL RECESSION")
            lSW.WriteLine("PERIODS INITIALLY SELECTED = " & liiDV)
            lSW.Flush()
            lSW.Close()
            lSW = Nothing
        End If

        'For Z As Integer = 1 To lNumRecessPeriods
        '    lSlope = -1 * lCoef1AR(Z)
        '    If lSlope > lSlopeMx Then lSlopeMx = lSlope
        '    If lSlope < lSlopeMn Then lSlopeMn = lSlope
        '    lOrigNoAR(Z) = Z
        '    lXMNArray(Z) = lXMeanAR(Z)
        '    lCoefArray(Z) = lCoef1AR(Z)
        'Next

        'sort the three arrays (of the same size) to be in ascending order
        'Order(lNumRecessPeriods, lXMNArray, lCoefArray, lOrigNoAR)

        Dim lListOfChosenSegments As New atcCollection

        Dim lCoef1Max As Double = 0
        Dim lCoef1Min As Double = 3000
        Dim lMeanQLogMaxC As Double = 0 'max among selected/chosen segs
        Dim lMeanQLogMinC As Double = 3000 'min among selected/chosen segs
        For Each lPeakDate As String In listOfSegments.Keys
            lSeg = listOfSegments.ItemByKey(lPeakDate)
            If Not lSeg.IsExcluded Then
                lListOfChosenSegments.Add(lPeakDate, lSeg)
                With lSeg
                    If .MeanLogQ > lMeanQLogMaxC Then lMeanQLogMaxC = .MeanLogQ
                    If .MeanLogQ < lMeanQLogMinC Then lMeanQLogMinC = .MeanLogQ

                    If .Coefficient1 > lCoef1Max Then lCoef1Max = .Coefficient1
                    If .Coefficient1 < lCoef1Min Then lCoef1Min = .Coefficient1
                End With
                'Analyse each again here to make sure the parameters are set for summary
                'RecessAnalyse(lSeg)
            End If
        Next

        If lListOfChosenSegments.Count = 0 OrElse lListOfChosenSegments.Count = 1 Then
            If GraphTs IsNot Nothing Then GraphTs.Clear()
            Bulletin = "Unable to perform Recess analysis. " & vbCrLf & "Must select at least two recession segments."
            Exit Sub
        End If

        lListOfChosenSegments.SortByValue() 'sort MeanLogQ in Ascending order
        lMsg = New Text.StringBuilder()
        lMsg.AppendLine("MAXIMUM LOG Q FOR ALL CHOSEN RECESSIONS= " & String.Format("{0:0.00000}", lMeanQLogMaxC).PadLeft(8, " "))
        lMsg.AppendLine("MINIMUM LOG Q FOR ALL CHOSEN RECESSIONS= " & String.Format("{0:0.00000}", lMeanQLogMinC).PadLeft(8, " "))

        Dim lAskUserNumRecToBeEliminated As Integer = listOfSegments.Count - lListOfChosenSegments.Count
        If SaveInterimResults And pHasWritePermission Then
            lSW = New IO.StreamWriter(pFileOut1, FileOut1Created)
            lSW.WriteLine("NUMBER OF RECESSION PERIODS INITIALLY SELECTED=" & lListOfChosenSegments.Count)
            lSW.WriteLine("MAXIMUM LOG Q FOR ALL RECESSIONS=" & XLogQMax)
            lSW.WriteLine("MINIMUM LOG Q FOR ALL RECESSIONS=" & XLogQMin)
            lSW.WriteLine("--------------------------------------------------------------------")
            lSW.WriteLine("       RECESSION PERIODS AFTER SORTING BY LOG Q:")
            lSW.WriteLine("ORIG.                              GRAPHIC OF RECESSION INDEX (K)")
            Dim lStrSlopeMin As String = String.Format("{0:0.0}", lSlopeMn).PadLeft(7, " ")
            Dim lStrSlopeMax As String = String.Format("{0:0.0}", lSlopeMx).PadLeft(7, " ")
            lSW.WriteLine("NUMBER LOG Q     K        ".PadRight(26, " ") & lStrSlopeMin & Space(33) & lStrSlopeMax)
            '   18 FORMAT (A26, 1F7.1, 33X, 1F7.1)

            For Z As Integer = lListOfChosenSegments.Count - 1 To 0 Step -1 'loop 310
                lSeg = lListOfChosenSegments.Item(Z)

                'lPickAR(Z) = 1
                lSlope = -1 * lSeg.Coefficient1
                lDiff = lSlope - lSlopeMn
                Dim lNumBlanks As Integer = Math.Floor(lDiff * 40 / (lSlopeMx - lSlopeMn))

                '   15 FORMAT (1I3, 2F9.3, 9X, 42A1)
                Dim lStrOrigNo As String = "X".PadLeft(3, " ")
                Dim lStrXMN As String = String.Format("{0:0.000}", lSeg.MeanLogQ).PadLeft(9, " ")
                Dim lStrCoef As String = String.Format("{0:0.000}", lSeg.Coefficient1).PadLeft(9, " ")
                Dim lStrBlnk As String = Space(lNumBlanks) & "*"
                lSW.WriteLine(lStrOrigNo & lStrXMN & lStrCoef & lStrBlnk)
            Next 'loop 310

            '-------- SELECT DATA LINES TO BE DELETED BEFORE REGRESSION:  --------

            lSW.WriteLine("BEFORE OBTAINING THE LEAST-SQUARES BEST FIT ")
            lSW.WriteLine("EQUATION FOR K (DELTA T/DELTA LOG Q) VERSUS")
            lSW.WriteLine("LOG Q, THIS NUMBER OF RECESSIONS WAS ELIMINATED:" & lAskUserNumRecToBeEliminated)
            lSW.Flush()
            lSW.Close()
            lSW = Nothing

            If lAskUserNumRecToBeEliminated > 0 Then
                lSW = New IO.StreamWriter(pFileOut2, True)
                lSW.WriteLine("NOTE THAT THESE RECESSIONS, IDENTIFIED BY   ")
                lSW.WriteLine("THEIR ORIGINAL SEQUENTIAL NUMBERS, WERE     ")
                lSW.WriteLine("DELETED FROM ANALYSIS BEFORE DETERMINING   ")
                lSW.WriteLine("BEST-FIT EQUATIONS:")
                'Apparently, this loop is to delete the previously determine number
                ' of recession values from the list
                'ENTER RECESSION TO ELIMINATE (ENTER ITS "ORIGINAL NUMBER")
                'Document excluded recession segment
                For Each lPeakDayDate As String In listOfSegments.Keys
                    If listOfSegments.ItemByKey(lPeakDayDate).IsExcluded Then
                        lSW.WriteLine("             " & lPeakDayDate)
                    End If
                Next
                lSW.Flush()
                lSW.Close()
                lSW = Nothing
            End If
        End If 'has write permission and save interim results

        '----ASSIGN VALUES TO X AND Y TO BE SENT TO THE REGRESSION SUBROUTINE:----
        'lNum = 0
        'For Z As Integer = 1 To lNumRecessPeriods 'loop 340
        '    If lOrigNoAR(Z) = 0 Then Continue For
        '    lNum += 1
        '    lX(lNum) = lXMNArray(Z)
        '    lY(lNum) = lCoefArray(Z)
        '    lOrigNoAR(lNum) = lOrigNoAR(Z)
        'Next 'loop 340
        'lNumRecessPeriods = lNum
        'this lmsg ought to be displayed
        'For Z As Integer = lNumRecessPeriods To 1 Step -1
        '    lMsg &= lX(Z) & "      " & lY(Z) & vbCrLf
        'Next
        'Logger.Dbg(lMsg)

        lMsg.AppendLine("Selected recession periods:")
        lMsg.AppendLine("   PeakDay   MeanLogQ         K")
        For Z As Integer = lListOfChosenSegments.Count - 1 To 0 Step -1
            lSeg = lListOfChosenSegments.Item(Z)
            lMsg.AppendLine(lSeg.PeakDayDateToString & String.Format("{0:0.000}", lSeg.MeanLogQ).PadLeft(11, " ") & String.Format("{0:0.000}", lSeg.Coefficient1).PadLeft(10, " "))
        Next
        'For Each lPeakDayDate As String In lListOfChosenSegments.Keys
        '    lSeg = lListOfChosenSegments.ItemByKey(lPeakDayDate)
        '    lMsg.AppendLine(lPeakDayDate & String.Format("{0:0.000}", lSeg.MeanLogQ).PadLeft(6, " ") & String.Format("{0:0.000}", lSeg.Coefficient1).PadLeft(6, " "))
        'Next

        'Bulletin = lMsg.ToString
        'lMsg.Length = 0

        '-- PERFORM REGRESSION AND WRITE BEST EQUATION FOR K AS FUNCT. OF DISCHARGE
        'Dim lMNLogQC As Double = lX(1)
        'Dim lMXLogQC As Double = lX(lNumRecessPeriods)
        Dim lCoeffA As Double = 0.0
        Dim lCoeffB As Double = 0.0
        Dim lX(lListOfChosenSegments.Count) As Double
        Dim lY(lListOfChosenSegments.Count) As Double
        Dim lKDates(lListOfChosenSegments.Count) As Double
        For Z As Integer = 0 To lListOfChosenSegments.Count - 1
            lSeg = lListOfChosenSegments.Item(Z)
            lX(Z + 1) = lSeg.MeanLogQ
            lY(Z + 1) = lSeg.Coefficient1
            lKDates(Z + 1) = lSeg.PeakDayDate
        Next
        DoRegression2(lX, lY, lListOfChosenSegments.Count, lCoeffA, lCoeffB)
        '---- AFTER INTEGRATION WRITE EQUATION FOR TIME AS FUNCTION OF DISCHARGE:
        'Dim lCoeffC As Double = -0.5 * lCoeffA * lMXLogQC ^ 2 - lCoeffB * lMXLogQC
        Dim lCoeffC As Double = -0.5 * lCoeffA * lMeanQLogMaxC ^ 2 - lCoeffB * lMeanQLogMaxC

        '--------------- SHOW ORDERED DATA, AFTER ELIMINATION:  --------------
        If SaveInterimResults And pHasWritePermission Then
            lSW = New IO.StreamWriter(pFileOut1, FileOut1Created)
            lSW.WriteLine(" ")
            lSW.WriteLine("-----------------------------------------------------------------------")
            lSW.WriteLine("        RECESSION PERIODS LEFT AFTER ELIMINATION: ")
            lSW.WriteLine("ORIG.  ORDERED                      GRAPHIC OF RECESSION INDEX (K)")
            Dim lStrSlopeMin As String = String.Format("{0:0.0}", lSlopeMn).PadLeft(7, " ")
            Dim lStrSlopeMax As String = String.Format("{0:0.0}", lSlopeMx).PadLeft(7, " ")
            lSW.WriteLine("NUMBER  LOG Q    K         " & lStrSlopeMin & Space(33) & lStrSlopeMax)

            For Z As Integer = lListOfChosenSegments.Count - 1 To 0 Step -1 'loop 352
                lSeg = lListOfChosenSegments.Item(Z)
                lSlope = -1.0 * lSeg.Coefficient1 'lY(Z)
                lDiff = lSlope - lSlopeMn
                Dim lNumBlanks As Integer = Math.Floor(lDiff * 40 / (lSlopeMx - lSlopeMn))

                Dim lStrOrigNo As String = lSeg.PeakDayDateToString
                Dim lStrX As String = String.Format("{0:0.000}", lSeg.MeanLogQ).PadLeft(9, " ") ' lX(Z)
                Dim lStrY As String = String.Format("{0:0.000}", lSeg.Coefficient1).PadLeft(9, " ") 'lY(Z)
                Dim lStrBlnk As String = Space(lNumBlanks) & "*" '.PadRight(42, " ")
                lSW.WriteLine(lStrOrigNo & lStrX & lStrY & lStrBlnk)
            Next 'loop 352

            lSW.WriteLine("AMONG THE SELECTED RECESSION PERIODS, THESE ARE THE")
            lSW.WriteLine("MIN AND MAX VALUES OF LOGQ FOR WHICH K (DAYS PER ")
            lSW.WriteLine("LOGQC WAS CALCULATED:" & lMeanQLogMinC & "   " & lMeanQLogMaxC)

            lSW.WriteLine(" ")

            lSW.WriteLine("--------------------------------------------------------------------")
            lSW.WriteLine("BEST-FIT LINEAR EQUATION FOR K VS. LOG Q:")
            Dim lStrCoeffA As String = String.Format("{0:0.00}", lCoeffA).PadLeft(8, " ")
            Dim lStrCoeffB As String = String.Format("{0:0.00}", lCoeffB).PadLeft(8, " ")
            lSW.WriteLine("DELTA T/DELTA LOGQ = (" & lStrCoeffA & " * LOGQ ) + " & lStrCoeffB)
            lSW.WriteLine("  RESULTS OF THIS EQUATION:")
            lSW.WriteLine("             ")
            lSW.WriteLine("                                    GRAPHIC OF RECESSION INDEX (K)")
            lStrSlopeMin = String.Format("{0:0.0}", lSlopeMn).PadLeft(7, " ")
            lStrSlopeMax = String.Format("{0:0.0}", lSlopeMx).PadLeft(7, " ")
            lSW.WriteLine("      LOG Q               " & lStrSlopeMin & Space(33) & lStrSlopeMax)

            Dim lXLogQ As Double = lMeanQLogMaxC 'lMXLogQC
            While True 'loop 370
                lSlope = -1 * lCoeffA * lXLogQ - 1 * lCoeffB
                lDiff = lSlope - lSlopeMn
                Dim lNumBlanks As Integer = Math.Floor(lDiff * 40 / (lSlopeMx - lSlopeMn))
                If lNumBlanks < 0 Then
                    lSW.WriteLine(lXLogQ & "   " & lSlope & "    ")
                Else
                    lSW.WriteLine(lXLogQ & "   " & lSlope & Space(lNumBlanks) & "*")
                End If
                lXLogQ -= 0.05
                If lXLogQ > lMeanQLogMinC Then 'lMNLogQC
                    Continue While
                Else
                    Exit While
                End If
            End While 'loop 370

            ''---- AFTER INTEGRATION WRITE EQUATION FOR TIME AS FUNCTION OF DISCHARGE:
            ''Dim lCoeffC As Double = -0.5 * lCoeffA * lMXLogQC ^ 2 - lCoeffB * lMXLogQC
            'Dim lCoeffC As Double = -0.5 * lCoeffA * lQLogMax ^ 2 - lCoeffB * lQLogMax
            lSW.WriteLine(" ")
            lSW.WriteLine("---------------------------------------------------------------------")
            lSW.WriteLine("AFTER INTEGRATION, THE FOLLOWING EQUATION IS ")
            lSW.WriteLine("OBTAINED. IT GIVES TIME (IN DAYS) AS A FUNCTION")
            lSW.WriteLine("OF LOG Q. INITIAL CONDITION IS T=0 AT LOG Q= THE")
            lSW.WriteLine("MAXIMUM LOG Q FOR WHICH A VALUE OF K WAS ")
            lSW.WriteLine("CALCULATED.")
            lStrCoeffA = String.Format("{0:0.00}", lCoeffA / 2.0).PadLeft(8, " ")
            lStrCoeffB = String.Format("{0:0.00}", lCoeffB).PadLeft(8, " ")
            Dim lStrCoeffC As String = String.Format("{0:0.00}", lCoeffC).PadLeft(8, " ")
            lSW.WriteLine(" T =" & lStrCoeffA & " * LOGQ^2 + " & lStrCoeffB & " * LOGQ + " & lStrCoeffC)
            lSW.WriteLine("         RESULTS OF THIS EQUATION:")
            lSW.WriteLine("                                                GRAPHIC OF TIME:")
            Dim lTimeMax As Double = 0.5 * lCoeffA * XLogQMin ^ 2 + lCoeffB * XLogQMin + lCoeffC 'overall min Log Q
            lSW.WriteLine("    TIME(D)         LOG Q           Q       0.0 ")
            lXLogQ = lMeanQLogMaxC 'lMXLogQC
            While True 'loop 380
                Dim lT As Double = 0.5 * lCoeffA * lXLogQ ^ 2 + lCoeffB * lXLogQ + lCoeffC
                Dim lXQ As Double = 10 ^ lXLogQ
                Dim lNumBlanks As Integer = Math.Floor(lT * 25 / lTimeMax)
                Dim lBlankStrNStar As String = ""
                If lNumBlanks > 0 Then
                    lBlankStrNStar = Space(lNumBlanks) & "*"
                End If
                lSW.WriteLine(lT & " " & lXLogQ & " " & lXQ & " " & lBlankStrNStar)
                lXLogQ -= 0.05
                If lXLogQ > lMeanQLogMinC Then 'lMNLogQC
                    Continue While
                Else
                    Exit While
                End If
            End While 'loop 380

            lSW.WriteLine("--------------------------------------------------------------------")
            lSW.Flush()
            lSW.Close()
            lSW = Nothing
        End If

        ''------------   DETERMINE MAX, MIN, AND MEDIAN K (aka Coefficient1): -----------------------
        Dim lK(lListOfChosenSegments.Count) As Double
        For Z As Integer = 1 To lListOfChosenSegments.Count
            lK(Z) = lY(Z) * -1
        Next
        Order(lListOfChosenSegments.Count, lK)
        Dim lKMax As Double = lK(lListOfChosenSegments.Count)
        Dim lKMin As Double = lK(1)
        Dim liDown As Integer = 0
        Dim liUp As Integer = lListOfChosenSegments.Count + 1
        Dim liCnt As Integer = 0
        Dim lKMed As Double
        While True 'loop 395
            liCnt += 1
            If liCnt > MaxNumRecPeriods Then
                Logger.Dbg("PROBLEMS WITH DETERMINATION OF MEDIAN")
                'lMsg.Length = 0
                lMsg.AppendLine("PROBLEMS WITH DETERMINATION OF MEDIAN")

                Bulletin = lMsg.ToString
                Exit Sub
                'Exit While
            End If
            If liUp - liDown = 2 Then
                lKMed = lK(Math.Floor((liUp + liDown) / 2))
                Exit While
            ElseIf liUp - liDown = 1 Then
                lKMed = (lK(liUp) + lK(liDown)) / 2
                Exit While
            Else
                liDown += 1
                liUp -= 1
            End If
        End While 'loop 395
        ReDim lK(0)

        'construct graph TS for just the coefficients
        If GraphTs IsNot Nothing Then GraphTs.Clear()
        GraphTs = New atcTimeseries(Nothing)
        GraphTs.Dates = New atcTimeseries(Nothing)
        Order(lListOfChosenSegments.Count, lKDates, lY)
        GraphTs.Dates.Values = lKDates
        GraphTs.Values = lY
        GraphTs = GraphTs * -1
        With GraphTs
            .Value(0) = GetNaN()
            .Dates.Value(0) = .Dates.Value(1) - JulianHour * 24.0
            .Attributes.SetValue("YAxis", "LEFT")
            .Attributes.SetValue("point", True)
            .Attributes.SetValue("Constituent", "")
            .Attributes.SetValue("Scenario", "")
            .Attributes.SetValue("Units", "K")
        End With

        ''------------------- WRITE RAW RECESSION DATA TO "y-file"  ----------------
        If pHasWritePermission And SaveInterimResults Then
            lSW = New IO.StreamWriter(pFileOut2, True)
            lSW.WriteLine("----------------------------------------------------------------------")
            lSW.WriteLine("     Tpeak            Tmrc          LogQ           Q               Seq#  ")

            Dim lStrYY As String = ""
            Dim lStrZZ As String = ""
            Dim lStrXX As String = ""
            Dim lStr10ExpXX As String = ""
            Dim lStrIndicator As String = ""
            Dim lZZ As Double
            lDiff = 0
            For Z As Integer = listOfSegments.Count - 1 To 0 Step -1
                lSeg = listOfSegments.Item(Z)
                With lSeg
                    If .IsExcluded Then Continue For
                    For D As Integer = .MaxDayOrdinal To .MinDayOrdinal Step -1
                        If .QLog(D) = 0 And D = 0 Then
                            lSW.WriteLine("   ")
                            Dim lT As Double = 0.5 * lCoeffA * .QLog(D) ^ 2 + lCoeffB * .QLog(D) + lCoeffC
                            lDiff = lT - D
                            lZZ = lT
                            '   31 format (4f14.6, 1i14)
                            lStrYY = String.Format("{0:0.000000}", D).PadLeft(14, " ")
                            lStrZZ = String.Format("{0:0.000000}", lT).PadLeft(14, " ")
                            lStrXX = String.Format("{0:0.000000}", .QLog(D)).PadLeft(14, " ")
                            lStr10ExpXX = String.Format("{0:0.000000}", 10 ^ .QLog(D)).PadLeft(14, " ")
                            lStrIndicator = .PeakDayDateToString.PadLeft(14, " ")
                            lSW.WriteLine(lStrYY & lStrZZ & lStrXX & lStr10ExpXX & lStrIndicator)
                            Exit For
                        Else
                            lZZ = lDiff + D 'lYY(II)
                            lStrYY = String.Format("{0:0.000000}", D).PadLeft(14, " ")
                            lStrZZ = String.Format("{0:0.000000}", lZZ).PadLeft(14, " ")
                            lStrXX = String.Format("{0:0.000000}", .QLog(D)).PadLeft(14, " ")
                            lStr10ExpXX = String.Format("{0:0.000000}", 10 ^ .QLog(D)).PadLeft(14, " ")
                            lStrIndicator = .PeakDayDateToString.PadLeft(14, " ")
                            lSW.WriteLine(lStrYY & lStrZZ & lStrXX & lStr10ExpXX & lStrIndicator)
                        End If
                    Next
                End With
            Next
            lSW.Flush()
            lSW.Close()
            lSW = Nothing

            ' '
            'If you have executed this program simply to obtain'
            'a median recession index (for use in program RORA)'
            'then you might elect not to write additional data '
            'describing the master recession curve to file     '
            'RECSUM.txt: .............                         '
            '                                                  '
            '  To only write the median recession index (to file'
            '     "index.txt"), enter 1 '
            '  To write more results, including information about'
            '    the master recession curve to file "recsum.txt" '
            '    (in addition to "index.txt"), enter 2 
            Dim lAskUserRecIndexOnly As Integer = 2

            Dim lStrInputFile As String = InputFilename11(pDataFilename)

            If lAskUserRecIndexOnly <> 1 Then
                lSW = New IO.StreamWriter(pFileRecSum, True)
                '   17 FORMAT (A12,A1,1X,1I4,'-',1I4,1I3,3F6.1,2F8.3,1F9.4,2F10.4)
                Dim lDate(5) As Integer
                J2Date(FlowData.Dates.Value(0), lDate)
                Dim lStrYearStart As String = lDate(0).ToString
                J2Date(FlowData.Dates.Value(FlowData.numValues - 1), lDate)
                Dim lStrYearEnd As String = lDate(0).ToString
                Dim lStrDuration As String = lStrYearStart & "-" & lStrYearEnd & "  "

                Dim lStrKMin As String = String.Format("{0:0.0}", lKMin).PadLeft(6, " ")
                Dim lStrKMed As String = String.Format("{0:0.0}", lKMed).PadLeft(6, " ")
                Dim lStrKMax As String = String.Format("{0:0.0}", lKMax).PadLeft(6, " ")
                Dim lStrMNLogQC As String = String.Format("{0:0.000}", lMeanQLogMinC).PadLeft(8, " ") 'lMNLogQC
                Dim lStrMXLogQC As String = String.Format("{0:0.000}", lMeanQLogMaxC).PadLeft(8, " ") 'lMXLogQC
                Dim lStrCoeffA As String = String.Format("{0:0.0000}", 0.5 * lCoeffA).PadLeft(9, " ")
                Dim lStrCoeffB As String = String.Format("{0:0.0000}", lCoeffB).PadLeft(10, " ")
                Dim lStrCoeffC As String = String.Format("{0:0.0000}", lCoeffC).PadLeft(10, " ")

                lSW.WriteLine(lStrInputFile & SeasonLabel & " " & lStrDuration & lListOfChosenSegments.Count & _
                              lStrKMin & lStrKMed & lStrKMax & lStrMNLogQC & lStrMXLogQC & lStrCoeffA & lStrCoeffB & lStrCoeffC)
                lSW.Flush()
                lSW.Close()
                lSW = Nothing
            End If

            'here is the final result

            Bulletin = lMsg.ToString
            lSW = New IO.StreamWriter(pFileIndex, True)
            '   13 format (A12,1f8.2)
            lSW.WriteLine(lStrInputFile.PadRight(12, " ") & String.Format("{0:0.00}", lKMed).PadLeft(8, " "))
            lSW.Flush()
            lSW.Close()
            lSW = Nothing
        End If
        pRecessionIndex = lKMed
        'here is the final result
        lMsg.AppendLine(vbCrLf & "Final median recession index: " & String.Format("{0:0.00}", lKMed).PadLeft(8, " "))
        Bulletin = lMsg.ToString
    End Sub

    Public Shared Function InputFilename11(ByVal aFilename As String) As String

        Dim lFilenameLength As Integer = 11
        If aFilename.Length > lFilenameLength Then
            If aFilename.StartsWith("NWIS_discharge_") Then aFilename = aFilename.Substring(15)
        End If
        If aFilename.Length > lFilenameLength Then
            aFilename = aFilename.Substring(0, lFilenameLength)
        End If
        Return aFilename.PadRight(12, " ")

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
        'Dim liEnd As Integer = liStart + aiDisplayLastDay - 1
        Dim liEnd As Integer = aiDisplayLastDay
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
        'lStr.AppendLine("   PEAK   LOG Q    LOG Q      Q        START   YEAR    MONTH    DAY")
        lStr.AppendLine("   PEAK   LOG Q    LOG Q      Q        START   Date")
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
                'Dim lFlowStr As String = String.Format("{0:0.0000}", aFlow(I)).ToString.PadLeft(10, " ")
                Dim lFlowStr As String = String.Format("{0:0.000}", aFlow(I)).ToString.PadLeft(9, " ")
                Dim liCountPeakStr As String = (I + aiPeak).ToString.PadLeft(8, " ")
                J2Date(aDates(I), lDate)
                Dim lYearStr As String = lDate(0).ToString.PadLeft(7, " ")
                'Dim lMonStr As String = lDate(1).ToString.PadLeft(8, " ")
                'Dim lDayStr As String = lDate(2).ToString.PadLeft(8, " ")
                Dim lMonStr As String = lDate(1).ToString.PadLeft(2, " ")
                Dim lDayStr As String = lDate(2).ToString.PadLeft(2, " ")
                Dim lDateStr As String = lYearStr & "/" & lMonStr & "/" & lDayStr
                'lStr.AppendLine(liCountStr & lQLogStr & lDelQLogStr & lFlowStr & liCountPeakStr & lYearStr & lMonStr & lDayStr)
                lStr.AppendLine(liCountStr & lQLogStr & lDelQLogStr & lFlowStr & liCountPeakStr & lDateStr)
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
    Public Shared Sub DoRegression2(ByVal aSegment As clsRecessionSegment)
        Dim lA(2, 4) As Double
        Dim lRoot1 As Double = 0.0
        Dim lRoot2 As Double = 0.0
        Dim lQLog() As Double
        Dim lOrdinal() As Integer
        Dim lNewRecessLength As Integer
        With aSegment
            lNewRecessLength = .MaxDayOrdinal - .MinDayOrdinal + 1
            ReDim lQLog(lNewRecessLength)
            ReDim lOrdinal(lNewRecessLength)
            For Z As Integer = .MinDayOrdinal To .MaxDayOrdinal
                lQLog(Z - .MinDayOrdinal + 1) = .QLog(Z)
                lOrdinal(Z - .MinDayOrdinal + 1) = Z 'actual ordinal number, NOT the rescaled ordinals
            Next
        End With
        For I As Integer = 1 To lNewRecessLength
            lA(1, 1) += lQLog(I) ^ 2
            lA(1, 2) += lQLog(I)
            lA(2, 1) += lQLog(I)
            lRoot1 += lQLog(I) * lOrdinal(I)
            lRoot2 += lOrdinal(I)
        Next

        lA(2, 2) = lNewRecessLength
        Dim lN As Integer = 2

        Inverse(lA, lN)

        aSegment.Coefficient1 = lA(1, 1) * lRoot1 + lA(1, 2) * lRoot2
        aSegment.Coefficient2 = lA(2, 1) * lRoot1 + lA(2, 2) * lRoot2

    End Sub

    ' -----THIS SUBROUTINE PERFORMS LEAST-SQUARES REGRESSION TO FIND BEST-FIT ---
    ' ---------------- EQUATION OF LINEAR BASIS ( Y = A*X + B ) -----------------
    Private Sub DoRegression2(ByVal aX() As Double, ByVal aY() As Double, ByVal aNumOfDaysInPeriod As Integer, ByRef aCoeff1 As Double, ByRef aCoeff2 As Double)
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

    '  INPUT IS 3 LISTS OF NUMBERS. ALL LISTS are the same length
    '  OUTPUT IS SAME, IN ASCENDING ORDER, SORTED BY VALUES IN LIST1.
    'LIST1 e.g XMean
    'LIST2 e.g Coefficient1
    'LIST3 e.g Original ordinal number of each segment
    Private Sub Order(ByVal aNumOfRecessPeriods As Integer, _
                      ByRef aList1() As Double, _
             Optional ByRef aList2() As Double = Nothing, _
             Optional ByRef aList3() As Integer = Nothing)
        Dim lSwapped As Boolean
        Dim lPasses As Integer = 0
        Dim lTempValue As Double
        While True
            lSwapped = False
            lPasses += 1
            Dim I As Integer = 1
            While True
                If aList1(I) > aList1(I + 1) Then
                    lTempValue = aList1(I + 1)
                    aList1(I + 1) = aList1(I)
                    aList1(I) = lTempValue

                    If aList2 IsNot Nothing Then
                        lTempValue = aList2(I + 1)
                        aList2(I + 1) = aList2(I)
                        aList2(I) = lTempValue
                    End If

                    If aList3 IsNot Nothing Then
                        lTempValue = aList3(I + 1)
                        aList3(I + 1) = aList3(I)
                        aList3(I) = lTempValue
                    End If

                    lSwapped = True
                End If
                I += 1
                If I <= aNumOfRecessPeriods - lPasses Then
                    Continue While
                Else
                    Exit While
                End If
            End While
            If lSwapped Then
                Continue While
            Else
                Exit While
            End If
        End While
    End Sub
End Class
