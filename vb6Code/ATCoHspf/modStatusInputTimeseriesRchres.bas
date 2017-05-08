Attribute VB_Name = "modStatusInputTimeseriesRchres"
Option Explicit
'Copyright 2002 AQUA TERRA Consultants - Royalty-free use permitted under open source license

Public Sub UpdateInputTimeseriesRchres(O As HspfOperation, TimserStatus As HspfStatus)
  Dim ltable As HspfTable
  Dim i&
  Dim nExits&, Odfvfg&(5), Odgtfg&(5), nGqual&
  Dim nCons&, coadfg&(20), sandfg&, wetadfg&, dryadfg&, ctemp$
  
  If O.TableExists("ACTIVITY") Then
    Set ltable = O.Tables("ACTIVITY")
    If O.TableExists("GEN-INFO") Then
      nExits = O.Tables("GEN-INFO").Parms("NEXITS")
    Else
      nExits = 1
    End If

    'section hydr
    If ltable.Parms("HYDRFG") = 1 Then
      If O.TableExists("HYDR-PARM1") Then
        With O.Tables("HYDR-PARM1")
          Odfvfg(1) = .Parms("ODFVF1")
          Odfvfg(2) = .Parms("ODFVF2")
          Odfvfg(3) = .Parms("ODFVF3")
          Odfvfg(4) = .Parms("ODFVF4")
          Odfvfg(5) = .Parms("ODFVF5")
          Odgtfg(1) = .Parms("ODGTF1")
          Odgtfg(2) = .Parms("ODGTF2")
          Odgtfg(3) = .Parms("ODGTF3")
          Odgtfg(4) = .Parms("ODGTF4")
          Odgtfg(5) = .Parms("ODGTF5")
        End With
      Else
        Odfvfg(1) = 0: Odfvfg(2) = 0: Odfvfg(3) = 0: Odfvfg(4) = 0: Odfvfg(5) = 0
        Odgtfg(1) = 0: Odgtfg(2) = 0: Odgtfg(3) = 0: Odgtfg(4) = 0: Odgtfg(5) = 0
      End If
      If O.Uci.CategoryBlock Is Nothing Then
        'not using category block
        TimserStatus.Change "INFLOW:IVOL", 1, HspfStatusOptional
      Else
        'have category block
        For i = 1 To O.Uci.CategoryBlock.Count
          TimserStatus.Change "INFLOW:CIVOL", i, HspfStatusOptional
        Next i
      End If
      If O.Uci.CategoryBlock Is Nothing Then
        'not using category block
        TimserStatus.Change "EXTNL:IVOL", 1, HspfStatusOptional
      Else
        'have category block
        For i = 1 To O.Uci.CategoryBlock.Count
          TimserStatus.Change "EXTNL:CIVOL", i, HspfStatusOptional
        Next i
      End If
      TimserStatus.Change "EXTNL:PREC", 1, HspfStatusOptional
      TimserStatus.Change "EXTNL:POTEV", 1, HspfStatusOptional
      For i = 1 To nExits
        If Odfvfg(i) < 0 Then
          TimserStatus.Change "EXTNL:COLIND", i, HspfStatusRequired
        End If
        If O.Uci.CategoryBlock Is Nothing Then
          'not using category block
          If Odgtfg(i) > 0 Then
            TimserStatus.Change "EXTNL:OUTDGT", Odgtfg(i), HspfStatusRequired
          End If
        Else
          'have category block
          If Odgtfg(i) > 0 Then
            'make optional since we cant don't accomodate string versions of sub1,sub2
            TimserStatus.Change "EXTNL:COTDGT", Odgtfg(i), HspfStatusOptional
          End If
        End If
      Next i
    End If
      
    'section adcalc
    If ltable.Parms("ADFG") = 1 Then
      If ltable.Parms("HYDRFG") = 0 Then 'need what HYDR would have sent
        TimserStatus.Change "HYDR:VOL", 1, HspfStatusRequired
        For i = 1 To nExits
          TimserStatus.Change "HYDR:O", i, HspfStatusRequired
        Next i
      End If
    End If
    
    'section cons
    If ltable.Parms("CONSFG") = 1 Then
      If O.TableExists("NCONS") Then
        nCons = O.Tables("NCONS").Parms("NCONS")
      Else
        nCons = 0
      End If
      If O.TableExists("CONS-AD-FLAGS") Then
        With O.Tables("CONS-AD-FLAGS")
          For i = 1 To 20
            ctemp = "COADFG(" & CStr(i) & ")"
            coadfg(i) = .Parms(ctemp)
          Next i
        End With
      Else
        For i = 1 To 20
          coadfg(i) = 0
        Next i
      End If
      
      For i = 1 To nCons
        If coadfg(((i - 1) * 2) + 1) = -1 Then
          'dry flux timser required
          TimserStatus.Change "EXTNL:COADFX", i, HspfStatusRequired
        End If
        If coadfg(((i - 1) * 2) + 2) = -1 Then
          'wet conc timser required
          TimserStatus.Change "EXTNL:COADCN", i, HspfStatusRequired
        End If
        If coadfg(((i - 1) * 2) + 1) <> 0 Then
          'dry ad simulated
          dryadfg = 1
        End If
        If coadfg(((i - 1) * 2) + 2) <> 0 Then
          'wet ad simulated
          wetadfg = 1
          TimserStatus.Change "EXTNL:PREC", 1, HspfStatusRequired
        End If
        TimserStatus.Change "EXTNL:ICON", i, HspfStatusOptional
        TimserStatus.Change "INFLOW:ICON", i, HspfStatusOptional
      Next i
      If (wetadfg = 1 Or dryadfg = 1) And ltable.Parms("HYDRFG") = 0 Then
        'ad simulated and hydr off
        TimserStatus.Change "HYDR:SAREA", 1, HspfStatusRequired
      End If
    End If
    
    'section htrch
    If ltable.Parms("HTFG") = 1 Then
      TimserStatus.Change "INFLOW:IHEAT", 1, HspfStatusOptional
      TimserStatus.Change "EXTNL:SOLRAD", 1, HspfStatusRequired
      TimserStatus.Change "EXTNL:PREC", 1, HspfStatusOptional
      TimserStatus.Change "EXTNL:CLOUD", 1, HspfStatusRequired
      TimserStatus.Change "EXTNL:DEWTMP", 1, HspfStatusRequired
      TimserStatus.Change "EXTNL:GATMP", 1, HspfStatusRequired
      TimserStatus.Change "EXTNL:WIND", 1, HspfStatusRequired
      If ltable.Parms("HYDRFG") = 0 Then
        TimserStatus.Change "HYDR:AVDEP", 1, HspfStatusRequired
      End If
    End If
    
    'section sedtran
    If ltable.Parms("SEDFG") = 1 Then
      TimserStatus.Change "INFLOW:ISED", 1, HspfStatusOptional
      TimserStatus.Change "INFLOW:ISED", 2, HspfStatusOptional
      TimserStatus.Change "INFLOW:ISED", 3, HspfStatusOptional
      If O.TableExists("SANDFG") Then
        sandfg = O.Tables("SANDFG").Parms("SANDFG")
      Else
        sandfg = 0
      End If
      If ltable.Parms("HYDRFG") = 0 Then
        TimserStatus.Change "HYDR:TAU", 1, HspfStatusRequired
        TimserStatus.Change "HYDR:AVDEP", 1, HspfStatusRequired
        TimserStatus.Change "HYDR:AVVEL", 1, HspfStatusRequired
        If sandfg = 1 Or sandfg = 2 Then
          TimserStatus.Change "HYDR:RO", 1, HspfStatusRequired
          TimserStatus.Change "HYDR:HRAD", 1, HspfStatusRequired
          TimserStatus.Change "HYDR:TWID", 1, HspfStatusRequired
        End If
      End If
      If ltable.Parms("HTFG") = 0 Then
        If sandfg = 1 Or sandfg = 2 Then
          TimserStatus.Change "HTRCH:TW", 1, HspfStatusRequired
        End If
      End If
    End If
    
    'section gqual
    If ltable.Parms("GQALFG") = 1 Then
      If O.TableExists("GQ-GENDATA") Then
        nGqual = O.Tables("GQ-GENDATA").Parms("NGQUAL")
      Else
        nGqual = 1
      End If
      For i = 1 To nGqual
        TimserStatus.Change2 "INFLOW:IDQAL", i, 1, HspfStatusOptional
        TimserStatus.Change2 "INFLOW:ISQAL", 1, i, HspfStatusOptional
        TimserStatus.Change2 "INFLOW:ISQAL", 2, i, HspfStatusOptional
        TimserStatus.Change2 "INFLOW:ISQAL", 3, i, HspfStatusOptional
        If dryadfg = 1 Then
          TimserStatus.Change "EXTNL:GQADFX", i, HspfStatusRequired
        End If
        If wetadfg = 1 Then
          TimserStatus.Change "EXTNL:GQADCN", i, HspfStatusRequired
        End If
      Next i
      If wetadfg = 1 Then
        TimserStatus.Change "EXTNL:PREC", 1, HspfStatusRequired
      End If
      
      If O.TableExists("GQ-GENDATA") Then
        If O.Tables("GQ-GENDATA").Parms("PHFLAG") = 1 And _
          ltable.Parms("PHFG") = 0 Then
          TimserStatus.Change "EXTNL:PHVAL", 1, HspfStatusOptional 'req if there is hydrolysis
        End If
        If O.Tables("GQ-GENDATA").Parms("ROXFG") = 1 Then
          TimserStatus.Change "EXTNL:ROC", 1, HspfStatusOptional 'req if there is free radical oxidation
        End If
        If O.Tables("GQ-GENDATA").Parms("CLDFG") = 1 Then
          TimserStatus.Change "EXTNL:CLOUD", 1, HspfStatusOptional 'req if there is photolysis
        End If
      End If
      For i = 1 To nGqual
        TimserStatus.Change "EXTNL:BIO", i, HspfStatusOptional 'if qual number I undergoes biodegradation and GQPM2(7,I)=1
      Next i

      If O.TableExists("GEN-INFO") Then
        If O.Tables("GEN-INFO").Parms("LKFG") = 1 Then
          TimserStatus.Change "EXTNL:WIND", 1, HspfStatusOptional 'req if there is volatilization
        End If
      End If
      If ltable.Parms("HYDRFG") = 0 Then
        If O.TableExists("GEN-INFO") Then
          If O.Tables("GEN-INFO").Parms("LKFG") = 0 Then
            TimserStatus.Change "HYDR:AVDEP", 1, HspfStatusOptional 'req if volatilization is on
            TimserStatus.Change "HYDR:AVVEL", 1, HspfStatusOptional 'req if volatilization is on
          End If
        End If
        If (wetadfg = 1 Or dryadfg = 1) Then
          'ad simulated
          TimserStatus.Change "HYDR:SAREA", 1, HspfStatusRequired
        End If
      End If
      If O.TableExists("GQ-GENDATA") Then
        If ltable.Parms("HTFG") = 0 And O.Tables("GQ-GENDATA").Parms("TEMPFG") = 1 Then
          TimserStatus.Change "HTRCH:TW", 1, HspfStatusRequired
        End If
        If ltable.Parms("PLKFG") = 0 Or O.Tables("GQ-GENDATA").Parms("PHYTFG") = 0 Then
          If O.Tables("GQ-GENDATA").Parms("PHYTFG") = 1 Then
            TimserStatus.Change "PLANK:PHYTO", 1, HspfStatusOptional 'req if there is photolysis
          End If
        End If
        If ltable.Parms("SEDFG") = 0 And O.Tables("GQ-GENDATA").Parms("SDFG") = 1 Then
          TimserStatus.Change "SEDTRN:SSED", 1, HspfStatusOptional 'req if there is photolysis
          TimserStatus.Change "SEDTRN:SSED", 2, HspfStatusOptional 'req if there is photolysis
          TimserStatus.Change "SEDTRN:SSED", 3, HspfStatusOptional 'req if there is photolysis
          TimserStatus.Change "SEDTRN:SSED", 4, HspfStatusOptional 'req if there is photolysis
        End If
      End If
    End If
    
    'section oxrx
    If ltable.Parms("OXFG") = 1 Then
      'TimserStatus.Change "INFLOW:IDOX", 1, HspfStatusOptional
      'TimserStatus.Change "INFLOW:IBOD", 1, HspfStatusOptional
      TimserStatus.Change "INFLOW:OXIF", 1, HspfStatusOptional
      TimserStatus.Change "INFLOW:OXIF", 2, HspfStatusOptional
      If O.TableExists("GEN-INFO") Then
        If O.Tables("GEN-INFO").Parms("LKFG") = 1 Then
          TimserStatus.Change "EXTNL:WIND", 1, HspfStatusRequired
        End If
      End If
      If ltable.Parms("HYDRFG") = 0 Then
        TimserStatus.Change "HYDR:AVDEP", 1, HspfStatusRequired
        TimserStatus.Change "HYDR:AVVEL", 1, HspfStatusRequired 'man had avdep twice?
      End If
      If ltable.Parms("HTFG") = 0 Then
        TimserStatus.Change "HTRCH:TW", 1, HspfStatusRequired
      End If
    End If
    
    'section nutrx
    If ltable.Parms("NUTFG") = 1 Then
