VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "ATCclsTserDate"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit
'##MODULE_REMARKS Copyright 2001-3AQUA TERRA Consultants - Royalty-free use permitted under open source license

Private pValues() As Double      'Julian date array
Private pFlags() As Long       'date interval boundary 6=year, 5=month, 4=day, 3=hour, 2=min, 1=sec, 0=finer than sec
Private pFile As ATCclsTserFile 'data file these dates come from
Private pSummary As ATTimSerDateSummary
Private pSerial As Long

Public Function IndexAtOrAfter(targetDate As Double) As Long
'##SUMMARY Index of value at or after specified julian date.
  Dim retval&
  
  If pSummary.NVALS = 0 Then
    retval = 1
  Else
    retval = IndexAtOrBefore(targetDate)
    If Value(retval) < targetDate Then retval = retval + 1
  End If
  IndexAtOrAfter = retval
End Function

Public Function IndexAtOrBefore(targetDate As Double) As Long
'##SUMMARY Index of value at or before specified julian date.
  
  If pSummary.NVALS = 0 Then
    IndexAtOrBefore = 1
  ElseIf targetDate <= Value(1) Then
    IndexAtOrBefore = 1
  ElseIf targetDate >= pSummary.EJDay Then
    IndexAtOrBefore = pSummary.NVALS
  ElseIf pSummary.CIntvl And pSummary.Intvl > 0 And pSummary.Intvl < 28 Then 'can calculate index
    IndexAtOrBefore = Int((targetDate - pSummary.SJDay) / pSummary.Intvl)
  Else                        'binary search for closest value
    Dim retIndex As Long, minIndex As Long, maxIndex As Long
    Dim retval As Double, minVal As Double, maxVal As Double
    minIndex = 1
    maxIndex = pSummary.NVALS
    minVal = Value(minIndex)
    maxVal = Value(maxIndex)
    While maxIndex - minIndex > 1
      retIndex = minIndex + (maxIndex - minIndex) / 2
      retval = Value(retIndex)
      If retval > targetDate Then
        maxIndex = retIndex
        maxVal = retval
      ElseIf retval < targetDate Then
        minIndex = retIndex
        minVal = retval
      Else
        maxIndex = retIndex
        minIndex = retIndex
        'maxVal = retVal
        'minVal = retVal
      End If
    Wend
    IndexAtOrBefore = minIndex
  End If
End Function

'##SUMMARY Returns a subset of dates starting after s and ending at or before e. _
Optionally sets StartFrom to the index in the old array where the new one starts
Public Function SubSetByDate(s As Double, e As Double, Optional ByRef StartFrom As Long) As ATCclsTserDate
  Dim indexStart&, indexEnd&, newNvals&
  Dim newdate As ATCclsTserDate, newSummary As ATTimSerDateSummary
  Dim newFlags() As Long
  Dim newVals() As Double
  
  indexStart = IndexAtOrAfter(s)
  indexEnd = IndexAtOrBefore(e)
  
  'Don't want to include value AT start date requested, just after it
  If Value(indexStart) - JulianSecond < s Then indexStart = indexStart + 1
  If Not IsMissing(StartFrom) Then StartFrom = indexStart
  newNvals = indexEnd - indexStart + 1
  
  Set newdate = New ATCclsTserDate
  newSummary = pSummary
  
  With newSummary
    .NVALS = newNvals
    .SJDay = Value(indexStart)
    If .CIntvl Then .SJDay = TimSubtract(.SJDay, .Tu, .ts)
    .EJDay = Value(indexEnd)
  End With
  newdate.Summary = newSummary

  If pSummary.CIntvl Or newNvals < 1 Then                'Don't need to copy values array
    ReDim newVals(0)
    newdate.Values = newVals
  Else                              'Copy values array
    ReDim newVals(newNvals)
    CopyMemory newVals(1), pValues(indexStart), newNvals * 8 '8=bytes per Double value
    newdate.Values = newVals
    newdate.calcSummary
  End If
  
  If UBound(pFlags) < indexEnd Or newNvals < 1 Then 'No flags to copy
    ReDim newFlags(0)
  Else                              'Copy applicable flags
    ReDim newFlags(newNvals)
    CopyMemory newFlags(1), pFlags(indexStart), newNvals * 4 '4=bytes per Long value
  End If
  newdate.flags = newFlags
  
  Set SubSetByDate = newdate
