VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "clsPestTSPExceedence"
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
Private pETableName As String
Private pETableNameCom As String
Private pTimeUnits As String          '"year","month","day","hour","min","sec"
Private pTimeUnitsCom As String
Private pUnderOver As String          '"under" or "over"(default)
Private pUnderOverCom As String
Private pFlow As FastCollection       'of real numbers
Private pFlowCom As FastCollection
Private pDelay As FastCollection      'if used, must coincide with Flow collection
Private pDelayCom As FastCollection

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

Public Property Get ETableName() As String
  ETableName = pETableName
End Property
Public Property Let ETableName(ByVal newvalue As String)
  pETableName = newvalue
End Property

Public Property Get ETableNameCom() As String
  ETableNameCom = pETableNameCom
End Property
Public Property Let ETableNameCom(ByVal newvalue As String)
  pETableNameCom = newvalue
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

Public Property Get UnderOver() As String
  UnderOver = pUnderOver
End Property
Public Property Let UnderOver(ByVal newvalue As String)
  pUnderOver = newvalue
End Property

Public Property Get UnderOverCom() As String
  UnderOverCom = pUnderOverCom
End Property
Public Property Let UnderOverCom(ByVal newvalue As String)
  pUnderOverCom = newvalue
End Property

Public Property Get Flow() As FastCollection
  Set Flow = pFlow
End Property
Public Property Let Flow(ByVal newvalue As FastCollection)
  Set pFlow = newvalue
End Property

Public Property Get FlowCom() As FastCollection
  Set FlowCom = pFlowCom
End Property
Public Property Let FlowCom(ByVal newvalue As FastCollection)
  Set pFlowCom = newvalue
End Property

Public Property Get Delay() As FastCollection
  Set Delay = pDelay
End Property
Public Property Let Delay(ByVal newvalue As FastCollection)
  Set pDelay = newvalue
End Property

Public Property Get DelayCom() As FastCollection
  Set DelayCom = pDelayCom
End Property
Public Property Let DelayCom(ByVal newvalue As FastCollection)
  Set pDelayCom = newvalue
End Property

Public Sub Populate(str As String)
  'str is contents of EXCEEDENCE_TIME block (within header and END records)
  Dim rec As String, com As String, kwd As String, spec As String
  Dim curFlow As String

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
        Case "E_TABLE_NAME"
          pETableName = spec
          If Len(com) > 0 Then pETableNameCom = com
        Case "EXCEEDENCE_TIME_UNITS"
          pTimeUnits = spec
          If Len(com) > 0 Then pTimeUnitsCom = com
        Case "UNDER_OVER"
          pUnderOver = spec
          If Len(com) > 0 Then pUnderOverCom = com
        Case "FLOW"
          ProcessKwdColl spec, pFlow, com, pFlowCom
          curFlow = spec 'save flow as key if needed for Delay collection
        Case "DELAY"
          pDelay.Add spec, curFlow 'use current flow as key to match Delay and Flow occurences
          If Len(com) > 0 Then
            pDelayCom.Add com, curFlow
          End If
      End Select
      com = "" 'reset comment string after processing a keyword
    End If
  Wend
End Sub

Public Function WriteBlock() As String
  Dim str As String
  Dim lv As Variant

  If Len(pComments) > 0 Then
    str = pComments & vbCrLf & "START EXCEEDENCE_TIME"
  Else
    str = "START EXCEEDENCE_TIME"
  End If
  For Each lv In pContext
    If pContextCom.Count > 0 Then
      If pContextCom.KeyExists(lv) Then str = str & vbCrLf & pContextCom(lv)
    End If
    str = str & vbCrLf & "  CONTEXT " & lv
  Next
  If Len(pSeriesNameCom) > 0 Then str = str & vbCrLf & pSeriesNameCom
  str = str & vbCrLf & "  SERIES_NAME " & pSeriesName
  If Len(pETableNameCom) > 0 Then str = str & vbCrLf & pETableNameCom
  str = str & vbCrLf & "  NEW_E_TABLE_NAME " & pETableName
  If Len(pTimeUnitsCom) > 0 Then str = str & vbCrLf & pTimeUnitsCom
  str = str & vbCrLf & "  EXCEEDENCE_TIME_UNITS " & pTimeUnits
  If Len(pUnderOverCom) > 0 Then str = str & vbCrLf & pUnderOverCom
  If Len(pUnderOver) > 0 Then str = str & vbCrLf & "  UNDER_OVER " & pUnderOver
  For Each lv In pFlow
    If pFlowCom.Count > 0 Then
      If pFlowCom.KeyExists(lv) Then str = str & vbCrLf & pFlowCom(lv)
    End If
    str = str & vbCrLf & "  FLOW " & lv
    If pDelay.Count > 0 Then
      If pDelay.KeyExists(lv) Then 'should be matching Delay key for this Flow
        If pDelayCom.Count > 0 Then
          If pDelayCom.KeyExists(lv) Then str = str & vbCrLf & pDelayCom(lv)
        End If
        str = str & vbCrLf & "  DELAY " & pDelay(lv)
      End If
    End If
  Next
  str = str & vbCrLf & "END EXCEEDENCE_TIME"
  WriteBlock = str

End Function

Private Sub Class_Initialize()
  pComments = ""
  Set pContext = New FastCollection
  Set pContextCom = New FastCollection
  pSeriesName = ""
  pSeriesNameCom = ""
  pETableName = ""
  pETableNameCom = ""
  pTimeUnits = ""
  pTimeUnitsCom = ""
  pUnderOver = "over"
  pUnderOverCom = ""
  Set pFlow = New FastCollection
  Set pFlowCom = New FastCollection
  Set pDelay = New FastCollection
  Set pDelayCom = New FastCollection
End Sub

Public Function Valid(Prop As String) As FastCollection
  Dim lProp As String
  Dim lColl As FastCollection

  Set lColl = Nothing
  lProp = LCase(Prop)
  If lProp = "exceedence_time_units" Or lProp = "under_over" Then
    Set lColl = New FastCollection
    Select Case lProp
      Case "exceedence_time_units"
        lColl.Add "year"
        lColl.Add "month"
        lColl.Add "day"
        lColl.Add "hour"
        lColl.Add "min"
        lColl.Add "sec"
      Case "under_over"
        lColl.Add "over"
        lColl.Add "under"
    End Select
  End If
  Set Valid = lColl
End Function
