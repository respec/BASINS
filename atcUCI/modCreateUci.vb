'Copyright 2006 AQUA TERRA Consultants - Royalty-free use permitted under open source license
Option Strict Off
Option Explicit On

Imports System.Collections.ObjectModel
Imports MapWinUtility
Imports atcUtility
Imports atcData

Friend Class LandUse
    Public Name As String
    Public Type As String
    Public Reach As String
    Public Area As Double
    Public Slope As Double
    Public Distance As Double
    Public Id As Integer
    Public Oper As String
End Class
Friend Class Reach
    Public Id As String
    Public Name As String
    Public WsId As String
    Public NExits As Integer
    Public Type As String
    Public Length As Double
    Public DeltH As Double
    Public DownID As String
    Public Elev As Double
    Public Depth As Double
    Public Width As Double
    Public Manning As Double
    Public Order As Integer
End Class

Module modCreateUci
    Dim pLandUses As Collection(Of LandUse)
    Dim pReaches As Collection(Of Reach)
    'channel file
    Dim chanid() As String
    Dim ChanL() As Single
    Dim ChanYm() As Single
    Dim ChanWm() As Single
    Dim ChanN() As Single
    Dim ChanS() As Single
    Dim ChanM11() As Single
    Dim ChanM12() As Single
    Dim ChanYc() As Single
    Dim ChanM21() As Single
    Dim ChanM22() As Single
    Dim ChanYt1() As Single
    Dim ChanYt2() As Single
    Dim ChanM31() As Single
    Dim ChanM32() As Single
    Dim ChanW11() As Single
    Dim ChanW12() As Single
    Dim ChanRecCnt As Integer

    Dim pLandName(2) As String
    Dim pLastSeg(2) As Integer
    Dim pFirstSeg(2) As Integer

    Dim FacilityCount As Integer
    Dim FacilityName() As String
    Dim FacilityNpdes() As String
    Dim FacilityReach() As String
    Dim FacilityMile() As Single

    Dim PollutantCount As Integer
    Dim PollutantFacID() As Integer
    Dim PollutantName() As String
    Dim PollutantLoad() As Single

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

            pLandUses = New Collection(Of LandUse)
            pReaches = New Collection(Of Reach)
            Dim lReturnCode As Integer
            Call ReadWSDFile(aWsdName, lReturnCode)
            Call ReadRCHFile(aWsdName, lReturnCode)
            Call ReadPTFFile(aWsdName, lReturnCode)
            Call ReadPSRFile(aWsdName, lReturnCode)

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
                For Each lOpn As HspfOperation In aUci.OpnSeqBlock.Opns
                    lOpnBlk = aUci.OpnBlks.Item(lOpn.Name)
                    lOpnBlk.Ids.Add(lOpn)
                    lOpn.OpnBlk = lOpnBlk
                Next
                For Each lOpnBlk In aUci.OpnBlks  'perlnd, implnd, etc
                    If lOpnBlk.Count > 0 Then
                        lOpnBlk.CreateTables(aMsg.BlockDefs.Item(lOpnBlk.Name))
                    End If
                Next

                For i As Integer = 1 To ChanRecCnt 'process ftables
                    Dim lOpn As HspfOperation = aUci.OpnBlks.Item("RCHRES").OperFromID(CShort(chanid(i)))
                    If Not lOpn Is Nothing Then
                        lOpn.FTable.FTableFromCrossSect(ChanL(i), ChanYm(i), ChanWm(i), ChanN(i), ChanS(i), ChanM11(i), ChanM12(i), ChanYc(i), ChanM21(i), ChanM22(i), ChanYt1(i), ChanYt2(i), ChanM31(i), ChanM32(i), ChanW11(i), ChanW12(i))
                    End If
                Next i

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
                setDefault(aUci, lDefUci)
                setDefaultML(aUci, lDefUci)
            End If
        End If

        aUci.Edited = False 'all the reads set edited

    End Sub

    Private Sub ReadWSDFile(ByRef aName As String, ByRef aReturnCode As Integer)
        aReturnCode = 0
        Dim lDelim As String = " "
        Dim lQuote As String = """"

        'read wsd file
        Dim lFileUnit As Integer = FreeFile()
        On Error GoTo ErrHandler
        Dim lName As String = FilenameOnly(aName) & ".wsd"
        FileOpen(lFileUnit, lName, OpenMode.Input)
        Dim lStr As String = LineInput(lFileUnit) 'header line
        Do Until EOF(lFileUnit)
            lStr = LineInput(lFileUnit)
            Dim lLandUse As New LandUse
            'count the number of fields in this string
            Dim lStrTemp As String = lStr
            Dim lFieldCount As Integer = 0
            Do While StrSplit(lStrTemp, lDelim, lQuote).Length > 0
                lFieldCount += 1
            Loop
            If lFieldCount = 6 Then
                'this is the normal way
                lLandUse.Name = StrSplit(lStr, lDelim, lQuote)
                lLandUse.Type = CInt(StrSplit(lStr, lDelim, lQuote))
                lLandUse.Reach = StrSplit(lStr, lDelim, lQuote)
                lLandUse.Area = CDbl(StrSplit(lStr, lDelim, lQuote))
                lLandUse.Slope = CDbl(StrSplit(lStr, lDelim, lQuote))
                lLandUse.Distance = CDbl(StrSplit(lStr, lDelim, lQuote))
            Else
                'if coming from old delineator might not be space delimited
                lLandUse.Name = StrSplit(lStr, lDelim, lQuote)
                If lStr.Length > 23 Then
                    lLandUse.Distance = CSng(Mid(lStr, lStr.Length - 7, 8))
                    lLandUse.Slope = CSng(Mid(lStr, lStr.Length - 15, 8))
                    lLandUse.Area = CSng(Mid(lStr, lStr.Length - 23, 8))
                End If
                lLandUse.Type = CInt(StrSplit(lStr, lDelim, lQuote))
                lLandUse.Reach = StrSplit(lStr, lDelim, lQuote)
            End If
            pLandUses.Add(lLandUse)
        Loop
        FileClose(lFileUnit)
        Exit Sub
ErrHandler:
        Logger.Msg("Problem reading file " & lName, "Create Problem")
        aReturnCode = 1
    End Sub

    Private Sub ReadRCHFile(ByRef newName As String, ByRef ret As Integer)
        Dim tname, quote, delim, lstr, tstr As String
        Dim i, amax As Integer

        ret = 0
        delim = " "
        quote = """"

        'read rch file
        i = FreeFile()
        On Error GoTo ErrHandler
        tname = Left(newName, Len(newName) - 3) & "rch"
        FileOpen(i, tname, OpenMode.Input)
        lstr = LineInput(i) 'header line
        Do Until EOF(i)
            lstr = LineInput(i)
            Dim lReach As New Reach
            lReach.Id = StrSplit(lstr, delim, quote)
            lReach.Name = StrSplit(lstr, delim, quote)
            lReach.WsId = StrSplit(lstr, delim, quote)
            tstr = StrSplit(lstr, delim, quote)
            lReach.NExits = CInt(StrSplit(lstr, delim, quote))
            tstr = StrSplit(lstr, delim, quote)
            lReach.Type = StrSplit(lstr, delim, quote)
            lReach.Length = CDbl(StrSplit(lstr, delim, quote))
            lReach.DeltH = CDbl(StrSplit(lstr, delim, quote))
            lReach.Elev = CDbl(StrSplit(lstr, delim, quote))
            tstr = StrSplit(lstr, delim, quote)
            tstr = StrSplit(lstr, delim, quote)
            lReach.DownID = StrSplit(lstr, delim, quote)
            tstr = StrSplit(lstr, delim, quote)
            tstr = StrSplit(lstr, delim, quote)
            tstr = StrSplit(lstr, delim, quote)
            tstr = StrSplit(lstr, delim, quote)
            tstr = StrSplit(lstr, delim, quote)
            tstr = StrSplit(lstr, delim, quote)
            lReach.Depth = CDbl(StrSplit(lstr, delim, quote))
            lReach.Width = CDbl(StrSplit(lstr, delim, quote))
            tstr = StrSplit(lstr, delim, quote)
            tstr = StrSplit(lstr, delim, quote)
            tstr = StrSplit(lstr, delim, quote)
            tstr = StrSplit(lstr, delim, quote)
            tstr = StrSplit(lstr, delim, quote)
            tstr = StrSplit(lstr, delim, quote)
            lReach.Manning = CDbl(StrSplit(lstr, delim, quote))
            lReach.Order = pReaches.Count
            pReaches.Add(lReach)
        Loop
        FileClose(i)
        Exit Sub
