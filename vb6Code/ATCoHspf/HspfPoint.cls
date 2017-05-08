VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "HspfPoint"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit
'Copyright 2002 AQUA TERRA Consultants - Royalty-free use permitted under open source license

Dim pId As Long
Dim pName As String
Dim pCon As String
Dim pMFact As Double
Dim pRFact As Double
Dim pTran As String
Dim pSgapstrg As String
Dim pSsystem As String
Dim pSource As HspfSrcTar
Dim pTarget As HspfSrcTar
Dim pAssocOper As Long

Public Property Get Id() As Long
  Id = pId
End Property
Public Property Let Id(newId As Long)
  pId = newId
End Property

Public Property Get AssocOper() As Long
  AssocOper = pAssocOper
End Property
Public Property Let AssocOper(newAssocOper As Long)
  pAssocOper = newAssocOper
End Property

Public Property Get Name() As String
  Name = pName
End Property
Public Property Let Name(newName As String)
  pName = newName
End Property
Public Property Get Con() As String
  Con = pCon
End Property
Public Property Let Con(newCon As String)
  pCon = newCon
End Property
Public Property Get MFact() As Double
  MFact = pMFact
End Property
Public Property Let MFact(newMFact As Double)
  pMFact = newMFact
End Property
Public Property Get RFact() As Double
  RFact = pRFact
End Property
Public Property Let RFact(newRFact As Double)
  pRFact = newRFact
End Property
Public Property Get Ssystem() As String
  Ssystem = pSsystem
End Property
Public Property Let Ssystem(newSsystem As String)
  pSsystem = newSsystem
End Property
Public Property Get Sgapstrg() As String
  Sgapstrg = pSgapstrg
End Property
Public Property Let Sgapstrg(newSgapstrg As String)
  pSgapstrg = newSgapstrg
End Property
Public Property Get Source() As HspfSrcTar
  Set Source = pSource
End Property
Public Property Set Source(newSource As HspfSrcTar)
  Set pSource = newSource
End Property
Public Property Get Target() As HspfSrcTar
  Set Target = pTarget
End Property
Public Property Set Target(newTarget As HspfSrcTar)
  Set pTarget = newTarget
End Property
Public Property Get Tran() As String
  Tran = pTran
End Property
Public Property Let Tran(newTran As String)
  pTran = newTran
End Property

Private Sub Class_Initialize()
  Set pSource = New HspfSrcTar
  Set pTarget = New HspfSrcTar
  pMFact = 1#
  pRFact = 1#
  pSgapstrg = ""
  pSsystem = ""
End Sub

Public Sub WriteUciFile(icol&(), ilen&(), f)
  Dim j&, i&, str$, t$
     
  str = Trim(pSource.VolName)
  str = str & Space(icol(1) - Len(str) - 1) 'pad prev field
  t = Space(ilen(1)) 'right justify numbers
  RSet t = CStr(pSource.VolId)
  str = str & t
  str = str & Space(icol(2) - Len(str) - 1)
  str = str & pSource.member
  str = str & Space(icol(3) - Len(str) - 1)
  If pSource.MemSub1 <> 0 Then
    t = Space(ilen(3))
    RSet t = CStr(pSource.MemSub1)
    str = str & t
  End If
  str = str & Space(icol(4) - Len(str) - 1)
  str = str & Me.Ssystem
  str = str & Space(icol(5) - Len(str) - 1)
  str = str & Me.Sgapstrg
  str = str & Space(icol(6) - Len(str) - 1)
  If Me.MFact <> 1 Then
    t = Space(ilen(6))
    RSet t = CStr(Me.MFact)
    str = str & t
  End If
  str = str & Space(icol(7) - Len(str) - 1)
  str = str & Me.Tran
  str = str & Space(icol(8) - Len(str) - 1)
  str = str & Me.Target.VolName
  str = str & Space(icol(9) - Len(str) - 1)
  t = Space(ilen(9))
  If Me.Target.VolId > 0 And Me.Target.VolIdL > 0 Then
    'have a range of operations, just write the one for the assoc oper
    RSet t = CStr(Me.AssocOper)
  Else
    RSet t = CStr(Me.Target.VolId)
  End If
  str = str & t
  str = str & Space(icol(11) - Len(str) - 1)
  str = str & Me.Target.Group
  str = str & Space(icol(12) - Len(str) - 1)
  str = str & Me.Target.member
  str = str & Space(icol(13) - Len(str) - 1)
  If Me.Target.MemSub1 > 0 Then
    t = Space(ilen(13))
    RSet t = CStr(Me.Target.MemSub1)
    If Me.Target.VolName = "RCHRES" Then t = Me.Target.Opn.Uci.IntAsCat(Me.Target.member, 1, t)
    str = str & t
    str = str & Space(icol(14) - Len(str) - 1)
  End If
  If Me.Target.MemSub2 > 0 Then
    t = Space(ilen(14))
    RSet t = CStr(Me.Target.MemSub2)
    If Me.Target.VolName = "RCHRES" Then t = Me.Target.Opn.Uci.IntAsCat(Me.Target.member, 2, t)
    str = str & t
  End If
  Print #f, str
End Sub

