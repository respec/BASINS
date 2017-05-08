Attribute VB_Name = "UtilTSMetCorrect"
Option Explicit
'Copyright 2001 by AQUA TERRA Consultants

Public Function CalcPrecipDistances(lTSer As ATCclsTserData, Stations As FastCollection) As FastCollection
  'calculate distance between a single station (lTSer) and a collection of nearby "Stations"
  'considering both geographic distance and difference between annual precipitation averages from PRISM
  Dim i As Long, AnnAdj As Single, lTSerAnn As Single, StaAnn As Single
  Dim CompareAnnuals As Boolean
  Dim GDist As Single, lDist() As Single, order() As Long, rank() As Long
  Dim Dist As FastCollection

  Set Dist = New FastCollection
  ReDim lDist(Stations.Count)
  ReDim order(Stations.Count)
  ReDim rank(Stations.Count)
  lTSerAnn = lTSer.Attribnumeric("PRECIP")
  If lTSerAnn = 0 Then
    CompareAnnuals = False
  Else
    CompareAnnuals = True
  End If
  For i = 1 To Stations.Count
    AnnAdj = 1 'assume no adjustment for annual averages
    If CompareAnnuals Then
      StaAnn = Stations.ItemByIndex(i).Attribnumeric("PRECIP")
      If StaAnn > 0 Then
        AnnAdj = lTSerAnn / StaAnn
        If AnnAdj < 1 Then AnnAdj = 1 / AnnAdj
      End If
    End If
    GDist = GeoDist(lTSer, Stations.ItemByIndex(i)) 'now calculate geographic distance
    lDist(i) = AnnAdj * GDist
  Next i
  SortRealArray 0, Stations.Count, lDist, order
  SortIntegerArray 0, Stations.Count, order, rank
  For i = 1 To Stations.Count
    Dist.Add lDist(i), CStr(rank(i))
  Next i
  Set CalcPrecipDistances = Dist
End Function

Public Function GeoDist(ts1 As ATCclsTserData, ts2 As ATCclsTserData) As Single
  'calculate the geographic distance between two stations (TSers)
  'based on projected feet ()
  Dim X1!, X2!, Y1!, Y2!
  X1 = ts1.Attribnumeric("UBC024")
  Y1 = ts1.Attribnumeric("UBC025")
  X2 = ts2.Attribnumeric("UBC024")
  Y2 = ts2.Attribnumeric("UBC025")
  If X1 = 0 Or X2 = 0 Or Y1 = 0 Or Y2 = 0 Then 'something wrong, make it huge
    GeoDist = 1E+30
  Else
    GeoDist = Sqr((X1 - X2) ^ 2 + (Y1 - Y2) ^ 2)
  End If
End Function

Public Function DisPrecip(DyTSer As ATCclsTserData, OTTSer As ATCclsTserData, HrTSer As FastCollection, Tolerance As Single, s As String, Optional QuTSer As ATCclsTserData = Nothing) As ATCclsTserData
  'DyTSer - daily time series being disaggregated
  'OTTSer - daily obs time timeseries
  'HrTser - collection of hourly timeseries used to disaggregate daily
  'Tolerance - tolerance for comparison of hourly daily sums to daily value (0.01 - 1.0)
  's - string of output summary info

  Dim i&, HrPos&, HrVals!(), QuVals!(), Carry!, RndOff!, MaxHrVal!, MaxHrInd&
  Dim DyInd&, HrInd&, DaySum!, ClosestDaySum!, Ratio!
  Dim sdt#, edt#, tmpdate&(5), Qu As Boolean, ls As String
  Dim lHrVals!(24), retcod&, UsedTriang&, nsta&, nHrs&, ObsTime&, CurOT&
  Dim ClosestHrTser As ATCclsTserData
  Dim lDisTs As ATCclsTserData, lDateSum As ATTimSerDateSummary

  On Error GoTo DisPrecipErrHnd
  UsedTriang = 0
  RndOff = 0.001
  nsta = 10
  s = ""

  Call InitCmpTs(DyTSer, TUHour, 1, 24, lDisTs)
  lDisTs.Header.con = "HPCP"
  lDisTs.AttribSet "TSTYPE", lDisTs.Header.con
  If QuTSer Is Nothing Then 'no quality tser needed
    Qu = False
  Else 'need a quality code tser also
    Call InitCmpTs(DyTSer, TUHour, 1, 24, QuTSer)
    QuTSer.Header.con = "QUHPCP"
    QuTSer.AttribSet "TSTYPE", QuTSer.Header.con
    ReDim QuVals(QuTSer.dates.Summary.NVALS)
    Qu = True
  End If
  '*** use same start/end date as daily tser for resulting hourly tser
