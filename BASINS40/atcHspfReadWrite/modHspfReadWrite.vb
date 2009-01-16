Imports System.Collections.ObjectModel
Imports MapWinUtility
Imports atcUtility

Module modHspfReadWrite
    Dim pTestPath As String
    Dim pBaseName As String
    Dim pBmpSpecFileName As String
    Dim pLuChangeSpecFileName As String
    Dim pEdit As Boolean = False
    Dim pMassLinkOffsetId As Integer = 10

    Sub Initialize()
        Dim lTestName As String
        'lTestName = "SFBayColma"
        'lTestName = "lu2030a2bmp"
        'lTestName = "lu2030b2bmp"
        'lTestName = "Mono10bmp"
        'lTestName = "Mono70bmp"
        'lTestName = "lu2090a2bmp"
        lTestName = "lu2090b2bmp"

        Select Case lTestName
            Case "mono"
                pTestPath = "d:\mono_base"
                pBaseName = "base"
                pEdit = True
                pBmpSpecFileName = pTestPath & "\BMPSpecs.txt"
                pLuChangeSpecFileName = pTestPath & "\LuChangeSpecs.txt"
            Case "lu2030a2bmp"
                pTestPath = "c:\mono_luChange\output\lu2030a2bmp"
                pBaseName = "base"
                pEdit = True
                pBmpSpecFileName = pTestPath & "\BMPSpecs.txt"
                pLuChangeSpecFileName = pTestPath & "\LuChangeSpecs.txt"
            Case "lu2030b2bmp"
                pTestPath = "c:\mono_luChange\output\lu2030b2bmp"
                pBaseName = "base"
                pEdit = True
                pBmpSpecFileName = pTestPath & "\BMPSpecs.txt"
                pLuChangeSpecFileName = pTestPath & "\LuChangeSpecs.txt"
            Case "Mono10bmp"
                pTestPath = "c:\mono_luChange\output\Mono10bmp"
                pBaseName = "base"
                pEdit = True
                pBmpSpecFileName = pTestPath & "\BMPSpecs.txt"
                pLuChangeSpecFileName = pTestPath & "\LuChangeSpecs.txt"
            Case "Mono70bmp"
                pTestPath = "c:\mono_luChange\output\Mono70bmp"
                pBaseName = "base"
                pEdit = True
                pBmpSpecFileName = pTestPath & "\BMPSpecs.txt"
                pLuChangeSpecFileName = pTestPath & "\LuChangeSpecs.txt"
            Case "lu2090a2bmp"
                pTestPath = "c:\mono_luChange\output\lu2090a2bmp"
                pBaseName = "base"
                pEdit = True
                pBmpSpecFileName = pTestPath & "\BMPSpecs.txt"
                pLuChangeSpecFileName = pTestPath & "\LuChangeSpecs.txt"
            Case "lu2090b2bmp"
                pTestPath = "c:\mono_luChange\output\lu2090b2bmp"
                pBaseName = "base"
                pEdit = True
                pBmpSpecFileName = pTestPath & "\BMPSpecs.txt"
                pLuChangeSpecFileName = pTestPath & "\LuChangeSpecs.txt"
            Case "SFBayColma"
                pTestPath = "G:\SFBay\Task1\UCOLMAnew"
                pBaseName = "UCOLMA"
        End Select

    End Sub

    Sub main()
        Initialize()
        atcUtility.ChDriveDir(pTestPath)

        Logger.StartToFile(pBaseName & ".log")

        Dim lMsg As New atcUCI.HspfMsg("hspfmsg.mdb")

        Dim lHspfUci As New atcUCI.HspfUci
        With lHspfUci
            .FastReadUciForStarter(lMsg, pBaseName & ".uci")
            .Name = pBaseName & ".rev.uci"
            If pEdit Then
                SaveFileString("AreaSummaryBase.txt", .AreaReport(True))
                AddBMPs(lHspfUci, pMassLinkOffsetId, pBmpSpecFileName, pLuChangeSpecFileName, True)
                SaveFileString("AreaSummaryFinal.txt", .AreaReport(True))
            End If
            .Save()
        End With
        Logger.Flush()
        lHspfUci = Nothing
        lHspfUci = New atcUCI.HspfUci
        With lHspfUci
            .FastReadUciForStarter(lMsg, pBaseName & ".rev.uci")
            .Name = pBaseName & ".rev2.uci"
            .SaveAs(pBaseName, pBaseName & "New", 1, 1)
        End With
        MsgBox("Done adding BMPs into " & pTestPath)
    End Sub

    Private Sub AddBMPs(ByVal aUci As atcUCI.HspfUci, _
                        ByVal aMassLinkIdOffset As Integer, _
                        ByVal aBmpSpecFileName As String, _
                        ByVal aLuChangeSpecFileName As String, _
                        ByVal aAreaConversion As Boolean)
        Dim lFormat As String = "#######.00"
        With aUci 'add new masslinks based on old ones
            Dim lBmpracRchresMassLinkId As Integer
            Dim lNewMassLinks As New Collection(Of atcUCI.HspfMassLink)
            For Each lMassLinkExisting As atcUCI.HspfMassLink In .MassLinks
                If lMassLinkExisting.Target.VolName = "RCHRES" Then

                    'TODO: dont add BMPRAC masslink if it already exists!

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

            Dim lOperationIndex As Integer = 0

            If aAreaConversion Then
                Logger.Dbg("Converting Areas")
                Dim lLuSpecTable As New atcUtility.atcTableDelimited
                lLuSpecTable.Delimiter = ","
                lLuSpecTable.OpenFile(aLuChangeSpecFileName)
                Dim lConvertedArea As Double = 0.0
                Dim lNotConvertedArea As Double = 0.0
                lOperationIndex = 0
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
                                Dim lId As Integer = CInt(lSourceConnection.Source.Opn.Id / 100)
                                Dim lFromLUName As String = lName.Substring(lName.Length - 3)
                                If lLuSpecTable.FindFirst(1, lFromLUName) Then
                                    Do
                                        Dim lToLUName As String = lLuSpecTable.Value(2)
                                        Dim lConvertFrac As Double = lLuSpecTable.Value(3)
                                        Dim lNewSourceConnectionIndex As Integer = 0
                                        Dim lAreaConverted As Boolean = False
                                        Dim lAreaConvertedNow As Double = 0.0
                                        While lNewSourceConnectionIndex < lOperation.Sources.Count
                                            Dim lNewSourceConnection As atcUCI.HspfConnection = lOperation.Sources(lNewSourceConnectionIndex)
                                            If lNewSourceConnection.Source.VolName = "PERLND" OrElse _
                                               lNewSourceConnection.Source.VolName = "IMPLND" Then
                                                Dim lNewSourceName As String = lNewSourceConnection.Source.Opn.Tables("GEN-INFO").Parms(0).Value
                                                Dim lNewSourceId As String = CInt(lNewSourceConnection.Source.Opn.Id / 100)
                                                If lNewSourceName.Substring(lNewSourceName.Length - 3) = lToLUName AndAlso _
                                                   lNewSourceId = lId Then 'add area here
                                                    Dim lNewSourceOperation As atcUCI.HspfOperation = lNewSourceConnection.Source.Opn
                                                    If lSourceConnection.Comment.Length = 0 Then
                                                        lSourceConnection.Comment = "*** Area Before LUConversion " & lSourceConnection.MFact
                                                    End If
                                                    With lNewSourceConnection
                                                        lAreaConvertedNow = lSourceConnection.Comment.Substring(29) * lConvertFrac
                                                        If lAreaConvertedNow > lSourceConnection.MFact Then
                                                            Logger.Dbg(" From " & IdWithName(lSourceConnection.Source.Opn.Id) & _
                                                                       " To " & IdWithName(.Source.Opn.Id) & _
                                                                       " BadArea1 " & Format(lAreaConvertedNow, lFormat).PadLeft(10) & _
                                                                       " AreaAvail " & Format(lSourceConnection.MFact, lFormat).PadLeft(10) & _
                                                                       " " & lSourceConnection.Comment)
                                                            lAreaConvertedNow = lSourceConnection.MFact
                                                        Else
                                                            Logger.Dbg(" From " & IdWithName(lSourceConnection.Source.Opn.Id) & _
                                                                       " To " & IdWithName(.Source.Opn.Id) & _
                                                                       " Convert1 " & Format(lAreaConvertedNow, lFormat).PadLeft(10) & _
                                                                       " AreaAvail " & Format(lSourceConnection.MFact, lFormat).PadLeft(10) & _
                                                                       " " & lSourceConnection.Comment)
                                                        End If
                                                        .MFact += Format(lAreaConvertedNow, lFormat)
                                                        .MFactAsRead = ""
                                                        lSourceConnection.MFact -= Format(lAreaConvertedNow, lFormat)
                                                        lSourceConnection.MFactAsRead = ""
                                                    End With
                                                    lAreaConverted = True
                                                    Exit While
                                                End If
                                            End If
                                            lNewSourceConnectionIndex += 1
                                        End While
                                        If Not lAreaConverted Then 'can we add a new schematic entry?
                                            Dim lOperationOffset As Integer = 0
                                            Select Case lToLUName
                                                Case "nal"
                                                    lOperationOffset = 12
                                                Case "npa"
                                                    lOperationOffset = 17
                                            End Select
                                            Dim lOperationId As Integer = (CInt(lSourceConnection.Source.Opn.Id / 100)) * 100 + lOperationOffset
                                            Dim lOperationKey As String = "K" & lOperationId
                                            If .OpnBlks("PERLND").Ids.Contains(lOperationKey) Then 'yes we can
                                                Dim lFromOperationNew As atcUCI.HspfOperation = .OpnBlks("PERLND").Ids(lOperationKey)
                                                Dim lConnection As atcUCI.HspfConnection = lSourceConnection.Clone
                                                If lSourceConnection.Comment.Length = 0 Then
                                                    lSourceConnection.Comment = "*** Area Before LUConversion " & lSourceConnection.MFact
                                                End If
                                                With lConnection
                                                    .Comment = ""
                                                    .Source.Opn = lFromOperationNew
                                                    .Source.VolId = lFromOperationNew.Id
                                                    lAreaConvertedNow = lSourceConnection.Comment.Substring(29) * lConvertFrac
                                                    If lAreaConvertedNow > lSourceConnection.MFact Then
                                                        Logger.Dbg(" From " & IdWithName(lSourceConnection.Source.Opn.Id) & _
                                                                   " To " & IdWithName(.Source.Opn.Id) & _
                                                                   " BadArea2 " & Format(lAreaConvertedNow, lFormat).PadLeft(10) & _
                                                                   " AreaAvail " & Format(lSourceConnection.MFact, lFormat).PadLeft(10) & _
                                                                   " " & lSourceConnection.Comment)
                                                        lAreaConvertedNow = lSourceConnection.MFact
                                                    Else
                                                        Logger.Dbg(" From " & IdWithName(lSourceConnection.Source.Opn.Id) & _
                                                                   " To " & IdWithName(.Source.Opn.Id) & _
                                                                   " Convert2 " & Format(lAreaConvertedNow, lFormat).PadLeft(10) & _
                                                                   " AreaAvail " & Format(lSourceConnection.MFact, lFormat).PadLeft(10) & _
                                                                   " " & lSourceConnection.Comment)
                                                    End If
                                                    .MFact = Format(lAreaConvertedNow, lFormat)
                                                    .MFactAsRead = ""
                                                    lSourceConnection.MFact -= Format(lAreaConvertedNow, lFormat)
                                                    lSourceConnection.MFactAsRead = ""
                                                    lConnection.Source.Opn.Targets.Add(lConnection)
                                                End With
                                                With lSourceConnection.Target.Opn.Sources
                                                    Dim lInsertPosition As Integer = 0
                                                    While lInsertPosition < .Count
                                                        Dim lSource As atcUCI.HspfSrcTar = .Item(lInsertPosition).Source
                                                        If lSource.Opn IsNot Nothing AndAlso lSource.Opn.Id > lOperationId Then
                                                            Exit While
                                                        End If
                                                        lInsertPosition += 1
                                                    End While
                                                    .Insert(lInsertPosition, lConnection)
                                                    lAreaConverted = True
                                                End With
                                            End If
                                        End If
                                        If lAreaConverted Then
                                            lConvertedArea += lAreaConvertedNow
                                        Else
                                            Logger.Dbg("CantConvert " & lFromLUName & " to " & lToLUName & _
                                                        " in " & lSourceConnection.Target.Opn.Id & " from " & lSourceConnection.Source.Opn.Id & _
                                                        " area " & lSourceConnection.MFact & " frac " & lConvertFrac)
                                            lNotConvertedArea += lSourceConnection.MFact * lConvertFrac
                                        End If
                                    Loop While lLuSpecTable.FindNext(1, lFromLUName)
                                End If
                            End If
                            lSourceConnectionIndex += 1
                        End While
                    End If
                    lOperationIndex += 1
                End While
                Debug.Print("AreaConverted " & Format(lConvertedArea) & " Not " & Format(lNotConvertedArea))
                SaveFileString("AreaSummaryLuConvert.txt", .AreaReport(True))
            Else
                Logger.Dbg("Skip Converting Areas")
            End If

            Dim lBmpSpecTable As New atcUtility.atcTableDelimited
            lBmpSpecTable.Delimiter = ","
            lBmpSpecTable.OpenFile(aBmpSpecFileName)
            'add bmprac operations template
            Dim lBmpOpnBlkTemplate As atcUCI.HspfOpnBlk = BmpOperationsFromSpecs(lBmpSpecTable, .Msg)

            lOperationIndex = 0
            While lOperationIndex < .OpnSeqBlock.Opns.Count
                Dim lOperation As atcUCI.HspfOperation = .OpnSeqBlock.Opns(lOperationIndex)
                If lOperation.OpTyp = atcUCI.HspfData.HspfOperType.hRchres Then
                    'update schematic as needed
                    Logger.Dbg("UpdateSchematicForRchres " & lOperation.Id & " " & lOperation.Description)
                    Dim lSourceConnectionIndex As Integer = 0
                    While lSourceConnectionIndex < lOperation.Sources.Count
                        Dim lSourceConnection As atcUCI.HspfConnection = lOperation.Sources(lSourceConnectionIndex)
                        If lSourceConnection.Source.VolName = "PERLND" OrElse _
                           lSourceConnection.Source.VolName = "IMPLND" Then
                            Dim lName As String = lSourceConnection.Source.Opn.Tables("GEN-INFO").Parms(0).Value
                            Dim lLandUseName As String = lName.Substring(lName.Length - 3)
                            If lBmpSpecTable.FindFirst(1, lLandUseName) Then
                                For lBmpIndex As Integer = 2 To lBmpSpecTable.NumFields
                                    If lBmpSpecTable.Value(lBmpIndex) > 0 Then 'need a bmp operation
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
                                        If lSourceConnection.Comment.Length = 0 OrElse _
                                           (lSourceConnection.Comment.Contains("LUConversion") AndAlso _
                                            Not lSourceConnection.Comment.Contains("BMP")) Then
                                            If lSourceConnection.Comment.Length > 0 Then
                                                lSourceConnection.Comment &= vbCrLf
                                            End If
                                            If lSourceConnection.MFact < 0 Then
                                                Logger.Dbg("BadArea")
                                                lSourceConnection.MFact = 0
                                            End If
                                            lSourceConnection.Comment &= "*** Area Before BMP " & Format(lSourceConnection.MFact, lFormat)
                                        End If
                                        Dim lRemovalFrac As Double = lBmpSpecTable.Value(lBmpIndex)
                                        With lConnection
                                            .Comment = ""
                                            .MassLink = lSourceConnection.MassLink + aMassLinkIdOffset
                                            Dim lStartAreaPosition As Integer = lSourceConnection.Comment.LastIndexOf(" ")
                                            Dim lAreaConvertedNow As Double = lSourceConnection.Comment.Substring(lStartAreaPosition) * lRemovalFrac
                                            If lAreaConvertedNow < 0 Then
                                                Debug.Print("BadArea")
                                                lAreaConvertedNow = 0
                                            End If
                                            .MFact = Format(lAreaConvertedNow, lFormat)
                                            .MFactAsRead = ""
                                            Logger.Dbg(" BMP " & lBmpId & _
                                                       " Reach " & .Target.VolId & _
                                                       " " & lSourceConnection.Source.VolName & " " & lSourceConnection.Source.VolId & _
                                                       " Convert3 " & Format(lAreaConvertedNow, lFormat).PadLeft(10) & _
                                                       " AreaAvail " & Format(lSourceConnection.MFact, lFormat).PadLeft(10) & _
                                                       " " & lSourceConnection.Comment.Replace(vbCrLf, vbTab))
                                            lSourceConnection.MFact -= Format(lAreaConvertedNow, lFormat)
                                            lSourceConnection.MFactAsRead = ""
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

    Private Function IdWithName(ByVal aId As Integer) As String
        Dim lSeg As Integer
        Math.DivRem(aId, 100, lSeg)
        Dim lName As String = "unk"
        Select Case lSeg
            Case 1 : lName = "alf"
            Case 2 : lName = "bar"
            Case 3 : lName = "ext"
            Case 4 : lName = "for"
            Case 5 : lName = "grs"
            Case 6 : lName = "hom"
            Case 7 : lName = "hvf"
            Case 8 : lName = "hwm"
            Case 9 : lName = "hyo"
            Case 10 : lName = "hyw"
            Case 11 : lName = "lwm"
            Case 12 : lName = "nal"
            Case 13 : lName = "nhi"
            Case 14 : lName = "nho"
            Case 15 : lName = "nhy"
            Case 16 : lName = "nlo"
            Case 17 : lName = "npa"
            Case 18 : lName = "pas"
            Case 19 : lName = "puh"
            Case 20 : lName = "pul"
            Case 21 : lName = "trp"
            Case 22 : lName = "urs"
            Case 23 : lName = "afo"
            Case 24 : lName = "imh"
            Case 25 : lName = "iml"
        End Select
        Dim lIdWithName As String = aId.ToString & "(" & lName & ")"
        Return lIdWithName
    End Function
End Module
