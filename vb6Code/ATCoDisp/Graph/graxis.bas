Attribute VB_Name = "GrAxis"
Option Explicit
'Copyright 2001 by AQUA TERRA Consultants

Private Const Grid_Color = 13158600 'rgb(200, 200, 200)

Public Function RNDBST(r!)

    'Given a number, R, find one of the best intervals to contain
    'it.  The returned number will not be less than R and will
    'be as close to it as possible using a factor of 1, 2, 5, or
    '10 and some integer power of 10.  Used to create labels
    'for tics on a graph that have nice values.

    Dim n&
    Dim POW!, WHOLE!, FRAC!, tp!, MULT!

    If r = 0# Then
      RNDBST = 0#
    Else
      'FIND POWER OF 10 THAT R REPRESENTS
      POW = Log10(CDbl(r))

      'PARTITION POW SO THAT IT IS THE SUM OF A POTENTIALLY SIGNED
      'WHOLE NUMBER + A POSITIVE FRACTIONAL NUMBER

      If POW >= 0# Then
        n = Fix(POW)
      Else
        n = -Fix(-POW) - 1
      End If
      WHOLE = n
      FRAC = POW - WHOLE

      'CONVERT FRAC TO A NUMBER IN THE SCALE OF R

      tp = 10# ^ FRAC

      'NOW FIND A NICE WHOLE NUMBER THAT WHEN MULTIPLIED BY 10**N WILL
      'BE AS CLOSE TO R AS POSSIBLE BUT STILL BE LARGER.
      'WE KNOW THAT 1.0 <= TP < 10.0
      If tp <= 1# Then
        MULT = 1#
      ElseIf tp <= 2# Then
        MULT = 2#
      ElseIf tp <= 5# Then
        MULT = 5#
      Else
        MULT = 10#
      End If

      If n >= 0 Then
        RNDBST = MULT * 10 ^ n
      Else
        n = -n
        RNDBST = MULT / 10 ^ n
      End If
    End If

    End Function

Public Sub AXSCL1(xmin!, xmax!, AXLEN!, TICSPC!, LFRAC!, RFRAC!, NTICS&, PLMIN!, PLMAX!)

    'FIND THE MAXIMUM AND MINIMUM VALUES TO USE ON AN AXIS SO THAT
    'THE TICS ARE SPACED PROPERLY AND THE LABELS ARE NICE.

    'XMIN  - minimum value to be plotted-world units
    'XMAX  - maximum value to be plotted-world units
    'AXLEN - length of the axis
    'TICSPC- distance between tics on the axis in the same units
    '        as AXLEN.  The units of AXLEN and TICSPC can be any
    '         convenient set.
    'LFRAC - the decimal fraction of the range by which to reduce
    '        XMIN to avoid having one or more plotted points fall on the
    '        edge of the graph
    'RFRAC - the decimal fraction of the range by which to increase
    '        XMAX to avoid having one or more  plotted points fall on the
    '        edge of the graph
    'NTICS - number of tics on the axis.
    'PLMIN - minimum value to appear on a tic label
    'PLMAX - maximum value to appear on a tic label

    Dim NMAX&, n&
    Dim DX!, XTMIN!, XTMAX!, TX!, RTX!, FN!, tp!

    'ESTIMATE THE APPROXIMATE MAXIMUM NUMBER OF TICS

    NMAX = AXLEN / TICSPC + 1

    'ESTIMATE THE LENGTH OF THE AXIS IN WORLD UNITS

    DX = xmax - xmin

    If DX = 0# Then
      'The curve is a straight line perpendicular to the axis.
      DX = 1#
    End If
    'INFLATE THE LIMITS BASED ON FRACTIONS OF THE RANGE

    XTMIN = xmin - LFRAC * DX
    XTMAX = xmax + RFRAC * DX

    'NOW GET THE NEW LENGTH FOR THE AXIS IN WORLD UNITS
    DX = XTMAX - XTMIN

    'COMPUTE THE INTERVAL BETWEEN TICS

    TX = DX / (NMAX - 1)

    'FIND A NICE VALUE THAT INCLUDES TX

    RTX = RNDBST(TX)

    'NOW ADJUST THE MINIMUM VALUE TO BE ROUNDED TO A VALUE THAT IS
    'AN INTEGER MULTIPLE OF RTX BUT EQUAL TO OR LESS THAN XTMIN

    If XTMIN >= 0# Then
      tp = (Fix(XTMIN / RTX + 1)) * RTX
      If (tp - XTMIN) / (XTMIN + RTX) > 0.00001 Then tp = tp - RTX
    Else
      tp = (Fix(XTMIN / RTX - 0.9999)) * RTX
      If (tp - XTMIN) / Abs(XTMIN) > 0.00001 Then tp = tp - RTX
    End If
    XTMIN = tp

    'ADJUST THE MAXIMUM VALUE TO BE AN INTEGER MULTIPLE OF RTX BUT
    'EQUAL TO OR LARGER THAN XTMAX
    If XTMAX >= 0# Then
      tp = (Fix(XTMAX / RTX)) * RTX
      If (XTMAX - tp) / (XTMAX + RTX) > 0.00001 Then tp = tp + RTX
    Else
      tp = (Fix(XTMAX / RTX - 0.0001)) * RTX
      If (XTMAX - tp) / Abs(XTMAX) > 0.00001 Then tp = tp + RTX
    End If
    XTMAX = tp

    'Compute the number of tics
    FN = (XTMAX - XTMIN) / RTX + 0.01
    n = Fix(FN)
    NTICS = n + 1
    PLMIN = XTMIN
    PLMAX = XTMAX

    End Sub

Public Sub AuxAxe(outG As Object, XLen!, YLen!, ALen!, SizeL!, YALabel$, tics&, pmin!, pmax!, rmax!)
  'labels the axis for the auxilary plot

  '+ + + ARGUMENT DEFINITIONS + + +
  'XLEN   - length of x-axis, in world coordinates
  'YLEN   - length of y-axis or length of main Y-axis and
  '         auxilary axis plus small space between, in world coordinates
  'ALEN   - auxilary plot axis length
  'SIZEL  - height of lettering, in world coordinates
  'YALABL - label for auxilary plot on top
  'TICS   - number of tics on axis less one on bottom
  'PMIN   - minimum value for axis
  'PMAX   - maximum value for axis
  'RMAX   - inches from axis for bottom of label

  Dim i&, tmax&, NumL&, pos&, ilen&, pos1&(6), pos2&(6)
  Dim xp!, yp!, base!, dif!, val!, TLen!, tstr$
  Dim lwid!, lhgt!, tleft!, twidth!, ypos!
  Dim FontToUse&, FontToSave&

  Const nxtlin$ = "&"

  base = YLen + SizeL

  'Make box, begin and end at lower left
  outG.Line (0, base)-(0, base + ALen)
  outG.Line -(XLen, base + ALen)
  outG.Line -(XLen, base)
  outG.Line -(0, base)
  
  'add tics
  If tics < 1 Then tics = 1
  dif = ALen / tics
  xp = 0.5 * SizeL
  For i = 2 To tics
    yp = base + (i - 1) * dif
    outG.Line (0, yp)-(xp, yp)
  Next i
  xp = XLen - 0.5 * SizeL
  For i = 2 To tics
    yp = base + (i - 1) * dif
    outG.Line (XLen, yp)-(xp, yp)
  Next i

  dif = (pmax - pmin) / tics
  For i = 1 To tics + 1
    val = pmin + dif * (i - 1)
'      CALL DECCHX (VAL,LEN,SIGDIG,DECPLA,STR)
'      CALL CLNSFT (LEN,IFLG,STR,OLEN,IDUM)
'      xp = -0.8 * SizeL * (Real(OLEN) + 1#)
    outG.CurrentX = -1.1 * outG.TextWidth(CStr(val))
    outG.CurrentY = base + ((i - 1) * ALen / tics) + 0.5 * SizeL
    frmG.GraphPrint CStr(val)
