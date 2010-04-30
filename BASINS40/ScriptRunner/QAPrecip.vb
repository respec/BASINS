Imports System.Collections.Specialized
Imports MapWindow.Interfaces
Imports MapWinUtility

Imports atcUtility
Imports atcData
Imports atcSeasons

Public Module ScriptQAPrecip
    Private Const pInputPath As String = "C:\BASINSMet\WDMFinal\"
    Private Const pFormat As String = "#,##0.00"
    Private Const pMaxHrlySuspect As Double = 10.0
    Private Const pMaxDlySuspect As Double = 20.0
    Private Const pMaxMonSuspect As Double = 40.0
    Private Const pSaveAsString As Boolean = False
    Private Const pHourly As Boolean = False

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        ChDriveDir(pInputPath)
        Logger.Dbg("QAPrecip: Start")

        Dim lD2SStart As New atcDateFormat
        lD2SStart.IncludeHours = False
        lD2SStart.IncludeMinutes = False
        lD2SStart.Midnight24 = False
        Dim lD2SEnd As New atcDateFormat
        lD2SEnd.IncludeHours = False
        lD2SEnd.IncludeMinutes = False
        Dim lts As atcTimeseries
        Dim ltsMon As atcTimeseries
        Dim lPrecCnt As Integer = 0
        Dim lChkCnt As Integer = 0
        Dim lChkMonCnt As Integer = 0
        Dim lSuspectColl As New atcCollection
        Dim lMonColl As New atcCollection
        Dim lMax As Double

        If pHourly Then
            Logger.Dbg("QAPrecip:  Looking for suspect Hourly Values")
            Logger.Dbg("QAPrecip:    Hourly values greater than " & pMaxHrlySuspect)
        Else
            Logger.Dbg("QAPrecip:  Looking for suspect Daily Values")
            Logger.Dbg("QAPrecip:    Daily values greater than " & pMaxDlySuspect)
        End If
        Logger.Dbg("QAPrecip:    Monthly values greater than " & pMaxMonSuspect)
        Dim lWdmFiles As NameValueCollection = Nothing
        AddFilesInDir(lWdmFiles, pInputPath, True, "*.wdm")
        Logger.Dbg("QAPrecip: Found " & lWdmFiles.Count & " data files")
        Dim lWdmCnt As Integer = 0
        Dim lWdmSkipped As Integer = 0
        For Each lFile As String In lWdmFiles
            lWdmCnt += 1
            Dim lWDMFile As New atcWDM.atcDataSourceWDM
            lWDMFile.Open(lFile)
            lts = Nothing
            If pHourly Then
                lts = lWDMFile.DataSets.ItemByKey(1)
            Else 'look for daily precip timeseries
                For i As Integer = 1001 To 1020
                    lts = lWDMFile.DataSets.ItemByKey(i)
                    If Not lts Is Nothing Then
                        If lts.Attributes.GetValue("Constituent") = "PRCP" Then
                            Exit For
                        Else
                            lts = Nothing
                        End If
                    End If
                Next
            End If
            If Not lts Is Nothing Then
                lPrecCnt += 1
                lMax = lts.Attributes.GetValue("Max")
                If (pHourly AndAlso lMax > pMaxHrlySuspect) Or _
                   (Not pHourly AndAlso lMax > pMaxDlySuspect) Then
                    lSuspectColl.Add(lts.ToString & ", " & lts.Attributes.GetValue("Stanam"), lMax)
                    If pHourly Then
                        Logger.Dbg("QAPrecip: For " & lts.ToString & ", " & lts.Attributes.GetValue("Stanam") & " - Max Hourly Value: " & lMax)
                    Else
                        Logger.Dbg("QAPrecip: For " & lts.ToString & ", " & lts.Attributes.GetValue("Stanam") & " - Max Daily Value: " & lMax)
                    End If
                    'lChkCnt += 1
                End If
                ltsMon = Aggregate(lts, atcTimeUnit.TUMonth, 1, atcTran.TranSumDiv, Nothing)
                lMax = ltsMon.Attributes.GetValue("Max")
                If lMax > pMaxMonSuspect Then
                    lMonColl.Add(lts.ToString & ", " & lts.Attributes.GetValue("Stanam"), lMax)
                    Logger.Dbg("QAPrecip: For " & lts.ToString & ", " & lts.Attributes.GetValue("Stanam") & " - Max Monthly Value: " & lMax)
                    'lChkMonCnt += 1
                End If
            End If
            lWDMFile.DataSets.Clear()
            lWDMFile = Nothing
        Next
        Logger.Dbg("QAPrecip:")
        Logger.Dbg("QAPrecip:")
        If pHourly Then
            Logger.Dbg("QAPrecip: Sorted Hourly values")
        Else
            Logger.Dbg("QAPrecip: Sorted Daily values")
        End If
        lSuspectColl.SortByValue()
        For i As Integer = 0 To lSuspectColl.Count - 1
            Logger.Dbg("QAPrecip:  " & lSuspectColl.Keys(i) & "  -  " & lSuspectColl.ItemByIndex(i))
        Next
        Logger.Dbg("QAPrecip:")
        Logger.Dbg("QAPrecip:")
        Logger.Dbg("QAPrecip: Sorted Monthly values")
        lMonColl.SortByValue()
        For i As Integer = 0 To lMonColl.Count - 1
            Logger.Dbg("QAPrecip:  " & lMonColl.Keys(i) & "  -  " & lMonColl.ItemByIndex(i))
        Next
        Logger.Dbg("QAPrecip:")
        Logger.Dbg("QAPrecip: All Done - checked " & lWdmCnt & " WDMs")
        Logger.Dbg("QAPrecip:          - " & lPrecCnt & " Precip datasets")
        If pHourly Then
            Logger.Dbg("QAPrecip:          - " & lSuspectColl.Count & " Datasets with suspect Hourly values (" & (lSuspectColl.Count / lPrecCnt) * 100 & "%)")
        Else
            Logger.Dbg("QAPrecip:          - " & lSuspectColl.Count & " Datasets with suspect Daily values (" & (lSuspectColl.Count / lPrecCnt) * 100 & "%)")
        End If
        Logger.Dbg("QAPrecip:          - " & lMonColl.Count & " Datasets with suspect Monthly values (" & (lMonColl.Count / lPrecCnt) * 100 & "%)")
    End Sub

End Module
