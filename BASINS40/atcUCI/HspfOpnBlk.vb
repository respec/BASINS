'Copyright 2006 AQUA TERRA Consultants - Royalty-free use permitted under open source license
Option Strict Off
Option Explicit On

Imports System.Collections.ObjectModel
Imports System.Text

Public Class HspfOpnBlks
    Inherits KeyedCollection(Of String, HspfOpnBlk)
    Protected Overrides Function GetKeyForItem(ByVal aHspfOpnBlk As HspfOpnBlk) As String
        Return aHspfOpnBlk.Name
    End Function
End Class

Public Class HspfOpnBlk
    Private pName As String
    Private pIds As HspfOperations
    Private pEdited As Boolean
    Private pTables As Collection '(Of HspfTable)
    Private pUci As HspfUci
    Private pComment As String

    Public Property Comment() As String
        Get
            Comment = pComment
        End Get
        Set(ByVal Value As String)
            pComment = Value
        End Set
    End Property

    Public Property Edited() As Boolean
        Get
            Edited = pEdited
        End Get
        Set(ByVal Value As Boolean)
            pEdited = Value
            If Value Then pUci.Edited = True
        End Set
    End Property

    Public Property Name() As String
        Get
            Name = pName
        End Get
        Set(ByVal Value As String)
            pName = Value
        End Set
    End Property

    Public ReadOnly Property Ids() As HspfOperations
        Get
            Ids = pIds
        End Get
    End Property

    Public Property Uci() As HspfUci
        Get
            Uci = pUci
        End Get
        Set(ByVal Value As HspfUci)
            pUci = Value
        End Set
    End Property

    ReadOnly Property Tables() As Collection
        Get 'of hspftables
            'make smarter if not found!!!
            'Set Tables = pIds(1).Tables
            Tables = pTables
        End Get
    End Property

    Public Function OperFromID(ByRef Id As Integer) As HspfOperation
        For Each lOperation As HspfOperation In Me.Ids
            If lOperation.Id = Id Then Return lOperation
        Next
        Return Nothing
    End Function

    Public Function NthOper(ByRef nth As Integer) As HspfOperation
        Dim order() As Integer
        Dim cnt As Integer
        Dim lids() As Integer
        Dim lOpn As HspfOperation

        ReDim order(pIds.Count())
        ReDim lids(pIds.Count())

        cnt = 0
        For Each lOpn In Me.Ids
            cnt = cnt + 1
            lids(cnt) = lOpn.Id
        Next

        'sort the order array
        SortIntegerArray(0, cnt, lids, order)

        For Each lOpn In Me.Ids
            If lOpn.Id = lids(order(nth)) Then Return lOpn
        Next
        Return Nothing
    End Function

    Public Function Count() As Integer
        Count = pIds.Count()
    End Function

    Public Sub New()
        MyBase.New()
        pName = ""
        pIds = New HspfOperations
        pTables = New Collection
    End Sub

    Public Function TableExists(ByRef aName As String) As Boolean
        Return pTables.Contains(aName)
    End Function

    Public Sub setTableValues(ByRef blk As HspfBlockDef)
        If pUci.FastFlag Then
            GetCommentBeforeBlock(pName, pComment)
        End If
        Call readTables(blk)
    End Sub

    Private Sub readTables(ByRef blk As HspfBlockDef)
        Dim kwd As String = Nothing
        Dim retid, init, kflg, contfg, Nqual As Integer
        Dim nGqual, noccur, thisoccur, scnt, srec As Integer
        Dim ltable As HspfTable
        Dim i, itable As Integer
        Dim s() As String = {}
        Dim c() As String = {}
        Dim opf, opl As Integer
        Dim sopl As String
        Dim lId As HspfOperation
        Dim vId As Object
        Dim lOpTyp As Integer
        Dim TableComment As String = ""
        Dim lCombineOk As Boolean

        lOpTyp = HspfOperNum(pName)
        init = 1
        Debug.Print("Starting readTables at " & TimeOfDay)
        itable = 0
        Do
            'return table names
            If pUci.FastFlag Then
                itable = itable + 1
                kwd = blk.TableDefs.Item(itable).Name
                StartingRecordofOperationTable((blk.Name), kwd, srec, noccur)
                If srec > 0 Then
                    kflg = 1 'does it exist, 1 if so
                Else
                    kflg = 0
                End If
                If itable < blk.TableDefs.Count() Then
                    contfg = 1 'more tables to read flag, 1 if so
                Else
                    contfg = 0
                End If
                retid = (lOpTyp * 1000) + itable
            Else
                Call REM_GTNXKW((Me.Uci), init, CInt(lOpTyp + 120), kwd, kflg, contfg, retid)
                kwd = AddChar2Keyword(kwd)
            End If
            init = 0
            If kflg > 0 And retid <> 0 Then
                'check for multiple occurences
                If Not pUci.FastFlag Then
                    Call REM_GETOCR((Me.Uci), retid, noccur)
                End If
                For thisoccur = 1 To noccur
                    If pUci.FastFlag Then
                        GetTableComment(srec, kwd, thisoccur, TableComment)
                    End If
                    Call GetTableRecordsFromUCI(lOpTyp + 120, blk.TableDefs.Item(kwd).SGRP, (blk.Name), kwd, srec, thisoccur, scnt, s, c)
                    For i = 1 To scnt
                        'loop through each record
                        opf = CInt(Left(s(i), 5))
                        sopl = Trim(Mid(s(i), 6, 5))
                        lCombineOk = True
                        If Len(sopl) > 0 Then
                            opl = CInt(sopl)
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
                            lId = vId
                            If opf = lId.Id Or (opf <= lId.Id And lId.Id <= opl) Then
                                ltable = New HspfTable
                                ltable.Opn = lId
                                ltable.Def = blk.TableDefs.Item(kwd)
                                ltable.initTable((s(i)))
                                ltable.Opn = lId
                                ltable.OccurCount = noccur
                                ltable.OccurNum = thisoccur
                                ltable.OccurIndex = 0
                                ltable.TableComment = TableComment
                                ltable.CombineOK = lCombineOk
                                If pName = "PERLND" And ltable.Def.Parent.Name = "PQUAL" Then
                                    'need to compute proper index
                                    If lId.TableExists("NQUALS") Then
                                        Nqual = lId.Tables.Item("NQUALS").ParmValue("NQUAL")
                                        ltable.SetQualIndex(thisoccur, Nqual)
                                    End If
                                End If
                                If pName = "IMPLND" And ltable.Def.Parent.Name = "IQUAL" Then
                                    'need to compute proper index
                                    If lId.TableExists("NQUALS") Then
                                        Nqual = lId.Tables.Item("NQUALS").ParmValue("NQUAL")
                                        ltable.SetQualIndex(thisoccur, Nqual)
                                    End If
                                End If
                                If pName = "RCHRES" And ltable.Def.Parent.Name = "GQUAL" Then
                                    'need to compute proper index
                                    If lId.TableExists("GQ-GENDATA") Then
                                        nGqual = lId.Tables.Item("GQ-GENDATA").ParmValue("NGQUAL")
                                        ltable.SetQualIndex(thisoccur, nGqual)
                                    End If
                                End If
                                If Len(c(i)) > 0 Then ltable.Comment = c(i)
                                If noccur > 1 And thisoccur > 1 Then
                                    lId.Tables.Add(ltable, ltable.Name & ":" & thisoccur)
                                    If Not TableExists(ltable.Name & ":" & thisoccur) Then
                                        pTables.Add(ltable, ltable.Name & ":" & thisoccur)
                                    End If
                                Else
                                    Try
                                        lId.Tables.Add(ltable, ltable.Name)
                                        If Not TableExists((ltable.Name)) Then
                                            pTables.Add(ltable, ltable.Name)
                                        End If
                                    Catch
                                        Debug.Print("TableProblem:" & ltable.Name & ":" & lId.Id)
                                    End Try
                                End If
                                If ltable.Name = "GEN-INFO" Then
                                    lId.Description = ltable.Parms.Item(1).Value
                                End If
                                If ltable.Name = "HYDR-PARM2" Then
                                    lId.FTable = New HspfFtable
                                    lId.FTable.Operation = lId
                                    lId.FTable.Id = ltable.Parms.Item(2).Value
                                End If
                            End If
                        Next vId
                    Next i
                    If scnt = 0 Then
                        'still need to add the dummy table to this opnblk
                        ltable = New HspfTable
                        ltable.Opn = pIds.Item(1)
                        ltable.Def = blk.TableDefs.Item(kwd)
                        ltable.initTable((""))
                        ltable.OccurCount = noccur
                        ltable.OccurNum = thisoccur
                        ltable.OccurIndex = 0
                        ltable.TableComment = TableComment
                        If noccur > 1 And thisoccur > 1 Then
                            If Not TableExists(ltable.Name & ":" & thisoccur) Then
                                pTables.Add(ltable, ltable.Name & ":" & thisoccur)
                            End If
                        Else
                            If Not TableExists((ltable.Name)) Then
                                pTables.Add(ltable, ltable.Name)
                            End If
                        End If
                    End If
                Next thisoccur
            End If
        Loop While contfg = 1
        Debug.Print("Finishing readTables at " & TimeOfDay)
    End Sub

    Private Sub GetTableRecordsFromUCI(ByRef SCLU As Integer, ByRef SGRP As Integer, ByRef blockname As String, ByRef tablename As String, ByRef srec As Integer, ByRef thisoccur As Integer, ByRef scnt As Integer, ByRef s() As String, ByRef c() As String)
        Dim opf, retcod, uunits, tinit, retkey, sameoper, i As Integer
        Dim stemp As String = Nothing
        Dim pastHeader As Boolean
        Dim rectyp As Integer

        tinit = 1
        uunits = 1
        scnt = 0
        ReDim c(1)
        pastHeader = False
        Do
            retkey = -1
            If pUci.FastFlag Then
                GetNextRecordFromTable(blockname, tablename, srec, tinit, thisoccur, stemp, rectyp, retcod)
                'stemp = record returned
                'rectyp = record type returned, 0-normal, -1 comment, -2 blank
                'retcod = 1-returned header, 2-returned normal, 3-comment, 10-no more
            Else
                Call REM_XTABLEEX((Me.Uci), SCLU, SGRP, uunits, tinit, CInt(1), thisoccur, retkey, stemp, rectyp, retcod)
            End If
            tinit = 0
            If retcod = 2 Then
                'this is the type of record we want
                opf = CInt(Left(stemp, 5))
                'see if we already have a string with this oper
                sameoper = 0
                For i = 1 To scnt
                    If CDbl(Left(s(i), 5)) = opf Then
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
            ElseIf retcod = 1 Then  'normal header ???
            ElseIf retcod = 3 Then  'comment
                If Len(c(scnt + 1)) = 0 Then
                    c(scnt + 1) = stemp
                Else
                    c(scnt + 1) = c(scnt + 1) & vbCrLf & stemp
                End If
            ElseIf retcod = 10 Then
                Exit Do
            Else
                MsgBox(stemp)
            End If
        Loop
    End Sub

    Public Sub createTables(ByRef blk As HspfBlockDef)
        Dim ltable As HspfTable
        Dim s, kwd As String
        Dim lId As HspfOperation
        Dim vId As Object
        Dim lOpTyp As Integer
        Dim vTabList() As String
        Dim vTab As String
        Static PERLNDtabList() As String = {"ACTIVITY", "PRINT-INFO", "GEN-INFO", "PWAT-PARM1", "PWAT-PARM2", "PWAT-PARM3", "PWAT-PARM4", "MON-INTERCEP", "MON-LZETPARM", "PWAT-STATE1"}
        Static IMPLNDtabList() As String = {"ACTIVITY", "PRINT-INFO", "GEN-INFO", "IWAT-PARM1", "IWAT-PARM2", "IWAT-PARM3", "IWAT-STATE1"}
        Static RCHREStabList() As String = {"ACTIVITY", "PRINT-INFO", "GEN-INFO", "HYDR-PARM1", "HYDR-PARM2", "HYDR-INIT"}

        lOpTyp = HspfOperNum(pName)
        'could do something here with table status info?
        Select Case blk.Name
            Case "PERLND" : vTabList = PERLNDtabList
            Case "IMPLND" : vTabList = IMPLNDtabList
            Case "RCHRES" : vTabList = RCHREStabList
            Case Else : ReDim vTabList(-1)
        End Select

        For Each vTab In vTabList
            kwd = vTab
            For Each vId In pIds
                lId = vId
                ltable = New HspfTable
                ltable.Opn = lId
                ltable.Def = blk.TableDefs.Item(kwd)
                s = ""
                ltable.initTable((s))
                ltable.OccurCount = 1
                ltable.OccurNum = 1
                ltable.Opn = lId
                lId.Tables.Add(ltable, ltable.Name)
                If ltable.Name = "HYDR-PARM2" Then
                    lId.FTable = New HspfFtable
                    lId.FTable.Operation = lId
                    ltable.Parms.Item(2).Value = lId.Id
                    lId.FTable.Id = lId.Id
                End If
                If Not Me.TableExists((ltable.Name)) Then
                    Me.Tables.Add(ltable, ltable.Name) 'pbd - needs to be added?
                End If
            Next vId
        Next vTab
    End Sub

    Public Sub AddTable(ByRef opid As Integer, ByRef tabname As String, ByRef blk As HspfBlockDef)
        'add a table to the uci object for this operation id
        Dim ltable As HspfTable
        Dim s, t As String
        Dim O, i As Integer
        Dim lId As HspfOperation
        Dim vId As Object

        For Each vId In pIds
            lId = vId
            If lId.Id = opid Then
                ltable = New HspfTable
                ltable.Opn = lId
                i = InStr(tabname, ":")
                If i > 0 Then
                    t = Left(tabname, i - 1)
                    O = CShort(Right(tabname, Len(tabname) - i))
                Else
                    t = tabname
                    O = 1
                End If
                ltable.Def = blk.TableDefs.Item(t)
                s = ""
                ltable.initTable((s))
                ltable.OccurCount = O
                ltable.OccurNum = O
                If O > 1 Then
                    'set occurcounts for previous occurrances
                    If Me.TableExists(t) Then
                        Me.Tables.Item(t).OccurCount = O
                    End If
                    For i = 2 To O - 1
                        If Me.TableExists(t & ":" & i) Then
                            Me.Tables.Item(t & ":" & i).OccurCount = O
                        End If
                    Next i
                End If
                ltable.Opn = lId
                If Not lId.TableExists(tabname) Then
                    lId.Tables.Add(ltable, tabname)
                End If
                If Not Me.TableExists(tabname) Then
                    Me.Tables.Add(ltable, tabname)
                End If
                Exit For
            End If
        Next vId
    End Sub

    Public Sub AddTableForAll(ByRef tabname As String, ByRef opname As String)
        'add a table to the uci object for all operation ids
        Dim ltable As HspfTable
        Dim s, t As String
        Dim O, i As Integer
        Dim lId As HspfOperation
        Dim vId As Object
        Dim blk As HspfBlockDef

        blk = pUci.Msg.BlockDefs(opname)

        For Each vId In pIds
            lId = vId
            ltable = New HspfTable
            ltable.Opn = lId
            i = InStr(tabname, ":")
            If i > 0 Then
                t = Left(tabname, i - 1)
                O = CShort(Right(tabname, Len(tabname) - i))
            Else
                t = tabname
                O = 1
            End If
            ltable.Def = blk.TableDefs.Item(t)
            s = ""
            ltable.initTable((s))
            ltable.OccurCount = O
            ltable.OccurNum = O
            If O > 1 Then
                'set occurcounts for previous occurrances
                If Me.TableExists(t) Then
                    Me.Tables.Item(t).OccurCount = O
                End If
                For i = 2 To O - 1
                    If Me.TableExists(t & ":" & i) Then
                        Me.Tables.Item(t & ":" & i).OccurCount = O
                    End If
                Next i
            End If
            ltable.Opn = lId
            If Not lId.TableExists(tabname) Then 'add to this id
                lId.Tables.Add(ltable, tabname)
            End If
            If Not Me.TableExists(tabname) Then 'add to this oper-type block
                Me.Tables.Add(ltable, tabname)
            End If
        Next vId
    End Sub

    Public Sub RemoveTable(ByRef opid As Integer, ByRef tabname As String)
        'remove this oper from a table, remove whole table if this is the last
        Dim lOper As HspfOperation

        lOper = OperFromID(opid)
        lOper.Tables.Remove(tabname)
    End Sub

    Public Overrides Function ToString() As String
        Dim lSB As New StringBuilder
        Dim lTableDef As HspfTableDef
        Dim i As Integer
        Dim lTable As HspfTable
        Dim lBlockDef As HspfBlockDef
        Dim j, k As Integer
        Dim t As String
        Dim vId As Object
        Dim lId As HspfOperation
        Dim ttable As HspfTable
        Dim firstTable As Boolean
        Dim lGroupIndex, lFirstInGroup As Integer
        Dim lInGroup As Boolean
        Dim lLastInGroup, lLastGroupIndex, lCurrentOccurGroup As Integer

        firstTable = True
        If pComment.Length > 0 Then
            lSB.AppendLine(pComment)
        End If
        lSB.AppendLine(pName)
        lBlockDef = pUci.Msg.BlockDefs.Item(pName)

        lInGroup = False

        For i = 1 To lBlockDef.TableDefs.Count() 'must look thru all possible tables
            lTableDef = lBlockDef.TableDefs.Item(i)

            If lTableDef.OccurGroup = 0 And Not lInGroup Then
                'the basic case
                For Each vId In pIds
                    lId = vId
                    If lId.TableExists(lTableDef.Name) Then
                        lTable = lId.Tables.Item(lTableDef.Name)
                        If lTable.TableComment.Length > 0 Then
                            lSB.AppendLine(lTable.TableComment)
                        End If
                        If Not (firstTable) Then
                            lSB.AppendLine(" ")
                        End If
                        lSB.AppendLine(lTable.ToString) 'this writes all like this
                        Exit For
                    End If
                Next vId

            Else 'this is a multiple occurence group (like pqual, iqual, gqual)

                If lInGroup Then
                    If lTableDef.OccurGroup <> lCurrentOccurGroup Or i = lBlockDef.TableDefs.Count() Then
                        'we were in a multiple occurence group but have reached end of group
                        lGroupIndex = lGroupIndex + 1 'look for next occurence
                        If lGroupIndex > lLastGroupIndex Then
                            lInGroup = False 'no more to do
                            If lLastInGroup > 0 Then i = lLastInGroup
                        Else
                            lLastInGroup = i - 1 'remember which was the last table in group
                            i = lFirstInGroup
                            lTableDef = lBlockDef.TableDefs.Item(i)
                        End If
                    End If
                Else 'start of a multiple occurence group
                    lInGroup = True
                    lGroupIndex = 1
                    lFirstInGroup = i
                    lLastGroupIndex = 0
                    lLastInGroup = 0
                    lCurrentOccurGroup = lTableDef.OccurGroup
                End If

                If lInGroup Then
                    For Each vId In pIds
                        lId = vId
                        'If lId.TableExists(lTableDef.Name) Then  'accomodate empty placeholder tables
                        If lId.OpnBlk.TableExists((lTableDef.Name)) Then
                            lTable = lId.OpnBlk.Tables(lTableDef.Name)
                            If lTable.OccurIndex = 0 Or (lTable.OccurIndex > 0 And lTable.OccurIndex <= lGroupIndex) Then
                                If lGroupIndex > 1 And lTable.OccurIndex = 0 Then
                                    'write the comment that applies to this table
                                    t = lTable.Name & ":" & lGroupIndex
                                    ttable = lId.OpnBlk.Tables(t)
                                    If ttable.TableComment.Length > 0 Then
                                        lSB.AppendLine(ttable.TableComment)
                                    End If
                                Else
                                    If lTable.TableComment.Length > 0 Then
                                        lSB.AppendLine(lTable.TableComment)
                                    End If
                                End If
                                If Not (firstTable) Then
                                    lSB.AppendLine(" ")
                                End If
                                If lTable.OccurIndex = 0 Then 'write out just this occurence
                                    lSB.AppendLine(lTable.ToStringByIndex(lGroupIndex))
                                Else
                                    'special case for some p/i/gqual tables
                                    j = 0
                                    For k = 1 To lTable.OccurCount
                                        t = lTable.Name
                                        If k > 1 Then
                                            t = t & ":" & k
                                        End If
                                        ttable = lId.OpnBlk.Tables(t)
                                        If ttable.OccurIndex = lGroupIndex Then
                                            j = ttable.OccurNum
                                            Exit For
                                        End If
                                    Next k
                                    If j > 0 Then 'write out just this occurence
                                        lSB.AppendLine(lTable.ToStringByIndex(j))
                                    End If
                                End If
                                If lTable.OccurCount > lLastGroupIndex Then
                                    lLastGroupIndex = lTable.OccurCount
                                End If
                                Exit For
                            End If
                        End If
                    Next vId
                End If
            End If
        Next i
        lSB.AppendLine("END " & pName)
        Return lSB.ToString
    End Function

    Public Function OperByDesc(ByRef Desc As String) As HspfOperation
        Dim lId As HspfOperation
        Dim vId As Object

        'UPGRADE_NOTE: Object OperByDesc may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        OperByDesc = Nothing 'Changed by Mark
        For Each vId In Ids
            lId = vId
            If lId.Description = Desc Then
                OperByDesc = lId 'Changed by Mark
                Exit For
            End If
        Next vId
    End Function

    Private Sub SortIntegerArray(ByRef opt As Integer, ByRef cnt As Integer, ByRef iVal() As Integer, ByRef pos() As Integer)
        ' ##SUMMARY Sorts integers in array into ascending order.
        ' ##PARAM opt I Sort option (0 = sort in place, 1 = move values in array to sorted position)
        ' ##PARAM cnt I Count of integers to sort
        ' ##PARAM iVal I Array of integers to sort
        ' ##PARAM pos O Array containing sorted order of integers
        Dim i As Integer
        Dim j As Integer
        Dim jpt As Integer
        Dim jpt1 As Integer
        Dim itmp As Integer
        ' ##LOCAL i - long counter for outer loop
        ' ##LOCAL j - long counter for inner loop
        ' ##LOCAL jpt - long pointer to j index
        ' ##LOCAL jpt1 - long pointer to (j + 1) index
        ' ##LOCAL itmp - long temporary holder for values in iVal array

        'set default positions(assume in order)
        For i = 1 To cnt
            pos(i) = i
        Next i

        'make a pointer to values with bubble sort
        For i = cnt To 2 Step -1
            For j = 1 To i - 1
                jpt = pos(j)
                jpt1 = pos(j + 1)
                If (iVal(jpt) > iVal(jpt1)) Then
                    pos(j + 1) = jpt
                    pos(j) = jpt1
                End If
            Next j
        Next i

        If (opt = 1) Then
            'move integer values to their sorted positions
            For i = 1 To cnt
                If (pos(i) <> i) Then
                    'need to move ints, first save whats in target space
                    itmp = iVal(i)
                    'move sorted data to target position
                    iVal(i) = iVal(pos(i))
                    'move temp data to source position
                    iVal(pos(i)) = itmp
                    'find the pointer to the other value we are moving
                    j = i
                    Do
                        j = j + 1
                    Loop While (pos(j) <> i)
                    pos(j) = pos(i)
                    pos(i) = i
                End If
            Next i
        End If
    End Sub

End Class