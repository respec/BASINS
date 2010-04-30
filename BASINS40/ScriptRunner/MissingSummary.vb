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
'Imports atcDataTree
'Imports atcEvents

Public Module MissingSummary
    'Private Const pInputPath As String = "F:\BASINSMet\original\SOD\unzipped\"
    Private Const pStationPath As String = "C:\BasinsMet\Stations\"
    Private Const pOutputPath As String = "C:\BASINSMet\WDMRaw\"

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        Logger.Dbg("MissingSummary:Start")
        ChDriveDir(pOutputPath)
        Logger.Dbg(" CurDir:" & CurDir())

        Dim lts As atcTimeseries
        Dim lCons As String = ""

        Dim lStr As String = ""
        Dim lFileStr As String = ""

        Dim lMVal As Double = -999.0
        Dim lMAcc As Double = -998.0
        Dim lFMin As Double = -100.0
        Dim lFMax As Double = 10000.0
        Dim lRepType As Integer = 1 'DBF parsing output format
        Dim lCurWDM As String = pOutputPath & "current.wdm"
        Dim lDBF As atcTableDBF = Nothing
        Dim lStatePath As String = ""
        Dim lCurPath As String = ""
        Dim lFName As String = pStationPath & "MissingSummary.dbf"

        Logger.Dbg("MissingSummary: Get all files in data directory " & pOutputPath)
        Dim lFiles As NameValueCollection = Nothing
        AddFilesInDir(lFiles, pOutputPath, True, "*.wdm")
        Logger.Dbg("MissingSummary: Found " & lFiles.Count & " data files")

        For Each lFile As String In lFiles
            Logger.Dbg("MissingSummary: Opening data file - " & lFile)
            'lStatePath = Right(PathNameOnly(lFile), 2) & "\"
            FileCopy(lFile, lCurWDM)
            Dim lWDMfile As New atcWDM.atcDataSourceWDM
            lWDMfile.Open(lCurWDM)
            lStr = ""
            For Each lts In lWDMfile.DataSets
                lCons = lts.Attributes.GetValue("Constituent")
                If Not lCons.Contains("-OBS") Then
                    Logger.Dbg("MissSummary:Summarizing station DSN " & lts.Attributes.GetValue("ID") & _
                               " - Constituent: " & lCons)
                    lStr &= MissingDataSummary(lts, lMVal, lMAcc, lFMin, lFMax, lRepType)
                End If
            Next
            If lRepType = 0 Then 'send output to text file
                lFileStr &= lStr
            Else 'send output to DBF file
                If lDBF Is Nothing Then 'create DBF file
                    lDBF = BuildDBFSummary(lStr, lFName)
                    If lDBF Is Nothing Then
                        Logger.Dbg("MissingSummary: BIG PROBLEM - Couldn't create DBF file")
                        Exit For
                    End If
                End If
                Add2DBFSummary(lDBF, lStr)
            End If
            lWDMfile.DataSets.Clear()
            Kill(lCurWDM)
            lWDMfile = Nothing
        Next
        Logger.Dbg("MissingSummary:Completed Summaries")
        If lRepType = 0 Then
            SaveFileString(FilenameNoExt(lFName) & ".txt", lFileStr)
            Logger.Dbg("MissingSummary:Wrote Text output file")
        ElseIf lRepType = 1 Then
            lDBF.WriteFile(lFName)
            'Logger.Dbg("MissingSummary:Wrote DBF File for " & lCurPath.Substring(0, 2))
            Logger.Dbg("MissingSummary:Wrote DBF File" & lFName)
        End If

        'Application.Exit()

    End Sub

    Private Function BuildDBFSummary(ByVal aSummStr As String, ByVal aFName As String) As atcTableDBF
        'Try
        Dim i As Integer ', j As Long, TSCount As Long, 
        Dim lFldLen As Integer
        'Dim tmpv As Long, maxv As Long, 
        Dim lTmpStr As String
        Dim lStr As String
        Dim lFldNames As New atcCollection
        Dim lFldTypes As New atcCollection
        Dim lFirstVals As New atcCollection
        Dim lDBF As New atcTableDBF

        Logger.Dbg("BuildDBFSummary:Start")
        lFldLen = 6
        'build dbf file
        'extract field names, first from Station record
        lTmpStr = StrSplit(aSummStr, "STATION:HEADER", "")
        lTmpStr = StrSplit(aSummStr, vbCrLf, "")
        While Len(lTmpStr) > 0
            lFldNames.Add(StrSplit(lTmpStr, ",", ""))
        End While
        Logger.Dbg("BuildDBFSummary:Extracted field names from Station record")

        lTmpStr = StrSplit(aSummStr, "STATION:DATA", "")
        lTmpStr = StrSplit(aSummStr, vbCrLf, "")
        While Len(lTmpStr) > 0
            lStr = StrSplit(lTmpStr, ",", "")
            If lStr.Length > 1 And lStr.StartsWith("0") Then  'preceeding 0 usually means character field
                lFldTypes.Add("C")
            ElseIf Not IsNumeric(lStr) Then
                lFldTypes.Add("C")
            Else
                lFldTypes.Add("N")
            End If
            lFirstVals.Add(lStr)
        End While
        Logger.Dbg("BuildDBFSummary:Extracted field types from Station record")

        'now from Summary record
        lTmpStr = StrSplit(aSummStr, "SUMMARY:HEADER", "")
        lTmpStr = Trim(StrSplit(aSummStr, vbCrLf, ""))
        While Len(lTmpStr) > 0
            lFldNames.Add(StrSplit(lTmpStr, ",", ""))
        End While
        Logger.Dbg("BuildDBFSummary:Extracted field names from Summary record")

        lTmpStr = StrSplit(aSummStr, "SUMMARY:DATA", "")
        lTmpStr = StrSplit(aSummStr, vbCrLf, "")
        While Len(lTmpStr) > 0
            lStr = StrSplit(lTmpStr, ",", "")
            If lStr.IndexOf("/") > 0 Then 'date field
                lFldTypes.Add("D")
            ElseIf lStr.Length > 1 And lStr.StartsWith("0") Then  'preceeding 0 usually means character field
                lFldTypes.Add("C")
            ElseIf Not IsNumeric(lStr) Then
                lFldTypes.Add("C")
            Else
                lFldTypes.Add("N")
            End If
            lFirstVals.Add(lStr)
        End While
        Logger.Dbg("BuildDBFSummary:Extracted field types names from Summary record")
        lDBF.NumFields = lFldNames.Count
        Logger.Dbg("BuildDBFSummary:Set number of fields")

        For i = 0 To lDBF.NumFields - 1
            lDBF.FieldType(i + 1) = lFldTypes.ItemByIndex(i)
            lDBF.FieldName(i + 1) = lFldNames.ItemByIndex(i)
            If lFldTypes.ItemByIndex(i) = "N" Then
                lDBF.FieldLength(i + 1) = lFldLen 'use length of max number for numeric fields
            ElseIf lFldTypes.ItemByIndex(i) = "D" Then
                lDBF.FieldLength(i + 1) = 10
            Else
                If lFldNames.ItemByIndex(i) = "Description" Then
                    lDBF.FieldLength(i + 1) = 32
                Else 'all other character fields use width of 8
                    lDBF.FieldLength(i + 1) = 8
                End If
            End If
            If i < lDBF.NumFields Then
                lDBF.FieldDecimalCount(i + 1) = 0
            Else
                lDBF.FieldDecimalCount(i + 1) = 1 'last field (percent missing) needs a decimal
            End If
        Next i
        Logger.Dbg("BuildDBFSummary:Set DBF Field info")

        lDBF.InitData()
        lDBF.CurrentRecord = 1
        Logger.Dbg("BuildDBFSummary:Initialized data for DBF")
        lFldNames.Clear()
        lFldNames = Nothing
        lFldTypes.Clear()
        lFldTypes = Nothing
        lFirstVals.Clear()
        lFirstVals = Nothing
        Return lDBF
        'Catch ex As Exception
        '    Logger.Dbg("BuildDBFSummary: PROBLEM creating DBF file " & aFName & vbCrLf & ex.ToString)
        '    Return Nothing
        'End Try

    End Function

    Private Sub Add2DBFSummary(ByRef aDBF As atcTableDBF, ByVal aSummStr As String)
        Dim i As Integer
        Dim lTmpStr As String

        'For i = 0 To aDBF.NumFields - 1
        '    aDBF.Value(i + 1) = lFirstVals.ItemByIndex(i)
        'Next i
        'Logger.Dbg("BuildDBFSummary:Set first record values")

        While Len(aSummStr) > 0
            aDBF.CurrentRecord = aDBF.CurrentRecord + 1
            lTmpStr = StrSplit(aSummStr, "STATION:DATA", "")
            lTmpStr = StrSplit(aSummStr, vbCrLf, "")
            i = 0
            While Len(lTmpStr) > 0
                i += 1
                aDBF.Value(i) = StrSplit(lTmpStr, ",", "")
            End While
            lTmpStr = StrSplit(aSummStr, "SUMMARY:DATA", "")
            lTmpStr = StrSplit(aSummStr, vbCrLf, "")
            While Len(lTmpStr) > 0
                i += 1
                aDBF.Value(i) = StrSplit(lTmpStr, ",", "")
            End While
        End While
    End Sub

End Module