'      TimserStatus.Change "INFLOW:INO3", 1, HspfStatusOptional
'      TimserStatus.Change "INFLOW:ITAM", 1, HspfStatusOptional
'      TimserStatus.Change "INFLOW:INO2", 1, HspfStatusOptional
'      TimserStatus.Change "INFLOW:IPO4", 1, HspfStatusOptional
'      TimserStatus.Change "INFLOW:ISNH4", 1, HspfStatusOptional
'      TimserStatus.Change "INFLOW:ISPO4", 1, HspfStatusOptional
      For i = 1 To 4
        TimserStatus.Change "INFLOW:NUIF1", i, HspfStatusOptional
      Next i
      For i = 1 To 3
        TimserStatus.Change2 "INFLOW:NUIF2", i, 1, HspfStatusOptional
        TimserStatus.Change2 "INFLOW:NUIF2", i, 2, HspfStatusOptional
      Next i
      If wetadfg = 1 Or dryadfg = 1 Then
        TimserStatus.Change "EXTNL:NUADFX", 1, HspfStatusRequired
        TimserStatus.Change "EXTNL:NUADFX", 2, HspfStatusRequired
        TimserStatus.Change "EXTNL:NUADFX", 3, HspfStatusRequired
      End If
      If wetadfg = 1 Then
        TimserStatus.Change "EXTNL:NUADCN", 1, HspfStatusRequired
        TimserStatus.Change "EXTNL:NUADCN", 2, HspfStatusRequired
        TimserStatus.Change "EXTNL:NUADCN", 3, HspfStatusRequired
        TimserStatus.Change "EXTNL:PREC", 1, HspfStatusRequired
      End If
      If (wetadfg = 1 Or dryadfg = 1) And ltable.Parms("HYDRFG") = 0 Then
        TimserStatus.Change "HYDR:SAREA", 1, HspfStatusRequired
      End If
      If ltable.Parms("HTFG") = 0 Then
        TimserStatus.Change "HTRCH:TW", 1, HspfStatusRequired
      End If
      If ltable.Parms("SEDFG") = 0 Then
        TimserStatus.Change "SEDTRN:SSED", 1, HspfStatusOptional  'req if particulate NH4 or PO4 is simulated
      End If
    End If
    
    'section plank
    If ltable.Parms("PLKFG") = 1 Then
