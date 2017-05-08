Attribute VB_Name = "modCreateUci"
Option Explicit
'Copyright 2002 AQUA TERRA Consultants - Royalty-free use permitted under open source license

'land use file
Dim LURecCnt&, LUName$(), LUType&(), LUReach$(), LUArea!()
Dim LUSlope!(), LUDistance!(), luoper$(), luid&()
'reach file
Dim RCHRecCnt&, RCHId$(), RCHName$(), RCHWsid$(), RCHNexits&(), RCHType$()
Dim RCHLength!(), RCHDelth!(), RCHElev!(), RCHDownID$()
Dim RCHDepth!(), RCHWidth!(), RCHMann!(), rchorder&()
'channel file
Dim chanid$(), ChanL!(), ChanYm!(), ChanWm!(), ChanN!(), ChanS!()
Dim ChanM11!(), ChanM12!(), ChanYc!(), ChanM21!(), ChanM22!()
Dim ChanYt1!(), ChanYt2!(), ChanM31!(), ChanM32!(), ChanW11!()
Dim ChanW12!(), ChanRecCnt&

Dim mGlobalBlock As HspfGlobalBlk
Dim mFilesBlock As HspfFilesBlk
Dim mOpnSeqBlock As HspfOpnSeqBlk
Dim mOpnBlks As Collection 'of hspfopnblk
Dim mConnections As Collection 'of hspfconnection
Dim mMassLinks As Collection 'of hspfmasslink

Dim lastseg&(2), landname$(2), firstseg&(2)

Dim FacilityCount&
Dim FacilityName() As String
Dim FacilityNpdes() As String
Dim FacilityReach() As String
Dim FacilityMile() As Single

Dim PollutantCount&
Dim PollutantFacID&()
Dim PollutantName() As String
Dim PollutantLoad() As Single

