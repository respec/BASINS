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

Public Module ReadSODToWDM
    Private Const pInputPath As String = "C:\BASINSMet\original\SOD\unzipped\"
    Private Const pStationPath As String = "C:\BasinsMet\Stations\"
    Private Const pOutputPath As String = "C:\BasinsMet\WDMRaw\"

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        Logger.Dbg("ReadSODToWDM:Start")
        ChDriveDir(pInputPath)
        Logger.Dbg(" CurDir:" & CurDir())

        Dim lCurWDM As String = pOutputPath & "current.wdm"
        Dim lCurState As String = ""

        Dim lStaHistory As String = pStationPath & "coop_Summ.dbf"
        Dim lNOAADbf As New atcTableDBF
        If lNOAADbf.OpenFile(lStaHistory) Then
            Logger.Dbg("ReadSODToWDM: Opened Station History file " & lStaHistory)
        End If

        Dim lStaID As String
        Dim lLat As Double
        Dim lLng As Double
        Dim lStaName As String
        Dim lCnt As Integer = 0

        Logger.Dbg("ReadSODToWDM: Get all files in data directory " & pInputPath)
        Dim lFiles As NameValueCollection = nothing
        AddFilesInDir(lFiles, pInputPath, False, "*.dat")
        Logger.Dbg("ReadSODToWDM: Found " & lFiles.Count & " data files")

        For Each lFile As String In lFiles
            Logger.Dbg("ReadSODToWDM: Opening data file - " & lFile)
            lCnt += 1
            Dim lNOAAFile As New atcTimeseriesNOAA.atcDataSourceNOAA
            lNOAAFile.Open(lFile)
            lStaID = FilenameNoPath(FilenameNoExt(lFile)).Substring(0, 6)
            If lNOAADbf.FindFirst(1, lStaID) Then
                Logger.Dbg("ReadSODToWDM: " & lStaID & " was found in history file")
                lStaName = lNOAADbf.Value(7)
                lLat = lNOAADbf.Value(10)
                lLng = lNOAADbf.Value(11)
            Else
                Logger.Dbg("ReadSODToWDM: PROBLEM - " & lStaID & " was NOT found in history file")
                lStaName = ""
                lLat = -999
                lLng = -999
            End If
            Logger.Dbg("ReadSODToWDM: Found " & lNOAAFile.DataSets.Count & " data sets")
            If lNOAAFile.DataSets.Count > 0 Then
                Dim lWDMfile As New atcWDM.atcDataSourceWDM
                lWDMfile.Open(lCurWDM)
                'Logger.Dbg("ReadSODToWDM: Opening WDM file - " & lWDMfile.Name)
                For Each lDS As atcDataSet In lNOAAFile.DataSets
                    lDS.Attributes.SetValue("STANAM", lStaName)
                    lDS.Attributes.SetValue("LATDEG", lLat)
                    lDS.Attributes.SetValue("LNGDEG", lLng)
                    If lWDMfile.AddDataSet(lDS) Then
                        Logger.Dbg("ReadSODToWDM: Saved " & lDS.Attributes.GetValue("Constituent") & " on DSN " & lDS.Attributes.GetValue("ID") & " of WDM file")
                    Else
                        Logger.Dbg("ReadSODToWDM: PROBLEM Saving " & lDS.Attributes.GetValue("Constituent") & " on DSN " & lDS.Attributes.GetValue("ID") & " of WDM file")
                    End If
                Next
                lCurState = FilenameNoPath(lFile).Substring(0, 2)
                MkDirPath(pOutputPath & lCurState)
                'Logger.Dbg("ReadSODToWDM: Copy " & lCurWDM & " to " & pOutputPath & lCurState & "\" & FilenameNoPath(FilenameNoExt(lFile)) & ".wdm")
                FileCopy(lCurWDM, pOutputPath & lCurState & "\" & lStaID & ".wdm")
                lWDMfile.DataSets.Clear()
                Kill(lCurWDM)
                lWDMfile = Nothing
            Else
                Logger.Dbg("ReadSODToWDM:  NOTE - No datasets found for station " & lStaid)
            End If
            lNOAAFile.DataSets.Clear()
            lNOAAFile = Nothing
        Next

        Logger.Dbg("ReadSODToWDM:  Completed processing Original SOD data - " & lCnt & " stations processed")
        'Application.Exit()

    End Sub

End Module