'      TimserStatus.Change "INFLOW:IPHYTO", 1, HspfStatusOptional
'      TimserStatus.Change "INFLOW:IZOO", 1, HspfStatusOptional
'      TimserStatus.Change "INFLOW:IORN", 1, HspfStatusOptional
'      TimserStatus.Change "INFLOW:IORP", 1, HspfStatusOptional
'      TimserStatus.Change "INFLOW:IORC", 1, HspfStatusOptional
      For i = 1 To 5
        TimserStatus.Change "INFLOW:PKIF", i, HspfStatusOptional
      Next i
      TimserStatus.Change "EXTNL:SOLRAD", 1, HspfStatusRequired
      If wetadfg = 1 Or dryadfg = 1 Then
        TimserStatus.Change "EXTNL:PLADFX", 1, HspfStatusRequired
        TimserStatus.Change "EXTNL:PLADFX", 2, HspfStatusRequired
        TimserStatus.Change "EXTNL:PLADFX", 3, HspfStatusRequired
      End If
      If wetadfg = 1 Then
        TimserStatus.Change "EXTNL:PLADCN", 1, HspfStatusRequired
        TimserStatus.Change "EXTNL:PLADCN", 2, HspfStatusRequired
        TimserStatus.Change "EXTNL:PLADCN", 3, HspfStatusRequired
        TimserStatus.Change "EXTNL:PREC", 1, HspfStatusRequired
      End If
      If (wetadfg = 1 Or dryadfg = 1) And ltable.Parms("HYDRFG") = 0 Then
        TimserStatus.Change "HYDR:SAREA", 1, HspfStatusRequired
      End If
      If ltable.Parms("HTFG") = 0 Then
        TimserStatus.Change "HTRCH:TW", 1, HspfStatusRequired
      End If
      If ltable.Parms("SEDFG") = 0 Then
        If O.TableExists("PLNK-FLAGS") Then
          If O.Tables("PLNK-FLAGS").Parms("SDLTFG") = 1 Then
            TimserStatus.Change "SEDTRN:SSED", 1, HspfStatusRequired
            TimserStatus.Change "SEDTRN:SSED", 2, HspfStatusRequired
            TimserStatus.Change "SEDTRN:SSED", 3, HspfStatusRequired
            TimserStatus.Change "SEDTRN:SSED", 4, HspfStatusRequired
          End If
        End If
      End If
    End If
    
    'section phcarb
    If ltable.Parms("PHFG") = 1 Or ltable.Parms("PHFG") = 3 Then
