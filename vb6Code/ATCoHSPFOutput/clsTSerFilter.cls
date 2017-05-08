VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "clsTSerFilter"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit
'Copyright 2002 by AQUA TERRA Consultants
Dim pCriteria As FastCollection
Dim pParent As Object
Dim pFilterTypes As Variant

Public Property Get Criteria() As FastCollection
  Set Criteria = pCriteria
End Property
Public Property Set Criteria(newCriteria As FastCollection)
  Set pCriteria = newCriteria
End Property

Public Property Get FilterTypes() As Variant
  FilterTypes = pFilterTypes
End Property
Public Property Set FilterTypes(newFilterTypes As Variant)
  pFilterTypes = newFilterTypes
End Property

Public Property Get Parent() As Object
  Set Parent = pParent
End Property
Public Property Set Parent(newParent As Object)
  Set pParent = newParent
End Property

Public Sub ShowFilterEdit(icon As Object) 'should this be a ATCclsTserFile property or function?
  Dim f As frmFilterEdit
  
  Set f = New frmFilterEdit
  Set f.Parent = Me
  Set f.FilterTypes = pFilterTypes
  Set f.icon = icon
  f.Show vbModal
End Sub

Public Function Filter(ts As Collection) As Collection
  Dim vTs As Variant, vCriteria As Variant
  Dim hide As Boolean
  Dim lTs As ATCclsTserData
  Dim c As Collection
  Dim name As String
  Dim value As String
  
  Set c = Nothing
  Set c = New Collection
  
  For Each vTs In ts
    Set lTs = vTs
    hide = False
    For Each vCriteria In pCriteria
      value = vCriteria
      name = StrSplit(value, ":", "")
      If lTs.attrib(name) = value Then
        hide = True
        Exit For
      End If
    Next
    If Not (hide) Then
      c.Add lTs
    End If
  Next
  Set Filter = c
End Function

Public Sub changeCriteria(name As String, value As String, Optional Add As Boolean = True)
  Dim lCriteria As String
  
  lCriteria = name & ":" & value
  If Add Then
    If Not (pCriteria.KeyExists(lCriteria)) Then
      pCriteria.Add lCriteria, lCriteria
    End If
  Else
    If pCriteria.KeyExists(lCriteria) Then
      pCriteria.RemoveByKey lCriteria
    End If
  End If
End Sub

Private Sub Class_Initialize()
  Set pCriteria = New FastCollection
End Sub
