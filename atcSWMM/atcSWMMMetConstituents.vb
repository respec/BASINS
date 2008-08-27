Imports System.Collections.ObjectModel
Imports System.IO
Imports MapWinUtility
Imports atcUtility
Imports System.Text

Public Class MetConstituents
    Inherits KeyedCollection(Of String, MetConstituent)
    Protected Overrides Function GetKeyForItem(ByVal aMetConstituent As MetConstituent) As String
        Dim lKey As String = aMetConstituent.Type
        Return lKey
    End Function

    Public SWMMProject As SWMMProject

    Public Overrides Function ToString() As String
        Dim lSB As New StringBuilder

        Dim lFoundEvap As Boolean = False
        For Each lMetConstituent As MetConstituent In Me
            If lMetConstituent.Type = "EVAP" Or lMetConstituent.Type = "PEVT" Then

                lSB.Append("[EVAPORATION]" & vbCrLf & _
                           ";;Type       Parameters" & vbCrLf & _
                           ";;---------- ----------" & vbCrLf)

                With lMetConstituent
                    lSB.Append(StrPad("TIMESERIES", 12, " ", False))
                    lSB.Append(" ")
                    lSB.Append(StrPad(.TimeSeries.Attributes.GetValue("Location") & ":E", 10, " ", False))
                    lSB.Append(" ")
                    lSB.Append(vbCrLf)
                End With
                lFoundEvap = True
                Exit For
            End If
        Next

        For Each lMetConstituent As MetConstituent In Me
            If lMetConstituent.Type = "ATEM" Or lMetConstituent.Type = "ATMP" Then
                If lFoundEvap Then
                    lSB.Append(vbCrLf)
                End If

                lSB.Append("[TEMPERATURE]" & vbCrLf)

                With lMetConstituent
                    lSB.Append(StrPad("TIMESERIES", 12, " ", False))
                    lSB.Append(" ")
                    lSB.Append(StrPad(.TimeSeries.Attributes.GetValue("Location") & ":T", 10, " ", False))
                    lSB.Append(" ")
                    lSB.Append(vbCrLf)
                End With
                Exit For
            End If
        Next

        Return lSB.ToString
    End Function

    Public Function TimeSeriesHeaderToString() As String
        Dim lSB As New StringBuilder
        lSB.Append("[TIMESERIES]" & vbCrLf & _
                   ";;Name           Date       Time       Value     " & vbCrLf & _
                   ";;-------------- ---------- ---------- ----------")
        Return lSB.ToString
    End Function

    Public Function TimeSeriesToStream(ByVal aSW As IO.StreamWriter) As String

        Dim lFoundEvap As Boolean = False
        For Each lMetConstituent As MetConstituent In Me
            If lMetConstituent.Type = "EVAP" Or lMetConstituent.Type = "PEVT" Then
                aSW.Write(";EVAPORATION" & vbCrLf)
                Me.SWMMProject.TimeSeriesToStream(lMetConstituent.TimeSeries, lMetConstituent.TimeSeries.Attributes.GetValue("Location") & ":E", aSW)
                lFoundEvap = True
                Exit For
            End If
        Next

        For Each lMetConstituent As MetConstituent In Me
            If lMetConstituent.Type = "ATEM" Or lMetConstituent.Type = "ATMP" Then
                If lFoundEvap Then
                    aSW.Write(vbCrLf)
                End If
                aSW.Write(";TEMPERATURE" & vbCrLf)
                Me.SWMMProject.TimeSeriesToStream(lMetConstituent.TimeSeries, lMetConstituent.TimeSeries.Attributes.GetValue("Location") & ":T", aSW)
            End If
        Next

        Return aSW.ToString
    End Function
End Class

Public Class MetConstituent
    Public Type As String 'Evap or Temp
    Public TimeSeries As atcData.atcTimeseries
End Class
