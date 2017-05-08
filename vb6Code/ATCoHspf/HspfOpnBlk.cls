VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "HspfOpnBlk"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit
'Copyright 2002 AQUA TERRA Consultants - Royalty-free use permitted under open source license

Dim pName As String
Dim pIds As Collection 'of HspfOperation
Dim pEdited As Boolean
Dim pTables As Collection 'of HspfTable
Dim pUci As HspfUci
Dim pComment As String

Public Property Get Comment() As String
  Comment = pComment
End Property

Public Property Let Comment(newComment As String)
  pComment = newComment
End Property

Public Property Get Edited() As Boolean
  Edited = pEdited
End Property
Public Property Let Edited(newEdited As Boolean)
  pEdited = newEdited
  If newEdited Then pUci.Edited = True
End Property

Public Property Get Name() As String
Attribute Name.VB_MemberFlags = "200"
  Name = pName
End Property
Public Property Let Name(newName As String)
  pName = newName
End Property

Public Property Get Ids() As Collection
  Set Ids = pIds
End Property

Public Function OperFromID(Id&) As HspfOperation
  Dim vOpn As Variant
  For Each vOpn In Me.Ids
    Set OperFromID = vOpn
    If OperFromID.Id = Id Then Exit For
  Next vOpn
  If Not OperFromID Is Nothing Then
    If OperFromID.Id <> Id Then
      Set OperFromID = Nothing
    End If
  End If
End Function

Public Function NthOper(nth&) As HspfOperation
  Dim vOpn As Variant
  Dim order&(), cnt&, lids&(), i&
  Dim lOpn As HspfOperation
  
  ReDim order(pIds.Count)
  ReDim lids(pIds.Count)
  
  cnt = 0
  For Each vOpn In Me.Ids
    Set lOpn = vOpn
    cnt = cnt + 1
    lids(cnt) = lOpn.Id
  Next vOpn
    
  'sort the order array
  Call SortIntegerArray(0, cnt, lids, order)

  For Each vOpn In Me.Ids
    Set NthOper = vOpn
    If NthOper.Id = lids(order(nth)) Then Exit For
  Next vOpn
End Function

Public Function Count() As Long
  Count = pIds.Count
End Function

Public Property Get Uci() As HspfUci
  Set Uci = pUci
End Property
Public Property Set Uci(newUci As HspfUci)
  Set pUci = newUci
End Property

Private Sub Class_Initialize()
  pName = ""
  Set pIds = New Collection
  Set pTables = New Collection
End Sub

Property Get Tables() As Collection 'of hspftables
  'make smarter if not found!!!
  'Set Tables = pIds(1).Tables
  Set Tables = pTables
End Property
Public Function TableExists(Name$) As Boolean
  Dim vTable As Variant
  
  On Error GoTo NoTable:
  Set vTable = pTables(Name)
  TableExists = True
  Exit Function
NoTable:
  TableExists = False
End Function

Public Sub setTableValues(blk As HspfBlockDef)
  If pUci.FastFlag Then
    GetCommentBeforeBlock pName, pComment
  End If
  Call readTables(blk)
End Sub