'      Call ANGTXA(xp, yp, OLEN, Str, ihv)
  Next i

  'determine max # char/line
  tmax = Int(ALen / (0.75 * SizeL)) + 2
  If tmax > 40 Then tmax = 40
  'determine number of lines
  NumL = 1
  pos = 1
  While pos > 0
    If InStr(pos, YALabel, nxtlin) > 0 And _
      NumL < 7 Then
      pos2(NumL) = InStr(pos, YALabel, nxtlin)
      pos = pos2(NumL) + 1
      NumL = NumL + 1
    Else
      pos = 0
    End If
  Wend

  'set begin and end positions of each line in buffer
  pos1(1) = 1
  i = 1
  While i <= NumL
    pos1(i + 1) = pos2(i) + 1
    pos2(i) = pos2(i) - 1
    i = i + 1
  Wend
  pos2(NumL) = 80

  For i = 1 To NumL
    ilen = pos2(i) - pos1(i) + 1
    tstr = Mid(YALabel, pos1(i), ilen)
    'temporarily set horizontal scale to
    'vertical to determine vertical text extent
    tleft = outG.ScaleLeft
    twidth = outG.ScaleWidth
    outG.ScaleLeft = 0
    outG.ScaleWidth = Abs(outG.ScaleHeight)
    'multiply text extent by width/height ratio
    TLen = Abs(outG.ScaleWidth / outG.ScaleHeight * outG.TextWidth(tstr))
    'reset horizontal scale values
    outG.ScaleLeft = tleft
    outG.ScaleWidth = twidth
    If TLen > 0 Then
      If TLen > tmax Then TLen = tmax
      outG.CurrentX = rmax - (NumL - i) * 0.75 * SizeL
      outG.CurrentY = base + ALen * 0.5 - 0.5 * TLen
      'draw the label
      frmG.GraphPrint tstr, 90
    End If
  Next i
End Sub

Public Sub matchl(outG As Object, SizeL!, amin!, amax!, ymin!, ymax!, ytype&, atics&, ytics&, rmax1!)
    'determine minimum distance from left axes to
    'line up labels for y and aux axes
    Dim i&
    Dim yinc!, rtmp!

    rmax1 = 0
    'start w/aux lengths
    yinc = (amax - amin) / atics
    rtmp = amin
    For i = 0 To atics
      If outG.TextWidth(CStr(rtmp)) > rmax1 Then
        rmax1 = outG.TextWidth(CStr(rtmp))
      End If
      rtmp = rtmp + yinc
    Next i

    'now do main y lengths
    If ytype = 1 Then  'arithmetic, check all labels
      yinc = (ymax - ymin) / ytics
      rtmp = ymin
      For i = 0 To ytics
        If outG.TextWidth(CStr(rtmp)) > rmax1 Then
          rmax1 = outG.TextWidth(CStr(rtmp))
        End If
        rtmp = rtmp + yinc
      Next i
    Else  'use largest log cycle
      rtmp = 10 ^ ymax
      If outG.TextWidth(rtmp) > rmax1 Then
        rmax1 = outG.TextWidth(rtmp)
      End If
    End If
    rmax1 = -rmax1 - 1.25 * SizeL

End Sub

Public Sub PbAxis(outG As Object, XLen!, YLen!, AType&, LabelX$, TLen!, xmin!, xmax!, stdev!, Gridx&)
Attribute PbAxis.VB_Description = "put tics and numbers on a probability x-axis"
    Dim i&, j&, axfg&, iMin&, iMax&, lbl$
    Dim xpos!, t!, xused!, xneed!, xlp!, thgt!
    Static inifg&, xp(18) As Single, pct(18) As Single, rtp(18) As Single, dscal!

    If inifg = 0 Then
      'init percentage and return period values
      Call PbInit(pct(), rtp())
      inifg = 1
    End If

    'draw left and right axes lines
    outG.Line (0, 0)-(0, YLen)
    outG.Line (XLen, 0)-(XLen, YLen)

    dscal = XLen / (2 * stdev)
    
    axfg = 0
    iMin = 19
    For i = 0 To 18
      If AType <= 3 Then
        'exceedance plot (scale 99-1)
        j = i
      Else
        'non-exceedance plot (scale 1-99)
        j = 18 - i
      End If
      If AType = 2 Or AType = 5 Then
        t = 1# / rtp(j)
      Else
        t = pct(j) / 100#
      End If
      'On Error Resume Next
      xp(j) = dscal * (gausex(t) + stdev) 'was xpos=
      If xp(j) > 0 And xp(j) < XLen Then
        If i < iMin Then iMin = i
        'just do tics here
        Call GridTic(outG, 4, xp(j), XLen, YLen, Gridx, TLen / 2, "")
      End If
'      If (xpos > 0 And axfg = 0) Then
'        Call GridTic(outG, 4, 0, XLen, YLen, 0, TLen, "")
'        axfg = 1
'      ElseIf (xpos > XLen And axfg = 1) Then
'        Call GridTic(outG, 4, XLen, XLen, YLen, 0, TLen, "")
'        axfg = 2
'      End If
'      'within x-axis range, put bottom/top tics
'      Call GridTic(outG, 4, xpos, XLen, YLen, Gridx, TLen / 2, lbl)
    Next i
    iMax = 19 - iMin
    'now label the tics done above
    thgt = Abs(outG.TextHeight(""))
    xused = 0.5 * thgt
    For i = iMin To iMax
      If AType <= 3 Then
        j = i
      Else
        j = 18 - i
      End If
      'labels
      If AType = 2 Or AType = 5 Then
        lbl = CStr(rtp(j))
      ElseIf AType = 3 Or AType = 6 Then
        lbl = CStr(0.01 * pct(j))
      Else
        lbl = CStr(pct(j))
      End If
      xneed = outG.TextWidth(lbl)
      xlp = xp(i) - 0.5 * xneed
      If xlp > (xused + 0.5 * thgt) Then
        'enough space to label the tic
        Call GridTic(outG, 4, xp(i), XLen, YLen, Gridx, TLen / 2, lbl)
        xused = xp(i) + 0.5 * xneed
      End If
    Next i
    'label the X axis
    Call LabX(outG, XLen, LabelX)
End Sub

Private Sub PbInit(pct!(), rtp!())
Attribute PbInit.VB_Description = "initialize array constants for probability axis"
    pct(0) = 99.9
    pct(1) = 99.8
    pct(2) = 99.5
    pct(3) = 99
    pct(4) = 98
    pct(5) = 95
    pct(6) = 90
    pct(7) = 80
    pct(8) = 70
    pct(9) = 50
    pct(10) = 30
    pct(11) = 20
    pct(12) = 10
    pct(13) = 5
    pct(14) = 2
    pct(15) = 1
    pct(16) = 0.5
    pct(17) = 0.2
    pct(18) = 0.1
    rtp(0) = 1.001
    rtp(1) = 1.002
    rtp(2) = 1.005
    rtp(3) = 1.01
    rtp(4) = 1.02
    rtp(5) = 1.05
    rtp(6) = 1.1
    rtp(7) = 1.25
    rtp(8) = 1.5
    rtp(9) = 2
    rtp(10) = 3
    rtp(11) = 5
    rtp(12) = 10
    rtp(13) = 20
    rtp(14) = 50
    rtp(15) = 100
    rtp(16) = 200
    rtp(17) = 500
    rtp(18) = 1000
End Sub

Public Sub RhtBdr(outG As Object, Trans&, XLen!, YLen!, TLen!, tics&, Miny!, Maxy!)
    'put tics on an unused right y-axis
    Dim i&, j&, yp!, posinc!, ypp!

    If tics < 1 Then tics = 1
    If Trans = 1 Then 'arithmetic
      posinc = YLen / tics
      yp = 0
      For i = 1 To tics - 1
        yp = yp + posinc
        Call GridTic(outG, 2, yp, XLen, YLen, 0, TLen, "")
      Next i
    Else 'logarithmic
      posinc = YLen / (Maxy - Miny)
      yp = 0
      For i = Miny + 1 To Maxy
        'might want intermediate tics
        If Maxy - Miny < 5 Then
          'put small intermediate tics in between cycles
          For j = 2 To 9
            ypp = yp + Log10(CDbl(j)) * posinc
            Call GridTic(outG, 2, ypp, XLen, YLen, 0, TLen / 2, "")
          Next j
        End If
        yp = yp + posinc
        Call GridTic(outG, 2, yp, XLen, YLen, 0, TLen, "")
      Next i
    End If

