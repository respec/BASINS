Imports atcUtility
Imports MapWinUtility
Imports atcUCI

Public Class frmPollutant

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.Icon = pIcon
        Me.MinimumSize = Me.Size

        If pUCI.Pollutants.Count = 0 Then
            ReadPollutants()
        End If

        For lIndex As Integer = 1 To pDefUCI.Pollutants.Count
            lstPollutants.Items.Add(pDefUCI.Pollutants(lIndex).Name)
            If pDefUCI.Pollutants(lIndex).ModelType = "DataIn" Then
                lstPollutants.SetSelected(lIndex, True)
            Else
                lstPollutants.SetSelected(lIndex, False)
            End If
        Next lIndex

        For lIndex As Integer = 1 To pUCI.Pollutants.Count
            lstPollutants.Items.Add(pUCI.Pollutants(lIndex).Name)
            lstPollutants.SetSelected(lIndex, True)
        Next lIndex

    End Sub

    Private Sub cmdOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOK.Click
        'Dim i, vPoll As Object, lPoll As HspfPollutant, found As Boolean
        'Dim j&, pcount&, icount&, rcount&

        ''make sure not too many
        'pcount = 0
        'icount = 0
        'rcount = 0
        'For i = 1 To aslGQual.RightCount
        '    For Each vPoll In myUci.Pollutants
        '        lPoll = vPoll
        '        If lPoll.Name = aslGQual.RightItem(i - 1) Then
        '            If lPoll.modeltype = "PIG" Then
        '                pcount = pcount + 1
        '                icount = icount + 1
        '                rcount = rcount + 1
        '            ElseIf lPoll.modeltype = "PIOnly" Then
        '                pcount = pcount + 1
        '                icount = icount + 1
        '            ElseIf lPoll.modeltype = "GOnly" Then
        '                rcount = rcount + 1
        '            End If
        '        End If
        '    Next vPoll
        '    For Each vPoll In defUci.Pollutants
        '        lPoll = vPoll
        '        If lPoll.Name = aslGQual.RightItem(i - 1) Then
        '            If lPoll.modeltype = "PIG" Then
        '                pcount = pcount + 1
        '                icount = icount + 1
        '                rcount = rcount + 1
        '            ElseIf lPoll.modeltype = "PIOnly" Then
        '                pcount = pcount + 1
        '                icount = icount + 1
        '            ElseIf lPoll.modeltype = "GOnly" Then
        '                rcount = rcount + 1
        '            End If
        '        End If
        '    Next vPoll
        'Next i
        'If pcount > 10 Or icount > 10 Then
        '    'too many p/i quals
        '    Index = -1
        '    myMsgBox.Show("The number of PERLND or IMPLND quality constituents exceeds the maximum." & vbCrLf & _
        '      "Remove some pollutants from the 'Selected' list.", "Pollutant Problem", "OK")
        'End If
        'If rcount > 3 Then
        '    'too many gquals
        '    Index = -1
        '    myMsgBox.Show("The number of General quality constituents exceeds the maximum." & vbCrLf & _
        '      "Remove some pollutants from the 'Selected' list.", "Pollutant Problem", "OK")
        'End If

        'If Index = 0 Then
        '    'okay
        '    For i = 1 To aslGQual.RightCount
        '        found = False
        '        For Each vPoll In myUci.Pollutants
        '            lPoll = vPoll
        '            If lPoll.Name = aslGQual.RightItem(i - 1) Then
        '                found = True
        '            End If
        '        Next vPoll
        '        For Each vPoll In defUci.Pollutants
        '            lPoll = vPoll
        '            If lPoll.Name = aslGQual.RightItem(i - 1) And _
        '              lPoll.modeltype = "DataIn" Then
        '                found = True
        '            End If
        '        Next vPoll
        '        If Not found Then
        '            'need to add
        '            myUci.Edited = True
        '            j = 1
        '            Do While j <= defUci.Pollutants.Count
        '                lPoll = defUci.Pollutants(j)
        '                If lPoll.Name = aslGQual.RightItem(i - 1) Then
        '                    'add this one
        '                    myUci.Pollutants.Add(lPoll)
        '                    If Mid(lPoll.modeltype, 1, 4) = "Data" Then
        '                        'instead of removing it, flag as in use
        '                        defUci.Pollutants(j).modeltype = "DataIn"
        '                    Else
        '                        'remove the other types
        '                        defUci.Pollutants.Remove(j)
        '                    End If
        '                    Exit Do
        '                Else
        '                    j = j + 1
        '                End If
        '            Loop
        '        End If
        '    Next i

        '    j = 1
        '    Do While j <= myUci.Pollutants.Count
        '        lPoll = myUci.Pollutants(j)
        '        found = False
        '        For i = 1 To aslGQual.RightCount
        '            If lPoll.Name = aslGQual.RightItem(i - 1) Then
        '                found = True
        '            End If
        '        Next i
        '        If Not found Then
        '            'need to remove
        '            myUci.Edited = True
        '            'are there any associated ext targets?
        '            FindAndRemoveExtTargets(j)
        '            defUci.Pollutants.Add(lPoll)
        '            myUci.Pollutants.Remove(j)
        '        Else
        '            j = j + 1
        '        End If
        '    Loop

        '    For i = 1 To aslGQual.LeftCount
        '        For Each vPoll In defUci.Pollutants
        '            lPoll = vPoll
        '            If lPoll.Name = aslGQual.LeftItem(i - 1) Then
        '                'found in def list
        '                If lPoll.modeltype = "DataIn" Then
        '                    lPoll.modeltype = "Data"
        '                End If
        '            End If
        '        Next vPoll
        '    Next i
        '    unload(Me)
        'End If

    End Sub

    Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        'unload(Me)
    End Sub

    Private Sub ReadPollutants()

        'Dim delim$, quote$, i&, tname$, amax&, tstr$, tcnt&
        'Dim tabend As Boolean
        'Dim ctab$, opf&, opl&, sopl$, j&
        'Dim vOper As Object, lOper As HspfOperation, tOper As HspfOperation
        'Dim lTable As HspfTable, lOpnBlk As HspfOpnBlk
        'Dim massend As Boolean
        'Dim lML As HspfMassLink, found As Boolean
        'Dim tempTable As HspfTable, thistable&, tempname$

        'Dim lPollutantFileName As String = PathNameOnly(pDefUCI.Name) & "\pollutants.txt"
        'If Not FileExists(lPollutantFileName) Then
        '    lPollutantFileName = FindFile("Please locate pollutants.txt", "pollutants.txt")
        'End If

        'Dim lStr As String
        'Dim lLen As Integer
        'Dim lTmp As String
        'Dim lCcons As String
        'Dim lPtype As Integer
        'Dim lItype As Integer
        'Dim lRtype As Integer
        'Dim lCend As Boolean
        'Dim lFound As Boolean
        'Dim lOpTyp As String
        'Dim lOpEnd As Boolean
        'If FileExists(lPollutantFileName) Then
        '    Dim lRecords As Collection = LinesInFile(lPollutantFileName)

        '    Dim lCurrentIndex As Integer = 0
        '    For Each lCurrentRecord As String In lRecords
        '        lCurrentIndex += 1
        '        lStr = lCurrentRecord.TrimEnd
        '        lLen = Len(lStr)

        '        If lLen > 6 Then
        '            If lStr.StartsWith("CONSTIT") Then
        '                'found start of a constituent

        '                Dim lPoll As New HspfPollutant
        '                lTmp = StrRetRem(lStr)
        '                lCcons = lStr
        '                lPoll.Name = lStr
        '                lPtype = 0
        '                lItype = 0
        '                lRtype = 0
        '                lCend = False
        '                Do While Not lCend
        '                    lCurrentIndex += 1
        '                    lStr = Trim(lRecords(lCurrentIndex))
        '                    lLen = Len(lStr)

        '                    If lStr.StartsWith("END CONSTIT") Then
        '                        'found end of constituent
        '                        lCend = True
        '                        lPoll.Id = pDefUCI.Pollutants.Count + 1
        '                        lPoll.Index = pUCI.Pollutants.Count + 1
        '                        If lPtype = 1 And lRtype = 1 Then
        '                            lPoll.ModelType = "PIG"
        '                        ElseIf lPtype = 1 Then
        '                            lPoll.ModelType = "PIOnly"
        '                        ElseIf lRtype = 1 Then
        '                            lPoll.ModelType = "GOnly"
        '                        Else
        '                            lPoll.ModelType = "Data"
        '                        End If
        '                        lFound = False
        '                        For Each lTempPoll As HspfPollutant In pUCI.Pollutants
        '                            If lTempPoll.Name = lPoll.Name Then
        '                                lFound = True
        '                            End If
        '                        Next
        '                        For Each lTempPoll As HspfPollutant In pDefUCI.Pollutants
        '                            If lTempPoll.Name = lPoll.Name Then
        '                                lFound = True
        '                            End If
        '                        Next
        '                        If lFound = False Then
        '                            pDefUCI.Pollutants.Add(lPoll)
        '                        End If
        '                        lPoll = Nothing
        '                    Else
        '                        lOpTyp = lStr.Substring(1, lLen)
        '                        If lOpTyp = "PERLND" Or lOpTyp = "IMPLND" Or lOpTyp = "RCHRES" Then
        '                            'found start of an operation
        '                            lOpnBlk = New HspfOpnBlk
        '                            lOpnBlk.Name = lOpTyp
        '                            lOpnBlk.Uci = pDefUCI
        '                            For Each lOper In pUCI.OpnBlks(lOpTyp).Ids
        '                                lOpnBlk.Ids.Add(lOper, lOpTyp & lOper.Id)
        '                            Next
        '                            For Each lOper In pUCI.OpnBlks(lOpTyp).Ids
        '                                tOper = New HspfOperation
        '                                tOper.Name = lOper.Name
        '                                tOper.Id = lOper.Id
        '                                tOper.Description = lOper.Description
        '                                tOper.DefOpnId = DefaultOpnId(tOper, pDefUCI)
        '                                tOper.OpnBlk = lOpnBlk
        '                                lPoll.Operations.Add(lOpTyp & tOper.Id, tOper)                                    Next vOper

        '                                lOpEnd = False
        '                                Do While Not lOpEnd
        '                                    lCurrentIndex += 1
        '                                    lStr = lRecords(lCurrentIndex)
        '                                    lLen = Len(RTrim(lStr))
        '                                    If Left(lStr, ilen) = "END " & coptyp Then
        '                                        'found end of operation
        '                                        opend = True
        '                                    ElseIf ilen > 0 Then
        '                                        'found start of table
        '                                        ctab = RTrim(Mid(lStr, 3))

        '                                        tabend = False
        '                                        Do While Not tabend
        '            Line Input #i, lstr
        '                                            ilen = Len(lStr)
        '                                            lStr = RTrim(lStr)
        '                                            If ilen > 0 Then
        '                                                If Left(lStr, ilen) = "  END " & ctab Then
        '                                                    'found end of table
        '                                                    tabend = True
        '                                                Else
        '                                                    If InStr(1, lStr, "***") Then
        '                                                        'comment, ignore
        '                                                    Else
        '                                                        'found line of table
        '                                                        opf = Left(lStr, 5)
        '                                                        sopl = Trim(Mid(lStr, 6, 5))
        '                                                        If Len(sopl) > 0 Then
        '                                                            opl = sopl
        '                                                        Else
        '                                                            opl = opf
        '                                                        End If
        '                                                        For Each vOper In lPoll.Operations
        '                                                            lOper = vOper
        '                                                            If lOper.Name = coptyp Then
        '                                                                If opf = lOper.DefOpnId Or (opf <= lOper.DefOpnId And lOper.DefOpnId <= opl) Then
        '                                                                    lTable = New HspfTable
        '                                                                    lTable.Def = myMsg.BlockDefs(coptyp).TableDefs(ctab)
        '                                                                    lTable.Opn = lOper
        '                                                                    lTable.InitTable(lStr)
        '                                                                    If lTable.Name = "GQ-QALDATA" Then
        '                                                                        rtype = 1
        '                                                                    ElseIf lTable.Name = "QUAL-PROPS" Then
        '                                                                        ptype = 1
        '                                                                        itype = 1
        '                                                                    End If
        '                                                                    'Set lTable.Opn = lOper
        '                                                                    lTable.OccurCount = 1
        '                                                                    lTable.OccurNum = 1
        '                                                                    lTable.OccurIndex = 0
        '                                                                    If Not lOper.TableExists(lTable.Name) Then
        '                                                                        lOper.Tables.Add(lTable, lTable.Name)
        '                                                                        If Not lPoll.TableExists(lTable.Name) Then
        '                                                                            lPoll.Tables.Add(lTable, lTable.Name)
        '                                                                        End If
        '                                                                    Else
        '                                                                        'handle multiple occurs of this table
        '                                                                        tempTable = lOper.Tables(lTable.Name)
        '                                                                        thistable = tempTable.OccurCount + 1
        '                                                                        tempTable.OccurCount = thistable
        '                                                                        For j = 2 To thistable - 1
        '                                                                            tempname = lTable.Name & ":" & CStr(j)
        '                                                                            tempTable = lOper.Tables(tempname)
        '                                                                            tempTable.OccurCount = thistable
        '                                                                        Next j
        '                                                                        lTable.OccurCount = thistable
        '                                                                        lTable.OccurNum = thistable
        '                                                                        tempname = lTable.Name & ":" & CStr(thistable)
        '                                                                        lOper.Tables.Add(lTable, tempname)
        '                                                                        If Not lPoll.TableExists(tempname) Then
        '                                                                            lPoll.Tables.Add(lTable, tempname)
        '                                                                        End If
        '                                                                    End If
        '                                                                End If
        '                                                            End If
        '                                                        Next vOper
        '                                                    End If
        '                                                End If
        '                                            End If
        '                                        Loop

        '                                    End If
        '                                Loop
        '                        ElseIf coptyp = "MASS-LINKS" Then
        '                                massend = False
        '                                Do While Not massend
        '        Line Input #i, lstr
        '                                    ilen = Len(lStr)
        '                                    If Left(lStr, ilen) = "END " & coptyp Then
        '                                        'found end of masslinks
        '                                        massend = True
        '                                    ElseIf ilen > 0 Then
        '                                        'found a masslink
        '                                        lML = New HspfMassLink
        '                                        lML.Uci = defUci
        '                                        lML.Source.VolName = Trim(Mid(lStr, 1, 6))
        '                                        lML.Source.Group = Trim(Mid(lStr, 12, 6))
        '                                        lML.Source.Member = Trim(Mid(lStr, 19, 6))
        '                                        istr = Trim(Mid(lStr, 26, 1))
        '                                        If Len(istr) = 0 Then
        '                                            lML.Source.MemSub1 = 0
        '                                        Else
        '                                            lML.Source.MemSub1 = CInt(istr)
        '                                        End If
        '                                        istr = Trim(Mid(lStr, 28, 1))
        '                                        If Len(istr) = 0 Then
        '                                            lML.Source.MemSub2 = 0
        '                                        Else
        '                                            lML.Source.MemSub2 = CInt(istr)
        '                                        End If
        '                                        istr = Trim(Mid(lStr, 30, 10))
        '                                        If Len(istr) = 0 Then
        '                                            lML.MFact = 1
        '                                        Else
        '                                            'lML.MFact = CSng(istr)
        '                                            lML.MFact = istr
        '                                        End If
        '                                        lML.Target.VolName = Trim(Mid(lStr, 44, 6))
        '                                        lML.Target.Group = Trim(Mid(lStr, 59, 6))
        '                                        lML.Target.Member = Trim(Mid(lStr, 66, 6))
        '                                        istr = Trim(Mid(lStr, 73, 1))
        '                                        If Len(istr) = 0 Then
        '                                            lML.Target.MemSub1 = 0
        '                                        Else
        '                                            lML.Target.MemSub1 = CInt(istr)
        '                                        End If
        '                                        istr = Trim(Mid(lStr, 75, 1))
        '                                        If Len(istr) = 0 Then
        '                                            lML.Target.MemSub2 = 0
        '                                        Else
        '                                            lML.Target.MemSub2 = CInt(istr)
        '                                        End If
        '                                        lML.MassLinkId = myUci.MassLinks(1).FindMassLinkID(lML.Source.VolName, lML.Target.VolName)
        '                                        lPoll.MassLinks.Add(lML)
        '                                    End If
        '                                Loop
        '                        End If
        '                    End If
        '                Loop
        '            End If
        '        End If
        '    Next
        'End If
    End Sub

    'Private Sub FindAndRemoveExtTargets(ByVal j&)
    '    Dim lConn As HspfConnection, tConn As HspfConnection
    '    Dim lPoll As HspfPollutant, tPoll As HspfPollutant
    '    Dim lTable As HspfTable, lOper As HspfOperation
    '    Dim thisindex&, pscount&, pcount&, iscount&, icount&
    '    Dim rcount&, i&, remflag As Boolean, k&
    '    Dim psflag As Boolean, pflag As Boolean, isflag As Boolean
    '    Dim iflag As Boolean, rflag As Boolean

    '    'figure out which gqual, pqual, iqual to look for
    '    lPoll = myUci.Pollutants(j)
    '    thisindex = lPoll.Index
    '    pscount = 0  'perlnd qual sed assoc count
    '    pcount = 0   'perlnd qual count
    '    iscount = 0  'implnd qual sed assoc count
    '    icount = 0   'implnd qual count
    '    rcount = 0   'gqual count
    '    For i = 1 To myUci.Pollutants.Count
    '        tPoll = myUci.Pollutants(i)
    '        If tPoll.Index <= thisindex Then
    '            For k = 1 To tPoll.Operations.Count
    '                If tPoll.Operations(k).Name = "PERLND" Then
    '                    If tPoll.Operations(k).TableExists("QUAL-PROPS") Then
    '                        lTable = tPoll.Operations(k).tables("QUAL-PROPS")
    '                        If lTable.Parms("QSDFG") = 1 Then
    '                            pscount = pscount + 1
    '                        End If
    '                        pcount = pcount + 1
    '                    End If
    '                    Exit For
    '                End If
    '            Next k
    '            For k = 1 To tPoll.Operations.Count
    '                If tPoll.Operations(k).Name = "IMPLND" Then
    '                    If tPoll.Operations(k).TableExists("QUAL-PROPS") Then
    '                        lTable = tPoll.Operations(k).tables("QUAL-PROPS")
    '                        If lTable.Parms("QSDFG") = 1 Then
    '                            iscount = iscount + 1
    '                        End If
    '                        icount = icount + 1
    '                    End If
    '                    Exit For
    '                End If
    '            Next k
    '            For k = 1 To tPoll.Operations.Count
    '                If tPoll.Operations(k).Name = "RCHRES" Then
    '                    If tPoll.Operations(k).TableExists("GQ-QALDATA") Then
    '                        rcount = rcount + 1
    '                    End If
    '                    Exit For
    '                End If
    '            Next k
    '        End If
    '    Next i

    '    'figure out if we are removing a pqual, iqual, rqual
    '    psflag = False   'removing a pqual sed assoc
    '    pflag = False    'removing a pqual
    '    isflag = False   'removing a iqual sed assoc
    '    iflag = False    'removing a iqual
    '    rflag = False    'removing a gqual
    '    For k = 1 To lPoll.Operations.Count
    '        If lPoll.Operations(k).Name = "PERLND" Then
    '            If lPoll.Operations(k).TableExists("QUAL-PROPS") Then
    '                lTable = lPoll.Operations(k).tables("QUAL-PROPS")
    '                If lTable.Parms("QSDFG") = 1 Then
    '                    psflag = True
    '                End If
    '                pflag = True
    '            End If
    '            Exit For
    '        End If
    '    Next k
    '    For k = 1 To lPoll.Operations.Count
    '        If lPoll.Operations(k).Name = "IMPLND" Then
    '            If lPoll.Operations(k).TableExists("QUAL-PROPS") Then
    '                lTable = lPoll.Operations(k).tables("QUAL-PROPS")
    '                If lTable.Parms("QSDFG") = 1 Then
    '                    isflag = True
    '                End If
    '                iflag = True
    '            End If
    '            Exit For
    '        End If
    '    Next k
    '    For k = 1 To lPoll.Operations.Count
    '        If lPoll.Operations(k).Name = "RCHRES" Then
    '            If lPoll.Operations(k).TableExists("GQ-QALDATA") Then
    '                rflag = True
    '            End If
    '            Exit For
    '        End If
    '    Next k

    '    'look through all ext targets for ones to remove
    '    k = 1
    '    Do While k <= myUci.Connections.Count
    '        lConn = myUci.Connections(k)
    '        remflag = False
    '        If lConn.Typ = 4 Then
    '            If lConn.Source.volname = "RCHRES" And lConn.Source.group = "GQUAL" Then
    '                If lConn.Source.member = "TIQAL" Or lConn.Source.member = "DQAL" Or _
    '                   lConn.Source.member = "RDQAL" Or lConn.Source.member = "RRQAL" Or _
    '                   lConn.Source.member = "IDQAL" Or lConn.Source.member = "PDQAL" Or _
    '                   lConn.Source.member = "GQADDR" Or lConn.Source.member = "GQADWT" Or _
    '                   lConn.Source.member = "GQADEP" Or lConn.Source.member = "RODQAL" Or _
    '                   lConn.Source.member = "TROQAL" Then
    '                    If rflag And lConn.Source.memsub1 = rcount Then
    '                        remflag = True
    '                    End If
    '                Else
    '                    If rflag And lConn.Source.memsub2 = rcount Then
    '                        remflag = True
    '                    End If
    '                End If
    '            ElseIf lConn.Source.volname = "PERLND" And lConn.Source.group = "PQUAL" Then
    '                If lConn.Source.member = "SOQSP" Or lConn.Source.member = "WASHQS" Or _
    '                   lConn.Source.member = "SCRQS" Or lConn.Source.member = "SOQS" Then
    '                    If psflag And lConn.Source.memsub1 = pscount Then
    '                        remflag = True
    '                    End If
    '                Else
    '                    If pflag And lConn.Source.memsub1 = pcount Then
    '                        remflag = True
    '                    End If
    '                End If
    '            ElseIf lConn.Source.volname = "IMPLND" And lConn.Source.group = "IQUAL" Then
    '                If lConn.Source.member = "SOQSP" Or lConn.Source.member = "SOQS" Then
    '                    If isflag And lConn.Source.memsub1 = iscount Then
    '                        remflag = True
    '                    End If
    '                Else
    '                    If iflag And lConn.Source.memsub1 = icount Then
    '                        remflag = True
    '                    End If
    '                End If
    '            End If
    '        End If
    '        If remflag Then
    '            'remove the ext target from the uci and the operation
    '            myUci.Connections.Remove(k)
    '            lOper = myUci.OpnBlks(lConn.Source.volname).operfromid(lConn.Source.volid)
    '            i = 1
    '            Do While i <= lOper.targets.Count
    '                tConn = lOper.targets(i)
    '                If tConn.Target.volname = lConn.Target.volname And _
    '                   tConn.Target.volid = lConn.Target.volid Then
    '                    lOper.targets.Remove(i)
    '                Else
    '                    i = i + 1
    '                End If
    '            Loop
    '        Else
    '            k = k + 1
    '        End If
    '    Loop

    '    'look through all ext targets for ones to decrement
    '    k = 1
    '    Do While k <= myUci.Connections.Count
    '        lConn = myUci.Connections(k)
    '        If lConn.Typ = 4 Then
    '            If lConn.Source.volname = "RCHRES" And lConn.Source.group = "GQUAL" Then
    '                If lConn.Source.member = "TIQAL" Or lConn.Source.member = "DQAL" Or _
    '                   lConn.Source.member = "RDQAL" Or lConn.Source.member = "RRQAL" Or _
    '                   lConn.Source.member = "IDQAL" Or lConn.Source.member = "PDQAL" Or _
    '                   lConn.Source.member = "GQADDR" Or lConn.Source.member = "GQADWT" Or _
    '                   lConn.Source.member = "GQADEP" Or lConn.Source.member = "RODQAL" Or _
    '                   lConn.Source.member = "TROQAL" Then
    '                    If rflag And lConn.Source.memsub1 > rcount Then
    '                        lConn.Source.memsub1 = lConn.Source.memsub1 - 1
    '                    End If
    '                Else
    '                    If rflag And lConn.Source.memsub2 > rcount Then
    '                        lConn.Source.memsub2 = lConn.Source.memsub2 - 1
    '                    End If
    '                End If
    '            ElseIf lConn.Source.volname = "PERLND" And lConn.Source.group = "PQUAL" Then
    '                If lConn.Source.member = "SOQSP" Or lConn.Source.member = "WASHQS" Or _
    '                   lConn.Source.member = "SCRQS" Or lConn.Source.member = "SOQS" Then
    '                    If psflag And lConn.Source.memsub1 > pscount Then
    '                        lConn.Source.memsub1 = lConn.Source.memsub1 - 1
    '                    End If
    '                Else
    '                    If pflag And lConn.Source.memsub1 > pcount Then
    '                        lConn.Source.memsub1 = lConn.Source.memsub1 - 1
    '                    End If
    '                End If
    '            ElseIf lConn.Source.volname = "IMPLND" And lConn.Source.group = "IQUAL" Then
    '                If lConn.Source.member = "SOQSP" Or lConn.Source.member = "SOQS" Then
    '                    If isflag And lConn.Source.memsub1 = iscount Then
    '                        lConn.Source.memsub1 = lConn.Source.memsub1 - 1
    '                    End If
    '                Else
    '                    If iflag And lConn.Source.memsub1 = icount Then
    '                        lConn.Source.memsub1 = lConn.Source.memsub1 - 1
    '                    End If
    '                End If
    '            End If
    '        End If
    '        If Not remflag Then
    '            k = k + 1
    '        End If
    '    Loop

    'End Sub
End Class