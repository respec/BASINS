Attribute VB_Name = "GenScnEntry"
Option Explicit
'Copyright 2000 by AQUA TERRA Consultants

Global Const AppName As String = "GenScn"

Private Const TserFileClassName = "ATCTSfile"
Private Const AnalysisClassName = "ATCAnalysis"

Global Batch As BatchGenScn

Global RunningVB As Boolean
Global IPC As ATCoCtl.ATCoIPC 'AtCoLaunch
Global DispFile As ATCoDispFile
Global Registry As ATCoRegistry
Global InMemFile As ATCclsTserFile
Global ExeName As String 'name of executable (updated if in devel envir, GenScnEntry, frmGenScn
Global ExePath As String
Global LogFileName As String
Global SWExeName As String
Global SDExeName As String

'Global ScenFile As String ' frmGenScn, frmGenScnActivate
Global CntDsn As Long
Global dsn As Long
Global CSDat(6) As Long, ComSDat(6) As Long
Global CEDat(6) As Long, ComEDat(6) As Long
Global ctunit&, CTStep&, CDTran&

Global NtsCol&

Global specSen$(), specLoc$(), specCon$()
Global scntSen%, scntLoc%, scntCon%
Global sSLC$(4)
Global gProfPlot As ATCoGraph
'Global dbg As ATCoCtl.ATCoDebug
'Global DebugMode As Boolean

Global TserFiles As ATCPlugInManager
'Global Analyses As ATCPlugInManager

Type WDMFile
    Name As String
    Unit As Long
End Type

Type GenScnProject
    EditFlg As Boolean
    StatusFileName As String
    StatusFilePath As String
    ScenName As CollString
    ScenDesc As CollString
    ScenFile As CollString
    ScenType As CollString
    LocnName As CollString
    LocnDesc As CollString
    LocnGeoCode As Collection
    ConsName As CollString
    ConsDesc As CollString
    MapName As String
    HSPFMsg As WDMFile
    WDMFiles As Collection ' of ATCclsTserFile
    ExternalForms As Collection
    PreferredUnits As FastCollection
    UnitsRequired As Boolean
End Type
Global p As GenScnProject
Global Tser As Collection
Global nts As Long

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

Global atEXE As ATCoCtl.ATCoEXE

Global MyRchAvail As Boolean

Sub FillReachDetails(ts As Collection)
  Dim lDSName$, lLen!, i&, j&, k&, l&, lInvrt!, tsCount&, rchMax& ', tsPadCount&
  Dim lts As ATCclsTserData, pTs As ATCclsTserData
  Dim prevFound As Boolean
  Dim StartTime As Single, OpenedStatus As Boolean, thisPercent&, lastPercent&, lastTime&, thisTime&
  Dim attrDef As ATCclsAttributeDefinition
  Set attrDef = New ATCclsAttributeDefinition
  attrDef.DataType = NONE
  StartTime = Timer
  If MyRchAvail Then
    tsCount = ts.Count
    rchMax = UBound(MyRch)
    'update reach info for timeseries, if available
    If rchMax > 0 Then
      'tsPadCount = 0
      For i = 1 To tsCount
        GoSub UpdateStatus
        
        Set lts = ts(i)
        With lts
          'If IsNumeric(.Header.loc) Then
          '  j = .Header.loc
            'If j <= rchMax Then
            '  Debug.Print .Header.loc, MyRch(j).Name
            'End If
          'End If
          j = 0
          While j <= rchMax
            If .Header.loc = MyRch(j).Name Then
              .AttribSet "BRANCH", MyRch(j).Branch, attrDef
              .AttribSet "NODE", MyRch(j).Name, attrDef
              .AttribSet "DSNODE", MyRch(j).DownName, attrDef
              .AttribSet "DISTANCE", CStr(MyRch(j).Length), attrDef
              lInvrt = .AttribNumeric("INVRT")
              If lInvrt > 0.001 Then 'have an invert from somewhere
                If (lInvrt - MyRch(j).DSInvert) > 0.001 Then
                  DbgMsg "Inconsistent inverts(DATUM) for " & lts(i).StaNam & _
                          " Timser: " & CStr(lts(i).invrt) & _
                          " MyRch: " & CStr(MyRch(j).DSInvert), _
                          2, "FillReachDetails", "E"
                End If
                If MyRch(j).DSInvert > 0.001 Then 'have one in myrch too, use it
                  .AttribSet "INVRT", CStr(MyRch(j).DSInvert), attrDef
                End If
              Else 'no invert so far, use myrch
                .AttribSet "INVRT", CStr(MyRch(j).DSInvert), attrDef
              End If
              j = rchMax
            End If
            j = j + 1
          Wend
        End With
      Next i
      'tsPadCount = tsCount
'      lastPercent = 0
'      For i = 1 To tsCount
'        GoSub UpdateStatus
'
'        prevFound = False
'        Set lTs = ts(i)
'        For j = i - 1 To 1 Step -1
'          Set pTs = ts(j)
'          If pTs.Header.loc = lTs.Header.loc Then
'            prevFound = True
'            lDSName = pTs.attrib("DSNODE")
'            llen = pTs.AttribNumeric("DISTANCE")
'            Exit For
'          End If
'        Next j
'
'        With lTs
'          If prevFound Then 'use prev reach
'            .AttribSet "DSNODE", lDSName, attrDef
'            .AttribSet "DISTANCE", CStr(llen), attrDef
'          Else
'            'determine lengths to next downstream timeseries node
'            llen = 0
'            lDSName = .attrib("DSNODE")
'            While lDSName <> ""
'              k = 0
'              While k <= rchMax
'                If MyRch(k).Name = lDSName Then
'                  'this is the downstream node
'                  llen = llen + MyRch(k).Length
'                  l = 1
'                  While l <= tsCount
'                    If ts(l).attrib("NODE") = lDSName Then
'                      'timeseries at this downstream node,
'                      'stop calculating distance for this node
'                      l = tsCount
'                      'update the downstream node and
'                      'distance to it for this timeseries
'                      .AttribSet "DSNODE", lDSName, attrDef
'                      .AttribSet "DISTANCE", CStr(llen), attrDef
'                      lDSName = ""
'                    End If
'                    l = l + 1
'                  Wend
'                  If lDSName <> "" Then
'                    'no timeseries at this downstream node,
'                    'find the next downstream node
'                    lDSName = MyRch(k).DownName
'                  End If
'                  k = rchMax + 1
'                End If
'                k = k + 1
'              Wend
'              If k = rchMax + 1 Then 'no downstream node found
'                lDSName = ""
'                'this means no downstream node for this timeseries
'                .AttribSet "DSNODE", "", attrDef
'              End If
'            Wend
'          End If
'        End With
'      Next i
    End If
  End If
