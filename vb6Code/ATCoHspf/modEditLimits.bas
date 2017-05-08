Attribute VB_Name = "modEditLimits"
Option Explicit
'Copyright 2002 AQUA TERRA Consultants - Royalty-free use permitted under open source license

Dim NoDsn&(), NoDsnCount&, scen$(), locn$(), cons$(), cwdmid$(), resp&

Public Sub CheckLimitsExtSources(g As Control, myUci As HspfUci)
  
  g.ClearValues
  If g.col = 0 Then 'svol
    Call SetLimitsWDM(g, myUci)
  ElseIf g.col = 1 Then 'svolno
    Call CheckValidDsn(g, myUci)
  ElseIf g.col = 2 Then 'smemn
    Call CheckValidMemberName(g, myUci)
  ElseIf g.col = 3 Then 'qflg
    g.ColMin(g.col) = 0
    g.ColMax(g.col) = 31
  ElseIf g.col = 4 Then 'ssyst
    g.addvalue "ENGL"
    g.addvalue "METR"
  ElseIf g.col = 5 Then 'sgapst
    g.addvalue " " 'allow default blank
    g.addvalue "UNDF"
    g.addvalue "ZERO"
  ElseIf g.col = 6 Then 'mfactr
  ElseIf g.col = 7 Then 'tran
    Call SetValidTrans(g)
  ElseIf g.col = 8 Then 'tvol
    Call SetValidOperations(g, myUci)
  ElseIf g.col = 9 Then 'topfst
    Call SetOperationMinMax(g, myUci, 1)
  ElseIf g.col = 10 Then 'group
    Call SetGroupNames(g, myUci.Msg, 2)
  ElseIf g.col = 11 Then 'tmemn
    Call SetMemberNames(g, myUci.Msg, 3)
  ElseIf g.col = 12 Then 'tmems1
  ElseIf g.col = 13 Then 'tmems2
  End If
End Sub

Public Sub CheckLimitsExtTargets(g As Control, myUci As HspfUci)
  
  g.ClearValues
  If g.col = 0 Then 'svol
    Call SetValidOperations(g, myUci)
  ElseIf g.col = 1 Then 'svolno
    Call SetOperationMinMax(g, myUci, 1)
  ElseIf g.col = 2 Then 'sgrpn
    Call SetGroupNames(g, myUci.Msg, 2)
  ElseIf g.col = 3 Then 'smemn
    Call SetMemberNames(g, myUci.Msg, 3)
  ElseIf g.col = 4 Then 'smems1
  ElseIf g.col = 5 Then 'smems2
  ElseIf g.col = 6 Then 'mfactr
  ElseIf g.col = 7 Then 'tran
    Call SetValidTrans(g)
  ElseIf g.col = 8 Then 'tvol:wdm and what else
    Call SetLimitsWDM(g, myUci)
  ElseIf g.col = 9 Then 'tvolno
    Call CheckValidDsn(g, myUci)
  ElseIf g.col = 10 Then 'tmemn
    Call CheckValidMemberName(g, myUci)
  ElseIf g.col = 11 Then 'qflg
    g.ColMin(g.col) = 0
    g.ColMax(g.col) = 31
  ElseIf g.col = 12 Then 'tsyst
    g.addvalue "ENGL"
    g.addvalue "METR"
  ElseIf g.col = 13 Then 'aggst
    g.addvalue " " 'allow default blank
    g.addvalue "AGGR"
  ElseIf g.col = 14 Then 'amdst
    g.addvalue "ADD "
    g.addvalue "REPL"
  End If
End Sub

