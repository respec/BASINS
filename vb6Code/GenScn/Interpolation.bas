Attribute VB_Name = "Interpolation"
Option Explicit

'Slop factor for use in testing equality of floating point dates
Const JulianSecond = 1# / 86400# 'One day divided by (24 hours * 3600 seconds per hour)

Private nNewFeq&
Private oldFilIndex&()
Private oldNval&()

'de-allocate any added data structures we created to hold interpolated dates
Public Sub DeallocateModifiedTSer()
  Dim curFEQ&, curTS&, c&
  
  If nNewFeq > 0 Then
    For curTS = 0 To nts - 1
      'TSer(curTS).FilIndex = oldFilIndex(curTS)
      'TSer(curTS).NVal = oldNval(curTS)
    Next curTS
    
    'For curFEQ = 1 To nNewFeq
    '  With p.FeoData(p.FeoCount - 1)
        'For c = 0 To .LocCount - 1
        '  ReDim .LocInfo(c).Constit(0)
        'Next c
    '    ReDim .LocInfo(0)
    '    ReDim .JDay(0)
    '  End With
    'Next curFEQ
    
    'p.FeoCount = p.FeoCount - nNewFeq
    'ReDim Preserve p.FeoData(p.FeoCount - 1)
    'nNewFeq = 0
  End If
  'ModifiedTSer = False
End Sub

