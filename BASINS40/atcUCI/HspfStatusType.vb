Option Strict Off
Option Explicit On
<System.Runtime.InteropServices.ProgId("HspfStatusType_NET.HspfStatusType")> Public Class HspfStatusType
    'Copyright 2006 AQUA TERRA Consultants - Royalty-free use permitted under open source license
	
	Dim pName As String
	Dim pOccur As Integer
	Dim pMax As Integer
	Dim pReqOptUnn As Integer ' HspfStatusReqOptUnnEnum
	Dim pPresent As Boolean ' HspfStatusPresentMissingEnum
	Dim pTag As String
	Dim pDefn As Object
	
	Public Property Name() As String
		Get
			Name = pName
		End Get
		Set(ByVal Value As String)
			pName = Value
		End Set
	End Property
	
	Public Property Occur() As Integer
		Get
			Occur = pOccur
		End Get
		Set(ByVal Value As Integer)
			pOccur = Value
		End Set
	End Property
	
	Public Property Max() As Integer
		Get
			Max = pMax
		End Get
		Set(ByVal Value As Integer)
			pMax = Value
		End Set
	End Property
	
	Public Property ReqOptUnn() As Integer
		Get
			ReqOptUnn = pReqOptUnn
		End Get
		Set(ByVal Value As Integer)
			pReqOptUnn = Value
		End Set
	End Property
	
	Public Property Present() As Boolean
		Get
			Present = pPresent
		End Get
		Set(ByVal Value As Boolean)
			pPresent = Value
		End Set
	End Property
	
	Public Property Tag() As String
		Get
			Tag = pTag
		End Get
		Set(ByVal Value As String)
			pTag = Value
		End Set
	End Property
	
	Public Property Defn() As Object
		Get
			Defn = pDefn
		End Get
		Set(ByVal Value As Object)
			pDefn = Value
		End Set
	End Property
	
	Public Sub New()
        MyBase.New()
        pName = ""
        pOccur = 0
        pReqOptUnn = 0
        pPresent = HspfStatus.HspfStatusPresentMissingEnum.HspfStatusMissing
        pReqOptUnn = HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusUnneeded
    End Sub
End Class