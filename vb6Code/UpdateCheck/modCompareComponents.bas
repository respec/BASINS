Attribute VB_Name = "modCompareComponents"
Option Explicit

Public Function CompareComponents(localNode As ChilkatXml, availNode As ChilkatXml) As String
  Dim s As String, nodeText As String
  Dim i As Long
  
  If localNode Is Nothing Then
    s = "" '(Not currently installed)
  ElseIf availNode Is Nothing Then
    s = "" 'No update available, don't bother reporting
  Else
    On Error GoTo NoVersionInfo
    If CompareVersionString(localNode.GetAttrValue("Version"), _
                            availNode.GetAttrValue("Version")) = -1 Then
      
      s = "Newer version available" & vbCrLf
    Else
      s = FindVariableDifferences(localNode, availNode)
    End If
    If Len(s) > 0 Then
      
FindDifferentAttributes:
      
      On Error GoTo ErrHand
      'Find different attributes or ones in just local
      For i = 0 To localNode.NumAttributes - 1
        nodeText = "<unknown>"
        On Error Resume Next 'may be missing
        nodeText = availNode.GetAttrValue(localNode.GetAttributeName(i))
        On Error GoTo ErrHand
        If localNode.GetAttributeValue(i) <> nodeText Then
          s = s & "  Current " & localNode.GetAttributeName(i) & ": " & localNode.GetAttributeValue(i) & " Available: " & nodeText & vbCrLf
        End If
      Next
      
      'Find attributes in available but not in local
'      On Error GoTo LocalMissingAttribute
      For i = 0 To availNode.NumAttributes - 1
        If Len(localNode.GetAttrValue(availNode.GetAttributeName(i))) = 0 Then
          s = s & "   " & availNode.GetAttributeName(i) & " Current: <unknown> Available: " & availNode.GetAttributeValue(i) & vbCrLf
        End If
      Next
      On Error GoTo ErrHand
    
    End If
  End If
  CompareComponents = s
  Exit Function

'LocalMissingAttribute:
'  s = s & "   " & availNode.GetAttributeName(i) & " Current: <unknown> Available: " & availNode.GetAttributeValue(i) & vbCrLf
'  Resume Next

ErrHand:
  CompareComponents = "CompareComponents Error: " & Err.Description
  Exit Function

NoVersionInfo:
  s = "could not compare versions"
  GoTo FindDifferentAttributes
End Function

'Returns -1 if ver1 < ver2
'Returns  0 if ver1 = ver2
'Returns  1 if ver1 > ver2
Private Function CompareVersionString(ByVal ver1 As String, ByVal ver2 As String) As Long
  Dim v1 As Variant
  Dim v2 As Variant
  
  Dim i1 As Long
  Dim i2 As Long
  
  v1 = StrSplit(ver1, ".", "")
  v2 = StrSplit(ver2, ".", "")
  
  If IsNumeric(v1) Then
    i1 = CLng(v1)
    If IsNumeric(v2) Then
      i2 = CLng(v2)
      If i1 < i2 Then
        CompareVersionString = -1
      ElseIf i1 > i2 Then
        CompareVersionString = 1
      Else
        CompareVersionString = CompareVersionString(ver1, ver2)
      End If
    ElseIf i1 > 0 Then
      CompareVersionString = 1
    Else
      CompareVersionString = CompareVersionString(ver1, ver2)
    End If
  Else
    If IsNumeric(v2) Then
      If CLng(v2) > 0 Then
        CompareVersionString = -1
      Else
        CompareVersionString = CompareVersionString(ver1, ver2)
      End If
    Else
      CompareVersionString = 0
    End If
  End If
End Function

Private Function FindVariableDifferences(localNode As ChilkatXml, availNode As ChilkatXml) As String
  Dim retval As String
  Dim tmpCollVars As FastCollection
  Dim child As Variant
  Dim VariableName As String
  Dim curVariables As New FastCollection
  Dim newVariables As New FastCollection
  Dim varIndex As Long
  
  On Error GoTo ErrHandL
  
  If Not localNode Is Nothing Then
    Set tmpCollVars = GetChildrenWithTag(localNode, "variable")
    For Each child In tmpCollVars
      curVariables.Add child.Content, child.GetAttrValue("Name")
