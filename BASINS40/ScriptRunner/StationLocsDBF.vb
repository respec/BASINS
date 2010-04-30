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

Public Module StationLocsDBF
    Private Const pInputPath As String = "C:\BASINSMet\Stations\"
    Private Const pOutputPath As String = "C:\BASINSMet\Stations\"
    Private Const pDataPath As String = "C:\BASINSMet\WDMFiltered\"

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        Logger.Dbg("StationLocsDBF:Start")
        ChDriveDir(pOutputPath)
        Logger.Dbg(" CurDir:" & CurDir())

        Dim lSODStaDBF As New atcTableDBF
        Dim lHPDStaDBF As New atcTableDBF
        Dim lISHStaDBF As New atcTableDBF
        Dim lMissSummDBF As New atcTableDBF
        Dim lMasterStaDBF As New atcTableDBF
        Dim lStation As String = ""
        Dim lCurSta As String = ""
        Dim lStatePath As String = ""
        Dim lCurPath As String = ""
        Dim lCons As String = ""
        Dim lAddMe As Boolean = False

        Dim lFlds() As String = {"", "STAID", "STANAM", "STCODE", "LATDEG", "LNGDEG", "ELEV"}

        lMasterStaDBF.NumFields = lFlds.GetUpperBound(0)
        For i As Integer = 1 To lMasterStaDBF.NumFields
            If i <= 3 Then
                lMasterStaDBF.FieldType(i) = "C"
            Else
                lMasterStaDBF.FieldType(i) = "N"
            End If
            lMasterStaDBF.FieldName(i) = lFlds(i)
            If i = 2 Then
                lMasterStaDBF.FieldLength(i) = 30
            Else
                lMasterStaDBF.FieldLength(i) = 10
            End If
            If i = 4 Or i = 5 Then
                lMasterStaDBF.FieldDecimalCount(i) = 4
            End If
        Next

        If lMissSummDBF.OpenFile(pInputPath & "MissingSummary.dbf") Then
            Logger.Dbg("StationLocsDBF: Opened Missing Data Summary file " & pInputPath & "MissingSummary.dbf")
        End If

        If lSODStaDBF.OpenFile(pInputPath & "coop_Summ.dbf") Then
            Logger.Dbg("StationLocsDBF: Opened SOD Station Summary file " & pInputPath & "coop_Summ.dbf")
        End If

        If lHPDStaDBF.OpenFile(pInputPath & "HPD_Stations.dbf") Then
            Logger.Dbg("StationLocsDBF: Opened HPD Station Summary file " & pInputPath & "HPD_Stations.dbf")
        End If

        If lISHStaDBF.OpenFile(pInputPath & "ISH_Stations.dbf") Then
            Logger.Dbg("StationLocsDBF: Opened ISH Station Summary file " & pInputPath & "ISH_Stations.dbf")
        End If

        'lMasterStaDBF.NumRecords = lSODStaDBF.NumRecords + lISHStaDBF.NumRecords
        lMasterStaDBF.CurrentRecord = 1
        lMissSummDBF.CurrentRecord = 1
        While Not lMissSummDBF.atEOF
            lAddMe = False
            lStation = lMissSummDBF.Value(3)
            If lStation <> lCurSta Then
                Logger.Dbg("StationLocsDBF: Look for Station " & lStation)
                'new station, find it on one of the station history files
                If lMissSummDBF.Value(5).StartsWith("Summary") Then
                    If FileExists(pDataPath & lStation.Substring(0, 2) & "\" & lStation & ".wdm") Then
                        If lSODStaDBF.FindFirst(1, lStation) Then
                            Logger.Dbg("StationLocsDBF: Found Station " & lStation & " on SOD Station History file")
                            lMasterStaDBF.Value(1) = lSODStaDBF.Value(1)
                            lMasterStaDBF.Value(2) = lSODStaDBF.Value(7)
                            lMasterStaDBF.Value(3) = lSODStaDBF.Value(5)
                            lMasterStaDBF.Value(4) = lSODStaDBF.Value(10)
                            lMasterStaDBF.Value(5) = lSODStaDBF.Value(11)
                            lMasterStaDBF.Value(6) = lSODStaDBF.Value(12)
                            lAddMe = True
                        Else
                            Logger.Dbg("StationLocsDBF: PROBLEM - Station " & lStation & " not found on SOD Station History file")
                        End If
                    Else
                        Logger.Dbg("StationLocsDBF: NOTE: No WDM file found for station " & lStation)
                    End If
                ElseIf lMissSummDBF.Value(5).StartsWith("Hourly") Then
                    If FileExists(pDataPath & lStation.Substring(0, 2) & "\" & lStation & ".wdm") Then
                        If lHPDStaDBF.FindFirst(1, lStation) Then
                            Logger.Dbg("StationLocsDBF: Found Station " & lStation & " on HPD Station History file")
                            lMasterStaDBF.Value(1) = lHPDStaDBF.Value(1)
                            lMasterStaDBF.Value(2) = lHPDStaDBF.Value(2)
                            lMasterStaDBF.Value(3) = lHPDStaDBF.Value(4)
                            lMasterStaDBF.Value(4) = lHPDStaDBF.Value(10)
                            lMasterStaDBF.Value(5) = lHPDStaDBF.Value(11)
                            lMasterStaDBF.Value(6) = lHPDStaDBF.Value(9)
                            lAddMe = True
                        Else
                            Logger.Dbg("StationLocsDBF: PROBLEM - Station " & lStation & " not found on HPD Station History file")
                        End If
                    Else
                        Logger.Dbg("StationLocsDBF: Note: No WDM file found for station " & lStation)
                    End If
                ElseIf lMissSummDBF.Value(5).StartsWith("Integrated") Then
                    If lISHStaDBF.FindFirst(1, lStation) Then
                        If FileExists(pDataPath & lISHStaDBF.Value(6) & "\" & lStation & ".wdm") Then
                            Logger.Dbg("StationLocsDBF: Found Station " & lStation & " on ISH Station History file")
                            lMasterStaDBF.Value(1) = lISHStaDBF.Value(1)
                            lMasterStaDBF.Value(2) = lISHStaDBF.Value(3)
                            lMasterStaDBF.Value(3) = lISHStaDBF.Value(6)
                            lMasterStaDBF.Value(4) = lISHStaDBF.Value(11)
                            lMasterStaDBF.Value(5) = lISHStaDBF.Value(12)
                            lMasterStaDBF.Value(6) = CStr(CDbl(lISHStaDBF.Value(10)) / 0.3281)
                            lAddMe = True
                        Else
                            Logger.Dbg("StationLocsDBF: Note: No WDM file found for station " & lStation)
                        End If
                    Else
                        Logger.Dbg("StationLocsDBF: PROBLEM - Station " & lStation & " not found on ISH Station History file")
                    End If
                Else
                    Logger.Dbg("StationLocsDBF:  PROBLEM, Station " & lStation & " not mapped to any station DBF file!")
                End If
                lCurSta = lStation
            End If
            If lAddMe Then
                lMasterStaDBF.CurrentRecord += 1
            End If
            lMissSummDBF.CurrentRecord += 1
        End While
        lMasterStaDBF.NumRecords = lMasterStaDBF.CurrentRecord - 1
        Logger.Dbg("StationLocsDBF: Writing Station Location Master File - " & lMasterStaDBF.NumRecords & " records")
        lMasterStaDBF.WriteFile(pOutputPath & "StationLocs.dbf")
        Logger.Dbg("StationLocsDBF: Completed Station Location Generation")

        'Application.Exit()

    End Sub

End Module
