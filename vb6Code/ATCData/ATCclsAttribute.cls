VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "ATCclsAttribute"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit

Dim pName As String
Dim pDesc As String
Dim pValid As String
Dim pDataType As ATCoDataType
Dim pMin As Single
Dim pMax As Single
Dim pDefault As Single

Public Property Get Name() As String
  Name = pName
End Property
Public Property Let Name(newName As String)
  pName = newName
End Property

Public Property Get Description() As String
  Description = pDesc
End Property
Public Property Let Description(newDesc As String)
  pDesc = newDesc
End Property

Public Property Get DataType() As ATCoDataType
  DataType = pDataType
End Property
Public Property Let DataType(newType As ATCoDataType)
  pDataType = newType
End Property

Public Property Get Min() As Single
  Min = pMin
End Property
Public Property Let Min(newMin As Single)
  pMin = newMin
End Property

Public Property Get Max() As Single
  Max = pMax
End Property
Public Property Let Max(newMax As Single)
  pMax = newMax
End Property

'Comma-separated list of possible values
Public Property Get ValidValues() As String
  ValidValues = pValid
End Property
Public Property Let ValidValues(newValid As String)
  pValid = newValid
End Property

Public Property Get Default() As Single
  Default = pDefault
End Property
Public Property Let Default(newrDef As Single)
  pDefault = newrDef
End Property

