Attribute VB_Name = "modStatusTimeseriesUtility"
Option Explicit
'Copyright 2002 AQUA TERRA Consultants - Royalty-free use permitted under open source license

Public Sub UpdateInputTimeseriesDuranl(O As HspfOperation, TimserStatus As HspfStatus)
  'only timser for duranl is the input timser
  TimserStatus.Change "INPUT:TIMSER", 1, HspfStatusRequired
End Sub

Public Sub UpdateOutputTimeseriesDuranl(O As HspfOperation, TimserStatus As HspfStatus)
  'no output timsers for duranl
End Sub

Public Sub UpdateInputTimeseriesCopy(O As HspfOperation, TimserStatus As HspfStatus)
  Dim npt&, nmn&, i&
  
  If O.TableExists("TIMESERIES") Then
    npt = O.Tables("TIMESERIES").Parms("NPT")
    nmn = O.Tables("TIMESERIES").Parms("NMN")
  Else
    npt = 0
    nmn = 0
  End If
  For i = 1 To npt
    TimserStatus.Change "INPUT:POINT", i, HspfStatusOptional
  Next i
  For i = 1 To nmn
    TimserStatus.Change "INPUT:MEAN", i, HspfStatusOptional
  Next i
End Sub

Public Sub UpdateOutputTimeseriesCopy(O As HspfOperation, TimserStatus As HspfStatus)
  Dim npt&, nmn&, i&
  
  If O.TableExists("TIMESERIES") Then
    npt = O.Tables("TIMESERIES").Parms("NPT")
    nmn = O.Tables("TIMESERIES").Parms("NMN")
  Else
    npt = 0
    nmn = 0
  End If
  For i = 1 To npt
    TimserStatus.Change "OUTPUT:POINT", i, HspfStatusOptional
  Next i
  For i = 1 To nmn
    TimserStatus.Change "OUTPUT:MEAN", i, HspfStatusOptional
  Next i
End Sub

Public Sub UpdateInputTimeseriesPltgen(O As HspfOperation, TimserStatus As HspfStatus)
  'only timser for pltgen is the input timser
  Dim npt&, nmn&, i&
  
  If O.TableExists("PLOTINFO") Then
    npt = O.Tables("PLOTINFO").Parms("NPT")
    nmn = O.Tables("PLOTINFO").Parms("NMN")
  Else
    npt = 0
    nmn = 0
  End If
  For i = 1 To npt
    TimserStatus.Change "INPUT:POINT", i, HspfStatusRequired
  Next i
  For i = 1 To nmn
    TimserStatus.Change "INPUT:MEAN", i, HspfStatusRequired
  Next i
End Sub

Public Sub UpdateOutputTimeseriesPltgen(O As HspfOperation, TimserStatus As HspfStatus)
  'no output timsers for pltgen
End Sub

Public Sub UpdateInputTimeseriesDisply(O As HspfOperation, TimserStatus As HspfStatus)
  'only timser for disply is the input timser
  TimserStatus.Change "INPUT:TIMSER", 1, HspfStatusRequired
End Sub

Public Sub UpdateOutputTimeseriesDisply(O As HspfOperation, TimserStatus As HspfStatus)
  'no output timsers for disply
End Sub

Public Sub UpdateInputTimeseriesGener(O As HspfOperation, TimserStatus As HspfStatus)
  'two possible input timsers for gener
  Dim Opcode&
  
  If O.TableExists("OPCODE") Then
    Opcode = O.Tables("OPCODE").Parms("OPCODE")
  Else
    Opcode = 0
  End If
  If Opcode > 0 And Opcode <> 24 Then
    TimserStatus.Change "INPUT:ONE", 1, HspfStatusRequired
  End If
  If Opcode > 15 And Opcode < 24 Then
    TimserStatus.Change "INPUT:TWO", 1, HspfStatusRequired
  End If
End Sub

Public Sub UpdateOutputTimeseriesGener(O As HspfOperation, TimserStatus As HspfStatus)
  'only one output timser for gener
  TimserStatus.Change "OUTPUT:TIMSER", 1, HspfStatusOptional
End Sub

Public Sub UpdateInputTimeseriesMutsin(O As HspfOperation, TimserStatus As HspfStatus)
  'no input timsers for mutsin
End Sub

Public Sub UpdateOutputTimeseriesMutsin(O As HspfOperation, TimserStatus As HspfStatus)
  'only timser for mutsin is the output timser
  Dim npt&, nmn&, i&
  
  If O.TableExists("MUTSINFO") Then
    npt = O.Tables("MUTSINFO").Parms("NPT")
    nmn = O.Tables("MUTSINFO").Parms("NMN")
  Else
    npt = 0
    nmn = 0
  End If
  For i = 1 To npt
    TimserStatus.Change "OUTPUT:POINT", i, HspfStatusRequired
  Next i
  For i = 1 To nmn
    TimserStatus.Change "OUTPUT:MEAN", i, HspfStatusRequired
  Next i
