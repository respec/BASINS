Option Strict Off
Option Explicit On
Module HspfAddChar2Keyword
    'Copyright 2006 AQUA TERRA Consultants - Royalty-free use permitted under open source license
	
	Public lastOperationSerial As Integer
	Public IPC As ATCoCtl.ATCoIPC
	Public IPCset As Boolean
	
	Function AddChar2Keyword(ByRef k As String) As String
		Dim kwd As String
		
		kwd = k
		Select Case kwd
			Case "MON-IFLW-CON" : kwd = kwd & "C"
			Case "MON-GRND-CON" : kwd = kwd & "C"
			Case "PEST-AD-FLAG" : kwd = kwd & "S"
			Case "PHOS-AD-FLAG" : kwd = kwd & "S"
			Case "TRAC-AD-FLAG" : kwd = kwd & "S"
			Case "PLNK-AD-FLAG" : kwd = kwd & "S"
			Case "HYDR-CATEGOR" : kwd = kwd & "Y"
			Case Else
		End Select
		AddChar2Keyword = kwd
	End Function
End Module