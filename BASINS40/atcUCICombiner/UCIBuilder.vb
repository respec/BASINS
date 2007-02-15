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
                If Not FileExists(lUciname, False, True) Then
                    'create this uci
                    FileCopy(pOutputDir & "base.uci", lUciname)
                End If
                Dim lWdmname As String = pOutputDir & lProjectbase & "\" & lProjectbase & ".wdm"
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

                'save each uci
                lUci.Save()

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

        End If
    End Sub
End Module
