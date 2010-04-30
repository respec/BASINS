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

Public Module NOAAHPDStations
    Private Const pInputPath As String = "F:\BASINSMet\original\HPD\"
    Private Const pOutputPath As String = "F:\BASINSMet\WDMRaw\"

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        Logger.Dbg("NOAAHPDStations:Start")
        ChDriveDir(pInputPath)
        Logger.Dbg(" CurDir:" & CurDir())

        'Dim lArgs As Object()
        'Dim lErr As String

        'Dim lBasinsPlugIn As Object = Scripting.Run("vb", "", "subFindBasinsPlugIn.vb", lErr, False, aMapWin, aMapWin)
        'If lBasinsPlugIn Is Nothing Then
        '    Logger.Msg("Failed to Find BasinsPlugIn")
        '    Exit Sub
        'End If
        'Dim lDataManager As atcDataManager = lBasinsPlugIn.DataManager
        'If lDataManager Is Nothing Then
        '  Logger.Msg("Failed to Set DataManager")
        '  Exit Sub
        'End If

        Dim lStaFile As String = pInputPath & "HPD_MasterStationList.his"
        Dim lRepType As Integer = 1
        Dim lSta As atcCollection
        Dim lFlds() As String = {"STAID", "STCODE", "STANAM", "START", "END", "LATDEG", "LNGDEG"}
        Dim i As Integer

        Dim lNOAAHPDStations As atcCollection = ReadNOAAHPDAttributes(lStaFile)
        Logger.Dbg("NOAAHPDStations:Opened Station file " & lStaFile & " : " & _
                   lNOAAHPDStations.Count & " stations found")

        If lRepType = 0 Then
            Dim lStr As String = "Staid  ST Station Name                   Start  End    Lat    Long" & vbCrLf

            For Each lSta In lNOAAHPDStations
                lStr &= lSta.ItemByKey("STAID") & " "
                lStr &= lSta.ItemByKey("STCODE") & " "
                lStr &= lSta.ItemByKey("STANAM") & " "
                lStr &= lSta.ItemByKey("START") & " "
                lStr &= lSta.ItemByKey("END") & " "
                lStr &= DoubleToString(lSta.ItemByKey("LATDEG"), 6, "###.00", , , 5) & " "
                lStr &= DoubleToString(lSta.ItemByKey("LNGDEG"), 7, "###.00", , , 6) & vbCrLf
            Next

            Logger.Dbg("NOAAHPDStations:Completed Summaries")
            SaveFileString(FilenameNoExt(lStaFile) & "_Summ.txt", lStr)
            Logger.Dbg("NOAAHPDStations:Wrote Text output file")
        ElseIf lRepType = 1 Then 'build dbf file
            Dim lDBF As New atcTableDBF
            lDBF.NumFields = lFlds.GetUpperBound(0) + 1
            For i = 1 To lDBF.NumFields
                lDBF.FieldName(i) = lFlds(i - 1)
                If lFlds(i - 1).Contains("DEG") Then 'lat/lng
                    lDBF.FieldType(i) = "N"
                    lDBF.FieldDecimalCount(i) = 2
                Else
                    lDBF.FieldType(i) = "C"
                End If
                If lFlds(i - 1).Contains("STANAM") Then
                    lDBF.FieldLength(i) = 33
                Else
                    lDBF.FieldLength(i) = 10
                End If
            Next
            lDBF.NumRecords = lNOAAHPDStations.Count
            lDBF.CurrentRecord = 1
            For Each lSta In lNOAAHPDStations
                For i = 1 To lDBF.NumFields
                    lDBF.Value(i) = lSta.ItemByKey(lFlds(i - 1))
                Next
                lDBF.CurrentRecord += 1
            Next
            lDBF.WriteFile(pOutputPath & FilenameNoExt(FilenameNoPath(lStaFile)) & "_Summ.dbf")
            Logger.Dbg("NOAAHPDStations: Wrote DBF File")
        End If
        'Application.Exit()

    End Sub

End Module
