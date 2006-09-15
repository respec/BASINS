Option Strict Off
Option Explicit On
<System.Runtime.InteropServices.ProgId("HspfMetSeg_NET.HspfMetSeg")> Public Class HspfMetSeg
    'Copyright 2006 AQUA TERRA Consultants - Royalty-free use permitted under open source license
	
	Dim pMetSegRecs(8) As HspfMetSegRecord
	Dim pId As Integer
	Dim pName As String
	Dim pUci As HspfUci
	Dim pAirType As Integer '1-GATMP, 2-AIRTMP
	
	Public Property MetSegRecs() As HspfMetSegRecord()
		Get
			MetSegRecs = VB6.CopyArray(pMetSegRecs)
		End Get
		Set(ByVal Value() As HspfMetSegRecord)
			Dim itype As Integer
			For itype = 1 To 8
				pMetSegRecs(itype) = Value(itype)
			Next 
		End Set
    End Property

	Public ReadOnly Property MetSegRec(ByVal msr As HspfMetSegRecord.MetSegRecordType) As HspfMetSegRecord
		Get
			MetSegRec = pMetSegRecs(msr)
		End Get
    End Property

	Public Property Id() As Integer
		Get
			Id = pId
		End Get
		Set(ByVal Value As Integer)
			pId = Value
		End Set
    End Property

	Public Property AirType() As Integer
		Get
			AirType = pAirType
		End Get
		Set(ByVal Value As Integer)
			pAirType = Value
		End Set
    End Property

	Public Property Name() As String
		Get
			Name = pName
		End Get
		Set(ByVal Value As String)
			pName = Value
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

	Public Function Add(ByRef newConn As HspfConnection) As Boolean
		Dim lMetSegRec As HspfMetSegRecord
        Dim itype As Integer
		
		lMetSegRec = New HspfMetSegRecord
		If newConn.Target.VolName = "RCHRES" Then
			lMetSegRec.MFactR = newConn.MFact
		ElseIf newConn.Target.VolName = "PERLND" Or newConn.Target.VolName = "IMPLND" Then 
			lMetSegRec.MFactP = newConn.MFact
		End If
		lMetSegRec.Source = newConn.Source
		lMetSegRec.Tran = newConn.Tran
		lMetSegRec.Sgapstrg = newConn.Sgapstrg
		lMetSegRec.Ssystem = newConn.Ssystem
		lMetSegRec.Typ = Str2Type(newConn.Target.Member)
		
		Add = True
		
		If newConn.Target.VolName = "PERLND" Or newConn.Target.VolName = "IMPLND" Or newConn.Target.VolName = "RCHRES" Then
			itype = lMetSegRec.Typ
			If itype <> HspfMetSegRecord.MetSegRecordType.msrUNK Then
				If Len(pMetSegRecs(itype).Source.VolName) > 0 Then
					'dont add if already have this type of record
					Add = False
				Else
                    pMetSegRecs(itype) = Nothing
					pMetSegRecs(itype) = lMetSegRec
					If newConn.Target.Member = "GATMP" Then
						pAirType = 1
					ElseIf newConn.Target.Member = "AIRTMP" Then 
						pAirType = 2
					End If
				End If
			Else ' not needed
				Add = False
			End If
		Else
			Add = False
		End If
		
		If Not Add Then
            lMetSegRec = Nothing
		End If
		
    End Function

    Private Function Str2Type(ByRef aStr As String) As HspfMetSegRecord.MetSegRecordType
        Select Case aStr
            Case "PREC" : Str2Type = HspfMetSegRecord.MetSegRecordType.msrPREC
            Case "GATMP" : Str2Type = HspfMetSegRecord.MetSegRecordType.msrGATMP
            Case "AIRTMP" : Str2Type = HspfMetSegRecord.MetSegRecordType.msrGATMP
            Case "DTMPG" : Str2Type = HspfMetSegRecord.MetSegRecordType.msrDTMPG
            Case "DEWTMP" : Str2Type = HspfMetSegRecord.MetSegRecordType.msrDTMPG
            Case "WINMOV" : Str2Type = HspfMetSegRecord.MetSegRecordType.msrWINMOV
            Case "WIND" : Str2Type = HspfMetSegRecord.MetSegRecordType.msrWINMOV
            Case "SOLRAD" : Str2Type = HspfMetSegRecord.MetSegRecordType.msrSOLRAD
            Case "CLOUD" : Str2Type = HspfMetSegRecord.MetSegRecordType.msrCLOUD
            Case "PETINP" : Str2Type = HspfMetSegRecord.MetSegRecordType.msrPETINP
            Case "POTEV" : Str2Type = HspfMetSegRecord.MetSegRecordType.msrPOTEV
            Case Else : Str2Type = HspfMetSegRecord.MetSegRecordType.msrUNK
        End Select
    End Function
	
	Public Function Compare(ByRef newMetSeg As HspfMetSeg, ByRef opname As String) As Boolean
		Compare = True 'assume the best
		Dim itype As Integer
		Dim newMetSegRecs() As HspfMetSegRecord
		
		newMetSegRecs = VB6.CopyArray(newMetSeg.MetSegRecs)
		For itype = 1 To 8
			If Not (pMetSegRecs(itype).Compare(newMetSegRecs(itype), opname)) Then
				Compare = False
			End If
		Next itype
	End Function
	
	Public Sub UpdateMetSeg(ByRef newMetSeg As HspfMetSeg)
        For itype As Integer = 1 To 8
            With newMetSeg.MetSegRecs(itype)
                If pMetSegRecs(itype).MFactR = -999.0# And .MFactR <> -999.0# Then
                    pMetSegRecs(itype).MFactR = .MFactR
                    pMetSegRecs(itype).Sgapstrg = .Sgapstrg
                    pMetSegRecs(itype).Source = .Source
                    pMetSegRecs(itype).Ssystem = .Ssystem
                    pMetSegRecs(itype).Tran = .Tran
                    pMetSegRecs(itype).typ = .typ
                End If
            End With
        Next itype
    End Sub
	
    Public Sub New()
        MyBase.New()
        For lType As Integer = 1 To 8
            pMetSegRecs(lType) = New HspfMetSegRecord
        Next
        pAirType = 0
    End Sub
	
	Public Sub ExpandMetSegName(ByRef wdmid As String, ByRef idsn As Integer)
		Dim itype As Integer
        Dim addstr As String = ""
        Dim Con As String = ""

		Dim newMetSegRecs() As HspfMetSegRecord
		
        Me.Name = Me.Uci.GetWDMAttr(wdmid, idsn, "LOC")
		
		newMetSegRecs = VB6.CopyArray(Me.MetSegRecs)
		For itype = 1 To 8
			Select Case itype
				Case 1 : Con = "PREC"
				Case 2 : Con = "GATMP"
				Case 3 : Con = "DTMPG"
				Case 4 : Con = "WINMOV"
				Case 5 : Con = "SOLRAD"
				Case 6 : Con = "CLOUD"
				Case 7 : Con = "PETINP"
				Case 8 : Con = "POTEV"
			End Select
			If itype = 2 And Me.AirType = 2 Then
				Con = "AIRTMP"
			End If
			If pMetSegRecs(itype).MFactP <> 1 And pMetSegRecs(itype).MFactP <> 0 And pMetSegRecs(itype).MFactP <> -999 Then
                addstr &= ",PI:" & Con & "=" & CStr(pMetSegRecs(itype).MFactP)
			End If
			If pMetSegRecs(itype).MFactR <> 1 And pMetSegRecs(itype).MFactR <> 0 And pMetSegRecs(itype).MFactR <> -999 Then
                addstr &= ",R:" & Con & "=" & CStr(pMetSegRecs(itype).MFactR)
			End If
		Next itype
		
		If Len(addstr) > 0 Then
            Me.Name &= addstr
		End If
		
	End Sub
	
	Public Sub WriteUciFile(ByRef optyp As String, ByRef icol() As Integer, ByRef ilen() As Integer, ByRef f As Object)
		Dim lOpn As HspfOperation
        Dim j As Integer
        Dim lastj As Integer = pUci.OpnBlks.Item(optyp).Ids.Count
        Dim firstid, lastid As Integer
		
		firstid = 0
        lastid = 0
        For j = 1 To lastj
            lOpn = pUci.OpnBlks.Item(optyp).NthOper(j)
            If lOpn.MetSeg.Id = Me.Id Then
                If firstid = 0 Then
                    firstid = lOpn.Id
                Else
                    lastid = lOpn.Id
                End If
            ElseIf firstid > 0 Then
                WriteRecs(optyp, firstid, lastid, icol, ilen, f)
                firstid = 0
                lastid = 0
            End If
        Next j
		If firstid > 0 Then WriteRecs(optyp, firstid, lastid, icol, ilen, f)
		
	End Sub
	
	Public Sub WriteRecs(ByRef optyp As String, ByRef firstid As Integer, ByRef lastid As Integer, ByRef icol() As Integer, ByRef ilen() As Integer, ByRef f As Object)
        Dim lStr, t As String
		Dim segRec As Integer
        Dim tmember As String = ""
		
		For segRec = 1 To 8
			With pMetSegRecs(segRec)
				If .Typ <> 0 Then 'type exists
					If (optyp = "RCHRES" And .MFactR > 0#) Or (optyp = "PERLND" And .MFactP > 0#) Or (optyp = "IMPLND" And .MFactP > 0#) Then
						'have this type of met seg record
                        lStr = Trim(.Source.VolName)
                        lStr &= Space(icol(1) - Len(lStr) - 1) 'pad prev field
						t = Space(ilen(1)) 'right justify numbers
						t = RSet(CStr(.Source.VolId), Len(t))
                        lStr &= t
                        lStr &= Space(icol(2) - Len(lStr) - 1)
                        lStr &= .Source.Member
                        lStr &= Space(icol(3) - Len(lStr) - 1)
						If .Source.MemSub1 <> 0 Then
							t = Space(ilen(3))
							t = RSet(CStr(.Source.MemSub1), Len(t))
                            lStr &= t
						End If
                        lStr &= Space(icol(4) - Len(lStr) - 1)
                        lStr &= .Ssystem
                        lStr &= Space(icol(5) - Len(lStr) - 1)
                        lStr &= .Sgapstrg
                        lStr &= Space(icol(6) - Len(lStr) - 1)
						If optyp = "RCHRES" Then
							If .MFactR <> 1 Then
								t = Space(ilen(6))
								t = RSet(CStr(.MFactR), Len(t))
                                lStr &= t
							End If
						Else
							If .MFactP <> 1 Then
								t = Space(ilen(6))
								t = RSet(CStr(.MFactP), Len(t))
                                lStr &= t
							End If
						End If
                        lStr &= Space(icol(7) - Len(lStr) - 1)
                        lStr &= .Tran
                        lStr &= Space(icol(8) - Len(lStr) - 1)
                        lStr &= optyp
                        lStr &= Space(icol(9) - Len(lStr) - 1)
						t = Space(ilen(9))
						t = RSet(CStr(firstid), Len(t))
                        lStr &= t
						If lastid > 0 Then
                            lStr &= Space(icol(10) - Len(lStr) - 1)
							t = Space(ilen(9))
							t = RSet(CStr(lastid), Len(t))
                            lStr &= t
						End If
                        lStr &= Space(icol(11) - Len(lStr) - 1)
						If optyp <> "RCHRES" And pAirType = 2 And .Typ = 2 Then
                            lStr &= "ATEMP"
						Else
                            lStr &= "EXTNL"
						End If
                        lStr &= Space(icol(12) - Len(lStr) - 1)
						If optyp = "RCHRES" Then
							Select Case .Typ
								Case 1 : tmember = "PREC"
								Case 2 : tmember = "GATMP"
								Case 3 : tmember = "DEWTMP"
								Case 4 : tmember = "WIND"
								Case 5 : tmember = "SOLRAD"
								Case 6 : tmember = "CLOUD"
								Case 7 : tmember = "PETINP"
								Case 8 : tmember = "POTEV"
							End Select
						Else
							Select Case .Typ
								Case 1 : tmember = "PREC"
								Case 2 : tmember = "GATMP"
								Case 3 : tmember = "DTMPG"
								Case 4 : tmember = "WINMOV"
								Case 5 : tmember = "SOLRAD"
								Case 6 : tmember = "CLOUD"
								Case 7 : tmember = "PETINP"
								Case 8 : tmember = "POTEV"
							End Select
							If .Typ = 2 Then
								'get right air temp member name
								If pAirType = 1 Then
									tmember = "GATMP"
								ElseIf pAirType = 2 Then 
									tmember = "AIRTMP"
								End If
							End If
						End If
                        lStr &= tmember
                        lStr &= Space(icol(13) - Len(lStr) - 1)
						If segRec = 1 Then
                            PrintLine(f, "*** Met Seg " & pName)
						End If
                        PrintLine(f, lStr)
					End If
				End If
			End With
		Next segRec
		
	End Sub
End Class