ExitSub:
  If OpenedStatus Then IPC.SendMonitorMessage "(CLOSE Filling Reach Details)"
  Exit Sub
  
UpdateStatus:
  If OpenedStatus Then
'CheckStatus:
    'Select Case Launch.UserInput
    '  Case "C": GoTo ExitSub
    '  Case "P": DoEvents: GoTo CheckStatus
    'End Select
    'thisPercent = 100 * CSng(i + tsPadCount) / (tsCount * 2)
    thisPercent = 100 * i / tsCount '50 * CSng(i + tsPadCount) / tsCount
    If thisPercent > lastPercent Then
      thisTime = Timer
      If thisTime <> lastTime Then
        IPC.SendMonitorMessage "(PROGRESS " & thisPercent & ")" '& " " & Timer & ")"
        lastPercent = thisPercent
        lastTime = thisTime
      End If
    End If
  ElseIf StartTime - Time > 1 Then
    'If Launch.UserInput = "" Then
    'End If 'Discard things the user may have pressed before
    IPC.SendMonitorMessage "(OPEN Filling Reach Details)"
    'Launch.SendMonitorMessage "(BUTTOFF Cancel)"
    'Launch.SendMonitorMessage "(BUTTOFF Pause)"
    OpenedStatus = True
  End If
  Return
End Sub

'ConstInt will be false on return if a non-constant interval ts was filled
Public Function FillTimSerExt(ts As Collection, ConstInt As Boolean) As Collection
  Dim lts As Collection
  Dim Its As Long
  Dim curTs As ATCclsTserData
  Dim firstTS As ATCclsTserData

  'get the data values for selected data sets
  Set lts = FillTimSer(ts)
  Set FillTimSerExt = lts

  'look for any non-constant interval data
  ConstInt = True
  For Its = lts.Count To 1 Step -1
    Set curTs = lts.item(Its)
    
    If curTs.dates.Summary.NVALS < 1 Then
      lts.Remove Its 'Remove empty timeseries
    ElseIf Not ConstInt Then
      'already know it is not all the same interval, don't need to check
    ElseIf Not (curTs.dates.Summary.CIntvl) Then
      ConstInt = False
    ElseIf firstTS Is Nothing Then
      Set firstTS = curTs
    Else
      If firstTS.dates.Summary.SJDay <> curTs.dates.Summary.SJDay Then ConstInt = False
      If firstTS.dates.Summary.EJDay <> curTs.dates.Summary.EJDay Then ConstInt = False
      If firstTS.dates.Summary.NVALS <> curTs.dates.Summary.NVALS Then ConstInt = False
    End If
  Next

End Function

