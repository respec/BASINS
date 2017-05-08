Attribute VB_Name = "TSMetCompute"
Option Explicit
'Copyright 2000 by AQUA TERRA Consultants

Public Function CmpSol(CldTSer As Collection, LatDeg!) As ATCclsTserData
  'compute daily solar radiation based on daily cloud cover
  Dim i&, dtold&(5), dtnew&(5), SolRad!(), CldCov!()
  Dim lCmpTs As ATCclsTserData, lTserSum As ATTimSerDateSummary

  Call InitCmpTs(CldTSer(1), TUDay, 1, 1, lCmpTs)

  Call J2Date(lCmpTs.dates.Summary.sjday, dtold())
  ReDim SolRad(lCmpTs.dates.Summary.NVALS)
  ReDim CldCov(lCmpTs.dates.Summary.NVALS)
  CldCov = CldTSer(1).Values
  For i = 1 To lCmpTs.dates.Summary.NVALS
    If CldCov(i) <= 0# Then CldCov(i) = 0.000001
    Call RadClc(LatDeg, CldCov(i), dtold(1), dtold(2), SolRad(i))
    Call F90_TIMADD(dtold(0), lCmpTs.dates.Summary.Tu, lCmpTs.dates.Summary.ts, 1, dtnew(0))
    Call CopyI(6, dtnew(), dtold())
  Next i
  lCmpTs.Values = SolRad
  Set CmpSol = lCmpTs

End Function

Public Function CmpJen(ATmpSRad As Collection, cts!(), CTX!, TmpTyp$) As ATCclsTserData
  'compute JENSEN-HAISE - PET
  'ATmpSRad - min/max temp and solar radiation timeseries
  'CTS - monthly variable coefficients
  'CTX - constant coefficient
  'TmpTyp - "F" or "C"

  Dim i&, retcod&, dtold&(5), dtnew&(5)
  Dim AirTmp!(), PanEvp!()
  Dim lCmpTs As ATCclsTserData, tsfil!(3)

  Call InitCmpTs(ATmpSRad(1), TUDay, 1, 1, lCmpTs)

  Call J2Date(lCmpTs.dates.Summary.sjday, dtold())
  ReDim SolRad(lCmpTs.dates.Summary.NVALS)
  For i = 1 To ATmpSRad.Count 'get fill value for input dsns
    tsfil(i) = ATmpSRad(i).AttribNumeric("TSFILL", -999)
  Next i

  ReDim PanEvp(lCmpTs.dates.Summary.NVALS)
  ReDim AirTmp(lCmpTs.dates.Summary.NVALS)
  For i = 1 To lCmpTs.dates.Summary.NVALS
    If Abs(ATmpSRad(1).Value(i) - tsfil(1)) < 0.000001 Or _
       Abs(ATmpSRad(2).Value(i) - tsfil(2)) < 0.000001 Or _
       Abs(ATmpSRad(3).Value(i) - tsfil(3)) < 0.000001 Then
      'missing data
      PanEvp(i) = tsfil(1)
    Else 'compute pet
      AirTmp(i) = (ATmpSRad(1).Value(i) + ATmpSRad(2).Value(i)) / 2
      Call Jensen(dtold(1), cts, AirTmp(i), TmpTyp, CTX, ATmpSRad(3).Value(i), PanEvp(i), retcod)
    End If
    Call F90_TIMADD(dtold(0), lCmpTs.dates.Summary.Tu, lCmpTs.dates.Summary.ts, 1, dtnew(0))
    Call CopyI(6, dtnew, dtold)
  Next i
  lCmpTs.Values = PanEvp
  Set CmpJen = lCmpTs

End Function

Public Function CmpHam(MnMxTmp As Collection, LatDeg!, cts!(), TmpTyp$) As ATCclsTserData
  'compute HAMON - PET
  'MnMxTmp - min/max temp timeseries
  'LatDeg - latitude, in degrees
  'CTS - monthly variable coefficients
  'TmpTyp - "F" or "C"

  Dim i&, dtold&(5), dtnew&(5), retcod&
  Dim AirTmp!(), PanEvp!()
  Dim lCmpTs As ATCclsTserData, tsfil!(2)

  Call InitCmpTs(MnMxTmp(1), TUDay, 1, 1, lCmpTs)

  Call J2Date(lCmpTs.dates.Summary.sjday, dtold())
  For i = 1 To MnMxTmp.Count
    'get fill value for input dsn
    tsfil(i) = MnMxTmp(i).AttribNumeric("TSFILL", -999)
  Next i

  ReDim PanEvp(lCmpTs.dates.Summary.NVALS)
  ReDim AirTmp(lCmpTs.dates.Summary.NVALS)
  For i = 1 To lCmpTs.dates.Summary.NVALS
    If Abs(MnMxTmp(1).Value(i) - tsfil(1)) < 0.000001 Or _
       Abs(MnMxTmp(2).Value(i) - tsfil(2)) < 0.000001 Then
      'missing data
      PanEvp(i) = tsfil(1)
    Else 'compute pet
      AirTmp(i) = (MnMxTmp(1).Value(i) + MnMxTmp(2).Value(i)) / 2
      Call Hamon(dtold(1), dtold(2), cts, LatDeg, AirTmp(i), TmpTyp, PanEvp(i), retcod)
    End If
    Call F90_TIMADD(dtold(0), lCmpTs.dates.Summary.Tu, lCmpTs.dates.Summary.ts, 1, dtnew(0))
    Call CopyI(6, dtnew, dtold)
  Next i
  lCmpTs.Values = PanEvp
  Set CmpHam = lCmpTs

End Function

Public Function CmpPen(InTs As Collection) As ATCclsTserData
  'compute PENMAN - PET
  'InTS - input timeseries (1/2 - Min/Max Temp,
  '3 - Dewpoint Temp, 4 - Wind Movement, 5 - Solar Radiation)

  Dim i&, j&, lret&, PanEvp!()
  Dim lCmpTs As ATCclsTserData, tsfil!(5)

  Call InitCmpTs(InTs(1), TUDay, 1, 1, lCmpTs)

  For i = 1 To InTs.Count
    'get fill value for input dsn
    tsfil(i) = InTs(i).AttribNumeric("TSFILL", -999)
  Next i

  ReDim PanEvp(lCmpTs.dates.Summary.NVALS)
  For i = 1 To lCmpTs.dates.Summary.NVALS
    If Abs(InTs(1).Value(i) - tsfil(1)) < 0.000001 Or _
       Abs(InTs(2).Value(i) - tsfil(2)) < 0.000001 Or _
       Abs(InTs(3).Value(i) - tsfil(3)) < 0.000001 Or _
       Abs(InTs(4).Value(i) - tsfil(4)) < 0.000001 Or _
       Abs(InTs(5).Value(i) - tsfil(5)) < 0.000001 Then
      'missing data
      PanEvp(i) = tsfil(1)
    Else 'compute pet
      Call PNEVAP(InTs(1).Value(i), InTs(2).Value(i), InTs(3).Value(i), InTs(4).Value(i), InTs(5).Value(i), PanEvp(i))
    End If
  Next i
  lCmpTs.Values = PanEvp
  Set CmpPen = lCmpTs

End Function

