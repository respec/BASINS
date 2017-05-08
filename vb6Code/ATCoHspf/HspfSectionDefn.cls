VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "HspfSectionDef"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit
'Copyright 2002 AQUA TERRA Consultants - Royalty-free use permitted under open source license

Dim pId As Long
Dim pName As String
Dim pParent As HspfBlockDef
Dim pTableDefs As Collection 'of HspfTableDef

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

Public Property Get TableDefs() As Collection 'of HspfTableDef
  Set TableDefs = pTableDefs
End Property
Public Property Set TableDefs(newTableDef As Collection) 'of HspfTableDef
  Set pTableDefs = newTableDef
End Property

