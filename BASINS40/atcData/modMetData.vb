Imports System.IO
Imports atcUtility
Imports MapWinUtility

Public Module modMetData

  Public Sub FillDailyTsers(ByVal aTS2Fill As atcCollection, ByVal aTSAvail As atcCollection, ByVal aTSObsTimes As atcCollection)
    Dim i As Integer = 0
    Dim j As Integer
    Dim k As Integer
    Dim lInd As Long
    Dim ii As Integer
    Dim lCurInd As Integer
    Dim lFPos As Integer
    Dim lSPos As Integer
    Dim lMLen As Integer
    Dim ld(5) As Integer
    Dim lFillOT As Integer
    Dim lStaOT As Integer
    Dim s As String
    Dim lStr As String
    Dim lOStr As String
    Dim ls As String
    Dim lTUStr As String
    Dim lFName As String
    Dim lSJDay As Double
    Dim lMJDay As Double
    Dim lFVal As Double
    Dim lFAnn As Double
    Dim lStaAnn As Double
    Dim lDist As atcCollection
    Dim lTSer As atcTimeseries

    For Each lTS2Fill As atcTimeseries In aTS2Fill
      i += 1
      Logger.Dbg("Filling station " & i & " of " & aTS2Fill.Count)
      lOStr = "Filling Daily Precip for " & lTS2Fill.ToString & ", " & lTS2Fill.Attributes.GetValue("Description") & vbCrLf
      s = MissingDataSummary(lTS2Fill, -999, -998, -1, 100, 1)
      lStr = StrSplit(s, "DETAIL:DATA", "")
      If Len(s) > 0 Then 'missing data found
        lDist = Nothing
        lDist = CalcPrecipDistances(lTS2Fill, aTSAvail)
        lFAnn = lTS2Fill.Attributes.GetValue("PRECIP")
        lSJDay = lTS2Fill.Dates.Value(0)
        While Len(s) > 0
          lStr = StrSplit(s, ",", "")
          lMJDay = CDbl(StrSplit(s, ",", ""))
          lMLen = CLng(StrSplit(s, ",", ""))
          J2Date(lMJDay, ld)
          ls = "  For period starting " & ld(0) & "/" & ld(1) & "/" & ld(2) & " lasting " & lMLen & " days:" & vbCrLf
          lCurInd = 0
          For k = 1 To lMLen
            lFPos = lMJDay - lSJDay + k
            lFillOT = CurrentObsTime(aTSObsTimes(i), lFPos)
            j = 0
            While j < aTSAvail.Count
              lInd = lDist.IndexFromKey(CStr(j))
              lTSer = aTSAvail.ItemByIndex(lInd)
              lFVal = -1
              If (lMJDay + k - 1) >= lTSer.Dates.Value(0) And _
                 (lMJDay + k - 1) < lTSer.Dates.Value(lTSer.numValues) Then
                'within date range, check value
                If lTSer.Attributes.GetValue("TU") = atcTimeUnit.TUDay Then 'check daily value
                  lSPos = lMJDay - lTSer.Dates.Value(0) + k
                  If lTSer.Value(lSPos) > -1 And lTSer.Value(lSPos) < 100 Then 'check obs time
                    lStaOT = CurrentObsTime(aTSObsTimes(lInd), lSPos)
                    If Math.Abs(lStaOT - lFillOT) <= 2 Then 'obs times close enough
                      lFVal = lTSer.Value(lSPos)
                    End If
                  End If
                ElseIf lTSer.Attributes.GetValue("TU") = atcTimeUnit.TUHour Then '*** for now, assume hourly !!!
                  lSPos = 24 * (lMJDay - lTSer.Dates.Value(0) + k - 1) + lFillOT
                  lFVal = 0
                  For ii = lSPos To lSPos - 23 Step -1
                    If ii > 0 And ii <= lTSer.numValues Then
                      lFVal += lTSer.Value(ii)
                    End If
                  Next ii
                End If
                If lFVal > -0.1 And lFVal < 100 Then 'valid fill value found
                  lStaAnn = lTSer.Attributes.GetValue("PRECIP")
                  If lFAnn > 0 And lStaAnn > 0 Then 'adjust for annual averages
                    lTS2Fill.Value(lFPos) = lFAnn / lStaAnn * lFVal
                  Else
                    lTS2Fill.Value(lFPos) = lFVal
                  End If
                  J2Date(lMJDay + k - 1, ld)
                  If lCurInd <> lInd Then 'changing station used to fill
                    If lTSer.Attributes.GetValue("TU") = atcTimeUnit.TUDay Then
                      lTUStr = "Daily"
                    Else 'assume hourly
                      lTUStr = "Hourly"
                    End If
                    ls = ls & "    Filling from " & lTUStr & " TS# " & lTSer.ToString & ", " & lTSer.Attributes.GetValue("Description") & vbCrLf
                    lCurInd = lInd
                  End If
                  ls = ls & "      " & ld(0) & "/" & ld(1) & "/" & ld(2) & " - " & lTS2Fill.Value(lFPos) & vbCrLf
                  j = aTSAvail.Count
                End If
              End If
              j += 1
            End While
          Next k
          lOStr &= ls
          lStr = StrSplit(s, "DETAIL:DATA", "")
        End While
      Else 'no missing data
        lOStr &= "  No missing data to fill!" & vbCrLf
      End If
      lFName = "DV_Fill_" & lTS2Fill.Attributes.GetValue("Location") & ".sum"
      SaveFileString(lFName, lOStr)
    Next
  End Sub

  Public Function CalcPrecipDistances(ByVal aTSer As atcTimeseries, ByVal aStations As atcCollection) As atcCollection
    'calculate distance between a single station (lTSer) and a collection of nearby "Stations"
    'considering both geographic distance and difference between annual precipitation averages from PRISM
    Dim i As Integer
    Dim lAnnAdj As Double
    Dim lTSerAnn As Double
    Dim lStaAnn As Double
    Dim lCompareAnnuals As Boolean
    Dim lGDist As Double
    Dim lDistance(aStations.Count - 1) As Double
    Dim lOrder(aStations.Count - 1) As Integer
    Dim lRank(aStations.Count - 1) As Integer
    Dim lDist As New atcCollection

    lTSerAnn = aTSer.Attributes.GetValue("PRECIP")
    If lTSerAnn = 0 Then
      lCompareAnnuals = False
    Else
      lCompareAnnuals = True
    End If
    For i = 0 To aStations.Count - 1
      lAnnAdj = 1 'assume no adjustment for annual averages
      If lCompareAnnuals Then
        lStaAnn = aStations.ItemByIndex(i).Attribnumeric("PRECIP")
        If lStaAnn > 0 Then
          lAnnAdj = lTSerAnn / lStaAnn
          If lAnnAdj < 1 Then lAnnAdj = 1 / lAnnAdj
        End If
      End If
      lGDist = GeoDist(aTSer, aStations.ItemByIndex(i)) 'now calculate geographic distance
      lDistance(i) = lAnnAdj * lGDist
    Next i
    SortRealArray(0, aStations.Count, lDistance, lOrder)
    SortIntegerArray(0, aStations.Count, lOrder, lRank)
    For i = 0 To aStations.Count - 1
      lDist.Add(lDistance(i), CStr(lRank(i)))
    Next i
    Return lDist
  End Function

  Public Function GeoDist(ByVal aTS1 As atcTimeseries, ByVal aTS2 As atcTimeseries) As Single
    'calculate the geographic distance between two stations (TSers)
    'based on projected feet ()
    Dim X1 As Double = aTS1.Attributes.GetValue("UBC024")
    Dim Y1 As Double = aTS1.Attributes.GetValue("UBC025")
    Dim X2 As Double = aTS2.Attributes.GetValue("UBC024")
    Dim Y2 As Double = aTS2.Attributes.GetValue("UBC025")
    If X1 = 0 Or X2 = 0 Or Y1 = 0 Or Y2 = 0 Then 'something wrong, make it huge
      GeoDist = 1.0E+30
    Else
      GeoDist = Math.Sqrt((X1 - X2) ^ 2 + (Y1 - Y2) ^ 2)
    End If
  End Function

  Public Function CurrentObsTime(ByVal aTS As atcTimeseries, ByVal aPos As Integer) As Integer
    'given a daily Timeseries and a position in its data,
    'return the current observation time
    Dim lOT As Integer = -1
    Dim lInc As Integer = -1 'look back in values if initial "pos" value is undefined
    Dim lCPos As Integer = aPos

    While lOT < 0
      If aTS.Value(lCPos) > 0 And aTS.Value(lCPos) < 25 Then
        lOT = aTS.Value(lCPos)
      Else
        lCPos = lCPos + lInc
      End If
      If lCPos < 1 Then 'at beginning of values, look ahead of initial "pos"
        lInc = 1
        lCPos = aPos + 1
      ElseIf lCPos > aTS.numValues Then 'at end of values, no obs time found, set to 24
        lOT = 24
      End If
    End While
    Return lOT
  End Function

  Public Function ClosestHourly(ByVal aTSer As atcTimeseries, ByVal aStations As atcCollection, ByVal aDV As Double, ByVal aSDt As Double, ByVal aEDt As Double, ByVal aTol As Double, ByVal aNSta As Integer) As atcTimeseries
    'find the hourly station (from the collection Stations) that is closest to
    'a daily precip station (lTSer) considering both geographic distance and
    'difference between daily totals for the day specified by sdt and edt
    '(hourly daily totals adjusted based on PRISM average annual values)
    Dim i As Integer
    Dim lTSerAnn As Double = aTSer.Attributes.GetValue("PRECIP")
    Dim lStaAnn As Double
    Dim lCompareAnnuals As Boolean
    Dim lGDist As Double
    Dim lHrDist As Double
    Dim lClosestDist As Double = Double.MaxValue
    Dim lDaySumHrTSer As atcTimeseries
    Dim lStaTSer As atcTimeseries
    Dim lHrTSer As atcTimeseries
    Dim lDaySum As Double
    Dim lRatio As Double
    Dim lHrInd As Integer
    Dim lInd As Integer
    Static lDist As atcCollection
    Static lCurID As Integer

    If aTSer.Attributes.GetValue("ID") <> lCurID Then 'calculate station distances
      lDist = New atcCollection
      Dim lDistance(aStations.Count - 1) As Double
      Dim lOrder(aStations.Count - 1) As Integer
      Dim lRank(aStations.Count - 1) As Integer
      Dim lAnnAdj As Double
      If lTSerAnn = 0 Then
        lCompareAnnuals = False
      Else
        lCompareAnnuals = True
      End If
      For i = 0 To aStations.Count - 1
        lAnnAdj = 1 'assume no adjustment for annual averages
        lStaTSer = aStations.ItemByIndex(i)
        If lCompareAnnuals Then
          lStaAnn = lStaTSer.Attributes.GetValue("PRECIP")
          If lStaAnn > 0 Then
            lAnnAdj = lTSerAnn / lStaAnn
            If lAnnAdj < 1 Then lAnnAdj = 1 / lAnnAdj
          End If
        End If
        lGDist = GeoDist(aTSer, lStaTSer) 'now calculate geographic distance
        lDistance(i) = lAnnAdj * lGDist
      Next i
      SortRealArray(0, aStations.Count, lDistance, lOrder)
      SortIntegerArray(0, aStations.Count, lOrder, lRank)
      For i = 0 To aStations.Count - 1
        lDist.Add(CStr(lRank(i)), lDistance(i))
      Next i
      lCurID = aTSer.Attributes.GetValue("ID")
    End If

    lHrTSer = Nothing
    For i = 0 To aNSta - 1 'look through clostest nsta stations for an acceptable daily total
      lInd = lDist.IndexFromKey(CStr(i))
      lStaTSer = aStations.ItemByIndex(lInd)
      lDaySumHrTSer = SubsetByDate(lStaTSer, aSDt, aEDt, Nothing)
      lDaySum = lDaySumHrTSer.Attributes.GetValue("Sum")
      If lDaySum > 0 Then
        If lCompareAnnuals Then
          lStaAnn = lDaySumHrTSer.Attributes.GetValue("PRECIP")
          If lStaAnn > 0 Then
            lDaySum = lDaySum * lTSerAnn / lStaAnn
          End If
        End If
        lRatio = aDV / lDaySum
        If lRatio > 1 Then lRatio = 1 / lRatio
        If lRatio >= 1 - aTol Then 'acceptable daily total, include distance factor
          lGDist = GeoDist(aTSer, lDaySumHrTSer) 'now calculate geographic distance
          'try 2 as a "weighting factor" for daily precip ratio
          lHrDist = (2 - lRatio) * lGDist
          If lHrDist < lClosestDist Then
            lClosestDist = lHrDist
            lHrTSer = lDaySumHrTSer
          End If
        End If
      End If
    Next i
    Return lHrTSer
  End Function

  Public Function MissingDataSummary(ByVal aTSer As atcTimeseries, Optional ByVal aMVal As Double = -9.99, Optional ByVal aMAcc As Double = -9.98, Optional ByVal aFMin As Double = -1, Optional ByVal aFMax As Double = 90000, Optional ByVal aRepTyp As Integer = 0) As String

    Dim i As Integer
    Dim j As Integer
    Dim lMisPd As Integer
    Dim lNV As Integer
    Dim lMPos As Integer
    Dim lMCod As Integer
    Dim lNMVals As Integer
    Dim lPos As Integer
    Dim lTotMissIncs As Integer
    Dim lNMVPd As Integer = 0
    Dim lNMDPd As Integer = 0
    Dim lNBVPd As Integer = 0
    Dim lTotMV As Integer = 0
    Dim lTotMD As Integer = 0
    Dim lTotBV As Integer = 0
    Dim lAccVal As Double
    Dim ldate(5) As Integer
    Dim lDateStr As String
    Dim lDateStrEnd As String
    Dim lTSStr As String
    Dim lTUStr() As String = {"", "seconds", "minutes", "hours", "days", "months", "years"}
    Dim s As String
    Dim ls As String

    If aRepTyp = 0 Then
      s = "For Data-set " & aTSer.ToString
      ls = aTSer.Attributes.GetValue("Description")
      If Len(ls) > 0 Then
        s = s & ", " & ls
      End If
      s = s & vbCrLf
    Else
      s = "STATION:HEADER  Index, Scenario, Location, Constituent, Description, TStep, TUnits" & vbCrLf
      s = s & "STATION:DATA  " & aTSer.Attributes.GetFormattedValue("ID") & ", " & _
                                 aTSer.Attributes.GetValue("Scenario") & ", " & _
                                 aTSer.Attributes.GetValue("Location") & ", " & _
                                 aTSer.Attributes.GetValue("Constituent") & ", " & _
                                 aTSer.Attributes.GetValue("Description") & ", " & _
                                 aTSer.Attributes.GetValue("TS") & ", " & _
                                 aTSer.Attributes.GetValue("TU") & vbCrLf
    End If
    'build time step/units string for display
    lTSStr = lTUStr(aTSer.Attributes.GetValue("TU"))  'start with time units
    If aTSer.Attributes.GetValue("TS") > 1 Then 'need time step too
      lTSStr = aTSer.Attributes.GetValue("TS") & " " & lTSStr
      'remove plural from time units, add 'intervals'
      lTSStr = lTSStr.TrimEnd("s") & " intervals"
    End If
    lMisPd = 0
    lMCod = 1
    lPos = 1
    Do While lMCod > 0
      Call NxtMis(lPos, aTSer.Values, aMVal, aMAcc, aFMin, aFMax, lMCod, lMPos, lNMVals, lAccVal)
      If lMCod > 0 Then 'somethings missing
        J2Date(aTSer.Dates.Value(lMPos), ldate)
        lDateStr = ldate(0) & "/" & ldate(1) & "/" & ldate(2)
        If aTSer.Attributes.GetValue("TU") < 4 Then 'display h,m,s along with y,m,d
          lDateStr &= " " & ldate(3) & ":" & ldate(4) & ":" & ldate(5)
        End If
        If lMCod = 1 Then 'missing values
          ls = " of missing values starting "
          lNMVPd += 1
          lTotMV += lNMVals
        ElseIf lMCod = 2 Then 'accumulated values
          ls = " of missing time distribution starting "
          lNMDPd += 1
          lTotMD += lNMVals
        ElseIf lMCod = 3 Then 'screwball values
          ls = " of faulty values starting "
          lNBVPd += 1
          lTotBV += lNMVals
        End If
        lMisPd += 1
        lPos += lMPos + lNMVals
        If aRepTyp = 0 Then
          s &= "  " & lNMVals & " " & lTSStr & ls & lDateStr & vbCrLf
        Else
          s &= "DETAIL:HEADER  Index, Start, Length, Type, AccumVal" & vbCrLf
          s &= "DETAIL:DATA  " & lMisPd & ", " & Date2J(ldate) & ", " & lNMVals & ", " & lMCod & ", " & lAccVal & vbCrLf
        End If
      End If
    Loop
    lTotMissIncs = lTotMV + lTotMD + lTotBV
    If aRepTyp = 0 Then
      If lMisPd > 0 Then 'missing data found for this time series
        s &= lMisPd & " period(s) of missing or bad data." & vbCrLf
        s &= "TotalIncs:" & RightJustify(aTSer.numValues, 8) & vbCrLf & _
             "  MisValPd:" & RightJustify(lNMVPd, 9) & vbCrLf & _
             "  MisValTot:" & RightJustify(lTotMV, 8) & vbCrLf & _
             "  MisDisPd:" & RightJustify(lNMDPd, 9) & vbCrLf & _
             "  MisDisTot: " & RightJustify(lTotMD, 7) & vbCrLf & _
             "  BadValPd:" & RightJustify(lNBVPd, 9) & vbCrLf & _
             "  BadValTot:" & RightJustify(lTotBV, 8) & vbCrLf & _
             "  PctMiss: " & DoubleToString(100 * lTotMissIncs / aTSer.numValues, 9, "##0.00", , , 5) & vbCrLf
      Else 'nothing missing
        s &= "  No missing data for this time series" & vbCrLf
      End If
    Else
      J2Date(aTSer.Dates.Value(0), ldate)
      lDateStr = ldate(0) & "/" & ldate(1) & "/" & ldate(2)
      J2Date(aTSer.Dates.Value(aTSer.numValues), ldate)
      lDateStrEnd = ldate(0) & "/" & ldate(1) & "/" & ldate(2)
      s &= "SUMMARY:HEADER  Start, End, TotalIncs, TotalMissPd, TotalMissIncs, MisValPd, MisValTot, MisDisPd, MisDisTot, BadValPd, BadValTot, PctMiss" & vbCrLf
      s &= "SUMMARY:DATA  " & lDateStr & ", " & lDateStrEnd & ", " & _
                              aTSer.numValues & ", " & lMisPd & ", " & lTotMissIncs & ", " & _
                              lNMVPd & ", " & lTotMV & ", " & lNMDPd & ", " & lTotMD & ", " & _
                              lNBVPd & ", " & lTotBV & ", " & DoubleToString(100 * lTotMissIncs / aTSer.numValues, 6, "##0.00", , , 5) & vbCrLf
    End If
    Return s

  End Function

  Public Sub NxtMis(ByVal aBufPos As Integer, ByVal aDBuff() As Double, ByVal aValMis As Double, ByVal aValAcc As Double, ByVal aFaultMin As Double, ByVal aFaultMax As Double, ByRef aMisCod As Double, ByRef aMisPos As Integer, ByRef aNVals As Integer, ByRef aMVal As Double)

    'Find the next missing value or distribution or screwball value
    'in the data buffer (aDBUFF) starting at positions aBufPos.
    'If none found, return MISCOD = 0.

    'aBufPos  - size of data buffer array
    'aDBUFF   - array of data values in which to look for missing data
    'aValMis  - value indicating missing data values
    'aValAcc  - value indicating accumulated values (missing distribution)
    'aFaultMin- value indicating value is too low to be valid
    'aFaultMax- value indicating value is too high to be valid
    'aMisCod  - type of missing data
    '           1 - missing value
    '           2 - missing distribution
    '           3 - garbage value
    'aMisPos  - position in array where missing data starts
    'aNVals   - number of data values missing
    'aMVal    - accumulated value (for accumulated data) or dummy
    '           for other types of missing data

    Dim i As Integer = aBufPos
    Dim lMisFlg As Integer = 0
    Dim lAccFlg As Integer = 0
    Dim lBadFlg As Integer = 0
    Dim lDonFlg As Integer = 0

    Do While lDonFlg = 0 And i < aDBuff.Length
      If Double.IsNaN(aDBuff(i)) Or Math.Abs(aDBuff(i) - aValMis) < Double.Epsilon Then
        'we have a missing value
        If lAccFlg > 0 Then
          'we were in the midst of an accum value array,
          'change it all to missing because dont know total
          lMisFlg = lAccFlg
          lAccFlg = 0
        End If
        lMisFlg += 1
        If lMisFlg = 1 Then
          'first missing value, save start position
          aMisPos = i
        End If
      ElseIf Math.Abs(aDBuff(i) - aValAcc) < Double.Epsilon Then
        'we have an accumulated value
        If lMisFlg > 0 Then
          'report only missing values now, will get accum next time
          lDonFlg = 1
        Else
          'accumulated value w/no distribution
          lAccFlg += 1
          If lAccFlg = 1 Then
            'first accumulated value, save start position
            aMisPos = i
          End If
        End If
      ElseIf aDBuff(i) < aFaultMin Or aDBuff(i) > aFaultMax Then
        'we have a screwball value
        lBadFlg += 1
        If lBadFlg = 1 Then
          'first bad value, save start position
          aMisPos = i
        End If
      Else
        'no problem with this value
        If lMisFlg > 0 Or lAccFlg > 0 Or lBadFlg > 0 Then
          'end of missing period
          lDonFlg = 1
          If lAccFlg > 0 Then
            'include first good value in accumulated count
            lAccFlg += 1
          End If
        End If
      End If
      'check next data value
      i += 1
    Loop

    If lMisFlg > 0 Then
      'missing values
      aMisCod = 1
      aNVals = lMisFlg
      aMVal = Double.NaN
    ElseIf lAccFlg > 0 Then
      'missing distribution
      aMisCod = 2
      aNVals = lAccFlg
      'save accumulated value for possible distribution later
      aMVal = aDBuff(i)
    ElseIf lBadFlg > 0 Then
      'screwball values
      aMisCod = 3
      aNVals = lBadFlg
      aMVal = -1.0E+30
    Else
      'no missing data in buffer
      aMisCod = 0
      aMisPos = 0
      aNVals = 0
      aMVal = 0.0#
    End If

  End Sub

  Public Sub BuildDBFSummary(ByVal aSummStr As String, ByVal aFName As String)
    Dim i As Integer ', j As Long, TSCount As Long, 
    Dim lFldLen As Integer
    'Dim tmpv As Long, maxv As Long, 
    Dim lTmpStr As String
    Dim lStr As String
    Dim lFldNames As New atcCollection
    Dim lFldTypes As New atcCollection
    Dim lFirstVals As New atcCollection
    Dim lDBF As New atcTableDBF

    Logger.Dbg("BuildDBFSummary:Start")
    'maxv = -1000000.0#
    'TSCount = 0
    'i = InStr(s, "SUMMARY:DATA")
    'While i > 0
    '  i = i + 12
    '  j = InStr(i, s, ",")
    '  tmpv = CLng(Mid(s, i, j - i))
    '  If tmpv > maxv Then maxv = tmpv
    '  TSCount = TSCount + 1
    '  i = InStr(j, s, "SUMMARY:DATA")
    'End While
    'tmpstr = maxv
    'FldLen = Len(tmpstr)
    lFldLen = 6
    'build dbf file
    'dbf.NumRecords = TSCount
    'extract field names, first from Station record
    lTmpStr = StrSplit(aSummStr, "STATION:HEADER", "")
    lTmpStr = StrSplit(aSummStr, vbCrLf, "")
    While Len(lTmpStr) > 0
      lFldNames.Add(StrSplit(lTmpStr, ",", ""))
    End While
    Logger.Dbg("BuildDBFSummary:Extracted field names from Station record")

    lTmpStr = StrSplit(aSummStr, "STATION:DATA", "")
    lTmpStr = StrSplit(aSummStr, vbCrLf, "")
    While Len(lTmpStr) > 0
      lStr = StrSplit(lTmpStr, ",", "")
      If lStr.Length > 1 And lStr.StartsWith("0") Then  'preceeding 0 usually means character field
        lFldTypes.Add("C")
      ElseIf Not IsNumeric(lStr) Then
        lFldTypes.Add("C")
      Else
        lFldTypes.Add("N")
      End If
      lFirstVals.Add(lStr)
    End While
    Logger.Dbg("BuildDBFSummary:Extracted field types from Station record")

    'now from Summary record
    lTmpStr = StrSplit(aSummStr, "SUMMARY:HEADER", "")
    lTmpStr = Trim(StrSplit(aSummStr, vbCrLf, ""))
    While Len(lTmpStr) > 0
      lFldNames.Add(StrSplit(lTmpStr, ",", ""))
    End While
    Logger.Dbg("BuildDBFSummary:Extracted field names from Summary record")

    lTmpStr = StrSplit(aSummStr, "SUMMARY:DATA", "")
    lTmpStr = StrSplit(aSummStr, vbCrLf, "")
    While Len(lTmpStr) > 0
      lStr = StrSplit(lTmpStr, ",", "")
      If lStr.IndexOf("/") > 0 Then 'date field
        lFldTypes.Add("D")
      ElseIf lStr.Length > 1 And lStr.StartsWith("0") Then  'preceeding 0 usually means character field
        lFldTypes.Add("C")
      ElseIf Not IsNumeric(lStr) Then
        lFldTypes.Add("C")
      Else
        lFldTypes.Add("N")
      End If
      lFirstVals.Add(lStr)
    End While
    Logger.Dbg("BuildDBFSummary:Extracted field types names from Summary record")
    lDBF.NumFields = lFldNames.Count
    Logger.Dbg("BuildDBFSummary:Set number of fields")

    For i = 0 To lDBF.NumFields - 1
      lDBF.FieldType(i + 1) = lFldTypes.ItemByIndex(i)
      lDBF.FieldName(i + 1) = lFldNames.ItemByIndex(i)
      If lFldTypes.ItemByIndex(i) = "N" Then
        lDBF.FieldLength(i + 1) = lFldLen 'use length of max number for numeric fields
      ElseIf lFldTypes.ItemByIndex(i) = "D" Then
        lDBF.FieldLength(i + 1) = 10
      Else
        If lFldNames.ItemByIndex(i) = "Description" Then
          lDBF.FieldLength(i + 1) = 32
        Else 'all other character fields use width of 8
          lDBF.FieldLength(i + 1) = 8
        End If
      End If
      If i < lDBF.NumFields Then
        lDBF.FieldDecimalCount(i + 1) = 0
      Else
        lDBF.FieldDecimalCount(i + 1) = 1 'last field (percent missing) needs a decimal
      End If
    Next i
    Logger.Dbg("BuildDBFSummary:Set DBF Field info")

    lDBF.InitData()
    lDBF.CurrentRecord = 1
    Logger.Dbg("BuildDBFSummary:Initialized data for DBF")
    For i = 0 To lDBF.NumFields - 1
      lDBF.Value(i + 1) = lFirstVals.ItemByIndex(i)
    Next i
    Logger.Dbg("BuildDBFSummary:Set first record values")

    While Len(aSummStr) > 0
      lDBF.CurrentRecord = lDBF.CurrentRecord + 1
      lTmpStr = StrSplit(aSummStr, "STATION:DATA", "")
      lTmpStr = StrSplit(aSummStr, vbCrLf, "")
      i = 0
      While Len(lTmpStr) > 0
        i += 1
        lDBF.Value(i) = StrSplit(lTmpStr, ",", "")
      End While
      lTmpStr = StrSplit(aSummStr, "SUMMARY:DATA", "")
      lTmpStr = StrSplit(aSummStr, vbCrLf, "")
      While Len(lTmpStr) > 0
        i += 1
        lDBF.Value(i) = StrSplit(lTmpStr, ",", "")
      End While
    End While
    Logger.Dbg("BuildDBFSummary:set all record values")

    lDBF.WriteFile(aFName)
    Logger.Dbg("BuildDBFSummary:Wrote DBF File")
  End Sub

  Public Sub SortIntegerArray(ByVal aOpt As Integer, ByVal aCnt As Integer, ByRef aIVal() As Integer, ByRef aPos() As Integer)
    ' ##SUMMARY Sorts integers in array into ascending order.
    ' ##PARAM aOpt I Sort option (0 = sort in place, 1 = move values in array to sorted position)
    ' ##PARAM aCnt I Count of integers to sort
    ' ##PARAM aIVal M Array of integers to sort
    ' ##PARAM aPos O Array containing sorted order of integers
    Dim i As Long
    Dim j As Long
    Dim jpt As Long
    Dim jpt1 As Long
    Dim itmp As Long
    ' ##LOCAL i - long counter for outer loop
    ' ##LOCAL j - long counter for inner loop
    ' ##LOCAL jpt - long pointer to j index
    ' ##LOCAL jpt1 - long pointer to (j + 1) index
    ' ##LOCAL itmp - long temporary holder for values in iVal array

    'set default positions(assume in order)
    For i = 1 To aCnt
      aPos(i) = i
    Next i

    'make a pointer to values with bubble sort
    For i = aCnt To 2 Step -1
      For j = 1 To i - 1
        jpt = aPos(j)
        jpt1 = aPos(j + 1)
        If (aIVal(jpt) > aIVal(jpt1)) Then
          aPos(j + 1) = jpt
          aPos(j) = jpt1
        End If
      Next j
    Next i

    If (aOpt = 1) Then
      'move integer values to their sorted positions
      For i = 1 To aCnt
        If (aPos(i) <> i) Then
          'need to move ints, first save whats in target space
          itmp = aIVal(i)
          'move sorted data to target position
          aIVal(i) = aIVal(aPos(i))
          'move temp data to source position
          aIVal(aPos(i)) = itmp
          'find the pointer to the other value we are moving
          j = i
