VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "ATCclsAttributeDefinition"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit
'##MODULE_REMARKS Copyright 2001-3 AQUA TERRA Consultants - Royalty-free _
 use permitted under open source license.
'##MODULE_SUMMARY An instance of this class contains a definition of _
 an Attribute.
'##PROJECT_INTRODUCTION Generic classes used in many applications.&nbsp; _
 They&nbsp;<b>must<b> not be changed without close coordination with AQUA _
 TERRA!

Dim pName As String
Dim pDesc As String
Dim pValid As String
Dim pDataType As ATCoDataType
Dim pDefault As Single
Dim pEditable As Boolean
Dim pMin As Single
Dim pMax As Single

'##SUMMARY This function returns a string containing details of the _
 Attribute Definition.
Public Function Dump() As String
  Dim retval As String
  retval = "Name: " & pName
  If Len(pDesc) > 0 Then retval = retval & " Description: " & pDesc
  Select Case pDataType
    Case ATCoClr: retval = retval & " Type: ATCoClr"
    Case ATCoTxt: retval = retval & " Type: ATCoTxt"
    Case ATCoSng: retval = retval & " Type: ATCoSng"
    Case ATCoInt: retval = retval & " Type: ATCoInt"
  End Select
  If Len(pValid) > 0 Then retval = retval & " Valid: " & pValid
  If Len(pEditable) > 0 Then retval = retval & " Editable: " & pEditable
  If pDefault <> 0 Then retval = retval & " Default: " & pDefault
  If Min <> -999 Then retval = retval & " Min: " & pMin
  If Max <> -999 Then retval = retval & " Max: " & pMin

  Debug.Print retval
  Dump = retval
End Function

'##SUMMARY The name of the Attribute being defined.
Public Property Get Name() As String
  Name = pName
End Property
Public Property Let Name(newName As String)
  pName = newName
End Property

'##SUMMARY A text description of the Attribute.
Public Property Get Description() As String
  Description = pDesc
End Property
Public Property Let Description(newDesc As String)
  pDesc = newDesc
End Property

'##SUMMARY Type of the Attribute (as an ATCoDataType)
Public Property Get DataType() As ATCoDataType
  DataType = pDataType
End Property
Public Property Let DataType(newType As ATCoDataType)
  pDataType = newType
End Property

'##SUMMARY Default value for the Attribute.
Public Property Get Default() As Single
  Default = pDefault
End Property
Public Property Let Default(newrDef As Single)
  pDefault = newrDef
End Property

'##SUMMARY Flag indicating if editing of the Attribute is allowed
Public Property Get Editable() As Boolean
  Editable = pEditable
End Property
Public Property Let Editable(newEditable As Boolean)
  pEditable = newEditable
End Property

'##SUMMARY Minimum value allowed for the Attribute.
Public Property Get Min() As Single
  Min = pMin
End Property
Public Property Let Min(newMin As Single)
  pMin = newMin
End Property

'##SUMMARY Maximum value allowed for the Attribute.
Public Property Get Max() As Single
  Max = pMax
End Property
Public Property Let Max(newMax As Single)
  pMax = newMax
End Property

'##SUMMARY Comma-separated list of possible values for the Attribute.
Public Property Get ValidValues() As String
  ValidValues = pValid
End Property
Public Property Let ValidValues(ValidValues As String)
  pValid = ValidValues
End Property

Private Sub Class_Initialize()
  pMin = -999
  pMax = -999
End Sub
