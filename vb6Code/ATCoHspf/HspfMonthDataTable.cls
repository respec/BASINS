VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "HspfMonthDataTable"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit
'Copyright 2002 AQUA TERRA Consultants - Royalty-free use permitted under open source license

Dim pId As Long
Dim pMonthValues(12) As Double
Dim pBlock As HspfMonthData
Dim pReferencedBy As Collection 'of hspfoperation

Public Property Get Id() As Long
  Id = pId
End Property
Public Property Let Id(newId As Long)
  pId = newId
End Property

Public Property Get Block() As HspfMonthData
  Set Block = pBlock
End Property
Public Property Set Block(newBlock As HspfMonthData)
  Set pBlock = newBlock
End Property

Public Property Get MonthValue(Month As Long) As Single
  MonthValue = pMonthValues(Month)
End Property
Public Property Let MonthValue(Month As Long, newMonthValue As Single)
  pMonthValues(Month) = newMonthValue
End Property

Public Property Get ReferencedBy() As Collection
  Set ReferencedBy = pReferencedBy
End Property

Private Sub Class_Initialize()
  Set pReferencedBy = New Collection
End Sub
