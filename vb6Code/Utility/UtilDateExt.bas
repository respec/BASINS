Attribute VB_Name = "UtilDateExt"
Option Explicit
'##MODULE_REMARKS Copyright 2001-3AQUA TERRA Consultants - Royalty-free use permitted under open source license
      
Public Sub DTMCMN(sdates() As Long, edates() As Long, _
                  TSTEP() As Long, TCODE() As Long, _
                  sdat() As Long, edat() As Long, _
                  ts As Long, tc As Long, retcod As Long) 'tc As ATCTimeUnit
'##SUMMARY DTMCMN - determine the time period common to a number of pairs of dates. _
  also determines the smallest common time step and unit.
'##PARM sdates - input array of beginning dates
'##PARM edates - input array of ending dates
'##PARM tstep  - input array of time steps
'##PARM tcode  - input array of time units codes _
                 1 - second        4 - day _
                 2 - minute        5 - month _
                 3 - hour          6 - year
'##PARM stat   - output common starting date
'##PARM edat   - output common ending date
'##PARM ts     - output smallest common time step
'##PARM tc     - output smallest common time units code
'##PARM retcod - output return code _
                  0 - there is a common time period and time step and units _
                 -1 - there is no common time period _
                 -2 - there is a common time period, but the time step and _
                      time units are not compatible
  Dim ndat As Long, n As Long, tstepf As Long, tcdcmp As Long
  '##LOCAL ndat   - number of dates to compare
  '##LOCAL n      - index of date being compared
  '##LOCAL tstepf - time step compatibility flag _
                    0 - compatible time steps _
                    1 - incompatible time steps
  '##LOCAL tcdcmp - flag indicating order of time steps _
                    0 - time steps are the same _
                    1 - first time step is smaller _
                    2 - second time step is smaller _
                   -1 - time units span day-month boundry

  DatCmn sdates, edates, sdat, edat, retcod   'get common time period
  
  If (retcod = 0) Then 'get common time step and units
    ts = TSTEP(1)
    tc = TCODE(1)
    'check others
    ndat = (UBound(sdates) / 6) '- 1 jlk 10/14/99
    n = 1
    Do While n < ndat And retcod = 0  'look for smallest common time step and unit
      cmptim TCODE(n), TSTEP(n), tc, ts, tstepf, tcdcmp
      If (tstepf = 0 And tcdcmp <> -1) Then 'compatible time steps, do not span day-month boundry
        If (tcdcmp = 2) Then 'new larger time step
          ts = TSTEP(n)
          tc = TCODE(n)
        End If
      Else 'incompatible time steps or time units span day-month boundry
        retcod = -2
      End If
      n = n + 1
    Loop
    If (retcod = -2) Then 'time step and time units are not all compatible
      ts = 0
      tc = 0
    End If
  Else 'no common time period
    retcod = -1
    ts = 0
    tc = 0
  End If
End Sub

Public Sub DatCmn(sd() As Long, ed() As Long, _
                  SDate() As Long, EDate() As Long, retcod As Long)
'##SUMMARY DatCmn - determine the time period common to a number of sets of dates.
'##PARM sd     - input array of beginning dates
'##PARM ed     - input array of ending dates
'##PARM sdate  - output common starting data
'##PARM edate  - output common ending date
'##PARM retcod - output return code _
                  0 - there is a common time period _
                 -1 - there is no common time period
   Dim ljdate As Double, ndat As Long
   Dim sjdate As Double, ejdate As Double, j As Long, i As Long, d(5) As Long
   '##LOCAL ljdate - temp modified julian date
   '##LOCAL ndat   - number of dates to check
   '##LOCAL sjdate - earliest modfied julian date
   '##LOCAL ejdate - latest modified julian date
   '##LOCAL j      - input array pointer
   '##LOCAL i      - loop counter thru dates arrays
   '##LOCAL d      - temp date array
   ndat = (UBound(sd) / 6) ' - 1 jlk 10/14/99
   sjdate = -1E+30 'way in past
   For i = 0 To ndat - 1
     j = i * 6
     d(0) = sd(j): d(1) = sd(j + 1): d(2) = sd(j + 2)
     d(3) = sd(j + 3): d(4) = sd(j + 4): d(5) = sd(j + 5)
     ljdate = Date2J(d)
     If ljdate > sjdate Then 'new latest start
       sjdate = ljdate
     End If
   Next i
   
   ejdate = 1E+30 'way in future
   For i = 0 To ndat - 1
     j = i * 6
     d(0) = ed(j): d(1) = ed(j + 1): d(2) = ed(j + 2)
     d(3) = ed(j + 3): d(4) = ed(j + 4): d(5) = ed(j + 5)
     ljdate = Date2J(d)
     If ljdate < ejdate Then 'new earliest end
       ejdate = ljdate
     End If
   Next i
   
   If ejdate > sjdate Then 'common start date before common end date, as hoped for
     J2Date sjdate, SDate
     J2Date ejdate, EDate
     retcod = 0
   Else
     For i = 0 To 5
       SDate(i) = 0
       EDate(i) = 0
     Next i
     retcod = -1
   End If
