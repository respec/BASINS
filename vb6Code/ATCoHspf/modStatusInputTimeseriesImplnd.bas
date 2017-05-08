Attribute VB_Name = "modStatusInputTimeseriesImplnd"
Option Explicit
'Copyright 2002 AQUA TERRA Consultants - Royalty-free use permitted under open source license

Public Sub UpdateInputTimeseriesImplnd(O As HspfOperation, TimserStatus As HspfStatus)
  Dim ltable As HspfTable, nquals&
  Dim i&, icefg&, csnofg&, iffcfg&, iqadfg&(20), ctemp$
  Dim wetadfg&, dryadfg&, qualof&, qualif&, qualgw&, qualsd&, snopfg&
  
  If O.TableExists("ACTIVITY") Then
    Set ltable = O.Tables("ACTIVITY")
    
    'section atemp
    If ltable.Parms("ATMPFG") = 1 Then
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
      If ltable.Parms("ATMPFG") = 0 Then
        TimserStatus.Change "ATEMP:AIRTMP", 1, HspfStatusRequired
      End If
    End If
    
    If O.TableExists("IWAT-PARM1") Then
      csnofg = O.Tables("IWAT-PARM1").Parms("CSNOFG")
    Else
      csnofg = 0
    End If
    
    'section iwater
    If ltable.Parms("IWATFG") = 1 Then
      TimserStatus.Change "EXTNL:PETINP", 1, HspfStatusRequired
      TimserStatus.Change "EXTNL:SURLI", 1, HspfStatusOptional
      If csnofg = 0 Then
        TimserStatus.Change "EXTNL:PREC", 1, HspfStatusRequired
      End If
      If csnofg = 1 Then
        If ltable.Parms("ATMPFG") = 0 Then
          TimserStatus.Change "ATEMP:AIRTMP", 1, HspfStatusRequired
        End If
        If ltable.Parms("SNOWFG") = 0 Then
          TimserStatus.Change "SNOW:RAINF", 1, HspfStatusRequired
          TimserStatus.Change "SNOW:SNOCOV", 1, HspfStatusRequired
          TimserStatus.Change "SNOW:WYIELD", 1, HspfStatusRequired
        End If
      End If
    End If
    
    'section solids
    If ltable.Parms("SLDFG") = 1 Then
      TimserStatus.Change "EXTNL:PREC", 1, HspfStatusRequired
      TimserStatus.Change "EXTNL:SLSLD", 1, HspfStatusOptional
      If ltable.Parms("IWATFG") = 0 Then
        TimserStatus.Change "IWATER:SURO", 1, HspfStatusRequired
        TimserStatus.Change "IWATER:SURS", 1, HspfStatusRequired
      End If
    End If
    
    'section iwtgas
    If ltable.Parms("IWGFG") = 1 Then
      If ltable.Parms("ATMPFG") = 0 Then
          TimserStatus.Change "ATEMP:AIRTMP", 1, HspfStatusRequired
        End If
      If ltable.Parms("SNOWFG") = 0 And csnofg = 1 Then
        TimserStatus.Change "SNOW:WYIELD", 1, HspfStatusRequired
      End If
      If ltable.Parms("IWATFG") = 0 Then
        TimserStatus.Change "IWATER:SURO", 1, HspfStatusRequired
      End If
    End If
    
    If O.TableExists("IQL-AD-FLAGS") Then
      With O.Tables("IQL-AD-FLAGS")
        For i = 1 To 20
          ctemp = "IQADFG(" & CStr(i) & ")"
          iqadfg(i) = .Parms(ctemp)
        Next i
      End With
    Else
      For i = 1 To 20
        iqadfg(i) = 0
      Next i
    End If
    
    'section iqual
    If ltable.Parms("IQALFG") = 1 Then
      If O.TableExists("NQUALS") Then
        nquals = O.Tables("NQUALS").Parms("NQUAL")
      Else
        nquals = 1
      End If
      wetadfg = 0
      dryadfg = 0
      qualof = 0
      qualif = 0
      qualgw = 0
      qualsd = 0
      For i = 1 To nquals
        wetadfg = 0
        dryadfg = 0
        If iqadfg(((i - 1) * 2) + 1) < 0 Then 'need input timeseries
          'dry ad simulated
          dryadfg = 1
        End If
        If iqadfg(((i - 1) * 2) + 2) < 0 Then 'need input timeseries
          'wet ad simulated
          wetadfg = 1
        End If
        ctemp = "QUAL-PROPS" & CStr(i)
        If O.TableExists(ctemp) Then
          If O.Tables(ctemp).Parms("QSOFG") > 0 Then
            qualof = 1
          End If
          If O.Tables(ctemp).Parms("QSDFG") > 0 Then
            qualsd = 1
          End If
        End If
        If dryadfg = 1 Then
          TimserStatus.Change "EXTNL:IQADFX", i, HspfStatusRequired
        End If
        If wetadfg = 1 Then
          TimserStatus.Change "EXTNL:IQADCN", i, HspfStatusRequired
        End If
        If wetadfg = 1 Then
          TimserStatus.Change "EXTNL:PREC", 1, HspfStatusRequired
        End If
      Next i
      
      If ltable.Parms("IWATFG") = 0 Then
        If qualof > 0 Then
          TimserStatus.Change "PWATER:SURO", 1, HspfStatusRequired 'or req if SOQC is required for one or more QUALs
        End If
      End If
      If ltable.Parms("SLDFG") = 0 And qualsd > 0 Then
        TimserStatus.Change "SOLIDS:SOSLD", 1, HspfStatusRequired
      End If
    End If
    
  End If
End Sub


