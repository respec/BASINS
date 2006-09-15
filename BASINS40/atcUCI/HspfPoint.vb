Option Strict Off
Option Explicit On
<System.Runtime.InteropServices.ProgId("HspfPoint_NET.HspfPoint")> Public Class HspfPoint
    'Copyright 2006 AQUA TERRA Consultants - Royalty-free use permitted under open source license
	
	Dim pId As Integer
	Dim pName As String
	Dim pCon As String
	Dim pMFact As Double
	Dim pRFact As Double
	Dim pTran As String
	Dim pSgapstrg As String
	Dim pSsystem As String
	Dim pSource As HspfSrcTar
	Dim pTarget As HspfSrcTar
	Dim pAssocOper As Integer
	
	Public Property Id() As Integer
		Get
			Id = pId
		End Get
		Set(ByVal Value As Integer)
			pId = Value
		End Set
	End Property
	
	Public Property AssocOper() As Integer
		Get
			AssocOper = pAssocOper
		End Get
		Set(ByVal Value As Integer)
			pAssocOper = Value
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
	Public Property Con() As String
		Get
			Con = pCon
		End Get
		Set(ByVal Value As String)
			pCon = Value
		End Set
	End Property
	Public Property MFact() As Double
		Get
			MFact = pMFact
		End Get
		Set(ByVal Value As Double)
			pMFact = Value
		End Set
	End Property
	Public Property RFact() As Double
		Get
			RFact = pRFact
		End Get
		Set(ByVal Value As Double)
			pRFact = Value
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
	
	Public Sub New()
        MyBase.New()
        pSource = New HspfSrcTar
        pTarget = New HspfSrcTar
        pMFact = 1.0#
        pRFact = 1.0#
        pSgapstrg = ""
        pSsystem = ""
    End Sub
	
	Public Sub WriteUciFile(ByRef icol() As Integer, ByRef ilen() As Integer, ByRef f As Object)
        Dim lStr, t As String
		
        lStr = Trim(pSource.VolName)
        lStr &= Space(icol(1) - Len(lStr) - 1) 'pad prev field
		t = Space(ilen(1)) 'right justify numbers
		t = RSet(CStr(pSource.VolId), Len(t))
        lStr &= t
        lStr &= Space(icol(2) - Len(lStr) - 1)
        lStr &= pSource.Member
        lStr &= Space(icol(3) - Len(lStr) - 1)
		If pSource.MemSub1 <> 0 Then
			t = Space(ilen(3))
			t = RSet(CStr(pSource.MemSub1), Len(t))
            lStr &= t
		End If
        lStr &= Space(icol(4) - Len(lStr) - 1)
        lStr &= Me.Ssystem
        lStr &= Space(icol(5) - Len(lStr) - 1)
        lStr &= Me.Sgapstrg
        lStr &= Space(icol(6) - Len(lStr) - 1)
		If Me.MFact <> 1 Then
			t = Space(ilen(6))
			t = RSet(CStr(Me.MFact), Len(t))
            lStr &= t
		End If
        lStr &= Space(icol(7) - Len(lStr) - 1)
        lStr &= Me.Tran
        lStr &= Space(icol(8) - Len(lStr) - 1)
        lStr &= Me.Target.VolName
        lStr &= Space(icol(9) - Len(lStr) - 1)
		t = Space(ilen(9))
		If Me.Target.VolId > 0 And Me.Target.VolIdL > 0 Then
			'have a range of operations, just write the one for the assoc oper
			t = RSet(CStr(Me.AssocOper), Len(t))
		Else
			t = RSet(CStr(Me.Target.VolId), Len(t))
		End If
        lStr &= t
        lStr &= Space(icol(11) - Len(lStr) - 1)
        lStr &= Me.Target.Group
        lStr &= Space(icol(12) - Len(lStr) - 1)
        lStr &= Me.Target.Member
        lStr &= Space(icol(13) - Len(lStr) - 1)
		If Me.Target.MemSub1 > 0 Then
			t = Space(ilen(13))
			t = RSet(CStr(Me.Target.MemSub1), Len(t))
            If Me.Target.VolName = "RCHRES" Then t = Me.Target.Opn.Uci.IntAsCat(Me.Target.Member, 1, t)
            lStr &= t
            lStr &= Space(icol(14) - Len(lStr) - 1)
		End If
		If Me.Target.MemSub2 > 0 Then
			t = Space(ilen(14))
			t = RSet(CStr(Me.Target.MemSub2), Len(t))
            If Me.Target.VolName = "RCHRES" Then t = Me.Target.Opn.Uci.IntAsCat(Me.Target.Member, 2, t)
            lStr &= t
		End If
        PrintLine(f, lStr)
	End Sub
End Class