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

Public Module ReadObsTimesUpdates
    Private Const pInputPath As String = "\\BigRed\BASINSMet\original\SOD\2002\unzipped\"
    Private Const pOutputPath As String = "\\BigRed\BASINSMet\WDMFiltered\ObsTimes\"

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        Logger.Dbg("ReadObsTimesUpdates:Start")
        ChDriveDir(pInputPath)
        Logger.Dbg(" CurDir:" & CurDir())

        Dim lNewYears() As String = {"2002", "2003", "2004"}

        Dim lCurWDM As String = pOutputPath & "current.wdm"
        Dim lCurState As String = ""
        Dim lOutWDM As String = ""

        Dim lStaHistory As String = pOutputPath & "coop_Summ.dbf"
        Dim lNOAADbf As New atcTableDBF
        If lNOAADbf.OpenFile(lStaHistory) Then
            Logger.Dbg("ReadObsTimesUpdates: Opened Station History file " & lStaHistory)
        End If

        Dim lStaID As String
        Dim lWriteProblem As Boolean = False
        Dim lUpdating As Boolean = False

        Logger.Dbg("ReadObsTimesUpdates: Updating SOD data from directory " & pInputPath)
        For Each lYear As String In lNewYears
            Dim lFiles As New NameValueCollection
            Logger.Dbg(" ")
            Logger.Dbg("ReadObsTimesUpdates: Updating for Year " & lYear)
            AddFilesInDir(lFiles, pInputPath, False, "3200*" & lYear & "-" & lYear)
            Logger.Dbg("ReadObsTimesUpdates: Found " & lFiles.Count & " update files")

            For Each lFile As String In lFiles
                Logger.Dbg("ReadObsTimesUpdates: Opening data file - " & lFile)
                Dim lNOAAFile As New atcTimeseriesNOAA.atcDataSourceNOAA
                lNOAAFile.Open(lFile)
                lStaID = FilenameNoPath(FilenameNoExt(lFile)).Substring(5, 6)
                If lNOAADbf.FindFirst(1, lStaID) Then
                    Logger.Dbg("ReadObsTimesUpdates: " & lStaID & " was found in history file")
                Else
                    Logger.Dbg("ReadObsTimesUpdates: " & lStaID & " was NOT found in history file")
                End If
                Logger.Dbg("ReadObsTimesUpdates: Found " & lNOAAFile.DataSets.Count & " data sets")
                If lNOAAFile.DataSets.Count > 0 Then
                    lCurState = lStaID.Substring(0, 2)
                    Dim lOutFile As New NameValueCollection
                    AddFilesInDir(lOutFile, pOutputPath & lCurState, False, lStaID & ".wdm")
                    If lOutFile.Count = 1 Then 'WDM file exists, copy to working version
                        Logger.Dbg("ReadObsTimesUpdates: Found existing WDM file for station " & lStaID)
                        FileCopy(lOutFile.Item(0), lCurWDM)
                    ElseIf lOutFile.Count > 1 Then
                        Logger.Dbg("ReadObsTimesUpdates: PROBLEM!!!  Found " & lOutFile.Count & " WDM files for station " & lStaID)
                    ElseIf lOutFile.Count = 0 Then 'new station
                        Logger.Dbg("ReadObsTimesUpdates: NEW Station - " & lStaID)
                    End If
                    Dim lWDMfile As New atcWDM.atcDataSourceWDM
                    lWDMfile.Open(lCurWDM)
                    'Logger.Dbg("ReadSODUpdates: Opening WDM file - " & lWDMfile.Name)
                    lWriteProblem = False
                    For Each lDS As atcDataSet In lNOAAFile.DataSets
                        Dim lCons As String = lDS.Attributes.GetValue("Constituent")
                        If lCons.EndsWith("-OBS") Then
                            lUpdating = False
                            For Each lWDMDS As atcDataSet In lWDMfile.DataSets
                                If lDS.Attributes.GetValue("Constituent") = lWDMDS.Attributes.GetValue("Constituent") Then
                                    'WDM Dataset exists already, set new data-set number to existing
                                    lDS.Attributes.SetValue("ID", lWDMDS.Attributes.GetValue("ID"))
                                    lUpdating = True
                                    Exit For
                                End If
                            Next
                            If lUpdating Then
                                If lWDMfile.AddDataset(lDS, atcDataSource.EnumExistAction.ExistAppend) Then
                                    Logger.Dbg("ReadObsTimesUpdates: Updated " & lDS.Attributes.GetValue("Constituent") & " on DSN " & lDS.Attributes.GetValue("ID"))
                                Else
                                    lWriteProblem = True
                                    Logger.Dbg("ReadObsTimesUpdates: PROBLEM updating " & lDS.Attributes.GetValue("Constituent") & " on DSN " & lDS.Attributes.GetValue("ID"))
                                End If
                            ElseIf lWDMfile.AddDataset(lDS, atcDataSource.EnumExistAction.ExistRenumber) Then
                                Logger.Dbg("ReadObsTimesUpdates: NEW data for " & lDS.Attributes.GetValue("Constituent") & " on DSN " & lDS.Attributes.GetValue("ID"))
                            End If
                        End If
                    Next
                    lOutWDM = pOutputPath & lCurState & "\" & lStaID & ".wdm"
                    If lWriteProblem Then 'problem, keep existing WDM file, write new with different name
                        lOutWDM = FilenameNoExt(lOutWDM) & "_xxx.wdm"
                        Logger.Dbg("ReadObsTimesUpdates: PROBLEM updating station " & lStaID & " - update saved to " & lOutWDM)
                        FileCopy(lCurWDM, lOutWDM)
                    Else 'everything looks good, remove existing WDM file, then copy to it
                        If FileExists(lOutWDM) Then
                            Kill(lOutWDM)
                            Logger.Dbg("ReadObsTimesUpdates: Updated " & lOutWDM)
                        Else
                            Logger.Dbg("ReadObsTimesUpdates: Created " & lOutWDM)
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
        'Application.Exit()

    End Sub

End Module
