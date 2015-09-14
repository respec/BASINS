Imports atcUtility
Imports MapWinUtility

''' <summary>Manages a set of available Data Extensions and a set of currently open data</summary>
Public Class DataManager

    'Private pExtensions As New atcCollection ' of IDataExtension
    Private pFunctions As New atcCollection
    'Private pPlugins As ICollection

    ''' <summary>
    ''' Create a new instance of DataManager
    ''' </summary>
    Public Sub New() 'ByVal aMapWin As MapWindow.Interfaces.IMapWin)
    End Sub

    '''' <summary>Load an extension class</summary>
    'Public Sub AddExtension(ByVal aNewExtension As IDataExtension)
    '    pExtensions.Add(aNewExtension)
    'End Sub

    '''' <summary>Return all loaded extension classes</summary>
    'Public ReadOnly Property AvailableExtensions() As ICollection
    '    Get
    '        Dim lMatchingPlugIns As New atcCollection
    '        For Each lCurPlugin As MapWindow.Interfaces.IPlugin In pPlugins
    '            If Not lCurPlugin Is Nothing AndAlso TypeOf (lCurPlugin) Is IDataExtension Then
    '                lMatchingPlugIns.Add(lCurPlugin.Name, lCurPlugin)
    '            End If
    '        Next
    '        Return lMatchingPlugIns
    '    End Get
    'End Property

    '''' <summary>Return all functions available</summary>
    '''' <param name="aQueryToMatch">
    ''''     Return only functions that match this query
    '''' </param>  
    '''' <remarks>
    '''' QuerySchema from all loaded extensions are searched for matches
    '''' XML string may be replaced by a custom class that holds queries
    '''' </remarks>
    'Public Function AvailableFunctions(Optional ByVal aQueryToMatch As String = "") As atcCollection
    '    Dim lCollectionQuery As New atcCollection
    '    If aQueryToMatch.Length > 0 Then
    '        Dim lQueryXML As New Xml.XmlDocument
    '        lQueryXML.LoadXml(aQueryToMatch)
    '        Dim lNode As Xml.XmlNode = lQueryXML.FirstChild
    '        For lAttribute As Integer = 1 To lNode.Attributes.Count
    '            lCollectionQuery.Add(lNode.Attributes.Item(lAttribute).Name.ToLower, lNode.Attributes.Item(lAttribute))
    '        Next
    '    End If
    '    Return AvailableFunctions(lCollectionQuery)
    'End Function

    ''' <summary>Convert XML string into an atcCollection</summary>
    Private Function XMLtoCollection(ByVal aXML As String) As atcCollection
        Dim lXML As New Xml.XmlDocument
        lXML.LoadXml(aXML)
        Return XMLtoCollection(lXML)
    End Function

    ''' <summary>Convert Chilkat.Xml object into an atcCollection</summary>
    Private Function XMLtoCollection(ByVal aXML As Xml.XmlDocument) As atcCollection
        Dim lCollection As New atcCollection

        lCollection.Add("<tag>", aXML.Name)
        For lAttribute As Integer = 1 To aXML.Attributes.Count
            lCollection.Add(aXML.Attributes.Item(lAttribute).Name.ToLower, aXML.Attributes.Item(lAttribute))
        Next

        Dim lChildXML As Xml.XmlNode = aXML.FirstChild
        While Not lChildXML Is Nothing
            Dim lChildCollection As atcCollection = XMLtoCollection(lChildXML)
            Dim lChildKey As String = lChildCollection.ItemByKey("Name")
            If lChildKey Is Nothing Then lChildKey = lChildXML.Name
            lCollection.Add(lChildKey.ToLower, lChildCollection)
            lChildXML = lChildXML.NextSibling
        End While

        Return lCollection
    End Function

    '''' <summary>Return all functions available</summary>
    '''' <param name="aQueryToMatch">
    ''''     Return only functions that match this query
    '''' </param>  
    '''' <remarks>
    '''' QuerySchema from all loaded extensions are searched for matches
    '''' </remarks>
    'Public Function AvailableFunctions(ByVal aQueryToMatch As atcCollection) As atcCollection
    '    Dim lFunctions As New atcCollection
    '    If aQueryToMatch.Count = 0 And pFunctions.Count > 0 Then
    '        lFunctions = pFunctions
    '    Else
    '        If aQueryToMatch.Count > 0 And pFunctions.Count = 0 Then
    '            AvailableFunctions()
    '        End If
    '        If pFunctions.Count = 0 Then
    '            For Each lExtension As IDataExtension In AvailableExtensions()
    '                Dim lExtensionFunctions As atcCollection = XMLtoCollection(lExtension.QuerySchema)
    '                For Each lFunctionDefinition As Object In lExtensionFunctions
    '                    If lFunctionDefinition.GetType.Name.EndsWith("atcCollection") Then
    '                        lFunctionDefinition.Add("extension", lExtension)
    '                        lFunctions.Add(lFunctionDefinition.ItemByKey("Name").ToLower, lFunctionDefinition)
    '                    End If
    '                Next
    '            Next
    '            If aQueryToMatch.Count = 0 Then pFunctions = lFunctions
    '        End If
    '        If aQueryToMatch.Count > 0 Then
    '            For Each lFunctionDefinition As Object In pFunctions
    '                'TODO: check whether this function matches aQueryToMatch before adding
    '                lFunctions.Add(lFunctionDefinition.ItemByKey("Name").ToLower, lFunctionDefinition)
    '            Next
    '        End If

    '    End If
    '    Return lFunctions
    'End Function

    'Public ReadOnly Property DataGroups() As ICollection
    '    Get
    '        Return pDataGroups
    '    End Get
    'End Property

    ''' <summary>Perform a function that was described in QuerySchema.</summary>
    ''' <remarks>
    ''' If values are included for all required parameters, no user interaction
    ''' will be required.
    ''' </remarks>
    Public Function Execute(ByVal aQuery As String) As String
        Dim lResult As String = ""
        Try
            Dim lFunctionName As String = ""
            Dim lQueryDoc As New Xml.XmlDocument
            Dim lQueries As New Generic.List(Of String)
            Dim lEndFunction As Integer = aQuery.ToLower.IndexOf("</function>")
            While lEndFunction > 0
                lQueries.Add(aQuery.Substring(0, lEndFunction + 11))
                aQuery = aQuery.Substring(lEndFunction + 11)
                lEndFunction = aQuery.ToLower.IndexOf("</function>")
            End While
            Dim lSingleQuery As Boolean = True
            If lQueries.Count > 1 Then
                lSingleQuery = False
                Logger.Progress(0, lQueries.Count)
            End If
            For Each lQuery As String In lQueries
                lQueryDoc.LoadXml(lQuery)
                Dim lFunction As Xml.XmlNode = lQueryDoc.FirstChild
                If lFunction.Name.ToLower = "function" Then
                    lFunctionName = lFunction.Attributes.GetNamedItem("name").Value
                    Logger.Dbg("Function " & lFunctionName)
                    'Dim lExtension As IDataExtension = GetExtensionSupportingFunction(lFunctionName)
                    'If lExtension IsNot Nothing Then
                    '    Logger.Dbg("Extension " & lExtension.Name)
                    '    Logger.Dbg("Query: " & lQuery)
                    '    'TODO: how do defaults from lExtension.QuerySchema into query?
                    '    Logger.Status(lFunctionName)
                    '    Using lLevel As New ProgressLevel(Not lSingleQuery, lSingleQuery)
                    '        lResult &= lExtension.Execute(lQuery)
                    '    End Using
                    'Else
                    '    Logger.Msg("Cannot find extension for function '" & lFunctionName & "'" & vbCrLf & vbCrLf & lQuery, "Data Manager")
                    'End If
                Else
                    Logger.Msg(lQuery, "DataManager:Cannot handle queries that do not start with function tag", "Data Manager")
                    'TODO: handle queries that specify the end result by finding function(s) that can produce it
                End If
            Next
        Catch lCancelEx As ProgressCancelException
            lResult = "<error>User Canceled</error>" 'TODO: send back partial results???
            Logger.Canceled = False
        Catch lEx As Exception
            lResult = "<error>" & lEx.Message & "</error>"
        End Try
        Return lResult
    End Function

    'Private Function GetExtensionSupportingFunction(ByVal aFunctionName As String) As IDataExtension
    '    aFunctionName = aFunctionName.ToLower
    '    For Each lExtension As IDataExtension In AvailableExtensions()
    '        Dim lSchema As New Xml.XmlDocument
    '        lSchema.LoadXml(lExtension.QuerySchema)
    '        Dim lNode As Xml.XmlNode = lSchema.FirstChild.FirstChild
    '        While Not lNode Is Nothing
    '            If lNode.Attributes.GetNamedItem("name").Value.ToLower = aFunctionName Then Return lExtension
    '            lNode = lNode.NextSibling
    '        End While
    '    Next
    '    'Dim lLastPlugIn As Integer = pMapWin.Plugins.Count() - 1
    '    'For iPlugin As Integer = 0 To lLastPlugIn
    '    '  Dim lCurPlugin As MapWindow.Interfaces.IPlugin = pMapWin.Plugins.Item(iPlugin)
    '    '  If Not lCurPlugin Is Nothing AndAlso TypeOf (lCurPlugin) Is IDataExtension Then
    '    '    Dim lExtension As IDataExtension = lCurPlugin
    '    '    Dim lSchema As New Chilkat.Xml
    '    '    lSchema.LoadXml(lExtension.QuerySchema)
    '    '    lSchema.FirstChild2()
    '    '    While Not lSchema Is Nothing
    '    '      If lSchema.GetAttrValue("name").ToLower = aFunctionName Then Return lExtension
    '    '      If Not lSchema.NextSibling2() Then Exit While
    '    '    End While
    '    '  End If
    '    'Next
    '    Return Nothing
    'End Function
End Class
