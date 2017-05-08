Attribute VB_Name = "modXML"
Option Explicit
  
'Public Sub MergeRequirements(branch As String, merged As DOMDocument, newDoc As DOMDocument)
'  Dim newChild As Long, oldChild As Long
'  Dim newTag As String, oldTag As String
'  Dim found As Boolean
'  Dim iChild As Long, jChild As Long
'
'  For iChild = 0 To newDoc.childNodes(0).childNodes.Length - 1
'    With newDoc.childNodes(0).childNodes.Item(iChild)
'      If LCase(.nodeName) = branch Then
'        For jChild = 0 To .childNodes.Length - 1
'          newTag = LCase(.childNodes.Item(jChild).nodeName)
'          found = False
'          For oldChild = 0 To merged.documentElement.childNodes.Length - 1
'            If newTag = LCase(merged.documentElement.childNodes.Item(oldChild).nodeName) Then
'              found = True
'              Exit For
'            End If
'          Next
'          If Not found Then
'            merged.documentElement.appendChild .childNodes.Item(jChild).cloneNode(True)
'          End If
'        Next
'      End If
'    End With
'  Next
'
'End Sub

'Public Sub DomStatus(lDom As DOMDocument, f As String)
'  Dim i As Long
'
'  If (Len(f) > 0) Then 'have a file to write status to
'    i = FreeFile(0)
'    Open f For Append As i
'
'    If lDom.parseError = 0 Then
'      Print #i, "DOM Status:OK"
'      Print #i, "  XML:", lDom.xml
'    Else
'      With lDom.parseError
'        Print #i, "DOM Status:Parse Error"
'        Print #i, "  Code: " & .errorCode
'        Print #i, "  Line: " & .Line
'        Print #i, "  lPos: " & .linepos
'        Print #i, "  Reason: " & .reason
'        Print #i, "  Src: " & .srcText
'        Print #i, "  fPos: " & .filepos
'        Print #i, "  Url: " & .Url
'      End With
'    End If
'    Close #i
'  End If
'End Sub

Public Function WebQueryCheck(Query As DOMDocument) As Boolean
  'add more !!!
  WebQueryCheck = True
  If Query.childNodes.Length = 0 Then '
    WebQueryCheck = False
  End If
End Function
