VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "HspfSrcTar"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit
'Copyright 2002 AQUA TERRA Consultants - Royalty-free use permitted under open source license

Dim pOpn As HspfOperation
Dim pVolName As String
Dim pVolId As Long
Dim pVolIdL As Long
Dim pGroup As String
Dim pMember As String
Dim pMemSub1 As Long
Dim pMemSub2 As Long

Public Property Get Group() As String
  Group = pGroup
End Property
Public Property Let Group(newGroup As String)
  pGroup = newGroup
End Property

Public Property Get Member() As String
  Member = pMember
End Property
Public Property Let Member(newMember As String)
  pMember = newMember
End Property

Public Property Get MemSub1() As Long
  MemSub1 = pMemSub1
End Property
Public Property Let MemSub1(newMemSub1 As Long)
  pMemSub1 = newMemSub1
End Property

Public Property Get MemSub2() As Long
  MemSub2 = pMemSub2
End Property
Public Property Let MemSub2(newMemSub2 As Long)
  pMemSub2 = newMemSub2
End Property

Public Property Get VolName() As String
  VolName = pVolName
End Property
Public Property Let VolName(newVolName As String)
  pVolName = newVolName
End Property

Public Property Get VolId() As Long
  VolId = pVolId
End Property
Public Property Let VolId(newVolId As Long)
  pVolId = newVolId
End Property

Public Property Get Opn() As HspfOperation
  Set Opn = pOpn
End Property
Public Property Set Opn(newOpn As HspfOperation)
  Set pOpn = newOpn
  pVolName = pOpn.Name
  If pOpn.ID < pVolId Then
    pVolId = pOpn.ID
  End If
End Property

Public Property Get VolIdL() As Long
  VolIdL = pVolIdL
End Property
Public Property Let VolIdL(newVolIdL As Long)
  pVolIdL = newVolIdL
End Property
