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

Public Module ReadHPDUpdates
    Private Const pInputPath As String = "C:\BASINSMet\original\HPD\2006\"
    Private Const pStationPath As String = "C:\BasinsMet\Stations\"
    Private Const pOutputPath As String = "C:\BASINSMet\WDMRaw\"

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        Logger.Dbg("ReadHPDUpdates:Start")
        ChDriveDir(pInputPath)
        Logger.Dbg(" CurDir:" & CurDir())

        Dim lCurWDM As String = pOutputPath & "current.wdm"
        Dim lCurState As String = ""
        Dim lOutWDM As String = ""

        Dim lStaHistory As String = pStationPath & "HPD_Stations.dbf"
        Dim lNOAADbf As New atcTableDBF
        If lNOAADbf.OpenFile(lStaHistory) Then
            Logger.Dbg("ReadHPDUpdates: Opened Station History file " & lStaHistory)
        Else
            Logger.Dbg("ReadHPDUpdates: PROBLEM opening Station History file " & lStaHistory)
        End If

        Dim lStaID As String
        Dim lLat As Double
        Dim lLng As Double
        Dim lStaName As String
        Dim lWriteProblem As Boolean = False
        Dim lUpdating As Boolean = False
        Dim lCnt As Integer = 0

        Logger.Dbg("ReadHPDUpdates: Updating HPD data from directory " & pInputPath)
        Dim lFiles As New NameValueCollection
        AddFilesInDir(lFiles, pInputPath, False)
        Logger.Dbg("ReadHPDUpdates: Found " & lFiles.Count & " update files")

        For Each lFile As String In lFiles
            Logger.Dbg("ReadHPDUpdates: Opening data file - " & lFile)
            Dim lNOAAFile As New atcNOAAHPD.atcDataSourceNOAAHPD
            lNOAAFile.Open(lFile)
            lCnt += 1
            lStaID = FilenameNoPath(lFile)
            If lNOAADbf.FindFirst(1, lStaID) Then
                Logger.Dbg("ReadHPDUpdates: " & lStaID & " was found in history file")
                lStaName = lNOAADbf.Value(2)
                lLat = lNOAADbf.Value(7)
                lLng = lNOAADbf.Value(8)
            Else
                Logger.Dbg("ReadHPDUpdates: " & lStaID & " was NOT found in history file")
                lStaName = ""
                lLat = -999
                lLng = -999
            End If
            'Logger.Dbg("ReadHPDUpdates: Found " & lNOAAFile.DataSets.Count & " data sets")
            If lNOAAFile.DataSets.Count > 0 Then
                lCurState = lStaID.Substring(0, 2)
                Dim lOutFile As New NameValueCollection
                AddFilesInDir(lOutFile, pOutputPath & lCurState, False, lStaID & ".wdm")
                If lOutFile.Count = 1 Then 'WDM file exists, copy to working version
                    Logger.Dbg("ReadHPDUpdates: Found existing WDM file for station " & lStaID)
                    FileCopy(lOutFile.Item(0), lCurWDM)
                ElseIf lOutFile.Count > 1 Then
                    Logger.Dbg("ReadHPDUpdates: PROBLEM!!!  Found " & lOutFile.Count & " WDM files for station " & lStaID)
                ElseIf lOutFile.Count = 0 Then 'new station
                    Logger.Dbg("ReadHPDUpdates: Creating New WDM file for station " & lStaID)
                End If
                Dim lWDMfile As New atcWDM.atcDataSourceWDM
                lWDMfile.Open(lCurWDM)
                'Logger.Dbg("ReadHPDUpdates: Opening WDM file - " & lWDMfile.Name)
                lWriteProblem = False
                For Each lDS As atcDataSet In lNOAAFile.DataSets
                    lUpdating = False
                    Dim lWDMDS As atcDataSet = Nothing
                    For Each lWDMDS In lWDMfile.DataSets
                        If lDS.Attributes.GetValue("Constituent") = lWDMDS.Attributes.GetValue("Constituent") Then
                            'WDM Dataset exists already, set new data-set number to existing
                            lDS.Attributes.SetValue("ID", lWDMDS.Attributes.GetValue("ID"))
                            lUpdating = True
                            Exit For
                        End If
                    Next
                    If lUpdating Then
                        If lWDMfile.AddDataset(lDS, atcDataSource.EnumExistAction.ExistAppend) Then
                            Logger.Dbg("ReadHPDUpdates: Updated " & lDS.Attributes.GetValue("Constituent") & " on DSN " & lDS.Attributes.GetValue("ID"))
                        Else 'likely overlapping data, try subset of appending data
                            Dim lSubTS As atcTimeseries = SubsetByDate(lDS, lWDMDS.Attributes.GetValue("EJDay"), lDS.Attributes.GetValue("EJDay"), Nothing)
                            If lSubTS.Attributes.GetValue("EJDay") <= lWDMDS.Attributes.GetValue("EJDay") Then
                                Logger.Dbg("ReadHPDUpdates: No newer data in update file than that on existing DSN " & lWDMDS.Attributes.GetValue("ID"))
                            ElseIf lWDMfile.AddDataset(lSubTS, atcDataSource.EnumExistAction.ExistAppend) Then
                                Logger.Dbg("ReadHPDUpdates: Updated " & lDS.Attributes.GetValue("Constituent") & " on DSN " & lSubTS.Attributes.GetValue("ID") & " starting " & lWDMDS.Attributes.GetValue("EJDay"))
                            Else
                                lWriteProblem = True
                                Logger.Dbg("ReadHPDUpdates: PROBLEM updating " & lDS.Attributes.GetValue("Constituent") & " on DSN " & lDS.Attributes.GetValue("ID"))
                            End If
                        End If
                    Else 'new Dataset, save station attributes
                            lDS.Attributes.SetValue("ID", 100)
                            lDS.Attributes.SetValue("STANAM", lStaName)
                            lDS.Attributes.SetValue("LATDEG", lLat)
                            lDS.Attributes.SetValue("LNGDEG", lLng)
                            If lWDMfile.AddDataset(lDS, atcDataSource.EnumExistAction.ExistRenumber) Then
                                Logger.Dbg("ReadHPDUpdates: NEW data for " & lDS.Attributes.GetValue("Constituent") & " on DSN " & lDS.Attributes.GetValue("ID"))
                            End If
                    End If
                Next
                lOutWDM = pOutputPath & lCurState & "\" & lStaID & ".wdm"
                If lWriteProblem Then 'problem, keep existing WDM file, write new with different name
                    lOutWDM = FilenameNoExt(lOutWDM) & "_xxx.wdm"
                    Logger.Dbg("ReadHPDUpdates: PROBLEM updating station " & lStaID & " - update saved to " & lOutWDM)
                    FileCopy(lCurWDM, lOutWDM)
                Else 'everything looks good, remove existing WDM file, then copy to it
                    If FileExists(lOutWDM) Then
                        Kill(lOutWDM)
                        Logger.Dbg("ReadHPDUpdates: Updated " & lOutWDM)
                    Else
                        Logger.Dbg("ReadHPDUpdates: Created " & lOutWDM)
                    End If
                    FileCopy(lCurWDM, lOutWDM)
                End If
                lWDMfile.DataSets.Clear()
                Kill(lCurWDM)
                lWDMfile = Nothing
            Else
                Logger.Dbg("ReadHPDUpdates:  NOTE - No datasets found for station " & lStaID)
            End If
            lNOAAFile.DataSets.Clear()
            lNOAAFile = Nothing
        Next
        Logger.Dbg("ReadHPDUpdates: Completed HPD Updates - " & lCnt & " stations processed")
        'Application.Exit()

    End Sub

End Module
