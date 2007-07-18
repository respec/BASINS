Imports System.Collections.Specialized
Imports MapWindow.Interfaces
Imports MapWinUtility

Imports atcUtility
Imports atcData

Module ScriptSummarizeTimeseries
    Private Const pInputPath As String = "D:\GisData\SERDP\Semp\Ecology\Stream water chemistry data for small Fort Benning Streams 1999-2003"
    Private Const pInputFile As String = "MulhollandData.dbf"
    Private Const pAllOpenData As Boolean = True
    Private Const pFormat As String = "#,##0.00"

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        ChDriveDir(pInputPath)
        Logger.Dbg(" CurDir:" & CurDir())

        Dim lD2SStart As New atcDateFormat
        lD2SStart.IncludeHours = True
        lD2SStart.IncludeMinutes = True
        lD2SStart.Midnight24 = False
        Dim lD2SEnd As New atcDateFormat
        lD2SEnd.IncludeHours = True
        lD2SEnd.IncludeMinutes = True

        Dim lString As New Text.StringBuilder
        lString.AppendLine("ID" & vbTab & _
                           "Location" & vbTab & _
                           "Constituent" & vbTab & _
                           "StartDate" & vbTab & _
                           "EndDate" & vbTab & _
                           "Count" & vbTab & _
                           "Mean" & vbTab & _
                           "Geometric Mean" & vbTab & _
                           "Minimum" & vbTab & _
                           "Maximum" & vbTab)

        Dim lDataSets As atcDataGroup = Nothing
        If pAllOpenData Then
            Dim lErr As String = ""
            lDataSets = atcDataManager.DataSets
        Else
            Dim lTimeseriesFile As New atcTimseriesDbf.atcDataSourceTimeseriesDbf
            Logger.Dbg("Open:" & pInputFile)
            lTimeseriesFile.Open(pInputFile)
            lDataSets = lTimeseriesFile.DataSets
        End If
        Logger.Dbg("DatasetCount " & lDataSets.Count)
        For Each lDS As atcDataSet In lDataSets
            Dim lSJDay As Double = lDS.Attributes.GetValue("SJDay")
            lString.AppendLine(lDS.Attributes.GetValue("ID", "?") & vbTab & _
                               lDS.Attributes.GetValue("Location", "?") & vbTab & _
                               lDS.Attributes.GetValue("Constituent", "?") & vbTab & _
                               "'" & lD2SStart.JDateToString(lDS.Attributes.GetValue("SJDay")) & "'" & vbTab & _
                               "'" & lD2SEnd.JDateToString(lDS.Attributes.GetValue("EJDay")) & "'" & vbTab & _
                               lDS.Attributes.GetValue("Count") & vbTab & _
                               DoubleToString(lDS.Attributes.GetValue("Mean"), , pFormat) & vbTab & _
                               DoubleToString(lDS.Attributes.GetValue("Geometric Mean"), , pFormat) & vbTab & _
                               DoubleToString(lDS.Attributes.GetValue("Min"), , pFormat) & vbTab & _
                               DoubleToString(lDS.Attributes.GetValue("Max"), , pFormat) & vbTab)
        Next
        SaveFileString(FilenameNoExt(pInputFile) & ".txt", lString.ToString)

        If Not pAllOpenData Then
            lDataSets.Clear()
            lDataSets = Nothing
        End If
        Logger.Dbg("All Done")
    End Sub
End Module
