Attribute VB_Name = "modBatchGraph"
Option Explicit
Global TserFiles As ATCPlugInManager
Const MxCurve = 20
Const MxData = 20
Const MxVar = 20
Const MxAxis = 4
Global launch As AtCoLaunch

Sub main()
  Dim o As ATCograph
  Dim f&
  Dim s$, sTyp$
  Dim t As ATCclsTserFile, tc As Collection
  Dim d As ATCclsTserData
  Dim lMsgUnit&
  Dim lNum&, lPos&, lRet&, lLen&, lVals() As Double, lDates() As Double
  Dim lCrv&, lCrvtyp&(MxCurve), lLinTyp&(MxCurve), lLinThk&(MxCurve), lSymTyp&(MxCurve), lColor&(MxCurve), lLbl$(MxCurve)
  Dim lData&, lTStep&(MxData), lTUnit&(MxData), lSDate&(6), lEDate&(6), lDType&(MxData)
  Dim lVar&, lMin!(MxVar), lMax!(MxVar), lWhich&(MxVar), lTran&(MxVar), lLab$(MxVar)
  Dim lAxMin!(MxAxis), lAxMax!(MxAxis), lNTics&(MxAxis)
  Dim lXtype&, lYtype&, lYrtype&, lAuxlen!, lXlab$, lYlab$, lYrlab$, lAlab$
  Dim lTitle$, lCapt$
  Dim b&, i&
  Dim frm As Form
  
  Set launch = New AtCoLaunch
  launch.NconvertPath = "c:\vbexperimental\genscn\bin\nconvert.exe"
  
  initAllTser
  
  Set tc = Nothing
  Set tc = New Collection
  
  Set o = New ATCograph
  o.Init
  
  f = 1
  Open "c:\vbexperimental\batchgraph\data\batchgraph.txt" For Input As #f
    
  lData = 0
  lCrv = 0
  lNum = 0
  lPos = 0
  While Not EOF(f)
    Line Input #f, s
    s = Trim(s)
    sTyp = Trim(UCase(StrRetRem(s)))
    Select Case sTyp
    Case "AXIS"
      lXtype = StrRetRem(s)
      lYtype = StrRetRem(s)
      lYrtype = StrRetRem(s)
      lAuxlen = StrRetRem(s)
      lXlab = StrRetRem(s)
      lYlab = StrRetRem(s)
      lYrlab = StrRetRem(s)
      lAlab = s
    Case "CURVE"
      lCrvtyp(lCrv) = StrRetRem(s)
      lLinTyp(lCrv) = StrRetRem(s)
      lLinThk(lCrv) = StrRetRem(s)
      lSymTyp(lCrv) = StrRetRem(s)
      lColor(lCrv) = StrRetRem(s)
      lLbl(lCrv) = s
      lCrv = lCrv + 1
    Case "DATA"
      getData tc(tc.Count), s, lVals, lDates, lTStep(lCrv), lTUnit(lCrv), lDType(lCrv)
      lLen = UBound(lVals)
      o.SetData lNum, lPos, lLen, lVals, lRet
      lPos = lPos + lLen
      o.SetData lNum + 1, lPos, lLen, lDates, lRet
      o.SetVars lCrv, lNum, lNum + 1
      lWhich(lNum + 1) = 4 'dummy for time
      lLbl(lNum + 1) = "TIME DUMMY"
      lPos = lPos + lLen
      'lCrv = lCrv + 1
    Case "FILE"
      Set t = New ATCclsTserFile
      Set t = openTser(StrRetRem(s), s, lMsgUnit)
      tc.Add t
      'MsgBox t.DataCount
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
      frm.WindowState = vbMaximized
      launch.SavePictureAs frm.scrgraph, s
      Unload frm
    Case "SHOW"
      o.SetNumVars lCrv, lNum
      o.SetTime lTStep, lTUnit, lSDate, lEDate, lDType
      o.SetVarInfo lMin, lMax, lWhich, lTran, lLab
      o.SetScale lAxMin, lAxMax, lNTics
      o.SetAxesInfo lXtype, lYtype, lYrtype, lAuxlen, lXlab, lYlab, lYrlab, lAlab
      o.SetCurveInfo lCrvtyp, lLinTyp, lLinThk, lSymTyp, lColor, lLbl
      o.SetTitles lTitle, lCapt
      If Len(s) > 0 Then 'wait
        o.ShowIt True
      Else
        o.ShowIt
      End If
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
      lNum = lNum + 2 'very tied to DATA
    End Select
  Wend
  
End Sub

Sub initAllTser()
  Set TserFiles = Nothing
  Set TserFiles = New ATCData.ATCPlugInManager
  TserFiles.Load "ATCTSfile"
End Sub

Function openTser(typ$, nam$, msg&) As ATCclsTserFile
  Dim i&
  Dim o As Object ' ATCPlugIn
  Dim t As ATCclsTserFile
  
  i = TserFiles.AvailIndexByName("clsTSer" & typ)
  Call TserFiles.Create(i)
  Set o = TserFiles.CurrentActive.obj
  If typ = "WDM" Then
    o.msgUnit = msg
  End If
  Set t = o
  t.Filename = nam
  Set openTser = t
End Function

Sub getData(t As ATCclsTserFile, s$, vals() As Double, dates() As Double, TStep&, TUnit&, Dtype&)
  Dim typ$, i&, vdata As Variant
  Dim d As ATCclsTserData
  
  While Len(s) > 0
    typ = UCase(StrRetRem(s))
    Select Case typ
    Case "IND"
      i = s
      Set d = t.Data(i)
    Case "DSN"
      i = StrRetRem(s)
      For Each vdata In t.DataCollection
        If vdata.header.id = i Then
          Set d = vdata
          'MsgBox "found " & i
          Exit For
        End If
      Next vdata
    Case "TSTEP"
      TStep = StrRetRem(s)
    Case "TUNIT"
      TUnit = StrRetRem(s)
    Case "DTYPE"
      Dtype = StrRetRem(s)
    End Select
  Wend
  'might need a dummy here to aggregate
  ReDim vals(d.dates.Summary.NVALS)
  ReDim dates(d.dates.Summary.NVALS)
  For i = 1 To d.dates.Summary.NVALS
    vals(i) = d.Value(i)
    dates(i) = d.dates.Value(i)
  Next i
End Sub
