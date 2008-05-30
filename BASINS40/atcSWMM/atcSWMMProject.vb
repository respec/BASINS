Imports MapWinUtility
Imports System.Text
Imports atcUtility

Public Class SWMMProject
    Public Catchments As Catchments
    Public Conduits As Conduits
    Public Nodes As Nodes
    Public Landuses As Landuses
    Public RainGages As RainGages
    Public MetConstituents As MetConstituents

    Public Name As String = ""
    Public Title As String = ""
    Public SJDate As Double = 0.0
    Public EJDate As Double = 0.0
    Public BackdropFile As String = ""
    Public BackdropX1 As Double = 0.0
    Public BackdropY1 As Double = 0.0
    Public BackdropX2 As Double = 0.0
    Public BackdropY2 As Double = 0.0
    Public MapUnits As String = "METERS"

    Public Sub New()
        Name = ""
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

    Public Function Save(ByVal aFileName As String) As Boolean
        Dim lSB As New StringBuilder

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
        Dim lStartTimeString As String = lSDate(3) & ":" & lSDate(4)
        lSB.AppendLine("[OPTIONS]")
        lSB.AppendLine("START_DATE           " & lStartDateString)
        lSB.AppendLine("START_TIME           " & lStartTimeString)
        lSB.AppendLine("REPORT_START_DATE    " & lStartDateString)
        lSB.AppendLine("REPORT_START_TIME    " & lStartTimeString)
        lSB.AppendLine("END_DATE             " & lEDate(1) & "/" & lEDate(2) & "/" & lEDate(0))
        lSB.AppendLine("END_TIME             " & lEDate(3) & ":" & lEDate(4))
        lSB.AppendLine("")

        '[EVAPORATION] and [TEMPERATURE]
        lSB.AppendLine(MetConstituents.ToString)

        '[RAINGAGES]
        lSB.AppendLine(RainGages.ToString)

        '[SUBCATCHMENTS]
        lSB.AppendLine(Catchments.ToString)

        'lSB.AppendLine("[SUBAREAS]")

        '[JUNCTIONS] and [OUTFALLS]
        lSB.AppendLine(Nodes.ToString)

        '[CONDUITS] and [XSECTIONS]
        lSB.AppendLine(Conduits.ToString)

        '[LANDUSES]
        lSB.AppendLine(Landuses.ToString)

        '[COVERAGES]
        lSB.AppendLine(Landuses.CoveragesToString)

        '[TIMESERIES]
        lSB.AppendLine(RainGages.TimeSeriesHeaderToString)
        lSB.AppendLine(RainGages.TimeSeriesToString)
        lSB.AppendLine(MetConstituents.TimeSeriesToString)

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
            lSB.AppendLine("[BACKDROP]")
            lSB.AppendLine("FILE       " & """" & BackdropFile & """")
            lSB.AppendLine("DIMENSIONS " & Format(BackdropX1, "0.000") & " " & Format(BackdropY1, "0.000") & " " & Format(BackdropX2, "0.000") & " " & Format(BackdropY2, "0.000"))
        End If

        SaveFileString(aFileName, lSB.ToString)
        Return True
    End Function

    Public Function TimeSeriesToString(ByVal aTimeSeries As atcData.atcTimeseries, ByVal aTimeseriesTag As String) As String
        Dim lSB As New StringBuilder

        For lIndex As Integer = aTimeSeries.Dates.IndexOfValue(Me.SJDate, True) To aTimeSeries.Dates.IndexOfValue(Me.EJDate, True)
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
