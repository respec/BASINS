Attribute VB_Name = "modStatusOutputTimeseriesImplnd"
Option Explicit
'Copyright 2002 AQUA TERRA Consultants - Royalty-free use permitted under open source license

Public Sub UpdateOutputTimeseriesImplnd(O As HspfOperation, TimserStatus As HspfStatus)
  Dim ltable As HspfTable, nquals&
  Dim i&, icefg&, csnofg&, iffcfg&, iqadfg&(20), ctemp$
  Dim nqof&, nqsd&, snopfg&
  
  If O.TableExists("ACTIVITY") Then
    Set ltable = O.Tables("ACTIVITY")
    
    'section atemp
    If ltable.Parms("ATMPFG") = 1 Then
      TimserStatus.Change "ATEMP:AIRTMP", 1, HspfStatusOptional
    End If
    
    'section snow
    If O.TableExists("SNOW-FLAGS") Then
      snopfg = O.Tables("SNOW-FLAGS").Parms("SNOPFG")
    Else
      snopfg = 0
    End If
    If ltable.Parms("SNOWFG") = 1 Then
      TimserStatus.Change "SNOW:PACK", 1, HspfStatusOptional
      TimserStatus.Change "SNOW:PACKF", 1, HspfStatusOptional
      TimserStatus.Change "SNOW:PACKW", 1, HspfStatusOptional
      TimserStatus.Change "SNOW:PACKI", 1, HspfStatusOptional
      TimserStatus.Change "SNOW:PDEPTH", 1, HspfStatusOptional
      TimserStatus.Change "SNOW:COVINX", 1, HspfStatusOptional
      TimserStatus.Change "SNOW:NEGHTS", 1, HspfStatusOptional
      TimserStatus.Change "SNOW:XLNMLT", 1, HspfStatusOptional
      TimserStatus.Change "SNOW:RDENPF", 1, HspfStatusOptional
      TimserStatus.Change "SNOW:SKYCLR", 1, HspfStatusOptional
      TimserStatus.Change "SNOW:SNOCOV", 1, HspfStatusOptional
      If snopfg = 0 Then
        TimserStatus.Change "SNOW:DULL", 1, HspfStatusOptional
        TimserStatus.Change "SNOW:ALBEDO", 1, HspfStatusOptional
      End If
      TimserStatus.Change "SNOW:PAKTMP", 1, HspfStatusOptional
      TimserStatus.Change "SNOW:SNOTMP", 1, HspfStatusOptional
      TimserStatus.Change "SNOW:DEWTMP", 1, HspfStatusOptional
      TimserStatus.Change "SNOW:SNOWF", 1, HspfStatusOptional
      TimserStatus.Change "SNOW:PRAIN", 1, HspfStatusOptional
      If snopfg = 0 Then
        TimserStatus.Change "SNOW:SNOWE", 1, HspfStatusOptional
      End If
      TimserStatus.Change "SNOW:WYIELD", 1, HspfStatusOptional
      TimserStatus.Change "SNOW:MELT", 1, HspfStatusOptional
      TimserStatus.Change "SNOW:RAINF", 1, HspfStatusOptional
    End If
    
    'section iwater
    If ltable.Parms("IWATFG") = 1 Then
      TimserStatus.Change "IWATER:IMPS", 1, HspfStatusOptional
      TimserStatus.Change "IWATER:RETS", 1, HspfStatusOptional
      TimserStatus.Change "IWATER:SURS", 1, HspfStatusOptional
      TimserStatus.Change "IWATER:PETADJ", 1, HspfStatusOptional
      TimserStatus.Change "IWATER:SUPY", 1, HspfStatusOptional
      TimserStatus.Change "IWATER:SURO", 1, HspfStatusOptional
      TimserStatus.Change "IWATER:PET", 1, HspfStatusOptional
      TimserStatus.Change "IWATER:IMPEV", 1, HspfStatusOptional
      TimserStatus.Change "IWATER:SURI", 1, HspfStatusOptional
    End If
    
    'section solids
    If ltable.Parms("SLDFG") = 1 Then
      TimserStatus.Change "SOLIDS:SLDS", 1, HspfStatusOptional
      TimserStatus.Change "SOLIDS:SOSLD", 1, HspfStatusOptional
    End If
    
    'section iwtgas
    If ltable.Parms("IWGFG") = 1 Then
      TimserStatus.Change "IWTGAS:SOTMP", 1, HspfStatusOptional
      TimserStatus.Change "IWTGAS:SODOX", 1, HspfStatusOptional
      TimserStatus.Change "IWTGAS:SOCO2", 1, HspfStatusOptional
      TimserStatus.Change "IWTGAS:SOHT", 1, HspfStatusOptional
      TimserStatus.Change "IWTGAS:SODOXM", 1, HspfStatusOptional
      TimserStatus.Change "IWTGAS:SOCO2M", 1, HspfStatusOptional
    End If
    
    'section iqual
    If ltable.Parms("IQALFG") = 1 Then
      If O.TableExists("NQUALS") Then
        nquals = O.Tables("NQUALS").Parms("NQUAL")
      Else
        nquals = 1
      End If
      nqof = 0
      nqsd = 0
      For i = 1 To nquals
        ctemp = "QUAL-PROPS" & CStr(i)
        If O.TableExists(ctemp) Then
          If O.Tables(ctemp).Parms("QSOFG") > 0 Then
            nqof = nqof + 1
          End If
          If O.Tables(ctemp).Parms("QSDFG") > 0 Then
            nqsd = nqsd + 1
          End If
        End If
      Next i
    
      For i = 1 To nquals
        TimserStatus.Change "IQUAL:SOQUAL", i, HspfStatusOptional
        TimserStatus.Change "IQUAL:SOQC", i, HspfStatusOptional
        TimserStatus.Change "IQUAL:IQADDR", i, HspfStatusOptional
        TimserStatus.Change "IQUAL:IQADWT", i, HspfStatusOptional
        TimserStatus.Change "IQUAL:IQADEP", i, HspfStatusOptional
      Next i
      For i = 1 To nqof
        TimserStatus.Change "IQUAL:SQO", i, HspfStatusOptional
        TimserStatus.Change "IQUAL:SOQO", i, HspfStatusOptional
        TimserStatus.Change "IQUAL:SOQOC", i, HspfStatusOptional
      Next i
      For i = 1 To nqsd
        TimserStatus.Change "IQUAL:SOQS", i, HspfStatusOptional
        TimserStatus.Change "IQUAL:SOQSP", i, HspfStatusOptional
      Next i
    End If
    
  End If
End Sub



