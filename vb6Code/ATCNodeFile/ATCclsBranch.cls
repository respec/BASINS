VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "ATCclsBranch"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit

Private pAttributes As ATCclsAttributeCollection
Private pSerial As Long
Private pNodes As Collection
Private pShapes As Collection

Public Property Get Attributes() As ATCclsAttributeCollection
  Set Attributes = pAttributes
End Property
Public Property Set Attributes(newCollection As ATCclsAttributeCollection)
  Set pAttributes = Nothing
  Set pAttributes = newCollection
End Property

'Nodes is a collection of ATCclsNode
'starting with the upstream end of the branch
'ending with the downstream end of the branch
'A simple branch will have only two nodes
Public Property Get Nodes() As Collection
  Set Nodes = pNodes
End Property
Public Property Set Nodes(newNodes As Collection)
  Set pNodes = Nothing
  Set pNodes = newNodes
End Property

Public Property Get NodeUpstream() As ATCclsNode
  If pNodes.Count > 0 Then Set NodeUpstream = pNodes(1)
End Property
Public Property Get NodeDownstream() As ATCclsNode
  If pNodes.Count > 0 Then Set NodeDownstream = pNodes(pNodes.Count)
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
  Set pNodes = New Collection
  Set pShapes = New Collection
  pSerial = NextBranchSerial
  NextBranchSerial = NextBranchSerial + 1
End Sub

Private Sub Class_Terminate()
  Set pAttributes = Nothing
  Set pNodes = Nothing
  Set pShapes = Nothing
End Sub
