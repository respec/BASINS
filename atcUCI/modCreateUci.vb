'Copyright 2006 AQUA TERRA Consultants - Royalty-free use permitted under open source license
Option Strict Off
Option Explicit On

Imports System.Collections.ObjectModel
Imports MapWinUtility
Imports atcUtility
Imports atcData

Module modCreateUci
    Dim pLandUses As LandUses
    Dim pReaches As Reaches
    Dim pChannels As Channels
    Dim pPointLoads As PointLoads

    Dim pLandName(2) As String
    Dim pLastSeg(2) As Integer
    Dim pFirstSeg(2) As Integer

    Friend Sub CreateUciFromBASINS(ByRef aUci As HspfUci, _
                                   ByRef aMsg As HspfMsg, _
                                   ByRef aWsdName As String, _
                                   ByRef aDataSources As Collection(Of atcDataSource), _
                                   ByRef aMetBaseDsn As Integer, _
                                   ByRef aMetWdmId As String, _
                                   ByRef aStartDate() As Integer, _
                                   ByRef aEndDate() As Integer, _
                                   ByRef aOneSeg As Boolean, _
                                   ByRef aStarterUciName As String, _
                                   Optional ByRef aPollutantListFileName As String = "")

        If Not IO.File.Exists(aWsdName) Then
            aUci.ErrorDescription = "WsdFileName '" & aWsdName & "' not found"
        Else
            aUci.Name = FilenameOnly(aWsdName) & ".uci"
        End If

        aUci.Msg = aMsg

        If aUci.Name.Length > 0 Then
            Dim lScenario As String = FilenameOnly(aUci.Name)

            'dummy global block
            With aUci.GlobalBlock  'add global block to empty uci
                .Comment = " "
                .Uci = aUci
                .RunInf.Value = "UCI Created by WinHSPF for " & lScenario
                .emfg = 1
                .outlev.Value = CStr(1)
                .runfg = 1
            End With

            'add files block to uci
            CreateFilesBlock(aUci, lScenario, aDataSources)
            aUci.Save() 'TODO: required by prescanfilesblock, need in memory version of prescan...
            Dim lFilesBlockStatus As Boolean = False
            Dim lEchoFileName As String = ""
            aUci.PreScanFilesBlock(lFilesBlockStatus, lEchoFileName)

            Dim lReturnCode As Integer
            pLandUses = New LandUses
            lReturnCode = pLandUses.Open(aWsdName)
            pReaches = New Reaches
            lReturnCode = pReaches.Open(aWsdName)
            pChannels = New Channels
            lReturnCode = pChannels.Open(aWsdName)
            pPointLoads = New PointLoads
            lReturnCode = pPointLoads.Open(aWsdName)

            If lReturnCode = 0 Then  'everything read okay, continue
                'build initial met segment 
                DefaultBASINSMetseg(aUci, aMetBaseDsn, aMetWdmId)

                aUci.Initialized = True

                With aUci.GlobalBlock  'update start and end date from met data
                    For i As Integer = 0 To 5
                        .SDate(i) = aStartDate(i)
                        .EDate(i) = aEndDate(i)
                    Next i
                End With

                'add opn seq block
                CreateOpnSeqBlock(aUci, aOneSeg)

                'set all operation types
                Dim lOpnIndex As Integer = 1
                Dim lOpnName As String = HspfOperName(lOpnIndex)
                Dim lOpnBlk As HspfOpnBlk
                While lOpnName <> "UNKNOWN"
                    lOpnBlk = New HspfOpnBlk
                    lOpnBlk.Name = lOpnName
                    lOpnBlk.Uci = aUci
                    aUci.OpnBlks.Add(lOpnBlk)
                    lOpnIndex += 1
                    lOpnName = HspfOperName(lOpnIndex)
                End While

                'create tables for each operation
                For Each lOperation As HspfOperation In aUci.OpnSeqBlock.Opns
                    lOpnBlk = aUci.OpnBlks.Item(lOperation.Name)
                    lOpnBlk.Ids.Add(lOperation)
                    lOperation.OpnBlk = lOpnBlk
                Next
                For Each lOpnBlk In aUci.OpnBlks  'perlnd, implnd, etc
                    If lOpnBlk.Count > 0 Then
                        lOpnBlk.CreateTables(aMsg.BlockDefs.Item(lOpnBlk.Name))
                    End If
                Next

                For Each lChannel As Channel In pChannels 'process ftables
                    Dim lOpn As HspfOperation = aUci.OpnBlks.Item("RCHRES").OperFromID(CShort(lChannel.ChanId))
                    If Not lOpn Is Nothing Then
                        lOpn.FTable.FTableFromCrossSect(lChannel.ChanL, lChannel.ChanYm, lChannel.ChanWm, lChannel.ChanN, lChannel.ChanS, lChannel.ChanM11, lChannel.ChanM12, lChannel.ChanYc, lChannel.ChanM21, lChannel.ChanM22, lChannel.ChanYt1, lChannel.ChanYt2, lChannel.ChanM31, lChannel.ChanM32, lChannel.ChanW11, lChannel.ChanW12)
                    End If
                Next

                'create schematic, ext src blocks
                CreateConnectionsSchematic(aUci)
                CreateConnectionsMet(aUci)

                'set timeser connections
                For Each lOpn As HspfOperation In aUci.OpnSeqBlock.Opns
                    lOpn.setTimSerConnections()
                Next
                'create masslinks
                CreateMassLinks(aUci)

                'set initial values in uci from basins values
                SetInitValues(aUci)

                CreatePointSourceDSNs(aUci, aPollutantListFileName)

                CreateDefaultOutput(aUci)
                CreateBinaryOutput(aUci, lScenario)

                'look for met segments
                'newUci.Source2MetSeg

                'get starter uci ready
                Dim lDefUci As New HspfUci
                lDefUci.FastReadUciForStarter(aUci.Msg, aStarterUciName)

                'set default parameter values and mass links from starter
                SetDefault(aUci, lDefUci)
                SetDefaultMassLink(aUci, lDefUci)
            End If
        End If

        aUci.Edited = False 'all the reads set edited

    End Sub

    Private Sub SetInitValues(ByVal aUci As HspfUci)

        'set init values in uci
        For Each lOperation As HspfOperation In aUci.OpnBlks.Item("PERLND").Ids
            Dim lTable As HspfTable = lOperation.Tables.Item("ACTIVITY")
            lTable.Parms("PWATFG").Value = 1
            Dim lLandUse As LandUse = pLandUses(lOperation.Description & ":" & lOperation.Name & ":" & lOperation.Tag)
            If lLandUse Is Nothing Then
                Logger.Dbg("Missing Landuse" & lOperation.Description & ":" & lOperation.Name & ":" & lOperation.Tag)
            Else
                lTable = lOperation.Tables.Item("GEN-INFO")
                lTable.Parms("LSID").Value = lLandUse.Description
                lTable = lOperation.Tables.Item("PWAT-PARM2")
                If lLandUse.Slope > 0 Then
                    lTable.Parms("SLSUR").Value = lLandUse.Slope
                Else
                    lTable.Parms("SLSUR").Value = 0.001 'must have some slope
                End If
                'default lsur based on slsur
                lTable.Parms("LSUR").Value = DefaultLSURFromSLSUR(lTable.Parms("SLSUR").Value)
            End If
        Next lOperation

        For Each lOperation As HspfOperation In aUci.OpnBlks.Item("IMPLND").Ids
            Dim lTable As HspfTable = lOperation.Tables.Item("ACTIVITY")
            lTable.Parms("IWATFG").Value = 1
            Dim lLandUse As LandUse = pLandUses(lOperation.Description & ":" & lOperation.Name & ":" & lOperation.Tag)
            If lLandUse Is Nothing Then
                Logger.Dbg("Missing Landuse" & lOperation.Description & ":" & lOperation.Name & ":" & lOperation.Tag)
            Else
                lTable = lOperation.Tables.Item("GEN-INFO")
                lTable.Parms("LSID").Value = lLandUse.Description
                lTable = lOperation.Tables.Item("IWAT-PARM2")
                If lLandUse.Slope > 0 Then
                    lTable.Parms("SLSUR").Value = lLandUse.Slope
                Else
                    lTable.Parms("SLSUR").Value = 0.001 'must have some slope
                End If
                'default lsur based on slsur
                lTable.Parms("LSUR").Value = DefaultLSURFromSLSUR(lTable.Parms("SLSUR").Value)
            End If
        Next lOperation

        Dim j As Integer = -1
        For Each lOperation As HspfOperation In aUci.OpnBlks.Item("RCHRES").Ids
            j += 1
            Dim lTable As HspfTable = lOperation.Tables.Item("ACTIVITY")
            lTable.Parms("HYDRFG").Value = 1
            lTable = lOperation.Tables.Item("GEN-INFO")
            Dim lStr As String = pReaches(pReaches(j).Order).Name
            Dim lLen As Integer = lStr.Length
            If lLen < 19 And _
              (Not IsNumeric(pReaches(pReaches(j).Order).Id) Or _
               pReaches(pReaches(j).Order).Id.Length > 5) Then
                lStr &= " " & Right(pReaches(pReaches(j).Order).Id, 19 - lLen)
            End If
            lTable.Parms("RCHID").Value = lStr
            lTable.Parms("NEXITS").Value = pReaches(pReaches(j).Order).NExits
            lTable.Parms("PUNITE").Value = 91
            If pReaches(pReaches(j).Order).Type = "R" Then
                lTable.Parms("LKFG").Value = 1
            End If
            lTable = lOperation.Tables.Item("HYDR-PARM1")
            lTable.Parms("AUX1FG").Value = 1
            lTable.Parms("AUX2FG").Value = 1
            lTable.Parms("AUX3FG").Value = 1
            lTable.Parms("ODFVF1").Value = 4
            lTable = lOperation.Tables.Item("HYDR-PARM2")
            lTable.Parms("LEN").Value = pReaches(pReaches(j).Order).Length
            lTable.Parms("DELTH").Value = System.Math.Round(pReaches(pReaches(j).Order).DeltH, 0)
        Next lOperation
    End Sub

    Private Sub CreateMassLinks(ByRef aUci As HspfUci)
        Dim lMassLink As HspfMassLink
        Dim lopnblk As HspfOpnBlk

        For Each lopnblk In aUci.OpnBlks
            If lopnblk.Name = "PERLND" Then
                lMassLink = New HspfMassLink
                lMassLink.Uci = aUci
                lMassLink.MassLinkID = 2
                lMassLink.Source.VolName = "PERLND"
                lMassLink.Source.VolId = 0
                lMassLink.Source.Group = "PWATER"
                lMassLink.Source.Member = "PERO"
                lMassLink.MFact = 0.0833333
                lMassLink.Tran = ""
                lMassLink.Target.VolName = "RCHRES"
                lMassLink.Target.VolId = 0
                lMassLink.Target.Group = "INFLOW"
                lMassLink.Target.Member = "IVOL"
                aUci.MassLinks.Add(lMassLink)

            ElseIf lopnblk.Name = "IMPLND" Then
                lMassLink = New HspfMassLink
                lMassLink.Uci = aUci
                lMassLink.MassLinkID = 1
                lMassLink.Source.VolName = "IMPLND"
                lMassLink.Source.VolId = 0
                lMassLink.Source.Group = "IWATER"
                lMassLink.Source.Member = "SURO"
                lMassLink.MFact = 0.0833333
                lMassLink.Tran = ""
                lMassLink.Target.VolName = "RCHRES"
                lMassLink.Target.VolId = 0
                lMassLink.Target.Group = "INFLOW"
                lMassLink.Target.Member = "IVOL"
                aUci.MassLinks.Add(lMassLink)

            ElseIf lopnblk.Name = "RCHRES" Then
                lMassLink = New HspfMassLink
                lMassLink.Uci = aUci
                lMassLink.MassLinkID = 3
                lMassLink.Source.VolName = "RCHRES"
                lMassLink.Source.VolId = 0
                lMassLink.Source.Group = "ROFLOW"
                lMassLink.Source.Member = ""
                lMassLink.MFact = 1.0#
                lMassLink.Tran = ""
                lMassLink.Target.VolName = "RCHRES"
                lMassLink.Target.VolId = 0
                lMassLink.Target.Group = "INFLOW"
                lMassLink.Target.Member = ""
                aUci.MassLinks.Add(lMassLink)
            End If
        Next lopnblk
    End Sub

    Private Sub CreateConnectionsSchematic(ByRef aUci As HspfUci)
        Dim j, i As Integer
        Dim lConnection As HspfConnection

        For i = 0 To pReaches.Count - 1
            For j = 2 To 1 Step -1
                For Each lOperation As HspfOperation In aUci.OpnBlks.Item(pLandName(j)).Ids
                    Try
                        Dim lLandUse As LandUse = pLandUses(lOperation.Description & ":" & lOperation.Name & ":" & i + 1)
                        lConnection = New HspfConnection
                        lConnection.Uci = aUci
                        lConnection.Typ = 3
                        lConnection.Source.VolName = lOperation.Name
                        lConnection.Source.VolId = lOperation.Id
                        lConnection.MFact = lLandUse.Area
                        lConnection.Target.VolName = "RCHRES"
                        lConnection.Target.VolId = i + 1
                        lConnection.MassLink = TypeId(pLandUses(j).Type)
                        aUci.Connections.Add(lConnection)
                    Catch e As Collections.Generic.KeyNotFoundException
                    End Try
                Next
            Next
        Next

        For i = 0 To pReaches.Count - 1
            'add entries for each reach to reach connection
            For j = 0 To pReaches.Count - 1
                If pReaches(pReaches(j).Order).Id = pReaches(pReaches(i).Order).DownID Then
                    lConnection = New HspfConnection
                    lConnection.Uci = aUci
                    lConnection.Typ = 3
                    lConnection.Source.VolName = "RCHRES"
                    lConnection.Source.VolId = CInt(pReaches(pReaches(i).Order).Id)
                    lConnection.MFact = 1.0#
                    lConnection.Target.VolName = "RCHRES"
                    lConnection.Target.VolId = CInt(pReaches(pReaches(j).Order).Id)
                    lConnection.MassLink = 3
                    aUci.Connections.Add(lConnection)
                End If
            Next j
        Next i
    End Sub

    Private Sub CreateConnectionsMet(ByRef aUci As HspfUci)
        If aUci.MetSegs.Count > 0 Then
            Dim lOpTypes() As String = {"PERLND", "IMPLND", "RCHRES"} 'operations with assoc met segs
            For Each lOpTyp As String In lOpTypes
                For Each lOpn As HspfOperation In aUci.OpnBlks.Item(lOpTyp).Ids
                    lOpn.MetSeg = aUci.MetSegs.Item(0)
                Next
            Next
        Else
            Logger.Msg("No Met Segments Available", "CreateProblem")
        End If
    End Sub

    Private Sub CreateFilesBlock(ByRef aUci As HspfUci, _
                                 ByRef aScenario As String, _
                                 ByRef aDataSources As Collection(Of atcData.atcDataSource))

        Dim lFile As New HspfData.HspfFile
        aUci.FilesBlock.Clear()
        aUci.FilesBlock.Uci = aUci

        lFile.Comment = "<FILE>  <UN#>***<----FILE NAME------------------------------------------------->"
        lFile.Name = aScenario & ".ech"
        lFile.Typ = "MESSU"
        lFile.Unit = 24
        aUci.FilesBlock.Add(lFile)

        lFile.Comment = ""
        lFile.Name = aScenario & ".out"
        lFile.Typ = " "
        lFile.Unit = 91
        aUci.FilesBlock.Add(lFile)

        Dim lOutput As atcDataSource = aDataSources(0)
        If lOutput.Name.Length > 0 Then
            lFile.Name = RelativeFilename(lOutput.Specification, CurDir)
            lFile.Typ = lOutput.Name.Substring(lOutput.Name.LastIndexOf(":") + 1) & 1
            lFile.Unit = 25
            aUci.FilesBlock.Add(lFile)
        End If

        For lWdmIndex As Integer = 1 To aDataSources.Count - 1
            If Not aDataSources(lWdmIndex) Is Nothing AndAlso aDataSources(lWdmIndex).Name.Length > 0 Then
                lFile.Name = RelativeFilename(aDataSources(lWdmIndex).Specification, CurDir)
                With aDataSources(lWdmIndex)
                    lFile.Typ = .Name.Substring(.Name.LastIndexOf(":") + 1) & lWdmIndex + 1
                End With
                lFile.Unit = 25 + lWdmIndex
                aUci.FilesBlock.Add(lFile)
            End If
        Next lWdmIndex
    End Sub

    Private Sub CreateOpnSeqBlock(ByRef aUci As HspfUci, ByRef aOneSeg As Boolean)
        Try
            aUci.OpnSeqBlock.Delt = 60
            aUci.OpnSeqBlock.Uci = aUci
            If pReaches.Count > 1 Then  'reaches have to be in order, fix if needed
                Dim lOutOfOrder As Boolean = True
                Do Until Not lOutOfOrder
                    lOutOfOrder = False
                    Dim i As Integer = 1
                    Do Until i = pReaches.Count - 1
                        Dim lStringToFind As String = pReaches(pReaches(i).Order).DownID
                        Dim j As Integer = 1
                        Do Until j = pReaches.Count - 1
                            If pReaches(pReaches(j).Order).Id = lStringToFind And j < i Then
                                'reaches are out of order, swap places
                                Dim lOrder As Integer = pReaches(i).Order
                                pReaches(i).Order = pReaches(j).Order
                                pReaches(j).Order = lOrder
                                lOutOfOrder = True
                            Else
                                j += 1
                            End If
                        Loop
                        i += 1
                    Loop
                Loop
            End If

            'add rec to opn seq block for each land use
            pLandName(2) = "PERLND"
            pLandName(1) = "IMPLND"
            If aOneSeg Then 'only one segment for all land uses
                Call CreateOpnsForOneSeg(aUci)
            Else 'user wants multiple segments
                Call CreateOpnsForMultSegs(aUci)
            End If

            'add record to opn seq block for each reach
            For i As Integer = 0 To pReaches.Count - 1
                'now add each rchres to opn seq block
                Dim lOpn As New HspfOperation
                lOpn.Uci = aUci
                lOpn.Name = "RCHRES"
                If IsNumeric(pReaches(pReaches(i).Order).Id) And pReaches(pReaches(i).Order).Id.Length < 5 Then
                    lOpn.Description = pReaches(pReaches(i).Order).Name
                Else
                    lOpn.Description = pReaches(pReaches(i).Order).Name & " (" & pReaches(pReaches(i).Order).Id & ")"
                End If
                'newOpn.Id = i
                lOpn.Id = CInt(pReaches(pReaches(i).Order).Id)
                aUci.OpnSeqBlock.Add(lOpn)
            Next i
        Catch e As ApplicationException
            Logger.Msg(e.Message)
        End Try
    End Sub

    Private Sub CreateOpnsForOneSeg(ByRef aUci As HspfUci)
        Dim lUniqueNameCount As Integer
        Dim lAddflag As Boolean
        Dim lNewOperation As HspfOperation
        Dim lToperId As Integer

        For j As Integer = 2 To 1 Step -1
            lUniqueNameCount = 0
            For i As Integer = 0 To pLandUses.Count - 1
                If TypeId(pLandUses(i).Type) = j Then
                    If lUniqueNameCount = 0 Then
                        'add it
                        lNewOperation = New HspfOperation
                        lNewOperation.Uci = aUci
                        lNewOperation.Name = pLandName(j)
                        lToperId = 101
                        pFirstSeg(j) = lToperId
                        lNewOperation.Id = lToperId
                        pLastSeg(j) = lToperId
                        lNewOperation.Description = pLandUses(i).Description
                        lNewOperation.Tag = pLandUses(i).Reach
                        aUci.OpnSeqBlock.Add(lNewOperation)
                        lUniqueNameCount += 1
                    Else
                        lAddflag = True
                        For Each lOperation As HspfOperation In aUci.OpnSeqBlock.Opns
                            If lOperation.Description = pLandUses(i).Description And _
                               lOperation.Name = pLandName(j) Then
                                lAddflag = False
                                lToperId = lOperation.Id
                            End If
                        Next lOperation
                        If lAddflag Then
                            lUniqueNameCount += 1
                            lNewOperation = New HspfOperation
                            lNewOperation.Uci = aUci
                            lNewOperation.Name = pLandName(j)
                            lNewOperation.Description = pLandUses(i).Description
                            lToperId = 100 + lUniqueNameCount
                            lNewOperation.Id = lToperId
                            pLastSeg(j) = lToperId
                            lNewOperation.Tag = pLandUses(i).Reach
                            aUci.OpnSeqBlock.Add(lNewOperation)
                        End If
                    End If
                End If
            Next i
        Next j
    End Sub

    Private Function TypeId(ByVal aType As String) As Integer
        Dim lTypeId As Integer = 0
        Select Case aType
            Case "PERLND" : lTypeId = 2
            Case "IMPLND" : lTypeId = 1
        End Select
        Return lTypeId
    End Function

    Private Sub CreateOpnsForMultSegs(ByRef aUci As HspfUci)
        Dim j, i, k As Integer
        Dim lImplndNames As New atcCollection
        Dim lPerlndNames As New atcCollection

        'prescan to see how many perlnds and implnds per segment
        For i = 0 To pLandUses.Count - 1
            If pLandUses(i).Type = "PERLND" Then
                If lPerlndNames.IndexFromKey(pLandUses(i).Description) = 0 Then
                    lPerlndNames.Add(pLandUses(i).Description)
                End If
            ElseIf pLandUses(i).Type = "IMPLND" Then
                If lImplndNames.IndexFromKey(pLandUses(i).Description) = 0 Then
                    lImplndNames.Add(pLandUses(i).Description)
                End If
            End If
        Next i

        Dim lDigits As Integer = 0
        For i = 0 To pReaches.Count - 1
            If pReaches(i).Id.Length > lDigits Then
                lDigits = pReaches(i).Id.Length
            End If
        Next i

        Dim lBase As Integer
        If lDigits = 1 Or lDigits = 0 Then
            'use 101, 102, 201, 202 scheme
            lBase = 100
        ElseIf lDigits = 2 And lPerlndNames.Count < 10 And lImplndNames.Count < 10 Then
            'use 11, 12, 21, 22 scheme
            lBase = 10
        Else
            'too many to use the multiple seg scheme
            Logger.Msg("There are too many segments to use this segmentation scheme." & vbCrLf & "Create will use the 'Grouped' scheme instead", MsgBoxStyle.OkOnly, "Create Problem")
            Call CreateOpnsForOneSeg(aUci)
            lBase = 0
        End If

        If lBase > 0 Then
            'create these perlnd operations
            pFirstSeg(1) = 99999
            pLastSeg(1) = 0
            pFirstSeg(2) = 99999
            pLastSeg(2) = 0
            For k = 0 To pReaches.Count - 1
                'loop through each reach
                For i = 0 To pLandUses.Count - 1
                    'look to see if this landuse rec goes to this reach
                    If pLandUses(i).Reach = pReaches(k).Id Then
                        'it does
                        If pLandUses(i).Type = "PERLND" Then
                            'add this perlnd oper
                            Dim lOpn As New HspfOperation
                            lOpn.Uci = aUci
                            lOpn.Name = "PERLND"
                            Dim lLandUseId As Integer = 0
                            For j = 0 To lPerlndNames.Count - 1
                                If lPerlndNames(j - 1) = pLandUses(i).Description Then
                                    'this is the land use we want
                                    lLandUseId = j
                                End If
                            Next j
                            Dim lOperId As Integer = (CDbl(pLandUses(i).Reach) * lBase) + lLandUseId
                            If lOperId < pFirstSeg(2) Then
                                pFirstSeg(2) = lOperId
                            End If
                            If lOperId > pLastSeg(2) Then
                                pLastSeg(2) = lOperId
                            End If
                            lOpn.Id = lOperId
                            lOpn.Description = pLandUses(i).Description
                            lOpn.Tag = pLandUses(i).Reach
                            aUci.OpnSeqBlock.Add(lOpn)
                        End If
                    End If
                Next i

                'now add implnds
                For i = 0 To pLandUses.Count - 1
                    If pLandUses(i).Reach = pReaches(k).Id Then
                        If pLandUses(i).Type = "IMPLND" Then
                            'add this implnd oper
                            Dim lOpn As New HspfOperation
                            lOpn.Uci = aUci
                            lOpn.Name = "IMPLND"
                            Dim lLandUseId As Integer = 0
                            For j = 0 To lImplndNames.Count - 1
                                If lImplndNames(j - 1) = pLandUses(i).Description Then
                                    'this is the land use we want
                                    lLandUseId = j
                                End If
                            Next j
                            Dim lOperId As Integer = (CDbl(pLandUses(i).Reach) * lBase) + lLandUseId
                            If lOperId < pFirstSeg(1) Then
                                pFirstSeg(1) = lOperId
                            End If
                            If lOperId > pLastSeg(1) Then
                                pLastSeg(1) = lOperId
                            End If
                            lOpn.Id = lOperId
                            lOpn.Description = pLandUses(i).Description
                            lOpn.Tag = pLandUses(i).Reach
                            aUci.OpnSeqBlock.Add(lOpn)
                        End If
                    End If
                Next i
            Next k
        End If

    End Sub

    Public Function WDMInd(ByRef wdmid As String) As Integer
        Dim w As String

        If Len(wdmid) > 3 Then
            w = Mid(wdmid, 4, 1)
            If w = " " Then w = "1"
        Else
            w = "1"
        End If
        WDMInd = CInt(w)
    End Function

    Private Sub CreatePointSourceDSNs(ByRef aUci As HspfUci, _
                                      ByRef aPollutantListFileName As String)
        

        Dim lMasterPollutantList As New Collection
        If aPollutantListFileName.Length > 0 Then
            lMasterPollutantList = ReadPollutantList(aPollutantListFileName)
        Else
            lMasterPollutantList = Nothing
        End If

        Dim lNewWdmId As String = ""
        Dim lNewDsn As Integer
        Dim lJDates(1) As Single
        Dim lRLoad(1) As Single
        Dim lScen As String = "PT-OBS"
        For Each lFacility As Facility In pPointLoads
            For Each lPollutant As PollutantLoad In lFacility.PollutantLoads
                Dim lCon As String = GetPollutantIDFromName(lMasterPollutantList, lPollutant.PollutantName)
                If lCon.Length = 0 Then
                    lCon = UCase(Mid(lPollutant.PollutantName, 1, 8))
                End If
                Dim lStanam As String = lFacility.FacilityName
                Dim lLocation As String = "RCH" & lFacility.FacilityReach
                Dim lTstype As String = UCase(Mid(lPollutant.PollutantName, 1, 4))
                lRLoad(1) = lPollutant.PollutantLoad
                aUci.AddPointSourceDataSet(lScen, lLocation, lCon, lStanam, lTstype, 0, lJDates, lRLoad, lNewWdmId, lNewDsn)
            Next
        Next
        
    End Sub

    Private Function GetPollutantIDFromName(ByRef PollutantList As Collection, ByRef PollutantName As String) As String
        Dim lPollutantID As String = "x"

        If PollutantList Is Nothing Then
            lPollutantID = ""
        Else
            Dim i As Integer = 1
            Do While lPollutantID = "x"
                If Trim(Mid(PollutantList.Item(i), 14)) = PollutantName.Trim Then
                    lPollutantID = Mid(PollutantList.Item(i), 1, 5)
                End If
                i += 1
                If i > PollutantList.Count Then
                    lPollutantID = ""
                End If
            Loop
        End If
        Return lPollutantID
    End Function

    Private Sub CreateDefaultOutput(ByRef aUci As HspfUci)
        Dim lOutletId As Integer = 0
        For Each lConnection As HspfConnection In aUci.Connections
            If lConnection.Typ = 3 Then 'schematic record
                If lConnection.Source.VolName = "RCHRES" And _
                   lConnection.Target.VolName = "RCHRES" Then
                    lOutletId = lConnection.Target.VolId
                End If
            End If
        Next lConnection

        If lOutletId > 0 Then 'found watershed outlet
            Dim lWdmId, lNewDsn As Integer
            aUci.AddOutputWDMDataSet("RCH" & lOutletId, "FLOW", 100, lWdmId, lNewDsn)
            aUci.AddExtTarget("RCHRES", lOutletId, "HYDR", "RO", 1, 1, 1.0#, "AVER", "WDM" & CStr(lWdmId), lNewDsn, "FLOW", 1, "ENGL", "AGGR", "REPL")
        End If
    End Sub

    Private Function DefaultLSURFromSLSUR(ByRef aSlopeSurface As Double) As Double
        Dim lLengthSurface As Double
        If aSlopeSurface < 0.005 Then
            lLengthSurface = 500
        ElseIf aSlopeSurface < 0.01 Then
            lLengthSurface = 400
        ElseIf aSlopeSurface < 0.03 Then
            lLengthSurface = 350
        ElseIf aSlopeSurface < 0.07 Then
            lLengthSurface = 300
        ElseIf aSlopeSurface < 0.1 Then
            lLengthSurface = 250
        ElseIf aSlopeSurface < 0.15 Then
            lLengthSurface = 200
        Else
            lLengthSurface = 150
        End If
        Return lLengthSurface
    End Function

    Private Sub CreateBinaryOutput(ByRef aUci As HspfUci, ByRef aScenario As String)
        'add file name to files block
        Dim newFile As New HspfData.HspfFile
        newFile.Name = aScenario & ".hbn"
        newFile.Typ = "BINO"
        newFile.Unit = 92
        aUci.FilesBlock.Add(newFile)

        'update bin output units
        Dim lOperation As HspfOperation
        For Each lOperation In aUci.OpnBlks.Item("PERLND").Ids
            lOperation.Tables.Item("GEN-INFO").ParmValue("BUNIT1") = 92
        Next lOperation
        For Each lOperation In aUci.OpnBlks.Item("IMPLND").Ids
            lOperation.Tables.Item("GEN-INFO").ParmValue("BUNIT1") = 92
        Next lOperation
        For Each lOperation In aUci.OpnBlks.Item("RCHRES").Ids
            lOperation.Tables.Item("GEN-INFO").ParmValue("BUNITE") = 92
        Next lOperation
        'add binary-info tables
        aUci.OpnBlks.Item("PERLND").AddTableForAll("BINARY-INFO", "PERLND")
        aUci.OpnBlks.Item("IMPLND").AddTableForAll("BINARY-INFO", "IMPLND")
        aUci.OpnBlks.Item("RCHRES").AddTableForAll("BINARY-INFO", "RCHRES")
    End Sub

    Private Sub SetDefault(ByVal aUci As HspfUci, ByVal aDefaultUci As HspfUci)
        Dim lOpTypNames() As String = {"PERLND", "IMPLND", "RCHRES"}
        For Each lOpTypName As String In lOpTypNames
            If aUci.OpnBlks(lOpTypName).Count > 0 Then
                Dim lOpTyp As HspfOpnBlk = aUci.OpnBlks(lOpTypName)
                'Logger.Dbg lOpTyp.Name
                For Each lOperation As HspfOperation In lOpTyp.Ids
                    'Logger.Dbg lOpn.Description
                    Dim lOperationDefault As HspfOperation = MatchOperWithDefault(lOperation.Name, lOperation.Description, aDefaultUci)
                    If Not lOperationDefault Is Nothing Then
                        Logger.Dbg("Match " & lOperation.Id & ":" & lOperationDefault.Id & " " & lOperation.Description & ":" & lOperationDefault.Description)
                        For Each lTable As HspfTable In lOperation.Tables
                            If DefaultThisTable(lOpTyp.Name, lTable.Name) Then
                                If lOperationDefault.TableExists(lTable.Name) Then
                                    Dim lTableDefault As HspfTable = lOperationDefault.Tables(lTable.Name)
                                    'Logger.Dbg lTab.Name
                                    For Each lParm As HspfParm In lTable.Parms
                                        If DefaultThisParameter(lOpTyp.Name, lTable.Name, lParm.Name) Then
                                            If lParm.Value <> lParm.Name Then
                                                lParm.Value = lTableDefault.Parms(lParm.Name).Value
                                            End If
                                        End If
                                    Next lParm
                                End If
                            End If
                        Next lTable
                    End If
                Next lOperation
            End If
        Next lOpTypName
    End Sub

    Private Function DefaultThisTable(ByVal aOperationName As String, ByVal aTableName As String) As Boolean
        Dim lDefaultThisTable As Boolean
        If aOperationName = "PERLND" Or aOperationName = "IMPLND" Then
            If aTableName = "ACTIVITY" Or _
               aTableName = "PRINT-INFO" Or _
               aTableName = "GEN-INFO" Or _
               aTableName = "PWAT-PARM5" Then
                lDefaultThisTable = False
            ElseIf aTableName.StartsWith("QUAL") Then
                lDefaultThisTable = False
            Else
                lDefaultThisTable = True
            End If
        ElseIf aOperationName = "RCHRES" Then
            If aTableName = "ACTIVITY" Or _
               aTableName = "PRINT-INFO" Or _
               aTableName = "GEN-INFO" Or _
               aTableName = "HYDR-PARM1" Then
                lDefaultThisTable = False
            ElseIf aTableName.StartsWith("GQ-") Then
                lDefaultThisTable = False
            Else
                lDefaultThisTable = True
            End If
        Else
            lDefaultThisTable = False
        End If
        Return lDefaultThisTable
    End Function

    Private Function DefaultThisParameter(ByVal aOperationName As String, _
                                          ByVal aTableName As String, _
                                          ByVal aParmName As String) As Boolean
        Dim lDefaultThisParameter As Boolean = True
        If aOperationName = "PERLND" Then
            If aTableName = "PWAT-PARM2" Then
                If aParmName = "SLSUR" Or aParmName = "LSUR" Then
                    lDefaultThisParameter = False
                End If
            ElseIf aTableName = "NQUALS" Then
                If aParmName = "NQUAL" Then
                    lDefaultThisParameter = False
                End If
            End If
        ElseIf aOperationName = "IMPLND" Then
            If aTableName = "IWAT-PARM2" Then
                If aParmName = "SLSUR" Or aParmName = "LSUR" Then
                    lDefaultThisParameter = False
                End If
            ElseIf aTableName = "NQUALS" Then
                If aParmName = "NQUAL" Then
                    lDefaultThisParameter = False
                End If
            End If
        ElseIf aOperationName = "RCHRES" Then
            If aTableName = "HYDR-PARM2" Then
                If aParmName = "LEN" Or _
                   aParmName = "DELTH" Or _
                   aParmName = "FTBUCI" Then
                    lDefaultThisParameter = False
                End If
            ElseIf aTableName = "GQ-GENDATA" Then
                If aParmName = "NGQUAL" Then
                    lDefaultThisParameter = False
                End If
            End If
        End If
        Return lDefaultThisParameter
    End Function

    Private Function MatchOperWithDefault(ByVal aOpTypName As String, _
                                          ByVal aDescriptionDefault As String, _
                                          ByVal aUciDefault As HspfUci) _
                                          As HspfOperation
        Dim lOperationMatch As HspfOperation = Nothing

        For Each lOperation As HspfOperation In aUciDefault.OpnBlks(aOpTypName).Ids
            If lOperation.Description = aDescriptionDefault Then
                lOperationMatch = lOperation
                Exit For
            End If
        Next lOperation

        If lOperationMatch Is Nothing Then
            'a complete match not found, look for partial
            For Each lOperation As HspfOperation In aUciDefault.OpnBlks(aOpTypName).Ids
                If lOperation.Description.StartsWith(aDescriptionDefault) Then
                    lOperationMatch = lOperation
                    Exit For
                ElseIf aDescriptionDefault.StartsWith(lOperation.Description) Then
                    lOperationMatch = lOperation
                    Exit For
                ElseIf lOperation.Description.StartsWith(aDescriptionDefault.Substring(0, 4)) Then
                    lOperationMatch = lOperation
                    Exit For
                End If
            Next lOperation
        End If

        If lOperationMatch Is Nothing Then
            'not found, use first one if avaluable
            If aUciDefault.OpnBlks(aOpTypName).Count > 0 Then
                lOperationMatch = aUciDefault.OpnBlks(aOpTypName).Ids(0)
            End If
        End If
        Return lOperationMatch
    End Function

    Private Sub SetDefaultMassLink(ByVal aUci As HspfUci, ByVal aDefUci As HspfUci)
        Dim lMassLink As HspfMassLink

        For Each lMassLinkExist As HspfMassLink In aDefUci.MassLinks
            With lMassLinkExist.Source
                If .VolName = "PERLND" And _
                   .Member = "PERO" Then
                ElseIf .VolName = "IMPLND" And _
                       .Member = "SURO" Then
                ElseIf .VolName = "RCHRES" And _
                       .Group = "ROFLOW" Then
                Else 'add the other ones
                    lMassLink = New HspfMassLink
                    lMassLink = lMassLinkExist
                    aUci.MassLinks.Add(lMassLink)
                End If
            End With
        Next lMassLinkExist

    End Sub

    Private Sub DefaultBASINSMetseg(ByVal aUci As HspfUci, _
                                    ByVal aMetBaseDsn As Integer, _
                                    ByVal aMetWdmId As String)

        Dim lMetSeg As New HspfMetSeg
        lMetSeg.Uci = aUci
        For lRecordIndex As Integer = 1 To 7
            lMetSeg.MetSegRec(lRecordIndex).Source.VolName = aMetWdmId
            lMetSeg.MetSegRec(lRecordIndex).Sgapstrg = ""
            lMetSeg.MetSegRec(lRecordIndex).Ssystem = "ENGL"
            lMetSeg.MetSegRec(lRecordIndex).Tran = "SAME"
            lMetSeg.MetSegRec(lRecordIndex).typ = lRecordIndex
            Select Case lRecordIndex
                Case 1
                    lMetSeg.MetSegRec(lRecordIndex).Source.VolId = aMetBaseDsn
                    lMetSeg.MetSegRec(lRecordIndex).Source.Member = "PREC"
                    lMetSeg.MetSegRec(lRecordIndex).MFactP = 1
                    lMetSeg.MetSegRec(lRecordIndex).MFactR = 1
                    lMetSeg.MetSegRec(lRecordIndex).Sgapstrg = "ZERO"
                Case 2
                    lMetSeg.MetSegRec(lRecordIndex).Source.VolId = aMetBaseDsn + 2
                    lMetSeg.MetSegRec(lRecordIndex).Source.Member = "ATEM"
                    lMetSeg.MetSegRec(lRecordIndex).MFactP = 1
                    lMetSeg.MetSegRec(lRecordIndex).MFactR = 1
                Case 3
                    lMetSeg.MetSegRec(lRecordIndex).Source.VolId = aMetBaseDsn + 6
                    lMetSeg.MetSegRec(lRecordIndex).Source.Member = "DEWP"
                    lMetSeg.MetSegRec(lRecordIndex).MFactP = 1
                    lMetSeg.MetSegRec(lRecordIndex).MFactR = 1
                Case 4
                    lMetSeg.MetSegRec(lRecordIndex).Source.VolId = aMetBaseDsn + 3
                    lMetSeg.MetSegRec(lRecordIndex).Source.Member = "WIND"
                    lMetSeg.MetSegRec(lRecordIndex).MFactP = 1
                    lMetSeg.MetSegRec(lRecordIndex).MFactR = 1
                Case 5
                    lMetSeg.MetSegRec(lRecordIndex).Source.VolId = aMetBaseDsn + 4
                    lMetSeg.MetSegRec(lRecordIndex).Source.Member = "SOLR"
                    lMetSeg.MetSegRec(lRecordIndex).MFactP = 1
                    lMetSeg.MetSegRec(lRecordIndex).MFactR = 1
                Case 6
                    lMetSeg.MetSegRec(lRecordIndex).Source.VolId = aMetBaseDsn + 7
                    lMetSeg.MetSegRec(lRecordIndex).Source.Member = "CLOU"
                    lMetSeg.MetSegRec(lRecordIndex).MFactP = 0
                    lMetSeg.MetSegRec(lRecordIndex).MFactR = 1
                Case 7
                    lMetSeg.MetSegRec(lRecordIndex).Source.VolId = aMetBaseDsn + 5
                    lMetSeg.MetSegRec(lRecordIndex).Source.Member = "PEVT"
                    lMetSeg.MetSegRec(lRecordIndex).MFactP = 1
                    lMetSeg.MetSegRec(lRecordIndex).MFactR = 1
            End Select
        Next lRecordIndex
        lMetSeg.ExpandMetSegName(aMetWdmId, aMetBaseDsn)
        lMetSeg.Id = aUci.MetSegs.Count + 1
        aUci.MetSegs.Add(lMetSeg)

    End Sub

    Private Function ReadPollutantList(ByVal aFileName As String) As Collection

        Dim lPollutantList As New Collection

        On Error GoTo ErrHandler
        Dim lFileUnit As Integer = FreeFile()
        FileOpen(lFileUnit, aFileName, OpenMode.Input)
        On Error Resume Next
        Dim pstring As String = LineInput(lFileUnit)
        Dim lIndex As Integer = 0
        Do Until EOF(lFileUnit)
            pstring = LineInput(lFileUnit)
            lIndex = lIndex + 1
            lPollutantList.Add(Trim(pstring), CStr(lIndex))
        Loop
        FileClose(lFileUnit)
        Return lPollutantList
        Exit Function

ErrHandler:
        Logger.Dbg("Unable to Open Pollutant List File (" & aFileName & ")", "Create Problem")
        Return Nothing

    End Function
End Module