Imports atcUtility
Imports MapWinUtility

''' <remarks>Copyright 2005 AQUA TERRA Consultants - Royalty-free use permitted under open source license</remarks>
''' <summary>GIS Utilities implemented thru MapWindow</summary>
Public Class GisUtil

    Private Shared pMapWin As MapWindow.Interfaces.IMapWin
    Private Const UseCurrent As Integer = -1

    ''' <summary>Run all tests</summary>
    ''' <returns>Results of testing</returns>
    ''' <requirements>Test data in 
    ''' <strong>c:\test\atcMwGisUtility\data</strong></requirements>
    Public Shared Function RunAllTests() As Boolean
        Dim lTests As New Test_GisUtil
        lTests.init()
        lTests.TestLoadProject()
        lTests.TestNumLayers()
        lTests.TestLayerFileName()
        lTests.TestLayerType()
        lTests.TestLayerName()

        lTests.Testset_CurrentLayer()
        lTests.Testget_CurrentLayer()

        lTests.TestNumFeatures()
        lTests.TestNumFields()
        lTests.TestFieldName()
        lTests.TestFieldIndex()
        lTests.TestFieldType()
        lTests.TestFieldValue()

        lTests.TestFeatureArea()
        lTests.TestFeatureLength()

        lTests.TestOverlappingPolygons()

        lTests.TestGridLayerMaximum()
        lTests.TestGridLayerMinimum()

        Return True
    End Function

    ''' <summary>Map Window Object</summary>
    ''' <exception cref="Exception.html#MappingObjectNotSet" caption="MappingObjectNotSet">Mapping Object Not Set</exception>
    Public Shared WriteOnly Property MappingObject() As MapWindow.Interfaces.IMapWin
        Set(ByVal aNewValue As MapWindow.Interfaces.IMapWin)
            pMapWin = aNewValue
        End Set
    End Property

    ''' <summary>Map Window Object</summary>
    ''' <exception cref="Exception.html#MappingObjectNotSet" caption="MappingObjectNotSet">Mapping Object Not Set</exception>
    Private Shared ReadOnly Property GetMappingObject() As MapWindow.Interfaces.IMapWin
        Get
            If pMapWin Is Nothing Then
                Throw New Exception("GisUtil:Mapping Object Not Set")
            Else
                Return pMapWin
            End If
        End Get
    End Property

    ''' <summary>Layers on map</summary>
    ''' <exception cref="Exception.html#MappingObjectNotSet" caption="MappingObjectNotSet">Mapping Object Not Set</exception>
    Private Shared Function MapLayers() As ArrayList
        Dim lMapLayers As New ArrayList
        Dim lLastLayerIndex As Integer = GetMappingObject.Layers.NumLayers - 1
        For lLayerIndex As Integer = 0 To lLastLayerIndex
            lMapLayers.Add(GetMappingObject.Layers(lLayerIndex))
        Next
        Return lMapLayers
    End Function

    ''' <summary>Check to see if feature index is valid</summary>
    ''' <exception cref="Exception.html#FeatureIndexOutOfRange" caption="FeatureIndexOutOfRange">Feature Index Out of Range</exception>
    ''' <exception cref="Exception.html#MappingObjectNotSet" caption="MappingObjectNotSet">Mapping Object Not Set</exception>
    Private Shared Function FeatureIndexValid(ByVal aFeatureIndex As Integer, ByVal aSf As MapWinGIS.Shapefile) As Boolean
        If aFeatureIndex < 0 Or aFeatureIndex >= aSf.NumShapes Then
            Throw New Exception("GisUtil:FieldValue:Error:FeatureIndex:" & aFeatureIndex & ":OutOfRange:0:" & aSf.NumShapes - 1)
        Else
            Return True
        End If
    End Function

    ''' <summary>Load MapWindow project</summary>
    ''' <param name="aProjectName">
    '''   <para>Filename of project to load</para>
    ''' </param>
    ''' <remarks>Current directory not changed</remarks>
    ''' <exception cref="Exception.html#LoadFailure" caption="LoadFailure">Failure to Load Project</exception>
    ''' <exception cref="Exception.html#FileNotFound" caption="FileNotFound">File Not Found</exception>
    ''' <exception cref="Exception.html#MappingObjectNotSet" caption="MappingObjectNotSet">Mapping Object Not Set</exception>
    Public Shared Sub LoadProject(ByVal aProjectName As String)
        If FileExists(aProjectName) Then
            Dim lBaseDir As String = CurDir()  'dont want to change curdir, save original
            Dim lRet As Boolean = GetMappingObject.Project.Load(aProjectName)
            If Not lRet Then
                Throw New Exception("GisUtil:LoadProject:LoadFailure:" & aProjectName)
            End If
            ChDriveDir(lBaseDir)
        Else
            Throw New Exception("GisUtil:LoadProject:FileNotFound:" & aProjectName)
        End If
    End Sub

    ''' <summary>Current Layer Index (Handle)</summary>
    ''' <exception cref="Exception.html#LayerIndexOutOfRange" caption="LayerIndexOutOfRange">Layer specified by aLayerIndex does not exist</exception>
    ''' <exception cref="Exception.html#MappingObjectNotSet" caption="MappingObjectNotSet">Mapping Object Not Set</exception>
    Public Shared Property CurrentLayer() As Integer
        Get
            Return GetMappingObject.Layers.CurrentLayer
        End Get
        Set(ByVal aLayerIndex As Integer)
            If Not LayerFromIndex(aLayerIndex) Is Nothing Then
                GetMappingObject.Layers.CurrentLayer = aLayerIndex
            End If
        End Set
    End Property

    ''' <summary>Obtain pointer to a shape file from a LayerIndex</summary>
    ''' <param name="aLayerIndex">
    '''   <para>Index of Layer containing ShapeFile</para>
    ''' </param>
    ''' <exception cref="Exception.html#LayerNotShapeFile" caption="LayerNotShapeFile">Layer specified by aLayerIndex is not a ShapeFile</exception>
    ''' <exception cref="Exception.html#LayerIndexOutOfRange" caption="LayerIndexOutOfRange">Layer specified by aLayerIndex does not exist</exception>
    ''' <exception cref="Exception.html#MappingObjectNotSet" caption="MappingObjectNotSet">Mapping Object Not Set</exception>
    Private Shared ReadOnly Property ShapeFileFromIndex(Optional ByVal aLayerIndex As Integer = UseCurrent) As MapWinGIS.Shapefile
        Get
            If aLayerIndex = UseCurrent Then
                aLayerIndex = CurrentLayer
            End If

            Dim lLayer As MapWindow.Interfaces.Layer = LayerFromIndex(aLayerIndex)
            If lLayer.LayerType = MapWindow.Interfaces.eLayerType.LineShapefile OrElse _
              lLayer.LayerType = MapWindow.Interfaces.eLayerType.PointShapefile OrElse _
              lLayer.LayerType = MapWindow.Interfaces.eLayerType.PolygonShapefile Then
                Return lLayer.GetObject
            Else
                Throw New Exception("GisUtil:ShapeFileFromIndex:Error:LayerIndex:" & aLayerIndex & ":Type:" & GetMappingObject.Layers(aLayerIndex).LayerType & ":IsNotShapeFile")
            End If
        End Get
    End Property

    ''' <summary>Obtain pointer to a polygon shape file from a LayerIndex</summary>
    ''' <param name="aLayerIndex">
    '''     <para>Index of Layer containing ShapeFile</para>
    ''' </param>
    ''' <exception cref="Exception.html#LayerNotPolygonShapeFile" caption="LayerNotPolygonShapeFile">Layer specified by aLayerIndex is not a polygon ShapeFile</exception>
    ''' <exception cref="Exception.html#MappingObjectNotSet" caption="MappingObjectNotSet">Mapping Object Not Set</exception>
    Private Shared ReadOnly Property PolygonShapeFileFromIndex(ByVal aLayerIndex As Integer) As MapWinGIS.Shapefile
        Get
            Dim lSf As MapWinGIS.Shapefile = ShapeFileFromIndex(aLayerIndex)
            If (lSf.ShapefileType = MapWinGIS.ShpfileType.SHP_POLYGON OrElse _
              lSf.ShapefileType = MapWinGIS.ShpfileType.SHP_POLYGONM OrElse _
              lSf.ShapefileType = MapWinGIS.ShpfileType.SHP_POLYGONZ) Then
                Return lSf
            Else
                Throw New Exception("GisUtil:PolygonShapeFileFromIndex:Error:LayerIndex:" & aLayerIndex & ":Type:" & GetMappingObject.Layers(aLayerIndex).LayerType & ":IsNotPolygonShapeFile")
            End If
        End Get
    End Property

    ''' <summary>Obtain pointer to a grid from a LayerIndex</summary>
    ''' <param name="aLayerIndex">
    '''     <para>Index of desired layer containing grid (defaults to Current Layer)</para>
    ''' </param>
    ''' <exception cref="Exception.html#LayerNotGrid" caption="LayerNotGrid">Layer specified by aLayerIndex is not a Grid</exception>
    ''' <exception cref="Exception.html#LayerIndexOutOfRange" caption="LayerIndexOutOfRange">Layer specified by aLayerIndex does not exist</exception>
    ''' <exception cref="Exception.html#MappingObjectNotSet" caption="MappingObjectNotSet">Mapping Object Not Set</exception>
    Private Shared ReadOnly Property GridFromIndex(Optional ByVal aLayerIndex As Integer = UseCurrent) As MapWinGIS.Grid
        Get
            If aLayerIndex = UseCurrent Then
                aLayerIndex = CurrentLayer
            End If

            Dim lLayer As MapWindow.Interfaces.Layer = LayerFromIndex(aLayerIndex)
            If lLayer.LayerType = MapWindow.Interfaces.eLayerType.Grid Then
                Return lLayer.GetGridObject
            Else
                Throw New Exception("GisUtil:GridFromIndex:Error:LayerIndex:" & aLayerIndex & ":Type:" & GetMappingObject.Layers(aLayerIndex).LayerType & ":IsNotGrid")
            End If
        End Get
    End Property

    ''' <summary>Obtain pointer to a layer from a LayerIndex</summary>
    ''' <param name="aLayerIndex">
    '''     <para>Index of desired layer (defaults to Current Layer)</para>
    ''' </param>
    ''' <exception cref="Exception.html#LayerIndexOutOfRange" caption="LayerIndexOutOfRange">Layer specified by aLayerIndex does not exist</exception>
    ''' <exception cref="Exception.html#MappingObjectNotSet" caption="MappingObjectNotSet">Mapping Object Not Set</exception>
    Private Shared ReadOnly Property LayerFromIndex(Optional ByVal aLayerIndex As Integer = UseCurrent) As MapWindow.Interfaces.Layer
        Get
            If aLayerIndex = UseCurrent Then
                aLayerIndex = CurrentLayer
            End If

            If aLayerIndex >= 0 And aLayerIndex < GetMappingObject.Layers.NumLayers Then
                Return (GetMappingObject.Layers(aLayerIndex))
            Else
                Throw New Exception("GisUtil:LayerFromIndex:Error:LayerIndex:" & aLayerIndex & ":OutOfRange:0:" & GetMappingObject.Layers.NumLayers - 1)
            End If
        End Get
    End Property

    ''' <summary>Determine if polygons overlap</summary>
    ''' <param name="aLayerIndex1">
    '''     <para>Index of first Layer containing ShapeFile</para>
    ''' </param>
    ''' <param name="aFeatureIndex1">
    '''     <para>Index of first feature</para>
    ''' </param>
    ''' <param name="aLayerIndex2">
    '''     <para>Index of second Layer containing ShapeFile</para>
    ''' </param>
    ''' <param name="aFeatureIndex2">
    '''     <para>Index of second feature</para>
    ''' </param>
    ''' <exception cref="Exception.html#LayerNotPolygonShapeFile" caption="LayerNotPolygonShapeFile">Layer specified by aLayerIndex is not a polygon ShapeFile</exception>
    ''' <exception cref="Exception.html#FeatureIndexOutOfRange" caption="FeatureIndexOutOfRange">Feature Index Out of Range</exception>
    ''' <exception cref="Exception.html#MappingObjectNotSet" caption="MappingObjectNotSet">Mapping Object Not Set</exception>
    Public Shared Function OverlappingPolygons( _
      ByVal aLayerIndex1 As Integer, ByVal aFeatureIndex1 As Integer, _
      ByVal aLayerIndex2 As Integer, ByVal aFeatureIndex2 As Integer) As Boolean

        OverlappingPolygons = False

        Dim lSf1 As MapWinGIS.Shapefile = PolygonShapeFileFromIndex(aLayerIndex1)
        Dim lSf1Shape As MapWinGIS.Shape
        If FeatureIndexValid(aFeatureIndex1, lSf1) Then
            lSf1Shape = lSf1.Shape(aFeatureIndex1)

            Dim lSf2 As MapWinGIS.Shapefile = PolygonShapeFileFromIndex(aLayerIndex2)
            Dim lSf2Shape As MapWinGIS.Shape
            If FeatureIndexValid(aFeatureIndex2, lSf2) Then
                lSf2Shape = lSf2.Shape(aFeatureIndex2)

                Dim lIndex As Integer
                Dim lX As Double, lY As Double

                If Not (lSf1.Extents.xMin > lSf2.Extents.xMax OrElse _
                        lSf1.Extents.xMax < lSf2.Extents.xMin OrElse _
                        lSf1.Extents.yMin > lSf2.Extents.yMax OrElse _
                        lSf1.Extents.yMax < lSf2.Extents.yMin) Then
                    'might be within, check in detail
                    For lIndex = 1 To lSf1Shape.numPoints
                        lX = lSf1Shape.Point(lIndex - 1).x
                        lY = lSf1Shape.Point(lIndex - 1).y
                        OverlappingPolygons = lSf2.PointInShape(aFeatureIndex2, lX, lY)
                        If OverlappingPolygons Then 'quit loop, we've found they overlap
                            Exit For
                        End If
                    Next lIndex

                    If Not OverlappingPolygons Then 'now check the opposite
                        For lIndex = 1 To lSf2Shape.numPoints
                            lX = lSf2Shape.Point(lIndex - 1).x
                            lY = lSf2Shape.Point(lIndex - 1).y
                            OverlappingPolygons = lSf1.PointInShape(aFeatureIndex1, lX, lY)
                            If OverlappingPolygons Then 'quit loop, we've found they overlap
                                Exit For
                            End If
                        Next lIndex
                    End If
                End If
            End If
        End If
    End Function

    ''' <summary>Given a line and a polygon, determine if any part of the line is in the polygon</summary>
    ''' <param name="aLineLayerIndex">
    '''     <para>Index of line Layer</para>
    ''' </param>
    ''' <param name="aLineIndex">
    '''     <para>Index of line Feature</para>
    ''' </param>
    ''' <param name="aPolygonLayerIndex">
    '''     <para>Index of second Layer containing ShapeFile</para>
    ''' </param>
    ''' <param name="aPolygonIndex">
    '''     <para>Index of polygon Feature</para>
    ''' </param>
    ''' <exception cref="Exception.html#LayerNotPolygonShapeFile" caption="LayerNotPolygonShapeFile">Layer specified by aLayerIndex is not a polygon ShapeFile</exception>
    ''' <exception cref="Exception.html#FeatureIndexOutOfRange" caption="FeatureIndexOutOfRange">Feature Index Out of Range</exception>
    ''' <exception cref="Exception.html#MappingObjectNotSet" caption="MappingObjectNotSet">Mapping Object Not Set</exception>
    Public Shared Function LineInPolygon(ByVal aLineLayerIndex As Integer, _
                                         ByVal aLineIndex As Integer, _
                                         ByVal aPolygonLayerIndex As Integer, _
                                         ByVal aPolygonIndex As Integer) As Integer
        Dim lLineSf As MapWinGIS.Shapefile = ShapeFileFromIndex(aLineLayerIndex)
        Dim lLineShape As MapWinGIS.Shape = lLineSf.Shape(aLineIndex - 1)
        Dim lPolygonSf As MapWinGIS.Shapefile = PolygonShapeFileFromIndex(aPolygonLayerIndex)
        Dim lX As Double, lY As Double

        LineInPolygon = False
        For i As Integer = 1 To lLineShape.numPoints
            lX = lLineShape.Point(i - 1).x
            lY = lLineShape.Point(i - 1).y
            LineInPolygon = lPolygonSf.PointInShape(aPolygonIndex, lX, lY)
            If LineInPolygon Then 'point of line in polygon, true result
                Exit For
            End If
        Next i
    End Function

    ''' <summary>given a polygon layer (like dem shape) and a containing layer (like subbasins), return which polygon in the containing layer each polygon lies within, determine if polygons overlap</summary>
    ''' <param name="aLayerIndex">
    '''     <para>Index of Layer containing polygon ShapeFile</para>
    ''' </param>
    ''' <param name="aLayerIndexContaining">
    '''     <para>Index of Layer containing polygon ShapeFile</para>
    ''' </param>
    ''' <param name="aIndex">
    '''     <para>Index of polygons from contained</para>
    ''' </param>
    ''' <exception cref="Exception.html#LayerNotPolygonShapeFile" caption="LayerNotPolygonShapeFile">Layer specified by aLayerIndex is not a polygon ShapeFile</exception>
    ''' <exception cref="Exception.html#MappingObjectNotSet" caption="MappingObjectNotSet">Mapping Object Not Set</exception>
    Public Shared Sub AssignContainingPolygons( _
                                               ByVal aLayerIndex As Integer, _
                                               ByVal aLayerIndexContaining As Integer, _
                                               ByRef aIndex() As Integer)
        Dim lSf1 As MapWinGIS.Shapefile = ShapeFileFromIndex(aLayerIndex)
        Dim lSf2 As MapWinGIS.Shapefile = PolygonShapeFileFromIndex(aLayerIndexContaining)

        Dim lPointIndex As Integer
        Dim lShapeIndex As Integer
        Dim lX As Double, lY As Double
        Dim lSf1Shape As New MapWinGIS.Shape
        Dim lNth As Integer

        ReDim aIndex(lSf1.NumShapes)

        lSf2.BeginPointInShapefile()
        For lShapeIndex = 1 To lSf1.NumShapes
            aIndex(lShapeIndex) = -1
            lSf1Shape = lSf1.Shape(lShapeIndex - 1)
            For lPointIndex = 2 To lSf1Shape.numPoints
                lX = lSf1Shape.Point(lPointIndex - 1).x
                lY = lSf1Shape.Point(lPointIndex - 1).y
                lNth = lSf2.PointInShapefile(lX, lY)
                If lNth > -1 Then 'add to index aray
                    aIndex(lShapeIndex) = lNth
                    Exit For
                End If
            Next lPointIndex
        Next lShapeIndex
        lSf2.EndPointInShapefile()
    End Sub

    ''' <summary>Obtain number of fields in a shape file from a LayerIndex</summary>
    ''' <param name="aLayerIndex">
    '''     <para>Index of Layer containing ShapeFile</para>
    ''' </param>
    ''' <exception cref="Exception.html#LayerNotShapeFile" caption="LayerNotShapeFile">Layer specified by aLayerIndex is not a ShapeFile</exception>
    ''' <exception cref="Exception.html#LayerIndexOutOfRange" caption="LayerIndexOutOfRange">Layer specified by aLayerIndex does not exist</exception>
    ''' <exception cref="Exception.html#MappingObjectNotSet" caption="MappingObjectNotSet">Mapping Object Not Set</exception>
    Public Shared ReadOnly Property NumFields(Optional ByVal aLayerIndex As Integer = UseCurrent) As Integer
        Get
            Return ShapeFileFromIndex(aLayerIndex).NumFields
        End Get
    End Property

    ''' <summary>Obtain name of a field in a shape file from a layer index and a field index</summary>
    ''' <param name="aFieldIndex">
    '''     <para>Index of field to obtain name for</para>
    ''' </param>
    ''' <param name="aSf">
    '''     <para>ShapeFile containing filed</para>
    ''' </param>
    ''' <exception cref="Exception.html#FieldIndexOutOfRange" caption="FieldIndexOutOfRange">Field specified by aFieldIndex does not exist</exception>
    ''' <exception cref="Exception.html#MappingObjectNotSet" caption="MappingObjectNotSet">Mapping Object Not Set</exception>
    Private Shared ReadOnly Property FieldIndexValid(ByVal aFieldIndex As Integer, ByVal aSf As MapWinGIS.Shapefile) As Boolean
        Get
            If aFieldIndex < aSf.NumFields AndAlso aFieldIndex >= 0 Then
                Return True
            Else
                Throw New Exception("GisUtil:FieldName:Error:FieldIndex:" & aFieldIndex & ":OutOfRange:0:" & aSf.NumFields - 1)
            End If
        End Get
    End Property

    ''' <summary>Obtain name of a field in a shape file from a layer index and a field index</summary>
    ''' <param name="aFieldIndex">
    '''     <para>Index of field to obtain name for</para>
    ''' </param>
    ''' <param name="aLayerIndex">
    '''     <para>Index of layer containing shape file</para>
    ''' </param>
    ''' <exception cref="Exception.html#LayerNotShapeFile" caption="LayerNotShapeFile">Layer specified by aLayerIndex is not a ShapeFile</exception>
    ''' <exception cref="Exception.html#LayerIndexOutOfRange" caption="LayerIndexOutOfRange">Layer specified by aLayerIndex does not exist</exception>
    ''' <exception cref="Exception.html#FieldIndexOutOfRange" caption="FieldIndexOutOfRange">Field specified by aFieldIndex does not exist</exception>
    ''' <exception cref="Exception.html#MappingObjectNotSet" caption="MappingObjectNotSet">Mapping Object Not Set</exception>
    Public Shared ReadOnly Property FieldName(ByVal aFieldIndex As Integer, ByVal aLayerIndex As Integer) As String
        Get
            Dim lSf As MapWinGIS.Shapefile = ShapeFileFromIndex(aLayerIndex)
            If FieldIndexValid(aFieldIndex, lSf) Then
                Return lSf.Field(aFieldIndex).Name
            Else
                Throw New Exception("GisUtil:FieldName:Error:FieldIndex:" & aFieldIndex & ":OutOfRange:0:" & lSf.NumFields - 1)
            End If
        End Get
    End Property

    ''' <summary>Obtain index of a field in a shape file from a layer index and a field name</summary>
    ''' <param name="aFieldName">
    '''     <para>Name of field to obtain index for</para>
    ''' </param>
    ''' <param name="aLayerIndex">
    '''     <para>Index of layer containing shape file</para>
    ''' </param>
    ''' <exception cref="Exception.html#LayerNotShapeFile" caption="LayerNotShapeFile">Layer specified by aLayerIndex is not a ShapeFile</exception>
    ''' <exception cref="Exception.html#LayerIndexOutOfRange" caption="LayerIndexOutOfRange">Layer specified by aLayerIndex does not exist</exception>
    ''' <exception cref="Exception.html#FieldNameNotRecognized" caption="FieldNameNotRecognized">Field specified by aFieldName is not recognized</exception>
    ''' <exception cref="Exception.html#MappingObjectNotSet" caption="MappingObjectNotSet">Mapping Object Not Set</exception>
    Public Shared Function FieldIndex(ByVal aLayerIndex As Integer, ByVal aFieldName As String) As Integer
        Dim lSf As MapWinGIS.Shapefile = ShapeFileFromIndex(aLayerIndex)

        For iFieldIndex As Integer = 0 To lSf.NumFields - 1
            If UCase(lSf.Field(iFieldIndex).Name) = UCase(aFieldName) Then 'this is the field we want
                Return iFieldIndex
            End If
        Next
        Throw New Exception("GisUtil:FieldIndex:Error:FieldName:" & aFieldName & ":IsNotRecognized")
    End Function

    Public Shared Function IsField(ByVal aLayerIndex As Integer, _
                                   ByVal aFieldName As String) As Boolean
        Dim lSf As MapWinGIS.Shapefile = ShapeFileFromIndex(aLayerIndex)
        For lFieldIndex As Integer = 0 To lSf.NumFields - 1
            If UCase(lSf.Field(lFieldIndex).Name) = UCase(aFieldName) Then 'this is the field we want
                Return True
            End If
        Next
        Return False 'field name not found
    End Function

    ''' <summary>Add a field in a shape file from a layer index and a field name</summary>
    ''' <param name="aLayerIndex">
    '''     <para>Index of layer containing shape file</para>
    ''' </param>
    ''' <param name="aFieldName">
    '''     <para>Name of field to add</para>
    ''' </param>
    ''' <exception cref="Exception.html#LayerNotShapeFile" caption="LayerNotShapeFile">Layer specified by aLayerIndex is not a ShapeFile</exception>
    ''' <exception cref="Exception.html#LayerIndexOutOfRange" caption="LayerIndexOutOfRange">Layer specified by aLayerIndex does not exist</exception>
    ''' <exception cref="Exception.html#MappingObjectNotSet" caption="MappingObjectNotSet">Mapping Object Not Set</exception>
    Public Shared Function AddField(ByVal aLayerIndex As Integer, ByVal aFieldName As String, _
                                    ByVal aFieldType As Integer, ByVal aFieldWidth As Integer) As Integer
        Dim lField As New MapWinGIS.Field
        lField.Name = aFieldName
        lField.Type = aFieldType
        lField.Width = aFieldWidth

        Dim lSf As MapWinGIS.Shapefile = ShapeFileFromIndex(aLayerIndex)

        lSf.StartEditingTable()
        'TODO: error handling
        Dim lBsuc As Boolean = lSf.EditInsertField(lField, lSf.NumFields)
        lSf.StopEditingTable()

        Return lSf.NumFields - 1
    End Function

    ''' <summary>Remove a field in a shape file from a layer index and a field name</summary>
    ''' <param name="aLayerIndex">
    '''     <para>Index of layer containing shape file</para>
    ''' </param>
    ''' <param name="aFieldIndex">
    '''     <para>Index of field to remove</para>
    ''' </param>
    ''' <exception cref="Exception.html#FieldIndexOutOfRange" caption="FieldIndexOutOfRange">Field specified by aFieldIndex does not exist</exception>
    ''' <exception cref="Exception.html#LayerNotShapeFile" caption="LayerNotShapeFile">Layer specified by aLayerIndex is not a ShapeFile</exception>
    ''' <exception cref="Exception.html#LayerIndexOutOfRange" caption="LayerIndexOutOfRange">Layer specified by aLayerIndex does not exist</exception>
    ''' <exception cref="Exception.html#MappingObjectNotSet" caption="MappingObjectNotSet">Mapping Object Not Set</exception>
    Public Shared Function RemoveField(ByVal aLayerIndex As Integer, ByVal aFieldIndex As Integer) As Boolean
        Dim lSf As MapWinGIS.Shapefile = ShapeFileFromIndex(aLayerIndex)

        If FieldIndexValid(aFieldIndex, lSf) Then
            lSf.StartEditingTable()
            'TODO: error handling
            RemoveField = lSf.EditDeleteField(aFieldIndex)
            lSf.StopEditingTable()
        Else
            RemoveField = False
        End If
    End Function

    ''' <summary>Returns the number of layers loaded in the MapWindow. Drawing layers are not counted. </summary>
    ''' <exception cref="Exception.html#MappingObjectNotSet" caption="MappingObjectNotSet">Mapping Object Not Set</exception>
    Public Shared ReadOnly Property NumLayers() As Integer
        Get
            Return GetMappingObject.Layers.NumLayers()
        End Get
    End Property

    ''' <summary>Layer type from a layer index</summary>
    ''' <param name="aLayerIndex">
    '''     <para>Index of layer. (Defaults to current layer)</para>
    ''' </param>
    ''' <exception cref="Exception.html#LayerIndexOutOfRange" caption="LayerIndexOutOfRange">Layer specified by aLayerIndex does not exist</exception>
    ''' <exception cref="Exception.html#MappingObjectNotSet" caption="MappingObjectNotSet">Mapping Object Not Set</exception>
    Public Shared ReadOnly Property LayerType(Optional ByVal aLayerIndex As Integer = UseCurrent) As Integer
        Get
            Return LayerFromIndex(aLayerIndex).LayerType
        End Get
    End Property

    ''' <summary>Layer name from a layer index</summary>
    ''' <param name="aLayerIndex">
    '''     <para>Index of layer (Defaults to current layer)</para>
    ''' </param>
    ''' <exception cref="Exception.html#LayerIndexOutOfRange" caption="LayerIndexOutOfRange">Layer specified by aLayerIndex does not exist</exception>
    ''' <exception cref="Exception.html#MappingObjectNotSet" caption="MappingObjectNotSet">Mapping Object Not Set</exception>
    Public Shared ReadOnly Property LayerName(Optional ByVal aLayerIndex As Integer = UseCurrent) As String
        Get
            Return LayerFromIndex(aLayerIndex).Name
        End Get
    End Property

    ''' <summary>Index of a layer from a name</summary>
    ''' <param name="aLayerName">
    '''     <para>Name of layer to obtain index for</para>
    ''' </param>
    ''' <exception cref="Exception.html#LayerNameNameNotRecognized" caption="LayerNameNameNotRecognized">Layer specified by aLayerName does not exist</exception>
    ''' <exception cref="Exception.html#LayerIndexOutOfRange" caption="LayerIndexOutOfRange">Layer specified by aLayerIndex does not exist</exception>
    ''' <exception cref="Exception.html#MappingObjectNotSet" caption="MappingObjectNotSet">Mapping Object Not Set</exception>
    Public Shared Function LayerIndex(ByVal aLayerName As String) As Integer
        LayerIndex = 0

        For Each llayer As MapWindow.Interfaces.Layer In MapLayers()
            If UCase(llayer.Name) = UCase(aLayerName) Then
                Return LayerIndex
            End If
            LayerIndex += 1
        Next

        Throw New Exception("GisUtil:LayerIndex:Error:LayerName:" & aLayerName & ":IsNotRecognized")
    End Function

    ''' <summary>Layer file name from a layer index</summary>
    ''' <param name="aLayerIndex">
    '''     <para>Index of layer (Defaults to current layer)</para>
    ''' </param>
    ''' <exception cref="Exception.html#LayerIndexOutOfRange" caption="LayerIndexOutOfRange">Layer specified by aLayerIndex does not exist</exception>
    ''' <exception cref="Exception.html#MappingObjectNotSet" caption="MappingObjectNotSet">Mapping Object Not Set</exception>
    Public Shared ReadOnly Property LayerFileName(Optional ByVal aLayerIndex As Integer = UseCurrent) As String
        Get
            Return LayerFromIndex(aLayerIndex).FileName
        End Get
    End Property

    ''' <summary>Number of features from a layer index</summary>
    ''' <param name="aLayerIndex">
    '''     <para>Index of layer (Defaults to current layer)</para>
    ''' </param>
    ''' <exception cref="Exception.html#LayerNotShapeFile" caption="LayerNotShapeFile">Layer specified by aLayerIndex is not a ShapeFile</exception>
    ''' <exception cref="Exception.html#LayerIndexOutOfRange" caption="LayerIndexOutOfRange">Layer specified by aLayerIndex does not exist</exception>
    ''' <exception cref="Exception.html#MappingObjectNotSet" caption="MappingObjectNotSet">Mapping Object Not Set</exception>
    Public Shared ReadOnly Property NumFeatures(Optional ByVal aLayerIndex As Integer = UseCurrent) As Integer
        Get
            Return ShapeFileFromIndex(aLayerIndex).NumShapes
        End Get
    End Property

    ''' <summary>Type of a field from a field index and a layer index</summary>
    ''' <param name="aFieldIndex">
    '''     <para>Index of field</para>
    ''' </param>
    ''' <param name="aLayerIndex">
    '''     <para>Index of layer (Defaults to current layer)</para>
    ''' </param>
    ''' <exception cref="Exception.html#FieldIndexOutOfRange" caption="FieldIndexOutOfRange">Field specified by aFieldIndex does not exist</exception>
    ''' <exception cref="Exception.html#LayerNotShapeFile" caption="LayerNotShapeFile">Layer specified by aLayerIndex is not a ShapeFile</exception>
    ''' <exception cref="Exception.html#LayerIndexOutOfRange" caption="LayerIndexOutOfRange">Layer specified by aLayerIndex does not exist</exception>
    ''' <exception cref="Exception.html#MappingObjectNotSet" caption="MappingObjectNotSet">Mapping Object Not Set</exception>
    Public Shared ReadOnly Property FieldType(ByVal aFieldIndex As Integer, Optional ByVal aLayerIndex As Integer = UseCurrent) As Integer
        Get
            Dim lSf As MapWinGIS.Shapefile = ShapeFileFromIndex(aLayerIndex)
            If aFieldIndex < 0 Or aFieldIndex >= lSf.NumFields Then
                Throw New Exception("GisUtil:FieldType:Error:FieldIndex:" & aFieldIndex & ":OutOfRange:0:" & lSf.NumFields - 1)
            Else
                Return lSf.Field(aFieldIndex).Type
            End If
        End Get
    End Property

    ''' <summary>Value of a field from a field index, a feature index and a layer index</summary>
    ''' <param name="aFieldIndex">
    '''     <para>Index of field</para>
    ''' </param>
    ''' <param name="aFeatureIndex">
    '''     <para>Index of feature</para>
    ''' </param>
    ''' <param name="aLayerIndex">
    '''     <para>Index of layer (Defaults to current layer)</para>
    ''' </param>
    ''' <exception cref="Exception.html#FeatureIndexOutOfRange" caption="FeatureIndexOutOfRange">Feature specified by aFeatureIndex does not exist</exception>
    ''' <exception cref="Exception.html#FieldIndexOutOfRange" caption="FieldIndexOutOfRange">Field specified by aFieldIndex does not exist</exception>
    ''' <exception cref="Exception.html#LayerNotShapeFile" caption="LayerNotShapeFile">Layer specified by aLayerIndex is not a ShapeFile</exception>
    ''' <exception cref="Exception.html#LayerIndexOutOfRange" caption="LayerIndexOutOfRange">Layer specified by aLayerIndex does not exist</exception>
    ''' <exception cref="Exception.html#MappingObjectNotSet" caption="MappingObjectNotSet">Mapping Object Not Set</exception>
    Public Shared Function FieldValue(ByVal aLayerIndex As Integer, ByVal aFeatureIndex As Integer, ByVal aFieldIndex As Integer) As String
        Dim lSf As MapWinGIS.Shapefile = ShapeFileFromIndex(aLayerIndex)
        If aFieldIndex < 0 Or aFieldIndex >= lSf.NumFields Then
            Throw New Exception("GisUtil:FieldValue:Error:FieldIndex:" & aFieldIndex & ":OutOfRange:0:" & lSf.NumFields - 1)
        ElseIf Not FeatureIndexValid(aFeatureIndex, lSf) Then
            Throw New Exception("GisUtil:FieldValue:Error:FeatureIndex:" & aFeatureIndex & ":OutOfRange")
        Else
            Dim lCellValue As Object = lSf.CellValue(aFieldIndex, aFeatureIndex)
            If lCellValue.GetType.Name = "DBNull" Then
                Return Nothing
            Else
                Return lCellValue
            End If
        End If
    End Function

    ''' <summary>Minimum value in a grid from a LayerIndex</summary>
    ''' <param name="aLayerIndex">
    '''     <para>Index of desired layer containing grid (defaults to Current Layer)</para>
    ''' </param>
    ''' <exception cref="Exception.html#LayerNotGrid" caption="LayerNotGrid">Layer specified by aLayerIndex is not a Grid</exception>
    ''' <exception cref="Exception.html#LayerIndexOutOfRange" caption="LayerIndexOutOfRange">Layer specified by aLayerIndex does not exist</exception>
    ''' <exception cref="Exception.html#MappingObjectNotSet" caption="MappingObjectNotSet">Mapping Object Not Set</exception>
    Public Shared ReadOnly Property GridLayerMinimum(Optional ByVal aLayerIndex As Integer = UseCurrent) As Object
        Get
            Return GridFromIndex(aLayerIndex).Minimum
        End Get
    End Property

    ''' <summary>Maximum value in a grid from a LayerIndex</summary>
    ''' <param name="aLayerIndex">
    '''     <para>Index of desired layer containing grid (defaults to Current Layer)</para>
    ''' </param>
    ''' <exception cref="Exception.html#LayerNotGrid" caption="LayerNotGrid">Layer specified by aLayerIndex is not a Grid</exception>
    ''' <exception cref="Exception.html#LayerIndexOutOfRange" caption="LayerIndexOutOfRange">Layer specified by aLayerIndex does not exist</exception>
    ''' <exception cref="Exception.html#MappingObjectNotSet" caption="MappingObjectNotSet">Mapping Object Not Set</exception>
    Public Shared ReadOnly Property GridLayerMaximum(Optional ByVal aLayerIndex As Integer = UseCurrent) As Object
        Get
            Return GridFromIndex(aLayerIndex).Maximum
        End Get
    End Property

    ''' <summary>Area of a polygon from a LayerIndex and a FeatureIndex</summary>
    ''' <param name="aLayerIndex">
    '''     <para>Index of desired layer containing grid (defaults to Current Layer)</para>
    ''' </param>
    ''' <exception cref="Exception.html#LayerNotPolygonShapeFile" caption="LayerNotPolygonShapeFile">Layer specified by aLayerIndex is not a polygon ShapeFile</exception>
    ''' <exception cref="Exception.html#FeatureIndexOutOfRange" caption="FeatureIndexOutOfRange">Feature Index Out of Range</exception>
    ''' <exception cref="Exception.html#MappingObjectNotSet" caption="MappingObjectNotSet">Mapping Object Not Set</exception>
    Public Shared Function FeatureArea(ByVal aLayerIndex As Integer, ByVal aFeatureIndex As Integer) As Double
        Dim lArea As Double

        Dim lSf As MapWinGIS.Shapefile = PolygonShapeFileFromIndex(aLayerIndex)
        If FeatureIndexValid(aFeatureIndex, lSf) Then
            Try
                lArea = MapWinX.Utils.Area(lSf.Shape(aFeatureIndex))
                If lArea < 0.000001 Then 'TODO: try to calculate?
                    lArea = Double.NaN
                End If
            Catch ex As Exception
                Logger.Dbg("GisUtil:FeatureArea:Exception:" & ex.Message)
                lArea = Double.NaN
            End Try
        End If
        Return lArea

    End Function

    Public Shared Function FeatureLength(ByVal aLayerIndex As Integer, ByVal aFeatureIndex As Integer) As Double
        Dim lLength As Double

        Try
            Dim lSf As MapWinGIS.Shapefile = ShapeFileFromIndex(aLayerIndex)
            If FeatureIndexValid(aFeatureIndex, lSf) Then
                Dim lUtils As New MapWinGIS.Utils
                lLength = lUtils.Length(lSf.Shape(aFeatureIndex))
                If lLength < 0.000001 Then 'could it be undefined
                    Dim lShapeFileType As MapWinGIS.ShpfileType = lSf.ShapefileType
                    If lShapeFileType <> MapWinGIS.ShpfileType.SHP_POLYLINE AndAlso _
                       lShapeFileType <> MapWinGIS.ShpfileType.SHP_POLYLINEM AndAlso _
                       lShapeFileType <> MapWinGIS.ShpfileType.SHP_POLYLINEZ Then
                        Logger.Dbg("GisUtil:FeatureLength:LengthUndefinedForShapeType:" & lSf.ShapefileType)
                        lLength = Double.NaN
                    End If
                End If
            End If
        Catch ex As Exception
            Logger.Dbg("GisUtil:FeatureLength:Exception:" & ex.Message)
            lLength = Double.NaN
        End Try
        Return lLength
    End Function

    ''' <summary>Determine end points of a shape</summary>
    ''' <param name="aLayerIndex">
    '''     <para>Index of layer containing ShapeFile</para>
    ''' </param>
    ''' <param name="aFeatureIndex">
    '''     <para>Index of feature</para>
    ''' </param>
    ''' <exception cref="Exception.html#LayerNotShapeFile" caption="LayerNotShapeFile">Layer specified by aLayerIndex is not a ShapeFile</exception>
    ''' <exception cref="Exception.html#FeatureIndexOutOfRange" caption="FeatureIndexOutOfRange">Feature Index Out of Range</exception>
    ''' <exception cref="Exception.html#MappingObjectNotSet" caption="MappingObjectNotSet">Mapping Object Not Set</exception>
    Public Shared Sub EndPointsOfLine(ByVal aLayerIndex As Integer, ByVal aFeatureIndex As Integer, _
                                      ByRef aX1 As Double, ByRef aY1 As Double, ByRef aX2 As Double, ByRef aY2 As Double)
        Dim lsf As MapWinGIS.Shapefile = ShapeFileFromIndex(aLayerIndex)
        If FeatureIndexValid(aFeatureIndex, lsf) Then
            Dim lShape As MapWinGIS.Shape = lsf.Shape(aFeatureIndex)
            aX1 = lShape.Point(0).x
            aY1 = lShape.Point(0).y
            aX2 = lShape.Point(lShape.numPoints - 1).x
            aY2 = lShape.Point(lShape.numPoints - 1).y
        End If
    End Sub

    ''' <summary>Determine first point of a shape</summary>
    ''' <param name="aLayerIndex">
    '''     <para>Index of layer containing ShapeFile</para>
    ''' </param>
    ''' <param name="aFeatureIndex">
    '''     <para>Index of feature</para>
    ''' </param>
    ''' <exception cref="Exception.html#LayerNotShapeFile" caption="LayerNotShapeFile">Layer specified by aLayerIndex is not a ShapeFile</exception>
    ''' <exception cref="Exception.html#FeatureIndexOutOfRange" caption="FeatureIndexOutOfRange">Feature Index Out of Range</exception>
    ''' <exception cref="Exception.html#MappingObjectNotSet" caption="MappingObjectNotSet">Mapping Object Not Set</exception>
    Public Shared Sub PointXY(ByVal aLayerIndex As Integer, ByVal aFeatureIndex As Integer, _
                              ByRef aX As Double, ByRef aY As Double)
        Dim lSf As MapWinGIS.Shapefile = ShapeFileFromIndex(aLayerIndex)
        If FeatureIndexValid(aFeatureIndex, lSf) Then
            Dim lShape As MapWinGIS.Shape = lSf.Shape(aFeatureIndex)
            aX = lShape.Point(0).x
            aY = lShape.Point(0).y
        End If
    End Sub

    ''' <summary>Remove feature from a ShapeFile</summary>
    ''' <param name="aLayerIndex">
    '''     <para>Index of layer containing ShapeFile</para>
    ''' </param>
    ''' <param name="aFeatureIndex">
    '''     <para>Index of feature to be removed</para>
    ''' </param>
    ''' <exception cref="Exception.html#LayerNotShapeFile" caption="LayerNotShapeFile">Layer specified by aLayerIndex is not a ShapeFile</exception>
    ''' <exception cref="Exception.html#FeatureIndexOutOfRange" caption="FeatureIndexOutOfRange">Feature Index Out of Range</exception>
    ''' <exception cref="Exception.html#MappingObjectNotSet" caption="MappingObjectNotSet">Mapping Object Not Set</exception>
    Public Shared Function RemoveFeature(ByVal aLayerIndex As Integer, ByVal aFeatureIndex As Integer) As Double
        Dim lSf As MapWinGIS.Shapefile = ShapeFileFromIndex(aLayerIndex)

        If FeatureIndexValid(aFeatureIndex, lSf) Then
            lSf.StartEditingShapes(True)
            Dim lRetc As Boolean = lSf.EditDeleteShape(aFeatureIndex)
            lSf.StopEditingShapes(True, True)
            'TODO: what is the return value
        End If
    End Function

    ''' <summary>Set value of a field in a feature in a ShapeFile</summary>
    ''' <param name="aLayerIndex">
    '''     <para>Index of layer containing ShapeFile</para>
    ''' </param>
    ''' <param name="aFieldIndex">
    '''     <para>Index of field to change</para>
    ''' </param>
    ''' <param name="aFeatureIndex">
    '''     <para>Index of feature containing field to change</para> 
    ''' </param>
    ''' <param name="aValue">
    '''     <para>New value for field</para> 
    ''' </param>
    ''' <exception cref="Exception.html#LayerNotShapeFile" caption="LayerNotShapeFile">Layer specified by aLayerIndex is not a ShapeFile</exception>
    ''' <exception cref="Exception.html#FeatureIndexOutOfRange" caption="FeatureIndexOutOfRange">Feature Index Out of Range</exception>
    ''' <exception cref="Exception.html#FieldIndexOutOfRange" caption="FieldIndexOutOfRange">Field specified by aFieldIndex does not exist</exception>
    ''' <exception cref="Exception.html#MappingObjectNotSet" caption="MappingObjectNotSet">Mapping Object Not Set</exception>
    Public Shared Sub SetFeatureValue(ByVal aLayerIndex As Integer, ByVal aFieldIndex As Integer, ByVal aFeatureIndex As Integer, ByVal aValue As Object)
        Dim lSf As MapWinGIS.Shapefile = ShapeFileFromIndex(aLayerIndex)
        If FeatureIndexValid(aFeatureIndex, lSf) Then
            If FieldIndexValid(aFieldIndex, lSf) Then
                Dim lRetc As Boolean
                'TODO: error checks
                lRetc = lSf.StartEditingTable()
                lRetc = lSf.EditCellValue(aFieldIndex, aFeatureIndex, aValue)
                lRetc = lSf.StopEditingTable()
            End If
        End If
    End Sub

    Public Shared Function NumSelectedFeatures(ByVal aLayerIndex As Integer) As Integer
        Dim lLayer As MapWindow.Interfaces.Layer
        Dim lSi As MapWindow.Interfaces.SelectInfo

        lSi = pMapWin.View.SelectedShapes
        lLayer = LayerFromIndex(aLayerIndex) 'TODO: what does this do?
        If lSi.LayerHandle = aLayerIndex Then
            NumSelectedFeatures = lSi.NumSelected()
        Else
            NumSelectedFeatures = 0
        End If
    End Function

    Public Shared Function SetSelectedFeature(ByVal aLayerIndex As Integer, ByVal aShapeIndex As Integer) As Boolean
        Dim lSelectColor As System.Drawing.Color

        pMapWin.Layers.CurrentLayer = aLayerIndex
        lSelectColor = pMapWin.View.SelectColor
        pMapWin.View.SelectedShapes.AddByIndex(aShapeIndex, lSelectColor)
        Return True
    End Function

    Public Shared Function IndexOfNthSelectedFeatureInLayer(ByVal nth As Integer, ByVal aLayerIndex As Integer) As Integer
        Dim lsi As MapWindow.Interfaces.SelectInfo = GetMappingObject.View.SelectedShapes
        Dim lLayer As MapWindow.Interfaces.Layer = LayerFromIndex(aLayerIndex)
        If lsi.LayerHandle = aLayerIndex Then
            IndexOfNthSelectedFeatureInLayer = lsi.Item(nth).ShapeIndex
        End If
    End Function

    Public Shared Sub SaveSelectedFeatures(ByVal aInputLayerName As String, ByVal aOutputLayerName As String)
        'save the selected features of a layer as a new shapefile
        Dim i As Integer
        Dim j As Integer
        Dim k As Integer
        Dim lInputLayerIndex As Integer
        Dim lRetc As Boolean

        lInputLayerIndex = LayerIndex(aInputLayerName)
        Dim lsf As MapWinGIS.Shapefile = ShapeFileFromIndex(lInputLayerIndex)

        Dim osf As New MapWinGIS.Shapefile
        'create new shapefile
        lRetc = osf.CreateNew(aOutputLayerName, lsf.ShapefileType)
        For i = 1 To lsf.NumFields
            Dim [of] As New MapWinGIS.Field
            [of].Name = lsf.Field(i - 1).Name
            [of].Type = lsf.Field(i - 1).Type
            [of].Width = lsf.Field(i - 1).Width
            [of].Precision = lsf.Field(i - 1).Precision
            lRetc = osf.EditInsertField([of], osf.NumFields)
            [of] = Nothing
        Next i
        osf.StartEditingShapes(True)

        Dim lsi As MapWindow.Interfaces.SelectInfo
        lsi = pMapWin.View.SelectedShapes
        If lsi.LayerHandle = lInputLayerIndex Then
            For i = 1 To lsf.NumShapes
                'loop through each shape of the layer
                For j = 1 To lsi.NumSelected
                    'loop through selected shapes
                    If i - 1 = lsi.Item(j - 1).ShapeIndex Then
                        osf.EditInsertShape(lsf.Shape(i - 1), osf.NumShapes)
                        For k = 1 To osf.NumFields
                            lRetc = osf.EditCellValue(k - 1, osf.NumShapes - 1, lsf.CellValue(k - 1, i - 1))
                        Next k
                    End If
                Next j
            Next i
        End If

        lRetc = osf.SaveAs(aOutputLayerName)
        osf.StopEditingShapes()
        osf.Close()

        AddLayer(aOutputLayerName, aOutputLayerName)

    End Sub

    Public Shared Sub ExtentsOfLayer(ByVal aLayerIndex As Integer, _
                                     ByVal aXMax As Double, ByVal aXMin As Double, ByVal aYMax As Double, ByVal aYMin As Double)
        Dim lsf As MapWinGIS.Shapefile = ShapeFileFromIndex(aLayerIndex)
        aXMax = lsf.Extents.xMax
        aXMin = lsf.Extents.xMin
        aYMax = lsf.Extents.yMax
        aYMin = lsf.Extents.yMin
    End Sub

    'Public Shared Function CreateNewShapefile(ByVal basename As String, ByVal type As Integer) As Boolean
    '  Dim osf As New MapWinGIS.Shapefile
    '  'MapWinGIS.ShpfileType.SHP_POLYGON = 5
    '  osf.CreateNew(basename, type)
    'End Function

    ''' <summary>Obtain file name of currently loaded project</summary>
    ''' <exception cref="Exception.html#MappingObjectNotSet" caption="MappingObjectNotSet">Mapping Object Not Set</exception>
    Public Shared ReadOnly Property ProjectFileName() As String
        Get
            Return GetMappingObject.Project.FileName
        End Get
    End Property

    Public Shared Function PointInPolygon(ByVal aPointLayerIndex As Integer, ByVal aPointIndex As Integer, ByVal aPolygonLayerIndex As Integer) As Integer
        'given a point and a polygon layer, return the polygon this point is in
        Dim lPointSf As MapWinGIS.Shapefile = ShapeFileFromIndex(aPointLayerIndex)
        Dim lPolygonSf As MapWinGIS.Shapefile = PolygonShapeFileFromIndex(aPolygonLayerIndex)
        Dim lX As Double, lY As Double

        lPolygonSf.BeginPointInShapefile()
        lX = lPointSf.Shape(aPointIndex - 1).Point(0).x
        lY = lPointSf.Shape(aPointIndex - 1).Point(0).y
        PointInPolygon = lPolygonSf.PointInShapefile(lX, lY)
        lPolygonSf.EndPointInShapefile()
    End Function

    Public Shared Function PointInPolygonXY(ByVal aX As Double, ByVal aY As Double, ByVal aPolygonLayerIndex As Integer) As Integer
        'given a point xy and a polygon layer, return the polygon this point is in
        Dim lPolygonSf As MapWinGIS.Shapefile = PolygonShapeFileFromIndex(aPolygonLayerIndex)

        lPolygonSf.BeginPointInShapefile()
        PointInPolygonXY = lPolygonSf.PointInShapefile(aX, aY)
        lPolygonSf.EndPointInShapefile()
    End Function

    Public Shared Function AddLayer(ByVal aFileName As String, _
                                    ByVal aLayerName As String) As Boolean
        'given a shape file name, add it to the map.
        'return true if the layer is already there or successfully added.
        If IsLayer(aLayerName) Then  'already on map 
            AddLayer = True
        Else
            Dim lLayer As MapWindow.Interfaces.Layer = GetMappingObject.Layers.Add(aFileName, aLayerName)
            If lLayer Is Nothing Then
                AddLayer = False
            Else
                lLayer.Visible = False
                AddLayer = True
            End If
        End If
    End Function

    ''' <summary>Layer part of project?</summary>
    ''' <param name="aLayerName">
    '''     <para>Name of layer to compare to layeres in project</para>
    ''' </param>
    Public Shared Function IsLayer(ByVal aLayerName As String) As Boolean
        For i As Integer = 1 To GetMappingObject.Layers.NumLayers
            If aLayerName = GetMappingObject.Layers(i).Name Then
                Return True
            End If
        Next i
        Return False
    End Function

    ''' <summary>Layer visible flag</summary>
    ''' <param name="aLayerName">
    '''     <para>Name of layer</para>
    ''' </param>
    ''' <exception cref="Exception.html#LayerNameNameNotRecognized" caption="LayerNameNameNotRecognized">Layer specified by aLayerName does not exist</exception>
    ''' <exception cref="Exception.html#MappingObjectNotSet" caption="MappingObjectNotSet">Mapping Object Not Set</exception>
    Public Shared Property LayerVisible(ByVal aLayerName As String) As Boolean
        Get
            Return LayerFromIndex(LayerIndex(aLayerName)).Visible
        End Get
        Set(ByVal aVisible As Boolean)
            LayerFromIndex(LayerIndex(aLayerName)).Visible = aVisible
        End Set
    End Property

    ''' <summary>Layer visible flag</summary>
    ''' <param name="aLayerIndex">
    '''     <para>Index of layer (Defaults to current layer)</para>
    ''' </param>
    ''' <exception cref="Exception.html#LayerIndexOutOfRange" caption="LayerIndexOutOfRange">Layer specified by aLayerIndex does not exist</exception>
    ''' <exception cref="Exception.html#MappingObjectNotSet" caption="MappingObjectNotSet">Mapping Object Not Set</exception>
    Public Shared Property LayerVisible(Optional ByVal aLayerIndex As Integer = UseCurrent) As Boolean
        Get
            If aLayerIndex = UseCurrent Then aLayerIndex = CurrentLayer
            Return LayerFromIndex(aLayerIndex).Visible
        End Get
        Set(ByVal aVisible As Boolean)
            If aLayerIndex = UseCurrent Then aLayerIndex = CurrentLayer
            LayerFromIndex(aLayerIndex).Visible = aVisible
        End Set
    End Property

    ''' <summary>Remove layer</summary>
    ''' <param name="aLayerIndex">
    '''     <para>Index of layer (Defaults to current layer)</para>
    ''' </param>
    ''' <exception cref="Exception.html#LayerIndexOutOfRange" caption="LayerIndexOutOfRange">Layer specified by aLayerIndex does not exist</exception>
    ''' <exception cref="Exception.html#MappingObjectNotSet" caption="MappingObjectNotSet">Mapping Object Not Set</exception>
    Public Shared Function RemoveLayer(Optional ByVal aLayerIndex As Integer = UseCurrent) As Boolean
        If aLayerIndex = UseCurrent Then aLayerIndex = CurrentLayer
        Dim lLayerName As String = LayerName(aLayerIndex) 'forces check of index
        GetMappingObject.Layers.Remove(aLayerIndex)
    End Function

    ''' <summary>Given a grid and a polygon layer, calculate the area of each grid category 
    '''          within each polygon.  Output array contains area of each grid category
    '''          and polygon combination.</summary>
    ''' <remark>This function can be accomplished in MapWindow by 
    '''         looping through each grid cell and counting the 
    '''         number of cells of each grid category within each 
    '''         feature. The MapWinGIS calls to use include the Grid 
    '''         Property Value, the GridHeader Properties dX, dY, XllCenter, 
    '''         and YllCenter, and the Shapefile Function PointInShapefile.</remark>
    ''' <param name="aGridLayerIndex">
    '''     <para>Index of grid layer containing values</para>
    ''' </param>
    ''' <param name="aPolygonLayerIndex">
    '''     <para>Index of polgon layer to calculate areas for</para>
    ''' </param>
    ''' <param name="aAreaGridPoly">
    '''     <para>2-D Array used to store output areas</para>
    ''' </param>
    Public Shared Sub TabulateAreas(ByVal aGridLayerIndex As Integer, _
                                    ByVal aPolygonLayerIndex As Integer, _
                                    ByRef aAreaGridPoly(,) As Double)
        'set input grid
        Dim lInputGrid As MapWinGIS.Grid = GridFromIndex(aGridLayerIndex)
        'set input polygon layer
        Dim lPolygonSf As MapWinGIS.Shapefile = PolygonShapeFileFromIndex(aPolygonLayerIndex)

        Dim lMinX As Double
        Dim lMaxX As Double
        Dim lMinY As Double
        Dim lMaxY As Double
        'figure out what part of the grid overlays these polygons
        If NumSelectedFeatures(aPolygonLayerIndex) = 0 Then
            'use whole extent of shapefile
            lMinX = lPolygonSf.Extents.xMin
            lMaxX = lPolygonSf.Extents.xMax
            lMinY = lPolygonSf.Extents.yMin
            lMaxY = lPolygonSf.Extents.yMax
        Else 'use only extent of selected features
            Dim lShapeIndex As Integer = IndexOfNthSelectedFeatureInLayer(0, aPolygonLayerIndex)
            lMinX = 1.0E+30
            lMaxX = -1.0E+30
            lMinY = 1.0E+30
            lMaxY = -1.0E+30
            For lSelectedIndex As Integer = 0 To NumSelectedFeatures(aPolygonLayerIndex) - 1
                Dim lPolyIndex As Integer = IndexOfNthSelectedFeatureInLayer(lSelectedIndex, aPolygonLayerIndex)
                If lPolygonSf.Shape(lPolyIndex).Extents.xMin < lMinX Then
                    lMinX = lPolygonSf.Shape(lPolyIndex).Extents.xMin
                End If
                If lPolygonSf.Shape(lPolyIndex).Extents.yMin < lMinY Then
                    lMinY = lPolygonSf.Shape(lPolyIndex).Extents.yMin
                End If
                If lPolygonSf.Shape(lPolyIndex).Extents.xMax > lMaxX Then
                    lMaxX = lPolygonSf.Shape(lPolyIndex).Extents.xMax
                End If
                If lPolygonSf.Shape(lPolyIndex).Extents.yMax > lMaxY Then
                    lMaxY = lPolygonSf.Shape(lPolyIndex).Extents.yMax
                End If
            Next
        End If

        Dim lStartingColumn As Integer
        Dim lEndingColumn As Integer
        Dim lStartingRow As Integer
        Dim lEndingRow As Integer
        lInputGrid.ProjToCell(lMinX, lMinY, lStartingColumn, lEndingRow)
        lInputGrid.ProjToCell(lMaxX, lMaxY, lEndingColumn, lStartingRow)

        Dim lCellArea As Double = lInputGrid.Header.dX * lInputGrid.Header.dY
        Dim lTotalCellCount As Integer = (lEndingColumn - lStartingColumn) * (lEndingRow - lStartingRow)
        Dim lCellCount As Integer = 0
        Dim lLastdisplayed As Integer = 0
        GetMappingObject.StatusBar.ShowProgressBar = True
        GetMappingObject.StatusBar.ProgressBarValue = 0

        lPolygonSf.BeginPointInShapefile()
        Dim lXPos As Double
        Dim lYPos As Double
        Dim lInsideId As Integer
        Dim lGridValue As Integer

        For lRow As Integer = lStartingRow To lEndingRow
            For lCol As Integer = lStartingColumn To lEndingColumn
                lInputGrid.CellToProj(lCol, lRow, lXPos, lYPos)
                lInsideId = lPolygonSf.PointInShapefile(lXPos, lYPos)
                If lInsideId > -1 Then 'this is in a subbasin
                    If lInputGrid.Value(lCol, lRow).GetType.Name = "SByte" Then
                        lGridValue = Convert.ToInt32(lInputGrid.Value(lCol, lRow))
                    Else
                        lGridValue = lInputGrid.Value(lCol, lRow)
                    End If
                    aAreaGridPoly(lGridValue, lInsideId) += lCellArea
                End If
                lCellCount += 1
                Dim lCurrentDisplay As Integer = Int(lCellCount / lTotalCellCount * 100)
                If lCurrentDisplay > lLastdisplayed Then
                    lLastdisplayed = lCurrentDisplay
                    GetMappingObject.StatusBar.ProgressBarValue = lCurrentDisplay
                End If
            Next lCol
        Next lRow

        GetMappingObject.StatusBar.ShowProgressBar = False
        lPolygonSf.EndPointInShapefile()

    End Sub

    Public Shared Sub GridMinMaxInPolygon(ByVal aGridLayerIndex As Integer, ByVal aPolygonLayerIndex As Integer, _
                                          ByVal aPolygonFeatureIndex As Integer, _
                                          ByRef aMin As Double, ByRef aMax As Double)
        'Given a grid and a polygon layer, find the min and max grid value within the feature.

        Dim lCol As Integer
        Dim lRow As Integer
        Dim lXPos As Double
        Dim lYPos As Double
        Dim lSubId As Integer
        Dim lStartCol As Integer
        Dim lEndCol As Integer
        Dim lStartRow As Integer
        Dim lEndRow As Integer
        Dim lVal As Integer
        Dim totalcellcount As Integer
        Dim cellcount As Integer
        Dim lastdisplayed As Integer

        'set input grid
        Dim lInputGrid As MapWinGIS.Grid = GridFromIndex(aGridLayerIndex)

        'set input polygon layer
        Dim lPolygonSf As MapWinGIS.Shapefile = PolygonShapeFileFromIndex(aPolygonLayerIndex)
        Dim lShape As New MapWinGIS.Shape
        If FeatureIndexValid(aPolygonFeatureIndex, lPolygonSf) Then
            lShape = lPolygonSf.Shape(aPolygonFeatureIndex)

            'figure out what part of the grid overlays this polygon
            lInputGrid.ProjToCell(lShape.Extents.xMin, lShape.Extents.yMin, lStartCol, lEndRow)
            lInputGrid.ProjToCell(lShape.Extents.xMax, lShape.Extents.yMax, lEndCol, lStartRow)

            totalcellcount = (lEndCol - lStartCol) * (lEndRow - lStartRow)
            cellcount = 0
            lastdisplayed = 0
            GetMappingObject.StatusBar.ShowProgressBar = True
            GetMappingObject.StatusBar.ProgressBarValue = 0

            aMin = 99999999
            aMax = -99999999
            lPolygonSf.BeginPointInShapefile()
            For lCol = lStartCol To lEndCol
                For lRow = lStartRow To lEndRow
                    lInputGrid.CellToProj(lCol, lRow, lXPos, lYPos)
                    lSubId = lPolygonSf.PointInShapefile(lXPos, lYPos)
                    If lSubId = aPolygonFeatureIndex Then 'this is in the polygon we want
                        lVal = lInputGrid.Value(lCol, lRow)
                        If lVal > aMax Then
                            aMax = lVal
                        End If
                        If lVal < aMin Then
                            aMin = lVal
                        End If
                    End If
                    cellcount = cellcount + 1
                    If Int(cellcount / totalcellcount * 100) > lastdisplayed Then
                        lastdisplayed = Int(cellcount / totalcellcount * 100)
                        GetMappingObject.StatusBar.ProgressBarValue = Int(cellcount / totalcellcount * 100)
                    End If
                Next lRow
            Next lCol
            GetMappingObject.StatusBar.ShowProgressBar = False
            lPolygonSf.EndPointInShapefile()
        End If
    End Sub

    Public Shared Function GridSlopeInPolygon(ByVal aGridLayerIndex As Integer, ByVal aPolygonLayerIndex As Integer, _
                                              ByVal aPolygonFeatureIndex As Integer) As Double
        'Given an elevation grid and a polygon layer, find the average slope value within the feature.

        Dim lCol As Integer
        Dim lRow As Integer
        Dim lXPos As Double
        Dim lYPos As Double
        Dim lSubId As Integer
        Dim lStartCol As Integer
        Dim lEndCol As Integer
        Dim lStartRow As Integer
        Dim lEndRow As Integer
        Dim lVal As Integer
        Dim totalcellcount As Integer
        Dim cellcount As Integer
        Dim lastdisplayed As Integer
        Dim lNeighborVal As Integer
        Dim lNeighborCount As Integer
        Dim lSum As Integer
        Dim lSubCellCount As Integer

        'set input grid
        Dim lInputGrid As MapWinGIS.Grid = GridFromIndex(aGridLayerIndex)

        'set input polygon layer
        Dim lPolygonSf As MapWinGIS.Shapefile = PolygonShapeFileFromIndex(aPolygonLayerIndex)
        Dim lShape As New MapWinGIS.Shape
        If FeatureIndexValid(aPolygonFeatureIndex, lPolygonSf) Then
            lShape = lPolygonSf.Shape(aPolygonFeatureIndex)

            'figure out what part of the grid overlays this polygon
            lInputGrid.ProjToCell(lShape.Extents.xMin, lShape.Extents.yMin, lStartCol, lEndRow)
            lInputGrid.ProjToCell(lShape.Extents.xMax, lShape.Extents.yMax, lEndCol, lStartRow)

            totalcellcount = (lEndCol - lStartCol) * (lEndRow - lStartRow)
            cellcount = 0
            lastdisplayed = 0
            GetMappingObject.StatusBar.ShowProgressBar = True
            GetMappingObject.StatusBar.ProgressBarValue = 0

            lPolygonSf.BeginPointInShapefile()
            lSum = 0
            lSubCellCount = 0
            For lCol = lStartCol To lEndCol
                For lRow = lStartRow To lEndRow
                    lInputGrid.CellToProj(lCol, lRow, lXPos, lYPos)
                    lSubId = lPolygonSf.PointInShapefile(lXPos, lYPos)
                    If lSubId = aPolygonFeatureIndex Then 'this is in the polygon we want
                        lVal = lInputGrid.Value(lCol, lRow)
                        lNeighborCount = 0
                        lNeighborVal = 0
                        'find neighboring elevations
                        If lCol > 1 Then
                            lNeighborVal = lNeighborVal + Math.Abs(lInputGrid.Value(lCol - 1, lRow) - lVal)
                            lNeighborCount = lNeighborCount + 1
                        End If
                        If lCol < lInputGrid.Header.NumberCols Then
                            lNeighborVal = lNeighborVal + Math.Abs(lInputGrid.Value(lCol + 1, lRow) - lVal)
                            lNeighborCount = lNeighborCount + 1
                        End If
                        If lRow > 1 Then
                            lNeighborVal = lNeighborVal + Math.Abs(lInputGrid.Value(lCol, lRow - 1) - lVal)
                            lNeighborCount = lNeighborCount + 1
                        End If
                        If lRow < lInputGrid.Header.NumberRows Then
                            lNeighborVal = lNeighborVal + Math.Abs(lInputGrid.Value(lCol, lRow + 1) - lVal)
                            lNeighborCount = lNeighborCount + 1
                        End If
                        'find average difference in neighboring elevation
                        If lNeighborCount > 0 Then
                            lNeighborVal = lNeighborVal / lNeighborCount
                        Else
                            lNeighborVal = 0
                        End If
                        'sum the elev diff for each cell in polygon
                        lSum = lSum + lNeighborVal
                        lSubCellCount = lSubCellCount + 1
                    End If
                    cellcount = cellcount + 1
                    If Int(cellcount / totalcellcount * 100) > lastdisplayed Then
                        lastdisplayed = Int(cellcount / totalcellcount * 100)
                        GetMappingObject.StatusBar.ProgressBarValue = Int(cellcount / totalcellcount * 100)
                    End If
                Next lRow
            Next lCol
            GridSlopeInPolygon = (lSum / lSubCellCount) / lInputGrid.Header.dX
            GetMappingObject.StatusBar.ShowProgressBar = False
            lPolygonSf.EndPointInShapefile()
        End If
    End Function

    Public Shared Function GridValueAtPoint(ByVal aGridLayerIndex As Integer, ByVal aX As Double, ByVal aY As Double) As Integer
        'set input grid
        Dim gridLayer As MapWindow.Interfaces.Layer
        gridLayer = GetMappingObject.Layers(aGridLayerIndex)
        Dim lInputGrid As New MapWinGIS.Grid
        lInputGrid = gridLayer.GetGridObject

        Dim lCol As Integer
        Dim lRow As Integer
        lInputGrid.ProjToCell(aX, aY, lCol, lRow)
        Return lInputGrid.Value(lCol, lRow)
    End Function

    ''' <summary>Overlay Layer1 and Layer2 (eg landuse and subbasins), creating a polygon layer containing features from both layers</summary>
    ''' <param name="aLayer1Name">
    '''     <para>Name of first layer to overlay</para>
    ''' </param>
    ''' <param name="aLayer1FieldName">
    '''     <para>Name of field in first layer to overlay</para>
    ''' </param>
    ''' <param name="aLayer2Name">
    '''     <para>Name of second layer to overlay</para>
    ''' </param>
    ''' <param name="aLayer2FieldName">
    '''     <para>Name of field in second layer to overlay</para>
    ''' </param>
    ''' <param name="aOutputLayerName">
    '''     <para>Name of output layer</para>
    ''' </param>
    ''' <param name="aCreateNew">
    '''     <para>Flag, true if a new output shape file is desired, false if append existing file is desired</para>
    ''' </param>
    Public Shared Sub Overlay(ByVal aLayer1Name As String, ByVal aLayer1FieldName As String, _
                              ByVal aLayer2Name As String, ByVal aLayer2FieldName As String, _
                              ByVal aOutputLayerName As String, ByVal aCreateNew As Boolean)
        Dim lLayer1Index As Integer = LayerIndex(aLayer1Name)
        Dim lLayer1FieldIndex As Integer = FieldIndex(lLayer1Index, aLayer1FieldName)
        Dim lLayer2Index As Integer = LayerIndex(aLayer2Name)
        Dim lLayer2FieldIndex As Integer = FieldIndex(lLayer2Index, aLayer2FieldName)

        Overlay(lLayer1Index, lLayer1FieldIndex, lLayer2Index, lLayer2FieldIndex, _
                aOutputLayerName, aCreateNew)

    End Sub

    ''' <summary>Overlay Layer1 and Layer2 (eg landuse and subbasins), creating a polygon layer containing features from both layers</summary>
    ''' <param name="aLayer1Index">
    '''     <para>Index of first layer to overlay</para>
    ''' </param>
    ''' <param name="aLayer1FieldIndex">
    '''     <para>Index of field in first layer to overlay</para>
    ''' </param>
    ''' <param name="aLayer2Index">
    '''     <para>Index of second layer to overlay</para>
    ''' </param>
    ''' <param name="aLayer2FieldIndex">
    '''     <para>Index of field in second layer to overlay</para>
    ''' </param>
    ''' <param name="aOutputLayerName">
    '''     <para>Name of output layer</para>
    ''' </param>
    ''' <param name="aCreateNew">
    '''     <para>Flag, true if a new output shape file is desired, false if append existing file is desired</para>
    ''' </param>
    Public Shared Sub Overlay(ByVal aLayer1Index As Integer, ByVal aLayer1FieldIndex As Integer, _
                              ByVal aLayer2Index As Integer, ByVal aLayer2FieldIndex As Integer, _
                              ByVal aOutputLayerName As String, ByVal aCreateNew As Boolean)

        'obtain handle to layer 1  
        Dim lLayer1 As MapWindow.Interfaces.Layer = GetMappingObject.Layers(aLayer1Index)
        Dim lSf1 As New MapWinGIS.Shapefile
        lSf1 = lLayer1.GetObject

        'set layer 2 (subbasins)
        Dim lLayer2 As MapWindow.Interfaces.Layer = GetMappingObject.Layers(aLayer2Index)
        Dim lSf2 As New MapWinGIS.Shapefile
        lSf2 = lLayer2.GetObject
        Dim lSf2Ext As MapWinGIS.Extents = lSf2.Extents

        'if any of layer 2 is selected, use only those
        Dim lLayer2Selected As New Collection
        For i As Integer = 1 To NumSelectedFeatures(aLayer2Index)
            lLayer2Selected.Add(IndexOfNthSelectedFeatureInLayer(i - 1, aLayer2Index))
        Next
        If lLayer2Selected.Count = 0 Then 'no subbasins selected, act as if all are selected
            For i As Integer = 1 To NumFeatures(aLayer2Index)
                lLayer2Selected.Add(i - 1)
            Next
        End If

        Dim lBsuc As Boolean
        Dim lSfOut As New MapWinGIS.Shapefile
        If aCreateNew Then 'create new output overlay shapefile
            lSfOut.CreateNew("overlay", MapWinGIS.ShpfileType.SHP_POLYGON)
            Dim [lOf] As New MapWinGIS.Field
            [lOf].Name = FieldName(aLayer1FieldIndex, aLayer1Index)
            [lOf].Type = MapWinGIS.FieldType.INTEGER_FIELD
            [lOf].Width = 10
            lBsuc = lSfOut.EditInsertField([lOf], 0)
            Dim lof2 As New MapWinGIS.Field
            lof2.Name = FieldName(aLayer2FieldIndex, aLayer2Index)
            lof2.Type = MapWinGIS.FieldType.STRING_FIELD
            lof2.Width = 10
            lBsuc = lSfOut.EditInsertField(lof2, 1)
            Dim lof3 As New MapWinGIS.Field
            lof3.Name = "Area"
            lof3.Type = MapWinGIS.FieldType.DOUBLE_FIELD
            lof3.Width = 10
            lof3.Precision = 3
            lBsuc = lSfOut.EditInsertField(lof3, 2)
        Else 'open existing output shape file
            lBsuc = lSfOut.Open(aOutputLayerName)
        End If

        lSfOut.StartEditingShapes(True)

        Dim lShapeNew As MapWinGIS.Shape
        Dim lShape1 As MapWinGIS.Shape
        Dim lShape2 As MapWinGIS.Shape
        Dim lShape2Ext As MapWinGIS.Extents

        'set up collections of subbasin shapes and extents to save computation time later
        Dim lSf2Shape As New Collection
        Dim lSf2ShapeExtXmax As New Collection
        Dim lSf2ShapeExtXmin As New Collection
        Dim lSf2ShapeExtYmax As New Collection
        Dim lSf2ShapeExtYmin As New Collection
        For k As Integer = 1 To lLayer2Selected.Count
            'loop thru each selected subbasin (or all if none selected)
            lShape2 = lSf2.Shape(lLayer2Selected(k))
            lShape2Ext = lShape2.Extents
            lSf2Shape.Add(lShape2)
            lSf2ShapeExtXmax.Add(lShape2Ext.xMax)
            lSf2ShapeExtXmin.Add(lShape2Ext.xMin)
            lSf2ShapeExtYmax.Add(lShape2Ext.yMax)
            lSf2ShapeExtYmin.Add(lShape2Ext.yMin)
        Next k

        '********** do overlay ***********
        GetMappingObject.StatusBar.ShowProgressBar = True
        GetMappingObject.StatusBar.ProgressBarValue = 0

        Dim lTotalPolygonCount As Integer = lSf1.NumShapes * lLayer2Selected.Count
        Dim lPolygonCount As Integer = 0
        Dim lLastDisplayed As Integer = 0
        Dim lNumShapes As Integer = lSf1.NumShapes
        For i As Integer = 1 To lNumShapes 'loop through each shape of the land use layer
            lShape1 = lSf1.Shape(i - 1)
            Dim lSf1Ext As MapWinGIS.Extents = lShape1.Extents
            If Not (lSf1Ext.xMin > lSf2Ext.xMax OrElse _
                    lSf1Ext.xMax < lSf2Ext.xMin OrElse _
                    lSf1Ext.yMin > lSf2Ext.yMax OrElse _
                    lSf1Ext.yMax < lSf2Ext.yMin) Then
                'current first polygon falls in the extents of the second shapefile
                For k As Integer = 1 To lLayer2Selected.Count
                    'loop thru each selected shape in second shapefile (or all if none selected)
                    Dim lShapeIndex As Integer = lLayer2Selected(k)
                    lPolygonCount = lPolygonCount + 1
                    If Not (lSf1Ext.xMin > lSf2ShapeExtXmax(k) OrElse _
                            lSf1Ext.xMax < lSf2ShapeExtXmin(k) OrElse _
                            lSf1Ext.yMin > lSf2ShapeExtYmax(k) OrElse _
                            lSf1Ext.yMax < lSf2ShapeExtYmin(k)) Then
                        'look for intersection from overlay of these shapes
                        lShapeNew = MapWinX.SpatialOperations.Intersection(lShape1, lSf2Shape(k))
                        If lShapeNew.numPoints > 0 Then 'Insert the shape into the shapefile 
                            lBsuc = lSfOut.EditInsertShape(lShapeNew, lSfOut.NumShapes)
                            If Not lBsuc Then
                                Logger.Dbg("Problem Adding Shape") 'TODO:add more details, message box?
                            End If
                            Dim lArea As Double = Math.Abs(MapWinX.Utils.Area(lShapeNew))
                            'keep track of field values from both shapefiles
                            Dim lFeature1Id As String = FieldValue(aLayer1Index, i - 1, aLayer1FieldIndex)
                            Dim lFeature2Id As String = FieldValue(aLayer2Index, lShapeIndex, aLayer2FieldIndex)
                            lBsuc = lSfOut.EditCellValue(0, lSfOut.NumShapes - 1, lFeature1Id)
                            lBsuc = lSfOut.EditCellValue(1, lSfOut.NumShapes - 1, lFeature2Id)
                            lBsuc = lSfOut.EditCellValue(2, lSfOut.NumShapes - 1, lArea)
                        End If
                        lShapeNew = Nothing
                    End If
                Next k
            Else 'quick processing of all polygons in layer2 because outside of extent
                lPolygonCount += lLayer2Selected.Count
            End If

            lSf1Ext = Nothing
            lShape1 = Nothing

            Dim lCurrentDisplay As Integer = Int(lPolygonCount / lTotalPolygonCount * 100)
            If lCurrentDisplay > lLastDisplayed Then
                lLastDisplayed = lCurrentDisplay
                GetMappingObject.StatusBar.ProgressBarValue = lCurrentDisplay
            End If
        Next i
        GetMappingObject.StatusBar.ShowProgressBar = False

        If aCreateNew Then 'delete old version of this file if it exists
            If FileExists(aOutputLayerName) Then
                System.IO.File.Delete(aOutputLayerName)
            End If
            If FileExists(FilenameNoExt(aOutputLayerName) & ".shx") Then
                System.IO.File.Delete(FilenameNoExt(aOutputLayerName) & ".shx")
            End If
            If FileExists(FilenameNoExt(aOutputLayerName) & ".dbf") Then
                System.IO.File.Delete(FilenameNoExt(aOutputLayerName) & ".dbf")
            End If
        End If

        lBsuc = lSfOut.SaveAs(aOutputLayerName)
        lSfOut.StopEditingShapes()
        lSfOut.Close()
    End Sub

    Public Shared Function ClipShapesWithPolygon(ByVal aInputLayerIndex As Integer, _
                                                 ByVal aClipperLayerIndex As Integer) As String
        'returns output shape file name as string 
        Dim i As Integer
        Dim j As Integer
        Dim k As Integer
        Dim lRetc As Boolean
        Dim lNewShapeFileName As String
        Dim lCount As Integer
        Dim lTotal As Integer

        ClipShapesWithPolygon = ""
        'set input layer (reaches for instance)
        Dim lSf As MapWinGIS.Shapefile = ShapeFileFromIndex(aInputLayerIndex)

        'set clipper layer (subbasins for instance)
        Dim lSfClip As MapWinGIS.Shapefile = ShapeFileFromIndex(aClipperLayerIndex)
        Dim lShapeClip As MapWinGIS.Shape
        Dim lPathClip As String = PathNameOnly(LayerFileName(aClipperLayerIndex))

        'create results shapefile
        i = 1
        lNewShapeFileName = lPathClip & "\stream" & i & ".shp"
        Do While FileExists(lNewShapeFileName)
            i += 1
            lNewShapeFileName = lPathClip & "\stream" & i & ".shp"
        Loop
        Dim lNewShapeFile As New MapWinGIS.Shapefile
        Dim rsf As New MapWinGIS.Shapefile
        lNewShapeFile.CreateNew(lNewShapeFileName, MapWinGIS.ShpfileType.SHP_POLYLINE)
        For i = 1 To lSf.NumFields
            lRetc = lNewShapeFile.EditInsertField(lSf.Field(i - 1), i - 1)
        Next i

        Dim issf As New MapWinGIS.Shapefile
        issf.CreateNew("temp_clip.shp", MapWinGIS.ShpfileType.SHP_POLYLINE)

        GetMappingObject.StatusBar.ShowProgressBar = True
        lCount = 0
        lTotal = lSfClip.NumShapes * lSf.NumShapes
        For i = 1 To lSfClip.NumShapes
            lShapeClip = lSfClip.Shape(i - 1)
            For j = 1 To lSf.NumShapes
                lCount += 1
                GetMappingObject.StatusBar.ProgressBarValue = Int(lCount / lTotal * 100)
                If IsLineInPolygon(lSf.Shape(j - 1), lSfClip, i - 1) Then
                    'at least one point of the line is in the polygon
                    If IsLineEntirelyInPolygon(lSf.Shape(j - 1), lSfClip, i - 1) Then
                        'just add to output shapefile
                        lRetc = lNewShapeFile.EditInsertShape(lSf.Shape(j - 1), lNewShapeFile.NumShapes)
                        'populate attributes of output shape
                        For k = 1 To lSf.NumFields
                            lRetc = lNewShapeFile.EditCellValue(k - 1, lNewShapeFile.NumShapes - 1, lSf.CellValue(k - 1, j - 1))
                        Next k
                    Else
                        'need to clip
                        lRetc = issf.EditInsertShape(lSf.Shape(j - 1), j - 1)
                        lRetc = MapWinX.SpatialOperations.ClipShapesWithPolygon(issf, lShapeClip, rsf)
                        lRetc = issf.EditDeleteShape(0)
                        If rsf.NumShapes > 0 Then
                            lRetc = lNewShapeFile.EditInsertShape(rsf.Shape(0), lNewShapeFile.NumShapes)
                            'populate attributes of output shape
                            For k = 1 To lSf.NumFields
                                lRetc = lNewShapeFile.EditCellValue(k - 1, lNewShapeFile.NumShapes - 1, lSf.CellValue(k - 1, j - 1))
                            Next k
                        End If
                    End If
                End If
            Next j
        Next i
        GetMappingObject.StatusBar.ShowProgressBar = False

        lRetc = lNewShapeFile.SaveAs(lNewShapeFileName)
        ClipShapesWithPolygon = lNewShapeFileName

    End Function

    Private Shared Function IsLineInPolygon(ByVal aLineShape As MapWinGIS.Shape, _
                                            ByVal aPolyShapeFile As MapWinGIS.Shapefile, _
                                            ByVal aIndex As Integer) As Boolean
        For i As Integer = 0 To aLineShape.numPoints - 1
            If aPolyShapeFile.PointInShape(aIndex, aLineShape.Point(i).x, _
                                                   aLineShape.Point(i).y) Then
                Return True 'a point was within polygon
            End If
        Next i
        Return False
    End Function

    Private Shared Function IsLineEntirelyInPolygon(ByVal aLineShape As MapWinGIS.Shape, _
                                                    ByVal aPolyShapeFile As MapWinGIS.Shapefile, _
                                                    ByVal aIndex As Integer) As Boolean
        For i As Integer = 0 To aLineShape.numPoints - 1
            If Not aPolyShapeFile.PointInShape(aIndex, aLineShape.Point(i).x, _
                                                       aLineShape.Point(i).y) Then
                Return False 'a point on line was outside polygon
            End If
        Next i
        Return True
    End Function

    Private Shared Function IsThisShapeTheSameAsOrPartOfAnotherShape(ByVal lSf1Shape As MapWinGIS.Shape, _
                                                                     ByVal lSf2Shape As MapWinGIS.Shape) As Boolean
        'given one shape and another which may be a piece of the first shape,
        'determine if the second shape is equivalent or part of the first shape
        '(after a reach segment has been clipped to a polygon boundary, we still need 
        'to identify which original reach it was a part of)
        Dim lFoundPoint As Boolean

        For j As Integer = 2 To lSf1Shape.numPoints - 1
            lFoundPoint = False
            For i As Integer = 1 To lSf2Shape.numPoints
                If lSf1Shape.Point(j - 1).x = lSf2Shape.Point(i - 1).x And _
                   lSf1Shape.Point(j - 1).y = lSf2Shape.Point(i - 1).y Then
                    lFoundPoint = True
                    Exit For
                End If
            Next i
            If lFoundPoint = False Then
                Return False
            End If
        Next j
        Return True
    End Function

    Public Shared Sub MergeFeaturesBasedOnAttribute(ByVal aLayerIndex As Integer, _
                                                    ByVal aFieldIndex As Integer)
        Dim lRetc As Boolean
        Dim lFound As Boolean

        Dim lsf As MapWinGIS.Shapefile = ShapeFileFromIndex(aLayerIndex)

        lsf.StartEditingShapes(True)
        'merge together based on common endpoints
        Dim i As Integer = 0
        Do While i < lsf.NumShapes
            lFound = False
            Dim lShape1 As MapWinGIS.Shape = lsf.Shape(i)
            Dim lTargetVal As Integer = FieldValue(aLayerIndex, i, aFieldIndex)
            Dim lEndX As Double = lShape1.Point(lShape1.numPoints - 1).x
            Dim lEndY As Double = lShape1.Point(lShape1.numPoints - 1).y
            Dim j As Integer = 0
            Do While j < lsf.NumShapes
                If i <> j Then
                    Dim lShape2 As MapWinGIS.Shape = lsf.Shape(j)
                    Dim lVal As Integer = FieldValue(aLayerIndex, j, aFieldIndex)
                    If lVal = lTargetVal Then
                        'see if these have common start/end 
                        If lEndX = lShape2.Point(0).x And lEndY = lShape2.Point(0).y Then
                            'end of shape 1 is start of shape2
                            For k As Integer = 1 To lShape2.numPoints
                                lRetc = lShape1.InsertPoint(lShape2.Point(k), lShape1.numPoints)
                            Next k
                            'remove shape2
                            lsf.EditDeleteShape(j)
                            lFound = True
                            Exit Do
                        End If
                    End If
                End If
                j += 1
            Loop
            If Not lFound Then
                i += 1
            End If
        Loop

        'merge together based on endpoint proximity
        Dim lStartX1 As Double
        Dim lStartY1 As Double
        Dim lEndX1 As Double
        Dim lEndY1 As Double
        Dim lStartX2 As Double
        Dim lStartY2 As Double
        Dim lEndX2 As Double
        Dim lEndY2 As Double
        i = 0
        Do While i < lsf.NumShapes
            lFound = False
            Dim lShape1 As MapWinGIS.Shape = lsf.Shape(i)
            Dim lTargetVal As Integer = FieldValue(aLayerIndex, i, aFieldIndex)
            lStartX1 = lShape1.Point(0).x
            lStartY1 = lShape1.Point(0).y
            lEndX1 = lShape1.Point(lShape1.numPoints - 1).x
            lEndY1 = lShape1.Point(lShape1.numPoints - 1).y
            Dim j As Integer = 0
            Do While j < lsf.NumShapes
                If i <> j Then
                    Dim lShape2 As MapWinGIS.Shape = lsf.Shape(j)
                    Dim lVal As Integer = FieldValue(aLayerIndex, j, aFieldIndex)
                    If lVal = lTargetVal Then
                        lStartX2 = lShape2.Point(0).x
                        lStartY2 = lShape2.Point(0).y
                        lEndX2 = lShape2.Point(lShape2.numPoints - 1).x
                        lEndY2 = lShape2.Point(lShape2.numPoints - 1).y
                        If (lStartX1 - lEndX2) ^ 2 + (lStartY1 - lEndY2) ^ 2 < (lStartX2 - lEndX1) ^ 2 + (lStartY2 - lEndY1) ^ 2 Then
                            'add shape 1 to the end of shape 2
                            For k As Integer = 1 To lShape1.numPoints
                                lRetc = lShape2.InsertPoint(lShape1.Point(k), lShape2.numPoints)
                            Next k
                            'remove shape1
                            lsf.EditDeleteShape(i)
                            lFound = True
                            Exit Do
                        Else
                            'add shape 2 to the end of shape 1
                            For k As Integer = 1 To lShape2.numPoints
                                lRetc = lShape1.InsertPoint(lShape2.Point(k), lShape1.numPoints)
                            Next k
                            'remove shape2
                            lsf.EditDeleteShape(j)
                            lFound = True
                            Exit Do
                        End If
                    End If
                End If
                j += 1
            Loop
            If Not lFound Then
                i += 1
            End If
        Loop

        lsf.StopEditingShapes(True)
    End Sub

    Public Shared Function AreaOverlappingPolygons(ByVal aLayer1index As Integer, _
                                                   ByVal aLayer1FeatureIndex As Integer, _
                                                   ByVal aLayer2index As Integer, _
                                                   ByVal aLayer2FeatureIndex As Integer) As Single
        'overlay feature from layer1 with feature from layer2, 
        'determining area of feature 1 in feature 2
        Dim lAreaOverlappingPolygons As Double = 0.0

        'set layer 1 
        Dim lLayer As MapWindow.Interfaces.Layer
        lLayer = GetMappingObject.Layers(aLayer1index)
        Dim lSf1 As MapWinGIS.Shapefile
        lSf1 = lLayer.GetObject
        Dim lShape1 As MapWinGIS.Shape
        lShape1 = lSf1.Shape(aLayer1FeatureIndex)

        'set layer 2 
        Dim lLayer2 As MapWindow.Interfaces.Layer
        lLayer2 = GetMappingObject.Layers(aLayer2index)
        Dim lsf2 As MapWinGIS.Shapefile
        lsf2 = lLayer2.GetObject
        Dim lShape2 As MapWinGIS.Shape
        lShape2 = lsf2.Shape(aLayer2FeatureIndex)

        Dim lNewShape As MapWinGIS.Shape

        lNewShape = MapWinX.SpatialOperations.Intersection(lShape1, lShape2)
        If lNewShape.numPoints > 0 Then
            lAreaOverlappingPolygons = Math.Abs(MapWinX.Utils.Area(lNewShape))
        End If
        lNewShape = Nothing
        Return lAreaOverlappingPolygons
    End Function
End Class
