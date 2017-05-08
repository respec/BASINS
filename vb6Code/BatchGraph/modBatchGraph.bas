Attribute VB_Name = "modBatchGraph"
Option Explicit
Global TserFiles As ATCPlugInManager
Const MxCurve = 20
Const MxData = 20
Const MxVar = 20
Const MxAxis = 4
Const MxDurBucket = 500
Global launch As AtCoLaunch

Declare Sub F90_FITLIN Lib "hass_ent.dll" (l&, l&, r!, r!, r!, r!)

Sub main()
  Dim o As ATCoGraph
  Dim f&
  Dim s$, sTyp$, sSav$
  Dim sMod As String * 80
  Dim t As ATCclsTserFile, tc As Collection, tv As Variant
  Dim d As ATCclsTserData, dc As Collection, dv As Variant, da As ATCclsTserData, dt As ATCclsTserData
  Dim lDate As ATCclsTserDate, lDateSummary As ATCData.ATTimSerDateSummary
  Dim lMsgUnit&
  Dim lNum&, lPos&, lRet&, lLen&, lVals() As Double, lDates() As Double
  Dim dvals() As Double, dpos() As Long
  Dim lCrv&, lCrvtyp&(MxCurve), lLinTyp&(MxCurve), lLinThk&(MxCurve), lSymTyp&(MxCurve), lColor&(MxCurve), lLbl$(MxCurve)
  Dim lTStep&(MxData), lTUnit&(MxData), lTAggr&(MxData), lSDate&(6), lEDate&(6), lDType&(MxData)
  Dim lVar&, lMin!(MxVar), lMax!(MxVar), lWhich&(MxVar), lTran&(MxVar), lLab$(MxVar)
  Dim lAxMin!(MxAxis), lAxMax!(MxAxis), lNTics&(MxAxis)
  Dim lXtype&, lYtype&, lYrtype&, lAuxlen!, lXlab$, lYlab$, lYrlab$, lAlab$
  Dim lGridX&, lGridY&, lGridYr&
  Dim lTitle$, lCapt$
  Dim lScenario$
  Dim b&, i&, j&
  Dim frm As Form
  Dim Wait As Boolean
  Dim L45 As Boolean, rdat!()
  Dim GraphType& '0:Time, 1:XY, 2:Duration
  Dim ShoOpt$
  Dim inputFileName$
  Dim lText$, lTextX!, lTextY!
  Dim hdle&, ExeName$
  
  Set launch = New AtCoLaunch
  launch.NconvertPath = "c:\vbexperimental\genscn\bin\nconvert.exe"
  
  initAllTser
  
  Set tc = Nothing
  Set tc = New Collection
  Set dc = Nothing
  Set dc = New Collection
  
  inputFileName = Command
  If Len(inputFileName) = 0 Then
    hdle = GetModuleHandle("BatchGraph")
    i = GetModuleFileName(hdle, sMod, 80)
    ExeName = UCase(Left(sMod, InStr(sMod, Chr(0)) - 1))
    If InStr(ExeName, "VB6.EXE") Then
      On Error GoTo Cancel
      frmDummy.cdl.CancelError = True
      frmDummy.cdl.ShowOpen
      On Error GoTo 0
      inputFileName = frmDummy.cdl.Filename
    Else
      inputFileName = "c:\vbexperimental\batchgraph\data\batchgraph.txt"
    End If
  End If
  
  f = 1
  Open inputFileName For Input As #f
    
  lCrv = 0
  lNum = 0
  lText = ""
  lScenario = ""
  
  While Not EOF(f)
    Line Input #f, s
    s = Trim(s)
    sTyp = Trim(UCase(StrRetRem(s)))
    Select Case sTyp
    Case "AGGR"
      lTUnit(lCrv) = StrRetRem(s)
      lTStep(lCrv) = StrRetRem(s)
      lTAggr(lCrv) = StrRetRem(s)
    Case "AXIS"
      lXtype = StrRetRem(s)
      lYtype = StrRetRem(s)
      lYrtype = StrRetRem(s)
      lAuxlen = StrRetRem(s)
      lXlab = StrRetRem(s)
      lYlab = StrRetRem(s)
      lYrlab = StrRetRem(s)
      lAlab = StrRetRem(s)
    Case "CLEAR"
      'always clear data
      Set dc = Nothing
      Set dc = New Collection
      If StrRetRem(s) = "FILE" Then
        MsgBox "CLEAR FILE is not working"
        'For Each tv In tc
        '  Set t = tv
        '  t.Clear
        '  tc(1).Delete
        'Next tv
      End If
      lCrv = 0
      lNum = 0
      lPos = 0
      lText = ""
    Case "CURVE"
      lCrvtyp(lCrv) = StrRetRem(s)
      lLinTyp(lCrv) = StrRetRem(s)
      lLinThk(lCrv) = StrRetRem(s)
      lSymTyp(lCrv) = StrRetRem(s)
      lColor(lCrv) = TextOrNumericColor(StrRetRem(s))
      lLbl(lCrv) = StrRetRem(s)
      lCrv = lCrv + 1
    Case "DATA"
      sSav = s
      Set d = getData(tc, s)
      If d Is Nothing Then
        MsgBox "Data not found with " & sSav
        End
      Else
        dc.Add d
      End If
    Case "FILE"
      Set t = New ATCclsTserFile
      Set t = openTser(StrRetRem(s), s, lMsgUnit)
      tc.Add t
      'MsgBox t.DataCount
    Case "GRID"
      lGridX = StrRetRem(s)
      lGridY = StrRetRem(s)
      lGridYr = StrRetRem(s)
    Case "INIT"
      o.Init
    Case "MSG"
      lMsgUnit = F90_WDBOPN(CLng(0), s, Len(s))
    Case "SCALE"
      For i = 0 To 3
        lAxMin(i) = StrRetRem(s)
        lAxMax(i) = StrRetRem(s)
        lNTics(i) = StrRetRem(s)
      Next i
    Case "SAVE"
      Set frm = o.GraphForm
      If GraphType <> 1 Then
        frm.WindowState = vbMaximized
      Else
        frm.Height = Screen.Height
        frm.Width = frm.Height
      End If
      launch.SavePictureAs frm.scrgraph, s
      Unload frm
    Case "SCENARIO"
      lScenario = StrRetRem(s)
    Case "SHOW"
      GraphType = 0
      Wait = False
      L45 = False
      Do While Len(s) > 0
        ShoOpt = Trim(UCase(StrRetRem(s)))
        Select Case ShoOpt
          Case "WAIT": Wait = True
          Case "XY": GraphType = 1
          Case "DUR": GraphType = 2
          Case "L45": L45 = True
        End Select
      Loop
      
      Set o = Nothing
      Set o = New ATCoGraph
      o.Init
      lCrv = 0
      lNum = 0
      lPos = 0
      ReDim dpos(dc.Count)
      dpos(0) = 0
      For Each dv In dc
        If Not (GraphType = 1 And lCrv > 1) Then 'only need the first 2 for XY ??
          Set da = dv
          Set dt = da.SubSetByDate(Date2J(lSDate), Date2J(lEDate))
          If dt.dates.Summary.CIntvl And dt.dates.Summary.NVALS > 2 Then
            lDType(lCrv) = 1
          Else
            lDType(lCrv) = 5
          End If
          If lTUnit(lCrv) = 0 Or Not (dt.dates.Summary.CIntvl) Then 'no aggr spec or not allowed
            lTUnit(lCrv) = dt.dates.Summary.Tu
            lTStep(lCrv) = dt.dates.Summary.ts
            Set d = dt
          Else
            Set lDate = Nothing
            Set lDate = New ATCclsTserDate
            With lDateSummary
              .Tu = lTUnit(lCrv)
              .ts = lTStep(lCrv)
              .SJDay = dt.dates.Summary.SJDay
              .EJDay = dt.dates.Summary.EJDay
              .CIntvl = True
              .Intvl = TimAddJ(.SJDay, .Tu, .ts, 1) - .SJDay
              timdif lSDate, lEDate, .Tu, .ts, i
              .NVALS = i
            End With
            lDate.Summary = lDateSummary
            Set d = dt.Aggregate(lDate, lTAggr(lCrv))
          End If
          ReDim lVals(d.dates.Summary.NVALS)
          ReDim lDates(d.dates.Summary.NVALS)
          lMin(lNum) = 1E+30
          lMax(lNum) = -1E+30
          For j = 1 To d.dates.Summary.NVALS
            lVals(j - 1) = d.Value(j)
            If lVals(j - 1) > lMax(lNum) Then
              lMax(lNum) = lVals(j - 1)
            End If
            If lVals(j - 1) < lMin(lNum) Then
              lMin(lNum) = lVals(j - 1)
            End If
            lDates(j - 1) = d.dates.Value(j)
          Next j
          If GraphType = 0 Then
            lMin(lNum + 1) = d.dates.Summary.SJDay
            lMin(lNum + 1) = d.dates.Summary.EJDay
          End If
          lLen = UBound(lVals)
          If L45 Then
            ReDim Preserve rdat(lLen * (lNum + 1))
            i = lNum * lLen
            For j = 0 To lLen
              rdat(j + i) = lVals(j)
            Next j
          End If
          If GraphType = 0 Then ' time graph - store date
            o.SetData lNum, lPos, lLen, lVals, lRet
            lPos = lPos + lLen
            lNum = lNum + 1
            o.SetData lNum, lPos, lLen, lDates, lRet
            lWhich(lNum) = 4 'dummy for time
            o.SetVars lCrv, lNum - 1, lNum
            lPos = lPos + lLen
            lNum = lNum + 1
            lCrv = lCrv + 1
          ElseIf GraphType = 1 Then 'xy
            o.SetData lNum, lPos, lLen, lVals, lRet
            If lNum = 0 Then 'x
              lWhich(lNum) = 4
              lTran(lNum) = lXtype
            ElseIf lNum = 1 Then 'y
              lWhich(lNum) = 1
              lTran(lNum) = lYtype
              o.SetVars lCrv, lNum, lNum - 1
              lCrv = lCrv + 1
            End If
            lPos = lPos + lLen
            lNum = lNum + 1
          ElseIf GraphType = 2 Then 'dur
            ReDim Preserve dvals(dpos(lCrv) + lLen)
            For i = 0 To lLen
              j = i + dpos(lCrv)
              dvals(j) = lVals(i)
            Next i
            lCrv = lCrv + 1
            lNum = lNum + 1
            dpos(lCrv) = dpos(lCrv - 1) + lLen
          End If
        End If
      Next dv
      
      If L45 Then
        CalcAndSaveRegression o, lNum, lPos, lCrv, lLen, _
                              lWhich, lCrvtyp, lSymTyp, lLinTyp, lLinThk, lColor, _
                              rdat, lMin, lMax, lXtype, lYtype, lTran, _
                              lText, lTextX, lTextY
      ElseIf GraphType = 2 Then 'dur
        CalcAndSaveDuration o, lNum, lPos, lCrv, lLen, _
                            lWhich, lCrvtyp, lSymTyp, lLinTyp, lLinThk, lColor, _
                            lMin, lMax, lXtype, lYtype, lTran, _
                            dpos, dvals
      End If
      o.SetNumVars lCrv, lNum
      If GraphType = 0 Then
        o.SetTime lTStep, lTUnit, lSDate, lEDate, lDType
      End If
      o.SetVarInfo lMin, lMax, lWhich, lTran, lLab
      o.SetScale lAxMin, lAxMax, lNTics
      o.SetAxesInfo lXtype, lYtype, lYrtype, lAuxlen, lXlab, lYlab, lYrlab, lAlab
      o.SetCurveInfo lCrvtyp, lLinTyp, lLinThk, lSymTyp, lColor, lLbl
      If Len(lScenario) = 0 Then
        lScenario = CStr(Date)
      End If
      o.SetTitles lTitle & "&" & lScenario, lCapt
      o.SetGrid lGridX, lGridY, lGridYr
      If Len(lText) > 0 Then
        o.SetAddText lTextX, lTextY, lText
      End If
      If Wait Then
        o.ShowIt True
      Else
        o.ShowIt
      End If
    Case "TEXT"
      lText = StrRetRem(s)
      lTextX = StrRetRem(s)
      lTextY = StrRetRem(s)
    Case "TIME"
      For i = 0 To 5
        lSDate(i) = StrRetRem(s)
      Next i
      For i = 0 To 5
        lEDate(i) = StrRetRem(s)
      Next i
    Case "TITLE"
      lTitle = StrRetRem(s)
      lCapt = s
    Case "VAR"
      lWhich(lNum) = StrRetRem(s)
      lTran(lNum) = StrRetRem(s)
      lNum = lNum + 2
    End Select
  Wend