Public Sub CreateUciFromBASINS(newUci As HspfUci, M As HspfMsg, _
    newName As String, outputwdm$, metwdms$(), wdmids$(), _
    MetDataDetails$, oneseg As Boolean, MasterPollutantList As Collection)
    
  Dim s$, ret&, basedsn&, i&
  Dim delim$, quote$
  Dim lOpnName$
  Dim lopnblk As HspfOpnBlk, vOpnBlk As Variant
  Dim lOpn As HspfOperation, vOpn As Variant
  Dim SDate&(6), EDate&(6), metwdmid$
  
    
  If Len(Dir(newName)) = 0 Then
    newUci.ErrorDescription = "WsdFileName '" & newName & "' not found"
  Else
    newUci.Name = Left(newName, Len(newName) - 3) & "uci"
  End If
  
  'newUci.SetMessageUnit
  Set newUci.Msg = M

  If Len(newUci.Name) > 0 Then
  
    s = FilenameOnly(newUci.Name)
    
    Call ReadWSDFile(newName, ret)
    Call ReadRCHFile(newName, ret)
    Call ReadPTFFile(newName, ret)
    Call ReadPSRFile(newName, ret)
    On Error Resume Next
    
    If ret = 0 Then
      'everything read okay, continue
    
      'get details from the met data details
      delim = ","
      quote = """"
      basedsn = StrSplit(MetDataDetails, delim, quote)
      For i = 0 To 5
        SDate(i) = StrSplit(MetDataDetails, delim, quote)
      Next i
      For i = 0 To 5
        EDate(i) = StrSplit(MetDataDetails, delim, quote)
      Next i
      metwdmid = MetDataDetails
      
      'add global block to empty uci
      newUci.Initialized = True
      Set mGlobalBlock = New HspfGlobalBlk
      Set mGlobalBlock.Uci = newUci
      mGlobalBlock.RunInf.Value = "UCI Created by WinHSPF for " & s
      mGlobalBlock.emfg = 1
      mGlobalBlock.outlev.Value = 1
      mGlobalBlock.runfg = 1
      For i = 0 To 5
        mGlobalBlock.SDate(i) = SDate(i)
        mGlobalBlock.EDate(i) = EDate(i)
      Next i
      newUci.GlobalBlock = mGlobalBlock
      
      'add files block to uci
      Set mFilesBlock = New HspfFilesBlk
      CreateFilesBlock s, outputwdm, metwdms, wdmids
      Set mFilesBlock.Uci = newUci
      newUci.FilesBlock = mFilesBlock
      
      'add opn seq block
      Set mOpnSeqBlock = New HspfOpnSeqBlk
      Set mOpnSeqBlock.Uci = newUci
      mOpnSeqBlock.Delt = 60
      'add recs for each operation
      CreateOpnSeqBlock newUci, oneseg
      mOpnSeqBlock.Uci = newUci
      newUci.OpnSeqBlock = mOpnSeqBlock
      
      'set all operation types
      Set mOpnBlks = New Collection 'of hspfopnblk
      i = 1
      lOpnName = HspfOperName(i)
      While lOpnName <> "UNKNOWN"
        Set lopnblk = New HspfOpnBlk
        lopnblk.Name = lOpnName
        Set lopnblk.Uci = newUci
        mOpnBlks.Add lopnblk, lOpnName
        i = i + 1
        lOpnName = HspfOperName(i)
      Wend
      newUci.OpnBlks = mOpnBlks
      
      'create tables for each operation
      For Each vOpn In mOpnSeqBlock.Opns
        Set lOpn = vOpn
        Set lopnblk = mOpnBlks(lOpn.Name)
        lopnblk.Ids.Add lOpn, "K" & lOpn.Id
        Set lOpn.OpnBlk = lopnblk
      Next vOpn
      For Each vOpnBlk In mOpnBlks 'perlnd, implnd, etc
        Set lopnblk = vOpnBlk
        If lopnblk.Count > 0 Then Call lopnblk.createTables(M.BlockDefs(lopnblk.Name))
      Next vOpnBlk
      
      For i = 1 To ChanRecCnt  'process ftables
        Set lOpn = newUci.OpnBlks("RCHRES").OperFromID(CInt(chanid(i)))
        If Not lOpn Is Nothing Then
          lOpn.FTable.FTableFromCrossSect ChanL(i), ChanYm(i), ChanWm(i), _
            ChanN(i), ChanS(i), ChanM11(i), ChanM12(i), ChanYc(i), ChanM21(i), ChanM22(i), _
            ChanYt1(i), ChanYt2(i), ChanM31(i), ChanM32(i), ChanW11(i), ChanW12(i)
        End If
      Next i
      
      Set mConnections = New Collection 'of hspfconnections
      'create schematic, ext src blocks
      CreateConnectionsSchematic newUci
      CreateConnectionsMet newUci
      newUci.Connections = mConnections
      
      'set timeser connections
      For Each vOpn In mOpnSeqBlock.Opns
        Set lOpn = vOpn
        lOpn.setTimSerConnections
      Next vOpn
      
      'create masslinks
      Set mMassLinks = New Collection 'of hspfmasslinks
      CreateMassLinks newUci
      newUci.MassLinks = mMassLinks
  
      'set initial values in uci from basins values
      SetInitValues
      
      CreatePointSourceDSNs newUci, MasterPollutantList
      
      CreateDefaultOutput newUci
      CreateBinaryOutput newUci, s
      
      'look for met segments
      'newUci.Source2MetSeg
    End If
  End If
  
  newUci.Edited = False 'all the reads set edited

End Sub

Private Sub ReadWSDFile(newName As String, ret&)
  Dim delim$, quote$, lstr$, i&, tname$, amax&, tstr$, tcnt&

  ret = 0
  delim = " "
  quote = """"
  
  'read wsd file
  i = FreeFile(0)
  On Error GoTo ErrHandler
  tname = Left(newName, Len(newName) - 3) & "wsd"
  Open tname For Input As #i
  Line Input #i, lstr  'header line
  LURecCnt = 0
  ReDim LUName(1)
  ReDim LUType(1)
  ReDim LUReach(1)
  ReDim LUArea(1)
  ReDim LUSlope(1)
  ReDim LUDistance(1)
  ReDim luoper(1)
  ReDim luid(1)
  Do Until EOF(i)
    Line Input #i, lstr
    LURecCnt = LURecCnt + 1
    amax = UBound(LUName)
    If LURecCnt > amax Then
      ReDim Preserve LUName(amax * 2)
      ReDim Preserve LUType(amax * 2)
      ReDim Preserve LUReach(amax * 2)
      ReDim Preserve LUArea(amax * 2)
      ReDim Preserve LUSlope(amax * 2)
      ReDim Preserve LUDistance(amax * 2)
      ReDim Preserve luoper(amax * 2)
      ReDim Preserve luid(amax * 2)
    End If
    'count the number of fields in this string
    tstr = lstr
    tcnt = 0
    Do While Len(StrSplit(tstr, delim, quote)) > 0
      tcnt = tcnt + 1
    Loop
    If tcnt = 6 Then
      'this is the normal way
      LUName(LURecCnt) = StrSplit(lstr, delim, quote)
      LUType(LURecCnt) = StrSplit(lstr, delim, quote)
      LUReach(LURecCnt) = StrSplit(lstr, delim, quote)
      LUArea(LURecCnt) = StrSplit(lstr, delim, quote)
      LUSlope(LURecCnt) = StrSplit(lstr, delim, quote)
      LUDistance(LURecCnt) = StrSplit(lstr, delim, quote)
    Else
      'if coming from old delineator might not be space delimited
      LUName(LURecCnt) = StrSplit(lstr, delim, quote)
      If Len(lstr) > 23 Then
        LUDistance(LURecCnt) = Mid(lstr, Len(lstr) - 7, 8)
        LUSlope(LURecCnt) = Mid(lstr, Len(lstr) - 15, 8)
        LUArea(LURecCnt) = Mid(lstr, Len(lstr) - 23, 8)
      End If
      LUType(LURecCnt) = StrSplit(lstr, delim, quote)
      LUReach(LURecCnt) = StrSplit(lstr, delim, quote)
    End If
  Loop
  Close #i
  Exit Sub
ErrHandler:
  Call MsgBox("Problem reading file " & tname, , "Create Problem")
  ret = 1
End Sub

