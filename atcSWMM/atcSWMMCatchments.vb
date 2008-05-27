Imports System.Collections.ObjectModel
Imports System.IO
Imports MapWinUtility
Imports atcUtility
Imports System.Text

Public Class Catchments
    Inherits KeyedCollection(Of String, Catchment)
    Protected Overrides Function GetKeyForItem(ByVal aCatchment As Catchment) As String
        Dim lKey As String = aCatchment.Name
        Return lKey
    End Function

    Public SWMMProject As SWMMProject

    Public Overrides Function ToString() As String
        Dim lSB As New StringBuilder

        lSB.Append("[SUBCATCHMENTS]" & vbCrLf & _
                       ";;                                                 Total    Pcnt.             Pcnt.    Curb     Snow    " & vbCrLf & _
                       ";;Name           Raingage         Outlet           Area     Imperv   Width    Slope    Length   Pack    " & vbCrLf & _
                       ";;-------------- ---------------- ---------------- -------- -------- -------- -------- -------- --------" & vbCrLf)

        For Each lCatchment As Catchment In Me
            With lCatchment
                lSB.Append(StrPad(.Name, 16, " ", False))
                lSB.Append(" ")
                If Not .RainGage Is Nothing Then
                    lSB.Append(StrPad(.RainGage.Name, 16, " ", False))
                Else
                    lSB.Append(StrPad("R1", 16, " ", False))
                End If
                lSB.Append(" ")
                If Not .Conduit Is Nothing Then
                    lSB.Append(StrPad(.Conduit.OutletNode.Name, 16, " ", False))
                Else
                    lSB.Append(StrPad("J1", 16, " ", False))
                End If
                lSB.Append(" ")
                lSB.Append(StrPad(Format(.Area, "0.0"), 8, " ", False))
                lSB.Append(" ")
                lSB.Append(StrPad(Format(.PercentImpervious, "0.0"), 8, " ", False))
                lSB.Append(" ")
                lSB.Append(StrPad(Format(.Width, "0.0"), 8, " ", False))
                lSB.Append(" ")
                lSB.Append(StrPad(Format(.Slope, "0.0"), 8, " ", False))
                lSB.Append(" ")
                lSB.Append(StrPad(Format(.CurbLength, "0.0"), 8, " ", False))
                lSB.Append(" ")
                lSB.Append(.SnowPackName)
                lSB.Append(vbCrLf)
            End With
        Next

        Return lSB.ToString
    End Function

    Public Function PolygonsToString() As String
        Dim lSB As New StringBuilder
        lSB.Append("[Polygons]" & vbCrLf & _
                       ";;Subcatchment   X-Coord            Y-Coord           " & vbCrLf & _
                       ";;-------------- ------------------ ------------------" & vbCrLf)

        For Each lCatchment As Catchment In Me
            With lCatchment
                For lIndex As Integer = 0 To .X.GetUpperBound(0) - 1
                    lSB.Append(StrPad(.Name, 16, " ", False))
                    lSB.Append(" ")
                    lSB.Append(StrPad(Format(.X(lIndex), "0.000"), 18, " ", False))
                    lSB.Append(" ")
                    lSB.Append(StrPad(Format(.Y(lIndex), "0.000"), 18, " ", False))
                    lSB.Append(vbCrLf)
                Next
            End With
        Next

        Return lSB.ToString
    End Function
End Class

Public Class Catchment
    Public Name As String
    Public RainGage As RainGage
    Public Conduit As Conduit
    Public Area As Double = 0.0 'in acres or hectares
    Public PercentImpervious As Double = 0.0
    Public Width As Double = 0.0 'in feet or meters
    Public Slope As Double = 0.0 'percent
    Public CurbLength As Double = 0.0
    Public SnowPackName As String = "" 'blank if none
    Public X() As Double
    Public Y() As Double
End Class

