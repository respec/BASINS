'Copyright 2006 AQUA TERRA Consultants - Royalty-free use permitted under open source license
Option Strict Off
Option Explicit On

Module modPollutants

    Public Sub modPollutantsBuild(ByRef aUci As HspfUci, ByRef aMsg As HspfMsg)
        'figure out how many p or i quals
        Dim lNQuals As Integer = 0
        For Each lOpn As HspfOperation In aUci.OpnBlks("PERLND").Ids
            Dim litemp As Integer
            If lOpn.TableExists("NQUALS") Then
                litemp = lOpn.Tables.Item("NQUALS").ParmValue("NQUAL")
            ElseIf lOpn.TableExists("QUAL-PROPS") Then
                litemp = 1
            End If
            If litemp > lNQuals Then
                lNQuals = litemp
            End If
        Next lOpn

        For Each lOpn As HspfOperation In aUci.OpnBlks("IMPLND").Ids
            Dim litemp As Integer
            If lOpn.TableExists("NQUALS") Then
                litemp = lOpn.Tables.Item("NQUALS").ParmValue("NQUAL")
            ElseIf lOpn.TableExists("QUAL-PROPS") Then
                litemp = 1
            End If
            If litemp > lNQuals Then
                lNQuals = litemp
            End If
        Next lOpn

        'figure out how many gquals
        Dim lnGqual As Integer = 0
        For Each lOpn As HspfOperation In aUci.OpnBlks("RCHRES").Ids
            If lOpn.TableExists("GQ-QALDATA") Then
                Dim litemp As Integer
                If lOpn.TableExists("GQ-GENDATA") Then
                    litemp = lOpn.Tables.Item("GQ-GENDATA").ParmValue("NGQUAL")
                Else
                    litemp = 1
                End If
                If litemp > lnGqual Then
                    lnGqual = litemp
                End If
            End If
        Next lOpn

        Dim lISedAssoc() As Integer = {}
        Dim lPSedAssoc() As Integer = {}
        If lNQuals > 0 Then
            'keep track of sed assoc flags
            For lPI As Integer = 1 To 2
                Dim lCtype As String = ""
                If lPI = 1 Then
                    lCtype = "PERLND"
                Else
                    lCtype = "IMPLND"
                End If
                Dim lSedCnt As Integer = 0
                For lQualIndex As Integer = 1 To lNQuals
                    Dim lTname As String
                    If lQualIndex > 1 Then
                        lTname = "QUAL-PROPS:" & lQualIndex
                    Else
                        lTname = "QUAL-PROPS"
                    End If
                    If aUci.OpnBlks(lCtype).TableExists(lTname) Then
                        Dim lSedFg As Integer = 0
                        For Each lOpn As HspfOperation In aUci.OpnBlks(lCtype).Ids
                            If lOpn.TableExists(lTname) Then
                                lSedFg = lOpn.Tables.Item(lTname).ParmValue("QSDFG")
                                Exit For
                            End If
                        Next lOpn
                        If lSedFg > 0 Then
                            lSedCnt = lSedCnt + 1
                            If lPI = 1 Then
                                ReDim Preserve lPSedAssoc(lSedCnt)
                                lPSedAssoc(lSedCnt) = lQualIndex
                            Else
                                ReDim Preserve lISedAssoc(lSedCnt)
                                lISedAssoc(lSedCnt) = lQualIndex
                            End If
                        End If
                    End If
                Next
            Next lPI
        End If

        If lnGqual > 0 Then 'big loop for each gqual
            For lGQIndex As Integer = 1 To lnGqual
                'create new instance of a pollutant
                Dim ltPoll As New HspfPollutant
                Dim lpdone As Boolean = False
                Dim lidone As Boolean = False
                Dim lrdone As Boolean = False

                'look through all mass-links
                Dim lMLIndex As Integer = 0
                Do While lMLIndex < aUci.MassLinks.Count
                    Dim lML As HspfMassLink = aUci.MassLinks(lMLIndex)
                    If (lML.Target.Member = "IDQAL" And lML.Target.MemSub1 = lGQIndex) Or (lML.Target.Member = "ISQAL" And lML.Target.MemSub2 = lGQIndex) Then

                        ltPoll.MassLinks.Add(lML)
                        aUci.MassLinks.RemoveAt(lMLIndex)

                        If (lML.Source.Group = "PQUAL" Or lML.Source.Group = "IQUAL") Then
                            'this is type p-i-g
                            ltPoll.ModelType = "PIG"
                            Dim lCtype As String = ""
                            If lML.Source.Group = "PQUAL" Then
                                lCtype = "PERLND"
                            ElseIf lML.Source.Group = "IQUAL" Then
                                lCtype = "IMPLND"
                            End If
                            'found the p/i qual it is connected to
                            Dim lPIconn As Integer
                            Dim lNthsed As Integer
                            If lML.Target.Member <> "ISQAL" Then
                                lPIconn = lML.Source.MemSub1 'the normal case
                            Else
                                lNthsed = lML.Source.MemSub1 'figure out which qual this is
                                If lML.Source.Group = "PQUAL" Then
                                    lPIconn = lPSedAssoc(lNthsed)
                                Else
                                    lPIconn = lISedAssoc(lNthsed)
                                End If
                            End If

                            'remove tables and add to this collection
                            If (lCtype = "PERLND" And Not lpdone) Or (lCtype = "IMPLND" And Not lidone) Then
                                For Each lOpn As HspfOperation In aUci.OpnBlks(lCtype).Ids
                                    Dim lNewOpn As New HspfOperation
                                    lNewOpn.Id = lOpn.Id
                                    lNewOpn.Name = lOpn.Name
                                    ltPoll.Operations.Add(lCtype & lNewOpn.Id, lNewOpn)
                                    For Each lTableDef As HspfTableDef In aMsg.BlockDefs(lCtype).SectionDefs(Left(lCtype, 1) & "QUAL").TableDefs
                                        Dim ltname As String
                                        If lPIconn > 1 Then
                                            ltname = lTableDef.Name & ":" & lPIconn
                                        Else
                                            ltname = lTableDef.Name
                                        End If
                                        If lOpn.TableExists(ltname) Then
                                            Dim lTable As HspfTable = lOpn.Tables.Item(ltname)
                                            ltPoll.Operations.Item(lCtype & lNewOpn.Id).Tables.Add(lTable)
                                            lOpn.Tables.Remove((ltname))
                                        End If
                                        If aUci.OpnBlks(lCtype).TableExists(ltname) Then
                                            aUci.OpnBlks(lCtype).Tables.Remove(ltname)
                                        End If
                                    Next lTableDef
                                    If Len(ltPoll.Name) = 0 Then
                                        ltPoll.Name = ltPoll.Operations.Item(lCtype & lNewOpn.Id).Tables("QUAL-PROPS").ParmValue("QUALID")
                                        ltPoll.Index = lPIconn
                                    End If
                                Next lOpn
                                If lCtype = "PERLND" Then
                                    lpdone = True
                                ElseIf lCtype = "IMPLND" Then
                                    lidone = True
                                End If
                            End If
                        Else
                            If Len(ltPoll.ModelType) = 0 Then
                                ltPoll.ModelType = "GOnly"
                            End If
                        End If

                        If Not lrdone Then
                            Dim lCtype As String = "RCHRES"
                            For Each lOpn As HspfOperation In aUci.OpnBlks(lCtype).Ids
                                Dim lNewOpn As New HspfOperation
                                lNewOpn.Id = lOpn.Id
                                lNewOpn.Name = lOpn.Name
                                ltPoll.Operations.Add(lCtype & lNewOpn.Id, lNewOpn)
                                For Each lTableDef As HspfTableDef In aMsg.BlockDefs(lCtype).SectionDefs("GQUAL").TableDefs
                                    Dim ltname As String
                                    If lGQIndex > 1 Then
                                        ltname = lTableDef.Name & ":" & lGQIndex
                                    Else
                                        ltname = lTableDef.Name
                                    End If
                                    If lOpn.TableExists(ltname) Then
                                        Dim lTable As HspfTable = lOpn.Tables.Item(ltname)
                                        ltPoll.Operations.Item(lCtype & lNewOpn.Id).Tables.Add(lTable)
                                        lOpn.Tables.Remove((ltname))
                                        If aUci.OpnBlks(lCtype).TableExists(ltname) Then
                                            aUci.OpnBlks(lCtype).Tables.Remove(ltname)
                                        End If
                                    End If
                                Next lTableDef
                                If Len(ltPoll.Name) = 0 Then
                                    ltPoll.Name = ltPoll.Operations.Item(lCtype & lNewOpn.Id).Tables("GQ-QALDATA").ParmValue("GQID")
                                    ltPoll.Index = lGQIndex
                                End If
                            Next lOpn
                            lrdone = True
                        End If
                    Else
                        lMLIndex = lMLIndex + 1
                    End If
                Loop

                If Len(ltPoll.ModelType) > 0 Then
                    If aUci.Pollutants.Count > 0 Then
                        ltPoll.Id = aUci.Pollutants.Count + 1
                    Else
                        ltPoll.Id = 1
                    End If
                    aUci.Pollutants.Add(ltPoll)
                End If
            Next lGQIndex

        End If

        If lNQuals > 0 Then 'big loop for each p or i qual not to gqual

            For lQIndex As Integer = 1 To lNQuals
                'create new instance of a pollutant
                Dim ltPoll As New HspfPollutant
                Dim lpdone As Boolean = False
                Dim lidone As Boolean = False

                'look through all mass-links
                Dim lMLIndex As Integer = 0
                Do While lMLIndex < aUci.MassLinks.Count
                    Dim lML As HspfMassLink = aUci.MassLinks(lMLIndex)
                    If ((lML.Source.Group = "PQUAL" And lML.Source.MemSub1 = lQIndex) Or (lML.Source.Group = "IQUAL" And lML.Source.MemSub1 = lQIndex)) And lML.Target.VolName = "RCHRES" Then

                        ltPoll.MassLinks.Add(lML)
                        aUci.MassLinks.RemoveAt(lMLIndex)

                        ltPoll.ModelType = "PIOnly"
                        Dim lCtype As String = ""
                        If lML.Source.Group = "PQUAL" Then
                            lCtype = "PERLND"
                        ElseIf lML.Source.Group = "IQUAL" Then
                            lCtype = "IMPLND"
                        End If

                        'remove tables and add to this collection
                        If (lCtype = "PERLND" And Not lpdone) Or (lCtype = "IMPLND" And Not lidone) Then
                            For Each lOpn As HspfOperation In aUci.OpnBlks(lCtype).Ids
                                Dim newOpn As New HspfOperation
                                newOpn.Id = lOpn.Id
                                newOpn.Name = lOpn.Name
                                ltPoll.Operations.Add(lCtype & newOpn.Id, newOpn)
                                For Each lTableDef As HspfTableDef In aMsg.BlockDefs(lCtype).SectionDefs(Left(lCtype, 1) & "QUAL").TableDefs
                                    Dim tname As String
                                    If lQIndex > 1 Then
                                        tname = lTableDef.Name & ":" & lQIndex
                                    Else
                                        tname = lTableDef.Name
                                    End If
                                    If lOpn.TableExists(tname) Then
                                        Dim lTable As HspfTable = lOpn.Tables.Item(tname)
                                        ltPoll.Operations.Item(lCtype & newOpn.Id).Tables.Add(lTable)
                                        lOpn.Tables.Remove((tname))
                                    End If
                                    If aUci.OpnBlks(lCtype).TableExists(tname) Then
                                        aUci.OpnBlks(lCtype).Tables.Remove(tname)
                                    End If
                                Next lTableDef
                                If Len(ltPoll.Name) = 0 Then
                                    ltPoll.Name = ltPoll.Operations.Item(lCtype & newOpn.Id).Tables("QUAL-PROPS").ParmValue("QUALID")
                                    ltPoll.Index = lQIndex
                                End If
                            Next lOpn
                            If lCtype = "PERLND" Then
                                lpdone = True
                            ElseIf lCtype = "IMPLND" Then
                                lidone = True
                            End If
                        End If
                    Else
                        lMLIndex = lMLIndex + 1
                    End If
                Loop

                If Len(ltPoll.ModelType) > 0 Then
                    If aUci.Pollutants.Count > 0 Then
                        ltPoll.Id = aUci.Pollutants.Count + 1
                    Else
                        ltPoll.Id = 1
                    End If
                    aUci.Pollutants.Add(ltPoll)
                End If
            Next lQIndex

        End If

    End Sub

    Public Sub modPollutantsUnBuild(ByRef aUci As HspfUci, ByRef myMsg As HspfMsg)

        Dim lchanged As Boolean = False
        If aUci.Edited Then
            lchanged = True
        End If

        'find total number of each type
        Dim lPqualCnt As Integer = 0
        Dim lIqualCnt As Integer = 0
        Dim lGqualCnt As Integer = 0
        Dim lPoll As HspfPollutant
        For Each lPoll In aUci.Pollutants
            Dim liflag As Integer = 0
            Dim lpflag As Integer = 0
            Dim lrflag As Integer = 0
            If Len(lPoll.ModelType) > 0 Then
                For Each lnewOpn As HspfOperation In lPoll.Operations.Values
                    If lnewOpn.Name = "IMPLND" Then
                        If lPoll.ModelType = "PIOnly" Or lPoll.ModelType = "PIG" Then
                            liflag = 1
                        End If
                    ElseIf lnewOpn.Name = "PERLND" Then
                        If lPoll.ModelType = "PIOnly" Or lPoll.ModelType = "PIG" Then
                            lpflag = 1
                        End If
                    ElseIf lnewOpn.Name = "RCHRES" Then
                        If lPoll.ModelType = "GOnly" Or lPoll.ModelType = "PIG" Then
                            lrflag = 1
                        End If
                    End If
                Next
                lIqualCnt = lIqualCnt + liflag
                lPqualCnt = lPqualCnt + lpflag
                lGqualCnt = lGqualCnt + lrflag
            End If
        Next

        'remove any lingering mls from pquals or iquals
        Dim lIndex As Integer = 0
        Do While lIndex < aUci.MassLinks.Count
            Dim lML As HspfMassLink = aUci.MassLinks(lIndex)
            If lML.Source.VolName = "PERLND" And lML.Source.Group = "PQUAL" And lML.Target.VolName = "RCHRES" Then
                aUci.MassLinks.RemoveAt(lIndex)
            ElseIf lML.Source.VolName = "IMPLND" And lML.Source.Group = "IQUAL" And lML.Target.VolName = "RCHRES" Then
                aUci.MassLinks.RemoveAt(lIndex)
            Else
                lIndex += 1
            End If
        Loop

        Dim lnPqual As Integer = 0
        Dim lnIqual As Integer = 0
        Dim lnGqual As Integer = 0
        Dim lisedcnt As Integer = 0
        Dim lpsedcnt As Integer = 0
        Dim lCName As String = ""
        Do While aUci.Pollutants.Count > 0
            'find lowest index to preserve order of pollutants
            Dim lLowestIndex As Integer = 999
            Dim lLowestPos As Integer = 1
            Dim lCounter As Integer = 0
            For Each lPoll In aUci.Pollutants
                lCounter = lCounter + 1
                If lPoll.Index < lLowestIndex Then
                    lLowestIndex = lPoll.Index
                    lLowestPos = lCounter
                End If
            Next
            lPoll = aUci.Pollutants(lLowestPos - 1)

            Dim lIflag As Integer = 0
            Dim lPflag As Integer = 0
            Dim lRflag As Integer = 0
            If Len(lPoll.ModelType) > 0 Then
                'put tables back
                For Each lnewOpn As HspfOperation In lPoll.Operations.Values
                    If lnewOpn.Name = "IMPLND" Then
                        If lPoll.ModelType = "PIOnly" Or lPoll.ModelType = "PIG" Then
                            lIflag = 1
                        End If
                    ElseIf lnewOpn.Name = "PERLND" Then
                        If lPoll.ModelType = "PIOnly" Or lPoll.ModelType = "PIG" Then
                            lPflag = 1
                        End If
                    ElseIf lnewOpn.Name = "RCHRES" Then
                        If lPoll.ModelType = "GOnly" Or lPoll.ModelType = "PIG" Then
                            lRflag = 1
                        End If
                    End If
                Next
                lnIqual = lnIqual + lIflag
                lnPqual = lnPqual + lPflag
                lnGqual = lnGqual + lRflag
                Dim lIsedfg As Integer = 0
                Dim lPsedfg As Integer = 0
                For Each lnewOpn As HspfOperation In lPoll.Operations.Values
                    Dim lCtype As String = lnewOpn.Name
                    Dim lOpn As HspfOperation = aUci.OpnBlks(lnewOpn.Name).Ids("K" & lnewOpn.Id)
                    For Each lTable As HspfTable In lnewOpn.Tables
                        If lCtype = "IMPLND" Then
                            If lTable.Name <> "NQUALS" And lTable.Name <> "IQL-AD-FLAGS" And lTable.Name <> "LAT-FACTOR" Then
                                If lTable.TableNeededForAllQuals Then
                                    lTable.OccurNum = lnIqual
                                    lTable.OccurCount = lIqualCnt
                                    lTable.OccurIndex = 0
                                Else
                                    lTable.OccurIndex = lnIqual
                                End If
                            End If
                            If lTable.Name = "QUAL-PROPS" Then
                                lIsedfg = lTable.Parms("QSDFG").Value
                            End If
                        ElseIf lCtype = "PERLND" Then
                            If lTable.Name <> "NQUALS" And lTable.Name <> "PQL-AD-FLAGS" And lTable.Name <> "LAT-FACTOR" Then '(these three can only appear once)
                                If lTable.TableNeededForAllQuals Then
                                    lTable.OccurNum = lnPqual
                                    lTable.OccurCount = lPqualCnt
                                    lTable.OccurIndex = 0
                                Else
                                    lTable.OccurIndex = lnPqual
                                End If
                            End If
                            If lTable.Name = "QUAL-PROPS" Then
                                lPsedfg = lTable.Parms("QSDFG").Value
                            End If
                        ElseIf lCtype = "RCHRES" Then
                            If lTable.Name <> "GQ-GENDATA" Then
                                If lTable.TableNeededForAllQuals Then
                                    lTable.OccurNum = lnGqual
                                    lTable.OccurCount = lGqualCnt
                                    lTable.OccurIndex = 0
                                Else
                                    lTable.OccurIndex = lnGqual
                                End If
                            End If
                        End If

                        Dim lMaxOccur As Integer = myMsg.BlockDefs(lCtype).TableDefs(lTable.Name).NumOccur
                        Dim lSecName As String = myMsg.BlockDefs(lCtype).TableDefs(lTable.Name).Parent.Name
                        Dim lSecId As Integer = myMsg.BlockDefs(lCtype).SectionID(lSecName)
                        'calc count for this table
                        If Not lTable.TableNeededForAllQuals Then
                            Dim lOccurCount As Integer = 1
                            For Each lTempTable As HspfTable In lOpn.Tables
                                If lTempTable.Name = lTable.Name Then
                                    lOccurCount = lOccurCount + 1
                                End If
                            Next
                            lTable.OccurNum = lOccurCount
                            If lOccurCount <= lMaxOccur Then
                                'can add another
                                lTable.OccurCount = lOccurCount
                                For Each lTempTable As HspfTable In lOpn.Tables
                                    If lTempTable.Name = lTable.Name Then
                                        'set occurence count for previous tables
                                        lTempTable.OccurCount = lOccurCount
                                    End If
                                Next
                            End If
                        End If

                        If lTable.OccurNum <= lMaxOccur Then
                            'can add another
                            If lTable.OccurNum > 1 Then
                                lCName = lTable.Name & ":" & lTable.OccurNum
                            Else
                                lCName = lTable.Name
                            End If
                            lOpn.Tables.Add(lTable)
                            If lOpn.TableExists("ACTIVITY") Then
                                With lOpn.Tables.Item("ACTIVITY")
                                    .Parms(lSecId - 1).Value = 1 'turn on this section
                                    'turn on other prerequisite sections
                                    Select Case lOpn.Name
                                        Case "RCHRES"
                                            For lIndex = 6 To lSecId - 1
                                                .Parms(lIndex).Value = 1 'previous rqual sections must be on
                                            Next
                                            If lSecId > 1 Then
                                                .Parms(0).Value = 1 'hydr must be on
                                            End If
                                            If lSecId > 2 Then
                                                .Parms(1).Value = 1 'adcalc must be on
                                            End If
                                            If lSecId > 4 Then
                                                .Parms(3).Value = 1 'htrch must be on
                                            End If
                                        Case "IMPLND"
                                            If lSecId = 5 Or lSecId = 2 Then
                                                .Parms(0).Value = 1 'atemp must be on
                                            End If
                                            If lSecId > 3 Then
                                                .Parms(2).Value = 1 'iwater must be on
                                            End If
                                        Case "PERLND"
                                            If lSecId > 8 Then
                                                .Parms(7).Value = 1 'mstlay must be on
                                            End If
                                            If lSecId = 5 Or lSecId = 2 Or lSecId = 6 Or lSecId = 10 Or lSecId = 11 Or lSecId = 12 Then
                                                .Parms(0).Value = 1 'atemp must be on
                                            End If
                                            If lSecId = 4 Or lSecId = 6 Or lSecId = 7 Or lSecId = 9 Or lSecId = 10 Or lSecId = 11 Then
                                                .Parms(2).Value = 1 'pwater must be on
                                            End If
                                            If lSecId = 6 Or lSecId = 10 Or lSecId = 11 Then
                                                .Parms(4).Value = 1 'pstemp must be on
                                            End If
                                    End Select
                                End With
                            End If

                            If Not aUci.OpnBlks(lCtype).TableExists(lCName) Then
                                aUci.OpnBlks(lCtype).Tables.Add(lTable)
                                If lTable.OccurNum > 1 And lTable.TableNeededForAllQuals Then
                                    'make sure all previous occurs of this table exist
                                    For lIndex = 1 To lTable.OccurNum - 1
                                        If lIndex > 1 Then
                                            lCName = lTable.Name & ":" & lIndex
                                        Else
                                            lCName = lTable.Name
                                        End If
                                        If Not aUci.OpnBlks(lCtype).TableExists(lCName) Then
                                            Dim ltempTable As New HspfTable
                                            ltempTable.Opn = lOpn
                                            ltempTable.Def = myMsg.BlockDefs(lCtype).TableDefs(lTable.Name)
                                            Dim ls As String = ""
                                            ltempTable.InitTable((ls))
                                            ltempTable.OccurNum = lIndex
                                            ltempTable.OccurCount = lTable.OccurCount
                                            aUci.OpnBlks(lCtype).Tables.Add(ltempTable)
                                        End If
                                    Next
                                End If
                            End If
                        End If
                    Next
                Next
                If lIsedfg = 1 Then
                    lisedcnt = lisedcnt + 1
                End If
                If lPsedfg = 1 Then
                    lpsedcnt = lpsedcnt + 1
                End If

                'put masslinks back
                For Each lML As HspfMassLink In lPoll.MassLinks
                    If lML.Source.Group = "PQUAL" Then
                        If lML.Target.Member = "ISQAL" Then
                            lML.Source.MemSub1 = lpsedcnt
                        Else
                            lML.Source.MemSub1 = lnPqual
                        End If
                    ElseIf lML.Source.Group = "IQUAL" Then
                        If lML.Target.Member = "ISQAL" Then
                            lML.Source.MemSub1 = lisedcnt
                        Else
                            lML.Source.MemSub1 = lnIqual
                        End If
                    End If
                    If lML.Target.Member = "IDQAL" Then
                        lML.Target.MemSub1 = lnGqual
                    ElseIf lML.Target.Member = "ISQAL" Then
                        lML.Target.MemSub2 = lnGqual
                    End If
                    'make sure there isnt already a ml to this target
                    Dim lIExist As Integer = -1
                    For lIndex = 1 To aUci.MassLinks.Count
                        Dim lTempML As HspfMassLink = aUci.MassLinks(lIndex - 1)
                        If lTempML.Source.VolName = lML.Source.VolName And lTempML.Target.VolName = lML.Target.VolName And lTempML.Target.Group = lML.Target.Group And lTempML.Target.Member = lML.Target.Member And lTempML.Target.MemSub1 = lML.Target.MemSub1 And lTempML.Target.MemSub2 = lML.Target.MemSub2 Then
                            If lTempML.Source.Group = lML.Source.Group And lTempML.Source.Member = lML.Source.Member And lTempML.Source.MemSub1 = lML.Source.MemSub1 And lTempML.Source.MemSub2 = lML.Source.MemSub2 Then
                                'exact duplicate
                                lIExist = lIndex
                            ElseIf lTempML.Source.Group <> lML.Source.Group Then
                                'different source group, remove it
                                lIExist = lIndex
                            End If
                        End If
                    Next
                    If lIExist > -1 Then
                        aUci.MassLinks.RemoveAt(lIExist - 1)
                    End If
                    aUci.MassLinks.Add(lML)
                Next
            End If
            aUci.Pollutants.RemoveAt(lLowestPos - 1)
        Loop

        'set nquals
        lCName = "PERLND"
        For Each lOpn As HspfOperation In aUci.OpnBlks(lCName).Ids
            If lOpn.TableExists("NQUALS") Then
                lOpn.Tables.Item("NQUALS").ParmValue("NQUAL") = lnPqual
            Else
                If lnPqual > 0 Then
                    aUci.OpnBlks(lCName).AddTableForAll("NQUALS", lCName)
                    lOpn.Tables.Item("NQUALS").ParmValue("NQUAL") = lnPqual
                End If
            End If
            If lnPqual = 0 Then
                lOpn.Tables.Item("ACTIVITY").ParmValue("PQALFG") = 0
            End If
        Next
        lCName = "IMPLND"
        For Each lOpn As HspfOperation In aUci.OpnBlks(lCName).Ids
            If lOpn.TableExists("NQUALS") Then
                lOpn.Tables.Item("NQUALS").ParmValue("NQUAL") = lnIqual
            Else
                If lnIqual > 0 Then
                    aUci.OpnBlks(lCName).AddTableForAll("NQUALS", lCName)
                    lOpn.Tables.Item("NQUALS").ParmValue("NQUAL") = lnIqual
                End If
            End If
            If lnIqual = 0 Then
                lOpn.Tables.Item("ACTIVITY").ParmValue("IQALFG") = 0
            End If
        Next
        lCName = "RCHRES"
        For Each lOpn As HspfOperation In aUci.OpnBlks(lCName).Ids
            If lOpn.TableExists("GQ-GENDATA") Then
                lOpn.Tables.Item("GQ-GENDATA").ParmValue("NGQUAL") = lnGqual
            Else
                If lnGqual > 0 Then
                    aUci.OpnBlks(lCName).AddTableForAll("GQ-GENDATA", lCName)
                    lOpn.Tables.Item("GQ-GENDATA").ParmValue("NGQUAL") = lnGqual
                End If
            End If
            If lnGqual = 0 Then
                lOpn.Tables.Item("ACTIVITY").ParmValue("GQALFG") = 0
            End If
        Next

        If lchanged Then
            aUci.Edited = True
        Else
            aUci.Edited = False
        End If
    End Sub
End Module