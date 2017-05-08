VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "clsPestTSPSeriesClean"
Attribute VB_GlobalNameSpace = True
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit
'Copyright 2003 AQUA TERRA Consultants - Royalty-free use permitted under open source license

Private pComments As String           'comments for block
Private pContext As FastCollection    'of CONTEXT strings
Private pContextCom As FastCollection 'of CONTEXT comment strings
Private pSeriesName As String
Private pSeriesNameCom As String
Private pNewSeriesName As String
Private pNewSeriesNameCom As String
Private pLEraseBnd As Single
Private pLEraseBndCom As String
Private pUEraseBnd As Single
Private pUEraseBndCom As String
Private pSubValue As String
Private pSubValueCom As String

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

Public Property Get SeriesName() As String
  SeriesName = pSeriesName
End Property
Public Property Let SeriesName(ByVal newvalue As String)
  pSeriesName = newvalue
End Property

Public Property Get SeriesNameCom() As String
  SeriesNameCom = pSeriesNameCom
End Property
Public Property Let SeriesNameCom(ByVal newvalue As String)
  pSeriesNameCom = newvalue
End Property

Public Property Get NewSeriesName() As String
  NewSeriesName = pNewSeriesName
End Property
Public Property Let NewSeriesName(ByVal newvalue As String)
  pNewSeriesName = newvalue
End Property

Public Property Get NewSeriesNameCom() As String
  NewSeriesNameCom = pNewSeriesNameCom
End Property
Public Property Let NewSeriesNameCom(ByVal newvalue As String)
  pNewSeriesNameCom = newvalue
End Property

Public Property Get LEraseBnd() As Single
  LEraseBnd = pLEraseBnd
End Property
Public Property Let LEraseBnd(ByVal newvalue As Single)
  pLEraseBnd = newvalue
End Property

Public Property Get LEraseBndCom() As String
  LEraseBndCom = pLEraseBndCom
End Property
Public Property Let LEraseBndCom(ByVal newvalue As String)
  pLEraseBndCom = newvalue
End Property

Public Property Get UEraseBnd() As Single
  UEraseBnd = pUEraseBnd
End Property
Public Property Let UEraseBnd(ByVal newvalue As Single)
  pUEraseBnd = newvalue
End Property

Public Property Get UEraseBndCom() As String
  UEraseBndCom = pUEraseBndCom
End Property
Public Property Let UEraseBndCom(ByVal newvalue As String)
  pUEraseBndCom = newvalue
End Property

Public Property Get SubValue() As String
  SubValue = pSubValue
End Property
Public Property Let SubValue(ByVal newvalue As String)
  pSubValue = newvalue
End Property

Public Property Get SubValueCom() As String
  SubValueCom = pSubValueCom
End Property
Public Property Let SubValueCom(ByVal newvalue As String)
  pSubValueCom = newvalue
End Property

Public Sub Populate(str As String)
  'str is contents of SERIES_CLEAN block (within header and END records)
  Dim rec As String, com As String, kwd As String, spec As String
  Dim curSite As String

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
          pSeriesName = spec
          If Len(com) > 0 Then pSeriesNameCom = com
        Case "NEW_SERIES_NAME"
          pNewSeriesName = spec
          If Len(com) > 0 Then pNewSeriesNameCom = com
        Case "LOWER_ERASE_BOUNDARY"
          pLEraseBnd = spec
          If Len(com) > 0 Then pLEraseBndCom = com
        Case "UPPER_ERASE_BOUNDARY"
          pUEraseBnd = spec
          If Len(com) > 0 Then pUEraseBndCom = com
        Case "SUBSTITUTE_VALUE"
          pSubValue = spec
          If Len(com) > 0 Then pSubValueCom = com
      End Select
      com = "" 'reset comment string after processing a keyword
    End If
  Wend
End Sub

Public Function WriteBlock() As String
  Dim str As String
  Dim lv As Variant

  If Len(pComments) > 0 Then
    str = pComments & vbCrLf & "START SERIES_CLEAN"
  Else
    str = "START SERIES_CLEAN"
  End If
  For Each lv In pContext
    If pContextCom.Count > 0 Then
      If pContextCom.KeyExists(lv) Then str = str & vbCrLf & pContextCom(lv)
    End If
    str = str & vbCrLf & "  CONTEXT " & lv
  Next
  If Len(pSeriesNameCom) > 0 Then str = str & vbCrLf & pSeriesNameCom
  str = str & vbCrLf & "  SERIES_NAME " & pSeriesName
  If Len(pNewSeriesNameCom) > 0 Then str = str & vbCrLf & pNewSeriesNameCom
  If Len(pNewSeriesName) > 0 Then
    str = str & vbCrLf & "  NEW_SERIES_NAME " & pNewSeriesName
  ElseIf pSubValue = "delete" Then
    '!!!ERROR, NewSeriesName required if SubValue is "delete"
  End If
  If Len(pLEraseBndCom) > 0 Then str = str & vbCrLf & pLEraseBndCom
  If Len(pLEraseBnd) > 0 Then str = str & vbCrLf & "  LOWER_ERASE_BOUNDARY " & pLEraseBnd
  If Len(pUEraseBndCom) > 0 Then str = str & vbCrLf & pUEraseBndCom
  If Len(pUEraseBnd) > 0 Then str = str & vbCrLf & "  UPPER_ERASE_BOUNDARY " & pUEraseBnd
  If Len(pSubValueCom) > 0 Then str = str & vbCrLf & pSubValueCom
  If Len(pSubValue) > 0 Then str = str & vbCrLf & "  SUBSTITUTE_VALUE " & pSubValue
  str = str & vbCrLf & "END SERIES_CLEAN"
  WriteBlock = str

End Function

Private Sub Class_Initialize()
  pComments = ""
  Set pContext = New FastCollection
  Set pContextCom = New FastCollection
  pSeriesName = ""
  pSeriesNameCom = ""
  pNewSeriesName = ""
  pNewSeriesNameCom = ""
  pLEraseBnd = 0#
  pLEraseBndCom = ""
  pUEraseBnd = 0#
  pUEraseBndCom = ""
  pSubValue = ""
  pSubValueCom = "00:00:00"
End Sub
