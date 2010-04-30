Imports System.Collections.Specialized
Imports MapWindow.Interfaces
Imports MapWinUtility

Imports atcUtility
Imports atcData
Imports atcSeasons

Public Module ScriptSummarizeWdms
    Private Const pInputPath As String = "C:\BASINSMet\WDMFinal\"
    'Private Const pInputPath As String = "D:\GisData\CBP\met\8405"
    Private Const pFormat As String = "#,##0.00"
    Private Const pSaveAsString As Boolean = False
    Private Const pSaveAsDBF As Boolean = True

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        ChDriveDir(pInputPath)
        Logger.Dbg("QACheck:Start")
        Logger.Dbg(" CurDir:" & CurDir())

        Dim lD2SStart As New atcDateFormat
        lD2SStart.IncludeHours = False
        lD2SStart.IncludeMinutes = False
        lD2SStart.Midnight24 = False
        Dim lD2SEnd As New atcDateFormat
        lD2SEnd.IncludeHours = False
        lD2SEnd.IncludeMinutes = False

        Dim lString As New Text.StringBuilder
        If pSaveAsString Then
            lString.AppendLine("Dsn" & vbTab & _
                               "Location" & vbTab & _
                               "StaNam" & vbTab & _
                               "Latitude" & vbTab & _
                               "Longitude" & vbTab & _
                               "Constituent" & vbTab & _
                               "StartDate" & vbTab & _
                               "EndDate" & vbTab & _
                               "Count")
        End If
        Dim lDBF As New atcTableDBF
        If pSaveAsDBF Then
            With lDBF
                .Year = CInt(Format(Now, "yyyy")) - 1900
                .Month = CByte(Format(Now, "mm"))
                .Day = CByte(Format(Now, "dd"))
                .NumFields = 9
                .FieldName(1) = "DSN"
                .FieldType(1) = "N"
                .FieldLength(1) = 4
                .FieldDecimalCount(1) = 0
                .FieldName(2) = "LOCATION"
                .FieldType(2) = "C"
                .FieldLength(2) = 10
                .FieldDecimalCount(2) = 0
                .FieldName(3) = "STANAM"
                .FieldType(3) = "C"
                .FieldLength(3) = 48
                .FieldDecimalCount(3) = 0
                .FieldName(4) = "LATITUDE"
                .FieldType(4) = "N"
                .FieldLength(4) = 9
                .FieldDecimalCount(4) = 4
                .FieldName(5) = "LONGITUDE"
                .FieldType(5) = "N"
                .FieldLength(5) = 9
                .FieldDecimalCount(5) = 4
                .FieldName(6) = "CONSTITUEN"
                .FieldType(6) = "C"
                .FieldLength(6) = 10
                .FieldDecimalCount(6) = 0
                .FieldName(7) = "STARTDATE"
                .FieldType(7) = "D"
                .FieldLength(7) = 8
                .FieldDecimalCount(7) = 0
                .FieldName(8) = "ENDDATE"
                .FieldType(8) = "D"
                .FieldLength(8) = 8
                .FieldDecimalCount(8) = 0
                .FieldName(9) = "COUNT"
                .FieldType(9) = "N"
                .FieldLength(9) = 7
                .FieldDecimalCount(9) = 0
                '.NumRecords = 30000 'safe estimate
                '.InitData()
                .CurrentRecord = 1
            End With

        End If

        Dim lWdmFiles As NameValueCollection = Nothing
        AddFilesInDir(lWdmFiles, pInputPath, True, "*.wdm")
        Logger.Dbg("Found " & lWdmFiles.Count & " data files")
        Dim lWdmCnt As Integer = 0
        Dim lWdmSkipped As Integer = 0
        For Each lFile As String In lWdmFiles
            lWdmCnt += 1
            Dim lWDMFile As New atcWDM.atcDataSourceWDM
            lWDMFile.Open(lFile)
            Dim lDSCnt As Integer = 0
            For Each lDS As atcDataSet In lWDMFile.DataSets
                Dim lID As Integer = lDS.Attributes.GetValue("ID")
                Dim lCons As String = lDS.Attributes.GetValue("Constituent")
                If lID < 11 Then
                    lDSCnt += 1
                    If pSaveAsString Then
                        lD2SStart.DateSeparator = "/"
                        lString.AppendLine(lID & vbTab & _
                                           lDS.Attributes.GetValue("Location") & vbTab & _
                                           lDS.Attributes.GetValue("STANAM") & vbTab & _
                                           lDS.Attributes.GetValue("LatDeg") & vbTab & _
                                           lDS.Attributes.GetValue("LngDeg") & vbTab & lCons & vbTab & _
                                           lD2SStart.JDateToString(lDS.Attributes.GetValue("SJDay")) & vbTab & _
                                           lD2SEnd.JDateToString(lDS.Attributes.GetValue("EJDay")) & vbTab & _
                                           lDS.Attributes.GetValue("Count"))
                    End If
                    If pSaveAsDBF Then
                        lDBF.Value(1) = lID
                        lDBF.Value(2) = lDS.Attributes.GetValue("Location")
                        lDBF.Value(3) = lDS.Attributes.GetValue("STANAM")
                        lDBF.Value(4) = lDS.Attributes.GetValue("LatDeg")
                        lDBF.Value(5) = lDS.Attributes.GetValue("LngDeg")
                        lDBF.Value(6) = lCons
                        lD2SStart.DateSeparator = ""
                        lDBF.Value(7) = lD2SStart.JDateToString(lDS.Attributes.GetValue("SJDay"))
                        lD2SEnd.DateSeparator = ""
                        lDBF.Value(8) = lD2SEnd.JDateToString(lDS.Attributes.GetValue("EJDay"))
                        lDBF.Value(9) = lDS.Attributes.GetValue("Count")
                        lDBF.CurrentRecord += 1
                    End If
                Else
                    lWdmSkipped += 1
                End If
            Next
            If lDSCnt = 0 Then 'no final hourly data on this WDM, store separately
                Logger.Dbg("SummarizeFinal:  No final Hourly data on WDM file " & lFile)
                If Not FileExists(pInputPath & "NoFinalData\" & FilenameNoPath(lFile)) Then
                    FileCopy(lFile, pInputPath & "NoFinalData\" & FilenameNoPath(lFile))
                End If
            End If
            lWDMFile.DataSets.Clear()
            lWDMFile = Nothing
            Dim lPercent As String = "(" & DoubleToString((100 * lWdmCnt) / lWdmFiles.Count, , pFormat) & "%)"
            Logger.Dbg("Done " & lWdmCnt & lPercent & lFile & " MemUsage " & MemUsage())
        Next
        If pSaveAsString Then SaveFileString("Summary.txt", lString.ToString)
        If pSaveAsDBF Then
            lDBF.CurrentRecord -= 1
            lDBF.NumRecords = lDBF.CurrentRecord
            lDBF.WriteFile("MetStations.dbf")
        End If
        Logger.Dbg("All Done " & lWdmCnt & " WDMs")
        Logger.Dbg(" Skipped " & lWdmSkipped & " WDMs")
    End Sub

    Private Function MemUsage() As String
        System.GC.WaitForPendingFinalizers()
        Return "MemoryUsage(MB):" & DoubleToString(Process.GetCurrentProcess.PrivateMemorySize64 / (2 ^ 20), , pFormat) & _
                    " Local(MB):" & DoubleToString(System.GC.GetTotalMemory(True) / (2 ^ 20), , pFormat)
    End Function
End Module
