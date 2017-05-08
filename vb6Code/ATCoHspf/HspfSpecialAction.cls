VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "HspfSpecialAction"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit
'Copyright 2000 by AQUA TERRA Consultants
'class module for special actions ACTION records

Dim pOptyp$
Dim pOpidf&
Dim pOpidl&
Dim pCtCode1$
Dim pTstep1&
Dim pYear&
Dim pMon&
Dim pDay&
Dim pHr&
Dim pMin&
Dim pDsind&
Dim pTypcod&
Dim pVname$
Dim pCsub1$
Dim pCsub2$
Dim pCsub3$
Dim pAddr&
Dim pActcod&
Dim pVal!
Dim pUvqnam$
Dim pCtCode2$
Dim pTstep2&
Dim pNuminc&

Public Property Get Text() As String
  Text = ""
End Property
Public Property Let Text(newText As String)
  pOptyp = Trim(Left(newText, 6))
  pOpidf = Mid(newText, 7, 3)
  pOpidl = Mid(newText, 10, 4)
  pCtCode1 = Mid(newText, 14, 2)
  pTstep1 = Mid(newText, 16, 3)
  pYear = Mid(newText, 19, 4)
  pMon = Mid(newText, 23, 3)
  pDay = Mid(newText, 26, 3)
  pHr = Mid(newText, 29, 3)
  pMin = Mid(newText, 32, 3)
  pDsind = Mid(newText, 35, 2)
  pTypcod = Mid(newText, 37, 2)
  pVname = Mid(newText, 41, 6)
  pCsub1 = Mid(newText, 47, 3)
  pCsub2 = Mid(newText, 50, 3)
  pCsub3 = Mid(newText, 53, 3)
  pAddr = Mid(newText, 41, 8)
  pActcod = Mid(newText, 56, 3)
  pVal = Mid(newText, 59, 10)
  pUvqnam = Mid(newText, 63, 6)
  pCtCode2 = Mid(newText, 70, 2)
  pTstep2 = Mid(newText, 73, 3)
  pNuminc = Mid(newText, 76, 3)
End Property

Public Property Get Optyp() As String
  Optyp = pOptyp
End Property
Public Property Let Optyp(newOptyp As String)
  pOptyp = newOptyp
End Property

Public Property Get CtCode1() As String
  CtCode1 = pCtCode1
End Property
Public Property Let CtCode1(newCtCode1 As String)
  pCtCode1 = newCtCode1
End Property

Public Property Get Vname() As String
  Vname = pVname
End Property
Public Property Let Vname(newVname As String)
  pVname = newVname
End Property

Public Property Get Csub1() As String
  Csub1 = pCsub1
End Property
Public Property Let Csub1(newCsub1 As String)
  pCsub1 = newCsub1
End Property

Public Property Get Csub2() As String
  Csub2 = pCsub2
End Property
Public Property Let Csub2(newCsub2 As String)
  pCsub2 = newCsub2
End Property

Public Property Get Csub3() As String
  Csub3 = pCsub3
End Property
Public Property Let Csub3(newCsub3 As String)
  pCsub3 = newCsub3
End Property

Public Property Get Uvqnam() As String
  Uvqnam = pUvqnam
End Property
Public Property Let Uvqnam(newUvqnam As String)
  pUvqnam = newUvqnam
End Property

Public Property Get CtCode2() As String
  CtCode2 = pCtCode2
End Property
Public Property Let CtCode2(newCtCode2 As String)
  pCtCode2 = newCtCode2
End Property

Public Property Get Opidf() As Long
  Opidf = pOpidf
End Property
Public Property Let Opidf(newOpidf As Long)
  pOpidf = newOpidf
End Property

Public Property Get Opidl() As Long
  Opidl = pOpidl
End Property
Public Property Let Opidl(newOpidl As Long)
  pOpidl = newOpidl
End Property

Public Property Get Tstep1() As Long
  Tstep1 = pTstep1
End Property
Public Property Let Tstep1(newTstep1 As Long)
  pTstep1 = newTstep1
End Property

Public Property Get Year() As Long
  Year = pYear
End Property
Public Property Let Year(newYear As Long)
  pYear = newYear
End Property

Public Property Get Mon() As Long
  Mon = pMon
End Property
Public Property Let Mon(newMon As Long)
  pMon = newMon
End Property

Public Property Get Day() As Long
  Day = pDay
End Property
Public Property Let Day(newDay As Long)
  pDay = newDay
End Property

Public Property Get Hr() As Long
  Hr = pHr
End Property
Public Property Let Hr(newHr As Long)
  pHr = newHr
End Property

Public Property Get Min() As Long
  Min = pMin
End Property
Public Property Let Min(newMin As Long)
  pMin = newMin
End Property

Public Property Get Dsind() As Long
  Dsind = pDsind
End Property
Public Property Let Dsind(newDsind As Long)
  pDsind = newDsind
End Property

Public Property Get Typcod() As Long
  Typcod = pTypcod
End Property
Public Property Let Typcod(newTypcod As Long)
  pTypcod = newTypcod
End Property

Public Property Get Addr() As Long
  Addr = pAddr
End Property
Public Property Let Addr(newAddr As Long)
  pAddr = newAddr
End Property

Public Property Get Actcod() As Long
  Actcod = pActcod
End Property
Public Property Let Actcod(newActcod As Long)
  pActcod = newActcod
End Property

Public Property Get Tstep2() As Long
  Tstep2 = pTstep2
End Property
Public Property Let Tstep2(newTstep2 As Long)
  pTstep2 = newTstep2
End Property

Public Property Get Numinc() As Long
  Numinc = pNuminc
End Property
Public Property Let Numinc(newNuminc As Long)
  pNuminc = newNuminc
End Property

Public Property Get Val() As Single
  Val = pVal
End Property
Public Property Let Val(newVal As Single)
  pVal = newVal
End Property

