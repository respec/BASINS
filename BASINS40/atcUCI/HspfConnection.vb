Option Strict Off
Option Explicit On
<System.Runtime.InteropServices.ProgId("HspfConnection_NET.HspfConnection")> Public Class HspfConnection
    'Copyright 2006 AQUA TERRA Consultants - Royalty-free use permitted under open source license
	
	Dim pMFact As Double
	Dim pMFactAsRead As String
	Dim pTyp As Integer '1-ExtSource,2-Network,3-Schematic,4-ExtTarget
	Dim pTran As String
	Dim pSgapstrg As String
	Dim pAmdstrg As String
	Dim pSsystem As String
	Dim pSource As HspfSrcTar
	Dim pTarget As HspfSrcTar
	Dim pMassLink As Integer
	Dim pUci As HspfUci
	Dim DesiredType As String
	Dim pComment As String
	
	Public Property MFact() As Double
		Get
			MFact = pMFact
		End Get
		Set(ByVal Value As Double)
			pMFact = Value
		End Set
	End Property
	
	Public Property MFactAsRead() As String
		Get
			MFactAsRead = pMFactAsRead
		End Get
		Set(ByVal Value As String)
			pMFactAsRead = Value
		End Set
	End Property
	
	Public Property Uci() As HspfUci
		Get
			Uci = pUci
		End Get
		Set(ByVal Value As HspfUci)
			pUci = Value
		End Set
	End Property
	
	Public Property Source() As HspfSrcTar
		Get
			Source = pSource
		End Get
		Set(ByVal Value As HspfSrcTar)
			pSource = Value
		End Set
	End Property
	
	Public Property Target() As HspfSrcTar
		Get
			Target = pTarget
		End Get
		Set(ByVal Value As HspfSrcTar)
			pTarget = Value
		End Set
	End Property
	
	Public Property Tran() As String
		Get
			Tran = pTran
		End Get
		Set(ByVal Value As String)
			pTran = Value
		End Set
	End Property
	
	
	Public Property Comment() As String
		Get
			Comment = pComment
		End Get
		Set(ByVal Value As String)
			pComment = Value
		End Set
	End Property
	
	Public Property Ssystem() As String
		Get
			Ssystem = pSsystem
		End Get
		Set(ByVal Value As String)
			pSsystem = Value
		End Set
	End Property
	
	Public Property Sgapstrg() As String
		Get
			Sgapstrg = pSgapstrg
		End Get
		Set(ByVal Value As String)
			pSgapstrg = Value
		End Set
	End Property
	
	Public Property Amdstrg() As String
		Get
			Amdstrg = pAmdstrg
		End Get
		Set(ByVal Value As String)
			pAmdstrg = Value
		End Set
	End Property
	Public Property Typ() As Integer
		Get
			Typ = pTyp
		End Get
		Set(ByVal Value As Integer)
			pTyp = Value
		End Set
	End Property
	
	Public Property MassLink() As Integer
		Get
			MassLink = pMassLink
		End Get
		Set(ByVal Value As Integer)
			pMassLink = Value
		End Set
	End Property
	Public ReadOnly Property EditControlName() As String
		Get
			EditControlName = "ATCoHspf.ctlConnectionEdit"
		End Get
	End Property
	Public ReadOnly Property DesiredRecordType() As String
		Get
			DesiredRecordType = DesiredType
		End Get
	End Property
	Public ReadOnly Property Caption() As String
		Get
			Caption = DesiredType & " Block"
		End Get
	End Property
	Public Sub readTimSer(ByRef myUci As HspfUci)
		Dim retcod, OmCode, init, retkey, rectyp As Integer
		Dim cbuff As String
        Dim lConnection As HspfConnection
		Dim s, c As String
		Dim pastHeader As Boolean
		Dim t As String
		
		pUci = myUci
		OmCode = HspfOmCode("EXT SOURCES")
		init = 1
		c = ""
		pastHeader = False
		retkey = -1
		Do 
			If myUci.FastFlag Then
				GetNextRecordFromBlock("EXT SOURCES", retkey, cbuff, rectyp, retcod)
			Else
				retkey = -1
				Call REM_XBLOCKEX(myUci, OmCode, init, retkey, cbuff, rectyp, retcod)
			End If
			If retcod <> 2 Then Exit Do
			init = 0
			If rectyp = 0 Then
				lConnection = New HspfConnection
				lConnection.Uci = myUci
				lConnection.Typ = 1
				lConnection.Source.VolName = Trim(Left(cbuff, 6))
				lConnection.Source.VolId = CInt(Mid(cbuff, 7, 4))
				lConnection.Source.Member = Trim(Mid(cbuff, 12, 6))
				s = Trim(Mid(cbuff, 18, 2))
				If Len(s) > 0 Then lConnection.Source.MemSub1 = CInt(s)
				lConnection.Ssystem = Mid(cbuff, 21, 4)
				lConnection.Sgapstrg = Mid(cbuff, 25, 4)
				s = Trim(Mid(cbuff, 29, 10))
				lConnection.MFactAsRead = Mid(cbuff, 29, 10)
				If Len(s) > 0 Then lConnection.MFact = CDbl(s)
				s = Mid(cbuff, 39, 4)
				If Len(s) > 0 Then lConnection.Tran = s
				lConnection.Target.VolName = Trim(Mid(cbuff, 44, 6))
				lConnection.Target.VolId = CInt(Mid(cbuff, 51, 3))
				s = Trim(Mid(cbuff, 55, 3))
				If Len(s) > 0 Then lConnection.Target.VolIdL = CInt(s)
				lConnection.Target.Group = Trim(Mid(cbuff, 59, 6))
				lConnection.Target.Member = Trim(Mid(cbuff, 66, 6))
				s = Trim(Mid(cbuff, 72, 2))
				If Len(s) > 0 And IsNumeric(s) Then
					lConnection.Target.MemSub1 = CInt(s)
				Else
					If Len(s) > 0 Then lConnection.Target.MemSub1 = myUci.CatAsInt(s)
				End If
				s = Trim(Mid(cbuff, 74, 2))
				If Len(s) > 0 And IsNumeric(s) Then
					lConnection.Target.MemSub2 = CInt(s)
				Else
					If Len(s) > 0 Then lConnection.Target.MemSub2 = myUci.CatAsInt(s)
				End If
				lConnection.Comment = c
				myUci.Connections.Add(lConnection)
				c = ""
			ElseIf rectyp = -1 And retcod <> 1 Then 
				'save comment
				t = Left(cbuff, 6)
				If t = "<Name>" Then 'a cheap rule to identify the last header line
					pastHeader = True
				ElseIf pastHeader Then 
					If Len(c) = 0 Then
						c = cbuff
					Else
						c = c & vbCrLf & cbuff
					End If
				End If
			End If
		Loop 
		
		OmCode = HspfOmCode("NETWORK")
		init = 1
		c = ""
		pastHeader = False
		retkey = -1
		Do 
			If myUci.FastFlag Then
				GetNextRecordFromBlock("NETWORK", retkey, cbuff, rectyp, retcod)
			Else
				retkey = -1
				Call REM_XBLOCKEX(myUci, OmCode, init, retkey, cbuff, rectyp, retcod)
			End If
			If retcod <> 2 Then Exit Do
			init = 0
			If rectyp = 0 Then
				lConnection = New HspfConnection
				lConnection.Uci = myUci
				lConnection.Typ = 2
				lConnection.Source.VolName = Trim(Left(cbuff, 6))
				lConnection.Source.VolId = CInt(Mid(cbuff, 7, 4))
				lConnection.Source.Group = Trim(Mid(cbuff, 12, 6))
				lConnection.Source.Member = Trim(Mid(cbuff, 19, 6))
				s = Trim(Mid(cbuff, 25, 2))
				If Len(s) > 0 And IsNumeric(s) Then
					lConnection.Source.MemSub1 = CInt(s)
				Else
					If Len(s) > 0 Then lConnection.Source.MemSub1 = myUci.CatAsInt(s)
				End If
				s = Trim(Mid(cbuff, 27, 2))
				If Len(s) > 0 And IsNumeric(s) Then
					lConnection.Source.MemSub2 = CInt(s)
				Else
					If Len(s) > 0 Then lConnection.Source.MemSub2 = myUci.CatAsInt(s)
				End If
				s = Trim(Mid(cbuff, 29, 10))
				lConnection.MFactAsRead = Mid(cbuff, 29, 10)
				If Len(s) > 0 Then lConnection.MFact = CDbl(s)
				lConnection.Tran = Trim(Mid(cbuff, 39, 4))
				lConnection.Target.VolName = Trim(Mid(cbuff, 44, 6))
				lConnection.Target.VolId = CInt(Mid(cbuff, 51, 3))
				s = Trim(Mid(cbuff, 55, 3))
				If Len(s) > 0 Then lConnection.Target.VolIdL = CInt(s)
				lConnection.Target.Group = Trim(Mid(cbuff, 59, 6))
				lConnection.Target.Member = Trim(Mid(cbuff, 66, 6))
				s = Trim(Mid(cbuff, 72, 2))
				If Len(s) > 0 And IsNumeric(s) Then
					lConnection.Target.MemSub1 = CInt(s)
				Else
					If Len(s) > 0 Then lConnection.Target.MemSub1 = myUci.CatAsInt(s)
				End If
				s = Trim(Mid(cbuff, 74, 2))
				If Len(s) > 0 And IsNumeric(s) Then
					lConnection.Target.MemSub2 = CInt(s)
				Else
					If Len(s) > 0 Then lConnection.Target.MemSub2 = myUci.CatAsInt(s)
				End If
				lConnection.Comment = c
				myUci.Connections.Add(lConnection)
				c = ""
			ElseIf rectyp = -1 And retcod <> 1 Then 
				'save comment
				t = Left(cbuff, 6)
				If t = "<Name>" Then 'a cheap rule to identify the last header line
					pastHeader = True
				ElseIf pastHeader Then 
					If Len(c) = 0 Then
						c = cbuff
					Else
						c = c & vbCrLf & cbuff
					End If
				End If
			End If
		Loop 
		
		OmCode = HspfOmCode("SCHEMATIC")
		init = 1
		c = ""
		pastHeader = False
		retkey = -1
		Do 
			If myUci.FastFlag Then
				GetNextRecordFromBlock("SCHEMATIC", retkey, cbuff, rectyp, retcod)
			Else
				retkey = -1
				Call REM_XBLOCKEX(myUci, OmCode, init, retkey, cbuff, rectyp, retcod)
			End If
			If retcod <> 2 Then Exit Do
			init = 0
			If rectyp = 0 Then
				lConnection = New HspfConnection
				lConnection.Uci = myUci
				lConnection.Typ = 3
				lConnection.Source.VolName = Trim(Left(cbuff, 6))
				lConnection.Source.VolId = CInt(Mid(cbuff, 7, 4))
				s = Trim(Mid(cbuff, 29, 10))
				lConnection.MFactAsRead = Mid(cbuff, 29, 10)
				If Len(s) > 0 Then lConnection.MFact = CDbl(s)
				lConnection.Target.VolName = Trim(Mid(cbuff, 44, 6))
				lConnection.Target.VolId = CInt(Mid(cbuff, 50, 4))
				lConnection.MassLink = CInt(Mid(cbuff, 57, 4))
				s = Trim(Mid(cbuff, 72, 2))
				If Len(s) > 0 And IsNumeric(s) Then
					lConnection.Target.MemSub1 = CInt(s)
				Else
					If Len(s) > 0 Then lConnection.Target.MemSub1 = myUci.CatAsInt(s)
				End If
				s = Trim(Mid(cbuff, 74, 2))
				If Len(s) > 0 And IsNumeric(s) Then
					lConnection.Target.MemSub2 = CInt(s)
				Else
					If Len(s) > 0 Then lConnection.Target.MemSub2 = myUci.CatAsInt(s)
				End If
				lConnection.Comment = c
				myUci.Connections.Add(lConnection)
				c = ""
			ElseIf rectyp = -1 And retcod <> 1 Then 
				'save comment
				t = Left(cbuff, 6)
				If t = "<Name>" Then 'a cheap rule to identify the last header line
					pastHeader = True
				ElseIf pastHeader Then 
					If Len(c) = 0 Then
						c = cbuff
					Else
						c = c & vbCrLf & cbuff
					End If
				End If
			End If
		Loop 
		
		OmCode = HspfOmCode("EXT TARGETS")
		init = 1
		c = ""
		pastHeader = False
		retkey = -1
		Do 
			If myUci.FastFlag Then
				GetNextRecordFromBlock("EXT TARGETS", retkey, cbuff, rectyp, retcod)
			Else
				retkey = -1
				Call REM_XBLOCKEX(myUci, OmCode, init, retkey, cbuff, rectyp, retcod)
			End If
			If retcod <> 2 Then Exit Do
			init = 0
			If rectyp = 0 Then
				lConnection = New HspfConnection
				lConnection.Uci = myUci
				lConnection.Typ = 4
				lConnection.Source.VolName = Trim(Left(cbuff, 6))
				lConnection.Source.VolId = CInt(Mid(cbuff, 7, 4))
				lConnection.Source.Group = Trim(Mid(cbuff, 12, 6))
				lConnection.Source.Member = Trim(Mid(cbuff, 19, 6))
				s = Trim(Mid(cbuff, 25, 2))
				If Len(s) > 0 And IsNumeric(s) Then
					lConnection.Source.MemSub1 = CInt(s)
				Else
					If Len(s) > 0 Then lConnection.Source.MemSub1 = myUci.CatAsInt(s)
				End If
				s = Trim(Mid(cbuff, 27, 2))
				If Len(s) > 0 And IsNumeric(s) Then
					lConnection.Source.MemSub2 = CInt(s)
				Else
					If Len(s) > 0 Then lConnection.Source.MemSub1 = myUci.CatAsInt(s)
				End If
				s = Trim(Mid(cbuff, 29, 10))
				lConnection.MFactAsRead = Mid(cbuff, 29, 10)
				If Len(s) > 0 Then lConnection.MFact = CDbl(s)
				s = Trim(Mid(cbuff, 39, 4))
				If Len(s) > 0 Then lConnection.Tran = s
				lConnection.Target.VolName = Trim(Mid(cbuff, 44, 6))
				lConnection.Target.VolId = CInt(Mid(cbuff, 50, 4))
				lConnection.Target.Member = Trim(Mid(cbuff, 55, 6))
				s = Trim(Mid(cbuff, 61, 2))
				If Len(s) > 0 Then lConnection.Target.MemSub1 = CInt(s)
				lConnection.Ssystem = Trim(Mid(cbuff, 64, 4))
				lConnection.Sgapstrg = Trim(Mid(cbuff, 69, 4))
				lConnection.Amdstrg = Trim(Mid(cbuff, 74, 4))
				lConnection.Comment = c
				myUci.Connections.Add(lConnection)
				c = ""
			ElseIf rectyp = -1 And retcod <> 1 Then 
				'save comment
				t = Left(cbuff, 6)
				If t = "<Name>" Then 'a cheap rule to identify the last header line
					pastHeader = True
				ElseIf pastHeader Then 
					If Len(c) = 0 Then
						c = cbuff
					Else
						c = c & vbCrLf & cbuff
					End If
				End If
			End If
		Loop 
	End Sub
	
	'UPGRADE_NOTE: Class_Initialize was upgraded to Class_Initialize_Renamed. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"'
	Private Sub Class_Initialize_Renamed()
		pSource = New HspfSrcTar
		pTarget = New HspfSrcTar
		pTyp = 0
		pMFact = 1#
	End Sub
	Public Sub New()
		MyBase.New()
		Class_Initialize_Renamed()
	End Sub
	Public Sub EditExtSrc()
		DesiredType = "EXT SOURCES"
		editInit(Me, Me.Uci.icon, True) 'add remove ok
	End Sub
	Public Sub EditExtTar()
		DesiredType = "EXT TARGETS"
		editInit(Me, Me.Uci.icon, True) 'add remove ok
	End Sub
	Public Sub EditNetwork()
		DesiredType = "NETWORK"
		editInit(Me, Me.Uci.icon, True) 'add remove ok
	End Sub
	Public Sub EditSchematic()
		DesiredType = "SCHEMATIC"
		editInit(Me, Me.Uci.icon, True) 'add remove ok
	End Sub
	
	Public Sub WriteUciFile(ByRef f As Short, ByRef M As HspfMsg)
        Dim lStr, s, optyp As String
        Dim lBlockDef As HspfBlockDef
        Dim lTableDef As HspfTableDef
        Dim j, i, k As Integer
        Dim typeexists(4) As Boolean
        Dim icol(15) As Integer
        Dim ilen(15) As Integer
        Dim lOper As HspfOperation
        Dim lConn As HspfConnection
        Dim lOpnSeqBlock As HspfOpnSeqBlk
        Dim lParmDef As HSPFParmDef
        Dim t As String
        Dim lMetSeg As HspfMetSeg
        Dim lPtSrc As HspfPoint
        Static lOpTypes() As String = {"PERLND", "IMPLND", "RCHRES"} 'operations with assoc met segs

        typeexists(0) = False 'ext sou
        typeexists(1) = False 'network
        typeexists(2) = False 'schematic
        typeexists(3) = False 'ext tar

        If pUci.MetSegs.Count() > 0 Then
            typeexists(0) = True
        End If
        If pUci.PointSources.Count() > 0 Then
            typeexists(0) = True
        End If

        lOpnSeqBlock = pUci.OpnSeqBlock
        For i = 1 To lOpnSeqBlock.Opns.Count
            lOper = lOpnSeqBlock.Opn(i)
            For j = 1 To lOper.Targets.Count()
                lConn = lOper.Targets.Item(j)
                typeexists(lConn.Typ - 1) = True
            Next j
            For j = 1 To lOper.Sources.Count()
                lConn = lOper.Sources.Item(j)
                typeexists(lConn.Typ - 1) = True
            Next j
        Next i

        For i = 1 To 4
            If typeexists(i - 1) Then
                Select Case i
                    Case 1 : s = "EXT SOURCES"
                    Case 2 : s = "NETWORK"
                    Case 3 : s = "SCHEMATIC"
                    Case 4 : s = "EXT TARGETS"
                    Case Else : s = ""
                End Select
                lBlockDef = M.BlockDefs.Item(s)
                lTableDef = lBlockDef.TableDefs.Item(1)
                'get lengths and starting positions
                j = 0
                For Each lParmDef In lTableDef.ParmDefs
                    icol(j) = lParmDef.StartCol
                    ilen(j) = lParmDef.Length
                    j = j + 1
                Next lParmDef
                PrintLine(f, " ")
                PrintLine(f, s)
                'now start building the records
                Select Case i
                    Case 1 'ext srcs
                        PrintLine(f, "<-Volume-> <Member> SsysSgap<--Mult-->Tran <-Target vols> <-Grp> <-Member-> ***")
                        PrintLine(f, "<Name>   x <Name> x tem strg<-factor->strg <Name>   x   x        <Name> x x ***")
                        'do met segs
                        For Each optyp In lOpTypes
                            For Each lMetSeg In pUci.MetSegs
                                lMetSeg.WriteUciFile(optyp, icol, ilen, f)
                            Next
                        Next
                        'do point sources
                        If pUci.PointSources.Count() > 0 And pUci.MetSegs.Count() > 0 Then
                            PrintLine(f, "") 'write a blank line between met segs and pt srcs
                        End If
                        For Each lPtSrc In pUci.PointSources
                            lPtSrc.WriteUciFile(icol, ilen, f)
                        Next
                        'now do everything else
                        For k = 1 To lOpnSeqBlock.Opns.Count
                            lOper = lOpnSeqBlock.Opn(k)
                            For j = 1 To lOper.Sources.Count()
                                lConn = lOper.Sources.Item(j)
                                If lConn.Typ = i Then
                                    lStr = Trim(lConn.Source.VolName)
                                    lStr &= Space(icol(1) - Len(lStr) - 1) 'pad prev field
                                    t = Space(ilen(1)) 'right justify numbers
                                    t = RSet(CStr(lConn.Source.VolId), Len(t))
                                    lStr &= t
                                    lStr &= Space(icol(2) - Len(lStr) - 1)
                                    lStr &= lConn.Source.Member
                                    lStr &= Space(icol(3) - Len(lStr) - 1)
                                    If lConn.Source.MemSub1 <> 0 Then
                                        t = Space(ilen(3))
                                        t = RSet(CStr(lConn.Source.MemSub1), Len(t))
                                        If lConn.Source.VolName = "RCHRES" Then t = pUci.IntAsCat(lConn.Source.Member, 1, t)
                                        lStr &= t
                                    End If
                                    lStr &= Space(icol(4) - Len(lStr) - 1)
                                    lStr &= lConn.Ssystem
                                    lStr &= Space(icol(5) - Len(lStr) - 1)
                                    lStr &= lConn.Sgapstrg
                                    lStr &= Space(icol(6) - Len(lStr) - 1)
                                    If NumericallyTheSame((lConn.MFactAsRead), (lConn.MFact)) Then
                                        t = lConn.MFactAsRead
                                        lStr &= t
                                    ElseIf lConn.MFact <> 1 Then
                                        t = Space(ilen(6))
                                        t = RSet(CStr(lConn.MFact), Len(t))
                                        lStr &= t
                                    End If
                                    lStr &= Space(icol(7) - Len(lStr) - 1)
                                    lStr &= lConn.Tran
                                    lStr &= Space(icol(8) - Len(lStr) - 1)
                                    lStr &= lOper.Name
                                    lStr &= Space(icol(9) - Len(lStr) - 1)
                                    t = Space(ilen(9))
                                    t = RSet(CStr(lOper.Id), Len(t))
                                    lStr &= t
                                    lStr &= Space(icol(11) - Len(lStr) - 1)
                                    lStr &= lConn.Target.Group
                                    lStr &= Space(icol(12) - Len(lStr) - 1)
                                    lStr &= lConn.Target.Member
                                    lStr &= Space(icol(13) - Len(lStr) - 1)
                                    If lConn.Target.MemSub1 <> 0 Then
                                        t = Space(ilen(13))
                                        t = RSet(CStr(lConn.Target.MemSub1), Len(t))
                                        If lConn.Target.VolName = "RCHRES" Then t = pUci.IntAsCat(lConn.Target.Member, 1, t)
                                        lStr &= t
                                    End If
                                    lStr &= Space(icol(14) - Len(lStr) - 1)
                                    If lConn.Target.MemSub2 <> 0 Then
                                        t = Space(ilen(14))
                                        t = RSet(CStr(lConn.Target.MemSub2), Len(t))
                                        If lConn.Target.VolName = "RCHRES" Then t = pUci.IntAsCat(lConn.Target.Member, 2, t)
                                        lStr &= t
                                    End If
                                    If Len(lConn.Comment) > 0 Then
                                        PrintLine(f, lConn.Comment)
                                    End If
                                    PrintLine(f, lStr)
                                End If
                            Next j
                        Next k
                    Case 2 'network
                        PrintLine(f, "<-Volume-> <-Grp> <-Member-><--Mult-->Tran <-Target vols> <-Grp> <-Member->  ***")
                        PrintLine(f, "<Name>   x        <Name> x x<-factor->strg <Name>   x   x        <Name> x x  ***")
                        For k = 1 To lOpnSeqBlock.Opns.Count
                            lOper = lOpnSeqBlock.Opn(k)
                            For j = 1 To lOper.Sources.Count() 'used to go thru targets, misses range
                                lConn = lOper.Sources.Item(j)
                                If lConn.Typ = i Then
                                    lStr = Trim(lConn.Source.VolName)
                                    lStr &= Space(icol(1) - Len(lStr) - 1) 'pad prev field
                                    t = Space(ilen(1)) 'right justify numbers
                                    t = RSet(CStr(lConn.Source.VolId), Len(t))
                                    lStr &= t
                                    lStr &= Space(icol(2) - Len(lStr) - 1)
                                    lStr &= lConn.Source.Group
                                    lStr &= Space(icol(3) - Len(lStr) - 1)
                                    lStr &= lConn.Source.Member
                                    lStr &= Space(icol(4) - Len(lStr) - 1)
                                    If lConn.Source.MemSub1 <> 0 Then
                                        t = Space(ilen(4))
                                        t = RSet(CStr(lConn.Source.MemSub1), Len(t))
                                        If lConn.Source.VolName = "RCHRES" Then t = pUci.IntAsCat(lConn.Source.Member, 1, t)
                                        lStr &= t
                                    End If
                                    lStr &= Space(icol(5) - Len(lStr) - 1)
                                    If lConn.Source.MemSub2 <> 0 Then
                                        t = Space(ilen(5))
                                        t = RSet(CStr(lConn.Source.MemSub2), Len(t))
                                        If lConn.Source.VolName = "RCHRES" Then t = pUci.IntAsCat(lConn.Source.Member, 2, t)
                                        lStr &= t
                                    End If
                                    lStr &= Space(icol(6) - Len(lStr) - 1)
                                    If NumericallyTheSame((lConn.MFactAsRead), (lConn.MFact)) Then
                                        t = lConn.MFactAsRead
                                        lStr &= t
                                    ElseIf lConn.MFact <> 1 Then
                                        t = Space(ilen(6))
                                        t = RSet(CStr(lConn.MFact), Len(t))
                                        lStr &= t
                                    End If
                                    lStr &= Space(icol(7) - Len(lStr) - 1)
                                    lStr &= lConn.Tran
                                    lStr &= Space(icol(8) - Len(lStr) - 1)
                                    lStr &= lOper.Name
                                    lStr &= Space(icol(9) - Len(lStr) - 1)
                                    t = Space(ilen(9))
                                    t = RSet(CStr(lOper.Id), Len(t))
                                    lStr &= t
                                    lStr &= Space(icol(11) - Len(lStr) - 1)
                                    lStr &= lConn.Target.Group
                                    lStr &= Space(icol(12) - Len(lStr) - 1)
                                    lStr &= lConn.Target.Member
                                    lStr &= Space(icol(13) - Len(lStr) - 1)
                                    If lConn.Target.MemSub1 <> 0 Then
                                        t = Space(ilen(13))
                                        t = RSet(CStr(lConn.Target.MemSub1), Len(t))
                                        If lConn.Target.VolName = "RCHRES" Then t = pUci.IntAsCat(lConn.Target.Member, 1, t)
                                        lStr &= t
                                    End If
                                    lStr &= Space(icol(14) - Len(lStr) - 1)
                                    If lConn.Target.MemSub2 <> 0 Then
                                        t = Space(ilen(14))
                                        t = RSet(CStr(lConn.Target.MemSub2), Len(t))
                                        If lConn.Target.VolName = "RCHRES" Then t = pUci.IntAsCat(lConn.Target.Member, 2, t)
                                        lStr &= t
                                    End If
                                    If Len(lConn.Comment) > 0 Then
                                        PrintLine(f, lConn.Comment)
                                    End If
                                    PrintLine(f, lStr)
                                End If
                            Next j
                        Next k
                    Case 3 'schematic
                        PrintLine(f, "<-Volume->                  <--Area-->     <-Volume->  <ML#> ***       <sb>")
                        PrintLine(f, "<Name>   x                  <-factor->     <Name>   x        ***        x x")
                        For k = 1 To lOpnSeqBlock.Opns.Count
                            lOper = lOpnSeqBlock.Opn(k)
                            For j = 1 To lOper.Sources.Count()
                                lConn = lOper.Sources.Item(j)
                                If lConn.Typ = i Then
                                    lStr = Trim(lConn.Source.VolName)
                                    lStr &= Space(icol(1) - Len(lStr) - 1) 'pad prev field
                                    t = Space(ilen(1)) 'right justify numbers
                                    t = RSet(CStr(lConn.Source.VolId), Len(t))
                                    lStr &= t
                                    lStr &= Space(icol(2) - Len(lStr) - 1)
                                    If NumericallyTheSame((lConn.MFactAsRead), (lConn.MFact)) Then
                                        t = lConn.MFactAsRead
                                        lStr &= t
                                    ElseIf lConn.MFact <> 1 Then
                                        lConn.MFact = System.Math.Round(lConn.MFact, 2)
                                        t = Space(ilen(2))
                                        t = RSet(CStr(lConn.MFact), Len(t))
                                        lStr &= t
                                    End If
                                    lStr &= Space(icol(3) - Len(lStr) - 1)
                                    lStr &= lConn.Target.VolName
                                    lStr &= Space(icol(4) - Len(lStr) - 1)
                                    t = Space(ilen(5))
                                    t = RSet(CStr(lConn.Target.VolId), Len(t))
                                    lStr &= t
                                    lStr &= Space(icol(5) - Len(lStr) - 1)
                                    t = Space(ilen(5))
                                    t = RSet(CStr(lConn.MassLink), Len(t))
                                    lStr &= t
                                    If lConn.Target.MemSub1 > 0 Then
                                        lStr &= Space(icol(6) - Len(lStr) - 1)
                                        t = Space(ilen(6))
                                        t = RSet(CStr(lConn.Target.MemSub1), Len(t))
                                        If lConn.Target.VolName = "RCHRES" Then t = pUci.IntAsCat(lConn.Target.Member, 1, t)
                                        lStr &= t
                                    End If
                                    If lConn.Target.MemSub2 > 0 Then
                                        lStr &= Space(icol(7) - Len(lStr) - 1)
                                        t = Space(ilen(7))
                                        t = RSet(CStr(lConn.Target.MemSub2), Len(t))
                                        If lConn.Target.VolName = "RCHRES" Then t = pUci.IntAsCat(lConn.Target.Member, 2, t)
                                        lStr &= t
                                    End If
                                    If Len(lConn.Comment) > 0 Then
                                        PrintLine(f, lConn.Comment)
                                    End If
                                    PrintLine(f, lStr)
                                End If
                            Next j
                        Next k
                    Case 4 'ext targ
                        PrintLine(f, "<-Volume-> <-Grp> <-Member-><--Mult-->Tran <-Volume-> <Member> Tsys Aggr Amd ***")
                        PrintLine(f, "<Name>   x        <Name> x x<-factor->strg <Name>   x <Name>qf  tem strg strg***")
                        For k = 1 To lOpnSeqBlock.Opns.Count
                            lOper = lOpnSeqBlock.Opn(k)
                            For j = 1 To lOper.Targets.Count()
                                lConn = lOper.Targets.Item(j)
                                If lConn.Typ = i Then
                                    lStr = Trim(lConn.Source.VolName)
                                    lStr &= Space(icol(1) - Len(lStr) - 1) 'pad prev field
                                    t = Space(ilen(1)) 'right justify numbers
                                    t = RSet(CStr(lConn.Source.VolId), Len(t))
                                    lStr &= t
                                    lStr &= Space(icol(2) - Len(lStr) - 1)
                                    lStr &= lConn.Source.Group
                                    lStr &= Space(icol(3) - Len(lStr) - 1)
                                    lStr &= lConn.Source.Member
                                    lStr &= Space(icol(4) - Len(lStr) - 1)
                                    If lConn.Source.MemSub1 <> 0 Then
                                        t = Space(ilen(4))
                                        t = RSet(CStr(lConn.Source.MemSub1), Len(t))
                                        If lConn.Source.VolName = "RCHRES" Then t = pUci.IntAsCat(lConn.Source.Member, 1, t)
                                        lStr &= t
                                    End If
                                    lStr &= Space(icol(5) - Len(lStr) - 1)
                                    If lConn.Source.MemSub2 <> 0 Then
                                        t = Space(ilen(5))
                                        t = RSet(CStr(lConn.Source.MemSub2), Len(t))
                                        If lConn.Source.VolName = "RCHRES" Then t = pUci.IntAsCat(lConn.Source.Member, 2, t)
                                        lStr &= t
                                    End If
                                    lStr &= Space(icol(6) - Len(lStr) - 1)
                                    If NumericallyTheSame((lConn.MFactAsRead), (lConn.MFact)) Then
                                        t = lConn.MFactAsRead
                                        lStr &= t
                                    ElseIf lConn.MFact <> 1 Then
                                        t = Space(ilen(6) - 1)
                                        'lConn.MFact = Format(lConn.MFact, "0.#######")
                                        'RSet t = CStr(lConn.MFact)
                                        t = RSet(HspfTable.NumFmtRE(lConn.MFact, ilen(6) - 1), Len(t))
                                        lConn.MFact = CDbl(t)
                                        lStr &= " " & t
                                    End If
                                    lStr &= Space(icol(7) - Len(lStr) - 1)
                                    lStr &= lConn.Tran
                                    lStr &= Space(icol(8) - Len(lStr) - 1)
                                    lStr &= lConn.Target.VolName
                                    lStr &= Space(icol(9) - Len(lStr) - 1)
                                    t = Space(ilen(9))
                                    t = RSet(CStr(lConn.Target.VolId), Len(t))
                                    lStr &= t
                                    lStr &= Space(icol(10) - Len(lStr) - 1)
                                    If Len(lConn.Target.Member) > ilen(10) Then 'dont write more chars than there is room for
                                        lConn.Target.Member = Mid(lConn.Target.Member, 1, ilen(10))
                                    End If
                                    lStr = Trim(lStr & lConn.Target.Member)
                                    If (icol(11) - Len(lStr) - 1 > 0) Then 'check to make sure not spacing zero or fewer characters
                                        lStr &= Space(icol(11) - Len(lStr) - 1)
                                    End If
                                    If lConn.Target.MemSub1 <> 0 Then
                                        t = Space(ilen(11))
                                        t = RSet(CStr(lConn.Target.MemSub1), Len(t))
                                        If lConn.Target.VolName = "RCHRES" Then t = pUci.IntAsCat(lConn.Target.Member, 1, t)
                                        lStr &= t
                                    End If
                                    lStr &= Space(icol(12) - Len(lStr) - 1)
                                    lStr &= lConn.Ssystem
                                    lStr &= Space(icol(13) - Len(lStr) - 1)
                                    lStr &= lConn.Sgapstrg
                                    lStr &= Space(icol(14) - Len(lStr) - 1)
                                    lStr &= lConn.Amdstrg
                                    If Len(lConn.Comment) > 0 Then
                                        PrintLine(f, lConn.Comment)
                                    End If
                                    PrintLine(f, lStr)
                                End If
                            Next j
                        Next k
                End Select
                PrintLine(f, "END " & s)
            End If
        Next i
	End Sub
	
	Private Function NumericallyTheSame(ByRef ValueAsRead As String, ByRef ValueStored As Single) As Boolean
		'see if the current mfact value is the same as the value as read from the uci
		'4. is the same as 4.0
		Dim rtemp1 As Single
		
		NumericallyTheSame = False
		If IsNumeric(ValueStored) Then
			If IsNumeric(ValueAsRead) Then
				'simple case
				rtemp1 = CSng(ValueAsRead)
				If rtemp1 = ValueStored Then
					NumericallyTheSame = True
				End If
			End If
		End If
	End Function
End Class