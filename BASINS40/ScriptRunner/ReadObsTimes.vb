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

Public Module ReadObsTimes
    Private Const pInputPath As String = "F:\BASINSMet\original\SOD\unzipped\"
    Private Const pOutputPath As String = "F:\BASINSMet\WDMFiltered\ObsTimes\"

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        Logger.Dbg("ReadObsTimes:Start")
        ChDriveDir(pInputPath)
        Logger.Dbg(" CurDir:" & CurDir())

        'Dim lArgs As Object()
        'Dim lErr As String

        'Dim lBasinsPlugIn As Object = Scripting.Run("vb", "", "subFindBasinsPlugIn.vb", lErr, False, aMapWin, aMapWin)
        'If lBasinsPlugIn Is Nothing Then
        '    Logger.Msg("Failed to Find BasinsPlugIn")
        '    Exit Sub
        'End If

        'Dim lStr As String
        Dim lCurWDM As String = pOutputPath & "current.wdm"
        Dim lCurState As String = ""
        Dim lCons As String = ""

        Dim lStaHistory As String = pOutputPath & "coop_Summ.dbf"
        Dim lNOAADbf As New atcTableDBF
        lNOAADbf.OpenFile(lStaHistory)
        Logger.Dbg("ReadObsTimes: Opened Station History file " & lStaHistory)

        Dim lStaID As String

        Logger.Dbg("ReadObsTimes: Get all files in data directory " & pInputPath)
        Dim lFiles As NameValueCollection = Nothing
        AddFilesInDir(lFiles, pInputPath, False, "*.dat")
        Logger.Dbg("ReadObsTimes: Found " & lFiles.Count & " data files")

        For Each lFile As String In lFiles
            Logger.Dbg("ReadObsTimes: Opening data file - " & lFile)
            Dim lNOAAFile As New atcTimeseriesNOAA.atcDataSourceNOAA
            lNOAAFile.Open(lFile)
            lStaID = FilenameNoPath(FilenameNoExt(lFile)).Substring(0, 6)
            If lNOAADbf.FindFirst(1, lStaID) Then
                Logger.Dbg("ReadObsTimes: " & lStaID & " was found in history file")
            Else
                Logger.Dbg("ReadObsTimes: " & lStaID & " was NOT found in history file")
            End If
            Logger.Dbg("ReadObsTimes: Found " & lNOAAFile.DataSets.Count & " data sets")
            If lNOAAFile.DataSets.Count > 0 Then
                Dim lWDMfile As New atcWDM.atcDataSourceWDM
                lWDMfile.Open(lCurWDM)
                'Logger.Dbg("ReadObsTimes: Opening WDM file - " & lWDMfile.Name)
                For Each lDS As atcDataSet In lNOAAFile.DataSets
                    lCons = lDS.Attributes.GetValue("Constituent")
                    If lCons.EndsWith("-OBS") Then
                        'reduce ID by one to match Obs Time DSN with Constituent's DSN
                        Dim lID As Integer = lDS.Attributes.GetValue("ID") - 1
                        lDS.Attributes.SetValue("ID", lID)
                        lWDMfile.AddDataset(lDS)
                    End If
                Next
                lCurState = FilenameNoPath(lFile).Substring(0, 2)
                MkDirPath(pOutputPath & lCurState)
                Logger.Dbg("ReadObsTimes: Copy " & lCurWDM & " to " & pOutputPath & lCurState & "\" & FilenameNoPath(FilenameNoExt(lFile)) & ".wdm")
                FileCopy(lCurWDM, pOutputPath & lCurState & "\" & lStaID & ".wdm")
                lWDMfile.DataSets.Clear()
                Kill(lCurWDM)
                lWDMfile = Nothing
            End If
            lNOAAFile.DataSets.Clear()
            lNOAAFile = Nothing
        Next

        'Application.Exit()

    End Sub

End Module