'      TimserStatus.Change "INFLOW:ITIC", 1, HspfStatusOptional
'      TimserStatus.Change "INFLOW:ICO2", 1, HspfStatusOptional
      For i = 1 To 2
        TimserStatus.Change "INFLOW:PHIF", i, HspfStatusOptional
      Next i
      If ltable.Parms("CONSFG") = 0 And O.TableExists("PH-PARM1") Then
        For i = 1 To O.Tables("PH-PARM1").Parms("ALKCON")
          TimserStatus.Change "CONS:CON", i, HspfStatusRequired
        Next i
      End If
      If ltable.Parms("HTFG") = 0 Then
        TimserStatus.Change "HTRCH:TW", 1, HspfStatusRequired
      End If
    End If
    
    Dim lAcidph As Boolean
    lAcidph = False
    'check to see if acidph is available
    For i = 1 To O.Uci.Msg.BlockDefs("RCHRES").SectionDefs.Count
      If O.Uci.Msg.BlockDefs("RCHRES").SectionDefs(i).Name = "ACIDPH" Then
        lAcidph = True
      End If
    Next i
    If lAcidph Then
      'section acidph
      If ltable.Parms("PHFG") = 2 Or ltable.Parms("PHFG") = 3 Then
        For i = 1 To 7
          TimserStatus.Change "INFLOW:ACINFL", i, HspfStatusOptional
        Next i
        If ltable.Parms("HYDRFG") = 0 Then
          TimserStatus.Change "HYDR:AVDEP", 1, HspfStatusRequired
        End If
        If ltable.Parms("HTFG") = 0 Then
          TimserStatus.Change "HTRCH:TW", 1, HspfStatusRequired
        End If
      End If
    End If
    
  End If
End Sub
