Imports System.Collections.Specialized
Imports MapWindow.Interfaces
Imports MapWinUtility

Imports atcUtility
Imports atcData
Imports atcSeasons

Public Module SummarizeWdms
    Private Const pInputPath As String = "C:\Test\R4Whitlock\"
    'Private Const pInputPath As String = "D:\GisData\CBP\prad\ns611a"
    'Private Const pInputPath As String = "D:\GisData\CBP\met\8405"
    Private Const pFormat As String = "##,###,##0.00"

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        ChDriveDir(pInputPath)
        Logger.Dbg(" CurDir:" & CurDir())

        Dim lD2SStart As New atcDateFormat
        lD2SStart.IncludeHours = False
        lD2SStart.IncludeMinutes = False
        lD2SStart.Midnight24 = False
        Dim lD2SEnd As New atcDateFormat
        lD2SEnd.IncludeHours = False
        lD2SEnd.IncludeMinutes = False

        Dim lSB As New Text.StringBuilder
        lSB.AppendLine("Wdm" & vbTab & _
                       "Dsn" & vbTab & _
                       "Scenario" & vbTab & _
                       "Constituent" & vbTab & _
                       "Value" & vbTab & _
                       "StartDate" & vbTab & _
                       "EndDate" & vbTab & _
                       "Count" & vbTab & _
                       "File")

        Dim lWdmFiles As NameValueCollection = Nothing
        AddFilesInDir(lWdmFiles, pInputPath, True, "*.wdm")
        Logger.Dbg("Found " & lWdmFiles.Count & " data files")
        Dim lWdmCnt As Integer = 0
        For Each lFile As String In lWdmFiles
            lWdmCnt += 1
            Dim lWDMFile As New atcWDM.atcDataSourceWDM
            lWDMFile.Open(lFile)
            Dim lWDMName As String = FilenameNoExt(FilenameNoPath(lFile))
            For Each lTimeseries As atcTimeseries In lWDMFile.DataSets
                Dim lCons As String = lTimeseries.Attributes.GetValue("Constituent")
                'Dim lScenario As String = lDS.Attributes.GetValue("Scenario")
                'special case for cbp
                Dim lScenario As String = lFile.Replace(pInputPath.ToLower, "").Replace(lWDMName & ".wdm", "").Replace("\", "")
                If lScenario.Length = 0 Then lScenario = "base"

                Dim lValue As String
                Select Case lCons 'dont include constituents dont want to summarize
                    Case "EVAP", "HPRC", "NO23", "NH4A", "NH4D", "ORGN", "PO4A", "ORGP"
                        lValue = DoubleToString(lTimeseries.Attributes.GetValue("SumAnnual"), , pFormat)
                    Case "DEWP", "WNDH", "RADH", "ATMP", "CLDC"
                        lValue = DoubleToString(lTimeseries.Attributes.GetValue("Mean"), , pFormat)
                    Case Else
                        lValue = DoubleToString(lTimeseries.Attributes.GetValue("Mean"), 16, pFormat) '"?"
                End Select
                If lValue <> "?" Then
                    lSB.AppendLine(lWDMName & vbTab & _
                                       lTimeseries.Attributes.GetValue("ID") & vbTab & _
                                       lScenario & vbTab & _
                                       lCons & vbTab & _
                                       lValue & vbTab & _
                                       "'" & lD2SStart.JDateToString(lTimeseries.Attributes.GetValue("SJDay")) & "'" & vbTab & _
                                       "'" & lD2SEnd.JDateToString(lTimeseries.Attributes.GetValue("EJDay")) & "'" & vbTab & _
                                       lTimeseries.Attributes.GetValue("Count") & vbTab & _
                                       lFile)
                Else
                    'TODO: first time skip message
                End If
            Next
            lWDMFile.DataSets.Clear()
            lWDMFile = Nothing
            Dim lPercent As String = "(" & DoubleToString((100 * lWdmCnt) / lWdmFiles.Count, , pFormat) & "%)"
            Logger.Dbg("Done " & lWdmCnt & lPercent & lFile & " MemUsage " & MemUsage())
        Next
        SaveFileString("Summary.txt", lSB.ToString)
        Logger.Dbg("All Done " & lWdmCnt & " WDMs")
    End Sub

    Private Function MemUsage() As String
        System.GC.WaitForPendingFinalizers()
        Return "MemoryUsage(MB):" & DoubleToString(Process.GetCurrentProcess.PrivateMemorySize64 / (2 ^ 20), , pFormat) & _
                    " Local(MB):" & DoubleToString(System.GC.GetTotalMemory(True) / (2 ^ 20), , pFormat)
    End Function
End Module
