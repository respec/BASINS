Imports atcData
Imports atcData.atcDataSource.EnumExistAction
Imports atcUtility
Imports MapWindow.Interfaces
Imports MapWinUtility

Imports Microsoft.VisualBasic
Imports System.Collections
Imports System.IO

Public Module ScriptStatTest
    Private Const pFormat As String = "#,###,###,##0.00"

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        Logger.Dbg("SummarizeTimeseriesStart:CurDir:" & CurDir())

        Dim lAttributes As New atcCollection
        With lAttributes
            .Add("ID")
            .Add("STANAM")
            .Add("Constituent")
            .Add("Start Date")
            .Add("End Date")
            .Add("Count")
            .Add("Mean")
            .Add("Geometric Mean")
            .Add("Minimum")
            .Add("Maximum")
            .Add("7Q10")
            .Add("30Low2")
            .Add("365Low10")
            .Add("1High100")
        End With

        Dim lD2SStart As New atcDateFormat
        lD2SStart.IncludeHours = False
        lD2SStart.IncludeMinutes = False
        lD2SStart.Midnight24 = False
        Dim lD2SEnd As New atcDateFormat
        lD2SEnd.IncludeHours = False
        lD2SEnd.IncludeMinutes = False

        Dim lStringBuilder As New Text.StringBuilder
        Dim lString As String = ""
        For Each lAttribute As String In lAttributes
            lString &= lAttribute & vbTab
        Next
        lString.Trim(vbTab)
        lStringBuilder.AppendLine(lString)

        Dim lTsMath As atcDataSource = New atcTimeseriesMath.atcTimeseriesMath
        'Dim lHighLowSource As atcDataSource = New atcTimeseriesNdayHighLow.atcTimeseriesNdayHighLow
        Dim lArgsMath As New atcDataAttributes

        Dim lDataSets As atcDataGroup = atcDataManager.DataSets
        Dim lDataGroup As New atcDataGroup
        Logger.Dbg("DatasetCount " & lDataSets.Count)
        For Each lDS As atcDataSet In lDataSets
            Dim lID As Integer = lDS.Attributes.GetValue("Id")
            'If lID = 4 Or lID = 15 Or lID = 30 Then '  Or lID = 8 Then
            lStringBuilder.AppendLine(Summary(lDS, lAttributes, lD2SStart, lD2SEnd))
            Dim lNumYears As Integer = 16
            Dim lYearStartFirst As Integer = 1958
            Dim lYearStartLast As Integer = 1991 '1992+15=2007
            For lYearStart As Integer = lYearStartFirst To lYearStartLast
                lTsMath.DataSets.Clear()
                lArgsMath.Clear()
                lArgsMath.SetValue("timeseries", lDS)
                Dim lDateString = CStr(lYearStart) & "/4/1"
                lArgsMath.SetValue("Start Date", lDateString)
                lDateString = CStr(lYearStart + lNumYears) & "/4/1"
                lArgsMath.SetValue("End Date", lDateString)
                lArgsMath.SetValue("LogFlg", False)
                lTsMath.Open("subset by date", lArgsMath)
                lStringBuilder.AppendLine(Summary(lTsMath.DataSets(0), lAttributes, lD2SStart, lD2SEnd))
            Next lYearStart
            'End If
        Next
        SaveFileString("SummarizeTimeseries.txt", lStringBuilder.ToString)
        Logger.Dbg("SummarizeTimeseriesDone")
    End Sub

    Private Function Summary(ByVal aDs As atcDataSet, _
                             ByVal aAttributes As atcCollection, _
                             ByVal aD2SStart As atcDateFormat, _
                             ByVal aD2SEnd As atcDateFormat) As String

        Dim lValueString As String
        Dim lString As String = ""
        For Each lAttribute As String In aAttributes
            lValueString = aDs.Attributes.GetValue(lAttribute, "?")
            If lValueString <> "?" Then
                Select Case lAttribute
                    Case "Start Date"
                        lString &= "'" & aD2SStart.JDateToString(lValueString) & "'"
                    Case "End Date"
                        lString &= "'" & aD2SEnd.JDateToString(lValueString) & "'"
                    Case "Count", "ID"
                        lString &= lValueString
                    Case Else
                        If IsNumeric(lValueString) Then
                            lString &= DoubleToString(lValueString, , pFormat).TrimEnd("0").TrimEnd(".")
                        Else
                            lString &= lValueString
                        End If
                End Select
            Else
                lString &= lValueString
            End If
            lString &= vbTab
        Next
        lString.Trim(vbTab)
        Return lString
    End Function
End Module
