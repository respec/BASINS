Attribute VB_Name = "GraphList"
Option Explicit
'Copyright 2002 AQUA TERRA Consultants - Royalty-free use permitted under open source license

Public Ncrv&, nvar&
Dim xaxtyp&, YAxTyp&, YRAxTyp&, auxlen!
Dim XZLineFg&, YZLineFg&
Dim xlab$, ylab$, yrlab$, alab$
Dim tmin!, tmax!
Dim Ctitl$, capt$, clab$(), vlab$()
Dim Cic As Object
Dim pmin!(1 To 4), pMax!(1 To 4), NTic&(1 To 4)
Dim vmin!(), vmax!(), whch&(), Tran&()
Dim ctyp&(), lTyp&(), lthk&(), styp&(), clr&()
Dim ltu&(), lts&(), ldtr&(), dtype&()
Dim lsdate&(5), ledate&(5), lConInt As Boolean
Dim XLegLoc!, YLegLoc!
Dim L45 As Boolean
Dim ACoeff!, BCoeff!, RSquare!

'these came from genplot
Type vardata
  Vals() As Single
  Trans As Long '0-arithmetic, 1-log
  Min As Single
  Max As Single
End Type

Type labeldata
  nLables As Long
  Labels() As String
  Position As Long   '0-Y axis, 1-Xaxis
  Orientation As Long
End Type

Type xyplotdata
  NVal As Long
  Var(1) As vardata '0-Y, 1-X
  DataLabels As labeldata
