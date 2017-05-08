Attribute VB_Name = "UtilTSSummary"
Option Explicit
'Copyright 2001 by AQUA TERRA Consultants

Public Sub MissingData(lTSer As ATCclsTserData, s As String, Optional MVal As Single = -9.99, Optional MAcc As Single = -9.98, Optional FMin As Single = -1, Optional FMax As Single = 90000, Optional RepTyp As Long = 0)

  Dim i&, j&, iMisPd&
  Dim nv&, mpos&, MCod&, NMVals&, lpos&, TotMissIncs&
  Dim NMVPd&, NMDPd&, NBVPd&, TotMV&, TotMD&, TotBV&
  Dim lbuff!(), AccVal!, SDate&(5), ldate&(5)
  Dim DateStr$, TSStr$, TUStr$(6), ls$

  TUStr(1) = "seconds"
  TUStr(2) = "minutes"
  TUStr(3) = "hours"
  TUStr(4) = "days"
  TUStr(5) = "months"
  TUStr(6) = "years"
  NMVPd = 0
  TotMV = 0
  NMDPd = 0
  TotMD = 0
  NBVPd = 0
  TotBV = 0

  If RepTyp = 0 Then
    s = "For Data-set number " & lTSer.Header.ID & " (" & lTSer.Header.sen & " " & lTSer.Header.Loc & " " & lTSer.Header.con
    If Len(lTSer.Header.Desc) > 0 Then
      s = s & ", " & lTSer.Header.Desc & ")" & vbCrLf
    Else
      s = s & ")" & vbCrLf
    End If
  Else
    s = "STATION:HEADER  Index, Scenario, Location, Constituent, Description, TStep, TUnits" & vbCrLf
    s = s & "STATION:DATA  " & lTSer.Header.ID & ", " & lTSer.Header.sen & ", " & lTSer.Header.Loc & ", " & lTSer.Header.con & ", " & lTSer.Header.Desc & ", " & lTSer.dates.Summary.ts & ", " & lTSer.dates.Summary.Tu & vbCrLf
  End If
  'build time step/units string for display
  TSStr = TUStr(lTSer.dates.Summary.Tu) 'start with time units
  If lTSer.dates.Summary.ts > 1 Then 'need time step too
    TSStr = lTSer.dates.Summary.ts & " " & TSStr
    'remove plural from time units, add 'intervals'
    TSStr = Left(TSStr, Len(TSStr) - 1) & " intervals"
  End If
  J2Date lTSer.dates.Summary.sjday, SDate
  iMisPd = 0
  MCod = 1
  mpos = 1
  Do While MCod > 0
    nv = lTSer.dates.Summary.NVALS - mpos + 1
    If nv > 0 Then 'values in buffer to check
      ReDim lbuff(nv)
      For j = mpos To lTSer.dates.Summary.NVALS
        lbuff(j - mpos + 1) = lTSer.Value(j)
      Next j
      Call NxtMis(nv, lbuff(), MVal, MAcc, FMin, FMax, MCod, lpos, NMVals, AccVal)
      If MCod > 0 Then 'somethings missing
        mpos = mpos + lpos - 1
        If mpos > 0 Then 'determine date
          If lTSer.dates.Summary.Tu > TUHour Then
            Call TIMADD(SDate, lTSer.dates.Summary.Tu, lTSer.dates.Summary.ts, mpos - 1, ldate)
          Else
            Call TIMADD(SDate, lTSer.dates.Summary.Tu, lTSer.dates.Summary.ts, mpos, ldate)
          End If
        Else 'must be at the start
          For j = 0 To 5
            ldate(j) = SDate(j)
          Next j
        End If
        If lTSer.dates.Summary.Tu < 4 Then 'display h,m,s
          DateStr = ldate(0) & "/" & ldate(1) & "/" & ldate(2) & " " & ldate(3) & ":" & ldate(4) & ":" & ldate(5)
        Else 'just display day
          DateStr = ldate(0) & "/" & ldate(1) & "/" & ldate(2)
        End If
        If MCod = 1 Then 'missing values
          ls = " of missing values starting "
          NMVPd = NMVPd + 1
          TotMV = TotMV + NMVals
        ElseIf MCod = 2 Then 'accumulated values
          ls = " of missing time distribution starting "
          NMDPd = NMDPd + 1
          TotMD = TotMD + NMVals
        ElseIf MCod = 3 Then 'screwball values
          ls = " of faulty values starting "
          NBVPd = NBVPd + 1
          TotBV = TotBV + NMVals
        End If
        iMisPd = iMisPd + 1
        mpos = mpos + NMVals
        If RepTyp = 0 Then
          s = s & "  " & NMVals & " " & TSStr & ls & DateStr & vbCrLf
        Else
          s = s & "DETAIL:HEADER  Index, Start, Length, Type, AccumVal" & vbCrLf
          s = s & "DETAIL:DATA  " & iMisPd & ", " & Date2J(ldate) & ", " & NMVals & ", " & MCod & ", " & AccVal & vbCrLf
        End If
      End If
    Else 'no values left to scan
      MCod = 0
    End If
  Loop
  TotMissIncs = TotMV + TotMD + TotBV
  If RepTyp = 0 Then
    If iMisPd > 0 Then 'missing data found for this time series
      s = s & iMisPd & " period(s) of missing or bad data." & vbCrLf
      s = s & "TotalIncs:" & NumFmtI(lTSer.dates.Summary.NVALS, 8) & vbCrLf & "  MisValPd:" & NumFmtI(NMVPd, 9) & vbCrLf _
            & "  MisValTot:" & NumFmtI(TotMV, 8) & vbCrLf & "  MisDisPd:" & NumFmtI(NMDPd, 9) & vbCrLf & "  MisDisTot: " & NumFmtI(TotMD, 7) & vbCrLf _
            & "  BadValPd:" & NumFmtI(NBVPd, 9) & vbCrLf & "  BadValTot:" & NumFmtI(TotBV, 8) & vbCrLf & "  PctMiss: " & NumFmted(100 * TotMissIncs / lTSer.dates.Summary.NVALS, 9, 1) & vbCrLf
    Else 'nothing missing
        s = s & "  No missing data for this time series" & vbCrLf
    End If
  Else
    s = s & "SUMMARY:HEADER  TotalIncs, TotalMissPd, TotalMissIncs, MisValPd, MisValTot, MisDisPd, MisDisTot, BadValPd, BadValTot, PctMiss" & vbCrLf
    s = s & "SUMMARY:DATA  " & lTSer.dates.Summary.NVALS & ", " & iMisPd & ", " & TotMissIncs & ", " & NMVPd & ", " & TotMV & ", " & NMDPd & ", " & TotMD & ", " & NBVPd & ", " & TotBV & ", " & NumFmted(100 * TotMissIncs / lTSer.dates.Summary.NVALS, 6, 1) & vbCrLf
  End If

