Option Strict Off
Option Explicit On

Imports atcuci.HspfStatus.HspfStatusReqOptUnnEnum

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
            If ltable.Parms("AIRTFG").Value = 1 Then
                TimserStatus.Change("ATEMP:AIRTMP", 1, HspfStatusOptional)
            End If

            'section snow
            If O.TableExists("SNOW-FLAGS") Then
                snopfg = O.Tables.Item("SNOW-FLAGS").ParmValue("SNOPFG")
            Else
                snopfg = 0
            End If
            If ltable.Parms("SNOWFG").Value = 1 Then
                TimserStatus.Change("SNOW:PACK", 1, HspfStatusOptional)
                TimserStatus.Change("SNOW:PACKF", 1, HspfStatusOptional)
                TimserStatus.Change("SNOW:PACKW", 1, HspfStatusOptional)
                TimserStatus.Change("SNOW:PACKI", 1, HspfStatusOptional)
                TimserStatus.Change("SNOW:PDEPTH", 1, HspfStatusOptional)
                TimserStatus.Change("SNOW:COVINX", 1, HspfStatusOptional)
                TimserStatus.Change("SNOW:NEGHTS", 1, HspfStatusOptional)
                TimserStatus.Change("SNOW:XLNMLT", 1, HspfStatusOptional)
                TimserStatus.Change("SNOW:RDENPF", 1, HspfStatusOptional)
                TimserStatus.Change("SNOW:SKYCLR", 1, HspfStatusOptional)
                TimserStatus.Change("SNOW:SNOCOV", 1, HspfStatusOptional)
                If snopfg = 0 Then
                    TimserStatus.Change("SNOW:DULL", 1, HspfStatusOptional)
                    TimserStatus.Change("SNOW:ALBEDO", 1, HspfStatusOptional)
                End If
                TimserStatus.Change("SNOW:PAKTMP", 1, HspfStatusOptional)
                TimserStatus.Change("SNOW:SNOTMP", 1, HspfStatusOptional)
                TimserStatus.Change("SNOW:DEWTMP", 1, HspfStatusOptional)
                TimserStatus.Change("SNOW:SNOWF", 1, HspfStatusOptional)
                TimserStatus.Change("SNOW:PRAIN", 1, HspfStatusOptional)
                If snopfg = 0 Then
                    TimserStatus.Change("SNOW:SNOWE", 1, HspfStatusOptional)
                End If
                TimserStatus.Change("SNOW:WYIELD", 1, HspfStatusOptional)
                TimserStatus.Change("SNOW:MELT", 1, HspfStatusOptional)
                TimserStatus.Change("SNOW:RAINF", 1, HspfStatusOptional)
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
            If ltable.Parms("PWATFG").Value = 1 Then
                TimserStatus.Change("PWATER:PERS", 1, HspfStatusOptional)
                TimserStatus.Change("PWATER:CEPS", 1, HspfStatusOptional)
                TimserStatus.Change("PWATER:SURS", 1, HspfStatusOptional)
                TimserStatus.Change("PWATER:UZS", 1, HspfStatusOptional)
                TimserStatus.Change("PWATER:IFWS", 1, HspfStatusOptional)
                TimserStatus.Change("PWATER:LZS", 1, HspfStatusOptional)
                TimserStatus.Change("PWATER:AGWS", 1, HspfStatusOptional)
                If hwtfg = 1 Then
                    TimserStatus.Change("PWATER:TGWS", 1, HspfStatusOptional)
                    TimserStatus.Change("PWATER:GWEL", 1, HspfStatusOptional)
                    TimserStatus.Change("PWATER:GWVS", 1, HspfStatusOptional)
                    TimserStatus.Change("PWATER:SURET", 1, HspfStatusOptional)
                End If
                If csnofg = 1 Then
                    TimserStatus.Change("PWATER:INFFAC", 1, HspfStatusOptional)
                End If
                TimserStatus.Change("PWATER:PETADJ", 1, HspfStatusOptional)
                If irrgfg = 2 Then
                    TimserStatus.Change("PWATER:RZWS", 1, HspfStatusOptional)
                End If
                TimserStatus.Change("PWATER:RPARM", 1, HspfStatusOptional)
                TimserStatus.Change("PWATER:SUPY", 1, HspfStatusOptional)
                TimserStatus.Change("PWATER:SURO", 1, HspfStatusOptional)
                TimserStatus.Change("PWATER:IFWO", 1, HspfStatusOptional)
                TimserStatus.Change("PWATER:AGWO", 1, HspfStatusOptional)
                TimserStatus.Change("PWATER:PERO", 1, HspfStatusOptional)
                TimserStatus.Change("PWATER:IGWI", 1, HspfStatusOptional)
                TimserStatus.Change("PWATER:PET", 1, HspfStatusOptional)
                TimserStatus.Change("PWATER:CEPE", 1, HspfStatusOptional)
                TimserStatus.Change("PWATER:UZET", 1, HspfStatusOptional)
                TimserStatus.Change("PWATER:LZET", 1, HspfStatusOptional)
                TimserStatus.Change("PWATER:AGWET", 1, HspfStatusOptional)
                TimserStatus.Change("PWATER:BASET", 1, HspfStatusOptional)
                TimserStatus.Change("PWATER:TAET", 1, HspfStatusOptional)
                TimserStatus.Change("PWATER:IFWI", 1, HspfStatusOptional)
                TimserStatus.Change("PWATER:UZI", 1, HspfStatusOptional)
                TimserStatus.Change("PWATER:INFIL", 1, HspfStatusOptional)
                TimserStatus.Change("PWATER:PERC", 1, HspfStatusOptional)
                TimserStatus.Change("PWATER:LZI", 1, HspfStatusOptional)
                TimserStatus.Change("PWATER:AGWI", 1, HspfStatusOptional)
                TimserStatus.Change("PWATER:SURI", 1, HspfStatusOptional)
                If irrgfg > 0 Then
                    TimserStatus.Change("PWATER:IRRDEM", 1, HspfStatusOptional)
                    TimserStatus.Change("PWATER:IRSHRT", 1, HspfStatusOptional)
                    For i = 1 To 3
                        TimserStatus.Change("PWATER:IRDRAW", i, HspfStatusOptional)
                    Next i
                    For i = 1 To 6
                        TimserStatus.Change("PWATER:IRRAPP", i, HspfStatusOptional)
                    Next i
                End If
            End If

            'section sedmnt
            If ltable.Parms("SEDFG").Value = 1 Then
                TimserStatus.Change("SEDMNT:DETS", 1, HspfStatusOptional)
                TimserStatus.Change("SEDMNT:STCAP", 1, HspfStatusOptional)
                TimserStatus.Change("SEDMNT:COVER", 1, HspfStatusOptional)
                TimserStatus.Change("SEDMNT:WSSD", 1, HspfStatusOptional)
                TimserStatus.Change("SEDMNT:SCRSD", 1, HspfStatusOptional)
                TimserStatus.Change("SEDMNT:SOSED", 1, HspfStatusOptional)
                TimserStatus.Change("SEDMNT:DET", 1, HspfStatusOptional)
                TimserStatus.Change("SEDMNT:NVSI", 1, HspfStatusOptional)
            End If

            'section pstemp
            If ltable.Parms("PSTFG").Value = 1 Then
                TimserStatus.Change("PSTEMP:AIRTC", 1, HspfStatusOptional)
                TimserStatus.Change("PSTEMP:SLTMP", 1, HspfStatusOptional)
                TimserStatus.Change("PSTEMP:ULTMP", 1, HspfStatusOptional)
                TimserStatus.Change("PSTEMP:LGTMP", 1, HspfStatusOptional)
            End If

            'section pwtgas
            If ltable.Parms("PWGFG").Value = 1 Then
                TimserStatus.Change("PWTGAS:SOTMP", 1, HspfStatusOptional)
                TimserStatus.Change("PWTGAS:IOTMP", 1, HspfStatusOptional)
                TimserStatus.Change("PWTGAS:AOTMP", 1, HspfStatusOptional)
                TimserStatus.Change("PWTGAS:SODOX", 1, HspfStatusOptional)
                TimserStatus.Change("PWTGAS:SOCO2", 1, HspfStatusOptional)
                TimserStatus.Change("PWTGAS:IODOX", 1, HspfStatusOptional)
                TimserStatus.Change("PWTGAS:IOCO2", 1, HspfStatusOptional)
                TimserStatus.Change("PWTGAS:AODOX", 1, HspfStatusOptional)
                TimserStatus.Change("PWTGAS:AOCO2", 1, HspfStatusOptional)
                TimserStatus.Change("PWTGAS:SOHT", 1, HspfStatusOptional)
                TimserStatus.Change("PWTGAS:IOHT", 1, HspfStatusOptional)
                TimserStatus.Change("PWTGAS:AOHT", 1, HspfStatusOptional)
                TimserStatus.Change("PWTGAS:POHT", 1, HspfStatusOptional)
                TimserStatus.Change("PWTGAS:SODOXM", 1, HspfStatusOptional)
                TimserStatus.Change("PWTGAS:SOCO2M", 1, HspfStatusOptional)
                TimserStatus.Change("PWTGAS:IODOXM", 1, HspfStatusOptional)
                TimserStatus.Change("PWTGAS:IOCO2M", 1, HspfStatusOptional)
                TimserStatus.Change("PWTGAS:AODOXM", 1, HspfStatusOptional)
                TimserStatus.Change("PWTGAS:AOCO2M", 1, HspfStatusOptional)
                TimserStatus.Change("PWTGAS:PODOXM", 1, HspfStatusOptional)
                TimserStatus.Change("PWTGAS:POCO2M", 1, HspfStatusOptional)
            End If

            'section pqual
            If ltable.Parms("PQALFG").Value = 1 Then
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
                    TimserStatus.Change("PQUAL:PQADDR", i, HspfStatusOptional)
                    TimserStatus.Change("PQUAL:PQADWT", i, HspfStatusOptional)
                    TimserStatus.Change("PQUAL:SOQUAL", i, HspfStatusOptional)
                    TimserStatus.Change("PQUAL:POQUAL", i, HspfStatusOptional)
                    TimserStatus.Change("PQUAL:SOQC", i, HspfStatusOptional)
                    TimserStatus.Change("PQUAL:POQC", i, HspfStatusOptional)
                    TimserStatus.Change("PQUAL:PQADEP", i, HspfStatusOptional)
                    TimserStatus.Change("PQUAL:ISQO", i, HspfStatusOptional)
                Next i
                For i = 1 To nqof
                    TimserStatus.Change("PQUAL:SQO", i, HspfStatusOptional)
                    TimserStatus.Change("PQUAL:SOQO", i, HspfStatusOptional)
                    TimserStatus.Change("PQUAL:SOQOC", i, HspfStatusOptional)
                Next i
                For i = 1 To nqsd
                    TimserStatus.Change("PQUAL:WASHQS", i, HspfStatusOptional)
                    TimserStatus.Change("PQUAL:SCRQS", i, HspfStatusOptional)
                    TimserStatus.Change("PQUAL:SOQS", i, HspfStatusOptional)
                    TimserStatus.Change("PQUAL:SOQSP", i, HspfStatusOptional)
                Next i
                For i = 1 To Nqif
                    TimserStatus.Change("PQUAL:IOQUAL", i, HspfStatusOptional)
                    TimserStatus.Change("PQUAL:IOQC", i, HspfStatusOptional)
                Next i
                For i = 1 To Nqgw
                    TimserStatus.Change("PQUAL:AOQUAL", i, HspfStatusOptional)
                    TimserStatus.Change("PQUAL:AOQC", i, HspfStatusOptional)
                Next i
            End If

            'section mstlay
            If ltable.Parms("MSTLFG").Value = 1 Then
                For i = 1 To 5
                    TimserStatus.Change("MSTLAY:MST", i, HspfStatusOptional)
                Next i
                For i = 1 To 8
                    TimserStatus.Change("MSTLAY:FRAC", i, HspfStatusOptional)
                Next i
            End If

            'section pest
            If ltable.Parms("PESTFG").Value = 1 Then
                If O.TableExists("PEST-FLAGS") Then
                    npst = O.Tables.Item("PEST-FLAGS").ParmValue("NPST")
                Else
                    npst = 1
                End If
                For i = 1 To 3
                    For j = 1 To npst
                        TimserStatus.Change2("PEST:SPS", i, j, HspfStatusOptional)
                        TimserStatus.Change2("PEST:UPS", i, j, HspfStatusOptional)
                        TimserStatus.Change2("PEST:LPS", i, j, HspfStatusOptional)
                        TimserStatus.Change2("PEST:APS", i, j, HspfStatusOptional)
                        TimserStatus.Change2("PEST:TPS", i, j, HspfStatusOptional)
                        TimserStatus.Change2("PEST:SSPSS", i, j, HspfStatusOptional)
                    Next j
                Next i
                For i = 1 To npst
                    TimserStatus.Change("PEST:IPS", i, HspfStatusOptional)
                    TimserStatus.Change("PEST:TOTPST", i, HspfStatusOptional)
                    TimserStatus.Change("PEST:SDEGPS", i, HspfStatusOptional)
                    TimserStatus.Change("PEST:UDEGPS", i, HspfStatusOptional)
                    TimserStatus.Change("PEST:LDEGPS", i, HspfStatusOptional)
                    TimserStatus.Change("PEST:ADEGPS", i, HspfStatusOptional)
                    TimserStatus.Change("PEST:TDEGPS", i, HspfStatusOptional)
                    TimserStatus.Change("PEST:SOSDPS", i, HspfStatusOptional)
                    TimserStatus.Change("PEST:POPST", i, HspfStatusOptional)
                    TimserStatus.Change("PEST:TOPST", i, HspfStatusOptional)
                    For j = 1 To 3
                        TimserStatus.Change2("PEST:PEADDR", i, j, HspfStatusOptional)
                        TimserStatus.Change2("PEST:PEADWT", i, j, HspfStatusOptional)
                        TimserStatus.Change2("PEST:PEADEP", i, j, HspfStatusOptional)
                    Next j
                Next i
                For i = 1 To 2
                    For j = 1 To npst
                        TimserStatus.Change2("PEST:SDPS", i, j, HspfStatusOptional)
                    Next j
                Next i
                For i = 1 To 5
                    For j = 1 To npst
                        TimserStatus.Change2("PEST:TSPSS", i, j, HspfStatusOptional)
                    Next j
                Next i
            End If

            'section nitr
            If ltable.Parms("NITRFG").Value = 1 Then
                TimserStatus.Change("NITR:AGPLTN", 1, HspfStatusOptional)
                TimserStatus.Change("NITR:LITTRN", 1, HspfStatusOptional)
                TimserStatus.Change("NITR:TOTNIT", 1, HspfStatusOptional)
                TimserStatus.Change("NITR:SOSEDN", 1, HspfStatusOptional)
                TimserStatus.Change("NITR:PONO3", 1, HspfStatusOptional)
                TimserStatus.Change("NITR:PONH4", 1, HspfStatusOptional)
                TimserStatus.Change("NITR:POORN", 1, HspfStatusOptional)
                TimserStatus.Change("NITR:PONITR", 1, HspfStatusOptional)
                TimserStatus.Change("NITR:TDENIF", 1, HspfStatusOptional)
                TimserStatus.Change("NITR:RETAGN", 1, HspfStatusOptional)
                For i = 1 To 3
                    TimserStatus.Change2("NITR:NIADDR", i, 1, HspfStatusOptional)
                    TimserStatus.Change2("NITR:NIADWT", i, 1, HspfStatusOptional)
                    TimserStatus.Change2("NITR:NIADEP", i, 1, HspfStatusOptional)
                    TimserStatus.Change2("NITR:NIADDR", i, 2, HspfStatusOptional)
                    TimserStatus.Change2("NITR:NIADWT", i, 2, HspfStatusOptional)
                    TimserStatus.Change2("NITR:NIADEP", i, 2, HspfStatusOptional)
                    TimserStatus.Change("NITR:SEDN", i, HspfStatusOptional)
                    TimserStatus.Change("NITR:SSAMS", i, HspfStatusOptional)
                    TimserStatus.Change("NITR:SSNO3", i, HspfStatusOptional)
                    TimserStatus.Change("NITR:SSSLN", i, HspfStatusOptional)
                    TimserStatus.Change("NITR:SSSRN", i, HspfStatusOptional)
                    TimserStatus.Change("NITR:RTLLN", i, HspfStatusOptional)
                    TimserStatus.Change("NITR:RTRLN", i, HspfStatusOptional)
                Next i
                For i = 1 To 4
                    TimserStatus.Change("NITR:IN", i, HspfStatusOptional)
                    TimserStatus.Change("NITR:NUPTG", i, HspfStatusOptional)
                    TimserStatus.Change("NITR:NITIF", i, HspfStatusOptional)
                Next i
                For i = 1 To 5
                    TimserStatus.Change("NITR:NDFCT", i, HspfStatusOptional)
                    TimserStatus.Change("NITR:TSAMS", i, HspfStatusOptional)
                    TimserStatus.Change("NITR:TSNO3", i, HspfStatusOptional)
                    TimserStatus.Change("NITR:TSSLN", i, HspfStatusOptional)
                    TimserStatus.Change("NITR:TSSRN", i, HspfStatusOptional)
                    TimserStatus.Change("NITR:NFIXFX", i, HspfStatusOptional)
                    TimserStatus.Change("NITR:AMVOL", i, HspfStatusOptional)
                    TimserStatus.Change("NITR:TNIT", i, HspfStatusOptional)
                    TimserStatus.Change("NITR:DENIF", i, HspfStatusOptional)
                    TimserStatus.Change("NITR:AMNIT", i, HspfStatusOptional)
                    TimserStatus.Change("NITR:AMIMB", i, HspfStatusOptional)
                    TimserStatus.Change("NITR:ORNMN", i, HspfStatusOptional)
                    TimserStatus.Change("NITR:NFIXFX", i, HspfStatusOptional)
                    TimserStatus.Change("NITR:REFRON", i, HspfStatusOptional)
                    TimserStatus.Change("NITR:NIIMB", i, HspfStatusOptional)
                    TimserStatus.Change("NITR:NIUPA", i, HspfStatusOptional)
                    TimserStatus.Change("NITR:AMUPA", i, HspfStatusOptional)
                    TimserStatus.Change("NITR:NIUPB", i, HspfStatusOptional)
                    TimserStatus.Change("NITR:AMUPB", i, HspfStatusOptional)
                    TimserStatus.Change("NITR:RTLBN", i, HspfStatusOptional)
                    TimserStatus.Change("NITR:RTRBN", i, HspfStatusOptional)
                Next i
                For i = 1 To 8
                    TimserStatus.Change("NITR:SN", i, HspfStatusOptional)
                    TimserStatus.Change("NITR:UN", i, HspfStatusOptional)
                    TimserStatus.Change("NITR:LN", i, HspfStatusOptional)
                    TimserStatus.Change("NITR:AN", i, HspfStatusOptional)
                    TimserStatus.Change("NITR:TN", i, HspfStatusOptional)
                Next i
            End If

            'section phos
            If ltable.Parms("PHOSFG").Value = 1 Then
                TimserStatus.Change("PHOS:IP", 1, HspfStatusOptional)
                TimserStatus.Change("PHOS:TOTPHO", 1, HspfStatusOptional)
                'TimserStatus.Change "PHOS:SPDFC", 1, HspfStatusOptional 'no longer in msg file?
                TimserStatus.Change("PHOS:SOSEDP", 1, HspfStatusOptional)
                TimserStatus.Change("PHOS:POPHOS", 1, HspfStatusOptional)
                For i = 1 To 2
                    TimserStatus.Change2("PHOS:PHADDR", i, 1, HspfStatusOptional)
                    TimserStatus.Change2("PHOS:PHADWT", i, 1, HspfStatusOptional)
                    TimserStatus.Change2("PHOS:PHADEP", i, 1, HspfStatusOptional)
                    TimserStatus.Change2("PHOS:PHADDR", i, 2, HspfStatusOptional)
                    TimserStatus.Change2("PHOS:PHADWT", i, 2, HspfStatusOptional)
                    TimserStatus.Change2("PHOS:PHADEP", i, 2, HspfStatusOptional)
                    TimserStatus.Change("PHOS:SEDP", i, HspfStatusOptional)
                Next i
                For i = 1 To 3
                    TimserStatus.Change("PHOS:SSP4S", i, HspfStatusOptional)
                    TimserStatus.Change("PHOS:PHOIF", i, HspfStatusOptional)
                Next i
                For i = 1 To 4
                    TimserStatus.Change("PHOS:SP", i, HspfStatusOptional)
                    TimserStatus.Change("PHOS:UP", i, HspfStatusOptional)
                    TimserStatus.Change("PHOS:LP", i, HspfStatusOptional)
                    TimserStatus.Change("PHOS:AP", i, HspfStatusOptional)
                    TimserStatus.Change("PHOS:TP", i, HspfStatusOptional)
                    TimserStatus.Change("PHOS:PUPTG", i, HspfStatusOptional)
                Next i
                For i = 1 To 5
                    TimserStatus.Change("PHOS:PDFCT", i, HspfStatusOptional)
                    TimserStatus.Change("PHOS:TSP4S", i, HspfStatusOptional)
                    TimserStatus.Change("PHOS:TPHO", i, HspfStatusOptional)
                    TimserStatus.Change("PHOS:P4IMB", i, HspfStatusOptional)
                    TimserStatus.Change("PHOS:ORPMN", i, HspfStatusOptional)
                Next i
            End If

            'section tracer
            If ltable.Parms("TRACFG").Value = 1 Then
                TimserStatus.Change("TRACER:STRSU", 1, HspfStatusOptional)
                TimserStatus.Change("TRACER:UTRSU", 1, HspfStatusOptional)
                TimserStatus.Change("TRACER:ITRSU", 1, HspfStatusOptional)
                TimserStatus.Change("TRACER:LTRSU", 1, HspfStatusOptional)
                TimserStatus.Change("TRACER:ATRSU", 1, HspfStatusOptional)
                TimserStatus.Change("TRACER:TRSU", 1, HspfStatusOptional)
                TimserStatus.Change("TRACER:POTRS", 1, HspfStatusOptional)
                For i = 1 To 2
                    TimserStatus.Change("TRACER:TRADDR", i, HspfStatusOptional)
                    TimserStatus.Change("TRACER:TRADWT", i, HspfStatusOptional)
                    TimserStatus.Change("TRACER:TRADEP", i, HspfStatusOptional)
                Next i
                For i = 1 To 3
                    TimserStatus.Change("TRACER:SSTRS", i, HspfStatusOptional)
                Next i
                For i = 1 To 5
                    TimserStatus.Change("TRACER:TSTRS", i, HspfStatusOptional)
                Next i
            End If
        End If
    End Sub
End Module