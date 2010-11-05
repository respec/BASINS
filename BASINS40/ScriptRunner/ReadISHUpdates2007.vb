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

Public Module ReadISHUpdates2007
    Private Const pInputPath As String = "H:\BasinsMet\original\ISH\"
    Private Const pStationPath As String = "C:\BasinsMet\Stations\"
    Private Const pOutputPath As String = "C:\BASINSMet\WDMFilled\"
    Private Const pUpdateOnly As Boolean = False 'Only add to existing datasets if set to "True"

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        Logger.Dbg("ReadISHUpdates2007:Start")
        ChDriveDir(pInputPath)
        Logger.Dbg(" CurDir:" & CurDir())

        Dim lNewYears() As String = {"2007", "2008", "2009"} '"2003", "2004", "2005"}

        Dim lCurWDM As String = pOutputPath & "current.wdm"
        Dim lOutWDM As String = ""
        Dim lStaName As String
        Dim lLat As Double
        Dim lLng As Double
        Dim lElev As Double
        Dim lState As String
        Dim lCnt As Integer

        Dim lStaHistory As String = pStationPath & "ISH_Stations.dbf"
        Dim lISHDBF As New atcTableDBF
        If lISHDBF.OpenFile(lStaHistory) Then
            Logger.Dbg("ReadISHUpdates2007: Opened Station History file " & lStaHistory)
        Else
            Logger.Dbg("ReadISHUpdates2007: Problem opening Station History file " & lStaHistory)
        End If

        Dim lStaID As String
        Dim lWriteProblem As Boolean = False
        Dim lUpdating As Boolean = False

        Logger.Dbg("ReadISHUpdates2007: Updating ISH data from directory " & pInputPath)
        For Each lYear As String In lNewYears
            lCnt = 0
            Dim lFiles As New NameValueCollection
            Logger.Dbg(" ")
            Logger.Dbg("ReadISHUpdates2007: Updating for Year " & lYear)
            AddFilesInDir(lFiles, pInputPath & lYear & "\", True)
            Logger.Dbg("ReadISHUpdates2007: Found " & lFiles.Count & " update files")

            For Each lFile As String In lFiles
                Logger.Dbg("ReadISHUpdates2007: Opening data file - " & lFile)
                lStaID = FilenameNoPath(lFile).Substring(0, 6)
                If lISHDBF.FindFirst(1, lStaID) Then
                    Dim lNOAAFile As New atcNOAAISH.atcDataSourceNOAAISH
                    lNOAAFile.Open(lFile)
                    lCnt += 1
                    lState = lISHDBF.Value(6)
                    If lState <> "" Then
                        Logger.Dbg("ReadISHToWDM: " & lStaID & " was found in history file")
                        lStaName = lISHDBF.Value(3)
                        lElev = lISHDBF.Value(10) / 10 'ISH Elevation values in 0.1 Meters
                        lLat = lISHDBF.Value(11)
                        lLng = lISHDBF.Value(12)
                        Logger.Dbg("ReadISHUpdates2007: Found " & lNOAAFile.DataSets.Count & " data sets")
                        If lNOAAFile.DataSets.Count > 0 Then
                            Dim lOutFile As New NameValueCollection
                            AddFilesInDir(lOutFile, pOutputPath, True, lStaID & ".wdm")
                            If lOutFile.Count = 1 Then 'WDM file exists, copy to working version
                                Logger.Dbg("ReadISHUpdates2007: Found existing WDM file for station " & lStaID)
                                FileCopy(lOutFile.Item(0), lCurWDM)
                                If Right(PathNameOnly(lOutFile.Item(0)), 2).ToUpper <> lState.ToUpper Then
                                    Logger.Dbg("ReadISHUpdates2007: NOTE - Existing data not in same state as current data")
                                    Logger.Dbg("ReadISHUpdates2007:        *** Removed existing WDM file " & lOutFile.Item(0))
                                    Kill(lOutFile.Item(0))
                                End If
                            ElseIf lOutFile.Count > 1 Then
                                Logger.Dbg("ReadISHUpdates2007: PROBLEM!!!  Found " & lOutFile.Count & " WDM files for station " & lStaID)
                            ElseIf lOutFile.Count = 0 Then 'new station
                                Logger.Dbg("ReadISHUpdates2007: Creating new WDM file for station - " & lStaID)
                            End If
                            Dim lWDMfile As New atcWDM.atcDataSourceWDM
                            lWDMfile.Open(lCurWDM)
                            'Logger.Dbg("ReadISHUpdates2007: Opening WDM file - " & lWDMfile.Name)
                            lWriteProblem = False
                            For Each lDS As atcDataSet In lNOAAFile.DataSets
                                lUpdating = False
                                Dim lWDMDS As atcDataSet = Nothing
                                For Each lWDMDS In lWDMfile.DataSets
                                    If lDS.Attributes.GetValue("Constituent") = lWDMDS.Attributes.GetValue("Constituent") Then
                                        'WDM Dataset exists already, set new data-set number to existing
                                        lDS.Attributes.SetValue("ID", lWDMDS.Attributes.GetValue("ID"))
                                        'Dim lTS As atcTimeseries = lWDMDS
                                        'Logger.Dbg("ReadISHUpdates2007: Matched Cons " & lTS.Attributes.GetValue("Constituent") & " End of existing: " & lTS.Dates.Value(lTS.numValues))
                                        lUpdating = True
                                        Exit For
                                    End If
                                Next
                                If lUpdating Then
                                    If lWDMfile.AddDataset(lDS, atcDataSource.EnumExistAction.ExistAppend) Then
                                        Logger.Dbg("ReadISHUpdates2007: Updated " & lDS.Attributes.GetValue("Constituent") & " on DSN " & lDS.Attributes.GetValue("ID"))
                                    Else 'try merging existing and new datasets
                                        Dim lDSRevised As atcTimeseries = MergeDatasets(lWDMDS, lDS)
                                        If lDSRevised Is Nothing Then
                                            lWriteProblem = True
                                            Logger.Dbg("ReadISHUpdates2007: PROBLEM updating " & lDS.Attributes.GetValue("Constituent") & " on DSN " & lDS.Attributes.GetValue("ID") & " - Could not Merge")
                                        ElseIf lWDMfile.AddDataset(lDSRevised, atcDataSource.EnumExistAction.ExistReplace) Then
                                            Logger.Dbg("ReadISHUpdates2007: Merged " & lDS.Attributes.GetValue("Constituent") & " on DSN " & lDS.Attributes.GetValue("ID"))
                                        Else
                                            lWriteProblem = True
                                            Logger.Dbg("ReadISHUpdates2007: PROBLEM updating " & lDS.Attributes.GetValue("Constituent") & " on DSN " & lDS.Attributes.GetValue("ID") & " - still failed after Merge")
                                        End If
                                        lDSRevised = Nothing
                                    End If
                                ElseIf Not pUpdateOnly Then 'new dataset, save station attributes
                                    lDS.Attributes.SetValue("STANAM", lStaName)
                                    lDS.Attributes.SetValue("LATDEG", lLat)
                                    lDS.Attributes.SetValue("LNGDEG", lLng)
                                    If lWDMfile.AddDataset(lDS, atcDataSource.EnumExistAction.ExistRenumber) Then
                                        Logger.Dbg("ReadISHUpdates2007: NEW data for " & lDS.Attributes.GetValue("Constituent") & " on DSN " & lDS.Attributes.GetValue("ID"))
                                    Else
                                        Logger.Dbg("ReadISHUpdates2007: PROBLEM writing new data for " & lDS.Attributes.GetValue("Constituent") & " on DSN " & lDS.Attributes.GetValue("ID"))
                                    End If
                                End If
                            Next
                            MkDirPath(pOutputPath & lState)
                            lOutWDM = pOutputPath & lState & "\" & lStaID & ".wdm"
                            If lWriteProblem Then 'problem, keep existing WDM file, write new with different name
                                lOutWDM = FilenameNoExt(lOutWDM) & "_xxx.wdm"
                                Logger.Dbg("ReadISHUpdates2007: PROBLEM updating station " & lStaID & " - update saved to " & lOutWDM)
                                FileCopy(lCurWDM, lOutWDM)
                            Else 'everything looks good, remove existing WDM file, then copy to it
                                If FileExists(lOutWDM) Then
                                    Kill(lOutWDM)
                                    Logger.Dbg("ReadISHUpdates2007: Updated " & lOutWDM)
                                Else
                                    Logger.Dbg("ReadISHUpdates2007: Created " & lOutWDM)
                                End If
                                FileCopy(lCurWDM, lOutWDM)
                            End If
                            lWDMfile.DataSets.Clear()
                            Kill(lCurWDM)
                            lWDMfile = Nothing
                        End If
                    Else
                        Logger.Dbg("ReadISHUpdates2007: Skipping station " & lStaID & " - not in a valid State")
                    End If
                    lNOAAFile.DataSets.Clear()
                    lNOAAFile = Nothing
                Else
                    Logger.Dbg("ReadISHUpdates2007: Skipping station " & lStaID & " - not found in history file")
                End If
            Next
            Logger.Dbg("ReadISHUpdates2007: Completed Year " & lYear & " - processed " & lCnt & " stations")
        Next
        Logger.Dbg("ReadISHUpdates2007: Completed Updating of ISH data")

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
                Logger.Dbg("ReadHPDUpdates-2:MergeDatasets:  Replaced existing value (" & _
                            lExistVal & ") with updated value (" & lNewVal & ")")
                lTSRevised.Value(lInd) = aTSNew.Value(1)
            End If
            lTSRevised.numValues = aTSExist.numValues + aTSNew.numValues - 1
            For i As Integer = 2 To aTSNew.numValues
                lInd += 1
                lTSRevised.Dates.Value(lInd) = aTSNew.Dates.Value(i)
                lTSRevised.Value(lInd) = aTSNew.Value(i)
            Next
            Logger.Dbg("ReadHPDUpdates-2:MergeDatasets:  Merge successful")
            lTSRevised.Attributes.DiscardCalculated()
        Else
            Logger.Dbg("ReadHPDUpdates-2:MergeDatasets:  Merge Failed - Existing End Date: " & aTSExist.Dates.Value(aTSExist.numValues) & _
                       "  New Start Date:    " & aTSNew.Dates.Value(1))
        End If
        Return lTSRevised
    End Function

End Module
