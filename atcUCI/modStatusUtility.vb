Option Strict Off
Option Explicit On

Imports atcuci.HspfStatus.HspfStatusReqOptUnnEnum

Module modStatusUtility
    'Copyright 2006 AQUA TERRA Consultants - Royalty-free use permitted under open source license

    Public Sub UpdateCopy(ByRef O As HspfOperation, ByRef TableStatus As HspfStatus)
        TableStatus.Change("TIMESERIES", 1, HspfStatusRequired)
    End Sub

    Public Sub UpdatePltgen(ByRef O As HspfOperation, ByRef TableStatus As HspfStatus)
        Dim lTable As HspfTable
        Dim Ncrv As Integer
        Dim i As Integer

        TableStatus.Change("PLOTINFO", 1, HspfStatusRequired)
        If O.TableExists("PLOTINFO") Then
            lTable = O.Tables.Item("PLOTINFO")
            Ncrv = lTable.Parms("NPT").Value + _
                   lTable.Parms("NMN").Value
        Else
            Ncrv = 1
        End If
        TableStatus.Change("GEN-LABELS", 1, HspfStatusRequired)
        TableStatus.Change("SCALING", 1, HspfStatusRequired)
        For i = 1 To Ncrv
            TableStatus.Change("CURV-DATA", i, HspfStatusRequired)
        Next i
    End Sub

    Public Sub UpdateDisply(ByRef O As HspfOperation, ByRef TableStatus As HspfStatus)
        TableStatus.Change("DISPLY-INFO1", 1, HspfStatusRequired)
        TableStatus.Change("DISPLY-INFO2", 1, HspfStatusOptional)
    End Sub

    Public Sub UpdateDuranl(ByRef O As HspfOperation, ByRef TableStatus As HspfStatus)
        TableStatus.Change("GEN-DURDATA", 1, HspfStatusRequired)
        TableStatus.Change("SEASON", 1, HspfStatusOptional)
        TableStatus.Change("DURATIONS", 1, HspfStatusOptional)
        TableStatus.Change("LEVELS", 1, HspfStatusOptional)
        TableStatus.Change("LCONC", 1, HspfStatusOptional)
    End Sub

    Public Sub UpdateGener(ByRef O As HspfOperation, ByRef TableStatus As HspfStatus)
        Dim lTable As HspfTable
        Dim Opcode As Integer

        TableStatus.Change("OPCODE", 1, HspfStatusRequired)
        If O.TableExists("OPCODE") Then
            lTable = O.Tables.Item("OPCODE")
            Opcode = lTable.Parms("OPCODE").Value
        Else
            Opcode = 0
        End If
        Select Case Opcode
            Case 8
                TableStatus.Change("NTERMS", 1, HspfStatusOptional)
                TableStatus.Change("COEFFS", 1, HspfStatusOptional)
            Case 9, 10, 11, 24, 25, 26
                TableStatus.Change("PARM", 1, HspfStatusOptional)
        End Select
    End Sub

    Public Sub UpdateMutsin(ByRef O As HspfOperation, ByRef TableStatus As HspfStatus)
        TableStatus.Change("MUTSINFO", 1, HspfStatusRequired)
    End Sub

    Public Sub UpdateBmprac(ByRef O As HspfOperation, ByRef TableStatus As HspfStatus)
        TableStatus.Change("PRINT-INFO", 1, HspfStatusOptional)
        TableStatus.Change("GEN-INFO", 1, HspfStatusRequired)
        TableStatus.Change("FLOW-FLAG", 1, HspfStatusOptional)
        TableStatus.Change("FLOW-FRAC", 1, HspfStatusOptional)
        TableStatus.Change("CONS-FLAG", 1, HspfStatusOptional)
        TableStatus.Change("CONS-FRAC", 1, HspfStatusOptional)
        TableStatus.Change("HEAT-FLAG", 1, HspfStatusOptional)
        TableStatus.Change("HEAT-FRAC", 1, HspfStatusOptional)
        TableStatus.Change("SED-FLAG", 1, HspfStatusOptional)
        TableStatus.Change("SED-FRAC", 1, HspfStatusOptional)
        TableStatus.Change("GQ-FLAG", 1, HspfStatusOptional)
        TableStatus.Change("GQ-FRAC", 1, HspfStatusOptional)
        TableStatus.Change("OXY-FLAG", 1, HspfStatusOptional)
        TableStatus.Change("OXY-FRAC", 1, HspfStatusOptional)
        TableStatus.Change("NUT-FLAG", 1, HspfStatusOptional)
        TableStatus.Change("DNUT-FRAC", 1, HspfStatusOptional)
        TableStatus.Change("ADSNUT-FRAC", 1, HspfStatusOptional)
        TableStatus.Change("PLANK-FLAG", 1, HspfStatusOptional)
        TableStatus.Change("PLANK-FRAC", 1, HspfStatusOptional)
        TableStatus.Change("PH-FLAG", 1, HspfStatusOptional)
        TableStatus.Change("PH-FRAC", 1, HspfStatusOptional)
    End Sub

    Public Sub UpdateReport(ByRef O As HspfOperation, ByRef TableStatus As HspfStatus)
        TableStatus.Change("REPORT-FLAGS", 1, HspfStatusRequired)
        TableStatus.Change("REPORT-TITLE", 1, HspfStatusRequired)
        TableStatus.Change("REPORT-SRC", 1, HspfStatusRequired)
        TableStatus.Change("REPORT-CON", 1, HspfStatusRequired)
        TableStatus.Change("REPORT-SUMM", 1, HspfStatusRequired)
    End Sub
End Module