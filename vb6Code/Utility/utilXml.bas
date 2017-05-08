Attribute VB_Name = "utilXml"
Option Explicit

' cousin to routine by same name in ATCDatabase, should it be in util???
Function xml2str(xml As String) As String
  Dim str As String
  
  str = ReplaceString(xml, "&lt;", "<")
  str = ReplaceString(str, "&gt;", ">")
  str = ReplaceString(str, "&amp;", "&")
  str = ReplaceString(str, "&quot;", """")
  str = ReplaceString(str, vbLf, vbCrLf) 'what if its already vbCrLf?
  xml2str = str
End Function

Function GetChildrenWithTag(aSource As ChilkatXml, aTag As String) As FastCollection
  Dim lNode As ChilkatXml
  Dim retval As New FastCollection
  Dim lTag As String
  lTag = LCase(aTag)
  
  If Not aSource Is Nothing Then
    Set lNode = aSource.firstChild
    While Not lNode Is Nothing
      If LCase(lNode.Tag) = lTag Then retval.Add lNode
      Set lNode = lNode.nextSibling 'Not using NextSibling2 because that changes what we added to retval
    Wend
  End If
  
  Set GetChildrenWithTag = retval
End Function

Sub RemoveChildrenWithTag(aSource As ChilkatXml, aTag As String)
  Dim vNode As Variant 'ChilkatXML
  Dim lChildren As FastCollection
  Set lChildren = GetChildrenWithTag(aSource, aTag)
  For Each vNode In lChildren
    vNode.RemoveFromTree
  Next
End Sub

Function CreateXmlElement(aTag As String, aContent As String) As ChilkatXml
  Set CreateXmlElement = New ChilkatXml
  CreateXmlElement.Tag = aTag
  CreateXmlElement.Content = aContent
End Function

Sub CopyXML(aSource As ChilkatXml, aDest As ChilkatXml)
  Dim i As Long
  Dim oldTag As String
  Dim newChild As ChilkatXml
  
  aDest.RemoveAllAttributes
  While aDest.NumChildren > 0
    aDest.ExtractChildByIndex 0
  Wend
  
  If aDest.Tag <> aSource.Tag Then
    oldTag = aDest.Tag
    aDest.Tag = aSource.Tag
    aDest.AddAttribute "Alias", oldTag
  End If
  aDest.Content = aSource.Content
  For i = 0 To aSource.NumAttributes - 1
    aDest.AddAttribute aSource.GetAttributeName(i), aSource.GetAttributeValue(i)
  Next
  For i = 0 To aSource.NumChildren - 1
    Set newChild = New ChilkatXml
    newChild.loadXML aSource.GetChild(i).GetXml
    aDest.AddChildTree newChild
    Set newChild = Nothing
  Next
End Sub