Public Sub DoProfPlot(ppts As Collection, tcnt&, tval$(), _
                      lnsen&, lsen$(), lncon&, lcon$(), _
                      ByVal peakfg&, ByVal stagefg&, ByVal invrtfg&, _
                      ByVal rangefg&, ByVal initfg&, ByVal relativefg&, _
                      Optional ByVal xaxlab As String = "Station", _
                      Optional ByVal yaxlab As String = "", _
                      Optional ByVal dataLabelAttribute As String = "")

    'argument definitions
    'tcnt    - count of time intervals being plotted
    'tval    - array of date strings for time intervals
    '          must be in format yyyy/mm/dd hh:mm:ss
    'lnsen   - number of scenarios being plotted
    'lsen    - array of scenario names
    'lncon   - number of constituents being plotted
    'lcon    - array of constituent names
    'peakfg  - plot peaks flag (0 - no, 1 - yes)
    'stagefg - include invert in stage calculation
    'invrtfg - show invert on plot (0 - no, 1 - yes)
    'rangefg - use whole time period for calculating min/max
    'initfg  - 1=initialize fresh plot
    'relativefg - 1=ppts(i).Dist are relative, 0=absolute
    'xaxlab  - X axis label
    'yaxlab  - Y axis label
    
  Dim i&, j&, k&, l&, n&, ip&, ic&, imatch%, invrt!
  Dim nc&, nv&, tind&, tstr$, rmin!, rmax!
  Dim cdist!, ldt&(5), jdt#
  Dim ltitl$, capt$, VLab$(), lgnd$()
  Dim XYD() As xyplotdata
  Dim ThisX As Single

  capt = "GenScn Profile Plot"
  'set data for plot
  'Call FillReachDetails(ppts)
  nc = tcnt * lnsen * lncon
  ReDim VLab(2 * nc + 1)
  ReDim lgnd(nc)
  ReDim XYD(nc)
  n = 0
  For k = 0 To lnsen - 1
    For ic = 0 To lncon - 1
      For j = 0 To tcnt - 1
        nv = 0
        XYD(n).Var(0).Trans = 1
        XYD(n).Var(1).Trans = 1
        XYD(n).Var(0).Min = 1000000#
        XYD(n).Var(0).Max = -1000000#
        cdist = 0
        For i = 1 To ppts.Count
          If ppts(i).Header.sen = lsen(k) And ppts(i).Header.con = lcon(ic) Then
            'scenarios match, include this point
            ReDim Preserve XYD(n).Var(0).Vals(nv)
            ReDim Preserve XYD(n).Var(1).Vals(nv)
            ReDim Preserve XYD(n).DataLabels.Labels(nv)
            If Len(dataLabelAttribute) > 0 Then
              XYD(n).DataLabels.Labels(nv) = ppts(i).Attrib(dataLabelAttribute)
            Else
              XYD(n).DataLabels.Labels(nv) = ""
            End If
            XYD(n).DataLabels.nLables = nv + 1
            'save y value (stage)
            If peakfg = 1 Then
              'use peak values
              XYD(n).Var(0).Vals(nv) = ppts(i).Max
            ElseIf Len(tval(j)) > 4 Then 'use value in time interval
              'find appropriate time interval for this scenario
              tstr = tval(j)
              ldt(0) = Mid(tstr, 1, 4)
              ip = 6
              For l = 1 To 5
                ldt(l) = Mid(tstr, ip, 2)
                ip = ip + 3
              Next l
              jdt = Date2J(ldt)
              tind = ppts(i).dates.IndexAtOrAfter(jdt)
              'tind = 1
              'While ppts(i).dates.value(tind) < jdt
              '  tind = tind + 1
              '  If tind > ppts(i).dates.summary.NVALS Then
              'Wend
              If tind > ppts(i).dates.Summary.NVALS Then Exit Sub
              Call J2Date(ppts(i).dates.Value(tind), ldt())
              tstr = ldt(0) & "/" & Format$(ldt(1), "00") & "/" & Format$(ldt(2), "00") & " " & Format$(ldt(3), "00") & ":" & Format$(ldt(4), "00") & ":" & Format$(ldt(5), "00")
              XYD(n).Var(0).Vals(nv) = ppts(i).Value(tind)
            End If
            invrt = ppts(i).AttribNumeric("INVRT")
            If stagefg = 1 And XYD(n).Var(0).Vals(nv) < invrt Then
              'include invert in y value
              XYD(n).Var(0).Vals(nv) = XYD(n).Var(0).Vals(nv) + invrt
            End If
            If rangefg = 1 Then
              'find extent of values for whole time period
              If stagefg = True Then
                rmin = ppts(i).Min + invrt
                rmax = ppts(i).Max + invrt
              Else
                rmin = ppts(i).Min
                rmax = ppts(i).Max
              End If
            Else 'just find extent for this time period
              rmin = XYD(n).Var(0).Vals(nv)
              rmax = XYD(n).Var(0).Vals(nv)
            End If
            If rmin < XYD(n).Var(0).Min Then
              XYD(n).Var(0).Min = rmin
            End If
            If rmax > XYD(n).Var(0).Max Then
              XYD(n).Var(0).Max = rmax
            End If
            'save x value (distance)
            If xaxlab = "Station" Then
              ThisX = ppts(i).AttribNumeric("DISTANCE")
            Else
              ThisX = ppts(i).AttribNumeric(xaxlab)
            End If
            If relativefg = 0 Then
              XYD(n).Var(1).Vals(nv) = ThisX
            Else
              XYD(n).Var(1).Vals(nv) = cdist
              cdist = cdist + ThisX
            End If
            nv = nv + 1
          End If
        Next i
        XYD(n).NVal = nv
        XYD(n).Var(1).Min = XYD(n).Var(1).Vals(0)
        XYD(n).Var(1).Max = XYD(n).Var(1).Vals(nv - 1)
        If relativefg = 0 Then 'x values are not necessarily in order, so search for min and max
          For i = 0 To nv - 1
            If XYD(n).Var(1).Vals(i) < XYD(n).Var(1).Min Then
              XYD(n).Var(1).Min = XYD(n).Var(1).Vals(i)
            ElseIf XYD(n).Var(1).Vals(i) > XYD(n).Var(1).Max Then
              XYD(n).Var(1).Max = XYD(n).Var(1).Vals(i)
            End If
          Next i
        End If
        lgnd(n) = ""
        If lnsen > 1 Then 'include scenario in legend
          lgnd(n) = lsen(k)
        End If
        If lncon > 1 Then 'include constituent in legend
          lgnd(n) = lgnd(n) & " " & lcon(ic)
        End If
        If Len(lgnd(n)) > 0 Then
          lgnd(n) = Trim(lgnd(n)) & " - "
        End If
        If peakfg = 1 Then
          'plotting peak values
          lgnd(n) = lgnd(n) & "Peaks"
        ElseIf tcnt > 1 Then 'include time interval in legend
          lgnd(n) = lgnd(n) & tstr
        End If
        If Len(lgnd(n)) > 0 Then
          If Mid(lgnd(n), Len(lgnd(n)) - 1, 1) = "-" Then
            lgnd(n) = Left(lgnd(n), Len(lgnd(n)) - 2)
          End If
        End If
        If lgnd(n) <> "" Then
          VLab(2 * n) = lgnd(n)
        Else 'only 1 scenario and interval,
          'use constituent for label
          VLab(2 * n) = ppts(1).Header.con
        End If
        VLab(2 * n + 1) = "Station"
        'increment curve counter
        n = n + 1
      Next j
    Next ic
  Next k
  If invrtfg = 1 Then
    'display invert(s)
    For k = 0 To lnsen - 1
      nv = 0
      ReDim Preserve VLab(2 * nc + 1)
      ReDim Preserve lgnd(nc)
      ReDim Preserve XYD(nc)
      XYD(nc).Var(0).Trans = 1
      XYD(nc).Var(1).Trans = 1
      XYD(nc).Var(0).Min = 1000000#
      XYD(nc).Var(0).Max = -1000000#
      cdist = 0
      For i = 1 To ppts.Count
        If ppts(i).sen = lsen(k) Then
          ReDim Preserve XYD(nc).Var(0).Vals(nv)
          ReDim Preserve XYD(nc).Var(1).Vals(nv)
          'save Y value
          XYD(nc).Var(0).Vals(nv) = ppts(i).Attrib("INVRT") 'ppts(i).invrt
          If XYD(nc).Var(0).Vals(nv) < XYD(nc).Var(0).Min Then
            XYD(nc).Var(0).Min = XYD(nc).Var(0).Vals(nv)
          End If
          If XYD(nc).Var(0).Vals(nv) > XYD(nc).Var(0).Max Then
            XYD(nc).Var(0).Max = XYD(nc).Var(0).Vals(nv)
          End If
          'save x value (distance)
          If ppts(i).Dist = 0 And nv > 0 And relativefg = 1 Then
            'new branch starts at 0, add cumulative distance
            cdist = XYD(nc).Var(1).Vals(nv - 1)
          End If
          XYD(nc).Var(1).Vals(nv) = ppts(i).Dist + cdist
          nv = nv + 1
        End If
      Next i
      XYD(nc).NVal = nv
      XYD(nc).Var(1).Min = XYD(nc).Var(1).Vals(0)
      XYD(nc).Var(1).Max = XYD(nc).Var(1).Vals(nv - 1)
      lgnd(nc) = lsen(k) & " Invert"
      VLab(2 * nc) = lgnd(nc)
      VLab(2 * nc + 1) = "Station"
      If k > 0 Then 'compare w/previous inverts
        j = tcnt * lnsen
        While j < tcnt * lnsen + k
          imatch = 1 'assume matching inverts
          If XYD(j).NVal = XYD(j + k).NVal Then
            'same # pts, check pt values
            For i = 0 To XYD(j).NVal - 1
              If XYD(j).Var(0).Vals(i) <> XYD(j + k).Var(0).Vals(i) Then
                imatch = 0
              ElseIf XYD(j).Var(1).Vals(i) <> XYD(j + k).Var(1).Vals(i) Then
                imatch = 0
              End If
            Next i
          Else
            imatch = 0
          End If
          If imatch = 1 Then 'matching inverts,
            'don't need another curve
            nc = nc - 1
            j = tcnt * lnsen + k
          Else 'look for match to next invert
            j = j + 1
          End If
        Wend
      ElseIf lnsen > 1 Then 'more inverts possible
        nc = nc + 1
      End If
    Next k
    If nc = tcnt * lnsen Then
      'only 1 invert, don't need scen in legend
      lgnd(nc) = "Invert"
      VLab(2 * nc) = lgnd(nc)
    End If
    nc = nc + 1
  End If
  If initfg <> 1 Then
    If nc <> GraphList.ncrv Then initfg = 1
  End If
  If initfg = 1 Then 'fresh plot, initialize
    Call GLInit(1, gProfPlot, nc, 2 * nc)
    gProfPlot.HelpFileName = App.HelpFile
    GoSub SetTitle
    If yaxlab = "" Then
      yaxlab = ppts(1).Header.con
      For i = 1 To lncon - 1
        yaxlab = yaxlab & ", " & VLab(2 * i)
      Next i
    End If
    Call GLTitl(ltitl, capt)
    Call GLIcon(frmGenProfPlot.Icon)
    Call GLAxLab(xaxlab, yaxlab, "", "")
    Call GLAxTics(5, 10, 10, 0)
    Call GLVarLab(VLab())
  Else 'reusing graph, probably in animation
    If tcnt = 1 And peakfg <> 1 Then    'Title changes as date increments
      GoSub SetTitle
      Call GLTitl(ltitl, capt)
    End If
  End If
  
  Call GLLegend(lgnd())
  Call GLDoXY(gProfPlot, 1, XYD(), initfg)
  Exit Sub
  
