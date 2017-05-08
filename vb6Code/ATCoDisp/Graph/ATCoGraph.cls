VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "ATCoGraph"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit
'Copyright 2001 by AQUA TERRA Consultants

'Public Event ClickDataPoint(GraphCaption$, CurveNum&, PtNum&)

'Public Sub RaiseClickDataPoint(GraphCaption$, CurveNum&, PtNum&)
'  RaiseEvent ClickDataPoint(GraphCaption, CurveNum, PtNum)
'End Sub

Public Property Set Launch(newLaunch As AtCoLaunch)
  'Set pLaunch = newLaunch
End Property
Public Property Set IPC(newIPC As ATCoIPC)
  Set pIPC = newIPC
End Property

'Add a straight line to the graph
Public Sub AddLine(WchAx&, LType&, LThck&, SType&, Color&, LegLbl$, Value$)
  frmG.AddLine WchAx, LType, LThck, SType, Color, LegLbl, Value
End Sub

Public Property Let HelpFileName(FileName$)
  GraphHelpFileName = FileName
  App.HelpFile = FileName
End Property

Public Property Get GraphOpen() As Boolean
  GraphOpen = frmG.Visible
End Property

'Public Function Count&()
'Attribute Count.VB_Description = "returns count of HGraphs opened by calling application. "

'    Dim i&, j&
'    i = Forms.Count
'    Count = 0
'    For j = 0 To i - 1
'      If Forms(j).Caption <> "Graph" Then
'        Count = Count + 1
'      End If
'    Next j
'
'End Function

Public Sub GetLegLoc(XLPos!, YLPos!)
  DbgMsg "GetLegLoc: (" & XLPos & ", " & YLPos & ")"
  Call frmG.GetLegLoc(XLPos, YLPos)
End Sub

Public Sub SetLegLoc(XLPos!, YLPos!)
  DbgMsg "SetLegLoc: (" & XLPos & ", " & YLPos & ")"
  Call frmG.SetLegLoc(XLPos, YLPos)
End Sub

Public Sub SetAddText(XTPos!, YTPos!, AddedText$)
  DbgMsg "SetAddText: " & AddedText & " (" & XTPos & ", " & YTPos & ") "
  Call frmG.SetAddText(XTPos, YTPos, AddedText)
End Sub

Public Sub SetGrid(Gridx&, lGridy&, rGridy&)
'Attribute SetGrid.VB_Description = "sets grid options for a HGraph"
  DbgMsg "SetGrid: (" & Gridx & ", " & lGridy & ", " & rGridy & ") "
  Call frmG.SetGrid(Gridx, lGridy, rGridy)

End Sub

Public Sub init()
'Attribute Init.VB_Description = "initializes a new HGraph"
  DbgMsg "init"
  Set curG = New frmG
  Set frmG = curG
  NumGra = NumGra + 1
  curG.Top = ((NumGra + 1) * 300) Mod (Screen.Height - curG.Height)
  curG.Left = ((NumGra + 1) * 200) Mod (Screen.Width - curG.Width)
  'Set curG.atcgr = Me

  curG.SetLegLoc 0, 1
  curG.SetAddText 0, 0.5, ""
  'Call frmG.init(Me)
End Sub

Public Sub ReDrawGraph(outflag&)
  DbgMsg "ReDrawGraph"
  Call frmG.ReDrawGraph(outflag)
End Sub