End Sub

Public Sub AxAxis(outG As Object, XLen!, YLen!, LabelX$, xmin!, xmax!, TLen!, tics&, Gridx&)
Attribute AxAxis.VB_Description = "put tics and numbers on an arithmetic x-axis"
    'put tics and numbers on an arithmetic x-axis
    Dim i&, xp!, xv!, posinc!, valinc!, ndpl&, lbl$

    If tics < 1 Then tics = 1
    posinc = XLen / tics
    valinc = (xmax - xmin) / tics
    Call DecReq(xmin, valinc, tics, ndpl)
    xv = xmin
    For i = 0 To tics
      If ndpl > 0 Then 'decimals needed
        Call F90_DECCHX(xv, 12, 4, ndpl, lbl)
        lbl = Trim(lbl) 'trim any blanks
        Call DecTrim(lbl) 'trim trailing "0"s
      Else
        lbl = CStr(xv)
      End If
      Call GridTic(outG, 4, xp, XLen, YLen, Gridx, TLen, lbl)
      xp = xp + posinc
      xv = xv + valinc
    Next i
    'label the X axis
    Call LabX(outG, XLen, LabelX)

End Sub

Public Sub TmAxisCalc(outG As Object, XLen!, YLen!, ALen!, LabelX$, TLen!, Gridx&, ncrv&, TSTEP&(), TUnits&(), sdatim&(), edatim&(), ConstInt As Boolean, SType&, SInt&)
  Dim tumin&, k&, l5&, l1&, tmp&, flg1&, flg2&, npts&, tsmin&
  Dim t1&, t2&, i&
   
    'determine minimum time units
    tumin = TUnits(0)
    tsmin = TSTEP(0)
    If ConstInt Then
      If ncrv > 1 Then
        For k = 1 To ncrv - 1
          t1 = TUnits(k)
          t2 = TSTEP(k)
          Call F90_CMPTIM(tumin, tsmin, t1, t2, flg1, flg2)
          If flg2 = 2 Then
            'found shorter time interval
            tumin = TUnits(k)
            tsmin = TSTEP(k)
          End If
        Next k
      End If

      If tumin = 2 And tsmin = 1440 Then 'daily time step
        tumin = 4
        tsmin = 1
      End If

      If tumin = 3 Then 'convert hour to minutes
        tumin = 2
        tsmin = 60 * tsmin
      End If

      If tumin = 2 Then 'check case of several months with short time interval
        Call timdif(sdatim, edatim, 5, 1, tmp)
        If (tmp >= 3) Then
          tumin = 4
          tsmin = 1
        End If
      End If
    Else 'not constant interval, get to < 50 intervals
      Do
        Call timdif(sdatim, edatim, tumin, tsmin, npts)
        If npts <= 50 Or tumin = 6 Then 'good interval or best we can do
          Exit Do
        ElseIf tumin < 6 Then 'try larger time units
          tumin = tumin + 1
        End If
      Loop
      If tumin = 3 Then
        tumin = 2
        tsmin = tsmin * 60
      End If
    End If

    i = DateIntrvl(sdatim) 'determine interval boundary of start of data
    If i < tumin Then 'make sure X axis starts correctly
      tumin = i
      If tumin < 4 Then
        tumin = 4 'less than day does not work
        If tsmin = 60 Then tsmin = 1
      End If
    End If
    
    SType = tumin
    SInt = tsmin
    
End Sub

Public Sub TmAxis(outG As Object, XLen!, YLen!, ALen!, sdatim&(), SType&, npts&, ndays&, tstype&, tsmin&, grid&)
    Dim SIZELX!, SIZELY!, BOTOM!
    Dim lSdatim(6) As Long
  
    SIZELX = Abs(outG.TextWidth("D"))
    SIZELY = Abs(outG.TextHeight("D"))
    J2Date Date2J(sdatim), lSdatim 'midnight at 0:0 (not 24:00 of prev day)
    If SType >= 6 Then 'annual data
      Call YrAxis(outG, XLen, YLen, npts, SIZELX, SIZELY, lSdatim(0), ALen, tstype, BOTOM, grid)
    ElseIf SType = 5 Then 'monthly data
      'If sdatim(2) > 27 Then sdatim(1) = sdatim(1) + 1
      Call MoAxis(outG, XLen, YLen, npts, SIZELX, SIZELY, ALen, lSdatim(0), lSdatim(1), lSdatim(2), ndays, tstype, BOTOM, grid)
    ElseIf SType = 4 Then 'daily data
      Call DyAxis(outG, XLen, YLen, ndays, SIZELX, SIZELY, ALen, lSdatim(0), lSdatim(1), lSdatim(2), BOTOM, grid)
    Else 'hourly or min data
      Call MiAxis(outG, XLen, YLen, tsmin, npts, SIZELX, SIZELY, lSdatim, ALen, BOTOM, grid)
    End If
End Sub

Private Sub YrAxis(outG As Object, XLen!, YLen!, npts&, SIZELX!, SIZELY!, yr&, ALen!, tstype&, BOTOM!, grid&)
    'draws time axis for annual data using years.
    'XLEN   - length of X-axis, in world coordinates
    'YLEN   - length of Y-axis, in world coordinates
    'NPTS   - number of points
    'SIZELY - height of lettering, in world Y coordinates
    'SIZELX - height of lettering, in world X coordinates
    'YR     - starting year
    'ALEN   - auxilary plot axis length
    '         0 - no auxilary plot
    '        >0 - length of auxilary plot
    'TSTYPE - type of time series (2-point, 1-mean)
    'BOTOM  - location of botom line of label in world
    '         coordinates below axis
    Dim i&, IYR&, ICHK&, TBFLG&
    Dim xinc!, yp!, xp!, fract!
    Dim lbl$

    xinc = XLen / CSng(npts)
    'set year labels every 1, 2, 5, 10, 20, ...years
    lbl = CStr(yr)
    ICHK = CLng(outG.TextWidth(lbl) / xinc) + 1
    If (ICHK = 4 Or ICHK = 3) Then
      ICHK = 5
    ElseIf (ICHK > 5) Then
      ICHK = ((ICHK - 1) / 10) * 10 + 10
    End If

    'draw left and right axes lines
    outG.Line (0, 0)-(0, YLen)
    outG.Line (XLen, 0)-(XLen, YLen)

    'year
    IYR = yr - 1
    For i = 1 To npts Step 1
      IYR = IYR + 1
      xp = i * xinc
      yp = 0#
      If (IYR Mod ICHK = 0) Then
        'print year
        If (ICHK < 5) Then
          fract = 0.25
        Else
          fract = 0.5
        End If
        If i < npts Then Call TmTics(outG, xp, YLen, SIZELY, fract, ALen, grid)

        lbl = CStr(IYR)
        If (tstype = 1) Then
          'center in space
          xp = xp - 0.5 * xinc - 0.5 * outG.TextWidth(lbl)
        Else
          'center on point
          xp = xp - 0.5 * outG.TextWidth(lbl)
        End If
        yp = -1# * SIZELY
        Call Angtx(outG, xp, yp, lbl, 1)
      Else
        fract = 0.25
        If i < npts Then Call TmTics(outG, xp, YLen, SIZELY, fract, ALen, grid)
      End If
    Next i

End Sub

