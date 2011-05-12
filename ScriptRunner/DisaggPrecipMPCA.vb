Imports Microsoft.VisualBasic
Imports System.Collections
Imports System.Collections.Specialized
Imports System.IO
Imports System.Windows.Forms
Imports System.Math
Imports MapWindow.Interfaces
Imports MapWinUtility
Imports MapWinUtility.Strings

Imports atcUtility
Imports atcData
Imports atcWDM

Public Module DisaggPrecipMPCA
    Private Const pInputPath As String = "D:\BasinsMet\MPCA\Combined\"
    Private Const pStationPath As String = "D:\BasinsMet\MPCA\Combined\"
    Private Const pOutputPath As String = "D:\BasinsMet\MPCA\Combined\"
    Private Const pMaxNearStas As Integer = 10
    Private Const pTolerance As Integer = 90
    Private Const pFormat As String = "#,##0.00"
    Declare Sub F90_MSG Lib "hass_ent.dll" _
        (ByVal aMsg As String, ByVal aMsgLen As Short)

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        Logger.StartToFile(pStationPath & "DisaggPrecipMPCA.log", , , True)
        Logger.Dbg("DisaggPrec:Start")
        ChDriveDir(pOutputPath)
        Logger.Dbg(" CurDir:" & CurDir())

        Dim lStationFile As New atcTableDelimited
        If lStationFile.OpenFile(pStationPath & "MetStationTable.csv") Then
            Logger.Dbg("DisaggPrec: Opened Station Location Master file")
        End If

        Dim X1 As Double
        Dim Y1 As Double
        Dim X2(lStationFile.NumRecords) As Double
        Dim Y2(lStationFile.NumRecords) As Double
        Dim lDist(lStationFile.NumRecords) As Double
        Dim lPos(lStationFile.NumRecords) As Integer
        Dim lRank(lStationFile.NumRecords) As Integer

        Logger.Dbg("DisaggPrec: Read all lat/lng values")
        lStationFile.CurrentRecord = 1
        While Not lStationFile.EOF
            X2(lStationFile.CurrentRecord) = lStationFile.Value(2)
            Y2(lStationFile.CurrentRecord) = lStationFile.Value(1)
            lStationFile.CurrentRecord += 1
        End While

        Dim lCurWdm As String = pOutputPath & "MNFilledMet.wdm"
        Dim lWDMfile As New atcWDM.atcDataSourceWDM
        lWDMfile.Open(lCurWdm)

        Dim lBasinsMetWDM As String = pOutputPath & "MN_Met.wdm"
        Dim lBasinsMetWDMfile As New atcWDM.atcDataSourceWDM
        lBasinsMetWDMfile.Open(lBasinsMetWDM)

        Dim lNewWDM As String = pOutputPath & "MNDisaggMet.wdm"
        Dim lNewWDMfile As New atcWDM.atcDataSourceWDM
        lNewWDMfile.Open(lNewWDM)

        Dim lStation As String = ""
        Dim lStaFill As String = ""
        Dim lCons As String = ""
        Dim lAddMe As Boolean = True
        Dim i As Integer
        Dim j As Integer
        Dim lFillTsers As atcCollection
        Dim lFillTS As atcTimeseries = Nothing
        Dim lFillers As atcDataGroup = Nothing
        For Each lts As atcTimeseries In lWDMfile.DataSets
            lAddMe = False
            lCons = lts.Attributes.GetValue("Constituent")
            lStation = lts.Attributes.GetValue("Location")
            'look for precip datasets to disaggregate
            If lCons = "PRCP" AndAlso lts.Attributes.GetValue("tu") = atcTimeUnit.TUDay Then
                lAddMe = True
            End If
            If lAddMe Then
                If X1 < Double.Epsilon AndAlso Y1 < Double.Epsilon Then 'determine nearest geographic stations
                    lStation = lts.Attributes.GetValue("Stanam")
                    If lStationFile.FindFirst(3, lStation) Then
                        X1 = lStationFile.Value(2)
                        Y1 = lStationFile.Value(1)
                    End If
                    Logger.Dbg("DisaggPrec: For Station " & lStation & " at Lat/Lng " & Y1 & " / " & X1)
                    For i = 1 To lStationFile.NumRecords - 1
                        lDist(i) = System.Math.Sqrt((X1 - X2(i)) ^ 2 + (Y1 - Y2(i)) ^ 2)
                    Next
                    SortRealArray(0, lStationFile.NumRecords - 1, lDist, lPos)
                    Logger.Dbg("DisaggPrec: Sorted stations by distance")
                End If
                Logger.Dbg("DisaggPrec:    Nearby Stations:")

                lFillers = New atcTimeseriesGroup

                i = 2
                j = 0
                While j < pMaxNearStas AndAlso i < lStationFile.NumRecords
                    'look through stations, in order of proximity, that can be used to fill
                    lStationFile.CurrentRecord = lPos(i)
                    lStaFill = lStationFile.Value(3)
                    lFillTsers = FindHrlyPrecips(lts, "PREC", lStaFill, lBasinsMetWDMfile)
                    For k As Integer = 0 To lFillTsers.Count - 1
                        lFillTS = lFillTsers(k)
                        If Not lFillTS Is Nothing Then
                            'contains data for time period being filled
                            lFillers.Add(lDist(lPos(i)), lFillTS)
                            j += 1
                            Logger.Dbg("DisaggPrec:  Using " & _
                                      lFillTS.Attributes.GetValue("Constituent") & " from " & _
                                      lFillTS.Attributes.GetValue("Location") & " " & _
                                      lFillTS.Attributes.GetValue("STANAM") & " at Lat/Lng " & _
                                      lStationFile.Value(2) & "/" & lStationFile.Value(1))
                        End If
                    Next
                    i += 1
                End While

                If j > 0 Then
                    Logger.Dbg("DisaggPrec:  Found " & j & " nearby stations for disaggregation")
                    Dim lSummFile As String = pOutputPath & lStation & "_Disagg.sum"
                    Dim lDisaggTS As atcTimeseries = DisPrecip(lts, Nothing, lFillers, 0, pTolerance, lSummFile)
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
                    Logger.Dbg("DisaggPrec:  PROBLEM - Could not find any nearby stations for filling")
                End If
            End If
        Next
        Logger.Dbg("DisaggPrec: Completed Disaggregation")

    End Sub

    Private Function FindHrlyPrecips(ByVal aCurTS As atcTimeseries, ByVal aCons As String, ByVal aLocn As String, ByVal aWDMFile As atcWDM.atcDataSourceWDM) As atcCollection
        Dim lSJD As Double = aCurTS.Attributes.GetValue("SJDay")
        Dim lEJD As Double = aCurTS.Attributes.GetValue("EJDay")
        Dim lCons As String = ""
        Dim lChkDates As Boolean = False
        Dim lTSers As New atcCollection

        For Each lts As atcTimeseries In aWDMFile.DataSets
            lChkDates = False
            lCons = lts.Attributes.GetValue("Constituent")

            lCons = lts.Attributes.GetValue("Constituent")
            Dim lLocn As String = ""
            If aLocn.Length <= 8 Then
                lLocn = lts.Attributes.GetValue("Location")
            Else
                lLocn = lts.Attributes.GetValue("StaNam")
            End If
            If lCons = aCons And lLocn = aLocn Then
                'got right constituent, check quality (UBC200) and period of record
                If lts.Attributes.GetValue("UBC200") < 50 AndAlso _
                   lts.numValues > 44000 AndAlso _
                   lts.Attributes.GetValue("SJDay") < lEJD AndAlso _
                   lts.Attributes.GetValue("EJDay") > lSJD Then 'some portion falls within filling period
                    lTSers.Add(lts)
                End If
            End If
        Next

        Return lTSers
    End Function

    Public Function DisPrecip(ByVal aDyTSer As atcTimeseries, ByVal aDataSource As atcTimeseriesSource, ByVal aHrTSer As atcTimeseriesGroup, ByVal aObsTime As Integer, ByVal aTolerance As Double, Optional ByVal aSumFile As String = "") As atcTimeseries
        'aDyTSer - daily time series being disaggregated
        'aHrTser - collection of hourly timeseries used to disaggregate daily
        'aObsTime - observation time of daily precip (1 - 24)
        'aTolerance - tolerance for comparison of hourly daily sums to daily value (%)
        'aSumFile - name of file for output summary info

        'build obs time TSer with constant value from aObsTime argument
        Dim lObsTimeTS As atcTimeseries = aDyTSer.Clone
        For i As Integer = 1 To lObsTimeTS.numValues
            lObsTimeTS.Values(i) = aObsTime
        Next
        lObsTimeTS.Attributes.SetValue("Scenario", "CONST-" & aObsTime)
        lObsTimeTS.Attributes.SetValue("Constituent", aDyTSer.Attributes.GetValue("Constituent") & "-OBS")
        Return DisaggPrecip(aDyTSer, aDataSource, aHrTSer, lObsTimeTS, aTolerance, aSumFile)
    End Function

    'DisaggPrecip performs disaggregation of daily precip to hourly 
    'using a timeseries for the observation time
    Public Function DisaggPrecip(ByVal aDyTSer As atcTimeseries, ByVal aDataSource As atcTimeseriesSource, ByVal aHrTSer As atcTimeseriesGroup, ByVal aObsTimeTS As atcTimeseries, ByVal aTolerance As Double, Optional ByVal aSumFile As String = "") As atcTimeseries
        'aDyTSer - daily time series being disaggregated
        'aHrTser - collection of hourly timeseries used to disaggregate daily
        'aObsTimeTS - timeseries of observation times for daily precip data (1 - 24)
        'aTolerance - tolerance for comparison of hourly daily sums to daily value (%)
        'aSumFile - name of file for output summary info

        Dim lHrPos, i, lMaxHrInd As Integer
        Dim lRndOff, lCarry, lMaxHrVal As Double
        Dim lDyInd, lHrInd As Integer
        Dim lRatio, lDaySum, lClosestDaySum, lClosestRatio As Double
        Dim lSDt, lEDt As Double
        Dim lTolerance As Double
        Dim lDate(5) As Integer
        Dim lOutSumm As Boolean
        Dim lOutFil As Integer
        Dim s As String = ""
        Dim rsp, retcod, lUsedTriang As Integer
        Dim lClosestHrTser As atcTimeseries = Nothing
        Dim lDaySumHrTser As atcTimeseries
        Dim lDisTs As New atcTimeseries(aDataSource)
        Dim lDistCnt As Integer = 0
        Dim lTriDistCnt As Integer = 0
        Dim lTriDist0To1Cnt As Integer = 0
        Dim lTriDist1To2Cnt As Integer = 0
        Dim lTriDist2To3Cnt As Integer = 0
        Dim lTriDistGT3Cnt As Integer = 0
        Dim lNVals As Integer

        On Error GoTo DisPrecipErrHnd
        lUsedTriang = 0
        lRndOff = 0.001
        If Len(aSumFile) > 0 Then
            lOutSumm = True
            lOutFil = FreeFile()
            FileOpen(lOutFil, aSumFile, OpenMode.Output)
        Else
            lOutSumm = False
        End If
        If aTolerance > 1.0 Then 'assume tolerance passed as percentage if greater than 1
            lTolerance = aTolerance / 100
        Else 'assume tolerance passed as fraction
            lTolerance = aTolerance
        End If

        CopyBaseAttributes(aDyTSer, lDisTs)
        lDisTs.Attributes.SetValue("tu", atcTimeUnit.TUHour)
        lDisTs.Attributes.SetValue("ts", 1)
        lDisTs.Attributes.SetValue("Scenario", "COMPUTED")
        lDisTs.Attributes.SetValue("Constituent", "PREC")
        lDisTs.Attributes.SetValue("TSTYPE", "PREC")
        lDisTs.Attributes.SetValue("Description", "Hourly Precipitation disaggregated from Daily")
        lDisTs.Attributes.AddHistory("Disaggregated Precipitation - inputs: DPRC, HPCP, Observation Hour, Data Tolerance")
        lDisTs.Attributes.Add("DPRC", aDyTSer.ToString)
        lDisTs.Attributes.Add("HPCP", aHrTSer.ToString)
        lDisTs.Attributes.Add("Observation Timeseries", aObsTimeTS.ToString)
        lDisTs.Attributes.Add("Data Tolerance", aTolerance)

        'build new date array for hourly TSer, set start date to previous day's Obs Time
        lSDt = aDyTSer.Attributes.GetValue("SJDAY") - 1
        Call J2Date(lSDt, lDate)
        lDate(3) = CurrentObsTime(aObsTimeTS, 1)
        'aDyTSer.Attributes.SetValue("SJDAY", Date2J(lDate))
        'need to set first date value to shift back to previous day's Obs Time
        aDyTSer.Dates.Values(0) = Date2J(lDate)
        lDisTs.Dates = DisaggDates(aDyTSer, aDataSource)
        lDisTs.numValues = lDisTs.Dates.numValues

        'set initial start date, back up one day
        lEDt = aDyTSer.Dates.Value(1) - 1
        Call J2Date(lEDt, lDate)
        'now set hour value to initial Obs Time
        lDate(3) = CurrentObsTime(aObsTimeTS, 1)
        lEDt = Date2J(lDate)
        lSDt = lEDt - 1

        Dim lHrVals(lDisTs.numValues) As Double
        lHrPos = 0
        For lDyInd = 1 To aDyTSer.numValues
            If lOutSumm Then 'output summary message to file
                Call J2Date(aDyTSer.Dates.Value(lDyInd) - 1, lDate)
                WriteLine(lOutFil, "Distributing Daily Data for " & lDate(0) & "/" & lDate(1) & "/" & lDate(2) & ":  Value is " & SignificantDigits(aDyTSer.Value(lDyInd), 4))
            End If
            'determine end date, start by backing up to previous day's end
            lEDt = aDyTSer.Dates.Value(lDyInd) - 1
            Call J2Date(lEDt, lDate)
            'now add obs hour to get actual end of 24-hour period
            lDate(3) = CurrentObsTime(aObsTimeTS, lDyInd)
            lEDt = Date2J(lDate)
            lNVals = Math.Round(24 * (lEDt - lSDt))
            If aDyTSer.Value(lDyInd) > 0 Then 'something to disaggregate
                lClosestRatio = 0
                For Each lHrTSer As atcTimeseries In aHrTSer
                    lDaySumHrTser = SubsetByDate(lHrTSer, lSDt, lEDt, Nothing)
                    lDaySum = 0
                    For lHrInd = 1 To lDaySumHrTser.numValues
                        lDaySum = lDaySum + lDaySumHrTser.Value(lHrInd)
                    Next lHrInd
                    If lDaySum > 0 Then
                        lRatio = aDyTSer.Value(lDyInd) / lDaySum
                        If lRatio > 1 Then lRatio = 1 / lRatio
                        If lRatio > lClosestRatio Then
                            lClosestRatio = lRatio
                            lClosestHrTser = lDaySumHrTser
                            lClosestDaySum = lDaySum
                        End If
                    End If
                Next
                If lClosestRatio >= 1 - lTolerance Then 'hourly data found to do disaggregation
                    lRatio = aDyTSer.Value(lDyInd) / lClosestDaySum
                    lMaxHrVal = 0
                    lDaySum = 0
                    lCarry = 0
                    For lHrInd = 1 To lClosestHrTser.numValues
                        i = lHrPos + lHrInd
                        lHrVals(i) = lRatio * lClosestHrTser.Value(lHrInd) + lCarry
                        If lHrVals(i) > 0.00001 Then
                            lCarry = lHrVals(i) - (Math.Round(lHrVals(i) / lRndOff) * lRndOff)
                            lHrVals(i) = lHrVals(i) - lCarry
                        Else
                            lHrVals(i) = 0.0#
                        End If
                        If lHrVals(i) > lMaxHrVal Then
                            lMaxHrVal = lHrVals(i)
                            lMaxHrInd = i
                        End If
                        lDaySum = lDaySum + lHrVals(i)
                    Next lHrInd
                    If lCarry > 0 Then 'add remainder to max hourly value
                        lDaySum = lDaySum - lHrVals(lMaxHrInd)
                        lHrVals(lMaxHrInd) = lHrVals(lMaxHrInd) + lCarry
                        lDaySum = lDaySum + lHrVals(lMaxHrInd)
                    End If
                    If lOutSumm Then
                        WriteLine(lOutFil, "  Using Data-set:  " & lClosestHrTser.ToString & ", daily sum = " & SignificantDigits(lClosestDaySum, 4))
                    End If
                    If Math.Abs(lDaySum - aDyTSer.Value(lDyInd)) > lRndOff Then
                        'values not distributed properly
                        s = "PROBLEM distributing " & aDyTSer.Value(lDyInd) & " on " & lDate(1) & "/" & lDate(2) & "/" & lDate(0) & vbCrLf & _
                            "Daily value: " & aDyTSer.Value(lDyInd) & vbCrLf & _
                            "Hourly sum:  " & lDaySum
                        If lOutSumm Then
                            WriteLine(lOutFil, s)
                        End If
                        Logger.Dbg(s)
                        'rsp = MsgBox(s, MsgBoxStyle.Exclamation + MsgBoxStyle.OkCancel, "Precipitation Disaggregation Problem")
                        'If rsp = MsgBoxResult.Cancel Then
                        '    'lDisTs.errordescription = s
                        '    Err.Raise(vbObjectError + 513)
                        'End If
                    End If
                Else 'no data available at hourly stations,
                    'distribute using triangular distribution
                    Dim lTmpHrVals(24) As Double
                    Call DistTriang(aDyTSer.Value(lDyInd), lTmpHrVals, retcod)
                    lTriDistCnt += 1
                    Select Case aDyTSer.Value(lDyInd)
                        Case Is < 1 : lTriDist0To1Cnt += 1
                        Case Is < 2 : lTriDist1To2Cnt += 1
                        Case Is < 3 : lTriDist2To3Cnt += 1
                        Case Else : lTriDistGT3Cnt += 1
                    End Select
                    If lNVals < 24 Then 'obs time moved to earlier in day, don't have full day to distribute values
                        Dim lNumNonZero As Integer = 0
                        For i = 1 To 24
                            If lTmpHrVals(i) > 0 Then lNumNonZero += 1
                        Next
                        If lNumNonZero > lNVals Then 'can't fit disaggregated values in available space
                            s = "PROBLEM - Unable to fit distributed values in available hours due to change in Obs Time"
                            retcod = -3
                        Else
                            Dim lSPos As Integer = Math.Truncate((24 - lNVals) / 2)
                            For i = 1 To lNVals
                                lHrVals(lHrPos + i) = lTmpHrVals(lSPos + i)
                            Next
                        End If
                    Else
                        Dim lNGap As Integer = lNVals - 24
                        If lNGap > 0 Then 'obs time moved to later in day, fill "gap" with 0
                            For i = 1 To lNGap
                                lHrVals(lHrPos + i) = 0
                            Next
                        End If
                        'now fill final 24 hours with triangular distributed values
                        For lHrInd = 1 To 24
                            lHrVals(lHrPos + lNGap + lHrInd) = lTmpHrVals(lHrInd)
                        Next lHrInd
                    End If
                    If retcod = -1 Then
                        s = "PROBLEM - Unable to distribute this much rain (" & lDaySum & ") using triangular distribution." & "Hourly values will be set to -9.98"
                        Logger.Dbg(s)
                        'rsp = MsgBox(s, MsgBoxStyle.Exclamation + MsgBoxStyle.OkCancel, "Precipitation Disaggregation Problem")
                    ElseIf retcod = -2 Then
                        s = "PROBLEM distributing " & aDyTSer.Value(lDyInd) & " using triangular distribution on " & lDate(1) & "/" & lDate(2) & "/" & lDate(0)
                        Logger.Dbg(s)
                        'rsp = MsgBox(s, MsgBoxStyle.Exclamation + MsgBoxStyle.OkCancel, "Precipitation Disaggregation Problem")
                    End If
                    If lOutSumm Then
                        WriteLine(lOutFil, "  *** No hourly total within tolerance - " & SignificantDigits(aDyTSer.Value(lDyInd), 4) & "  distributed using triangular distribution ***")
                        If retcod <> 0 Then
                            WriteLine(lOutFil, "  *** " & s & " ***")
                        End If
                    End If
                    'If rsp = MsgBoxResult.Cancel Then
                    '    'lDisTs.errordescription = s
                    '    Err.Raise(vbObjectError + 513 + System.Math.Abs(retcod))
                    'End If
                End If
                lDistCnt += 1
            Else 'no daily value to distribute, fill hourly
                For lHrInd = lHrPos + 1 To lHrPos + lNVals '24
                    lHrVals(lHrInd) = 0
                Next lHrInd
            End If
            lHrPos = lHrPos + lNVals '24
            lSDt = lEDt 'set next periods start date to end date of this period
        Next lDyInd

