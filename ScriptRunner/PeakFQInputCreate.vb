Imports System.Collections.Specialized
Imports MapWindow.Interfaces
Imports MapWinUtility

Imports atcUtility
Imports atcData

Module ScriptPeakFQInputCreate
    Private Const pFormat As String = "#,###,###,##0.00"
    Private Const pTestPath As String = "D:\MountainViewData\SantaClaraResultsWDM"
    Private Const pWDMFileName As String = "SCRWS.wdm"
    Private Const pSummarizeData As Boolean = False

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        ChDriveDir(pTestPath)
        Logger.Dbg("ScriptPeakFQInputCreate:CurDir:" & CurDir())

        Dim lWDM As New atcWDM.atcDataSourceWDM
        lWDM.Open(pWDMFileName)
        Dim lDataSets As atcDataGroup = lWDM.DataSets
        Logger.Dbg("DatasetCount " & lDataSets.Count)

        Dim lStringBuilder As Text.StringBuilder

        If pSummarizeData Then
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

            lStringBuilder = New Text.StringBuilder
            Dim lString As String = ""
            For Each lAttribute As String In lAttributes
                lString &= lAttribute & vbTab
            Next
            lString.Trim(vbTab)
            lStringBuilder.AppendLine(lString)

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
            SaveFileString("PeakFQInput.txt", lStringBuilder.ToString)
        End If

        Dim lNewWDM As New atcWDM.atcDataSourceWDM
        lNewWDM.Open("Annual.wdm")
        Dim lFreqStringBuilder As New Text.StringBuilder
        lStringBuilder = New Text.StringBuilder
        For Each lDS As atcDataSet In lDataSets
            Dim lConstituent As String
            lConstituent = lDS.Attributes.GetValue("Constituent", "?")
            If lConstituent.ToLower.Contains("flow") Then
                Logger.Dbg("Flow Found DSN " & lDS.Attributes.GetValue("ID", "?"))
                Dim lCalculator As New atcTimeseriesNdayHighLow.atcTimeseriesNdayHighLow
                Dim lArgs As New atcDataAttributes
                lArgs.SetValue("Timeseries", lDS)
                lArgs.SetValue("NDay", "1")
                If lCalculator.Open("n-day high timeseries", lArgs) Then
                    Dim lTimeseries As atcTimeseries = lCalculator.DataSets(0)
                    If lTimeseries.numValues > 0 Then
                        Logger.Dbg(lTimeseries.ToString & " " & lTimeseries.Value(1))
                        Dim lStaNum As String = lDS.Attributes.GetValue("location", "?").ToString.PadLeft(8)
                        If lStringBuilder.Length > 0 Then
                            lStringBuilder.Remove(0, lStringBuilder.Length)
                        End If
                        lStringBuilder.AppendLine(lStaNum)
                        With lFreqStringBuilder
                            .AppendLine("*")
                            .AppendLine("Z" & Space(32) & "ATC-HSPF Results")
                            .AppendLine("H " & lStaNum)
                            .AppendLine("N " & lStaNum)
                            .AppendLine("Y " & lStaNum)
                            .AppendLine("2 " & lStaNum)
                            For lIndex As Integer = 1 To lTimeseries.numValues
                                Dim lPeakDateArray(5) As Integer
                                J2Date(lTimeseries.ValueAttributes(lIndex).GetDefinedValue("PeakDate").Value, lPeakDateArray)
                                Dim lPeakDateString As String = lPeakDateArray(0) & lPeakDateArray(1).ToString.PadLeft(2, "0") & lPeakDateArray(2).ToString.PadLeft(2, "0")
                                .AppendLine("3 " & lStaNum & Space(6) & lPeakDateString & DoubleToString(lTimeseries.Value(lIndex), 7, "######0").PadLeft(7))
                                lStringBuilder.AppendLine(lPeakDateString & vbTab & DoubleToString(lTimeseries.Value(lIndex), 7, "######0").PadLeft(7))
                            Next
                        End With
                        SaveFileString("pks_" & lStaNum.Trim & ".txt", lStringBuilder.ToString)
                        If lNewWDM.AddDataset(lTimeseries, atcDataSource.EnumExistAction.ExistRenumber) Then
                            Logger.Dbg("Save timeseries " & lTimeseries.ToString)
                        Else
                            Logger.Dbg("Problem writing timeseries " & lTimeseries.ToString)
                        End If
                    Else
                        Logger.Dbg("Skip:NoOutput")
                    End If
                Else
                    Logger.Dbg("Skip")
                End If
            End If
        Next
        SaveFileString("PeakFQInput.inp", lFreqStringBuilder.ToString)
        Logger.Dbg("PeakFQInputCreateDone")
    End Sub
    '* WCF2.DATA  1/9/89 -- BULLETIN 17 EXAMPLES
    '*---+----1----+----2----+----3----+----4----+----5----+----6----+----7----+----8

    'Z                               USGS
    'H 03606500      3602190881342004747017SW 6040005 205.00         380.58
    'N 03606500      BIG SANDY RIVER AT BRUCETON, TENN
    'Y 03606500      2000.00
    '2 03606500
    '3 03606500      189703    250007              18.00
    '3 03606500      191903    210007 
End Module
