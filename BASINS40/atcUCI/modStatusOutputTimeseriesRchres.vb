Option Strict Off
Option Explicit On

Imports atcuci.HspfStatus.HspfStatusReqOptUnnEnum

Module modStatusOutputTimeseriesRchres
    'Copyright 2006 AQUA TERRA Consultants - Royalty-free use permitted under open source license

    Public Sub UpdateOutputTimeseriesRchres(ByRef O As HspfOperation, ByRef TimserStatus As HspfStatus)
        Dim ltable As HspfTable
        Dim AUX2FG, i, j, AUX3FG As Integer
        Dim nCons, nExits, nGqual As Integer
        Dim Odfvfg(5) As Integer
        Dim Odgtfg(5) As Integer

        Dim lAcidph As Boolean
        If O.TableExists("ACTIVITY") Then
            ltable = O.Tables.Item("ACTIVITY")
            If O.TableExists("GEN-INFO") Then
                nExits = O.Tables.Item("GEN-INFO").ParmValue("NEXITS")
            Else
                nExits = 1
            End If

            'section hydr
            If ltable.Parms("HYDRFG").Value = 1 Then
                If O.TableExists("HYDR-PARM1") Then
                    With O.Tables.Item("HYDR-PARM1")
                        AUX2FG = .ParmValue("AUX2FG")
                        AUX3FG = .ParmValue("AUX3FG")
                    End With
                Else
                    AUX2FG = 0
                    AUX2FG = 0
                End If
                TimserStatus.Change("HYDR:VOL", 1, HspfStatusOptional)
                If Not O.Uci.CategoryBlock Is Nothing Then 'have category block
                    For Each lCategory As HspfCategory In O.Uci.CategoryBlock.Categories
                        TimserStatus.Change("HYDR:CVOL", lCategory.Id, HspfStatusOptional)
                    Next lCategory
                End If

                TimserStatus.Change("HYDR:DEP", 1, HspfStatusOptional)
                TimserStatus.Change("HYDR:STAGE", 1, HspfStatusOptional)
                TimserStatus.Change("HYDR:AVDEP", 1, HspfStatusOptional)
                TimserStatus.Change("HYDR:TWID", 1, HspfStatusOptional)
                TimserStatus.Change("HYDR:HRAD", 1, HspfStatusOptional)
                TimserStatus.Change("HYDR:SAREA", 1, HspfStatusOptional)
                'auxflgs
                If AUX2FG = 1 Then
                    TimserStatus.Change("HYDR:AVVEL", 1, HspfStatusOptional)
                    TimserStatus.Change("HYDR:AVSECT", 1, HspfStatusOptional)
                End If
                If AUX3FG = 1 Then
                    TimserStatus.Change("HYDR:USTAR", 1, HspfStatusOptional)
                    TimserStatus.Change("HYDR:TAU", 1, HspfStatusOptional)
                End If
                TimserStatus.Change("HYDR:RO", 1, HspfStatusOptional)
                If Not O.Uci.CategoryBlock Is Nothing Then 'have category block
                    For Each lCategory As HspfCategory In O.Uci.CategoryBlock.Categories
                        TimserStatus.Change("HYDR:CRO", lCategory.Id, HspfStatusOptional)
                    Next lCategory
                End If

                TimserStatus.Change("ROFLOW:ROVOL", 1, HspfStatusOptional)
                If Not O.Uci.CategoryBlock Is Nothing Then 'have category block
                    For Each lCategory As HspfCategory In O.Uci.CategoryBlock.Categories
                        TimserStatus.Change("ROFLOW:CROVOL", lCategory.Id, HspfStatusOptional)
                    Next lCategory
                End If

                If nExits > 1 Then
                    For i = 1 To nExits
                        TimserStatus.Change("HYDR:O", i, HspfStatusOptional)
                        If Not O.Uci.CategoryBlock Is Nothing Then 'have category block
                            For Each lCategory As HspfCategory In O.Uci.CategoryBlock.Categories
                                TimserStatus.Change2("HYDR:CO", i, lCategory.Id, HspfStatusOptional)
                                TimserStatus.Change2("HYDR:CDFVOL", i, lCategory.Id, HspfStatusOptional)
                                TimserStatus.Change2("HYDR:COVOL", i, lCategory.Id, HspfStatusOptional)
                                TimserStatus.Change2("OFLOW:COVOL", i, lCategory.Id, HspfStatusOptional)
                            Next lCategory
                        End If
                        TimserStatus.Change("OFLOW:OVOL", i, HspfStatusOptional)
                        TimserStatus.Change("HYDR:OVOL", i, HspfStatusOptional)
                    Next i
                End If
                TimserStatus.Change("HYDR:IVOL", 1, HspfStatusOptional)
                If Not O.Uci.CategoryBlock Is Nothing Then 'have category block
                    For Each lCategory As HspfCategory In O.Uci.CategoryBlock.Categories
                        TimserStatus.Change("HYDR:CIVOL", lCategory.Id, HspfStatusOptional)
                    Next lCategory
                End If

                TimserStatus.Change("HYDR:PRSUPY", 1, HspfStatusOptional)
                TimserStatus.Change("HYDR:VOLEV", 1, HspfStatusOptional)
                TimserStatus.Change("HYDR:ROVOL", 1, HspfStatusOptional)
                If Not O.Uci.CategoryBlock Is Nothing Then 'have category block
                    For Each lCategory As HspfCategory In O.Uci.CategoryBlock.Categories
                        TimserStatus.Change("HYDR:CROVOL", lCategory.Id, HspfStatusOptional)
                    Next lCategory
                End If

                TimserStatus.Change("HYDR:RIRDEM", 1, HspfStatusOptional)
                TimserStatus.Change("HYDR:RIRSHT", 1, HspfStatusOptional)
            End If

            'section cons
            If ltable.Parms("CONSFG").Value = 1 Then
                If O.TableExists("NCONS") Then
                    nCons = O.Tables.Item("NCONS").ParmValue("NCONS")
                Else
                    nCons = 0
                End If
                For i = 1 To nCons
                    TimserStatus.Change("CONS:CON", i, HspfStatusOptional)
                    TimserStatus.Change("CONS:ICON", i, HspfStatusOptional)
                    TimserStatus.Change("CONS:COADDR", i, HspfStatusOptional)
                    TimserStatus.Change("CONS:COADWT", i, HspfStatusOptional)
                    TimserStatus.Change("CONS:COADEP", i, HspfStatusOptional)
                    TimserStatus.Change("CONS:ROCON", i, HspfStatusOptional)
                    TimserStatus.Change("ROFLOW:ROCON", i, HspfStatusOptional)
                    If nExits > 1 Then
                        For j = 1 To nExits
                            TimserStatus.Change2("CONS:OCON", j, i, HspfStatusOptional)
                            TimserStatus.Change2("OFLOW:OCON", j, i, HspfStatusOptional)
                        Next j
                    End If
                Next i
            End If

            'section htrch			
            If ltable.Parms("HTFG").Value = 1 Then
                TimserStatus.Change("HTRCH:TW", 1, HspfStatusOptional)
                TimserStatus.Change("HTRCH:AIRTMP", 1, HspfStatusOptional)
                TimserStatus.Change("HTRCH:IHEAT", 1, HspfStatusOptional)
                TimserStatus.Change("HTRCH:HTEXCH", 1, HspfStatusOptional)
                TimserStatus.Change("HTRCH:ROHEAT", 1, HspfStatusOptional)
                TimserStatus.Change("HTRCH:SHDFAC", 1, HspfStatusOptional)
                TimserStatus.Change("ROFLOW:ROHEAT", 1, HspfStatusOptional)
                If nExits > 1 Then
                    For i = 1 To nExits
                        TimserStatus.Change("HTRCH:OHEAT", i, HspfStatusOptional)
                        TimserStatus.Change("OFLOW:OHEAT", i, HspfStatusOptional)
                    Next i
                End If
                For i = 1 To 7
                    TimserStatus.Change("HTRCH:HTCF4", i, HspfStatusOptional)
                Next i
            End If

            'section sedtran
            If ltable.Parms("SEDFG").Value = 1 Then
                For i = 1 To 4
                    TimserStatus.Change("SEDTRN:SSED", i, HspfStatusOptional)
                    TimserStatus.Change("SEDTRN:ISED", i, HspfStatusOptional)
                    TimserStatus.Change("SEDTRN:DEPSCR", i, HspfStatusOptional)
                    TimserStatus.Change("SEDTRN:ROSED", i, HspfStatusOptional)
                Next i
                For i = 1 To 3
                    TimserStatus.Change("ROFLOW:ROSED", i, HspfStatusOptional)
                    TimserStatus.Change("SEDTRN:TSED", i, HspfStatusOptional)
                Next i
                For i = 1 To 10
                    TimserStatus.Change("SEDTRN:RSED", i, HspfStatusOptional)
                Next i
                TimserStatus.Change("SEDTRN:BEDDEP", 1, HspfStatusOptional)
                If nExits > 1 Then
                    For j = 1 To 4
                        For i = 1 To nExits
                            TimserStatus.Change2("SEDTRN:OSED", i, j, HspfStatusOptional)
                        Next i
                    Next j
                    For j = 1 To 3
                        For i = 1 To nExits
                            TimserStatus.Change2("OFLOW:OSED", i, j, HspfStatusOptional)
                        Next i
                    Next j
                End If
            End If

            'section gqual
            If ltable.Parms("GQALFG").Value = 1 Then
                If O.TableExists("GQ-GENDATA") Then
                    nGqual = O.Tables.Item("GQ-GENDATA").ParmValue("NGQUAL")
                Else
                    nGqual = 1
                End If
                For i = 1 To nGqual
                    TimserStatus.Change("GQUAL:DQAL", i, HspfStatusOptional)
                    TimserStatus.Change("GQUAL:RDQAL", i, HspfStatusOptional)
                    TimserStatus.Change("GQUAL:RRQAL", i, HspfStatusOptional)
                    TimserStatus.Change("GQUAL:IDQAL", i, HspfStatusOptional)
                    TimserStatus.Change("GQUAL:TIQAL", i, HspfStatusOptional)
                    TimserStatus.Change("GQUAL:PDQAL", i, HspfStatusOptional)
                    TimserStatus.Change("GQUAL:GQADDR", i, HspfStatusOptional)
                    TimserStatus.Change("GQUAL:GQADWT", i, HspfStatusOptional)
                    TimserStatus.Change("GQUAL:GQADEP", i, HspfStatusOptional)
                    TimserStatus.Change("GQUAL:RODQAL", i, HspfStatusOptional)
                    TimserStatus.Change("GQUAL:TROQAL", i, HspfStatusOptional)
                    TimserStatus.Change("ROFLOW:RODQAL", i, HspfStatusOptional)
                    For j = 1 To 7
                        TimserStatus.Change2("GQUAL:DDQAL", j, i, HspfStatusOptional)
                        TimserStatus.Change2("GQUAL:SQDEC", j, i, HspfStatusOptional)
                        TimserStatus.Change2("GQUAL:ADQAL", j, i, HspfStatusOptional)
                    Next j
                    For j = 1 To 12
                        TimserStatus.Change2("GQUAL:RSQAL", j, i, HspfStatusOptional)
                    Next j
                    For j = 1 To 6
                        TimserStatus.Change2("GQUAL:SQAL", j, i, HspfStatusOptional)
                    Next j
                    For j = 1 To 4
                        TimserStatus.Change2("GQUAL:DSQAL", j, i, HspfStatusOptional)
                        TimserStatus.Change2("GQUAL:ISQAL", j, i, HspfStatusOptional)
                        TimserStatus.Change2("GQUAL:ROSQAL", j, i, HspfStatusOptional)
                    Next j
                    For j = 1 To 3
                        TimserStatus.Change2("ROFLOW:ROSQAL", j, i, HspfStatusOptional)
                    Next j
                    If nExits > 1 Then
                        For j = 1 To nExits
                            TimserStatus.Change2("GQUAL:ODQAL", j, i, HspfStatusOptional)
                            TimserStatus.Change2("GQUAL:TOSQAL", j, i, HspfStatusOptional)
                            TimserStatus.Change2("OFLOW:ODQAL", j, i, HspfStatusOptional)
                            TimserStatus.Change2("GQUAL:OSQAL", j, i, HspfStatusOptional)
                            TimserStatus.Change2("OFLOW:OSQAL", j, i, HspfStatusOptional)
                            TimserStatus.Change2("GQUAL:OSQAL", j, i + 1, HspfStatusOptional)
                            TimserStatus.Change2("OFLOW:OSQAL", j, i + 1, HspfStatusOptional)
                            TimserStatus.Change2("GQUAL:OSQAL", j, i + 2, HspfStatusOptional)
                            TimserStatus.Change2("OFLOW:OSQAL", j, i + 2, HspfStatusOptional)
                        Next j
                    End If
                Next i
            End If

            'section oxrx
            If ltable.Parms("OXFG").Value = 1 Then
                TimserStatus.Change("OXRX:DOX", 1, HspfStatusOptional)
                TimserStatus.Change("OXRX:BOD", 1, HspfStatusOptional)
                TimserStatus.Change("OXRX:SATDO", 1, HspfStatusOptional)
                TimserStatus.Change("OXRX:OXIF", 1, HspfStatusOptional)
                TimserStatus.Change("OXRX:OXIF", 2, HspfStatusOptional)
                TimserStatus.Change("OXRX:OXCF1", 1, HspfStatusOptional)
                TimserStatus.Change("OXRX:OXCF1", 2, HspfStatusOptional)
                TimserStatus.Change("ROFLOW:OXCF1", 1, HspfStatusOptional)
                TimserStatus.Change("ROFLOW:OXCF1", 2, HspfStatusOptional)
                If nExits > 1 Then
                    For j = 1 To nExits
                        TimserStatus.Change2("OXRX:OXCF2", j, 1, HspfStatusOptional)
                        TimserStatus.Change2("OXRX:OXCF2", j, 2, HspfStatusOptional)
                        TimserStatus.Change2("OFLOW:OXCF2", j, 1, HspfStatusOptional)
                        TimserStatus.Change2("OFLOW:OXCF2", j, 2, HspfStatusOptional)
                    Next j
                End If
                For j = 1 To 8
                    TimserStatus.Change("OXRX:OXCF3", j, HspfStatusOptional)
                    TimserStatus.Change("OXRX:OXCF4", j, HspfStatusOptional)
                Next j
            End If

            'section nutrx
            If ltable.Parms("NUTFG").Value = 1 Then
                TimserStatus.Change("NUTRX:NUCF6", 1, HspfStatusOptional)
                For i = 1 To 3
                    TimserStatus.Change("NUTRX:SNH4", i, HspfStatusOptional)
                    TimserStatus.Change("NUTRX:SPO4", i, HspfStatusOptional)
                    TimserStatus.Change("NUTRX:NUADDR", i, HspfStatusOptional)
                    TimserStatus.Change("NUTRX:NUADWT", i, HspfStatusOptional)
                    TimserStatus.Change("NUTRX:NUADEP", i, HspfStatusOptional)
                Next i
                For i = 1 To 12
                    TimserStatus.Change("NUTRX:RSNH4", i, HspfStatusOptional)
                    TimserStatus.Change("NUTRX:RSPO4", i, HspfStatusOptional)
                Next i
                For i = 1 To 4
                    TimserStatus.Change("NUTRX:NUST", i, HspfStatusOptional)
                    TimserStatus.Change("NUTRX:NUCF1", i, HspfStatusOptional)
                    TimserStatus.Change("NUTRX:NUIF1", i, HspfStatusOptional)
                    TimserStatus.Change("NUTRX:TNUIF", i, HspfStatusOptional)
                    TimserStatus.Change("NUTRX:TNUCF1", i, HspfStatusOptional)
                    TimserStatus.Change("ROFLOW:NUCF1", i, HspfStatusOptional)
                Next i
                For i = 1 To 6
                    TimserStatus.Change("NUTRX:DNUST", i, HspfStatusOptional)
                    TimserStatus.Change("NUTRX:DNUST2", i, HspfStatusOptional)
                Next i
                For i = 1 To 7
                    TimserStatus.Change("NUTRX:NUCF4", i, HspfStatusOptional)
                Next i
                For i = 1 To 8
                    TimserStatus.Change("NUTRX:NUCF5", i, HspfStatusOptional)
                Next i
                For i = 1 To 6
                    TimserStatus.Change("NUTRX:NUCF7", i, HspfStatusOptional)
                Next i
                For i = 1 To 3
                    For j = 1 To 2
                        TimserStatus.Change2("ROFLOW:NUCF2", i, j, HspfStatusOptional)
                    Next j
                Next i
                For i = 1 To 4
                    For j = 1 To 2
                        TimserStatus.Change2("NUTRX:NUIF2", i, j, HspfStatusOptional)
                        TimserStatus.Change2("NUTRX:NUCF2", i, j, HspfStatusOptional)
                        TimserStatus.Change2("NUTRX:NUCF3", i, j, HspfStatusOptional)
                        TimserStatus.Change2("NUTRX:NUCF8", i, j, HspfStatusOptional)
                    Next j
                Next i
                If nExits > 1 Then
                    For i = 1 To nExits
                        For j = 1 To 4
                            TimserStatus.Change2("NUTRX:NUCF9", i, j, HspfStatusOptional)
                            TimserStatus.Change2("OFLOW:NUCF9", i, j, HspfStatusOptional)
                            TimserStatus.Change2("NUTRX:OSNH4", i, j, HspfStatusOptional)
                            TimserStatus.Change2("NUTRX:OSPO4", i, j, HspfStatusOptional)
                            TimserStatus.Change2("NUTRX:TNUCF2", i, j, HspfStatusOptional)
                        Next j
                        For j = 1 To 3
                            TimserStatus.Change2("OFLOW:OSNH4", i, j, HspfStatusOptional)
                            TimserStatus.Change2("OFLOW:OSPO4", i, j, HspfStatusOptional)
                        Next j
                    Next i
                End If
            End If

            'section plank
            If ltable.Parms("PLKFG").Value = 1 Then
                TimserStatus.Change("PLANK:PHYTO", 1, HspfStatusOptional)
                TimserStatus.Change("PLANK:ZOO", 1, HspfStatusOptional)
                For i = 1 To 4
                    TimserStatus.Change("PLANK:BENAL", i, HspfStatusOptional)
                Next i
                TimserStatus.Change("PLANK:TBENAL", 1, HspfStatusOptional)
                TimserStatus.Change("PLANK:TBENAL", 2, HspfStatusOptional)
                TimserStatus.Change("PLANK:PHYCLA", 1, HspfStatusOptional)
                For i = 1 To 4
                    TimserStatus.Change("PLANK:BALCLA", i, HspfStatusOptional)
                Next i
                For i = 1 To 7
                    TimserStatus.Change("PLANK:PKST3", i, HspfStatusOptional)
                Next i
                TimserStatus.Change("PLANK:PKST4", 1, HspfStatusOptional)
                TimserStatus.Change("PLANK:PKST4", 2, HspfStatusOptional)
                For i = 1 To 5
                    TimserStatus.Change("PLANK:PKIF", i, HspfStatusOptional)
                    TimserStatus.Change("PLANK:TPKIF", i, HspfStatusOptional)
                    TimserStatus.Change("PLANK:PKCF1", i, HspfStatusOptional)
                    TimserStatus.Change("ROFLOW:PKCF1", i, HspfStatusOptional)
                    TimserStatus.Change("PLANK:TPKCF1", i, HspfStatusOptional)
                    TimserStatus.Change("PLANK:PKCF5", i, HspfStatusOptional)
                    TimserStatus.Change("PLANK:PKCF8", i, HspfStatusOptional)
                    TimserStatus.Change("PLANK:PKCF9", i, HspfStatusOptional)
                    TimserStatus.Change("PLANK:PKCF10", i, HspfStatusOptional)
                    If nExits > 1 Then
                        For j = 1 To nExits
                            TimserStatus.Change2("PLANK:PKCF2", j, i, HspfStatusOptional)
                            TimserStatus.Change2("OFLOW:PKCF2", j, i, HspfStatusOptional)
                            TimserStatus.Change2("PLANK:TPKCF2", j, i, HspfStatusOptional)
                        Next j
                    End If
                Next i
                For i = 1 To 3
                    TimserStatus.Change("PLANK:PLADDR", i, HspfStatusOptional)
                    TimserStatus.Change("PLANK:PLADWT", i, HspfStatusOptional)
                    TimserStatus.Change("PLANK:PLADEP", i, HspfStatusOptional)
                    TimserStatus.Change("PLANK:PKCF6", i, HspfStatusOptional)
                    TimserStatus.Change("PLANK:TPKCF7", i, HspfStatusOptional)
                Next i
                For i = 1 To 4
                    For j = 1 To 3
                        TimserStatus.Change2("PLANK:PKCF7", j, i, HspfStatusOptional)
                    Next j
                Next i
            End If

            'section phcarb
            If ltable.Parms("PHFG").Value = 1 Or _
               ltable.Parms("PHFG").Value = 3 Then
                TimserStatus.Change("PHCARB:SATCO2", 1, HspfStatusOptional)
                For i = 1 To 3
                    TimserStatus.Change("PHCARB:PHST", i, HspfStatusOptional)
                Next i
                For i = 1 To 2
                    TimserStatus.Change("PHCARB:PHIF", i, HspfStatusOptional)
                    TimserStatus.Change("PHCARB:PHCF1", i, HspfStatusOptional)
                    TimserStatus.Change("ROFLOW:PHCF1", i, HspfStatusOptional)
                    If nExits > 1 Then
                        For j = 1 To nExits
                            TimserStatus.Change2("PHCARB:PHCF2", j, i, HspfStatusOptional)
                            TimserStatus.Change2("OFLOW:PHCF2", j, i, HspfStatusOptional)
                        Next j
                    End If
                Next i
                For i = 1 To 7
                    TimserStatus.Change("PHCARB:PHCF3", i, HspfStatusOptional)
                Next i
            End If

            lAcidph = False
            'check to see if acidph is available			
            For i = 1 To O.Uci.Msg.BlockDefs("RCHRES").SectionDefs.Count
                If O.Uci.Msg.BlockDefs("RCHRES").SectionDefs(i).Name = "ACIDPH" Then
                    lAcidph = True
                End If
            Next i
            If lAcidph Then
                'section acidph
                If ltable.Parms("PHFG").Value = 2 Or _
                   ltable.Parms("PHFG").Value = 3 Then
                    TimserStatus.Change("ACIDPH:ACPH", 1, HspfStatusOptional)
                    For i = 1 To 7
                        TimserStatus.Change("ACIDPH:ACCONC", i, HspfStatusOptional)
                        TimserStatus.Change("ACIDPH:ACSTOR", i, HspfStatusOptional)
                        TimserStatus.Change("ACIDPH:ACFLX1", i, HspfStatusOptional)
                        TimserStatus.Change("ROFLOW:ACFLX1", i, HspfStatusOptional)
                        If nExits > 1 Then
                            For j = 1 To nExits
                                TimserStatus.Change2("ACIDPH:ACFLX2", j, i, HspfStatusOptional)
                                TimserStatus.Change2("OFLOW:ACFLX2", j, i, HspfStatusOptional)
                            Next j
                        End If
                    Next i
                End If
            End If
        End If
    End Sub
End Module