End Sub

Private Sub cmptim(tcode1 As Long, tstep1 As Long, _
                   tcode2 As Long, tstep2 As Long, _
                   tstepf As Long, tcdcmp As Long) 'tcode1 As ATCTimeUnit, tcode2 As ATCTimeUnit

'     Compare one time unit and step to a second time unit and
'     step.  Two flags are returned.  The first flag indicates
'     compatible/incompatible time steps.  The second flag
'     indicates which time step is smaller.  Time steps are
'     considered compatible if one is an even multiple of the
'     other.  One hour and 30 minutes are compatible; one hour
'     and 90 minutes are incompatible.  Comparison of time units
'     and time steps which cross the day-month boundry are handled
'     a little different.  If the smaller time step is a day or
'     less and is compatible with 1 day and the larger time step
'     is compatible with one month, than the smaller and the
'     larger time steps are considered to be compatible.  The time
'     step of a day or less will be considered to be the smaller
'     time step.
'     EXAMPLES:  TCODE1 TSTEP1 TCODE2 TSTEP2 TSTEPF TCDCMP
'                  3      1      2      60     0      0
'                  3      1      2      90     1      1
'                  3      1      2      30     0      2
 
'     TCODE1 - time units code
'              1 - second    4 - day
'              2 - minute    5 - month
'              3 - hour      6 - year
'     TSTEP1 - time step, in TCODE1 units
'     TCODE2 - time units code
'              1 - second    4 - day
'              2 - minute    5 - month
'              3 - hour      6 - year
'     TSTEP2 - time step in TCODE2 units
'     TSTEPF - time step compatability flag
'              0 - compatible time steps
'              1 - incompatible time steps
'     TCDCMP - flag indicating order of time steps
'               0 - time steps are the same
'               1 - first time step is smaller
'               2 - second time step is smaller
'              -1 - time units span day-month boundry
 
  Dim tc(2) As Long, ts(2) As Long, tsx As Long 'tc(2) As ATCTimeUnit
  Dim tcx As Long, tsfx(2) As Long, tcdx(2) As Long 'tcx As ATCTimeUnit, tcdx(2) As ATCTimeUnit
  
  tc(1) = tcode1
  tc(2) = tcode2
  ts(1) = tstep1
  ts(2) = tstep2

  'If (tc(1) < TUSecond Or tc(1) > TUYear Or

  If (tc(1) < 1 Or tc(1) > 6 Or _
      tc(2) < 1 Or tc(2) > 6 Or _
      ts(1) < 1 Or ts(1) > 1440 Or _
      ts(2) < 1 Or ts(2) > 1440) Then
    'an invalid time units code or time step
    tstepf = 1
    tcdcmp = -1
  ElseIf ((tc(1) <= 4 And tc(2) >= 5) Or _
          (tc(2) <= 4 And tc(1) >= 5)) Then
    'special case for time units that cross day-month boundry
    tstepf = 1
    tcdcmp = -1
    If (tc(1) <= 4) Then
      'first time unit is day or smaller, second is month or larger
      tsx = 1
      tcx = 4
      Call cmptm2(tc(1), ts(1), tcx, tsx, tsfx(1), tcdx(1))
      tsx = 1
      tcx = 5
      Call cmptm2(tc(2), ts(2), tcx, tsx, tsfx(2), tcdx(2))
      If (tsfx(1) = 0 And tsfx(2) = 0) Then
        'times compatible with boundaries
        If ((tcdx(1) = 0 Or tcdx(1) = 1) And _
            (tcdx(2) = 0 Or tcdx(2) = 2)) Then
          'smaller time a day or less, larger time a month or more
          tstepf = 0
          tcdcmp = 1
        End If
      End If
    Else
      'second time unit is day or smaller, first is month or larger
      tsx = 1
      tcx = 5
      Call cmptm2(tc(1), ts(1), tcx, tsx, tsfx(1), tcdx(1))
      tsx = 1
      tcx = 4
      Call cmptm2(tc(2), ts(2), tcx, tsx, tsfx(2), tcdx(2))
      If (tsfx(1) = 0 And tsfx(2) = 0) Then
        'times compatible with boundaries
        If ((tcdx(1) = 0 Or tcdx(1) = 2) And _
            (tcdx(2) = 0 Or tcdx(2) = 1)) Then
          'larger time a month or more, smaller time a day or less
          tstepf = 0
          tcdcmp = 2
        End If
      End If
    End If
  Else
    'valid time steps and units do not cross day-month boundry
    Call cmptm2(tc(1), ts(1), tc(2), ts(2), tstepf, tcdcmp)
  End If
