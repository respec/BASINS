Option Strict Off
Option Explicit On
Module modStatusInputTimeseriesRchres
    'Copyright 2006 AQUA TERRA Consultants - Royalty-free use permitted under open source license
	
	Public Sub UpdateInputTimeseriesRchres(ByRef O As HspfOperation, ByRef TimserStatus As HspfStatus)
		Dim ltable As HspfTable
		Dim i As Integer
		Dim nExits, nGqual As Integer
		Dim Odfvfg(5) As Integer
		Dim Odgtfg(5) As Integer
		Dim wetadfg, nCons, sandfg, dryadfg As Integer
		Dim coadfg(20) As Integer
		Dim ctemp As String
		
		Dim lAcidph As Boolean
		If O.TableExists("ACTIVITY") Then
			ltable = O.Tables.Item("ACTIVITY")
			If O.TableExists("GEN-INFO") Then
                nExits = O.Tables.Item("GEN-INFO").ParmValue("NEXITS")
			Else
				nExits = 1
			End If
			
			'section hydr
            If ltable.Parms.Item("HYDRFG") = 1 Then
                If O.TableExists("HYDR-PARM1") Then
                    With O.Tables.Item("HYDR-PARM1")
                        Odfvfg(1) = .ParmValue("ODFVF1")
                        Odfvfg(2) = .ParmValue("ODFVF2")
                        Odfvfg(3) = .ParmValue("ODFVF3")
                        Odfvfg(4) = .ParmValue("ODFVF4")
                        Odfvfg(5) = .ParmValue("ODFVF5")
                        Odgtfg(1) = .ParmValue("ODGTF1")
                        Odgtfg(2) = .ParmValue("ODGTF2")
                        Odgtfg(3) = .ParmValue("ODGTF3")
                        Odgtfg(4) = .ParmValue("ODGTF4")
                        Odgtfg(5) = .ParmValue("ODGTF5")
                    End With
                Else
                    Odfvfg(1) = 0 : Odfvfg(2) = 0 : Odfvfg(3) = 0 : Odfvfg(4) = 0 : Odfvfg(5) = 0
                    Odgtfg(1) = 0 : Odgtfg(2) = 0 : Odgtfg(3) = 0 : Odgtfg(4) = 0 : Odgtfg(5) = 0
                End If
                If O.Uci.CategoryBlock Is Nothing Then
                    'not using category block
                    TimserStatus.Change("INFLOW:IVOL", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                Else
                    'have category block
                    For i = 1 To O.Uci.CategoryBlock.Count
                        TimserStatus.Change("INFLOW:CIVOL", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    Next i
                End If
                If O.Uci.CategoryBlock Is Nothing Then
                    'not using category block
                    TimserStatus.Change("EXTNL:IVOL", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                Else
                    'have category block
                    For i = 1 To O.Uci.CategoryBlock.Count
                        TimserStatus.Change("EXTNL:CIVOL", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    Next i
                End If
                TimserStatus.Change("EXTNL:PREC", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                TimserStatus.Change("EXTNL:POTEV", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                For i = 1 To nExits
                    If Odfvfg(i) < 0 Then
                        TimserStatus.Change("EXTNL:COLIND", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusRequired)
                    End If
                    If O.Uci.CategoryBlock Is Nothing Then
                        'not using category block
                        If Odgtfg(i) > 0 Then
                            TimserStatus.Change("EXTNL:OUTDGT", Odgtfg(i), HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusRequired)
                        End If
                    Else
                        'have category block
                        If Odgtfg(i) > 0 Then
                            'make optional since we cant don't accomodate string versions of sub1,sub2
                            TimserStatus.Change("EXTNL:COTDGT", Odgtfg(i), HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                        End If
                    End If
                Next i
            End If
			
			'section adcalc			
			If ltable.Parms.Item("ADFG") = 1 Then
                If ltable.Parms.Item("HYDRFG") = 0 Then 'need what HYDR would have sent
                    TimserStatus.Change("HYDR:VOL", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusRequired)
                    For i = 1 To nExits
                        TimserStatus.Change("HYDR:O", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusRequired)
                    Next i
                End If
			End If
			
			'section cons
            If ltable.Parms.Item("CONSFG") = 1 Then
                If O.TableExists("NCONS") Then
                    nCons = O.Tables.Item("NCONS").ParmValue("NCONS")
                Else
                    nCons = 0
                End If
                If O.TableExists("CONS-AD-FLAGS") Then
                    With O.Tables.Item("CONS-AD-FLAGS")
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
                        TimserStatus.Change("EXTNL:COADFX", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusRequired)
                    End If
                    If coadfg(((i - 1) * 2) + 2) = -1 Then
                        'wet conc timser required
                        TimserStatus.Change("EXTNL:COADCN", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusRequired)
                    End If
                    If coadfg(((i - 1) * 2) + 1) <> 0 Then
                        'dry ad simulated
                        dryadfg = 1
                    End If
                    If coadfg(((i - 1) * 2) + 2) <> 0 Then
                        'wet ad simulated
                        wetadfg = 1
                        TimserStatus.Change("EXTNL:PREC", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusRequired)
                    End If
                    TimserStatus.Change("EXTNL:ICON", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change("INFLOW:ICON", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                Next i
                If (wetadfg = 1 Or dryadfg = 1) And ltable.Parms.Item("HYDRFG") = 0 Then
                    'ad simulated and hydr off
                    TimserStatus.Change("HYDR:SAREA", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusRequired)
                End If
            End If
			
			'section htrch
            If ltable.Parms.Item("HTFG") = 1 Then
                TimserStatus.Change("INFLOW:IHEAT", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                TimserStatus.Change("EXTNL:SOLRAD", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusRequired)
                TimserStatus.Change("EXTNL:PREC", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                TimserStatus.Change("EXTNL:CLOUD", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusRequired)
                TimserStatus.Change("EXTNL:DEWTMP", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusRequired)
                TimserStatus.Change("EXTNL:GATMP", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusRequired)
                TimserStatus.Change("EXTNL:WIND", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusRequired)
                If ltable.Parms.Item("HYDRFG") = 0 Then
                    TimserStatus.Change("HYDR:AVDEP", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusRequired)
                End If
            End If
			
			'section sedtran
            If ltable.Parms.Item("SEDFG") = 1 Then
                TimserStatus.Change("INFLOW:ISED", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                TimserStatus.Change("INFLOW:ISED", 2, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                TimserStatus.Change("INFLOW:ISED", 3, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                If O.TableExists("SANDFG") Then
                    sandfg = O.Tables.Item("SANDFG").ParmValue("SANDFG")
                Else
                    sandfg = 0
                End If
                If ltable.Parms.Item("HYDRFG") = 0 Then
                    TimserStatus.Change("HYDR:TAU", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusRequired)
                    TimserStatus.Change("HYDR:AVDEP", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusRequired)
                    TimserStatus.Change("HYDR:AVVEL", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusRequired)
                    If sandfg = 1 Or sandfg = 2 Then
                        TimserStatus.Change("HYDR:RO", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusRequired)
                        TimserStatus.Change("HYDR:HRAD", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusRequired)
                        TimserStatus.Change("HYDR:TWID", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusRequired)
                    End If
                End If
                If ltable.Parms.Item("HTFG") = 0 Then
                    If sandfg = 1 Or sandfg = 2 Then
                        TimserStatus.Change("HTRCH:TW", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusRequired)
                    End If
                End If
            End If
			
			'section gqual
            If ltable.Parms.Item("GQALFG") = 1 Then
                If O.TableExists("GQ-GENDATA") Then
                    nGqual = O.Tables.Item("GQ-GENDATA").ParmValue("NGQUAL")
                Else
                    nGqual = 1
                End If
                For i = 1 To nGqual
                    TimserStatus.Change2("INFLOW:IDQAL", i, 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change2("INFLOW:ISQAL", 1, i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change2("INFLOW:ISQAL", 2, i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change2("INFLOW:ISQAL", 3, i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    If dryadfg = 1 Then
                        TimserStatus.Change("EXTNL:GQADFX", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusRequired)
                    End If
                    If wetadfg = 1 Then
                        TimserStatus.Change("EXTNL:GQADCN", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusRequired)
                    End If
                Next i
                If wetadfg = 1 Then
                    TimserStatus.Change("EXTNL:PREC", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusRequired)
                End If

                If O.TableExists("GQ-GENDATA") Then
                    If O.Tables.Item("GQ-GENDATA").ParmValue("PHFLAG") = 1 And ltable.Parms.Item("PHFG") = 0 Then
                        TimserStatus.Change("EXTNL:PHVAL", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional) 'req if there is hydrolysis
                    End If
                    If O.Tables.Item("GQ-GENDATA").ParmValue("ROXFG") = 1 Then
                        TimserStatus.Change("EXTNL:ROC", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional) 'req if there is free radical oxidation
                    End If
                    If O.Tables.Item("GQ-GENDATA").ParmValue("CLDFG") = 1 Then
                        TimserStatus.Change("EXTNL:CLOUD", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional) 'req if there is photolysis
                    End If
                End If
                For i = 1 To nGqual
                    TimserStatus.Change("EXTNL:BIO", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional) 'if qual number I undergoes biodegradation and GQPM2(7,I)=1
                Next i

                If O.TableExists("GEN-INFO") Then
                    If O.Tables.Item("GEN-INFO").ParmValue("LKFG") = 1 Then
                        TimserStatus.Change("EXTNL:WIND", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional) 'req if there is volatilization
                    End If
                End If
                If ltable.Parms.Item("HYDRFG") = 0 Then
                    If O.TableExists("GEN-INFO") Then
                        If O.Tables.Item("GEN-INFO").ParmValue("LKFG") = 0 Then
                            TimserStatus.Change("HYDR:AVDEP", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional) 'req if volatilization is on
                            TimserStatus.Change("HYDR:AVVEL", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional) 'req if volatilization is on
                        End If
                    End If
                    If (wetadfg = 1 Or dryadfg = 1) Then
                        'ad simulated
                        TimserStatus.Change("HYDR:SAREA", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusRequired)
                    End If
                End If
                If O.TableExists("GQ-GENDATA") Then
                    If ltable.Parms.Item("HTFG") = 0 And O.Tables.Item("GQ-GENDATA").ParmValue("TEMPFG") = 1 Then
                        TimserStatus.Change("HTRCH:TW", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusRequired)
                    End If
                    If ltable.Parms.Item("PLKFG") = 0 Or O.Tables.Item("GQ-GENDATA").ParmValue("PHYTFG") = 0 Then
                        If O.Tables.Item("GQ-GENDATA").ParmValue("PHYTFG") = 1 Then
                            TimserStatus.Change("PLANK:PHYTO", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional) 'req if there is photolysis
                        End If
                    End If
                    If ltable.Parms.Item("SEDFG") = 0 And O.Tables.Item("GQ-GENDATA").ParmValue("SDFG") = 1 Then
                        TimserStatus.Change("SEDTRN:SSED", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional) 'req if there is photolysis
                        TimserStatus.Change("SEDTRN:SSED", 2, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional) 'req if there is photolysis
                        TimserStatus.Change("SEDTRN:SSED", 3, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional) 'req if there is photolysis
                        TimserStatus.Change("SEDTRN:SSED", 4, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional) 'req if there is photolysis
                    End If
                End If
            End If
			
			'section oxrx
            If ltable.Parms.Item("OXFG") = 1 Then
                TimserStatus.Change("INFLOW:OXIF", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                TimserStatus.Change("INFLOW:OXIF", 2, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                If O.TableExists("GEN-INFO") Then
                    If O.Tables.Item("GEN-INFO").ParmValue("LKFG") = 1 Then
                        TimserStatus.Change("EXTNL:WIND", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusRequired)
                    End If
                End If
                If ltable.Parms.Item("HYDRFG") = 0 Then
                    TimserStatus.Change("HYDR:AVDEP", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusRequired)
                    TimserStatus.Change("HYDR:AVVEL", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusRequired) 'man had avdep twice?
                End If
                If ltable.Parms.Item("HTFG") = 0 Then
                    TimserStatus.Change("HTRCH:TW", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusRequired)
                End If
            End If
			
			'section nutrx
            If ltable.Parms.Item("NUTFG") = 1 Then
                For i = 1 To 4
                    TimserStatus.Change("INFLOW:NUIF1", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                Next i
                For i = 1 To 3
                    TimserStatus.Change2("INFLOW:NUIF2", i, 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change2("INFLOW:NUIF2", i, 2, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                Next i
                If wetadfg = 1 Or dryadfg = 1 Then
                    TimserStatus.Change("EXTNL:NUADFX", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusRequired)
                    TimserStatus.Change("EXTNL:NUADFX", 2, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusRequired)
                    TimserStatus.Change("EXTNL:NUADFX", 3, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusRequired)
                End If
                If wetadfg = 1 Then
                    TimserStatus.Change("EXTNL:NUADCN", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusRequired)
                    TimserStatus.Change("EXTNL:NUADCN", 2, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusRequired)
                    TimserStatus.Change("EXTNL:NUADCN", 3, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusRequired)
                    TimserStatus.Change("EXTNL:PREC", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusRequired)
                End If
                If (wetadfg = 1 Or dryadfg = 1) And ltable.Parms.Item("HYDRFG") = 0 Then
                    TimserStatus.Change("HYDR:SAREA", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusRequired)
                End If
                If ltable.Parms.Item("HTFG") = 0 Then
                    TimserStatus.Change("HTRCH:TW", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusRequired)
                End If
                If ltable.Parms.Item("SEDFG") = 0 Then
                    TimserStatus.Change("SEDTRN:SSED", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional) 'req if particulate NH4 or PO4 is simulated
                End If
            End If
			
			'section plank
            If ltable.Parms.Item("PLKFG") = 1 Then
                For i = 1 To 5
                    TimserStatus.Change("INFLOW:PKIF", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                Next i
                TimserStatus.Change("EXTNL:SOLRAD", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusRequired)
                If wetadfg = 1 Or dryadfg = 1 Then
                    TimserStatus.Change("EXTNL:PLADFX", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusRequired)
                    TimserStatus.Change("EXTNL:PLADFX", 2, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusRequired)
                    TimserStatus.Change("EXTNL:PLADFX", 3, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusRequired)
                End If
                If wetadfg = 1 Then
                    TimserStatus.Change("EXTNL:PLADCN", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusRequired)
                    TimserStatus.Change("EXTNL:PLADCN", 2, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusRequired)
                    TimserStatus.Change("EXTNL:PLADCN", 3, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusRequired)
                    TimserStatus.Change("EXTNL:PREC", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusRequired)
                End If
                If (wetadfg = 1 Or dryadfg = 1) And ltable.Parms.Item("HYDRFG") = 0 Then
                    TimserStatus.Change("HYDR:SAREA", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusRequired)
                End If
                If ltable.Parms.Item("HTFG") = 0 Then
                    TimserStatus.Change("HTRCH:TW", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusRequired)
                End If
                If ltable.Parms.Item("SEDFG") = 0 Then
                    If O.TableExists("PLNK-FLAGS") Then
                        If O.Tables.Item("PLNK-FLAGS").ParmValue("SDLTFG") = 1 Then
                            TimserStatus.Change("SEDTRN:SSED", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusRequired)
                            TimserStatus.Change("SEDTRN:SSED", 2, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusRequired)
                            TimserStatus.Change("SEDTRN:SSED", 3, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusRequired)
                            TimserStatus.Change("SEDTRN:SSED", 4, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusRequired)
                        End If
                    End If
                End If
            End If
			
			'section phcarb
            If ltable.Parms.Item("PHFG") = 1 Or ltable.Parms.Item("PHFG") = 3 Then
                For i = 1 To 2
                    TimserStatus.Change("INFLOW:PHIF", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                Next i
                If ltable.Parms.Item("CONSFG") = 0 And O.TableExists("PH-PARM1") Then
                    For i = 1 To O.Tables.Item("PH-PARM1").ParmValue("ALKCON")
                        TimserStatus.Change("CONS:CON", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusRequired)
                    Next i
                End If
                If ltable.Parms.Item("HTFG") = 0 Then
                    TimserStatus.Change("HTRCH:TW", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusRequired)
                End If
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
                    For i = 1 To 7
                        TimserStatus.Change("INFLOW:ACINFL", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    Next i
                    If ltable.Parms.Item("HYDRFG") = 0 Then
                        TimserStatus.Change("HYDR:AVDEP", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusRequired)
                    End If
                    If ltable.Parms.Item("HTFG") = 0 Then
                        TimserStatus.Change("HTRCH:TW", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusRequired)
                    End If
                End If
			End If
			
		End If
	End Sub
End Module