50:       'CONTINUE
          j = j + 1
          If (aPos(j) <> i) Then GoTo 50
          aPos(j) = aPos(i)
          aPos(i) = i
        End If
      Next i
    End If

  End Sub

  Public Sub SortRealArray(ByVal aOpt As Integer, ByVal aCnt As Integer, _
                           ByRef aRVal() As Double, ByRef aPos() As Integer)
    ' ##SUMMARY Sorts array of real numbers into ascending order.
    ' ##PARAM aOpt I Integer indicating sort option: 0 - sort in place, 
    '                                                1 - move values in array to sorted position.
    ' ##PARAM aCnt I Count of real numbers to sort.
    ' ##PARAM aRVal M Array of real numbers to sort.
    ' ##PARAM aPos O Integer array containing sorted order of real numbers.
    Dim i As Long
    Dim j As Long
    Dim jpt As Long
    Dim jpt1 As Long
    Dim rtmp As Single
    ' ##LOCAL i - long counter for outer loop
    ' ##LOCAL j - long counter for inner loop
    ' ##LOCAL jpt - long pointer to j index
    ' ##LOCAL jpt1 - long pointer to (j + 1) index
    ' ##LOCAL itmp - long temporary holder for values in rVal array

    'set default positions(assume in order)
    For i = 1 To aCnt
      aPos(i) = i
    Next i

    'make a pointer to values with bubble sort
    For i = aCnt To 2 Step -1
      For j = 1 To i - 1
        jpt = aPos(j)
        jpt1 = aPos(j + 1)
        If (aRVal(jpt) > aRVal(jpt1)) Then
          aPos(j + 1) = jpt
          aPos(j) = jpt1
        End If
      Next j
    Next i

    If (aOpt = 1) Then
      'move real values to their sorted positions
      For i = 1 To aCnt
        If (aPos(i) <> i) Then
          'need to move reals, first save whats in target space
          rtmp = aRVal(i)
          'move sorted data to target position
          aRVal(i) = aRVal(aPos(i))
          'move temp data to source position
          aRVal(aPos(i)) = rtmp
          'find the pointer to the other value we are moving
          j = i
