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

Public Module ReadSODUpdates
    Private Const pInputPath As String = "C:\BASINSMet\original\SOD\2002\unzipped\"
    Private Const pStationPath As String = "C:\BasinsMet\Stations\"
    Private Const pOutputPath As String = "C:\BASINSMet\WDMRaw\"

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        Logger.Dbg("ReadSODUpdates:Start")
        ChDriveDir(pInputPath)
        Logger.Dbg(" CurDir:" & CurDir())

        Dim lNewYears() As String = {"2006"} '"2002", "2003", "2004", "2005"}

        Dim lCurWDM As String = pOutputPath & "current.wdm"
        Dim lCurState As String = ""
        Dim lOutWDM As String = ""

        Dim lStaHistory As String = pStationPath & "coop_Summ.dbf"
        Dim lNOAADbf As New atcTableDBF
        If lNOAADbf.OpenFile(lStaHistory) Then
            Logger.Dbg("ReadSODUpdates: Opened Station History file " & lStaHistory)
        End If

        Dim lStaID As String
        Dim lLat As Double
        Dim lLng As Double
        Dim lStaName As String
        Dim lWriteProblem As Boolean = False
        Dim lUpdating As Boolean = False
        Dim lSJDay As Double
        Dim lEJDay As Double
        Dim lWDMDS As atcDataSet = Nothing

        Logger.Dbg("ReadSODUpdates: Updating SOD data from directory " & pInputPath)
        For Each lYear As String In lNewYears
            Dim lFiles As New NameValueCollection
            Logger.Dbg(" ")
            Logger.Dbg("ReadSODUpdates: Updating for Year " & lYear)
            AddFilesInDir(lFiles, pInputPath, False, "3200*" & lYear & "-" & lYear)
            Logger.Dbg("ReadSODUpdates: Found " & lFiles.Count & " update files")

            For Each lFile As String In lFiles
                Logger.Dbg("ReadSODUpdates: Opening data file - " & lFile)
                Dim lNOAAFile As New atcTimeseriesNOAA.atcDataSourceNOAA
                lNOAAFile.Open(lFile)
                lStaID = FilenameNoPath(FilenameNoExt(lFile)).Substring(5, 6)
                If lNOAADbf.FindFirst(1, lStaID) Then
                    Logger.Dbg("ReadSODUpdates: " & lStaID & " was found in history file")
                    lStaName = lNOAADbf.Value(7)
                    lLat = lNOAADbf.Value(10)
                    lLng = lNOAADbf.Value(11)
                Else
                    Logger.Dbg("ReadSODUpdates: PROBLEM - " & lStaID & " was NOT found in history file")
                    lStaName = ""
                    lLat = -999
                    lLng = -999
                End If
                Logger.Dbg("ReadSODUpdates: Found " & lNOAAFile.DataSets.Count & " data sets")
                If lNOAAFile.DataSets.Count > 0 Then
                    lCurState = lStaID.Substring(0, 2)
                    Dim lOutFile As New NameValueCollection
                    AddFilesInDir(lOutFile, pOutputPath & lCurState, False, lStaID & ".wdm")
                    If lOutFile.Count = 1 Then 'WDM file exists, copy to working version
                        Logger.Dbg("ReadSODUpdates: Found existing WDM file for station " & lStaID)
                        FileCopy(lOutFile.Item(0), lCurWDM)
                    ElseIf lOutFile.Count > 1 Then
                        Logger.Dbg("ReadSODUpdates: PROBLEM!!!  Found " & lOutFile.Count & " WDM files for station " & lStaID)
                    ElseIf lOutFile.Count = 0 Then 'new station
                        Logger.Dbg("ReadSODUpdates: Creating New WDM file for station " & lStaID)
                    End If
                    Dim lWDMfile As New atcWDM.atcDataSourceWDM
                    lWDMfile.Open(lCurWDM)
                    'Logger.Dbg("ReadSODUpdates: Opening WDM file - " & lWDMfile.Name)
                    lWriteProblem = False
                    For Each lDS As atcDataSet In lNOAAFile.DataSets
                        lUpdating = False
                        For Each lWDMDS In lWDMfile.DataSets
                            If lDS.Attributes.GetValue("Constituent") = lWDMDS.Attributes.GetValue("Constituent") Then
                                'WDM Dataset exists already, set new data-set number to existing
                                lDS.Attributes.SetValue("ID", lWDMDS.Attributes.GetValue("ID"))
                                lUpdating = True
                                Exit For
                            End If
                        Next
                        If lUpdating Then
                            lEJDay = lWDMDS.Attributes.GetValue("EJDay")
                            lSJDay = lDS.Attributes.GetValue("SJDay")
                            If lSJDay < lEJDay Then 'datasets overlap, assume update should overwrite existing
                                lSJDay = lWDMDS.Attributes.GetValue("SJDay")
                                lEJDay = lDS.Attributes.GetValue("SJDay")
                                If lEJDay > lSJDay Then 'keep subset of previously existing data
                                    Dim lSubTS As atcTimeseries = SubsetByDate(lWDMDS, lSJDay, lEJDay, Nothing)
                                    If lWDMfile.AddDataset(lSubTS, atcDataSource.EnumExistAction.ExistReplace) Then
                                        Logger.Dbg("ReadSODUpdates: Keep subset of existing data for " & lSubTS.Attributes.GetValue("Constituent") & " on DSN " & lSubTS.Attributes.GetValue("ID") & " from " & DumpDate(lSJDay) & " to " & DumpDate(lEJDay))
                                        lSubTS.Attributes.DiscardCalculated()
                                    Else
                                        Logger.Dbg("ReadSODUpdates: PROBLEM writing subset of existing data for " & lSubTS.Attributes.GetValue("Constituent") & " on DSN " & lSubTS.Attributes.GetValue("ID"))
                                    End If
                                Else 'new data overlaps entire span of existing, delete existing
                                    lWDMfile.RemoveDataset(lWDMDS)
                                End If
                            End If
                            If lWDMfile.AddDataset(lDS, atcDataSource.EnumExistAction.ExistAppend) Then
                                Logger.Dbg("ReadSODUpdates: Updated " & lDS.Attributes.GetValue("Constituent") & " on DSN " & lDS.Attributes.GetValue("ID") & " for period " & DumpDate(lDS.Attributes.GetValue("SJDay")) & " to " & DumpDate(lDS.Attributes.GetValue("EJDay")))
                            Else
                                lWriteProblem = True
                                Logger.Dbg("ReadSODUpdates: PROBLEM updating " & lDS.Attributes.GetValue("Constituent") & " on DSN " & lDS.Attributes.GetValue("ID"))
                            End If
                        Else 'new Dataset, save station attributes
                            lDS.Attributes.SetValue("STANAM", lStaName)
                            lDS.Attributes.SetValue("LATDEG", lLat)
                            lDS.Attributes.SetValue("LNGDEG", lLng)
                            If lWDMfile.AddDataset(lDS, atcDataSource.EnumExistAction.ExistRenumber) Then
                                Logger.Dbg("ReadSODUpdates: NEW data for " & lDS.Attributes.GetValue("Constituent") & " on DSN " & lDS.Attributes.GetValue("ID"))
                            End If
                        End If
                    Next
                    lOutWDM = pOutputPath & lCurState & "\" & lStaID & ".wdm"
                    If lWriteProblem Then 'problem, keep existing WDM file, write new with different name
                        lOutWDM = FilenameNoExt(lOutWDM) & "_xxx.wdm"
                        Logger.Dbg("ReadSODUpdates: PROBLEM updating station " & lStaID & " - update saved to " & lOutWDM)
                        FileCopy(lCurWDM, lOutWDM)
                    Else 'everything looks good, remove existing WDM file, then copy to it
                        If FileExists(lOutWDM) Then
                            Kill(lOutWDM)
                            Logger.Dbg("ReadSODUpdates: Updated " & lOutWDM)
                        Else
                            Logger.Dbg("ReadSODUpdates: Created " & lOutWDM)
                        End If
                        FileCopy(lCurWDM, lOutWDM)
                    End If
                    lWDMfile.DataSets.Clear()
                    Kill(lCurWDM)
                    lWDMfile = Nothing
                End If
                lNOAAFile.DataSets.Clear()
                lNOAAFile = Nothing
            Next
        Next
        Logger.Dbg("ReadSODUpdates: Complete SOD Updates")
        'Application.Exit()

    End Sub

End Module
