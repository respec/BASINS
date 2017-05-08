Attribute VB_Name = "modStatusInputTimeseriesPerlnd"
Option Explicit
'Copyright 2002 AQUA TERRA Consultants - Royalty-free use permitted under open source license

Public Sub UpdateInputTimeseriesPerlnd(O As HspfOperation, TimserStatus As HspfStatus)
  Dim ltable As HspfTable, nquals&
  Dim i&, j&, icefg&, csnofg&, iffcfg&, pqadfg&(20), ctemp$
  Dim wetadfg&, dryadfg&, qualof&, qualif&, qualgw&, qualsd&
  Dim adopf1&, adopf2&, adopf3&, npst&, snopfg&, irrgfg&
  
  If O.TableExists("ACTIVITY") Then
    Set ltable = O.Tables("ACTIVITY")
    
    'section atemp
    If ltable.Parms("AIRTFG") = 1 Then
      TimserStatus.Change "EXTNL:GATMP", 1, HspfStatusRequired
      TimserStatus.Change "EXTNL:PREC", 1, HspfStatusRequired
    End If
    
    'section snow
    If O.TableExists("SNOW-FLAGS") Then
      snopfg = O.Tables("SNOW-FLAGS").Parms("SNOPFG")
    Else
      snopfg = 0
    End If
    If ltable.Parms("SNOWFG") = 1 Then
      TimserStatus.Change "EXTNL:PREC", 1, HspfStatusRequired
      If snopfg = 0 Then
        TimserStatus.Change "EXTNL:DTMPG", 1, HspfStatusRequired
        TimserStatus.Change "EXTNL:WINMOV", 1, HspfStatusRequired
        TimserStatus.Change "EXTNL:SOLRAD", 1, HspfStatusRequired
        TimserStatus.Change "EXTNL:CLOUD", 1, HspfStatusOptional
      Else
        TimserStatus.Change "EXTNL:DTMPG", 1, HspfStatusOptional
      End If
      If ltable.Parms("AIRTFG") = 0 Then
        TimserStatus.Change "ATEMP:AIRTMP", 1, HspfStatusRequired
      End If
    End If
    
    If O.TableExists("ICE-FLAG") Then
      icefg = O.Tables("ICE-FLAG").Parms("ICEFG")
    Else
      icefg = 0
    End If
    If O.TableExists("PWAT-PARM1") Then
      csnofg = O.Tables("PWAT-PARM1").Parms("CSNOFG")
      iffcfg = O.Tables("PWAT-PARM1").Parms("IFFCFG")
    Else
      csnofg = 0
      iffcfg = 0
    End If
    
    'section pwater
    If ltable.Parms("PWATFG") = 1 Then
      TimserStatus.Change "EXTNL:PETINP", 1, HspfStatusRequired
      If O.TableExists("PWAT-PARM1") Then
        irrgfg = O.Tables("PWAT-PARM1").Parms("IRRGFG")
      Else
        irrgfg = 0
      End If
      If irrgfg = 1 Then
        TimserStatus.Change "EXTNL:IRRINP", 1, HspfStatusRequired
      End If
      If csnofg = 0 Then
        TimserStatus.Change "EXTNL:PREC", 1, HspfStatusRequired
      End If
      TimserStatus.Change "EXTNL:SURLI", 1, HspfStatusOptional
      TimserStatus.Change "EXTNL:UZLI", 1, HspfStatusOptional
      TimserStatus.Change "EXTNL:IFWLI", 1, HspfStatusOptional
      TimserStatus.Change "EXTNL:LZLI", 1, HspfStatusOptional
      TimserStatus.Change "EXTNL:AGWLI", 1, HspfStatusOptional
      If csnofg = 1 Then
        If ltable.Parms("AIRTFG") = 0 Then
          TimserStatus.Change "ATEMP:AIRTMP", 1, HspfStatusRequired
        End If
        If ltable.Parms("SNOWFG") = 0 Then
          TimserStatus.Change "SNOW:RAINF", 1, HspfStatusRequired
          TimserStatus.Change "SNOW:SNOCOV", 1, HspfStatusRequired
          TimserStatus.Change "SNOW:WYIELD", 1, HspfStatusRequired
          If icefg = 1 Then
            TimserStatus.Change "SNOW:PACKI", 1, HspfStatusRequired
          End If
        End If
      End If
      If ltable.Parms("PSTFG") = 0 And iffcfg = 2 Then
        TimserStatus.Change "PSTEMP:LGTMP", 1, HspfStatusRequired
      End If
    End If
    
    'section sedmnt
    If ltable.Parms("SEDFG") = 1 Then
      TimserStatus.Change "EXTNL:PREC", 1, HspfStatusRequired
      TimserStatus.Change "EXTNL:SLSED", 1, HspfStatusOptional
      If ltable.Parms("SNOWFG") = 0 And csnofg = 1 Then
        TimserStatus.Change "SNOW:RAINF", 1, HspfStatusRequired
        TimserStatus.Change "SNOW:SNOCOV", 1, HspfStatusRequired
      End If
      If ltable.Parms("PWATFG") = 0 Then
        TimserStatus.Change "PWATER:SURO", 1, HspfStatusRequired
        TimserStatus.Change "PWATER:SURS", 1, HspfStatusRequired
      End If
    End If
    
    'section pstemp
    If ltable.Parms("PSTFG") = 1 Then
      If ltable.Parms("AIRTFG") = 0 Then
        TimserStatus.Change "ATEMP:AIRTMP", 1, HspfStatusRequired
      End If
    End If
    
    'section pwtgas
    If ltable.Parms("PWGFG") = 1 Then
      If ltable.Parms("SNOWFG") = 0 And csnofg = 1 Then
        TimserStatus.Change "SNOW:WYIELD", 1, HspfStatusRequired
      End If
      If ltable.Parms("PWATFG") = 0 Then
        TimserStatus.Change "PWATER:SURO", 1, HspfStatusRequired
        TimserStatus.Change "PWATER:IFWO", 1, HspfStatusRequired
        TimserStatus.Change "PWATER:AGWO", 1, HspfStatusRequired
      End If
      If ltable.Parms("PSTFG") = 0 Then
        TimserStatus.Change "PSTEMP:SLTMP", 1, HspfStatusRequired
        TimserStatus.Change "PSTEMP:ULTMP", 1, HspfStatusRequired
        TimserStatus.Change "PSTEMP:LGTMP", 1, HspfStatusRequired
      End If
    End If
    
    'section pqual
    If ltable.Parms("PQALFG") = 1 Then
      If O.TableExists("PQL-AD-FLAGS") Then
        With O.Tables("PQL-AD-FLAGS")
          For i = 1 To 20
            ctemp = "PQADFG(" & CStr(i) & ")"
            pqadfg(i) = .Parms(ctemp)
          Next i
        End With
      Else
        For i = 1 To 20
          pqadfg(i) = 0
        Next i
      End If
      If O.TableExists("NQUALS") Then
        nquals = O.Tables("NQUALS").Parms("NQUAL")
      Else
        nquals = 1
      End If
      qualof = 0
      qualif = 0
      qualgw = 0
      qualsd = 0
      For i = 1 To nquals
        wetadfg = 0
        dryadfg = 0
        If pqadfg(((i - 1) * 2) + 1) < 0 Then 'need input timeseries
          'dry ad simulated
          dryadfg = 1
        End If
        If pqadfg(((i - 1) * 2) + 2) < 0 Then 'need input timeseries
          'wet ad simulated
          wetadfg = 1
        End If
        ctemp = "QUAL-PROPS" & CStr(i)
        If O.TableExists(ctemp) Then
          If O.Tables(ctemp).Parms("QSOFG") > 0 Then
            qualof = 1
          End If
          If O.Tables(ctemp).Parms("QIFWFG") > 0 Then
            qualif = 1
          End If
          If O.Tables(ctemp).Parms("QAGWFG") > 0 Then
            qualgw = 1
          End If
          If O.Tables(ctemp).Parms("QSDFG") > 0 Then
            qualsd = 1
          End If
        End If

        If dryadfg = 1 Then
          TimserStatus.Change "EXTNL:PQADFX", i, HspfStatusRequired
        End If
        If wetadfg = 1 Then
          TimserStatus.Change "EXTNL:PQADCN", i, HspfStatusRequired
        End If
        If wetadfg = 1 Then
          TimserStatus.Change "EXTNL:PREC", 1, HspfStatusRequired
        End If
      Next i
      If ltable.Parms("PWATFG") = 0 Then
        If qualof > 0 Then
          TimserStatus.Change "PWATER:SURO", 1, HspfStatusRequired 'or req if SOQC is required for one or more QUALs
        End If
        If qualif > 0 Then
          TimserStatus.Change "PWATER:IFWO", 1, HspfStatusRequired
        End If
        If qualgw > 0 Then
          TimserStatus.Change "PWATER:AGWO", 1, HspfStatusRequired
        End If
        TimserStatus.Change "PWATER:PERO", 1, HspfStatusRequired 'only required if POQC is required for one or more QUALs
      End If
      If ltable.Parms("SEDFG") = 0 And qualsd > 0 Then
        TimserStatus.Change "SEDMNT:WSSD", 1, HspfStatusRequired
        TimserStatus.Change "SEDMNT:SCRSD", 1, HspfStatusRequired
      End If
    End If
    
    'section mstlay
    If ltable.Parms("MSTLFG") = 1 Then
      If ltable.Parms("PWATFG") = 0 Then
        TimserStatus.Change "PWATER:SURI", 1, HspfStatusRequired
        TimserStatus.Change "PWATER:LZS", 1, HspfStatusRequired
        TimserStatus.Change "PWATER:IGWI", 1, HspfStatusRequired
        TimserStatus.Change "PWATER:AGWI", 1, HspfStatusRequired
        TimserStatus.Change "PWATER:AGWS", 1, HspfStatusRequired
        TimserStatus.Change "PWATER:AGWO", 1, HspfStatusRequired
        TimserStatus.Change "PWATER:SURS", 1, HspfStatusRequired
        TimserStatus.Change "PWATER:SURO", 1, HspfStatusRequired
        TimserStatus.Change "PWATER:INFIL", 1, HspfStatusRequired
        TimserStatus.Change "PWATER:IFWI", 1, HspfStatusRequired
        TimserStatus.Change "PWATER:UZI", 1, HspfStatusRequired
        TimserStatus.Change "PWATER:UZS", 1, HspfStatusRequired
        TimserStatus.Change "PWATER:PERC", 1, HspfStatusRequired
        TimserStatus.Change "PWATER:IFWS", 1, HspfStatusRequired
        TimserStatus.Change "PWATER:IFWO", 1, HspfStatusRequired
      End If
    End If
    
    'section pest
    If ltable.Parms("PESTFG") = 1 Then
      If O.TableExists("PEST-FLAGS") Then
        npst = O.Tables("PEST-FLAGS").Parms("NPST")
      Else
        npst = 0
      End If
      If dryadfg = 1 Then
        For i = 1 To npst
          TimserStatus.Change2 "EXTNL:PEADFX", i, 1, HspfStatusRequired
          TimserStatus.Change2 "EXTNL:PEADFX", i, 2, HspfStatusRequired
          TimserStatus.Change2 "EXTNL:PEADFX", i, 3, HspfStatusRequired
        Next i
      End If
      If wetadfg = 1 Then
        For i = 1 To npst
          TimserStatus.Change2 "EXTNL:PEADCN", i, 1, HspfStatusRequired
          TimserStatus.Change2 "EXTNL:PEADCN", i, 2, HspfStatusRequired
          TimserStatus.Change2 "EXTNL:PEADCN", i, 3, HspfStatusRequired
        Next i
        TimserStatus.Change "EXTNL:PREC", 1, HspfStatusRequired
      End If
      If ltable.Parms("SEDFG") = 0 Then
        TimserStatus.Change "SEDMNT:SOSED", 1, HspfStatusRequired
      End If
      If ltable.Parms("MSTLFG") = 0 Then
        For i = 1 To 5
          TimserStatus.Change "MSTLAY:MST", i, HspfStatusRequired
        Next i
        'For i = 1 To 8
        '  TimserStatus.Change "MSTLAY:FRAC", i, HspfStatusRequired
        'Next i
      End If
      If O.TableExists("PEST-FLAGS") Then
        adopf1 = O.Tables("PEST-FLAGS").Parms("ADOPF1")
        adopf2 = O.Tables("PEST-FLAGS").Parms("ADOPF2")
        adopf3 = O.Tables("PEST-FLAGS").Parms("ADOPF3")
      Else
        adopf1 = 0
        adopf2 = 0
        adopf3 = 0
      End If
      If adopf1 > 0 Or adopf2 > 0 Or adopf3 > 0 Then
        If ltable.Parms("PSTFG") = 0 Then
          TimserStatus.Change "PSTEMP:SLTMP", 1, HspfStatusRequired
          TimserStatus.Change "PSTEMP:ULTMP", 1, HspfStatusRequired
          TimserStatus.Change "PSTEMP:LGTMP", 1, HspfStatusRequired
        End If
      End If
    End If
    
    'section nitr
    If ltable.Parms("NITRFG") = 1 Then
      If dryadfg = 1 Then
        For i = 1 To 3
          For j = 1 To 2
            TimserStatus.Change2 "EXTNL:NIADFX", i, j, HspfStatusRequired
          Next j
        Next i
      End If
      If wetadfg = 1 Then
        For i = 1 To 3
          For j = 1 To 2
            TimserStatus.Change2 "EXTNL:NIADCN", i, j, HspfStatusRequired
          Next j
        Next i
        TimserStatus.Change "EXTNL:PREC", 1, HspfStatusRequired
      End If
      If ltable.Parms("PESTFG") = 0 Then
        If ltable.Parms("SEDFG") = 0 Then
          TimserStatus.Change "SEDMNT:SOSED", 1, HspfStatusRequired
        End If
        If ltable.Parms("MSTLFG") = 0 Then
          For i = 1 To 5
            TimserStatus.Change "MSTLAY:MST", i, HspfStatusRequired
          Next i
          'For i = 1 To 8
          '  TimserStatus.Change "MSTLAY:FRAC", i, HspfStatusRequired
          'Next i
        End If
        If O.TableExists("PEST-FLAGS") Then
          adopf1 = O.Tables("PEST-FLAGS").Parms("ADOPF1")
          adopf2 = O.Tables("PEST-FLAGS").Parms("ADOPF2")
          adopf3 = O.Tables("PEST-FLAGS").Parms("ADOPF3")
        Else
          adopf1 = 0
          adopf2 = 0
          adopf3 = 0
        End If
        If adopf1 > 0 Or adopf2 > 0 Or adopf3 > 0 Then
          If ltable.Parms("PSTFG") = 0 Then
            TimserStatus.Change "PSTEMP:SLTMP", 1, HspfStatusRequired
            TimserStatus.Change "PSTEMP:ULTMP", 1, HspfStatusRequired
            TimserStatus.Change "PSTEMP:LGTMP", 1, HspfStatusRequired
          End If
        End If
      End If
    End If
    
    'section phos
    If ltable.Parms("PHOSFG") = 1 Then
      If dryadfg = 1 Then
        For i = 1 To 2
          For j = 1 To 2
            TimserStatus.Change2 "EXTNL:PHADFX", i, j, HspfStatusRequired
          Next j
        Next i
      End If
      If wetadfg = 1 Then
        For i = 1 To 2
          For j = 1 To 2
            TimserStatus.Change2 "EXTNL:PHADCN", i, j, HspfStatusRequired
          Next j
        Next i
        TimserStatus.Change "EXTNL:PREC", 1, HspfStatusRequired
      End If
      If ltable.Parms("PESTFG") = 0 Then
        If ltable.Parms("SEDFG") = 0 Then
          TimserStatus.Change "SEDMNT:SOSED", 1, HspfStatusRequired
        End If
        If ltable.Parms("MSTLFG") = 0 Then
          For i = 1 To 5
            TimserStatus.Change "MSTLAY:MST", i, HspfStatusRequired
          Next i
          'For i = 1 To 8
          '  TimserStatus.Change "MSTLAY:FRAC", i, HspfStatusRequired
          'Next i
        End If
        If O.TableExists("PEST-FLAGS") Then
          adopf1 = O.Tables("PEST-FLAGS").Parms("ADOPF1")
          adopf2 = O.Tables("PEST-FLAGS").Parms("ADOPF2")
          adopf3 = O.Tables("PEST-FLAGS").Parms("ADOPF3")
        Else
          adopf1 = 0
          adopf2 = 0
          adopf3 = 0
        End If
        If adopf1 > 0 Or adopf2 > 0 Or adopf3 > 0 Then
          If ltable.Parms("PSTFG") = 0 Then
            TimserStatus.Change "PSTEMP:SLTMP", 1, HspfStatusRequired
            TimserStatus.Change "PSTEMP:ULTMP", 1, HspfStatusRequired
            TimserStatus.Change "PSTEMP:LGTMP", 1, HspfStatusRequired
          End If
        End If
      End If
    End If
    
    'section tracer
    If ltable.Parms("TRACFG") = 1 Then
      If dryadfg = 1 Then
        TimserStatus.Change "EXTNL:TRADFX", 1, HspfStatusRequired
      End If
      If wetadfg = 1 Then
        TimserStatus.Change "EXTNL:TRADCN", 1, HspfStatusRequired
        TimserStatus.Change "EXTNL:PREC", 1, HspfStatusRequired
      End If
      If ltable.Parms("MSTLFG") = 0 And ltable.Parms("PESTFG") = 0 And _
         ltable.Parms("NITRFG") = 0 And ltable.Parms("PHOSFG") = 0 Then
        For i = 1 To 5
          TimserStatus.Change "MSTLAY:MST", i, HspfStatusRequired
        Next i
        'For i = 1 To 8
        '  TimserStatus.Change "MSTLAY:FRAC", i, HspfStatusRequired
        'Next i
      End If
    End If
    
  End If
End Sub
