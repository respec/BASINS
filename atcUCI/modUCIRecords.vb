Option Strict Off
Option Explicit On
Module modUCIRecords
	
	Dim uciRec() As String
	Dim uciRecCnt As Integer
	
	Public Sub ReadUCIRecords(ByRef pName As String)
		Dim i As Integer
		Dim s As String
		
		i = FreeFile()
		FileOpen(i, pName, OpenMode.Input)
		uciRecCnt = 0
		ReDim uciRec(500)
		Do Until EOF(i)
			s = LineInput(i)
			uciRecCnt = uciRecCnt + 1
			If UBound(uciRec) < uciRecCnt Then
				ReDim Preserve uciRec(uciRecCnt * 2)
			End If
			uciRec(uciRecCnt) = s
		Loop 
		ReDim Preserve uciRec(uciRecCnt)
		FileClose(i)
	End Sub
	
	Public Sub GetUCIRecord(ByRef i As Integer, ByRef s As String)
		s = uciRec(i)
	End Sub
	
	Public Sub GetNextRecordFromBlock(ByRef blockname As String, ByRef retkey As Integer, ByRef cbuff As String, ByRef rectyp As Integer, ByRef retcod As Integer)
		Dim i, ilen As Integer
		
		If retkey = -1 Then
			For i = 1 To uciRecCnt
				ilen = Len(blockname)
				If Len(uciRec(i)) >= ilen Then
					If Left(uciRec(i), ilen) = blockname Then
						'found start
						retkey = i
						Exit For
					End If
				End If
			Next i
			retcod = 10
		End If
		'start at retkey+1
		If retkey > -1 Then
			For i = retkey + 1 To uciRecCnt
				If Trim(uciRec(i)) = "END " & blockname Then
					retkey = 0
					retcod = 10
					Exit For
				End If
				If InStr(1, uciRec(i), "***") = 0 And Len(Trim(uciRec(i))) > 0 Then
					'found a real line of this block
					retkey = i
					cbuff = uciRec(i)
					rectyp = 0
					retcod = 2
					Exit For
				ElseIf InStr(1, uciRec(i), "***") > 0 Then 
					'found comment
					retkey = i
					cbuff = uciRec(i)
					rectyp = -1
					retcod = 2
					Exit For
				ElseIf Len(Trim(uciRec(i))) = 0 And blockname <> "FTABLES" Then 
					'found blank line
					retkey = i
					cbuff = ""
					rectyp = -2
					retcod = 2
					Exit For
				End If
			Next i
		End If
		If retkey = uciRecCnt Then
			retcod = 0
		End If
	End Sub
	
	Public Sub StartingRecordofOperationTable(ByRef opname As String, ByRef kwd As String, ByRef srec As Integer, ByRef noccur As Integer)
		Dim ostart, i, ilen, oend As Integer
		
		srec = 0
		noccur = 0
		ostart = 0
		For i = 1 To uciRecCnt
			ilen = Len(opname)
			If Len(uciRec(i)) >= ilen Then
				If Left(uciRec(i), ilen) = opname Then
					'found start of this operation type block
					ostart = i
					Exit For
				End If
			End If
		Next i
		
		oend = 0
		If ostart > 0 Then
			For i = ostart + 1 To uciRecCnt
				ilen = Len("END " & opname)
				If Len(uciRec(i)) >= ilen Then
					If Left(uciRec(i), ilen) = "END " & opname Then
						'found end of this operation type block
						oend = i
						Exit For
					End If
				End If
			Next i
		End If
		
		If ostart > 0 And oend > 0 Then
			For i = ostart + 1 To oend
				ilen = Len("  " & kwd)
				If Len(uciRec(i)) >= ilen And InStr(1, uciRec(i), "***") = 0 Then
					'If Left(uciRec(i), ilen) = "  " & kwd Then
					'pbd -- distinguish between soil-data and soil-data2 for instance
					If RTrim(Left(uciRec(i), ilen + 1)) = "  " & kwd Then
						'found start of this table
						If srec = 0 Then
							srec = i
						End If
						noccur = noccur + 1
					End If
				End If
			Next i
		End If
	End Sub
	
	Public Sub GetNextRecordFromTable(ByRef blockname As String, ByRef tablename As String, ByRef srec As Integer, ByRef initfg As Integer, ByRef noccur As Integer, ByRef cbuff As String, ByRef rectyp As Integer, ByRef retcod As Integer)
		Dim i, ilen As Integer
		
		If noccur > 1 And initfg = 1 Then
			'first time in, need to find start of the next one of these tables
			For i = srec + 1 To uciRecCnt
				ilen = Len("  " & tablename)
				If Len(uciRec(i)) >= ilen And InStr(1, uciRec(i), "***") = 0 Then
					If Left(uciRec(i), ilen) = "  " & tablename Then
						'found start of this table
						'pbd 9/04 always want next occur
						srec = i
						Exit For
					End If
				End If
			Next i
		End If
		
		For i = srec + 1 To uciRecCnt
			If RTrim(uciRec(i)) = "  END " & tablename Then
				retcod = 10
				Exit For
			End If
			If InStr(1, uciRec(i), "***") = 0 And Len(Trim(uciRec(i))) > 0 Then
				'found a real line of this block
				srec = i
				cbuff = uciRec(i)
				rectyp = 0
				retcod = 2
				Exit For
			ElseIf InStr(1, uciRec(i), "***") > 0 Then 
				'found comment
				srec = i
				cbuff = uciRec(i)
				rectyp = -1
				retcod = 3
				Exit For
			End If
		Next i
		
		If srec = uciRecCnt Then
			retcod = 0
		End If
	End Sub
	
	Public Sub GetCommentBeforeBlock(ByRef blockname As String, ByRef Comment As String)
		Dim ilen, i, retkey As Integer
		
		retkey = -1
		Comment = ""
		For i = 1 To uciRecCnt
			ilen = Len(blockname)
			If Len(uciRec(i)) >= ilen Then
				If Left(uciRec(i), ilen) = blockname Then
					'found start
					retkey = i
					Exit For
				End If
			End If
		Next i
		
		'start at retkey+1
		If retkey > -1 Then
			If Len(Trim(uciRec(retkey - 1))) = 0 Then
				'skip blank line immediately preceeding
				retkey = retkey - 1
			End If
			For i = retkey - 1 To 0 Step -1
				If Len(Trim(uciRec(i))) > 0 And InStr(1, uciRec(i), "***") = 0 Then
					'something on this line and its not a comment
					Exit For
				ElseIf InStr(1, uciRec(i), "***") > 0 Then 
					'found comment
					If Len(Comment) = 0 Then
						Comment = uciRec(i)
					Else
						Comment = uciRec(i) & vbCrLf & Comment
					End If
				ElseIf Len(Trim(uciRec(i))) = 0 Then 
					'found blank line
					If Len(Comment) = 0 Then
						Comment = " "
					Else
						Comment = " " & vbCrLf & Comment
					End If
				End If
			Next i
		End If
	End Sub
	
	Public Sub GetTableComment(ByRef srec As Integer, ByRef tabname As String, ByRef thisoccur As Integer, ByRef Comment As String)
        Dim retkey, i, noccur As Integer
		
		Comment = ""
		retkey = srec
		If thisoccur > 1 Then
			retkey = -1
			noccur = 1
			For i = srec + 1 To uciRecCnt
				If Trim(tabname) = Trim(uciRec(i)) Then
					'found another occurance
					noccur = noccur + 1
					If noccur = thisoccur Then
						retkey = i
					End If
					Exit For
				End If
			Next i
		End If
		
		'start at retkey+1
		If retkey > 0 Then
			'If Len(Trim(uciRec(retkey - 1))) = 0 Then
			'skip blank line immediately preceeding
			'  retkey = retkey - 1
			'End If
			For i = retkey - 1 To 0 Step -1
				If Len(Trim(uciRec(i))) > 0 And InStr(1, uciRec(i), "***") = 0 Then
					'something on this line and its not a comment
					Exit For
				ElseIf InStr(1, uciRec(i), "***") > 0 Then 
					'found comment
					If Len(Comment) = 0 Then
						Comment = uciRec(i)
					Else
						Comment = uciRec(i) & vbCrLf & Comment
					End If
				ElseIf Len(Trim(uciRec(i))) = 0 Then 
					'found blank line
					'dont tack it on if the preceeding line is real
					If Len(Trim(uciRec(i - 1))) > 0 And InStr(1, uciRec(i - 1), "***") = 0 Then
						'something on the preceeding line and its not a comment
						Exit For
					End If
					If Len(Comment) = 0 Then
						Comment = " "
					Else
						Comment = " " & vbCrLf & Comment
					End If
				End If
			Next i
		End If
	End Sub
	
	Public Sub SaveBlockOrder(ByRef cOrder As Collection)
		'default order is:
		'"GLOBAL"
		'"FILES"
		'"OPN SEQUENCE"
		'"MONTH DATA"
		'"PERLND"
		'"IMPLND"
		'"RCHRES"
		'"FTABLES"
		'"COPY"
		'"PLTGEN"
		'"DISPLY"
		'"DURANL"
		'"GENER"
		'"MUTSIN"
		'"BMPRAC"
		'"REPORT"
		'"CONNECTIONS"
		'"MASSLINKS"
		'"SPECIAL ACTIONS"
		Dim i, j As Integer
		Dim myOrder As New Collection
		Dim connfound As Boolean
		Dim bfound As Boolean
		Dim iprev As Integer
		
		For i = 1 To uciRecCnt
			If Trim(uciRec(i)) = "GLOBAL" Then
				myOrder.Add("GLOBAL")
			ElseIf Trim(uciRec(i)) = "FILES" Then 
				myOrder.Add("FILES")
			ElseIf Trim(uciRec(i)) = "OPN SEQUENCE" Then 
				myOrder.Add("OPN SEQUENCE")
			ElseIf Trim(uciRec(i)) = "MONTH-DATA" Then 
				myOrder.Add("MONTH DATA")
			ElseIf Trim(uciRec(i)) = "PERLND" Then 
				myOrder.Add("PERLND")
			ElseIf Trim(uciRec(i)) = "IMPLND" Then 
				myOrder.Add("IMPLND")
			ElseIf Trim(uciRec(i)) = "RCHRES" Then 
				myOrder.Add("RCHRES")
			ElseIf Trim(uciRec(i)) = "FTABLES" Then 
				myOrder.Add("FTABLES")
			ElseIf Trim(uciRec(i)) = "COPY" Then 
				myOrder.Add("COPY")
			ElseIf Trim(uciRec(i)) = "PLTGEN" Then 
				myOrder.Add("PLTGEN")
			ElseIf Trim(uciRec(i)) = "DISPLY" Then 
				myOrder.Add("DISPLY")
			ElseIf Trim(uciRec(i)) = "DURANL" Then 
				myOrder.Add("DURANL")
			ElseIf Trim(uciRec(i)) = "GENER" Then 
				myOrder.Add("GENER")
			ElseIf Trim(uciRec(i)) = "MUTSIN" Then 
				myOrder.Add("MUTSIN")
			ElseIf Trim(uciRec(i)) = "BMPRAC" Then 
				myOrder.Add("BMPRAC")
			ElseIf Trim(uciRec(i)) = "REPORT" Then 
				myOrder.Add("REPORT")
			ElseIf Trim(uciRec(i)) = "EXT SOURCES" And Not connfound Then 
				myOrder.Add("CONNECTIONS")
				connfound = True
			ElseIf Trim(uciRec(i)) = "EXT TARGETS" And Not connfound Then 
				myOrder.Add("CONNECTIONS")
				connfound = True
			ElseIf Trim(uciRec(i)) = "SCHEMATIC" And Not connfound Then 
				myOrder.Add("CONNECTIONS")
				connfound = True
			ElseIf Trim(uciRec(i)) = "NETWORK" And Not connfound Then 
				myOrder.Add("CONNECTIONS")
				connfound = True
			ElseIf Trim(uciRec(i)) = "MASS-LINK" Then 
				myOrder.Add("MASSLINKS")
			ElseIf Trim(uciRec(i)) = "SPEC-ACTIONS" Then 
				myOrder.Add("SPECIAL ACTIONS")
			End If
		Next i
		
		'see if there are any blocks in cOrder that aren't in myOrder
		iprev = 0
		For i = 1 To cOrder.Count()
			bfound = False
			For j = 1 To myOrder.Count()
                If myOrder.Item(j) = cOrder.Item(i) Then
                    'found it in myOrder
                    bfound = True
                    iprev = j
                    Exit For
                End If
			Next j
			If Not bfound Then
				'add this one so that myOrder has a complete set
				If iprev = 0 Then
					myOrder.Add(cOrder.Item(i),  , 1)
				Else
					'figure out where to put it
					myOrder.Add(cOrder.Item(i),  ,  , iprev)
					iprev = iprev + 1
				End If
			End If
		Next i
		'now pass back myOrder
		While cOrder.Count() > 0
			cOrder.Remove(1)
		End While
		For i = 1 To myOrder.Count()
			cOrder.Add(myOrder.Item(i))
		Next i
		
	End Sub
End Module