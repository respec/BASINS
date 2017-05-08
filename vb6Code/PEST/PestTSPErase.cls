VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "clsPestTSPErase"
Attribute VB_GlobalNameSpace = True
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit
'Copyright 2003 AQUA TERRA Consultants - Royalty-free use permitted under open source license

Private pComments As String           'comments for block
Private pContext As FastCollection    'of CONTEXT strings
Private pContextCom As FastCollection 'of CONTEXT comment strings
Private pSeriesName As FastCollection
Private pSeriesNameCom As FastCollection
Private pSTableName As FastCollection
Private pSTableNameCom As FastCollection
Private pVTableName As FastCollection
Private pVTableNameCom As FastCollection
Private pETableName As FastCollection
Private pETableNameCom As FastCollection

Public Property Get Comments() As String
  Comments = pComments
End Property
Public Property Let Comments(newvalue As String)
  pComments = newvalue
End Property

Public Property Get Context() As FastCollection
  Set Context = pContext
End Property
Public Property Let Context(ByVal newvalue As FastCollection)
  Set pContext = newvalue
End Property

Public Property Get ContextCom() As FastCollection
  Set ContextCom = pContextCom
End Property
Public Property Let ContextCom(ByVal newvalue As FastCollection)
  Set pContextCom = newvalue
End Property

Public Property Get SeriesName() As FastCollection
  Set SeriesName = pSeriesName
End Property
Public Property Let SeriesName(ByVal newvalue As FastCollection)
  Set pSeriesName = newvalue
End Property

Public Property Get SeriesNameCom() As FastCollection
  Set SeriesNameCom = pSeriesNameCom
End Property
Public Property Let SeriesNameCom(ByVal newvalue As FastCollection)
  Set pSeriesNameCom = newvalue
End Property

Public Property Get STableName() As FastCollection
  Set STableName = pSTableName
End Property
Public Property Let STableName(ByVal newvalue As FastCollection)
  Set pSTableName = newvalue
End Property

Public Property Get STableNameCom() As FastCollection
  Set STableNameCom = pSTableNameCom
End Property
Public Property Let STableNameCom(ByVal newvalue As FastCollection)
  Set pSTableNameCom = newvalue
End Property

Public Property Get VTableName() As FastCollection
  Set VTableName = pVTableName
End Property
Public Property Let VTableName(ByVal newvalue As FastCollection)
  Set pVTableName = newvalue
End Property

Public Property Get VTableNameCom() As FastCollection
  Set VTableNameCom = pVTableNameCom
End Property
Public Property Let VTableNameCom(ByVal newvalue As FastCollection)
  Set pVTableNameCom = newvalue
End Property

Public Property Get ETableName() As FastCollection
  Set ETableName = pETableName
End Property
Public Property Let ETableName(ByVal newvalue As FastCollection)
  Set pETableName = newvalue
End Property

Public Property Get ETableNameCom() As FastCollection
  Set ETableNameCom = pETableNameCom
End Property
Public Property Let ETableNameCom(ByVal newvalue As FastCollection)
  Set pETableNameCom = newvalue
End Property

Public Sub Populate(str As String)
  'str is contents of ERASE_ENTITY block (within header and END records)
  Dim rec As String, com As String, kwd As String, spec As String

  com = ""
  While Len(str) > 0
    rec = Trim(StrSplit(str, vbCrLf, ""))
    If Left(rec, 1) = "#" Then 'comment line
      If Len(com) > 0 Then
        com = com & vbCrLf & "  " & rec
      Else
        com = "  " & rec
      End If
    ElseIf Len(rec) > 0 Then
      kwd = Trim(StrRetRem(rec))
      spec = Trim(rec)
      Select Case kwd
        Case "CONTEXT"
          ProcessKwdColl spec, pContext, com, pContextCom
        Case "SERIES_NAME"
          ProcessKwdColl spec, pSeriesName, com, pSeriesNameCom
        Case "S_TABLE_NAME"
          ProcessKwdColl spec, pSTableName, com, pSTableNameCom
        Case "V_TABLE_NAME"
          ProcessKwdColl spec, pVTableName, com, pVTableNameCom
        Case "E_TABLE_NAME"
          ProcessKwdColl spec, pETableName, com, pETableNameCom
      End Select
      com = "" 'reset comment string after processing a keyword
    End If
  Wend
End Sub

Public Function WriteBlock() As String
  Dim str As String
  Dim lv As Variant

  If Len(pComments) > 0 Then
    str = pComments & vbCrLf & "START ERASE_ENTITY"
  Else
    str = "START ERASE_ENTITY"
  End If
  For Each lv In pContext
    If pContextCom.Count > 0 Then
      If pContextCom.KeyExists(lv) Then str = str & vbCrLf & pContextCom(lv)
    End If
    str = str & vbCrLf & "  CONTEXT " & lv
  Next
  For Each lv In pSeriesName
    If pSeriesNameCom.Count > 0 Then
      If pSeriesNameCom.KeyExists(lv) Then str = str & vbCrLf & pSeriesNameCom(lv)
    End If
    str = str & vbCrLf & "  SERIES_NAME " & lv
  Next
  For Each lv In pSTableName
    If pSTableNameCom.Count > 0 Then
      If pSTableNameCom.KeyExists(lv) Then str = str & vbCrLf & pSTableNameCom(lv)
    End If
    str = str & vbCrLf & "  S_TABLE_NAME " & lv
  Next
  For Each lv In pVTableName
    If pVTableNameCom.Count > 0 Then
      If pVTableNameCom.KeyExists(lv) Then str = str & vbCrLf & pVTableNameCom(lv)
    End If
    str = str & vbCrLf & "  V_TABLE_NAME " & lv
  Next
  For Each lv In pETableName
    If pETableNameCom.Count > 0 Then
      If pETableNameCom.KeyExists(lv) Then str = str & vbCrLf & pETableNameCom(lv)
    End If
    str = str & vbCrLf & "  E_TABLE_NAME " & lv
  Next
  str = str & vbCrLf & "END ERASE_ENTITY"
  WriteBlock = str

End Function

Private Sub Class_Initialize()
  pComments = ""
  Set pContext = New FastCollection
  Set pContextCom = New FastCollection
  Set pSeriesName = New FastCollection
  Set pSeriesNameCom = New FastCollection
  Set pSTableName = New FastCollection
  Set pSTableNameCom = New FastCollection
  Set pVTableName = New FastCollection
  Set pVTableNameCom = New FastCollection
  Set pETableName = New FastCollection
  Set pETableNameCom = New FastCollection
End Sub
