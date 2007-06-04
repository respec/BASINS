Imports System.Collections.Specialized
Imports MapWindow.Interfaces
Imports MapWinUtility

Imports atcUtility
Imports atcData
Imports atcSeasons

Public Module ScriptSummarizeWdms
    Private Const pInputPath As String = "D:\GisData\CBP\prad\ns611a"
    Private Const pFormat As String = "#,##0.00"

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

        Dim lWdmFiles As NameValueCollection = Nothing
        AddFilesInDir(lWdmFiles, pInputPath, True, "*.wdm")
        Logger.Dbg("Found " & lWdmFiles.Count & " data files")
        Dim lWdmCnt As Integer = 0
        Dim lCounty As String
        For Each lFile As String In lWdmFiles
            lWdmCnt += 1
            Dim lWDMfile As New atcWDM.atcDataSourceWDM
            lWDMfile.Open(lFile)
            lCounty = FilenameNoExt(FilenameNoPath(lFile))
            For Each lDS As atcDataSet In lWDMfile.DataSets
                lString.AppendLine(lCounty & vbTab & _
                                   lDS.Attributes.GetValue("ID") & vbTab & _
                                   lDS.Attributes.GetValue("Constituent") & vbTab & _
                                   DoubleToString(lDS.Attributes.GetValue("SumAnnual"), , pFormat) & vbTab & _
                                   lD2SStart.JDateToString(lDS.Attributes.GetValue("SJDay")) & vbTab & _
                                   lD2SEnd.JDateToString(lDS.Attributes.GetValue("EJDay")) & vbTab & _
                                   lDS.Attributes.GetValue("NumValues") & vbTab & _
                                   lFile & vbTab)
            Next
            lWDMfile.DataSets.Clear()
            lWDMfile = Nothing
            Logger.Dbg("Done " & lFile & " MemUsage " & memusage)
        Next
        SaveFileString("Summary.txt", lString.ToString)
        Logger.Dbg("All Done " & lWdmCnt & " WDMs")
    End Sub

    Private Function MemUsage() As String
        System.GC.WaitForPendingFinalizers()
        Return "MemoryUsage(MB):" & Process.GetCurrentProcess.PrivateMemorySize64 / (2 ^ 20) & _
                    " Local(MB):" & System.GC.GetTotalMemory(True) / (2 ^ 20)
    End Function
End Module