DisPrecipErrHnd:
        On Error GoTo OuttaHere 'in case there's an error in these statements
        Array.Copy(lHrVals, 1, lDisTs.Values, 1, lDisTs.numValues)
        If lOutSumm Then
            WriteLine(lOutFil, "")
            WriteLine(lOutFil, "  Total Number of Values Distributed: " & lDistCnt)
            WriteLine(lOutFil, "  Number of Triangular Distributed Values : " & lTriDistCnt)
            WriteLine(lOutFil, "  Percentage of Triangular Distributed Values: " & lTriDistCnt / lDistCnt * 100)
            WriteLine(lOutFil, "    Breakdown of Triangular Distributions:")
            WriteLine(lOutFil, "      Number used for < 1 inch: " & lTriDist0To1Cnt)
            WriteLine(lOutFil, "      Number used for 1 to 2 inches: " & lTriDist1To2Cnt)
            WriteLine(lOutFil, "      Number used for 2 to 3 inches: " & lTriDist2To3Cnt)
            WriteLine(lOutFil, "      Number used for > 3 inches: " & lTriDistGT3Cnt)
            WriteLine(lOutFil, "")
            WriteLine(lOutFil, "  QA Checks")
            WriteLine(lOutFil, "    Average Annual")
            WriteLine(lOutFil, "      Original Daily:  " & SignificantDigits(aDyTSer.Attributes.GetValue("SumAnnual"), 4))
            WriteLine(lOutFil, "      Disagged Hourly: " & SignificantDigits(lDisTs.Attributes.GetValue("SumAnnual"), 4))
            WriteLine(lOutFil, "    Total Sum ")
            WriteLine(lOutFil, "      Original Daily:  " & SignificantDigits(aDyTSer.Attributes.GetValue("Sum"), 6))
            WriteLine(lOutFil, "      Disagged Hourly: " & SignificantDigits(lDisTs.Attributes.GetValue("Sum"), 6))
            FileClose(lOutFil)
        End If
        If aDyTSer.Attributes.GetValue("SumAnnual") - lDisTs.Attributes.GetValue("SumAnnual") > 0.1 OrElse _
           aDyTSer.Attributes.GetValue("Sum") - lDisTs.Attributes.GetValue("Sum") > 1 Then
            'significant difference between original and disaggregated timeseries
            Logger.Dbg("PROBLEM: Average Annual or Total Sum values don't match between original daily and disaggregated hourly timeseries")
            Logger.Dbg("         Average Annual - Original Daily: " & SignificantDigits(aDyTSer.Attributes.GetValue("SumAnnual"), 4) & "   Disagged Hourly: " & SignificantDigits(lDisTs.Attributes.GetValue("SumAnnual"), 4))
            Logger.Dbg("         Total Sum      - Original Daily: " & SignificantDigits(aDyTSer.Attributes.GetValue("Sum"), 6) & "   Disagged Hourly: " & SignificantDigits(lDisTs.Attributes.GetValue("Sum"), 6))
        End If

