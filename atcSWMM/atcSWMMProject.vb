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

    Public Function Save(ByVal aFileName As String) As Boolean
        Dim lSB As New StringBuilder
        FileName = aFileName

        '[TITLE]
        lSB.AppendLine("[TITLE]")
        lSB.AppendLine(Title)
        lSB.AppendLine("")

        '[OPTIONS]
        Dim lSDate(6) As Integer
        J2Date(SJDate, lSDate)
        Dim lEDate(6) As Integer
        J2Date(EJDate, lEDate)
        Dim lStartDateString As String = lSDate(1) & "/" & lSDate(2) & "/" & lSDate(0)
        Dim lStartTimeString As String = Format(lSDate(3), "00") & ":" & Format(lSDate(4), "00") & ":00"
        lSB.AppendLine("[OPTIONS]")
        lSB.AppendLine("FLOW_UNITS           " & FlowUnits)
        lSB.AppendLine("INFILTRATION         " & InfiltrationMethod)
        lSB.AppendLine("FLOW_ROUTING         " & FlowRouting)
        lSB.AppendLine("START_DATE           " & lStartDateString)
        lSB.AppendLine("START_TIME           " & lStartTimeString)
        lSB.AppendLine("REPORT_START_DATE    " & lStartDateString)
        lSB.AppendLine("REPORT_START_TIME    " & lStartTimeString)
        lSB.AppendLine("END_DATE             " & lEDate(1) & "/" & lEDate(2) & "/" & lEDate(0))
        lSB.AppendLine("END_TIME             " & Format(lEDate(3), "00") & ":" & Format(lEDate(4), "00") & ":00")
        lSB.AppendLine("SWEEP_START          " & SweepStart)
        lSB.AppendLine("SWEEP_END            " & SweepEnd)
        lSB.AppendLine("DRY_DAYS             " & DryDays)
        lSB.AppendLine("REPORT_STEP          " & ReportStep)
        lSB.AppendLine("WET_STEP             " & WetStep)
        lSB.AppendLine("DRY_STEP             " & DryStep)
        lSB.AppendLine("ROUTING_STEP         " & RoutingStep)
        lSB.AppendLine("ALLOW_PONDING        " & AllowPonding)
        lSB.AppendLine("INERTIAL_DAMPING     " & InertialDamping)
        lSB.AppendLine("VARIABLE_STEP        " & VariableStep)
        lSB.AppendLine("LENGTHENING_STEP     " & LengtheningStep)
        lSB.AppendLine("MIN_SURFAREA         " & MinSurfArea)
        lSB.AppendLine("NORMAL_FLOW_LIMITED  " & NormalFlowLimited)
        lSB.AppendLine("SKIP_STEADY_STATE    " & SkipSteadyState)
        lSB.AppendLine("IGNORE_RAINFALL      " & IgnoreRainfall)
        lSB.AppendLine("FORCE_MAIN_EQUATION  " & ForceMainEquation)
        lSB.AppendLine("LINK_OFFSETS         " & LinkOffsets)
        lSB.AppendLine("")

        '[EVAPORATION] and [TEMPERATURE]
        lSB.AppendLine(MetConstituents.ToString)

        '[RAINGAGES]
        lSB.AppendLine(RainGages.ToString)
        RainGages.TimeSeriesToFile()

        '[SUBCATCHMENTS]
        lSB.AppendLine(Catchments.ToString)

        '[SUBAREAS]
        lSB.AppendLine(Catchments.SubareasToString)

        '[INFILTRATION]
        lSB.AppendLine(Catchments.InfiltrationToString)

        '[JUNCTIONS] and [OUTFALLS]
        lSB.AppendLine(Nodes.ToString)

        '[CONDUITS] and [XSECTIONS]
        lSB.AppendLine(Conduits.ToString)

        '[LOSSES]
        lSB.AppendLine("[LOSSES]")
        lSB.AppendLine(";;Link           Inlet      Outlet     Average    Flap Gate ")
        lSB.AppendLine(";;-------------- ---------- ---------- ---------- ----------")
        lSB.AppendLine("")

        '[LANDUSES]
        lSB.AppendLine(Landuses.ToString)

        '[COVERAGES]
        lSB.AppendLine(Landuses.CoveragesToString)

        '[TIMESERIES]
        lSB.AppendLine(MetConstituents.TimeSeriesHeaderToString)
        lSB.AppendLine(MetConstituents.TimeSeriesToString)

        '[REPORT]
        lSB.AppendLine("[REPORT]")
        lSB.AppendLine("INPUT      NO")
        lSB.AppendLine("CONTROLS   NO")
        lSB.AppendLine("")

        '[TAGS]
        lSB.AppendLine("[TAGS]")
        lSB.AppendLine("")

        '[MAP]
        lSB.AppendLine("[MAP]")
        lSB.AppendLine("UNITS      " & MapUnits)
        lSB.AppendLine("")

        '[COORDINATES]
        lSB.AppendLine(Nodes.CoordinatesToString)

        '[VERTICES]
        lSB.AppendLine(Conduits.VerticesToString)

        '[Polygons]
        lSB.AppendLine(Catchments.PolygonsToString)

        '[SYMBOLS]
        lSB.AppendLine(RainGages.CoordinatesToString)

        '[BACKDROP]
        If BackdropFile.Length > 0 Then
            lSB.AppendLine("")
            lSB.AppendLine("[BACKDROP]")
            lSB.AppendLine("FILE       " & """" & BackdropFile & """")
            lSB.AppendLine("DIMENSIONS " & Format(BackdropX1, "0.000") & " " & Format(BackdropY1, "0.000") & " " & Format(BackdropX2, "0.000") & " " & Format(BackdropY2, "0.000"))
        End If

        SaveFileString(FileName, lSB.ToString)
        Return True
    End Function

    Public Function HourlyTimeSeriesToString(ByVal aTimeSeries As atcData.atcTimeseries, ByVal aTimeseriesTag As String) As String
        Dim lSB As New StringBuilder

        For lIndex As Integer = aTimeSeries.Dates.IndexOfValue(Me.SJDate, True) To aTimeSeries.Dates.IndexOfValue(Me.EJDate, True)
            If aTimeSeries.Values(lIndex) > 0 Then
                lSB.Append(StrPad(aTimeseriesTag, 16, " ", False))
                lSB.Append(" ")
                Dim lJDate As Double = aTimeSeries.Dates.Values(lIndex)
                Dim lDate(6) As Integer
                J2Date(lJDate, lDate)
                lSB.Append(StrPad(lDate(0), 10, " ", False))
                lSB.Append(" ")
                lSB.Append(StrPad(lDate(1), 10, " ", False))
                lSB.Append(" ")
                lSB.Append(StrPad(lDate(2), 10, " ", False))
                lSB.Append(" ")
                lSB.Append(StrPad(lDate(3), 10, " ", False))
                lSB.Append(" ")
                lSB.Append(StrPad(lDate(4), 10, " ", False))
                lSB.Append(" ")
                lSB.Append(StrPad(Format(aTimeSeries.Values(lIndex), "0.000"), 10, " ", False))
                lSB.Append(vbCrLf)
            End If
        Next

        Return lSB.ToString
    End Function

    Public Function TimeSeriesToString(ByVal aTimeSeries As atcData.atcTimeseries, ByVal aTimeseriesTag As String) As String
        Dim lSB As New StringBuilder

        Dim lStartIndex As Integer = aTimeSeries.Dates.IndexOfValue(Me.SJDate, True)
        If Me.SJDate = aTimeSeries.Dates.Values(0) Then
            lStartIndex = 0
        End If
        Dim lEndIndex As Integer = aTimeSeries.Dates.IndexOfValue(Me.EJDate, True)
        For lIndex As Integer = lStartIndex To lEndIndex
            lSB.Append(StrPad(aTimeseriesTag, 16, " ", False))
            lSB.Append(" ")
            Dim lJDate As Double = aTimeSeries.Dates.Values(lIndex)
            Dim lDate(6) As Integer
            J2Date(lJDate, lDate)
            Dim lDateString As String = lDate(1) & "/" & lDate(2) & "/" & lDate(0)
            Dim lTimeString As String = lDate(3) & ":" & lDate(4)
            lSB.Append(StrPad(lDateString, 10, " ", False))
            lSB.Append(" ")
            lSB.Append(StrPad(lTimeString, 10, " ", False))
            lSB.Append(" ")
            lSB.Append(StrPad(Format(aTimeSeries.Values(lIndex), "0.000"), 10, " ", False))
            lSB.Append(vbCrLf)
        Next

        Return lSB.ToString
    End Function

    Public Sub Run(ByVal aInputFileName As String)
        If IO.File.Exists(aInputFileName) Then
            Dim lSWMMexe As String = atcUtility.FindFile("Please locate the EPA SWMM 5.0 Executable", "\Program Files\EPA SWMM 5.0\epaswmm5.exe")
            If IO.File.Exists(lSWMMexe) Then
                LaunchProgram(lSWMMexe, IO.Path.GetDirectoryName(aInputFileName), "/f " & aInputFileName, False)
            Else
                Logger.Msg("Cannot find the EPA SWMM 5.0 Executable", MsgBoxStyle.Critical, "BASINS SWMM Problem")
            End If
        Else
            Logger.Msg("Cannot find SWMM 5.0 Input File " & aInputFileName)
        End If
    End Sub
End Class
