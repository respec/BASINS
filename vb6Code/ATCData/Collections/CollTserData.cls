VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "CollTserData"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit
'##MODULE_REMARKS Copyright 2001-3AQUA TERRA Consultants - Royalty-free use permitted under open source license

Private pColl As Collection
Private keyColl As Collection

Public Function Add(ByVal newValue As ATCclsTserData, Optional ByVal key As String) As String
  If IsMissing(key) Then
    pColl.Add newValue
  Else
    On Error GoTo RemoveIt
    pColl.Add newValue, key
    On Error Resume Next
    keyColl.Add key, key
  End If
  Add = newValue
  
  Exit Function
  
RemoveIt:
  If Err.Number = 457 Then ' Err.Description = "This key is already associated with an element of this collection"
    pColl.Remove key
    pColl.Add newValue, key
  Else
    MsgBox Err.Description & vbCr & "Collection Add(" & newValue & ", " & key & ")", vbExclamation, "Collection Add"
  End If
End Function

Public Sub Clear()
  Set pColl = Nothing
  Set pColl = New Collection
  Set keyColl = Nothing
  Set keyColl = New Collection
End Sub

Public Property Get Coll() As Collection
  Set Coll = pColl
End Property

Public Property Set Coll(newColl As Collection)
  Set pColl = newColl
End Property

Public Function Count() As Long
   Count = pColl.Count
End Function

Public Property Get Keys() As Collection
  Set Keys = keyColl
End Property

Public Sub Remove(ByVal key As Variant)
   pColl.Remove key
   keyColl.Remove key
End Sub

'default
Public Function Item(ByVal key As Variant) As ATCclsTserData
  On Error GoTo NoItem
  Item = pColl.Item(key)
  Exit Function
NoItem:
  Set Item = Nothing
End Function

' NewEnum must return the IUnknown interface of a
' collection's enumerator.
Public Function NewEnum() As IUnknown
   Set NewEnum = pColl.[_NewEnum]
End Function

Private Sub Class_Initialize()
  Clear
End Sub

