VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "HspfTSGroupDef"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit
'Copyright 2002 AQUA TERRA Consultants - Royalty-free use permitted under open source license

Dim pName As String
Dim pId As Long
Dim pBlockID As Long
Dim pMemberDefs As Collection 'of HspfTSMemberDef

Public Property Get Name() As String
  Name = pName
End Property
Public Property Let Name(newName As String)
  pName = newName
End Property

Public Property Get Id() As Long
  Id = pId
End Property
Public Property Let Id(newId As Long)
  pId = newId
End Property

Public Property Get BlockId() As Long
  BlockId = pBlockID
End Property
Public Property Let BlockId(newBlockId As Long)
  pBlockID = newBlockId
End Property

Public Property Get MemberDefs() As Collection 'of HspfTSMemberDef
  Set MemberDefs = pMemberDefs
End Property
Public Property Set MemberDefs(newMemberDefs As Collection) 'of HspfTSMemberDef
  Set pMemberDefs = newMemberDefs
End Property