Private Sub ReadRCHFile(newName As String, ret&)
  Dim delim$, quote$, lstr$, i&, tname$, amax&, tstr$

  ret = 0
  delim = " "
  quote = """"
  
  'read rch file
  i = FreeFile(0)
  On Error GoTo ErrHandler
  tname = Left(newName, Len(newName) - 3) & "rch"
  Open tname For Input As #i
  Line Input #i, lstr  'header line
  RCHRecCnt = 0
  ReDim RCHId(1)
  ReDim RCHName(1)
  ReDim RCHWsid(1)
  ReDim RCHNexits(1)
  ReDim RCHType(1)
  ReDim RCHLength(1)
  ReDim RCHDelth(1)
  ReDim RCHElev(1)
  ReDim RCHDownID(1)
  ReDim RCHDepth(1)
  ReDim RCHWidth(1)
  ReDim RCHMann(1)
  ReDim rchorder(1)
  Do Until EOF(i)
    Line Input #i, lstr
    RCHRecCnt = RCHRecCnt + 1
    amax = UBound(RCHId)
    If RCHRecCnt > amax Then
      ReDim Preserve RCHId(amax * 2)
      ReDim Preserve RCHName(amax * 2)
      ReDim Preserve RCHWsid(amax * 2)
      ReDim Preserve RCHNexits(amax * 2)
      ReDim Preserve RCHType(amax * 2)
      ReDim Preserve RCHLength(amax * 2)
      ReDim Preserve RCHDelth(amax * 2)
      ReDim Preserve RCHElev(amax * 2)
      ReDim Preserve RCHDownID(amax * 2)
      ReDim Preserve RCHDepth(amax * 2)
      ReDim Preserve RCHWidth(amax * 2)
      ReDim Preserve RCHMann(amax * 2)
      ReDim Preserve rchorder(amax * 2)
    End If
    RCHId(RCHRecCnt) = StrSplit(lstr, delim, quote)
    RCHName(RCHRecCnt) = StrSplit(lstr, delim, quote)
    RCHWsid(RCHRecCnt) = StrSplit(lstr, delim, quote)
    tstr = StrSplit(lstr, delim, quote)
    RCHNexits(RCHRecCnt) = StrSplit(lstr, delim, quote)
    tstr = StrSplit(lstr, delim, quote)
    RCHType(RCHRecCnt) = StrSplit(lstr, delim, quote)
    RCHLength(RCHRecCnt) = StrSplit(lstr, delim, quote)
    RCHDelth(RCHRecCnt) = StrSplit(lstr, delim, quote)
    RCHElev(RCHRecCnt) = StrSplit(lstr, delim, quote)
    tstr = StrSplit(lstr, delim, quote)
    tstr = StrSplit(lstr, delim, quote)
    RCHDownID(RCHRecCnt) = StrSplit(lstr, delim, quote)
    tstr = StrSplit(lstr, delim, quote)
    tstr = StrSplit(lstr, delim, quote)
    tstr = StrSplit(lstr, delim, quote)
    tstr = StrSplit(lstr, delim, quote)
    tstr = StrSplit(lstr, delim, quote)
    tstr = StrSplit(lstr, delim, quote)
    RCHDepth(RCHRecCnt) = StrSplit(lstr, delim, quote)
    RCHWidth(RCHRecCnt) = StrSplit(lstr, delim, quote)
    tstr = StrSplit(lstr, delim, quote)
    tstr = StrSplit(lstr, delim, quote)
    tstr = StrSplit(lstr, delim, quote)
    tstr = StrSplit(lstr, delim, quote)
    tstr = StrSplit(lstr, delim, quote)
    tstr = StrSplit(lstr, delim, quote)
    RCHMann(RCHRecCnt) = StrSplit(lstr, delim, quote)
    rchorder(RCHRecCnt) = RCHRecCnt
  Loop
  Close #i
  Exit Sub
ErrHandler:
  Call MsgBox("Problem reading file " & tname, , "Create Problem")
  ret = 2
End Sub

Public Sub ReadPTFFile(newName As String, ret&)

  Dim delim$, quote$, lstr$, i&, tname$, amax&, tstr$

  ret = 0
  delim = " "
  quote = """"

  'read ptf file for channel data
  i = FreeFile(0)
  On Error GoTo ErrHandler
  tname = Left(newName, Len(newName) - 3) & "ptf"
  Open tname For Input As #i
  Line Input #i, lstr  'header line
  ChanRecCnt = 0
  ReDim chanid(1)
  ReDim ChanL(1)
  ReDim ChanYm(1)
  ReDim ChanWm(1)
  ReDim ChanN(1)
  ReDim ChanS(1)
  ReDim ChanM11(1)
  ReDim ChanM12(1)
  ReDim ChanYc(1)
  ReDim ChanM21(1)
  ReDim ChanM22(1)
  ReDim ChanYt1(1)
  ReDim ChanYt2(1)
  ReDim ChanM31(1)
  ReDim ChanM32(1)
  ReDim ChanW11(1)
  ReDim ChanW12(1)
  Do Until EOF(i)
    Line Input #i, lstr
    ChanRecCnt = ChanRecCnt + 1
    amax = UBound(chanid)
    If ChanRecCnt > amax Then
      ReDim Preserve chanid(amax * 2)
      ReDim Preserve ChanL(amax * 2)
      ReDim Preserve ChanYm(amax * 2)
      ReDim Preserve ChanWm(amax * 2)
      ReDim Preserve ChanN(amax * 2)
      ReDim Preserve ChanS(amax * 2)
      ReDim Preserve ChanM11(amax * 2)
      ReDim Preserve ChanM12(amax * 2)
      ReDim Preserve ChanYc(amax * 2)
      ReDim Preserve ChanM21(amax * 2)
      ReDim Preserve ChanM22(amax * 2)
      ReDim Preserve ChanYt1(amax * 2)
      ReDim Preserve ChanYt2(amax * 2)
      ReDim Preserve ChanM31(amax * 2)
      ReDim Preserve ChanM32(amax * 2)
      ReDim Preserve ChanW11(amax * 2)
      ReDim Preserve ChanW12(amax * 2)
    End If
    chanid(ChanRecCnt) = StrSplit(lstr, delim, quote) 'reach id
    ChanL(ChanRecCnt) = StrSplit(lstr, delim, quote)  'length
    ChanYm(ChanRecCnt) = StrSplit(lstr, delim, quote) 'mean depth
    ChanWm(ChanRecCnt) = StrSplit(lstr, delim, quote) 'mean width
    ChanN(ChanRecCnt) = StrSplit(lstr, delim, quote)  'mann n
    ChanS(ChanRecCnt) = StrSplit(lstr, delim, quote)  'long slope
    If ChanS(ChanRecCnt) < 0.0001 Then
      ChanS(ChanRecCnt) = 0.0001
    End If
    tstr = StrSplit(lstr, delim, quote)
    ChanM31(ChanRecCnt) = StrSplit(lstr, delim, quote)  'side slope upper left
    ChanM21(ChanRecCnt) = StrSplit(lstr, delim, quote)  'side slope lower left
    ChanW11(ChanRecCnt) = StrSplit(lstr, delim, quote)  'zero slope width left
    ChanM11(ChanRecCnt) = StrSplit(lstr, delim, quote)  'side slope chan left
    ChanM12(ChanRecCnt) = StrSplit(lstr, delim, quote)  'side slope chan right
    ChanW12(ChanRecCnt) = StrSplit(lstr, delim, quote)  'zero slope width right
    ChanM22(ChanRecCnt) = StrSplit(lstr, delim, quote)  'side slope lower right
    ChanM32(ChanRecCnt) = StrSplit(lstr, delim, quote)  'side slope upper right
    ChanYc(ChanRecCnt) = StrSplit(lstr, delim, quote)  'channel depth
    ChanYt1(ChanRecCnt) = StrSplit(lstr, delim, quote)  'depth at slope change
    ChanYt2(ChanRecCnt) = StrSplit(lstr, delim, quote)  'channel max depth
  Loop
  Close #i
  Exit Sub
