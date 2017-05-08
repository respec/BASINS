VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "clsPestTSPSeriesDisplace"
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
Private pLagIncrement As Long
Private pLagIncrementCom As String
Private pFillValue As Single
Private pFillValueCom As String

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

Public Property Get LagIncrement() As Long
  LagIncrement = pLagIncrement
End Property
Public Property Let LagIncrement(ByVal newvalue As Long)
  pLagIncrement = newvalue
End Property

Public Property Get LagIncrementCom() As String
  LagIncrementCom = pLagIncrementCom
End Property
Public Property Let LagIncrementCom(ByVal newvalue As String)
  pLagIncrementCom = newvalue
End Property

Public Property Get FillValue() As Single
  FillValue = pFillValue
End Property
Public Property Let FillValue(ByVal newvalue As Single)
  pFillValue = newvalue
End Property

Public Property Get FillValueCom() As String
  FillValueCom = pFillValueCom
End Property
Public Property Let FillValueCom(ByVal newvalue As String)
  pFillValueCom = newvalue
End Property

Public Sub Populate(str As String)
  'str is contents of SERIES_DISPLACE block (within header and END records)
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
        Case "LAG_INCREMENT"
          pLagIncrement = spec
          If Len(com) > 0 Then pLagIncrementCom = com
        Case "FILL_VALUE"
          pFillValue = spec
          If Len(com) > 0 Then pFillValueCom = com
      End Select
      com = "" 'reset comment string after processing a keyword
    End If
  Wend
End Sub

Public Function WriteBlock() As String
  Dim str As String
  Dim lv As Variant

  If Len(pComments) > 0 Then
    str = pComments & vbCrLf & "START SERIES_DISPLACE"
  Else
    str = "START SERIES_DISPLACE"
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
  If Len(pLagIncrementCom) > 0 Then str = str & vbCrLf & pLagIncrementCom
  str = str & vbCrLf & "  LAG_INCREMENT " & pLagIncrement
  If Len(pFillValueCom) > 0 Then str = str & vbCrLf & pFillValueCom
  str = str & vbCrLf & "  FILL_VALUE " & pFillValue
  str = str & vbCrLf & "END SERIES_DISPLACE"
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
  pLagIncrement = 0
  pLagIncrementCom = ""
  pFillValue = 0#
  pFillValueCom = ""
End Sub
