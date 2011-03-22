Imports atcData
Imports atcUtility
Imports System.Collections.Generic

''' <summary>Computes PenmanMonteith PET based on code from SWAT 2005</summary>
''' <remarks></remarks>
Public Module modPenmanMonteith_SWAT

    Dim WetDryDayProbabilityDistribution As UniformDistribution 'SWAT rndseed(1:hru)
    Dim SolarRadiationDistribution As UniformDistribution 'SWAT rndseed(2:hru)
    Dim PrecipitationDistribution As UniformDistribution 'SWAT rndseed(3:hru)
    Dim UsleRainfallErosionIndexDistribution As UniformDistribution 'SWAT rndseed(4:hru)
    Dim WindSpeedDistribution As UniformDistribution 'SWAT rndseed(5:hru)
    Dim HalfHourRainfallFractionDistribution As UniformDistribution 'SWAT rndseed(6:hru)
    Dim RelativeHumidityDistribution As TriangularDistribution  'SWAT rndseed(7:hru)
    Dim MaxAirTemperatureDistribution As UniformDistribution 'SWAT rndseed(8:hru)
    Dim MinAirTemperatureDistribution As UniformDistribution 'SWAT rndseed(9:hru)
    Dim SubDailyPrecipitationDistribution As UniformDistribution 'SWAT rndseed(10:hru)

    Sub InitRandomNumbers(Optional ByVal aUseDefaults As Boolean = True)
        If Not aUseDefaults Then Throw New Exception("Only know how to use default random numbers!")
        WetDryDayProbabilityDistribution = New UniformDistribution(748932582)
        SolarRadiationDistribution = New UniformDistribution(1985072130)
        PrecipitationDistribution = New UniformDistribution(1631331038)
        UsleRainfallErosionIndexDistribution = New UniformDistribution(67377721)
        WindSpeedDistribution = New UniformDistribution(366304404)
        HalfHourRainfallFractionDistribution = New UniformDistribution(1094585182)
        RelativeHumidityDistribution = New TriangularDistribution(1767585417)
        MaxAirTemperatureDistribution = New UniformDistribution(1980520317)
        MinAirTemperatureDistribution = New UniformDistribution(392682216)
    End Sub

    ''' <summary></summary>
    ''' <param name="aElevation"></param>
    ''' <param name="aPrecipitationTS"></param>
    ''' <param name="aAirTemperatureTS"></param>
    ''' <param name="aSource"></param>
    ''' <param name="aSwatWeatherStation"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function PanEvaporationTimeseriesComputedByPenmanMonteith(ByVal aElevation As Double, ByVal aPrecipitationTS As atcTimeseries, ByVal aAirTemperatureTS As atcTimeseries, _
                                                                     ByVal aSource As atcTimeseriesSource, ByVal aSwatWeatherStation As SwatWeatherStation, _
                                                                     Optional ByVal aCanopyFactor As Double = 49.0, _
                                                                     Optional ByVal aCo2Conc As Double = 0.0, _
                                                                     Optional ByVal aDaily As Boolean = True, _
                                                                     Optional ByVal aDebug As Boolean = False) As atcTimeseries
        InitRandomNumbers()

        Dim lPrecipitationTS As atcTimeseries
        Dim lAirTemperatureMinTS As atcTimeseries
        Dim lAirTemperatureMaxTS As atcTimeseries

        If aDaily Then
            lPrecipitationTS = Aggregate(aPrecipitationTS, atcTimeUnit.TUDay, 1, atcTran.TranSumDiv)
            lAirTemperatureMinTS = Aggregate(aAirTemperatureTS, atcTimeUnit.TUDay, 1, atcTran.TranMin)
            lAirTemperatureMaxTS = Aggregate(aAirTemperatureTS, atcTimeUnit.TUDay, 1, atcTran.TranMax)
        Else
            lPrecipitationTS = aPrecipitationTS
            lAirTemperatureMinTS = aAirTemperatureTS
            lAirTemperatureMaxTS = aAirTemperatureTS
        End If
        If CommonDates(New atcTimeseriesGroup(lPrecipitationTS, lAirTemperatureMaxTS, lAirTemperatureMinTS), 0, 0, lCommonStart, lCommonEnd) Then
            lPrecipitationTS = SubsetByDate(lPrecipitationTS, lCommonStart, lCommonEnd, Nothing)
            lAirTemperatureMinTS = SubsetByDate(lAirTemperatureMinTS, lCommonStart, lCommonEnd, Nothing)
            lAirTemperatureMaxTS = SubsetByDate(lAirTemperatureMaxTS, lCommonStart, lCommonEnd, Nothing)

        If aDebug Then
            Dim lPrecipValue As Double = 0.0
            Dim lIndex As Integer = 1
            Dim lStr As String = "Precip "
            While lIndex < 100 'lPrecipValue < 0.05
                lPrecipValue = lPrecipitationTS.Value(lIndex)
                lStr &= DoubleToString(lPrecipValue, , "##.00") & ", "
                lIndex += 1
            End While
            MapWinUtility.Logger.Dbg("Count " & lIndex)
            MapWinUtility.Logger.Dbg(lStr)
            lStr = "ATempMin "
            For lIndex2 As Integer = 1 To lIndex
                lStr &= DoubleToString(lAirTemperatureMinTS.Value(lIndex2), , "###.00") & ", "
            Next
            MapWinUtility.Logger.Dbg(lStr)
            lStr = "ATempMax "
            For lIndex2 As Integer = 1 To lIndex
                lStr &= DoubleToString(lAirTemperatureMaxTS.Value(lIndex2), , "###.00") & ", "
            Next
            MapWinUtility.Logger.Dbg(lStr)
        End If

        'convert to SI units (degF -> degC and in -> mm)
        lPrecipitationTS = lPrecipitationTS * 25.4
        Dim lArgs As New atcDataAttributes
        lArgs.Add("timeseries", lAirTemperatureMaxTS)
        lAirTemperatureMaxTS = DoMath("ftoc", lArgs)
        lArgs.Clear()
        lArgs.Add("timeseries", lAirTemperatureMinTS)
        lAirTemperatureMinTS = DoMath("ftoc", lArgs)
        Dim lElevation As Double = aElevation * 0.3048

        Dim lPanEvapTimeSeries As New atcTimeseries(aSource)
        lPanEvapTimeSeries = lAirTemperatureMinTS.Clone
        With lPanEvapTimeSeries
            .Attributes.SetValue("Constituent", "PMET")
            .Attributes.SetValue("Location", aSwatWeatherStation)
            .Attributes.SetValue("ID", aAirTemperatureTS + 6)
            .Attributes.SetValue("TU", atcTimeUnit.TUDay)
            .Attributes.SetValue("description", "Daily SWAT PM ET mm")
            .Attributes.SetValue("interval", 1.0)
            .Attributes.SetValue("TSTYPE", "PMET")
            Dim lSJDate As Double = lAirTemperatureMinTS.Dates.Value(0)
            Dim lEJDate As Double = lAirTemperatureMinTS.Dates.Value(lAirTemperatureMaxTS.numValues)
            Dim lSJDText As String = lAirTemperatureMinTS.Attributes.GetFormattedValue("start date")
            .Attributes.SetValue("start date", lSJDText)
            Dim lEJDText As String = lAirTemperatureMinTS.Attributes.GetFormattedValue("end date")
            .Attributes.SetValue("end date", lEJDText)
            Dim lDate(5) As Integer
            J2Date(lSJDate, lDate)
            .Attributes.SetValue("TSBYR", lDate(0))
            .Dates.Values = NewDates(lSJDate, lEJDate, atcTimeUnit.TUDay, 1)
            .Value(0) = 0.0
            For lDateIndex As Integer = 1 To lAirTemperatureMinTS.numValues
                .Value(lDateIndex) = PanEvaporationValueComputedbyPenmanMonteith _
                                      (.Dates.Value(lDateIndex), lElevation, aSwatWeatherStation, _
                                       lAirTemperatureMinTS.Value(lDateIndex), lAirTemperatureMaxTS.Value(lDateIndex), lPrecipitationTS.Value(lDateIndex), _
                                       aCo2Conc, aCanopyFactor, aDebug)
            Next
        End With

        'convert to English units (mm -> in)
        lPanEvapTimeSeries = lPanEvapTimeSeries / 25.4

        Return lPanEvapTimeSeries
        Else
            Throw New ApplicationException("No common time period between arguments to PanEvaporationTimeseriesComputedByPenmanMonteith")
        End If
    End Function

    ''' <summary></summary>
    ''' <param name="aJDate"></param>
    ''' <param name="aElevation"></param>
    ''' <param name="aStation"></param>
    ''' <param name="aAirTemperatureMin"></param>
    ''' <param name="aAirtemperatureMax"></param>
    ''' <param name="aPrecipitation"></param>
    ''' <param name="aCo2Conc"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function PanEvaporationValueComputedbyPenmanMonteith(ByVal aJDate As Double, ByVal aElevation As Double, _
                                                         ByVal aStation As SwatWeatherStation, _
                                                         ByVal aAirTemperatureMin As Double, ByVal aAirtemperatureMax As Double, _
                                                         ByVal aPrecipitation As Double, ByVal aCo2Conc As Double, ByVal aCanopyFactor As Double, _
                                                         Optional ByVal aDebug As Boolean = False) As Double
        Dim lYr, lMo, lDy As Integer
        INVMJD(aJDate, lYr, lMo, lDy)

        If aJDate = 35882 Then
            Debug.Print("ProblemSpot")
        End If
        'clicon
        '  weatgn
        Dim lWgnCur(2) As Double
        WeatherParmGenerator(lWgnCur)
        '  clgen
        Dim lJulianDayWithinYear As Double = aJDate - Date2J(lYr, 1, 1)
        'equation 2.1.2 in SWAT manual
        Dim lSolarDeclination As Double = Math.Asin(0.4 * Math.Sin((lJulianDayWithinYear - 82.0) / 58.09)) '365/2pi = 58.09
        'relative distance of earth from sun - equation 2.1.1 in SWAT Manual
        Dim lRelativeEarthToSunDistance As Double = 1.0 + (0.033 * Math.Cos(lJulianDayWithinYear / 58.09))
        'daylength - equation 2.1.6 in SWAT manual
        Dim lCh As Double = -aStation.LatSinR * Math.Tan(lSolarDeclination) / aStation.LatCosR
        Dim lH As Double = 0.0
        If lCh >= -1.0 AndAlso lCh < 1.0 Then
            lH = Math.Acos(lCh)
        Else
            lH = 3.1416
        End If
        Dim lYS As Double = aStation.LatSinR * Math.Sin(lSolarDeclination)
        Dim lYC As Double = aStation.LatCosR * Math.Cos(lSolarDeclination)
        Dim lSolarRadiationMax As Double = 30 * lRelativeEarthToSunDistance * ((lH * lYS) + (lYC * Math.Sin(lH)))

        '  slrgen
        '  rhgen
        '  wndgen
        Dim lWindAt10m As Double = Wind(aStation.WindAv(lMo), WindSpeedDistribution)

        Dim lAirTemperatureAve As Double = (aAirtemperatureMax + aAirTemperatureMin) / 2

        'temperature lapse rate as default value of -6 deg C/km (SWAT2005 reads this as a variable from input)
        Dim lLapseRate As Double = -6.0
        '  elevation adjustments
        Dim lAirTemperatureDiff As Double = (aElevation - aStation.Elev) * lLapseRate / 1000.0
        lAirTemperatureAve += lAirTemperatureDiff

        'etPot (mm)
        'tk
        Dim lAirTemperatureK As Double = lAirTemperatureAve + 273.15
        'pb
        Dim lMeanBarometricPressure As Double = 101.3 - (aElevation * (0.01152 - (0.000000544 * aElevation)))
        'x1
        Dim lLatentHeatOfVaproization As Double = 2.501 - (0.002361 * lAirTemperatureAve)
        'gma
        Dim lPsychrometricConstant As Double = 0.001013 * lMeanBarometricPressure / (0.622 * lLatentHeatOfVaproization)
        'ea
        Dim lSaturationVaporPressure As Double = SaturationVaporPressure(lAirTemperatureAve)
        'rhd
        Dim lRelativeHumidity As Double = RelativeHumidity(lAirTemperatureAve, aPrecipitation, _
                                                           aStation.AirTempMaxAv(lMo), aStation.AirTempMinAv(lMo), aStation.DewpointAv(lMo), _
                                                           aStation.ProportionWet(lMo), RelativeHumidityDistribution)
        'ed
        Dim lActualVaporPressure As Double = lSaturationVaporPressure * lRelativeHumidity

        'vpd
        Dim lVaporPressureDeficit As Double = lSaturationVaporPressure - lActualVaporPressure
        'dlt
        Dim lSlopeSaturationVaporPressureCurve As Double = 4098 * lSaturationVaporPressure / ((lAirTemperatureAve + 237.3) ^ 2)

        'hru_ra
        Dim lSolarRadiation As Double = SolarRadiation(aPrecipitation, aStation.ProportionWet(lMo), aStation.SolarAv(lMo), lSolarRadiationMax, lWgnCur(2))
        'ralb
        Dim lNetShortWaveRadiation As Double = lSolarRadiation * (1 - 0.23)
        'rbo SWAT equation 1.2.20
        Dim lNetEmissivity As Double = -(0.34 - (0.139 * Math.Sqrt(lActualVaporPressure)))
        'rto SWAT equation 1.2.19
        Dim lCloudCoverFactor As Double = 0.9 * (lSolarRadiation / lSolarRadiationMax) + 0.1
        'rout SWAT equation 1.2.21
        Dim lNetLongWaveRadiation As Double = lNetEmissivity * lCloudCoverFactor * 0.0000000049 * (lAirTemperatureK ^ 4)
        'rn_pet
        Dim lNetRadiation As Double = lNetShortWaveRadiation + lNetLongWaveRadiation
        'rho
        Dim lRho As Double = 1710 - (6.85 * lAirTemperatureAve)
        'u10
        If lWindAt10m < 0.01 Then lWindAt10m = 0.01
        'wind at 1.7m
        Dim lWindAt170cm As Double = lWindAt10m * ((170.0 / 1000.0) ^ 0.2)
        'rv - aerodynamic resistance to sensible heat and vapor transfer
        Dim lResistance As Double = 114 / lWindAt170cm
        'rc - canopy resistance (TODO:check details of canopy factor)
        Dim lCanopyResistance As Double = aCanopyFactor / (1.4 - (0.4 * aCo2Conc) / 330.0)

        Dim lEvaporationValue As Double = ((lSlopeSaturationVaporPressureCurve * lNetRadiation) + _
                                           (lPsychrometricConstant * lRho * lVaporPressureDeficit / lResistance)) / _
                                          (lLatentHeatOfVaproization * (lSlopeSaturationVaporPressureCurve + (lPsychrometricConstant * (1.0 + (lCanopyResistance / lResistance)))))
        If aDebug Then
            Dim lFormat As String = "0.0000"
            MapWinUtility.Logger.Dbg("Debug " & aJDate & vbTab & lJulianDayWithinYear & vbTab & _
                                     "a:" & DoubleToString(lSolarDeclination, , lFormat) & vbTab & _
                                     "b:" & DoubleToString(lRelativeEarthToSunDistance, , lFormat) & vbTab & _
                                     "c:" & DoubleToString(lSolarRadiationMax, , lFormat) & vbTab & _
                                     "d:" & DoubleToString(lWindAt10m, , lFormat) & vbTab & _
                                     "e:" & DoubleToString(lAirTemperatureAve, , lFormat) & vbTab & _
                                     "f:" & DoubleToString(lAirTemperatureDiff, , lFormat) & vbTab & _
                                     "g:" & DoubleToString(lAirTemperatureAve, , lFormat) & vbTab & _
                                     "h:" & DoubleToString(lMeanBarometricPressure, , lFormat) & vbTab & _
                                     "i:" & DoubleToString(lLatentHeatOfVaproization, , lFormat) & vbTab & _
                                     "j:" & DoubleToString(lPsychrometricConstant, , lFormat) & vbTab & _
                                     "k:" & DoubleToString(lSaturationVaporPressure, , lFormat) & vbTab & _
                                     "l:" & DoubleToString(lRelativeHumidity, , lFormat) & vbTab & _
                                     "m:" & DoubleToString(lActualVaporPressure, , lFormat) & vbTab & _
                                     "n:" & DoubleToString(lVaporPressureDeficit, , lFormat) & vbTab & _
                                     "o:" & DoubleToString(lSlopeSaturationVaporPressureCurve, , lFormat) & vbTab & _
                                     "p:" & DoubleToString(lSolarRadiation, , lFormat) & vbTab & _
                                     "q:" & DoubleToString(lNetShortWaveRadiation, , lFormat) & vbTab & _
                                     "r:" & DoubleToString(lNetEmissivity, , lFormat) & vbTab & _
                                     "s:" & DoubleToString(lCloudCoverFactor, , lFormat) & vbTab & _
                                     "t:" & DoubleToString(lNetLongWaveRadiation, , lFormat) & vbTab & _
                                     "u:" & DoubleToString(lNetRadiation, , lFormat) & vbTab & _
                                     "v:" & DoubleToString(lRho, , lFormat) & vbTab & _
                                     "w:" & DoubleToString(lWindAt10m, , lFormat) & vbTab & _
                                     "x:" & DoubleToString(lWindAt170cm, , lFormat) & vbTab & _
                                     "y:" & DoubleToString(lResistance, , lFormat) & vbTab & _
                                     "z:" & DoubleToString(lCanopyResistance, , lFormat) & vbTab & _
                                     "1:" & DoubleToString(lEvaporationValue, , lFormat))
        End If
        Return Math.Max(0.0, lEvaporationValue)
    End Function

    Sub WeatherParmGenerator(ByRef aWgnCur() As Double)
        Static lA(,) As Double = New Double(2, 2) {{0.567, 0.253, -0.006}, {0.086, 0.504, -0.039}, {-0.002, -0.05, 0.244}}
        Static lB(,) As Double = New Double(2, 2) {{0.781, 0.328, 0.238}, {0.0, 0.637, -0.341}, {0.0, 0.0, 0.873}}
        Static lWgnOld() As Double = New Double(2) {0.0, 0.0, 0.0}

        Dim lE(2) As Single
        Dim lV1 As Single = MaxAirTemperatureDistribution.PreviousValue
        Dim lV2 As Single = MaxAirTemperatureDistribution.Generate
        lE(0) = Dstn(lV1, lV2)
        lV1 = MinAirTemperatureDistribution.PreviousValue
        lV2 = MinAirTemperatureDistribution.Generate
        lE(1) = Dstn(lV1, lV2)
        lV1 = SolarRadiationDistribution.PreviousValue
        lV2 = SolarRadiationDistribution.Generate
        lE(2) = Dstn(lV1, lV2)
        Dim lXX(2) As Single
        For lN As Integer = 0 To 2
            aWgnCur(lN) = 0.0
            For lL As Integer = 0 To 2
                aWgnCur(lN) += (lB(lL, lN) * lE(lL))
                lXX(lN) += (lA(lL, lN) * lWgnOld(lL))
            Next
        Next
        For lN As Integer = 0 To 2
            aWgnCur(lN) += lXX(lN)
            lWgnOld(lN) = aWgnCur(lN)
        Next
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="aV1"></param>
    ''' <param name="av2"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function Dstn(ByVal aV1 As Single, ByVal av2 As Single) As Single
        Dim lDstn As Single = Math.Sqrt(-2.0 * Math.Log(aV1)) * Math.Cos(6.283185 * av2)
        Return lDstn
    End Function
End Module
