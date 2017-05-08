Attribute VB_Name = "modStatusUtility"
Option Explicit
'Copyright 2002 AQUA TERRA Consultants - Royalty-free use permitted under open source license

Public Sub UpdateCopy(O As HspfOperation, TableStatus As HspfStatus)
  TableStatus.Change "TIMESERIES", 1, HspfStatusRequired
End Sub

Public Sub UpdatePltgen(O As HspfOperation, TableStatus As HspfStatus)
  Dim lTable As HspfTable
  Dim Ncrv, i&
  
  TableStatus.Change "PLOTINFO", 1, HspfStatusRequired
  If O.TableExists("PLOTINFO") Then
    Set lTable = O.Tables("PLOTINFO")
    Ncrv = lTable.Parms("NPT") + lTable.Parms("NMN")
  Else
    Ncrv = 1
  End If
  TableStatus.Change "GEN-LABELS", 1, HspfStatusRequired
  TableStatus.Change "SCALING", 1, HspfStatusRequired
  For i = 1 To Ncrv
    TableStatus.Change "CURV-DATA", i, HspfStatusRequired
  Next i
End Sub

Public Sub UpdateDisply(O As HspfOperation, TableStatus As HspfStatus)
  TableStatus.Change "DISPLY-INFO1", 1, HspfStatusRequired
  TableStatus.Change "DISPLY-INFO2", 1, HspfStatusOptional
End Sub

Public Sub UpdateDuranl(O As HspfOperation, TableStatus As HspfStatus)
  TableStatus.Change "GEN-DURDATA", 1, HspfStatusRequired
  TableStatus.Change "SEASON", 1, HspfStatusOptional
  TableStatus.Change "DURATIONS", 1, HspfStatusOptional
  TableStatus.Change "LEVELS", 1, HspfStatusOptional
  TableStatus.Change "LCONC", 1, HspfStatusOptional
End Sub

Public Sub UpdateGener(O As HspfOperation, TableStatus As HspfStatus)
  Dim lTable As HspfTable
  Dim Opcode
  
  TableStatus.Change "OPCODE", 1, HspfStatusRequired
  If O.TableExists("OPCODE") Then
    Set lTable = O.Tables("OPCODE")
    Opcode = lTable.Parms("OPCODE")
  Else
    Opcode = 0
  End If
  If Opcode = 8 Then
    TableStatus.Change "NTERMS", 1, HspfStatusOptional
    TableStatus.Change "COEFFS", 1, HspfStatusOptional
  End If
  If Opcode = 9 Or Opcode = 10 Or Opcode = 11 Or _
     Opcode = 24 Or Opcode = 25 Or Opcode = 26 Then
    TableStatus.Change "PARM", 1, HspfStatusOptional
  End If
End Sub

Public Sub UpdateMutsin(O As HspfOperation, TableStatus As HspfStatus)
  TableStatus.Change "MUTSINFO", 1, HspfStatusRequired
End Sub

Public Sub UpdateBmprac(O As HspfOperation, TableStatus As HspfStatus)
  TableStatus.Change "PRINT-INFO", 1, HspfStatusOptional
  TableStatus.Change "GEN-INFO", 1, HspfStatusRequired
  TableStatus.Change "FLOW-FLAG", 1, HspfStatusOptional
  TableStatus.Change "FLOW-FRAC", 1, HspfStatusOptional
  TableStatus.Change "CONS-FLAG", 1, HspfStatusOptional
  TableStatus.Change "CONS-FRAC", 1, HspfStatusOptional
  TableStatus.Change "HEAT-FLAG", 1, HspfStatusOptional
  TableStatus.Change "HEAT-FRAC", 1, HspfStatusOptional
  TableStatus.Change "SED-FLAG", 1, HspfStatusOptional
  TableStatus.Change "SED-FRAC", 1, HspfStatusOptional
  TableStatus.Change "GQ-FLAG", 1, HspfStatusOptional
  TableStatus.Change "GQ-FRAC", 1, HspfStatusOptional
  TableStatus.Change "OXY-FLAG", 1, HspfStatusOptional
  TableStatus.Change "OXY-FRAC", 1, HspfStatusOptional
  TableStatus.Change "NUT-FLAG", 1, HspfStatusOptional
  TableStatus.Change "DNUT-FRAC", 1, HspfStatusOptional
  TableStatus.Change "ADSNUT-FRAC", 1, HspfStatusOptional
  TableStatus.Change "PLANK-FLAG", 1, HspfStatusOptional
  TableStatus.Change "PLANK-FRAC", 1, HspfStatusOptional
  TableStatus.Change "PH-FLAG", 1, HspfStatusOptional
  TableStatus.Change "PH-FRAC", 1, HspfStatusOptional
End Sub

Public Sub UpdateReport(O As HspfOperation, TableStatus As HspfStatus)
  TableStatus.Change "REPORT-FLAGS", 1, HspfStatusRequired
  TableStatus.Change "REPORT-TITLE", 1, HspfStatusRequired
  TableStatus.Change "REPORT-SRC", 1, HspfStatusRequired
  TableStatus.Change "REPORT-CON", 1, HspfStatusRequired
  TableStatus.Change "REPORT-SUMM", 1, HspfStatusRequired
End Sub

