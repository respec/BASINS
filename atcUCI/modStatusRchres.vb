Option Strict Off
Option Explicit On
Module modStatusRchres
    'Copyright 2006 AQUA TERRA Consultants - Royalty-free use permitted under open source license
	
	Public Sub UpdateRchres(ByRef O As HspfOperation, ByRef TableStatus As HspfStatus)
		Dim ltable As HspfTable
		Dim i, j As Integer
		Dim t, f As String
		'added j   THJ
		Dim Bedflg, nCons, Tgflg As Integer
		Dim mPhoto, mCloud, mPhval, nGqual, mWatemp, mRoxygen, mSedconc, mBio As Integer
		'added mBio flag   THJ
		Dim Oxidation, Hydrolysis, Photolysis As Boolean
		Dim Biodegradation, Volatilization As Boolean
		'added Biodegradation flag  THJ
		Dim nGenDec, nBiod, nPhot, nHydro, nOxid, nVolat, nBioM, nSedAs As Integer
		'added gqual process counters - these are SUB1-SUB7 in PGQUAL   THJ
		Dim Gdaufg(6) As Integer
		Dim Daufg(6) As Integer
		Dim nDaught As Integer
		'added gqual daughter product flags
		Dim Lkfg, Benrfg, Reamfg As Integer
		Dim Po4fg, Phflag, Tamfg, Amvfg, Adnhfg, Adpofg As Integer
		Dim Zoofg, Phyfg, Balfg As Integer
		
		'always can be present
		TableStatus.Change("ACTIVITY", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusRequired)
		TableStatus.Change("PRINT-INFO", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
		TableStatus.Change("GEN-INFO", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusRequired)
		
		If O.TableExists("GEN-INFO") Then
            Lkfg = O.Tables.Item("GEN-INFO").ParmValue("LKFG")
		Else
			Lkfg = 0
		End If
		
		Dim lAcidph As Boolean
		If O.TableExists("ACTIVITY") Then
			'moved Benrfg to OXRX  THJ
			ltable = O.Tables.Item("ACTIVITY")
            If ltable.Parms.Item("HYDRFG") = 1 Then
                TableStatus.Change("HYDR-PARM1", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                If O.TableExists("HYDR-PARM1") Then
                    If O.Tables.Item("HYDR-PARM1").ParmValue("VCONFG") = 1 Then
                        TableStatus.Change("MON-CONVF", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    End If
                End If
                TableStatus.Change("HYDR-PARM2", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusRequired)
                TableStatus.Change("HYDR-IRRIG", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                'added HYDR-IRRIG   THJ
                TableStatus.Change("HYDR-INIT", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                'must pass NCAT from CATEGORY to NCATS in HYDR to be able to
                'process categories.
            End If
            If ltable.Parms.Item("ADFG") = 1 Then
                TableStatus.Change("ADCALC-DATA", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
            End If
            If ltable.Parms.Item("CONSFG") = 1 Then
                TableStatus.Change("NCONS", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                TableStatus.Change("CONS-AD-FLAG", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                If O.TableExists("NCONS") Then
                    nCons = O.Tables.Item("NCONS").ParmValue("NCONS")
                Else
                    nCons = 1
                End If
                For i = 1 To nCons
                    TableStatus.Change("CONS-DATA", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusRequired)
                Next i
            End If			
			If ltable.Parms.Item("HTFG") = 1 Then
				TableStatus.Change("HT-BED-FLAGS", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
				TableStatus.Change("HEAT-PARM", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
				If O.TableExists("HT-BED-FLAGS") Then
                    Bedflg = O.Tables.Item("HT-BED-FLAGS").ParmValue("BEDFLG")
                    Tgflg = O.Tables.Item("HT-BED-FLAGS").ParmValue("TGFLG")
					If Bedflg = 1 Or Bedflg = 2 Then
						TableStatus.Change("HT-BED-PARM", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
						If Tgflg = 3 Then
							TableStatus.Change("MON-HT-TGRND", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
						End If
					ElseIf Bedflg = 3 Then 
						TableStatus.Change("HT-BED-DELH", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
						TableStatus.Change("HT-BED-DELTT", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
					End If
				End If
				TableStatus.Change("HEAT-INIT", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
			End If
            If ltable.Parms.Item("SEDFG") = 1 Then
                TableStatus.Change("SANDFG", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                TableStatus.Change("SED-GENPARM", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusRequired)
                If ltable.Parms.Item("HYDRFG") = 0 Then
                    TableStatus.Change("SED-HYDPARM", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusRequired)
                End If
                TableStatus.Change("SAND-PM", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusRequired)
                TableStatus.Change("SILT-CLAY-PM", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusRequired)
                TableStatus.Change("SILT-CLAY-PM", 2, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusRequired)
                'added second occurrence  Technically, these tables are defaultable,
                'but not realistically.  I'd just as soon leave them Required  THJ
                TableStatus.Change("SSED-INIT", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                TableStatus.Change("BED-INIT", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
            End If
            If ltable.Parms.Item("GQALFG") = 1 Then
                TableStatus.Change("GQ-GENDATA", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                TableStatus.Change("GQ-AD-FLAGS", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                If O.TableExists("GQ-GENDATA") Then
                    With O.Tables.Item("GQ-GENDATA")
                        nGqual = .ParmValue("NGQUAL")
                        mWatemp = .ParmValue("TEMPFG")
                        mPhval = .ParmValue("PHFLAG")
                        mRoxygen = .ParmValue("ROXFG")
                        mCloud = .ParmValue("CLDFG")
                        mSedconc = .ParmValue("SDFG")
                        mPhoto = .ParmValue("PHYTFG")
                    End With
                Else 'defaults
                    nGqual = 1
                    mWatemp = 2
                    mPhval = 2
                    mRoxygen = 2
                    mCloud = 2
                    mSedconc = 2
                    mPhoto = 2
                End If
                Hydrolysis = False
                Oxidation = False
                Photolysis = False
                Volatilization = False
                Biodegradation = False
                nHydro = 0
                nOxid = 0
                nPhot = 0
                nVolat = 0
                nBiod = 0
                nBioM = 0
                nGenDec = 0
                nSedAs = 0
                For j = 1 To 6
                    Gdaufg(j) = 0
                Next j
                nDaught = 0
                For i = 1 To nGqual
                    TableStatus.Change("GQ-QALDATA", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusRequired)
                    TableStatus.Change("GQ-QALFG", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TableStatus.Change("GQ-FLG2", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    f = "GQ-FLG2"
                    If i > 1 Then f = f & ":" & i
                    'first parse flg2 to get dauther relationships and bio
                    If O.TableExists(f) Then
                        With O.Tables.Item(f)
                            Daufg(1) = .ParmValue("GQPM21")
                            Daufg(2) = .ParmValue("GQPM22")
                            Daufg(3) = .ParmValue("GQPM23")
                            Daufg(4) = .ParmValue("GQPM24")
                            Daufg(5) = .ParmValue("GQPM25")
                            Daufg(6) = .ParmValue("GQPM26")
                            For j = 1 To 5
                                If Daufg(j) = 1 Then
                                    Gdaufg(j) = 1
                                End If
                            Next j
                            mBio = .ParmValue("GQPM27")
                        End With
                    Else
                        mBio = 2
                    End If
                    'now can parse qalfg  THJ
                    t = "GQ-QALFG"
                    If i > 1 Then t = t & ":" & i
                    If O.TableExists(t) Then
                        With O.Tables.Item(t)
                            If .ParmValue("QALFG1") = 1 Then
                                nHydro = nHydro + 1
                                TableStatus.Change("GQ-HYDPM", nHydro, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusRequired)
                                Hydrolysis = True
                            End If
                            If .ParmValue("QALFG2") = 1 Then
                                nOxid = nOxid + 1
                                TableStatus.Change("GQ-ROXPM", nOxid, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusRequired)
                                Oxidation = True
                            End If
                            If .ParmValue("QALFG3") = 1 Then
                                nPhot = nPhot + 1
                                TableStatus.Change("GQ-PHOTPM", nPhot, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                                Photolysis = True
                            End If
                            If .ParmValue("QALFG4") = 1 Then
                                nVolat = nVolat + 1
                                TableStatus.Change("GQ-CFGAS", nVolat, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusRequired)
                                Volatilization = True
                            End If
                            If .ParmValue("QALFG5") = 1 Then
                                nBiod = nBiod + 1
                                TableStatus.Change("GQ-BIOPM", nBiod, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusRequired)
                                Biodegradation = True
                                If mBio = 3 Then
                                    nBioM = nBioM + 1
                                    TableStatus.Change("MON-BIO", nBioM, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusRequired)
                                End If
                            End If
                            If .ParmValue("QALFG6") = 1 Then
                                nGenDec = nGenDec + 1
                                TableStatus.Change("GQ-GENDECAY", nGenDec, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusRequired)
                            End If
                            If .ParmValue("QALFG7") = 1 Then
                                nSedAs = nSedAs + 1
                                TableStatus.Change("GQ-SEDDECAY", nSedAs, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                                TableStatus.Change("GQ-KD", nSedAs, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusRequired)
                                TableStatus.Change("GQ-ADRATE", nSedAs, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusRequired)
                                TableStatus.Change("GQ-ADTHETA", nSedAs, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                                TableStatus.Change("GQ-SEDCONC", nSedAs, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                            End If
                        End With
                    End If
                Next i

                TableStatus.Change("GQ-VALUES", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                'only one occurrence  THJ
                If mWatemp = 3 Then
                    TableStatus.Change("MON-WATEMP", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                End If
                If mPhval = 3 And Hydrolysis Then
                    TableStatus.Change("MON-PHVAL", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                End If
                If mRoxygen And Oxidation Then
                    TableStatus.Change("MON-ROXYGEN", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    'corrected spelling of table name  THJ
                End If
                If Photolysis Then
                    TableStatus.Change("GQ-ALPHA", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusRequired)
                    TableStatus.Change("GQ-GAMMA", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TableStatus.Change("GQ-DELTA", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TableStatus.Change("GQ-CLDFACT", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    If mCloud Then
                        TableStatus.Change("MON-CLOUD", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    End If
                    If mSedconc Then
                        TableStatus.Change("MON-SEDCONC", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    End If
                    If mPhoto Then
                        TableStatus.Change("MON-PHYTO", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    End If
                    If ltable.Parms.Item("HTFG") = 0 Then
                        TableStatus.Change("SURF-EXPOSED", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    End If
                End If
                If Volatilization Then
                    TableStatus.Change("OX-FLAGS", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    If O.TableExists("OX-FLAGS") Then
                        Reamfg = O.Tables.Item("OX-FLAGS").ParmValue("REAMFG")
                    Else
                        Reamfg = 2
                    End If
                    If ltable.Parms.Item("HTFG") = 0 Then
                        TableStatus.Change("ELEV", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                        'added check on HTFG   THJ
                    End If
                    If Lkfg = 1 Then
                        TableStatus.Change("OX-CFOREA", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                        'added check on Lkfg   THJ
                    Else
                        If Reamfg = 1 Then
                            TableStatus.Change("OX-TSIVOGLOU", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                            If ltable.Parms.Item("HYDRFG") = 0 Then
                                TableStatus.Change("OX-LEN-DELTH", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusRequired)
                            End If
                        ElseIf Reamfg = 2 Then
                            TableStatus.Change("OX-TCGINV", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                        ElseIf Reamfg = 3 Then
                            TableStatus.Change("OX-REAPARM", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusRequired)
                        End If
                    End If
                End If
                'reworked GQ-DAUGHTER   THJ
                For i = 1 To 6
                    If Gdaufg(i) = 1 Then
                        nDaught = nDaught + 1
                        TableStatus.Change("GQ-DAUGHTER", nDaught, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    End If
                Next i
            End If
            If ltable.Parms.Item("OXFG") = 1 Then
                TableStatus.Change("BENTH-FLAG", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                If O.TableExists("BENTH-FLAG") Then
                    Benrfg = O.Tables.Item("BENTH-FLAG").ParmValue("BENRFG")
                Else
                    Benrfg = 0
                End If
                TableStatus.Change("SCOUR-PARMS", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                TableStatus.Change("OX-FLAGS", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                If O.TableExists("OX-FLAGS") Then
                    Reamfg = O.Tables.Item("OX-FLAGS").ParmValue("REAMFG")
                Else
                    Reamfg = 2
                End If
                If ltable.Parms.Item("HTFG") = 0 Then
                    TableStatus.Change("ELEV", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                End If
                If Lkfg = 1 Then
                    TableStatus.Change("OX-CFOREA", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                Else
                    If Reamfg = 1 Then
                        TableStatus.Change("OX-TSIVOGLOU", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                        If ltable.Parms.Item("HYDRFG") = 0 Then
                            TableStatus.Change("OX-LEN-DELTH", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusRequired)
                        End If
                    ElseIf Reamfg = 2 Then
                        TableStatus.Change("OX-TCGINV", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    ElseIf Reamfg = 3 Then
                        TableStatus.Change("OX-REAPARM", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusRequired)
                    End If
                End If
                If Benrfg = 1 Then
                    TableStatus.Change("OX-BENPARM", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                End If
                TableStatus.Change("OX-GENPARM", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusRequired)
                TableStatus.Change("OX-INIT", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
            End If
            If ltable.Parms.Item("NUTFG") = 1 Then
                If ltable.Parms.Item("PLKFG") = 1 Then
                    TableStatus.Change("NUT-FLAGS", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusRequired)
                Else
                    TableStatus.Change("NUT-FLAGS", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                End If
                If O.TableExists("NUT-FLAGS") Then
                    With O.Tables.Item("NUT-FLAGS")
                        Tamfg = .ParmValue("NH3FG")
                        Amvfg = .ParmValue("AMVFG")
                        Phflag = .ParmValue("PHFLAG")
                        Adnhfg = .ParmValue("ADNHFG")
                        Po4fg = .ParmValue("PO4FG")
                        Adpofg = .ParmValue("ADPOFG")
                    End With
                Else 'defaults
                    Tamfg = 0
                    Amvfg = 0
                    Phflag = 2
                    Adnhfg = 0
                    Po4fg = 0
                    Adpofg = 0
                End If
                TableStatus.Change("NUT-AD-FLAGS", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                TableStatus.Change("CONV-VAL1", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                If Benrfg = 1 Then
                    TableStatus.Change("NUT-BENPARM", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                End If
                TableStatus.Change("NUT-NITDENIT", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusRequired)
                If Tamfg = 1 And Amvfg = 1 Then
                    TableStatus.Change("NUT-NH3VOLAT", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                End If
                If Tamfg = 1 And Phflag = 3 Then
                    TableStatus.Change("NUT-PHVAL", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                End If
                If (Tamfg = 1 And Adnhfg = 1) Or (Po4fg = 1 And Adpofg = 1) Then
                    TableStatus.Change("NUT-BEDCONC", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TableStatus.Change("NUT-ADSPARM", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TableStatus.Change("NUT-ADSINIT", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                End If
                TableStatus.Change("NUT-DINIT", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
            End If
            If ltable.Parms.Item("PLKFG") = 1 Then
                TableStatus.Change("PLNK-FLAGS", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusRequired)
                If O.TableExists("PLNK-FLAGS") Then
                    With O.Tables.Item("PLNK-FLAGS")
                        Phyfg = .ParmValue("PHYFG")
                        Zoofg = .ParmValue("ZOOFG")
                        Balfg = .ParmValue("BALFG")
                    End With
                Else
                    Phyfg = 0
                    Zoofg = 0
                    Balfg = 0
                End If
                TableStatus.Change("PLNK-AD-FLAGS", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                If ltable.Parms.Item("HTFG") = 0 Then
                    TableStatus.Change("SURF-EXPOSED", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    'changed from required to optional - default is 1  THJ
                End If
                TableStatus.Change("NUT-BENPARM", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                'No, Really!!!!  I couldn't believe it!   THJ
                TableStatus.Change("PLNK-PARM1", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusRequired)
                TableStatus.Change("PLNK-PARM2", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                TableStatus.Change("PLNK-PARM3", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                If Phyfg = 1 Then
                    TableStatus.Change("PHYTO-PARM", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusRequired)
                    If Zoofg = 1 Then
                        TableStatus.Change("ZOO-PARM1", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusRequired)
                        TableStatus.Change("ZOO-PARM2", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    End If
                End If
                If Balfg = 1 Then
                    TableStatus.Change("BENAL-PARM", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                End If
                TableStatus.Change("PLNK-INIT", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
            End If
			If ltable.Parms.Item("PHFG") = 1 Or ltable.Parms.Item("PHFG") = 3 Then
                TableStatus.Change("PH-PARM1", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                TableStatus.Change("PH-PARM2", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                TableStatus.Change("PH-INIT", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
            End If
			
			lAcidph = False
			'check to see if acidph is available
            For i = 1 To O.Uci.Msg.BlockDefs("RCHRES").SectionDefs.Count
                If O.Uci.Msg.BlockDefs("RCHRES").SectionDefs(i).Name = "ACIDPH" Then
                    lAcidph = True
                End If
            Next i
			If lAcidph Then
				If ltable.Parms.Item("PHFG") = 2 Or ltable.Parms.Item("PHFG") = 3 Then
                    'acidph is on
                    TableStatus.Change("ACID-FLAGS", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TableStatus.Change("ACID-PARMS", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TableStatus.Change("ACID-INIT", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                End If
			End If
			
		End If
	End Sub
End Module