ErrHandler:
  Call MsgBox("Problem reading file " & tname, , "Create Problem")
  ret = 3
End Sub

Public Sub WritePTFFile(newName As String, chanid$, ArrayVals!())

  Dim delim$, quote$, lstr$, i&, tname$, amax&, tstr$, Id$
  
  i = Len(chanid)
  If i > 0 Then  'have a reach id
    'write ptf file
    i = FreeFile(0)
    On Error GoTo ErrHandler
    tname = newName
    Open tname For Output As #i
    lstr = "'Reach Number','Length(ft)','Mean Depth(ft)','Mean Width (ft)'," & _
           "'Mannings Roughness Coeff.','Long. Slope','Type of x-section','Side slope of upper FP left'," & _
           "'Side slope of lower FP left','Zero slope FP width left(ft)','Side slope of channel left'," & _
           "'Side slope of channel right','Zero slope FP width right(ft)','Side slope lower FP right'," & _
           "'Side slope upper FP right','Channel Depth(ft)','Flood side slope change at depth','Max. depth'," & _
           "'No. of exits','Fraction of flow through exit 1','Fraction of flow through exit 2'," & _
           "'Fraction of flow through exit 3','Fraction of flow through exit 4','Fraction of flow through exit 5'"
    Print #i, lstr  'header line
    lstr = chanid & " " & _
          ArrayVals(1) & " " & _
          ArrayVals(2) & " " & _
          ArrayVals(3) & " " & _
          ArrayVals(4) & " " & _
          ArrayVals(5) & " " & _
          "Trapezoidal" & " " & _
          ArrayVals(13) & " " & _
          ArrayVals(12) & " " & _
          ArrayVals(11) & " " & _
          ArrayVals(10) & " " & _
          ArrayVals(9) & " " & _
          ArrayVals(8) & " " & _
          ArrayVals(7) & " " & _
          ArrayVals(6) & " " & _
          ArrayVals(14) & " " & _
          ArrayVals(15) & " " & _
          ArrayVals(16) & " " & _
          "1 1 0 0 0 0"
    Print #i, lstr
    Close #i
    Exit Sub
  End If
ErrHandler:
End Sub

Public Sub GetPTFFileIds(cnt&, ArrayIds$())
  Dim j&
  
  cnt = ChanRecCnt
  ReDim ArrayIds(cnt)
  For j = 1 To cnt
    ArrayIds(j) = chanid(j)
  Next j
End Sub

Public Sub GetPTFData(RCHId$, ArrayVals!())
  Dim i&, Id$
  
  i = Len(RCHId)
  If i > 0 Then  'have a reach id
    Id = CInt(RCHId)
    For i = 1 To ChanRecCnt
      If Trim(chanid(i)) = Id Then  'found the one
        ArrayVals(1) = ChanL(i)
        ArrayVals(2) = ChanYm(i)
        ArrayVals(3) = ChanWm(i)
        ArrayVals(4) = ChanN(i)
        ArrayVals(5) = ChanS(i)
        ArrayVals(6) = ChanM32(i)
        ArrayVals(7) = ChanM22(i)
        ArrayVals(8) = ChanW12(i)
        ArrayVals(9) = ChanM12(i)
        ArrayVals(10) = ChanM11(i)
        ArrayVals(11) = ChanW11(i)
        ArrayVals(12) = ChanM21(i)
        ArrayVals(13) = ChanM31(i)
        ArrayVals(14) = ChanYc(i)
        ArrayVals(15) = ChanYt1(i)
        ArrayVals(16) = ChanYt2(i)
      End If
    Next i
  End If
End Sub