Public Sub CheckLimitsNetwork(g As Control, myUci As HspfUci)
   
  g.ClearValues
  If g.col = 0 Then 'svol
    Call SetValidOperations(g, myUci)
  ElseIf g.col = 1 Then 'svolno
    Call SetOperationMinMax(g, myUci, 1)
  ElseIf g.col = 2 Then 'sgrpn
    Call SetGroupNames(g, myUci.Msg, 2)
  ElseIf g.col = 3 Then 'smemn
    Call SetMemberNames(g, myUci.Msg, 3)
  ElseIf g.col = 4 Then 'smems1
  ElseIf g.col = 5 Then 'smems2
  ElseIf g.col = 6 Then 'mfactr
  ElseIf g.col = 7 Then 'tran
    Call SetValidTrans(g)
  ElseIf g.col = 8 Then 'tvol
    Call SetValidOperations(g, myUci)
  ElseIf g.col = 9 Then 'topfst
    Call SetOperationMinMax(g, myUci, 1)
  ElseIf g.col = 10 Then 'tgrpn
    Call SetGroupNames(g, myUci.Msg, 2)
  ElseIf g.col = 11 Then 'tmem
    Call SetMemberNames(g, myUci.Msg, 3)
  ElseIf g.col = 12 Then 'tmems1
  ElseIf g.col = 13 Then 'tmems2
  End If
End Sub

Public Sub CheckLimitsMassLink(g As Control, myUci As HspfUci)

  g.ClearValues
  If g.col = 0 Then 'svol
    Call SetAllOperations(g, myUci)
  ElseIf g.col = 1 Then 'sgrpn
    Call SetGroupNames(g, myUci.Msg, 1)
  ElseIf g.col = 2 Then 'smemn
    Call SetMemberNames(g, myUci.Msg, 2)
  ElseIf g.col = 3 Then 'smems1
  ElseIf g.col = 4 Then 'smems2
  ElseIf g.col = 5 Then 'mfactr
  ElseIf g.col = 6 Then 'tvol
    Call SetAllOperations(g, myUci)
  ElseIf g.col = 7 Then 'tgrpn
    Call SetGroupNames(g, myUci.Msg, 1)
  ElseIf g.col = 8 Then 'tmem
    Call SetMemberNames(g, myUci.Msg, 2)
  ElseIf g.col = 9 Then 'tmems1
  ElseIf g.col = 10 Then 'tmems2
  End If
End Sub

Public Sub CheckLimitsSchematic(g As Control, myUci As HspfUci)
  
  g.ClearValues
  If g.col = 0 Then 'svol
    Call SetValidOperations(g, myUci)
  ElseIf g.col = 1 Then 'svolno
    Call SetOperationMinMax(g, myUci, 1)
  ElseIf g.col = 2 Then 'afacter
  ElseIf g.col = 3 Then 'tvol
    Call SetValidOperations(g, myUci)
  ElseIf g.col = 4 Then 'tvolno
    Call SetOperationMinMax(g, myUci, 1)
  ElseIf g.col = 5 Then 'mslkno
    Call SetValidMassLinks(g, myUci)
  End If
End Sub

