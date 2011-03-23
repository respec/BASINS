Imports System.Collections.Specialized
Imports MapWindow.Interfaces
Imports MapWinUtility
Imports atcMetCmp
Imports atcData
Imports atcUtility

Module MetCmp_PenmanMonteith
    Dim pWdmDataPath As String = "G:\Data\BasinsMet\WDMFinal\"
    Dim pDebug As Boolean = False
    Dim pSingleStation As Boolean = False
    Dim pDateSubset As Boolean = False
    Dim pSJDate As Double = Date2J(2000, 1, 1)
    Dim pEJDate As Double = Date2J(2001, 1, 1)

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
            Dim lFileName As String = pWdmDataPath & "AK502316.wdm" '"NY303184.wdm" '"GA090451" Hartsfield
            lFileNames.Add(lFileName.ToLower, lFileName)
        Else
            AddFilesInDir(lFileNames, pWdmDataPath, False, "*.wdm")
        End If
        Logger.Dbg("MetCmp_PenmanMonteith: Found " & lFileNames.Count & " WDM data files")

        AppendFileString(pWdmDataPath & "Diff\Summary.txt", _
                         "Station".PadRight(8) & vbTab & "SumAnnDif".PadLeft(10) & vbTab & "SumAnnVB".PadLeft(10) & vbTab & "SumAnnFtn".PadLeft(10) & vbTab & "Percent".PadLeft(10) & vbCrLf)
        Dim lFormat As String = "##.0000"

        Dim lCount As Integer = 0
        For Each lFileName As String In lFileNames
            lCount += 1
            Dim lMetStation As String = IO.Path.GetFileNameWithoutExtension(lFileName)
            If Not pSingleStation AndAlso IO.File.Exists(pWdmDataPath & "PM_VB\" & lMetStation & ".txt") Then
                Logger.Dbg("Skip " & lMetStation)
            Else
                Logger.Dbg("Process " & lMetStation & "(" & lCount & " of " & lFileNames.Count & ")")
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
                            lPanEvapTimeseries.SetInterval(atcTimeUnit.TUDay, 1)

                            If pCompareToOld Then
                                'lPanEvapTimeseries.Attributes.CalculateAll()
                                Dim lSumAnnualVB As Double = lPanEvapTimeseries.Attributes.GetDefinedValue("SumAnnual").Value
                                SaveFileString(pWdmDataPath & "PM_VB\" & lMetStation & ".txt", lPanEvapTimeseries.Attributes.ToString)

                                If lMetStation = "az029287" OrElse lMetStation = "ia133584" OrElse lMetStation = "mo237435" OrElse lMetStation = "nd322695" Then
                                    Logger.Dbg("SkipFortranProblemStaion " & lMetStation)
                                Else
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
                                        If lPanEvapTimeseries.Values.GetUpperBound(0) < lNVals Then
                                            lNVals = lPanEvapTimeseries.Values.GetUpperBound(0)
                                        End If
                                        For lIndex As Integer = 0 To lNVals
                                            lPanEvapTimeseriesFromTTFortran.Value(lIndex) = lPMETValsSingle(lIndex)
                                        Next
                                        lPanEvapTimeseriesFromTTFortran.SetInterval(atcTimeUnit.TUDay, 1)
                                        'lPanEvapTimeseriesFromTTFortran.Attributes.CalculateAll()
                                        Dim lSumAnnualFtn As Double = lPanEvapTimeseriesFromTTFortran.Attributes.GetDefinedValue("SumAnnual").Value
                                        SaveFileString(pWdmDataPath & "PM_FTN\" & lMetStation & ".txt", lPanEvapTimeseriesFromTTFortran.Attributes.ToString)

                                        Dim lBothTimeseries As New atcTimeseriesGroup
                                        lBothTimeseries.Add(lPanEvapTimeseries)
                                        lBothTimeseries.Add(lPanEvapTimeseriesFromTTFortran)
                                        Dim lArgsMath As New atcDataAttributes
                                        lArgsMath.SetValue("timeseries", lBothTimeseries)
                                        Dim lTsMath As atcTimeseriesSource = New atcTimeseriesMath.atcTimeseriesMath
                                        If lTsMath.Open("subtract", lArgsMath) Then
                                            Dim lDiffTs As atcTimeseries = lTsMath.DataSets(0)
                                            'lDiffTs.Attributes.CalculateAll()
                                            Dim lSumAnnualDiff As Double = lDiffTs.Attributes.GetDefinedValue("SumAnnual").Value
                                            Dim lOutputFileName As String = pWdmDataPath & "Diff\" & lMetStation & ".txt"
                                            SaveFileString(lOutputFileName, lDiffTs.Attributes.ToString)
                                            If pSingleStation AndAlso pDateSubset Then
                                                Dim lSB As New Text.StringBuilder
                                                lSB.AppendLine("Index" & vbTab & "JDate".PadLeft(6) & vbTab & "PEV_VB".PadLeft(10) & vbTab & "PEV_FTN".PadLeft(10) & vbTab & _
                                                               "Diff".PadLeft(10) & vbTab & "Prec".PadLeft(10) & vbTab & "TMin".PadLeft(10) & vbTab & "TMax".PadLeft(10))
                                                For lIndex As Integer = 1 To lPanEvapTimeseriesFromTTFortran.numValues
                                                    lSB.AppendLine(lIndex.ToString.PadLeft(5) & vbTab & lPanEvapTimeseries.Dates.Value(lIndex).ToString.PadLeft(6) & vbTab & _
                                                                   DoubleToString(lPanEvapTimeseries.Value(lIndex), , lFormat, lFormat).PadLeft(10) & vbTab & _
                                                                   DoubleToString(lPanEvapTimeseriesFromTTFortran.Value(lIndex), , lFormat, lFormat).PadLeft(10) & vbTab & _
                                                                   DoubleToString(lDiffTs.Value(lIndex), , lFormat, lFormat).PadLeft(10) & vbTab & _
                                                                   DoubleToString(lPrecVals(lIndex), , lFormat, lFormat).PadLeft(10) & vbTab & _
                                                                   DoubleToString(lTMinVals(lIndex), , lFormat, lFormat).PadLeft(10) & vbTab & _
                                                                   DoubleToString(lTMaxVals(lIndex), , lFormat, lFormat).PadLeft(10) & vbTab)
                                                Next
                                                AppendFileString(lOutputFileName, lSB.ToString)
                                            End If

                                            Logger.Dbg(" MeanDiff " & lDiffTs.Attributes.GetDefinedValue("Mean").Value)
                                            Dim lStr As String = lMetStation & vbTab & _
                                                                 DoubleToString(lSumAnnualDiff, , lFormat, lFormat).PadLeft(10) & vbTab & _
                                                                 DoubleToString(lSumAnnualVB, , lFormat, lFormat).PadLeft(10) & vbTab & _
                                                                 DoubleToString(lSumAnnualFtn, , lFormat, lFormat).PadLeft(10) & vbTab & _
                                                                 DoubleToString((100 * lSumAnnualDiff / lSumAnnualFtn), , lFormat, lFormat).PadLeft(10) & vbCrLf
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
            End If
        Next
    End Sub
End Module
