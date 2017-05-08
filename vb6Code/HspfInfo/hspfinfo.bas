Attribute VB_Name = "HSPFmsg"

  Sub Main()
  
    Dim hmsg$, i&, hwdm$, hscn$, dbname$, hdle&
    Dim s As String * 80
    Dim Init&, id&, kwd$, kflg&, contfg&, retid&, opnid&, offset&
    Dim tabno&, uunits&
    Dim lnflds&, lscol&(30), lflen&(30), lftyp$, lapos&(30)
    Dim limin&(30), limax&(30), lidef&(30)
    Dim lrmin!(30), lrmax!(30), lrdef!(30)
    Dim limetmin&(30), limetmax&(30), limetdef&(30)
    Dim lrmetmin!(30), lrmetmax!(30), lrmetdef!(30)
    Dim lnmhdr&, hdrbuf$(10), lfdnam$(30)
    Dim ParmTableID&, ParmTableType&
    Dim snam$, Assoc$, AssocID&, crit$
    Dim gnum&, initfg&, olen&, cont&, obuff$
    Dim Occur&, Name$, AppearName$, IDVarName$, SubsKeyName$, OName$
    Dim SCLU&, SGRP&, opcnt&, gptr&, fptr&(64), lheader$
    Dim tabnam$(), omcode&(), tabcnt&, tabret&, adjLen&, isect&, irept&
    Dim groupbase&(), ngroup, ilen&, itype&, desc$, rmin!, rmax!, rdef!
    Dim hlen&, hrec&, hpos&, svalid$, vlen&
    Dim myQRS As Recordset, BlockID&
    Dim lnmhdrM&, hdrbufM$(10)

    Call F90_W99OPN  'open error file
    Call F90_WDBFIN  'initialize WDM record buffer
    Call F90_PUTOLV(10)
    ChDir ("c:\vb6\hspfinfo")
    
    hmsg = "c:\lib3.0\hspfmsg.wdm"
    i = 1
    fmsg = F90_WDBOPN(i, hmsg, Len(hmsg))
    Call F90_MSGUNIT(fmsg)
    
    'build the access database
    Call BuildHspMsgDB
    Set myRec = myDb.OpenRecordset("BlockDefns", dbOpenDynaset)

    'pull info from hspfmsg.wdm and populate access database
    initfg = 1
    cont = 1
    SCLU = 201
    SGRP = 22
    opcnt = 0
    tabcnt = 0
    Do While cont <> 0
      'get each block and its id number
      retid = 0
      olen = 80
      Call F90_WMSGTT(fmsg, SCLU, SGRP, initfg, olen, cont, obuff)
      tabcnt = tabcnt + 1
      ReDim Preserve tabnam(tabcnt)
      ReDim Preserve omcode(tabcnt)
      tabnam(tabcnt - 1) = Mid(obuff, 1, 12)
      omcode(tabcnt - 1) = CLng(Mid(obuff, 18, 3))
      If omcode(tabcnt - 1) = 100 Then
        opcnt = opcnt + 1
        omcode(tabcnt - 1) = 120 + opcnt
      End If
      With myRec
        'add each block name to the block definition table
        .AddNew
        !Name = Trim(tabnam(tabcnt - 1))
        !id = omcode(tabcnt - 1)
        .Update
      End With
      initfg = 0
    Loop
    
    'fill table of perlnd, implnd, rchres sections
    Set myRec = myDb.OpenRecordset("SectionDefns", dbOpenDynaset)
    For i = 1 To 3
      initfg = 1
      cont = 1
      SCLU = 120 + i
      SGRP = 2
      Do While cont <> 0
        'get each section and its id number
        retid = 0
        olen = 80
        Call F90_WMSGTT(fmsg, SCLU, SGRP, initfg, olen, cont, obuff)
        With myRec
          'add each block name to the block definition table
          'If Trim(Mid(obuff, 1, 6)) <> "ACIDPH" Then
            .AddNew
            !Name = Trim(Mid(obuff, 1, 8))
            !BlockID = SCLU
            !id = (100 * i) + CLng(Mid(obuff, 10, 3))
            .Update
          'End If
        End With
        initfg = 0
      Loop
    Next i
    For i = 1 To tabcnt
      'add dummy sections for blocks without sections
      If omcode(i - 1) < 121 Or omcode(i - 1) > 123 Then
        With myRec
          'add each block name to the block definition table
          .AddNew
          !Name = "<NONE>"
          !BlockID = omcode(i - 1)
          !id = omcode(i - 1)
          .Update
        End With
      End If
    Next i
    
    Set mytableRec = myDb.OpenRecordset("TableDefns", dbOpenDynaset)
    Set myparmrec = myDb.OpenRecordset("ParmDefns", dbOpenDynaset)
    uunits = 1
    For i = 1 To tabcnt
      'loop through each block
      retid = 0
      tabno = 1
      If omcode(i - 1) > 2 Then
        'cant do for global block, get details about all others
        If omcode(i - 1) < 100 Then
          'uci block (files,ext targs, etc)
          Call F90_XTINFO(omcode(i - 1), tabno, 2, 0, _
              lnflds, lscol, lflen, lftyp, lapos, _
              limetmin, limetmax, limetdef, lrmetmin, lrmetmax, lrmetdef, _
              lnmhdr, hdrbuf, lfdnam, isect, irept, retid)
          Call F90_XTINFO(omcode(i - 1), tabno, uunits, 0, _
              lnflds, lscol, lflen, lftyp, lapos, _
              limin, limax, lidef, lrmin, lrmax, lrdef, _
              lnmhdr, hdrbuf, lfdnam, isect, irept, retid)
          Call F90_WMSGTW(CLng(1), Assoc)
          Call F90_WMSGTH(gptr, fptr(0))
          If retid = 0 Then
            'got some info about this block
            With mytableRec
              'write to table definition table
              .AddNew
              '!Name = Trim(tabnam(i - 1))
              !Name = "<NONE>"
              !id = .RecordCount + 1
              !sectionid = omcode(i - 1)
              !numoccur = irept
              !SGRP = 1
              lheader = addComment(RTrim(hdrbuf(0)), 0)
              For j = 2 To lnmhdr
                lheader = lheader & vbCrLf & addComment(RTrim(hdrbuf(j - 1)), 0)
              Next j
              !headerE = lheader
              !headerM = lheader
              !occurgroupid = 0
              .Update
            End With
            With myparmrec
              'write to parameter definition table
              For j = 1 To lnflds
                'loop through parameter fields
                If Len(Trim(lfdnam(j - 1))) > 0 Then
                  'dont add fields without field names
                  .AddNew
                  !Name = Trim(lfdnam(j - 1))
                  !id = .RecordCount + 1
                  !TableId = mytableRec.RecordCount
                  !Type = Mid(lftyp, j, 1)
                  !StartCol = lscol(j - 1) - 3
                  !length = lflen(j - 1)
                  If !Type = "I" Then
                    !minimum = limin(lapos(j - 1) - 1)
                    !maximum = limax(lapos(j - 1) - 1)
                    !Default = lidef(lapos(j - 1) - 1)
                    !metricminimum = limetmin(lapos(j - 1) - 1)
                    !metricmaximum = limetmax(lapos(j - 1) - 1)
                    !metricDefault = limetdef(lapos(j - 1) - 1)
                  ElseIf !Type = "R" Then
                    !minimum = lrmin(lapos(j - 1) - 1)
                    !maximum = lrmax(lapos(j - 1) - 1)
                    !Default = lrdef(lapos(j - 1) - 1)
                    !metricminimum = lrmetmin(lapos(j - 1) - 1)
                    !metricmaximum = lrmetmax(lapos(j - 1) - 1)
                    !metricDefault = lrmetdef(lapos(j - 1) - 1)
                  End If
                  .Update
                End If
              Next j
            End With
          End If
        Else
          'this is an operation type table
          Init = 1
          Do While retid > -1
            'loop through each operation table (perlnd, mutsin, etc)
            Call F90_GTNXKW(Init, omcode(i - 1), kwd, kflg, contfg, tabret)
            Init = 0
            'do for metric table, then english
            Call F90_XTINFO(omcode(i - 1), tabno, 2, 0, _
              lnflds, lscol, lflen, lftyp, lapos, _
              limetmin, limetmax, limetdef, lrmetmin, lrmetmax, lrmetdef, _
              lnmhdrM, hdrbufM, lfdnam, isect, irept, retid)
            Call F90_XTINFO(omcode(i - 1), tabno, uunits, 0, _
              lnflds, lscol, lflen, lftyp, lapos, _
              limin, limax, lidef, lrmin, lrmax, lrdef, _
              lnmhdr, hdrbuf, lfdnam, isect, irept, retid)
            Call F90_WMSGTW(CLng(1), Assoc)
            If retid = 0 Then
              'got a table
              If lflen(0) = 8 Then
                'need to add 2 characters to starting pos
                adjLen = 2
              Else
                adjLen = 0
              End If
              
              'If Mid(kwd, 1, 5) <> "ACID-" Then
                'need to update some table names that were truncated
                kwd = AddChar2Keyword(kwd)
                
                With mytableRec
                  'add info about this table to table definitions table
                  .AddNew
                  !Name = Trim(kwd)
                  !id = .RecordCount + 1
                  If isect = 0 Then
                    !sectionid = omcode(i - 1)
                    !occurgroupid = 0
                  ElseIf isect > 20 Then
                    'a group of repeating tables is denoted by this
                    'strip off the last digit of isect
                    !sectionid = (100 * (omcode(i - 1) - 120)) + Int((isect / 10))
                    !occurgroupid = isect - (Int((isect / 10)) * 10)
                  Else
                    !sectionid = (100 * (omcode(i - 1) - 120)) + isect
                    !occurgroupid = 0
                  End If
                  !numoccur = irept
                  !SGRP = tabno
                  lheader = addComment(RTrim(hdrbuf(0)), adjLen)
                  For j = 2 To lnmhdr
                    lheader = lheader & vbCrLf & addComment(RTrim(hdrbuf(j - 1)), adjLen)
                  Next j
                  !headerE = lheader
                  lheader = addComment(RTrim(hdrbufM(0)), adjLen)
                  For j = 2 To lnmhdrM
                    lheader = lheader & vbCrLf & addComment(RTrim(hdrbufM(j - 1)), adjLen)
                  Next j
                  !headerM = lheader
                  .Update
                End With
                With myparmrec
                  'add info about the fields of this table to the
                  'parameter definition table
                  For j = 2 To lnflds
                    'loop through fields
                    If Len(Trim(lfdnam(j - 1))) > 0 Then
                      'dont add fields without field names
                      .AddNew
                      !Name = Trim(lfdnam(j - 1))
                      !id = .RecordCount + 1
                      !TableId = mytableRec.RecordCount
                      !Type = Mid(lftyp, j, 1)
                      !StartCol = lscol(j - 1) + adjLen
                      !length = lflen(j - 1)
                      If !Type = "I" Then
                        !minimum = limin(lapos(j - 1) - 1)
                        !maximum = limax(lapos(j - 1) - 1)
                        !Default = lidef(lapos(j - 1) - 1)
                        !metricminimum = limetmin(lapos(j - 1) - 1)
                        !metricmaximum = limetmax(lapos(j - 1) - 1)
                        !metricDefault = limetdef(lapos(j - 1) - 1)
                      ElseIf !Type = "R" Then
                        !minimum = lrmin(lapos(j - 1) - 1)
                        !maximum = lrmax(lapos(j - 1) - 1)
                        !Default = lrdef(lapos(j - 1) - 1)
                        !metricminimum = lrmetmin(lapos(j - 1) - 1)
                        !metricmaximum = lrmetmax(lapos(j - 1) - 1)
                        !metricDefault = lrmetdef(lapos(j - 1) - 1)
                      ElseIf !Type = "C" Then
                        'special case for some 4character category parms
                        If lflen(j - 1) = 4 Then
                          If (Left(Trim(lfdnam(j - 1)), 4) = "CTAG" Or Left(Trim(lfdnam(j - 1)), 5) = "CFVOL" Or Trim(lfdnam(j - 1)) = "CEVAP" _
                            Or Trim(lfdnam(j - 1)) = "CPREC" Or Trim(lfdnam(j - 1)) = "ICAT") Then
                            !length = 2
                            !StartCol = lscol(j - 1) + adjLen + 2
                          End If
                        End If
                      End If
                      .Update
                    End If
                  Next j
                End With
              'End If
            End If
            tabno = tabno + 1
          Loop
        End If
      End If
    Next i
    
    'get help information for each table
    Set mytableRec = myDb.OpenRecordset("TableDefns", dbOpenDynaset)
    Set myQRS = myDb.OpenRecordset("BlockIDFromTableID", dbOpenDynaset)
    Set myparmrec = myDb.OpenRecordset("ParmDefns", dbOpenDynaset)
    With mytableRec
      .MoveLast
      j = .RecordCount
      .MoveFirst
      On Error Resume Next
      myparmrec.MoveFirst
      For i = 1 To j
        myQRS.MoveFirst
        myQRS.FindFirst ("TableDefns.ID = " & i)
        BlockID = myQRS("BlockDefns.ID")
        Call F90_XTINFO(BlockID, !SGRP, uunits, 0, _
              lnflds, lscol, lflen, lftyp, lapos, _
              limin, limax, lidef, lrmin, lrmax, lrdef, _
              lnmhdr, hdrbuf, lfdnam, isect, irept, retid)
        initfg = 1
        olen = 80
        cont = 1
        obuff = ""
        tempbuff = ""
        SCLU = -1
        Call F90_WMSGTH(gptr, fptr(0))
        Do While cont = 1
          If gptr > 0 Then
            olen = 80
            Call F90_WMSGTT(fmsg, SCLU, gptr, initfg, olen, cont, obuff)
            If Len(tempbuff) = 0 Then
              tempbuff = Trim(obuff)
            Else
              tempbuff = tempbuff & " " & Trim(obuff)
            End If
            SCLU = 0
          Else
            cont = 0
          End If
        Loop
        .Edit
        !help = Trim(tempbuff)
        .Update
        .MoveNext
        'now fill parameter help
        istart = 2
        If BlockID = 4 Then
          'special case for ftables
          istart = 1
        End If
        For k = istart To lnflds
          If Len(Trim(lfdnam(k - 1))) > 0 Then
            'dont add help for fields without field names
            If fptr(k - 1) > 0 Then
              initfg = 1
              olen = 80
              cont = 1
              obuff = ""
              tempbuff = ""
              SCLU = -1
              Do While cont = 1
                olen = 80
                Call F90_WMSGTT(fmsg, SCLU, fptr(k - 1), initfg, olen, cont, obuff)
                If Len(tempbuff) = 0 Then
                  tempbuff = Trim(obuff)
                Else
                  tempbuff = tempbuff & " " & Trim(obuff)
                End If
                SCLU = 0
              Loop
              myparmrec.Edit
              myparmrec!help = Trim(tempbuff)
              myparmrec.Update
            End If
            myparmrec.MoveNext
          End If
        Next k
      Next i
    End With
    
    'fill table of timeseries groups/member names for each operation
    For i = 1 To tabcnt
      Set myRec = myDb.OpenRecordset("TSGroupDefns", dbOpenDynaset)
      If omcode(i - 1) > 100 Then
        initfg = 1
        cont = 1
        SGRP = 1
        igroup = 0
        Do While cont <> 0
          'get each group name
          retid = 0
          olen = 10
          Call F90_WMSGTT(fmsg, omcode(i - 1) + 20, SGRP, initfg, olen, cont, obuff)
          With myRec
            'add each group name to the group definition table
            .AddNew
            !Name = Trim(Mid(obuff, 1, 6))
            !BlockID = omcode(i - 1)
            igroup = igroup + 1
            !id = ((omcode(i - 1) - 120) * 100) + igroup
            .Update
          End With
          'save base number for associated sgrp
          ReDim Preserve groupbase(igroup)
          groupbase(igroup - 1) = CInt(Mid(obuff, 8, 3))
          initfg = 0
        Loop
        'now populate member names
        Set myRec = myDb.OpenRecordset("TSMemberDefns", dbOpenDynaset)
        For j = 1 To igroup
          initfg = 1
          cont = 1
          SGRP = 1 + j
          ngroup = 1
          Do While cont <> 0
            'get each member name
            retid = 0
            olen = 8
            Call F90_WMSGTT(fmsg, omcode(i - 1) + 20, SGRP, initfg, olen, cont, obuff)
            With myRec
              'add each member name to the member definition table
              .AddNew
              !Name = Trim(Mid(obuff, 1, 6))
              !TSGroupID = ((omcode(i - 1) - 120) * 100) + j
              !id = .RecordCount + 1
              !SCLU = omcode(i - 1) + 20
              !SGRP = groupbase(j - 1) + ngroup
              .Update
            End With
            ngroup = ngroup + 1
            initfg = 0
          Loop
        Next j
      End If
    Next i
    'now populate member details
    Set myRec = myDb.OpenRecordset("TSMemberDefns", dbOpenDynaset)
    With myRec
      .MoveLast
      j = .RecordCount
      .MoveFirst
      On Error Resume Next
      For i = 1 To j
        'get first line of details
        initfg = 1
        olen = 80
        cont = 1
        obuff = ""
        Call F90_WMSGTT(fmsg, !SCLU, !SGRP, initfg, olen, cont, obuff)
        .Edit
        !mdim1 = CInt(Mid(obuff, 7, 3))
        !mdim2 = CInt(Mid(obuff, 10, 3))
        !maxsb1 = CInt(Mid(obuff, 13, 6))
        !maxsb2 = CInt(Mid(obuff, 19, 6))
        !mkind = CInt(Mid(obuff, 25, 2))
        !sptrn = CInt(Mid(obuff, 27, 2))
        !msect = CInt(Mid(obuff, 29, 2))
        !mio = CInt(Mid(obuff, 31, 2))
        !osvbas = CInt(Mid(obuff, 33, 5))
        !osvoff = CInt(Mid(obuff, 38, 6))
        If Len(Trim(Mid(obuff, 49, 8))) > 0 Then
          !eunits = Trim(Mid(obuff, 49, 8))
        Else
          !eunits = " "
        End If
        !ltval1 = CSng(Mid(obuff, 57, 4))
        !ltval2 = CSng(Mid(obuff, 61, 8))
        !ltval3 = CSng(Mid(obuff, 69, 4))
        !ltval4 = CSng(Mid(obuff, 73, 8))
        'now get second line of details
        initfg = 0
        olen = 80
        Call F90_WMSGTT(fmsg, !SCLU, !SGRP, initfg, olen, cont, obuff)
        !defn = Trim(Mid(obuff, 1, 43))
        If Len(Trim(Mid(obuff, 49, 8))) > 0 Then
          !munits = Trim(Mid(obuff, 49, 8))
        Else
          !munits = " "
        End If
        !ltval5 = CSng(Mid(obuff, 57, 4))
        !ltval6 = CSng(Mid(obuff, 61, 8))
        !ltval7 = CSng(Mid(obuff, 69, 4))
        !ltval8 = CSng(Mid(obuff, 73, 8))
        .Update
        .MoveNext
      Next i
    End With
    
    'build table of table aliases
    Set myTableAliasDefn = myDb.OpenRecordset("TableAliasDefn", dbOpenDynaset)
    With myTableAliasDefn
      gnum = 10
      SCLU = 120
      Do While SCLU < 123 'more operation types
        SCLU = SCLU + 1
        initfg = 1
        cont = 1
        opnid = SCLU - 120
        OName = ""
        Do While cont <> 0
          olen = 80
          Call F90_WMSGTT(fmsg, SCLU, gnum, initfg, olen, cont, obuff)
          initfg = 0
          .AddNew
          !OpnTypID = opnid
          !Name = Left(obuff, 12)
          If OName <> !Name Then
            Occur = 1
            OName = !Name
          Else
            Occur = Occur + 1
          End If
          !Occur = Occur
          If Len(obuff) > 14 Then
            !AppearName = Mid(obuff, 15, 20)
          Else
            !AppearName = " "
          End If
          If Len(obuff) > 36 Then
            !IDVarName = Mid(obuff, 37, 6)
            crit = "Name = '" & !IDVarName & "'"
            myRec.FindFirst crit
            !IDVar = myRec!id
          Else
            !IDVarName = " "
          End If
          If Len(obuff) > 44 Then
            !SubsKeyName = Mid(obuff, 45, 6)
            crit = "Name = '" & !SubsKeyName & "'"
            myRec.FindFirst crit
            !IDSubs = myRec!id
          Else
            !SubsKeyName = " "
          End If
          .Update
        Loop
      Loop
    End With
    myTableAliasDefn.Close
    
    'build table of wdm attributes
    On Error GoTo 0
    Set myRec = myDb.OpenRecordset("WDMAttribDefns", dbOpenDynaset)
      id = 1
      Do While id < 500
        Call F90_WDSAGY(fmsg, id, ilen, itype, rmin, rmax, rdef, hlen, hrec, hpos, vlen, Name, desc, svalid)
        'add info about this attrib to attrib definitions table
        With myRec
          If Len(Name) > o Then
            .AddNew
            !Name = Trim(Name)
            !id = id
            !length = ilen
            !Type = itype
            !desc = Trim(desc)
            !Min = rmin
            !Max = rmax
            !Def = rdef
            If vlen > 0 Then
              !valid = svalid
            End If
            .Update
          End If
        End With
        id = id + 1
      Loop
    
End Sub

Private Function addComment(s$, adjLen&) As String
  Dim i&, t$
  If adjLen > 0 Then 'adjust length for old aide 78 char problem - add 2 blanks at 1 and 6
    s = " " & Left(s, 4) & " " & Right(s, Len(s) - 4)
  End If
  If Not (InStr(s, "***")) Then 'not a comment
    i = InStr(s, "   ")
    If i > 0 Then 'replace first three blanks
      t = Left(s, i - 1) & "***" & Right(s, Len(s) - i - 2)
      s = t
    ElseIf Len(s) < 78 Then 'at end
      s = s & "***"
    Else
      s = Left(s, 77) & "***"
    End If
  End If
  addComment = s
End Function
