Option Strict Off
Option Explicit On

Imports atcUtility

<System.Runtime.InteropServices.ProgId("HspfFilesBlk_NET.HspfFilesBlk")> Public Class HspfFilesBlk
    'Copyright 2006 AQUA TERRA Consultants - Royalty-free use permitted under open source license
	
	Dim pFiles As Collection 'of HspfFile
	Dim pUci As HspfUci
	Dim pComment As String
	
	Friend Property Uci() As HspfUci
		Get
			Uci = pUci
		End Get
		Set(ByVal Value As HspfUci)
			pUci = Value
		End Set
	End Property
	
	Public ReadOnly Property Caption() As String
		Get
			Caption = "Files Block"
		End Get
	End Property
	
	
	Public Property Comment() As String
		Get
			Comment = pComment
		End Get
		Set(ByVal Value As String)
			pComment = Value
		End Set
	End Property
	
	Public ReadOnly Property EditControlName() As String
		Get
			EditControlName = "ATCoHspf.ctlFilesBlkEdit"
		End Get
	End Property
	
	Public ReadOnly Property Count() As Integer
		Get
			Count = pFiles.Count()
		End Get
	End Property
	
	Public Property Value(ByVal Index As Integer) As HspfData.HspfFile
		Get
			If Index > 0 And Index <= pFiles.Count() Then
                Value = pFiles.Item(Index)
            Else
                Value = New HspfData.HspfFile
                Value.Name = ""
                Value.Typ = ""
                Value.Unit = 0
			End If
		End Get
		Set(ByVal Value As HspfData.HspfFile) '????
			If Index <= pFiles.Count() Then
				pFiles.Remove(Index)
				pFiles.Add(Value,  , Index)
				'Set pFiles(Index) = newValue
			ElseIf Index = pFiles.Count() + 1 Then 
				pFiles.Add(Value)
			Else 'error?
			End If
		End Set
	End Property
	
	Public Sub Clear()
		'UPGRADE_NOTE: Object pFiles may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
		pFiles = Nothing
		pFiles = New Collection
	End Sub
	
	Public Sub Add(ByRef newValue As HspfData.HspfFile) 'to end, how about in between
		pFiles.Add(newValue)
	End Sub
	
	Public Sub AddFromSpecs(ByRef newName As String, ByRef wid As String)
		Dim iunit As Integer
        Dim vfile As HspfData.HspfFile
		Dim ifound As Boolean
        Dim newFile As New HspfData.HspfFile
		newFile.Name = newName
		newFile.Typ = wid
		'find available unit
		iunit = 25
		ifound = True
        While Not ifound
            iunit = iunit + 1
            ifound = False
            For Each vfile In pFiles
                If iunit = vfile.Unit Then
                    ifound = True
                End If
            Next vfile
        End While
		newFile.Unit = iunit
		pFiles.Add(newFile)
	End Sub
	
	Public Sub AddFromSpecsExt(ByRef newName As String, ByRef wid As String, ByRef Unit As Integer)
		Dim iunit As Integer
        Dim vfile As HspfData.HspfFile
		Dim ifound As Boolean
        Dim newFile As New HspfData.HspfFile
		newFile.Name = newName
		newFile.Typ = wid
		'find available unit
		iunit = Unit - 1
		ifound = True
		Do Until ifound = False
			iunit = iunit + 1
			ifound = False
			For	Each vfile In pFiles
                If iunit = vfile.Unit Then
                    ifound = True
                End If
			Next vfile
		Loop 
		newFile.Unit = iunit
		pFiles.Add(newFile)
	End Sub
	
	Public Sub Remove(ByRef Index As Integer)
		If Index > 0 And Index <= pFiles.Count() Then
			pFiles.Remove((Index))
		End If
	End Sub
	
	Public Sub SetTyp(ByRef Index As Integer, ByRef wid As String)
		Dim lFile As HspfData.HspfFile
        If Index > 0 And Index <= pFiles.Count() Then
            Dim lExistingFile As HspfData.HspfFile = pFiles.Item(Index)
            lFile.Typ = wid
            lFile.Comment = lExistingFile.Comment
            lFile.Name = lExistingFile.Name
            lFile.Unit = lExistingFile.Unit
            pFiles.Remove(Index)
            pFiles.Add(lFile)
        End If
	End Sub
	
	Public Sub Edit()
		editInit(Me, Me.Uci.icon, True)
	End Sub
	
	'UPGRADE_NOTE: Class_Initialize was upgraded to Class_Initialize_Renamed. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"'
	Private Sub Class_Initialize_Renamed()
		pFiles = New Collection
	End Sub
	Public Sub New()
		MyBase.New()
		Class_Initialize_Renamed()
	End Sub
	
	Private Sub Update()
		pUci.Edited = True
	End Sub
	
    'Public Function Check() As String
    '	'verify values are correct in relation to each other and other tables
    'End Function
	
	Friend Sub ReadUciFile()
		Dim lFile As HspfData.HspfFile
		Dim c As String
		Dim retkey, init, retcod, OmCode, rectyp As Integer
		Dim cbuff As String
		
		On Error GoTo ErrHand
		
		If pUci.FastFlag Then
			GetCommentBeforeBlock("FILES", pComment)
		End If
		
		retcod = 0
		init = 1
		OmCode = HspfOmCode("FILES")
		c = ""
		retkey = -1
		Do 
			If pUci.FastFlag Then
				GetNextRecordFromBlock("FILES", retkey, cbuff, rectyp, retcod)
			Else
				retkey = -1
				Call REM_XBLOCKEX((Me.Uci), OmCode, init, retkey, cbuff, rectyp, retcod)
			End If
			If retcod = 10 Then Exit Do
			If rectyp = 0 Then
				If Len(Trim(Left(cbuff, 6))) > 0 Then
					lFile.Typ = StrRetRem(cbuff)
				Else
					lFile.Typ = ""
				End If
				lFile.Unit = CInt(StrRetRem(cbuff))
				lFile.Name = cbuff
				lFile.Comment = c
				pFiles.Add(lFile)
				c = ""
			ElseIf rectyp = -1 Then 
				'save comment
				If Len(c) = 0 Then
					c = cbuff
				Else
					c = c & vbCrLf & cbuff
				End If
			ElseIf retcod = 2 And rectyp = -2 Then 
				'save blank line
				If Len(c) = 0 Then
					c = " "
				Else
					c = c & vbCrLf & " "
				End If
			End If
			init = 0
		Loop 
		
		Exit Sub
		
