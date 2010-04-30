Imports System.Collections.Specialized
Imports MapWindow.Interfaces
Imports MapWinUtility

Imports atcUtility
Imports atcData
Imports atcSeasons

Public Module ScriptSummarizeWdms
    Private Const pStationPath As String = "C:\BasinsMet\Stations\"
    Private Const pInputPath As String = "C:\BASINSMet\WDMFinal\"

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        ChDriveDir(pInputPath)
        Logger.Dbg("QACheck:Start")
        Logger.Dbg(" CurDir:" & CurDir())

        Dim lLatDeg As Double
        Dim lLngDeg As Double
        Dim lUpdate As Boolean
        Dim lNewWDM As String = pInputPath & "new.wdm"
        Dim lts As atcTimeseries

        Dim lStationDBF As New atcTableDBF
        If lStationDBF.OpenFile(pStationPath & "StationLocs.dbf") Then
            Logger.Dbg("CompileFinal: Opened Station Location Master file " & pStationPath & "StationLocs.dbf")
        Else
            Logger.Dbg("CompileFinal: PROBLEM Opening Station Location Master file " & pStationPath & "StationLocs.dbf")
        End If

        Dim lWdmFiles As NameValueCollection = Nothing
        AddFilesInDir(lWdmFiles, pInputPath, True, "*.wdm")
        Logger.Dbg("Found " & lWdmFiles.Count & " data files")
        Dim lWdmCnt As Integer = 0
        For Each lFile As String In lWdmFiles
            Dim lNewWDMfile As atcWDM.atcDataSourceWDM = Nothing
            Dim lWDMFile As New atcWDM.atcDataSourceWDM
            lWDMfile.Open(lFile)
            Dim lWDMName As String = FilenameNoExt(FilenameNoPath(lFile))
            For Each lDS As atcDataSet In lWDMFile.DataSets
                lUpdate = False
                If lDS.Attributes.GetValue("ID") < 10 Then
                    lLatDeg = lDS.Attributes.GetValue("LatDeg", 0)
                    lLngDeg = lDS.Attributes.GetValue("LngDeg", 0)
                    Dim lLoc As String = Right(FilenameOnly(lFile), 6)
                    If lLngDeg = 0 OrElse lLatDeg = 0 Then 'lat/lng missing from dataset, get from location database
                        lts = lDS.Clone
                        If lStationDBF.FindFirst(1, lLoc) Then
                            If lLatDeg = 0 Then lts.Attributes.SetValue("LatDeg", lStationDBF.Value(4))
                            If lLngDeg = 0 Then lts.Attributes.SetValue("LngDeg", lStationDBF.Value(5))
                            lUpdate = True
                        Else
                            Logger.Dbg("FixLatLng: PROBLEM Updating Lat/Lng for station " & lLoc & " - not found on location database")
                        End If
                    End If
                    If lUpdate Then
                        If lNewWDMfile Is Nothing Then
                            FileCopy(lFile, lNewWDM)
                            lNewWDMfile = New atcWDM.atcDataSourceWDM
                            lNewWDMfile.Open(lNewWDM)
                        End If
                        If lNewWDMfile.AddDataset(lts, atcDataSource.EnumExistAction.ExistReplace) Then
                            Logger.Dbg("FixLatLng: Updated Lat/Lng for " & lts.Attributes.GetValue("Constituent") & " for station " & lLoc)
                        Else
                            Logger.Dbg("FixLatLng: PROBLEM Updating Lat/Lng for " & lts.Attributes.GetValue("Constituent") & " for station " & lLoc)
                        End If
                    End If
                    lts = Nothing
                End If
            Next
            If Not lNewWDMfile Is Nothing Then 'updated WDM file, copy it back to original file
                FileCopy(lNewWDM, lFile)
                Kill(lNewWDM)
                lNewWDMfile.DataSets.Clear()
                lNewWDMfile = Nothing
            End If
            lWDMFile.DataSets.Clear()
            lWDMFile = Nothing
            'Dim lPercent As String = "(" & DoubleToString((100 * lWdmCnt) / lWdmFiles.Count, , pFormat) & "%)"
            'Logger.Dbg("Done " & lWdmCnt & lPercent & lFile & " MemUsage " & MemUsage())
        Next
        'SaveFileString("Summary.txt", lString.ToString)
        'Logger.Dbg("All Done " & lWdmCnt & " WDMs")
    End Sub

    'Private Function MemUsage() As String
    '    System.GC.WaitForPendingFinalizers()
    '    Return "MemoryUsage(MB):" & DoubleToString(Process.GetCurrentProcess.PrivateMemorySize64 / (2 ^ 20), , pFormat) & _
    '                " Local(MB):" & DoubleToString(System.GC.GetTotalMemory(True) / (2 ^ 20), , pFormat)
    'End Function
End Module