End Sub

Public Sub UpdateInputTimeseriesReport(O As HspfOperation, TimserStatus As HspfStatus)
  Dim ncon&, i&
  
  If O.TableExists("REPORT-FLAGS") Then
    ncon = O.Tables("REPORT-FLAGS").Parms("NCON")
  Else
    ncon = 0
  End If
  For i = 1 To ncon
    TimserStatus.Change "INPUT:TIMSER", i, HspfStatusRequired
  Next i
End Sub

Public Sub UpdateOutputTimeseriesReport(O As HspfOperation, TimserStatus As HspfStatus)
  'no output timsers for report
End Sub

Public Sub UpdateInputTimeseriesBmprac(O As HspfOperation, TimserStatus As HspfStatus)
  'input timsers for bmprac
  Dim nCons&, nGqual&, i&
  
  If O.TableExists("GEN-INFO") Then
    nCons = O.Tables("GEN-INFO").Parms("NCONS")
    nGqual = O.Tables("GEN-INFO").Parms("NGQUAL")
  Else
    nCons = 0
    nGqual = 0
  End If
  
  'group inflow
  TimserStatus.Change "INFLOW:IVOL", 1, HspfStatusOptional
  If Not O.Uci.CategoryBlock Is Nothing Then
    'have category block
    For i = 1 To O.Uci.CategoryBlock.Count
      TimserStatus.Change "INFLOW:CIVOL", i, HspfStatusOptional
    Next i
  End If
  For i = 1 To nCons
    TimserStatus.Change "INFLOW:ICON", i, HspfStatusOptional
  Next i
  TimserStatus.Change "INFLOW:IHEAT", 1, HspfStatusOptional
  For i = 1 To 3
    TimserStatus.Change "INFLOW:ISED", i, HspfStatusOptional
  Next i
  For i = 1 To nGqual
    TimserStatus.Change "INFLOW:IDQAL", i, HspfStatusOptional
  Next i
  For i = 1 To nGqual
    TimserStatus.Change2 "INFLOW:ISQAL", 1, i, HspfStatusOptional
    TimserStatus.Change2 "INFLOW:ISQAL", 2, i, HspfStatusOptional
    TimserStatus.Change2 "INFLOW:ISQAL", 3, i, HspfStatusOptional
  Next i
  For i = 1 To 2
    TimserStatus.Change "INFLOW:IOX", i, HspfStatusOptional
  Next i
  For i = 1 To 4
    TimserStatus.Change "INFLOW:IDNUT", i, HspfStatusOptional
  Next i
  For i = 1 To 3
    TimserStatus.Change2 "INFLOW:ISNUT", i, 1, HspfStatusOptional
    TimserStatus.Change2 "INFLOW:ISNUT", i, 2, HspfStatusOptional
  Next i
  For i = 1 To 5
    TimserStatus.Change "INFLOW:IPLK", i, HspfStatusOptional
  Next i
  For i = 1 To 2
    TimserStatus.Change "INFLOW:IPH", i, HspfStatusOptional
  Next i
  
End Sub

