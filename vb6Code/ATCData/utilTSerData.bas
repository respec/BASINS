Attribute VB_Name = "utilTSerData"
Option Explicit
'Copyright 2002 by AQUA TERRA Consultants

'args: mixed collection of ATCclsTserData and constant numbers in the appropriate order
'
'oper: Name of operation to perform (not case sensitive)
' if args(n) is a constant, ignore .Value(i) in the chart below
' oper, args.Count,  Definition
' "Add":     n    out(i) = args(1).Value(i) + args(2).Value(i) + ...
' "Mult":    n    out(i) = args(1).Value(i) * args(2).Value(i) * ...
' "Mean":    n    out(i) = Add(i) / args.count
' "Min":     n    out(i) = Min(args(1).Value(i), args(2).Value(i), ...)
' "Max":     n    out(i) = Max(args(1).Value(i), args(2).Value(i), ...)
' "Sub":     2    out(i) = args(1).Value(i) - args(2).Value(i)
' "Div":     2    out(i) = args(1).Value(i) / args(2).Value(i)
' "Exponent":2    out(i) = args(1).Value(i) ^ args(2).Value(i)
' "Line":    3    out(i) = args(1).Value(i) * args(2).Value(i) + args(3).Value(i)
' "Abs"      1    out(i) = Abs(args(1).Value(i))
' "C2F":     1    out(i) = args(1).Value(i) * 9/5 + 32
' "F2C":     1    out(i) =(args(1).Value(i) - 32) * 5/9
' "Log 10"   1    out(i) = log(args(1).Value(i))
' "Log e"    1    out(i) = ln(args(1).Value(i))
' "e**"      1    out(i) = e ^ args(1).Value(i)
' "Running Sum":1 out(i) = args(1).Value(1) + args(1).Value(2) + ... + args(1).Value(i)
' "Weight":  2n   out(i) = args(1).Value(i) * args(2).Value(i)
'                        + args(3).Value(i) * args(2).Value(i) + ...
'                        + args(args.count-1).Value(i) * args(args.count).Value(i)
' "Interpolate" Uses values from first ATCclsTserData arg
'               If no other args are ATCclsTserData, uses dates from first ATCclsTserData
'               If other args are ATCclsTserData, uses merged dates from all but first ATCclsTserData
'               If any args are constant, those values are treated as missing and interpolated over
Public Function MathColl(ByVal oper As String, args As Collection) As ATCclsTserData
  Dim argNum As Long
  Dim valNum As Long
  Dim argConst() As Boolean
  Dim argVal() As Double
  Dim curArgVal As Double, weightVal As Double
  Dim dataval() As Single
  Dim NVALS As Long
  Dim Nargs As Long
  Dim boo As Boolean
  Dim comdates As Collection
  Dim argVals As ATCclsTserData
  Dim arg As ATCclsTserData
  Dim nBefore As Long
  Dim nAfter As Long
  Dim retval As ATCclsTserData
  Set retval = New ATCclsTserData
  Set MathColl = retval
  Set comdates = New Collection
  
  ReDim argConst(1 To args.Count)
  ReDim argVal(1 To args.Count)
  
  Nargs = args.Count
  oper = LCase(oper)
  
  If Left(oper, 7) = "running" And Nargs > 2 Then
    oper = Mid(oper, 9)
    Nargs = Nargs - 2
    If IsNumeric(args(Nargs + 1)) Then
      nBefore = args(Nargs + 1)
    ElseIf Len(Trim(args(Nargs + 1))) > 0 Then
      nBefore = -1 'All
    End If
    If IsNumeric(args(Nargs + 2)) Then
      nAfter = args(Nargs + 2)
    ElseIf Len(Trim(args(Nargs + 2))) > 0 Then
      MsgBox "Including all values after is not currently supported - defaulting to " & nAfter & " values after.", vbOKOnly, "MathColl"
    End If
    Set retval = Nothing
    Set retval = MathRunning(args(1), nBefore, nAfter, oper)
  Else
  
    For argNum = 1 To Nargs 'Categorize args as constant or timeseries
      If IsNumeric(args(argNum)) Then
        argConst(argNum) = True
        argVal(argNum) = args(argNum)
      Else
        Set arg = args(argNum)
        If NVALS = 0 Then
          Set argVals = arg
          Set retval.dates = arg.dates
          NVALS = arg.dates.Summary.NVALS
        ElseIf oper = "interpolate" Then
          comdates.Add arg.dates
        ElseIf arg.dates.Summary.NVALS <> NVALS Then
          MsgBox "Number of values in time series must be the same in MathColl." & "Nvals = " & NVALS & " and Nvals = " & args(argNum).dates.Summary.NVALS
          Exit Function
        End If
      End If
    Next
  
    ReDim dataval(NVALS)
    argNum = 1
    Select Case oper
      Case "add", "+", "mean"
        If oper = "mean" Then boo = True Else boo = False
        For valNum = 1 To NVALS
          dataval(valNum) = 0
          For argNum = 1 To Nargs
            GoSub SetCurArgVal
            dataval(valNum) = dataval(valNum) + curArgVal
          Next
          If boo Then dataval(valNum) = dataval(valNum) / Nargs
        Next
      Case "geometric mean"
        For valNum = 1 To NVALS
          dataval(valNum) = 0
          For argNum = 1 To Nargs
            GoSub SetCurArgVal
            dataval(valNum) = dataval(valNum) + Log10(curArgVal)
          Next
          dataval(valNum) = 10 ^ (dataval(valNum) / Nargs)
        Next
      Case "mult", "*"
        For valNum = 1 To NVALS
          dataval(valNum) = 1
          For argNum = 1 To Nargs
            GoSub SetCurArgVal
            dataval(valNum) = dataval(valNum) * curArgVal
          Next
        Next
      Case "min"
        For valNum = 1 To NVALS
          argNum = 1
          GoSub SetCurArgVal
          dataval(valNum) = curArgVal
          For argNum = 2 To Nargs
            GoSub SetCurArgVal
            If curArgVal < dataval(valNum) Then dataval(valNum) = curArgVal
          Next
        Next
      Case "max"
        For valNum = 1 To NVALS
          argNum = 1
          GoSub SetCurArgVal
          dataval(valNum) = curArgVal
          For argNum = 2 To Nargs
            GoSub SetCurArgVal
            If curArgVal > dataval(valNum) Then dataval(valNum) = curArgVal
          Next
        Next
      Case "sub", "-"
        For valNum = 1 To NVALS
          argNum = 1
          GoSub SetCurArgVal
          dataval(valNum) = curArgVal
          argNum = 2
          GoSub SetCurArgVal
          dataval(valNum) = dataval(valNum) - curArgVal
        Next
      Case "div", "/"
        For valNum = 1 To NVALS
          argNum = 1
          GoSub SetCurArgVal
          dataval(valNum) = curArgVal
          argNum = 2
          GoSub SetCurArgVal
          dataval(valNum) = dataval(valNum) / curArgVal
        Next
      Case "exponent", "exp", "^", "**"
        For valNum = 1 To NVALS
          argNum = 1
          GoSub SetCurArgVal
          dataval(valNum) = curArgVal
          argNum = 2
          GoSub SetCurArgVal
          dataval(valNum) = dataval(valNum) ^ curArgVal
        Next
      Case "line"
        For valNum = 1 To NVALS
          argNum = 1
          GoSub SetCurArgVal
          dataval(valNum) = curArgVal
          argNum = 2
          GoSub SetCurArgVal
          dataval(valNum) = dataval(valNum) * curArgVal
          argNum = 3
          GoSub SetCurArgVal
          dataval(valNum) = dataval(valNum) + curArgVal
        Next
      Case "abs"
        For valNum = 1 To NVALS
          GoSub SetCurArgVal
          dataval(valNum) = Abs(curArgVal)
        Next
      Case "c2f", "ctof", "celsiustofahrenheit", "celsius2fahrenheit"
        For valNum = 1 To NVALS
          GoSub SetCurArgVal
          dataval(valNum) = curArgVal * 9 / 5 + 32
        Next
      Case "f2c", "ftoc", "fahrenheittocelsius", "fahrenheit2celsius"
        For valNum = 1 To NVALS
          GoSub SetCurArgVal
          dataval(valNum) = (curArgVal - 32) * 5 / 9
        Next
      Case "log 10"
        For valNum = 1 To NVALS
          GoSub SetCurArgVal
          dataval(valNum) = Log10(curArgVal)
        Next
      Case "log e"
        For valNum = 1 To NVALS
          GoSub SetCurArgVal
          dataval(valNum) = Log(curArgVal)
        Next
      Case "e**"
        For valNum = 1 To NVALS
          GoSub SetCurArgVal
          dataval(valNum) = Exp(curArgVal)
        Next