ErrHand: 
		MsgBox(Err.Description & vbCr & vbCr & cbuff, MsgBoxStyle.Critical, "Error in ReadUciFile")
		
	End Sub
	
	Friend Sub WriteUciFile(ByRef f As Short)
        Dim lFile As HspfData.HspfFile
		Dim tname As String
		Dim tpath As String
		
		If Len(pComment) > 0 Then
			PrintLine(f, pComment)
		End If
		PrintLine(f, " ")
		PrintLine(f, "FILES")
		If pFiles.Count() > 0 Then
            If Len(pFiles.Item(1).Comment) = 0 Then
                'need to add header
                PrintLine(f, "<FILE>  <UN#>***<----FILE NAME------------------------------------------------->")
            End If
		End If
        For Each lFile In pFiles
            tname = lFile.Name
            If InStr(1, tname, ":") Then
                'this is the absolute path name, make relative
                'tpath = CurDir
                tpath = IO.Path.GetDirectoryName(Me.Uci.Name)
                tname = RelativeFilename(tname, tpath)
                lFile.Name = tname
            End If
            If Len(lFile.Comment) > 0 Then
                PrintLine(f, lFile.Comment)
            End If
            PrintLine(f, lFile.Typ & Space(10 - Len(lFile.Typ)) & myFormatI(lFile.Unit, 3), Space(2) & tname)
        Next
		PrintLine(f, "END FILES")
	End Sub
	
	Public Sub newName(ByRef oldn As String, ByRef newn As String)
		Dim l, k, i, j, islash, itmp As Integer
		Dim tempn As String
		Dim lHspfFile As HspfData.HspfFile
		Dim lFiles As Collection

		lFiles = New Collection
        For Each lHspfFile In pFiles
            If Trim(lHspfFile.Typ) = "MESSU" Or Trim(lHspfFile.Typ) = "" Or Trim(lHspfFile.Typ) = "BINO" Then
                'Close lFile.Unit
                'replace file name
                tempn = lHspfFile.Name
                l = Len(oldn)
                itmp = InStr(1, UCase(tempn), UCase(oldn))
                j = Len(tempn)
                islash = 0
                For i = 1 To j
                    'check for a path in the name
                    If (Mid(tempn, i, 1) = "\") Then
                        islash = i
                    End If
                Next i
                If ((itmp > 0 And islash > 0 And itmp > islash) Or (itmp > 0 And islash = 0)) Then
                    'found the old name in this string, replace it
                    j = Len(newn)
                    lHspfFile.Name = Mid(tempn, 1, itmp - 1) & newn & Mid(tempn, itmp + l)
                Else
                    'just add the new scen name
                    k = Len(newn)
                    If islash = 0 Then
                        'no path
                        lHspfFile.Name = newn & "." & tempn
                    Else
                        'have a path name, insert after slash
                        lHspfFile.Name = Mid(tempn, 1, islash) & newn & "." & Mid(tempn, islash + 1, j)
                    End If
                End If
            End If
            lFiles.Add(lHspfFile)
        Next
		
        pFiles = Nothing
        pFiles = lFiles
	End Sub
End Class