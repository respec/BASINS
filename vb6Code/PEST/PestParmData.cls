VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "clsPestParmData"
Attribute VB_GlobalNameSpace = True
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit
'Copyright 2003 AQUA TERRA Consultants - Royalty-free use permitted under open source license

Private pName As String       'parameter name, <12 chars, case sensitive
Private pTrans As String      '"none", "log", "fixed", or "tied"
Private pLimType As String    '"relative" or "factor"
Private pInitVal As Single    'initial parameter value
Private pLBnd As Single       'lower boundary of parameter range
Private pUBnd As Single       'upper boundary of parameter range
Private pGroup As String      'group name, <12 chars, case sensitive
Private pMult As Single       'parameter value multiplier
Private pOffset As Single     'parameter value offset
Private pModelSpecs As String 'string of model input specifications

Public Property Get Name() As String
  Name = pName
End Property
Public Property Let Name(ByVal newvalue As String)
  pName = newvalue
End Property

Public Property Get Trans() As String
  Trans = pTrans
End Property
Public Property Let Trans(ByVal newvalue As String)
  pTrans = newvalue
End Property

Public Property Get LimType() As String
  LimType = pLimType
End Property
Public Property Let LimType(ByVal newvalue As String)
  pLimType = newvalue
End Property

Public Property Get InitVal() As Single
  InitVal = pInitVal
End Property
Public Property Let InitVal(ByVal newvalue As Single)
  pInitVal = newvalue
End Property

Public Property Get LBnd() As Single
  LBnd = pLBnd
End Property
Public Property Let LBnd(ByVal newvalue As Single)
  pLBnd = newvalue
End Property

Public Property Get UBnd() As Single
  UBnd = pUBnd
End Property
Public Property Let UBnd(ByVal newvalue As Single)
  pUBnd = newvalue
End Property

Public Property Get Group() As String
  Group = pGroup
End Property
Public Property Let Group(ByVal newvalue As String)
  pGroup = newvalue
End Property

Public Property Get Mult() As Single
  Mult = pMult
End Property
Public Property Let Mult(ByVal newvalue As Single)
  pMult = newvalue
End Property

Public Property Get Offset() As Single
  Offset = pOffset
End Property
Public Property Let Offset(ByVal newvalue As Single)
  pOffset = newvalue
End Property

Public Property Get ModelSpecs() As String
  ModelSpecs = pModelSpecs
End Property
Public Property Let ModelSpecs(ByVal newvalue As String)
  pModelSpecs = newvalue
End Property

Private Sub Class_Initialize()
  pName = ""
  pTrans = "none"
  pLimType = ""
  pInitVal = 0#
  pLBnd = 0#
  pUBnd = 0#
  pGroup = ""
  pMult = 1#
  pOffset = 0#
  pModelSpecs = ""
End Sub

Public Function Valid(Prop As String) As FastCollection
  Dim lColl As FastCollection
  Dim lProp As String

  Set lColl = Nothing
  lProp = LCase(Prop)
  If lProp = "trans" Or lProp = "limtype" Then
    Set lColl = New FastCollection
    Select Case lProp
      Case "trans"
        lColl.Add "none"
        lColl.Add "log"
        lColl.Add "fixed"
        lColl.Add "tied"
      Case "limtype"
        lColl.Add "relative"
        lColl.Add "factor"
    End Select
  End If
  Set Valid = lColl
End Function