Public Sub CheckLimitsSpecialActions(Index, g As Control, myUci As HspfUci)
  Dim vOpnBlk As Variant, lopnblk As HspfOpnBlk
  
  g.ClearValues
  If Index = 1 Then
    'action type record
    If g.col = 0 Then
      'valid operation types
      For Each vOpnBlk In myUci.OpnBlks
        Set lopnblk = vOpnBlk
        If lopnblk.Count > 0 Then
          If lopnblk.Name = "PERLND" Or lopnblk.Name = "IMPLND" Or _
             lopnblk.Name = "RCHRES" Or lopnblk.Name = "PLTGEN" Or _
             lopnblk.Name = "COPY" Or lopnblk.Name = "GENER" Then
            g.addvalue lopnblk.Name
          End If
        End If
      Next vOpnBlk
    ElseIf g.col = 1 Then
      Call SetOperationMinMax(g, myUci, 1)
    ElseIf g.col = 2 Then
      Call SetOperationMinMax(g, myUci, 2)
    ElseIf g.col = 3 Then
      g.addvalue "MI"
      g.addvalue "HR"
      g.addvalue "DY"
      g.addvalue "MO"
      g.addvalue "YR"
    ElseIf g.col = 4 Then
      g.ColMax(g.col) = -999
      g.ColMin(g.col) = 0
    ElseIf g.col = 5 Then
      g.ColMax(g.col) = -999
      g.ColMin(g.col) = 0
    ElseIf g.col = 6 Then
      g.ColMax(g.col) = -999
      g.ColMin(g.col) = 0
    ElseIf g.col = 7 Then
      g.ColMax(g.col) = -999
      g.ColMin(g.col) = 0
    ElseIf g.col = 8 Then
      g.ColMax(g.col) = -999
      g.ColMin(g.col) = 0
    ElseIf g.col = 9 Then
      g.ColMax(g.col) = -999
      g.ColMin(g.col) = 0
    ElseIf g.col = 10 Then
      g.ColMax(g.col) = -999
      g.ColMin(g.col) = 0
    ElseIf g.col = 11 Then
      g.ColMax(g.col) = 4
      g.ColMin(g.col) = 2
    ElseIf g.col = 18 Then
      g.addvalue "MI"
      g.addvalue "HR"
      g.addvalue "DY"
      g.addvalue "MO"
      g.addvalue "YR"
    ElseIf g.col = 19 Then
      g.ColMax(g.col) = -999
      g.ColMin(g.col) = 0
    ElseIf g.col = 20 Then
      g.ColMax(g.col) = -999
      g.ColMin(g.col) = 0
    End If
  ElseIf Index = 2 Then
    'distributes
    If g.col = 0 Then
      g.ColMax(g.col) = -999
      g.ColMin(g.col) = 0
    ElseIf g.col = 1 Then
      g.ColMax(g.col) = 10
      g.ColMin(g.col) = 1
    ElseIf g.col = 2 Then
      g.addvalue "MI"
      g.addvalue "HR"
      g.addvalue "DY"
      g.addvalue "MO"
      g.addvalue "YR"
    ElseIf g.col = 3 Then
      g.ColMax(g.col) = -999
      g.ColMin(g.col) = 0
    ElseIf g.col = 4 Then
      g.addvalue "SKIP"
      g.addvalue "SHIFT"
      g.addvalue "ACCUM"
    Else
      g.ColMax(g.col) = -999
      g.ColMin(g.col) = 0
    End If
  ElseIf Index = 3 Then
    'uvname
    If g.col = 1 Then
      g.ColMax(g.col) = -999
      g.ColMin(g.col) = 1
    ElseIf g.col = 7 Then
      g.addvalue "QUAN"
      g.addvalue "MOVT"
      g.addvalue "MOV1"
      g.addvalue "MOV2"
    ElseIf g.col = 13 Then
      g.addvalue ""
      g.addvalue "QUAN"
      g.addvalue "MOVT"
      g.addvalue "MOV1"
      g.addvalue "MOV2"
    End If
  ElseIf Index = 4 Then
    'User Defn Quan
    If g.col = 1 Then
      'valid operation types
      For Each vOpnBlk In myUci.OpnBlks
        Set lopnblk = vOpnBlk
        If lopnblk.Count > 0 Then
          If lopnblk.Name = "PERLND" Or lopnblk.Name = "IMPLND" Or _
             lopnblk.Name = "RCHRES" Or lopnblk.Name = "PLTGEN" Or _
             lopnblk.Name = "COPY" Or lopnblk.Name = "GENER" Then
            g.addvalue lopnblk.Name
          End If
        End If
      Next vOpnBlk
    ElseIf g.col = 2 Then
      Call SetOperationMinMax(g, myUci, 1)
    ElseIf g.col = 7 Then
      g.ColMax(g.col) = 4
      g.ColMin(g.col) = 2
    ElseIf g.col = 9 Then
      g.addvalue "MI"
      g.addvalue "HR"
      g.addvalue "DY"
      g.addvalue "MO"
      g.addvalue "YR"
    ElseIf g.col = 10 Then
      g.ColMax(g.col) = -999
      g.ColMin(g.col) = 0
    ElseIf g.col = 11 Then
      g.addvalue "MI"
      g.addvalue "HR"
      g.addvalue "DY"
      g.addvalue "MO"
      g.addvalue "YR"
    ElseIf g.col = 12 Then
      g.ColMax(g.col) = -999
      g.ColMin(g.col) = 0
    ElseIf g.col = 13 Then
      g.addvalue "SUM"
      g.addvalue "AVER"
      g.addvalue "MAX"
      g.addvalue "MIN"
    End If
  ElseIf Index = 5 Then
    'conditional
  End If
