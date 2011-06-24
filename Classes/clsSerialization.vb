'********************************************************************************************************
'File Name: clsColors.vb
'Description: serialization functions
'********************************************************************************************************
'The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License"); 
'you may not use this file except in compliance with the License. You may obtain a copy of the License at 
'http://www.mozilla.org/MPL/ 
'Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF 
'ANY KIND, either express or implied. See the License for the specificlanguage governing rights and 
'limitations under the License. 
'
'The Original Code is MapWindow Open Source Utility Library. 
'Contributor(s): (Open source contributors should list themselves and their modifications here). 
'
'********************************************************************************************************

Imports System.Xml
Imports System.Xml.Serialization

Public Class Serialization

    ''' <summary>
    ''' A generic method for serialization of object as a child of the given XML node
    ''' </summary>
    Public Shared Sub Serialize(ByRef obj As Object, ByRef Parent As XmlElement)

        ' creating serializer for the object
        Dim serializer As New XmlSerializer(obj.GetType())

        ' creating writer for the specified node
        Dim wr As XmlWriter = Parent.CreateNavigator.AppendChild()

        ' Serialize method will try to create new Doc otherwise and will violate wr.Settings.ConformanceLevel = Fragment
        wr.WriteComment("")

        ' for not writing namespace information
        Dim namespaces As New XmlSerializerNamespaces()
        namespaces.Add(String.Empty, String.Empty)

        ' serialization
        Try
            serializer.Serialize(wr, obj, namespaces)
        Catch
            ' do nothing
        End Try
        wr.Flush()
        wr.Close()

        ' removing comment
        Parent.RemoveChild(Parent.ChildNodes(0))
    End Sub

    ''' <summary>
    ''' A generic method for serialization of object as a child of the given XML node
    ''' </summary>
    Public Shared Sub Serialize2(ByRef obj As Object, ByRef nodeParent As XmlElement)

        ' creating serializer for the object
        Dim xs As New XmlSerializer(obj.GetType())
        Dim writer As IO.StringWriter = New IO.StringWriter()

        ' for not writing namespace information
        Dim namespaces As New XmlSerializerNamespaces()
        namespaces.Add(String.Empty, String.Empty)

        Try
            xs.Serialize(writer, obj, namespaces)
        Catch
            ' do nothing
        End Try
        Dim s As String = writer.ToString()

        ' excluding part <xml> part
        Dim pos As Integer = s.IndexOf(">")
        s = s.Substring(pos + 1) + Environment.NewLine

        nodeParent.InnerXml = s
    End Sub

    ''' <summary>
    ''' Generic method for deserialization of an object from the given node
    ''' </summary>
    ''' <param name="xelParent"></param>
    ''' <param name="t"></param>
    ''' <returns></returns>
    Public Shared Function Deserialize(ByRef xelParent As XmlElement, ByVal t As System.Type) As Object

        ' seeking the name of element; XmlRootAttribute of the class is used
        ' TODO: use name of type otherwise
        Dim ElementName As String = String.Empty
        For Each attr As Attribute In t.GetCustomAttributes(True)
            If TypeOf attr Is XmlRootAttribute Then
                ElementName = CType(attr, XmlRootAttribute).ElementName
            End If
        Next

        If ElementName <> String.Empty Then
            Dim xel As XmlElement = xelParent.Item(ElementName)
            If Not xel Is Nothing Then
                ' creating serializer
                Dim serializer As New XmlSerializer(t)

                ' deserializing
                Dim reader As New XmlNodeReader(xel)
                Try
                    Return serializer.Deserialize(reader)
                Catch
                    Return Nothing
                End Try
            End If
        End If

        Return Nothing
    End Function

    ''' <summary>
    ''' Generic method for deserialization of an object from the given node
    ''' </summary>
    ''' <param name="state"></param>
    ''' <param name="t"></param>
    ''' <returns></returns>
    Public Shared Function Deserialize(ByVal state As String, ByVal t As System.Type) As Object

        Dim doc As New XmlDocument
        doc.LoadXml(state)
        Dim xelParent As XmlElement = doc.DocumentElement

        Dim reader As New XmlNodeReader(xelParent)
        Dim serializer As New XmlSerializer(t)
        Return serializer.Deserialize(reader)
    End Function

End Class