'      Case "running sum"
'        valNum = 1
'        GoSub SetCurArgVal
'        dataval(valNum) = curArgVal
'        For valNum = 2 To NVALS
'          GoSub SetCurArgVal
'          dataval(valNum) = dataval(valNum - 1) + curArgVal
'        Next
      Case "weight"
        For valNum = 1 To NVALS
          dataval(valNum) = 0
          argNum = 1
          While argNum < Nargs
            GoSub SetCurArgVal
            weightVal = curArgVal
            argNum = argNum + 1
            GoSub SetCurArgVal
            dataval(valNum) = dataval(valNum) + curArgVal * weightVal
            argNum = argNum + 1
          Wend
        Next
      Case "interpolate"
        If comdates.Count = 1 Then
          retval.dates = comdates(1)
        ElseIf comdates.Count > 1 Then
          retval.dates = comdates(1).GetCommonDates(comdates)
        End If
        If NVALS <> retval.dates.Summary.NVALS Then
          NVALS = retval.dates.Summary.NVALS
          ReDim dataval(NVALS)
        End If
  
        Dim dateOld1 As Double, dateOld2 As Double, dateNew As Double
        Dim oldIndex&, newIndex&, numOldVals&
        Dim valOld1 As Single, valOld2 As Single
        Dim item As Variant
        For Each item In argVals.Attribs
          retval.AttribSet item.Name, item.value
        Next
        retval.flags = argVals.flags
        Set retval.Header = argVals.Header.Copy
        numOldVals = argVals.dates.Summary.NVALS
        oldIndex = 1
        valOld2 = argVals.value(oldIndex)
        GoSub SkipMissingVals
        valOld1 = valOld2
        dateOld2 = argVals.dates.value(oldIndex)
        dateOld1 = dateOld2
        ReDim newVals(NVALS)
        newIndex = 1
        While newIndex <= NVALS
          dateNew = retval.dates.value(newIndex)
          While dateNew > dateOld2 And oldIndex < numOldVals
            'Debug.Print newIndex, oldIndex
            oldIndex = oldIndex + 1
            dateOld1 = dateOld2
            dateOld2 = argVals.dates.value(oldIndex)
            valOld1 = valOld2
            valOld2 = argVals.value(oldIndex)
            GoSub SkipMissingVals
          Wend
          If dateNew > dateOld2 Or oldIndex = 1 Or dateOld1 = dateOld2 Then
            'First or last old value is used for new dates outside the range of old dates
            dataval(newIndex) = valOld2
          Else
            dataval(newIndex) = valOld1 + (valOld2 - valOld1) * (dateNew - dateOld1) / (dateOld2 - dateOld1)
          End If
          newIndex = newIndex + 1
        Wend
        retval.Header.sen = "Interp:" & retval.Header.sen
      Case Else
        MsgBox "Unknown MathColl operation: " & oper
    End Select
    
    retval.Values = dataval
    retval.calcSummary
  End If
  Set MathColl = retval
  Exit Function
