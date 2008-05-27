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
        MetConstituents = New MetConstituents
    End Sub

    Public Function Save(ByVal aFileName As String) As Boolean
        Dim lSB As New StringBuilder

        lSB.AppendLine("[TITLE]")
        lSB.AppendLine(Title)
        lSB.AppendLine("")

        'lSB.AppendLine("[OPTIONS]")
        'lSB.AppendLine("[EVAPORATION]")
        'lSB.AppendLine("[TEMPERATURE]")

        '[RAINGAGES]
        lSB.AppendLine(RainGages.ToString)

        '[SUBCATCHMENTS]
        lSB.AppendLine(Catchments.ToString)

        'lSB.AppendLine("[SUBAREAS]")

        '[JUNCTIONS] and [OUTFALLS]
        lSB.AppendLine(Nodes.ToString)

        '[CONDUITS]
        lSB.AppendLine(Conduits.ToString)

        'lSB.AppendLine("[XSECTIONS]")
        'lSB.AppendLine("[LANDUSES]")
        'lSB.AppendLine("[COVERAGES]")

        '[TIMESERIES]
        lSB.AppendLine(RainGages.TimeSeriesHeaderToString)
        lSB.AppendLine(RainGages.TimeSeriesToString)

        'lSB.AppendLine("[MAP]")

        '[COORDINATES]
        lSB.AppendLine(Nodes.CoordinatesToString)

        '[VERTICES]
        lSB.AppendLine(Conduits.VerticesToString)

        '[Polygons]
        lSB.AppendLine(Catchments.PolygonsToString)

        '[SYMBOLS]
        lSB.AppendLine(RainGages.CoordinatesToString)

        'lSB.AppendLine("[BACKDROP]")

        SaveFileString(aFileName, lSB.ToString)
        Return True
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