End Function

Private Function TimSubtract(ByVal Jday As Double, ByVal Tu As ATCTimeUnit, ByVal ts As Long)
  Dim tmpdat(5) As Long
  Select Case Tu
    Case TUSecond:  TimSubtract = Jday - ts / 86400#
    Case TUMinute:  TimSubtract = Jday - ts / 1440#
    Case TUHour:    TimSubtract = Jday - ts / 24#
    Case TUDay:     TimSubtract = Jday - ts
    Case TUMonth:                   GoTo SubtractMonths
    Case TUYear:    ts = ts * 12:   GoTo SubtractMonths
    Case TUCentury: ts = ts * 1200: GoTo SubtractMonths
  End Select
  Exit Function

SubtractMonths:
  While ts > 0
    Call J2Date(Jday - JulianSecond, tmpdat)
    Jday = Jday - daymon(tmpdat(0), tmpdat(1))
    ts = ts - 1
  Wend
  TimSubtract = Jday
End Function

Public Function GetCommonDates(cod As Collection) As ATCclsTserDate
'##SUMMARY Returns ATCclsTserDate containing all dates from the overlapping time _
period of all members in cod (which is a collection of ATCclsTserDate). _
The overlapping period is from the last start time to the first end time. _
If there is no overlapping time period, an empty ATCclsTserDate is returned.
  Dim ad As ATCclsTserDate, retval As ATCclsTserDate
  Dim lastStart As Double, firstEnd As Double, nextDate As Double 'modified julian
  Dim nod&        'Number of ATCclsTserDate on collection
  Dim dIndex&     'index into cod, nextIndex, and nextValue
  Dim nextdIndex& 'Which one has next date to add to newValues
  Dim nextNewDateIndex& 'Next position in newValues to fill
  Dim maxNewDates&      'size of newValues
  Dim nextIndex() As Long, nextValue() As Double 'Arrays tracking the leading edge of cod
  Dim lastIndex() As Long   'Index of last date to be used
  Dim newValues() As Double 'Date values to return
  Dim done As Long          'Number of elements of collection we are finished extracting values from
  
  nod = cod.Count
  If nod < 1 Then
NoCommonDates:
    GetCommonDates = New ATCclsTserDate
  ElseIf nod = 1 Then
    Set GetCommonDates = cod(1)
  Else
    Set retval = New ATCclsTserDate
    ReDim nextIndex(nod)
    ReDim nextValue(nod)
    ReDim lastIndex(nod)
    With cod(1).Summary
      lastStart = .SJDay
      firstEnd = .EJDay
    End With
    For dIndex = 2 To nod
      With cod(dIndex).Summary
        If .SJDay > lastStart Then lastStart = .SJDay
        If .EJDay < firstEnd Then firstEnd = .EJDay
      End With
    Next dIndex
    If firstEnd < lastStart Then
      GoTo NoCommonDates
    Else
      maxNewDates = 0
      For dIndex = 1 To nod 'skip to first overlapping value in each set of dates
        With cod(dIndex)
          nextIndex(dIndex) = .IndexAtOrAfter(lastStart)
          lastIndex(dIndex) = .IndexAtOrBefore(firstEnd)
          If nextIndex(dIndex) <= .Summary.NVALS Then
            nextValue(dIndex) = .Value(nextIndex(dIndex))
            'We will use at most all the dates from lastStart to firstEnd
            maxNewDates = maxNewDates + lastIndex(dIndex) - nextIndex(dIndex) + 1
          Else
            nextIndex(dIndex) = -1 'This should not happen if we have overlapping dates
          End If
        End With
      Next dIndex
      ReDim newValues(maxNewDates)
      done = 0
      nextNewDateIndex = 0
      While done < nod
        nextDate = 1E+300
        For dIndex = 1 To nod
          If nextIndex(dIndex) >= 0 Then
            If nextValue(dIndex) < nextDate Then
              nextdIndex = dIndex
              nextDate = nextValue(dIndex)
            End If
          End If
        Next dIndex
        newValues(nextNewDateIndex) = nextDate
        nextNewDateIndex = nextNewDateIndex + 1
        done = 0
        For dIndex = 1 To nod 'skip duplicate dates
          If nextIndex(dIndex) < 0 Then
            done = done + 1
          Else
            While nextValue(dIndex) <= nextDate And nextIndex(dIndex) <= lastIndex(dIndex)
              nextIndex(dIndex) = nextIndex(dIndex) + 1
              nextValue(dIndex) = cod(dIndex).Value(nextIndex(dIndex))
            Wend
            If nextIndex(dIndex) > lastIndex(dIndex) Then
              nextIndex(dIndex) = -1
              done = done + 1
            End If
          End If
        Next dIndex
      Wend
      ReDim Preserve newValues(nextNewDateIndex - 1)
      retval.Values = newValues
      retval.calcSummary
      Set GetCommonDates = retval
    End If
  End If
