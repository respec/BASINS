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

Public Module FillATemp06
    Private Const pSubsetPath As String = "C:\BASINSMet\WDMFinal\subset\"
    Private Const pInputPath As String = "C:\BASINSMet\WDMFinal\"
    Private Const pStationPath As String = "C:\BasinsMet\Stations\"
    Private Const pOutputPath As String = "C:\BASINSMet\WDMFinal\Corrected\"
    Private Const pMaxNearStas As Integer = 30
    Private Const pMaxFillLength As Integer = 11 'any span < max time shift (10 hrs for HI)
    Private Const pAlreadyDone As String = "AK,AL,AR,AZ,CA,CO,CT,DE,FL,GA,HI"
    Declare Sub F90_MSG Lib "hass_ent.dll" _
        (ByVal aMsg As String, ByVal aMsgLen As Short)

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        Logger.Dbg("FillATemp06:Start")
        ChDriveDir(pOutputPath)
        Logger.Dbg(" CurDir:" & CurDir())

        Dim lEDateExisting() As Integer = {2005, 12, 31, 24, 0, 0}
        Dim lEJDateExisting As Double = Date2J(lEDateExisting)

        Dim lStationDBF As New atcTableDBF
        Dim lStation As String = ""
        Dim lStaFill As String = ""
        Dim lCons As String = ""
        Dim lID As Integer
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
        Dim lts As atcTimeseries

        Dim lStr As String = ""
        Dim lMVal As Double = 0.0
        Dim lMAcc As Double = -998.0
        Dim lFMin As Double = -100.0
        Dim lFMax As Double = 10000.0
        Dim lRepType As Integer = 1 'DBF parsing output format
        Dim lMisPds As Integer
        Dim lMisVals As Integer
        Dim lOriginalAve As Double
        Dim lOriginal06Ave As Double
        Dim lUpdated06Ave As Double
        Dim lPercentChange As Double
        Dim lPercent06Change As Double

        If lStationDBF.OpenFile(pStationPath & "StationLocs-Dist.dbf") Then
            Logger.Dbg("FillATemp06: Opened Station Location Master file " & pStationPath & "StationLocs-Dist.dbf")
        End If

        Dim X1 As Double
        Dim Y1 As Double
        Dim X2(lStationDBF.NumRecords) As Double
        Dim Y2(lStationDBF.NumRecords) As Double
        Dim lDist(lStationDBF.NumRecords) As Double
        Dim lPos(lStationDBF.NumRecords) As Integer
        Dim lRank(lStationDBF.NumRecords) As Integer

        Logger.Dbg("FillATemp06: Read all lat/lng values")
        For lrec As Integer = 1 To lStationDBF.NumRecords
            lStationDBF.CurrentRecord = lrec
            X2(lStationDBF.CurrentRecord) = lStationDBF.Value(7)
            Y2(lStationDBF.CurrentRecord) = lStationDBF.Value(8)
        Next

        Dim lFiles As NameValueCollection = Nothing
        Dim lContinueProcessing As Boolean = False
        AddFilesInDir(lFiles, pInputPath, False, "*.wdm")
        Logger.Dbg("FillATemp06: Found " & lFiles.Count & " data files")
        For Each lfile As String In lFiles
            lStation = FilenameNoExt(FilenameNoPath(lfile)).ToUpper
            If lStation = "WA452508" Then lContinueProcessing = True
            'If Not pAlreadyDone.Contains(lStation.Substring(0, 2)) Then
            If lContinueProcessing Then
                Logger.StartToFile(pOutputPath & lStation & "_ATemp06" & ".log", , , True)
                If lStationDBF.FindFirst(1, lStation.Substring(2)) Then
                    X1 = 0
                    Y1 = 0
                    While IO.File.Exists(lCurWDM) AndAlso Not TryDelete(lCurWDM)
                        System.Threading.Thread.Sleep(200)
                        Application.DoEvents()
                    End While
                    While Not TryCopy(lfile, lCurWDM)
                        System.Threading.Thread.Sleep(200)
                        Application.DoEvents()
                    End While
                    Dim lWDMfile As New atcWDM.atcDataSourceWDM
                    lWDMfile.Open(lfile)
                    Dim lCurWDMFile As New atcWDM.atcDataSourceWDM
                    lCurWDMFile.Open(lCurWDM)
                    If lWDMfile.DataSets.Keys.Contains(3) OrElse lWDMfile.DataSets.Keys.Contains(4) OrElse _
                       lWDMfile.DataSets.Keys.Contains(7) OrElse lWDMfile.DataSets.Keys.Contains(8) Then
                        For Each ltser As atcTimeseries In lWDMfile.DataSets
                            lID = ltser.Attributes.GetValue("ID")
                            lCons = ltser.Attributes.GetValue("Constituent")
                            If lID = 3 OrElse lID = 4 OrElse lID = 7 OrElse lID = 8 Then
                                If ltser.Attributes.GetValue("EJDay") > lEJDateExisting Then
                                    'save existing values through 2005
                                    Dim ltsExist As atcTimeseries = SubsetByDate(ltser, ltser.Attributes.GetValue("SJDay"), lEJDateExisting, Nothing)
                                    lOriginalAve = ltsExist.Attributes.GetValue("Mean")
                                    lts = SubsetByDate(ltser, lEJDateExisting, ltser.Attributes.GetValue("EJDay"), Nothing)
                                    lOriginal06Ave = lts.Attributes.GetValue("Mean")
                                    lAddMe = False
                                    lInterpolate = False
                                    lStr = MissingDataSummary(lts, lMVal, lMAcc, lFMin, lFMax, lRepType)
                                    lPctMiss = CDbl(lStr.Substring(lStr.LastIndexOf(",") + 1))
                                    For i = 1 To 5
                                        lStr = lStr.Substring(0, lStr.LastIndexOf(","))
                                    Next i
                                    lMisVals = CInt(lStr.Substring(lStr.LastIndexOf(",") + 1))
                                    lStr = lStr.Substring(0, lStr.LastIndexOf(","))
                                    lMisPds = CInt(lStr.Substring(lStr.LastIndexOf(",") + 1))
                                    If lPctMiss > 0 AndAlso (ltser.Attributes.GetValue("Scenario") <> "COMPUTED" OrElse lMisPds <> lMisVals) Then
                                        'missing values to fill (and not computed values with single instances of zero)
                                        If ltsExist.Attributes.GetValue("Scenario") = "COMPUTED" Then 'fix imcorrect scenario label
                                            ltsExist.Attributes.SetValue("Scenario", "OBSERVED")
                                        End If
                                        Logger.Dbg("FillATemp06")
                                        Logger.Dbg("FillATemp06:  Filling data for " & lts.ToString & ", " & lts.Attributes.GetValue("Description"))
                                        lAddMe = True
                                        Logger.Dbg("FillATemp06:  Before Interpolation, % Missing:  " & lPctMiss)
                                        'try interpolation for these hourly constituents
                                        Dim lMaxInterpLen As Double
                                        If lCons = "CLOU" OrElse lCons = "WIND" Then
                                            'only fill very short gaps as longer 0-value instances are likely valid data
                                            lMaxInterpLen = 2
                                        Else
                                            lMaxInterpLen = pMaxFillLength
                                        End If
                                        Logger.Dbg("FillATemp06:  Max span to interpolate is " & lMaxInterpLen & " hours")
                                        Dim lInterpTS As atcTimeseries = FillMissingByInterpolation(lts, (lMaxInterpLen + 0.001) / 24, , lMVal)
                                        If Not lInterpTS Is Nothing Then
                                            lts = lInterpTS
                                            lStr = MissingDataSummary(lts, lMVal, lMAcc, lFMin, lFMax, lRepType)
                                            lPctMiss = CDbl(lStr.Substring(lStr.LastIndexOf(",") + 1))
                                            Logger.Dbg("FillATemp06:  After Interpolation, % Missing:  " & lPctMiss)
                                        Else
                                            Logger.Dbg("FillATemp06:  PROBLEM with Interpolation")
                                        End If
                                        lInterpTS = Nothing
                                    Else
                                        Logger.Dbg("FillATemp06:  For " & lts.ToString & ", no missing values")
                                    End If
                                    If lAddMe Then
                                        If lPctMiss > 0.12 AndAlso (lCons = "ATEM" Or lCons = "DEWP") Then 'greater than 10 intervals (max interp span) missing
                                            If X1 < Double.Epsilon AndAlso Y1 < Double.Epsilon Then 'determine nearest geographic stations
                                                X1 = lStationDBF.Value(7)
                                                Y1 = lStationDBF.Value(8)
                                                Logger.Dbg("FillATemp06: For Station " & lStation & ", " & lStationDBF.Value(2) & "  at Lat/Lng " & lStationDBF.Value(4) & " / " & lStationDBF.Value(5))
                                                For i = 1 To lStationDBF.NumRecords
                                                    lDist(i) = System.Math.Sqrt((X1 - X2(i)) ^ 2 + (Y1 - Y2(i)) ^ 2)
                                                Next
                                                SortRealArray(0, lStationDBF.NumRecords, lDist, lPos)
                                                Logger.Dbg("FillATemp06: Sorted stations by distance")
                                            End If
                                            Logger.Dbg("FillATemp06:    Nearby Stations:")
                                            lFillers = New atcCollection
                                            i = 2
                                            j = 0
                                            While j < pMaxNearStas AndAlso i < lStationDBF.NumRecords
                                                'look through stations, in order of proximity, that can be used to fill
                                                lStationDBF.CurrentRecord = lPos(i)
                                                lStaFill = lStationDBF.Value(1)
                                                lFileName = pInputPath & lStationDBF.Value(3) & lStaFill & ".wdm"
                                                If FileExists(lFileName) Then
                                                    lFillTsers = FindFillTSers(lts, lCons, lFileName)
                                                    For k As Integer = 0 To lFillTsers.Count - 1
                                                        lFillTS = lFillTsers(k)
                                                        If Not lFillTS Is Nothing Then
                                                            'contains data for time period being filled
                                                            lFillers.Add(lDist(lPos(i)), lFillTS)
                                                            j += 1
                                                            Logger.Dbg("FillATemp06:  Using " & _
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
                                                Logger.Dbg("FillATemp06:  Found " & j & " nearby stations for filling")
                                                Logger.Dbg("FillATemp06:  Before Filling, % Missing:  " & lPctMiss)
                                                If lts.Attributes.GetValue("TU") = atcTimeUnit.TUHour Then
                                                    If lPctMiss > 0 Then
                                                        FillHourlyTser(lts, lFillers, lMVal, lMAcc, 90)
                                                    Else
                                                        Logger.Dbg("FillATemp06:  All Missing periods filled via interpolation")
                                                    End If
                                                End If
                                                lStr = MissingDataSummary(lts, lMVal, lMAcc, lFMin, lFMax, lRepType)
                                                lPctMiss = CDbl(lStr.Substring(lStr.LastIndexOf(",") + 1))
                                                Logger.Dbg("FillATemp06:  After Filling, % Missing:  " & lPctMiss)
                                            Else
                                                Logger.Dbg("FillATemp06:  PROBLEM - Could not find any nearby stations for filling")
                                            End If
                                            Logger.Flush()
                                            If lPctMiss > 0 And WholeFileString(Logger.FileName).Contains("PROBLEM") Then
                                                'fill remaining missing by interpolation for these hourly constituents
                                                Dim lFillInstances As New ArrayList
                                                Logger.Dbg("FillATemp06:  NOTE - Forcing Interpolation of all remaining missing periods")
                                                Dim lInterpTS As atcTimeseries = FillMissingByInterpolation(lts, , lFillInstances, lMVal)
                                                If Not lInterpTS Is Nothing Then
                                                    lts = lInterpTS
                                                    lInterpTS = Nothing
                                                    lStr = MissingDataSummary(lts, lMVal, lMAcc, lFMin, lFMax, lRepType)
                                                    lPctMiss = CDbl(lStr.Substring(lStr.LastIndexOf(",") + 1))
                                                    Logger.Dbg("FillATemp06:  After Interpolation, % Missing:  " & lPctMiss)
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
                                                    Logger.Dbg("FillATemp06:  Forced Interpolation Summary" & vbCrLf & _
                                                               "                " & lFillInstances.Count & " instances of interpolation" & vbCrLf & _
                                                               "                   " & lWeek & " longer than 1 week" & vbCrLf & _
                                                               "                   " & lTwoDays & " longer than 2 days" & vbCrLf & _
                                                               "                   " & lDay & " longer than 1 Day" & vbCrLf & _
                                                               "                   " & lHalfDay & " longer than 12 hours" & vbCrLf & _
                                                               "                   " & lQtrDay & " longer than 6 hours" & vbCrLf & _
                                                               "                   " & lHours & " less than 6 hours" & vbCrLf)
                                                Else
                                                    Logger.Dbg("FillATemp06:  PROBLEM with Interpolation")
                                                End If
                                            End If
                                        Else
                                            Logger.Dbg("FillATemp06:  For " & lts.ToString & ", interpolation only, no filling from nearby stations needed.")
                                        End If
                                        'write filled data set to new WDM file
                                        ltsExist.Attributes.SetValue("ID", ltser.Attributes.GetValue("ID"))
                                        If lCurWDMFile.AddDataset(ltsExist, atcDataSource.EnumExistAction.ExistReplace) Then
                                            Logger.Dbg("FillATemp06:  Rewrote original " & lCons & " data through 2005 to WDM file for station " & lStation)
                                            lts.Attributes.DiscardCalculated()
                                            lts.Attributes.SetValue("ID", ltser.Attributes.GetValue("ID"))
                                            lUpdated06Ave = lts.Attributes.GetValue("Mean")
                                            If lCurWDMFile.AddDataset(lts, atcDataSource.EnumExistAction.ExistAppend) Then
                                                Logger.Dbg("FillATemp06:  Appended updated " & lCons & " data to WDM file for station " & lStation)
                                                Logger.Dbg("FillATemp06:  Original Ave (thru 05): " & DoubleToString(lOriginalAve, 4, "##.0") & ",  Original 2006 Ave: " & DoubleToString(lOriginal06Ave, 4, "##.0") & ",  Updated 2006 Ave: " & DoubleToString(lUpdated06Ave, 4, "##.0"))
                                                lPercent06Change = 100 * ((lUpdated06Ave - lOriginal06Ave) / lOriginal06Ave)
                                                lPercentChange = 100 * ((lUpdated06Ave - lOriginalAve) / lOriginalAve)
                                                Logger.Dbg("FillATemp06:  % 2006 Change: " & DoubleToString(lPercent06Change, 4, "##.0") & ",  % Diff (06 from previous): " & DoubleToString(lPercentChange, 4, "##.0"))
                                                If lPercent06Change > 20 Then Logger.Dbg("FillATemp06:  NOTE: Significant change in 2006 mean")
                                                If lPercentChange > 10 Then Logger.Dbg("FillATemp06:  NOTE: Significant difference in updated 2006 mean from rest of dataset")
                                                If lPercent06Change > 20 And lPercentChange > 10 Then Logger.Dbg("FillATemp06:  POSSIBLE ERROR - high percent change in 06 and difference from rest of dataset")
                                                FileCopy(lCurWDM, pOutputPath & lStation & ".wdm")
                                            End If
                                        Else
                                            Logger.Dbg("FillATemp06:  PROBLEM adding " & lCons & " dataset to WDM file for station " & lStation)
                                        End If
                                    End If
                                    ltsExist = Nothing
                                Else
                                    Logger.Dbg("FillATemp06:  No 2006 data available for " & ltser.ToString)
                                End If
                            End If
                        Next
                        lWDMfile.DataSets.Clear()
                        lWDMfile = Nothing
                        lCurWDMFile.DataSets.Clear()
                        lCurWDMFile = Nothing
                        Kill(lCurWDM)
                    Else
                        Logger.Dbg("FillATemp06:  No existing ATEM, WIND, DEWP, or CLOU data for WDM file: " & lWDMfile.Name)
                    End If
                Else
                    Logger.Dbg("FillMissing:  PROBLEM - could not find station on location DBF file")
                End If
            End If
        Next
        Logger.StartToFile("FillATemp06.log", , , True)
        Logger.Dbg("FillATemp06:Completed Filling")

        'Application.Exit()

    End Sub

    Private Function FindFillTSers(ByVal aCurTS As atcTimeseries, ByVal aCons As String, ByVal aWDMFile As String) As atcCollection
        Dim lSJD As Double = aCurTS.Attributes.GetValue("SJDay")
        Dim lEJD As Double = aCurTS.Attributes.GetValue("EJDay")
        Dim lCons As String = ""
        Dim lChkDates As Boolean = False
        Dim lTSers As New atcCollection

        'If FileExists("Temp.wdm") Then
        '    Kill("Temp.wdm")
        'End If
        'FileCopy(aWDMFile, "Temp.wdm")
        Dim lWDMfile As New atcWDM.atcDataSourceWDM
        lWDMfile.Open(aWDMFile) '("Temp.wdm")
        For Each lts As atcTimeseries In lWDMfile.DataSets
            lChkDates = False
            lCons = lts.Attributes.GetValue("Constituent")
            Select Case Left(aCons, 4)
                Case "HPCP"
                    If lCons = "HPCP" OrElse lCons = "HPCP1" Then
                        lChkDates = True
                    End If
                Case "TMIN", "TMAX", "WIND", "ATEM", "DEWP", "CLOU"
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
                    lts.EnsureValuesRead()
                    lTSers.Add(lts)
                End If
            End If
        Next
        lWDMfile.DataSets.Clear()
        lWDMfile = Nothing
        'Kill("Temp.wdm")
        Return lTSers
    End Function

End Module
