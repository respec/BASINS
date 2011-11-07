Imports atcData
Imports atcUtility
Imports MapWinUtility
Imports atcTimeseriesRDB
Imports atcGraph
Imports ZedGraph

Imports System.Windows.Forms
Imports System.Text.RegularExpressions

Public Class frmUSGSStreamFlowAnalysis

    'Form object that contains graph(s)
    Private pMaster As ZedGraph.MasterPane
    Private pAuxEnabled As Boolean = False
    Public AuxFraction As Single = 0.2
    Private WithEvents pZgc As ZedGraphControl
    Private pGrapher As clsGraphBase
    Private Shared SaveImageExtension As String = ".png"

    Public pDataGroup As atcTimeseriesGroup
    Private pGraphRecessDatagroup As atcTimeseriesGroup

    Private pRecess As clsRecess

    Public Sub Initialize(Optional ByVal aTimeseriesGroup As atcData.atcTimeseriesGroup = Nothing, _
                      Optional ByVal aBasicAttributes As Generic.List(Of String) = Nothing, _
                      Optional ByVal aShowForm As Boolean = True)
        pDataGroup = aTimeseriesGroup
        pGraphRecessDatagroup = New atcTimeseriesGroup()
        pRecess = New clsRecess()
        SetStyle(ControlStyles.DoubleBuffer Or ControlStyles.UserPaint Or ControlStyles.AllPaintingInWmPaint, True)
        InitMasterPane()
        Me.Show()
    End Sub

    Public Property Grapher() As clsGraphBase
        Get
            Return pGrapher
        End Get
        Set(ByVal newValue As clsGraphBase)
            pGrapher = newValue
            RefreshGraph()
        End Set
    End Property

    Private Sub InitMasterPane()
        pZgc = CreateZgc()
        Me.Controls.Add(pZgc)
        scDisplay.Panel2.Controls.Add(pZgc)
        With pZgc
            .Dock = System.Windows.Forms.DockStyle.Fill
            '.IsEnableHZoom = mnuViewHorizontalZoom.Checked
            '.IsEnableHPan = mnuViewHorizontalZoom.Checked
            '.IsEnableVZoom = mnuViewVerticalZoom.Checked
            '.IsEnableVPan = mnuViewVerticalZoom.Checked
            '.IsZoomOnMouseCenter = mnuViewZoomMouse.Checked
            pMaster = .MasterPane

        End With

        RefreshGraph()
    End Sub

    Public Sub RefreshGraph()
        pZgc.AxisChange()
        Invalidate()
        Refresh()
    End Sub

    Public Sub Recess()

        Dim lTsSF As atcTimeseries = pDataGroup(0)
        Dim lYearStart As Integer = 1971
        Dim lYearEnd As Integer = 1972
        Dim lStartDate As Double = Date2J(1971, 1, 1, 0, 0, 0)
        Dim lEndDate As Double = Date2J(1972, 12, 31, 24, 0, 0)

        lTsSF = SubsetByDate(lTsSF, lStartDate, lEndDate, Nothing)

        Dim lOutputPath As String = IO.Path.GetDirectoryName(lTsSF.Attributes.GetValue("history 1").substring("read from ".Length)).Trim()
        Dim lFileRecSum As String = "recsum.txt"
        Dim lFileIndex As String = "index.txt"
        Dim lFileRecData As String = "recdata.txt"
        Dim lFileRanges As String = "ranges.txt"
        Dim lFileOut1 As String = ""
        Dim lFileOut2 As String = ""

        lFileRecSum = IO.Path.Combine(lOutputPath, lFileRecSum)
        lFileRecData = IO.Path.Combine(lOutputPath, lFileRecData)
        lFileIndex = IO.Path.Combine(lOutputPath, lFileIndex)
        lFileRanges = IO.Path.Combine(lOutputPath, lFileRanges)

        Dim lMsg As String = ""
        Dim lInputfile As String = IO.Path.GetFileName(lTsSF.Attributes.GetValue("history 1").substring("read from ".Length))

        Dim pQMIN As Double = 0.0
        If (pQMIN < 0.001) Then
            'FOR THIS STATION, THE MINIMUM DISCHARGE IS ZERO.'
            'TO AVOID PROBLEMS WITH THE LOG FUNCTION, THE    '
            'PROGRAM WILL SHOW A MINIMUM DISCHARGE ON GRAPHS '
            'OF 1.0 CFS ( LOG Q = 0 ).  THIS CAN BE CHANGED  '
            'DURING INTERACTIVE DISPLAY IF THE USER WANTS.   '
            pQMIN = 1.0
        End If

        'User can select all 12 months of a year or 
        'only a sub set of months

        Dim lPickMonths As New ArrayList
        lPickMonths.Add(7) : lPickMonths.Add(8) : lPickMonths.Add(9)
        Dim lStrMonths As String = ""
        For Each lMon As Integer In lPickMonths
            lStrMonths &= lMon & ", "
        Next
        lStrMonths = lStrMonths.TrimEnd(" ")
        lStrMonths = lStrMonths.TrimEnd(",")

        'w=WINTER, v=SPRING, s=SUMMER, f=FALL
        'n=NO PARTICULAR SEASON
        Dim lAskUserSeason As String = "s"

        'IN ORDER THAT THE PROGRAM WILL DETECT A RECESSION '
        'SEGMENT, HOW MANY DAYS OF RECESSION ARE REQUIRED? 
        Dim lAskUseriFarcri As Integer = 5

        lFileOut1 = IO.Path.Combine(lOutputPath, "x" & lAskUserSeason & "Indian" & ".txt")
        lFileOut2 = IO.Path.Combine(lOutputPath, "y" & lAskUserSeason & "Indian" & ".txt")

        Dim lHeaderOutFile1 As String = ""
        lHeaderOutFile1 &= _
      " FILE " & lFileOut1 & "--  UNIT 10 OUTPUT OF RECESS.F " & vbCrLf & _
      " INPUT FILE = " & lInputfile & vbCrLf & _
      " START = " & lYearStart & vbCrLf & _
      " END =   " & lYearEnd & vbCrLf & _
      " DAYS OF RECESSION REQUIRED FOR DETECTION=" & lAskUseriFarcri.ToString & vbCrLf & _
      " MONTHS SELECTED:" & lStrMonths & vbCrLf & _
      " " & vbCrLf & _
      "-----------------------------------------------------------------------" & vbCrLf & _
      "              RECESSION PERIODS INITIALLY SELECTED: " & vbCrLf & _
      "   LOG Q       RECESS.INDEX     TIME SINCE PEAK    .       DATE OF PEAK " & vbCrLf & _
      "   (MEAN)    ( -dT/d(LogQ) ) (START)(MIDDLE)(END)  .        (yr, mo, d) " & vbCrLf

        Dim lHeaderOutFile2 As String = ""
        lHeaderOutFile2 &= _
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
        Dim liCount As Integer = 1
        Dim liPeak As Integer = 0
        Dim lOK As Integer = 0
        Dim lDate(5) As Integer
        Dim lNumRecessPeriods As Integer 'originally NMRECES
        Dim lDiff As Double = 0.0
        Dim lNumBlanks As Integer = 0
        Dim lNum As Integer
        Dim II As Integer = 1
        Dim lMaxNumDaysInAllRecPeriods As Integer = 3000
        Dim lIndicator(lMaxNumDaysInAllRecPeriods) As Integer
        Dim lXX(lMaxNumDaysInAllRecPeriods) As Double
        lXX(1) = -99
        Dim lYY(lMaxNumDaysInAllRecPeriods) As Double
        lYY(1) = -99
        Dim lZZ(lMaxNumDaysInAllRecPeriods) As Double
        Dim lMaxNumRecPeriods As Integer = 50
        Dim lX(lMaxNumRecPeriods) As Double
        Dim lY(lMaxNumRecPeriods) As Double
        Dim lK(lMaxNumRecPeriods) As Double
        Dim lDUMMY(lMaxNumRecPeriods) As Double

        Dim lXMeanAR(lMaxNumRecPeriods) As Double
        Dim lYMeanAR(lMaxNumRecPeriods) As Double
        Dim lCoef1AR(lMaxNumRecPeriods) As Double
        Dim lCoef2AR(lMaxNumRecPeriods) As Double
        Dim lMinAR(lMaxNumRecPeriods) As Double
        Dim lMaxAR(lMaxNumRecPeriods) As Double
        Dim lPickAR(lMaxNumRecPeriods) As Integer
        Dim lOrigNoAR(lMaxNumRecPeriods) As Integer
        Dim lDatesAR(lMaxNumRecPeriods) As Double
        Dim lXMNArray(lMaxNumRecPeriods) As Double
        Dim lCoefArray(lMaxNumRecPeriods) As Double

        Dim lQLogMax As Double = 0.0
        Dim lQLogMin As Double = 10.0

        For liCount = 2 To lTsSF.numValues 'original loop 200
            lOK = 0
            J2Date(lTsSF.Dates.Value(liCount - 1), lDate)
            If Not lPickMonths.Contains(lDate(1)) Then
                Continue For 'loop 200
            End If
            Dim lCurrentValue As Double = lTsSF.Value(liCount)
            If lCurrentValue <= lTsSF.Value(liCount - 1) Or lCurrentValue <= lTsSF.Value(liCount + 1) Then
                Continue For 'loop 200
            Else
                liPeak = liCount
                lOK = 1
            End If

            '-------------- ANALYZE THE RECESSION AFTER THE PEAK: -----------------
            Dim liHowFar As Integer
            While True 'loop 210
                liCount += 1
                If liCount > lTsSF.numValues Then Exit For 'loop 200
                If Math.Floor(100 * lTsSF.Value(liCount)) > Math.Floor(100 * lTsSF.Value(liCount - 1)) Then lOK = 0
                liHowFar = liCount - liPeak - 1
                If lOK = 1 Then
                    Continue While 'loop 210
                Else
                    Exit While 'loop 210
                End If
            End While 'loop 210
            If liHowFar < lAskUseriFarcri And lOK = 0 Then
                liCount -= 1
                Continue For 'loop 200
            End If
            If liHowFar >= lAskUseriFarcri And lOK = 0 Then 'This is a long if branch
                Dim lFlow(60) As Double
                Dim lQLog(60) As Double
                Dim lDates(60) As Double
                For I As Integer = 1 To 60 'loop 215
                    lFlow(I) = 0.0
                    lQLog(I) = -99.9
                Next 'loop 215
                lNum = liCount - liPeak - 1
                If lNum > 60 Then
                    lNum = 60
                    liCount = liPeak + 60
                End If
                Dim liMin As Integer = 1
                Dim liMax As Integer = lNum
                For I As Integer = 1 To lNum 'loop 220
                    lFlow(I) = lTsSF.Value(I + liPeak)
                    If lFlow(I) = 0.0 Then
                        lQLog(I) = -88.8
                    Else
                        lQLog(I) = Math.Log10(lFlow(I))
                    End If
                    lDates(I) = lTsSF.Dates.Value(I + liPeak - 1)
                Next 'loop 220

                J2Date(lTsSF.Dates.Value(liPeak - 1), lDate)
                Logger.Dbg("Number of recession periods used so far = " & lNumRecessPeriods)
                Logger.Dbg("Date of New Peak = " & lDate(0) & "/" & lDate(1) & "/" & lDate(2))
                Logger.Dbg("Period of subsequent recession (days) = " & lNum)

                'Whole bunch of user prompts
                lSW = New IO.StreamWriter(lFileRecData, True) 'keep appending to the file
                lSW.WriteLine(1 + liMax - liMin)
                For I As Integer = liMin To liMax 'loop 231
                    Dim lYGraph As Double = lQLog(I)
                    If lYGraph < -80 Then lYGraph = -2
                    lSW.WriteLine(I.ToString.PadLeft(10, " ") & String.Format("{0:0.00000}", lYGraph).ToString.PadLeft(15, " "))
                Next 'loop 231
                lSW.Close()
                lSW = Nothing

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
                Dim lSR As New IO.StreamReader(lFileRanges)
                Dim liDisplayLastDay As Integer
                Dim lXLogQMin As Double
                Dim lXLogQMax As Double
                Dim Line As String = lSR.ReadLine() 'title line
                Line = lSR.ReadLine().Trim() '(Last day plotted)
                Dim lArr() As String = Regex.Split(Line, "\s+")
                liDisplayLastDay = CInt(lArr(0))
                Line = lSR.ReadLine().Trim() '(Minimum value of LogQ plotted)
                lArr = Regex.Split(Line, "\s+")
                lXLogQMin = CDbl(lArr(0))
                Line = lSR.ReadLine().Trim() '(Maximum value of LogQ plotted)
                lArr = Regex.Split(Line, "\s+")
                lXLogQMax = CDbl(lArr(0))
                lSR.Close()
                lSR = Nothing

                'This pickstartingday thing can be a user interface element
                Dim liPickStartingDay As Integer = 1 'the only valid choices are 1, 11, 21
                Dim liPickInterval As Integer = 1 'the only valid choices: ENTER TIME INTERVAL (1=EVERY DAY, 2=EVERY OTHER DAY)'

                If lPick = "t" Or lPick = "t2" Then
                    'This branch only for display tabular recession data so far
                    'doesn't involve any real calculation, needs to be factored out
                    Dim lThisTable As String = ""
                    If liPickStartingDay <> 1 And liPickStartingDay <> 11 And liPickStartingDay <> 21 Then
                        'problem, can't do TableRecess
                    Else
                        'can save the table entries into some file
                        lThisTable = TableRecess(lQLog, lFlow, lDates, liPeak, liMin, liMax, liPickStartingDay, liDisplayLastDay)
                    End If
                    lThisTable &= "                                              " & vbCrLf
                    lThisTable &= "THIS --- INDICATES A DAY OUTSIDE THE PERIOD OF RECESSION, OR A DAY OUTSIDE " & vbCrLf
                    lThisTable &= "OF THE SEGMENT THAT HAS BEEN SELECTED." & vbCrLf
                    lSW = New IO.StreamWriter(IO.Path.Combine(lOutputPath, "Table.txt"), False)
                    lSW.WriteLine(lThisTable)
                    lSW.Flush() : lSW.Close() : lSW = Nothing
                    'Go To 230 marker to display again
                ElseIf lPick = "g" Or lPick = "g2" Then
                    'This branch only for display graph recession data so far
                    'doesn't involve any real calculation, needs to be factored out

                    Dim lThisGraph As String = ""
                    If liPickInterval <> 1 And liPickInterval <> 2 Then
                        'problem, can't do GraphRecess
                    Else
                        lThisGraph = GraphRecess(lQLog, lFlow, lDates, liPeak, liMin, liMax, lXLogQMin, lXLogQMax, liPickStartingDay, liPickInterval, liDisplayLastDay)
                    End If
                    lThisGraph &= vbCrLf & "                                              " & vbCrLf
                    lThisGraph &= "THIS * REPRESENTS FLOW.  THIS --- INDICATES A DAY OUTSIDE THE PERIOD  " & vbCrLf
                    lThisGraph &= "OF RECESSION, OR A DAY OUTSIDE OF THE SEGMENT THAT HAS BEEN SELECTED. " & vbCrLf
                    lThisGraph &= "THIS ::::: INDICATES FLOW OUTSIDE OF PLOTTING RANGE.                  "
                    lSW = New IO.StreamWriter(IO.Path.Combine(lOutputPath, "Graph.txt"), False)
                    lSW.WriteLine(lThisGraph)
                    lSW.Flush() : lSW.Close() : lSW = Nothing

                    'Go To 230 marker to display again
                ElseIf lPick = "c" Then
                    Dim liAskUserFirstDayofSegment As Integer = 2 'ENTER THE FIRST DAY OF THE SEGMENT *********** enter a NUMBER ******'
                    Dim liAskUserLastDayofSegment As Integer = 8 'ENTER THE LAST DAY OF THE SEGMENT ************ enter a NUMBER ******'
                    liMin = liAskUserFirstDayofSegment
                    liMax = liAskUserLastDayofSegment
                    'Go To 230 marker to display again
                ElseIf lPick = "b" Then
                    liMin = 1
                    liMax = liCount - liPeak - 1
                    'Go To 230 marker to display again

                ElseIf lPick = "a" Then
                    liCount -= 1
                    'Go To very beginning of loop 200
                    Continue For 'loop 200
                ElseIf lPick = "q" Then
                    Exit For
                ElseIf lPick = "r" Then
                    If lQLog(liMin) = lQLog(liMax) Then
                        liCount -= 1
                        lMsg = ""
                        lMsg &= "THE PROGRAM SKIPPED THIS RECESSION" & vbCrLf
                        lMsg &= "PERIOD BECAUSE FLOW DID NOT CHANGE" & vbCrLf
                        lMsg &= "(RECESSION INDEX UNDEFINED)."
                        Logger.Dbg(lMsg)
                        Continue For 'loop 200
                    End If
                    If liMax - liMin > 49 Then
                        lMsg = ""
                        lMsg &= "THE NUMBER OF DAYS SELECTED SHOULD BE" & vbCrLf
                        lMsg &= "LESS THAN " & (lMaxNumRecPeriods + 1).ToString & " BEFORE PICKING OPTION r"
                        Logger.Dbg(lMsg)
                        'Go To 230 marker to display again
                    End If

                    lNumRecessPeriods += 1
                    If lNumRecessPeriods > lMaxNumRecPeriods Then
                        lMsg = ""
                        lMsg &= "YOU HAVE ANALYZED THE MAXIMUM NUMBER OF RECESSION PERIODS."
                        lNumRecessPeriods -= 1
                        Exit For 'loop 200
                    End If

                    Dim I As Integer = 0
                    For IT As Integer = liMin To liMax 'loop 240
                        I += 1
                        II += 1
                        If II > lMaxNumDaysInAllRecPeriods Then
                            lMsg = ""
                            lMsg &= "THE TOTAL NUMBER OF DAYS IN ALL" & vbCrLf
                            lMsg &= "SELECTED RECESSION PERIODS EXCEEDS" & vbCrLf
                            lMsg &= "THE LIMIT OF " & lMaxNumDaysInAllRecPeriods & "."
                            Logger.Dbg(lMsg)
                            Exit For 'loop 200
                        End If
                        lIndicator(II) = lNumRecessPeriods
                        lX(I) = lQLog(IT)
                        lY(I) = IT
                        lXX(II) = lQLog(IT)
                        lYY(II) = IT
                        If lQLog(IT) > lQLogMax Then lQLogMax = lQLog(IT)
                        If lQLog(IT) < lQLogMin Then lQLogMin = lQLog(IT)
                    Next 'loop 240

                    II += 1
                    lXX(II) = 0.0
                    lYY(II) = 0.0
                    Dim lXTotal As Double = 0
                    Dim lYTotal As Double = 0
                    For P As Integer = 1 To I 'loop 245
                        lXTotal += lX(P)
                        lYTotal += lY(P)
                    Next 'loop 245

                    Dim lXMean As Double = lXTotal / I
                    Dim lYMean As Double = lYTotal / I

                    Dim lCoeff1 As Double = 0
                    Dim lCoeff2 As Double = 0

                    Dim lResults As String = ""
                    lResults &= "     DAYS         LOG Q                  " & vbCrLf
                    lResults &= "   ( Y(I) )      ( X(I) )               I" & vbCrLf
                    For P As Integer = 1 To I
                        lResults &= lY(P).ToString.PadLeft(9, " ") & String.Format("{0:0.000000}", lX(P)).PadLeft(15, " ") & P.ToString.PadLeft(10, " ") & vbCrLf
                    Next
                    lResults &= vbCrLf
                    DoRegression2(lX, lY, I, lCoeff1, lCoeff2)
                    lResults &= " BEST-FIT EQUATION:" & vbCrLf
                    lResults &= " T = ( " & String.Format("{0:0.0000}", lCoeff1).PadLeft(12, " ") & "* LOGQ )  +  " & String.Format("{0:0.0000}", lCoeff2).PadLeft(12, " ") & vbCrLf
                    lResults &= " DAYS/LOG CYCLE=" & -1 * lCoeff1 & vbCrLf
                    lResults &= " MEAN LOG Q = " & lXMean & vbCrLf
                    lResults &= " " & vbCrLf
                    lSW = New IO.StreamWriter(lFileOut1)
                    lSW.WriteLine("*************** Regression Results *****************")
                    lSW.WriteLine(lResults)
                    lSW.WriteLine("****************************************************" & vbCrLf & vbCrLf)

                    lSW.WriteLine(lHeaderOutFile1)
                    '   19 FORMAT (1F10.5,1F15.3,3F8.1,10X,1I6,2I3)
                    J2Date(lTsSF.Dates.Value(liPeak - 1), lDate)
                    Dim lStrxMean As String = String.Format("{0:0.00000}", lXMean).PadLeft(10, " ")
                    Dim lStrCoeff1 As String = String.Format("{0:0.000}", -1 * lCoeff1).PadLeft(15, " ")
                    Dim lStriMin As String = String.Format("{0:0.0}", liMin).PadLeft(8, " ")
                    Dim lStryMean As String = String.Format("{0:0.0}", lYMean).PadLeft(8, " ")
                    Dim lStriMax As String = String.Format("{0:0.0}", liMax).PadLeft(8, " ")
                    Dim lStrBlnk As String = Space(10)
                    Dim lStrYear As String = lDate(0).ToString.PadLeft(6, " ")
                    Dim lStrMonth As String = lDate(1).ToString.PadLeft(3, " ")
                    Dim lStrDay As String = lDate(2).ToString.PadLeft(3, " ")

                    lSW.WriteLine(lStrxMean & lStrCoeff1 & lStriMin & lStryMean & lStriMax & lStrBlnk & lStrYear & lStrMonth & lStrDay)
                    lSW.Flush()
                    lSW.Close()
                    lSW = Nothing

                    lXMeanAR(lNumRecessPeriods) = lXMean
                    lYMeanAR(lNumRecessPeriods) = lYMean
                    lCoef1AR(lNumRecessPeriods) = lCoeff1
                    lCoef2AR(lNumRecessPeriods) = lCoeff2
                    lMinAR(lNumRecessPeriods) = liMin
                    lMaxAR(lNumRecessPeriods) = liMax
                    lDatesAR(lNumRecessPeriods) = lTsSF.Dates.Value(liPeak - 1)
                    If lNumRecessPeriods = lMaxNumRecPeriods Then
                        lMsg = ""
                        lMsg &= "YOU HAVE ANALYZED " & lMaxNumRecPeriods & " RECESSIONS." & vbCrLf
                        lMsg &= "THIS IS THE MAXIMUM ALLOWABLE."
                        Logger.Dbg(lMsg)
                    End If
                    liCount -= 1
                    Continue For 'loop 200
                Else
                    Logger.Dbg("OPTION NOT RECOGNIZED. CHOOSE AGAIN.")
                    'Go To 230 marker to display again
                End If
            End If 'end of long if branch
        Next 'original loop 200

        '----------CONTINUE AFTER RECESSION PERIODS HAVE BEEN SELECTED:-------
        Dim lNumbrII As Integer = II 'within 3000
        Dim liiDV As Integer = 0
        For Z As Integer = 1 To lNumbrII 'loop 270
            If lXX(Z) <> 0 And lYY(Z) <> 0 And lXX(Z) <> -99 And lYY(Z) <> -99 Then
                liiDV += 1
            End If
        Next 'loop 270
        lSW = New IO.StreamWriter(lFileOut1, True)
        lSW.WriteLine("TOTAL NUMBER OF DAILY VALUES OF STREAMFLOW THAT WERE USED, FOR ALL RECESSION")
        lSW.WriteLine("PERIODS INITIALLY SELECTED = " & liiDV)
        lSW.Flush()
        lSW.Close()
        lSW = Nothing

        'original program has a console input to ask user 
        'if continue with analyzing this recession period,
        'seems it progressively analyze each recession period one at a time
        Dim lAskUserAnalyseThis As Boolean = True
        If Not lAskUserAnalyseThis Then
            'close down all files and quit the whole program
            Exit Sub
        End If

        ' ----- DETERMINE MAX AND MIN K AND TRANSFER LOGQ AND K TO OTHER ----
        ' ---------- VARIABLES FOR LISTING THEM BY DECREASING LOGQ:  --------

        Dim lSlopeMx As Double = 0
        Dim lSlopeMn As Double = 2000.0
        Dim lSlope As Double
        For Z As Integer = 1 To lNumRecessPeriods
            lSlope = -1 * lCoef1AR(Z)
            If lSlope > lSlopeMx Then lSlopeMx = lSlope
            If lSlope < lSlopeMn Then lSlopeMn = lSlope
            lOrigNoAR(Z) = Z
            lXMNArray(Z) = lXMeanAR(Z)
            lCoefArray(Z) = lCoef1AR(Z)
        Next

        'sort the three arrays (of the same size) to be in ascending order
        Order(lNumRecessPeriods, lXMNArray, lCoefArray, lOrigNoAR)

        lSW = New IO.StreamWriter(lFileOut1, True)
        lSW.WriteLine("NUMBER OF RECESSION PERIODS INITIALLY SELECTED=" & lNumRecessPeriods)
        lSW.WriteLine("MAXIMUM LOG Q FOR ALL RECESSIONS=" & lQLogMax)
        lSW.WriteLine("MINIMUM LOG Q FOR ALL RECESSIONS=" & lQLogMin)
        lSW.WriteLine("--------------------------------------------------------------------")
        lSW.WriteLine("       RECESSION PERIODS AFTER SORTING BY LOG Q:")
        lSW.WriteLine("ORIG.                              GRAPHIC OF RECESSION INDEX (K)")
        Dim lStrSlopeMin As String = String.Format("{0:0.0}", lSlopeMn).PadLeft(7, " ")
        Dim lStrSlopeMax As String = String.Format("{0:0.0}", lSlopeMx).PadLeft(7, " ")
        lSW.WriteLine("NUMBER LOG Q     K        ".PadRight(26, " ") & lStrSlopeMin & Space(33) & lStrSlopeMax)
        '   18 FORMAT (A26, 1F7.1, 33X, 1F7.1)

        For Z As Integer = lNumRecessPeriods To 1 Step -1 'loop 310
            lPickAR(Z) = 1
            lSlope = -1 * lCoefArray(Z)
            lDiff = lSlope - lSlopeMn
            lNumBlanks = Math.Floor(lDiff * 40 / (lSlopeMx - lSlopeMn))

            '   15 FORMAT (1I3, 2F9.3, 9X, 42A1)
            Dim lStrOrigNo As String = lOrigNoAR(Z).ToString.PadLeft(3, " ")
            Dim lStrXMN As String = String.Format("{0:0.000}", lXMNArray(Z)).PadLeft(9, " ")
            Dim lStrCoef As String = String.Format("{0:0.000}", lCoefArray(Z)).PadLeft(9, " ")
            Dim lStrBlnk As String = Space(lNumBlanks) & "*"
            lSW.WriteLine(lStrOrigNo & lStrXMN & lStrCoef & lStrBlnk)
        Next 'loop 310

        '-------- SELECT DATA LINES TO BE DELETED BEFORE REGRESSION:  --------
        Dim lAskUserNumRecToBeEliminated As Integer = 1
        lSW.WriteLine("BEFORE OBTAINING THE LEAST-SQUARES BEST FIT ")
        lSW.WriteLine("EQUATION FOR K (DELTA T/DELTA LOG Q) VERSUS")
        lSW.WriteLine("LOG Q, THIS NUMBER OF RECESSIONS WAS ELIMINATED:" & lAskUserNumRecToBeEliminated)
        lSW.Flush()
        lSW.Close()
        lSW = Nothing

        If lAskUserNumRecToBeEliminated = 0 Then

        Else
            lSW = New IO.StreamWriter(lFileOut2, True)
            lSW.WriteLine("NOTE THAT THESE RECESSIONS, IDENTIFIED BY   ")
            lSW.WriteLine("THEIR ORIGINAL SEQUENTIAL NUMBERS, WERE     ")
            lSW.WriteLine("DELETED FROM ANALYSIS BEFORE DETERMINING   ")
            lSW.WriteLine("BEST-FIT EQUATIONS:")
            For Z As Integer = 1 To lAskUserNumRecToBeEliminated
                'Apparently, this loop is to delete the previously determine number
                ' of recession values from the list
                'ENTER RECESSION TO ELIMINATE (ENTER ITS "ORIGINAL NUMBER")
                Dim lAskUserOriginalRecessionNumberToBeEliminated As Integer = 1
                lSW.WriteLine("             " & lAskUserOriginalRecessionNumberToBeEliminated)
                For J As Integer = 1 To lNumRecessPeriods
                    If lOrigNoAR(J) = lAskUserOriginalRecessionNumberToBeEliminated Then
                        lOrigNoAR(J) = 0
                    End If
                Next
            Next

            lSW.Flush()
            lSW.Close()
            lSW = Nothing
        End If

        '----ASSIGN VALUES TO X AND Y TO BE SENT TO THE REGRESSION SUBROUTINE:----
        lNum = 0
        For Z As Integer = 1 To lNumRecessPeriods 'loop 340
            If lOrigNoAR(Z) = 0 Then Continue For
            lNum += 1
            lX(lNum) = lXMNArray(Z)
            lY(lNum) = lCoefArray(Z)
            lOrigNoAR(lNum) = lOrigNoAR(Z)
        Next 'loop 340
        lNumRecessPeriods = lNum
        'this lmsg ought to be displayed
        lMsg = ""
        lMsg &= "    X               Y" & vbCrLf
        For Z As Integer = lNumRecessPeriods To 1 Step -1
            lMsg &= lX(Z) & "      " & lY(Z) & vbCrLf
        Next
        Logger.Dbg(lMsg)
        '--------------- SHOW ORDERED DATA, AFTER ELIMINATION:  --------------
        lSW = New IO.StreamWriter(lFileOut1, True)
        lSW.WriteLine(" ")
        lSW.WriteLine("-----------------------------------------------------------------------")
        lSW.WriteLine("        RECESSION PERIODS LEFT AFTER ELIMINATION: ")
        lSW.WriteLine("ORIG.  ORDERED                      GRAPHIC OF RECESSION INDEX (K)")
        lStrSlopeMin = String.Format("{0:0.0}", lSlopeMn).PadLeft(7, " ")
        lStrSlopeMax = String.Format("{0:0.0}", lSlopeMx).PadLeft(7, " ")
        lSW.WriteLine("NUMBER  LOG Q    K         " & lStrSlopeMin & Space(33) & lStrSlopeMax)
        For Z As Integer = lNumRecessPeriods To 1 Step -1 'loop 352
            lSlope = -1.0 * lY(Z)
            lDiff = lSlope - lSlopeMn
            lNumBlanks = Math.Floor(lDiff * 40 / (lSlopeMx - lSlopeMn))

            Dim lStrOrigNo As String = lOrigNoAR(Z).ToString.PadLeft(3, " ")
            Dim lStrX As String = String.Format("{0:0.000}", lX(Z)).PadLeft(9, " ")
            Dim lStrY As String = String.Format("{0:0.000}", lY(Z)).PadLeft(9, " ")
            Dim lStrBlnk As String = Space(lNumBlanks) & "*" '.PadRight(42, " ")
            lSW.WriteLine(lStrOrigNo & lStrX & lStrY & lStrBlnk)
        Next 'loop 352

        Dim lMNLogQC As Double = lX(1)
        Dim lMXLogQC As Double = lX(lNumRecessPeriods)

        lSW.WriteLine("AMONG THE SELECTED RECESSION PERIODS, THESE ARE THE")
        lSW.WriteLine("MIN AND MAX VALUES OF LOGQ FOR WHICH K (DAYS PER ")
        lSW.WriteLine("LOGQC WAS CALCULATED:" & lMNLogQC & "   " & lMXLogQC)

        '-- PERFORM REGRESSION AND WRITE BEST EQUATION FOR K AS FUNCT. OF DISCHARGE
        Dim lCoeffA As Double = 0.0
        Dim lCoeffB As Double = 0.0
        DoRegression2(lX, lY, lNum, lCoeffA, lCoeffB)
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

        Dim lXLogQ As Double = lMXLogQC
        While True 'loop 370
            lSlope = -1 * lCoeffA * lXLogQ - 1 * lCoeffB
            lDiff = lSlope - lSlopeMn
            lNumBlanks = Math.Floor(lDiff * 40 / (lSlopeMx - lSlopeMn))
            If lNumBlanks < 0 Then
                lSW.WriteLine(lXLogQ & "   " & lSlope & "    ")
            Else
                lSW.WriteLine(lXLogQ & "   " & lSlope & Space(lNumBlanks) & "*")
            End If
            lXLogQ -= 0.05
            If lXLogQ > lMNLogQC Then
                Continue While
            Else
                Exit While
            End If
        End While 'loop 370

        '---- AFTER INTEGRATION WRITE EQUATION FOR TIME AS FUNCTION OF DISCHARGE:
        Dim lCoeffC As Double = -0.5 * lCoeffA * lMXLogQC ^ 2 - lCoeffB * lMXLogQC
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
        Dim lTimeMax As Double = 0.5 * lCoeffA * lQLogMin ^ 2 + lCoeffB * lQLogMin + lCoeffC
        lSW.WriteLine("    TIME(D)         LOG Q           Q       0.0 ")
        lXLogQ = lMXLogQC
        While True 'loop 380
            Dim lT As Double = 0.5 * lCoeffA * lXLogQ ^ 2 + lCoeffB * lXLogQ + lCoeffC
            Dim lXQ As Double = 10 ^ lXLogQ
            lNumBlanks = Math.Floor(lT * 25 / lTimeMax)
            Dim lBlankStrNStar As String = ""
            If lNumBlanks > 0 Then
                lBlankStrNStar = Space(lNumBlanks) & "*"
            End If
            lSW.WriteLine(lT & " " & lXLogQ & " " & lXQ & " " & lBlankStrNStar)
            lXLogQ -= 0.05
            If lXLogQ > lMNLogQC Then
                Continue While
            Else
                Exit While
            End If
        End While 'loop 380

        lSW.WriteLine("--------------------------------------------------------------------")
        lSW.Flush()
        lSW.Close()
        lSW = Nothing

        '------------   DETERMINE MAX, MIN, AND MEDIAN K: -----------------------
        For Z As Integer = 1 To lNumRecessPeriods
            lK(Z) = -1 * lY(Z)
            lDUMMY(Z) = 1
        Next
        Order(lNumRecessPeriods, lK, lDUMMY, lOrigNoAR)
        Dim lKMax As Double = lK(lNumRecessPeriods)
        Dim lKMin As Double = lK(1)
        Dim liDown As Integer = 0
        Dim liUp As Integer = lNumRecessPeriods + 1
        Dim liCnt As Integer = 0
        Dim lKMed As Double
        While True 'loop 395
            liCnt += 1
            If liCnt > lMaxNumRecPeriods Then
                Logger.Dbg("PROBLEMS WITH DETERMINATION OF MEDIAN")
                lMsg = ""
                lMsg &= "PROBLEMS WITH DETERMINATION OF MEDIAN"
                Exit While
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

        '------------------- WRITE RAW RECESSION DATA TO "y-file"  ----------------
        lSW = New IO.StreamWriter(lFileOut2, True)
        lSW.WriteLine("----------------------------------------------------------------------")
        lSW.WriteLine("     Tpeak            Tmrc          LogQ           Q               Seq#  ")

        Dim lStrYY As String = ""
        Dim lStrZZ As String = ""
        Dim lStrXX As String = ""
        Dim lStr10ExpXX As String = ""
        Dim lStrIndicator As String = ""
        II = lNumbrII + 1
        lDiff = 0
        While True 'loop 520
            II -= 1
            If lXX(II) = 0 And lYY(II) = 0 Then
                lSW.WriteLine("   ")
                II -= 1
                Dim lT As Double = 0.5 * lCoeffA * lXX(II) ^ 2 + lCoeffB * lXX(II) + lCoeffC
                lDiff = lT - lYY(II)
                lZZ(II) = lT
                '   31 format (4f14.6, 1i14)
                lStrYY = String.Format("{0:0.000000}", lYY(II)).PadLeft(14, " ")
                lStrZZ = String.Format("{0:0.000000}", lZZ(II)).PadLeft(14, " ")
                lStrXX = String.Format("{0:0.000000}", lXX(II)).PadLeft(14, " ")
                lStr10ExpXX = String.Format("{0:0.000000}", 10 ^ lXX(II)).PadLeft(14, " ")
                lStrIndicator = lIndicator(II).ToString.PadLeft(14, " ")
                lSW.WriteLine(lStrYY & lStrZZ & lStrXX & lStr10ExpXX & lStrIndicator)
                Continue While
            ElseIf lXX(II) = -99 And lYY(II) = -99 Then
                'or if II = 1 as this loop is going backwards
                Exit While
            Else
                lZZ(II) = lDiff + lYY(II)
                lStrYY = String.Format("{0:0.000000}", lYY(II)).PadLeft(14, " ")
                lStrZZ = String.Format("{0:0.000000}", lZZ(II)).PadLeft(14, " ")
                lStrXX = String.Format("{0:0.000000}", lXX(II)).PadLeft(14, " ")
                lStr10ExpXX = String.Format("{0:0.000000}", 10 ^ lXX(II)).PadLeft(14, " ")
                lStrIndicator = lIndicator(II).ToString.PadLeft(14, " ")
                lSW.WriteLine(lStrYY & lStrZZ & lStrXX & lStr10ExpXX & lStrIndicator)
                Continue While
            End If
        End While 'loop 520
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

        Dim lStrInputFile As String = ""
        If lInputfile.Length <= 12 Then
            lStrInputFile = lInputfile
        Else
            lStrInputFile = lInputfile.Substring(0, 12)
        End If
        If lAskUserRecIndexOnly <> 1 Then
            lSW = New IO.StreamWriter(lFileRecSum, True)
            '   17 FORMAT (A12,A1,1X,1I4,'-',1I4,1I3,3F6.1,2F8.3,1F9.4,2F10.4)
            Dim lStrYearStart As String = lYearStart.ToString
            Dim lStrYearEnd As String = lYearEnd.ToString
            Dim lStrKMin As String = String.Format("{0:0.0}", lKMin).PadLeft(6, " ")
            Dim lStrKMed As String = String.Format("{0:0.0}", lKMed).PadLeft(6, " ")
            Dim lStrKMax As String = String.Format("{0:0.0}", lKMax).PadLeft(6, " ")
            Dim lStrMNLogQC As String = String.Format("{0:0.000}", lMNLogQC).PadLeft(8, " ")
            Dim lStrMXLogQC As String = String.Format("{0:0.000}", lMXLogQC).PadLeft(8, " ")
            lStrCoeffA = String.Format("{0:0.0000}", 0.5 * lCoeffA).PadLeft(9, " ")
            lStrCoeffB = String.Format("{0:0.0000}", lCoeffB).PadLeft(10, " ")
            lStrCoeffC = String.Format("{0:0.0000}", lCoeffC).PadLeft(10, " ")

            lSW.WriteLine(lStrInputFile & lAskUserSeason & " " & lStrYearStart & "-" & lStrYearEnd & lNumRecessPeriods.ToString & _
                          lStrKMin & lStrKMed & lStrKMax & lStrMNLogQC & lStrMXLogQC & lStrCoeffA & lStrCoeffB & lStrCoeffC)
            lSW.Flush()
            lSW.Close()
            lSW = Nothing
        End If

        'here is the final result
        lSW = New IO.StreamWriter(lFileIndex, True)
        '   13 format (A12,1f8.2)
        lSW.WriteLine(lStrInputFile & String.Format("{0:0.00}", lKMed).PadLeft(8, " "))
        lSW.Flush()
        lSW.Close()
        lSW = Nothing

    End Sub

    '--------- THIS SUBROUTINE MAKES TABULAR OUTPUT OF RECESSION DATA: -----
    Private Function TableRecess(ByVal aQLog() As Double, _
                            ByVal aFlow() As Double, _
                            ByVal aDates() As Double, _
                            ByVal aiPeak As Integer, _
                            ByVal aiMin As Integer, _
                            ByVal aiMax As Integer, _
                            ByVal aPickStartingDay As Integer, _
                            ByVal aiDisplayLastDay As Integer) As String

        Logger.Dbg("ENTER STARTING DAY (1, 11, OR 21)") 'originally dynamically read in
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
    Private Function GraphRecess(ByVal aQLog() As Double, _
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

    Public Sub RefreshGraphRecess(ByVal aDataGroup As atcTimeseriesGroup)
        pGrapher = New clsGraphTime(aDataGroup, pZgc)
        With pGrapher.ZedGraphCtrl.GraphPane
            '.YAxis.Type = AxisType.Log
            .AxisChange()
            .CurveList.Item(0).Color = Drawing.Color.Red
            '.CurveList.Item(1).Color = Drawing.Color.DarkBlue
            'CType(.CurveList.Item(1), LineItem).Line.Width = 2
        End With
        pZgc.Refresh()
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
    Private Sub Inverse(ByRef A(,) As Double, ByVal N As Integer)
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
    Private Sub Order(ByVal aNumOfRecessPeriods As Integer, ByRef aList1XMean() As Double, ByRef aList2Coef1() As Double, ByRef aList3OriginalOrder() As Integer)
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

    Private Sub frmUSGSStreamFlowAnalysis_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed
        If pDataGroup IsNot Nothing Then
            pDataGroup.Clear()
            pDataGroup = Nothing
        End If
    End Sub

    Private Sub btnTable_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTable.Click
        pRecess.RecessAnalysis("d")
        txtDisplayText.Text = pRecess.Table
        pGraphRecessDatagroup.Clear()
        pGraphRecessDatagroup.Add(pRecess.GraphTs)
        RefreshGraphRecess(pGraphRecessDatagroup)
    End Sub

    Private Sub btnRefineRecessionDuration_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRefineRecessionDuration.Click
        tabRefine.Visible = True
    End Sub

    Private Sub btnRefineRecessionDuration_MouseHover(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRefineRecessionDuration.MouseHover
        tabRefine.Visible = True
    End Sub

    Private Sub btnRefineRecessionDuration_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRefineRecessionDuration.MouseLeave
        tabRefine.Visible = False
    End Sub

    Private Sub btnSetDays_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSetDays.Click
        tabRefine.Visible = False
        'TODO: might need to check user entered days
        pRecess.AskUserFirstDayofSegment = CInt(txtAskUserFirstDayofSegment.Text)
        pRecess.AskUserLastDayofSegment = CInt(txtAskUserLastDayofSegment.Text)
        pRecess.RecessAnalysis("c")
        txtDisplayText.Text = pRecess.Table
        pGraphRecessDatagroup.Clear()
        pGraphRecessDatagroup.Add(pRecess.GraphTs)
        RefreshGraphRecess(pGraphRecessDatagroup)
    End Sub
End Class