VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "HspfCategoryBlk"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit
'Copyright 2002 AQUA TERRA Consultants - Royalty-free use permitted under open source license

Dim pCategories As Collection 'of HspfCategory
Dim pUci As HspfUci
Dim pComment As String

Friend Property Get Uci() As HspfUci
  Set Uci = pUci
End Property
Friend Property Set Uci(newUci As HspfUci)
  Set pUci = newUci
End Property

Public Property Get Caption() As String
  Caption = "Category Block"
End Property

Public Property Get Comment() As String
  Comment = pComment
End Property

Public Property Let Comment(newComment As String)
  pComment = newComment
End Property

Public Property Get EditControlName() As String
  EditControlName = "ATCoHspf.ctlCategoryBlkEdit"
End Property

Public Sub Clear()
  Set pCategories = Nothing
  Set pCategories = New Collection
End Sub

Public Property Get Count() As Long
  Count = pCategories.Count
End Property

Public Sub Add(newValue As HspfCategory)
  pCategories.Add newValue
End Sub

Public Sub AddFromSpecs(newName$, Tag$)
  Dim newCategory As HspfCategory
  newCategory.Name = newName
  newCategory.Tag = Tag
  pCategories.Add newCategory
End Sub

Public Sub Remove(Index&)
  If Index > 0 And Index <= pCategories.Count Then
    pCategories.Remove (Index)
  End If
End Sub

Public Property Get Value(Index&) As HspfCategory
  If Index > 0 And Index <= pCategories.Count Then
    Value = pCategories(Index)
  Else
    Value.Name = ""
    Value.Tag = ""
  End If
End Property

Public Property Let Value(Index&, newValue As HspfCategory) '????
  If Index <= pCategories.Count Then
    pCategories.Remove Index
    pCategories.Add newValue, , Index
  ElseIf Index = pCategories.Count + 1 Then
    pCategories.Add newValue
  Else 'error?
  End If
End Property

Public Sub Edit()
  editInit Me, Me.Uci.icon, True
End Sub

Private Sub Class_Initialize()
  Set pCategories = New Collection
End Sub

Private Sub Update()
  pUci.Edited = True
End Sub

Public Function Check() As String
  'verify values are correct in relation to each other and other tables
  
End Function

Friend Sub ReadUciFile()
  Dim lCategory As HspfCategory, c$
  Dim retcod&, init&, OmCode&, cbuff$, retkey&, rectyp&
  
  On Error GoTo ErrHand
  
  If pUci.FastFlag Then
    GetCommentBeforeBlock "CATEGORY", pComment
  End If
  
  retcod = 0
  init = 1
  OmCode = HspfOmCode("CATEGORY")
  c = ""
  retkey = -1
  Do
    If pUci.FastFlag Then
      GetNextRecordFromBlock "CATEGORY", retkey, cbuff, rectyp, retcod
    Else
      retkey = -1
      Call REM_XBLOCKEX(Me.Uci, OmCode, init, retkey, cbuff, rectyp, retcod)
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
      pCategories.Add lCategory
      c = ""
    ElseIf rectyp = -1 And init = 0 Then 'dont save first comment, its the header
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
  MsgBox err.Description & vbCr & vbCr & cbuff, vbCritical, "Error in ReadUciFile"

End Sub
 
Friend Sub WriteUciFile(f As Integer)
  Dim vCategory As Variant, lCategory As HspfCategory
  
  If Len(pComment) > 0 Then
    Print #f, pComment
  End If
  Print #f, " "
  Print #f, "CATEGORY"
  Print #f, "   <> <----catnam----> *** "
  For Each vCategory In pCategories
    lCategory = vCategory
    If Len(lCategory.Comment) > 0 Then
      Print #f, lCategory.Comment
    End If
    Print #f, Space(3) & lCategory.Tag & Space(1) & lCategory.Name
  Next vCategory
  Print #f, "END CATEGORY"
End Sub




