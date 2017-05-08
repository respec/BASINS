VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "HspfMetSegRecord"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit
'Copyright 2002 AQUA TERRA Consultants - Royalty-free use permitted under open source license

Public Enum MetSegRecordType
  msrUNK = 0
  msrPREC
  msrGATMP
  msrDTMPG
  msrWINMOV
  msrSOLRAD
  msrCLOUD
  msrPETINP
  msrPOTEV
End Enum
Dim pMFactP As Double
Dim pMFactR As Double
Dim pTyp As Long
Dim pTran As String
Dim pSgapstrg As String
Dim pSsystem As String
Dim pSource As HspfSrcTar

Public Property Get MFactP() As Double
  MFactP = pMFactP
End Property
Public Property Let MFactP(newMFactP As Double)
  pMFactP = newMFactP
End Property
Public Property Get MFactR() As Double
  MFactR = pMFactR
End Property
Public Property Let MFactR(newMFactR As Double)
  pMFactR = newMFactR
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
Public Property Get Tran() As String
  Tran = pTran
End Property
Public Property Let Tran(newTran As String)
  pTran = newTran
End Property
Public Property Get typ() As Long
  typ = pTyp
End Property
Public Property Let typ(newTyp As Long)
  pTyp = newTyp
End Property

Public Function Compare(tMetSegRecord As HspfMetSegRecord, opname As String) As Boolean
  Compare = True
  If opname = "PERLND" Or opname = "IMPLND" Then
    If tMetSegRecord.MFactP <> Me.MFactP Then
      Compare = False
    End If
  ElseIf opname = "RCHRES" Then
    If tMetSegRecord.MFactR <> Me.MFactR And Me.MFactR <> -999# Then
      Compare = False
    End If
  End If
  
  If opname = "RCHRES" And Me.MFactP = -999# And Me.MFactR = -999 Then
    'dont bother to compare.  this is a rchres, and mfactp has not
    'been set, so whatever this record contains it will be fine.
    '(for situation like basins evap, that only gets written for rchres)
  ElseIf opname = "RCHRES" And tMetSegRecord.MFactR = -999# Then
    'dont bother to compare.  this is a rchres, mfactp has been set
    'but mfactr is not set, whatever this record contains will be fine.
    '(for situation like basins pevt, that only gets written for per/implnd
  Else
    If tMetSegRecord.Tran <> Me.Tran Then
      Compare = False
    ElseIf tMetSegRecord.Sgapstrg <> Me.Sgapstrg Then
      Compare = False
    ElseIf tMetSegRecord.Ssystem <> Me.Ssystem Then
      Compare = False
    ElseIf tMetSegRecord.Source.VolName <> Me.Source.VolName Then
      Compare = False
    ElseIf tMetSegRecord.Source.VolId <> Me.Source.VolId Then
      Compare = False
    ElseIf tMetSegRecord.Source.Member <> Me.Source.Member Then
      Compare = False
    ElseIf tMetSegRecord.Source.MemSub1 <> Me.Source.MemSub1 Then
      Compare = False
    ElseIf tMetSegRecord.Source.MemSub2 <> Me.Source.MemSub2 Then
      Compare = False
    End If
  End If
  
End Function

Private Sub Class_Initialize()
  Set pSource = New HspfSrcTar
  pTyp = 0
  pMFactP = -999#
  pMFactR = -999#
  pSgapstrg = ""
  pSsystem = ""
End Sub