OuttaHere:
        If lUsedTriang > 0 Then
            'inform calling routine that automatic triangular distribution was used
            s = "WARNING:  Automatic Triangular Distribution was used " & lUsedTriang & " times." & vbCrLf
            If lOutSumm Then
                s = s & "Check summary output file (" & aSumFile & ") for details of when Triangular Distribution was used"
            End If
            'lDisTs.errordescription = s
        End If
        Return lDisTs

    End Function

    Private Sub DistTriang(ByVal aDaySum As Double, ByRef aHrVals() As Double, ByRef aRetCod As Integer)
        'Distribute a daily value to 24 hourly values using a triangular distribution
        'DaySum - daily value
        'HrVals - array of hourly values
        'Retcod - 0 - OK, -1 - DaySum too big,
        '        -2 - sum of hourly values does not match daily value (likely a round off problem)

        Dim i, j As Integer
        Dim lRndOff, lRatio, lCarry, lDaySum As Single

        aRetCod = 0
        i = 1
        Do While aDaySum > Sums(i)
            i = i + 1
            If i > 12 Then
                aRetCod = -1
                Exit Sub
            End If
        Loop

        lRndOff = 0.001
        lCarry = 0
        lRatio = aDaySum / Sums(i)
        lDaySum = 0
        For j = 1 To 24
            aHrVals(j) = lRatio * Triang(j, i) + lCarry
            If aHrVals(j) > 0.00001 Then
                lCarry = aHrVals(j) - (Math.Round(aHrVals(j) / lRndOff) * lRndOff)
                aHrVals(j) = aHrVals(j) - lCarry
            Else
                aHrVals(j) = 0.0#
            End If
            lDaySum = lDaySum + aHrVals(j)
        Next j
        If lCarry > 0.00001 Then
            lDaySum = lDaySum - aHrVals(12)
            aHrVals(12) = aHrVals(12) + lCarry
            lDaySum = lDaySum + aHrVals(12)
        End If
        If Math.Abs(aDaySum - lDaySum) > lRndOff Then
            'values not distributed properly
            aRetCod = -2
        End If
        If aRetCod <> 0 Then 'set to accumulated, with daily value at end
            For i = 1 To 23
                aHrVals(i) = -9.98
            Next i
            aHrVals(24) = aDaySum
        End If

    End Sub


    Private Function DisaggDates(ByVal aInTS As atcTimeseries, ByVal aDataSource As atcTimeseriesSource) As atcTimeseries
        'build new date timeseries class for hourly TSer based on daily TSer (aInTS)

        Dim lDates As New atcTimeseries(aDataSource)
        'lDates.numValues = aInTS.numValues * 24
        'lDates.ValuesNeedToBeRead = True
        lDates.Values = NewDates(aInTS, atcTimeUnit.TUHour, 1)
        Return lDates

        ''NOTE: Only valid for constant interval timeseries
        'Dim lPoint As Boolean = aInTS.Attributes.GetValue("point", False)

        'If lPoint Then
        '    'lDates.Value(ip) = GetNaN
        '    Return Nothing
        'Else
        '    Dim lJDay As Double
        '    Dim lDates As New atcTimeseries(aDataSource)
        '    lDates.numValues = aInTS.numValues * 24
        '    lJDay = aInTS.Attributes.GetValue("SJDAY")
        '    Dim ip As Integer = 0
        '    lDates.Value(ip) = lJDay
        '    'lJDay += lHrInc
        '    For i As Integer = 0 To aInTS.numValues - 1
        '        For j As Integer = 1 To 24
        '            ip += 1
        '            lDates.Value(ip) = aInTS.Dates.Value(i) + j * JulianHour
        '        Next
        '    Next
        '    Return lDates
        'End If
        ''For i As Integer = 1 To lDates.numValues
        ''    lDates.Value(i) = lJDay
        ''    lJDay += lHrInc
        ''Next i
    End Function

    Private Triang(,) As Double = { _
  {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}, _
  {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}, _
  {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}, _
  {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}, _
  {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}, _
  {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}, _
  {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}, _
  {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0.01, 0.01}, _
  {0, 0, 0, 0, 0, 0, 0, 0, 0, 0.01, 0.01, 0.1, 0.11}, _
  {0, 0, 0, 0, 0, 0, 0, 0.01, 0.01, 0.08, 0.09, 0.45, 0.55}, _
  {0, 0, 0, 0, 0, 0.01, 0.01, 0.06, 0.07, 0.28, 0.36, 1.2, 1.65}, _
  {0, 0, 0, 0.01, 0.01, 0.04, 0.05, 0.15, 0.21, 0.56, 0.84, 2.1, 3.3}, _
  {0, 0.01, 0.01, 0.02, 0.03, 0.06, 0.1, 0.2, 0.35, 0.7, 1.26, 2.52, 4.62}, _
  {0, 0, 0.01, 0.01, 0.03, 0.04, 0.1, 0.15, 0.35, 0.56, 1.26, 2.1, 4.62}, _
  {0, 0, 0, 0, 0.01, 0.01, 0.05, 0.06, 0.21, 0.28, 0.84, 1.2, 3.3}, _
  {0, 0, 0, 0, 0, 0, 0.01, 0.01, 0.07, 0.08, 0.36, 0.45, 1.65}, _
  {0, 0, 0, 0, 0, 0, 0, 0, 0.01, 0.01, 0.09, 0.1, 0.55}, _
  {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0.01, 0.01, 0.11}, _
  {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0.01}, _
  {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}, _
  {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}, _
  {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}, _
  {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}, _
  {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}, _
  {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}}
    Private Sums() As Double = {0, 0.01, 0.02, 0.04, 0.08, 0.16, 0.32, 0.64, 1.28, 2.56, 5.12, 10.24, 20.48}
End Module
