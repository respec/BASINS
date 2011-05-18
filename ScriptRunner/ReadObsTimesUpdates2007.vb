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

Public Module ReadObsTimesUpdates2007
    Private Const pInputPath As String = "H:\BASINSMet\original\SOD\2007\" '"\\BigRed\BASINSMet\original\SOD\2002\unzipped\"
    Private Const pOutputPath As String = "C:\BASINSMet\WDMFilled\" '"\\BigRed\BASINSMet\WDMFiltered\ObsTimes\"
    Private Const pStationPath As String = "H:\BASINSMet\Stations\"

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        Logger.Dbg("ReadObsTimesUpdates2007:Start")
        ChDriveDir(pInputPath)
        Logger.Dbg(" CurDir:" & CurDir())

        Dim lNewYears() As String = {"2007 = 2009"} '{"2002", "2003", "2004"}

        Dim lCurWDM As String = pOutputPath & "current.wdm"
        Dim lCurState As String = ""
        Dim lOutWDM As String = ""

        Dim lStaHistory As String = pStationPath & "coop_Summ.dbf"
        Dim lNOAADbf As New atcTableDBF
        If lNOAADbf.OpenFile(lStaHistory) Then
            Logger.Dbg("ReadObsTimesUpdates2007: Opened Station History file " & lStaHistory)
        End If

        Dim lStaID As String
        Dim lWriteProblem As Boolean = False
        Dim lUpdating As Boolean = False

        Logger.Dbg("ReadObsTimesUpdates2007: Updating SOD data from directory " & pInputPath)
        For Each lYear As String In lNewYears
            Dim lFiles As New NameValueCollection
            Logger.Dbg(" ")
            Logger.Dbg("ReadObsTimesUpdates2007: Updating for Year " & lYear)
            AddFilesInDir(lFiles, pInputPath, False, "*.dat")
            Logger.Dbg("ReadObsTimesUpdates2007: Found " & lFiles.Count & " update files")

            For Each lFile As String In lFiles
                Logger.Dbg("ReadObsTimesUpdates2007: Opening data file - " & lFile)
                Dim lNOAAFile As New atcTimeseriesNOAA.atcDataSourceNOAA
                lNOAAFile.Open(lFile)
                lStaID = FilenameNoPath(FilenameNoExt(lFile)).Substring(0, 6)
                If lNOAADbf.FindFirst(1, lStaID) Then
                    Logger.Dbg("ReadObsTimesUpdates2007: " & lStaID & " was found in history file")
                Else
                    Logger.Dbg("ReadObsTimesUpdates2007: " & lStaID & " was NOT found in history file")
                    GoTo SkipIt
                End If
                Logger.Dbg("ReadObsTimesUpdates2007: Found " & lNOAAFile.DataSets.Count & " data sets")
                If lNOAAFile.DataSets.Count > 0 Then
                    lCurState = lStaID.Substring(0, 2)
                    Dim lOutFile As New NameValueCollection
                    AddFilesInDir(lOutFile, pOutputPath & lCurState, False, lStaID & ".wdm")
                    If lOutFile.Count = 1 Then 'WDM file exists, copy to working version
                        Logger.Dbg("ReadObsTimesUpdates2007: Found existing WDM file for station " & lStaID)
                        FileCopy(lOutFile.Item(0), lCurWDM)
                    ElseIf lOutFile.Count > 1 Then
                        Logger.Dbg("ReadObsTimesUpdates2007: PROBLEM!!!  Found " & lOutFile.Count & " WDM files for station " & lStaID)
                    ElseIf lOutFile.Count = 0 Then 'new station
                        'Logger.Dbg("ReadObsTimesUpdates2007: NEW Station - " & lStaID)
                        Logger.Dbg("ReadObsTimesUpdates2007: Updating only, skip new Station - " & lStaID)
                        GoTo SkipIt
                    End If
                    Dim lWDMfile As New atcWDM.atcDataSourceWDM
                    lWDMfile.Open(lCurWDM)
                    'Logger.Dbg("ReadSODUpdates: Opening WDM file - " & lWDMfile.Name)
                    lWriteProblem = False
                    For Each lDS As atcDataSet In lNOAAFile.DataSets
                        Dim lCons As String = lDS.Attributes.GetValue("Constituent")
                        If lCons.EndsWith("-OBS") Then
                            lUpdating = False
                            lCons = lCons.Substring(0, lCons.Length - 4) 'look for corresponding Cons
                            For Each lWDMDS As atcDataSet In lWDMfile.DataSets
                                If lCons = lWDMDS.Attributes.GetValue("Constituent") Then
                                    'found Cons corresponding to this ObsTime record
                                    'for 2007 update, make a new ObsTime dataset
                                    Dim lts As atcTimeseries = SubsetByDate(lWDMDS, lWDMDS.Attributes.GetValue("SJDay"), lDS.Attributes.GetValue("SJDay"), Nothing)
                                    lts.Attributes.SetValue("id", 5000 + lWDMDS.Attributes.GetValue("ID"))
                                    lts.Attributes.SetValue("Constituent", lCons & "-OBS")
                                    'write "dummy" dataset up to start of new obs time values
                                    'this assures that the constituent and obs time datasets have the same time span
                                    lWDMfile.AddDataset(lts, atcDataSource.EnumExistAction.ExistReplace)
                                    lDS.Attributes.SetValue("ID", lts.Attributes.GetValue("ID"))
                                    lUpdating = True
                                    Exit For
                                End If
                            Next
                            If lUpdating Then
                                If lWDMfile.AddDataset(lDS, atcDataSource.EnumExistAction.ExistAppend) Then
                                    Logger.Dbg("ReadObsTimesUpdates2007: Added " & lDS.Attributes.GetValue("Constituent") & " on DSN " & lDS.Attributes.GetValue("ID"))
                                Else
                                    lWriteProblem = True
                                    Logger.Dbg("ReadObsTimesUpdates2007: PROBLEM updating " & lDS.Attributes.GetValue("Constituent") & " on DSN " & lDS.Attributes.GetValue("ID"))
                                End If
                            Else 'If lWDMfile.AddDataset(lDS, atcDataSource.EnumExistAction.ExistRenumber) Then
                                Logger.Dbg("ReadObsTimesUpdates2007: Skipping ObsTime data for " & lDS.Attributes.GetValue("Constituent"))
                            End If
                        End If
                    Next
                    lOutWDM = pOutputPath & lCurState & "\" & lStaID & ".wdm"
                    If lWriteProblem Then 'problem, keep existing WDM file, write new with different name
                        lOutWDM = FilenameNoExt(lOutWDM) & "_xxx.wdm"
                        Logger.Dbg("ReadObsTimesUpdates2007: PROBLEM updating station " & lStaID & " - update saved to " & lOutWDM)
                        FileCopy(lCurWDM, lOutWDM)
                    Else 'everything looks good, remove existing WDM file, then copy to it
                        If FileExists(lOutWDM) Then
                            Kill(lOutWDM)
                            Logger.Dbg("ReadObsTimesUpdates2007: Updated " & lOutWDM)
                        Else
                            Logger.Dbg("ReadObsTimesUpdates2007: Created " & lOutWDM)
                        End If
                        FileCopy(lCurWDM, lOutWDM)
                    End If
                    lWDMfile.DataSets.Clear()
                    Kill(lCurWDM)
                    lWDMfile = Nothing
                End If
SkipIt:
                lNOAAFile.DataSets.Clear()
                lNOAAFile = Nothing
            Next
        Next
        'Application.Exit()

    End Sub

End Module
