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
Imports atcMetCmp
'Imports atcDataTree
'Imports atcEvents

Public Module GenATMP
    Private Const pInputPath As String = "C:\BASINSMet\WDMFiltered\"
    Private Const pOutputPath As String = "C:\BASINSMet\WDMFilled\"
    Private Const pStationPath As String = "C:\BASINSMet\Stations\"

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        Logger.Dbg("GenATMP:Start")
        ChDriveDir(pOutputPath)
        Logger.Dbg(" CurDir:" & CurDir())

        Dim lStationDBF As New atcTableDBF
        Dim lNewWDMFile As atcWDM.atcDataSourceWDM = Nothing
        Dim lStation As String = ""
        Dim lStatePath As String = ""
        Dim lts As New atcTimeseries(Nothing)
        Dim ltsTMin As atcTimeseries
        Dim ltsTMax As atcTimeseries
        Dim ltsObs As atcTimeseries
        Dim lCurWDM As String = pOutputPath & "current.wdm"
        Dim lGenCnt As Integer = 0

        Dim lFName As String = pStationPath & "StationLocs.dbf"
        If lStationDBF.OpenFile(lFName) Then
            Logger.Dbg("GenATMP: Opened Station Location file " & lFName)
        End If

        Dim lFiles As New NameValueCollection
        AddFilesInDir(lFiles, pOutputPath, True, "*.wdm")
        Logger.Dbg("GenATMP: Found " & lFiles.Count & " WDM data files")
        For Each lFile As String In lFiles
            Logger.Dbg("GenATMP: Opening data file - " & lFile)
            lStatePath = Right(PathNameOnly(lFile), 2)
            If IsNumeric(lStatePath) Then 'only process SOD data under numeric state directories
                lStation = FilenameOnly(lFile)
                If lStationDBF.FindFirst(1, lStation) Then 'found station ID
                    FileCopy(lFile, lCurWDM)
                    lNewWDMFile = New atcWDM.atcDataSourceWDM
                    lNewWDMFile.Open(lCurWDM)
                    Dim lLat As Double = lStationDBF.Value(4)
                    ltsTMin = Nothing
                    ltsTMax = Nothing
                    For Each lDS As atcDataSet In lNewWDMFile.DataSets
                        If lDS.Attributes.GetValue("Constituent") = "TMIN" Then
                            ltsTMin = lDS.Clone
                        ElseIf lDS.Attributes.GetValue("Constituent") = "TMAX" Then
                            ltsTMax = lDS.Clone
                        End If
                    Next
                    If Not ltsTMin Is Nothing AndAlso Not ltsTMax Is Nothing Then
                        Dim lOTWDMFileName As String = pInputPath & lStatePath & "\" & lStation & ".wdm"
                        ltsObs = FindObsTimeTS(lOTWDMFileName, ltsTMin)
                        If ltsTMin.Attributes.GetValue("SJDay") <> ltsTMax.Attributes.GetValue("SJDay") OrElse _
                           ltsTMin.Attributes.GetValue("EJDay") <> ltsTMax.Attributes.GetValue("EJDay") Then
                            'tmin/tmax start/end not the same, need to use common subset
                            Dim lSJDay As Double
                            Dim lEJDay As Double
                            If ltsTMin.Attributes.GetValue("SJDay") < ltsTMax.Attributes.GetValue("SJDay") Then
                                'tmax starts after tmin, use tmax start date
                                lSJDay = ltsTMax.Attributes.GetValue("SJDay")
                            Else 'use tmin start date
                                lSJDay = ltsTMin.Attributes.GetValue("SJDay")
                            End If
                            If ltsTMin.Attributes.GetValue("EJDay") < ltsTMax.Attributes.GetValue("EJDay") Then
                                'tmin ends before tmax, use tmin end date
                                lEJDay = ltsTMin.Attributes.GetValue("EJDay")
                            Else 'use tmax end date
                                lEJDay = ltsTMax.Attributes.GetValue("EJDay")
                            End If
                            Dim lTMinSub As atcTimeseries = SubsetByDate(ltsTMin, lSJDay, lEJDay, Nothing)
                            Dim lTMaxSub As atcTimeseries = SubsetByDate(ltsTMax, lSJDay, lEJDay, Nothing)
                            Dim lOTSub As atcTimeseries = SubsetByDate(ltsObs, lSJDay, lEJDay, Nothing)
                            Logger.Dbg("GenATMP:  Generating hourly Air Temp for subset period " & DumpDate(lSJDay) & " to " & DumpDate(lEJDay))
                            lts = DisaggTemp(lTMinSub, lTMaxSub, Nothing, lOTSub)
                        Else 'common dates for tmin/tmax, no need to subset
                            Logger.Dbg("GenATMP:  Generating hourly Air Temp for period " & DumpDate(ltsTMin.Attributes.GetValue("SJDay")).Substring(14) & " to " & DumpDate(ltsTMin.Attributes.GetValue("EJDay")).Substring(14))
                            lts = DisaggTemp(ltsTMin, ltsTMax, Nothing, ltsObs)
                        End If
                        lts.Attributes.SetValue("ID", 107)
                        lts.Attributes.SetValue("Location", lStation)
                        If lNewWDMFile.AddDataset(lts) Then
                            Logger.Dbg("GenATMP: Wrote Hourly ATEM to DSN 107")
                            lGenCnt += 1
                            'delete existing file and copy current working WDM to it
                            Kill(lFile)
                            FileCopy(lCurWDM, lFile)
                        Else
                            Logger.Dbg("GenATMP: PROBLEM - Writing Hourly ATEM to DSN 107")
                        End If
                        lts = Nothing
                    Else
                        Logger.Dbg("GenATMP:  No TMin/TMax for this station.")
                    End If
                    Kill(lCurWDM)
                    lNewWDMFile.DataSets.Clear()
                    lNewWDMFile = Nothing
                Else
                    Logger.Dbg("GenATMP: PROBLEM - station not found in Station Location DBF file")
                End If
            End If
        Next

        Logger.Dbg("GenATMP: Completed ATEM generation - " & lGenCnt & " ATEM datasets created")

        'Application.Exit()

    End Sub

    Private Function FindObsTimeTS(ByVal aWDMFileName As String, ByVal aTSer As atcTimeseries) As atcTimeseries
        Dim lCons As String = aTSer.Attributes.GetValue("Constituent")
        Dim lTSer As New atcTimeseries(Nothing)
        Dim lWDMFile As New atcWDM.atcDataSourceWDM
        lWDMFile.Open(aWDMFileName)

        If aTSer.Attributes.GetValue("TU") = atcTimeUnit.TUDay Then
            Dim lStaCons As String
            For Each lts As atcTimeseries In lWDMFile.DataSets
                lStaCons = lts.Attributes.GetValue("Constituent")
                If lStaCons = lCons & "-OBS" Then
                    lTSer = lts
                    lTSer.EnsureValuesRead()
                    'Logger.Dbg("FillMissing:FindObsTimeTS: Found Obs Time TSer " & lTSer.ToString & " containing " & lTSer.numValues & " values.")
                    Exit For
                End If
            Next
        End If
        Return lTSer
    End Function

End Module