Cancel:
  Unload frmDummy
  End
End Sub

Sub initAllTser()
  Set TserFiles = Nothing
  Set TserFiles = New ATCData.ATCPlugInManager
  TserFiles.Load "ATCTSfile"
End Sub

Function openTser(typ$, nam$, Msg&) As ATCclsTserFile
  Dim i&
  Dim o As Object ' ATCPlugIn
  Dim t As ATCclsTserFile
  
  i = TserFiles.AvailIndexByName("clsTSer" & typ)
  Call TserFiles.Create(i)
  Set o = TserFiles.CurrentActive.obj
  If typ = "WDM" Then
    o.msgUnit = Msg
  End If
  Set t = o
  t.Filename = nam
  Set openTser = t
End Function

Function getData(tc As Collection, s$) As ATCclsTserData
  Dim typ$, i&, vdata As Variant
  Dim fileIndex&
  Dim t As ATCclsTserFile
  Dim d As ATCclsTserData, dt As ATCclsTserData
  Dim argC As Collection
  Dim sen$, loc$, con$
  
  sen = "": loc = "": con = ""
  fileIndex = tc.Count
  While Len(s) > 0
    typ = UCase(StrRetRem(s))
    Select Case typ
    Case "FILE" 'use an earlier file
      fileIndex = StrRetRem(s)
    Case "IND"
      i = StrRetRem(s)
      Set t = tc(fileIndex)
      Set d = t.Data(i)
    Case "DSN"
      i = StrRetRem(s)
      Set t = tc(fileIndex)
      For Each vdata In t.DataCollection
        If vdata.Header.id = i Then
          Set d = vdata
          'MsgBox "found " & i
          Exit For
        End If
      Next vdata
    Case "MULT"
      Set argC = New Collection
      argC.Add d
      argC.Add StrRetRem(s)
      Set dt = MathColl("MULT", argC)
      Set dt.Attribs = d.Attribs
      Set dt.Header = d.Header.Copy
      dt.Header.desc = d.Header.desc & " (mult by " & s & ")"
      Set d = dt
      Set argC = Nothing
    Case "SEN"
      sen = StrRetRem(s)
      Set d = FindIfAllDefined(sen, loc, con, tc(fileIndex))
    Case "LOC"
      loc = StrRetRem(s)
      Set d = FindIfAllDefined(sen, loc, con, tc(fileIndex))
    Case "CON"
      con = StrRetRem(s)
      Set d = FindIfAllDefined(sen, loc, con, tc(fileIndex))
    End Select
  Wend
  Set getData = d
