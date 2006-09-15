Option Strict Off
Option Explicit On
<System.Runtime.InteropServices.ProgId("HspfMonthDataTable_NET.HspfMonthDataTable")> Public Class HspfMonthDataTable
    'Copyright 2006 AQUA TERRA Consultants - Royalty-free use permitted under open source license
	
	Dim pId As Integer
	Dim pMonthValues(12) As Double
	Dim pBlock As HspfMonthData
	Dim pReferencedBy As Collection 'of hspfoperation
	
	Public Property Id() As Integer
		Get
			Id = pId
		End Get
		Set(ByVal Value As Integer)
			pId = Value
		End Set
	End Property
	
	Public Property Block() As HspfMonthData
		Get
			Block = pBlock
		End Get
		Set(ByVal Value As HspfMonthData)
			pBlock = Value
		End Set
	End Property
	
	'UPGRADE_NOTE: Month was upgraded to Month_Renamed. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"'
	Public Property MonthValue(ByVal Month_Renamed As Integer) As Single
		Get
			MonthValue = pMonthValues(Month_Renamed)
		End Get
		Set(ByVal Value As Single)
			pMonthValues(Month_Renamed) = Value
		End Set
	End Property
	
	Public ReadOnly Property ReferencedBy() As Collection
		Get
			ReferencedBy = pReferencedBy
		End Get
	End Property
	
	'UPGRADE_NOTE: Class_Initialize was upgraded to Class_Initialize_Renamed. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"'
	Private Sub Class_Initialize_Renamed()
		pReferencedBy = New Collection
	End Sub
	Public Sub New()
		MyBase.New()
		Class_Initialize_Renamed()
	End Sub
End Class