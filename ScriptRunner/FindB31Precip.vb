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

Public Module FindB31Precip
    Private Const pInputPath As String = "C:\BasinsMet\WDMFilled\"
    Private Const pStationPath As String = "C:\BasinsMet\Stations\"

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        Logger.Dbg("FindB31Precip:Start")
        ChDriveDir(pStationPath)
        Logger.Dbg(" CurDir:" & CurDir())

        Dim i As Integer
        Dim lStation As String
        Dim lStaFound As Boolean
        Dim lStations As New atcCollection
        Dim lStates() As String = {"AL", "AZ", "AR", "CA", "CO", "CT", "DE", "FL", "GA", "ID", _
                                   "IL", "IN", "IA", "KS", "KY", "LA", "ME", "MD", "MA", "MI", _
                                   "MN", "MS", "MO", "MT", "NE", "NV", "NH", "NJ", "NM", "NY", _
                                   "NC", "ND", "OH", "OK", "OR", "PA", "RI", "SC", "SD", "TN", _
                                   "TX", "UT", "VT", "VA", "WA", "WV", "WI", "WY", "AK", "HI"}
        Dim lState As String
        Dim lStIds As New atcCollection
        i = 1
        For Each lState In lStates
            If i < 10 Then
                lStIds.Add(lState, "0" & CStr(i))
            Else
                lStIds.Add(lState, CStr(i))
                If i = 48 Then i += 1 'skip unused 48
            End If
            i += 1
        Next

        Dim lB31StnDBF As New atcTableDBF
        If lB31StnDBF.OpenFile("BASINS31Map.dbf") Then
            Logger.Dbg("FindB31Precip: Opened BASINS 3.1 Station Location file BASINS31Map.dbf")
        Else
            Logger.Dbg("FindB31Precip: PROBLEM - Opening BASINS 3.1 Station Location file BASINS31Map.dbf")
        End If

        Dim lHPDFinalDBF As New atcTableDBF
        If lHPDFinalDBF.OpenFile("FinalHPD.dbf") Then
            Logger.Dbg("FindB31Stations: Opened HPD Station Location Master file FinalHPD.dbf")
        Else
            Logger.Dbg("FindB31Stations: PROBLEM - Opening HPD Station Location Master file FinalHPD.dbf")
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
            lState = lStIds.ItemByKey(lB31StnDBF.Value(2))
            If lState.Length > 0 Then
                lStation = lState & Format(CInt(lB31StnDBF.Value(3)), "0000.")
                Logger.Dbg("FindB31Precip: Processing station " & lStation)
                If FileExists(pInputPath & lState & "\" & lStation & ".wdm") Then
                    Logger.Dbg("FindB31Precip: WDM file found for this station")
                    If lHPDFinalDBF.FindFirst(1, lStation) Then
                        If lHPDFinalDBF.Value(8) > 38700 Then
                            Logger.Dbg("FindB31Precip: Data is current!")
                        Else
                            Logger.Dbg("FindB31Precip: PROBLEM - Data is not current - " & DumpDate(CDbl(lHPDFinalDBF.Value(8))))
                        End If
                    Else
                        Logger.Dbg("FindB31Precip: PROBLEM - station not found on final HPD station file")
                    End If
                Else
                    Logger.Dbg("FindB31Precip: PROBLEM - no WDM file found for this station")
                End If
            Else
                Logger.Dbg("FindB31Precip: PROBLEM - State (" & lB31StnDBF.Value(2) & ") not found in state array")
            End If
            lB31StnDBF.CurrentRecord += 1
        End While

        Logger.Dbg("FindB31Stations:  Completed finding BASINS 3.1 Precip data")

    End Sub

End Module
