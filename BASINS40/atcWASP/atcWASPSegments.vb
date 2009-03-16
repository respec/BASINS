Imports System.Collections.ObjectModel
Imports MapWinUtility

Public Class Segments
    Inherits KeyedCollection(Of String, Segment)
    Protected Overrides Function GetKeyForItem(ByVal aSegment As Segment) As String
        Dim lKey As String = aSegment.ID & ":" & aSegment.Name
        Return lKey
    End Function

    Public WASPProject As WASPProject

    Public Sub AddRange(ByVal aEnumerable As IEnumerable)
        For Each lSegment As Segment In aEnumerable
            Try
                Me.Add(lSegment)
            Catch
            End Try
        Next
    End Sub

    Public Overrides Function ToString() As String
        Dim lString As New System.Text.StringBuilder

        lString.Append(";Name                            ID               Length     Width      Depth      Slope      Roughness  DownSegID        " & vbCrLf)
        lString.Append(";_______________________________ ________________ __________ __________ __________ __________ __________ ________________ " & vbCrLf)

        For Each lSegment As Segment In Me
            With lSegment
                lString.Append(.Name.PadRight(32) & " ")
                lString.Append(.ID.PadRight(16) & " ")
                lString.Append(Format(.Length, "0.00").PadRight(10) & " ")
                lString.Append(Format(.Width, "0.00").PadRight(10) & " ")
                lString.Append(Format(.Depth, "0.00").PadRight(10) & " ")
                lString.Append(.Slope.PadRight(10) & " ")
                lString.Append(Format(.Roughness, "0.000").PadRight(10) & " ")
                lString.Append(.DownID.PadRight(16) & " ")
                lString.Append(vbCrLf)
            End With
        Next

        Return lString.ToString
    End Function

    Public Function TimeseriesDirectoryToString() As String
        Dim lString As New System.Text.StringBuilder

        lString.Append(";Segment Name                    Segment ID       Timeseries Type  Timeseries File Name                    " & vbCrLf)
        lString.Append(";_______________________________ ________________ ________________ ________________________________________" & vbCrLf)

        For Each lSegment As Segment In Me
            For Each lTimeseries As WASPTimeseries In lSegment.InputTimeseriesCollection
                lString.Append(lSegment.Name.PadRight(32) & " ")
                lString.Append(lSegment.ID.PadRight(16) & " ")
                lString.Append(lTimeseries.Type.PadRight(16) & " ")
                lString.Append(lTimeseries.TimeSeries.Attributes.GetDefinedValue("Location").Value & "." & lTimeseries.Type & ".DAT")
                lString.Append(vbCrLf)
            Next
        Next

        Return lString.ToString
    End Function

End Class

Public Class Segment
    Public Name As String = ""
    Public ID As String = ""
    Public DownID As String = ""
    Public Length As Double = 0.0
    Public Width As Double = 0.0
    Public Depth As Double = 0.0
    Public Slope As String = "0.05"
    Public Roughness As Double = 0.0
    Public Velocity As Double = 0.0
    Public InputTimeseriesCollection As WASPTimeseriesCollection = Nothing
    Public BaseID As String = ""  'the id of the segment before breaking it up
    Public CentroidX As Double
    Public CentroidY As Double
    Public DrainageArea As Double

    Public Function Clone() As Segment
        Dim lNewSegment As New Segment
        lNewSegment.Depth = Me.Depth
        lNewSegment.DownID = Me.DownID
        lNewSegment.ID = Me.ID
        lNewSegment.Length = Me.Length
        lNewSegment.Name = Me.Name
        lNewSegment.Roughness = Me.Roughness
        lNewSegment.Slope = Me.Slope
        lNewSegment.Velocity = Me.Velocity
        lNewSegment.Width = Me.Width
        lNewSegment.BaseID = Me.BaseID
        lNewSegment.CentroidX = Me.CentroidX
        lNewSegment.CentroidY = Me.CentroidY
        lNewSegment.DrainageArea = Me.DrainageArea

        Dim lTimeseriesCollection As New atcWASP.WASPTimeseriesCollection
        lNewSegment.InputTimeseriesCollection = lTimeseriesCollection
        For Each lWASPTimeseries As WASPTimeseries In Me.InputTimeseriesCollection
            lNewSegment.InputTimeseriesCollection.Add(lWASPTimeseries)
        Next

        Return lNewSegment
    End Function
End Class
