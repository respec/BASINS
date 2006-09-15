Option Strict Off
Option Explicit On
<System.Runtime.InteropServices.ProgId("HspfSectionDef_NET.HspfSectionDef")> Public Class HspfSectionDef
    'Copyright 2006 AQUA TERRA Consultants - Royalty-free use permitted under open source license
	
	Dim pId As Integer
	Dim pName As String
	Dim pParent As HspfBlockDef
	Dim pTableDefs As Collection 'of HspfTableDef
	
	Public Property Name() As String
		Get
			Name = pName
		End Get
		Set(ByVal Value As String)
			pName = Value
		End Set
	End Property
	
	Public Property Id() As Integer
		Get
			Id = pId
		End Get
		Set(ByVal Value As Integer)
			pId = Value
		End Set
	End Property
	
	Public Property TableDefs() As Collection
		Get 'of HspfTableDef
			TableDefs = pTableDefs
		End Get
		Set(ByVal Value As Collection) 'of HspfTableDef
			pTableDefs = Value
		End Set
	End Property
End Class