SetCurArgVal:
  If argConst(argNum) Then
    curArgVal = argVal(argNum)
  Else
    curArgVal = args(argNum).value(valNum)
  End If
  Return

SkipMissingVals:
CheckNextMissingVal:
  For argNum = 1 To Nargs
    If argConst(argNum) Then
      If valOld2 = argVal(argNum) Then
        oldIndex = oldIndex + 1
        dateOld2 = argVals.dates.value(oldIndex)
        valOld2 = argVals.value(oldIndex)
        If oldIndex < numOldVals Then GoTo CheckNextMissingVal
      End If
    End If
  Next
  Return
End Function

'If computing geometric mean, values <= 0 are skipped (treated as ones)
Private Function MathRunning(ByVal ts As ATCclsTserData, _
                                   nBefore As Long, _
                                   nAfter As Long, _
                                   oper As String) As ATCclsTserData
  Dim nValsPerMean As Long
  Dim NVALS As Long
  Dim NnewVALS As Long
  Dim NbadVals As Long 'Values <= 0 in current window make geometric mean undefined
  Dim AllBefore As Boolean
  Dim oldValNum As Long
  Dim newValNum As Long
  Dim fromOldVal As Long
  Dim tmpOldVal As Double
  Dim tmpNewVal As Double
  Dim newval() As Single
  Dim newDate() As Double
  Dim operation As Long
  Dim retval As ATCclsTserData
  Set retval = New ATCclsTserData
  Set retval.dates = New ATCclsTserDate
  
  Select Case LCase(oper)
    Case "geometric mean":          operation = 1
    Case "arithmetic mean", "mean": operation = 2
    Case "sum", "add":              operation = 3
    Case "min":                     operation = 4
    Case "max":                     operation = 5
    Case Else
      MsgBox "Unknown operation '" & oper & "' in MathRunning"
      Set MathRunning = retval
      Exit Function
  End Select
  
  nValsPerMean = 1 'Always include the value at each time step
  If nBefore > 0 Then nValsPerMean = nValsPerMean + nBefore
  If nAfter > 0 Then nValsPerMean = nValsPerMean + nAfter
  
  If nBefore < 0 Then AllBefore = True: nBefore = 0
  
  NVALS = ts.dates.Summary.NVALS
  NnewVALS = NVALS - nValsPerMean + 1
  ReDim newval(NnewVALS)
  ReDim newDate(NnewVALS)
  Select Case operation
    Case 1, 2, 3
      newValNum = 1
      tmpNewVal = 0
      For oldValNum = 1 To NVALS
        tmpOldVal = ts.value(oldValNum)
        Select Case operation
          Case 1
            If tmpOldVal > 0 Then
              tmpNewVal = tmpNewVal + Log(tmpOldVal)
            Else
              NbadVals = NbadVals + 1
            End If
          Case 2, 3: tmpNewVal = tmpNewVal + tmpOldVal
        End Select
        If oldValNum >= nValsPerMean Then
          newDate(newValNum) = ts.dates.value(oldValNum - nAfter)
          Select Case operation
            Case 1
              If NbadVals = 0 Then
                newval(newValNum) = Exp(tmpNewVal / nValsPerMean)
              Else
                newval(newValNum) = -999
              End If
            Case 2: newval(newValNum) = tmpNewVal / nValsPerMean
            Case 3: newval(newValNum) = tmpNewVal
          End Select
          If AllBefore Then
            nValsPerMean = nValsPerMean + 1
          Else 'Subtract value that has fallen behind the running window
            tmpOldVal = ts.value(oldValNum - nValsPerMean + 1)
            Select Case operation
              Case 1
                If tmpOldVal > 0 Then
                  tmpNewVal = tmpNewVal - Log(tmpOldVal)
                Else
                  NbadVals = NbadVals - 1
                End If
              Case 2, 3: tmpNewVal = tmpNewVal - tmpOldVal
            End Select
          End If
          newValNum = newValNum + 1
        End If
      Next
    Case 4, 5 'Min, Max
      If operation = 4 Then tmpNewVal = 1E+30 Else tmpNewVal = -1E+30
      For newValNum = 1 To NnewVALS
        If AllBefore Then
          fromOldVal = 1
          nValsPerMean = newValNum + nAfter
          newDate(newValNum) = ts.dates.value(newValNum)
        Else
          newDate(newValNum) = ts.dates.value(newValNum + nBefore)
          fromOldVal = newValNum
        End If
        newval(newValNum) = tmpNewVal
        For oldValNum = fromOldVal To fromOldVal + nValsPerMean
          tmpOldVal = ts.value(oldValNum)
          If operation = 4 Then
            If tmpOldVal < newval(newValNum) Then newval(newValNum) = tmpOldVal
          Else
            If tmpOldVal > newval(newValNum) Then newval(newValNum) = tmpOldVal
          End If
        Next
      Next
  End Select
  retval.dates.Values = newDate
  retval.dates.calcSummary
  retval.Values = newval
  retval.calcSummary
  Set MathRunning = retval
