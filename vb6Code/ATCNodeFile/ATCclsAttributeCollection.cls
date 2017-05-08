VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "ATCclsAttributeCollection"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit
'Copyright 2001 by AQUA TERRA Consultants

Private pAttributes As Collection
Private pErrorDescription As String
Private pSerial As Long

Public Function Dump() As String
  Dim retval As String
  Dim a As Variant
  
  retval = "Serial number = " & pSerial
  
  For Each a In pAttributes
    retval = retval & "Attribute: " & a.Name & " = " & a.Value
    'retval = retval & " (" & a.Definition.Dump & ") " & vbCr
  Next a
  
  Debug.Print retval
  Dump = retval
End Function

Public Sub SetValue(AttrName As String, AttrValue As Variant, Optional AttrDef As ATCclsAttributeDefinition)
  Dim a As Variant, fnd As Boolean, lAttr As ATTimSerAttribute, attrIndex As Long
  
  fnd = False
  For attrIndex = 1 To pAttributes.Count
    If pAttributes(attrIndex).Name = AttrName Then 'update existing
      lAttr = pAttributes(attrIndex)
      lAttr.Value = AttrValue
      pAttributes.Remove attrIndex
      If attrIndex < pAttributes.Count Then
        pAttributes.Add lAttr, , attrIndex + 1
      Else
        pAttributes.Add lAttr
      End If
      fnd = True
      Exit For
    End If
  Next
  If Not (fnd) Then
    lAttr.Name = AttrName
    lAttr.Value = AttrValue
    If IsMissing(AttrDef) Then
      Set lAttr.Definition = New ATCclsAttributeDefinition
      lAttr.Definition.Name = AttrName
      lAttr.Definition.DataType = NONE
    ElseIf AttrDef Is Nothing Then
      Set lAttr.Definition = New ATCclsAttributeDefinition
      lAttr.Definition.Name = AttrName
      lAttr.Definition.DataType = NONE
    Else
      Set lAttr.Definition = AttrDef
    End If
    pAttributes.Add lAttr
  End If
End Sub

'Gets values by name
Public Property Get Value(AttrName As String, Optional Default As String = "") As Variant
  Dim a As Variant
  
  Value = Default
  
  For Each a In pAttributes
    If a.Name = AttrName Then
      Value = a.Value
      Exit For
    End If
  Next a

End Property

Public Property Get Names() As Collection
  Dim a As Variant
  Dim retval As New Collection
  
  On Error Resume Next
  For Each a In pAttributes
    retval.Add a.Name, a.Name
  Next

  Set Names = retval
End Property

Public Property Set All(newV As Collection) ' of ATCData.ATTimSerAttribute
  Set pAttributes = newV
End Property
Public Property Get All() As Collection ' of ATCData.ATTimSerAttribute
  Set All = pAttributes
End Property

'Serial number assigned in order of creation
'Guaranteed to be unique at runtime but will be different from one run to another
Public Property Get Serial() As Long
  Serial = pSerial
End Property

Private Sub Class_Initialize()
  Set pAttributes = New Collection
  pSerial = NextAttribSerial
  NextAttribSerial = NextAttribSerial + 1
End Sub

Private Sub Class_Terminate()
  Set pAttributes = Nothing
End Sub

