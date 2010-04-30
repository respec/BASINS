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

Public Module SantaClaraDisagg
    Private Const pTestPath As String = "C:\BASINS\Data\met_data\CA-Ventura\"

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        ChDriveDir(pTestPath)
        Logger.Dbg(" CurDir:" & CurDir())

        Dim lErr As String = ""
        Dim lDataManager As atcDataManager = Scripting.Run("vb", "", "subFindDataManager.vb", lErr, False, aMapWin, aMapWin)

        Dim lWDMfile As atcDataSource = New atcWDM.atcDataSourceWDM

        Dim lOutFile As String = "Summary.txt"
        SaveFileString(lOutFile, "Entry" & vbCrLf)

        Dim lDsn2Fix As Integer = 1001
        Dim lDsns() As Integer = {1008, 1004, 1013, 1011, 1007, 1010, 1005, 1006, 1012, 1009, 1002}

        Dim lWdmName As String = "rainfall.wdm"
        lDataManager.OpenDataSource(lWDMfile, lWdmName, Nothing)
        AppendFileString(lOutFile, " Opened: " & lWdmName & vbCrLf)
        AppendFileString(lOutFile, "   Disaggregating DSN " & lDsn2Fix & vbCrLf)

        Dim lIntervalsToLookAround As Integer = 23
        AppendFileString(lOutFile, "   IntervalsToLookAround: " & lIntervalsToLookAround & vbCrLf & vbCrLf)

        Dim lDateArray(5) As Integer
        Dim lJDate As Double
        Dim lFilled As Boolean

        Dim lTimser As atcTimeseries = Nothing
        lTimser = lWDMfile.DataSets.ItemByKey(lDsn2Fix)
        'For Each lTimser As atcTimeseries In lWDMfile.DataSets
        If Not lTimser Is Nothing Then
            Dim lId As Integer = lTimser.Attributes.GetValue("ID")
            If lId = lDsn2Fix Then
                For lDateIndex As Integer = 0 To lTimser.numValues
                    lJDate = lTimser.Dates.Values(lDateIndex)
                    J2Date(lJDate, lDateArray)
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
                            AppendFileString(lOutFile, " Disaggregating " & Format(lTimser.Value(lDateIndex), "0.00") & _
                                                       " at " & DumpDate(lTimser.Dates.Values(lDateIndex)) & vbCrLf)
                            lFilled = False
                            For Each lId In lDsns
                                Dim lts As atcTimeseries = Nothing
                                lts = lWDMfile.DataSets.ItemByKey(lId)
                                'lId = lts.Attributes.GetValue("ID")
                                'Dim lIndex As Integer = System.Array.IndexOf(lDsns, lId)
                                If Not lts Is Nothing Then
                                    If lts.Attributes.GetValue("SJDay") <= lJDate - 1 AndAlso _
                                       lts.Attributes.GetValue("EJDay") >= lJDate Then
                                        Dim lSubTS As atcTimeseries
                                        'don't back up beyond start of TS to fill
                                        If lJDate - 1 > lTimser.Attributes.GetValue("SJDay") Then
                                            lSubTS = SubsetByDate(lts, lJDate - 1, lJDate, Nothing)
                                        Else
                                            lSubTS = SubsetByDate(lts, lTimser.Attributes.GetValue("SJDay"), lJDate, Nothing)
                                        End If
                                        Dim lTotVal As Double = lSubTS.Attributes.GetValue("Sum")
                                        If lTotVal > 0 AndAlso lTotVal <> lSubTS.Values(lSubTS.numValues) Then
                                            'precip exists and it's not all in the last value,
                                            'disaggregate using this TS
                                            AppendFileString(lOutFile, "   Using Dataset " & lts.Attributes.GetValue("ID") & " to disaggregate." & vbCrLf)
                                            Dim lRatio As Double = lTimser.Value(lDateIndex) / lTotVal
                                            Dim lCarry As Double = 0
                                            Dim lFPos As Integer
                                            Dim lRndOff As Double = 0.001
                                            Dim lMaxFillVal As Double = 0
                                            Dim lMaxFillInd As Integer
                                            Dim ld(5) As Integer
                                            For k As Integer = 1 To lSubTS.numValues
                                                lFPos = lDateIndex - lSubTS.numValues + k + 1
                                                lTimser.Value(lFPos) = lRatio * lSubTS.Value(k) + lCarry
                                                If lTimser.Value(lFPos) > Double.Epsilon Then
                                                    lCarry = lTimser.Value(lFPos) - (System.Math.Round(lTimser.Value(lFPos) / lRndOff) * lRndOff)
                                                    lTimser.Value(lFPos) = lTimser.Value(lFPos) - lCarry
                                                Else
                                                    lTimser.Value(lFPos) = 0.0#
                                                End If
                                                If lTimser.Value(lFPos) > lMaxFillVal Then
                                                    lMaxFillVal = lTimser.Value(lFPos)
                                                    lMaxFillInd = lFPos
                                                End If
                                                J2Date(lJDate - 1 + k * JulianHour, ld)
                                                AppendFileString(lOutFile, "      " & ld(0) & "/" & ld(1) & "/" & ld(2) & ":" & ld(3) & " - " & lTimser.Value(lFPos) & vbCrLf)
                                            Next k
                                            If lCarry > 0 Then 'add remainder to max hourly value
                                                lTimser.Value(lMaxFillInd) = lTimser.Value(lMaxFillInd) + lCarry
                                            End If

                                            lFilled = True
                                            Exit For
                                        End If
                                        lSubTS = Nothing
                                    End If
                                End If
                            Next
                            If Not lFilled Then
                                AppendFileString(lOutFile, "NOTE - could not disaggregate this value" & vbCrLf)
                            End If
                        End If
                    End If
NoProblem:
                Next lDateIndex
                'finished going through timeseries, rewrite it
                Dim lSaveTS As atcTimeseries = lTimser.Clone
                lSaveTS.Attributes.SetValue("ID", lSaveTS.Attributes.GetValue("ID") + 4000)
                lWDMfile.AddDataSet(lSaveTS)
            End If
        End If

        AppendFileString(lOutFile, " Done" & vbCrLf)
        Application.Exit()
    End Sub
End Module