Private Sub ReadPSRFile(newName As String, ret&)

  Dim delim$, quote$, lstr$, i&, tname$, amax&, tstr$, j&

  ret = 0
  delim = " "
  quote = """"
  FacilityCount = 0
  PollutantCount = 0
  
  'read psr file for point source data
  i = FreeFile(0)
  On Error GoTo ErrHandler
  tname = Left(newName, Len(newName) - 3) & "psr"
  Open tname For Input As #i
  Line Input #i, lstr  'number of facilities
  If Len(lstr) > 0 Then
    FacilityCount = CInt(lstr)
  End If
  Line Input #i, lstr  'blank line
  Line Input #i, lstr  'header line
  
  If FacilityCount > 0 Then
    'have some facilities
    ReDim FacilityName(FacilityCount)
    ReDim FacilityNpdes(FacilityCount)
    ReDim FacilityReach(FacilityCount)
    ReDim FacilityMile(FacilityCount)
    For j = 1 To FacilityCount
      Line Input #i, lstr
      FacilityName(j - 1) = StrSplit(lstr, delim, quote)
      FacilityNpdes(j - 1) = StrSplit(lstr, delim, quote)
      FacilityReach(j - 1) = StrSplit(lstr, delim, quote)
      FacilityMile(j - 1) = CSng(StrSplit(lstr, delim, quote))
    Next j
    If Not EOF(i) Then Line Input #i, lstr  'blank line
    If Not EOF(i) Then Line Input #i, lstr  'header line
    PollutantCount = 0
    Do Until EOF(i)
      Line Input #i, lstr
      If Len(lstr) > 0 Then
        PollutantCount = PollutantCount + 1
        ReDim Preserve PollutantFacID(PollutantCount)
        ReDim Preserve PollutantName(PollutantCount)
        ReDim Preserve PollutantLoad(PollutantCount)
        PollutantFacID(PollutantCount - 1) = StrSplit(lstr, delim, quote)
        PollutantName(PollutantCount - 1) = StrSplit(lstr, delim, quote)
        PollutantLoad(PollutantCount - 1) = CSng(StrSplit(lstr, delim, quote))
      End If
    Loop
  End If
  Close #i
  Exit Sub
ErrHandler:
  Call MsgBox("Problem reading file " & tname, , "Create Problem")
  ret = 4
End Sub

Private Sub SetInitValues()
  Dim i&, j&, tstr$
  Dim lOpn As HspfOperation
  Dim ltable As HspfTable
  
  'set init values in uci
  For i = 1 To mOpnBlks("PERLND").Count
    Set lOpn = mOpnBlks("PERLND").Ids(i)
    Set ltable = lOpn.Tables("ACTIVITY")
    ltable.Parms("PWATFG") = 1
    j = 0
    Do Until lOpn.Id = luid(j) And luoper(j) = "PERLND"
      j = j + 1
      If lOpn.Id = luid(j) And luoper(j) = "PERLND" Then
        Set ltable = lOpn.Tables("GEN-INFO")
        ltable.Parms("LSID") = LUName(j)
        Set ltable = lOpn.Tables("PWAT-PARM2")
        If LUSlope(j) > 0 Then
          ltable.Parms("SLSUR") = LUSlope(j)
        Else
          ltable.Parms("SLSUR") = 0.001  'must have some slope
        End If
        'default lsur based on slsur
        ltable.Parms("LSUR") = DefaultLSURFromSLSUR(ltable.Parms("SLSUR"))
        'ltable.Parms("LSUR") = Round(LUDistance(j), 1)
      End If
    Loop
  Next i
  For i = 1 To mOpnBlks("IMPLND").Count
    Set lOpn = mOpnBlks("IMPLND").Ids(i)
    Set ltable = lOpn.Tables("ACTIVITY")
    ltable.Parms("IWATFG") = 1
    j = 0
    Do Until lOpn.Id = luid(j) And luoper(j) = "IMPLND"
      j = j + 1
      If lOpn.Id = luid(j) And luoper(j) = "IMPLND" Then
        Set ltable = lOpn.Tables("GEN-INFO")
        ltable.Parms("LSID") = LUName(j)
        Set ltable = lOpn.Tables("IWAT-PARM2")
        If LUSlope(j) > 0 Then
          ltable.Parms("SLSUR") = LUSlope(j)
        Else
          ltable.Parms("SLSUR") = 0.001  'must have some slope
        End If
        'default lsur based on slsur
        ltable.Parms("LSUR") = DefaultLSURFromSLSUR(ltable.Parms("SLSUR"))
        'ltable.Parms("LSUR") = Round(LUDistance(j), 1)
      End If
    Loop
  Next i
  For i = 1 To RCHRecCnt
    Set lOpn = mOpnBlks("RCHRES").Ids(i)
    Set ltable = lOpn.Tables("ACTIVITY")
    ltable.Parms("HYDRFG") = 1
    Set ltable = lOpn.Tables("GEN-INFO")
    tstr = RCHName(rchorder(i))
    j = Len(tstr)
    If j < 19 And (Not IsNumeric(RCHId(rchorder(i))) Or Len(RCHId(rchorder(i))) > 5) Then
      tstr = tstr & " " & Right(RCHId(rchorder(i)), 19 - j)
    End If
    ltable.Parms("RCHID") = tstr
    ltable.Parms("NEXITS") = RCHNexits(rchorder(i))
    ltable.Parms("PUNITE") = 91
    If RCHType(rchorder(i)) = "R" Then
      ltable.Parms("LKFG") = 1
    End If
    Set ltable = lOpn.Tables("HYDR-PARM1")
    ltable.Parms("AUX1FG") = 1
    ltable.Parms("AUX2FG") = 1
    ltable.Parms("AUX3FG") = 1
    ltable.Parms("ODFVF1") = 4
    Set ltable = lOpn.Tables("HYDR-PARM2")
    ltable.Parms("LEN") = RCHLength(rchorder(i))
    ltable.Parms("DELTH") = Round(RCHDelth(rchorder(i)), 0)
  Next i
End Sub

Private Sub CreateMassLinks(newUci As HspfUci)
  Dim lMassLink As HspfMassLink
  Dim lopnblk As HspfOpnBlk, i&
  
  For Each lopnblk In mOpnBlks
    If lopnblk.Name = "PERLND" Then
      Set lMassLink = New HspfMassLink
      Set lMassLink.Uci = newUci
      lMassLink.MassLinkID = 2
      lMassLink.Source.VolName = "PERLND"
      lMassLink.Source.VolId = 0
      lMassLink.Source.Group = "PWATER"
      lMassLink.Source.Member = "PERO"
      lMassLink.MFact = 0.0833333
      lMassLink.Tran = ""
      lMassLink.Target.VolName = "RCHRES"
      lMassLink.Target.VolId = 0
      lMassLink.Target.Group = "INFLOW"
      lMassLink.Target.Member = "IVOL"
      mMassLinks.Add lMassLink
      
    ElseIf lopnblk.Name = "IMPLND" Then
      Set lMassLink = New HspfMassLink
      Set lMassLink.Uci = newUci
      lMassLink.MassLinkID = 1
      lMassLink.Source.VolName = "IMPLND"
      lMassLink.Source.VolId = 0
      lMassLink.Source.Group = "IWATER"
      lMassLink.Source.Member = "SURO"
      lMassLink.MFact = 0.0833333
      lMassLink.Tran = ""
      lMassLink.Target.VolName = "RCHRES"
      lMassLink.Target.VolId = 0
      lMassLink.Target.Group = "INFLOW"
      lMassLink.Target.Member = "IVOL"
      mMassLinks.Add lMassLink
      
    ElseIf lopnblk.Name = "RCHRES" Then
      Set lMassLink = New HspfMassLink
      Set lMassLink.Uci = newUci
      lMassLink.MassLinkID = 3
      lMassLink.Source.VolName = "RCHRES"
      lMassLink.Source.VolId = 0
      lMassLink.Source.Group = "ROFLOW"
      lMassLink.Source.Member = ""
      lMassLink.MFact = 1#
      lMassLink.Tran = ""
      lMassLink.Target.VolName = "RCHRES"
      lMassLink.Target.VolId = 0
      lMassLink.Target.Group = "INFLOW"
      lMassLink.Target.Member = ""
      mMassLinks.Add lMassLink
    End If
  Next lopnblk
End Sub

Private Sub CreateConnectionsSchematic(newUci As HspfUci)
  Dim i&, j&, k&
  Dim lConnection As HspfConnection

  Set lConnection = New HspfConnection 'dummy to get entry point
  For i = 1 To RCHRecCnt
    For j = 1 To LURecCnt
      'add entries for each land use to each reach
      If RCHWsid(i) = LUReach(j) Then
        Set lConnection = New HspfConnection
        Set lConnection.Uci = newUci
        lConnection.Typ = 3
        lConnection.Source.VolName = luoper(j)
        lConnection.Source.VolId = luid(j)
        lConnection.MFact = LUArea(j)
        lConnection.Target.VolName = "RCHRES"
        For k = 1 To RCHRecCnt
          If rchorder(k) = i Then
            lConnection.Target.VolId = RCHId(rchorder(k))
          End If
        Next k
        lConnection.MassLink = LUType(j)
        mConnections.Add lConnection
      End If
    Next j
  Next i
  For i = 1 To RCHRecCnt
    'add entries for each reach to reach connection
    For j = 1 To RCHRecCnt
      If RCHId(rchorder(j)) = RCHDownID(rchorder(i)) Then
        Set lConnection = New HspfConnection
        Set lConnection.Uci = newUci
        lConnection.Typ = 3
        lConnection.Source.VolName = "RCHRES"
        lConnection.Source.VolId = RCHId(rchorder(i))
        lConnection.MFact = 1#
        lConnection.Target.VolName = "RCHRES"
        lConnection.Target.VolId = RCHId(rchorder(j))
        lConnection.MassLink = 3
        mConnections.Add lConnection
      End If
    Next j
  Next i
  Set lConnection = Nothing
End Sub

Private Sub CreateConnectionsMet(newUci As HspfUci)
  Dim vOpn As Variant, lOpn As HspfOperation
  Dim vOpTyps As Variant, vOpTyp As Variant
  
  vOpTyps = Array("PERLND", "IMPLND", "RCHRES") 'operations with assoc met segs
  For Each vOpTyp In vOpTyps
    For Each vOpn In newUci.OpnBlks(vOpTyp).Ids
      Set lOpn = vOpn
      Set lOpn.MetSeg = newUci.MetSegs(1)
    Next vOpn
  Next vOpTyp
  
End Sub

Private Sub CreateFilesBlock(s As String, outputwdm$, metwdms$(), wdmids$())
  Dim icnt&, i&
  Dim newFile As HspfFile
 
  mFilesBlock.Clear
  newFile.Name = s & ".ech"
  newFile.Typ = "MESSU"
  newFile.Unit = 24
  mFilesBlock.Add newFile
  newFile.Name = s & ".out"
  newFile.Typ = " "
  newFile.Unit = 91
  mFilesBlock.Add newFile
  icnt = 0
  If Len(outputwdm) > 0 Then
    newFile.Name = outputwdm
    icnt = icnt + 1
    newFile.Typ = wdmids(0)
    newFile.Unit = 25
    mFilesBlock.Add newFile
  End If
  For i = 1 To 3
    If Len(metwdms(i)) > 0 Then
      newFile.Name = metwdms(i)
      icnt = icnt + 1
      newFile.Typ = wdmids(i)
      newFile.Unit = 25 + i
      mFilesBlock.Add newFile
    End If
  Next i
End Sub
Private Sub CreateOpnSeqBlock(newUci As HspfUci, oneseg As Boolean)
  
  Dim j&, i&, toperid&, stringtofind$, itemp&
  Dim newOpn As HspfOperation
  Dim lOpn As HspfOperation, outoforder As Boolean

  If RCHRecCnt > 1 Then
    'reaches have to be in order
    outoforder = True
    Do Until Not outoforder
      outoforder = False
      i = 2
      Do Until i = RCHRecCnt + 1
        stringtofind = RCHDownID(rchorder(i))
        j = 1
        Do Until j = RCHRecCnt
          If RCHId(rchorder(j)) = stringtofind And j < i Then
            'reaches are out of order, swap places
            itemp = rchorder(i)
            rchorder(i) = rchorder(j)
            rchorder(j) = itemp
            outoforder = True
          Else
            j = j + 1
          End If
        Loop
        i = i + 1
      Loop
    Loop
  End If
  
  'add rec to opn seq block for each land use
  landname(2) = "PERLND"
  landname(1) = "IMPLND"
  If oneseg Then
    'only one segment for all land uses
    Call CreateOpnsForOneSeg(newUci)
  Else
    'user wants multiple segments
    Call CreateOpnsForMultSegs(newUci)
  End If
  
  'add record to opn seq block for each reach
  For i = 1 To RCHRecCnt
    'now add each rchres to opn seq block
    Set newOpn = New HspfOperation
    Set newOpn.Uci = newUci
    newOpn.Name = "RCHRES"
    If IsNumeric(RCHId(rchorder(i))) And Len(RCHId(rchorder(i))) < 5 Then
      newOpn.Description = RCHName(rchorder(i))
    Else
      newOpn.Description = RCHName(rchorder(i)) & " (" & RCHId(rchorder(i)) & ")"
    End If
    'newOpn.Id = i
    newOpn.Id = RCHId(rchorder(i))
    mOpnSeqBlock.Add newOpn
  Next i
End Sub

Private Sub CreateOpnsForOneSeg(newUci As HspfUci)

    Dim j, i
    Dim UniqueNameCount&, addflag As Boolean
    Dim newOpn As HspfOperation, toperid&
    Dim lOpn As HspfOperation
  
    For j = 2 To 1 Step -1
      UniqueNameCount = 0
      For i = 1 To LURecCnt
        If LUType(i) = j Then
          If UniqueNameCount = 0 Then
            'add it
            Set newOpn = New HspfOperation
            Set newOpn.Uci = newUci
            newOpn.Name = landname(j)
            toperid = 101
            firstseg(j) = toperid
            newOpn.Id = toperid
            lastseg(j) = toperid
            newOpn.Description = LUName(i)
            mOpnSeqBlock.Add newOpn
            UniqueNameCount = UniqueNameCount + 1
          Else
            addflag = True
            For Each lOpn In mOpnSeqBlock.Opns
              If lOpn.Description = LUName(i) And lOpn.Name = landname(j) Then
                addflag = False
                toperid = lOpn.Id
              End If
            Next lOpn
            If addflag Then
              UniqueNameCount = UniqueNameCount + 1
              Set newOpn = New HspfOperation
              Set newOpn.Uci = newUci
              newOpn.Name = landname(j)
              newOpn.Description = LUName(i)
              toperid = 100 + UniqueNameCount
              newOpn.Id = toperid
              lastseg(j) = toperid
              mOpnSeqBlock.Add newOpn
            End If
          End If
          'remember what we named this land use
          luoper(i) = landname(j)
          luid(i) = toperid
        End If
      Next i
    Next j
End Sub

Private Sub CreateOpnsForMultSegs(newUci As HspfUci)

    Dim j&, i&, ibase&, ioper&, toperid&, k&
    Dim PerlndCount&, addflag As Boolean, ImplndCount&
    Dim PerNames() As String, ndigits&
    Dim ImpNames() As String
    Dim newOpn As HspfOperation
    Dim lOpn As HspfOperation
    
    PerlndCount = 0
    ImplndCount = 0
    'prescan to see how many perlnds and implnds per segment
    For i = 1 To LURecCnt
      If LUType(i) = 2 Then
        'perlnd
        addflag = True
        If PerlndCount > 0 Then
          For j = 1 To UBound(PerNames)
            If PerNames(j) = LUName(i) Then
              addflag = False
            End If
          Next j
        End If
        If addflag Then
          PerlndCount = PerlndCount + 1
          ReDim Preserve PerNames(PerlndCount)
          PerNames(PerlndCount) = LUName(i)
        End If
      ElseIf LUType(i) = 1 Then
        'implnd
        addflag = True
        If ImplndCount > 0 Then
          For j = 1 To UBound(ImpNames)
            If ImpNames(j) = LUName(i) Then
              addflag = False
            End If
          Next j
        End If
        If addflag Then
          ImplndCount = ImplndCount + 1
          ReDim Preserve ImpNames(ImplndCount)
          ImpNames(ImplndCount) = LUName(i)
        End If
      End If
    Next i
    
    ndigits = 0
    For i = 1 To RCHRecCnt
      If Len(RCHId(i)) > ndigits Then
        ndigits = Len(RCHId(i))
      End If
    Next i
    
    If ndigits = 1 Or ndigits = 0 Then
      'use 101, 102, 201, 202 scheme
      ibase = 100
    ElseIf ndigits = 2 And PerlndCount < 10 And ImplndCount < 10 Then
      'use 11, 12, 21, 22 scheme
      ibase = 10
    Else
      'too many to use the multiple seg scheme
      Call MsgBox("There are too many segments to use this segmentation scheme." & vbCrLf & _
                  "Create will use the 'Grouped' scheme instead", vbOKOnly, "Create Problem")
      Call CreateOpnsForOneSeg(newUci)
      ibase = 0
    End If
    
    If ibase > 0 Then
      'create these perlnd operations
      firstseg(1) = 99999
      lastseg(1) = 0
      firstseg(2) = 99999
      lastseg(2) = 0
      For k = 1 To RCHRecCnt
        'loop through each reach
        For i = 1 To LURecCnt
          'look to see if this landuse rec goes to this reach
          If LUReach(i) = RCHId(k) Then
            'it does
            If LUType(i) = 2 Then
              'add this perlnd oper
              Set newOpn = New HspfOperation
              Set newOpn.Uci = newUci
              newOpn.Name = "PERLND"
              ioper = 0
              For j = 1 To PerlndCount
                If PerNames(j) = LUName(i) Then
                  'this is the land use we want
                  ioper = j
                End If
              Next j
              toperid = (LUReach(i) * ibase) + ioper
              If toperid < firstseg(2) Then firstseg(2) = toperid
              If toperid > lastseg(2) Then lastseg(2) = toperid
              newOpn.Id = toperid
              newOpn.Description = LUName(i)
              mOpnSeqBlock.Add newOpn
              'remember what we named this land use
              luoper(i) = "PERLND"
              luid(i) = toperid
            End If
          End If
        Next i
      
        'now add implnds
        For i = 1 To LURecCnt
          If LUReach(i) = RCHId(k) Then
            If LUType(i) = 1 Then
              'add this implnd oper
              Set newOpn = New HspfOperation
              Set newOpn.Uci = newUci
              newOpn.Name = "IMPLND"
              ioper = 0
              For j = 1 To ImplndCount
                If ImpNames(j) = LUName(i) Then
                  'this is the land use we want
                  ioper = j
                End If
              Next j
              toperid = (LUReach(i) * ibase) + ioper
              If toperid < firstseg(1) Then firstseg(1) = toperid
              If toperid > lastseg(1) Then lastseg(1) = toperid
              newOpn.Id = toperid
              newOpn.Description = LUName(i)
              mOpnSeqBlock.Add newOpn
              'remember what we named this land use
              luoper(i) = "IMPLND"
              luid(i) = toperid
            End If
          End If
        Next i
      Next k
    End If
    
End Sub

Public Function WDMInd(wdmid$) As Long
  Dim w$
  
  If Len(wdmid) > 3 Then
    w = Mid(wdmid, 4, 1)
    If w = " " Then w = "1"
  Else
    w = "1"
  End If
  WDMInd = w
End Function

Private Sub CreatePointSourceDSNs(myUci As HspfUci, MasterPollutantList As Collection)
  Dim i, newwdmid$, newdsn&
  Dim sen$, loc$, Con$, stanam$, tstype$, jdates!(1), rload!(1)

  On Error Resume Next
  sen = "PT-OBS"
  For i = 0 To PollutantCount - 1
    If FacilityReach(PollutantFacID(i)) > 0 Then
      Con = GetPollutantIDFromName(MasterPollutantList, PollutantName(i))
      If Len(Con) = 0 Then
        Con = UCase(Mid(PollutantName(i), 1, 8))
      End If
      stanam = FacilityName(PollutantFacID(i))
      loc = "RCH" & CStr(FacilityReach(PollutantFacID(i)))
      tstype = UCase(Mid(PollutantName(i), 1, 4))
      rload(1) = PollutantLoad(i)
      myUci.AddPointSourceDataSet sen, loc, Con, stanam, tstype, 0, jdates, rload, _
         newwdmid, newdsn
    End If
  Next i
  'If Len(newwdmid) = 4 Then
  '  myUci.GetWDMObj(CInt(Right(newwdmid, 1))).Refresh
  'End If
  
End Sub

Private Function GetPollutantIDFromName(PollutantList As Collection, PollutantName As String) As String
  Dim i&
  
  GetPollutantIDFromName = "x"
  i = 1
  Do While GetPollutantIDFromName = "x"
    If Trim(Mid(PollutantList(i), 14)) = Trim(PollutantName) Then
      GetPollutantIDFromName = Mid(PollutantList(i), 1, 5)
    End If
    i = i + 1
    If i > PollutantList.Count Then GetPollutantIDFromName = ""
  Loop
End Function

Private Sub CreateDefaultOutput(myUci As HspfUci)
  Dim vConn As Variant
  Dim lConn As HspfConnection
  Dim bottomid&, wdmid&, newdsn&
  
  bottomid = 0
  For Each vConn In myUci.Connections
    Set lConn = vConn
    If lConn.Typ = 3 Then
      'schematic record
      If lConn.Source.VolName = "RCHRES" And lConn.Target.VolName = "RCHRES" Then
        bottomid = lConn.Target.VolId
      End If
    End If
  Next vConn
  
  If bottomid > 0 Then 'found watershed outlet
    Call myUci.AddOutputWDMDataSet("RCH" & bottomid, "FLOW", 100, wdmid, newdsn)
    myUci.AddExtTarget "RCHRES", bottomid, "HYDR", "RO", 1, 1, 1#, "AVER", _
               "WDM" & CStr(wdmid), newdsn, "FLOW", 1, "ENGL", "AGGR", "REPL"
    'myUci.GetWDMObj(wdmid).Refresh
  End If
  
End Sub

Private Function DefaultLSURFromSLSUR(slsur As Single)
  If slsur < 0.005 Then
    DefaultLSURFromSLSUR = 500
  ElseIf slsur < 0.01 Then
    DefaultLSURFromSLSUR = 400
  ElseIf slsur < 0.03 Then
    DefaultLSURFromSLSUR = 350
  ElseIf slsur < 0.07 Then
    DefaultLSURFromSLSUR = 300
  ElseIf slsur < 0.1 Then
    DefaultLSURFromSLSUR = 250
  ElseIf slsur < 0.15 Then
    DefaultLSURFromSLSUR = 200
  Else
    DefaultLSURFromSLSUR = 150
  End If
End Function

Private Sub CreateBinaryOutput(myUci As HspfUci, s$)
  Dim newFile As HspfFile, i&
  Dim lOper As HspfOperation
 
  'add file name to files block
  newFile.Name = s & ".hbn"
  newFile.Typ = "BINO"
  newFile.Unit = 92
  mFilesBlock.Add newFile
  'update bin output units
  For i = 1 To myUci.OpnBlks("PERLND").Count
    Set lOper = myUci.OpnBlks("PERLND").Ids(i)
    lOper.Tables("GEN-INFO").Parms("BUNIT1") = 92
  Next i
  For i = 1 To myUci.OpnBlks("IMPLND").Count
    Set lOper = myUci.OpnBlks("IMPLND").Ids(i)
    lOper.Tables("GEN-INFO").Parms("BUNIT1") = 92
  Next i
  For i = 1 To myUci.OpnBlks("RCHRES").Count
    Set lOper = myUci.OpnBlks("RCHRES").Ids(i)
    lOper.Tables("GEN-INFO").Parms("BUNITE") = 92
  Next i
  'add binary-info tables
  myUci.OpnBlks("PERLND").AddTableForAll "BINARY-INFO", "PERLND"
  myUci.OpnBlks("IMPLND").AddTableForAll "BINARY-INFO", "IMPLND"
  myUci.OpnBlks("RCHRES").AddTableForAll "BINARY-INFO", "RCHRES"
End Sub
