Attribute VB_Name = "modStatusPerlnd"
Option Explicit
'Copyright 2002 AQUA TERRA Consultants - Royalty-free use permitted under open source license

Public Sub UpdatePerlnd(O As HspfOperation, TableStatus As HspfStatus)
  Dim ltable As HspfTable
  Dim i&, j&, n&, t$, f$
  Dim Vkmfg&
  Dim Vcsfg&, Vuzfg&, Vnnfg&, Vifwfg&, Vircfg&, Vlefg&, Hwtfg&, Irrgfg&
  Dim Szonfg&, Vcrdfg, Vawdfg
  Dim Vcrvfg&, Vsivfg&
  Dim Sltvfg&, Ultvfg&, Lgtvfg&, Tsopfg&
  Dim Idvfg&, Icvfg&, Gdvfg&, Gcvfg&
  Dim Nqual&, nqsd&, nqof&, Nqif&, Nqgw&, Qsdfg&, Qsofg&, Qifwfg&, Qagwfg&
  Dim Vpfwfg&, Vpfsfg&, Vqofg&, Viqcfg&, Vaqcfg&
  Dim Npest&, Adopfg&(3)
  Dim Vnutfg&, Forafg&, Nuptfg&, Amvofg&, Alpnfg&, Vnprfg&
  Dim Vputfg&, Forpfg&, Puptfg&, tabname$
  
  'always can be present
  TableStatus.Change "ACTIVITY", 1, HspfStatusRequired
  TableStatus.Change "PRINT-INFO", 1, HspfStatusOptional
  TableStatus.Change "GEN-INFO", 1, HspfStatusRequired
  
  If O.TableExists("ACTIVITY") Then
    Set ltable = O.Tables("ACTIVITY")
    If ltable.Parms("AIRTFG") = 1 Then
      TableStatus.Change "ATEMP-DAT", 1, HspfStatusOptional
    End If
    If ltable.Parms("SNOWFG") = 1 Then
      TableStatus.Change "ICE-FLAG", 1, HspfStatusOptional
      TableStatus.Change "SNOW-FLAGS", 1, HspfStatusOptional
      If O.TableExists("SNOW-FLAGS") Then
        Vkmfg = O.Tables("SNOW-FLAGS").Parms("VKMFG")
      Else
        Vkmfg = 0
      End If
      TableStatus.Change "SNOW-PARM1", 1, HspfStatusRequired
      TableStatus.Change "SNOW-PARM2", 1, HspfStatusOptional
      If Vkmfg = 1 Then
        TableStatus.Change "MON-MELT-FAC", 1, HspfStatusOptional
      End If
      TableStatus.Change "SNOW-INIT1", 1, HspfStatusOptional
      TableStatus.Change "SNOW-INIT2", 1, HspfStatusOptional
    End If
    If ltable.Parms("PWATFG") = 1 Then
      TableStatus.Change "PWAT-PARM1", 1, HspfStatusOptional
      If O.TableExists("PWAT-PARM1") Then
        Vcsfg = O.Tables("PWAT-PARM1").Parms("VCSFG")
        Vuzfg = O.Tables("PWAT-PARM1").Parms("VUZFG")
        Vnnfg = O.Tables("PWAT-PARM1").Parms("VNNFG")
        Vifwfg = O.Tables("PWAT-PARM1").Parms("VIFWFG")
        Vircfg = O.Tables("PWAT-PARM1").Parms("VIRCFG")
        Vlefg = O.Tables("PWAT-PARM1").Parms("VLEFG")
        Hwtfg = O.Tables("PWAT-PARM1").Parms("HWTFG")
        Irrgfg = O.Tables("PWAT-PARM1").Parms("IRRGFG")
        If ltable.Parms("SNOWFG") = 1 Then
          O.Tables("PWAT-PARM1").Parms("CSNOFG") = 1
        End If
      Else
        Vcsfg = 0
        Vuzfg = 0
        Vnnfg = 0
        Vifwfg = 0
        Vircfg = 0
        Vlefg = 0
        Hwtfg = 0
        Irrgfg = 0
      End If
      TableStatus.Change "PWAT-PARM2", 1, HspfStatusRequired
      TableStatus.Change "PWAT-PARM3", 1, HspfStatusOptional
      If Vcsfg = 0 Or Vuzfg = 0 Or Vnnfg = 0 Or Vifwfg = 0 Or Vircfg = 0 Or Vlefg = 0 Then
        TableStatus.Change "PWAT-PARM4", 1, HspfStatusRequired
      End If
      TableStatus.Change "PWAT-PARM5", 1, HspfStatusOptional
      If Hwtfg = 1 Then
        TableStatus.Change "PWAT-PARM6", 1, HspfStatusRequired
        TableStatus.Change "PWAT-PARM7", 1, HspfStatusRequired
      End If
      'some of the monthlies are optional  THJ
      If Vcsfg = 1 Then
        TableStatus.Change "MON-INTERCEP", 1, HspfStatusOptional
      End If
      If Vuzfg = 1 Then
        TableStatus.Change "MON-UZSN", 1, HspfStatusRequired
      End If
      If Vnnfg = 1 Then
        TableStatus.Change "MON-MANNING", 1, HspfStatusOptional
      End If
      If Vifwfg = 1 Then
        TableStatus.Change "MON-INTERFLW", 1, HspfStatusRequired
      End If
      If Vircfg = 1 Then
        TableStatus.Change "MON-IRC", 1, HspfStatusRequired
      End If
      If Vlefg = 1 Then
        TableStatus.Change "MON-LZETPARM", 1, HspfStatusOptional
      End If
      TableStatus.Change "PWAT-STATE1", 1, HspfStatusOptional
    End If
    If Irrgfg >= 1 Then
      If Irrgfg = 2 Or Irrgfg = 3 Then
        TableStatus.Change "IRRIG-PARM1", 1, HspfStatusOptional
        If O.TableExists("IRRIG-PARM1") Then
          Szonfg = O.Tables("IRRIG-PARM1").Parms("SZONFG")
          Vcrdfg = O.Tables("IRRIG-PARM1").Parms("VCRDFG")
          Vawdfg = O.Tables("IRRIG-PARM1").Parms("VAWDFG")
        Else
          Szonfg = 0
          Vcrdfg = 0
          Vawdfg = 0
        End If
      End If
      TableStatus.Change "IRRIG-PARM2", 1, HspfStatusOptional
      TableStatus.Change "IRRIG-SOURCE", 1, HspfStatusOptional
      TableStatus.Change "IRRIG-TARGET", 1, HspfStatusOptional
      If Irrgfg = 3 Then
        TableStatus.Change "IRRIG-SCHED", 1, HspfStatusOptional
      ElseIf Irrgfg = 2 Then
        TableStatus.Change "SOIL-DATA", 1, HspfStatusOptional
        If Szonfg = 1 Then
          TableStatus.Change "SOIL-DATA2", 1, HspfStatusOptional
          TableStatus.Change "SOIL-DATA3", 1, HspfStatusOptional
        End If
        TableStatus.Change "CROP-DATES", 1, HspfStatusOptional
        If Vcrdfg = 1 Or Vawdfg = 1 Then
          TableStatus.Change "CROP-STAGES", 1, HspfStatusOptional
          TableStatus.Change "CROP-SEASPM", 1, HspfStatusOptional
          If Vcrdfg = 1 Then
            TableStatus.Change "MON-IRR-CRDP", 1, HspfStatusOptional
          End If
          If Vawdfg = 1 Then
            TableStatus.Change "MON-IRR-AWD", 1, HspfStatusOptional
          End If
        End If
      End If
    End If
    If ltable.Parms("SEDFG") = 1 Then
      TableStatus.Change "SED-PARM1", 1, HspfStatusOptional
      If O.TableExists("SED-PARM1") Then
        Vcrvfg = O.Tables("SED-PARM1").Parms("CRVFG")
        Vsivfg = O.Tables("SED-PARM1").Parms("VSIVFG")
      Else
        Vcrvfg = 0
        Vsivfg = 0
      End If
      TableStatus.Change "SED-PARM2", 1, HspfStatusRequired
      TableStatus.Change "SED-PARM3", 1, HspfStatusRequired
      If Vcrvfg = 1 Then
        TableStatus.Change "MON-COVER", 1, HspfStatusOptional
      End If
      If Vsivfg = 1 Then
        TableStatus.Change "MON-NVSI", 1, HspfStatusOptional
      End If
      TableStatus.Change "SED-STOR", 1, HspfStatusOptional
    End If
    If ltable.Parms("PSTFG") = 1 Then
      TableStatus.Change "PSTEMP-PARM1", 1, HspfStatusOptional
      If O.TableExists("PSTEMP-PARM1") Then
        Sltvfg = O.Tables("PSTEMP-PARM1").Parms("SLTVFG")
        Ultvfg = O.Tables("PSTEMP-PARM1").Parms("ULTVFG")
        Lgtvfg = O.Tables("PSTEMP-PARM1").Parms("LGTVFG")
        Tsopfg = O.Tables("PSTEMP-PARM1").Parms("TSOPFG")
      Else
        Sltvfg = 0
        Ultvfg = 0
        Lgtvfg = 0
        Tsopfg = 0
      End If
      If Sltvfg = 1 Then
        TableStatus.Change "MON-ASLT", 1, HspfStatusOptional
        TableStatus.Change "MON-BSLT", 1, HspfStatusOptional
      End If
      If Ultvfg = 1 Then
        TableStatus.Change "MON-ULTP1", 1, HspfStatusRequired
        TableStatus.Change "MON-ULTP2", 1, HspfStatusRequired
      End If
      If Lgtvfg = 1 Then
        TableStatus.Change "MON-LGTP1", 1, HspfStatusRequired
        If Tsopfg = 0 Or Tsopfg = 2 Then
          TableStatus.Change "MON-LGTP2", 1, HspfStatusRequired
        End If
      End If
      If O.TableExists("MON-ASLT") And O.TableExists("MON-BSLT") And _
         O.TableExists("MON-ULTP1") And O.TableExists("MON-ULTP2") And _
         O.TableExists("MON-LGTP1") And _
         (O.TableExists("MON-LGTP2") Or Tsopfg = 1) Then
        'dont need pstemp-parm2
      Else
        TableStatus.Change "PSTEMP-PARM2", 1, HspfStatusRequired
      End If
      TableStatus.Change "PSTEMP-TEMPS", 1, HspfStatusOptional
    End If
    If ltable.Parms("PWGFG") = 1 Then
      TableStatus.Change "PWT-PARM1", 1, HspfStatusOptional
      If O.TableExists("PWT-PARM1") Then
        Idvfg = O.Tables("PWT-PARM1").Parms("IDVFG")
        Icvfg = O.Tables("PWT-PARM1").Parms("ICVFG")
        Gdvfg = O.Tables("PWT-PARM1").Parms("GDVFG")
        Gcvfg = O.Tables("PWT-PARM1").Parms("GCVFG")
      Else
        Idvfg = 0
        Icvfg = 0
        Gdvfg = 0
        Gcvfg = 0
      End If
      TableStatus.Change "PWT-PARM2", 1, HspfStatusOptional
      TableStatus.Change "LAT-FACTOR", 1, HspfStatusOptional
      If Idvfg = 1 Then
        TableStatus.Change "MON-IFWDOX", 1, HspfStatusOptional
      End If
      If Icvfg = 1 Then
        TableStatus.Change "MON-IFWCO2", 1, HspfStatusOptional
      End If
      If Gdvfg = 1 Then
        TableStatus.Change "MON-GRNDDOX", 1, HspfStatusOptional
      End If
      If Gcvfg = 1 Then
        TableStatus.Change "MON-GRNDCO2", 1, HspfStatusOptional
      End If
      TableStatus.Change "PWT-TEMPS", 1, HspfStatusOptional
      TableStatus.Change "PWT-GASES", 1, HspfStatusOptional
    End If
    If ltable.Parms("PQALFG") = 1 Then
      TableStatus.Change "NQUALS", 1, HspfStatusOptional
      If O.TableExists("NQUALS") Then
        Nqual = O.Tables("NQUALS").Parms("NQUAL")
      Else
        Nqual = 1
      End If
      TableStatus.Change "PQL-AD-FLAGS", 1, HspfStatusOptional
      nqsd = 0
      nqof = 0
      Nqif = 0
      Nqgw = 0
      For i = 1 To Nqual
        TableStatus.Change "QUAL-PROPS", i, HspfStatusRequired
        If i > 1 Then
          tabname = "QUAL-PROPS:" & i
        Else
          tabname = "QUAL-PROPS"
        End If
        If O.TableExists(tabname) Then
          Qsdfg = O.Tables(tabname).Parms("QSDFG")
          Vpfwfg = O.Tables(tabname).Parms("VPFWFG")
          Vpfsfg = O.Tables(tabname).Parms("VPFSFG")
          Qsofg = O.Tables(tabname).Parms("QSOFG")
          Vqofg = O.Tables(tabname).Parms("VQOFG")
          Qifwfg = O.Tables(tabname).Parms("QSDFG")
          Viqcfg = O.Tables(tabname).Parms("VIQCFG")
          Qagwfg = O.Tables(tabname).Parms("QSDFG")
          Vaqcfg = O.Tables(tabname).Parms("VAQCFG")
        Else
          Qsdfg = 0
          Vpfwfg = 0
          Vpfsfg = 0
          Qsofg = 0
          Vqofg = 0
          Qifwfg = 0
          Viqcfg = 0
          Qagwfg = 0
          Vaqcfg = 0
        End If
        TableStatus.Change "QUAL-INPUT", i, HspfStatusOptional
        If Qsdfg = 1 Then
          nqsd = nqsd + 1
          If Vpfwfg >= 1 Then
            TableStatus.Change "MON-POTFW", nqsd, HspfStatusRequired
          End If
          If Vpfsfg = 1 Then
            TableStatus.Change "MON-POTFS", nqsd, HspfStatusRequired
          End If
        End If
        If Qsofg = 1 Or Qsofg = 2 Then
          nqof = nqof + 1
          If Vqofg >= 1 Then
            TableStatus.Change "MON-ACCUM", nqof, HspfStatusRequired
            TableStatus.Change "MON-SQOLIM", nqof, HspfStatusRequired
          End If
        End If
        If Qifwfg = 1 Then
          Nqif = Nqif + 1
          If Viqcfg >= 1 Then
            TableStatus.Change "MON-IFLW-CONC", Nqif, HspfStatusOptional
          End If
        End If
        If Qagwfg = 1 Then
          Nqgw = Nqgw + 1
          If Vaqcfg >= 1 Then
            TableStatus.Change "MON-GRND-CONC", Nqgw, HspfStatusOptional
          End If
        End If
      Next i
    End If
    If ltable.Parms("MSTLFG") = 1 Then
      If ltable.Parms("PWATFG") = 0 Then 'pwater inactive
        TableStatus.Change "VUZFG", 1, HspfStatusOptional
        If O.TableExists("VUZFG") Then
          Vuzfg = O.Tables("VUZFG").Parms("VUZFG")
        Else
          Vuzfg = 0
        End If
        TableStatus.Change "UZSN-LZSN", 1, HspfStatusRequired
        If Vuzfg = 1 Then
          TableStatus.Change "MON-UZSN", 1, HspfStatusRequired
        End If
      End If
      TableStatus.Change "MST-PARM", 1, HspfStatusOptional
      TableStatus.Change "MST-TOPSTOR", 1, HspfStatusOptional
      TableStatus.Change "MST-TOPFLX", 1, HspfStatusOptional
      TableStatus.Change "MST-SUBSTOR", 1, HspfStatusOptional
      TableStatus.Change "MST-SUBFLX", 1, HspfStatusOptional
    End If
    If ltable.Parms("PESTFG") = 1 Then
      TableStatus.Change "PEST-FLAGS", 1, HspfStatusOptional
      If O.TableExists("PEST-FLAGS") Then
         Npest = O.Tables("PEST-FLAGS").Parms("NPST")
         Adopfg(1) = O.Tables("PEST-FLAGS").Parms("ADOPF1")
         Adopfg(2) = O.Tables("PEST-FLAGS").Parms("ADOPF2")
         Adopfg(3) = O.Tables("PEST-FLAGS").Parms("ADOPF3")
      Else
         Npest = 1
         Adopfg(1) = 2
      End If
      TableStatus.Change "PEST-AD-FLAGS", 1, HspfStatusOptional
      TableStatus.Change "SOIL-DATA", 1, HspfStatusRequired
      For i = 1 To Npest
        TableStatus.Change "PEST-ID", i, HspfStatusRequired
        If Adopfg(i) = 1 Then
          TableStatus.Change "PEST-THETA", i, HspfStatusOptional
          For j = 1 To 4
            n = 4 * (i - 1) + j
            TableStatus.Change "PEST-FIRSTPM", n, HspfStatusOptional
          Next j
        Else
          TableStatus.Change "PEST-CMAX", i, HspfStatusOptional
          For j = 1 To 4
            n = 4 * (i - 1) + j
            If Adopfg(i) = 2 Then
              TableStatus.Change "PEST-SVALPM", n, HspfStatusRequired
            Else
              TableStatus.Change "PEST-NONSVPM", n, HspfStatusRequired
            End If
          Next j
        End If
        TableStatus.Change "PEST-DEGRAD", i, HspfStatusOptional
        For j = 1 To 4
          n = 4 * (i - 1) + j
          TableStatus.Change "PEST-STOR1", n, HspfStatusOptional
        Next j
        TableStatus.Change "PEST-STOR2", i, HspfStatusOptional
      Next i
    End If
    If ltable.Parms("NITRFG") = 1 Then
      TableStatus.Change "NIT-FLAGS", 1, HspfStatusRequired
      If O.TableExists("NIT-FLAGS") Then
        Vnutfg = O.Tables("NIT-FLAGS").Parms("VNUTFG")
        Forafg = O.Tables("NIT-FLAGS").Parms("FORAFG")
        Nuptfg = O.Tables("NIT-FLAGS").Parms("NUPTFG")
        Amvofg = O.Tables("NIT-FLAGS").Parms("AMVOFG")
        Alpnfg = O.Tables("NIT-FLAGS").Parms("ALPNFG")
        Vnprfg = O.Tables("NIT-FLAGS").Parms("VNPRFG")
      Else
        Vnutfg = 0
        Forafg = 0
        Nuptfg = 0
        Amvofg = 0
        Alpnfg = 0
        Vnprfg = 0
      End If
      TableStatus.Change "NIT-AD-FLAGS", 1, HspfStatusOptional
      TableStatus.Change "SOIL-DATA", 1, HspfStatusRequired
      If Nuptfg = 0 Then
        If Vnutfg = 0 Then
          TableStatus.Change "NIT-UPTAKE", 1, HspfStatusOptional
        Else
          For j = 1 To 4
            TableStatus.Change "MON-NITUPT", j, HspfStatusOptional
          Next j
        End If
      ElseIf Nuptfg = 1 Then
        TableStatus.Change "NIT-YIELD", 1, HspfStatusOptional
        TableStatus.Change "SOIL-DATA2", 1, HspfStatusOptional
        TableStatus.Change "CROP-DATES", 1, HspfStatusOptional
        TableStatus.Change "MON-NUPT-FR1", 1, HspfStatusOptional
        For j = 1 To 4
          TableStatus.Change "MON-NUPT-FR2", j, HspfStatusOptional
        Next j
      Else
        If Nuptfg = -2 Then
          If Vnutfg = 0 Then
            TableStatus.Change "NIT-UPIMKMAX", 1, HspfStatusOptional
          Else
            TableStatus.Change "MON-NITUPNI", 1, HspfStatusOptional
            TableStatus.Change "MON-NITUPAM", 1, HspfStatusOptional
            TableStatus.Change "MON-NITIMNI", 1, HspfStatusOptional
            TableStatus.Change "MON-NITIMAM", 1, HspfStatusOptional
          End If
          TableStatus.Change "NIT-UPIMCSAT", 1, HspfStatusOptional
        Else
          For j = 1 To 4
            If Vnutfg = 0 Then
              TableStatus.Change "NIT-UPIMKMAX", j, HspfStatusOptional
            Else
              TableStatus.Change "MON-NITUPNI", j, HspfStatusOptional
              TableStatus.Change "MON-NITUPAM", j, HspfStatusOptional
              TableStatus.Change "MON-NITIMNI", j, HspfStatusOptional
              TableStatus.Change "MON-NITIMAM", j, HspfStatusOptional
            End If
            TableStatus.Change "NIT-UPIMCSAT", j, HspfStatusOptional
          Next j
        End If
      End If
      If Alpnfg = 1 Then
        If Vnutfg = 1 Then
          For j = 1 To 4
            TableStatus.Change "MON-NITAGUTF", j, HspfStatusOptional
          Next j
        Else
          TableStatus.Change "NIT-AGUTF", 1, HspfStatusOptional
        End If
      End If
      If Amvofg = 1 Then
        TableStatus.Change "NIT-AMVOLAT", 1, HspfStatusOptional
      End If
      If Vnprfg = 1 Then
        For j = 1 To 4
          TableStatus.Change "MON-NPRETBG", j, HspfStatusOptional
        Next j
        TableStatus.Change "MON-NPRETFBG", 1, HspfStatusOptional
        If Alpnfg = 1 Then
          TableStatus.Change "MON-NPRETAG", 1, HspfStatusOptional
          For j = 1 To 2
            TableStatus.Change "MON-NPRETLI", j, HspfStatusOptional
          Next j
          TableStatus.Change "MON-NPRETFLI", 1, HspfStatusOptional
        End If
      Else
        TableStatus.Change "NIT-BGPLRET", 1, HspfStatusOptional
        If Alpnfg = 1 Then
          TableStatus.Change "NIT-AGPLRET", 1, HspfStatusOptional
        End If
      End If
      TableStatus.Change "NIT-FSTGEN", 1, HspfStatusOptional
      For j = 1 To 4
        TableStatus.Change "NIT-FSTPM", j, HspfStatusOptional
      Next j
      If Forafg = 1 Then
        TableStatus.Change "NIT-CMAX", 1, HspfStatusOptional
        For j = 1 To 4
          TableStatus.Change "NIT-SVALPM", j, HspfStatusRequired
        Next j
      End If
      TableStatus.Change "NIT-ORGPM", 1, HspfStatusOptional
      For j = 1 To 4
        TableStatus.Change "NIT-STOR1", j, HspfStatusOptional
      Next j
      TableStatus.Change "NIT-STOR2", 1, HspfStatusOptional
    End If
    If ltable.Parms("PHOSFG") = 1 Then
      TableStatus.Change "PHOS-FLAGS", 1, HspfStatusRequired
      If O.TableExists("PHOS-FLAGS") Then
        Vputfg = O.Tables("PHOS-FLAGS").Parms("VPUTFG")
        Forpfg = O.Tables("PHOS-FLAGS").Parms("FORPFG")
        Puptfg = O.Tables("PHOS-FLAGS").Parms("PUPTFG")
      Else
        Vputfg = 0
        Forpfg = 0
        Puptfg = 0
      End If
      TableStatus.Change "PHOS-AD-FLAGS", 1, HspfStatusOptional
      TableStatus.Change "SOIL-DATA", 1, HspfStatusRequired
      If Puptfg = 0 Then
        If Vputfg = 0 Then
          TableStatus.Change "PHOS-UPTAKE", 1, HspfStatusOptional
        Else
          For j = 1 To 4
            TableStatus.Change "MON-PHOSUPT", j, HspfStatusOptional
          Next j
        End If
      ElseIf Puptfg = 1 Then
        TableStatus.Change "PHOS-YIELD", 1, HspfStatusOptional
        TableStatus.Change "SOIL-DATA2", 1, HspfStatusOptional
        TableStatus.Change "CROP-DATES", 1, HspfStatusOptional
        TableStatus.Change "MON-PUPT-FR1", 1, HspfStatusOptional
        For j = 1 To 4
          TableStatus.Change "MON-PUPT-FR2", j, HspfStatusOptional
        Next j
      End If
      TableStatus.Change "PHOS-FSTGEN", 1, HspfStatusOptional
      For j = 1 To 4
        TableStatus.Change "PHOS-FSTPM", j, HspfStatusOptional
      Next j
      If Forafg = 1 Then
        TableStatus.Change "PHOS-CMAX", 1, HspfStatusOptional
        For j = 1 To 4
          TableStatus.Change "PHOS-SVALPM", j, HspfStatusRequired
        Next j
      End If
      For j = 1 To 4
        TableStatus.Change "PHOS-STOR1", j, HspfStatusOptional
      Next j
      TableStatus.Change "PHOS-STOR2", 1, HspfStatusOptional
    End If
    If ltable.Parms("TRACFG") = 1 Then
      TableStatus.Change "TRAC-AD-FLAGS", 1, HspfStatusOptional
      TableStatus.Change "TRAC-ID", 1, HspfStatusOptional
      TableStatus.Change "TRAC-TOPSTOR", 1, HspfStatusOptional
      TableStatus.Change "TRAC-SUBSTOR", 1, HspfStatusOptional
    End If
  End If
End Sub
