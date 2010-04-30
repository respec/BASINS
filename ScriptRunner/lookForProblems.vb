Imports atcData
Imports atcUtility
Imports atcSeasons
Imports MapWindow.Interfaces
Imports MapWinUtility

Imports Microsoft.VisualBasic
Imports System.Collections
Imports System.IO
Imports System.Windows.Forms
Imports System.Array

Public Module ScriptSynop
    Private Const pTestPath As String = "C:\BASINS\Data\met_data\CA-Ventura"

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        ChDriveDir(pTestPath)
        Logger.Dbg(" CurDir:" & CurDir)

        Dim lErr As String = ""
        Dim lDataManager As atcDataManager = Scripting.Run("vb", "", "subFindDataManager.vb", lErr, False, aMapWin, aMapWin)

        Dim lWDMfile As atcDataSource = New atcWdm.atcDataSourceWdm
        'Dim lSummary As New atcDataTree.atcDataTreePlugin

        Dim lOutFile As String = "Summary.txt"
        SaveFileString(lOutFile, "Entry" & vbCrLf)

        Dim lWdmName As String = "rainfall.wdm"
        lDataManager.OpenDataSource(lWDMfile, lWdmName, Nothing)
        AppendFileString(lOutFile, " Opened: " & lWdmName & vbCrLf)
        AppendFileString(lOutFile, "   CountWDM: " & lWDMfile.DataSets.Count & vbCrLf)
        AppendFileString(lOutFile, "   CountManager: " & lDataManager.DataSets.Count & vbCrLf)

        Dim lIntervalsToLookAround as Integer = 23
        AppendFileString(lOutFile, "   IntervalsToLookAround: " & lIntervalsToLookAround & vbCrLf & vbCrLf)

        Dim lDsns() As Integer = {1010, 1011, 1012, 1013}
        'Dim lDsns() As Integer = {102, 106, 107, 108, _
        '                          312, 313, 314, 315, 316, 317, 318, 319, 320, _
        '                          4021, 4022, 4023, 4024, 4024, 4025, 4026, 4027, 4028, 4029, 4030, _
        '                          5001, 5002, 5003, 5004, 5005, 5006, 5007, 5008, 5009, 5010, 5011, 5012, 5013, _
        '                          5019, 5020, 5021, 5022, 5023, 5025}

        For Each lTimser As atcTimeseries In lWDMFile.DataSets
            Dim lId As Integer = lTimser.Attributes.GetValue("ID")
            Dim lIndex As Integer = System.Array.IndexOf(lDsns, lId)
            If lIndex >= 0 Then
                For lDateIndex As Integer = 0 To lTimser.NumValues
                    Dim lDateArray(5) As Integer
                    J2Date(lTimser.Dates.Values(lDateIndex), lDateArray)
                    If lDateArray(3) = 8 And lDateArray(4) = 0 Then 'check only at 8am
                        If lTimser.Value(lDateIndex) > 0 Then
                            For lCheckValueIndex As Integer = lDateIndex - 1 To lDateIndex - lIntervalsToLookAround Step -1
                                If lCheckValueIndex > 0 AndAlso lTimser.Value(lCheckValueIndex) > 0 Then
                                    GoTo NoProblem
                                End If
                            Next
                            For lCheckValueIndex As Integer = lDateIndex + 1 To lDateIndex + lIntervalsToLookAround Step 1
                                If lCheckValueIndex < lTimser.numValues AndAlso lTimser.Value(lCheckValueIndex) > 0 Then
                                    GoTo NoProblem
                                End If
                            Next
                            AppendFileString(lOutFile, " Dsn " & lId & _
                                                       " Problem at " & DumpDate(lTimser.Dates.Values(lDateIndex)) & _
                                                       " Value " & Format(lTimser.Value(lDateIndex),"0.00") & vbCrLf)
                        End If
                    End If
NoProblem:
                Next lDateIndex
            End If
        Next lTimser

        AppendFileString(lOutFile, " Done" & vbCrLf)
        Application.Exit()
    End Sub
End Module