Public Sub SetData(inum&, ipos&, nv&, arra#(), retcod&)
'Attribute SetData.VB_Description = "puts data values to graph into a HGraph"
  DbgMsg "SetData: inum=" & inum & ", ipos=" & ipos & "nv=" & nv
  Call frmG.SetData(inum, ipos, nv, arra, retcod)

End Sub

Public Sub SetDataLabels(scrpos&, ipos&, nv&, arra$(), retcod&)
  DbgMsg "SetDataLabels: scrpos=" & scrpos & ", ipos=" & ipos & "nv=" & nv
  Call frmG.SetDataLabels(scrpos, ipos, nv, arra, retcod)
End Sub

Public Sub SetFill(icrv&, ifill&)
  DbgMsg "SetFill: icrv=" & icrv & ", ifill=" & ifill
  Call frmG.SetFill(icrv, ifill)
End Sub

Public Sub GetTime(gtstep&(), gtunit&(), gsdate&(), gedate&(), gdtype&())
  DbgMsg "GetTime"
  Call frmG.GetTime(gtstep, gtunit, gsdate, gedate, gdtype)
End Sub

Public Sub SetTime(gtstep&(), gtunit&(), gsdate&(), gedate&(), gdtype&())
  DbgMsg "SetTime"
  Call frmG.SetTime(gtstep, gtunit, gsdate, gedate, gdtype)
End Sub

'Attribute GetAxesInfo.VB_Description = "sets axis types, labels, titles and captions for a HGraph"
Public Sub GetAxesInfo(xtype&, ytype&, yrtype&, auxlen!, xlab$, ylab$, yrlab$, alab$)
  Dim XStyp&, Xint& 'dummy for now, only used with frmGEdit
  DbgMsg "GetAxesInfo: xtype=" & xtype & ", ytype=" & ytype & "yrtype=" & yrtype & "auxlen=" & auxlen & "xlab=" & xlab & "ylab=" & ylab & "yrlab=" & yrlab & "alab=" & alab
  Call frmG.GetAxesInfo(xtype, XStyp, Xint, ytype, yrtype, auxlen, xlab, ylab, yrlab, alab)
End Sub
Public Sub SetAxesInfo(xtype&, ytype&, yrtype&, auxlen!, xlab$, ylab$, yrlab$, alab$)
  DbgMsg "SetAxesInfo: xtype=" & xtype & ", ytype=" & ytype & "yrtype=" & yrtype & "auxlen=" & auxlen & "xlab=" & xlab & "ylab=" & ylab & "yrlab=" & yrlab & "alab=" & alab
  Dim XStyp&, Xint& 'dummy for now, only used with frmGEdit
  XStyp = 0
  Xint = 0
  Call frmG.SetAxesInfo(xtype, XStyp, Xint, ytype, yrtype, auxlen, xlab, ylab, yrlab, alab)
End Sub

Public Sub SetConstLines(yconfg&, yconval!, xconfg&, xconval!)
  DbgMsg "SetConstLines: yconfg=" & yconfg & ", yconval=" & yconval & "xconfg=" & xconfg & "xconval=" & xconval
  Call frmG.SetConstLines(yconfg, yconval, xconfg, xconval)
End Sub

Public Sub GetCurveInfo(crvtyp&(), LinTyp&(), linthk&(), symtyp&(), icolor&(), ilbl$())
'Attribute GetCurveInfo.VB_Description = "sets legend for a curve on a HGraph"
  DbgMsg "GetCurveInfo"
  Call frmG.GetCurveInfo(crvtyp, LinTyp, linthk, symtyp, icolor, ilbl)
End Sub

Public Sub SetCurveInfo(crvtyp&(), LinTyp&(), linthk&(), symtyp&(), icolor&(), ilbl$())
  DbgMsg "SetCurveInfo"
  Call frmG.SetCurveInfo(crvtyp, LinTyp, linthk, symtyp, icolor, ilbl)
End Sub

Public Sub GetNumVars(icrv&, ivar&) ', lnNonDataLines&)
'Attribute GetNumVars.VB_Description = "sets number of curves and variables for a HGraph"
  DbgMsg "GetNumVars: icrv=" & icrv & ", ivar=" & ivar
  'Call frmG.GetNumVars(icrv, ivar)
  icrv = frmG.NumCurves
  ivar = frmG.NumVars
End Sub

Public Sub SetNumVars(icrv&, ivar&)
  DbgMsg "SetNumVars: icrv=" & icrv & ", ivar=" & ivar
  'Call frmG.SetNumVars(icrv, ivar, 0)
  frmG.NumCurves = icrv
  frmG.NumVars = ivar
End Sub

Public Sub GetScale(plmn!(), plmx!(), NTICS&())
'Attribute GetScale.VB_Description = "sets scaling and tics for a HGraph"
  DbgMsg "GetScale"
  Call frmG.GetScale(plmn, plmx, NTICS)
End Sub

Public Sub SetScale(plmn!(), plmx!(), NTICS&())
  DbgMsg "SetScale"
  Call frmG.SetScale(plmn, plmx, NTICS)
End Sub

Public Sub GetTitles(titl$, capt$)
  DbgMsg "GetTitles: titl=" & titl & ", capt=" & capt
  Call frmG.GetTitles(titl, capt)
End Sub

Public Sub SetTitles(titl$, capt$)
  DbgMsg "SetTitles: titl=" & titl & ", capt=" & capt
  Call frmG.SetTitles(titl, capt)
End Sub
Public Sub SetIcon(ic As Object)
  DbgMsg "SetIcon: icon=" & ic
  Call frmG.SetIcon(ic)
End Sub

Public Sub GetVarInfo(vmin!(), vmax!(), which&(), tran&(), vlab$())
  DbgMsg "GetVarInfo"
  Call frmG.GetVarInfo(vmin, vmax, which, tran, vlab)
End Sub

Public Sub SetVarInfo(vmin!(), vmax!(), which&(), tran&(), vlab$())
  DbgMsg "SetVarInfo"
  Call frmG.SetVarInfo(vmin, vmax, which, tran, vlab)
End Sub

Public Sub GetVars(icrv&, y&, x&)
  DbgMsg "GetVars: icrv=" & icrv & ", Y=" & y & ", X=" & x
  Call frmG.GetVars(icrv, y, x)
End Sub

Public Sub SetVars(icrv&, y&, x&)
  DbgMsg "SetVars: icrv=" & icrv & ", Y=" & y & ", X=" & x
  Call frmG.SetVars(icrv, y, x)
End Sub

Public Sub SetCurDrvDir(V$, d$)
  DbgMsg "SetCurDrvDir: v=" & V & ", d=" & d
  Call frmG.SetCurDrvDir(V$, d$)
End Sub

Public Sub ShowIt(Optional modal As Boolean = False)
'Attribute ShowIt.VB_Description = "draws a new HGraph - called after all data has been set"
  DbgMsg "ShowIt " & modal
  frmG.ShowIt modal
End Sub

Public Function gausex(exprob!) As Single
  gausex = gausex(exprob)
  DbgMsg "gausex(" & exprob & ") = " & gausex
End Function

Private Sub DbgMsg(Msg$)
  If Not (pIPC Is Nothing) Then pIPC.dbg "ATCoGraph:" & Msg
End Sub

Public Property Get GraphForm() As Object
  Set GraphForm = frmG
End Property

