VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "clsPestTSPSeriesStats"
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
Private pNewSTableName As String
Private pNewSTableNameCom As String
Private pSumOpt As String             '"yes" or "no" (default)
Private pSumOptCom As String
Private pMeanOpt As String            '"yes" or "no" (default)
Private pMeanOptCom As String
Private pStdDevOpt As String          '"yes" or "no" (default)
Private pStdDevOptCom As String
Private pMaxOpt As String             '"yes" or "no" (default)
Private pMaxOptCom As String
Private pMinOpt As String             '"yes" or "no" (default)
Private pMinOptCom As String
Private pLogOpt As String             'use Log10 of tser values, "yes" or "no" (default)
Private pLogOptCom As String
Private pPower As Single              'raise tser values to this power, >0, default of 1.0
Private pPowerCom As String
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

Public Property Get NewSTableName() As String
  NewSTableName = pNewSTableName
End Property
Public Property Let NewSTableName(ByVal newvalue As String)
  pNewSTableName = newvalue
End Property

Public Property Get NewSTableNameCom() As String
  NewSTableNameCom = pNewSTableNameCom
End Property
Public Property Let NewSTableNameCom(ByVal newvalue As String)
  pNewSTableNameCom = newvalue
End Property

Public Property Get SumOpt() As String
  SumOpt = pSumOpt
End Property
Public Property Let SumOpt(ByVal newvalue As String)
  pSumOpt = newvalue
End Property

Public Property Get SumOptCom() As String
  SumOptCom = pSumOptCom
End Property
Public Property Let SumOptCom(ByVal newvalue As String)
  pSumOptCom = newvalue
End Property

Public Property Get MeanOpt() As String
  MeanOpt = pMeanOpt
End Property
Public Property Let MeanOpt(ByVal newvalue As String)
  pMeanOpt = newvalue
End Property

Public Property Get MeanOptCom() As String
  MeanOptCom = pMeanOptCom
End Property
Public Property Let MeanOptCom(ByVal newvalue As String)
  pMeanOptCom = newvalue
End Property

Public Property Get StdDevOpt() As String
  StdDevOpt = pStdDevOpt
End Property
Public Property Let StdDevOpt(ByVal newvalue As String)
  pStdDevOpt = newvalue
End Property

Public Property Get StdDevOptCom() As String
  StdDevOptCom = pStdDevOptCom
End Property
Public Property Let StdDevOptCom(ByVal newvalue As String)
  pStdDevOptCom = newvalue
End Property

Public Property Get MaxOpt() As String
  MaxOpt = pMaxOpt
End Property
Public Property Let MaxOpt(ByVal newvalue As String)
  pMaxOpt = newvalue
End Property

Public Property Get MaxOptCom() As String
  MaxOptCom = pMaxOptCom
End Property
Public Property Let MaxOptCom(ByVal newvalue As String)
  pMaxOptCom = newvalue
End Property

Public Property Get MinOpt() As String
  MinOpt = pMinOpt
End Property
Public Property Let MinOpt(ByVal newvalue As String)
  pMinOpt = newvalue
End Property

Public Property Get MinOptCom() As String
  MinOptCom = pMinOptCom
End Property
Public Property Let MinOptCom(ByVal newvalue As String)
  pMinOptCom = newvalue
End Property

Public Property Get LogOpt() As String
  LogOpt = pLogOpt
End Property
Public Property Let LogOpt(ByVal newvalue As String)
  pLogOpt = newvalue
End Property

Public Property Get LogOptCom() As String
  LogOptCom = pLogOptCom
End Property
Public Property Let LogOptCom(ByVal newvalue As String)
  pLogOptCom = newvalue
End Property

Public Property Get Power() As Single
  Power = pPower
End Property
Public Property Let Power(ByVal newvalue As Single)
  pPower = newvalue
End Property

Public Property Get PowerCom() As String
  PowerCom = pPowerCom