ErrHandler:
        Logger.Msg("Problem reading file " & tname, "Create Problem")
        ret = 2
    End Sub

    Public Sub ReadPTFFile(ByRef newName As String, ByRef ret As Integer)

        Dim tname, quote, delim, lstr, tstr As String
        Dim i, amax As Integer

        ret = 0
        delim = " "
        quote = """"

        'read ptf file for channel data
        i = FreeFile()
        On Error GoTo ErrHandler
        tname = Left(newName, Len(newName) - 3) & "ptf"
        FileOpen(i, tname, OpenMode.Input)
        lstr = LineInput(i) 'header line
        ChanRecCnt = 0
        ReDim chanid(1)
        ReDim ChanL(1)
        ReDim ChanYm(1)
        ReDim ChanWm(1)
        ReDim ChanN(1)
        ReDim ChanS(1)
        ReDim ChanM11(1)
        ReDim ChanM12(1)
        ReDim ChanYc(1)
        ReDim ChanM21(1)
        ReDim ChanM22(1)
        ReDim ChanYt1(1)
        ReDim ChanYt2(1)
        ReDim ChanM31(1)
        ReDim ChanM32(1)
        ReDim ChanW11(1)
        ReDim ChanW12(1)
        Do Until EOF(i)
            lstr = LineInput(i)
            ChanRecCnt = ChanRecCnt + 1
            amax = UBound(chanid)
            If ChanRecCnt > amax Then
                ReDim Preserve chanid(amax * 2)
                ReDim Preserve ChanL(amax * 2)
                ReDim Preserve ChanYm(amax * 2)
                ReDim Preserve ChanWm(amax * 2)
                ReDim Preserve ChanN(amax * 2)
                ReDim Preserve ChanS(amax * 2)
                ReDim Preserve ChanM11(amax * 2)
                ReDim Preserve ChanM12(amax * 2)
                ReDim Preserve ChanYc(amax * 2)
                ReDim Preserve ChanM21(amax * 2)
                ReDim Preserve ChanM22(amax * 2)
                ReDim Preserve ChanYt1(amax * 2)
                ReDim Preserve ChanYt2(amax * 2)
                ReDim Preserve ChanM31(amax * 2)
                ReDim Preserve ChanM32(amax * 2)
                ReDim Preserve ChanW11(amax * 2)
                ReDim Preserve ChanW12(amax * 2)
            End If
            chanid(ChanRecCnt) = StrSplit(lstr, delim, quote) 'reach id
            ChanL(ChanRecCnt) = CSng(StrSplit(lstr, delim, quote)) 'length
            ChanYm(ChanRecCnt) = CSng(StrSplit(lstr, delim, quote)) 'mean depth
            ChanWm(ChanRecCnt) = CSng(StrSplit(lstr, delim, quote)) 'mean width
            ChanN(ChanRecCnt) = CSng(StrSplit(lstr, delim, quote)) 'mann n
            ChanS(ChanRecCnt) = CSng(StrSplit(lstr, delim, quote)) 'long slope
            If ChanS(ChanRecCnt) < 0.0001 Then
                ChanS(ChanRecCnt) = 0.0001
            End If
            tstr = StrSplit(lstr, delim, quote)
            ChanM31(ChanRecCnt) = CSng(StrSplit(lstr, delim, quote)) 'side slope upper left
            ChanM21(ChanRecCnt) = CSng(StrSplit(lstr, delim, quote)) 'side slope lower left
            ChanW11(ChanRecCnt) = CSng(StrSplit(lstr, delim, quote)) 'zero slope width left
            ChanM11(ChanRecCnt) = CSng(StrSplit(lstr, delim, quote)) 'side slope chan left
            ChanM12(ChanRecCnt) = CSng(StrSplit(lstr, delim, quote)) 'side slope chan right
            ChanW12(ChanRecCnt) = CSng(StrSplit(lstr, delim, quote)) 'zero slope width right
            ChanM22(ChanRecCnt) = CSng(StrSplit(lstr, delim, quote)) 'side slope lower right
            ChanM32(ChanRecCnt) = CSng(StrSplit(lstr, delim, quote)) 'side slope upper right
            ChanYc(ChanRecCnt) = CSng(StrSplit(lstr, delim, quote)) 'channel depth
            ChanYt1(ChanRecCnt) = CSng(StrSplit(lstr, delim, quote)) 'depth at slope change
            ChanYt2(ChanRecCnt) = CSng(StrSplit(lstr, delim, quote)) 'channel max depth
        Loop
        FileClose(i)
        Exit Sub
ErrHandler:
        Call MsgBox("Problem reading file " & tname, , "Create Problem")
        ret = 3
    End Sub

    Public Sub WritePTFFile(ByRef newName As String, ByRef chanid As String, ByRef ArrayVals() As Single)

        Dim tstr, lstr, delim, quote, tname, Id As String
        Dim i, amax As Integer

        i = Len(chanid)
        If i > 0 Then 'have a reach id
            'write ptf file
            i = FreeFile()
            On Error GoTo ErrHandler
            tname = newName
            FileOpen(i, tname, OpenMode.Output)
            lstr = "'Reach Number','Length(ft)','Mean Depth(ft)','Mean Width (ft)'," & "'Mannings Roughness Coeff.','Long. Slope','Type of x-section','Side slope of upper FP left'," & "'Side slope of lower FP left','Zero slope FP width left(ft)','Side slope of channel left'," & "'Side slope of channel right','Zero slope FP width right(ft)','Side slope lower FP right'," & "'Side slope upper FP right','Channel Depth(ft)','Flood side slope change at depth','Max. depth'," & "'No. of exits','Fraction of flow through exit 1','Fraction of flow through exit 2'," & "'Fraction of flow through exit 3','Fraction of flow through exit 4','Fraction of flow through exit 5'"
            PrintLine(i, lstr) 'header line
            lstr = chanid & " " & ArrayVals(1) & " " & ArrayVals(2) & " " & ArrayVals(3) & " " & ArrayVals(4) & " " & ArrayVals(5) & " " & "Trapezoidal" & " " & ArrayVals(13) & " " & ArrayVals(12) & " " & ArrayVals(11) & " " & ArrayVals(10) & " " & ArrayVals(9) & " " & ArrayVals(8) & " " & ArrayVals(7) & " " & ArrayVals(6) & " " & ArrayVals(14) & " " & ArrayVals(15) & " " & ArrayVals(16) & " " & "1 1 0 0 0 0"
            PrintLine(i, lstr)
            FileClose(i)
            Exit Sub
        End If
ErrHandler:
    End Sub

    Public Sub GetPTFFileIds(ByRef cnt As Integer, ByRef ArrayIds() As String)
        Dim j As Integer

        cnt = ChanRecCnt
        ReDim ArrayIds(cnt)
        For j = 1 To cnt
            ArrayIds(j) = chanid(j)
        Next j
    End Sub

    Public Sub GetPTFData(ByRef RCHId As String, ByRef ArrayVals() As Single)
        Dim i As Integer
        Dim Id As String

        i = Len(RCHId)
        If i > 0 Then 'have a reach id
            Id = CStr(CShort(RCHId))
            For i = 1 To ChanRecCnt
                If Trim(chanid(i)) = Id Then 'found the one
                    ArrayVals(1) = ChanL(i)
                    ArrayVals(2) = ChanYm(i)
                    ArrayVals(3) = ChanWm(i)
                    ArrayVals(4) = ChanN(i)
                    ArrayVals(5) = ChanS(i)
                    ArrayVals(6) = ChanM32(i)
                    ArrayVals(7) = ChanM22(i)
                    ArrayVals(8) = ChanW12(i)
                    ArrayVals(9) = ChanM12(i)
                    ArrayVals(10) = ChanM11(i)
                    ArrayVals(11) = ChanW11(i)
                    ArrayVals(12) = ChanM21(i)
                    ArrayVals(13) = ChanM31(i)
                    ArrayVals(14) = ChanYc(i)
                    ArrayVals(15) = ChanYt1(i)
                    ArrayVals(16) = ChanYt2(i)
                End If
            Next i
        End If
    End Sub

    Private Sub ReadPSRFile(ByRef newName As String, ByRef ret As Integer)

        Dim tname, quote, delim, lstr, tstr As String
        Dim amax, i, j As Integer

        ret = 0
        delim = " "
        quote = """"
        FacilityCount = 0
        PollutantCount = 0

        'read psr file for point source data
        i = FreeFile()
        On Error GoTo ErrHandler
        tname = Left(newName, Len(newName) - 3) & "psr"
        FileOpen(i, tname, OpenMode.Input)
        lstr = LineInput(i) 'number of facilities
        If lstr.Length > 0 Then
            FacilityCount = CShort(lstr)
        End If
        lstr = LineInput(i) 'blank line
        lstr = LineInput(i) 'header line

        If FacilityCount > 0 Then
            'have some facilities
            ReDim FacilityName(FacilityCount)
            ReDim FacilityNpdes(FacilityCount)
            ReDim FacilityReach(FacilityCount)
            ReDim FacilityMile(FacilityCount)
            For j = 1 To FacilityCount
                lstr = LineInput(i)
                FacilityName(j - 1) = StrSplit(lstr, delim, quote)
                FacilityNpdes(j - 1) = StrSplit(lstr, delim, quote)
                FacilityReach(j - 1) = StrSplit(lstr, delim, quote)
                FacilityMile(j - 1) = CSng(StrSplit(lstr, delim, quote))
            Next j
            If Not EOF(i) Then lstr = LineInput(i) 'blank line
            If Not EOF(i) Then lstr = LineInput(i) 'header line
            PollutantCount = 0
            Do Until EOF(i)
                lstr = LineInput(i)
                If lstr.Length > 0 Then
                    PollutantCount = PollutantCount + 1
                    ReDim Preserve PollutantFacID(PollutantCount)
                    ReDim Preserve PollutantName(PollutantCount)
                    ReDim Preserve PollutantLoad(PollutantCount)
                    PollutantFacID(PollutantCount - 1) = CInt(StrSplit(lstr, delim, quote))
                    PollutantName(PollutantCount - 1) = StrSplit(lstr, delim, quote)
                    PollutantLoad(PollutantCount - 1) = CSng(StrSplit(lstr, delim, quote))
                End If
            Loop
        End If
        FileClose(i)
        Exit Sub
