Imports MapWinUtility
Imports atcUtility
Imports atcData
Imports atcWDM
Imports System.Collections.Specialized

Public Module UCICombiner
    Private pBaseDrive As String = "d:\"
    Private pBaseDir As String = pBaseDrive & "cbp_working\"
    Private pWorkingDir As String = pBaseDir & "output\"  '= "subset\"
    Private pOutputDir As String = pWorkingDir & "combined\"
    Private pWatershed As String = "Monocacy"
    Private pModelParmDir As String = pBaseDir & "model\p512\pp\"
    Private pConnectionDir As String = pModelParmDir & "catalog\iovars\"
    Private pAreaTableDir As String = pModelParmDir & "data\land_use\"
    Private pLandUseYear As String = "2002"
    Private pScenario As String = "base"

    Public Sub UCMain()
        Logger.StartToFile(pOutputDir & "uciCombiner.log")

        ChDriveDir(pWorkingDir)
        Logger.Dbg("WorkingDirectory " & CurDir())

        Dim lMsg As New atcUCI.HspfMsg
        lMsg.Open("hspfmsg.mdb")

        'get names of all ucis in dir
        Dim lUcis As Collection = HSPFUciNames(pWorkingDir)
        Logger.Dbg("UCICount " & lUcis.Count)

        'create a new uci to be the combined uci
        Dim lCombinedUci As New atcUCI.HspfUci

        ''for testing uci save
        'lWorkingDir = "C:\cbp_working\output\CBP716Copy\"
        'ChDir(lWorkingDir)
        'lCombinedUci.FastReadUciForStarter(lMsg, pscenario & ".uci")
        'ChDir(lWorkingDir & "temp")
        'lCombinedUci.Save()

        'read first uci
        lCombinedUci.FastReadUciForStarter(lMsg, lUcis(1))
        lCombinedUci.MetSeg2Source()
        lCombinedUci.Point2Source()

        'make this the combined uci
        lCombinedUci.Name = pScenario & ".uci"
        lCombinedUci.GlobalBlock.RunInf.Value = "UCI " & pScenario & " for " & pWatershed

        Dim lMetSegCounter As Integer = 100
        Dim lLandUseCounter As Integer = 1
        Dim lOrigId As Integer
        Dim lTotalMetSegCount As Integer = 7

        'change this operation number
        Dim lOper As atcUCI.HspfOperation
        lOper = lCombinedUci.OpnBlks("PERLND").Ids(0)
        lOrigId = lOper.Id
        lOper.Id = 101
        'remove all the targets from this perlnd
        Dim lConn As atcUCI.HspfConnection
        'For Each lConn In lOper.Targets
        lOper.Targets.Clear() '.Remove(1)
        'Next lConn
        'renumber data sets to reflect met seg number
        For Each lConn In lOper.Sources
            lConn.Source.VolId = lConn.Source.VolId + lMetSegCounter
        Next lConn

        'change operation number in all special actions
        Dim lRecord As atcUCI.HspfSpecialRecord
        Dim lUvQuanIndex As Integer
        RenumberOperationInSpecialActions(lCombinedUci, "PERLND", lOrigId, lOper.Id)
        Dim lRecordIndex As Integer = 1
        Do While lRecordIndex < lCombinedUci.SpecialActionBlk.Records.Count
            lRecord = lCombinedUci.SpecialActionBlk.Records(lRecordIndex)
            If InStr(lRecord.Text, "+=         0.") = 58 Then
                lCombinedUci.SpecialActionBlk.Records.Remove(lRecordIndex)
            ElseIf lRecord.SpecType = atcUCI.HspfData.HspfSpecialRecordType.hUserDefineQuan And _
                Mid(lRecord.Text, 1, 16) = "  UVQUAN prec   " Then
                'need one of these for each met segment
                lRecord.Text = "  UVQUAN prec1  " & Mid(lRecord.Text, 17)
                lUvQuanIndex = lRecordIndex
            Else
                If Mid(lRecord.Text, 1, 9) = "IF (prec " Then
                    'update to reflect met segment number
                    lRecord.Text = "IF (prec1 " & Mid(lRecord.Text, 10)
                End If
                lRecordIndex += 1
            End If
        Loop

        'save names of met and precip wdms for each met seg
        Dim lMetWDMNames As New Collection
        lMetWDMNames.Add(ReplaceString(lCombinedUci.FilesBlock.Value(1).Name.ToLower, "c:\", pBaseDrive))
        Dim lPrecWDMNames As New Collection
        lPrecWDMNames.Add(ReplaceString(lCombinedUci.FilesBlock.Value(2).Name.ToLower, "c:\", pBaseDrive))
        'save names of each pt src and output wdm for each rchres
        Dim lPtSrcWDMNames As New Collection
        Dim lOutputWDMNames As New Collection

        'update files block
        UpdateFilesBlock(lCombinedUci)

        Logger.Dbg("CombinedUCI build based on " & lUcis(1))

        'now start looping through the rest of the ucis
        For lUciIndex As Integer = 2 To lUcis.Count
            'read each uci 
            Dim lUci As New atcUCI.HspfUci
            lUci.FastReadUciForStarter(lMsg, lUcis(lUciIndex))
            lUci.MetSeg2Source()
            lUci.Point2Source()

            'add operations of second uci into first uci
            Dim lNewOperId As Integer
            For Each lOper In lUci.OpnSeqBlock.Opns
                If lOper.Name = "PERLND" Or lOper.Name = "IMPLND" Then
                    lMetSegCounter = lMetSegCounter + 100
                    If lMetSegCounter > lTotalMetSegCount * 100 Then
                        Logger.Dbg("NewLandUse " & lOper.Tables("GEN-INFO").Parms(0).Value)
                        lLandUseCounter = lLandUseCounter + 1
                        lMetSegCounter = 100
                    End If
                    'save the names of the wdm files associated with each met segment
                    lMetWDMNames.Add(ReplaceString(lUci.FilesBlock.Value(1).Name.ToLower, "c:\", pBaseDrive))
                    lPrecWDMNames.Add(ReplaceString(lUci.FilesBlock.Value(2).Name.ToLower, "c:\", pBaseDrive))
                    lNewOperId = lMetSegCounter + lLandUseCounter
                Else
                    lNewOperId = 1
                End If
                'make sure this is a unique number
                Do While Not lCombinedUci.OpnBlks(lOper.Name).OperFromID(lNewOperId) Is Nothing
                    lNewOperId = lNewOperId + 1
                Loop

                'add this operation
                Dim lOpn As New atcUCI.HspfOperation
                lOpn = lOper
                lOpn.Name = lOper.Name
                lOrigId = lOper.Id
                lOpn.Id = lNewOperId
                lOpn.Uci = lCombinedUci
                lCombinedUci.OpnBlks(lOper.Name).Ids.Add(lOpn) ', "K" & lOpn.Id)
                lOpn.OpnBlk = lCombinedUci.OpnBlks(lOper.Name)
                lCombinedUci.OpnSeqBlock.Add(lOper)

                'remove the comments so we don't get repeated headers
                For Each lTable As atcUCI.HspfTable In lOpn.Tables
                    lTable.Comment = ""
                Next lTable

                If lOper.Name = "IMPLND" Then
                    'make adjustment to GEN-INFO table where cbp fields are off
                    lOper.Tables("GEN-INFO").Parms(3).Value = lOper.Tables("GEN-INFO").Parms(4).Value
                    lOper.Tables("GEN-INFO").Parms(4).Value = lOper.Tables("GEN-INFO").Parms(5).Value
                    lOper.Tables("GEN-INFO").Parms(5).Value = lOper.Tables("GEN-INFO").Parms(6).Value
                End If

                If lOper.Name = "RCHRES" Then
                    'update ftable number
                    lOper.FTable.Id = lNewOperId
                    lOper.Tables("HYDR-PARM2").ParmValue("FTBUCI") = lNewOperId
                    'make note of which met segment this uci uses
                    Dim lUciPrecWdmName As String = ReplaceString(lUci.FilesBlock.Value(2).Name.ToLower, "c:\", pBaseDrive)
                    For i As Integer = 1 To lPrecWDMNames.Count
                        If lPrecWDMNames(i) = lUciPrecWdmName Then
                            lMetSegCounter = i * 100
                            Exit For
                        End If
                    Next i
                    'store name of the pt src wdm this uci uses (needs to be modified slightly)
                    lPtSrcWDMNames.Add(Mid(lUci.FilesBlock.Value(3).Name, 1, 17) & "609" & Mid(lUci.FilesBlock.Value(3).Name, 21))
                    'store name of the output wdm this uci uses
                    lOutputWDMNames.Add(lUci.FilesBlock.Value(4).Name)
                End If

                If lOper.Name = "PERLND" Or lOper.Name = "IMPLND" Then
                    'remove all the targets from perlnds and implnds
                    '(we can pass internally without writing to wdm)
                    'For Each lConn In lOpn.Targets
                    lOpn.Targets.Clear() ' .Remove(1)
                    'Next lConn
                    'renumber data sets to reflect met seg number
                    For Each lConn In lOpn.Sources
                        If lConn.Target.VolName = lOper.Name Then
                            'renumber the dsn in the ext sources block
                            lConn.Source.VolId = lConn.Source.VolId + lMetSegCounter
                        End If
                    Next lConn
                ElseIf lOper.Name = "RCHRES" Then
                    'remove all the sources coming from upstream through wdms
                    '(we can pass internally without reading from wdm)
                    For lSourceIndex As Integer = lOper.Sources.Count - 1 To 0 Step -1
                        If lOper.Sources(lSourceIndex).Source.VolName = "WDM4" Then
                            lOper.Sources.RemoveAt(lSourceIndex)
                        End If
                    Next
                    'renumber data sets to reflect met seg number
                    For Each lConn In lOpn.Sources
                        If lConn.Target.VolName = lOper.Name Then
                            'renumber the dsn in the ext sources block
                            lConn.Source.VolId = lConn.Source.VolId + lMetSegCounter
                        End If
                    Next lConn
                    'reset the connection operation numbers
                    For Each lConn In lOper.Targets
                        If lConn.Source.VolName = "RCHRES" Then
                            lConn.Source.Opn.Id = lNewOperId
                            lConn.Source.VolId = lNewOperId
                            If lConn.Target.VolName = "WDM4" Then
                                lConn.Target.VolId = lConn.Target.VolId + (100 * (lNewOperId - 1))
                            End If
                        End If
                    Next lConn
                    For Each lConn In lOper.Sources
                        If lConn.Target.VolName = "RCHRES" Then
                            If Not lConn.Target.Opn Is Nothing Then
                                lConn.Target.Opn.Id = lNewOperId
                            End If
                            lConn.Target.VolId = lNewOperId
                        End If
                    Next lConn
                End If

                RenumberOperationInSpecialActions(lUci, lOper.Name, lOrigId, lOper.Id)
                'now add the special actions records to the uci
                For Each lRecord In lUci.SpecialActionBlk.Records
                    If lRecord.SpecType = atcUCI.HspfData.HspfSpecialRecordType.hAction Or _
                       lRecord.SpecType = atcUCI.HspfData.HspfSpecialRecordType.hCondition Or _
                       lRecord.SpecType = atcUCI.HspfData.HspfSpecialRecordType.hUserDefineName Then
                        If Mid(lRecord.Text, 1, 9) = "IF (prec " Then
                            'update to reflect met segment number
                            lRecord.Text = "IF (prec" & Mid(CStr(lOper.Id), 1, 1) & " " & Mid(lRecord.Text, 10)
                        End If
                        If InStr(lRecord.Text, "+=         0.") <> 58 Then
                            lCombinedUci.SpecialActionBlk.Records.Add(lRecord)
                        End If
                    End If
                Next
            Next lOper

            lUci = Nothing
            Logger.Dbg("Added " & lUcis(lUciIndex))
        Next lUciIndex

        'build connections
        AddSchematicBlock(lCombinedUci)
        Logger.Dbg("ConnectionsBuilt")

        'build mass links
        AddMassLinks(lCombinedUci)
        Logger.Dbg("MassLinksBuilt")

        'remove duplicate mass links
        FilterMassLinks(lCombinedUci)
        Logger.Dbg("MassLinkDuplicatesRemoved")

        'convert back to met segs to look nice in the save
        lCombinedUci.Source2MetSeg()
        Logger.Dbg("MetSegmentsPolished")

        'add uvquans for each met segment
        lRecord = lCombinedUci.SpecialActionBlk.Records(lUvQuanIndex)
        For i As Integer = 2 To lTotalMetSegCount
            Dim lNewRecord As New atcUCI.HspfSpecialRecord
            lNewRecord.SpecType = lRecord.SpecType
            lNewRecord.Text = "  UVQUAN prec" & CStr(i) & "  " & Mid(lRecord.Text, 17, 7) & CStr(i) & Mid(lRecord.Text, 25)
            lCombinedUci.SpecialActionBlk.Records.Add(lNewRecord, , , lUvQuanIndex)
            lUvQuanIndex = lUvQuanIndex + 1
        Next i
        Logger.Dbg("UvquansAdded")

        'combine the WDM files used by the combined UCI
        ChDir(pOutputDir)
        BuildCombinedWDMs(lCombinedUci, lMetWDMNames, lPrecWDMNames, lPtSrcWDMNames, lOutputWDMNames)
        Logger.Dbg("CombinedWdmsBuilt")

        With lCombinedUci
            'fill in name of pltgen file 
            .FilesBlock.AddFromSpecs(pScenario & ".plt", "", 31)
            'write the combined uci 
            .Save()
            Logger.Dbg("CombinedUCI Saved")

            'try to add binary output
            Dim lAllOperations As Boolean = True
            Dim lBinChange As Boolean = False
            Dim lIndex As Integer = 0
            Dim lOpnTypesWithBinary() As String = {"RCHRES", "PERLND", "IMPLND"} '{"RCHRES"} 
            For Each lOpnType As String In lOpnTypesWithBinary
                lIndex += 1
                Dim lOpnBlk As atcUCI.HspfOpnBlk = .OpnBlks(lOpnType)

                If lOpnBlk.Ids.Count = 0 Then
                    Logger.Dbg("No " & lOpnType & " to add binary output to")
                Else
                    Dim lBinUnit As Integer = 40 + lIndex  'TODO: don't hard code this unit number
                    .FilesBlock.AddFromSpecs(pScenario & "." & lOpnType.ToLower & ".hbn", "BINO", lBinUnit)

                    Dim lTableName As String = "BINARY-INFO"
                    If Not lOpnBlk.TableExists(lTableName) Then
                        lOpnBlk.AddTableForAll(lTableName, lOpnType)
                    End If

                    Dim lStartOperation As Integer = lOpnBlk.Ids.Count
                    If lAllOperations Then
                        lStartOperation = 0
                    End If
                    For lOperationIndex As Integer = lStartOperation To lOpnBlk.Ids.Count - 1
                        Dim lOperation As atcUCI.HspfOperation = lOpnBlk.Ids(lOperationIndex)
                        Dim lTable As atcUCI.HspfTable = lOperation.Tables("GEN-INFO")
                        Logger.Dbg("GenInfoParmCount " & lTable.Parms.Count)
                        lTable.Parms(lTable.Parms.Count - 2).Value = lBinUnit
                        lTable = lOperation.Tables(lTableName)

                        Logger.Dbg("BinaryTableParmCount " & lTable.Parms.Count)
                        For i As Integer = 0 To lTable.Parms.Count - 3
                            lTable.Parms(i).Value = 3 'day 4 'month
                        Next
                        lTable.Parms(lTable.Parms.Count - 1).Value = 12
                        Logger.Dbg("Binary output added for " & lOperation.Name & " " & _
                                                                lOperation.Id)

                        lBinChange = True
                    Next lOperationIndex
                End If
            Next

            If lBinChange Then
                .Name = "bin_" & .Name
                .Save()
            End If
        End With
    End Sub

    Private Function HSPFUciNames(ByVal aWorkingDir As String) As Collection
        Dim lPerlndUcis As New Collection
        Dim lImplndUcis As New Collection
        Dim lRchresUcis As New Collection
        Dim lUciFullNames As New NameValueCollection
        Dim lString As String
        Dim lUciName As String
        Dim i As Integer
        Dim lUcis As New Collection

        AddFilesInDir(lUciFullNames, aWorkingDir, False, "*.uci")
        Logger.Dbg("Processing " & lUciFullNames.Count & " ucis")

        'we could open each uci and look to see what operation it contains,
        'but we know based on the naming convention
        For Each lString In lUciFullNames
            lUciName = FilenameNoPath(lString)
            If Mid(lUciName, 1, 3) = "afo" Or _
               Mid(lUciName, 1, 3) = "imh" Or _
               Mid(lUciName, 1, 3) = "iml" Then
                lImplndUcis.Add(lUciName)
            ElseIf Mid(lUciName, 1, 3) = "riv" Then
                lRchresUcis.Add(lUciName)
            Else
                lPerlndUcis.Add(lUciName)
            End If
        Next lString
        'put them in order
        For i = 1 To lPerlndUcis.Count
            lUcis.Add(lPerlndUcis(i))
        Next i
        For i = 1 To lImplndUcis.Count
            lUcis.Add(lImplndUcis(i))
        Next i
        For i = 1 To lRchresUcis.Count
            lUcis.Add(lRchresUcis(i))
        Next i

        HSPFUciNames = lUcis

    End Function

    Private Function UpdateFilesBlock(ByVal aUci As atcUCI.HspfUci) As Boolean
        Dim lHspfFile As New atcUCI.HspfData.HspfFile
        With aUci.FilesBlock
            lHspfFile.Name = pScenario & ".wdm"
            lHspfFile.Typ = "WDM1"
            lHspfFile.Unit = "21"
            .Value(0) = lHspfFile
            lHspfFile.Name = pScenario & ".ptsrc.wdm"
            lHspfFile.Typ = "WDM3"
            lHspfFile.Unit = "23"
            .Value(1) = lHspfFile
            lHspfFile.Name = pScenario & ".output.wdm"
            lHspfFile.Typ = "WDM4"
            lHspfFile.Unit = "24"
            .Value(2) = lHspfFile
            lHspfFile.Name = pScenario & ".ech"
            lHspfFile.Typ = "MESSU"
            lHspfFile.Unit = "25"
            .Value(3) = lHspfFile
            lHspfFile.Name = pScenario & ".out"
            lHspfFile.Typ = ""
            lHspfFile.Unit = "26"
            .Value(4) = lHspfFile
        End With
    End Function

    Private Function RenumberOperationInSpecialActions(ByVal aUci As atcUCI.HspfUci, ByVal aOperName As String, ByVal aOrigId As Integer, ByVal aNewId As Integer) As Boolean
        Dim lRecord As atcUCI.HspfSpecialRecord
        Dim i As Integer

        For Each lRecord In aUci.SpecialActionBlk.Records
            i = InStr(lRecord.Text, aOperName & "  " & aOrigId)
            If i > 0 Then
                Mid(lRecord.Text, i) = aOperName & aNewId
            End If
            i = InStr(lRecord.Text, aOperName & "   " & aOrigId)
            If i > 0 Then
                Mid(lRecord.Text, i) = aOperName & " " & aNewId
            End If
        Next
    End Function

    Private Function AddSchematicBlock(ByVal aCombinedUci As atcUCI.HspfUci) As Boolean
        'build connections 
        Dim lAreaTable As New atcTableDelimited
        If lAreaTable.OpenFile(pAreaTableDir & "land_use_base10_" & pLandUseYear & ".csv") Then
            'look through each rchres
            Dim lLandUse As String
            Dim lFieldNum(1) As Integer
            Dim lTableOper(1) As String
            Dim lFieldVal(1) As String
            Dim lArea As Single
            Dim lOperTypes() As String = {"PERLND", "IMPLND"}
            lFieldNum(0) = 1
            lFieldNum(1) = 2
            lTableOper(0) = "="
            lTableOper(1) = "="
            Dim lRchIDs As New Collection
            Dim lDownRchIDs As New Collection
            For Each lRchOper As atcUCI.HspfOperation In aCombinedUci.OpnBlks("RCHRES").Ids
                lFieldVal(0) = lRchOper.Tables("GEN-INFO").Parms("RCHID").Value
                For Each lOperType As String In lOperTypes
                    For Each lLandOper As atcUCI.HspfOperation In aCombinedUci.OpnBlks(lOperType).Ids
                        lFieldVal(1) = Mid(lLandOper.Tables("GEN-INFO").Parms("LSID").Value, 1, 6)
                        lLandUse = Trim(Mid(lLandOper.Tables("GEN-INFO").Parms("LSID").Value, 7))
                        If lAreaTable.FindMatch(lFieldNum, lTableOper, lFieldVal) Then
                            'found this land series contributing to this reach
                            lArea = lAreaTable.Value(lAreaTable.FieldNumber(lLandUse.ToLower))
                            If lArea > 0 Then
                                Dim lConn As New atcUCI.HspfConnection
                                lConn.Uci = aCombinedUci
                                lConn.Typ = 3
                                lConn.Source.VolName = lLandOper.Name
                                lConn.Source.VolId = lLandOper.Id
                                lConn.Source.Opn = lLandOper
                                lConn.MFact = lArea
                                lConn.Target.VolName = "RCHRES"
                                lConn.Target.VolId = lRchOper.Id
                                lConn.Target.Opn = lRchOper
                                lConn.MassLink = Mid(CStr(lLandOper.Id), 2)
                                aCombinedUci.Connections.Add(lConn)
                                lLandOper.Targets.Add(lConn)
                                lRchOper.Sources.Add(lConn)
                            End If
                        End If
                    Next lLandOper
                Next lOperType
                'also get reach to reach connections out of the reach id
                lRchIDs.Add(Mid(lFieldVal(0), 5, 4))
                lDownRchIDs.Add(Mid(lFieldVal(0), 10, 4))
            Next lRchOper
            'add reach to reach connections
            For i As Integer = 1 To lRchIDs.Count
                For j As Integer = 1 To lRchIDs.Count
                    If lRchIDs(j) = lDownRchIDs(i) Then
                        Dim lConn As New atcUCI.HspfConnection
                        lConn.Uci = aCombinedUci
                        lConn.Typ = 3
                        lConn.Source.VolName = "RCHRES"
                        lConn.Source.VolId = i
                        lConn.Source.Opn = aCombinedUci.OpnBlks("RCHRES").OperFromID(i)
                        lConn.MFact = 1.0#
                        lConn.Target.VolName = "RCHRES"
                        lConn.Target.VolId = j
                        lConn.Target.Opn = aCombinedUci.OpnBlks("RCHRES").OperFromID(j)
                        lConn.MassLink = 99
                        aCombinedUci.Connections.Add(lConn)
                        aCombinedUci.OpnBlks("RCHRES").OperFromID(i).Targets.Add(lConn)
                        aCombinedUci.OpnBlks("RCHRES").OperFromID(j).Sources.Add(lConn)
                    End If
                Next j
            Next i
        Else
            Throw New Exception("Failed to Open LandUse in '" & pAreaTableDir & "'")
        End If
    End Function

    Private Function AddMassLinks(ByVal aCombinedUci As atcUCI.HspfUci) As Boolean
        'build mass links 
        Dim lMassLink As atcUCI.HspfMassLink
        Dim lLandOper As atcUCI.HspfOperation
        Dim lLandUse As String

        'TODO why this - aCombinedUci.MassLinks.Remove(1)
        Dim lMultTable As New atcTableFixed
        lMultTable.NumFields = 3
        lMultTable.FieldStart(1) = 9     'riv id
        lMultTable.FieldLength(1) = 3
        lMultTable.FieldStart(2) = 23    'land id
        lMultTable.FieldLength(2) = 3
        lMultTable.FieldStart(3) = 36    'mfact
        lMultTable.FieldLength(3) = 8

        If lMultTable.OpenFile(pConnectionDir & "land_to_river") Then
            Dim lRchresTable As New atcTableFixed
            lRchresTable.NumFields = 5
            lRchresTable.FieldStart(1) = 7   'riv id
            lRchresTable.FieldLength(1) = 3
            lRchresTable.FieldStart(2) = 14  'group
            lRchresTable.FieldLength(2) = 6
            lRchresTable.FieldStart(3) = 21  'member 
            lRchresTable.FieldLength(3) = 6
            lRchresTable.FieldStart(4) = 28  'mems1
            lRchresTable.FieldLength(4) = 1
            lRchresTable.FieldStart(5) = 30  'mems2
            lRchresTable.FieldLength(5) = 1

            If lRchresTable.OpenFile(pConnectionDir & "rchres_in") Then
                Dim lLandTable As New atcTableFixed
                lLandTable.NumFields = 6
                lLandTable.FieldStart(1) = 1   'land id
                lLandTable.FieldLength(1) = 3
                lLandTable.FieldStart(2) = 7   'group
                lLandTable.FieldLength(2) = 6
                lLandTable.FieldStart(3) = 14  'member 
                lLandTable.FieldLength(3) = 6
                lLandTable.FieldStart(4) = 21  'mems1
                lLandTable.FieldLength(4) = 1
                lLandTable.FieldStart(5) = 23  'mems2
                lLandTable.FieldLength(5) = 1
                lLandTable.FieldStart(6) = 68  'land use names
                lLandTable.FieldLength(6) = 88

                'now loop through each perlnd/implnd record making mass links
                For Each lLandType As String In New String() {"PERLND", "IMPLND"}
                    If lLandTable.OpenFile(pConnectionDir & lLandType.ToLower) Then
                        For Each lLandOper In aCombinedUci.OpnBlks(lLandType).ids
                            If lLandOper.Id < 200 Then
                                'only need to do for the first segment
                                lLandUse = Trim(Mid(lLandOper.Tables("GEN-INFO").parms("LSID").value, 7))

                                For lLandTableIndex As Integer = 1 To lLandTable.NumRecords 'Do Until lLandTable.atEOF
                                    lLandTable.CurrentRecord = lLandTableIndex
                                    If IsNumeric(lLandTable.Value(1)) And InStr(lLandTable.Value(6), lLandUse) > 0 Then
                                        'look for this land id in mfacts table
                                        For lMultTableIndex As Integer = 1 To lMultTable.NumRecords 'Do Until lMultTable.atEOF
                                            lMultTable.CurrentRecord = lMultTableIndex
                                            If lLandTable.Value(1) = lMultTable.Value(2) Then
                                                'look for this riv id in rchres table
                                                For lRchresTableIndex As Integer = 1 To lRchresTable.NumRecords 'Do Until lRchresTable.atEOF
                                                    lRchresTable.CurrentRecord = lRchresTableIndex
                                                    If lMultTable.Value(1) = lRchresTable.Value(1) Then
                                                        'this is a hit, add it as a mass link
                                                        lMassLink = New atcUCI.HspfMassLink
                                                        lMassLink.Uci = aCombinedUci
                                                        lMassLink.MassLinkId = Mid(CStr(lLandOper.Id), 2)
                                                        lMassLink.Source.VolName = lLandType
                                                        lMassLink.Source.VolId = 0
                                                        lMassLink.Source.Group = lLandTable.Value(2)
                                                        lMassLink.Source.Member = lLandTable.Value(3)
                                                        If IsNumeric(lLandTable.Value(4)) Then
                                                            lMassLink.Source.MemSub1 = lLandTable.Value(4)
                                                        End If
                                                        If IsNumeric(lLandTable.Value(5)) Then
                                                            lMassLink.Source.MemSub2 = lLandTable.Value(5)
                                                        End If
                                                        If IsNumeric(lMultTable.Value(3)) Then
                                                            lMassLink.MFact = lMultTable.Value(3)
                                                        End If
                                                        lMassLink.Tran = ""
                                                        lMassLink.Target.VolName = "RCHRES"
                                                        lMassLink.Target.VolId = 0
                                                        lMassLink.Target.Group = lRchresTable.Value(2)
                                                        lMassLink.Target.Member = lRchresTable.Value(3)
                                                        If IsNumeric(lRchresTable.Value(4)) Then
                                                            lMassLink.Target.MemSub1 = lRchresTable.Value(4)
                                                        End If
                                                        If IsNumeric(lRchresTable.Value(5)) Then
                                                            lMassLink.Target.MemSub2 = lRchresTable.Value(5)
                                                        End If
                                                        aCombinedUci.MassLinks.Add(lMassLink)
                                                    End If
                                                    'lRchresTable.MoveNext()
                                                Next 'Loop
                                            End If
                                            'lMultTable.MoveNext()
                                        Next 'Loop
                                    End If
                                    'lLandTable.MoveNext()
                                Next 'Loop
                            End If
                        Next lLandOper
                    Else
                        Throw New Exception("Failed to Open LandUseTable for " & lLandType & _
                                            " in '" & pConnectionDir & "'")
                    End If
                Next

                'also add the rchres to rchres mass-link
                lMassLink = New atcUCI.HspfMassLink
                lMassLink.Uci = aCombinedUci
                lMassLink.MassLinkID = 99
                lMassLink.Source.VolName = "RCHRES"
                lMassLink.Source.VolId = 0
                lMassLink.Source.Group = "OFLOW"
                lMassLink.Source.Member = ""
                lMassLink.MFact = 1.0#
                lMassLink.Tran = ""
                lMassLink.Target.VolName = "RCHRES"
                lMassLink.Target.VolId = 0
                lMassLink.Target.Group = "INFLOW"
                lMassLink.Target.Member = ""
                aCombinedUci.MassLinks.Add(lMassLink)
            Else
                Throw New Exception("Failed to Open rchres_in table in '" & pConnectionDir & "'")
            End If
        Else
            Throw New Exception("Failed to Open Land_To_River table in '" & pConnectionDir & "'")
        End If
    End Function

    Private Function FilterMassLinks(ByVal aCombinedUci As atcUCI.HspfUci) As Boolean
        'remove identical mass links 
        Dim i As Integer
        Dim j As Integer
        Dim k As Integer
        Dim lMassLink As atcUCI.HspfMassLink
        Dim mlno(-1) As Integer
        Dim mlcnt As Integer = 0
        Dim found As Boolean
        Dim lIdentical As Boolean
        Dim lML1 As atcUCI.HspfMassLink
        Dim lML2 As atcUCI.HspfMassLink
        Dim lConn As atcUCI.HspfConnection

        'develop list of mass link ids
        For j = 0 To aCombinedUci.MassLinks.Count - 1
            lMassLink = aCombinedUci.MassLinks(j)
            found = False
            For k = 0 To mlcnt - 1
                If lMassLink.MassLinkId = mlno(k) Then
                    found = True
                End If
            Next k
            If found = False Then
                mlcnt = mlcnt + 1
                ReDim Preserve mlno(mlcnt)
                mlno(mlcnt - 1) = lMassLink.MassLinkId
            End If
        Next j

        For i = 1 To mlcnt - 1
            'see if this ml is like any previous
            Dim lUmls As New Collection
            For Each lMassLink In aCombinedUci.MassLinks
                If lMassLink.MassLinkID = mlno(i) Then
                    lUmls.Add(lMassLink)
                End If
            Next lMassLink
            For j = 0 To i - 1
                Dim lLmls As New Collection
                For Each lMassLink In aCombinedUci.MassLinks
                    If lMassLink.MassLinkID = mlno(j) Then
                        lLmls.Add(lMassLink)
                    End If
                Next lMassLink
                'now compare
                lIdentical = True
                If lLmls.Count <> lUmls.Count Then
                    lIdentical = False
                Else
                    For k = 1 To lUmls.Count
                        lML1 = lLmls(k)
                        lML2 = lUmls(k)
                        If lML1.Source.VolName = lML2.Source.VolName And _
                           lML1.Source.Group = lML2.Source.Group And _
                           lML1.Source.Member = lML2.Source.Member And _
                           lML1.Source.MemSub1 = lML2.Source.MemSub1 And _
                           lML1.Source.MemSub2 = lML2.Source.MemSub2 And _
                           lML1.MFact = lML2.MFact And _
                           lML1.Target.VolName = lML2.Target.VolName And _
                           lML1.Target.Group = lML2.Target.Group And _
                           lML1.Target.Member = lML2.Target.Member And _
                           lML1.Target.MemSub1 = lML2.Target.MemSub1 And _
                           lML1.Target.MemSub2 = lML2.Target.MemSub2 Then
                        Else
                            lIdentical = False
                        End If
                    Next
                End If
                If lIdentical Then  'get rid of the second one
                    'For Each lMassLink In aCombinedUci.MassLinks
                    '    If lMassLink.MassLinkId = mlno(i) Then
                    '        aCombinedUci.MassLinks.Remove(lMassLink)
                    '        Exit For
                    '    End If
                    'Next lMassLink
                    'alternative approach to remove more than one - will we stop?
                    For lMassLinkIndex As Integer = aCombinedUci.MassLinks.Count - 1 To 0 Step -1
                        If aCombinedUci.MassLinks(lMassLinkIndex).MassLinkId = mlno(i) Then
                            aCombinedUci.MassLinks.RemoveAt(lMassLinkIndex)
                        End If
                    Next

                    'set schematic block records to first one
                    For Each lConn In aCombinedUci.Connections
                        If lConn.MassLink = mlno(i) Then
                            lConn.MassLink = mlno(j)
                        End If
                    Next
                End If
            Next j
        Next i
    End Function

    Private Function BuildCombinedWDMs(ByVal aCombinedUci As atcUCI.HspfUci, _
                                       ByVal aMetWDMNames As Collection, _
                                       ByVal aPrecWDMNames As Collection, _
                                       ByVal aPtSrcWDMNames As Collection, _
                                       ByVal aOutputWDMNames As Collection) As Boolean
        Dim lOper As atcUCI.HspfOperation
        Dim lConn As atcUCI.HspfConnection
        Dim lWdmIndex As Integer
        Dim lOrigDsn As Integer

        Dim pStartDate As Double = MJD(1984, 1, 1)
        Dim pEndDate As Double = MJD(2001, 1, 1)
        Dim pExistOptionReplace As atcDataSource.EnumExistAction = atcDataSource.EnumExistAction.ExistReplace

        Try 'build combined wdms for prec, met data 
            Logger.Dbg("MetSegmentWdms")
            Dim lMetSeg As atcUCI.HspfMetSeg
            Dim lMetSegRec As atcUCI.HspfMetSegRecord
            For Each lMetSeg In aCombinedUci.MetSegs
                Logger.Dbg("MetSegment " & lMetSeg.Name)
                For Each lMetSegRec In lMetSeg.MetSegRecs
                    'the index is the second digit
                    If Not lMetSegRec Is Nothing Then
                        If Not lMetSegRec.Source.VolName Is Nothing Then
                            Logger.Dbg("CombineUcis:VolId:" & lMetSegRec.Source.VolId)
                            lWdmIndex = CInt(Mid(CStr(lMetSegRec.Source.VolId), 2, 1))
                            lOrigDsn = CInt(Mid(CStr(lMetSegRec.Source.VolId), 1, 1) & "0" & Mid(CStr(lMetSegRec.Source.VolId), 3, 2))
                            If lMetSegRec.Source.VolName = "WDM1" Then
                                CopyDataSet("wdm", aMetWDMNames(lWdmIndex), lOrigDsn, _
                                            "wdm", pScenario & ".wdm", lMetSegRec.Source.VolId)
                            ElseIf lMetSegRec.Source.VolName = "WDM2" Then
                                CopyDataSet("wdm", aPrecWDMNames(lWdmIndex), lOrigDsn, _
                                            "wdm", pScenario & ".wdm", lMetSegRec.Source.VolId)
                                lMetSegRec.Source.VolName = "WDM1"
                            End If

                            'set to start and end dates of run
                            Dim lDataSourceWDM As New atcWDM.atcDataSourceWDM
                            If lDataSourceWDM.Open(pScenario & ".wdm") Then
                                Dim lTimser As atcTimeseries = SubsetByDate(lDataSourceWDM.DataSets.ItemByKey(lMetSegRec.Source.VolId), _
                                                                            pStartDate, pEndDate, Nothing)
                                lDataSourceWDM.AddDataset(lTimser, pExistOptionReplace)
                                lTimser.Clear()
                            End If

                            SetWDMAttribute(pScenario & ".wdm", lMetSegRec.Source.VolId, "idscen", "OBSERVED")
                            SetWDMAttribute(pScenario & ".wdm", lMetSegRec.Source.VolId, "idlocn", "SEG" & CStr(lWdmIndex))
                        End If
                    End If
                Next
            Next lMetSeg
        Catch lEx As Exception
            Logger.Msg("BuildMetException" & vbCrLf & lEx.ToString)
        End Try

        Try 'build combined wdms for pt srcs and other input wdms
            Logger.Dbg("PointSourcesAndOtherInputWdms")
            For Each lOper In aCombinedUci.OpnSeqBlock.Opns
                For Each lConn In lOper.Sources
                    If Mid(lConn.Source.VolName, 1, 3) = "WDM" Then
                        lOrigDsn = CInt(Mid(CStr(lConn.Source.VolId), 1, 1) & "0" & Mid(CStr(lConn.Source.VolId), 3, 2))

                        'kludge for ignoring that CBP added NH4D as 2004 but we don't have the ucis that have 2004 referenced
                        Select Case lOrigDsn
                            Case 2004 : lOrigDsn = 2005
                            Case 2005 : lOrigDsn = 2006
                            Case 2006 : lOrigDsn = 2007
                        End Select

                        lWdmIndex = CInt(Mid(CStr(lConn.Source.VolId), 2, 1))
                        Logger.Dbg("Volume " & lConn.Source.VolName & " DSN " & lOrigDsn)
                        If lConn.Source.VolName = "WDM3" Then
                            CopyDataSet("wdm", "..\" & aPtSrcWDMNames(lConn.Target.VolId), lOrigDsn, _
                                        "wdm", pScenario & ".ptsrc.wdm", lConn.Source.VolId)
                            SetWDMAttribute(pScenario & ".ptsrc.wdm", lConn.Source.VolId, "idscen", "PT-OBS")
                        ElseIf lConn.Source.VolName = "WDM2" Then
                            CopyDataSet("wdm", aPrecWDMNames(lWdmIndex), lOrigDsn, _
                                        "wdm", pScenario & ".wdm", lConn.Source.VolId)
                            Logger.Dbg("CopyFrom " & aPrecWDMNames(lWdmIndex) & " " & lOrigDsn & " To " & pScenario & ".wdm" & " " & lConn.Source.VolId)
                            'set to start and end dates of run
                            Dim lDataSourceWDM As New atcWDM.atcDataSourceWDM
                            If lDataSourceWDM.Open(pScenario & ".wdm") Then
                                Dim lTimser As atcTimeseries = SubsetByDate(lDataSourceWDM.DataSets.ItemByKey(lConn.Source.VolId), _
                                                                            pStartDate, pEndDate, Nothing)
                                lDataSourceWDM.AddDataset(lTimser, pExistOptionReplace)
                                lTimser.Clear()
                            End If
                            SetWDMAttribute(pScenario & ".wdm", lConn.Source.VolId, "idscen", "OBSERVED")
                            lConn.Source.VolName = "WDM1"
                        End If
                    End If
                Next lConn
            Next lOper
        Catch lEx As Exception
            Logger.Msg("BuildPointException" & vbCrLf & lEx.ToString)
        End Try

        Try 'build combined wdms for output wdms
            Logger.Dbg("OutputWdms")
            For Each lOper In aCombinedUci.OpnSeqBlock.Opns
                For Each lConn In lOper.Targets
                    If lConn.Target.VolName = "WDM4" Then
                        lOrigDsn = CInt("1" & Mid(CStr(lConn.Target.VolId), 2, 2))
                        Logger.Dbg("Volume " & lConn.Target.VolName & " DSN " & lOrigDsn)
                        CopyDataSet("wdm", "..\" & aOutputWDMNames(lConn.Source.VolId), lOrigDsn, _
                                    "wdm", pScenario & ".output.wdm", lConn.Target.VolId)
                        SetWDMAttribute(pScenario & ".output.wdm", lConn.Target.VolId, "idscen", pScenario)
                        SetWDMAttribute(pScenario & ".output.wdm", lConn.Target.VolId, "idlocn", "RIV" & CStr(lOper.Id))
                    End If
                Next lConn
            Next lOper
        Catch lEx As Exception
            Logger.Msg("BuildOutputException" & vbCrLf & lEx.ToString)
        End Try
    End Function

    Public Sub UClimateMain()
        'copy datasets from source wdm to target wdm according to csv file
        Dim lBaseWDMDir As String = "d:\gisdata\CBP\cat\"
        ChDriveDir(lBaseWDMDir)
        Dim lPrecSeries As String = "M" 'M, E, F10 or F30

        Dim lClimScens As New Collection
        Dim lOutputPath As String

        Logger.StartToFile(lBaseWDMDir & "wdmCombiner.log")

        'lClimScens.Add("a_10_cccm")
        'lClimScens.Add("a_10_ccsr")
        'lClimScens.Add("a_10_csir")
        'lClimScens.Add("a_10_echm")
        'lClimScens.Add("a_10_gfdl")
        'lClimScens.Add("a_10_hadc")
        'lClimScens.Add("a_10_ncar")
        'lClimScens.Add("b_10_cccm")
        'lClimScens.Add("b_10_ccsr")
        'lClimScens.Add("b_10_csir")
        'lClimScens.Add("b_10_echm")
        'lClimScens.Add("b_10_gfdl")
        'lClimScens.Add("b_10_hadc")
        'lClimScens.Add("b_10_ncar")
        lClimScens.Add("a_70_cccm")
        lClimScens.Add("a_70_ccsr")
        lClimScens.Add("a_70_csir")
        lClimScens.Add("a_70_echm")
        lClimScens.Add("a_70_gfdl")
        lClimScens.Add("a_70_hadc")
        lClimScens.Add("a_70_ncar")
        lClimScens.Add("b_70_cccm")
        lClimScens.Add("b_70_ccsr")
        lClimScens.Add("b_70_csir")
        lClimScens.Add("b_70_echm")
        lClimScens.Add("b_70_gfdl")
        lClimScens.Add("b_70_hadc")
        lClimScens.Add("b_70_ncar")
        'lClimScens.Add("base")

        For Each lScen As String In lClimScens
            lOutputPath = lBaseWDMDir & lScen & "_" & lPrecSeries & "\"
            If Not FileExists(lOutputPath, True, False) Then
                MkDir(lOutputPath)
            End If
            If Not FileExists(lOutputPath & "base.wdm", False, True) Then
                System.IO.File.Copy(lBaseWDMDir & "base.wdm", lOutputPath & "base.wdm")
            End If
            WDMUpdates(lBaseWDMDir, lBaseWDMDir & "..\met\subset\" & lScen & "\", lOutputPath, lBaseWDMDir & "..\prad\subset\" & lScen & "_" & lPrecSeries & "\")
        Next
    End Sub


    Public Sub WDMUpdates(ByVal aBaseWDMDir As String, ByVal aFromFolder As String, ByVal aToFolder As String, ByVal aAltFromFolder As String)
        'copy datasets from source wdm to target wdm according to csv file
        Dim lWDMTable As New atcTableDelimited
        If Not lWDMTable.OpenFile(aBaseWDMDir & "wdmupdates.csv") Then
            Logger.Dbg("Could not open wdmupdates.csv")
        End If
        Dim lFromWDM As String
        Dim lFromDSN As Integer
        Dim lToWDM As String
        Dim lToDSN As Integer
        Dim lScen As String
        Dim lLoc As String
        Dim lFromFolder As String
        For lWDMTableIndex As Integer = 1 To lWDMTable.NumRecords 'Do Until lWDMTable.atEOF
            lWDMTable.CurrentRecord = lWDMTableIndex
            lFromWDM = lWDMTable.Value(1)
            lFromDSN = lWDMTable.Value(2)
            lToWDM = lWDMTable.Value(3)
            lToDSN = lWDMTable.Value(4)
            lScen = lWDMTable.Value(5)
            lLoc = lWDMTable.Value(6)
            lFromFolder = aFromFolder
            If Not FileExists(lFromFolder & lFromWDM) Then
                'use alternate location
                lFromFolder = aAltFromFolder
            End If
            If Not CopyDataSet("wdm", lFromFolder & lFromWDM, lFromDSN, _
                            "wdm", aToFolder & lToWDM, lToDSN) Then
                Logger.Dbg("problem copying dataset " & lFromDSN & " to " & lToDSN)
            End If
            SetWDMAttribute(aToFolder & lToWDM, lToDSN, "idscen", lScen)
            SetWDMAttribute(aToFolder & lToWDM, lToDSN, "idlocn", Mid(IO.Path.GetFileNameWithoutExtension(lFromWDM), 5))
            'write out summary info
            Dim lDataSource As New atcWDM.atcDataSourceWDM
            If lDataSource.Open(aToFolder & lToWDM) Then
                Dim lDataSetIndex As Integer = lDataSource.DataSets.IndexFromKey(lToDSN)
                Dim lDataSet As atcTimeseries = Nothing
                If lDataSetIndex >= 0 Then
                    lDataSet = lDataSource.DataSets.ItemByIndex(lDataSetIndex)
                    lDataSource.ReadData(lDataSet)
                    Dim lsum As Double
                    For i As Integer = 1 To lDataSet.numValues
                        lsum = lsum + lDataSet.Value(i)
                    Next
                    Dim lmean As Double
                    lmean = lsum / lDataSet.numValues
                    Logger.Dbg("Copied data set " & lToDSN & " mean " & lmean)
                End If
            End If
        Next ' Loop
    End Sub
End Module
