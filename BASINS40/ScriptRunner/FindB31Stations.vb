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

Public Module FindB31Stations
    'Private Const pInputPath As String = "F:\BASINSMet\original\SOD\unzipped\"
    Private Const pStationPath As String = "C:\BasinsMet\Stations\"

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        Logger.Dbg("FindB31Stations:Start")
        ChDriveDir(pStationPath)
        Logger.Dbg(" CurDir:" & CurDir())

        Dim i As Integer
        Dim lStaID As String
        Dim lStation As String
        Dim lStaFound As Boolean
        Dim lStations As New atcCollection

        Dim lB31StnDBF As New atcTableDBF
        If lB31StnDBF.OpenFile("B31Locs.dbf") Then
            Logger.Dbg("FindB31Stations: Opened BASINS 3.1 Station Location file B31Locs.dbf")
        Else
            Logger.Dbg("FindB31Stations: PROBLEM - Opening BASINS 3.1 Station Location file B31Locs.dbf")
        End If

        Dim lISHStnDBF As New atcTableDBF
        If lISHStnDBF.OpenFile("ISH_Stations.dbf") Then
            Logger.Dbg("FindB31Stations: Opened ISH Station Location Master file ISH_Stations.dbf")
        Else
            Logger.Dbg("FindB31Stations: PROBLEM - Opening ISH Station Location Master file ISH_Stations.dbf")
        End If

        Dim lISHFinalDBF As New atcTableDBF
        If lISHFinalDBF.OpenFile("FinalISH.dbf") Then
            Logger.Dbg("FindB31Stations: Opened ISH Station Location Master file FinalISH.dbf")
        Else
            Logger.Dbg("FindB31Stations: PROBLEM - Opening ISH Station Location Master file FinalISH.dbf")
        End If

        Dim lB31Map As New atcTableDBF
        lB31Map.NumFields = 6
        For i = 1 To 4
            lB31Map.FieldName(i) = lB31StnDBF.FieldName(i)
            lB31Map.FieldLength(i) = lB31StnDBF.FieldLength(i)
            lB31Map.FieldType(i) = lB31StnDBF.FieldType(i)
        Next
        lB31Map.FieldName(5) = "ISH ID"
        lB31Map.FieldLength(5) = 8
        lB31Map.FieldType(5) = "C"
        lB31Map.FieldName(6) = "CHECK?"
        lB31Map.FieldLength(6) = 6
        lB31Map.FieldType(6) = "C"

        lB31Map.CurrentRecord = 1
        lB31StnDBF.CurrentRecord = 1
        While Not lB31StnDBF.atEOF
            lStaFound = True 'assume we'll find a station
            lStation = lB31StnDBF.Value(2) & lB31StnDBF.Value(3)
            lStaID = lB31StnDBF.Value(4)
            If lStaID.Length = 4 Then lStaID = "0" & lStaID 'add leading 0 for small IDs
            If Not lStations.Contains(lStation) Then
                lStations.Add(lStation)
                Logger.Dbg("FindB31Stations: For state/station " & lB31StnDBF.Value(2) & _
                           "/" & lB31StnDBF.Value(3) & "(" & lB31StnDBF.Value(1) & _
                           ") - looking to match NWS ID " & lStaID)
                For i = 1 To 3
                    lB31Map.Value(i) = lB31StnDBF.Value(i)
                Next
                lB31Map.Value(4) = lStaID
                lISHStnDBF.CurrentRecord = 1
                Dim lISHID As String = ""
                While lISHStnDBF.FindNext(2, lStaID)
                    If lISHStnDBF.Value(1) <> "999999" Then
                        lISHID = lISHStnDBF.Value(1)
                        Exit While
                    End If
                End While
                If lISHID.Length > 0 Then 'see what kind of data we have for this station
                    lISHFinalDBF.CurrentRecord = 1
                    If lISHFinalDBF.FindFirst(1, lISHID) Then
                        For i = 7 To 13
                            If CLng(lISHFinalDBF.Value(i)) < 87000 Then 'not enough data
                                If i = 7 Then
                                    Logger.Dbg("FindB31Stations:   NOTE - PRECIP insufficient (" & lISHFinalDBF.Value(i) & " values) - need to get PRECIP from HPD")
                                    lB31Map.Value(6) = "Prec"
                                Else
                                    Logger.Dbg("FindB31Stations:   PROBLEM - " & lISHFinalDBF.FieldName(i) & " insufficient (" & lISHFinalDBF.Value(i) & " values)")
                                    lStaFound = False
                                End If
                            ElseIf CLng(lISHFinalDBF.Value(i)) < 96400 Then 'check start/end dates
                                Logger.Dbg("FindB31Stations:   NOTE - Need to check start/end dates, number of values is " & lISHFinalDBF.Value(i))
                                lB31Map.Value(6) = "Dates"
                            End If
                        Next
                    Else
                        Logger.Dbg("FindB31Stations:   PROBLEM - No data in final database for station " & lISHID)
                        lStaFound = False
                    End If
                Else
                    Logger.Dbg("FindB31Stations:   PROBLEM - no ISH station found for NWS ID " & lStaID)
                    lStaFound = False
                End If
                If lStaFound Then
                    lB31Map.Value(5) = lISHID
                    Logger.Dbg("FindB31Stations:    Found sufficient data from station " & lISHID)
                End If
                lB31Map.CurrentRecord += 1
            Else
                Logger.Dbg("FindB31Stations: ***Already processed station " & lStation)
            End If
            lB31StnDBF.CurrentRecord += 1
        End While

        If lB31Map.WriteFile("BASINS31Map.dbf") Then
            Logger.Dbg("FindB31Stations: Wrote BASINS3.1 Map file BASINS31Map.dbf")
        Else
            Logger.Dbg("FindB31Stations: PROBLEM Writing BASINS3.1 Map file BASINS31Map.dbf")
        End If

        Logger.Dbg("FindB31Stations:  Completed finding BASINS 3.1 station data")

    End Sub

End Module
