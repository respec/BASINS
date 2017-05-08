VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "ATCclsParm"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit
'##MODULE_SUMMARY Class containing a model parameter value.
'##MODULE_REMARKS Copyright 2001-3AQUA TERRA Consultants - Royalty-free use permitted under open source license

Dim pValue As String
Dim pDef As ATCclsParmDef
Dim pParent As Object

'##SUMMARY <P>Value of parameter.
Public Property Get Value() As String
Attribute Value.VB_UserMemId = 0
Attribute Value.VB_MemberFlags = "200"
  Value = pValue
End Property
Public Property Let Value(newValue As String)
  pValue = newValue
  If Not (IsNull(pParent)) Then
    pParent.Edited = True
  End If
End Property

'##SUMMARY Link to object containing definition of parameter.
Public Property Get Def() As ATCclsParmDef
  Set Def = pDef
End Property
Public Property Set Def(newDef As ATCclsParmDef)
  Set pDef = newDef
End Property

'##SUMMARY Link to object that is the parent of this parameter.
Public Property Get Parent() As Object
  Set Parent = pParent
End Property
Public Property Set Parent(newParent As Object)
  Set pParent = newParent
End Property

'##SUMMARY Name of parameter.
Public Property Get Name() As String
  Name = pDef.Name
End Property

Private Sub Class_Initialize()
  Set pParent = Nothing
End Sub
