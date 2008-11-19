Imports System.Collections.ObjectModel
Imports System.IO
Imports MapWinUtility
Imports atcUtility
Imports System.Text
Imports atcData

Public Class Segments
    Inherits KeyedCollection(Of String, Segment)
    Protected Overrides Function GetKeyForItem(ByVal aSegment As Segment) As String
        Dim lKey As String = aSegment.Name
        Return lKey
    End Function

    Public WASPProject As WASPProject

    Public Sub AddRange(ByVal aEnumerable As IEnumerable)
        For Each lSegment As Segment In aEnumerable
            Me.Add(lSegment)
        Next
    End Sub

    Public Overrides Function ToString() As String
        Dim lString As New StringBuilder
        Dim lSegment As Segment

        lString.Append(Me.Count)
        lString.Append(" ")
        For Each lSegment In Me
            lString.Append(StrPad(lSegment.Name, 16, " ", False))
            lString.Append(" ")
        Next

        lString.Append("C Seg Number   Seg Length  Seg Width  DMult   Vmult  SegSlope  SegRough  DownSeg  InflowTS")
        lString.Append(" ")

        For Each lSegment In Me
            With lSegment
                lString.Append(StrPad(.ID, 16, " ", False))
                lString.Append(" ")
                lString.Append(StrPad(Format(.Length, "0.00"), 10, " ", False))
                lString.Append(" ")
                lString.Append(StrPad(Format(.Width, "0.00"), 10, " ", False))
                lString.Append(" ")
                lString.Append(StrPad(Format(.Dmult, "0.00"), 10, " ", False))
                lString.Append(" ")
                lString.Append(StrPad(Format(.Vmult, "0.00"), 10, " ", False))
                lString.Append(" ")
                lString.Append(StrPad(Format(.Slope, "0.0000"), 10, " ", False))
                lString.Append(" ")
                lString.Append(StrPad(Format(.Roughness, "0.000"), 10, " ", False))
                lString.Append(" ")
                lString.Append(StrPad(.DownID, 16, " ", False))
                lString.Append(vbCrLf)
            End With
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
    Public Dmult As Double = 0.0
    Public Vmult As Double = 0.0
    Public Slope As Double = 0.05
    Public Roughness As Double = 0.0
    Public InflowTimeseriesID As String = ""
End Class
