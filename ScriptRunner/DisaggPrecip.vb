Imports Microsoft.VisualBasic
Imports System.Collections
Imports System.Collections.Specialized
Imports System.IO
Imports System.Windows.Forms
Imports System.Math
'Imports MapWinGIS
Imports MapWindow.Interfaces
Imports MapWinUtility
'Imports BASINS

Imports atcUtility
Imports atcData
Imports atcWDM
Imports atcMetCmp
'Imports atcDataTree
'Imports atcEvents

Public Module DisaggPrecip
    Private Const pInputPath As String = "C:\BASINSMet\WDMFilled\"
    Private Const pSubsetPath As String = "C:\BASINSMet\WDMFilled\subset\"
    Private Const pObsTimePath As String = "C:\BasinsMet\WDMRaw\" '"C:\BASINSMet\WDMFiltered\"
    Private Const pStationPath As String = "C:\BasinsMet\Stations\"
    Private Const pOutputPath As String = "C:\BASINSMet\DisaggPrec\subset\"
    Private Const pMaxNearStas As Integer = 30
    Private Const pTolerance As Integer = 90
    Private Const pFormat As String = "#,##0.00"
    Private Const pAlreadyDone As String = "" '"01,02,03,04,05,06,07,08,09,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40,41,42,43,44,45,46" ',47,48,50,51,66,67,ak,al,ar,az,ca,co,ct,de,fl,ga,hi,ia,id,il,in,ks,ky,la,ma,md,me,mi,mn,mo,ms,mt,nc,nd,ne,nh,nj,nm,nv,ny"
    Declare Sub F90_MSG Lib "hass_ent.dll" _
        (ByVal aMsg As String, ByVal aMsgLen As Short)

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        Logger.Dbg("DisaggPrec:Start")
        ChDriveDir(pOutputPath)
        Logger.Dbg(" CurDir:" & CurDir())

        Dim lStationDBF As New atcTableDBF
        Dim lStation As String = ""
        Dim lStaFill As String = ""
        Dim lStatePath As String
        Dim lCons As String = ""
        Dim lFName As String = ""
        Dim lCurWDM As String = pOutputPath & "current.wdm"
        Dim lNewWDM As String = pOutputPath & "new.wdm"
        Dim lAddMe As Boolean = True
        Dim i As Integer
        Dim j As Integer
        Dim lFillTsers As atcCollection
        Dim lFillTS As atcTimeseries = Nothing
        Dim lFileName As String = ""
        Dim lFilledTS As atcTimeseries = Nothing
        Dim lFillers As atcDataGroup = Nothing
        Dim lWDMCnt As Integer = 0

        If lStationDBF.OpenFile(pStationPath & "StationLocs-Dist.dbf") Then
            Logger.Dbg("DisaggPrec: Opened Station Location Master file " & pStationPath & "StationLocs-Dist.dbf")
        End If

        Dim X1 As Double
        Dim Y1 As Double
        Dim X2(lStationDBF.NumRecords) As Double
        Dim Y2(lStationDBF.NumRecords) As Double
        Dim lDist(lStationDBF.NumRecords) As Double
        Dim lPos(lStationDBF.NumRecords) As Integer
        Dim lRank(lStationDBF.NumRecords) As Integer

        Logger.Dbg("DisaggPrec: Read all lat/lng values")
        For lrec As Integer = 1 To lStationDBF.NumRecords
            lStationDBF.CurrentRecord = lrec
            X2(lStationDBF.CurrentRecord) = lStationDBF.Value(7)
            Y2(lStationDBF.CurrentRecord) = lStationDBF.Value(8)
        Next

        Dim lFiles As NameValueCollection = Nothing
        AddFilesInDir(lFiles, pSubsetPath, True, "*.wdm")
        Logger.Dbg("DisaggPrec: Found " & lFiles.Count & " data files")
        For Each lfile As String In lFiles
            lWDMCnt += 1
            lStation = FilenameNoExt(FilenameNoPath(lfile))
            lStatePath = Right(PathNameOnly(lfile), 2) & "\"
            If Not pAlreadyDone.Contains(lStatePath.Substring(0, 2)) Then
                Logger.StartToFile(pOutputPath & lStatePath & lStation & "_Disagg.log", , , True)
                If lStationDBF.FindFirst(1, lStation) Then
                    X1 = 0
                    Y1 = 0
                    FileCopy(lfile, lCurWDM)
                    Dim lWDMfile As New atcWDM.atcDataSourceWDM
                    lWDMfile.Open(lCurWDM)
                    Dim lNewWDMfile As New atcWDM.atcDataSourceWDM
                    lNewWDMfile.Open(lNewWDM)
                    For Each lts As atcTimeseries In lWDMfile.DataSets
                        lAddMe = False
                        lCons = lts.Attributes.GetValue("Constituent")
                        'look for precip datasets to disaggregate
                        If lCons = "PRCP" AndAlso lts.Attributes.GetValue("tu") = atcTimeUnit.TUDay Then
                            lAddMe = True
                        End If
                        If lAddMe Then
                            If X1 < Double.Epsilon AndAlso Y1 < Double.Epsilon Then 'determine nearest geographic stations
                                X1 = lStationDBF.Value(7)
                                Y1 = lStationDBF.Value(8)
                                Logger.Dbg("DisaggPrec: For Station " & lStation & ", " & lStationDBF.Value(2) & "  at Lat/Lng " & lStationDBF.Value(4) & " / " & lStationDBF.Value(5))
                                For i = 1 To lStationDBF.NumRecords
                                    lDist(i) = System.Math.Sqrt((X1 - X2(i)) ^ 2 + (Y1 - Y2(i)) ^ 2)
                                Next
                                SortRealArray(0, lStationDBF.NumRecords, lDist, lPos)
                                'SortIntegerArray(0, lStationDBF.NumRecords, lPos, lRank)
                                Logger.Dbg("DisaggPrec: Sorted stations by distance")
                            End If
                            Logger.Dbg("DisaggPrec:    Nearby Stations:")
                            lFillers = New atcTimeseriesGroup
                            i = 2
                            j = 0
                            'see if HPCP exists on same WDM
                            For Each llts As atcTimeseries In lWDMfile.DataSets
                                If llts.Attributes.GetValue("Constituent") = "HPCP" Then
                                    lFillers.Add(0, llts)
                                    Logger.Dbg("DisaggPrec:  Using " & _
                                               llts.Attributes.GetValue("Constituent") & " from " & _
                                               llts.Attributes.GetValue("Location") & " " & _
                                               llts.Attributes.GetValue("STANAM") & " at Lat/Lng " & _
                                               llts.Attributes.GetValue("LATDEG") & "/" & llts.Attributes.GetValue("LNGDEG"))
                                    j += 1
                                End If
                            Next
                            While j < pMaxNearStas AndAlso i < lStationDBF.NumRecords
                                'look through stations, in order of proximity, that can be used to fill
                                lStationDBF.CurrentRecord = lPos(i)
                                lStaFill = lStationDBF.Value(1)
                                If Left(lStaFill, 2) < 68 Then 'OrElse Left(lStaFill, 2) = "91" Then 'Coop station
                                    lFileName = pInputPath & Left(lStaFill, 2) & "\" & lStaFill & ".wdm"
                                Else 'ISH Station
                                    lFileName = pInputPath & lStationDBF.Value(3) & "\" & lStaFill & ".wdm"
                                End If
                                If FileExists(lFileName) Then
                                    lFillTsers = FindHrlyPrecips(lts, lFileName)
                                    For k As Integer = 0 To lFillTsers.Count - 1
                                        lFillTS = lFillTsers(k)
                                        If Not lFillTS Is Nothing Then
                                            'contains data for time period being filled
                                            lFillers.Add(lFillTS)
                                            j += 1
                                            Logger.Dbg("DisaggPrec:  Using " & _
                                                       lFillTS.Attributes.GetValue("Constituent") & " from " & _
                                                       lFillTS.Attributes.GetValue("Location") & " " & _
                                                       lFillTS.Attributes.GetValue("STANAM") & " at Lat/Lng " & _
                                                       lStationDBF.Value(4) & "/" & lStationDBF.Value(5))
                                        End If
                                    Next
                                End If
                                i += 1
                            End While
                            If j > 0 Then
                                Logger.Dbg("DisaggPrec:  Found " & j & " nearby stations for disaggregation")
                                Dim lObsTimeTS As atcTimeseries = FindObsTimeTS(pObsTimePath & lStatePath & lStation & ".wdm", lts)
                                If Not lObsTimeTS Is Nothing Then
                                    Dim lSummFile As String = pOutputPath & lStatePath & lStation & "_Disagg.sum"
                                    Dim lDisaggTS As atcTimeseries = atcMetCmp.DisaggPrecip(lts, Nothing, lFillers, lObsTimeTS, pTolerance, lSummFile)
                                    If Not lDisaggTS Is Nothing Then 'disagg completed
                                        Logger.Dbg("DisaggPrec:  Completed disaggregation of " & lCons & " - details follow")
                                        Dim lSummStr As String = vbCrLf & vbCrLf & WholeFileString(lSummFile)
                                        Logger.Dbg(lSummStr)
                                        Kill(lSummFile)
                                        'write disaggregated data set to new WDM file
                                        If lNewWDMfile.AddDataset(lDisaggTS) Then
                                            Logger.Dbg("FillMissing:  Added " & lCons & " dataset to WDM file for station " & lStation)
                                        Else
                                            Logger.Dbg("FillMissing:  PROBLEM adding " & lCons & " dataset to WDM file for station " & lStation)
                                        End If
                                    Else
                                        Logger.Dbg("DisaggPrec: PROBLEM with disaggregation step")
                                    End If
                                Else
                                    Logger.Dbg("DisaggPrec: PROBLEM - Could not find corresponding Obs Time TSer")
                                End If
                            Else
                                Logger.Dbg("DisaggPrec:  PROBLEM - Could not find any nearby stations for filling")
                            End If
                        End If
                    Next
                    If lNewWDMfile.DataSets.Count > 0 Then 'save new WDM file
                        MkDirPath(pOutputPath & lStatePath)
                        lFName = pOutputPath & lStatePath & lStation & "_Disagg.wdm"
                        FileCopy(lNewWDM, lFName)
                        Logger.Dbg("DisaggPrec:  Wrote updated WDM file to " & lFName)
                        lNewWDMfile.DataSets.Clear()
                    Else
                        Logger.Dbg("DisaggPrec:  No daily precip to disaggregate for station " & lStation)
                    End If
                    lNewWDMfile = Nothing
                    Kill(lNewWDM)
                    lWDMfile.DataSets.Clear()
                    lWDMfile = Nothing
                    Kill(lCurWDM)
                Else
                    Logger.Dbg("DisaggPrec:  PROBLEM - could not find station on location DBF file")
                End If
                'lPercent = "(" & DoubleToString((100 * lWDMCnt) / lFiles.Count, , pFormat) & "%)"
                'Logger.Dbg("Done " & lfile & ",  MemUsage " & MemUsage())
            End If
        Next
        Logger.Dbg("DisaggPrec: Completed Disaggregation")

        'Application.Exit()

    End Sub

    Private Function FindHrlyPrecips(ByVal aCurTS As atcTimeseries, ByVal aWDMFile As String) As atcCollection
        Dim lSJD As Double = aCurTS.Attributes.GetValue("SJDay")
        Dim lEJD As Double = aCurTS.Attributes.GetValue("EJDay")
        Dim lCons As String = ""
        Dim lChkDates As Boolean = False
        Dim lTSers As New atcCollection

        FileCopy(aWDMFile, "Temp.wdm")
        Dim lWDMfile As New atcWDM.atcDataSourceWDM
        lWDMfile.Open("Temp.wdm")
        For Each lts As atcTimeseries In lWDMfile.DataSets
            lChkDates = False
            lCons = lts.Attributes.GetValue("Constituent")
            If lCons = "HPCP" OrElse lCons = "HPCP1" Then
                'got right constituent, check quality (UBC200) and period of record
                If lts.Attributes.GetValue("UBC200") < 50 AndAlso _
                   lts.numValues > 44000 AndAlso _
                   lts.Attributes.GetValue("SJDay") < lEJD AndAlso _
                   lts.Attributes.GetValue("EJDay") > lSJD Then 'some portion falls within filling period
                    lTSers.Add(lts)
                End If
            End If
        Next
        lWDMfile.DataSets.Clear()
        lWDMfile = Nothing
        Kill("Temp.wdm")
        Return lTSers
    End Function

    Private Function FindObsTimeTS(ByVal aWDMFileName As String, ByVal aTSer As atcTimeseries) As atcTimeseries
        Dim lCons As String = aTSer.Attributes.GetValue("Constituent")
        Dim lTSer As New atcTimeseries(Nothing)
        Dim lSJDay As Double
        Dim lEJDay As Double
        Dim lWDMFile As New atcWDM.atcDataSourceWDM
        lWDMFile.Open(aWDMFileName)

        If aTSer.Attributes.GetValue("TU") = atcTimeUnit.TUDay Then
            Dim lStaCons As String
            For Each lts As atcTimeseries In lWDMFile.DataSets
                lStaCons = lts.Attributes.GetValue("Constituent")
                If lStaCons = lCons & "-OBS" Then
                    lSJDay = aTSer.Attributes.GetValue("SJDay")
                    lEJDay = aTSer.Attributes.GetValue("EJDay")
                    lTSer = SubsetByDate(lts, lSJDay, lEJDay, Nothing)
                    lTSer.EnsureValuesRead()
                    'Logger.Dbg("FillMissing:FindObsTimeTS: Found Obs Time TSer " & lTSer.ToString & " containing " & lTSer.numValues & " values.")
                    Exit For
                End If
            Next
        End If
        Return lTSer
    End Function

    Private Sub ExtendISHTSer(ByRef aTSer As atcTimeseries)
        Dim lEJDay As Double = aTSer.Attributes.GetValue("EJDay")
        If lEJDay > 0 Then
            Dim lNewEJDay As Double = System.Math.Round(lEJDay)
            If lNewEJDay > lEJDay Then
                Dim lNumOldVals As Integer = aTSer.numValues
                Dim lNumNewVals As Integer = System.Math.Round((lNewEJDay - lEJDay) * 24)
                aTSer.numValues = lNumOldVals + lNumNewVals
                For i As Integer = 1 To lNumNewVals
                    aTSer.Dates.Values(lNumOldVals + i) = aTSer.Dates.Values(lNumOldVals) + i / 24
                    aTSer.Values(lNumOldVals + i) = Double.NaN 'aTSer.Attributes.GetValue("TSFILL")
                Next
                aTSer.Attributes.DiscardCalculated()
            End If
        End If
    End Sub

    'Private Function MemUsage() As String
    '    System.GC.WaitForPendingFinalizers()
    '    Return "MemoryUsage(MB):" & DoubleToString(Process.GetCurrentProcess.PrivateMemorySize64 / (2 ^ 20), , pFormat) & _
    '                " Local(MB):" & DoubleToString(System.GC.GetTotalMemory(True) / (2 ^ 20), , pFormat)
    'End Function
End Module