End Sub

Private Sub cmptm2(tc1 As Long, ts1 As Long, tc2 As Long, ts2 As Long, tstepf As Long, tcdcmp As Long)

'     This routine compares one time unit and step to a second time
'     unit and step.  Two flags are returned.  The first flag
'     indicates compatible/incompatible time steps.  The second flag
'     indicates which timestep is smaller.

'     TC1    - time units code
'              1 - second    4 - day
'              2 - minute    5 - month
'              3 - hour      6 - year
'     TS1    - time step, in TC1 units
'     TC2    - time units code, see TC1
'     TS2    - time step, in TC2 units
'     TSTEPF - time step compatability flag
'              0 - compatible time series
'              1 - incompatible time steps
'     TCDCMP - flag indicating order of time steps
'               0 - time steps are the same
'               1 - first time step is smaller
'               2 - second time step is smaller
'              -1 - time units span day-month boundry
 
  Dim convdn As Variant
  
  convdn = Array(-1, 0, 60, 60, 24, 0, 12, 100)

  If ((tc1 <= 4 And tc2 > 4) Or _
      (tc1 > 4 And tc2 <= 4)) Then
    'time units span day-month boundry
    tstepf = 1
    tcdcmp = -1
  Else
    'acceptable time units
    If (tc1 <> tc2) Then
      'time units not same, adjust larger to agree with smaller
      If (tc1 < tc2) Then
        'adjust second time units to agree with first
        Do While tc1 < tc2
          ts2 = ts2 * convdn(tc2)
          tc2 = tc2 - 1
        Loop
      Else
        'adjust first time units to agree with second
        Do While tc2 < tc1
          ts1 = ts1 * convdn(tc1)
          tc1 = tc1 - 1
        Loop
      End If
    End If

    'Time units converted, check time step
    tstepf = 0
    If (ts1 = ts2) Then
      'Same time step
      tcdcmp = 0
    ElseIf (ts1 < ts2) Then
      'First time step smaller
      tcdcmp = 1
      If (ts2 Mod ts1) <> 0 Then tstepf = 1
    Else
      'Second time step smaller
      tcdcmp = 2
      If (ts1 Mod ts2) <> 0 Then tstepf = 1
    End If
  End If
End Sub

Public Sub timcnv(d() As Long)
'     Convert a date that uses the midnight convention of 00:00
'     to the convention 24:00.  For example, 1982/10/01 00:00:00
'     would be converted to the date 1982/09/30 24:00:00.

  If (d(3) = 0) Then
    If (d(4) = 0 And d(5) = 0) Then
'     date using new day boundry convention, convert to old
      d(3) = 24
      d(2) = d(2) - 1
      If (d(2) = 0) Then
        d(1) = d(1) - 1
        If (d(1) = 0) Then
          d(0) = d(0) - 1
          d(1) = 12
        End If
        d(2) = daymon(d(0), d(1))
      End If
    End If
  End If
