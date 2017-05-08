Attribute VB_Name = "GraphList"
Option Explicit
'Copyright 2000 by AQUA TERRA Consultants

Public ncrv&, nvar&
Dim XAxTyp&, YAxTyp&, YRAxTyp&, auxlen!
Dim XZLineFg&, YZLineFg&
Dim xlab$, ylab$, yrlab$, alab$
Dim tmin!, tmax!
Dim Ctitl$, capt$, clab$(), VLab$()
Dim Cic As Object
Dim pmin!(1 To 4), pmax!(1 To 4), NTic&(1 To 4)
Dim vmin!(), vmax!(), whch&(), Tran&()
Dim ctyp&(), ltyp&(), lthk&(), styp&(), clr&()
Dim ltu&(), lTs&(), ldtr&(), dtype&()
Dim lsdate&(5), ledate&(5), lConInt As Boolean
Dim XLegLoc!, YLegLoc!
Dim L45 As Boolean
Dim ACoeff!, BCoeff!, RSquare!
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
    Dim pr!, rtmp!, rctmp!, rdtmp!, p!, t!
    
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
      rdtmp = (1# + t * (d1 + t * (d2 + t * d3)))
      rtmp = t - rctmp / rdtmp
      If p > 0.5 Then rtmp = -rtmp
    End If
    gausex = rtmp

End Function

'Call after GLInit for each curve
'InitMatchingColors must be called first or this will assign random colors
Public Sub GLSetColor(crv&, ColorRule As String)
  On Error Resume Next 'ignore bad attempts to set colors of curves that don't exist
  clr(crv) = GetMatchingColor(ColorRule)
End Sub

Public Sub GLAxLab(cxlab$, cylab$, Cyrlab$, Calab$)

    xlab = cxlab
    ylab = cylab
    yrlab = Cyrlab
    alab = Calab

End Sub
Public Sub GLAxTics(nxtic&, nytic&, nyrtic&, natic&)

    NTic(4) = nxtic
    NTic(1) = nytic
    NTic(2) = nyrtic
    NTic(3) = natic

End Sub
Public Sub GLDoFD(o As Object, opt%, fddat As Collection)

    Dim i&, j&, l&, n&, ipos&, indx&, iexp&
    Dim nci&, ncim1&, retcod&, tnum&, v#
    Dim lMin#, lMax#, c#
    Dim bound!(1), cr!, clas!(200), clog!, rdat#(200)
    Dim cnuma&(), numa&()
    Dim suma#(), cpcta#()

    Call o.Init
    Call o.SetCurDrvDir("", CurDir)
    
    nvar = ncrv + 1
    Call o.SetNumVars(ncrv, nvar)
    'determine bounds for duration analysis
    lMin = 1000000#
    lMax = -1000000#
    For j = 0 To ncrv - 1
      vmin(j) = fddat(j + 1).Min
      vmax(j) = fddat(j + 1).Max
      If vmin(j) < lMin Then lMin = vmin(j)
      If vmax(j) > lMax Then lMax = vmax(j)
    Next j
    iexp = Fix(Log10(lMax))
    bound(1) = 10# ^ (iexp + 1)
    If lMin <= 0# Then lMin = lMax / 1000#
    iexp = Int(Log10(lMin))
    bound(0) = 10# ^ (iexp)
    'set up class intervals
    cr = (bound(0) / bound(1)) ^ (1# / 201#)
    clas(0) = 0#
    clas(1) = bound(0)
    clas(200) = bound(1)
    For j = 1 To 198
      i = 200 - j
      clas(i) = clas(i + 1) * cr
    Next j
    'round off class intervals
    For i = 1 To 200
      c = clas(i)
      clog = Log10(c) + 0.001
      If clog < 0# Then clog = clog - 1
      l = Fix(clog)
      l = l - 1
      c = (c / (10# ^ l)) + 0.5
      clas(i) = (Fix(c)) * (10# ^ l)
    Next i
    nci = 201

    ReDim cnuma(ncrv)
    ReDim cpcta(ncrv, 200)
    ReDim numa(ncrv, 200)
    ReDim suma(ncrv, 200)

    'define axes, x is probability percent
    XAxTyp = 6
    YAxTyp = 2 'set y to log

    ncim1 = nci - 1
    For i = 0 To ncim1 - 1 'set Y data
      rdat(i) = clas(i + 1)
    Next i
    'put the y data in the plot buffer
    ipos = 1
    indx = 0
    Call o.SetData(indx, ipos, ncim1, rdat(), retcod)
    'set up y axis scaling for class interval data
    vmin(0) = rdat(0)
    vmax(0) = rdat(ncim1 - 1)
    Tran(0) = YAxTyp
    whch(0) = 1

    'fill class intervals for each curve
    For j = 0 To ncrv - 1
      For n = 1 To fddat(j + 1).dates.Summary.NVALS
        i = nci
        v = fddat(j + 1).value(n)
        Do
          i = i - 1
        Loop While v < clas(i) And i > 0
        numa(j, i) = numa(j, i) + 1
        suma(j, i) = suma(j, i) + fddat(j + 1).value(n)
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

      For i = 0 To ncim1 - 1 'set X data
        rdat(i) = -gausex(0.01 * cpcta(j, i + 1))
      Next i
      indx = indx + 1
      ipos = ipos + ncim1
      'put the x data in the plot buffer
      Call o.SetData(indx, ipos, ncim1, rdat(), retcod)
      'save axis info for this variable
      whch(indx) = 4
      vmin(indx) = rdat(ncim1 - 1)
      vmax(indx) = rdat(0)
      Tran(indx) = 1
      'save where the data is for this curve
      Call o.SetVars(j, 0, indx)
      ctyp(j) = 6 'xy type curve
    Next j
    
    'make sure vmin not so small that it ruins plot scaling
    If vmin(0) < -0.1 * vmax(0) Then
      'change gvmin for a more meaningful plot
      vmin(0) = -0.1 * vmax(0)
    End If
    'scale y axis
'    Call scalit((YAxTyp), vmin(0), vmax(0), pmin(1), pmax(1))
    pmin(1) = vmin(0)
    pmax(1) = vmax(0)
    If pmax(1) - pmin(1) < 0.000000000000001 Then
      pmax(1) = pmax(1) + 0.000000000000001
    End If
    'x axis
    pmin(4) = -3#
    pmax(4) = 3#
    If opt = 1 Then
      Call o.SetVarInfo(vmin, vmax, whch, Tran, VLab)
      Call o.SetScale(pmin, pmax, NTic)
      Call o.SetAxesInfo(XAxTyp, YAxTyp, YRAxTyp, auxlen, xlab, ylab, yrlab, alab)
      Call o.SetCurveInfo(ctyp, ltyp, lthk, styp, clr, clab)
      Call o.SetTitles(Ctitl, capt)
      Call o.SetLegLoc(XLegLoc, YLegLoc)
    ElseIf opt = 2 Then
      For j = 0 To ncrv - 1
        Call o.SetFldPrms(j, 8, 4, 1)
      Next j
      Call o.SetSums(CLng(1), ldtr, ltu, CLng(9))
      For j = 0 To nvar - 1
        Call o.SetLabel(j, VLab(j))
      Next j
      Call o.SetTitles(Ctitl, "GenScn List")
    End If
    Call o.ShowIt
    p.ExternalForms.Add o.GraphForm

End Sub

'Public Sub GLDoTS(o As Object, opt%, PLTser() As Timser, Timbuf() As Double)
Public Sub GLDoTS(o As Object, opt%, PLTser As Collection)

    Dim i&, j&, pos&, indx&, naux&, s$
    Dim TBuf() As Double
    Dim sparseData As Boolean, lMin#, lMinX#, lMinCnt&, lcnt&, v# 'for sparse data check
    Dim lPLTser As ATCclsTserData

    indx = 0
    For j = 1 To ncrv
      Set lPLTser = PLTser(j)
      If CDTran = 4 Then
        If lPLTser.dates.Summary.CIntvl Then
          ldtr(j - 1) = lPLTser.Dtran
          ltu(j - 1) = lPLTser.dates.Summary.Tu
          lTs(j - 1) = lPLTser.dates.Summary.ts
        Else
          ldtr(j - 1) = 1
          ltu(j - 1) = 1
          lTs(j - 1) = 1
        End If
      Else
        ldtr(j - 1) = CDTran
        ltu(j - 1) = ctunit
        lTs(j - 1) = CTStep
      End If
      vmin(indx) = lPLTser.Min
      vmax(indx) = lPLTser.Max
      indx = indx + 1
      If Not lConInt Then 'set variable info for time values
        vmin(indx) = lPLTser.dates.Summary.SJDay
        vmax(indx) = lPLTser.dates.Summary.EJDay
        whch(indx) = 4 'time value on x axis
        indx = indx + 1
      End If
    Next j

    Call o.Init
    Call o.SetCurDrvDir("", CurDir)

    pmin(1) = 1E+30
    pmax(1) = -1E+30
    indx = 0
    pos = 1
    For j = 1 To ncrv
      Set lPLTser = PLTser(j)
      lMin = 1E+30
      lMinX = lMin
      lcnt = 0
      sparseData = False
      ReDim TBuf(lPLTser.dates.Summary.NVALS)
      For i = 1 To lPLTser.dates.Summary.NVALS
        v = lPLTser.value(i)
        TBuf(i - 1) = v ' -1 added to move from zero index to one index
        If v < lMin Then
          lMinX = lMin 'prev low value
          lMin = v
          lMinCnt = 1
        ElseIf v - lMin < 0.0001 Then
          lMinCnt = lMinCnt + 1
        ElseIf v < lMinX Then
          lMinX = v 'min which is not missing data
        End If
        lcnt = lcnt + 1
      Next i
      s = UCase(Left(lPLTser.Header.con, 4))
      If s <> "PREC" And s <> "SNOW" And InStr(capt, "Bar") = 0 And CLng(lcnt) <> 0 Then 'skip precip(and related), always looks sparse
        If CLng(lMinCnt) / CLng(lcnt) > 0.75 And lMinCnt <> lcnt Then 'sparse data
          ltyp(j - 1) = 0 ' no line
          styp(j - 1) = 3 ' asterisk symbol
          dtype(j - 1) = 2 ' point type
          sparseData = True
        ElseIf Not lPLTser.dates.Summary.CIntvl Then  ' not const interval - show points
          'should this be done for swmm and feq???
          ltyp(j - 1) = 0
          styp(j - 1) = 3
          dtype(j - 1) = 2
          sparseData = True
        End If
      End If
      Call o.SetData(indx, pos, lPLTser.dates.Summary.NVALS, TBuf, i)
      pos = pos + lPLTser.dates.Summary.NVALS
      If pmax(1) < lPLTser.Max Then
        pmax(1) = lPLTser.Max
      End If
      If pmin(1) > lPLTser.Min Then
        If sparseData Then
          pmin(1) = lMinX
        ElseIf lPLTser.Min < -10000000000# Then
          pmin(1) = 0
        Else
          pmin(1) = lPLTser.Min
        End If
      End If
      indx = indx + 1
      If Not lConInt Then 'And Not lPLTser.dates.Summary.CIntvl Then 'not constant interval, need timeseries
        If lPLTser.dates.Summary.CIntvl Then
          'ctyp(j - 1) = 1
          dtype(j - 1) = 1 'jlk 8/14
        Else
          dtype(j - 1) = 2
        End If
        ctyp(j - 1) = 5 'non constant
        For i = 1 To lPLTser.dates.Summary.NVALS
          TBuf(i - 1) = lPLTser.dates.value(i)
        Next i
        Call o.SetData(indx, pos, lPLTser.dates.Summary.NVALS, TBuf, i)
        Call o.SetVars(j - 1, indx - 1, indx)
        indx = indx + 1
      Else
        Call o.SetVars(j, j, ncrv)
      End If
      pos = pos + lPLTser.dates.Summary.NVALS
    Next j
    If lConInt Then 'constant interval, same # vars as curves
      Call o.SetNumVars(ncrv, ncrv)
    Else
      Call o.SetNumVars(ncrv, 2 * ncrv)
    End If
    Call o.SetTime(lTs(), ltu(), lsdate(), ledate(), dtype())
    tmin = pmin(1)
    tmax = pmax(1)
    Call Scalit((YAxTyp), tmin, tmax, pmin(1), pmax(1))
    If Abs(pmin(1)) < 0.001 Then pmin(1) = 0
    If IsNumeric(Cic) Then
      Call o.SetIcon(Cic)
    End If
    If opt = 1 Then 'plot
      pmin(3) = 1E+30
      pmax(3) = -1E+30
      indx = 0
      If ncrv > 1 Then 'see if any precip curves to put on aux axis
        naux = 0
        For j = 1 To ncrv
          Set lPLTser = PLTser(j)
          If InStr(lPLTser.Header.con, "PREC") > 0 Then 'put precip on aux axis
            naux = naux + 1
            whch(indx) = 3
            If vmin(indx) < pmin(3) Then pmin(3) = vmin(indx)
            If vmax(indx) > pmax(3) Then pmax(3) = vmax(indx)
          End If
          indx = indx + 1
          If Not lConInt Then indx = indx + 1
        Next j
        If naux > 0 And naux < ncrv Then
          'some, but not all precip, put on aux axis
          auxlen = 0.2
          tmin = pmin(3)
          tmax = pmax(3)
          Call Scalit(1, tmin, tmax, pmin(3), pmax(3))
          'only use 5 tics on l/r y axes
          NTic(1) = 5
          NTic(2) = 5
        ElseIf naux = ncrv Then
          'all curves are precip, reset to go on left y
          indx = 0
          For j = 0 To ncrv - 1
            whch(indx) = 1
            indx = indx + 1
            If Not lConInt Then indx = indx + 1
          Next j
        End If
      End If
      Call o.SetVarInfo(vmin, vmax, whch, Tran, VLab)
      Call o.SetScale(pmin, pmax, NTic)
      Call o.SetAxesInfo(XAxTyp, YAxTyp, YRAxTyp, auxlen, xlab, ylab, yrlab, alab)
      Call o.SetCurveInfo(ctyp, ltyp, lthk, styp, clr, clab)
      Call o.SetTitles(Ctitl, capt)
      If XZLineFg = 1 Or YZLineFg = 1 Then
        'draw zero line(s) on plot
        Call o.SetConstLines(YZLineFg, 0, XZLineFg, 0)
      End If
      Call o.SetLegLoc(XLegLoc, YLegLoc)
    ElseIf opt = 2 Then 'list
      'the value of ltype is set when setdate is called
      'If lConInt Then
      '  Call o.SetListType(1)
      'Else
      '  Call o.SetListType(2)
      'End If
      For j = 0 To ncrv - 1
        'If pmax(1) < 10 Then 'use 3 decimal plages
          Call o.SetFldPrms(j, 10, 5, 3)
        'elseIf pmax(1) < 100 Then 'use 2 decimal places
        '  Call o.SetFldPrms(j, 8, 4, 2)
        'Else 'use one decimal place
        '  Call o.SetFldPrms(j, 8, 4, 1)
        'End If
        Call o.SetFldRange(j, -1E+30, 1E+30, True)
        Call o.SetEditable(j, False)
      Next j
      'work on default year ends on month ?
      If ltu(0) < 6 Then 'jlk 9/99 - year ends at end of month - 9 not good for annual data which starts on 1/1
        Call o.SetSums(CLng(1), ldtr, ltu, 9, 0)
      Else
        Call o.SetSums(CLng(1), ldtr, ltu, 12, 0)
      End If
      For j = 0 To nvar - 1
        Call o.SetVarLabel(j, VLab(j))
      Next j
      Call o.SetTitles(Ctitl, capt)
    End If
    Call o.ShowIt
    On Error Resume Next 'If it is a list, never mind adding it
    p.ExternalForms.Add o.GraphForm
End Sub

' o is graph or list
Public Sub GLDoXY(o As Object, ByVal opt&, XYData() As xyplotdata, initfg&)

    Dim i&, j&, pos&, indx&, rtmp#(1), lstr$
    Dim vbuf#()

    YAxTyp = XYData(0).Var(0).Trans
    XAxTyp = XYData(0).Var(1).Trans
    
    If initfg <> 1 Then ' even if the caller didn't ask for it, we may want to initialize graph
      Call o.GetScale(pmin, pmax, NTic)
      If pmin(4) = pmax(4) Then initfg = 1
      If pmin(1) = pmax(1) Then initfg = 1
    End If
    
    If initfg = 1 Then
      Call o.Init
      Call o.SetCurDrvDir("", CurDir)
    End If
    pmin(1) = 1E+30
    pmax(1) = -1E+30
    NTic(4) = 5 'only use 5 tics on x axis
    pmin(4) = 1E+30
    pmax(4) = -1E+30
    indx = 0
    pos = 1
    For j = 0 To ncrv - 1
      'set the y data
      ReDim vbuf(XYData(j).NVal)
      For i = 0 To XYData(j).NVal - 1
        vbuf(i) = XYData(j).Var(0).Vals(i)
      Next i
      Call o.SetData(indx, pos, XYData(j).NVal, vbuf(), i)
      If XYData(j).DataLabels.nLables > 0 Then
        Call o.SetDataLabels(0, pos, XYData(j).NVal, XYData(j).DataLabels.Labels, i)
      End If
      pos = pos + XYData(j).NVal
      
      vmin(indx) = XYData(j).Var(0).Min
      vmax(indx) = XYData(j).Var(0).Max
      whch(indx) = 1
      Tran(indx) = XYData(j).Var(0).Trans
      If pmax(1) < vmax(indx) Then
        pmax(1) = vmax(indx)
      End If
      If pmin(1) > vmin(indx) Then
        pmin(1) = vmin(indx)
      End If
      
      'set the x data
      indx = indx + 1
      For i = 0 To XYData(j).NVal - 1
        vbuf(i) = XYData(j).Var(1).Vals(i)
      Next i
      Call o.SetData(indx, pos, XYData(j).NVal, vbuf, i)
      
      vmin(indx) = XYData(j).Var(1).Min
      vmax(indx) = XYData(j).Var(1).Max
      whch(indx) = 4
      Tran(indx) = XYData(j).Var(1).Trans
      If pmax(4) < vmax(indx) Then
        pmax(4) = vmax(indx)
      End If
      If pmin(4) > vmin(indx) Then
        pmin(4) = vmin(indx)
      End If
      Call o.SetVars(j, indx - 1, indx)
      
      indx = indx + 1
      pos = pos + XYData(j).NVal
      ctyp(j) = 6 'xy type curve
      styp(j) = j + 1 'set symbol type
      ltyp(j) = 0 'no line as default
    Next j
    If L45 Then '45 degree/regression lines being drawn
      ReDim Preserve ctyp(ncrv + 1)
      ReDim Preserve styp(ncrv + 1)
      ReDim Preserve ltyp(ncrv + 1)
      ReDim Preserve lthk(ncrv + 1)
      ReDim Preserve clr(ncrv + 1)
      ReDim Preserve clab(ncrv + 1)
      ctyp(ncrv) = 6
      ctyp(ncrv + 1) = 6
      styp(ncrv) = 0
      styp(ncrv + 1) = 0
      ltyp(ncrv) = 1
      ltyp(ncrv + 1) = 1
      lthk(ncrv) = 1
      lthk(ncrv + 1) = 1
      clr(ncrv) = 0
      clr(ncrv + 1) = clr(0)
      'set y and x max to be the same
      If pmax(1) > pmax(4) Then
        pmax(4) = pmax(1)
      Else
        pmax(1) = pmax(4)
      End If
    End If
    'set Y-axis scale
    tmin = pmin(1)
    tmax = pmax(1)
    Call Scalit((YAxTyp), tmin, tmax, pmin(1), pmax(1))
    If Abs(pmin(1)) < 0.001 Then pmin(1) = 0
    'set X-axis scale
    tmin = pmin(4)
    tmax = pmax(4)
    Call Scalit((YAxTyp), tmin, tmax, pmin(4), pmax(4))
    If pmin(4) = pmax(4) Then
      If tmin <= tmax Then
        pmin(4) = pmin(4) - 1: pmax(4) = pmax(4) + 1
      Else
        pmin(4) = pmin(4) + 1: pmax(4) = pmax(4) - 1
      End If
    End If
    If L45 Then
      'set data points for the 45 degree line
      ReDim Preserve vmin(nvar + 4)
      ReDim Preserve vmax(nvar + 4)
      ReDim Preserve whch(nvar + 4)
      ReDim Preserve Tran(nvar + 4)
      ReDim Preserve VLab(nvar + 4)
      '45 degree line, y points first
      rtmp(0) = 0.00001   'avoid log problem
      rtmp(1) = pmax(1)
      Call o.SetData(indx, pos, 2, rtmp, i)
      whch(indx) = 1
      Tran(indx) = YAxTyp
      'now x points
      rtmp(0) = 0.00001   'avoid log problem
      rtmp(1) = pmax(4)
      indx = indx + 1
      pos = pos + 2
      Call o.SetData(indx, pos, 2, rtmp, i)
      whch(indx) = 4
      Tran(indx) = XAxTyp
      Call o.SetVars(ncrv, indx - 1, indx)
      ncrv = ncrv + 1
      nvar = nvar + 2
      'regression line, y points first
      rtmp(0) = (vmin(1) * ACoeff) + BCoeff
      rtmp(1) = (vmax(1) * ACoeff) + BCoeff
      indx = indx + 1
      pos = pos + 2
      Call o.SetData(indx, pos, 2, rtmp, i)
      whch(indx) = 1
      Tran(indx) = YAxTyp
      'now x points
      rtmp(0) = vmin(1)
      rtmp(1) = vmax(1)
      indx = indx + 1
      pos = pos + 2
      Call o.SetData(indx, pos, 2, rtmp, i)
      whch(indx) = 4
      Tran(indx) = XAxTyp
      Call o.SetVars(ncrv, indx - 1, indx)
      ncrv = ncrv + 1
      nvar = nvar + 2
    End If
    Call o.SetNumVars(ncrv, nvar)
    If opt = 1 Then
      Call o.SetVarInfo(vmin, vmax, whch, Tran, VLab)
      If initfg = 1 Then
        Call o.SetScale(pmin, pmax, NTic)
        Call o.SetAxesInfo(XAxTyp, YAxTyp, YRAxTyp, auxlen, xlab, ylab, yrlab, alab)
        Call o.SetCurveInfo(ctyp, ltyp, lthk, styp, clr, clab)
        Call o.SetLegLoc(XLegLoc, YLegLoc)
      End If
      If L45 Then 'add regression line text
        If RSquare < 0 Then
          'compute correlation coef from coef of
          'determination, special case if negative
          rtmp(0) = -1 * (Sqr(-1 * RSquare))
        Else
          rtmp(0) = Sqr(RSquare)
        End If
        If ACoeff < 0 Then
          'neg slope, correlation coef also negative
          rtmp(0) = -1 * rtmp(0)
        End If
        'lstr = "Y = " & NumFmted(ACoeff, 8, 3) & " X + " & NumFmted(BCoeff, 8, 3) & "&Corr Coef = " & NumFmted(CSng(rtmp(0)), 8, 3)
        lstr = "Y = " & NumFmted(ACoeff, 8, 3) & " X "
        If BCoeff > 0 Then lstr = lstr & "+ "
        lstr = lstr & NumFmted(BCoeff, 8, 3) & "&Corr Coef = " & NumFmted(CSng(rtmp(0)), 8, 3)
        Call o.SetAddText(0.05, 0.9, lstr)
      End If
      Call o.SetTitles(Ctitl, capt)
      If IsNumeric(Cic) Then 'update plot form's icon
        Call o.SetIcon(Cic)
      End If
      If XZLineFg = 1 Or YZLineFg = 1 Then
        'draw zero line(s) on plot
        Call o.SetConstLines(YZLineFg, 0, XZLineFg, 0)
      End If
    ElseIf opt = 2 Then
      For j = 0 To ncrv - 1
        Call o.SetFldPrms(j, 8, 4, 1)
      Next j
      Call o.SetSums(CLng(1), ldtr, ltu, CLng(9))
      For j = 0 To ncrv - 1
        Call o.SetLabel(j, clab(j))
      Next j
      Call o.SetTitles(Ctitl, "GenScn List")
    End If
    If initfg = 1 Then 'new graph
      Call o.ShowIt
      p.ExternalForms.Add o.GraphForm
    ElseIf opt = 1 Then 'update existing graph
      Call o.ReDrawGraph(0)
    End If

End Sub

Public Sub GLDoBox(o As Object, opt%, XYData() As xyplotdata, initfg&)

    Dim i&, j&, pos&, indx&, rtmp#(1), lstr$
    Dim vbuf#()

    YAxTyp = 0
    XAxTyp = -1

    If initfg = 1 Then
      Call o.Init
      Call o.SetCurDrvDir("", CurDir)
    End If
    pmin(1) = 0
    pmax(1) = 1
    pmin(4) = 1E+30
    pmax(4) = -1E+30
    indx = 0
    pos = 1
    For j = 0 To ncrv - 1
      'set the x data
      ReDim vbuf(XYData(j).NVal)
      For i = 0 To XYData(j).NVal - 1
        vbuf(i) = XYData(j).Var(0).Vals(i)
      Next i
      Call o.SetData(indx, pos, XYData(j).NVal, vbuf(), i)
      pos = pos + XYData(j).NVal
      vmin(0) = XYData(j).Var(0).Min
      vmax(0) = XYData(j).Var(0).Max
      whch(0) = 4
      Tran(0) = XYData(j).Var(0).Trans
      If pmax(4) < vmax(0) Then
        pmax(4) = vmax(0)
      End If
      If pmin(4) > vmin(0) Then
        pmin(4) = vmin(0)
      End If
      Call o.SetVars(j, indx - 1, indx)
      indx = indx + 1
      pos = pos + XYData(j).NVal
      ctyp(j) = 10 'box plot type curve
      styp(j) = j + 1 'set symbol type
      ltyp(j) = 0 'no line as default
    Next j
    'set y-axis scale
    tmin = pmin(4)
    tmax = pmax(4)
    Call Scalit((YAxTyp), tmin, tmax, pmin(4), pmax(4))
    Call o.SetNumVars(ncrv, nvar)
    If opt = 1 Then
      Call o.SetVarInfo(vmin, vmax, whch, Tran, VLab)
      If initfg = 1 Then
        Call o.SetScale(pmin, pmax, NTic)
      End If
      Call o.SetAxesInfo(XAxTyp, YAxTyp, YRAxTyp, auxlen, xlab, ylab, yrlab, alab)
      Call o.SetCurveInfo(ctyp, ltyp, lthk, styp, clr, clab)
      Call o.SetLegLoc(XLegLoc, YLegLoc)
      Call o.SetTitles(Ctitl, capt)
      If IsNumeric(Cic) Then
        Call o.SetIcon(Cic)
      End If
      If XZLineFg = 1 Or YZLineFg = 1 Then
        'draw zero line(s) on plot
        Call o.SetConstLines(YZLineFg, 0, XZLineFg, 0)
      End If
    ElseIf opt = 2 Then
      For j = 0 To ncrv - 1
        Call o.SetFldPrms(j, 8, 4, 1)
      Next j
      Call o.SetSums(CLng(1), ldtr, ltu, CLng(9))
      For j = 0 To ncrv - 1
        Call o.SetLabel(j, clab(j))
      Next j
      Call o.SetTitles(Ctitl, "GenScn List")
    End If
    If initfg = 1 Then 'new graph
      Call o.ShowIt
      p.ExternalForms.Add o.GraphForm
    ElseIf opt = 1 Then 'update existing graph
      Call o.ReDrawGraph(0)
    End If

End Sub
Public Sub GLInit(opt&, o As Object, incrv&, invar&)
    Dim j%
    'Static g As New HGrph
    'Static l As New HList

    ncrv = incrv
    nvar = invar
    If ncrv > 18 Then 'max from hgraph
      If nvar >= 2 * ncrv Then '2 vars/curve
        nvar = nvar - (2 * (ncrv - 18))
      Else
        nvar = nvar - (ncrv - 18)
      End If
      ncrv = 18
    End If
    If opt = 1 Then
      Set o = New ATCoGraph
      'Set IPC so the graph can send debug messages to status monitor
      On Error Resume Next
      Set o.IPC = IPC 'This started causing a type mismatch error, not sure why
    Else
      Set o = New ATCoList
    End If
    o.HelpFileName = App.HelpFile

    ReDim ltu(ncrv)
    ReDim lTs(ncrv)
    ReDim ldtr(ncrv)
    ReDim dtype(ncrv)
    ReDim vmin(nvar)
    ReDim vmax(nvar)
    ReDim whch(nvar)
    ReDim Tran(nvar)
    ReDim VLab(nvar)
    ReDim clab(ncrv)
    ReDim ctyp(ncrv)
    ReDim ltyp(ncrv)
    ReDim lthk(ncrv)
    ReDim styp(ncrv)
    ReDim clr(ncrv)
    For j = 0 To ncrv - 1
      dtype(j) = 1   'default to mean values
      ctyp(j) = 1    'regular time plot
      ltyp(j) = 1    'solid line
      lthk(j) = 1    'thinnest line
      styp(j) = 0    'no symbols
      clr(j) = (j + 9) Mod 15 'vary the colors
    Next j
    For j = 0 To nvar - 1
      whch(j) = 1
      Tran(j) = 1
    Next j
    For j = 1 To 4
      NTic(j) = 10
    Next j
    'reset aux axis tics
    NTic(3) = 2
    XAxTyp = 0
    YAxTyp = 1
    YRAxTyp = 0
    auxlen = 0#
    XZLineFg = 0
    YZLineFg = 0
    XLegLoc = 0.02 'Just inside left Y axis
    YLegLoc = 1
    L45 = False

End Sub
Public Sub GLDate(IStDate&(), IEnDate&(), ConstInt As Boolean)

    Dim i%

    For i = 0 To 5
      lsdate(i) = IStDate(i)
      ledate(i) = IEnDate(i)
    Next i
    lConInt = ConstInt

End Sub

Public Sub GLRegLines(L45Flg&, ACoef!, BCoef!, RSquar!)

    L45 = L45Flg
    ACoeff = ACoef
    BCoeff = BCoef
    RSquare = RSquar

End Sub

Public Sub GLSetEditable(o As Object, tsIndex&)
  Call o.SetEditable(tsIndex, True)
End Sub

Public Sub GLTitl(Title$, Caption$)

    Ctitl = Title
    capt = Caption

End Sub
Public Sub GLIcon(ic As Object)

    Set Cic = ic

End Sub

Public Sub GLLegend(leglab() As String)

    Dim i%

    ReDim clab(ncrv)
    For i = 0 To ncrv - 1
      clab(i) = leglab(i)
    Next i

End Sub
Public Sub GLLegLoc(XLPos!, YLPos!)

    XLegLoc = XLPos
    YLegLoc = YLPos

End Sub
Public Sub GLVarLab(ivlab() As String)

    Dim i%

    ReDim VLab(nvar - 1)
    For i = 0 To nvar - 1
      VLab(i) = ivlab(i)
    Next i

End Sub
Public Sub GLZLine(YFg&, XFg&)

    YZLineFg = YFg
    XZLineFg = XFg

End Sub
Public Sub GLLSpec(icrv&, lt&, lw&, ls&, lc&)

    If lt >= 0 Then
      ltyp(icrv) = lt   'line type
    End If
    If lw >= 0 Then
      lthk(icrv) = lw   'line width
    End If
    If ls >= 0 Then
      styp(icrv) = ls   'symbols
    End If
    If lc >= 0 Then
      clr(icrv) = lc    'colors
    End If

End Sub
