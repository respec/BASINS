Attribute VB_Name = "modWinHSPF"
Option Explicit

Public Sub ConvertNetworkToSchematic()
    Dim ifound&
    Dim vConn As Variant, lConn As HspfConnection
    Dim vSchConn As Variant, lSchConn As HspfConnection
    Dim vML As Variant, lML As HspfMassLink, i&
    Dim lOpn As HspfOperation, vOpn As Variant
    
    i = 1
    For Each vConn In myUci.Connections
      Set lConn = vConn
      If lConn.typ = 2 Then 'network entry
        
        'convert to schematic
        If (lConn.Source.volname = "PERLND" Or _
            lConn.Source.volname = "IMPLND" Or _
            lConn.Source.volname = "RCHRES") And _
            lConn.Target.volname = "RCHRES" Then
          'only convert these at this point
          ifound = 0
          For Each vSchConn In myUci.Connections
            Set lSchConn = vSchConn
            If lSchConn.typ = 3 Then 'schematic record
              If lSchConn.Source.volname = lConn.Source.volname And _
                 lSchConn.Source.volid = lConn.Source.volid And _
                 lSchConn.Target.volname = lConn.Target.volname And _
                 lSchConn.Target.volid = lConn.Target.volid Then
                ifound = 1
              End If
            End If
          Next vSchConn
          If ifound = 0 Then
            'add this record to schematic block
            Dim newConn As New HspfConnection
            Dim newSource As New HspfSrcTar
            Dim newTarget As New HspfSrcTar
            Set newSource.Opn = lConn.Source.Opn
            Set newTarget.Opn = lConn.Target.Opn
            Set newConn.Source = newSource
            Set newConn.Target = newTarget
            newConn.typ = 3
            If (lConn.Source.group = "PWATER" And _
                lConn.Source.member = "PERO") Or _
               (lConn.Source.group = "IWATER" And _
                lConn.Source.member = "SURO") Then
              'need to mult area by 12
              newConn.MFact = lConn.MFact * 12#
            Else
              newConn.MFact = lConn.MFact
            End If
            newConn.Source.volname = lConn.Source.volname
            newConn.Source.volid = lConn.Source.volid
            newConn.Target.volname = lConn.Target.volname
            newConn.Target.volid = lConn.Target.volid
            If lConn.Source.volname = "PERLND" Then
              newConn.MassLink = 1
            ElseIf lConn.Source.volname = "IMPLND" Then
              newConn.MassLink = 2
            ElseIf lConn.Source.volname = "RCHRES" Then
              newConn.MassLink = 3
            End If
            Set newConn.Uci = myUci
            myUci.Connections.Add newConn
            Set newConn = Nothing
            Set newSource = Nothing
            Set newTarget = Nothing
          End If
          
          'add this record to mass link list
          ifound = 0
          For Each vML In myUci.MassLinks
            Set lML = vML
            If lConn.Source.volname = lML.Source.volname And _
               lConn.Source.group = lML.Source.group And _
               lConn.Source.member = lML.Source.member And _
               lConn.Target.volname = lML.Target.volname And _
               lConn.Target.group = lML.Target.group And _
               lConn.Target.member = lML.Target.member Then
              ifound = 1
            End If
          Next vML
          If ifound = 0 Then
            'not already in list
            Dim newML As New HspfMassLink
            newML.Source.volname = lConn.Source.volname
            newML.Source.group = lConn.Source.group
            newML.Source.member = lConn.Source.member
            newML.Tran = lConn.Tran
            newML.Target.volname = lConn.Target.volname
            newML.Target.group = lConn.Target.group
            newML.Target.member = lConn.Target.member
            If lConn.Source.volname = "PERLND" Then
              newML.MassLinkID = 1
              If lConn.Source.group = "PWATER" And _
                 lConn.Source.member = "PERO" Then
                newML.MFact = 0.0833333
              End If
            ElseIf lConn.Source.volname = "IMPLND" Then
              newML.MassLinkID = 2
              If lConn.Source.group = "IWATER" And _
                 lConn.Source.member = "SURO" Then
                newML.MFact = 0.0833333
              End If
            ElseIf lConn.Source.volname = "RCHRES" Then
              newML.MassLinkID = 3
            End If
            Set newML.Uci = myUci
            myUci.MassLinks.Add newML
            Set newML = Nothing
          End If
            
          'now delete this connection
          myUci.Connections.Remove i
        Else
          i = i + 1
        End If
      Else
        i = i + 1
      End If
    Next vConn
    
    'set timser connections
    For i = 1 To myUci.OpnSeqBlock.Opns.Count
      'remove sources from opns
      Set lOpn = myUci.OpnSeqBlock.Opn(i)
      Do While lOpn.Sources.Count > 0
        lOpn.Sources.Remove 1
      Loop
      'remove targets from opns
      Do While lOpn.targets.Count > 0
        lOpn.targets.Remove 1
      Loop
    Next i
    For Each vOpn In myUci.OpnSeqBlock.Opns
      Set lOpn = vOpn
      lOpn.setTimSerConnections
    Next vOpn
    myUci.Source2MetSeg
