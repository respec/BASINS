Imports System.Text
Imports atcUtility

Public Class SWMMProject
    Public Catchments As Catchments
    Public Conduits As Conduits
    Public Landuses As Landuses
    Public RainGages As RainGages
    Public MetConstituents As MetConstituents

    Public Name As String = ""
    Public Title As String = ""

    Public Sub New()
        Name = ""
        Title = ""
        Catchments = New Catchments
        Conduits = New Conduits
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
        'lSB.AppendLine("[RAINGAGES]")

        '[SUBCATCHMENTS]
        lSB.AppendLine(Catchments.ToString)

        'lSB.AppendLine("[SUBAREAS]")
        'lSB.AppendLine("[JUNCTIONS]")
        'lSB.AppendLine("[OUTFALLS]")
        'lSB.AppendLine("[CONDUITS]")
        'lSB.AppendLine("[XSECTIONS]")
        'lSB.AppendLine("[LANDUSES]")
        'lSB.AppendLine("[COVERAGES]")
        'lSB.AppendLine("[TIMESERIES]")
        'lSB.AppendLine("[MAP]")
        'lSB.AppendLine("[COORDINATES]")
        'lSB.AppendLine("[VERTICES]")
        'lSB.AppendLine("[Polygons]")
        'lSB.AppendLine("[SYMBOLS]")
        'lSB.AppendLine("[BACKDROP]")

        SaveFileString(aFileName, lSB.ToString)
        Return True
    End Function
End Class