Public Sub UpdateOutputTimeseriesBmprac(O As HspfOperation, TimserStatus As HspfStatus)
  'output timsers for bmprac
  Dim nCons&, nGqual&, i&
  
  If O.TableExists("GEN-INFO") Then
    nCons = O.Tables("GEN-INFO").Parms("NCONS")
    nGqual = O.Tables("GEN-INFO").Parms("NGQUAL")
  Else
    nCons = 0
    nGqual = 0
  End If
  
  'group receiv
  TimserStatus.Change "RECEIV:IVOL", 1, HspfStatusOptional
  If Not O.Uci.CategoryBlock Is Nothing Then
    'have category block
    For i = 1 To O.Uci.CategoryBlock.Count
      TimserStatus.Change "RECEIV:CIVOL", i, HspfStatusOptional
    Next i
  End If
  For i = 1 To nCons
    TimserStatus.Change "RECEIV:ICON", 1, HspfStatusOptional
  Next i
  TimserStatus.Change "RECEIV:IHEAT", 1, HspfStatusOptional
  For i = 1 To 3
    TimserStatus.Change "RECEIV:ISED", i, HspfStatusOptional
  Next i
  For i = 1 To nGqual
    TimserStatus.Change "RECEIV:IDQAL", i, HspfStatusOptional
  Next i
  For i = 1 To nGqual
    TimserStatus.Change2 "RECEIV:ISQAL", 1, i, HspfStatusOptional
    TimserStatus.Change2 "RECEIV:ISQAL", 2, i, HspfStatusOptional
    TimserStatus.Change2 "RECEIV:ISQAL", 3, i, HspfStatusOptional
  Next i
  For i = 1 To 2
    TimserStatus.Change "RECEIV:IOX", i, HspfStatusOptional
  Next i
  For i = 1 To 4
    TimserStatus.Change "RECEIV:IDNUT", i, HspfStatusOptional
  Next i
  For i = 1 To 3
    TimserStatus.Change2 "RECEIV:ISNUT", i, 1, HspfStatusOptional
    TimserStatus.Change2 "RECEIV:ISNUT", i, 2, HspfStatusOptional
  Next i
  For i = 1 To 5
    TimserStatus.Change "RECEIV:IPLK", i, HspfStatusOptional
  Next i
  For i = 1 To 2
    TimserStatus.Change "RECEIV:IPH", i, HspfStatusOptional
  Next i
  
  'group roflow
  TimserStatus.Change "ROFLOW:ROVOL", 1, HspfStatusOptional
  If Not O.Uci.CategoryBlock Is Nothing Then
    'have category block
    For i = 1 To O.Uci.CategoryBlock.Count
      TimserStatus.Change "ROFLOW:CROVOL", i, HspfStatusOptional
    Next i
  End If
  For i = 1 To nCons
    TimserStatus.Change "ROFLOW:ROCON", 1, HspfStatusOptional
  Next i
  TimserStatus.Change "ROFLOW:ROHEAT", 1, HspfStatusOptional
  For i = 1 To 3
    TimserStatus.Change "ROFLOW:ROSED", i, HspfStatusOptional
  Next i
  For i = 1 To nGqual
    TimserStatus.Change "ROFLOW:RODQAL", i, HspfStatusOptional
  Next i
  For i = 1 To nGqual
    TimserStatus.Change2 "ROFLOW:ROSQAL", 1, i, HspfStatusOptional
    TimserStatus.Change2 "ROFLOW:ROSQAL", 2, i, HspfStatusOptional
    TimserStatus.Change2 "ROFLOW:ROSQAL", 3, i, HspfStatusOptional
  Next i
  For i = 1 To 2
    TimserStatus.Change "ROFLOW:ROOX", i, HspfStatusOptional
  Next i
  For i = 1 To 4
    TimserStatus.Change "ROFLOW:RODNUT", i, HspfStatusOptional
  Next i
  For i = 1 To 3
    TimserStatus.Change2 "ROFLOW:ROSNUT", i, 1, HspfStatusOptional
    TimserStatus.Change2 "ROFLOW:ROSNUT", i, 2, HspfStatusOptional
  Next i
  For i = 1 To 5
    TimserStatus.Change "ROFLOW:ROPLK", i, HspfStatusOptional
  Next i
  For i = 1 To 2
    TimserStatus.Change "ROFLOW:ROPH", i, HspfStatusOptional
  Next i
  
  'group remove
  TimserStatus.Change "REMOVE:RMVOL", 1, HspfStatusOptional
  If Not O.Uci.CategoryBlock Is Nothing Then
    'have category block
    For i = 1 To O.Uci.CategoryBlock.Count
      TimserStatus.Change "REMOVE:CRMVOL", i, HspfStatusOptional
    Next i
  End If
  For i = 1 To nCons
    TimserStatus.Change "REMOVE:RMCON", 1, HspfStatusOptional
  Next i
  TimserStatus.Change "REMOVE:RMHEAT", 1, HspfStatusOptional
  For i = 1 To 3
    TimserStatus.Change "REMOVE:RMSED", i, HspfStatusOptional
  Next i
  For i = 1 To nGqual
    TimserStatus.Change "REMOVE:RMDQAL", i, HspfStatusOptional
  Next i
  For i = 1 To nGqual
    TimserStatus.Change2 "REMOVE:RMSQAL", 1, i, HspfStatusOptional
    TimserStatus.Change2 "REMOVE:RMSQAL", 2, i, HspfStatusOptional
    TimserStatus.Change2 "REMOVE:RMSQAL", 3, i, HspfStatusOptional
  Next i
  For i = 1 To 2
    TimserStatus.Change "REMOVE:RMOX", i, HspfStatusOptional
  Next i
  For i = 1 To 4
    TimserStatus.Change "REMOVE:RMDNUT", i, HspfStatusOptional
  Next i
  For i = 1 To 3
    TimserStatus.Change2 "REMOVE:RMSNUT", i, 1, HspfStatusOptional
    TimserStatus.Change2 "REMOVE:RMSNUT", i, 2, HspfStatusOptional
  Next i
  For i = 1 To 5
    TimserStatus.Change "REMOVE:RMPLK", i, HspfStatusOptional
  Next i
  For i = 1 To 2
    TimserStatus.Change "REMOVE:RMPH", i, HspfStatusOptional
  Next i
  
End Sub
