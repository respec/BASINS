VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "clsPestTSPSettings"
Attribute VB_GlobalNameSpace = True
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit
'Copyright 2003 AQUA TERRA Consultants - Royalty-free use permitted under open source license

Private pComments As String      'comments for block
Private pContext As FastCollection
Private pContextCom As FastCollection
Private pContextOn As FastCollection 'boolean of whether or not Context is active
Private pDateFormat As String
Private pDateFormatCom As String
Private pEndComments As String   'any comments after Date and Context records

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

Public Property Get ContextOn() As FastCollection
  Set ContextOn = pContextOn
End Property
Public Property Set ContextOn(ByVal newvalue As FastCollection)
  Set pContextOn = newvalue
End Property

Public Property Get DateFormat() As String
  DateFormat = pDateFormat
End Property
Public Property Let DateFormat(ByVal newvalue As String)
  pDateFormat = newvalue
End Property

Public Property Get DateFormatCom() As String
  DateFormatCom = pDateFormatCom
End Property
Public Property Let DateFormatCom(ByVal newvalue As String)
  pDateFormatCom = newvalue
End Property

Public Property Get EndComments() As String
  EndComments = pEndComments
End Property
Public Property Let EndComments(newvalue As String)
  pEndComments = newvalue
End Property

Public Sub Populate(str As String)
  'str is contents of SETTINGS block (within header and END records)
  Dim rec As String, com As String, kwd As String, spec As String
  Dim ContextOn As Boolean

  com = ""
  While Len(str) > 0
    rec = Trim(StrSplit(str, vbCrLf, ""))
    'look for CONTEXT, even if its commented
    If InStr(rec, "CONTEXT") > 0 Then
      If Left(rec, 1) = "#" Then
        ContextOn = False
        rec = Mid(rec, 2) 'process as a regular record (not a comment)
      Else
        ContextOn = True
      End If
    End If
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
          pContext.Add spec
          pContextOn.Add ContextOn, spec
          If Len(com) > 0 Then pContextCom.Add com, spec
        Case "DATE_FORMAT"
          pDateFormat = spec
          If Len(com) > 0 Then pDateFormatCom = com
      End Select
      com = "" 'reset comment string after processing a keyword
    End If
  Wend
  If Len(com) > 0 Then 'comments at end of block
    pEndComments = Trim(com) 'likely a commented Context record, remove leading blanks
  End If
End Sub

Public Function WriteBlock() As String
  Dim str As String, commstr As String
  Dim lv As Variant

  If Len(pComments) > 0 Then
    str = pComments & vbCrLf & "START SETTINGS"
  Else
    str = "START SETTINGS"
  End If
  If Len(pDateFormatCom) > 0 Then str = str & vbCrLf & pDateFormatCom
  str = str & vbCrLf & "  DATE_FORMAT " & pDateFormat
  For Each lv In pContext
    If pContextCom.Count > 0 Then
      If pContextCom.KeyExists(lv) Then
        str = str & vbCrLf & ContextCom(lv)
      End If
    End If
    commstr = "" 'assume this CONTEXT won't be commented
    If pContextOn.Count > 0 Then
      If pContextOn.KeyExists(lv) Then
        If Not pContextOn(lv) Then
          commstr = "#"
        End If
      End If
    End If
    str = str & vbCrLf & commstr & "  CONTEXT " & lv
  Next
  If Len(pEndComments) > 0 Then str = str & vbCrLf & pEndComments
  str = str & vbCrLf & "END SETTINGS"
  WriteBlock = str

End Function

Private Sub Class_Initialize()
  pComments = ""
  Set pContext = New FastCollection
  Set pContextCom = New FastCollection
  Set pContextOn = New FastCollection
  pDateFormat = ""
  pDateFormatCom = ""
  pEndComments = ""
End Sub
