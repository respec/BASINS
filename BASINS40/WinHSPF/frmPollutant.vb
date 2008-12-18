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

        If pDefUCI.Pollutants.Count = 0 Then
            ReadPollutants()
        End If

        For lIndex As Integer = 0 To pUCI.Pollutants.Count - 1
            lstPollutants.Items.Add(pUCI.Pollutants(lIndex).Name)
            lstPollutants.SetSelected(lIndex, True)
        Next lIndex

        For lIndex As Integer = 0 To pDefUCI.Pollutants.Count - 1
            'is this one already in the list?
            Dim lInList As Boolean = False
            If lstPollutants.Items.Count > 0 Then
                For lListIndex As Integer = 0 To lstPollutants.Items.Count - 1
                    If lstPollutants.Items(lListIndex) = pDefUCI.Pollutants(lIndex).Name Then
                        lInList = True
                    End If
                Next
            End If
            If Not lInList Then
                lstPollutants.Items.Add(pDefUCI.Pollutants(lIndex).Name)
            End If
            If pDefUCI.Pollutants(lIndex).ModelType = "DataIn" Then
                lstPollutants.SetSelected(lIndex + pUCI.Pollutants.Count, True)
            End If
        Next lIndex

    End Sub

    Private Sub cmdOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOK.Click
        'Dim i, vPoll As Object, lPoll As HspfPollutant, found As Boolean

        'make sure not too many
        Dim lpcount As Integer = 0
        Dim licount As Integer = 0
        Dim lrcount As Integer = 0
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
        Me.Dispose()
    End Sub

    Private Sub ReadPollutants()

        Dim lPollutantFileName As String = PathNameOnly(pDefUCI.Name) & "\pollutants.txt"
        If Not FileExists(lPollutantFileName) Then
            lPollutantFileName = FindFile("Please locate pollutants.txt", "pollutants.txt")
        End If

        Dim lRecords As New Collection
        If FileExists(lPollutantFileName) Then
            For Each lRecord As String In LinesInFile(lPollutantFileName)
                lRecords.Add(lRecord)
            Next
        End If

        Dim lCurrentIndex As Integer = 1
        Dim lCurrentRecord As String = ""
        Do While lCurrentIndex < lRecords.Count
            lCurrentRecord = lRecords(lCurrentIndex)
            If lCurrentRecord.StartsWith("CONSTIT") Then

                'found start of a constituent
                Dim lPoll As New HspfPollutant
                Dim lTemp As String = StrRetRem(lCurrentRecord)
                'lCcons = lcurrentrecord
                lPoll.Name = lCurrentRecord

                Dim lPtype As Integer = 0
                Dim lItype As Integer = 0
                Dim lRtype As Integer = 0
                Dim lFoundConstituentEnd As Boolean = False

                Do While Not lFoundConstituentEnd
                    lCurrentIndex += 1
                    lCurrentRecord = lRecords(lCurrentIndex)
                    If lCurrentRecord.StartsWith("END CONSTIT") Then
                        'this is the end of the constituent
                        lFoundConstituentEnd = True
                        lPoll.Id = pDefUCI.Pollutants.Count + 1
                        lPoll.Index = pUCI.Pollutants.Count + 1
                        If lPtype = 1 And lRtype = 1 Then
                            lPoll.ModelType = "PIG"
                        ElseIf lPtype = 1 Then
                            lPoll.ModelType = "PIOnly"
                        ElseIf lRtype = 1 Then
                            lPoll.ModelType = "GOnly"
                        Else
                            lPoll.ModelType = "Data"
                        End If
                        'see if we already have this constituent in the uci or defuci
                        Dim lFoundThisConstituentAlready As Boolean = False
                        For Each lTempPoll As HspfPollutant In pUCI.Pollutants
                            If lTempPoll.Name = lPoll.Name Then
                                lFoundThisConstituentAlready = True
                            End If
                        Next
                        For Each lTempPoll As HspfPollutant In pDefUCI.Pollutants
                            If lTempPoll.Name = lPoll.Name Then
                                lFoundThisConstituentAlready = True
                            End If
                        Next
                        If Not lFoundThisConstituentAlready Then
                            'add this constituent to the defuci
                            pDefUCI.Pollutants.Add(lPoll)
                        End If
                        lPoll = Nothing
                    ElseIf lCurrentRecord.StartsWith("PERLND") Or lCurrentRecord.StartsWith("IMPLND") Or lCurrentRecord.StartsWith("RCHRES") Then
                        'found start of an operation
                        Dim lOpnBlk As New HspfOpnBlk
                        Dim lOpTyp As String = Trim(Mid(lCurrentRecord, 1, 6))
                        lOpnBlk.Name = lOpTyp
                        lOpnBlk.Uci = pDefUCI
                        For Each lOper As HspfOperation In pUCI.OpnBlks(lOpTyp).Ids
                            lOpnBlk.Ids.Add(lOper)
                            lPoll.Operations.Add(lOpTyp & lOper.Id, lOper)
                        Next
                        Dim lEndofOperation As Boolean = False
                        Do While Not lEndofOperation
                            lCurrentIndex += 1
                            lCurrentRecord = lRecords(lCurrentIndex)
                            If lCurrentRecord.StartsWith("END " & lOpTyp) Then
                                'found end of operation
                                lEndofOperation = True
                            ElseIf lCurrentRecord.Trim.Length > 0 Then
                                'found start of table
                                Dim lTableName = RTrim(Mid(lCurrentRecord, 3))
                                Dim lEndofTable As Boolean = False
                                Do While Not lEndofTable
                                    lCurrentIndex += 1
                                    lCurrentRecord = lRecords(lCurrentIndex)
                                    If lCurrentRecord.Trim.Length > 0 Then
                                        If lCurrentRecord.StartsWith("  END " & lTableName) Then
                                            'found end of table
                                            lEndofTable = True
                                        Else
                                            If InStr(1, lCurrentRecord, "***") Then
                                                'comment, ignore
                                            Else
                                                'found line of table
                                                Dim lOpf As String = Mid(lCurrentRecord, 1, 5)
                                                Dim lOpl = Trim(Mid(lCurrentRecord, 6, 5))
                                                If Len(lOpl) = 0 Then
                                                    lOpl = lOpf
                                                End If
                                                For Each lOper As Generic.KeyValuePair(Of String, HspfOperation) In lPoll.Operations
                                                    If lOper.Value.Name = lOpTyp Then
                                                        If lOpf = lOper.Value.DefOpnId Or (lOpf <= lOper.Value.DefOpnId And lOper.Value.DefOpnId <= lOpl) Then
                                                            Dim lTable As New HspfTable
                                                            lTable.Def = pMsg.BlockDefs(lOpTyp).TableDefs(lTableName)
                                                            lTable.Opn = lOper.Value
                                                            lTable.InitTable(lCurrentRecord)
                                                            If lTable.Name = "GQ-QALDATA" Then
                                                                lRtype = 1
                                                            ElseIf lTable.Name = "QUAL-PROPS" Then
                                                                lPtype = 1
                                                                lItype = 1
                                                            End If
                                                            lTable.OccurCount = 1
                                                            lTable.OccurNum = 1
                                                            lTable.OccurIndex = 0
                                                            If Not lOper.Value.TableExists(lTable.Name) Then
                                                                lOper.Value.Tables.Add(lTable)
                                                                If Not lPoll.TableExists(lTable.Name) Then
                                                                    lPoll.Tables.Add(lTable.Name, lTable)
                                                                End If
                                                            Else
                                                                'handle multiple occurs of this table
                                                                Dim ltempTable As HspfTable = lOper.Value.Tables(lTable.Name)
                                                                Dim lNOccurance As Integer = ltempTable.OccurCount + 1
                                                                Dim lTempName As String = ""
                                                                ltempTable.OccurCount = lNOccurance
                                                                For lTableIndex As Integer = 2 To lNOccurance - 1
                                                                    lTempName = lTable.Name & ":" & CStr(lTableIndex)
                                                                    ltempTable = lOper.Value.Tables(lTempName)
                                                                    ltempTable.OccurCount = lNOccurance
                                                                Next
                                                                lTable.OccurCount = lNOccurance
                                                                lTable.OccurNum = lNOccurance
                                                                lTempName = lTable.Name & ":" & CStr(lNOccurance)
                                                                lOper.Value.Tables.Add(lTable)
                                                                If Not lPoll.TableExists(lTempName) Then
                                                                    lPoll.Tables.Add(lTempName, lTable)
                                                                End If
                                                            End If
                                                        End If
                                                    End If
                                                Next
                                            End If
                                        End If
                                    End If
                                Loop
                            End If
                        Loop

                    ElseIf lCurrentRecord.StartsWith("MASS-LINKS") Then
                            Dim lFoundEndofMassLinks As Boolean = False
                            Do While Not lFoundEndofMassLinks
                                lCurrentIndex += 1
                                lCurrentRecord = lRecords(lCurrentIndex)
                                If lCurrentRecord.StartsWith("END MASS-LINKS") Then
                                    'found end of masslinks
                                    lFoundEndofMassLinks = True
                                ElseIf lCurrentRecord.Trim.Length > 0 Then
                                    'found a masslink
                                    Dim lML As New HspfMassLink
                                    lML.Uci = pDefUCI
                                    lML.Source.VolName = Trim(Mid(lCurrentRecord, 1, 6))
                                    lML.Source.Group = Trim(Mid(lCurrentRecord, 12, 6))
                                    lML.Source.Member = Trim(Mid(lCurrentRecord, 19, 6))
                                    Dim lIstr As String = Trim(Mid(lCurrentRecord, 26, 1))
                                    If Len(lIstr) = 0 Then
                                        lML.Source.MemSub1 = 0
                                    Else
                                        lML.Source.MemSub1 = CInt(lIstr)
                                    End If
                                    lIstr = Trim(Mid(lCurrentRecord, 28, 1))
                                    If Len(lIstr) = 0 Then
                                        lML.Source.MemSub2 = 0
                                    Else
                                        lML.Source.MemSub2 = CInt(lIstr)
                                    End If
                                    lIstr = Trim(Mid(lCurrentRecord, 30, 10))
                                    If Len(lIstr) = 0 Then
                                        lML.MFact = 1
                                    Else
                                        lML.MFact = lIstr
                                    End If
                                    lML.Target.VolName = Trim(Mid(lCurrentRecord, 44, 6))
                                    lML.Target.Group = Trim(Mid(lCurrentRecord, 59, 6))
                                    lML.Target.Member = Trim(Mid(lCurrentRecord, 66, 6))
                                    lIstr = Trim(Mid(lCurrentRecord, 73, 1))
                                    If Len(lIstr) = 0 Then
                                        lML.Target.MemSub1 = 0
                                    Else
                                        lML.Target.MemSub1 = CInt(lIstr)
                                    End If
                                    lIstr = Trim(Mid(lCurrentRecord, 75, 1))
                                    If Len(lIstr) = 0 Then
                                        lML.Target.MemSub2 = 0
                                    Else
                                        lML.Target.MemSub2 = CInt(lIstr)
                                    End If
                                    lML.MassLinkId = pUCI.MassLinks(1).FindMassLinkID(lML.Source.VolName, lML.Target.VolName)
                                    lPoll.MassLinks.Add(lML)
                                End If
                            Loop
                    End If
                Loop
            End If

            lCurrentIndex += 1
        Loop

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