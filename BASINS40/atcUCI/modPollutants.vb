'Copyright 2006 AQUA TERRA Consultants - Royalty-free use permitted under open source license
Option Strict Off
Option Explicit On

Module modPollutants
    Public Sub modPollutantsBuild(ByRef aUci As HspfUci, ByRef aMsg As HspfMsg)
        'figure out how many p or i quals
        Dim nquals As Integer = 0
        For Each lOpn As HspfOperation In aUci.OpnBlks("PERLND").Ids
            Dim itemp As Integer
            If lOpn.TableExists("NQUALS") Then
                itemp = lOpn.Tables.Item("NQUALS").ParmValue("NQUAL")
            ElseIf lOpn.TableExists("QUAL-PROPS") Then
                itemp = 1
            End If
            If itemp > nquals Then
                nquals = itemp
            End If
        Next lOpn

        For Each lOpn As HspfOperation In aUci.OpnBlks("IMPLND").Ids
            Dim itemp As Integer
            If lOpn.TableExists("NQUALS") Then
                itemp = lOpn.Tables.Item("NQUALS").ParmValue("NQUAL")
            ElseIf lOpn.TableExists("QUAL-PROPS") Then
                itemp = 1
            End If
            If itemp > nquals Then
                nquals = itemp
            End If
        Next lOpn

        'figure out how many gquals
        Dim nGqual As Integer = 0
        For Each lOpn As HspfOperation In aUci.OpnBlks("RCHRES").Ids
            If lOpn.TableExists("GQ-QALDATA") Then
                Dim itemp As Integer
                If lOpn.TableExists("GQ-GENDATA") Then
                    itemp = lOpn.Tables.Item("GQ-GENDATA").ParmValue("NGQUAL")
                Else
                    itemp = 1
                End If
                If itemp > nGqual Then
                    nGqual = itemp
                End If
            End If
        Next lOpn

        Dim isedassoc() As Integer = {}
        Dim psedassoc() As Integer = {}
        If nquals > 0 Then
            'keep track of sed assoc flags
            For j As Integer = 1 To 2
                Dim lCtype As String = ""
                If j = 1 Then
                    lCtype = "PERLND"
                Else
                    lCtype = "IMPLND"
                End If
                Dim sedcnt As Integer = 0
                For i As Integer = 1 To nquals
                    Dim tname As String
                    If i > 1 Then
                        tname = "QUAL-PROPS:" & i
                    Else
                        tname = "QUAL-PROPS"
                    End If
                    If aUci.OpnBlks(lCtype).TableExists(tname) Then
                        Dim sedfg As Integer = 0
                        For Each lOpn As HspfOperation In aUci.OpnBlks(lCtype).Ids
                            If lOpn.TableExists(tname) Then
                                sedfg = lOpn.Tables.Item(tname).ParmValue("QSDFG")
                                Exit For
                            End If
                        Next lOpn
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

        If nGqual > 0 Then 'big loop for each gqual
            For k As Integer = 1 To nGqual
                'create new instance of a pollutant
                Dim tPoll As New HspfPollutant
                Dim pdone As Boolean = False
                Dim idone As Boolean = False
                Dim rdone As Boolean = False

                'look through all mass-links
                Dim j As Integer = 0
                Do While j <= aUci.MassLinks.Count
                    Dim lML As HspfMassLink = aUci.MassLinks(j)
                    If (lML.Target.Member = "IDQAL" And lML.Target.MemSub1 = k) Or (lML.Target.Member = "ISQAL" And lML.Target.MemSub2 = k) Then

                        tPoll.MassLinks.Add(lML)
                        aUci.MassLinks.RemoveAt(j)

                        If (lML.Source.Group = "PQUAL" Or lML.Source.Group = "IQUAL") Then
                            'this is type p-i-g
                            tPoll.ModelType = "PIG"
                            Dim lCtype As String = ""
                            If lML.Source.Group = "PQUAL" Then
                                lCtype = "PERLND"
                            ElseIf lML.Source.Group = "IQUAL" Then
                                lCtype = "IMPLND"
                            End If
                            'found the p/i qual it is connected to
                            Dim piconn As Integer
                            Dim nthsed As Integer
                            If lML.Target.Member <> "ISQAL" Then
                                piconn = lML.Source.MemSub1 'the normal case
                            Else
                                nthsed = lML.Source.MemSub1 'figure out which qual this is
                                If lML.Source.Group = "PQUAL" Then
                                    piconn = psedassoc(nthsed)
                                Else
                                    piconn = isedassoc(nthsed)
                                End If
                            End If

                            'remove tables and add to this collection
                            If (lCtype = "PERLND" And Not pdone) Or (lCtype = "IMPLND" And Not idone) Then
                                For Each lOpn As HspfOperation In aUci.OpnBlks(lCtype).Ids
                                    Dim newOpn As New HspfOperation
                                    newOpn.Id = lOpn.Id
                                    newOpn.Name = lOpn.Name
                                    tPoll.Operations.Add(lCtype & newOpn.Id, newOpn)
                                    For Each lTableDef As HspfTableDef In aMsg.BlockDefs(lCtype).SectionDefs(Left(lCtype, 1) & "QUAL").TableDefs
                                        Dim tname As String
                                        If piconn > 1 Then
                                            tname = lTableDef.Name & ":" & piconn
                                        Else
                                            tname = lTableDef.Name
                                        End If
                                        If lOpn.TableExists(tname) Then
                                            Dim lTable As HspfTable = lOpn.Tables.Item(tname)
                                            tPoll.Operations.Item(lCtype & newOpn.Id).Tables.Add(lTable)
                                            lOpn.Tables.Remove((tname))
                                        End If
                                        If aUci.OpnBlks(lCtype).TableExists(tname) Then
                                            aUci.OpnBlks(lCtype).Tables.Remove(tname)
                                        End If
                                    Next lTableDef
                                    If Len(tPoll.Name) = 0 Then
                                        tPoll.Name = tPoll.Operations.Item(lCtype & newOpn.Id).Tables("QUAL-PROPS").ParmValue("QUALID")
                                        tPoll.Index = piconn
                                    End If
                                Next lOpn
                                If lCtype = "PERLND" Then
                                    pdone = True
                                ElseIf lCtype = "IMPLND" Then
                                    idone = True
                                End If
                            End If
                        Else
                            If Len(tPoll.ModelType) = 0 Then
                                tPoll.ModelType = "GOnly"
                            End If
                        End If

                        If Not rdone Then
                            Dim lCtype As String = "RCHRES"
                            For Each lOpn As HspfOperation In aUci.OpnBlks(lCtype).Ids
                                Dim newOpn As New HspfOperation
                                newOpn.Id = lOpn.Id
                                newOpn.Name = lOpn.Name
                                tPoll.Operations.Add(lCtype & newOpn.Id, newOpn)
                                For Each lTableDef As HspfTableDef In aMsg.BlockDefs(lCtype).SectionDefs("GQUAL").TableDefs
                                    Dim tname As String
                                    If k > 1 Then
                                        tname = lTableDef.Name & ":" & k
                                    Else
                                        tname = lTableDef.Name
                                    End If
                                    If lOpn.TableExists(tname) Then
                                        Dim lTable As HspfTable = lOpn.Tables.Item(tname)
                                        tPoll.Operations.Item(lCtype & newOpn.Id).Tables.Add(lTable)
                                        lOpn.Tables.Remove((tname))
                                        If aUci.OpnBlks(lCtype).TableExists(tname) Then
                                            aUci.OpnBlks(lCtype).Tables.Remove(tname)
                                        End If
                                    End If
                                Next lTableDef
                                If Len(tPoll.Name) = 0 Then
                                    tPoll.Name = tPoll.Operations.Item(lCtype & newOpn.Id).Tables("GQ-QALDATA").ParmValue("GQID")
                                    tPoll.Index = k
                                End If
                            Next lOpn
                            rdone = True
                        End If
                    Else
                        j = j + 1
                    End If
                Loop

                If Len(tPoll.ModelType) > 0 Then
                    If aUci.Pollutants.Count > 0 Then
                        tPoll.Id = aUci.Pollutants.Count + 1
                    Else
                        tPoll.Id = 1
                    End If
                    aUci.Pollutants.Add(tPoll)
                End If
            Next k

        End If

        If nquals > 0 Then 'big loop for each p or i qual not to gqual

            For k As Integer = 1 To nquals
                'create new instance of a pollutant
                Dim tPoll As New HspfPollutant
                Dim pdone As Boolean = False
                Dim idone As Boolean = False

                'look through all mass-links
                Dim j As Integer = 0
                Do While j <= aUci.MassLinks.Count
                    Dim lML As HspfMassLink = aUci.MassLinks(j)
                    If ((lML.Source.Group = "PQUAL" And lML.Source.MemSub1 = k) Or (lML.Source.Group = "IQUAL" And lML.Source.MemSub1 = k)) And lML.Target.VolName = "RCHRES" Then

                        tPoll.MassLinks.Add(lML)
                        aUci.MassLinks.RemoveAt(j)

                        tPoll.ModelType = "PIOnly"
                        Dim lCtype As String = ""
                        If lML.Source.Group = "PQUAL" Then
                            lCtype = "PERLND"
                        ElseIf lML.Source.Group = "IQUAL" Then
                            lCtype = "IMPLND"
                        End If

                        'remove tables and add to this collection
                        If (lCtype = "PERLND" And Not pdone) Or (lCtype = "IMPLND" And Not idone) Then
                            For Each lOpn As HspfOperation In aUci.OpnBlks(lCtype).Ids
                                Dim newOpn As New HspfOperation
                                newOpn.Id = lOpn.Id
                                newOpn.Name = lOpn.Name
                                tPoll.Operations.Add(lCtype & newOpn.Id, newOpn)
                                For Each lTableDef As HspfTableDef In aMsg.BlockDefs(lCtype).SectionDefs(Left(lCtype, 1) & "QUAL").TableDefs
                                    Dim tname As String
                                    If k > 1 Then
                                        tname = lTableDef.Name & ":" & k
                                    Else
                                        tname = lTableDef.Name
                                    End If
                                    If lOpn.TableExists(tname) Then
                                        Dim lTable As HspfTable = lOpn.Tables.Item(tname)
                                        tPoll.Operations.Item(lCtype & newOpn.Id).Tables.Add(lTable)
                                        lOpn.Tables.Remove((tname))
                                    End If
                                    If aUci.OpnBlks(lCtype).TableExists(tname) Then
                                        aUci.OpnBlks(lCtype).Tables.Remove(tname)
                                    End If
                                Next lTableDef
                                If Len(tPoll.Name) = 0 Then
                                    tPoll.Name = tPoll.Operations.Item(lCtype & newOpn.Id).Tables("QUAL-PROPS").ParmValue("QUALID")
                                    tPoll.Index = k
                                End If
                            Next lOpn
                            If lCtype = "PERLND" Then
                                pdone = True
                            ElseIf lCtype = "IMPLND" Then
                                idone = True
                            End If
                        End If
                    Else
                        j = j + 1
                    End If
                Loop

                If Len(tPoll.ModelType) > 0 Then
                    If aUci.Pollutants.Count > 0 Then
                        tPoll.Id = aUci.Pollutants.Count + 1
                    Else
                        tPoll.Id = 1
                    End If
                    aUci.Pollutants.Add(tPoll)
                End If
            Next k

        End If

    End Sub

    Public Sub modPollutantsUnBuild(ByRef aUci As HspfUci, ByRef myMsg As HspfMsg)
        Dim lOpn As HspfOperation
        Dim i, j, nGqual As Integer
        Dim lML As HspfMassLink
        Dim tPoll As HspfPollutant
        Dim tML As HspfMassLink
        Dim newOpn As HspfOperation
        Dim lCtype As String
        Dim ltable, tempTable As HspfTable
        Dim cname As String
        Dim pflag, nIqual, nPqual, iflag, rflag As Integer
        Dim psedfg, isedfg, IqualCnt, PqualCnt, GqualCnt, isedcnt, psedcnt As Integer
        Dim OccurCount, lowestpos, lowestindex, Counter, MaxOccur As Integer
        Dim ttable As HspfTable
        Dim SecName As String
        Dim SecId As Integer
        Dim iexist As Integer
        Dim changed As Boolean
        Dim s As String

        If aUci.Edited = False Then
            changed = False
        Else
            changed = True
        End If

        'find total number of each type
        PqualCnt = 0
        IqualCnt = 0
        GqualCnt = 0
        For Each tPoll In aUci.Pollutants
            iflag = 0
            pflag = 0
            rflag = 0
            If Len(tPoll.ModelType) > 0 Then
                For Each newOpn In tPoll.Operations.Values
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
                Next
                IqualCnt = IqualCnt + iflag
                PqualCnt = PqualCnt + pflag
                GqualCnt = GqualCnt + rflag
            End If
        Next

        'remove any lingering mls from pquals or iquals
        i = 0
        Do While i < aUci.MassLinks.Count
            tML = aUci.MassLinks(i)
            If tML.Source.VolName = "PERLND" And tML.Source.Group = "PQUAL" And tML.Target.VolName = "RCHRES" Then
                aUci.MassLinks.RemoveAt(i)
            ElseIf tML.Source.VolName = "IMPLND" And tML.Source.Group = "IQUAL" And tML.Target.VolName = "RCHRES" Then
                aUci.MassLinks.RemoveAt(i)
            Else
                i = i + 1
            End If
        Loop

        nPqual = 0
        nIqual = 0
        nGqual = 0
        isedcnt = 0
        psedcnt = 0
        Do While aUci.Pollutants.Count > 0
            'find lowest index to preserve order of pollutants
            lowestindex = 999
            lowestpos = 1
            Counter = 0
            For Each tPoll In aUci.Pollutants
                Counter = Counter + 1
                If tPoll.Index < lowestindex Then
                    lowestindex = tPoll.Index
                    lowestpos = Counter
                End If
            Next
            tPoll = aUci.Pollutants(lowestpos)

            iflag = 0
            pflag = 0
            rflag = 0
            If Len(tPoll.ModelType) > 0 Then
                'put tables back
                For Each newOpn In tPoll.Operations.Values
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
                Next
                nIqual = nIqual + iflag
                nPqual = nPqual + pflag
                nGqual = nGqual + rflag
                isedfg = 0
                psedfg = 0
                For j = 1 To tPoll.Operations.Count
                    newOpn = tPoll.Operations.Item(j)
                    lCtype = newOpn.Name
                    lOpn = aUci.OpnBlks(newOpn.Name).Ids("K" & newOpn.Id)
                    For Each ltable In newOpn.Tables
                        If lCtype = "IMPLND" Then
                            If ltable.Name <> "NQUALS" And ltable.Name <> "IQL-AD-FLAGS" And ltable.Name <> "LAT-FACTOR" Then
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
                        ElseIf lCtype = "PERLND" Then
                            If ltable.Name <> "NQUALS" And ltable.Name <> "PQL-AD-FLAGS" And ltable.Name <> "LAT-FACTOR" Then '(these three can only appear once)
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
                        ElseIf lCtype = "RCHRES" Then
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

                        MaxOccur = myMsg.BlockDefs(lCtype).TableDefs(ltable.Name).NumOccur
                        SecName = myMsg.BlockDefs(lCtype).TableDefs(ltable.Name).Parent.Name
                        SecId = myMsg.BlockDefs(lCtype).SectionID(SecName)
                        'calc count for this table
                        If Not ltable.TableNeededForAllQuals Then
                            OccurCount = 1
                            For Each ttable In lOpn.Tables
                                If ttable.Name = ltable.Name Then
                                    OccurCount = OccurCount + 1
                                End If
                            Next
                            ltable.OccurNum = OccurCount
                            If OccurCount <= MaxOccur Then
                                'can add another
                                ltable.OccurCount = OccurCount
                                For Each ttable In lOpn.Tables
                                    If ttable.Name = ltable.Name Then
                                        'set occurence count for previous tables
                                        ttable.OccurCount = OccurCount
                                    End If
                                Next
                            End If
                        End If

                        If ltable.OccurNum <= MaxOccur Then
                            'can add another
                            If ltable.OccurNum > 1 Then
                                cname = ltable.Name & ":" & ltable.OccurNum
                            Else
                                cname = ltable.Name
                            End If
                            lOpn.Tables.Add(ltable)
                            If lOpn.TableExists("ACTIVITY") Then
                                With lOpn.Tables.Item("ACTIVITY")
                                    .Parms(SecId).Value = 1 'turn on this section
                                    'turn on other prerequisite sections
                                    Select Case lOpn.Name
                                        Case "RCHRES"
                                            For i = 7 To SecId - 1
                                                .Parms(i).Value = 1 'previous rqual sections must be on
                                            Next i
                                            If SecId > 1 Then
                                                .Parms(1).Value = 1 'hydr must be on
                                            End If
                                            If SecId > 2 Then
                                                .Parms(2).Value = 1 'adcalc must be on
                                            End If
                                            If SecId > 4 Then
                                                .Parms(4).Value = 1 'htrch must be on
                                            End If
                                        Case "IMPLND"
                                            If SecId = 5 Or SecId = 2 Then
                                                .Parms(1).Value = 1 'atemp must be on
                                            End If
                                            If SecId > 3 Then
                                                .Parms(3).Value = 1 'iwater must be on
                                            End If
                                        Case "PERLND"
                                            If SecId > 8 Then
                                                .Parms(8).Value = 1 'mstlay must be on
                                            End If
                                            If SecId = 5 Or SecId = 2 Or SecId = 6 Or SecId = 10 Or SecId = 11 Or SecId = 12 Then
                                                .Parms(1).Value = 1 'atemp must be on
                                            End If
                                            If SecId = 4 Or SecId = 6 Or SecId = 7 Or SecId = 9 Or SecId = 10 Or SecId = 11 Then
                                                .Parms(3).Value = 1 'pwater must be on
                                            End If
                                            If SecId = 6 Or SecId = 10 Or SecId = 11 Then
                                                .Parms(5).Value = 1 'pstemp must be on
                                            End If
                                    End Select
                                End With
                            End If

                            If Not aUci.OpnBlks(lCtype).TableExists(cname) Then
                                aUci.OpnBlks(lCtype).Tables.Add(ltable)
                                If ltable.OccurNum > 1 And ltable.TableNeededForAllQuals Then
                                    'make sure all previous occurs of this table exist
                                    For i = 1 To ltable.OccurNum - 1
                                        If i > 1 Then
                                            cname = ltable.Name & ":" & i
                                        Else
                                            cname = ltable.Name
                                        End If
                                        If Not aUci.OpnBlks(lCtype).TableExists(cname) Then
                                            tempTable = New HspfTable
                                            tempTable.Opn = lOpn
                                            tempTable.Def = myMsg.BlockDefs(lCtype).TableDefs(ltable.Name)
                                            s = ""
                                            tempTable.initTable((s))
                                            tempTable.OccurNum = i
                                            tempTable.OccurCount = ltable.OccurCount
                                            aUci.OpnBlks(lCtype).Tables.Add(tempTable)
                                        End If
                                    Next i
                                End If
                            End If
                        End If
                    Next
                Next j
                If isedfg = 1 Then
                    isedcnt = isedcnt + 1
                End If
                If psedfg = 1 Then
                    psedcnt = psedcnt + 1
                End If

                'put masslinks back
                For Each lML In tPoll.MassLinks
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
                    For i = 1 To aUci.MassLinks.Count
                        tML = aUci.MassLinks(i)
                        If tML.Source.VolName = lML.Source.VolName And tML.Target.VolName = lML.Target.VolName And tML.Target.Group = lML.Target.Group And tML.Target.Member = lML.Target.Member And tML.Target.MemSub1 = lML.Target.MemSub1 And tML.Target.MemSub2 = lML.Target.MemSub2 Then
                            If tML.Source.Group = lML.Source.Group And tML.Source.Member = lML.Source.Member And tML.Source.MemSub1 = lML.Source.MemSub1 And tML.Source.MemSub2 = lML.Source.MemSub2 Then
                                'exact duplicate
                                iexist = i
                            ElseIf tML.Source.Group <> lML.Source.Group Then
                                'different source group, remove it
                                iexist = i
                            End If
                        End If
                    Next i
                    If iexist > 0 Then
                        aUci.MassLinks.RemoveAt(iexist)
                    End If
                    aUci.MassLinks.Add(lML)
                Next
            End If
            aUci.Pollutants.RemoveAt(lowestpos)
        Loop

        'set nquals
        cname = "PERLND"
        For Each lOpn In aUci.OpnBlks(cname).Ids
            If lOpn.TableExists("NQUALS") Then
                lOpn.Tables.Item("NQUALS").ParmValue("NQUAL") = nPqual
            Else
                If nPqual > 0 Then
                    aUci.OpnBlks(cname).AddTableForAll("NQUALS", cname)
                    lOpn.Tables.Item("NQUALS").ParmValue("NQUAL") = nPqual
                End If
            End If
            If nPqual = 0 Then
                lOpn.Tables.Item("ACTIVITY").ParmValue("PQALFG") = 0
            End If
        Next
        cname = "IMPLND"
        For Each lOpn In aUci.OpnBlks(cname).Ids
            If lOpn.TableExists("NQUALS") Then
                lOpn.Tables.Item("NQUALS").ParmValue("NQUAL") = nIqual
            Else
                If nIqual > 0 Then
                    aUci.OpnBlks(cname).AddTableForAll("NQUALS", cname)
                    lOpn.Tables.Item("NQUALS").ParmValue("NQUAL") = nIqual
                End If
            End If
            If nIqual = 0 Then
                lOpn.Tables.Item("ACTIVITY").ParmValue("IQALFG") = 0
            End If
        Next
        cname = "RCHRES"
        For Each lOpn In aUci.OpnBlks(cname).Ids
            If lOpn.TableExists("GQ-GENDATA") Then
                lOpn.Tables.Item("GQ-GENDATA").ParmValue("NGQUAL") = nGqual
            Else
                If nGqual > 0 Then
                    aUci.OpnBlks(cname).AddTableForAll("GQ-GENDATA", cname)
                    lOpn.Tables.Item("GQ-GENDATA").ParmValue("NGQUAL") = nGqual
                End If
            End If
            If nGqual = 0 Then
                lOpn.Tables.Item("ACTIVITY").ParmValue("GQALFG") = 0
            End If
        Next

        If changed Then
            aUci.Edited = True
        Else
            aUci.Edited = False
        End If
    End Sub
End Module