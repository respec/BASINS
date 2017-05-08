Attribute VB_Name = "modStatusRchres"
Option Explicit
'Copyright 2002 AQUA TERRA Consultants - Royalty-free use permitted under open source license

Public Sub UpdateRchres(O As HspfOperation, TableStatus As HspfStatus)
  Dim ltable As HspfTable
  Dim i&, j&, t$, f$
  'added j   THJ
  Dim nCons&, Bedflg&, Tgflg&
  Dim nGqual&, mWatemp&, mPhval&, mRoxygen&, mCloud&, mSedconc&, mPhoto&, mBio&
  'added mBio flag   THJ
  Dim Hydrolysis As Boolean, Oxidation As Boolean, Photolysis As Boolean
  Dim Biodegradation As Boolean, Volatilization As Boolean
  'added Biodegradation flag  THJ
  Dim nHydro&, nOxid&, nPhot&, nVolat&, nBiod&, nBioM&, nGenDec&, nSedAs&
  'added gqual process counters - these are SUB1-SUB7 in PGQUAL   THJ
  Dim Gdaufg&(6), Daufg&(6), nDaught&
  'added gqual daughter product flags
  Dim Benrfg&, Lkfg&, Reamfg&
  Dim Tamfg&, Amvfg&, Phflag&, Adnhfg&, Po4fg&, Adpofg&
  Dim Phyfg&, Zoofg&, Balfg&
  
  'always can be present
  TableStatus.Change "ACTIVITY", 1, HspfStatusRequired
  TableStatus.Change "PRINT-INFO", 1, HspfStatusOptional
  TableStatus.Change "GEN-INFO", 1, HspfStatusRequired
  
  If O.TableExists("GEN-INFO") Then
    Lkfg = O.Tables("GEN-INFO").Parms("LKFG")
  Else
    Lkfg = 0
  End If

  If O.TableExists("ACTIVITY") Then
    'moved Benrfg to OXRX  THJ
    Set ltable = O.Tables("ACTIVITY")
    If ltable.Parms("HYDRFG") = 1 Then
      TableStatus.Change "HYDR-PARM1", 1, HspfStatusOptional
      If O.TableExists("HYDR-PARM1") Then
        If O.Tables("HYDR-PARM1").Parms("VCONFG") = 1 Then
          TableStatus.Change "MON-CONVF", 1, HspfStatusOptional
        End If
      End If
      TableStatus.Change "HYDR-PARM2", 1, HspfStatusRequired
      TableStatus.Change "HYDR-IRRIG", 1, HspfStatusOptional
      'added HYDR-IRRIG   THJ
      TableStatus.Change "HYDR-INIT", 1, HspfStatusOptional
      'must pass NCAT from CATEGORY to NCATS in HYDR to be able to
      'process categories.
    End If
    If ltable.Parms("ADFG") = 1 Then
      TableStatus.Change "ADCALC-DATA", 1, HspfStatusOptional
    End If
    If ltable.Parms("CONSFG") = 1 Then
      TableStatus.Change "NCONS", 1, HspfStatusOptional
      TableStatus.Change "CONS-AD-FLAG", 1, HspfStatusOptional
      If O.TableExists("NCONS") Then
        nCons = O.Tables("NCONS").Parms("NCONS")
      Else
        nCons = 1
      End If
      For i = 1 To nCons
        TableStatus.Change "CONS-DATA", i, HspfStatusRequired
      Next i
    End If
    If ltable.Parms("HTFG") = 1 Then
      TableStatus.Change "HT-BED-FLAGS", 1, HspfStatusOptional
      TableStatus.Change "HEAT-PARM", 1, HspfStatusOptional
      If O.TableExists("HT-BED-FLAGS") Then
        Bedflg = O.Tables("HT-BED-FLAGS").Parms("BEDFLG")
        Tgflg = O.Tables("HT-BED-FLAGS").Parms("TGFLG")
        If Bedflg = 1 Or Bedflg = 2 Then
          TableStatus.Change "HT-BED-PARM", 1, HspfStatusOptional
          If Tgflg = 3 Then
            TableStatus.Change "MON-HT-TGRND", 1, HspfStatusOptional
          End If
        ElseIf Bedflg = 3 Then
          TableStatus.Change "HT-BED-DELH", 1, HspfStatusOptional
          TableStatus.Change "HT-BED-DELTT", 1, HspfStatusOptional
        End If
      End If
      TableStatus.Change "HEAT-INIT", 1, HspfStatusOptional
    End If
    If ltable.Parms("SEDFG") = 1 Then
      TableStatus.Change "SANDFG", 1, HspfStatusOptional
      TableStatus.Change "SED-GENPARM", 1, HspfStatusRequired
      If ltable.Parms("HYDRFG") = 0 Then
        TableStatus.Change "SED-HYDPARM", 1, HspfStatusRequired
      End If
      TableStatus.Change "SAND-PM", 1, HspfStatusRequired
      TableStatus.Change "SILT-CLAY-PM", 1, HspfStatusRequired
      TableStatus.Change "SILT-CLAY-PM", 2, HspfStatusRequired
      'added second occurrence  Technically, these tables are defaultable,
      'but not realistically.  I'd just as soon leave them Required  THJ
      TableStatus.Change "SSED-INIT", 1, HspfStatusOptional
      TableStatus.Change "BED-INIT", 1, HspfStatusOptional
    End If
    If ltable.Parms("GQALFG") = 1 Then
      TableStatus.Change "GQ-GENDATA", 1, HspfStatusOptional
      TableStatus.Change "GQ-AD-FLAGS", 1, HspfStatusOptional
      If O.TableExists("GQ-GENDATA") Then
        nGqual = O.Tables("GQ-GENDATA").Parms("NGQUAL")
        mWatemp = O.Tables("GQ-GENDATA").Parms("TEMPFG")
        mPhval = O.Tables("GQ-GENDATA").Parms("PHFLAG")
        mRoxygen = O.Tables("GQ-GENDATA").Parms("ROXFG")
        mCloud = O.Tables("GQ-GENDATA").Parms("CLDFG")
        mSedconc = O.Tables("GQ-GENDATA").Parms("SDFG")
        mPhoto = O.Tables("GQ-GENDATA").Parms("PHYTFG")
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
        TableStatus.Change "GQ-QALDATA", i, HspfStatusRequired
        TableStatus.Change "GQ-QALFG", i, HspfStatusOptional
        TableStatus.Change "GQ-FLG2", i, HspfStatusOptional
        f = "GQ-FLG2"
        If i > 1 Then f = f & ":" & i
        'first parse flg2 to get dauther relationships and bio
        If O.TableExists(f) Then
          Daufg(1) = O.Tables(f).Parms("GQPM21")
          Daufg(2) = O.Tables(f).Parms("GQPM22")
          Daufg(3) = O.Tables(f).Parms("GQPM23")
          Daufg(4) = O.Tables(f).Parms("GQPM24")
          Daufg(5) = O.Tables(f).Parms("GQPM25")
          Daufg(6) = O.Tables(f).Parms("GQPM26")
          For j = 1 To 5
            If Daufg(j) = 1 Then
              Gdaufg(j) = 1
            End If
          Next j
          mBio = O.Tables(f).Parms("GQPM27")
        Else
          mBio = 2
        End If
        'now can parse qalfg  THJ
        t = "GQ-QALFG"
        If i > 1 Then t = t & ":" & i
        If O.TableExists(t) Then
          If O.Tables(t).Parms("QALFG1") = 1 Then
            nHydro = nHydro + 1
            TableStatus.Change "GQ-HYDPM", nHydro, HspfStatusRequired
            Hydrolysis = True
          End If
          If O.Tables(t).Parms("QALFG2") = 1 Then
            nOxid = nOxid + 1
            TableStatus.Change "GQ-ROXPM", nOxid, HspfStatusRequired
            Oxidation = True
          End If
          If O.Tables(t).Parms("QALFG3") = 1 Then
            nPhot = nPhot + 1
            TableStatus.Change "GQ-PHOTPM", nPhot, HspfStatusOptional
            Photolysis = True
          End If
          If O.Tables(t).Parms("QALFG4") = 1 Then
            nVolat = nVolat + 1
            TableStatus.Change "GQ-CFGAS", nVolat, HspfStatusRequired
            Volatilization = True
          End If
          If O.Tables(t).Parms("QALFG5") = 1 Then
            nBiod = nBiod + 1
            TableStatus.Change "GQ-BIOPM", nBiod, HspfStatusRequired
            Biodegradation = True
            If mBio = 3 Then
              nBioM = nBioM + 1
              TableStatus.Change "MON-BIO", nBioM, HspfStatusRequired
            End If
          End If
          If O.Tables(t).Parms("QALFG6") = 1 Then
            nGenDec = nGenDec + 1
            TableStatus.Change "GQ-GENDECAY", nGenDec, HspfStatusRequired
          End If
          If O.Tables(t).Parms("QALFG7") = 1 Then
            nSedAs = nSedAs + 1
            TableStatus.Change "GQ-SEDDECAY", nSedAs, HspfStatusOptional
            TableStatus.Change "GQ-KD", nSedAs, HspfStatusRequired
            TableStatus.Change "GQ-ADRATE", nSedAs, HspfStatusRequired
            TableStatus.Change "GQ-ADTHETA", nSedAs, HspfStatusOptional
            TableStatus.Change "GQ-SEDCONC", nSedAs, HspfStatusOptional
          End If
        End If
      Next i
     
      TableStatus.Change "GQ-VALUES", 1, HspfStatusOptional
      'only one occurrence  THJ
      If mWatemp = 3 Then
        TableStatus.Change "MON-WATEMP", 1, HspfStatusOptional
      End If
      If mPhval = 3 And Hydrolysis Then
        TableStatus.Change "MON-PHVAL", 1, HspfStatusOptional
      End If
      If mRoxygen And Oxidation Then
        TableStatus.Change "MON-ROXYGEN", 1, HspfStatusOptional
        'corrected spelling of table name  THJ
      End If
      If Photolysis Then
        TableStatus.Change "GQ-ALPHA", 1, HspfStatusRequired
        TableStatus.Change "GQ-GAMMA", 1, HspfStatusOptional
        TableStatus.Change "GQ-DELTA", 1, HspfStatusOptional
        TableStatus.Change "GQ-CLDFACT", 1, HspfStatusOptional
        If mCloud Then
          TableStatus.Change "MON-CLOUD", 1, HspfStatusOptional
        End If
        If mSedconc Then
          TableStatus.Change "MON-SEDCONC", 1, HspfStatusOptional
        End If
        If mPhoto Then
          TableStatus.Change "MON-PHYTO", 1, HspfStatusOptional
        End If
        If ltable.Parms("HTFG") = 0 Then
          TableStatus.Change "SURF-EXPOSED", 1, HspfStatusOptional
        End If
      End If
      If Volatilization Then
        TableStatus.Change "OX-FLAGS", 1, HspfStatusOptional
        If O.TableExists("OX-FLAGS") Then
          Reamfg = O.Tables("OX-FLAGS").Parms("REAMFG")
        Else
          Reamfg = 2
        End If
        If ltable.Parms("HTFG") = 0 Then
          TableStatus.Change "ELEV", 1, HspfStatusOptional
          'added check on HTFG   THJ
        End If
        If Lkfg = 1 Then
          TableStatus.Change "OX-CFOREA", 1, HspfStatusOptional
          'added check on Lkfg   THJ
        Else
          If Reamfg = 1 Then
            TableStatus.Change "OX-TSIVOGLOU", 1, HspfStatusOptional
            If ltable.Parms("HYDRFG") = 0 Then
              TableStatus.Change "OX-LEN-DELTH", 1, HspfStatusRequired
            End If
          ElseIf Reamfg = 2 Then
            TableStatus.Change "OX-TCGINV", 1, HspfStatusOptional
          ElseIf Reamfg = 3 Then
            TableStatus.Change "OX-REAPARM", 1, HspfStatusRequired
          End If
        End If
      End If
      'reworked GQ-DAUGHTER   THJ
      For i = 1 To 6
        If Gdaufg(i) = 1 Then
          nDaught = nDaught + 1
          TableStatus.Change "GQ-DAUGHTER", nDaught, HspfStatusOptional
        End If
      Next i
    End If
    If ltable.Parms("OXFG") = 1 Then
      TableStatus.Change "BENTH-FLAG", 1, HspfStatusOptional
      If O.TableExists("BENTH-FLAG") Then
        Benrfg = O.Tables("BENTH-FLAG").Parms("BENRFG")
      Else
        Benrfg = 0
      End If
      TableStatus.Change "SCOUR-PARMS", 1, HspfStatusOptional
      TableStatus.Change "OX-FLAGS", 1, HspfStatusOptional
      If O.TableExists("OX-FLAGS") Then
        Reamfg = O.Tables("OX-FLAGS").Parms("REAMFG")
      Else
        Reamfg = 2
      End If
      If ltable.Parms("HTFG") = 0 Then
        TableStatus.Change "ELEV", 1, HspfStatusOptional
      End If
      If Lkfg = 1 Then
        TableStatus.Change "OX-CFOREA", 1, HspfStatusOptional
      Else
        If Reamfg = 1 Then
          TableStatus.Change "OX-TSIVOGLOU", 1, HspfStatusOptional
          If ltable.Parms("HYDRFG") = 0 Then
            TableStatus.Change "OX-LEN-DELTH", 1, HspfStatusRequired
          End If
        ElseIf Reamfg = 2 Then
          TableStatus.Change "OX-TCGINV", 1, HspfStatusOptional
        ElseIf Reamfg = 3 Then
          TableStatus.Change "OX-REAPARM", 1, HspfStatusRequired
        End If
      End If
      If Benrfg = 1 Then
        TableStatus.Change "OX-BENPARM", 1, HspfStatusOptional
      End If
      TableStatus.Change "OX-GENPARM", 1, HspfStatusRequired
      TableStatus.Change "OX-INIT", 1, HspfStatusOptional
    End If
    If ltable.Parms("NUTFG") = 1 Then
      If ltable.Parms("PLKFG") = 1 Then
        TableStatus.Change "NUT-FLAGS", 1, HspfStatusRequired
      Else
        TableStatus.Change "NUT-FLAGS", 1, HspfStatusOptional
      End If
      If O.TableExists("NUT-FLAGS") Then
        Tamfg = O.Tables("NUT-FLAGS").Parms("NH3FG")
        Amvfg = O.Tables("NUT-FLAGS").Parms("AMVFG")
        Phflag = O.Tables("NUT-FLAGS").Parms("PHFLAG")
        Adnhfg = O.Tables("NUT-FLAGS").Parms("ADNHFG")
        Po4fg = O.Tables("NUT-FLAGS").Parms("PO4FG")
        Adpofg = O.Tables("NUT-FLAGS").Parms("ADPOFG")
      Else 'defaults
        Tamfg = 0
        Amvfg = 0
        Phflag = 2
        Adnhfg = 0
        Po4fg = 0
        Adpofg = 0
      End If
      TableStatus.Change "NUT-AD-FLAGS", 1, HspfStatusOptional
      TableStatus.Change "CONV-VAL1", 1, HspfStatusOptional
      If Benrfg = 1 Then
        TableStatus.Change "NUT-BENPARM", 1, HspfStatusOptional
      End If
      TableStatus.Change "NUT-NITDENIT", 1, HspfStatusRequired
      If Tamfg = 1 And Amvfg = 1 Then
        TableStatus.Change "NUT-NH3VOLAT", 1, HspfStatusOptional
      End If
      If Tamfg = 1 And Phflag = 3 Then
        TableStatus.Change "NUT-PHVAL", 1, HspfStatusOptional
      End If
      If (Tamfg = 1 And Adnhfg = 1) Or (Po4fg = 1 And Adpofg = 1) Then
        TableStatus.Change "NUT-BEDCONC", 1, HspfStatusOptional
        TableStatus.Change "NUT-ADSPARM", 1, HspfStatusOptional
        TableStatus.Change "NUT-ADSINIT", 1, HspfStatusOptional
      End If
      TableStatus.Change "NUT-DINIT", 1, HspfStatusOptional
    End If
    If ltable.Parms("PLKFG") = 1 Then
      TableStatus.Change "PLNK-FLAGS", 1, HspfStatusRequired
      If O.TableExists("PLNK-FLAGS") Then
        Phyfg = O.Tables("PLNK-FLAGS").Parms("PHYFG")
        Zoofg = O.Tables("PLNK-FLAGS").Parms("ZOOFG")
        Balfg = O.Tables("PLNK-FLAGS").Parms("BALFG")
      Else
        Phyfg = 0
        Zoofg = 0
        Balfg = 0
      End If
      TableStatus.Change "PLNK-AD-FLAGS", 1, HspfStatusOptional
      If ltable.Parms("HTFG") = 0 Then
        TableStatus.Change "SURF-EXPOSED", 1, HspfStatusOptional
        'changed from required to optional - default is 1  THJ
      End If
      TableStatus.Change "NUT-BENPARM", 1, HspfStatusOptional
      'No, Really!!!!  I couldn't believe it!   THJ
      TableStatus.Change "PLNK-PARM1", 1, HspfStatusRequired
      TableStatus.Change "PLNK-PARM2", 1, HspfStatusOptional
      TableStatus.Change "PLNK-PARM3", 1, HspfStatusOptional
      If Phyfg = 1 Then
        TableStatus.Change "PHYTO-PARM", 1, HspfStatusRequired
        If Zoofg = 1 Then
          TableStatus.Change "ZOO-PARM1", 1, HspfStatusRequired
          TableStatus.Change "ZOO-PARM2", 1, HspfStatusOptional
        End If
      End If
      If Balfg = 1 Then
        TableStatus.Change "BENAL-PARM", 1, HspfStatusOptional
      End If
      TableStatus.Change "PLNK-INIT", 1, HspfStatusOptional
    End If
    If ltable.Parms("PHFG") = 1 Or ltable.Parms("PHFG") = 3 Then
      TableStatus.Change "PH-PARM1", 1, HspfStatusOptional
      TableStatus.Change "PH-PARM2", 1, HspfStatusOptional
      TableStatus.Change "PH-INIT", 1, HspfStatusOptional
    End If
    
    Dim lAcidph As Boolean
    lAcidph = False
    'check to see if acidph is available
    For i = 1 To O.Uci.Msg.BlockDefs("RCHRES").SectionDefs.Count
      If O.Uci.Msg.BlockDefs("RCHRES").SectionDefs(i).Name = "ACIDPH" Then
        lAcidph = True
      End If
    Next i
    If lAcidph Then
      If ltable.Parms("PHFG") = 2 Or ltable.Parms("PHFG") = 3 Then
        'acidph is on
        TableStatus.Change "ACID-FLAGS", 1, HspfStatusOptional
        TableStatus.Change "ACID-PARMS", 1, HspfStatusOptional
        TableStatus.Change "ACID-INIT", 1, HspfStatusOptional
      End If
    End If
    
  End If
End Sub
