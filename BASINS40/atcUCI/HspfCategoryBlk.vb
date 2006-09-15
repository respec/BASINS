Option Strict Off
Option Explicit On

Imports atcUtility

<System.Runtime.InteropServices.ProgId("HspfCategoryBlk_NET.HspfCategoryBlk")> Public Class HspfCategoryBlk
    'Copyright 2006 AQUA TERRA Consultants - Royalty-free use permitted under open source license
	
	Dim pCategories As Collection 'of HspfCategory
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
			Caption = "Category Block"
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
			EditControlName = "ATCoHspf.ctlCategoryBlkEdit"
		End Get
	End Property
	
	Public ReadOnly Property Count() As Integer
		Get
			Count = pCategories.Count()
		End Get
	End Property
	
	
	Public Property Value(ByVal Index As Integer) As HspfData.HspfCategory
		Get
			If Index > 0 And Index <= pCategories.Count() Then
                Value = pCategories.Item(Index)
            Else
                Value = New HspfData.HspfCategory
                Value.Name = ""
                Value.Tag = ""
			End If
		End Get
		Set(ByVal Value As HspfData.HspfCategory) '????
			If Index <= pCategories.Count() Then
				pCategories.Remove(Index)
				pCategories.Add(Value,  , Index)
			ElseIf Index = pCategories.Count() + 1 Then 
				pCategories.Add(Value)
			Else 'error?
			End If
		End Set
	End Property
	
	Public Sub Clear()
		'UPGRADE_NOTE: Object pCategories may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
		pCategories = Nothing
		pCategories = New Collection
	End Sub
	
	Public Sub Add(ByRef newValue As HspfData.HspfCategory)
		pCategories.Add(newValue)
	End Sub
	
	Public Sub AddFromSpecs(ByRef newName As String, ByRef Tag As String)
        Dim newCategory As New HspfData.HspfCategory
		newCategory.Name = newName
		newCategory.Tag = Tag
		pCategories.Add(newCategory)
	End Sub
	
	Public Sub Remove(ByRef Index As Integer)
		If Index > 0 And Index <= pCategories.Count() Then
			pCategories.Remove((Index))
		End If
	End Sub
	
	Public Sub Edit()
		editInit(Me, Me.Uci.icon, True)
	End Sub
	
	'UPGRADE_NOTE: Class_Initialize was upgraded to Class_Initialize_Renamed. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"'
	Private Sub Class_Initialize_Renamed()
		pCategories = New Collection
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
    '       Return ""
    'End Function
	
	Friend Sub ReadUciFile()
		Dim lCategory As HspfData.HspfCategory
		Dim c As String
		Dim retkey, init, retcod, OmCode, rectyp As Integer
		Dim cbuff As String
		
		On Error GoTo ErrHand
		
		If pUci.FastFlag Then
			GetCommentBeforeBlock("CATEGORY", pComment)
		End If
		
		retcod = 0
		init = 1
		OmCode = HspfOmCode("CATEGORY")
		c = ""
		retkey = -1
		Do 
			If pUci.FastFlag Then
				GetNextRecordFromBlock("CATEGORY", retkey, cbuff, rectyp, retcod)
			Else
				retkey = -1
				Call REM_XBLOCKEX((Me.Uci), OmCode, init, retkey, cbuff, rectyp, retcod)
			End If
			If retcod = 10 Then Exit Do
			If rectyp = 0 Then
				If Len(Trim(Mid(cbuff, 4, 2))) > 0 Then
					lCategory.Tag = StrRetRem(cbuff)
				Else
					lCategory.Tag = ""
				End If
				lCategory.Name = cbuff
				lCategory.Comment = c
				pCategories.Add(lCategory)
				c = ""
			ElseIf rectyp = -1 And init = 0 Then  'dont save first comment, its the header
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
        If Len(pComment) > 0 Then
            PrintLine(f, pComment)
        End If
		PrintLine(f, " ")
		PrintLine(f, "CATEGORY")
		PrintLine(f, "   <> <----catnam----> *** ")
        For Each lCategory As HspfData.HspfCategory In pCategories
            If Len(lCategory.Comment) > 0 Then
                PrintLine(f, lCategory.Comment)
            End If
            PrintLine(f, Space(3) & lCategory.Tag & Space(1) & lCategory.Name)
        Next
		PrintLine(f, "END CATEGORY")
	End Sub
End Class