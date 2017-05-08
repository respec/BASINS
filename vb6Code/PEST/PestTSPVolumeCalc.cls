VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "clsPestTSPVolumeCalc"
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
Private pNewVTableName As String
Private pNewVTableNameCom As String
Private pDateFileName As String
Private pDateFileNameCom As String
Private pTimeUnits As String          '"year","month","day","hour","min","sec"
Private pTimeUnitsCom As String
Private pFactor As Single             'default is 1.0
Private pFactorCom As String


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

Public Property Get NewVTableName() As String
  NewVTableName = pNewVTableName
End Property
Public Property Let NewVTableName(ByVal newvalue As String)
  pNewVTableName = newvalue
End Property

Public Property Get NewVTableNameCom() As String
  NewVTableNameCom = pNewVTableNameCom
End Property
Public Property Let NewVTableNameCom(ByVal newvalue As String)
  pNewVTableNameCom = newvalue
End Property

Public Property Get DateFileName() As String
  DateFileName = pDateFileName
End Property
Public Property Let DateFileName(ByVal newvalue As String)
  pDateFileName = newvalue
End Property

Public Property Get DateFileNameCom() As String
  DateFileNameCom = pDateFileNameCom
End Property
Public Property Let DateFileNameCom(ByVal newvalue As String)
  pDateFileNameCom = newvalue
End Property

Public Property Get TimeUnits() As String
  TimeUnits = pTimeUnits
End Property
Public Property Let TimeUnits(ByVal newvalue As String)
  pTimeUnits = newvalue
End Property

Public Property Get TimeUnitsCom() As String
  TimeUnitsCom = pTimeUnitsCom
End Property
Public Property Let TimeUnitsCom(ByVal newvalue As String)
  pTimeUnitsCom = newvalue
End Property

Public Property Get Factor() As Single
  Factor = pFactor
End Property
Public Property Let Factor(ByVal newvalue As Single)
  pFactor = newvalue
End Property

Public Property Get FactorCom() As String
  FactorCom = pFactorCom
End Property
Public Property Let FactorCom(ByVal newvalue As String)
  pFactorCom = newvalue
End Property

Public Sub Populate(str As String)
  'str is contents of VOLUME_CALCULATION block (within header and END records)
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
        Case "NEW_V_TABLE_NAME"
          pNewVTableName = spec
          If Len(com) > 0 Then pNewVTableNameCom = com
        Case "DATE_FILE"
          pDateFileName = spec
          If Len(com) > 0 Then pDateFileNameCom = com
        Case "FLOW_TIME_UNITS"
          pTimeUnits = spec
          If Len(com) > 0 Then pTimeUnitsCom = com
        Case "FACTOR"
          pFactor = CSng(spec)
          If Len(com) > 0 Then pFactorCom = com
      End Select
      com = "" 'reset comment string after processing a keyword
    End If
  Wend
End Sub

Public Function WriteBlock() As String
  Dim str As String
  Dim lv As Variant

  If Len(pComments) > 0 Then
    str = pComments & vbCrLf & "START VOLUME_CALCULATION"
  Else
    str = "START VOLUME_CALCULATION"
  End If
  For Each lv In pContext
    If pContextCom.Count > 0 Then
      If pContextCom.KeyExists(lv) Then str = str & vbCrLf & pContextCom(lv)
    End If
    str = str & vbCrLf & "  CONTEXT " & lv
  Next
  If Len(pSeriesNameCom) > 0 Then str = str & vbCrLf & pSeriesNameCom
  str = str & vbCrLf & "  SERIES_NAME " & pSeriesName
  If Len(pNewVTableNameCom) > 0 Then str = str & vbCrLf & pNewVTableNameCom
  str = str & vbCrLf & "  NEW_V_TABLE_NAME " & pNewVTableName
  If Len(pDateFileNameCom) > 0 Then str = str & vbCrLf & pDateFileNameCom
  str = str & vbCrLf & "  DATE_FILE " & pDateFileName
  If Len(pTimeUnitsCom) > 0 Then str = str & vbCrLf & pTimeUnitsCom
  str = str & vbCrLf & "  FLOW_TIME_UNITS " & pTimeUnits
  If Len(pFactorCom) > 0 Then str = str & vbCrLf & pFactorCom
  str = str & vbCrLf & "  FACTOR " & pFactor
  str = str & vbCrLf & "END VOLUME_CALCULATION"
  WriteBlock = str

End Function

Private Sub Class_Initialize()
  pComments = ""
  Set pContext = New FastCollection
  Set pContextCom = New FastCollection
  pSeriesName = ""
  pSeriesNameCom = ""
  pNewVTableName = ""
  pNewVTableNameCom = ""
  pDateFileName = ""
  pDateFileNameCom = ""
  pTimeUnits = ""
  pTimeUnitsCom = ""
  pFactor = 1#
  pFactorCom = ""
End Sub

Public Function Valid(Prop As String) As FastCollection
  Dim lProp As String
  Dim lColl As FastCollection

  Set lColl = Nothing
  lProp = LCase(Prop)
  If lProp = "flow_time_units" Then
    Set lColl = New FastCollection
    lColl.Add "year"
    lColl.Add "month"
    lColl.Add "day"
    lColl.Add "hour"
    lColl.Add "min"
    lColl.Add "sec"
  End If
  Set Valid = lColl
End Function
