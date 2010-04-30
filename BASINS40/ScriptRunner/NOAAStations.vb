Imports Microsoft.VisualBasic
Imports System.Collections
Imports System.IO
Imports System.Windows.Forms
'Imports MapWinGIS
Imports MapWindow.Interfaces
Imports MapWinUtility
'Imports BASINS

Imports atcUtility
Imports atcData

Public Module NOAAStations
    Private Const pInputPath As String = "C:\BASINSMet\Stations\"
    Private Const pOutputPath As String = "C:\BASINSMet\Stations\"

  Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        Logger.Dbg("NOAAStations:Start")
        ChDriveDir(pInputPath)
        Logger.Dbg(" CurDir:" & CurDir())

        Dim lStaFile As String = pInputPath & "coop.txt"
        Dim lRepType As Integer = 1
        Dim lSta As atcCollection
        Dim lFlds() As String = {"STAID", "WBAN", "WMO", "ICAO", "STCODE", "TIME", "STANAM", "START", "END", "LATDEG", "LNGDEG", "ELEV"}
        Dim i As Integer

        Dim lNOAAStations As atcCollection = ReadNOAAAttributes(lStaFile)
        Logger.Dbg("NOAAStations:Opened Station file " & lStaFile & " : " & _
                   lNOAAStations.Count & " stations found")

        If lRepType = 0 Then
            Dim lStr As String = "Staid  ST Station Name                   Start  End    Lat    Long" & vbCrLf

            For Each lSta In lNOAAStations
                lStr &= lSta.ItemByKey("STAID") & " "
                lStr &= lSta.ItemByKey("STCODE") & " "
                lStr &= lSta.ItemByKey("STANAM") & " "
                lStr &= lSta.ItemByKey("START") & " "
                lStr &= lSta.ItemByKey("END") & " "
                lStr &= DoubleToString(lSta.ItemByKey("LATDEG"), 6, "###.00", , , 5) & " "
                lStr &= DoubleToString(lSta.ItemByKey("LNGDEG"), 7, "###.00", , , 6) & vbCrLf
            Next

            Logger.Dbg("NOAAStations:Completed Summaries")
            SaveFileString(FilenameNoExt(lStaFile) & "_Summ.txt", lStr)
            Logger.Dbg("NOAAStations:Wrote Text output file")
        ElseIf lRepType = 1 Then 'build dbf file
            Dim lDBF As New atcTableDBF
            lDBF.NumFields = lFlds.GetUpperBound(0) + 1
            For i = 1 To lDBF.NumFields
                lDBF.FieldName(i) = lFlds(i - 1)
                If lFlds(i - 1).Contains("DEG") Or lFlds(i - 1).Contains("TIME") Then 'lat/lng or time
                    lDBF.FieldType(i) = "N"
                    If lFlds(i - 1).Contains("DEG") Then
                        lDBF.FieldDecimalCount(i) = 2
                    Else
                        lDBF.FieldDecimalCount(i) = 0
                    End If
                Else
                    lDBF.FieldType(i) = "C"
                End If
                If lFlds(i - 1).Contains("STANAM") Then
                    lDBF.FieldLength(i) = 33
                Else
                    lDBF.FieldLength(i) = 10
                End If
            Next
            lDBF.NumRecords = lNOAAStations.Count
            lDBF.CurrentRecord = 1
            For Each lSta In lNOAAStations
                For i = 1 To lDBF.NumFields
                    lDBF.Value(i) = lSta.ItemByKey(lFlds(i - 1))
                Next
                lDBF.CurrentRecord += 1
            Next
            lDBF.WriteFile(pOutputPath & FilenameNoExt(FilenameNoPath(lStaFile)) & "_Summ.dbf")
            Logger.Dbg("NOAAStations: Wrote DBF File")
        End If
        'Application.Exit()

    End Sub

End Module
