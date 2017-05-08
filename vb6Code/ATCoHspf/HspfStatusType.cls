VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "HspfStatusType"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit
'Copyright 2002 AQUA TERRA Consultants - Royalty-free use permitted under open source license

Dim pName As String
Dim pOccur As Long
Dim pMax As Long
Dim pReqOptUnn As Long ' HspfStatusReqOptUnnEnum
Dim pPresent As Boolean ' HspfStatusPresentMissingEnum
Dim pTag As String
Dim pDefn As Object

Public Property Let Name(newName As String)
  pName = newName
End Property
Public Property Get Name() As String
  Name = pName
End Property

Public Property Let Occur(newOccur As Long)
  pOccur = newOccur
End Property
Public Property Get Occur() As Long
  Occur = pOccur
End Property

Public Property Let Max(newMax As Long)
  pMax = newMax
End Property
Public Property Get Max() As Long
  Max = pMax
End Property

Public Property Let ReqOptUnn(newReqOptUnn As Long)
  pReqOptUnn = newReqOptUnn
End Property
Public Property Get ReqOptUnn() As Long
  ReqOptUnn = pReqOptUnn
End Property

Public Property Let Present(newPresent As Boolean)
  pPresent = newPresent
End Property
Public Property Get Present() As Boolean
  Present = pPresent
End Property

Public Property Let Tag(newTag As String)
  pTag = newTag
End Property
Public Property Get Tag() As String
  Tag = pTag
End Property

Public Property Set Defn(newDefn As Object)
  Set pDefn = newDefn
End Property
Public Property Get Defn() As Object
  Set Defn = pDefn
End Property

Private Sub Class_Initialize()
  pName = ""
  pOccur = 0
  pReqOptUnn = 0
  pPresent = HspfStatusMissing
  pReqOptUnn = HspfStatusUnneeded
  Set pDefn = Nothing
End Sub
