VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "clsVBitem"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = False
Option Explicit
Private pBody As String
Private pPath As String
Private pName As String
Private pItems As Collection
Private pNextLevelEntry As String

Public Property Get Body() As String
  Body = pBody
End Property
Public Property Let Body(newValue As String)
  pBody = newValue
End Property

Public Property Get Path() As String
  Path = pPath
End Property
Public Property Let Path(newValue As String)
  pPath = newValue
End Property

Public Property Get Name() As String
  Name = pName
End Property
Public Property Let Name(newValue As String)
  pName = newValue
End Property

'Public Property Get NextLevelEntry() As String
'  NextLevelEntry = pNextLevelEntry
'End Property
'Public Property Let NextLevelEntry(newValue As String)
'  pNextLevelEntry = newValue
'End Property

Public Property Get VBItype() As VBitemType
  VBItype = vbi_Misc
End Property

Public Property Get nItems() As Long
  nItems = pItems.Count
End Property

Public Property Get item(index As Long) As clsVBitem
  If index <= pItems.Count Then
    item = pItems(item)
  End If
End Property

Public Sub clear()
  Set pItems = Nothing
  Set pItems = New Collection
  pBody = ""
  pName = ""
End Sub

Private Sub Class_Initialize()
  clear
End Sub

