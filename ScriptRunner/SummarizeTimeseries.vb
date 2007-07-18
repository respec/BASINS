Imports System.Collections.Specialized
Imports MapWindow.Interfaces
Imports MapWinUtility

Imports atcUtility
Imports atcData

Module ScriptSummarizeTimeseries
    Private Const pFormat As String = "#,###,###,##0.00"

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        Logger.Dbg("SummarizeTimeseriesStart:CurDir:" & CurDir())

        Dim lAttributes As New atcCollection
        With lAttributes
            .Add("ID")
            .Add("Location")
            .Add("Constituent")
            .Add("Start Date")
            .Add("End Date")
            .Add("Count")
            .Add("Mean")
            .Add("Geometric Mean")
            .Add("Minimum")
            .Add("Maximum")
        End With

        Dim lD2SStart As New atcDateFormat
        lD2SStart.IncludeHours = True
        lD2SStart.IncludeMinutes = True
        lD2SStart.Midnight24 = False
        Dim lD2SEnd As New atcDateFormat
        lD2SEnd.IncludeHours = True
        lD2SEnd.IncludeMinutes = True

        Dim lStringBuilder As New Text.StringBuilder
        Dim lString As String = ""
        For Each lAttribute As String In lAttributes
            lString &= lAttribute & vbTab
        Next
        lString.Trim(vbTab)
        lStringBuilder.AppendLine(lString)

        Dim lDataSets As atcDataGroup = atcDataManager.DataSets
        Logger.Dbg("DatasetCount " & lDataSets.Count)
        For Each lDS As atcDataSet In lDataSets
            Dim lValueString As String
            lString = ""
            For Each lAttribute As String In lAttributes
                lValueString = lDS.Attributes.GetValue(lAttribute, "?")
                If lValueString = "?" Then 'no value available for this attribute
                    lString &= lValueString
                Else
                    Select Case lAttribute
                        Case "Start Date"
                            lString &= "'" & lD2SStart.JDateToString(lValueString) & "'"
                        Case "End Date"
                            lString &= "'" & lD2SEnd.JDateToString(lValueString) & "'"
                        Case "Count", "ID"
                            lString &= lValueString
                        Case Else
                            If IsNumeric(lValueString) Then
                                lString &= DoubleToString(lValueString, , pFormat).TrimEnd("0").TrimEnd(".")
                            Else
                                lString &= lValueString
                            End If
                    End Select
                End If
                lString &= vbTab
            Next
            lString.Trim(vbTab)
            lStringBuilder.AppendLine(lString)
        Next
        SaveFileString("SummarizeTimeseries.txt", lStringBuilder.ToString)
        Logger.Dbg("SummarizeTimeseriesDone")
    End Sub
End Module
