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

Public Module ConvertMissSummDates
    'Private Const pInputPath As String = "F:\BASINSMet\original\SOD\unzipped\"
    Private Const pStationPath As String = "C:\BasinsMet\Stations\"
    Private Const pOutputPath As String = "C:\BASINSMet\WDMRaw\"

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        Logger.Dbg("ConvertMissSummDates:Start")
        ChDriveDir(pOutputPath)
        Logger.Dbg(" CurDir:" & CurDir())

        Dim lMissDBF As New atcTableDBF
        Dim lFName As String = pStationPath & "MissingSummary.dbf"
        If lMissDBF.OpenFile(lFName) Then
            Dim lNewMissDBF As New atcTableDBF
            lNewMissDBF.NumFields = lMissDBF.NumFields
            lNewMissDBF.NumRecords = lMissDBF.NumRecords
            'use only 8.3 file name for importing into Access
            lFName = pStationPath & "NwMisSum.dbf"
            For i As Integer = 1 To lMissDBF.NumFields
                lNewMissDBF.FieldName(i) = lMissDBF.FieldName(i)
                lNewMissDBF.FieldLength(i) = lMissDBF.FieldLength(i)
                If i >= 8 And i <= 9 Then
                    lNewMissDBF.FieldType(i) = "N"
                Else
                    lNewMissDBF.FieldType(i) = lMissDBF.FieldType(i)
                End If
                lNewMissDBF.FieldDecimalCount(i) = lMissDBF.FieldDecimalCount(i)
            Next
            Dim lStr As String
            Dim lYr As String
            'Dim lMon As String
            'Dim lDay As String
            lMissDBF.CurrentRecord = 1
            While Not lMissDBF.atEOF
                For i As Integer = 1 To lMissDBF.NumFields
                    lNewMissDBF.Value(i) = lMissDBF.Value(i)
                    If i >= 8 And i <= 9 Then 'convert date values
                        lStr = lNewMissDBF.Value(i)
                        If Len(lStr) > 0 Then
                            lYr = StrSplit(lStr, "/", "")
                            'lMon = StrSplit(lStr, "/", "")
                            'lDay = lStr
                            'lStr = lMon & "/" & lDay & "/" & lYr
                            lNewMissDBF.Value(i) = lYr 'lStr
                        End If
                    End If
                Next
                lMissDBF.CurrentRecord += 1
                lNewMissDBF.CurrentRecord += 1
            End While
            lNewMissDBF.WriteFile(lFName)
        End If
        Logger.Dbg("ConvertMissSummDates: Wrote New Missing Summary DBF File" & lFName)

        'Application.Exit()

    End Sub

End Module