End Sub

Private Sub SetLimitsWDM(g As Control, myUci As HspfUci)
  Dim myFiles As HspfFilesBlk
  Dim i&, s$
  
  Set myFiles = myUci.FilesBlock
  For i = 1 To myFiles.Count
    s = myFiles.Value(i).Typ
    If Left(s, 3) = "WDM" Then
      g.addvalue s
    End If
  Next i
End Sub

Private Sub CheckValidDsn(g As Control, myUci As HspfUci)
  Dim dsnObj As ATCclsTserData
  Dim s$, wdmid$, dsn&
  
  s = g.Text
  If IsNumeric(s) Then
    dsn = CLng(s)
    wdmid = g.TextMatrix(g.row, g.col - 1)
    Set dsnObj = myUci.GetDataSetFromDsn(WDMInd(wdmid), dsn)
    If dsnObj Is Nothing Then
      'g.TextBackColor = vbRed
    Else
      g.TextBackColor = vbWhite
    End If
  Else
    g.TextBackColor = vbRed
    s = ""
  End If
End Sub

Private Sub CheckValidMemberName(g As Control, myUci As HspfUci)
  Dim dsnObj As ATCclsTserData
  Dim s$, wdmid$, dsn&
  
  s = g.Text
  dsn = g.TextMatrix(g.row, g.col - 1)
  wdmid = g.TextMatrix(g.row, g.col - 2)
  Set dsnObj = myUci.GetDataSetFromDsn(WDMInd(wdmid), dsn)
  If Not dsnObj Is Nothing Then
    g.addvalue dsnObj.Attrib("TSTYPE")
  End If
End Sub

Private Sub SetValidOperations(g As Control, myUci As HspfUci)

  Dim vOpnBlk As Variant, lopnblk As HspfOpnBlk
  
  For Each vOpnBlk In myUci.OpnBlks
    Set lopnblk = vOpnBlk
    If lopnblk.Count > 0 Then
      g.addvalue lopnblk.Name
    End If
  Next vOpnBlk
End Sub

Private Sub SetAllOperations(g As Control, myUci As HspfUci)

  Dim vOpnBlk As Variant, lopnblk As HspfOpnBlk
  
  For Each vOpnBlk In myUci.OpnBlks
    Set lopnblk = vOpnBlk
    g.addvalue lopnblk.Name
  Next vOpnBlk
End Sub

Private Sub SetOperationMinMax(g As Control, myUci As HspfUci, firstlast&)
  
  Dim lopnblk As HspfOpnBlk
  Dim i&, itmp&
  
  If Len(g.TextMatrix(g.row, g.col - firstlast)) > 0 Then
    Set lopnblk = myUci.OpnBlks(Trim(g.TextMatrix(g.row, g.col - firstlast)))
    If lopnblk.Count > 0 Then
      For i = 1 To lopnblk.Count
        g.addvalue lopnblk.Ids(i).Id
      Next i
    End If
  End If
  
End Sub

Private Sub SetGroupNames(g As Control, myMsg As HspfMsg, opercol&)
  Dim vGroup As Variant, lGroup As HspfTSGroupDef
  Dim i&, s$, soper$, ioper&
  
  soper = Trim(g.TextMatrix(g.row, g.col - opercol))
  If Len(soper) > 0 Then
    ioper = HspfOperNum(soper)
    If ioper > 0 Then
      For Each vGroup In myMsg.TSGroupDefs
        Set lGroup = vGroup
        If lGroup.BlockId = 120 + ioper Then
          s = lGroup.Name
          g.addvalue s
        End If
      Next vGroup
    End If
  End If
End Sub