End Sub

Public Sub CheckSchematic(schemfound As Boolean, netfound As Boolean)
  Dim vConn As Variant, lConn As HspfConnection
  
  schemfound = False
  netfound = False
  For Each vConn In myUci.Connections
    Set lConn = vConn
    If lConn.typ = 3 Then
      schemfound = True
    ElseIf lConn.typ = 2 Then
      netfound = True
    End If
  Next vConn
  
End Sub

Public Sub ConvertMutsin(s$, mutfileunit&, frompoint&, retc&)
  Dim facilityname$, retcod&, nconstits$, consnames$(), ndates&
  Dim jdates!(), rloads!(), curgroup$, curmember$, newwdmid$, newdsn&
  Dim sen$, loc$, con$, stanam$, tstype$, awdmid$(), adsn&()
  Dim dashpos&, i&, rtemp!(), j&, mutid&, rchid&, longloc$, Desc$
  Dim lOpnBlk As HspfOpnBlk, hs!, he!, SDate&(5), EDate&(5)
  Dim loper As HspfOperation, vOper As Variant
  Dim myFiles As hspffilesblk
  Dim lConn As HspfConnection
  Dim iresp As Long
  
  Call ReadMutsin(s, facilityname, retcod, nconstits, consnames(), _
                  ndates, jdates(), rloads())
                      
  'make sure this span covers the span of the run
  For i = 0 To 5
    SDate(i) = myUci.GlobalBlock.SDate(i)
    EDate(i) = myUci.GlobalBlock.EDate(i)
  Next i
  hs = Date2J(SDate)
  he = Date2J(EDate)
  iresp = 1
  retc = 1
  If hs < jdates(1) Or he > jdates(ndates) Then
    'time span of mutsin file shorter than time span of run
    iresp = myMsgBox.Show("The time span of the simulation is not within the time " & _
                          vbCrLf & "span of the point sources in MUTSIN file " & s & "." & _
                          vbCrLf & vbCrLf & "Do you want to convert this MUTSIN file anyway?", _
                          "Convert MUTSIN", "+&Yes", "-&No")
  End If
  If iresp = 1 Then
    'connect mutsin file unit with mustin operation
    retc = 0
    Set lOpnBlk = myUci.OpnBlks("MUTSIN")
    mutid = 0
    i = 1
    Do While i <= lOpnBlk.Count
      Set loper = lOpnBlk.NthOper(i)
      If loper.tables("MUTSINFO").Parms("MUTFL") = mutfileunit Then
        mutid = loper.Id   'this is the mutsin id associated with this file
        Exit Do
      End If
      i = i + 1
    Loop
    rchid = 0
    If mutid > 0 Then
      'connect mutsin operation with a rchres
      i = 1
      Do While i <= myUci.Connections.Count
        Set lConn = myUci.Connections(i)
        If lConn.typ = 2 And lConn.Source.volname = "MUTSIN" And _
          lConn.Source.volid = mutid And lConn.Target.volname = "RCHRES" Then
          rchid = lConn.Target.volid
          Desc = lConn.Target.Opn.Description
          loc = "RCH" & CStr(rchid)
          Exit Do
        Else
          i = i + 1
        End If
      Loop
    Else
      loc = ""
    End If
    
    'build new dsns
    sen = "PT-OBS"
    ReDim awdmid(nconstits)
    ReDim adsn(nconstits)
    For i = 1 To nconstits
      con = UCase(consnames(i))
      stanam = UCase(facilityname)
      tstype = Mid(con, 1, 4)
      ReDim rtemp(ndates)
      For j = 1 To ndates
        rtemp(j) = rloads(((j - 1) * nconstits) + i)
      Next j
      myUci.AddPointSourceDataSet sen, loc, con, stanam, _
         tstype, ndates, jdates, rtemp, newwdmid, newdsn
      awdmid(i) = newwdmid
      adsn(i) = newdsn
      If frompoint = 1 Then
        'add to master point list
        longloc = "RCHRES " & rchid & " - " & Desc
        frmPoint.UpdateListsForNewPointSource sen, stanam, loc, con, newwdmid, _
          newdsn, "RCHRES", CInt(Mid(loc, 4)), longloc
      End If
    Next i
    
    If mutid > 0 Then
      'look through connections to get group and member names
      i = 1
      j = 1
      Do While i <= myUci.Connections.Count
        Set lConn = myUci.Connections(i)
        If lConn.typ = 2 And lConn.Source.volname = "MUTSIN" And _
          lConn.Source.volid = mutid And lConn.Target.volname = "RCHRES" Then
          curgroup = lConn.Target.group
          curmember = lConn.Target.member
          If j <= nconstits Then
            newwdmid = awdmid(j)
            newdsn = adsn(j)
            j = j + 1
          Else
            newwdmid = awdmid(1)
            newdsn = adsn(1)
          End If
          If frompoint = 0 Then
            'add to point source structure
            myUci.AddPoint newwdmid, newdsn, rchid, stanam, _
                          curgroup, curmember, 0, 0
          End If
        End If
        i = i + 1
      Loop
      
      'delete mutsin operation
      myUci.DeleteOperation "MUTSIN", mutid
    End If
  End If

