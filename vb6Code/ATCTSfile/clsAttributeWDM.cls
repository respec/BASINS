VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "clsAttributeWDM"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit
'Copyright 2002 by AQUA TERRA Consultants
Implements ATCclsAttributeDefinition

Dim Pname As String
Dim pDesc As String
Dim pValid As String
Dim pDataType As ATCoDataType
Dim pEditable As Boolean
Dim pDefault As Single
Dim pMin As Single
Dim pMax As Single
Dim pInd As Long
Dim piLen As Long
Dim phLen As Long
Dim phRec As Long
Dim phPos As Long
Dim pvLen As Long

Private Function ATCclsAttributeDefinition_Dump() As String
  Dim retval As String
  retval = "Name: " & Pname
  If Len(pDesc) > 0 Then retval = retval & " Description: " & pDesc
  Select Case pDataType
    Case ATCoClr: retval = retval & " Type: ATCoClr"
    Case ATCoTxt: retval = retval & " Type: ATCoTxt"
    Case ATCoSng: retval = retval & " Type: ATCoSng"
    Case ATCoInt: retval = retval & " Type: ATCoInt"
  End Select
  If Len(pEditable) > 0 Then retval = retval & " Editable: " & pEditable
  If Len(pValid) > 0 Then retval = retval & " Valid: " & pValid
  If pDefault <> 0 Then retval = retval & " Default: " & pDefault
  If Min <> -999 Then retval = retval & " Min: " & pMin
  If Max <> -999 Then retval = retval & " Max: " & pMin
  If pInd <> 0 Then retval = retval & " Ind: " & pInd
  If piLen <> 0 Then retval = retval & " iLen: " & piLen
  If phLen <> 0 Then retval = retval & " hLen: " & phLen
  If phRec <> 0 Then retval = retval & " hRec: " & phRec
  If phPos <> 0 Then retval = retval & " hPos: " & phPos
  If pvLen <> 0 Then retval = retval & " vLen: " & piLen
  Debug.Print retval
  ATCclsAttributeDefinition_Dump = retval
End Function

Private Property Get ATCclsAttributeDefinition_Name() As String
  ATCclsAttributeDefinition_Name = Pname
End Property
Private Property Let ATCclsAttributeDefinition_Name(newName As String)
  Pname = newName
End Property

Private Property Get ATCclsAttributeDefinition_Description() As String
  ATCclsAttributeDefinition_Description = pDesc
End Property
Private Property Let ATCclsAttributeDefinition_Description(newDesc As String)
  pDesc = newDesc
End Property

Private Property Get ATCclsAttributeDefinition_DataType() As ATCoDataType
  ATCclsAttributeDefinition_DataType = pDataType
End Property
Private Property Let ATCclsAttributeDefinition_DataType(newType As ATCoDataType)
  pDataType = newType
End Property

Public Property Get ATCclsAttributeDefinition_Editable() As Boolean
  ATCclsAttributeDefinition_Editable = pEditable
End Property
Public Property Let ATCclsAttributeDefinition_Editable(newEditable As Boolean)
  pEditable = newEditable
End Property

Private Property Get ATCclsAttributeDefinition_Default() As Single
  ATCclsAttributeDefinition_Default = pDefault
End Property
Private Property Let ATCclsAttributeDefinition_Default(newrDef As Single)
  pDefault = newrDef
End Property

Private Property Get ATCclsAttributeDefinition_Min() As Single
  ATCclsAttributeDefinition_Min = pMin
End Property
Private Property Let ATCclsAttributeDefinition_Min(newMin As Single)
  pMin = newMin
End Property

Private Property Get ATCclsAttributeDefinition_Max() As Single
  ATCclsAttributeDefinition_Max = pMax
End Property
Private Property Let ATCclsAttributeDefinition_Max(newMax As Single)
  pMax = newMax
End Property

'Comma-separated list of possible values
Private Property Get ATCclsAttributeDefinition_ValidValues() As String
  ATCclsAttributeDefinition_ValidValues = pValid
End Property
Private Property Let ATCclsAttributeDefinition_ValidValues(newValidValues As String)
  pValid = newValidValues
End Property

'------Local duplicates so we can refer to them when using either type

Public Property Get Name() As String
Attribute Name.VB_MemberFlags = "200"
  Name = Pname
End Property
Public Property Let Name(newName As String)
  Pname = newName
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

Public Property Get Editable() As Boolean
  Editable = pEditable
End Property
Public Property Let Editable(newEditable As Boolean)
  pEditable = newEditable
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
Public Property Let ValidValues(newValidValues As String)
  pValid = newValidValues
End Property

Public Property Get Default() As Single
  Default = pDefault
End Property
Public Property Let Default(newrDef As Single)
  pDefault = newrDef
End Property

Public Property Get Ind() As Long
  Ind = pInd
End Property
Public Property Let Ind(newInd As Long)
  pInd = newInd
End Property

Public Property Get ilen() As Long
  ilen = piLen
End Property
Public Property Let ilen(newiLen As Long)
  piLen = newiLen
End Property

Public Property Get hlen() As Long
  hlen = phLen
End Property
Public Property Let hlen(newhLen As Long)
  phLen = newhLen
End Property

Public Property Get hrec() As Long
  hrec = phRec
End Property
Public Property Let hrec(newhRec As Long)
  phRec = newhRec
End Property

Public Property Get hpos() As Long
  hpos = phPos
End Property
Public Property Let hpos(newhPos As Long)
  phPos = newhPos
End Property

Public Property Get vlen() As Long
  vlen = pvLen
End Property
Public Property Let vlen(newvLen As Long)
  pvLen = newvLen
End Property
