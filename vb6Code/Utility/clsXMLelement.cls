VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "clsXMLelement"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = False
Option Explicit
'Copyright 2001 by AQUA TERRA Consultants

Private pTag As String
Private pBody As String
Private pAttributeValues As Collection
Private pAttributeNames As Collection
Private pSubElements As Collection

Private Sub Class_Initialize()
  Clear
End Sub

Private Sub Clear()
  pTag = ""
  pBody = ""
  Set pAttributeValues = Nothing
  Set pAttributeNames = Nothing
  Set pSubElements = Nothing
  Set pAttributeValues = New Collection
  Set pAttributeNames = New Collection
  Set pSubElements = New Collection
End Sub

Public Property Get tag() As String
  tag = LCase(pTag)
End Property
Public Property Let tag(ByVal newValue As String)
  pTag = newValue
End Property

Public Property Get Body() As String
  Body = LCase(pBody)
End Property
Public Property Let Body(ByVal newValue As String)
  pBody = newValue
End Property

Public Property Get AttributeNames() As Collection
  Set AttributeNames = pAttributeNames
End Property

'Public Property Get AttributeValues() As Collection
'  Set AttributeValues = pAttributeValues
'End Property

Public Property Get AttributeValue(AttribName As Variant, Optional DefaultValue As Variant = "") As Variant
  On Error Resume Next
  AttributeValue = DefaultValue
  AttributeValue = pAttributeValues(LCase(AttribName))
End Property

Public Property Get SubElements() As Collection
  Set SubElements = pSubElements
End Property

Public Sub SetString(ByRef newValue As String)
  Dim Lpos As Long, Rpos As Long, spacePos As Long, tagLen As Long
  Dim EqualPos As Long, StartQuote As Long, EndQuote As Long
  Dim attName As String
  Dim subtags As String
  Dim curChar As String
  Dim newSubTag As clsXMLelement
  
  Clear
  
  Lpos = InStr(newValue, "<")
  Rpos = InStr(Lpos + 1, newValue, ">")
  If Lpos = 0 Or Rpos = 0 Then
'    For spacePos = 1 To Len(newValue)
'      curChar = Mid(newValue, spacePos, 1)
'      If Asc(curChar) > 31 Then pBody = pBody & curChar
'    Next
    newValue = ""
  Else
    spacePos = InStr(Lpos + 1, newValue, " ")
    If spacePos = 0 Or spacePos > Rpos Then
      tagLen = Rpos - Lpos - 1
    Else
      tagLen = spacePos - Lpos - 1
      While spacePos > 0 And spacePos < Rpos
        EqualPos = InStr(spacePos + 1, newValue, "=")
        StartQuote = InStr(EqualPos + 1, newValue, """")
        EndQuote = InStr(StartQuote + 1, newValue, """")
        If EqualPos = 0 Or StartQuote = 0 Or EndQuote = 0 Or EndQuote > Rpos Then
          pBody = Mid(newValue, spacePos + 1, Rpos - spacePos - 1)
          spacePos = 0
        Else
          attName = Trim(Mid(newValue, spacePos + 1, EqualPos - spacePos - 1))
          pAttributeNames.Add attName
          pAttributeValues.Add Trim(Mid(newValue, StartQuote + 1, EndQuote - StartQuote - 1)), _
                               LCase(attName)
          spacePos = InStr(EndQuote + 1, newValue, " ")
        End If
      Wend
    End If
    pTag = StrPrintable(Mid(newValue, Lpos + 1, tagLen))
    Lpos = CloseTagSameNestingLevel(pTag, newValue, Rpos + 1, 0)
    If Lpos > 0 Then
      subtags = Mid(newValue, Rpos + 1, Lpos - Rpos - 1)
      newValue = Mid(newValue, Lpos + Len(pTag) + 3)
      While Len(subtags) > 0
        Set newSubTag = New clsXMLelement
        newSubTag.SetString subtags
        If Len(newSubTag.tag) > 0 Or Len(newSubTag.Body) > 0 Then
          pSubElements.Add newSubTag
        ElseIf Len(subtags) > 0 Then
          Debug.Print "Discarding potential subtags of " & pTag & ": " & subtags
          subtags = ""
        End If
        Set newSubTag = Nothing
      Wend
    ElseIf Rpos > 0 Then
      newValue = Mid(newValue, Rpos + 1)
    Else
      newValue = ""
    End If
  End If
End Sub

Private Function CloseTagSameNestingLevel(tag As String, str As String, startPos As Long, NestLevel As Long) As Long
  Dim BeginPos As Long
  Dim EndPos As Long
  BeginPos = InStr(startPos, str, "<" & tag)
  EndPos = InStr(startPos, str, "</" & tag)
  If BeginPos = 0 Then
    CloseTagSameNestingLevel = EndPos
  ElseIf EndPos = 0 Then
    CloseTagSameNestingLevel = 0
  ElseIf EndPos < BeginPos Then
    If NestLevel = 0 Then
      CloseTagSameNestingLevel = EndPos
    Else
      CloseTagSameNestingLevel = CloseTagSameNestingLevel(tag, str, EndPos + 1, NestLevel - 1)
    End If
  Else
    CloseTagSameNestingLevel = CloseTagSameNestingLevel(tag, str, BeginPos + 1, NestLevel + 1)
  End If
End Function

Public Function GetString() As String
  Dim v As Variant
  Dim retval As String
  If Len(pTag) > 0 Then retval = "<" & pTag
  If Len(pBody) > 0 Then
    If Len(pTag) > 0 Then retval = retval & " "
    retval = retval & pBody
  End If
  
  For Each v In pAttributeNames
    retval = retval & " " & v & "=""" & pAttributeValues(v) & """"
  Next
  
  If Len(pTag) > 0 Then retval = retval & ">"
  If pSubElements.Count > 0 Then
    For Each v In pSubElements
      retval = retval & vbCrLf & v.GetString & vbCrLf
    Next
    If Len(pTag) > 0 Then retval = retval & "</" & pTag & ">"
  End If
  GetString = retval
End Function
