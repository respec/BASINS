Imports System.Collections.Specialized
Imports MapWindow.Interfaces
Imports MapWinUtility

Imports atcUtility
Imports atcData
Imports atcSeasons

Module ScriptSummarizeTimeseries
    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        Logger.Dbg("SummarizeTimeseriesStart:CurDir:" & CurDir())

        Dim lSeasonNames As New ArrayList
        lSeasonNames.Add("DJF")
        lSeasonNames.Add("MAM")
        lSeasonNames.Add("JJA")
        lSeasonNames.Add("SON")

        Dim lSeasonalAttributeNames As New ArrayList
        lSeasonalAttributeNames.Add("Mean")
        lSeasonalAttributeNames.Add("SumAnnual")

        Dim lAttributes As New atcCollection
        With lAttributes
            .Add("ID", True)
            .Add("Location", True)
            .Add("Constituent", True)
            .Add("Start Date", True)
            .Add("End Date", True)
            .Add("Count", True)
            .Add("Mean", True)
            .Add("SumAnnual", True)
            .Add("Geometric Mean", True)
            .Add("Minimum", True)
            .Add("Maximum", True)
        End With
        Dim lAsk As New frmArgs
        If lAsk.AskUser("Attributes", lAttributes) Then
            Dim lTimeseriesDataGroup As atcTimeseriesGroup = atcDataManager.DataSets
            SaveFileString("SummarizeTimeseries.txt", _
                           SummarizeTimeseries(lAttributes, lTimeseriesDataGroup, True, lSeasonalAttributeNames, lSeasonNames))
            Logger.Dbg("SummarizeTimeseriesDone")
        Else
            Logger.Dbg("SummarizeTimeseriesCancelled")
        End If
    End Sub

    Friend Function SummarizeTimeseries(ByVal aAttributes As atcCollection, _
                                        ByVal aTimeseriesDatagroup As atcTimeseriesGroup, _
                               Optional ByVal aIncludeHeader As Boolean = True, _
                               Optional ByVal aSeasonalAttributeNames As ArrayList = Nothing, _
                               Optional ByVal aSeasonNames As ArrayList = Nothing, _
                               Optional ByVal aConstituents As ArrayList = Nothing) As String
        Dim lAttributes As atcCollection = aAttributes.Clone
        For Each lSeasonalAttributeName As String In aSeasonalAttributeNames
            If lAttributes.ItemByKey(lSeasonalAttributeName) Then
                For Each lSeasonName As String In aSeasonNames
                    lAttributes.Insert(lAttributes.Keys.IndexOf(lSeasonalAttributeName) + 1, lSeasonName & "-" & lSeasonalAttributeName, True)
                Next
            End If
        Next
        Dim lD2SStart As New atcDateFormat
        lD2SStart.IncludeHours = True
        lD2SStart.IncludeMinutes = True
        lD2SStart.Midnight24 = False
        Dim lD2SEnd As New atcDateFormat
        lD2SEnd.IncludeHours = True
        lD2SEnd.IncludeMinutes = True

        Dim lStringBuilder As New Text.StringBuilder
        Dim lString As String = ""
        If aIncludeHeader Then
            For Each lAttribute As String In lAttributes.Keys
                lString &= lAttribute & vbTab
            Next
            lString.Trim(vbTab)
            lStringBuilder.AppendLine(lString)
        End If

        Logger.Dbg("DatasetCount " & aTimeseriesDatagroup.Count)
        For Each lTimeseries As atcTimeseries In aTimeseriesDatagroup
            Dim lConstituent As String = lTimeseries.Attributes.GetValue("CONS", "?")
            If aConstituents Is Nothing OrElse aConstituents.Contains(lConstituent) Then
                For Each lSeasonName As String In aSeasonNames
                    Dim lSeasons As atcSeasonBase = SeasonsMonthFromString(lSeasonName)
                    Dim lSeasonTimeseries As atcTimeseries = lSeasons.SplitBySelected(lTimeseries, Nothing)(0)
                    With lTimeseries.Attributes
                        For Each lSeasonalAttributeName As String In aSeasonalAttributeNames
                            .SetValue(lSeasonName & "-" & lSeasonalAttributeName, lSeasonTimeseries.Attributes.GetValue(lSeasonalAttributeName))
                        Next
                    End With
                Next
                Dim lValueString As String
                lString = ""
                For Each lAttribute As String In lAttributes.Keys
                    If lAttributes.ItemByKey(lAttribute) Then
                        lValueString = lTimeseries.Attributes.GetValue(lAttribute, "?")
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
                                        lString &= DecimalAlign(lValueString)
                                    Else
                                        lString &= lValueString
                                    End If
                            End Select
                        End If
                        lString &= vbTab
                    End If
                Next
                lString.Trim(vbTab)
                lStringBuilder.AppendLine(lString)
            End If
        Next
        Return lStringBuilder.ToString
    End Function

    Private Function SeasonsMonthFromString(ByVal aSeasonText As String) As atcSeasonBase
        Dim lSeasonsMonth As New atcSeasonsMonth
        Select Case aSeasonText
            Case "DJF"
                lSeasonsMonth.SeasonSelected(12) = True
                lSeasonsMonth.SeasonSelected(1) = True
                lSeasonsMonth.SeasonSelected(2) = True
            Case "MAM"
                lSeasonsMonth.SeasonSelected(3) = True
                lSeasonsMonth.SeasonSelected(4) = True
                lSeasonsMonth.SeasonSelected(5) = True
            Case "JJA"
                lSeasonsMonth.SeasonSelected(6) = True
                lSeasonsMonth.SeasonSelected(7) = True
                lSeasonsMonth.SeasonSelected(8) = True
            Case "SON"
                lSeasonsMonth.SeasonSelected(9) = True
                lSeasonsMonth.SeasonSelected(10) = True
                lSeasonsMonth.SeasonSelected(11) = True
        End Select
        Return lSeasonsMonth
    End Function
End Module