SetTitle:
  ltitl = "Profile Plot of "
  If lnsen = 1 Then 'include scenario in title
    ltitl = ltitl & ppts(1).Header.sen & " "
  End If
  If lncon = 1 Then 'include constituent in title
    ltitl = ltitl & ppts(1).Header.con
  End If
  ltitl = ltitl & "&"
  'add branch/node extent to title
  ltitl = ltitl & "Branch " & ppts(1).Attrib("BRANCH") & _
                  ", Node " & ppts(1).Attrib("NODE") & _
                  " - Branch " & ppts(ppts.Count).Attrib("BRANCH") & _
                  ", Node " & ppts(ppts.Count).Attrib("NODE")
  If tcnt = 1 Then
    If peakfg = 1 Then
      ltitl = ltitl & "&Peak Values"
    Else
      ltitl = ltitl & "&" & tstr
    End If
  End If
  Return
End Sub

Public Function FillTimSer(ts As Collection) As Collection
  Dim v As Variant
  Dim JSdt#, JEdt#, Sdt&(6), Edt&(6)
  Dim lts As Collection
  Dim ds As ATCclsTserDate
  Dim subSet As ATCclsTserData
  Dim aggDat As ATCclsTserData

  JSdt = Date2J(CSDat())
  JEdt = Date2J(CEDat())
  
  Set lts = Nothing
  Set lts = New Collection
  
  If CDTran = 4 Then
    For Each v In ts
      If JSdt > v.dates.Summary.SJDay Or JEdt < v.dates.Summary.EJDay Then
        lts.Add v.SubSetByDate(JSdt, JEdt)
      Else
        lts.Add v
      End If
    Next
  Else
    Dim lDateSummary As ATTimSerDateSummary
    With lDateSummary
      .CIntvl = True
      .SJDay = JSdt
      .EJDay = JEdt
      .ts = CTStep
      .Tu = ctunit
      Select Case ctunit
        Case TUSecond:  .Intvl = CTStep / 86400#
        Case TUMinute:  .Intvl = CTStep / 1440#
        Case TUHour:    .Intvl = CTStep / 24#
        Case TUDay:     .Intvl = CTStep
        Case TUMonth:   .Intvl = CTStep * 30.44
        Case TUYear:    .Intvl = CTStep * 365.25
        Case TUCentury: .Intvl = CTStep * 36525
        Case Else: .Tu = TUCentury: .Intvl = 36525 'should not happen
      End Select
      If .Intvl <= 1 Then
        .NVALS = (JEdt - JSdt) / .Intvl '+ 1
      Else 'special case for long intervals
        Call J2Date(JSdt, Sdt)
        Call J2Date(JEdt, Edt)
        Call timdif(Sdt, Edt, .Tu, .ts, .NVALS)
      End If
    End With
    Set ds = New ATCclsTserDate
    ds.Summary = lDateSummary
    For Each v In ts
      With v.dates.Summary
        If Not .CIntvl Or JSdt > .SJDay Or JEdt < .EJDay Or CTStep <> .ts Or ctunit <> .Tu Or lDateSummary.NVALS <> .NVALS Then
          Set subSet = v.SubSetByDate(JSdt, JEdt)
          Set aggDat = subSet.Aggregate(ds, CDTran)
          lts.Add aggDat
          Set aggDat = Nothing
          Set subSet = Nothing
        Else
          lts.Add v
        End If
      End With
    Next
  End If
  'get the data values for selected data sets
  Set FillTimSer = Nothing
  Set FillTimSer = lts
  'End If
