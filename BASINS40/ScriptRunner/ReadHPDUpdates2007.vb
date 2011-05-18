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

Public Module ReadHPDUpdates2007
    Private Const pInputPath As String = "H:\BasinsMet\original\HPD\2007\"
    Private Const pStationPath As String = "C:\BasinsMet\Stations\"
    Private Const pOutputPath As String = "C:\BASINSMet\WDMFilled\"
    Private Const pUpdateOnly As Boolean = True 'Only add to existing datasets if set to "True"

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        Logger.StartToFile(pOutputPath & "ReadHPDUpdates2007.log")
        Logger.Dbg("ReadHPDUpdates2007:Start")
        ChDriveDir(pInputPath)
        Logger.Dbg(" CurDir:" & CurDir())

        Dim lCurWDM As String = pOutputPath & "current.wdm"
        Dim lCurState As String = ""
        Dim lOutWDM As String = ""

        Dim lStaHistory As String = pStationPath & "HPD_Stations.dbf"
        Dim lNOAADbf As New atcTableDBF
        If lNOAADbf.OpenFile(lStaHistory) Then
            Logger.Dbg("ReadHPDUpdates2007: Opened Station History file " & lStaHistory)
        Else
            Logger.Dbg("ReadHPDUpdates2007: PROBLEM opening Station History file " & lStaHistory)
        End If

        Dim lStaID As String
        Dim lElev As Double
        Dim lLat As Double
        Dim lLng As Double
        Dim lStaName As String
        Dim lWriteProblem As Boolean = False
        Dim lUpdating As Boolean = False
        Dim lCnt As Integer = 0

        Logger.Dbg("ReadHPDUpdates2007: Updating HPD data from directory " & pInputPath)
        Dim lFiles As New NameValueCollection
        AddFilesInDir(lFiles, pInputPath, False)
        Logger.Dbg("ReadHPDUpdates2007: Found " & lFiles.Count & " update files")

        For Each lFile As String In lFiles
            Logger.Dbg("ReadHPDUpdates2007: Opening data file - " & lFile)
            Dim lNOAAFile As New atcNOAAHPD.atcDataSourceNOAAHPD
            lNOAAFile.Open(lFile)
            lCnt += 1
            lStaID = FilenameNoPath(lFile).Substring(0, 6)
            If lNOAADbf.FindFirst(1, lStaID) Then
                Logger.Dbg("ReadHPDUpdates2007: " & lStaID & " was found in history file")
                lStaName = lNOAADbf.Value(2)
                lElev = lNOAADbf.Value(9) * 0.3048 'HPD Elevation values in feet, convert to meters
                lLat = lNOAADbf.Value(7)
                lLng = lNOAADbf.Value(8)
                'Logger.Dbg("ReadHPDUpdates2007: Found " & lNOAAFile.DataSets.Count & " data sets")
                If lNOAAFile.DataSets.Count > 0 Then
                    lCurState = lStaID.Substring(0, 2)
                    Dim lOutFile As New NameValueCollection
                    AddFilesInDir(lOutFile, pOutputPath & lCurState, False, lStaID & ".wdm")
                    If lOutFile.Count = 1 Then 'WDM file exists, copy to working version
                        Logger.Dbg("ReadHPDUpdates2007: Found existing WDM file for station " & lStaID)
                        FileCopy(lOutFile.Item(0), lCurWDM)
                    ElseIf lOutFile.Count > 1 Then
                        Logger.Dbg("ReadHPDUpdates2007: PROBLEM!!!  Found " & lOutFile.Count & " WDM files for station " & lStaID)
                    ElseIf lOutFile.Count = 0 Then 'new station
                        'Logger.Dbg("ReadHPDUpdates2007: Creating New WDM file for station " & lStaID)
                        Logger.Dbg("ReadHPDUpdates2007: Updating only; Will NOT create new WDM file for station " & lStaID)
                        GoTo SkipIt
                    End If
                    Dim lWDMfile As New atcWDM.atcDataSourceWDM
                    lWDMfile.Open(lCurWDM)
                    'Logger.Dbg("ReadHPDUpdates2007: Opening WDM file - " & lWDMfile.Name)
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
                                Logger.Dbg("ReadHPDUpdates2007: Updated " & lDS.Attributes.GetValue("Constituent") & " on DSN " & lDS.Attributes.GetValue("ID"))
                            Else 'likely overlapping data, try subset of appending data
                                Dim lSubTS As atcTimeseries = SubsetByDate(lDS, lWDMDS.Attributes.GetValue("EJDay"), lDS.Attributes.GetValue("EJDay"), Nothing)
                                If lSubTS.Attributes.GetValue("EJDay") <= lWDMDS.Attributes.GetValue("EJDay") Then
                                    Logger.Dbg("ReadHPDUpdates2007: No newer data in update file than that on existing DSN " & lWDMDS.Attributes.GetValue("ID"))
                                ElseIf lWDMfile.AddDataset(lSubTS, atcDataSource.EnumExistAction.ExistAppend) Then
                                    Logger.Dbg("ReadHPDUpdates2007: Updated " & lDS.Attributes.GetValue("Constituent") & " on DSN " & lSubTS.Attributes.GetValue("ID") & " starting " & lWDMDS.Attributes.GetValue("EJDay"))
                                Else
                                    lWriteProblem = True
                                    Logger.Dbg("ReadHPDUpdates2007: PROBLEM updating " & lDS.Attributes.GetValue("Constituent") & " on DSN " & lDS.Attributes.GetValue("ID"))
                                End If
                            End If
                            'store Elevation value as WDM attribute
                            lWDMDS.Attributes.SetValue("ELEV", lElev)
                            lWDMfile.WriteAttributes(lWDMDS)
                        ElseIf Not pUpdateOnly Then 'new Dataset, save station attributes
                            lDS.Attributes.SetValue("ID", 100)
                            lDS.Attributes.SetValue("STANAM", lStaName)
                            lDS.Attributes.SetValue("LATDEG", lLat)
                            lDS.Attributes.SetValue("LNGDEG", lLng)
                            If lWDMfile.AddDataset(lDS, atcDataSource.EnumExistAction.ExistRenumber) Then
                                Logger.Dbg("ReadHPDUpdates2007: NEW data for " & lDS.Attributes.GetValue("Constituent") & " on DSN " & lDS.Attributes.GetValue("ID"))
                            End If
                        End If
                    Next
                    lOutWDM = pOutputPath & lCurState & "\" & lStaID & ".wdm"
                    If lWriteProblem Then 'problem, keep existing WDM file, write new with different name
                        lOutWDM = FilenameNoExt(lOutWDM) & "_xxx.wdm"
                        Logger.Dbg("ReadHPDUpdates2007: PROBLEM updating station " & lStaID & " - update saved to " & lOutWDM)
                        FileCopy(lCurWDM, lOutWDM)
                    Else 'everything looks good, remove existing WDM file, then copy to it
                        If FileExists(lOutWDM) Then
                            Kill(lOutWDM)
                            Logger.Dbg("ReadHPDUpdates2007: Updated " & lOutWDM)
                        ElseIf Not pUpdateOnly Then
                            Logger.Dbg("ReadHPDUpdates2007: Created " & lOutWDM)
                        End If
                        FileCopy(lCurWDM, lOutWDM)
                    End If
                    lWDMfile.DataSets.Clear()
                    Kill(lCurWDM)
                    lWDMfile = Nothing
SkipIt:
                Else
                    Logger.Dbg("ReadHPDUpdates2007:  NOTE - No datasets found for station " & lStaID)
                End If
            Else
                Logger.Dbg("ReadHPDUpdates2007: " & lStaID & " was NOT found in history file")
            End If
            lNOAAFile.DataSets.Clear()
            lNOAAFile = Nothing
        Next
        Logger.Dbg("ReadHPDUpdates2007: Completed HPD Updates - " & lCnt & " stations processed")
        'Application.Exit()

    End Sub

End Module
