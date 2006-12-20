Option Strict Off
Option Explicit On
Imports atcData
Imports atcUtility
Imports MapWinUtility

Public Module modMetCompute
    'Copyright 2005 by AQUA TERRA Consultants
    Private X1() As Double = {0, 10.00028, 41.0003, 69.22113, 100.5259, 130.8852, 161.2853, _
                          191.7178, 222.1775, 253.66, 281.1629, 309.6838, 341.221}
    Private c(,) As Double = { _
      {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}, _
      {0, 4.0, 2.0, -1.5, -3.0, -2.0, 1.0, 3.0, 2.5, 1.0, 1.0, 2.0, 1.0}, _
      {0, 3.0, 4.0, 0.0, -3.0, -2.5, 0.0, 2.0, 3.0, 2.0, 1.5, 2.0, 1.0}, _
      {0, 0.0, 3.5, 1.5, -1.0, -2.0, -1.0, 1.5, 3.0, 3.0, 1.5, 2.0, 1.0}, _
      {0, -2.0, 2.5, 3.5, 0.0, -2.0, -1.0, 0.5, 3.0, 3.0, 2.0, 2.0, 1.0}, _
      {0, -4.0, 0.5, 3.0, 1.0, -0.5, -1.0, 0.0, 2.0, 2.5, 2.5, 2.0, 1.0}, _
      {0, -5.0, -1.5, 2.0, 3.0, 0.5, -1.0, -0.5, 1.0, 2.5, 2.5, 2.0, 1.0}, _
      {0, -5.0, -3.5, 1.0, 3.0, 1.5, 0.0, -0.5, 1.0, 2.0, 2.0, 2.0, 1.0}, _
      {0, -4.0, -4.5, -1.0, 2.5, 3.0, 1.0, 0.0, 0.0, 1.5, 2.0, 2.0, 1.0}, _
      {0, -2.0, -4.0, -3.0, 1.0, 3.0, 2.0, 0.5, 0.0, 1.5, 2.0, 1.0, 1.0}, _
      {0, 0.0, -3.5, -4.0, -0.5, 3.0, 3.0, 1.5, 1.0, 1.0, 2.0, 1.0, 1.0}}
    Private XLax(,) As Double = { _
      {-9, -9, -9, -9, -9, -9, -9}, {-9, -9, -9, -9, -9, -9, -9}, {-9, -9, -9, -9, -9, -9, -9}, _
      {-9, -9, -9, -9, -9, -9, -9}, {-9, -9, -9, -9, -9, -9, -9}, {-9, -9, -9, -9, -9, -9, -9}, _
      {-9, -9, -9, -9, -9, -9, -9}, {-9, -9, -9, -9, -9, -9, -9}, {-9, -9, -9, -9, -9, -9, -9}, _
      {-9, -9, -9, -9, -9, -9, -9}, {-9, -9, -9, -9, -9, -9, -9}, {-9, -9, -9, -9, -9, -9, -9}, _
      {-9, -9, -9, -9, -9, -9, -9}, {-9, -9, -9, -9, -9, -9, -9}, {-9, -9, -9, -9, -9, -9, -9}, _
      {-9, -9, -9, -9, -9, -9, -9}, {-9, -9, -9, -9, -9, -9, -9}, {-9, -9, -9, -9, -9, -9, -9}, _
      {-9, -9, -9, -9, -9, -9, -9}, {-9, -9, -9, -9, -9, -9, -9}, {-9, -9, -9, -9, -9, -9, -9}, _
      {-9, -9, -9, -9, -9, -9, -9}, {-9, -9, -9, -9, -9, -9, -9}, {-9, -9, -9, -9, -9, -9, -9}, _
      {-9, -9, -9, -9, -9, -9, -9}, _
      {-9, 616.17, -147.83, -27.17, -3.17, 11.84, 2.02}, _
      {-9, 609.97, -154.71, -27.49, -2.97, 12.04, 1.3}, _
      {-9, 603.69, -161.55, -27.69, -2.78, 12.22, 0.64}, _
      {-9, 597.29, -168.33, -27.78, -2.6, 12.38, 0.02}, _
      {-9, 590.81, -175.05, -27.74, -2.43, 12.53, -0.56}, _
      {-9, 584.21, -181.72, -27.57, -2.28, 12.67, -1.1}, _
      {-9, 577.53, -188.34, -27.29, -2.14, 12.8, -1.6}, _
      {-9, 570.73, -194.91, -26.89, -2.02, 12.92, -2.05}, _
      {-9, 563.85, -201.42, -26.37, -1.91, 13.03, -2.45}, _
      {-9, 556.85, -207.29, -25.72, -1.81, 13.13, -2.8}, _
      {-9, 549.77, -214.29, -24.96, -1.72, 13.22, -3.1}, _
      {-9, 542.57, -220.65, -24.07, -1.64, 13.3, -3.35}, _
      {-9, 535.3, -226.96, -23.07, -1.59, 13.36, -3.58}, _
      {-9, 527.9, -233.22, -21.95, -1.55, 13.4, -3.77}, _
      {-9, 520.44, -239.43, -20.7, -1.52, 13.42, -3.92}, _
      {-9, 512.84, -245.59, -19.33, -1.51, 13.42, -4.03}, _
      {-9, 505.19, -251.69, -17.83, -1.51, 13.41, -4.1}, _
      {-9, 497.4, -257.74, -16.22, -1.52, 13.39, -4.13}, _
      {-9, 489.52, -263.74, -14.49, -1.54, 13.36, -4.12}, _
      {-9, 481.53, -269.7, -12.63, -1.57, 13.32, -4.07}, _
      {-9, 473.45, -275.6, -10.65, -1.63, 13.27, -3.98}, _
      {-9, 465.27, -281.45, -8.55, -1.71, 13.21, -3.85}, _
      {-9, 456.99, -287.25, -6.33, -1.8, 13.14, -3.68}, _
      {-9, 448.61, -292.99, -3.98, -1.9, 13.07, -3.47}, _
      {-9, 440.14, -298.68, -1.51, -2.01, 13.0#, -3.3}, _
      {-9, 431.55, -304.32, 1.08, -2.13, 12.92, -3.17}, _
      {-9, 431.55, -304.32, 1.08, -2.13, 12.92, -3.17}}
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

    Public Function CmpSol(ByVal aCldTSer As atcTimeseries, ByVal aSource As atcDataSource, ByVal aLatDeg As Double) As atcTimeseries
        'compute daily solar radiation based on daily cloud cover
        Dim i As Integer
        Dim ldate(5) As Integer
        Dim SolRad(aCldTSer.numValues) As Double
        Dim CldCov(aCldTSer.numValues) As Double
        Dim lCmpTs As New atcTimeseries(aSource)
        Dim lPoint As Boolean = aCldTSer.Attributes.GetValue("point", False)

        CopyBaseAttributes(aCldTSer, lCmpTs)
        lCmpTs.Attributes.SetValue("Constituent", "DSOL")
        lCmpTs.Attributes.SetValue("TSTYPE", "DSOL")
        lCmpTs.Attributes.SetValue("Scenario", "COMPUTED")
        lCmpTs.Attributes.SetValue("Description", "Daily Solar Radiation (langleys) computed from Daily Cloud Cover")
        lCmpTs.Attributes.AddHistory("Computed Daily Solar Radiation - inputs: DCLD, Latitude")
        lCmpTs.Attributes.Add("DCLD", aCldTSer.ToString)
        lCmpTs.Attributes.Add("Latitude", aLatDeg)
        lCmpTs.Dates = aCldTSer.Dates
        lCmpTs.numValues = aCldTSer.numValues
        Array.Copy(aCldTSer.Values, 1, CldCov, 1, aCldTSer.numValues)

        For i = 1 To lCmpTs.numValues
            If Not Double.IsNaN(CldCov(i)) Then
                If CldCov(i) <= 0.0# Then CldCov(i) = 0.000001
                If lPoint Then
                    Call J2Date(aCldTSer.Dates.Value(i), ldate)
                Else
                    Call J2Date(aCldTSer.Dates.Value(i - 1), ldate)
                End If
                Call RadClc(aLatDeg, CldCov(i), ldate(1), ldate(2), SolRad(i))
            Else
                SolRad(i) = Double.NaN
            End If
        Next i
        Array.Copy(SolRad, 1, lCmpTs.Values, 1, lCmpTs.numValues)

        Return lCmpTs

    End Function

    Public Function CmpCldFromSolar(ByVal aDSolTSer As atcTimeseries, ByVal aSource As atcDataSource, ByVal aLatDeg As Double) As atcTimeseries
        'compute daily cloud cover based on daily solar radiation 
        Dim i As Integer
        Dim ldate(5) As Integer
        Dim SolRad(aDSolTSer.numValues) As Double
        Dim CldCov(aDSolTSer.numValues) As Double
        Dim lCmpTs As New atcTimeseries(aSource)
        Dim lPoint As Boolean = aDSolTSer.Attributes.GetValue("point", False)

        CopyBaseAttributes(aDSolTSer, lCmpTs)
        lCmpTs.Attributes.SetValue("Constituent", "CLDC")
        lCmpTs.Attributes.SetValue("TSTYPE", "CLDC")
        lCmpTs.Attributes.SetValue("Scenario", "COMPUTED")
        lCmpTs.Attributes.SetValue("Description", "Daily Cloud Cover (0-10) computed from Daily Solar Radiation (Langleys)")
        lCmpTs.Attributes.AddHistory("Computed Daily Cloud Cover - inputs: DSOL, Latitude")
        lCmpTs.Attributes.Add("DSOL", aDSolTSer.ToString)
        lCmpTs.Attributes.Add("Latitude", aLatDeg)
        lCmpTs.Dates = aDSolTSer.Dates
        lCmpTs.numValues = aDSolTSer.numValues
        Array.Copy(aDSolTSer.Values, 1, SolRad, 1, aDSolTSer.numValues)

        For i = 1 To lCmpTs.numValues
            If lPoint Then
                Call J2Date(aDSolTSer.Dates.Value(i), ldate)
            Else
                Call J2Date(aDSolTSer.Dates.Value(i - 1), ldate)
            End If
            Call CldClc(aLatDeg, SolRad(i), ldate(1), ldate(2), CldCov(i))
        Next i
        Array.Copy(CldCov, 1, lCmpTs.Values, 1, lCmpTs.numValues)

        Return lCmpTs

    End Function

    Public Function CmpJen(ByVal aTMinTS As atcTimeseries, ByVal aTMaxTS As atcTimeseries, ByVal aSRadTS As atcTimeseries, ByVal aSource As atcDataSource, ByVal aDegF As Boolean, ByVal aCTX As Double, ByVal aCTS() As Double) As atcTimeseries
        'compute JENSEN-HAISE - PET
        'aTMinTS/aTMaxTS - min/max temp timeseries
        'aSRadTS - solar radiation timeseries
        'aCTS    - monthly variable coefficients
        'aCTX    - constant coefficient
        'aDegF   - temperature in Fahrenheit (True) or Celsius (False)

        Dim i As Integer
        Dim lRetCod As Integer
        Dim lDate(5) As Integer
        Dim lAirTmp(aTMinTS.numValues) As Double
        Dim lPanEvp(aTMinTS.numValues) As Double
        Dim lCmpTs As New atcTimeseries(aSource)
        Dim tsfil(3) As Double
        Dim lPoint As Boolean = aTMinTS.Attributes.GetValue("point", False)

        CopyBaseAttributes(aTMinTS, lCmpTs)
        lCmpTs.Attributes.SetValue("Constituent", "PET")
        lCmpTs.Attributes.SetValue("TSTYPE", "EVAP")
        lCmpTs.Attributes.SetValue("Scenario", "COMPUTED")
        lCmpTs.Attributes.SetValue("Description", "Daily Potential ET (in) computed using Jensen algorithm")
        lCmpTs.Attributes.AddHistory("Daily Potential ET using Jensen - inputs: TMIN, TMAX, SRAD, Degrees F, Constant Coefficient, Monthly Coefficient")
        lCmpTs.Attributes.Add("TMIN", aTMinTS.ToString)
        lCmpTs.Attributes.Add("TMAX", aTMaxTS.ToString)
        lCmpTs.Attributes.Add("SRAD", aSRadTS.ToString)
        lCmpTs.Attributes.Add("Degrees F", aDegF)
        lCmpTs.Attributes.Add("Constant Coefficient", aCTX)
        Dim ls As String = "("
        For i = 1 To 12
            ls &= aCTS(i) & ", "
        Next
        ls = Left(ls, Len(ls) - 2) & ")"
        lCmpTs.Attributes.Add("Monthly Coefficients", ls)
        lCmpTs.Dates = aTMinTS.Dates
        lCmpTs.numValues = aTMinTS.numValues

        tsfil(1) = aTMinTS.Attributes.GetValue("TSFILL", -999)
        tsfil(2) = aTMaxTS.Attributes.GetValue("TSFILL", -999)
        tsfil(3) = aSRadTS.Attributes.GetValue("TSFILL", -999)

        For i = 1 To lCmpTs.numValues
            If Math.Abs(aTMinTS.Value(i) - tsfil(1)) < 0.000001 Or _
               Math.Abs(aTMaxTS.Value(i) - tsfil(2)) < 0.000001 Or _
               Math.Abs(aSRadTS.Value(i) - tsfil(3)) < 0.000001 Then
                'missing data
                lPanEvp(i) = tsfil(1)
            Else 'compute pet
                lAirTmp(i) = (aTMinTS.Value(i) + aTMaxTS.Value(i)) / 2
                If lPoint Then
                    Call J2Date(lCmpTs.Dates.Value(i), lDate)
                Else
                    Call J2Date(lCmpTs.Dates.Value(i - 1), lDate)
                End If
                Call Jensen(lDate(1), aCTS, lAirTmp(i), aDegF, aCTX, aSRadTS.Value(i), lPanEvp(i), lRetCod)
            End If
        Next i
        Array.Copy(lPanEvp, 1, lCmpTs.Values, 1, lCmpTs.numValues)
        Return lCmpTs

    End Function

    ''' <summary>
    ''' Compute HAMON PET from less than daily air temperature timeseries
    ''' </summary>
    ''' <param name="aTemperature"></param>
    ''' <param name="aSource"></param>
    ''' <param name="aDegF"></param>
    ''' <param name="aLatDeg"></param>
    ''' <param name="aCTS"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function CmpHamX(ByVal aTemperature As atcTimeseries, _
                            ByVal aSource As atcDataSource, _
                            ByVal aDegF As Boolean, _
                            ByVal aLatDeg As Double, _
                            ByVal aCTS() As Double) As atcTimeseries
        Dim lMin As Double = 1.0E+30, lMax As Double = -1.0E+30
        Dim lValue As Double, lDate As Double, lDateYesterday As Integer = 0
        Dim lIndex As Integer
        Dim lMinValues As New ArrayList
        Dim lMaxValues As New ArrayList
        Dim lDates As New ArrayList

        For lIndex = 1 To aTemperature.numValues
            lDate = aTemperature.Dates.Value(lIndex)
            If lDateYesterday = 0 Then
                lDateYesterday = CInt(lDate)
            ElseIf CInt(lDate) <> lDateYesterday Then
                lMinValues.Add(lMin)
                lMaxValues.Add(lMax)
                lDates.Add(CDbl(lDateYesterday))
                lDateYesterday = CInt(lDate)
                lMin = 1.0E+30
                lMax = -1.0E+30
            End If
            lValue = aTemperature.Value(lIndex)
            If lValue < lMin Then
                lMin = lValue
            End If
            If lValue > lMax Then
                lMax = lValue
            End If
        Next

        Dim lDatesTS As New atcTimeseries(aSource)
        lDatesTS.numValues = lDates.Count
        Dim lTMinTS As New atcTimeseries(aSource)
        Dim lTMaxTS As New atcTimeseries(aSource)
        lTMinTS.Attributes.SetValue("TS", 1)
        lTMaxTS.Attributes.SetValue("TS", 1)
        lTMinTS.Attributes.SetValue("TU", 4)
        lTMaxTS.Attributes.SetValue("TU", 4)
        lTMinTS.Dates = lDatesTS
        lTMaxTS.Dates = lDatesTS
        lTMinTS.numValues = lMinValues.Count
        lTMaxTS.numValues = lMaxValues.Count
        lDatesTS.Value(0) = lDates(0)
        For lIndex = 1 To lMaxValues.Count
            lDatesTS.Value(lIndex) = lDates(lIndex - 1) + 1
            lTMinTS.Value(lIndex) = lMinValues(lIndex - 1)
            lTMaxTS.Value(lIndex) = lMaxValues(lIndex - 1)
        Next
        Logger.Dbg("CmpHamX:Count:" & lMaxValues.Count)

        Return CmpHam(lTMinTS, lTMaxTS, aSource, aDegF, aLatDeg, aCTS)
    End Function

    Public Function CmpHam(ByVal aTMinTS As atcTimeseries, ByVal aTMaxTS As atcTimeseries, ByVal aSource As atcDataSource, ByVal aDegF As Boolean, ByVal aLatDeg As Double, ByVal aCTS() As Double) As atcTimeseries
        'compute HAMON - PET
        'aTminTS/aTMaxTS - min/max temp timeseries
        'aDegF   - Temperature in Degrees F (True) or C (False)
        'aLatDeg - latitude, in degrees
        'aCTS    - monthly variable coefficients

        Dim i As Integer
        Dim lRetCod As Integer
        Dim ldate(5) As Integer
        Dim lAirTmp(aTMinTS.numValues) As Double
        Dim lPanEvp(aTMinTS.numValues) As Double
        Dim tsfil(2) As Double
        Dim lCmpTs As New atcTimeseries(aSource)
        Dim lPoint As Boolean = aTMinTS.Attributes.GetValue("point", False)

        CopyBaseAttributes(aTMinTS, lCmpTs)
        lCmpTs.Attributes.SetValue("Constituent", "PET")
        lCmpTs.Attributes.SetValue("TSTYPE", "EVAP")
        lCmpTs.Attributes.SetValue("Scenario", "COMPUTED")
        lCmpTs.Attributes.SetValue("Description", "Daily Potential ET (in) computed using Hamon algorithm")
        lCmpTs.Attributes.AddHistory("Computed Daily Potential ET using Hamon - inputs: TMIN, TMAX, Degrees F, Latitude, Monthly Coefficients")
        lCmpTs.Attributes.Add("TMIN", aTMinTS.ToString)
        lCmpTs.Attributes.Add("TMAX", aTMaxTS.ToString)
        lCmpTs.Attributes.Add("Degrees F", aDegF)
        lCmpTs.Attributes.Add("LATDEG", aLatDeg)
        Dim ls As String = "("
        For i = 1 To 12
            ls &= aCTS(i) & ", "
        Next
        ls = Left(ls, Len(ls) - 2) & ")"
        lCmpTs.Attributes.Add("Monthly Coefficients", ls)
        lCmpTs.Dates = aTMinTS.Dates
        lCmpTs.numValues = aTMinTS.numValues

        'get fill value for input dsns
        tsfil(1) = aTMinTS.Attributes.GetValue("TSFILL", -999)
        tsfil(2) = aTMaxTS.Attributes.GetValue("TSFILL", -999)

        For i = 1 To lCmpTs.numValues
            If Math.Abs(aTMinTS.Value(i) - tsfil(1)) < 0.000001 Or _
               Math.Abs(aTMaxTS.Value(i) - tsfil(2)) < 0.000001 Then
                'missing data
                lPanEvp(i) = tsfil(1)
            Else 'compute pet
                lAirTmp(i) = (aTMinTS.Value(i) + aTMaxTS.Value(i)) / 2
                If lPoint Then
                    Call J2Date(lCmpTs.Dates.Value(i), ldate)
                Else
                    Call J2Date(lCmpTs.Dates.Value(i - 1), ldate)
                End If
                Call Hamon(ldate(1), ldate(2), aCTS, aLatDeg, lAirTmp(i), aDegF, lPanEvp(i), lRetCod)
            End If
        Next i
        Array.Copy(lPanEvp, 1, lCmpTs.Values, 1, lCmpTs.numValues)
        Return lCmpTs

    End Function

    Public Function CmpPen(ByVal aTMinTS As atcTimeseries, ByVal aTMaxTS As atcTimeseries, ByVal aSRadTS As atcTimeseries, ByVal aDewPTS As atcTimeseries, ByVal aWindTS As atcTimeseries, ByVal aSource As atcDataSource) As atcTimeseries
        'compute PENMAN - PET
        'input timeseries are Min/Max Temp, Dewpoint Temp, Solar Radiation, Wind Movement

        Dim i As Integer
        Dim lPanEvp(aTMinTS.numValues) As Double
        Dim tsfil(5) As Double
        Dim lCmpTs As New atcTimeseries(aSource)

        CopyBaseAttributes(aTMinTS, lCmpTs)
        lCmpTs.Attributes.SetValue("Constituent", "DEVP")
        lCmpTs.Attributes.SetValue("TSTYPE", "EVAP")
        lCmpTs.Attributes.SetValue("Scenario", "COMPUTED")
        lCmpTs.Attributes.SetValue("Description", "Daily Pan Evaporation (in) computed using Penman algorithm")
        lCmpTs.Attributes.AddHistory("Computed Daily Pan Evaporation using Penman - inputs: TMIN, TMAX, SRAD, DEWP, WIND")
        lCmpTs.Attributes.Add("TMIN", aTMinTS.ToString)
        lCmpTs.Attributes.Add("TMAX", aTMaxTS.ToString)
        lCmpTs.Attributes.Add("SRAD", aSRadTS.ToString)
        lCmpTs.Attributes.Add("DEWP", aDewPTS.ToString)
        lCmpTs.Attributes.Add("WIND", aWindTS.ToString)
        lCmpTs.Dates = aTMinTS.Dates
        lCmpTs.numValues = aTMinTS.numValues

        tsfil(1) = aTMinTS.Attributes.GetValue("TSFILL", -999)
        tsfil(2) = aTMaxTS.Attributes.GetValue("TSFILL", -999)
        tsfil(3) = aSRadTS.Attributes.GetValue("TSFILL", -999)
        tsfil(4) = aDewPTS.Attributes.GetValue("TSFILL", -999)
        tsfil(5) = aWindTS.Attributes.GetValue("TSFILL", -999)

        For i = 1 To lCmpTs.numValues
            If Math.Abs(aTMinTS.Value(i) - tsfil(1)) < 0.000001 Or _
               Math.Abs(aTMaxTS.Value(i) - tsfil(2)) < 0.000001 Or _
               Math.Abs(aSRadTS.Value(i) - tsfil(3)) < 0.000001 Or _
               Math.Abs(aDewPTS.Value(i) - tsfil(4)) < 0.000001 Or _
               Math.Abs(aWindTS.Value(i) - tsfil(5)) < 0.000001 Then
                'missing data
                lPanEvp(i) = tsfil(1)
            Else 'compute pet
                Call PNEVAP(aTMinTS.Value(i), aTMaxTS.Value(i), aDewPTS.Value(i), aWindTS.Value(i), aSRadTS.Value(i), lPanEvp(i))
            End If
        Next i
        Array.Copy(lPanEvp, 1, lCmpTs.Values, 1, lCmpTs.numValues)
        Return lCmpTs

    End Function

    Public Function CmpCld(ByVal aPctSun As atcTimeseries, ByVal aSource As atcDataSource) As atcTimeseries
        'compute %cloud cover from %sunshine

        Dim i, j As Integer
        Dim lSSSum, lSSDiv As Double
        Dim lCldCov(aPctSun.numValues) As Double
        Dim lSunVals() As Double
        Dim lCmpTs As New atcTimeseries(aSource)

        CopyBaseAttributes(aPctSun, lCmpTs)
        lCmpTs.Attributes.SetValue("Constituent", "CCOV")
        lCmpTs.Attributes.SetValue("TSTYPE", "CCOV")
        lCmpTs.Attributes.SetValue("Scenario", "COMPUTED")
        lCmpTs.Attributes.SetValue("Description", "Daily Cloud Cover (%) computed using Daily Percent Sun data")
        lCmpTs.Attributes.AddHistory("Computed Daily Cloud Cover using Daily Percent Sun - inputs: PSUN")
        lCmpTs.Attributes.Add("PSUN", aPctSun.ToString)
        lCmpTs.Dates = aPctSun.Dates
        lCmpTs.numValues = aPctSun.numValues

        lSunVals = aPctSun.Values
        'see if sunshine values are percent or fraction
        i = 1
        j = 0
        Do While j < 5 And i <= aPctSun.numValues
            If lSunVals(i) > 0 Then
                lSSSum = lSSSum + lSunVals(i)
                j = j + 1
            End If
            i = i + 1
        Loop
        'see what the sum of these values is
        If lSSSum > j Then 'must be percentages
            lSSDiv = 100
        Else 'must be fractions
            lSSDiv = 1
        End If
        For i = 1 To lCmpTs.numValues
            If lSunVals(i) < 0.0# Then lSunVals(i) = 0.0#
            lCldCov(i) = 10 * ((1 - lSunVals(i) / lSSDiv) ^ (3 / 5))
            If lCldCov(i) < 0.0# Then lCldCov(i) = 0.0#
        Next i
        Array.Copy(lCldCov, 1, lCmpTs.Values, 1, lCmpTs.numValues)
        Return lCmpTs

    End Function

    Public Function CmpWnd(ByVal aWndSpd As atcTimeseries, ByVal aSource As atcDataSource) As atcTimeseries
        'compute daily total wind travel (mi) from
        'average daily wind speed (mph)

        Dim i As Integer
        Dim lTotWnd(aWndSpd.numValues) As Double
        Dim lCmpTs As New atcTimeseries(aSource)

        CopyBaseAttributes(aWndSpd, lCmpTs)
        lCmpTs.Attributes.SetValue("Constituent", "TWND")
        lCmpTs.Attributes.SetValue("TSTYPE", "TWND")
        lCmpTs.Attributes.SetValue("Scenario", "COMPUTED")
        lCmpTs.Attributes.SetValue("Description", "Daily Total Wind (mi) computed using Average Daily Wind Speed (mph)")
        lCmpTs.Attributes.AddHistory("Computed Daily Total Wind using Daily Wind Speed - inputs: WIND")
        lCmpTs.Attributes.Add("WIND", aWndSpd.ToString)
        lCmpTs.Dates = aWndSpd.Dates
        lCmpTs.numValues = aWndSpd.numValues

        For i = 1 To lCmpTs.numValues
            If aWndSpd.Value(i) <= 0.0# Then 'not valid wind speed value
                lTotWnd(i) = 0
            Else
                lTotWnd(i) = 24 * aWndSpd.Value(i)
            End If
        Next i
        Array.Copy(lTotWnd, 1, lCmpTs.Values, 1, lCmpTs.numValues)
        Return lCmpTs

    End Function

    Public Function DisTemp(ByVal aMnTmpTS As atcTimeseries, ByVal aMxTmpTS As atcTimeseries, ByVal aDataSource As atcDataSource, ByVal aObsTime As Integer) As atcTimeseries ', ByRef SJDt As Double, ByRef EJDt As Double
        'Disaggregate daily min/max temperature to hourly temperature
        'Mn/MxTmpTS - input daily min/max temp values, 
        '             since disagg method uses either previous or ensuing 
        '             days values, these arrays contain two more days of 
        '             values than the period being disaggregated
        '  For Min Temp, the two extra values are at the end
        '  For Max Temp , there is one extra value at each end of the data set
        'aObsTime - hour of observation for the TMin/TMax values

        '*** Note:  SJDt/EJDt no longer in use, time of output TSer assumed same as input
        'SJDt - actual start date for output time series
        'EJDt - actual end date for output time series

        Dim lHrPos, ioff, j, i, lOpt, joff As Integer
        Dim lDate(5) As Integer
        Dim lHrVals(24) As Double
        Dim tsfil(1) As Single
        Dim lDisTs As New atcTimeseries(aDataSource)

        CopyBaseAttributes(aMnTmpTS, lDisTs)
        lDisTs.Attributes.SetValue("tu", atcTimeUnit.TUHour)
        lDisTs.Attributes.SetValue("ts", 1)
        lDisTs.Attributes.SetValue("Scenario", "COMPUTED")
        lDisTs.Attributes.SetValue("Constituent", "ATMP")
        lDisTs.Attributes.SetValue("TSTYPE", "ATMP")
        lDisTs.Attributes.SetValue("Description", "Hourly Temperature disaggregated from Daily TMin/TMax")
        lDisTs.Attributes.AddHistory("Disaggregated Temperature - inputs: TMIN, TMAX, Observation Time")
        lDisTs.Attributes.Add("TMIN", aMnTmpTS.ToString)
        lDisTs.Attributes.Add("TMAX", aMxTmpTS.ToString)
        lDisTs.Attributes.Add("Observation Time", aObsTime)

        lDisTs.Dates = DisaggDates(aMnTmpTS, aDataSource)
        lDisTs.numValues = lDisTs.Dates.numValues

        tsfil(0) = aMnTmpTS.Attributes.GetValue("TSFILL", -999)
        tsfil(1) = aMxTmpTS.Attributes.GetValue("TSFILL", -999)

        If aObsTime > 0 And aObsTime <= 6 Then
            'option 1 is early morning observations,
            'use max and min temp from previous calendar day
            lOpt = 1
        ElseIf aObsTime > 6 And aObsTime <= 16 Then
            'option 2 is mid day observations,
            'use max temp from previous day and min temp of current day
            lOpt = 2
        ElseIf aObsTime > 16 And aObsTime <= 24 Then
            'option 3 is evening observations,
            'use max and min temp from current day
            lOpt = 3
        End If

        Dim lHRTemp(lDisTs.numValues) As Double
        Dim lMinTmp(aMnTmpTS.numValues + 2) As Double
        Dim lMaxTmp(aMxTmpTS.numValues + 1) As Double

        'get max temp data
        joff = 0
        If lOpt = 3 Then 'back up one day (try to use 1st array element)
            ioff = 0
            'used to look for values prior to start date, but now just do whole available time span
            'If aMxTmpTS.Item(2).dates.Summary.SJDay >= SJDt Then
            'no data available prior to start date
            joff = 1
            'fill first max value with first available max value
            lMaxTmp(1) = aMxTmpTS.Value(1)
            'ElseIf aMxTmpTS.Item(2).dates.Summary.SJDay < SJDt Then
            '1st element preceeds start date, don't need it
            'ioff = 1
        Else '1st element is at start date, need it
            ioff = 0
        End If
        For i = 1 To aMxTmpTS.numValues - ioff
            lMaxTmp(i + joff) = aMxTmpTS.Value(i + ioff)
        Next i
        If lOpt <> 3 Then ' And aMxTmpTS.Item(2).dates.Summary.EJDay <= EJDt Then
            'extra value needed, but not available, fill with previous
            lMaxTmp(aMxTmpTS.numValues + 1) = lMaxTmp(aMxTmpTS.numValues)
        End If

        'get min temp data
        If lOpt = 1 Then 'move up one day for min temp values
            ioff = 1
        Else
            ioff = 0
        End If
        For i = 1 To aMnTmpTS.numValues - ioff
            lMinTmp(i) = aMnTmpTS.Value(i + ioff)
        Next i
        'check end points
        'If MnMxTmp.Item(1).dates.Summary.EJDay <= EJDt Then
        If lOpt = 1 Then 'need 2 extra values at end, fill with last good one
            lMinTmp(aMnTmpTS.numValues + 1) = lMinTmp(aMnTmpTS.numValues - 1)
        End If
        lMinTmp(aMnTmpTS.numValues + 1 - ioff) = lMinTmp(aMnTmpTS.numValues - ioff)
        'End If

        lHrPos = 0
        For i = 1 To aMnTmpTS.numValues
            If (Math.Abs(lMaxTmp(i) - tsfil(1)) > 0.000001) Or (Math.Abs(lMinTmp(i) - tsfil(0)) > 0.000001) Then
                'value is not missing, so distribute
                Call DISTRB(lMaxTmp(i), lMinTmp(i), lMaxTmp(i + 1), lMinTmp(i + 1), lHrVals)
                For j = 1 To 24
                    lHRTemp(lHrPos + j) = lHrVals(j)
                Next j
            Else 'value is missing, so leave hourlies missing
                For j = 1 To 24
                    lHRTemp(lHrPos + j) = tsfil(1)
                Next j
            End If
            lHrPos = lHrPos + 24
        Next i
        Array.Copy(lHRTemp, 1, lDisTs.Values, 1, lDisTs.numValues)
        Return lDisTs

    End Function

    ''' <summary>
    ''' Disaggregate daily SOLAR or PET to hourly
    ''' </summary>
    ''' <param name="aInTs">input timeseries to be disaggregated</param>
    ''' <param name="aDataSource"></param>
    ''' <param name="aDisOpt">1 does Solar, DisOpt = 2 does PET</param>
    ''' <param name="aLatDeg">latitude, in degrees</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function DisSolPet(ByVal aInTs As atcTimeseries, ByVal aDataSource As atcDataSource, ByVal aDisOpt As Integer, ByVal aLatDeg As Double) As atcTimeseries
        Dim lHrPos, i, j, retcod As Integer
        Dim lDate(5) As Integer
        Dim lDisTs As New atcTimeseries(aDataSource)
        Dim lPoint As Boolean = aInTs.Attributes.GetValue("point", False)
        Dim lHrVals(24) As Double

        CopyBaseAttributes(aInTs, lDisTs)
        lDisTs.Attributes.SetValue("Scenario", "COMPUTED")
        lDisTs.Attributes.SetValue("tu", atcTimeUnit.TUHour)
        lDisTs.Attributes.SetValue("ts", 1)
        If aDisOpt = 1 Then 'solar disaggregation
            lDisTs.Attributes.SetValue("Constituent", "SOLR")
            lDisTs.Attributes.SetValue("TSTYPE", "SOLR")
            lDisTs.Attributes.SetValue("Description", "Hourly Solar Radiation (Langleys) disaggregated from Daily")
            lDisTs.Attributes.AddHistory("Disaggregated Solar Radiation - inputs: SRAD, Latitude")
            lDisTs.Attributes.Add("SRAD", aInTs.ToString)
        Else 'ET disaggregation
            lDisTs.Attributes.SetValue("Constituent", "EVAP")
            lDisTs.Attributes.SetValue("TSTYPE", "EVAP")
            lDisTs.Attributes.SetValue("Description", "Hourly Evapotranspiration disaggregated from Daily")
            lDisTs.Attributes.AddHistory("Disaggregated Evapotranspiration - inputs: DEVT, Latitude")
            lDisTs.Attributes.Add("DEVT", aInTs.ToString)
        End If
        lDisTs.Attributes.Add("Latitude", aLatDeg)

        lDisTs.Dates = DisaggDates(aInTs, aDataSource)
        lDisTs.numValues = lDisTs.Dates.numValues

        Dim lOutTs(lDisTs.numValues) As Double
        lHrPos = 0
        For i = 1 To aInTs.numValues
            If Not Double.IsNaN(aInTs.Value(i)) Then
                If lPoint Then
                    Call J2Date(aInTs.Dates.Value(i), lDate)
                Else
                    Call J2Date(aInTs.Dates.Value(i - 1), lDate)
                End If
                If aDisOpt = 1 Then 'solar
                    Call RADDST(aLatDeg, lDate(1), lDate(2), aInTs.Value(i), lHrVals, retcod)
                ElseIf aDisOpt = 2 Then  'pet
                    Call PETDST(aLatDeg, lDate(1), lDate(2), aInTs.Value(i), lHrVals, retcod)
                End If
                For j = 1 To 24
                    lOutTs(lHrPos + j) = lHrVals(j)
                Next j
            Else
                For j = 1 To 24
                    lOutTs(lHrPos + j) = Double.NaN
                Next j
            End If
            'increment to next 24 hour period
            lHrPos = lHrPos + 24
        Next i
        Array.Copy(lOutTs, 1, lDisTs.Values, 1, lDisTs.numValues)
        Return lDisTs

    End Function

    Public Function DisWnd(ByVal aInTs As atcTimeseries, ByVal aDataSource As atcDataSource, ByVal aDCurve() As Double) As atcTimeseries
        'Disaggregate daily wind to hourly
        'InTs - input daily wind timeseries
        'DCurve - hourly diurnal curve for wind disaggregation

        Dim k, i, j As Integer
        Dim lDisTs As New atcTimeseries(aDataSource)
        Dim lHrVals(24) As Double

        CopyBaseAttributes(aInTs, lDisTs)
        lDisTs.Attributes.SetValue("tu", atcTimeUnit.TUHour)
        lDisTs.Attributes.SetValue("ts", 1)
        lDisTs.Attributes.SetValue("Scenario", "COMPUTED")
        lDisTs.Attributes.SetValue("Constituent", "WIND")
        lDisTs.Attributes.SetValue("TSTYPE", "WIND")
        lDisTs.Attributes.SetValue("Description", "Hourly Wind Travel disaggregated from Daily")
        lDisTs.Attributes.AddHistory("Disaggregated Wind Travel - inputs: TWND, Hourly Distribution")
        lDisTs.Attributes.Add("TWND", aInTs.ToString)
        lDisTs.Attributes.Add("Hourly Distribution", aDCurve)

        'build new date array for hourly TSer
        lDisTs.Dates = DisaggDates(aInTs, aDataSource)
        lDisTs.numValues = lDisTs.Dates.numValues

        Dim lOutTs(lDisTs.numValues) As Double
        k = 0
        For i = 1 To aInTs.numValues
            For j = 1 To 24
                lOutTs(k + j) = aInTs.Value(i) * aDCurve(j)
            Next j
            'increment to next 24 hour period
            k = k + 24
        Next i
        Array.Copy(lOutTs, 1, lDisTs.Values, 1, lDisTs.numValues)
        Return lDisTs

    End Function

    Public Function DisDewPoint(ByVal aInTs As atcTimeseries, ByVal aDataSource As atcDataSource, ByVal aAirTemp As atcTimeseries) As atcTimeseries
        'disaggregate daily Dewpoint Temp to hourly, adjust using min temp as needed
        'aInTs   - input timeseries to be disaggregated
        'aMinTemp - hourly min temp timeseries to make sure disaggregated Dewpoint doesn't exceed

        Dim lAirPos As Integer
        Dim lDewPos As Integer
        Dim lDisTs As atcTimeseries = Aggregate(aInTs, modDate.atcTimeUnit.TUHour, 1, modDate.atcTran.TranAverSame, aDataSource)

        lDisTs.Attributes.SetValue("Scenario", "COMPUTED")
        lDisTs.Attributes.SetValue("Constituent", "DEWP")
        lDisTs.Attributes.SetValue("TSTYPE", "DEWP")
        lDisTs.Attributes.SetValue("Description", "Hourly Dewpoint Temp disaggregated from Daily")
        lDisTs.Attributes.AddHistory("Disaggregated Dewpoint - inputs: DDPT, ATMP")
        lDisTs.Attributes.Add("DDPT", aInTs.ToString)
        lDisTs.Attributes.Add("ATMP", aAirTemp.ToString)

        If aAirTemp.Attributes.GetValue("TU") = modDate.atcTimeUnit.TUHour Then
            'check air temp values to make sure dewpoint doesn't exceed it
            Dim lAirSJD As Double = aAirTemp.Dates.Value(1)
            Dim lAirEJD As Double = aAirTemp.Dates.Value(lDisTs.numValues)
            Dim lDewSJD As Double = lDisTs.Dates.Value(1)
            Dim lDewEJD As Double = lDisTs.Dates.Value(lDisTs.numValues)
            If lAirEJD < lDewSJD Or lAirSJD > lDewEJD Then 'no overlap
                Logger.Dbg("WARNING: Temperature Timeseries period does not overlap Dewpoint Timeseries period." & vbCrLf & _
                           "The Dewpoint data have been disaggregated, but not checked" & vbCrLf & _
                           "to see that they don't fall below the current temperature." & vbCrLf & _
                           "Julian Start/End of Temperature: " & lAirSJD & " : " & lAirEJD & vbCrLf & _
                           "Julian Start/End of Dewpoint: " & lDewSJD & " : " & lDewEJD)
            Else
                If lAirSJD <= lDewSJD Then 'move up in airtemp TSer
                    If lAirEJD > lDewSJD Then 'the two timeseries overlap beginning at start of dewpoint
                        lAirPos = (lDewSJD - lAirSJD) / JulianHour
                        lDewPos = 1
                    End If
                    If lAirEJD < lDewEJD Then 'not a full overlap
                        Logger.Dbg("WARNING: Temperature Timeseries period does not completely overlap Dewpoint Timeseries period." & vbCrLf & _
                                   "The Dewpoint data have been disaggregated, but portions have not been" & vbCrLf & _
                                   "checked to see that they don't fall below the current temperature." & vbCrLf & _
                                   "Julian Start/End of Temperature: " & lAirSJD & " : " & lAirEJD & vbCrLf & _
                                   "Julian Start/End of Dewpoint: " & lDewSJD & " : " & lDewEJD)
                    End If
                ElseIf lAirSJD < lDewEJD Then 'overlap begins at start of airtemp, not a full overlap
                    lAirPos = 0
                    lDewPos = (lAirSJD - lDewSJD) / JulianHour + 1
                    Logger.Dbg("WARNING: Temperature Timeseries period does not completely overlap Dewpoint Timeseries period." & vbCrLf & _
                               "The Dewpoint data have been disaggregated, but portions have not been" & vbCrLf & _
                               "checked to see that they don't fall below the current temperature." & vbCrLf & _
                               "Julian Start/End of Temperature: " & lAirSJD & " : " & lAirEJD & vbCrLf & _
                               "Julian Start/End of Dewpoint: " & lDewSJD & " : " & lDewEJD)
                End If
                For i As Integer = lDewPos To lDisTs.numValues
                    lAirPos += 1
                    If lAirPos <= aAirTemp.numValues Then
                        If lDisTs.Value(i) > aAirTemp.Value(lAirPos) Then
                            lDisTs.Value(i) = aAirTemp.Value(i)
                        End If
                    Else
                        Exit For
                    End If
                Next i
            End If
        Else
            Logger.Dbg("WARNING: Temperature Timeseries for Dewpoint Disaggregation is not Hourly." & vbCrLf & _
                       "The Dewpoint data have been disaggregated, but not checked" & vbCrLf & _
                       "to see that they don't fall below the current temperature." & vbCrLf & _
                       "Temperature Time Units should be '3', but are " & aAirTemp.Attributes.GetValue("TU"))
        End If
        Return lDisTs

    End Function

    Private Function DisaggDates(ByVal aInTS As atcTimeseries, ByVal aDataSource As atcDataSource) As atcTimeseries
        'build new date timeseries class for hourly TSer based on daily TSer (aInTS)

        Dim lDates As New atcTimeseries(aDataSource)
        'lDates.numValues = aInTS.numValues * 24
        'lDates.ValuesNeedToBeRead = True
        lDates.Values = NewDates(aInTS, atcTimeUnit.TUHour, 1)
        Return lDates

        ''NOTE: Only valid for constant interval timeseries
        'Dim lPoint As Boolean = aInTS.Attributes.GetValue("point", False)

        'If lPoint Then
        '    'lDates.Value(ip) = Double.NaN
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
    'Public Function DisDwpnt(ByRef InTs As Collection) As atcData.ATCclsTserData
    '  'Disaggregate dewpoint temperature from daily to hourly
    '  'assuming daily average is constant for 24 hours

    '  Dim k, i, j, lnval As Integer
    '  Dim lDisTs As atcData.ATCclsTserData
    '  Dim OutTs() As Single

    '  'UPGRADE_WARNING: Couldn't resolve default property of object InTs(). Click for more: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
    '  Call InitCmpTs(InTs.Item(1), atcData.ATCTimeUnit.TUHour, 1, 24, lDisTs)
    '  ReDim OutTs(lDisTs.dates.Summary.NVALS)

    '  k = 1
    '  'UPGRADE_WARNING: Couldn't resolve default property of object InTs(1).dates. Click for more: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
    '  For i = 1 To InTs.Item(1).dates.Summary.NVALS
    '    For j = 0 To 23
    '      'UPGRADE_WARNING: Couldn't resolve default property of object InTs().Value. Click for more: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
    '      OutTs(k + j) = InTs.Item(1).Value(i)
    '    Next j
    '    'increment to next 24 hour period
    '    k = k + 24
    '  Next i
    '  lDisTs.Values = VB6.CopyArray(OutTs)
    '  DisDwpnt = lDisTs

    'End Function

    Public Sub DISTRB(ByVal PreMax As Double, ByVal CurMin As Double, ByVal CurMax As Double, ByVal NxtMin As Double, ByRef HRTemp() As Double)

        'Distribute daily max-min temperatures to hourly values

        'PREMAX - previous max temperature
        'CURMIN - current min temperature
        'CURMAX - current max temperature
        'NXTMIN - next min temperature
        'HRTEMP - array of hourly values

        Dim lDif2, lDif1, lDif3 As Double

        lDif1 = PreMax - CurMin
        lDif2 = CurMin - CurMax
        lDif3 = CurMax - NxtMin

        HRTemp(1) = CurMin + lDif1 * 0.15
        HRTemp(2) = CurMin + lDif1 * 0.1
        HRTemp(3) = CurMin + lDif1 * 0.06
        HRTemp(4) = CurMin + lDif1 * 0.03
        HRTemp(5) = CurMin + lDif1 * 0.01
        HRTemp(6) = CurMin
        HRTemp(7) = CurMin - lDif2 * 0.16
        HRTemp(8) = CurMin - lDif2 * 0.31
        HRTemp(9) = CurMin - lDif2 * 0.45
        HRTemp(10) = CurMin - lDif2 * 0.59
        HRTemp(11) = CurMin - lDif2 * 0.71
        HRTemp(12) = CurMin - lDif2 * 0.81
        HRTemp(13) = CurMin - lDif2 * 0.89
        HRTemp(14) = CurMin - lDif2 * 0.95
        HRTemp(15) = CurMin - lDif2 * 0.99
        HRTemp(16) = CurMax
        HRTemp(17) = NxtMin + lDif3 * 0.89
        HRTemp(18) = NxtMin + lDif3 * 0.78
        HRTemp(19) = NxtMin + lDif3 * 0.67
        HRTemp(20) = NxtMin + lDif3 * 0.57
        HRTemp(21) = NxtMin + lDif3 * 0.47
        HRTemp(22) = NxtMin + lDif3 * 0.38
        HRTemp(23) = NxtMin + lDif3 * 0.29
        HRTemp(24) = NxtMin + lDif3 * 0.22

    End Sub

    Private Sub RADDST(ByVal aLatDeg As Double, ByVal aMonth As Integer, ByVal aDay As Integer, ByVal aDayRad As Double, ByRef aHrRad() As Double, ByRef aRetCod As Integer)
        'Distributes daily solar radiation to hourly
        'values, based on a method used in HSP (Hydrocomp, 1976).
        'It uses the latitude, month, day, and daily radiation.

        'aLatDeg - latitude(degrees)
        'aMONTH  - month of the year (1-12)
        'aDAY    - day of the month (1-31)
        'aDAYRAD - input daily radiation (langleys)
        'aHRRAD  - output array of hourly radiation (langleys)
        'aRETCOD - return code (0 = ok, -1 = bad latitude)

        Dim IK As Integer
        Dim TR3, TRise, CRAD, DTR2, Delt, CS, AD, JulDay, RK, LatRdn, Phi, SS, X2, SunR, DTR4, SL, TR2, TR4 As Double

        'julian date
        JulDay = 30.5 * (aMonth - 1) + aDay

        'check latitude
        If aLatDeg < -66.5 Or aLatDeg > 66.5 Then 'invalid latitude, return
            aRetCod = -1
        Else 'latitude ok
            'convert to radians
            LatRdn = aLatDeg * 0.0174582

            Phi = LatRdn
            AD = 0.40928 * System.Math.Cos(0.0172141 * (172.0# - JulDay))
            SS = Math.Sin(Phi) * Math.Sin(AD)
            CS = Math.Cos(Phi) * Math.Cos(AD)
            X2 = -SS / CS
            Delt = 7.6394 * (1.5708 - Math.Atan(X2 / Math.Sqrt(1.0# - X2 ^ 2)))
            SunR = 12.0# - Delt / 2.0#

            'develop hourly distribution given sunrise,
            'sunset and length of day (DELT)
            DTR2 = Delt / 2.0#
            DTR4 = Delt / 4.0#
            CRAD = 0.66666667 / DTR2
            SL = CRAD / DTR4
            TRise = SunR
            TR2 = TRise + DTR4
            TR3 = TR2 + DTR2
            TR4 = TR3 + DTR4

            'hourly loop
            For IK = 1 To 24
                RK = IK
                If RK > TRise Then
                    If RK > TR2 Then
                        If RK > TR3 Then
                            If RK > TR4 Then
                                aHrRad(IK) = 0.0#
                            Else
                                aHrRad(IK) = (CRAD - (RK - TR3) * SL) * aDayRad
                            End If
                        Else
                            aHrRad(IK) = CRAD * aDayRad
                        End If
                    Else
                        aHrRad(IK) = (RK - TRise) * SL * aDayRad
                    End If
                Else
                    aHrRad(IK) = 0.0#
                End If
            Next IK
            aRetCod = 0
        End If

    End Sub

    Public Sub PETDST(ByVal aLatDeg As Double, ByVal aMonth As Integer, ByVal aDay As Integer, ByVal aDayPet As Double, ByRef aHrPet() As Double, ByRef aRetCod As Integer)

        'Distributes daily PET to hourly values,
        'based on a method used to disaggregate solar radiation
        'in HSP (Hydrocomp, 1976) using latitude, month, day,
        'and daily PET.

        'aLatDeg - latitude(degrees)
        'aMONTH  - month of the year (1-12)
        'aDAY    - day of the month (1-31)
        'aDAYPET - input daily PET (inches)
        'aHRPET  - output array of hourly PET (inches)
        'aRETCOD - return code (0 = ok, -1 = bad latitude)

        Dim IK As Integer
        Dim TR3, TRise, CRAD, DTR2, Delt, CS, AD, JulDay, RK, LatRdn, Phi, SS, X2, SunR, DTR4, SL, TR2, TR4 As Double
        Dim CURVE(24) As Double

        'julian date
        JulDay = 30.5 * (aMonth - 1) + aDay

        'check latitude
        If aLatDeg < -66.5 Or aLatDeg > 66.5 Then 'invalid latitude, return
            aRetCod = -1
        Else 'latitude ok
            'convert to radians
            LatRdn = aLatDeg * 0.0174582

            Phi = LatRdn
            AD = 0.40928 * Math.Cos(0.0172141 * (172.0# - JulDay))
            SS = Math.Sin(Phi) * Math.Sin(AD)
            CS = Math.Cos(Phi) * Math.Cos(AD)
            X2 = -SS / CS
            Delt = 7.6394 * (1.5708 - Math.Atan(X2 / Math.Sqrt(1.0# - X2 ^ 2)))
            SunR = 12.0# - Delt / 2.0#

            'develop hourly distribution given sunrise,
            'sunset and length of day (DELT)
            DTR2 = Delt / 2.0#
            DTR4 = Delt / 4.0#
            CRAD = 0.66666667 / DTR2
            SL = CRAD / DTR4
            TRise = SunR
            TR2 = TRise + DTR4
            TR3 = TR2 + DTR2
            TR4 = TR3 + DTR4

            'calculate hourly distribution curve
            For IK = 1 To 24
                RK = IK
                If RK > TRise Then
                    If RK > TR2 Then
                        If RK > TR3 Then
                            If RK > TR4 Then
                                CURVE(IK) = 0.0#
                                aHrPet(IK) = CURVE(IK)
                            Else
                                CURVE(IK) = (CRAD - (RK - TR3) * SL)
                                aHrPet(IK) = CURVE(IK) * aDayPet
                            End If
                        Else
                            CURVE(IK) = CRAD
                            aHrPet(IK) = CURVE(IK) * aDayPet
                        End If
                    Else
                        CURVE(IK) = (RK - TRise) * SL
                        aHrPet(IK) = CURVE(IK) * aDayPet
                    End If
                Else
                    CURVE(IK) = 0.0#
                    aHrPet(IK) = CURVE(IK)
                End If
            Next IK
            aRetCod = 0
        End If

    End Sub

    Private Sub RadClc(ByRef aDegLat As Double, ByRef aCloud As Double, ByRef aMon As Integer, ByRef aDay As Integer, ByRef aDayRad As Double)

        'This routine computes the total daily solar radiation based on
        'the HSPII (Hydrocomp, 1978) RADIATION procedure, which is based
        'on empirical curves of radiation as a function of latitude
        '(Hamon et al, 1954, Monthly Weather Review 82(6):141-146.

        Dim ILat, ii As Integer
        Dim Lat3, Lat1, Lat2, Lat4 As Double
        Dim A1, b, Exp2, Exp1, a, A0, A2 As Double
        Dim b2, A3, b1, Frac As Double
        Dim SS, x As Double
        Dim Y100, YRD As Double

        'integer part of latitude
        ILat = Int(aDegLat)

        'fractional part of latitude
        Frac = aDegLat - CSng(ILat)
        If Frac <= 0.0001 Then Frac = 0.0#

        A0 = XLax(ILat, 1) + Frac * (XLax(ILat + 1, 1) - XLax(ILat, 1))
        A1 = XLax(ILat, 2) + Frac * (XLax(ILat + 1, 2) - XLax(ILat, 2))
        A2 = XLax(ILat, 3) + Frac * (XLax(ILat + 1, 3) - XLax(ILat, 3))
        A3 = XLax(ILat, 4) + Frac * (XLax(ILat + 1, 4) - XLax(ILat, 4))
        b1 = XLax(ILat, 5) + Frac * (XLax(ILat + 1, 5) - XLax(ILat, 5))
        b2 = XLax(ILat, 6) + Frac * (XLax(ILat + 1, 6) - XLax(ILat, 6))
        b = aDegLat - 44.0#
        a = aDegLat - 25.0#
        Exp1 = 0.7575 - 0.0018 * a
        Exp2 = 0.725 + 0.00288 * b
        Lat1 = 2.139 + 0.0423 * a
        Lat2 = 30.0# - 0.667 * a
        Lat3 = 2.9 - 0.0629 * b
        Lat4 = 18.0# + 0.833 * b

        'Percent sunshine
        SS = 100.0# * (1.0# - (aCloud / 10.0#) ^ (5.0# / 3.0#))
        If SS < 0.0# Then
            'can't have SS being negative
            SS = 0.0#
        End If

        x = X1(aMon) + aDay
        'convert to radians
        x = x * 2.0# * 3.14159 / 360.0#

        Y100 = A0 + A1 * Math.Cos(x) + A2 * Math.Cos(2 * x) + A3 * Math.Cos(3 * x) + b1 * Math.Sin(x) + b2 * Math.Sin(2 * x)

        ii = Math.Ceiling((SS + 10.0#) / 10.0#)

        If aDegLat > 43.0# Then
            YRD = Lat3 * SS ^ Exp2 + Lat4
        Else
            YRD = Lat1 * SS ^ Exp1 + Lat2
        End If

        If ii < 11 Then
            YRD = YRD + c(ii, aMon)
        End If

        If YRD >= 100.0# Then
            aDayRad = Y100
        Else
            aDayRad = Y100 * YRD / 100.0#
        End If

    End Sub

    Private Sub CldClc(ByRef aDegLat As Double, ByRef aDayRad As Double, ByRef aMon As Integer, ByRef aDay As Integer, ByRef aCloud As Double)

        'This routine computes the daily cloud cover based on daily solar radiation.
        'NOTE:  This routine makes what is likely a gross assumption - 
        'that percent sun, and thus, cloud cover is essentially the ratio
        'of actual solar radiation to potential max solar radiation.
        'Max solar radiation is based on the above routine (RadClc), which uses
        'the HSPII (Hydrocomp, 1978) RADIATION procedure, which is based
        'on empirical curves of radiation as a function of latitude
        '(Hamon et al, 1954, Monthly Weather Review 82(6):141-146.

        Dim ILat, ii As Integer
        Dim Lat3, Lat1, Lat2, Lat4 As Double
        Dim A1, b, Exp2, Exp1, a, A0, A2 As Double
        Dim b2, A3, b1, Frac As Double
        Dim SS, x As Double
        Dim Y100, YRD As Double

        'integer part of latitude
        ILat = Int(aDegLat)

        'fractional part of latitude
        Frac = aDegLat - CSng(ILat)
        If Frac <= 0.0001 Then Frac = 0.0#

        A0 = XLax(ILat, 1) + Frac * (XLax(ILat + 1, 1) - XLax(ILat, 1))
        A1 = XLax(ILat, 2) + Frac * (XLax(ILat + 1, 2) - XLax(ILat, 2))
        A2 = XLax(ILat, 3) + Frac * (XLax(ILat + 1, 3) - XLax(ILat, 3))
        A3 = XLax(ILat, 4) + Frac * (XLax(ILat + 1, 4) - XLax(ILat, 4))
        b1 = XLax(ILat, 5) + Frac * (XLax(ILat + 1, 5) - XLax(ILat, 5))
        b2 = XLax(ILat, 6) + Frac * (XLax(ILat + 1, 6) - XLax(ILat, 6))
        b = aDegLat - 44.0#
        a = aDegLat - 25.0#
        Exp1 = 0.7575 - 0.0018 * a
        Exp2 = 0.725 + 0.00288 * b
        Lat1 = 2.139 + 0.0423 * a
        Lat2 = 30.0# - 0.667 * a
        Lat3 = 2.9 - 0.0629 * b
        Lat4 = 18.0# + 0.833 * b

        x = X1(aMon) + aDay
        'convert to radians
        x = x * 2.0# * 3.14159 / 360.0#

        Y100 = A0 + A1 * Math.Cos(x) + A2 * Math.Cos(2 * x) + A3 * Math.Cos(3 * x) + b1 * Math.Sin(x) + b2 * Math.Sin(2 * x)

        YRD = (aDayRad / Y100) * 100

        'NOTE: here's where the ratio of Rad to Max Rad is used as %Sun
        ii = Math.Ceiling((Math.Min(100, YRD) + 10.0#) / 10.0#)
        If ii < 11 Then
            YRD = YRD - c(ii, aMon)
        End If

        If aDegLat > 43.0# Then
            SS = Math.Pow(((YRD - Lat4) / Lat3), 1 / Exp2)
        Else
            SS = Math.Pow(((YRD - Lat2) / Lat1), 1 / Exp1)
        End If

        If SS < 0.0# Then
            'can't have SS being negative
            SS = 0.0#
        End If
        'get cloud cover from %sun
        aCloud = 10 * Math.Pow(-((SS / 100) - 1), 3 / 5)

    End Sub

    Private Sub Jensen(ByVal aMon As Integer, ByVal aCTS() As Double, ByVal aAirTmp As Double, ByVal aDegF As Boolean, ByVal aCTX As Double, ByVal aSolRad As Double, ByRef aPanEvp As Double, ByRef aRetCod As Integer)

        'Generates daily pan evaporation (inches)
        'using a coefficient for the month, the daily average air
        'temperature (F), a coefficient, and solar radiation
        '(langleys/day). The computations are based on the Jensen
        'and Haise (1963) formula.

        'aCTS    - array of monthly coefficients
        'aAirTmp - daily average air temperature (F)
        'aDegF   - temperature in Fahrenheit (True) or Celsius (False)
        'aCTX    - coefficient
        'aSolRad - solar radiation (langleys/day)
        'aPanEvp - daily pan evaporation (inches)
        'aRetCod - return code
        '          0 - operation successful
        '         -1 - operation failed

        Dim lTAVF As Double
        Dim lSRadIn As Double
        Dim lTAVC As Double

        aRetCod = 0

        If aDegF Then 'input is fahrenheit
            lTAVF = aAirTmp
            lTAVC = (aAirTmp - 32.0#) * 5.0# / 9.0#
        Else 'input is celsius
            lTAVC = aAirTmp
            lTAVF = (aAirTmp * (9.0# / 5.0#)) + 32.0#
        End If

        'convert solar radiation from langleys to equivalent inches of water evaporation
        lSRadIn = aSolRad / ((597.3 - 0.57 * lTAVC) * 2.54)

        'compute evaporation using Jensen-Haise (1963) formula
        aPanEvp = aCTS(aMon) * (lTAVF - aCTX) * lSRadIn

        'when the estimated pan evaporation
        'is negative the value is set to zero
        If aPanEvp < 0.0# Then
            aPanEvp = 0.0#
        End If

    End Sub

    Private Sub Hamon(ByVal aMonth As Integer, ByVal aDay As Integer, ByVal aCTS() As Double, ByVal aLatDeg As Double, ByVal aTAVC As Double, ByVal aDegF As Boolean, ByRef PanEvp As Single, ByVal aRetCod As Integer)

        'Generates daily pan evaporation (inches)
        'using a coefficient for the month, the possible hours of
        'sunshine (computed from latitude), and absolute humidity.
        'The computations are based on the Hamon (1961) formula.

        'CTS    - array of monthly coefficients
        'LatDeg - latitude
        'TAVC   - Average daily temperature (C)
        'aDegF  - temperature in Fahrenheit (True) or Celsius (False)
        'PanEvp - daily pan evaporation (inches)
        'RetCod - return code
        '          0 - operation successful
        '         -1 - operation failed

        Dim VDSAT, SUNS, Delt, CS, AD, JulDay, LatRdn, Phi, SS, X2, SunR, DYL, VPSAT As Double

        aRetCod = 0

        'julian date
        JulDay = 30.5 * (aMonth - 1) + aDay

        'check latitude
        If aLatDeg < -66.5 Or aLatDeg > 66.5 Then 'invalid latitude, return
            aRetCod = -1
        Else 'latitude ok,convert to radians
            LatRdn = aLatDeg * 0.0174582
            Phi = LatRdn
            AD = 0.40928 * System.Math.Cos(0.0172141 * (172.0# - JulDay))
            SS = System.Math.Sin(Phi) * System.Math.Sin(AD)
            CS = System.Math.Cos(Phi) * System.Math.Cos(AD)
            X2 = -SS / CS
            Delt = 7.6394 * (1.5708 - System.Math.Atan(X2 / System.Math.Sqrt(1.0# - X2 ^ 2)))
            SunR = 12.0# - Delt / 2.0#
            SUNS = 12.0# + Delt / 2.0#
            DYL = (SUNS - SunR) / 12

            'convert temperature to Centigrade if necessary
            If aDegF Then aTAVC = (aTAVC - 32.0#) * (5.0# / 9.0#)

            'Hamon equation
            VPSAT = 6.108 * System.Math.Exp(17.26939 * aTAVC / (aTAVC + 237.3))
            VDSAT = 216.7 * VPSAT / (aTAVC + 273.3)
            PanEvp = aCTS(aMonth) * DYL * DYL * VDSAT

            'when the estimated pan evaporation is negative
            'the value is set to zero
            If PanEvp < 0.0# Then
                PanEvp = 0.0#
            End If
        End If

    End Sub

    Private Sub PNEVAP(ByVal aMinTmp As Double, ByVal aMaxTmp As Double, ByVal aDewTmp As Double, ByVal aWindSp As Double, ByVal aSolRad As Double, ByRef aPanEvp As Double)

        'Generates daily pan evaporation (inches) using
        'daily minimum air temperature (F), daily maximum air
        'temperature, dewpoint (F), wind movement (miles/day), and solar
        'radiation (langleys/day). The computations are based on the Penman
        '(1948) formula and the method of Kohler, Nordensen, and Fox (1955).

        'aMinTmp - daily minimum air temperature (F)
        'aMaxTmp - daily maximum air temperature (F)
        'aDewTmp - dewpoint(F)
        'aWindSp - wind movement (miles/day)
        'aSolRad - solar radiation (langleys/day)
        'aPanEvp - daily pan evaporation (inches)

        Dim lQNDelt, lEsMiEa, lDelta, lEaGama, lAirTmp As Double

        'compute average daily air temperature
        lAirTmp = (aMinTmp + aMaxTmp) / 2.0#

        'net radiation exchange * delta
        If aSolRad <= 0.0# Then aSolRad = 0.00001
        lQNDelt = Math.Exp((lAirTmp - 212.0#) * (0.1024 - 0.01066 * Math.Log(aSolRad))) - 0.0001

        'Vapor pressure deficit between surface and
        'dewpoint temps(Es-Ea) IN of Hg
        lEsMiEa = (6413252.0# * System.Math.Exp(-7482.6 / (lAirTmp + 398.36))) - (6413252.0# * Math.Exp(-7482.6 / (aDewTmp + 398.36)))

        'pan evaporation assuming air temp equals water surface temp.

        'when vapor pressure deficit turns negative it is set equal to zero
        If lEsMiEa < 0.0# Then
            lEsMiEa = 0.0#
        End If

        'pan evap * GAMMA, GAMMA = 0.0105 inch Hg/F
        lEaGama = 0.0105 * (lEsMiEa ^ 0.88) * (0.37 + 0.0041 * aWindSp)

        'Delta = slope of saturation vapor pressure curve at air temperature
        lDelta = 47987800000.0# * Math.Exp(-7482.6 / (lAirTmp + 398.36)) / ((lAirTmp + 398.36) ^ 2)

        'pan evaporation rate in inches per day
        aPanEvp = (lQNDelt + lEaGama) / (lDelta + 0.0105)

        'when the estimated pan evaporation is negative
        'the value is set to zero
        If aPanEvp < 0.0# Then
            aPanEvp = 0.0#
        End If

    End Sub

    Private Sub InitRadclcConsts(ByRef X1() As Double, ByRef ac(,) As Double, ByRef aXLax(,) As Double)

        Dim i, j As Integer


        ac(1, 1) = 4.0#
        ac(2, 1) = 3.0#
        ac(3, 1) = 0.0#
        ac(4, 1) = -2.0#
        ac(5, 1) = -4.0#
        ac(6, 1) = -5.0#
        ac(7, 1) = -5.0#
        ac(8, 1) = -4.0#
        ac(9, 1) = -2.0#
        ac(10, 1) = 0.0#
        ac(1, 2) = 2.0#
        ac(2, 2) = 4.0#
        ac(3, 2) = 3.5
        ac(4, 2) = 2.5
        ac(5, 2) = 0.5
        ac(6, 2) = -1.5
        ac(7, 2) = -3.5
        ac(8, 2) = -4.5
        ac(9, 2) = -4.0#
        ac(10, 2) = -3.5
        ac(1, 3) = -1.5
        ac(2, 3) = 0.0#
        ac(3, 3) = 1.5
        ac(4, 3) = 3.5
        ac(5, 3) = 3.0#
        ac(6, 3) = 2.0#
        ac(7, 3) = 1.0#
        ac(8, 3) = -1.0#
        ac(9, 3) = -3.0#
        ac(10, 3) = -4.0#
        ac(1, 4) = -3.0#
        ac(2, 4) = -3.0#
        ac(3, 4) = -1.0#
        ac(4, 4) = 0.0#
        ac(5, 4) = 1.0#
        ac(6, 4) = 3.0#
        ac(7, 4) = 3.0#
        ac(8, 4) = 2.5
        ac(9, 4) = 1.0#
        ac(10, 4) = -0.5
        ac(1, 5) = -2.0#
        ac(2, 5) = -2.5
        ac(3, 5) = -2.0#
        ac(4, 5) = -2.0#
        ac(5, 5) = -0.5
        ac(6, 5) = 0.5
        ac(7, 5) = 1.5
        ac(8, 5) = 3.0#
        ac(9, 5) = 3.0#
        ac(10, 5) = 3.0#
        ac(1, 6) = 1.0#
        ac(2, 6) = 0.0#
        ac(3, 6) = -1.0#
        ac(4, 6) = -1.0#
        ac(5, 6) = -1.0#
        ac(6, 6) = -1.0#
        ac(7, 6) = 0.0#
        ac(8, 6) = 1.0#
        ac(9, 6) = 2.0#
        ac(10, 6) = 3.0#
        ac(1, 7) = 3.0#
        ac(2, 7) = 2.0#
        ac(3, 7) = 1.5
        ac(4, 7) = 0.5
        ac(5, 7) = 0.0#
        ac(6, 7) = -0.5
        ac(7, 7) = -0.5
        ac(8, 7) = 0.0#
        ac(9, 7) = 0.5
        ac(10, 7) = 1.5
        ac(1, 8) = 2.5
        ac(2, 8) = 3.0#
        ac(3, 8) = 3.0#
        ac(4, 8) = 3.0#
        ac(5, 8) = 2.0#
        ac(6, 8) = 1.0#
        ac(7, 8) = 1.0#
        ac(8, 8) = 0.0#
        ac(9, 8) = 0.0#
        ac(10, 8) = 1.0#
        ac(1, 9) = 1.0#
        ac(2, 9) = 2.0#
        ac(3, 9) = 3.0#
        ac(4, 9) = 3.0#
        ac(5, 9) = 2.5
        ac(6, 9) = 2.5
        ac(7, 9) = 2.0#
        ac(8, 9) = 1.5
        ac(9, 9) = 1.5
        ac(10, 9) = 1.0#
        ac(1, 10) = 1.0#
        ac(2, 10) = 1.5
        ac(3, 10) = 1.5
        ac(4, 10) = 2.0#
        ac(5, 10) = 2.5
        ac(6, 10) = 2.5
        ac(7, 10) = 2.0#
        ac(8, 10) = 2.0#
        ac(9, 10) = 2.0#
        ac(10, 10) = 2.0#
        ac(1, 11) = 2.0#
        ac(2, 11) = 2.0#
        ac(3, 11) = 2.0#
        ac(4, 11) = 2.0#
        ac(5, 11) = 2.0#
        ac(6, 11) = 2.0#
        ac(7, 11) = 2.0#
        ac(8, 11) = 2.0#
        ac(9, 11) = 1.0#
        ac(10, 11) = 1.0#
        ac(1, 12) = 1.0#
        ac(2, 12) = 1.0#
        ac(3, 12) = 1.0#
        ac(4, 12) = 1.0#
        ac(5, 12) = 1.0#
        ac(6, 12) = 1.0#
        ac(7, 12) = 1.0#
        ac(8, 12) = 1.0#
        ac(9, 12) = 1.0#
        ac(10, 12) = 1.0#

        For i = 1 To 24
            For j = 1 To 6
                aXLax(i, j) = -9999.0#
            Next j
        Next i

        aXLax(25, 1) = 616.17
        aXLax(25, 2) = -147.83
        aXLax(25, 3) = -27.17
        aXLax(25, 4) = -3.17
        aXLax(25, 5) = 11.84
        aXLax(25, 6) = 2.02
        aXLax(26, 1) = 609.97
        aXLax(26, 2) = -154.71
        aXLax(26, 3) = -27.49
        aXLax(26, 4) = -2.97
        aXLax(26, 5) = 12.04
        aXLax(26, 6) = 1.3
        aXLax(27, 1) = 603.69
        aXLax(27, 2) = -161.55
        aXLax(27, 3) = -27.69
        aXLax(27, 4) = -2.78
        aXLax(27, 5) = 12.22
        aXLax(27, 6) = 0.64
        aXLax(28, 1) = 597.29
        aXLax(28, 2) = -168.33
        aXLax(28, 3) = -27.78
        aXLax(28, 4) = -2.6
        aXLax(28, 5) = 12.38
        aXLax(28, 6) = 0.02
        aXLax(29, 1) = 590.81
        aXLax(29, 2) = -175.05
        aXLax(29, 3) = -27.74
        aXLax(29, 4) = -2.43
        aXLax(29, 5) = 12.53
        aXLax(29, 6) = -0.56
        aXLax(30, 1) = 584.21
        aXLax(30, 2) = -181.72
        aXLax(30, 3) = -27.57
        aXLax(30, 4) = -2.28
        aXLax(30, 5) = 12.67
        aXLax(30, 6) = -1.1
        aXLax(31, 1) = 577.53
        aXLax(31, 2) = -188.34
        aXLax(31, 3) = -27.29
        aXLax(31, 4) = -2.14
        aXLax(31, 5) = 12.8
        aXLax(31, 6) = -1.6
        aXLax(32, 1) = 570.73
        aXLax(32, 2) = -194.91
        aXLax(32, 3) = -26.89
        aXLax(32, 4) = -2.02
        aXLax(32, 5) = 12.92
        aXLax(32, 6) = -2.05
        aXLax(33, 1) = 563.85
        aXLax(33, 2) = -201.42
        aXLax(33, 3) = -26.37
        aXLax(33, 4) = -1.91
        aXLax(33, 5) = 13.03
        aXLax(33, 6) = -2.45
        aXLax(34, 1) = 556.85
        aXLax(34, 2) = -207.29
        aXLax(34, 3) = -25.72
        aXLax(34, 4) = -1.81
        aXLax(34, 5) = 13.13
        aXLax(34, 6) = -2.8
        aXLax(35, 1) = 549.77
        aXLax(35, 2) = -214.29
        aXLax(35, 3) = -24.96
        aXLax(35, 4) = -1.72
        aXLax(35, 5) = 13.22
        aXLax(35, 6) = -3.1
        aXLax(36, 1) = 542.57
        aXLax(36, 2) = -220.65
        aXLax(36, 3) = -24.07
        aXLax(36, 4) = -1.64
        aXLax(36, 5) = 13.3
        aXLax(36, 6) = -3.35
        aXLax(37, 1) = 535.3
        aXLax(37, 2) = -226.96
        aXLax(37, 3) = -23.07
        aXLax(37, 4) = -1.59
        aXLax(37, 5) = 13.36
        aXLax(37, 6) = -3.58
        aXLax(38, 1) = 527.9
        aXLax(38, 2) = -233.22
        aXLax(38, 3) = -21.95
        aXLax(38, 4) = -1.55
        aXLax(38, 5) = 13.4
        aXLax(38, 6) = -3.77
        aXLax(39, 1) = 520.44
        aXLax(39, 2) = -239.43
        aXLax(39, 3) = -20.7
        aXLax(39, 4) = -1.52
        aXLax(39, 5) = 13.42
        aXLax(39, 6) = -3.92
        aXLax(40, 1) = 512.84
        aXLax(40, 2) = -245.59
        aXLax(40, 3) = -19.33
        aXLax(40, 4) = -1.51
        aXLax(40, 5) = 13.42
        aXLax(40, 6) = -4.03
        aXLax(41, 1) = 505.19
        aXLax(41, 2) = -251.69
        aXLax(41, 3) = -17.83
        aXLax(41, 4) = -1.51
        aXLax(41, 5) = 13.41
        aXLax(41, 6) = -4.1
        aXLax(42, 1) = 497.4
        aXLax(42, 2) = -257.74
        aXLax(42, 3) = -16.22
        aXLax(42, 4) = -1.52
        aXLax(42, 5) = 13.39
        aXLax(42, 6) = -4.13
        aXLax(43, 1) = 489.52
        aXLax(43, 2) = -263.74
        aXLax(43, 3) = -14.49
        aXLax(43, 4) = -1.54
        aXLax(43, 5) = 13.36
        aXLax(43, 6) = -4.12
        aXLax(44, 1) = 481.53
        aXLax(44, 2) = -269.7
        aXLax(44, 3) = -12.63
        aXLax(44, 4) = -1.57
        aXLax(44, 5) = 13.32
        aXLax(44, 6) = -4.07
        aXLax(45, 1) = 473.45
        aXLax(45, 2) = -275.6
        aXLax(45, 3) = -10.65
        aXLax(45, 4) = -1.63
        aXLax(45, 5) = 13.27
        aXLax(45, 6) = -3.98
        aXLax(46, 1) = 465.27
        aXLax(46, 2) = -281.45
        aXLax(46, 3) = -8.55
        aXLax(46, 4) = -1.71
        aXLax(46, 5) = 13.21
        aXLax(46, 6) = -3.85
        aXLax(47, 1) = 456.99
        aXLax(47, 2) = -287.25
        aXLax(47, 3) = -6.33
        aXLax(47, 4) = -1.8
        aXLax(47, 5) = 13.14
        aXLax(47, 6) = -3.68
        aXLax(48, 1) = 448.61
        aXLax(48, 2) = -292.99
        aXLax(48, 3) = -3.98
        aXLax(48, 4) = -1.9
        aXLax(48, 5) = 13.07
        aXLax(48, 6) = -3.47
        aXLax(49, 1) = 440.14
        aXLax(49, 2) = -298.68
        aXLax(49, 3) = -1.51
        aXLax(49, 4) = -2.01
        aXLax(49, 5) = 13.0#
        aXLax(49, 6) = -3.3
        aXLax(50, 1) = 431.55
        aXLax(50, 2) = -304.32
        aXLax(50, 3) = 1.08
        aXLax(50, 4) = -2.13
        aXLax(50, 5) = 12.92
        aXLax(50, 6) = -3.17
        aXLax(51, 1) = 431.55
        aXLax(51, 2) = -304.32
        aXLax(51, 3) = 1.08
        aXLax(51, 4) = -2.13
        aXLax(51, 5) = 12.92
        aXLax(51, 6) = -3.17

    End Sub

    Public Function DisPrecip(ByVal aDyTSer As atcTimeseries, ByVal aDataSource As atcDataSource, ByVal aHrTSer As atcDataGroup, ByVal aObsTime As Integer, ByVal aTolerance As Double, Optional ByVal aSumFile As String = "") As atcTimeseries
        'aDyTSer - daily time series being disaggregated
        'aHrTser - collection of hourly timeseries used to disaggregate daily
        'aObsTime - observation time of daily precip (1 - 24)
        'aTolerance - tolerance for comparison of hourly daily sums to daily value (%)
        'aSumFile - name of file for output summary info

        Dim lHrPos, i, lMaxHrInd As Integer
        Dim lRndOff, lCarry, lMaxHrVal As Double
        Dim lTmpHrVals(24) As Double
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
        lDisTs.Attributes.Add("Observation Hour", aObsTime)
        lDisTs.Attributes.Add("Data Tolerance", aTolerance)

        'build new date array for hourly TSer, set start date to previous day's Obs Time
        lSDt = aDyTSer.Attributes.GetValue("SJDAY") - 1
        Call J2Date(lSDt, lDate)
        lDate(3) = aObsTime
        aDyTSer.Attributes.SetValue("SJDAY", Date2J(lDate))
        lDisTs.Dates = DisaggDates(aDyTSer, aDataSource)
        lDisTs.numValues = lDisTs.Dates.numValues

        Dim lHrVals(lDisTs.numValues) As Double
        lHrPos = 0
        For lDyInd = 1 To aDyTSer.numValues
            If lOutSumm Then 'output summary message to file
                Call J2Date(aDyTSer.Dates.Value(lDyInd) - 1, lDate)
                WriteLine(lOutFil, "Distributing Daily Data for " & lDate(0) & "/" & lDate(1) & "/" & lDate(2) & ":  Value is " & SignificantDigits(aDyTSer.Value(lDyInd), 4))
            End If
            If aDyTSer.Value(lDyInd) > 0 Then 'something to disaggregate
                'back up a day, then add obs hour to get actual end of period
                lEDt = aDyTSer.Dates.Value(lDyInd) - 1
                Call J2Date(lEDt, lDate)
                lDate(3) = aObsTime
                lEDt = Date2J(lDate)
                lSDt = lEDt - 1
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
                        WriteLine(lOutFil, "  Using Data-set Number:  " & lClosestHrTser.Attributes.GetValue("ID") & ", daily sum = " & SignificantDigits(lClosestDaySum, 4))
                    End If
                    If Math.Abs(lDaySum - aDyTSer.Value(lDyInd)) > lRndOff Then
                        'values not distributed properly
                        s = "Problem distributing " & aDyTSer.Value(lDyInd) & " on " & lDate(1) & "/" & lDate(2) & "/" & lDate(0)
                        If lOutSumm Then
                            WriteLine(lOutFil, "  *** " & s & "  ***")
                        End If
                        rsp = MsgBox(s, MsgBoxStyle.Exclamation + MsgBoxStyle.OkCancel, "Precipitation Disaggregation Problem")
                        If rsp = MsgBoxResult.Cancel Then
                            'lDisTs.errordescription = s
                            Err.Raise(vbObjectError + 513)
                        End If
                    End If
                Else 'no data available at hourly stations,
                    'distribute using triangular distribution
                    Call DistTriang(aDyTSer.Value(lDyInd), lTmpHrVals, retcod)
                    lUsedTriang = lUsedTriang + 1
                    For lHrInd = 1 To 24
                        lHrVals(lHrPos + lHrInd) = lTmpHrVals(lHrInd)
                    Next lHrInd
                    If retcod = -1 Then
                        s = "Unable to distribute this much rain (" & lDaySum & ") using triangular distribution." & "Hourly values will be set to -9.98"
                        rsp = MsgBox(s, MsgBoxStyle.Exclamation + MsgBoxStyle.OkCancel, "Precipitation Disaggregation Problem")
                    ElseIf retcod = -2 Then
                        s = "Problem distributing " & aDyTSer.Value(lDyInd) & " using triangular distribution on " & lDate(1) & "/" & lDate(2) & "/" & lDate(0)
                        rsp = MsgBox(s, MsgBoxStyle.Exclamation + MsgBoxStyle.OkCancel, "Precipitation Disaggregation Problem")
                    End If
                    If lOutSumm Then
                        WriteLine(lOutFil, "  *** No hourly total within tolerance - " & SignificantDigits(aDyTSer.Value(lDyInd), 4) & "  distributed using triangular distribution ***")
                        If retcod <> 0 Then
                            WriteLine(lOutFil, "  *** " & s & " ***")
                        End If
                    End If
                    If rsp = MsgBoxResult.Cancel Then
                        'lDisTs.errordescription = s
                        Err.Raise(vbObjectError + 513 + System.Math.Abs(retcod))
                    End If
                End If
            Else 'no daily value to distribute, fill hourly
                For lHrInd = lHrPos + 1 To lHrPos + 24
                    lHrVals(lHrInd) = 0
                Next lHrInd
            End If
            lHrPos = lHrPos + 24
        Next lDyInd

DisPrecipErrHnd:
        On Error GoTo OuttaHere 'in case there's an error in these statements
        If lOutSumm Then FileClose(lOutFil)
        Array.Copy(lHrVals, 1, lDisTs.Values, 1, lDisTs.numValues)

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

    Public Function DisCliGenPrecip(ByVal aDPrecTSer As atcTimeseries, ByVal aDurTSer As atcTimeseries, ByVal aTimePkTSer As atcTimeseries, ByVal aPeakTSer As atcTimeseries, Optional ByVal aDataSource As atcDataSource = Nothing) As atcTimeseries
        'aDPrecTSer - daily time series being disaggregated
        'aDurTSer - storm duration timeseries (hrs)
        'aTimePkTSer - time to peak timeseries (fraction of duration)
        'aPeakTSer - peak intensity timeseries (normalized relative to average intensity)
        Dim lDisTs As New atcTimeseries(aDataSource)
        Dim lHrPos As Integer
        Dim lDyInd As Integer
        Dim lHrInd As Integer
        Dim lEventDur As Integer
        Dim lEventStart As Integer
        Dim lHrDay(24) As Double
        Dim i As Integer

        CopyBaseAttributes(aDPrecTSer, lDisTs)
        lDisTs.Attributes.SetValue("tu", atcTimeUnit.TUHour)
        lDisTs.Attributes.SetValue("ts", 1)
        lDisTs.Attributes.SetValue("Scenario", "COMPUTED")
        lDisTs.Attributes.SetValue("Constituent", "HPCP")
        lDisTs.Attributes.SetValue("TSTYPE", "HPCP")
        lDisTs.Attributes.SetValue("Description", "Hourly Precipitation disaggregated from Daily CliGen Precip")
        lDisTs.Attributes.AddHistory("Disaggregated CliGen Precipitation - input: DPRC")
        lDisTs.Attributes.Add("DPRC", aDPrecTSer.ToString)

        'build new date array for hourly TSer, set start date to same as daily
        Dim lSJDay As Double = aDPrecTSer.Attributes.GetValue("SJDAY", 0)
        If lSJDay = 0 Then aDPrecTSer.Attributes.SetValue("SJDAY", aDPrecTSer.Dates.Value(0))
        lDisTs.Dates = DisaggDates(aDPrecTSer, aDataSource)
        lDisTs.numValues = lDisTs.Dates.numValues

        Dim lHrVals(lDisTs.numValues) As Double
        lHrPos = 0
        For lDyInd = 1 To aDPrecTSer.numValues
            For lHrInd = lHrPos + 1 To lHrPos + 24
                lHrVals(lHrInd) = 0
            Next lHrInd
            If aDPrecTSer.Value(lDyInd) > 0 Then 'something to disaggregate
                lHrDay = DisCliGenPrecDay(aDPrecTSer.Value(lDyInd), aDurTSer.Value(lDyInd), aTimePkTSer.Value(lDyInd), aPeakTSer.Value(lDyInd))
                If Math.Abs(lHrDay(0) - aDPrecTSer.Value(lDyInd)) > 0.0001 Then
                    Logger.Dbg("Daily CliGen precip value not properly disaggregated." & vbCrLf & _
                               "Daily value: " & aDPrecTSer.Value(lDyInd) & vbCrLf & _
                               "Sum of Hourly values: " & lHrDay(0))
                End If
                'subtract .001 for to make sure whole Dur value (e.g. 12.0) ends up as same duration length (e.g. 12)
                lEventDur = Int(Math.Ceiling(aDurTSer.Value(lDyInd)))
                lEventStart = 12 - lEventDur / 2
                For i = 1 To lEventDur
                    lHrInd = lHrPos + lEventStart + i
                    lHrVals(lHrInd) = lHrDay(i)
                Next
            End If
            lHrPos = lHrPos + 24
        Next
        lDisTs.Values = lHrVals
        Return lDisTs
    End Function

    'DisagCliGenPrec uses double exponential function to disaggregate
    'CliGen daily precip data (precip, duration, time to peak, peak intensity)
    'into hourly values - assumes 1 storm per day
    'borrowed from WEPP code
    'Note: Sum of distributed hourly values is placed in 0th position of returning array
    Public Function DisCliGenPrecDay(ByVal aPrec As Double, ByVal aDur As Double, _
                                     ByVal aPkTime As Double, ByVal aPkIntensity As Double) As Double()
        Dim lNInt As Integer = 11
        Dim lDeltFq As Double = 1 / (lNInt - 1)
        Dim lFq As Double
        Dim lTimeDl(20) As Double
        Dim lIntDl(20) As Double
        Dim lIntPrcp(20) As Double
        Dim lLoopFg As Integer
        Dim lInt As Integer
        Dim lFirstTime As Double
        Dim lSecTime As Double
        Dim lDurSeconds As Double = aDur * 3600
        Dim lHrVals(24) As Double
        Dim lCurTime As Double
        Dim lCurSpan As Double
        Dim lSTime As Double
        Dim lETime As Double
        Dim lNextHr As Integer

        If aPkIntensity < 1 Then aPkIntensity = 1
        If aPkTime > 1 Or aPkIntensity = 1 Then
            aPkTime = 1
        ElseIf aPkTime < 0 Then
            aPkTime = 0.01
        End If
        Do
            lLoopFg = 0
            If aPkTime >= 1 And aPkIntensity <= 1 Then
                ConstInt(lNInt, lDeltFq, lTimeDl, lIntDl)
            Else
                DblEx(aPkIntensity, aPkTime, lNInt, lDeltFq, lTimeDl, lIntDl)
            End If
            lFirstTime = lTimeDl(1) * lDurSeconds
            lInt = 1
            Do
                lInt += 1
                lSecTime = lTimeDl(lInt) * lDurSeconds
                If lSecTime - lFirstTime < 300 Then
                    'Time step is less than 5 minutes.  Decrease the number
                    'of dimensionless steps and try again.
                    lNInt -= 1
                    If lNInt < 2 Then
                        'Disaggregated rainfall distribution is set to
                        'constant intensity with 2 time steps.
                        lTimeDl(2) = 1
                        lIntDl(1) = 1
                        lIntDl(2) = 1
                        lNInt = 2
                    Else 'Re-initialize for decreased step.
                        lDeltFq = 1.0 / (lNInt - 1)
                        lFq = 0.0
                        lTimeDl(1) = 0.0
                        lIntDl(lNInt) = 0.0
                        lLoopFg = 2
                    End If
                Else
                    lFirstTime = lSecTime
                End If

            Loop While (lInt < lNInt And lLoopFg = 0)
        Loop While lLoopFg = 2
        lIntDl(lNInt) = 0 'last intensity value is always 0
        'calculate actual time and intensity values
        For lInt = 1 To lNInt
            lTimeDl(lInt) = lTimeDl(lInt) * aDur 'calculate time in hours
            lIntDl(lInt) = lIntDl(lInt) * aPrec / aDur 'intensity in mm/hr
            If lInt > 1 Then 'calculate total precip for this interval
                lSTime = lTimeDl(lInt - 1)
                lETime = lTimeDl(lInt)
                lCurSpan = lETime - lSTime
                lIntPrcp(lInt - 1) = (lTimeDl(lInt) - lTimeDl(lInt - 1)) * lIntDl(lInt - 1)
                'distribute precip to hourly intervals
                lCurTime = lSTime
                While lCurTime < lETime
                    lNextHr = Int(lCurTime) + 1
                    If lNextHr < lETime Then
                        lHrVals(lNextHr) += ((lNextHr - lCurTime) / lCurSpan) * lIntPrcp(lInt - 1)
                        lCurTime = lNextHr
                    Else
                        lHrVals(lNextHr) += ((lETime - lCurTime) / lCurSpan) * lIntPrcp(lInt - 1)
                        lCurTime = lETime
                    End If
                End While
            End If
        Next
        'put sum of distributed values in 0th position for QA check
        For lInt = 1 To 24
            lHrVals(0) += lHrVals(lInt)
        Next
        Return lHrVals
    End Function

    Private Sub ConstInt(ByVal aNint As Integer, ByVal aDeltFq As Double, _
                         ByRef aTimeDl() As Double, ByRef aIntDl() As Double)
        Dim lFqx As Double '- temporarily holds DELTFQ*I.
        Dim i As Integer

        lFqx = 0.0

        For i = 1 To aNint - 1
            lFqx += aDeltFq
            aTimeDl(i + 1) = lFqx
            aIntDl(i) = 1.0
        Next

    End Sub
    Private Sub DblEx(ByVal aPkIntensity As Double, ByVal aPkTime As Double, _
                      ByVal aNInt As Integer, ByVal aDeltFq As Double, _
                      ByRef aTimeDl As Double(), ByRef aIntDl As Double())
        Dim lErr As Integer '0 - equation solved; 1 - no solution for given A
        Dim i As Integer
        Dim i1 As Integer
        Dim lA As Double 'constant in the equation: 1 - exp(-U) = A*U. (in this routine, A = 1/Ip.)
        Dim lU As Double
        Dim lB As Double 'coefficient in double exponential
        Dim lD As Double 'coefficient in double exponential
        Dim lFqx As Double 'for idntermediate calcs. Starts with the value of 0.

        'Check to make sure Peak Intensity is in range so machine can make the
        'calculations without a machine overflow, make sure Peak Intensity <= 60.0
        If aPkIntensity > 60 Then aPkIntensity = 60
        If aPkTime > 0.99 Then aPkTime = 0.99

        'Newton's method for B and then A in i(t)=a*exp(b*t)
        EqRoot(1.0 / aPkIntensity, lErr, lU)
        lB = lU / aPkTime
        lA = aPkIntensity * Math.Exp(-lU)
        'The formulas for dissagregation give u=btp=d(1-tp).
        lD = lU / (1 - aPkTime)
        'aTimeDl(1) and aIntDl(aNint) are initialized in DISAG.
        aTimeDl(aNInt) = 1.0
        lFqx = 0.0
        For i = 1 To aNInt - 1
            i1 = i + 1
            If i < aNInt - 1 Then
                lFqx = lFqx + aDeltFq
                If lFqx < aPkTime Then
                    aTimeDl(i1) = (1.0 / lB) * Math.Log(1.0 + (lB / lA) * lFqx)
                Else
                    aTimeDl(i1) = aPkTime - (1.0 / lD) * Math.Log(1.0 - (lD / aPkIntensity) * (lFqx - aPkTime))
                End If
            End If
            aIntDl(i) = aDeltFq / (aTimeDl(i1) - aTimeDl(i))
        Next
    End Sub

    'EqRoot Solves the following equation for U: 1 - exp(-u) = a*u, for
    'positive values of A less than 1, and positive values of U.
    '(If A=1, U=0).  Newton's method is used, with special
    'approximations for small values of A (A <= 0.06),  and large
    'values of A (A >= 0.999).
    Private Sub EqRoot(ByVal aA As Double, ByVal aErr As Double, ByRef aEqrt As Double)
        'aA    - constant A in the equation:  1 - exp(-u) = a*u.
        '        (In this routine, A = 1/ip.)
        'aErr  - flag. 0: equation solved.
        '              1: no solution for given A.
        'aEqrt - solution returned for U in the equation:
        '        1 - exp(-u) = a*u.

        Dim lD As Double ' - A - F
        Dim lE As Double ' - exp(-U)
        Dim lF As Double ' - (1 - E) / U
        Dim lR As Double ' - A / TMPVR1
        Dim lS As Double ' - abs(D / A) Or abs(D / TMPVR1)
        Dim lU As Double ' - See definition of A
        Dim lTmpVr1 As Double
        Static lEqRtx As Double ' - saved value of EQRT from prev. call
        Static lAx As Double ' - saved value of A from prev. call
        Dim lLoopFg As Integer ' - Flag.  Set to exit loop.

        'See if value of A has changed since last call to EQROOT.
        If aA <> lAx Then
            'Verify that A is within the valid range of values; ie, 0.0 < A <= 1.0.
            If aA > 0.0 AndAlso aA <= 1.0 Then
                If aA <= 0.06 Then
                    'Small A:  0 < A <= 0.06.  (Answer good to machine precision).
                    aErr = 0
                    aEqrt = 1.0 / aA
                ElseIf aA < 0.999 Then
                    'Usual Case:  0.06 < A < 0.999.
                    'Estimate starting value for U.
                    If aA <= 0.2 Then
                        lU = 1.0 / aA
                    ElseIf aA <= 0.5 Then
                        lU = 0.968732 / aA - 1.55098 * aA + 0.431653
                    ElseIf aA <= 0.94 Then
                        lU = 1.13243 / aA - 0.92824 * aA - 0.207111
                    Else
                        lU = (3.0 / 2.0) - Math.Sqrt(6.0 * aA - (15.0 / 4.0))
                    End If
                    'Iterate.
                    lLoopFg = 0
                    Do
                        lE = Math.Exp(-lU)
                        lF = (1.0 - lE) / lU
                        lD = aA - lF
                        lTmpVr1 = ((lU + 1.0) * lF - 1.0)
                        lR = aA / lTmpVr1
                        If lR <= 1.0 Then
                            lS = Math.Abs(lD / aA)
                        Else
                            lS = Math.Abs(lD / lTmpVr1)
                        End If
                        If lS >= 0.00000059 Then
                            lU = lU * (1.0 + lD / (lE - lF))
                        Else
                            lLoopFg = 1
                        End If
                    Loop While lLoopFg = 0
                    'Exit with solution.
                    aErr = 0
                    aEqrt = lU
                ElseIf aA < 1.0 Then
                    'Large A: 0.999 <= A < 1. (Answer good to about 10 places).
                    aErr = 0
                    aEqrt = (3.0 / 2.0) - Math.Sqrt(6.0 * aA - (15.0 / 4.0))
                Else
                    'Special Case: A=1 (exact limiting solution).
                    aErr = 0
                    aEqrt = 0.0
                End If
                'Save values of A & EQRT.
                lEqRtx = aEqrt
                lAx = aA
            Else
                'Error: A outside range.  A <= 0  or  A > 1.
                aErr = 1
            End If
        Else
            'Value of 'A' same as last time EQROOT was called.
            aEqrt = lEqRtx
        End If

    End Sub
End Module
