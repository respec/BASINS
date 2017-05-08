VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "clsPestTSPNewTimeBase"
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
Private pTBSeriesName As String
Private pTBSeriesNamecom As String


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

Public Property Get TBSeriesName() As String
  TBSeriesName = pTBSeriesName
End Property
Public Property Let TBSeriesName(ByVal newvalue As String)
  pTBSeriesName = newvalue
End Property

Public Property Get TBSeriesNameCom() As String
  TBSeriesNameCom = pTBSeriesNamecom
End Property
Public Property Let TBSeriesNameCom(ByVal newvalue As String)
  pTBSeriesNamecom = newvalue
End Property

Public Sub Populate(str As String)
  'str is contents of NEW_TIME_BASE block (within header and END records)
  Dim rec As String, com As String, kwd As String, spec As String
  Dim curLabel As String

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
        Case "TB_SERIES_NAME"
          pTBSeriesName = spec
          If Len(com) > 0 Then pTBSeriesNamecom = com
      End Select
      com = "" 'reset comment string after processing a keyword
    End If
  Wend
End Sub

Public Function WriteBlock() As String
  Dim str As String
  Dim lv As Variant

  If Len(pComments) > 0 Then
    str = pComments & vbCrLf & "START NEW_TIME_BASE"
  Else
    str = "START NEW_TIME_BASE"
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
  str = str & vbCrLf & "  NEW_SERIES_NAME " & pNewSeriesName
  If Len(pTBSeriesNamecom) > 0 Then str = str & vbCrLf & pTBSeriesNamecom
  str = str & vbCrLf & "  TB_SERIES_NAME " & pTBSeriesName
  str = str & vbCrLf & "END NEW_TIME_BASE"
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
  pTBSeriesName = ""
  pTBSeriesNamecom = ""
End Sub