End Property
Public Property Let PowerCom(ByVal newvalue As String)
  pPowerCom = newvalue
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
  'str is contents of SERIES_STATISTICS block (within header and END records)
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
        Case "NEW_STABLE_NAME"
          pNewSTableName = spec
          If Len(com) > 0 Then pNewSTableNameCom = com
        Case "SUM"
          pSumOpt = spec
          If Len(com) > 0 Then pSumOptCom = com
        Case "MEAN"
          pMeanOpt = spec
          If Len(com) > 0 Then pMeanOptCom = com
        Case "STD_DEV"
          pStdDevOpt = spec
          If Len(com) > 0 Then pStdDevOptCom = com
        Case "MAXIMUM"
          pMaxOpt = spec
          If Len(com) > 0 Then pMaxOptCom = com
        Case "MINIMUM"
          pMinOpt = spec
          If Len(com) > 0 Then pMinOptCom = com
        Case "LOG"
          pLogOpt = spec
          If Len(com) > 0 Then pLogOptCom = com
        Case "POWER"
          pPower = spec
          If Len(com) > 0 Then pPowerCom = com
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
    str = pComments & vbCrLf & "START SERIES_STATISTICS"
  Else
    str = "START SERIES_STATISTICS"
  End If
  For Each lv In pContext
    If pContextCom.Count > 0 Then
      If pContextCom.KeyExists(lv) Then str = str & vbCrLf & pContextCom(lv)
    End If
    str = str & vbCrLf & "  CONTEXT " & lv
  Next
  If Len(pSeriesNameCom) > 0 Then str = str & vbCrLf & pSeriesNameCom
  str = str & vbCrLf & "  SERIES_NAME " & pSeriesName
  If Len(pNewSTableNameCom) > 0 Then str = str & vbCrLf & pNewSTableNameCom
  str = str & vbCrLf & "  NEW_STABLE_NAME " & pNewSTableName
  If Len(pSumOptCom) > 0 Then str = str & vbCrLf & pSumOptCom
  If Len(pSumOpt) > 0 Then str = str & vbCrLf & "  SUM " & pSumOpt
  If Len(pMeanOptCom) > 0 Then str = str & vbCrLf & pMeanOptCom
  If Len(pMeanOpt) > 0 Then str = str & vbCrLf & "  MEAN " & pMeanOpt
  If Len(pStdDevOptCom) > 0 Then str = str & vbCrLf & pStdDevOptCom
  If Len(pStdDevOpt) > 0 Then str = str & vbCrLf & "  STD_DEV " & pStdDevOpt
  If Len(pMaxOptCom) > 0 Then str = str & vbCrLf & pMaxOptCom
  If Len(pMaxOpt) > 0 Then str = str & vbCrLf & "  MAXIMUM " & pMaxOpt
  If Len(pMinOptCom) > 0 Then str = str & vbCrLf & pMinOptCom
  If Len(pMinOpt) > 0 Then str = str & vbCrLf & "  MINIMUM " & pMinOpt
  If Len(pLogOptCom) > 0 Then str = str & vbCrLf & pLogOptCom
  If Len(pLogOpt) > 0 Then str = str & vbCrLf & "  LOG " & pLogOpt
  If Len(pPowerCom) > 0 Then str = str & vbCrLf & pPowerCom
  If Len(pPower) > 0 Then str = str & vbCrLf & "  POWER " & pPower
  If Len(pDate1Com) > 0 Then str = str & vbCrLf & pDate1Com
  If Len(pDate1) > 0 Then str = str & vbCrLf & "  DATE_1 " & pDate1
  If Len(pTime1Com) > 0 Then str = str & vbCrLf & pTime1Com
  If Len(pTime1) > 0 Then str = str & vbCrLf & "  TIME_1 " & pTime1
  If Len(pDate2Com) > 0 Then str = str & vbCrLf & pDate2Com
  If Len(pDate2) > 0 Then str = str & vbCrLf & "  DATE_2 " & pDate2
  If Len(pTime2Com) > 0 Then str = str & vbCrLf & pTime2Com
  If Len(pTime2) > 0 Then str = str & vbCrLf & "  TIME_2 " & pTime2
  str = str & vbCrLf & "END SERIES_STATISTICS"
  WriteBlock = str

End Function

Private Sub Class_Initialize()
  pComments = ""
  Set pContext = New FastCollection
  Set pContextCom = New FastCollection
  pSeriesName = ""
  pSeriesNameCom = ""
  pNewSTableName = ""
  pNewSTableNameCom = ""
  pSumOpt = "no"
  pSumOptCom = ""
  pMeanOpt = "no"
  pMeanOptCom = ""
  pStdDevOpt = "no"
  pStdDevOptCom = ""
  pMaxOpt = "no"
  pMaxOptCom = ""
  pMinOpt = "no"
  pMinOptCom = ""
  pLogOpt = "no"
  pLogOptCom = ""
  pPower = 1#
  pPowerCom = ""
  pDate1 = ""
  pDate1Com = ""
  pTime1 = ""
  pTime1Com = "00:00:00"
  pDate2 = ""
  pDate2Com = ""
  pTime2 = ""
  pTime2Com = "00:00:00"
End Sub