50:       'CONTINUE
          j = j + 1
          If (aPos(j) <> i) Then GoTo 50
          aPos(j) = aPos(i)
          aPos(i) = i
        End If
      Next i
    End If

  End Sub

  Public Function ReadNOAAAttributes(ByVal aFilename As String) As atcCollection
    Dim lInFile As Integer
    'Dim j As Integer
    Dim lStr As String
    Dim lStaCode As String
    Dim lStateCode As String
    Dim lDecDeg As Double
    Dim lNOAAattribs As atcCollection
    Dim lStations As New atcCollection
    Dim lStationIndex As Integer

    Dim inStream As New FileStream(aFilename, FileMode.Open, FileAccess.Read)
    Dim inBuffer As New BufferedStream(inStream)
    Dim inReader As New BinaryReader(inBuffer)
    'Open aFilename For Random As inFile Len = 295
    'j = 1

    Try
      Do 'Until EOF(lInFile)
        'Get linFile, j, lstr
        lStr = NextLine(inReader)
        lStaCode = Mid(lStr, 13, 6)
        If IsNumeric(lStaCode) Then
          lStateCode = Mid(lStr, 72, 2)
          If lStaCode > 0 And lStateCode.ToUpper = "GA" Then
            lNOAAattribs = New atcCollection
            lNOAAattribs.Add("STAID", lStaCode)
            lNOAAattribs.Add("STCODE", Mid(lStr, 72, 2))
            lNOAAattribs.Add("COCODE", Mid(lStr, 75, 30))
            lNOAAattribs.Add("TMZONE", Mid(lStr, 106, 5))
            lNOAAattribs.Add("STANAM", Mid(lStr, 112, 30))
            lNOAAattribs.Add("DESCRP", Mid(lStr, 143, 30))
            lNOAAattribs.Add("START", Mid(lStr, 176, 6))
            lNOAAattribs.Add("END", Mid(lStr, 185, 6))
            If IsNumeric(Mid(lStr, 192, 3)) Then
              lDecDeg = CLng(Mid(lStr, 192, 3))
              If lDecDeg > 0 Then
                lDecDeg = lDecDeg + CLng(Mid(lStr, 196, 2)) / 60 + CLng(Mid(lStr, 199, 2)) / 3600
              Else
                lDecDeg = lDecDeg - (CLng(Mid(lStr, 196, 2)) / 60 + CLng(Mid(lStr, 199, 2)) / 3600)
              End If
              lNOAAattribs.Add("LATDEG", lDecDeg)
            End If
            If IsNumeric(Mid(lStr, 202, 4)) Then
              lDecDeg = CLng(Mid(lStr, 202, 4))
              If lDecDeg > 0 Then
                lDecDeg = lDecDeg + CLng(Mid(lStr, 207, 2)) / 60 + CLng(Mid(lStr, 210, 2)) / 3600
              Else
                lDecDeg = lDecDeg - (CLng(Mid(lStr, 207, 2)) / 60 + CLng(Mid(lStr, 210, 2)) / 3600)
              End If
              lNOAAattribs.Add("LNGDEG", lDecDeg)
            End If
            lNOAAattribs.Add("ELEV", Mid(lStr, 223, 2))
            lStationIndex = lStations.IndexFromKey(lStaCode)
            If lStationIndex > 0 Then 'If there is an older station history entry, overwrite it
              If CDbl(lNOAAattribs.ItemByKey("END")) > CDbl(lStations.ItemByIndex(lStationIndex).ItemByKey("END")) Then
                lStations.RemoveAt(lStationIndex)
                lStations.Add(lStaCode, lNOAAattribs)
              End If
            Else
              lStations.Add(lStaCode, lNOAAattribs)
            End If
          End If
        End If
        'j = j + 1
      Loop
    Catch endEx As EndOfStreamException
    End Try
    'Close(lInFile)

  End Function

End Module
