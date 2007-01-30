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

        Dim lAreaTable As New atcTableDelimited
        Dim lUcibase As String
        If lAreaTable.OpenFile(pDataDir & "land.csv") Then

            'get land use names from header of file
            Dim lLandUseNames As New Collection
            Dim lName As String
            For lIndex As Integer = 2 To lAreaTable.NumFields
                lname = lAreaTable.Value(lIndex)
                If lname.Length > 0 Then
                    lLandUseNames.Add(lname)
                End If
            Next

            'loop thru each subbasin
            lAreaTable.CurrentRecord = 1
            Do Until lAreaTable.atEOF
                lAreaTable.MoveNext()
                lUcibase = lAreaTable.Value(1)
                If Not FileExists(pOutputDir & lUcibase, True, False) Then
                    'create this folder
                    MkDir(pOutputDir & lUcibase)
                End If
                Dim lUciname As String = pOutputDir & lUcibase & "\" & lUcibase & ".uci"
                If Not FileExists(lUciname, False, True) Then
                    'create this uci
                    FileCopy(pOutputDir & "base.uci", lUciname)
                End If
                Dim lWdmname As String = pOutputDir & lUcibase & "\" & lUcibase & ".wdm"
                If Not FileExists(lWdmname, False, True) Then
                    'copy blank wdm
                    FileCopy(pOutputDir & "base.wdm", lWdmname)
                End If

                'open each uci for mods
                Dim lUci As New atcUCI.HspfUci
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
                                        lConn.MFact = lAreaTable.Value(lIndex + 1)
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
