Option Strict Off
Option Explicit On
Module modStatusOutputTimeseriesPerlnd
    'Copyright 2006 AQUA TERRA Consultants - Royalty-free use permitted under open source license
	
	Public Sub UpdateOutputTimeseriesPerlnd(ByRef O As HspfOperation, ByRef TimserStatus As HspfStatus)
		Dim ltable As HspfTable
		Dim nquals As Integer
        Dim csnofg, j, i As Integer
		Dim pqadfg(20) As Integer
		Dim ctemp As String
        Dim Nqgw, nqof, Nqif, nqsd As Integer
        Dim hwtfg, npst, snopfg, irrgfg As Integer
		
		If O.TableExists("ACTIVITY") Then
			ltable = O.Tables.Item("ACTIVITY")
			
			'section atemp
            If ltable.Parms.Item("AIRTFG") = 1 Then
                TimserStatus.Change("ATEMP:AIRTMP", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
            End If
			
			'section snow
			If O.TableExists("SNOW-FLAGS") Then
                snopfg = O.Tables.Item("SNOW-FLAGS").ParmValue("SNOPFG")
			Else
				snopfg = 0
			End If
            If ltable.Parms.Item("SNOWFG") = 1 Then
                TimserStatus.Change("SNOW:PACK", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                TimserStatus.Change("SNOW:PACKF", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                TimserStatus.Change("SNOW:PACKW", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                TimserStatus.Change("SNOW:PACKI", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                TimserStatus.Change("SNOW:PDEPTH", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                TimserStatus.Change("SNOW:COVINX", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                TimserStatus.Change("SNOW:NEGHTS", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                TimserStatus.Change("SNOW:XLNMLT", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                TimserStatus.Change("SNOW:RDENPF", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                TimserStatus.Change("SNOW:SKYCLR", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                TimserStatus.Change("SNOW:SNOCOV", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                If snopfg = 0 Then
                    TimserStatus.Change("SNOW:DULL", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change("SNOW:ALBEDO", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                End If
                TimserStatus.Change("SNOW:PAKTMP", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                TimserStatus.Change("SNOW:SNOTMP", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                TimserStatus.Change("SNOW:DEWTMP", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                TimserStatus.Change("SNOW:SNOWF", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                TimserStatus.Change("SNOW:PRAIN", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                If snopfg = 0 Then
                    TimserStatus.Change("SNOW:SNOWE", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                End If
                TimserStatus.Change("SNOW:WYIELD", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                TimserStatus.Change("SNOW:MELT", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                TimserStatus.Change("SNOW:RAINF", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
            End If
			
			'section pwater
            If O.TableExists("PWAT-PARM1") Then
                With O.Tables.Item("PWAT-PARM1")
                    hwtfg = .ParmValue("HWTFG")
                    csnofg = .ParmValue("CSNOFG")
                    irrgfg = .ParmValue("IRRGFG")
                End With
            Else
                hwtfg = 0
                csnofg = 0
                irrgfg = 0
            End If
            If ltable.Parms.Item("PWATFG") = 1 Then
                TimserStatus.Change("PWATER:PERS", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                TimserStatus.Change("PWATER:CEPS", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                TimserStatus.Change("PWATER:SURS", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                TimserStatus.Change("PWATER:UZS", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                TimserStatus.Change("PWATER:IFWS", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                TimserStatus.Change("PWATER:LZS", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                TimserStatus.Change("PWATER:AGWS", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                If hwtfg = 1 Then
                    TimserStatus.Change("PWATER:TGWS", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change("PWATER:GWEL", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change("PWATER:GWVS", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change("PWATER:SURET", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                End If
                If csnofg = 1 Then
                    TimserStatus.Change("PWATER:INFFAC", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                End If
                TimserStatus.Change("PWATER:PETADJ", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                If irrgfg = 2 Then
                    TimserStatus.Change("PWATER:RZWS", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                End If
                TimserStatus.Change("PWATER:RPARM", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                TimserStatus.Change("PWATER:SUPY", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                TimserStatus.Change("PWATER:SURO", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                TimserStatus.Change("PWATER:IFWO", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                TimserStatus.Change("PWATER:AGWO", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                TimserStatus.Change("PWATER:PERO", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                TimserStatus.Change("PWATER:IGWI", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                TimserStatus.Change("PWATER:PET", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                TimserStatus.Change("PWATER:CEPE", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                TimserStatus.Change("PWATER:UZET", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                TimserStatus.Change("PWATER:LZET", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                TimserStatus.Change("PWATER:AGWET", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                TimserStatus.Change("PWATER:BASET", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                TimserStatus.Change("PWATER:TAET", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                TimserStatus.Change("PWATER:IFWI", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                TimserStatus.Change("PWATER:UZI", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                TimserStatus.Change("PWATER:INFIL", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                TimserStatus.Change("PWATER:PERC", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                TimserStatus.Change("PWATER:LZI", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                TimserStatus.Change("PWATER:AGWI", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                TimserStatus.Change("PWATER:SURI", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                If irrgfg > 0 Then
                    TimserStatus.Change("PWATER:IRRDEM", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change("PWATER:IRSHRT", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    For i = 1 To 3
                        TimserStatus.Change("PWATER:IRDRAW", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    Next i
                    For i = 1 To 6
                        TimserStatus.Change("PWATER:IRRAPP", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    Next i
                End If
            End If
			
			'section sedmnt
            If ltable.Parms.Item("SEDFG") = 1 Then
                TimserStatus.Change("SEDMNT:DETS", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                TimserStatus.Change("SEDMNT:STCAP", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                TimserStatus.Change("SEDMNT:COVER", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                TimserStatus.Change("SEDMNT:WSSD", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                TimserStatus.Change("SEDMNT:SCRSD", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                TimserStatus.Change("SEDMNT:SOSED", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                TimserStatus.Change("SEDMNT:DET", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                TimserStatus.Change("SEDMNT:NVSI", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
            End If
			
			'section pstemp
            If ltable.Parms.Item("PSTFG") = 1 Then
                TimserStatus.Change("PSTEMP:AIRTC", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                TimserStatus.Change("PSTEMP:SLTMP", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                TimserStatus.Change("PSTEMP:ULTMP", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                TimserStatus.Change("PSTEMP:LGTMP", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
            End If
			
			'section pwtgas
            If ltable.Parms.Item("PWGFG") = 1 Then
                TimserStatus.Change("PWTGAS:SOTMP", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                TimserStatus.Change("PWTGAS:IOTMP", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                TimserStatus.Change("PWTGAS:AOTMP", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                TimserStatus.Change("PWTGAS:SODOX", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                TimserStatus.Change("PWTGAS:SOCO2", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                TimserStatus.Change("PWTGAS:IODOX", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                TimserStatus.Change("PWTGAS:IOCO2", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                TimserStatus.Change("PWTGAS:AODOX", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                TimserStatus.Change("PWTGAS:AOCO2", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                TimserStatus.Change("PWTGAS:SOHT", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                TimserStatus.Change("PWTGAS:IOHT", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                TimserStatus.Change("PWTGAS:AOHT", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                TimserStatus.Change("PWTGAS:POHT", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                TimserStatus.Change("PWTGAS:SODOXM", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                TimserStatus.Change("PWTGAS:SOCO2M", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                TimserStatus.Change("PWTGAS:IODOXM", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                TimserStatus.Change("PWTGAS:IOCO2M", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                TimserStatus.Change("PWTGAS:AODOXM", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                TimserStatus.Change("PWTGAS:AOCO2M", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                TimserStatus.Change("PWTGAS:PODOXM", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                TimserStatus.Change("PWTGAS:POCO2M", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
            End If
			
			'section pqual
            If ltable.Parms.Item("PQALFG") = 1 Then
                If O.TableExists("NQUALS") Then
                    nquals = O.Tables.Item("NQUALS").ParmValue("NQUAL")
                Else
                    nquals = 1
                End If
                nqof = 0
                Nqif = 0
                Nqgw = 0
                nqsd = 0
                For i = 1 To nquals
                    ctemp = "QUAL-PROPS" & CStr(i)
                    If O.TableExists(ctemp) Then
                        With O.Tables.Item(ctemp)
                            If .ParmValue("QSOFG") > 0 Then
                                nqof = nqof + 1
                            End If
                            If .ParmValue("QIFWFG") > 0 Then
                                Nqif = Nqif + 1
                            End If
                            If .ParmValue("QAGWFG") > 0 Then
                                Nqgw = Nqgw + 1
                            End If
                            If .ParmValue("QSDFG") > 0 Then
                                nqsd = nqsd + 1
                            End If
                        End With
                    End If
                Next i

                For i = 1 To nquals
                    TimserStatus.Change("PQUAL:PQADDR", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change("PQUAL:PQADWT", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change("PQUAL:SOQUAL", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change("PQUAL:POQUAL", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change("PQUAL:SOQC", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change("PQUAL:POQC", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change("PQUAL:PQADEP", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change("PQUAL:ISQO", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                Next i
                For i = 1 To nqof
                    TimserStatus.Change("PQUAL:SQO", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change("PQUAL:SOQO", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change("PQUAL:SOQOC", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                Next i
                For i = 1 To nqsd
                    TimserStatus.Change("PQUAL:WASHQS", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change("PQUAL:SCRQS", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change("PQUAL:SOQS", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change("PQUAL:SOQSP", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                Next i
                For i = 1 To Nqif
                    TimserStatus.Change("PQUAL:IOQUAL", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change("PQUAL:IOQC", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                Next i
                For i = 1 To Nqgw
                    TimserStatus.Change("PQUAL:AOQUAL", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change("PQUAL:AOQC", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                Next i
            End If
			
			'section mstlay
            If ltable.Parms.Item("MSTLFG") = 1 Then
                For i = 1 To 5
                    TimserStatus.Change("MSTLAY:MST", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                Next i
                For i = 1 To 8
                    TimserStatus.Change("MSTLAY:FRAC", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                Next i
            End If
			
			'section pest
            If ltable.Parms.Item("PESTFG") = 1 Then
                If O.TableExists("PEST-FLAGS") Then
                    npst = O.Tables.Item("PEST-FLAGS").ParmValue("NPST")
                Else
                    npst = 1
                End If
                For i = 1 To 3
                    For j = 1 To npst
                        TimserStatus.Change2("PEST:SPS", i, j, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                        TimserStatus.Change2("PEST:UPS", i, j, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                        TimserStatus.Change2("PEST:LPS", i, j, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                        TimserStatus.Change2("PEST:APS", i, j, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                        TimserStatus.Change2("PEST:TPS", i, j, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                        TimserStatus.Change2("PEST:SSPSS", i, j, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    Next j
                Next i
                For i = 1 To npst
                    TimserStatus.Change("PEST:IPS", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change("PEST:TOTPST", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change("PEST:SDEGPS", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change("PEST:UDEGPS", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change("PEST:LDEGPS", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change("PEST:ADEGPS", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change("PEST:TDEGPS", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change("PEST:SOSDPS", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change("PEST:POPST", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change("PEST:TOPST", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    For j = 1 To 3
                        TimserStatus.Change2("PEST:PEADDR", i, j, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                        TimserStatus.Change2("PEST:PEADWT", i, j, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                        TimserStatus.Change2("PEST:PEADEP", i, j, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    Next j
                Next i
                For i = 1 To 2
                    For j = 1 To npst
                        TimserStatus.Change2("PEST:SDPS", i, j, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    Next j
                Next i
                For i = 1 To 5
                    For j = 1 To npst
                        TimserStatus.Change2("PEST:TSPSS", i, j, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    Next j
                Next i
            End If
			
			'section nitr
            If ltable.Parms.Item("NITRFG") = 1 Then
                TimserStatus.Change("NITR:AGPLTN", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                TimserStatus.Change("NITR:LITTRN", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                TimserStatus.Change("NITR:TOTNIT", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                TimserStatus.Change("NITR:SOSEDN", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                TimserStatus.Change("NITR:PONO3", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                TimserStatus.Change("NITR:PONH4", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                TimserStatus.Change("NITR:POORN", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                TimserStatus.Change("NITR:PONITR", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                TimserStatus.Change("NITR:TDENIF", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                TimserStatus.Change("NITR:RETAGN", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                For i = 1 To 3
                    TimserStatus.Change2("NITR:NIADDR", i, 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change2("NITR:NIADWT", i, 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change2("NITR:NIADEP", i, 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change2("NITR:NIADDR", i, 2, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change2("NITR:NIADWT", i, 2, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change2("NITR:NIADEP", i, 2, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change("NITR:SEDN", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change("NITR:SSAMS", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change("NITR:SSNO3", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change("NITR:SSSLN", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change("NITR:SSSRN", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change("NITR:RTLLN", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change("NITR:RTRLN", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                Next i
                For i = 1 To 4
                    TimserStatus.Change("NITR:IN", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change("NITR:NUPTG", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change("NITR:NITIF", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                Next i
                For i = 1 To 5
                    TimserStatus.Change("NITR:NDFCT", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change("NITR:TSAMS", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change("NITR:TSNO3", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change("NITR:TSSLN", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change("NITR:TSSRN", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change("NITR:NFIXFX", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change("NITR:AMVOL", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change("NITR:TNIT", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change("NITR:DENIF", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change("NITR:AMNIT", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change("NITR:AMIMB", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change("NITR:ORNMN", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change("NITR:NFIXFX", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change("NITR:REFRON", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change("NITR:NIIMB", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change("NITR:NIUPA", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change("NITR:AMUPA", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change("NITR:NIUPB", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change("NITR:AMUPB", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change("NITR:RTLBN", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change("NITR:RTRBN", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                Next i
                For i = 1 To 8
                    TimserStatus.Change("NITR:SN", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change("NITR:UN", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change("NITR:LN", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change("NITR:AN", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change("NITR:TN", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                Next i
            End If
			
			'section phos
            If ltable.Parms.Item("PHOSFG") = 1 Then
                TimserStatus.Change("PHOS:IP", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                TimserStatus.Change("PHOS:TOTPHO", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                'TimserStatus.Change "PHOS:SPDFC", 1, HspfStatusOptional 'no longer in msg file?
                TimserStatus.Change("PHOS:SOSEDP", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                TimserStatus.Change("PHOS:POPHOS", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                For i = 1 To 2
                    TimserStatus.Change2("PHOS:PHADDR", i, 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change2("PHOS:PHADWT", i, 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change2("PHOS:PHADEP", i, 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change2("PHOS:PHADDR", i, 2, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change2("PHOS:PHADWT", i, 2, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change2("PHOS:PHADEP", i, 2, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change("PHOS:SEDP", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                Next i
                For i = 1 To 3
                    TimserStatus.Change("PHOS:SSP4S", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change("PHOS:PHOIF", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                Next i
                For i = 1 To 4
                    TimserStatus.Change("PHOS:SP", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change("PHOS:UP", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change("PHOS:LP", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change("PHOS:AP", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change("PHOS:TP", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change("PHOS:PUPTG", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                Next i
                For i = 1 To 5
                    TimserStatus.Change("PHOS:PDFCT", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change("PHOS:TSP4S", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change("PHOS:TPHO", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change("PHOS:P4IMB", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change("PHOS:ORPMN", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                Next i
            End If
			
			'section tracer
            If ltable.Parms.Item("TRACFG") = 1 Then
                TimserStatus.Change("TRACER:STRSU", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                TimserStatus.Change("TRACER:UTRSU", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                TimserStatus.Change("TRACER:ITRSU", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                TimserStatus.Change("TRACER:LTRSU", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                TimserStatus.Change("TRACER:ATRSU", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                TimserStatus.Change("TRACER:TRSU", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                TimserStatus.Change("TRACER:POTRS", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                For i = 1 To 2
                    TimserStatus.Change("TRACER:TRADDR", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change("TRACER:TRADWT", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TimserStatus.Change("TRACER:TRADEP", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                Next i
                For i = 1 To 3
                    TimserStatus.Change("TRACER:SSTRS", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                Next i
                For i = 1 To 5
                    TimserStatus.Change("TRACER:TSTRS", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                Next i
            End If
			
		End If
	End Sub
End Module