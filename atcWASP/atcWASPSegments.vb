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

        lString.Append(";Name            ID               Length     Width      Depth      Slope      Roughness  DownSegID        InflowTS        " & vbCrLf)
        lString.Append(";_______________ ________________ __________ __________ __________ __________ __________ ________________ ________________" & vbCrLf)

        For Each lSegment As Segment In Me
            With lSegment
                lString.Append(.Name.PadRight(16) & " ")
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
End Class
