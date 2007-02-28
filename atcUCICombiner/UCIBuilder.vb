Imports MapWinUtility
Imports atcUtility
Imports atcData
Imports atcWDM
Imports System.Collections.Specialized

Public Module UCIBuilder
    Private pBaseDrive As String = "c:\"
    Private pBaseDir As String = pBaseDrive & "SFBay\"
    Private pOutputDir As String = pBaseDir & "UCIs\"
    Private pDataDir As String = pBaseDir & "datafiles\"

    Public Sub Main()
        Logger.StartToFile(pOutputDir & "uciBuilder.log")

        ChDriveDir(pOutputDir)
        Logger.Dbg("WorkingDirectory " & CurDir())

        Dim lMsg As New atcUCI.HspfMsg
        lMsg.Open("hspfmsg.mdb")

        Dim lStreamTable As New atcTableDelimited
        Dim lStreambase As String
        Dim lStreamLen As Single
        Dim lStreamDeltah As Single
        Dim lStreamName As String
        Dim lStreamDepth As Single
        Dim lStreamWidth As Single
        Dim lStreamSlope As Single
        If Not lStreamTable.OpenFile(pDataDir & "streams.csv") Then
            Logger.Dbg("Could not open streams.csv")
        End If

        Dim lMetTable As New atcTableDelimited
        If Not lMetTable.OpenFile(pDataDir & "met4model.csv") Then
            Logger.Dbg("Could not open met4model.csv")
        End If

        Dim lSoilClassTable As New atcTableDelimited
        If Not lSoilClassTable.OpenFile(pDataDir & "soilclass.csv") Then
            Logger.Dbg("Could not open soilclass.csv")
        End If

        Dim lSlopeClassTable As New atcTableDelimited
        If Not lSlopeClassTable.OpenFile(pDataDir & "meanslopes.csv") Then
            Logger.Dbg("Could not open meanslopes.csv")
        End If

        Dim lParmTable As New atcTableDelimited
        Dim lParmValue(12, 83) As String
        If Not lParmTable.OpenFile(pDataDir & "parametervalues.csv") Then
            Logger.Dbg("Could not open parametervalues.csv")
        Else
            'read parm table into memory for later use
            Dim lrow As Integer = 0
            lParmTable.CurrentRecord = 1
            Do Until lParmTable.atEOF
                lParmTable.MoveNext()
                lrow = lrow + 1
                For icol As Integer = 1 To 12
                    lParmValue(icol, lrow) = lParmTable.Value(icol + 1)
                Next icol
            Loop
        End If

        Dim lAreaTable As New atcTableDelimited
        Dim lUcibase As String
        Dim lProjectbase As String
        Dim lProjectNames As New Collection
        Dim lUCINames As New Collection
        Dim lOper As atcUCI.HspfOperation
        Dim lconn As atcUCI.HspfConnection

        If lAreaTable.OpenFile(pDataDir & "land.csv") Then

            'get land use names from header of file
            Dim lLandUseNames As New Collection
            Dim lName As String
            For lIndex As Integer = 3 To lAreaTable.NumFields
                lName = lAreaTable.Value(lIndex)
                If lName.Length > 0 Then
                    lLandUseNames.Add(lName)
                End If
            Next

            'loop thru each subbasin
            lAreaTable.CurrentRecord = 1
            Do Until lAreaTable.atEOF
                lAreaTable.MoveNext()
                lProjectbase = lAreaTable.Value(1)
                If Not lProjectNames.Contains(lProjectbase) Then
                    lProjectNames.Add(lProjectbase, lProjectbase)
                End If
                lUcibase = lAreaTable.Value(2)
                lUCINames.Add(lUcibase, lUcibase)
                If Not FileExists(pOutputDir & lProjectbase, True, False) Then
                    'create this folder
                    MkDir(pOutputDir & lProjectbase)
                End If
                Dim lUciname As String = pOutputDir & lProjectbase & "\" & lUcibase & ".uci"
                If FileExists(lUciname, False, True) Then
                    Kill(lUciname)
                End If
                If Not FileExists(lUciname, False, True) Then
                    'create this uci
                    FileCopy(pOutputDir & "base.uci", lUciname)
                End If
                Dim lWdmname As String = pOutputDir & lProjectbase & "\" & lProjectbase & ".wdm"
                If FileExists(lWdmname, False, True) Then
                    Kill(lWdmname)
                End If
                If Not FileExists(lWdmname, False, True) Then
                    'copy blank wdm
                    FileCopy(pOutputDir & "base.wdm", lWdmname)
                End If

                'open each uci for mods
                Dim lUci As New atcUCI.HspfUci
                lUci.FastReadUciForStarter(lMsg, lUciname)

                'change base name throughout
                lUci.GlobalBlock.RunInf.Value = "UCI Created for ABAG for " & lProjectbase
                For lIndex As Integer = 1 To lUci.FilesBlock.Count
                    Dim lHspfFile As atcUCI.HspfData.HspfFile = lUci.FilesBlock.Value(lIndex)
                    If lHspfFile.Name = "base.wdm" Then
                        lHspfFile.Name = lProjectbase & ".wdm"
                    End If
                    If lHspfFile.Name = "base.out" Then
                        lHspfFile.Name = lProjectbase & ".out"
                    End If
                    If lHspfFile.Name = "base.ech" Then
                        lHspfFile.Name = lProjectbase & ".ech"
                    End If
                    If lHspfFile.Name = "base.hbn" Then
                        lHspfFile.Name = lProjectbase & ".hbn"
                    End If
                    lUci.FilesBlock.Value(lIndex) = lHspfFile
                Next

                'look thru streams table looking for a match
                lOper = lUci.OpnBlks("RCHRES").ids(1)
                lStreamTable.CurrentRecord = 1
                Do Until lStreamTable.atEOF
                    lStreamTable.MoveNext()
                    lStreambase = lStreamTable.Value(1)
                    If lStreambase = lUcibase Then
                        'found the match, update some parms
                        lStreamLen = SignificantDigits(lStreamTable.Value(4) / 1610, 4)
                        lOper.Tables("HYDR-PARM2").parmvalue("LEN") = lStreamLen
                        lStreamDeltah = SignificantDigits((lStreamTable.Value(12) - lStreamTable.Value(11)) * 3.281 / 100, 4)
                        lOper.Tables("HYDR-PARM2").parmvalue("DELTH") = lStreamDeltah
                        lStreamName = lStreamTable.Value(14)
                        lOper.Tables("GEN-INFO").parmvalue("RCHID") = lStreamName
                        'update the ftable
                        lStreamDepth = lStreamTable.Value(10) * 3.28
                        lStreamWidth = lStreamTable.Value(9) * 3.28
                        lStreamSlope = lStreamTable.Value(13) / 10000 'to convert to m/m
                        lOper.FTable.FTableFromCrossSect(lStreamLen * 5280, lStreamDepth, lStreamWidth, 0.05, lStreamSlope, 1.0, 1.0, lStreamDepth * 1.25, 0.5, 0.5, lStreamDepth * 1.875, lStreamDepth * 62.5, 0.5, 0.5, lStreamWidth, lStreamWidth)
                        lStreamTable.MoveLast()
                    End If
                Loop

                'update areas in schematic block
                Dim i As Integer
                Dim lOperType As String
                Dim lLandOper As atcUCI.HspfOperation
                For i = 1 To 2
                    If i = 1 Then
                        lOperType = "PERLND"
                    Else
                        lOperType = "IMPLND"
                    End If
                    For lIndex As Integer = 1 To lLandUseNames.Count
                        lName = lLandUseNames(lIndex)
                        For Each lLandOper In lUci.OpnBlks(lOperType).ids
                            If lName = lLandOper.Tables("GEN-INFO").parms("LSID").value Then
                                'this is a land use match
                                For Each lconn In lLandOper.Targets
                                    If lconn.Target.VolName = "RCHRES" Then
                                        'assume this is the one
                                        lconn.MFact = lAreaTable.Value(lIndex + 2)
                                    End If
                                Next
                            End If
                        Next
                    Next
                Next i

                'look thru met table looking for a match
                lMetTable.CurrentRecord = 1
                Dim lmetbase As String = ""
                Do Until lMetTable.atEOF
                    lMetTable.MoveNext()
                    lMetbase = lMetTable.Value(2)
                    If lMetbase = lUcibase Then
                        'found the match, update met data
                        Dim lEtMfact As Double = lMetTable.Value(3)
                        Dim lPrecMfact As Double = lMetTable.Value(4)
                        Dim lPrecDsn As Integer = lMetTable.Value(7)
                        Dim lMetSeg As atcUCI.HspfMetSeg = lUci.MetSegs(1)

                        lMetSeg.MetSegRecs(1).Source.VolId = lPrecDsn
                        lMetSeg.MetSegRecs(1).MFactP = lPrecMfact
                        lMetSeg.MetSegRecs(1).MFactR = lPrecMfact
                        lMetSeg.MetSegRecs(7).MFactP = lEtMfact
                        lMetSeg.MetSegRecs(8).MFactR = lEtMfact

                        If lProjectbase = "PENINSUL" Then
                            'special exception, 2 precip datasets
                            lMetSeg.MetSegRecs(1).MFactP = lPrecMfact * 0.5
                            lMetSeg.MetSegRecs(1).MFactR = lPrecMfact * 0.5
                            'add some ext src records
                            lUci.MetSeg2Source()
                            For Each lOper In lUci.OpnSeqBlock.Opns
                                lconn = New atcUCI.HspfConnection
                                lconn.Uci = lUci
                                lconn.Typ = 1
                                lconn.Source.VolName = "WDM2"
                                lconn.Source.VolId = 116
                                lconn.Source.Member = "HPCP"
                                lconn.Ssystem = "ENGL"
                                lconn.Sgapstrg = "ZERO"
                                lconn.MFact = lPrecMfact * 0.5
                                lconn.Tran = "SAME"
                                lconn.Target.VolName = lOper.Name
                                lconn.Target.VolId = lOper.Id
                                lconn.Target.Group = "EXTNL"
                                lconn.Target.Member = "PREC"
                                lconn.Target.Opn = lOper
                                lOper.Sources.Add(lconn)
                                lUci.Connections.Add(lconn)
                            Next
                            lUci.Source2MetSeg()
                        End If
                        lMetTable.MoveLast()
                    End If
                Loop

                'look thru soil class table looking for a match
                lSoilClassTable.CurrentRecord = 1
                Dim lsoilbase As String = ""
                Dim lSoilGroup As String = ""
                Do Until lSoilClassTable.atEOF
                    lSoilClassTable.MoveNext()
                    lsoilbase = lSoilClassTable.Value(4)
                    If lsoilbase = lUcibase Then
                        'found the match, store soil group
                        lSoilGroup = lSoilClassTable.Value(3)
                        lSoilClassTable.MoveLast()
                    End If
                Loop

                'get slope from slope table 
                lSlopeClassTable.CurrentRecord = 1
                Dim lSlopeBase As String = ""
                Dim lAgSlope As Single = 0
                Dim lDevSlope As Single = 0
                Dim lForSlope As Single = 0
                Dim lGrassSlope As Single = 0
                Dim lShrubSlope As Single = 0
                Do Until lSlopeClassTable.atEOF
                    lSlopeClassTable.MoveNext()
                    lSlopeBase = lSlopeClassTable.Value(1)
                    If lSlopeBase = lUcibase Then
                        'found the match, store slopes by land use
                        lAgSlope = lSlopeClassTable.Value(3)
                        lDevSlope = lSlopeClassTable.Value(4)
                        lForSlope = lSlopeClassTable.Value(5)
                        lGrassSlope = lSlopeClassTable.Value(6)
                        lShrubSlope = lSlopeClassTable.Value(7)
                        lSlopeClassTable.MoveLast()
                    End If
                Loop

                'begin loop thru each operation to set parameters
                Dim lSlope As Single = 0
                Dim lSlopeClass As Integer = 1
                Dim lParmLine As Integer = 1
                For Each lOper In lUci.OpnSeqBlock.Opns
                    If lOper.Name = "PERLND" Or lOper.Name = "IMPLND" Then
                        If lOper.Name = "PERLND" And lOper.Id = 101 Then
                            'forest
                            lSlope = lForSlope
                            lParmLine = 1
                        ElseIf lOper.Name = "PERLND" And lOper.Id = 102 Then
                            'shrub
                            lSlope = lShrubSlope
                            lParmLine = 2
                        ElseIf lOper.Name = "PERLND" And lOper.Id = 103 Then
                            'grass
                            lSlope = lGrassSlope
                            lParmLine = 3
                        ElseIf lOper.Name = "PERLND" And lOper.Id = 104 Then
                            'dev
                            lSlope = lDevSlope
                            lParmLine = 4
                        ElseIf lOper.Name = "PERLND" And lOper.Id = 105 Then
                            'ag
                            lSlope = lAgSlope
                            lParmLine = 3
                        ElseIf lOper.Name = "IMPLND" Then
                            lSlope = lDevSlope
                            lParmLine = 4
                        End If
                        'assign slope class
                        If lSlope < 5 Then
                            lSlopeClass = 1
                        ElseIf lSlope < 10 Then
                            lSlopeClass = 2
                        ElseIf lSlope < 20 Then
                            lSlopeClass = 3
                        Else
                            lSlopeClass = 4
                        End If

                        'update parameters based on parm table
                        Dim lParmColumn As Integer
                        If lSoilGroup = "A" Then
                            lParmColumn = lSlopeClass
                        ElseIf lSoilGroup = "B" Then
                            lParmColumn = 4 + lSlopeClass
                        Else
                            lParmColumn = 8 + lSlopeClass
                        End If
                        If lOper.Name = "PERLND" Then
                            lOper.Tables("PWAT-PARM2").parmvalue("LZSN") = lParmValue(lParmColumn, lParmLine + 1)
                            lOper.Tables("PWAT-PARM2").parmvalue("INFILT") = lParmValue(lParmColumn, lParmLine + 8)
                            lOper.Tables("PWAT-PARM2").parmvalue("LSUR") = lParmValue(lParmColumn, lParmLine + 15)
                            lOper.Tables("PWAT-PARM2").parmvalue("SLSUR") = lParmValue(lParmColumn, lParmLine + 22)
                            lOper.Tables("PWAT-PARM2").parmvalue("KVARY") = lParmValue(lParmColumn, lParmLine + 29)
                            lOper.Tables("PWAT-PARM2").parmvalue("AGWRC") = lParmValue(lParmColumn, lParmLine + 36)
                            lOper.Tables("PWAT-PARM3").parmvalue("BASETP") = lParmValue(lParmColumn, lParmLine + 43)
                            lOper.Tables("PWAT-PARM4").parmvalue("UZSN") = lParmValue(lParmColumn, lParmLine + 50)
                            lOper.Tables("PWAT-PARM4").parmvalue("NSUR") = lParmValue(lParmColumn, lParmLine + 57)
                            lOper.Tables("PWAT-PARM4").parmvalue("INTFW") = lParmValue(lParmColumn, lParmLine + 64)
                            lOper.Tables("PWAT-PARM4").parmvalue("IRC") = lParmValue(lParmColumn, lParmLine + 71)
                            lOper.Tables("PWAT-PARM3").parmvalue("DEEPFR") = lParmValue(lParmColumn, lParmLine + 78)
                        Else
                            lOper.Tables("IWAT-PARM2").parmvalue("LSUR") = lParmValue(lParmColumn, lParmLine + 15)
                            lOper.Tables("IWAT-PARM2").parmvalue("SLSUR") = lParmValue(lParmColumn, lParmLine + 22)
                            lOper.Tables("IWAT-PARM2").parmvalue("NSUR") = lParmValue(lParmColumn, lParmLine + 57)
                        End If

                    End If

                Next lOper

                'save each uci
                lUci.Save()

            Loop

            'read reach connections table
            Dim lConnTable As New atcTableDelimited
            If Not lConnTable.OpenFile(pDataDir & "ReachConnections.csv") Then
                Logger.Dbg("Could not open ReachConnections.csv")
            End If
            Dim lConnSources As New Collection
            Dim lConnTargets As New Collection
            lConnTable.CurrentRecord = 1
            Do Until lConnTable.atEOF
                lConnTable.MoveNext()
                lConnSources.Add(lConnTable.Value(1))
                lConnTargets.Add(lConnTable.Value(2))
            Loop

            'combine the ucis within each project
            For Each lProjectbase In lProjectNames
                'see how many ucis within this projectname
                Dim lUCIsInProject As New Collection
                For Each lUcibase In lUCINames
                    If Mid(lUcibase, 1, lUcibase.Length - 1) = lProjectbase Then
                        lUCIsInProject.Add(lUcibase, lUcibase)
                    End If
                Next
                If lUCIsInProject.Count > 0 Then
                    'combine each of these together into 1 uci
                    Dim lUciname As String = pOutputDir & lProjectbase & "\" & lProjectbase & ".uci"
                    'create this uci
                    FileCopy(pOutputDir & lProjectbase & "\" & lUCIsInProject(1) & ".uci", lUciname)
                    Kill(pOutputDir & lProjectbase & "\" & lUCIsInProject(1) & ".uci")
                    'open new combined uci
                    Dim lCombinedUci As New atcUCI.HspfUci
                    lCombinedUci.FastReadUciForStarter(lMsg, lUciname)
                    lCombinedUci.MetSeg2Source()
                    lCombinedUci.Point2Source()

                    'now start looping through the rest of the ucis
                    For lUciIndex As Integer = 2 To lUCIsInProject.Count
                        'read each uci 
                        Dim lUci As New atcUCI.HspfUci
                        lUci.FastReadUciForStarter(lMsg, pOutputDir & lProjectbase & "\" & lUCIsInProject(lUciIndex) & ".uci")
                        lUci.MetSeg2Source()
                        lUci.Point2Source()

                        'add operations of second uci into first uci
                        Dim lNewOperId As Integer
                        For Each lOper In lUci.OpnSeqBlock.Opns
                            If lOper.Name = "PERLND" Or lOper.Name = "IMPLND" Then
                                lNewOperId = ((lUciIndex - 1) * 100) + lOper.Id
                            Else
                                lNewOperId = 1
                            End If
                            'make sure this is a unique number
                            Do While Not lCombinedUci.OpnBlks(lOper.Name).operfromid(lNewOperId) Is Nothing
                                lNewOperId = lNewOperId + 1
                            Loop

                            'add this operation
                            Dim lOpn As New atcUCI.HspfOperation
                            Dim lOrigId As Integer
                            lOpn = lOper
                            lOpn.Name = lOper.Name
                            lOrigId = lOper.Id
                            lOpn.Id = lNewOperId
                            lOpn.Uci = lCombinedUci
                            lCombinedUci.OpnBlks(lOper.Name).Ids.Add(lOpn, "K" & lOpn.Id)
                            lOpn.OpnBlk = lCombinedUci.OpnBlks(lOper.Name)
                            lCombinedUci.OpnSeqBlock.Add(lOper)

                            'remove the comments so we don't get repeated headers
                            For Each lTable As atcUCI.HspfTable In lOpn.Tables
                                lTable.Comment = ""
                            Next lTable

                            If lOper.Name = "RCHRES" Then
                                'update ftable number
                                lOper.FTable.Id = lNewOperId
                                lOper.Tables("HYDR-PARM2").parmvalue("FTBUCI") = lNewOperId
                                'do we need to add a connection to this reach?
                                For i As Integer = 1 To lConnTargets.Count
                                    If lConnTargets(i) = lUCIsInProject(lUciIndex) Then
                                        For j As Integer = 1 To lUCIsInProject.Count
                                            If lConnSources(i) = lUCIsInProject(j) Then
                                                'add a connection from RCHRES j to RCHRES lUciIndex
                                                lconn = New atcUCI.HspfConnection
                                                lconn.Uci = lCombinedUci
                                                lconn.Typ = 3
                                                lconn.Source.VolName = "RCHRES"
                                                lconn.Source.VolId = j
                                                lconn.MFact = 1.0#
                                                lconn.Target.VolName = "RCHRES"
                                                lconn.Target.VolId = lUciIndex
                                                lconn.MassLink = 3
                                                lCombinedUci.Connections.Add(lconn)
                                                lCombinedUci.OpnBlks("RCHRES").operfromid(j).targets.add(lconn)
                                                lOper.Sources.Add(lconn)
                                            End If
                                        Next j
                                    End If
                                Next
                            End If

                            'reset the connection operation numbers
                            For Each lconn In lOper.Targets
                                If lconn.Source.VolName = lOper.Name Then
                                    lconn.Source.VolId = lNewOperId
                                End If
                            Next lconn
                            For Each lconn In lOper.Sources
                                If lconn.Target.VolName = lOper.Name Then
                                    lconn.Target.VolId = lNewOperId
                                End If
                            Next lconn

                        Next lOper

                        lUci = Nothing
                        Logger.Dbg("Added " & lUCIsInProject(lUciIndex))

                        'can delete uci now
                        Kill(pOutputDir & lProjectbase & "\" & lUCIsInProject(lUciIndex) & ".uci")

                    Next lUciIndex

                    lCombinedUci.Source2MetSeg()
                    lCombinedUci.Save()
                End If
            Next

            'add bay connector reaches to some projects
            Dim lBayTable As New atcTableDelimited
            If Not lBayTable.OpenFile(pDataDir & "BayConnectors.csv") Then
                Logger.Dbg("Could not open BayConnectors.csv")
            End If
            lBayTable.CurrentRecord = 1
            Do Until lBayTable.atEOF
                lBayTable.MoveNext()
                Dim lUciname As String = pOutputDir & lBayTable.Value(1) & "\" & lBayTable.Value(1) & ".uci"
                Dim lUci As New atcUCI.HspfUci
                lUci.FastReadUciForStarter(lMsg, lUciname)
                lUci.MetSeg2Source()

                Dim lNewOperId As Integer = 1
                'make sure this is a unique number
                Do While Not lUci.OpnBlks("RCHRES").operfromid(lNewOperId) Is Nothing
                    lNewOperId = lNewOperId + 1
                Loop
                'open a copy of this operation
                Dim lCopyUci As New atcUCI.HspfUci
                lCopyUci.FastReadUciForStarter(lMsg, lUciname)
                Dim lCopyOpn As atcUCI.HspfOperation = lCopyUci.OpnBlks("RCHRES").operfromid(lNewOperId - 1)
                'add this operation to this uci
                Dim lOpn As New atcUCI.HspfOperation
                lOpn = lCopyOpn
                lOpn.Id = lNewOperId
                lOpn.Uci = lUci
                lUci.OpnBlks("RCHRES").Ids.Add(lOpn, "K" & lOpn.Id)
                lOpn.OpnBlk = lUci.OpnBlks("RCHRES")
                lUci.OpnSeqBlock.Add(lCopyOpn)
                'remove the comments so we don't get repeated headers
                For Each lTable As atcUCI.HspfTable In lOpn.Tables
                    lTable.Comment = ""
                Next lTable

                'update ftable number
                lOpn.FTable.Id = lNewOperId
                lOpn.Tables("HYDR-PARM2").parmvalue("FTBUCI") = lNewOperId
                'update some parms
                lStreamLen = SignificantDigits(lBayTable.Value(2) / 1610, 4)
                lOpn.Tables("HYDR-PARM2").parmvalue("LEN") = lStreamLen
                lStreamDeltah = SignificantDigits((lBayTable.Value(6) - lBayTable.Value(5)) * 3.281 / 100, 4)
                lOpn.Tables("HYDR-PARM2").parmvalue("DELTH") = lStreamDeltah
                lStreamName = lBayTable.Value(7)
                lOpn.Tables("GEN-INFO").parmvalue("RCHID") = lStreamName
                'update the ftable
                lStreamDepth = lBayTable.Value(4) * 3.28
                lStreamWidth = lBayTable.Value(3) * 3.28
                lStreamSlope = lStreamDeltah / lStreamLen
                lOpn.FTable.FTableFromCrossSect(lStreamLen * 5280, lStreamDepth, lStreamWidth, 0.05, lStreamSlope, 1.0, 1.0, lStreamDepth * 1.25, 0.5, 0.5, lStreamDepth * 1.875, lStreamDepth * 62.5, 0.5, 0.5, lStreamWidth, lStreamWidth)

                'remove all connections to this reach
                Do While lOpn.Sources.Count > 0
                    lOpn.Sources.Remove(1)
                Loop
                ''add a connection from RCHRES lNewOperId-1 to RCHRES lNewOperId
                lconn = New atcUCI.HspfConnection
                lconn.Uci = lUci
                lconn.Typ = 3
                lconn.Source.VolName = "RCHRES"
                lconn.Source.VolId = lNewOperId - 1
                lconn.MFact = 1.0#
                lconn.Target.VolName = "RCHRES"
                lconn.Target.VolId = lNewOperId
                lconn.MassLink = 3
                lconn.Source.Opn = lUci.OpnBlks("RCHRES").operfromid(lNewOperId - 1)
                lconn.Target.Opn = lOpn
                lUci.Connections.Add(lconn)
                lUci.OpnBlks("RCHRES").operfromid(lNewOperId - 1).targets.add(lconn)
                lOpn.Sources.Add(lconn)

                lUci.Source2MetSeg()
                lUci.Save()
            Loop

            'connect upstream/downstream projects
            For Each lProjectbase In lProjectNames
                For i As Integer = 1 To lConnSources.Count
                    If lConnSources(i) = lProjectbase Then
                        'add output from this project to connect to downstream project
                        Dim lUciname As String = pOutputDir & lProjectbase & "\" & lProjectbase & ".uci"
                        Dim lWdmname As String = pOutputDir & lProjectbase & "\" & lProjectbase & ".wdm"
                        Dim lUci As New atcUCI.HspfUci
                        lUci.FastReadUciForStarter(lMsg, lUciname)
                        lUci.MetSeg2Source()
                        MakeMods_AddExtTarLink(lUci, lWdmname)
                        lUci.Source2MetSeg()
                        lUci.Save()
                    End If
                Next
                For i As Integer = 1 To lConnTargets.Count
                    If lConnTargets(i) = lProjectbase Then
                        'add upstream inflow to this project 
                        Dim lUciname As String = pOutputDir & lProjectbase & "\" & lProjectbase & ".uci"
                        Dim lUci As New atcUCI.HspfUci
                        lUci.FastReadUciForStarter(lMsg, lUciname)
                        lUci.MetSeg2Source()
                        MakeMods_AddExtSrcLink(lUci)
                        'add ref to upstream wdm 
                        Dim lHspfFile As New atcUCI.HspfData.HspfFile
                        lHspfFile.Name = lConnSources(i) & ".wdm"
                        lHspfFile.Typ = "WDM3"
                        lHspfFile.Unit = 27
                        lUci.FilesBlock.Add(lHspfFile)
                        lUci.Source2MetSeg()
                        lUci.Save()
                    End If
                Next
            Next

        End If
    End Sub

    Private Sub MakeMods_AddExtSrcLink(ByVal aUci As atcUCI.HspfUci)
        Dim tstype$(8), member$(8), memsub1&(8), memsub2&(8)
        Dim lOper As atcUCI.HspfOperation
        Dim lConn As atcUCI.HspfConnection

        tstype(1) = "VOL" : member(1) = "IVOL" : memsub1(1) = 1 : memsub2(1) = 1
        tstype(2) = "SED1" : member(2) = "ISED" : memsub1(2) = 1 : memsub2(2) = 1
        tstype(3) = "SED2" : member(3) = "ISED" : memsub1(3) = 2 : memsub2(3) = 1
        tstype(4) = "SED3" : member(4) = "ISED" : memsub1(4) = 3 : memsub2(4) = 1
        tstype(5) = "DQAL1" : member(5) = "IDQAL" : memsub1(5) = 1 : memsub2(5) = 1
        tstype(6) = "SQAL11" : member(6) = "ISQAL" : memsub1(6) = 1 : memsub2(6) = 1
        tstype(7) = "SQAL21" : member(7) = "ISQAL" : memsub1(7) = 2 : memsub2(7) = 1
        tstype(8) = "SQAL31" : member(8) = "ISQAL" : memsub1(8) = 3 : memsub2(8) = 1

        Dim lIndex As Integer = aUci.OpnBlks("RCHRES").Ids.count
        lOper = aUci.OpnBlks("RCHRES").Ids(lIndex)
        'add whole set like this for upstream inflows
        For i As Integer = 1 To 8
            lConn = New atcUCI.HspfConnection
            If i = 1 Then
                lConn.Comment = "*** Upstream Inflows"
            End If
            lConn.Uci = aUci
            lConn.Typ = 1
            lConn.Source.VolName = "WDM3"
            lConn.Source.VolId = 100 + i
            lConn.Source.Member = tstype(i)
            lConn.Ssystem = "ENGL"
            lConn.Sgapstrg = ""
            lConn.MFact = 1
            lConn.Tran = "SAME"
            lConn.Target.VolName = lOper.Name
            lConn.Target.VolId = lOper.Id
            lConn.Target.Group = "INFLOW"
            lConn.Target.Member = member(i)
            lConn.Target.MemSub1 = memsub1(i)
            lConn.Target.MemSub2 = memsub2(i)
            lConn.Target.Opn = lOper
            lOper.Sources.Add(lConn)
            aUci.Connections.Add(lConn)
        Next

    End Sub

    Private Sub MakeMods_AddExtTarLink(ByVal aUci As atcUCI.HspfUci, ByVal aWdmname As String)
        Dim lOper As atcUCI.HspfOperation
        Dim tstype$(8), member$(8), memsub1&(8), memsub2&(8)

        Dim lWdm As New atcWDM.atcDataSourceWDM
        lWdm.Open(aWdmname)

        tstype(1) = "VOL" : member(1) = "ROVOL" : memsub1(1) = 1 : memsub2(1) = 1
        tstype(2) = "SED1" : member(2) = "ROSED" : memsub1(2) = 1 : memsub2(2) = 1
        tstype(3) = "SED2" : member(3) = "ROSED" : memsub1(3) = 2 : memsub2(3) = 1
        tstype(4) = "SED3" : member(4) = "ROSED" : memsub1(4) = 3 : memsub2(4) = 1
        tstype(5) = "DQAL1" : member(5) = "RODQAL" : memsub1(5) = 1 : memsub2(5) = 1
        tstype(6) = "SQAL11" : member(6) = "ROSQAL" : memsub1(6) = 1 : memsub2(6) = 1
        tstype(7) = "SQAL21" : member(7) = "ROSQAL" : memsub1(7) = 2 : memsub2(7) = 1
        tstype(8) = "SQAL31" : member(8) = "ROSQAL" : memsub1(8) = 3 : memsub2(8) = 1

        Dim lOpnBlk As atcUCI.HspfOpnBlk = aUci.OpnBlks("RCHRES")
        Dim lIndex As Integer = lOpnBlk.Ids.Count
        lOper = lOpnBlk.Ids(lIndex)
        'add whole set like this for the bottom-most reach
        Dim lnewdsn As Integer = 0
        For i As Integer = 1 To 8

            'add wdm data set
            Dim GenTs As atcData.atcTimeseries
            GenTs = New atcData.atcTimeseries(lWdm)
            With GenTs.Attributes
                .SetValue("ID", 100 + i)
                .SetValue("ts", 1)
                .SetValue("tu", 3)
                .SetValue("Scenario", UCase(FilenameOnly(aUci.Name)))
                .SetValue("Constituent", UCase(tstype(i)))
                .SetValue("Location", UCase("R" & CStr(lOper.Id)))
                .SetValue("Description", lOper.Description)
            End With
            GenTs.Attributes.SetValue("TSTYPE", GenTs.Attributes.GetValue("Constituent"))
            Dim TsDate As atcData.atcTimeseries
            TsDate = New atcData.atcTimeseries(Nothing)
            GenTs.Dates = TsDate
            lWdm.AddDataset(GenTs, 0)

            'add external targets record
            aUci.AddExtTarget("RCHRES", lOper.Id, "ROFLOW", member(i), memsub1(i), _
              memsub2(i), 1.0, "SAME", "WDM1", _
              100 + i, tstype(i), 1, "ENGL", "AGGR", "REPL")
        Next i

        lWdm.Save(aWdmname)

    End Sub
End Module