End Type
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
' o is graph or list
Public Sub GLDoXY(O As Object, ByVal opt&, XYData() As xyplotdata, initfg&)

    Dim i&, j&, pos&, indx&, rtmp#(1), lstr$
    Dim vbuf#()

    YAxTyp = XYData(0).Var(0).Trans
    xaxtyp = XYData(0).Var(1).Trans
    
    If initfg <> 1 Then ' even if the caller didn't ask for it, we may want to initialize graph
      Call O.GetScale(pmin, pMax, NTic)
      If pmin(4) = pMax(4) Then initfg = 1
      If pmin(1) = pMax(1) Then initfg = 1
    End If
    
    If initfg = 1 Then
      Call O.init
      Call O.SetCurDrvDir("", CurDir)
    End If
    pmin(1) = 1E+30
    pMax(1) = -1E+30
    NTic(4) = 5 'only use 5 tics on x axis
    pmin(4) = 1E+30
    pMax(4) = -1E+30
    indx = 0
    pos = 1
    For j = 0 To Ncrv - 1
      'set the y data
      ReDim vbuf(XYData(j).NVal)
      For i = 0 To XYData(j).NVal - 1
        vbuf(i) = XYData(j).Var(0).Vals(i)
      Next i
      Call O.SetData(indx, pos, XYData(j).NVal, vbuf(), i)
      If XYData(j).DataLabels.nLables > 0 Then
        Call O.SetDataLabels(0, pos, XYData(j).NVal, XYData(j).DataLabels.Labels, i)
      End If
      pos = pos + XYData(j).NVal
      
      vmin(indx) = XYData(j).Var(0).Min
      vmax(indx) = XYData(j).Var(0).Max
      whch(indx) = 1
      Tran(indx) = XYData(j).Var(0).Trans
      If pMax(1) < vmax(indx) Then
        pMax(1) = vmax(indx)
      End If
      If pmin(1) > vmin(indx) Then
        pmin(1) = vmin(indx)
      End If
      
      'set the x data
      indx = indx + 1
      For i = 0 To XYData(j).NVal - 1
        vbuf(i) = XYData(j).Var(1).Vals(i)
      Next i
      Call O.SetData(indx, pos, XYData(j).NVal, vbuf, i)
      
      vmin(indx) = XYData(j).Var(1).Min
      vmax(indx) = XYData(j).Var(1).Max
      whch(indx) = 4
      Tran(indx) = XYData(j).Var(1).Trans
      If pMax(4) < vmax(indx) Then
        pMax(4) = vmax(indx)
      End If
      If pmin(4) > vmin(indx) Then
        pmin(4) = vmin(indx)
      End If
      Call O.SetVars(j, indx - 1, indx)
      
      indx = indx + 1
      pos = pos + XYData(j).NVal
      ctyp(j) = 6 'xy type curve
      styp(j) = j + 1 'set symbol type
      lTyp(j) = 1 'changed from gralis, want line
    Next j
    If L45 Then '45 degree/regression lines being drawn
      ReDim Preserve ctyp(Ncrv + 1)
      ReDim Preserve styp(Ncrv + 1)
      ReDim Preserve lTyp(Ncrv + 1)
      ReDim Preserve lthk(Ncrv + 1)
      ReDim Preserve clr(Ncrv + 1)
      ReDim Preserve clab(Ncrv + 1)
      ctyp(Ncrv) = 6
      ctyp(Ncrv + 1) = 6
      styp(Ncrv) = 0
      styp(Ncrv + 1) = 0
      lTyp(Ncrv) = 1
      lTyp(Ncrv + 1) = 1
      lthk(Ncrv) = 1
      lthk(Ncrv + 1) = 1
      clr(Ncrv) = 0
      clr(Ncrv + 1) = clr(0)
      'set y and x max to be the same
      If pMax(1) > pMax(4) Then
        pMax(4) = pMax(1)
      Else
        pMax(1) = pMax(4)
      End If
    End If
    'set Y-axis scale
    tmin = pmin(1)
    tmax = pMax(1)
    Call Scalit((YAxTyp), tmin, tmax, pmin(1), pMax(1))
    If Abs(pmin(1)) < 0.001 Then pmin(1) = 0
    'set X-axis scale
    tmin = pmin(4)
    tmax = pMax(4)
    Call Scalit((YAxTyp), tmin, tmax, pmin(4), pMax(4))
    If pmin(4) = pMax(4) Then
      If tmin <= tmax Then
        pmin(4) = pmin(4) - 1: pMax(4) = pMax(4) + 1
      Else
        pmin(4) = pmin(4) + 1: pMax(4) = pMax(4) - 1
      End If
    End If
    If L45 Then
      'set data points for the 45 degree line
      ReDim Preserve vmin(nvar + 4)
      ReDim Preserve vmax(nvar + 4)
      ReDim Preserve whch(nvar + 4)
      ReDim Preserve Tran(nvar + 4)
      ReDim Preserve vlab(nvar + 4)
      '45 degree line, y points first
      rtmp(0) = 0
      rtmp(1) = pMax(1)
      Call O.SetData(indx, pos, 2, rtmp, i)
      whch(indx) = 1
      Tran(indx) = YAxTyp
      'now x points
      rtmp(0) = 0
      rtmp(1) = pMax(4)
      indx = indx + 1
      pos = pos + 2
      Call O.SetData(indx, pos, 2, rtmp, i)
      whch(indx) = 4
      Tran(indx) = xaxtyp
      Call O.SetVars(Ncrv, indx - 1, indx)
      Ncrv = Ncrv + 1
      nvar = nvar + 2
      'regression line, y points first
      rtmp(0) = (vmin(1) * ACoeff) + BCoeff
      rtmp(1) = (vmax(1) * ACoeff) + BCoeff
      indx = indx + 1
      pos = pos + 2
      Call O.SetData(indx, pos, 2, rtmp, i)
      whch(indx) = 1
      Tran(indx) = YAxTyp
      'now x points
      rtmp(0) = vmin(1)
      rtmp(1) = vmax(1)
      indx = indx + 1
      pos = pos + 2
      Call O.SetData(indx, pos, 2, rtmp, i)
      whch(indx) = 4
      Tran(indx) = xaxtyp
      Call O.SetVars(Ncrv, indx - 1, indx)
      Ncrv = Ncrv + 1
      nvar = nvar + 2
    End If
    Call O.SetNumVars(Ncrv, nvar)
    If opt = 1 Then
      Call O.SetVarInfo(vmin, vmax, whch, Tran, vlab)
      If initfg = 1 Then
        Call O.SetScale(pmin, pMax, NTic)
        Call O.SetAxesInfo(xaxtyp, YAxTyp, YRAxTyp, auxlen, xlab, ylab, yrlab, alab)
        Call O.SetCurveInfo(ctyp, lTyp, lthk, styp, clr, clab)
        Call O.SetLegLoc(XLegLoc, YLegLoc)
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
        Call O.SetAddText(0.05, 0.9, lstr)
      End If
      Call O.SetTitles(Ctitl, capt)
      If IsNumeric(Cic) Then 'update plot form's icon
        Call O.SetIcon(Cic)
      End If
      If XZLineFg = 1 Or YZLineFg = 1 Then
        'draw zero line(s) on plot
        Call O.SetConstLines(YZLineFg, 0, XZLineFg, 0)
      End If
    ElseIf opt = 2 Then
      For j = 0 To Ncrv - 1
        Call O.SetFldPrms(j, 8, 4, 1)
      Next j
      Call O.setsums(CLng(1), ldtr, ltu, CLng(9))
      For j = 0 To Ncrv - 1
        Call O.SetLabel(j, clab(j))
      Next j
      Call O.SetTitles(Ctitl, "GenScn List")
    End If
    If initfg = 1 Then 'new graph
      Call O.ShowIt(True)
    ElseIf opt = 1 Then 'update existing graph
      Call O.ReDrawGraph(0)
    End If

