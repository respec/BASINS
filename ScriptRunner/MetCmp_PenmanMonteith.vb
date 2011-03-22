Imports System.Collections.Specialized
Imports MapWindow.Interfaces
Imports MapWinUtility
Imports atcMetCmp
Imports atcData
Imports atcUtility

Module MetCmp_PenmanMonteith
    Dim pWdmDataPath As String = "G:\Data\BasinsMet\WDMFinal\"
    Dim pSingleStation As Boolean = True
    Dim pDateSubset As Boolean = True
    Dim pSJDate As Double = Date2J(1998, 1, 1)
    Dim pEJDate As Double = Date2J(1999, 1, 1)
    Dim pDebug As Boolean = True

    Dim pCompareToOld As Boolean = True
    Private Declare Sub PMPEVT Lib "tt_met.dll" (ByRef idmet As Integer, _
                                                ByRef istyrZ As Integer, _
                                                ByRef istdyZ As Integer, _
                                                ByRef nbyrZ As Integer, _
                                                ByRef sub_elevZ As Single, _
                                                ByRef sub_latZ As Single, _
                                                ByRef CO2Z As Single, _
                                                ByRef numdata As Integer, _
                                                ByRef PrecipZ As Single, _
                                                ByRef TmaxZ As Single, _
                                                ByRef TminZ As Single, _
                                                ByRef PevtPMZ As Single)

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        Logger.Dbg("MetCmp_PenmanMonteith Start")

        Dim lFileNames As New NameValueCollection
        If pSingleStation Then
            Dim lFileName As String = pWdmDataPath & "NY303184.wdm" '"GA090451" Hartsfield
            lFileNames.Add(lFileName.ToLower, lFileName)
        Else
            AddFilesInDir(lFileNames, pWdmDataPath, False, "*.wdm")
        End If
        Logger.Dbg("MetCmp_PenmanMonteith: Found " & lFileNames.Count & " WDM data files")

        For Each lFileName As String In lFileNames
            Dim lMetStation As String = IO.Path.GetFileNameWithoutExtension(lFileName)
            If Not pSingleStation AndAlso IO.File.Exists(pWdmDataPath & "PM_VB\" & lMetStation & ".txt") Then
                Logger.Dbg("Skip " & lMetStation)
            Else
                Logger.Dbg("Process " & lMetStation)
                Dim lWDMFile As atcWDM.atcDataSourceWDM = New atcWDM.atcDataSourceWDM
                If lWDMFile.Open(lFileName) AndAlso lWDMFile.DataSets.Count > 0 Then
                    Dim lPrecipitationTS As atcTimeseries = lWDMFile.DataSets.FindData("Constituent", "PREC", 1).Item(0)
                    Dim lAirTemperatureTS As atcTimeseries = lWDMFile.DataSets.FindData("Constituent", "ATEM", 1).Item(0)
                    If lPrecipitationTS Is Nothing Then
                        Logger.Dbg("Missing Precip ")
                    ElseIf lAirTemperatureTS Is Nothing Then
                        Logger.Dbg("Missing AirTemp")
                    Else 'Find common period for precip/temp
                        Dim lDate(5) As Integer
                        Dim lSJD As Double = Math.Max(lPrecipitationTS.Attributes.GetValue("SJDay"), _
                                                      lAirTemperatureTS.Attributes.GetValue("SJDay"))
                        If pDateSubset Then lSJD = pSJDate
                        J2Date(lSJD, lDate)
                        If lDate(1) > 1 OrElse lDate(2) > 1 Then 'start is not jan 1, figure out julian date for the year
                            lDate(0) += 1
                            lDate(1) = 1
                            lDate(2) = 1
                            lDate(3) = 0
                            lDate(4) = 0
                            lDate(5) = 0
                            lSJD = Date2J(lDate)
                        End If
                        Dim lEJD As Double = Math.Min(lPrecipitationTS.Attributes.GetValue("EJDay"), _
                                                      lAirTemperatureTS.Attributes.GetValue("EJDay"))
                        If pDateSubset Then lEJD = pEJDate
                        J2Date(lEJD, lDate)
                        If lDate(1) > 1 OrElse lDate(2) > 1 Then 'start is not jan 1, figure out julian date for the year
                            lDate(1) = 1
                            lDate(2) = 1
                            lDate(3) = 0
                            lDate(4) = 0
                            lDate(5) = 0
                            lEJD = Date2J(lDate)
                        End If
                        If lSJD < lEJD Then 'common period found
                            lPrecipitationTS = SubsetByDate(lPrecipitationTS, lSJD, lEJD, Nothing)
                            lAirTemperatureTS = SubsetByDate(lAirTemperatureTS, lSJD, lEJD, Nothing)
                            Dim lSwatWeatherStations As New SwatWeatherStations
                            Dim lLatitude As Double = lAirTemperatureTS.Attributes.GetDefinedValue("Latitude").Value
                            Dim lLongitude As Double = lAirTemperatureTS.Attributes.GetDefinedValue("Longitude").Value
                            Dim lNearestStations As SortedList(Of Double, PointLocation) = lSwatWeatherStations.Closest(lLatitude, lLongitude, 5)
                            Dim lNearestStation As SwatWeatherStation = lNearestStations.Values(0)

                            'TODO: get actual elevation of location rather than using station elveation
                            Dim lElevation As Double = lNearestStation.Elev / 0.3048
                            Dim lPanEvapTimeseries As atcTimeseries = _
                                PanEvaporationTimeseriesComputedByPenmanMonteith(lElevation, lPrecipitationTS, lAirTemperatureTS, Nothing, lNearestStation, , , , pDebug)

                            If pCompareToOld Then
                                lPanEvapTimeseries.Attributes.CalculateAll()
                                SaveFileString(pWdmDataPath & "PM_VB\" & lMetStation & ".txt", lPanEvapTimeseries.Attributes.ToString)
                                Dim lCo2 As Single = 0.0
                                Dim lDateStart(5) As Integer
                                J2Date(lPrecipitationTS.Dates.Values(0), lDateStart)
                                Dim lNumYears As Integer = timdifJ(lPrecipitationTS.Dates.Values(0), lPrecipitationTS.Dates.Values(lPrecipitationTS.numValues), atcTimeUnit.TUYear, 1)
                                Dim lPrecVals As Double() = Aggregate(lPrecipitationTS, atcTimeUnit.TUDay, 1, atcTran.TranSumDiv).Values
                                Dim lTMinVals As Double() = Aggregate(lAirTemperatureTS, atcTimeUnit.TUDay, 1, atcTran.TranMin).Values
                                Dim lTMaxVals As Double() = Aggregate(lAirTemperatureTS, atcTimeUnit.TUDay, 1, atcTran.TranMax).Values

                                Dim lNVals As Integer = lPrecVals.GetUpperBound(0)
                                Dim lPMETValsSingle(lNVals) As Single
                                Dim lPrecValsSingle(lNVals) As Single
                                Dim lTMinValsSingle(lNVals) As Single
                                Dim lTMaxValsSingle(lNVals) As Single
                                'Copy values into single array for calling fortran
                                For lIndex As Integer = 0 To lNVals
                                    lPrecValsSingle(lIndex) = lPrecVals(lIndex)
                                    lTMinValsSingle(lIndex) = lTMinVals(lIndex)
                                    lTMaxValsSingle(lIndex) = lTMaxVals(lIndex)
                                Next

                                SaveFileString("statwgn.txt", lNearestStation.Record)

                                Try
                                    PMPEVT(lNearestStation.Id, lDateStart(0), lDateStart(2), lNumYears, _
                                           lNearestStation.Elev, lLatitude, lCo2, lNVals, _
                                           lPrecValsSingle(1), lTMaxValsSingle(1), lTMinValsSingle(1), lPMETValsSingle(1))

                                    Dim lPanEvapTimeseriesFromTTFortran As atcTimeseries = lPanEvapTimeseries.Clone
                                    For lIndex As Integer = 0 To lNVals
                                        lPanEvapTimeseriesFromTTFortran.Values(lIndex) = lPMETValsSingle(lIndex)
                                    Next
                                    lPanEvapTimeseriesFromTTFortran.Attributes.CalculateAll()
                                    SaveFileString(pWdmDataPath & "PM_FTN\" & lMetStation & ".txt", lPanEvapTimeseriesFromTTFortran.Attributes.ToString)

                                    Dim lBothTimeseries As New atcTimeseriesGroup
                                    lBothTimeseries.Add(lPanEvapTimeseries)
                                    lBothTimeseries.Add(lPanEvapTimeseriesFromTTFortran)
                                    Dim lArgsMath As New atcDataAttributes
                                    lArgsMath.SetValue("timeseries", lBothTimeseries)
                                    Dim lTsMath As atcTimeseriesSource = New atcTimeseriesMath.atcTimeseriesMath
                                    If lTsMath.Open("subtract", lArgsMath) Then
                                        Dim lDiffTs As atcTimeseries = lTsMath.DataSets(0)
                                        lDiffTs.Attributes.CalculateAll()
                                        Dim lOutputFileName As String = pWdmDataPath & "Diff\" & lMetStation & ".txt"
                                        SaveFileString(lOutputFileName, lDiffTs.Attributes.ToString)
                                        If pSingleStation AndAlso pDateSubset Then
                                            Dim lSB As New Text.StringBuilder
                                            For lIndex As Integer = 0 To lPanEvapTimeseriesFromTTFortran.numValues
                                                lSB.AppendLine(lPanEvapTimeseries.Dates.Values(lIndex).ToString & vbTab & lPanEvapTimeseries.Values(lIndex) & vbTab & _
                                                               lPanEvapTimeseriesFromTTFortran.Values(lIndex) & vbTab & lDiffTs.Values(lIndex))
                                            Next
                                            AppendFileString(lOutputFileName, lSB.ToString)
                                        End If

                                        Logger.Dbg(" MeanDiff " & lDiffTs.Attributes.GetDefinedValue("Mean").Value)
                                        Dim lStr As String = lMetStation & " SumAnnual " & DoubleToString(lDiffTs.Attributes.GetDefinedValue("SumAnnual").Value, , "#.0000", "#.0000") & vbCrLf
                                        AppendFileString(pWdmDataPath & "Diff\Summary.txt", lStr)
                                    End If
                                Catch lEx As Exception
                                    Logger.Dbg("Exception " & lEx.ToString)
                                End Try
                            End If
                        End If
                    End If
                End If
            End If
        Next
    End Sub
End Module
