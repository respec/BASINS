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
                lUcibase = lAreaTable.Value(2)
                If Not FileExists(pOutputDir & lProjectbase, True, False) Then
                    'create this folder
                    MkDir(pOutputDir & lProjectbase)
                End If
                Dim lUciname As String = pOutputDir & lProjectbase & "\" & lUcibase & ".uci"
                If Not FileExists(lUciname, False, True) Then
                    'create this uci
                    FileCopy(pOutputDir & "base.uci", lUciname)
                End If
                Dim lWdmname As String = pOutputDir & lProjectbase & "\" & lUcibase & ".wdm"
                If Not FileExists(lWdmname, False, True) Then
                    'copy blank wdm
                    FileCopy(pOutputDir & "base.wdm", lWdmname)
                End If

                'open each uci for mods
                Dim lUci As New atcUCI.HspfUci
                Dim lOper As atcUCI.HspfOperation
                lUci.FastReadUciForStarter(lMsg, lUciname)

                'change base name throughout
                lUci.GlobalBlock.RunInf.Value = "UCI Created for ABAG for " & lUcibase
                For lIndex As Integer = 1 To lUci.FilesBlock.Count
                    Dim lHspfFile As atcUCI.HspfData.HspfFile = lUci.FilesBlock.Value(lIndex)
                    If lHspfFile.Name = "base.wdm" Then
                        lHspfFile.Name = lUcibase & ".wdm"
                    End If
                    If lHspfFile.Name = "base.out" Then
                        lHspfFile.Name = lUcibase & ".out"
                    End If
                    If lHspfFile.Name = "base.ech" Then
                        lHspfFile.Name = lUcibase & ".ech"
                    End If
                    If lHspfFile.Name = "base.hbn" Then
                        lHspfFile.Name = lUcibase & ".hbn"
                    End If
                    lUci.FilesBlock.Value(lIndex) = lHspfFile
                Next

                'look thru streams table looking for a match
                lOper = lUci.OpnBlks("RCHRES").ids(1)
                lStreamTable.CurrentRecord = 1
                Do Until lStreamTable.atEOF
                    lStreamTable.MoveNext()
                    lStreambase = lStreamTable.Value(1)
                    If lstreambase = lUcibase Then
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
                                For Each lConn As atcUCI.HspfConnection In lLandOper.Targets
                                    If lConn.Target.VolName = "RCHRES" Then
                                        'assume this is the one
                                        lConn.MFact = lAreaTable.Value(lIndex + 2)
                                    End If
                                Next
                            End If
                        Next
                    Next
                Next i

                'save each uci
                lUci.Save()

            Loop

        End If
    End Sub
End Module