End Sub

Public Sub GLInit(opt&, O As Object, incrv&, invar&)

    Dim j%
    'Static g As New HGrph
    'Static l As New HList

    Ncrv = incrv
    nvar = invar
    If Ncrv > 18 Then 'max from hgraph
      If nvar >= 2 * Ncrv Then '2 vars/curve
        nvar = nvar - (2 * (Ncrv - 18))
      Else
        nvar = nvar - (Ncrv - 18)
      End If
      Ncrv = 18
    End If
    If opt = 1 Then
      Set O = New ATCoGraph
    Else
      Set O = New ATCoList
    End If

    ReDim ltu(Ncrv)
    ReDim lts(Ncrv)
    ReDim ldtr(Ncrv)
    ReDim dtype(Ncrv)
    ReDim vmin(nvar)
    ReDim vmax(nvar)
    ReDim whch(nvar)
    ReDim Tran(nvar)
    ReDim vlab(nvar)
    ReDim clab(Ncrv)
    ReDim ctyp(Ncrv)
    ReDim lTyp(Ncrv)
    ReDim lthk(Ncrv)
    ReDim styp(Ncrv)
    ReDim clr(Ncrv)
    For j = 0 To Ncrv - 1
      dtype(j) = 1   'default to mean values
      ctyp(j) = 1    'regular time plot
      lTyp(j) = 1    'solid line
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
    xaxtyp = 0
    YAxTyp = 1
    YRAxTyp = 0
    auxlen = 0#
    XZLineFg = 0
    YZLineFg = 0
    XLegLoc = 0
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

Public Sub GLSetEditable(O As Object, tsIndex&)
  Call O.SetEditable(tsIndex, True)
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

    ReDim clab(Ncrv)
    For i = 0 To Ncrv - 1
      clab(i) = leglab(i)
    Next i

End Sub
Public Sub GLLegLoc(XLPos!, YLPos!)

    XLegLoc = XLPos
    YLegLoc = YLPos

End Sub
Public Sub GLVarLab(ivlab() As String)

    Dim i%

    ReDim vlab(nvar - 1)
    For i = 0 To nvar - 1
      vlab(i) = ivlab(i)
    Next i

End Sub
Public Sub GLZLine(YFg&, XFg&)

    YZLineFg = YFg
    XZLineFg = XFg

End Sub
Public Sub GLLSpec(icrv&, lt&, lw&, ls&, lc&)

    If lt >= 0 Then
      lTyp(icrv) = lt   'line type
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