End Function

Private Function FindIfAllDefined(sen$, loc$, con$, t As ATCclsTserFile) As ATCclsTserData
  Dim vdata As Variant, h As ATTimSerDataHeader
  
  If Len(sen) > 0 And Len(loc) > 0 And Len(con) > 0 Then 'all defined
    For Each vdata In t.DataCollection
      Set h = vdata.Header
      If h.con = con And h.loc = loc And h.sen = sen Then
        Set FindIfAllDefined = vdata
        'MsgBox "found:" & sen & ":" & loc & ":" & con
        Exit For
      End If
    Next vdata
  End If
End Function

Sub CalcAndSaveRegression(o As ATCoGraph, lNum&, lPos&, lCrv&, lLen&, _
                          lWhich&(), lCrvtyp&(), lSymTyp&(), lLinTyp&(), lLinThk&(), lColor&(), _
                          rdat!(), lMin!(), lMax!(), lXtype&, lYtype&, lTran&(), _
                          lText$, lTextX!, lTextY!)
  Dim cMxFit&
  Dim ACoef!, BCoef!, RSquar!, rXtmp#(), rYtmp#(), rtmp#(2), lRet&, i&, j&
  Dim rDtmp!()
  
  lCrvtyp(lCrv) = 6
  lSymTyp(lCrv) = 0
  lLinTyp(lCrv) = 1
  lLinThk(lCrv) = 1
  lColor(lCrv) = 0
  rtmp(0) = 0
  rtmp(1) = 100000000#
  'x
  o.SetData lNum, lPos, 2, rtmp, lRet
  lWhich(lNum) = 4
  lTran(lNum) = lXtype
  lNum = lNum + 1
  lPos = lPos + 2
  'y
  o.SetData lNum, lPos, 2, rtmp, lRet
  lWhich(lNum) = 1
  lTran(lNum) = lYtype
  o.SetVars lCrv, lNum, lNum - 1
  lNum = lNum + 1
  lPos = lPos + 2
  lCrv = lCrv + 1
  
  'regression calc
  'needs y first
  j = UBound(rdat)
  ReDim Preserve rDtmp(j)
  j = j / 2
  For i = 0 To j - 1
    rDtmp(i) = rdat(i + j)
    rDtmp(i + j) = rdat(i)
  Next i
  Call F90_FITLIN(lLen, 2 * lLen, rDtmp(0), ACoef, BCoef, RSquar)
  'compute correlation coef from coef of determination
  If RSquar < 0 Then 'special case
    rtmp(0) = -1 * (Sqr(-1 * RSquar))
  Else
    rtmp(0) = Sqr(RSquar)
  End If
  If ACoef < 0 Then 'neg slope, correlation coef also negative
    rtmp(0) = -1 * rtmp(0)
  End If
  lText = "Y = " & NumFmted(ACoef, 8, 3) & " X "
  If BCoef > 0.001 Then
    lText = lText & "+ " & NumFmted(BCoef, 8, 3)
  ElseIf BCoef < -0.001 Then 'have minus sign
    lText = lText & NumFmted(BCoef, 8, 3)
  End If
  lText = lText & "&Corr Coef = " & NumFmted(CSng(rtmp(0)), 8, 3)
  lTextX = 0.05
  lTextY = 0.9
  
  'regression line
  lCrvtyp(lCrv) = 6
  lSymTyp(lCrv) = 0
  lLinTyp(lCrv) = 1
  lLinThk(lCrv) = 1
  lColor(lCrv) = lColor(0)
  cMxFit = 50
  ReDim rXtmp(cMxFit)
  ReDim rYtmp(cMxFit)
  
  For i = 0 To cMxFit - 1
   rXtmp(i) = lMin(0) + i * (lMax(0) - lMin(0)) / cMxFit
   rYtmp(i) = (rXtmp(i) * ACoef) + BCoef
  Next i
  'X
  'debug min and max
  'lText = lText & "&X" & NumFmted(rXtmp(0), 8, 3) & "-" & NumFmted(rXtmp(cMxFit - 1), 8, 3)
  o.SetData lNum, lPos, cMxFit, rXtmp, lRet
  lWhich(lNum) = 4
  lTran(lNum) = lXtype
  lNum = lNum + 1
  lPos = lPos + cMxFit
  'debug min and max
  'lText = lText & "&Y" & NumFmted(rYtmp(0), 8, 3) & "-" & NumFmted(rYtmp(cMxFit - 1), 8, 3)
  o.SetData lNum, lPos, cMxFit, rYtmp, lRet
  lWhich(lNum) = 1
  lTran(lNum) = lYtype
  o.SetVars lCrv, lNum, lNum - 1
  lNum = lNum + 1
  lPos = lPos + cMxFit
  lCrv = lCrv + 1
