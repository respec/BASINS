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

Public Module FinalSODStations
    Private Const pStationPath As String = "C:\BasinsMet\Stations\"
    Private Const pOutputPath As String = "C:\BASINSMet\WDMFilled\"

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        Logger.Dbg("FinalSODStations:Start")
        ChDriveDir(pOutputPath)
        Logger.Dbg(" CurDir:" & CurDir())

        Dim lts As atcTimeseries
        Dim lCons As String = ""

        Dim i As Integer

        Dim lStation As String
        Dim lStatePath As String = ""
        Dim lCurPath As String = ""
        Dim lStationDBF As New atcTableDBF

        If lStationDBF.OpenFile(pstationPath & "StationLocs.dbf") Then
            Logger.Dbg("FinalSODStations: Opened Station Location Master file " & pStationPath & "StationLocs.dbf")
        End If

        Dim lSODDBF As New atcTableDBF
        lSODDBF.NumFields = lStationDBF.NumFields + 6
        For i = 1 To lStationDBF.NumFields
            lSODDBF.FieldName(i) = lStationDBF.FieldName(i)
            lSODDBF.FieldType(i) = lStationDBF.FieldType(i)
            lSODDBF.FieldLength(i) = lStationDBF.FieldLength(i)
            lSODDBF.FieldDecimalCount(i) = lStationDBF.FieldDecimalCount(i)
        Next
        lSODDBF.FieldName(lStationDBF.NumFields + 1) = "PREC-CNT"
        lSODDBF.FieldType(lStationDBF.NumFields + 1) = "N"
        lSODDBF.FieldLength(lStationDBF.NumFields + 1) = 8
        lSODDBF.FieldName(lStationDBF.NumFields + 2) = "PREC-END"
        lSODDBF.FieldType(lStationDBF.NumFields + 2) = "N"
        lSODDBF.FieldLength(lStationDBF.NumFields + 2) = 8
        lSODDBF.FieldName(lStationDBF.NumFields + 3) = "ATEM-CNT"
        lSODDBF.FieldType(lStationDBF.NumFields + 3) = "N"
        lSODDBF.FieldLength(lStationDBF.NumFields + 3) = 8
        lSODDBF.FieldName(lStationDBF.NumFields + 4) = "ATEM-END"
        lSODDBF.FieldType(lStationDBF.NumFields + 4) = "N"
        lSODDBF.FieldLength(lStationDBF.NumFields + 4) = 8
        lSODDBF.FieldName(lStationDBF.NumFields + 5) = "EVAP-CNT"
        lSODDBF.FieldType(lStationDBF.NumFields + 5) = "N"
        lSODDBF.FieldLength(lStationDBF.NumFields + 5) = 8
        lSODDBF.FieldName(lStationDBF.NumFields + 6) = "EVAP-END"
        lSODDBF.FieldType(lStationDBF.NumFields + 6) = "N"
        lSODDBF.FieldLength(lStationDBF.NumFields + 6) = 8
        lSODDBF.CurrentRecord = 1

        Logger.Dbg("FinalSODStations: Get all files in data directory " & pOutputPath)
        Dim lFiles As NameValueCollection = Nothing
        AddFilesInDir(lFiles, pOutputPath, True, "*.wdm")
        Logger.Dbg("FinalSODStations: Found " & lFiles.Count & " data files")

        For Each lFile As String In lFiles
            Logger.Dbg("FinalSODStations: Opening data file - " & lFile)
            lStatePath = Right(PathNameOnly(lFile), 2)
            If isnumeric(lStatePath) Then
                Dim lWDMfile As New atcWDM.atcDataSourceWDM
                lWDMfile.Open(lFile)
                lStation = FilenameNoExt(FilenameNoPath(lfile))
                Logger.Dbg("FinalSODStations: For station " & lStation)
                If lStationDBF.FindFirst(1, lStation) Then
                    lSODDBF.CurrentRecord += 1
                    For i = 1 To lStationDBF.NumFields
                        lSODDBF.Value(i) = lStationDBF.Value(i)
                    Next
                    For Each lts In lWDMfile.DataSets
                        lCons = lts.Attributes.GetValue("Constituent")
                        Select Case lCons.ToUpper
                            Case "PRCP"
                                lSODDBF.Value(lStationDBF.NumFields + 1) = lts.numValues
                                lSODDBF.Value(lStationDBF.NumFields + 2) = lts.Attributes.GetValue("EJDay")
                            Case "ATEM"
                                lSODDBF.Value(lStationDBF.NumFields + 3) = lts.numValues
                                lSODDBF.Value(lStationDBF.NumFields + 4) = lts.Attributes.GetValue("EJDay")
                            Case "EVAP"
                                lSODDBF.Value(lStationDBF.NumFields + 5) = lts.numValues
                                lSODDBF.Value(lStationDBF.NumFields + 6) = lts.Attributes.GetValue("EJDay")
                        End Select
                    Next
                Else
                    Logger.Dbg("FinalSODStations: PROBLEM - couldn't find station on station file!")
                End If
            End If
        Next
        lSODDBF.WriteFile(pStationPath & "FinalSOD.DBF")
        Logger.Dbg("FinalSODStations: Wrote DBF File " & pStationPath & "FinalSOD.DBF")

        'Application.Exit()

    End Sub

End Module
