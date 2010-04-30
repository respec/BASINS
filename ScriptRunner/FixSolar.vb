Imports System.Collections.Specialized
Imports MapWindow.Interfaces
Imports MapWinUtility

Imports atcUtility
Imports atcData
Imports atcSeasons

Public Module FixSolar
    Private Const pBackupPath As String = "C:\BasinsMet\Backup\WDMFinal\"
    Private Const pInputPath As String = "C:\BASINSMet\WDMFinal\"

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        ChDriveDir(pInputPath)
        Logger.Dbg("FixSolar: Start")
        Logger.Dbg(" CurDir:" & CurDir())

        Dim lts As atcTimeseries

        Dim lWdmFiles As NameValueCollection = Nothing
        AddFilesInDir(lWdmFiles, pInputPath, True, "*.wdm")
        Logger.Dbg("Found " & lWdmFiles.Count & " data files")
        Dim lWdmCnt As Integer = 0
        For Each lFile As String In lWdmFiles
            Logger.Dbg("FixSolar: For WDM file " & lFile)
            Dim lWDMFile As New atcWDM.atcDataSourceWDM
            lWDMFile.Open(lFile)
            If lWDMFile.DataSets.Keys.Contains(103) Then 'remove erroneous DSOL Dataset (DSN 103)
                lts = Nothing
                lts = lWDMFile.DataSets.ItemByKey(103)
                If lWDMFile.RemoveDataset(lts) Then
                    Logger.Dbg("FixSolar: Removed erroneous DSN 103")
                Else
                End If
            End If
            Dim lBackWDMFile As New atcWDM.atcDataSourceWDM
            If lBackWDMFile.Open(pBackupPath & FilenameNoPath(lFile)) Then
                lts = Nothing
                lts = lBackWDMFile.DataSets.ItemByKey(5)
                If Not lts Is Nothing Then 'SOLR exists, copy to working version
                    lts.EnsureValuesRead()
                    If lWDMFile.AddDataset(lts) Then 'overwrite Dataset
                        Logger.Dbg("FixSolar: Updated SOLR, DSN 5")
                    Else
                        Logger.Dbg("FixSolar:  PROBLEM updating SOLR, DSN 5")
                    End If
                End If
                lts = Nothing
                lts = lBackWDMFile.DataSets.ItemByKey(1102)
                If Not lts Is Nothing Then 'DSOL exists, copy to working version
                    If lWDMFile.AddDataset(lts) Then 'overwrite Dataset
                        Logger.Dbg("FixSolar: Updated DSOL, DSN 1102")
                    Else
                        Logger.Dbg("FixSolar:  PROBLEM updating DSOL, DSN 1102")
                    End If
                End If
            End If
            lWDMFile.DataSets.Clear()
            lWDMFile = Nothing
            lBackWDMFile.DataSets.Clear()
            lBackWDMFile = Nothing
        Next
        'Logger.Dbg("All Done " & lWdmCnt & " WDMs")
    End Sub

    'Private Function MemUsage() As String
    '    System.GC.WaitForPendingFinalizers()
    '    Return "MemoryUsage(MB):" & DoubleToString(Process.GetCurrentProcess.PrivateMemorySize64 / (2 ^ 20), , pFormat) & _
    '                " Local(MB):" & DoubleToString(System.GC.GetTotalMemory(True) / (2 ^ 20), , pFormat)
    'End Function
End Module
