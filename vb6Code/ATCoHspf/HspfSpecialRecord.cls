VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "HspfSpecialRecord"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit
'Copyright 2002 AQUA TERRA Consultants - Royalty-free use permitted under open source license

Dim pText$
Dim pSpecType As HspfSpecialRecordType

Public Property Get Text() As String
  Text = pText
End Property
Public Property Let Text(newText As String)
  pText = newText
End Property

Public Property Get SpecType() As HspfSpecialRecordType
  SpecType = pSpecType
End Property
Public Property Let SpecType(newSpecType As HspfSpecialRecordType)
  pSpecType = newSpecType
End Property
