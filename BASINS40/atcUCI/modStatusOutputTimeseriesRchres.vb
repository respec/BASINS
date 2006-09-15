Option Strict Off
Option Explicit On
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
                nExits = O.Tables.Item("GEN-INFO").Parms("NEXITS")
			Else
				nExits = 1
			End If
			
			'section hydr
            If ltable.Parms.Item("HYDRFG") = 1 Then
                If O.TableExists("HYDR-PARM1") Then
                    AUX2FG = O.Tables.Item("HYDR-PARM1").Parms("AUX2FG")
                    AUX3FG = O.Tables.Item("HYDR-PARM1").Parms("AUX3FG")
                Else
                    AUX2FG = 0
                    AUX2FG = 0
                End If
                TimserStatus.Change("HYDR:VOL", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                If Not O.Uci.CategoryBlock Is Nothing Then
                    'have category block
                    For i = 1 To O.Uci.CategoryBlock.Count
                        TimserStatus.Change("HYDR:CVOL", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    Next i
                End If

                TimserStatus.Change("HYDR:DEP", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                TimserStatus.Change("HYDR:STAGE", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                TimserStatus.Change("HYDR:AVDEP", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                TimserStatus.Change("HYDR:TWID", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                TimserStatus.Change("HYDR:HRAD", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                TimserStatus.Change("HYDR:SAREA", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                'auxflgs
                If AUX2FG = 1 Then
                    TimserStatus.Change("HYDR:AVVEL", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change("HYDR:AVSECT", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                End If
                If AUX3FG = 1 Then
                    TimserStatus.Change("HYDR:USTAR", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change("HYDR:TAU", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                End If
                TimserStatus.Change("HYDR:RO", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                If Not O.Uci.CategoryBlock Is Nothing Then
                    'have category block
                    For i = 1 To O.Uci.CategoryBlock.Count
                        TimserStatus.Change("HYDR:CRO", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    Next i
                End If

                TimserStatus.Change("ROFLOW:ROVOL", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                If Not O.Uci.CategoryBlock Is Nothing Then
                    'have category block
                    For i = 1 To O.Uci.CategoryBlock.Count
                        TimserStatus.Change("ROFLOW:CROVOL", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    Next i
                End If

                If nExits > 1 Then
                    For i = 1 To nExits
                        TimserStatus.Change("HYDR:O", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                        If Not O.Uci.CategoryBlock Is Nothing Then
                            'have category block
                            For j = 1 To O.Uci.CategoryBlock.Count
                                TimserStatus.Change2("HYDR:CO", i, j, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                                TimserStatus.Change2("HYDR:CDFVOL", i, j, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                                TimserStatus.Change2("HYDR:COVOL", i, j, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                                TimserStatus.Change2("OFLOW:COVOL", i, j, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                            Next j
                        End If
                        TimserStatus.Change("OFLOW:OVOL", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                        TimserStatus.Change("HYDR:OVOL", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    Next i
                End If
                TimserStatus.Change("HYDR:IVOL", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                If Not O.Uci.CategoryBlock Is Nothing Then
                    'have category block
                    For i = 1 To O.Uci.CategoryBlock.Count
                        TimserStatus.Change("HYDR:CIVOL", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    Next i
                End If

                TimserStatus.Change("HYDR:PRSUPY", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                TimserStatus.Change("HYDR:VOLEV", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                TimserStatus.Change("HYDR:ROVOL", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                If Not O.Uci.CategoryBlock Is Nothing Then
                    'have category block
                    For i = 1 To O.Uci.CategoryBlock.Count
                        TimserStatus.Change("HYDR:CROVOL", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    Next i
                End If

                TimserStatus.Change("HYDR:RIRDEM", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                TimserStatus.Change("HYDR:RIRSHT", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
            End If
			
			'section cons
            If ltable.Parms.Item("CONSFG") = 1 Then
                If O.TableExists("NCONS") Then
                    nCons = O.Tables.Item("NCONS").Parms("NCONS")
                Else
                    nCons = 0
                End If
                For i = 1 To nCons
                    TimserStatus.Change("CONS:CON", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change("CONS:ICON", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change("CONS:COADDR", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change("CONS:COADWT", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change("CONS:COADEP", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change("CONS:ROCON", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change("ROFLOW:ROCON", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    If nExits > 1 Then
                        For j = 1 To nExits
                            TimserStatus.Change2("CONS:OCON", j, i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                            TimserStatus.Change2("OFLOW:OCON", j, i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                        Next j
                    End If
                Next i
            End If
			
			'section htrch			
			If ltable.Parms.Item("HTFG") = 1 Then
				TimserStatus.Change("HTRCH:TW", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
				TimserStatus.Change("HTRCH:AIRTMP", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
				TimserStatus.Change("HTRCH:IHEAT", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
				TimserStatus.Change("HTRCH:HTEXCH", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
				TimserStatus.Change("HTRCH:ROHEAT", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
				TimserStatus.Change("HTRCH:SHDFAC", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
				TimserStatus.Change("ROFLOW:ROHEAT", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
				If nExits > 1 Then
					For i = 1 To nExits
						TimserStatus.Change("HTRCH:OHEAT", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
						TimserStatus.Change("OFLOW:OHEAT", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
					Next i
				End If
				For i = 1 To 7
					TimserStatus.Change("HTRCH:HTCF4", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
				Next i
			End If
			
			'section sedtran
            If ltable.Parms.Item("SEDFG") = 1 Then
                For i = 1 To 4
                    TimserStatus.Change("SEDTRN:SSED", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change("SEDTRN:ISED", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change("SEDTRN:DEPSCR", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change("SEDTRN:ROSED", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                Next i
                For i = 1 To 3
                    TimserStatus.Change("ROFLOW:ROSED", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change("SEDTRN:TSED", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                Next i
                For i = 1 To 10
                    TimserStatus.Change("SEDTRN:RSED", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                Next i
                TimserStatus.Change("SEDTRN:BEDDEP", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                If nExits > 1 Then
                    For j = 1 To 4
                        For i = 1 To nExits
                            TimserStatus.Change2("SEDTRN:OSED", i, j, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                        Next i
                    Next j
                    For j = 1 To 3
                        For i = 1 To nExits
                            TimserStatus.Change2("OFLOW:OSED", i, j, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                        Next i
                    Next j
                End If
            End If
			
			'section gqual
            If ltable.Parms.Item("GQALFG") = 1 Then
                If O.TableExists("GQ-GENDATA") Then
                    nGqual = O.Tables.Item("GQ-GENDATA").Parms("NGQUAL")
                Else
                    nGqual = 1
                End If
                For i = 1 To nGqual
                    TimserStatus.Change("GQUAL:DQAL", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change("GQUAL:RDQAL", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change("GQUAL:RRQAL", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change("GQUAL:IDQAL", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change("GQUAL:TIQAL", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change("GQUAL:PDQAL", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change("GQUAL:GQADDR", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change("GQUAL:GQADWT", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change("GQUAL:GQADEP", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change("GQUAL:RODQAL", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change("GQUAL:TROQAL", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change("ROFLOW:RODQAL", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    For j = 1 To 7
                        TimserStatus.Change2("GQUAL:DDQAL", j, i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                        TimserStatus.Change2("GQUAL:SQDEC", j, i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                        TimserStatus.Change2("GQUAL:ADQAL", j, i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    Next j
                    For j = 1 To 12
                        TimserStatus.Change2("GQUAL:RSQAL", j, i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    Next j
                    For j = 1 To 6
                        TimserStatus.Change2("GQUAL:SQAL", j, i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    Next j
                    For j = 1 To 4
                        TimserStatus.Change2("GQUAL:DSQAL", j, i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                        TimserStatus.Change2("GQUAL:ISQAL", j, i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                        TimserStatus.Change2("GQUAL:ROSQAL", j, i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    Next j
                    For j = 1 To 3
                        TimserStatus.Change2("ROFLOW:ROSQAL", j, i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    Next j
                    If nExits > 1 Then
                        For j = 1 To nExits
                            TimserStatus.Change2("GQUAL:ODQAL", j, i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                            TimserStatus.Change2("GQUAL:TOSQAL", j, i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                            TimserStatus.Change2("OFLOW:ODQAL", j, i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                            TimserStatus.Change2("GQUAL:OSQAL", j, i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                            TimserStatus.Change2("OFLOW:OSQAL", j, i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                            TimserStatus.Change2("GQUAL:OSQAL", j, i + 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                            TimserStatus.Change2("OFLOW:OSQAL", j, i + 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                            TimserStatus.Change2("GQUAL:OSQAL", j, i + 2, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                            TimserStatus.Change2("OFLOW:OSQAL", j, i + 2, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                        Next j
                    End If
                Next i
            End If
			
			'section oxrx
            If ltable.Parms.Item("OXFG") = 1 Then
                TimserStatus.Change("OXRX:DOX", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                TimserStatus.Change("OXRX:BOD", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                TimserStatus.Change("OXRX:SATDO", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                TimserStatus.Change("OXRX:OXIF", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                TimserStatus.Change("OXRX:OXIF", 2, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                TimserStatus.Change("OXRX:OXCF1", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                TimserStatus.Change("OXRX:OXCF1", 2, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                TimserStatus.Change("ROFLOW:OXCF1", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                TimserStatus.Change("ROFLOW:OXCF1", 2, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                If nExits > 1 Then
                    For j = 1 To nExits
                        TimserStatus.Change2("OXRX:OXCF2", j, 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                        TimserStatus.Change2("OXRX:OXCF2", j, 2, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                        TimserStatus.Change2("OFLOW:OXCF2", j, 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                        TimserStatus.Change2("OFLOW:OXCF2", j, 2, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    Next j
                End If
                For j = 1 To 8
                    TimserStatus.Change("OXRX:OXCF3", j, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change("OXRX:OXCF4", j, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                Next j
            End If
			
			'section nutrx
            If ltable.Parms.Item("NUTFG") = 1 Then
                TimserStatus.Change("NUTRX:NUCF6", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                For i = 1 To 3
                    TimserStatus.Change("NUTRX:SNH4", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change("NUTRX:SPO4", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change("NUTRX:NUADDR", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change("NUTRX:NUADWT", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change("NUTRX:NUADEP", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                Next i
                For i = 1 To 12
                    TimserStatus.Change("NUTRX:RSNH4", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change("NUTRX:RSPO4", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                Next i
                For i = 1 To 4
                    TimserStatus.Change("NUTRX:NUST", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change("NUTRX:NUCF1", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change("NUTRX:NUIF1", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change("NUTRX:TNUIF", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change("NUTRX:TNUCF1", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change("ROFLOW:NUCF1", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                Next i
                For i = 1 To 6
                    TimserStatus.Change("NUTRX:DNUST", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change("NUTRX:DNUST2", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                Next i
                For i = 1 To 7
                    TimserStatus.Change("NUTRX:NUCF4", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                Next i
                For i = 1 To 8
                    TimserStatus.Change("NUTRX:NUCF5", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                Next i
                For i = 1 To 6
                    TimserStatus.Change("NUTRX:NUCF7", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                Next i
                For i = 1 To 3
                    For j = 1 To 2
                        TimserStatus.Change2("ROFLOW:NUCF2", i, j, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    Next j
                Next i
                For i = 1 To 4
                    For j = 1 To 2
                        TimserStatus.Change2("NUTRX:NUIF2", i, j, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                        TimserStatus.Change2("NUTRX:NUCF2", i, j, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                        TimserStatus.Change2("NUTRX:NUCF3", i, j, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                        TimserStatus.Change2("NUTRX:NUCF8", i, j, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    Next j
                Next i
                If nExits > 1 Then
                    For i = 1 To nExits
                        For j = 1 To 4
                            TimserStatus.Change2("NUTRX:NUCF9", i, j, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                            TimserStatus.Change2("OFLOW:NUCF9", i, j, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                            TimserStatus.Change2("NUTRX:OSNH4", i, j, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                            TimserStatus.Change2("NUTRX:OSPO4", i, j, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                            TimserStatus.Change2("NUTRX:TNUCF2", i, j, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                        Next j
                        For j = 1 To 3
                            TimserStatus.Change2("OFLOW:OSNH4", i, j, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                            TimserStatus.Change2("OFLOW:OSPO4", i, j, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                        Next j
                    Next i
                End If
            End If
			
			'section plank
            If ltable.Parms.Item("PLKFG") = 1 Then
                TimserStatus.Change("PLANK:PHYTO", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                TimserStatus.Change("PLANK:ZOO", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                For i = 1 To 4
                    TimserStatus.Change("PLANK:BENAL", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                Next i
                TimserStatus.Change("PLANK:TBENAL", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                TimserStatus.Change("PLANK:TBENAL", 2, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                TimserStatus.Change("PLANK:PHYCLA", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                For i = 1 To 4
                    TimserStatus.Change("PLANK:BALCLA", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                Next i
                For i = 1 To 7
                    TimserStatus.Change("PLANK:PKST3", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                Next i
                TimserStatus.Change("PLANK:PKST4", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                TimserStatus.Change("PLANK:PKST4", 2, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                For i = 1 To 5
                    TimserStatus.Change("PLANK:PKIF", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change("PLANK:TPKIF", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change("PLANK:PKCF1", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change("ROFLOW:PKCF1", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change("PLANK:TPKCF1", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change("PLANK:PKCF5", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change("PLANK:PKCF8", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change("PLANK:PKCF9", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change("PLANK:PKCF10", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    If nExits > 1 Then
                        For j = 1 To nExits
                            TimserStatus.Change2("PLANK:PKCF2", j, i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                            TimserStatus.Change2("OFLOW:PKCF2", j, i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                            TimserStatus.Change2("PLANK:TPKCF2", j, i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                        Next j
                    End If
                Next i
                For i = 1 To 3
                    TimserStatus.Change("PLANK:PLADDR", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change("PLANK:PLADWT", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change("PLANK:PLADEP", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change("PLANK:PKCF6", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change("PLANK:TPKCF7", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                Next i
                For i = 1 To 4
                    For j = 1 To 3
                        TimserStatus.Change2("PLANK:PKCF7", j, i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    Next j
                Next i
            End If
			
			'section phcarb
            If ltable.Parms.Item("PHFG") = 1 Or ltable.Parms.Item("PHFG") = 3 Then
                TimserStatus.Change("PHCARB:SATCO2", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                For i = 1 To 3
                    TimserStatus.Change("PHCARB:PHST", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                Next i
                For i = 1 To 2
                    TimserStatus.Change("PHCARB:PHIF", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change("PHCARB:PHCF1", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change("ROFLOW:PHCF1", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    If nExits > 1 Then
                        For j = 1 To nExits
                            TimserStatus.Change2("PHCARB:PHCF2", j, i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                            TimserStatus.Change2("OFLOW:PHCF2", j, i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                        Next j
                    End If
                Next i
                For i = 1 To 7
                    TimserStatus.Change("PHCARB:PHCF3", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
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
                If ltable.Parms.Item("PHFG") = 2 Or ltable.Parms.Item("PHFG") = 3 Then
                    TimserStatus.Change("ACIDPH:ACPH", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    For i = 1 To 7
                        TimserStatus.Change("ACIDPH:ACCONC", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                        TimserStatus.Change("ACIDPH:ACSTOR", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                        TimserStatus.Change("ACIDPH:ACFLX1", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                        TimserStatus.Change("ROFLOW:ACFLX1", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                        If nExits > 1 Then
                            For j = 1 To nExits
                                TimserStatus.Change2("ACIDPH:ACFLX2", j, i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                                TimserStatus.Change2("OFLOW:ACFLX2", j, i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                            Next j
                        End If
                    Next i
                End If
			End If
			
		End If
	End Sub
End Module