Imports atcUtility
Imports MapWinUtility
Imports atcUCI

Public Class frmPollutant

    Friend pLoading As Boolean

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        Me.Icon = pIcon
        Me.MinimumSize = Me.Size
        pLoading = True

        If pDefUCI.Pollutants.Count = 0 Then
            ReadPollutants()
        End If

        clbPollutants.Items.Clear()
        For lIndex As Integer = 0 To pUCI.Pollutants.Count - 1
            clbPollutants.Items.Add(pUCI.Pollutants(lIndex).Name, True)
        Next lIndex

        For lIndex As Integer = 0 To pDefUCI.Pollutants.Count - 1
            clbPollutants.Items.Add(pDefUCI.Pollutants(lIndex).Name)
            If pDefUCI.Pollutants(lIndex).ModelType = "DataIn" Then
                clbPollutants.SetItemChecked(lIndex + pUCI.Pollutants.Count, True)
            Else
                'if any aren't selected, uncheck the 'all/none' box
                cbxSelect.Checked = False
            End If
        Next lIndex

        pLoading = False

    End Sub

    Private Sub cmdOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOK.Click

        'make sure not too many
        Dim lpcount As Integer = 0
        Dim licount As Integer = 0
        Dim lrcount As Integer = 0
        Dim lIndex As Integer = 0
        For lIndex = 1 To clbPollutants.CheckedItems.Count
            For Each lPoll As HspfPollutant In pUCI.Pollutants
                If lPoll.Name = clbPollutants.CheckedItems(lIndex - 1) Then
                    If lPoll.ModelType = "PIG" Then
                        lpcount = lpcount + 1
                        licount = licount + 1
                        lrcount = lrcount + 1
                    ElseIf lPoll.ModelType = "PIOnly" Then
                        lpcount = lpcount + 1
                        licount = licount + 1
                    ElseIf lPoll.ModelType = "GOnly" Then
                        lrcount = lrcount + 1
                    End If
                End If
            Next
            For Each lPoll As HspfPollutant In pDefUCI.Pollutants
                If lPoll.Name = clbPollutants.CheckedItems(lIndex - 1) Then
                    If lPoll.ModelType = "PIG" Then
                        lpcount = lpcount + 1
                        licount = licount + 1
                        lrcount = lrcount + 1
                    ElseIf lPoll.ModelType = "PIOnly" Then
                        lpcount = lpcount + 1
                        licount = licount + 1
                    ElseIf lPoll.ModelType = "GOnly" Then
                        lrcount = lrcount + 1
                    End If
                End If
            Next
        Next

        If lpcount > 10 Or licount > 10 Then
            'too many p/i quals
            Logger.Msg("The number of PERLND or IMPLND quality constituents exceeds the maximum." & vbCrLf & _
              "Remove some pollutants from the 'Selected' list.", MsgBoxStyle.OkOnly, "Pollutant Problem")
            Exit Sub
        End If
        If lrcount > 3 Then
            'too many gquals
            Logger.Msg("The number of General quality constituents exceeds the maximum." & vbCrLf & _
              "Remove some pollutants from the 'Selected' list.", MsgBoxStyle.OkOnly, "Pollutant Problem")
            Exit Sub
        End If

        'figure out which ones we need to add 
        For lIndex = 1 To clbPollutants.CheckedItems.Count
            Dim lFound As Boolean = False
            For Each lPoll As HspfPollutant In pUCI.Pollutants
                If lPoll.Name = clbPollutants.CheckedItems(lIndex - 1) Then
                    lFound = True
                End If
            Next
            For Each lPoll As HspfPollutant In pDefUCI.Pollutants
                If lPoll.Name = clbPollutants.CheckedItems(lIndex - 1) And _
                  lPoll.ModelType = "DataIn" Then
                    lFound = True
                End If
            Next
            If Not lFound Then
                'need to add
                pUCI.Edited = True
                Dim lDefIndex As Integer = 0
                Do While lDefIndex < pDefUCI.Pollutants.Count
                    Dim lPoll As HspfPollutant = pDefUCI.Pollutants(lDefIndex)
                    If lPoll.Name = clbPollutants.CheckedItems(lIndex - 1) Then
                        'add this one
                        pUCI.Pollutants.Add(lPoll)
                        If Mid(lPoll.ModelType, 1, 4) = "Data" Then
                            'instead of removing it, flag as in use
                            pDefUCI.Pollutants(lDefIndex).ModelType = "DataIn"
                        Else
                            'remove the other types
                            pDefUCI.Pollutants.Remove(lPoll)
                        End If
                        Exit Do
                    Else
                        lDefIndex += 1
                    End If
                Loop
            End If
        Next

        'figure out which ones we need to remove
        lIndex = 0
        Do While lIndex < pUCI.Pollutants.Count
            Dim lPoll As HspfPollutant = pUCI.Pollutants(lIndex)
            Dim lFound As Boolean = False
            For lSelectedIndex As Integer = 1 To clbPollutants.CheckedItems.Count
                If lPoll.Name = clbPollutants.CheckedItems(lSelectedIndex - 1) Then
                    lFound = True
                End If
            Next
            If Not lFound Then
                'need to remove
                pUCI.Edited = True
                'are there any associated ext targets?
                FindAndRemoveExtTargets(lIndex)
                pDefUCI.Pollutants.Add(lPoll)
                pUCI.Pollutants.Remove(lPoll)
            Else
                lIndex += 1
            End If
        Loop

        For lIndex = 1 To clbPollutants.Items.Count
            Dim lfound As Boolean = False
            For lSelectedIndex As Integer = 1 To clbPollutants.CheckedItems.Count
                If clbPollutants.Items(lIndex - 1) = clbPollutants.CheckedItems(lSelectedIndex - 1) Then
                    'this is selected 
                    lfound = True
                End If
            Next
            If Not lfound Then
                'this one is not selected
                For Each lPoll As HspfPollutant In pDefUCI.Pollutants
                    If lPoll.Name = clbPollutants.Items(lIndex - 1) Then
                        'found this unselected item in def list, flag it as not in use
                        If lPoll.ModelType = "DataIn" Then
                            lPoll.ModelType = "Data"
                        End If
                    End If
                Next
            End If
        Next
        Me.Dispose()

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
                            Dim lTempOper As New HspfOperation
                            lTempOper.Name = lOper.Name
                            lTempOper.Id = lOper.Id
                            lTempOper.Description = lOper.Description
                            lTempOper.DefOpnId = DefaultOpnId(lTempOper, pDefUCI)
                            lTempOper.OpnBlk = lOpnBlk
                            lPoll.Operations.Add(lOpTyp & lTempOper.Id, lTempOper)
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
                                                Dim lOpf As Integer = CInt(Mid(lCurrentRecord, 1, 5))
                                                Dim lOpl As Integer
                                                If Trim(Mid(lCurrentRecord, 6, 5)).Length = 0 Then
                                                    lOpl = lOpf
                                                Else
                                                    lOpl = CInt(Mid(lCurrentRecord, 6, 5))
                                                End If
                                                For Each lOper As Generic.KeyValuePair(Of String, HspfOperation) In lPoll.Operations
                                                    If lOper.Value.Name = lOpTyp Then
                                                        lOper.Value.DefOpnId = DefaultOpnId(lOper.Value, pDefUCI)
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

    Private Sub FindAndRemoveExtTargets(ByVal j&)

        'figure out which gqual, pqual, iqual to look for
        Dim lPoll As HspfPollutant = pUCI.Pollutants(j)
        Dim lThisIndex As Integer = lPoll.Index
        Dim lpscount As Integer = 0  'perlnd qual sed assoc count
        Dim lpcount As Integer = 0   'perlnd qual count
        Dim liscount As Integer = 0  'implnd qual sed assoc count
        Dim licount As Integer = 0   'implnd qual count
        Dim lrcount As Integer = 0   'gqual count
        For lIndex As Integer = 0 To pUCI.Pollutants.Count - 1
            Dim ltPoll As HspfPollutant = pUCI.Pollutants(lIndex)
            If ltPoll.Index <= lThisIndex Then
                For Each lOper As HspfOperation In ltPoll.Operations.Values
                    If lOper.Name = "PERLND" Then
                        If lOper.TableExists("QUAL-PROPS") Then
                            Dim lTable As HspfTable = lOper.Tables("QUAL-PROPS")
                            If lTable.Parms("QSDFG").Value = 1 Then
                                lpscount = lpscount + 1
                            End If
                            lpcount = lpcount + 1
                        End If
                        Exit For
                    End If
                Next
                For Each lOper As HspfOperation In ltPoll.Operations.Values
                    If lOper.Name = "IMPLND" Then
                        If lOper.TableExists("QUAL-PROPS") Then
                            Dim lTable As HspfTable = lOper.Tables("QUAL-PROPS")
                            If lTable.Parms("QSDFG").Value = 1 Then
                                liscount = liscount + 1
                            End If
                            licount = licount + 1
                        End If
                        Exit For
                    End If
                Next
                For Each lOper As HspfOperation In ltPoll.Operations.Values
                    If lOper.Name = "RCHRES" Then
                        If lOper.TableExists("GQ-QALDATA") Then
                            lrcount = lrcount + 1
                        End If
                        Exit For
                    End If
                Next
            End If
        Next

        'figure out if we are removing a pqual, iqual, rqual
        Dim lpsflag As Boolean = False   'removing a pqual sed assoc
        Dim lpflag As Boolean = False    'removing a pqual
        Dim lisflag As Boolean = False   'removing a iqual sed assoc
        Dim liflag As Boolean = False    'removing a iqual
        Dim lrflag As Boolean = False    'removing a gqual
        For Each lOper As HspfOperation In lPoll.Operations.Values
            If lOper.Name = "PERLND" Then
                If lOper.TableExists("QUAL-PROPS") Then
                    Dim lTable As HspfTable = lOper.Tables("QUAL-PROPS")
                    If lTable.Parms("QSDFG").Value = 1 Then
                        lpsflag = True
                    End If
                    lpflag = True
                End If
                Exit For
            End If
        Next
        For Each lOper As HspfOperation In lPoll.Operations.Values
            If lOper.Name = "IMPLND" Then
                If lOper.TableExists("QUAL-PROPS") Then
                    Dim lTable As HspfTable = lOper.Tables("QUAL-PROPS")
                    If lTable.Parms("QSDFG").Value = 1 Then
                        lisflag = True
                    End If
                    liflag = True
                End If
                Exit For
            End If
        Next
        For Each lOper As HspfOperation In lPoll.Operations.Values
            If lOper.Name = "RCHRES" Then
                If lOper.TableExists("GQ-QALDATA") Then
                    lrflag = True
                End If
                Exit For
            End If
        Next

        'look through all ext targets for ones to remove
        Dim lConnIndex As Integer = 0
        Dim lRemflag As Boolean
        Do While lConnIndex < pUCI.Connections.Count
            Dim lConn As HspfConnection = pUCI.Connections(lConnIndex)
            lRemflag = False
            If lConn.Typ = 4 Then
                If lConn.Source.VolName = "RCHRES" And lConn.Source.Group = "GQUAL" Then
                    If lConn.Source.Member = "TIQAL" Or lConn.Source.Member = "DQAL" Or _
                       lConn.Source.Member = "RDQAL" Or lConn.Source.Member = "RRQAL" Or _
                       lConn.Source.Member = "IDQAL" Or lConn.Source.Member = "PDQAL" Or _
                       lConn.Source.Member = "GQADDR" Or lConn.Source.Member = "GQADWT" Or _
                       lConn.Source.Member = "GQADEP" Or lConn.Source.Member = "RODQAL" Or _
                       lConn.Source.Member = "TROQAL" Then
                        If lrflag And lConn.Source.MemSub1 = lrcount Then
                            lRemflag = True
                        End If
                    Else
                        If lrflag And lConn.Source.MemSub2 = lrcount Then
                            lRemflag = True
                        End If
                    End If
                ElseIf lConn.Source.VolName = "PERLND" And lConn.Source.Group = "PQUAL" Then
                    If lConn.Source.Member = "SOQSP" Or lConn.Source.Member = "WASHQS" Or _
                       lConn.Source.Member = "SCRQS" Or lConn.Source.Member = "SOQS" Then
                        If lpsflag And lConn.Source.MemSub1 = lpscount Then
                            lRemflag = True
                        End If
                    Else
                        If lpflag And lConn.Source.MemSub1 = lpcount Then
                            lRemflag = True
                        End If
                    End If
                ElseIf lConn.Source.VolName = "IMPLND" And lConn.Source.Group = "IQUAL" Then
                    If lConn.Source.Member = "SOQSP" Or lConn.Source.Member = "SOQS" Then
                        If lisflag And lConn.Source.MemSub1 = liscount Then
                            lRemflag = True
                        End If
                    Else
                        If liflag And lConn.Source.MemSub1 = licount Then
                            lRemflag = True
                        End If
                    End If
                End If
            End If
            If lRemflag Then
                'remove the ext target from the uci and the operation
                pUCI.Connections.Remove(lConn)
                Dim lOper As HspfOperation = pUCI.OpnBlks(lConn.Source.VolName).OperFromID(lConn.Source.VolId)
                Dim lIndex = 1
                Do While lIndex <= lOper.Targets.Count
                    Dim ltConn As HspfConnection = lOper.Targets(lIndex)
                    If ltConn.Target.VolName = lConn.Target.VolName And _
                       ltConn.Target.VolId = lConn.Target.VolId Then
                        lOper.Targets.RemoveAt(lIndex)
                    Else
                        lIndex = lIndex + 1
                    End If
                Loop
            Else
                lConnIndex = lConnIndex + 1
            End If
        Loop

        'look through all ext targets for ones to decrement
        lConnIndex = 0
        Do While lConnIndex < pUCI.Connections.Count
            Dim lConn As HspfConnection = pUCI.Connections(lConnIndex)
            If lConn.Typ = 4 Then
                If lConn.Source.VolName = "RCHRES" And lConn.Source.Group = "GQUAL" Then
                    If lConn.Source.Member = "TIQAL" Or lConn.Source.Member = "DQAL" Or _
                       lConn.Source.Member = "RDQAL" Or lConn.Source.Member = "RRQAL" Or _
                       lConn.Source.Member = "IDQAL" Or lConn.Source.Member = "PDQAL" Or _
                       lConn.Source.Member = "GQADDR" Or lConn.Source.Member = "GQADWT" Or _
                       lConn.Source.Member = "GQADEP" Or lConn.Source.Member = "RODQAL" Or _
                       lConn.Source.Member = "TROQAL" Then
                        If lrflag And lConn.Source.MemSub1 > lrcount Then
                            lConn.Source.MemSub1 = lConn.Source.MemSub1 - 1
                        End If
                    Else
                        If lrflag And lConn.Source.MemSub2 > lrcount Then
                            lConn.Source.MemSub2 = lConn.Source.MemSub2 - 1
                        End If
                    End If
                ElseIf lConn.Source.VolName = "PERLND" And lConn.Source.Group = "PQUAL" Then
                    If lConn.Source.Member = "SOQSP" Or lConn.Source.Member = "WASHQS" Or _
                       lConn.Source.Member = "SCRQS" Or lConn.Source.Member = "SOQS" Then
                        If lpsflag And lConn.Source.MemSub1 > lpscount Then
                            lConn.Source.MemSub1 = lConn.Source.MemSub1 - 1
                        End If
                    Else
                        If lpflag And lConn.Source.MemSub1 > lpcount Then
                            lConn.Source.MemSub1 = lConn.Source.MemSub1 - 1
                        End If
                    End If
                ElseIf lConn.Source.VolName = "IMPLND" And lConn.Source.Group = "IQUAL" Then
                    If lConn.Source.Member = "SOQSP" Or lConn.Source.Member = "SOQS" Then
                        If lisflag And lConn.Source.MemSub1 = liscount Then
                            lConn.Source.MemSub1 = lConn.Source.MemSub1 - 1
                        End If
                    Else
                        If liflag And lConn.Source.MemSub1 = licount Then
                            lConn.Source.MemSub1 = lConn.Source.MemSub1 - 1
                        End If
                    End If
                End If
            End If
            If Not lRemflag Then
                lConnIndex += 1
            End If
        Loop

    End Sub

    Private Sub cbxSelect_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxSelect.CheckedChanged
        If Not pLoading Then
            For lRow As Integer = 0 To clbPollutants.Items.Count - 1
                clbPollutants.SetItemChecked(lRow, cbxSelect.Checked)
            Next
        End If
    End Sub

    Private Sub frmPollutant_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyValue = Windows.Forms.Keys.F1 Then
            ShowHelp(pWinHSPFManualName)
            ShowHelp("User's Guide\Detailed Functions\Pollutant Selection.html")
        End If
    End Sub
End Class