Private Sub MoAxis(outG As Object, XLen!, YLen!, npts&, SIZELX!, SIZELY!, ALen!, ly&, LM&, LD&, ndays&, tstype&, BOTOM!, grid&)
    'XLEN   - length of X-axis, in world coordinates
    'YLEN   - length of Y-axis, in world coordinates
    'NPTS   - number of months
    'SIZEL  - height of lettering, in world coordinates
    'ALEN   - auxilary plot axis length
    '         0 - no auxilary plot
    '        >0 - length of auxilary plot
    'LY     - year
    'LM     - month
    'LD     - day
    'NDAYS  - total number of days for axis
    'TSTYPE - time series type (2-point, 1-mean)
    'BOTOM  - location of botom line of label in world
    '         coordinates below axis
    Dim i&, CUM&, DM&, id&, CUMM&, ICHK&, lbl$
    Dim xinc!, xp!, xpos!, ypos!, yoff!, INPMO!, SPACE!, fract!
    Dim MonthName As String
    Dim MonthNameWidth As Single
    Dim YrLabeled As Boolean
    Static MONTH$(12), mon$(12), m$(12), initfg&

    If initfg = 0 Then
      Call MonthNameInit(initfg, MONTH, mon, m)
    End If

    'set year labels every 1, 2, 5, 10, 20, ...years
    xinc = XLen / (CSng(npts) / 12#)

    lbl = CStr(ly)
    ICHK = CLng(outG.TextWidth(lbl) / xinc) + 1
    If (ICHK = 4 Or ICHK = 3) Then
      ICHK = 5
    ElseIf (ICHK > 5) Then
      ICHK = ((ICHK - 1) / 10) * 10 + 10
    End If

    xinc = XLen / CSng(ndays) 'inches per day on plot
    CUM = 0
    INPMO = xinc * 30#

    'draw left and right axes lines
    outG.Line (0, 0)-(0, YLen)
    outG.Line (XLen, 0)-(XLen, YLen)
    
    LM = LM - 1
    xp = 0# - xinc * LD 'If we are not starting on a month boundary, back up start of month left of Y axis
    CUMM = 0
    While xp < XLen
      LM = LM + 1
      If (LM > 12) Then
        ly = ly + 1
        YrLabeled = False
        LM = 1
      End If
      id = F90_DAYMON(ly, LM)

      If (INPMO >= 10# * SIZELX) Then
        MonthName = MONTH(LM)        'print month and spell out
      ElseIf (INPMO >= 5# * SIZELX) Then
        MonthName = mon(LM)          'print month, space for 4 letters
      ElseIf (INPMO > SIZELX) Then
        MonthName = m(LM)            'for real small plots just label with first letter of the month.
      Else
        MonthName = ""               'months won't be shown since not enough space
      End If
      If Len(MonthName) > 0 Then
        MonthNameWidth = outG.TextWidth(MonthName)
        xpos = xp + 0.5 * CSng(id) * xinc
        If (tstype = 1) Then
          xpos = xpos - 0.5 * MonthNameWidth
        End If
        ypos = -0.5 * SIZELY
        If xpos > 0 And xpos + MonthNameWidth < XLen Then
          Call Angtx(outG, xpos, ypos, MonthName, 1)
        End If
      End If
      DM = F90_DAYMON(ly, LM)
      CUM = CUM + DM
      xp = xp + CSng(DM) * xinc
      If (INPMO >= SIZELX And LM = 12) Then
        fract = 0.5
      Else
        fract = 0.25
      End If
      If (INPMO >= SIZELX Or LM = 12) Then
        If xp < XLen Then Call TmTics(outG, xp, YLen, SIZELY, fract, ALen, grid)
      End If

      CUMM = CUMM + id
      If (LM = 12 Or (Not (YrLabeled) And xp + INPMO > XLen)) Then
        'label the year
        If (INPMO > SIZELX) Then
          'add year when months were shown
          SPACE = CSng(CUMM) * xinc
          If (SPACE > 6# * SIZELX) Then
            'enough space for printing the year
            xpos = xp - 0.5 * SPACE - 1.8 * SIZELX
            ypos = -1.5 * SIZELY
            lbl = CStr(ly)
            If (ly < 2100) Then
              'not "generic" year so plot
              If xpos > 0 Then Call Angtx(outG, xpos, ypos, lbl, 1)
            End If
          End If
          If (xp + INPMO < XLen) Then
            'put year mark if not beginning or end
            If (tstype = 1) Then
              ypos = -0.5 * SIZELY
            Else
              ypos = -1# * SIZELY
            End If
            yoff = -1.5 * SIZELY
            outG.Line (xp, ypos)-Step(0, yoff)
          End If
          CUMM = 0
        Else
          'no months, just label years
          SPACE = CSng(CUMM) * xinc
          If (ly Mod ICHK = 0) Then
            'print year
            If (ICHK < 5) Then
              fract = 0.25
            Else
              fract = 0.5
            End If
            If xp < XLen Then Call TmTics(outG, xp, YLen, SIZELY, fract, ALen, grid)
            lbl = CStr(ly)
            If (tstype = 1) Then
              'center in space
              xpos = xp - 0.5 * SPACE - 0.5 * outG.TextWidth(lbl)
            Else
              'center on point
              xpos = xp - 0.5 * outG.TextWidth(lbl)
            End If
            ypos = -1# * SIZELY
            If (xpos > 0#) Then
              'check on first year
              If xpos > 0 Then Call Angtx(outG, xpos, ypos, lbl, 1)
            End If
          Else
            fract = 0.25
            If xp < XLen Then Call TmTics(outG, xp, YLen, SIZELY, fract, ALen, grid)
          End If
          CUMM = 0
        End If
        YrLabeled = True
      End If
    Wend

    BOTOM = -2# * SIZELY

End Sub


Private Sub DyAxis(outG As Object, XLen!, YLen!, npts&, SIZELX!, SIZELY!, ALen!, ly&, LM&, LD&, BOTOM!, grid&)
    'draws time axis using days, months and years.
    'XLEN   - length of X-axis, in world coordinates
    'YLEN   - length of Y-axis, in world coordinates
    'NPTS   - number of days
    'SIZELX - horiz. height of lettering, in X world coordinates
    'SIZELY - vert. width of lettering, in Y world coordinates
    'ALEN   - auxilary plot axis length
    '         0 - no auxilary plot
    '        >0 - length of auxilary plot
    'LY     - year
    'LM     - month
    'LD     - day
    'BOTOM  - location of botom line of label in world
    '         coordinates below axis
    Dim CN&, i&, CUMM&, JUST&, l&, olen&, ihv&, DPM&, ILY&, ILM&
    Dim YCHK&, ILD&
    Dim INPMO!, INPDY!, xp!, xpos!, ypos!, yoff!, SPACE!, fract!, x!(2), y!(2)
    Dim CLEN!, INPYR!, YPOSDY!, YPOSMO!, YPOSYR!, AVAIL!, NEED!
    Static MONTH$(12), mon$(12), m$(12), initfg&
    Dim lbl$
    Dim PRTDY As Boolean, PRTMO As Boolean
    Dim CurMoPrt As Boolean, MoStrtIndx& ' jlk 2/98 be sure to print something

    If initfg = 0 Then
      Call MonthNameInit(initfg, MONTH, mon, m)
    End If
    ILY = ly
    ILM = LM
    ILD = LD
    'inches per day
    INPDY = XLen / CSng(npts)
    'inches per month
    INPMO = 30# * INPDY
    'inches per year
    INPYR = 365# * INPDY
    'offset if days numbered on axis, and flag for tics
    PRTDY = True
    'CN is frequency of days on axis label
    If (INPMO > 100# * SIZELX) Then
      CN = 1
    ElseIf (INPMO > 50# * SIZELX) Then
      CN = 2
    ElseIf (INPMO > 24 * SIZELX) Then
      CN = 5
    ElseIf (INPMO > 10# * SIZELX) Then
      CN = 10
    ElseIf (INPMO > 7# * SIZELX) Then
      CN = 15
    Else
      'no days will be printed
      CN = 32
      PRTDY = False
    End If

    'set year labels every 1, 2, 5, 10, 20, ...years
    lbl = CStr(ly)
    YCHK = Int(1.25 * outG.TextWidth(lbl) / INPYR) + 1
    If (YCHK = 4 Or YCHK = 3) Then
      YCHK = 5
    ElseIf (YCHK > 5) Then
      YCHK = ((YCHK - 1) / 10) * 10 + 10
    End If

    'set conditions and positions for axis labels
    If (INPYR > (1.1 * outG.TextWidth("JFMAMJJAASOND"))) Then
      PRTMO = True
    Else
      PRTMO = False
    End If
    If (PRTDY) Then
      YPOSDY = -0.1 * SIZELY
      YPOSMO = -0.8 * SIZELY
      YPOSYR = -1.5 * SIZELY
    ElseIf (PRTMO) Then
      YPOSDY = 0#
      YPOSMO = -0.5 * SIZELY
      YPOSYR = -1.5 * SIZELY
    Else
      YPOSDY = 0#
      YPOSMO = 0#
      YPOSYR = -1# * SIZELY
    End If

    'draw left and right axes lines
    outG.Line (0, 0)-(0, YLen)
    outG.Line (XLen, 0)-(XLen, YLen)
    
    LD = LD - 1
    CUMM = 0
    CurMoPrt = False
    MoStrtIndx = 1
    For i = 1 To npts Step 1
      LD = LD + 1
      If (LD > F90_DAYMON(ly, LM)) Then
        MoStrtIndx = i
        CurMoPrt = False
        LM = LM + 1
        LD = 1
        If (LM > 12) Then
          ly = ly + 1
          LM = 1
        End If
      End If
      xp = CSng(i) * INPDY

      If (PRTMO) Then
        'enough space to put more than just the year
        If (LD = F90_DAYMON(ly, LM)) Then
          'print end-of-month marker
          fract = 0.5
          If i < npts Then Call TmTics(outG, xp, YLen, SIZELY, fract, ALen, grid)
        End If
        If (INPMO >= 5# * SIZELX) Then
          If (LD = 15) Then
            'print month
            If ((ly <> ILY Or LM <> ILM) And npts - i >= 13) Then
              CurMoPrt = True
              'not first or last month so not special case
              'note NPTS-I is number of days after the 15th that
              'are left in the last month remaining in month,
              '15+"13" = 28 in case last month is February
              If (INPMO >= 10# * SIZELX) Then
                'print month and spell out
                xpos = xp - 0.5 * outG.TextWidth(MONTH(LM))
                ypos = YPOSMO
                ihv = 1
                Call Angtx(outG, xpos, ypos, MONTH(LM), ihv)
              Else
                'use abbreviation
                xpos = xp - 0.5 * outG.TextWidth(mon(LM))
                ypos = YPOSMO
                ihv = 1
                Call Angtx(outG, xpos, ypos, mon(LM), ihv)
              End If
            End If
          End If

          If (LD = F90_DAYMON(ly, LM) And LM = ILM And ly = ILY) Then
            'first month, check for space
            AVAIL = INPDY * CSng(F90_DAYMON(ly, LM) - ILD + 1)
            If (INPMO >= 10# * SIZELX) Then
              NEED = outG.TextWidth(MONTH(LM))
            Else
              NEED = outG.TextWidth(mon(LM))
            End If
            If (NEED < 1.1 * AVAIL) Then
              CurMoPrt = True
              'print the month
              xpos = xp - 0.5 * AVAIL - 0.5 * NEED
              ypos = YPOSMO
              ihv = 1
              If (INPMO >= 10# * SIZELX) Then
                Call Angtx(outG, xpos, ypos, MONTH(LM), ihv)
              Else
                Call Angtx(outG, xpos, ypos, mon(LM), ihv)
              End If
            End If
          End If
          'If (i = npts And LD < 28) Then
          If (i = npts And Not (CurMoPrt)) Then
            'last month with less than 28 days to plot
            'check for space
            AVAIL = INPDY * CSng(npts - MoStrtIndx + 1)
            If (INPMO >= 10# * SIZELX) Then
              NEED = outG.TextWidth(MONTH(LM))
            Else
              NEED = outG.TextWidth(mon(LM))
            End If
            If (NEED < 1.1 * AVAIL) Then
              'print the month
              xpos = xp - 0.5 * AVAIL - 0.5 * NEED
              ypos = YPOSMO
              ihv = 1
              If (INPMO >= 10# * SIZELX) Then
                Call Angtx(outG, xpos, ypos, MONTH(LM), ihv)
              Else
                Call Angtx(outG, xpos, ypos, mon(LM), ihv)
              End If
            End If
          End If
          If (PRTDY) Then
            DPM = F90_DAYMON(ly, LM)
            If (LD Mod CN = 0 Or LD = DPM) Then
              'print every CN-TH day and last day of month
              'xpos = xp - 0.6 * SIZELX
              'If (LD < 10) Then
              '  xpos = xp - 0.3 * SIZELX
              'End If
              If ((DPM - LD) * INPDY > 2.5 * SIZELX Or LD = DPM) Then
                'enough space for day numbers
                ypos = YPOSDY
                lbl = CStr(LD)
                If CN = 1 Then
                  xpos = xp - (INPDY + outG.TextWidth(lbl)) * 0.5
                Else
                  xpos = xp - outG.TextWidth(lbl)
                End If
                ihv = 1
                Call Angtx(outG, xpos, ypos, lbl, ihv)
                fract = 0.25
                If i < npts Then Call TmTics(outG, xp, YLen, SIZELY, fract, ALen, grid)
              End If
            End If
          End If
        Else
          'for real small plots just label with first letter of the month.
          If (LD = 15) Then
            'print first letter of the month
            CLEN = outG.TextWidth(m(3))
            If (CLEN * 1.2 < INPMO Or LM Mod 2 = 0) Then
              'enough room for month character
              xpos = xp - 0.5 * outG.TextWidth(m(LM))
              ypos = YPOSMO
              ihv = 1
              Call Angtx(outG, xpos, ypos, m(LM), ihv)
            End If
          End If
        End If
      End If

      CUMM = CUMM + 1
      If ((LM = 12 And LD = 31) Or (i = npts)) Then
        If (ly Mod YCHK = 0) Then
          'label the year
          lbl = CStr(ly)
          If (CSng(CUMM) * INPDY > 1.25 * outG.TextWidth(lbl) Or CUMM >= 365) Then
            'write year if full year or partial year with enough space
            SPACE = CSng(CUMM) * INPDY
            xpos = xp - 0.5 * SPACE - 0.5 * outG.TextWidth(lbl)
            ypos = YPOSYR
            If (ly < 2100) Then
              'not "generic" year so plot
              ihv = 1
              Call Angtx(outG, xpos, ypos, lbl, ihv)
            End If
          End If
        End If
        If (i > 1 And i < npts) Then
          'put year mark if not beginning or end
          If (PRTMO) Then
            'case where months are labeled, put mark between years outside the plot
            ypos = -1.7 * SIZELY
            yoff = 1.55 * SIZELY
          Else
            'just years labeled, put tics inside
            ypos = 0#
            If (ly Mod YCHK = 0) Then
              yoff = 0.75 * SIZELY
            Else
              yoff = 0.5 * SIZELY
            End If
          End If
          outG.Line (xp, ypos)-Step(0, yoff)
          If grid = 1 Then
            outG.DrawStyle = 2
            outG.Line (xp, 0)-(xp, YLen), Grid_Color
            outG.DrawStyle = 0
          End If
        End If
        CUMM = 0
      End If
    Next i

    BOTOM = YPOSYR

End Sub

Sub MonthNameInit(initfg&, MONTH$(), mon$(), m$())
   initfg = 1
   MONTH(1) = "JANUARY"
   MONTH(2) = "FEBRUARY"
   MONTH(3) = "MARCH"
   MONTH(4) = "APRIL"
   MONTH(5) = "MAY"
   MONTH(6) = "JUNE"
   MONTH(7) = "JULY"
   MONTH(8) = "AUGUST"
   MONTH(9) = "SEPTEMBER"
   MONTH(10) = "OCTOBER"
   MONTH(11) = "NOVEMBER"
   MONTH(12) = "DECEMBER"
   mon(1) = "JAN"
   mon(2) = "FEB"
   mon(3) = "MAR"
   mon(4) = "APR"
   mon(5) = "MAY"
   mon(6) = "JUNE"
   mon(7) = "JULY"
   mon(8) = "AUG"
   mon(9) = "SEPT"
   mon(10) = "OCT"
   mon(11) = "NOV"
   mon(12) = "DEC"
   m(1) = "J"
   m(2) = "F"
   m(3) = "M"
   m(4) = "A"
   m(5) = "M"
   m(6) = "J"
   m(7) = "J"
   m(8) = "A"
   m(9) = "S"
   m(10) = "O"
   m(11) = "N"
   m(12) = "D"
End Sub

Private Sub MiAxis(outG As Object, XLen!, YLen!, tsmin&, npts&, SIZELX!, SIZELY!, sdatim&(), ALen!, BOTOM!, grid&)
    'for time steps less than one day costructs a horizontal time-axis using hour/minutes.
    'TSMIN  - timestep, in minutes
    'NPTS   - number of intervals to plot
    'SIZELY - height of lettering, in world Y coordinates
    'SIZELX - width of lettering, in world X coordinates
    'SDATIM - start date (yr,mo,dy,hr,mn,sc)
    'ALEN   - auxilary plot axis length
    '         0 - no auxilary plot
    '        >0 - length of auxilary plot
    'BOTOM  - location of botom line of label in world
    '         coordinates below axis
    Dim MPIN&, i&, UB&, IOPT&, DT&(6), INTV&, j&, INTH&, CUMM&, DD&
    Dim l!, TBFLG&, DCHK&(6), PASS2&, MYRFLG&
    Dim xp!, xpos!, ypos!, yoff!, SPACE!, fract!, xinc!, DPIN!, INPD!, YPOSMX!
    Dim lbl$, Dlbl$
    Static MONTH$(12), mon$(12), m$(12), initfg&

    If initfg = 0 Then
      Call MonthNameInit(initfg, MONTH, mon, m)
    End If

    Call CopyI(6, sdatim, DT)

    MPIN = CLng(tsmin * npts / XLen)
    'minutes per inch
    DPIN = CSng(MPIN) / 1440#
    'days per inch
    xinc = XLen / CSng(npts)
    CUMM = 0

    'draw left and right axes lines
    outG.Line (0, 0)-(0, YLen)
    outG.Line (XLen, 0)-(XLen, YLen)

    'check spacing
    DCHK(0) = 1999
    DCHK(1) = 9
    DCHK(2) = 30
    DCHK(3) = 0
    DCHK(4) = 0
    DCHK(5) = 0
    IOPT = 2
    'compute inches per day
    INPD = 1# / DPIN
    Call DatStrg(outG, DCHK, IOPT, INPD, SIZELX, l, Dlbl)

    If l > INPD Then
      'can't fit y/m/d under 1 day, so just put day and
      'label month/year for each month if space,
      'usually more than 1 day per inch
      If (DPIN < 3) Then
        'print every day
        DD = 1
      ElseIf (DPIN < 6) Then
        'print every other day
        DD = 2
      ElseIf (DPIN < 15) Then
        DD = 5
      ElseIf (DPIN < 30) Then
        DD = 10
      Else
        DD = 99
      End If

      MYRFLG = 0
      PASS2 = 0
      YPOSMX = -1.5
      For i = 1 To npts Step 1
        'increment time
        UB = 1
        Call F90_DATNXT(tsmin, UB, DT(0))
        xp = CSng(i) * xinc
        CUMM = CUMM + 1
        If ((DT(3) = 24 And DT(4) = 0 And DT(2) = F90_DAYMON(DT(0), DT(1))) Or i = npts) Then
          'label with month and year
          SPACE = CSng(CUMM) * xinc
          IOPT = 1
          Call DatStrg(outG, DT, IOPT, SPACE, SIZELX, l, Dlbl)
          If (SPACE > 9# * SIZELX) Then
            'enough space for printing the month/year
            IOPT = 1
            Call DatStrg(outG, DT, IOPT, SPACE, SIZELX, l, Dlbl)
            xpos = xp - 0.5 * SPACE - 0.5 * outG.TextWidth(Dlbl)
            ypos = -0.8 * SIZELY
            Call Angtx(outG, xpos, ypos, Dlbl, 1)
            MYRFLG = 1
          ElseIf (SPACE > 4# * SIZELX) Then
            'put abbreviated month
            xpos = xp - 0.5 * SPACE - 0.5 * outG.TextWidth(mon(DT(1)))
            ypos = -0.8 * SIZELY
            Call Angtx(outG, xpos, ypos, mon(DT(1)), 1)
          ElseIf (SPACE > SIZELX) Then
            'just put month character
            xpos = xp - 0.5 * SPACE - 0.5 * outG.TextWidth(m(DT(1)))
            ypos = -0.8 * SIZELY
            Call Angtx(outG, xpos, ypos, m(DT(1)), 1)
          End If

          If (DT(1) = 12 Or i = npts) Then
            'write year if needed
            If (MYRFLG = 0 And PASS2 = 1) Then
              'did not use year month on one line
              If (365# / DPIN < xp) Then
                'there is a full year on the plot
                lbl = CStr(DT(0))
                xpos = xp - 0.5 * 365# / DPIN - 0.5 * outG.TextWidth(lbl)
                ypos = -2.3 * SIZELY
                Call Angtx(outG, xpos, ypos, lbl, 1)
                YPOSMX = -2.3
              ElseIf (3.5 * SIZELX < xp) Then
                'room for partial year
                lbl = CStr(DT(0))
                xpos = xp - 0.5 * xp - 0.5 * outG.TextWidth(lbl)
                ypos = -1.5 * SIZELY
                Call Angtx(outG, xpos, ypos, lbl, 1)
                YPOSMX = -2.3
              Else
                'no room for year
              End If
            End If
          End If

          If (i > 1 And i < npts) Then
            'put month mark if not beginning or end
            ypos = -0.8 * SIZELY
            yoff = YPOSMX * SIZELY
            outG.Line (xp, ypos)-(xp, yoff)
'            yoff = -2# * SIZELY
'            outG.Line (xp, ypos)-Step(0, yoff)
          End If
          CUMM = 0
          PASS2 = 1
          fract = 0.5
          If i < npts Then Call TmTics(outG, xp, YLen, SIZELY, fract, ALen, grid)
        End If

        If (DT(3) = 12 And DT(4) = 0 And DD = 1) Then
          'print day between tics
          xpos = xp - SIZELX
          If (DT(2) < 10) Then
            xpos = xp - 0.5 * SIZELX
          End If
          ypos = -0.1 * SIZELY
          lbl = CStr(DT(2))
          Call Angtx(outG, xpos, ypos, lbl, 1)
        ElseIf (DT(3) = 24 And DT(4) = 0 And DT(2) Mod DD = 0) Then
          fract = 0.25
          If i < npts Then Call TmTics(outG, xp, YLen, SIZELY, fract, ALen, grid)
          If (DD > 1) Then
            'print day just left of tics
            lbl = CStr(DT(2))
            xpos = xp - outG.TextWidth(lbl)
            ypos = -0.1 * SIZELY
            Call Angtx(outG, xpos, ypos, lbl, 1)
          End If
        End If
      Next i

      'write title on axis.
      lbl = "TIME, IN DAYS"
      xpos = (XLen - outG.TextWidth(lbl)) / 2#
      ypos = YPOSMX * SIZELY
      Call Angtx(outG, xpos, ypos, lbl, 1)
    Else
      'can fit y/m/d under each day, usually when 1 day is greater then 1 inches.
      'compute frequency of minute tics if any
      INTV = 999
      INTH = 1
      If (MPIN > 720) Then
        INTH = 12
      ElseIf (MPIN > 360) Then
        INTH = 6
      ElseIf (MPIN > 180) Then
        INTH = 4
      ElseIf (MPIN > 90) Then
        INTH = 2
      ElseIf (MPIN > 40) Then
        INTH = 1
      ElseIf (MPIN > 30) Then
        'use minute tics
        INTV = 30
      ElseIf (MPIN > 20) Then
        INTV = 15
      ElseIf (MPIN > 10) Then
        INTV = 10
      ElseIf (MPIN > 5) Then
        INTV = 5
      ElseIf (MPIN > 3) Then
        INTV = 2
      Else
        INTV = 1
      End If

      'put hour label at first tic
      xp = 0#
      If (DT(4) = 0 And DT(3) Mod INTH = 0) Then
        'print hour
        fract = 0.5
        Call TmTics(outG, xp, YLen, SIZELY, fract, ALen, grid)
        xpos = xp - 1.6 * SIZELX
        ypos = -0.1 * SIZELY
        Call IcHour(DT(3) * 100, lbl)
        Call Angtx(outG, xpos, ypos, lbl, 1)
      ElseIf (DT(4) Mod INTV = 0 And INTV <> 999) Then
        'print minute
        fract = 0.25
        Call TmTics(outG, xp, YLen, SIZELY, fract, ALen, grid)
        xpos = xp - 1.6 * SIZELX
        ypos = -0.1 * SIZELY
        j = 100 * DT(3) + DT(4)
        Call IcHour(j, lbl)
        Call Angtx(outG, xpos, ypos, lbl, 1)
      End If

      For i = 1 To npts Step 1
        'increment time.
        UB = 1
        Call F90_DATNXT(tsmin, UB, DT(0))
        xp = CSng(i) * xinc
        CUMM = CUMM + 1
        If ((DT(3) = 24 And DT(4) = 0) Or (i = npts)) Then
          'label the day
          SPACE = CSng(CUMM) * xinc
          IOPT = 2
          Call DatStrg(outG, DT, IOPT, SPACE, SIZELX, l, Dlbl)
          If (SPACE > CSng(l) * SIZELX) Then
            'enough space for printing the month,day,year
            xpos = xp - 0.5 * SPACE - 0.5 * outG.TextWidth(Dlbl)
            ypos = -0.8 * SIZELY
            Call Angtx(outG, xpos, ypos, Dlbl, 1)
          End If
          If (i > 1 And i < npts) Then
            'put day mark if not beginning or end
            ypos = -0.8 * SIZELY
            yoff = -0.7 * SIZELY
            outG.Line (xp, ypos)-Step(0, yoff)
          End If
          CUMM = 0
        End If
        If (DT(4) = 0 And DT(3) Mod INTH = 0) Then
          'print hour.
          fract = 0.5
          If i < npts Then Call TmTics(outG, xp, YLen, SIZELY, fract, ALen, grid)
          xpos = xp - 1.6 * SIZELX
          ypos = -0.1 * SIZELY
          Call IcHour(DT(3) * 100, lbl)
          Call Angtx(outG, xpos, ypos, lbl, 1)
        ElseIf (DT(4) Mod INTV = 0 And INTV <> 999) Then
          'print minute
          fract = 0.25
          If i < npts Then Call TmTics(outG, xp, YLen, SIZELY, fract, ALen, grid)
          xpos = xp - 1.6 * SIZELX
          ypos = -0.1 * SIZELY
          j = 100 * DT(3) + DT(4)
          Call IcHour(j, lbl)
          Call Angtx(outG, xpos, ypos, lbl, 1)
        End If
      Next i
      'write title on axis.
      lbl = "TIME, IN HOURS"
      xpos = (XLen - outG.TextWidth(lbl)) / 2#
      ypos = -1.5 * SIZELY
      Call Angtx(outG, xpos, ypos, lbl, 1)
    End If

    BOTOM = ypos

End Sub

Sub DatStrg(outG As Object, DT&(), IOPT&, SPACE!, SizeL!, l!, DSTR$)
'  takes a date as a 6 integer array and produces
'   a character string with the month abreviated to 4 characters
'   and a comma between the day and year.
'   DT     - date string (yr,mo,dy,hr,mn,sc)
'   IOPT   - output option
'            1 - year and month
'            2 - year, month and day
'   SPACE  - available space on plot, in inches
'   SIZEL  - height of lettering, in inches
'   l      - size of output character string
'   DSTR   - output character string containing date

    Static MONTH$(12), mon$(12), m$(12), initfg&

    If initfg = 0 Then
      Call MonthNameInit(initfg, MONTH, mon, m)
    End If

    If (IOPT = 1) Then ' year and month
      DSTR = MONTH(DT(1)) & " " & CStr(DT(0))
      If (outG.TextWidth(DSTR) > SPACE) Then
        DSTR = mon(DT(1)) & " " & CStr(DT(0))
        If (outG.TextWidth(DSTR) > SPACE) Then
          DSTR = mon(DT(1))
          If (outG.TextWidth(DSTR) > SPACE) Then
            DSTR = m(DT(1))
          End If
        End If
      End If
    Else ' year month day
      DSTR = MONTH(DT(1)) & " " & CStr(DT(2)) & ", " & CStr(DT(0))
      If (outG.TextWidth(DSTR) > SPACE) Then
        DSTR = mon(DT(1)) & " " & CStr(DT(2)) & ", " & CStr(DT(0))
        If (outG.TextWidth(DSTR) > SPACE) Then
          DSTR = mon(DT(1)) & " " & CStr(DT(2))
          'If (outG.TextWidth(DSTR) > SPACE) Then
          '  DSTR = CStr(DT(2))
          'End If
        End If
      End If
    End If
    l = outG.TextWidth(DSTR)
End Sub

Sub IcHour(DT&, CHOUR$)
'  converts an integer number for hour-minutes
'  into a character string with blanks as zeros.
'    DT     - hour-minutes
'    CHOUR  - character string of hours-minutes with zeros filled
'             in the blanks.
   Dim hr&, mi&, cmi$
   mi = DT Mod 100
   If (mi < 10) Then
     cmi = "0" & CStr(mi)
   Else
     cmi = CStr(mi)
   End If
   hr = (DT - mi) / 100
   CHOUR = CStr(hr) & ":" & CStr(cmi)
End Sub


Private Sub TmTics(outG As Object, xp!, YLen!, SizeL!, fract!, ALen!, grid&)
    'places tic marks for time on botton and top
    'of main plot and on auxilary plot if used.
    'XP     - horizontal location on plot, in world coordinates
    'YLEN   - length of Y-axis, in world coordinates
    'SIZEL  - height of lettering, in world coordinates
    'FRACT  - tic size, as a fraction of SIZEL
    'ALEN   - auxilary plot axis length
    '          0 - no auxilary plot
    '         >0 - length of auxilary axis
    Dim TLen!, base!

    TLen = SizeL * fract
    
    If grid = 1 Then
      outG.DrawStyle = 2
      outG.Line (xp, 0)-(xp, YLen), Grid_Color
      outG.DrawStyle = 0
    End If
    
    outG.Line (xp, 0)-Step(0, TLen)
    'top line main plot
    outG.Line (xp, YLen)-Step(0, -TLen)
    'tics for auxiliary plot
    If (ALen > 0.0001) Then
      base = YLen + Abs(outG.TextHeight(""))
      
      If grid = 1 Then
        outG.DrawStyle = 2
        outG.Line (xp, base)-(xp, base + ALen), Grid_Color
        outG.DrawStyle = 0
      End If
      
      'bottom of aux plot
      outG.Line (xp, base)-Step(0, TLen * 0.6)
      'top of aux plot
      outG.Line (xp, base + ALen)-Step(0, -TLen * 0.6)
    
    End If
End Sub

Public Sub LxAxis(outG As Object, XLen!, YLen!, LabelX$, xmin!, xmax!, TLen!, Gridx&)
    'put tics and numbers on an log x-axis
    Dim i!, j!, xp!, xpp!, posinc!

    posinc = XLen / (xmax - xmin)
    xp = 0
    For i = xmin To xmax Step 1
      Call GridTic(outG, 4, xp, XLen, YLen, Gridx, TLen, CStr(10 ^ i))
      If i < xmax Then
        'might want intermediate tics
        If xmax - xmin < 5 Then
          'put small intermediate tics in between cycles
          For j = 2 To 9
            xpp = xp + Log10(CDbl(j)) * posinc
            Call GridTic(outG, 4, xpp, XLen, YLen, Gridx, TLen / 2, "")
          Next j
        End If
      End If
      xp = xp + posinc
    Next i
    'label the X axis
    Call LabX(outG, XLen, LabelX)
End Sub

Private Sub LabX(outG As Object, XLen!, LabelX$)
  'label x axis
  Dim xpos!, ypos!, lwid!, lhei!
  'where the label is printed
  lwid = outG.TextWidth(LabelX)
  xpos = (XLen - lwid) / 2#
  ypos = 1.25 * outG.TextHeight(LabelX)
  Call Angtx(outG, xpos, ypos, LabelX, 1)
End Sub

Public Sub ArAxis(outG As Object, XLen!, YLen!, LabelY$, ymin!, ymax!, lftrt&, Aux!, TLen!, tics&, Gridy&)
    'put tics and numbers on an arithmetic y-axis
    Dim i&, yp!, xpmin!, yv!, posinc!, valinc!
    Dim ndpl&, lbl$

    If tics < 1 Then tics = 1
    posinc = YLen / tics
    valinc = (ymax - ymin) / tics
    Call DecReq(ymin, valinc, tics, ndpl)
    yp = 0
    yv = ymin
    xpmin = 0
    For i = 0 To tics
      If ndpl > 0 Then 'decimals needed
        Call F90_DECCHX(yv, 12, 4, ndpl, lbl)
        lbl = Trim(lbl) 'trim any blanks
        Call DecTrim(lbl) 'trim trailing "0"s
      Else
        lbl = CStr(yv)
      End If
      Call GridTic(outG, lftrt, yp, XLen, YLen, Gridy, TLen, lbl)
      If Abs(outG.TextWidth(lbl)) > Abs(xpmin) Then
        xpmin = -Abs(outG.TextWidth(lbl))
      End If
      yp = yp + posinc
      yv = yv + valinc
    Next i
    'label the y axis
    Call LabY(outG, LabelY, Aux, xpmin, lftrt, XLen, YLen)

End Sub

Public Sub LgAxis(outG As Object, XLen!, YLen!, LabelY$, ymin!, ymax!, lftrt&, Aux!, TLen!, Gridy&)
    'put tics and numbers on a logarithmic y-axis
    Dim i!, j&, yp!, ypp!, posinc!, xpmin!

    posinc = YLen / (ymax - ymin)
    yp = 0
    xpmin = 0
    For i = ymin To ymax
      Call GridTic(outG, lftrt, yp, XLen, YLen, Gridy, TLen, CStr(10 ^ i))
      If Abs(outG.TextWidth(CStr(10 ^ i))) > Abs(xpmin) Then
        xpmin = -Abs(outG.TextWidth(CStr(10 ^ i)))
      End If
      If i < ymax Then
        'might want intermediate tics
        If ymax - ymin < 5 Then
          'put small intermediate tics in between cycles
          For j = 2 To 9
            ypp = yp + Log10(CDbl(j)) * posinc
            Call GridTic(outG, lftrt, ypp, XLen, YLen, Gridy, TLen / 2, "")
          Next j
        End If
      End If
      yp = yp + posinc
    Next i
    'label the y axis
    Call LabY(outG, LabelY, Aux, xpmin, lftrt, XLen, YLen)

End Sub

Private Sub LabY(outG As Object, LabelY$, Aux!, xpmin!, lftrt&, XLen!, YLen!)
    'label Y axis
    Dim lwid!, lhgt!, tleft!, twidth!, ypos!

    'temporarily set horizontal scale to
    'vertical to determine vertical text extent
    'tleft = outG.ScaleLeft
    'twidth = outG.ScaleWidth
    'outG.ScaleLeft = 0
    'outG.ScaleWidth = Abs(outG.ScaleHeight)
    'multiply text extent by width/height ratio
    lwid = outG.TextWidth(LabelY)
    lwid = lwid * outG.Width / outG.ScaleWidth
    lwid = Abs(lwid * outG.ScaleHeight / outG.Height)
    'lwid = Abs(outG.ScaleWidth / outG.ScaleHeight * outG.TextWidth(LabelY))
    'reset horizontal scale values
    'outG.ScaleLeft = tleft
    'outG.ScaleWidth = twidth
    ypos = (YLen - lwid) / 2#
    outG.CurrentY = ypos
    
    If lftrt = 2 Then  'labeling right axis
      outG.CurrentX = XLen + Abs(xpmin) + 0.25 * Abs(outG.TextHeight(""))
    ElseIf Aux < 0 Then
      'aux axis in use, align aux and main labels
      outG.CurrentX = Aux
    Else  'just main left
      outG.CurrentX = xpmin - 1.25 * Abs(outG.TextHeight(""))
    End If
    If outG.CurrentX < outG.ScaleLeft Then
      outG.CurrentX = outG.ScaleLeft
    End If
    'draw the label
    frmG.GraphPrint LabelY, 90


End Sub

Private Sub GridTic(outG As Object, xyfg&, p!, XLen!, YLen!, grid&, TLen!, lbl$)
    'put tics and optional grid on either axis
    Dim clr&, dfg&

    dfg = 0
    clr = outG.ForeColor

    If xyfg = 4 Then
      If Abs(p) < 0.000001 Or Abs(p - XLen) < 0.000001 Then
        dfg = 1
      ElseIf p > 0 And p < XLen Then
        If grid = 1 Then
          dfg = 2
        Else
          dfg = 3
        End If
      End If
    Else
      If Abs(p) < 0.000001 Or Abs(p - YLen) < 0.000001 Then
        dfg = 1
      ElseIf p > 0 And p < YLen Then
        If grid = 1 Then
          dfg = 2
        Else
          dfg = 3
        End If
      End If
    End If

    If dfg = 2 Then
      'grey grid
      clr = Grid_Color
      'dotted lines
      outG.DrawStyle = 2
    End If
  
    If grid <> -1 Then
      If dfg = 1 Or dfg = 2 Then
        If xyfg = 4 Then ' x
          outG.Line (p, 0)-(p, YLen), clr
        Else ' y
          outG.Line (0, p)-(XLen, p), clr
        End If
        outG.DrawStyle = 0
      End If
    Else
      'bottom tics only for boxplots
      outG.Line (p, 0)-Step(0, TLen)
    End If

    If dfg > 1 And grid <> -1 Then
      'not at either axis - need tics, not boxplot
      If xyfg = 4 Then
        'bottom & top tics
        outG.Line (p, 0)-Step(0, TLen)
        outG.Line (p, YLen)-Step(0, -TLen)
      ElseIf xyfg = 1 Then 'left y tics
        outG.Line (0, p)-Step(TLen, 0)
      ElseIf xyfg = 2 Then 'right y tics
        outG.Line (XLen, p)-Step(-TLen, 0)
      End If
    End If

    If Len(lbl) > 0 And dfg <> 0 Then
      If xyfg = 4 Then
        outG.CurrentX = p - (outG.TextWidth(lbl) / 2)
        outG.CurrentY = 0.25 * outG.TextHeight(lbl)
      Else
        outG.CurrentY = p - (outG.TextHeight(lbl) / 2)
        If xyfg = 1 Or xyfg = 3 Then 'left y tic labels
          'use 1.1 to move labels away from axis a bit
          outG.CurrentX = -1.1 * outG.TextWidth(lbl)
        ElseIf xyfg = 2 Then 'right y tic labels
          outG.CurrentX = 1.01 * XLen
        End If
      End If
      frmG.GraphPrint lbl
    End If
End Sub

Public Sub DecReq(vmin!, vinc!, ninc&, ndpl&)

    'determine max number of decimals needed
    'when displaying a series of numbers
    Dim i&, lVal!, lndpl&, lstr$

    ndpl = 0
    lVal = vmin
    For i = 0 To ninc
      Call F90_DECCHX(lVal, 12, 4, 4, lstr)
      lstr = Trim(lstr)
      lndpl = 0
      Call DecTrim(lstr) 'remove "0"s after decimal
      While Mid(lstr, Len(lstr)) <> "."
        lndpl = lndpl + 1
        lstr = Left(lstr, Len(lstr) - 1)
      Wend
      If lndpl > ndpl Then ndpl = lndpl
      lVal = lVal + vinc
    Next i

End Sub

Public Sub DecTrim(NStr$)

    If InStr(NStr, ".") > 0 Then
      While Right(NStr, 1) = "0"
        NStr = Left(NStr, Len(NStr) - 1)
      Wend
    End If

End Sub
