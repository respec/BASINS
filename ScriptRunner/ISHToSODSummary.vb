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

Public Module ISHToSODSummary
    Private Const pInputPath As String = "F:\BASINSMet\original\ISH\95-99\"
    Private Const pOutputPath As String = "F:\BASINSMet\WDMRaw\"

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        Logger.Dbg("ISHToSODSummary:Start")
        ChDriveDir(pInputPath)
        Logger.Dbg(" CurDir:" & CurDir())

        Dim lArgs As Object()
        Dim lErr As String

        'Dim lBasinsPlugIn As Object = Scripting.Run("vb", "", "subFindBasinsPlugIn.vb", lErr, False, aMapWin, aMapWin)
        'If lBasinsPlugIn Is Nothing Then
        '    Logger.Msg("Failed to Find BasinsPlugIn")
        '    Exit Sub
        'End If

        Dim lStr As String
        Dim lCurWDM As String = pOutputPath & "current.wdm"
        Dim lCurState As String = ""
        Dim lOutWDM As String = ""

        Dim lStaHistory As String = pOutputPath & "coop_Summ.dbf"
        Dim lNOAADbf As New atcTableDBF
        lNOAADbf.OpenFile(lStaHistory)

        Dim lStaID As String
        Dim lWBAN As String
        Dim lWriteProblem As Boolean = False

        Logger.Dbg("ISHToSODSummary: Reading HPD data from directory " & pInputPath)
        Dim lFiles As New NameValueCollection
        Logger.Dbg(" ")
        AddFilesInDir(lFiles, pInputPath, True)
        Dim lBogus As Integer = 0
        Dim lMatch As Integer = 0
        Dim lNoMatch As Integer = 0

        For Each lFile As String In lFiles
            'Logger.Dbg("ISHToSODSummary: Opening data file - " & lFile)
            'Dim lNOAAFile As New atcNOAAHPD.atcDataSourceNOAAHPD
            'lNOAAFile.Open(lFile)
            'lStaID = FilenameNoPath(FilenameNoExt(lFile)).Substring(0, 6)
            lStr = WholeFileString(lFile)
            lWBAN = lStr.Substring(10, 5) 'WBAN Number
            If IsNumeric(lWBAN) Then
                lStaID = FilenameNoPath(lFile)
                If lWBAN = "99999" Then 'bogus WBAN ID
                    Logger.Dbg("ISHToSODSummary:  " & lStaID & "  -  " & lWBAN)
                    lBogus += 1
                Else
                    If lNOAADbf.FindFirst(2, lWBAN) Then
                        Logger.Dbg("ISHToSODSummary:  " & lStaID & "  -  " & lWBAN & "  -  " & lNOAADbf.Value(1))
                        lMatch += 1
                        While lNOAADbf.FindNext(2, lWBAN)
                            Logger.Dbg("ISHToSODSummary:  " & lStaID & "  -  " & lWBAN & "  -  " & lNOAADbf.Value(1) & " ***")
                        End While
                    Else
                        lNoMatch += 1
                        Logger.Dbg("ISHToSODSummary:  " & lStaID & "  -  " & lWBAN)
                    End If
                End If
            End If
            'Logger.Dbg("ISHToSODSummary: Found " & lNOAAFile.DataSets.Count & " data sets")
            'If lNOAAFile.DataSets.Count > 0 Then
            '    lCurState = lStaID.Substring(0, 2)
            '    Dim lOutFile As New NameValueCollection
            '    AddFilesInDir(lOutFile, pOutputPath & lCurState, False, lStaID & ".wdm")
            '    If lOutFile.Count = 0 Then 'nw WDM file, new station
            '        Logger.Dbg("ReadISHToWDM: NEW WDM file created for station " & lStaID)
            '    ElseIf lOutFile.Count = 1 Then 'WDM file exists, copy to working version
            '        Logger.Dbg("ReadISHToWDM: Found existing WDM file for station " & lStaID)
            '        FileCopy(lOutFile.Item(0), lCurWDM)
            '    ElseIf lOutFile.Count > 1 Then
            '        Logger.Dbg("ReadISHToWDM: PROBLEM!!!  Found " & lOutFile.Count & " WDM files for station " & lStaID)
            '    End If
            '    Dim lWDMfile As New atcWDM.atcDataSourceWDM
            '    lWDMfile.Open(lCurWDM)
            '    Logger.Dbg("ReadISHToWDM: Opening WDM file - " & lWDMfile.Name)
            '    lWriteProblem = False
            '    For Each lDS As atcDataSet In lNOAAFile.DataSets
            '        lDS.Attributes.SetValue("ID", 100)
            '        If lWDMfile.AddDataset(lDS, atcDataSource.EnumExistAction.ExistRenumber) Then
            '            Logger.Dbg("ReadISHToWDM: Writing HPCP for station " & lStaID & " to dataset " & lDS.Attributes.GetValue("ID"))
            '        Else
            '            lWriteProblem = True
            '            Logger.Dbg("ReadISHToWDM: PROBLEM writing HPCP for station " & lStaID & " for dataset " & lDS.Attributes.GetValue("ID"))
            '        End If
            '    Next
            '    lOutWDM = pOutputPath & lCurState & "\" & lStaID & ".wdm"
            '    If lWriteProblem Then 'problem, keep existing WDM file, write new with different name
            '        lOutWDM = FilenameNoExt(lOutWDM) & "_xxx.wdm"
            '        Logger.Dbg("ReadISHToWDM: Copy " & lCurWDM & " to " & lOutWDM)
            '    Else 'everything looks good, remove existing WDM file, then copy to it
            '        If FileExists(lOutWDM) Then Kill(lOutWDM)
            '        Logger.Dbg("ReadISHToWDM: Copy " & lCurWDM & " to " & lOutWDM)
            '    End If
            '    FileCopy(lCurWDM, lOutWDM)
            '    lWDMfile.DataSets.Clear()
            '    Kill(lCurWDM)
            '    lWDMfile = Nothing
            'End If
            'lNOAAFile.DataSets.Clear()
            'lNOAAFile = Nothing
        Next
        Logger.Dbg("")
        Logger.Dbg("ISHToSODSummary: Found " & lFiles.Count & " ISH data files")
        Logger.Dbg("ISHToSODSummary: Matching WBAN IDs:     " & lMatch)
        Logger.Dbg("ISHToSODSummary: NOT Matching WBAN IDs: " & lNoMatch)
        Logger.Dbg("ISHToSODSummary: Bogus WBAN IDs:        " & lBogus)

        'Application.Exit()

    End Sub

End Module