NextVariableL:
    Next
  End If
  
  On Error GoTo ErrHandA
  
  If Not availNode Is Nothing Then
    Set tmpCollVars = GetChildrenWithTag(availNode, "variable")
    For Each child In tmpCollVars
      VariableName = child.GetAttrValue("Name")
      varIndex = curVariables.IndexFromKey(VariableName)
      If varIndex = 0 Then
        retval = retval & "New Variable " & VariableName _
                        & " = """ & child.Content & """" & vbCrLf
      ElseIf child.Content <> curVariables.ItemByIndex(varIndex) Then
        retval = retval & "Updated Variable " & VariableName _
                        & " = """ & child.Content & """" & vbCrLf _
                        & "Old value is """ & curVariables.ItemByIndex(varIndex) & """" & vbCrLf
        curVariables.RemoveByIndex varIndex
      Else
        curVariables.RemoveByIndex varIndex
      End If
NextVariableA:
    Next
  End If
  
  For varIndex = 1 To curVariables.Count
    retval = retval & "Removed Variable " & curVariables.key(varIndex) _
                    & " = """ & curVariables.ItemByIndex(varIndex) & """" & vbCrLf
  Next
  
  FindVariableDifferences = retval
  
  Exit Function
  
ErrHandL:
  Resume NextVariableL
ErrHandA:
  Resume NextVariableA
End Function

Public Function FindNodeText(lNode As ChilkatXml, aTag As String) As String
  Dim child As Variant
  Dim lColl As FastCollection
  
  Set lColl = GetChildrenWithTag(lNode, aTag)
  For Each child In lColl
    FindNodeText = FindNodeText & child.Content
  Next
End Function

Public Function MakeComponentXML(filename As String, _
                                 Destination As String, _
                                 Optional Instructions As String = "", _
                                 Optional ReleaseNote As String = "", _
                                 Optional ComponentName As String = "") As String
  Dim xml As String
  Dim thisFileInfo As String
  Dim verString As String
  On Error GoTo SomeError
  
  verString = GetFileVerString(filename)
  While Right(verString, 2) = ".0"
    verString = Left(verString, Len(verString) - 2)
  Wend
  
  If Len(ComponentName) = 0 Then ComponentName = FilenameOnly(filename)
  'SetWinDirs
  xml = "<?xml version=""1.0"" standalone=""no""?>"
  'xml = xml & vbCrLf & "<!DOCTYPE ATCCompMl SYSTEM ""http://hspf.com/pub/download/ATCCompMl.dtd"">"
  xml = xml & vbCrLf & "<ATCCompMl>"
  xml = xml & vbCrLf & "  <Component"
  xml = xml & vbCrLf & "     File=""" & FilenameNoPath(filename) & """"
  xml = xml & vbCrLf & "     Version=""" & verString & """"
  xml = xml & vbCrLf & "     Date=""" & Format(FileDateTime(filename), "MM/DD/YYYY hh:mm:ss am/pm") & """"
  xml = xml & vbCrLf & "     Size=""" & Format(FileLen(filename), "#,###") & """"
  xml = xml & vbCrLf & "     Name=""" & ComponentName & """"
  xml = xml & vbCrLf & "     Destination=""" & Destination & """"
  xml = xml & ">"
  xml = xml & vbCrLf & "     <Instructions>" & vbCrLf & Instructions & vbCrLf & "     </Instructions>"
  xml = xml & vbCrLf & "     <ReleaseNote>" & vbCrLf & ReleaseNote & vbCrLf & "     </ReleaseNote>"
  xml = xml & vbCrLf & "  </Component>"
  xml = xml & vbCrLf & "</ATCCompMl>"
  MakeComponentXML = xml
  Exit Function
SomeError:
  MsgBox Err.Description, vbCritical, "MakeComponentXML"
End Function

