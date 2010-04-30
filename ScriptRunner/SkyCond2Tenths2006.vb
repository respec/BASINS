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

Public Module SkyCond2Tenths2006
    Private Const pOutputPath As String = "C:\BASINSMet\WDMFiltered\"
    Private Const pStationPath As String = "C:\BASINSMet\Stations\"

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        Logger.Dbg("SkyCond2Tenths2006:Start")
        ChDriveDir(pOutputPath)
        Logger.Dbg(" CurDir:" & CurDir())

        Dim lISHWDMFile As atcWDM.atcDataSourceWDM = Nothing
        Dim lState As String = ""
        Dim lCons As String = ""
        Dim lts As atcTimeseries
        Dim lCurWDM As String = pOutputPath & "current.wdm"
        Dim ltsCloud As atcTimeseries
        Dim lNUpdated As Integer = 0
        Dim lNSky As Integer = 0

        Dim lFiles As New NameValueCollection
        AddFilesInDir(lFiles, pOutputPath, True, "*.wdm")
        Logger.Dbg("SkyCond2Tenths2006: Found " & lFiles.Count & " WDM data files")
        For Each lFile As String In lFiles
            lState = Right(PathNameOnly(lFile), 2)
            If Not IsNumeric(lState) Then
                Logger.Dbg("SkyCond2Tenths2006: Opening data file - " & lFile)
                FileCopy(lFile, lCurWDM)
                lISHWDMFile = New atcWDM.atcDataSourceWDM
                lISHWDMFile.Open(lCurWDM)
                ltsCloud = Nothing
                For Each lts In lISHWDMFile.DataSets
                    If lts.Attributes.GetValue("Constituent") = "CLOU" Then
                        ltsCloud = lts
                        Exit For
                    End If
                Next
                If Not ltsCloud Is Nothing Then 'found existing cloud dataset
                    For Each lts In lISHWDMFile.DataSets
                        lCons = lts.Attributes.GetValue("Constituent")
                        If lCons = "SKYCOND" Then
                            lNSky += 1
                            Logger.Dbg("SkyCond2Tenths2006: Found timeseries " & lts.ToString)
                            If lts.Attributes.GetValue("EJDay") > ltsCloud.Attributes.GetValue("EJDay") Then
                                Dim ltssub As atcTimeseries = SubsetByDate(lts, ltsCloud.Attributes.GetValue("EJDay"), lts.Attributes.GetValue("EJDay"), Nothing)
                                Dim ltsCloudSub As atcTimeseries = SkyCondOktas2CloudTenths(ltssub)
                                ltsCloudSub.Attributes.SetValue("ID", 100)
                                ltsCloudSub.Attributes.SetValue("Constituent", "CLOU")
                                If lISHWDMFile.AddDataset(ltsCloudSub, atcDataSource.EnumExistAction.ExistAppend) Then
                                    Logger.Dbg("SkyCond2Tenths2006: Updated " & ltsCloud.ToString & " through " & DumpDate(ltsCloudSub.Attributes.GetValue("EJDay")))
                                    lNUpdated += 1
                                Else
                                    Logger.Dbg("SkyCond2Tenths2006: PROBLEM updating " & ltsCloud.ToString & " through " & DumpDate(ltsCloudSub.Attributes.GetValue("EJDay")))
                                End If
                            Else
                                Logger.Dbg("SkyCond2Tenths2006: No newer SKYCOND data to convert")
                            End If
                            Exit For
                        End If
                    Next
                    If lCons <> "SKYCOND" Then
                        Logger.Dbg("SkyCond2Tenths2006: PROBLEM - Station contains CLOU, but not SKYCOND dataset")
                    End If
                End If
                lISHWDMFile.DataSets.Clear()
                FileCopy(lCurWDM, lFile)
                Kill(lCurWDM)
                lISHWDMFile = Nothing
            End If
        Next

        Logger.Dbg("SkyCond2Tenths2006: Updated " & lNUpdated & " CLOU datasets of the existing " & lNSky)
        Logger.Dbg("SkyCond2Tenths2006: Completed Updating conversion of SKYCOND to Cloud Cover")

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
