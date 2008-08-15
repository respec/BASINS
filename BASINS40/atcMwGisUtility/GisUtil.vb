Imports atcUtility
Imports MapWinUtility

''' <remarks>Copyright 2005 AQUA TERRA Consultants - Royalty-free use permitted under open source license</remarks>
''' <summary>GIS Utilities implemented thru MapWindow</summary>
Public Class GisUtil
    Private Shared pMapWin As MapWindow.Interfaces.IMapWin
    Private Const UseCurrent As Integer = -1
    Private Shared pStatusShow As Boolean = True

    ''' <summary>Map Window Object</summary>
    ''' <exception cref="MappingObjectNotSetException">Mapping Object Not Set</exception>
    <CLSCompliant(False)> _
    Public Shared WriteOnly Property MappingObject() As MapWindow.Interfaces.IMapWin
        Set(ByVal aNewValue As MapWindow.Interfaces.IMapWin)
            pMapWin = aNewValue
        End Set
    End Property

    Public Shared Property StatusShow() As Boolean
        Get
            Return pStatusShow
        End Get
        Set(ByVal aNewValue As Boolean)
            pStatusShow = aNewValue
        End Set
    End Property

    ''' <summary>Map Window Object</summary>
    ''' <exception cref="MappingObjectNotSetException">Mapping Object Not Set</exception>
    Private Shared ReadOnly Property GetMappingObject() As MapWindow.Interfaces.IMapWin
        Get
            If pMapWin Is Nothing Then
                Throw New MappingObjectNotSetException
            Else
                Return pMapWin
            End If
        End Get
    End Property

    ''' <summary>Layers on map</summary>
    ''' <exception cref="MappingObjectNotSetException">Mapping Object Not Set</exception>
    Private Shared Function MapLayers() As ArrayList
        Dim lMapLayers As New ArrayList
        Dim lLastLayerIndex As Integer = GetMappingObject.Layers.NumLayers - 1
        For lLayerIndex As Integer = 0 To lLastLayerIndex
            lMapLayers.Add(LayerFromIndex(lLayerIndex))
        Next
        Return lMapLayers
    End Function

    ''' <summary>Check to see if feature index is valid</summary>
    ''' <exception cref="System.Exception" caption="FeatureIndexOutOfRange">Feature Index Out of Range</exception>
    ''' <exception cref="MappingObjectNotSetException">Mapping Object Not Set</exception>
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
    ''' <exception cref="System.Exception" caption="LoadFailure">Failure to Load Project</exception>
    ''' <exception cref="System.IO.IOException" caption="FileNotFound">File Not Found</exception>
    ''' <exception cref="MappingObjectNotSetException">Mapping Object Not Set</exception>
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
    ''' <exception cref="System.Exception" caption="LayerIndexOutOfRange">Layer specified by aLayerIndex does not exist</exception>
    ''' <exception cref="MappingObjectNotSetException">Mapping Object Not Set</exception>
    Public Shared Property CurrentLayer() As Integer
        Get
            'return the index, not the handle, of the current layer
            Dim lLayerIndex As Integer = 0
            For Each llayer As MapWindow.Interfaces.Layer In MapLayers()
                If GetMappingObject.Layers.GetHandle(lLayerIndex) = GetMappingObject.Layers.CurrentLayer Then
                    Return lLayerIndex
                End If
                lLayerIndex += 1
            Next
            'Throw New Exception("GisUtil:CurrentLayer:HasNoCorrespondingIndex")
            Return -1  'it is possible and acceptable to have no current layer
        End Get
        Set(ByVal aLayerIndex As Integer)
            If Not LayerFromIndex(aLayerIndex) Is Nothing Then
                GetMappingObject.Layers.CurrentLayer = GetMappingObject.Layers.GetHandle(aLayerIndex)
            End If
        End Set
    End Property

    ''' <summary>Obtain pointer to a shape file from a LayerIndex</summary>
    ''' <param name="aLayerIndex">
    '''   <para>Index of Layer containing ShapeFile</para>
    ''' </param>
    ''' <exception cref="System.Exception" caption="LayerNotShapeFile">Layer specified by aLayerIndex is not a ShapeFile</exception>
    ''' <exception cref="System.Exception" caption="LayerIndexOutOfRange">Layer specified by aLayerIndex does not exist</exception>
    ''' <exception cref="MappingObjectNotSetException">Mapping Object Not Set</exception>
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
                Throw New Exception("GisUtil:ShapeFileFromIndex:Error:LayerIndex:" & aLayerIndex & ":Type:" & LayerFromIndex(aLayerIndex).LayerType & ":IsNotShapeFile")
            End If
        End Get
    End Property

    ''' <summary>Obtain pointer to a polygon shape file from a LayerIndex</summary>
    ''' <param name="aLayerIndex">
    '''     <para>Index of Layer containing ShapeFile</para>
    ''' </param>
    ''' <exception cref="System.Exception" caption="LayerNotPolygonShapeFile">Layer specified by aLayerIndex is not a polygon ShapeFile</exception>
    ''' <exception cref="MappingObjectNotSetException">Mapping Object Not Set</exception>
    Private Shared ReadOnly Property PolygonShapeFileFromIndex(ByVal aLayerIndex As Integer) As MapWinGIS.Shapefile
        Get
            Dim lSf As MapWinGIS.Shapefile = ShapeFileFromIndex(aLayerIndex)
            If (lSf.ShapefileType = MapWinGIS.ShpfileType.SHP_POLYGON OrElse _
              lSf.ShapefileType = MapWinGIS.ShpfileType.SHP_POLYGONM OrElse _
              lSf.ShapefileType = MapWinGIS.ShpfileType.SHP_POLYGONZ) Then
                Return lSf
            Else
                Throw New Exception("GisUtil:PolygonShapeFileFromIndex:Error:LayerIndex:" & aLayerIndex & ":Type:" & LayerFromIndex(aLayerIndex).LayerType & ":IsNotPolygonShapeFile")
            End If
        End Get
    End Property

    ''' <summary>Obtain pointer to a grid from a LayerIndex</summary>
    ''' <param name="aLayerIndex">
    '''     <para>Index of desired layer containing grid (defaults to Current Layer)</para>
    ''' </param>
    ''' <exception cref="System.Exception" caption="LayerNotGrid">Layer specified by aLayerIndex is not a Grid</exception>
    ''' <exception cref="System.Exception" caption="LayerIndexOutOfRange">Layer specified by aLayerIndex does not exist</exception>
    ''' <exception cref="MappingObjectNotSetException">Mapping Object Not Set</exception>
    Private Shared ReadOnly Property GridFromIndex(Optional ByVal aLayerIndex As Integer = UseCurrent) As MapWinGIS.Grid
        Get
            If aLayerIndex = UseCurrent Then
                aLayerIndex = CurrentLayer
            End If

            Dim lLayer As MapWindow.Interfaces.Layer = LayerFromIndex(aLayerIndex)
            If lLayer.LayerType = MapWindow.Interfaces.eLayerType.Grid Then
                Return lLayer.GetGridObject
            Else
                Throw New Exception("GisUtil:GridFromIndex:Error:LayerIndex:" & aLayerIndex & ":Type:" & LayerFromIndex(aLayerIndex).LayerType & ":IsNotGrid")
            End If
        End Get
    End Property

    ''' <summary>Obtain pointer to a layer from a LayerIndex</summary>
    ''' <param name="aLayerIndex">
    '''     <para>Index of desired layer (defaults to Current Layer)</para>
    ''' </param>
    ''' <exception cref="System.Exception" caption="LayerIndexOutOfRange">Layer specified by aLayerIndex does not exist</exception>
    ''' <exception cref="MappingObjectNotSetException">Mapping Object Not Set</exception>
    Private Shared ReadOnly Property LayerFromIndex(Optional ByVal aLayerIndex As Integer = UseCurrent) As MapWindow.Interfaces.Layer
        Get
            If aLayerIndex = UseCurrent Then
                aLayerIndex = CurrentLayer
            End If

            If aLayerIndex >= 0 And aLayerIndex < GetMappingObject.Layers.NumLayers Then
                'Return (GetMappingObject.Layers(aLayerIndex))  'this is a bad idea
                Return (GetMappingObject.Layers.Item(GetMappingObject.Layers.GetHandle(aLayerIndex)))
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
    ''' <exception cref="System.Exception" caption="LayerNotPolygonShapeFile">Layer specified by aLayerIndex is not a polygon ShapeFile</exception>
    ''' <exception cref="System.Exception" caption="FeatureIndexOutOfRange">Feature Index Out of Range</exception>
    ''' <exception cref="MappingObjectNotSetException">Mapping Object Not Set</exception>
    Public Shared Function OverlappingPolygons( _
      ByVal aLayerIndex1 As Integer, ByVal aFeatureIndex1 As Integer, _
      ByVal aLayerIndex2 As Integer, ByVal aFeatureIndex2 As Integer) As Boolean

        Dim lOverlappingPolygons As Boolean = False

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
                        lOverlappingPolygons = lSf2.PointInShape(aFeatureIndex2, lX, lY)
                        If lOverlappingPolygons Then 'quit loop, we've found they overlap
                            Exit For
                        End If
                    Next lIndex

                    If Not lOverlappingPolygons Then 'now check the opposite
                        For lIndex = 1 To lSf2Shape.numPoints
                            lX = lSf2Shape.Point(lIndex - 1).x
                            lY = lSf2Shape.Point(lIndex - 1).y
                            lOverlappingPolygons = lSf1.PointInShape(aFeatureIndex1, lX, lY)
                            If lOverlappingPolygons Then 'quit loop, we've found they overlap
                                Exit For
                            End If
                        Next lIndex
                    End If
                End If
            End If
        End If
        Return lOverlappingPolygons
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
    ''' <exception cref="System.Exception" caption="LayerNotPolygonShapeFile">Layer specified by aLayerIndex is not a polygon ShapeFile</exception>
    ''' <exception cref="System.Exception" caption="FeatureIndexOutOfRange">Feature Index Out of Range</exception>
    ''' <exception cref="MappingObjectNotSetException">Mapping Object Not Set</exception>
    Public Shared Function LineInPolygon(ByVal aLineLayerIndex As Integer, _
                                         ByVal aLineIndex As Integer, _
                                         ByVal aPolygonLayerIndex As Integer, _
                                         ByVal aPolygonIndex As Integer) As Integer
        Dim lLineSf As MapWinGIS.Shapefile = ShapeFileFromIndex(aLineLayerIndex)
        Dim lLineShape As MapWinGIS.Shape = lLineSf.Shape(aLineIndex - 1)
        Dim lPolygonSf As MapWinGIS.Shapefile = PolygonShapeFileFromIndex(aPolygonLayerIndex)
        Dim lX As Double, lY As Double
        Dim lLineInPolygon As Integer = False
        For i As Integer = 1 To lLineShape.numPoints
            lX = lLineShape.Point(i - 1).x
            lY = lLineShape.Point(i - 1).y
            lLineInPolygon = lPolygonSf.PointInShape(aPolygonIndex, lX, lY)
            If lLineInPolygon Then 'point of line in polygon, true result
                Exit For
            End If
        Next i
        Return lLineInPolygon
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
    ''' <exception cref="System.Exception" caption="LayerNotPolygonShapeFile">Layer specified by aLayerIndex is not a polygon ShapeFile</exception>
    ''' <exception cref="MappingObjectNotSetException">Mapping Object Not Set</exception>
    Public Shared Sub AssignContainingPolygons( _
                                               ByVal aLayerIndex As Integer, _
                                               ByVal aLayerIndexContaining As Integer, _
                                               ByRef aIndex() As Integer)
        Dim lSf1 As MapWinGIS.Shapefile = ShapeFileFromIndex(aLayerIndex)
        Dim lSf2 As MapWinGIS.Shapefile = PolygonShapeFileFromIndex(aLayerIndexContaining)

        Dim lX As Double, lY As Double
        Dim lSf1Shape As New MapWinGIS.Shape
        Dim lNth As Integer

        ReDim aIndex(lSf1.NumShapes)

        lSf2.BeginPointInShapefile()
        For lShapeIndex As Integer = 1 To lSf1.NumShapes
            aIndex(lShapeIndex) = -1
            lSf1Shape = lSf1.Shape(lShapeIndex - 1)
            For lPointIndex As Integer = 2 To lSf1Shape.numPoints
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
    ''' <exception cref="System.Exception" caption="LayerNotShapeFile">Layer specified by aLayerIndex is not a ShapeFile</exception>
    ''' <exception cref="System.Exception" caption="LayerIndexOutOfRange">Layer specified by aLayerIndex does not exist</exception>
    ''' <exception cref="MappingObjectNotSetException">Mapping Object Not Set</exception>
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
    ''' <exception cref="System.Exception" caption="FieldIndexOutOfRange">Field specified by aFieldIndex does not exist</exception>
    ''' <exception cref="MappingObjectNotSetException">Mapping Object Not Set</exception>
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
    ''' <exception cref="System.Exception" caption="LayerNotShapeFile">Layer specified by aLayerIndex is not a ShapeFile</exception>
    ''' <exception cref="System.Exception" caption="LayerIndexOutOfRange">Layer specified by aLayerIndex does not exist</exception>
    ''' <exception cref="System.Exception" caption="FieldIndexOutOfRange">Field specified by aFieldIndex does not exist</exception>
    ''' <exception cref="MappingObjectNotSetException">Mapping Object Not Set</exception>
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
    ''' <exception cref="System.Exception" caption="LayerNotShapeFile">Layer specified by aLayerIndex is not a ShapeFile</exception>
    ''' <exception cref="System.Exception" caption="LayerIndexOutOfRange">Layer specified by aLayerIndex does not exist</exception>
    ''' <exception cref="System.Exception" caption="FieldNameNotRecognized">Field specified by aFieldName is not recognized</exception>
    ''' <exception cref="MappingObjectNotSetException">Mapping Object Not Set</exception>
    Public Shared Function FieldIndex(ByVal aLayerIndex As Integer, ByVal aFieldName As String) As Integer
        Dim lSf As MapWinGIS.Shapefile = ShapeFileFromIndex(aLayerIndex)

        For iFieldIndex As Integer = 0 To lSf.NumFields - 1
            If lSf.Field(iFieldIndex).Name.ToUpper = aFieldName.ToUpper Then 'this is the field we want
                Return iFieldIndex
            End If
        Next
        Throw New Exception("GisUtil:FieldIndex:Error:FieldName:" & aFieldName & ":IsNotRecognized")
    End Function

    Public Shared Function IsField(ByVal aLayerIndex As Integer, _
                                   ByVal aFieldName As String) As Boolean
        Dim lSf As MapWinGIS.Shapefile = ShapeFileFromIndex(aLayerIndex)
        For lFieldIndex As Integer = 0 To lSf.NumFields - 1
            If lSf.Field(lFieldIndex).Name.ToUpper = aFieldName.ToUpper Then 'this is the field we want
                Return True
            End If
        Next
        Return False 'field name not found
    End Function

    Public Shared Function FieldIndexAddIfMissing(ByVal aLayerIndex As Integer, ByVal aFieldName As String, _
                                                  ByVal aFieldType As Integer, ByVal aFieldWidth As Integer) As Integer
        Dim lFieldIndex As Integer
        If IsField(aLayerIndex, aFieldName) Then
            lFieldIndex = GisUtil.FieldIndex(aLayerIndex, aFieldName)
        Else 'need to add it
            lFieldIndex = GisUtil.AddField(aLayerIndex, aFieldName, aFieldType, aFieldWidth)
        End If
        Return lFieldIndex
    End Function

    ''' <summary>Add a field in a shape file from a layer index and a field name</summary>
    ''' <param name="aLayerIndex">
    '''     <para>Index of layer containing shape file</para>
    ''' </param>
    ''' <param name="aFieldName">
    '''     <para>Name of field to add</para>
    ''' </param>
    Public Shared Function AddField(ByVal aLayerIndex As Integer, ByVal aFieldName As String, _
                                    ByVal aFieldType As Integer, ByVal aFieldWidth As Integer) As Integer
        Dim lSf As MapWinGIS.Shapefile = ShapeFileFromIndex(aLayerIndex)
        Return AddField(aLayerIndex, aFieldName, aFieldType, aFieldWidth, lSf.NumFields)
    End Function


    ''' <summary>Add a field in a shape file from a layer index and a field name</summary>
    ''' <param name="aLayerIndex">
    '''     <para>Index of layer containing shape file</para>
    ''' </param>
    ''' <param name="aFieldName">
    '''     <para>Name of field to add</para>
    ''' </param>
    ''' <exception cref="System.Exception" caption="LayerNotShapeFile">Layer specified by aLayerIndex is not a ShapeFile</exception>
    ''' <exception cref="System.Exception" caption="LayerIndexOutOfRange">Layer specified by aLayerIndex does not exist</exception>
    ''' <exception cref="MappingObjectNotSetException">Mapping Object Not Set</exception>
    Public Shared Function AddField(ByVal aLayerIndex As Integer, ByVal aFieldName As String, _
                                    ByVal aFieldType As Integer, ByVal aFieldWidth As Integer, _
                                    ByVal aFieldAfter As Integer) As Integer
        Dim lField As New MapWinGIS.Field
        lField.Name = aFieldName
        lField.Type = aFieldType
        lField.Width = aFieldWidth

        Dim lSf As MapWinGIS.Shapefile = ShapeFileFromIndex(aLayerIndex)

        lSf.StartEditingTable()
        'TODO: error handling
        Dim lBsuc As Boolean = lSf.EditInsertField(lField, aFieldAfter)
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
    ''' <exception cref="System.Exception" caption="FieldIndexOutOfRange">Field specified by aFieldIndex does not exist</exception>
    ''' <exception cref="System.Exception" caption="LayerNotShapeFile">Layer specified by aLayerIndex is not a ShapeFile</exception>
    ''' <exception cref="System.Exception" caption="LayerIndexOutOfRange">Layer specified by aLayerIndex does not exist</exception>
    ''' <exception cref="MappingObjectNotSetException">Mapping Object Not Set</exception>
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
    ''' <exception cref="MappingObjectNotSetException">Mapping Object Not Set</exception>
    Public Shared ReadOnly Property NumLayers() As Integer
        Get
            Return GetMappingObject.Layers.NumLayers()
        End Get
    End Property

    ''' <summary>Layer type from a layer index</summary>
    ''' <param name="aLayerIndex">
    '''     <para>Index of layer. (Defaults to current layer)</para>
    ''' </param>
    ''' <exception cref="System.Exception" caption="LayerIndexOutOfRange">Layer specified by aLayerIndex does not exist</exception>
    ''' <exception cref="MappingObjectNotSetException">Mapping Object Not Set</exception>
    Public Shared ReadOnly Property LayerType(Optional ByVal aLayerIndex As Integer = UseCurrent) As Integer
        Get
            Return LayerFromIndex(aLayerIndex).LayerType
        End Get
    End Property

    ''' <summary>Layer name from a layer index</summary>
    ''' <param name="aLayerIndex">
    '''     <para>Index of layer (Defaults to current layer)</para>
    ''' </param>
    ''' <exception cref="System.Exception" caption="LayerIndexOutOfRange">Layer specified by aLayerIndex does not exist</exception>
    ''' <exception cref="MappingObjectNotSetException">Mapping Object Not Set</exception>
    Public Shared ReadOnly Property LayerName(Optional ByVal aLayerIndex As Integer = UseCurrent) As String
        Get
            Return LayerFromIndex(aLayerIndex).Name
        End Get
    End Property

    ''' <summary>Index of a layer from a name</summary>
    ''' <param name="aLayerName">
    '''     <para>Name or filename of layer to obtain index for</para>
    ''' </param>
    ''' <exception cref="System.Exception" caption="LayerNameNameNotRecognized">Layer specified by aLayerName does not exist</exception>
    ''' <exception cref="System.Exception" caption="LayerIndexOutOfRange">Layer specified by aLayerIndex does not exist</exception>
    ''' <exception cref="MappingObjectNotSetException">Mapping Object Not Set</exception>
    Public Shared Function LayerIndex(ByVal aLayerName As String) As Integer
        'start at topmost layer and work down looking for a match
        aLayerName = aLayerName.ToUpper
        Dim i As Integer
        For i = GetMappingObject.Layers.NumLayers - 1 To 0 Step -1
            If LayerName(i).ToUpper = aLayerName OrElse LayerFileName(i).ToUpper = aLayerName Then
                Return i
            End If
        Next

        Throw New Exception("GisUtil:LayerIndex:Error:LayerName:" & aLayerName & ":IsNotRecognized")
    End Function

    ''' <summary>Layer file name from a layer index</summary>
    ''' <param name="aLayerIndex">
    '''     <para>Index of layer (Defaults to current layer)</para>
    ''' </param>
    ''' <exception cref="System.Exception" caption="LayerIndexOutOfRange">Layer specified by aLayerIndex does not exist</exception>
    ''' <exception cref="MappingObjectNotSetException">Mapping Object Not Set</exception>
    Public Shared ReadOnly Property LayerFileName(Optional ByVal aLayerIndex As Integer = UseCurrent) As String
        Get
            Return LayerFromIndex(aLayerIndex).FileName
        End Get
    End Property

    ''' <summary>Index of a group from a name</summary>
    ''' <param name="aGroupName">
    '''     <para>Name of group to obtain index for</para>
    ''' </param>
    ''' <exception cref="System.Exception" caption="GroupNameNameNotRecognized">Group specified by aGroupName does not exist</exception>
    ''' <exception cref="MappingObjectNotSetException">Mapping Object Not Set</exception>
    Public Shared Function GroupIndex(ByVal aGroupName As String) As Integer
        GroupIndex = 0
        For i As Integer = 0 To GetMappingObject.Layers.Groups.Count - 1
            If GetMappingObject.Layers.Groups(i).Text.ToUpper = aGroupName.ToUpper Then
                Return i
            End If
        Next

        Throw New Exception("GisUtil:GroupIndex:Error:GroupName:" & aGroupName & ":IsNotRecognized")
    End Function

    ''' <summary>handle of a group from a name</summary>
    ''' <param name="aGroupName">
    '''     <para>Name of group to obtain handle for</para>
    ''' </param>
    ''' <exception cref="System.Exception" caption="GroupNameNameNotRecognized">Group specified by aGroupName does not exist</exception>
    ''' <exception cref="MappingObjectNotSetException">Mapping Object Not Set</exception>
    Public Shared Function GroupHandle(ByVal aGroupName As String) As Integer
        GroupHandle = GetMappingObject.Layers.Groups.ItemByPosition(GroupIndex(aGroupName)).Handle
    End Function

    Public Shared Function AddGroup(ByVal aGroupName As String) As Integer
        'given a group name, add it to the map.
        'return true if the group is already there or successfully added.
        AddGroup = -1
        Try
            AddGroup = GroupIndex(aGroupName)
        Catch
            AddGroup = GetMappingObject.Layers.Groups.Add(aGroupName)
        End Try
    End Function

    ''' <summary>Number of features from a layer index</summary>
    ''' <param name="aLayerIndex">
    '''     <para>Index of layer (Defaults to current layer)</para>
    ''' </param>
    ''' <exception cref="System.Exception" caption="LayerNotShapeFile">Layer specified by aLayerIndex is not a ShapeFile</exception>
    ''' <exception cref="System.Exception" caption="LayerIndexOutOfRange">Layer specified by aLayerIndex does not exist</exception>
    ''' <exception cref="MappingObjectNotSetException">Mapping Object Not Set</exception>
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
    ''' <exception cref="System.Exception" caption="FieldIndexOutOfRange">Field specified by aFieldIndex does not exist</exception>
    ''' <exception cref="System.Exception" caption="LayerNotShapeFile">Layer specified by aLayerIndex is not a ShapeFile</exception>
    ''' <exception cref="System.Exception" caption="LayerIndexOutOfRange">Layer specified by aLayerIndex does not exist</exception>
    ''' <exception cref="MappingObjectNotSetException">Mapping Object Not Set</exception>
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
    ''' <exception cref="System.Exception" caption="FeatureIndexOutOfRange">Feature specified by aFeatureIndex does not exist</exception>
    ''' <exception cref="System.Exception" caption="FieldIndexOutOfRange">Field specified by aFieldIndex does not exist</exception>
    ''' <exception cref="System.Exception" caption="LayerNotShapeFile">Layer specified by aLayerIndex is not a ShapeFile</exception>
    ''' <exception cref="System.Exception" caption="LayerIndexOutOfRange">Layer specified by aLayerIndex does not exist</exception>
    ''' <exception cref="MappingObjectNotSetException">Mapping Object Not Set</exception>
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

    Public Shared Function FindFeatureIndex(ByVal aLayerIndex As Integer, ByVal aFieldIndex As Integer, ByVal aValue As String) As Integer
        For lFeatureIndex As Integer = 0 To GisUtil.NumFeatures(aLayerIndex) - 1
            If GisUtil.FieldValue(aLayerIndex, lFeatureIndex, aFieldIndex) = aValue Then
                Return lFeatureIndex
            End If
        Next
        Return -1
    End Function

    Public Shared Sub FindNearestPointAndLoc(ByRef aX As Double, ByRef aY As Double, _
                                             ByVal aLineLayerIndex As Integer, ByVal aLineFeatureIndex As Integer)
        Dim lPoint As New MapWinGIS.Point
        lPoint.x = aX
        lPoint.y = aY
        Dim lsf As MapWinGIS.Shapefile = ShapeFileFromIndex(aLineLayerIndex)
        If FeatureIndexValid(aLineFeatureIndex, lsf) Then
            Dim lShape As MapWinGIS.Shape = lsf.Shape(aLineFeatureIndex)
            Dim lPointOut As New MapWinGIS.Point
            Dim lLocation As Integer
            Dim lDistance As Double
            MapWinGeoProc.Utils.FindNearestPointAndLoc(lPoint, lShape, lPointOut, lLocation, lDistance)
            aX = lPointOut.x
            aY = lPointOut.y
        Else
            Throw New ApplicationException("TODO:")
        End If
    End Sub
    ''' <summary>Minimum value in a grid from a LayerIndex</summary>
    ''' <param name="aLayerIndex">
    '''     <para>Index of desired layer containing grid (defaults to Current Layer)</para>
    ''' </param>
    ''' <exception cref="System.Exception" caption="LayerNotGrid">Layer specified by aLayerIndex is not a Grid</exception>
    ''' <exception cref="System.Exception" caption="LayerIndexOutOfRange">Layer specified by aLayerIndex does not exist</exception>
    ''' <exception cref="MappingObjectNotSetException">Mapping Object Not Set</exception>
    Public Shared ReadOnly Property GridLayerMinimum(Optional ByVal aLayerIndex As Integer = UseCurrent) As Object
        Get
            Return GridFromIndex(aLayerIndex).Minimum
        End Get
    End Property

    ''' <summary>Maximum value in a grid from a LayerIndex</summary>
    ''' <param name="aLayerIndex">
    '''     <para>Index of desired layer containing grid (defaults to Current Layer)</para>
    ''' </param>
    ''' <exception cref="System.Exception" caption="LayerNotGrid">Layer specified by aLayerIndex is not a Grid</exception>
    ''' <exception cref="System.Exception" caption="LayerIndexOutOfRange">Layer specified by aLayerIndex does not exist</exception>
    ''' <exception cref="MappingObjectNotSetException">Mapping Object Not Set</exception>
    Public Shared ReadOnly Property GridLayerMaximum(Optional ByVal aLayerIndex As Integer = UseCurrent) As Object
        Get
            Return GridFromIndex(aLayerIndex).Maximum
        End Get
    End Property

    ''' <summary>Area of a polygon from a LayerIndex and a FeatureIndex</summary>
    ''' <param name="aLayerIndex">
    '''     <para>Index of desired layer containing grid (defaults to Current Layer)</para>
    ''' </param>
    ''' <exception cref="System.Exception" caption="LayerNotPolygonShapeFile">Layer specified by aLayerIndex is not a polygon ShapeFile</exception>
    ''' <exception cref="System.Exception" caption="FeatureIndexOutOfRange">Feature Index Out of Range</exception>
    ''' <exception cref="MappingObjectNotSetException">Mapping Object Not Set</exception>
    Public Shared Function FeatureArea(ByVal aLayerIndex As Integer, ByVal aFeatureIndex As Integer) As Double
        Dim lArea As Double

        Dim lSf As MapWinGIS.Shapefile = PolygonShapeFileFromIndex(aLayerIndex)
        If FeatureIndexValid(aFeatureIndex, lSf) Then
            Try
                lArea = MapWinGeoProc.Utils.Area(lSf.Shape(aFeatureIndex))
                If lArea < 0.000001 Then 'TODO: try to calculate?
                    lArea = GetNaN()
                End If
            Catch ex As Exception
                Logger.Dbg("GisUtil:FeatureArea:Exception:" & ex.Message)
                lArea = GetNaN()
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
                        lLength = GetNaN()
                    End If
                End If
            End If
        Catch ex As Exception
            Logger.Dbg("GisUtil:FeatureLength:Exception:" & ex.Message)
            lLength = GetNaN()
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
    ''' <exception cref="System.Exception" caption="LayerNotShapeFile">Layer specified by aLayerIndex is not a ShapeFile</exception>
    ''' <exception cref="System.Exception" caption="FeatureIndexOutOfRange">Feature Index Out of Range</exception>
    ''' <exception cref="MappingObjectNotSetException" caption="MappingObjectNotSet">Mapping Object Not Set</exception>
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

    ''' <summary>
    ''' Determine a points of a line
    ''' </summary>
    ''' <param name="aLayerIndex"></param>
    ''' <param name="aFeatureIndex"></param>
    ''' <param name="aX"></param>
    ''' <param name="aY"></param>
    ''' <remarks></remarks>
    Public Shared Sub PointsOfLine(ByVal aLayerIndex As Integer, ByVal aFeatureIndex As Integer, _
                                   ByRef aX() As Double, ByRef aY() As Double)
        Dim lsf As MapWinGIS.Shapefile = ShapeFileFromIndex(aLayerIndex)
        If FeatureIndexValid(aFeatureIndex, lsf) Then
            Dim lShape As MapWinGIS.Shape = lsf.Shape(aFeatureIndex)
            ReDim aX(lShape.numPoints - 1)
            ReDim aY(lShape.numPoints - 1)
            For lIndex As Integer = 0 To lShape.numPoints - 1
                aX(lIndex) = lShape.Point(lIndex).x
                aY(lIndex) = lShape.Point(lIndex).y
            Next
        End If
    End Sub

    ''' <summary>Determine first point of a shape</summary>
    ''' <param name="aLayerIndex">
    '''     <para>Index of layer containing ShapeFile</para>
    ''' </param>
    ''' <param name="aFeatureIndex">
    '''     <para>Index of feature</para>
    ''' </param>
    ''' <exception cref="System.Exception" caption="LayerNotShapeFile">Layer specified by aLayerIndex is not a ShapeFile</exception>
    ''' <exception cref="System.Exception" caption="FeatureIndexOutOfRange">Feature Index Out of Range</exception>
    ''' <exception cref="MappingObjectNotSetException" caption="MappingObjectNotSet">Mapping Object Not Set</exception>
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
    ''' <exception cref="System.Exception" caption="LayerNotShapeFile">Layer specified by aLayerIndex is not a ShapeFile</exception>
    ''' <exception cref="System.Exception" caption="FeatureIndexOutOfRange">Feature Index Out of Range</exception>
    ''' <exception cref="MappingObjectNotSetException">Mapping Object Not Set</exception>
    Public Shared Function RemoveFeature(ByVal aLayerIndex As Integer, ByVal aFeatureIndex As Integer) As Double
        Dim lSf As MapWinGIS.Shapefile = ShapeFileFromIndex(aLayerIndex)

        If FeatureIndexValid(aFeatureIndex, lSf) Then
            lSf.StartEditingShapes(True)
            Dim lRetc As Boolean = lSf.EditDeleteShape(aFeatureIndex)
            lSf.StopEditingShapes(True, True)
            pMapWin.View.Redraw()
            'TODO: what is the return value
        End If
    End Function

    Public Shared Sub StartRemoveFeature(ByVal aLayerIndex As Integer)
        Dim lSf As MapWinGIS.Shapefile = ShapeFileFromIndex(aLayerIndex)
        lSf.StartEditingShapes(True)
    End Sub

    Public Shared Function RemoveFeatureNoStartStop(ByVal aLayerIndex As Integer, ByVal aFeatureIndex As Integer) As Double
        Dim lSf As MapWinGIS.Shapefile = ShapeFileFromIndex(aLayerIndex)

        If FeatureIndexValid(aFeatureIndex, lSf) Then
            Dim lRetc As Boolean = lSf.EditDeleteShape(aFeatureIndex)
        End If
    End Function

    Public Shared Function AddPoint(ByVal aLayerIndex As Integer, ByVal aX As Double, ByVal aY As Double) As Boolean
        Dim lResult As Boolean
        Dim lSf As MapWinGIS.Shapefile = ShapeFileFromIndex(aLayerIndex)
        Dim lPoint As New MapWinGIS.Point
        lPoint.x = aX
        lPoint.y = aY
        Dim lShape As New MapWinGIS.Shape
        lResult = lShape.Create(MapWinGIS.ShpfileType.SHP_POINT)
        lResult = lShape.InsertPoint(lPoint, 0)
        lResult = lSf.StartEditingShapes()
        lResult = lSf.EditInsertShape(lShape, lSf.NumShapes)
        lResult = lSf.StopEditingShapes
        pMapWin.View.Redraw()
    End Function

    Public Shared Sub StopRemoveFeature(ByVal aLayerIndex As Integer)
        Dim lSf As MapWinGIS.Shapefile = ShapeFileFromIndex(aLayerIndex)
        lSf.StopEditingShapes(True, True)
        pMapWin.View.Redraw()
    End Sub

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
    ''' <exception cref="System.Exception" caption="LayerNotShapeFile">Layer specified by aLayerIndex is not a ShapeFile</exception>
    ''' <exception cref="System.Exception" caption="FeatureIndexOutOfRange">Feature Index Out of Range</exception>
    ''' <exception cref="System.Exception" caption="FieldIndexOutOfRange">Field specified by aFieldIndex does not exist</exception>
    ''' <exception cref="MappingObjectNotSetException">Mapping Object Not Set</exception>
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

    Public Shared Sub StartSetFeatureValue(ByVal aLayerIndex As Integer)
        Dim lSf As MapWinGIS.Shapefile = ShapeFileFromIndex(aLayerIndex)
        Dim lRetc As Boolean
        lRetc = lSf.StartEditingTable()
    End Sub

    Public Shared Sub SetFeatureValueNoStartStop(ByVal aLayerIndex As Integer, ByVal aFieldIndex As Integer, ByVal aFeatureIndex As Integer, ByVal aValue As Object)
        Dim lSf As MapWinGIS.Shapefile = ShapeFileFromIndex(aLayerIndex)
        If FeatureIndexValid(aFeatureIndex, lSf) Then
            If FieldIndexValid(aFieldIndex, lSf) Then
                Dim lRetc As Boolean
                'TODO: error checks
                lRetc = lSf.EditCellValue(aFieldIndex, aFeatureIndex, aValue)
            End If
        End If
    End Sub

    Public Shared Sub StopSetFeatureValue(ByVal aLayerIndex As Integer)
        Dim lSf As MapWinGIS.Shapefile = ShapeFileFromIndex(aLayerIndex)
        Dim lRetc As Boolean
        lRetc = lSf.StopEditingTable()
    End Sub

    Public Shared Function NumSelectedFeatures(ByVal aLayerIndex As Integer) As Integer
        Dim lLayer As MapWindow.Interfaces.Layer
        Dim lSi As MapWindow.Interfaces.SelectInfo

        lSi = pMapWin.View.SelectedShapes
        lLayer = LayerFromIndex(aLayerIndex)
        If lSi.LayerHandle = lLayer.Handle Then
            NumSelectedFeatures = lSi.NumSelected()
        Else
            NumSelectedFeatures = 0
        End If
    End Function

    Public Shared Function ClearSelectedFeatures(ByVal aLayerIndex As Integer) As Boolean
        CurrentLayer = aLayerIndex
        pMapWin.View.SelectedShapes.ClearSelectedShapes()
        Return True
    End Function

    Public Shared Function SetSelectedFeature(ByVal aLayerIndex As Integer, ByVal aShapeIndex As Integer) As Boolean
        CurrentLayer = aLayerIndex
        Dim lSelectColor As System.Drawing.Color = pMapWin.View.SelectColor
        pMapWin.View.SelectedShapes.AddByIndex(aShapeIndex, lSelectColor)
        Return True
    End Function

    Public Shared Function IndexOfNthSelectedFeatureInLayer(ByVal nth As Integer, ByVal aLayerIndex As Integer) As Integer
        Dim lsi As MapWindow.Interfaces.SelectInfo = GetMappingObject.View.SelectedShapes
        Dim lLayer As MapWindow.Interfaces.Layer = LayerFromIndex(aLayerIndex)
        If CurrentLayer = aLayerIndex Then
            IndexOfNthSelectedFeatureInLayer = lsi.Item(nth).ShapeIndex
        End If
    End Function

    Public Shared Sub SaveSelectedFeatures(ByVal aInputLayerName As String, ByVal aOutputLayerName As String, _
                                           Optional ByVal aAddToMap As Boolean = True)
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
        Dim lInputProjectionFileName As String = FilenameSetExt(lsf.Filename, "prj")
        If FileExists(lInputProjectionFileName) Then
            FileCopy(lInputProjectionFileName, FilenameSetExt(aOutputLayerName, "prj"))
        End If
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

        If aAddToMap Then
            AddLayer(aOutputLayerName, aOutputLayerName)
        End If

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
    ''' <exception cref="MappingObjectNotSetException">Mapping Object Not Set</exception>
    Public Shared ReadOnly Property ProjectFileName() As String
        Get
            Return GetMappingObject.Project.FileName
        End Get
    End Property

    ''' <summary>
    ''' Projection string describing the current map projection for the open project
    ''' </summary>
    ''' <returns>PROJ 4 string</returns>
    ''' <exception cref="MappingObjectNotSetException">Mapping Object Not Set</exception>
    Public Shared Function ProjectProjection() As String
        Return GetMappingObject.Project.ProjectProjection
    End Function

    ''' <summary>
    ''' Changes aX and aY arguments from aInputProjection to aOutputProjection
    ''' </summary>
    ''' <param name="aX">X or longitude value (input and output)</param>
    ''' <param name="aY">Y or latitude value (input and output)</param>
    ''' <param name="aInputProjection">PROJ 4 string describing projection of aX and aY as they are passed in</param>
    ''' <param name="aOutputProjection">PROJ 4 string describing new projection of aX and aY as they are passed back</param>
    Public Shared Sub ProjectPoint(ByRef aX As Double, ByRef aY As Double, _
                                   ByVal aInputProjection As String, _
                                   ByVal aOutputProjection As String)
        MapWinGeoProc.SpatialReference.ProjectPoint(aX, aY, aInputProjection, aOutputProjection)
    End Sub

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
            If Not FileExists(aFileName) Then
                AddLayer = False
            Else
                Dim lLayer As MapWindow.Interfaces.Layer = GetMappingObject.Layers.Add(aFileName, aLayerName)
                If lLayer Is Nothing Then
                    AddLayer = False
                Else
                    lLayer.Name = aLayerName
                    lLayer.Visible = False
                    AddLayer = True
                    GetMappingObject.Project.Modified = True
                End If
            End If
        End If
    End Function

    Public Shared Function AddLayerToGroup(ByVal aFileName As String, _
                                           ByVal aLayerName As String, _
                                           ByVal aGroupName As String) As Boolean
        'given a shape file name, add it to the map in the specified group.
        'return true if the layer is already there or successfully added.
        AddLayerToGroup = AddLayer(aFileName, aLayerName)
        Dim lGroupHandle As Integer
        Try
            lGroupHandle = GroupHandle(aGroupName)
            LayerFromIndex(LayerIndex(aLayerName)).MoveTo(99, lGroupHandle)
        Catch
            AddLayerToGroup = False
        End Try
    End Function

    ''' <summary>Layer part of project?</summary>
    ''' <param name="aLayerName">
    '''     <para>Name of layer to compare to layeres in project</para>
    ''' </param>
    Public Shared Function IsLayer(ByVal aLayerName As String) As Boolean
        Dim lLayerName As String = aLayerName.ToUpper
        For i As Integer = 0 To GetMappingObject.Layers.NumLayers - 1
            Try
                If lLayerName = LayerFromIndex(i).Name.ToUpper OrElse lLayerName = LayerFromIndex(i).FileName.ToUpper Then
                    Return True
                End If
            Catch
                Logger.Dbg("Layer " & i & " of " & GetMappingObject.Layers.NumLayers & " Problem")
            End Try
        Next i
        Return False
    End Function

    ''' <summary>Layer part of project?</summary>
    ''' <param name="aLayerFileName">
    '''     <para>file name of layer to compare to layers in project</para>
    ''' </param>
    Public Shared Function IsLayerByFileName(ByVal aLayerFileName As String) As Boolean
        For i As Integer = 0 To GetMappingObject.Layers.NumLayers - 1
            Try
                If aLayerFileName.ToUpper = LayerFromIndex(i).FileName.ToUpper Then
                    Return True
                End If
            Catch
                Logger.Dbg("Layer " & i & " of " & GetMappingObject.Layers.NumLayers & " Problem")
            End Try
        Next i
        Return False
    End Function

    ''' <summary>Layer visible flag</summary>
    ''' <param name="aLayerName">
    '''     <para>Name of layer</para>
    ''' </param>
    ''' <exception cref="System.Exception" caption="LayerNameNameNotRecognized">Layer specified by aLayerName does not exist</exception>
    ''' <exception cref="MappingObjectNotSetException">Mapping Object Not Set</exception>
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
    ''' <exception cref="System.Exception" caption="LayerIndexOutOfRange">Layer specified by aLayerIndex does not exist</exception>
    ''' <exception cref="MappingObjectNotSetException">Mapping Object Not Set</exception>
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
    ''' <exception cref="System.Exception" caption="LayerIndexOutOfRange">Layer specified by aLayerIndex does not exist</exception>
    ''' <exception cref="MappingObjectNotSetException">Mapping Object Not Set</exception>
    Public Shared Function RemoveLayer(Optional ByVal aLayerIndex As Integer = UseCurrent) As Boolean
        If aLayerIndex = UseCurrent Then aLayerIndex = CurrentLayer
        Dim lLayerName As String = LayerName(aLayerIndex) 'forces check of index
        GetMappingObject.Layers.Remove(GetMappingObject.Layers.GetHandle(aLayerIndex))
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
        If lStartingRow < 0 Then
            lStartingRow = 0
        End If
        If lEndingRow > lInputGrid.Header.NumberRows - 1 Then
            lEndingRow = lInputGrid.Header.NumberRows - 1
        End If
        If lStartingColumn < 0 Then
            lStartingColumn = 0
        End If
        If lEndingColumn > lInputGrid.Header.NumberCols - 1 Then
            lEndingColumn = lInputGrid.Header.NumberCols - 1
        End If

        Dim lCellArea As Double = lInputGrid.Header.dX * lInputGrid.Header.dY
        Dim lTotalCellCount As Integer = (lEndingColumn - lStartingColumn) * (lEndingRow - lStartingRow)
        Dim lCellCount As Integer = 0

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
                    If lGridValue > -1 And lGridValue <= aAreaGridPoly.GetUpperBound(0) Then
                        'make sure the grid value is not going to blow out the array
                        aAreaGridPoly(lGridValue, lInsideId) += lCellArea
                    End If
                End If
                lCellCount += 1
                If pStatusShow Then Logger.Progress(lCellCount, lTotalCellCount)
            Next lCol
        Next lRow

        If pStatusShow Then Logger.Progress(lTotalCellCount, lTotalCellCount)
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
                    If pStatusShow Then Logger.Progress(cellcount, totalcellcount)
                Next lRow
            Next lCol
            If pStatusShow Then Logger.Progress(totalcellcount, totalcellcount)
            lPolygonSf.EndPointInShapefile()
        End If
    End Sub

    Public Shared Sub GridMeanInPolygon(ByVal aGridLayerName As String, ByVal aPolygonLayerIndex As Integer, _
                                        ByVal aPolygonFeatureIndex As Integer, _
                                        ByRef aMean As Double)
        'Given a grid and a polygon layer, find the mean grid value within the feature.

        'set input grid
        Dim lInputGrid As New MapWinGIS.Grid
        lInputGrid.Open(aGridLayerName)

        'set input polygon layer
        Dim lPolygonSf As MapWinGIS.Shapefile = PolygonShapeFileFromIndex(aPolygonLayerIndex)
        Dim lShape As New MapWinGIS.Shape
        If FeatureIndexValid(aPolygonFeatureIndex, lPolygonSf) Then
            lShape = lPolygonSf.Shape(aPolygonFeatureIndex)

            'figure out what part of the grid overlays this polygon
            Dim lStartCol As Integer
            Dim lEndCol As Integer
            Dim lStartRow As Integer
            Dim lEndRow As Integer
            lInputGrid.ProjToCell(lShape.Extents.xMin, lShape.Extents.yMin, lStartCol, lEndRow)
            lInputGrid.ProjToCell(lShape.Extents.xMax, lShape.Extents.yMax, lEndCol, lStartRow)

            Dim lCellcount As Integer = 0

            Dim lSum As Double = 0
            lPolygonSf.BeginPointInShapefile()
            For lCol As Integer = lStartCol To lEndCol
                For lRow As Integer = lStartRow To lEndRow
                    Dim lXPos As Double
                    Dim lYPos As Double
                    lInputGrid.CellToProj(lCol, lRow, lXPos, lYPos)
                    Dim lSubId As Integer = lPolygonSf.PointInShapefile(lXPos, lYPos)
                    If lSubId = aPolygonFeatureIndex Then 'this is in the polygon we want
                        If Not lInputGrid.Value(lCol, lRow) < 0 Then
                            lSum = lSum + lInputGrid.Value(lCol, lRow)
                            lCellcount = lCellcount + 1
                        End If
                    End If
                Next lRow
            Next lCol
            lPolygonSf.EndPointInShapefile()

            If lCellcount > 0 Then
                aMean = lSum / lCellcount
            Else
                aMean = -999
            End If
        End If

        lInputGrid.Close()
        lInputGrid = Nothing
    End Sub

    Public Shared Function GridSlopeInPolygon(ByVal aGridLayerIndex As Integer, ByVal aPolygonLayerIndex As Integer, _
                                              ByVal aPolygonFeatureIndex As Integer) As Double
        'Given an elevation grid and a polygon layer, find the average slope value within the feature.

        Dim lSlope As Double

        Dim lInputGridName As String = LayerFileName(aGridLayerIndex)
        Dim lDEMGrid As String = PathNameOnly(lInputGridName) & "\tempdem.tif"
        Dim lSlopeGrid As String = PathNameOnly(lInputGridName) & "\slope.tif"

        'set input polygon layer
        Dim lPolygonSf As MapWinGIS.Shapefile = PolygonShapeFileFromIndex(aPolygonLayerIndex)
        Dim lShape As New MapWinGIS.Shape
        If FeatureIndexValid(aPolygonFeatureIndex, lPolygonSf) Then
            lShape = lPolygonSf.Shape(aPolygonFeatureIndex)

            'clip grid to extents
            Dim lSuccess As Boolean = MapWinGeoProc.SpatialOperations.ClipGridWithPolygon(lInputGridName, lShape, lDEMGrid, True)

            'calc slope grid 
            lSuccess = MapWinGeoProc.TerrainAnalysis.Slope(lDEMGrid, lSlopeGrid, False, Nothing)

            'get average slope within polygon
            GridMeanInPolygon(lSlopeGrid, aPolygonLayerIndex, aPolygonFeatureIndex, lSlope)
            lSlope = lSlope / 100   'adjust so slope is not in percent

        End If
        'clean up
        lPolygonSf = Nothing
        Return lSlope
    End Function

    Public Shared Function GridValueAtPoint(ByVal aGridLayerIndex As Integer, ByVal aX As Double, ByVal aY As Double) As Integer
        'set input grid
        Dim gridLayer As MapWindow.Interfaces.Layer
        gridLayer = LayerFromIndex(aGridLayerIndex)
        Dim lInputGrid As New MapWinGIS.Grid
        lInputGrid = gridLayer.GetGridObject

        Dim lCol As Integer
        Dim lRow As Integer
        lInputGrid.ProjToCell(aX, aY, lCol, lRow)
        If lInputGrid.Value(lCol, lRow) < -9999999999 Then
            Return -999
        Else
            Return lInputGrid.Value(lCol, lRow)
        End If
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
        Dim lLayer1 As MapWindow.Interfaces.Layer = LayerFromIndex(aLayer1Index)
        Dim lSf1 As New MapWinGIS.Shapefile
        lSf1 = lLayer1.GetObject

        'set layer 2 (subbasins)
        Dim lLayer2 As MapWindow.Interfaces.Layer = LayerFromIndex(aLayer2Index)
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
            '[lOf].Type = MapWinGIS.FieldType.INTEGER_FIELD
            'fixed to be able to use string 
            [lOf].Type = FieldType(aLayer1FieldIndex, aLayer1Index)
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

        Dim lTotalPolygonCount As Integer = lSf1.NumShapes * lLayer2Selected.Count
        Dim lPolygonCount As Integer = 0
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
                        lShapeNew = MapWinGeoProc.SpatialOperations.Intersection(lShape1, lSf2Shape(k))
                        If lShapeNew.numPoints > 0 Then 'Insert the shape into the shapefile 
                            lBsuc = lSfOut.EditInsertShape(lShapeNew, lSfOut.NumShapes)
                            If Not lBsuc Then
                                Logger.Dbg("Problem Adding Shape") 'TODO:add more details, message box?
                            End If
                            Dim lArea As Double = Math.Abs(MapWinGeoProc.Utils.Area(lShapeNew))
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
            If pStatusShow Then Logger.Progress(lPolygonCount, lTotalPolygonCount)
        Next i
        If pStatusShow Then Logger.Progress(lTotalPolygonCount, lTotalPolygonCount)

        If aCreateNew Then 'delete old version of this file if it exists
            If Not TryDeleteShapefile(aOutputLayerName) Then
                'TryDeleteShapefile fails when the shapefile cannot be deleted, ie it is on the map
                Logger.Msg("The Shapefile " & aOutputLayerName & " could not be deleted." & vbCrLf & _
                           "This can cause misleading results from the overlay operation.", "Overlay Warning")
            End If
        End If

        lBsuc = lSfOut.SaveAs(aOutputLayerName)
        lSfOut.StopEditingShapes()
        lSfOut.Close()
    End Sub

    ''' <summary>Given two polygon layers, calculate the area of each polygon of the 
    '''          first layer with each polygon of the second layer. 
    '''          Output array contains area of each polygon combination.</summary>
    ''' <param name="aPolygonLayer1Index">
    '''     <para>Index of first polygon layer to calculate areas for</para>
    ''' </param>
    ''' <param name="aLayer1FieldIndex">
    '''     <para>Index of id field in first layer (eg land use code)</para>
    ''' </param>
    ''' <param name="aPolygonLayer2Index">
    '''     <para>Index of second polygon layer to calculate areas for</para>
    ''' </param>
    ''' <param name="aSelectedLayer2Indexes">
    '''     <para>Collection of polygon indexes of second polygon layer to calculate areas for</para>
    ''' </param>
    ''' <param name="aArea12">
    '''     <para>2-D Array used to store output areas</para>
    ''' </param>
    Public Shared Sub TabulatePolygonAreas(ByVal aPolygonLayer1Index As Integer, _
                                           ByVal aLayer1FieldIndex As Integer, _
                                           ByVal aPolygonLayer2Index As Integer, _
                                           ByVal aSelectedLayer2Indexes As Collection, _
                                           ByRef aArea12(,) As Double)

        'obtain handle to layer 1 (landuse) 
        Dim lLayer1 As MapWindow.Interfaces.Layer = LayerFromIndex(aPolygonLayer1Index)
        Dim lSf1 As New MapWinGIS.Shapefile
        lSf1 = lLayer1.GetObject

        'set layer 2 (subbasins)
        Dim lLayer2 As MapWindow.Interfaces.Layer = LayerFromIndex(aPolygonLayer2Index)
        Dim lSf2 As New MapWinGIS.Shapefile
        lSf2 = lLayer2.GetObject
        Dim lSf2Ext As MapWinGIS.Extents = lSf2.Extents

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
        For k As Integer = 1 To aSelectedLayer2Indexes.Count
            'loop thru each selected subbasin (or all if none selected)
            lShape2 = lSf2.Shape(aSelectedLayer2Indexes(k))
            lShape2Ext = lShape2.Extents
            lSf2Shape.Add(lShape2)
            lSf2ShapeExtXmax.Add(lShape2Ext.xMax)
            lSf2ShapeExtXmin.Add(lShape2Ext.xMin)
            lSf2ShapeExtYmax.Add(lShape2Ext.yMax)
            lSf2ShapeExtYmin.Add(lShape2Ext.yMin)
        Next k

        '********** do overlay ***********

        Dim lTotalPolygonCount As Integer = lSf1.NumShapes * aSelectedLayer2Indexes.Count
        Dim lPolygonCount As Integer = 0
        Dim lNumShapes As Integer = lSf1.NumShapes
        For i As Integer = 1 To lNumShapes 'loop through each shape of the land use layer
            lShape1 = lSf1.Shape(i - 1)
            Dim lSf1Ext As MapWinGIS.Extents = lShape1.Extents
            If Not (lSf1Ext.xMin > lSf2Ext.xMax OrElse _
                    lSf1Ext.xMax < lSf2Ext.xMin OrElse _
                    lSf1Ext.yMin > lSf2Ext.yMax OrElse _
                    lSf1Ext.yMax < lSf2Ext.yMin) Then
                'current first polygon falls in the extents of the second shapefile
                For k As Integer = 1 To aSelectedLayer2Indexes.Count
                    'loop thru each selected shape in second shapefile (or all if none selected)
                    Dim lShapeIndex As Integer = aSelectedLayer2Indexes(k)
                    lPolygonCount = lPolygonCount + 1
                    If Not (lSf1Ext.xMin > lSf2ShapeExtXmax(k) OrElse _
                            lSf1Ext.xMax < lSf2ShapeExtXmin(k) OrElse _
                            lSf1Ext.yMin > lSf2ShapeExtYmax(k) OrElse _
                            lSf1Ext.yMax < lSf2ShapeExtYmin(k)) Then
                        'look for intersection from overlay of these shapes
                        lShapeNew = MapWinGeoProc.SpatialOperations.Intersection(lShape1, lSf2Shape(k))
                        If lShapeNew.numPoints > 0 Then 'Insert the shape into the shapefile 
                            Dim lArea As Double = Math.Abs(MapWinGeoProc.Utils.Area(lShapeNew))
                            'keep track of field values from both shapefiles
                            Dim lFeature1Id As String = FieldValue(aPolygonLayer1Index, i - 1, aLayer1FieldIndex)
                            If lFeature1Id < aArea12.GetUpperBound(0) And k - 1 < aArea12.GetUpperBound(1) Then
                                aArea12(lFeature1Id, k - 1) = aArea12(lFeature1Id, k - 1) + lArea
                            End If
                        End If
                        lShapeNew = Nothing
                    End If
                Next k
            Else 'quick processing of all polygons in layer2 because outside of extent
                lPolygonCount += aSelectedLayer2Indexes.Count
            End If

            lSf1Ext = Nothing
            lShape1 = Nothing

            If pStatusShow Then Logger.Progress(lPolygonCount, lTotalPolygonCount)
        Next i
        If pStatusShow Then Logger.Progress(lTotalPolygonCount, lTotalPolygonCount)

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
        lNewShapeFile.CreateNew(lNewShapeFileName, lSf.ShapefileType)
        Dim lInputProjectionFileName As String = FilenameSetExt(lSf.Filename, "prj")
        If FileExists(lInputProjectionFileName) Then
            FileCopy(lInputProjectionFileName, FilenameSetExt(lNewShapeFileName, "prj"))
        End If
        For i = 1 To lSf.NumFields
            lRetc = lNewShapeFile.EditInsertField(lSf.Field(i - 1), i - 1)
        Next i
        lNewShapeFile.StartEditingShapes(True)

        Dim issf As New MapWinGIS.Shapefile
        If Not issf.CreateNew("temp_clip.shp", lSf.ShapefileType) Then
            Logger.Dbg("Failed to create temporary shapefile " & issf.Filename & " ErrorCode " & issf.LastErrorCode)
        End If

        lCount = 0
        lTotal = lSfClip.NumShapes * lSf.NumShapes
        lSfClip.BeginPointInShapefile()
        For i = 1 To lSfClip.NumShapes
            lShapeClip = lSfClip.Shape(i - 1)
            For j = 1 To lSf.NumShapes
                lCount += 1
                If pStatusShow Then Logger.Progress(lCount, lTotal)
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
                        Dim lTempShapeIndex As Integer = j - 1
                        lRetc = issf.EditInsertShape(lSf.Shape(j - 1), lTempShapeIndex)
                        lRetc = MapWinGeoProc.SpatialOperations.ClipShapesWithPolygon(issf, lShapeClip, rsf, True)
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
        lSfClip.EndPointInShapefile()
        If pStatusShow Then Logger.Progress(lTotal, lTotal)
        lNewShapeFile.StopEditingShapes(True, True)

        lRetc = lNewShapeFile.Close()
        ClipShapesWithPolygon = lNewShapeFileName

    End Function

    Private Shared Function IsLineInPolygon(ByVal aLineShape As MapWinGIS.Shape, _
                                            ByVal aPolyShapeFile As MapWinGIS.Shapefile, _
                                            ByVal aIndex As Integer) As Boolean
        For i As Integer = 0 To aLineShape.numPoints - 1
            If aPolyShapeFile.PointInShapefile(aLineShape.Point(i).x, _
                                               aLineShape.Point(i).y) = aIndex Then
                Return True 'a point was within polygon
            End If
        Next i
        Return False
    End Function

    Private Shared Function IsLineEntirelyInPolygon(ByVal aLineShape As MapWinGIS.Shape, _
                                                    ByVal aPolyShapeFile As MapWinGIS.Shapefile, _
                                                    ByVal aIndex As Integer) As Boolean
        For i As Integer = 0 To aLineShape.numPoints - 1
            If Not aPolyShapeFile.PointInShapefile(aLineShape.Point(i).x, _
                                                   aLineShape.Point(i).y) = aIndex Then
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
                                                    ByVal aFieldIndex As Integer, _
                                                    Optional ByVal CombineParts As Boolean = True)
        Dim lRetc As Boolean
        Dim lFound As Boolean

        Dim lsf As MapWinGIS.Shapefile = ShapeFileFromIndex(aLayerIndex)

        lsf.StartEditingShapes(True)
        'merge together based on common endpoints
        Dim i As Integer = 0
        Do While i < lsf.NumShapes
            If pStatusShow Then Logger.Progress(i, lsf.NumShapes)
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

        If CombineParts Then
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
                If pStatusShow Then Logger.Progress(i, lsf.NumShapes)
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
        End If

        lsf.StopEditingShapes(True)
        If pStatusShow Then Logger.Progress(lsf.NumShapes, lsf.NumShapes)
    End Sub

    Public Shared Function MergeSelectedShapes(ByVal aLayerIndex As Integer) As Boolean

        'build collection of selected shape indexes
        Dim lSelectedShapeIndexes As New atcCollection
        For lIndex As Integer = 1 To NumSelectedFeatures(aLayerIndex)
            lSelectedShapeIndexes.Add(IndexOfNthSelectedFeatureInLayer(lIndex - 1, aLayerIndex))
        Next
        lSelectedShapeIndexes.Sort()

        Dim lsf As MapWinGIS.Shapefile = ShapeFileFromIndex(aLayerIndex)
        lsf.StartEditingShapes(True)

        If lSelectedShapeIndexes.Count > 1 Then
            'merge first 2 shapes
            Dim lShape1 As MapWinGIS.Shape = lsf.Shape(lSelectedShapeIndexes(0))
            Dim lShape2 As MapWinGIS.Shape = lsf.Shape(lSelectedShapeIndexes(1))
            Dim lResultShape As New MapWinGIS.Shape
            lResultShape.Create(lsf.ShapefileType)
            Dim lSuccess As Boolean = MapWinGeoProc.SpatialOperations.MergeShapes(lShape1, lShape2, lResultShape)
            'add this shape to the end
            lSuccess = lsf.EditInsertShape(lResultShape, lsf.NumShapes)
            'set the attributes of the new shape to those of the first shape
            For lFieldIndex As Integer = 1 To lsf.NumFields
                lSuccess = lsf.EditCellValue(lFieldIndex - 1, lsf.NumShapes - 1, lsf.CellValue(lFieldIndex - 1, lSelectedShapeIndexes(1)))
            Next
        End If

        If lSelectedShapeIndexes.Count > 2 Then
            'merge each additional shape to the one at the end
            For lIndex As Integer = 2 To lSelectedShapeIndexes.Count - 1
                Dim lShape1 As MapWinGIS.Shape = lsf.Shape(lsf.NumShapes - 1)
                Dim lShape2 As MapWinGIS.Shape = lsf.Shape(lSelectedShapeIndexes(lIndex))
                'merge these 2 shapes
                Dim lResultShape As New MapWinGIS.Shape
                lResultShape.Create(lsf.ShapefileType)
                Dim lSuccess As Boolean = MapWinGeoProc.SpatialOperations.MergeShapes(lShape1, lShape2, lResultShape)
                'delete the one at the end
                lSuccess = lsf.EditDeleteShape(lsf.NumShapes - 1)
                'add this shape to the end
                lSuccess = lsf.EditInsertShape(lResultShape, lsf.NumShapes)
                'set the attributes of the new shape to those of the first shape
                For lFieldIndex As Integer = 1 To lsf.NumFields
                    lSuccess = lsf.EditCellValue(lFieldIndex - 1, lsf.NumShapes - 1, lsf.CellValue(lFieldIndex - 1, lSelectedShapeIndexes(lIndex)))
                Next
            Next
        End If

        ClearSelectedFeatures(aLayerIndex)

        If lSelectedShapeIndexes.Count > 1 Then
            'delete the original shapes
            For lIndex As Integer = lSelectedShapeIndexes.Count - 1 To 0 Step -1
                Dim lSuccess As Boolean = lsf.EditDeleteShape(lSelectedShapeIndexes(lIndex))
            Next
        End If

        lsf.StopEditingShapes(True)
        ClearSelectedFeatures(aLayerIndex)
    End Function

    Public Shared Function AreaOverlappingPolygons(ByVal aLayer1index As Integer, _
                                                   ByVal aLayer1FeatureIndex As Integer, _
                                                   ByVal aLayer2index As Integer, _
                                                   ByVal aLayer2FeatureIndex As Integer) As Single
        'overlay feature from layer1 with feature from layer2, 
        'determining area of feature 1 in feature 2
        Dim lAreaOverlappingPolygons As Double = 0.0

        'set layer 1 
        Dim lLayer As MapWindow.Interfaces.Layer
        lLayer = LayerFromIndex(aLayer1index)
        Dim lSf1 As MapWinGIS.Shapefile
        lSf1 = lLayer.GetObject
        Dim lShape1 As MapWinGIS.Shape
        lShape1 = lSf1.Shape(aLayer1FeatureIndex)

        'set layer 2 
        Dim lLayer2 As MapWindow.Interfaces.Layer
        lLayer2 = LayerFromIndex(aLayer2index)
        Dim lsf2 As MapWinGIS.Shapefile
        lsf2 = lLayer2.GetObject
        Dim lShape2 As MapWinGIS.Shape
        lShape2 = lsf2.Shape(aLayer2FeatureIndex)

        Dim lNewShape As MapWinGIS.Shape

        lNewShape = MapWinGeoProc.SpatialOperations.Intersection(lShape1, lShape2)
        If lNewShape.numPoints > 0 Then
            lAreaOverlappingPolygons = Math.Abs(MapWinGeoProc.Utils.Area(lNewShape))
        End If
        lNewShape = Nothing
        Return lAreaOverlappingPolygons
    End Function

    Public Shared Sub SaveSelectedFeatures(ByVal aAreaLayerIndex As Integer, _
                                    ByVal aSelectedAreaIndexes As Collection, _
                                    ByRef aNewFilename As String)

        Dim sf As New MapWinGIS.Shapefile
        Dim tollSF As New MapWinGIS.Shapefile
        Dim fld As New MapWinGIS.Field
        Dim seg As New MapWinGIS.Shape
        Dim i As Integer
        Dim lStatus As Boolean

        'set input layer
        Dim lLayer As MapWindow.Interfaces.Layer = LayerFromIndex(aAreaLayerIndex)
        sf = lLayer.GetObject

        If aNewFilename = "" Then
            'create new shape file for output
            Dim basename As String = FilenameNoExt(sf.Filename)
            i = 1
            aNewFilename = basename & i & ".shp"
            Do While FileExists(aNewFilename)
                i = i + 1
                aNewFilename = basename & i & ".shp"
            Loop
        End If

        If lLayer.LayerType = MapWindow.Interfaces.eLayerType.PolygonShapefile Then
            lStatus = tollSF.CreateNew(aNewFilename, MapWinGIS.ShpfileType.SHP_POLYGON)
        ElseIf lLayer.LayerType = MapWindow.Interfaces.eLayerType.LineShapefile Then
            lStatus = tollSF.CreateNew(aNewFilename, MapWinGIS.ShpfileType.SHP_POLYLINE)
        ElseIf lLayer.LayerType = MapWindow.Interfaces.eLayerType.PointShapefile Then
            lStatus = tollSF.CreateNew(aNewFilename, MapWinGIS.ShpfileType.SHP_POINT)
        End If

        Dim lInputProjectionFileName As String = FilenameNoExt(sf.Filename) & ".prj"
        If FileExists(lInputProjectionFileName) Then
            FileCopy(lInputProjectionFileName, FilenameNoExt(aNewFilename) & ".prj")
        End If

        If lStatus Then
            'start editing the output shapefile 
            lStatus = tollSF.StartEditingShapes(True)

            'add all fields
            Dim j As Integer
            For j = 0 To sf.NumFields - 1
                tollSF.EditInsertField(sf.Field(j), j)
            Next

            For i = 1 To aSelectedAreaIndexes.Count

                seg = sf.Shape(aSelectedAreaIndexes(i))

                'Insert it into the new shapefile
                lStatus = tollSF.EditInsertShape(seg, i - 1)

                Dim h As Integer
                For h = 0 To sf.NumFields - 1
                    tollSF.EditCellValue(h, i - 1, sf.CellValue(h, aSelectedAreaIndexes(i)))
                Next

            Next i
            'Stop editing and close the output shapefile
            tollSF.StopEditingShapes(True, True)
        End If

        tollSF.Close()
        tollSF = Nothing

    End Sub

    Public Shared Sub SetLayerRendererUniqueValues(ByVal aDesc As String, ByVal aFieldIndex As Integer)
        'create a unique values renderer for the given layer and field,
        'adapted from MapWindow SFColoringSchemeForm.vb

        Dim lColorScheme As New MapWinGIS.ShapefileColorScheme

        Dim lLayerIndex As Integer = GisUtil.LayerIndex(aDesc)
        Dim lLayer As MapWindow.Interfaces.Layer = LayerFromIndex(lLayerIndex)

        Dim lSf As New MapWinGIS.Shapefile
        lSf = lLayer.GetObject()

        lColorScheme.FieldIndex = aFieldIndex

        'Get unique values
        Dim i As Integer
        Dim lHt As New Hashtable
        Dim lVal As Object
        For i = 0 To lSf.NumShapes - 1
            If Not IsDBNull(lSf.CellValue(aFieldIndex, i)) Then
                lVal = lSf.CellValue(aFieldIndex, i)
                If lHt.ContainsKey(lVal) = False Then
                    lHt.Add(lVal, lVal)
                End If
            End If
        Next

        'Create sorted array:
        Dim lArr(lHt.Count - 1) As Object
        lHt.Values().CopyTo(lArr, 0)
        Array.Sort(lArr)

        'Create color for each unique value
        Dim lUsedColors As New Hashtable
        Dim lWebSafeColor As String
        Dim lR, lG, lB As Integer
        Dim lRandomColor As UInt32
        For i = 0 To lArr.Length - 1
            Dim lBrk As New MapWinGIS.ShapefileColorBreak

            'because the coloring is randomly chosen, it's possible (and happens often) that
            'the same color is chosen twice, so save the colors per scheme to avoid this.
            lR = CInt(Rnd() * 255)
            lG = CInt(Rnd() * 255)
            lB = CInt(Rnd() * 255)
            lWebSafeColor = Hex(lR).Substring(0, 1) & Hex(lG).Substring(0, 1) & Hex(lB).Substring(0, 1)
            lRandomColor = System.Convert.ToUInt32(RGB(lR, lG, lB))
            Do While lUsedColors.ContainsKey(lWebSafeColor)
                lR = CInt(Rnd() * 255)
                lG = CInt(Rnd() * 255)
                lB = CInt(Rnd() * 255)
                lWebSafeColor = Hex(lR).Substring(0, 1) & Hex(lG).Substring(0, 1) & Hex(lB).Substring(0, 1)
                lRandomColor = System.Convert.ToUInt32(RGB(lR, lG, lB))
            Loop
            lUsedColors.Add(lWebSafeColor, lWebSafeColor)

            lBrk.StartColor = lRandomColor
            lBrk.EndColor = lBrk.StartColor
            lBrk.StartValue = lArr(i)
            lBrk.EndValue = lArr(i)
            If IsNumeric(lArr(i)) Then
                If Int(lArr(i)) > 1 Then
                    lBrk.Caption = Int(lArr(i)).ToString
                Else
                    lBrk.Caption = SignificantDigits(lArr(i), 3).ToString
                End If
            Else
                lBrk.Caption = CStr(lArr(i))
            End If
            lColorScheme.Add(lBrk)
            lBrk = Nothing
        Next

        lUsedColors.Clear()
        lUsedColors = Nothing
        lHt = Nothing
        lSf = Nothing

        lLayer.ColoringScheme = lColorScheme
        lLayer.Expanded = True
        lLayer.Visible = True
    End Sub

    Public Shared Sub CreateShapefileOfCurrentMapExtents(ByVal aShapeName As String)
        Dim lShape As New MapWinGIS.Shape
        Dim lExtentsSf As New MapWinGIS.Shapefile
        If Not lExtentsSf.CreateNew(aShapeName, MapWinGIS.ShpfileType.SHP_POLYGON) Then
            Logger.Dbg("Failed to create new shapefile of extents.")
        End If
        If Not lExtentsSf.StartEditingShapes(True) Then
            Logger.Dbg("Failed to start editing new shapefile of extents.")
        End If
        lShape.Create(MapWinGIS.ShpfileType.SHP_POLYGON)
        Dim lPoint1 As New MapWinGIS.Point
        lPoint1.x = GisUtil.MapExtentXmin
        lPoint1.y = GisUtil.MapExtentYmin
        Dim lPoint2 As New MapWinGIS.Point
        lPoint2.x = GisUtil.MapExtentXmax
        lPoint2.y = GisUtil.MapExtentYmin
        Dim lPoint3 As New MapWinGIS.Point
        lPoint3.x = GisUtil.MapExtentXmax
        lPoint3.y = GisUtil.MapExtentYmax
        Dim lPoint4 As New MapWinGIS.Point
        lPoint4.x = GisUtil.MapExtentXmin
        lPoint4.y = GisUtil.MapExtentYmax
        Dim lPoint5 As New MapWinGIS.Point
        lPoint5.x = GisUtil.MapExtentXmin
        lPoint5.y = GisUtil.MapExtentYmin
        If Not (lShape.InsertPoint(lPoint1, 0) And lShape.InsertPoint(lPoint2, 0) And lShape.InsertPoint(lPoint3, 0) And lShape.InsertPoint(lPoint4, 0) And lShape.InsertPoint(lPoint5, 0)) Then
            Logger.Dbg("Failed to create shape of extents.")
        End If
        If Not lExtentsSf.EditInsertShape(lShape, 0) Then
            Logger.Dbg("Failed to insert shape of extents.")
        End If
        If Not lExtentsSf.Close() Then
            Logger.Dbg("Failed to close shapefile of extents.")
        End If
    End Sub

    ''' <summary>
    ''' given a shapefile name, point coordinates, and attributes, build a new point shapefile
    ''' </summary>
    ''' <param name="aShapefileName"></param>
    ''' <param name="aXPositions"></param>
    ''' <param name="aYPositions"></param>
    ''' <param name="aAttributeNames"></param>
    ''' <param name="aAttributeValues"></param>
    ''' <param name="aOutputProjection"></param>
    ''' <remarks></remarks>
    Public Shared Sub CreatePointShapefile(ByVal aShapefileName As String, _
                                           ByVal aXPositions As Collection, ByVal aYPositions As Collection, _
                                           ByVal aAttributeNames As Collection, ByVal aAttributeValues As Collection, _
                                           ByVal aOutputProjection As String)
        Dim lShapefile As New MapWinGIS.Shapefile
        Dim lInputProjection As String = "+proj=longlat +datum=NAD83"

        If Not lShapefile.CreateNew(aShapefileName, MapWinGIS.ShpfileType.SHP_POINT) Then
            Logger.Dbg("Failed to create new point shapefile " & aShapefileName & " ErrorCode " & lShapefile.LastErrorCode)
        End If
        If Not lShapefile.StartEditingShapes(True) Then
            Logger.Dbg("Failed to start editing new point shapefile " & aShapefileName & " ErrorCode " & lShapefile.LastErrorCode)
        End If

        Dim lAttributeIdIndex As Integer = -1
        Dim lAttributeIndex As Integer = 0
        For Each lAttributeName As String In aAttributeNames
            lAttributeIndex += 1
            Dim lField As New MapWinGIS.Field
            lField.Name = lAttributeName
            If lAttributeName.ToLower = "id" Then
                lAttributeIdIndex = lAttributeIndex
                Logger.Dbg("Id field " & lAttributeIndex)
            End If
            lField.Type = MapWinGIS.FieldType.STRING_FIELD
            lField.Width = 10
            If Not lShapefile.EditInsertField(lField, lShapefile.NumFields) Then
                Logger.Dbg("Failed to add new field " & lField.Name & " to shapefile " & aShapefileName & " ErrorCode " & lShapefile.LastErrorCode)
            End If
            lField = Nothing
        Next

        Dim lAttributeCount As Integer = aAttributeNames.Count
        For lIndex As Integer = 0 To aXPositions.Count - 1
            Dim lShape As New MapWinGIS.Shape
            Dim lPoint As New MapWinGIS.Point
            If Not lShape.Create(MapWinGIS.ShpfileType.SHP_POINT) Then
                Logger.Dbg("Failed to create new point shape, ErrorCode " & lShape.LastErrorCode)
            End If

            If IsNumeric(aXPositions(lIndex + 1)) And IsNumeric(aYPositions(lIndex + 1)) Then
                Dim lXpos As Double = aXPositions(lIndex + 1)
                Dim lYpos As Double = aYPositions(lIndex + 1)
                If aOutputProjection.Length > 0 Then
                    MapWinGeoProc.SpatialReference.ProjectPoint(lXpos, lYpos, lInputProjection, aOutputProjection)
                End If
                lPoint.x = lXpos
                lPoint.y = lYpos
                Dim lTempIndex As Integer = lIndex '2nd argument in InsertPoint set to 0 for some reason
                If Not lShape.InsertPoint(lPoint, lTempIndex) Then
                    Logger.Dbg("Failed to insert point into shape.")
                End If
                If Not lShapefile.EditInsertShape(lShape, lShapefile.NumShapes) Then
                    Logger.Dbg("Failed to insert point shape into shapefile.")
                End If

                For lAttributeIndex = 1 To lAttributeCount
                    If Not lShapefile.EditCellValue(lAttributeIndex - 1, lShapefile.NumShapes - 1, aAttributeValues((lIndex * lAttributeCount) + lAttributeIndex)) Then
                        Logger.Dbg("Failed to edit cell value.")
                    End If
                Next
            Else
                Logger.Dbg("No Latitude (" & aYPositions(lIndex + 1) & ") or Longitude (" & aXPositions(lIndex + 1) & _
                           ") set for point with id " & aAttributeValues((lIndex * lAttributeCount) + lAttributeIdIndex))
            End If

            lPoint = Nothing
            lShape = Nothing
        Next

        If Not lShapefile.StopEditingShapes(True, True) Then
            Logger.Dbg("Failed to stop editing shapes in " & lShapefile.Filename & " ErrorCode " & lShapefile.LastErrorCode)
        End If
        If aOutputProjection.Length > 0 Then
            lShapefile.Projection = aOutputProjection
        Else
            lShapefile.Projection = lInputProjection
        End If
        If Not lShapefile.Close() Then
            Logger.Dbg("Failed to close shapefile " & lShapefile.Filename & " ErrorCode " & lShapefile.LastErrorCode)
        End If

    End Sub

    Public Shared Sub ShapeCentroid(ByVal aLayerIndex As Integer, ByVal aFeatureIndex As Integer, ByRef aCentroidX As Double, ByRef aCentroidY As Double)
        Dim lSf As New MapWinGIS.Shapefile
        lSf = ShapeFileFromIndex(aLayerIndex)
        Dim lPt As New MapWinGIS.Point
        lPt = MapWinGeoProc.Utils.Centroid(lSf.Shape(aFeatureIndex))
        aCentroidX = lPt.x
        aCentroidY = lPt.y
    End Sub

    Public Shared Sub SaveMapAsImage(ByVal aImageFileName As String)
        Dim lImage As New MapWinGIS.Image

        lImage = CType(GetMappingObject.View.Snapshot(GetMappingObject.View.Extents), MapWinGIS.Image)

        If aImageFileName <> "" Then
            If Not lImage.Save(aImageFileName, False, MapWinGIS.ImageType.USE_FILE_EXTENSION) Then
                MapWinUtility.Logger.Msg("There were errors saving the image.", MsgBoxStyle.Exclamation, "Could Not Save")
                Exit Sub
            End If
        End If

        Try
            lImage.Close()
            lImage = Nothing
        Catch ex As Exception
        End Try
    End Sub

    Public Shared Sub UniqueValuesRenderer(ByVal aLayerIndex As Integer, ByVal aFieldIndex As Integer)
        'build a unique values renderer from a given layer and field

        Dim lMWlayer As MapWindow.Interfaces.Layer
        lMWlayer = pMapWin.Layers(pMapWin.Layers.GetHandle(aLayerIndex))

        Dim lColorScheme As New MapWinGIS.ShapefileColorScheme
        lColorScheme.FieldIndex = aFieldIndex
        lMWlayer.DrawFill = True

        Dim lSf As MapWinGIS.Shapefile
        lSf = ShapeFileFromIndex(aLayerIndex)

        'Get unique values
        Dim lValuesHt As New Hashtable
        Dim lIndex As Integer
        Dim lValue As Object
        For lIndex = 0 To lSf.NumShapes - 1
            lValue = lSf.CellValue(aFieldIndex, lIndex)
            If lValuesHt.ContainsKey(lValue) = False Then
                lValuesHt.Add(lValue, lValue)
            End If
        Next

        'Create sorted array
        Dim lValuesArray() As Object
        ReDim lValuesArray(lValuesHt.Count - 1)
        lValuesHt.Values().CopyTo(lValuesArray, 0)
        Array.Sort(lValuesArray)

        'Create color for each unique value
        For lIndex = 0 To lValuesArray.Length - 1
            Dim lBreak As New MapWinGIS.ShapefileColorBreak
            lBreak.StartColor = System.Convert.ToUInt32(RGB(CInt(Rnd() * 255), CInt(Rnd() * 255), CInt(Rnd() * 255)))
            lBreak.EndColor = lBreak.StartColor
            lBreak.StartValue = lValuesArray(lIndex)
            lBreak.EndValue = lValuesArray(lIndex)
            lBreak.Caption = lValuesArray(lIndex)
            lColorScheme.Add(lBreak)
        Next
        lMWlayer.ColoringScheme = lColorScheme

    End Sub

    Public Shared WriteOnly Property ColoringScheme(ByVal aLayerIndex As Integer) As Object
        Set(ByVal aNewValue As Object)
            Dim lMWlayer As MapWindow.Interfaces.Layer = pMapWin.Layers(pMapWin.Layers.GetHandle(aLayerIndex))
            lMWlayer.ColoringScheme = aNewValue
        End Set
    End Property

    Public Shared ReadOnly Property GetColoringScheme(ByVal aLayerIndex As Integer) As Object
        Get
            Dim lMWlayer As MapWindow.Interfaces.Layer = pMapWin.Layers(pMapWin.Layers.GetHandle(aLayerIndex))
            Return lMWlayer.ColoringScheme
        End Get
    End Property

    ''' <summary>Shapefile projection string from a layer index</summary>
    ''' <param name="aLayerIndex">
    '''     <para>Index of layer (Defaults to current layer)</para>
    ''' </param>
    ''' <exception cref="System.Exception" caption="LayerIndexOutOfRange">Layer specified by aLayerIndex does not exist</exception>
    ''' <exception cref="MappingObjectNotSetException">Mapping Object Not Set</exception>
    Public Shared ReadOnly Property ShapefileProjectionString(Optional ByVal aLayerIndex As Integer = UseCurrent) As String
        Get
            Dim lSf As New MapWinGIS.Shapefile
            lSf = ShapeFileFromIndex(aLayerIndex)
            Return lSf.Projection
        End Get
    End Property

    Public Shared ReadOnly Property LayerColor(Optional ByVal aLayerIndex As Integer = UseCurrent) As String
        Get
            Dim redcolor As Integer, greencolor As Integer, bluecolor As Integer
            redcolor = LayerFromIndex(aLayerIndex).Color.R
            greencolor = LayerFromIndex(aLayerIndex).Color.G
            bluecolor = LayerFromIndex(aLayerIndex).Color.B
            Return (bluecolor * 256 * 256) + (greencolor * 256) + redcolor
        End Get
    End Property

    Public Shared ReadOnly Property LayerOutlineColor(Optional ByVal aLayerIndex As Integer = UseCurrent) As String
        Get
            Dim redcolor As Integer, greencolor As Integer, bluecolor As Integer
            redcolor = LayerFromIndex(aLayerIndex).OutlineColor.R
            greencolor = LayerFromIndex(aLayerIndex).OutlineColor.G
            bluecolor = LayerFromIndex(aLayerIndex).OutlineColor.B
            Return (bluecolor * 256 * 256) + (greencolor * 256) + redcolor
        End Get
    End Property

    Public Shared ReadOnly Property LayerTransparent(Optional ByVal aLayerIndex As Integer = UseCurrent) As Boolean
        Get
            Return LayerFromIndex(aLayerIndex).DrawFill
        End Get
    End Property

    Public Shared ReadOnly Property MapExtentXmax() As Double
        Get
            Return GetMappingObject.View.Extents.xMax
        End Get
    End Property

    Public Shared ReadOnly Property MapExtentXmin() As Double
        Get
            Return GetMappingObject.View.Extents.xMin
        End Get
    End Property

    Public Shared ReadOnly Property MapExtentYmax() As Double
        Get
            Return GetMappingObject.View.Extents.yMax
        End Get
    End Property

    Public Shared ReadOnly Property MapExtentYmin() As Double
        Get
            Return GetMappingObject.View.Extents.yMin
        End Get
    End Property

End Class

''' <remarks>Copyright 2006 AQUA TERRA Consultants - Royalty-free use permitted under open source license</remarks>
''' <summary>The Map Window Object has not been set with code like "GisUtil.MappingObject = ?"</summary>
Public Class MappingObjectNotSetException
    Inherits ApplicationException

    Public Sub New()
        MyBase.New("Mapping Object Not Set")
    End Sub
End Class

