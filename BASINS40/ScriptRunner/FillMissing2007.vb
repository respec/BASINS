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

Public Module FillMissing2007
    Private Const pSubsetPath As String = "C:\BASINSMet\WDMFilled\subset\"
    Private Const pInputPath As String = "C:\BASINSMet\WDMFilled\" '"C:\BASINSMet\WDMFiltered\"
    Private Const pStationPath As String = "H:\BasinsMet\Stations\"
    Private Const pOutputPath As String = "C:\BASINSMet\WDMFilled2007\" '"C:\BASINSMet\WDMFilled\"
    Private Const pMaxNearStas As Integer = 30
    Private Const pMaxFillLength As Integer = 11 'any span < max time shift (10 hrs for HI)
    Private Const pMinNumHrly As Integer = 43830 '5 years of hourly values
    Private Const pMinNumDly As Integer = 1830 '5 years of daily
    Private Const pMaxPctMiss As Integer = 35 'was 20, use higher number since it's just one year appended to a quality tser
    Private Const pAlreadyDone As String = "" '"01,02,03,04,05,06,07,08,09,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35" ',36,37,38,39,40,41,42,43,44,45,46" ',47,48,50,51,66,67,ak,al,ar,az,ca,co,ct,de,fl,ga,hi,ia,id,il,in,ks,ky,la,ma,md,me,mi,mn,mo,ms,mt,nc,nd,ne,nh,nj,nm,nv,ny"
    Declare Sub F90_MSG Lib "hass_ent.dll" _
        (ByVal aMsg As String, ByVal aMsgLen As Short)

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        Logger.Dbg("FillMissing:Start")
        ChDriveDir(pOutputPath)
        Logger.Dbg(" CurDir:" & CurDir())

        Dim lEDateExisting() As Integer = {2006, 12, 31, 24, 0, 0}
        Dim lEJDateExisting As Double = Date2J(lEDateExisting)

        Dim lStationDBF As New atcTableDBF
        Dim lStation As String = ""
        Dim lStaFill As String = ""
        Dim lStatePath As String
        Dim lCons As String = ""
        Dim lFName As String = ""
        Dim lPctMiss As Double
        Dim lCurWDM As String = pOutputPath & "current.wdm"
        Dim lNewWDM As String = pOutputPath & "new.wdm"
        Dim lAddMe As Boolean = True
        Dim lInterpolate As Boolean
        Dim i As Integer
        Dim j As Integer
        Dim lFillTsers As atcCollection
        Dim lFillTS As atcTimeseries = Nothing
        Dim lFileName As String = ""
        Dim lFilledTS As atcTimeseries = Nothing
        Dim lFillers As atcCollection = Nothing
        Dim lFillerOTs As atcCollection = Nothing
        Dim lts As atcTimeseries

        Dim lStr As String = ""
        Dim lMVal As Double = -999.0
        Dim lMAcc As Double = -998.0
        Dim lFMin As Double = -100.0
        Dim lFMax As Double = 10000.0
        Dim lRepType As Integer = 1 'DBF parsing output format

        If lStationDBF.OpenFile(pStationPath & "StationLocs-Dist.dbf") Then
            Logger.Dbg("FillMissing: Opened Station Location Master file " & pStationPath & "StationLocs-Dist.dbf")
        End If

        Dim X1 As Double
        Dim Y1 As Double
        Dim X2(lStationDBF.NumRecords) As Double
        Dim Y2(lStationDBF.NumRecords) As Double
        Dim lDist(lStationDBF.NumRecords) As Double
        Dim lPos(lStationDBF.NumRecords) As Integer
        Dim lRank(lStationDBF.NumRecords) As Integer

        Logger.Dbg("FillMissing: Read all lat/lng values")
        For lrec As Integer = 1 To lStationDBF.NumRecords
            lStationDBF.CurrentRecord = lrec
            X2(lStationDBF.CurrentRecord) = lStationDBF.Value(7)
            Y2(lStationDBF.CurrentRecord) = lStationDBF.Value(8)
        Next

        Dim lFiles As NameValueCollection = Nothing
        'AddFilesInDir(lFiles, pInputPath, True, "*.wdm")
        AddFilesInDir(lFiles, pSubsetPath, True, "*.wdm")
        Logger.Dbg("FillMissing: Found " & lFiles.Count & " data files")
        For Each lfile As String In lFiles
            lStation = FilenameNoExt(FilenameNoPath(lfile))
            lStatePath = Right(PathNameOnly(lfile), 2) & "\"
            If Not pAlreadyDone.Contains(lStatePath.Substring(0, 2)) Then
                Logger.StartToFile(pOutputPath & lStatePath & lStation & "_Fill_2007" & ".log", , , True)
                If lStationDBF.FindFirst(1, lStation) Then
                    X1 = 0
                    Y1 = 0
                    If FileExists(pOutputPath & lStatePath & FilenameNoPath(lfile)) Then
                        'existing filled WDM exists for this station
                        Try
                            FileCopy(lfile, lCurWDM)
                            Dim lWDMfile As New atcWDM.atcDataSourceWDM
                            lWDMfile.Open(lCurWDM)
                            'open existing filled WDM file for appending 2007 data
                            Dim lNewWDMfile As New atcWDM.atcDataSourceWDM
                            lNewWDMfile.Open(pOutputPath & lStatePath & FilenameNoPath(lfile))
                            For Each ltser As atcTimeseries In lWDMfile.DataSets
                                If lNewWDMfile.DataSets.Keys.Contains(ltser.Attributes.GetValue("ID")) Then
                                    Dim ltsExist As atcTimeseries = lNewWDMfile.DataSets.ItemByKey(ltser.Attributes.GetValue("ID"))
                                    If ltser.Attributes.GetValue("EJDay") > ltsExist.Attributes.GetValue("EJDay") Then
                                        lts = SubsetByDate(ltser, ltsExist.Attributes.GetValue("EJDay"), ltser.Attributes.GetValue("EJDay"), Nothing)
                                        lAddMe = False
                                        lInterpolate = False
                                        lCons = lts.Attributes.GetValue("Constituent")
                                        lStr = MissingDataSummary(lts, lMVal, lMAcc, lFMin, lFMax, lRepType)
                                        lPctMiss = CDbl(lStr.Substring(lStr.LastIndexOf(",") + 1))
                                        'lPctMiss = CDbl(lts.Attributes.GetValue("UBC200"))
                                        Select Case lCons 'look for wanted constituents and check % missing
                                            Case "HPCP", "HPCP1", "TMIN", "TMAX", "PRCP"
                                                If lPctMiss < pMaxPctMiss Then '% missing OK
                                                    ExtendISHTSer(lts)
                                                    Logger.Dbg(vbCrLf & "FillMissing:  Filling data for " & lts.ToString & ", " & lts.Attributes.GetValue("Description"))
                                                    lAddMe = True
                                                Else
                                                    Logger.Dbg(vbCrLf & "FillMissing:  For " & lts.ToString & ", percent Missing (" & lPctMiss & ") too large (> " & pMaxPctMiss & ")")
                                                End If
                                            Case "ATEMP", "DPTEMP", "WIND", "CLOU"
                                                If lPctMiss < pMaxPctMiss Then '% missing OK
                                                    Logger.Dbg(vbCrLf & "FillMissing:  Filling data for " & lts.ToString & ", " & lts.Attributes.GetValue("Description"))
                                                    lAddMe = True
                                                    'extend TSer to end of last day (from ISH data being time shifted)
                                                    ExtendISHTSer(lts)
                                                    Logger.Dbg("FillMissing:  Before Interpolation, % Missing:  " & lPctMiss)
                                                    Logger.Dbg("FillMissing:  Max span to interpolate is " & pMaxFillLength & " hours")
                                                    'try interpolation for these hourly constituents
                                                    Dim lInterpTS As atcTimeseries = FillMissingByInterpolation(lts, (CDbl(pMaxFillLength) + 0.001) / 24, , lMVal)
                                                    If Not lInterpTS Is Nothing Then
                                                        lts = lInterpTS
                                                        lts.Attributes.SetValue("ID", ltsExist.Attributes.GetValue("ID"))
                                                        lInterpTS = Nothing
                                                        lStr = MissingDataSummary(lts, lMVal, lMAcc, lFMin, lFMax, lRepType)
                                                        lPctMiss = CDbl(lStr.Substring(lStr.LastIndexOf(",") + 1))
                                                        Logger.Dbg("FillMissing:  After Interpolation, % Missing:  " & lPctMiss)
                                                    Else
                                                        Logger.Dbg("FillMissing:  PROBLEM with Interpolation")
                                                    End If
                                                Else
                                                    Logger.Dbg(vbCrLf & "FillMissing:  For " & lts.ToString & ", percent Missing (" & lPctMiss & ") too large (> " & pMaxPctMiss & ")")
                                                End If
                                            Case Else
                                                Logger.Dbg(vbCrLf & "FillMissing:  Not processing constituent for " & lts.ToString)
                                        End Select
                                        If lAddMe Then
                                            If lPctMiss > 0 Then
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
                                                If lCons = "PRCP" Then 'see if HPCP exists on same WDM
                                                    For Each llts As atcTimeseries In lWDMfile.DataSets
                                                        If llts.Attributes.GetValue("Constituent") = "HPCP" Then
                                                            lFillers.Add(0, llts)
                                                            Logger.Dbg("FillMissing:  Using " & _
                                                                       llts.Attributes.GetValue("Constituent") & " from " & _
                                                                       llts.Attributes.GetValue("Location") & " " & _
                                                                       llts.Attributes.GetValue("STANAM") & " at Lat/Lng " & _
                                                                       llts.Attributes.GetValue("LATDEG") & "/" & llts.Attributes.GetValue("LNGDEG"))
                                                            j += 1
                                                        End If
                                                    Next
                                                End If
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
                                                        lFillTsers = FindFillTSers(lts, lCons, lFileName)
                                                        For k As Integer = 0 To lFillTsers.Count - 1
                                                            lFillTS = lFillTsers(k)
                                                            If Not lFillTS Is Nothing Then
                                                                'contains data for time period being filled
                                                                '"k * 0.1" multiplier used for case of daily and hourly precip
                                                                'available at same station to avoid duplicate keys
                                                                '"i * 0.01" multiplier for case of 2 stations at same location
                                                                lFillers.Add(lDist(lPos(i)) + k * 0.1 + i * 0.01, lFillTS)
                                                                j += 1
                                                                Logger.Dbg("FillMissing:  Using " & _
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
                                                    Logger.Dbg("FillMissing:  Found " & j & " nearby stations for filling")
                                                    Logger.Dbg("FillMissing:  Before Filling, % Missing:  " & lPctMiss)
                                                    If lts.Attributes.GetValue("TU") = atcTimeUnit.TUHour Then
                                                        If lPctMiss > 0 Then
                                                            FillHourlyTser(lts, lFillers, lMVal, lMAcc, 90)
                                                        Else
                                                            Logger.Dbg("FillMissing:  All Missing periods filled via interpolation")
                                                        End If
                                                    Else 'daily tser, locate obs time tsers
                                                        For Each lFiller As atcTimeseries In lFillers
                                                            lFillTS = FindObsTimeTS(pInputPath, lFiller)
                                                            lFillerOTs.Add(lFillTS)
                                                        Next
                                                        lFillTS = FindObsTimeTS(pInputPath, lts)
                                                        FillDailyTser(lts, lFillTS, lFillers, lFillerOTs, lMVal, lMAcc, 90)
                                                    End If
                                                    lStr = MissingDataSummary(lts, lMVal, lMAcc, lFMin, lFMax, lRepType)
                                                    lPctMiss = CDbl(lStr.Substring(lStr.LastIndexOf(",") + 1))
                                                    Logger.Dbg("FillMissing:  After Filling, % Missing:  " & lPctMiss)
                                                Else
                                                    Logger.Dbg("FillMissing:  PROBLEM - Could not find any nearby stations for filling")
                                                End If
                                                If lPctMiss > 0 AndAlso (lCons = "ATEMP" OrElse lCons = "DPTEMP" OrElse _
                                                                         lCons = "WIND" OrElse lCons = "CLOU") Then
                                                    'fill remaining missing by interpolation for these hourly constituents
                                                    Dim lFillInstances As New ArrayList
                                                    Logger.Dbg("FillMissing:  NOTE - Forcing Interpolation of all remaining missing periods")
                                                    Dim lInterpTS As atcTimeseries = FillMissingByInterpolation(lts, , lFillInstances)
                                                    If Not lInterpTS Is Nothing Then
                                                        lts = lInterpTS
                                                        lInterpTS = Nothing
                                                        lStr = MissingDataSummary(lts, lMVal, lMAcc, lFMin, lFMax, lRepType)
                                                        lPctMiss = CDbl(lStr.Substring(lStr.LastIndexOf(",") + 1))
                                                        Logger.Dbg("FillMissing:  After Interpolation, % Missing:  " & lPctMiss)
                                                        Dim lHours As Integer = 0
                                                        Dim lQtrDay As Integer = 0
                                                        Dim lHalfDay As Integer = 0
                                                        Dim lDay As Integer = 0
                                                        Dim lTwoDays As Integer = 0
                                                        Dim lWeek As Integer = 0
                                                        For Each lInstance As Double In lFillInstances
                                                            If lInstance > 7 Then
                                                                lWeek += 1
                                                            ElseIf lInstance > 2 Then
                                                                lTwoDays += 1
                                                            ElseIf lInstance > 1 Then
                                                                lDay += 1
                                                            ElseIf lInstance > 0.5 Then
                                                                lHalfDay += 1
                                                            ElseIf lInstance > 0.25 Then
                                                                lQtrDay += 1
                                                            Else
                                                                lHours += 1
                                                            End If
                                                        Next
                                                        Logger.Dbg("FillMissing:  Forced Interpolation Summary" & vbCrLf & _
                                                                   "                " & lFillInstances.Count & " instances of interpolation" & vbCrLf & _
                                                                   "                   " & lWeek & " longer than 1 week" & vbCrLf & _
                                                                   "                   " & lTwoDays & " longer than 2 days" & vbCrLf & _
                                                                   "                   " & lDay & " longer than 1 Day" & vbCrLf & _
                                                                   "                   " & lHalfDay & " longer than 12 hours" & vbCrLf & _
                                                                   "                   " & lQtrDay & " longer than 6 hours" & vbCrLf & _
                                                                   "                   " & lHours & " less than 6 hours" & vbCrLf)
                                                    Else
                                                        Logger.Dbg("FillMissing:  PROBLEM with Interpolation")
                                                    End If
                                                End If
                                            Else
                                                Logger.Dbg("FillMissing:  No Missing Data for this dataset!")
                                            End If
                                            'write filled data set to new WDM file
                                            If lNewWDMfile.AddDataset(lts, atcDataSource.EnumExistAction.ExistAppend) Then
                                                Logger.Dbg("FillMissing:  Appended " & lCons & " dataset to WDM file for station " & lStation)
                                            Else
                                                Logger.Dbg("FillMissing:  PROBLEM appending " & lCons & " dataset to WDM file for station " & lStation)
                                            End If
                                        End If
                                    Else
                                        Logger.Dbg("FillMissing:  No newer data available for " & ltsExist.ToString)
                                    End If
                                    ltsExist = Nothing
                                Else
                                    Logger.Dbg("FillMissing:  No existing data for 2007 update: " & ltser.ToString)
                                End If
                            Next
                            'If lNewWDMfile.DataSets.Count > 0 Then 'save new WDM file
                            '    MkDirPath(pOutputPath & lStatePath)
                            '    lFName = pOutputPath & lStatePath & lStation & ".wdm"
                            '    FileCopy(lNewWDM, lFName)
                            '    Logger.Dbg("FillMissing:  Wrote updated WDM file to " & lFName)
                            '    lNewWDMfile.DataSets.Clear()
                            'Else
                            '    Logger.Dbg("FillMissing:  No dataset processed for station " & lStation & ", WDM file removed")
                            'End If
                            lNewWDMfile.Clear()
                            lNewWDMfile = Nothing
                            'Kill(lNewWDM)
                            lWDMfile.Clear()
                            lWDMfile = Nothing
                            While IO.File.Exists(lCurWDM)
                                TryDelete(lCurWDM)
                            End While
                        Catch lEx As Exception
                            Logger.Dbg(vbCrLf & "Exception: " & lEx.ToString & vbCrLf)
                            If lEx.InnerException IsNot Nothing Then
                                Logger.Dbg(vbCrLf & "Inner Exception: " & lEx.InnerException.Message & vbCrLf & lEx.InnerException.StackTrace & vbCrLf)
                            End If
                            Logger.Flush()
                        End Try
                    End If
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
        Dim lCopied As Boolean = False

        While Not lCopied
            Try
                FileCopy(aWDMFile, "Temp.wdm")
                lCopied = True
            Catch
                Application.DoEvents()
                System.Threading.Thread.CurrentThread.Sleep(100)
            End Try
        End While
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
        While IO.File.Exists("Temp.wdm")
            TryDelete("Temp.wdm")
        End While
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
TryAgain:
                    Try
                        If FileExists("Temp.wdm") Then System.IO.File.Delete("Temp.wdm")
                        FileCopy(lWDMFileName, "Temp.wdm")
                    Catch
                        GoTo TryAgain
                    End Try
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
                    System.IO.File.Delete("Temp.wdm")
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