ErrHandler:
        Logger.Msg("Problem reading file " & tname, "Create Problem")
        ret = 4
    End Sub

    Private Sub SetInitValues(ByVal aUci As HspfUci)
        Dim j As Integer

        'set init values in uci
        For Each lOperation As HspfOperation In aUci.OpnBlks.Item("PERLND").Ids
            Dim lTable As HspfTable = lOperation.Tables.Item("ACTIVITY")
            lTable.Parms("PWATFG").Value = 1
            j = 0
            Do While j <= pLandUses.Count
                If lOperation.Id = pLandUses(j).Id And pLandUses(j).Oper = "PERLND" Then
                    lTable = lOperation.Tables.Item("GEN-INFO")
                    lTable.Parms("LSID").Value = pLandUses(j).Name
                    lTable = lOperation.Tables.Item("PWAT-PARM2")
                    If pLandUses(j).Slope > 0 Then
                        lTable.Parms("SLSUR").Value = pLandUses(j).Slope
                    Else
                        lTable.Parms("SLSUR").Value = 0.001 'must have some slope
                    End If
                    'default lsur based on slsur
                    lTable.Parms("LSUR").Value = DefaultLSURFromSLSUR(lTable.Parms("SLSUR").Value)
                    Exit Do
                Else
                    j += 1
                End If
            Loop
        Next lOperation

        For Each lOperation As HspfOperation In aUci.OpnBlks.Item("IMPLND").Ids
            Dim lTable As HspfTable = lOperation.Tables.Item("ACTIVITY")
            lTable.Parms("IWATFG").Value = 1
            j = 0
            Do While j < pLandUses.Count
                If lOperation.Id = pLandUses(j).Id And pLandUses(j).Oper = "IMPLND" Then
                    lTable = lOperation.Tables.Item("GEN-INFO")
                    lTable.Parms("LSID").Value = pLandUses(j).Name
                    lTable = lOperation.Tables.Item("IWAT-PARM2")
                    If pLandUses(j).Slope > 0 Then
                        lTable.Parms("SLSUR").Value = pLandUses(j).Slope
                    Else
                        lTable.Parms("SLSUR").Value = 0.001 'must have some slope
                    End If
                    'default lsur based on slsur
                    lTable.Parms("LSUR").Value = DefaultLSURFromSLSUR(lTable.Parms("SLSUR").Value)
                    Exit Do
                Else
                    j += 1
                End If
            Loop
        Next lOperation

        j = -1
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

    Private Sub CreateConnectionsSchematic(ByRef newUci As HspfUci)
        Dim j, i, k As Integer
        Dim lConnection As HspfConnection

        lConnection = New HspfConnection 'dummy to get entry point
        For i = 0 To pReaches.Count - 1
            For j = 0 To pLandUses.Count - 1
                'add entries for each land use to each reach
                If pReaches(i).WsId = pLandUses(j).Reach Then
                    lConnection = New HspfConnection
                    lConnection.Uci = newUci
                    lConnection.Typ = 3
                    lConnection.Source.VolName = pLandUses(j).Oper
                    lConnection.Source.VolId = pLandUses(j).Id
                    lConnection.MFact = pLandUses(j).Area
                    lConnection.Target.VolName = "RCHRES"
                    For k = 0 To pReaches.Count - 1
                        If pReaches(k).Order = i Then
                            lConnection.Target.VolId = CInt(pReaches(pReaches(k).Order).Id)
                        End If
                    Next k
                    lConnection.MassLink = pLandUses(j).Type
                    newUci.Connections.Add(lConnection)
                End If
            Next j
        Next i
        For i = 0 To pReaches.Count - 1
            'add entries for each reach to reach connection
            For j = 0 To pReaches.Count - 1
                If pReaches(pReaches(j).Order).Id = pReaches(pReaches(i).Order).DownID Then
                    lConnection = New HspfConnection
                    lConnection.Uci = newUci
                    lConnection.Typ = 3
                    lConnection.Source.VolName = "RCHRES"
                    lConnection.Source.VolId = CInt(pReaches(pReaches(i).Order).Id)
                    lConnection.MFact = 1.0#
                    lConnection.Target.VolName = "RCHRES"
                    lConnection.Target.VolId = CInt(pReaches(pReaches(j).Order).Id)
                    lConnection.MassLink = 3
                    newUci.Connections.Add(lConnection)
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
        Dim UniqueNameCount As Integer
        Dim addflag As Boolean
        Dim newOpn As HspfOperation
        Dim toperid As Integer
        Dim lOpn As HspfOperation

        For j As Integer = 2 To 1 Step -1
            UniqueNameCount = 0
            For i As Integer = 0 To pLandUses.Count - 1
                If pLandUses(i).Type = j Then
                    If UniqueNameCount = 0 Then
                        'add it
                        newOpn = New HspfOperation
                        newOpn.Uci = aUci
                        newOpn.Name = pLandName(j)
                        toperid = 101
                        pFirstSeg(j) = toperid
                        newOpn.Id = toperid
                        pLastSeg(j) = toperid
                        newOpn.Description = pLandUses(i).Name
                        aUci.OpnSeqBlock.Add(newOpn)
                        UniqueNameCount = UniqueNameCount + 1
                    Else
                        addflag = True
                        For Each lOpn In aUci.OpnSeqBlock.Opns
                            If lOpn.Description = pLandUses(i).Name And lOpn.Name = pLandName(j) Then
                                addflag = False
                                toperid = lOpn.Id
                            End If
                        Next lOpn
                        If addflag Then
                            UniqueNameCount = UniqueNameCount + 1
                            newOpn = New HspfOperation
                            newOpn.Uci = aUci
                            newOpn.Name = pLandName(j)
                            newOpn.Description = pLandUses(i).Name
                            toperid = 100 + UniqueNameCount
                            newOpn.Id = toperid
                            pLastSeg(j) = toperid
                            aUci.OpnSeqBlock.Add(newOpn)
                        End If
                    End If
                    'remember what we named this land use
                    pLandUses(i).Oper = pLandName(j)
                    pLandUses(i).Id = toperid
                End If
            Next i
        Next j
    End Sub

    Private Sub CreateOpnsForMultSegs(ByRef aUci As HspfUci)
        Dim j, i, k As Integer
        Dim lImplndNames As New atcCollection
        Dim lPerlndNames As New atcCollection

        'prescan to see how many perlnds and implnds per segment
        For i = 0 To pLandUses.Count - 1
            If pLandUses(i).Type = 2 Then
                'perlnd
                If lPerlndNames.IndexFromKey(pLandUses(i).Name) = 0 Then
                    lPerlndNames.Add(pLandUses(i).Name)
                End If
            ElseIf pLandUses(i).Type = 1 Then
                'implnd
                If lImplndNames.IndexFromKey(pLandUses(i).Name) = 0 Then
                    lImplndNames.Add(pLandUses(i).Name)
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
                        If pLandUses(i).Type = 2 Then
                            'add this perlnd oper
                            Dim lOpn As New HspfOperation
                            lOpn.Uci = aUci
                            lOpn.Name = "PERLND"
                            Dim lLandUseId As Integer = 0
                            For j = 0 To lPerlndNames.Count - 1
                                If lPerlndNames(j - 1) = pLandUses(i).Name Then
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
                            lOpn.Description = pLandUses(i).Name
                            aUci.OpnSeqBlock.Add(lOpn)
                            'remember what we named this land use
                            pLandUses(i).Oper = "PERLND"
                            pLandUses(i).Id = lOperId
                        End If
                    End If
                Next i

                'now add implnds
                For i = 0 To pLandUses.Count - 1
                    If pLandUses(i).Reach = pReaches(k).Id Then
                        If pLandUses(i).Type = 1 Then
                            'add this implnd oper
                            Dim lOpn As New HspfOperation
                            lOpn.Uci = aUci
                            lOpn.Name = "IMPLND"
                            Dim lLandUseId As Integer = 0
                            For j = 0 To lImplndNames.Count - 1
                                If lImplndNames(j - 1) = pLandUses(i).Name Then
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
                            lOpn.Description = pLandUses(i).Name
                            aUci.OpnSeqBlock.Add(lOpn)
                            'remember what we named this land use
                            pLandUses(i).Oper = "IMPLND"
                            pLandUses(i).Id = lOperId
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
        Dim newwdmid As String = ""
        Dim newdsn As Integer
        Dim stanam, lLocation, sen, Con, tstype As String
        Dim jdates(1) As Single
        Dim rload(1) As Single

        Dim lMasterPollutantList As New Collection
        If aPollutantListFileName.Length > 0 Then
            lMasterPollutantList = ReadPollutantList(aPollutantListFileName)
        Else
            lMasterPollutantList = Nothing
        End If

        'On Error Resume Next
        sen = "PT-OBS"
        For i As Integer = 0 To PollutantCount - 1
            If CDbl(FacilityReach(PollutantFacID(i))) > 0 Then
                Con = GetPollutantIDFromName(lMasterPollutantList, PollutantName(i))
                If Len(Con) = 0 Then
                    Con = UCase(Mid(PollutantName(i), 1, 8))
                End If
                stanam = FacilityName(PollutantFacID(i))
                lLocation = "RCH" & CStr(FacilityReach(PollutantFacID(i)))
                tstype = UCase(Mid(PollutantName(i), 1, 4))
                rload(1) = PollutantLoad(i)
                aUci.AddPointSourceDataSet(sen, lLocation, Con, stanam, tstype, 0, jdates, rload, newwdmid, newdsn)
            End If
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

    Private Sub setDefault(ByVal myUci As HspfUci, ByVal defUci As HspfUci)
        Dim vOpTyp As Object, loptyp As HspfOpnBlk
        Dim vOpn As Object, lOpn As HspfOperation, dOpn As HspfOperation
        Dim vTab As Object, lTab As HspfTable, dTab As HspfTable
        Dim vPar As Object
        Dim Id&

        Dim OpTyps() As Object = {"PERLND", "IMPLND", "RCHRES"}
        For Each vOpTyp In OpTyps
            If myUci.OpnBlks(vOpTyp).Count > 0 Then
                loptyp = myUci.OpnBlks(vOpTyp)
                'Debug.Print lOpTyp.Name
                For Each vOpn In loptyp.Ids
                    lOpn = vOpn
                    'Debug.Print lOpn.Description
                    Id = DefaultOpnId(lOpn, defUci)
                    If Id > 0 Then
                        dOpn = defUci.OpnBlks(lOpn.Name).OperFromID(Id)
                        If Not dOpn Is Nothing Then
                            For Each vTab In lOpn.Tables
                                lTab = vTab
                                If DefaultThisTable(loptyp.Name, lTab.Name) Then
                                    If dOpn.TableExists(lTab.Name) Then
                                        dTab = dOpn.Tables(lTab.Name)
                                        'Debug.Print lTab.Name
                                        For Each vPar In lTab.Parms
                                            If DefaultThisParameter(loptyp.Name, lTab.Name, vPar.Name) Then
                                                If vPar.Value <> vPar.Name Then
                                                    vPar.Value = dTab.Parms(vPar.Name).Value
                                                End If
                                            End If
                                        Next vPar
                                    End If
                                End If
                            Next vTab
                        End If
                    End If
                Next vOpn
            End If
        Next vOpTyp
    End Sub

    Private Function DefaultOpnId(ByVal lOpn As HspfOperation, ByVal defUci As HspfUci) As Long
        Dim dOpn As HspfOperation
        If lOpn.DefOpnId <> 0 Then
            DefaultOpnId = lOpn.DefOpnId
        Else
            dOpn = matchOperWithDefault(lOpn.Name, lOpn.Description, defUci)
            If dOpn Is Nothing Then
                DefaultOpnId = 0
            Else
                DefaultOpnId = dOpn.Id
            End If
        End If
    End Function

    Private Function DefaultThisTable(ByVal OperName$, ByVal TableName$) As Boolean
        If OperName = "PERLND" Or OperName = "IMPLND" Then
            If TableName = "ACTIVITY" Or _
               TableName = "PRINT-INFO" Or _
               TableName = "GEN-INFO" Or _
               TableName = "PWAT-PARM5" Then
                DefaultThisTable = False
            ElseIf Left(TableName, 4) = "QUAL" Then
                DefaultThisTable = False
            Else
                DefaultThisTable = True
            End If
        ElseIf OperName = "RCHRES" Then
            If TableName = "ACTIVITY" Or _
               TableName = "PRINT-INFO" Or _
               TableName = "GEN-INFO" Or _
               TableName = "HYDR-PARM1" Then
                DefaultThisTable = False
            ElseIf Left(TableName, 3) = "GQ-" Then
                DefaultThisTable = False
            Else
                DefaultThisTable = True
            End If
        Else
            DefaultThisTable = False
        End If
    End Function

    Private Function DefaultThisParameter(ByVal OperName$, ByVal TableName$, ByVal parmname$) As Boolean
        DefaultThisParameter = True
        If OperName = "PERLND" Then
            If TableName = "PWAT-PARM2" Then
                If parmname = "SLSUR" Or parmname = "LSUR" Then
                    DefaultThisParameter = False
                End If
            ElseIf TableName = "NQUALS" Then
                If parmname = "NQUAL" Then
                    DefaultThisParameter = False
                End If
            End If
        ElseIf OperName = "IMPLND" Then
            If TableName = "IWAT-PARM2" Then
                If parmname = "SLSUR" Or parmname = "LSUR" Then
                    DefaultThisParameter = False
                End If
            ElseIf TableName = "NQUALS" Then
                If parmname = "NQUAL" Then
                    DefaultThisParameter = False
                End If
            End If
        ElseIf OperName = "RCHRES" Then
            If TableName = "HYDR-PARM2" Then
                If parmname = "LEN" Or _
                   parmname = "DELTH" Or _
                   parmname = "FTBUCI" Then
                    DefaultThisParameter = False
                End If
            ElseIf TableName = "GQ-GENDATA" Then
                If parmname = "NGQUAL" Then
                    DefaultThisParameter = False
                End If
            End If
        End If
    End Function

    Private Function matchOperWithDefault(ByVal OpTypName$, ByVal OpnDesc$, ByVal defUci As HspfUci) As HspfOperation
        Dim vOpn As Object, lOpn As HspfOperation, ctemp$

        For Each vOpn In defUci.OpnBlks(OpTypName).Ids
            lOpn = vOpn
            If lOpn.Description = OpnDesc Then
                matchOperWithDefault = lOpn
                Exit Function
            End If
        Next vOpn
        'a complete match not found, look for partial
        For Each vOpn In defUci.OpnBlks(OpTypName).Ids
            lOpn = vOpn
            If Len(lOpn.Description) > Len(OpnDesc) Then
                ctemp = Left(lOpn.Description, Len(OpnDesc))
                If ctemp = OpnDesc Then
                    matchOperWithDefault = lOpn
                    Exit Function
                End If
            ElseIf Len(lOpn.Description) < Len(OpnDesc) Then
                ctemp = Left(OpnDesc, Len(lOpn.Description))
                If lOpn.Description = ctemp Then
                    matchOperWithDefault = lOpn
                    Exit Function
                End If
            End If
            If Len(OpnDesc) > 4 And Len(lOpn.Description) > 4 Then
                ctemp = Left(OpnDesc, 4)
                If Left(lOpn.Description, 4) = ctemp Then
                    matchOperWithDefault = lOpn
                    Exit Function
                End If
            End If
        Next vOpn
        'not found, use first one
        If defUci.OpnBlks(OpTypName).Count > 0 Then
            matchOperWithDefault = defUci.OpnBlks(OpTypName).Ids(0)
        Else
            matchOperWithDefault = Nothing
        End If
    End Function

    Private Sub setDefaultML(ByVal aUci As HspfUci, ByVal aDefUci As HspfUci)
        Dim vML As Object, dML As HspfMassLink
        Dim lMassLink As HspfMassLink

        For Each vML In aDefUci.MassLinks
            dML = vML
            If dML.Source.VolName = "PERLND" And dML.Source.Member = "PERO" Then
            ElseIf dML.Source.VolName = "IMPLND" And dML.Source.Member = "SURO" Then
            ElseIf dML.Source.VolName = "RCHRES" And dML.Source.Group = "ROFLOW" Then
            Else
                'add the other ones
                lMassLink = New HspfMassLink
                lMassLink = dML
                aUci.MassLinks.Add(lMassLink)
            End If
        Next vML

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