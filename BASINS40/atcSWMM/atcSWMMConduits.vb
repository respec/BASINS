Imports System.Collections.ObjectModel
Imports System.IO
Imports MapWinUtility
Imports atcUtility
Imports System.Text

Public Class Conduits
    Inherits KeyedCollection(Of String, Conduit)
    Protected Overrides Function GetKeyForItem(ByVal aConduit As Conduit) As String
        Dim lKey As String = aConduit.Name
        Return lKey
    End Function

    Public SWMMProject As SWMMProject

    Public Overrides Function ToString() As String
        Dim lString As New StringBuilder

        lString.Append("[CONDUITS]" & vbCrLf & _
                       ";;               Inlet            Outlet                      Manning    Inlet      Outlet     Init.      Max.      " & vbCrLf & _
                       ";;Name           Node             Node             Length     N          Offset     Offset     Flow       Flow      " & vbCrLf & _
                       ";;-------------- ---------------- ---------------- ---------- ---------- ---------- ---------- ---------- ----------" & vbCrLf)

        For Each lConduit As Conduit In Me
            With lConduit
                lString.Append(StrPad(.Name, 16, " ", False))
                lString.Append(" ")
                lString.Append(StrPad(.InletNode.Name, 16, " ", False))
                lString.Append(" ")
                lString.Append(StrPad(.OutletNode.Name, 16, " ", False))
                lString.Append(" ")
                lString.Append(StrPad(Format(.Length, "0.0"), 10, " ", False))
                lString.Append(" ")
                lString.Append(StrPad(Format(.ManningsN, "0.00"), 10, " ", False))
                lString.Append(" ")
                lString.Append(StrPad(Format(.InletOffset, "0.0"), 10, " ", False))
                lString.Append(" ")
                lString.Append(StrPad(Format(.OutletOffset, "0.0"), 10, " ", False))
                lString.Append(" ")
                lString.Append(StrPad(Format(.InitialFlow, "0.0"), 10, " ", False))
                lString.Append(" ")
                lString.Append(StrPad(Format(.MaxFlow, "0.0"), 10, " ", False))
                lString.Append(vbCrLf)
            End With
        Next

        Return lString.ToString
    End Function

    Public Function VerticesToString() As String
        Dim lSB As New StringBuilder
        lSB.Append("[VERTICES]" & vbCrLf & _
                       ";;Link           X-Coord            Y-Coord           " & vbCrLf & _
                       ";;-------------- ------------------ ------------------" & vbCrLf)

        For Each lConduit As Conduit In Me
            With lConduit
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

Public Class Conduit
    Public Name As String
    Public FeatureIndex As Integer
    Public InletNode As Node
    Public OutletNode As Node
    Public Length As Double 'in feet or meters
    Public ManningsN As Double = 0.05
    Public InletOffset As Double = 0.0
    Public OutletOffset As Double = 0.0
    Public InitialFlow As Double = 0.0
    Public MaxFlow As Double = 0.0
    Public X() As Double
    Public Y() As Double
End Class
