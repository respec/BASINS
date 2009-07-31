Imports System.Xml
Imports MapWinUtility

'Example metadata handling that would belong in ProjectShapefile:
'Dim lMetadataFilename As String = lLayerFilename & ".xml"
'Dim lMetadata As New Metadata(lMetadataFilename) 'Reads existing file or creates new blank metadata if file does not exist
'lShapeFile.Projection = aDesiredProjection       'Saves new projection in file
'Logger.Dbg("Projected " & lLayerFilename)
'lMetadata.AddProcessStep("Projected from '" & aNativeProjection & "' to '" & aDesiredProjection & "'")
''TODO: make sure bounding box is saved in unprojected Latitude and Longitude
'lMetadata.SetBoundingBox(lShapeFile.Extents.xMin, lShapeFile.Extents.xMax, lShapeFile.Extents.yMax, lShapeFile.Extents.yMin)
'IO.File.WriteAllText(lMetadataFilename, lMetadata.ToString)

''' <summary>
''' Create and manage FGDC metadata in XML format
''' </summary>
Public Class Metadata
    Private pXML As XmlDocument
    Private pFilename As String

    Private Const DTD_URL As String = "http://www.fgdc.gov/metadata/fgdc-std-001-1998.dtd"
    Private Shared DTD_Filename As String = Nothing 'Locally cached copy of DTD

    ''' <summary>
    ''' Create a new instance from an XML string
    ''' </summary>
    ''' <param name="aFileName">XML file containing metadata</param>
    Public Sub New(Optional ByVal aFileName As String = Nothing)
        pFilename = aFileName
        If FileExists(aFileName) Then
            Try
                Me.FromString(System.IO.File.ReadAllText(aFileName))
            Catch e As Exception
                Logger.Dbg("Exception reading metadata file '" & aFileName & "': " & e.Message)
                Me.FromString(DefaultXmlString)
            End Try
        Else
            Me.FromString(DefaultXmlString)
        End If
    End Sub

    ''' <summary>
    ''' Metadata structure as a System.Xml.XmlDocument
    ''' </summary>
    ''' <returns>Internal representation of this object</returns>
    ''' <remarks>Manipulation of this object may render it non-compliant with DTD</remarks>
    Public ReadOnly Property XML() As XmlDocument
        Get
            Return pXML
        End Get
    End Property

    ''' <summary>
    ''' Create a new blank instance with all required elements present with "missing" values
    ''' </summary>
    Private Function DefaultXmlString() As String
        Dim lAssembly As Reflection.Assembly = Reflection.Assembly.GetExecutingAssembly
        Dim lStreamReader As New IO.StreamReader(lAssembly.GetManifestResourceStream(lAssembly.GetName().Name & ".DefaultMetadata.xml"))
        Return lStreamReader.ReadToEnd.Replace("<metd>yyyyMMdd</metd>", "<metd>" & Format(Now, "yyyyMMdd") & "</metd>")
        'TODO: add basic user info for current user and anything else we can know that fits
    End Function

    ''' <summary>
    ''' Set bounding box in geographic coordinates
    ''' </summary>
    ''' <param name="aWestBC">west bounding coordinate (decimal degress of longitude)</param>
    ''' <param name="aEastBC">east bounding coordinate (decimal degress of longitude)</param>
    ''' <param name="aNorthBC">north bounding coordinate (decimal degress of latitude)</param>
    ''' <param name="aSouthBC">north bounding coordinate (decimal degress of latitude)</param>
    ''' <remarks></remarks>
    Public Sub SetBoundingBox(ByVal aWestBC As String, ByVal aEastBC As String, ByVal aNorthBC As String, ByVal aSouthBC As String)
        Dim lMetaData As XmlNode = FindOrAddChild(pXML, "metadata")
        Dim lIdinfo As XmlNode = FindOrAddChild(lMetaData, "idinfo")
        Dim lSpdom As XmlNode = FindOrAddChild(lIdinfo, "spdom")
        Dim lBounding As XmlNode = FindOrAddChild(lSpdom, "bounding")
        Dim lWestBC As XmlNode = FindOrAddChild(lBounding, "westbc")
        lWestBC.InnerText = aWestBC
        Dim lEastBC As XmlNode = FindOrAddChild(lBounding, "eastbc")
        lEastBC.InnerText = aEastBC
        Dim lNorthBC As XmlNode = FindOrAddChild(lBounding, "northbc")
        lNorthBC.InnerText = aNorthBC
        Dim lSouthBC As XmlNode = FindOrAddChild(lBounding, "southbc")
        lSouthBC.InnerText = aSouthBC
    End Sub

    ''' <summary>
    ''' Get bounding box in geographic coordinates
    ''' </summary>
    ''' <param name="aWestBC">west bounding coordinate (decimal degress of longitude)</param>
    ''' <param name="aEastBC">east bounding coordinate (decimal degress of longitude)</param>
    ''' <param name="aNorthBC">north bounding coordinate (decimal degress of latitude)</param>
    ''' <param name="aSouthBC">north bounding coordinate (decimal degress of latitude)</param>
    ''' <remarks></remarks>
    Public Sub GetBoundingBox(ByRef aWestBC As String, ByRef aEastBC As String, ByRef aNorthBC As String, ByRef aSouthBC As String)
        Dim lMetaData As XmlNode = FindOrAddChild(pXML, "metadata")
        Dim lIdinfo As XmlNode = FindOrAddChild(lMetaData, "idinfo")
        Dim lSpdom As XmlNode = FindOrAddChild(lIdinfo, "spdom")
        Dim lBounding As XmlNode = FindOrAddChild(lSpdom, "bounding")
        Dim lWestBC As XmlNode = FindOrAddChild(lBounding, "westbc")
        aWestBC = lWestBC.InnerText
        Dim lEastBC As XmlNode = FindOrAddChild(lBounding, "eastbc")
        aEastBC = lEastBC.InnerText
        Dim lNorthBC As XmlNode = FindOrAddChild(lBounding, "northbc")
        aNorthBC = lNorthBC.InnerText
        Dim lSouthBC As XmlNode = FindOrAddChild(lBounding, "southbc")
        aSouthBC = lSouthBC.InnerText
    End Sub

    ''' <summary>
    ''' Add a processing step to lineage within dataqual
    ''' </summary>
    ''' <param name="aDescription">Description of the processing step</param>
    Public Sub AddProcessStep(ByVal aDescription As String)
        AddProcessStep(aDescription, Now)
    End Sub

    ''' <summary>
    ''' Add a processing step to lineage within dataqual
    ''' </summary>
    ''' <param name="aDescription">Description of the processing step</param>
    ''' <param name="aDate">Date of the processing step, defaults to current date and time if omitted</param>
    Public Sub AddProcessStep(ByVal aDescription As String, ByVal aDate As Date)
        Dim lMetaData As XmlNode = FindOrAddChild(pXML, "metadata")
        Dim lDataQual As XmlNode = FindOrAddChild(lMetaData, "dataqual", "idinfo")
        Dim lLogic As XmlNode = FindOrAddChild(lDataQual, "logic")
        Dim lComplete As XmlNode = FindOrAddChild(lDataQual, "complete")
        Dim lLineage As XmlNode = FindOrAddChild(lDataQual, "lineage")

        Dim lProcStep As XmlNode = pXML.CreateNode(XmlNodeType.Element, "procstep", Nothing)
        AppendTextNode(lProcStep, "procdesc", aDescription)
        AppendTextNode(lProcStep, "procdate", Format(aDate, "yyyyMMdd"))
        AppendTextNode(lProcStep, "proctime", Format(aDate, "HHmmssss"))
        lLineage.AppendChild(lProcStep)
        ModifiedDate = Now
    End Sub

    Public Property ModifiedDate() As Date
        Get
            Dim lMetaData As XmlNode = FindOrAddChild(pXML, "metadata")
            Dim lMetaInfo As XmlNode = FindOrAddChild(lMetaData, "metainfo")
            Dim lMetd As XmlNode = FindOrAddChild(lMetaInfo, "metd")
            Dim lDateString As String = lMetd.InnerText
            If lDateString.Length = 8 AndAlso IsNumeric(lDateString) Then
                Return New Date(lMetd.InnerText.Substring(0, 4), lMetd.InnerText.Substring(4, 2), lMetd.InnerText.Substring(6, 2))
            Else
                Throw New ApplicationException("Invalid date format for metd: " & lDateString)
            End If
        End Get
        Set(ByVal newValue As Date)
            Dim lMetaData As XmlNode = FindOrAddChild(pXML, "metadata")
            Dim lMetaInfo As XmlNode = FindOrAddChild(lMetaData, "metainfo")
            Dim lMetd As XmlNode = FindOrAddChild(lMetaInfo, "metd")
            lMetd.InnerText = Format(newValue, "yyyyMMdd")
            'TODO: update metc?
        End Set
    End Property

    Public Property StylesheetURL() As String
        Get
            Dim lStylesheetNode As XmlNode = pXML.FirstChild
            While Not lStylesheetNode Is Nothing AndAlso lStylesheetNode.Name <> "xml-stylesheet"
                lStylesheetNode = lStylesheetNode.NextSibling
            End While
            If lStylesheetNode Is Nothing Then
                Return ""
            Else
                Return lStylesheetNode.Attributes.GetNamedItem("href").InnerText
            End If
        End Get
        Set(ByVal aNewValue As String)
            Dim lNewStylesheetNode As XmlNode = pXML.CreateProcessingInstruction("xml-stylesheet", "type=""text/xsl""" & " href=""" & aNewValue & """")
            'SetAttribute(lNewStylesheetNode, "href", aNewValue)

            Dim lOldStylesheetNode As XmlNode = pXML.FirstChild
            While Not lOldStylesheetNode Is Nothing AndAlso lOldStylesheetNode.Name <> "xml-stylesheet"
                lOldStylesheetNode = lOldStylesheetNode.NextSibling
            End While

            If lOldStylesheetNode Is Nothing Then
                pXML.InsertAfter(lNewStylesheetNode, pXML.FirstChild)
            Else
                pXML.ReplaceChild(lNewStylesheetNode, lOldStylesheetNode)
            End If
        End Set
    End Property

    Private Sub SetAttribute(ByVal aNode As XmlNode, ByVal aAttributeName As String, ByVal aAttributeValue As String)
        Dim lAttribute As XmlAttribute = pXML.CreateAttribute(aAttributeName)
        lAttribute.Value = aAttributeValue
        aNode.AppendChild(lAttribute)
    End Sub

    Private Function AppendTextNode(ByVal aParentNode As XmlNode, ByVal aTag As String, ByVal aText As String) As XmlNode
        Dim lNewNode As XmlNode = pXML.CreateNode(XmlNodeType.Element, aTag, Nothing)
        lNewNode.AppendChild(pXML.CreateTextNode(aText))
        aParentNode.AppendChild(lNewNode)
        Return lNewNode
    End Function

    ''' <summary>
    ''' Find a child node by case-insensitive search for the given tag
    ''' Create a new child with the given tag if not found
    ''' </summary>
    ''' <param name="aNode">Node whose children will be searched</param>
    ''' <param name="aTag">Search for a child with this tag</param>
    ''' <returns>the found or created node</returns>
    Private Function FindOrAddChild(ByVal aNode As XmlNode, ByVal aTag As String, Optional ByVal aAfter As String = Nothing) As XmlNode
        Dim lTag As String = aTag.ToLower
        Dim lChild As XmlNode = aNode.FirstChild
        While Not lChild Is Nothing AndAlso (lChild.Name.ToLower <> lTag OrElse lChild.NodeType <> XmlNodeType.Element)
            lChild = lChild.NextSibling
        End While
        If lChild Is Nothing Then
            lChild = pXML.CreateNode(XmlNodeType.Element, aTag, Nothing)
            If aAfter Is Nothing Then
                aNode.AppendChild(lChild)
            Else
                aNode.InsertAfter(lChild, FindOrAddChild(aNode, aAfter)) 'this assumes that aAfter is present or new child can be at end
            End If
        End If
        Return lChild
    End Function

    ''' <summary>
    ''' Replace the entire state of this object with an XML string
    ''' </summary>
    ''' <param name="aXML">New metadata XML string to use instead of current metadata</param>
    Public Sub FromString(ByVal aXML As String)
        If aXML.Contains(DTD_URL) Then
            If HaveDTDFile Then
                aXML = aXML.Replace(DTD_URL, "file://" & DTD_Filename.Replace("\", "/"))
            End If
        End If

        Dim lStream As New System.IO.StringReader(aXML)
        Dim lSettings As New XmlReaderSettings
        lSettings.ProhibitDtd = False
        lSettings.ValidationType = ValidationType.DTD 'Change to .None to disable validation while reading
        AddHandler lSettings.ValidationEventHandler, AddressOf ValidationEventHandler

        Dim lXMLReader As XmlReader = XmlReader.Create(lStream, lSettings)
        pXML = New XmlDocument
        pXML.Load(lXMLReader)

    End Sub

    Private Function HaveDTDFile() As Boolean
        If DTD_Filename Is Nothing Then
            DTD_Filename = IO.Path.Combine(IO.Path.GetTempPath, "fgdc-std-001-1998.dtd")
            If Not IO.File.Exists(DTD_Filename) Then
                Try
                    Dim lAssembly As Reflection.Assembly = Reflection.Assembly.GetExecutingAssembly
                    Dim lStreamReader As New IO.StreamReader( _
                        lAssembly.GetManifestResourceStream(lAssembly.GetName().Name & ".fgdc-std-001-1998.dtd"))
                    IO.File.WriteAllText(DTD_Filename, lStreamReader.ReadToEnd)
                Catch ex As Exception
                    Logger.Dbg("Could not write Metafile DTD as '" & DTD_Filename & "': " & ex.Message)
                    DTD_Filename = ""
                End Try
            End If
        End If
        Return IO.File.Exists(DTD_Filename)
    End Function

    Private Shared Sub ValidationEventHandler(ByVal sender As Object, ByVal args As Schema.ValidationEventArgs)
        Dim lMsg As String = ""
        If args.Severity = Schema.XmlSeverityType.Warning Then
            lMsg &= "WARNING: "
        ElseIf args.Severity = Schema.XmlSeverityType.Error Then
            lMsg &= "ERROR: "
        End If
        Logger.Dbg(lMsg & args.Message)
    End Sub

    ''' <summary>
    ''' Return an XML string containing current metadata
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function ToString() As String
        Dim lsb As New Text.StringBuilder
        Dim lWriter As New XmlTextWriter(New IO.StringWriter(lsb))
        lWriter.Formatting = Formatting.Indented
        pXML.WriteContentTo(lWriter)
        Dim lXML As String = lsb.ToString
        If HaveDTDFile() Then
            lXML = lXML.Replace("file://" & DTD_Filename.Replace("\", "/"), DTD_URL)
        End If
        Return lXML
    End Function

    ''' <summary>
    ''' Calling save with a file name will write an XML text file with the specified name.
    ''' If no file name is given, the file name this object was created with or previously saved to is used.
    ''' </summary>
    ''' <param name="aFileName">Specifying a filename will save the metadata to a new location.</param>
    ''' <remarks></remarks>
    Public Sub Save(Optional ByVal aFileName As String = Nothing)
        If aFileName Is Nothing Then
            aFileName = pFilename
        End If

        If aFileName Is Nothing Then
            Throw New ApplicationException("No file name given for Metadata.Save")
        Else
            Try
                IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(aFileName))
            Catch
            End Try
            IO.File.WriteAllText(aFileName, Me.ToString)
            pFilename = aFileName
        End If
    End Sub

End Class