'Takes n timeseries in array ts
'When finished, timeseries values have been interpolated
' so each timser has the same number of values at the same times
' Only values in the overlapping time span are retained.
'If useDates(i) is true, all overlapping dates from ts(i) will be included
Public Sub InterpolateTimeseries(nts&, ts() As ATCclsTserData, useDates() As Boolean)
'  Dim curTS&, chkTS&, nextIndex&, jd&
'  Dim found As Boolean
'  Dim JSdt#, JEdt#, OverlapStart#, OverlapEnd#
'  Dim Timbuf() As Double
'  Dim nDates&
'  Dim newDates() As Double
'  Dim ConstInt As Boolean
'  Dim r&, c&
'  Dim nOldFeq&, oldTimCount&
'  Dim curTime#, oldTime1#, oldTime2#
'  Dim oldVal1#, oldVal2#
'  Dim newVals!()
'  Dim oldType$()
'
'  For curTS = 0 To nts - 1
'    If ts(curTS).Type <> "FEQ" Then
'      If ts(curTS).Type = "WDM" And useDates(curTS) = True Then
'        'It is OK to use just dates from WDM because then we don't have to change them
'      Else
'        MsgBox "Interpolate does not yet support changing dates of non-FEQ data"
'        Exit Sub
'      End If
'    End If
'  Next curTS
'
'  If nNewFeq > 0 Then DeallocateModifiedTSer
'
'  ReDim oldFilIndex(nts)
'  ReDim oldNval(nts)
'  ReDim oldType(nts)
'  For curTS = 0 To nts - 1
'    oldFilIndex(curTS) = ts(curTS).FilIndex
'    oldNval(curTS) = ts(curTS).NVal
'    oldType(curTS) = ts(curTS).Type
'  Next curTS
'
'  'nOldFeq = p.FeoCount
'  'get the data values for selected data sets
'  ConstInt = False
'  ModifiedTSer = False
'  Call FillTimSerExt(nts, ts(), ConstInt)
'  ModifiedTSer = True
'
'  BuildNewDateArray nts, ts, useDates, Timbuf, nDates, newDates
'
'  For curTS = 0 To nts - 1
'    found = False
'    For chkTS = 0 To curTS - 1
'      If oldFilIndex(chkTS) = ts(curTS).FilIndex And oldType(chkTS) = ts(curTS).Type Then
'        found = True
'        ts(curTS).FilIndex = ts(chkTS).FilIndex
'        chkTS = curTS
'      End If
'    Next chkTS
'    If Not found Then
'      MsgBox "Problem: Interpolate needs to work with ATTimSer"
'      'If ts(curTS).Type = "FEQ" Then 'add new feodata to project
'      '  ReDim Preserve p.FeoData(p.FeoCount)
'      '  p.FeoCount = p.FeoCount + 1
'      '  copyfeqdata p.FeoData(ts(curTS).FilIndex), p.FeoData(p.FeoCount - 1), True, False
'      '  ReDim p.FeoData(p.FeoCount - 1).JDay(nDates - 1)
'      '  For jd = 0 To nDates - 1
'      '    p.FeoData(p.FeoCount - 1).JDay(jd) = newDates(jd)
'      '  Next jd
'      '  p.FeoData(p.FeoCount - 1).TimCount = nDates
'      '  ts(curTS).FilIndex = p.FeoCount - 1
'      '  ts(curTS).spos = 0
'      'End If
'    End If
'  Next curTS
'
'  ReDim newVals(nDates)
'  For curTS = 0 To nts - 1
'    If ts(curTS).Type = "FEQ" Then
'      MsgBox "Problem: Interpolate needs to work with ATTimSer"
'      'With p.FeoData(oldFilIndex(curTS))
'      '  curTime = p.FeoData(p.FeoCount - 1).JDay(0)
'      '  oldTimCount = .TimCount
'      '  oldTime2 = .JDay(1)
'      '  nextIndex = 2
'      '  For jd = 0 To nDates - 1
'      '    curTime = p.FeoData(p.FeoCount - 1).JDay(jd)
'
'          'skip to overlapping date
'      '    While oldTime2 < curTime And nextIndex < oldTimCount
'      '      oldTime2 = .JDay(nextIndex)
'      '      nextIndex = nextIndex + 1
'      '    Wend
'
'      '    oldTime1 = .JDay(nextIndex - 2)
'      '    oldVal1 = ts(curTS).Vals(nextIndex - 2)
'      '    oldVal2 = ts(curTS).Vals(nextIndex - 1)
'
'      '    If Abs(curTime - oldTime1) < JulianSecond Then     'close enough to this time
'      '      newVals(jd) = oldVal1
'      '    ElseIf Abs(curTime - oldTime2) < JulianSecond Then 'close enough to that time
'      '      newVals(jd) = oldVal2
'      '    ElseIf (oldTime2 - oldTime1) > 0.0000001 Then      'interpolate value
'      '      newVals(jd) = oldVal1 + (curTime - oldTime1) * (oldVal2 - oldVal1) / (oldTime2 - oldTime1)
'      '    Else 'if they happen at the same time, take average
'      '      newVals(jd) = (oldVal1 + oldVal2) / 2
'      '    End If
'      '  Next jd
'      '  ts(curTS).NVal = nDates
'      '  Call J2Date(p.FeoData(p.FeoCount - 1).JDay(0), ts(curTS).sdat)
'      '  Call J2Date(p.FeoData(p.FeoCount - 1).JDay(nDates - 1), ts(curTS).edat)
'      '  ts(curTS).Sen = "Intrp:" & ts(curTS).Sen
'      '  ReDim ts(curTS).Vals(nDates - 1)
'      '  For jd = 0 To nDates - 1
'      '    ts(curTS).Vals(jd) = newVals(jd)
'      '  Next jd
'      'End With
'    End If
'  Next curTS
'  'nNewFeq = p.FeoCount - nOldFeq
'  Dim strMsg$
'  For curTS = 0 To nts - 1
'    If useDates(curTS) Then strMsg = strMsg & ts(curTS).id & ", "
'  Next curTS
'  strMsg = "Interpolated " & nts & " timeseries to " & nDates & " times" & vbCr & "from DSN(s) " & Left(strMsg, Len(strMsg) - 2)
'  MsgBox strMsg, vbOKOnly, "Interpolation Complete"
   MsgBox "Interpolation Broken"
End Sub