End Function

'##SUMMARY ATCclsTserFile associated with this date class
Public Property Set File(newFile As ATCData.ATCclsTserFile)
  Set pFile = newFile
End Property
Public Property Get File() As ATCclsTserFile
  Set File = pFile
End Property

Public Property Get Flag(index As Long) As Long
'##SUMMARY Get value of a flag associated with a specific date/value _
 pair.
  If UBound(pFlags) >= index Then
    Flag = pFlags(index)
  Else
    Flag = JDateIntrvl(Value(index))
  End If
End Property

'##SUMMARY Flags for this date class
Public Property Let flags(newFlags() As Long)
  pFlags = newFlags
End Property
Public Property Get flags() As Long()
  verify_dates
  flags = pFlags
End Property

Public Property Get Serial() As Long
'##SUMMARY Serial number assigned in order of creation. _
Guaranteed to be unique at runtime but will be different from one run to another
  Serial = pSerial
End Property

'##SUMMARY Summary of dates in this date class.
Public Property Let Summary(newV As ATCData.ATTimSerDateSummary)
  pSummary = newV
End Property
Public Property Get Summary() As ATCData.ATTimSerDateSummary
  Summary = pSummary
End Property

'##SUMMARY Date value at specified index
Public Property Get Value(index As Long) As Double
  Dim tempdate&(6), newdate&(6)

  With pSummary
    If index < 1 Then
      Value = .SJDay
    Else
      If UBound(pValues) >= index Then ' have date array
        Value = pValues(index)
      ElseIf .CIntvl Then ' calculate date if constant interval
        If .Intvl > 0 And .Intvl < 28 Then
          Value = .SJDay + .Intvl * index
        Else 'special case for long intervals
          Call J2Date(.SJDay, tempdate)
          Call TIMADD(tempdate, .Tu, .ts, index, newdate)
          Value = Date2J(newdate)
        End If
      Else
        Value = 0
      End If
    End If
  End With
End Property

Public Property Let Value(index As Long, newValue As Double)
  If UBound(pValues) >= index And index >= 0 Then
    pValues(index) = newValue
  Else
    MsgBox "Attempted to set date value at " & index & " outside available range (0 to " & UBound(pValues), vbOKOnly, "ATCclsTserDate"
  End If
End Property

'##SUMMARY Array of date values in this class
Public Property Let Values(newValues() As Double)
  pValues = newValues
End Property
Public Property Get Values() As Double()
  verify_dates
  Values = pValues
End Property

