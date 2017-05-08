VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "clsPestTSPSeriesEquation"
Attribute VB_GlobalNameSpace = True
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit
'Copyright 2003 AQUA TERRA Consultants - Royalty-free use permitted under open source license

Private pComments As String           'comments for block
Private pContext As FastCollection    'of CONTEXT strings
Private pContextCom As FastCollection 'of CONTEXT comment strings
Private pNewSeriesName As String
Private pNewSeriesNameCom As String
Private pEquation As String
Private pEquationCom As String

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

Public Property Get Equation() As String
  Equation = pEquation
End Property
Public Property Let Equation(ByVal newvalue As String)
  pEquation = newvalue
End Property

Public Property Get EquationCom() As String
  EquationCom = pEquationCom
End Property
Public Property Let EquationCom(ByVal newvalue As String)
  pEquationCom = newvalue
End Property

Public Sub Populate(str As String)
  'str is contents of SERIES_EQUATION block (within header and END records)
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
        Case "NEW_SERIES_NAME"
          pNewSeriesName = spec
          If Len(com) > 0 Then pNewSeriesNameCom = com
        Case "EQUATION"
          pEquation = spec
          If Len(com) > 0 Then pEquationCom = com
      End Select
      com = "" 'reset comment string after processing a keyword
    End If
  Wend
End Sub

Public Function WriteBlock() As String
  Dim str As String
  Dim lv As Variant

  If Len(pComments) > 0 Then
    str = pComments & vbCrLf & "START SERIES_EQUATION"
  Else
    str = "START SERIES_EQUATION"
  End If
  For Each lv In pContext
    If pContextCom.Count > 0 Then
      If pContextCom.KeyExists(lv) Then str = str & vbCrLf & pContextCom(lv)
    End If
    str = str & vbCrLf & "  CONTEXT " & lv
  Next
  If Len(pNewSeriesNameCom) > 0 Then str = str & vbCrLf & pNewSeriesNameCom
  str = str & vbCrLf & "  NEW_SERIES_NAME " & pNewSeriesName
  If Len(pEquationCom) > 0 Then str = str & vbCrLf & pEquationCom
  str = str & vbCrLf & "  EQUATION " & pEquation
  str = str & vbCrLf & "END SERIES_EQUATION"
  WriteBlock = str

End Function

Private Sub Class_Initialize()
  pComments = ""
  Set pContext = New FastCollection
  Set pContextCom = New FastCollection
  pNewSeriesName = ""
  pNewSeriesNameCom = ""
  pEquation = ""
  pEquationCom = ""
End Sub