'Builds an array of julian dates starting with the latest start time in timbuf and ending with the earliest end time in timbuf
'If a time is less than a second from the previous time, it is not used
Private Sub BuildNewDateArray(nts&, ts() As ATCclsTserData, useDates() As Boolean, Timbuf#(), nDates&, newDates() As Double)
'  Dim curTS&            'counts through the set of timeseries in ts
'  Dim nextIndex&()      'keeps track of next unused time value in each ts
'  Dim nextNewDateIndex& 'location in newDates array where next date will be appended
'  Dim nextDate#         'Julian (day.fractionofday) date of potential next value for newDates
'  Dim thisDate#         'date we are checking as potential nextDate
'  Dim maxDates&         'size of newDates array
'  Dim prevDate#         'Julian (day.fractionofday) date of previous value for newDates
'
'  'Initialize nextIndex(), find latest start date and largest date array
'  nextDate = 0
'  ReDim nextIndex(nts)
'  For curTS = 0 To nts - 1
'    nextIndex(curTS) = 0
'
'    thisDate = Timbuf(0, curTS)
'    If thisDate > nextDate Then nextDate = thisDate
'
'    If ts(curTS).NVal > maxDates Then maxDates = ts(curTS).NVal
'  Next curTS
'  prevDate = nextDate
'
'  'Skip dates before last start date
'  For curTS = 0 To nts - 1
'    While Timbuf(nextIndex(curTS), curTS) < nextDate
'      nextIndex(curTS) = nextIndex(curTS) + 1
'      If nextIndex(curTS) >= ts(curTS).NVal Then GoTo NoOverlap
'    Wend
'  Next curTS
'
'  maxDates = maxDates * 2 'Start with twice as much room as largest ts
'  ReDim newDates(maxDates)
'
'  'To keep going after finding earliest end time:
'  'Uncomment the 2 lines "If nextIndex(curTS) >= 0 Then" and their "End If"s
'  'Change "Then nextDate = 0" to "Then nextIndex(curTS) = -1"
'  Do
'    DoEvents
'    nextDate = 0
'    For curTS = 0 To nts - 1
'      If useDates(curTS) Then
'        thisDate = Timbuf(nextIndex(curTS), curTS)
'        If nextDate = 0 Or thisDate < nextDate Then
'          nextDate = thisDate
'        End If
'      End If
'    Next curTS
'    If nextDate > 0 Then
'
'      If nDates = 0 Or nextDate - prevDate >= JulianSecond Then
'        'Put new date in array
'        If nDates >= maxDates Then
'          maxDates = maxDates * 2
'          ReDim Preserve newDates(maxDates)
'        End If
'        newDates(nDates) = nextDate
'        nDates = nDates + 1
'      End If
'
'      'Increment pointer(s) to this date
'      For curTS = 0 To nts - 1
'        While nextDate > 0 And Abs(nextDate - Timbuf(nextIndex(curTS), curTS)) < JulianSecond
'          nextIndex(curTS) = nextIndex(curTS) + 1
'          If nextIndex(curTS) >= ts(curTS).NVal Then nextDate = 0
'        Wend
'      Next curTS
'      prevDate = nextDate
'    End If
'
'  Loop While nextDate > 0
'  ReDim Preserve newDates(nDates - 1) 'Shrink array down to exact size needed
'  Exit Sub
'NoOverlap:
'  MsgBox "The selected timeseries do not overlap in time, so they cannot be interpolated."
'  nDates = 0
'  ReDim newDates(0)
End Sub

'Copy simple variables, not arrays LocInfo and JDay
Private Sub copyfeqdata(src As Object, dst As Object, copylocinfo As Boolean, copyjday As Boolean)
'Private Sub copyfeqdata(src As ATTimSer, dst As ATTimSer, copylocinfo As Boolean, copyjday As Boolean)
  MsgBox "need to make copyfeqdata work with ATTimSer"
'Private Sub copyfeqdata(src As FeqData, dst As FeqData, copylocinfo As Boolean, copyjday As Boolean)
'  dst.EJDay = src.EJDay
'  dst.ItemPerRec = src.ItemPerRec
'  dst.LeftItemCnt = src.LeftItemCnt
'  dst.LocCount = src.LocCount
'  dst.NameFeo = src.NameFeo
'  dst.NameFtf = src.NameFtf
'  dst.NameTsd = src.NameTsd
'  dst.NumbFullRec = src.NumbFullRec
'  dst.RecLen = src.RecLen
'  dst.Scenario = src.Scenario
'  dst.SJDay = src.SJDay
'  dst.Term = src.Term
'  dst.TimCount = src.TimCount
'  dst.version = src.version
'  If copylocinfo Then
'    Dim Loc&
'    ReDim dst.LocInfo(src.LocCount - 1)
'    For Loc = 0 To src.LocCount - 1
'      dst.LocInfo(Loc) = src.LocInfo(Loc)
'    Next Loc
'  End If
'  If copyjday Then
'    Dim jd&
'    ReDim dst.JDay(src.TimCount - 1)
'    For jd = 0 To src.TimCount - 1
'      dst.JDay(jd) = src.JDay(jd)
'    Next jd
'  End If
End Sub
