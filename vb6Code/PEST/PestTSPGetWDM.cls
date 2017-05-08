VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "clsPestTSPGetWDM"
Attribute VB_GlobalNameSpace = True
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit
'Copyright 2003 AQUA TERRA Consultants - Royalty-free use permitted under open source license

Private pComments As String           'comments for block
Private pContext As FastCollection    'of CONTEXT strings
Private pContextCom As FastCollection 'of CONTEXT comment strings
Private pWDMFile As String
Private pWDMFileCom As String
Private pDSN As Long
Private pDSNCom As String
Private pSeriesName As String
Private pSeriesNameCom As String
Private pDate1 As String
Private pDate1Com As String
Private pTime1 As String
Private pTime1Com As String
Private pDate2 As String
Private pDate2Com As String
Private pTime2 As String
Private pTime2Com As String
Private pDefTime As String            'default time for time step of days or greater
Private pDefTimeCom As String
Private pFilter As String             'value to ignore on import to TSPROC
Private pFilterCom As String

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

Public Property Get WDMFile() As String
  WDMFile = pWDMFile
End Property
Public Property Let WDMFile(ByVal newvalue As String)
  pWDMFile = newvalue
End Property

Public Property Get WDMFileCom() As String
  WDMFileCom = pWDMFileCom
End Property
Public Property Let WDMFileCom(ByVal newvalue As String)
  pWDMFileCom = newvalue
End Property

Public Property Get DSN() As Long
  DSN = pDSN
End Property
Public Property Let DSN(ByVal newvalue As Long)
  pDSN = newvalue
End Property

Public Property Get DSNCom() As String
  DSNCom = pDSNCom
End Property
Public Property Let DSNCom(ByVal newvalue As String)
  pDSNCom = newvalue
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

Public Property Get DefTime() As String
  DefTime = pDefTime
End Property
Public Property Let DefTime(ByVal newvalue As String)
  pDefTime = newvalue
End Property

Public Property Get DefTimeCom() As String
  DefTimeCom = pDefTimeCom
End Property
Public Property Let DefTimeCom(ByVal newvalue As String)
  pDefTimeCom = newvalue
End Property

Public Property Get Filter() As String
  Filter = pFilter
End Property
Public Property Let Filter(ByVal newvalue As String)
  pFilter = newvalue
End Property

Public Property Get FilterCom() As String
  FilterCom = pFilterCom
End Property
Public Property Let FilterCom(ByVal newvalue As String)
  pFilterCom = newvalue
End Property

Public Sub Populate(str As String)
  'str is contents of GET_SERIES_WDM block (within header and END records)
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
        Case "FILE"
          pWDMFile = spec
          If Len(com) > 0 Then pWDMFileCom = com
        Case "DSN"
          pDSN = CLng(spec)
          If Len(com) > 0 Then pDSNCom = com
        Case "NEW_SERIES_NAME"
          pSeriesName = spec
          If Len(com) > 0 Then pSeriesNameCom = com
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
        Case "DEF_TIME"
          pDefTime = spec
          If Len(com) > 0 Then pDefTimeCom = com
        Case "FILTER"
          pFilter = spec
          If Len(com) > 0 Then pFilterCom = com
      End Select
      com = "" 'reset comment string after processing a keyword
    End If
  Wend
End Sub

Public Function WriteBlock() As String
  Dim str As String
  Dim lv As Variant

  If Len(pComments) > 0 Then
    str = pComments & vbCrLf & "START GET_SERIES_WDM"
  Else
    str = "START GET_SERIES_WDM"
  End If
  For Each lv In pContext
    If pContextCom.Count > 0 Then
      If pContextCom.KeyExists(lv) Then str = str & vbCrLf & pContextCom(lv)
    End If
    str = str & vbCrLf & "  CONTEXT " & lv
  Next
  If Len(pWDMFileCom) > 0 Then str = str & vbCrLf & pWDMFileCom
  str = str & vbCrLf & "  FILE " & pWDMFile
  If Len(pDSNCom) > 0 Then str = str & vbCrLf & pDSNCom
  str = str & vbCrLf & "  DSN " & pDSN
  If Len(pSeriesNameCom) > 0 Then str = str & vbCrLf & pSeriesNameCom
  'add "a" at beginning of name for retrieving all data
  str = str & vbCrLf & "  NEW_SERIES_NAME a" & pSeriesName
  If Len(pDefTimeCom) > 0 Then str = str & vbCrLf & pDefTimeCom
  If Len(pDefTime) > 0 Then str = str & vbCrLf & "  DEF_TIME " & pDefTime
  If Len(pFilterCom) > 0 Then str = str & vbCrLf & pFilterCom
  If Len(pFilter) > 0 Then str = str & vbCrLf & "  FILTER " & pFilter
  str = str & vbCrLf & "END GET_SERIES_WDM"
  If Len(pDate1) > 0 Or Len(pTime1) > 0 Or Len(pDate2) > 0 Or Len(pTime2) > 0 Then
    'write block to reduce time span
    str = str & vbCrLf & "START REDUCE_TIME_SPAN"
    For Each lv In pContext
      If pContextCom.Count > 0 Then
        If pContextCom.KeyExists(lv) Then str = str & vbCrLf & pContextCom(lv)
      End If
      str = str & vbCrLf & "  CONTEXT " & lv
    Next
    str = str & vbCrLf & "  SERIES_NAME a" & pSeriesName
    str = str & vbCrLf & "  NEW_SERIES_NAME " & pSeriesName
    If Len(pDate1Com) > 0 Then str = str & vbCrLf & pDate1Com
    If Len(pDate1) > 0 Then str = str & vbCrLf & "  DATE_1 " & pDate1
    If Len(pTime1Com) > 0 Then str = str & vbCrLf & pTime1Com
    If Len(pTime1) > 0 Then str = str & vbCrLf & "  TIME_1 " & pTime1
    If Len(pDate2Com) > 0 Then str = str & vbCrLf & pDate2Com
    If Len(pDate2) > 0 Then str = str & vbCrLf & "  DATE_2 " & pDate2
    If Len(pTime2Com) > 0 Then str = str & vbCrLf & pTime2Com
    If Len(pTime2) > 0 Then str = str & vbCrLf & "  TIME_2 " & pTime2
    str = str & vbCrLf & "END REDUCE_TIME_SPAN"
  End If
  WriteBlock = str

End Function

Private Sub Class_Initialize()
  pComments = ""
  Set pContext = New FastCollection
  Set pContextCom = New FastCollection
  pWDMFile = ""
  pWDMFileCom = ""
  pDSN = 0
  pDSNCom = ""
  pSeriesName = ""
  pSeriesNameCom = ""
  pDate1 = ""
  pDate1Com = ""
  pTime1 = ""
  pTime1Com = ""
  pDate2 = ""
  pDate2Com = ""
  pTime2 = ""
  pTime2Com = ""
  pDefTime = "24:00:00"
  pFilter = -999
End Sub