Private Sub readTables(blk As HspfBlockDef)
  Dim kwd$, kflg&, init&, contfg&, retid&, Nqual&
  Dim ctxt$, thisoccur&, noccur&, scnt&, nGqual&, srec&
  Dim ltable As HspfTable
  Dim tabno&, s$(), c$(), i&, itable&
  Dim opf&, opl&, sopl$
  Dim lId As HspfOperation, vId As Variant
  Dim lOpTyp&, TableComment$, lCombineOk As Boolean
  
  lOpTyp = HspfOperNum(pName)
  init = 1
  Debug.Print "Starting readTables at " & Time
  itable = 0
  Do
    'return table names
    If pUci.FastFlag Then
      itable = itable + 1
      kwd = blk.TableDefs(itable).Name
      StartingRecordofOperationTable blk.Name, kwd, srec, noccur
      If srec > 0 Then
        kflg = 1  'does it exist, 1 if so
      Else
        kflg = 0
      End If
      If itable < blk.TableDefs.Count Then
        contfg = 1   'more tables to read flag, 1 if so
      Else
        contfg = 0
      End If
      retid = (lOpTyp * 1000) + itable
    Else
      Call REM_GTNXKW(Me.Uci, init, CLng(lOpTyp + 120), kwd, kflg, contfg, retid)
      kwd = AddChar2Keyword(kwd)
    End If
    init = 0
    If kflg > 0 And retid <> 0 Then
      'check for multiple occurances
      If Not pUci.FastFlag Then
        Call REM_GETOCR(Me.Uci, retid, noccur)
      End If
      For thisoccur = 1 To noccur
        If pUci.FastFlag Then
          GetTableComment srec, kwd, thisoccur, TableComment
        End If
        Call GetTableRecordsFromUCI(lOpTyp + 120, blk.TableDefs(kwd).SGRP, blk.Name, kwd, _
                                    srec, thisoccur, scnt, s, c)
        For i = 1 To scnt
          'loop through each record
          opf = Left(s(i), 5)
          sopl = Trim(Mid(s(i), 6, 5))
          lCombineOk = True
          If Len(sopl) > 0 Then
            opl = sopl
          Else
            opl = opf
            'check to see if this record could have been combined with the next record
            If i > 1 Then
              If compareTableString(1, 10, s(i), s(i - 1)) Then
                'if it could have but wasn't, assume the user wants it on its own line
                lCombineOk = False
              End If
            End If
          End If
          For Each vId In pIds
            Set lId = vId
            If opf = lId.Id Or (opf <= lId.Id And lId.Id <= opl) Then
              Set ltable = New HspfTable
              Set ltable.Opn = lId
              Set ltable.Def = blk.TableDefs(kwd)
              ltable.initTable (s(i))
              Set ltable.Opn = lId
              ltable.OccurCount = noccur
              ltable.OccurNum = thisoccur
              ltable.OccurIndex = 0
              ltable.TableComment = TableComment
              ltable.CombineOK = lCombineOk
              If pName = "PERLND" And ltable.Def.Parent.Name = "PQUAL" Then
                'need to compute proper index
                If lId.TableExists("NQUALS") Then
                  Nqual = lId.Tables("NQUALS").Parms("NQUAL")
                  ltable.SetQualIndex thisoccur, Nqual
                End If
              End If
              If pName = "IMPLND" And ltable.Def.Parent.Name = "IQUAL" Then
                'need to compute proper index
                If lId.TableExists("NQUALS") Then
                  Nqual = lId.Tables("NQUALS").Parms("NQUAL")
                  ltable.SetQualIndex thisoccur, Nqual
                End If
              End If
              If pName = "RCHRES" And ltable.Def.Parent.Name = "GQUAL" Then
                'need to compute proper index
                If lId.TableExists("GQ-GENDATA") Then
                  nGqual = lId.Tables("GQ-GENDATA").Parms("NGQUAL")
                  ltable.SetQualIndex thisoccur, nGqual
                End If
              End If
              If Len(c(i)) > 0 Then ltable.Comment = c(i)
              If noccur > 1 And thisoccur > 1 Then
                lId.Tables.Add ltable, ltable.Name & ":" & thisoccur
                If Not TableExists(ltable.Name & ":" & thisoccur) Then
                  pTables.Add ltable, ltable.Name & ":" & thisoccur
                End If
              Else
                lId.Tables.Add ltable, ltable.Name
                If Not TableExists(ltable.Name) Then
                  pTables.Add ltable, ltable.Name
                End If
              End If
              If ltable.Name = "GEN-INFO" Then
                lId.Description = ltable.Parms(1).Value
              End If
              If ltable.Name = "HYDR-PARM2" Then
                Set lId.FTable = New HspfFtable
                Set lId.FTable.Operation = lId
                lId.FTable.Id = ltable.Parms(2).Value
              End If
            End If
          Next vId
        Next i
        If scnt = 0 Then
          'still need to add the dummy table to this opnblk
          Set ltable = New HspfTable
          Set ltable.Opn = pIds(1)
          Set ltable.Def = blk.TableDefs(kwd)
          ltable.initTable ("")
          ltable.OccurCount = noccur
          ltable.OccurNum = thisoccur
          ltable.OccurIndex = 0
          ltable.TableComment = TableComment
          If noccur > 1 And thisoccur > 1 Then
            If Not TableExists(ltable.Name & ":" & thisoccur) Then
              pTables.Add ltable, ltable.Name & ":" & thisoccur
            End If
          Else
            If Not TableExists(ltable.Name) Then
              pTables.Add ltable, ltable.Name
            End If
          End If
        End If
      Next thisoccur
    End If
  Loop While contfg = 1
  Debug.Print "Finishing readTables at " & Time
