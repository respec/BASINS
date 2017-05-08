VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "clsPestTSPReduceSpan"
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
Private pDate1 As String
Private pDate1Com As String
Private pTime1 As String
Private pTime1Com As String
Private pDate2 As String
Private pDate2Com As String
Private pTime2 As String
Private pTime2Com As String

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

Public Property Get DATE1() As String
  DATE1 = pDate1
End Property
Public Property Let DATE1(ByVal newvalue As String)
  pDate1 = newvalue
End Property

Public Property Get Date1Com() As String
  Date1Com = pDate1Com
End Property
Public Property Let Date1Com(ByVal newvalue As String)
  pDate1Com = newvalue
End Property

Public Property Get Time1() As String
  Time1 = pTime1
End Property
Public Property Let Time1(ByVal newvalue As String)
  pTime1 = newvalue
End Property

Public Property Get Time1Com() As String
  Time1Com = pTime1Com
End Property
Public Property Let Time1Com(ByVal newvalue As String)
  pTime1Com = newvalue
End Property

Public Property Get DATE2() As String
  DATE2 = pDate2
End Property
Public Property Let DATE2(ByVal newvalue As String)
  pDate2 = newvalue
End Property

Public Property Get Date2Com() As String
  Date2Com = pDate2Com
End Property
Public Property Let Date2Com(ByVal newvalue As String)
  pDate2Com = newvalue
End Property

Public Property Get Time2() As String
  Time2 = pTime2
End Property
Public Property Let Time2(ByVal newvalue As String)
  pTime2 = newvalue
End Property

Public Property Get Time2Com() As String
  Time2Com = pTime2Com
End Property
Public Property Let Time2Com(ByVal newvalue As String)
  pTime2Com = newvalue
End Property

Public Sub Populate(str As String)
  'str is contents of REDUCE_TIME_SPAN block (within header and END records)
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
        Case "DATE_1"
          pDate1 = spec
          If Len(com) > 0 Then pDate1Com = com
        Case "TIME_1"
          pTime1 = spec
          If Len(com) > 0 Then pTime1Com = com
        Case "DATE_2"
          pDate2 = spec
          If Len(com) > 0 Then pDate2Com = com
        Case "TIME_2"
          pTime2 = spec
          If Len(com) > 0 Then pTime2Com = com
      End Select
      com = "" 'reset comment string after processing a keyword
    End If
  Wend
End Sub

Public Function WriteBlock() As String
  Dim str As String
  Dim lv As Variant

  If Len(pComments) > 0 Then
    str = pComments & vbCrLf & "START REDUCE_TIME_SPAN"
  Else
    str = "START REDUCE_TIME_SPAN"
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
  If Len(pDate1Com) > 0 Then str = str & vbCrLf & pDate1Com
  If Len(pDate1) > 0 Then str = str & vbCrLf & "  DATE_1 " & pDate1
  If Len(pTime1Com) > 0 Then str = str & vbCrLf & pTime1Com
  If Len(pTime1) > 0 Then str = str & vbCrLf & "  TIME_1 " & pTime1
  If Len(pDate2Com) > 0 Then str = str & vbCrLf & pDate2Com
  If Len(pDate2) > 0 Then str = str & vbCrLf & "  DATE_2 " & pDate2
  If Len(pTime2Com) > 0 Then str = str & vbCrLf & pTime2Com
  If Len(pTime2) > 0 Then str = str & vbCrLf & "  TIME_2 " & pTime2
  str = str & vbCrLf & "END REDUCE_TIME_SPAN"
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
  pDate1 = ""
  pDate1Com = ""
  pTime1 = ""
  pTime1Com = "00:00:00"
  pDate2 = ""
  pDate2Com = ""
  pTime2 = ""
  pTime2Com = "00:00:00"
End Sub
