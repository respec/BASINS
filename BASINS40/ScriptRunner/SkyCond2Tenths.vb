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

Public Module SkyCond2Tenths
    Private Const pOutputPath As String = "C:\BASINSMet\WDMFiltered\"
    Private Const pStationPath As String = "C:\BASINSMet\Stations\"

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        Logger.Dbg("SkyCond2Tenths:Start")
        ChDriveDir(pOutputPath)
        Logger.Dbg(" CurDir:" & CurDir())

        Dim lMissingDBF As New atcTableDBF
        Dim lISHWDMFile As atcWDM.atcDataSourceWDM = Nothing
        Dim lNewWDMFile As atcWDM.atcDataSourceWDM = Nothing
        Dim lStation As String = ""
        Dim lState As String = ""
        Dim lCons As String = ""
        Dim lts As atcTimeseries
        Dim lCurWDM As String = pOutputPath & "current.wdm"
        Dim ltsCloud As atcTimeseries
        Dim lNSta As Integer = 0
        Dim lNSky As Integer = 0

        'If lMissingDBF.OpenFile(pStationPath & "MissingSummary.dbf") Then
        '    Logger.Dbg("SkyCond2Tenths: Opened Station Location file " & pStationPath & "MissingSummary.dbf")
        'End If

        Dim lFiles As New NameValueCollection
        AddFilesInDir(lFiles, pOutputPath, True, "*.wdm")
        Logger.Dbg("SkyCond2Tenths: Found " & lFiles.Count & " WDM data files")
        For Each lFile As String In lFiles
            lState = Right(PathNameOnly(lFile), 2)
            If Not IsNumeric(lState) Then
                lNSta += 1
                Logger.Dbg("SkyCond2Tenths: Opening data file - " & lFile)
                FileCopy(lFile, lCurWDM)
                lISHWDMFile = New atcWDM.atcDataSourceWDM
                lISHWDMFile.Open(lCurWDM)
                lts = Nothing
                For Each lts In lISHWDMFile.DataSets
                    lCons = lts.Attributes.GetValue("Constituent")
                    If lCons = "SKYCOND" Then
                        lNSky += 1
                        Logger.Dbg("SkyCond2Tenths: Found timeseries " & lts.ToString)
                        ltsCloud = SkyCondOktas2CloudTenths(lts)
                        ltsCloud.Attributes.SetValue("ID", 100)
                        ltsCloud.Attributes.SetValue("Constituent", "CLOU")
                        If lISHWDMFile.AddDataset(ltsCloud) Then
                            Logger.Dbg("SkyCond2Tenths: Wrote new timeseries " & ltsCloud.ToString & " to WDM file")
                        Else
                            Logger.Dbg("SkyCond2Tenths: PROBLEM writing new timeseries " & ltsCloud.ToString & " to WDM file")
                        End If
                        Exit For
                    End If
                Next
                If lCons <> "SKYCOND" Then
                    Logger.Dbg("SkyCond2Tenths: Station does not contain SKYCOND dataset")
                End If
                lISHWDMFile.DataSets.Clear()
                FileCopy(lCurWDM, lFile)
                Kill(lCurWDM)
                lISHWDMFile = Nothing
            End If
        Next

        Logger.Dbg("SkyCond2Tenths: Found " & lNSta & " ISH Stations, of which " & lNSky & " contained SKYCOND/CLOU")
        Logger.Dbg("SkyCond2Tenths: Completed Converting SKYCOND to Cloud Cover")

        'Application.Exit()

    End Sub

    Private Function SkyCondOktas2CloudTenths(ByRef aHrlySky As atcTimeseries) As atcTimeseries
        'convert ISH hourly Sky Condition timeseries (recorded in Oktas)
        'to Cloud Cover timeseries in tenths

        Dim lts As atcTimeseries = aHrlySky.Clone
        Dim lVal As Double = 0

        For i As Integer = 1 To lts.numValues
            lVal = lts.Value(i)
            If lVal >= 0 AndAlso lVal <= 10 Then
                Select Case lVal
                    Case 0, 1 'same as recorded okta value
                        lts.Value(i) = lVal
                    Case 2 '2/10 - 3/10
                        lts.Value(i) = 2.5
                    Case 3, 4, 5 '1/10 higher than recorded okta value
                        lts.Value(i) = lVal + 1
                    Case 6 '7/10 - 8/10
                        lts.Value(i) = 7.5
                    Case 7, 8 '2/10 higher than recorded okta value
                        lts.Value(i) = lVal + 2
                    Case 9, 10 'assume fully covered
                        lts.Value(i) = 10
                    Case Else
                        lts.Value(i) = lVal
                End Select
            End If
        Next
        Return lts
    End Function

End Module
