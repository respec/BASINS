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
	
	'UPGRADE_NOTE: Class_Initialize was upgraded to Class_Initialize_Renamed. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"'
	Private Sub Class_Initialize_Renamed()
		pName = ""
		pOccur = 0
		pReqOptUnn = 0
		pPresent = HspfStatus.HspfStatusPresentMissingEnum.HspfStatusMissing
		pReqOptUnn = HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusUnneeded
		'UPGRADE_NOTE: Object pDefn may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
		pDefn = Nothing
	End Sub
	Public Sub New()
		MyBase.New()
		Class_Initialize_Renamed()
	End Sub
End Class