Public Function CmpCld(PctSun As Collection) As ATCclsTserData
  'compute %cloud cover from %sunshine

  Dim i&, j&, SSSum!, SSDiv!, CldCov!(), SunVals!()
  Dim lCmpTs As ATCclsTserData

  Call InitCmpTs(PctSun(1), TUDay, 1, 1, lCmpTs)

  SunVals = PctSun(1).Values
  'see if sunshine values are percent or fraction
  i = 1
  j = 0
  Do While j < 5 And i <= PctSun(1).dates.Summary.NVALS
    If SunVals(i) > 0 Then
      SSSum = SSSum + SunVals(i)
      j = j + 1
    End If
    i = i + 1
  Loop
  'see what the sum of these values is
  If SSSum > j Then 'must be percentages
    SSDiv = 100
  Else 'must be fractions
    SSDiv = 1
  End If
  ReDim CldCov(lCmpTs.dates.Summary.NVALS)
  For i = 1 To lCmpTs.dates.Summary.NVALS
    If SunVals(i) < 0# Then SunVals(i) = 0#
    CldCov(i) = 10 * ((1 - SunVals(i) / SSDiv) ^ (3 / 5))
    If CldCov(i) < 0# Then CldCov(i) = 0#
  Next i
  lCmpTs.Values = CldCov
  Set CmpCld = lCmpTs

End Function

Public Function CmpWnd(WndSpd As Collection) As ATCclsTserData
  'compute daily total wind travel (mi) from
  'average daily wind speed (mph)

  Dim i&, dtold&(5), dtnew&(5), TotWnd!()
  Dim lCmpTs As ATCclsTserData

  Call InitCmpTs(WndSpd(1), TUDay, 1, 1, lCmpTs)

  ReDim TotWnd(lCmpTs.dates.Summary.NVALS)
  For i = 1 To lCmpTs.dates.Summary.NVALS
    If WndSpd(1).Value(i) <= 0# Then 'not valid wind speed value
      TotWnd(i) = 0
    Else
      TotWnd(i) = 24 * WndSpd(1).Value(i)
    End If
  Next i
  lCmpTs.Values = TotWnd
  Set CmpWnd = lCmpTs

End Function

