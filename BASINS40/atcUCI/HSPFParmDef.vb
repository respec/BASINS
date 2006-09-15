Option Strict Off
Option Explicit On
<System.Runtime.InteropServices.ProgId("HSPFParmDef_NET.HSPFParmDef")> Public Class HSPFParmDef
	'##MODULE_SUMMARY Class containing definition of a model parameter.
	'##MODULE_REMARKS Copyright 2001-3AQUA TERRA Consultants - Royalty-free use permitted under open source license
	
	Dim pName As String
	Dim pTyp As Integer 'atcoDataType is in control
	Dim pMin As Double
	Dim pMax As Double
	Dim pDefault As String
	Dim pMetricMin As Double
	Dim pMetricMax As Double
	Dim pMetricDefault As String
	Dim pSoftMin As Double
	Dim pSoftMax As Double
	Dim pDefine As String
	Dim pOther As String
	Dim pStartCol As Integer
	Dim pLength As Integer
	Dim pParent As Object
	
	'##SUMMARY Name of parameter.
	Public Property Name() As String
		Get
			Name = pName
		End Get
		Set(ByVal Value As String)
			pName = Value
		End Set
	End Property
	
	'##SUMMARY Type of parameter.
	Public Property Typ() As Integer
		Get
			Typ = pTyp
		End Get
		Set(ByVal Value As Integer)
			pTyp = Value
		End Set
	End Property
	
	'##SUMMARY Minimum value for parameter.
	Public Property Min() As Double
		Get
			Min = pMin
		End Get
		Set(ByVal Value As Double)
			pMin = Value
		End Set
	End Property
	Public Property MetricMin() As Double
		Get
			MetricMin = pMetricMin
		End Get
		Set(ByVal Value As Double)
			pMetricMin = Value
		End Set
	End Property
	
	'##SUMMARY Maximum value for parameter
	Public Property Max() As Double
		Get
			Max = pMax
		End Get
		Set(ByVal Value As Double)
			pMax = Value
		End Set
	End Property
	Public Property MetricMax() As Double
		Get
			MetricMax = pMetricMax
		End Get
		Set(ByVal Value As Double)
			pMetricMax = Value
		End Set
	End Property
	
	'##SUMMARY Devault value for parameter.
	'UPGRADE_NOTE: Default was upgraded to Default_Renamed. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"'
	Public Property Default_Renamed() As String
		Get
			Default_Renamed = pDefault
		End Get
		Set(ByVal Value As String)
			pDefault = Value
		End Set
	End Property
	Public Property MetricDefault() As String
		Get
			MetricDefault = pMetricDefault
		End Get
		Set(ByVal Value As String)
			pMetricDefault = Value
		End Set
	End Property
	
	'##SUMMARY Recommended minimum value for parameter.
	Public Property SoftMin() As Double
		Get
			SoftMin = pSoftMin
		End Get
		Set(ByVal Value As Double)
			pSoftMin = Value
		End Set
	End Property
	
	'##SUMMARY Recommended maximum value for parameter.
	Public Property SoftMax() As Double
		Get
			SoftMax = pSoftMax
		End Get
		Set(ByVal Value As Double)
			pSoftMax = Value
		End Set
	End Property
	
	'##SUMMARY Text definition of parameter.
	Public Property Define() As String
		Get
			Define = pDefine
		End Get
		Set(ByVal Value As String)
			pDefine = Value
		End Set
	End Property
	
	'##SUMMARY Parent object of parameter definition.
	Public Property Parent() As Object
		Get
			Parent = pParent
		End Get
		Set(ByVal Value As Object)
			pParent = Value
		End Set
	End Property
	
	'##SUMMARY Additional information about parameter.
	Public Property Other() As String
		Get
			Other = pOther
		End Get
		Set(ByVal Value As String)
			pOther = Other
		End Set
	End Property
	
	'##SUMMARY Starting column for parameter within a text string.
	Public Property StartCol() As Integer
		Get
			StartCol = pStartCol
		End Get
		Set(ByVal Value As Integer)
			pStartCol = Value
		End Set
	End Property
	
	'##SUMMARY Length of parameter value when stored as a string.
	Public Property Length() As Integer
		Get
			Length = pLength
		End Get
		Set(ByVal Value As Integer)
			pLength = Value
		End Set
	End Property
	
	'UPGRADE_NOTE: Class_Initialize was upgraded to Class_Initialize_Renamed. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"'
	Private Sub Class_Initialize_Renamed()
		'Debug.Print "init HSPFParmDef"
	End Sub
	Public Sub New()
		MyBase.New()
		Class_Initialize_Renamed()
	End Sub
End Class