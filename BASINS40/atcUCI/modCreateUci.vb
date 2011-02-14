'Copyright 2006 AQUA TERRA Consultants - Royalty-free use permitted under open source license
Option Strict Off
Option Explicit On

Imports System.Collections.ObjectModel
Imports MapWinUtility
Imports atcUtility
Imports atcData
Imports atcSegmentation

Module modCreateUci
    Dim pWatershed As Watershed
    Dim pLandName(2) As String
    Dim pLastSeg(2) As Integer
    Dim pFirstSeg(2) As Integer

    Friend Sub CreateUciFromBASINS(ByRef aWatershed As Watershed, _
                                   ByRef aUci As HspfUci, _
                                   ByRef aDataSources As Collection(Of atcTimeseriesSource), _
                                   ByRef aStarterUci As HspfUci, _
                                   Optional ByRef aPollutantListFileName As String = "", _
                                   Optional ByRef aMetBaseDsn As Integer = 11, _
                                   Optional ByRef aMetWdmId As String = "WDM2")
        pWatershed = aWatershed

        aUci.Name = aWatershed.Name & ".uci"
        'dummy global block
        With aUci.GlobalBlock  'add global block to empty uci
            .Comment = " "
            .Uci = aUci
            .RunInf.Value = "UCI Created by WinHSPF for " & aWatershed.Name
            .EmFg = 1
            .OutLev.Value = CStr(1)
            .RunFg = 1
        End With

        'add files block to uci
        CreateFilesBlock(aUci, aWatershed.Name, aDataSources)
        aUci.Save() 'TODO: required by prescanfilesblock, need in memory version of prescan...
        Logger.Dbg("UCIRecordCount " & ReadUCIRecords(aUci.Name))

        Dim lEchoFileName As String = ""
        Dim lFilesBlockStatus As Boolean = aUci.PreScanFilesBlock(lEchoFileName)

        If aWatershed.MetSegments Is Nothing OrElse aWatershed.MetSegments.Count = 0 Then
            'build initial met segment 
            aUci.MetSegs.Add(DefaultBASINSMetseg(aUci, aMetBaseDsn, aMetWdmId))
        Else
            CreateBASINSMetsegs(aUci)
        End If

        aUci.Initialized = True

        'set start and end dates in global block
        Dim lSJDate As Double = 0
        Dim lEJDate As Double = 0
        GetStartEndDatesFromMetsegs(aUci, _
                                    lSJDate, lEJDate)
        Dim lStartDate(6) As Integer
        Dim lEndDate(6) As Integer
        J2Date(lSJDate, lStartDate)
        J2Date(lEJDate, lEndDate)
        With aUci.GlobalBlock  'update start and end date from met data
            For lDateIndex As Integer = 0 To 5
                .SDate(lDateIndex) = lStartDate(lDateIndex)
                .EDate(lDateIndex) = lEndDate(lDateIndex)
            Next lDateIndex
            .EDate(3) = 24
        End With

        'add opn seq block
        CreateOpnSeqBlock(aUci)

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
                lOpnBlk.CreateTables(aUci.Msg.BlockDefs.Item(lOpnBlk.Name))
            End If
        Next

        For Each lChannel As Channel In pWatershed.Channels 'process ftables
            Dim lOperation As HspfOperation = aUci.OpnBlks.Item("RCHRES").OperFromID(CShort(lChannel.Reach.Id))
            If Not lOperation Is Nothing Then
                lOperation.FTable.FTableFromCrossSect(lChannel)
            End If
        Next

        'create schematic, ext src blocks
        CreateConnectionsSchematic(aUci)
        CreateConnectionsMet(aUci)

        'set timeser connections
        For Each lOperation As HspfOperation In aUci.OpnSeqBlock.Opns
            lOperation.SetTimSerConnections()
        Next
        'create masslinks
        CreateMassLinks(aUci)

        'set initial values in uci from BASINS values
        SetInitValues(aUci)

        CreatePointSourceDSNs(aUci, aPollutantListFileName)

        CreateDefaultOutput(aUci)
        CreateBinaryOutput(aUci, aWatershed.Name)

        'set default parameter values and mass links from starter
        aUci.SetDefault(aStarterUci)
        SetDefaultMassLink(aUci, aStarterUci)

        aUci.Edited = False 'all the reads set edited
    End Sub

    Private Sub SetInitValues(ByVal aUci As HspfUci)
        'set init values in uci

        For Each lOperation As HspfOperation In aUci.OpnBlks.Item("PERLND").Ids
            For Each lLandUse As LandUse In pWatershed.LandUses
                If lOperation.Id = lLandUse.ModelID Then  'found a match
                    If lLandUse.Type = "PERLND" Or lLandUse.Type = "COMPOSITE" Then
                        Dim lTable As HspfTable = lOperation.Tables.Item("ACTIVITY")
                        lTable.Parms("PWATFG").Value = 1
                        lTable = lOperation.Tables.Item("GEN-INFO")
                        lTable.Parms("LSID").Value = lLandUse.Description
                        lTable = lOperation.Tables.Item("PWAT-PARM2")
                        lTable.Parms("SLSUR").Value = SignificantDigits(CompositeSlope(lLandUse.Reach.SegmentId, pWatershed.LandUses), 3)
                        If lLandUse.Slope <= 0 Then
                            lTable.Parms("SLSUR").Value = 0.001 'must have some slope
                        End If
                        lTable.Parms("LSUR").Value = DefaultLSURFromSLSUR(lTable.Parms("SLSUR").Value) 'default lsur based on slsur
                        Exit For
                    End If
                End If
            Next
        Next

        For Each lOperation As HspfOperation In aUci.OpnBlks.Item("IMPLND").Ids
            For Each lLandUse As LandUse In pWatershed.LandUses
                If lOperation.Id = lLandUse.ModelID Then  'found a match
                    If lLandUse.Type = "IMPLND" Or lLandUse.Type = "COMPOSITE" Then
                        Dim lTable As HspfTable = lOperation.Tables.Item("ACTIVITY")
                        lTable.Parms("IWATFG").Value = 1
                        lTable = lOperation.Tables.Item("GEN-INFO")
                        lTable.Parms("LSID").Value = lLandUse.Description
                        lTable = lOperation.Tables.Item("IWAT-PARM2")
                        lTable.Parms("SLSUR").Value = SignificantDigits(CompositeSlope(lLandUse.Reach.SegmentId, pWatershed.LandUses), 3)
                        If lLandUse.Slope <= 0 Then
                            lTable.Parms("SLSUR").Value = 0.001 'must have some slope
                        End If
                        lTable.Parms("LSUR").Value = DefaultLSURFromSLSUR(lTable.Parms("SLSUR").Value) 'default lsur based on slsur
                        Exit For
                    End If
                End If
            Next
        Next

        Dim lReachIndex As Integer = -1
        For Each lOperation As HspfOperation In aUci.OpnBlks.Item("RCHRES").Ids
            lReachIndex += 1
            Dim lTable As HspfTable = lOperation.Tables.Item("ACTIVITY")
            lTable.Parms("HYDRFG").Value = 1
            lTable = lOperation.Tables.Item("GEN-INFO")
            Dim lStr As String = pWatershed.Reaches(pWatershed.Reaches(lReachIndex).Order).Name
            Dim lLen As Integer = lStr.Length
            If lLen < 19 And _
              (Not IsNumeric(pWatershed.Reaches(pWatershed.Reaches(lReachIndex).Order).Id) Or _
               pWatershed.Reaches(pWatershed.Reaches(lReachIndex).Order).Id.Length > 5) Then
                lStr &= " " & Right(pWatershed.Reaches(pWatershed.Reaches(lReachIndex).Order).Id, 19 - lLen)
            End If
            lTable.Parms("RCHID").Value = lStr
            lTable.Parms("NEXITS").Value = pWatershed.Reaches(pWatershed.Reaches(lReachIndex).Order).NExits
            lTable.Parms("PUNITE").Value = 91
            If pWatershed.Reaches(pWatershed.Reaches(lReachIndex).Order).Type = "R" Then
                lTable.Parms("LKFG").Value = 1
            End If
            lTable = lOperation.Tables.Item("HYDR-PARM1")
            lTable.Parms("AUX1FG").Value = 1
            lTable.Parms("AUX2FG").Value = 1
            lTable.Parms("AUX3FG").Value = 1
            lTable.Parms("ODFVF1").Value = 4
            lTable = lOperation.Tables.Item("HYDR-PARM2")
            lTable.Parms("LEN").Value = pWatershed.Reaches(pWatershed.Reaches(lReachIndex).Order).Length
            lTable.Parms("DELTH").Value = System.Math.Round(pWatershed.Reaches(pWatershed.Reaches(lReachIndex).Order).DeltH, 0)
            'set initial volume in reach in ac-ft to 75% of length in miles * mean width in feet * mean depth in feet
            lTable = lOperation.Tables.Item("HYDR-INIT")
            lTable.Parms("VOL").Value = CInt(pWatershed.Reaches(pWatershed.Reaches(lReachIndex).Order).Length * 5280 * _
                                             pWatershed.Reaches(pWatershed.Reaches(lReachIndex).Order).Depth * _
                                             pWatershed.Reaches(pWatershed.Reaches(lReachIndex).Order).Width / 43560 * 0.75)
        Next lOperation
    End Sub

    Private Function CompositeSlope(ByVal aSegmentId As Integer, ByVal aLandUses As LandUses) As Single
        'look through all landuses of this model segment, compute composite slope 
        Dim lCompositeSlope As Single = 0.0

        Dim lTotalArea As Single = 0.0
        Dim lTotalSlopeTimesArea As Single = 0.0

        For Each lLanduse As LandUse In aLandUses
            If lLanduse.Reach.SegmentId = aSegmentId Then
                lTotalArea = lTotalArea + lLanduse.Area
                lTotalSlopeTimesArea = lTotalSlopeTimesArea + (lLanduse.Area * lLanduse.Slope)
            End If
        Next

        If lTotalArea > 0 Then
            lCompositeSlope = lTotalSlopeTimesArea / lTotalArea
        End If

        Return lCompositeSlope
    End Function

    Private Sub CreateMassLinks(ByRef aUci As HspfUci)
        Dim lMassLink As HspfMassLink
        For Each lOpnBlk As HspfOpnBlk In aUci.OpnBlks
            lMassLink = New HspfMassLink
            lMassLink.Uci = aUci
            If lOpnBlk.Name = "PERLND" Then
                lMassLink.MassLinkId = 2
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
            ElseIf lOpnBlk.Name = "IMPLND" Then
                lMassLink.MassLinkId = 1
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
            ElseIf lOpnBlk.Name = "RCHRES" Then
                lMassLink.MassLinkId = 3
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
        Next lOpnBlk
    End Sub

    Private Sub CreateConnectionsSchematic(ByRef aUci As HspfUci)
        Dim lConnection As HspfConnection

        For Each lLandUse As LandUse In pWatershed.LandUses
            Dim lTypes As New atcCollection
            Select Case lLandUse.Type
                Case "PERLND"
                    lTypes.Add("PERLND")
                Case "IMPLND"
                    lTypes.Add("IMPLND")
                Case "COMPOSITE"
                    lTypes.Add("PERLND")
                    lTypes.Add("IMPLND")
            End Select
            For Each lType As String In lTypes
                lConnection = New HspfConnection
                lConnection.Uci = aUci
                lConnection.Typ = 3
                lConnection.Source.VolName = lType
                lConnection.Source.VolId = lLandUse.ModelID
                If lType = "PERLND" Then
                    lConnection.MFact = lLandUse.Area * (1.0 - lLandUse.ImperviousFraction)
                Else 'IMPLND
                    lConnection.MFact = lLandUse.Area * lLandUse.ImperviousFraction
                End If
                lConnection.Target.VolName = "RCHRES"
                lConnection.Target.VolId = lLandUse.Reach.Id
                lConnection.MassLink = TypeId(lType)
                aUci.Connections.Add(lConnection)
            Next
        Next

        For Each lReachUpstream As Reach In pWatershed.Reaches
            'add entries for each reach to reach connection
            If lReachUpstream.DownID > 0 And pWatershed.Reaches.Contains(lReachUpstream.DownID) Then
                Dim lReachDownstream As Reach = pWatershed.Reaches(lReachUpstream.DownID)
                lConnection = New HspfConnection
                lConnection.Uci = aUci
                lConnection.Typ = 3
                lConnection.Source.VolName = "RCHRES"
                lConnection.Source.VolId = lReachUpstream.Id
                lConnection.MFact = 1.0#
                lConnection.Target.VolName = "RCHRES"
                lConnection.Target.VolId = lReachDownstream.Id
                lConnection.MassLink = 3
                aUci.Connections.Add(lConnection)
            End If
        Next
    End Sub

    Private Sub CreateConnectionsMet(ByRef aUci As HspfUci)
        If aUci.MetSegs.Count > 0 Then
            Dim lOpTypes() As String = {"PERLND", "IMPLND", "RCHRES"} 'operations with assoc met segs
            For Each lOpTyp As String In lOpTypes
                For Each lOpn As HspfOperation In aUci.OpnBlks.Item(lOpTyp).Ids
                    If aUci.MetSegs.Count = 1 Then
                        lOpn.MetSeg = aUci.MetSegs.Item(0)
                    Else
                        'figure out which met seg to use
                        Dim lOpnSet As Boolean = False
                        If lOpTyp = "RCHRES" Then
                            For Each lReach As Reach In pWatershed.Reaches
                                If lReach.Id = lOpn.Id Then
                                    For Each lMetSeg As HspfMetSeg In aUci.MetSegs
                                        If lMetSeg.Id = lReach.SegmentId Then
                                            lOpn.MetSeg = lMetSeg
                                            lOpnSet = True
                                            Exit For
                                        End If
                                    Next
                                    Exit For
                                End If
                            Next
                        Else  'PERLND or IMPLND (or COMPOSITE)
                            For Each lLanduse As LandUse In pWatershed.LandUses
                                If (lLanduse.Type = lOpTyp Or lLanduse.Type = "COMPOSITE") And lLanduse.ModelID = lOpn.Id Then
                                    For Each lMetSeg As HspfMetSeg In aUci.MetSegs
                                        If lMetSeg.Id = lLanduse.Reach.SegmentId Then
                                            lOpn.MetSeg = lMetSeg
                                            lOpnSet = True
                                            Exit For
                                        End If
                                    Next
                                    Exit For
                                End If
                            Next
                        End If
                        If Not lOpnSet Then
                            'if you haven't been able to find a corresponding met seg, just use the first one
                            lOpn.MetSeg = aUci.MetSegs.Item(0)
                        End If
                    End If
                Next
            Next
        Else
            Logger.Msg("No Met Segments Available", "CreateProblem")
        End If
    End Sub

    Private Sub CreateFilesBlock(ByRef aUci As HspfUci, _
                                 ByRef aScenario As String, _
                                 ByRef aDataSources As Collection(Of atcData.atcTimeseriesSource))

        aUci.FilesBlock.Clear()
        aUci.FilesBlock.Uci = aUci

        Dim lFile As New HspfFile
        lFile.Comment = "<FILE>  <UN#>***<----FILE NAME------------------------------------------------->"
        lFile.Name = aScenario & ".ech"
        lFile.Typ = "MESSU"
        lFile.Unit = 24
        aUci.FilesBlock.Add(lFile)

        lFile = New HspfFile
        lFile.Comment = ""
        lFile.Name = aScenario & ".out"
        lFile.Typ = " "
        lFile.Unit = 91
        aUci.FilesBlock.Add(lFile)

        Dim lOutput As atcTimeseriesSource = aDataSources(0)
        If lOutput.Name.Length > 0 Then
            lFile = New HspfFile
            lFile.Name = RelativeFilename(lOutput.Specification, CurDir)
            lFile.Typ = lOutput.Name.Substring(lOutput.Name.LastIndexOf(":") + 1) & 1
            lFile.Unit = 25
            aUci.FilesBlock.Add(lFile)
        End If

        For lWdmIndex As Integer = 1 To aDataSources.Count - 1
            If Not aDataSources(lWdmIndex) Is Nothing AndAlso aDataSources(lWdmIndex).Name.Length > 0 Then
                lFile = New HspfFile
                lFile.Name = RelativeFilename(aDataSources(lWdmIndex).Specification, CurDir)
                With aDataSources(lWdmIndex)
                    lFile.Typ = .Name.Substring(.Name.LastIndexOf(":") + 1) & lWdmIndex + 1
                End With
                lFile.Unit = 25 + lWdmIndex
                aUci.FilesBlock.Add(lFile)
            End If
        Next lWdmIndex
    End Sub

    Private Sub CreateOpnSeqBlock(ByRef aUci As HspfUci)
        Try
            aUci.OpnSeqBlock.Delt = 60
            aUci.OpnSeqBlock.Uci = aUci
            If pWatershed.Reaches.Count > 1 Then  'reaches have to be in order, fix if needed
                Dim lOutOfOrder As Boolean = True
                Do Until Not lOutOfOrder
                    lOutOfOrder = False
                    Dim i As Integer = 1
                    Do Until i = pWatershed.Reaches.Count
                        Dim lStringToFind As String = pWatershed.Reaches(pWatershed.Reaches(i).Order).DownID
                        Dim j As Integer = 0
                        Do Until j = pWatershed.Reaches.Count - 1
                            If pWatershed.Reaches(pWatershed.Reaches(j).Order).Id = lStringToFind And j < i Then
                                'reaches are out of order, swap places
                                Dim lOrder As Integer = pWatershed.Reaches(i).Order
                                pWatershed.Reaches(i).Order = pWatershed.Reaches(j).Order
                                pWatershed.Reaches(j).Order = lOrder
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
            CreateOpns(aUci)

            'add record to opn seq block for each reach
            For i As Integer = 0 To pWatershed.Reaches.Count - 1
                'now add each rchres to opn seq block
                Dim lOpn As New HspfOperation
                lOpn.Uci = aUci
                lOpn.Name = "RCHRES"
                If IsNumeric(pWatershed.Reaches(pWatershed.Reaches(i).Order).Id) And _
                             pWatershed.Reaches(pWatershed.Reaches(i).Order).Id.Length < 5 Then
                    lOpn.Description = pWatershed.Reaches(pWatershed.Reaches(i).Order).Name
                Else
                    lOpn.Description = pWatershed.Reaches(pWatershed.Reaches(i).Order).Name & " (" & pWatershed.Reaches(pWatershed.Reaches(i).Order).Id & ")"
                End If
                'newOpn.Id = i
                lOpn.Id = CInt(pWatershed.Reaches(pWatershed.Reaches(i).Order).Id)
                aUci.OpnSeqBlock.Add(lOpn)
            Next i
        Catch e As ApplicationException
            Logger.Msg(e.Message)
        End Try
    End Sub

    Private Sub CreateOpns(ByRef aUci As HspfUci)

        'prescan to see how many perlnds and implnds per segment
        Dim lImplndNames As New atcCollection
        Dim lPerlndNames As New atcCollection
        For Each lLandUse As LandUse In pWatershed.LandUses
            If lLandUse.Type = "PERLND" Or lLandUse.Type = "COMPOSITE" Then
                If lPerlndNames.IndexFromKey(lLandUse.Description) = -1 Then
                    lPerlndNames.Add(lLandUse.Description)
                End If
            End If
            If lLandUse.Type = "IMPLND" Or lLandUse.Type = "COMPOSITE" Then
                If lImplndNames.IndexFromKey(lLandUse.Description) = -1 Then
                    lImplndNames.Add(lLandUse.Description)
                End If
            End If
        Next

        'figure out the number of segments desired
        Dim lSegmentIds As New atcCollection
        For Each lReach As Reach In pWatershed.Reaches
            If lSegmentIds.IndexFromKey(lReach.SegmentId) = -1 Then
                lSegmentIds.Add(lReach.SegmentId)
            End If
        Next

        pFirstSeg(1) = 99999
        pLastSeg(1) = 0
        pFirstSeg(2) = 99999
        pLastSeg(2) = 0
        Dim lBase As Integer
        If lSegmentIds.Count < 10 Then
            'use 101, 102, 201, 202 scheme
            lBase = 100
        ElseIf lSegmentIds.Count >= 10 And lPerlndNames.Count <= 10 And lImplndNames.Count <= 10 Then
            'use 11, 12, 21, 22 scheme
            lBase = 10
        Else
            'too many segments and land uses to use the multiple seg scheme
            Logger.Msg("There are too many segments to use this segmentation scheme." & vbCrLf & "Create will use the 'Grouped' scheme instead", MsgBoxStyle.OkOnly, "Create Problem")
            lSegmentIds.Clear()
            lSegmentIds.Add(1)
            For Each lReach As Reach In pWatershed.Reaches
                lReach.SegmentId = 1
            Next
            lBase = 100
        End If

        'create these operations for each segment
        For Each lSegmentId As Integer In lSegmentIds
            'loop through each segment
            CreateOpnsForOneSeg(aUci, lBase, lSegmentId, lPerlndNames, lImplndNames)
        Next

    End Sub

    Private Sub CreateOpnsForOneSeg(ByRef aUci As HspfUci, ByRef aBase As Integer, ByRef aModelSeg As Integer, _
                                    ByRef aPerlndNames As atcCollection, ByRef aImplndNames As atcCollection)

        Dim lAddflag As Boolean
        Dim lToperId As Integer
        For j As Integer = 2 To 1 Step -1 'loop through perlnds then implnds
            Dim lUniqueNameCount As Integer = 0
            For Each lLandUse As LandUse In pWatershed.LandUses 'loop through each landuse record
                Dim lTypeId As Integer = TypeId(lLandUse.Type)
                If lTypeId = j Or lTypeId = 3 Then
                    If lLandUse.Reach.SegmentId = aModelSeg Then 'is this landuse part of this model segment?
                        lAddflag = True
                        For Each lOperation As HspfOperation In aUci.OpnSeqBlock.Opns
                            If lOperation.Description = lLandUse.Description And _
                               lOperation.Name = pLandName(j) And _
                               lOperation.Id = lLandUse.ModelID Then
                                'have already added one of these for this model segment, so don't add again
                                lAddflag = False
                                lToperId = lOperation.Id
                            End If
                        Next lOperation
                        If lAddflag Then
                            lUniqueNameCount += 1
                            Dim lNewOperation As New HspfOperation
                            lNewOperation.Uci = aUci
                            lNewOperation.Name = pLandName(j)
                            lNewOperation.Description = lLandUse.Description
                            'TODO: check these calcs in light of composite types
                            If aPerlndNames.IndexFromKey(lLandUse.Description) > -1 Then
                                lToperId = (aBase * aModelSeg) + aPerlndNames.IndexFromKey(lLandUse.Description) + 1
                            ElseIf aImplndNames.IndexFromKey(lLandUse.Description) > -1 Then
                                lToperId = (aBase * aModelSeg) + aImplndNames.IndexFromKey(lLandUse.Description) + 1
                            Else
                                lToperId = (aBase * aModelSeg) + lUniqueNameCount
                            End If
                            lNewOperation.Id = lToperId
                            lLandUse.ModelID = lToperId
                            pLastSeg(j) = lToperId
                            aUci.OpnSeqBlock.Add(lNewOperation)
                            If lUniqueNameCount = 1 Then
                                pFirstSeg(j) = lToperId
                            End If
                            'set the model id for any other records of this type, segment, and description
                            For Each lLandUseCheck As LandUse In pWatershed.LandUses
                                If lLandUse.Type = lLandUseCheck.Type And _
                                   lLandUse.Reach.SegmentId = lLandUseCheck.Reach.SegmentId And _
                                   lLandUse.Description = lLandUseCheck.Description Then
                                    lLandUseCheck.ModelID = lToperId
                                End If
                            Next
                        End If
                    End If
                End If
            Next
        Next j
    End Sub

    Private Function TypeId(ByVal aType As String) As Integer
        Dim lTypeId As Integer = 0
        Select Case aType
            Case "PERLND"
                lTypeId = 2
            Case "IMPLND"
                lTypeId = 1
            Case "COMPOSITE"
                lTypeId = 3
        End Select
        Return lTypeId
    End Function

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

        Dim lPollutantListFileName As String = "poltnt_2.prn"
        If aPollutantListFileName.Length = 0 Then
            'location master pollutant list 
            Dim lBinLoc As String = PathNameOnly(System.Reflection.Assembly.GetEntryAssembly.Location)
            Dim lPollutantListPath As String = lBinLoc.Substring(0, lBinLoc.Length - 3) & "models\hspf\bin\" & lPollutantListFileName
            If Not FileExists(lPollutantListPath) Then
                Dim lBasinsFolder As String = My.Computer.Registry.GetValue("HKEY_LOCAL_MACHINE\SOFTWARE\AQUA TERRA Consultants\BASINS", "Base Directory", "C:\Basins")
                lPollutantListPath = lBasinsFolder & "\models\hspf\bin\" & lPollutantListFileName
                If Not FileExists(lPollutantListPath) Then
                    lPollutantListPath = FindFile("Please locate " & lPollutantListFileName, lPollutantListFileName)
                End If
            End If
            lPollutantListFileName = lPollutantListPath
        Else
            lPollutantListFileName = aPollutantListFileName
        End If

        Dim lMasterPollutantList As New Collection
        If aPollutantListFileName.Length > 0 Then
            lMasterPollutantList = ReadPollutantList(lPollutantListFileName)
        Else
            lMasterPollutantList = Nothing
        End If

        Dim lNewWdmId As Integer = 0
        Dim lNewDsn As Integer
        Dim lJDates(1) As Double
        Dim lRLoad(1) As Double
        Dim lScen As String = "PT-OBS"
        For Each lFacility As Facility In pWatershed.PointLoads
            For Each lPollutant As Pollutant In lFacility.Pollutants
                Dim lCon As String = GetPollutantIDFromName(lMasterPollutantList, lPollutant.Name)
                If lCon.Length = 0 Then
                    lCon = UCase(Mid(lPollutant.Name, 1, 8))
                End If
                Dim lStanam As String = lFacility.Name
                Dim lLocation As String = "RCH" & lFacility.Reach
                Dim lTstype As String = UCase(Mid(lPollutant.Name, 1, 4))
                lRLoad(1) = lPollutant.Load
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

        If lOutletId = 0 Then
            'may have only 1 rchres, just set outlet id to last rchres
            If aUci.OpnBlks.Contains("RCHRES") Then
                Dim lOpnBlk As HspfOpnBlk = aUci.OpnBlks.Item("RCHRES")
                If lOpnBlk.Count > 0 Then
                    lOutletId = lOpnBlk.Ids.Item(lOpnBlk.Ids.Count - 1).Id
                End If
            End If
        End If

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
        Dim lNewFile As New HspfFile
        lNewFile.Name = aScenario & ".hbn"
        lNewFile.Typ = "BINO"
        lNewFile.Unit = 92
        aUci.FilesBlock.Add(lNewFile)

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

    Private Function DefaultBASINSMetseg(ByVal aUci As HspfUci, _
                                         ByVal aMetBaseDsn As Integer, _
                                         ByVal aMetWdmId As String) As HspfMetSeg

        Dim lMetSeg As New HspfMetSeg
        lMetSeg.Uci = aUci
        lMetSeg.MetSegRecs.Add(CreateMetSeqRecord(aMetWdmId, aMetBaseDsn, "PREC", 1, 1, "ZERO"))
        lMetSeg.MetSegRecs.Add(CreateMetSeqRecord(aMetWdmId, aMetBaseDsn + 2, "ATEM", 1, 1, "ZERO"))
        lMetSeg.MetSegRecs.Add(CreateMetSeqRecord(aMetWdmId, aMetBaseDsn + 6, "DEWP", 1, 1, "ZERO"))
        lMetSeg.MetSegRecs.Add(CreateMetSeqRecord(aMetWdmId, aMetBaseDsn + 3, "WIND", 1, 1, "ZERO"))
        lMetSeg.MetSegRecs.Add(CreateMetSeqRecord(aMetWdmId, aMetBaseDsn + 4, "SOLR", 1, 1, "ZERO"))
        lMetSeg.MetSegRecs.Add(CreateMetSeqRecord(aMetWdmId, aMetBaseDsn + 7, "CLOU", 1, 1, "ZERO"))
        lMetSeg.MetSegRecs.Add(CreateMetSeqRecord(aMetWdmId, aMetBaseDsn + 5, "PEVT", 1, 1, "ZERO"))

        lMetSeg.ExpandMetSegName(aMetWdmId, aMetBaseDsn)
        lMetSeg.Id = aUci.MetSegs.Count + 1
        Return lMetSeg
    End Function

    Private Function CreateMetSeqRecord _
                         (ByVal aMetWdmId As String, ByVal aVolId As Integer, ByVal aMember As String, _
                          ByVal aMFactP As Integer, ByVal aMFactR As Integer, ByVal aSgapstrg As String) _
                          As HspfMetSegRecord
        Dim lMetSegRecord As New HspfMetSegRecord
        lMetSegRecord.Name = aMember
        lMetSegRecord.Source.VolName = aMetWdmId
        lMetSegRecord.Sgapstrg = ""
        lMetSegRecord.Ssystem = "ENGL"
        lMetSegRecord.Tran = "SAME"
        lMetSegRecord.Source.VolId = aVolId
        lMetSegRecord.Source.Member = aMember
        lMetSegRecord.MFactP = aMFactP
        lMetSegRecord.MFactR = aMFactR
        lMetSegRecord.Sgapstrg = aSgapstrg
        Return lMetSegRecord
    End Function

    Private Sub CreateBASINSMetsegs(ByVal aUci As HspfUci)

        For Each lMetSegment As MetSegment In pWatershed.MetSegments
            Dim lMetSeg As New HspfMetSeg
            lMetSeg.Uci = aUci
            For Each lDataType As DataType In lMetSegment.DataTypes
                Dim lMetSegRecord As New HspfMetSegRecord
                With lDataType
                    lMetSegRecord.Name = .Name
                    lMetSegRecord.Source.VolName = .WdmID
                    lMetSegRecord.Source.VolId = .Dsn
                    lMetSegRecord.Source.Member = .Name
                    lMetSegRecord.MFactP = .MFactPI
                    lMetSegRecord.MFactR = .MFactR
                    If .Name = "PREC" Then
                        lMetSegRecord.Sgapstrg = "ZERO"
                    Else
                        lMetSegRecord.Sgapstrg = ""
                    End If
                End With
                lMetSegRecord.Ssystem = "ENGL"
                lMetSegRecord.Tran = "SAME"
                'make sure this dsn exists
                If Not aUci.GetDataSetFromDsn(lMetSegRecord.Source.VolName.Substring(3), lMetSegRecord.Source.VolId) Is Nothing Then
                    lMetSeg.MetSegRecs.Add(lMetSegRecord)
                End If
            Next
            With lMetSegment.DataTypes("PREC")
                lMetSeg.ExpandMetSegName(.WdmID, .Dsn)
            End With
            lMetSeg.Id = lMetSegment.Id
            aUci.MetSegs.Add(lMetSeg)
        Next

        'now see if any we can fill in any missing metsegrecs with those from another metseg
        'for instance, no pet for this station -> use pet from another station
        For Each lMetSeg As HspfMetSeg In aUci.MetSegs
            For Each lMetSegRec As HspfMetSegRecord In lMetSeg.MetSegRecs
                For Each lMetSegToCheck As HspfMetSeg In aUci.MetSegs
                    Dim lFound As Boolean = False
                    For Each lMetSegRecToCheck As HspfMetSegRecord In lMetSegToCheck.MetSegRecs
                        If lMetSegRecToCheck.Name = lMetSegRec.Name Then
                            lFound = True
                        End If
                    Next
                    If Not lFound Then
                        lMetSegToCheck.MetSegRecs.Add(lMetSegRec)
                    End If
                Next
            Next
        Next

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

    Private Sub GetStartEndDatesFromMetsegs(ByVal aUci As HspfUci, _
                                            ByRef aSJDate As Double, ByRef aEJDate As Double)

        aSJDate = 0
        aEJDate = 0

        Dim lWdmId As String
        Dim lWdmIndex As Integer
        Dim lDsn As Integer
        For Each lMetSeg As HspfMetSeg In aUci.MetSegs
            'get start and end dates from prec and pevt datasets
            Dim lSJDate As Double = 0
            Dim lEJDate As Double = 0
            lWdmId = lMetSeg.MetSegRecs("PREC").Source.VolName
            lWdmIndex = lWdmId.Substring(3)
            lDsn = lMetSeg.MetSegRecs("PREC").Source.VolId
            Dim lDataSet As atcData.atcTimeseries = aUci.GetDataSetFromDsn(lWdmIndex, lDsn)
            If Not lDataSet Is Nothing Then
                lSJDate = lDataSet.Dates.Value(0)
                lEJDate = lDataSet.Dates.Value(lDataSet.numValues) - 1

                'also check dates of PEVT dataset
                If lMetSeg.MetSegRecs.Contains("PEVT") Then
                    lWdmId = lMetSeg.MetSegRecs("PEVT").Source.VolName
                    lWdmIndex = lWdmId.Substring(3)
                    lDsn = lMetSeg.MetSegRecs("PEVT").Source.VolId
                    lDataSet = aUci.GetDataSetFromDsn(lWdmIndex, lDsn)
                    If Not lDataSet Is Nothing Then
                        Dim lSJDate2 As Double = lDataSet.Dates.Value(0)
                        Dim lEJDate2 As Double = lDataSet.Dates.Value(lDataSet.numValues) - 1
                        If lSJDate2 > lSJDate Then
                            lSJDate = lSJDate2
                        End If
                        If lEJDate2 < lEJDate Then
                            lEJDate = lEJDate2
                        End If
                    End If
                End If
            End If

            If lSJDate > aSJDate Then
                aSJDate = lSJDate
            End If
            If lEJDate < aEJDate Or aEJDate < 1 Then
                aEJDate = lEJDate
            End If
        Next
    End Sub
End Module