Imports MapWinUtility
Imports System.Text
Imports atcUtility

Public Class SWMMProject
    Private pIsMetric As Boolean

    Public Catchments As Catchments
    Public Conduits As Conduits
    Public Nodes As Nodes
    Public Landuses As Landuses
    Public RainGages As RainGages
    Public MetConstituents As MetConstituents

    Public Name As String = ""
    Public FileName As String = ""
    Public Title As String = ""
    Public FlowUnits As String = "CFS"
    Public InfiltrationMethod As String = "HORTON"
    Public FlowRouting As String = "KINWAVE"
    Public SJDate As Double = 0.0
    Public EJDate As Double = 0.0
    Public SweepStart As String = "1/1"
    Public SweepEnd As String = "12/31"
    Public DryDays As Integer = 0
    Public ReportStep As String = "00:15:00"
    Public WetStep As String = "00:15:00"
    Public DryStep As String = "01:00:00"
    Public RoutingStep As String = "0:00:30"
    Public AllowPonding As String = "NO"
    Public InertialDamping As String = "PARTIAL"
    Public VariableStep As Double = 0.75
    Public LengtheningStep As Integer = 0
    Public MinSurfArea As Integer = 0
    Public NormalFlowLimited As String = "BOTH"
    Public SkipSteadyState As String = "NO"
    Public IgnoreRainfall As String = "NO"
    Public ForceMainEquation As String = "H-W"
    Public LinkOffsets As String = "DEPTH"

    Public BackdropFile As String = ""
    Public BackdropX1 As Double = 0.0
    Public BackdropY1 As Double = 0.0
    Public BackdropX2 As Double = 0.0
    Public BackdropY2 As Double = 0.0
    Public MapUnits As String = "METERS"

    Public Sub New()
        Name = ""
        FileName = ""
        Title = ""
        Catchments = New Catchments
        Catchments.SWMMProject = Me
        Conduits = New Conduits
        Conduits.SWMMProject = Me
        Nodes = New Nodes
        Landuses = New Landuses
        RainGages = New RainGages
        RainGages.SWMMProject = Me
        MetConstituents = New MetConstituents
        MetConstituents.SWMMProject = Me
        BackdropFile = ""
    End Sub

    Public Property IsMetric() As Boolean
        Get
            Return pIsMetric
        End Get
        Set(ByVal aIsMetric As Boolean)
            pIsMetric = aIsMetric
            Conduit.IsMetric = aIsMetric
        End Set
    End Property

    Public Function Run() As Boolean
        Dim lSWMMExeName As String = GetSetting("BASINS4", "SWMM", "ExeFileName", "")
        If Not IO.File.Exists(lSWMMExeName) Then
            lSWMMExeName = FindFile("Please locate SWMM Exe File", "Epaswmm5.exe", "*.exe")
        End If
        If IO.File.Exists(lSWMMExeName) Then
            Dim lProcess As New System.Diagnostics.Process
            With lProcess.StartInfo

                .FileName = lSWMMExeName
                .WorkingDirectory = "" 'lWorkingDirectory
                .Arguments = FileName
                .CreateNoWindow = True
                .UseShellExecute = False
                .RedirectStandardInput = True
                .RedirectStandardOutput = True
                AddHandler lProcess.OutputDataReceived, AddressOf MonitorMessageHandler
                .RedirectStandardError = True
                AddHandler lProcess.ErrorDataReceived, AddressOf MonitorMessageHandler
            End With
            lProcess.Start()
            '
            'NOTE: to debug pMonitorProcess, need Visual Studio (not Express) - choose Tools:AttachToProcess - StatusMonitor
            '
            lProcess.BeginErrorReadLine()
            lProcess.BeginOutputReadLine()
            Logger.Dbg("SWMM Started")
            lProcess.WaitForExit()
        End If
    End Function

    Private Sub MonitorMessageHandler(ByVal aSendingProcess As Object, _
                                  ByVal aOutLine As DataReceivedEventArgs)
        If Not String.IsNullOrEmpty(aOutLine.Data) Then
            Dim lMessage As String = aOutLine.Data.ToString
            Logger.Dbg("MsgFromStatusMonitor " & lMessage)
        End If
    End Sub

    Public Function Load(ByVal aFileName As String) As Boolean
        If Not IO.File.Exists(aFileName) Then
            Logger.Msg("File '" & aFileName & "' not found", "SWMMProjectLoadProblem")
            Return False
        Else
            Dim lSW As New IO.StreamReader(aFileName)
            While Not lSW.EndOfStream
                Dim lStr As String = lSW.ReadLine
                Select Case lStr.ToUpper
                    Case "[TITLE]"
                        Title = lSW.ReadLine
                    Case "[OPTIONS]"
                        'TODO:parse block

                    Case "[EVAPORATION]", "[TEMPERATURE]"
                        'TODO:parse block

                    Case "[RAINGAGES]"
                        'TODO:parse block

                    Case "[SUBCATCHMENTS]"
                        'TODO:parse block

                    Case "[SUBAREAS]"
                        'TODO:parse block

                    Case "[INFILTRATION]"
                        'TODO:parse block

                    Case "[JUNCTIONS]", "[OUTFALLS]"
                        'TODO:parse block

                    Case "[CONDUITS]", "[XSECTIONS]"
                        'TODO:parse block

                    Case "[LOSSES]"
                        'TODO:parse block

                    Case "[Landuses]"
                        'TODO:parse block

                    Case "[COVERAGES]"
                        'TODO:parse block

                    Case "[TIMESERIES]"
                        'TODO:parse block

                    Case "[REPORT]"
                        'TODO:parse block

                    Case "[TAGS]"
                        'TODO:parse block

                    Case "[MAP]"
                        'TODO:parse block

                    Case "[COORDINATES]"
                        'TODO:parse block

                    Case "[VERTICES]"
                        'TODO:parse block

                    Case "[POLYGONS]"
                        'TODO:parse block

                    Case "[SYMBOLS]"
                        'TODO:parse block

                    Case "[BACKDROP]"
                        'TODO:parse block

                    Case Else
                        Logger.Dbg("'" & lStr & "' is not a known input block  ")
                End Select
            End While
        End If
    End Function

    Public Function Save(ByVal aFileName As String) As Boolean
        Dim lSW As New IO.StreamWriter(aFileName)
        FileName = aFileName

        '[TITLE]
        lSW.WriteLine("[TITLE]")
        lSW.WriteLine(Title)
        lSW.WriteLine("")

        '[OPTIONS]
        Dim lSDate(6) As Integer
        J2Date(SJDate, lSDate)
        Dim lEDate(6) As Integer
        J2Date(EJDate, lEDate)
        Dim lStartDateString As String = lSDate(1) & "/" & lSDate(2) & "/" & lSDate(0)
        Dim lStartTimeString As String = Format(lSDate(3), "00") & ":" & Format(lSDate(4), "00") & ":00"
        lSW.WriteLine("[OPTIONS]")
        lSW.WriteLine("FLOW_UNITS           " & FlowUnits)
        lSW.WriteLine("INFILTRATION         " & InfiltrationMethod)
        lSW.WriteLine("FLOW_ROUTING         " & FlowRouting)
        lSW.WriteLine("START_DATE           " & lStartDateString)
        lSW.WriteLine("START_TIME           " & lStartTimeString)
        lSW.WriteLine("REPORT_START_DATE    " & lStartDateString)
        lSW.WriteLine("REPORT_START_TIME    " & lStartTimeString)
        lSW.WriteLine("END_DATE             " & lEDate(1) & "/" & lEDate(2) & "/" & lEDate(0))
        lSW.WriteLine("END_TIME             " & Format(lEDate(3), "00") & ":" & Format(lEDate(4), "00") & ":00")
        lSW.WriteLine("SWEEP_START          " & SweepStart)
        lSW.WriteLine("SWEEP_END            " & SweepEnd)
        lSW.WriteLine("DRY_DAYS             " & DryDays)
        lSW.WriteLine("REPORT_STEP          " & ReportStep)
        lSW.WriteLine("WET_STEP             " & WetStep)
        lSW.WriteLine("DRY_STEP             " & DryStep)
        lSW.WriteLine("ROUTING_STEP         " & RoutingStep)
        lSW.WriteLine("ALLOW_PONDING        " & AllowPonding)
        lSW.WriteLine("INERTIAL_DAMPING     " & InertialDamping)
        lSW.WriteLine("VARIABLE_STEP        " & VariableStep)
        lSW.WriteLine("LENGTHENING_STEP     " & LengtheningStep)
        lSW.WriteLine("MIN_SURFAREA         " & MinSurfArea)
        lSW.WriteLine("NORMAL_FLOW_LIMITED  " & NormalFlowLimited)
        lSW.WriteLine("SKIP_STEADY_STATE    " & SkipSteadyState)
        lSW.WriteLine("IGNORE_RAINFALL      " & IgnoreRainfall)
        lSW.WriteLine("FORCE_MAIN_EQUATION  " & ForceMainEquation)
        lSW.WriteLine("LINK_OFFSETS         " & LinkOffsets)
        lSW.WriteLine("")

        '[EVAPORATION] and [TEMPERATURE]
        lSW.WriteLine(MetConstituents.ToString)

        '[RAINGAGES]
        lSW.WriteLine(RainGages.ToString)
        RainGages.TimeSeriesToFile()

        '[SUBCATCHMENTS]
        lSW.WriteLine(Catchments.ToString)

        '[SUBAREAS]
        lSW.WriteLine(Catchments.SubareasToString)

        '[INFILTRATION]
        lSW.WriteLine(Catchments.InfiltrationToString)

        '[JUNCTIONS] and [OUTFALLS]
        lSW.WriteLine(Nodes.ToString)

        '[CONDUITS] and [XSECTIONS]
        lSW.WriteLine(Conduits.ToString)

        '[LOSSES]
        lSW.WriteLine("[LOSSES]")
        lSW.WriteLine(";;Link           Inlet      Outlet     Average    Flap Gate ")
        lSW.WriteLine(";;-------------- ---------- ---------- ---------- ----------")
        lSW.WriteLine("")

        '[LANDUSES]
        lSW.WriteLine(Landuses.ToString)

        '[COVERAGES]
        lSW.WriteLine(Landuses.CoveragesToString)

        '[TIMESERIES]
        lSW.WriteLine(MetConstituents.TimeSeriesHeaderToString)
        If EJDate - SJDate < 30 Then
            MetConstituents.TimeSeriesToStream(lSW)
        Else
            'more than 30 days, write to file
            lSW.WriteLine(MetConstituents.TimeSeriesFileNamesToString)
            MetConstituents.TimeSeriesToFile()
        End If
        lSW.WriteLine()

        '[REPORT]
        lSW.WriteLine("[REPORT]")
        lSW.WriteLine("INPUT      NO")
        lSW.WriteLine("CONTROLS   NO")
        lSW.WriteLine("")

        '[TAGS]
        lSW.WriteLine("[TAGS]")
        lSW.WriteLine("")

        '[MAP]
        lSW.WriteLine("[MAP]")
        lSW.WriteLine("UNITS      " & MapUnits)
        lSW.WriteLine("")

        '[COORDINATES]
        lSW.WriteLine(Nodes.CoordinatesToString)

        '[VERTICES]
        lSW.WriteLine(Conduits.VerticesToString)

        '[Polygons]
        lSW.WriteLine(Catchments.PolygonsToString)

        '[SYMBOLS]
        lSW.WriteLine(RainGages.CoordinatesToString)

        '[BACKDROP]
        If BackdropFile.Length > 0 Then
            lSW.WriteLine("")
            lSW.WriteLine("[BACKDROP]")
            lSW.WriteLine("FILE       " & """" & BackdropFile & """")
            lSW.WriteLine("DIMENSIONS " & Format(BackdropX1, "0.000") & " " & Format(BackdropY1, "0.000") & " " & Format(BackdropX2, "0.000") & " " & Format(BackdropY2, "0.000"))
        End If

        lSW.Close()

        Return True
    End Function

    Public Function TimeSeriesToString(ByVal aTimeSeries As atcData.atcTimeseries, _
                                       ByVal aTimeseriesTag As String, _
                                       Optional ByVal aType As String = "PREC") As String
        Dim lStartIndex As Integer = aTimeSeries.Dates.IndexOfValue(Me.SJDate, True)
        If Me.SJDate = aTimeSeries.Dates.Values(0) Or lStartIndex < 0 Then
            lStartIndex = 0
        End If
        Dim lEndIndex As Integer = aTimeSeries.Dates.IndexOfValue(Me.EJDate, True)
        Dim lSB As New StringBuilder
        For lIndex As Integer = lStartIndex To lEndIndex - 1
            If aType = "PREC" Then
                lSB.Append(StrPad(aTimeseriesTag, 16, " ", False))
                lSB.Append(" ")
            End If
            Dim lJDate As Double = aTimeSeries.Dates.Values(lIndex)
            Dim lDate(6) As Integer
            J2Date(lJDate, lDate)
            Dim lDateString As String
            If aType = "PREC" Then
                lDateString = lDate(0) & "  " & lDate(1) & "  " & lDate(2) & "  " & lDate(3) & "  " & lDate(4)
            Else
                lDateString = lDate(1) & "/" & lDate(2) & "/" & lDate(0) & " " & lDate(3) & ":" & lDate(4)
            End If
            lSB.Append(StrPad(lDateString, 20, " ", False))
            lSB.Append(" ")
            lSB.Append(StrPad(Format(aTimeSeries.Values(lIndex + 1), "0.000"), 10, " ", False))
            lSB.Append(vbCrLf)
        Next

        Return lSB.ToString
    End Function

    Public Sub TimeSeriesToStream(ByVal aTimeSeries As atcData.atcTimeseries, _
                                  ByVal aTimeseriesTag As String, _
                                  ByVal aSW As IO.StreamWriter)

        Dim lStartIndex As Integer = aTimeSeries.Dates.IndexOfValue(Me.SJDate, True)
        If Me.SJDate = aTimeSeries.Dates.Values(0) Or lStartIndex < 0 Then
            lStartIndex = 0
        End If
        Dim lEndIndex As Integer = aTimeSeries.Dates.IndexOfValue(Me.EJDate, True)
        For lIndex As Integer = lStartIndex To lEndIndex - 1
            aSW.Write(StrPad(aTimeseriesTag, 16, " ", False))
            aSW.Write(" ")
            Dim lJDate As Double = aTimeSeries.Dates.Values(lIndex)
            Dim lDate(6) As Integer
            J2Date(lJDate, lDate)
            Dim lDateString As String = lDate(1) & "/" & lDate(2) & "/" & lDate(0)
            Dim lTimeString As String = lDate(3).ToString.PadLeft(2, "0") & ":" & lDate(4).ToString.PadLeft(2, "0")
            aSW.Write(StrPad(lDateString, 10, " ", False))
            aSW.Write(" ")
            aSW.Write(StrPad(lTimeString, 10, " ", False))
            aSW.Write(" ")
            aSW.Write(StrPad(Format(aTimeSeries.Values(lIndex + 1), "0.000"), 10, " ", False))
            aSW.Write(vbCrLf)
        Next
    End Sub

    Public Sub Run(ByVal aInputFileName As String)
        If IO.File.Exists(aInputFileName) Then
            Dim lSWMMexe As String = atcUtility.FindFile("Please locate the EPA SWMM 5.0 Executable", "\Program Files\EPA SWMM 5.0\epaswmm5.exe")
            If IO.File.Exists(lSWMMexe) Then
                LaunchProgram(lSWMMexe, IO.Path.GetDirectoryName(aInputFileName), "/f " & aInputFileName, False)
                Logger.Dbg("SWMM launched with input " & aInputFileName)
            Else
                Logger.Msg("Cannot find the EPA SWMM 5.0 Executable", MsgBoxStyle.Critical, "BASINS SWMM Problem")
            End If
        Else
            Logger.Msg("Cannot find SWMM 5.0 Input File " & aInputFileName)
        End If
    End Sub
End Class