End Sub

Private Sub GetTableRecordsFromUCI(SCLU&, SGRP&, blockname$, tablename$, srec&, thisoccur&, _
                                   scnt&, s$(), c$())
  Dim tinit&, uunits&, retkey&, retcod&, stemp$, sameoper&, opf&, i&
  Dim pastHeader As Boolean, t$, rectyp&
  
  tinit = 1
  uunits = 1
  scnt = 0
  ReDim c(1)
  pastHeader = False
  Do
    retkey = -1
    If pUci.FastFlag Then
      GetNextRecordFromTable blockname, tablename, srec, tinit, thisoccur, stemp, rectyp, retcod
      'stemp = record returned
      'rectyp = record type returned, 0-normal, -1 comment, -2 blank
      'retcod = 1-returned header, 2-returned normal, 3-comment, 10-no more
    Else
      Call REM_XTABLEEX(Me.Uci, SCLU, SGRP, _
                        uunits, tinit, CLng(1), thisoccur, _
                        retkey, stemp, rectyp, retcod)
    End If
    tinit = 0
    If retcod = 2 Then
      'this is the type of record we want
      opf = Left(stemp, 5)
      'see if we already have a string with this oper
      sameoper = 0
      For i = 1 To scnt
        If Left(s(i), 5) = opf Then
          sameoper = i
        End If
      Next i
      If sameoper = 0 Then
        'this is a new operation
        scnt = scnt + 1
        ReDim Preserve s(scnt)
        ReDim Preserve c(scnt + 1)
        s(scnt) = stemp
      Else
        'this is the same operation number, add to end for multiple line tables
        If Len(s(sameoper)) < 80 Then
          'pad with blanks
          For i = (Len(s(sameoper)) + 1) To 80 'pad with blanks
            s(sameoper) = s(sameoper) & " "
          Next i
        End If
        s(sameoper) = s(sameoper) & Mid(stemp, 11)
        For i = (Len(stemp) + 1) To 80 'pad with blanks
          s(sameoper) = s(sameoper) & " "
        Next i
      End If
    ElseIf retcod = 1 Then 'normal header ???
    ElseIf retcod = 3 Then 'comment
      If Len(c(scnt + 1)) = 0 Then
        c(scnt + 1) = stemp
      Else
        c(scnt + 1) = c(scnt + 1) & vbCrLf & stemp
      End If
    ElseIf retcod = 10 Then
      Exit Do
    Else
      MsgBox stemp
    End If
  Loop
End Sub

Public Sub createTables(blk As HspfBlockDef)
  Dim ltable As HspfTable
  Dim s$, kwd$
  Dim lId As HspfOperation, vId As Variant
  Dim lOpTyp&
  Dim vTabList As Variant, vTab As Variant
  
  lOpTyp = HspfOperNum(pName)
  'could do something here with table status info?
  If blk.Name = "PERLND" Then
    vTabList = Array("ACTIVITY", "PRINT-INFO", "GEN-INFO", "PWAT-PARM1", "PWAT-PARM2", "PWAT-PARM3", "PWAT-PARM4", _
                     "MON-INTERCEP", "MON-LZETPARM", "PWAT-STATE1")
  ElseIf blk.Name = "IMPLND" Then
    vTabList = Array("ACTIVITY", "PRINT-INFO", "GEN-INFO", "IWAT-PARM1", "IWAT-PARM2", "IWAT-PARM3", "IWAT-STATE1")
  ElseIf blk.Name = "RCHRES" Then
    vTabList = Array("ACTIVITY", "PRINT-INFO", "GEN-INFO", "HYDR-PARM1", "HYDR-PARM2", "HYDR-INIT")
  End If
  
  For Each vTab In vTabList
    kwd = vTab
    For Each vId In pIds
      Set lId = vId
      Set ltable = New HspfTable
      Set ltable.Opn = lId
      Set ltable.Def = blk.TableDefs(kwd)
      s = ""
      ltable.initTable (s)
      ltable.OccurCount = 1
      ltable.OccurNum = 1
      Set ltable.Opn = lId
      lId.Tables.Add ltable, ltable.Name
      If ltable.Name = "HYDR-PARM2" Then
        Set lId.FTable = New HspfFtable
        Set lId.FTable.Operation = lId
        ltable.Parms(2).Value = lId.Id
        lId.FTable.Id = lId.Id
      End If
      If Not Me.TableExists(ltable.Name) Then
        Me.Tables.Add ltable, ltable.Name    'pbd - needs to be added?
      End If
    Next vId
  Next vTab