End Sub

Public Sub NxtMis(BufSiz&, DBuff!(), ValMis!, ValAcc!, FaultMin!, FaultMax!, MisCod&, MisPos&, NVALS&, MVal!)

  'Find the next missing value or distribution or screwball value
  'in the data buffer (DBUFF).  If none found, return MISCOD = 0.

  'BUFSIZ - size of data buffer array
  'DBUFF  - array of data values in which to look for missing data
  'VALMIS - value indicating missing data values
  'VALACC - value indicating accumulated values (missing distribution)
  'FaultMin - value indicating value is too low to be valid
  'FaultMax - value indicating value is too high to be valid
  'MISCOD - type of missing data
  '         1 - missing value
  '         2 - missing distribution
  '         3 - garbage value
  'MISPOS - position in array where missing data starts
  'NVALS  - number of data values missing
  'MVAL   - accumulated value (for accumulated data) or dummy
  '         for other types of missing data
    
  Dim i&, MisFlg&, AccFlg&, BadFlg&, DonFlg&
    
  MisFlg = 0
  AccFlg = 0
  BadFlg = 0
  DonFlg = 0

  i = 0
  Do While DonFlg = 0 And i < BufSiz
    'check next data value
    i = i + 1
    If Abs(DBuff(i) - ValMis) < 0.00001 Then
      'we have a missing value
      If AccFlg > 0 Then
        'we were in the midst of an accum value array,
        'change it all to missing because dont know total
        MisFlg = AccFlg
        AccFlg = 0
      End If
      MisFlg = MisFlg + 1
      If MisFlg = 1 Then
        'first missing value, save start position
        MisPos = i
      End If
    ElseIf Abs(DBuff(i) - ValAcc) < 0.00001 Then
      'we have an accumulated value
      If MisFlg > 0 Then
        'report only missing values now, will get accum next time
        DonFlg = 1
      Else
        'accumulated value w/no distribution
        AccFlg = AccFlg + 1
        If AccFlg = 1 Then
          'first accumulated value, save start position
          MisPos = i
        End If
      End If
    ElseIf DBuff(i) < FaultMin Or DBuff(i) > FaultMax Then
      'we have a screwball value
      BadFlg = BadFlg + 1
      If BadFlg = 1 Then
        'first bad value, save start position
        MisPos = i
      End If
    Else
      'no problem with this value
      If MisFlg > 0 Or AccFlg > 0 Or BadFlg > 0 Then
        'end of missing period
        DonFlg = 1
        If AccFlg > 0 Then
          'include first good value in accumulated count
          AccFlg = AccFlg + 1
        End If
      End If
    End If
  Loop

  If MisFlg > 0 Then
    'missing values
    MisCod = 1
    NVALS = MisFlg
    MVal = -1E+30
  ElseIf AccFlg > 0 Then
    'missing distribution
    MisCod = 2
    NVALS = AccFlg
    'save accumulated value for possible distribution later
    MVal = DBuff(i)
  ElseIf BadFlg > 0 Then
    'screwball values
    MisCod = 3
    NVALS = BadFlg
    MVal = -1E+30
  Else
    'no missing data in buffer
    MisCod = 0
    MisPos = 0
    NVALS = 0
    MVal = 0#
  End If

End Sub

Public Sub SummarizeObsTimes(lTSer As ATCclsTserData, s As String, Optional MVal As Single = -999)

  Dim i As Long, CD(5) As Long
  Dim CurVal As Single

  s = "For Timeseries number " & lTSer.Header.ID & " (" & lTSer.Header.sen & "," & lTSer.Header.Loc & "," & lTSer.Header.con & ", " & lTSer.Header.Desc & "):" & vbCrLf
  If lTSer.dates.Summary.NVALS > 0 Then
    CurVal = lTSer.Value(1)
    J2Date lTSer.dates.Summary.sjday, CD
    s = s & "  Starting " & CD(0) & "/" & CD(1) & "/" & CD(2) & " observation time is " & CurVal & vbCrLf
    For i = 2 To lTSer.dates.Summary.NVALS
      If lTSer.Value(i) <> MVal And lTSer.Value(i) <> CurVal Then 'obs time change
        CurVal = lTSer.Value(i)
        J2Date lTSer.dates.Value(i - 1), CD
        s = s & "  Starting " & CD(0) & "/" & CD(1) & "/" & CD(2) & " observation time is " & CurVal & vbCrLf
      End If
    Next i
  Else
    s = s & "  There are no observation times available."
  End If
End Sub