'  'adjust start/end date based on Obstime
'  lDateSum = lDisTs.Dates.Summary
'  Call J2Date(lDateSum.sjday, tmpdate())
'  tmpdate(3) = ObsTime
'  'back up exactly one day for start date
'  lDateSum.sjday = Date2J(tmpdate()) - 1
'  Call J2Date(lDateSum.ejday, tmpdate())
'  tmpdate(3) = ObsTime
'  lDateSum.ejday = Date2J(tmpdate()) - 1
'  lDisTs.Dates.Summary = lDateSum
  ReDim HrVals(lDisTs.dates.Summary.NVALS)
  HrPos = 0
  CurOT = CurrentObsTime(OTTSer, 1)
  For DyInd = 1 To DyTSer.dates.Summary.NVALS
    If DyTSer.Value(DyInd) > 0 Then 'something to disaggregate
      Call J2Date(DyTSer.dates.Value(DyInd) - 1, tmpdate())
      ls = "  Distributing " & DyTSer.Value(DyInd) & " on " & tmpdate(0) & "/" & tmpdate(1) & "/" & tmpdate(2)
      'back up a day, then add obs hour to get actual end of period
      edt = DyTSer.dates.Value(DyInd) - 1
      Call J2Date(edt, tmpdate())
      ObsTime = CurrentObsTime(OTTSer, DyInd)
      If CurOT <> ObsTime Then 'need to adjust position in output array
        HrPos = HrPos + ObsTime - CurOT
        CurOT = ObsTime
      End If
      tmpdate(3) = ObsTime
      edt = Date2J(tmpdate)
      sdt = edt - 1
      If sdt < DyTSer.dates.Summary.sjday Then sdt = DyTSer.dates.Summary.sjday
      If edt > DyTSer.dates.Summary.ejday Then sdt = DyTSer.dates.Summary.ejday
      nHrs = (edt - sdt) * 24
      Set ClosestHrTser = ClosestHourly(DyTSer, HrTSer, DyTSer.Value(DyInd), sdt, edt, Tolerance, nsta)
      If Not ClosestHrTser Is Nothing Then 'hourly data found to do disaggregation
        ClosestDaySum = 0
        For HrInd = 1 To ClosestHrTser.dates.Summary.NVALS
          ClosestDaySum = ClosestDaySum + ClosestHrTser.Value(HrInd)
        Next HrInd
        Ratio = DyTSer.Value(DyInd) / ClosestDaySum
        MaxHrVal = 0
        DaySum = 0
        Carry = 0
        For HrInd = 1 To ClosestHrTser.dates.Summary.NVALS
          i = HrPos + HrInd
          HrVals(i) = Ratio * ClosestHrTser.Value(HrInd) + Carry
          If HrVals(i) > 0.00001 Then
            Carry = HrVals(i) - (Round(HrVals(i) / RndOff) * RndOff)
            HrVals(i) = HrVals(i) - Carry
          Else
            HrVals(i) = 0#
          End If
          If HrVals(i) > MaxHrVal Then
            MaxHrVal = HrVals(i)
            MaxHrInd = i
          End If
          DaySum = DaySum + HrVals(i)
        Next HrInd
        If Carry > 0 Then 'add remainder to max hourly value
          DaySum = DaySum - HrVals(MaxHrInd)
          HrVals(MaxHrInd) = HrVals(MaxHrInd) + Carry
          DaySum = DaySum + HrVals(MaxHrInd)
        End If
        If Abs(DaySum - DyTSer.Value(DyInd)) > RndOff Then
          'values not distributed properly
          ls = ls & "  *** PROBLEM *** (Daily value= " & DyTSer.Value(DyInd) & ", Hourly total= " & DaySum & ")" & vbCrLf
        Else
          ls = ls & " using TS# " & ClosestHrTser.Header.ID & ", daily sum = " & ClosestDaySum & vbCrLf
        End If
      Else 'no data available at hourly stations,
        'distribute using triangular distribution
        Call DistTriang(DyTSer.Value(DyInd), lHrVals, retcod)
        UsedTriang = UsedTriang + 1
        '*** potential for losing triangular dist. values if at the
        '*** beginning or end of timeseries, depending on obs time
        For HrInd = 1 To 24
          HrVals(HrPos + HrInd) = lHrVals(HrInd)
          If Qu Then QuVals(HrPos + HrInd) = -1 'flag triangular dist. values
        Next HrInd
        If retcod = 0 Then
          ls = ls & "  *** Triangular distribution used" & vbCrLf
        ElseIf retcod = -1 Then
          ls = ls & "  *** Triangular FAILURE:  too much rain (Values set to -9.98)" & vbCrLf
        ElseIf retcod = -2 Then
          ls = ls & "  *** Triangular FAILURE:  (Values set to -9.98)" & vbCrLf
        End If
      End If
      s = s & ls
    Else 'no daily value to distribute, fill hourly
      If DyInd = 1 Then 'adjust first day to only contain values up to obs time
        nHrs = CurrentObsTime(OTTSer, DyInd)
      Else
        nHrs = 24
      End If
      For HrInd = HrPos + 1 To HrPos + nHrs
        HrVals(HrInd) = 0
      Next HrInd
    End If
    HrPos = HrPos + nHrs
  Next DyInd
  
