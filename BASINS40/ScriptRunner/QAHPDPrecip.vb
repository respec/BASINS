Imports System.Collections.Specialized
Imports MapWindow.Interfaces
Imports MapWinUtility

Imports atcUtility
Imports atcData
Imports atcSeasons

Public Module ScriptQAPrecip
    Private Const pInputPath As String = "C:\BASINSMet\WDMFilled\"
    Private Const pFormat As String = "#,##0.00"
    Private Const pMaxNotSuspect As Double = 5.0
    Private Const pSaveAsString As Boolean = False
    Private Const pSaveAsDBF As Boolean = True

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
        Dim lPrecCnt As Integer = 0
        Dim lChkCnt As Integer = 0
        Dim lMax As Double
        Dim lState As String

        Dim lWdmFiles As NameValueCollection = Nothing
        AddFilesInDir(lWdmFiles, pInputPath, True, "*.wdm")
        Logger.Dbg("QAHPDPrecip: Found " & lWdmFiles.Count & " data files")
        Dim lWdmCnt As Integer = 0
        Dim lWdmSkipped As Integer = 0
        For Each lFile As String In lWdmFiles
            lWdmCnt += 1
            lState = lFile.Substring(23, 2)
            If IsNumeric(lState) Then
                Dim lWDMFile As New atcWDM.atcDataSourceWDM
                lWDMFile.Open(lFile)
                lts = Nothing
                lts = lWDMFile.DataSets.ItemByKey(100)
                If Not lts Is Nothing Then
                    lPrecCnt += 1
                    lMax = lts.Attributes.GetValue("Max")
                    Logger.Dbg("QAHPDPrecip: For " & lts.Attributes.GetValue("Location") & ", " & lts.Attributes.GetValue("Stanam") & " - Max Value: " & lMax)
                    If lMax > pMaxNotSuspect Then
                        Logger.Dbg("QAHPDPrecip:  CHECK ME!")
                        lChkCnt += 1
                    End If
                End If
                lWDMFile.DataSets.Clear()
                lWDMFile = Nothing
            End If
        Next
        Logger.Dbg("QAHPDPrecip: All Done - checked " & lWdmCnt & " WDMs")
        Logger.Dbg("QAHPDPrecip:          - " & lPrecCnt & " HPD Precip datasets")
        Logger.Dbg("QAHPDPrecip:          - " & lChkCnt & " Suspect datasets (" & (lChkCnt / lPrecCnt) * 100 & "%)")
    End Sub

End Module
