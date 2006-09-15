Option Strict Off
Option Explicit On
<System.Runtime.InteropServices.ProgId("HspfSpecialRecord_NET.HspfSpecialRecord")> Public Class HspfSpecialRecord
    'Copyright 2006 AQUA TERRA Consultants - Royalty-free use permitted under open source license
	
	Dim pText As String
	Dim pSpecType As HspfData.HspfSpecialRecordType
	
	Public Property Text() As String
		Get
			Text = pText
		End Get
		Set(ByVal Value As String)
			pText = Value
		End Set
	End Property
	
	Public Property SpecType() As HspfData.HspfSpecialRecordType
		Get
			SpecType = pSpecType
		End Get
		Set(ByVal Value As HspfData.HspfSpecialRecordType)
			pSpecType = Value
		End Set
	End Property
End Class