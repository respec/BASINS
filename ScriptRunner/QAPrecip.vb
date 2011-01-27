Imports System.Collections.Specialized
Imports MapWindow.Interfaces
Imports MapWinUtility

Imports atcUtility
Imports atcData
Imports atcSeasons

Public Module QAPrecip
    Private Const pBasePath As String = "G:\Projects\TT_GCRP\MetDataFinalWithManualZip\"
    Private Const pOutputPath As String = pBasePath & "QA\"
    Private Const pInputPath As String = pBasePath & "Projects\"

    Private Const pFormat As String = "#,##0.00"
    Private Const pMaxHrlySuspect As Double = 10.0
    Private Const pMaxDlySuspect As Double = 20.0
    Private Const pMaxMonSuspect As Double = 40.0
    Private Const pMaxYearSuspect As Double = 100.0
    Private Const pSaveAsString As Boolean = False

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        Dim lOriginalFolder As String = IO.Directory.GetCurrentDirectory
        Dim lOriginalLog As String = Logger.FileName
        Dim lOriginalDisplayMessages As Boolean = Logger.DisplayMessageBoxes

        ChDriveDir(pOutputPath)
        Logger.StartToFile("QAPrecip.log", False, False, True)
        Logger.Dbg("QAPrecip: Start")

        Dim lD2SStart As New atcDateFormat
        lD2SStart.IncludeHours = False
        lD2SStart.IncludeMinutes = False
        lD2SStart.Midnight24 = False
        Dim lD2SEnd As New atcDateFormat
        lD2SEnd.IncludeHours = False
        lD2SEnd.IncludeMinutes = False
        Dim lPrecCnt As Integer = 0
        Dim lHourSuspect As New atcCollection
        Dim lDaySuspect As New atcCollection
        Dim lMonSuspact As New atcCollection
        Dim lYearSuspact As New atcCollection

        Logger.Dbg("Looking for Hourly Values greater than " & DoubleToString(pMaxHrlySuspect, , pFormat))
        Logger.Dbg("Looking for Daily values greater than " & DoubleToString(pMaxDlySuspect, , pFormat))
        Logger.Dbg("Looking for Monthly values greater than " & DoubleToString(pMaxMonSuspect, , pFormat))
        Logger.Dbg("Looking for Annual values greater than " & DoubleToString(pMaxYearSuspect, , pFormat))

        Dim lWdmFiles As NameValueCollection = Nothing
        AddFilesInDir(lWdmFiles, pInputPath, True, "*.wdm")
        Logger.Dbg("  Found " & lWdmFiles.Count & " wdm data files")
        Dim lWdmCnt As Integer = 0
        Dim lWdmSkipped As Integer = 0
        For Each lFile As String In lWdmFiles
            lWdmCnt += 1
            Dim lWDMFile As New atcWDM.atcDataSourceWDM
            If lWDMFile.Open(lFile) Then
                Logger.Dbg("Processing " & lFile)
                For Each lTs As atcDataSet In lWDMFile.DataSets
                    Dim lCons As String = lTs.Attributes.GetValue("Constituent")
                    If lCons = "PREC" Then
                        lPrecCnt += 1
                        Dim lProblem As Boolean = False
                        Dim lMax As Double = lTs.Attributes.GetValue("Max")
                        Dim lMaxDay As Double
                        Dim lTu As atcTimeUnit = lTs.Attributes.GetValue("TU")
                        If (lTu = atcTimeUnit.TUHour) Then
                            If (lMax > pMaxHrlySuspect) Then
                                lHourSuspect.Add(lTs.ToString & ", " & lTs.Attributes.GetValue("Stanam"), lMax)
                                Logger.Dbg("For " & lTs.ToString & ", " & lTs.Attributes.GetValue("Stanam") & _
                                           " - Max Hourly Value: " & DoubleToString(lMax, , pFormat) & " on " & lTs.Attributes.GetFormattedValue("MaxDate"))
                                lProblem = True
                            End If

                            Dim lTsDay As atcTimeseries = Aggregate(lTs, atcTimeUnit.TUDay, 1, atcTran.TranSumDiv, Nothing)
                            lMaxDay = lTsDay.Attributes.GetValue("Max")
                            If lMaxDay > pMaxDlySuspect Then
                                lDaySuspect.Add(lTsDay.ToString & ", " & lTsDay.Attributes.GetValue("Stanam"), lMaxDay)
                                Logger.Dbg("For " & lTsDay.ToString & ", " & lTsDay.Attributes.GetValue("Stanam") & _
                                           " - Max Daily Value: " & DoubleToString(lMaxDay, , pFormat) & " on " & lTsDay.Attributes.GetFormattedValue("MaxDate"))
                                lProblem = True
                            End If
                        ElseIf lTu = atcTimeUnit.TUDay Then
                            lMaxDay = lMax
                            If lMax > pMaxDlySuspect Then
                                lDaySuspect.Add(lTs.ToString & ", " & lTs.Attributes.GetValue("Stanam"), lMaxDay)
                                Logger.Dbg("For " & lTs.ToString & ", " & lTs.Attributes.GetValue("Stanam") & _
                                           " - Max Daily Value: " & DoubleToString(lMax, , pFormat) & " on " & lTs.Attributes.GetFormattedValue("MaxDate"))
                                lProblem = True
                            End If
                        Else
                            Logger.Dbg("???? TimeUnits " & lTu)
                        End If

                        Dim lTsMon As atcTimeseries = Aggregate(lTs, atcTimeUnit.TUMonth, 1, atcTran.TranSumDiv, Nothing)
                        Dim lMaxMon As Double = lTsMon.Attributes.GetValue("Max")
                        If lMaxMon > pMaxMonSuspect Then
                            lMonSuspact.Add(lTs.ToString & ", " & lTs.Attributes.GetValue("Stanam"), lMaxMon)
                            Logger.Dbg("For " & lTs.ToString & ", " & lTs.Attributes.GetValue("Stanam") & _
                                       " - Max Monthly Value: " & DoubleToString(lMaxMon, , pFormat) & " on " & lTsMon.Attributes.GetFormattedValue("MaxDate"))
                            lProblem = True
                        End If

                        Dim lTsYear As atcTimeseries = Aggregate(lTsMon, atcTimeUnit.TUYear, 1, atcTran.TranSumDiv, Nothing)
                        Dim lMaxYear As Double = lTsYear.Attributes.GetValue("Max")
                        If lMaxYear > pMaxYearSuspect Then
                            lYearSuspact.Add(lTs.ToString & ", " & lTs.Attributes.GetValue("Stanam"), lMaxYear)
                            Logger.Dbg("For " & lTs.ToString & ", " & lTs.Attributes.GetValue("Stanam") & _
                                       " - Max Yearly Value: " & DoubleToString(lMaxYear, , pFormat) & " on " & lTsYear.Attributes.GetFormattedValue("MaxDate"))
                            lProblem = True
                        End If

                        Dim lStr As String = "OK "
                        If lProblem Then
                            lStr = "?? "
                        End If
                        Logger.Dbg(lStr & lTs.Attributes.GetValue("id") & " " & lTs.Attributes.GetValue("location") & _
                                   " '" & lTs.Attributes.GetValue("stanam") & "' " & _
                                   " Hour:" & DoubleToString(lMax, , pFormat) & _
                                   " Day:" & DoubleToString(lMaxDay, , pFormat) & _
                                   " Mon:" & DoubleToString(lMaxMon, , pFormat) & _
                                   " Year:" & DoubleToString(lMaxYear, , pFormat))
                    End If
                Next
            End If
            lWDMFile.DataSets.Clear()
            lWDMFile = Nothing
        Next

        If lHourSuspect.Count > 0 Then
            Logger.Dbg("Sorted suspect Hourly values( > " & pMaxHrlySuspect & ")")
            lHourSuspect.SortByValue()
            For i As Integer = lHourSuspect.Count - 1 To 0 Step -1
                Logger.Dbg(" " & lHourSuspect.Keys(i) & "  -  " & DoubleToString(lHourSuspect.ItemByIndex(i), , pFormat))
            Next
        End If
        If lDaySuspect.Count > 0 Then
            Logger.Dbg("Sorted suspect Daily values( > " & pMaxDlySuspect & ")")
            lHourSuspect.SortByValue()
            For i As Integer = lDaySuspect.Count - 1 To 0 Step -1
                Logger.Dbg(" " & lDaySuspect.Keys(i) & "  -  " & DoubleToString(lDaySuspect.ItemByIndex(i), , pFormat))
            Next
        End If
        If lMonSuspact.Count > 0 Then
            Logger.Dbg("Sorted suspect Monthly values( > " & pMaxMonSuspect & ")")
            lMonSuspact.SortByValue()
            For i As Integer = lMonSuspact.Count - 1 To 0 Step -1
                Logger.Dbg(" " & lMonSuspact.Keys(i) & "  -  " & DoubleToString(lMonSuspact.ItemByIndex(i), , pFormat))
            Next
        End If
        If lYearSuspact.Count > 0 Then
            Logger.Dbg("Sorted suspect Yearly values( > " & pMaxYearSuspect & ")")
            lYearSuspact.SortByValue()
            For i As Integer = lYearSuspact.Count - 1 To 0 Step -1
                Logger.Dbg(" " & lYearSuspact.Keys(i) & "  -  " & DoubleToString(lYearSuspact.ItemByIndex(i), , pFormat))
            Next
        End If
        Logger.Dbg("All Done - checked " & lWdmCnt & " WDMs")
        Logger.Dbg("         - " & lPrecCnt & " Precip datasets")
        Logger.Dbg("         - " & lHourSuspect.Count & " Datasets with suspect Hourly values (" & DoubleToString((lHourSuspect.Count / lPrecCnt) * 100, , pFormat) & "%)")
        Logger.Dbg("         - " & lDaySuspect.Count & " Datasets with suspect Daily values (" & DoubleToString((lDaySuspect.Count / lPrecCnt) * 100, , pFormat) & "%)")
        Logger.Dbg("         - " & lMonSuspact.Count & " Datasets with suspect Monthly values (" & DoubleToString((lMonSuspact.Count / lPrecCnt) * 100, , pFormat) & "%)")
        Logger.Dbg("         - " & lYearSuspact.Count & " Datasets with suspect Yearly values (" & DoubleToString((lYearSuspact.Count / lPrecCnt) * 100, , pFormat) & "%)")

        IO.Directory.SetCurrentDirectory(lOriginalFolder)
        Logger.Dbg("QAPrecipDone")
        Logger.StartToFile(lOriginalLog, True, , True)
        Logger.Dbg("QAPrecipDone")
        Logger.DisplayMessageBoxes = lOriginalDisplayMessages
    End Sub

End Module
