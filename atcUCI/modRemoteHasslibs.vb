Option Strict Off
Option Explicit On

Imports atcUtility

Module modRemoteHasslibs
	
	Public Sub REM_GLOBLK(ByRef myUci As HspfUci, ByRef sdatim() As Integer, ByRef edatim() As Integer, ByRef outlev As Integer, ByRef spout As Integer, ByRef runfg As Integer, ByRef emfg As Integer, ByRef rninfo As String)
		Dim M As String
		Dim i As Integer
		
		'UPGRADE_WARNING: Couldn't resolve default property of object myUci.Monitor.SendProcessMessage. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		myUci.Monitor.SendProcessMessage("HSPFUCI", "GLOBLK")
		M = myUci.WaitForChildMessage
		For i = 0 To 5
			sdatim(i) = CInt(StrRetRem(M))
		Next 
		For i = 0 To 5
			edatim(i) = CInt(StrRetRem(M))
		Next 
		outlev = CInt(StrRetRem(M))
		spout = CInt(StrRetRem(M))
		runfg = CInt(StrRetRem(M))
		emfg = CInt(StrRetRem(M))
		rninfo = M
	End Sub
	
	Public Sub REM_GLOPRMI(ByRef myUci As HspfUci, ByRef ival As Integer, ByRef parmname As String)
		Dim M As String

		'UPGRADE_WARNING: Couldn't resolve default property of object myUci.Monitor.SendProcessMessage. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		myUci.Monitor.SendProcessMessage("HSPFUCI", "GLOPRMI " & parmname)
		M = myUci.WaitForChildMessage
		ival = -999
		If Len(Trim(M)) > 0 Then
			If IsNumeric(M) Then
				ival = CShort(M)
			End If
		End If
	End Sub
	
	Public Sub REM_XBLOCK(ByRef myUci As HspfUci, ByRef blkno As Integer, ByRef init As Integer, ByRef retkey As Integer, ByRef cbuff As String, ByRef retcod As Integer)
		Dim M As String
		Dim i As Integer
		
		'UPGRADE_WARNING: Couldn't resolve default property of object myUci.Monitor.SendProcessMessage. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		myUci.Monitor.SendProcessMessage("HSPFUCI", "XBLOCK " & blkno & " " & init)
		M = myUci.WaitForChildMessage
		'Debug.Print blkno & "=" & M
		blkno = CInt(StrRetRem(M))
		init = CInt(StrRetRem(M))
		retkey = CInt(StrRetRem(M))
		i = InStr(M, " ")
		retcod = CInt(Left(M, i - 1))
		cbuff = Right(M, Len(M) - i)
	End Sub
	
	Public Sub REM_XBLOCKEX(ByRef myUci As HspfUci, ByRef blkno As Integer, ByRef init As Integer, ByRef retkey As Integer, ByRef cbuff As String, ByRef rectyp As Integer, ByRef retcod As Integer)
		Dim M As String
		Dim i As Integer
		
		'UPGRADE_WARNING: Couldn't resolve default property of object myUci.Monitor.SendProcessMessage. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		myUci.Monitor.SendProcessMessage("HSPFUCI", "XBLOCKEX " & blkno & " " & init & " " & retkey)
		M = myUci.WaitForChildMessage
		Debug.Print(blkno & "=" & M)
		blkno = CInt(StrRetRem(M))
		init = CInt(StrRetRem(M))
		retkey = CInt(StrRetRem(M))
		rectyp = CInt(StrRetRem(M))
		i = InStr(M, " ")
		retcod = CInt(Left(M, i - 1))
		cbuff = Right(M, Len(M) - i)
	End Sub
	
	Public Sub REM_GTNXKW(ByRef myUci As HspfUci, ByRef init As Integer, ByRef Id As Integer, ByRef ckwd As String, ByRef kwdfg As Integer, ByRef contfg As Integer, ByRef retid As Integer)
		Dim M As String
		
		'UPGRADE_WARNING: Couldn't resolve default property of object myUci.Monitor.SendProcessMessage. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		myUci.Monitor.SendProcessMessage("HSPFUCI", "GTNXKW " & init & " " & Id)
		M = myUci.WaitForChildMessage
		init = CInt(StrRetRem(M))
		Id = CInt(StrRetRem(M))
		kwdfg = CInt(StrRetRem(M))
		contfg = CInt(StrRetRem(M))
		retid = CInt(StrRetRem(M))
		ckwd = M
	End Sub
	
	Public Sub REM_GETOCR(ByRef myUci As HspfUci, ByRef itype As Integer, ByRef noccur As Integer)
		Dim M As String
		
		'UPGRADE_WARNING: Couldn't resolve default property of object myUci.Monitor.SendProcessMessage. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		myUci.Monitor.SendProcessMessage("HSPFUCI", "GETOCR " & itype)
		M = myUci.WaitForChildMessage
		itype = CInt(StrRetRem(M))
		noccur = CInt(M)
	End Sub
	
	Public Sub REM_XTABLEEX(ByRef myUci As HspfUci, ByRef OmCode As Integer, ByRef tabno As Integer, ByRef uunits As Integer, ByRef init As Integer, ByRef addfg As Integer, ByRef Occur As Integer, ByRef retkey As Integer, ByRef cbuff As String, ByRef rectyp As Integer, ByRef retcod As Integer)
		Dim M As String
		Dim i As Integer
		
		'UPGRADE_WARNING: Couldn't resolve default property of object myUci.Monitor.SendProcessMessage. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		myUci.Monitor.SendProcessMessage("HSPFUCI", "XTABLEEX " & OmCode & " " & tabno & " " & uunits & " " & init & " " & addfg & " " & Occur & " " & retkey)
		M = myUci.WaitForChildMessage
		retkey = CInt(StrRetRem(M))
		rectyp = CInt(StrRetRem(M))
		i = InStr(M, " ")
		retcod = CInt(Left(M, i - 1))
		cbuff = Right(M, Len(M) - i)
		
	End Sub
End Module