'##SUMMARY Call after populating Values array. _
Do not call for constant interval data that does not use Values array.
Public Sub calcSummary(Optional PointFlg As Boolean = False)
  Dim lintvl#, i&, yr&, mn&, dy&

  With pSummary
    .NVALS = UBound(pValues)
    If .NVALS > 0 Then
      .SJDay = pValues(1)
      .EJDay = pValues(.NVALS)
    Else
      .SJDay = 0
      .EJDay = 0
    End If
    If .EJDay < .SJDay Then
      MsgBox "Sanity check: .EJDay < .SJDay in calcSummary", vbOKOnly, "ATCclsTserDate"
      'Stop
    End If
    'constant interval
    .CIntvl = True 'assume yes
    If .NVALS > 2 Then
      'If pValues(0) > 0.00001 Then
      '  i = 0
      '  .NVals = .NVals + 1
      'Else
        i = 1
      'End If
      
      .Intvl = pValues(i + 1) - pValues(i) 'assume first valid
      For i = 2 To .NVALS
        lintvl = pValues(i) - pValues(i - 1)
        If Abs(lintvl - .Intvl) > 0.000001 Then 'might not be constant
          If ((lintvl >= 28 And lintvl <= 31) And _
              (.Intvl >= 28 And .Intvl <= 31)) Then
                  'monthly
          ElseIf ((lintvl >= 365 And lintvl <= 366) And _
                  (.Intvl >= 365 And .Intvl <= 366)) Then
                  'annual
          Else
            .CIntvl = False  ' not const intrvl
            .Intvl = -1
            Exit For
          End If
        End If
      Next i
    Else 'only one value
      .Intvl = 0
      .CIntvl = False
    End If
    
    'update summary
    If .CIntvl Then
      Call INVMJD(CLng(.SJDay), yr, mn, dy)
      If .Intvl >= 365 Then  'year
        If daymon(yr - 1, 2) = 29 Then
          .Intvl = 366
        Else
          .Intvl = 365
        End If
        .ts = 1
        .Tu = 6
      ElseIf .Intvl >= 28 Then 'month
        If mn > 1 Then
          .Intvl = daymon(yr, mn - 1)
        Else
          .Intvl = daymon(yr - 1, 12)
        End If
        .ts = 1
        .Tu = 5
      ElseIf .Intvl >= 1 Then 'day
        .ts = .Intvl
        .Tu = 4
      ElseIf .Intvl > 0.04 Then  'hour
        .ts = .Intvl * 24
        .Tu = 3
      ElseIf .Intvl > 0.00069 Then  'minute
        .ts = .Intvl * 24 * 60
        .Tu = 2
      Else
        .ts = .Intvl * 24 * 60 * 60  'second
        .Tu = 1
      End If
      If PointFlg Then
        .CIntvl = False
      Else
        .SJDay = .SJDay - .Intvl
      End If
    Else 'assume daily for non constant data
      .Tu = 4
      .ts = 1
    End If
  End With
End Sub

Private Sub verify_dates() 'Make sure we have an array of Values populated
  Dim j&, jd#
  
  With pSummary
    If UBound(pValues) > 0 Then ' have date array
    
    ElseIf .NVALS > 0 And .CIntvl Then 'try to generate
      ReDim pValues(.NVALS)
      ReDim pFlags(.NVALS)
      For j = 0 To .NVALS
        jd = TimAddJ(.SJDay, .Tu, .ts, j)
        pValues(j) = jd
        pFlags(j) = JDateIntrvl(jd)
      Next j
    End If
  End With
End Sub

Private Sub Class_Initialize()
  ReDim pValues(0)
  ReDim pFlags(0)
  pSerial = NextDateSerial
  NextDateSerial = NextDateSerial + 1
End Sub

Private Sub Class_Terminate()
  ReDim pValues(0)
  ReDim pFlags(0)
End Sub

Public Function Copy() As ATCclsTserDate
'##SUMMARY Returns a copy of this date class.
  Dim retval As ATCclsTserDate
  Set retval = New ATCclsTserDate
  Set retval.File = File
  retval.Values = pValues
  retval.flags = pFlags
  retval.Summary = pSummary
  Set Copy = retval
End Function
