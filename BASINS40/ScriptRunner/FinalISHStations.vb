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

Public Module FinalISHStations
    'Private Const pInputPath As String = "F:\BASINSMet\original\SOD\unzipped\"
    Private Const pStationPath As String = "C:\BasinsMet\Stations\"
    Private Const pOutputPath As String = "C:\BASINSMet\WDMFilled\"

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        Logger.Dbg("FinalISHStations:Start")
        ChDriveDir(pOutputPath)
        Logger.Dbg(" CurDir:" & CurDir())

        Dim lts As atcTimeseries
        Dim lCons As String = ""

        Dim i As Integer
        Dim lStr As String = ""
        Dim lFileStr As String = ""

        Dim lMVal As Double = -999.0
        Dim lMAcc As Double = -998.0
        Dim lFMin As Double = -100.0
        Dim lFMax As Double = 10000.0
        Dim lRepType As Integer = 1 'DBF parsing output format
        Dim lCurWDM As String = pOutputPath & "current.wdm"
        Dim lDBF As atcTableDBF = Nothing
        Dim lStation As String
        Dim lStatePath As String = ""
        Dim lCurPath As String = ""
        Dim lStationDBF As New atcTableDBF
        Dim lConsCnt As Integer

        If lStationDBF.OpenFile(pstationPath & "StationLocs.dbf") Then
            Logger.Dbg("FinalISHStations: Opened Station Location Master file " & pStationPath & "StationLocs.dbf")
        End If

        Dim lISHDBF As New atcTableDBF
        lISHDBF.NumFields = lStationDBF.NumFields + 7
        For i = 1 To lStationDBF.NumFields
            lISHDBF.FieldName(i) = lStationDBF.FieldName(i)
            lISHDBF.FieldType(i) = lStationDBF.FieldType(i)
            lISHDBF.FieldLength(i) = lStationDBF.FieldLength(i)
            lISHDBF.FieldDecimalCount(i) = lStationDBF.FieldDecimalCount(i)
        Next
        i = lStationDBF.NumFields
        lISHDBF.FieldName(i + 1) = "PREC"
        lISHDBF.FieldName(i + 2) = "EVAP"
        lISHDBF.FieldName(i + 3) = "ATEM"
        lISHDBF.FieldName(i + 4) = "WIND"
        lISHDBF.FieldName(i + 5) = "SOLR"
        lISHDBF.FieldName(i + 6) = "DEWP"
        lISHDBF.FieldName(i + 7) = "CLOU"
        For i = lStationDBF.NumFields + 1 To lISHDBF.NumFields
            lISHDBF.FieldType(i) = "N"
            lISHDBF.FieldLength(i) = 8
        Next
        lISHDBF.CurrentRecord = 1

        Logger.Dbg("FinalISHStations: Get all files in data directory " & pOutputPath)
        Dim lFiles As NameValueCollection = Nothing
        AddFilesInDir(lFiles, pOutputPath, True, "*.wdm")
        Logger.Dbg("FinalISHStations: Found " & lFiles.Count & " data files")

        For Each lFile As String In lFiles
            Logger.Dbg("MissingSummary: Opening data file - " & lFile)
            lStatePath = Right(PathNameOnly(lFile), 2)
            If Not isnumeric(lStatePath) Then
                Dim lWDMfile As New atcWDM.atcDataSourceWDM
                lWDMfile.Open(lFile)
                lStation = FilenameNoExt(FilenameNoPath(lfile))
                Logger.Dbg("FinalISHStations: For station " & lStation)
                If lStationDBF.FindFirst(1, lStation) Then
                    lConsCnt = 0
                    lISHDBF.CurrentRecord += 1
                    For i = 1 To lStationDBF.NumFields
                        lISHDBF.Value(i) = lStationDBF.Value(i)
                    Next
                    For i = lStationDBF.NumFields + 1 To lISHDBF.NumFields
                        lISHDBF.Value(i) = 0
                    Next
                    i = lStationDBF.NumFields
                    For Each lts In lWDMfile.DataSets
                        lCons = lts.Attributes.GetValue("Constituent")
                        Select Case lCons.ToUpper
                            Case "HPCP1" : lISHDBF.Value(i + 1) = lts.numValues
                            Case "EVAP" : lISHDBF.Value(i + 2) = lts.numValues
                            Case "ATEMP" : lISHDBF.Value(i + 3) = lts.numValues
                            Case "WIND" : lISHDBF.Value(i + 4) = lts.numValues
                            Case "SOLR" : lISHDBF.Value(i + 5) = lts.numValues
                            Case "DPTEMP" : lISHDBF.Value(i + 6) = lts.numValues
                            Case "CLOU" : lISHDBF.Value(i + 7) = lts.numValues
                        End Select
                    Next
                Else
                    Logger.Dbg("FinalISHStations: PROBLEM - couldn't find station on station file!")
                End If
            End If
        Next
        lISHDBF.WriteFile(pStationPath & "FinalISH.DBF")
        'Logger.Dbg("MissingSummary:Wrote DBF File for " & lCurPath.Substring(0, 2))
        Logger.Dbg("FinalISHStations: Wrote DBF File " & pStationPath & "FinalISH.DBF")

        'Application.Exit()

    End Sub

End Module