End Function

Public Sub RefreshSLC()

  Dim i&, StartTime As Variant
  Dim vTserFile As Variant
  Dim ATserF As ATCclsTserFile
  'Dim Atser As ATCclsTserData
  Dim newColl As Collection 'build collection of all datasets
  Dim OpenFiles As Collection
  Dim tc As Collection

  Set OpenFiles = New Collection
  Set newColl = New Collection
  For Each vTserFile In TserFiles.Active
    Set ATserF = vTserFile.obj
    OpenFiles.Add ATserF
    For i = 1 To ATserF.DataCount
      newColl.Add ATserF.Data(i)
    Next i
  Next vTserFile
  
  Set p.ConsName = uniqueAttributeValues("CONS", newColl)
  Set p.LocnName = uniqueAttributeValues("LOCN", newColl)
  Set p.ScenName = uniqueAttributeValues("SCEN", newColl)
  
  StartTime = Timer
  FillReachDetails newColl
  Debug.Print "Gentry:RefreshSLC:FillReachDetails took " & Timer - StartTime & " sec"
  Set frmGenScn.TimserGrid.WholeList = newColl
  Set frmGenScn.TimserGrid.OpenFiles = OpenFiles
  CntDsn = newColl.Count ' CountAllTimser
  Call frmGenScn.RefreshMain 'Was this commented out for a reason?
  Set OpenFiles = Nothing
  Set newColl = Nothing
End Sub

