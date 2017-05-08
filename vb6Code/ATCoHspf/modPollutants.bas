Attribute VB_Name = "modPollutants"
Option Explicit
'Copyright 2002 AQUA TERRA Consultants - Royalty-free use permitted under open source license

Public Sub modPollutantsBuild(myUci As Object, myMsg As Object)
  Dim vOpn As Variant, lOpn As HspfOperation, cMassLinks As Collection
  Dim nquals&, lML As HspfMassLink, j&, itemp&, nGqual&, i&, k&
  Dim tPoll As HspfPollutant, lTableDef As HspfTableDef, piconn&
  Dim newOpn As HspfOperation, ctype$, rdone As Boolean, tname$
  Dim lML2 As HspfMassLink, n&, pdone As Boolean, idone As Boolean
  Dim nthsed&, sedcnt&, sedfg&, isedassoc&(), psedassoc&()
 
  'figure out how many p or i quals
  nquals = 0
  For Each vOpn In myUci.OpnBlks("PERLND").Ids
    Set lOpn = vOpn
    If lOpn.TableExists("NQUALS") Then
      itemp = lOpn.Tables("NQUALS").Parms("NQUAL")
    ElseIf lOpn.TableExists("QUAL-PROPS") Then
      itemp = 1
    End If
    If itemp > nquals Then
      nquals = itemp
    End If
  Next vOpn

  For Each vOpn In myUci.OpnBlks("IMPLND").Ids
    Set lOpn = vOpn
    If lOpn.TableExists("NQUALS") Then
      itemp = lOpn.Tables("NQUALS").Parms("NQUAL")
    ElseIf lOpn.TableExists("QUAL-PROPS") Then
      itemp = 1
    End If
    If itemp > nquals Then
      nquals = itemp
    End If
  Next vOpn
  
  'figure out how many gquals
  nGqual = 0
  For Each vOpn In myUci.OpnBlks("RCHRES").Ids
    Set lOpn = vOpn
    If lOpn.TableExists("GQ-QALDATA") Then
      If lOpn.TableExists("GQ-GENDATA") Then
        itemp = lOpn.Tables("GQ-GENDATA").Parms("NGQUAL")
      Else
        itemp = 1
      End If
      If itemp > nGqual Then
        nGqual = itemp
      End If
    End If
  Next vOpn
  
  If nquals > 0 Then
    'keep track of sed assoc flags
    For j = 1 To 2
      If j = 1 Then
        ctype = "PERLND"
      Else
        ctype = "IMPLND"
      End If
      sedcnt = 0
      For i = 1 To nquals
        If i > 1 Then
          tname = "QUAL-PROPS:" & i
        Else
          tname = "QUAL-PROPS"
        End If
        If myUci.OpnBlks(ctype).TableExists(tname) Then
          sedfg = 0
          For Each vOpn In myUci.OpnBlks(ctype).Ids
            Set lOpn = vOpn
            If lOpn.TableExists(tname) Then
              sedfg = lOpn.Tables(tname).Parms("QSDFG").Value
              Exit For
            End If
          Next vOpn
          If sedfg > 0 Then
            sedcnt = sedcnt + 1
            If j = 1 Then
              ReDim Preserve psedassoc(sedcnt)
              psedassoc(sedcnt) = i
            Else
              ReDim Preserve isedassoc(sedcnt)
              isedassoc(sedcnt) = i
            End If
          End If
        End If
      Next i
    Next j
  End If
  
  If nGqual > 0 Then     'big loop for each gqual
      
    For k = 1 To nGqual
      'create new instance of a pollutant
      Set tPoll = New HspfPollutant
      pdone = False
      idone = False
      rdone = False
      
      'look through all mass-links
      j = 1
      Do While j <= myUci.MassLinks.Count
        Set lML = myUci.MassLinks(j)
        If (lML.Target.Member = "IDQAL" And lML.Target.MemSub1 = k) Or _
           (lML.Target.Member = "ISQAL" And lML.Target.MemSub2 = k) Then
           
          tPoll.MassLinks.Add lML
          myUci.MassLinks.Remove j
          
          If (lML.Source.Group = "PQUAL" Or lML.Source.Group = "IQUAL") Then
            'this is type p-i-g
            tPoll.ModelType = "PIG"
            If lML.Source.Group = "PQUAL" Then
              ctype = "PERLND"
            ElseIf lML.Source.Group = "IQUAL" Then
              ctype = "IMPLND"
            End If
            'found the p/i qual it is connected to
            If lML.Target.Member <> "ISQAL" Then
              piconn = lML.Source.MemSub1  'the normal case
            Else
              nthsed = lML.Source.MemSub1  'figure out which qual this is
              If lML.Source.Group = "PQUAL" Then
                piconn = psedassoc(nthsed)
              Else
                piconn = isedassoc(nthsed)
              End If
            End If
                
            'remove tables and add to this collection
            If (ctype = "PERLND" And Not pdone) Or _
              (ctype = "IMPLND" And Not idone) Then
              For Each vOpn In myUci.OpnBlks(ctype).Ids
                Set lOpn = vOpn
                Set newOpn = New HspfOperation
                newOpn.Id = lOpn.Id
                newOpn.Name = lOpn.Name
                tPoll.Operations.Add newOpn, ctype & newOpn.Id
                For Each lTableDef In myMsg.BlockDefs(ctype).SectionDefs(Left(ctype, 1) & "QUAL").TableDefs
                  If piconn > 1 Then
                    tname = lTableDef.Name & ":" & piconn
                  Else
                    tname = lTableDef.Name
                  End If
                  If lOpn.TableExists(tname) Then
                    tPoll.Operations(ctype & newOpn.Id).Tables.Add lOpn.Tables(tname), lTableDef.Name
                    lOpn.Tables.Remove (tname)
                  End If
                  If myUci.OpnBlks(ctype).TableExists(tname) Then
                    myUci.OpnBlks(ctype).Tables.Remove tname
                  End If
                Next lTableDef
                If Len(tPoll.Name) = 0 Then
                  'todo: make sure table exists before doing this
                  tPoll.Name = tPoll.Operations(ctype & newOpn.Id).Tables("QUAL-PROPS").Parms("QUALID").Value
                  tPoll.Index = piconn
                End If
              Next vOpn
              If ctype = "PERLND" Then
                pdone = True
              ElseIf ctype = "IMPLND" Then
                idone = True
              End If
            End If
          Else
            If Len(tPoll.ModelType) = 0 Then
              tPoll.ModelType = "GOnly"
            End If
          End If
              
          If Not rdone Then
            ctype = "RCHRES"
            For Each vOpn In myUci.OpnBlks(ctype).Ids
              Set lOpn = vOpn
              Set newOpn = New HspfOperation
              newOpn.Id = lOpn.Id
              newOpn.Name = lOpn.Name
              tPoll.Operations.Add newOpn, ctype & newOpn.Id
              For Each lTableDef In myMsg.BlockDefs(ctype).SectionDefs("GQUAL").TableDefs
                If k > 1 Then
                  tname = lTableDef.Name & ":" & k
                Else
                  tname = lTableDef.Name
                End If
                If lOpn.TableExists(tname) Then
                  tPoll.Operations(ctype & newOpn.Id).Tables.Add lOpn.Tables(tname), lTableDef.Name
                  lOpn.Tables.Remove (tname)
                  If myUci.OpnBlks(ctype).TableExists(tname) Then
                    myUci.OpnBlks(ctype).Tables.Remove tname
                  End If
                End If
              Next lTableDef
              If Len(tPoll.Name) = 0 Then
                'todo: make sure table exists before doing this
                tPoll.Name = tPoll.Operations(ctype & newOpn.Id).Tables("GQ-QALDATA").Parms("GQID").Value
                tPoll.Index = k
              End If
            Next vOpn
            rdone = True
          End If
        Else
          j = j + 1
        End If
      Loop
      
      If Len(tPoll.ModelType) > 0 Then
        If myUci.Pollutants.Count > 0 Then
          tPoll.Id = myUci.Pollutants.Count + 1
        Else
          tPoll.Id = 1
        End If
        myUci.Pollutants.Add tPoll
      End If
    Next k
    
  End If
  
  If nquals > 0 Then     'big loop for each p or i qual not to gqual
      
    For k = 1 To nquals
      'create new instance of a pollutant
      Set tPoll = New HspfPollutant
      pdone = False
      idone = False
      
      'look through all mass-links
      j = 1
      Do While j <= myUci.MassLinks.Count
        Set lML = myUci.MassLinks(j)
        If ((lML.Source.Group = "PQUAL" And lML.Source.MemSub1 = k) Or _
           (lML.Source.Group = "IQUAL" And lML.Source.MemSub1 = k)) And _
           lML.Target.VolName = "RCHRES" Then
           
          tPoll.MassLinks.Add lML
          myUci.MassLinks.Remove j
          
          tPoll.ModelType = "PIOnly"
          If lML.Source.Group = "PQUAL" Then
            ctype = "PERLND"
          ElseIf lML.Source.Group = "IQUAL" Then
            ctype = "IMPLND"
          End If
              
          'remove tables and add to this collection
          If (ctype = "PERLND" And Not pdone) Or _
            (ctype = "IMPLND" And Not idone) Then
            For Each vOpn In myUci.OpnBlks(ctype).Ids
              Set lOpn = vOpn
              Set newOpn = New HspfOperation
              newOpn.Id = lOpn.Id
              newOpn.Name = lOpn.Name
              tPoll.Operations.Add newOpn, ctype & newOpn.Id
              For Each lTableDef In myMsg.BlockDefs(ctype).SectionDefs(Left(ctype, 1) & "QUAL").TableDefs
                If k > 1 Then
                  tname = lTableDef.Name & ":" & k
                Else
                  tname = lTableDef.Name
                End If
                If lOpn.TableExists(tname) Then
                  tPoll.Operations(ctype & newOpn.Id).Tables.Add lOpn.Tables(tname), lTableDef.Name
                  lOpn.Tables.Remove (tname)
                End If
                If myUci.OpnBlks(ctype).TableExists(tname) Then
                  myUci.OpnBlks(ctype).Tables.Remove tname
                End If
              Next lTableDef
              If Len(tPoll.Name) = 0 Then
                tPoll.Name = tPoll.Operations(ctype & newOpn.Id).Tables("QUAL-PROPS").Parms("QUALID").Value
                tPoll.Index = k
              End If
            Next vOpn
            If ctype = "PERLND" Then
              pdone = True
            ElseIf ctype = "IMPLND" Then
              idone = True
            End If
          End If
        Else
          j = j + 1
        End If
      Loop
      
      If Len(tPoll.ModelType) > 0 Then
        If myUci.Pollutants.Count > 0 Then
          tPoll.Id = myUci.Pollutants.Count + 1
        Else
          tPoll.Id = 1
        End If
        myUci.Pollutants.Add tPoll
      End If
    Next k
    
  End If
  
