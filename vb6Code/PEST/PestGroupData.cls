VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "clsPestGroupData"
Attribute VB_GlobalNameSpace = True
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit
'Copyright 2003 AQUA TERRA Consultants - Royalty-free use permitted under open source license

Private pName As String     'group name, <12 chars, case sensitive
Private pIncType As String  '"relative", "absolute", or "rel_to_max"
Private pDerInc As Single   'derivative increment
Private pDILBnd As Single   'derivative increment lower bound
Private pForCen As String   'Forward/Central diff option, "always_2", "always_3", or "switch"
Private pDIMult As Single   'derivative increment multiplier
Private pDerMeth As String  'derivative method, "parabolic", "best_fit", or "outside_pts"

Public Property Get Name() As String
  Name = pName
End Property
Public Property Let Name(ByVal newvalue As String)
  pName = newvalue
End Property

Public Property Get IncType() As String
  IncType = pIncType
End Property
Public Property Let IncType(ByVal newvalue As String)
  pIncType = newvalue
End Property

Public Property Get DerInc() As Single
  DerInc = pDerInc
End Property
Public Property Let DerInc(ByVal newvalue As Single)
  pDerInc = newvalue
End Property

Public Property Get DILBnd() As Single
  DILBnd = pDILBnd
End Property
Public Property Let DILBnd(ByVal newvalue As Single)
  pDILBnd = newvalue
End Property

Public Property Get ForCen() As String
  ForCen = pForCen
End Property
Public Property Let ForCen(ByVal newvalue As String)
  pForCen = newvalue
End Property

Public Property Get DIMult() As Single
  DIMult = pDIMult
End Property
Public Property Let DIMult(ByVal newvalue As Single)
  pDIMult = newvalue
End Property

Public Property Get DerMeth() As String
  DerMeth = pDerMeth
End Property
Public Property Let DerMeth(ByVal newvalue As String)
  pDerMeth = newvalue
End Property

Private Sub Class_Initialize()
  pName = ""
  pIncType = ""
  pDerInc = 0.01
  pDILBnd = 0#
  pForCen = ""
  pDIMult = 1#
  pDerMeth = ""
End Sub

Public Function Valid(Prop As String) As FastCollection
  Dim lColl As FastCollection
  Dim lProp As String

  Set lColl = Nothing
  lProp = LCase(Prop)
  If lProp = "inctype" Or lProp = "forcen" Or lProp = "dermeth" Then
    Set lColl = New FastCollection
    Select Case lProp
      Case "inctype"
        lColl.Add "relative"
        lColl.Add "absolute"
        lColl.Add "rel_to_max"
      Case "forcen"
        lColl.Add "always_2"
        lColl.Add "always_3"
        lColl.Add "switch"
      Case "dermeth"
        lColl.Add "parabolic"
        lColl.Add "best_fit"
        lColl.Add "outside_pts"
    End Select
  End If
  Set Valid = lColl
End Function

