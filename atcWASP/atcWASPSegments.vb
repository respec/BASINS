Imports System.Collections.ObjectModel
Imports System.IO
Imports MapWinUtility
Imports atcUtility
Imports System.Text
Imports atcData

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
        Dim lString As New StringBuilder
        lString.Append(Me.Count & " ")
        For Each lSegment As Segment In Me
            lString.Append(lSegment.Name.PadRight(16) & " ")
        Next

        lString.Append("C Seg Number   Seg Length  Seg Width  DMult   Vmult  SegSlope  SegRough  DownSeg  InflowTS ")

        For Each lSegment As Segment In Me
            With lSegment
                lString.Append(.ID.PadRight(16) & " ")
                lString.Append(Format(.Length, "0.00").PadRight(10) & " ")
                lString.Append(Format(.Width, "0.00").PadRight(10) & " ")
                lString.Append(Format(.Dmult, "0.00").PadRight(10) & " ")
                lString.Append(Format(.Vmult, "0.00").PadRight(10) & " ")
                lString.Append(.Slope.PadRight(10) & " ")
                lString.Append(Format(.Roughness, "0.000").PadRight(10) & " ")
                lString.AppendLine(.DownID.PadRight(16))
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
    Public Slope As String = "0.05"
    Public Roughness As Double = 0.0
    Public InflowTimeseriesID As String = ""
End Class
