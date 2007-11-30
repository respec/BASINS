'Option Strict Off
'Option Explicit On

'Imports atcUtility

Module modCreateUci
    '    'Copyright 2006 AQUA TERRA Consultants - Royalty-free use permitted under open source license

    '	'land use file
    '	Dim LURecCnt As Integer
    '	Dim LUName() As String
    '	Dim LUType() As Integer
    '	Dim LUReach() As String
    '	Dim LUArea() As Single
    '	Dim LUSlope() As Single
    '	Dim LUDistance() As Single
    '	Dim luoper() As String
    '	Dim luid() As Integer
    '	'reach file
    '	Dim RCHRecCnt As Integer
    '	Dim RCHId() As String
    '	Dim RCHName() As String
    '	Dim RCHWsid() As String
    '	Dim RCHNexits() As Integer
    '	Dim RCHType() As String
    '	Dim RCHLength() As Single
    '	Dim RCHDelth() As Single
    '	Dim RCHElev() As Single
    '	Dim RCHDownID() As String
    '	Dim RCHDepth() As Single
    '	Dim RCHWidth() As Single
    '	Dim RCHMann() As Single
    '	Dim rchorder() As Integer
    '	'channel file
    '	Dim chanid() As String
    '	Dim ChanL() As Single
    '	Dim ChanYm() As Single
    '	Dim ChanWm() As Single
    '	Dim ChanN() As Single
    '	Dim ChanS() As Single
    '	Dim ChanM11() As Single
    '	Dim ChanM12() As Single
    '	Dim ChanYc() As Single
    '	Dim ChanM21() As Single
    '	Dim ChanM22() As Single
    '	Dim ChanYt1() As Single
    '	Dim ChanYt2() As Single
    '	Dim ChanM31() As Single
    '	Dim ChanM32() As Single
    '	Dim ChanW11() As Single
    '	Dim ChanW12() As Single
    '	Dim ChanRecCnt As Integer

    '	Dim mGlobalBlock As HspfGlobalBlk
    '	Dim mFilesBlock As HspfFilesBlk
    '	Dim mOpnSeqBlock As HspfOpnSeqBlk
    '	Dim mOpnBlks As Collection 'of hspfopnblk
    '	Dim mConnections As Collection 'of hspfconnection
    '	Dim mMassLinks As Collection 'of hspfmasslink

    '	Dim lastseg(2) As Integer
    '	Dim landname(2) As String
    '	Dim firstseg(2) As Integer

    '	Dim FacilityCount As Integer
    '	Dim FacilityName() As String
    '	Dim FacilityNpdes() As String
    '	Dim FacilityReach() As String
    '	Dim FacilityMile() As Single

    '	Dim PollutantCount As Integer
    '	Dim PollutantFacID() As Integer
    '	Dim PollutantName() As String
    '	Dim PollutantLoad() As Single

    '	Public Sub CreateUciFromBASINS(ByRef newUci As HspfUci, ByRef M As HspfMsg, ByRef newName As String, ByRef outputwdm As String, ByRef metwdms() As String, ByRef wdmids() As String, ByRef MetDataDetails As String, ByRef oneseg As Boolean, ByRef MasterPollutantList As Collection)

    '		Dim s As String
    '		Dim basedsn, ret, i As Integer
    '		Dim delim, quote As String
    '		Dim lOpnName As String
    '		Dim lopnblk As HspfOpnBlk
    '        Dim lOpn As HspfOperation
    '        Dim SDate(6) As Integer
    '		Dim EDate(6) As Integer
    '		Dim metwdmid As String

    '        If Not IO.File.Exists(newName) Then
    '            newUci.ErrorDescription = "WsdFileName '" & newName & "' not found"
    '        Else
    '            newUci.Name = Left(newName, Len(newName) - 3) & "uci"
    '        End If

    '		'newUci.SetMessageUnit
    '		newUci.Msg = M

    '		If Len(newUci.Name) > 0 Then

    '			s = FilenameOnly((newUci.Name))

    '			Call ReadWSDFile(newName, ret)
    '			Call ReadRCHFile(newName, ret)
    '			Call ReadPTFFile(newName, ret)
    '			Call ReadPSRFile(newName, ret)
    '			On Error Resume Next

    '			If ret = 0 Then
    '				'everything read okay, continue

    '				'get details from the met data details
    '				delim = ","
    '				quote = """"
    '				basedsn = CInt(StrSplit(MetDataDetails, delim, quote))
    '				For i = 0 To 5
    '					SDate(i) = CInt(StrSplit(MetDataDetails, delim, quote))
    '				Next i
    '				For i = 0 To 5
    '					EDate(i) = CInt(StrSplit(MetDataDetails, delim, quote))
    '				Next i
    '				metwdmid = MetDataDetails

    '				'add global block to empty uci
    '				newUci.Initialized = True
    '				mGlobalBlock = New HspfGlobalBlk
    '				mGlobalBlock.Uci = newUci
    '				mGlobalBlock.RunInf.Value = "UCI Created by WinHSPF for " & s
    '				mGlobalBlock.emfg = 1
    '				mGlobalBlock.outlev.Value = CStr(1)
    '				mGlobalBlock.runfg = 1
    '				For i = 0 To 5
    '					mGlobalBlock.SDate(i) = SDate(i)
    '					mGlobalBlock.EDate(i) = EDate(i)
    '				Next i
    '				newUci.GlobalBlock = mGlobalBlock

    '				'add files block to uci
    '				mFilesBlock = New HspfFilesBlk
    '				CreateFilesBlock(s, outputwdm, metwdms, wdmids)
    '				mFilesBlock.Uci = newUci
    '				newUci.FilesBlock = mFilesBlock

    '				'add opn seq block
    '				mOpnSeqBlock = New HspfOpnSeqBlk
    '				mOpnSeqBlock.Uci = newUci
    '				mOpnSeqBlock.Delt = 60
    '				'add recs for each operation
    '				CreateOpnSeqBlock(newUci, oneseg)
    '				mOpnSeqBlock.Uci = newUci
    '				newUci.OpnSeqBlock = mOpnSeqBlock

    '				'set all operation types
    '				mOpnBlks = New Collection 'of hspfopnblk
    '				i = 1
    '				lOpnName = HspfOperName(i)
    '				While lOpnName <> "UNKNOWN"
    '					lopnblk = New HspfOpnBlk
    '					lopnblk.Name = lOpnName
    '					lopnblk.Uci = newUci
    '					mOpnBlks.Add(lopnblk, lOpnName)
    '					i = i + 1
    '					lOpnName = HspfOperName(i)
    '				End While
    '				newUci.OpnBlks = mOpnBlks

    '				'create tables for each operation
    '                For Each lOpn In mOpnSeqBlock.Opns
    '                    lopnblk = mOpnBlks.Item(lOpn.Name)
    '                    lopnblk.Ids.Add(lOpn, "K" & lOpn.Id)
    '                    lOpn.OpnBlk = lopnblk
    '                Next
    '                For Each lopnblk In mOpnBlks 'perlnd, implnd, etc
    '                    If lopnblk.Count > 0 Then Call lopnblk.createTables(M.BlockDefs.Item(lopnblk.Name))
    '                Next

    '				For i = 1 To ChanRecCnt 'process ftables
    '                    lOpn = newUci.OpnBlks.Item("RCHRES").OperFromID(CShort(chanid(i)))
    '					If Not lOpn Is Nothing Then
    '						lOpn.FTable.FTableFromCrossSect(ChanL(i), ChanYm(i), ChanWm(i), ChanN(i), ChanS(i), ChanM11(i), ChanM12(i), ChanYc(i), ChanM21(i), ChanM22(i), ChanYt1(i), ChanYt2(i), ChanM31(i), ChanM32(i), ChanW11(i), ChanW12(i))
    '					End If
    '				Next i

    '				mConnections = New Collection 'of hspfconnections
    '				'create schematic, ext src blocks
    '				CreateConnectionsSchematic(newUci)
    '				CreateConnectionsMet(newUci)
    '				newUci.Connections = mConnections

    '				'set timeser connections
    '                For Each lOpn In mOpnSeqBlock.Opns
    '                    lOpn.setTimSerConnections()
    '                Next
    '				'create masslinks
    '				mMassLinks = New Collection 'of hspfmasslinks
    '				CreateMassLinks(newUci)
    '				newUci.MassLinks = mMassLinks

    '				'set initial values in uci from basins values
    '				SetInitValues()

    '				CreatePointSourceDSNs(newUci, MasterPollutantList)

    '				CreateDefaultOutput(newUci)
    '				CreateBinaryOutput(newUci, s)

    '				'look for met segments
    '				'newUci.Source2MetSeg
    '			End If
    '		End If

    '		newUci.Edited = False 'all the reads set edited

    '	End Sub

    '	Private Sub ReadWSDFile(ByRef newName As String, ByRef ret As Integer)
    '		Dim tname, quote, delim, lstr, tstr As String
    '		Dim amax, i, tcnt As Integer

    '		ret = 0
    '		delim = " "
    '		quote = """"

    '		'read wsd file
    '		i = FreeFile()
    '		On Error GoTo ErrHandler
    '		tname = Left(newName, Len(newName) - 3) & "wsd"
    '		FileOpen(i, tname, OpenMode.Input)
    '		lstr = LineInput(i) 'header line
    '		LURecCnt = 0
    '		ReDim LUName(1)
    '		ReDim LUType(1)
    '		ReDim LUReach(1)
    '		ReDim LUArea(1)
    '		ReDim LUSlope(1)
    '		ReDim LUDistance(1)
    '		ReDim luoper(1)
    '		ReDim luid(1)
    '		Do Until EOF(i)
    '			lstr = LineInput(i)
    '			LURecCnt = LURecCnt + 1
    '			amax = UBound(LUName)
    '			If LURecCnt > amax Then
    '				ReDim Preserve LUName(amax * 2)
    '				ReDim Preserve LUType(amax * 2)
    '				ReDim Preserve LUReach(amax * 2)
    '				ReDim Preserve LUArea(amax * 2)
    '				ReDim Preserve LUSlope(amax * 2)
    '				ReDim Preserve LUDistance(amax * 2)
    '				ReDim Preserve luoper(amax * 2)
    '				ReDim Preserve luid(amax * 2)
    '			End If
    '			'count the number of fields in this string
    '			tstr = lstr
    '			tcnt = 0
    '			Do While Len(StrSplit(tstr, delim, quote)) > 0
    '				tcnt = tcnt + 1
    '			Loop 
    '			If tcnt = 6 Then
    '				'this is the normal way
    '				LUName(LURecCnt) = StrSplit(lstr, delim, quote)
    '				LUType(LURecCnt) = CInt(StrSplit(lstr, delim, quote))
    '				LUReach(LURecCnt) = StrSplit(lstr, delim, quote)
    '				LUArea(LURecCnt) = CSng(StrSplit(lstr, delim, quote))
    '				LUSlope(LURecCnt) = CSng(StrSplit(lstr, delim, quote))
    '				LUDistance(LURecCnt) = CSng(StrSplit(lstr, delim, quote))
    '			Else
    '				'if coming from old delineator might not be space delimited
    '				LUName(LURecCnt) = StrSplit(lstr, delim, quote)
    '				If lStr.Length > 23 Then
    '					LUDistance(LURecCnt) = CSng(Mid(lstr, lStr.Length - 7, 8))
    '					LUSlope(LURecCnt) = CSng(Mid(lstr, lStr.Length - 15, 8))
    '					LUArea(LURecCnt) = CSng(Mid(lstr, lStr.Length - 23, 8))
    '				End If
    '				LUType(LURecCnt) = CInt(StrSplit(lstr, delim, quote))
    '				LUReach(LURecCnt) = StrSplit(lstr, delim, quote)
    '			End If
    '		Loop 
    '		FileClose(i)
    '		Exit Sub
    'ErrHandler: 
    '		Call MsgBox("Problem reading file " & tname,  , "Create Problem")
    '		ret = 1
    '	End Sub

    '	Private Sub ReadRCHFile(ByRef newName As String, ByRef ret As Integer)
    '		Dim tname, quote, delim, lstr, tstr As String
    '		Dim i, amax As Integer

    '		ret = 0
    '		delim = " "
    '		quote = """"

    '		'read rch file
    '		i = FreeFile()
    '		On Error GoTo ErrHandler
    '		tname = Left(newName, Len(newName) - 3) & "rch"
    '		FileOpen(i, tname, OpenMode.Input)
    '		lstr = LineInput(i) 'header line
    '		RCHRecCnt = 0
    '		ReDim RCHId(1)
    '		ReDim RCHName(1)
    '		ReDim RCHWsid(1)
    '		ReDim RCHNexits(1)
    '		ReDim RCHType(1)
    '		ReDim RCHLength(1)
    '		ReDim RCHDelth(1)
    '		ReDim RCHElev(1)
    '		ReDim RCHDownID(1)
    '		ReDim RCHDepth(1)
    '		ReDim RCHWidth(1)
    '		ReDim RCHMann(1)
    '		ReDim rchorder(1)
    '		Do Until EOF(i)
    '			lstr = LineInput(i)
    '			RCHRecCnt = RCHRecCnt + 1
    '			amax = UBound(RCHId)
    '			If RCHRecCnt > amax Then
    '				ReDim Preserve RCHId(amax * 2)
    '				ReDim Preserve RCHName(amax * 2)
    '				ReDim Preserve RCHWsid(amax * 2)
    '				ReDim Preserve RCHNexits(amax * 2)
    '				ReDim Preserve RCHType(amax * 2)
    '				ReDim Preserve RCHLength(amax * 2)
    '				ReDim Preserve RCHDelth(amax * 2)
    '				ReDim Preserve RCHElev(amax * 2)
    '				ReDim Preserve RCHDownID(amax * 2)
    '				ReDim Preserve RCHDepth(amax * 2)
    '				ReDim Preserve RCHWidth(amax * 2)
    '				ReDim Preserve RCHMann(amax * 2)
    '				ReDim Preserve rchorder(amax * 2)
    '			End If
    '			RCHId(RCHRecCnt) = StrSplit(lstr, delim, quote)
    '			RCHName(RCHRecCnt) = StrSplit(lstr, delim, quote)
    '			RCHWsid(RCHRecCnt) = StrSplit(lstr, delim, quote)
    '			tstr = StrSplit(lstr, delim, quote)
    '			RCHNexits(RCHRecCnt) = CInt(StrSplit(lstr, delim, quote))
    '			tstr = StrSplit(lstr, delim, quote)
    '			RCHType(RCHRecCnt) = StrSplit(lstr, delim, quote)
    '			RCHLength(RCHRecCnt) = CSng(StrSplit(lstr, delim, quote))
    '			RCHDelth(RCHRecCnt) = CSng(StrSplit(lstr, delim, quote))
    '			RCHElev(RCHRecCnt) = CSng(StrSplit(lstr, delim, quote))
    '			tstr = StrSplit(lstr, delim, quote)
    '			tstr = StrSplit(lstr, delim, quote)
    '			RCHDownID(RCHRecCnt) = StrSplit(lstr, delim, quote)
    '			tstr = StrSplit(lstr, delim, quote)
    '			tstr = StrSplit(lstr, delim, quote)
    '			tstr = StrSplit(lstr, delim, quote)
    '			tstr = StrSplit(lstr, delim, quote)
    '			tstr = StrSplit(lstr, delim, quote)
    '			tstr = StrSplit(lstr, delim, quote)
    '			RCHDepth(RCHRecCnt) = CSng(StrSplit(lstr, delim, quote))
    '			RCHWidth(RCHRecCnt) = CSng(StrSplit(lstr, delim, quote))
    '			tstr = StrSplit(lstr, delim, quote)
    '			tstr = StrSplit(lstr, delim, quote)
    '			tstr = StrSplit(lstr, delim, quote)
    '			tstr = StrSplit(lstr, delim, quote)
    '			tstr = StrSplit(lstr, delim, quote)
    '			tstr = StrSplit(lstr, delim, quote)
    '			RCHMann(RCHRecCnt) = CSng(StrSplit(lstr, delim, quote))
    '			rchorder(RCHRecCnt) = RCHRecCnt
    '		Loop 
    '		FileClose(i)
    '		Exit Sub
    'ErrHandler: 
    '		Call MsgBox("Problem reading file " & tname,  , "Create Problem")
    '		ret = 2
    '	End Sub

    '	Public Sub ReadPTFFile(ByRef newName As String, ByRef ret As Integer)

    '		Dim tname, quote, delim, lstr, tstr As String
    '		Dim i, amax As Integer

    '		ret = 0
    '		delim = " "
    '		quote = """"

    '		'read ptf file for channel data
    '		i = FreeFile()
    '		On Error GoTo ErrHandler
    '		tname = Left(newName, Len(newName) - 3) & "ptf"
    '		FileOpen(i, tname, OpenMode.Input)
    '		lstr = LineInput(i) 'header line
    '		ChanRecCnt = 0
    '		ReDim chanid(1)
    '		ReDim ChanL(1)
    '		ReDim ChanYm(1)
    '		ReDim ChanWm(1)
    '		ReDim ChanN(1)
    '		ReDim ChanS(1)
    '		ReDim ChanM11(1)
    '		ReDim ChanM12(1)
    '		ReDim ChanYc(1)
    '		ReDim ChanM21(1)
    '		ReDim ChanM22(1)
    '		ReDim ChanYt1(1)
    '		ReDim ChanYt2(1)
    '		ReDim ChanM31(1)
    '		ReDim ChanM32(1)
    '		ReDim ChanW11(1)
    '		ReDim ChanW12(1)
    '		Do Until EOF(i)
    '			lstr = LineInput(i)
    '			ChanRecCnt = ChanRecCnt + 1
    '			amax = UBound(chanid)
    '			If ChanRecCnt > amax Then
    '				ReDim Preserve chanid(amax * 2)
    '				ReDim Preserve ChanL(amax * 2)
    '				ReDim Preserve ChanYm(amax * 2)
    '				ReDim Preserve ChanWm(amax * 2)
    '				ReDim Preserve ChanN(amax * 2)
    '				ReDim Preserve ChanS(amax * 2)
    '				ReDim Preserve ChanM11(amax * 2)
    '				ReDim Preserve ChanM12(amax * 2)
    '				ReDim Preserve ChanYc(amax * 2)
    '				ReDim Preserve ChanM21(amax * 2)
    '				ReDim Preserve ChanM22(amax * 2)
    '				ReDim Preserve ChanYt1(amax * 2)
    '				ReDim Preserve ChanYt2(amax * 2)
    '				ReDim Preserve ChanM31(amax * 2)
    '				ReDim Preserve ChanM32(amax * 2)
    '				ReDim Preserve ChanW11(amax * 2)
    '				ReDim Preserve ChanW12(amax * 2)
    '			End If
    '			chanid(ChanRecCnt) = StrSplit(lstr, delim, quote) 'reach id
    '			ChanL(ChanRecCnt) = CSng(StrSplit(lstr, delim, quote)) 'length
    '			ChanYm(ChanRecCnt) = CSng(StrSplit(lstr, delim, quote)) 'mean depth
    '			ChanWm(ChanRecCnt) = CSng(StrSplit(lstr, delim, quote)) 'mean width
    '			ChanN(ChanRecCnt) = CSng(StrSplit(lstr, delim, quote)) 'mann n
    '			ChanS(ChanRecCnt) = CSng(StrSplit(lstr, delim, quote)) 'long slope
    '			If ChanS(ChanRecCnt) < 0.0001 Then
    '				ChanS(ChanRecCnt) = 0.0001
    '			End If
    '			tstr = StrSplit(lstr, delim, quote)
    '			ChanM31(ChanRecCnt) = CSng(StrSplit(lstr, delim, quote)) 'side slope upper left
    '			ChanM21(ChanRecCnt) = CSng(StrSplit(lstr, delim, quote)) 'side slope lower left
    '			ChanW11(ChanRecCnt) = CSng(StrSplit(lstr, delim, quote)) 'zero slope width left
    '			ChanM11(ChanRecCnt) = CSng(StrSplit(lstr, delim, quote)) 'side slope chan left
    '			ChanM12(ChanRecCnt) = CSng(StrSplit(lstr, delim, quote)) 'side slope chan right
    '			ChanW12(ChanRecCnt) = CSng(StrSplit(lstr, delim, quote)) 'zero slope width right
    '			ChanM22(ChanRecCnt) = CSng(StrSplit(lstr, delim, quote)) 'side slope lower right
    '			ChanM32(ChanRecCnt) = CSng(StrSplit(lstr, delim, quote)) 'side slope upper right
    '			ChanYc(ChanRecCnt) = CSng(StrSplit(lstr, delim, quote)) 'channel depth
    '			ChanYt1(ChanRecCnt) = CSng(StrSplit(lstr, delim, quote)) 'depth at slope change
    '			ChanYt2(ChanRecCnt) = CSng(StrSplit(lstr, delim, quote)) 'channel max depth
    '		Loop 
    '		FileClose(i)
    '		Exit Sub
    'ErrHandler: 
    '		Call MsgBox("Problem reading file " & tname,  , "Create Problem")
    '		ret = 3
    '	End Sub

    '	Public Sub WritePTFFile(ByRef newName As String, ByRef chanid As String, ByRef ArrayVals() As Single)

    '		Dim tstr, lstr, delim, quote, tname, Id As String
    '		Dim i, amax As Integer

    '		i = Len(chanid)
    '		If i > 0 Then 'have a reach id
    '			'write ptf file
    '			i = FreeFile()
    '			On Error GoTo ErrHandler
    '			tname = newName
    '			FileOpen(i, tname, OpenMode.Output)
    '			lstr = "'Reach Number','Length(ft)','Mean Depth(ft)','Mean Width (ft)'," & "'Mannings Roughness Coeff.','Long. Slope','Type of x-section','Side slope of upper FP left'," & "'Side slope of lower FP left','Zero slope FP width left(ft)','Side slope of channel left'," & "'Side slope of channel right','Zero slope FP width right(ft)','Side slope lower FP right'," & "'Side slope upper FP right','Channel Depth(ft)','Flood side slope change at depth','Max. depth'," & "'No. of exits','Fraction of flow through exit 1','Fraction of flow through exit 2'," & "'Fraction of flow through exit 3','Fraction of flow through exit 4','Fraction of flow through exit 5'"
    '			PrintLine(i, lstr) 'header line
    '			lstr = chanid & " " & ArrayVals(1) & " " & ArrayVals(2) & " " & ArrayVals(3) & " " & ArrayVals(4) & " " & ArrayVals(5) & " " & "Trapezoidal" & " " & ArrayVals(13) & " " & ArrayVals(12) & " " & ArrayVals(11) & " " & ArrayVals(10) & " " & ArrayVals(9) & " " & ArrayVals(8) & " " & ArrayVals(7) & " " & ArrayVals(6) & " " & ArrayVals(14) & " " & ArrayVals(15) & " " & ArrayVals(16) & " " & "1 1 0 0 0 0"
    '			PrintLine(i, lstr)
    '			FileClose(i)
    '			Exit Sub
    '		End If
    'ErrHandler: 
    '	End Sub

    '	Public Sub GetPTFFileIds(ByRef cnt As Integer, ByRef ArrayIds() As String)
    '		Dim j As Integer

    '		cnt = ChanRecCnt
    '		ReDim ArrayIds(cnt)
    '		For j = 1 To cnt
    '			ArrayIds(j) = chanid(j)
    '		Next j
    '	End Sub

    '	Public Sub GetPTFData(ByRef RCHId As String, ByRef ArrayVals() As Single)
    '		Dim i As Integer
    '		Dim Id As String

    '		i = Len(RCHId)
    '		If i > 0 Then 'have a reach id
    '			Id = CStr(CShort(RCHId))
    '			For i = 1 To ChanRecCnt
    '				If Trim(chanid(i)) = Id Then 'found the one
    '					ArrayVals(1) = ChanL(i)
    '					ArrayVals(2) = ChanYm(i)
    '					ArrayVals(3) = ChanWm(i)
    '					ArrayVals(4) = ChanN(i)
    '					ArrayVals(5) = ChanS(i)
    '					ArrayVals(6) = ChanM32(i)
    '					ArrayVals(7) = ChanM22(i)
    '					ArrayVals(8) = ChanW12(i)
    '					ArrayVals(9) = ChanM12(i)
    '					ArrayVals(10) = ChanM11(i)
    '					ArrayVals(11) = ChanW11(i)
    '					ArrayVals(12) = ChanM21(i)
    '					ArrayVals(13) = ChanM31(i)
    '					ArrayVals(14) = ChanYc(i)
    '					ArrayVals(15) = ChanYt1(i)
    '					ArrayVals(16) = ChanYt2(i)
    '				End If
    '			Next i
    '		End If
    '	End Sub

    '	Private Sub ReadPSRFile(ByRef newName As String, ByRef ret As Integer)

    '		Dim tname, quote, delim, lstr, tstr As String
    '		Dim amax, i, j As Integer

    '		ret = 0
    '		delim = " "
    '		quote = """"
    '		FacilityCount = 0
    '		PollutantCount = 0

    '		'read psr file for point source data
    '		i = FreeFile()
    '		On Error GoTo ErrHandler
    '		tname = Left(newName, Len(newName) - 3) & "psr"
    '		FileOpen(i, tname, OpenMode.Input)
    '		lstr = LineInput(i) 'number of facilities
    '		If lStr.Length > 0 Then
    '			FacilityCount = CShort(lstr)
    '		End If
    '		lstr = LineInput(i) 'blank line
    '		lstr = LineInput(i) 'header line

    '		If FacilityCount > 0 Then
    '			'have some facilities
    '			ReDim FacilityName(FacilityCount)
    '			ReDim FacilityNpdes(FacilityCount)
    '			ReDim FacilityReach(FacilityCount)
    '			ReDim FacilityMile(FacilityCount)
    '			For j = 1 To FacilityCount
    '				lstr = LineInput(i)
    '				FacilityName(j - 1) = StrSplit(lstr, delim, quote)
    '				FacilityNpdes(j - 1) = StrSplit(lstr, delim, quote)
    '				FacilityReach(j - 1) = StrSplit(lstr, delim, quote)
    '				FacilityMile(j - 1) = CSng(StrSplit(lstr, delim, quote))
    '			Next j
    '			If Not EOF(i) Then lstr = LineInput(i) 'blank line
    '			If Not EOF(i) Then lstr = LineInput(i) 'header line
    '			PollutantCount = 0
    '			Do Until EOF(i)
    '				lstr = LineInput(i)
    '				If lStr.Length > 0 Then
    '					PollutantCount = PollutantCount + 1
    '					ReDim Preserve PollutantFacID(PollutantCount)
    '					ReDim Preserve PollutantName(PollutantCount)
    '					ReDim Preserve PollutantLoad(PollutantCount)
    '					PollutantFacID(PollutantCount - 1) = CInt(StrSplit(lstr, delim, quote))
    '					PollutantName(PollutantCount - 1) = StrSplit(lstr, delim, quote)
    '					PollutantLoad(PollutantCount - 1) = CSng(StrSplit(lstr, delim, quote))
    '				End If
    '			Loop 
    '		End If
    '		FileClose(i)
    '		Exit Sub
    'ErrHandler: 
    '		Call MsgBox("Problem reading file " & tname,  , "Create Problem")
    '		ret = 4
    '	End Sub

    '	Private Sub SetInitValues()
    '		Dim i, j As Integer
    '		Dim tstr As String
    '		Dim lOpn As HspfOperation
    '		Dim ltable As HspfTable

    '		'set init values in uci
    '        For i = 1 To mOpnBlks.Item("PERLND").Count
    '            lOpn = mOpnBlks.Item("PERLND").Ids(i)
    '            ltable = lOpn.Tables.Item("ACTIVITY")
    '            ltable.Parms.Add(1, "PWATFG")
    '            j = 0
    '            Do Until lOpn.Id = luid(j) And luoper(j) = "PERLND"
    '                j = j + 1
    '                If lOpn.Id = luid(j) And luoper(j) = "PERLND" Then
    '                    ltable = lOpn.Tables.Item("GEN-INFO")
    '                    ltable.Parms.Add(LUName(j), "LSID")
    '                    ltable = lOpn.Tables.Item("PWAT-PARM2")
    '                    If LUSlope(j) > 0 Then
    '                        ltable.Parms.Add(LUSlope(j), "SLSUR")
    '                    Else
    '                        ltable.Parms.Add(0.001, "SLSUR") 'must have some slope
    '                    End If
    '                    'default lsur based on slsur
    '                    ltable.Parms.Add(DefaultLSURFromSLSUR(ltable.Parms("SLSUR")), "LSUR")
    '                End If
    '            Loop
    '        Next i
    '        For i = 1 To mOpnBlks.Item("IMPLND").Count
    '            lOpn = mOpnBlks.Item("IMPLND").Ids(i)
    '            ltable = lOpn.Tables.Item("ACTIVITY")
    '            ltable.Parms.Add(1, "IWATFG")
    '            j = 0
    '            Do Until lOpn.Id = luid(j) And luoper(j) = "IMPLND"
    '                j = j + 1
    '                If lOpn.Id = luid(j) And luoper(j) = "IMPLND" Then
    '                    ltable = lOpn.Tables.Item("GEN-INFO")
    '                    ltable.Parms.Add(LUName(j), "LSID")
    '                    ltable = lOpn.Tables.Item("IWAT-PARM2")
    '                    If LUSlope(j) > 0 Then
    '                        ltable.Parms.Add(LUSlope(j), "SLSUR")
    '                    Else
    '                        ltable.Parms.Add(0.001, "SLSUR") 'must have some slope
    '                    End If
    '                    'default lsur based on slsur
    '                    ltable.Parms.Add(DefaultLSURFromSLSUR(ltable.Parms("SLSUR")), "LSUR")
    '                End If
    '            Loop
    '        Next i
    '		For i = 1 To RCHRecCnt
    '            lOpn = mOpnBlks.Item("RCHRES").Ids(i)
    '			ltable = lOpn.Tables.Item("ACTIVITY")
    '            ltable.Parms.Add(1, "HYDRFG")
    '			ltable = lOpn.Tables.Item("GEN-INFO")
    '			tstr = RCHName(rchorder(i))
    '			j = Len(tstr)
    '			If j < 19 And (Not IsNumeric(RCHId(rchorder(i))) Or Len(RCHId(rchorder(i))) > 5) Then
    '				tstr = tstr & " " & Right(RCHId(rchorder(i)), 19 - j)
    '			End If
    '            ltable.Parms.Add(tstr, "RCHID")
    '            ltable.Parms.Add(RCHNexits(rchorder(i)), "NEXITS")
    '            ltable.Parms.Add(91, "PUNITE")
    '			If RCHType(rchorder(i)) = "R" Then
    '                ltable.Parms.Add(1, "LKFG")
    '			End If
    '			ltable = lOpn.Tables.Item("HYDR-PARM1")
    '            ltable.Parms.Add(1, "AUX1FG")
    '            ltable.Parms.Add(1, "AUX2FG")
    '            ltable.Parms.Add(1, "AUX3FG")
    '            ltable.Parms.Add(4, "ODFVF1")
    '			ltable = lOpn.Tables.Item("HYDR-PARM2")
    '            ltable.Parms.Add(RCHLength(rchorder(i)), "LEN")
    '            ltable.Parms.Add(System.Math.Round(RCHDelth(rchorder(i)), 0), "DELTH")
    '		Next i
    '	End Sub

    '	Private Sub CreateMassLinks(ByRef newUci As HspfUci)
    '		Dim lMassLink As HspfMassLink
    '		Dim lopnblk As HspfOpnBlk

    '		For	Each lopnblk In mOpnBlks
    '			If lopnblk.Name = "PERLND" Then
    '				lMassLink = New HspfMassLink
    '				lMassLink.Uci = newUci
    '				lMassLink.MassLinkID = 2
    '				lMassLink.Source.VolName = "PERLND"
    '				lMassLink.Source.VolId = 0
    '				lMassLink.Source.Group = "PWATER"
    '				lMassLink.Source.Member = "PERO"
    '				lMassLink.MFact = 0.0833333
    '				lMassLink.Tran = ""
    '				lMassLink.Target.VolName = "RCHRES"
    '				lMassLink.Target.VolId = 0
    '				lMassLink.Target.Group = "INFLOW"
    '				lMassLink.Target.Member = "IVOL"
    '				mMassLinks.Add(lMassLink)

    '			ElseIf lopnblk.Name = "IMPLND" Then 
    '				lMassLink = New HspfMassLink
    '				lMassLink.Uci = newUci
    '				lMassLink.MassLinkID = 1
    '				lMassLink.Source.VolName = "IMPLND"
    '				lMassLink.Source.VolId = 0
    '				lMassLink.Source.Group = "IWATER"
    '				lMassLink.Source.Member = "SURO"
    '				lMassLink.MFact = 0.0833333
    '				lMassLink.Tran = ""
    '				lMassLink.Target.VolName = "RCHRES"
    '				lMassLink.Target.VolId = 0
    '				lMassLink.Target.Group = "INFLOW"
    '				lMassLink.Target.Member = "IVOL"
    '				mMassLinks.Add(lMassLink)

    '			ElseIf lopnblk.Name = "RCHRES" Then 
    '				lMassLink = New HspfMassLink
    '				lMassLink.Uci = newUci
    '				lMassLink.MassLinkID = 3
    '				lMassLink.Source.VolName = "RCHRES"
    '				lMassLink.Source.VolId = 0
    '				lMassLink.Source.Group = "ROFLOW"
    '				lMassLink.Source.Member = ""
    '				lMassLink.MFact = 1#
    '				lMassLink.Tran = ""
    '				lMassLink.Target.VolName = "RCHRES"
    '				lMassLink.Target.VolId = 0
    '				lMassLink.Target.Group = "INFLOW"
    '				lMassLink.Target.Member = ""
    '				mMassLinks.Add(lMassLink)
    '			End If
    '		Next lopnblk
    '	End Sub

    '	Private Sub CreateConnectionsSchematic(ByRef newUci As HspfUci)
    '		Dim j, i, k As Integer
    '		Dim lConnection As HspfConnection

    '		lConnection = New HspfConnection 'dummy to get entry point
    '		For i = 1 To RCHRecCnt
    '			For j = 1 To LURecCnt
    '				'add entries for each land use to each reach
    '				If RCHWsid(i) = LUReach(j) Then
    '					lConnection = New HspfConnection
    '					lConnection.Uci = newUci
    '					lConnection.Typ = 3
    '					lConnection.Source.VolName = luoper(j)
    '					lConnection.Source.VolId = luid(j)
    '					lConnection.MFact = LUArea(j)
    '					lConnection.Target.VolName = "RCHRES"
    '					For k = 1 To RCHRecCnt
    '						If rchorder(k) = i Then
    '							lConnection.Target.VolId = CInt(RCHId(rchorder(k)))
    '						End If
    '					Next k
    '					lConnection.MassLink = LUType(j)
    '					mConnections.Add(lConnection)
    '				End If
    '			Next j
    '		Next i
    '		For i = 1 To RCHRecCnt
    '			'add entries for each reach to reach connection
    '			For j = 1 To RCHRecCnt
    '				If RCHId(rchorder(j)) = RCHDownID(rchorder(i)) Then
    '					lConnection = New HspfConnection
    '					lConnection.Uci = newUci
    '					lConnection.Typ = 3
    '					lConnection.Source.VolName = "RCHRES"
    '					lConnection.Source.VolId = CInt(RCHId(rchorder(i)))
    '					lConnection.MFact = 1#
    '					lConnection.Target.VolName = "RCHRES"
    '					lConnection.Target.VolId = CInt(RCHId(rchorder(j)))
    '					lConnection.MassLink = 3
    '					mConnections.Add(lConnection)
    '				End If
    '			Next j
    '		Next i
    '		'UPGRADE_NOTE: Object lConnection may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
    '		lConnection = Nothing
    '	End Sub

    '	Private Sub CreateConnectionsMet(ByRef newUci As HspfUci)
    '        Dim lOpn As HspfOperation
    '        Dim vOpTyp As String
    '        Static lOpTypes() As String = {"PERLND", "IMPLND", "RCHRES"} 'operations with assoc met segs

    '        For Each vOpTyp In lOpTypes
    '            For Each lOpn In newUci.OpnBlks.Item(vOpTyp).Ids
    '                lOpn.MetSeg = newUci.MetSegs.Item(1)
    '            Next
    '        Next

    '	End Sub

    '	Private Sub CreateFilesBlock(ByRef s As String, ByRef outputwdm As String, ByRef metwdms() As String, ByRef wdmids() As String)
    '		Dim icnt, i As Integer
    '        Dim newFile As New HspfData.HspfFile

    '		mFilesBlock.Clear()
    '		newFile.Name = s & ".ech"
    '		newFile.Typ = "MESSU"
    '		newFile.Unit = 24
    '		mFilesBlock.Add(newFile)
    '		newFile.Name = s & ".out"
    '		newFile.Typ = " "
    '		newFile.Unit = 91
    '		mFilesBlock.Add(newFile)
    '		icnt = 0
    '		If Len(outputwdm) > 0 Then
    '			newFile.Name = outputwdm
    '			icnt = icnt + 1
    '			newFile.Typ = wdmids(0)
    '			newFile.Unit = 25
    '			mFilesBlock.Add(newFile)
    '		End If
    '		For i = 1 To 3
    '			If Len(metwdms(i)) > 0 Then
    '				newFile.Name = metwdms(i)
    '				icnt = icnt + 1
    '				newFile.Typ = wdmids(i)
    '				newFile.Unit = 25 + i
    '				mFilesBlock.Add(newFile)
    '			End If
    '		Next i
    '	End Sub
    '	Private Sub CreateOpnSeqBlock(ByRef newUci As HspfUci, ByRef oneseg As Boolean)

    '        Dim j, i, itemp As Integer
    '		Dim stringtofind As String
    '		Dim newOpn As HspfOperation
    '        Dim outoforder As Boolean

    '		If RCHRecCnt > 1 Then
    '			'reaches have to be in order
    '			outoforder = True
    '			Do Until Not outoforder
    '				outoforder = False
    '				i = 2
    '				Do Until i = RCHRecCnt + 1
    '					stringtofind = RCHDownID(rchorder(i))
    '					j = 1
    '					Do Until j = RCHRecCnt
    '						If RCHId(rchorder(j)) = stringtofind And j < i Then
    '							'reaches are out of order, swap places
    '							itemp = rchorder(i)
    '							rchorder(i) = rchorder(j)
    '							rchorder(j) = itemp
    '							outoforder = True
    '						Else
    '							j = j + 1
    '						End If
    '					Loop 
    '					i = i + 1
    '				Loop 
    '			Loop 
    '		End If

    '		'add rec to opn seq block for each land use
    '		landname(2) = "PERLND"
    '		landname(1) = "IMPLND"
    '		If oneseg Then
    '			'only one segment for all land uses
    '			Call CreateOpnsForOneSeg(newUci)
    '		Else
    '			'user wants multiple segments
    '			Call CreateOpnsForMultSegs(newUci)
    '		End If

    '		'add record to opn seq block for each reach
    '		For i = 1 To RCHRecCnt
    '			'now add each rchres to opn seq block
    '			newOpn = New HspfOperation
    '			newOpn.Uci = newUci
    '			newOpn.Name = "RCHRES"
    '			If IsNumeric(RCHId(rchorder(i))) And Len(RCHId(rchorder(i))) < 5 Then
    '				newOpn.Description = RCHName(rchorder(i))
    '			Else
    '				newOpn.Description = RCHName(rchorder(i)) & " (" & RCHId(rchorder(i)) & ")"
    '			End If
    '			'newOpn.Id = i
    '			newOpn.Id = CInt(RCHId(rchorder(i)))
    '			mOpnSeqBlock.Add(newOpn)
    '		Next i
    '	End Sub

    '	Private Sub CreateOpnsForOneSeg(ByRef newUci As HspfUci)
    '        Dim UniqueNameCount As Integer
    '        Dim addflag As Boolean
    '        Dim newOpn As HspfOperation
    '        Dim toperid As Integer
    '        Dim lOpn As HspfOperation

    '        For j As Integer = 2 To 1 Step -1
    '            UniqueNameCount = 0
    '            For i As Integer = 1 To LURecCnt
    '                If LUType(i) = j Then
    '                    If UniqueNameCount = 0 Then
    '                        'add it
    '                        newOpn = New HspfOperation
    '                        newOpn.Uci = newUci
    '                        newOpn.Name = landname(j)
    '                        toperid = 101
    '                        firstseg(j) = toperid
    '                        newOpn.Id = toperid
    '                        lastseg(j) = toperid
    '                        newOpn.Description = LUName(i)
    '                        mOpnSeqBlock.Add(newOpn)
    '                        UniqueNameCount = UniqueNameCount + 1
    '                    Else
    '                        addflag = True
    '                        For Each lOpn In mOpnSeqBlock.Opns
    '                            If lOpn.Description = LUName(i) And lOpn.Name = landname(j) Then
    '                                addflag = False
    '                                toperid = lOpn.Id
    '                            End If
    '                        Next lOpn
    '                        If addflag Then
    '                            UniqueNameCount = UniqueNameCount + 1
    '                            newOpn = New HspfOperation
    '                            newOpn.Uci = newUci
    '                            newOpn.Name = landname(j)
    '                            newOpn.Description = LUName(i)
    '                            toperid = 100 + UniqueNameCount
    '                            newOpn.Id = toperid
    '                            lastseg(j) = toperid
    '                            mOpnSeqBlock.Add(newOpn)
    '                        End If
    '                    End If
    '                    'remember what we named this land use
    '                    luoper(i) = landname(j)
    '                    luid(i) = toperid
    '                End If
    '            Next i
    '        Next j
    '    End Sub

    '	Private Sub CreateOpnsForMultSegs(ByRef newUci As HspfUci)

    '		Dim toperid, ibase, j, i, ioper, k As Integer
    '		Dim PerlndCount, ImplndCount As Integer
    '		Dim addflag As Boolean
    '		Dim PerNames() As String
    '		Dim ndigits As Integer
    '		Dim ImpNames() As String
    '		Dim newOpn As HspfOperation

    '		PerlndCount = 0
    '		ImplndCount = 0
    '		'prescan to see how many perlnds and implnds per segment
    '		For i = 1 To LURecCnt
    '			If LUType(i) = 2 Then
    '				'perlnd
    '				addflag = True
    '				If PerlndCount > 0 Then
    '					For j = 1 To UBound(PerNames)
    '						If PerNames(j) = LUName(i) Then
    '							addflag = False
    '						End If
    '					Next j
    '				End If
    '				If addflag Then
    '					PerlndCount = PerlndCount + 1
    '					ReDim Preserve PerNames(PerlndCount)
    '					PerNames(PerlndCount) = LUName(i)
    '				End If
    '			ElseIf LUType(i) = 1 Then 
    '				'implnd
    '				addflag = True
    '				If ImplndCount > 0 Then
    '					For j = 1 To UBound(ImpNames)
    '						If ImpNames(j) = LUName(i) Then
    '							addflag = False
    '						End If
    '					Next j
    '				End If
    '				If addflag Then
    '					ImplndCount = ImplndCount + 1
    '					ReDim Preserve ImpNames(ImplndCount)
    '					ImpNames(ImplndCount) = LUName(i)
    '				End If
    '			End If
    '		Next i

    '		ndigits = 0
    '		For i = 1 To RCHRecCnt
    '			If Len(RCHId(i)) > ndigits Then
    '				ndigits = Len(RCHId(i))
    '			End If
    '		Next i

    '		If ndigits = 1 Or ndigits = 0 Then
    '			'use 101, 102, 201, 202 scheme
    '			ibase = 100
    '		ElseIf ndigits = 2 And PerlndCount < 10 And ImplndCount < 10 Then 
    '			'use 11, 12, 21, 22 scheme
    '			ibase = 10
    '		Else
    '			'too many to use the multiple seg scheme
    '			Call MsgBox("There are too many segments to use this segmentation scheme." & vbCrLf & "Create will use the 'Grouped' scheme instead", MsgBoxStyle.OKOnly, "Create Problem")
    '			Call CreateOpnsForOneSeg(newUci)
    '			ibase = 0
    '		End If

    '		If ibase > 0 Then
    '			'create these perlnd operations
    '			firstseg(1) = 99999
    '			lastseg(1) = 0
    '			firstseg(2) = 99999
    '			lastseg(2) = 0
    '			For k = 1 To RCHRecCnt
    '				'loop through each reach
    '				For i = 1 To LURecCnt
    '					'look to see if this landuse rec goes to this reach
    '					If LUReach(i) = RCHId(k) Then
    '						'it does
    '						If LUType(i) = 2 Then
    '							'add this perlnd oper
    '							newOpn = New HspfOperation
    '							newOpn.Uci = newUci
    '							newOpn.Name = "PERLND"
    '							ioper = 0
    '							For j = 1 To PerlndCount
    '								If PerNames(j) = LUName(i) Then
    '									'this is the land use we want
    '									ioper = j
    '								End If
    '							Next j
    '							toperid = (CDbl(LUReach(i)) * ibase) + ioper
    '							If toperid < firstseg(2) Then firstseg(2) = toperid
    '							If toperid > lastseg(2) Then lastseg(2) = toperid
    '							newOpn.Id = toperid
    '							newOpn.Description = LUName(i)
    '							mOpnSeqBlock.Add(newOpn)
    '							'remember what we named this land use
    '							luoper(i) = "PERLND"
    '							luid(i) = toperid
    '						End If
    '					End If
    '				Next i

    '				'now add implnds
    '				For i = 1 To LURecCnt
    '					If LUReach(i) = RCHId(k) Then
    '						If LUType(i) = 1 Then
    '							'add this implnd oper
    '							newOpn = New HspfOperation
    '							newOpn.Uci = newUci
    '							newOpn.Name = "IMPLND"
    '							ioper = 0
    '							For j = 1 To ImplndCount
    '								If ImpNames(j) = LUName(i) Then
    '									'this is the land use we want
    '									ioper = j
    '								End If
    '							Next j
    '							toperid = (CDbl(LUReach(i)) * ibase) + ioper
    '							If toperid < firstseg(1) Then firstseg(1) = toperid
    '							If toperid > lastseg(1) Then lastseg(1) = toperid
    '							newOpn.Id = toperid
    '							newOpn.Description = LUName(i)
    '							mOpnSeqBlock.Add(newOpn)
    '							'remember what we named this land use
    '							luoper(i) = "IMPLND"
    '							luid(i) = toperid
    '						End If
    '					End If
    '				Next i
    '			Next k
    '		End If

    '	End Sub

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

    '	Private Sub CreatePointSourceDSNs(ByRef myUci As HspfUci, ByRef MasterPollutantList As Collection)
    '        Dim newwdmid As String
    '		Dim newdsn As Integer
    '        Dim stanam, lLocation, sen, Con, tstype As String
    '		Dim jdates(1) As Single
    '		Dim rload(1) As Single

    '		On Error Resume Next
    '		sen = "PT-OBS"
    '        For i As Integer = 0 To PollutantCount - 1
    '            If CDbl(FacilityReach(PollutantFacID(i))) > 0 Then
    '                Con = GetPollutantIDFromName(MasterPollutantList, PollutantName(i))
    '                If Len(Con) = 0 Then
    '                    Con = UCase(Mid(PollutantName(i), 1, 8))
    '                End If
    '                stanam = FacilityName(PollutantFacID(i))
    '                lLocation = "RCH" & CStr(FacilityReach(PollutantFacID(i)))
    '                tstype = UCase(Mid(PollutantName(i), 1, 4))
    '                rload(1) = PollutantLoad(i)
    '                myUci.AddPointSourceDataSet(sen, lLocation, Con, stanam, tstype, 0, jdates, rload, newwdmid, newdsn)
    '            End If
    '        Next
    '    End Sub

    '	Private Function GetPollutantIDFromName(ByRef PollutantList As Collection, ByRef PollutantName As String) As String
    '		Dim i As Integer

    '		GetPollutantIDFromName = "x"
    '		i = 1
    '		Do While GetPollutantIDFromName = "x"
    '            If Trim(Mid(PollutantList.Item(i), 14)) = Trim(PollutantName) Then
    '                GetPollutantIDFromName = Mid(PollutantList.Item(i), 1, 5)
    '            End If
    '			i = i + 1
    '			If i > PollutantList.Count() Then GetPollutantIDFromName = ""
    '		Loop 
    '	End Function

    '	Private Sub CreateDefaultOutput(ByRef myUci As HspfUci)
    '		Dim vConn As Object
    '		Dim lConn As HspfConnection
    '		Dim wdmid, bottomid, newdsn As Integer

    '		bottomid = 0
    '		For	Each vConn In myUci.Connections
    '			lConn = vConn
    '			If lConn.Typ = 3 Then
    '				'schematic record
    '				If lConn.Source.VolName = "RCHRES" And lConn.Target.VolName = "RCHRES" Then
    '					bottomid = lConn.Target.VolId
    '				End If
    '			End If
    '		Next vConn

    '		If bottomid > 0 Then 'found watershed outlet
    '			Call myUci.AddOutputWDMDataSet("RCH" & bottomid, "FLOW", 100, wdmid, newdsn)
    '			myUci.AddExtTarget("RCHRES", bottomid, "HYDR", "RO", 1, 1, 1#, "AVER", "WDM" & CStr(wdmid), newdsn, "FLOW", 1, "ENGL", "AGGR", "REPL")
    '			'myUci.GetWDMObj(wdmid).Refresh
    '		End If

    '	End Sub

    '	Private Function DefaultLSURFromSLSUR(ByRef slsur As Single) As Object
    '		If slsur < 0.005 Then
    '            DefaultLSURFromSLSUR = 500
    '		ElseIf slsur < 0.01 Then 
    '            DefaultLSURFromSLSUR = 400
    '		ElseIf slsur < 0.03 Then 
    '            DefaultLSURFromSLSUR = 350
    '		ElseIf slsur < 0.07 Then 
    '            DefaultLSURFromSLSUR = 300
    '		ElseIf slsur < 0.1 Then 
    '            DefaultLSURFromSLSUR = 250
    '		ElseIf slsur < 0.15 Then 
    '            DefaultLSURFromSLSUR = 200
    '		Else
    '            DefaultLSURFromSLSUR = 150
    '		End If
    '	End Function

    '	Private Sub CreateBinaryOutput(ByRef myUci As HspfUci, ByRef s As String)
    '        Dim newFile As New HspfData.HspfFile
    '		Dim i As Integer
    '		Dim lOper As HspfOperation

    '		'add file name to files block
    '		newFile.Name = s & ".hbn"
    '		newFile.Typ = "BINO"
    '		newFile.Unit = 92
    '		mFilesBlock.Add(newFile)
    '		'update bin output units
    '        For i = 1 To myUci.OpnBlks.Item("PERLND").Count
    '            lOper = myUci.OpnBlks.Item("PERLND").Ids(i)
    '            lOper.Tables.Item("GEN-INFO").ParmValue("BUNIT1") = 92
    '        Next i
    '        For i = 1 To myUci.OpnBlks.Item("IMPLND").Count
    '            lOper = myUci.OpnBlks.Item("IMPLND").Ids(i)
    '            lOper.Tables.Item("GEN-INFO").ParmValue("BUNIT1") = 92
    '        Next i
    '        For i = 1 To myUci.OpnBlks.Item("RCHRES").Count
    '            lOper = myUci.OpnBlks.Item("RCHRES").Ids(i)
    '            lOper.Tables.Item("GEN-INFO").ParmValue("BUNITE") = 92
    '        Next i
    '		'add binary-info tables
    '        myUci.OpnBlks.Item("PERLND").AddTableForAll("BINARY-INFO", "PERLND")
    '        myUci.OpnBlks.Item("IMPLND").AddTableForAll("BINARY-INFO", "IMPLND")
    '        myUci.OpnBlks.Item("RCHRES").AddTableForAll("BINARY-INFO", "RCHRES")
    '	End Sub
End Module