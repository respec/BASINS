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

Public Module FinalHPDStations
    Private Const pStationPath As String = "C:\BasinsMet\Stations\"
    Private Const pOutputPath As String = "C:\BASINSMet\WDMFilled\"

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        Logger.Dbg("FinalHPDStations:Start")
        ChDriveDir(pOutputPath)
        Logger.Dbg(" CurDir:" & CurDir())

        Dim lts As atcTimeseries
        Dim lCons As String = ""

        Dim i As Integer

        Dim lDBF As atcTableDBF = Nothing
        Dim lStation As String
        Dim lStatePath As String = ""
        Dim lCurPath As String = ""
        Dim lStationDBF As New atcTableDBF

        If lStationDBF.OpenFile(pstationPath & "StationLocs.dbf") Then
            Logger.Dbg("FinalHPDStations: Opened Station Location Master file " & pStationPath & "StationLocs.dbf")
        End If

        Dim lHPDDBF As New atcTableDBF
        lHPDDBF.NumFields = lStationDBF.NumFields + 2
        For i = 1 To lStationDBF.NumFields
            lHPDDBF.FieldName(i) = lStationDBF.FieldName(i)
            lHPDDBF.FieldType(i) = lStationDBF.FieldType(i)
            lHPDDBF.FieldLength(i) = lStationDBF.FieldLength(i)
            lHPDDBF.FieldDecimalCount(i) = lStationDBF.FieldDecimalCount(i)
        Next
        lHPDDBF.FieldName(lStationDBF.NumFields + 1) = "PREC-CNT"
        lHPDDBF.FieldType(lStationDBF.NumFields + 1) = "N"
        lHPDDBF.FieldLength(lStationDBF.NumFields + 1) = 8
        lHPDDBF.FieldName(lStationDBF.NumFields + 2) = "PREC-END"
        lHPDDBF.FieldType(lStationDBF.NumFields + 2) = "N"
        lHPDDBF.FieldLength(lStationDBF.NumFields + 2) = 8
        lHPDDBF.CurrentRecord = 1

        Logger.Dbg("FinalHPDStations: Get all files in data directory " & pOutputPath)
        Dim lFiles As NameValueCollection = Nothing
        AddFilesInDir(lFiles, pOutputPath, True, "*.wdm")
        Logger.Dbg("FinalHPDStations: Found " & lFiles.Count & " data files")

        For Each lFile As String In lFiles
            Logger.Dbg("FinalHPDStations: Opening data file - " & lFile)
            lStatePath = Right(PathNameOnly(lFile), 2)
            If isnumeric(lStatePath) Then
                Dim lWDMfile As New atcWDM.atcDataSourceWDM
                lWDMfile.Open(lFile)
                lStation = FilenameNoExt(FilenameNoPath(lfile))
                Logger.Dbg("FinalISHStations: For station " & lStation)
                If lStationDBF.FindFirst(1, lStation) Then
                    For Each lts In lWDMfile.DataSets
                        lCons = lts.Attributes.GetValue("Constituent")
                        If lcons = "HPCP" Then
                            lHPDDBF.CurrentRecord += 1
                            For i = 1 To lStationDBF.NumFields
                                lHPDDBF.Value(i) = lStationDBF.Value(i)
                            Next
                            lHPDDBF.Value(lStationDBF.NumFields + 1) = lts.numValues
                            lHPDDBF.Value(lStationDBF.NumFields + 2) = lts.Attributes.GetValue("EJDay")
                        End If
                    Next
                Else
                    Logger.Dbg("FinalHPDStations: PROBLEM - couldn't find station on station file!")
                End If
            End If
        Next
        lHPDDBF.WriteFile(pStationPath & "FinalHPD.DBF")
        Logger.Dbg("FinalHPDStations: Wrote DBF File " & pStationPath & "FinalHPD.DBF")

        'Application.Exit()

    End Sub

End Module
