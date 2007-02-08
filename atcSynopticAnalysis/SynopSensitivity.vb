Imports atcData
Imports atcUtility
Imports atcControls
Imports atcSynopticAnalysis
Imports MapWindow.Interfaces
Imports MapWinUtility

Imports Microsoft.VisualBasic
Imports System.Collections
Imports System.IO
Imports System.Windows.Forms
Imports System.Array

Public Module ScriptSynop
    Private Const pTestPath As String = "d:\Basins\data\ClimateCbp\"

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        ChDriveDir(pTestPath)
        Logger.Dbg(" CurDir:" & CurDir)

        Dim lErr As String = ""
        Dim lDataManager As atcDataManager = Scripting.Run("vb", "", "subFindDataManager.vb", lErr, False, aMapWin, aMapWin)

        Dim lWDMfile As atcDataSource = New atcWdm.atcDataSourceWdm
        Dim lSummary As New atcDataTree.atcDataTreePlugin

        Dim lWdmName As String = "base.wdm"
        lDataManager.OpenDataSource(lWDMfile, lWdmName, Nothing)
        Logger.Dbg(" Opened: " & lWdmName)
        Logger.Dbg("   CountWDM: " & lWDMfile.DataSets.Count)
        Logger.Dbg("   CountManager: " & lDataManager.DataSets.Count)

        Dim lDsns() As Integer = {2100, 2700}
        Dim lGroupNames() As String = {"One Group"}
        Dim lColumnTitles() As String = {"Group", "Events", _
                                         "Volume", "Volume", "Volume", "Volume", _
                                         "Duration", "Duration", "Duration", "Duration", _
                                         "Intensity", "Intensity", "Intensity", _
                                         "Time Since Last", "Time Since Last", "Time Since Last"}
        Dim lColumnAttributes() As String = {"", "", _
                                             "Max", "Sum", "Mean", "Cumulative", _
                                             "Max", "Sum", "Mean", "Standard Deviation", _
                                             "Max", "Mean", "Standard Deviation", _
                                             "Max", "Mean", "Variance"}
        Dim lThresholds() As Integer = {0, 5, _
                                        10, 15, 20, 30, 40, 50, 75, _
                                        100, 150, 200, 300, 400, 500, 750, _
                                        1000, 1500, 2000} 'thousands of an inch
        Dim lGaps() As Integer = {1, 2, 3, 4, 5, 6, 8, 10, 12, 15, 18, 24} 'hours
        'Dim lThresholds() As Integer = {10} 'thousands of an inch
        'Dim lGaps() As Integer = {1, 2} 'hours

        Dim lOutFile As String = "Summary.txt"
        Dim lReport As New System.IO.StreamWriter(lOutFile)

        Dim lHeaderNeeded As Boolean = True
        Dim lCnt As Integer = 0

        For Each lTimser As atcTimeseries In lWDMFile.DataSets
            Dim lId As Integer = lTimser.Attributes.GetValue("ID")
            Dim lIndex As Integer = System.Array.IndexOf(lDsns, lId)
            If lIndex >= 0 Then
                Dim lDataGroup As New atcDataGroup(lTimser)

                For Each lThreshold As Integer In lThresholds
                    For Each lGap As Integer In lGaps
                        lCnt += 1
                        Dim lGapHours As Double = (lgap / 24) + (JulianSecond / 2)
                        Dim lEvents As atcDataGroup = atcSynopticAnalysisPlugin.ComputeEvents(lDataGroup, lThreshold / 1000, lGapHours, True)
                        For Each lGroupName As String In lGroupNames
                            Dim lGroup As atcCollection = atcSynopticAnalysisPlugin.ComputeGroups(lGroupName, lEvents)
                            Dim lGrid As atcGridSource = atcsynopticAnalysisPlugin.PopulateGrid(lGroup, "Hours", lColumnTitles, lColumnAttributes)
                            Dim lGridString As String = lGrid.ToString
                            If lHeaderNeeded Then
                                lReport.WriteLine("Id" & vbTab & "Threshold" & vbTab & "Gap" & vbTab & StrSplit(lGridString, vbCrLf, ""))
                                lReport.WriteLine(vbTab & "in" & vbTab & "Hours" & vbTab & StrSplit(lGridString, vbCrLf, ""))
                                lReport.WriteLine(vbTab & vbTab & StrSplit(lGridString, vbCrLf, ""))
                                lHeaderNeeded = False
                            Else
                                Dim lPos As Integer = 0
                                For i As Integer = 1 To 3 Step 1
                                    lPos = lGridString.IndexOf(vbCrLf, lPos + 1)
                                Next
                                lGridString = lGridString.Substring(lPos + 2)
                            End If
                            While lGridString.Length > 0
                                lReport.WriteLine(lId & vbTab & lThreshold / 1000 & vbTab & lGap & vbTab & StrSplit(lGridString, vbCrLf, ""))
                            End While
                            'get rid of output groups from this analysis!!!!
                            lGroup = Nothing
                        Next
                        'get rid of events from this analysis!!!!
                        lEvents = Nothing
                    Next
                    Logger.Dbg("   DoneThreshold " & lThreshold / 1000)
                Next
                Logger.Dbg("   DoneDsn " & lId)
            End If
        Next lTimser

        lReport.Close()
        Logger.Dbg("   DoneAnalysisCount " & lCnt)
        Application.Exit()
    End Sub
End Module