Sub Main()
  Dim hdle&, i&, binpos&
  Dim s As String * 80
  Dim ExCmd As String 'command line
  Dim StartDir As String
  Dim uCommand As String
  Dim cpos&, ctype$, cbloc$, cwshed$, cmap$, crem$
  Dim LoadProg As Boolean
  Dim tmp As String
  Dim DebugTrace As String
  
  DebugTrace = "GenScnEntry:Main"
  On Error GoTo MainError
  
  ReDim specSen(0)
  ReDim specLoc(0)
  ReDim specCon(0)
  
  StartDir = CurDir
  DebugTrace = DebugTrace & vbCr & "StartDir = '" & StartDir & "'"
  uCommand = UCase(command)
  DebugTrace = DebugTrace & vbCr & "command = '" & command & "'"

  'If InStr(uCommand, "DEBUGALL") > 0 Then DebugMode = True Else DebugMode = False
  
  DebugTrace = DebugTrace & vbCr & "GetModuleHandle"
  hdle = GetModuleHandle("Genscn")
  DebugTrace = DebugTrace & vbCr & "GetModuleFileName"
  i = GetModuleFileName(hdle, s, 80)
  ExeName = UCase(Left(s, InStr(s, Chr(0)) - 1))
  If InStr(ExeName, "VB6.EXE") Then
    RunningVB = True
    ExeName = UCase(MACHINE_EXENAME)
    'ExCmd = UCase(MACHINE_EXECMD)
    DebugTrace = DebugTrace & vbCr & "ShowWin"
    ShowWin "Microsoft Visual Basic", SW_MINIMIZE, 0
  Else
    RunningVB = False
  End If
  ExCmd = command$
  uCommand = UCase(ExCmd)
  binpos = InStr(ExeName, "\BIN")
  If binpos < 1 Then binpos = InStrRev(ExeName, "\")
  If binpos < 1 Then
    ExePath = CurDir
  Else
    ExePath = Left(ExeName, binpos)
  End If
  If Right(ExePath, 1) <> "\" Then ExePath = ExePath & "\"
  
  DebugTrace = DebugTrace & vbCr & "Set Registry"
  Set Registry = New ATCoRegistry
  Registry.AppName = AppName

  App.HelpFile = Registry.RegGetString(HKEY_LOCAL_MACHINE, "SOFTWARE\AQUA TERRA Consultants\GenScn\DocPath", "") & "\GenScn.chm"
  If Not FileExists(App.HelpFile) Then App.HelpFile = ExePath & "doc\genscn.chm"
  If Not FileExists(App.HelpFile) Then App.HelpFile = "genscn.chm"

'  LoadProg = False
'  If InStr(uCommand, "/REGPATH") > 0 Then
'    ExePath = ExePath & "bin"
'    If MsgBox("Setting registry key:" & vbCr & "HKEY_LOCAL_MACHINE\" & Registry.GlobalPrefix & Registry.AppName & "\ExePath" & vbCr & "To:" & vbCr & ExePath, vbOKCancel, "Register Path") = vbOK Then
'      Registry.GlobalValue("", "ExePath") = ExePath
'    End If
'  ElseIf InStr(uCommand, "/UNREGPATH") > 0 Then
'    MsgBox "You may manually remove the key" & vbCr _
'    & "HKEY_LOCAL_MACHINE\Software\AQUA TERRA Consultants\GenScn\ExePath", vbOKOnly, "GenScn Registry"
'  Else
    LoadProg = True
    'test for basins/genscn link
    'command line includes type, basins loc, project, map file name
    'crem = "/BASHSPF C:\BASINS basins3 c:\shena\data\shena.map"
    cpos = InStr(uCommand, "/BAS")
    If cpos > 0 Then
      ctype = Mid(uCommand, cpos + 4, 4)
      crem = Mid(ExCmd, cpos + 9)
      cbloc = StrRetRem(crem)
      cwshed = StrRetRem(crem)
      cmap = StrRetRem(crem)
      frmBasins.Init ctype, cbloc, cwshed, cmap
      frmBasins.Show vbModal
      frmBasins.GetStaName ExCmd
      If Len(ExCmd) = 0 Then LoadProg = False
    End If
'  End If

  If LoadProg Then
    DebugTrace = DebugTrace & vbCr & "Set p.ExternalForms = New Collection"
    Set p.ExternalForms = New Collection
    
    DebugTrace = DebugTrace & vbCr & "Set atEXE"
    Set atEXE = New ATCoCtl.ATCoEXE
    
    DebugTrace = DebugTrace & vbCr & "Set InMemFile"
    Set InMemFile = New clsTSerMemory
    
    DebugTrace = DebugTrace & vbCr & "Set p.WDMFiles"
    Set p.WDMFiles = New Collection
    
    DebugTrace = DebugTrace & vbCr & "Set DispFile"
    Set DispFile = New ATCoDispFile
    
    DebugTrace = DebugTrace & vbCr & "Set p.PreferredUnits = New FastCollection"
    Set p.PreferredUnits = New FastCollection
    
    'If the current dir is where GenScn.exe is, change to sample data dir
    On Error Resume Next
    If Mid(ExCmd, 2, 1) = ":" Then
      If Left(ExCmd, 1) <> Left(CurDir, 1) Then
        ChDrive Left(ExCmd, 1)
      End If
    End If
    If UCase(CurDir) = UCase(ExePath) & "BIN" Then
      If FileExists("..\Data", True, False) Then
        ChDir "..\Data"
      Else
        tmp = Registry.RegGetString(HKEY_LOCAL_MACHINE, "SOFTWARE\AQUA TERRA Consultants\GenScn\SampleData", "")
        DebugTrace = DebugTrace & vbCr & "GenScn\SampleData = " & tmp
        ChDriveDir tmp
      End If
    End If
    
    DebugTrace = DebugTrace & vbCr & "Set IPC"
    Set IPC = New ATCoIPC ' AtCoLaunch
    IPC.SendMonitorMessage "Caption GenScn Status Monitor"
    binpos = InStr(UCase(ExePath), "BASINS")
    If binpos > 0 Then
      LogFileName = Left(ExePath, binpos - 1) & "Basins\Cache\Log\"
    Else
      LogFileName = ExePath & "log\"
    End If
    LogFileName = LogFileName & "GenScn.txt"
      
    IPC.SendMonitorMessage "LogToFile " & LogFileName
'    DebugTrace = DebugTrace & vbCr & "Launch.StartMonitor '" & ExePath & "bin\status.exe'"
'    launch.StartMonitor (ExePath & "bin\status.exe")
'    DebugTrace = DebugTrace & vbCr & "hin = Launch.ComputeRead"
    
    If InStr(UCase(command), "DEBUG") > 0 Then
      MsgBox "IPC.SendMonitorMessage (SHOW)"
      IPC.SendMonitorMessage "(SHOW)"
      MsgBox "Status should have opened."
    End If

    DbgMsg "GenScnEntry:Main:StartDir = " & StartDir
    DbgMsg "GenScnEntry:Main:CurDir = " & CurDir
    DbgMsg "GenScnEntry:Main:ExePath = " & ExePath
    DbgMsg "GenScnEntry:Main:ExCmd = " & ExCmd
    DbgMsg "GenScnEntry:Main:ExeName = " & ExeName
    DbgMsg "GenScnEntry:Main:App.HelpFile = " & App.HelpFile
    
'    launch.NconvertPath = Registry.RegGetString(HKEY_LOCAL_MACHINE, "SOFTWARE\AQUA TERRA Consultants\NconvertPath", "") & "\Nconvert.exe"
'    If Len(launch.NconvertPath) = 0 Then launch.NconvertPath = ExePath & "Nconvert.exe"
'    If Len(Dir(launch.NconvertPath)) = 0 Then launch.NconvertPath = ExePath & "bin\Nconvert.exe"
'    If Len(Dir(launch.NconvertPath)) = 0 Then launch.NconvertPath = "Nconvert.exe"
'    If Len(Dir(launch.NconvertPath)) = 0 Then launch.NconvertPath = ""
'    DbgMsg "GenScnEntry:NconvertPath = '" & launch.NconvertPath & "'"
    
    DbgMsg "GenScnEntry:F90_W99OPN"
    Call F90_W99OPN  'open error file for fortan problems
    
    DbgMsg "GenScnEntry:F90_WDBFIN"
    Call F90_WDBFIN  'initialize WDM record buffer
    
    DbgMsg "GenScnEntry:F90_PUTOLV"
    Call F90_PUTOLV(10)
  
    DbgMsg "GenScnEntry:F90_SPIPH"
    Call F90_SPIPH(IPC.hPipeReadFromProcess(0), IPC.hPipeWriteToProcess(0))
    'Call F90_SCNDBG(1&)
    
    DbgMsg "GenScnEntry:Set TSer"
    Set Tser = New Collection
    
    'InitATCoAnalysis
    
    sSLC(0) = "Scenario"
    sSLC(1) = "Location"
    sSLC(2) = "Constituent"
    sSLC(3) = "Location3"
    sSLC(4) = "Location4"

    Set Batch = New BatchGenScn
    'Load frmGenScn 'load main form
    DbgMsg "GenScnEntry:frmGenScn.Show"
    frmGenScn.Show
    DbgMsg "GenScnEntry:Back from frmGenScn.Show"
    Set Batch.ComDlg = frmGenScn.cdl
    If Len(ExCmd) > 0 Then
      If Len(Dir(ExCmd)) > 0 Then 'might be a status file
        DbgMsg "GenScnEntry:OpenStatusFile '" & ExCmd & "'"
        Call frmGenScn.OpenStatusFile(ExCmd)
      End If
    End If
  End If
  Exit Sub
MainError:
  MsgBox err.Description & vbCr & DebugTrace, vbCritical, "Error in GenScnEntry:Main"
End Sub

Public Sub DbgMsg(Msg As String, Optional lvl As Long = 0, Optional CodeGroup As String = "", Optional typ As String = "")
  If Len(CodeGroup) > 0 Then
    IPC.dbg "GenScn:" & CodeGroup & ":" & Msg
  Else
    IPC.dbg "GenScn:" & Msg
  End If
End Sub

'Public Sub InitATCoAnalysis()
'  Dim regKey As Variant
'  Dim iKey As Integer 'index of registry key being examined
'
'  Set Analyses = Nothing
'  'On Error GoTo PlugInsError
'  Set Analyses = New ATCData.ATCPlugInManager
'  On Error GoTo 0
'  regKey = GetAllSettings("ATCoPlugin", AnalysisClassName)
'  If IsEmpty(regKey) Then
'    SaveSetting "ATCoPlugin", AnalysisClassName, AnalysisClassName, AnalysisClassName & ".dll"
'    regKey = GetAllSettings("ATCoPlugin", AnalysisClassName)
'  End If
'  For iKey = LBound(regKey, 1) To UBound(regKey, 1)
'    If Not Analyses.Load(CStr(regKey(iKey, 0))) Then
'      MsgBox "Could not load Analysis " & regKey(iKey, 0) & " from " & regKey(iKey, 1) & vbCr & Analyses.ErrorDescription
'    End If
'  Next iKey
'
'  Exit Sub
'
'PlugInsError:
'  MsgBox "Could not load Analysis from " & AnalysisClassName & vbCr & Analyses.ErrorDescription
'End Sub

Public Sub InitATCoTSer()
  Dim regKey As Variant
  Dim iKey As Integer 'index of registry key being examined
  Dim TserIndex As Long
  
  InitMatchingColors ExePath & "GraphColors.txt"
    
  Set TserFiles = Nothing
  On Error GoTo TSerPlugInsError
  Set TserFiles = New ATCData.ATCPlugInManager
  On Error GoTo 0
  regKey = GetAllSettings("ATCoPlugin", TserFileClassName)
  If IsEmpty(regKey) Then
    SaveSetting "ATCoPlugin", TserFileClassName, TserFileClassName, TserFileClassName & ".dll"
    regKey = GetAllSettings("ATCoPlugin", TserFileClassName)
  End If
  For iKey = LBound(regKey, 1) To UBound(regKey, 1)
    If Not TserFiles.Load(CStr(regKey(iKey, 0))) Then
      MsgBox "Could not load Timeseries " & regKey(iKey, 0) & " from " & regKey(iKey, 1) & vbCr & TserFiles.ErrorDescription
    End If
  Next iKey
  TserIndex = TserFiles.AvailIndexByName("clsTSerMemory")
  If TserIndex > 0 Then
    Call TserFiles.Create(TserIndex)
    Set InMemFile = Nothing
    Set InMemFile = TserFiles.CurrentActive.obj
  End If
  
  Exit Sub
  
TSerPlugInsError:
  MsgBox "Could not load Timeseries from " & TserFileClassName & vbCr & TserFiles.ErrorDescription
End Sub

Public Function CountAllTimser() As Long
  Dim vTserFile As Variant, curClsTserFile As ATCclsTserFile, i&
   
  i = 0
  For Each vTserFile In TserFiles.Active
    Set curClsTserFile = vTserFile.obj
    i = i + curClsTserFile.DataCount
  Next vTserFile
  CountAllTimser = i
End Function

Public Sub FindTimSer(sen$, loc$, con$, lts As Collection)
  Dim dsn&, i&, j&, k&, l&, s$, GRPSIZ&, r!, imatch%
  Dim usen$, uloc$, ucon$
  Dim Nosen As Boolean, Noloc As Boolean, Nocon As Boolean
  Dim vTserFile As Variant, curClsTserFile As ATCclsTserFile, lds As ATTimSerDateSummary
  'Dim t As ATCclsTserData
  'dim newTs As ATTimSer

  If Len(Trim(sen)) = 0 Then Nosen = True Else Nosen = False: usen = UCase(sen)
  If Len(Trim(loc)) = 0 Then Noloc = True Else Noloc = False: uloc = UCase(loc)
  If Len(Trim(con)) = 0 Then Nocon = True Else Nocon = False: ucon = UCase(con)
  Set lts = Nothing
  Set lts = New Collection
  For Each vTserFile In TserFiles.Active
    Set curClsTserFile = vTserFile.obj
    For j = 1 To curClsTserFile.DataCount

'      This comprehensible If statement works, but is slow because VB evaluates all the parts every time
'      If (usen = UCase(curClsTserFile.Data(j).header.sen) Or Nosen) And _
'         (uloc = UCase(curClsTserFile.Data(j).header.loc) Or Noloc) And _
'         (ucon = UCase(curClsTserFile.Data(j).header.con) Or Nocon) Then 'need this timser
'        lts.Add curClsTserFile.Data(j)
'      End If

      'This set of ifs and gotos replaces the above If statement
      If Nosen Then GoTo CheckLoc
      If usen = UCase(curClsTserFile.Data(j).Header.sen) Then GoTo CheckLoc
      GoTo NextTS
CheckLoc:
      If Noloc Then GoTo CheckCon
      If uloc = UCase(curClsTserFile.Data(j).Header.loc) Then GoTo CheckCon
      GoTo NextTS
CheckCon:
      If Nocon Then GoTo AddThisTS
      If ucon = UCase(curClsTserFile.Data(j).Header.con) Then GoTo AddThisTS
      GoTo NextTS
AddThisTS:
      lts.Add curClsTserFile.Data(j)
NextTS:
    Next j
  Next vTserFile
    
End Sub

Public Function GetDescription$(SCNorCON$, itemname$)
  Dim i&
  GetDescription = ""
  On Error Resume Next
  If SCNorCON = "SCN" Then
    GetDescription = p.ScenDesc(itemname)
  ElseIf SCNorCON = "CON" Then
    GetDescription = p.ConsDesc(itemname)
  End If
End Function

Public Function ScenPath(s$) As String
    ScenPath = p.ScenFile(s)
End Function

Public Sub UpdateSite(opt%, lts As Collection, l As ListBox) 'only called for location lists
  Dim i%, j%, s$
  
  'l.clear
  For i = 1 To lts.Count
    If opt = 0 Then
      s = lts(i).Header.sen
    Else
      s = lts(i).Header.con
    End If
    j = 0
    While j < l.ListCount
      If s = l.List(j) Then
        j = l.ListCount + 1 'already in list
      Else
        j = j + 1
      End If
    Wend
    If j = l.ListCount Then 'not already in list
      l.AddItem s
    End If
  Next i
  'Call frmGenScn.SyncLists(frmGenScn.lstSLC(2 * opt), l)
End Sub

'Public Sub AddUniqueSLC(s$, slc$)
'  Dim i%, fnd As Boolean, us$
'
'  fnd = False
'  us = UCase(s)
'  Select Case slc
'
'  Case "Scen":
'    For i = LBound(p.Scen) To UBound(p.Scen)
'      If us = UCase(p.Scen(i).Name) Then
'        fnd = True
'        Exit For
'      End If
'    Next i
'    If Not (fnd) Then
'      ReDim Preserve p.Scen(p.ScenCount)
'      p.Scen(p.ScenCount).Name = s
'      p.ScenCount = p.ScenCount + 1
'    End If
'  Case "Locn":
'    For i = LBound(p.Locn) To UBound(p.Locn)
'      If us = UCase(p.Locn(i).Name) Then
'        fnd = True
'        Exit For
'      End If
'    Next i
'    If Not (fnd) Then
'      ReDim Preserve p.Locn(p.LocnCount)
'      p.Locn(p.LocnCount).Name = s
'      p.LocnCount = p.LocnCount + 1
'    End If
'  Case "Cons":
'    For i = LBound(p.Cons) To UBound(p.Cons)
'      If us = UCase(p.Cons(i).Name) Then
'        fnd = True
'        Exit For
'      End If
'    Next i
'    If Not (fnd) Then
'      ReDim Preserve p.Cons(p.ConsCount)
'      p.Cons(p.ConsCount).Name = s
'      p.ConsCount = p.ConsCount + 1
'    End If
'  End Select
'
'End Sub

Sub GetInfoFromWDMTSer(dsn&, sen$, loc$, con$, Tu&, ts&, sdat&(), edat&())
  Dim myData As ATCclsTserData, myWdm As clsTSerWDM
  
  Set myWdm = p.WDMFiles(1)
  Set myData = myWdm.GetDataSetFromDsn(dsn)
  
  On Error GoTo x:
  sen = myData.Header.sen
  loc = myData.Header.loc
  con = myData.Header.con
  Tu = myData.dates.Summary.Tu
  ts = myData.dates.Summary.ts
  Call J2Date(myData.dates.Summary.SJDay, sdat)
  Call J2Date(myData.dates.Summary.EJDay, edat)
x:
End Sub

Sub InitSLCCollections()
  If p.ConsDesc Is Nothing Then Set p.ConsDesc = New CollString Else p.ConsDesc.Clear
  If p.ConsName Is Nothing Then Set p.ConsName = New CollString Else p.ConsName.Clear
  If p.LocnDesc Is Nothing Then Set p.LocnDesc = New CollString Else p.LocnDesc.Clear
  If p.LocnName Is Nothing Then Set p.LocnName = New CollString Else p.LocnName.Clear
  If p.ScenDesc Is Nothing Then Set p.ScenDesc = New CollString Else p.ScenDesc.Clear
  If p.ScenFile Is Nothing Then Set p.ScenFile = New CollString Else p.ScenFile.Clear
  If p.ScenName Is Nothing Then Set p.ScenName = New CollString Else p.ScenName.Clear
  If p.ScenType Is Nothing Then Set p.ScenType = New CollString Else p.ScenType.Clear
'  Set p.ConsDesc = Nothing
'  Set p.ConsDesc = New Collection
'  Set p.ConsName = Nothing
'  Set p.ConsName = New Collection
'  Set p.LocnDesc = Nothing
'  Set p.LocnDesc = New Collection
  Set p.LocnGeoCode = Nothing
  Set p.LocnGeoCode = New Collection
'  Set p.LocnName = Nothing
'  Set p.LocnName = New Collection
'  Set p.ScenDesc = Nothing
'  Set p.ScenDesc = New Collection
'  Set p.ScenFile = Nothing
'  Set p.ScenFile = New Collection
'  Set p.ScenName = Nothing
'  Set p.ScenName = New Collection
'  Set p.ScenType = Nothing
'  Set p.ScenType = New Collection
End Sub

Function StatusString(Optional AboutFlag As Boolean = True) As String
  Dim vTserFile As Variant
  Dim curClsTserFile As ATCclsTserFile
  Dim s$, i&
  
  If AboutFlag Then
    s = "GenScn - Version " & App.Major & "." & App.Minor
    If App.Revision >= 1000 Then
      If App.Revision > 1000 Then s = s & " build " & App.Revision - 1000
      s = s & vbCrLf ' " final" & vbCrLf
    Else
      s = s & " beta " & App.Revision & vbCrLf
      s = s & "FOR TESTING AND EVALUATION USE ONLY" & vbCrLf
    End If
    s = s & "-----------" & vbCrLf
    s = s & "Inquiries about this software should be directed to" & vbCrLf
    's = s & vbCrLf
    's = s & "U.S. Geological Survey" & vbCrLf
    's = s & "Hydrologic Analysis Software Support Program" & vbCrLf
    's = s & "437 National Center" & vbCrLf
    's = s & "Reston, VA 20192 " & vbCrLf
    's = s & "(electronic mail: h2osoft@usgs.gov)" & vbCrLf
    s = s & "the organization which supplied you this software." & vbCrLf
    s = s & "-----------" & vbCrLf
    i = 0
  Else
    i = 2
  End If
  
  s = s & Space(i) & "Current Directory: " & CurDir & vbCrLf & vbCrLf
  If Len(p.StatusFileName) = 0 Then
    s = s & Space(i) & "No Project Active" & vbCrLf
  Else
    s = s & Space(i) & "Project File: " & p.StatusFileName & vbCrLf
    If Len(frmGenScn.Map1.MapFileName) > 0 Then
      s = s & Space(i) & "Map File: " & frmGenScn.Map1.MapFileName & vbCrLf
    End If
    If Len(p.HSPFMsg.Name) > 0 Then
      s = s & Space(i) & "HSPF Message File: " & p.HSPFMsg.Name & vbCrLf
    End If
    
    For Each vTserFile In TserFiles.Active
      Set curClsTserFile = vTserFile.obj
      s = s & Space(i) & " " & curClsTserFile.label & _
        " File: " & curClsTserFile.Filename & vbCrLf
    Next vTserFile
  End If
  
  StatusString = s
End Function
