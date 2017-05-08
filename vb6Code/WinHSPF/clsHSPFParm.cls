VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "clsHSPFParm"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = False
Option Explicit
Dim pDesc As String
Dim pParm As String
Dim pTable As String
Dim pValue As Single
Dim pOpType As String
Dim pOpId As Long
Dim pId As Long

Public Property Get Desc() As String
  Desc = pDesc
End Property
Public Property Let Desc(newDesc As String)
  pDesc = newDesc
End Property

Public Property Get Parm() As String
  Parm = pParm
End Property
Public Property Let Parm(newParm As String)
  pParm = newParm
End Property

Public Property Get Table() As String
  Table = pTable
End Property
Public Property Let Table(newTable As String)
  pTable = newTable
End Property

Public Property Get Value() As Single
  Value = pValue
End Property
Public Property Let Value(newValue As Single)
  pValue = newValue
End Property

Public Property Get OpType() As String
  OpType = pOpType
End Property
Public Property Let OpType(newOpType As String)
  pOpType = newOpType
End Property

Public Property Get OpId() As Long
  OpId = pOpId
End Property
Public Property Let OpId(newOpId As Long)
  pOpId = newOpId
End Property

Public Property Get Id() As Long
  Id = pId
End Property
Public Property Let Id(newId As Long)
  pId = newId
End Property
