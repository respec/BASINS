VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "HspfTableDef"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit
'Copyright 2002 AQUA TERRA Consultants - Royalty-free use permitted under open source license

Dim pId As Long
Dim pName As String
Dim pDefine As String
Dim pHeaderE As String
Dim pHeaderM As String
Dim pNumOccur As Long
Dim pOccurGroup As Long
Dim pParent As HspfSectionDef
Dim pParmDefs As Collection 'of HSPFParm
Dim pSGRP As Long

Public Property Get Define() As String
  Define = pDefine
End Property
Public Property Let Define(newDefine As String)
  pDefine = ReplaceString(newDefine, vbCrLf, " ")
End Property

Public Property Get HeaderM() As String
  HeaderM = pHeaderM
End Property
Public Property Let HeaderM(newHeader As String)
  pHeaderM = newHeader
End Property

Public Property Get HeaderE() As String
  HeaderE = pHeaderE
End Property
Public Property Let HeaderE(newHeader As String)
  pHeaderE = newHeader
End Property

Public Property Get Id() As Long
  Id = pId
End Property
Public Property Let Id(newId As Long)
  pId = newId
End Property

Public Property Get Name() As String
Attribute Name.VB_MemberFlags = "200"
  Name = pName
End Property
Public Property Let Name(newName As String)
  pName = newName
End Property

Public Property Get NumOccur() As Long
  NumOccur = pNumOccur
End Property
Public Property Let NumOccur(newNumOccur As Long)
  pNumOccur = newNumOccur
End Property

Public Property Get OccurGroup() As Long
  OccurGroup = pOccurGroup
End Property
Public Property Let OccurGroup(newOccurGroup As Long)
  pOccurGroup = newOccurGroup
End Property

Public Property Get Parent() As HspfSectionDef
  Set Parent = pParent
End Property
Public Property Set Parent(newParent As HspfSectionDef)
  Set pParent = newParent
End Property

Public Property Get ParmDefs() As Collection 'of HSPFParm
  Set ParmDefs = pParmDefs
End Property
Public Property Set ParmDefs(newParmDef As Collection) 'of HSPFParm
  Set pParmDefs = newParmDef
End Property

Public Property Get SGRP() As Long
  SGRP = pSGRP
End Property
Public Property Let SGRP(newSGRP As Long)
  pSGRP = newSGRP
End Property