Private Sub SetMemberNames(g As Control, myMsg As HspfMsg, opercol&)
  Dim vMember As Variant, lMember As HspfTSMemberDef
  Dim i&, s$, sgroup$, skey$, soper$, ioper&
  Dim vGroup As Variant, lGroup As HspfTSGroupDef
  
  soper = Trim(g.TextMatrix(g.row, g.col - opercol))
  sgroup = Trim(g.TextMatrix(g.row, g.col - 1))
  If Len(soper) > 0 Then
    ioper = HspfOperNum(soper)
    If ioper > 0 Then
      For Each vGroup In myMsg.TSGroupDefs
        Set lGroup = vGroup
        If lGroup.BlockId = 120 + ioper And lGroup.Name = sgroup Then
          'this is the one we want
          skey = CStr(lGroup.Id)
          For Each vMember In myMsg.TSGroupDefs(skey).MemberDefs
            Set lMember = vMember
            s = lMember.Name
            g.addvalue s
          Next vMember
          Exit Sub
        End If
      Next vGroup
    End If
  End If
End Sub

Private Sub SetValidMassLinks(g As Control, myUci As HspfUci)

  Dim vMassLink As Variant, lMassLink As HspfMassLink
  Dim tMassLinks$(), tcnt&, i&, ifound As Boolean
  
  tcnt = 0
  
  For Each vMassLink In myUci.MassLinks
    Set lMassLink = vMassLink
    ifound = False
    For i = 1 To tcnt
      If tMassLinks(i) = lMassLink.MassLinkID Then
        ifound = True
      End If
    Next i
    If ifound = False Then
      tcnt = tcnt + 1
      ReDim Preserve tMassLinks(tcnt)
      tMassLinks(tcnt) = lMassLink.MassLinkID
    End If
  Next vMassLink
  
  For i = 1 To tcnt
    g.addvalue tMassLinks(i)
  Next i
End Sub

Private Sub SetValidTrans(g As Control)
  g.addvalue " " 'allow default blank
  g.addvalue "SAME"
  g.addvalue "AVER"
  g.addvalue "DIV "
  g.addvalue "INTP"
  g.addvalue "LAST"
  g.addvalue "MAX "
  g.addvalue "MIN "
  g.addvalue "SUM "
End Sub

Public Sub CheckDataSetExistance(g As Control, myUci As HspfUci, retcod&)
  Dim dsnObj As ATCclsTserData
  Dim dsn$, wdmid$, i&
  
  NoDsnCount = 0
  ReDim NoDsn(g.rows)
  ReDim cwdmid(g.rows)
  ReDim scen(g.rows)
  ReDim locn(g.rows)
  ReDim cons(g.rows)
  
  For i = 1 To g.rows
    'does this wdm data set exist
    dsn = g.TextMatrix(i, 9)
    wdmid = g.TextMatrix(i, 8)
    If Len(dsn) > 0 And Len(wdmid) > 0 Then
      Set dsnObj = myUci.GetDataSetFromDsn(WDMInd(wdmid), CInt(dsn))
      If dsnObj Is Nothing Then
        'does not exist
        NoDsnCount = NoDsnCount + 1
        NoDsn(NoDsnCount) = dsn
        cwdmid(NoDsnCount) = wdmid
        scen(NoDsnCount) = UCase(FilenameOnly(myUci.Name))
        locn(NoDsnCount) = g.TextMatrix(i, 0) & g.TextMatrix(i, 1)
        cons(NoDsnCount) = g.TextMatrix(i, 10)
      End If
    End If
  Next i
  
  retcod = -1 'add dataset window not needed
  If NoDsnCount > 0 Then
    Set frmAddDataSet.icon = myUci.icon
    Call frmAddDataSet.SetUCI(myUci)
    frmAddDataSet.Show 1
    retcod = resp 'return code from add data set, 0-ok, 1-cancel
  End If
End Sub

Public Sub GetNonExistentDataSetInfo(i&, adsn&, wid$, s$, l$, c$)
  
  adsn = NoDsn(i)
  wid = cwdmid(i)
  s = scen(i)
  l = locn(i)
  c = cons(i)
End Sub

Public Sub GetNonExistentDataSetCount(n&)
  n = NoDsnCount
End Sub

Public Sub UpdateRespFromAddDataSet(i%)
  '1 - cancel from 'add data set'
  '0 - ok from 'add data set'
  '-1 - 'add data set' not needed
  resp = i
End Sub


