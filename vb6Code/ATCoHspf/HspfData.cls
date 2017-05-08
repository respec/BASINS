VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "HspfData"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit
'Copyright 2002 AQUA TERRA Consultants - Royalty-free use permitted under open source license

Public Type HspfFile
  Typ As String  'valid are MESSU, WDM(x), DSSx or blank
  Unit As Long    'use 21-99
  Name As String  'complete path
  Comment As String 'preceeding comment
End Type

Public Type HspfCategory
  Tag As String
  Name As String
  Comment As String 'preceeding comment
End Type

Public Enum HspfOperType
  hPerlnd = 1
  hImplnd = 2
  hRchres = 3
  hCopy = 4
  hPltgen = 5
  hDisply = 6
  hDuranl = 7
  hGener = 8
  hMutsin = 9
  hBmprac = 10
  hReport = 11
End Enum

Public Enum HspfSpecialRecordType
  hAction = 1
  hDistribute = 2
  hUserDefineName = 3
  hUserDefineQuan = 4
  hCondition = 5
  hComment = 6
End Enum

Private Sub Class_Initialize()
  Debug.Print "Init HspfDataTypes"
End Sub
