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

Public Module ReadISHToWDM
    Private Const pInputPath As String = "C:\BASINSMet\original\ISH\95-99\"
    Private Const pStationPath As String = "C:\BasinsMet\Stations\"
    Private Const pOutputPath As String = "C:\BASINSMet\WDMRaw\"

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        Logger.Dbg("ReadISHToWDM:Start")
        ChDriveDir(pInputPath)
        Logger.Dbg(" CurDir:" & CurDir())

        Dim lCurWDM As String = pOutputPath & "current.wdm"
        Dim lCurState As String = ""
        Dim lOutWDM As String = ""
        Dim lStaName As String
        Dim lLat As Double
        Dim lLng As Double
        Dim lCnt As Integer = 0

        Dim lStaHistory As String = pStationPath & "ISH_Stations.dbf"
        Dim lNOAADbf As New atcTableDBF
        If lNOAADbf.OpenFile(lStaHistory) Then
            Logger.Dbg("ReadISHToWDM: Opened Station History file " & lStaHistory)
        End If

        Dim lStaID As String
        'Dim lWBAN As String
        Dim lWriteProblem As Boolean = False

        Logger.Dbg("ReadISHToWDM: Reading ISH data from directory " & pInputPath)
        Dim lFiles As New NameValueCollection
        Logger.Dbg(" ")
        AddFilesInDir(lFiles, pInputPath, True)
        Logger.Dbg("ReadISHToWDM: Found " & lFiles.Count & " ISH Stations")

        For Each lFile As String In lFiles
            Logger.Dbg("ReadISHToWDM: Opening data file - " & lFile)
            Dim lNOAAFile As New atcNOAAISH.atcDataSourceNOAAISH
            lNOAAFile.Open(lFile)
            lCnt += 1
            lStaID = FilenameNoPath(lFile)
            If lNOAADbf.FindFirst(1, lStaID) Then
                Logger.Dbg("ReadISHToWDM: " & lStaID & " was found in history file")
                lStaName = lNOAADbf.Value(3)
                lLat = lNOAADbf.Value(11)
                lLng = lNOAADbf.Value(12)
            Else
                Logger.Dbg("ReadISHToWDM: " & lStaID & " was NOT found in history file")
                lStaName = ""
                lLat = -999
                lLng = -999
            End If
            Logger.Dbg("ReadISHToWDM: Found " & lNOAAFile.DataSets.Count & " data sets")
            If lNOAAFile.DataSets.Count > 0 Then
                lCurState = lFile.Substring(lFile.Length - 9, 2)
                Dim lWDMfile As New atcWDM.atcDataSourceWDM
                lWDMfile.Open(lCurWDM)
                'Logger.Dbg("ReadISHToWDM: Opening WDM file - " & lWDMfile.Name)
                lWriteProblem = False
                For Each lDS As atcDataSet In lNOAAFile.DataSets
                    lDS.Attributes.SetValue("STANAM", lStaName)
                    lDS.Attributes.SetValue("LATDEG", lLat)
                    lDS.Attributes.SetValue("LNGDEG", lLng)
                    If lWDMfile.AddDataset(lDS) Then
                        Logger.Dbg("ReadISHToWDM: Writing " & lDS.Attributes.GetValue("Constituent") & _
                                   " for station " & lStaID & " to dataset " & lDS.Attributes.GetValue("ID"))
                    Else
                        lWriteProblem = True
                        Logger.Dbg("ReadISHToWDM: PROBLEM writing " & lDS.Attributes.GetValue("Constituent") & _
                                   " for station " & lStaID & " for dataset " & lDS.Attributes.GetValue("ID"))
                    End If
                Next
                MkDirPath(pOutputPath & lCurState)
                lOutWDM = pOutputPath & lCurState & "\" & lStaID & ".wdm"
                FileCopy(lCurWDM, lOutWDM)
                lWDMfile.DataSets.Clear()
                Kill(lCurWDM)
                lWDMfile = Nothing
            End If
            lNOAAFile.DataSets.Clear()
            lNOAAFile = Nothing
        Next

        Logger.Dbg("ReadISHToWDM:  Completed Reading ISH Data - " & lCnt & " stations processed")
    End Sub

End Module