End Function


'Creates a new constant-interval ATCclsTserData with all values set to NewValue
Public Function NewTser(sjdate As Double, _
                        ejdate As Double, _
                        TimeStep As Long, _
                        TimeUnits As Long, _
                        newvalue As Single) As ATCclsTserData
  Dim index As Long
  Dim newval() As Single
  Dim dsS As ATTimSerDateSummary
  Dim dsOut As ATCclsTserDate
  Dim tsOut As ATCclsTserData
  
  Set tsOut = New ATCclsTserData
  Set dsOut = New ATCclsTserDate
  Set tsOut.dates = dsOut
  dsS.SJDay = sjdate
  dsS.EJDay = ejdate
  dsS.CIntvl = True
  dsS.ts = TimeStep
  dsS.Tu = TimeUnits
  dsS.NVALS = timdifJ(sjdate, ejdate, dsS.Tu, dsS.ts)
  dsS.Intvl = (ejdate - sjdate) / dsS.NVALS
  dsOut.Summary = dsS
  ReDim newval(dsS.NVALS)
  For index = 1 To dsS.NVALS
    newval(index) = newvalue
  Next
  tsOut.Values = newval
  Set NewTser = tsOut
End Function

'Public Function Statistic(ByVal oper As String, args As Collection, _
'                          Optional LowerLimit As Single = -1E+30, _
'                          Optional UpperLimit As Single = 1E+30) As Single
'  Dim argNum As Long
'  Dim valNum As Long
'  Dim argConst() As Boolean
'  Dim argVal() As Single
'  Dim curArgVal As Double
'  Dim simulated As Double
'  Dim meanerror As Double
'  Dim tmpVal As Double
'  Dim retval As Double
'  Dim mean1 As Double
'  Dim mean2 As Double
'  Dim NVALS As Long, nUsedVals As Long
'  Dim Nargs As Long
'  Dim recurArgs As Collection
'  Dim argVals As ATCclsTserData
'  Dim arg As ATCclsTserData
'
'  ReDim argConst(1 To args.count)
'  ReDim argVal(1 To args.count)
'
'  Nargs = args.count
'  oper = LCase(oper)
'
'  For argNum = 1 To Nargs 'Categorize args as constant or timeseries
'    If IsNumeric(args(argNum)) Then
'      argConst(argNum) = True
'      argVal(argNum) = args(argNum)
'    Else
'      Set arg = args(argNum)
'      If NVALS = 0 Then
'        Set argVals = arg
'        NVALS = arg.dates.Summary.NVALS
'      ElseIf arg.dates.Summary.NVALS <> NVALS Then
'        MsgBox "Number of values in time series must be the same in Statistic." & "Nvals = " & NVALS & " and Nvals = " & args(argNum).dates.Summary.NVALS
'        Exit Function
'      End If
'    End If
'  Next
'
'  retval = 0
'
'  Select Case LCase(oper)
'    Case "mean": retval = Statistic("add", args, LowerLimit, UpperLimit) / (Nargs * NVALS)
'    Case "geometric mean"
'      For valNum = 1 To NVALS
'        tmpVal = 0
'        For argNum = 1 To Nargs
'          curArgVal = args(argNum).value(valNum)
'          If curArgVal >= LowerLimit And curArgVal < UpperLimit Then
'            retval = retval + Log(curArgVal)
'            nUsedVals = nUsedVals + 1
'          End If
'        Next
'      Next
'      If nUsedVals > 0 Then retval = Exp(retval / nUsedVals)
'    Case "add", "sum"
'      For valNum = 1 To NVALS
'        For argNum = 1 To Nargs
'          curArgVal = args(argNum).value(valNum)
'          If curArgVal >= LowerLimit And curArgVal < UpperLimit Then
'            retval = retval + curArgVal
'          End If
'        Next
'      Next
'    Case "mult"
'      retval = 1
'      For valNum = 1 To NVALS
'        For argNum = 1 To Nargs
'          curArgVal = args(argNum).value(valNum)
'          If curArgVal >= LowerLimit And curArgVal < UpperLimit Then
'            retval = retval * curArgVal
'          End If
'        Next
'      Next
'    Case "min"
'      retval = 1E+30
'      For valNum = 1 To NVALS
'        For argNum = 1 To Nargs
'          curArgVal = args(argNum).value(valNum)
'          If curArgVal >= LowerLimit And curArgVal < UpperLimit Then
'            If curArgVal < retval Then retval = curArgVal
'          End If
'        Next
'      Next
'    Case "max"
'      retval = -1E+30
'      For valNum = 1 To NVALS
'        For argNum = 1 To Nargs
'          curArgVal = args(argNum).value(valNum)
'          If curArgVal >= LowerLimit And curArgVal < UpperLimit Then
'            If curArgVal > retval Then retval = curArgVal
'          End If
'        Next
'      Next
'    Case "count1"
'      For valNum = 1 To NVALS
'        curArgVal = args(2).value(valNum)
'        If curArgVal >= LowerLimit And curArgVal < UpperLimit Then
'          retval = retval + 1
'        End If
'      Next
'    Case "count2"
'      For valNum = 1 To NVALS
'        simulated = args(1).value(valNum)
'        If simulated >= LowerLimit And simulated < UpperLimit Then
'          retval = retval + 1
'        End If
'      Next
'    Case "correlation coefficient", "coefficient of determination"
'      mean1 = args(1).AttribNumeric("mean")
'      mean2 = args(2).AttribNumeric("mean")
'      For valNum = 1 To NVALS
'        simulated = args(1).value(valNum)
'        curArgVal = args(2).value(valNum)
'        If curArgVal >= LowerLimit And curArgVal < UpperLimit Then
'          retval = retval + (simulated - mean1) * (curArgVal - mean2)
'          nUsedVals = nUsedVals + 1
'        End If
'      Next
'      If nUsedVals > 1 Then retval = retval / (nUsedVals - 1)
'      retval = retval / args(1).AttribNumeric("STDDEVIATION")
'      retval = retval / args(2).AttribNumeric("STDDEVIATION")
'
'      If LCase(oper) = "coefficient of determination" Then retval = retval ^ 2
'
'    Case "mean error"
'      For valNum = 1 To NVALS
'        simulated = args(1).value(valNum)
'        curArgVal = args(2).value(valNum)
'        If curArgVal >= LowerLimit And curArgVal < UpperLimit Then
'          retval = retval + (simulated - curArgVal)
'          nUsedVals = nUsedVals + 1
'        End If
'      Next
'      If nUsedVals > 0 Then retval = retval / nUsedVals
'    Case "mean absolute error"
'      For valNum = 1 To NVALS
'        simulated = args(1).value(valNum)
'        curArgVal = args(2).value(valNum)
'        If curArgVal >= LowerLimit And curArgVal < UpperLimit Then
'          retval = retval + Abs(simulated - curArgVal) '/ NVALS
'          nUsedVals = nUsedVals + 1
'        End If
'      Next
'      If nUsedVals > 0 Then retval = retval / nUsedVals
'    Case "rms error"
'      For valNum = 1 To NVALS
'        simulated = args(1).value(valNum)
'        curArgVal = args(2).value(valNum)
'        If curArgVal >= LowerLimit And curArgVal < UpperLimit Then
'          retval = retval + (simulated - curArgVal) ^ 2 '/ NVALS
'          nUsedVals = nUsedVals + 1
'        End If
'      Next
'      If nUsedVals > 0 Then retval = retval / nUsedVals
'      If retval > 0 Then retval = Sqr(retval)
''    Case "std deviation of error"
''      meanerror = Statistic("mean error", args)
''      For valNum = 1 To NVALS
''        argNum = 1
''        curArgVal = args(argNum).value(valNum)
''        simulated = curArgVal
''        argNum = 2
''        curArgVal = args(argNum).value(valNum)
''        retval = Sqr((simulated - curArgVal - meanerror) ^ 2 / '(NVALS - 1))
''      Next
''      If retval > 0 Then retval = Sqr(retval)
'    Case "nash sutcliffe", "model fit efficiency"
''      Set recurArgs = New Collection
''      recurArgs.Add args(2)
'      mean2 = args(2).AttribNumeric("mean") ' Statistic("mean", recurArgs)
'      tmpVal = 0
''      Set recurArgs = Nothing
'      For valNum = 1 To NVALS
'        simulated = args(1).value(valNum)
'        curArgVal = args(2).value(valNum)
'        If curArgVal >= LowerLimit And curArgVal < UpperLimit Then
'          retval = retval + (simulated - curArgVal) ^ 2
'          tmpVal = tmpVal + (curArgVal - mean2) ^ 2
'        End If
'      Next
'      If tmpVal > 0 Then retval = retval / tmpVal
'      If LCase(oper) = "model fit efficiency" Then retval = 1 - retval
'    Case Else
'      MsgBox "Unknown Statistic operation: " & oper
'  End Select
'  Statistic = retval
'End Function

Public Function uniqueAttributeNames(Optional TSC As Collection) As Collection
  Dim lTs As Variant, lAt As Variant
  Dim retval As Collection, lArg As Boolean
  
  Set retval = New Collection
  
  If Not TSC Is Nothing Then
    If TSC.Count > 0 Then
      For Each lAt In TSC(1).AttribNames
        retval.Add lAt, lAt
      Next
      Set retval = TSC(1).AttribNames
      On Error Resume Next
      For Each lTs In TSC
        For Each lAt In lTs.Attribs
          retval.Add lAt.Name, lAt.Name
        Next
      Next
    End If
  End If
  
  Set uniqueAttributeNames = retval
End Function

Public Function uniqueAttributeValues(AttrName As String, ts As Collection) As CollString ', Optional cnow As CollString
  Dim vTs As Variant, lTs As ATCclsTserData
  Dim retval As CollString
  Dim s$
  
  Set retval = New CollString

  On Error Resume Next
  
  For Each vTs In ts
    Set lTs = vTs
    s = lTs.Attrib(AttrName)
    retval.Add s, s
  Next
  
  Set uniqueAttributeValues = retval
  
End Function

Public Function TserFileProperties(tsf As ATCclsTserFile) As String
  Dim retval As String
  On Error GoTo errHand
  retval = tsf.label & " File: " & tsf.Filename & vbCrLf
  If Len(tsf.Description) > 0 Then retval = retval & tsf.Description & vbCrLf
  retval = retval & tsf.DataCount & " datasets" & vbCrLf
  If tsf.Filename = "<in memory>" Then
    'don't look for file
'  ElseIf Len(Dir(tsf.Filename, vbDirectory)) > 0 Then
'    Dim nFiles As Long
'    nFiles = -1 'make up for . and .. entries
'    While Len(Dir) > 0
'      nFiles = nFiles + 1
'    Wend
'    retval = retval & "Directory contains " & nFiles & " files"
  ElseIf Len(Dir(tsf.Filename)) = 0 Then
    retval = retval & "File not found on disk" & vbCrLf
  Else
    retval = retval & "File Size: " & Format(FileLen(tsf.Filename), "###,###,###,###") & " bytes" & vbCrLf
    retval = retval & "Modified:  " & FileDateTime(tsf.Filename) & vbCrLf
  End If
  TserFileProperties = Left(retval, Len(retval) - 2) 'trim last vbCrLf
  Exit Function

errHand:
  TserFileProperties = retval

End Function

Public Function TSerAsText(t As ATCclsTserData, Optional lev As Long = 2, Optional delim As String = vbCrLf) As String
  Dim s As String, i As Long
  Dim vAttrib As Variant, lAttrib As ATTimSerAttribute
  
  With t
    With .Header
      s = "ID:" & .ID
      s = s & delim & "Scen:" & .sen
      s = s & delim & "Locn:" & .loc
      s = s & delim & "Cons:" & .con
      s = s & delim & "Desc:" & .desc
    End With
    s = s & delim & "File:" & .File.Filename
    s = s & delim & "Type:" & .File.Description
    If lev > 0 Then 'attributes
      For Each vAttrib In .Attribs
        lAttrib = vAttrib
        With lAttrib
          s = s & delim & "Attrib:" & .Name & ":" & .value
        End With
      Next
      If lev > 1 Then 'date summary
        With .dates.Summary
          s = s & delim & "Ts:" & .ts
          s = s & delim & "Tu:" & .Tu
          s = s & delim & "Intvl:" & .Intvl
          s = s & delim & "Nvals:" & .NVALS
          s = s & delim & "CIntvl:" & .CIntvl
          s = s & delim & "SJDay:" & DumpDate(.SJDay)
          s = s & delim & "EJDay:" & DumpDate(.EJDay)
        End With
        If lev > 2 Then 'data
          s = s & delim & "Data Values and Dates:"
          s = s & delim & "<undef> " & DumpDate(.dates.value(i), "J")
          For i = 1 To .dates.Summary.NVALS
            s = s & delim & .value(i) & " " & DumpDate(.dates.value(i), "J")
          Next i
        End If
      End If
    End If
  End With
  TSerAsText = s & delim
End Function
