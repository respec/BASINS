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

Public Module GenATMP07
    Private Const pInputPath As String = "C:\BASINSMet\WDMFilled\" '"C:\BASINSMet\WDMFiltered\"
    Private Const pOutputPath As String = "C:\BASINSMet\WDMFilled2007\" '"C:\BASINSMet\WDMFinal\"
    Private Const pStationPath As String = "C:\BASINSMet\Stations\"

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        Logger.StartToFile(pOutputPath & "GenATMP07.log")
        Logger.Dbg("GenATMP07:Start")
        ChDriveDir(pOutputPath)
        Logger.Dbg(" CurDir:" & CurDir())

        Dim lNewDate() As Integer = {2007, 1, 1, 0, 0, 0}
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
            Logger.Dbg("GenATMP07: Opened Station Location file " & lFName)
        End If

        Dim lFiles As New NameValueCollection
        AddFilesInDir(lFiles, pOutputPath, True, "*.wdm")
        Logger.Dbg("GenATMP07: Found " & lFiles.Count & " WDM data files")
        For Each lFile As String In lFiles
            Logger.Dbg("GenATMP07: Opening data file - " & lFile)
            lNewWDMFile = New atcWDM.atcDataSourceWDM
            lNewWDMFile.Open(lFile)
            lts = Nothing
            'assuming all previously computed ATEM data is in DSN 107
            lts = lNewWDMFile.DataSets.ItemByKey(107)
            If Not lts Is Nothing Then 'AndAlso lts.Attributes.GetValue("EJDay") <= lNewJDate Then
                'has existing computed temperature record
                lNewJDate = lts.Attributes.GetValue("EJDay")
                lStation = FilenameNoPath(lFile).Substring(0, 6)
                lStatePath = Right(PathNameOnly(lFile), 2) 'FilenameNoPath(lStation).Substring(0, 2)
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
                        If Not ltsObsSub Is Nothing Then
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
                                Logger.Dbg("GenATMP07:  Generating hourly Air Temp for subset period " & DumpDate(lSJDay) & " to " & DumpDate(lEJDay))
                                lts = DisaggTemp(lTMinSub, lTMaxSub, Nothing, lOTSub)
                            Else 'common dates for tmin/tmax, no need to subset
                                Logger.Dbg("GenATMP07:  Generating hourly Air Temp for period " & ltsTMin.Attributes.GetFormattedValue("SJDay") & " to " & ltsTMin.Attributes.GetFormattedValue("EJDay"))
                                lts = DisaggTemp(ltsTMin, ltsTMax, Nothing, ltsObsSub)
                            End If
                            lts.Attributes.SetValue("ID", 107)
                            If lNewWDMFile.AddDataset(lts, atcDataSource.EnumExistAction.ExistAppend) Then
                                Logger.Dbg("GenATMP07: Appended Hourly ATEM to DSN 107")
                                lGenCnt += 1
                            Else
                                Logger.Dbg("GenATMP07: PROBLEM - Appending Hourly ATEM to DSN 107")
                            End If
                            lts = Nothing
                        Else
                            Logger.Dbg("GenATMP07: No updated OBS TIME data available")
                        End If
                    Else
                        Logger.Dbg("GenATMP07:  No current TMin/TMax for updating ATMP at this station.")
                    End If
                    lNewWDMFile.DataSets.Clear()
                    lNewWDMFile = Nothing
                Else
                    Logger.Dbg("GenATMP07: PROBLEM - station not found in Station Location DBF file")
                End If
            Else
                Logger.Dbg("GenATMP07:  No ATMP needing update at this station")
            End If
        Next

        Logger.Dbg("GenATMP07: Completed ATEM generation - " & lGenCnt & " ATEM datasets created")

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