Public Function DisTemp(MnMxTmp As Collection, ObsTime&, SJDt#, EJDt#) As ATCclsTserData
  'Disaggregate daily min/max temperature to hourly temperature
  'MnMxTmp - input daily min/max temp values, since disagg
  '          method uses either previous or ensuing days values,
  '          these arrays contain two more days of values
  '          than the period being disaggregated
  '  For Min Temp (index 1 of collection), the two extra
  '    values are at the end
  '  For Max Temp (index 2 of collection), there is one extra
  '    value at each end of the data set
  'SJDt - actual start date for output time series
  'EJDt - actual end date for output time series

  Dim i&, j&, opt&, ioff&, joff&, dtnew&(5), HrPos&, lnval&
  Dim HrVals!(1 To 24), HRTemp!(), tsfil!(3)
  Dim MinTmp!(), MaxTmp!(), lDisTs As ATCclsTserData
  Dim lDateSum As ATTimSerDateSummary

  Call InitCmpTs(MnMxTmp(1), TUHour, 1, 24, lDisTs)
  lDateSum = lDisTs.dates.Summary
  'since lDisTs was built based on min temp TSer (which contains
  '2 extra values), need to adjust number of values and end date
  lDateSum.NVALS = 24 * (EJDt - SJDt)
  lDateSum.ejday = EJDt
  lDisTs.dates.Summary = lDateSum

  For i = 1 To MnMxTmp.Count 'get fill value for input dsns
    tsfil(i) = MnMxTmp(i).AttribNumeric("TSFILL", -999)
  Next i

  If ObsTime > 0 And ObsTime <= 6 Then
    'option 1 is early morning observations,
    'use max and min temp from previous calendar day
    opt = 1
  ElseIf ObsTime > 6 And ObsTime <= 16 Then
    'option 2 is mid day observations,
    'use max temp from previous day and min temp of current day
    opt = 2
  ElseIf ObsTime > 16 And ObsTime <= 24 Then
    'option 3 is evening observations,
    'use max and min temp from current day
    opt = 3
  End If

  ReDim HRTemp(lDisTs.dates.Summary.NVALS)
  ReDim MinTmp(MnMxTmp(1).dates.Summary.NVALS + 2)
  ReDim MaxTmp(MnMxTmp(2).dates.Summary.NVALS + 1)

  'get max temp data
  joff = 0
  If opt = 3 Then 'back up one day (try to use 1st array element)
    ioff = 0
    If MnMxTmp(2).dates.Summary.sjday >= SJDt Then
      'no data available prior to start date
      joff = 1
      'fill first max value with first available max value
      MaxTmp(1) = MnMxTmp(2).Value(1)
    End If
  ElseIf MnMxTmp(2).dates.Summary.sjday < SJDt Then
    '1st element preceeds start date, don't need it
    ioff = 1
  Else '1st element is at start date, need it
    ioff = 0
  End If
  For i = 1 To MnMxTmp(2).dates.Summary.NVALS - ioff
    MaxTmp(i + joff) = MnMxTmp(2).Value(i + ioff)
  Next i
  If opt <> 3 And MnMxTmp(2).dates.Summary.ejday <= EJDt Then
    'extra value needed, but not available, fill with previous
    MaxTmp(MnMxTmp(2).dates.Summary.NVALS + 1) = MaxTmp(MnMxTmp(2).dates.Summary.NVALS)
  End If
'  'check end points
'  If opt = 3 And Abs(MaxTmp(1) - tsfil(2)) < 0.000001 Then
'    'first value absent - fill with following day
'    MaxTmp(1) = MaxTmp(2)
'  End If
'  If opt <> 3 And Abs(MaxTmp(MnMxTmp(2).dates.summary.NVals - 1) - tsfil(2)) < 0.000001 Then
'    'last value was absent - fill with previous day
'    MaxTmp(MnMxTmp(2).dates.summary.NVals - 1) = MaxTmp(MnMxTmp(2).dates.summary.NVals - 2)
'  End If

  'get min temp data
  If opt = 1 Then 'move up one day for min temp values
    ioff = 1
  Else
    ioff = 0
  End If
  For i = 1 To MnMxTmp(1).dates.Summary.NVALS - ioff
    MinTmp(i) = MnMxTmp(1).Value(i + ioff)
  Next i
  'check end points
  If MnMxTmp(1).dates.Summary.ejday <= EJDt Then
    If opt = 1 Then 'need 2 extra values at end, fill with last good one
      MinTmp(MnMxTmp(1).dates.Summary.NVALS + 1) = MinTmp(MnMxTmp(1).dates.Summary.NVALS - 1)
    End If
    MinTmp(MnMxTmp(1).dates.Summary.NVALS + 1 - ioff) = MinTmp(MnMxTmp(1).dates.Summary.NVALS - ioff)
  End If
'  If opt = 1 And Abs(MinTmp(MnMxTmp(1).dates.summary.NVals - 2) - tsfil(0)) < 0.000001 Then
'    'next to last value was absent - fill with previous day
'    MinTmp(MnMxTmp(1).dates.summary.NVals - 2) = MinTmp(MnMxTmp(1).dates.summary.NVals - 3)
'  End If
'  If Abs(MinTmp(MnMxTmp(1).dates.summary.NVals - 1) - tsfil(0)) < 0.000001 Then
'    'last value was absent - fill with previous day
'    MinTmp(MnMxTmp(1).dates.summary.NVals - 1) = MinTmp(MnMxTmp(1).dates.summary.NVals - 2)
'  End If

  'get TSFILL value from output DSN
  tsfil(3) = lDisTs.AttribNumeric("TSFILL", -999)

  HrPos = 0
  For i = 1 To (EJDt - SJDt)
    If (Abs(MaxTmp(i) - tsfil(2)) > 0.000001) Or _
       (Abs(MinTmp(i) - tsfil(1)) > 0.000001) Then
      'value is not missing, so distribute
      Call DISTRB(MaxTmp(i), MinTmp(i), MaxTmp(i + 1), MinTmp(i + 1), HrVals())
      For j = 1 To 24
        HRTemp(HrPos + j) = HrVals(j)
      Next j
    Else 'value is missing, so leave hourlies missing
      For j = 1 To 24
        HRTemp(HrPos + j) = tsfil(2)
      Next j
    End If
    HrPos = HrPos + 24
  Next i
  lDisTs.Values = HRTemp
  Set DisTemp = lDisTs

End Function

Public Function DisSolPet(InTs As Collection, DisOpt&, LatDeg!) As ATCclsTserData
  'disaggregate daily SOLAR or PET to hourly
  'InTs - input timeseries to be disaggregated
  'DisOpt = 1 does Solar, DisOpt = 2 does PET
  'LatDeg - latitude, in degrees

  Dim i&, j&, HrPos&, dtnew&(5), dtold&(5), retcod&
  Dim lDisTs As ATCclsTserData, OutTs!(), HrVals!(1 To 24)

  Call InitCmpTs(InTs(1), TUHour, 1, 24, lDisTs)

  ReDim OutTs(lDisTs.dates.Summary.NVALS)
  Call J2Date(lDisTs.dates.Summary.sjday, dtold())
  HrPos = 0
  For i = 1 To InTs(1).dates.Summary.NVALS
    If DisOpt = 1 Then 'solar
      Call RADDST(LatDeg, dtold(1), dtold(2), InTs(1).Value(i), HrVals(), retcod)
    ElseIf DisOpt = 2 Then 'pet
      Call PETDST(LatDeg, dtold(1), dtold(2), InTs(1).Value(i), HrVals(), retcod)
    End If
    For j = 1 To 24
      OutTs(HrPos + j) = HrVals(j)
    Next j
    Call F90_TIMADD(dtold(0), InTs(1).dates.Summary.Tu, InTs(1).dates.Summary.ts, 1, dtnew(0))
    Call CopyI(6, dtnew, dtold)
    'increment to next 24 hour period
    HrPos = HrPos + 24
  Next i
  lDisTs.Values = OutTs
  Set DisSolPet = lDisTs

End Function

Public Function DisWnd(InTs As Collection, DCurve!()) As ATCclsTserData
  'Disaggregate daily wind to hourly
  'InTs - input daily wind timeseries
  'DCurve - hourly diurnal curve for wind disaggregation

  Dim i&, j&, k&, lnval&
  Dim lDisTs As ATCclsTserData, OutTs!(), HrVals!(1 To 24)

  Call InitCmpTs(InTs(1), TUHour, 1, 24, lDisTs)

  ReDim OutTs(lDisTs.dates.Summary.NVALS)
  k = 0
  For i = 1 To InTs(1).dates.Summary.NVALS
    For j = 1 To 24
      OutTs(k + j) = InTs(1).Value(i) * DCurve(j)
    Next j
    'increment to next 24 hour period
    k = k + 24
  Next i
  lDisTs.Values = OutTs
  Set DisWnd = lDisTs

End Function

Public Function DisDwpnt(InTs As Collection) As ATCclsTserData
  'Disaggregate dewpoint temperature from daily to hourly
  'assuming daily average is constant for 24 hours

  Dim i&, j&, k&, lnval&
  Dim lDisTs As ATCclsTserData, OutTs!()

  Call InitCmpTs(InTs(1), TUHour, 1, 24, lDisTs)
  ReDim OutTs(lDisTs.dates.Summary.NVALS)

  k = 1
  For i = 1 To InTs(1).dates.Summary.NVALS
    For j = 0 To 23
      OutTs(k + j) = InTs(1).Value(i)
    Next j
    'increment to next 24 hour period
    k = k + 24
  Next i
  lDisTs.Values = OutTs
  Set DisDwpnt = lDisTs

End Function
Public Sub InitCmpTs(InTs As ATCclsTserData, NTu&, NTs&, NewIntvls&, _
                     NewTs As ATCclsTserData)
  'InTs - input TSer upon which new TSer is based
  'NTu/NTs - new TSer's time units and time step
  'NewIntvls - number of new time intervals between InTs and NewTs
  '           (e.g. 24 if going from daily to hourly,
  '                 15 if going from hourly to 15-minute)
  'NewTs - new TSer being built

  Dim lTserSum As ATTimSerDateSummary

  Set NewTs = New ATCclsTserData
  Set NewTs.dates = New ATCclsTserDate
  With lTserSum
    .CIntvl = True
    .Intvl = InTs.dates.Summary.Intvl / NewIntvls
    .NVALS = InTs.dates.Summary.NVALS * NewIntvls
    .sjday = InTs.dates.Summary.sjday
    .ejday = InTs.dates.Summary.ejday
    .ts = NTs
    .Tu = NTu
  End With
  NewTs.dates.Summary = lTserSum
  Set NewTs.File = TserMemory
  TserMemory.addtimser NewTs

End Sub
Public Sub DISTRB(PreMax!, CurMin!, CurMax!, NxtMin!, HRTemp!())

    'Distribute daily max-min temperatures to hourly values

    'PREMAX - previous max temperature
    'CURMIN - current min temperature
    'CURMAX - current max temperature
    'NXTMIN - next min temperature
    'HRTEMP - array of hourly values

    Dim Dif1!, Dif2!, Dif3!

    Dif1 = PreMax - CurMin
    Dif2 = CurMin - CurMax
    Dif3 = CurMax - NxtMin

    HRTemp(1) = CurMin + Dif1 * 0.15
    HRTemp(2) = CurMin + Dif1 * 0.1
    HRTemp(3) = CurMin + Dif1 * 0.06
    HRTemp(4) = CurMin + Dif1 * 0.03
    HRTemp(5) = CurMin + Dif1 * 0.01
    HRTemp(6) = CurMin
    HRTemp(7) = CurMin - Dif2 * 0.16
    HRTemp(8) = CurMin - Dif2 * 0.31
    HRTemp(9) = CurMin - Dif2 * 0.45
    HRTemp(10) = CurMin - Dif2 * 0.59
    HRTemp(11) = CurMin - Dif2 * 0.71
    HRTemp(12) = CurMin - Dif2 * 0.81
    HRTemp(13) = CurMin - Dif2 * 0.89
    HRTemp(14) = CurMin - Dif2 * 0.95
    HRTemp(15) = CurMin - Dif2 * 0.99
    HRTemp(16) = CurMax
    HRTemp(17) = NxtMin + Dif3 * 0.89
    HRTemp(18) = NxtMin + Dif3 * 0.78
    HRTemp(19) = NxtMin + Dif3 * 0.67
    HRTemp(20) = NxtMin + Dif3 * 0.57
    HRTemp(21) = NxtMin + Dif3 * 0.47
    HRTemp(22) = NxtMin + Dif3 * 0.38
    HRTemp(23) = NxtMin + Dif3 * 0.29
    HRTemp(24) = NxtMin + Dif3 * 0.22

End Sub

Private Sub RADDST(LatDeg!, Month&, Day&, DayRad!, HrRad!(), retcod&)
    'Distributes daily solar radiation to hourly
    'values, based on a method used in HSP (Hydrocomp, 1976).
    'It uses the latitude, month, day, and daily radiation.

    'LatDeg - latitude(degrees)
    'MONTH  - month of the year (1-12)
    'DAY    - day of the month (1-31)
    'DAYRAD - input daily radiation (langleys)
    'HRRAD  - output array of hourly radiation (langleys)
    'RETCOD - return code (0 = ok, -1 = bad latitude)

    Dim IK&
    Dim RK!, LatRdn!, JulDay!, Phi!, AD!, SS!, CS!, _
        X2!, Delt!, SunR!, DTR2!, DTR4!, CRAD!, SL!, _
        TRise!, TR2!, TR3!, TR4!

    'julian date
    JulDay = 30.5 * (Month - 1) + Day

    'check latitude
    If LatDeg < -66.5 Or LatDeg > 66.5 Then 'invalid latitude, return
      retcod = -1
    Else 'latitude ok
      'convert to radians
      LatRdn = LatDeg * 0.0174582

      Phi = LatRdn
      AD = 0.40928 * Cos(0.0172141 * (172# - JulDay))
      SS = Sin(Phi) * Sin(AD)
      CS = Cos(Phi) * Cos(AD)
      X2 = -SS / CS
      Delt = 7.6394 * (1.5708 - Atn(X2 / Sqr(1# - X2 ^ 2)))
      SunR = 12# - Delt / 2#

      'develop hourly distribution given sunrise,
      'sunset and length of day (DELT)
      DTR2 = Delt / 2#
      DTR4 = Delt / 4#
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
                HrRad(IK) = 0#
              Else
                HrRad(IK) = (CRAD - (RK - TR3) * SL) * DayRad
              End If
            Else
              HrRad(IK) = CRAD * DayRad
            End If
          Else
            HrRad(IK) = (RK - TRise) * SL * DayRad
          End If
        Else
          HrRad(IK) = 0#
        End If
      Next IK
      retcod = 0
    End If

End Sub

Public Sub PETDST(LatDeg!, Month&, Day&, DayPet!, HrPet!(), retcod&)

    'Distributes daily PET to hourly values,
    'based on a method used to disaggregate solar radiation
    'in HSP (Hydrocomp, 1976) using latitude, month, day,
    'and daily PET.

    'LatDeg - latitude(degrees)
    'MONTH  - month of the year (1-12)
    'DAY    - day of the month (1-31)
    'DAYPET - input daily PET (inches)
    'HRPET  - output array of hourly PET (inches)
    'RETCOD - return code (0 = ok, -1 = bad latitude)

    Dim IK&
    Dim RK!, LatRdn!, JulDay!, Phi!, AD!, SS!, CS!, _
        X2!, Delt!, SunR!, DTR2!, DTR4!, CRAD!, SL!, _
        TRise!, TR2!, TR3!, TR4!, CURVE!(24)

    'julian date
    JulDay = 30.5 * (Month - 1) + Day

    'check latitude
    If LatDeg < -66.5 Or LatDeg > 66.5 Then 'invalid latitude, return
      retcod = -1
    Else 'latitude ok
      'convert to radians
      LatRdn = LatDeg * 0.0174582

      Phi = LatRdn
      AD = 0.40928 * Cos(0.0172141 * (172# - JulDay))
      SS = Sin(Phi) * Sin(AD)
      CS = Cos(Phi) * Cos(AD)
      X2 = -SS / CS
      Delt = 7.6394 * (1.5708 - Atn(X2 / Sqr(1# - X2 ^ 2)))
      SunR = 12# - Delt / 2#

      'develop hourly distribution given sunrise,
      'sunset and length of day (DELT)
      DTR2 = Delt / 2#
      DTR4 = Delt / 4#
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
                CURVE(IK) = 0#
                HrPet(IK) = CURVE(IK)
              Else
                CURVE(IK) = (CRAD - (RK - TR3) * SL)
                HrPet(IK) = CURVE(IK) * DayPet
              End If
            Else
              CURVE(IK) = CRAD
              HrPet(IK) = CURVE(IK) * DayPet
            End If
          Else
            CURVE(IK) = (RK - TRise) * SL
            HrPet(IK) = CURVE(IK) * DayPet
          End If
        Else
          CURVE(IK) = 0#
          HrPet(IK) = CURVE(IK)
        End If
      Next IK
      retcod = 0
    End If

End Sub

Public Sub RadClc(DegLat!, Cloud!, Month&, Day&, DayRad!)

    'This routine computes the total daily solar radiation based on
    'the HSPII (Hydrocomp, 1978) RADIATION procedure, which is based
    'on empirical curves of radiation as a function of latitude
    '(Hamon et al, 1954, Monthly Weather Review 82(6):141-146.

    Dim ILat&, ii&, i&, j&, inifg%
    Dim lLat#, Lat1#, Lat2#, Lat3#, Lat4#
    Dim Exp1#, Exp2#, a#, b#, A0#, A1#, A2#
    Dim A3#, b1#, b2#, Frac#, XLax#(51, 6)
    Dim c#(10, 12), x#, SS#, CldDP#
    Dim X1#(12), YRD#, Y100#, y#

    If inifg = 0 Then 'init constant arrays
      Call InitRadclcConsts(X1(), c(), XLax())
      inifg = 1
    End If

    'assign values to local variables LAT and CLDDP (double precision)
    lLat = CDbl(DegLat)
    CldDP = CDbl(Cloud)
    'integer part of latitude
    ILat = Int(lLat)

    'fractional part of latitude
    Frac = lLat - CSng(ILat)
    If Frac <= 0.0001 Then Frac = 0#

    A0 = XLax(ILat, 1) + Frac * (XLax(ILat + 1, 1) - XLax(ILat, 1))
    A1 = XLax(ILat, 2) + Frac * (XLax(ILat + 1, 2) - XLax(ILat, 2))
    A2 = XLax(ILat, 3) + Frac * (XLax(ILat + 1, 3) - XLax(ILat, 3))
    A3 = XLax(ILat, 4) + Frac * (XLax(ILat + 1, 4) - XLax(ILat, 4))
    b1 = XLax(ILat, 5) + Frac * (XLax(ILat + 1, 5) - XLax(ILat, 5))
    b2 = XLax(ILat, 6) + Frac * (XLax(ILat + 1, 6) - XLax(ILat, 6))
    b = lLat - 44#
    a = lLat - 25#
    Exp1 = 0.7575 - 0.0018 * a
    Exp2 = 0.725 + 0.00288 * b
    Lat1 = 2.139 + 0.0423 * a
    Lat2 = 30# - 0.667 * a
    Lat3 = 2.9 - 0.0629 * b
    Lat4 = 18# + 0.833 * b

    'Percent sunshine
    SS = 100# * (1# - (CldDP / 10#) ^ (5# / 3#))
    If SS < 0# Then
      'can't have SS being negative
      SS = 0#
    End If

    x = X1(Month) + Day
    'convert to radians
    x = x * 2# * 3.14159 / 360#

    Y100 = A0 + A1 * Cos(x) + A2 * Cos(2 * x) + A3 * Cos(3 * x) + b1 * Sin(x) + b2 * Sin(2 * x)

    ii = CEIL((SS + 10#) / 10#)

    If lLat > 43# Then
      YRD = Lat3 * SS ^ Exp2 + Lat4
    Else
      YRD = Lat1 * SS ^ Exp1 + Lat2
    End If

    If ii < 11 Then
      YRD = YRD + c(ii, Month)
    End If

    If YRD >= 100# Then
      y = Y100
    Else
      y = Y100 * YRD / 100#
    End If

    'assign single precision value of radiation for return
    DayRad = CSng(y)

End Sub
      
Public Function CEIL(x#) As Long

    'This routine returns the next higher integer
    'from the input double precision argument X.

    If (x - CDbl(Fix(x))) <= 0.00001 Then
      CEIL = Fix(x)
    Else
      CEIL = Fix(x) + 1
    End If

End Function
Public Sub Jensen(Month&, cts!(), AirTmp!, TmpTyp$, CTX!, SolRad!, PanEvp!, retcod&)

    'Generates daily pan evaporation (inches)
    'using a coefficient for the month, the daily average air
    'temperature (F), a coefficient, and solar radiation
    '(langleys/day). The computations are based on the Jensen
    'and Haise (1963) formula.

    'CTS    - array of monthly coefficients
    'AirTmp - daily average air temperature (F)
    'TmpTyp - temperature type, C or F
    'CTX - coefficient
    'SolRad - solar radiation (langleys/day)
    'PanEvp - daily pan evaporation (inches)
    'RetCod - return code
    '          0 - operation successful
    '         -1 - operation failed

    Dim SRadIn!, TAVF!, TAVC!

    retcod = 0

    If TmpTyp = "C" Then 'input is celsius
      TAVC = AirTmp
      TAVF = (AirTmp * (9# / 5#)) + 32#
    Else 'input is fahrenheit
      TAVF = AirTmp
      TAVC = (AirTmp - 32#) * 5# / 9#
    End If

    'convert solar radiation from langleys to equivalent inches of
    'water evaporation
    SRadIn = SolRad / ((597.3 - 0.57 * TAVC) * 2.54)

    'compute evaporation using Jensen-Haise (1963) formula
    PanEvp = cts(Month) * (TAVF - CTX) * SRadIn

    'when the estimated pan evaporation
    'is negative the value is set to zero
    If PanEvp < 0# Then
      PanEvp = 0#
    End If

End Sub
Public Sub Hamon(Month&, Day&, cts!(), LatDeg!, TAVC!, TmpTyp$, PanEvp!, retcod&)

    'Generates daily pan evaporation (inches)
    'using a coefficient for the month, the possible hours of
    'sunshine (computed from latitude), and absolute humidity.
    'The computations are based on the Hamon (1961) formula.

    'CTS    - array of monthly coefficients
    'LatDeg - latitude
    'TAVC   - Average daily temperature (C)
    'TmpTyp - temperature type, C or F
    'PanEvp - daily pan evaporation (inches)
    'RetCod - return code
    '          0 - operation successful
    '         -1 - operation failed

    Dim LatRdn!, JulDay!, Phi!, AD!, SS!, CS!, _
        X2!, Delt!, SunR!, SUNS!, DYL!, VDSAT!, VPSAT!

      retcod = 0

    'julian date
    JulDay = 30.5 * (Month - 1) + Day

    'check latitude
    If LatDeg < -66.5 Or LatDeg > 66.5 Then 'invalid latitude, return
      retcod = -1
    Else 'latitude ok,convert to radians
      LatRdn = LatDeg * 0.0174582
      Phi = LatRdn
      AD = 0.40928 * Cos(0.0172141 * (172# - JulDay))
      SS = Sin(Phi) * Sin(AD)
      CS = Cos(Phi) * Cos(AD)
      X2 = -SS / CS
      Delt = 7.6394 * (1.5708 - Atn(X2 / Sqr(1# - X2 ^ 2)))
      SunR = 12# - Delt / 2#
      SUNS = 12# + Delt / 2#
      DYL = (SUNS - SunR) / 12

      'convert temperature to Centigrade if necessary
      If TmpTyp = "F" Then TAVC = (TAVC - 32#) * (5# / 9#)

      'Hamon equation
      VPSAT = 6.108 * Exp(17.26939 * TAVC / (TAVC + 237.3))
      VDSAT = 216.7 * VPSAT / (TAVC + 273.3)
      PanEvp = cts(Month) * DYL * DYL * VDSAT

      'when the estimated pan evaporation is negative
      'the value is set to zero
      If PanEvp < 0# Then
        PanEvp = 0#
      End If
    End If

End Sub

Public Sub PNEVAP(MinTmp!, MaxTmp!, DewTmp!, WindSp!, SolRad!, PanEvp!)

    'Generates daily pan evaporation (inches) using
    'daily minimum air temperature (F), daily maximum air
    'temperature, dewpoint (F), wind movement (miles/day), and solar
    'radiation (langleys/day). The computations are based on the Penman
    '(1948) formula and the method of Kohler, Nordensen, and Fox (1955).

    'MinTmp - daily minimum air temperature (F)
    'MaxTmp - daily maximum air temperature (F)
    'DewTmp - dewpoint(F)
    'WindSp - wind movement (miles/day)
    'SolRad - solar radiation (langleys/day)
    'PanEvp - daily pan evaporation (inches)

    Dim Delta#, EsMiEa#, EaGama#, QNDelt#, AirTmp#

    'compute average daily air temperature
    AirTmp = (MinTmp + MaxTmp) / 2#

    'net radiation exchange * delta
    If SolRad <= 0# Then SolRad = 0.00001
    QNDelt = Exp((AirTmp - 212#) * (0.1024 - 0.01066 * Log(SolRad))) - 0.0001

    'Vapor pressure deficit between surface and
    'dewpoint temps(Es-Ea) IN of Hg
    EsMiEa = (6413252# * Exp(-7482.6 / (AirTmp + 398.36))) - (6413252# * Exp(-7482.6 / (DewTmp + 398.36)))

    'pan evaporation assuming air temp equals water surface temp.

    'when vapor pressure deficit turns negative it is set equal to zero
    If EsMiEa < 0# Then
      EsMiEa = 0#
    End If

    'pan evap * GAMMA, GAMMA = 0.0105 inch Hg/F
    EaGama = 0.0105 * (EsMiEa ^ 0.88) * (0.37 + 0.0041 * WindSp)

    'Delta = slope of saturation vapor pressure curve at air temperature
    Delta = 47987800000# * Exp(-7482.6 / (AirTmp + 398.36)) / ((AirTmp + 398.36) ^ 2)

    'pan evaporation rate in inches per day
    PanEvp = (QNDelt + EaGama) / (Delta + 0.0105)

    'when the estimated pan evaporation is negative
    'the value is set to zero
    If PanEvp < 0# Then
      PanEvp = 0#
    End If

End Sub

Public Sub InitRadclcConsts(X1#(), c#(), XLax#())

    Dim i&, j&

    X1(1) = 10.00028
    X1(2) = 41.0003
    X1(3) = 69.22113
    X1(4) = 100.5259
    X1(5) = 130.8852
    X1(6) = 161.2853
    X1(7) = 191.7178
    X1(8) = 222.1775
    X1(9) = 253.66
    X1(10) = 281.1629
    X1(11) = 309.6838
    X1(12) = 341.221

    c(1, 1) = 4#
    c(2, 1) = 3#
    c(3, 1) = 0#
    c(4, 1) = -2#
    c(5, 1) = -4#
    c(6, 1) = -5#
    c(7, 1) = -5#
    c(8, 1) = -4#
    c(9, 1) = -2#
    c(10, 1) = 0#
    c(1, 2) = 2#
    c(2, 2) = 4#
    c(3, 2) = 3.5
    c(4, 2) = 2.5
    c(5, 2) = 0.5
    c(6, 2) = -1.5
    c(7, 2) = -3.5
    c(8, 2) = -4.5
    c(9, 2) = -4#
    c(10, 2) = -3.5
    c(1, 3) = -1.5
    c(2, 3) = 0#
    c(3, 3) = 1.5
    c(4, 3) = 3.5
    c(5, 3) = 3#
    c(6, 3) = 2#
    c(7, 3) = 1#
    c(8, 3) = -1#
    c(9, 3) = -3#
    c(10, 3) = -4#
    c(1, 4) = -3#
    c(2, 4) = -3#
    c(3, 4) = -1#
    c(4, 4) = 0#
    c(5, 4) = 1#
    c(6, 4) = 3#
    c(7, 4) = 3#
    c(8, 4) = 2.5
    c(9, 4) = 1#
    c(10, 4) = -0.5
    c(1, 5) = -2#
    c(2, 5) = -2.5
    c(3, 5) = -2#
    c(4, 5) = -2#
    c(5, 5) = -0.5
    c(6, 5) = 0.5
    c(7, 5) = 1.5
    c(8, 5) = 3#
    c(9, 5) = 3#
    c(10, 5) = 3#
    c(1, 6) = 1#
    c(2, 6) = 0#
    c(3, 6) = -1#
    c(4, 6) = -1#
    c(5, 6) = -1#
    c(6, 6) = -1#
    c(7, 6) = 0#
    c(8, 6) = 1#
    c(9, 6) = 2#
    c(10, 6) = 3#
    c(1, 7) = 3#
    c(2, 7) = 2#
    c(3, 7) = 1.5
    c(4, 7) = 0.5
    c(5, 7) = 0#
    c(6, 7) = -0.5
    c(7, 7) = -0.5
    c(8, 7) = 0#
    c(9, 7) = 0.5
    c(10, 7) = 1.5
    c(1, 8) = 2.5
    c(2, 8) = 3#
    c(3, 8) = 3#
    c(4, 8) = 3#
    c(5, 8) = 2#
    c(6, 8) = 1#
    c(7, 8) = 1#
    c(8, 8) = 0#
    c(9, 8) = 0#
    c(10, 8) = 1#
    c(1, 9) = 1#
    c(2, 9) = 2#
    c(3, 9) = 3#
    c(4, 9) = 3#
    c(5, 9) = 2.5
    c(6, 9) = 2.5
    c(7, 9) = 2#
    c(8, 9) = 1.5
    c(9, 9) = 1.5
    c(10, 9) = 1#
    c(1, 10) = 1#
    c(2, 10) = 1.5
    c(3, 10) = 1.5
    c(4, 10) = 2#
    c(5, 10) = 2.5
    c(6, 10) = 2.5
    c(7, 10) = 2#
    c(8, 10) = 2#
    c(9, 10) = 2#
    c(10, 10) = 2#
    c(1, 11) = 2#
    c(2, 11) = 2#
    c(3, 11) = 2#
    c(4, 11) = 2#
    c(5, 11) = 2#
    c(6, 11) = 2#
    c(7, 11) = 2#
    c(8, 11) = 2#
    c(9, 11) = 1#
    c(10, 11) = 1#
    c(1, 12) = 1#
    c(2, 12) = 1#
    c(3, 12) = 1#
    c(4, 12) = 1#
    c(5, 12) = 1#
    c(6, 12) = 1#
    c(7, 12) = 1#
    c(8, 12) = 1#
    c(9, 12) = 1#
    c(10, 12) = 1#

    For i = 1 To 24
      For j = 1 To 6
        XLax(i, j) = -9999#
      Next j
    Next i

    XLax(25, 1) = 616.17
    XLax(25, 2) = -147.83
    XLax(25, 3) = -27.17
    XLax(25, 4) = -3.17
    XLax(25, 5) = 11.84
    XLax(25, 6) = 2.02
    XLax(26, 1) = 609.97
    XLax(26, 2) = -154.71
    XLax(26, 3) = -27.49
    XLax(26, 4) = -2.97
    XLax(26, 5) = 12.04
    XLax(26, 6) = 1.3
    XLax(27, 1) = 603.69
    XLax(27, 2) = -161.55
    XLax(27, 3) = -27.69
    XLax(27, 4) = -2.78
    XLax(27, 5) = 12.22
    XLax(27, 6) = 0.64
    XLax(28, 1) = 597.29
    XLax(28, 2) = -168.33
    XLax(28, 3) = -27.78
    XLax(28, 4) = -2.6
    XLax(28, 5) = 12.38
    XLax(28, 6) = 0.02
    XLax(29, 1) = 590.81
    XLax(29, 2) = -175.05
    XLax(29, 3) = -27.74
    XLax(29, 4) = -2.43
    XLax(29, 5) = 12.53
    XLax(29, 6) = -0.56
    XLax(30, 1) = 584.21
    XLax(30, 2) = -181.72
    XLax(30, 3) = -27.57
    XLax(30, 4) = -2.28
    XLax(30, 5) = 12.67
    XLax(30, 6) = -1.1
    XLax(31, 1) = 577.53
    XLax(31, 2) = -188.34
    XLax(31, 3) = -27.29
    XLax(31, 4) = -2.14
    XLax(31, 5) = 12.8
    XLax(31, 6) = -1.6
    XLax(32, 1) = 570.73
    XLax(32, 2) = -194.91
    XLax(32, 3) = -26.89
    XLax(32, 4) = -2.02
    XLax(32, 5) = 12.92
    XLax(32, 6) = -2.05
    XLax(33, 1) = 563.85
    XLax(33, 2) = -201.42
    XLax(33, 3) = -26.37
    XLax(33, 4) = -1.91
    XLax(33, 5) = 13.03
    XLax(33, 6) = -2.45
    XLax(34, 1) = 556.85
    XLax(34, 2) = -207.29
    XLax(34, 3) = -25.72
    XLax(34, 4) = -1.81
    XLax(34, 5) = 13.13
    XLax(34, 6) = -2.8
    XLax(35, 1) = 549.77
    XLax(35, 2) = -214.29
    XLax(35, 3) = -24.96
    XLax(35, 4) = -1.72
    XLax(35, 5) = 13.22
    XLax(35, 6) = -3.1
    XLax(36, 1) = 542.57
    XLax(36, 2) = -220.65
    XLax(36, 3) = -24.07
    XLax(36, 4) = -1.64
    XLax(36, 5) = 13.3
    XLax(36, 6) = -3.35
    XLax(37, 1) = 535.3
    XLax(37, 2) = -226.96
    XLax(37, 3) = -23.07
    XLax(37, 4) = -1.59
    XLax(37, 5) = 13.36
    XLax(37, 6) = -3.58
    XLax(38, 1) = 527.9
    XLax(38, 2) = -233.22
    XLax(38, 3) = -21.95
    XLax(38, 4) = -1.55
    XLax(38, 5) = 13.4
    XLax(38, 6) = -3.77
    XLax(39, 1) = 520.44
    XLax(39, 2) = -239.43
    XLax(39, 3) = -20.7
    XLax(39, 4) = -1.52
    XLax(39, 5) = 13.42
    XLax(39, 6) = -3.92
    XLax(40, 1) = 512.84
    XLax(40, 2) = -245.59
    XLax(40, 3) = -19.33
    XLax(40, 4) = -1.51
    XLax(40, 5) = 13.42
    XLax(40, 6) = -4.03
    XLax(41, 1) = 505.19
    XLax(41, 2) = -251.69
    XLax(41, 3) = -17.83
    XLax(41, 4) = -1.51
    XLax(41, 5) = 13.41
    XLax(41, 6) = -4.1
    XLax(42, 1) = 497.4
    XLax(42, 2) = -257.74
    XLax(42, 3) = -16.22
    XLax(42, 4) = -1.52
    XLax(42, 5) = 13.39
    XLax(42, 6) = -4.13
    XLax(43, 1) = 489.52
    XLax(43, 2) = -263.74
    XLax(43, 3) = -14.49
    XLax(43, 4) = -1.54
    XLax(43, 5) = 13.36
    XLax(43, 6) = -4.12
    XLax(44, 1) = 481.53
    XLax(44, 2) = -269.7
    XLax(44, 3) = -12.63
    XLax(44, 4) = -1.57
    XLax(44, 5) = 13.32
    XLax(44, 6) = -4.07
    XLax(45, 1) = 473.45
    XLax(45, 2) = -275.6
    XLax(45, 3) = -10.65
    XLax(45, 4) = -1.63
    XLax(45, 5) = 13.27
    XLax(45, 6) = -3.98
    XLax(46, 1) = 465.27
    XLax(46, 2) = -281.45
    XLax(46, 3) = -8.55
    XLax(46, 4) = -1.71
    XLax(46, 5) = 13.21
    XLax(46, 6) = -3.85
    XLax(47, 1) = 456.99
    XLax(47, 2) = -287.25
    XLax(47, 3) = -6.33
    XLax(47, 4) = -1.8
    XLax(47, 5) = 13.14
    XLax(47, 6) = -3.68
    XLax(48, 1) = 448.61
    XLax(48, 2) = -292.99
    XLax(48, 3) = -3.98
    XLax(48, 4) = -1.9
    XLax(48, 5) = 13.07
    XLax(48, 6) = -3.47
    XLax(49, 1) = 440.14
    XLax(49, 2) = -298.68
    XLax(49, 3) = -1.51
    XLax(49, 4) = -2.01
    XLax(49, 5) = 13#
    XLax(49, 6) = -3.3
    XLax(50, 1) = 431.55
    XLax(50, 2) = -304.32
    XLax(50, 3) = 1.08
    XLax(50, 4) = -2.13
    XLax(50, 5) = 12.92
    XLax(50, 6) = -3.17
    XLax(51, 1) = 431.55
    XLax(51, 2) = -304.32
    XLax(51, 3) = 1.08
    XLax(51, 4) = -2.13
    XLax(51, 5) = 12.92
    XLax(51, 6) = -3.17

End Sub

Public Function DisPrecip(DyTSer As ATCclsTserData, HrTSer As Collection, ObsTime&, Tolerance!, Optional SumFile As String = "") As ATCclsTserData
  'DyTSer - daily time series being disaggregated
  'HrTser - collection of hourly timeseries used to disaggregate daily
  'ObsTime - observation time of daily precip (1 - 24)
  'Tolerance - tolerance for comparison of hourly daily sums to daily value (0.01 - 1.0)
  'SumFile - name of file for output summary info

  Dim i&, HrPos&, HrVals!(), CARRY!, RndOff!, MaxHrVal!, MaxHrInd&
  Dim DyInd&, HrInd&, DaySum!, ClosestDaySum!, Ratio!, ClosestRatio!
  Dim sdt#, edt#, tmpdate&(5), OutSumm As Boolean, OutFil&, s$
  Dim lHrVals!(24), retcod&, rsp&, UsedTriang&
  Dim ClosestHrTser As ATCclsTserData
  Dim vHrTser As Variant, lHrTser As ATCclsTserData
  Dim DaySumHrTser As ATCclsTserData
  Dim lDisTs As ATCclsTserData, lDateSum As ATTimSerDateSummary

  On Error GoTo DisPrecipErrHnd
  UsedTriang = 0
  RndOff = 0.001
  If Len(SumFile) > 0 Then
    OutSumm = True
    OutFil = FreeFile(0)
    Open SumFile For Output As #OutFil
  Else
    OutSumm = False
  End If
  Call InitCmpTs(DyTSer, TUHour, 1, 24, lDisTs)
  'adjust start/end date based on Obstime
  lDateSum = lDisTs.dates.Summary
  Call J2Date(lDateSum.sjday, tmpdate())
  tmpdate(3) = ObsTime
  'back up exactly one day for start date
'replace following code with sdt=edt-1
'      For i = 1 To 23
'        Call F90_TIMBAK(3, tmpdate(0))
'      Next i
'      lDisTs.dates.summary.SJDay = Date2J(tmpdate)
  lDateSum.sjday = Date2J(tmpdate()) - 1
  Call J2Date(lDateSum.ejday, tmpdate())
  tmpdate(3) = ObsTime
  lDateSum.ejday = Date2J(tmpdate()) - 1
  lDisTs.dates.Summary = lDateSum
  ReDim HrVals(lDisTs.dates.Summary.NVALS)
  HrPos = 0
  For DyInd = 1 To DyTSer.dates.Summary.NVALS
    If OutSumm Then 'output summary message to file
      Call J2Date(DyTSer.dates.Value(DyInd) - 1, tmpdate())
      Write #OutFil, "Distributing Daily Data for " & tmpdate(0) & "/" & tmpdate(1) & "/" & tmpdate(2) & ":  Value is " & DyTSer.Value(DyInd)
    End If
    If DyTSer.Value(DyInd) > 0 Then 'something to disaggregate
      'back up a day, then add obs hour to get actual end of period
      edt = DyTSer.dates.Value(DyInd) - 1
      Call J2Date(edt, tmpdate())
      tmpdate(3) = ObsTime
      edt = Date2J(tmpdate)
'replace following code with sdt=edt-1
'      For i = 1 To 23
'        Call F90_TIMBAK(3, tmpdate(0))
'      Next i
'      sdt = Date2J(tmpdate)
      sdt = edt - 1
      ClosestRatio = 0
      For Each vHrTser In HrTSer
        Set lHrTser = vHrTser
        Set DaySumHrTser = lHrTser.SubSetByDate(sdt, edt)
        DaySum = 0
        For HrInd = 1 To DaySumHrTser.dates.Summary.NVALS
          DaySum = DaySum + DaySumHrTser.Value(HrInd)
        Next HrInd
        If DaySum > 0 Then
          Ratio = DyTSer.Value(DyInd) / DaySum
          If Ratio > 1 Then Ratio = 1 / Ratio
          If Ratio > ClosestRatio Then
            ClosestRatio = Ratio
            Set ClosestHrTser = DaySumHrTser
            ClosestDaySum = DaySum
          End If
        End If
      Next
      If ClosestRatio >= 1 - Tolerance Then 'hourly data found to do disaggregation
        Ratio = DyTSer.Value(DyInd) / ClosestDaySum
        MaxHrVal = 0
        DaySum = 0
        CARRY = 0
        For HrInd = 1 To ClosestHrTser.dates.Summary.NVALS
          i = HrPos + HrInd
          HrVals(i) = Ratio * ClosestHrTser.Value(HrInd) + CARRY
          If HrVals(i) > 0.00001 Then
            CARRY = HrVals(i) - (Round(HrVals(i) / RndOff) * RndOff)
            HrVals(i) = HrVals(i) - CARRY
          Else
            HrVals(i) = 0#
          End If
          If HrVals(i) > MaxHrVal Then
            MaxHrVal = HrVals(i)
            MaxHrInd = i
          End If
          DaySum = DaySum + HrVals(i)
        Next HrInd
        If CARRY > 0 Then 'add remainder to max hourly value
          DaySum = DaySum - HrVals(MaxHrInd)
          HrVals(MaxHrInd) = HrVals(MaxHrInd) + CARRY
          DaySum = DaySum + HrVals(MaxHrInd)
        End If
        If OutSumm Then
          Write #OutFil, "  Using Data-set Number:  " & ClosestHrTser.Header.id & ", daily sum = " & ClosestDaySum
        End If
        If Abs(DaySum - DyTSer.Value(DyInd)) > RndOff Then
          'values not distributed properly
          s = "Problem distributing " & DyTSer.Value(DyInd) & " on " & tmpdate(1) & "/" & tmpdate(2) & "/" & tmpdate(0)
          If OutSumm Then
            Write #OutFil, "  *** " & s & "  ***"
          End If
          rsp = MsgBox(s, vbExclamation + vbOKCancel, "Precipitation Disaggregation Problem")
          If rsp = vbCancel Then
            lDisTs.errordescription = s
            Err.Raise vbObjectError + 513
          End If
        End If
      Else 'no data available at hourly stations,
        'distribute using triangular distribution
        Call DistTriang(DyTSer.Value(DyInd), lHrVals, retcod)
        UsedTriang = UsedTriang + 1
        For HrInd = 1 To 24
          HrVals(HrPos + HrInd) = lHrVals(HrInd)
        Next HrInd
        If retcod = -1 Then
          s = "Unable to distribute this much rain (" & DaySum & ") using triangular distribution." & _
              "Hourly values will be set to -9.98"
          rsp = MsgBox(s, vbExclamation + vbOKCancel, "Precipitation Disaggregation Problem")
        ElseIf retcod = -2 Then
          s = "Problem distributing " & DyTSer.Value(DyInd) & " using triangular distribution on " & tmpdate(1) & "/" & tmpdate(2) & "/" & tmpdate(0)
          rsp = MsgBox(s, vbExclamation + vbOKCancel, "Precipitation Disaggregation Problem")
        End If
        If OutSumm Then
          Write #OutFil, "  *** No hourly total within tolerance - " & DyTSer.Value(DyInd) & "  distributed using triangular distribution ***"
          If retcod <> 0 Then
            Write #OutFil, "  *** " & s & " ***"
          End If
        End If
        If rsp = vbCancel Then
          lDisTs.errordescription = s
          Err.Raise vbObjectError + 513 + Abs(retcod)
        End If
      End If
    Else 'no daily value to distribute, fill hourly
      For HrInd = HrPos + 1 To HrPos + 24
        HrVals(HrInd) = 0
      Next HrInd
    End If
    HrPos = HrPos + 24
  Next DyInd
  
DisPrecipErrHnd:
  On Error GoTo OuttaHere 'in case there's an error in these statements
  If OutSumm Then Close #OutFil
  lDisTs.Values = HrVals

OuttaHere:
  If UsedTriang > 0 Then
    'inform calling routine that automatic triangular distribution was used
    s = "WARNING:  Automatic Triangular Distribution was used " & UsedTriang & " times." & vbCrLf
    If OutSumm Then
      s = s & "Check summary output file (" & SumFile & ") for details of when Triangular Distribution was used"
    End If
    lDisTs.errordescription = s
  End If
  Set DisPrecip = lDisTs

End Function

Private Sub DistTriang(DaySum!, HrVals!(), retcod&)
  'Distribute a daily value to 24 hourly values using a triangular distribution
  'DaySum - daily value
  'HrVals - array of hourly values
  'Retcod - 0 - OK, -1 - DaySum too big,
  '        -2 - sum of hourly values does not match daily value (likely a round off problem)

  Dim i&, j&, VTriang As Variant, VSum As Variant
  Dim Ratio!, CARRY!, RndOff!, lDaySum!
  Static initfg&, Sums!(12), Triang!(24, 12)

  If initfg = 0 Then
    VTriang = Array(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, _
                    0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, _
                    0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 2, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, _
                    0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 3, 3, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, _
                    0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 4, 6, 4, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, _
                    0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 5, 10, 10, 5, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, _
                    0, 0, 0, 0, 0, 0, 0, 0, 1, 6, 15, 20, 15, 6, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, _
                    0, 0, 0, 0, 0, 0, 0, 0, 1, 7, 21, 35, 35, 21, 7, 1, 0, 0, 0, 0, 0, 0, 0, 0, _
                    0, 0, 0, 0, 0, 0, 0, 1, 8, 28, 56, 70, 56, 28, 8, 1, 0, 0, 0, 0, 0, 0, 0, 0, _
                    0, 0, 0, 0, 0, 0, 0, 1, 9, 36, 84, 126, 126, 84, 36, 9, 1, 0, 0, 0, 0, 0, 0, 0, _
                    0, 0, 0, 0, 0, 0, 1, 10, 45, 120, 210, 252, 210, 120, 45, 10, 1, 0, 0, 0, 0, 0, 0, 0, _
                    0, 0, 0, 0, 0, 0, 1, 11, 55, 165, 330, 462, 462, 330, 165, 55, 11, 1, 0, 0, 0, 0, 0, 0)
    VSum = Array(0, 1, 2, 4, 8, 16, 32, 64, 128, 256, 512, 1024, 2048)

    For j = 1 To 12
      For i = 1 To 24
        Triang(i, j) = CSng(VTriang((j - 1) * 24 + i) / 100)
      Next i
      Sums(j) = CSng(VSum(j) / 100)
    Next j
    initfg = 1
  End If

  retcod = 0
  i = 1
  Do While DaySum > Sums(i)
    i = i + 1
    If i > 12 Then
      retcod = -1
    End If
  Loop

  RndOff = 0.001
  CARRY = 0
  Ratio = DaySum / Sums(i)
  lDaySum = 0
  For j = 1 To 24
    HrVals(j) = Ratio * Triang(j, i) + CARRY
    If HrVals(j) > 0.00001 Then
      CARRY = HrVals(j) - (Round(HrVals(j) / RndOff) * RndOff)
      HrVals(j) = HrVals(j) - CARRY
    Else
      HrVals(j) = 0#
    End If
    lDaySum = lDaySum + HrVals(j)
  Next j
  If CARRY > 0.00001 Then
    lDaySum = lDaySum - HrVals(12)
    HrVals(12) = HrVals(12) + CARRY
    lDaySum = lDaySum + HrVals(12)
  End If
  If Abs(DaySum - lDaySum) > RndOff Then
    'values not distributed properly
    retcod = -2
  End If
  If retcod <> 0 Then 'set to accumulated, with daily value at end
    For i = 1 To 23
      HrVals(i) = -9.98
    Next i
    HrVals(24) = DaySum
  End If

End Sub