DisPrecipErrHnd:
  On Error GoTo OuttaHere 'in case there's an error in these statements
  lDisTs.Values = HrVals
  If Qu Then QuTSer.Values = QuVals

OuttaHere:
  Set DisPrecip = lDisTs

End Function

Private Sub DistTriang(DaySum!, HrVals!(), retcod&)
  'Distribute a daily value to 24 hourly values using a triangular distribution
  'DaySum - daily value
  'HrVals - array of hourly values
  'Retcod - 0 - OK, -1 - DaySum too big,
  '        -2 - sum of hourly values does not match daily value (likely a round off problem)

  Dim i&, j&, VTriang As Variant, VSum As Variant
  Dim Ratio!, Carry!, RndOff!, lDaySum!
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
  Carry = 0
  Ratio = DaySum / Sums(i)
  lDaySum = 0
  For j = 1 To 24
    HrVals(j) = Ratio * Triang(j, i) + Carry
    If HrVals(j) > 0.00001 Then
      Carry = HrVals(j) - (Round(HrVals(j) / RndOff) * RndOff)
      HrVals(j) = HrVals(j) - Carry
    Else
      HrVals(j) = 0#
    End If
    lDaySum = lDaySum + HrVals(j)
  Next j
  If Carry > 0.00001 Then
    lDaySum = lDaySum - HrVals(12)
    HrVals(12) = HrVals(12) + Carry
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

Public Sub InitCmpTs(InTs As ATCclsTserData, NTu&, nts&, NewIntvls&, _
                     newTS As ATCclsTserData)
  'InTs - input TSer upon which new TSer is based
  'NTu/NTs - new TSer's time units and time step
  'NewIntvls - number of new time intervals between InTs and NewTs
  '           (e.g. 24 if going from daily to hourly,
  '                 15 if going from hourly to 15-minute)
  'NewTs - new TSer being built

  Dim lTserSum As ATTimSerDateSummary

  Set newTS = InTs.Copy
  Set newTS.dates = New ATCclsTserDate
  With lTserSum
    .CIntvl = True
    .Intvl = InTs.dates.Summary.Intvl / NewIntvls
    .NVALS = InTs.dates.Summary.NVALS * NewIntvls
    .sjday = InTs.dates.Summary.sjday
    .ejday = InTs.dates.Summary.ejday
    .ts = nts
    .Tu = NTu
  End With
  newTS.dates.Summary = lTserSum

End Sub