End Sub

Public Sub AddTable(opid&, tabname$, blk As HspfBlockDef)
  'add a table to the uci object for this operation id
  Dim ltable As HspfTable
  Dim s$, O&, t$, i&
  Dim lId As HspfOperation, vId As Variant
  
  For Each vId In pIds
    Set lId = vId
    If lId.Id = opid Then
      Set ltable = New HspfTable
      Set ltable.Opn = lId
      i = InStr(tabname, ":")
      If i > 0 Then
        t = Left(tabname, i - 1)
        O = CInt(Right(tabname, Len(tabname) - i))
      Else
        t = tabname
        O = 1
      End If
      Set ltable.Def = blk.TableDefs(t)
      s = ""
      ltable.initTable (s)
      ltable.OccurCount = O
      ltable.OccurNum = O
      If O > 1 Then
        'set occurcounts for previous occurrances
        If Me.TableExists(t) Then
          Me.Tables(t).OccurCount = O
        End If
        For i = 2 To O - 1
          If Me.TableExists(t & ":" & i) Then
            Me.Tables(t & ":" & i).OccurCount = O
          End If
        Next i
      End If
      Set ltable.Opn = lId
      If Not lId.TableExists(tabname) Then
        lId.Tables.Add ltable, tabname
      End If
      If Not Me.TableExists(tabname) Then
        Me.Tables.Add ltable, tabname
      End If
      Exit For
    End If
  Next vId
End Sub

Public Sub AddTableForAll(tabname$, opname$)
  'add a table to the uci object for all operation ids
  Dim ltable As HspfTable
  Dim s$, O&, t$, i&
  Dim lId As HspfOperation, vId As Variant
  Dim blk As HspfBlockDef
  
  Set blk = pUci.Msg.BlockDefs(opname)
  
  For Each vId In pIds
    Set lId = vId
    Set ltable = New HspfTable
    Set ltable.Opn = lId
    i = InStr(tabname, ":")
    If i > 0 Then
      t = Left(tabname, i - 1)
      O = CInt(Right(tabname, Len(tabname) - i))
    Else
      t = tabname
      O = 1
    End If
    Set ltable.Def = blk.TableDefs(t)
    s = ""
    ltable.initTable (s)
    ltable.OccurCount = O
    ltable.OccurNum = O
    If O > 1 Then
      'set occurcounts for previous occurrances
      If Me.TableExists(t) Then
        Me.Tables(t).OccurCount = O
      End If
      For i = 2 To O - 1
        If Me.TableExists(t & ":" & i) Then
          Me.Tables(t & ":" & i).OccurCount = O
        End If
      Next i
    End If
    Set ltable.Opn = lId
    If Not lId.TableExists(tabname) Then  'add to this id
      lId.Tables.Add ltable, tabname
    End If
    If Not Me.TableExists(tabname) Then   'add to this oper-type block
      Me.Tables.Add ltable, tabname
    End If
  Next vId
End Sub

Public Sub RemoveTable(opid&, tabname$)
  'remove this oper from a table, remove whole table if this is the last
  Dim lOper As HspfOperation
  
  Set lOper = OperFromID(opid)
  lOper.Tables.Remove tabname
  
End Sub