End Sub

Public Function TimAddJ(jStartDate As Double, TCODE As Long, TSTEP As Long, NVALS As Long) As Double
  Select Case TCODE
    Case 1: TimAddJ = jStartDate + TSTEP * NVALS * JulianSecond
    Case 2: TimAddJ = jStartDate + TSTEP * NVALS * JulianMinute
    Case 3: TimAddJ = jStartDate + TSTEP * NVALS * JulianHour
    Case 4: TimAddJ = jStartDate + TSTEP * NVALS ' JulianDay = 1
    Case 5, 6, 7 'month, year, century
      Dim DATE1(6) As Long, DATE2(6) As Long
      J2Date jStartDate, DATE1
      TIMADD DATE1, TCODE, TSTEP, NVALS, DATE2
      TimAddJ = Date2J(DATE2)
  End Select
End Function

Public Sub TIMADD(DATE1() As Long, TCODE As Long, TSTEP As Long, NVALS As Long, DATE2() As Long)

' Add NVALS time steps to first date to compute second date.
' The first date is assumed to be valid.
'     DATE1  - starting date
'     TCODE  - time units
'              1 - second          5 - month
'              2 - minute          6 - year
'              3 - hour            7 - century
'              4 - Day
'     TSTEP  - time step in TCODE units
'     NVALS  - number of time steps to be added
'     DATE2  - new date
Dim CARRY As Long, DPM As Long, TYR As Long, TMO As Long, TDY As Long, THR As Long, TMN As Long, TSC As Long, STPOS As Long, DONFG As Long
  
  TYR = DATE1(0)
  TMO = DATE1(1)
  TDY = DATE1(2)
  THR = DATE1(3)
  TMN = DATE1(4)
  TSC = DATE1(5)
  
  'figure out how much time to add and where to start
  CARRY = NVALS * TSTEP
  STPOS = TCODE
  If (STPOS = 7) Then 'time units are centuries, convert to years
    STPOS = 6
    CARRY = CARRY * 100
  End If

  'add the time, not changing insig. parts
  If (STPOS = 1) Then 'seconds
    TSC = TSC + CARRY
    CARRY = Fix(TSC / 60)
    TSC = TSC - (CARRY * 60)
  End If
  If (STPOS <= 2 And CARRY > 0) Then ' minutes
    TMN = TMN + CARRY
    CARRY = Fix(TMN / 60)
    TMN = TMN - (CARRY * 60)
  End If
  If (STPOS <= 3 And CARRY > 0) Then ' hours
    THR = THR + CARRY
    CARRY = Fix(THR / 24)
    THR = THR - (CARRY * 24)
    If (THR = 0 And TMN = 0 And TSC = 0) Then 'this is the day boundry problem
      THR = 24
      CARRY = CARRY - 1
    End If
  End If
  If (STPOS <= 4 And CARRY > 0) Then ' days
    TDY = TDY + CARRY
    If (TDY > 28) Then 'may need month/year adjustment
      DONFG = 0
      While DONFG = 0
        DPM = daymon(TYR, TMO)
        If (TDY > DPM) Then 'add another month
          TDY = TDY - DPM
          TMO = TMO + 1
          If (TMO > 12) Then 'add year
            TMO = 1
            TYR = TYR + 1
          End If
        ElseIf (TDY <= 0) Then 'subtract another month
          TMO = TMO - 1
          If (TMO = 0) Then
            TYR = TYR - 1
            TMO = 12
          End If
          TDY = TDY - daymon(TYR, TMO)
        Else
          DONFG = 1
        End If
      Wend
    End If
    'month and year updated here all done
  End If

  If (STPOS >= 5) Then
    If (STPOS = 5) Then 'months
      TMO = TMO + CARRY
      CARRY = Fix((TMO - 1) / 12)
      TMO = TMO - (CARRY * 12)
    End If
    If (STPOS <= 6 And CARRY > 0) Then ' years
      TYR = TYR + CARRY
    End If

    'check days/month
    DPM = daymon(TYR, TMO)
    If (DPM < TDY) Then
      TDY = DPM
    End If
    If (daymon(DATE1(0), DATE1(1)) = DATE1(2)) Then
      TDY = DPM
    End If
  End If

  DATE2(0) = TYR
  DATE2(1) = TMO
  DATE2(2) = TDY
  DATE2(3) = THR
  DATE2(4) = TMN
  DATE2(5) = TSC
