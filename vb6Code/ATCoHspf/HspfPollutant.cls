VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "HspfPollutant"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit
'Copyright 2002 AQUA TERRA Consultants - Royalty-free use permitted under open source license

Dim pId As Long
Dim pName As String
Dim pModelType As String
Dim pTables As Collection
Dim pMassLinks As Collection
Dim pOperations As Collection
Dim pIndex As Long

Public Property Get Id() As Long
  Id = pId
End Property
Public Property Let Id(newId As Long)
  pId = newId
End Property
Public Property Get Index() As Long
  Index = pIndex
End Property
Public Property Let Index(newIndex As Long)
  pIndex = newIndex
End Property
Public Property Get Name() As String
  Name = pName
End Property
Public Property Let Name(newName As String)
  pName = newName
End Property
Public Property Get ModelType() As String
  ModelType = pModelType
End Property
Public Property Let ModelType(newModelType As String)
  pModelType = newModelType
End Property
Public Property Get Tables() As Collection 'of HspfTable
  Set Tables = pTables
End Property
Public Property Get Operations() As Collection 'of HspfOperation
  Set Operations = pOperations
End Property
Public Property Let Operations(newOperations As Collection) 'of HspfMassLinks
  Dim lOperation As HspfOperation, vOperation As Variant
  For Each vOperation In newOperations
    Set lOperation = vOperation
    pOperations.Add lOperation
  Next vOperation
End Property
Public Property Get MassLinks() As Collection 'of HspfMasslink
  Set MassLinks = pMassLinks
End Property
Public Property Let MassLinks(newMassLinks As Collection) 'of HspfMassLinks
  Dim lMassLink As HspfMassLink, vMassLink As Variant
  For Each vMassLink In newMassLinks
    Set lMassLink = vMassLink
    pMassLinks.Add lMassLink
  Next vMassLink
End Property

Private Sub Class_Initialize()
  Set pTables = New Collection
  Set pOperations = New Collection
  Set pMassLinks = New Collection
  pId = 0
  pName = ""
  pModelType = ""
  pIndex = 0
End Sub

Public Function TableExists(Name$) As Boolean
  Dim vTable As Variant
  
  On Error GoTo NoTable:
  Set vTable = pTables(Name)
  TableExists = True
  Exit Function
NoTable:
  TableExists = False
End Function