Public Sub WriteUciFile(f As Integer, M As HspfMsg)
  Dim vTableDef As Variant, lTableDef As HspfTableDef, i&
  Dim vTable As Variant, ltable As HspfTable
  Dim lBlockDef As HspfBlockDef, j&, k&, t$
  Dim vId As Variant, lId As HspfOperation, ttable As HspfTable
  Dim firstTable As Boolean
  Dim lGroupIndex&, lFirstInGroup&, lInGroup As Boolean
  Dim lLastGroupIndex&, lLastInGroup&, lCurrentOccurGroup&
  
  firstTable = True
  If Len(pComment) > 0 Then
    Print #f, pComment
  End If
  Print #f, " "
  Print #f, pName
  Set lBlockDef = M.BlockDefs(pName)
  
  lInGroup = False
  
  For i = 1 To lBlockDef.TableDefs.Count 'must look thru all possible tables
    Set lTableDef = lBlockDef.TableDefs(i)
    
    If lTableDef.OccurGroup = 0 And Not lInGroup Then
      'the basic case
      For Each vId In pIds
        Set lId = vId
        If lId.TableExists(lTableDef.Name) Then
          Set ltable = lId.Tables(lTableDef.Name)
          If Len(ltable.TableComment) > 0 Then
            Print #f, ltable.TableComment
          End If
          If Not (firstTable) Then Print #f, " "
          Call ltable.WriteUciFile(f)  'this writes all like this
          Exit For
        End If
      Next vId
      
    Else 'this is a multiple occurance group (like pqual, iqual, gqual)
    
      If lInGroup Then
        If lTableDef.OccurGroup <> lCurrentOccurGroup Or i > lBlockDef.TableDefs.Count Then
          'we were in a multiple occurance group but have reached end of group
          lGroupIndex = lGroupIndex + 1  'look for next occurance
          If lGroupIndex > lLastGroupIndex Then
            lInGroup = False  'no more to do
            If lLastInGroup > 0 Then i = lLastInGroup
          Else
            lLastInGroup = i - 1 'remember which was the last table in group
            i = lFirstInGroup
            Set lTableDef = lBlockDef.TableDefs(i)
          End If
        End If
      Else 'start of a multiple occurance group
        lInGroup = True
        lGroupIndex = 1
        lFirstInGroup = i
        lLastGroupIndex = 0
        lLastInGroup = 0
        lCurrentOccurGroup = lTableDef.OccurGroup
      End If
      
      If lInGroup Then
        For Each vId In pIds
          Set lId = vId
          'If lId.TableExists(lTableDef.Name) Then  'accomodate empty placeholder tables
          If lId.OpnBlk.TableExists(lTableDef.Name) Then
            Set ltable = lId.OpnBlk.Tables(lTableDef.Name)
            If ltable.OccurIndex = 0 Or (ltable.OccurIndex > 0 And ltable.OccurIndex <= lGroupIndex) Then
              If lGroupIndex > 1 And ltable.OccurIndex = 0 Then
                'write the comment that applies to this table
                t = ltable.Name & ":" & lGroupIndex
                Set ttable = lId.OpnBlk.Tables(t)
                If Len(ttable.TableComment) > 0 Then
                  Print #f, ttable.TableComment
                End If
              Else
                If Len(ltable.TableComment) > 0 Then
                  Print #f, ltable.TableComment
                End If
              End If
              If Not (firstTable) Then Print #f, " "
              If ltable.OccurIndex = 0 Then
                Call ltable.WriteUciFile(f, lGroupIndex) 'write out just this occurance
              Else
                'special case for some p/i/gqual tables
                j = 0
                For k = 1 To ltable.OccurCount
                  t = ltable.Name
                  If k > 1 Then
                    t = t & ":" & k
                  End If
                  Set ttable = lId.OpnBlk.Tables(t)
                  If ttable.OccurIndex = lGroupIndex Then
                    j = ttable.OccurNum
                    Exit For
                  End If
                Next k
                If j > 0 Then
                  Call ltable.WriteUciFile(f, j) 'write out just this occurance
                End If
              End If
              If ltable.OccurCount > lLastGroupIndex Then lLastGroupIndex = ltable.OccurCount
              Exit For
            End If
          End If
        Next vId
        
      End If
      
    End If
  Next i
  Print #f, "END " & pName
End Sub

Public Function OperByDesc(Desc$) As HspfOperation
   Dim lId As HspfOperation, vId As Variant
   
   Set OperByDesc = Nothing 'Changed by Mark
   For Each vId In Ids
     Set lId = vId
     If lId.Description = Desc Then
       Set OperByDesc = lId 'Changed by Mark
       Exit For
     End If
   Next vId
End Function