Public Function CurrentObsTime(ts As ATCclsTserData, pos As Long) As Long
  'given a daily Timeseries and a position in its data,
  'return the current observation time
  Dim ot As Long, inc As Long, cpos As Long

  cpos = pos
  inc = -1 'look back in values if initial "pos" value is undefined
  ot = -1
  While ot < 0
    If ts.Value(cpos) > 0 And ts.Value(cpos) < 25 Then
      ot = ts.Value(cpos)
    Else
      cpos = cpos + inc
    End If
    If cpos < 1 Then 'at beginning of values, look ahead of initial "pos"
      inc = 1
      cpos = pos + 1
    ElseIf cpos > ts.dates.Summary.NVALS Then 'at end of values, no obs time found, set to 24
      ot = 24
    End If
  Wend
  CurrentObsTime = ot
End Function

Public Function ClosestHourly(lTSer As ATCclsTserData, Stations As FastCollection, dv As Single, sdt As Double, edt As Double, Tol As Single, nsta As Long) As ATCclsTserData
  'find the hourly station (from the collection Stations) that is closest to
  'a daily precip station (lTSer) considering both geographic distance and
  'difference between daily totals for the day specified by sdt and edt
  '(hourly daily totals adjusted based on PRISM average annual values)
  Dim i As Long, AnnAdj As Single, lTSerAnn As Single, StaAnn As Single
  Dim CompareAnnuals As Boolean
  Dim GDist As Single, lDist() As Single, order() As Long, rank() As Long
  Dim HrDist As Single, ClosestDist As Single
  Dim DaySumHrTSer As ATCclsTserData, lHrTSer As ATCclsTserData
  Dim DaySum As Single, Ratio As Single
  Dim HrInd As Long, Ind As Long
  Static Dist As FastCollection
  Static CurID As Long

  If lTSer.Header.ID <> CurID Then 'calculate station distances
    Set Dist = New FastCollection
    ReDim lDist(Stations.Count)
    ReDim order(Stations.Count)
    ReDim rank(Stations.Count)
    lTSerAnn = lTSer.Attribnumeric("PRECIP")
    If lTSerAnn = 0 Then
      CompareAnnuals = False
    Else
      CompareAnnuals = True
    End If
    For i = 1 To Stations.Count
      AnnAdj = 1 'assume no adjustment for annual averages
      If CompareAnnuals Then
        StaAnn = Stations.ItemByIndex(i).Attribnumeric("PRECIP")
        If StaAnn > 0 Then
          AnnAdj = lTSerAnn / StaAnn
          If AnnAdj < 1 Then AnnAdj = 1 / AnnAdj
        End If
      End If
      GDist = GeoDist(lTSer, Stations.ItemByIndex(i)) 'now calculate geographic distance
      lDist(i) = AnnAdj * GDist
    Next i
    SortRealArray 0, Stations.Count, lDist, order
    SortIntegerArray 0, Stations.Count, order, rank
    For i = 1 To Stations.Count
      Dist.Add lDist(i), CStr(rank(i))
    Next i
    CurID = lTSer.Header.ID
  End If

  ClosestDist = 1E+30
  Set lHrTSer = Nothing
  For i = 1 To nsta 'look through clostest nsta stations for an acceptable daily total
    Ind = Dist.IndexFromKey(CStr(i))
    Set DaySumHrTSer = Stations.ItemByIndex(Ind).subsetbydate(sdt, edt)
    DaySum = DaySumHrTSer.Sum
'    For HrInd = 1 To DaySumHrTSer.dates.Summary.NVALS
'      DaySum = DaySum + DaySumHrTSer.Value(HrInd)
'    Next HrInd
    If DaySum > 0 Then
      If CompareAnnuals Then
        StaAnn = DaySumHrTSer.Attribnumeric("PRECIP")
        If StaAnn > 0 Then
          DaySum = DaySum * lTSerAnn / StaAnn
        End If
      End If
      Ratio = dv / DaySum
      If Ratio > 1 Then Ratio = 1 / Ratio
      If Ratio >= 1 - Tol Then 'acceptable daily total, include distance factor
        GDist = GeoDist(lTSer, DaySumHrTSer) 'now calculate geographic distance
        'try 2 as a "weighting factor" for daily precip ratio
        HrDist = (2 - Ratio) * GDist
        If HrDist < ClosestDist Then
          ClosestDist = HrDist
          Set lHrTSer = DaySumHrTSer
        End If
      End If
    End If
  Next i
  Set ClosestHourly = lHrTSer
End Function
