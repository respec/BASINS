Imports Microsoft.VisualBasic
Imports System.Collections
Imports System.Collections.Specialized
Imports System.IO
Imports System.Windows.Forms
'Imports MapWinGIS
Imports MapWindow.Interfaces
Imports MapWinUtility
Imports MapWinUtility.Strings
'Imports BASINS

Imports atcUtility
Imports atcData
Imports atcMetCmp
'Imports atcDataTree
'Imports atcEvents

Public Module GenATMP
    Private Const pInputPath As String = "C:\BASINSMet\WDMFiltered\"
    Private Const pOutputPath As String = "C:\BASINSMet\WDMFinal\"
    Private Const pStationPath As String = "C:\BASINSMet\Stations\"

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        Logger.Dbg("GenATMP06:Start")
        ChDriveDir(pOutputPath)
        Logger.Dbg(" CurDir:" & CurDir())

        Dim lNewDate() As Integer = {2006, 1, 1, 0, 0, 0}
        Dim lNewJDate As Double = Date2J(lNewDate)
        Dim lStationDBF As New atcTableDBF
        Dim lNewWDMFile As atcWDM.atcDataSourceWDM = Nothing
        Dim lStation As String = ""
        Dim lStatePath As String = ""
        Dim lts As New atcTimeseries(Nothing)
        Dim ltsTMin As atcTimeseries
        Dim ltsTMax As atcTimeseries
        Dim ltsObs As atcTimeseries
        Dim ltsObsSub As atcTimeseries
        Dim lGenCnt As Integer = 0

        Dim lFName As String = pStationPath & "StationLocs.dbf"
        If lStationDBF.OpenFile(lFName) Then
            Logger.Dbg("GenATMP06: Opened Station Location file " & lFName)
        End If

        Dim lFiles As New NameValueCollection
        AddFilesInDir(lFiles, pOutputPath, True, "*.wdm")
        Logger.Dbg("GenATMP06: Found " & lFiles.Count & " WDM data files")
        For Each lFile As String In lFiles
            Logger.Dbg("GenATMP06: Opening data file - " & lFile)
            lNewWDMFile = New atcWDM.atcDataSourceWDM
            lNewWDMFile.Open(lFile)
            lts = Nothing
            lts = lNewWDMFile.DataSets.ItemByKey(3)
            If Not lts Is Nothing AndAlso lts.Attributes.GetValue("EJDay") <= lNewJDate Then
                'has temperature data that is not through 2006
                lStation = FilenameOnly(lFile).Substring(2, 6)
                lStatePath = FilenameOnly(lStation).Substring(0, 2)
                If lStationDBF.FindFirst(1, lStation) Then 'found station ID
                    Dim lLat As Double = lStationDBF.Value(4)
                    ltsTMin = Nothing
                    ltsTMax = Nothing
                    For Each lDS As atcDataSet In lNewWDMFile.DataSets
                        If lDS.Attributes.GetValue("Constituent") = "TMIN" AndAlso _
                           lDS.Attributes.GetValue("EJDay") > lNewJDate Then
                            ltsTMin = SubsetByDate(lDS, lNewJDate, lDS.Attributes.GetValue("EJDay"), Nothing)
                        ElseIf lDS.Attributes.GetValue("Constituent") = "TMAX" AndAlso _
                               lDS.Attributes.GetValue("EJDay") > lNewJDate Then
                            ltsTMax = SubsetByDate(lDS, lNewJDate, lDS.Attributes.GetValue("EJDay"), Nothing)
                        End If
                    Next
                    If Not ltsTMin Is Nothing AndAlso Not ltsTMax Is Nothing Then
                        Dim lOTWDMFileName As String = pInputPath & lStatePath & "\" & lStation & ".wdm"
                        ltsObs = FindObsTimeTS(lOTWDMFileName, ltsTMin)
                        ltsObsSub = SubsetByDate(ltsObs, lNewJDate, ltsObs.Attributes.GetValue("EJDay"), Nothing)
                        lts = Nothing
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
                            Logger.Dbg("GenATMP06:  Generating hourly Air Temp for subset period " & DumpDate(lSJDay) & " to " & DumpDate(lEJDay))
                            lts = DisaggTemp(lTMinSub, lTMaxSub, Nothing, lOTSub)
                        Else 'common dates for tmin/tmax, no need to subset
                            Logger.Dbg("GenATMP06:  Generating hourly Air Temp for period " & DumpDate(ltsTMin.Attributes.GetValue("SJDay")).Substring(14) & " to " & DumpDate(ltsTMin.Attributes.GetValue("EJDay")).Substring(14))
                            lts = DisaggTemp(ltsTMin, ltsTMax, Nothing, ltsObsSub)
                        End If
                        lts.Attributes.SetValue("ID", 3)
                        If lNewWDMFile.AddDataset(lts, atcDataSource.EnumExistAction.ExistAppend) Then
                            Logger.Dbg("GenATMP06: Appended Hourly ATEM to DSN 3")
                            lGenCnt += 1
                        Else
                            Logger.Dbg("GenATMP06: PROBLEM - Appending Hourly ATEM to DSN 3")
                        End If
                        lts = Nothing
                    Else
                        Logger.Dbg("GenATMP06:  No current TMin/TMax for updating ATMP at this station.")
                    End If
                    lNewWDMFile.DataSets.Clear()
                    lNewWDMFile = Nothing
                Else
                    Logger.Dbg("GenATMP06: PROBLEM - station not found in Station Location DBF file")
                End If
            Else
                Logger.Dbg("GenATMP06:  No ATMP needing update at this station")
            End If
        Next

        Logger.Dbg("GenATMP06: Completed ATEM generation - " & lGenCnt & " ATEM datasets created")

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