End Sub

Public Sub modPollutantsUnBuild(myUci As Object, myMsg As Object)
  Dim vOpn As Variant, lOpn As HspfOperation, cMassLinks As Collection
  Dim nquals&, lML As HspfMassLink, j&, itemp&, nGqual&, i&, k&
  Dim tPoll As HspfPollutant, lTableDef As HspfTableDef, tML As HspfMassLink
  Dim newOpn As HspfOperation, ctype$, rdone As Boolean, vPoll As Variant
  Dim vTable As Variant, ltable As HspfTable, cname$, tempTable As HspfTable
  Dim vML As Variant, nPqual&, nIqual&, iflag&, pflag&, rflag&
  Dim PqualCnt&, IqualCnt&, GqualCnt&, isedfg&, isedcnt&, psedfg&, psedcnt&
  Dim lowestindex&, lowestpos&, Counter&, OccurCount&, MaxOccur&
  Dim vtTable As Variant, ttable As HspfTable, SecName$, SecId&
  Dim iexist As Long
  Dim changed As Boolean
  Dim s As String

  If myUci.Edited = False Then
    changed = False
  Else
    changed = True
  End If
  
  'find total number of each type
  PqualCnt = 0
  IqualCnt = 0
  GqualCnt = 0
  For Each vPoll In myUci.Pollutants
    Set tPoll = vPoll
    iflag = 0
    pflag = 0
    rflag = 0
    If Len(tPoll.ModelType) > 0 Then
      For Each vOpn In tPoll.Operations
        Set newOpn = vOpn
        If newOpn.Name = "IMPLND" Then
          If tPoll.ModelType = "PIOnly" Or tPoll.ModelType = "PIG" Then
            iflag = 1
          End If
        ElseIf newOpn.Name = "PERLND" Then
          If tPoll.ModelType = "PIOnly" Or tPoll.ModelType = "PIG" Then
            pflag = 1
          End If
        ElseIf newOpn.Name = "RCHRES" Then
          If tPoll.ModelType = "GOnly" Or tPoll.ModelType = "PIG" Then
            rflag = 1
          End If
        End If
      Next vOpn
      IqualCnt = IqualCnt + iflag
      PqualCnt = PqualCnt + pflag
      GqualCnt = GqualCnt + rflag
    End If
  Next vPoll
      
  'remove any lingering mls from pquals or iquals
  i = 1
  Do While i <= myUci.MassLinks.Count
    Set tML = myUci.MassLinks(i)
    If tML.Source.VolName = "PERLND" And _
       tML.Source.Group = "PQUAL" And _
       tML.Target.VolName = "RCHRES" Then
      myUci.MassLinks.Remove i
    ElseIf tML.Source.VolName = "IMPLND" And _
       tML.Source.Group = "IQUAL" And _
       tML.Target.VolName = "RCHRES" Then
      myUci.MassLinks.Remove i
    Else
      i = i + 1
    End If
  Loop
      
  nPqual = 0
  nIqual = 0
  nGqual = 0
  isedcnt = 0
  psedcnt = 0
  Do While myUci.Pollutants.Count > 0
    'find lowest index to preserve order of pollutants
    lowestindex = 999
    lowestpos = 1
    Counter = 0
    For Each vPoll In myUci.Pollutants
      Set tPoll = vPoll
      Counter = Counter + 1
      If tPoll.Index < lowestindex Then
        lowestindex = tPoll.Index
        lowestpos = Counter
      End If
    Next vPoll
    Set tPoll = myUci.Pollutants(lowestpos)

    iflag = 0
    pflag = 0
    rflag = 0
    If Len(tPoll.ModelType) > 0 Then
      'put tables back
      For Each vOpn In tPoll.Operations
        Set newOpn = vOpn
        If newOpn.Name = "IMPLND" Then
          If tPoll.ModelType = "PIOnly" Or tPoll.ModelType = "PIG" Then
            iflag = 1
          End If
        ElseIf newOpn.Name = "PERLND" Then
          If tPoll.ModelType = "PIOnly" Or tPoll.ModelType = "PIG" Then
            pflag = 1
          End If
        ElseIf newOpn.Name = "RCHRES" Then
          If tPoll.ModelType = "GOnly" Or tPoll.ModelType = "PIG" Then
            rflag = 1
          End If
        End If
      Next vOpn
      nIqual = nIqual + iflag
      nPqual = nPqual + pflag
      nGqual = nGqual + rflag
      isedfg = 0
      psedfg = 0
      For j = 1 To tPoll.Operations.Count
        Set newOpn = tPoll.Operations(j)
        ctype = newOpn.Name
        Set lOpn = myUci.OpnBlks(newOpn.Name).Ids("K" & newOpn.Id)
        For Each vTable In newOpn.Tables
          Set ltable = vTable
          If ctype = "IMPLND" Then
            If ltable.Name <> "NQUALS" And ltable.Name <> "IQL-AD-FLAGS" And _
              ltable.Name <> "LAT-FACTOR" Then
              If ltable.TableNeededForAllQuals Then
                ltable.OccurNum = nIqual
                ltable.OccurCount = IqualCnt
                ltable.OccurIndex = 0
              Else
                ltable.OccurIndex = nIqual
              End If
            End If
            If ltable.Name = "QUAL-PROPS" Then
              isedfg = ltable.Parms("QSDFG").Value
            End If
          ElseIf ctype = "PERLND" Then
            If ltable.Name <> "NQUALS" And ltable.Name <> "PQL-AD-FLAGS" And _
              ltable.Name <> "LAT-FACTOR" Then  '(these three can only appear once)
              If ltable.TableNeededForAllQuals Then
                ltable.OccurNum = nPqual
                ltable.OccurCount = PqualCnt
                ltable.OccurIndex = 0
              Else
                ltable.OccurIndex = nPqual
              End If
            End If
            If ltable.Name = "QUAL-PROPS" Then
              psedfg = ltable.Parms("QSDFG").Value
            End If
          ElseIf ctype = "RCHRES" Then
            If ltable.Name <> "GQ-GENDATA" Then
              If ltable.TableNeededForAllQuals Then
                ltable.OccurNum = nGqual
                ltable.OccurCount = GqualCnt
                ltable.OccurIndex = 0
              Else
                ltable.OccurIndex = nGqual
              End If
            End If
          End If
          
          MaxOccur = myMsg.BlockDefs(ctype).TableDefs(ltable.Name).NumOccur
          SecName = myMsg.BlockDefs(ctype).TableDefs(ltable.Name).Parent.Name
          SecId = myMsg.BlockDefs(ctype).SectionID(SecName)
          'calc count for this table
          If Not ltable.TableNeededForAllQuals Then
            OccurCount = 1
            For Each vtTable In lOpn.Tables
              Set ttable = vtTable
              If ttable.Name = ltable.Name Then
                OccurCount = OccurCount + 1
              End If
            Next vtTable
            ltable.OccurNum = OccurCount
            'ltable.OccurCount = OccurCount  'moved down to keep counts right if too many
            If OccurCount <= MaxOccur Then
              'can add another
              ltable.OccurCount = OccurCount
              For Each vtTable In lOpn.Tables
                Set ttable = vtTable
                If ttable.Name = ltable.Name Then
                  'set occurance count for previous tables
                  ttable.OccurCount = OccurCount
                End If
              Next vtTable
            End If
          End If
                    
          If ltable.OccurNum <= MaxOccur Then
            'can add another
            If ltable.OccurNum > 1 Then
              cname = ltable.Name & ":" & ltable.OccurNum
            Else
              cname = ltable.Name
            End If
            lOpn.Tables.Add ltable, cname
            If lOpn.TableExists("ACTIVITY") Then
              lOpn.Tables("ACTIVITY").Parms(SecId) = 1 'turn on this section
              'turn on other prerequisite sections
              If lOpn.Name = "RCHRES" Then
                For i = 7 To SecId - 1
                  lOpn.Tables("ACTIVITY").Parms(i) = 1  'previous rqual sections must be on
                Next i
                If SecId > 1 Then
                  lOpn.Tables("ACTIVITY").Parms(1) = 1  'hydr must be on
                End If
                If SecId > 2 Then
                  lOpn.Tables("ACTIVITY").Parms(2) = 1  'adcalc must be on
                End If
                If SecId > 4 Then
                  lOpn.Tables("ACTIVITY").Parms(4) = 1  'htrch must be on
                End If
              ElseIf lOpn.Name = "IMPLND" Then
                If SecId = 5 Or SecId = 2 Then
                  lOpn.Tables("ACTIVITY").Parms(1) = 1  'atemp must be on
                End If
                If SecId > 3 Then
                  lOpn.Tables("ACTIVITY").Parms(3) = 1  'iwater must be on
                End If
              ElseIf lOpn.Name = "PERLND" Then
                If SecId > 8 Then
                  lOpn.Tables("ACTIVITY").Parms(8) = 1  'mstlay must be on
                End If
                If SecId = 5 Or SecId = 2 Or SecId = 6 Or SecId = 10 Or SecId = 11 Or SecId = 12 Then
                  lOpn.Tables("ACTIVITY").Parms(1) = 1  'atemp must be on
                End If
                If SecId = 4 Or SecId = 6 Or SecId = 7 Or SecId = 9 Or SecId = 10 Or SecId = 11 Then
                  lOpn.Tables("ACTIVITY").Parms(3) = 1  'pwater must be on
                End If
                If SecId = 6 Or SecId = 10 Or SecId = 11 Then
                  lOpn.Tables("ACTIVITY").Parms(5) = 1  'pstemp must be on
                End If
              End If
            End If
            
            If Not myUci.OpnBlks(ctype).TableExists(cname) Then
              myUci.OpnBlks(ctype).Tables.Add ltable, cname
              If ltable.OccurNum > 1 And ltable.TableNeededForAllQuals Then
                'make sure all previous occurs of this table exist
                For i = 1 To ltable.OccurNum - 1
                  If i > 1 Then
                    cname = ltable.Name & ":" & i
                  Else
                    cname = ltable.Name
                  End If
                  If Not myUci.OpnBlks(ctype).TableExists(cname) Then
                    'myUci.OpnBlks(ctype).AddTableForAll cname, ctype
                    Set tempTable = New HspfTable
                    Set tempTable.Opn = lOpn
                    Set tempTable.Def = myMsg.BlockDefs(ctype).TableDefs(ltable.Name)
                    s = ""
                    tempTable.initTable (s)
                    tempTable.OccurNum = i
                    tempTable.OccurCount = ltable.OccurCount
                    myUci.OpnBlks(ctype).Tables.Add tempTable, cname
                    'lOpn.Tables.Add tempTable, cname
                  End If
                Next i
              End If
            End If
          End If
        Next vTable
      Next j
      If isedfg = 1 Then
        isedcnt = isedcnt + 1
      End If
      If psedfg = 1 Then
        psedcnt = psedcnt + 1
      End If
      
      'put masslinks back
      For Each vML In tPoll.MassLinks
        Set lML = vML
        If lML.Source.Group = "PQUAL" Then
          If lML.Target.Member = "ISQAL" Then
            lML.Source.MemSub1 = psedcnt
          Else
            lML.Source.MemSub1 = nPqual
          End If
        ElseIf lML.Source.Group = "IQUAL" Then
          If lML.Target.Member = "ISQAL" Then
            lML.Source.MemSub1 = isedcnt
          Else
            lML.Source.MemSub1 = nIqual
          End If
        End If
        If lML.Target.Member = "IDQAL" Then
          lML.Target.MemSub1 = nGqual
        ElseIf lML.Target.Member = "ISQAL" Then
          lML.Target.MemSub2 = nGqual
        End If
        'make sure there isnt already a ml to this target
        iexist = 0
        For i = 1 To myUci.MassLinks.Count
          Set tML = myUci.MassLinks(i)
          If tML.Source.VolName = lML.Source.VolName And _
             tML.Target.VolName = lML.Target.VolName And _
             tML.Target.Group = lML.Target.Group And _
             tML.Target.Member = lML.Target.Member And _
             tML.Target.MemSub1 = lML.Target.MemSub1 And _
             tML.Target.MemSub2 = lML.Target.MemSub2 Then
             If tML.Source.Group = lML.Source.Group And _
                tML.Source.Member = lML.Source.Member And _
                tML.Source.MemSub1 = lML.Source.MemSub1 And _
                tML.Source.MemSub2 = lML.Source.MemSub2 Then
               'exact duplicate
               iexist = i
             ElseIf tML.Source.Group <> lML.Source.Group Then
               'different source group, remove it
               iexist = i
             End If
          End If
        Next i
        If iexist > 0 Then
          myUci.MassLinks.Remove iexist
        End If
        myUci.MassLinks.Add lML
      Next vML
    End If
    myUci.Pollutants.Remove lowestpos
  Loop
  
  'set nquals
  cname = "PERLND"
  For Each vOpn In myUci.OpnBlks(cname).Ids
    Set lOpn = vOpn
    If lOpn.TableExists("NQUALS") Then
      lOpn.Tables("NQUALS").Parms("NQUAL") = nPqual
    Else
      If nPqual > 0 Then
        myUci.OpnBlks(cname).AddTableForAll "NQUALS", cname
        lOpn.Tables("NQUALS").Parms("NQUAL") = nPqual
      End If
    End If
    If nPqual = 0 Then
      lOpn.Tables("ACTIVITY").Parms("PQALFG") = 0
    End If
  Next vOpn
  cname = "IMPLND"
  For Each vOpn In myUci.OpnBlks(cname).Ids
    Set lOpn = vOpn
    If lOpn.TableExists("NQUALS") Then
      lOpn.Tables("NQUALS").Parms("NQUAL") = nIqual
    Else
      If nIqual > 0 Then
        myUci.OpnBlks(cname).AddTableForAll "NQUALS", cname
        lOpn.Tables("NQUALS").Parms("NQUAL") = nIqual
      End If
    End If
    If nIqual = 0 Then
      lOpn.Tables("ACTIVITY").Parms("IQALFG") = 0
    End If
  Next vOpn
  cname = "RCHRES"
  For Each vOpn In myUci.OpnBlks(cname).Ids
    Set lOpn = vOpn
    If lOpn.TableExists("GQ-GENDATA") Then
      lOpn.Tables("GQ-GENDATA").Parms("NGQUAL") = nGqual
    Else
      If nGqual > 0 Then
        myUci.OpnBlks(cname).AddTableForAll "GQ-GENDATA", cname
        lOpn.Tables("GQ-GENDATA").Parms("NGQUAL") = nGqual
      End If
    End If
    If nGqual = 0 Then
      lOpn.Tables("ACTIVITY").Parms("GQALFG") = 0
    End If
  Next vOpn
  
  If changed Then
    myUci.Edited = True
  Else
    myUci.Edited = False
  End If
End Sub


