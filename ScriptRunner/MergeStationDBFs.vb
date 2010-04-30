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

Public Module FirstFilter
    Private Const pInputPath As String = "\\Bigred\BASINSMet\WDMRaw\"
    Private Const pOutputPath As String = "\\Bigred\BASINSMet\WDMFiltered\"

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        Logger.Dbg("MergeStationDBFs:Start")
        ChDriveDir(pOutputPath)
        Logger.Dbg(" CurDir:" & CurDir())

        Dim lSODStaDBF As New atcTableDBF
        Dim lISHStaDBF As New atcTableDBF
        Dim lMasterStaDBF As New atcTableDBF
        Dim lStation As String = ""
        Dim lStatePath As String = ""
        Dim lCurPath As String = ""
        Dim lCons As String = ""
        Dim lAddMe As Boolean = True

        Dim lFlds() As String = {"", "STAID", "STANAM", "STCODE", "LATDEG", "LNGDEG", "ELEV"}

        lMasterStaDBF.NumFields = lFlds.GetUpperBound(0)
        For i as Integer = 1 to lMasterStaDBF.NumFields
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

        If lSODStaDBF.OpenFile(pInputPath & "coop_Summ.dbf") Then
             Logger.Dbg("MergeStationDBFs: Opened SOD Station Summary file " & pInputPath & "coop_Summ.dbf")
        End If

        If lISHStaDBF.OpenFile(pInputPath & "ISHStations.dbf") Then
             Logger.Dbg("MergeStationDBFs: Opened ISH Station Summary file " & pInputPath & "ISHStations.dbf")
        End If

        lMasterStaDBF.NumRecords = lSODStaDBF.NumRecords + lISHStaDBF.NumRecords
        lMasterStaDBF.CurrentRecord = 1
        lSODStaDBF.CurrentRecord = 1
        While Not lSODStaDBF.atEOF
            If Not lSODStaDBF.Value(1).StartsWith("91") OrElse lSODStaDBF.Value(5) = "HI" Then
                lMasterStaDBF.Value(1) = lSODStaDBF.Value(1)
                lMasterStaDBF.Value(2) = lSODStaDBF.Value(7)
                lMasterStaDBF.Value(3) = lSODStaDBF.Value(5)
                lMasterStaDBF.Value(4) = lSODStaDBF.Value(10)
                lMasterStaDBF.Value(5) = lSODStaDBF.Value(11)
                lMasterStaDBF.Value(6) = lSODStaDBF.Value(12)
                lMasterStaDBF.CurrentRecord += 1
            End If
            lSODStaDBF.CurrentRecord += 1
        End While

        lISHStaDBF.CurrentRecord = 1
        While Not lISHStaDBF.atEOF
            If lISHStaDBF.Value(6) = "US" Then 'only include US ISH Stations
                If lISHStaDBF.Value(5).Length > 0 Then 'weed out bouys/ships w/out state names
                    lMasterStaDBF.Value(1) = lISHStaDBF.Value(1)
                    lMasterStaDBF.Value(2) = lISHStaDBF.Value(4)
                    lMasterStaDBF.Value(3) = lISHStaDBF.Value(5)
                    lMasterStaDBF.Value(4) = lISHStaDBF.Value(7)
                    lMasterStaDBF.Value(5) = lISHStaDBF.Value(8)
                    lMasterStaDBF.Value(6) = lISHStaDBF.Value(9)
                    lMasterStaDBF.CurrentRecord += 1
                End If
            End If
            lISHStaDBF.CurrentRecord += 1
        End While

        Logger.Dbg("MergeStationDBFs:Writing Merged DBF")
        lMasterStaDBF.WriteFile("C:\dev\BASINS40\BASINS\StationMaster.dbf")
        Logger.Dbg("MergeStationDBFs:Completed Merging")

        'Application.Exit()

    End Sub

End Module
