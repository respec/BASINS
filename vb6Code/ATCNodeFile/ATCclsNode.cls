VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "ATCclsNode"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit

Private pAttributes As ATCclsAttributeCollection
Private pShapes As Collection
Private pSerial As Long

Public Property Get Serial() As Long
  Serial = pSerial
End Property

Public Property Get Attributes() As ATCclsAttributeCollection
  Set Attributes = pAttributes
End Property
Public Property Set Attributes(newCollection As ATCclsAttributeCollection)
  Set pAttributes = Nothing
  Set pAttributes = newCollection
End Property

Public Property Get Shapes() As Collection
  Set Shapes = pShapes
End Property
Public Property Set Shapes(newShapes As Collection)
  Set pShapes = Nothing
  Set pShapes = newShapes
End Property

Private Sub Class_Initialize()
  Set pAttributes = New ATCclsAttributeCollection
  Set pShapes = New Collection
  pSerial = NextNodeSerial
  NextNodeSerial = NextNodeSerial + 1
End Sub

Private Sub Class_Terminate()
  Set pAttributes = Nothing
End Sub


