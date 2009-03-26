Imports System.Collections.ObjectModel
Imports System.IO
Imports MapWinUtility
Imports atcUtility
Imports System.Text

Public Class WASPTimeseriesCollection
    Inherits KeyedCollection(Of String, WASPTimeseries)
    Protected Overrides Function GetKeyForItem(ByVal aWASPTimeseries As WASPTimeseries) As String
        Dim lKey As String = aWASPTimeseries.Type & ":" & aWASPTimeseries.Description
        Return lKey
    End Function

    Public Function TimeSeriesToFile(ByVal aBaseFileName As String, ByVal aSJDate As Double, ByVal aEJDate As Double) As Boolean
        For Each lWASPTimeseries As WASPTimeseries In Me
            If lWASPTimeseries.TimeSeries Is Nothing Then
                Dim lLocation As String = lWASPTimeseries.Identifier
                Dim lFileName As String = PathNameOnly(aBaseFileName) & "\" & lLocation & "." & lWASPTimeseries.Type & ".DAT"
                Dim lSB As New StringBuilder
                lSB.Append(lWASPTimeseries.ConstantToString(aSJDate, aEJDate))
                SaveFileString(lFileName, lSB.ToString)
            Else
                Dim lLocation As String = lWASPTimeseries.TimeSeries.Attributes.GetDefinedValue("Location").Value
                Dim lFileName As String = PathNameOnly(aBaseFileName) & "\" & lLocation & "." & lWASPTimeseries.Type & ".DAT"
                Dim lSB As New StringBuilder
                lSB.Append(lWASPTimeseries.TimeSeriesToString(aSJDate, aEJDate))
                SaveFileString(lFileName, lSB.ToString)
            End If
        Next
    End Function

End Class

Public Class WASPTimeseries
    Public Identifier As String  'location 
    Public Type As String 'flow, air temp, solrad, water temp, etc.
    Public SDate As Double
    Public EDate As Double
    Public TimeSeries As atcData.atcTimeseries
    Public Description As String
    Public ID As Integer  'dsn if wdm
    Public DataSourceName As String
    Public LocationX As Double
    Public LocationY As Double

    Public Function TimeSeriesToString(ByVal aSJDate As Double, ByVal aEJDate As Double) As String
        Dim lStartIndex As Integer = Me.TimeSeries.Dates.IndexOfValue(aSJDate, True)
        If aSJDate = Me.TimeSeries.Dates.Values(0) Or lStartIndex < 0 Then
            lStartIndex = 0
        End If
        Dim lEndIndex As Integer = Me.TimeSeries.Dates.IndexOfValue(aEJDate, True)
        Dim lSB As New StringBuilder
        For lIndex As Integer = lStartIndex To lEndIndex - 1
            Dim lJDate As Double = Me.TimeSeries.Dates.Values(lIndex)
            Dim lDate(6) As Integer
            J2Date(lJDate, lDate)
            Dim lDateString As String = lDate(1) & "/" & lDate(2) & "/" & lDate(0)
            Dim lTimeString As String = lDate(3).ToString.PadLeft(2, "0") & ":" & lDate(4).ToString.PadLeft(2, "0")
            lSB.Append(StrPad(lDateString, 10, " ", False))
            lSB.Append(" ")
            lSB.Append(StrPad(lTimeString, 10, " ", False))
            lSB.Append(" ")
            lSB.Append(StrPad(Format(Me.TimeSeries.Values(lIndex + 1), "0.000"), 10, " ", False))
            lSB.Append(vbCrLf)
        Next

        Return lSB.ToString
    End Function

    Public Function ConstantToString(ByVal aSJDate As Double, ByVal aEJDate As Double) As String
        Dim lSB As New StringBuilder

        'for the constant case, the mean annual flow is stored in the Me.DataSourceName
        Dim lValue As Double = 0.0
        If IsNumeric(Me.DataSourceName) Then
            lValue = CDbl(Me.DataSourceName)
        End If

        Dim lDate(6) As Integer
        J2Date(aSJDate, lDate)
        Dim lDateString As String = lDate(1) & "/" & lDate(2) & "/" & lDate(0)
        Dim lTimeString As String = lDate(3).ToString.PadLeft(2, "0") & ":" & lDate(4).ToString.PadLeft(2, "0")
        lSB.Append(StrPad(lDateString, 10, " ", False))
        lSB.Append(" ")
        lSB.Append(StrPad(lTimeString, 10, " ", False))
        lSB.Append(" ")
        lSB.Append(StrPad(Format(lValue, "0.000"), 10, " ", False))
        lSB.Append(vbCrLf)

        J2Date(aEJDate, lDate)
        lDateString = lDate(1) & "/" & lDate(2) & "/" & lDate(0)
        lTimeString = lDate(3).ToString.PadLeft(2, "0") & ":" & lDate(4).ToString.PadLeft(2, "0")
        lSB.Append(StrPad(lDateString, 10, " ", False))
        lSB.Append(" ")
        lSB.Append(StrPad(lTimeString, 10, " ", False))
        lSB.Append(" ")
        lSB.Append(StrPad(Format(lValue, "0.000"), 10, " ", False))
        lSB.Append(vbCrLf)

        Return lSB.ToString
    End Function
End Class