End Sub

Sub CalcAndSaveDuration(o As ATCoGraph, lNum&, lPos&, lCrv&, lLen&, _
                        lWhich&(), lCrvtyp&(), lSymTyp&(), lLinTyp&(), lLinThk&(), lColor&(), _
                        lMin!(), lMax!(), lXtype&, lYtype&, lTran&(), _
                        dpos&(), dvals#())
                        
  Dim j&, i&, n&, l&, nci&, c#, lRet&, v#
  Dim dMin!, dMax!, clog!, dExp!, bound!(1), cr!, clas!(MxDurBucket), rdat#(MxDurBucket), cdat#(MxDurBucket)
  Dim cnuma&(), cpcta#(), numa&(), suma#(), tnum&
  
  lXtype = 6
  lYtype = 2
  
  dMin = 1E+30
  dMax = -1E+30
  For j = 0 To lCrv - 1
    If dMin > lMin(j) Then dMin = lMin(j)
    If dMax < lMax(j) Then dMax = lMax(j)
  Next j
  dExp = Fix(Log10(dMax))
  bound(1) = 10# ^ (dExp + 1)
  If dMin <= 0# Then dMin = dMax / 1000#
  dExp = Int(Log10(dMin))
  bound(0) = 10# ^ (dExp)
  'set up class intervals
  cr = (bound(0) / bound(1)) ^ (1# / (MxDurBucket + 1))
  clas(0) = 0
  clas(1) = bound(0)
  clas(MxDurBucket) = bound(1)
  
  For j = 1 To MxDurBucket - 2
    i = MxDurBucket - j
    clas(i) = clas(i + 1) * cr
  Next j
  'round off class intervals
  For i = 1 To MxDurBucket
    c = clas(i)
    clog = Log10(c) + 0.001
    If clog < 0# Then clog = clog - 1
    l = Fix(clog)
    l = l - 1
    c = (c / (10# ^ l)) + 0.5
    clas(i) = (Fix(c)) * (10# ^ l)
  Next i
  nci = MxDurBucket + 1
  
  ReDim cnuma(lCrv)
  ReDim cpcta(lCrv, MxDurBucket)
  ReDim numa(lCrv, MxDurBucket)
  ReDim suma(lCrv, MxDurBucket)
  
  For i = 0 To nci - 2
    cdat(i) = clas(i + 1)
  Next i

  lNum = 0
  For j = 0 To lCrv - 1
    o.SetData lNum, lPos, nci - 1, cdat, lRet 'Y
    lWhich(lNum) = 1
    lTran(lNum) = 2
    lPos = lPos + nci - 1
    lNum = lNum + 1
    
    For n = dpos(j) To dpos(j + 1) - 1
      i = nci
      v = dvals(n)
      Do
        i = i - 1
      Loop While v < clas(i) And i > 0
      numa(j, i) = numa(j, i) + 1
      suma(j, i) = suma(j, i) + v
    Next n
    tnum = 0
    For i = 0 To nci - 1
      tnum = tnum + numa(j, i)
    Next i
    cnuma(j) = tnum
    For i = 0 To nci - 1
      If numa(j, i) > 0 Then
        suma(j, i) = suma(j, i) / CDbl(numa(j, i))
      End If
      If i > 0 Then cnuma(j) = cnuma(j) - numa(j, i - 1)
      cpcta(j, i) = 100# * CDbl(cnuma(j)) / CDbl(tnum)
    Next i
    For i = 0 To nci - 2 'set x data
      rdat(i) = -gausex(0.01 * cpcta(j, i + 1))
    Next i
    lWhich(lNum) = 4 'force to x
    o.SetData lNum, lPos, nci - 1, rdat, lRet
    o.SetVars j, lNum - 1, lNum
    lNum = lNum + 1
    lPos = lPos + nci - 1
  Next j
  
End Sub

Public Function gausex(exprob!) As Single

    'GAUSSIAN PROBABILITY FUNCTIONS   W.KIRBY  JUNE 71
       'GAUSEX=VALUE EXCEEDED WITH PROB EXPROB
       'GAUSAB=VALUE (NOT EXCEEDED) WITH PROBCUMPROB
       'GAUSCF=CUMULATIVE PROBABILITY FUNCTION
       'GAUSDY=DENSITY FUNCTION
    'SUBPGMS USED -- NONE

    'GAUSCF MODIFIED 740906 WK -- REPLACED ERF FCN REF BY RATIONAL APPRX N
    'ALSO REMOVED DOUBLE PRECISION FROM GAUSEX AND GAUSAB.
    '76-05-04 WK -- TRAP UNDERFLOWS IN EXP IN GUASCF AND DY.

    'rev 8/96 by PRH for VB
    
    Const c0! = 2.515517
    Const c1! = 0.802853
    Const c2! = 0.010328
    Const d1! = 1.432788
    Const d2! = 0.189269
    Const d3! = 0.001308
    Dim pr!, rtmp!, rctmp!, rDtmp!, p!, t!
    
    p = exprob
    If p >= 1# Then
      'set to minimum
      rtmp = -10#
    ElseIf p <= 0# Then
      'set at maximum
      rtmp = 10#
    Else
      'compute value
      pr = p
      If p > 0.5 Then pr = 1# - pr
      t = (-2# * Log(pr)) ^ 0.5
      rctmp = c0 + t * (c1 + t * c2)
      rDtmp = (1# + t * (d1 + t * (d2 + t * d3)))
      rtmp = t - rctmp / rDtmp
      If p > 0.5 Then rtmp = -rtmp
    End If
    gausex = rtmp

End Function

