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

Public Module ReadISHUpdates
    Private Const pInputPath As String = "C:\BasinsMet\original\ISH\"
    Private Const pStationPath As String = "C:\BasinsMet\Stations\"
    Private Const pOutputPath As String = "C:\BasinsMet\WDMRaw\"

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        Logger.Dbg("ReadISHUpdates:Start")
        ChDriveDir(pInputPath)
        Logger.Dbg(" CurDir:" & CurDir())

        Dim lNewYears() As String = {"2000", "2001", "2002"}

        Dim lCurWDM As String = pOutputPath & "current.wdm"
        Dim lCurState As String = ""
        Dim lOutWDM As String = ""
        Dim lStaName As String
        Dim lLat As Double
        Dim lLng As Double
        Dim lCnt As Integer

        Dim lStaHistory As String = pStationPath & "ISH_Stations.dbf"
        Dim lNOAADbf As New atcTableDBF
        If lNOAADbf.OpenFile(lStaHistory) Then
            Logger.Dbg("ReadISHUpdates: Opened Station History file " & lStaHistory)
        Else
            Logger.Dbg("ReadIshUpdates: Problem opening Station History file " & lStaHistory)
        End If

        Dim lStaID As String
        Dim lWriteProblem As Boolean = False
        Dim lUpdating As Boolean = False

        Logger.Dbg("ReadISHUpdates: Updating SOD data from directory " & pInputPath)
        For Each lYear As String In lNewYears
            lCnt = 0
            Dim lFiles As New NameValueCollection
            Logger.Dbg(" ")
            Logger.Dbg("ReadISHUpdates: Updating for Year " & lYear)
            AddFilesInDir(lFiles, pInputPath & lYear & "\", True)
            Logger.Dbg("ReadISHUpdates: Found " & lFiles.Count & " update files")

            For Each lFile As String In lFiles
                Logger.Dbg("ReadISHUpdates: Opening data file - " & lFile)
                Dim lNOAAFile As New atcNOAAISH.atcDataSourceNOAAISH
                lNOAAFile.Open(lFile)
                lCnt += 1
                lStaID = FilenameNoPath(lFile)
                If lNOAADbf.FindFirst(1, lStaID) Then
                    Logger.Dbg("ReadISHUpdates: " & lStaID & " was found in history file")
                    lStaName = lNOAADbf.Value(3)
                    lLat = lNOAADbf.Value(11)
                    lLng = lNOAADbf.Value(12)
                Else
                    Logger.Dbg("ReadISHUpdates: " & lStaID & " was NOT found in history file")
                    lStaName = ""
                    lLat = -999
                    lLng = -999
                End If
                Logger.Dbg("ReadISHUpdates: Found " & lNOAAFile.DataSets.Count & " data sets")
                If lNOAAFile.DataSets.Count > 0 Then
                    lCurState = lFile.Substring(lFile.Length - 9, 2)
                    Dim lOutFile As New NameValueCollection
                    AddFilesInDir(lOutFile, pOutputPath & lCurState, False, lStaID & ".wdm")
                    If lOutFile.Count = 1 Then 'WDM file exists, copy to working version
                        Logger.Dbg("ReadISHUpdates: Found existing WDM file for station " & lStaID)
                        FileCopy(lOutFile.Item(0), lCurWDM)
                    ElseIf lOutFile.Count > 1 Then
                        Logger.Dbg("ReadISHUpdates: PROBLEM!!!  Found " & lOutFile.Count & " WDM files for station " & lStaID)
                    ElseIf lOutFile.Count = 0 Then 'new station
                        Logger.Dbg("ReadISHUpdates: Creating new WDM file for station - " & lStaID)
                    End If
                    Dim lWDMfile As New atcWDM.atcDataSourceWDM
                    lWDMfile.Open(lCurWDM)
                    'Logger.Dbg("ReadISHUpdates: Opening WDM file - " & lWDMfile.Name)
                    lWriteProblem = False
                    For Each lDS As atcDataSet In lNOAAFile.DataSets
                        lUpdating = False
                        Dim lWDMDS As atcDataSet = Nothing
                        For Each lWDMDS In lWDMfile.DataSets
                            If lDS.Attributes.GetValue("Constituent") = lWDMDS.Attributes.GetValue("Constituent") Then
                                'WDM Dataset exists already, set new data-set number to existing
                                lDS.Attributes.SetValue("ID", lWDMDS.Attributes.GetValue("ID"))
                                'Dim lTS As atcTimeseries = lWDMDS
                                'Logger.Dbg("ReadISHUpdates: Matched Cons " & lTS.Attributes.GetValue("Constituent") & " End of existing: " & lTS.Dates.Value(lTS.numValues))
                                lUpdating = True
                                Exit For
                            End If
                        Next
                        If lUpdating Then
                            If lWDMfile.AddDataset(lDS, atcDataSource.EnumExistAction.ExistAppend) Then
                                Logger.Dbg("ReadISHUpdates: Updated " & lDS.Attributes.GetValue("Constituent") & " on DSN " & lDS.Attributes.GetValue("ID"))
                            Else 'try merging existing and new datasets
                                Dim lDSRevised As atcTimeseries = MergeDatasets(lWDMDS, lDS)
                                If lDSRevised Is Nothing Then
                                    lWriteProblem = True
                                    Logger.Dbg("ReadISHUpdates: PROBLEM updating " & lDS.Attributes.GetValue("Constituent") & " on DSN " & lDS.Attributes.GetValue("ID") & " - Could not Merge")
                                ElseIf lWDMfile.AddDataset(lDSRevised, atcDataSource.EnumExistAction.ExistReplace) Then
                                    Logger.Dbg("ReadISHUpdates: Merged " & lDS.Attributes.GetValue("Constituent") & " on DSN " & lDS.Attributes.GetValue("ID"))
                                Else
                                    lWriteProblem = True
                                    Logger.Dbg("ReadISHUpdates: PROBLEM updating " & lDS.Attributes.GetValue("Constituent") & " on DSN " & lDS.Attributes.GetValue("ID") & " - still failed after Merge")
                                End If
                                lDSRevised = Nothing
                            End If
                        Else 'new dataset, save station attributes
                            lDS.Attributes.SetValue("STANAM", lStaName)
                            lDS.Attributes.SetValue("LATDEG", lLat)
                            lDS.Attributes.SetValue("LNGDEG", lLng)
                            If lWDMfile.AddDataset(lDS, atcDataSource.EnumExistAction.ExistRenumber) Then
                                Logger.Dbg("ReadISHUpdates: NEW data for " & lDS.Attributes.GetValue("Constituent") & " on DSN " & lDS.Attributes.GetValue("ID"))
                            Else
                                Logger.Dbg("ReadISHUpdates: PROBLEM writing new data for " & lDS.Attributes.GetValue("Constituent") & " on DSN " & lDS.Attributes.GetValue("ID"))
                            End If
                        End If
                    Next
                    MkDirPath(pOutputPath & lCurState)
                    lOutWDM = pOutputPath & lCurState & "\" & lStaID & ".wdm"
                    If lWriteProblem Then 'problem, keep existing WDM file, write new with different name
                        lOutWDM = FilenameNoExt(lOutWDM) & "_xxx.wdm"
                        Logger.Dbg("ReadISHUpdates: PROBLEM updating station " & lStaID & " - update saved to " & lOutWDM)
                        FileCopy(lCurWDM, lOutWDM)
                    Else 'everything looks good, remove existing WDM file, then copy to it
                        If FileExists(lOutWDM) Then
                            Kill(lOutWDM)
                            Logger.Dbg("ReadISHUpdates: Updated " & lOutWDM)
                        Else
                            Logger.Dbg("ReadISHUpdates: Created " & lOutWDM)
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
            Logger.Dbg("ReadISHUpdates: Completed Year " & lYear & " - processed " & lCnt & " stations")
        Next
        Logger.Dbg("ReadISHUpdates: Completed Updating of ISH data")

    End Sub

    Private Function MergeDatasets(ByRef aTSExist As atcTimeseries, ByRef aTSNew As atcTimeseries) As atcTimeseries
        'tries to handle the case of updated ISH data starting
        'at the same hour interval that existing data ends
        Dim lTSRevised As atcTimeseries = Nothing
        If aTSExist.Dates.Value(aTSExist.numValues) = aTSNew.Dates.Value(1) Then
            lTSRevised = aTSExist.Clone
            Dim lExistVal As Double = aTSExist.Value(aTSExist.numValues)
            Dim lNewVal As Double = aTSNew.Value(1)
            Dim lInd As Integer = lTSRevised.numValues
            If lExistVal = aTSExist.Attributes.GetValue("TSFILL", -999) AndAlso _
               lNewVal <> aTSNew.Attributes.GetValue("TSFILL", -999) Then
                Logger.Dbg("ReadISHUpdates:MergeDatasets:  Replaced existing value (" & _
                            lExistVal & ") with updated value (" & lNewVal & ")")
                lTSRevised.Value(lInd) = aTSNew.Value(1)
            End If
            lTSRevised.numValues = aTSExist.numValues + aTSNew.numValues - 1
            For i As Integer = 2 To aTSNew.numValues
                lInd += 1
                lTSRevised.Dates.Value(lInd) = aTSNew.Dates.Value(i)
                lTSRevised.Value(lInd) = aTSNew.Value(i)
            Next
            Logger.Dbg("ReadISHUpdates:MergeDatasets:  Merge successful")
            lTSRevised.Attributes.DiscardCalculated()
        Else
            Logger.Dbg("ReadISHUpdates:MergeDatasets:  Merge Failed - Existing End Date: " & aTSExist.Dates.Value(aTSExist.numValues) & _
                       "  New Start Date:    " & aTSNew.Dates.Value(1))
        End If
        Return lTSRevised
    End Function

End Module
