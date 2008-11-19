Imports System.Collections.ObjectModel
Imports atcUtility

Module modHspfReadWrite
    Dim pTestPath As String
    Dim pBaseName As String
    Dim pBmpSpecFileName As String
    Dim pEdit As Boolean = False
    Dim pMassLinkOffsetId As Integer = 10

    Sub Initialize()
        Dim lTestName As String
        'lTestName = "SFBayColma"
        lTestName = "mono"

        Select Case lTestName
            Case "mono"
                pTestPath = "d:\mono_base"
                pBaseName = "base"
                pEdit = True
                pBmpSpecFileName = pTestPath & "\BMPSpecs.txt"
            Case "SFBayColma"
                pTestPath = "G:\SFBay\Task1\UCOLMAnew"
                pBaseName = "UCOLMA"
        End Select
    End Sub

    Sub main()
        Initialize()
        atcUtility.ChDriveDir(pTestPath)

        Dim lMsg As New atcUCI.HspfMsg("hspfmsg.mdb")

        Dim lHspfUci As New atcUCI.HspfUci
        With lHspfUci
            .FastReadUciForStarter(lMsg, pBaseName & ".uci")
            .Name = pBaseName & ".rev.uci"
            If pEdit Then
                AddBMPs(lHspfUci, pMassLinkOffsetId, pBmpSpecFileName)
            End If
            .Save()
        End With
        lHspfUci = Nothing
        lHspfUci = New atcUCI.HspfUci
        With lHspfUci
            .FastReadUciForStarter(lMsg, pBaseName & ".rev.uci")
            .Name = pBaseName & ".rev2.uci"
            .SaveAs(pBaseName, pBaseName & "New", 1, 1)
        End With
    End Sub

    Private Sub AddBMPs(ByVal aUci As atcUCI.HspfUci, _
                        ByVal aMassLinkIdOffset As Integer, _
                        ByVal aBmpSpecFileName As String)
        With aUci
            'add new masslinks based on old ones
            Dim lBmpracRchresMassLinkId As Integer
            Dim lNewMassLinks As New Collection(Of atcUCI.HspfMassLink)
            For Each lMassLinkExisting As atcUCI.HspfMassLink In .MassLinks
                If lMassLinkExisting.Target.VolName = "RCHRES" Then
                    Dim lMassLink As atcUCI.HspfMassLink = lMassLinkExisting.Clone
                    With lMassLink
                        .MassLinkId += aMassLinkIdOffset
                        If .Source.VolName = "RCHRES" Then
                            lBmpracRchresMassLinkId = .MassLinkId
                            With .Source
                                .VolName = "BMPRAC"
                                .Group = "ROFLOW"
                                .Member = ""
                                .MemSub1 = 0
                            End With
                        Else
                            .Target.VolName = "BMPRAC"
                            If .Target.Member = "OXIF" Then
                                .Target.Member = "IOX"
                            ElseIf .Target.Member = "NUIF1" Then
                                .Target.Member = "IDNUT"
                            ElseIf .Target.Member = "NUIF2" Then
                                .Target.Member = "ISNUT"
                            ElseIf .Target.Member = "PKIF" Then
                                .Target.Member = "IPLK"
                            End If
                        End If
                    End With
                    lNewMassLinks.Add(lMassLink)
                End If
            Next
            For Each lNewMassLink As atcUCI.HspfMassLink In lNewMassLinks
                .MassLinks.Add(lNewMassLink)
            Next

            Dim lBmpRemovalTable As New atcUtility.atcTableDelimited
            lBmpRemovalTable.Delimiter = ","
            lBmpRemovalTable.OpenFile(aBmpSpecFileName)

            'add bmprac operations template
            Dim lBmpOpnBlkTemplate As atcUCI.HspfOpnBlk = BmpOperationsFromSpecs(lBmpRemovalTable, .Msg)

            Dim lOperationIndex As Integer = 0
            While lOperationIndex < .OpnSeqBlock.Opns.Count
                Dim lOperation As atcUCI.HspfOperation = .OpnSeqBlock.Opns(lOperationIndex)
                If lOperation.OpTyp = atcUCI.HspfData.HspfOperType.hRchres Then
                    'update schematic as needed
                    Dim lSourceConnectionIndex As Integer = 0
                    While lSourceConnectionIndex < lOperation.Sources.Count
                        Dim lSourceConnection As atcUCI.HspfConnection = lOperation.Sources(lSourceConnectionIndex)
                        If lSourceConnection.Source.VolName = "PERLND" OrElse _
                           lSourceConnection.Source.VolName = "IMPLND" Then
                            Dim lName As String = lSourceConnection.Source.Opn.Tables("GEN-INFO").Parms(0).Value
                            Dim lLandUseName As String = lName.Substring(lName.Length - 3)
                            If lBmpRemovalTable.FindFirst(1, lLandUseName) Then
                                For lBmpIndex As Integer = 2 To lBmpRemovalTable.NumFields
                                    If lBmpRemovalTable.Value(lBmpIndex) > 0 Then 'need a bmp operation
                                        Dim lBmpOperationTemplate As atcUCI.HspfOperation = lBmpOpnBlkTemplate.Ids(lBmpIndex - 2)
                                        Dim lBmpId As Integer = (10 * lOperation.Id) + lBmpIndex - 1
                                        If Not .OperationExists("BMPRAC", lBmpId) Then
                                            Dim lNewBmprac As atcUCI.HspfOperation = .AddOperation("BMPRAC", lBmpId)
                                            lNewBmprac.Tables.AddRange(lBmpOperationTemplate.Tables.Clone, lNewBmprac)
                                            .OpnSeqBlock.Opns.Insert(lOperationIndex, lNewBmprac)
                                            Dim lConnectionBmp As New atcUCI.HspfConnection
                                            With lConnectionBmp
                                                .Typ = 3
                                                .Uci = aUci
                                                .Source.VolName = "BMPRAC"
                                                .Source.VolId = lBmpId
                                                .Source.Opn = lNewBmprac
                                                .MassLink = lBmpracRchresMassLinkId
                                                .Target.VolName = "RCHRES"
                                                .Target.VolId = lOperation.Id
                                                .Target.Opn = lOperation
                                            End With
                                            .Connections.Add(lConnectionBmp)
                                            lOperationIndex += 1
                                            lOperation.Sources.Add(lConnectionBmp)
                                        End If

                                        Dim lConnection As atcUCI.HspfConnection = lSourceConnection.Clone
                                        If lSourceConnection.Comment.Length = 0 Then
                                            lSourceConnection.Comment = "*** Area Before BMP " & lSourceConnection.MFact
                                        End If
                                        Dim lRemovalFrac As Double = lBmpRemovalTable.Value(lBmpIndex)
                                        With lConnection
                                            .Comment = ""
                                            .MassLink = lSourceConnection.MassLink + aMassLinkIdOffset
                                            .MFact = DoubleToString(lSourceConnection.Comment.Substring(20) * lRemovalFrac)
                                            lSourceConnection.MFact -= .MFact
                                            .MFactAsRead = .MFact.ToString.PadLeft(10)
                                            .Target.VolName = "BMPRAC"
                                            .Target.VolId = (10 * lOperation.Id) + lBmpIndex - 1
                                            Dim lBmpOperation As atcUCI.HspfOperation = aUci.OpnBlks("BMPRAC").OperFromID(.Target.VolId)
                                            .Target.Opn = lBmpOperation
                                            lConnection.Source.Opn.Targets.Add(lConnection)
                                            lBmpOperation.Sources.Add(lConnection)
                                        End With
                                        .Connections.Add(lConnection)
                                    End If
                                Next
                            End If
                        End If
                        lSourceConnectionIndex += 1
                    End While
                End If
                lOperationIndex += 1
            End While
        End With
    End Sub

    Private Function BmpOperationsFromSpecs(ByVal aBmpRemovalTable As atcUtility.atcTableDelimited, _
                                            ByVal aMsg As atcUCI.HspfMsg) As atcUCI.HspfOpnBlk
        Const lBmpRacName As String = "BMPRAC"
        Dim lBmpOpnBlkTemplate As New atcUCI.HspfOpnBlk
        Dim lBmpNames As New Collection
        For lFieldIndex As Integer = 2 To aBmpRemovalTable.NumFields
            lBmpNames.Add(aBmpRemovalTable.FieldName(lFieldIndex))
        Next
        Dim lBmpIndex As Integer = 0
        For Each lBmpName As String In lBmpNames
            lBmpIndex += 1
            Dim lBmpOperation As New atcUCI.HspfOperation
            lBmpOperation.Name = lBmpRacName
            lBmpOperation.Id = lBmpIndex
            lBmpOpnBlkTemplate.Ids.Add(lBmpOperation)
            With lBmpOpnBlkTemplate
                .AddTable(lBmpIndex, "PRINT-INFO", aMsg.BlockDefs.Item(lBmpRacName))
                .AddTable(lBmpIndex, "GEN-INFO", aMsg.BlockDefs.Item(lBmpRacName))
                .AddTable(lBmpIndex, "SED-FRAC", aMsg.BlockDefs.Item(lBmpRacName))
                .AddTable(lBmpIndex, "DNUT-FRAC", aMsg.BlockDefs.Item(lBmpRacName))
                .AddTable(lBmpIndex, "ADSNUT-FRAC", aMsg.BlockDefs.Item(lBmpRacName))
            End With
            With lBmpOperation
                .Tables("GEN-INFO").Parms(0).Value = lBmpName
                .Tables("GEN-INFO").Parms(6).Value = 26
                .Tables("PRINT-INFO").Parms(0).Value = 5
                .Tables("PRINT-INFO").Parms(1).Value = 5
                .Tables("PRINT-INFO").Parms(2).Value = 5
                .Tables("PRINT-INFO").Parms(3).Value = 5
                .Tables("PRINT-INFO").Parms(4).Value = 5
                .Tables("PRINT-INFO").Parms(5).Value = 5
                .Tables("PRINT-INFO").Parms(6).Value = 5
                .Tables("PRINT-INFO").Parms(7).Value = 5
                .Tables("PRINT-INFO").Parms(9).Value = 1
                .Tables("PRINT-INFO").Parms(10).Value = 12
                'TODO - get these parms from the spec table (or another table)
                Select Case lBmpName
                    Case "AllAg"
                        .Tables("SED-FRAC").Parms(0).Value = 0.17
                        .Tables("SED-FRAC").Parms(1).Value = 0.17
                        .Tables("SED-FRAC").Parms(2).Value = 0.17
                        .Tables("DNUT-FRAC").Parms(0).Value = 0.07
                        .Tables("DNUT-FRAC").Parms(1).Value = 0.07
                        .Tables("DNUT-FRAC").Parms(2).Value = 0.07
                        .Tables("DNUT-FRAC").Parms(3).Value = 0.11
                        .Tables("ADSNUT-FRAC").Parms(0).Value = 0.07
                        .Tables("ADSNUT-FRAC").Parms(1).Value = 0.07
                        .Tables("ADSNUT-FRAC").Parms(2).Value = 0.07
                        .Tables("ADSNUT-FRAC").Parms(3).Value = 0.11
                        .Tables("ADSNUT-FRAC").Parms(4).Value = 0.11
                        .Tables("ADSNUT-FRAC").Parms(5).Value = 0.11
                    Case "CropLand"
                        .Tables("SED-FRAC").Parms(0).Value = 0.04
                        .Tables("SED-FRAC").Parms(1).Value = 0.04
                        .Tables("SED-FRAC").Parms(2).Value = 0.04
                        .Tables("DNUT-FRAC").Parms(0).Value = 0.3
                        .Tables("DNUT-FRAC").Parms(1).Value = 0.3
                        .Tables("DNUT-FRAC").Parms(2).Value = 0.3
                        .Tables("DNUT-FRAC").Parms(3).Value = 0.03
                        .Tables("ADSNUT-FRAC").Parms(0).Value = 0.3
                        .Tables("ADSNUT-FRAC").Parms(1).Value = 0.3
                        .Tables("ADSNUT-FRAC").Parms(2).Value = 0.3
                        .Tables("ADSNUT-FRAC").Parms(3).Value = 0.03
                        .Tables("ADSNUT-FRAC").Parms(4).Value = 0.03
                        .Tables("ADSNUT-FRAC").Parms(5).Value = 0.03
                    Case "Pasture"
                        .Tables("SED-FRAC").Parms(0).Value = 0.39
                        .Tables("SED-FRAC").Parms(1).Value = 0.39
                        .Tables("SED-FRAC").Parms(2).Value = 0.39
                        .Tables("DNUT-FRAC").Parms(0).Value = 0.24
                        .Tables("DNUT-FRAC").Parms(1).Value = 0.24
                        .Tables("DNUT-FRAC").Parms(2).Value = 0.24
                        .Tables("DNUT-FRAC").Parms(3).Value = 0.29
                        .Tables("ADSNUT-FRAC").Parms(0).Value = 0.24
                        .Tables("ADSNUT-FRAC").Parms(1).Value = 0.24
                        .Tables("ADSNUT-FRAC").Parms(2).Value = 0.24
                        .Tables("ADSNUT-FRAC").Parms(3).Value = 0.29
                        .Tables("ADSNUT-FRAC").Parms(4).Value = 0.29
                        .Tables("ADSNUT-FRAC").Parms(5).Value = 0.29
                    Case "Urban"
                        .Tables("SED-FRAC").Parms(0).Value = 0.12
                        .Tables("SED-FRAC").Parms(1).Value = 0.12
                        .Tables("SED-FRAC").Parms(2).Value = 0.12
                        .Tables("DNUT-FRAC").Parms(0).Value = 0.2
                        .Tables("DNUT-FRAC").Parms(1).Value = 0.2
                        .Tables("DNUT-FRAC").Parms(2).Value = 0.2
                        .Tables("DNUT-FRAC").Parms(3).Value = 0.26
                        .Tables("ADSNUT-FRAC").Parms(0).Value = 0.2
                        .Tables("ADSNUT-FRAC").Parms(1).Value = 0.2
                        .Tables("ADSNUT-FRAC").Parms(2).Value = 0.2
                        .Tables("ADSNUT-FRAC").Parms(3).Value = 0.26
                        .Tables("ADSNUT-FRAC").Parms(4).Value = 0.26
                        .Tables("ADSNUT-FRAC").Parms(5).Value = 0.26
                End Select
            End With
        Next
        Return lBmpOpnBlkTemplate
    End Function
End Module
