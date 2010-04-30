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
'Imports atcDataTree
'Imports atcEvents

Public Module FillPrecip
    'Private Const pSubsetPath As String = "C:\BASINSMet\WDMFiltered\subset\"
    'Private Const pInputPath As String = "C:\BASINSMet\WDMFiltered\"
    Private Const pStationPath As String = "C:\BASINS\Data\Met_Data\CA-Ventura\"
    Private Const pOutputPath As String = "C:\BASINS\Data\Met_Data\CA-Ventura\"
    Private Const pMaxNearStas As Integer = 30
    Private Const pMaxFillLength As Integer = 11 'any span < max time shift (10 hrs for HI)
    Private Const pDSNMinToDo As Integer = 353
    Private Const pDSNMaxToDo As Integer = 358
    Private pFillDsns() As Integer = {351, 352, 353, 354, 355, 356, 357, 358}
    Declare Sub F90_MSG Lib "hass_ent.dll" _
        (ByVal aMsg As String, ByVal aMsgLen As Short)

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        Logger.Dbg("FillMissing:Start")
        ChDriveDir(pOutputPath)
        Logger.Dbg(" CurDir:" & CurDir())

        Dim lStationDBF As New atcTableDBF
        Dim lStation As String = ""
        Dim lStaFill As String = ""
        Dim lCons As String = ""
        Dim lConsFill As String = ""
        Dim lFName As String = ""
        Dim lPctMiss As Double
        Dim lCurWDM As String = pOutputPath & "Precip_12-8-06.wdm"
        Dim lNewWDM As String = pOutputPath & "new.wdm"
        Dim lAddMe As Boolean = True
        Dim lInterpolate As Boolean
        Dim i As Integer
        Dim j As Integer
        Dim lDSN As Integer
        Dim lTU As Integer
        Dim lTUFill As Integer
        Dim lFillTS As atcTimeseries = Nothing
        Dim lTStep As Integer
        Dim lTStepFill As Integer
        Dim lFileName As String = ""
        Dim lFilledTS As atcTimeseries = Nothing
        Dim lFillers As atcCollection = Nothing
        Dim lFillerOTs As atcCollection = Nothing
        Dim lTSObs As atcTimeseries = Nothing

        Dim lStr As String = ""
        Dim lMVal As Double = -999.0
        Dim lMAcc As Double = -99.0
        Dim lFMin As Double = -100.0
        Dim lFMax As Double = 10000.0
        Dim lRepType As Integer = 1 'DBF parsing output format

        If lStationDBF.OpenFile(pStationPath & "CAStationLocs.dbf") Then
            Logger.Dbg("FillMissing: Opened Station Location Master file " & pStationPath & "CAStationLocs.dbf")
        End If

        Dim X1 As Double
        Dim Y1 As Double
        Dim X2(lStationDBF.NumRecords) As Double
        Dim Y2(lStationDBF.NumRecords) As Double
        Dim lDist(lStationDBF.NumRecords) As Double
        Dim lPos(lStationDBF.NumRecords) As Integer
        Dim lRank(lStationDBF.NumRecords) As Integer

        Logger.Dbg("FillMissing: Read all lat/lng values")
        lStationDBF.CurrentRecord = 1
        While Not lStationDBF.atEOF
            X2(lStationDBF.CurrentRecord) = lStationDBF.Value(7)
            Y2(lStationDBF.CurrentRecord) = lStationDBF.Value(8)
            lStationDBF.CurrentRecord += 1
        End While

        'Dim lFiles As NameValueCollection = Nothing
        'AddFilesInDir(lFiles, pInputPath, True, "*.wdm")
        'Logger.Dbg("FillMissing: Found " & lFiles.Count & " data files")
        Dim lWDMfile As New atcWDM.atcDataSourceWDM
        lWDMfile.Open(lCurWDM)
        FileCopy(lCurWDM, lNewWDM)
        Dim lNewWDMfile As New atcWDM.atcDataSourceWDM
        lNewWDMfile.Open(lNewWDM)

        For Each lts As atcTimeseries In lWDMfile.DataSets
            If lts.Attributes.GetValue("Constituent") = "PREC-OBS" Then
                lTSObs = lts
                Exit For
            End If
        Next

        For Each lts As atcTimeseries In lWDMfile.DataSets
            lDSN = lts.Attributes.GetValue("ID")
            If lDSN >= pDSNMinToDo AndAlso lDSN <= pDSNMaxToDo Then
                lStation = lts.Attributes.GetValue("Location")
                Logger.StartToFile(pOutputPath & lStation & "_Fill" & ".log", , , True)
                If lStationDBF.FindFirst(1, lStation) Then
                    X1 = 0
                    Y1 = 0
                    lAddMe = False
                    lInterpolate = False
                    lCons = lts.Attributes.GetValue("Constituent")
                    lTU = lts.Attributes.GetValue("tu")
                    lTStep = lts.Attributes.GetValue("ts")
                    'lPctMiss = CDbl(lts.Attributes.GetValue("UBC200"))
                    Select Case lCons.ToUpper 'look for wanted constituents and check % missing
                        Case "PREC", "HPCP", "HPCP1", "PRCP" ', "TMIN", "TMAX"
                            'If (lCons.StartsWith("HP") AndAlso lts.numValues > 26000) OrElse _
                            '   lts.numValues > 1000 Then 'want 3+ years minimum
                            Logger.Dbg("FillMissing:  Filling data for " & lts.ToString & ", " & lts.Attributes.GetValue("Description"))
                            lStr = MissingDataSummary(lts, lMVal, lMAcc, lFMin, lFMax, lRepType)
                            lPctMiss = CDbl(lStr.Substring(lStr.LastIndexOf(",") + 1))
                            Logger.Dbg("FillMissing:  % Missing:  " & lPctMiss)
                            lAddMe = True
                            'Else
                            'Logger.Dbg("FillMissing:  Not enough values for " & lts.ToString)
                            'End If
                        Case Else
                            Logger.Dbg("FillMissing:  Not processing constituent for " & lts.ToString)
                    End Select
                    If lAddMe Then
                        'If lPctMiss > 0 Then
                        If X1 < Double.Epsilon AndAlso Y1 < Double.Epsilon Then 'determine nearest geographic stations
                            X1 = lStationDBF.Value(7)
                            Y1 = lStationDBF.Value(8)
                            Logger.Dbg("FillMissing: For Station " & lStation & ", " & lStationDBF.Value(2) & "  at Lat/Lng " & lStationDBF.Value(4) & " / " & lStationDBF.Value(5))
                            For i = 1 To lStationDBF.NumRecords
                                lDist(i) = System.Math.Sqrt((X1 - X2(i)) ^ 2 + (Y1 - Y2(i)) ^ 2)
                            Next
                            SortRealArray(0, lStationDBF.NumRecords, lDist, lPos)
                            'SortIntegerArray(0, lStationDBF.NumRecords, lPos, lRank)
                            Logger.Dbg("FillMissing: Sorted stations by distance")
                        End If
                        Logger.Dbg("FillMissing:    Nearby Stations:")
                        lFillers = New atcCollection
                        lFillerOTs = New atcCollection
                        i = 2
                        j = 0
                        While j < pMaxNearStas AndAlso i < lStationDBF.NumRecords
                            'look through stations, in order of proximity, that can be used to fill
                            lStationDBF.CurrentRecord = lPos(i)
                            lStaFill = lStationDBF.Value(1)
                            For Each llTS As atcTimeseries In lWDMfile.DataSets
                                If llTS.Attributes.GetValue("Location") = lStaFill Then
                                    Dim lId As Integer = llTS.Attributes.GetValue("ID")
                                    Dim lIndex As Integer = System.Array.IndexOf(pFillDsns, lId)
                                    If lIndex >= 0 Then
                                        lConsFill = llTS.Attributes.GetValue("Constituent")
                                        lTUFill = llTS.Attributes.GetValue("tu")
                                        lTStepFill = llTS.Attributes.GetValue("ts")
                                        Select Case lConsFill.ToUpper
                                            Case "PREC", "HPCP", "HPCP1", "PRCP"
                                                If llTS.Attributes.GetValue("SJDay") < lts.Attributes.GetValue("EJDay") AndAlso _
                                                       llTS.Attributes.GetValue("EJDay") > lts.Attributes.GetValue("SJDay") Then
                                                    'some portion falls within filling period
                                                    If lTUFill < lTU OrElse (lTUFill = lTU AndAlso lTStepFill < lTStep) Then
                                                        'shorter time interval, aggregate to current
                                                        Dim lAggTS As atcTimeseries = Nothing
                                                        If lTU = atcTimeUnit.TUDay Then
                                                            'aggregate less than daily data to hourly
                                                            'so that proper hours are used based on Obs Time
                                                            If lTUFill < atcTimeUnit.TUHour Then
                                                                'aggregate 15-minute to hourly
                                                                lAggTS = Aggregate(llTS, atcTimeUnit.TUHour, lTStep, atcTran.TranSumDiv)
                                                            Else 'just leave hourly as is
                                                                lAggTS = llTS
                                                            End If
                                                        Else
                                                            lAggTS = Aggregate(llTS, lTU, lTStep, atcTran.TranSumDiv)
                                                        End If
                                                        If Not lAggTS Is Nothing Then
                                                            lFillers.Add(lDist(lPos(i)), lAggTS)
                                                            Logger.Dbg("FillMissing:  Using Aggregated " & _
                                                                       llTS.Attributes.GetValue("Constituent") & " from " & _
                                                                       llTS.Attributes.GetValue("Location") & " " & _
                                                                       llTS.Attributes.GetValue("STANAM") & " at Lat/Lng " & _
                                                                       llTS.Attributes.GetValue("LATDEG") & "/" & llTS.Attributes.GetValue("LNGDEG"))
                                                            j += 1
                                                        Else
                                                            Logger.Dbg("FillMissing:  Could not Aggregate " & _
                                                                       llTS.Attributes.GetValue("Constituent") & " from " & _
                                                                       llTS.Attributes.GetValue("Location") & " " & _
                                                                       llTS.Attributes.GetValue("STANAM"))
                                                        End If
                                                    ElseIf lTUFill = lTU Then
                                                        lFillers.Add(lDist(lPos(i)), llTS)
                                                        Logger.Dbg("FillMissing:  Using " & _
                                                                   llTS.Attributes.GetValue("Constituent") & " from " & _
                                                                   llTS.Attributes.GetValue("Location") & " " & _
                                                                   llTS.Attributes.GetValue("STANAM") & " at Lat/Lng " & _
                                                                   llTS.Attributes.GetValue("LATDEG") & "/" & llTS.Attributes.GetValue("LNGDEG"))
                                                        j += 1
                                                    End If
                                                End If
                                            Case Else
                                        End Select
                                    Else
                                        Logger.Dbg("FillMissing:  Found station at " & lStaFill & ", but it is not in Fill DSNs array")
                                    End If
                                End If
                            Next
                            i += 1
                        End While
                        If j > 0 Then
                            Logger.Dbg("FillMissing:  Found " & j & " nearby stations for filling")
                            'Logger.Dbg("FillMissing:  Before Filling, % Missing:  " & lPctMiss)
                            If lTU <= atcTimeUnit.TUHour Then
                                FillHourlyTser(lts, lFillers, lMVal, lMAcc, 90)
                            Else 'daily tser, locate obs time tsers
                                For Each lFiller As atcTimeseries In lFillers
                                    'lFillTS = FindObsTimeTS(pInputPath, lFiller)
                                    lFillerOTs.Add(lTSObs) 'lFillTS)
                                Next
                                'lFillTS = FindObsTimeTS(pInputPath, lts)
                                FillDailyTser(lts, lTSObs, lFillers, lFillerOTs, lMVal, lMAcc, 90)
                            End If
                            lStr = MissingDataSummary(lts, lMVal, lMAcc, lFMin, lFMax, lRepType)
                            lPctMiss = CDbl(lStr.Substring(lStr.LastIndexOf(",") + 1))
                            Logger.Dbg("FillMissing:  After Filling, % Missing:  " & lPctMiss)
                        Else
                            Logger.Dbg("FillMissing:  PROBLEM - Could not find any nearby stations for filling")
                        End If
                        'Else
                        '    Logger.Dbg("FillMissing:  No Missing Data for this dataset!")
                        'End If
                        'write filled data set to new WDM file
                        If lNewWDMfile.AddDataset(lts, atcDataSource.EnumExistAction.ExistReplace) Then
                            Logger.Dbg("FillMissing:  Updated " & lCons & " dataset to WDM file for station " & lStation)
                        Else
                            Logger.Dbg("FillMissing:  PROBLEM Updating " & lCons & " dataset to WDM file for station " & lStation)
                        End If
                    End If
                    'If lNewWDMfile.DataSets.Count > 0 Then 'save new WDM file
                    '    MkDirPath(pOutputPath & lStatePath)
                    '    lFName = pOutputPath & lStatePath & lStation & ".wdm"
                    '    FileCopy(lNewWDM, lFName)
                    '    Logger.Dbg("FillMissing:  Wrote updated WDM file to " & lFName)
                    '    lNewWDMfile.DataSets.Clear()
                    'Else
                    '    Logger.Dbg("FillMissing:  No dataset processed for station " & lStation & ", WDM file removed")
                    'End If
                    lFillers = Nothing
                    lFillerOTs = Nothing
                Else
                    Logger.Dbg("FillMissing:  PROBLEM - could not find station on location DBF file")
                End If
            End If
        Next
        Logger.StartToFile("FillMissEnd.log", , , True)
        Logger.Dbg("FillMissing:Completed Filling")

        'Application.Exit()

    End Sub

    Private Function FindFillTSers(ByVal aCurTS As atcTimeseries, ByVal aCons As String, ByVal aWDMFile As String) As atcCollection
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
            Select Case Left(aCons, 4)
                Case "HPCP"
                    If lCons = "HPCP" OrElse lCons = "HPCP1" Then
                        lChkDates = True
                    End If
                Case "TMIN", "TMAX", "WIND", "ATEM", "DPTE", "CLOU"
                    If lCons = aCons Then
                        lChkDates = True
                    End If
                Case "PRCP"
                    If lCons = "HPCP" OrElse lCons = "HPCP1" OrElse lCons = "PRCP" Then
                        lChkDates = True
                    End If
            End Select
            If lChkDates Then 'got right constituent, check quality (UBC200) and period of record
                If lts.Attributes.GetValue("UBC200") < 50 AndAlso _
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

    Private Function FindObsTimeTS(ByVal aObsTimePath As String, ByVal aTSer As atcTimeseries) As atcTimeseries
        Dim lCons As String = aTSer.Attributes.GetValue("Constituent")
        Dim lStation As String = aTSer.Attributes.GetValue("Location")
        Dim lState As String = lStation.Substring(0, 2)
        Dim lTSer As New atcTimeseries(Nothing)

        If aTSer.Attributes.GetValue("TU") = atcTimeUnit.TUDay AndAlso IsNumeric(lState) Then
            If lState < 68 OrElse lState = 91 Then
                'coop station, may find daily obs time dataset
                Dim lWDMFileName As String = aObsTimePath & lState & "\" & lStation & ".wdm"
                If FileExists(lWDMFileName) Then
                    FileCopy(lWDMFileName, "Temp.wdm")
                    Dim lWDMfile As New atcWDM.atcDataSourceWDM
                    lWDMfile.Open("Temp.wdm")
                    Dim lStaCons As String
                    For Each lts As atcTimeseries In lWDMfile.DataSets
                        lStaCons = lts.Attributes.GetValue("Constituent")
                        If lStaCons = lCons & "-OBS" Then
                            lTSer = lts
                            lTSer.EnsureValuesRead()
                            'Logger.Dbg("FillMissing:FindObsTimeTS: Found Obs Time TSer " & lTSer.ToString & " containing " & lTSer.numValues & " values.")
                            Exit For
                        End If
                    Next
                    lWDMfile.DataSets.Clear()
                    lWDMfile = Nothing
                    Kill("Temp.wdm")
                End If
            End If
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

End Module
