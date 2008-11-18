Imports System.Collections.ObjectModel

Module modHspfReadWrite
    Dim pTestPath As String
    Dim pBaseName As String
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
                AddBMPs(lHspfUci, pMassLinkOffsetId)
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

    Private Sub AddBMPs(ByVal aUci As atcUCI.HspfUci, ByVal aMassLinkIdOffset As Integer)
        With aUci
            'add new masslinks based on old ones
            Dim lMassLinksNew As New Collection(Of atcUCI.HspfMassLink)
            For Each lMassLinkExisting As atcUCI.HspfMassLink In .MassLinks
                If lMassLinkExisting.Target.VolName = "RCHRES" AndAlso _
                   lMassLinkExisting.Source.VolName <> "RCHRES" Then
                    Dim lMassLink As atcUCI.HspfMassLink = lMassLinkExisting.Clone
                    With lMassLink
                        .Target.VolName = "BMPRAC"
                        .MassLinkId += aMassLinkIdOffset
                    End With
                    lMassLinksNew.Add(lMassLink)
                End If
            Next
            For Each lMassLinkNew As atcUCI.HspfMassLink In lMassLinksNew
                .MassLinks.Add(lMassLinkNew)
            Next

            'add bmprac operations
            Dim lBmpNames As New Collection
            lBmpNames.Add("BMP1")
            lBmpNames.Add("BMP2")
            lBmpNames.Add("BMP3")
            lBmpNames.Add("BMP4")
            Dim lOperationIndex As Integer = 0
            While lOperationIndex < .OpnSeqBlock.Opns.Count
                Dim lOperation As atcUCI.HspfOperation = .OpnSeqBlock.Opns(lOperationIndex)
                If lOperation.OpTyp = atcUCI.HspfData.HspfOperType.hRchres Then
                    Dim lBmpIndex As Integer = 1
                    For Each lBmpName As String In lBmpNames
                        Dim lNewBmprac As atcUCI.HspfOperation = _
                           .AddOperation("BMPRAC", (10 * lOperation.Id) + lBmpIndex) 'TODO: need better way to calc ID
                        .OpnSeqBlock.Opns.Insert(lOperationIndex, lNewBmprac)
                        lOperationIndex += 1
                        lBmpIndex += 1
                    Next
                End If
                lOperationIndex += 1
            End While

            For Each lOperation As atcUCI.HspfOperation In .OpnBlks("BMPRAC").Ids
                .AddTable("BMPRAC", lOperation.Id, "PRINT-INFO")
                .AddTable("BMPRAC", lOperation.Id, "GEN-INFO")
                .AddTable("BMPRAC", lOperation.Id, "SED-FRAC")
                .AddTable("BMPRAC", lOperation.Id, "DNUT-FRAC")
                .AddTable("BMPRAC", lOperation.Id, "ADSNUT-FRAC")
                With lOperation
                    .Tables("PRINT-INFO").Parms(0).Value = 5
                    .Tables("PRINT-INFO").Parms(1).Value = 5
                    .Tables("PRINT-INFO").Parms(2).Value = 5
                    .Tables("PRINT-INFO").Parms(3).Value = 5
                    .Tables("PRINT-INFO").Parms(4).Value = 5
                    .Tables("PRINT-INFO").Parms(5).Value = 5
                    .Tables("PRINT-INFO").Parms(6).Value = 5
                    .Tables("PRINT-INFO").Parms(7).Value = 5
                    .Tables("PRINT-INFO").Parms(9).Value = 1
                    .Tables("PRINT-INFO").Parms(10).Value = 9
                    .Tables("GEN-INFO").Parms(3).Value = 3
                    .Tables("GEN-INFO").Parms(6).Value = 91

                    .Tables("GEN-INFO").Parms(0).Value = "New Development"
                    .Tables("SED-FRAC").Parms(0).Value = 0.8
                    .Tables("SED-FRAC").Parms(1).Value = 0.8
                    .Tables("SED-FRAC").Parms(2).Value = 0.8
                    .Tables("DNUT-FRAC").Parms(3).Value = 0.5
                    .Tables("ADSNUT-FRAC").Parms(3).Value = 0.5
                    .Tables("ADSNUT-FRAC").Parms(4).Value = 0.5
                    .Tables("ADSNUT-FRAC").Parms(5).Value = 0.5
                End With
            Next lOperation

            'update schematic
        End With
    End Sub
End Module
