Imports System.Collections.ObjectModel
Imports System.IO
Imports MapWinUtility
Imports atcUtility
Imports System.Text

Public Class Nodes
    Inherits KeyedCollection(Of String, Node)
    Protected Overrides Function GetKeyForItem(ByVal aNode As Node) As String
        Dim lKey As String = aNode.Name
        Return lKey
    End Function

    Public Sub AddRange(ByVal aEnumerable As IEnumerable)
        For Each lNode As Node In aEnumerable
            Me.Add(lNode)
        Next
    End Sub

    Public Overrides Function ToString() As String
        Dim lString As New StringBuilder

        lString.Append("[JUNCTIONS]" & vbCrLf & _
                       ";;               Invert     Max.       Init.      Surcharge  Ponded    " & vbCrLf & _
                       ";;Name           Elev.      Depth      Depth      Depth      Area      " & vbCrLf & _
                       ";;-------------- ---------- ---------- ---------- ---------- ----------" & vbCrLf)

        For Each lNode As Node In Me
            If lNode.Type = "JUNCTION" Then
                With lNode
                    lString.Append(StrPad(.Name, 16, " ", False))
                    lString.Append(" ")
                    lString.Append(StrPad(Format(.InvertElevation, "0.0"), 10, " ", False))
                    lString.Append(" ")
                    lString.Append(StrPad(Format(.MaxDepth, "0.0"), 10, " ", False))
                    lString.Append(" ")
                    lString.Append(StrPad(Format(.InitDepth, "0.0"), 10, " ", False))
                    lString.Append(" ")
                    lString.Append(StrPad(Format(.SurchargeDepth, "0.0"), 10, " ", False))
                    lString.Append(" ")
                    lString.Append(StrPad(Format(.PondedArea, "0.0"), 10, " ", False))
                    lString.Append(vbCrLf)
                End With
            End If
        Next

        lString.Append(vbCrLf)
        lString.Append("[OUTFALLS]" & vbCrLf & _
                       ";;               Invert     Outfall    Stage/Table      Tide" & vbCrLf & _
                       ";;Name           Elev.      Type       Time Series      Gate" & vbCrLf & _
                       ";;-------------- ---------- ---------- ---------------- ----" & vbCrLf)

        For Each lNode As Node In Me
            If lNode.Type = "OUTFALL" Then
                With lNode
                    lString.Append(StrPad(.Name, 16, " ", False))
                    lString.Append(" ")
                    lString.Append(StrPad(Format(.InvertElevation, "0.0"), 10, " ", False))
                    lString.Append(" ")
                    lString.Append(StrPad(.OutfallType, 10, " ", False))
                    lString.Append(" ")
                    lString.Append(StrPad(.StageTable, 16, " ", False))
                    lString.Append(" ")
                    lString.Append(StrPad(.TideGate, 4, " ", False))
                    lString.Append(vbCrLf)
                End With
            End If
        Next

        Return lString.ToString
    End Function

    Public Function CoordinatesToString() As String
        Dim lString As New StringBuilder

        lString.Append("[COORDINATES]" & vbCrLf & _
                       ";;Node           X-Coord            Y-Coord           " & vbCrLf & _
                       ";;-------------- ------------------ ------------------" & vbCrLf)

        For Each lNode As Node In Me
            With lNode
                lString.Append(StrPad(.Name, 16, " ", False))
                lString.Append(" ")
                lString.Append(StrPad(Format(.XPos, "0.000"), 18, " ", False))
                lString.Append(" ")
                lString.Append(StrPad(Format(.YPos, "0.000"), 18, " ", False))
                lString.Append(vbCrLf)
            End With
        Next

        Return lString.ToString
    End Function
End Class

Public Class Node
    Public Name As String
    Public Type As String = "" 'junction or outfall
    Public InvertElevation As Double 'in feet or meters
    Public MaxDepth As Double = 0.0
    Public InitDepth As Double = 0.0
    Public SurchargeDepth As Double = 0.0
    Public PondedArea As Double = 0.0
    Public OutfallType As String = "FREE"
    Public StageTable As String = ""
    Public TideGate As String = "NO"
    Public YPos As Double = 0.0
    Public XPos As Double = 0.0
End Class
