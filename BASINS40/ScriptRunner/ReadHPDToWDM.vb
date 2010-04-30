Imports Microsoft.VisualBasic
Imports System.Collections
Imports System.Collections.Specialized
Imports System.IO
Imports System.Windows.Forms
'Imports MapWinGIS
Imports MapWindow.Interfaces
Imports MapWinUtility
'Imports BASINS

Imports atcUtility
Imports atcData

Public Module ReadHPDToWDM
    Private Const pInputPath As String = "C:\BASINSMet\original\HPD\Stations\"
    Private Const pStationPath As String = "C:\BasinsMet\Stations\"
    Private Const pOutputPath As String = "C:\BASINSMet\WDMRaw\"

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        Logger.Dbg("ReadHPDToWDM:Start")
        ChDriveDir(pInputPath)
        Logger.Dbg(" CurDir:" & CurDir())

        Dim lCurWDM As String = pOutputPath & "current.wdm"
        Dim lCurState As String = ""
        Dim lOutWDM As String = ""
        Dim lStaName As String
        Dim lLat As Double
        Dim lLng As Double
        Dim lCnt As Integer = 0

        Dim lStaHistory As String = pStationPath & "HPD_Stations.dbf"
        Dim lNOAADbf As New atcTableDBF
        If lNOAADbf.OpenFile(lStaHistory) Then
            Logger.Dbg("ReadHPDToWDM: Opened Station History file " & lStaHistory)
        Else
            Logger.Dbg("ReadHPDToWDM: PROBLEM opening Station History file " & lStaHistory)
        End If

        Dim lStaID As String
        Dim lWriteProblem As Boolean = False

        Logger.Dbg("ReadHPDToWDM: Reading HPD data from directory " & pInputPath)
        Dim lFiles As New NameValueCollection
        Logger.Dbg(" ")
        AddFilesInDir(lFiles, pInputPath, False)
        Logger.Dbg("ReadHPDToWDM: Found " & lFiles.Count & " HPD data files")

        For Each lFile As String In lFiles
            Logger.Dbg("ReadHPDToWDM: Opening data file - " & lFile)
            Dim lNOAAFile As New atcNOAAHPD.atcDataSourceNOAAHPD
            lNOAAFile.Open(lFile)
            lCnt += 1
            lStaID = FilenameNoPath(FilenameNoExt(lFile)).Substring(0, 6)
            If lNOAADbf.FindFirst(1, lStaID) Then
                Logger.Dbg("ReadHPDToWDM: " & lStaID & " was found in history file")
                lStaName = lNOAADbf.Value(2)
                lLat = lNOAADbf.Value(10)
                lLng = lNOAADbf.Value(11)
            Else
                Logger.Dbg("ReadHPDToWDM: " & lStaID & " was NOT found in history file")
                lStaName = ""
                lLat = -999
                lLng = -999
            End If
            'Logger.Dbg("ReadHPDToWDM: Found " & lNOAAFile.DataSets.Count & " data sets")
            If lNOAAFile.DataSets.Count > 0 Then
                lCurState = lStaID.Substring(0, 2)
                Dim lOutFile As New NameValueCollection
                AddFilesInDir(lOutFile, pOutputPath & lCurState, False, lStaID & ".wdm")
                If lOutFile.Count = 0 Then 'no WDM file, new station
                    Logger.Dbg("ReadHPDToWDM: NEW WDM file created for station " & lStaID)
                ElseIf lOutFile.Count = 1 Then 'WDM file exists, copy to working version
                    Logger.Dbg("ReadHPDToWDM: Found existing WDM file for station " & lStaID)
                    FileCopy(lOutFile.Item(0), lCurWDM)
                ElseIf lOutFile.Count > 1 Then
                    Logger.Dbg("ReadHPDToWDM: PROBLEM!!!  Found " & lOutFile.Count & " WDM files for station " & lStaID)
                End If
                Dim lWDMfile As New atcWDM.atcDataSourceWDM
                lWDMfile.Open(lCurWDM)
                'Logger.Dbg("ReadHPDToWDM: Opening WDM file - " & lWDMfile.Name)
                lWriteProblem = False
                For Each lDS As atcDataSet In lNOAAFile.DataSets
                    lDS.Attributes.SetValue("ID", 100)
                    lDS.Attributes.SetValue("STANAM", lStaName)
                    lDS.Attributes.SetValue("LATDEG", lLat)
                    lDS.Attributes.SetValue("LNGDEG", lLng)
                    If lWDMfile.AddDataset(lDS, atcDataSource.EnumExistAction.ExistRenumber) Then
                        Logger.Dbg("ReadHPDToWDM: Writing HPCP for station " & lStaID & " to dataset " & lDS.Attributes.GetValue("ID"))
                    Else
                        lWriteProblem = True
                        Logger.Dbg("ReadHPDToWDM: PROBLEM writing HPCP for station " & lStaID & " for dataset " & lDS.Attributes.GetValue("ID"))
                    End If
                Next
                lOutWDM = pOutputPath & lCurState & "\" & lStaID & ".wdm"
                If lWriteProblem Then 'problem, keep existing WDM file, write new with different name
                    lOutWDM = FilenameNoExt(lOutWDM) & "_xxx.wdm"
                    Logger.Dbg("ReadHPDToWDM: Copy " & lCurWDM & " to " & lOutWDM)
                Else 'everything looks good, remove existing WDM file, then copy to it
                    If FileExists(lOutWDM) Then
                        Kill(lOutWDM)
                        Logger.Dbg("ReadHPDToWDM: Updated " & lOutWDM)
                    Else
                        Logger.Dbg("ReadHPDToWDM: Created " & lOutWDM)
                    End If
                End If
                FileCopy(lCurWDM, lOutWDM)
                lWDMfile.DataSets.Clear()
                Kill(lCurWDM)
                lWDMfile = Nothing
            Else
                Logger.Dbg("ReadHPDToWDM: NOTE - no datasets found for station " & lStaID)
            End If
            lNOAAFile.DataSets.Clear()
            lNOAAFile = Nothing
        Next
        Logger.Dbg("ReadHPDToWDM:  Completed Reading HPD data - " & lCnt & " stations processed")
        'Application.Exit()

    End Sub

End Module