End Sub

Public Sub timdif(DATE1() As Long, DATE2() As Long, TCODE As Long, TSTEP As Long, NVALS As Long)

'     Calculate the number of time steps between two dates.  Part
'     intervals at a time step less than TCODE and TSSTEP are not
'     included.  If the second date is before the first date, or the
'     second date is the same as the first date, the number of time
'     steps will be returned as 0.  Dates are assumed to be valid.
'       DATE1  - first (starting) date
'       DATE2  - second (ending) date
'       TCODE  - time units code
'                1 - seconds     5 - months
'                2 - minutes     6 - years
'                3 - hours       7 - centuries
'                4 - days
'       TSTEP  - time step in TCODE units
' Output:
'       NVALS  - number of time steps between DATE1 and DATE2

  NVALS = pTimDif(Date2J(DATE1), Date2J(DATE2), DATE1, DATE2, TCODE, TSTEP)
End Sub
  
Public Function timdifJ(StartJDate As Double, EndJDate As Double, TCODE As Long, TSTEP As Long) As Long
'     Calculate the number of time steps between two dates.  Part
'     intervals at a time step less than TCODE and TSSTEP are not
'     included.  If the second date is before the first date, or the
'     second date is the same as the first date, the number of time
'     steps will be returned as 0.  Dates are assumed to be valid.
'       StartJDate - first (starting) Julian date
'       EndJDate   - second (ending) Julian date
'       TCODE      - time units code
'                1 - seconds     5 - months
'                2 - minutes     6 - years
'                3 - hours       7 - centuries
'                4 - days
'       TSTEP  - time step in TCODE units
'Returns NVALS - number of time steps between DATE1 and DATE2
  
  If (EndJDate <= StartJDate) Then ' end date is the same as or before start date
    timdifJ = 0
  Else  'end date follows start date, make temp copies of dates
    Dim tmpstr(6) As Long, tmpend(6) As Long
    J2Date StartJDate, tmpstr
    J2Date EndJDate, tmpend
    timdifJ = pTimDif(StartJDate, EndJDate, tmpstr, tmpend, TCODE, TSTEP)
  End If
End Function

Private Function pTimDif(StartJDate As Double, EndJDate As Double, DATE1() As Long, DATE2() As Long, TCODE As Long, TSTEP As Long) As Long
  Dim NVALS As Long
  Dim tmpDATE(5) As Long
  'convert dates to old format
  Call timcnv(DATE1)
  Call timcnv(DATE2)
  
  Select Case TCODE
    Case 1 'seconds
      NVALS = ((EndJDate - StartJDate) * 86400#) / TSTEP
    Case 2 'minutes
      NVALS = ((EndJDate - StartJDate) * 1440#) / TSTEP
    Case 3 'hours
      NVALS = ((EndJDate - StartJDate) * 24) / TSTEP
    Case 4 'days
      NVALS = (EndJDate - StartJDate) / TSTEP
    Case 5 'months
      NVALS = ((DATE2(0) - DATE1(0)) * 12 + DATE2(1) - DATE1(1)) / TSTEP
    Case 6 'years
      NVALS = (DATE2(0) - DATE1(0)) / TSTEP
    Case 7 'centuries
      NVALS = (DATE2(0) - DATE1(0)) / (TSTEP * 100)
    Case Else
      NVALS = 0
  End Select
  
  tmpDATE(0) = DATE2(0)
  tmpDATE(1) = DATE2(1)
  tmpDATE(2) = DATE2(2)
  tmpDATE(3) = DATE2(3)
  tmpDATE(4) = DATE2(4)
  tmpDATE(5) = DATE2(5)
  Do
    Call TIMADD(DATE1, TCODE, TSTEP, NVALS, tmpDATE)
    If EndJDate < Date2J(tmpDATE) Then 'estimate too high
      NVALS = NVALS - 1
    Else 'estimate ok
      Exit Do
    End If
  Loop
  pTimDif = NVALS
End Function