End Sub

Public Sub ReadMutsin(newname$, facname$, ret&, ncons$, cons$(), _
  ndate&, jdates!(), rVal!())
  
  Dim delim$, quote$, lstr$, i&, tname$, amax&, tstr$, tcnt&
  Dim reccnt&, ilen&, nhead&, j&, idate&(6)

  ret = 0
  delim = " "
  quote = """"
  
  'read mut file
  i = FreeFile(0)
  On Error GoTo ErrHandler
  Open newname For Input As #i
  reccnt = 0
  
  Line Input #i, lstr  'line with number of header lines
  reccnt = reccnt + 1
  ilen = Len(lstr)
  If IsNumeric(Mid(lstr, 43, 5)) Then
    nhead = CInt(Mid(lstr, 43, 5))
  Else
    nhead = 25
  End If
  
  Line Input #i, lstr  'line with facility name
  reccnt = reccnt + 1
  facname = Trim(lstr)
  
  For j = 1 To 5
    Line Input #i, lstr  'unused lines
    reccnt = reccnt + 1
  Next j
  
  ncons = 0
  Do Until Len(lstr) = 0
    Line Input #i, lstr  'read cons
    reccnt = reccnt + 1
    If Len(Trim(lstr)) > 0 Then
      ncons = ncons + 1
      ReDim Preserve cons(ncons)
      cons(ncons) = Trim(lstr)
    End If
  Loop
  
  For j = reccnt + 1 To nhead
    Line Input #i, lstr  'unused lines
  Next j
  
  ndate = 0
  Do Until EOF(i)
    Line Input #i, lstr
    ndate = ndate + 1
    ReDim Preserve jdates(ndate)
    ReDim Preserve rVal(ndate * ncons)
    idate(0) = StrSplit(lstr, delim, quote)
    idate(1) = StrSplit(lstr, delim, quote)
    idate(2) = StrSplit(lstr, delim, quote)
    idate(3) = StrSplit(lstr, delim, quote)
    idate(4) = StrSplit(lstr, delim, quote)
    idate(5) = 0
    jdates(ndate) = Date2J(idate)
    For j = 1 To ncons
      rVal(((ndate - 1) * ncons) + j) = StrSplit(lstr, delim, quote)
    Next j
  Loop
  Close #i
  Exit Sub
ErrHandler:
  Call MsgBox("Problem reading file " & tname, , "Convert Problem")
  ret = 1
End Sub

